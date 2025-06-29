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
*    %name:          dlgcenetalarmevents.h %
*    %version:       5 %
*    %instance:      hsw_3 %
*    %created_by:    klotzw %
*    %date_modified: Wed Oct 16 16:15:40 2002 %
*    ===================================================================
*/

#ifndef __DLGC_ENETALARM_EVENTS_H__
#define __DLGC_ENETALARM_EVENTS_H__

#include "dlgcevents.h"

#if 0

#include "dlgceventbasedef.h"

const     unsigned long   MAX_ENETALARM_DESCRIPTION = 256;
const     unsigned long   DLGC_EVT_LINK_UP = (DLGC_ENETALARMS_BASE_EVT_ID | 0x0001);
const     unsigned long   DLGC_EVT_LINK_DOWN = (DLGC_ENETALARMS_BASE_EVT_ID | 0x0002);
const     unsigned long   DLGC_EVT_PORT_FAILURE = (DLGC_ENETALARMS_BASE_EVT_ID | 0x0003); 
const     unsigned long   DLGC_EVT_INACTIVE_PEER = (DLGC_ENETALARMS_BASE_EVT_ID | 0x0004);
const     unsigned long   DLGC_EVT_ENET_UNKNOWN = (DLGC_ENETALARMS_BASE_EVT_ID | 0x0010);

//associated payload
typedef struct EnetMsgT 
{
    AUID   auid;
    char   szDescription[MAX_ENETALARM_DESCRIPTION];
    int    EthernetInterfaceNumber;
} EnetMsg, *pEnetMsg;

typedef struct InactivePeerMsgT 
{ 
    AUID auid;
    char szDescription[MAX_ENETALARM_DESCRIPTION];
    int  RefNum1[MAX_ENETALARM_DESCRIPTION];
    int  RefNum2[MAX_ENETALARM_DESCRIPTION];
} InactivePeerMsg, *pInactivePeerMsg;

#endif // #if 0

#endif // __DLGC_ENETALARM_EVENTS_H__
