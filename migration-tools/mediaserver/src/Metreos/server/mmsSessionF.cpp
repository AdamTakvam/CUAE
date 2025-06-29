//
// MmsSessionF.cpp
// 
// Session/operation media renegotation support
//
#include "StdAfx.h"
#pragma warning(disable:4786)
#ifdef MMS_WINPLATFORM
#include <minmax.h>
#endif
#include "mmsSession.h"
#include "mmsParameterMap.h"
#include "mmsMediaEvent.h"
#include "mmsSessionManager.h"
#include "mmsAudioFileDescriptor.h"
#include "mmsCommandTypes.h"

#ifdef MMS_ENABLE_MEMORY_LEAK_DETECTION
#define new MMS_MEMLEAK_NEW
#endif // MMS_ENABLE_MEMORY_LEAK_DETECTION



// - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
// MmsReconnectInfo and session reconnect logic  
// - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -


MmsReconnectInfo::MmsReconnectInfo(MmsSession* session, MmsSession::Op* thisop)
{
  // This object stores state for a reconnect operation in progress.

  this->clear();
  this->session = session;
  this->operationID = thisop->opID();       // Connect command operation

  ACE_Guard<ACE_Thread_Mutex> x(session->optableLock);

  MmsReconnectInfo::OpInfo* opinfo = &this->opInfo[0];

  MmsSession::Op* op = &session->optable[0];

  for(int i=0; i < MMS_MAX_SESSION_OPERATIONS; i++, op++)
  {
      // For each of this session's active operations, if the operation is 
      // executing voice media, save the operation's state in this object.

      if (!op->isBusy()) continue;

      MmsDeviceVoice* deviceVoice = op->assignedVoiceDevice();
      if (deviceVoice == NULL) continue;
      this->unlistenedCount++;

      opinfo->operationID = op->opID();
      opinfo->wasOwned    = deviceVoice->isOwned();
      opinfo->wasVoiceMediaPlaying = deviceVoice->isMediaActive();
      deviceVoice->setOwned(TRUE);
      opinfo++;
  }

  if (this->unlistenedCount)
      MMSLOG((LM_INFO,"%s suspended %d media operations\n", 
              session->name(), this->unlistenedCount));
}



int MmsReconnectInfo::relisten()
{
  // Relistens to voice after IP/port switch while voice was in use
  // Returns count of operations relistened

  MmsDeviceIP* deviceIP = this->session? session->ipDevice(): NULL;
  if (!deviceIP) return 0;
  int relistenCount=0;

  ACE_Guard<ACE_Thread_Mutex> x(session->optableLock);
  MmsReconnectInfo::OpInfo* opinfo = &this->opInfo[0];

  for(int i=0; i < MMS_MAX_SESSION_OPERATIONS; i++, opinfo++)
  {
    const int opID = opinfo->operationID; if (opID == 0) continue;

    MmsSession::Op* operation = session->findByOpID(opID, FALSE);
    if (operation == NULL) continue;

    MmsSession::ListenDirection listenDirection 
      = session->getListenDirectionByCommandID(operation->cmdID());

    MmsDeviceVoice* deviceVoice = operation->voiceDevice(TRUE, TRUE, listenDirection);

    if (deviceVoice) 
    {
        MMSLOG((LM_INFO,"%s media resumed on operation %d\n", session->name(), opID));
        relistenCount++;

        if (!opinfo->wasOwned) deviceVoice->setOwned(FALSE);
    }
    else MMSLOG((LM_NOTICE,"%s could not resume media on operation %d\n", 
                 session->name(), opID));
  }

  return relistenCount;
}



void MmsReconnectInfo::clear()
{
  memset(this,0,sizeof(MmsReconnectInfo));
}


// - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
// Session media renegotation support
// - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -


int MmsSession::holdRemoteSession(MmsSession::Op* operation)
{
  // Place IP session on hold during a mid-session change of IP/port
   
  MmsDeviceIP* deviceIP = this->ipDevice(); 
  if  (!deviceIP) return FALSE;
                                            // Mark session for reconnect
  this->reconnectInfo = new MmsReconnectInfo(this, operation); 
  operation->putMapHeader(setFlag, MmsServerCmdHeader::IS_SESSION_RECONNECT);

  if  (this->isConferenceParty())           // Indicate conference reconnect
       operation->putMapHeader(setFlag, MmsServerCmdHeader::IS_CONFERENCE);

  if (!deviceIP->isStopped())
       deviceIP->stop(STOP_ALL);            // Unlisten and stop remote session 

  return 0;
}



int MmsSession::isRemoteMediaChange (const char* ipAddr, const int port, 
  const unsigned int newRemoteAttrs, const unsigned int currentRemoteAttrs)
{
  // Determine if incoming connect requests a change of IP and/or port
  // for an existing remote connection. Either IP or port, or both, may
  // be changed. Also determine if an attribute of the connection is
  // being changed, whether or not the ip/port was changed.

  // Returns 0 if remote media parameters not changed
  //         1 if remote media parameters changed

  this->clearReconnectState();
  if  (!ipAddr && !port) return FALSE;
  if  (!this->isRemoteSessionStarted()) return FALSE;

  MmsDeviceIP* deviceIP = this->ipDevice();  
  const char* currIP    = deviceIP->remoteIP();
  const char* newIP     = ipAddr? ipAddr: currIP;
  const int currPort    = deviceIP->remotePort();
  const int newPort     = port? port: currPort;
  
  const int isSameIP    = (0 == ACE_OS::strcmp(newIP, currIP));
  const int isSamePort  = (newPort == currPort);
  const int isChanging  = !(isSameIP && isSamePort);

  const int remoteAttributeChangeCount       
      = this->isRemoteConnectionAttributeChange
             (newRemoteAttrs, currentRemoteAttrs, TRUE); 

  static const char *with = "with coder change", *without = "";
 
  if  (isChanging)
  {
       // We assume remote attributes (coder, framesize) will not change unless
       // ip/port also change, since client hold/resume drives the reconnect, 
       // and with Cisco hold/resume always results in an ip/port change. 
       const char* which = remoteAttributeChangeCount? with: without; 

       MMSLOG((LM_INFO,"%s %s/%d moves to %s/%d %s\n",  
               objname, currIP, currPort, newIP, newPort, which));
  }
  else
  if  (remoteAttributeChangeCount)           
       MMSLOG((LM_INFO,"%s %s/%d connect attrs changing\n",  
               objname, currIP, currPort));

  const int isRequireIpSessionRestart       
      = isChanging || remoteAttributeChangeCount;

  // Note that we are here ignoring the connect parameter "modify" (which we
  // could identify here by map flag IS_MODIFY_CONNECT set), and determining
  // whether to restart the IP session based solely upon the above comparison
  // of current and new connection attributes. If this session is busy with a
  // media operation, IS_MODIFY_CONNECT allowed the reconnect command to 
  // interrupt the session if indeed it was busy, and proceed to this point   
  // (i.e. not return a session busy error). 

  return isRequireIpSessionRestart;          
}



int MmsSession::Op::isMediaParameterChangeRequest()
{
  // Determine if this connect command has the media parameter change syntax, 
  // whether or not the parameter values are in fact different from those
  // of the existing connection. 

  char* pport=0, *pip=0, *pattrs=0, *flatmap = this->flatmap();
  if (!flatmap) return 0;
  MmsFlatMapReader map(flatmap);
  map.find(MMSP_PORT_NUMBER, &pport);
  map.find(MMSP_IP_ADDRESS, &pip);
  map.find(MMSP_REMOTE_CONX_ATTRIBUTES, &pattrs);
  return pport != 0 || pip != 0 || pattrs != 0;
}



int MmsSession::reconnectOutOfBand(const int operationID)
{  
  // This is invoked presumably in a service thread to suspend an ongoing voice
  // operation, suspend the IP session, reestablish the IP session using new
  // remote attributes (IP/port, coder), and resume the voice operation. This
  // is invoked only when a voice operation is in progress. It is assumed that
  // this session's suspendTerminationLock has been set prior to arrival here.
  // That lock prevents the voice command logic from continuing should the voice
  // operation terminate while we are executing the reconnect. If no voice 
  // operation is ongoing, we will not arrive here, as the reconnect will instead
  // be handled elsewhere as a normal connect command. Note that the the parameter
  // map passed to this method is the parameter flatmap for the connect command,
  // the session->flatmap() being the parameter map for the active voice command.

  // Check for attempt to reconnect while another reconnect is pending
  if (this->isReconnect()) return MMS_ERROR_CONNECTION_BUSY;

  MmsSession::Op* operation = this->findByOpID(operationID);
  if (operation == NULL) return MMS_ERROR_NO_SUCH_OPERATION;
  char* flatmap = operation->flatmap();
  if (flatmap == NULL)   return MMS_ERROR_NO_SUCH_OPERATION;
   
  char* conxID=0, *ipAddr=0, *pport=0, *premoteattrs=0, *plocalattrs=0;
  unsigned int remoteattrs=0, localattrs=0;
  int iplen=0, result=0, length=0;

  MmsFlatMapReader map(flatmap);                                            
  iplen  = map.find(MMSP_IP_ADDRESS,  &ipAddr);
  length = map.find(MMSP_PORT_NUMBER, &pport);
  length = map.find(MMSP_REMOTE_CONX_ATTRIBUTES, &premoteattrs);
  length = map.find(MMSP_LOCAL_CONX_ATTRIBUTES,  &plocalattrs);
  if (0 != (result = this->editIpAddress(ipAddr))) return result;

  const int port = pport? *((int*)pport): 0;                                                                               
                                          
  MmsDeviceIP* deviceIP = this->ipDevice(); 
  const int localIpLen  = ACE_OS::strlen(deviceIP->localIP());                                             

  if (premoteattrs) remoteattrs = *((unsigned int*)premoteattrs);
  if (plocalattrs)  localattrs  = *((unsigned int*)plocalattrs);
  const unsigned int currRemoteAttrs = this->remoteIpAttrs;
                                            // Default attrs if not supplied
  this->setDefaultReconnectAttributes(remoteattrs, localattrs); 

  const int isMediaParametersChanged
      = this->isRemoteMediaChange(ipAddr, port, remoteattrs, currRemoteAttrs);

  if  (isMediaParametersChanged)
  {                                         // If IP/port or other change ...
       this->holdRemoteSession(operation);  // ... suspend remote session, and                                      
                                            // ... restart remote session
       result = deviceIP->start(ipAddr, port, remoteattrs, localattrs, EV_SYNC);    
  }     
  else operation->putMapHeader(setReasonCode, MMS_REASON_NO_CHANGE);                        
                                            
  *((int*)(pport)) = deviceIP->localPort(); // Return local IP/port as usual   
  ACE_OS::memset(ipAddr, 0, MMS_SIZEOF_IPADDRESS);   
  ACE_OS::memcpy(ipAddr, deviceIP->localIP(), min(localIpLen, iplen-1));  

  if (isMediaParametersChanged)             // If we suspended voice ...
      this->resumeMediaIfReconnect();       // ... resume voice operation   
                                            // If we suspended conference ...
  if (operation->isConferenceOperation())   // 424
      this->reconnectToConference(NULL);    // ... resume conference
             
  return result;                            // Return 0 if OK, -1 otherwise
}



int MmsSession::resumeMediaIfReconnect()
{
  // Relisten to voice media (if any) after a reconnect operation
  // Returns 1 if relistened; 0 if not relistened but OK; -1 if listen error

  // Ensure we wait to reconnect to conference if voice operation terminates
  ACE_Guard<ACE_Thread_Mutex> x(this->slock); 
  if (this->reconnectInfo == NULL) return 0;

  const int result = this->reconnectInfo->relisten();

  delete this->reconnectInfo;
  this->reconnectInfo = NULL;

  return result;
} 



MmsSession::ListenDirection MmsSession::getListenDirectionByCommandID(const int commandID)
{
  switch(commandID)
  {
    case COMMANDTYPE_PLAY:
    case COMMANDTYPE_PLAYTONE:
         return MmsSession::ListenDirection::FULLDUPLEX;
  }

  return MmsSession::ListenDirection::HALFDUPLEX;
} 
