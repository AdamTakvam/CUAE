// -*- C++ -*-

//==========================================================================
/**
 *  @file    SV_Message.h
 *
 *  SV_Message.h,v 4.10 2002/05/27 06:05:41 jwillemsen Exp
 *
 *  @author Doug Schmidt
 */
//==========================================================================


#ifndef ACE_SV_MESSAGE_H
#define ACE_SV_MESSAGE_H

#include "ace/pre.h"

#include "ace/ACE_export.h"

#if !defined (ACE_LACKS_PRAGMA_ONCE)
# pragma once
#endif /* ACE_LACKS_PRAGMA_ONCE */

#include "ace/OS.h"

/**
 * @class ACE_SV_Message
 *
 * @brief Defines the header file for the C++ wrapper for message queues.
 */
class ACE_Export ACE_SV_Message
{
public:
  /// Constructor
  ACE_SV_Message (long type = 0);

  /// Destructor
  ~ACE_SV_Message (void);

  /// Get the message type.
  long type (void) const;

  /// Set the message type.
  void type (long);

  /// Dump the state of an object.
  void dump (void) const;

  /// Declare the dynamic allocation hooks.
  ACE_ALLOC_HOOK_DECLARE;

protected:
  /// Type of the message.
  long type_;
};

#if defined (__ACE_INLINE__)
#include "ace/SV_Message.i"
#endif /* __ACE_INLINE__ */

#include "ace/post.h"

#endif /* ACE_SV_MESSAGE_H */
