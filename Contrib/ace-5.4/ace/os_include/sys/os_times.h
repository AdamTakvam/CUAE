// -*- C++ -*-

//=============================================================================
/**
 *  @file    os_times.h
 *
 *  file access and modification times structure
 *
 *  os_times.h,v 1.3 2003/11/01 11:15:19 dhinton Exp
 *
 *  @author Don Hinton <dhinton@dresystems.com>
 *  @author This code was originally in various places including ace/OS.h.
 */
//=============================================================================

#ifndef ACE_OS_INCLUDE_SYS_OS_TIMES_H
#define ACE_OS_INCLUDE_SYS_OS_TIMES_H

#include /**/ "ace/pre.h"

#include "ace/config-all.h"

#if !defined (ACE_LACKS_PRAGMA_ONCE)
# pragma once
#endif /* ACE_LACKS_PRAGMA_ONCE */

#include "ace/os_include/sys/os_types.h"

#if !defined (ACE_LACKS_SYS_TIMES_H)
#  include /**/ <sys/times.h>
#endif /* !ACE_LACKS_SYS_TIMES_H */

// Place all additions (especially function declarations) within extern "C" {}
#ifdef __cplusplus
extern "C"
{
#endif /* __cplusplus */

#ifdef __cplusplus
}
#endif /* __cplusplus */

#include /**/ "ace/post.h"
#endif /* ACE_OS_INCLUDE_SYS_OS_TIMES_H */
