// 
// mmsSessionManager.cpp
//
// Controls server command assignment to session objects
//
#include "StdAfx.h"
#pragma warning(disable:4786)
#include "mmsSessionManager.h"
#include "mmsServerCmdHeader.h"
#include "mmsParameterMap.h"
#include "mmsCommandTypes.h"
#include "mmsReporter.h"
#include "mmsAsr.h"

#ifdef MMS_ENABLE_MEMORY_LEAK_DETECTION
#define new MMS_MEMLEAK_NEW
#endif // MMS_ENABLE_MEMORY_LEAK_DETECTION

enum DECISIONBITS{CONX_INITIAL=1, CONX_REMOTE=2, CONX_EXISTING=4, NOSTARTMEDIA=8,
     CONFCREATE=16, CONFJOIN=32, ERR_TFP=1024, ERR_ALREADY=2048};



int MmsSessionManager::handleMessage(MmsMsg* msg)             
{ 
  switch(msg->type())
  {        
    case MMSM_TIMER:
         this->onTimer(msg);
         break;
                                 
    case MMSM_SESSIONTASK:                  // Work assignment from server mgr 
         this->onSessionTask(msg);
         break;

    case MMSM_MEDIAEVENT:                   // Notification of async 
         this->onMediaEvent(msg);           // media event firing
         break;

    case MMSM_SERVICEPOOLTASK_RETURN:       // Pool task complete or error 
         this->onServicePoolTaskReturn(msg);  
         break;

    case MMSM_SERVICEPOOLTASKEX_RETURN:     // Pool task complete or error
         this->onServicePoolTaskExReturn(msg);
         break;

    case MMSM_NOTIFY:                       // Pulse from server mgr
         this->onNotify(msg);
         break;

    case MMSM_CSPDATAREADY:                 // Notification of CSP data data ready
         this->onCspDataReady(msg);
         break;

    case MMSM_START_COMPUTATION_THREAD:     // Start VR computation thread
         this->onStartComputationThread(msg);
         break;

    case MMSM_INITTASK:
         this->onInitTask();
         break;

    case MMSM_SHUTDOWN:                     // Shutdown request from server mgr
         m_isShutdownRequest = TRUE;
         this->postMessage(MMSM_QUIT);
         break; 

    default: return 0;
  } 

  return 1;
}


                                             
int MmsSessionManager::onSessionTask(MmsMsg* msg)
{
  // Message contains a server command packaged as a parameter flatmap.
  // We'll prevalidate the parameter map, find or designate a session object,
  // and place the session on the service thread pool's work queue.

  char* params = (char*)msg->param();
  if  (params && isServerCmdHeader(params));
  else return -1;   

  int  sessionaction = EXISTING_SESSION;     
                                            // Prevalidate parameters
  int  result = prevalidateServerCommand(params, &sessionaction);

  if  (result)                              // Bogus parameter mix?
       return postErrorReturn(params, (result < ERR_TFP? result:
              result == ERR_ALREADY? MMS_ERROR_ALREADY_CONNECTED: 
              MMS_ERROR_TOO_FEW_PARAMETERS)); 

  MmsSession* session = NULL;
  MmsSession::Op* operation = NULL;
  const int connectionID = getFlatmapConnectionID(params);
  const int commandID  = getFlatmapCommand(params);  
  const void* clientID = getFlatmapClientHandle(params); 
  int  operationID     = getFlatmapOperationID(params); 
  int  isConcurrentOK  = 0, canceledOperationCount = 0;                                        
   

  switch(sessionaction)                        
  { 
    case NEW_SESSION:
    {               
         // Command requests either an IP connection, or a temporary,  
         // non-IP session; so assign the new session
                                               
         if (NULL == (session = m_sessionPool->findAvailableSessionEx(params)))  
             return postErrorReturn(params, MMS_ERROR_SERVER_BUSY);

         ACE_Guard<ACE_Thread_Mutex> x1(session->sessionDataLock);

         // Attempt to ensure we do not complete a connection for a client
         // subsequent to client requesting disconnect from media server  
         if (this->isClientDisconnecting(params)) return -1;  

         session->markHandlingCommand();                                              
                                             
         if (-1  == m_sessionPool->bindSessionToConnection(session, clientID))
             return postErrorReturn(params, MMS_ERROR_SERVER_INTERNAL);
                                            // Record new connection ID 
         setFlatmapConnectionID(params, session->connectionID());  
    }       
         break;

    case MULTI_SESSION:

         // The only multisession command is that which requests disconnect 
         // of all sessions in a conference. We handle this elsewhere.

         return this->onAbandonConference(params);
   
    case EXISTING_SESSION:
    default: 
    {
         // Command addresses an existing session, so look up the session         
         if  (NULL == (session = this->getSessionByID(connectionID)))
              return postErrorReturn(params, MMS_ERROR_NO_SUCH_CONNECTION); 

         ACE_Guard<ACE_Thread_Mutex> x(session->sessionDataLock);

         if  (m_config->diagnostics.flags & MMS_DIAG_LOG_OPTABLE)   
              session->dumpOpTable();

         if  (operationID && (0 == (operation = session->findByOpID(operationID))))
              return postErrorReturn(params, MMS_ERROR_NO_SUCH_OPERATION); 

         // If we're currently disconnecting this session, ensure we can't
         // identify an ensuing disconnect command as a barging disconnect

         if  (session->isDisconnecting()) 
              return postErrorReturn(params, MMS_ERROR_CONNECTION_BUSY);

         if (!isGoodConnectionState(session, commandID))
              return postErrorReturn(params, MMS_ERROR_NOT_CONNECTED, operation); 

         // If this is a session or server disco and session manager has not 
         // yet completed the first leg of prior command (provisional not yet
         // sent), cache the disco and continue without returning a result.

         if  (isBusyDisconnect(params, session)) return 0;    

         // Determine if it is OK for this command to operate over a busy session.
         // IS_SESSION_BREAK_IN was set for send digits or stop media.
         // IS_MODIFY_CONNECT was set at adapter when modifying an existing
         // connection, which may or may not be busy with a media operation.

         const int isOutOfBandOperation = isFlatmapFlagSet   
             (params, MmsServerCmdHeader::IS_SESSION_BREAK_IN); 

         const int isBargingReconnect = isFlatmapFlagSet    
             (params, MmsServerCmdHeader::IS_MODIFY_CONNECT);  

         // Do not permit a command to begin on this session while a prior
         // command has not yet returned its result (provisional if async,
         // final if sync) to client, unless this command is extra-session. 
         // If it looks as if a provisional is likely ready to complete in 
         // another thread, we yield this thread to permit it to do so.
         // This avoids most potential session busy conditions when commands
         // on a session do not wait for prior results and arrive very close. 
                                     
         if  (session->isHandlingCommand() && !isOutOfBandOperation)            
         {    mmsYield();                   // Still busy?  
              if (session->isHandlingCommand())
                  return this->postConnectionBusyReturn(session, params); 
         }

         session->markHandlingCommand();    

         // This barrier prevents a session and any of its pending operations 
         // from terminating during the brief period in which we are examining
         // the session for busy state, and possibly then executing a barging
         // command on the session. It blocks progress both at handleEventVoice 
         // and at handleEventVoxDigitsReceived. If this command is not such
         // an operation, the lock is removed immediately, below, If it is such
         // an operation, the lock is reset upon return from the operation,
         // at either doExternalOperation or onServicePoolTaskExReturn.

         session->suspendTerminationLock.acquire(); 

         if (isOutOfBandOperation)          // SendDigits or StopMediaOperation
             return this->executeCommandInline(params, session); 

         // A session disconnect arriving during an ongoing async operation  
         // barges the async media operation, canceling the media operation.

         const int isBargingDisconnect = this->isBargeDisconnect(params, session);

         if (isBargingDisconnect)            
             canceledOperationCount = this->bargeDisconnect(params, session);
          
         // Session is active if it has ongoing async media operations 
         const int isActive = session->isBusy(); 

         // If this command is a barging disconnect, isActive will be false, 
         // and we have just executed the barge, canceling all current operations, 
         // and returning final results to client for each. When this is the case, 
         // the disconnect command will now proceed normally.

         // If command is a reconnect, changing connection and media parameters,
         // and the session is active, execute the reconnect inline and return.
         if (isActive && isBargingReconnect)                              
             return this->executeCommandInline(params, session); 

         // If session is active, determine if the incoming command can execute
         // concurrently with the existing operations executing for the session
         if (isActive) 
             isConcurrentOK = this->isConcurrentOperationOK(commandID, session, operation);

         session->suspendTerminationLock.release();

         if (isActive && !isConcurrentOK)   // If busy, return a busy error
             return this->postConnectionBusyReturn(session, params); 

    }    // case EXISTING_SESSION:     
  }  
          
  operationID = session->onCommandStart(params, (void**)&operation);

  if (!isValidOperationID(operationID)) return MMS_ERROR_RESOURCE_UNAVAILABLE;

                                            // Delegate to a service thread    
  return m_threadPool->postMessage(MMSM_SERVICEPOOLTASK, 
           (long)this->currentCommand(commandID, session, operation, params));
}


                                        
int MmsSessionManager::prevalidateServerCommand(char* flatmap, int* sessionaction)
{
  // Briefly examines parameter map to determine if command is a connect,
  // or disconnect, and if so, whether command addresses the session,
  // conference, or both. 
  //
  // If command is disconnect and conference ID is supplied, action is
  // assumed to be disconnect from conference only; otherwise action is
  // disconnect IP, and also from conference if IP is in conference.
  //
  // If command is connect, connect action is interpreted according
  // to the following decision table. "Z" = zero, "-" = not specified,
  // "+" = nonzero. We make no distinction between connection ID zero
  // and connection ID not specified.
  
  // ##  ConxID  ConfID  Port  Action
  // === ======  ======  ====  =======================================
  //  0.    Z       -      -   ERR: tfp
  //  1.    Z       -      Z   InitialConx; nostart; noconf
  //  2.    Z       -      +   InitialConx; start;   noconf
  //
  //  3.    Z       Z      -   ERR: tfp (must startIP to create conf)            
  //  4.    Z       Z      Z   ERR: tfp (must startIP to create conf)      
  //  5.    Z       Z      +   InitialConx; start; createconf
  //
  //  6.    Z       +      -   ERR: tfp (must startIP to join conf)
  //  7.    Z       +      Z   ERR: tfp (must startIP to join conf)
  //  8.    Z       +      +   Initial conx; start; joinconf
  //
  //  9.    +       -      -   ERR: already connected
  // 10.    +       -      Z   ERR: already connected
  // 11.    +       -      +   RemoteConx, start, noconf
  //
  // 12.    +       Z      -   ExistingConx, createconf;
  // 13.    +       Z      Z   ERR: tfp (port must be - or +)
  // 14.    +       Z      +   RemoteConx; start; createconf; 
  //
  // 15.    +       +      -   ExistingConx; joinconf;
  // 16.    +       +      Z   ERR: already connected
  // 17.    +       +      +   RemoteConx, start, joinconf

  static unsigned int connectDecisionTable[] =
  {  
    ERR_TFP,                                // 0
    CONX_INITIAL | NOSTARTMEDIA,            // 1
    CONX_INITIAL,                           // 2
                                              
    ERR_TFP,                                // 3
    ERR_TFP,                                // 4
    CONX_INITIAL | CONFCREATE,              // 5

    ERR_TFP,                                // 6
    ERR_TFP,                                // 7
    CONX_INITIAL | CONFJOIN,                // 8

    ERR_ALREADY,                            // 9
    ERR_ALREADY,                            // 10
    CONX_REMOTE,                            // 11

    CONX_EXISTING| NOSTARTMEDIA| CONFCREATE,// 12
    ERR_TFP,                                // 13
    CONX_REMOTE  | CONFCREATE,              // 14

    CONX_EXISTING| NOSTARTMEDIA| CONFJOIN,  // 15
    ERR_ALREADY,                            // 16
    CONX_REMOTE  | CONFJOIN                 // 17
  };

  const int commandID = getFlatmapCommand(flatmap);

  switch(commandID)                    
  { case COMMANDTYPE_CONNECT:
    case COMMANDTYPE_DISCONNECT:            // We'll examine these below  
         break;

    case COMMANDTYPE_PLAY:                  // We'll check these further  
    case COMMANDTYPE_RECORD:                // for "play to conference"
    case COMMANDTYPE_PLAYTONE:
         break;  
 
    case COMMANDTYPE_STOP_OPERATION:        // If command operates on already
    case COMMANDTYPE_SEND_DIGITS:           // active session, mark it so ...
    case COMMANDTYPE_ADJUST_PLAY:
         setFlatmapFlag(flatmap, MmsServerCmdHeader::IS_SESSION_BREAK_IN);  
         return 0;

    default: return 0;
  }

  MmsFlatMapReader map(flatmap);
  int   conferenceID = 0, port = 0, isReconnect = 0, index = 0, decision = 0;
  int   connectionID = getFlatmapConnectionID(flatmap);
  char* pconfID=0, *pport = 0;

  map.find(MMSP_CONFERENCE_ID, &pconfID);
  if  (pconfID) conferenceID = *((int*)pconfID);


  switch(commandID) 
  { 
    case COMMANDTYPE_CONNECT: 

         map.find(MMSP_PORT_NUMBER, &pport);
         if  (pport) port = *((int*)pport); 

         if  (connectionID) index += 9;     // Connection ID nonzero
              
         if  (conferenceID) index += 6;     // Conference ID nonzero               
         else
         if  (pconfID)      index += 3;     // Conference ID specified
 
         if  (port)         index += 2;     // Port nonzero
         else
         if  (pport)        index += 1;     // Port parameter specified
              
         decision = connectDecisionTable[index];
                                            // Bad parameter combination?
         if  (decision >= ERR_TFP) return decision;
                                            // Set connection type flags 
         if  (decision & (CONFCREATE | CONFJOIN))
              setFlatmapFlag(flatmap, MmsServerCmdHeader::IS_CONFERENCE);

         if  (decision & (CONX_EXISTING | CONX_REMOTE))
              setFlatmapFlag(flatmap, MmsServerCmdHeader::IS_EXISTING_CONNECTION);
         else *sessionaction = NEW_SESSION; // Inform caller of new session 

         if  (decision & NOSTARTMEDIA)
              setFlatmapFlag(flatmap, MmsServerCmdHeader::IS_NOSTARTMEDIA);        
         break;

    case COMMANDTYPE_DISCONNECT: 

          // IS_CONFERENCE to a disconnect means disconnect IP (session) from 
          // conference but do not disconnect the remote session. 
          // IS_MULTISESSION indicates the disconnect applies to all sessions
          // in a conference.

         if  (conferenceID)       
         {    
              setFlatmapFlag (flatmap, MmsServerCmdHeader::IS_CONFERENCE);
          
              if (!connectionID)            // Return conference ID to caller
              {   setFlatmapParam(flatmap, conferenceID);
                  setFlatmapFlag (flatmap, MmsServerCmdHeader::IS_MULTISESSION);
                  *sessionaction = MULTI_SESSION;
              }
         }
         break;

    default:

         // Examine parameters for play, record, and playtone commands. 
         // Media can be "played" to a conference. Such a request arrives as
         // a voice media command with conference ID and without connection ID. 
         // IS_UTILITY_SESSION indicates the need to assign a temporary session
         // which will provide context to execute and monitor such a command

         if  (conferenceID && !connectionID)
         {
              setFlatmapFlag (flatmap, MmsServerCmdHeader::IS_UTILITY_SESSION);
              setFlatmapParam(flatmap, conferenceID);
              *sessionaction = NEW_SESSION; // Inform caller new session required
         }  
  } 

  return 0;                                  
}


                                            
int MmsSessionManager::onMediaEvent(MmsMsg* msg)
{
  // Notification received from the event dispatcher that an event fired,
  // indicating that an async media event we launched earlier has completed.
  // Note that we are responsible for freeing the memory referenced by the 
  // MmsEventRegistry::DispatchMap* which arrives as msg->param(), so we'll 
  // assume the service procedure is going to take care of that.

  return m_threadPool->postMessage(MMSM_SERVICEPOOLEVENT, msg->param());
}



int MmsSessionManager::onServicePoolTaskReturn(MmsMsg* msg)
{
  // A service pool thread completed some work we delegated. We'll forward the
  // notification on to client, but we'll also check if the session has ended,
  // and if so, return the sesssion to available status.

  TaskResultParams* resultData = (TaskResultParams*) msg->param();
  MmsSession* session = resultData->session;
  const int opID = resultData->operationID;
  delete resultData; 

  if  (!session) return -1;                  
  const int sID = session->sessionID();

  if  (session->isSessionClosed())           
  {    
       // If mms client fails to wait for provisional on a prior command before 
       // sending a disco, termination might occur after session has been closed
       MMSLOG((LM_INFO,"SMGR discard event on closed session %d\n", sID)); 
       return -1;
  }

  MmsSession::Op* operation = session->findByOpID(opID);

  if  (!operation)
  {
       MMSLOG((LM_INFO,"SMGR discard event on closed operation %d-%d\n",sID,opID)); 
       return -1;
  }

  char* flatmap = operation->flatmap();     // Session may time out in debugger
  if (!Mms::isFlatmapReferenced(flatmap, 10, sID, opID)) return -1;

  operation->onServiceReturn();                  
                                            // If provisional result, go send it
  if  (getFlatmapRetcode(flatmap) == MMS_COMMAND_EXECUTING)  
       return this->postProvisionalReturn(flatmap, operation);    

  int  commandID = operation->cmdID(), isSessionEnd = FALSE;

  // A session is terminated after an IP disconnect, a failed IP connect,
  // or session timeout, the latter being detected and handled elsewhere.
  // A temporary (utility) session is always terminated at command completion.
   
  switch(commandID)
  { 
    case COMMANDTYPE_DISCONNECT:

         // Unless server command was to disconnect from conference only,
         // end the session 
     
         if  (this->isConferenceOperation(flatmap)); 
         else isSessionEnd = TRUE;
         break;

    case COMMANDTYPE_CONNECT:
         
         // Unless connection was already active (e.g. server command was a 
         // reconnect or conference connect on an active connection),
         // then if a connection error occurred, end the session

         if  (session->isRemoteSessionStarted());
         else isSessionEnd = this->isCommandError(flatmap);
         break;

    default:                                // A utility session exists only
                                            // for the duration of the command
         isSessionEnd = m_sessionPool->isUtilitySession(flatmap);
  }     

  session->closeOperation(opID, commandID);  

  if  (isSessionEnd)                        
  {                                         // If session end ...  
       session->onSessionEnd();             // ... unmap & tear down session
       m_sessionPool->returnSessionToAvailablePool(session);
  }                                         // otherwise close operation      
                                            
  this->logResourceState();                 // Log diagnostics if requested
                                             
  session->markHandlingCommand(FALSE);      // Return result to client via serv
  parent->postMessage(MMSM_SERVERCMD_RETURN, (long)flatmap);
        
  return 0;
}



int MmsSessionManager::onServicePoolTaskExReturn(MmsMsg* msg)
{
  // A pool thread completed some extra-session work for the session manager.  
  // We'll forward the notification on to client if indicated, after freeing
  // the SuperTaskParams parameter block we allocated previously.

  SuperTaskParams* params = (SuperTaskParams*)msg->param();
  char* flatmap       = params->flatmap;
  const int result    = params->result; 
  const int opID      = params->operationID;
  MmsSession* session = params->session; 
  MmsSession::Op* op  = session && opID? session->findByOpID(opID): NULL;
  const int commandID = flatmap? getFlatmapCommand(flatmap): 0;

  
  switch(params->sessionManagerTask)
  {
    case SuperTaskParams::ABANDON_SESSIONS:

         // This was a session manager internal multisession teardown
         // There is therefore no client to notify of this event.   
         break;

    case SuperTaskParams::ABANDON_CONFERENCE:

         // This was requested by client as a disconnect conference ID.  
         // Client may be blocking on the result, which we'll return now

         if  (result > 0)                   // We return the session teardown
         {                                  // count to adapter in map param               
              setFlatmapParam(flatmap, result);

              this->postNormalReturn(flatmap, NULL);
         }
         else this->postErrorReturn (flatmap, MMS_ERROR_NO_SUCH_CONFERENCE, op);
         break;

    case SuperTaskParams::STOPMEDIA:                           

         session->suspendTerminationLock.release();
         session->closeOperation(opID, commandID, FALSE);

         // If client just executed an explicit stop media on a utility session's
         // connection ID (returned from play to or record conference), then end
         // the utility session. It was removed from conference in stopOperation().  
         if  (session->isUtilitySession()) 
         { 
              session->onSessionEnd();      // Unmap & tear down session
              m_sessionPool->returnSessionToAvailablePool(session);
         }  

         if  (result == 0)            
              this->postNormalReturn(flatmap, session);         
         else this->postErrorReturn (flatmap, result, op);
         break;

    case SuperTaskParams::RECONNECT:
    case SuperTaskParams::ADJUSTPLAY:

         // This barrier was set at onSessionTask, above. See comments there.
         session->suspendTerminationLock.release();
         session->closeOperation(opID, commandID, FALSE);
                                
         if  (result == 0)            
              this->postNormalReturn(flatmap, session);         
         else this->postErrorReturn (flatmap, result, op);
         break;

    case SuperTaskParams::VOICEREC_START_COMPTHREAD:
         break;

    case SuperTaskParams::VOICEREC_DONE: 
       
         session->closeOperation(opID, commandID, FALSE);
         if  (result == 0)            
              this->postNormalReturn(flatmap, session);         
         else this->postErrorReturn (flatmap, result, op);
         break;

    case SuperTaskParams::POLICE_DIRECTORIES:
         break;
  }

  delete params;

  if (session) 
  {   session->flags &= ~MMS_SESSION_FLAG_IS_EXTOP_EXECUTING;
      session->markHandlingCommand(FALSE);
  }

  return 0;
}



int MmsSessionManager::executeCommandInline(char* flatmap, MmsSession* session)
{
  // Execute inline command on a session

  const int operationID = session->onCommandStart(flatmap);
  if (!isValidOperationID(operationID)) return MMS_ERROR_RESOURCE_UNAVAILABLE;

  const int commandID = getFlatmapCommand(flatmap);

  const int result = this->doExternalOperation(flatmap, session);

  session->markHandlingCommand(FALSE);
  return result;
}



int MmsSessionManager::doExternalOperation(char* flatmap, MmsSession* session)
{
  // For synchronous commands which can operate on a session which is already   
  // running async media, we handle the command here from outside the session. 

  // If invoked from onSessionTask, onSessionTask has set suspendTerminationLock
  // before calling here. Anything executed here should therefore release the 
  // suspendTerminationLock immediately after execution completes. Any command
  // dropped into the service pool from here will have the suspendTerminationLock 
  // set for the duration of execution, and must release the suspendTerminationLock 
  // at onServicePoolTaskExReturn. When suspendTerminationLock is set, no HMP event
  // handling can proceed to completion, and thus no async command can complete
  // while we are performing this external operation.

  int  result = 0;
  SuperTaskParams* params = NULL;

  setFlatmapSessionID(flatmap, session->sessionID());
  session->flags |= MMS_SESSION_FLAG_IS_EXTOP_EXECUTING;
  const int commandID = getFlatmapCommand(flatmap);
  const int opID  = getFlatmapOperationID(flatmap);
  setFlatmapOperationID(flatmap,opID);


  switch(commandID)               
 {
   case COMMANDTYPE_STOP_OPERATION:
            
        params = new SuperTaskParams
                    (SuperTaskParams::STOPMEDIA, flatmap, session, opID); 
       
        result = m_threadPool->postMessage(MMSM_SERVICEPOOLTASKEX, (long)params);          
        break;        

   case COMMANDTYPE_SEND_DIGITS:             
        {
          // SendDigits is not assigned to a service thread, but is handled inline 
          // (in the session manager thread context), so we release the lock here 
          // which was set at onSessionTask, above. See comments there.

          MmsSession::Op* op = opID? session->findByOpID(opID): NULL;

          result = op? op->handleSendDigits(flatmap): MMS_ERROR_SERVER_INTERNAL;

          session->suspendTerminationLock.release();

          session->closeOperation(opID, COMMANDTYPE_SEND_DIGITS, FALSE);
          session->flags &= ~MMS_SESSION_FLAG_IS_EXTOP_EXECUTING;  

          if  (result)
               postErrorReturn (flatmap, result, op);  
          else postNormalReturn(flatmap, session); 
        }       
        break;

   case COMMANDTYPE_CONNECT:                // Inline reconnect

        params = new SuperTaskParams(SuperTaskParams::RECONNECT, flatmap, session, opID);
        result = m_threadPool->postMessage(MMSM_SERVICEPOOLTASKEX, (long)params); 
        break;

   case COMMANDTYPE_ADJUST_PLAY:            // Adjust play volume and/or speed

        params = new SuperTaskParams(SuperTaskParams::ADJUSTPLAY, flatmap, session, opID);
        result = m_threadPool->postMessage(MMSM_SERVICEPOOLTASKEX, (long)params); 
        break;

   default:
        session->closeOperation(opID);
        session->flags &= ~MMS_SESSION_FLAG_IS_EXTOP_EXECUTING;
 }

  return result;
}

                                          

int MmsSessionManager::isBargeDisconnect(char* flatmap, MmsSession* session)
{
  // Determine if this is a disconnect which barges an async media operation
  
  return session->isBusy() && this->isSessionDisconnect(flatmap, session);
}



int MmsSessionManager::isBusyDisconnect(char* flatmap, MmsSession* session)
{
  // Determines if session manager is currently handling a command and if so,
  // queues the disconnect operation, to be completed when isHandlingCommand
  // becomes false. Can be configured out with Server.cacheBusyDisconnect = 0

  // Must be a session or server disconnect, not a conference-only disco
  if (!this->isSessionDisconnect(flatmap, session)) return FALSE;

  if (!m_config->serverParams.cacheBusyDisconnect)  return FALSE;

  // We queue a disco when we are busy in a "critical section". We do not
  // do so when an async connect is pending, since the "barge disconnect"
  // path picks up a pending connect as well as voice media, canceling
  // the connect and returning a result for both. If we were to queue the 
  // disconnect we would also test for session->isAsyncConnectPending();
  // but handling them with a barge and cancel media as we do here results
  // in much better performance since if an IP connect is hanging, it is
  // canceled as soon as the disco is received rtaher than waiting on HMP.

  if (!session->isHandlingCommand()) return FALSE;

  // Previous command is still in the pipeline -- cache the disconnect
  MMSLOG((LM_INFO,"SMGR queueing disconnect for busy session %d\n",
          session->sessionID())); 
  
  session->pendingDisconnectMap = flatmap; 

  return TRUE;
}



int MmsSessionManager::isSessionDisconnect(char* flatmap, MmsSession* session)
{
  // Determines if this command is a session disconnect, and if so, and if so
  // configured, cancels the currently executing media operation along with
  // the associated termination event. 

  if  (!session || !flatmap) return FALSE;

  // If we're already disconnecting, we obviously can't disconnect again 
  if (session->isDisconnecting() || session->isDisconnectCached()) return FALSE;

  // Must be a session or server disconnect, not a conference-only disco
  if (getFlatmapCommand(flatmap) != COMMANDTYPE_DISCONNECT) return FALSE;
  if (isDisconnectEntireConference(flatmap)) return FALSE;                                            

  return TRUE;
}



int MmsSessionManager::bargeDisconnect(char* discoCommandMap, MmsSession* session)
{
  // Executes a session-barging session disconnect, canceling the currently-executing
  // media operation along with the associated termination event. Results are returned
  // for both the barged and barging commands. The suspendTerminationLock is expected 
  // to be set while we are here. That barrier prevents all media operations we intend
  // to barge, from terminating out from under us while we are here. 

  // The actual disconnect will be executed in the normal logic flow. We have cleared
  // the way for the disconnect to operate by terminating all active operations. 

  // Indicate that we're disconnecting, so that a concurrent second disconnect command 
  // cannot arrive and also be interpreted as a barging disconnect, but instead will  
  // elicit a session busy.
  session->flags |= MMS_SESSION_FLAG_IS_DISCONNECTING;  

  // Cancel all ongoing operations on the session, returning a final result package
  // to client for each such operation, and ensuring that any termination events 
  // currently in the pipeline are or will be discarded. 
  
  const int canceledOperationCount = session->stopAllOperations(this->parent); 

  return canceledOperationCount;                               
}



void MmsSessionManager::onTimer(MmsMsg* msg)  
{
  // Handler for any timers registered to notify session manager 

  // ID of the session owning a call state timer is embedded in the timer ID
  int timerID = msg->param(), sessionID = 0;

  if (timerID > MMS_CALLSTATE_SESSSIONID_TIMERID_OFFSET)                         
  {   sessionID = timerID - MMS_CALLSTATE_SESSSIONID_TIMERID_OFFSET;            
      timerID   = MMS_TIMERID_CALLSTATE;       
  }

  switch(timerID)
  {                                         
    case MMS_TIMERID_CALLSTATE:         
         this->terminateCallStateOperation(sessionID);
         break; 
  }     
}


                                            // Ctor
MmsSessionManager::MmsSessionManager(SessionManagerParams* params): 
  MmsBasicTask((InitialParams*) params)
{
  ACE_ASSERT(params->length == sizeof(SessionManagerParams));
  ACE_ASSERT(params->sessionPool  && params->threadPool   
          && params->timerManager && params->config
          && params->resourceMgr  && params->conferenceMgr);

  m_config        = (MmsConfig*)params->config;
  m_reporter      = params->reporter;
  m_sessionPool   = params->sessionPool;
  m_threadPool    = params->threadPool;
  m_timers        = params->timerManager;
  m_resourceMgr   = params->resourceMgr;
  m_conferenceMgr = params->conferenceMgr;
  m_isShutdownRequest = 0;

  // Initialize resource logging interval manual timers 
  m_resstatTimer.reset(m_config->serverParams.resourceDetailLogIntervalMsecs);
  m_sysstatTimer.reset(m_config->serverParams.systemStatsLogIntervalMsecs);
}



MmsSessionManager::~MmsSessionManager()
{
  
}



// - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
// Posting of results to client
// - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -


int MmsSessionManager::postErrorReturn(char* map, int retcode, MmsSession::Op* op)
{   
  // Set return code and return error 

  if (!Mms::isFlatmapReferenced(map,11)) return -1; 

  setFlatmapRetcode(map, retcode);

  setFlatmapFlag(map, MmsServerCmdHeader::IS_ERROR);

  if (op && op->isBusy())
  {      
      setFlatmapOperationID(map, op->opID());
      MmsSession* session = op->Session();
      if (session) 
      {   setFlatmapSessionID(map, session->sessionID()); 
          session->markHandlingCommand(FALSE); 
      }
  }
                                              
  return parent->postMessage(MMSM_SERVERCMD_RETURN, (long)map);
}
  

                                            
int MmsSessionManager::postNormalReturn(char* map, MmsSession* session)
{
  // Return acknowledgement for non-sessionpooled command

  if (session) session->markHandlingCommand(FALSE);       

  return parent->postMessage(MMSM_SERVERCMD_RETURN, (long)map);
}


                                            
int MmsSessionManager::postProvisionalReturn(char* map, MmsSession::Op* op)
{
  // Return provisional response indicating command has successfully begun 
  // execution. Adapter must not free the parameter map memory when this
  // is the case, since the session continues to reference it.

  // Notes on avoiding race conditions made possible by provisional results:
  // When a provisional result is returned to adapter, the parameter map
  // is still resident in the session. Since the command termination event
  // could fire before the adapter thread has completed processing of the
  // provisional result, the session could write to the map while the adapter
  // is referencing it. The adapter should therefore not depend on any infor-
  // mation in the map which the session might change, such as return code
  // or bitflags. For this reason we provide the xflags entry in the map
  // command header. The server side agrees not to change this entry after 
  // sending a provisional, and the adapter side agrees to change this entry 
  // only during processing of a provisional.

  setFlatmapXflag(map, MmsServerCmdHeader::IS_PROVISIONAL_RESULT);
 
  int result = parent->postMessage(MMSM_SERVERCMD_RETURN, (long)map);

  if (op)
  {
      const int termCondMetResult = op->handleMmsTerminatingConditionsMet(); 
      MmsSession*  session = op->Session();
      if (session) session->markHandlingCommand(FALSE);  

      if (termCondMetResult == MMS_HMP_TERMCOND_MET)     
      {   
          // If the HMP termination condition was met before the provisional,
          // was sent, presumably indicating that we executed an ordinarily 
          // async command synchronously for whatever reason, then follow
          // the provisional result immediately with the final result
          setFlatmapRetcode(map, 0);

          parent->postMessage(MMSM_SERVERCMD_RETURN, (long)map);

          if (session) session->closeOperation(op->opID(), op->cmdID());
      }         
  }
   
  return result;
} 



int MmsSessionManager::postConnectionBusyReturn(MmsSession* session, char* flatmap)
{
  // session.isBusy indicates that at least one async command is operating on
  // the session. session.isBusy() becomes true just before a command is dropped 
  // into the service pool. session.isBusy() becomes false when session state 
  // reverts to idle after its last command ends.

  MMSLOG((LM_INFO,"SMGR connection %d session %d is busy\n",
          session->connectionID(), session->sessionID())); 

  return postErrorReturn(flatmap, MMS_ERROR_CONNECTION_BUSY);
}  



// - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
// Session and command timeout monitoring
// - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

                                             
void MmsSessionManager::onNotify(MmsMsg* msg)
{
  // Server manager tells us when it is time to police the session pool
  if  (msg->param() == MMS_MONITOR_INTERVAL)
       this->monitorSessionPool();
}



bool MmsSessionManager::monitorSessionPool()
{ 
  // Periodically monitor all active sessions for timeouts. Each session hosts
  // simple countdown timers for the session, and for the command. A session is
  // timed out if it has seen no activity for the number of seconds the timer
  // is set for. A command is timed out if its associated async event has not 
  // fired within the number of milliseconds the timer is set for, or if it
  // has been executing in a service thread for that length of time. Command
  // timeouts may be indicative of a serious problem, since ordinarily an HMP
  // termination condition should fire if the the command is merely taking
  // more time than is desired. Countdown timers are updated periodically here,
  // every config.serverParams.sessionMonitorIntervalSeconds. A session timer 
  // is reset to config.Server.sessionTimeoutSecondsDefault by default, and may
  // be overridden by a MMSP_SESSION_TIMEOUT_SECS parameter with the connection.
  // A command timer is reset to config.Server.commandTimeoutMsecsXXXXXX ms
  // by default, and may be overridden by specifying a MMSP_COMMAND_TIMEOUT_MS 
  // parameter with the server command. Timeout resolution is obviously coarse,
  // since timeout checks are likely done here only every few seconds, however
  // command timeouts are largely media firmware viability checks, while reso-
  // lution of session timeout should not be an issue. We should be judicious 
  // about how often we come through here, since we lock out all event handling
  // while we're here.  

  // Note regarding IP sessions in conference. For now, we don't permit these
  // to time out. This introduces the possiblity of orphaned sessions however,
  // so we'll want to write some code to also periodically monitor the
  // conference pool, somehow identifying dead conferences, and killing all
  // sessions in such conferences, along with the conference itself.
  // Our strategy could be: for each active conference, if no active talkers.
  // count down conference timer. No, this is heavy cpu use, instead attach
  // a voice drone to the conference, and monitor cst silence on that. Alter-
  // natively, a cluster manager could monitor conferences externally.

  // Third parameter indicates tryacquire first to avoid deadlock
  ACE_Guard<ACE_Thread_Mutex> x(this->sessionPoolLock(), 0);

  MmsSession* session    = m_sessionPool->base();
  const int sessionCount = m_sessionPool->size();
                                            // For each session object ...
  for(int i=0; i < sessionCount; i++, session++)
  {                             
      session->sessionDataLock.acquire();
            
      if (!session->isInUse());             // ... which is active ...
      else                                  // ... udpdate timers                                            
      if  (session->countdownSession() == -1)
                                            // Session timer expired
           if  (session->isConferenceParty())      
                session->resetSessionTimer();     

           else this->onSessionTimeout(session);
      else                                  
      {    session->optableLock.acquire();  // Session not timed out ... 
           MmsSession::Op* op = &session->optable[0];
                                            // ... so update op timers
           for(int j=0; j < MMS_MAX_SESSION_OPERATIONS; j++, op++) 
           {
               if (!op->isBusy()) continue;
               if (!op->isInit()) continue;
               if  (op->countdownOperation() == -1)    
                    this->onCommandTimeout(op);  
           } 

           session->optableLock.release();
      }

      session->sessionDataLock.release();
  }

  return true;
}



void MmsSessionManager::onCommandTimeout(MmsSession::Op* operation)
{
  // Invoked when the periodic monitoring of the session pool indicates that
  // a command has not completed execution within its allotted time. The media
  // operation may have been a lengthy one with client not supplying an 
  // overriding command timeout of sufficient duration. Another possibility is
  // that HMP service state has been compromised. 

  operation->setCommandTimeout();
  char* map = operation->flatmap(); 
  MmsSession* session = operation->Session(); 
  const int opID  = operation->opID();
  const int cmdID = operation->cmdID();
  const int sID   = session? session->sessionID(): 0;
  int ensureOperationClosed = TRUE, isErrorReturn = TRUE, isVoiceCommand = TRUE; 
 

  if  (IS_LISTENONLY_VOICE_COMMAND(cmdID))
       // When no termination event is expected, end the operation without error
       isErrorReturn = FALSE;  
  else
  {    MMSLOG((LM_ERROR,"SMGR session %d op %d %s timed out\n", 
               sID, opID, Mms::commandName(cmdID))); 
   
       if  (operation->isWaiting() || operation->isStreaming())   
       {          
            if  (Mms::isFlatmapReferenced(map, 17, sID, opID)) 
                 setFlatmapFlag(map, MmsServerCmdHeader::IS_ERROR); 
            
            if  (IS_ASYNC_VOICE_COMMAND(cmdID))  
            {    // stopOperation will cancel term event, close the operation, 
                 // and return a final result to client, hence we set a flag
                 // so as not to do this again below:                                                    
                 if  (0 == session->stopOperation(operation, this->parent, FALSE))
                      ensureOperationClosed = FALSE;
            }
            else isVoiceCommand = FALSE;
       }
  }

  if  (ensureOperationClosed)
  {
       session->closeOperation(opID, cmdID); 

       if  (isErrorReturn)
            this->postErrorReturn(map, MMS_ERROR_TIMEOUT_OPERATION);
       else 
       if  (isVoiceCommand)
            this->postNormalReturn(map, session);

       else this->postMessage(MMSM_SERVICEPOOLTASK_RETURN, 
                 (long) new TaskResultParams(session, opID)); 
  }

  if (session->isUtilitySession())  
  {   // 20070607 when the timed-out command is hosted by a system session, which 
      // by definition hosts only this one operation, close out the session now.
      session->onSessionEnd();             
      m_sessionPool->returnSessionToAvailablePool(session);
  }
}



void MmsSessionManager::onSessionTimeout(MmsSession* session)
{
  // Session has timed out. We terminate the connection and return the session
  // to service. Subsequent requests on the connection ID will elicit a no such
  // connection error. If session code is currently executing in a thread we're  
  // almost definitely hosed. If the session were to maintain a record of the 
  // adapter creating the session (from map server command header sender), we 
  // could easily notify the adapter, via server manager, that the session, or 
  // connection ID, has timed out. However it is not clear whether this would 
  // be useful, as either adapter would need to be maintaining state, or adapter
  // client would have to be receptive to event push. 

  // Note that caller holds the session pool lock, so no terminationevent can 
  // complete on the session, and neither can the session be abandoned, while 
  // we're here.

  // Indicate that we're disconnecting, so that if a disconnect command were 
  // to arrive it will now elicit a session busy.
  session->flags |= MMS_SESSION_FLAG_IS_DISCONNECTING;  

  MMSLOG((LM_ERROR,"SMGR session %d timed out: closing connection\n", 
          session->sessionID()));

  const int canceledOperationCount = session->stopAllOperations(this->parent); 
 
  session->onSessionEnd();             
  m_sessionPool->returnSessionToAvailablePool(session);
}



int MmsSessionManager::onAbandonConference(char* flatmap)
{
  // Disconnect all sessions in a conference and close the conference
  // Conference ID is expected in map param. We assign this work to a pool
  // thread. Result message is posted to client at onServicePoolTaskExReturn.

  SuperTaskParams* params = new SuperTaskParams;
  params->sessionManagerTask = SuperTaskParams::ABANDON_CONFERENCE;
  params->flatmap = flatmap;
  params->client  = (MmsTask*)getFlatmapSender(flatmap);
  params->handle  =  getFlatmapClientHandle(flatmap);
  params->conferenceID = getFlatmapParam(flatmap);

  return m_threadPool->postMessage(MMSM_SERVICEPOOLTASKEX, (long)params);
}

 

int MmsSessionManager::onAbandonSessions
( void* client, void* handle, const int conferenceID)
{ 
  // Clean up sessions abandoned on premature client disconnect or on media
  // server shutdown, or on conference disconnect.

  // When a client abandons active sessions and events, the media server must
  // close these out, particularly in the case of sessions in conference,
  // which do not time out. This notification is triggered by client adapter
  // disconnection from media server. 

  // If a conference ID is specified, we close out only those sessions
  // which are members of the specified conference. The conference is 
  // implicitly closed as the last participant session is removed.

  // If no client is specified, the server was stopped via server command
  // e.g. server control gui or console. In that case we close all sessions.

  // Since the IP stop process is time-consuming, this should be executed 
  // in a service thread if session manager availability is a concern.

  // Since we hold the session pool lock, no media event handling can occur
  // while we're here (see onServicePoolEvent). So, it should not be necessary
  // to serialize the parameter map memory reclamation done here. Likewise, a 
  // session cannot time out while we're here, since timeout monitoring also 
  // requires the session pool lock.

  // Note that the server can receive a shutdown command from server admin
  // and a bulk teardown thus initiated, while another bulk teardown is in
  // progress, e.g. due to client disconnect and abandonment of sessions.
  // We therefore obtain the session pool lock as early as possible 
  // to ensure that state is current.

  ACE_Guard<ACE_Thread_Mutex> x(this->sessionPoolLock()); 
                                       
  const int abandonedSessions               // See if there is work to do here
    = this->abandonedSessionsCount(client, handle, conferenceID);

  if  (abandonedSessions == 0) return 0;
  this->flags |= MMS_SESSION_FLAG_ABANDON_IN_PROGRESS;
  this->discoListAdd(client);               // Indicate client disco in progress
   
  if  (conferenceID)
       MMSLOG((LM_INFO,"SMGR close conference %d, %d session(s)\n", 
               conferenceID & 0xffff, abandonedSessions));
  else 
  if  (handle == NULL)
       MMSLOG((LM_INFO,"SMGR close %d abandoned session(s)\n", 
               abandonedSessions));
  else MMSLOG((LM_NOTICE,"SMGR close %d session(s) abandoned by client %x\n", 
               abandonedSessions, handle));

  int   teardownCount = 0, i;        
  const int sessionCount = m_sessionPool->size();
  MmsSession* session    = m_sessionPool->base();
                                                             
  for(i=0; i < sessionCount; i++, session++)
  {                 
      if  (teardownCount == abandonedSessions) break;                         
      if  (!session->isInUse()) continue;   // For each abandoned session ...
      if  (client && (session->client != client)) continue; 
      if  (handle && (session->clientHandle != handle)) continue; 
      if  (conferenceID && (session->confInfo.id != conferenceID)) continue;
                                            // Reject further commands
      session->flags |= MMS_SESSION_FLAG_IS_DISCONNECTING;  
                                            // Cancel async operations
      session->stopAllOperations(this->parent);
                                            // Stop listening, and clear                                            // Leave conference if applicable
      session->onSessionEnd();              // out session object.
                                             
      m_sessionPool->returnSessionToAvailablePool(session);
      teardownCount++;
  }

  // Indicate this client disconnect no longer in progress. 
  // Presumably any of this client's transactions which were in the pipeline
  // when client began disconnecting, have queried this list and canceled
  // themselves by now. If it turns out to be the case that we do not catch all
  // pending transactions in this manner, we'll have to change this scheme to
  // retain client handles in the disco list for some period of time. We would
  // then require some means of determining when to remove the client handle
  // from the list. MMS-195.
  this->discoListRemove(client);             

  return teardownCount;                     // Return abandoned sessions count
}



int MmsSessionManager::onAbandonSessionsBackground(void* client, void* handle)
{
  // Assign onAbandonSessions to a service thread

  if  (this->abandonedSessionsCount(client, handle) == 0) return 0;

  SuperTaskParams* params = new SuperTaskParams;
  params->sessionManagerTask = SuperTaskParams::ABANDON_SESSIONS;
  params->client = (MmsTask*)client;
  params->handle = handle;

  return m_threadPool->postMessage(MMSM_SERVICEPOOLTASKEX, (long)params);
}
   


int MmsSessionManager::abandonedSessionsCount
( void* client, void* handle, const int conferenceID)
{
  int   abandonedSessions = 0;        
  const int sessionCount  = m_sessionPool->size();
  MmsSession* session     = m_sessionPool->base();
                                                       
  for(int i=0; i < sessionCount; i++, session++)
  {   
      if  (!session->isInUse()) continue; 
      if  (client && (session->client != client)) continue; 
      if  (handle && (session->clientHandle != handle)) continue; 
      if  (conferenceID && (session->confInfo.id != conferenceID)) continue;
      abandonedSessions++;
  }
     
  return abandonedSessions;
}



void MmsSessionManager::logResourceState()
{
  // Verbose diagnostic tool enabled via config entry Server.logResourceStatus
  // Displays media device state, session state, and conference manager state.
  // If detail logging is needed, this must be further enabled via engineering
  // diagnostics switches masked into config::Diagnostics.flags, the bits being
  // MMS_DIAG_LOG_DEVICE_DETAIL and MMS_DIAG_LOG_SESSION_DETAIL respectively.

  // When many sessions are active, resource state logging becomes interleaved
  // and cluttered, to the point of losing utility. For this reason, we try to
  // acquire a lock before we begin; if another thread has the lock, we skip
  // that round of resource logging. This means we may not always see the most
  // current snapshot of resource state in the log, but we will see only one
  // pass at a time, which is the better tradeoff.

  static char* xmask = "SMGR ses bz-av:%d-%d ip:%d v:%d c:%d lb:%d ts:%d vr:%d\n";
  static char* dstates[]={"close","avail","inuse"," idle","limbo"};
  static enum  dxstates  { CLOSX,  AVAIX,  BUSX,   IDLX,   LIMBX};
  static char* mstates[]={"unkn","idle","live","stop"};
  static enum  mxstates  { UNKX,  IDXX,  LIVX,  STPX}; 

  if (m_sysstatTimer.countdown() == -1)
      Mms::logMemUsage();                   // Log system stats every n ms
                                             
  const int pendingEventCount = MmsEventRegistry::instance()->entries();

  if  (pendingEventCount > m_sessionPool->size())
  {
       char* msg = "pending event count exceeds threshold";
       MMSLOG((LM_ERROR, "SMGR warning %s\n", msg));
       MmsReporter::raiseServerAlarm(MmsReporter::NDX_UNEXPECTED_CONDITION, msg);
  }
                                            // Log diags only if configured
  if  (!m_config->diagnostics.logResourceStatus) return;
                                            // Blow it off if busy
  if  (-1 == m_logStateLock.tryacquire()) return;

  const int configPriority  = m_config->serverLogger.globalMessageLevel;
  ACE_Log_Priority msgPriority = configPriority <= 3? LM_DEBUG:
            pendingEventCount > 9? LM_NOTICE:
            pendingEventCount > 4? LM_INFO: LM_DEBUG;

  MMSLOG((msgPriority,"SMGR %d events pending\n", pendingEventCount));

  // Show aggregate resource counts:  
  const int sesBusy  = m_sessionPool->ipSessionsActive();
  const int sesAvail = m_sessionPool->ipSessionsAvail();

  msgPriority = MmsAs::avlG711 < 3 || MmsAs::avlVox < 3 || MmsAs::avlConf < 3? LM_INFO:
                MmsAs::licG729 > 0 && MmsAs::avlG729 < 3? LM_INFO:
                MmsAs::licTTS  > 1 && MmsAs::avlTTS  < 2? LM_INFO:
                // As::licCSP && MmsAs::licASR && (!MmsAs::avlASR || !MmsAs::avlCSP)? LM_INFO:
                LM_DEBUG;
                                                      
  MMSLOG((msgPriority, xmask,               // Aggregate availability counts
          sesBusy, sesAvail,                // Session busy/available
          MmsAs::avlG711,                   // G729 available
          MmsAs::avlVox,                    // Vox available
          MmsAs::avlConf,                   // Conference resx available
          MmsAs::avlG729,                   // G729 available
          MmsAs::avlTTS,                    // TTS ports available
          MmsAs::avlCSP));                  // CSP resx available                  
                                             
                                            // Log detail only ...
  const int logDetailInterval = m_config->serverParams.resourceDetailLogIntervalMsecs;
  const int isLogDetailNow    = logDetailInterval == 0            // ... always, or ...
                             || m_resstatTimer.countdown() == -1; // ... every n ms
  if (!isLogDetailNow) return;
                                             
  if((m_resourceMgr->config->diagnostics.flags & MMS_DIAG_LOG_DEVICE_DETAIL)
   && m_resourceMgr->mediaResourceTable.size())
  {                                         // Show state of each media device:
      HmpResourceManager::MmsMediaDeviceTable::iterator i;
                                            // Static table so don't need lock 
      for(i  = m_resourceMgr->mediaResourceTable.begin(); 
          i != m_resourceMgr->mediaResourceTable.end(); i++)
      {                                          
          MmsMediaDevice* device = i->second; 
                                            // For now, omit IP from listing
          if  (device->type() == MmsMediaDevice::IP) continue;
                                            // Show only assigned devices
          if ((device->type() == MmsMediaDevice::IP    && device->isMediaIdle()) 
           || (device->type() == MmsMediaDevice::VOICE && device->isAvailable())
           || (device->type() == MmsMediaDevice::CONF))
               continue;
   
          char* dstate = dstates[CLOSX],*mstate = mstates[UNKX]; 

          if  (device->isAvailable())    dstate = dstates[AVAIX]; else
          if  (device->isBusy())         dstate = dstates[BUSX];  else
          if  (device->isIdle())         dstate = dstates[IDLX];  else
          if  (device->isLimbo())        dstate = dstates[LIMBX];

          if  (device->isMediaActive())  mstate = mstates[LIVX];  else
          if  (device->isMediaWaiting()) mstate = mstates[IDXX];  else  
          if  (device->isMediaIdle())    mstate = mstates[STPX];  

          MMSLOG((LM_INFO,"SMGR %s %d owner %d: %s, media %s\n",   
          device->name(), device->handle(), device->owner(), dstate, mstate));
      }
  }
                                            // Show state of each session:
                                            // Aggregate session counts:
  MMSLOG((LM_DEBUG,"SMGR ip sessions total/active/avail %d %d %d\n",
       m_sessionPool->ipPoolSize(), m_sessionPool->ipSessionsActive(), 
       m_sessionPool->ipSessionsAvail()));  

  const int utilityActive = m_sessionPool->utilSessionsActive(); 

  if  (utilityActive) 
  {                                         // Aggregate utility sessions
       const int utilityTotal  = m_sessionPool->utilityPoolSize(); 
       const int utilityAvail  = m_sessionPool->utilSessionsAvail();
       msgPriority = utilityAvail < 3? LM_NOTICE: LM_DEBUG;
       MMSLOG((msgPriority,"SMGR utility sessions total/active/avail %d %d %d\n",
               utilityTotal, utilityActive,utilityAvail));
  }

                                             
  if (m_resourceMgr->config->diagnostics.flags & MMS_DIAG_LOG_SESSION_DETAIL)
  {                                          
      MmsSession* session    = m_sessionPool->base();
      const int sessionCount = m_sessionPool->size();
      const int sessionID    = session->sessionID();
                                            // Show state of each session:
      for(int j=0; j < sessionCount; j++, session++)
      {                                         
          if (!session->isInUse()) continue;// If session is currently bound.
                                            // to a connection, it must be ...
          if  (session->isInConference())   // ... in conference ...
               if  (session->isHairpinConference())
                    MMSLOG((LM_INFO,"SMGR session %d in hairpin confx %d\n",
                            sessionID, session->confinfo().id));                   
               else MMSLOG((LM_INFO,"SMGR session %d in conference %d\n",
                             sessionID, session->confinfo().id));
          else
          if (!session->isBusy())           // ... executing no operations ...
               MMSLOG((LM_INFO,"SMGR session %d idle\n", sessionID));
                                            // ... or executing 1 or more ops
          else MMSLOG((LM_INFO,"SMGR session %d pending %d operations\n", 
                       sessionID, session->operationCount()));
      }
  }
  

  m_conferenceMgr->slock.acquire();       
                                           // If there are any conferences ...
  if (m_conferenceMgr->conferences.size() > 0)                                            
  {                                        // ... show state of each conference
      std::map<int, MmsConference*>::iterator i 
               = m_conferenceMgr->conferences.begin();
      for(; i != m_conferenceMgr->conferences.end(); i++)
      {
          MmsConference* confx = i->second; 
          const int confID = i->first, siz = confx->size(), mons = confx->nmonitors();

          if  (confx->isHairpinned()) 
               MMSLOG((LM_INFO,"SMGR hairpin conference %d parties %d\n", confID, siz));        
   
          else MMSLOG((LM_INFO,"SMGR conference %d parties %d mon %d\n", 
                       confID, siz, mons));
      }
  }

  m_conferenceMgr->slock.release();

  m_logStateLock.release();
}


