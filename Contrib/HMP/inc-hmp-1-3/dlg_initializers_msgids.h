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

#ifndef _DLG_INITIALIZERS_MSGID_H
#define _DLG_INITIALIZERS_MSGID_H

#include "dlgfacil.h"


 
/*  
    Initializer Framework  Message  Definition 
    For use by clients.
 */

/*
    example how to define internal msg id.

const DlgSysResultType DLG_INTERNAL_EXAMPLE = 
( DLG_INFO_CODE  | DLG_INITIALIZER_SERVICE_FACILITY | 0x00000000 );     

  
*/
  


/*  FOR INTERNAL USE ONLY.
    Initializer   Framework  Message  Definition 
    For use by dialogic not for client use.
 */

/*
    example how to define external msg id.

const DlgSysResultType DLG_EXTERNAL_EXAMPLE = 
( DLG_INTERNAL_MASK | DLG_ERROR_CODE | DLG_INITIALIZER_SERVICE_FACILITY | 0x00000000);

*/
const DlgSysResultType DLG_INITIALIZER_OK = ( DLG_INFO_CODE  |DLG_INITIALIZER_SERVICE_FACILITY | 0x00000000 );

const DlgSysResultType DLG_INITIALIZER_INVALID_AGENT = ( DLG_ERROR_CODE  |DLG_INITIALIZER_SERVICE_FACILITY | 0x00000001 );

const DlgSysResultType DLG_INITIALIZER_ERROR_DEVICE = ( DLG_ERROR_CODE  |DLG_INITIALIZER_SERVICE_FACILITY |0x00000002 );     

const DlgSysResultType DLG_INITIALIZER_ERROR_STARTUP = ( DLG_ERROR_CODE  |DLG_INITIALIZER_SERVICE_FACILITY | 0x00000003 ); 

const DlgSysResultType DLG_INITIALIZER_DB_ERROR = ( DLG_ERROR_CODE  |DLG_INITIALIZER_SERVICE_FACILITY | 0x00000004 ); 

const DlgSysResultType DLG_INITIALIZER_AGENT_NOT_FOUND = ( DLG_ERROR_CODE  |DLG_INITIALIZER_SERVICE_FACILITY| 0x00000005 );

const DlgSysResultType DLG_INITIALIZER_PARTIAL_SUCCESS = ( DLG_WARN_CODE  |DLG_INITIALIZER_SERVICE_FACILITY| 0x00000006 );

const DlgSysResultType DLG_INITIALIZER_FAIL = ( DLG_ERROR_CODE  |DLG_INITIALIZER_SERVICE_FACILITY| 0x00000007);

const DlgSysResultType DLG_INITIALIZER_ERROR_SEND_EVENT = ( DLG_ERROR_CODE  |DLG_INITIALIZER_SERVICE_FACILITY| 0x00000008);

const DlgSysResultType DLG_INITIALIZER_DISABLE_DEVICE = ( DLG_INFO_CODE  |DLG_INITIALIZER_SERVICE_FACILITY| 0x00000009);

const DlgSysResultType DLG_INITIALIZER_CONNECTION_FAIL = ( DLG_ERROR_CODE  |DLG_INITIALIZER_SERVICE_FACILITY| 0x0000000A);

const DlgSysResultType DLG_INITIALIZER_OB_EXCEPTION = ( DLG_ERROR_CODE  |DLG_INITIALIZER_SERVICE_FACILITY| 0x0000000B);


#endif
