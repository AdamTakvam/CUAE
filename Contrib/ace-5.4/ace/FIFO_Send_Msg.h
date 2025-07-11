/* -*- C++ -*- */

//=============================================================================
/**
 *  @file    FIFO_Send_Msg.h
 *
 *  FIFO_Send_Msg.h,v 4.14 2003/07/19 19:04:11 dhinton Exp
 *
 *  @author Doug Schmidt
 */
//=============================================================================


#ifndef ACE_FIFO_SEND_MSG_H
#define ACE_FIFO_SEND_MSG_H
#include /**/ "ace/pre.h"

#include "ace/FIFO_Send.h"

#if !defined (ACE_LACKS_PRAGMA_ONCE)
# pragma once
#endif /* ACE_LACKS_PRAGMA_ONCE */

// Forward Decls
class ACE_Str_Buf;

/**
 * @class ACE_FIFO_Send_Msg
 *
 * @brief Sender side for the Record oriented C++ wrapper for UNIX
 * FIFOs.
 */
class ACE_Export ACE_FIFO_Send_Msg : public ACE_FIFO_Send
{
public:
  // = Initialization methods.
  /// Default constructor.
  ACE_FIFO_Send_Msg (void);

  /// Open up a record-oriented named pipe for writing.
  ACE_FIFO_Send_Msg (const ACE_TCHAR *rendezvous,
                     int flags = O_WRONLY,
                     int perms = ACE_DEFAULT_FILE_PERMS,
                     LPSECURITY_ATTRIBUTES sa = 0);

  /// Open up a record-oriented named pipe for writing.
  int open (const ACE_TCHAR *rendezvous,
            int flags = O_WRONLY,
            int perms = ACE_DEFAULT_FILE_PERMS,
            LPSECURITY_ATTRIBUTES sa = 0);

  /// Send <buf> of up to <len> bytes.
  ssize_t send (const ACE_Str_Buf &msg);

  /// Send <buf> of exactly <len> bytes (block until done).
  ssize_t send (const void *buf, size_t len);

#if defined (ACE_HAS_STREAM_PIPES)
  /// Send <data> and <cntl> message via Stream pipes.
  ssize_t send (const ACE_Str_Buf *data,
                const ACE_Str_Buf *cntl = 0,
                int flags = 0);

  /// Send <data> and <cntl> message via Stream pipes in "band" mode.
  ssize_t send (int band,
                const ACE_Str_Buf *data,
                const ACE_Str_Buf *cntl = 0,
                int flags = MSG_BAND);
#endif /* ACE_HAS_STREAM_PIPES */

  /// Dump the state of an object.
  void dump (void) const;

  /// Declare the dynamic allocation hooks.
  ACE_ALLOC_HOOK_DECLARE;
};

#if !defined (ACE_LACKS_INLINE_FUNCTIONS)
#include "ace/FIFO_Send_Msg.i"
#endif

#include /**/ "ace/post.h"
#endif /* ACE_FIFO_SEND_MSG_H */
