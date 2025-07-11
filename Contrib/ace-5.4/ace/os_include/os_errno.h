// -*- C++ -*-

//=============================================================================
/**
 *  @file    os_errno.h
 *
 *  system error numbers
 *
 *  os_errno.h,v 1.8 2003/12/23 23:04:27 shuston Exp
 *
 *  @author Don Hinton <dhinton@dresystems.com>
 *  @author This code was originally in various places including ace/OS.h.
 */
//=============================================================================

#ifndef ACE_OS_INCLUDE_OS_ERRNO_H
#define ACE_OS_INCLUDE_OS_ERRNO_H

#include /**/ "ace/pre.h"

#include "ace/config-all.h"

#if !defined (ACE_LACKS_PRAGMA_ONCE)
# pragma once
#endif /* ACE_LACKS_PRAGMA_ONCE */

#if !defined (ACE_LACKS_ERRNO_H)
# include /**/ <errno.h>
#endif /* !ACE_LACKS_ERRNO_H */

// Needed for VxWorks to pickup errnoSet()
#if defined (VXWORKS)
#include /**/ <errnoLib.h>
#endif /* VXWORKS */

// Place all additions (especially function declarations) within extern "C" {}
#ifdef __cplusplus
extern "C"
{
#endif /* __cplusplus */

#if defined (ACE_PSOS)
#  if !defined (EPERM)
#    define EPERM        1        /* Not super-user                        */
#  endif /* EPERM */
#  if !defined (ENOENT)
#    define ENOENT       2        /* No such file or directory             */
#  endif /* ENOENT */
#  if !defined (ESRCH)
#    define ESRCH        3        /* No such process                       */
#  endif /* ESRCH */
#  if ! defined (EINTR)
#    define EINTR        4        /* interrupted system call               */
#  endif /* EINTR */
#  if !defined (EBADF)
#    define EBADF        9        /* Bad file number                       */
#  endif /* EBADF */
#  if !defined (EAGAIN)
#    define EAGAIN       11       /* Resource temporarily unavailable      */
#  endif /* EAGAIN */
#  if !defined (EWOULDBLOCK)
#    define EWOULDBLOCK  EAGAIN   /* Blocking resource request would block */
#  endif /* EWOULDBLOCK */
#  if !defined (ENOMEM)
#    define ENOMEM       12       /* Not enough core                       */
#  endif /* ENOMEM */
#  if !defined (EACCES)
#    define EACCES      13       /* Permission denied                     */
#  endif /* EACCES */
#  if !defined (EFAULT)
#    define EFAULT       14       /* Bad access                            */
#  endif /* EFAULT */
#  if !defined (EEXIST)
#    define EEXIST       17       /* File exists                           */
#  endif /* EEXIST */
#  if !defined (ENOSPC)
#    define ENOSPC       28       /* No space left on device               */
#  endif /* ENOSPC */
#  if !defined (EPIPE)
#    define EPIPE        32       /* Broken pipe                           */
#  endif /* EPIPE */
#  if !defined (ETIME)
#    define ETIME        62       /* timer expired                         */
#  endif /* ETIME */
#  if !defined (ENAMETOOLONG)
#    define ENAMETOOLONG 78       /* path name is too long                 */
#  endif /* ENAMETOOLONG */
#  if !defined (ENOSYS)
#    define ENOSYS       89       /* Unsupported file system operation     */
#  endif /* ENOSYS */
#  if !defined (EADDRINUSE)
#    define EADDRINUSE   125      /* Address already in use                */
#  endif /* EADDRINUSE */
#  if !defined (ENETUNREACH)
#    define ENETUNREACH  128      /* Network is unreachable                */
#  endif /* ENETUNREACH */
#  if !defined (EISCONN)
#    define EISCONN      133      /* Socket is already connected           */
#  endif /* EISCONN */
#  if !defined (ESHUTDOWN)
#    define ESHUTDOWN    143      /* Can't send after socket shutdown      */
#  endif /* ESHUTDOWN */
#  if !defined (ECONNREFUSED)
#    define ECONNREFUSED 146      /* Connection refused                    */
#  endif /* ECONNREFUSED */
#  if !defined (EINPROGRESS)
#    define EINPROGRESS  150      /* operation now in progress             */
#  endif /* EINPROGRESS */
#  if !defined (ERRMAX)
#    define ERRMAX       151      /* Last error number                     */
#  endif /* ERRMAX */
#endif /* ACE_PSOS */

#if defined (ACE_WIN32)
   // error code mapping for windows
#  define ETIME                   ERROR_SEM_TIMEOUT
#  define EWOULDBLOCK             WSAEWOULDBLOCK
#  define EINPROGRESS             WSAEINPROGRESS
#  define EALREADY                WSAEALREADY
#  define ENOTSOCK                WSAENOTSOCK
#  define EDESTADDRREQ            WSAEDESTADDRREQ
#  define EMSGSIZE                WSAEMSGSIZE
#  define EPROTOTYPE              WSAEPROTOTYPE
#  define ENOPROTOOPT             WSAENOPROTOOPT
#  define EPROTONOSUPPORT         WSAEPROTONOSUPPORT
#  define ESOCKTNOSUPPORT         WSAESOCKTNOSUPPORT
#  define EOPNOTSUPP              WSAEOPNOTSUPP
#  define EPFNOSUPPORT            WSAEPFNOSUPPORT
#  define EAFNOSUPPORT            WSAEAFNOSUPPORT
#  define EADDRINUSE              WSAEADDRINUSE
#  define EADDRNOTAVAIL           WSAEADDRNOTAVAIL
#  define ENETDOWN                WSAENETDOWN
#  define ENETUNREACH             WSAENETUNREACH
#  define ENETRESET               WSAENETRESET
#  define ECONNABORTED            WSAECONNABORTED
#  define ECONNRESET              WSAECONNRESET
#  define ENOBUFS                 WSAENOBUFS
#  define EISCONN                 WSAEISCONN
#  define ENOTCONN                WSAENOTCONN
#  define ESHUTDOWN               WSAESHUTDOWN
#  define ETOOMANYREFS            WSAETOOMANYREFS
#  define ETIMEDOUT               WSAETIMEDOUT
#  define ECONNREFUSED            WSAECONNREFUSED
#  define ELOOP                   WSAELOOP
#  define EHOSTDOWN               WSAEHOSTDOWN
#  define EHOSTUNREACH            WSAEHOSTUNREACH
#  define EPROCLIM                WSAEPROCLIM
#  define EUSERS                  WSAEUSERS
#  define EDQUOT                  WSAEDQUOT
#  define ESTALE                  WSAESTALE
#  define EREMOTE                 WSAEREMOTE
   // Grrr! ENAMETOOLONG and ENOTEMPTY are already defined by the horrible
   // 'standard' library.
   // #define ENAMETOOLONG            WSAENAMETOOLONG
#  if defined (__BORLANDC__) && (__BORLANDC__ <= 0x540)
#    define ENOTEMPTY               WSAENOTEMPTY
#  endif /* __BORLANDC__  && __BORLANDC__ <= 0x540*/
#  define EADDRINUSE WSAEADDRINUSE
#endif /* ACE_WIN32 */

#if defined (ACE_HAS_H_ERRNO)
void herror (const char *str);
#endif /* ACE_HAS_H_ERRNO */

#if !defined (ACE_WIN32) && !defined (ACE_PSOS) && defined (ACE_LACKS_T_ERRNO)
extern int t_errno;
#endif /* ACE_WIN32 && !ACE_PSOS && ACE_LACKS_T_ERRNO */

#if !defined (ENOSYS)
# define ENOSYS EFAULT /* Operation not supported or unknown error. */
#endif /* !ENOSYS */

#if !defined (ENOTSUP)
# define ENOTSUP ENOSYS  /* Operation not supported. */
#endif /* !ENOTSUP */

#if !defined (ESUCCESS)
#  define ESUCCESS 0
#endif /* !ESUCCESS */

#if !defined (EIDRM)
#  define EIDRM 0
#endif /* !EIDRM */

#if !defined (ENFILE)
#  define ENFILE EMFILE /* No more socket descriptors are available. */
#endif /* !ENFILE */

#if !defined (ECOMM)
   // Not the same, but ECONNABORTED is provided on NT.
#  define ECOMM ECONNABORTED
#endif /* ECOMM */

#if !defined (EDEADLK)
#  define EDEADLK 1000 /* Some large number.... */
#endif /* !EDEADLK */

#if !defined (ENXIO)     /* Needed in SOCK_Dgram_Mcast */
#   define ENXIO  6
#endif /* ENXIO */

#if !defined (ETIMEDOUT) && defined (ETIME)
#  define ETIMEDOUT ETIME
#endif /* ETIMEDOUT */

#if !defined (ETIME) && defined (ETIMEDOUT)
#  define ETIME ETIMEDOUT
#endif /* ETIMED */

#if !defined (EBUSY)
#  define EBUSY ETIME
#endif /* EBUSY */

#ifdef __cplusplus
}
#endif /* __cplusplus */

#include /**/ "ace/post.h"
#endif /* ACE_OS_INCLUDE_OS_ERRNO_H */
