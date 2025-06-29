// 
// mmsTimerHandler.cpp 
//
// Timer manager. Assume a max of 60 timers per handler, (since Win version  
// is based on WaitForMultipleObjects, which has a limit of 61 handles.
//
#include "StdAfx.h"
#include "mmsTimerHandler.h"

#ifdef MMS_ENABLE_MEMORY_LEAK_DETECTION
#define new MMS_MEMLEAK_NEW
#endif // MMS_ENABLE_MEMORY_LEAK_DETECTION



int MmsTimerHandler::handleMessage(MmsMsg* msg)             
{ 
  switch(msg->type())
  { case MMSM_INITTASK:
         parent->postMessage(MMSM_REGISTER_TIMERS, (unsigned long)this);
         break;

    case MMSM_SHUTDOWN:
         this->isShutdownRequest = TRUE;
         this->postMessage(MMSM_QUIT);
         break;

    default: return 0;
  } 

  return 1;
}
   
  
                                            // Set a timer
int MmsTimerHandler::setTimer(MmsTask* notify, int mmsTimerID, 
  const MmsTime& duration, const MmsTime& repeat, const int iscallback)    
{                                           // Allocate instance data 
  MmsTimerData* timerData = new MmsTimerData;     
  timerData->taskToNotify = notify;
  timerData->mmsTimerID   = mmsTimerID;
  if  (repeat != Mms::NOTIME)               // If a repeating timer ...
       timerData->markRepeatable();         // ... mark data persistent
  if  (iscallback)                          // If sink to be notified 
       timerData->markForCallback();        // synchronously, so indicate

  // MMSLOG((LM_DEBUG,"%s setting timer %d for %d\n",taskName,mmsTimerID,duration));

  timerData->aceTimerID                     // Set timer
     = this->reaktor->schedule_timer(this, timerData, duration, repeat);

  if  (timerData->aceTimerID  == -1)        // 60 timers max -- if we ran out  
       delete timerData;                    // delete data we allocated above
                                             
  return timerData->aceTimerID;             // return -1 if no can do
}


                                            // Cancel specified timer
int MmsTimerHandler::cancelTimer(int timerID)
{                                           // Retrieve timer instance data
  MmsTimerData* timerData = NULL;           // Result is a timer ID, or -1
  int result = this->reaktor->cancel_timer(timerID, (const void **)&timerData);

  if  (MmsTimerData::isValid(timerData))    // If data returned ... 
       delete timerData;                    // delete timer's attached data

  return result;
}


                                            // Timer expiration callback
int MmsTimerHandler::handle_timeout(const MmsTime&, const void *arg)
{ 
  MmsTimerData* args = (MmsTimerData*)arg;  // Retrieve timer instance data

  // static unsigned seq;                   // Debug logging  
  // MMSLOG((LM_DEBUG,"TIM1 timeout on timer %d (%d)\n",args->mmsTimerID, ++seq));
                                             
  if  (MmsTimerData::isValid(args) && args->taskToNotify) 
  {                                         // Notify client of timer firing
       // if  (args->mmsTimerID == 101)     // Debug logging
       //      MMSLOG((LM_DEBUG,"TIM1 timer 2 fired\n"));

       if  (args->isCallback())             // Synchronously? (not used in mms fyi)
            args->taskToNotify->sendMessage(MMSM_TIMER, args->mmsTimerID);
                                            // Asynchronously
       else args->taskToNotify->postMessage(MMSM_TIMER, args->mmsTimerID);

       if  (args->isOneShot())              // If not a repeating timer ...
            delete args;                    // delete timer's instance data
  }
         
  // ACE_OS::thr_yield();                   // Removed in cume 2.4    
  return 0;                                 // fyi -1 would exit event loop
}



MmsTimerHandler::MmsTimerHandler(MmsTask::InitialParams* p): MmsReactiveTask(p)
{ 
  this->isShutdownRequest = 0;
}


                                          
