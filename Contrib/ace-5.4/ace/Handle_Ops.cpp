// Handle_Ops.cpp,v 1.6 2003/11/01 11:15:12 dhinton Exp

#include "ace/Handle_Ops.h"

#if defined (ACE_LACKS_INLINE_FUNCTIONS)
#include "ace/Handle_Ops.i"
#endif /* ACE_LACKS_INLINE_FUNCTIONS */

#include "ace/OS_NS_errno.h"
#include "ace/OS_NS_fcntl.h"
#include "ace/Time_Value.h"

ACE_RCSID(ace, Handle_Ops, "Handle_Ops.cpp,v 1.6 2003/11/01 11:15:12 dhinton Exp")

ACE_HANDLE
ACE_Handle_Ops::handle_timed_open (ACE_Time_Value *timeout,
                                   const ACE_TCHAR *name,
                                   int flags,
                                   int perms,
                                   LPSECURITY_ATTRIBUTES sa)
{
  ACE_TRACE ("ACE_Handle_Ops::handle_timed_open");

  if (timeout != 0)
    {
#if !defined (ACE_WIN32)
      // On Win32, ACE_NONBLOCK gets recognized as O_WRONLY so we
      // don't use it there
      flags |= ACE_NONBLOCK;
#endif /* ACE_WIN32 */

      // Open the named pipe or file using non-blocking mode...
      ACE_HANDLE handle = ACE_OS::open (name,
                                        flags,
                                        perms,
                                        sa);
      if (handle == ACE_INVALID_HANDLE
          && (errno == EWOULDBLOCK
              && (timeout->sec () > 0 || timeout->usec () > 0)))
        // This expression checks if we were polling.
        errno = ETIMEDOUT;

      return handle;
    }
  else
    return ACE_OS::open (name, flags, perms, sa);
}
