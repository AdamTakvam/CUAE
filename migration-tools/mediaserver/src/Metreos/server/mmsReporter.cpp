// 
// mmsReporter.cpp
//
// Alarms and Statistics interface
//
// - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
// Resource accounting and alarm generation for the various resource types
// occurs at the following code points. Note that high/low watermark alarms
// are generated in this module, as a by-product of statistics calls. 
// Statistics calls are made in MmsAs:: object (mms.cpp) as a by-product
// of resource (license) burn and un-burn calls.
//
// G711  burn resource:    session.handleConnect() (2 spots) 
// G711  unburn resource:  sessionD.releaseResources()
// G711  out of resource:  sessionPool.findAvailableSessionEx()
//
// G729  burn resource:    sessionC.reserveLowBitrateCoderEx()
// G729  unburn resource:  sessionC.reserveLowBitrateCoderEx()
// G729  out of resource:  sessionC.verifyCoderAvailabilityEx(), 
//                         deviceIP.verifyCoderAvailabilityEx()
// VOX   burn resource:    operation.voiceResourceAcquire()
// VOX   unburn resource:  operation.voiceResourceRelease()
// VOX   out of resource:  operation.voiceResourceAcquire()
//
// CONF  burn resource:    deviceConference.create(), join(), listenOnMonitorChannel()
// CONF  unburn resource:  deviceConference.leave(), leaveHost(), removeMonitorChannel();
// CONF  out of resource:  deviceConference.create(), join(), listenOnMonitorChannel()
//
// TTS   burn resource:    tts.render()
// TTS   unburn resource:  tts.render()
// TTS   out of resource:  tts.render()
//
// CSP   burn resource:    sessionH.handleVoiceRec(); sessionH.createAsrChannel()
// CSP   unburn resource:  sessionG.Op.stopVoiceRecOperation() 
// CSP   out of resource:  sessionH.handleVoiceRec()
//
// Note that voice recognition resource monitoring accounts for Dialogic CSP
// (continuous speech processing) resources only. It is of course also dependent
// upon ports available on the ASR (speech rec) engine; however since the ASR
// engine is not a media server component, but rather licensed separately by
// the customer, media server currently has no way of determining the ports
// licensed by the customer on that ASR engine, and so we do not monitor ASR
// port usage. The code is present to do so if were to need it however.
//
// Note that this object maintains an IPC connection to the stats server.
// If the connection is lost, a reconnect is attempted when a stat request is
// received by this object, in which case that stat request is discarded. 
// By doing so we can lose one two stats or alarms, not critical, but by doing   
// so we avoid the possibly fatal effects of accumulating alarm/stat requests  
// in the reporter object message queue, waiting for a socket response. 

// Once a few consecutive connect attempts triggered by incoming transactions fail, 
// we stop attempting connections on incoming transactions and only attempt again 
// when pinged by server manager to do so, every few minutes.
//
#include "StdAfx.h"
#include "mmsReporter.h"
#include "mmsCommandTypes.h"
#include "mmsParameterMap.h"
#include <time.h>

#ifdef MMS_ENABLE_MEMORY_LEAK_DETECTION
#define new MMS_MEMLEAK_NEW
#endif // MMS_ENABLE_MEMORY_LEAK_DETECTION


MmsTask*     MmsReporter::thisx = 0;
MmsConfig*   MmsReporter::configx = 0;
MmsReporter* MmsReporter::singleton = 0;
int MmsReporter::isConnecting = 0;
int MmsReporter::isDisconnecting = 0;
ACE_Thread_Mutex* MmsReporter::connectLock = new ACE_Thread_Mutex();

const char *MMS_UMASK = "%u", *MMS_SSTSMASK = "MMS-%08X";



int MmsReporter::handleMessage(MmsMsg* msg)             
{                    
  switch(msg->type())
  {     
    case MMSM_STATSREQUEST:
         if (this->isEnabled)
             this->onStatsRequest(msg);
         break;

    case MMSM_ALARMREQUEST:
         if (this->isEnabled) 
             this->onAlarmRequest(msg);
         break;  

    case MMSM_COMMAND:                      
         this->onCommand(msg);
         break;

    case MMSM_STOP:                         // Disable alarms and stats
         this->isEnabled = FALSE;
         this->onStopReporter();
         break;

    case MMSM_START:                        // Re-enable alarms and stats
         this->onStartReporter();
         this->isEnabled = TRUE;
         break;

    case MMSM_REPORTER_LOG_SERVERDISCO:
         MMSLOG((LM_DEBUG,"REPO disconnected from stat server\n"));
         break;

    case MMSM_REPORTER_LOG_SOCKETERR:
         MMSLOG((LM_ERROR,"REPO stat server IPC connection lost\n"));
         break;

    case MMSM_INITTASK:
         break;
    
    case MMSM_SHUTDOWN:
         this->setShutdown();
         this->postMessage(MMSM_QUIT);
         break;

    default: return 0;
  } 

  return 1;
}



void MmsReporter::reporterCallback(void* datastruct)
{
   // Static callback from mms.cpp and other non-mmsTask code.
   // We post a message from here to the reporter message queue, rather than call
   // an instance handler method directly. We do so in order to decouple from the 
   // global (mms.h) calling thread and thus allow the mutex serializing it 
   // to be released.
 
   MmsAlarmStatParams* params = (MmsAlarmStatParams*) datastruct;
   if (!params) return;
    
   switch (params->signature)
   {
     case MmsStatsParams::sig:              // Stat 

          if (MmsReporter::configx->reports.disableStats) 
          {   
              // MMSLOG((LM_DEBUG,"REPO stat suppressed: %d\n", params->paramType)); 
              delete (MmsStatsParams*) datastruct;
          }  
          else thisx->postMessage(MMSM_STATSREQUEST, (long)params);
          break;
     
     case MmsAlarmParams::sig:              // Alarm

          thisx->postMessage(MMSM_ALARMREQUEST, (long)params);
          break;

     default:                               // Won't happen
          MMSLOG((LM_ERROR,"REPO memory leak at reporterCallback\n")); 
   }
}



int MmsReporter::onAlarmRequest(MmsMsg* msg)
{
  // Handle alarm request -- raise alarm

  // Caller has allocated an MmsAlarmParams object and passed a pointer  
  // to this struct as the msg.param().     
  MmsAlarmParams* params = (MmsAlarmParams*)msg->param();

  return this->onAlarmRequest(params);
} 



int MmsReporter::onAlarmRequest(MmsAlarmParams* params)
{
  // Handle alarm request - raise alarm
 
  if (params == NULL) return -1;
  int result = -1;

  do 
  { if (!this->isConnected()) 
    {   // If connection is dead we discard this alarm request and try to reconnect
        result = this->connectSync(); 
        break;        
    }

    if (params->signature != MmsAlarmParams::sig) break;
          
    if (this->config->reports.disableAlarms) 
    {
        if  (!params->isClearingAlarm)
             MMSLOG((LM_DEBUG,"REPO alarm type %d suppressed\n", params->paramType)); 
        break;
    }

    if  (params->isClearingAlarm)
         result = this->clearAlarm(params);
    else result = this->raiseAlarm(params);

  } while(0);

  if (!params->isStaticallyAllocated)
      delete params;                        // Free caller's memory

  return result;
} 



int MmsReporter::onStatsRequest(MmsMsg* msg)
{
  // Handle stats request - publish statistics

  // Caller has allocated an MmsStatsParams object and passed a pointer   
  // to this struct as the msg.param().  
  MmsStatsParams* params = (MmsStatsParams*)msg->param();
  if (params == NULL) return -1;
  int result = 0;

  do 
  { if (!this->isConnected()) 
    {   // If connection is dead we discard this stat and try to reconnect
        result = this->connectSync(); 
        break;        
    }
   
    if (params->signature != MmsStatsParams::sig) break;  
    if (config->reports.disableStats) break;   
    
    result = this->handleAlarmStateOnResourceChange(params);

    result = this->publishStats(params);

  } while(0);

  if (!params->isStaticallyAllocated)
      delete params;                        // Free caller's memory

  return result;
} 



int MmsReporter::handleAlarmStateOnResourceChange(MmsStatsParams* params)
{
  // Fire or clear certain alarms on an incoming resource count change event. 

  // Note that resource exhaustion alarms have been fired at the source. 
  // We do not see a resource change on such an event, since the count was
  // already zero and did not change. We thus do not fire resource exhausted
  // alarms here, but we do reset those alarms.

  // When resource count prior to the event is the same as resource count after
  // the event, it signifies some condition where there would have been overflow
  // or underflow of the resource count, and so it was not changed. If any alarm
  // is associated with such an event, it will have already been fired.
  if (params->newValue == params->oldValue) return 0;
  int result = 0;

  switch(params->paramType)
  {  
    case MMS_STAT_CATEGORY_RTP: 
    
         result = this->monitorAlarmStateForResource(params, 
               config->reports.hiwaterG711Resx, config->reports.lowaterG711Resx,
               NDX_HIWATER_CONNECTIONS, NDX_LOWATER_CONNECTIONS, NDX_NO_MORE_CONNECTIONS);         
         break; 
 
    case MMS_STAT_CATEGORY_CONFRESX:

         result = this->monitorAlarmStateForResource(params, 
               config->reports.hiwaterConferenceResx, config->reports.lowaterConferenceResx,
               NDX_HIWATER_CONFRESX_IN_SVC, NDX_LOWATER_CONFRESX_IN_SVC, NDX_NO_MORE_CONFRESX_IN_SVC);         
         break; 
                 
    case MMS_STAT_CATEGORY_VOX: 
        
         result = this->monitorAlarmStateForResource(params, 
               config->reports.hiwaterVoiceResx, config->reports.lowaterVoiceResx,
               NDX_HIWATER_VOX, NDX_LOWATER_VOX, NDX_NO_MORE_VOX);         
         break; 
             
    case MMS_STAT_CATEGORY_CONFSLOTS: 

         // We do not monitor conferee slots per conference     
         break; 

    case MMS_STAT_CATEGORY_G729: 

         result = this->monitorAlarmStateForResource(params, 
               config->reports.hiwaterG729EtcResx, config->reports.lowaterG729EtcResx,
               NDX_HIWATER_G729_ETC, NDX_LOWATER_G729_ETC, NDX_NO_MORE_G729_ETC);   
         break; 

    case MMS_STAT_CATEGORY_CONFERENCES:

         result = this->monitorAlarmStateForResource(params, 
               config->reports.hiwaterConferences, config->reports.lowaterConferences,
               NDX_HIWATER_CONFERENCES, NDX_LOWATER_CONFERENCES, NDX_NO_MORE_CONFERENCES);           
         break;

    case MMS_STAT_CATEGORY_TTS: 

         result = this->monitorAlarmStateForResource(params, 
               config->reports.hiwaterTtsResx, config->reports.lowaterTtsResx,
               NDX_HIWATER_TTS_PORTS, NDX_LOWATER_TTS_PORTS, NDX_NO_MORE_TTS_PORTS_FAILS);  
         break; 
      
    case MMS_STAT_CATEGORY_ASR:

         // Presumably we will not monitor voice rec (Nuance) port licenses. 
         // If we do, the alarm for VR and ASR is the same alarm.
         result = this->monitorAlarmStateForResource(params, 
               config->reports.hiwaterSpeechRecResx, config->reports.lowaterSpeechRecResx,
               NDX_HIWATER_ASR_RESX, NDX_LOWATER_ASR_RESX, NDX_NO_MORE_ASR_RESX);  
         break; 
       
    case MMS_STAT_CATEGORY_CSP:

         result = this->monitorAlarmStateForResource(params, 
               config->reports.hiwaterSpeechIntResx, config->reports.lowaterSpeechIntResx,
               NDX_HIWATER_ASR_RESX, NDX_LOWATER_ASR_RESX, NDX_NO_MORE_ASR_RESX);  
         break;        
  }
   
  return result;
}



int MmsReporter::monitorAlarmStateForResource
( MmsStatsParams* params, const int hiwatermark, const int lowatermark, 
  const int hiwaterAlarmIndex, const int lowaterAlarmIndex, const int noMoreAlarmIndex
)
{ // For caller's specific resource type, RTP resources for example, check the new resource 
  // count against the specified high or low watermark value for that resource type, 
  // depending on whether this event is burning or un-burning the resource. If the watermark 
  // is configured zero, this specifies that no water mark monitoring is configured, and thus
  // no water mark alarm activity is generated here. Otherwise, if the new resource count has  
  // hit the water mark, raise the approriate alarm. 

  // In addition, if a high or low watermark alarm is currently active for this resource type,
  // and this event has caused the resource count to drop below the high water, or above the
  // low water, clear that alarm.

  // Finally, if a resources exhausted alarm is set for this resource type, and this event
  // unburns a resource, clear that alarm as well. Note that these (out of resource) alarms
  // are generated at the source, as they are detected, not here.
  if (params->oldValue == params->newValue) return 0; 

  // Although individual alarms are locked in the alarm table while they are being modified,
  // we must serialize access to the entire alarm table here, since we may be both setting  
  // and clearing multiple alarms whose states are dependent upon each other.
  ACE_Guard<ACE_Thread_Mutex> x(this->alarmStatesTableLock);

  // Old value and new value are IN USE resource counts. So old value less  
  // than new value means we now have fewer resources of that type free, 
  // hence we burned one or more licenses with the event. 
  const bool isBurningResource = params->oldValue < params->newValue;    
  const int  resourceValue = params->newValue;
                                                        
  if (isBurningResource)                    // Event burned a resource license
  {                                            
      if (this->isAlarmSet(hiwaterAlarmIndex) && resourceValue < hiwatermark)
          this->generateAlarm               // Just dropped back below hi water 
               (MMSM_CLEAR_ALARM, hiwaterAlarmIndex, isBurningResource, params);

      if (lowatermark > 0 && resourceValue == lowatermark)
          this->generateAlarm               // Just hit low water  
               (MMSM_SET_ALARM, lowaterAlarmIndex, isBurningResource, params);
  }
  else                                      // Event unburned a resource license
  {   
      if (this->isAlarmSet(noMoreAlarmIndex))
          this->generateAlarm               // Just got back above zero 
               (MMSM_CLEAR_ALARM, noMoreAlarmIndex, isBurningResource, params); 

      if (this->isAlarmSet(lowaterAlarmIndex) && resourceValue > lowatermark)
          this->generateAlarm               // Just got back above lo water 
               (MMSM_CLEAR_ALARM, lowaterAlarmIndex, isBurningResource, params); 

      if (hiwatermark > 0 && params->newValue == hiwatermark)
          this->generateAlarm               // Just hit hi water  
               (MMSM_SET_ALARM, hiwaterAlarmIndex, isBurningResource, params);
  }       

  return 0;
}



int MmsReporter::generateAlarm
( const int msgID, const int alarmIndex, const bool isBurningResource, MmsStatsParams* params)
{
  // Generate alarm content appropriate to the current statistic, 
  // and fire that alarm by sending it through the pipeline

  const char *hi = "high", *lo = "low";
  const char *mask = "media resource type '%s' hit %s usage water mark %d";
  const int resourceType = params->paramType, resourceValue = params->newValue;
  MmsAlarmParams* alarmParams = NULL;
  int result = 0;

  switch(msgID)
  {
    case MMSM_SET_ALARM:
         alarmParams = new MmsAlarmParams(alarmIndex, resourceType, severityTypeCaution);

         ACE_OS::sprintf(alarmParams->text, mask, this->mediaTypeText(resourceType), 
                 isBurningResource? lo:hi, resourceValue);
         break;

    case MMSM_CLEAR_ALARM:
         alarmParams = new MmsAlarmParams(alarmIndex, resourceType, severityTypeNormal);
         alarmParams->isClearingAlarm = TRUE;
         break;
  }

  result = this->onAlarmRequest(alarmParams) == 0;
  return result;
}



int MmsReporter::raiseAlarm(MmsAlarmParams* params) 
{
  // Set alarm internally and transmit alarm via IPC

  if (params == NULL) return -1;
  int result = 0;
  const int alarmIndex = params->paramType;

  struct alarmState* alarm = this->findAlarmByAlarmTypeIndex(alarmIndex);
  if (alarm == NULL) return -1;

  if (alarm->lock)
      alarm->lock->acquire();     // Serialize access to this alarm table entry
  
  if (!alarm->isAlarmActive())    // If alarm is active, we don't fire it again  
  {   
      alarm->setAlarm();

      Metreos::FlatMapWriter writer;

      this->buildAlarmMap(params, alarm, writer);

      result = this->transmitAlarm(writer);

      if (result == 1)
          MMSLOG((LM_INFO,"REPO alarm type %d raised\n", params->paramType)); 
  }

  if (alarm->lock)
      alarm->lock->release();

  return result;
}



int MmsReporter::clearAlarm(MmsAlarmParams* params)    
{
  // Clear alarm internally if set, and transmit clear message via IPC

  if (params == NULL) return -1;
  int result = 0;
  const int alarmIndex = params->paramType;

  struct alarmState* alarm = this->findAlarmByAlarmTypeIndex(alarmIndex);
  if (alarm == NULL) return -1;

  if (alarm->lock)
      alarm->lock->acquire();     // Serialize access to this alarm table entry
  
  if (alarm->isAlarmActive())     // If alarm is active, we can clear it  
  {   
      alarm->clearAlarm();

      Metreos::FlatMapWriter writer;

      this->buildAlarmMap(params, alarm, writer);

      result = this->transmitAlarm(writer, params->isClearingAlarm);

      if (result == 1)
          MMSLOG((LM_INFO,"REPO alarm type %d cleared\n", params->paramType)); 
  }

  if (alarm->lock)
      alarm->lock->release();

  return result;
} 



int MmsReporter::publishStats(MmsStatsParams* params)  
{
  // Publish statistic indicated by specified params, via IPC
  if (params == NULL) return -1;

  Metreos::FlatMapWriter writer;

  this->buildStatsMap(params, writer);

  const int result = this->transmitStats(writer);

  return result;
}



int MmsReporter::transmitAlarm(Metreos::FlatMapWriter& writer, const int isClearingAlarm) 
{
  // Transmit alarm on stat server IPC connection
  // The alarm is wrapped in the specified flatmap.

  #if(0)
  if  (isClearingAlarm)
       MMSLOG((LM_INFO,"REPO sending alarm clear to stat server\n"));  
  else MMSLOG((LM_INFO,"REPO sending alarm set to stat server\n"));
  #endif  

  const int messageID = isClearingAlarm? MMSM_CLEAR_ALARM: MMSM_SET_ALARM;   

  const int result 
      = this->ipcClient == NULL? -1:
        this->ipcClient->postIpcMessage(messageID, writer);       

  return result;
}



int MmsReporter::transmitStats(Metreos::FlatMapWriter& writer) 
{
  // Transmit statistic on stat server IPC connection
  // The statistic is wrapped in the specified flatmap.
  // MMSLOG((LM_INFO,"REPO sending stat to stat server\n")); 
 
  const int result = this->ipcClient == NULL? -1:
        this->ipcClient->postIpcMessage(MmsReporter::MMSM_SET_STATISTIC, writer);     

  return result;
}



int MmsReporter::isAlarmSet(const int alarmIndex)  
{
  // Determine if specified alarm is active internally
  struct alarmState* alarm = this->findAlarmByAlarmTypeIndex(alarmIndex);
  return (alarm != NULL && alarm->instanceID != 0);  
} 



MmsReporter::alarmState* MmsReporter::findAlarmByAlarmTypeIndex(const int index)
{
  if (index < 0 || index >= NDX_ALARM_TYPES_COUNT)
  {
      MMSLOG((LM_ERROR,"REPO alarm index %d out of range\n", index)); 
      return NULL;
  }

  return &alarmStates[index];
}



void MmsReporter::buildAlarmMap
(MmsAlarmParams* params, MmsReporter::alarmState* alarm, Metreos::FlatMapWriter& map) 
{
  // Builds/returns a flatmap of alarm interface parameters based on supplied parameters
  time_t now; time(&now);

  #ifdef IS_STAT_SERVER_BINARY_PARAMS
  // Stat server does not accept binary parameters. It should.

  map.insert(MMSP_ALARM_TYPE, alarm->alarmType);
  map.insert(MMSP_SEVERITY, params->severity);
  map.insert(MMSP_TIMESTAMP, now);
  map.insert(MMSP_GUID, (long)alarm->instanceID);

  #else   // IS_STAT_SERVER_BINARY_PARAMS

  char buf[128]; int len; 

  len = ACE_OS::sprintf(buf, MMS_UMASK, alarm->alarmType);
  map.insert(MMSP_ALARM_TYPE,Metreos::FlatMap::STRING, ++len, buf);

  len = ACE_OS::sprintf(buf, MMS_UMASK, params->severity);
  map.insert(MMSP_SEVERITY,  Metreos::FlatMap::STRING, ++len, buf);

  len = this->timestampText(buf, now);
  map.insert(MMSP_TIMESTAMP, Metreos::FlatMap::STRING, ++len, buf);

  len = ACE_OS::sprintf(buf, MMS_SSTSMASK, alarm->instanceID);
  map.insert(MMSP_GUID,      Metreos::FlatMap::STRING, ++len, buf);  

  #endif  // IS_STAT_SERVER_BINARY_PARAMS

  const int textlen = strlen(params->text);
  if (textlen)
      map.insert(MMSP_TEXT, MmsFlatMap::STRING, textlen+1, params->text);
}

                

void MmsReporter::buildStatsMap(MmsStatsParams* params, Metreos::FlatMapWriter& map) 
{ 
  // Builds/returns a flatmap of stats interface parameters based on supplied parameters

  time_t now; time(&now);
  const unsigned thistick = this->getUniqueTick();

  const int statType = params->paramType, statValue = params->newValue;
  // MMSLOG((LM_DEBUG,"REPO reporting stat type %d value %d\n", statType, statValue)); 

  #ifdef IS_STAT_SERVER_BINARY_PARAMS
  // Stat server does not accept binary parameters. It should.

  map.insert(MMSP_STATS_TYPE,  statType);
  map.insert(MMSP_STATS_VALUE, statValue);
  map.insert(MMSP_TIMESTAMP, (long)now);
  map.insert(MMSP_GUID, (long)thistick);

  #else   // IS_STAT_SERVER_BINARY_PARAMS

  char buf[128]; int len; 

  len = ACE_OS::sprintf(buf,  MMS_UMASK, statType);
  map.insert(MMSP_STATS_TYPE, Metreos::FlatMap::STRING, ++len, buf);

  len = ACE_OS::sprintf(buf,  MMS_UMASK, statValue);
  map.insert(MMSP_STATS_VALUE,Metreos::FlatMap::STRING, ++len, buf);

  len = this->timestampText(buf, now);
  map.insert(MMSP_TIMESTAMP,  Metreos::FlatMap::STRING, ++len, buf);

  len = ACE_OS::sprintf(buf,  MMS_SSTSMASK, thistick);
  map.insert(MMSP_GUID,       Metreos::FlatMap::STRING, ++len, buf); 

  #endif  // IS_STAT_SERVER_BINARY_PARAMS
}



void MmsReporter::publishStartupShutdownStats()
{
  // Publish zero resources in use for each statistics type

  if (!this->isConnected()) return;

  const int statCategories[] 
    = { MMS_STAT_CATEGORY_RTP,         MMS_STAT_CATEGORY_VOX,  MMS_STAT_CATEGORY_G729,
        MMS_STAT_CATEGORY_CONFRESX,    MMS_STAT_CATEGORY_TTS,  MMS_STAT_CATEGORY_CSP,
        MMS_STAT_CATEGORY_CONFERENCES, MMS_STAT_CATEGORY_ASR 
      }; 

  const int numCategories = sizeof(statCategories) / sizeof(int);

  MmsStatsParams statParams(0,0,0);

  for(int i=0; i < numCategories; i++)
  {
      statParams.paramType = statParams.resourceType = statCategories[i];
      
      this->publishStats(&statParams);
  }
}



// - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - 
// Support methods
// - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -


void MmsReporter::onStartReporter()
{
  // Actions on starting the reporter

  if  (config->reports.disableStats && config->reports.disableStats)
  {
  }
  else this->connectSync();

  this->isinitialized = TRUE;
}



void MmsReporter::onStopReporter()
{
  // Actions on stopping the reporter
  this->isEnabled = FALSE;

  if (this->ipcClient)
      this->ipcClient->disconnectFromStatServer();
}



int MmsReporter::onCommand(MmsMsg* msg)
{
  // Handle external commands
  // Currently monitor stat server connection is the only command.
  // Server manager pings us every few minutes to monitor the connection.

  const int command = msg->param();    
  if (command != MMS_COMMAND_MONITOR_CONNECTION) return -1;
  int result = 0;

  if  (this->isConnected()) 
  {
  }  
  else 
  if (this->isInitialized()) 
  {
      MMSLOG((LM_DEBUG,"REPO attempting stats reconnect per config\n")); 
      result = this->connectSync(TRUE);
  }

  return result;
}



int MmsReporter::isConnected()
{
  // Determine if a stat server connection exists
  return this->ipcClient && this->ipcClient->isconnected;
}



int MmsReporter::connectSync(const int tryAlways)
{
  // Initiate connection to stat server synchronously. This is the way we always 
  // connect, since this code is running in the reporter thread, and since we do 
  // not queue up alarm or stat requests when no connection to the stat server exists, 
  // but instead discard those transactions, there is no need to connect asynchronously.

  // Once a few consecutive connect attempts triggered by incoming transactions fail, 
  // we stop attempting connections on incoming transactions and only attempt again 
  // when pinged by server manager to do so, every few minutes (tryAlways)

  if (tryAlways);
  else
  if (this->ipcClient->consecutiveConnectFailures 
   >= MmsReporterIpcClient::MAX_CONNECT_ATTEMPTS)
      return -1; 
  else
  if (this->ipcClient->consecutiveConnectLocks 
   >= MmsReporterIpcClient::MAX_CONNECT_LOCKOUTS)
      return -1; 
  else
  if (MmsReporter::isConnecting)
      return -1;

  MmsReporter::isConnecting = TRUE;
  int result = FALSE;
         
  if (MmsReporter::connectLock->tryacquire() == -1) 
  {    
      MMSLOG((LM_DEBUG,"REPO connect attempt blocked and deferred\n")); 
      
      if (++this->ipcClient->consecutiveConnectLocks  
       >= MmsReporterIpcClient::MAX_CONNECT_LOCKOUTS)
          // Failsafe code - we should not be able to reach this condition
          MMSLOG((LM_ERROR,"REPO max connect block threshold reached\n")); 
  }
  else    // Acquired the lock so do the connect
  {   this->ipcClient->consecutiveConnectLocks = 0;

      if  (this->isConnected())              
           result = TRUE;

      else result = this->ipcClient->blockingConnectToStatServer 
                (config->reports.statServerPort, config->reports.statServerIP);

      MmsReporter::connectLock->release();
  }

  MmsReporter::isConnecting = FALSE;

  if (this->isConnected() && !this->isStartupStatsPublished)
  {   // Reset resources in use to zero
      this->isStartupStatsPublished = TRUE;
      this->publishStartupShutdownStats();
  }

  return result? 0: -1;
}



int MmsReporter::connectAsync()
{
  // Initiate connection to stat server - async call
  // We never use async connect -- see comments above at connectSync().

  #if(0)
  if (this->isConnecting) return 0; 
 
  if (this->ipcClient->consecutiveConnectFailures 
   >= MmsReporterIpcClient::MAX_CONNECT_ATTEMPTS)
      return -1;                        

  this->isConnecting = TRUE;
  int result = FALSE;
      
  // Ensure only one concurrent connect attempt - lock cleared as connect thread exits
  if (this->ipcClient->connectLock.tryacquire() != -1)  
  {
      result = this->ipcClient->initiateConnectToStatServer
         (config->reports.statServerPort, config->reports.statServerIP);

      if (!result) this->ipcClient->connectLock.release();
  }

  if (!result) this->isConnecting = FALSE;

  return result? 0: -1;

  #else
  return 0;
  #endif
}



int MmsReporter::onThreadStarted()          // Task thread startup hook
{ 
  this->isEnabled = TRUE;
  MMSLOG((LM_INFO,"REPO thread %t started at priority %d\n", osPriority)); 
  return 0;
} 



int MmsReporter::close(unsigned long)       // Task thread exit hook
{                  
  #if(0)                                    // We never connect async
  if (this->listenerThread)                 
  {   try                                    
      { ACE_Thread_Manager *tm = ACE_Thread_Manager::instance();
        // do not wait on an active connect thread 
        if (tm->testresume(this->listenerThread) == 1)
            tm->join(this->listenerThread);
      } 
      catch(...) { } 
  }
  #endif  
               
  MMSLOG((LM_DEBUG,"REPO thread %t exit\n"));
  return 0;
}



int MmsReporter::initializeAlarmState()     // Initialize alarm state table
{
  struct alarmState* state = &alarmStates[0];

  for(int i = 0; i < MmsReporter::NDX_ALARM_TYPES_COUNT; i++, state++) state->clear();
   
  alarmStates[NDX_SERVER_COMPROMISED].alarmType           = MMS_SERVER_COMPROMISED; 
  alarmStates[NDX_SERVER_COMPROMISED].statCategory        = MMS_STAT_CATEGORY_SERVER;    
  alarmStates[NDX_UNEXPECTED_CONDITION].alarmType         = MMS_UNEXPECTED_CONDITION;
  alarmStates[NDX_UNEXPECTED_CONDITION].statCategory      = MMS_STAT_CATEGORY_SERVER;    
  alarmStates[NDX_UNSCHEDULED_SHUTDOWN].alarmType         = MMS_UNSCHEDULED_SHUTDOWN;
  alarmStates[NDX_UNSCHEDULED_SHUTDOWN].statCategory      = MMS_STAT_CATEGORY_SERVER;    
  alarmStates[NDX_RESX_NOT_DEPLOYED].alarmType            = MMS_RESX_NOT_DEPLOYED; 
  alarmStates[NDX_RESX_NOT_DEPLOYED].statCategory         = MMS_STAT_CATEGORY_SERVER; 
  
  alarmStates[NDX_NO_MORE_CONNECTIONS].alarmType          = MMS_NO_MORE_CONNECTIONS; 
  alarmStates[NDX_NO_MORE_CONNECTIONS].statCategory       = MMS_STAT_CATEGORY_RTP; 
  alarmStates[NDX_NO_MORE_CONNECTIONS].lock               = new ACE_Thread_Mutex();    
  alarmStates[NDX_HIWATER_CONNECTIONS].alarmType          = MMS_HIWATER_CONNECTIONS; 
  alarmStates[NDX_HIWATER_CONNECTIONS].statCategory       = MMS_STAT_CATEGORY_RTP; 
  alarmStates[NDX_HIWATER_CONNECTIONS].lock               = new ACE_Thread_Mutex();     
  alarmStates[NDX_LOWATER_CONNECTIONS].alarmType          = MMS_LOWATER_CONNECTIONS; 
  alarmStates[NDX_LOWATER_CONNECTIONS].statCategory       = MMS_STAT_CATEGORY_RTP; 
  alarmStates[NDX_LOWATER_CONNECTIONS].lock               = new ACE_Thread_Mutex();     
     
  alarmStates[NDX_NO_MORE_VOX].alarmType                  = MMS_NO_MORE_VOX;
  alarmStates[NDX_NO_MORE_VOX].statCategory               = MMS_STAT_CATEGORY_VOX; 
  alarmStates[NDX_NO_MORE_VOX].lock                       = new ACE_Thread_Mutex();        
  alarmStates[NDX_HIWATER_VOX].alarmType                  = MMS_HIWATER_VOX; 
  alarmStates[NDX_HIWATER_VOX].statCategory               = MMS_STAT_CATEGORY_VOX; 
  alarmStates[NDX_HIWATER_VOX].lock                       = new ACE_Thread_Mutex();        
  alarmStates[NDX_LOWATER_VOX].alarmType                  = MMS_LOWATER_VOX;    
  alarmStates[NDX_LOWATER_VOX].statCategory               = MMS_STAT_CATEGORY_VOX; 
  alarmStates[NDX_LOWATER_VOX].lock                       = new ACE_Thread_Mutex();     

  alarmStates[NDX_NO_MORE_G729_ETC].alarmType             = MMS_NO_MORE_G729_ETC; 
  alarmStates[NDX_NO_MORE_G729_ETC].statCategory          = MMS_STAT_CATEGORY_G729; 
  alarmStates[NDX_NO_MORE_G729_ETC].lock                  = new ACE_Thread_Mutex();        
  alarmStates[NDX_HIWATER_G729_ETC].alarmType             = MMS_HIWATER_G729_ETC; 
  alarmStates[NDX_HIWATER_G729_ETC].statCategory          = MMS_STAT_CATEGORY_G729; 
  alarmStates[NDX_HIWATER_G729_ETC].lock                  = new ACE_Thread_Mutex();       
  alarmStates[NDX_LOWATER_G729_ETC].alarmType             = MMS_LOWATER_G729_ETC;  
  alarmStates[NDX_LOWATER_G729_ETC].statCategory          = MMS_STAT_CATEGORY_G729;
  alarmStates[NDX_LOWATER_G729_ETC].lock                  = new ACE_Thread_Mutex();     
  
  alarmStates[NDX_NO_MORE_CONFRESX_IN_SVC].alarmType      = MMS_NO_MORE_CONFRESX_IN_SVC;
  alarmStates[NDX_NO_MORE_CONFRESX_IN_SVC].statCategory   = MMS_STAT_CATEGORY_CONFRESX; 
  alarmStates[NDX_NO_MORE_CONFRESX_IN_SVC].lock           = new ACE_Thread_Mutex();        
  alarmStates[NDX_HIWATER_CONFRESX_IN_SVC].alarmType      = MMS_HIWATER_CONFRESX_IN_SVC;
  alarmStates[NDX_HIWATER_CONFRESX_IN_SVC].statCategory   = MMS_STAT_CATEGORY_CONFRESX; 
  alarmStates[NDX_HIWATER_CONFRESX_IN_SVC].lock           = new ACE_Thread_Mutex();            
  alarmStates[NDX_LOWATER_CONFRESX_IN_SVC].alarmType      = MMS_LOWATER_CONFRESX_IN_SVC;
  alarmStates[NDX_LOWATER_CONFRESX_IN_SVC].statCategory   = MMS_STAT_CATEGORY_CONFRESX;
  alarmStates[NDX_LOWATER_CONFRESX_IN_SVC].lock           = new ACE_Thread_Mutex();          
   
  alarmStates[NDX_NO_MORE_SLOTS_IN_CONF].alarmType        = MMS_NO_MORE_SLOTS_IN_CONF; 
  alarmStates[NDX_NO_MORE_SLOTS_IN_CONF].statCategory     = MMS_STAT_CATEGORY_CONFSLOTS;   
  alarmStates[NDX_HIWATER_SLOTS_IN_CONF].alarmType        = MMS_HIWATER_SLOTS_IN_CONF;
  alarmStates[NDX_HIWATER_SLOTS_IN_CONF].statCategory     = MMS_STAT_CATEGORY_CONFSLOTS;       
  alarmStates[NDX_LOWATER_SLOTS_IN_CONF].alarmType        = MMS_LOWATER_SLOTS_IN_CONF; 
  alarmStates[NDX_LOWATER_SLOTS_IN_CONF].statCategory     = MMS_STAT_CATEGORY_CONFSLOTS; 

  alarmStates[NDX_NO_MORE_CONFERENCES].alarmType          = MMS_NO_MORE_CONFERENCES;
  alarmStates[NDX_NO_MORE_CONFERENCES].statCategory       = MMS_STAT_CATEGORY_CONFERENCES; 
  alarmStates[NDX_NO_MORE_CONFERENCES].lock               = new ACE_Thread_Mutex();        
  alarmStates[NDX_HIWATER_CONFERENCES].alarmType          = MMS_HIWATER_CONFERENCES;
  alarmStates[NDX_HIWATER_CONFERENCES].statCategory       = MMS_STAT_CATEGORY_CONFERENCES; 
  alarmStates[NDX_HIWATER_CONFERENCES].lock               = new ACE_Thread_Mutex();            
  alarmStates[NDX_LOWATER_CONFERENCES].alarmType          = MMS_LOWATER_CONFERENCES;
  alarmStates[NDX_LOWATER_CONFERENCES].statCategory       = MMS_STAT_CATEGORY_CONFERENCES;
  alarmStates[NDX_LOWATER_CONFERENCES].lock               = new ACE_Thread_Mutex();     
   
  alarmStates[NDX_NO_MORE_TTS_PORTS_FAILS].alarmType      = MMS_NO_MORE_TTS_PORTS_FAILS; 
  alarmStates[NDX_NO_MORE_TTS_PORTS_FAILS].statCategory   = MMS_STAT_CATEGORY_TTS; 
  alarmStates[NDX_NO_MORE_TTS_PORTS_FAILS].lock           = new ACE_Thread_Mutex();          
  alarmStates[NDX_NO_MORE_TTS_PORTS_QUEUES].alarmType     = MMS_NO_MORE_TTS_PORTS_QUEUES; 
  alarmStates[NDX_NO_MORE_TTS_PORTS_QUEUES].lock          = new ACE_Thread_Mutex();
  alarmStates[NDX_NO_MORE_TTS_PORTS_QUEUES].statCategory  = MMS_STAT_CATEGORY_TTS;         
  alarmStates[NDX_HIWATER_TTS_PORTS].alarmType            = MMS_HIWATER_TTS_PORTS;    
  alarmStates[NDX_HIWATER_TTS_PORTS].statCategory         = MMS_STAT_CATEGORY_TTS; 
  alarmStates[NDX_HIWATER_TTS_PORTS].lock                 = new ACE_Thread_Mutex();          
  alarmStates[NDX_LOWATER_TTS_PORTS].alarmType            = MMS_LOWATER_TTS_PORTS; 
  alarmStates[NDX_LOWATER_TTS_PORTS].statCategory         = MMS_STAT_CATEGORY_TTS; 
  alarmStates[NDX_LOWATER_TTS_PORTS].lock                 = new ACE_Thread_Mutex();          
   
  alarmStates[NDX_NO_MORE_ASR_RESX].alarmType             = MMS_NO_MORE_ASR_RESX; 
  alarmStates[NDX_NO_MORE_ASR_RESX].statCategory          = MMS_STAT_CATEGORY_ASR; 
  alarmStates[NDX_NO_MORE_ASR_RESX].lock                  = new ACE_Thread_Mutex();          
        
  alarmStates[NDX_HIWATER_ASR_RESX].alarmType             = MMS_HIWATER_ASR_RESX;    
  alarmStates[NDX_HIWATER_ASR_RESX].statCategory          = MMS_STAT_CATEGORY_ASR;
  alarmStates[NDX_HIWATER_ASR_RESX].lock                  = new ACE_Thread_Mutex();          
      
  alarmStates[NDX_LOWATER_ASR_RESX].alarmType             = MMS_LOWATER_ASR_RESX;    
  alarmStates[NDX_LOWATER_ASR_RESX].statCategory          = MMS_STAT_CATEGORY_ASR;
  alarmStates[NDX_LOWATER_ASR_RESX].lock                  = new ACE_Thread_Mutex();                

  return 0;
}



void MmsReporter::cleanup()
{
  struct alarmState* state = &alarmStates[0];

  for(int i = 0; i < MmsReporter::NDX_ALARM_TYPES_COUNT; i++, state++) 
      if (state->lock)
      {   delete state->lock;
          state->lock = NULL;
      }
}



char* MmsReporter::mediaTypeText(const int nType)
{
  // For the specified resource type (from enum statCategories), return a text
  // description of the type, to be used in an alarm desciption

  const char* mediaTypes[MMS_STAT_CATEGORY_COUNT] =
  { "server", "RTP", "voice", "low bit rate", "conference", "conference slots", 
    "text to speech", "speech recognition", "speech recognition" 
  };
  
  return nType >= 0 && nType < MMS_STAT_CATEGORY_COUNT? mediaTypes[nType]: "unknown";
}



int MmsReporter::timestampText(char* buf, time_t stamp) 
{
  // Convert time_t to string format expected by stat server "yyyy-MM-dd HH-mm-ss"
  // Returns number of characters in string.

  const char* STATSRV_TIMESTAMPMASK = "%04d-%02d-%02d %02d-%02d-%02d"; 
  struct tm* t = ACE_OS::localtime(&stamp);

  return ACE_OS::sprintf(buf, STATSRV_TIMESTAMPMASK, 
         t->tm_year+1900, t->tm_mon+1, t->tm_mday,
         t->tm_hour,      t->tm_min,   t->tm_sec);
}


unsigned MmsReporter::getUniqueTick() 
{
  // Unduplicate tickcount
  static unsigned lasttick;   
  unsigned thistick = Mms::getTickCount();

  if (thistick < lasttick) 
      lasttick = thistick; // close transactions, or rollover
   
  if (thistick == lasttick) thistick++;

  lasttick = thistick;
  return thistick;
}


void MmsReporter::raiseServerAlarm(const int alarmType, char* msg)
{
  // Raise an alarm indicating a server condition, as opposed to a resource condition.
  // NDX_SERVER_COMPROMISED      
  // NDX_UNEXPECTED_CONDITION    
  // NDX_UNSCHEDULED_SHUTDOWN    
  // NDX_RESX_NOT_DEPLOYED        
   
  MmsAlarmParams* params = new MmsAlarmParams(alarmType, 
      MMS_STAT_CATEGORY_SERVER, MmsReporter::severityTypeSevere);

  if (msg? ACE_OS::strlen(msg): 0)
      ACE_OS::strncpy(params->text, msg, MmsAlarmParams::maxTextLength); 
  
  thisx->postMessage(MMSM_ALARMREQUEST, (long) params);
}



void MmsReporter::setShutdown()
{
   this->isEnabled = FALSE;
   this->isShutdownRequest = TRUE;

   if (this->ipcClient)
       this->ipcClient->isshutdown = TRUE;  
}



MmsReporter::MmsReporter(MmsTask::InitialParams* params): MmsBasicTask(params) 
{
  this->config = (MmsConfig*)params->config;               // ctor
  this->logger = (MmsTask*)params->logCallback;
  this->isShutdownRequest = FALSE;
  this->listenerThread = NULL;
  this->isinitialized = this->isStartupStatsPublished = 0;
  MmsAs::reporterCallback = MmsReporter::reporterCallback; // Plug in global callback
  MmsReporter::thisx = this;      // Static pointer to this object's message queue 
  MmsReporter::singleton = this;  // To do: formalize singleton
  MmsReporter::configx = this->config; 
  this->ipcClient = new MmsReporterIpcClient(this);
  this->initializeAlarmState();
}



void MmsReporter::waitOnConnectLock()
{
  if (!MmsReporter::connectLock) return;
  MmsReporter::connectLock->acquire();
  MmsReporter::connectLock->release();
}



void MmsReporter::destroy()
{
  if (MmsReporter::singleton)
  {                  
      MmsReporter::singleton->setShutdown();
      MmsReporter::singleton->onStopReporter();                    
      MmsReporter::singleton->cleanup();
                     
      delete MmsReporter::singleton;
      MmsReporter::singleton = NULL;
      delete MmsReporter::connectLock;
      MmsReporter::connectLock = NULL;
  }
}



MmsReporter::~MmsReporter()
{
  if (this->ipcClient) 
  {
      // It could be the case that the mms prematurely exited, due for example
      // to a licensing issue, and this object is being deleted as a result,
      // but the initial stat server socket connect (MmsReporter::connectSync())
      // is still pending and has not yet returned. If we were to not wait for 
      // the connect to return, the code at that point would have no object 
      // context (since we are here in the dtor), and it would therefore crash. 
      // However, MmsReporter::connectSync() sets a lock and clears it on return, 
      // so we wait on that same lock here in order to not delete this object 
      // out from under the connect.
        
      this->waitOnConnectLock();
      int n = 0;

      while((MmsReporter::isConnecting || MmsReporter::isDisconnecting) && ++n < 10)
             mmsSleep(MMS_N_MS(500)); 

      delete this->ipcClient;
  }
}



// - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - 
// MmsReporterIpcClient: stat server IPC client methods
// - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

MmsReporterIpcClient::MmsReporterIpcClient(MmsReporter* parent)
{
  this->statServerPort = this->isshutdown = this->isconnected = isreconnect 
      = consecutiveConnectLocks = consecutiveConnectFailures = 0;
  this->statServerIP = NULL;
  this->reporter = parent;
}



int MmsReporterIpcClient::postIpcMessage(const int msg, Metreos::FlatMapWriter& writer)
{
  // Post message to stat server
  bool result = FlatMapIpcClient::Write(msg, writer);

  return result == true;
}

  

void MmsReporterIpcClient::OnIncomingFlatMapMessage
( const int msgType, const Metreos::FlatMapReader& reader)
{
  // Callback for message received from stat server
  // We ignore any and all responses from stat server. 
  // MMSLOG((LM_DEBUG, "REPO reply received from stat server\n"));  
}



int MmsReporterIpcClient::blockingConnectToStatServer(const int port, char* ip)
{
  // Synchronous connect to stat server. Returns 1 if OK, 0 if not.

  this->statServerPort = port; this->statServerIP = ip;
  int result = TRUE;

  MMSLOG((LM_INFO,"REPO connecting to stat server ...\n"));

  bool connectResult = this->Connect(ip, port); 

  if  (connectResult)  
  {   
       MMSLOG((LM_INFO,"REPO connected to stat server %s/%d\n", ip, port));
       mmsSleep(MMS_N_MS(MmsReporterIpcClient::WAITMS_AFTER_CONNECT)); 
       this->isreconnect = this->isconnected = TRUE;
  }
  else
  {    result = FALSE;
       MMSLOG((LM_ERROR,"REPO failed connect to stat server %s/%d\n", ip, port));

       if ((++consecutiveConnectFailures) >= MmsReporterIpcClient::MAX_CONNECT_ATTEMPTS)
       {
            // Once we hit this threshhold we do not attempt again until pinged by smgr.
            // Semikludgy logic below ensures we do not keep incrementing and roll over.
            consecutiveConnectFailures = MmsReporterIpcClient::MAX_CONNECT_ATTEMPTS;                
       }
       else mmsSleep(MMS_N_MS(MmsReporterIpcClient::WAITMS_AFTER_CONNECTFAIL));
  }        

  return result;
}



int MmsReporterIpcClient::initiateConnectToStatServer(const int port, char* ip)
{
  // Kick off async connect to stat server. IPC calls back to OnConnected()
  // or OnFailure().

  #if(0)                                    // We do not async connect
  this->statServerPort = port; this->statServerIP = ip;

  const int spawnResult = ACE_Thread_Manager::instance()->spawn(ConnectThread, 
        this, THR_NEW_LWP | THR_JOINABLE, &reporter->listenerThread);

  return (spawnResult != -1);
  #else
  return 0;
  #endif
}



int MmsReporterIpcClient::disconnectFromStatServer()
{
  // Disconnect IPC connection to stat server. IPC calls back to OnDisconnected().
  int result = 0;

  if  (this->isconnected && !MmsReporter::isDisconnecting)
  {    
       this->Disconnect();
  }
  else result = -1;

  return result;
}



void MmsReporterIpcClient::OnDisconnected() 
{
  // Callback for IPC listener thread termination

  this->isconnected = FALSE;

  // This call apparently comes in on another thread because logging does not work.
  // So we post a message to the parent object in order to get the logging done.
  // MMSLOG((LM_DEBUG,"REPO disconnected from stat server\n")); 

  if (reporter->isEnabled)
      reporter->postMessage(MMSM_REPORTER_LOG_SERVERDISCO); 
}



void MmsReporterIpcClient::OnFailure() 
{
  // Callback for IPC socket failure

  this->isconnected = FALSE;

// This call apparently comes in on another thread because logging does not work.
// So we post a message to the parent object in order to get the logging done.
  if (reporter->isEnabled)
      reporter->postMessage(MMSM_REPORTER_LOG_SOCKETERR); 
}



ACE_THR_FUNC_RETURN MmsReporterIpcClient::ConnectThread(void* data)
{
  // Thread with which to do an async connect to stat server. We are not connecting 
  // async so this is currently unused. See comments at MmsReporter::connectSync()

  #if(0)
  // Thread to initiate socket connection to stat server and wait for result
  MmsReporterIpcClient* parent = (MmsReporterIpcClient*)(data);
  const int maxattempts = parent->isreconnect? 1: 
            MmsReporterIpcClient::INITIAL_CONNECT_ATTEMPTS;
  bool connectResult;
  ACE_DEBUG((LM_ERROR,"REPO connecting to stat server ...\n"));

  for(int i=0; i < maxattempts; i++)
  {  
      if (parent->isshutdown) break;

      if (connectResult = parent->Connect(parent->statServerIP, parent->statServerPort)) 
          break; 
      
      mmsSleep(MMS_N_MS(MmsReporterIpcClient::WAITMS_BETWEEN_CONNECTS));    
  }

  if  (parent->isshutdown);
  else
  if  (connectResult)
  {    parent->consecutiveConnectFailures = 0;
       MMSLOG((LM_DEBUG,"REPO stat server connection OK\n"));
       mmsSleep(MMS_N_MS(MmsReporterIpcClient::WAITMS_AFTER_CONNECT)); 
  }
  else
  {    parent->consecutiveConnectFailures++;
       MMSLOG((LM_ERROR,"REPO connect failed to stat server %s/%d\n",
               parent->statServerIP, parent->statServerPort));

       if  (parent->consecutiveConnectFailures == MmsReporterIpcClient::MAX_CONNECT_ATTEMPTS)
       {
       }
       else mmsSleep(MMS_N_MS(MmsReporterIpcClient::WAITMS_AFTER_CONNECTFAIL));
  }

  parent->isreconnect = TRUE;
  parent->reporter->listenerThread = NULL;
  parent->reporter->isConnecting = FALSE;
  parent->connectLock.release();
  // Don't debug break here since connects can now get through
  return connectResult == true;
  #else
  return 0;
  #endif
}


