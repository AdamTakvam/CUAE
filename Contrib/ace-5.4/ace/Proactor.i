/* -*- C++ -*- */
// Proactor.i,v 4.7 2001/12/24 22:36:17 schmidt Exp

ACE_INLINE int
ACE_Proactor::run_event_loop (void)
{
  ACE_TRACE ("ACE_Proactor::run_event_loop");
  ACE_Proactor *p = ACE_Proactor::instance ();

  if (p == 0)
    return -1;

  return p->proactor_run_event_loop (ACE_Proactor::check_reconfiguration);
}

ACE_INLINE int
ACE_Proactor::run_event_loop (ACE_Time_Value &tv)
{
  ACE_TRACE ("ACE_Proactor::run_event_loop (tv)");
  ACE_Proactor *p = ACE_Proactor::instance ();

  if (p == 0)
    return -1;

  return p->proactor_run_event_loop 
    (tv, ACE_Proactor::check_reconfiguration);
}

ACE_INLINE int
ACE_Proactor::reset_event_loop(void)
{
  ACE_TRACE ("ACE_Proactor::reset_event_loop");
  ACE_Proactor *p = ACE_Proactor::instance ();

  if (p == 0)
    return -1;

  return p->proactor_reset_event_loop ();
}

ACE_INLINE int
ACE_Proactor::end_event_loop (void)
{
  ACE_TRACE ("ACE_Proactor::end_event_loop");
  ACE_Proactor *p = ACE_Proactor::instance ();

  if (p == 0)
    return -1;

  return p->proactor_end_event_loop ();
}

ACE_INLINE int
ACE_Proactor::event_loop_done (void)
{
  ACE_TRACE ("ACE_Proactor::event_loop_done");
  ACE_Proactor *p = ACE_Proactor::instance ();

  if (p == 0)
    return -1;

  return p->proactor_event_loop_done ();
}

ACE_INLINE int
ACE_Proactor::post_wakeup_completions (int how_many)
{
  ACE_TRACE ("ACE_Proactor::post_wakeup_completions");
  ACE_Proactor *p = ACE_Proactor::instance ();

  if (p == 0)
    return -1;

  return p->proactor_post_wakeup_completions (how_many);
}
