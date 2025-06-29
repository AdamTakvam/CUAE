//
// MmsOperation.cpp
//
// Session operation object
//
#include "StdAfx.h"
#pragma warning(disable:4786)
#include "mmsSession.h"
#include "mmsReporter.h"
#include "mmsParameterMap.h"
#include "mmsMediaEvent.h"
#include "mmsSessionManager.h"
#include "mmsCommandTypes.h"
#include "mmsAudioFileDescriptor.h"
#include "mmsTts.h"
#include "mmsAsr.h"

#ifdef MMS_ENABLE_MEMORY_LEAK_DETECTION
#define new MMS_MEMLEAK_NEW
#endif // MMS_ENABLE_MEMORY_LEAK_DETECTION


// - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
// operation locate
// - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -


MmsSession::Op* MmsSession::findByOpID(const int opid, const int needLock)
{
  // Find and return the operation for which the key is supplied

  if (needLock) this->optableLock.acquire();
  MmsSession::Op* op = &this->optable[0];

  for(int i=0; i < MMS_MAX_SESSION_OPERATIONS; i++, op++)  
      if (op->opID() == opid) break;

  if (needLock) this->optableLock.release();
  return (i >= MMS_MAX_SESSION_OPERATIONS)? NULL: op;
}



MmsSession::Op* MmsSession::findByEventID(const int eventid, const int needLock)
{
  // Find and return the operation waiting on the specified event

  if (needLock) this->optableLock.acquire();
  MmsSession::Op* op = &this->optable[0];

  for(int i=0; i < MMS_MAX_SESSION_OPERATIONS; i++, op++)  
      if (op->waitinfo().eventID == eventid) break;

  if (needLock) this->optableLock.release();
  return (i >= MMS_MAX_SESSION_OPERATIONS)? NULL: op;
}



MmsSession::Op* MmsSession::getAvailableOp()
{
  // Get an available slot in the session operation table. Once located,
  // the slot is reserved and the session operation count bumped.
  // session.closeOperation must be invoked to return the slot to use.

  ACE_Guard<ACE_Thread_Mutex> x(this->optableLock);
  MmsSession::Op* opslot = &this->optable[0];

  for(int i=0; i < MMS_MAX_SESSION_OPERATIONS; i++, opslot++)   
      if (opslot->isIdle()) break;

  if (i >= MMS_MAX_SESSION_OPERATIONS) return NULL;

  opslot->reserve();
  opslot->stamp();
  this->opcount++;
  return opslot;
}



int  MmsSession::getNumActiveOperations(const int voiceOpsOnly)
{
  // Get number of active (voice) operations in the same session.
  int count = 0;

  ACE_Guard<ACE_Thread_Mutex> x(this->optableLock);
  MmsSession::Op* opslot = &this->optable[0];

  for(int i=0; i < MMS_MAX_SESSION_OPERATIONS; i++, opslot++)   
  {
    if (opslot->isIdle()) continue;

    // if voice ops only requested, ignore non-voice command
    // COMMANDTYPE_PLAY               
    // COMMANDTYPE_RECORD             
    // COMMANDTYPE_RECORD_TRANSACTION 
    // COMMANDTYPE_PLAYTONE           
    // COMMANDTYPE_VOICEREC           
    // COMMANDTYPE_MONITOR_CALL_STATE 
    // COMMANDTYPE_RECEIVE_DIGITS

    if (voiceOpsOnly && !IS_ASYNC_VOICE_COMMAND(opslot->cmdid)) continue;

    count++; 
  }

  return count;
}



int MmsSession::hasOperation(int cmdId)
{
  // Indicate if specified command is currently resident
  int bFound = 0;

  ACE_Guard<ACE_Thread_Mutex> x(this->optableLock);
  MmsSession::Op* opslot = &this->optable[0];

  for(int i=0; i < MMS_MAX_SESSION_OPERATIONS && !bFound; i++, opslot++)   
  {
    if (opslot->isIdle()) continue;

    bFound = opslot->cmdid == cmdId;
  }

  return bFound;
}



MmsSession::Op* MmsSession::getOperation(MmsFlatMapReader& parameterMap, const int isLog)
{
  // Given a map reader, get operation ID from it and return operation

  const char* flatmap = (char*)parameterMap.header();
  const int operationID = getFlatmapOperationID(flatmap);

  MmsSession::Op* operation = this->findByOpID(operationID);

  if (!operation && isLog)
      MMSLOG((LM_ERROR,"%s invalid map operation ID %d\n", objname, operationID));
      
  return operation; 
}



int MmsSession::clearOptable(const int isLog)
{
  // Clear the session's operation table 

  ACE_Guard<ACE_Thread_Mutex> x(this->optableLock);
  MmsSession::Op* op = &this->optable[0];
  int count=0;

  for(int i=0; i < MMS_MAX_SESSION_OPERATIONS; i++, op++) 
  {
      if (op->isIdle()) continue;

      count++;

      if (isLog)
          MMSLOG((LM_ERROR,"%s warning session %d cmd %s was not closed\n", 
                  objname, ordinal, Mms::commandName(op->cmdID())));

      op->clear();
  }  
     
  this->opcount=0;
  return count;
}



MmsSession::Op* MmsSession::first()
{
  // Get the first active operation in the operation table. 

  ACE_Guard<ACE_Thread_Mutex> x(this->optableLock);
  MmsSession::Op* op = &this->optable[0];

  for(int i=0; i < MMS_MAX_SESSION_OPERATIONS; i++, op++) 
      if (op->isBusy()) break;

  return (i >= MMS_MAX_SESSION_OPERATIONS)? NULL: op;
}



// - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
// operation state
// - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -


int MmsSession::closeOperation(const int opid, const int cmdID, const int isLog)
{
  // Closes an operation (media server command), releasing its resources and
  // freeing its operation table slot. If this session's count of pending 
  // operations is now zero, the session is marked idle. 

  // We may want to define a new lock in the session which must be acquired 
  // here before an operation can be closed. Such a lock would guarantee that   
  // an operation currently under consideration cannot be closed while inspection
  // is in process. Let's postpone this until a demonstrated need arises.  
  ACE_Guard<ACE_Thread_Mutex> x(this->optableLock);

  MmsSession::Op* op = this->findByOpID(opid, FALSE);

  if (op == NULL) 
  {   MMSLOG((LM_ERROR,"OPER session %d could not close op %d cmd %s\n", 
              ordinal, opid, Mms::commandName(cmdID)));
      return -1;
  }

  // If this is a utility session, this is the only operation on the session, 
  // and this session is in conference. A utility session's operation's voice  
  // device handle is its CDT (conference descriptor table) entry index, so we 
  // need to leave the conference prior to releasing the operation's voice device. 
 
  if (this->isUtilitySession() && !this->isExitSupressed())  // UTLS         
      sessionMgr->conferenceManager()->leaveConference(this, this->confInfo.id);

  op->onCommandEnd(cmdID);

  op->clear();
  if (--this->opcount == 0) this->markIdle();

  if (isLog) MMSLOG((LM_INFO,"%s operation %d %s end\n", 
             objname, opid, Mms::commandName(cmdID)));

  return 0;
}



int MmsSession::Op::setState(const states newstate)
{
  // Set operation state to new state

  switch(newstate)
  {
    case states::IDLE: 
    case states::LIVE:
    case states::WAIT:
    case states::PEND:
         break;

    case states::RSVD:
         if (this->state != IDLE) return -1;
  }

  this->state = newstate;
  return 0;
}

 

void MmsSession::Op::markWaiting(int type, int id)
{
  if (type)
  {   this->waitInfo.eventType = type;
      this->waitInfo.eventID   = id;
      this->state = WAIT;
  }
  else this->waitInfo.eventType = this->waitInfo.eventID = 0;
}



void MmsSession::Op::init(MmsSession* session, const int operationID)
{ 
  // Initialize a new operation

  this->clear();
  this->session = session;
  this->opid = operationID;
  this->stamp();
  sprintf(logkey,"S%03d", session->ordinal);
}



void MmsSession::Op::clear()
{ 
  // Clear operation object. Parent session reference is not cleared.

  this->opid = this->cmdid = this->flags = this->hvox = 0;
  this->waitInfo.eventID = this->waitInfo.eventType = 0 ;
  this->svcThreadHandle = 0;
  this->timestamp = 0;
  this->asrChan   = NULL;
  this->ttsData   = NULL;
  memset(digitlist,0,MMS_SIZE_DIGITLIST);
  this->parameterMap.clear();
  this->state = IDLE;
  this->matchcount = 0;
}



// - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
// operation state transition event handlers
// - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -


void MmsSession::Op::onCommandStart(char* newflatmap)
{
  // Invoked on each command assignment to service (session manager context)
  // There is a version of this in the session as well.

  this->state = LIVE;
  this->commandTimeoutMs = session->getEffectiveDefaultCommandTimeout(newflatmap); 
  // Set valid command timeout before command starts  
  // so command timeout monitor won't retire the command immediately
  this->resetOperationTimer(this->commandTimeoutMs);

  if  (this->flatmap())
       MMSLOG((LM_ERROR,"%s warning op %d parameter map leak\n",logkey,opid));

  setFlatmapOperationID(newflatmap, this->opid);
                                              
  this->parameterMap.load(newflatmap);      // Realize parameter map index

  this->cmdid = getFlatmapCommand(newflatmap);

  this->flags |= MMS_OP_FLAG_COMMAND_INITIALIZED;

  if  (Config()->diagnostics.flags & MMS_DIAG_LOG_MAP_LIFETIME)
       MMSLOG((LM_DEBUG,"%s op %d assign map %x\n",
               logKey(), opid, this->flatmap()));
}



void MmsSession::Op::onCommandServiceStart(const int defaultCommandTimeoutMs)  
{
  // Invoked after command service begins. We set the command timeout here.
  // If client supplies a timeout, use that; otherwise, if there is a
  // specific config default for the command type, use that; otherwise use the 
  // default generic config command timeout. Command timeouts are primarily 
  // used to detect media firmware problems, such as when a command may not 
  // have generated an expected completion event. Media termination conditions 
  // should be used for the normal flow of timing out a command. Of course,
  // for the connection operation, this command timeout is the only choice.  

  this->tickstart = Mms::getTicks();         

  char* pcmdtimeout = NULL; 

  int  result = parameterMap.find(MMSP_COMMAND_TIMEOUT_MS, &pcmdtimeout);

  this->commandTimeoutMs = pcmdtimeout? *((int*)pcmdtimeout): 0;

  // If client wants infinite timeout on a command, they must pass an override
  // command timeout of zero with the command. We don't want to configure infinite 
  // timeouts across the board, since a "one-time" configuration can be forgotten.
  const int isInfiniteTimeout = pcmdtimeout && (this->commandTimeoutMs == 0);

  if  (this->commandTimeoutMs == 0 && !isInfiniteTimeout) 
       this->commandTimeoutMs  = defaultCommandTimeoutMs;

  if  (this->commandTimeoutMs == 0 && !isInfiniteTimeout) 
       this->commandTimeoutMs  = Config()->serverParams.commandTimeoutMsecsDefault; 

  if  (this->commandTimeoutMs == 0) 
       MMSLOG((LM_INFO,"%s %s command timeout is infinite\n",
               logKey(), Mms::commandName(this->cmdID())));

  resetOperationTimer(this->commandTimeoutMs);
}



void MmsSession::Op::onCommandEnter(int isExecutedAsync)  
{
  // Invoked ahead of the HMP call that executes the current command

  if (isExecutedAsync)                      // If it will be run async, indicate
  {   char* map = this->flatmap();          // provisional has not yet completed
      if (map) 
          setFlatmapXflag(map, MmsServerCmdHeader::IS_DEPENDENCY_PENDING); 
  }
}



void MmsSession::Op::onEventSink()   
{
  // Invoked on each event assignment to service (session manager context)

  session->resetSessionTimer();
                             
  // In case a provisional return had set the result code to other than zero 
  putMapHeader(setRetcode, 0);  
}



void MmsSession::Op::onServiceThreadEntry()   
{
  // Invoked in service thread context as thread goes active

  ACE_Thread_Manager::instance()->thr_self(this->svcThreadHandle);
}



int MmsSession::Op::onEventServiceStart(char* text, const int reset)  
{
  // Invoked after event service begins, after dispatch map has been examined

  const int sid = session? session->sessionID(): 0;

  if (session && (session->flags & MMS_SESSION_FLAG_TEARDOWN_IN_PROGRESS))
  {
      if (text) MMSLOG((LM_INFO,"POOL session %d event '%s' discarded\n",sid,text));
      return -1;
  }
     
  if (text) MMSLOG((LM_INFO,"POOL session %d op %d event is '%s'\n",sid,opid,text));
   
  if (reset) this->markWaiting(FALSE);

  return 0;
}



void MmsSession::Op::onServiceThreadExit()
{
  // Invoked in service thread context as thread goes idle

  this->svcThreadHandle = 0;
  MMSLOG((LM_DEBUG,"POOL svc thread %t is idle\n"));
}



void MmsSession::Op::onServiceReturn()     
{
  // Invoked on return from service to session manager. At this point either
  // the command has executed successfully, or an error was detected.
}

              

void MmsSession::Op::onCommandEnd(const int commandID)  
{
  // Operation close activities which can be done by the operation itself.
  // Invoked once a command result has been dispatched to client.
  // This should ordinarily be invoked via session.closeOperation, which
  // compresses the operation slot out of the session op table.

  this->idleResources();
}



// - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
// operation resources
// - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

                                             
MmsDeviceVoice* MmsSession::Op::voiceDevice(const int connect, const int acquire, 
  const ListenDirection listenDirection, const int capabilities)   
{
  // Returns the voice device object corresponding to the voice resource
  // handle. If the voice resource is not currently listening to the session
  // IP resource, that connection is made, unless requested otherwise. Parameter
  // listenDirection determines if the connection is full or half-duplex. If
  // the voice resource handle identifies a device not currently owned by this 
  // operation, we request another voice resource from the resource manager, 
  // unless instructed otherwise. 

  MmsDeviceVoice* device     = NULL;
  MmsDeviceIP*    deviceIP   = session->ipDevice();
  HmpResourceManager* resmgr = session->resourceManager();

  if  (isValidDeviceHandle(this->hvox))     // Do we already have one?  
       device = (MmsDeviceVoice*)resmgr->getDevice(this->hvox);                                            
                                            
  if  (device)                              // If it still belongs to us ...
  {    
       if  (device->subowner() == this->opid)
       {
            if  (device->isIdle())          // If we idled it ...
            {                               // remove it from idle pool
                 resmgr->resourcePoolUnidle(this->hvox, TRUE);
                 device->setBusy(this->sessionID(), this->opid);
            }  
       }
       else device = NULL;                  // Indicate need to reacquire                                   
  }
  else                                                                               
  if (!acquire)                             // If don't already have one, and 
       return NULL;                         // not asked to get one, we're done
  
 
  if  (!device && acquire)                  // If we don't have a voice resource
  {                                         // and were asked to get one, get one
       device = this->voiceResourceAcquire(capabilities);      
  }


  if  (device && connect)                   // If asked to connect the
  {                                         // vox with another device ...
       if  (deviceIP)                       // If this is an IP session ...
       {                                     // ... hook up vox to IP
            device->dataLock.acquire();     // 2-way listen is atomic

            const int direction = listenDirection == FULLDUPLEX? 
                      MmsMediaDevice::FULLDUPLEX: MmsMediaDevice::HALFDUPLEX;
            
            const int listenResult = deviceIP->busConnect(device, direction); 

            device->dataLock.release();

            if (listenResult == -1)                     
            {
                MMSLOG((LM_ERROR,"%s could not listen ip/vox\n", logkey));
                return NULL;
            }
             
       }     // A session with no IP resource is a utility session. We do not
       else; // listen a utility session's voice resource with its conference     
  }          // here -- that is instead done in the conference join

  return device;  
}

                                       

MmsDeviceVoice* MmsSession::Op::voiceResourceAcquire(const int capabilities)
{
  // Get an available voice resource. Depending on config settings, we may
  // block waiting for an available resource, so this should be executed
  // in a service thread context. Here we set this session as owner of the
  // resource, so that we can detect the situation where another session
  // appropriates the resource while idle.

  // Atomically check for out of vox, and reserve a vox if available LICX VOXOUT, VOX-
  const int isVoxResourcesExhausted = MmsAs::vox(MmsAs::RESX_ISZERO, MmsAs::RESX_RESERVE);

  if (isVoxResourcesExhausted)              // No more voice resources
      return this->raiseVoxExhaustedAlarm();      

  HmpResourceManager* resmgr = session->resourceManager();

  int grflags = 0;
  MmsDeviceVoice* deviceVoice = NULL;
  const int isCspRequest = (capabilities & MmsDeviceVoice::DEVICECAP_CSP) != 0;
  if (isCspRequest) grflags |= HmpResourceManager::GAD_CSP_CAPABLE;

  do {
      
  mmsDeviceHandle handle = resmgr->getResource(MmsMediaDevice::VOICE, grflags);
 
  if (isBadDeviceHandle(handle)) 
  {    
      MMSLOG((LM_ERROR,"%s op %d could not acquire vox resource\n",logkey,opid));
      break;   
  }  

  this->hvox = handle;

  deviceVoice = (MmsDeviceVoice*)resmgr->getDevice(handle); 
  if  (!deviceVoice) break;
                       
  deviceVoice->owner(sessionID(), opid); 
  deviceVoice->elapsedTimeReset();

  #if 0 // The received digits handler now clears the digit buffer
  deviceVoice->clearDigitBuffer();
  this->flags |= MMS_OP_FLAG_DIGITBUFFER_CLEARED;
  #endif

  this->adjustPlayPerConfig(deviceVoice);  
                    
  MMSLOG((LM_DEBUG,"%s op %d using vox%d\n",logkey,opid,handle)); 

  } while(0); 

  if (deviceVoice == NULL)                  // Un-reserve the unused license    
      MmsAs::vox(MmsAs::RESX_INC);          // LICX VOX+ (1 of 2)
                           
  return deviceVoice;
}



int MmsSession::Op::raiseG711ExhaustedAlarm()
{
   // Fire an alarm indicating G711 ports exhausted, also log the condition

  char alarmDescription[MmsAlarmParams::maxTextLength+1];
  sprintf(alarmDescription, MmsAs::szResxExhausted, "G.711 RTP");

  MMSLOG((LM_ERROR,"%s %s\n", logkey, alarmDescription));

  MmsAlarmParams* params = new MmsAlarmParams(MmsReporter::NDX_NO_MORE_CONNECTIONS, 
     MMS_STAT_CATEGORY_RTP, MmsReporter::severityTypeSevere);
  params->settext(alarmDescription);

  MmsReporter::instance()->postMessage(MMSM_ALARMREQUEST, (long)params);
  return MMS_ERROR_RESOURCE_UNAVAILABLE;
}



MmsDeviceVoice* MmsSession::Op::raiseVoxExhaustedAlarm()
{
  // Fire an alarm indicating voice resources exhausted, also log the condition

  char alarmDescription[MmsAlarmParams::maxTextLength+1];
  sprintf(alarmDescription, MmsAs::szResxExhausted, "vox");

  MMSLOG((LM_ERROR,"%s op %d %s\n",logkey,opid,alarmDescription));

  MmsAlarmParams* params = new MmsAlarmParams(MmsReporter::NDX_NO_MORE_VOX, 
     MMS_STAT_CATEGORY_VOX, MmsReporter::severityTypeSevere);
  params->settext(alarmDescription);

  MmsReporter::instance()->postMessage(MMSM_ALARMREQUEST, (long)params);

  return NULL;
}



MmsDeviceVoice* MmsSession::Op::assignedVoiceDevice()
{
  // Get voice resource currently assigned to the operation, if one exists
  return session ? (MmsDeviceVoice*)session->resourceManager()->getDevice(this->hvox) : NULL; 
}



void MmsSession::Op::idleResources()
{ 
  // Unlisten and idle the voice resource assigned to the session, if there
  // is one, and unlisten the IP resource 

  // Previous to implementing multiple operations per session, we did not release
  // resources on command end, but rather "idled" resources. The voice resource in 
  // use by the session was unlistened but remained with the session, such that if 
  // marked as owned by the session, or if no other session claimed it, the session 
  // would use the same voice resource for successive voice operations. With multiple
  // operations, not only is it not practical to retain multiple voice resources
  // with the session, but also we would not know which one of the possible multiple 
  // voice resources to use for the next voice operation. It is unclear at the moment
  // what side effects not retaining the voice resource might have. If we determine
  // that some combination of consecutive session operations must use the same voice
  // resource, we might need to mark a single resource as owned by the session, and
  // retain the single voice with the session, rather than the operation.

  this->releaseResources();

  #if(0)                                    // Pre-multiple-operations code:
  if  (isValidDeviceHandle(this->hvox))
       this->voiceResourceIdle();           // Unlistens ip as well
  else 
  if  (session->isInConference());          // Unless session is still in
  else                                      // conference, unlisten IP
  {    MmsDeviceIP* deviceIP = Session()->ipDevice();
       if (deviceIP && deviceIP->isListening() && !deviceIP->isSelfListening())                             
           deviceIP->unlisten();  
  }
  #endif         
} 

  

void MmsSession::Op::releaseResources()
{
  // Release resources associated with an operation. These include HMP resources
  // plus anything else allocated dynamically for the operation. 

  if (this->cmdid == COMMANDTYPE_MONITOR_CALL_STATE) 
  {   
      // When terminating command is monitorCallState, remove state object
      
      MmsCallStateMonitor* callStateMon = session? session->callStateMon: NULL;
      if (callStateMon)
      {   Session()->callStateMon = NULL;
          delete callStateMon;
      }
  }

  MmsTtsSessionData* ttsdata = this->ttsData;   
  this->ttsData = NULL;
  if (ttsdata) delete ttsdata;

  if (this->isStreaming()) this->stopVoiceRecOperation();

  this->voiceResourceRelease();
}

   

int MmsSession::Op::voiceResourceRelease()       
{
  // Return specified voice resource to available resource pool

  // As an alternative to making listen assumptions based on current command,
  // could we instead generalize unlistens by looking up the device for the 
  // timeslot a device is listening to, and if one exists, unlisten that device.

  MmsDeviceVoice* deviceVoice = this->assignedVoiceDevice();
  if (!deviceVoice) return 0;
  this->hvox = 0;

  // The voice device data lock mutex is acquired for atomic two-way voice/IP
  // listens, and during digit buffer write access, which includes sendDigits
  // and clearDigitBuffer. We acquire that lock here to ensure that we do not
  // release the device while one of these operations is in progress.

  ACE_Guard<ACE_Thread_Mutex> x(deviceVoice->dataLock);

  if  (deviceVoice->isListening()) 
       deviceVoice->unlisten();

  deviceVoice->setAvailable();

  MmsDeviceIP* deviceIP = isTwoWayListen(deviceVoice) && session? 
               session->ipDevice(): NULL;

  // If session is in conference then do not unlisten from IP,
  // since this would break the connection between IP and CONF devices
  if (deviceIP && deviceIP->isListening() && !session->isConferenceParty()) 
      deviceIP->unlisten();   

  if  (MmsVolumeSpeedEncoder::isDefaultVolumeSpeed(deviceVoice->volspeed));
  else deviceVoice->clearVolumeSpeedAdjustments();
  
  deviceVoice->reset();                     // Stop any media operations

  session->resourceManager()->resourcePoolAvailable(deviceVoice->handle());
  MmsAs::vox(MmsAs::RESX_INC);              // LICX VOX+ (2 of 2)

  MMSLOG((LM_DEBUG,"%s op %d vox%d released\n",logkey,opid,deviceVoice->handle()));
  return 0;
}



int MmsSession::Op::isTwoWayListen(MmsDeviceVoice* vox)
{
  // Determine if specified vox is likely listening full duplex to session IP
  if (vox->isIdle()) return FALSE;

  switch(cmdid)
  { case COMMANDTYPE_PLAY:
    case COMMANDTYPE_PLAYTONE:
    case COMMANDTYPE_MONITOR_CALL_STATE:
    case COMMANDTYPE_VOICEREC:
         return TRUE;
  } 
  return FALSE;
}



int MmsSession::Op::voiceResourceIdle()
{
  // Idle our voice resource. This may be done only when an operation is idle
  // See comments at idleResources(). We are currently not idling voice at
  // operation termination, but instead releasing it. 
 
  #if(0)
  MmsDeviceVoice* deviceVoice = this->assignedVoiceDevice();
  if  (!deviceVoice) return -1;

  if  (deviceVoice->isIdle()) return 0;  

  if  (deviceVoice->isListening()) 
       deviceVoice->unlisten(); 

  MmsDeviceIP* deviceIP = isTwoWayListen(deviceVoice)? session()->ipDevice(): NULL;

  if (deviceIP && deviceIP->isListening()) 
      deviceIP->unlisten();   

  // "owned" flag indicates the device can't be commandeered from its session.
  // "owner" indicates the session in which the device is currently resident.
  // We do not idle the device if we have marked it not commandeerable, since 
  // if we were to do so, some other session needing a vox device could grab it.
  // We should do a deviceVoice->setOwned(FALSE) as soon as we no longer need
  // exclusive use of the vox. The owned flag will however eventually get reset
  // when the session ends, if we fail to reset it explicitly before then.  
  if  (deviceVoice->isOwned() && deviceVoice->subowner() == this->opid);
  else this->resourceMgr->resourcePoolIdle(this->hvox, TRUE);
  #endif

  return 0;
}



int MmsSession::voiceResourceCount(const int needLock)
{
  // Return count of voice resources currently resident in session
  int count=0;
  
  if (needLock) this->optableLock.acquire();
  MmsSession::Op* op = &this->optable[0];

  for(int i=0; i < MMS_MAX_SESSION_OPERATIONS; i++, op++)    
      if (op->isBusy() && isValidDeviceHandle(op->hVoice())) count++;

  if (needLock) this->optableLock.release();
  return count;
}
  


int MmsSession::lockVoxAll(const int isLocking, const int needLock)
{
  // Acquire the voice device's data lock for all vox active in the session.
  // The voice device cannot be released while this lock is held.
  int count=0;
  
  if (needLock) this->optableLock.acquire();
  MmsSession::Op* op = &this->optable[0];

  for(int i=0; i < MMS_MAX_SESSION_OPERATIONS; i++, op++) 
  { 
      if (op->isBusy() && isValidDeviceHandle(op->hVoice())); else continue; 

      MmsDeviceVoice* voiceDevice = op->assignedVoiceDevice();
      if (!voiceDevice) continue; 
      count++;
  
      if (isLocking)
      {
          voiceDevice->dataLock.acquire();

          if (!isValidDeviceHandle(op->hVoice()))
          {   // Head off an unlikely but insidious race condition, where between
              // the time we accessed the voice device and acquired the lock, the
              // voice resource had been released.
              voiceDevice->dataLock.release();
              count--;
          }
      }
      else voiceDevice->dataLock.release();      
  }

  if (needLock) this->optableLock.release();
  return count;
}



// - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
// operation event registration
// - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -


int MmsSession::Op::registerForAsyncEventNotification(const int eventType, const int isIP) 
{
  // Registers with the event monitor to receive notification when an HMP event
  // on the specified device, and of of the specified event type, fires. We specify
  // that notification is via the session manager message queue. The registration is 
  // a one-shot, meaning that the the registration will expire when the event fires.

  mmsDeviceHandle deviceHandle = isIP? session->hIP: this->hvox;

  const int eventID = MmsEventRegistry::instance()->registerOneShotEvent
      (deviceHandle, eventType, this->sessionID(), 0, this->sessionManager());

  if  (eventID == -1)
       MMSLOG((LM_ERROR,"%s op %d event registration error\n", logkey, opid));
  else this->markWaiting(eventType, eventID);
          
  return eventID;
}



int MmsSession::Op::cancelAsyncEventNotification
( const int eventType, const int isIP, const int eventDisposition) 
{
  // Cancels a pending event. It is possible that the event has fired, a service
  // thread has it, and so the event will no longer exist in the event table and
  // the unregister will fail. The nolog parameter supresses this log error.

  if (!this->isWaiting()) return 0; 
  if (isIP && !session)   return 0;         

  mmsDeviceHandle deviceHandle = isIP? session->hIP: this->hvox;
  this->markWaiting(FALSE);                  
  this->state = LIVE;    

  const int result = MmsEventRegistry::instance()->unregister(deviceHandle, eventType);

  if  (result == -1 && eventDisposition != MMS_CANCEL_EVENT_NOLOG)
       MMSLOG((LM_ERROR,"%s op %d unregister failed for event %d\n", 
               logkey, opid, eventType));
      
  return result;
}



int MmsSession::Op::registerForRfc2833EventNotification() 
{
  const long rfc2833eventType = MmsDeviceIP::getRfc2833EventType();

  const int eventID = MmsEventRegistry::instance()->registerRecurringEvent
     (session->ipDevice()->handle(), rfc2833eventType, this->sessionID(), 0, 
      this->sessionManager());

  if (eventID == -1) MMSLOG((LM_ERROR,"RFC2833 event registration error\n"));
        
  return eventID;
}



int MmsSession::Op::cancelRfc2833EventNotification()
{
  const long rfc2833eventType = MmsDeviceIP::getRfc2833EventType();

  const int result = MmsEventRegistry::instance()->unregister
                    (session->ipDevice()->handle(), rfc2833eventType);

  if (result == -1) MMSLOG((LM_ERROR,"RFC2833 event unreg failed\n"));

  return result;
}