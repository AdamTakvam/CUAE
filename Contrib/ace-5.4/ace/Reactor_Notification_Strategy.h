/* -*- C++ -*- */

//=============================================================================
/**
 *  @file   Reactor_Notification_Strategy.h
 *
 *  Reactor_Notification_Strategy.h,v 4.3 2003/07/19 19:04:13 dhinton Exp
 *
 *  @author Doug Schmidt
 */
//=============================================================================
#ifndef ACE_REACTOR_NOTIFICATION_STRATEGY_H
#define ACE_REACTOR_NOTIFICATION_STRATEGY_H
#include /**/ "ace/pre.h"

#include "ace/Notification_Strategy.h"

#if !defined (ACE_LACKS_PRAGMA_ONCE)
# pragma once
#endif /* ACE_LACKS_PRAGMA_ONCE */

/**
 * @class ACE_Reactor_Notification_Strategy
 *
 * @brief Used to notify an ACE_Reactor
 *
 * Integrates the <ACE_Message_Queue> notification into the
 * <ACE_Reactor::notify> method.
 */
class ACE_Export ACE_Reactor_Notification_Strategy : public ACE_Notification_Strategy
{
public:
  ACE_Reactor_Notification_Strategy (ACE_Reactor *reactor,
                                     ACE_Event_Handler *eh,
                                     ACE_Reactor_Mask mask);

  /// Default dtor.
  virtual ~ACE_Reactor_Notification_Strategy (void);

  virtual int notify (void);

  virtual int notify (ACE_Event_Handler *,
                      ACE_Reactor_Mask mask);

  /// Get the reactor
  ACE_Reactor *reactor (void);

  /// Set the reactor
  void reactor (ACE_Reactor *r);

protected:
  /// The Reactor
  ACE_Reactor *reactor_;
};

#if defined (__ACE_INLINE__)
#include "ace/Reactor_Notification_Strategy.inl"
#endif /* __ACE_INLINE __ */

#include /**/ "ace/post.h"
#endif /*ACE_REACTOR_NOTIFICATION_STRATEGY_H */
