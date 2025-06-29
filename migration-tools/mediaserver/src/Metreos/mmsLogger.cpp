//
// mmsLogger.cpp 
//
// Logger object; logs to any of various installable destinations
// 
//      
// Let's agree to not make any calls to the logger from the logger, that is,
// don't use any MMSLOG, MMSDEBUG, ACE_ERROR, etc, from this context.  
// We could call the logger recursively, but let's not.
//
// We need to be able to check the logrec to determine if the message
// is only for the console, so we do not print it to log
//
// Removal of logging destinations is not yet supported. To implement this
// we'll want to do a suspend and wait, since locking the destination at
// every use is too costly simply to allow for an action that is rarely done. 
//
// A way we might handle resource-intensive network logging is to attach an
// additional task to the logger to handle just these requests. The task will
// of course have multiple threads itself. This task would be inherent in the
// MmsLogDestination, instantiated in the constructor, although it should be
// a member of the (global scoped) logger class. In this way the destination's
// printToDestination() method will queue the log message onto the attached
// task queue, rather than rendering it directly.
// 
// We do not defer to the destinations for log record formatting because
// (a) we need the timestamp and priority from the log record, and (b) we 
// don't want to allocate and queue a 4K log record when we can queue a   
// formatted log message which averages about one percent of that.
// Should we need to do formatting at the destination, say for xml network
// transmission, we'll just chop off the header in the destination object. 
//
// Note that the log record flags, specifying verbosity and priority, are 
// specific to the thread! So the flags we check here are those set for the
// logger task. We can optionally filter messages by priority at the client 
// thread, so we can dynamically adjust log content for a thread or thread
// group. There is an additional indicator in MmsTask::InitialParams,
// indicating if we wish to filter log messages at the thread, and if so,
// what priority we'll filter at. 
//
// If the client thread generating the log record is configured to receive 
// the callback from ACE logging, it filters the log record based upon its 
// own msg level setting. If the log record does not pass the filter tests,
// the log record is discarded and it never arrives here. If it passes the
// client's filter, the client masks its own message level on top of the
// one in the log record, and forwards the log record here (logger.log()).
// Here we test the log record message level for that overlay. If present,
// we know the log record has been pre-filtered. If on the other hand, there
// is no overlay present, we assume we were called back directly from 
// ACE_Log_Msg, and we therefore filter the log record here.
//
// Punt: If a threaded logger is not desirable, we can revert to: (a) 
// derive MmsLogger from ACE_Log_Msg_Callback only; (b) lose handleMessage(), 
// and the postMessage, and (c) print directly from log(), in which case we 
// can allocate the message buffer on the stack, there would be no need to 
// new it in that case. We'd lose the handling of log output in threads, but
// we'd retain most of the logger structure, including destinations etc.
// In fact we could even retain the current task-based logger class, we 
// would just not activate() it.
// - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
#include "StdAfx.h"
#include "mmsLogger.h"

#ifdef MMS_ENABLE_MEMORY_LEAK_DETECTION
#define new MMS_MEMLEAK_NEW
#endif // MMS_ENABLE_MEMORY_LEAK_DETECTION
      

void MmsLogger::log(ACE_Log_Record& logrec)  
{ 
  // This is the final callback from ACE_Log_Msg. Each task has the opportunity
  // to set its own such callback, in which case it will filter the log message
  // and forward the callback to here if it passes the local filter tests. This
  // permits an individual MMS task to filter at a different threshold than that   
  // of the general population. If the thread generating the message has pre-
  // filtered the message, that thread will have so indicated by overlaying its 
  // own threshold onto the log message's priority level. 
  
  if  (this->isSuspended()) return;         // All logging currently suspended?
                                            // Test if pre-filtered 
  int  logRecordMsgLvl   =  logrec.type() & 0xffff;       
  int  preFilteredMsgLvl = (logrec.type() & 0xffff0000) >> 16;                
                                            
  if  (preFilteredMsgLvl == 0)              // Overlay present? 
  {                                         // No, filter the message now  
       if  (logRecordMsgLvl < this->currentMsgLevel)
            return;                         // Discarding message if indicated
  }                                      
  else logrec.type(logRecordMsgLvl);        // Yes, remove overlay                                                                
                                            // Log record has passed filter:
                                            // Log unformatted message to                                           
  if  (this->inlineDestinationCount > 0)    // console, debugger, etc.
       this->print(logrec.msg_data(), logrec.msg_data_len(), 
                   this->inlineDestinations, MMS_MAX_INLINE_DESTINATIONS);
                                            // If no threaded destinations ...
  if  (this->threadDestinationCount == 0)   // .. we're done
       return;                              
                                              
  ACE_TCHAR* buf = new ACE_TCHAR[MMS_MAXLOGRECSIZE+1]; 
                                             
  int len = this->formatLogMsg(logrec, ACE_Log_Msg::instance()->flags(), buf); 
  len++;                                    // Queue formatted log message for 
                                            // handling by a logger thread
  this->postMessage(MMSM_LOG, len, buf, len);
}



int MmsLogger::handleMessage(MmsMsg* msg)             
{   
  switch(msg->type())
  { 
    case MMSM_LOG:
         onLog(msg);
         break;  

    case MMSM_ENABLE:
         switch(msg->param())
         { case MMS_LOGMSG_TIMESTAMPS:      // Enable timestamping of log recs
                ACE_Log_Msg::instance()->set_flags(ACE_Log_Msg::VERBOSE_LITE);
                break;
         }
         break;  

    case MMSM_DISABLE:
         switch(msg->param())
         { case MMS_LOGMSG_TIMESTAMPS:      // Disable timestamping of log recs
                ACE_Log_Msg::instance()->clr_flags
               (ACE_Log_Msg::VERBOSE | ACE_Log_Msg::VERBOSE_LITE);
                break;
         }
         break; 

    case MMSM_CYCLE:
         this->cycle((const MmsTask*)msg->param());
         break; 

    case MMSM_FLUSH:
         this->flushLog((const MmsTask*)msg->param());
         break; 

    default: return 0;
  } 

  return 1;
}

 // --- enable next line for debugging ---
 // #define MMS_POINTER_VALIDATION_ENABLED


inline void MmsLogger::onLog(MmsMsg* msg)   // MMSM_LOG handler  
{ 
  if  (this->isSuspended()) return; 
  ACE_TCHAR* msgText = msg->base();
  if  (msgText == NULL) return;
 
  #ifdef MMS_POINTER_VALIDATION_ENABLED
  #ifdef MMS_WINPLATFORM                    // FYI IsBadReadPtr is thread safe
  if  (IsBadReadPtr(msgText, msg->param())) // param() is length incl nullterm
  {    ACE_OS::printf("LOGX >>> bad pointer at MmsLogger::onLog\n");
       return;
  }
  #endif
  #endif
                                             
  this->print(msgText, msg->param(),        // Print log message to destination
              threadDestinations, MMS_MAX_THREAD_DESTINATIONS);
  // #if 0                                  // delete msg->base() appears to
  delete[] msgText;                         // work either if we do it here,    
  msg->reset();                             // or if we wait for msg.release()
  // #endif                                 // However check for leak anyway
}
   


int MmsLogger::print                        // Output log rec to destination(s)
( const char* buf, const int buflen, MmsLogDestination** dests, const int count)
{ 
  // Output this log record to whatever destinations are active in the specified
  // destination array. This is executed both in main thread context (for inline 
  // destinations) and in logger thread context (for thread destinations)

  MmsLogDestination** desti = dests;        
                 
  for(int i=0; i < count; i++, desti++)      // For each possible destination               
  { MmsLogDestination* dest = *desti; 
    if (dest)                                // if destination[i] installed ...
        dest->out(buf, (const void*)buflen); // ... output msg to destination 
  }                

  return 0; 
} 
  
   
                                             // Ctor
MmsLogger::MmsLogger(MmsTask::InitialParams* p): MmsBasicTask(p),
  inlineDestinationCount(0), threadDestinationCount(0), suspended(0)  
{
  this->config = (MmsConfig*)p->config;

  memset(this->inlineDestinations, 0, sizeof(this->inlineDestinations));
  memset(this->threadDestinations, 0, sizeof(this->threadDestinations));

  if (p->length == sizeof(MmsLogger::LoggerParams))
  {
      MmsLogger::LoggerParams* params = (MmsLogger::LoggerParams*)p;

      if  (params->isTimestamped)
           ACE_Log_Msg::instance()->set_flags(ACE_Log_Msg::VERBOSE_LITE); 
      else ACE_Log_Msg::instance()->clr_flags
               (ACE_Log_Msg::VERBOSE | ACE_Log_Msg::VERBOSE_LITE);

      this->insertInitialDestinations(params);
  }
}



MmsLogger::~MmsLogger() 
{ 
  for(int i=0; i < MMS_MAX_INLINE_DESTINATIONS; i++)
      if  (inlineDestinations[i]) delete inlineDestinations[i]; 

  for(int j=0; j < MMS_MAX_THREAD_DESTINATIONS; j++)
      if  (threadDestinations[j]) delete threadDestinations[j]; 
}



int MmsLogger::formatLogMsg(ACE_Log_Record& logrec, unsigned long flags, ACE_TCHAR* buf) 
{ 
  // This is distilled from ACE_Log_Record.format_msg(). We removed the verbose 
  // formatting, removed message priority name, etc, but primarily we removed 
  // it from the log record since we don't want to pass a 4K log record in a 
  // MMSM_LOG message, but rather we want to format the message early and put  
  // the considerably shorter formatted message into the MMSM_LOG message. 

  static char* maskunstamped = ACE_LIB_TEXT("%s");
  static char* masktimestamp = ACE_LIB_TEXT("%s.%03ld");
  static char* maskstamped   = ACE_LIB_TEXT("%s(%d) %s");
  ACE_TCHAR*   logrecPayload =(ACE_TCHAR*)logrec.msg_data();
   
  #define MMS_FIXED_LOGKEYSIZE 24
  #define MMS_MAX_LOGPAYLOADSIZE (MMS_MAXLOGRECSIZE - MMS_FIXED_LOGKEYSIZE)

  // Ensure no overflow on log message buffer 3/5/2007
  if (strlen(logrecPayload) > MMS_MAX_LOGPAYLOADSIZE)
  {
    *(logrecPayload + (MMS_MAX_LOGPAYLOADSIZE-1)) = '\n';
    *(logrecPayload + MMS_MAX_LOGPAYLOADSIZE) = '\0';
  }


  ACE_TCHAR timestamp[26]; // 0123456789012345678901234 
  int  length = 0;         // Oct 18 14:25:36.000 2003<nul>
                           // flags = ACE_Log_Msg::instance()->flags()
  if  (0 == (flags & (ACE_Log_Msg::VERBOSE | ACE_Log_Msg::VERBOSE_LITE)))
  {
       length = ACE_OS::sprintf(buf, maskunstamped, logrecPayload);
  }
  else
  {    time_t now = logrec.time_stamp().sec();
       ACE_TCHAR ctp[26];  
       ACE_OS::ctime_r(&now, ctp, sizeof ctp);
                           // 01234567890123456789012345 
                           // Wed Oct 18 14:25:36 1989n0 
       ctp[19] = '\0';     // terminate after time
       ctp[24] = '\0';     // terminate after date
       ACE_OS::sprintf(timestamp, masktimestamp,      
               ctp+4, ((long)logrec.time_stamp().usec())/1000);  

       length = ACE_OS::sprintf(buf, maskstamped,
               timestamp, lmToMsgPriority(logrec.type()), logrecPayload);
  }

  return length;
}



void MmsLogger::insertInitialDestinations(MmsLogger::LoggerParams* params)
{
  if  (params->destinationFlags & MmsLogDestination::STDOUT)
       this->insertStdoutDestination();

  if  (params->destinationFlags & MmsLogDestination::DEBUG)
       this->insertSystemDebuggerDestination(); 

  if  (params->destinationFlags & MmsLogDestination::LOCALFILE)
       this->insertLocalFileDestination
            (params->path, params->append, params->isFullPath);

  if  (params->destinationFlags & MmsLogDestination::SOCKET)
       this->insertSocketDestination(params->remoteport); 

  if  (params->destinationFlags & MmsLogDestination::LOGSERVER)
       this->insertLogServerDestination(); 
} 



HDESTINATION MmsLogger::insertDestination(MmsLogDestination* destination)
{
  return destination == NULL? NULL:
         destination->isInline()? insertInlineDestination(destination):
         insertThreadDestination(destination);
} 



inline HDESTINATION MmsLogger::insertInlineDestination(MmsLogDestination* dest)
{
  ACE_ASSERT(dest);// Add a destination handled inline (not by a logger thread)
  if (this->inlineDestinationCount >= MMS_MAX_INLINE_DESTINATIONS) return NULL;
  inlineDestinations[inlineDestinationCount++] = dest; 
  return dest;  
} 


                       
inline HDESTINATION MmsLogger::insertThreadDestination(MmsLogDestination* dest)
{
  ACE_ASSERT(dest); // Add a destination to be handled by logger threads
  if (this->threadDestinationCount >= MMS_MAX_THREAD_DESTINATIONS) return NULL;
  threadDestinations[threadDestinationCount++] = dest; 
  return dest;  
} 

       
                                            // Convenience method
HDESTINATION MmsLogger::insertStdoutDestination()
{                                            
  MmsLogDestination* stdoutDestination = new MmsLogDestinationStdout;
  HDESTINATION hDestination = this->insertInlineDestination(stdoutDestination);
  if  (hDestination == NULL) 
  {   if (stdoutDestination) delete stdoutDestination; 
      stdoutDestination = NULL;  
      ACE_OS::printf("LOGX could not install console logger\n"); 
  }

  return hDestination;
}


                                            // Convenience method
HDESTINATION MmsLogger::insertSystemDebuggerDestination()
{                                           
  MmsLogDestination* debugDestination = new MmsLogDestinationSystemDebugger;
  HDESTINATION hDestination = this->insertInlineDestination(debugDestination);
  if  (hDestination)
       ACE_OS::printf("LOGD debug logger installed\n");
  else
  {    if (debugDestination) delete debugDestination; 
       debugDestination = NULL;
       ACE_OS::printf("LOGX could not install debug logger\n");
  }

  return hDestination;
}


                                            // Convenience method
HDESTINATION MmsLogger::insertLocalFileDestination
(ACE_TCHAR* filename, const int append, const int isFullPath) 
{
  HDESTINATION hDestination = NULL;
  MmsLogDestination* printDestination      // Create file destination
      = new MmsLogDestinationLocalFile(config, filename, append, isFullPath);
                                           // File opened OK?
  if  (printDestination && printDestination->isOpen())
       hDestination = this->insertThreadDestination(printDestination);

  if  (hDestination)
       ACE_OS::printf("LOGX print logger installed\n");
  else
  {    if (printDestination) delete printDestination;
       printDestination = NULL;  
       ACE_OS::printf("LOGX could not install print logger\n");
  }

  return hDestination;
}


                                            // Convenience method
HDESTINATION MmsLogger::insertSocketDestination(const int port) 
{
  HDESTINATION hDestination = NULL;
  MmsLogDestination* socketDestination      // Instantiate/open destination
      = new MmsLogDestinationSocket(config, port);
                                            // Socket opened OK?
  if  (socketDestination && socketDestination->isOpen())
       hDestination = this->insertThreadDestination(socketDestination);

  if  (hDestination)
       ACE_OS::printf("LOGX tcp logger installed\n");
  else
  {    if  (socketDestination) delete socketDestination; 
       socketDestination = NULL; 
       ACE_OS::printf("LOGX could not install tcp logger\n");
  }

  return hDestination;
}



HDESTINATION MmsLogger::insertLogServerDestination()
{                                            
  MmsLogDestination* logServerDestination = new MmsLogDestinationLogServer;
  HDESTINATION hDestination = this->insertInlineDestination(logServerDestination);
  if  (hDestination == NULL) 
  {    if (logServerDestination) delete logServerDestination;
       logServerDestination = NULL;
       ACE_OS::printf("LOGX could not install log server logger\n"); 
  }

  return hDestination;
}



int MmsLogger::suspendDestination(HDESTINATION dest) 
{                                           // Suspend logging on destination
  ACE_reinterpret_cast(MmsLogDestination*, dest)->disable();
  return 0;
}



int MmsLogger::resumeDestination(HDESTINATION dest) 
{                                           // Resume logging on destination
  ACE_reinterpret_cast(MmsLogDestination*, dest)->enable();
  return 0;
}



HDESTINATION MmsLogger::stdoutDestination()
{                                           // Return stdout destination handle   
  return this->findByDestinationType(MmsLogDestination::STDOUT,         
         inlineDestinations, MMS_MAX_INLINE_DESTINATIONS);
}



HDESTINATION MmsLogger::systemDebuggerDestinationDestination()
{                                           // Return debug destination handle
  return this->findByDestinationType(MmsLogDestination::DEBUG,         
         inlineDestinations, MMS_MAX_INLINE_DESTINATIONS);
}



HDESTINATION MmsLogger::localFileDestination(HDESTINATION hSkip) 
{                                           // Return file destination handle
  return this->findByDestinationType(MmsLogDestination::LOCALFILE,          
         threadDestinations, MMS_MAX_THREAD_DESTINATIONS, hSkip);
}



HDESTINATION MmsLogger::socketDestination()
{                                           // Return debug destination handle
  return this->findByDestinationType(MmsLogDestination::SOCKET,         
         threadDestinations, MMS_MAX_THREAD_DESTINATIONS);
}



HDESTINATION MmsLogger::logServerDestination()
{                                           // Return debug destination handle
  return this->findByDestinationType(MmsLogDestination::LOGSERVER,         
         threadDestinations, MMS_MAX_THREAD_DESTINATIONS);
}


                                           
HDESTINATION MmsLogger::findByDestinationType(const int type, 
  MmsLogDestination** dests, const int count, const MmsLogDestination* skip)
{
  HDESTINATION hReturn = NULL;              // Find destination of specified 
                                            // type and return its handle
  for(int i=0; i < count; i++)
  {   MmsLogDestination* dest = dests[i];
      if  (dest == NULL) continue;  
      if  (dest == skip) continue;
      if  (dest->type() != type) continue;
      hReturn = (HDESTINATION)dest;
      break;
  }

  return hReturn;
}



void MmsLogger::cycle(const MmsTask* caller)
{
  HDESTINATION localPrintDestination = this->localFileDestination();
  if  (localPrintDestination)  
       localPrintDestination->cycle(caller);
} 

void MmsLogger::flushLog(const MmsTask* caller)
{
  HDESTINATION localPrintDestination = this->localFileDestination();
  if  (localPrintDestination)  
       localPrintDestination->flushLog(caller);
} 
