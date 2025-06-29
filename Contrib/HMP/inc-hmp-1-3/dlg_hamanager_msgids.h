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

#ifndef _DLG_HAMANAGER_MSGID_H
#define _DLG_HAMANAGER_MSGID_H

#include "dlgfacil.h"


/*  
    HA Manager Framework  Message  Definition 
    For use by clients.
 */

const DlgSysResultType DLG_HAMANAGER_OK        =
    ( DLG_INFO_CODE  | DLG_HAMANAGER_FACILITY  | 0x00000000 );

const DlgSysResultType  DLG_HAMANAGER_NOTRDY   =
    ( DLG_ERROR_CODE  | DLG_HAMANAGER_FACILITY | 0x00000001 );

const DlgSysResultType  DLG_HAMANAGER_ORBINIT  =
    ( DLG_INFO_CODE  | DLG_HAMANAGER_FACILITY  | 0x00000002 );

const DlgSysResultType DLG_HAMANAGER_NAMINGDIR =
    ( DLG_ERROR_CODE  | DLG_HAMANAGER_FACILITY | 0x00000003 );

const DlgSysResultType  DLG_HAMANAGER_ORBINIT_ERR  =
    ( DLG_INFO_CODE  | DLG_HAMANAGER_FACILITY  | 0x00000004 );

const DlgSysResultType DLG_HAMANAGER_RUNNING   =
    ( DLG_INFO_CODE  | DLG_HAMANAGER_FACILITY  | 0x00000005 );

const DlgSysResultType DLG_HAMANAGER_STOPPED   =
    ( DLG_INFO_CODE  | DLG_HAMANAGER_FACILITY  | 0x00000006 );

const DlgSysResultType DLG_HAMANAGER_REGISTER =
    ( DLG_INFO_CODE  | DLG_HAMANAGER_FACILITY  | 0x00000007 );

const DlgSysResultType DLG_HAMANAGER_REGISTER_ERR =
    ( DLG_INFO_CODE  | DLG_HAMANAGER_FACILITY  | 0x00000008 );

const DlgSysResultType DLG_HAMANAGER_UNREGISTER =
    ( DLG_INFO_CODE  | DLG_HAMANAGER_FACILITY  | 0x00000009 );

const DlgSysResultType DLG_HAMANAGER_UNREGISTER_ERR =
    ( DLG_INFO_CODE  | DLG_HAMANAGER_FACILITY  | 0x0000000A );

const DlgSysResultType DLG_HAMANAGER_LISTEN =
    ( DLG_INFO_CODE  | DLG_HAMANAGER_FACILITY  | 0x0000000B );

const DlgSysResultType DLG_HAMANAGER_LISTEN_ERR =
    ( DLG_INFO_CODE  | DLG_HAMANAGER_FACILITY  | 0x0000000C );

const DlgSysResultType DLG_HAMANAGER_UNLISTEN =
    ( DLG_INFO_CODE  | DLG_HAMANAGER_FACILITY  | 0x0000000D );

const DlgSysResultType DLG_HAMANAGER_UNLISTEN_ERR =
    ( DLG_INFO_CODE  | DLG_HAMANAGER_FACILITY  | 0x0000000E );

const DlgSysResultType DLG_HAMANAGER_ERR        =
    ( DLG_INFO_CODE  | DLG_HAMANAGER_FACILITY  | 0x0000000F );

const DlgSysResultType DLG_HAMANAGER_AGENT_NOT_FOUND        =
    ( DLG_ERROR_CODE  | DLG_HAMANAGER_FACILITY  | 0x00000010 );



/*  FOR INTERNAL USE ONLY.
    Ha Manager Framework  Message  Definition 
    For use by Intel Corporation not for client use.
 */

/*
    example how to define external msg id.
    const DlgSysResultType DLG_EXTERNAL_EXAMPLE =   
    ( DLG_INTERNAL_MASK | DLG_ERROR_CODE | DLG_HAMANAGER_FACILITY | 0x00000000);

*/

#endif
