//
// mmsCallStateMonitor.h  
// Attached to session, encapsulates call state transition logic
//
#ifndef MMS_CALLSTATEMON_H
#define MMS_CALLSTATEMON_H
#include "mms.h"
#include "mmsConfig.h"
#include "mmsFlatMap.h"
#include "mmsMediaEvent.h"
#include "mmsDeviceIP.h"
#include "mmsDeviceVoice.h"
#include "mmsDeviceConference.h"
#include "mmsServerCmdHeader.h"
#include "mmsMediaResourceMgr.h" 
#include "mmsTimerHandler.h"


class MmsCallStateMonitor
{
  public: 
  MmsCallStateMonitor(int st, int id, int op, int dr, 
     MmsDeviceVoice* dv, MmsTimerHandler* tm, MmsTask* mq, MmsConfig* cf);
 
  virtual ~MmsCallStateMonitor(); 
 
  int  registerStateTranstitions();

  int  unregisterStateTransitions();
  
  int  onStateTransition(MmsEventRegistry::DispatchMap* dispatchMap);  

  int  startTimer(int interval);
 
  void killTimer();
 
  int  registerCallStateEventNotification();

  int  unregisterCallStateEventNotification();

  int  CallState() { return this->callState; }
  int  EventID()   { return this->eventID;   }
  int  EventType() { return TDX_CST; }

  protected:
  MmsDeviceVoice*  deviceVoice;
  MmsTimerHandler* timerManager;
  MmsTask*         messageQueue;            // Queue (session manager) which
                                            // will receive timer notifications
  int timerID;                              // Timer manager's timer handle
  int callState;                            // Which call state is monitored
  int duration;                             // Timer expiration interval
  int sessionID;                            // 1-based session identifier
  int operationID;                          // Session operation key
  int eventID;                              // HMP ID of registered event
  DX_CST cst;                               // Event data return buffer
  MmsConfig* config;                                          
}; 

#endif
