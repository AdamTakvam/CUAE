/* -*- C++ -*- */
/**
 * @file Thread_Semaphore.cpp
 *
 * Thread_Semaphore.cpp,v 4.5 2003/12/18 22:56:52 dhinton Exp
 *
 * Originally in Synch.cpp
 *
 * @author Douglas C. Schmidt <schmidt@cs.wustl.edu>
 */

#include "ace/Thread_Semaphore.h"

#if defined (ACE_HAS_THREADS)

#if !defined (__ACE_INLINE__)
#include "ace/Thread_Semaphore.inl"
#endif /* __ACE_INLINE__ */

#include "ace/ACE.h"

ACE_RCSID(ace, Thread_Semaphore, "Thread_Semaphore.cpp,v 4.5 2003/12/18 22:56:52 dhinton Exp")


void
ACE_Thread_Semaphore::dump (void) const
{
#if defined (ACE_HAS_DUMP)
// ACE_TRACE ("ACE_Thread_Semaphore::dump");

  ACE_Semaphore::dump ();
#endif /* ACE_HAS_DUMP */
}

ACE_Thread_Semaphore::ACE_Thread_Semaphore (u_int count,
                                            const ACE_TCHAR *name,
                                            void *arg,
                                            int max)
  : ACE_Semaphore (count, USYNC_THREAD, name, arg, max)
{
// ACE_TRACE ("ACE_Thread_Semaphore::ACE_Thread_Semaphore");
}

/*****************************************************************************/

ACE_Thread_Semaphore *
ACE_Malloc_Lock_Adapter_T<ACE_Thread_Semaphore>::operator () (const ACE_TCHAR *name)
{
  ACE_Thread_Semaphore *p = 0;
  if (name == 0)
    ACE_NEW_RETURN (p, ACE_Thread_Semaphore (1, name), 0);
  else
    ACE_NEW_RETURN (p, ACE_Thread_Semaphore (1, ACE::basename (name,
                                                               ACE_DIRECTORY_SEPARATOR_CHAR)),
                    0);
  return p;
}

#endif /* ACE_HAS_THREADS */
