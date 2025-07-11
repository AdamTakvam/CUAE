/* -*- C++ -*- */

//=============================================================================
/**
 *  @file    LSOCK_Stream.h
 *
 *  LSOCK_Stream.h,v 4.16 2003/07/19 19:04:11 dhinton Exp
 *
 *  @author Doug Schmidt
 */
//=============================================================================


#ifndef ACE_LOCAL_SOCK_STREAM_H
#define ACE_LOCAL_SOCK_STREAM_H
#include /**/ "ace/pre.h"

#include "ace/config-all.h"

#if !defined (ACE_LACKS_PRAGMA_ONCE)
# pragma once
#endif /* ACE_LACKS_PRAGMA_ONCE */

#if !defined (ACE_LACKS_UNIX_DOMAIN_SOCKETS)

#include "ace/SOCK_Stream.h"
#include "ace/UNIX_Addr.h"
#include "ace/LSOCK.h"

/**
 * @class ACE_LSOCK_Stream
 *
 * @brief Create a Local ACE_SOCK stream.
 */
class ACE_Export ACE_LSOCK_Stream : public ACE_SOCK_Stream, public ACE_LSOCK
{
public:
  // = Send/recv methods.
  /// Send iovecs via <::writev>.
  ssize_t send_msg (const iovec iov[],
                    size_t n,
                    ACE_HANDLE handle);

  /// Send iovecs via <::writev>.
  ssize_t recv_msg (iovec iov[],
                    size_t n,
                    ACE_HANDLE &handle);

  /// Get handle.
  ACE_HANDLE get_handle (void) const;

  /// Overrides set_handle from the base classes.
  void set_handle (ACE_HANDLE fd);

  // = Meta-type info
  typedef ACE_UNIX_Addr PEER_ADDR;

  /// Dump the state of an object.
  void dump (void) const;

  /// Declare the dynamic allocation hooks.
  ACE_ALLOC_HOOK_DECLARE;

  /// This method simply returns the "local" addr.
  int get_local_addr (ACE_Addr &) const;

  /// This method returns the "local" addr since it's the same value
  /// for UNIX domain sockets.
  int get_remote_addr (ACE_Addr &) const;
};

#if !defined (ACE_LACKS_INLINE_FUNCTIONS)
#include "ace/LSOCK_Stream.i"
#endif

#endif /* ACE_LACKS_UNIX_DOMAIN_SOCKETS */
#include /**/ "ace/post.h"
#endif /* ACE_LOCAL_SOCK_STREAM_H */
