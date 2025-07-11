/* -*- C++ -*- */
// Module.i,v 4.6 2003/11/07 20:27:28 shuston Exp

// Module.i

#include "ace/OS_NS_string.h"

template <ACE_SYNCH_DECL> ACE_INLINE void *
ACE_Module<ACE_SYNCH_USE>::arg (void) const
{
  ACE_TRACE ("ACE_Module<ACE_SYNCH_USE>::arg");
  return this->arg_;
}

template <ACE_SYNCH_DECL> ACE_INLINE void
ACE_Module<ACE_SYNCH_USE>::arg (void *a)
{
  ACE_TRACE ("ACE_Module<ACE_SYNCH_USE>::arg");
  this->arg_ = a;
}

template <ACE_SYNCH_DECL> ACE_INLINE const ACE_TCHAR *
ACE_Module<ACE_SYNCH_USE>::name (void) const
{
  ACE_TRACE ("ACE_Module<ACE_SYNCH_USE>::name");
  return this->name_;
}

template <ACE_SYNCH_DECL> ACE_INLINE void
ACE_Module<ACE_SYNCH_USE>::name (const ACE_TCHAR *n)
{
  ACE_TRACE ("ACE_Module<ACE_SYNCH_USE>::name");
  ACE_OS::strsncpy (this->name_, n, MAXNAMLEN);
}

template <ACE_SYNCH_DECL> ACE_INLINE ACE_Task<ACE_SYNCH_USE> *
ACE_Module<ACE_SYNCH_USE>::writer (void)
{ 
  ACE_TRACE ("ACE_Module<ACE_SYNCH_USE>::writer");
  return this->q_pair_[1];
}

template <ACE_SYNCH_DECL> ACE_INLINE ACE_Task<ACE_SYNCH_USE> *
ACE_Module<ACE_SYNCH_USE>::reader (void)
{
  ACE_TRACE ("ACE_Module<ACE_SYNCH_USE>::reader");
  return this->q_pair_[0];
}

template <ACE_SYNCH_DECL> ACE_INLINE ACE_Module<ACE_SYNCH_USE> *
ACE_Module<ACE_SYNCH_USE>::next (void)
{
  ACE_TRACE ("ACE_Module<ACE_SYNCH_USE>::next");
  return this->next_;
}

template <ACE_SYNCH_DECL> ACE_INLINE void
ACE_Module<ACE_SYNCH_USE>::next (ACE_Module<ACE_SYNCH_USE> *m) 
{
  ACE_TRACE ("ACE_Module<ACE_SYNCH_USE>::next");
  this->next_ = m;
}


