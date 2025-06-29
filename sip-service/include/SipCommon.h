/**
 * $Id: H323Common.h 15948 2005-11-22 14:13:09Z marascio $
 *
 * Common global definitions and includes.
 */

#ifndef SIP_COMMON_H
#define SIP_COMMON_H

#ifdef WIN32
#   pragma warning(disable : 4786) // Identifier was truncated in debug information
#endif // WIN32

#include <iostream>
#include <sstream>

/* * * * * * * * * * * * * * * * * * * * * * * * * * * * *
 * Common resiprocate includes
 */

/* * * * * * * * * * * * * * * * * * * * * * * * * * * * *
 * Common ACE includes.
 */
#include "ace/OS.h"
#include "ace/ACE.h"
#include "ace/Task.h"
#include "ace/Synch.h"
#include "ace/Time_Value.h"


#include "logclient/logclient.h"
using namespace Metreos::LogClient;

#define SIPLOG(X) do { LogServerClient::Instance()->WriteLog X; } while(0)

#define MSG_HANDLER_TRACE(handler, method, msg) \
{ \
	ostringstream os; \
	os <<(handler) <<"::" <<(method) <<": " <<(msg); \
	LogServerClient::Instance()->LogFormattedMsg(Log_Verbose, os.str().c_str()); \
}

/* * * * * * * * * * * * * * * * * * * * * * * * * * * * *
 * Microsoft CRT memory leak detection.  Memory leak
 * detection is compiled in or compiled out. To turn on 
 * memory leak detection define "H323_MEM_LEAK_DETECTION" 
 * and rebuild the solution.  Run the execution and when
 * the process terminates a list of possible leaks
 * will be printed to the debug window.  Use DebugView
 * to view the leak output.
 */
//#define H323_MEM_LEAK_DETECTION
#ifdef WIN32
#ifdef SIP_MEM_LEAK_DETECTION
#   define _CRTDBG_MAP_ALLOC
#   include <stdlib.h>
#   include <crtdbg.h>
#endif
#endif


/* * * * * * * * * * * * * * * * * * * * * * * * * * * * *
 * Macros used to gather timing statistics around various
 * portions of the code.  These are compiled in or compiled
 * out based on whether "GATHER_TIMING_STATS" is defined.
 * To use these macros, do the following in code:
 * 
 *   STAT_BEGIN(someStatisticName);
 *   ... do something that takes time ...
 *   STAT_END(someStatisticName);
 *   STAT("My statistic is: %d", someStatisticName);
 *
 * That will generate a log file with the result of the timing
 * statistic.
 */
//#define GATHER_TIMING_STATS
#ifdef GATHER_TIMING_STATS
#   define STAT_BEGIN(X)                                \
        ACE_Time_Value X_time = ACE_OS::gettimeofday(); \
        long X;                                         \
        long X_startMs = X_time.msec();

#   define STAT_END(X)                                  \
        X_time = ACE_OS::gettimeofday();                \
        X = X_time.msec() - X_startMs;                  

#   define STAT(X)    do { LogServerClient::Instance()->WriteLog X; } while(0)
#else
#   define STAT_BEGIN(X)
#   define STAT_END(X)
#   define STAT(X)
#endif


namespace Metreos
{

namespace Sip
{

static int CanDeref(void* p)
{            
    int result = 0;

    if (p)  
    {
        try  
        {
            char value = *(char*)p;
            result = 1;
        }
        catch(...)  { }
    }

    return result;
}


/* * * * * * * * * * * * * * * * * * * * * * * * * * * * *
 * Possible call end reasons that the application server
 * expects.  These are translated from OpenH323 call end
 * reasons.
 */
#define OTHER_OR_UNKNOWN                    0
#define NORMAL_CALL_CLEARING                1
#define NO_ANSWER                           2
#define REMOTE_BUSY                         3
#define UNREACHABLE                         4
#define ATTEMPT_FAILOVER                    5


/* * * * * * * * * * * * * * * * * * * * * * * * * * * * *
 * Possible return values for answer call responses.
 */
#define ANSWER_CALL_NOW                     0
#define ANSWER_CALL_PENDING                 1
#define ANSWER_CALL_DENIED                  2


} // namespace Sip
} // namespace Metreos

#endif // SIP_COMMON_H