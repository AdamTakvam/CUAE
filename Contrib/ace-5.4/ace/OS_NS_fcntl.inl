// -*- C++ -*-
// OS_NS_fcntl.inl,v 1.3 2003/11/04 07:05:40 jwillemsen Exp

#include "ace/OS_NS_errno.h"

ACE_INLINE int
ACE_OS::fcntl (ACE_HANDLE handle, int cmd, long arg)
{
  ACE_OS_TRACE ("ACE_OS::fcntl");
# if defined (ACE_LACKS_FCNTL)
  ACE_UNUSED_ARG (handle);
  ACE_UNUSED_ARG (cmd);
  ACE_UNUSED_ARG (arg);
  ACE_NOTSUP_RETURN (-1);
# else
  ACE_OSCALL_RETURN (::fcntl (handle, cmd, arg), int, -1);
# endif /* ACE_LACKS_FCNTL */
}

