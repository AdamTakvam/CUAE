// -*- C++ -*-

//=============================================================================
/**
 *  @file    os_grp.h
 *
 *  group structure
 *
 *  os_grp.h,v 1.2 2003/07/19 19:04:15 dhinton Exp
 *
 *  @author Don Hinton <dhinton@dresystems.com>
 *  @author This code was originally in various places including ace/OS.h.
 */
//=============================================================================

#ifndef ACE_OS_INCLUDE_OS_GRP_H
#define ACE_OS_INCLUDE_OS_GRP_H

#include /**/ "ace/pre.h"

#include "ace/config-all.h"

#if !defined (ACE_LACKS_PRAGMA_ONCE)
# pragma once
#endif /* ACE_LACKS_PRAGMA_ONCE */

#include "ace/os_include/sys/os_types.h" // for gid_t

#if !defined (ACE_LACKS_GRP_H)
# include /**/ <grp.h>
#endif /* !ACE_LACKS_GRP_H */

// Place all additions (especially function declarations) within extern "C" {}
#ifdef __cplusplus
extern "C"
{
#endif /* __cplusplus */

#ifdef __cplusplus
}
#endif /* __cplusplus */

#include /**/ "ace/post.h"
#endif /* ACE_OS_INCLUDE_OS_GRP_H */
