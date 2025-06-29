//
// mmsTask.cpp: message-driven thread (base class) 
//
#include "StdAfx.h"
#include "mmsTask.h"
#include "mmsLogger.h"

#ifdef MMS_ENABLE_MEMORY_LEAK_DETECTION
#define new MMS_MEMLEAK_NEW
#endif // MMS_ENABLE_MEMORY_LEAK_DETECTION

MmsTime* MmsTask::TIMEOUT_IMMED = &(MmsTime(0));
MmsTime* MmsTask::TIMEOUT_NEVER = NULL;
MmsTime* MmsTask::TIMEOUT_BLOCK = NULL;
 
         
                           
// - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
// MmsTask overridables executed in thread context 
// - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -  
                                                                                     
int MmsTask::defHandleMessage(MmsMsg* msg)  // Default message handler  
{
  switch(msg->type())
  { 
    case MMSM_PING:                         // Request for response
         if (msg->param())
            ((MmsTask*)msg->param())->postMessage(MMSM_PINGBACK, (long)this);
         break;

    case MMSM_SET_MSGLEVEL:                 // Set log msglevel to specific LM_
         this->currentMsgLevel              // or back to default
             = msg->param() > 0? msg->param(): this->defaultMsgLevel;
         mmsForceMsgLevelInRange(this->currentMsgLevel);
         break;                             

    case MMSM_BUMP_MSGLEVEL:                // Set msglevel relative +/- (1-11)
         this->currentMsgLevel              // Convert to ordinal & back  
             = msgPriorityToLm(lmToMsgPriority(this->currentMsgLevel) + msg->param());
         mmsForceMsgLevelInRange(this->currentMsgLevel);
         break;

    case MMSM_FILTER_LOGMESSAGES:           // Start or stop local filtering
         if  (msg->param())                 // of log messages
              this->flags |=  LOGFILTER_LOCAL;
         else this->flags &= ~LOGFILTER_LOCAL;
         break;

    case MMSM_SET_QUIESCE_INTERVAL:         // Set message loop sleep interval
         if  (msg->size() == sizeof(MmsTime))   
              this->quiesce = *((MmsTime*)msg->base());
         break;
       
    default: return 0;
  } 
 
  return 1;
}



int MmsTask::postHandleMessages()           // Run from message loop after queue
{                                           // is emptied or a batch of messages
  if  (this->quiesce == MmsTime::zero)      // has been handled. Implementor may
       ACE_OS::thr_yield();                 // override to perform tasks which
  else ACE_OS::sleep(this->quiesce);        // are not message-driven     
  return 0;                               
}


// - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
// MmsTask message queueing (instantiator context)
// - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
                                            // Post existing msg to this queue 
int MmsTask::putq(MmsMsg* msg, unsigned int type, long param, MmsTime* timeout)
{ 
  if  (!msg) return -1;
  msg->type (type);
  msg->param(param);
  return this->putq(msg, timeout);
}  

                                            // Post existing msg to this queue
int MmsTask::putq(MmsMsg* msg, MmsTime* timeout)
{ 
  if  (!msg) return -1;

  // Check if queue is full before enqueue.  
  // ACE defaults queue size (bytes) to 16KB.  If the queue overflows, svc() may stop pumping messages.
  // In case we worry about running into the problem in the future, change the LWM and HWM during creation.
  // See pcap-service for example.
  if (this->msg_queue()->is_full()) Mms::alertedTask = this->taskID;

  int result = msg->msg_priority() == ACE_DEFAULT_MESSAGE_BLOCK_PRIORITY ?
      this->msg_queue()->enqueue_tail(msg,timeout):   // Queue fifo 
      this->msg_queue()->enqueue_prio(msg,timeout);   // Queue by priority
                                                      // Indicate if queue maxed

  return this->postputq(msg, result);                 // Task implementation hook
}     
      
  
                                            
MmsMsg* MmsTask::getq(MmsTime* timeout)     // Get msg from this thread's queue
{                                         
  MmsMsg* msg = NULL;                       // Returns ptr to msg, or NULL 
  this->msg_queue()->dequeue_head((ACE_Message_Block*&)msg, timeout);

  return msg;                              
}  


                                            // Alloc msg and post to this queue
int MmsTask::postMessage(unsigned int type, long param, MmsTime* timeout)
{ 
  MmsMsg* msg = this->allocMsg(type, param);// Message block with no payload                         
                                            // Dequeue must release message
  return this->putq(msg, type, param, timeout);
} 


                                            // Alloc msg and post to this queue
int MmsTask::postMessage(unsigned int type, long param, const char* payload,  
    int len, MmsTime* timeout)
{ 
  if (payload == NULL) return this->postMessage(type, param, timeout);
                                            // Message block with payload
  MmsMsg* msg = this->allocMsg(type, param, payload, len);
                                            // Dequeue must release message
  return this->putq(msg, type, param, timeout);
} 


                                            // Alloc msg and send direct (block)
int MmsTask::sendMessage(unsigned int type, long param)
{                                            
  MmsMsg* msg = this->allocMsg(type, param);// Message block with no payload  

  int  result = handleMessage(msg);         // Handle this message
  if (!result)                              // If not handled ... 
       result = defHandleMessage(msg);      // ... handle it here

  MMSMSG_RELEASE(msg);                      // Decrement ref count; delete if 0 

  return result;
} 


                                            // Alloc msg and send direct (block)
int MmsTask::sendMessage(unsigned int type, long param, const char* payload, int len)
{ 
  MmsMsg* msg = this->allocMsg(type, param, payload, len);   

  int  result = handleMessage(msg);         // Handle this message
  if (!result)                              // If not handled ... 
       result = defHandleMessage(msg);      // ... handle it here

  MMSMSG_RELEASE(msg);                      // Decrement ref count; delete if 0 

  return result;
} 


                                            // Allocate MmsMsg obj w/o payload
inline MmsMsg* MmsTask::allocMsg(unsigned int type, long param)
{
  #ifdef MMS_USING_MESSAGE_FACTORY
  MmsMsg::MMSMSGPARAMS params(type, param);
  return mmsMsgFactory::instance()->get(&params); 
  #else
  MmsMsg* msg = new MmsMsg();
  msg->type(type);
  msg->param(param);
  return msg;
  #endif
}
   

                                            // Allocate MmsMsg obj with payload
inline MmsMsg* MmsTask::allocMsg(unsigned int type, long param, 
  const char* payload, const int len)
{
  int length = len > 0? len: ACE_OS::strlen(payload) + 1;
  #ifdef MMS_USING_MESSAGE_FACTORY
  MmsMsg::MMSMSGPARAMS params(type, param);
  return mmsMsgFactory::instance()->get(&params, payload, length);
  #else
  MmsMsg* msg = new MmsMsg(payload, length);
  msg->type(type);
  msg->param(param);
  return msg;
  #endif
}

// - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
// MmsTask other public methods etc (instantiator context)
// - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
 

MmsTask::MmsTask(InitialParams* params)     // Ctor configure task from params
{
  this->taskID  = params->taskID;           // Task ordinal within group
  this->parent  = params->parent;           // Parent thread
  this->reaktor = params->reactor;          // Optional
  this->quiesce = MmsTime(0,0);             // Sleep between message loop polls
  this->flags   = 0;                        // Bitflags
  this->reactorNotificationBinding = 0;     // No binding yet 
                                            // # of threads run by task
  this->threadCount = params->threadCount > 0? params->threadCount: 1;
                                            // Thread priority
  this->relativeThreadPriority = params->relativePriority;  
  this->osPriority  = params->baselinePriority;
                                            // Thread joinable or detached
  if  (params->isDetached) flags |= THREAD_DETACHED;
                                            // Logging configuration:
  this->logCallback = params->logCallback;  // Logger object
  this->defaultMsgLevel = params->defaultMsgLevel > 0?  
                          params->defaultMsgLevel: MMS_DEFAULT_MSGLEVEL;
  this->currentMsgLevel = params->currentMsgLevel > 0?  
                          params->currentMsgLevel: this->defaultMsgLevel;
  if  (params->isThreadFiltersLogMsgs) flags |= LOGFILTER_LOCAL;
      
  memcpy(this->taskName, *params->taskName? 
               params->taskName: MMS_DEFAULT_TASK_NAME, InitialParams::TASKNAMEMAX);
  this->taskName[InitialParams::TASKNAMEMAX-1] = '\0'; 
                                            // Thread group to suspend or wait on
  ACE_Thread_Manager::instance()->set_grp(this, params->groupID);
}



MmsTask::~MmsTask() 
{ 
  if (reactorNotificationBinding) 
  {   delete reactorNotificationBinding;
      this->msg_queue()->notification_strategy(NULL);
  }
}



int MmsTask::start()                        // Start this task's thread(s) 
{ 
  int  result  = this->open(0);           
  if  (result != -1)
       this->postMessage(MMSM_INITTASK);    // Post initial message to queue
  return result;
}    
      

                                   
int MmsTask::open(void*)                    // Thread startup initialization
{                                           // private hook
  const unsigned long aceThreadType             
      = THR_NEW_LWP |(this->flags & THREAD_DETACHED)? THR_DETACHED: THR_JOINABLE; 

  const unsigned long acePriority = getOsSpecificThreadPriority(this->osPriority);
                                            // Start thread(s) ...
  return this->activate(aceThreadType, this->threadCount, 0, acePriority);    
}



void MmsTask::setThreadInfo()               // Internal thread info intialization
{ 
  // This method is invoked by Mms tasks immediately on thread startup.                                          
  // We first retrieve the priority value that was set by the specific OS. 
  // When a thread is launched accepting a default priority, we can use this 
  // specific value as a baseline for determining OS-specific values of lesser 
  // or greater thread priorities.
  thr_mgr()->thr_self(this->threadInternalHandle);
  ACE_Thread::getprio(this->threadInternalHandle, this->osPriority); 
 
  // Setting a log callback routes the thread-specific log record created in 
  // ACE_DEBUG etc to the specified instance of ACE_Log_Msg_Callback, for
  // example MmsTask. The instance specified must have implemented callback
  // instance->log(ACE_Log_Record&). ACE logging will try to output the log 
  // message prior to the callback, so we must clear flags in the thread-local
  // log message instance to turn off all ACE log output operations.  
  // If the thread was asked at construction to filter its logging at the thread 
  // level, we set the callback to this thread, which will filter log messages
  // and forward them to the logger object if appropriate; otherwise we set
  // the callback directly to the logger. Callback is the MMS way of handling
  // logging, but if required for testing, we could not specify a callback
  // and logging from the thread would then happen inline in the normal ACE way.

  if  (this->logCallback)                   // If we were passed a callback ...
  {                                         // ... turn off ACE log output ...
       ACE_Log_Msg::instance()->clr_flags(ACE_Log_Msg::STDERR | ACE_Log_Msg::OSTREAM);
       ACE_Log_Msg_Callback* callbackObject // ... and set the callback
         =  (this->flags & LOGFILTER_LOCAL)? this: this->logCallback;
       ACE_Log_Msg::instance()->msg_callback(callbackObject);
       ACE_Log_Msg::instance()->set_flags(ACE_Log_Msg::MSG_CALLBACK);
  }
} 


                                       
int MmsTask::bindReactorToMessageQueue(ACE_Reactor* reactor, ACE_Reactor_Mask mask)
{ 
  // Binding a reactor to the message queue causes an event to be fired to the  
  // specified reactor when a message arrives in this task's queue. Doing so 
  // permits blocking on reactor.handle_events as opposed to polling the queue 
  // since queue activity now wakes up the thread and calls an event handler.
  if  (reactorNotificationBinding) delete reactorNotificationBinding;
  ACE_Reactor* thereactor = reactor == 0? this->reaktor: reactor;
  if  (this->reaktor == 0) this->reaktor = reactor;
  ACE_ASSERT(thereactor);
                                            // Initialize binding object:
  reactorNotificationBinding                // Event handler assumed 'this'
   = new ACE_Reactor_Notification_Strategy(thereactor, this, mask);
                                            // Bind queue to reactor 
  this->msg_queue()->notification_strategy(reactorNotificationBinding);
  return 0;
}



void MmsTask::log(ACE_Log_Record& logrec) 
{
  // ACE logging callback. We have stipulated that no logger calls will be made
  // in this context, that is no MMSLOG, no ACE_DEBUG, etc. This default 
  // implementation is invoked if LOGFILTER_LOCAL flag is set for the thread.
  // If that is the case, we filter the log record based on that thread's 
  // current message level setting, and forward it on to the logger if it 
  // passes filter tests, temporarily overlaying that thread's message level
  // onto the log record. If instead, LOGFILTER_LOCAL is not set, log records 
  // have been routed directly to MmsLogger.log() and do not pass though here.

  if ((this->logCallback == NULL) ||        // No callback, no need to continue
      (this->logCallback == this)) return;  // Ensure no infinite callback, 

  int  logRecordMsgLevel = logrec.type();   // Filter the log record
  if  (logRecordMsgLevel < this->currentMsgLevel)
       return;                              // Discard message if indicated 
                                            // Overlay thread's threshold
  logRecordMsgLevel |= (this->currentMsgLevel << 16);
  logrec.type(logRecordMsgLevel);           // onto log record's type member
  
  this->logCallback->log(logrec);           // Pass log record on to logger
}



int MmsTask::getMinPriority()     
{
   return ACE_Sched_Params::priority_min(ACE_SCHED_RR, ACE_SCOPE_THREAD);
}


int MmsTask::getMaxPriority()    
{ 
  return ACE_Sched_Params::priority_max(ACE_SCHED_RR, ACE_SCOPE_THREAD);
}



int MmsTask::getNextHigherPriority(const int currentPriority)  
{ 
  // Returns the OS-specific thread priority which is one priority higher
  // than the specified priority. If none, the supplied priority is returned
  int returnPriority = currentPriority, found = 0;
  ACE_Sched_Priority_Iterator priorities(ACE_SCHED_RR, ACE_SCOPE_THREAD);

  while(1)
  { int  p  = priorities.priority();  
    if  (p == currentPriority)
         found = 1;
    else
    if  (found)
    {    returnPriority = p;
         break;
    }

    if  (priorities.more())
         priorities.next();
    else break;
  }

  return returnPriority;
}



int MmsTask::getNextLowerPriority(const int currentPriority) 
{ 
  // Returns the OS-specific thread priority which is one priority lower
  // than the specified priority. If none, the supplied priority is returned.
  int returnPriority = currentPriority;
  ACE_Sched_Priority_Iterator priorities(ACE_SCHED_RR, ACE_SCOPE_THREAD);

  while(1)
  { int  p  = priorities.priority();  
    if  (p == currentPriority)
         break;
       
    returnPriority = p;
    if  (priorities.more())
         priorities.next();
    else break;
  }

   return returnPriority;
}
                             


unsigned long MmsTask::getOsSpecificThreadPriority(const int baselinePriority)
{ // Return a thread priority value specific to the OS. This is relative
  // to, and dependent upon, a baseline specific priority supplied in the 
  // ctor, presumably obtained from a thread previously created. If relative
  // priority is zero, the ACE constant THREAD_PRIORITY_NORMAL is returned,
  // which ACE recognizes and subsequently calculates the normal priority
  // for the OS (and which it seems we can't determine without first creating
  // a thread using this default, and then checking the priority value).
  // Relative priorities are obtained using an ACE object which iterates
  // over its internal set of available priority values for a particular OS.
  int  aceSpecificPriority = THREAD_PRIORITY_NORMAL;

  switch(this->relativeThreadPriority)
  { case InitialParams::ABOVENORMAL:
         aceSpecificPriority = getNextHigherPriority(this->osPriority);
         break;
    case InitialParams::BELOWNORMAL:
         aceSpecificPriority = getNextLowerPriority(this->osPriority);
         break;
    case InitialParams::HIGH:
         aceSpecificPriority = getNextHigherPriority(this->osPriority);
         aceSpecificPriority = getNextHigherPriority(aceSpecificPriority); 
         break;
    case InitialParams::LOW:
         aceSpecificPriority = getNextLowerPriority(this->osPriority);
         aceSpecificPriority = getNextLowerPriority(aceSpecificPriority);
         break;
    case InitialParams::LOWEST:
         aceSpecificPriority = getMinPriority();
         break;
  }   
 
  return aceSpecificPriority;
} 


// - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
// MmsBasicTask  
// - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

int MmsBasicTask::svc(void)                 // Thread proc                            
{ 
  setThreadInfo(); 
  if  (onThreadStarted() < 0) return 0;     // User hook
                                        
  while(1)                                  // Thread message loop:
  {  
    MmsMsg* msg = getq(TIMEOUT_BLOCK);      // Get a msg from q w/o blocking                                          
    if  (msg == NULL)
    {    if  (postHandleMessages() < 0)     // Default merely yields processor
              break;
         else continue; 
    }           
     
    int  msgtype = msg->type();            
           
    if  (handleMessage(msg));               // Handle this message
    else defHandleMessage(msg);             // If not handled, handle it here

    MMSMSG_RELEASE(msg);                    // Decrement ref count; delete if 0 

    if  (MMSM_QUIT == msgtype)              // this->flush()?
    {    if  (this->thr_count() > 1)        // If not task's last thread ...
              this->postMessage(MMSM_QUIT); // ... post a quit for next thread
         break;                             // Exit this thread on MMSM_QUIT
    }
  } 

  return 0;
}


// - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
// MmsReactiveTask event loop and event handlers
// - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

int MmsReactiveTask::svc()                  // Thread proc
{ 
  setThreadInfo(); 
  if  (onThreadStarted() < 0) return 0;     // User hook

  reaktor->owner(ACE_Thread::self());       // Must own it to handle events
                                            // Make us react to queue activity
  this->bindReactorToMessageQueue(this->reaktor);
 
  reaktor->run_reactor_event_loop();        // Event loop runs until MMSM_QUIT

  return 0;
}



                                            // Message queue activity callback
int MmsReactiveTask::handle_input(ACE_HANDLE handle)  
{     
  while(1)                                  // While messages in queue ...
  {  
    MmsMsg* msg = getq(TIMEOUT_IMMED);      // Get a msg from q w/o blocking                                          
    if  (msg == NULL)
    {    if  (postHandleMessages() < 0)     // Default merely yields processor
              reaktor->end_reactor_event_loop();
         break;
    }           
     
    int  msgtype  = msg->type();   
           
    if  (handleMessage(msg));               // Handle this message
    else defHandleMessage(msg);             // If not handled, handle it here

    MMSMSG_RELEASE(msg);                    // Decrement ref count; delete if 0 

    if  (MMSM_QUIT == msgtype)              // this->flush()?
    {    if  (this->thr_count() > 1)        // If not task's last thread ...
              this->postMessage(MMSM_QUIT); // ... post a quit for next thread
                                        
         reaktor->end_reactor_event_loop(); // Stop reactor & alert all threads
         break;                             // Exit message loop on MMSM_QUIT
    }
  }

  return 0;
}


