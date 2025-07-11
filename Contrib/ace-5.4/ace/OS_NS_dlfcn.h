// -*- C++ -*-

//=============================================================================
/**
 *  @file   OS_NS_dlfcn.h
 *
 *  OS_NS_dlfcn.h,v 1.3 2003/11/01 23:42:24 dhinton Exp
 *
 *  @author Douglas C. Schmidt <schmidt@cs.wustl.edu>
 *  @author Jesper S. M|ller<stophph@diku.dk>
 *  @author and a cast of thousands...
 *
 *  Originally in OS.h.
 */
//=============================================================================

#ifndef ACE_OS_NS_DLFCN_H
# define ACE_OS_NS_DLFCN_H

# include /**/ "ace/pre.h"

# include "ace/config-all.h"

# if !defined (ACE_LACKS_PRAGMA_ONCE)
#  pragma once
# endif /* ACE_LACKS_PRAGMA_ONCE */

#include "ace/os_include/os_dlfcn.h"
#include "ace/ACE_export.h"

#if defined (ACE_EXPORT_MACRO)
#  undef ACE_EXPORT_MACRO
#endif
#define ACE_EXPORT_MACRO ACE_Export

namespace ACE_OS {

  //@{ @name A set of wrappers for explicit dynamic linking.
  ACE_NAMESPACE_INLINE_FUNCTION
  int dlclose (ACE_SHLIB_HANDLE handle);

  ACE_NAMESPACE_INLINE_FUNCTION
  ACE_TCHAR *dlerror (void);

  ACE_NAMESPACE_INLINE_FUNCTION
  ACE_SHLIB_HANDLE dlopen (const ACE_TCHAR *filename,
                           int mode = ACE_DEFAULT_SHLIB_MODE);

  ACE_NAMESPACE_INLINE_FUNCTION
  void *dlsym (ACE_SHLIB_HANDLE handle,
               const ACE_TCHAR *symbol);
  //@}

} /* namespace ACE_OS */

# if defined (ACE_HAS_INLINED_OSCALLS)
#   if defined (ACE_INLINE)
#     undef ACE_INLINE
#   endif /* ACE_INLINE */
#   define ACE_INLINE inline
#   include "ace/OS_NS_dlfcn.inl"
# endif /* ACE_HAS_INLINED_OSCALLS */

# include /**/ "ace/post.h"
#endif /* ACE_OS_NS_DLFCN_H */
