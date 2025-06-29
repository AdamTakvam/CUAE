/**********************************************************************
 *
 *	C Header:		GcVoip.h
 *	Instance:		dte_1
 *	Description:	GC VOIP MakeCall Block and constants
 *	%created_by:	gollapuc %
 *	%date_created:	Thu Jan 02 15:24:27 2003 %
 *	%created_by:	gollapuc %
 *	%date_created:	Thu Jan 02 15:24:27 2003 %
 *
 **********************************************************************/
/**********************************************************************
 *  Copyright (C) 2000-2002 Intel Corporation
 *  All Rights Reserved
 *
 *  THIS IS UNPUBLISHED PROPRIETARY SOURCE CODE OF Dialogic Corp.
 *  The copyright notice above does not evidence any actual or
 *  intended publication of such source code.
 **********************************************************************/

#ifndef _dte_1_GcVoip_h_H
#define _dte_1_GcVoip_h_H

#ifndef lint
static char    *_dte_1_GcVoip_h = "@(#) %filespec: GcVoip.h-11 %  (%full_filespec: GcVoip.h-11:incl:dte#1 %)";
#endif

#include "qtypes.h"
#include "ntscdefs.h"
#include "gcip_defs.h"

/* Additionnal parm for gc_SetParm */
#ifndef GCPR_INCOMING_CODERS	
#define GCPR_INCOMING_CODERS		0x5258
#endif

#ifndef GCPR_OUTGOING_CODERS	
#define GCPR_OUTGOING_CODERS		0x5458
#endif

/* Structure used in gc_SetParm, 
	with parm GCPR_INCOMING_CODERS and GCPR_OUTGOING_CODERS */
typedef struct Ip_Coders {
	NetTSC_Coder_t* CoderList;
	int nCoders;
} IP_CODERS;

/* Structure used as cclib* in gc_MakeCall */
typedef struct Ip_MakeCallBlock {
	NetTSC_Coder_t* CoderList;
	int nCoders;
	char *Display;
	unsigned int DisplayLength;
	char *IPUUI;
	unsigned int UUILength;
	char *SourceAddress;
	NetTSC_NonStdParm_t nonStandardData;
	char *PhoneList;
} IP_MAKECALL_BLK;
#endif // GcVoip.h

/* Additionnal parm for GetCallInfo */
#ifndef GC_CallInfo_TxCoder
	// This parm takes a NetTSC_Coder_t * as 3rd argument of gc_GetCallInfo
	#define GC_CallInfo_TxCoder	0x6801
#endif

#ifndef GC_CallInfo_RxCoder
	// This parm takes a NetTSC_Coder_t * as 3rd argument of gc_GetCallInfo
	#define GC_CallInfo_RxCoder	0x6802
#endif

#ifndef GC_CallInfo_NonStdParm
	// This parm takes a NetTSC_NonStdParm_t * as 3rd argument of gc_GetCallInfo
	#define GC_CallInfo_NonStdParm	0x6803
#endif

#ifndef MAX_DISPLAY_LENGTH
#define MAX_DISPLAY_LENGTH		82
#endif

#ifndef GC_CallInfo_Display
	// This parm takes char[MAX_DISPLAY_LENGTH] as 3rd argument of gc_GetCallInfo
	#define GC_CallInfo_Display	0x6804
#endif

#ifndef GC_CallInfo_DurationTime
	// This parm takes int * as 3rd argument of gc_GetCallInfo
	#define GC_CallInfo_DurationTime	0x6805
#endif

// Will also be via ipm_GetSessionInfo, when ASYNC is needed.
#ifndef GC_CallInfo_RTCPInfo
	// This parm takes NetTSC_RTCPInfo_t * as 3rd argument of gc_GetCallInfo
	#define GC_CallInfo_RTCPInfo	0x6806
#endif

#ifndef GC_CallInfo_PhoneList
	// This parm takes a char[GC_ADDRSIZE]
	#define GC_CallInfo_PhoneList	0x6807
#endif

#ifndef SndMsg_NonStdData
#define SndMsg_NonStdData	0x1e00
#endif

#ifndef SndMsg_Q931Facility
#define SndMsg_Q931Facility	0x1e01
#endif
