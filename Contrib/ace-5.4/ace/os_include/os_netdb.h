// -*- C++ -*-

//=============================================================================
/**
 *  @file    os_netdb.h
 *
 *  definitions for network database operations
 *
 *  os_netdb.h,v 1.4 2003/07/19 19:04:15 dhinton Exp
 *
 *  @author Don Hinton <dhinton@dresystems.com>
 *  @author This code was originally in various places including ace/OS.h.
 */
//=============================================================================

#ifndef ACE_OS_INCLUDE_OS_NETDB_H
#define ACE_OS_INCLUDE_OS_NETDB_H

#include /**/ "ace/pre.h"

#include "ace/config-all.h"

#if !defined (ACE_LACKS_PRAGMA_ONCE)
# pragma once
#endif /* ACE_LACKS_PRAGMA_ONCE */

#include "ace/os_include/netinet/os_in.h"
#include "ace/os_include/os_limits.h"

#if !defined (ACE_LACKS_NETDB_H)
#  if defined (ACE_HAS_STL_QUEUE_CONFLICT)
#    define queue _Queue_
#  endif /* ACE_HAS_STL_QUEUE_CONFLICT */
   extern "C" {
#  include /**/ <netdb.h>
   }
#  if defined (ACE_HAS_STL_QUEUE_CONFLICT)
#    undef queue
#  endif /* ACE_HAS_STL_QUEUE_CONFLICT */
#endif /* !ACE_LACKS_NETDB_H */

#if defined (VXWORKS)
#  include /**/ <hostLib.h>
#endif /* VXWORKS */

// Place all additions (especially function declarations) within extern "C" {}
#ifdef __cplusplus
extern "C"
{
#endif /* __cplusplus */

// VxWorks does define these, at least on a Sun cross compile, so this
// may need to be added back for windows.
#if defined (ACE_PSOS) //|| defined (VXWORKS)
   struct  hostent {
     char    *h_name;        /* official name of host */
     char    **h_aliases;    /* alias list */
     int     h_addrtype;     /* host address type */
     int     h_length;       /* address length */
     char    **h_addr_list;  /* (first, only) address from name server */
#    define h_addr h_addr_list[0]   /* the first address */
   };

  struct  servent {
    char     *s_name;    /* official service name */
    char    **s_aliases; /* alias list */
    int       s_port;    /* port # */
    char     *s_proto;   /* protocol to use */
  };
#endif /* ACE_PSOS */

#if defined (ACE_HAS_STRUCT_NETDB_DATA)
   typedef char ACE_HOSTENT_DATA[sizeof(struct hostent_data)];
   typedef char ACE_SERVENT_DATA[sizeof(struct servent_data)];
   typedef char ACE_PROTOENT_DATA[sizeof(struct protoent_data)];
#else
#  if !defined ACE_HOSTENT_DATA_SIZE
#    define ACE_HOSTENT_DATA_SIZE (4*1024)
#  endif /*ACE_HOSTENT_DATA_SIZE */
#  if !defined ACE_SERVENT_DATA_SIZE
#    define ACE_SERVENT_DATA_SIZE (4*1024)
#  endif /*ACE_SERVENT_DATA_SIZE */
#  if !defined ACE_PROTOENT_DATA_SIZE
#    define ACE_PROTOENT_DATA_SIZE (2*1024)
#  endif /*ACE_PROTOENT_DATA_SIZE */
   typedef char ACE_HOSTENT_DATA[ACE_HOSTENT_DATA_SIZE];
   typedef char ACE_SERVENT_DATA[ACE_SERVENT_DATA_SIZE];
   typedef char ACE_PROTOENT_DATA[ACE_PROTOENT_DATA_SIZE];
#endif /* ACE_HAS_STRUCT_NETDB_DATA */

# if !defined(MAXHOSTNAMELEN)
#   define MAXHOSTNAMELEN  HOST_NAME_MAX
# endif /* MAXHOSTNAMELEN */

#ifdef __cplusplus
}
#endif /* __cplusplus */

#include /**/ "ace/post.h"
#endif /* ACE_OS_INCLUDE_OS_NETDB_H */
