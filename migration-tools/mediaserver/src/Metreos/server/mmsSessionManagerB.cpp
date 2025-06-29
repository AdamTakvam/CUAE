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
#include "mmsAsr.h"

#ifdef MMS_ENABLE_MEMORY_LEAK_DETECTION
#define new MMS_MEMLEAK_NEW
#endif // MMS_ENABLE_MEMORY_LEAK_DETECTION


// - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
// Utility methods
// - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -


MmsCurrentCommand* MmsSessionManager::currentCommand
( int cmd, MmsSession* session, MmsSession::Op* opobj, char* map, int flags)
{
  // Allocate and return a MmsCurrentCommand object initialized with the supplied
  // parameters. Used to package command parameters into a MmsMsg parameter.
   
  MmsCurrentCommand* cc = new MmsCurrentCommand(cmd, session, opobj, map);
  cc->flags = flags;
  return cc;
}



MmsSession* MmsSessionManager::getSessionByID(const int connectionID)
{
  MmsSession* session = NULL;
  if  (NULL == (session = m_sessionPool->findByConnectionID (connectionID)))
       MMSLOG((LM_NOTICE,"SMGR connection ID %d not found\n",connectionID)); 
  return session;
}



int MmsSessionManager::isGoodConnectionState(MmsSession* session, const int command)
{                  
  // For a command on existing session, ensure session is fully connected
                         
  MmsDeviceIP* deviceIP = session->ipDevice();
  if  (deviceIP == NULL) return TRUE;       // Utility session state always OK

  switch(command)                           // Connect or disco OK on session
  { case COMMANDTYPE_CONNECT:               // which is not fully connected 
    case COMMANDTYPE_DISCONNECT: return TRUE;
  }         
                                            // Otherwise return whether or not
  return deviceIP->isStarted();             // remote IP is connected 
}



int MmsSessionManager::isDisconnectEntireConference(char* flatmap) 
{
   // Determine if command is a conference-only disconnect
   return ((COMMANDTYPE_DISCONNECT == getFlatmapCommand(flatmap))
        && (0 == getFlatmapConnectionID(flatmap)) 
        && isFlatmapFlagSet(flatmap, MmsServerCmdHeader::IS_CONFERENCE));
}



int MmsSessionManager::isConcurrentOperationOK
( const int cmdID, MmsSession* session, MmsSession::Op* op)
{
  // Determines if specified operation can run concurrently with existing operations
  if (op && op->isNoPromptVoiceRec()) return TRUE;

  const unsigned int opCaps = getConcurrentOpCaps(cmdID, session);
  int isConcurrentOK = (opCaps & OPCAPS_AYSNC_OK) != 0;

  return isConcurrentOK;
}



unsigned int MmsSessionManager::getConcurrentOpCaps(const int cmdID, MmsSession* session)
{
  // Evaluates session state and determines whether the indicated operation can
  // execute concurrently with any existing session operations. 
  // Some operations, such as external operations, are serialized, so we don't 
  // ordinarily reject them, but rather make them wait in their thread. 

  unsigned int result = 0;

  if (session->isBusy()) 
      result |= OPCAPS_ONGOING_ASYNC;

  if (session->isExternalOpExecuting()) 
      result |= OPCAPS_ONGOING_EXTOP;
      
  if (IS_ASYNC_VOICE_COMMAND(cmdID)) 
  {   
      // Async voice can execute concurrently but with the following exceptions:
      // * Multiple bi-directionally-listened commands cannot coexist in a session.
      // * Only one call state monitor per session; ditto voice rec with prompt.
      // Others can execute concurrently; for example, play and record, voicerec
      // and getdigits, getdigits and getdigits, etc.
      
      switch(cmdID)
      {
        case COMMANDTYPE_PLAY:
        case COMMANDTYPE_PLAYTONE:    
        case COMMANDTYPE_VOICEREC:          // Disallow concurrent 2-way listens
             if  (session->isRunningDuplexOperation() == 0) 
                  result |= OPCAPS_AYSNC_OK;
             break;
                                            // Disallow multiple call state 
        case COMMANDTYPE_MONITOR_CALL_STATE:
             if  (session->isCallStateMonitoring());
             else result |= OPCAPS_AYSNC_OK;
             break;

        default:                             
             result |= OPCAPS_AYSNC_OK;      
      }
  }

  return result; 
}



void MmsSessionManager::terminateCallStateOperation(int sessionID)  
{
  // Terminate a monitorCallState command. This command is not playing media,
  // but rather is passively watching for all state transition events.

  MmsSession* session = m_sessionPool->findBySessionID(sessionID);
  if (session == NULL) return;

  MmsSession::Op* operation = session->findCallStateOperation();
  if (operation == NULL) return;

  char* flatmap = operation->flatmap(); 

  operation->handleCstTimeExpired();        // Stop monitoring events
                                            // Terminate command
  session->closeOperation(operation->opID(), COMMANDTYPE_MONITOR_CALL_STATE);         
  
  session->markHandlingCommand(FALSE);  
                                            // Return result to client via serv
  parent->postMessage(MMSM_SERVERCMD_RETURN, (long)flatmap);
} 



// - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
// Voice recognition event handlers
// - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -


int MmsSessionManager::onCspDataReady(MmsMsg* msg)
{
  // Handle CSP data chunk from CSP streaming callback.
  // The only piece of information we have is the device handle of CSP capable voice device.
  // We use the device handle to look for the voice device through resource manager then
  // perform a session look up in session pool based on session id, which is device's owner id.
  mmsDeviceHandle dh = (mmsDeviceHandle)msg->param();

  MmsDeviceVoice* device = (MmsDeviceVoice*)Asr::instance()->m_resourceManager->getDevice(dh);   
  if (device == NULL) return -1;

  // Perform reverse lookup to find the session from session pool.
  MmsSession* session = this->sessionPool()->findBySessionID(device->owner());
  if (session == NULL) return -1;

  MmsSession::Op* op = session->findByOpID(device->subowner());

  if (op == NULL || op->asrChan == NULL)
      return -1;

  // Write data into VR endpoint then recognizer
  return Asr::instance()->handleData(op->asrChan, // ASR channel associated w session/operation
                                    msg->base(),  // streaming data chunk  
                                    msg->size()); // data chunk size
}



int MmsSessionManager::onStartComputationThread(MmsMsg* msg)
{
  // Each active ASR channel has a computation thread to check the recognition status 
  // constantly. We start the thread when session ASR channel is activated and started.
  MmsSession::Op* op = (MmsSession::Op*)msg->param();
  SuperTaskParams* params = NULL;
  params = new SuperTaskParams;        
  params->sessionManagerTask = SuperTaskParams::VOICEREC_START_COMPTHREAD;
  params->session = op->Session();
  params->operationID = op->opID();

  // Post a super task into threadpool and start the thread in session pool context.
  return m_threadPool->postMessage(MMSM_SERVICEPOOLTASKEX, (long)params); 
}



// - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
// Disconnecting clients list maintenance
// - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -


int MmsSessionManager::discoListAdd(void* clienthandle)
{
  if (NULL == clienthandle) return -1;
  ACE_Guard<ACE_Thread_Mutex> x(m_discosLock);
  m_discos.push_back(clienthandle);
  return 0;
}



int MmsSessionManager::discoListRemove(void* clienthandle)
{   
  ACE_Guard<ACE_Thread_Mutex> x(m_discosLock);
  if (m_discos.size() == 0) return -1;  
  m_discos.remove(clienthandle);
  return 0;
}



int MmsSessionManager::isClientDisconnecting(char* flatmap)
{
  if (m_discos.size() == 0) return FALSE;
  if (!Mms::isFlatmapReferenced(flatmap, 9, 0, 0)) return FALSE;

  void*  clienthandle = getFlatmapClientHandle(flatmap);

  return this->isClientDisconnecting(clienthandle);
} 



int MmsSessionManager::isClientDisconnecting(void* clienthandle)
{
  if (m_discos.size() == 0) return FALSE;

  int result = 0;
  ACE_Guard<ACE_Thread_Mutex> x(m_discosLock);

  std::list<void*>::iterator i = m_discos.begin();

  for(; i != m_discos.end(); i++)
      if (*i == clienthandle) { result = TRUE; break; }

  return result;
} 
