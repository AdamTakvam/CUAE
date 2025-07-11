/**
 * This is a configuration file to define all the macros that Doxygen
 * needs
 *
 * @file config-doxygen.h
 *
 * config-doxygen.h,v 1.16 2003/07/02 13:23:19 dhinton Exp
 *
 * @author Carlos O'Ryan <coryan@uci.edu>
 * @author Darrell Brunsch <brunsch@uci.edu>
 *
 */
#ifndef ACE_CONFIG_DOXYGEN_H
#define ACE_CONFIG_DOXYGEN_H

/// Make sure that we always turn inlining on.
#defind __ACE_INLINE__

/// Make the wchar_t interfaces available.
#define ACE_HAS_WCHAR

/// Make all the emulation versions of string operations visible
// #define ACE_LACKS_WCSTOK
#define ACE_LACKS_ITOW
#define ACE_LACKS_STRCASECMP
#define ACE_LACKS_STRCSPN
#define ACE_LACKS_STRCHR
#define ACE_LACKS_STRRCHR
#define ACE_LACKS_WCSCAT
#define ACE_LACKS_WCSCHR
#define ACE_LACKS_WCSCMP
#define ACE_LACKS_WCSCPY
#define ACE_LACKS_WCSICMP
#define ACE_LACKS_WCSLEN
#define ACE_LACKS_WCSNCAT
#define ACE_LACKS_WCSNCMP
#define ACE_LACKS_WCSNCPY
#define ACE_LACKS_WCSNICMP
#define ACE_LACKS_WCSPBRK
#define ACE_LACKS_WCSRCHR
#define ACE_LACKS_WCSCSPN
#define ACE_LACKS_WCSSPN
#define ACE_LACKS_WCSSTR

/// Support for threads enables several important classes
#define ACE_HAS_THREADS

/// Support for Win32 enables the WFMO_Reactor and several Async I/O
/// classes
#define ACE_WIN32

/// Enable support for POSIX Asynchronous I/O calls
#define ACE_HAS_AIO_CALLS

/// Enable support for TLI interfaces
#define ACE_HAS_TLI

/// Enable support for the SSL wrappers
#define ACE_HAS_SSL 1

/// Several GUI Reactors that are only enabled in some platforms.
#define ACE_HAS_XT
#define ACE_HAS_FL
#define ACE_HAS_QT
#define ACE_HAS_TK
#define ACE_HAS_GTK

/// Enable exceptions
#define ACE_HAS_EXCEPTIONS

/// Enable timeprobes
#define ACE_COMPILE_TIMEPROBES

/// Enable unicode to generate ACE_Registry_Name_Space
#define UNICODE

/// These defines make sure that Svc_Conf_y.cpp and Svc_Conf_l.cpp are correctly
/// parsed
#define __cplusplus
#define ACE_YY_USE_PROTOS

/// TAO features that should be documented too
#define TAO_HAS_RT_CORBA 1
#define TAO_HAS_MINIMUM_CORBA 0
#define TAO_HAS_AMI 1
#define TAO_HAS_INTERCEPTORS 1
#define TAO_HAS_SCIOP 1

/// Generate token library documentation
#define ACE_HAS_TOKENS_LIBRARY

/// Generate ACE ATM classes documentation
#define ACE_HAS_ATM

/// Generate ACE XTI ATM class documentation
#define ACE_HAS_XTI_ATM

/// Generate ACE_Dev_Poll_Reactor documentation
#define ACE_HAS_EVENT_POLL

/// Generate ACE_Event_Handler_T documentation
#define ACE_HAS_TEMPLATE_TYPEDEFS

/// Generate ACE_Log_Msg_NT_Event_Log documentation
#define ACE_HAS_LOG_MSG_NT_EVENT_LOG

/// Generate strptime documentation
#define ACE_HAS_STRPTIME

/// Doxygen is capable of parsing using
#define ACE_HAS_USING_KEYWORD

#endif /* ACE_CONFIG_DOXYGEN_H */
