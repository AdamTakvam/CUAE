/* -*- C++ -*- */

//=============================================================================
/**
 *  @file    Msg_WFMO_Reactor.h
 *
 *  Msg_WFMO_Reactor.h,v 4.14 2002/04/05 12:00:45 dhinton Exp
 *
 *  @author Beskrovny Evgeny <evgeny_beskrovny@icomverse.com>
 *  @author Irfan Pyarali <irfan@cs.wustl.edu>
 */
//=============================================================================


#ifndef ACE_MSG_WFMO_REACTOR_H
#define ACE_MSG_WFMO_REACTOR_H
#include "ace/pre.h"

#include "ace/config-all.h"

#if !defined (ACE_LACKS_PRAGMA_ONCE)
# pragma once
#endif /* ACE_LACKS_PRAGMA_ONCE */

#if defined (ACE_WIN32) && !defined (ACE_LACKS_MSG_WFMO)

#include "ace/WFMO_Reactor.h"

/**
 * @class ACE_Msg_WFMO_Reactor
 *
 * @brief An OO event demultiplexor and event handler dispatcher for
 * Win32 <MsgWaitForMultipleObjects>.
 *
 * The ACE_Msg_WFMO_Reactor is an OO event demultiplexor and
 * event handler Reactor. It differs from <ACE_WFMO_Reactor> by
 * its ability to react on Windows messages.  It is needed when
 * the task should serve also as a COM/DCOM server.
 */
class ACE_Export ACE_Msg_WFMO_Reactor : public ACE_WFMO_Reactor
{
public:
  // = Initialization and termination methods.
  /// Initialize <ACE_Msg_WFMO_Reactor> with the default size.
  ACE_Msg_WFMO_Reactor (ACE_Sig_Handler * = 0,
                        ACE_Timer_Queue * = 0);

  /**
   * Initialize <ACE_Msg_WFMO_Reactor> with size <size>.  Two slots will be
   * added to the <size> parameter which will store handles used for
   * internal management purposes.
   */
  ACE_Msg_WFMO_Reactor (size_t size,
                        int unused = 0,
                        ACE_Sig_Handler * = 0,
                        ACE_Timer_Queue * = 0);

  /// Close down the ACE_Msg_WFMO_Reactor and release all of its resources.
  virtual ~ACE_Msg_WFMO_Reactor (void);

  /**
   * This event loop driver blocks for up to <max_wait_time> before
   * returning.  It will return earlier if timer events, I/O events,
   * window events, or signal events occur.  Note that <max_wait_time>
   * can be 0, in which case this method blocks indefinitely until
   * events occur.
   *
   * <max_wait_time> is decremented to reflect how much time this call
   * took.  For instance, if a time value of 3 seconds is passed to
   * handle_events and an event occurs after 2 seconds,
   * <max_wait_time> will equal 1 second.  This can be used if an
   * application wishes to handle events for some fixed amount of
   * time.
   *
   * <MsgWaitForMultipleObjects> is used as the demultiplexing call
   *
   * Returns the total number of <ACE_Event_Handler>s that were
   * dispatched, 0 if the <max_wait_time> elapsed without dispatching
   * any handlers, or -1 if an error occurs.
   *
   * The only difference between <alertable_handle_events> and
   * <handle_events> is that in the alertable case, MWMO_ALERTABLE is
   * passed to <MsgWaitForMultipleObjects> for the <bAlertable>
   * option.
   */
  virtual int handle_events (ACE_Time_Value *max_wait_time = 0);
  virtual int alertable_handle_events (ACE_Time_Value *max_wait_time = 0);

  /**
   * This method is just like the one above, except the
   * <max_wait_time> value is a reference and can therefore never be
   * NULL.
   */
  virtual int handle_events (ACE_Time_Value &max_wait_time);
  virtual int alertable_handle_events (ACE_Time_Value &max_wait_time);

protected:
  /// Wait for timer and I/O events to occur.
  virtual int wait_for_multiple_events (int timeout,
                                        int alertable);

  /// Check for activity on remaining handles.
  virtual DWORD poll_remaining_handles (size_t index);

  /// Dispatches window messages.
  virtual int dispatch_window_messages (void);
};

#endif /* ACE_WIN32 && !ACE_LACKS_MSG_WFMO */

#if defined (__ACE_INLINE__)
#include "ace/Msg_WFMO_Reactor.i"
#endif /* __ACE_INLINE__ */

#include "ace/post.h"
#endif /* ACE_MSG_WFMO_REACTOR_H */
