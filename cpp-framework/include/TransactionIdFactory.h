#ifndef TRANSACTION_ID_FACTORY_H
#define TRANSACTION_ID_FACTORY_H

#include "cpp-core.h"
#include "ace/Thread_Manager.h"

namespace Metreos
{

/**
 * class TransactionIdFactory
 * 
 * Simple, thread-safe mechanism for generating an
 * ordered sequence of integers.  When the maximum
 * value for an unsigned int is reached, the next
 * transaction ID automatically wraps to 0.
 */
class CPPCORE_API TransactionIdFactory
{
public:
    TransactionIdFactory();
    virtual ~TransactionIdFactory();

    int GetId();
    
protected:
    ACE_Thread_Mutex    m_nextIdLock;
    unsigned int        m_nextId;
};

} // namespace Metreos

#endif // TRANSACTION_ID_FACTORY_H