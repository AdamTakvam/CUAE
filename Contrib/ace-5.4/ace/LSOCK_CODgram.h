/* -*- C++ -*- */

//=============================================================================
/**
 *  @file    LSOCK_CODgram.h
 *
 *  LSOCK_CODgram.h,v 4.11 2003/07/19 19:04:11 dhinton Exp
 *
 *  @author Douglas C. Schmidt <schmidt@cs.wustl.edu>
 */
//=============================================================================


#ifndef ACE_LOCAL_SOCK_CODGRAM_H
#define ACE_LOCAL_SOCK_CODGRAM_H
#include /**/ "ace/pre.h"

#include "ace/config-all.h"

#if !defined (ACE_LACKS_PRAGMA_ONCE)
# pragma once
#endif /* ACE_LACKS_PRAGMA_ONCE */

#if !defined (ACE_LACKS_UNIX_DOMAIN_SOCKETS)

#include "ace/LSOCK.h"
#include "ace/SOCK_CODgram.h"
#include "ace/Addr.h"

/**
 * @class ACE_LSOCK_CODgram
 *
 * @brief Defines the member functions for the <ACE_LSOCK> connected
 * datagram abstraction.
 */
class ACE_Export ACE_LSOCK_CODgram : public ACE_SOCK_CODgram, public ACE_LSOCK
{
public:
  // = Initialization methods.
  /// Default constructor.
  ACE_LSOCK_CODgram (void);

  /// Initiate a connected-datagram.
  ACE_LSOCK_CODgram (const ACE_Addr &remote_sap,
                     const ACE_Addr &local_sap = ACE_Addr::sap_any,
                     int protocol_family = PF_UNIX,
                     int protocol = 0);

  /// Initiate a connected-datagram.
  int open (const ACE_Addr &remote_sap,
            const ACE_Addr &local_sap = ACE_Addr::sap_any,
            int protocol_family = PF_UNIX,
            int protocol = 0);

  /// Get underlying handle.
  ACE_HANDLE get_handle (void) const;

  /// Set underlying handle.
  void set_handle (ACE_HANDLE);

  /// Dump the state of an object.
  void dump (void) const;

  /// Declare the dynamic allocation hooks.
  ACE_ALLOC_HOOK_DECLARE;
};

#if !defined (ACE_LACKS_INLINE_FUNCTIONS)
#include "ace/LSOCK_CODgram.i"
#endif

#endif /* ACE_LACKS_UNIX_DOMAIN_SOCKETS */
#include /**/ "ace/post.h"
#endif /* ACE_LOCAL_SOCK_CODGRAM_H */
