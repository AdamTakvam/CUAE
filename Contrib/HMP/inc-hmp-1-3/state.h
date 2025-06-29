/***********************************************************************
 *        FILE: state.h
 * DESCRIPTION: Header File for DIALOGIC Windows NT library
 *
 *   Copyright (c) 1991-1996 Dialogic Corp. All Rights Reserved
 *
 *   THIS IS UNPUBLISHED PROPRIETARY SOURCE CODE OF Dialogic Corp.
 *   The copyright notice above does not evidence any actual or
 *   intended publication of such source code.
 **********************************************************************/

#ifndef __STATE_H__
#define __STATE_H__

/*
 ******** Message passing error codes
 */
#define DT_IDLE               0x01    /* Station is IDLE */
#define DT_RING               0x02    /* Station is Ringing */
#define DT_WAITCALL           0x04    /* Station is waiting for a call */
#define DT_WAITEVT            0x08    /* Station is waiting on an event */
#define DT_WTEVTORCALL        0x10    /* Waiting for a call or an event */
#define DT_BLOCKED            0x20    /* Station is Blocked */
#endif
