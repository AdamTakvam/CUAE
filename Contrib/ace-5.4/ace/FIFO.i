/* -*- C++ -*- */
// FIFO.i,v 4.4 2003/11/01 11:15:12 dhinton Exp

// FIFO.i

#include "ace/OS_NS_unistd.h"

ACE_INLINE int
ACE_FIFO::get_local_addr (const ACE_TCHAR *&r) const
{
  ACE_TRACE ("ACE_FIFO::get_local_addr");
  r = this->rendezvous_;
  return 0;
}

ACE_INLINE int
ACE_FIFO::remove (void)
{
  ACE_TRACE ("ACE_FIFO::remove");
  int result = this->close ();
  return ACE_OS::unlink (this->rendezvous_) == -1 || result == -1 ? -1 : 0;
}
