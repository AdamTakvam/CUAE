// -*- C++ -*-

//==========================================================================
/**
 *  @file    DEV_Addr.h
 *
 *  DEV_Addr.h,v 4.17 2003/07/19 19:04:11 dhinton Exp
 *
 *  @author Gerhard Lenzer and Douglas C. Schmidt
 */
//==========================================================================


#ifndef ACE_DEV_ADDR_H
#define ACE_DEV_ADDR_H

#include /**/ "ace/pre.h"

#include "ace/Addr.h"

#if !defined (ACE_LACKS_PRAGMA_ONCE)
# pragma once
#endif /* ACE_LACKS_PRAGMA_ONCE */

#include "ace/os_include/os_dirent.h"

/**
 * @class ACE_DEV_Addr
 *
 * @brief Defines device address family address format.
 */
class ACE_Export ACE_DEV_Addr : public ACE_Addr
{
public:
  // = Initialization methods.
  /// Default constructor.
  ACE_DEV_Addr (void);

  /// Copy constructor.
  ACE_DEV_Addr (const ACE_DEV_Addr &sa);

  /// Acts like a copy constructor.
  int set (const ACE_DEV_Addr &sa);

  /// Create a ACE_DEV_Addr from a device name.
  ACE_EXPLICIT ACE_DEV_Addr (const ACE_TCHAR *devname);

  /// Create a ACE_Addr from a ACE_DEV pathname.
  void set (const ACE_TCHAR *devname);

  /// Assignment operator.
  ACE_DEV_Addr &operator= (const ACE_DEV_Addr &);

  /// Return a pointer to the address.
  virtual void *get_addr (void) const;

  /// Transform the current address into string format.
  virtual int addr_to_string (ACE_TCHAR *addr, size_t) const;

  /// Compare two addresses for equality.
  int operator == (const ACE_DEV_Addr &SAP) const;

  /// Compare two addresses for inequality.
  int operator != (const ACE_DEV_Addr &SAP) const;

  /// Return the path name used for the rendezvous point.
  const ACE_TCHAR *get_path_name (void) const;

  /// Dump the state of an object.
  void dump (void) const;

  /// Declare the dynamic allocation hooks.
  ACE_ALLOC_HOOK_DECLARE;

private:
  /// Name of the device.
  ACE_TCHAR devname_[MAXNAMLEN + 1];
};

#if defined (__ACE_INLINE__)
#include "ace/DEV_Addr.i"
#endif /* __ACE_INLINE__ */

#include /**/ "ace/post.h"

#endif /* ACE_DEV_ADDR_H */
