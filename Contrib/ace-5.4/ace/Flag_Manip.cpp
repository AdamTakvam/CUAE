// Flag_Manip.cpp,v 1.3 2003/11/05 13:15:06 jwillemsen Exp

#include "Flag_Manip.h"

#if defined (ACE_LACKS_INLINE_FUNCTIONS)
#include "ace/Flag_Manip.i"
#endif /* ACE_LACKS_INLINE_FUNCTIONS */

#if defined (ACE_WIN32) || defined (VXWORKS) || defined (ACE_LACKS_FCNTL)
#  include "ace/OS_NS_stropts.h"
#  include "ace/OS_NS_errno.h"
#endif /* ACE_WIN32 || VXWORKS || ACE_LACKS_FCNTL */

#if defined (CYGWIN32)
#  include "ace/os_include/os_termios.h"
#endif /* CYGWIN32 */

ACE_RCSID(ace, Flag_Manip, "Flag_Manip.cpp,v 1.3 2003/11/05 13:15:06 jwillemsen Exp")

// Flags are file status flags to turn on.

int
ACE_Flag_Manip::set_flags (ACE_HANDLE handle, int flags)
{
  ACE_TRACE ("ACE_Flag_Manip::set_flags");
#if defined (ACE_WIN32) || defined (VXWORKS) || defined (ACE_LACKS_FCNTL)
  switch (flags)
    {
    case ACE_NONBLOCK:
      // nonblocking argument (1)
      // blocking:            (0)
      {
        u_long nonblock = 1;
        return ACE_OS::ioctl (handle, FIONBIO, &nonblock);
      }
    default:
      ACE_NOTSUP_RETURN (-1);
    }
#else
  int val = ACE_OS::fcntl (handle, F_GETFL, 0);

  if (val == -1)
    return -1;

  // Turn on flags.
  ACE_SET_BITS (val, flags);

  if (ACE_OS::fcntl (handle, F_SETFL, val) == -1)
    return -1;
  else
    return 0;
#endif /* ACE_WIN32 || ACE_LACKS_FCNTL */
}

// Flags are the file status flags to turn off.

int
ACE_Flag_Manip::clr_flags (ACE_HANDLE handle, int flags)
{
  ACE_TRACE ("ACE_Flag_Manip::clr_flags");

#if defined (ACE_WIN32) || defined (VXWORKS) || defined (ACE_LACKS_FCNTL)
  switch (flags)
    {
    case ACE_NONBLOCK:
      // nonblocking argument (1)
      // blocking:            (0)
      {
        u_long nonblock = 0;
        return ACE_OS::ioctl (handle, FIONBIO, &nonblock);
      }
    default:
      ACE_NOTSUP_RETURN (-1);
    }
#else
  int val = ACE_OS::fcntl (handle, F_GETFL, 0);

  if (val == -1)
    return -1;

  // Turn flags off.
  ACE_CLR_BITS (val, flags);

  if (ACE_OS::fcntl (handle, F_SETFL, val) == -1)
    return -1;
  else
    return 0;
#endif /* ACE_WIN32 || ACE_LACKS_FCNTL */
}
