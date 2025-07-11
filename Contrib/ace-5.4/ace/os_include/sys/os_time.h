// -*- C++ -*-

//=============================================================================
/**
 *  @file    os_time.h
 *
 *  time types
 *
 *  os_time.h,v 1.4 2003/11/04 17:17:32 ossama Exp
 *
 *  @author Don Hinton <dhinton@dresystems.com>
 *  @author This code was originally in various places including ace/OS.h.
 */
//=============================================================================

#ifndef ACE_OS_INCLUDE_SYS_OS_TIME_H
#define ACE_OS_INCLUDE_SYS_OS_TIME_H

#include /**/ "ace/pre.h"

#include "ace/config-all.h"

#if !defined (ACE_LACKS_PRAGMA_ONCE)
# pragma once
#endif /* ACE_LACKS_PRAGMA_ONCE */

#include "ace/os_include/sys/os_types.h"

#if !defined (ACE_LACKS_SYS_TIME_H)
#  include /**/ <sys/time.h>
#endif /* !ACE_LACKS_SYS_TIME_H */

// Place all additions (especially function declarations) within extern "C" {}
#ifdef __cplusplus
extern "C"
{
#endif /* __cplusplus */

#if defined (ACE_HAS_SVR4_GETTIMEOFDAY)
# if !defined (m88k) && !defined (SCO)
  int gettimeofday (struct timeval *tp, void * = 0);
# else
  int gettimeofday (struct timeval *tp);
# endif  /*  !m88k && !SCO */
#elif defined (ACE_HAS_OSF1_GETTIMEOFDAY)
  int gettimeofday (struct timeval *tp, struct timezone * = 0);
#elif defined (ACE_HAS_SUNOS4_GETTIMEOFDAY)
# define ACE_HAS_SVR4_GETTIMEOFDAY
#endif /* ACE_HAS_SVR4_GETTIMEOFDAY */

#ifdef __cplusplus
}
#endif /* __cplusplus */

#include /**/ "ace/post.h"
#endif /* ACE_OS_INCLUDE_SYS_OS_TIME_H */
