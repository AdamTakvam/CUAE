// -*- C++ -*-
// OS_Errno.cpp,v 4.2 2001/10/28 19:15:11 mk1 Exp

#include "ace/OS_Errno.h"

ACE_RCSID(ace, OS_Errno, "OS_Errno.cpp,v 4.2 2001/10/28 19:15:11 mk1 Exp")

#if !defined (ACE_HAS_INLINED_OSCALLS)
# include "ace/OS_Errno.inl"
#endif /* ACE_HAS_INLINED_OS_CALLS */

#if defined (ACE_HAS_WINCE_BROKEN_ERRNO)

ACE_CE_Errno *ACE_CE_Errno::instance_ = 0;
DWORD ACE_CE_Errno::errno_key_ = 0xffffffff;

void
ACE_CE_Errno::init ()
{
  ACE_NEW (ACE_CE_Errno::instance_,
           ACE_CE_Errno ());
  ACE_CE_Errno::errno_key_ = TlsAlloc ();
}

void
ACE_CE_Errno::fini ()
{
  TlsFree (ACE_CE_Errno::errno_key_);
  delete ACE_CE_Errno::instance_;
  ACE_CE_Errno::instance_ = 0;
}

#endif /* ACE_HAS_WINCE_BROKEN_ERRNO */
