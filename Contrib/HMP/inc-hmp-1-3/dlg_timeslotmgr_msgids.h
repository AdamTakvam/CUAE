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

#ifndef _DLG_TIMESLOT_MGR_MSGID_H
#define _DLG_TIMESLOT_MGR_MSGID_H

#include "dlgfacil.h"


/*  
    Timeslot Manager Framework  Message  Definition 
    For use by clients.
 */

/*
    example how to define internal msg id.

const DlgSysResultType DLG_INTERNAL_EXAMPLE  = 
( DLG_INFO_CODE  | DLG_TIMESLOT_MGR_FACILITY | 0x00000000 );     

  
*/
/*  
    Timeslot Mgr Message  Definition 
    For use by clients.
 */
const DlgSysResultType DLG_TIMESLOT_MGR_OK = ( DLG_INFO_CODE  | DLG_TIMESLOT_MGR_FACILITY | 0x00000000 );

const DlgSysResultType DLG_TIMESLOT_MGR_INVALID_AUID = ( DLG_ERROR_CODE  | DLG_TIMESLOT_MGR_FACILITY | 0x00000001 );

const DlgSysResultType DLG_TIMESLOT_MGR_INVALID_CLAIMID = ( DLG_ERROR_CODE  | DLG_TIMESLOT_MGR_FACILITY | 0x00000002 );     

const DlgSysResultType DLG_TIMESLOT_MGR_DB_EXCEPITON= ( DLG_ERROR_CODE  | DLG_TIMESLOT_MGR_FACILITY | 0x00000003 ); 

const DlgSysResultType DLG_TIMESLOT_MGR_INSUFFICIENT_TIMESLOTS = ( DLG_ERROR_CODE  | DLG_TIMESLOT_MGR_FACILITY | 0x00000004 ); 

const DlgSysResultType DLG_TIMESLOT_MGR_INVALID_BUSID = ( DLG_ERROR_CODE  | DLG_TIMESLOT_MGR_FACILITY | 0x00000005 );

const DlgSysResultType DLG_TIMESLOT_MGR_INVALID_INPUT = ( DLG_ERROR_CODE  | DLG_TIMESLOT_MGR_FACILITY | 0x00000006 );

const DlgSysResultType DLG_TIMESLOT_MGR_INVALID_STARTTIMESLOT = ( DLG_ERROR_CODE  | DLG_TIMESLOT_MGR_FACILITY | 0x00000007);

const DlgSysResultType DLG_TIMESLOT_MGR_ERROR_SERVER_CONNECTION = ( DLG_ERROR_CODE  | DLG_TIMESLOT_MGR_FACILITY | 0x00000008);

const DlgSysResultType DLG_TIMESLOT_MGR_SERVER_EXCEPTION = ( DLG_ERROR_CODE  | DLG_TIMESLOT_MGR_FACILITY | 0x00000009);


/*  FOR INTERNAL USE ONLY.
    Timeslot Manager  Framework  Message  Definition 
    For use by dialogic not for client use.
 */

/*
    example how to define external msg id.
const DlgSysResultType DLG_EXTERNAL_EXAMPLE =   
( DLG_INTERNAL_MASK | DLG_ERROR_CODE | DLG_TIMESLOT_MGR_FACILITY | 0x00000000);
*/





#endif
