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

#ifndef _DLG_DATAMGR_MSGID_H
#define _DLG_DATAMGR_MSGID_H

#include "dlgfacil.h"


 
/*  
    Data Manager  Framework  Message  Definition 
    For use by clients.
 */


const DlgSysResultType DLG_DBMGR_ERROR_NULL_PROXY   	= ( DLG_ERROR_CODE  | DLG_DATA_MGR_FACILITY | 0x00000000 );     
const DlgSysResultType DLG_DBMGR_COMM_SYSTEM_ERROR  	= ( DLG_ERROR_CODE  | DLG_DATA_MGR_FACILITY | 0x00000001 );     
const DlgSysResultType DLG_DBMGR_INVALID_NODE_ERROR 	= ( DLG_ERROR_CODE  | DLG_DATA_MGR_FACILITY | 0x00000002 );     
const DlgSysResultType DLG_DBMGR_INVALID_SHELFID_ERROR 	= ( DLG_ERROR_CODE  | DLG_DATA_MGR_FACILITY | 0x00000003 ); 
const DlgSysResultType DLG_DBMGR_COMM_INIT_ERROR	= ( DLG_ERROR_CODE  | DLG_DATA_MGR_FACILITY | 0x00000004 ); 
const DlgSysResultType DLG_DBMGR_CANT_RESOLVE_SERVER_ERROR = ( DLG_ERROR_CODE  | DLG_DATA_MGR_FACILITY | 0x00000005 ); 
const DlgSysResultType DLG_DBMGR_ERROR_GETPRODUCTINFOMGR = ( DLG_ERROR_CODE  | DLG_DATA_MGR_FACILITY | 0x00000006 ); 
const DlgSysResultType DLG_DBMGR_ERROR_GETING_MGRS 	= ( DLG_ERROR_CODE  | DLG_DATA_MGR_FACILITY | 0x00000007 ); 
const DlgSysResultType DLG_DBMGR_REPLY_EMPTY 		= ( DLG_ERROR_CODE  | DLG_DATA_MGR_FACILITY | 0x00000008 ); 
const DlgSysResultType DLG_DBMGR_FAIL_GETTING_IPADDRESS = ( DLG_ERROR_CODE  | DLG_DATA_MGR_FACILITY | 0x00000009 ); 
const DlgSysResultType DLG_DBMGR_INVALID_PRODUCT   	= ( DLG_ERROR_CODE  | DLG_DATA_MGR_FACILITY | 0x0000000A ); 
const DlgSysResultType DLG_DBMGR_INVALID_BLOCK_ID	= ( DLG_ERROR_CODE  | DLG_DATA_MGR_FACILITY | 0x0000000B ); 
const DlgSysResultType DLG_DBMGR_FUNTION_NOT_IMPLEMETNED= ( DLG_ERROR_CODE  | DLG_DATA_MGR_FACILITY | 0x0000000C ); 
const DlgSysResultType DLG_DBMGR_TECHNOLOGY_NOTIMPLEMETNED= (DLG_ERROR_CODE | DLG_DATA_MGR_FACILITY | 0x0000000D ); 
const DlgSysResultType DLG_DBMGR_CANT_RESOLVE_BUS_SERVANT= (DLG_ERROR_CODE | DLG_DATA_MGR_FACILITY | 0x0000000E ); 
const DlgSysResultType DLG_DBMGR_CANT_RESOLVE_SYSTEM_SERVANT= (DLG_ERROR_CODE | DLG_DATA_MGR_FACILITY | 0x0000000F ); 
const DlgSysResultType DLG_DBMGR_CANT_RESOLVE_TIMESLOT_SERVANT=(DLG_ERROR_CODE | DLG_DATA_MGR_FACILITY | 0x00000010); 
const DlgSysResultType DLG_DBMGR_CANT_RESOLVE_DM3PRODUCT_SERVANT= (DLG_ERROR_CODE | DLG_DATA_MGR_FACILITY | 0x00000011); 
const DlgSysResultType DLG_DBMGR_CANT_RESOLVE_PMACPRODUCT_SERVANT= (DLG_ERROR_CODE | DLG_DATA_MGR_FACILITY | 0x00000012); 
const DlgSysResultType DLG_DBMGR_CANT_RESOLVE_ANYPRODUCT_SERVANT= (DLG_ERROR_CODE | DLG_DATA_MGR_FACILITY | 0x00000013); 
const DlgSysResultType DLG_DBMGR_CANT_ACTIVATE_BOARD_SERVANT= (DLG_ERROR_CODE | DLG_DATA_MGR_FACILITY | 0x00000014); 
const DlgSysResultType DLG_DBMGR_CANT_FIND_BOARD_SERVANT= (DLG_ERROR_CODE | DLG_DATA_MGR_FACILITY | 0x00000015); 
const DlgSysResultType DLG_DBMGR_CANT_RESOLVE_CLOCKING_SERVANT= (DLG_ERROR_CODE | DLG_DATA_MGR_FACILITY | 0x00000016); 

  


/*  FOR INTERNAL USE ONLY.
    Data Manager Framework  Message  Definition 
    For use by dialogic not for client use.
 */

/*
    example how to define external msg id.
const DlgSysResultType DLG_EXTERNAL_EXAMPLE = 
( DLG_INTERNAL_MASK | DLG_ERROR_CODE | DLG_DATA_MGR_FACILITY | 0x00000000);

*/




#endif
