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
*    %name:          %
*    %version:       %
*    %instance:      %
*    %created_by:    %
*    %date_modified: %
*    ===================================================================
*/

#ifndef _DLG_SYSCONTROLLER_MSGID_H
#define _DLG_SYSCONTROLLER_MSGID_H

#include "dlgfacil.h"


/*  
    System Controller  Framework  Message  Definition 
    For use by clients.
 */



const DlgSysResultType DLG_SYSCTL_INIT =
( DLG_INFO_CODE | DLG_SYSTEM_CONTROLLER_FACILITY | 0x00000000);

const DlgSysResultType DLG_SYSCTL_SUPPLIER_ERROR =
( DLG_ERROR_CODE | DLG_SYSTEM_CONTROLLER_FACILITY | 0x00000001);

const DlgSysResultType DLG_SYSCTL_CONSUMER_ERROR =
( DLG_ERROR_CODE | DLG_SYSTEM_CONTROLLER_FACILITY | 0x00000002);

const DlgSysResultType DLG_SYSCTL_RESOLVE_CLOCKDAEMON =
( DLG_ERROR_CODE | DLG_SYSTEM_CONTROLLER_FACILITY | 0x00000003);

const DlgSysResultType DLG_SYSCTL_RESOLVE_DBSYSMGR =
( DLG_ERROR_CODE | DLG_SYSTEM_CONTROLLER_FACILITY | 0x00000004);

const DlgSysResultType DLG_SYSCTL_AUTO_NONE_ON =
( DLG_INFO_CODE | DLG_SYSTEM_CONTROLLER_FACILITY | 0x00000005);

const DlgSysResultType DLG_SYSCTL_AUTO_DETECT_ON =
( DLG_INFO_CODE | DLG_SYSTEM_CONTROLLER_FACILITY | 0x00000006);

const DlgSysResultType DLG_SYSCTL_AUTO_START_ON =
( DLG_INFO_CODE | DLG_SYSTEM_CONTROLLER_FACILITY | 0x00000007);

const DlgSysResultType DLG_SYSCTL_AUTO_INIT_ON =
( DLG_INFO_CODE | DLG_SYSTEM_CONTROLLER_FACILITY | 0x00000008);

const DlgSysResultType DLG_SYSCTL_INVALID_AUTO_CONTROL =
( DLG_INTERNAL_MASK | DLG_INFO_CODE | DLG_SYSTEM_CONTROLLER_FACILITY | 0x00000009);

const DlgSysResultType DLG_SYSCTL_INVALID_AUTO_CONTROL_DEFAULT =
( DLG_INFO_CODE | DLG_SYSTEM_CONTROLLER_FACILITY | 0x0000000A);

const DlgSysResultType DLG_SYSCTL_BOARD_NOTFOUND =
( DLG_ERROR_CODE | DLG_SYSTEM_CONTROLLER_FACILITY | 0x0000000B);

const DlgSysResultType DLG_SYSCTL_EXIT =
( DLG_INFO_CODE | DLG_SYSTEM_CONTROLLER_FACILITY |  0x0000000C);

const DlgSysResultType DLG_SYSCTL_PERFORMING_POWERUP =
( DLG_INFO_CODE | DLG_SYSTEM_CONTROLLER_FACILITY |  0x0000000D);

const DlgSysResultType DLG_SYSCTL_POWER_ERROR =
( DLG_INTERNAL_MASK | DLG_ERROR_CODE | DLG_SYSTEM_CONTROLLER_FACILITY |  0x0000000E);


const DlgSysResultType DLG_SYSCTL_PARTIAL_ERROR =
( DLG_INTERNAL_MASK | DLG_ERROR_CODE | DLG_SYSTEM_CONTROLLER_FACILITY |  0x0000000F);



#endif
