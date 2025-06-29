//
// MmsSessionD.cpp
// 
// Session object utility methods
//
#include "StdAfx.h"
#pragma warning(disable:4786)
#ifdef MMS_WINPLATFORM
#include <minmax.h>
#endif
#include <ctype.h>
#include "mmsSession.h"
#include "mmsParameterMap.h"
#include "mmsMediaEvent.h"
#include "mmsSessionManager.h"
#include "mmsAudioFileDescriptor.h"
#include "mmsCommandTypes.h"

#ifdef MMS_ENABLE_MEMORY_LEAK_DETECTION
#define new MMS_MEMLEAK_NEW
#endif // MMS_ENABLE_MEMORY_LEAK_DETECTION  

      
                                            // Construction initialization
void MmsSession::init(const int ordinal, MmsConfig* config)    
{
  this->ordinal = ordinal;                  // 1-based session ID
  this->config  = config;
  this->sessionMgr     = NULL;
  this->resourceMgr    = NULL;
  this->reconnectInfo  = NULL;
  this->conxID = this->flags = 0;
  this->confInfo.clear();
  this->clearCommon();
  this->stamp(); 
  ACE_OS::sprintf(objname,"S%03d",ordinal);
}



void MmsSession::clearCommon()              // Common to init() and clear()
{
  this->client = this->clientHandle = NULL; 
  this->pendingDisconnectMap = NULL; 
  this->flags = this->opcount = 0;
  ACE_OS::memset(&digitCache, 0, sizeof(digitCache));
  this->clearReconnectState();
  this->state = AVAIL;
  this->rfc2833Registered = 0;
  if (this->routingGuid)    
      delete [] this->routingGuid;
  if (this->cmdHeader)  
      delete this->cmdHeader;
  this->routingGuid = NULL;
  this->cmdHeader = NULL;  
}



int MmsSession::clear()                     // Clear session object
{
  this->sessionTimeoutSecs = 0;
  this->clearCommon();
  return 0;
}
  

                                             
MmsSession::MmsSession()                    // Ctor 
{
  this->hIP = -1;
  this->callStateMon = NULL;
  this->routingGuid = NULL;
  this->cmdHeader = NULL;

  MmsSession::Op* op = &this->optable[0];
  for(int i=0; i < MMS_MAX_SESSION_OPERATIONS; i++, op++) 
  {   op->clear(); 
      op->session = this;
  }
}



MmsSession::~MmsSession()                   // Dtor
{                                           
  // We allocate session objects only en masse as members of session pool arrays.
  // When we delete these session pool(s), the session's environment likely no 
  // longer exists, and so session object destructors should not attempt to 
  // release any resources other than instance memory which the session itself 
  // may have allocated; and at this writing there are no such allocations. 
  // For the same reasons we do not require a copy constructor.
}



// - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
// Session state transition hooks 
// - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -


int MmsSession::onSessionStart()
{
  // Invoked at moment a connect request is identified by session manager,
  // just prior to creating a session pool binding from a connection ID 
  // to a sesson object.
   
  if (this->flags & MMS_SESSION_FLAG_SESSION_ACTIVE) return -1;
  this->flags |= MMS_SESSION_FLAG_SESSION_ACTIVE;
  this->stamp();                            // Mark session start time
  // time(&this->senddigitstimestamp);
  this->senddigitstimestamp = 0; 
  resetSessionTimer();
  return 0;
}



int MmsSession::onCommandStart(char* newflatmap, void** outOp)
{
  // Invoked on each command assignment to service (session manager context)
  // Returns newly assigned operation ID, or MMS_OPERROR_OPTABLE_OVERFLOW  
  // if no slot remains available in the optable.

  MmsSession::Op* operation = this->getAvailableOp();
  if (!operation) return MMS_OPERROR_OPTABLE_OVERFLOW;  

  const int newOperationID = sessionMgr->sessionPool()->generateOperationID();

  operation->init(this, newOperationID);
  setFlatmapOperationID(newflatmap, newOperationID);

  this->state = BUSY; 
  this->clientHandle = getFlatmapClientHandle(newflatmap);
  this->client = getFlatmapSender(newflatmap);  
  setFlatmapSessionID(newflatmap, this->ordinal); 

  this->cacheCommandHeader(newflatmap);     // Save header info with session 

  operation->onCommandStart(newflatmap);    // Inititalize operation

  if (outOp) *outOp = operation;            // Return operation object ...                                             

  return newOperationID;                    // ... and operation ID
}



void MmsSession::cacheCommandHeader(char* map)
{
  if (this->cmdHeader || !map) return;
  this->cmdHeader = new MmsServerCmdHeader();
  MmsServerCmdHeader* p = (MmsServerCmdHeader*)(map + sizeof(MmsFlatMap::MapHeader));   
  memcpy(this->cmdHeader, p, sizeof(MmsServerCmdHeader));
}



void MmsSession::onAssignSessionTimeout(int sessionTimeoutSecs)
{
  // Invoked as timeout parameter is extracted from parameter map
   
  this->sessionTimeoutSecs = 
        sessionTimeoutSecs? sessionTimeoutSecs:
        config->serverParams.sessionTimeoutSecondsDefault;

  resetSessionTimer();
}



void MmsSession::onSessionEnd()
{
  // Invoked by session mgr on completion of disconnect or after session timeout
  this->flags |= MMS_SESSION_FLAG_TEARDOWN_IN_PROGRESS;  
  this->releaseResources();                   
                                             
  this->clear();                            // Clear out the session object
  this->clearOptable(TRUE);                 // Ensure op table is empty
  this->stamp();                            // Mark session end time

  MMSLOG((LM_INFO,"POOL session %d end\n", this->ordinal));
}



int MmsSession::getEffectiveDefaultCommandTimeout(char* flatmap)
{
  // Determine effective default command timeout as the maximum of the configured 
  // default timeout for a generic command, and the configured default timeout 
  // for the specific command.
   
  int commandID = flatmap? getFlatmapCommand(flatmap): 0;
  int specificCommandTimeoutMs = 0;

  switch(commandID)
  {
    case COMMANDTYPE_PLAY:
         specificCommandTimeoutMs = config->serverParams.commandTimeoutMsecsPlay;
         break;
         
    case COMMANDTYPE_RECORD: 
         specificCommandTimeoutMs = config->serverParams.commandTimeoutMsecsRecord;
         break;

    case COMMANDTYPE_RECEIVE_DIGITS: 
         specificCommandTimeoutMs = config->serverParams.commandTimeoutMsecsGetDigits;
         break;

    case COMMANDTYPE_MONITOR_CALL_STATE: 
         specificCommandTimeoutMs = config->serverParams.commandTimeoutMsecsMonitorCallState;
         break;

    case COMMANDTYPE_RECORD_TRANSACTION: 
         specificCommandTimeoutMs = config->serverParams.commandTimeoutMsecsRecordTransaction;
         break;
  }

  const int defaultTimeoutMs 
      = max(specificCommandTimeoutMs, config->serverParams.commandTimeoutMsecsDefault);

  return defaultTimeoutMs;
}



// - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
// Session media resource management
// - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -


MmsDeviceIP* MmsSession::ipDevice()          
{
  MmsDeviceIP* device = NULL;

  if  (isValidDeviceHandle(this->hIP))
       device = (MmsDeviceIP*)resourceMgr->getDevice(this->hIP);

  return device;   
}


                                             
MmsDeviceConference* MmsSession::conferenceDevice()         
{                                            
  MmsDeviceConference*  device = NULL;
  mmsDeviceHandle handle = this->confInfo.confResx;

  if (!isValidDeviceHandle(handle))
       MmsConferenceManager::conferenceDevice(&handle, &device, this->ordinal, 1);

  if  (isValidDeviceHandle(handle))
       device = (MmsDeviceConference*)resourceMgr->getDevice(handle);

  return device;   
}



int MmsSession::isSessionClosed()
{
  // Indicate if session is in a state in which it can not respond to events
   
  int result = FALSE;

  if (((this->flags & MMS_SESSION_FLAG_SESSION_ACTIVE) == 0)   
    || (this->flags & MMS_SESSION_FLAG_TEARDOWN_IN_PROGRESS)) 
        result = TRUE;

  return result;
}



void MmsSession::releaseResources()
{ 
  // Release all resources allocated by/for the session

  ACE_Guard<ACE_Thread_Mutex> x(slock);
    
  if (this->isInConference())               // If currently in conference ...
      this->handleConferenceDisconnect();   // ...  ensure disconnected

  this->confInfo.clear();

  MmsDeviceIP* deviceIP = this->ipDevice(); // Stop IP
  if (deviceIP && !deviceIP->isStopped())
      deviceIP->stop(STOP_ALL);              

  // IP object close() adjusted the G729 resource count above if necessary
  this->flags &= ~MMS_SESSION_FLAG_G729_RESOURCE_SPENT;
                                            // Free up G.711 license 
  if (this->flags & MMS_SESSION_FLAG_G711_RESOURCE_SPENT)
  {   MmsAs::g711(MmsAs::RESX_INC);         // LICX G711+ 
      this->flags &= ~MMS_SESSION_FLAG_G711_RESOURCE_SPENT;
  }   // Codec license use is associated with the session, not with an 
      // individual operation, since the session *is* the RTP connection.                                              

  this->releaseSessionVox();                // Sanity check
}



void MmsSession::releaseSessionVox()
{ 
  // Release all voice resources remaining in session
  // Ideally this is not necessary; however we'll leave it here as a sanity 
  // check to ensure that all voice has been returned to available as expected.

  ACE_Guard<ACE_Thread_Mutex> x(this->optableLock);
  MmsSession::Op* op = &this->optable[0];
  HmpResourceManager* resourceMgr = this->resourceManager();

  for(int i=0; i < MMS_MAX_SESSION_OPERATIONS; i++, op++) 
  {
      if (!op->isBusy()) continue;
      MmsDeviceVoice* deviceVoice = op->assignedVoiceDevice();
      if (!deviceVoice) continue;

      const int hvox = op->hVoice();
      // Note FYI that we have never seen this message in the log 
      MMSLOG((LM_INFO,"%s warning vox %d remains in session\n", objname, hvox));

      if (deviceVoice->isListening()) deviceVoice->unlisten();

      if (resourceMgr->isVoxUnavailable(hvox))   
          resourceMgr->resourcePoolAvailable(hvox, TRUE);
  }  
}



MmsMediaDevice* MmsSession::getMediaDevice(mmsDeviceHandle hdev)
{
  return resourceManager()->getDevice(hdev); 
}



int MmsSession::selfListen(char* flatmap)
{
  // Listen connection to itself  

  // Since we listen inline (synchronous) during the connect, thus blocking 
  // the thread on which the connect is running, the IP media started event 
  // could fire before the connect code has run its course. Since a delay here
  // can be critical for some applications, rather than spin wait for the code
  // to catch up, we clear the dependency pending flag for this special case.
      
  if (flatmap)
      clearFlatmapXflag(flatmap, MmsServerCmdHeader::IS_DEPENDENCY_PENDING);

  MmsDeviceIP* deviceIP = this->ipDevice();

  const int result = deviceIP? deviceIP->selflisten(): -1;

  if (result != -1) MMSLOG((LM_DEBUG,"%s connection self-listens\n", objname));

  return result;
}



int MmsSession::switchFromConferenceToVoice(MmsMediaDevice* deviceVoice, ListenDirection dir)
{
  // Disconnect IP from conference and hook it to vox

  const int direction = dir == FULLDUPLEX? 
            MmsMediaDevice::FULLDUPLEX: MmsMediaDevice::HALFDUPLEX;

  if (this->isHairpinConference()) 
      return this->switchFromHairpinToVoice(deviceVoice, direction); 

  MmsDeviceIP*         deviceIP   = this->ipDevice();
  MmsDeviceConference* deviceConference = this->conferenceDevice();

  MmsHmpConference* conference = this->getHmpConferenceObject(deviceConference);
  if (conference == NULL) return MMS_ERROR_SERVER_INTERNAL;

  if  (conference->isMonitor(deviceIP))
  {
       deviceConference->unlistenOnMonitorChannel(confInfo.id, deviceIP);
       // Indicate that reconnectToConference is to remonitor
       this->flags |= MMS_SESSION_FLAG_IS_CONFERENCE_MONITOR;  
  }
  else deviceConference->busDisconnect(deviceIP, 0); 
                                       
  if  (-1 == this->ipDevice()->busConnect(deviceVoice, direction))
       return MMS_ERROR_DEVICE;

  return 0;
}



int MmsSession::switchFromHairpinToVoice(MmsMediaDevice* deviceVoice, unsigned int mode)
{
   // Disconnect this session from hairpin conference and hook it to voice
    
   MmsConference* conference = (MmsConference*)getMmsConferenceObject(confInfo.id);
   if (!conference || !conference->isactive || conference->size() < 2) return 0;

   int result = conference->unhairpin(TRUE);// Unlisten hairpin
                                            // Listen voice
   result = this->ipDevice()->busConnect(deviceVoice, mode);

   return result == -1? MMS_ERROR_DEVICE: 0;
}



int MmsSession::reconnectToConference(MmsMediaDevice* olddevice)
{
  // Reconnect IP to conference after temporarily listening to vox,
  // or after placing IP session on hold while switching port/IP
  // olddevice may be NULL, in which case it will be ignored.
 
  if  (this->isSessionClosed()) return MMS_ERROR_NOT_CONNECTED;

  MmsConference* mmsConference = (MmsConference*)getMmsConferenceObject(confInfo.id);
  if (!mmsConference || !mmsConference->isactive) return -1;

  if (this->isHairpinConference()) return this->reconnectToHairpin(); 

  // Prevent an errant disconnect from proceeding until reconnect has completed
  ACE_Guard<ACE_Thread_Mutex> x(slock);

  // We'll get a lock on the mms conference wrapper in order that the HMP 
  // conference cannot be destroyed in another thread while we're reconnecting. 
  // MmsConferenceManager::teardownConference will acquire the same lock.
  ACE_Guard<ACE_Thread_Mutex> y(mmsConference->dlock);

  MmsDeviceIP* deviceIP = this->ipDevice();
  MmsDeviceConference* deviceConference = this->conferenceDevice();

  MmsHmpConference* conference = this->getHmpConferenceObject(deviceConference);
  if (conference == NULL) return MMS_ERROR_SERVER_INTERNAL;

  int  result = 0, confid = confInfo.id & 0xffff;
                                       // Re-establish conference monitor
  if  (this->flags & MMS_SESSION_FLAG_IS_CONFERENCE_MONITOR)
  {    
       this->flags &= ~(MMS_SESSION_FLAG_IS_CONFERENCE_MONITOR);
       result = deviceConference->listenOnMonitorChannel(confInfo.id, deviceIP);
  }                                    // Disco IP from vox, rehook to conf                                       
  else result = deviceConference->connectResource(confInfo.id, 
                confInfo.handle, deviceIP, deviceIP, olddevice); 

  if  (result == -1)
  {    MMSLOG((LM_ERROR,"%s could not reconnect to conference %d\n",objname,confid)); 
       return MMS_ERROR_DEVICE;  
  }
  else MMSLOG((LM_INFO,"%s reconnect to conference %d\n", objname, confid)); 

  return 0;
} 



MmsHmpConference* MmsSession::getHmpConferenceObject
( MmsDeviceConference* deviceConf, const int isLog)
{
  // Get HMP conference object for session's conference ID

  if (!deviceConf)
  {   if (isLog) MMSLOG((LM_ERROR,"%s conference device not available\n", objname));
      return NULL;
  }

  const int conferenceID = confInfo.id;
  MmsHmpConference* conference = deviceConf->getConference(conferenceID);

  if (conference == NULL)   
      if (isLog)
          MMSLOG((LM_ERROR,"%s no HMP conference exists for conf ID %d\n",
                  objname, conferenceID));

  return conference;
}



int MmsSession::reconnectToHairpin()
{
  // Reconnect a hairpin after after temporarily listening to vox,
  // or after placing IP session on hold while switching port/IP

  // Prevent an errant disconnect from proceeding until reconnect has completed
  ACE_Guard<ACE_Thread_Mutex> x(slock);

  MmsConference* conference = (MmsConference*)getMmsConferenceObject(confInfo.id);
  if (!conference || !conference->isactive) return MMS_ERROR_NOT_CONNECTED;
  // If currently only one party in the hairpin pair, nothing to do
  if (conference->size() < 2) return 0;

  const int result = conference->hairpin(TRUE);

  return result == 0? 0: MMS_ERROR_DEVICE;
}



int MmsSession::muteHairpinned(const int isMuting)
{
  // Mute/unmute this member of a hairpinned conference

  MmsDeviceIP* deviceIP = this->ipDevice();
  ACE_Guard<ACE_Thread_Mutex> x(slock);

  MmsConference* conference = (MmsConference*)getMmsConferenceObject(confInfo.id);
  if (!conference || !conference->isactive) return -1;
    
  const int result = conference->mutehairpinned(this, isMuting);
  return result;
}



int MmsSession::promoteConference(char* flatmap)
{  
  MmsConference* conference = (MmsConference*)this->getMmsConferenceObject(confInfo.id);
  return conference? conference->promote(this, flatmap): -1;
} 



int MmsSession::demoteConference()
{     
  MmsConference* conference = (MmsConference*)this->getMmsConferenceObject(confInfo.id);
  return conference? conference->demote(): -1;
} 



int MmsSession::isInConference()
{     
  return confInfo.id > 0; 
} 



int MmsSession::sendCachedDigits(int voxcount, const int clearcache)
{
  // Send cached digits via IP through to all resident voice resources  

  ACE_Guard<ACE_Thread_Mutex> x(this->atomicOperationLock);

  MmsDeviceIP* deviceIP = this->ipDevice(); 
  const int cacheSize = this->digitCacheSize();
  if (cacheSize == 0 || deviceIP == NULL) return 0;
  int result = 0;

  if (voxcount == 0)
      voxcount = this->voiceResourceCount();

  if (voxcount == 0)
  {
      MMSLOG((LM_ERROR,"%s no vox available to receive cached '%s'\n", 
              objname, this->digitCache)); 
      result = -1;
  }
  else
  {   MMSLOG((LM_DEBUG,"%s sending cached '%s' via IP%d to %d vox\n", 
              objname, this->digitCache, deviceIP->handle(), voxcount));  

      result = deviceIP->sendDigits(digitCache, cacheSize, MmsMediaDevice::SYNC);
      if (result != -1)
          this->stampSendDigits();
  }

  if (clearcache)
      ACE_OS::memset(&digitCache, 0, sizeof(digitCache));    

  return result == -1? MMS_ERROR_DEVICE: 0; 
}



int MmsSession::cacheDigits(const char* digits, const int numdigits)
{
  // Cache arriving digits in session digit cache buffer

  ACE_Guard<ACE_Thread_Mutex> x(this->atomicOperationLock);

  const int cacheSize = this->digitCacheSize();
  const int newsize = cacheSize + numdigits;
  
  if (newsize > MMS_SIZE_DIGITCACHE)
  {   MMSLOG((LM_ERROR,"%s digit cache overflow\n", objname));
      return -1; 
  }

  ACE_OS::strcat(this->digitCache, digits);

  if (this->config->diagnostics.flags & MMS_DIAG_LOG_DIGBUF)
      MMSLOG((LM_DEBUG,"%s caching '%s' cache is '%s'\n", objname, digits, this->digitCache)); 

  return 0;
}
  


int MmsSession::clearDigitCache(int cleardigitcount)
{
  // Clear digit cache and return count of digits cleared

  ACE_Guard<ACE_Thread_Mutex> x(this->atomicOperationLock);

  const int cacheSize = this->digitCacheSize();
  
  if (cleardigitcount && cacheSize>cleardigitcount)
  {
      char buff[MMS_SIZE_DIGITCACHE+1];
      if (this->config->diagnostics.flags & MMS_DIAG_LOG_DIGBUF)
      {
          if  (cacheSize)
          {
              ACE_OS::memset(&buff, 0, sizeof(buff));
              ACE_OS::memcpy(buff, digitCache, cleardigitcount);
              MMSLOG((LM_DEBUG,"%s clear digit cache '%s'\n", objname, buff)); 
          }
      }

      // we only want to clear x digits
      ACE_OS::memset(&buff, 0, sizeof(buff));
      ACE_OS::memcpy(buff, digitCache+cleardigitcount, cacheSize-cleardigitcount);
      ACE_OS::memcpy(digitCache, buff, sizeof(buff));
  }
  else
  {
      if (this->config->diagnostics.flags & MMS_DIAG_LOG_DIGBUF)
          if (cacheSize)
              MMSLOG((LM_DEBUG,"%s clear digit cache '%s'\n", objname, this->digitCache)); 

      ACE_OS::memset(&digitCache, 0, sizeof(digitCache));
  }

  return cacheSize; 
} 



int MmsSession::isDigitLingering()
{
    time_t now;
    time(&now);

    if (difftime(now, this->senddigitstimestamp) < 2)
        return 1;

    return 0;
}



int MmsSession::isRemoteSessionStarted()
{   
   MmsDeviceIP* deviceIP = this->ipDevice(); 
   return deviceIP && deviceIP->isStarted();
}



void MmsSession::clearReconnectState()
{   
   if (this->reconnectInfo) delete this->reconnectInfo;
   this->reconnectInfo = NULL;
}



void MmsSession::logDigitPatternTermination()
{
  if (config->serverLogger.globalMessageLevel >= 3)
      this->logTerminatingCondition(TM_METREOS_DIGPATTERN);   
}



void MmsSession::logTerminatingCondition(const unsigned int hmpTermCode)
{  
  char* x = "other"; 

  switch(hmpTermCode)
  {
    case TM_MAXDTMF:  x = "maxdtmf";  break;                   
    case TM_MAXTIME:  x = "maxtime";  break;             
    case TM_DIGIT:    x = "digit";    break;                             
    case TM_USRSTOP:  x = "userstop"; break;             
    case TM_EOD:      x = "eod";      break;
    case TM_MAXSIL:   x = "maxsil";   break;          
    case TM_MAXNOSIL: x = "maxnosil"; break;                              
    case TM_IDDTIME:  x = "iddtime";  break;   
    case TM_NORMTERM: x = "normterm"; break;                             
    case TM_ERROR:    x = "error";    break;              
    case TM_MAXDATA:  x = "maxdata";  break; 
    case TM_METREOS_AUTOSTOP:   x = "autostop"; break;
    case TM_METREOS_TIMEOUT:    x = "timeout";  break;
    case TM_METREOS_DIGPATTERN: x = "digitpattern";   
  }

  MMSLOG((LM_DEBUG,"%s media term %s\n", objname, x));
}


