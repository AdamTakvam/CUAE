/*
    Copyright (C) 2000-2002.  Intel Corporation.
 
    All Rights Reserved.  All names, products,
    and services mentioned herein are the trademarks
    or registered trademarks of their respective organizations
    and are the sole property of their respective owners.
 */
 
 
/*
*    AUTO-VERSIONING HEADER  DO NOT HAND MODIFY
*    ===================================================================
*    %name:          dlg_dm3diagnosticsagent_msgids.h %
*    %version:       2 %
*    %instance:      hsw_1 %
*    %created_by:    yangl %
*    %date_modified: Thu Jul 24 12:37:57 2003 %
*    ===================================================================
*/

#ifndef __DLG_DM3DIAGNOSTICSAGENT_MSGID_H__
#define __DLG_DM3DIAGNOSTICSAGENT_MSGID_H__

#include "dlgfacil.h"

/*  
    Initializer Framework  Message  Definition 
    For use by clients.
 */

/*
    example how to define internal msg id.

const DlgSysResultType DLG_INTERNAL_EXAMPLE = 
( DLG_INFO_CODE  | DLG_DM3DIAGNOSTICSAGENT_SERVICE_FACILITY | 0x00000000 );     

  
*/
  


/*  FOR INTERNAL USE ONLY.
    Initializer   Framework  Message  Definition 
    For use by dialogic not for client use.
 */

/*
    example how to define external msg id.

const DlgSysResultType DLG_EXTERNAL_EXAMPLE = 
( DLG_INTERNAL_MASK | DLG_ERROR_CODE | DLG_DM3DIAGNOSTICSAGENT_SERVICE_FACILITY | 0x00000000);

*/
const DlgSysResultType DLG_DM3DIAGNOSTICSAGENT_OK = 
    ( DLG_INFO_CODE | DLG_DM3DIAGNOSTICSAGENT_FACILITY | 0x00000000 );

const DlgSysResultType DLG_DM3DIAGNOSTICSAGENT_FAIL = 
    ( DLG_ERROR_CODE | DLG_DM3DIAGNOSTICSAGENT_FACILITY | 0x00000001);

const DlgSysResultType DLG_DM3DIAGNOSTICSAGENT_CREATION_FAILED = 
    ( DLG_ERROR_CODE | DLG_DM3DIAGNOSTICSAGENT_FACILITY | 0x00000002);

const DlgSysResultType DLG_DM3DIAGNOSTICSAGENT_AGENT_NOT_FOUND = 
    ( DLG_ERROR_CODE | DLG_DM3DIAGNOSTICSAGENT_FACILITY | 0x00000003);

const DlgSysResultType DLG_DM3DIAGNOSTICSAGENT_INVALID_INPUT = 
    ( DLG_ERROR_CODE | DLG_DM3DIAGNOSTICSAGENT_FACILITY | 0x00000004 );

const DlgSysResultType DLG_DM3DIAGNOSTICSAGENT_ERROR_DEVICE = 
    ( DLG_ERROR_CODE | DLG_DM3DIAGNOSTICSAGENT_FACILITY | 0x00000005 );     

const DlgSysResultType DLG_DM3DIAGNOSTICSAGENT_DB_ERROR = 
    ( DLG_ERROR_CODE | DLG_DM3DIAGNOSTICSAGENT_FACILITY | 0x00000006 ); 

const DlgSysResultType DLG_DM3DIAGNOSTICSAGENT_GET_DRIVER_STATUS = 
    ( DLG_ERROR_CODE | DLG_DM3DIAGNOSTICSAGENT_FACILITY | 0x00000007 ); 

const DlgSysResultType DLG_DM3DIAGNOSTICSAGENT_DOWNLOAD_DIAGNOSE = 
    ( DLG_ERROR_CODE | DLG_DM3DIAGNOSTICSAGENT_FACILITY | 0x00000008 ); 

const DlgSysResultType DLG_DM3DIAGNOSTICSAGENT_FEATURE_NOT_SUPPORTED = 
    ( DLG_ERROR_CODE | DLG_DM3DIAGNOSTICSAGENT_FACILITY | 0x00000009 ); 

#endif
