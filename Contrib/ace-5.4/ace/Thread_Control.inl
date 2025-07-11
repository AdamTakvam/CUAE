// -*- C++ -*-
// Thread_Control.inl,v 4.2 2002/07/02 19:56:49 shuston Exp

// Set the exit status.

ACE_INLINE ACE_THR_FUNC_RETURN
ACE_Thread_Control::status (ACE_THR_FUNC_RETURN s)
{
  ACE_OS_TRACE ("ACE_Thread_Control::status");
  return this->status_ = s;
}

// Get the exit status.

ACE_INLINE ACE_THR_FUNC_RETURN
ACE_Thread_Control::status (void)
{
  ACE_OS_TRACE ("ACE_Thread_Control::status");
  return this->status_;
}

// Returns the current <Thread_Manager>.

ACE_INLINE ACE_Thread_Manager *
ACE_Thread_Control::thr_mgr (void)
{
  ACE_OS_TRACE ("ACE_Thread_Control::thr_mgr");
  return this->tm_;
}

// Atomically set a new <Thread_Manager> and return the old
// <Thread_Manager>.

ACE_INLINE ACE_Thread_Manager *
ACE_Thread_Control::thr_mgr (ACE_Thread_Manager *tm)
{
  ACE_OS_TRACE ("ACE_Thread_Control::thr_mgr");
  ACE_Thread_Manager *o_tm = this->tm_;
  this->tm_ = tm;
  return o_tm;
}

