/* -*- C++ -*- */
//=============================================================================
/**
 *  @file   config-win32-msvc-7.h
 *
 *  config-win32-msvc-7.h,v 4.7 2003/11/19 15:55:06 bala Exp
 *
 *  @brief  Microsoft Visual C++ 7.0 configuration file.
 *
 *  This file is the ACE configuration file for Microsoft Visual C++ version 7.
 *
 *  @note Do not include this file directly, include config-win32.h instead.
 *
 *  @author Darrell Brunsch <brunsch@cs.wustl.edu>
 */
//=============================================================================

#ifndef ACE_CONFIG_WIN32_MSVC_7_H
#define ACE_CONFIG_WIN32_MSVC_7_H
#include /**/ "ace/pre.h"

#ifndef ACE_CONFIG_WIN32_H
#error Use config-win32.h in config.h instead of this header
#endif /* ACE_CONFIG_WIN32_H */

// Visual C++ 7.0 (.NET) deprecated the old iostreams
#if !defined (ACE_HAS_STANDARD_CPP_LIBRARY)
#define ACE_HAS_STANDARD_CPP_LIBRARY 1
#endif

#if !defined (ACE_USES_STD_NAMESPACE_FOR_STDCPP_LIB)
#define ACE_USES_STD_NAMESPACE_FOR_STDCPP_LIB 1
#endif

// Win64 SDK compiler claims std::auto_ptr<>::reset not available.
#if defined (_WIN64) || defined (WIN64)
#define ACE_AUTO_PTR_LACKS_RESET
#endif

#if !defined (ACE_HAS_BROKEN_NESTED_TEMPLATES)
#define ACE_HAS_BROKEN_NESTED_TEMPLATES
#endif

// By default, we disable the C++ casting because
// it requires the RTTI support to be turned on which
// is not something we usually do.
#if !defined (ACE_HAS_ANSI_CASTS)
#define ACE_HAS_ANSI_CASTS 0
#endif

#define ACE_HAS_EXPLICIT_KEYWORD
#define ACE_HAS_MUTABLE_KEYWORD
#define ACE_HAS_TYPENAME_KEYWORD
#define ACE_HAS_USING_KEYWORD

#define ACE_HAS_ITOA

#define ACE_HAS_BROKEN_IMPLICIT_CONST_CAST
#define ACE_HAS_WORKING_EXPLICIT_TEMPLATE_DESTRUCTOR

#define ACE_ITOA_EQUIVALENT ::_itoa
#define ACE_STRCASECMP_EQUIVALENT ::_stricmp
#define ACE_STRNCASECMP_EQUIVALENT ::_strnicmp
#define ACE_WCSDUP_EQUIVALENT ::_wcsdup

#if !defined (ACE_HAS_WINCE)
#define ACE_HAS_EXCEPTIONS
#endif /* ACE_HAS_WINCE */

#if defined (ACE_HAS_ANSI_CASTS) && (ACE_HAS_ANSI_CASTS == 0)
#undef  ACE_HAS_ANSI_CASTS
#endif /* ACE_HAS_ANSI_CASTS && ACE_HAS_ANSI_CASTS == 0 */

#define ACE_HAS_STRERROR
#define ACE_HAS_STRPTIME
#define ACE_LACKS_NATIVE_STRPTIME

#define ACE_HAS_SIG_ATOMIC_T
#define ACE_HAS_STD_TEMPLATE_SPECIALIZATION
#define ACE_HAS_STD_TEMPLATE_CLASS_MEMBER_SPECIALIZATION
#define ACE_LACKS_STATIC_DATA_MEMBER_TEMPLATES
#define ACE_LACKS_MODE_MASKS
#define ACE_LACKS_STRRECVFD
#define ACE_HAS_CPLUSPLUS_HEADERS

#define ACE_TEMPLATES_REQUIRE_SOURCE
#define ACE_HAS_TEMPLATE_SPECIALIZATION

#define ACE_INT64_FORMAT_SPECIFIER ACE_LIB_TEXT ("%I64d")
#define ACE_UINT64_FORMAT_SPECIFIER ACE_LIB_TEXT ("%I64u")

// Platform provides ACE_TLI function prototypes.
// For Win32, this is not really true, but saves a lot of hassle!
#define ACE_HAS_TLI_PROTOTYPES

// Platform support linebuffered streaming is broken
#define ACE_LACKS_LINEBUFFERED_STREAMBUF

#if defined (ACE_HAS_STANDARD_CPP_LIBRARY) && (ACE_HAS_STANDARD_CPP_LIBRARY != 0)

// Platform has its Standard C++ library in the namespace std
# if !defined (ACE_USES_STD_NAMESPACE_FOR_STDCPP_LIB)
# define ACE_USES_STD_NAMESPACE_FOR_STDCPP_LIB       1
# endif /* ACE_USES_STD_NAMESPACE_FOR_STDCPP_LIB */

// ace/iostream.h does not work with the standard cpp library (yet).
# if !defined (ACE_USES_OLD_IOSTREAMS)
# define ACE_LACKS_ACE_IOSTREAM
# endif /* ! ACE_USES_OLD_IOSTREAMS */

#else

// iostream header lacks ipfx (), isfx (), etc., declarations
#  define ACE_LACKS_IOSTREAM_FX

#endif

// There are too many instances of this warning to fix it right now.
// Maybe in the future.
// 'this' : used in base member initializer list
#pragma warning(disable:4355)
// 'class1' : inherits 'class2::member' via dominance
#pragma warning(disable:4250)
// C++ Exception Specification ignored
#pragma warning(disable:4290)

# pragma warning(disable:4231)
// Disable warning of using Microsoft Extension.

#define ACE_ENDTHREADEX(STATUS) ::_endthreadex ((DWORD) STATUS)

// With the MSVC7 compiler there is a new 'feature' when a base-class is a
// specialization of a class template. The class template must be explicit
// instantiated and exported.
#define ACE_HAS_EXPLICIT_TEMPLATE_INSTANTIATION_EXPORT

// At least for ACE_UNIMPLEMENTED_FUNC in class templates, this is needed to
// explicitly instantiate a template that has ACE_UNIMPLEMENTED_FUNC.
# define ACE_NEEDS_FUNC_DEFINITIONS

#include /**/ "ace/post.h"
#endif /* ACE_CONFIG_WIN32_MSVC_7_H */
