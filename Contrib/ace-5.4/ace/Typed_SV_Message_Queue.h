/* -*- C++ -*- */

//=============================================================================
/**
 *  @file    Typed_SV_Message_Queue.h
 *
 *  Typed_SV_Message_Queue.h,v 4.15 2003/07/19 19:04:14 dhinton Exp
 *
 *  @author Douglas C. Schmidt <schmidt@cs.wustl.edu>
 */
//=============================================================================

#ifndef ACE_TYPED_MESSAGE_QUEUE_H
#define ACE_TYPED_MESSAGE_QUEUE_H
#include /**/ "ace/pre.h"

#include "ace/SV_Message_Queue.h"

#if !defined (ACE_LACKS_PRAGMA_ONCE)
# pragma once
#endif /* ACE_LACKS_PRAGMA_ONCE */

#include "ace/Typed_SV_Message.h"

/**
 * @class ACE_Typed_SV_Message_Queue
 *
 * @brief Defines the header file for the C++ wrapper facade for typed message queues.
 */
template <class T>
class ACE_Typed_SV_Message_Queue
{
public:
  enum
  {
    ACE_CREATE = IPC_CREAT,
    ACE_OPEN = 0,
    ACE_NOWAIT = IPC_NOWAIT
  };

  // = Initialization and termination operations.
  ACE_Typed_SV_Message_Queue (void);
  ACE_Typed_SV_Message_Queue (key_t external_id,
                              int create = ACE_OPEN,
                              int perms = ACE_DEFAULT_FILE_PERMS);
  int open (key_t external_id,
            int create = ACE_OPEN,
            int perms = ACE_DEFAULT_FILE_PERMS);
  int close (void);
  int remove (void);
  ~ACE_Typed_SV_Message_Queue (void);

  /// Send method.
  int send (const ACE_Typed_SV_Message<T> &mb, int mflags = 0);

  /// Recv method.
  int recv (ACE_Typed_SV_Message<T> &mb, int mflags = 0);

  /// Return the id of the underlying ACE_SV_Message_Queue.
  int get_id (void) const;

  /// Control the underlying message queue.
  int control (int option, void *arg = 0);

  /// Dump the state of an object.
  void dump (void) const;

  /// Declare the dynamic allocation hooks.
  ACE_ALLOC_HOOK_DECLARE;

private:
  ACE_SV_Message_Queue message_queue_;
};

#if defined (__ACE_INLINE__)
#include "ace/Typed_SV_Message_Queue.i"
#endif /* __ACE_INLINE__ */

#if defined (ACE_TEMPLATES_REQUIRE_SOURCE)
#include "ace/Typed_SV_Message_Queue.cpp"
#endif /* ACE_TEMPLATES_REQUIRE_SOURCE */

#if defined (ACE_TEMPLATES_REQUIRE_PRAGMA)
#pragma implementation ("Typed_SV_Message_Queue.cpp")
#endif /* ACE_TEMPLATES_REQUIRE_PRAGMA */

#include /**/ "ace/post.h"
#endif /* ACE_TYPED_MESSAGE_QUEUE_H */
