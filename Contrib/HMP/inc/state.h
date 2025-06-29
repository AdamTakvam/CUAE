/*****************************************************************************
 * Copyright © 1991-1996, Intel Corporation. All rights reserved.  Intel is a 
 * trademark or registered trademark of Intel Corporation or its subsidiaries 
 * in the United States and other countries. Other names and brands may be 
 * claimed as the property of others. 
 *****************************************************************************/
 /***********************************************************************
 *        FILE: state.h
 * DESCRIPTION: Header File for DIALOGIC Windows NT library
 *
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
