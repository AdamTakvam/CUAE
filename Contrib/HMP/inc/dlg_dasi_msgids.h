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

#ifndef _DLG_DASI_MSGID_H
#define _DLG_DASI_MSGID_H

#include "dlgfacil.h"


/*  
    DASI Framework  Message  Definition 
    For use by clients.
 */

/*
    example how to define external msg id.
const DlgSysResultType DLG_EXTERNAL_EXAMPLE = 
( DLG_ERROR_CODE | DLG_DASI_FACILITY | 0x00000000);

*/
const DlgSysResultType DLG_DASI_ERROR_ORBINIT = 
(DLG_ERROR_CODE | DLG_DASI_FACILITY | 0x00000000);

const DlgSysResultType DLG_DASI_ERROR_SYSTEM = 
(DLG_ERROR_CODE | DLG_DASI_FACILITY | 0x00000001);

const DlgSysResultType DLG_DASI_ERROR_BOARD = 
(DLG_ERROR_CODE | DLG_DASI_FACILITY | 0x00000002);

const DlgSysResultType DLG_DASI_ERROR_SYSINIT = 
(DLG_ERROR_CODE | DLG_DASI_FACILITY | 0x00000003);

const DlgSysResultType DLG_DASI_ERROR_ORBDESTROY = 
(DLG_ERROR_CODE | DLG_DASI_FACILITY | 0x00000004);

const DlgSysResultType DLG_DASI_ERROR_UNKNOWN = 
(DLG_ERROR_CODE | DLG_DASI_FACILITY | 0x00000005);

const DlgSysResultType DLG_DASI_ERROR_CORBA = 
(DLG_ERROR_CODE | DLG_DASI_FACILITY | 0x00000006);

const DlgSysResultType DLG_DASI_ERROR_NODE_ALLOCATION = 
(DLG_ERROR_CODE | DLG_DASI_FACILITY | 0x00000007);

const DlgSysResultType DLG_DASI_ERROR_INVALID_PARM = 
(DLG_ERROR_CODE | DLG_DASI_FACILITY | 0x00000008);

const DlgSysResultType DLG_DASI_ERROR_INVALID_ARG = 
(DLG_ERROR_CODE | DLG_DASI_FACILITY | 0x00000009);

const DlgSysResultType DLG_DASI_ERROR_READONLY = 
(DLG_ERROR_CODE | DLG_DASI_FACILITY | 0x0000000A);

const DlgSysResultType DLG_DASI_ERROR_INVALID_OS = 
(DLG_ERROR_CODE | DLG_DASI_FACILITY | 0x0000000B);

const DlgSysResultType DLG_DASI_ERROR_CFGENVIRONMENT = 
(DLG_ERROR_CODE | DLG_DASI_FACILITY | 0x0000000C);

const DlgSysResultType DLG_DASI_ERROR_FUNCTION_UNSUPPORTED =
(DLG_ERROR_CODE | DLG_DASI_FACILITY | 0x0000000D);

/*  FOR INTERNAL USE ONLY.
    DASI Framework  Message  Definition 
    For use by dialogic not for client use.
 */

/*
    example how to define internal msg id.
const DlgSysResultType DLG_INTERNAL_EXAMPLE  = 
( DLG_INTERNAL_MASK | DLG_INFO_CODE  | DLG_DASI_FACILITY | 0x00000000 );     

  
*/

#endif
