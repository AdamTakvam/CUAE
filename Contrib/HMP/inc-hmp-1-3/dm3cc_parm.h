/**********************************************************************
*
*	C Header:		dm3cc_parm.h
*	Instance:		dte_1
*	Description:	DM3CC Call Control Library Parameters
*	%date_created:	Tue Sep 30 12:42:02 2003 %
*	%created_by:	simonarx %
*
**********************************************************************/
/**********************************************************************
 *  Copyright (C) 2003 Intel Corporation.
 *  All Rights Reserved
 *
 *  All names, products, and services mentioned herein are the
 *  trademarks or registered trademarks of their respective
 *  organizations and the sole property of their respective owner
 **********************************************************************/

#ifndef _dte_1_dm3cc_parm_h_H
#define _dte_1_dm3cc_parm_h_H

#ifndef lint
static char    *_dte_1_dm3cc_parm_h = "@(#) %filespec: dm3cc_parm.h-12 %  (%full_filespec: dm3cc_parm.h-12:incl:dte#1 %)";
#endif

/********************************************************************************************/
/****************** Extension Ids (ext_id in gc_Extension and/or for GCEV_EXTENSION) ********/
/********************************************************************************************/
#define CC_EXT_EVT_BCHAN_NEGOTIATED         1
#define CC_EXT_EVT_CALL_REJECTED            2
#define CC_EXT_EVT_XFER                     3
#define CC_EXT_EVT_DIAL                     4
#define CC_EXT_EVT_CALL_DIALING             5
#define CC_EXT_EVT_CALL_ORIGINATED          6
#define DM3CC_EXID_BIT_PATTERN              7
#define DM3CC_EXID_SETDLINKSTATE            0x0a	//the same as GCIS_EXID_SETDLINKSTATE in SW

/* And the parameters to enable them (using gc_SetParm) */
#define DM3CC_PARM_BCHAN_NEGOTIATED_EVENT	0xC001
#define DM3CC_PARM_CALL_REJECTED_EVENT		0xC002

/********************************************************************************************/
/******************************** Channel Set ***********************************************/
/********************************************************************************************/
#define CCSET_CHANNEL                           GC_DM3CCLIB_SET_MIN
#define CCPARM_CHANNEL_ID                       0x1
#define CCPARM_INTERFACE_ID                     0x2

/********************************************************************************************/
/** "New Channel" set - the parameters of this set are the same as those of Channel ID set **/
/********************************************************************************************/
#define CCSET_NEW_CHANNEL GC_DM3CCLIB_SET_MIN + 1

/********************************************************************************************/
/*************************** Call Rejection set *********************************************/
/********************************************************************************************/
#define CCSET_CALLREJECTION_INFO				GC_DM3CCLIB_SET_MIN + 2
#define CCPARM_CALLREJECTION_CAUSE				0x1
#define CCPARM_CALLREJECTION_CAUSE_ALIAS		0x2

/********************************************************************************************/
/**************************** Tsc_MsgDial Set ***********************************************/
/********************************************************************************************/
#define CCSET_DIAL								GC_DM3CCLIB_SET_MIN + 4
#define CCPARM_DIAL_STRING						0x1
#define CCPARM_DIAL_CALLPROGRESS				0x2

/********************************************************************************************/
/************************ Transparent Setparm to NetTsc board Set ***************************/
/********************************************************************************************/
#define CCSET_NETTSC_PARM						GC_DM3CCLIB_SET_MIN + 5

/********************************************************************************************/
/**************************** Enable Unsollicited GCEV_EXTENSION ****************************/
/********************************************************************************************/

#define CCSET_EXTENSIONEVT_MSK					GC_DM3CCLIB_SET_MIN + 6
/* The ParmID in this Set are GCACT_SETMSK, GCACT_ADDMSK and GCACT_SUBMSK */
/* Possible value buf */
#define EXTENSIONEVT_BCHAN_NEGOTIATED			(1 << CC_EXT_EVT_BCHAN_NEGOTIATED)	/* 0x2  */
#define EXTENSIONEVT_CALL_REJECTED				(1 << CC_EXT_EVT_CALL_REJECTED)		/* 0x4  */
#define EXTENSIONEVT_XFER						(1 << CC_EXT_EVT_XFER)				/* 0x10  */
#define EXTENSIONEVT_DIALING					(1 << CC_EXT_EVT_DIALING)			/* 0x20 */
#define EXTENSIONEVT_ORIGINATED					(1 << CC_EXT_EVT_CALL_ORIGINATED)	/* 0x40 */
#define EXTENSIONEVT_BIT_PATTERN				(1 << DM3CC_EXID_BIT_PATTERN)	/* 0x80 */
#define EXTENSIONEVT_ALL						0xFFFF

/********************************************************************************************/
/************************** Message Waiting Indication (MWI) Set ****************************/
/********************************************************************************************/
#define CCSET_MWI_MSGSET						GC_DM3CCLIB_SET_MIN + 7
#define CCPARM_MWI_ON							0x0
#define CCPARM_MWI_OFF							0x1

/********************************************************************************************/
/**************************** D Channel State Set *******************************************/
/********************************************************************************************/
#define DM3CC_SET_DLINK					GC_DM3CCLIB_SET_MIN + 8
#define DM3CC_PARM_DLINK_STATE		0x03	//the same as GCIS_PARM_DLINK_STATE in SW

/********************************************************************************************/
/**************************** CallerID on CallWaiting Set for Tsc_msgDial *******************/
/********************************************************************************************/
#define CCSET_CALLERID_CALLWAITING_DIAL			GC_DM3CCLIB_SET_MIN + 9
#define CCPARM_CALLERID_DT_STRING				0x1
#define CCPARM_CALLERID_NAME_STRING				0x2
#define CCPARM_CALLERID_NUM_STRING				0x3
#define CCPARM_CALLERID_NUMAR_STRING			0x4
#define CCPARM_CALLERID_NAMEAR_STRING			0x5
#define CCPARM_CALLERID_RAW_STRING				0x6

/********************************************************************************************/
/***************************** Volume Control for Stations **********************************/
/********************************************************************************************/
#define CCSET_VOLUME_CONTROL					GC_DM3CCLIB_SET_MIN + 10
#define CC_PARM_VOL_RES							0x1
#define CC_PARM_VOL_ADJ							0x2

/********************************************************************************************/
/*********************************** Distinctive ringing ************************************/
/********************************************************************************************/
#define CCSET_DISTINCTIVE_RING					GC_DM3CCLIB_SET_MIN + 11
#define CC_PARM_RINGID							0x1	

/********************************************************************************************/
/************************************ Call Analysis related parameters **********************/
/********************************************************************************************/
#define CCSET_CALLANALYSIS						GC_DM3CCLIB_SET_MIN + 12
#define CCPARM_CA_MODE							0x1
#define CCPARM_CA_PAMDSPDVAL					0x2
#define CCPARM_CA_HEDGE							0x3
#define CCPARM_CA_LOGLTCH						0x4
#define CCPARM_CA_HIGHLTCH						0x5
#define CCPARM_CA_ANSRDGL						0x6
#define CCPARM_CA_NOANSR						0x7
#define CCPARM_CA_NOSIG							0x8
#define CCPARM_CA_PAMDFAILURE					0x9
#define CCPARM_CA_PAMD_QTEMP					0xA
#define CCPARM_CA_PVD_QTEMP						0xB

// CCPARM_CA_MODE can take any value combinations of one of the following flags
#define GC_CA_BUSY 								0x1 	// Busy tone Detection (Pre-Connect)
#define GC_CA_RINGING							0x2 	// Ring back detection (Pre-Connect)
#define GC_CA_SIT								0x4 	// SIT tone detection (Pre-Connect)
#define GC_CA_FAX								0x100 	// Fax detection (Post-Connect)
#define GC_CA_PVD								0x200 	// PVD detection (Post-Connect)
#define GC_CA_PAMD								0x400 	// PAMD detection (Post-Connect)

// Global Call recommand using of the following combinations
#define GC_CA_DISABLE					0x0					// All disabled
#define GC_CA_PREONLY					(GC_CA_BUSY|GC_CA_RINGING)		// Busy and Ringing enabled (pre-connect only)
#define GC_CA_PREONLY_SIT				(GC_CA_BUSY|GC_CA_RINGING|GC_CA_SIT)	// All Pre-connect enabled (Busy, Ringing, Sit)
#define GC_CA_POSTONLY_PVD				(GC_CA_FAX|GC_CA_PVD)			// FAX + PVD (Post-connect only)
#define GC_CA_POSTONLY_PVD_PAMD			(GC_CA_FAX|GC_CA_PVD|GC_CA_PAMD)	// All Post-connect enabled
#define GC_CA_ENABLE_PVD				(GC_CA_BUSY|GC_CA_RINGING|GC_CA_SIT|GC_CA_FAX|GC_CA_PVD)		// All Pre-connect + FAX and PVD (Post-connect) enabled
#define GC_CA_ENABLE_ALL				(GC_CA_BUSY|GC_CA_RINGING|GC_CA_SIT|GC_CA_FAX|GC_CA_PVD|GC_CA_PAMD)	// All Pre-connect and Post-connect enabled

/********************************************************************************************/
/************************************ Direct Bit access *************************************/
/********************************************************************************************/
#define CCSET_BIT_PATTERN						GC_DM3CCLIB_SET_MIN + 13
#define CCPARM_INTPARM1							1
#define CCPARM_INTPARM2							2
#define CCPARM_CHARPARM1						4
#define CCPARM_CHARPARM2						5

/********************************************************************************************/
/********************* Transparent Setparm to DM3 Firmware instances ************************/
/********************************************************************************************/
#define CCSET_DM3FW_PARM						GC_DM3CCLIB_SET_MIN + 14

/* Set config parms at runtime on DM3 Analog TSC */
#define CCDM3FW_TSC_ANALOG_RUNTIMECONFIG									0x121e
#define CCDM3FW_TSC_ANALOG_DISABLE_TONE_DISCONNECT_SUPERVISION			0x00000001
#define CCDM3FW_TSC_ANALOG_ENABLE_TONE_DISCONNECT_SUPERVISION			0x80000001
#define CCDM3FW_TSC_ANALOG_DISABLE_DIAL_TONE_DETECTION					0x00000002
#define CCDM3FW_TSC_ANALOG_ENABLE_DIAL_TONE_DETECTION						0x80000002

/********************************************************************************************/
/************************ Configure Line/Protocol *************************************/
/********************************************************************************************/
#define CCSET_LINE_CONFIG						GC_DM3CCLIB_SET_MIN + 15
#define CCPARM_CRC								0x1601
#define CCPARM_USER_NETWORK						0x17

#endif	// dm3cc_parm.h
