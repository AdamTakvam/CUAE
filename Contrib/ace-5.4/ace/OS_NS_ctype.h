// -*- C++ -*-

//=============================================================================
/**
 *  @file   OS_NS_ctype.h
 *
 *  OS_NS_ctype.h,v 1.3 2003/11/01 23:42:24 dhinton Exp
 *
 *  @author Douglas C. Schmidt <schmidt@cs.wustl.edu>
 *  @author Jesper S. M|ller<stophph@diku.dk>
 *  @author and a cast of thousands...
 *
 *  Originally in OS.h.
 */
//=============================================================================

#ifndef ACE_OS_NS_CTYPE_H
# define ACE_OS_NS_CTYPE_H

# include /**/ "ace/pre.h"

# include "ace/config-all.h"

# if !defined (ACE_LACKS_PRAGMA_ONCE)
#  pragma once
# endif /* ACE_LACKS_PRAGMA_ONCE */

#include "ace/ACE_export.h"

#if defined (ACE_EXPORT_MACRO)
#  undef ACE_EXPORT_MACRO
#endif
#define ACE_EXPORT_MACRO ACE_Export

namespace ACE_OS {

  // these are non-standard names...

  /** @name Functions from <cctype>
   *
   *  Included are the functions defined in <cctype> and their <cwctype>
   *  equivalents.
   *
   *  Since they are often implemented as macros, we don't use the same name
   *  here.  Instead, we change by prepending "ace_" (with the exception of
   *  to_lower).
   *
   *  @todo To be complete, we should add: isalnum, isalpha, iscntrl
   *  isdigit, isgraph, islower, ispunct, isupper, isxdigit, and toupper.
   */
  //@{

  /// Returns true if the character is a printable character.
  ACE_NAMESPACE_INLINE_FUNCTION
  int ace_isprint (const ACE_TCHAR c);

  /// Returns true if the character is a space character.
  ACE_NAMESPACE_INLINE_FUNCTION
  int ace_isspace (const ACE_TCHAR c);

  /// Converts a character to lower case (char version).
  ACE_NAMESPACE_INLINE_FUNCTION
  int to_lower (int c);

#if defined (ACE_HAS_WCHAR) && !defined (ACE_LACKS_TOWLOWER)
  /// Converts a character to lower case (wchar_t version).
  ACE_NAMESPACE_INLINE_FUNCTION
  wint_t to_lower (wint_t c);
#endif /* ACE_HAS_WCHAR && !ACE_LACKS_TOWLOWER */

  //@}

} /* namespace ACE_OS */

# if defined (ACE_HAS_INLINED_OSCALLS)
#   if defined (ACE_INLINE)
#     undef ACE_INLINE
#   endif /* ACE_INLINE */
#   define ACE_INLINE inline
#   include "ace/OS_NS_ctype.inl"
# endif /* ACE_HAS_INLINED_OSCALLS */

# include /**/ "ace/post.h"
#endif /* ACE_OS_NS_CTYPE_H */
