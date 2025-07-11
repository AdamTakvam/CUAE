/* -*- C++ -*- */

//==========================================================================
/**
 *  @file    FIFO.h
 *
 *  FIFO.h,v 4.13 2002/05/02 04:08:13 ossama Exp
 *
 *  @author Doug Schmidt
 */
//==========================================================================


#ifndef ACE_FIFO_H
#define ACE_FIFO_H
#include "ace/pre.h"

#include "ace/IPC_SAP.h"

#if !defined (ACE_LACKS_PRAGMA_ONCE)
# pragma once
#endif /* ACE_LACKS_PRAGMA_ONCE */

/**
 * @class ACE_FIFO
 *
 * @brief Abstract base class for UNIX FIFOs
 *
 * UNIX FIFOs are also known Named Pipes, which are totally
 * unrelated to Win32 Named Pipes.  If you want to use a local
 * IPC mechanism that will be portable to both UNIX and Win32,
 * take a look at the <ACE_SPIPE_*> classes.
 */
class ACE_Export ACE_FIFO : public ACE_IPC_SAP
{
public:
  /// Open up the named pipe on the <rendezvous> in accordance with the
  /// flags.
  int open (const ACE_TCHAR *rendezvous, int flags, int perms,
            LPSECURITY_ATTRIBUTES sa = 0);

  /// Close down the ACE_FIFO without removing the rendezvous point.
  int close (void);

  /// Close down the ACE_FIFO and remove the rendezvous point from the
  /// file system.
  int remove (void);

  /// Return the local address of this endpoint.
  int get_local_addr (const ACE_TCHAR *&rendezvous) const;

  /// Dump the state of an object.
  void dump (void) const;

  /// Declare the dynamic allocation hooks.
  ACE_ALLOC_HOOK_DECLARE;

protected:
  // = Make these protected to ensure that the class is "abstract."
  /// Default constructor.
  ACE_FIFO (void);

  /// Open up the named pipe on the <rendezvous> in accordance with the
  /// flags.
  ACE_FIFO (const ACE_TCHAR *rendezvous, int flags, int perms,
            LPSECURITY_ATTRIBUTES sa = 0);

private:
  /// Rendezvous point in the file system.
  ACE_TCHAR rendezvous_[MAXPATHLEN + 1];
};

#if defined (__ACE_INLINE__)
#include "ace/FIFO.i"
#endif /* __ACE_INLINE__ */

#include "ace/post.h"
#endif /* ACE_FIFO_H */
