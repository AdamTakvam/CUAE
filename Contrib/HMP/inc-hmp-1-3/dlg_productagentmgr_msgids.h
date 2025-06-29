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

#ifndef _DLG_PRODUCTAGENTMGR_MSGID_H
#define _DLG_PRODUCTAGENTMGR_MSGID_H

#include "dlgfacil.h"


/*  
    Clock Deamon Framework  Message  Definition 
    For use by clients.
 */



const DlgSysResultType DLG_ERR_LOADING_AGENT_PRODUCT  = 
( DLG_ERROR_CODE  | DLG_PRODUCTAGENTMGR_FACILITY | 0x00000000 );     

const DlgSysResultType DLG_ERR_INVALID_CREATE_PRODUCT_FUNCTION  = 
( DLG_ERROR_CODE  | DLG_PRODUCTAGENTMGR_FACILITY  | 0x00000001 );     

const DlgSysResultType DLG_ERR_INVALID_DESTROY_PRODUCT_FUNCTION  = 
( DLG_ERROR_CODE  | DLG_PRODUCTAGENTMGR_FACILITY  | 0x00000002 );     


const DlgSysResultType DLG_ERR_INVALID_MULTITHREAD_PRODUCT_FUNCTION  = 
( DLG_ERROR_CODE  | DLG_PRODUCTAGENTMGR_FACILITY  | 0x00000003 );     

const DlgSysResultType DLG_ERR_AGENT_NOTFOUND  = 
( DLG_ERROR_CODE  | DLG_PRODUCTAGENTMGR_FACILITY  | 0x00000004 );     

const DlgSysResultType DLG_INFO_PRODUCT_CONFIGFILE_FOUND  = 
( DLG_INFO_CODE  | DLG_PRODUCTAGENTMGR_FACILITY  | 0x00000005 );     

const DlgSysResultType DLG_ERR_PRODUCT_CONFIGFILE_NOTFOUND  = 
( DLG_ERROR_CODE  | DLG_PRODUCTAGENTMGR_FACILITY  | 0x00000006 );     


/*  FOR INTERNAL USE ONLY.
    Clock Deamon Framework  Message  Definition 
    For use by dialogic not for client use.
 */

/*
    example how to define external msg id.

const DlgSysResultType DLG_EXTERNAL_EXAMPLE = 
( DLG_INTERNAL_MASK | DLG_ERROR_CODE | DLG_CLOCKFW_FACILITY | 0x00000000);

*/


#endif
