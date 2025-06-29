/**
 * $Id: MetreosUtilityThreadPool.h 15948 2005-11-22 14:13:09Z marascio $
 *
 * Implement a basic thread pool that knows how to handle three very specific
 * operations: answer, hangup, and set media.  These operations are implemented
 * on the thread pool because they may take several hundred milliseconds to
 * complete.
 *
 * Users of this class will invoke one of three methods that construct a
 * MetreosH323Message class and put it on the queue of the thread pool.  The 
 * thread pool is implemented by sub-classing ACE_Task.  To be useful, this
 * class should always be initialized as follows:
 *
 *    MetreosUtilityThreadPool threadPool = new MetreosUtilityThreadPool;
 *    threadPool.open();
 *    threadPool.activate(THR_NEW_LWP|THR_JOINABLE, 20);
 *
 * This constructs the MetreosUtilityThreadPool class and tells the ACE_Task
 * base class to use up to 20 threads to service messages.  If the thread
 * pool is not activated in a similiar fashion as above, ACE will only use
 * the default 1 thread to service messages resulting in less than optimal
 * behavior.
 */

#ifndef METREOS_UTILITY_THREAD_POOL_H
#define METREOS_UTILITY_THREAD_POOL_H

#ifdef H323_MEM_LEAK_DETECTION
#define DEBUG_NEW new(_NORMAL_BLOCK, __FILE__, __LINE__)
#define new DEBUG_NEW 
#endif

#include "H323Common.h" // Include this first to make sure we don't have
                        // conflicts between ACE and OpenH323.

#include "MetreosH323Message.h"

namespace Metreos
{

namespace H323
{

class MetreosH323CallState;

/**
 * class MetreosUtilityThreadPool
 *
 * Provides a a sub-class of ACE_Task that can respond to three commands
 * which may take several hundred milliseconds to complete within the
 * OpenH323 stack.
 */
class MetreosUtilityThreadPool : public ACE_Task<ACE_MT_SYNCH>
{
public:
    MetreosUtilityThreadPool();
    virtual int svc(void);

    int AddMessage(MetreosH323Message* msg);

    /**
     * Answer a call within the OpenH323 stack.
     */
    void AnswerCall(
        MetreosConnection* conx,                // Connection to answer
        const char*        displayNamePtr,      // Display name to present
        int                displayNameLen       // Length of the display name
    );


    /**
     * Hang up an active call within the OpenH323 stack.
     */
    void ClearCall(
        MetreosConnection* conx                 // Connection to hang up
    );


    /**
     * Set the media on an active connection within the OpenH323 stack.
     */
    void SetMedia(
        MetreosConnection*        conx,         // Connection to set media on
        const MetreosH323Message& h323Msg       // The original SetMedia mesage
    );

protected:
    bool AcquireAndCheckCallCancelled(MetreosH323CallState* callState);
};

} // namespace H323
} // namespace Metreos

#endif // METREOS_UTILITY_THREAD_POOL_H