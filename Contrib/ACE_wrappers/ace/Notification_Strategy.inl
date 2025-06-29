/* -*- C++ -*- */
//Notification_Strategy.inl,v 4.2 2001/12/11 01:02:12 bala Exp

ACE_INLINE ACE_Event_Handler *
ACE_Notification_Strategy::event_handler (void)
{
  return eh_;
}

ACE_INLINE void
ACE_Notification_Strategy::event_handler (ACE_Event_Handler *eh)
{
  this->eh_ = eh;
}

ACE_INLINE ACE_Reactor_Mask
ACE_Notification_Strategy::mask (void)
{
  return mask_;
}

ACE_INLINE void
ACE_Notification_Strategy::mask (ACE_Reactor_Mask m)
{
  this->mask_ = m;
}
