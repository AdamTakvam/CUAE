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
const DlgSysResultType DLG_TIMESLOTMGR_ERROR_INIT = ( DLG_INFO_CODE  |DLG_INITIALIZER_SERVICE_FACILITY | 0x00000001 );
const DlgSysResultType DLG_TIMESLOTMGR_READY = ( DLG_INFO_CODE  |DLG_INITIALIZER_SERVICE_FACILITY | 0x00000000 );





#endif
