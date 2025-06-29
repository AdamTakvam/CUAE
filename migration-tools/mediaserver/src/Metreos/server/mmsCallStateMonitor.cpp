//
// MmsCallStateMonitor.cpp
//
// Attached to session, encapsulates call state transition logic
//
#include "StdAfx.h"
#include "mmsCallStateMonitor.h"

#ifdef MMS_ENABLE_MEMORY_LEAK_DETECTION
#define new MMS_MEMLEAK_NEW
#endif // MMS_ENABLE_MEMORY_LEAK_DETECTION



MmsCallStateMonitor::MmsCallStateMonitor(int st, int id, int op, int dr, 
  MmsDeviceVoice* dv, MmsTimerHandler* tm, MmsTask* mq, MmsConfig* cf)
{ 
  this->callState    = st;
  this->sessionID    = id;
  this->operationID  = op;
  this->duration     = dr;
  this->deviceVoice  = dv; 
  this->timerManager = tm;
  this->messageQueue = mq; 
  this->config       = cf;
  this->timerID = -1; 

  this->registerStateTranstitions(); 
}



MmsCallStateMonitor::~MmsCallStateMonitor() 
{ 
  this->killTimer();
  this->unregisterStateTransitions();
}



int MmsCallStateMonitor::registerStateTranstitions()
{
  // Register for events on call state transitions, e.g. silence on, silence off

  int result = -1;

  switch(callState)
  {
    case MMS_CALL_STATE_SILENCE:
    case MMS_CALL_STATE_NONSILENCE: 

         result = deviceVoice->registerCallStatusSilence();            

         if (result == 0)
             result = registerCallStateEventNotification();
         break;
  }

  return result;
}



int MmsCallStateMonitor::unregisterStateTransitions()
{
  // Unregister for events on call state transitions

  unregisterCallStateEventNotification(); 
 
  return deviceVoice->clearCallStatus();
}



int MmsCallStateMonitor::onStateTransition(MmsEventRegistry::DispatchMap* dispatchMap)  
{                        
  // Act on a state transition event, e.g. silence on, silence off

  if (deviceVoice == NULL) return MMS_ERROR_DEVICE;

  if (dispatchMap->returnDataLength < 1 || dispatchMap->returnData == NULL) 
      return MMS_ERROR_ASYNC_EVENT;  

  // fyi: struct DX_CST { unsigned short cst_event; unsigned short cst_data; }  
  DX_CST* cst = (DX_CST*) dispatchMap->returnData;

  switch(cst->cst_event)
  {
    case DE_SILOF:                        // 3: non-silence detected 

         if  (config->diagnostics.flags & MMS_DIAG_LOG_CST)
              MMSLOG((LM_DEBUG,"CSMX SILOF on session %d\n",sessionID));

         switch(this->callState)
         {
           case MMS_CALL_STATE_SILENCE:
                this->killTimer();
                break;

           case MMS_CALL_STATE_NONSILENCE:
                this->startTimer(this->duration);
                break;
         }
         break;

    case DE_SILON:                        // 2: silence detected 

         if  (config->diagnostics.flags & MMS_DIAG_LOG_CST)
              MMSLOG((LM_DEBUG,"CSMX SILON on session %d\n",sessionID));

         switch(this->callState)
         {
           case MMS_CALL_STATE_SILENCE:
                this->startTimer(this->duration);
                break;

           case MMS_CALL_STATE_NONSILENCE:
                this->killTimer();
                break;
         }
         break;

    case DE_DIGITS:                       // 8: digit received 
         // We'll want to eventually use this for receive digits monitoring
         // as opposed to monitoring the send digits stream as we do currently
         break;

    #if(0)                                                
    case DE_LCOFF:      // loop current off 
    case DE_LCON:       // loop current on 
    case DE_LCREV:      // loop current reversal 
    case DE_RINGS:      // rings received 
    case DE_RNGOFF:     // caller hang up event    
    case DE_TONEOFF:    // tone off event 
    case DE_TONEON:     // tone on event 
    case DE_WINK:       // received a wink 
    case DX_OFFHOOK:    // offhook event 
    case DX_ONHOOK:     // onhook event 
         break;
    #endif
  }

  return 0;
}



int MmsCallStateMonitor::startTimer(int interval)
{
  // Session manager receives these timer notifications. The MMS timerID 
  // of the one-shot timer is the session ID plus an offset identifying
  // the fact that the timerID is a call state monitor timer. Thus when
  // the global timer fires we can identify the session it applies to.

  this->timerID = -1;
  MmsTime duration(0,interval*1000);

  if (this->timerManager)                 // Start the one-shot timer
      this->timerID = this->timerManager->setTimer(this->messageQueue, 
            MMS_CALLSTATE_SESSSIONID_TIMERID_OFFSET + this->sessionID, 
            duration, Mms::NOTIME, FALSE);
                                          
  return this->timerID;                   // -1 if error
} 



void MmsCallStateMonitor::killTimer()
{
  if (this->timerManager && this->timerID > 0) 
      this->timerManager->cancelTimer(this->timerID);

  this->timerID = -1;
}



int MmsCallStateMonitor::registerCallStateEventNotification()
{
  // We register to be notified of the TDX_CST event each time it occurs
  // on our voice handle, until we explicitly unregister the event.

  this->eventID = MmsEventRegistry::instance()->registerRecurringEvent
     (deviceVoice->handle(), TDX_CST, this->sessionID, 0, 
      this->messageQueue, sizeof(DX_CST), &this->cst);

  if (eventID == -1) MMSLOG((LM_ERROR,"CSMX event registration error\n"));
        
  return eventID;
}



int MmsCallStateMonitor::unregisterCallStateEventNotification()
{
  if (!eventID) return 0;

  int result = MmsEventRegistry::instance()->unregister(deviceVoice->handle(), TDX_CST);

  if (result == -1) MMSLOG((LM_ERROR,"CSMX unreg failed for TDX_CST\n"));
 
  this->eventID = 0;       
  return result;
}


 