// -*- C++ -*-

//=============================================================================
/**
 *  @file    os_in.h
 *
 *  Internet address family
 *
 *  os_in.h,v 1.3 2003/11/01 11:15:19 dhinton Exp
 *
 *  @author Don Hinton <dhinton@dresystems.com>
 *  @author This code was originally in various places including ace/OS.h.
 */
//=============================================================================

#ifndef ACE_OS_INCLUDE_NETINET_OS_IN_H
#define ACE_OS_INCLUDE_NETINET_OS_IN_H

#include /**/ "ace/pre.h"

#include "ace/config-all.h"

#if !defined (ACE_LACKS_PRAGMA_ONCE)
# pragma once
#endif /* ACE_LACKS_PRAGMA_ONCE */

#include "ace/os_include/os_inttypes.h"
#include "ace/os_include/sys/os_socket.h"

#if defined (ACE_HAS_WINSOCK2) && (ACE_HAS_WINSOCK2 != 0)
#  include /**/ <ws2tcpip.h>
#endif /* ACE_HAS_WINSOCK2 */

#if !defined (ACE_LACKS_NETINET_IN_H)
#  if defined (ACE_HAS_STL_QUEUE_CONFLICT)
#    define queue _Queue_
#  endif /* ACE_HAS_STL_QUEUE_CONFLICT */
   extern "C" {
#  include /**/ <netinet/in.h>
   }
#  if defined (ACE_HAS_STL_QUEUE_CONFLICT)
#    undef queue
#  endif /* ACE_HAS_STL_QUEUE_CONFLICT */
#endif /* !ACE_LACKS_NETINET_IN_H */

// Place all additions (especially function declarations) within extern "C" {}
#ifdef __cplusplus
extern "C"
{
#endif /* __cplusplus */

# if defined (ACE_HAS_PHARLAP_RT)
#   define ACE_IPPROTO_TCP SOL_SOCKET
# else
#   define ACE_IPPROTO_TCP IPPROTO_TCP
# endif /* ACE_HAS_PHARLAP_RT */

# if !defined (ACE_HAS_IP_MULTICAST)  &&  defined (ACE_LACKS_IP_ADD_MEMBERSHIP)
  // Even if ACE_HAS_IP_MULTICAST is not defined, if IP_ADD_MEMBERSHIP
  // is defined, assume that the ip_mreq struct is also defined
  // (presumably in netinet/in.h).
  struct ip_mreq
  {
    /// IP multicast address of group
    struct in_addr imr_multiaddr;
    /// Local IP address of interface
    struct in_addr imr_interface;
  };
# endif /* ! ACE_HAS_IP_MULTICAST  &&  ACE_LACKS_IP_ADD_MEMBERSHIP */

#if !defined (IPPORT_RESERVED)
#  define IPPORT_RESERVED       1024
#endif /* !IPPORT_RESERVED */

#if !defined (IPPORT_USERRESERVED)
#  define IPPORT_USERRESERVED       5000
#endif /* !IPPORT_USERRESERVED */

// Define INET loopback address constant if it hasn't been defined
// Dotted Decimal 127.0.0.1 == Hexidecimal 0x7f000001
#if !defined (INADDR_LOOPBACK)
#  define INADDR_LOOPBACK ((ACE_UINT32) 0x7f000001)
#endif /* INADDR_LOOPBACK */

// The INADDR_NONE address is generally 255.255.255.255.
#if !defined (INADDR_NONE)
#  define INADDR_NONE ((ACE_UINT32) 0xffffffff)
#endif /* INADDR_NONE */

// Define INET string length constants if they haven't been defined
//
// for IPv4 dotted-decimal
#if !defined (INET_ADDRSTRLEN)
#  define INET_ADDRSTRLEN 16
#endif /* INET_ADDRSTRLEN */
//
// for IPv6 hex string
#if !defined (INET6_ADDRSTRLEN)
#  define INET6_ADDRSTRLEN 46
#endif /* INET6_ADDRSTRLEN */

# if !defined (IP_DROP_MEMBERSHIP)
#   define IP_DROP_MEMBERSHIP 0
# endif /* IP_DROP_MEMBERSHIP */

# if !defined (IP_ADD_MEMBERSHIP)
#   define IP_ADD_MEMBERSHIP 0
#   define ACE_LACKS_IP_ADD_MEMBERSHIP
# endif /* IP_ADD_MEMBERSHIP */

# if !defined (IP_DEFAULT_MULTICAST_TTL)
#   define IP_DEFAULT_MULTICAST_TTL 0
# endif /* IP_DEFAULT_MULTICAST_TTL */

# if !defined (IP_DEFAULT_MULTICAST_LOOP)
#   define IP_DEFAULT_MULTICAST_LOOP 0
# endif /* IP_DEFAULT_MULTICAST_LOOP */

# if !defined (IP_MULTICAST_IF)
#   define IP_MULTICAST_IF 0
#endif /* IP_MULTICAST_IF */

# if !defined (IP_MULTICAST_TTL)
#   define IP_MULTICAST_TTL 1
#endif /* IP_MULTICAST_TTL */

# if !defined (IP_MAX_MEMBERSHIPS)
#   define IP_MAX_MEMBERSHIPS 0
# endif /* IP_MAX_MEMBERSHIP */

#ifdef __cplusplus
}
#endif /* __cplusplus */

#include /**/ "ace/post.h"
#endif /* ACE_OS_INCLUDE_NETINET_OS_IN_H */
