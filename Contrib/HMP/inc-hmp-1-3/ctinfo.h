/**********************************************************************
 *  Copyright (C) 2002 Intel Corporation
 *  All Rights Reserved
 *
 *  THIS IS UNPUBLISHED PROPRIETARY SOURCE CODE OF Intel Corp.
 *  The copyright notice above does not evidence any actual or
 *  intended publication of such source code.
 **********************************************************************/
/**********************************************************************
*
*	C Header:		ctinfo.h
*	Description:	contains CT device information. 
*	%date_created:	Tue Feb 25 15:50:43 2003 %
*
**********************************************************************/

#ifndef _hsw_1_ctinfo_h_H
#define _hsw_1_ctinfo_h_H

#ifndef lint
static
#ifdef __cplusplus          
const /* C++ needs const */     
#endif
char    *_hsw_1_ctinfo_h = "@(#) %filespec: ctinfo.h-dte#2 %  (%full_filespec: ctinfo.h-dte#2:incl:hsw#1 %)";
#endif

/* Type definition for Channel/Timeslot device info structure and its values*/
#ifndef __CT_DEVINFO__
#define __CT_DEVINFO__

/* CT_DEVINFO structure*/
typedef struct ct_devinfo
{
   unsigned long  ct_prodid;		/* product id*/
   unsigned char  ct_devfamily;		/* dev family*/
   unsigned char  ct_devmode;		/* dev mode*/
   unsigned char  ct_nettype;		/* network interface*/
   unsigned char  ct_busmode;		/* bus architecture*/
   unsigned char  ct_busencoding;	/* bus encoding*/
   union { 
    		unsigned char ct_RFU[7];	/* reserved*/
			struct {
				unsigned char ct_prottype;
         	} ct_net_devinfo;
   } ct_ext_devinfo;

} CT_DEVINFO;

#define ct_rfu  ct_ext_devinfo.ct_RFU

/****** device family ******/
#define CT_DFD41D			0x01
#define CT_DFD41E			0x02
#define CT_DFSPAN			0x03
#define CT_DFMSI			0x04
#define CT_DFDM3			0x05
#define CT_DFSCX			0x06
#define CT_DFNETSTRUCTIP	0x07
#define CT_DFSSP			0x08

/****** device mode ******/
#define CT_DMNETWORK	0x00	/* flexible routing*/
#define CT_DMRESOURCE	0x01	/* fixed routing | network dev*/

/****** network type ******/
#define CT_NTNONE			0x00	/* resource only config*/
#define CT_NTT1				0x01	/* T1 digital*/
#define CT_NTE1				0x02	/* E1 digital*/
#define CT_NTANALOG			0x03	/* analog*/
#define CT_NTMSI			0x04	/* MSI/SC station*/
#define CT_NTIPT			0x05	/* IPT*/
#define CT_NTHIZ            0x80	/* not documented in dti guide*/
#define CT_NTTXZSWITCHABLE  0x40	/* not documented in dti guide*/

/****** bus mode ******/
#define CT_BMPEB		0x00
#define CT_BMSCBUS		0x01
#define CT_BMVIRTUAL	0x05
#define CT_BMH100		0x06
#define CT_BMH110		0x07

/****** bus encoding ******/
#define CT_BEULAW   0x00
#define CT_BEALAW   0x01
#define CT_BELLAW	0x02	/* not documented*/
#define CT_BEBYPASS	0x03	/* not documented*/

/****** protocol type ******/
#define CT_ISDN     0x01
#define CT_CAS      0x02
#define CT_R2MF     0x03
#define CT_CLEAR    0x04

#endif // __CT_DEVINFO__

#endif
