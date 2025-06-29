//
// MmsSession.cpp
//
// Session operation object async event handlers
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
#include "mmsCommandTypes.h"
static char *cplay="play",*crecord="record",*ctone="tone",*ccst="cst";
static char *cdef="ERR",*emptystr="",*cstream="stream";

#ifdef MMS_ENABLE_MEMORY_LEAK_DETECTION
#define new MMS_MEMLEAK_NEW
#endif // MMS_ENABLE_MEMORY_LEAK_DETECTION



int MmsSession::Op::handleEventVoice(MmsEventRegistry::DispatchMap* dispatchMap) 
{ 
  char* et = cdef;
  const static int TEC_STREAM_ = 0xe0;

  switch(dispatchMap->eventType)
  { case TDX_PLAY:    et = cplay;   break;
    case TDX_RECORD:  et = crecord; break;
    case TDX_PLAYTONE:et = ctone;   break;
    case TEC_STREAM_: et = cstream; break;
  } 

  if (-1 == this->onEventServiceStart(et)) return MMS_ERROR_EVENT_UNKNOWN;

  // This barrier prevents this voice media command from completing during 
  // a concurrent out-of-band command such as a reconnect (IP/port change).
  // Note that the session.flags | MMS_SESSION_FLAG_EVENT_FIRED
  // flag is set before we arrive here, and MMS_SESSION_FLAG_HANDLING_EVENT
  // is not yet set. session.isTerminationEventBlocked() tests this.
  ACE_Guard<ACE_Thread_Mutex> x(session->suspendTerminationLock); 

  // If during such an operation we closed the operation, return now
  if ((this->opid != dispatchMap->operationID) || !session) return 0;

  // If during such an operation we canceled the event, return now
  if (this->wasEventCanceled()) return 0;
 
  session->flags |= MMS_SESSION_FLAG_HANDLING_EVENT;

  MmsDeviceVoice* deviceVoice = this->assignedVoiceDevice();
  if (!deviceVoice) return MMS_ERROR_DEVICE;

  deviceVoice->isChannelBusy = FALSE;
                                             
  this->setElapsedTimeReturn(deviceVoice);  // Set media elapsed time
                                              
  deviceVoice->iottReset();                 // Close play/record files 
                                            
  this->setTermReasonReturn(deviceVoice);   // Retrieve terminating conditions
  int result = 0;  

  // If we were playing to conferee, hook IP back up to conference.
  // This activity will wait on the session.slock mutex, so certain activites
  // cannot occur concurrently, such as conference teardown and relisten voice                                              
  if  (session->isConferenceParty())            
       result = session->reconnectToConference(deviceVoice);
  else
  if  (session->isUtilitySession())         // If playing to conference ...
       deviceVoice->unlisten();             // ... unhook voice from conference  

  if  (Config()->serverParams.crashServerOnVoiceEvent)
       Mms::crashServer();                  // Generate hard crash for testing
            
  return result; 
}



int MmsSession::Op::handleEventVoxDigitsReceived(MmsEventRegistry::DispatchMap* dispatchMap)
{
  // Event service for TDX_GETDIG event. We'll return the collected digits 
  // to adapter via the parameter map.

  if (-1 == this->onEventServiceStart("digits")) return MMS_ERROR_EVENT_UNKNOWN;

  // This barrier prevents this voice media command from completing during a
  // concurrent out-of-band command such as a reconnect (IP/port change).
  ACE_Guard<ACE_Thread_Mutex> x(session->suspendTerminationLock); 

  // If during such an operation we closed the operation, return now
  if ((this->opid != dispatchMap->operationID) || !session) return 0;

  // If during such an operation we canceled the event, return now
  if (this->wasEventCanceled()) return 0;
   
  session->flags |= MMS_SESSION_FLAG_HANDLING_EVENT; 

  MmsDeviceVoice* deviceVoice = this->assignedVoiceDevice();
  if (!deviceVoice) return MMS_ERROR_DEVICE;
  deviceVoice->isChannelBusy = FALSE;
  const int sessionID = this->sID();

  this->setTermReasonReturn(deviceVoice);  

  int returndigitcount = 0;
  MMS_DV_TPT_LIST terminationConditions;
  DV_TPT* digType = NULL;

  int conditionCount = this->getTerminationConditionParameters(deviceVoice, terminationConditions); 
  if (conditionCount)
  {
    if (deviceVoice->terminationReason() == TM_MAXDTMF)
    {        
        digType = this->session->findTerminationCondition(terminationConditions, DX_MAXDTMF);
        if (digType) // Return only the number of digits defined in tp_length
            returndigitcount = digType->tp_length;
    }
    else if (deviceVoice->terminationReason() == TM_DIGIT)
    {        
        digType = this->session->findTerminationCondition(terminationConditions, DX_DIGTYPE);
        if (digType)
        {   // Return number of digits up to last termination digit
            char termDigit = (char)digType->tp_length;
            char *p = ACE_OS::strrchr(this->session->digitCache, termDigit);
            returndigitcount = p? (int)(p - this->session->digitCache + 1): 0;
        }
    }
  }

  this->setDigitsReceivedReturn(deviceVoice, returndigitcount); 

                                            // Diagnostics: log digits
  if (Config()->diagnostics.flags & MMS_DIAG_LOG_DIGBUF)   
  {
      char* p = deviceVoice->checkUserDigitBuffer() > 0? 
                deviceVoice->digitBuffer(): emptystr;

      char* q = this->session->digitCacheSize() > 0 ? 
                this->session->digitCache : emptystr;

      MMSLOG((LM_DEBUG,"POOL session %d op %d vox%d digits '%s' cache '%s'\n", 
              sessionID, opid, deviceVoice->handle(), p, q));
  }
           
  this->session->clearDigitCache(returndigitcount);
  deviceVoice->clearDigitBuffer();   

  if (!Mms::isFlatmapReferenced(flatmap(),30, sessionID, opid)) return -1;
  
  // This should be all we need to do to tear down a receiveDigits command 
  // on a party in conference. When this is the case, we unlisten voice from
  // the IP. Conference is still listening to IP, so we do not want to do
  // the normal voiceResourceIdle at onCommandEnd, which would unlisten
  // both ends, Idling the vox here will short circuit that. Ideally,
  // voiceResourceIdle would know whether it should unlisten both ends,  
  // or just the one, so let's keep that in mind as an improvement.  

  if (session->isConferenceParty())
  {
      deviceVoice->unlisten();  
      // Note that pre-2.2 we idled the voice resource here. We no longer idle
      // resources -- the operation will end and the resource will be released.
  }

  return 0;
}



int MmsSession::Op::handleEventStartMedia(MmsEventRegistry::DispatchMap*) 
{ 
  // If we're doing the connection asynchronously, we'll get this event on
  // completion of the IP start media (same as setRemoteMediaInfo).  
  // We'll then hook the IP up to a conference if requested.
 
  if (-1 == this->onEventServiceStart("start IP") || !session) 
      return MMS_ERROR_EVENT_UNKNOWN;
   
  session->flags |=  MMS_SESSION_FLAG_HANDLING_EVENT;
  session->flags &= ~MMS_SESSION_FLAG_IS_ASYNC_CONNECT_PENDING;
                                              
  if (session->flags & MMS_SESSION_FLAG_RETRANSMIT_CONNECTION)  
  {                                          
      if (-1 == session->selfListen(this->flatmap()))    
          return MMS_ERROR_DEVICE;          // Self-listen connection
  }        
                                              
  if (this->isConferenceOperation())
  {                                         // Join conference                          
      if (-1 == session->handleConferenceConnect(this->parameterMap))
          return MMS_ERROR_RESOURCE_UNAVAILABLE;
  }
  
  return 0; 
} 


                                             
int MmsSession::Op::handleEventCallStateTransition(MmsEventRegistry::DispatchMap* dispatchMap)  
{     
  // Note: we may want to think about not assigning call state transition 
  // events to a thread, handling them inline instead. We should monitor 
  // how frequent the silence on/off transitions turn out to be.

  if (!session || !session->callStateMon) return MMS_ERROR_STATE;
  if (-1 == this->onEventServiceStart("cst", FALSE)) return MMS_ERROR_EVENT_UNKNOWN;  
  session->flags |= MMS_SESSION_FLAG_HANDLING_EVENT; 

  const int result = session->callStateMon->onStateTransition(dispatchMap); 

  return result == 0? MMS_COMMAND_WAITING: result;
} 



int MmsSession::Op::handleCstTimeExpired()    
{      
  // Act on expiration of a call state transition timer.
  // This is invoked mainline, not from a service thread. The expired timer was 
  // a one-shot so it is not necessary to kill the timer here. The CST command 
  // is terminated by session manager on receipt of this event

  if (!session || !session->callStateMon) return MMS_ERROR_STATE;

  MMSLOG((LM_INFO,"%s cst wait expires\n", logkey)); 

  return session->callStateMon->unregisterStateTransitions();
}



int MmsSession::Op::handleEventIpDigitsReceived(MmsEventRegistry::DispatchMap*)
{ 
  // We'll implement this later. HMP 1.0 does not support it. We'll need to 
  // collect one digit at a time here, then once when we get server command  
  // which results in an ip->stop(STOP_RECEIVE_DIGITS), we'll trap the 
  // resulting event, IPM_DIGITS_RECEIVED. At that event handler we'll 
  // return the collected digits as we do for vox. 

  return MMS_ERROR_NO_SUCH_COMMAND;  
} 



int MmsSession::Op::handleEventIpStopped(MmsEventRegistry::DispatchMap* dispatchMap) 
{ 
  return MMS_ERROR_NO_SUCH_COMMAND;
}



int MmsSession::handleRfc2833SignalReceived
( MmsEventRegistry::DispatchMap* dispatchMap, MmsTask* serverMgr)
{
  // This barrier prevents this voice media command from completing during 
  // a concurrent out-of-band command such as a reconnect (IP/port change).
  // Note that the session.flags | MMS_SESSION_FLAG_EVENT_FIRED
  // flag is set before we arrive here, and MMS_SESSION_FLAG_HANDLING_EVENT
  // is not yet set. session.isTerminationEventBlocked() tests this.
  ACE_Guard<ACE_Thread_Mutex> x(this->suspendTerminationLock); 

  this->flags |= MMS_SESSION_FLAG_HANDLING_EVENT;

  if (dispatchMap->returnDataLength < 1 || dispatchMap->returnData == NULL) 
      return MMS_ERROR_ASYNC_EVENT;  

  const int rfc2833digit 
   = MmsDeviceIP::getDigitFromRfc2833EventData(dispatchMap->returnData);
  
  if (rfc2833digit >= 0)     // If a valid RFC2833 event, notify appserver            
      this->postPushRfc2833Digit(serverMgr, rfc2833digit);

  return 0;
}



int MmsSession::postPushRfc2833Digit(MmsTask* serverMgr, const int digit)
{
  // Wrap up the digit and client routing GUID and push RFC2833 event to client
  if (!serverMgr || !this->cmdHeader || !this->routingGuid) return -1;

  MmsFlatMapWriter map(256);
  const int len = ACE_OS::strlen(this->routingGuid)+1;
  map.insert(MMSP_ROUTING_GUID, MmsFlatMap::STRING, len, this->routingGuid);

  this->cmdHeader->param = digit; // Send digit in param field of map header
  const int extraHeaderSize = sizeof(MmsServerCmdHeader);
  const int mapbufsize = map.length() + extraHeaderSize;                                             
  char* flatmap = new char[mapbufsize];

  map.marshal(flatmap, extraHeaderSize, (char*)this->cmdHeader);

  return serverMgr->postMessage(MMSM_SERVERPUSH, (long)flatmap);
}



int MmsSession::Op::wasEventCanceled()   
{
  // Check if another object has canceled event handling for the current voice
  // media termination event. This could be the case if we barged the media
  // operation and returned a userstop result in the barging handler itself.
  if(!(this->flags & MMS_OP_FLAG_EVENT_HANDLED)) return FALSE;
  this->flags    &= ~MMS_OP_FLAG_EVENT_HANDLED; 
  return TRUE;
}


