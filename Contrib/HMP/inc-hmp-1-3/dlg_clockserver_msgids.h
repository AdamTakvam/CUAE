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

#ifndef _DLG_CLOCKSERVER_MSGID_H
#define _DLG_CLOCKSERVER_MSGID_H

#include "dlgfacil.h"


/*  
    Clock Server Message  Definition 
 */

const DlgSysResultType DLG_CLOCKSERVER_NULL_AUID = 
(DLG_ERROR_CODE  |DLG_CLOCKSERVER_FACILITY|0x000000000);

const DlgSysResultType DLG_CLOCKSERVER_FAIL_GETDEVICECAPABILITIES = 
(DLG_ERROR_CODE  | DLG_CLOCKSERVER_FACILITY | 0x00000001);

const DlgSysResultType DLG_CLOCKSERVER_FAIL_GETDEVICEBUSCONFIG = 
(DLG_ERROR_CODE  | DLG_CLOCKSERVER_FACILITY | 0x00000002);

const DlgSysResultType DLG_CLOCKSERVER_FAIL_SETDEVICEBUSCONFIG = 
(DLG_ERROR_CODE  | DLG_CLOCKSERVER_FACILITY | 0x00000003);

const DlgSysResultType DLG_CLOCKSERVER_FAIL_CREATE_AGENT= 
(DLG_ERROR_CODE  | DLG_CLOCKSERVER_FACILITY | 0x00000004);

const DlgSysResultType DLG_CLOCKSERVER_FAIL_INIT = 
(DLG_ERROR_CODE  | DLG_CLOCKSERVER_FACILITY | 0x00000005);

const DlgSysResultType DLG_CLOCKSERVERCLIENT_FAIL_INIT = 
(DLG_ERROR_CODE  | DLG_CLOCKSERVER_FACILITY | 0x00000006);

const DlgSysResultType DLG_CLOCKSERVERCLIENT_UNKNOWN_EXCEPTION = 
(DLG_ERROR_CODE  | DLG_CLOCKSERVER_FACILITY | 0x00000007);

/*  FOR INTERNAL USE ONLY.
    Clock Server Message  Definition 
    For use by dialogic not for client use.
 */

#endif
