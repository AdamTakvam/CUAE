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
*    %name:          dlg_detectors_msgids.h %
*    %version:       9 %
*    %instance:      hsw_1 %
*    %created_by:    dunnm %
*    %date_modified: Wed Apr 23 16:41:37 2003 %
*    ===================================================================
*/

#ifndef _DLG_DETECTORS_MSGID_H
#define _DLG_DETECTORS_MSGID_H

#include "dlgfacil.h"


 
/*  
    Detectors  Framework  Message  Definition 
    For use by clients.
 */

const DlgSysResultType  DLG_DETECTORFW_NOTRDY  = 
    ( DLG_ERROR_CODE  | DLG_DETECTORS_FACILITY | 0x00000000 );     

const DlgSysResultType  DLG_DETECTORFW_ORBINIT  = 
    ( DLG_INFO_CODE  | DLG_DETECTORS_FACILITY  | 0x00000001 );     

const DlgSysResultType DLG_DETECTORFW_NAMINGDIR_ERR  = 
    ( DLG_ERROR_CODE  | DLG_DETECTORS_FACILITY | 0x00000002 );     

const DlgSysResultType DLG_DETECTORFW_ORBINIT_ERR  = 
    ( DLG_ERROR_CODE  | DLG_DETECTORS_FACILITY | 0x00000003 );    
 
const DlgSysResultType DLG_DETECTORFW_RUNNING  = 
    ( DLG_INFO_CODE  | DLG_DETECTORS_FACILITY  | 0x00000004 );     

const DlgSysResultType DLG_DETECTORFW_STOPPED  = 
    ( DLG_INFO_CODE  | DLG_DETECTORS_FACILITY  | 0x00000005 );     

const DlgSysResultType DLG_DETECTORFW_INVALID_ADAPTOR  = 
    ( DLG_INFO_CODE  | DLG_DETECTORS_FACILITY  | 0x00000006 );    

const DlgSysResultType DLG_DETECTOR_DETECTION_ERROR  = 
    ( DLG_ERROR_CODE  | DLG_DETECTORS_FACILITY  | 0x00000007 );    

const DlgSysResultType DLG_DETECTOR_DETECTION_OK  = 
    ( DLG_INFO_CODE  | DLG_DETECTORS_FACILITY   | 0x00000008 );    

const DlgSysResultType DLG_DETECTOR_DETECTION_PARTIAL_ERRORS  = 
    ( DLG_ERROR_CODE  | DLG_DETECTORS_FACILITY  | 0x00000009 );    

const DlgSysResultType DLG_DETECTOR_CANT_CREATEAGENT_ERROR = 
    ( DLG_ERROR_CODE  | DLG_DETECTORS_FACILITY  | 0x0000000A );    




/*  FOR INTERNAL USE ONLY.
    Detectors  Framework  Message  Definition 
    Not for client use.
 */

/*
    example how to define external msg id.
    const DlgSysResultType DLG_EXTERNAL_EXAMPLE    
    = ( DLG_INTERNAL_MASK | DLG_ERROR_CODE | DLG_DETECTORS_FACILITY | 0x00000000);

*/




#endif
