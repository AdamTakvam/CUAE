// -*- C++ -*-

//==========================================================================
/**
 *  @file    FIFO_Send.h
 *
 *  FIFO_Send.h,v 4.13 2002/05/02 04:08:13 ossama Exp
 *
 *  @author Doug Schmidt
 */
//==========================================================================


#ifndef ACE_FIFO_SEND_H
#define ACE_FIFO_SEND_H

#include "ace/pre.h"

#include "ace/FIFO.h"

#if !defined (ACE_LACKS_PRAGMA_ONCE)
# pragma once
#endif /* ACE_LACKS_PRAGMA_ONCE */

#include "ace/ACE.h"

/**
 * @class ACE_FIFO_Send
 *
 * @brief Sender side for the bytestream C++ wrapper for UNIX FIFOs
 */
class ACE_Export ACE_FIFO_Send : public ACE_FIFO
{
public:
  // = Initialization methods.
  /// Default constructor.
  ACE_FIFO_Send (void);

  /// Open up a bytestream named pipe for writing.
  ACE_FIFO_Send (const ACE_TCHAR *rendezvous,
                 int flags = O_WRONLY,
                 int perms = ACE_DEFAULT_FILE_PERMS,
                 LPSECURITY_ATTRIBUTES sa = 0);

  /// Open up a bytestream named pipe for writing.
  int open (const ACE_TCHAR *rendezvous,
            int flags = O_WRONLY,
            int perms = ACE_DEFAULT_FILE_PERMS,
            LPSECURITY_ATTRIBUTES sa = 0);

  /// Send <buf> of up to <len> bytes.
  ssize_t send (const void *buf, size_t len);

  /// Send <buf> of exactly <len> bytes (block until done).
  ssize_t send_n (const void *buf, size_t len);

  /// Dump the state of an object.
  void dump (void) const;

  /// Declare the dynamic allocation hooks.
  ACE_ALLOC_HOOK_DECLARE;
};

#if !defined (ACE_LACKS_INLINE_FUNCTIONS)
#include "ace/FIFO_Send.i"
#endif

#include "ace/post.h"

#endif /* ACE_FIFO_SEND_H */
