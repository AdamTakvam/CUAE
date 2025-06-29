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

#ifndef _DLG_CLKFRAMEWORK_MSGID_H
#define _DLG_CLKFRAMEWORK_MSGID_H

#include "dlgfacil.h"


/*  
    Clock Deamon Framework  Message  Definition 
    For use by clients.
 */



const DlgSysResultType DLG_CLOCKFW_RUNNING  = 
    ( DLG_INFO_CODE  | DLG_CLOCKFW_FACILITY | 0x00000000 );     

const DlgSysResultType DLG_CLOCKFW_NOTRDY  = 
    ( DLG_INFO_CODE  | DLG_CLOCKFW_FACILITY | 0x00000001 );     

const DlgSysResultType DLG_CLOCKFW_STOPPED  = 
    ( DLG_INFO_CODE  | DLG_CLOCKFW_FACILITY | 0x00000002 );     


const DlgSysResultType DLG_CLOCKFW_ORBINIT_ERR  = 
    ( DLG_ERROR_CODE  | DLG_CLOCKFW_FACILITY | 0x00000003 );     
  
const DlgSysResultType DLG_CLOCKFW_EVENT_CONSUMER_ERR  = 
    ( DLG_ERROR_CODE  | DLG_CLOCKFW_FACILITY | 0x00000004 );     

const DlgSysResultType DLG_CLOCKFW_NAMINGDIR_ERR  = 
    ( DLG_ERROR_CODE  | DLG_CLOCKFW_FACILITY | 0x00000005 );  

const DlgSysResultType DLG_CLOCKFW_ORBINIT  = 
    ( DLG_ERROR_CODE  | DLG_CLOCKFW_FACILITY | 0x00000006 );  


   
  


/*  FOR INTERNAL USE ONLY.
    Clock Deamon Framework  Message  Definition 
    For use by dialogic not for client use.
 */

/*
    example how to define external msg id.
    const DlgSysResultType DLG_EXTERNAL_EXAMPLE    
    = ( DLG_INTERNAL_MASK | DLG_ERROR_CODE | DLG_CLOCKFW_FACILITY | 0x00000000);
*/


#endif
