/* -*- C++ -*- */

//=============================================================================
/**
 *  @file    SOCK_CODgram.h
 *
 *  SOCK_CODgram.h,v 4.11 2001/12/03 21:30:09 shuston Exp
 *
 *  @author Doug Schmidt
 */
//=============================================================================


#ifndef ACE_SOCK_CODGRAM_H
#define ACE_SOCK_CODGRAM_H
#include "ace/pre.h"

#include "ace/SOCK_IO.h"

#if !defined (ACE_LACKS_PRAGMA_ONCE)
# pragma once
#endif /* ACE_LACKS_PRAGMA_ONCE */

#include "ace/Addr.h"
#include "ace/INET_Addr.h"

/**
 * @class ACE_SOCK_CODgram
 *
 * @brief Defines the member functions for the ACE_SOCK connected
 * datagram abstraction.
 */
class ACE_Export ACE_SOCK_CODgram : public ACE_SOCK_IO
{
public:
  // = Initialization methods.
  /// Default constructor.
  ACE_SOCK_CODgram (void);

  ACE_SOCK_CODgram (const ACE_Addr &remote_sap,
                    const ACE_Addr &local_sap = ACE_Addr::sap_any,
                    int protocol_family = ACE_PROTOCOL_FAMILY_INET,
                    int protocol = 0,
                    int reuse_addr = 0);

  /// Default dtor.
  ~ACE_SOCK_CODgram (void);

  // Initiate a connected dgram.

  /// Initiate a connected dgram.
  int open (const ACE_Addr &remote_sap,
            const ACE_Addr &local_sap = ACE_Addr::sap_any,
            int protocol_family = ACE_PROTOCOL_FAMILY_INET,
            int protocol = 0,
            int reuse_addr = 0);

  /// Dump the state of an object.
  void dump (void) const;

  /// Declare the dynamic allocation hooks.
  ACE_ALLOC_HOOK_DECLARE;
};

#if !defined (ACE_LACKS_INLINE_FUNCTIONS)
#include "ace/SOCK_CODgram.i"
#endif

#include "ace/post.h"
#endif /* ACE_SOCK_CODGRAM_H */
