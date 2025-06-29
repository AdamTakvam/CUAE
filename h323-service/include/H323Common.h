/**
 * $Id: H323Common.h 19103 2006-01-04 21:57:35Z jdliau $
 *
 * Common global definitions and includes.
 */

#ifndef H323_COMMON_H
#define H323_COMMON_H

#ifdef WIN32
#   pragma warning(disable : 4786) // Identifier was truncated in debug information
#endif // WIN32

#include <iostream>
#include <sstream>

/* * * * * * * * * * * * * * * * * * * * * * * * * * * * *
 * Common OpenH323 and PWLib includes.
 */
#include "ptlib.h"
#include "h323.h"
#include "h323pdu.h"
#include "h323neg.h"
#include "h261codec.h"


/* * * * * * * * * * * * * * * * * * * * * * * * * * * * *
 * OpenH323 just defined these, but ACE is also going to.
 * To avoid collisions, undef them because we want the ACE ones.
 * This is ugly, but I can't think of a pretty, simple solution.
 */
#undef EINPROGRESS
#undef ENOTSOCK
#undef EMSGSIZE
#undef ESOCKTNOSUPPORT
#undef EOPNOTSUPP
#undef EPFNOSUPPORT
#undef EAFNOSUPPORT
#undef EADDRINUSE
#undef EADDRNOTAVAIL
#undef ENETDOWN
#undef ENETUNREACH
#undef ENETRESET
#undef ECONNABORTED
#undef ECONNRESET
#undef ENOBUFS
#undef EISCONN
#undef ENOTCONN
#undef ESHUTDOWN
#undef ETOOMANYREFS
#undef ETIMEDOUT
#undef ECONNREFUSED
#undef EHOSTDOWN
#undef EHOSTUNREACH


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

extern LogServerClient* logger;

//#define H323LOG(X) do { logger->WriteLog X; } while(0)


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
#ifdef H323_MEM_LEAK_DETECTION
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

namespace H323
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


/* * * * * * * * * * * * * * * * * * * * * * * * * * * * *
 * Translate OpenH323 call end reasons into Metreos call
 * end reasons.
 */
static int GetCallEndedReasonFromH323Reason(H323Connection::CallEndReason reason)
{
    switch(reason)
    {
        case H323Connection::CallEndReason::EndedByLocalUser:
        case H323Connection::CallEndReason::EndedByRemoteUser:
        case H323Connection::CallEndReason::EndedByCallerAbort:
            return NORMAL_CALL_CLEARING;

        case H323Connection::CallEndReason::EndedByNoAnswer:
        case H323Connection::CallEndReason::EndedByAnswerDenied:
            return NO_ANSWER;
        
        case H323Connection::CallEndReason::EndedByRemoteBusy:
            return REMOTE_BUSY;

        case H323Connection::CallEndReason::EndedByUnreachable:
        case H323Connection::CallEndReason::EndedByLocalCongestion:
        case H323Connection::CallEndReason::EndedByNoBandwidth:
        case H323Connection::CallEndReason::EndedByNoUser:
        case H323Connection::CallEndReason::EndedByRemoteCongestion:
            return UNREACHABLE;

        case H323Connection::CallEndReason::EndedByRefusal:
        case H323Connection::CallEndReason::EndedByConnectFail:
        case H323Connection::CallEndReason::EndedByNoEndPoint:
        case H323Connection::CallEndReason::EndedByHostOffline:
            return ATTEMPT_FAILOVER;

        default:
            return OTHER_OR_UNKNOWN;
    }
}


/* * * * * * * * * * * * * * * * * * * * * * * * * * * * *
 * Translate from an integer into the appropriate OpenH323
 * answer call response as defined by the enumeration
 * H323Connection::AnswerCallResponse.
 */
static H323Connection::AnswerCallResponse 
GetH323AnswerCallResultFromInt(unsigned int answerCallResult)
{
    switch(answerCallResult)
    {
    case ANSWER_CALL_NOW:
        return H323Connection::AnswerCallResponse::AnswerCallNow;

    case ANSWER_CALL_DENIED:
        return H323Connection::AnswerCallResponse::AnswerCallDenied;

    case ANSWER_CALL_PENDING:
        return H323Connection::AnswerCallResponse::AnswerCallPending;

    default:
        return H323Connection::AnswerCallResponse::AnswerCallDenied;
    }
}

} // namespace H323
} // namespace Metreos

#endif // H323_COMMON_H