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
*    %name:          dlgproductagenttype.h %
*    %version:       15 %
*    %instance:      hsw_1 %
*    %created_by:    cruzj %
*    %date_modified: Wed Oct 30 11:31:25 2002 %
*    ===================================================================
*/

#ifndef __DLGPRODUCTAGENTTYPE_H__
#define __DLGPRODUCTAGENTTYPE_H__

/* Product Agent Name */

#define MAX_PRODUCT_AGENT_NAME  50

#define DETECTOR_AGENT      "DETECTOR_AGENT"
#define INITIALIZER_AGENT   "INITIALIZER_AGENT"
#define FAULTDETECTOR_AGENT "FAULTDETECTOR_AGENT"
#define CLOCKING_AGENT      "CLOCKING_AGENT"
#define PNPDETECTOR_AGENT   "PNPDETECTOR_AGENT"
#define DIAGNOSTIC_AGENT    "DIAGNOSTIC_AGENT"
#define OSLEXT_AGENT        "OSLEXT_AGENT"

//For any new technology product agent  supported must be defined here

/* Technology Name */

#define MAX_TECHNOLOGY_NAME 50

#define DM3                 "DM3"
#define SPRINGWARE          "SPRINGWARE"
#define PMAC                "PMAC"
#define COMMON              "COMMON"

enum AGENT_TECHNOLOGIES { DLG_UNKNOWN=0, DLG_DM3, DLG_SPRINGWARE, DLG_PMAC };

//define Family types
#define	DM3QUADSPAN		"QUADSPAN"
#define	DM3IPLINK		"IPLINK"

enum AGENT_FAMILIES { FAMILY_UNKNOWN=0, FAMILY_QUADSPAN, FAMILY_IPLINK };

#endif  // __DLGPRODUCTAGENTTYPE_H__

