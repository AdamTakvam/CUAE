/***********************************************************************
 *        FILE: dxdigit.h
 * DESCRIPTION: Header File for DIALOGIC Windows NT library
 *
 *   Copyright (c) 1991-1996 Dialogic Corp. All Rights Reserved
 *
 *   THIS IS UNPUBLISHED PROPRIETARY SOURCE CODE OF Dialogic Corp.
 *   The copyright notice above does not evidence any actual or
 *   intended publication of such source code.
 **********************************************************************/

#ifndef __DXDIGIT_H__
#define __DXDIGIT_H__

#ifdef __STANDALONE__            /* List of required header files */
#endif


/* 
 * dx_getdig( ) related defines
 */
#define LEN_DIGBUF   31          /* Max # of entries for collecting DTMF */
#define DG_MAXDIGS   LEN_DIGBUF  /* Max Digits Returned by dx_getdig() */
#define DG_END       0xff        /* Terminator for dg_type Array in DV_DIGIT */


/*
 * Defines for Digit Control Mask.
 */
#define D_DTMF       0x01        /* Enable DTMF detection */
#define D_LPD        0x02        /* Enable Loop pulse detection */
#define D_APD        0x04        /* Enable Audio pulse detection */
#define D_MF         0x08        /* Enable MF digit detection */
#define D_DPD        0x10        /* Enable DPD detection */
#define D_DPDZ       0x20        /* Enable Zero-Trained DPD Detection */

/*
 * Ascii digit type returned in DV_DIGIT structure.
 */
#define DG_DTMF_ASCII   0x0030   /* DTMF Digit */
#define DG_LPD_ASCII    0x0031   /* Loop Pulse Digit  */
#define DG_APD_ASCII    0x0032   /* Audio Pulse Digit */
#define DG_DPD_ASCII    0x0032   /* Dial Pulse Digit */
#define DG_MF_ASCII     0x0033   /* MF Digit */


/* 
 * DV_DIGIT structure
 */
typedef struct dv_digit {
   char dg_value[DG_MAXDIGS +1];
   char dg_type[DG_MAXDIGS +1];
} DV_DIGIT;


#endif

