// -*- C++ -*-

//==========================================================================
/**
 *  @file    Thread_Semaphore.h
 *
 *  Thread_Semaphore.h,v 4.4 2003/12/18 22:56:52 dhinton Exp
 *
 *   Moved from Synch.h.
 *
 *  @author Douglas C. Schmidt <schmidt@cs.wustl.edu>
 */
//==========================================================================

#ifndef ACE_THREAD_SEMAPHORE_H
#define ACE_THREAD_SEMAPHORE_H
#include /**/ "ace/pre.h"

#include "ace/ACE_export.h"

#if !defined (ACE_LACKS_PRAGMA_ONCE)
# pragma once
#endif /* ACE_LACKS_PRAGMA_ONCE */

#if !defined (ACE_HAS_THREADS)
#  include "ace/Null_Semaphore.h"
#else /* ACE_HAS_THREADS */
// ACE platform supports some form of threading.

#include "ace/Semaphore.h"

/**
 * @class ACE_Thread_Semaphore
 *
 * @brief Wrapper for Dijkstra style general semaphores that work
 * only within one process.
 */
class ACE_Export ACE_Thread_Semaphore : public ACE_Semaphore
{
public:
  /// Initialize the semaphore, with an initial value of <count>,
  /// maximum value of <max>, and unlocked by default.
  ACE_Thread_Semaphore (unsigned int count = 1, // By default make this unlocked.
                        const ACE_TCHAR *name = 0,
                        void * = 0,
                        int max = 0x7FFFFFFF);

  /// Default dtor.
  ~ACE_Thread_Semaphore (void);

  /// Dump the state of an object.
  void dump (void) const;

  /// Declare the dynamic allocation hooks.
  ACE_ALLOC_HOOK_DECLARE;
};

/*****************************************************************************/

template <class T> class ACE_Malloc_Lock_Adapter_T;

/**
 * @class ACE_Malloc_Lock_Adapter_T<ACE_Thread_Semaphore>
 *
 * @brief Template specialization of ACE_Malloc_Lock_Adapter_T for
 * ACE_Thread_Semaphore.
 *
 * This is needed since the ctor for ACE_Thread_Semaphore doesn't match
 * the standard form used by other lock strategy classes.
 */
ACE_TEMPLATE_SPECIALIZATION
class ACE_Export ACE_Malloc_Lock_Adapter_T<ACE_Thread_Semaphore>
{
public:
  ACE_Thread_Semaphore * operator () (const ACE_TCHAR *name);
};

#if defined (__ACE_INLINE__)
#include "ace/Thread_Semaphore.inl"
#endif /* __ACE_INLINE__ */

#endif /* !ACE_HAS_THREADS */

#include /**/ "ace/post.h"
#endif /* ACE_THREAD_SEMAPHORE_H */
