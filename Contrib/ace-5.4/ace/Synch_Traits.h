// -*- C++ -*-

//==========================================================================
/**
 *  @file    Synch_Traits.h
 *
 *  Synch_Traits.h,v 4.2 2003/11/10 21:48:26 shuston Exp
 *
 *   Moved from Synch.h.
 *
 *  @author Douglas C. Schmidt <schmidt@cs.wustl.edu>
 */
//==========================================================================

#ifndef ACE_SYNCH_TRAITS_H
#define ACE_SYNCH_TRAITS_H
#include /**/ "ace/pre.h"

#include "ace/config-all.h"    /* Need to know threads, template settings */
#include "ace/Lock.h"

#if !defined (ACE_LACKS_PRAGMA_ONCE)
# pragma once
#endif /* ACE_LACKS_PRAGMA_ONCE */

// Forward decl
class ACE_Null_Mutex;
class ACE_Null_Condition;
class ACE_Null_Semaphore;
class ACE_Null_Mutex;
class ACE_Thread_Mutex;
class ACE_Process_Mutex;
class ACE_Recursive_Thread_Mutex;
class ACE_RW_Thread_Mutex;
class ACE_Condition_Thread_Mutex;
class ACE_Condition_Recursive_Thread_Mutex;
class ACE_Thread_Semaphore;

#if defined (ACE_HAS_TEMPLATE_TYPEDEFS)

/**
 * @class ACE_NULL_SYNCH
 *
 * @brief Implement a do nothing Synchronization wrapper that
 * typedefs the <ACE_Condition> and <ACE_Mutex> to the Null* versions.
 */
class ACE_Export ACE_NULL_SYNCH
{
public:
  typedef ACE_Null_Mutex MUTEX;
  typedef ACE_Null_Mutex NULL_MUTEX;
  typedef ACE_Null_Mutex PROCESS_MUTEX;
  typedef ACE_Null_Mutex RECURSIVE_MUTEX;
  typedef ACE_Null_Mutex RW_MUTEX;
  typedef ACE_Null_Condition CONDITION;
  typedef ACE_Null_Condition RECURSIVE_CONDITION;
  typedef ACE_Null_Semaphore SEMAPHORE;
  typedef ACE_Null_Mutex NULL_SEMAPHORE;
};

#if defined (ACE_HAS_THREADS)

class ACE_Process_Mutex;
class ACE_Condition_Recursive_Thread_Mutex;

/**
 * @class ACE_MT_SYNCH
 *
 * @brief Implement a default thread safe synchronization wrapper that
 * typedefs the <ACE_Condition> and <ACE_Mutex> to the
 * <ACE_Condition> and <ACE_Mutex> versions.  Note that this
 * should be a template, but SunC++ 4.0.1 complains about
 * this...
 */
class ACE_Export ACE_MT_SYNCH
{
public:
  typedef ACE_Thread_Mutex MUTEX;
  typedef ACE_Null_Mutex NULL_MUTEX;
  typedef ACE_Process_Mutex PROCESS_MUTEX;
  typedef ACE_Recursive_Thread_Mutex RECURSIVE_MUTEX;
  typedef ACE_RW_Thread_Mutex RW_MUTEX;
  typedef ACE_Condition_Thread_Mutex CONDITION;
  typedef ACE_Condition_Recursive_Thread_Mutex RECURSIVE_CONDITION;
  typedef ACE_Thread_Semaphore SEMAPHORE;
  typedef ACE_Null_Semaphore NULL_SEMAPHORE;
};

#endif /* ACE_HAS_THREADS */

#define ACE_SYNCH_MUTEX ACE_SYNCH::MUTEX
#define ACE_SYNCH_NULL_MUTEX ACE_SYNCH::NULL_MUTEX
#define ACE_SYNCH_RECURSIVE_MUTEX ACE_SYNCH::RECURSIVE_MUTEX
#define ACE_SYNCH_RW_MUTEX ACE_SYNCH::RW_MUTEX
#define ACE_SYNCH_CONDITION ACE_SYNCH::CONDITION
#define ACE_SYNCH_RECURSIVE_CONDITION ACE_SYNCH::RECURSIVE_CONDITION
#define ACE_SYNCH_NULL_SEMAPHORE ACE_SYNCH::NULL_SEMAPHORE
#define ACE_SYNCH_SEMAPHORE ACE_SYNCH::SEMAPHORE

#else /* !ACE_HAS_TEMPLATE_TYPEDEFS */

#if defined (ACE_HAS_OPTIMIZED_MESSAGE_QUEUE)
#define ACE_NULL_SYNCH ACE_Null_Mutex, ACE_Null_Condition, ACE_Null_Mutex
#define ACE_MT_SYNCH ACE_Thread_Mutex, ACE_Condition_Thread_Mutex, ACE_Thread_Semaphore
#else
#define ACE_NULL_SYNCH ACE_Null_Mutex, ACE_Null_Condition
#define ACE_MT_SYNCH ACE_Thread_Mutex, ACE_Condition_Thread_Mutex
#endif /* ACE_HAS_OPTIMIZED_MESSAGE_QUEUE */

#if defined (ACE_HAS_THREADS)

#define ACE_SYNCH_MUTEX ACE_Thread_Mutex
#define ACE_SYNCH_NULL_MUTEX  ACE_Null_Mutex
#define ACE_SYNCH_RECURSIVE_MUTEX ACE_Recursive_Thread_Mutex
#define ACE_SYNCH_RW_MUTEX ACE_RW_Thread_Mutex
#define ACE_SYNCH_CONDITION ACE_Condition_Thread_Mutex
#define ACE_SYNCH_RECURSIVE_CONDITION ACE_Condition_Recursive_Thread_Mutex
#define ACE_SYNCH_SEMAPHORE ACE_Thread_Semaphore
#define ACE_SYNCH_NULL_SEMAPHORE  ACE_Null_Semaphore

#else /* ACE_HAS_THREADS */

#define ACE_SYNCH_MUTEX ACE_Null_Mutex
#define ACE_SYNCH_NULL_MUTEX ACE_Null_Mutex
#define ACE_SYNCH_RECURSIVE_MUTEX ACE_Null_Mutex
#define ACE_SYNCH_RW_MUTEX ACE_Null_Mutex
#define ACE_SYNCH_CONDITION ACE_Null_Condition
#define ACE_SYNCH_RECURSIVE_CONDITION ACE_Null_Condition
#define ACE_SYNCH_SEMAPHORE ACE_Null_Semaphore
#define ACE_SYNCH_NULL_SEMAPHORE ACE_Null_Mutex

#endif /* ACE_HAS_THREADS */
#endif /* ACE_HAS_TEMPLATE_TYPEDEFS */

// These are available on *all* platforms
#define ACE_SYNCH_PROCESS_SEMAPHORE ACE_Process_Semaphore
#define ACE_SYNCH_PROCESS_MUTEX  ACE_Process_Mutex

#if defined (ACE_HAS_THREADS)
#define ACE_SYNCH ACE_MT_SYNCH
#else /* ACE_HAS_THREADS */
#define ACE_SYNCH ACE_NULL_SYNCH
#endif /* ACE_HAS_THREADS */

#include /**/ "ace/post.h"
#endif /* ACE_SYNCH_TRAITS_H */
