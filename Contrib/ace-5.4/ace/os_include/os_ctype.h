// -*- C++ -*-

//=============================================================================
/**
 *  @file    os_ctype.h
 *
 *  character types
 *
 *  os_ctype.h,v 1.2 2003/07/19 19:04:15 dhinton Exp
 *
 *  @author Don Hinton <dhinton@dresystems.com>
 *  @author This code was originally in various places including ace/OS.h.
 */
//=============================================================================

#ifndef ACE_OS_INCLUDE_OS_CTYPE_H
#define ACE_OS_INCLUDE_OS_CTYPE_H

#include /**/ "ace/pre.h"

#include "ace/config-all.h"

#if !defined (ACE_LACKS_PRAGMA_ONCE)
# pragma once
#endif /* ACE_LACKS_PRAGMA_ONCE */

#if !defined (ACE_LACKS_CTYPE_H)
# include /**/ <ctype.h>
#endif /* !ACE_LACKS_CTYPE_H */

// Place all additions (especially function declarations) within extern "C" {}
#ifdef __cplusplus
extern "C"
{
#endif /* __cplusplus */

// @todo move the is* and is* emulation methods in ACE_OS here
// and let ACE_OS just call them.

#ifdef __cplusplus
}
#endif /* __cplusplus */

#include /**/ "ace/post.h"
#endif /* ACE_OS_INCLUDE_OS_CTYPE_H */
