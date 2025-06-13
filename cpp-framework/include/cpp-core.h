#ifndef CPP_CORE_H
#define CPP_CORE_H
#include "BuildConst.h"

#define CORE_DP "CORE "

/*
 * Standard DLL export.  On Windows define the
 * standard declspec import/export statement 
 * and do nothing on other platforms.
 */
#ifdef WIN32
#   ifdef CPPCORE_EXPORTS
#       define CPPCORE_API __declspec(dllexport)
#   else
#       define CPPCORE_API __declspec(dllimport)
#   endif
#else
#   define CPPCORE_API
#endif // WIN32

#endif // CPP_CORE_H


