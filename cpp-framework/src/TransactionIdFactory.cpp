#include "stdafx.h"

#include "ace/Log_Msg.h"
#include "ace/Synch.h"
#include "TransactionIdFactory.h"

using namespace Metreos;

TransactionIdFactory::TransactionIdFactory() : 
    m_nextId(0)
{
}

TransactionIdFactory::~TransactionIdFactory()
{
}

int TransactionIdFactory::GetId()
{
    /**
     * Is there a more efficient way of doing this?
     * I've looked for an atomic integer equivalent
     * in ACE but couldn't find one.
     */

    ACE_Guard<ACE_Thread_Mutex> guard(m_nextIdLock);

    unsigned int id = m_nextId++;

    guard.release();

    ACE_ASSERT(id >= 0);

    return id;
}