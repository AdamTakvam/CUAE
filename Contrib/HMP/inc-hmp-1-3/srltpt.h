/***********************************************************************
 *        FILE: srltpt.h
 * DESCRIPTION: Header File for DIALOGIC Windows NT library
 *
 *   Copyright (c) Dialogic Corp. All Rights Reserved
 *
 *   THIS IS UNPUBLISHED PROPRIETARY SOURCE CODE OF Dialogic Corp.
 *   The copyright notice above does not evidence any actual or
 *   intended publication of such source code.
 ***********************************************************************/


#ifndef __SRLTPT_H__
#define __SRLTPT_H__


/*
 * Termination Parameter Types
 */
#define IO_CONT   0x01        /* Next TPT is contiguous in memory  */
#define IO_LINK   0x02        /* Next TPT found thru tp_nextp ptr */
#define IO_EOT    0x04        /* End of the Termination Parameters */


/*
 * DV_TPT - Termination Parameter Table Structure.
 */
typedef struct dv_tpt DV_TPT;

struct dv_tpt {
   unsigned short tp_type;    /* Flags Describing this Entry */
   unsigned short tp_termno;  /* Termination Parameter Number */
   unsigned short tp_length;  /* Length of Terminator */
   unsigned short tp_flags;   /* Termination Parameter Attributes Flag */
   unsigned short tp_data;    /* Optional Additional Data */
   unsigned short rfu;        /* Reserved */
   DV_TPT        *tp_nextp;   /* Ptr to next DV_TPT if IO_LINK set */
};

#endif

