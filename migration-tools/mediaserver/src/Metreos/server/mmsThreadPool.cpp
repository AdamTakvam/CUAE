// 
// mmsThreadPool.cpp
//
// Manages the service thread pool
//
#include "StdAfx.h"
#include "mmsThreadPool.h"
#include "mmsCommandTypes.h"
#include "mmsParameterMap.h"
#include "mmsDeviceIP.h"

#ifdef MMS_ENABLE_MEMORY_LEAK_DETECTION
#define new MMS_MEMLEAK_NEW
#endif // MMS_ENABLE_MEMORY_LEAK_DETECTION

GENERICCALLBACK MmsThreadPool::policeDirectoriesCallback = NULL; 



int MmsThreadPool::handleMessage(MmsMsg* msg)             
{ 
  switch(msg->type())
  { 
    case MMSM_SERVICEPOOLTASK:
         this->onServicePoolTask(msg);
         break;

    case MMSM_SERVICEPOOLEVENT:
         this->onServicePoolEvent(msg);
         break;

    case MMSM_SERVICEPOOLTASKEX:
         this->onServicePoolTaskEx(msg);
         break;

    case MMSM_INITTASK:
         MMSLOG((LM_INFO,"%s service threads %d\n",taskName,threadCount));
         break;

    case MMSM_SHUTDOWN:
         this->isShutdownRequest = TRUE;
         this->postMessage(MMSM_QUIT);
         break;
    #if 0            
    case MMSM_QUIT:                         // Acknowledge all threads shut down                                            
         if  (this->isShutdownRequest && this->thr_count() <= 1)  
              m_serverManager->postMessage(MMSM_ACK, MMSM_SHUTDOWN);                                       
         break;
    #endif

    default: return 0;
  } 

  return 1;
}



int MmsThreadPool::onServicePoolTask(MmsMsg* msg)
{
  // A session task was placed on the thread pool message queue presumably by
  // the session manager, and this thread has dequeued it for handling. 
  // All work that comes through here is a new server command, represented by 
  // the session referenced in passed parameters. This session object has been 
  // preinitialized by the session manager with the server command parameter 
  // map, and in busy state. 
   
  MmsCurrentCommand* commandParams = (MmsCurrentCommand*)msg->param(); 
  MmsCurrentCommand params;
  commandParams->copy(params);
  delete commandParams;                     // Free memory from SMGR.onSessionTask()

  MmsSession* session = params.session;
  MmsSession::Op* operation = params.operation;
  if (!operation) return MMS_ERROR_SERVER_INTERNAL;
  operation->onServiceThreadEntry();
                                            // Session's service threads are serialized 
  const int isThreadSerialized              // by default but this can be overridden  
   = !isFlatmapFlagSet(params.flatmap, MmsServerCmdHeader::IS_CONCURRENT_SERVICE_THREAD);

  if (isThreadSerialized) session->threadPoolSerializer.acquire(); 
                              
  const int command   = params.command;             
  const int sessionID = session->sessionID(); 
  const int opID      = operation->opID();

  MMSLOG((LM_INFO,"POOL svc thread %t has session %d command %s\n", 
            sessionID, Mms::commandName(command)));
  int result = 0, cmdresult = 0;

  
  switch(command)
  {
    case COMMANDTYPE_CONNECT:
         cmdresult = operation->handleConnect(params);
         break;
                
    case COMMANDTYPE_DISCONNECT: 
         cmdresult = operation->handleDisconnect(params);
         break;
                         
    case COMMANDTYPE_PLAY: 
         cmdresult = operation->handlePlay(params);
         break;
              
    case COMMANDTYPE_RECORD: 
         cmdresult = operation->handleRecord(params);
         break;
              
    case COMMANDTYPE_RECORD_TRANSACTION:  
         cmdresult = operation->handleRecordTransaction(params);
         break;
              
    case COMMANDTYPE_PLAYTONE: 
         cmdresult = operation->handlePlaytone(params);
         break;
              
    case COMMANDTYPE_RECEIVE_DIGITS: 
         cmdresult = operation->handleReceiveDigits(params);
         break;

    case COMMANDTYPE_STOP_OPERATION: 
         cmdresult = operation->handleStopOperation(params);
         break;
              
    case COMMANDTYPE_ASSIGN_VOLADJ_DIGIT:   
    case COMMANDTYPE_ASSIGN_SPEEDADJ_DIGIT:  
    case COMMANDTYPE_ADJUST_VOLUME:   
    case COMMANDTYPE_ADJUST_SPEED: 
    case COMMANDTYPE_CLEAR_VS_ADJUSTMENTS: 
         cmdresult = operation->handleAdjustments(params);
         break; 

    case COMMANDTYPE_MONITOR_CALL_STATE: 
         cmdresult = operation->handleMonitorCallState(params);
         break;     
              
    case COMMANDTYPE_CONFERENCE_RESOURCES: 
         cmdresult = operation->handleConferenceResourcesRemaining(params);
         break;  

    case COMMANDTYPE_CONFEREE_SETATTRIBUTE: 
         cmdresult = operation->handleConfereeSetAttribute(params);  
         break;                                  
              
    case COMMANDTYPE_CONFEREE_ENABLE_VOL:  
         cmdresult = operation->handleConferenceEnableVolumeControl(params);
         break;

    case COMMANDTYPE_VOICEREC: 
         cmdresult = operation->handleVoiceRec(params);
         break;

    case -1:
         cmdresult = MMS_ERROR_SERVER_INTERNAL;
         break;

    default: 
         cmdresult = MMS_ERROR_NO_SUCH_COMMAND;
  }


  operation->onServiceThreadExit();

  if (operation->opID() != opID)            // Timeout in debugger perhaps
      return releaseAndReturn(MMS_ERROR_SERVER_INTERNAL, 
             isThreadSerialized, session->threadPoolSerializer);
                                

  if  (cmdresult == MMS_COMMAND_EXECUTING)      
  {   
       // Media command is executing asynchronously, so we go wait for the 
       // termination event (see exception following). There is however the 
       // chance that the termination event has already fired, in situations 
       // where both the command is of short duration, and there are many 
       // threads competing. In such a situation, without a mechanism to force 
       // the event handler to wait for this code to execute, the command could 
       // have completed and its map destroyed, before we execute this code, 
       // which expects viable command state. We prevent the race condition by 
       // detecting in the event handler that dependency logic has not completed,
       // and subsequently waiting for it to do so, in onServicePoolEvent below. 
       // Note that since the above writing, other improvements have been made
       // such that this is usually observed only on async connect commands.

       // handleReceiveDigits can return MMS_COMMAND_EXECUTING (1), when in fact
       // the termination has occurred already and no async operation is executing.
       // We do this so that the provisional and final results for the command
       // can be sent at the same time.

       const int isProvisoRequired  = IS_ASYNC_VOICE_COMMAND(command);
       const int isTermConditionMet = operation->isHmpTerminationMet();

       if  (isTermConditionMet)
            MMSLOG((LM_INFO,"POOL session %d op %d term condition preexists\n",
                    sessionID, opID)); 
       else
       if  (isProvisoRequired)
            MMSLOG((LM_INFO,"POOL session %d op %d media started async\n",
                    sessionID, opID));
       else MMSLOG((LM_INFO,"POOL session %d op %d started async\n",
                    sessionID, opID));
                                            // Sanity check  
       if (!Mms::isFlatmapReferenced(operation->flatmap(), 4, sessionID, opID)) 
           return releaseAndReturn(MMS_ERROR_SERVER_INTERNAL, 
                       isThreadSerialized, session->threadPoolSerializer);
                                             
       m_sessionManager->logResourceState();
                                                    
       if (!isProvisoRequired)              // If no provisional is expected ...
       {                                    // permit term event to proceed now
           clearFlatmapXflag(operation->flatmap(),
                 MmsServerCmdHeader::IS_DEPENDENCY_PENDING);
                                            // ... and end critical section
           session->markHandlingCommand(FALSE);
       }
                                            // Post provisional OK message  
       result = isProvisoRequired? this->postProvisionalReturn(operation): 0;
  }   
  else
  if  (cmdresult == MMS_COMMAND_WAITING)        
  {                
       // The command is waiting for some event other than a media termination,
       // such as a call state transition (silence on/off events). 
       MMSLOG((LM_INFO,"POOL session %d wait for transition\n",sessionID));
                                            // Post provisional OK message 
       result = this->postProvisionalReturn(operation);
  }
  else                                      
  if  (MMS_ISCOMMANDERROR(cmdresult))       // If session event handler error
                                            // ... return error to session mgr
       result = postErrorReturn (operation, cmdresult);
                                            // Done: return OK to session mgr  
  else result = postNormalReturn(operation);    

  return releaseAndReturn(result, isThreadSerialized, session->threadPoolSerializer);
} 


     
int MmsThreadPool::onServicePoolEvent(MmsMsg* msg)       
{
  // A media event was placed on the thread pool message queue presumably by
  // the session manager, and this thread has dequeued it for handling. 
  // All work that comes through here is thus a continuation of an existing 
  // server command. The dispatch map coming in as msg->param() is allocated 
  // by MmsEventRegistry::hmpEventHandler, which abdicates responsibility for 
  // freeing that memory to us. 

  MmsSession* session = NULL;
  MmsSessionPool* sessionPool = m_sessionManager->sessionPool();

  MmsEventRegistry::DispatchMap* dispatchMap = 
 (MmsEventRegistry::DispatchMap*)msg->param();

  if (dispatchMap)  
      session = sessionPool->findBySessionID(dispatchMap->sessionID); 
  if (session == NULL) return 0;

  // We normally serialize service thread activity on a session such that no 
  // more than one service thread is active on the session at a time. A pool
  // task can be specified as concurrent, but event handlers can never execute
  // concurrently with other event handlers, or with serialized tasks.
  ACE_Guard<ACE_Thread_Mutex> x(session->threadPoolSerializer);

  const int  eventID   = dispatchMap->eventID;
  const long eventType = dispatchMap->eventType;
  const int  sessionID = dispatchMap->sessionID; 

  if (eventType == MmsDeviceIP::getRfc2833EventType())
  {
    const int result = this->handleRfc2833SignalEvent(session, dispatchMap);
    delete dispatchMap;
    return result;
  }

  // Look up the operation which is waiting on this event
  MmsSession::Op* operation = session->findByEventID(eventID);

  if (operation == NULL)
  {
      MMSLOG((LM_INFO,"POOL session %d unregistered event %d-%d discarded\n", 
              sessionID, eventType, eventID));
      return MMS_ERROR_EVENT_UNKNOWN;
  }

  const int operationID = operation->opID();

  // We first ensure that the command thread launching the async event has run      
  // its course, in order to further ensure that the command does not complete 
  // and free its memory while outstanding logic still expects it to be viable.

  this->waitForDependency(operation);

  // We don't want to begin handling an event during periodic policing     
  // of the session pool, so we acquire the session pool lock before we begin 
  ACE_Guard<ACE_Thread_Mutex> y(m_sessionManager->sessionPoolLock());

  // Detect when this session or operation was timed out during pool policing
  if (operation->opID() != operationID)
  {
      MMSLOG((LM_INFO,"POOL session %d event %d-%d on closed op %d discarded\n", 
              sessionID, eventType, eventID, operationID));
      return MMS_ERROR_EVENT_UNKNOWN;
  }

  // When a server disconnect interrupts a play/record conference, the resulting
  // teardown runs through common code in conferenceManager.leaveConference().
  // That code cancels the media operation but not the event, since the code is
  // general and in most cases the event is desired. However here, the event has
  // fired after the disconnect has completed. Since we do not run this case
  // through special case code, we'll catch the condition here and note it in 
  // the log. Coding a special case for the condition for the express purpose
  // of canceling the event before the fact would warrant careful design, and   
  // is likely unnecessary in any case. 
  int  result = 0, hmpError = 0, opID = operation->opID();

  if (!Mms::isFlatmapReferenced(operation->flatmap(), 26, sessionID, opID))   
  {   
      MMSLOG((LM_INFO,"POOL session %d event %d-%d discarded\n", 
              sessionID, eventType, eventID));
      return MMS_ERROR_SERVER_INTERNAL;
  }    

  operation->onServiceThreadEntry();        // State transition   
  operation->onEventSink();   
  dispatchMap->operationID = opID;                                      

  MMSLOG((LM_DEBUG,"POOL svc thread %t has session %d event %d-%d\n", 
          sessionID, eventType, eventID));  
                                            // Was event an HMP error notification?
  if  (dispatchMap->flags & dispatchMap->EVENT_ERROR)
       hmpError = this->getHmpAsyncEventError(operation, dispatchMap);   
                                            // Handle the event
  result = this->handleEvent(operation, dispatchMap);

  delete dispatchMap;                       // Done: clean up and return result: 
  operation->onServiceThreadExit(); 

  switch(result)                            
  { case MMS_COMMAND_EXECUTING:             // If more events expected ...
    case MMS_COMMAND_WAITING:            
    case MMS_ERROR_EVENT_UNKNOWN: return 0; // ... continue waiting
  }   
     
  operation->markWaiting(FALSE);            // Indicate no longer waiting

  const int errcode = MMS_ISCOMMANDERROR(result)? result: hmpError? hmpError: 0;

  if  (errcode)
       return postErrorReturn (operation, errcode);
  else return postNormalReturn(operation);                                            
} 



int MmsThreadPool::handleRfc2833SignalEvent(MmsSession* session, MmsEventRegistry::DispatchMap* dispatchMap)
{
  if (session == NULL) return MMS_ERROR_SERVER_INTERNAL;
  session->flags |= MMS_SESSION_FLAG_EVENT_FIRED;

  const int result = session->handleRfc2833SignalReceived(dispatchMap, this->parent);

  session->flags |= MMS_SESSION_FLAG_EVENT_HANDLER_DONE;  
  session->flags &= 
    ~(MMS_SESSION_FLAG_EVENT_FIRED | MMS_SESSION_FLAG_HANDLING_EVENT);  
 
  return result;
}



int MmsThreadPool::handleEvent
( MmsSession::Op* operation, MmsEventRegistry::DispatchMap* dispatchMap)
{
  MmsSession* session = operation->Session(); 
  if (session == NULL) return MMS_ERROR_SERVER_INTERNAL;
  session->flags |= MMS_SESSION_FLAG_EVENT_FIRED;
  int result = 0;

  switch(dispatchMap->eventType)
  {
    case TDX_PLAY: 
         if (operation->isAsrChannelActive())
         {
           result = operation->handleEventVoiceRecPromptDone(dispatchMap);
           break;
         }
    //case 0xE0 /*TEC_STREAM*/:
    case TDX_RECORD:
    case TDX_PLAYTONE:
         result = operation->handleEventVoice(dispatchMap);
         break;

    case TDX_GETDIG:                        // receiveDigits completed
         result = operation->handleEventVoxDigitsReceived(dispatchMap);
         break;

    case TDX_CST:
         result = operation->handleEventCallStateTransition(dispatchMap);
         break;

    case IPMEV_DIGITS_RECEIVED:             // One event per digit
         result = operation->handleEventIpDigitsReceived(dispatchMap);
         break;

    case IPMEV_STARTMEDIA:
         result = operation->handleEventStartMedia(dispatchMap);
         break; 

    case IPMEV_STOPPED:                     // Is this async?
         result = operation->handleEventIpStopped(dispatchMap);
         break;

    #if 0
    case IPMEV_EVENT_DISABLED:              // These are either executed
    case IPMEV_EVENT_ENABLED:               // synchronously, or are
    case IPMEV_GET_LOCAL_MEDIA_INFO:        // not implemented.
    case IPMEV_GET_SESSION_INFO:
    case IPMEV_GET_XMITTS_INFO:
    case IPMEV_LISTEN:
    case IPMEV_OPEN:
    case IPMEV_UNLISTEN:
    case IPMEV_ERROR:                       // These we already looked up and  
    case TDX_ERROR:                         // returned the registered event 
    #endif
    default: 
         MMSLOG((LM_INFO,"POOL ignoring unsupported event %d-%d\n",
                 dispatchMap->eventType, dispatchMap->sessionID));
  }
                                            // State is now out of handler
  session->flags |= MMS_SESSION_FLAG_EVENT_HANDLER_DONE;  
  session->flags &= 
    ~(MMS_SESSION_FLAG_EVENT_FIRED | MMS_SESSION_FLAG_HANDLING_EVENT);  
 
  return result;
}



int MmsThreadPool::waitForDependency(MmsSession::Op* operation)
{
  // It is possible for a command to initiate an async command, and for the
  // completion event to fire and be returned to client, before the thread
  // directing the async command's returned result regains control. This
  // would result in the adapter freeing the parameter map memory while the
  // command thread still expects to access it. Therefore, when a termination
  // event fires, we check to see if the dependency result has been returned
  // to client. If not, we wait a configurable period of time for the logic
  // on that thread to complete. Since this is a condition which very rarely 
  // occurs, we use a simple countdown timer, with a sleep to force a context
  // switch, in place of the more elegant semaphore with timeout.

  // Note that the session parameter map's dependency complete flag is set  
  // in MmsSession::OnCommandEnter(), and reset either in MmsMqAppAdapter::
  // PostClientReturnMessage(), for provisional results, or here in   
  // this.onServicePoolTask(), when no provisional is generated.  

  char* map = NULL;
  if (NULL != operation) map = operation->flatmap();
  if (NULL == map) return NULL;

  const unsigned int xflags = getFlatmapXflags(map);
  int isDependencyIncomplete 
    = (xflags & MmsServerCmdHeader::IS_DEPENDENCY_PENDING) != 0; 
                                             
  if(!isDependencyIncomplete) return 0;     // Normal and expected case
                                            // Unusual case: set a countdown timer
  const int waitMs = operation->Config()->serverParams.eventWaitForDependencyMsecs;
  MmsManualTimer manualTimer(waitMs);
  MmsSession* session = operation->Session();
  const int sessionID = session? session->sessionID(): 0;
  int isWaitTimeExceeded = 0;

  MMSLOG((LM_DEBUG,"POOL session %d event waiting for dependency\n", sessionID));

  while(1)                                  // Wait for flag to reset:
  {
    mmsSleep(MMS_N_MS(MMS_POLL_DEPENDENCY_WAIT_EVERY_MS));

    isDependencyIncomplete                  // Re-check flag  
      = isFlatmapXflagSet(map, MmsServerCmdHeader::IS_DEPENDENCY_PENDING);
    
    if (!isDependencyIncomplete) break;

    isWaitTimeExceeded = (manualTimer.countdown() == -1);
    if (isWaitTimeExceeded) break;
  }
          
  // In the unlikely event that after waiting the proscribed time, the 
  // dependency thread has still not run its course, we permit the event to
  // proceed. This is failsafe logic, since we wait up to two seconds by 
  // default. However server allows for this eventuality (OnServicePoolEvent), 
  // and as such should not be compromised, although log will show an invalid 
  // map reference. Client state could possibly become confused of course.
   
  if (isWaitTimeExceeded)          
      MMSLOG((LM_ERROR,"POOL session %d %d dependency did not complete\n",
              sessionID, operation->opID()));

  return isWaitTimeExceeded? -1:   0;   
}       



int MmsThreadPool::onServicePoolTaskEx(MmsMsg* msg)
{
  // A system or extra-session task was placed on the thread pool message queue 
  // by the session manager, and this thread has dequeued it for handling. 

  MmsSessionManager::SuperTaskParams* params =
 (MmsSessionManager::SuperTaskParams*)msg->param();
  ACE_ASSERT(params && params->isvalid()); 
  MMSLOG((LM_INFO,"%s svc thread %t has system task %d\n",
            taskName, params->sessionManagerTask));

  switch(params->sessionManagerTask)
  {
    case MmsSessionManager::SuperTaskParams::ABANDON_SESSIONS:
    case MmsSessionManager::SuperTaskParams::ABANDON_CONFERENCE:

         params->result = m_sessionManager->onAbandonSessions
                         (params->client, params->handle, params->conferenceID);
         break;

    case MmsSessionManager::SuperTaskParams::STOPMEDIA:
    { 
         MmsSession* session = params->session; 

         MmsStopMediaParams smp(params->flatmap);
         session->queryStopMediaParameters(smp); 
                                            // Stop all media ops except this
         if  (smp.operationID == 0)         // Result is 0; tag contains count
              params->tag = session->stopAllOperations
             (m_serverManager, params->operationID);
                                            // Stop one media operation
         else params->result = session->stopOperation
             (smp.operationID, m_serverManager, !smp.isSynchronous, TRUE); 
    }
         break;

    case MmsSessionManager::SuperTaskParams::RECONNECT:
        
         params->result = params->session->reconnectOutOfBand(params->operationID);
         break;

    case MmsSessionManager::SuperTaskParams::ADJUSTPLAY:
        
         params->result = params->session->adjustPlay(params->flatmap);
         break;

    case MmsSessionManager::SuperTaskParams::VOICEREC_START_COMPTHREAD:
    {
         MmsSession* session = params->session; 

         MmsSession::Op* op = session->findByOpID(params->operationID);
         params->result = op? op->startComputation(): -1;
    }
         break;

    case MmsSessionManager::SuperTaskParams::POLICE_DIRECTORIES:
         // Call back into server manager to execute directory cleanup on this thread
         MmsThreadPool::policeDirectoriesCallback(0);
         break;
  }

  m_sessionManager->postMessage(MMSM_SERVICEPOOLTASKEX_RETURN, (long)params);

  MMSLOG((LM_DEBUG,"POOL svc thread %t is idle\n"));
  return 0;
} 



int MmsThreadPool::getHmpAsyncEventError
( MmsSession::Op* operation, MmsEventRegistry::DispatchMap* dispatchMap)
{
  // Checks our event manager's event dispatch map for an error set by HMP during  
  // the async HMP action, and sets our error and reason codes accordingly. 
  int errorReturn = 0;

  if (dispatchMap->flags & dispatchMap->DATALENGTH_IS_ERRORCODE)
  { 
      const int hmpError = dispatchMap->returnDataLength;
      operation->putMapHeader(setReasonCode, hmpError);

      switch(hmpError)
      {
        case 1:            // ESR_SCAN   (mismatched audio file types)
        case 2:            // ESR_PARMID (bad audio parameter)
             errorReturn = MMS_ERROR_PARAMETER_VALUE;
             this->handleLowBitrateErrors(operation, dispatchMap->eventType, hmpError);  
             break;
        case 3:            // ESR_TMOUT  (srl function timeout)
        default:                     
             errorReturn = MMS_ERROR_ASYNC_EVENT; 
             break;
      }
  }
  else errorReturn = MMS_ERROR_ASYNC_EVENT; 

  return errorReturn;
}



int MmsThreadPool::handleLowBitrateErrors
( MmsSession::Op* operation, const int eventType, const int hmpError)
{
  int result = 0;

  if (IS_IP_EVENT(eventType))        
  {
      // HMP does not deal well with the case of requesting a low bitrate coder
      // when no license for same is present. HMP error is 'Invalid parameter'.
      // Another symptom of same is a crash on MMS exit in thread library code.
      // We should investigate with Dialogic: does HMP see a thread close exception
      // in Windows on HMP shutdown when unlicensed low bitrate coder is specified.

      const unsigned int flags = operation->Session()->Flags();

      if (flags & MMS_SESSION_FLAG_G729_RESOURCE_SPENT)
      {   MMSLOG((LM_ERROR,"POOL IP device error cause possibly G729\n"));
          result = 1;
      }
  }

  return 0;
}



int MmsThreadPool::onThreadStarted()        // Thread startup hook
{ 
  MMSLOG((LM_INFO,"POOL thread %t started at priority %d\n", osPriority)); 
  return 0;
} 



int MmsThreadPool::close(unsigned long)     // Thread exit hook
{                                    
  MMSLOG((LM_DEBUG,"POOL thread %t exit\n"));
  return 0;
}


                                            // Ctor
MmsThreadPool::MmsThreadPool(MmsTask::InitialParams* params): 
  MmsBasicTask(params), m_sessionManager(0)
{
  ACE_ASSERT(params->config && params->parent && params->user);
  this->config = (MmsConfig*)params->config;
  m_serverManager = this->parent;
  m_resourceManager = (HmpResourceManager*)params->user;
  this->isShutdownRequest = 0;
}



int MmsThreadPool::releaseAndReturn(const int rc, const int locked, ACE_Thread_Mutex& m)
{
  // Return from method after releasing lock
  if (locked) m.release();
  return rc;
}


                                            // Return OK to session manager
int MmsThreadPool::postNormalReturn(MmsSession::Op* op)
{       
  return m_sessionManager->postMessage(MMSM_SERVICEPOOLTASK_RETURN, 
     (long) new MmsSessionManager::TaskResultParams(op->Session(), op->opID()));
}


                                            // Return OK/executing to SMGR
int MmsThreadPool::postProvisionalReturn(MmsSession::Op* op)
{ 
  op->putMapHeader(setRetcode, MMS_COMMAND_EXECUTING); 

  return m_sessionManager->postMessage(MMSM_SERVICEPOOLTASK_RETURN, 
     (long) new MmsSessionManager::TaskResultParams(op->Session(), op->opID()));
}


                                            // Return error to session manager
int MmsThreadPool::postErrorReturn(MmsSession::Op* op, int retcode)
{
  op->putMapHeader(setRetcode, retcode); 
  op->putMapHeader(setFlag, MmsServerCmdHeader::IS_ERROR); 

  return m_sessionManager->postMessage(MMSM_SERVICEPOOLTASK_RETURN, 
     (long) new MmsSessionManager::TaskResultParams(op->Session(), op->opID()));
}


