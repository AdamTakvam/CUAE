/*****************************************************************************
 * Copyright © 1990-2002, Intel Corporation. All rights reserved.  Intel is a 
 * trademark or registered trademark of Intel Corporation or its subsidiaries 
 * in the United States and other countries. Other names and brands may be 
 * claimed as the property of others. 
 *****************************************************************************/

/*****************************************************************************
 * Filename:    srl_eblk.h 
 * DESCRIPTION: Header File for INTEL Windows NT library.   
 *****************************************************************************/
 
#ifndef __SRL_EBLK_H__
#define __SRL_EBLK_H__


/*
 * EV_EBLK
 *
 * Event Block Structure - if 8 bytes or less of event-specific data, it
 * is stored in ev_data and ev_datap is NULL.  Otherwise ev_datap points to
 * malloc'd memory and ev_data is unused.  In either case, ev_len specifies
 * the number of bytes of event-specific data present.
 */
typedef struct ev_eblk {
   long           ev_dev;     /* Device on which event occurred    */
   unsigned long  ev_event;   /* Event that occured                */
   long           ev_len;     /* # of bytes of event-specific data */
   unsigned char  ev_data[8]; /* Event-specific data               */
   void          *ev_datap;   /* Event-specific data pointer       */
   void          *ev_UserContextp;    /* Ptr to user information Used for New Conferencing Library */
} EV_EBLK;


#endif

