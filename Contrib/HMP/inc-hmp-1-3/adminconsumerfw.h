/**************************************************************
    Copyright (C) 2000-2004.  Intel Corporation.

    All Rights Reserved.  All names, products,
    and services mentioned herein are the trademarks
    or registered trademarks of their respective organizations
    and are the sole property of their respective owners.
 **************************************************************/

/*
*    AUTO-VERSIONING HEADER  DO NOT HAND MODIFY
*    ===================================================================
*    %name:          adminconsumerfw.h %
*    %version:       11 %
*    %instance:      hsw_1 %
*    %created_by:    ritscheg %
*    %date_modified: Tue Nov 09 13:19:22 2004 %
*    ===================================================================
*/

#ifndef _ADMIN_CONSUMER_FW_H
#define _ADMIN_CONSUMER_FW_H

#ifndef lint
static char    *_hsw_1_adminconsumerfw_h = "@(#) %filespec: adminconsumerfw.h-11 %  (%full_filespec: adminconsumerfw.h-11:incl:hsw#1 %)";
#endif

// The following ifdef block is the standard way of creating macros which make exporting
// from a DLL simpler. All files within this DLL are compiled with the ADMINCONSUMERFW_EXPORTS
// symbol defined on the command line. this symbol should not be defined on any project
// that uses this DLL. This way any other project whose source files include this file see
// ADMINCONSUMERFW_API functions as being imported from a DLL, wheras this DLL sees symbols
// defined with this macro as being exported.

#ifdef ADMINCONSUMERFW_EXPORTS
#   if (!defined(DLG_STATICALLY_LINKED) && defined(DLG_WIN32_OS))
#      define ADMINCONSUMERFW_API __declspec(dllexport)
#   else
#      define ADMINCONSUMERFW_API
#   endif
#else
#   if (!defined(DLG_STATICALLY_LINKED) && defined(DLG_WIN32_OS))
#      define ADMINCONSUMERFW_API __declspec(dllimport)
#   else
#      define ADMINCONSUMERFW_API
#   endif
#endif

#endif
