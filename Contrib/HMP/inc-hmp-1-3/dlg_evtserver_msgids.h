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

#ifndef _DLG_EXTERNAL_DLG_EVENT_SERVICE_ERROR_H
#define _DLG_EXTERNAL_DLG_EVENT_SERVICE_ERROR_H

#include "dlgfacil.h"


/*  
    Dialogic Eventing Framework External Message and Error Definition 
    Facility = DLG_EVENT_SERVER_FACILITY
    For use by clients.
 */

const DlgSysResultType DLG_EVENT_READY = 
( DLG_INFO_CODE  | DLG_EVENT_SERVER_FACILITY | 0x00000000 );     

const DlgSysResultType DLG_EVENT_ERROR_INIT = 
( DLG_ERROR_CODE | DLG_EVENT_SERVER_FACILITY | 0x00000001 );       

const DlgSysResultType DLG_EVENT_NOT_READY = 
( DLG_ERROR_CODE | DLG_EVENT_SERVER_FACILITY | 0x00000002 );       

const DlgSysResultType DLG_EVENT_CHANNEL_ERROR = 
( DLG_ERROR_CODE | DLG_EVENT_SERVER_FACILITY | 0x00000003 );       

/*  

    For Internal Use Only. Not to be used by clients.
    Dialogic Eventing Framework Message Definition 
    Facility = DLG_EVENT_SERVER_FACILITY
    Internal Error, Event, and Info Message Ids.

 */

const DlgSysResultType DLG_CONSUMER_EVT_DISCONECT_FROM_CHANN =
( DLG_INTERNAL_MASK | DLG_EVENT_CODE  | DLG_EVENT_SERVER_FACILITY | 0x00000000); 

const DlgSysResultType DLG_CONSUMER_ERR_ORB_INIT_FAIL = 
( DLG_INTERNAL_MASK | DLG_ERROR_CODE | DLG_EVENT_SERVER_FACILITY | 0x00000001);  

const DlgSysResultType DLG_CONSUMER_EVT_GET_CHAN_ERR = 
( DLG_INTERNAL_MASK | DLG_EVENT_CODE | DLG_EVENT_SERVER_FACILITY | 0x00000002);


const DlgSysResultType DLG_CONSUMER_WARN_ORB_SHUTDOWN = 
( DLG_INTERNAL_MASK | DLG_WARN_CODE | DLG_EVENT_SERVER_FACILITY | 0x00000003);


const DlgSysResultType DLG_CONSUMER_WARN_FILTER_NOTFOUND = 
( DLG_INTERNAL_MASK | DLG_WARN_CODE | DLG_EVENT_SERVER_FACILITY | 0x00000004);


const DlgSysResultType DLG_CONSUMER_WARN_CALLBACK_FILTER_NOTFOUND = 
( DLG_INTERNAL_MASK | DLG_WARN_CODE | DLG_EVENT_SERVER_FACILITY | 0x00000005);


const DlgSysResultType DLG_CONSUMER_EVT_CALLING_CALLBACK =
( DLG_INTERNAL_MASK | DLG_EVENT_CODE | DLG_EVENT_SERVER_FACILITY | 0x00000006);


const DlgSysResultType DLG_CONSUMER_EVT_NIL_CALLBACK =
( DLG_INTERNAL_MASK | DLG_WARN_CODE | DLG_EVENT_SERVER_FACILITY | 0x00000007);


const DlgSysResultType DLG_CONSUMER_ERR_XLATING_EVENTMSG = 
( DLG_INTERNAL_MASK | DLG_EVENT_CODE | DLG_EVENT_SERVER_FACILITY | 0x00000008);


const DlgSysResultType DLG_CONSUMER_EVT_CONN_DEF_CHANNEL = 
( DLG_INTERNAL_MASK | DLG_EVENT_CODE | DLG_EVENT_SERVER_FACILITY | 0x00000009);


const DlgSysResultType DLG_CONSUMER_ERR_NOTRESOLVE_EVENTSERVICE_REF = 
( DLG_INTERNAL_MASK | DLG_ERROR_CODE | DLG_EVENT_SERVER_FACILITY | 0x00000010);


const DlgSysResultType DLG_CONSUMER_ERR_EVENTSERVER_NOTRUNNING = 
( DLG_INTERNAL_MASK | DLG_ERROR_CODE | DLG_EVENT_SERVER_FACILITY | 0x00000011);

#endif
