// -*- C++ -*-

//=============================================================================
/**
 *  @file    os_un.h
 *
 *  definitions for UNIX domain sockets
 *
 *  os_un.h,v 1.3 2003/07/19 19:04:16 dhinton Exp
 *
 *  @author Don Hinton <dhinton@dresystems.com>
 *  @author This code was originally in various places including ace/OS.h.
 */
//=============================================================================

#ifndef ACE_OS_INCLUDE_SYS_OS_UN_H
#define ACE_OS_INCLUDE_SYS_OS_UN_H

#include /**/ "ace/pre.h"

#include "ace/config-all.h"

#if !defined (ACE_LACKS_PRAGMA_ONCE)
# pragma once
#endif /* ACE_LACKS_PRAGMA_ONCE */

#include "ace/os_include/sys/os_socket.h"

#if !defined (ACE_LACKS_SYS_UN_H)
#  include /**/ <sys/un.h>
#endif /* !ACE_LACKS_SYS_UN_H */

// Place all additions (especially function declarations) within extern "C" {}
#ifdef __cplusplus
extern "C"
{
#endif /* __cplusplus */

#if defined (VXWORKS)
struct sockaddr_un {
  short sun_family;    // AF_UNIX.
  char  sun_path[108]; // path name.
};
#endif /* VXWORKS */


#ifdef __cplusplus
}
#endif /* __cplusplus */

#include /**/ "ace/post.h"
#endif /* ACE_OS_INCLUDE_SYS_OS_UN_H */
