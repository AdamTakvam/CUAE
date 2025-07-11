// -*- C++ -*-

//=============================================================================
/**
 *  @file    UNIX_Addr.h
 *
 *  UNIX_Addr.h,v 4.15 2002/04/05 12:00:45 dhinton Exp
 *
 *  @author Doug Schmidt
 */
//=============================================================================


#ifndef ACE_UNIX_ADDR_H
#define ACE_UNIX_ADDR_H
#include "ace/pre.h"

#include "ace/config-all.h"

#if !defined (ACE_LACKS_PRAGMA_ONCE)
# pragma once
#endif /* ACE_LACKS_PRAGMA_ONCE */

#if !defined (ACE_LACKS_UNIX_DOMAIN_SOCKETS)

#include "ace/Addr.h"
#include "ace/Log_Msg.h"
#include "ace/ACE.h"

/**
 * @class ACE_UNIX_Addr
 *
 * @brief Defines the ``UNIX domain address family'' address format.
 */
class ACE_Export ACE_UNIX_Addr : public ACE_Addr
{
public:
  // = Initialization methods.
  /// Default constructor.
  ACE_UNIX_Addr (void);

  /// Copy constructor.
  ACE_UNIX_Addr (const ACE_UNIX_Addr &sa);

  /// Creates an ACE_UNIX_Addr from a string.
  ACE_UNIX_Addr (const char rendezvous_point[]);

  /// Creates an ACE_INET_Addr from a sockaddr_un structure.
  ACE_UNIX_Addr (const sockaddr_un *, int len);

  /// Creates an ACE_UNIX_Addr from another <ACE_UNIX_Addr>.
  int set (const ACE_UNIX_Addr &sa);

  /// Creates an ACE_UNIX_Addr from a string.
  int set (const char rendezvous_point[]);

  /// Creates an ACE_UNIX_Addr from a sockaddr_un structure.
  int set (const sockaddr_un *, int len);

  /// Return a pointer to the underlying network address.
  virtual void *get_addr (void) const;

  /// Set a pointer to the underlying network address.
  virtual void set_addr (void *addr, int len);

  /// Transform the current address into string format.
  virtual int addr_to_string (char addr[], size_t) const;

  /// Transform the string into the current addressing format.
  virtual int string_to_addr (const char addr[]);

  /// Compare two addresses for equality.
  int operator == (const ACE_UNIX_Addr &SAP) const;

  /// Compare two addresses for inequality.
  int  operator != (const ACE_UNIX_Addr &SAP) const;

  /// Return the path name of the underlying rendezvous point.
  const char *get_path_name (void) const;

  /// Computes and returns hash value.
  virtual u_long hash (void) const;

  /// Dump the state of an object.
  void dump (void) const;

  /// Declare the dynamic allocation hooks.
  ACE_ALLOC_HOOK_DECLARE;

private:
  /// Underlying socket address.
  sockaddr_un unix_addr_;
};

#if defined (__ACE_INLINE__)
#include "ace/UNIX_Addr.i"
#endif /* __ACE_INLINE__ */

#endif /* ACE_LACKS_UNIX_DOMAIN_SOCKETS */
#include "ace/post.h"
#endif /* ACE_UNIX_ADDR_H */
