// -*- C++ -*-
//=============================================================================
/**
 *  @file   Codeset_Registry.h
 *
 *  Codeset_Registry.h,v 1.3 2003/07/19 19:04:11 dhinton Exp
 *
 * ACE wrapper around access functions for the OSF's DCE codeset registry
 * access functions
 *
 * For environments that intrinsicly support the DCE defined access functions,
 * the methods in this class are simply wrappers. On other platforms, emulation
 * is provided. The motivation for this class is to support interoperability
 * via translators and the CDR streams, primarily in TAO, but this capability
 * is not restricted to CORBA.
 *
 * The emulated functionalty supports Open Group RFC #40, currently RFC 40.2,
 * www.opengroup.org/tech/rfc/rfc40.2.html
 *
 *  @author Phil Mesnier <mesnier_p@ociweb.com>
 */
//=============================================================================

#ifndef ACE_CODESET_REGISTRY_H
#define ACE_CODESET_REGISTRY_H

#include /**/ "ace/pre.h"
#include "ace/SString.h"
#include "ace/CDR_Base.h"

#if !defined (ACE_LACKS_PRAGMA_ONCE)
# pragma once
#endif /* ACE_LACKS_PRAGMA_ONCE */

#if defined (ACE_HAS_DCE_CODESET_REGISTRY)
#include /**/ <dce/rpc.h>
#endif /* ACE_HAS_DCE_CODESET_REGISTRY */

class ACE_Export ACE_Codeset_Registry
{
public:

  // based on a locale string, find the registry value and optional codeset
  // collection. This wraps the dce_cs_loc_to_rgy function, or emulates it.
  static int locale_to_registry (const ACE_CString &locale,
                                 ACE_CDR::ULong &codeset_id,
                                 ACE_CDR::UShort * = 0,
                                 ACE_CDR::UShort ** = 0);

  // based on a registry value, find the locale string and optional codeset
  // collection.  This wraps the dce_cs_rgy_to_loc function, or emulates it.
  static int registry_to_locale (ACE_CDR::ULong codeset_id,
                                 ACE_CString &locale,
                                 ACE_CDR::UShort * = 0,
                                 ACE_CDR::UShort ** = 0);

  // tell if two codesets are compatible. This wraps the
  //rpc_cs_char_set_compat_check function.
  static int is_compatible (ACE_CDR::ULong codeset_id,
                            ACE_CDR::ULong other);

  // return the max number of bytes required to represent a single character.
  // This wraps the rpc_rgy_get_max_bytes function.
  static ACE_CDR::Short get_max_bytes (ACE_CDR::ULong codeset_id);

  enum {max_charsets_ = 5};
protected:
  typedef struct {
    const char *     desc_;
    const char *     loc_name_;
    ACE_CDR::ULong   codeset_id_;
    ACE_CDR::UShort  num_sets_;
    ACE_CDR::UShort  char_sets_[max_charsets_];
    ACE_CDR::UShort  max_bytes_;
  } registry_entry;

private:
  static size_t num_registry_entries_;
  static registry_entry registry_db_[];

  static int locale_to_registry_i (const ACE_CString &locale,
                                   ACE_CDR::ULong &codeset_id,
                                   ACE_CDR::UShort * = 0,
                                   ACE_CDR::UShort ** = 0);
  static int registry_to_locale_i (ACE_CDR::ULong codeset_id,
                                   ACE_CString &locale,
                                   ACE_CDR::UShort * = 0,
                                   ACE_CDR::UShort ** = 0);
  static int is_compatible_i (ACE_CDR::ULong codeset_id,
                              ACE_CDR::ULong other);
  static ACE_CDR::Short get_max_bytes_i (ACE_CDR::ULong codeset_id);
};

#if defined (__ACE_INLINE__)
#include "ace/Codeset_Registry.inl"
#endif /* __ACE_INLINE__ */

#include /**/ "ace/post.h"
#endif /* ACE_CODESET_REGISTRY_H */
