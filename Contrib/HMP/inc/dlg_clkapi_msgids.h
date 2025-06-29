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

#ifndef _DLG_CLKAPI_MSGID_H
#define _DLG_CLKAPI_MSGID_H

#include "dlgfacil.h"


/*  
    Clock API Framework  Message  Definition 
    For use by clients.
 */

const DlgSysResultType DLG_CLKAPI_OPENFAIL = 
(DLG_ERROR_CODE  | DLG_CLOCK_API_FACILITY | 0x00000000);

const DlgSysResultType DLG_CLKAPI_CLOSEFAIL = 
(DLG_ERROR_CODE  | DLG_CLOCK_API_FACILITY | 0x00000001);

const DlgSysResultType DLG_CLKAPI_PATHOPENFAIL = 
(DLG_ERROR_CODE  | DLG_CLOCK_API_FACILITY | 0x00000002);

const DlgSysResultType DLG_CLKAPI_PATHCLOSEFAIL = 
(DLG_ERROR_CODE  | DLG_CLOCK_API_FACILITY | 0x00000003);

const DlgSysResultType DLG_CLKAPI_MESSAGEFAIL = 
(DLG_ERROR_CODE  | DLG_CLOCK_API_FACILITY | 0x00000004);

const DlgSysResultType DLG_CLKAPI_COMMUNICATIONSFAIL = 
(DLG_ERROR_CODE  | DLG_CLOCK_API_FACILITY | 0x00000005);

const DlgSysResultType DLG_CLKAPI_WRONGREPLYFAIL = 
(DLG_ERROR_CODE  | DLG_CLOCK_API_FACILITY | 0x00000006);

const DlgSysResultType DLG_CLKAPI_FIRMWAREFAIL = 
(DLG_ERROR_CODE  | DLG_CLOCK_API_FACILITY | 0x00000007);

const DlgSysResultType DLG_CLKAPI_INVALID_CLOCKMODEL =
(DLG_ERROR_CODE  | DLG_CLOCK_API_FACILITY | 0x00000008);

const DlgSysResultType DLG_CLKAPI_INVALID_MASTERROLE =
(DLG_ERROR_CODE  | DLG_CLOCK_API_FACILITY | 0x00000009);

const DlgSysResultType DLG_CLKAPI_INVALID_CLOCKLINE =
(DLG_ERROR_CODE  | DLG_CLOCK_API_FACILITY | 0x0000000A);

const DlgSysResultType DLG_CLKAPI_INVALID_BUSTYPE =
(DLG_ERROR_CODE  | DLG_CLOCK_API_FACILITY | 0x0000000B);

const DlgSysResultType DLG_CLKAPI_INVALID_CLOCKRATE =
(DLG_ERROR_CODE  | DLG_CLOCK_API_FACILITY | 0x0000000C);

const DlgSysResultType DLG_CLKAPI_INVALID_CLOCKSOURCE =
(DLG_ERROR_CODE  | DLG_CLOCK_API_FACILITY | 0x0000000D);

const DlgSysResultType DLG_CLKAPI_NOT_CAPABLE =
(DLG_ERROR_CODE  | DLG_CLOCK_API_FACILITY | 0x0000000E);

const DlgSysResultType DLG_CLKAPI_INVALID_PARAMETER =
(DLG_ERROR_CODE  | DLG_CLOCK_API_FACILITY | 0x0000000F);


/*  FOR INTERNAL USE ONLY.
    Clock API Framework  Message  Definition 
    For use by dialogic not for client use.
 */

/*
    example how to define external msg id.
const DlgSysResultType DLG_EXTERNAL_EXAMPLE= 
( DLG_INTERNAL_MASK | DLG_ERROR_CODE | DLG_CLOCK_API_FACILITY | 0x00000000);
*/





#endif
