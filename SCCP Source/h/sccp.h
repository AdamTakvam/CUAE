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
 *     sccp.h
 *
 *  CREATION/MODIFICATION DATES
 *     Created:  November 2002, Sam Hague
 *     Modified: 
 *
 *  DESCRIPTION
 *     Skinny Client Control Protocol header file
 */
#ifndef _SCCP_H_
#define _SCCP_H_

#include "sccp_platform.h"
#include "am.h"
#include "sem.h"
#include "sccpmsg.h"
#include "gapi.h"
#include "sccpcc.h"
#include "sccpcm.h"
#include "sccprec.h"
#include "sccpreg.h"


#define SCCP_UNDEFINED      "UNDEFINED"
#define SCCP_ENDIAN_BIG     0
#define SCCP_ENDIAN_LITTLE  1
#define SCCP_APPNAME_SIZE   32

typedef enum sccp_sems_t_ {
    SCCP_SEM_MIN = -1,
    SCCP_SEM_CC,
    SCCP_SEM_CM,
    SCCP_SEM_REC,
    SCCP_SEM_REG,
    SCCP_SEM_MAX
} sccp_sems_t;

typedef struct sccp_sccpcb_t_ {
    struct sccp_sccpcb_t_ *next;
    int               id;    
    void              *apphandle;
    char              appname[32 + 1];
    gapi_callbacks_t  *appcbs;
    sem_event_t       *events;
    sccpcc_cccb_t     *cccbs; /* call control control blocks */
    sccpcm_cmcb_t     *cmcbs; /* callmanager control blocks */
    sccpreg_regcb_t   *regcb; /* register control block */
    sccprec_reccb_t   *reccb; /* recovery control block */
    void              *event_run;
    int               version;
    int               endian;
    int               event_id;
    int               cmcb_id;
} sccp_sccpcb_t;

int sccp_push_event(sccp_sccpcb_t *sccpcb, void *data, int copy_len,
                    int event_id, sem_table_t *table, void *cb, int front);
int sccp_event_main(void *handle, void *data);
int sccp_tcp_main(void *handle, int msg_id, int socket,
                  gapi_tcp_events_e event, void *data,
                  int size);
char *sccp_get_sem_name(unsigned long id);
void sccp_free_sccpcb(sccp_sccpcb_t *sccpcb);
void *sccp_get_new_sccpcb();
int sccp_init(void);
int sccp_cleanup(void);
gapi_callbacks_t *sccp_get_callbacks(sccp_sccpcb_t *sccpcb);
void sccp_set_version(sccp_sccpcb_t *sccpcb, int version);
//sccp_sccpcb_t *sccp_get_sccpcb(void);

//#define SCCP_USE_POOL
#ifdef SCCP_USE_POOL
#ifdef SCCP_USE_POOL_STATS
void *sccp_pool_malloc(int type, unsigned int size, char *fname, int line);
void sccp_pool_free(int type, void *ptr, char *fname, int line);
#else
void *sccp_pool_malloc(int type, unsigned int size);
void sccp_pool_free(int type, void *ptr);
#endif /* SCCP_USE_POOL_STATS */
#endif /* SCCP_USE_POOL */

#if 1
int sccp_debug_show(sem_table_t *table, int msg_id);
#endif
#endif /* _SCCP_H_ */
