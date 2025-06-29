/****************************************************************************
 *                     Copyright (C) 1994-1997 Dialogic Corp.
 *                           All Rights Reserved
 ****************************************************************************
 *
 ****************************************************************************
 * 
 *                                 TITLE
 *
 * FILE: PRTMSD300LIB.H
 *
 * REVISION: 
 *
 * DATE: 
 *
 * PURPOSE: 
 *
 * INTERFACE: None
 *
 * Note: None
 *
 * REVISION HISTORY:
 *
 *
 ****************************************************************************/


#ifndef __PRTMSD300LIB_H__
#define __PRTMSD300LIB_H__

#include "prtmscmd.h"
#include "cclib.h"
#include "protimslib.h"
#include "prtmserr.h"


/* Code for PROTIMS */
#define CCEV_DLI_STARTLOOPBACK (DT_CC | 0x66) 
#define CCEV_DLI_STOPLOOPBACK (DT_CC | 0x67) 
#define CCEV_RING					(DT_CC | 0x68) /* a connection request has been made */
#define CCEV_CALL		(DT_CC | 0x69)
#define CCEV_DLI_CALLID	(DT_CC | 0x6A)
#define CCEV_DLI_CHNSTATUS	(DT_CC | 0x6B)
#define CCEV_DLI_PBXMSG	(DT_CC | 0x6C)
#define CCEV_CALLDISCONNECT (DT_CC | 0x6D)
#define PRTMSEV_RING       (DT_CC | 0x70) /* a connection request has been made */
#define PRTMSEV_CALL		(DT_CC | 0x71)
#define PRTMSEV_DLI_CALLID	(DT_CC | 0x72)
#define PRTMSEV_DLI_CHNSTATUS	(DT_CC | 0x73)
#define PRTMSEV_DLI_PBXMSG	(DT_CC | 0x74)
#define PRTMSEV_CALLDISCONNECT (DT_CC | 0x75)
#define CCEV_HOOKSTATE	(DT_CC | 0x76)
#define PRTMSEV_HOOKSTATE (DT_CC | 0x77)
#define CCEV_RINGING		(DT_CC | 0x78)
#define PRTMSEV_RINGING		(DT_CC | 0x79)
#define CCEV_DIALING		(DT_CC | 0x7A)
#define PRTMSEV_DIALING		(DT_CC | 0x7B)
/* Code for PROTIMS ends here */

#endif   /* for __PRTMSD300LIB_H__ */
