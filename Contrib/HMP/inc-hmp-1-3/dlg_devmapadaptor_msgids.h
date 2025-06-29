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

#ifndef _DLG_DEVMAPADPATOR_MSGID_H
#define _DLG_DEVMAPADPATOR_MSGID_H

#include "dlgfacil.h"


/*  
    DeviceMapper Adaptor Message  Definition 
    For use by clients.
 */

const DlgSysResultType DLG_DEVMAPADAPTOR_SUCCESS = 0; 

const DlgSysResultType DLG_DEVMAPADAPTOR_DATAMANAGER_FAIL = 
(DLG_ERROR_CODE  | DLG_DEVMAPADAPTOR_FACILITY | 0x00000000);

const DlgSysResultType DLG_DEVMAPADAPTOR_DEVMAP_FAIL = 
(DLG_ERROR_CODE  | DLG_DEVMAPADAPTOR_FACILITY | 0x00000001);

#endif
