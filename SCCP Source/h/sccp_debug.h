/*
 *  Copyright (c) 2002, 2003 by Cisco Systems, Inc. All Rights Reserved.
 *
 *  This work is subject to U.S. and international copyright laws and
 *  treaties. No part of this work may be used, practiced, performed,
 *  copied, distributed, revised, modified, translated, abridged, condensed,
 *  expanded, collected, compiled, linked, recast, transformed or adapted
 *  without the prior written consent of Cisco Systems, Inc. Any use or 
 *  exploitation of this work without authorization could subject the 
 *  perpetrator to criminal and civil liability.
 *
 *  FILENAME
 *     sccp_debug.h
 *
 *  CREATION/MODIFICATION DATES
 *     Created:  November 2002, Sam Hague
 *     Modified: 
 *
 *  DESCRIPTION
 *     SCCP debug header
 */
#ifndef _SCCP_DEBUG_H_
#define _SCCP_DEBUG_H_

#include "sccp_platform.h"


#define AM_ID      "AM     "
#define PLAT_ID    "PLAT   "
#define SCCP_ID    "SCCP   "
#define SCCPCC_ID  "SCCPCC "
#define SCCPCM_ID  "SCCPCM "
#define SCCPMSG_ID "SCCPMSG"
#define SCCPREC_ID "SCCPREC"
#define SCCPREG_ID "SCCPREG"
#define SEM_ID     "SEM    "

extern int am_debug;
extern int sccp_debug;
extern int sccpmsg_debug;
extern int sccpcc_debug;
extern int sccpcm_debug;
extern int sccprec_debug;
extern int sccpreg_debug;
extern int sem_debug;

#ifdef SCCP_DEBUG
#define SCCP_DBG(a)  sccp_platform_printf a

#else
#define SCCP_DBG(a) ((void)0)

#endif /* SCCP_DEBUG */

#endif /* _SCCP_DEBUG_H_ */
