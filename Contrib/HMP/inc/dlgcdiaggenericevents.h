/**************************************************************
    Copyright (C) 2000-2002.  Intel Corporation.
 
    All Rights Reserved.  All names, products,
    and services mentioned herein are the trademarks
    or registered trademarks of their respective organizations
    and are the sole property of their respective owners.
 **************************************************************/
 
/*
*    AUTO-VERSIONING HEADER  DO NOT HAND MODIFY
*    ===================================================================
*    %name:          dlgcdiaggenericevents.h %
*    %version:       6 %
*    %instance:      hsw_1 %
*    %created_by:    klotzw %
*    %date_modified: Wed Oct 16 16:15:38 2002 %
*    ===================================================================
*/

#ifndef _hsw_1_dlgcdiaggenericevents_h_H
#define _hsw_1_dlgcdiaggenericevents_h_H

#include "dlgcevents.h"

#if 0

#ifndef lint
static char    *_hsw_1_dlgcdiaggenericevents_h = "@(#) %filespec: dlgcdiaggenericevents.h-6 %  (%full_filespec: dlgcdiaggenericevents.h-6:incl:hsw#1 %)";
#endif

#include    "dlgceventbasedef.h"

const     unsigned long    MAX_DIAGGENERIC_DESCRIPTION    = 256;

const     unsigned long   DLGC_EVT_GENERIC_ERROR					=(DLGC_DIAGGENERIC_BASE_EVT_ID | 0x0001);
const     unsigned long   DLGC_EVT_PING_SUCCESS						=(DLGC_DIAGGENERIC_BASE_EVT_ID | 0x0002);
const     unsigned long   DLGC_EVT_PING_FAILURE						=(DLGC_DIAGGENERIC_BASE_EVT_ID | 0x0003);
const     unsigned long   DLGC_EVT_DIAGNOSTIC_SUCCESS				=(DLGC_DIAGGENERIC_BASE_EVT_ID | 0x0004);
const     unsigned long   DLGC_EVT_DIAGNOSTIC_FAILURE				=(DLGC_DIAGGENERIC_BASE_EVT_ID | 0x0005);
const     unsigned long   DLGC_EVT_NETWORK_QUALITY_LOSS				=(DLGC_DIAGGENERIC_BASE_EVT_ID | 0x0006);
const     unsigned long   DLGC_EVT_MEMORY_UTILIZATION				=(DLGC_DIAGGENERIC_BASE_EVT_ID | 0x0007);
const     unsigned long   DLGC_EVT_UNKNOWN_ALARM_DIAG                    =(DLGC_DIAGGENERIC_BASE_EVT_ID | 0x0008);
//associated payload
typedef struct DiagGenericMsgT {
	AUID	auid;
	int		entityType;
	int		entityInstance;
	int		eventName;
	int		timeStamp;
	int		streamId;
	char    szDescription[MAX_DIAGGENERIC_DESCRIPTION];
} DiagGenericMsg, *DiagGenericMsgPtr;


#endif // #if 0

#endif  // _hsw_1_dlgcdiaggenericevents_h_H



