//
// mmsTask.h 
//
#ifndef MMS_TASK_H
#define MMS_TASK_H
#ifdef MMS_WINPLATFORM
#pragma once
#endif
#include "mms.h"   
#include "ace/Sched_Params.h"
#include "ace/Reactor_Notification_Strategy.h"
#include "ace/Log_Msg_Callback.h"
#include "mmsMsg.h"
                                             
#ifdef  MMS_USING_MESSAGE_FACTORY            
#include "mmsMsgFactory.h"          
#define MMSMSG_RELEASE(x) mmsMsgFactory::instance()->release(x)
#else
                                            // If using a message factory to  
#define MMSMSG_RELEASE(x) x->releasex()     // allocate MmsMsgs, we also use 
                                            // it to release them
#endif

#define MMS_DEFAULT_TASK_NAME "Mms task thread"
#define mmsForceMsgLevelInRange(n) do{if(n>LM_MAX)n=LM_MAX;else if(n<1)n=1;}while(0)




class MmsTask: public ACE_Task<ACE_MT_SYNCH>, public ACE_Log_Msg_Callback
//-----------------------------------------------------------------------------
// MmsTask: Message-driven thread (base class)
//-----------------------------------------------------------------------------
// Thread class paired with a synchronized message queue (of MmsMsg messages). 
// Its postMessage/sendMessage are THE methods of inter-thread communication.
// MmsTask::start() starts the thread, starts the thread's message loop, and
// posts an MMSM_INITTASK message to the queue. The message loop runs until a
// MMSM_QUIT message is received.
// 
// Derived base classes are distinguished by the manner in which they implement
// a thread event loop, in each case using ACE_Task::svc() to do so. 
//
// Implementations must implement handleMessage() which will look similar to:
//
// int handleMessage(MmsMsg* msg)           // Handle a message
// {
//   switch(msg->type())
//   { case MMSM_INITTASK:
//          doSomething();
//          break;
//     case SOME_OTHER_MESSAGE:
//          onSomeOtherMessage();
//          break; 
//     case MMSM_QUIT:
//          dontReallyNeedToHandleThis();
//          break;
//     default: return 0;                   // Indicate not handled
//   }                                          
//   return 1;                              // Indicate handled
// }
//
// If the implementation handles the message it must return 1. If zero is
// instead returned, MmsTask invokes the default handler for that message.
//
// Constructor accepts a pointer to a MmsTask::InitialParams structure, which
// informs MmsTask of the parent MmsTask* (if any), an arbitrary group number
// (used if the parent wishes to utilize Task Manager thread group methods),
// and an ID ordinal within the group. Subclasses may derive from this struct
// if additional info is to be supplied to the derived class.
// Subclasses may override close() if they wish to take some action after the
// message loop shuts down but before the destructor. To start task thread(s) 
// instantiate the task and invoke it's start() method.
//
// Messaging methods:
// . postMessage(messagetype [, messageparam, timeout])
//     Allocates MmsMsg of specified type and posts it to end of queue
//     Example: this->postMessage(MMSM_QUIT);  
// . postMessage(messageType, messageParam, payload ,payload length[,timeout])
//     As above, and including arbitrary reference-counted payload 
//     Example: parent->postMessage(MMSM_DATA, 0, "foo");
// . sendMessage(messagetype [, messageparam, timeout])
//     Synchronous message. Calls handleMessage() directly. Note that this
//     blocks; also note that MMSM_QUIT should be posted, not sent, otherwise
//     the message pump will cannot see the message to shut itself down.
// . putq(message, messagetype {messageparam, timeout])
//     Post a previously-allocated message to end of queue 
//     Example: MmsMsg* msg = new MmsMsg; this->putq(msg, MMSM_QUIT);
// . getq([timeout])
//     Gets next message in queue, returning MmsMsg* or NULL
//     Example: MmsMsg* msg = getq(TIMEOUT_IMMED);
// - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
{ public: 
                                                                                                       
  // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
  // Overridables executed in thread context 
  // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -                                               
  virtual int handleMessage(MmsMsg* msg)=0; // Implementation message handler  
                                           
  virtual int postHandleMessages();         // Run from message loop after queue

  virtual int defHandleMessage(MmsMsg* msg);// Default message handler  

  virtual int onThreadStarted() {return 0;} // Thread startup hook


  // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
  // Message queueing (instantiator context)
  // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
                                            // Alloc msg and post to this queue
  int postMessage(unsigned int type, long param=0, MmsTime* timeout=TIMEOUT_BLOCK);
                                            // Alloc msg and post to this queue
  int postMessage(unsigned int type, long param, const char* payload, int len=0, 
      MmsTime* timeout = TIMEOUT_BLOCK); 
                                            // Alloc & send msg direct (block)
  int sendMessage(unsigned int type, long param=0);
                                            // Alloc & send msg direct (block)
  int sendMessage(unsigned int type, long param, const char* payload, int len=0); 
                                            // Post existing msg to this queue
  int putq(MmsMsg* msg, unsigned int type, long param=0, MmsTime* timeout=TIMEOUT_BLOCK); 
                                            // Ditto
  int putq(MmsMsg* msg, MmsTime* timeout=TIMEOUT_BLOCK); 
                                            // Task hook invoked after putq
  virtual int postputq(MmsMsg* msg, const int result) { return result; }      
                                            // Get msg from this thread's queue
  MmsMsg* getq(MmsTime* timeout=TIMEOUT_BLOCK);                                              
                                       
  
  // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
  // Other public methods etc (instantiator context)
  // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
  int start();                              // Start this task's thread
                  
  struct InitialParams                      // MmsTask constructor parameters
  {                                         // Implementations must set actual 
    int length;                             // length of derived struct used 
    int groupID;                            // Thread manager group
    int taskID;                             // Thread ordinal within group
    int threadCount;                        // # of task threads default 1
    int relativePriority;                   // Relative thread priority 
    int baselinePriority;                   // OS-specific 'normal' priority
    int isDetached;                         // If false, joinable (waitable)
    int defaultMsgLevel;                    // Thread's log filtering threshold 
    int currentMsgLevel;                    // Thread's log filtering threshold
    int isThreadFiltersLogMsgs;             // Thread filters logging activity?
    MmsTask* parent;                        // Handle to creator object
    MmsTask* reporter;                      // Message handle to alarms/stats
    void*    config;                        // Configuration object
    ACE_Reactor* reactor;                   // Optional reactor
    ACE_Log_Msg_Callback* logCallback;      // Optional ACE logging callback
    enum {TASKNAMEMAX = 16,};               // Max length of taskName+nullterm
    ACE_TCHAR taskName[TASKNAMEMAX];        // Name appearing in log(null-term)
    unsigned long user;                     // Available to subclasses
    enum threadPriority {DEFAULT, LOWEST, LOW, BELOWNORMAL, NORMAL, ABOVENORMAL, HIGH};

    InitialParams(const int tid, int gid, MmsTask* p) 
    { memset(this, 0, sizeof(InitialParams));
      taskID = tid; groupID = gid; parent = p; 
      length = sizeof(InitialParams);
    }
  };

  virtual void log(ACE_Log_Record&);        // ACE logging callback

  static MmsTime* TIMEOUT_IMMED;            // Constant: do not block
  static MmsTime* TIMEOUT_NEVER;            // Constant: block up to forever
  static MmsTime* TIMEOUT_BLOCK;            // Constant: block up to forever

  MmsTask(InitialParams* params);           // Ctor

  virtual ~MmsTask();                       // Dtor

  //  
  // Binding a reactor to the message queue causes an event to be fired to the  
  // specified reactor when a message arrives in this task's queue. Doing so 
  // permits blocking on reactor.handle_events as opposed to polling the queue 
  // since queue activity now wakes up the thread and calls an event handler.
  //                                         
  int bindReactorToMessageQueue
     (ACE_Reactor* reactor, ACE_Reactor_Mask mask = ACE_Event_Handler::READ_MASK);

  int getOsPriority() { return this->osPriority;}

  static int getMinPriority();  // OS-specific lowest thread priority value   
 
  static int getMaxPriority();  // OS-specific highest thread priority value  

  static int getNextHigherPriority(const int currentPriority); 
  // Returns the OS-specific thread priority which is one priority higher
  // than the specified priority. If none, the supplied priority is returned.
 
  static int getNextLowerPriority(const int currentPriority); 
  // Returns the OS-specific thread priority which is one priority lower
  // than the specified priority. If none, the supplied priority is returned.
  // ACE_Thread_Descriptor* threadInfo() {return thr_mgr()->thread_desc_self();}

  int threads() { return this->threadCount; }

  char* name()  { return this->taskName; }

  protected:                                 
                                             
  virtual int open(void*);                  // Thread startup initialization
                                            // Default thread exit hook
  virtual int close(unsigned long) { return 0; }    

  void setThreadInfo();                     // Set internal thread info
                                            // Get priority relative to baseline
  unsigned long getOsSpecificThreadPriority(const int baselinePriority);
                                            // Inline message alloaction
  MmsMsg* allocMsg(unsigned int type, long param);
  MmsMsg* allocMsg(unsigned int type, long param, const char* payload, const int len);


  enum biflags{THREAD_DETACHED = 1, LOGFILTER_LOCAL = 2,};
   
  int   taskID;                             // Ordinal within thread group
  int   relativeThreadPriority;             // Relative to baseline priority 
  int   osPriority;                         // Specific initial thread priority 
  int   threadCount;                        // Threads per task (default 1) 
  int   currentMsgLevel;                    // Task log msg severity threshold
  int   defaultMsgLevel;                    // Task log msg severity threshold
  int   flags;                              // Whether detached or joinable
  ACE_hthread_t threadInternalHandle;       // OS's handle to the thread

  MmsTask*     parent;                      // Parent thread
  ACE_Reactor* reaktor;                     // Event loop
  MmsTime      quiesce;                     // Sleep time between queue checks
  ACE_Log_Msg_Callback* logCallback;        // Logging callback object
                                            // Binding of queue to reactor
  ACE_Reactor_Notification_Strategy* reactorNotificationBinding;
  
  char taskName[InitialParams::TASKNAMEMAX];// Task name for log purposes
 
  MmsTask() { }                             // Disallow default ctor
};




class MmsBasicTask: public MmsTask
//-----------------------------------------------------------------------------
// MmsBasicTask: Message-driven thread (base class)
//-----------------------------------------------------------------------------
// An MmsTask whose thread runs an event loop which continually polls the task
// message queue. See MmsTask for the bulk of the class documentation. 
// - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
{ protected:
  virtual int svc();                        // Event loop    

  MmsBasicTask() { }                        // Disallow default ctor

  public:
  MmsBasicTask(MmsTask::InitialParams* params): MmsTask(params) { }   

  virtual ~MmsBasicTask() {}
};



class MmsReactiveTask: public MmsTask
//-----------------------------------------------------------------------------
// MmsReactiveTask: Message-driven reactor-based thread (base class)
//-----------------------------------------------------------------------------
// An MmsTask whose thread runs an event loop which continually polls the 
// task's dedicated event registry (ACE reactor). When a message arrives in   
// the queue, an event is fired which unblocks the event loop and calls the 
// handle_input callback. When a MMSM_QUIT message is recognized, the event 
// loop is terminated. Implementors will generally provide additional event  
// handler callbacks, such as the timer expiration hook. 
// IMPORTANT: A MmsReactive task will not start on its own, you must send it
// a MMSM_START or some other message, in order to get it to handle the
// initial MMSM_INITTASK in its queue. The task will not handle MMSM_START, 
// it merely wakes up the reactor so that it will check its message queue.
// - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
{ protected:

  virtual int svc();                        // Event loop

  virtual int handle_input(ACE_HANDLE);     // Queue activity callback
                                              
  MmsReactiveTask() { }                     // Disallow default ctor

  public:                                   // ctor
  MmsReactiveTask(MmsTask::InitialParams* params): MmsTask(params)
  { 
    ACE_ASSERT(this->reaktor);
  }

  virtual MmsReactiveTask::~MmsReactiveTask() { } 

  virtual int preHandleMessages(ACE_HANDLE) // handle_input hook -- return
  { return 0;                               // -1 to prevent running msg loop
  }
};



#endif
