// -*- C++ -*-

//===========================================================================
/**
 * @file Copy_Disabled.h
 *
 * Copy_Disabled.h,v 4.4 2003/07/19 19:04:11 dhinton Exp
 *
 * @author Carlos O'Ryan <coryan@uci.edu>
 */
//===========================================================================

#ifndef ACE_COPY_DISABLED_H
#define ACE_COPY_DISABLED_H

#include /**/ "ace/pre.h"

#include "ace/ACE_export.h"

#if !defined (ACE_LACKS_PRAGMA_ONCE)
# pragma once
#endif /* ACE_LACKS_PRAGMA_ONCE */

/**
 * @class ACE_Copy_Disabled
 *
 * @brief Helper class to disable copy construction and assignment
 *
 * Classes used to control OS and other resources are not "canonical",
 * i.e. they have their copy constructor and assignment operators
 * disabled.
 * This is often done by making the copy constructor and assignment
 * operators private, effectively disallowing copying by clients of
 * the class (including derived classes).  If the copy constructor and
 * assingment operators are left unimplemented then the class itself
 * cannot make any copies of its instances, because it would result in
 * link errors.
 *
 * To use this class simply use private inheritance:
 *
 * class Foo : private ACE_Copy_Disabled
 * {
 * // code here
 * };
 *
 */
class ACE_Export ACE_Copy_Disabled
{
public:
  /// Default constructor
  ACE_Copy_Disabled (void);

private:
  ACE_Copy_Disabled (const ACE_Copy_Disabled &);
  ACE_Copy_Disabled &operator= (const ACE_Copy_Disabled &);
};

#include /**/ "ace/post.h"

#endif /* ACE_COPY_DISABLED_H */
