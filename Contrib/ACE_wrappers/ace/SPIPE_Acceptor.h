/* -*- C++ -*- */

//=============================================================================
/**
 *  @file    SPIPE_Acceptor.h
 *
 *  SPIPE_Acceptor.h,v 4.19 2002/04/03 23:37:13 shuston Exp
 *
 *  @author Doug Schmidt
 *  @author Prashant Jain
 */
//=============================================================================


#ifndef ACE_SPIPE_ACCEPTOR_H
#define ACE_SPIPE_ACCEPTOR_H
#include "ace/pre.h"

#include "ace/SPIPE_Stream.h"

#if !defined (ACE_LACKS_PRAGMA_ONCE)
# pragma once
#endif /* ACE_LACKS_PRAGMA_ONCE */

#if defined (ACE_WIN32)
#include "ace/Synch.h"
#endif /* ACE_WIN32 */

/**
 * @class ACE_SPIPE_Acceptor
 *
 * @brief A factory class that produces ACE_SPIPE_Stream objects.
 *
 * ACE_SPIPE_Acceptor is a factory class that accepts SPIPE connections.
 * Each accepted connection produces an ACE_SPIPE_Stream object.
 *
 * @warning Windows: Works only on Windows NT 4 and higher. To use this
 * class with the ACE_Reactor framework, note that the handle to
 * demultiplex on is an event handle and should be registered with the
 * ACE_Reactor::register_handler (ACE_Event_Handler *, ACE_HANDLE) method.
 *
 * @warning Works on non-Windows platforms only when @c ACE_HAS_STREAM_PIPES
 * is defined.
 * 
 */
class ACE_Export ACE_SPIPE_Acceptor : public ACE_SPIPE
{
public:
  // = Initialization and termination methods.
  /// Default constructor.
  ACE_SPIPE_Acceptor (void);

  /// Initiate a passive-mode STREAM pipe listener.
  /**
   * @param local_sap   The name of the pipe instance to open and listen on.
   * @param reuse_addr  Optional, and ignored. Needed for API compatibility
   *                    with other acceptor classes.
   * @param perms       Optional, the protection mask to create the pipe
   *                    with. Ignored on Windows.
   * @param sa          Optional, ignored on non-Windows. The
   *                    SECURITY_ATTRIBUTES to create the named pipe
   *                    instances with. This pointer is remembered and
   *                    reused on each new named pipe instance, so only
   *                    pass a value that remains valid as long as this
   *                    object does.
   */
  ACE_SPIPE_Acceptor (const ACE_SPIPE_Addr &local_sap,
                      int reuse_addr = 1,
                      int perms = ACE_DEFAULT_FILE_PERMS,
                      LPSECURITY_ATTRIBUTES sa = 0);

  /// Initiate a passive-mode STREAM pipe listener.
  /**
   * @param local_sap   The name of the pipe instance to open and listen on.
   * @param reuse_addr  Optional, and ignored. Needed for API compatibility
   *                    with other acceptor classes.
   * @param perms       Optional, the protection mask to create the pipe
   *                    with. Ignored on Windows.
   * @param sa          Optional, ignored on non-Windows. The
   *                    SECURITY_ATTRIBUTES to create the named pipe
   *                    instances with. This pointer is remembered and
   *                    reused on each new named pipe instance, so only
   *                    pass a value that remains valid as long as this
   *                    object does.
   *
   * @retval 0 for success.
   * @retval -1 for failure.
   */
  int open (const ACE_SPIPE_Addr &local_sap,
            int reuse_addr = 1,
            int perms = ACE_DEFAULT_FILE_PERMS,
            LPSECURITY_ATTRIBUTES sa = 0);

  /// Close down the passive-mode STREAM pipe listener.
  int close (void);

  /// Remove the underlying mounted pipe from the file system.
  int remove (void);

  // = Passive connection acceptance method.
  /**
   * Accept a new data transfer connection.
   *
   * @param ipc_sap_spipe  The ACE_SPIPE_Stream to initialize with the
   *                       newly-accepted pipe.
   * @param remote_addr    Optional, accepts the address of the peer.
   * @param timeout        0 means block forever, {0, 0} means poll.
   * @param restart        1 means "restart if interrupted."
   *
   * @retval 0 for success.
   * @retval -1 for failure.
   */
  int accept (ACE_SPIPE_Stream &ipc_sap_spipe,
              ACE_SPIPE_Addr *remote_addr = 0,
              ACE_Time_Value *timeout = 0,
              int restart = 1,
              int reset_new_handle = 0);

  // = Meta-type info
  typedef ACE_SPIPE_Addr PEER_ADDR;
  typedef ACE_SPIPE_Stream PEER_STREAM;

  /// Dump the state of an object.
  void dump (void) const;

  /// Declare the dynamic allocation hooks.
  ACE_ALLOC_HOOK_DECLARE;

private:
  /// Create a new instance of an SPIPE.
  int create_new_instance (int perms = 0);

#if (defined (ACE_WIN32) && defined (ACE_HAS_WINNT4) && (ACE_HAS_WINNT4 != 0))
  // On Windows, the SECURITY_ATTRIBUTES specified for the initial accept
  // operation is reused on all subsequent pipe instances as well.
  LPSECURITY_ATTRIBUTES sa_;

  // On Windows, the handle maintained in the ACE_IPC_SAP class is the
  // event handle from event_. The pipe handle is useless for telling
  // when a pipe connect is done/ready, and it changes on each pipe
  // acceptance, quite unlike other acceptor-type classes in ACE.
  // This allows the get_handle()-obtained handle to be used for
  // registering with the reactor (albeit for signal, not input)
  // to tell when a pipe accept is done.
  ACE_OVERLAPPED   overlapped_;
  ACE_Manual_Event event_;
  ACE_HANDLE       pipe_handle_;
  int              already_connected_;
#endif /* ACE_WIN32 */

};

#include "ace/post.h"
#endif /* ACE_SPIPE_ACCEPTOR_H */
