// -*- C++ -*-

//==========================================================================
/**
 *  @file    Event.h
 *
 *  Event.h,v 4.2 2003/11/01 11:15:12 dhinton Exp
 *
 *   Moved from Synch.h.
 *
 *  @author Douglas C. Schmidt <schmidt@cs.wustl.edu>
 */
//==========================================================================

#ifndef ACE_EVENT_H
#define ACE_EVENT_H
#include /**/ "ace/pre.h"

#include "ace/ACE_export.h"

#if !defined (ACE_LACKS_PRAGMA_ONCE)
# pragma once
#endif /* ACE_LACKS_PRAGMA_ONCE */

#include "ace/OS_NS_Thread.h"

/**
 * @class ACE_Event
 *
 * @brief A wrapper around the Win32 event locking mechanism.
 *
 * Portable implementation of an Event mechanism, which is native to
 * Win32, but must be emulated on UNIX.  All platforms support
 * process-scope locking support.  However, only Win32 platforms
 * support global naming and system-scope locking support.
 */
class ACE_Export ACE_Event
{
public:
  /// Constructor that creates event.
  ACE_Event (int manual_reset = 0,
             int initial_state = 0,
             int type = USYNC_THREAD,
             const ACE_TCHAR *name = 0,
             void *arg = 0);

  /// Implicitly destroy the event variable.
  ~ACE_Event (void);

  /**
   * Explicitly destroy the event variable.  Note that only one thread
   * should call this method since it doesn't protect against race
   * conditions.
   */
  int remove (void);

  /// Underlying handle to event.
  ACE_event_t handle (void) const;

  /**
   * Set the underlying handle to event. Note that this method assumes
   * ownership of the <handle> and will close it down in <remove>.  If
   * you want the <handle> to stay open when <remove> is called make
   * sure to call <dup> on the <handle> before closing it.  You are
   * responsible for the closing the existing <handle> before
   * overwriting it.
   */
  void handle (ACE_event_t new_handle);

  /**
   * if MANUAL reset
   *    sleep till the event becomes signaled
   *    event remains signaled after wait() completes.
   * else AUTO reset
   *    sleep till the event becomes signaled
   *    event resets wait() completes.
   */
  int wait (void);

  /// Same as wait() above, but this one can be timed
  /// <abstime> is absolute time-of-day if if <use_absolute_time>
  /// is non-0, else it is relative time.
  int wait (const ACE_Time_Value *abstime,
            int use_absolute_time = 1);

  /**
   * if MANUAL reset
   *    wake up all waiting threads
   *    set to signaled state
   * else AUTO reset
   *    if no thread is waiting, set to signaled state
   *    if thread(s) are waiting, wake up one waiting thread and
   *    reset event
   */
  int signal (void);

  /**
   * if MANUAL reset
   *    wakeup all waiting threads and
   *    reset event
   * else AUTO reset
   *    wakeup one waiting thread (if present) and
   *    reset event
   */
  int pulse (void);

  /// Set to nonsignaled state.
  int reset (void);

  /// Dump the state of an object.
  void dump (void) const;

  /// Declare the dynamic allocation hooks
  ACE_ALLOC_HOOK_DECLARE;

protected:
  /// The underlying handle.
  ACE_event_t handle_;

  /// Keeps track of whether <remove> has been called yet to avoid
  /// multiple <remove> calls, e.g., explicitly and implicitly in the
  /// destructor.  This flag isn't protected by a lock, so make sure
  /// that you don't have multiple threads simultaneously calling
  /// <remove> on the same object, which is a bad idea anyway...
  int removed_;

private:
  // = Prevent copying.
  ACE_Event (const ACE_Event& event);
  const ACE_Event &operator= (const ACE_Event &rhs);
};

#if defined (__ACE_INLINE__)
#include "ace/Event.inl"
#endif /* __ACE_INLINE__ */

#include /**/ "ace/post.h"
#endif /* ACE_EVENT_H */
