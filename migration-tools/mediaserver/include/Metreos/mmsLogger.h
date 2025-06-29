//
// MmsLogger.h
//
// Multithreaded logger. The way this works is:
// a) ACE_Log_Msg instances are attached to each thread (yes thread, not task).
// b) A log call(MMSLOG, ACE_ERROR, etc) writes to the thread's ACE_Log_Record
// c) Ordinarily the log record is formatted and displayed/printed based upon
//    flags set for the ACE_Log_Msg instance; however ...
// d) MMS has turned off all ACE formatting and log record ouput.
// e) MMS has set the ACE_Log_Msg to pass the ACE_Log_Record to a callback,
//    which may be within the thread context, or may be in the logger. 
// f) If the former, the thread generating the log record gets the callback,
//    and filters the message based on current flag settings. If it passes
//    the filter tests, the thread forwards the log record to the logger.
//    If it does not pass the filter, for example if the log record is a
//    debug level, and we're only logging errors, the log record is discarded.
// g) The logger receives the log record at its log() callback. 
// h) The logger has installed some number of log destination objects.
//    Destinations are of two types, inline or threaded.
// i) The logger outputs to all its active inline destinations at that
//    point (console or debugger output for example). These records are
//    not formatted with a timestamp.
// j) If the logger has any installed threaded destinations, it formats
//    the log record, posts it to the logger queue in a MMSM_LOG message,
//    and returns.
// k) The logger task is running one or more threads (specified at task
//    instantiation). One of these threads picks up the MMSM_LOG off the
//    queue, and outputs the formatted log record to whatever threaded
//    destinations are active.
//
// For reference, here are the ACE log message priorities. The internal value, 
// a power of 2, is logrec.type; the external logrec.priority(), is log2(value). 
//
// Constant    Value Pri Meaning
// LM_SHUTDOWN     1   1 Not used; MMS could reassign
// LM_TRACE        2   2 Program trace info
// LM_DEBUG        4   3 Program diagnostic info
// LM_INFO         8   4 User informational
// LM_NOTICE      16   5 User action may be indicated
// LM_WARNING     32   6 User warning
// LM_STARTUP     64   7 Logger startup (not used; MMS could reassign)
// LM_ERROR      128   8 User error notice
// LM_CRITICAL   256   9 User critical eg hard device error
// LM_ALERT      512  10 User correct immediately eg corrupt database
// LM_EMERGENCY 1024  11 User panic usu broadcast to all users
// LM_MAX       1024  11 Maximum message level
// - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
#ifndef MMS_LOGGER_H
#define MMS_LOGGER_H
#ifdef MMS_WINPLATFORM
#pragma once
#endif
#include "mms.h"
#include "mmsTask.h"
#include "mmsConfig.h"
#include "mmsLogDestination.h"
                                            // Default message level used if   
#define MMS_DEFAULT_MSGLEVEL LM_INFO        // not available from config

#define MMS_MAX_INLINE_DESTINATIONS 2       // Max immediate-ouput log dests
#define MMS_MAX_THREAD_DESTINATIONS 2       // Mmax threaded-output log dests

#define MMS_LOGMSG_TIMESTAMPS 1             // MMSM_ENABLE/DISABLE parameter

#define MMS_MAXLOGRECSIZE 160               // Max characters in a log record



                                             
class MmsLogger: public MmsBasicTask        // Implements ACE_Log_Msg_Callback
{ public:                                    

  virtual void log(ACE_Log_Record&);        // ACE callback

  int  handleMessage(MmsMsg*);              // MmsTask message handler
                                            // Timestamp etc formatter
  int  formatLogMsg(ACE_Log_Record&, unsigned long flags, ACE_TCHAR *buf);
                                            // Output the message
  int  print(const char* buf, const int buflen, 
             MmsLogDestination** dests, const int count);

  MmsLogger(MmsTask::InitialParams*);

  virtual ~MmsLogger(); 

  HDESTINATION insertDestination(MmsLogDestination* destination);
  int  suspendDestination(HDESTINATION);
  int  resumeDestination (HDESTINATION);       
  int  removeDestination (HDESTINATION);    // Not implemented - locks required
  void suspendLogging()  {this->suspended = 1;}
  void resumeLogging()   {this->suspended = 0;}
  int  isSuspended()     {return this->suspended;}

  enum destinationType{INLINE, THREAD};

  HDESTINATION insertStdoutDestination();   // Convenience methods:
  HDESTINATION insertSystemDebuggerDestination();
  HDESTINATION insertLocalFileDestination
              (ACE_TCHAR* fn, const int append=0, const int isFullPath=0);
  HDESTINATION insertSocketDestination(const int port);
  HDESTINATION insertLogServerDestination();

  HDESTINATION stdoutDestination();
  HDESTINATION socketDestination();
  HDESTINATION systemDebuggerDestinationDestination();
  HDESTINATION localFileDestination(HDESTINATION h=0);
  HDESTINATION logServerDestination();
                                         
  struct LoggerParams: public MmsTask::InitialParams  
  {                                         // Parameters to constructor  
    unsigned int destinationFlags;
    int append;
    int isFullPath;
    int isTimestamped;
    int remoteport;
    ACE_TCHAR path[MAXPATHLEN]; 
    ACE_TCHAR remoteip[64];

    LoggerParams(const int t, int g, MmsTask* p): MmsTask::InitialParams(t,g,p)
    { destinationFlags = append = isFullPath = remoteport = 0; 
      memset(path, 0, sizeof(path)); memset(remoteip, 0, sizeof(remoteip)); 
      length = sizeof(LoggerParams);
    }
  };


  protected:
  MmsConfig* config; 

  void onLog(MmsMsg*);                       
  MmsLogger() {}     

  void insertInitialDestinations(MmsLogger::LoggerParams*);
  HDESTINATION insertInlineDestination(MmsLogDestination* destination);
  HDESTINATION insertThreadDestination(MmsLogDestination* destination);
  int  removeInlineDestination (HDESTINATION);
  int  removeThreadDestination (HDESTINATION);
  int  suspendInlineDestination(HDESTINATION);
  int  resumeThreadDestination (HDESTINATION);
  void cycle(const MmsTask* cycler);
  void flushLog(const MmsTask* cycler);

  HDESTINATION MmsLogger::findByDestinationType(const int type, 
  MmsLogDestination** dests, const int count, const MmsLogDestination* skip=0);

  int inlineDestinationCount, threadDestinationCount, suspended;
  MmsLogDestination* inlineDestinations[MMS_MAX_INLINE_DESTINATIONS];
  MmsLogDestination* threadDestinations[MMS_MAX_THREAD_DESTINATIONS];
};



#endif