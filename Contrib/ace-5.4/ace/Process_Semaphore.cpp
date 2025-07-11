// Process_Semaphore.cpp,v 4.5 2003/12/18 22:56:51 dhinton Exp

#include "ace/Process_Semaphore.h"
#include "ace/Log_Msg.h"

#if !defined (__ACE_INLINE__)
#include "ace/Process_Semaphore.inl"
#endif /* __ACE_INLINE__ */

#include "ace/ACE.h"

ACE_RCSID(ace, Process_Semaphore, "Process_Semaphore.cpp,v 4.5 2003/12/18 22:56:51 dhinton Exp")

void
ACE_Process_Semaphore::dump (void) const
{
#if defined (ACE_HAS_DUMP)
// ACE_TRACE ("ACE_Process_Semaphore::dump");
  ACE_DEBUG ((LM_DEBUG, ACE_BEGIN_DUMP, this));
  this->lock_.dump ();
  ACE_DEBUG ((LM_DEBUG, ACE_END_DUMP));
#endif /* ACE_HAS_DUMP */
}

ACE_Process_Semaphore::ACE_Process_Semaphore (u_int count,
                                              const ACE_TCHAR *name,
                                              void *arg,
                                              int max)
#if defined (ACE_WIN32) || defined (ACE_HAS_POSIX_SEM) || defined (ACE_PSOS)
  : lock_ (count, USYNC_PROCESS, name, arg, max)
#else
  : lock_ (name, ACE_SV_Semaphore_Complex::ACE_CREATE, count)
#endif /* ACE_WIN32 || ACE_HAS_POSIX_SEM || ACE_PSOS */
{
  arg = arg;
  max = max;
// ACE_TRACE ("ACE_Process_Semaphore::ACE_Process_Semaphore");
}

ACE_Process_Semaphore::~ACE_Process_Semaphore (void)
{
  // ACE_TRACE ("ACE_Process_Semaphore::~ACE_Process_Semaphore");
}

// Explicitly destroy the semaphore.

int
ACE_Process_Semaphore::remove (void)
{
// ACE_TRACE ("ACE_Process_Semaphore::remove");
  return this->lock_.remove ();
}

// Block the thread until the semaphore count becomes
// greater than 0, then decrement it.

int
ACE_Process_Semaphore::acquire (void)
{
// ACE_TRACE ("ACE_Process_Semaphore::acquire");
  return this->lock_.acquire ();
}

// Conditionally decrement the semaphore if count is greater
// than 0 (i.e., won't block).

int
ACE_Process_Semaphore::tryacquire (void)
{
// ACE_TRACE ("ACE_Process_Semaphore::tryacquire");
  return this->lock_.tryacquire ();
}

// Increment the semaphore, potentially unblocking
// a waiting thread.

int
ACE_Process_Semaphore::release (void)
{
// ACE_TRACE ("ACE_Process_Semaphore::release");
  return this->lock_.release ();
}

/*****************************************************************************/

ACE_Process_Semaphore *
ACE_Malloc_Lock_Adapter_T<ACE_Process_Semaphore>::operator () (const ACE_TCHAR *name)
{
  ACE_Process_Semaphore *p = 0;
  if (name == 0)
    ACE_NEW_RETURN (p, ACE_Process_Semaphore (1, name), 0);
  else
    ACE_NEW_RETURN (p, ACE_Process_Semaphore (1, ACE::basename (name,
                                                                ACE_DIRECTORY_SEPARATOR_CHAR)),
                    0);
  return p;
}

//
// These are instantiated both with and without ACE_HAS_THREADS.
//
#if defined (ACE_HAS_EXPLICIT_TEMPLATE_INSTANTIATION)

// template class ACE_Guard<ACE_Process_Semaphore>;

#elif defined (ACE_HAS_TEMPLATE_INSTANTIATION_PRAGMA)

// #pragma instantiate ACE_Guard<ACE_Process_Semaphore>

#endif /* ACE_HAS_EXPLICIT_TEMPLATE_INSTANTIATION */
