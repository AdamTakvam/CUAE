/*COPYRIGHT*/
//++
//**********************************************************************
// Copyright 2004 (C) Intel Corporation. All rights reserved.
// To the extent possible unpublished rights reserved under the copyright
// laws of the United States.
// This document contains proprietary, confidential and unpublished
// information of Intel Corporation.
// No use or disclosure of any portion of this material may be made without
// the prior express written consent of Intel Corporation.
//
//     RESTRICTED RIGHTS LEGEND
// Use, duplication, or disclosure by the Government is subject to
// restrictions as set forth in (c)(1)(ii) of The Rights in Technical
// Data and Computer Software clause at DFARS 252.227-7013 or subparagraphs
// (c)(1) and (2) of the Commercial Computer Software -- Restricted
// Rights at 48 CFR 52.227-19, as applicable.
//
// The software contains trade secrets which are subject to exemption
// of 5 USC Section 552(b)(4)
//
// Intel Corporation
// 1515 Route Ten
// Parsippany, NJ 07054-4596 USA
//
//++
//
// $Author$
// $Revision$
// $Date$
// $Id$
// $Source$
//
// History:
// $Log$
//
//++
//*********************************************************************

#ifndef LICENSE_MANAGER_OEM_H
#define LICENSE_MANAGER_OEM_H

/*
** Error codes that can be returned by the OEM "get id" function
*/

typedef enum
{
	LIC_OEM_SUCCESS=0,	// Note that we start at 0 so that we can use the enum's as an array index
   LIC_OEM_NOT_INSTALLED,
   LIC_OEM_NOT_RUNNING
} LICoemStatus;

static const char *LICoemErrorStrings[] =
{
   "Success",                                      // LIC_OEM_SUCCESS
   "The OEM application is not installed",         // LIC_OEM_NOT_INSTALLED
   "The OEM application is not running"            // LIC_OEM_NOT_RUNNING
};

#define LIC_OEM_ERROR_STRINGS_SIZE (sizeof(LICoemErrorStrings)/sizeof(LICoemErrorStrings[0]))

/*
** Size of the buffer to allocate when retrieving the OEM ID
*/

#ifndef LIC_MAX_LOCKCODE_LENGTH
#define LIC_MAX_LOCKCODE_LENGTH                2049 
#endif

#define LIC_VENDOR_LICENSING_REGISTRY_KEY   "SOFTWARE\\Dialogic\\Installed Boards\\DM3\\HMP_Software #0 in slot 0/65535"
#define LIC_VENDOR_LICENSING_LIB_KEY        "GetVendorIdLibraryPath"
#define LIC_VENDOR_LICENSING_FUN_KEY        "GetVendorIdFunction"

/*
** Signature of the OEM "get id" function
*/

typedef LICoemStatus (*LIC_GETOEMID)(unsigned long *pOEMid);



#endif  // LICENSE_MANAGER_OEM_H
