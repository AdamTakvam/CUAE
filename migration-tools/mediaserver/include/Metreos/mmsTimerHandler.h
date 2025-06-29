//
// mmsTimerHandler.h
//
#ifndef MMS_TIMERHANDLER_H
#define MMS_TIMERHANDLER_H
#ifdef  MMS_WINPLATFORM
#pragma once
#endif  // MMS_WINPLATFORM
#include "StdAfx.h" 
#include "mms.h"
#include "mmsTask.h"



class MmsTimerHandler: public MmsReactiveTask
{ 
  public:

  int handleMessage(MmsMsg* msg);

  int setTimer(MmsTask* notify, int mmsTimerID, const MmsTime& duration, 
               const MmsTime& repeat = Mms::NOTIME, const int iscallback=0); 
 
  int cancelTimer(int timerID); 

  int handle_timeout(const MmsTime&, const void *arg);

  MmsTimerHandler(MmsTask::InitialParams* p); 

  virtual ~MmsTimerHandler() { }

  virtual int onThreadStarted()             // Thread startup hook
  { 
    MMSLOG((LM_INFO,"%s thread %t started at priority %d\n", taskName,osPriority)); 
    return 0;
  } 

  virtual int close(unsigned long)          // Thread exit hook
  {
    MMSLOG((LM_DEBUG, "%s thread %t exit\n",taskName));
    return 0;
  }
                                            
  protected:
  ACE_Thread_Manager* thrmgr;
  int  isShutdownRequest;

  public:

  struct MmsTimerData                       // Instance data attached to timer
  { 
    int      length;
    unsigned int signature;
    int      mmsTimerID;
    int      aceTimerID;
    unsigned int flags;
    MmsTask* taskToNotify;

    enum {MMSTIMERDATASIG = 0x1a2b3c4e, MAXDATAVALUE = 0xffff};
    enum bitflags {IS_REPEATING_TIMER=1, IS_CALLBACK=2};

    MmsTimerData() { this->clear();}
                                            // Does p point to a MmsTimerData
    static int isValid(const MmsTimerData* p)
    {
      #ifdef MMS_POINTER_VALIDATION_ENABLED
      #ifdef MMS_WINPLATFORM                     
      if  (IsBadReadPtr(p, sizeof(MmsTimerData)))  
      {    ACE_OS::printf(">>> bad MmsTimerData pointer\n");
           return 0;
      }
      #endif
      #endif

      return p > (MmsTimerData*)MAXDATAVALUE && p->length == sizeof(MmsTimerData) 
          && p->signature == MMSTIMERDATASIG;
    } 

    void clear() 
    { memset(this, 0, sizeof(MmsTimerData)); 
      length    = sizeof(MmsTimerData);
      signature = MMSTIMERDATASIG;
    }
    int  isOneShot()  {return (flags  & IS_REPEATING_TIMER) == 0;}
    void markRepeatable()     {flags |= IS_REPEATING_TIMER;}
    int  isCallback() {return (flags  & IS_CALLBACK) != 0;} 
    void markForCallback()    {flags |= IS_CALLBACK;} 
  };// MmsTimerData
};



#endif  // MMS_TIMERHANDLER_H
