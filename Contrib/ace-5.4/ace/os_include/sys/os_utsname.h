// -*- C++ -*-

//=============================================================================
/**
 *  @file    os_utsname.h
 *
 *  system name structure
 *
 *  os_utsname.h,v 1.2 2003/07/19 19:04:16 dhinton Exp
 *
 *  @author Don Hinton <dhinton@dresystems.com>
 *  @author This code was originally in various places including ace/OS.h.
 */
//=============================================================================

#ifndef ACE_OS_INCLUDE_SYS_OS_UTSNAME_H
#define ACE_OS_INCLUDE_SYS_OS_UTSNAME_H

#include /**/ "ace/pre.h"

#include "ace/config-all.h"

#if !defined (ACE_LACKS_PRAGMA_ONCE)
# pragma once
#endif /* ACE_LACKS_PRAGMA_ONCE */

#if !defined (ACE_LACKS_SYS_UTSNAME_H)
#  include /**/ <sys/utsname.h>
#endif /* !ACE_LACKS_SYS_UTSNAME_H */

// Place all additions (especially function declarations) within extern "C" {}
#ifdef __cplusplus
extern "C"
{
#endif /* __cplusplus */

#ifdef __cplusplus
}
#endif /* __cplusplus */

#include /**/ "ace/post.h"
#endif /* ACE_OS_INCLUDE_SYS_OS_UTSNAME_H */
