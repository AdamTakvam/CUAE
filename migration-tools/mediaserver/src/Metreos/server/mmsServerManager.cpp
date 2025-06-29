//
// mmsServerManager.cpp
//
// Media server manager routes messaging between adapters and session manager
// handles some server housekeeping tasks, and schedules others.
//
#include "StdAfx.h"
#pragma warning(disable:4786)
#include "mmsServerManager.h"
#include "mmsServerCmdheader.h"
#include "mmsFlatMap.h"
#include "mmsReporter.h"
#include "mmsMediaEvent.h"
#include "mmsCommandTypes.h"
#include "mmsAudioFileDescriptor.h" 
#include "mmsTts.h"
#include "mmsAsr.h"
#ifdef MMS_WINPLATFORM
#include <minmax.h>
#endif

#ifdef MMS_ENABLE_MEMORY_LEAK_DETECTION
#define new MMS_MEMLEAK_NEW
#endif // MMS_ENABLE_MEMORY_LEAK_DETECTION

MmsServerManager* MmsServerManager::m_this = NULL;


                                            
int MmsServerManager::handleMessage(MmsMsg* msg) 
{                                                  
  switch(msg->type())                        
  {      
    case MMSM_TIMER:
         this->onTimer(msg);
         break;

    case MMSM_SERVERCMD:                    // Server command from client adapter
         this->onServerCommand(msg);
         break;

    case MMSM_SERVERCMD_RETURN:             // Command return from session mgr  
         this->onServerCommandReturn(msg);
         break;

    case MMSM_SERVERCONTROL:                // Server control from client 
         this->onServerControl(msg);
         break;

    case MMSM_PINGBACK:
         this->onPingback(msg);
         break;

    case MMSM_SERVERPUSH:
         this->onServerPush(msg);
         break;

    case MMSM_TEARDOWN:                     // Teardown client sessions
         this->onTeardown(msg);
         break;

    case MMSM_REGISTER:                     // Register a client adapter
         this->onRegister(msg);
         break;

    case MMSM_UNREGISTER:                   // Unregister a client adapter
         this->onUnregister(msg);            
         break;                         

    case MMSM_REGISTER_TIMERS:              // Register a timer manager 
         this->onTimerPoolRegister(msg);  
         break;

    case MMSM_ACK:                           
         this->onAck(msg);
         break;

    case MMSM_INITTASK: 
         break;

    case MMSM_SHUTDOWN:
         this->shutdownMediaServer();       // Stop message pump and exit 
         break;

    default: return 0;                    
  } 
                                         
  return 1;                               
}


                                             
void MmsServerManager::onServerCommand(MmsMsg* msg)
{
  // Handles a server command message received from client adapter. 
  // We simply place this work on the session manager's queue.
                                            
  if  (m_shutdownInProgress)                  
       this->returnClientMessage(msg, MMS_ERROR_SERVER_INACTIVE);

  else m_sessionManager->postMessage(MMSM_SESSIONTASK, (long)msg->param());
}


                                            
int MmsServerManager::onServerInitialized()  
{          
  // One-time actions taken when server has completely initialized

  m_isServerInitialized = TRUE;

  if (config->hmp.logMaxResources)  
      HmpResourceManager::showLicensedLimits(TRUE); 

  if (config->diagnostics.flags != 0)
      MMSLOG((LM_NOTICE,"SERV diagnostics enabled 0x%x\n",
              config->diagnostics.flags));

  MMSLOG((LM_NOTICE,"SERV utility session pool size %d\n",
          config->calculated.utilityPoolSize));

  this->initTtsServices();                  // Initialize text to speech

  this->initAsrServices();                  // Initialize voice recognition
                           
  MmsEventRegistry::registerForErrorEvents();

  MmsEventRegistry::instance()->open();     // Now OK to begin handling events

  return 0;
}  



void MmsServerManager::onServerControl(MmsMsg* msg)
{
  switch(msg->param())
  {
    case MMS_SERVERCTRL_SHUTDOWN:           // Adapter directs server shutdown
         MMSLOG((LM_NOTICE,"SERV shutdown request received\n"));
         m_shutdownInProgress = TRUE;

         // Here we want to tear down abandoned sessions inline, rather than
         // in a background thread, since we want to ensure this gets done
         // prior to to starting server shutdown sequence.
                 
         m_sessionManager->onAbandonSessions(0);

         this->postMessage(MMSM_SHUTDOWN);  // Trigger server shutdown sequence
         break;

    case MMS_SERVERCTRL_REFRESHCONFIG:      // Adapter asks reread config file
       
         this->onRefreshConfiguration();
         break;

    case MMS_SERVERCTRL_CYCLELOG:           // Adapter asks cycle log file
       
         this->cycleLogfile();
         break;

    case MMS_SERVERCTRL_FLUSHLOG:           // Adapter asks flush log file  
     
         this->flushLogfile();
         break;

    case MMS_SERVERCTRL_POLICEDIRECTORIES: 

         #ifdef MMS_POLICE_DIRECTORIES_INLINE 
         this->scanDirectories();
         #else                              // We do it this way
         this->onThreadedPoliceDirectories(); 
         #endif

         break;
  }
} 



void MmsServerManager::onTimer(MmsMsg* msg) // A timer fired
{
  switch(msg->param())
  {                                         
    case MMS_TIMERID_SMGR_HEARTBEAT:        // This may be a callback so do only
         if  (m_shutdownState) break;       // simple actions e.g. postMessage
         this->onHeartbeat();
         break; 
  }     
}



void MmsServerManager::onPingback(MmsMsg* msg)  
{                                           // A threaded task responded to ping
  MmsTask* task = (MmsTask*) msg->param(); if (!task) return;
  MonitoredTask* monitoredTask = monitoredTasks;

  for(int i=0; i < MONITORED_TASK_COUNT; i++, monitoredTask++)
      if (monitoredTask->task == task)
      {   monitoredTask->count = 0;         // Indicated responded
          break;
      }
}



void MmsServerManager::onHeartbeat()
{
  // The heartbeat notifications below assume one pulse per second. If we
  // change that interval, this code will have to calculate the difference.
  ++m_heartbeats;
  Mms::tick();

  #if 0                                     
  if  (config->diagnostics.shutdownAtHearbeat)
       if (m_heartbeats == config->diagnostics.shutdownAtHearbeat)           
       {   this->postMessage(MMSM_SHUTDOWN);
           return; 
       } 
  #endif

  this->monitorQueueCapacity(); 
  this->monitorThreadState();             
                                            // Send pulse to adapters
  this->notifyAll(MMSM_HEARTBEAT, m_heartbeats);
                                            // Send pulse to session manager
  if  (m_heartbeats % config->serverParams.sessionMonitorIntervalSeconds == 0)
  {    
       m_sessionManager->postMessage(MMSM_NOTIFY, MMS_MONITOR_INTERVAL);

       // m_sessionManager->msg_queue()->notify();   
  }

  this->monitorReporterConnection();

  this->monitorExpiredFiles();
}



void MmsServerManager::monitorThreadState()  
{                                           
  // Ping threaded tasks periodically

  const int serverTimeoutSeconds = config->serverParams.serverTimeoutSeconds;
  if (serverTimeoutSeconds <= 0) return;    // Thread monitoring disabled?

  const int pingIntervalSeconds  = config->serverParams.threadMonitorIntervalSeconds;
  if (m_heartbeats % pingIntervalSeconds != 0) return;

  MonitoredTask* monitoredTask = monitoredTasks;

  for(int i=0; i < MONITORED_TASK_COUNT; i++, monitoredTask++)
  {
      if (monitoredTask->task == NULL) continue;

      const int secsSinceLastResponse = monitoredTask->count * pingIntervalSeconds;

      if  (monitoredTask->count)
           MMSLOG((LM_WARNING,"SERV %s has not responded in %d seconds\n", 
                   monitoredTask->task->name(), secsSinceLastResponse));

      if  (secsSinceLastResponse < serverTimeoutSeconds)
      {   
           monitoredTask->count++;

           monitoredTask->task->postMessage(MMSM_PING, (long)this);
      } 
      else                                  // Nonresponsive: force panic stop
      {    this->forceServer(monitoredTask->task->name());
           break;         
      }
  }
}



void MmsServerManager::scanDirectories()
{
  if (m_isScanningDirectories) return;
  m_isScanningDirectories = TRUE;

  this->policeAudioDirectory(); 

  this->policeLogDirectory();

  m_isScanningDirectories = FALSE;
}



void MmsServerManager::monitorQueueCapacity()
{
  if  (Mms::alertedTask)                    // Log if any queue reached capacity
  {
       char msgbuf[128];
       const char* mask = "queue %s is full and blocking";
       ACE_OS::sprintf(msgbuf, mask, Mms::alertedTask); 
       MMSLOG((LM_WARNING,"SERV %s\n", msgbuf));
       MmsReporter::raiseServerAlarm(MmsReporter::NDX_UNEXPECTED_CONDITION, msgbuf);
       Mms::alertedTask = 0;
  }
}



void MmsServerManager::monitorExpiredFiles()
{
  // Delete expired files periodically
  const int cleanFilesIntervalSeconds       
          = config->serverParams.cleanDirsIntervalMinutes * 60;
                                            
  if  (m_heartbeats % cleanFilesIntervalSeconds == 0)        
       this->postMessage(MMSM_SERVERCONTROL, MMS_SERVERCTRL_POLICEDIRECTORIES);
}



void MmsServerManager::monitorReporterConnection()
{
  // Ping reporter to check stat server connection periodically

  const int monitorReporterIntervalSeconds       
          = config->reports.monitorConnectionIntervalSeconds;  
           
  if ((monitorReporterIntervalSeconds > 0 )                                 
   && (m_heartbeats % monitorReporterIntervalSeconds == 0))       
       m_reporter->postMessage(MMSM_COMMAND, MMS_COMMAND_MONITOR_CONNECTION);
}


                                            
void MmsServerManager::onTimerPoolRegister(MmsMsg* msg)
{ 
  // MMSLOG((LM_DEBUG,"SERV register timers %x\n",msg->param()));  
  m_timers = (MmsTimerHandler*)msg->param();// MMSM_REGISTER_TIMERS handler
  setHeartbeatTimer();   
             
  this->onServerInitialized();              // Server now completely intitalized
}
     

                                     
void MmsServerManager::onServerPush(MmsMsg* msg)
{ 
  this->notifyAll(MMSM_SERVERPUSH, (long)msg->param());
}



void MmsServerManager::onAck(MmsMsg* msg)   // MMSM_ACK 
{                
  #ifdef MMS_ACKNOWLEDGE_TASK_EXIT          // This is not defined                               
  if (m_shutdownState)                      // A component acknowledged shutdown
      this->doMediaServerShutdownSequence();// Continue shutdown sequence
  #endif
}
   

                                         
// - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
// protocol adapter registration/messaging/broadcast
// - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

                                             
void MmsServerManager::onServerCommandReturn(MmsMsg* msg)
{
  // Server command return from the session manager. Command return codes, 
  // and return values if any, are embedded in the parameter map. 

  char* map = (char*)msg->param(); 

  if  (!Mms::isFlatmapReferenced(map,2))
  {    this->forceServer(NULL, MMS_BAD_PTR_MSG); 
       return;
  }
       
  MmsTask* adaptertask = (MmsTask*)getFlatmapSender(map); 

  ACE_Guard<ACE_Thread_Mutex> x(this->adapterslock);

  if  (this->isRegistered(adaptertask))
                                    
       adaptertask->postMessage(MMSM_SERVERCMD_RETURN, (long)map);

  else this->discardClientMessage(msg);
}


                                             
void MmsServerManager::returnClientMessage(MmsMsg* msg, int reason)
{
  // Turn command package from adapter around with an error code

  char* map = (char*)msg->param();           
  setFlatmapRetcode(map, reason);

  MmsTask* adaptertask = (MmsTask*)getFlatmapSender(map);

  ACE_Guard<ACE_Thread_Mutex> x(this->adapterslock);

  if  (this->isRegistered(adaptertask))
       adaptertask->postMessage(MMSM_SERVERCMD_RETURN, (long)map);
}                                           


                                             
void MmsServerManager::onRegister(MmsMsg* msg)
{
  // Handle a MMSM_REGISTER request from an adapter  

  MmsTask* adaptertask = (MmsTask*)msg->param();
  ACE_ASSERT(adaptertask);
  MMSLOG((LM_INFO,"SERV register adapter %x\n", adaptertask));
                                            
  if  (this->registerAdapter(adaptertask) && !m_shutdownState) 
                                            // Confirm registration                 
       adaptertask->postMessage(MMSM_ACK, MMSM_REGISTER);

  // We monitor only the primary adapter, which is expected to be the first
  // registered. Once the primary adapter has registered, all monitored tasks
  // have instantiated, and we can begin periodic pinging of threads.
  if  (adapters.size() == 1)                 
       this->setMonitoredTasks();           // Begin monitoring thread status
}


                                             
int MmsServerManager::registerAdapter(MmsTask* task)
{
  int  result = 1;

  if  (isRegistered(task))
       result = 0;
  else adapters.push_back(task);

  return result;
}



void MmsServerManager::onUnregister(MmsMsg* msg)
{
  // Handle a MMSM_UNREGISTER request from an adapter

  MmsTask* adaptertask = (MmsTask*)msg->param();
  ACE_ASSERT(adaptertask);
                                     
  this->unregisterAdapter(adaptertask);
}      


                                            
int MmsServerManager::unregisterAdapter(const MmsTask* adaptertask)
{                                            
  ACE_Guard<ACE_Thread_Mutex> x(this->adapterslock);

  int  adapterOrdinal = this->isRegistered(adaptertask);

  if  (adapterOrdinal) 
  {    MMSLOG((LM_INFO,"SERV unregister adapter %x\n", adaptertask));
       adapters.erase(adapters.begin() + (adapterOrdinal-1));
  }
 
  return adapterOrdinal != 0;               // Return boolean wasUnregistered
}



void MmsServerManager::onTeardown(MmsMsg* msg)
{      
  TEARDOWNPARAMS* params  =(TEARDOWNPARAMS*)msg->param();
  MmsTask*   adaptertask  =(MmsTask*)params->adapter;
  void*      clienthandle = params->handle;
  ACE_ASSERT(adaptertask);
  if  (params->isHeapAlloc)
       delete params;

  // Here we notify session manager to tear down any sessions abandoned by
  // this client, assigning the batch teardown to a service thread, in order
  // that a large teardown won't cause a session manager availability hiccup.
            
  #if 0                                     // Inline teardown    
  m_sessionManager->onAbandonSessions(adaptertask, clienthandle);
  #else
											                      // Service thread teardown
  m_sessionManager->onAbandonSessionsBackground(adaptertask, clienthandle);
  #endif
}


                                            
int MmsServerManager::shutdownAdapters()
{                  
  const int adapterscount = adapters.size();
 
  // Note that adapters unregister themselves after receiving MMSM_SHUTDOWN
  // and therefore we do not unregister each adapter here. The thinking is
  // that we may in the future want adapters to respond to shutdown with
  // an acknowledgement message.

  for(int i=0; i < adapterscount; i++)      
  {   
      MmsTask* task = adapters[i];
      if  (task)  
      {                                     // Stagger shutdowns 
           task->postMessage(MMSM_SHUTDOWN);
           ACE_Thread_Manager::instance()->wait_task(task);
      }
  }              

  return 0;
}


                                            // Is specified adapter registered
int MmsServerManager::isRegistered(const MmsTask* task)
{                                           // Returns ordinal (1-based) of 
  const int n = adapters.size();            // adapter or zero if not registered
  for(int i=0; i < n; i++)
      if  (adapters[i] == task) 
           return (i+1);
  return 0;
}


                                            // Broadcast to all registrants
int MmsServerManager::notifyAll(const unsigned int msg, const long param)
{
  const int n = adapters.size();

  for(int i=0; i < n; i++)
      adapters[i]->postMessage(msg, param);

  return 0;
}



void MmsServerManager::discardClientMessage(MmsMsg* msg)
{
  // Handle the rare case in which adapter unregisters and subsequently 
  // is target for an outstanding command return

  char* map = (char*)msg->param();
  MmsIpcAdapter* adapter = map? getFlatmapSender(map): 0;
  MMSLOG((LM_INFO,"SERV discard msg for unregistered adapter %x\n",adapter));
  if   (map) delete[] map;
}



// - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
// server manager utility methods
// - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -


void MmsServerManager::onRefreshConfiguration()
{
  MmsConfig* configcopy = new MmsConfig();  // Make a copy
  memcpy(configcopy, this->config, sizeof(MmsConfig));

  if  (-1 == config->readLocalConfigFile()) // Reread config 
       MMSLOG((LM_ERROR, "SERV could not read config\n"));
  else                                      // Act on changes
  {    this->monitorConfigurationChanges(configcopy);
       MMSLOG((LM_NOTICE,"SERV config refreshed\n"));
  }

  delete configcopy;                         
}



void MmsServerManager::monitorConfigurationChanges(MmsConfig* oldconfig)
{
  // On refresh of the server configuration (e.g. rereading the config file)
  // act on changes to selected configurable properties 
                                            // Log message display level
  if  (oldconfig->serverLogger.globalMessageLevel
       != config->serverLogger.globalMessageLevel)
       this->registerMsgLevelChange();

  if  (oldconfig->serverLogger.flush
       != config->serverLogger.flush)
       MMSLOG((LM_NOTICE,"SERV flush after write is now %d\n",
          config->serverLogger.flush)); 
                                            // Active talkers
  if  (oldconfig->media.conferenceActiveTalkersEnabled
       != config->media.conferenceActiveTalkersEnabled)
  {    const int onOff = config->media.conferenceActiveTalkersEnabled;
       if  (this->conferencingDevice()     // Device does the log messages
         &&(-1 == m_deviceConf->enableActiveTalkerMonitoring(onOff)))
              config->media.conferenceActiveTalkersEnabled =!onOff;
  }
}



void MmsServerManager::registerMsgLevelChange()                                          
{
  // On refresh of the server configuration (e.g. rereading the config file)
  // if the log message level has been changed, we  notify all our threaded 
  // subtasks of the change.

  const int newMsgLevel = config->serverLogger.globalMessageLevel;
  const int oldMsgLevel = this->currentMsgLevel;
  if  (newMsgLevel == oldMsgLevel) return;
  this->currentMsgLevel = newMsgLevel;
                                            // Make a new ACE msglevel bitmask
  unsigned int priorityMask = 0;            // of all bits >= the new priority
  for (unsigned int bit = newMsgLevel; bit <= LM_MAX; bit <<= 1)   
       priorityMask |= bit;                 // Set the new msglevel app-wide
  ACE_Log_Msg::instance()->priority_mask(priorityMask, ACE_Log_Msg::PROCESS);
   
                                            // Notify all subtasks of change
  MmsTask* logger = (MmsTask*)this->logCallback;
  logger->postMessage          (MMSM_SET_MSGLEVEL, newMsgLevel);
  m_sessionManager->postMessage(MMSM_SET_MSGLEVEL, newMsgLevel);
  m_timers->postMessage        (MMSM_SET_MSGLEVEL, newMsgLevel);
  this->notifyAll              (MMSM_SET_MSGLEVEL, newMsgLevel);
  mmsYield();
 
  MMSLOG((LM_NOTICE,"SERV log priority was %d is now %d\n",
          lmToMsgPriority(oldMsgLevel), lmToMsgPriority(newMsgLevel))); 
} 


#if(0)
int MmsServerManager::getServerResourceCounts(MMS_RESOURCECOUNTS* counts, const int availOnly)
{
  // This method is no longer used - we now use MmsAs:: counts for all stats.
  // It has some useful coding examples so we keep the code here uncompiled.
  if (availOnly)
  {                                         // Get counts from saved counts
      counts->ipInstalled = this->m_resxCounts.ipInstalled;
      counts->dxInstalled = this->m_resxCounts.dxInstalled;
      counts->dtInstalled = this->m_resxCounts.dtInstalled;
      counts->lbInstalled = this->m_resxCounts.lbInstalled;
  }
  else
  {   counts->ipInstalled                   // Get counts from HMP etc
            = m_resourceMgr->deviceInventory().getDeviceCount(MmsMediaDevice::IP);
      counts->dxInstalled 
            = m_resourceMgr->deviceInventory().getDeviceCount(MmsMediaDevice::VOICE);
      counts->dtInstalled 
            = m_resourceMgr->getResourceCountConference();
      counts->lbInstalled = MmsAs::licG729;
  }

  counts->lbAvailable = counts->lbInstalled - Mms::lowBitrateResourcesInUse;

  counts->ipAvailable = m_sessionPool->ipSessionsAvail();

  #if(0)
  // We benchmarked dcb_dsprescount at 30ms. Since this is too much overhead on
  // connect and disconnect, and since clients are not currently using conference
  // and voice resource counts, we'll just let these default to zero until such time
  // as we can code up a less resource-intensive method of maintaining resource counts
  counts->dxAvailable 
        = m_resourceMgr->resourcePoolAvailableCount(MmsMediaDevice::VOICE);

  if  (config->serverParams.reassignIdleVoiceResources)
       counts->dxAvailable  
       += m_resourceMgr->resourcePoolIdleCount(MmsMediaDevice::VOICE);

  if  (this->conferencingDevice())
       counts->dtAvailable = max(m_deviceConf->resourcesRemaining(), 0);
  #endif 
                                            // Save current counts
  memcpy(&m_resxCounts, counts, sizeof(MMS_RESOURCECOUNTS));
  return 0;
} 
#endif



int MmsServerManager::conferencingDevice()
{
  // Cache conference device (singleton) to reduce overhead of frequent
  // requests for resource stats etc.

  if  (m_deviceConf == NULL)                                                       
       m_deviceConf = (MmsDeviceConference*)m_resourceMgr->getConferencingDevice();

  return (m_deviceConf != NULL);
}

      

// - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
// server manager startup and initialization activities
// - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
 
                                            // Ctor
MmsServerManager::MmsServerManager(MmsTask::InitialParams* params): 
  MmsBasicTask(params), m_sessionPool(0) 
{
  m_this = this;
  this->config =(MmsConfig*)params->config;

  m_reporter = params->reporter;

  m_resourceMgr =(HmpResourceManager*)params->user;                                          
                                            // Allocate session pool
  this->m_sessionPool = new MmsSessionPool(config);
                                             
  m_threadPool  = NULL;                     // Set via init()
  m_timers      = NULL;                     // Set via MMSM_REGISTER_TIMERS
  m_deviceConf  = NULL;                     // See getServerResourceCounts
  m_heartbeatTimerID = 0;
  m_isServerInitialized = m_shutdownState = m_shutdownInProgress = m_isScanningDirectories = 0; 

  this->policeAudioDirectory(FALSE); 
  this->policeLogDirectory(); 
}


                                            // Pre-start initialization
int MmsServerManager::init(MmsSessionManager* smgr, MmsTask* threadpool)
{
  // This is executed in main thread context prior to server manager start.

  m_sessionManager = smgr; 
  m_threadPool = threadpool;
  
  const int result = this->preallocateSessionResources();

  return result;
}



int MmsServerManager::preallocateSessionResources()
{
  // Intitialize session objects with those resident references which cannot
  // be assigned during object construction.

  // Paints each IP session object with an opened IP resource. Returns boolean
  // indicating whether resources were successfully loaded. It should be the
  // case that there are an equal number of IP session objects and IP resources.
  // This is executed in main thread context prior to server manager start.

  // Utility session pool follows the IP session pool in the pool table. 
  // Utility sessions are used as context for playing media to conferences. 
  // They do not host an IP resource; using only a voice resource which
  // they obtain and release as needed.

  int ipdevicecount = m_resourceMgr->getDeviceCount(MmsMediaDevice::IP);
  if (ipdevicecount < 1)
  {   MMSLOG((LM_ALERT,"SERV IP devices not ready\n"));  
      return 0;
  }

  const int sessioncount   = m_sessionPool->size();
  const int ipsessioncount = m_sessionPool->ipPoolSize();
  MmsSession* session      = m_sessionPool->base();

                                            // For each IP and session ...
  for(int i = 0; i < ipdevicecount, i < sessioncount; i++, session++)
  {
      session->sessionMgr  = m_sessionManager;
      session->resourceMgr = m_resourceMgr;
      session->sessionTimeoutSecs = config->serverParams.sessionTimeoutSecondsDefault;

      if  (i < ipsessioncount)              // If IP session ...
      {                                     // Get IP resource from pool
           mmsDeviceHandle handle = m_resourceMgr->getResource(MmsMediaDevice::IP);

           if  (isValidDeviceHandle(handle))
           {
                session->ipResource(handle);// Assign IP resource to session[i]
                MmsMediaDevice* device = m_resourceMgr->getDevice(handle); 
                device->owner(session->sessionID());
           }
           else
           {    MMSLOG((LM_ERROR,"SERV error obtaining IP resource %d\n",i));  
                return 0;
           }
      }
      else session->ipResource(-1);    
  }

  m_isIpResourcesLoaded = TRUE; 
  return 1;
}




    
                                        
int MmsServerManager::onThreadStarted()     // Thread startup hook
{ 
  MMSLOG((LM_INFO,"SERV thread %t started at priority %d\n", osPriority)); 
                                            // If we did not initialize OK ...
  if (!m_sessionManager || !m_isIpResourcesLoaded  || !m_threadPool)
       return -1;                           // ... bail (don't start msg pump)
                                             
  return 0;
} 
  


int MmsServerManager::setHeartbeatTimer()
{
  // Code is much simpler if we mandate the heartbeat at one per second.
  // If it turns out we need a more frequent pulse, the code remains below,
  // ifdef'ed out.

  m_heartbeats = 0;  
  
  #if 0

  int  hbmillisecs = config->serverParams.heartbeatIntervalMsecs;
  if  (hbmillisecs < 100) hbmillisecs = 1000;

  config->calculated.heartbeatsPerSecond 
    = (1000 / hbmillisecs) + (1000 % hbmillisecs != 0);

  MmsTime heartbeatInterval(0,1000 * hbmillisecs);

  #else

  MmsTime heartbeatInterval(1);
  config->calculated.heartbeatsPerSecond = 1;

  #endif

  m_heartbeatTimerID = m_timers->setTimer(this, MMS_TIMERID_SMGR_HEARTBEAT, 
    heartbeatInterval, heartbeatInterval, FALSE);

  return m_heartbeatTimerID;
}



void MmsServerManager::setMonitoredTasks()
{
  // Initialize monitored tasks table. In this table resides the threaded tasks
  // we will ping periodically, paired with counts of unanswered pings.
  // We currently do not have a method of polling individual service threads, 
  // since the thread pool's message queue does not address specific threads,
  // so we'll want to come up with a method to do so, perhaps using thread local  
  // storage. We also do not currently monitor the adapter listener threads, 
  // nor any adapters other than the first, or primary, adapater.

  clearMonitoredTasks();

  monitoredTasks[0].task  = m_sessionManager;
  monitoredTasks[1].task  = m_threadPool;
  monitoredTasks[2].task  =(MmsTask*)logCallback;
  monitoredTasks[3].task  = adapters[0];    // Primary adapter
  // itoredTasks[4].task  = m_reporter;
}



void MmsServerManager::initTtsServices()
{
  // Initialize text to speech services if so configured.
  int result = Tts::instance()->init(config);

  switch(result)
  {
    case TTS_OK:
         MMSLOG((LM_NOTICE,"SERV TTS services are active\n")); 
         break;

    case TTS_ERROR_INIT_DISABLED:
         MMSLOG((LM_NOTICE,"SERV TTS services disabled per config\n"));
         break;

    default:
         MMSLOG((LM_ERROR, "SERV TTS services could not be activated\n"));
  }
} 



void MmsServerManager::closeTtsServices()
{
  // Dispose of any TTS services
  Tts::destroy();       
}



void MmsServerManager::initAsrServices()
{
  // Initialize voice recognition services if so configured.
  int result = Asr::instance()->init(config);

  switch(result)
  {
    case ASR_OK:
         MMSLOG((LM_NOTICE,"SERV ASR services are active\n")); 
         break;

    case ASR_ERROR_INIT_DISABLED:
         MMSLOG((LM_NOTICE,"SERV ASR services disabled per config\n"));
         break;

    default:
         MMSLOG((LM_ERROR,"SERV ASR services could not be activated\n"));
  }
} 



void MmsServerManager::closeAsrServices()
{
  // Dispose of any ASR services
  Asr::destroy();            
}



// - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
// directory policing and file expiration
// - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -


void MmsServerManager::policeAudioDirectory(int isGetLock)
{              
  // Recurse the media server audio directory, deleting any files
  // recorded by media server which have reached their expiration date

  const static char* maska = "SERV audio scan terminated at %d directories\n";
  const static char* maskb = "SERV expired %d of %d recorded audio\n";
  const static char* maskc = "SERV audio expire scan time %d\n";
  unsigned elapsed = Mms::getTickCount();

  if  (isGetLock)
       m_sessionManager->audiolock.acquire();

  MmsAudioDirectoryWalker walker(config->serverParams.maxDirectoryScanCount);
  walker.recurse(config->serverParams.audioBasePath);

  if  (walker.isMaxDirectories())
       if  (this->isServerInitialized()) 
            MMSLOG((LM_INFO, maska, walker.m_maxdirs)); 
       else ACE_OS::printf  (maska, walker.m_maxdirs);

  if  (walker.erased())  
       if  (this->isServerInitialized())  
            MMSLOG((LM_INFO, maskb, walker.erased(), walker.count()));                
       else ACE_OS::printf  (maskb, walker.erased(), walker.count());

  if  (isGetLock)
       m_sessionManager->audiolock.release();

  elapsed = Mms::getTickCount() - elapsed;

  if  (this->isServerInitialized())
       MMSLOG((LM_INFO, maskc, elapsed));
  else ACE_OS::printf  (maskc, elapsed);
}



int MmsAudioDirectoryWalker::filecallback(char* path, void* param)
{
  // MmsDirectoryRecursor callback to examine current file and if a descriptor
  // for a recorded audio file, remove both files if expiration date reached

  PathInfo pathinfo; this->parsepath(pathinfo, path);
  if  (ACE_OS::strcmp(pathinfo.ext, MMS_RECORD_PROPERTIES_FILE_EXTENSION) != 0) 
       return 0;                            // Not a *.mms file
  if  (_access(path, 2) == -1) return 0;    // File is currently open

  MmsAudioFileDescriptor rec;               // Read audio file descriptor
  if  (!rec.read(path) || !rec.isValid()) return 0;   
  m_count++;

  if  (rec.daysToExpiration() < 1)          // If file has expired ...
  {    
       ACE_OS::unlink(path);                // Delete the descriptor file
       char* ext = rec.isVox()? ".vox": rec.isWav()? ".wav": NULL;
       if   (ext == NULL) return 0;         // Build path to audio file

       ACE_OS::strcpy(pathinfo.ext, ext);   // Overwrite file extension
       ACE_OS::unlink(path);                // Delete the audio file        
       m_erased++;
  }         
        
  return 0;
}



void MmsServerManager::policeLogDirectory()
{              
  // Scan the media server log directory, deleting expired log files

  const static char* maskb = "SERV expired %d of %d server logs\n";
  const static char* maskc = "SERV log expire scan time %d\n";
  unsigned elapsed = Mms::getTickCount();
                                             
  ACE_Time_Value expDays(config->serverParams.cleanLogsAfterDays * (60*60*24));
  const long expirationSeconds = expDays.sec();

  char dirpath[MAXPATHLEN]; ACE_OS::strcpy(dirpath, config->serverLogger.filepath);
  stripLogFilename(dirpath);
  const unsigned starttick = Mms::getTickCount();

  MmsLogDirectoryScanner scanner;
  scanner.scan(dirpath, (void*)&expirationSeconds);

  if (scanner.erased())  
      if  (this->isServerInitialized()) 
           MMSLOG((LM_INFO, maskb, scanner.erased(), scanner.count()));                    
      else ACE_OS::printf  (maskb, scanner.erased(), scanner.count());

  elapsed = Mms::getTickCount() - elapsed;

  if  (this->isServerInitialized())
       MMSLOG((LM_INFO, maskc, elapsed));
  else ACE_OS::printf  (maskc, elapsed);
}



int MmsLogDirectoryScanner::filecallback(char* path, void* pexpsecs)
{
  // MmsDirectoryRecursor callback to examine current file and if a server
  // log archive file, and it has expired, remove it

  PathInfo pathinfo; this->parsepath(pathinfo, path);
  if  (ACE_OS::strcmp(pathinfo.ext, ".log") != 0) return 0; 
  if  (pathinfo.filenamelength != 14) return 0;
  m_count++;

  ACE_stat fileinfo; ACE_OS::stat(path, &fileinfo);
  ACE_Time_Value fileTime = fileinfo.st_mtime;
                                            // If log file expired ...
  long timeDifference = (long) difftime(this->currentTime.sec(), fileTime.sec());
  if  (timeDifference > *((long*)pexpsecs))
  {    ACE_OS::unlink(path);                // ... delete the log file
       m_erased++;
  }
 
  return 0;
}



void MmsServerManager::onThreadedPoliceDirectories()
{
  // Kick off directory policing activities on a service thread

  MMSLOG((LM_DEBUG,"SERV begin background directory scan ...\n"));
  MmsSessionManager::SuperTaskParams* params = new MmsSessionManager::SuperTaskParams(); 
  params->sessionManagerTask = MmsSessionManager::SuperTaskParams::POLICE_DIRECTORIES;

  m_threadPool->postMessage(MMSM_SERVICEPOOLTASKEX, (long)params);  
}



void MmsServerManager::policeDirectoriesCallback(void*)    
{
   // Static callback to execute directory policing activites.
   // We use this static callback to execute directory cleanup from another thread
   // which may not have access to mmsServerManager.h in order to call the cleanup
   // functions directly.
 
   m_this->scanDirectories();
}



void MmsServerManager::cycleLogfile()
{
  if (this->logCallback)
     ((MmsTask*)this->logCallback)->postMessage(MMSM_CYCLE);
}



void MmsServerManager::flushLogfile()
{
  if (this->logCallback)
     ((MmsTask*)this->logCallback)->postMessage(MMSM_FLUSH);
}


// - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
// server manager shutdown activities
// - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

                                            // Quit all server process threads
void MmsServerManager::shutdownMediaServer()
{
  m_shutdownState = SHUTDOWN_BEGIN;
  this->doMediaServerShutdownSequence();
}


                                            // Execute ordered server shutdown
void MmsServerManager::doMediaServerShutdownSequence()
{
  // Shutdown is accomplished via operating system waits on individual task
  // threads. Ordering of component shutdown is critical; any reordering must
  // therefore be carefully considered.
   
  MMSLOG((LM_NOTICE,"SERV begin shutdown sequence\n"));
                                            // Reset stats to zero
  MmsReporter::instance()->publishStartupShutdownStats();
  mmsSleep(MMS_N_MS(500));      

  this->enableAlarmsAndStats(FALSE);        // Stop alarms/stats

  this->clearMonitoredTasks();              // Stop pinging threads  

  this->cancelHeartbeatTimer();             // Cancel timer notifications

  const int pending = MmsEventRegistry::instance()->entries();
  if (pending) MMSLOG((LM_NOTICE,"SERV %d events pending at shutdown\n",pending));
                                            
  MmsEventRegistry::instance()->close();    // Cancel event notifications  

  this->notifyAll(MMSM_STOP);               // Quiesce adapters
  mmsSleep(1);                              // Let log catch up

  this->closeTtsServices();

  this->closeAsrServices();

  // HMP resources are closed as early as possible in the shutdown sequence
  // so that if for any reason the shutdown does not run to completion and 
  // the process canceled manually, HMP service state is not compromised. 
  m_sessionManager->conferenceManager()->closeAll();
  m_resourceMgr->closeAll();                

  MMSLOG((LM_INFO,"SERV closing adapters\n"));
  this->shutdownAdapters();                 // Tell listeners to shut down
  mmsSleep(max(adapters.size()/2,1));   
                                            
  MMSLOG((LM_INFO,"SERV closing service pool\n")); 
  m_shutdownState = SHUTDOWN_THREADPOOL;                   
  m_threadPool->postMessage(MMSM_SHUTDOWN); // Tell POOL to shut down 
  ACE_Thread_Manager::instance()->wait_task(m_threadPool);  

  MMSLOG((LM_INFO,"SERV closing timers\n"));                                              
  m_shutdownState = SHUTDOWN_TIMERS;              
  m_timers->postMessage(MMSM_QUIT);           
  m_timers->msg_queue()->notify();          // Tell TIM1 to shut down
  ACE_Thread_Manager::instance()->wait_task(m_timers);

  MMSLOG((LM_INFO,"SERV closing alarms/stats\n"));
  m_shutdownState = SHUTDOWN_REPORTER;
  m_reporter->postMessage(MMSM_SHUTDOWN);   // Tell stats to shut down
  // If stats server is backed up, wait_task can block, so we sleep instead
  // ACE_Thread_Manager::instance()->wait_task(m_reporter); 
  mmsSleep(MMS_N_MS(700));

  MMSLOG((LM_INFO,"SERV closing session manager\n"));                                            
  m_shutdownState = SHUTDOWN_SESSIONMGR;    
  m_sessionManager->postMessage(MMSM_QUIT); // Tell SMGR to shut down
  ACE_Thread_Manager::instance()->wait_task(m_sessionManager);

  MMSLOG((LM_INFO,"SERV closing server manager\n"));
  m_shutdownState = SHUTDOWN_SELF;   
  this->deallocateSessionResources();
                                            // All components have shutdown, so 
  this->postMessage(MMSM_QUIT);             // so shut down logger and ourself  
}



int MmsServerManager::deallocateSessionResources()
{
  // Return all allocated IP resources to available pool. 

  int sessioncount    = m_sessionPool->ipPoolSize();
  MmsSession* session = m_sessionPool->base();
                                            // For each session ...
  for(int i = 0; i < sessioncount; i++, session++)
  {
      mmsDeviceHandle handle = session->ipResource();

      m_resourceMgr->releaseResource(handle);  
  }

  return 0;
}



void MmsServerManager::cancelHeartbeatTimer()
{
  if  (m_heartbeatTimerID)
       m_timers->cancelTimer(m_heartbeatTimerID);
}



void MmsServerManager::enableAlarmsAndStats(const int bEnable)
{
  if (m_reporter)  
      m_reporter->postMessage(bEnable? MMSM_START: MMSM_STOP);
}



void MmsServerManager::closeLogger()
{
  if  (this->logCallback)                                           
  {    
       MMSLOG((LM_INFO,"SERV closing logger\n"));
       MmsTask* logger = (MmsTask*)this->logCallback;
       logger->postMessage(MMSM_QUIT);  
       logger->msg_queue()->notify();
  }
}



void MmsServerManager::forceServer(char* task, char* msg)
{       
  const static char* msgA = "appears down - forcing server stop"; 
  const static char* msgB = " - forcing server stop";
  const static char* msgC  = "unrecoverable error";

  char msgbuf[128];
                     
  if (task)                                 // Server panic stop
      ACE_OS::sprintf(msgbuf, "%s %s", task, msgA);
  else 
  {   const char* msgx = msg? msg: msgC;
      ACE_OS::sprintf(msgbuf, "%s %s", msgx, msgB);
  }

  MMSLOG((LM_ERROR,"SMGR %s\n", msgbuf));  
  MmsReporter::raiseServerAlarm(MmsReporter::NDX_UNSCHEDULED_SHUTDOWN, msgbuf);

  this->cycleLogfile();
  mmsSleep(MMS_HALF_SECOND);
  m_shutdownState = SHUTDOWN_PANIC;

  this->postMessage(MMSM_QUIT);             // Exit without cleanup  
}



int MmsServerManager::close(unsigned long)  // Thread exit hook
{
  MMSLOG((LM_DEBUG,"SERV thread %t exit\n")); 

  // If MAIN is able to wait on the service manager task, as opposed to waiting
  // on all threads, then MAIN can close the logger. Otherwise server manager
  // must close logger, and the last possible spot is here at thread exit.

  #ifdef MMS_SMGR_CLOSES_LOGGER             // This is not defined
  this->closeLogger();                                         
  #endif

  return 0;
}


                                            
MmsServerManager::~MmsServerManager()       // Dtor
{ 
  if  (m_sessionPool)
       delete m_sessionPool;
}

