// -*- C++ -*-
//
// SSL_SOCK.cpp,v 1.6 2003/11/04 16:46:04 dhinton Exp


#include "SSL_SOCK.h"

#if defined (ACE_LACKS_INLINE_FUNCTIONS)
#include "SSL_SOCK.i"
#endif

#include "ace/OS_NS_errno.h"
#include "ace/os_include/os_signal.h"

ACE_RCSID (ACE_SSL,
           SSL_SOCK,
           "SSL_SOCK.cpp,v 1.6 2003/11/04 16:46:04 dhinton Exp")


ACE_SSL_SOCK::ACE_SSL_SOCK (void)
{
  ACE_TRACE ("ACE_SSL_SOCK::ACE_SSL_SOCK");
}

ACE_SSL_SOCK::~ACE_SSL_SOCK (void)
{
  ACE_TRACE ("ACE_SSL_SOCK::~ACE_SSL_SOCK");
}

int
ACE_SSL_SOCK::enable (int value) const
{
  ACE_TRACE ("ACE_SSL_SOCK::enable");

  switch (value)
    {
#ifdef SIGURG
    case SIGURG:
    case ACE_SIGURG:
#endif  /* SIGURG */
    case SIGIO:
    case ACE_SIGIO:
    case ACE_CLOEXEC:
      ACE_NOTSUP_RETURN (-1);
    case ACE_NONBLOCK:
      return ACE_IPC_SAP::enable (value);
    default:
      return -1;
    }
}

int
ACE_SSL_SOCK::disable (int value) const
{
  ACE_TRACE("ACE_SSL_SOCK::disable");
  switch (value)
    {
#ifdef SIGURG
    case SIGURG:
    case ACE_SIGURG:
#endif  /* SIGURG */
    case SIGIO:
    case ACE_SIGIO:
    case ACE_CLOEXEC:
      ACE_NOTSUP_RETURN (-1);
    case ACE_NONBLOCK:
      return ACE_IPC_SAP::disable (value);
    default:
      return -1;
    }
}
