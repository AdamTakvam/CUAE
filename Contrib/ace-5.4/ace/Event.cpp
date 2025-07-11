// Event.cpp,v 4.3 2003/11/01 11:15:12 dhinton Exp

#include "ace/Event.h"

#if !defined (__ACE_INLINE__)
#include "ace/Event.inl"
#endif /* __ACE_INLINE__ */

#include "ace/Log_Msg.h"

ACE_RCSID(ace, Event, "Event.cpp,v 4.3 2003/11/01 11:15:12 dhinton Exp")

ACE_Event::ACE_Event (int manual_reset,
                      int initial_state,
                      int type,
                      const ACE_TCHAR *name,
                      void *arg)
  : removed_ (0)
{
  if (ACE_OS::event_init (&this->handle_,
                          manual_reset,
                          initial_state,
                          type,
                          name,
                          arg) != 0)
    ACE_ERROR ((LM_ERROR,
                ACE_LIB_TEXT ("%p\n"),
                ACE_LIB_TEXT ("ACE_Event::ACE_Event")));
}

ACE_Event::~ACE_Event (void)
{
  this->remove ();
}

int
ACE_Event::remove (void)
{
  int result = 0;
  if (this->removed_ == 0)
    {
      this->removed_ = 1;
      result = ACE_OS::event_destroy (&this->handle_);
    }
  return result;
}

int
ACE_Event::wait (void)
{
  return ACE_OS::event_wait (&this->handle_);
}

int
ACE_Event::wait (const ACE_Time_Value *abstime, int use_absolute_time)
{
  return ACE_OS::event_timedwait (&this->handle_,
                                  (ACE_Time_Value *) abstime,
                                  use_absolute_time);
}

int
ACE_Event::signal (void)
{
  return ACE_OS::event_signal (&this->handle_);
}

int
ACE_Event::pulse (void)
{
  return ACE_OS::event_pulse (&this->handle_);
}

int
ACE_Event::reset (void)
{
  return ACE_OS::event_reset (&this->handle_);
}

void
ACE_Event::dump (void) const
{
#if defined (ACE_HAS_DUMP)
  ACE_DEBUG ((LM_DEBUG, ACE_BEGIN_DUMP, this));
  ACE_DEBUG ((LM_DEBUG, ACE_END_DUMP));
#endif /* ACE_HAS_DUMP */
}
