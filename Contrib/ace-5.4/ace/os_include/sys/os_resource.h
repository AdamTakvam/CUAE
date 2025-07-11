// -*- C++ -*-

//=============================================================================
/**
 *  @file    os_resource.h
 *
 *  definitions for XSI resource operations
 *
 *  os_resource.h,v 1.4 2003/11/07 01:12:51 dhinton Exp
 *
 *  @author Don Hinton <dhinton@dresystems.com>
 *  @author This code was originally in various places including ace/OS.h.
 */
//=============================================================================

#ifndef ACE_OS_INCLUDE_SYS_OS_RESOURCE_H
#define ACE_OS_INCLUDE_SYS_OS_RESOURCE_H

#include /**/ "ace/pre.h"

#include "ace/config-all.h"

#if !defined (ACE_LACKS_PRAGMA_ONCE)
# pragma once
#endif /* ACE_LACKS_PRAGMA_ONCE */

#include "ace/os_include/sys/os_time.h"
#include "ace/os_include/sys/os_types.h"

#if !defined (ACE_LACKS_SYS_RESOURCE_H)
#  include /**/ <sys/resource.h>
#endif /* !ACE_LACKS_SYS_RESOURCE_H */

#if defined (ACE_HAS_SYSINFO)
#  include /**/ <sys/systeminfo.h>
#endif /* ACE_HAS_SYS_INFO */

#if defined (ACE_HAS_SYSCALL_H)
#  include /**/ <sys/syscall.h>
#endif /* ACE_HAS_SYSCALL_H */

// Place all additions (especially function declarations) within extern "C" {}
#ifdef __cplusplus
extern "C"
{
#endif /* __cplusplus */

// There must be a better way to do this...
#if !defined (RLIMIT_NOFILE)
#  if defined (linux) || defined (AIX) || defined (SCO)
#    if defined (RLIMIT_OFILE)
#      define RLIMIT_NOFILE RLIMIT_OFILE
#    else
#      define RLIMIT_NOFILE 200
#    endif /* RLIMIT_OFILE */
#  endif /* defined (linux) || defined (AIX) || defined (SCO) */
#endif /* RLIMIT_NOFILE */

#if defined (ACE_HAS_BROKEN_SETRLIMIT)
   typedef struct rlimit ACE_SETRLIMIT_TYPE;
#else
   typedef const struct rlimit ACE_SETRLIMIT_TYPE;
#endif /* ACE_HAS_BROKEN_SETRLIMIT */

#if defined (ACE_WIN32)
#  define RUSAGE_SELF 1
   /// Fake the UNIX rusage structure.  Perhaps we can add more to this
   /// later on?
   struct rusage
   {
     FILETIME ru_utime;
     FILETIME ru_stime;
   };
#else /* !ACE_WIN32 */
#   if defined (m88k)
#     define RUSAGE_SELF 1
#   endif  /*  m88k */
#endif /* ACE_WIN32 */

#if defined (ACE_LACKS_RLIMIT_PROTOTYPE)
  int getrlimit (int resource, struct rlimit *rlp);
  int setrlimit (int resource, const struct rlimit *rlp);
#endif /* ACE_LACKS_RLIMIT_PROTOTYPE */

#if defined (ACE_HAS_PRUSAGE_T)
   typedef prusage_t ACE_Rusage;
#elif defined (ACE_HAS_GETRUSAGE)
   typedef rusage ACE_Rusage;
#else
   typedef int ACE_Rusage;
#endif /* ACE_HAS_PRUSAGE_T */

#if !defined (ACE_WIN32)
// These prototypes are chronically lacking from many versions of
// UNIX.
# if !defined (ACE_HAS_GETRUSAGE_PROTO)
  int getrusage (int who, struct rusage *rusage);
# endif /* ! ACE_HAS_GETRUSAGE_PROTO */

# if defined (ACE_LACKS_SYSCALL)
  int syscall (int, ACE_HANDLE, struct rusage *);
# endif /* ACE_LACKS_SYSCALL */
#endif /* !ACE_WIN32 */

#ifdef __cplusplus
}
#endif /* __cplusplus */

#include /**/ "ace/post.h"
#endif /* ACE_OS_INCLUDE_SYS_OS_RESOURCE_H */
