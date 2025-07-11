// -*- C++ -*-

//=============================================================================
/**
 *  @file    Exception.h
 *
 *  Exception.h,v 1.5 2002/07/02 03:03:35 kitty Exp
 *
 *  @author Nanbor Wang <nanbor@cs.wustl.edu>
 */
//=============================================================================

#ifndef _ACEXML_EXCEPTION_H_
#define _ACEXML_EXCEPTION_H_

#include "ace/pre.h"
#include "ACEXML/common/ACEXML_Export.h"

#if !defined (ACE_LACKS_PRAGMA_ONCE)
#pragma once
#endif /* ACE_LACKS_PRAGMA_ONCE */

#include "ACEXML/common/XML_Types.h"

/**
 * @class ACEXML_Exception Exception.h "ACEXML/common/Exception.h"
 *
 * @brief ACEXML_Exception
 *
 * ACEXML_Exception is the base class for all ACEXML related exceptions.
 * Since ACEXML currently does not support native exceptions, all
 * exceptions should be thrown thru ACEXML_Env.
 *
 * @sa ACEXML_Env
 */
class ACEXML_Export ACEXML_Exception
{
public:
  /// Default contructor.
  ACEXML_Exception (void);

  /// Copy constructor.
  ACEXML_Exception (const ACEXML_Exception &ex);

  /// Destructor.
  virtual ~ACEXML_Exception (void);

  /// Accessor for the exception name.
  static const ACEXML_Char *name (void);

  /// Return the exception type.  (for safe downcast.)
  virtual const ACEXML_Char *id (void);

  /// Dynamically create a copy of this exception.
  virtual ACEXML_Exception *duplicate (void) = 0;

  /// Check whether this is an exception of type specified by <name>.
  virtual int is_a (const ACEXML_Char *name) = 0;

  /// Print out exception using ACE_DEBUG.
  virtual void print (void) = 0;

protected:
  /// All exceptions have names.  This name is used to identify the
  /// type of an exception.
  static const ACEXML_Char *exception_name_;

  /// A null string that we return when there is no exception.
  static const ACEXML_Char *null_;
};

#if defined (__ACEXML_INLINE__)
# include "ACEXML/common/Exception.i"
#endif /* __ACEXML_INLINE__ */

#include "ace/post.h"

#endif /* _ACEXML_EXCEPTION_H_ */
