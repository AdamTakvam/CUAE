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

#ifndef _DLG_CDM_MSGID_H
#define _DLG_CDM_MSGID_H

#include "dlgfacil.h"


 
/*  
    CDM Library  Message  Definition 
    For use by clients.
 */
const DlgSysResultType DLG_CDM_OK = ( DLG_INFO_CODE  | DLG_CDM_FACILITY | 0x00000000 );
const DlgSysResultType DLG_CDM_ERROR = ( DLG_ERROR_CODE  | DLG_CDM_FACILITY | 0x00000001 );

const DlgSysResultType DLG_CDM_ERROR_DBRETRIVE	= ( DLG_ERROR_CODE  | DLG_CDM_FACILITY | 0x00000002 );     

const DlgSysResultType DLG_CDM_ERROR_DBUPDATE	= ( DLG_ERROR_CODE  | DLG_CDM_FACILITY | 0x00000003 ); 

const DlgSysResultType DLG_CDM_ERROR_AGENT_UPDATE = ( DLG_ERROR_CODE  | DLG_CDM_FACILITY | 0x00000004 ); 

const DlgSysResultType DLG_CDM_WARN = ( DLG_WARN_CODE  | DLG_CDM_FACILITY | 0x00000005 );

const DlgSysResultType DLG_CDM_WARN_EVENT = ( DLG_WARN_CODE  | DLG_CDM_FACILITY | 0x00000006 );
const DlgSysResultType DLG_CDM_ERROR_INVALID_INPUT = ( DLG_ERROR_CODE  | DLG_CDM_FACILITY | 0x00000007);
const DlgSysResultType DLG_CDM_ERROR_DB_EXCEPTION = ( DLG_ERROR_CODE  | DLG_CDM_FACILITY | 0x00000008);
const DlgSysResultType DLG_CDM_NOT_FOUND = ( DLG_INFO_CODE  | DLG_CDM_FACILITY | 0x00000009);
const DlgSysResultType DLG_CDM_FOUND = ( DLG_INFO_CODE  | DLG_CDM_FACILITY | 0x0000000A);
const DlgSysResultType DLG_CDM_ERROR_SET_TDM_CAPS = ( DLG_ERROR_CODE  | DLG_CDM_FACILITY | 0x0000000B);
const DlgSysResultType DLG_CDM_ERROR_GET_TDM_CAPS = ( DLG_ERROR_CODE  | DLG_CDM_FACILITY | 0x0000000C);
const DlgSysResultType DLG_CDM_ERROR_INVALID_STATE = ( DLG_ERROR_CODE  | DLG_CDM_FACILITY | 0x0000000D);


/*
    example how to define external msg id.
const DlgSysResultType DLG_EXTERNAL_EXAMPLE = 
( DLG_INTERNAL_MASK | DLG_ERROR_CODE | DLG_DATA_MGR_FACILITY | 0x00000000);

*/




#endif
