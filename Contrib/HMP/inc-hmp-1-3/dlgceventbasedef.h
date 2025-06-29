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
*    %name:          dlgceventbasedef.h %
*    %version:       16 %
*    %instance:      hsw_2 %
*    %created_by:    ritscheg %
*    %date_modified: Tue Nov 09 13:41:57 2004 %
*    ===================================================================
*/

#ifndef __DLGC_BASE_EVENT_DEF_H__
#define __DLGC_BASE_EVENT_DEF_H__

#ifndef lint
static char    *_hsw_2_dlgceventbasedef_h = "@(#) %filespec: dlgceventbasedef.h-16 %  (%full_filespec: dlgceventbasedef.h-16:incl:hsw#2 %)";
#endif


#include    "dlgctypes.h"

#define MAX_EVENT_DESCRIPTION 256

/*
  Thirdparty and Dialogic developers, please update this section when adding a new channel.

*/

#define FAULT_CHANNEL                       "FAULT"                     // Hardware Faults (CP/SP)
#define CLOCK_EVENT_CHANNEL                 "CLOCKING"                  // TDM Bus Faults
#define ADMIN_CHANNEL                       "ADMIN"                     // Generic Admin (Start/Stop)
#define DEFAULT_CHANNEL	                    "DEFAULT"                   // Not Really Used
#define STOP_DEVICE_REGISTRATION_CHANNEL    "STOP_DEVICE_REGISTRATION"
#define NETWORK_ALARM_CHANNEL               "NETWORKALARMS"             // All T1/E1 Network Alarms
#define ENET_ALARM_CHANNEL                  "ENETALARMS"                // Ethernet Alarms
#define DIAGGENERIC_CHANNEL                 "DIAG_GENERIC"              // Diagnostics
#define BRIDGING_CHANNEL                    "BRIDGING"                  // Bridging Events

/*
    BASE EVENTS Class Section
    For all thirdparty customers and Dialogic developers, please update this section properly
    when adding a new class of events. Typically but not always; a class of events
    is represented by a one to one relation to a channel.
*/

#define  DLGC_INTERNAL_EVT_ID                       0x00010000
#define  DLGC_CLOCKAPI_BASE_EVT_ID                  0x00020000
#define  DLGC_FAULT_BASE_EVT_ID                     0x00030000
#define  DLGC_ADMIN_BASE_EVT_ID                     0x00040000
#define  DLGC_STOP_BASE_EVT_ID                      0x00050000
#define  DLGC_NETWORKALARMS_BASE_EVT_ID             0x00060000
#define  DLGC_ENETALARMS_BASE_EVT_ID                0x00070000
#define  DLGC_DIAGGENERIC_BASE_EVT_ID               0x00080000
#define  DLGC_BRIDGING_BASE_EVT_ID                  0x00090000


#endif //__DLGC_BASE_EVENT_DEF_H__
