// -*- C++ -*-

//=============================================================================
/**
 *  @file    Timer_Queue_Adapters.h
 *
 *  Timer_Queue_Adapters.h,v 4.30 2002/06/24 12:33:31 schmidt Exp
 *
 *  @author Douglas C. Schmidt <schmidt@cs.wustl.edu> and 
 *          Carlos O'Ryan <coryan@uci.edu>
 */
//=============================================================================

#ifndef ACE_TIMER_QUEUE_ADAPTERS_H
#define ACE_TIMER_QUEUE_ADAPTERS_H
#include "ace/pre.h"

#include "ace/Task.h"

#if !defined (ACE_LACKS_PRAGMA_ONCE)
# pragma once
#endif /* ACE_LACKS_PRAGMA_ONCE */

#include "ace/Signal.h"

/**
 * @class ACE_Async_Timer_Queue_Adapter
 *
 * @brief Adapts a <TQ> to be run asynchronously.
 *
 * This implementation uses the <ualarm> call, which generates
 * the SIGARLM signal that is caught by this class.
 */
template <class TQ>
class ACE_Async_Timer_Queue_Adapter : public ACE_Event_Handler
{
public:
  typedef TQ TIMER_QUEUE;

  /// Constructor
  /**
   * Register the SIGALRM handler.  If <mask> == 0 then block all
   * signals when <SIGALRM> is run.  Otherwise, just block the signals
   * indicated in <mask>.
   */
  ACE_Async_Timer_Queue_Adapter (ACE_Sig_Set *mask = 0);

  /// Schedule the timer according to the semantics of the
  /// <ACE_Timer_List>.
  /**
   * This timer gets dispatched via a signal, rather than by a user
   * calling <expire>.  Note that interval timers are not implemented
   * yet.
   */
  long schedule (ACE_Event_Handler *type,
                 const void *act,
                 const ACE_Time_Value &future_time,
                 const ACE_Time_Value &interval = ACE_Time_Value::zero);

  /// Cancel the <timer_id> and pass back the <act> if an address is
  /// passed in.
  int cancel (long timer_id, const void **act = 0);

  /// Dispatch all timers whose values are <= <cur_time>.  Returns the
  /// number of timers canceled.
  int expire (void);

  /// Access the underlying <TIMER_QUEUE>.
  TQ &timer_queue (void);

private:
  /// Perform the logic to compute the new ualarm(2) setting.
  virtual int schedule_ualarm (void);

  /// Called back by <SIGALRM> handler.
  virtual int handle_signal (int signum, siginfo_t *, ucontext_t *);

  /// Handler for the <SIGALRM> signal, so that we can access our state
  /// without requiring any global variables.
  ACE_Sig_Handler sig_handler_;

  /// Implementation of the timer queue (e.g., <ACE_Timer_List>,
  /// <ACE_Timer_Heap>, etc.).
  TQ timer_queue_;

  /// Mask of signals to be blocked when we're servicing <SIGALRM>.
  ACE_Sig_Set mask_;
};

/**
 * @class ACE_Thread_Timer_Queue_Adapter
 *
 * @brief Adapts a Timer_Queue using a separate thread for dispatching.
 *
 * This implementation of a Timer_Queue uses a separate thread to
 * dispatch the timers. The base queue need not be thread safe,
 * this class takes all the necessary locks.
 *
 * @note This is a case where template parameters will be useful, but
 * (IMHO) the effort and portability problems discourage their
 * use.
 *
 */
template <class TQ>
class ACE_Thread_Timer_Queue_Adapter : public ACE_Task_Base
{
public:
  /// Trait for the underlying queue type.
  typedef TQ TIMER_QUEUE;

# if defined (ACE_HAS_DEFERRED_TIMER_COMMANDS)

  /// Typedef for the position at which to enqueue a deferred
  /// execution command.
  enum COMMAND_ENQUEUE_POSITION {HEAD, TAIL};

# endif /* ACE_HAS_DEFERRED_TIMER_COMMANDS */

  /// Creates the timer queue.  Activation of the task is the user's
  /// responsibility. Optionally a pointer to a timer queue can be passed,
  /// when no pointer is passed, a TQ is dynamically created
  ACE_Thread_Timer_Queue_Adapter (ACE_Thread_Manager * = ACE_Thread_Manager::instance (),
                                  TQ* timer_queue = 0);

  /// Destructor.
  virtual ~ACE_Thread_Timer_Queue_Adapter (void);

  /// Schedule the timer according to the semantics of the <TQ>; wakes
  /// up the dispatching thread.
  long schedule (ACE_Event_Handler *handler,
                 const void *act,
                 const ACE_Time_Value &future_time,
                 const ACE_Time_Value &interval = ACE_Time_Value::zero);

  /// Cancel the <timer_id> and return the <act> parameter if an
  /// address is passed in. Also wakes up the dispatching thread.
  int cancel (long timer_id, const void **act = 0);

  /// Runs the dispatching thread.
  virtual int svc (void);

  /// Inform the dispatching thread that it should terminate.
  virtual void deactivate (void);

  /// Access the locking mechanism, useful for iteration.
  ACE_SYNCH_RECURSIVE_MUTEX &mutex (void);

  /// @deprecated Access the implementation queue, useful for iteration.
  /// Use the method that returns a pointer instead
  TQ &timer_queue (void);

  /// Set a user-specified timer queue.
  int timer_queue (TQ *tq);

  /// Return the current <TQ>.
  TQ *timer_queue (void) const;

  /// Return the thread id of our active object.
  ACE_thread_t thr_id (void) const;

  /**
   * We override the default <activate> method so that we can ensure
   * that only a single thread is ever spawned.  Otherwise, too many
   * weird things can happen...
   */
  virtual int activate (long flags = THR_NEW_LWP | THR_JOINABLE,
                        int n_threads = 1,
                        int force_active = 0,
                        long priority = ACE_DEFAULT_THREAD_PRIORITY,
                        int grp_id = -1,
                        ACE_Task_Base *task = 0,
                        ACE_hthread_t thread_handles[] = 0,
                        void *stack[] = 0,
                        size_t stack_size[] = 0,
                        ACE_thread_t thread_names[] = 0);

# if defined (ACE_HAS_DEFERRED_TIMER_COMMANDS)

  /**
   * Enqueues a command object for execution just before waiting on the next
   * timer event. This allows deferred execution of commands that cannot
   * be performed in the timer event handler context, such as registering
   * or cancelling timers on platforms where the timer queue mutex is not
   * recursive.
   */
  int enqueue_command (ACE_Command_Base *command_,
                       COMMAND_ENQUEUE_POSITION pos = TAIL);

# endif /* ACE_HAS_DEFERRED_TIMER_COMMANDS */

private:

# if defined (ACE_HAS_DEFERRED_TIMER_COMMANDS)
  /// Dispatches all command objects enqueued in the most
  /// recent event handler context.
  int dispatch_commands (void);

  /// Queue of commands for deferred execution.
  ACE_Unbounded_Queue<ACE_Command_Base *> command_queue_;

  /// The mutual exclusion mechanism for the command queue.
  ACE_SYNCH_MUTEX command_mutex_;
# endif /* ACE_HAS_DEFERRED_TIMER_COMMANDS */

  /// The underlying Timer_Queue.
  TQ* timer_queue_;

  /// Keeps track of whether we should delete the timer queue (if we
  /// didn't create it, then we don't delete it).
  int delete_timer_queue_;

  /// The mutual exclusion mechanism that is required to use the
  /// <condition_>.
  ACE_SYNCH_RECURSIVE_MUTEX mutex_;

  /**
   * The dispatching thread sleeps on this condition while waiting to
   * dispatch the next timer; it is used to wake it up if there is a
   * change on the timer queue.
   */
  ACE_SYNCH_RECURSIVE_CONDITION condition_;

  /// When deactivate is called this variable turns to false and the
  /// dispatching thread is signalled, to terminate its main loop.
  int active_;

  /// Thread id of our active object task.
  ACE_thread_t thr_id_;
};

#if defined (__ACE_INLINE__)
# include "ace/Timer_Queue_Adapters.i"
#endif /* __ACE_INLINE__ */

#if defined (ACE_TEMPLATES_REQUIRE_SOURCE)
# include "ace/Timer_Queue_Adapters.cpp"
#endif /* ACE_TEMPLATES_REQUIRE_SOURCE */

#if defined (ACE_TEMPLATES_REQUIRE_PRAGMA)
# pragma implementation ("Timer_Queue_Adapters.cpp")
#endif /* ACE_TEMPLATES_REQUIRE_PRAGMA */

#include "ace/post.h"
#endif /* ACE_TIMER_QUEUE_ADAPTERS_H */
