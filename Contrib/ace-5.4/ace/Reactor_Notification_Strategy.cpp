#include "ace/Reactor_Notification_Strategy.h"
#include "ace/Reactor.h"

#if !defined (__ACE_INLINE__)
#include "ace/Reactor_Notification_Strategy.inl"
#endif /* __ACE_INLINE __ */

ACE_RCSID(ace, Reactor_Notification_Strategy, "Reactor_Notification_Strategy.cpp,v 4.2 2002/01/18 23:52:07 shuston Exp")

ACE_Reactor_Notification_Strategy::ACE_Reactor_Notification_Strategy (ACE_Reactor *reactor,
                                                                      ACE_Event_Handler *eh,
                                                                      ACE_Reactor_Mask mask)
  : ACE_Notification_Strategy (eh, mask),
    reactor_ (reactor)
{
}

ACE_Reactor_Notification_Strategy::~ACE_Reactor_Notification_Strategy (void)
{
}

int
ACE_Reactor_Notification_Strategy::notify (void)
{
  return this->reactor_->notify (this->eh_, this->mask_);
}

int
ACE_Reactor_Notification_Strategy::notify (ACE_Event_Handler *eh,
                                           ACE_Reactor_Mask mask)
{
  return this->reactor_->notify (eh, mask);
}
