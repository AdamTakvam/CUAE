/*
 *  Copyright (c) 2002, 2003, 2004 by Cisco Systems, Inc. All Rights Reserved.
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
 *     sccpcc.c
 *
 *  CREATION/MODIFICATION DATES
 *     Created:  November 2002, Sam Hague
 *     Modified: 
 *
 *  DESCRIPTION
 *     SCCP Call Control implementation
 */
#include "sccp.h"
#include "sccp_debug.h"
#include "sem.h"
#include "am.h"
#include "sllist.h"
#include "sccpmsg.h"
#include "gapi.h"


#define SCCPCC_MAX_CALLS 6

/*
 * SCCPCC_JUST_ACK_FEATURE is used to always ack a FEATURE_REQ
 * regardless of the actual status of the request. Currently, there is
 * no feedback from the CCM for feature requests, but GAPI requires an ack
 * for all feature requests.
 */
#define SCCPCC_JUST_ACK_FEATURE 1


static sccpmsg_msgcb_t sccpcc_msgcb;


typedef enum sccpcc_states_t_ {
    SCCPCC_S_MIN = -1,
    SCCPCC_S_IDLE,
    SCCPCC_S_CALL_INITIATED,
    SCCPCC_S_OUTGOING_PROCEEDING,
    SCCPCC_S_OUTGOING_ALERTING,
    SCCPCC_S_INCOMING_ALERTING,
    SCCPCC_S_INCOMING_CONNECTING,
    SCCPCC_S_CONNECTED,
    SCCPCC_S_OUTGOING_RELEASING,
    SCCPCC_S_INCOMING_RELEASING,
    SCCPCC_S_MAX
} sccpcc_states_t;

static char *sccpcc_state_names[] = {
    "S_IDLE",
    "S_CALL_INITIATED",
    "S_OUTGOING_PROCEEDING",
    "S_OUTGOING_ALERTING",
    "S_INCOMING_ALERTING",
    "S_INCOMING_CONNECTING",
    "S_CONNECTED",
    "S_OUTGOING_RELEASING",
    "S_INCOMING_RELEASING",
};

static char *sccpcc_event_names[] = {
    "E_SETUP",
    "E_SETUP_ACK",
    "E_PROCEEDING",
    "E_ALERTING",
    "E_CONNECT",
    "E_CONNECT_ACK",
    "E_DISCONNECT",
    "E_RELEASE",
    "E_RELEASE_COMPLETE",
    "E_OFFHOOK",
    "E_ONHOOK",
    "E_DIGITS",
    "E_FEATURE_REQ",
    "E_CALL_STATE",
    "E_CS_OFFHOOK",
    "E_CS_SETUP",
    "E_CS_PROCEEDING",
    "E_CS_ALERTING",
    "E_CS_CONNECTED",
    "E_CS_RELEASE",
    "E_UPDATE_UI",
    "E_MEDIA",
    "E_OPENRCV_RES",
    "E_TIMER",
    "E_CLEANUP",
    "E_DEVTOUSERDATA_REQ",
    "E_DEVTOUSERDATA_RES",
    "E_PASSTHRU"
};


static char *sccpcc_sem_name(int id);
static char *sccpcc_state_name(int id);
static char *sccpcc_event_name(int id);
static char *sccpcc_function_name(int id);
static void sccpcc_debug_sem_entry(sem_event_t *event);
static int  sccpcc_validate_cb(void *cb1, void *cb2);
static void sccpcc_change_state(sccpcc_cccb_t *cccb, int new_state,
                                char *fname);

static int sccpcc_sem_default(sem_event_t *event);
static int sccpcc_sem_setup(sem_event_t *event);
static int sccpcc_sem_connect(sem_event_t *event);
//static int sccpcc_sem_connect_ack(sem_event_t *event);
static int sccpcc_sem_release(sem_event_t *event);
static int sccpcc_sem_release_complete(sem_event_t *event);
static int sccpcc_sem_offhook(sem_event_t *event);
static int sccpcc_sem_digits(sem_event_t *event);
static int sccpcc_sem_update_ui(sem_event_t *event);
static int sccpcc_sem_call_state(sem_event_t *event);
static int sccpcc_sem_media(sem_event_t *event);
static int sccpcc_sem_openrcv_res(sem_event_t *event);
static int sccpcc_sem_idle_feature_req(sem_event_t *event);
static int sccpcc_sem_feature_req(sem_event_t *event);
static int sccpcc_sem_xxx_offhook(sem_event_t *event);
static int sccpcc_sem_cs_offhook(sem_event_t *event);
static int sccpcc_sem_cs_setup(sem_event_t *event);
static int sccpcc_sem_cs_proceeding(sem_event_t *event);
static int sccpcc_sem_cs_alerting(sem_event_t *event);
static int sccpcc_sem_oa_cs_connected(sem_event_t *event);
static int sccpcc_sem_ic_cs_connected(sem_event_t *event);
static int sccpcc_sem_ia_release_complete(sem_event_t *event);
static int sccpcc_sem_or_release(sem_event_t *event);
static int sccpcc_sem_cs_release(sem_event_t *event);
static int sccpcc_sem_cleanup(sem_event_t *event);
static int sccpcc_sem_devicetouserdata_req(sem_event_t *event);
static int sccpcc_sem_devicetouserdata_res(sem_event_t *event);
static int sccpcc_sem_passthru(sem_event_t *event);

typedef enum sccpcc_sem_functions_t_ {
    SCCPCC_SEM_MIN = -1,
    SCCPCC_SEM_DEFAULT,    
    SCCPCC_SEM_SETUP,    
    SCCPCC_SEM_CONNECT,    
//    SCCPCC_SEM_CONNECT_ACK,    
    SCCPCC_SEM_RELEASE,    
    SCCPCC_SEM_RELEASE_COMPLETE,    
    SCCPCC_SEM_OFFHOOK,    
    SCCPCC_SEM_DIGITS,    
    SCCPCC_SEM_UPDATE_UI,    
    SCCPCC_SEM_CALL_STATE,    
    SCCPCC_SEM_MEDIA,
    SCCPCC_SEM_OPENRCV_RES,
    SCCPCC_SEM_IDLE_FEATURE_REQ,
    SCCPCC_SEM_FEATURE_REQ,
    SCCPCC_SEM_XXX_OFFHOOK,
    SCCPCC_SEM_CS_OFFHOOK,
    SCCPCC_SEM_CS_SETUP,
    SCCPCC_SEM_CS_PROCEEDING,
    SCCPCC_SEM_CS_ALERTING,
    SCCPCC_SEM_OA_CS_CONNECTED,
    SCCPCC_SEM_IC_CS_CONNECTED,
    SCCPCC_SEM_IA_RELEASE_COMPLETE,
    SCCPCC_SEM_OR_RELEASE,
    SCCPCC_SEM_CS_RELEASE,
    SCCPCC_SEM_CLEANUP,
    SCCPCC_SEM_DEVTOUSERDATA_REQ,
    SCCPCC_SEM_DEVTOUSERDATA_RES,    
    SCCPCC_SEM_MAX        
} sccpcc_sem_functions_t;

static sem_function_t sccpcc_sem_functions[] = 
{
    sccpcc_sem_default,
    sccpcc_sem_setup,
    sccpcc_sem_connect,
//    sccpcc_sem_connect_ack,
    sccpcc_sem_release,
    sccpcc_sem_release_complete,
    sccpcc_sem_offhook,
    sccpcc_sem_digits,
    sccpcc_sem_update_ui,

    sccpcc_sem_call_state,
    sccpcc_sem_media,
    sccpcc_sem_openrcv_res,
    sccpcc_sem_idle_feature_req,
    sccpcc_sem_feature_req,    
    sccpcc_sem_xxx_offhook,

    sccpcc_sem_cs_offhook,
    sccpcc_sem_cs_setup,
    sccpcc_sem_cs_proceeding,
    sccpcc_sem_cs_alerting,
    sccpcc_sem_oa_cs_connected,
    sccpcc_sem_ic_cs_connected,
    sccpcc_sem_ia_release_complete,
    sccpcc_sem_or_release,
    sccpcc_sem_cs_release,
    sccpcc_sem_cleanup,
    sccpcc_sem_devicetouserdata_req,
    sccpcc_sem_devicetouserdata_res
};

static char *sccpcc_sem_function_names[] = 
{
    "sem_default",
    "sem_setup",
    "sem_connect",
//    "sem_connect_ack",
    "sem_release",
    "sem_release_complete",
    "sem_offhook",
    "sem_digits",     
    "sem_update_ui",
    "sem_call_state",
    "sem_media",
    "sem_openrcv_res",
    "sem_idle_feature_req",
    "sem_feature_req",
    "sem_xxx_offhook",
    "sem_cs_offhook",
    "sem_cs_setup",
    "sem_cs_proceeding",
    "sem_cs_alerting",
    "sem_oa_cs_connected",
    "sem_ic_cs_connected",
    "sem_ia_release_complete",
    "sem_or_release",
    "sem_cs_release",
    "sem_cleanup",
    "sem_devicetouserdata_req",
    "sem_devicetouserdata_res"
};

static sem_events_t sccpcc_sem_s_idle[] =
{
    {SCCPCC_E_SETUP,                SCCPCC_SEM_SETUP,                   SCCPCC_S_CALL_INITIATED},
    {SCCPCC_E_OFFHOOK,              SCCPCC_SEM_OFFHOOK,                 SCCPCC_S_CALL_INITIATED},
    {SCCPCC_E_FEATURE_REQ,          SCCPCC_SEM_IDLE_FEATURE_REQ,        SCCPCC_S_CALL_INITIATED},
    {SCCPCC_E_CALL_STATE,           SCCPCC_SEM_CALL_STATE,              SCCPCC_S_IDLE},
    {SCCPCC_E_CS_OFFHOOK,           SCCPCC_SEM_CS_OFFHOOK,              SCCPCC_S_CALL_INITIATED},
    {SCCPCC_E_CS_SETUP,             SCCPCC_SEM_CS_SETUP,                SCCPCC_S_INCOMING_ALERTING},
    {SCCPCC_E_UPDATE_UI,            SCCPCC_SEM_UPDATE_UI,               SCCPCC_S_IDLE},
    {SCCPCC_E_MEDIA,                SCCPCC_SEM_MEDIA,                   SCCPCC_S_IDLE},
    {SCCPCC_E_OPENRCV_RES,          SCCPCC_SEM_OPENRCV_RES,             SCCPCC_S_IDLE},
    {SCCPCC_E_CLEANUP,              SCCPCC_SEM_CLEANUP,                 SCCPCC_S_IDLE},
    {SCCPCC_E_DEVTOUSERDATA_REQ,    SCCPCC_SEM_DEVTOUSERDATA_REQ,       SCCPCC_S_IDLE},
    {SCCPCC_E_DEVTOUSERDATA_RES,    SCCPCC_SEM_DEVTOUSERDATA_RES,       SCCPCC_S_IDLE},
};

static sem_events_t sccpcc_sem_s_call_initiated[] =
{
    {SCCPCC_E_RELEASE,              SCCPCC_SEM_RELEASE,                 SCCPCC_S_OUTGOING_RELEASING},
    {SCCPCC_E_RELEASE_COMPLETE,     SCCPCC_SEM_CLEANUP,                 SCCPCC_S_IDLE},
    {SCCPCC_E_ONHOOK,               SCCPCC_SEM_RELEASE,                 SCCPCC_S_OUTGOING_RELEASING},
    {SCCPCC_E_DIGITS,               SCCPCC_SEM_DIGITS,                  SCCPCC_S_CALL_INITIATED},
    {SCCPCC_E_FEATURE_REQ,          SCCPCC_SEM_FEATURE_REQ,             SCCPCC_S_CALL_INITIATED},
    {SCCPCC_E_CALL_STATE,           SCCPCC_SEM_CALL_STATE,              SCCPCC_S_CALL_INITIATED},
    {SCCPCC_E_CS_PROCEEDING,        SCCPCC_SEM_CS_PROCEEDING,           SCCPCC_S_OUTGOING_PROCEEDING},
    {SCCPCC_E_CS_ALERTING,          SCCPCC_SEM_CS_ALERTING,             SCCPCC_S_OUTGOING_ALERTING},
    {SCCPCC_E_CS_CONNECTED,         SCCPCC_SEM_OA_CS_CONNECTED,         SCCPCC_S_CONNECTED},
    {SCCPCC_E_UPDATE_UI,            SCCPCC_SEM_UPDATE_UI,               SCCPCC_S_CALL_INITIATED},
    {SCCPCC_E_MEDIA,                SCCPCC_SEM_MEDIA,                   SCCPCC_S_CALL_INITIATED},
    {SCCPCC_E_OPENRCV_RES,          SCCPCC_SEM_OPENRCV_RES,             SCCPCC_S_CALL_INITIATED},
    {SCCPCC_E_CLEANUP,              SCCPCC_SEM_CLEANUP,                 SCCPCC_S_IDLE},
    {SCCPCC_E_DEVTOUSERDATA_REQ,    SCCPCC_SEM_DEVTOUSERDATA_REQ,       SCCPCC_S_CALL_INITIATED},
    {SCCPCC_E_DEVTOUSERDATA_RES,    SCCPCC_SEM_DEVTOUSERDATA_RES,       SCCPCC_S_CALL_INITIATED}
};

static sem_events_t sccpcc_sem_s_outgoing_proceeding[] =
{
    {SCCPCC_E_RELEASE,              SCCPCC_SEM_OR_RELEASE,              SCCPCC_S_IDLE},
    {SCCPCC_E_ONHOOK,               SCCPCC_SEM_OR_RELEASE,              SCCPCC_S_IDLE},
    {SCCPCC_E_FEATURE_REQ,          SCCPCC_SEM_FEATURE_REQ,             SCCPCC_S_OUTGOING_PROCEEDING},    
    {SCCPCC_E_CALL_STATE,           SCCPCC_SEM_CALL_STATE,              SCCPCC_S_OUTGOING_PROCEEDING},
    {SCCPCC_E_CS_ALERTING,          SCCPCC_SEM_CS_ALERTING,             SCCPCC_S_OUTGOING_ALERTING},
    {SCCPCC_E_CS_CONNECTED,         SCCPCC_SEM_OA_CS_CONNECTED,         SCCPCC_S_CONNECTED},
    {SCCPCC_E_CS_RELEASE,           SCCPCC_SEM_CS_RELEASE,              SCCPCC_S_INCOMING_RELEASING},
    {SCCPCC_E_UPDATE_UI,            SCCPCC_SEM_UPDATE_UI,               SCCPCC_S_OUTGOING_PROCEEDING},
    {SCCPCC_E_MEDIA,                SCCPCC_SEM_MEDIA,                   SCCPCC_S_OUTGOING_PROCEEDING},
    {SCCPCC_E_OPENRCV_RES,          SCCPCC_SEM_OPENRCV_RES,             SCCPCC_S_OUTGOING_PROCEEDING},
    {SCCPCC_E_CLEANUP,              SCCPCC_SEM_CLEANUP,                 SCCPCC_S_IDLE},
    {SCCPCC_E_DEVTOUSERDATA_REQ,    SCCPCC_SEM_DEVTOUSERDATA_REQ,       SCCPCC_S_OUTGOING_PROCEEDING},
    {SCCPCC_E_DEVTOUSERDATA_RES,    SCCPCC_SEM_DEVTOUSERDATA_RES,       SCCPCC_S_OUTGOING_PROCEEDING}
};

static sem_events_t sccpcc_sem_s_outgoing_alerting[] =
{
    {SCCPCC_E_RELEASE,              SCCPCC_SEM_RELEASE,                 SCCPCC_S_OUTGOING_RELEASING},
    {SCCPCC_E_ONHOOK,               SCCPCC_SEM_RELEASE,                 SCCPCC_S_OUTGOING_RELEASING},
    {SCCPCC_E_FEATURE_REQ,          SCCPCC_SEM_FEATURE_REQ,             SCCPCC_S_OUTGOING_ALERTING},
    {SCCPCC_E_CALL_STATE,           SCCPCC_SEM_CALL_STATE,              SCCPCC_S_OUTGOING_ALERTING},
    {SCCPCC_E_CS_CONNECTED,         SCCPCC_SEM_OA_CS_CONNECTED,         SCCPCC_S_CONNECTED},
    {SCCPCC_E_CS_RELEASE,           SCCPCC_SEM_CS_RELEASE,              SCCPCC_S_INCOMING_RELEASING},
    {SCCPCC_E_UPDATE_UI,            SCCPCC_SEM_UPDATE_UI,               SCCPCC_S_OUTGOING_ALERTING},
    {SCCPCC_E_MEDIA,                SCCPCC_SEM_MEDIA,                   SCCPCC_S_OUTGOING_ALERTING},
    {SCCPCC_E_OPENRCV_RES,          SCCPCC_SEM_OPENRCV_RES,             SCCPCC_S_OUTGOING_ALERTING},
    {SCCPCC_E_CLEANUP,              SCCPCC_SEM_CLEANUP,                 SCCPCC_S_IDLE},
    {SCCPCC_E_DEVTOUSERDATA_REQ,    SCCPCC_SEM_DEVTOUSERDATA_REQ,       SCCPCC_S_OUTGOING_ALERTING},
    {SCCPCC_E_DEVTOUSERDATA_RES,    SCCPCC_SEM_DEVTOUSERDATA_RES,       SCCPCC_S_OUTGOING_ALERTING}
};    

static sem_events_t sccpcc_sem_s_incoming_alerting[] =
{
    {SCCPCC_E_RELEASE,              SCCPCC_SEM_RELEASE,                 SCCPCC_S_OUTGOING_RELEASING},
    {SCCPCC_E_RELEASE_COMPLETE,     SCCPCC_SEM_IA_RELEASE_COMPLETE,     SCCPCC_S_INCOMING_ALERTING},
    {SCCPCC_E_OFFHOOK,              SCCPCC_SEM_CONNECT,                 SCCPCC_S_INCOMING_CONNECTING},
    {SCCPCC_E_CONNECT,              SCCPCC_SEM_CONNECT,                 SCCPCC_S_INCOMING_CONNECTING},
    {SCCPCC_E_FEATURE_REQ,          SCCPCC_SEM_FEATURE_REQ,             SCCPCC_S_INCOMING_ALERTING},
    {SCCPCC_E_CALL_STATE,           SCCPCC_SEM_CALL_STATE,              SCCPCC_S_INCOMING_ALERTING},
    {SCCPCC_E_CS_RELEASE,           SCCPCC_SEM_CS_RELEASE,              SCCPCC_S_INCOMING_RELEASING},
    {SCCPCC_E_UPDATE_UI,            SCCPCC_SEM_UPDATE_UI,               SCCPCC_S_INCOMING_ALERTING},
    {SCCPCC_E_MEDIA,                SCCPCC_SEM_MEDIA,                   SCCPCC_S_INCOMING_ALERTING},
    {SCCPCC_E_CLEANUP,              SCCPCC_SEM_CLEANUP,                 SCCPCC_S_IDLE},
    {SCCPCC_E_DEVTOUSERDATA_REQ,    SCCPCC_SEM_DEVTOUSERDATA_REQ,       SCCPCC_S_INCOMING_ALERTING},
    {SCCPCC_E_DEVTOUSERDATA_RES,    SCCPCC_SEM_DEVTOUSERDATA_RES,       SCCPCC_S_INCOMING_ALERTING}
};    

static sem_events_t sccpcc_sem_s_incoming_connecting[] =
{
    {SCCPCC_E_RELEASE,              SCCPCC_SEM_RELEASE,                 SCCPCC_S_OUTGOING_RELEASING},
    {SCCPCC_E_ONHOOK,               SCCPCC_SEM_RELEASE,                 SCCPCC_S_OUTGOING_RELEASING},
    {SCCPCC_E_OFFHOOK,              SCCPCC_SEM_XXX_OFFHOOK,             SCCPCC_S_INCOMING_CONNECTING},
    {SCCPCC_E_FEATURE_REQ,          SCCPCC_SEM_FEATURE_REQ,             SCCPCC_S_INCOMING_CONNECTING},
    {SCCPCC_E_CALL_STATE,           SCCPCC_SEM_CALL_STATE,              SCCPCC_S_INCOMING_CONNECTING},
    {SCCPCC_E_CS_CONNECTED,         SCCPCC_SEM_IC_CS_CONNECTED,         SCCPCC_S_CONNECTED},
    {SCCPCC_E_CS_RELEASE,           SCCPCC_SEM_CS_RELEASE,              SCCPCC_S_INCOMING_RELEASING},
    {SCCPCC_E_UPDATE_UI,            SCCPCC_SEM_UPDATE_UI,               SCCPCC_S_INCOMING_CONNECTING},
    {SCCPCC_E_MEDIA,                SCCPCC_SEM_MEDIA,                   SCCPCC_S_INCOMING_CONNECTING},
    {SCCPCC_E_OPENRCV_RES,          SCCPCC_SEM_OPENRCV_RES,             SCCPCC_S_INCOMING_CONNECTING},
    {SCCPCC_E_CLEANUP,              SCCPCC_SEM_CLEANUP,                 SCCPCC_S_IDLE},
    {SCCPCC_E_DEVTOUSERDATA_REQ,    SCCPCC_SEM_DEVTOUSERDATA_REQ,       SCCPCC_S_INCOMING_CONNECTING},
    {SCCPCC_E_DEVTOUSERDATA_RES,    SCCPCC_SEM_DEVTOUSERDATA_RES,       SCCPCC_S_INCOMING_CONNECTING}
};

static sem_events_t sccpcc_sem_s_connected[] =
{
    {SCCPCC_E_RELEASE,              SCCPCC_SEM_RELEASE,                 SCCPCC_S_OUTGOING_RELEASING},
    {SCCPCC_E_ONHOOK,               SCCPCC_SEM_RELEASE,                 SCCPCC_S_OUTGOING_RELEASING},
    {SCCPCC_E_DIGITS,               SCCPCC_SEM_DIGITS,                  SCCPCC_S_CONNECTED},
    {SCCPCC_E_FEATURE_REQ,          SCCPCC_SEM_FEATURE_REQ,             SCCPCC_S_CONNECTED},
    {SCCPCC_E_CALL_STATE,           SCCPCC_SEM_CALL_STATE,              SCCPCC_S_CONNECTED},
    {SCCPCC_E_CS_RELEASE,           SCCPCC_SEM_CS_RELEASE,              SCCPCC_S_INCOMING_RELEASING},
    {SCCPCC_E_UPDATE_UI,            SCCPCC_SEM_UPDATE_UI,               SCCPCC_S_CONNECTED},
    {SCCPCC_E_MEDIA,                SCCPCC_SEM_MEDIA,                   SCCPCC_S_CONNECTED},
    {SCCPCC_E_OPENRCV_RES,          SCCPCC_SEM_OPENRCV_RES,             SCCPCC_S_CONNECTED},
    {SCCPCC_E_CLEANUP,              SCCPCC_SEM_CLEANUP,                 SCCPCC_S_IDLE},
    {SCCPCC_E_DEVTOUSERDATA_REQ,    SCCPCC_SEM_DEVTOUSERDATA_REQ,       SCCPCC_S_CONNECTED},
    {SCCPCC_E_DEVTOUSERDATA_RES,    SCCPCC_SEM_DEVTOUSERDATA_RES,       SCCPCC_S_CONNECTED}
};    

static sem_events_t sccpcc_sem_s_outgoing_releasing[] =
{
    {SCCPCC_E_RELEASE,              SCCPCC_SEM_OR_RELEASE,              SCCPCC_S_OUTGOING_RELEASING},
    {SCCPCC_E_ONHOOK,               SCCPCC_SEM_OR_RELEASE,              SCCPCC_S_OUTGOING_RELEASING},
    {SCCPCC_E_FEATURE_REQ,          SCCPCC_SEM_FEATURE_REQ,             SCCPCC_S_OUTGOING_RELEASING},
    {SCCPCC_E_CALL_STATE,           SCCPCC_SEM_CALL_STATE,              SCCPCC_S_OUTGOING_RELEASING},
    {SCCPCC_E_UPDATE_UI,            SCCPCC_SEM_UPDATE_UI,               SCCPCC_S_OUTGOING_RELEASING},
    {SCCPCC_E_MEDIA,                SCCPCC_SEM_MEDIA,                   SCCPCC_S_OUTGOING_RELEASING},
    {SCCPCC_E_OPENRCV_RES,          SCCPCC_SEM_OPENRCV_RES,             SCCPCC_S_OUTGOING_RELEASING},
    {SCCPCC_E_CLEANUP,              SCCPCC_SEM_CLEANUP,                 SCCPCC_S_IDLE},
    {SCCPCC_E_DEVTOUSERDATA_REQ,    SCCPCC_SEM_DEVTOUSERDATA_REQ,       SCCPCC_S_OUTGOING_RELEASING},
    {SCCPCC_E_DEVTOUSERDATA_RES,    SCCPCC_SEM_DEVTOUSERDATA_RES,       SCCPCC_S_OUTGOING_RELEASING}
};        
    
static sem_events_t sccpcc_sem_s_incoming_releasing[] =
{
    {SCCPCC_E_RELEASE,              SCCPCC_SEM_CLEANUP,                 SCCPCC_S_IDLE},
    {SCCPCC_E_ONHOOK,               SCCPCC_SEM_CLEANUP,                 SCCPCC_S_IDLE},
    {SCCPCC_E_RELEASE_COMPLETE,     SCCPCC_SEM_CLEANUP,                 SCCPCC_S_IDLE},
    {SCCPCC_E_FEATURE_REQ,          SCCPCC_SEM_FEATURE_REQ,             SCCPCC_S_INCOMING_RELEASING},
    {SCCPCC_E_CALL_STATE,           SCCPCC_SEM_CALL_STATE,              SCCPCC_S_INCOMING_RELEASING},
    {SCCPCC_E_UPDATE_UI,            SCCPCC_SEM_UPDATE_UI,               SCCPCC_S_INCOMING_RELEASING},
    {SCCPCC_E_MEDIA,                SCCPCC_SEM_MEDIA,                   SCCPCC_S_INCOMING_RELEASING},
    {SCCPCC_E_OPENRCV_RES,          SCCPCC_SEM_OPENRCV_RES,             SCCPCC_S_INCOMING_RELEASING},
    {SCCPCC_E_CLEANUP,              SCCPCC_SEM_CLEANUP,                 SCCPCC_S_IDLE},
    {SCCPCC_E_DEVTOUSERDATA_REQ,    SCCPCC_SEM_DEVTOUSERDATA_REQ,       SCCPCC_S_INCOMING_RELEASING},
    {SCCPCC_E_DEVTOUSERDATA_RES,    SCCPCC_SEM_DEVTOUSERDATA_RES,       SCCPCC_S_INCOMING_RELEASING}
};    
    
static sem_states_t sccpcc_sem_states[] =
{
    {sccpcc_sem_s_idle,                sizeof(sccpcc_sem_s_idle)                / SEM_EVENTS_SIZE},
    {sccpcc_sem_s_call_initiated,      sizeof(sccpcc_sem_s_call_initiated)      / SEM_EVENTS_SIZE},
    {sccpcc_sem_s_outgoing_proceeding, sizeof(sccpcc_sem_s_outgoing_proceeding) / SEM_EVENTS_SIZE},
    {sccpcc_sem_s_outgoing_alerting,   sizeof(sccpcc_sem_s_outgoing_alerting)   / SEM_EVENTS_SIZE},
    {sccpcc_sem_s_incoming_alerting,   sizeof(sccpcc_sem_s_incoming_alerting)   / SEM_EVENTS_SIZE},
    {sccpcc_sem_s_incoming_connecting, sizeof(sccpcc_sem_s_incoming_connecting) / SEM_EVENTS_SIZE},
    {sccpcc_sem_s_connected,           sizeof(sccpcc_sem_s_connected)           / SEM_EVENTS_SIZE},
    {sccpcc_sem_s_outgoing_releasing,  sizeof(sccpcc_sem_s_outgoing_releasing)  / SEM_EVENTS_SIZE},
    {sccpcc_sem_s_incoming_releasing,  sizeof(sccpcc_sem_s_incoming_releasing)  / SEM_EVENTS_SIZE}
};

static sem_tbl_t sccpcc_sem_tbl = {
    sccpcc_sem_states, SCCPCC_S_MAX, SCCPCC_E_MAX
};

sem_table_t sccpcc_sem_table = 
{
    &sccpcc_sem_tbl,        sccpcc_sem_functions,
    sccpcc_state_name,      sccpcc_event_name,
    sccpcc_sem_name,        sccpcc_function_name,
    sccpcc_debug_sem_entry, sccpcc_validate_cb,
    (sem_change_state_f *)sccpcc_change_state
};

/*
 * FUNCTION:    sccpcc_sem_name
 *
 * DESCRIPTION: Returns the sccpcc name.
 *
 * PARAMETERS:  
 *     id: not used
 *
 * RETURNS:     None.
 */                    
static char *sccpcc_sem_name (int id)
{
    return (SCCPCC_ID);
}

static char *sccpcc_state_name (int id)
{
    if ((id <= SCCPCC_S_MIN) || (id >= SCCPCC_S_MAX)) {
        return (SCCP_UNDEFINED);
    }

    return (sccpcc_state_names[id]);
}

static char *sccpcc_event_name (int id)
{
    if ((id <= SCCPCC_E_MIN) || (id >= SCCPCC_E_MAX)) {
        return (SCCP_UNDEFINED);
    }

    return (sccpcc_event_names[id]);
}

static char *sccpcc_function_name (int id)
{
    if ((id <= SCCPCC_SEM_MIN) || (id >= SCCPCC_SEM_MAX)) {
        return (SCCP_UNDEFINED);
    }

    return (sccpcc_sem_function_names[id]);
}

static void sccpcc_debug_sem_entry (sem_event_t *event)
{
    sccpcc_cccb_t *cccb = (sccpcc_cccb_t *)(event->cb);
    
    SCCP_DBG((sccpcc_debug, 9,
             "%s %-2d:%-3d:%d: %-20s: %s <- %s\n",
             SCCPCC_ID, cccb->sccpcb->id,
             cccb->id, cccb->line,
             event->function_name,
             sccpcc_state_name(event->state_id),
             sccpcc_event_name(event->event_id)));
}

static int sccpcc_validate_cb (void *cb1, void *cb2)
{
    return (sllist_find_node(((sccp_sccpcb_t *)(cb1))->cccbs, cb2) == NULL ? 1 : 0);
}

static void sccpcc_change_state (sccpcc_cccb_t *cccb, int new_state,
                                 char *fname)
{
    SCCP_DBG((sccpcc_debug, 9,
             "%s %-2d:%-3d:%d: %-20s: %s -> %s\n",
             SCCPCC_ID, cccb->sccpcb->id,
             cccb->id, cccb->line, fname,
             sccpcc_state_name(cccb->state),
             sccpcc_state_name(new_state)));

    cccb->old_state = cccb->state;
    cccb->state     = new_state;
}

#if 0 /* not used yet */
static int sccpcc_get_new_cccb_id (void)
{
    static int id = 0;

    if (++id < 0) {
        id = 1;
    }

    return (id);
}
#endif

static void sccpcc_print_cccb (sccpcc_cccb_t *cccb, char *fname)
{
    SCCP_DBG((sccpcc_debug, 3,
             "%s %-2d:%-3d:%d: %-20s: cccb= 0x%08p\n",
             SCCPCC_ID,
             cccb == NULL ? 0 : cccb->sccpcb->id,
             cccb == NULL ? 0 : cccb->id,
             cccb == NULL ? 0 : cccb->line,
             fname, cccb));
}

static void sccpcc_free_cccb (sccpcc_cccb_t *cccb)
{
    if ((cccb != NULL) && (cccb->sccpcb != NULL)) {
        sllist_delete_node(cccb->sccpcb->cccbs, cccb);
    }
}

void sccpcc_free_cccb2 (sccpcc_cccb_t *cccb)
{
    if (cccb != NULL) {
        SCCP_FREE(cccb);
    }
}

void *sccpcc_get_new_cccb (sccp_sccpcb_t *sccpcb, int conn_id)
{
    char          *fname = "get_new_cccb";
    sccpcc_cccb_t *cccb;

    /*
     * Can we make another call?
     */
    if ((sllist_get_list_size(sccpcb->cccbs) - 1) >= SCCPCC_MAX_CALLS) {
        return (NULL);
    }

    cccb = (sccpcc_cccb_t *)(SCCP_MALLOC(sizeof(*cccb)));
    if (cccb == NULL) {
        return (NULL);
    }

    SCCP_MEMSET(cccb, 0, sizeof(*cccb));

    if (sllist_add_node(sccpcb->cccbs, cccb, 0) == NULL) {
        sccpcc_free_cccb(cccb);
        return (NULL);
    }

    cccb->sccpcb = sccpcb;
    cccb->id     = conn_id;
    cccb->precedence.level = SCCPMSG_MLPP_PRECEDENCE_ROUTINE;

    sccpcc_print_cccb(cccb, fname);
        
    return (cccb);
}

static sccpcc_cccb_t *sccpcc_get_cccb_by_id (sccp_sccpcb_t *sccpcb,
                                             int id)
{
    char          *fname = "get_cccb_by_id";
    sccpcc_cccb_t *cccb  = NULL;

    cccb = sllist_get_node_by_id(sccpcb->cccbs, id);

    sccpcc_print_cccb(cccb, fname);

    return (cccb);
}

static sccpcc_cccb_t *sccpcc_get_cccb_by_call_id (sccp_sccpcb_t *sccpcb,
                                                  unsigned long call_id)
{
    char          *fname = "get_cccb_by_call_id";
    sccpcc_cccb_t *cccb  = NULL;
    sccpcc_cccb_t *curr;

    if ((sccpcb != NULL) && (sccpcb->cccbs != NULL)) {
        for (curr = ((sccpcc_cccb_t *)(sccpcb->cccbs))->next; curr != NULL; curr = curr->next) {
            if (curr->sccp_call_id == call_id) {
                cccb = curr;
                break;
            }
        }
    }

    sccpcc_print_cccb(cccb, fname);

    return (cccb);
}

#if 0 /* not used yet */
static sccpcc_cccb_t *sccpcc_get_cccb_by_sccp_line (sccp_sccpcb_t *sccpcb,
                                                    unsigned long line)
{
    char          *fname = "get_cccb_by_line";
    sccpcc_cccb_t *cccb  = NULL;
    sccpcc_cccb_t *curr;

    if ((sccpcb != NULL) && (sccpcb->cccbs != NULL)) {
        for (curr = ((sccpcc_cccb_t *)(sccpcb->cccbs))->next; curr != NULL; curr = curr->next) {
            if (curr->sccp_line == line) {
                cccb = curr;
                break;
            }
        }
    }

    sccpcc_print_cccb(cccb, fname);

    return (cccb);
}
#endif

static sccpcc_cccb_t *sccpcc_get_cccb_by_state (sccp_sccpcb_t *sccpcb,
                                                int state)
{
    char          *fname = "get_cccb_by_state";
    sccpcc_cccb_t *cccb  = NULL;
    sccpcc_cccb_t *curr;

    if ((sccpcb != NULL) && (sccpcb->cccbs != NULL)) {
        for (curr = ((sccpcc_cccb_t *)(sccpcb->cccbs))->next; curr != NULL; curr = curr->next) {
            if (curr->state == state) {
                cccb = curr;
                break;
            }
        }
    }

    sccpcc_print_cccb(cccb, fname);

    return (cccb);
}

static sccpcc_cccb_t *sccpcc_get_cccb_by_state_media (sccp_sccpcb_t *sccpcb)
{
    char          *fname = "get_cccb_by_state_media";
    sccpcc_cccb_t *cccb  = NULL;

    sccpcc_cccb_t *curr;
    if ((sccpcb != NULL) && (sccpcb->cccbs != NULL)) {
        for (curr = ((sccpcc_cccb_t *)(sccpcb->cccbs))->next; curr != NULL; curr = curr->next) {
            if ((curr->state == SCCPCC_S_CALL_INITIATED)      ||
                (curr->state == SCCPCC_S_OUTGOING_PROCEEDING) ||
                (curr->state == SCCPCC_S_OUTGOING_ALERTING)   ||
                (curr->state == SCCPCC_S_INCOMING_ALERTING)   ||
                (curr->state == SCCPCC_S_INCOMING_CONNECTING) ||
                (curr->state == SCCPCC_S_CONNECTED)) {

                cccb = curr;
                break;
            }
        }
    }

    sccpcc_print_cccb(cccb, fname);

    return (cccb);
}

static sccpcc_cccb_t *sccpcc_get_cccb_by_passthru_party_id (sccp_sccpcb_t *sccpcb,
                                                            unsigned long conference_id,
                                                            unsigned long passthru_party_id)
{
    char          *fname = "get_cccb_by_passthru_party_id";
    sccpcc_cccb_t *cccb  = NULL;

    sccpcc_cccb_t *curr;
    if ((sccpcb != NULL) && (sccpcb->cccbs != NULL)) {
        for (curr = ((sccpcc_cccb_t *)(sccpcb->cccbs))->next; curr != NULL; curr = curr->next) {
            if ((curr->conference_id == conference_id) && 
                (curr->passthru_party_id == passthru_party_id)) {
                cccb = curr;
                break;
            }
        }
    }

    sccpcc_print_cccb(cccb, fname);

    return (cccb);
}

static void sccpcc_get_passthru_party_id (sccpmsg_general_t *msg,
                                          unsigned long *conference_id,
                                          unsigned long *passthru_party_id)
{
    *conference_id = 0;
    *passthru_party_id = 0;
    
    if (msg == NULL) {
        return;
    }

    switch (msg->body.msg_id.message_id) { 
    case (SCCPMSG_OPEN_RECEIVE_CHANNEL):
        *conference_id     = msg->body.open_receive_channel.conference_id;            
        *passthru_party_id = msg->body.open_receive_channel.passthru_party_id;

        break;
        
    case (SCCPMSG_CLOSE_RECEIVE_CHANNEL):
        *conference_id     = msg->body.close_receive_channel.conference_id;            
        *passthru_party_id = msg->body.close_receive_channel.passthru_party_id;

        break;
        
    case (SCCPMSG_START_MEDIA_TRANSMISSION):
        *conference_id     = msg->body.start_media_transmission.conference_id;            
        *passthru_party_id = msg->body.start_media_transmission.passthru_party_id;

        break;
        
    case (SCCPMSG_STOP_MEDIA_TRANSMISSION):
        *conference_id     = msg->body.stop_media_transmission.conference_id;            
        *passthru_party_id = msg->body.stop_media_transmission.passthru_party_id;

        break;
    }                       
}

sccpcc_cccb_t *sccpcc_get_cccb_by_sccpmsg (sccp_sccpcb_t *sccpcb,
                                           sccpmsg_general_t *msg)
{
    char          *fname = "get_cccb_by_sccpmsg";
    sccpcc_cccb_t *cccb  = NULL;
    unsigned long call_id = 0;
    unsigned long line    = 0;
    unsigned long conference_id;
    unsigned long passthru_party_id;

    if ((msg == NULL) || (sccpcb == NULL)) {
        sccpcc_print_cccb(cccb, fname); 
    
        return (cccb);
    }

    sccpmsg_get_sccp_msg_data(sccpcb, msg, &call_id, &line);

    switch (msg->body.msg_id.message_id) { 
    case SCCPMSG_OPEN_RECEIVE_CHANNEL:
        if (call_id != 0) {
            cccb = sccpcc_get_cccb_by_call_id(sccpcb, call_id);
            if (cccb != NULL) {
                break;
            } 
        }

        /* -- versions < 3.4 --
         * STATION_OPEN_RECEIVE_CHANNEL do not have call_references and
         * the passthru_party_id is new for each message so we will not
         * find a cccb handling this message. But, we can find a cccb
         * that is (hopefully) in a state to handle it.
         */
        cccb = sccpcc_get_cccb_by_state_media(sccpcb);
        if (cccb != NULL) {
            cccb->conference_id     = msg->body.open_receive_channel.conference_id;
            cccb->passthru_party_id = msg->body.open_receive_channel.passthru_party_id;        

            SCCP_DBG((sccpcc_debug, 9,
                     "%s %-2d:%-3d:%d: %-20s: conference_id= 0x%08x,"
                     " passthru_party_id= 0x%08x\n",
                     SCCPCC_ID, cccb->sccpcb->id,
                     cccb->id, cccb->line, fname,
                     cccb->conference_id, cccb->passthru_party_id));

            break;
        }

        /*
         * Still don't have a cccb, try the last ditch effort to
         * let the default cccb handle it.
         */
        cccb = sccpcc_get_cccb_by_id(sccpcb, SCCPCC_DEFAULT_ID);

        break;

    case SCCPMSG_CLOSE_RECEIVE_CHANNEL:
    case SCCPMSG_START_MEDIA_TRANSMISSION:
    case SCCPMSG_STOP_MEDIA_TRANSMISSION:
        if (call_id != 0) {
            cccb = sccpcc_get_cccb_by_call_id(sccpcb, call_id);
            if (cccb != NULL) {
                break;
            } 
        }

        sccpcc_get_passthru_party_id(msg, &conference_id, &passthru_party_id);
        cccb = sccpcc_get_cccb_by_passthru_party_id(sccpcb,
                                                    conference_id,
                                                    passthru_party_id);
        /*
         * Still don't have a cccb, try the last ditch effort to
         * let the default cccb handle it.
         */
        if (cccb == NULL) {
            cccb = sccpcc_get_cccb_by_id(sccpcb, SCCPCC_DEFAULT_ID);
        }

        break;
        
    default:
    case (SCCPMSG_DEFINE_TIME_DATE):
    case (SCCPMSG_DISPLAY_NOTIFY):
    case (SCCPMSG_CLEAR_NOTIFY):    
    case (SCCPMSG_START_TONE):
    case (SCCPMSG_STOP_TONE):
    case (SCCPMSG_KEYPAD_BUTTON):
    case (SCCPMSG_SET_RINGER):
    case (SCCPMSG_SET_LAMP):
    case (SCCPMSG_SET_SPEAKER_MODE):
    case (SCCPMSG_SET_MICRO_MODE):
    case (SCCPMSG_CALL_INFO):
    case (SCCPMSG_SELECT_SOFTKEYS):
    case (SCCPMSG_CALL_STATE):
    case (SCCPMSG_DISPLAY_PROMPT_STATUS):
    case (SCCPMSG_CLEAR_PROMPT_STATUS):
    case (SCCPMSG_ACTIVATE_CALL_PLANE):
    case (SCCPMSG_BACKSPACE_REQ):
    case (SCCPMSG_DIALED_NUMBER):
    case (SCCPMSG_CALL_SELECT_STAT):
    case (SCCPMSG_USER_TO_DEVICE_DATA):
    case (SCCPMSG_CONNECTION_STATISTICS_REQ):
    case (SCCPMSG_DISPLAY_PRIORITY_NOTIFY):
    case (SCCPMSG_CLEAR_PRIORITY_NOTIFY):
        if (call_id != 0) {
            cccb = sccpcc_get_cccb_by_call_id(sccpcb, call_id);
            if (cccb != NULL) {
                break;
            } 

            /*
             * The CALL_STATE message will be the first message for new
             * outgoing and incoming calls that has a call_reference.
             */
            if (msg->body.msg_id.message_id == SCCPMSG_CALL_STATE) { 
                if (msg->body.call_state.call_state == SCCPMSG_CALL_STATE_RING_IN) {
                    /*
                     * This is a new incoming call so grab a new cccb.
                     */
                    cccb = sccpcc_get_new_cccb(sccpcb, gapi_get_new_conn_id());
                }
                else if (msg->body.call_state.call_state ==
                         SCCPMSG_CALL_STATE_OFFHOOK) {

                    /*
                     * This must be for an outgoing call,
                     * because we do not have a call_id yet.
                     * Look for a call that has just started.
                     */
                    cccb = sccpcc_get_cccb_by_state(sccpcb,
                                                    SCCPCC_S_CALL_INITIATED);

                    /*
                     * And it is possible that this new CALL_STATE(offhook)
                     * might be for a brand new call. Normally the
                     * CALL_STATE(ringin) is the first incoming message
                     * for a new call, but for transfers, the 
                     * CALL_STATE(offhook) will be first.
                     */
                    if (cccb == NULL) {
                        /*
                         * This is a new incoming call so grab a new cccb.
                         */
                        cccb = sccpcc_get_new_cccb(sccpcb, gapi_get_new_conn_id());
                    }
                }
                //else if (msg->body.call_state.call_state == MULTILINE) {
                
                if (cccb != NULL) { 
                    cccb->sccp_call_id = msg->body.call_state.call_reference;
                    cccb->sccp_line    = msg->body.call_state.line_number;

                    SCCP_DBG((sccpcc_debug, 9,
                             "%s %-2d:%-3d:%d: %-20s: "
                             "call_reference= 0x%08x, line= 0x%08x\n",
                             SCCPCC_ID, cccb->sccpcb->id,
                             cccb->id, cccb->line, fname,
                             cccb->sccp_call_id, cccb->sccp_line));

                    break;
                }
            }
        }
#if 0
        if (line != 0) {
            cccb = sccpcc_get_cccb_by_sccp_line(sccpcb, line);
            if (cccb != NULL) {
                break;
            }
        }
#endif
        /*
         * The startTone message with a reorder tone is used to indicate
         * call setup failure, so let's try to see if it goes to a new
         * outgoing call.
         */
        if ((msg->body.msg_id.message_id == SCCPMSG_START_TONE) &&
            (msg->body.start_tone.tone == SCCPMSG_TONE_REORDER)) {
            cccb = sccpcc_get_cccb_by_state(sccpcb, SCCPCC_S_CALL_INITIATED);

            if (cccb != NULL) {
                /*
                 * Don't set the call_id for versions less than Parche,
                 * since those version will have the values set to defaults
                 * by sccpmsg.c.
                 */
                if (sccpcb->version >= SCCPMSG_VERSION_PARCHE) {
                    cccb->sccp_call_id = msg->body.start_tone.call_reference;
                    cccb->sccp_line    = msg->body.start_tone.line_number;
                }

                SCCP_DBG((sccpcc_debug, 9,
                         "%s %-2d:%-3d:%d: %-20s: "
                         "call_reference= 0x%08x, line= 0x%08x\n",
                         SCCPCC_ID, cccb->sccpcb->id,
                         cccb->id, cccb->line, fname,
                         cccb->sccp_call_id, cccb->sccp_line));

                break;
            }
        }

        /*
         * Still don't have a cccb, try the last ditch effort to
         * let the default cccb handle it.
         */
        cccb = sccpcc_get_cccb_by_id(sccpcb, SCCPCC_DEFAULT_ID);
        break;
    }
    
    sccpcc_print_cccb(cccb, fname); 
    
    return (cccb);
}

/*
 * FUNCTION:    sccpcc_copy_open_recv_media
 *
 * DESCRIPTION: Copy the media from the open_receive_channel message to a
 *              local format.
 *
 * PARAMETERS:
 *     event:   event control block
 *
 * RETURNS:     0 for success, otherwise 1.
 *
 * NOTES:
 */
static void sccpcc_copy_open_recv_media (gapi_media_t *media,
                                         sccpmsg_open_receive_channel_t *msg)
{
    SCCP_MEMSET(media, 0, sizeof(*media));
    media->sccp_media.conference_id    = msg->conference_id;
    media->sccp_media.passthruparty_id = msg->passthru_party_id;
    media->sccp_media.packet_size      = msg->ms_packet_size;
    media->sccp_media.payload_type     = msg->payload;
    media->sccp_media.g723_bitrate     = msg->qualifier.g723_bit_rate;
    media->sccp_media.echo_cancelation = msg->qualifier.echo_cancellation;
}

/*
 * FUNCTION:    sccpcc_copy_start_xmit_media
 *
 * DESCRIPTION: Copy the media from the start_media_tranmission media to 
 *              a local format.
 *
 * PARAMETERS:
 *     event:   event control block
 *
 * RETURNS:     0 for success, otherwise 1.
 *
 * NOTES:       gapi expects addr and port in network order. 
 */
/*
 * addr will be in big-endian, port will be little-endian.
 * convert port to big-endian.
 */
static void sccpcc_copy_start_xmit_media (gapi_media_t *media,
                                          sccpmsg_start_media_transmission_t *msg)
{
    SCCP_MEMSET(media, 0, sizeof(*media));
    media->sccp_media.conference_id       = msg->conference_id;
    media->sccp_media.passthruparty_id    = msg->passthru_party_id;
    media->sccp_media.addr                = msg->ip_address;
    media->sccp_media.port                = SCCP_HTONL(msg->port);
    media->sccp_media.packet_size         = msg->ms_packet_size;
    media->sccp_media.payload_type        = msg->payload;
    media->sccp_media.g723_bitrate        = msg->qualifier.g723_bit_rate;
    media->sccp_media.precedence          = msg->qualifier.precedence;
    media->sccp_media.silence_suppression = msg->qualifier.silence_supression;
    media->sccp_media.frames_per_packet   = msg->qualifier.max_frames_per_packet;
}

/*
 * FUNCTION:    sccpcc_copy_call_info
 *
 * DESCRIPTION: Copy the SCCP call_info to local data.
 *
 * PARAMETERS:
 *     event:   event control block
 *
 * RETURNS:     0 for success, otherwise 1.
 *
 * NOTES:
 */
static void sccpcc_copy_call_info (gapi_conninfo_t *conninfo,
                                   sccpmsg_call_info_t *call_info)
{
    if (conninfo == NULL) {
        return;
    }

    SCCP_MEMSET(conninfo, 0, sizeof(conninfo));

    conninfo->calling_name      = call_info->calling_party_name;
    conninfo->calling_number    = call_info->calling_party_number;
    conninfo->called_name       = call_info->called_party_name;
    conninfo->called_number     = call_info->called_party_number;
    conninfo->original_name     = call_info->original_called_party_name;
    conninfo->original_number   = call_info->original_called_party_number;
    conninfo->redirected_name   = call_info->last_redirecting_party_name;
    conninfo->redirected_number = call_info->last_redirecting_party_number;
}

static int sccpcc_event_setup (sccp_sccpcb_t *sccpcb, int msg_id,
                               int conn_id, int line,
                               void **msg, int size,
                               sccpcc_cccb_t **cccb)
{
    sccpcc_msg_setup_t *lmsg;

    *cccb = sccpcc_get_cccb_by_id(sccpcb, conn_id);
    if (cccb == NULL) {
        return (1);
    }

    *msg = SCCP_MALLOC(size);
    if (*msg == NULL) {
        return (2);
    }

    SCCP_MEMSET(*msg, 0, size);

    lmsg = *msg;

    lmsg->msg_id  = msg_id;
    lmsg->conn_id = conn_id;
    lmsg->line    = line;

    return (0);
}

int sccpcc_push_simple (sccp_sccpcb_t *sccpcb, int msg_id, int conn_id,
                        int line, int front)
{
    sccpcc_cccb_t *cccb;

    cccb = sccpcc_get_cccb_by_id(sccpcb, conn_id);
    if (cccb == NULL) {
        return (11);
    }

    sccp_push_event(sccpcb, NULL, 0, msg_id, &sccpcc_sem_table, cccb, front);

    return (0);
}

int sccpcc_push_cleanup (sccp_sccpcb_t *sccpcb, int msg_id, int conn_id,
                         int line, gapi_causes_e cause)
{
    sccpcc_msg_cleanup_t *msg;
    sccpcc_cccb_t        *cccb;
    int                  rc;

    rc = sccpcc_event_setup(sccpcb, SCCPCC_E_CLEANUP, conn_id,
                            line, (void **)(&msg), sizeof(*msg), &cccb);
    if (rc != 0) {
        return (rc);
    }

    sccp_push_event(sccpcb, msg, 0, msg_id, &sccpcc_sem_table, cccb, 0);

    return (0);
}

int sccpcc_push_passthru (sccp_sccpcb_t *sccpcb, int msg_id, void *msg, int len)
{
    int socket;
    int rc = 1; /* error */

    socket = sccpcm_get_primary_socket(sccpcb);
    if (socket != 0) {
        rc = sccpmsg_send_message(&sccpcc_msgcb, socket, msg, len);
    }

    return (rc);
}

int sccpcc_push_setup (sccp_sccpcb_t *sccpcb, int msg_id, int conn_id,
                       int line, gapi_conninfo_t *conninfo, char *digits,
                       int numdigits, gapi_media_t *media,
                       gapi_alert_info_e alert_info, gapi_privacy_e privacy,
                       gapi_precedence_t *precedence)
{
    sccpcc_msg_setup_t *msg;
    sccpcc_cccb_t      *cccb;
    int                rc;

    if (sccpcb == NULL)  {
        return (1);
    }

    /*
     * Make sure this is a new conn_id.
     */
    cccb = sccpcc_get_cccb_by_id(sccpcb, conn_id);
    if (cccb != NULL) {
        /*
         * Reject the setup since the conn_id is already in use.
         */
        SCCP_RELEASE_COMPLETE(cccb,
                              GAPI_MSG_RELEASE_COMPLETE,
                              conn_id, line, GAPI_CAUSE_CALL_ID_IN_USE,
                              &(cccb->precedence));

        return (0);
    }

    cccb = sccpcc_get_new_cccb(sccpcb, conn_id);
    if (cccb == NULL) {
        /*
         * Reject the new call since we could not grab a cccb.
         */
        SCCP_RELEASE_COMPLETE(cccb,
                              GAPI_MSG_RELEASE_COMPLETE,
                              conn_id, line, GAPI_CAUSE_NO_RESOURCE,
                              &(cccb->precedence));

        return (0);
    }

    rc = sccpcc_event_setup(sccpcb, SCCPCC_E_SETUP, conn_id,
                            line, (void **)(&msg), sizeof(*msg), &cccb);
    if (rc != 0) {
        SCCP_RELEASE_COMPLETE(cccb,
                              GAPI_MSG_RELEASE_COMPLETE,
                              conn_id, line, GAPI_CAUSE_NO_RESOURCE,
                              &(cccb->precedence));

        sccpcc_free_cccb(cccb);

        return (0);
    }
    
    if (conninfo != NULL) {
        msg->conninfo = *conninfo;
    }
    if (numdigits > GAPI_DIRECTORY_NUMBER_SIZE) {
        msg->numdigits = GAPI_DIRECTORY_NUMBER_SIZE;
    }
    else {
        msg->numdigits = numdigits;
    }
    if (msg->digits != NULL) {
        SCCP_MEMCPY(msg->digits, digits, msg->numdigits);
    }
    if (media != NULL) {
        msg->media = *media;
    }
    msg->alert_info = alert_info;
    msg->privacy    = privacy;
    if (precedence != NULL) {
        msg->precedence = *precedence;
    }

    sccp_push_event(sccpcb, msg, 0, SCCPCC_E_SETUP,
                    &sccpcc_sem_table, cccb, 0);

    return (0);
}

int sccpcc_push_setup_ack (sccp_sccpcb_t *sccpcb, int msg_id, int conn_id,
                           int line, gapi_conninfo_t *conninfo,
                           gapi_media_t *media, gapi_causes_e cause,
                           gapi_precedence_t *precedence)
{
    sccpcc_msg_setup_ack_t *msg;
    sccpcc_cccb_t          *cccb;
    int                    rc;

    rc = sccpcc_event_setup(sccpcb, SCCPCC_E_SETUP_ACK, conn_id,
                            line, (void **)(&msg), sizeof(*msg), &cccb);
    if (rc != 0) {
        return (rc);
    }

    if (conninfo != NULL) {
        msg->conninfo = *conninfo;
    }
    if (media != NULL) {
        msg->media = *media;
    }
    msg->cause = cause;
    if (precedence != NULL) {
        msg->precedence = *precedence;
    }

    sccp_push_event(sccpcb, msg, 0, SCCPCC_E_SETUP_ACK,
                    &sccpcc_sem_table, cccb, 0);

    return (0);
}

int sccpcc_push_proceeding (sccp_sccpcb_t *sccpcb, int msg_id, int conn_id,
                            int line, gapi_conninfo_t *conninfo,
                            gapi_precedence_t *precedence)
{
    sccpcc_msg_proceeding_t *msg;
    sccpcc_cccb_t           *cccb;
    int                     rc;

    rc = sccpcc_event_setup(sccpcb, SCCPCC_E_PROCEEDING, conn_id,
                            line, (void **)(&msg), sizeof(*msg), &cccb);
    if (rc != 0) {
        return (rc);
    }

    if (conninfo != NULL) {
        msg->conninfo = *conninfo;
    }
    if (precedence != NULL) {
        msg->precedence = *precedence;
    }

    sccp_push_event(sccpcb, msg, 0, SCCPCC_E_PROCEEDING,
                    &sccpcc_sem_table, cccb, 0);

    return (0);
}

int sccpcc_push_alerting (sccp_sccpcb_t *sccpcb, int msg_id, int conn_id,
                          int line, gapi_conninfo_t *conninfo,
                          gapi_media_t *media, gapi_inband_e inband,
                          gapi_precedence_t *precedence)
{
    sccpcc_msg_alerting_t *msg;
    sccpcc_cccb_t         *cccb;
    int                    rc;

    rc = sccpcc_event_setup(sccpcb, SCCPCC_E_ALERTING, conn_id,
                            line, (void **)(&msg), sizeof(*msg), &cccb);
    if (rc != 0) {
        return (rc);
    }

    if (conninfo != NULL) {
        msg->conninfo = *conninfo;
    }
    if (media != NULL) {
        msg->media = *media;
    }
    msg->inband = inband;
    if (precedence != NULL) {
        msg->precedence = *precedence;
    }

    sccp_push_event(sccpcb, msg, 0, SCCPCC_E_ALERTING,
                    &sccpcc_sem_table, cccb, 0);

    return (0);
}

int sccpcc_push_connect (sccp_sccpcb_t *sccpcb, int msg_id, int conn_id,
                         int line, gapi_conninfo_t *conninfo,
                         gapi_media_t *media,
                         gapi_precedence_t *precedence)
{
    sccpcc_msg_connect_t *msg;
    sccpcc_cccb_t        *cccb;
    int                  rc;

    rc = sccpcc_event_setup(sccpcb, SCCPCC_E_CONNECT, conn_id,
                            line, (void **)(&msg), sizeof(*msg), &cccb);
    if (rc != 0) {
        return (rc);
    }

    if (conninfo != NULL) {
        msg->conninfo = *conninfo;
    }
    if (media != NULL) {
        msg->media = *media;
    }
    if (precedence != NULL) {
        msg->precedence = *precedence;
    }

    sccp_push_event(sccpcb, msg, 0, SCCPCC_E_CONNECT,
                    &sccpcc_sem_table, cccb, 0);

    return (0);
}

int sccpcc_push_connect_ack (sccp_sccpcb_t *sccpcb, int msg_id, int conn_id,
                             int line, gapi_conninfo_t *conninfo,
                             gapi_media_t *media,
                             gapi_precedence_t *precedence)
{
    sccpcc_msg_connect_ack_t *msg;
    sccpcc_cccb_t            *cccb;
    int                      rc;

    rc = sccpcc_event_setup(sccpcb, SCCPCC_E_CONNECT_ACK, conn_id,
                            line, (void **)(&msg), sizeof(*msg), &cccb);
    if (rc != 0) {
        return (rc);
    }

    if (conninfo != NULL) {
        msg->conninfo = *conninfo;
    }
    if (media != NULL) {
        msg->media = *media;
    }
    if (precedence != NULL) {
        msg->precedence = *precedence;
    }

    sccp_push_event(sccpcb, msg, 0, SCCPCC_E_CONNECT_ACK,
                    &sccpcc_sem_table, cccb, 0);

    return (0);
}

int sccpcc_push_disconnect (sccp_sccpcb_t *sccpcb, int msg_id, int conn_id,
                            int line, gapi_causes_e cause,
                            gapi_precedence_t *precedence)
{
    sccpcc_msg_disconnect_t *msg;
    sccpcc_cccb_t           *cccb;
    int                     rc;

    rc = sccpcc_event_setup(sccpcb, SCCPCC_E_DISCONNECT, conn_id,
                            line, (void **)(&msg), sizeof(*msg), &cccb);
    if (rc != 0) {
        return (rc);
    }

    msg->cause = cause;
    if (precedence != NULL) {
        msg->precedence = *precedence;
    }

    sccp_push_event(sccpcb, msg, 0, SCCPCC_E_DISCONNECT,
                    &sccpcc_sem_table, cccb, 0);

    return (0);
}

int sccpcc_push_release (sccp_sccpcb_t *sccpcb, int msg_id, int conn_id,
                         int line, gapi_causes_e cause,
                         gapi_precedence_t *precedence)
{
    sccpcc_msg_release_t *msg;
    sccpcc_cccb_t        *cccb;
    int                  rc;

    rc = sccpcc_event_setup(sccpcb, SCCPCC_E_RELEASE, conn_id,
                            line, (void **)(&msg), sizeof(*msg), &cccb);
    if (rc != 0) {
        return (rc);
    }

    msg->cause   = cause;
    if (precedence != NULL) {
        msg->precedence = *precedence;
    }

    sccp_push_event(sccpcb, msg, 0, SCCPCC_E_RELEASE,
                    &sccpcc_sem_table, cccb, 0);

    return (0);
}

int sccpcc_push_release_complete (sccp_sccpcb_t *sccpcb, int msg_id,
                                  int conn_id, int line, gapi_causes_e cause,
                                  gapi_precedence_t *precedence)
{
    sccpcc_msg_release_complete_t *msg;
    sccpcc_cccb_t                 *cccb;
    int                           rc;

    rc = sccpcc_event_setup(sccpcb, SCCPCC_E_RELEASE_COMPLETE, conn_id,
                            line, (void **)(&msg), sizeof(*msg), &cccb);
    if (rc != 0) {
        return (rc);
    }

    msg->cause = cause;
    if (precedence != NULL) {
        msg->precedence = *precedence;
    }

    sccp_push_event(sccpcb, msg, 0, SCCPCC_E_RELEASE_COMPLETE,
                    &sccpcc_sem_table, cccb, 0);
    
    return (0);
}

int sccpcc_push_openrcv_res (sccp_sccpcb_t *sccpcb, int msg_id, int conn_id,
                             int line, gapi_media_t *media,
                             gapi_causes_e cause)
{
    sccpcc_msg_openrcv_res_t *msg;
    sccpcc_cccb_t            *cccb;
    int                      rc;

    rc = sccpcc_event_setup(sccpcb, SCCPCC_E_OPENRCV_RES, conn_id,
                            line, (void **)(&msg), sizeof(*msg), &cccb);
    if (rc != 0) {
        return (rc);
    }

    if (media != NULL) {
        msg->media = *media;
    }
    msg->cause = cause;

    sccp_push_event(sccpcb, msg, 0, SCCPCC_E_OPENRCV_RES,
                    &sccpcc_sem_table, cccb, 0);

    return (0);
}

int sccpcc_push_feature_req (sccp_sccpcb_t *sccpcb, int msg_id, int conn_id,
                             int line, gapi_features_e feature,
                             gapi_feature_data_u *data, gapi_causes_e cause)
{
    sccpcc_msg_feature_req_t *msg;
    sccpcc_cccb_t            *cccb;
    int                      rc;

    if (sccpcb == NULL)  {
        return (1);
    }

    /*
     * Is this fewture for a new call or an existing one?
     */
    cccb = sccpcc_get_cccb_by_id(sccpcb, conn_id);
    if (cccb == NULL) {
        /*
         * Can't find it so it must be a new call.
         */
        if ((feature == GAPI_FEATURE_REDIAL) ||
            (feature == GAPI_FEATURE_SPEEDDIAL)) {

            cccb = sccpcc_get_new_cccb(sccpcb, conn_id);
            if (cccb == NULL) {
                /*
                 * Reject the new call since we could not grab a cccb.
                 */
                SCCP_RELEASE_COMPLETE(cccb,
                                             GAPI_MSG_RELEASE_COMPLETE,
                                             conn_id, line, GAPI_CAUSE_NO_RESOURCE,
                                             &(cccb->precedence));

                return (0);
            }
        }
        /*
         * Guess it wasn't for a new call - toss it.
         */
        else {
            return (0);
        }
    }

    rc = sccpcc_event_setup(sccpcb, SCCPCC_E_FEATURE_REQ, conn_id,
                            line, (void **)(&msg), sizeof(*msg), &cccb);
    if (rc != 0) {
        return (rc);
    }

    msg->feature = feature;
    if (data != NULL) {
        msg->data = *data;
    }
    msg->cause = cause;

    sccp_push_event(sccpcb, msg, 0, SCCPCC_E_FEATURE_REQ,
                    &sccpcc_sem_table, cccb, 0);

    return (0);
}

int sccpcc_push_feature_res (sccp_sccpcb_t *sccpcb, int msg_id, int conn_id,
                             int line, gapi_features_e feature,
                             gapi_feature_data_u *data, gapi_causes_e cause)
{
    sccpcc_cccb_t *cccb;

    if (sccpcb == NULL)  {
        return (1);
    }

    cccb = sccpcc_get_cccb_by_id(sccpcb, conn_id);
    if (cccb == NULL) {
        return (1);
    }

    SCCP_DBG((sccpcc_debug, 9,
             "%s %-2d:%-3d:%d: %-20s: feature_res= %d\n",
             SCCPCC_ID, cccb->sccpcb->id,
             cccb->id, cccb->line,
             "sccpcc_feature_res",
             feature));

    /*
     * SCCPCC_E_FEATURE_RES events are ignored, so don't add the event
     * to the queue.
     */

    return (0);
}

int sccpcc_push_cs_release (sccp_sccpcb_t *sccpcb, int msg_id, int conn_id,
                            int line, gapi_causes_e cause)
{
    sccpcc_msg_release_t *msg;
    sccpcc_cccb_t        *cccb;
    int                  rc;

    rc = sccpcc_event_setup(sccpcb, SCCPCC_E_CS_RELEASE, conn_id,
                            line, (void **)(&msg), sizeof(*msg), &cccb);
    if (rc != 0) {
        return (rc);
    }

    msg->cause   = cause;

    sccp_push_event(sccpcb, msg, 0, SCCPCC_E_CS_RELEASE,
                    &sccpcc_sem_table, cccb, 0);

    return (0);
}

int sccpcc_push_offhook (sccp_sccpcb_t *sccpcb, int msg_id, int conn_id,
                         int line)
{
    sccpcc_cccb_t *cccb;
    int           rc;

    if (sccpcb == NULL)  {
        return (1);
    }

    /*
     * Is this offhook for a new call or an existing one?
     */
    cccb = sccpcc_get_cccb_by_id(sccpcb, conn_id);
    if (cccb == NULL) {
        /*
         * Can't find it so it must be a new call.
         */
        rc = sccpcc_push_setup(sccpcb, SCCPCC_E_SETUP, conn_id,
                               line, NULL, NULL,
                               0, NULL,
                               0, 0, NULL);
    }
    else {
        rc = sccpcc_push_simple(sccpcb, SCCPCC_E_OFFHOOK, conn_id, line, 1);
    }

    return (rc);
}

int sccpcc_push_digits (sccp_sccpcb_t *sccpcb, int msg_id, int conn_id,
                        int line, char *digits, int numdigits)
{
    sccpcc_msg_digits_t *msg;
    sccpcc_cccb_t       *cccb;
    int                 rc;

    rc = sccpcc_event_setup(sccpcb, SCCPCC_E_DIGITS, conn_id,
                            line, (void **)(&msg), sizeof(*msg), &cccb);
    if (rc != 0) {
        return (rc);
    }

    if (numdigits > GAPI_DIRECTORY_NUMBER_SIZE) {
        msg->numdigits = GAPI_DIRECTORY_NUMBER_SIZE;
    }
    else {
        msg->numdigits = numdigits;
    }
    SCCP_STRNCPY(msg->digits, digits, msg->numdigits);
    msg->digits[GAPI_DIRECTORY_NUMBER_SIZE] = '\0';

    sccp_push_event(sccpcb, msg, 0, SCCPCC_E_DIGITS,
                    &sccpcc_sem_table, cccb, 0);

    return (0);
}

int sccpcc_push_devicetouserdata_req(struct sccp_sccpcb_t_ *sccpcb,
                                     int msg_id, int conn_id, int line,
                                     gapi_user_and_device_data_t *data)
{
    sccpcc_msg_devicetouserdata_req_t *msg;
    sccpcc_cccb_t                     *cccb;
    int                               rc;

    rc = sccpcc_event_setup(sccpcb, SCCPCC_E_DEVTOUSERDATA_REQ, conn_id,
                            line, (void **)(&msg), sizeof(*msg), &cccb);
    if (rc != 0) {
        return (rc);
    }

    msg->data.line_number       = data->line_number;
    msg->data.call_reference    = data->call_reference;
    SCCP_MEMCPY(&msg->data.data, data->data, data->data_length);
    msg->data.data_length       = data->data_length;
    msg->data.application_id    = data->application_id;
    msg->data.transaction_id    = data->transaction_id;
    
    sccp_push_event(sccpcb, msg, 0, SCCPCC_E_DEVTOUSERDATA_REQ,
                    &sccpcc_sem_table, cccb, 0);
    
    return (0);
}

int sccpcc_push_devicetouserdata_res(struct sccp_sccpcb_t_ *sccpcb,
                                     int msg_id, int conn_id, int line,
                                     gapi_user_and_device_data_t *data)
{
    sccpcc_msg_devicetouserdata_res_t *msg;
    sccpcc_cccb_t                     *cccb;
    int                               rc;
    
    rc = sccpcc_event_setup(sccpcb, SCCPCC_E_DEVTOUSERDATA_RES, conn_id,
                            line, (void **)(&msg), sizeof(*msg), &cccb);
    if (rc != 0) {
        return (rc);
    }
    
    msg->data.line_number       = data->line_number;
    msg->data.call_reference    = data->call_reference;
    SCCP_MEMCPY(&msg->data.data, data->data, data->data_length);
    msg->data.data_length       = data->data_length;
    msg->data.application_id    = data->application_id;
    msg->data.transaction_id    = data->transaction_id;
    
    sccp_push_event(sccpcb, msg, 0, SCCPCC_E_DEVTOUSERDATA_RES,
                    &sccpcc_sem_table, cccb, 0);
    
    return (0);
}

static sccpmsg_keypad_buttons_t sccpcc_gapi_to_sccp_digit (char digit)
{
    sccpmsg_keypad_buttons_t button;

    switch (digit) {
    case ('0'):
        button = SCCPMSG_KEYPAD_BUTTON_ZERO;
        break;

    case ('1'):
        button = SCCPMSG_KEYPAD_BUTTON_ONE;
        break;

    case ('2'):
        button = SCCPMSG_KEYPAD_BUTTON_TWO;
        break;

    case ('3'):
        button = SCCPMSG_KEYPAD_BUTTON_THREE;
        break;

    case ('4'):
        button = SCCPMSG_KEYPAD_BUTTON_FOUR;
        break;

    case ('5'):
        button = SCCPMSG_KEYPAD_BUTTON_FIVE;
        break;

    case ('6'):
        button = SCCPMSG_KEYPAD_BUTTON_SIX;
        break;

    case ('7'):
        button = SCCPMSG_KEYPAD_BUTTON_SEVEN;
        break;

    case ('8'):
        button = SCCPMSG_KEYPAD_BUTTON_EIGHT;
        break;

    case ('9'):
        button = SCCPMSG_KEYPAD_BUTTON_NINE;
        break;

    case ('a'):
    case ('A'):
        button = SCCPMSG_KEYPAD_BUTTON_A;
        break;
        
    case ('b'):
    case ('B'):
        button = SCCPMSG_KEYPAD_BUTTON_B;
        break;
        
    case ('c'):
    case ('C'):
        button = SCCPMSG_KEYPAD_BUTTON_C;
        break;
        
    case ('d'):
    case ('D'):
        button = SCCPMSG_KEYPAD_BUTTON_D;
        break;
        
    case ('*'):
        button = SCCPMSG_KEYPAD_BUTTON_STAR;
        break;
        
    case ('#'):
        button = SCCPMSG_KEYPAD_BUTTON_POUND;
        break;
        
    default:
        button = SCCPMSG_KEYPAD_BUTTON_INVALID;
        break;
    }

    return (button);
}

/*
 * FUNCTION:    sccpcc_sem_default
 *
 * DESCRIPTION: Handle unrecognized events.
 *
 * PARAMETERS:
 *     event:   event control block
 *
 * RETURNS:     0 for success, otherwise 1.
 *
 * NOTES:
 *
 * ============================================================================
 * S_ALL                     E_???                         NO CHANGE
 * ----------------------------------------------------------------------------
 */
static int sccpcc_sem_default (sem_event_t *event)
{
    return (0);
}

/*
 * FUNCTION:    sccpcc_sem_cleanup
 *
 * DESCRIPTION: Handle xx event.
 *
 * PARAMETERS:
 *     event:   event control block
 *
 * RETURNS:     0 for success, otherwise 1.
 *
 * NOTES:
 *
 * ============================================================================
 * S_ALL                     E_CLEANUP                     S_IDLE
 * ----------------------------------------------------------------------------
 */
static int sccpcc_sem_cleanup (sem_event_t *event)
{
    sccpcc_cccb_t *cccb = (sccpcc_cccb_t *)(event->cb);

    sccpcc_free_cccb(cccb);

    return (0);
}

/*
 * FUNCTION:    sccpcc_sem_offhook
 *
 * DESCRIPTION: Handle offhook event. Setup initial cb data and send offhook to
 *              CCM.
 *
 * PARAMETERS:
 *     event:   event control block
 *
 * RETURNS:     0 for success, otherwise 1.
 *
 * NOTES:
 *
 * ============================================================================
 * S_IDLE                    E_OFFHOOK                     S_CALL_INITIATED
 * ----------------------------------------------------------------------------
 */
static int sccpcc_sem_offhook (sem_event_t *event)
{
    sccpcc_cccb_t        *cccb = (sccpcc_cccb_t *)(event->cb);
    sccpcc_msg_offhook_t *msg  = (sccpcc_msg_offhook_t *)(event->data);
    
    cccb->line      = msg->line;
    cccb->sccp_line = msg->line;

    /*
     * Wait until we get the callState message to set the sccp_call_id.
     */
    //cccb->sccp_call_id = ;

    /*
     * Make sure we are connected to a CM.
     */
    cccb->socket = sccpcm_get_primary_socket(cccb->sccpcb);
    if (cccb->socket == 0) {
        SCCP_RELEASE_COMPLETE(cccb,
                                     GAPI_MSG_SETUP_ACK,
                                     cccb->id, cccb->line,
                                     GAPI_CAUSE_CM_CONNECT_FAILURE,
                                     &(cccb->precedence));

        sccpcc_push_cleanup(cccb->sccpcb, SCCPCC_E_CLEANUP, msg->conn_id,
                            msg->line, GAPI_CAUSE_OK);

        return (0);
    } 

    sccpmsg_send_offhook(&sccpcc_msgcb, cccb->socket,
                         cccb->sccp_line, cccb->sccp_call_id);

    return (0);
}

/*
 * FUNCTION:    sccpcc_sem_idle_feature_req
 *
 * DESCRIPTION: Handle feature_req event. Setup initial cb data and
 *              send feature to CCM.
 *
 * PARAMETERS:
 *     event:   event control block
 *
 * RETURNS:     0 for success, otherwise 1.
 *
 * NOTES:
 *
 * ============================================================================
 * S_IDLE                    E_FEATURE_REQ                 S_CALL_INITIATED
 * ----------------------------------------------------------------------------
 */
static int sccpcc_sem_idle_feature_req (sem_event_t *event)
{
    sccpcc_cccb_t            *cccb = (sccpcc_cccb_t *)(event->cb);
    sccpcc_msg_feature_req_t *msg  = (sccpcc_msg_feature_req_t *)(event->data);
    
    cccb->line      = msg->line;
    cccb->sccp_line = msg->line;

    /*
     * Wait until we get the callState message to set the sccp_call_id.
     */
    //cccb->sccp_call_id = ;

    /*
     * Make sure we are connected to a CM.
     */
    cccb->socket = sccpcm_get_primary_socket(cccb->sccpcb);
    if (cccb->socket == 0) {
        SCCP_RELEASE_COMPLETE(cccb,
                                     GAPI_MSG_SETUP_ACK,
                                     cccb->id, cccb->line,
                                     GAPI_CAUSE_CM_CONNECT_FAILURE,
                                     &(cccb->precedence));

        sccpcc_push_cleanup(cccb->sccpcb, SCCPCC_E_CLEANUP, msg->conn_id,
                            msg->line, GAPI_CAUSE_OK);

        return (0);
    } 

    sccpmsg_send_softkey_event(&sccpcc_msgcb, cccb->socket,
                               sccpmsg_gapi_to_sccp_feature(msg->feature),
                               cccb->sccp_line,
                               cccb->sccp_call_id);
#if SCCPCC_JUST_ACK_FEATURE
    SCCP_FEATURE_RES(cccb,
                            GAPI_MSG_FEATURE_RES, cccb->id, cccb->line,
                            msg->feature, NULL, GAPI_CAUSE_OK);
#endif                          

    return (0);
}

/*
 * FUNCTION:    sccpcc_sem_xxx_offhook
 *
 * DESCRIPTION: Handle offhook event during a non-IDLE state. Send offhook
 *              to CCM.
 *
 * PARAMETERS:
 *     event:   event control block
 *
 * RETURNS:     0 for success, otherwise 1.
 *
 * NOTES:
 *
 * ============================================================================
 * S_INCOMING_ALERTING       E_OFFHOOK                     NO CHANGE
 * ----------------------------------------------------------------------------
 * S_RESUMING                E_OFFHOOK                     NO CHANGE
 * ----------------------------------------------------------------------------
 */
static int sccpcc_sem_xxx_offhook (sem_event_t *event)
{
    sccpcc_cccb_t *cccb = (sccpcc_cccb_t *)(event->cb);
    
    sccpmsg_send_offhook(&sccpcc_msgcb, cccb->socket,
                         cccb->sccp_line, cccb->sccp_call_id);

    return (0);
}

/*
 * FUNCTION:    sccpcc_sem_setup
 *
 * DESCRIPTION: Handle setup event. Setup initial cb data and send offhook to
 *              CCM. Push digits (if any) back onto the event queue.
 *
 * PARAMETERS:
 *     event:   event control block
 *
 * RETURNS:     0 for success, otherwise 1.
 *
 * NOTES:
 *
 * ============================================================================
 * S_IDLE                    E_SETUP                       S_CALL_INITIATED
 * ----------------------------------------------------------------------------
 */
static int sccpcc_sem_setup (sem_event_t *event)
{
    sccpcc_cccb_t      *cccb  = (sccpcc_cccb_t *)(event->cb);
    sccpcc_msg_setup_t *msg   = (sccpcc_msg_setup_t *)(event->data);

    cccb->line      = msg->line;
    cccb->sccp_line = msg->line;

    /*
     * Wait until we get the call_state message to set the sccp_call_id.
     */
    //cccb->sccp_call_id = ;

    /*
     * Make sure we are connected to a CM.
     */
    cccb->socket = sccpcm_get_primary_socket(cccb->sccpcb);
    if (cccb->socket == 0) {
        SCCP_RELEASE_COMPLETE(cccb,
                                     GAPI_MSG_SETUP_ACK,
                                     cccb->id, cccb->line,
                                     GAPI_CAUSE_CM_CONNECT_FAILURE,
                                     &(cccb->precedence));

        sccpcc_push_cleanup(cccb->sccpcb, SCCPCC_E_CLEANUP, msg->conn_id,
                            msg->line, GAPI_CAUSE_OK);

        return (0);
    } 

    sccpmsg_send_offhook(&sccpcc_msgcb, cccb->socket,
                         cccb->sccp_line, cccb->sccp_call_id);

#if 0
    /*
     * Requeue the digits so that the sem can process them. This allows the
     * offhook and setup events to follow the same behaviour.
     */
    if ((msg->numdigits > 0) && (msg->digits[0] != '\0')) {
        sccpcc_push_digit(cccb->sccpcb, SCCPCC_E_DIGIT, cccb->id, cccb->line,
                          msg->digit, msg->numdigits);
    }
#endif
    /*
     * Copy the digits to temp storage. The digits can not be sent until 
     * the CCM has moved the client to the OFFHOOK state.
     */
    if ((msg->numdigits > 0) && (msg->digits[0] != '\0')) {
        cccb->numdigits = msg->numdigits;
        SCCP_STRNCPY(cccb->digits, msg->digits, msg->numdigits);
        cccb->digits[GAPI_DIRECTORY_NUMBER_SIZE] = '\0';
    }

    return (0);
}

/*
 * FUNCTION:    sccpcc_sem_digits
 *
 * DESCRIPTION: Handle digits event. Send keypad_button to CCM.
 *
 * PARAMETERS:
 *     event:   event control block
 *
 * RETURNS:     0 for success, otherwise 1.
 *
 * NOTES:       Application should not send digits until it receives 
 *              a setup_ack.
 *
 * ============================================================================
 * S_CALL_INITIATED          E_DIGITS                      S_CALL_INITIATED
 * S_CALL_CONNECTED          E_DIGITS                      S_CALL_CONNECTED
 * ----------------------------------------------------------------------------
 */
static int sccpcc_sem_digits (sem_event_t *event)
{
    sccpcc_cccb_t      *cccb = (sccpcc_cccb_t *)(event->cb);
    sccpcc_msg_digits_t *msg = (sccpcc_msg_digits_t *)(event->data);
    int                i;

    for (i = 0; i < msg->numdigits; i++) {
        sccpmsg_send_keypress(&sccpcc_msgcb, cccb->socket,
                              sccpcc_gapi_to_sccp_digit(msg->digits[i]),
                              cccb->sccp_line, cccb->sccp_call_id);
    }

    return (0);
}

/*
 * FUNCTION:    sccpcc_sem_cs_offhook
 *
 * DESCRIPTION: Handle call_state setup event. An incoming call has been
 *              received. Setup cb data and inform the application of the 
 *              incoming call.
 *
 * PARAMETERS:
 *     event:   event control block
 *
 * RETURNS:     0 for success, otherwise 1.
 *
 * NOTES:
 *
 * ============================================================================
 * S_IDLE                    E_CS_OFFHOOK                  S_INCOMING_ALERTING
 * ----------------------------------------------------------------------------
 */
static int sccpcc_sem_cs_offhook (sem_event_t *event)
{
    sccpcc_cccb_t *cccb = (sccpcc_cccb_t *)(event->cb);

    cccb->line = (int)(cccb->sccp_line);

    /*
     * Make sure we are ocnnected to a primary.
     */
    cccb->socket = sccpcm_get_primary_socket(cccb->sccpcb);
    if (cccb->socket == 0) {
        sccpcc_push_cleanup(cccb->sccpcb, SCCPCC_E_CLEANUP, cccb->id,
                            cccb->line, GAPI_CAUSE_OK);

        return (0);
    }

    /*
     * Inform the application of the new incoming call.
     */
    SCCP_OFFHOOK(cccb,
                        GAPI_MSG_OFFHOOK, cccb->id, cccb->line);

    return (0);
}

/*
 * FUNCTION:    sccpcc_sem_cs_setup
 *
 * DESCRIPTION: Handle call_state setup event. An incoming call has been
 *              received. Setup cb data and inform the application of the 
 *              incoming call.
 *
 * PARAMETERS:
 *     event:   event control block
 *
 * RETURNS:     0 for success, otherwise 1.
 *
 * NOTES:
 *
 * ============================================================================
 * S_IDLE                    E_CS_SETUP                    S_INCOMING_ALERTING
 * ----------------------------------------------------------------------------
 */
static int sccpcc_sem_cs_setup (sem_event_t *event)
{
    sccpcc_cccb_t *cccb = (sccpcc_cccb_t *)(event->cb);

    cccb->line = (int)(cccb->sccp_line);

    /*
     * Make sure we are ocnnected to a primary.
     */
    cccb->socket = sccpcm_get_primary_socket(cccb->sccpcb);
    if (cccb->socket == 0) {
        sccpcc_push_cleanup(cccb->sccpcb, SCCPCC_E_CLEANUP, cccb->id,
                            cccb->line, GAPI_CAUSE_OK);

        return (0);
    }

    /*
     * Inform the application of the new incoming call.
     */
    SCCP_SETUP(cccb,
               GAPI_MSG_SETUP, cccb->id, cccb->line,
               NULL, NULL, 0, NULL, 0,
               cccb->privacy, &(cccb->precedence));

    return (0);
}

/*
 * FUNCTION:    sccpcc_sem_cs_proceeding
 *
 * DESCRIPTION: Handle call_state proceeding event. The CCM has acknowledged
 *              that it has enough information to proceed with the call. Inform
 *              the application.
 *
 * PARAMETERS:
 *     event:   event control block
 *
 * RETURNS:     0 for success, otherwise 1.
 *
 * NOTES:
 *
 * ============================================================================
 * S_CALL_INITIATED          E_CS_PROCEEDINF               S_OUTGOING_PROCEEDING
 * ----------------------------------------------------------------------------
 */
static int sccpcc_sem_cs_proceeding (sem_event_t *event)
{
    sccpcc_cccb_t *cccb = (sccpcc_cccb_t *)(event->cb);

    /*
     * Inform the application that CCM has enough information to proceed with
     * the call.
     */
    SCCP_PROCEEDING(cccb,
                           GAPI_MSG_PROCEEDING,
                           cccb->id, cccb->line, NULL, &(cccb->precedence));

    return (0);
}

/*
 * FUNCTION:    sccpcc_sem_cs_alerting
 *
 * DESCRIPTION: Handle call_state alerting event. The farend is ringing. Inform
 *              the application.
 *
 * PARAMETERS:
 *     event:   event control block
 *
 * RETURNS:     0 for success, otherwise 1.
 *
 * NOTES:
 *
 * ============================================================================
 * S_CALL_INITIATED          E_CS_ALERTING                 S_OUTGOING_ALERTING
 * ----------------------------------------------------------------------------
 * S_OUTGOING_PROCEEDING     E_CS_ALERTING                 S_OUTGOING_ALERTING
 * ----------------------------------------------------------------------------
 */
static int sccpcc_sem_cs_alerting (sem_event_t *event)
{
    sccpcc_cccb_t *cccb = (sccpcc_cccb_t *)(event->cb);

    /*
     * Inform the application that the farend is ringing.
     */
    SCCP_ALERTING(cccb,
                  GAPI_MSG_ALERTING,
                  cccb->id, cccb->line, NULL, NULL, 0,
                  &(cccb->precedence));

    return (0);
}

/*
 * FUNCTION:    sccpcc_sem_oa_connect
 *
 * DESCRIPTION: Handle call_state connect event during S_OUTGOING_ALERTING.
 *              The farend has answered the call. Send connect to application.
 *
 * PARAMETERS:
 *     event:   event control block
 *
 * RETURNS:     0 for success, otherwise 1.
 *
 * NOTES:
 *
 * ============================================================================
 * S_CALL_INITIATED          E_CS_CONNECT                  S_CONNECTED
 * ----------------------------------------------------------------------------
 * S_OUTGOING_PROCEEDING     E_CS_CONNECT                  S_CONNECTED
 * ----------------------------------------------------------------------------
 * S_OUTGOING_ALERTING       E_CS_CONNECT                  S_CONNECTED
 * ----------------------------------------------------------------------------
 */
static int sccpcc_sem_oa_cs_connected (sem_event_t *event)
{
    sccpcc_cccb_t *cccb = (sccpcc_cccb_t *)(event->cb);

    /*
     * Inform the application that the farend has answered the call.
     */
    SCCP_CONNECT(cccb,
                 GAPI_MSG_CONNECT, cccb->id, cccb->line,
                 NULL, NULL, &(cccb->precedence));

    return (0);
}

/*
 * FUNCTION:    sccpcc_sem_ic_cs_connected
 *
 * DESCRIPTION: Handle call_state offhook event. The CCM has awarded the call
 *              to this client. Inform the application.
 *
 * PARAMETERS:
 *     event:   event control block
 *
 * RETURNS:     0 for success, otherwise 1.
 *
 * NOTES:
 *
 * ============================================================================
 * S_INCOMING_CONNECTING     E_CS_CONNECTED                S_CONNECTED
 * ----------------------------------------------------------------------------
 */
static int sccpcc_sem_ic_cs_connected (sem_event_t *event)
{
    sccpcc_cccb_t *cccb = (sccpcc_cccb_t *)(event->cb);

    SCCP_CONNECT_ACK(cccb,
                     GAPI_MSG_CONNECT_ACK,
                     cccb->id, cccb->line, NULL, NULL,
                     &(cccb->precedence));

    return (0);
}

/*
 * FUNCTION:    sccpcc_sem_cs_release
 *
 * DESCRIPTION: Handle call_state release event. The CCM is trying to clear the
 *              call. Inform the application.
 *
 * PARAMETERS:
 *     event:   event control block
 *
 * RETURNS:     0 for success, otherwise 1.
 *
 * NOTES:
 *
 * ============================================================================
 * S_OUTGOING_PROCEEDING     E_CS_RELEASE                  S_INCOMING_RELEASING
 * ----------------------------------------------------------------------------
 * S_OUTGOING_ALERTING       E_CS_RELEASE                  S_INCOMING_RELEASING
 * ----------------------------------------------------------------------------
 * S_INCOMING_ALERTING       E_CS_RELEASE                  S_INCOMING_RELEASING
 * ----------------------------------------------------------------------------
 * S_INCOMING_CONNECTING     E_CS_RELEASE                  S_INCOMING_RELEASING
 * ----------------------------------------------------------------------------
 * S_CONNECTED               E_CS_RELEASE                  S_INCOMING_RELEASING
 * ----------------------------------------------------------------------------
 */
static int sccpcc_sem_cs_release (sem_event_t *event)
{
    sccpcc_cccb_t *cccb = (sccpcc_cccb_t *)(event->cb);

    /*
     * Inform the appication that the CCM wants to clear the call.
     */
    SCCP_RELEASE(cccb,
                 GAPI_MSG_RELEASE,
                 cccb->id, cccb->line, GAPI_CAUSE_OK,
                 &(cccb->precedence));

    return (0);
}

/*
 * FUNCTION:    sccpcc_sem_call_state
 *
 * DESCRIPTION: Handle call_state event. Perform actions applicable for the 
 *              call_state or translate the event to a more usable sccpcc
 *              event.
 *
 * PARAMETERS:
 *     event:   event control block
 *
 * RETURNS:     0 for success, otherwise 1.
 *
 * NOTES:
 *
 * ============================================================================
 * S_ALL                     E_CALL_STATE                  NO CHANGE
 * ----------------------------------------------------------------------------
 */
static int sccpcc_sem_call_state (sem_event_t *event)
{
    sccpcc_cccb_t        *cccb    = (sccpcc_cccb_t *)(event->cb);
    sccpmsg_general_t    *gen_msg = (sccpmsg_general_t *)(event->data);
    sccpmsg_call_state_t *msg     = &(gen_msg->body.call_state);
    int                  i;

    /*
     * Copy the precedence data. CCM controls the values so just overwrite
     * the current values.
     */
    cccb->precedence.domain = msg->precedence.domain;
    cccb->precedence.level  = msg->precedence.level;

    switch (msg->call_state) {
    case (SCCPMSG_CALL_STATE_OFFHOOK):
        switch (cccb->state) {
#if 1
        case (SCCPCC_S_IDLE):
            /*
             * Features like transfer will open a new call plane starting
             * with the CALL_STATE(offhook) message.
             */
            sccpcc_push_simple(cccb->sccpcb, SCCPCC_E_CS_OFFHOOK,
                               cccb->id, cccb->line, 1);

            break;
#endif
        case (SCCPCC_S_CALL_INITIATED):
            /*
             * Acknowledge the new outgoing call.
             */
            SCCP_SETUP_ACK(cccb,
                           GAPI_MSG_SETUP_ACK,
                           cccb->id, cccb->line, NULL, NULL,
                           GAPI_CAUSE_OK,
                           &(cccb->precedence));

            /*
             * Send digits if the application has already supplied them.
             */
            for (i = 0; i < cccb->numdigits; i++) {
                sccpmsg_send_keypress(&sccpcc_msgcb, cccb->socket,
                                      sccpcc_gapi_to_sccp_digit(cccb->digits[i]),
                                      cccb->sccp_line, cccb->sccp_call_id);
            }
            cccb->numdigits = 0;

            break;

        default:
            SCCP_OFFHOOK(cccb,
                                GAPI_MSG_OFFHOOK,
                                cccb->id, cccb->line);

            break;
        }

        break;

    case (SCCPMSG_CALL_STATE_ONHOOK):
        switch (cccb->state) {
        case (SCCPCC_S_IDLE):
        case (SCCPCC_S_INCOMING_RELEASING):
            break;

        case (SCCPCC_S_CALL_INITIATED):
        case (SCCPCC_S_OUTGOING_RELEASING):
            SCCP_RELEASE_COMPLETE(cccb,
                                         GAPI_MSG_RELEASE_COMPLETE,
                                         cccb->id, cccb->line,
                                         GAPI_CAUSE_NORMAL,
                                         &(cccb->precedence));

            sccpcc_push_cleanup(cccb->sccpcb, SCCPCC_E_CLEANUP, cccb->id,
                                cccb->line, GAPI_CAUSE_OK);

            break;

        default:
            sccpcc_push_simple(cccb->sccpcb, SCCPCC_E_CS_RELEASE,
                               cccb->id, cccb->line, 1);
        }

        break;

    case (SCCPMSG_CALL_STATE_RING_OUT):
        sccpcc_push_simple(cccb->sccpcb, SCCPCC_E_CS_ALERTING,
                           cccb->id, cccb->line, 1);

        break;

    case (SCCPMSG_CALL_STATE_RING_IN):
        sccpcc_push_simple(cccb->sccpcb, SCCPCC_E_CS_SETUP,
                           cccb->id, cccb->line, 1);

        break;

    case (SCCPMSG_CALL_STATE_CONNECTED):
        sccpcc_push_simple(cccb->sccpcb, SCCPCC_E_CS_CONNECTED,
                           cccb->id, cccb->line, 1);

        break;

    case (SCCPMSG_CALL_STATE_PROCEED):
        sccpcc_push_simple(cccb->sccpcb, SCCPCC_E_CS_PROCEEDING,
                           cccb->id, cccb->line, 1);

        break;
        
    case (SCCPMSG_CALL_STATE_HOLD):
        SCCP_FEATURE_REQ(cccb,
                         GAPI_MSG_FEATURE_REQ, cccb->id, cccb->line,
                         GAPI_FEATURE_HOLD,
                         NULL, GAPI_CAUSE_OK);

        break;

    default:
        break;
    }

    return (0);
}

/*
 * FUNCTION:    sccpcc_sem_openrcv_res
 *
 * DESCRIPTION: Handle openrcv_res event. The stack has requested an
 *              open_receive_channel. Send open_receive_channel_ack
*               to the CCM.
 *
 * PARAMETERS:
 *     event:   event control block
 *
 * RETURNS:     0 for success, otherwise 1.
 *
 * NOTES:
 *
 * ============================================================================
 * S_ALL                     E_OPENRECV_RES                NO CHANGE
 * ----------------------------------------------------------------------------
 */
static int sccpcc_sem_openrcv_res (sem_event_t *event)
{
    sccpcc_cccb_t            *cccb  = (sccpcc_cccb_t *)(event->cb);
    sccpcc_msg_openrcv_res_t *msg   = (sccpcc_msg_openrcv_res_t *)(event->data);
    sccpmsg_orc_status_t     status;
    
    if (msg->cause == GAPI_CAUSE_OK) {
        status = SCCPMSG_ORC_STATUS_OK;
    }
    else {
        status = SCCPMSG_ORC_STATUS_ERROR;
    }

    sccpmsg_send_open_receive_channel_ack(
        &sccpcc_msgcb,
        cccb->socket,
        msg->media.sccp_media.addr, /* in network order */
        SCCP_NTOHL(msg->media.sccp_media.port), /* in host order */
        status,
        msg->media.sccp_media.passthruparty_id,
        cccb->sccp_call_id);

    return (0);
}

/*
 * FUNCTION:    sccpcc_sem_feature_req
 *
 * DESCRIPTION: Handle feature_req event. The application has requested a
 *              specific feature.
 *
 * PARAMETERS:
 *     event:   event control block
 *
 * RETURNS:     0 for success, otherwise 1.
 *
 * NOTES:
 *
 * ============================================================================
 * S_ALL                     E_FEATURE_REQ                 NO CHANGE
 * ----------------------------------------------------------------------------
 */
static int sccpcc_sem_feature_req (sem_event_t *event)
{
    sccpcc_cccb_t            *cccb = (sccpcc_cccb_t *)(event->cb);
    sccpcc_msg_feature_req_t *msg  = (sccpcc_msg_feature_req_t *)(event->data);
    
    sccpmsg_send_softkey_event(&sccpcc_msgcb, cccb->socket,
                               sccpmsg_gapi_to_sccp_feature(msg->feature),
                               cccb->sccp_line,
                               cccb->sccp_call_id);
#if SCCPCC_JUST_ACK_FEATURE
    SCCP_FEATURE_RES(cccb,
                            GAPI_MSG_FEATURE_RES, cccb->id, cccb->line,
                            msg->feature, NULL, GAPI_CAUSE_OK);
#endif                          
    return (0);
}

/*
 * FUNCTION:    sccpcc_sem_connect
 *
 * DESCRIPTION: Handle connect event. The application has answered an incoming
 *              call
 *
 * PARAMETERS:
 *     event:   event control block
 *
 * RETURNS:     0 for success, otherwise 1.
 *
 * NOTES:
 *
 * ============================================================================
 * S_INCOMING_ALERTING       E_CONNECT                     S_INCOMING_CONNECTING
 * ----------------------------------------------------------------------------
 * S_OFFHOOK                 E_CONNECT                     S_INCOMING_CONNECTING
 * ----------------------------------------------------------------------------
 */
static int sccpcc_sem_connect (sem_event_t *event)
{
    sccpcc_cccb_t *cccb = (sccpcc_cccb_t *)(event->cb);
    
    sccpmsg_send_offhook(&sccpcc_msgcb, cccb->socket,
                         cccb->sccp_line, cccb->sccp_call_id);

    return (0);
}

#if 0 /* not  used yet */
/*
 * FUNCTION:    sccpcc_sem_connect_ack
 *
 * DESCRIPTION: Handle connect_ack event. The application has accepted that
 *              the farend has answered the outgoing call.
 *
 * PARAMETERS:
 *     event:   event control block
 *
 * RETURNS:     0 for success, otherwise 1.
 *
 * NOTES:
 *
 * ============================================================================
 * S_OUTGOING_ALERTING       E_CONNECT_ACK                 S_CONNECTED
 * ----------------------------------------------------------------------------
 */
static int sccpcc_sem_connect_ack (sem_event_t *event)
{
    return (0);
}
#endif

/*
 * FUNCTION:    sccpcc_sem_release
 *
 * DESCRIPTION: Handle release event. The application is attempting to clear
 *              the call. Send onhook to the CCM.
 *
 * PARAMETERS:
 *     event:   event control block
 *
 * RETURNS:     0 for success, otherwise 1.
 *
 * NOTES:
 *
 * ============================================================================
 * S_CALL_INITIATED          E_RELEASE                     S_OUTGOING_RELEASING
 * ----------------------------------------------------------------------------
 * S_CALL_INITIATED          E_ONHOOK                      S_OUTGOING_RELEASING
 * ----------------------------------------------------------------------------
 * S_OUTGOING_ALERTING       E_RELEASE                     S_OUTGOING_RELEASING
 * ----------------------------------------------------------------------------
 * S_OUTGOING_ALERTING       E_ONHOOK                      S_OUTGOING_RELEASING
 * ----------------------------------------------------------------------------
 * S_INCOMING_ALERTING       E_RELEASE                     S_OUTGOING_RELEASING
 * ----------------------------------------------------------------------------
 * S_INCOMING_CONNECTING     E_RELEASE                     S_OUTGOING_RELEASING
 * ----------------------------------------------------------------------------
 * S_INCOMING_CONNECTING     E_ONHOOK                      S_OUTGOING_RELEASING
 * ----------------------------------------------------------------------------
 * S_CONNECTED               E_RELEASE                     S_OUTGOING_RELEASING
 * ----------------------------------------------------------------------------
 * S_CONNECTED               E_ONHOOK                      S_OUTGOING_RELEASING
 * ----------------------------------------------------------------------------
 */
static int sccpcc_sem_release (sem_event_t *event)
{
    sccpcc_cccb_t *cccb = (sccpcc_cccb_t *)(event->cb);
    
    sccpmsg_send_onhook(&sccpcc_msgcb, cccb->socket,
                        cccb->sccp_line, cccb->sccp_call_id);

    return (0);
}

/*
 * FUNCTION:    sccpcc_sem_ia_release_complete
 *
 * DESCRIPTION: Handle release_complete event from the application. The
 *              application is attempting to release a new incoming call. Send
 *              onhook to the CCM and cleanup.
 *
 * PARAMETERS:
 *     event:   event control block
 *
 * RETURNS:     0 for success, otherwise 1.
 *
 * NOTES:
 *
 * ============================================================================
 * S_INCOMING_ALERTING       E_RELEASE_COMPLETE            S_INCOMING_ALERTING
 * ----------------------------------------------------------------------------
 */
static int sccpcc_sem_ia_release_complete (sem_event_t *event)
{
    sccpcc_cccb_t *cccb = (sccpcc_cccb_t *)(event->cb);
    
    sccpmsg_send_onhook(&sccpcc_msgcb, cccb->socket,
                        cccb->sccp_line, cccb->sccp_call_id);
    
    /*
     * Application has rejected the new incoming call, so clean up.
     */
    sccpcc_push_cleanup(cccb->sccpcb, SCCPCC_E_CLEANUP, cccb->id,
                        cccb->line, GAPI_CAUSE_OK);

    return (0);
}

/*
 * FUNCTION:    sccpcc_sem_release_complete
 *
 * DESCRIPTION: Handle release_complete event from the application. Cleanup
 *              the call because this should be the final responce to a call
 *              clearing.
 *
 * PARAMETERS:
 *     event:   event control block
 *
 * RETURNS:     0 for success, otherwise 1.
 *
 * NOTES:
 *
 * ============================================================================
 * NOT USED
 * ----------------------------------------------------------------------------
 */
static int sccpcc_sem_release_complete (sem_event_t *event)
{
    sccpcc_cccb_t *cccb = (sccpcc_cccb_t *)(event->cb);
    
    sccpcc_push_cleanup(cccb->sccpcb, SCCPCC_E_CLEANUP, cccb->id,
                        cccb->line, GAPI_CAUSE_OK);

    return (0);
}

/*
 * FUNCTION:    sccpcc_sem_or_release
 *
 * DESCRIPTION: Handle release event from the application .
 *
 * PARAMETERS:
 *     event:   event control block
 *
 * RETURNS:     0 for success, otherwise 1.
 *
 * NOTES:
 *
 * ============================================================================
 * S_OUTGOING_PROCEEDING     E_RELEASE                     S_IDLE
 * ----------------------------------------------------------------------------
 * S_OUTGOING_PROCEEDING     E_ONHOOK                      S_IDLE
 * ----------------------------------------------------------------------------
 * S_OUTGOING_RELEASING      E_RELEASE                     S_IDLE
 * ----------------------------------------------------------------------------
 * S_OUTGOING_RELEASING      E_ONHOOK                      S_IDLE
 * ----------------------------------------------------------------------------
 */
static int sccpcc_sem_or_release (sem_event_t *event)
{
    sccpcc_cccb_t *cccb = (sccpcc_cccb_t *)(event->cb);
    
    SCCP_RELEASE_COMPLETE(cccb,
                                 GAPI_MSG_RELEASE_COMPLETE,
                                 cccb->id, cccb->line,
                                 GAPI_CAUSE_OK,
                                 &(cccb->precedence));

    sccpmsg_send_onhook(&sccpcc_msgcb, cccb->socket,
                        cccb->sccp_line, cccb->sccp_call_id);

    sccpcc_push_cleanup(cccb->sccpcb, SCCPCC_E_CLEANUP, cccb->id,
                        cccb->line, GAPI_CAUSE_OK);

    return (0);
}

/*
 * FUNCTION:    sccpcc_sem_media
 *
 * DESCRIPTION: Handle media events from the CCM. Inform the application of
 *              the event.
 *
 * PARAMETERS:
 *     event:   event control block
 *
 * RETURNS:     0 for success, otherwise 1.
 *
 * NOTES:
 *
 * ============================================================================
 * S_ALL                     E_MEDIA                       NO CHANGE
 * ----------------------------------------------------------------------------
 */
static int sccpcc_sem_media (sem_event_t *event)
{
    sccpcc_cccb_t        *cccb     = (sccpcc_cccb_t *)(event->cb);
    sccpmsg_general_t    *gen_msg  = (sccpmsg_general_t *)(event->data);
    sccpmsg_message_id_t *msg_id   = &(gen_msg->body.msg_id);
    gapi_media_t         media;

    SCCP_DBG((sccpcc_debug, 9, "%s %-2d:%-3d:%d: %-20s: msg= %s\n",
             SCCPCC_ID, cccb->sccpcb->id,
             cccb->id, cccb->line,
             event->function_name,
             sccpmsg_get_message_text(msg_id->message_id)));

    switch (msg_id->message_id) {
    case (SCCPMSG_OPEN_RECEIVE_CHANNEL): {
        sccpmsg_open_receive_channel_t *msg = &(gen_msg->body.open_receive_channel);
   
        sccpcc_copy_open_recv_media(&media, msg);

        SCCP_OPENRCV_REQ(cccb,
                         GAPI_MSG_OPENRCV_REQ, cccb->id, cccb->line,
                         &media);

        break;
    }

    case (SCCPMSG_CLOSE_RECEIVE_CHANNEL): {
        sccpmsg_close_receive_channel_t *msg = &(gen_msg->body.close_receive_channel);
    
        media.sccp_media.conference_id    = msg->conference_id;
        media.sccp_media.passthruparty_id = msg->passthru_party_id;

        SCCP_CLOSERCV(cccb,
                      GAPI_MSG_CLOSERCV, cccb->id, cccb->line,
                      &media);

        break;
    }

    case (SCCPMSG_START_MEDIA_TRANSMISSION): {
        sccpmsg_start_media_transmission_t *msg = &(gen_msg->body.start_media_transmission);
            
        sccpcc_copy_start_xmit_media(&media, msg);

        SCCP_STARTXMIT(cccb,
                       GAPI_MSG_STARTXMIT, cccb->id, cccb->line,
                       &media);

        break;
    }

    case (SCCPMSG_STOP_MEDIA_TRANSMISSION): {
        sccpmsg_stop_media_transmission_t *msg     = &(gen_msg->body.stop_media_transmission);
    
        media.sccp_media.conference_id    = msg->conference_id;
        media.sccp_media.passthruparty_id = msg->passthru_party_id;

        SCCP_STOPXMIT(cccb,
                      GAPI_MSG_STOPXMIT, cccb->id, cccb->line,
                      &media);

        break;
    }

    default:
        break;
    }

    return (0);
}

/*
 * FUNCTION:    sccpcc_sem_update_ui
 *
 * DESCRIPTION: Handle various UI events. 
 *
 * PARAMETERS:
 *     event:   event control block
 *
 * RETURNS:     0 for success, otherwise 1.
 *
 * NOTES:
 *
 * ============================================================================
 * S_ALL                     E_UPDATE_UI                   NO CHANGE
 * ----------------------------------------------------------------------------
 */
static int sccpcc_sem_update_ui (sem_event_t *event)
{
    sccpcc_cccb_t        *cccb    = (sccpcc_cccb_t *)(event->cb);
    sccpmsg_general_t    *gen_msg = (sccpmsg_general_t *)(event->data);
    sccpmsg_message_id_t *msg_id  = &(gen_msg->body.msg_id);
    
    SCCP_DBG((sccpcc_debug, 9, "%s %-2d:%-3d:%d: %-20s: msg= %s\n",
             SCCPCC_ID, cccb->sccpcb->id,
             cccb->id, cccb->line,
             event->function_name,
             sccpmsg_get_message_text(msg_id->message_id)));

    switch (msg_id->message_id) {
    case (SCCPMSG_START_TONE): {
        sccpmsg_start_tone_t *msg =
            (sccpmsg_start_tone_t *)&(gen_msg->body.start_tone);
        
        SCCP_STARTTONE(cccb,
                       GAPI_MSG_STARTTONE, cccb->id, cccb->line,
                       sccpmsg_sccp_to_gapi_tone(msg->tone),
                       sccpmsg_sccp_to_gapi_tone_direction(msg->direction));

#if 0 /* Let the application send onhook when it receives the
        start_tone(busytone) from the stack. */

        /*
         * start_tone with reorder tone is the first message CCM will send
         * if an outgoing call setup failed, so let's translate it
         * to a CS_RELEASE.
         */
        if (msg->tone == SCCPMSG_TONE_REORDER) {
            sccpcc_push_simple(cccb->sccpcb, SCCPCC_E_CS_RELEASE, cccb->id,
                               cccb->line, 1);
        }
#endif
        break;
    }

    case (SCCPMSG_STOP_TONE): {
        SCCP_STOPTONE(cccb, GAPI_MSG_STOPTONE, cccb->id, cccb->line);

        break;
    }

    case (SCCPMSG_SET_RINGER): {
        sccpmsg_set_ringer_t *msg =
            (sccpmsg_set_ringer_t *)&(gen_msg->body.set_ringer);
        
        SCCP_RINGER(cccb,
                    GAPI_MSG_RINGER, cccb->id, cccb->line,
                    sccpmsg_sccp_to_gapi_ringer(msg->ring_mode),
                    sccpmsg_sccp_to_gapi_ring_duration(msg->ring_duration));

        break;
    }

    case (SCCPMSG_SET_LAMP): {
        sccpmsg_set_lamp_t *msg =
            (sccpmsg_set_lamp_t *)&(gen_msg->body.set_lamp);
        
        SCCP_LAMP(cccb,
                  GAPI_MSG_LAMP, cccb->id, cccb->line,
                  sccpmsg_sccp_to_gapi_stimulus(msg->stimulus),
                  sccpmsg_sccp_to_gapi_lamp_mode(msg->lamp_mode));

        break;
    }

    case (SCCPMSG_SET_SPEAKER_MODE): {
        sccpmsg_set_speaker_mode_t *msg =
            (sccpmsg_set_speaker_mode_t *)&(gen_msg->body.set_speaker_mode);
        
        SCCP_SPEAKER(cccb,
                     GAPI_MSG_SPEAKER, cccb->id, cccb->line,
                     sccpmsg_sccp_to_gapi_ringer(msg->mode));

        break;
    }

    case (SCCPMSG_SET_MICRO_MODE): {
        sccpmsg_set_micro_mode_t *msg =
            (sccpmsg_set_micro_mode_t *)&(gen_msg->body.set_micro_mode);
        
        SCCP_MICRO(cccb,
                   GAPI_MSG_MICRO, cccb->id, cccb->line,
                   sccpmsg_sccp_to_gapi_micro(msg->mode));

        break;
    }

    case (SCCPMSG_CALL_INFO): {
        sccpmsg_call_info_t *msg =
            (sccpmsg_call_info_t *)&(gen_msg->body.call_info);
        gapi_conninfo_t     conninfo;

        sccpcc_copy_call_info(&conninfo, msg);

        SCCP_CONNINFO(cccb,
                      GAPI_MSG_CONNINFO, cccb->id, cccb->line,
                      &conninfo);
        break;
    }

    case (SCCPMSG_DEFINE_TIME_DATE): {
        sccpmsg_define_time_date_t *msg =
            (sccpmsg_define_time_date_t *)&(gen_msg->body.define_time_date);
        
        SCCP_TIMEDATE(cccb,
                      GAPI_MSG_TIMEDATE, cccb->id, cccb->line,
                      msg->system_time);

        break;
    }

#if 0 /* NOT USED YET */
    case (SCCPMSG_VERSION): {
        sccpmsg_version_t *msg =
            (sccpmsg_version_t *)&(gen_msg->body.version);
        
        SCCP_VERSION_RES(cccb,
                         GAPI_MSG_VERSION_RES, cccb->id, cccb->line,
                         msg->version);

        break;
    }
#endif
    case (SCCPMSG_DISPLAY_TEXT): {
        sccpmsg_display_text_t *msg =
            (sccpmsg_display_text_t *)&(gen_msg->body.display_text);
        
        SCCP_DISPLAY(cccb,
                     GAPI_MSG_DISPLAY, cccb->id, cccb->line,
                     msg->text);

        break;
    }

    case (SCCPMSG_CLEAR_DISPLAY): {
        SCCP_CLEARDISPLAY(cccb,
                          GAPI_MSG_CLEARDISPLAY, cccb->id, cccb->line);

        break;
    }

    case (SCCPMSG_DISPLAY_PROMPT_STATUS): {
        sccpmsg_display_prompt_status_t *msg =
            (sccpmsg_display_prompt_status_t *)&(gen_msg->body.display_prompt_status);
        
        SCCP_PROMPT(cccb,
                    GAPI_MSG_PROMPT, cccb->id, cccb->line,
                    msg->text, msg->timeout);

        break;
    }

    case (SCCPMSG_CLEAR_PROMPT_STATUS): {
        SCCP_CLEARPROMPT(cccb,
                         GAPI_MSG_CLEARPROMPT, cccb->id, cccb->line);

        break;
    }

    case (SCCPMSG_DISPLAY_NOTIFY): {
        sccpmsg_display_notify_t *msg =
            (sccpmsg_display_notify_t *)&(gen_msg->body.display_notify);
        
        SCCP_NOTIFY(cccb,
                    GAPI_MSG_NOTIFY, cccb->id, cccb->line,
                    msg->text, msg->timeout);

        break;
    }

    case (SCCPMSG_CLEAR_NOTIFY): {
        SCCP_CLEARNOTIFY(cccb,
                         GAPI_MSG_CLEARNOTIFY, cccb->id, cccb->line);

        break;
    }

    case (SCCPMSG_CONNECTION_STATISTICS_REQ): {
        sccpmsg_connection_statistics_req_t *msg =
            (sccpmsg_connection_statistics_req_t *)&(gen_msg->body.connection_statistics_req);
        gapi_connection_stats_t             stats;
        
        stats.call_identifier = cccb->id;
        SCCP_STRNCPY(stats.directory_number, msg->directory_number,
                     sizeof(stats.directory_number));
        stats.directory_number[SCCPMSG_DIRECTORY_NUMBER_SIZE] = '\0';
        stats.processing_mode = sccpmsg_sccp_to_gapi_processing_mode(msg->processing_mode);

        SCCP_CONNECTIONSTATS(cccb,
                             GAPI_MSG_CONNECTIONSTATS,
                             cccb->id, cccb->line, &stats);

        sccpmsg_send_connection_stats(&sccpcc_msgcb, cccb->socket,
                                      stats.directory_number,
                                      stats.call_identifier,
                                      stats.processing_mode,
                                      stats.number_packets_sent,
                                      stats.number_bytes_sent,
                                      stats.number_packets_received,
                                      stats.number_bytes_sent,
                                      stats.number_packets_lost,
                                      stats.jitter, stats.latency);

        break;
    }

    case (SCCPMSG_ACTIVATE_CALL_PLANE): {
        SCCP_ACTIVATEPLANE(cccb,
                           GAPI_MSG_ACTIVATEPLANE,
                           cccb->id, cccb->line);

        break;
    }

    case (SCCPMSG_DEACTIVATE_CALL_PLANE): {
        SCCP_DEACTIVATEPLANE(cccb,
                             GAPI_MSG_DEACTIVATEPLANE,
                             cccb->id, cccb->line);

        break;
    }

    case (SCCPMSG_BACKSPACE_REQ): {
        SCCP_BACKSPACE_REQ(cccb,
                           GAPI_MSG_BACKSPACE_REQ, cccb->id,
                           cccb->line);

        break;
    }

    case (SCCPMSG_DIALED_NUMBER): {
        sccpmsg_dialed_number_t *msg =
            (sccpmsg_dialed_number_t *)&(gen_msg->body.dialed_number);

        SCCP_DIALEDNUMBER(cccb,
                          GAPI_MSG_DIALEDNUMBER, cccb->id, cccb->line,
                          msg->dialed_number);

        break;
    }

    case (SCCPMSG_USER_TO_DEVICE_DATA): {
        sccpmsg_user_to_device_data_t *msg =
            (sccpmsg_user_to_device_data_t *)&(gen_msg->body.user_to_device_data);

        SCCP_USERTODEVICEDATA(cccb,
                              GAPI_MSG_USERTODEVICEDATA, cccb->id,
                              cccb->line, 
                              sccpmsg_sccp_to_gapi_data(&(msg->data)));

        break;
    }

    case (SCCPMSG_DISPLAY_PRIORITY_NOTIFY): {
        sccpmsg_display_priority_notify_t *msg =
            (sccpmsg_display_priority_notify_t *)&(gen_msg->body.display_priority_notify);
        
        SCCP_PRIORITYNOTIFY(cccb,
                            GAPI_MSG_PRIORITYNOTIFY, cccb->id,
                            cccb->line, msg->text, msg->timeout,
                            msg->priority);

        break;
    }

    case (SCCPMSG_CLEAR_PRIORITY_NOTIFY): {
        sccpmsg_clear_priority_notify_t *msg =
            (sccpmsg_clear_priority_notify_t *)&(gen_msg->body.clear_priority_notify);
        
        SCCP_CLEARPRIORITYNOTIFY(cccb,
                                 GAPI_MSG_CLEARPRIORITYNOTIFY,
                                 cccb->id, cccb->line, msg->priority);

        break;
    }

    default:
        break;
    }

    return (0);
}

/*
 * FUNCTION:    sccpcc_sem_devicetouserdata_req
 *
 * DESCRIPTION: Handle device_to_user_data event from the application. Send
 *              the data to the CCM.
 *
 * PARAMETERS:
 *     event:   event control block
 *
 * RETURNS:     0 for success, otherwise 1.
 *
 * NOTES:
 *
 * ============================================================================
 * S_ALL                     E_DEVTOUSERDATA_REQ           NO CHANGE
 * ----------------------------------------------------------------------------
 */
int sccpcc_sem_devicetouserdata_req (sem_event_t *event)
{
    sccpcc_cccb_t                     *cccb = (sccpcc_cccb_t *)(event->cb);
    sccpcc_msg_devicetouserdata_req_t *msg  =
        (sccpcc_msg_devicetouserdata_req_t *)(event->data);
    
    cccb->socket = sccpcm_get_primary_socket(cccb->sccpcb);
    if (cccb->socket == 0) {
        return (0);
    } 

    sccpmsg_send_device_to_user_data(&sccpcc_msgcb, cccb->socket,
                                     msg->data.data, msg->data.data_length,
                                     msg->data.application_id,
                                     msg->data.transaction_id,
                                     cccb->sccp_line, cccb->sccp_call_id);

    return (0);
}


/*
 * FUNCTION:    sccpcc_sem_devicetouserdata_res
 *
 * DESCRIPTION: Handle device_to_user_data_resp event from the application.
 *              Send the response to the CCM.
 *
 * PARAMETERS:
 *     event:   event control block
 *
 * RETURNS:     0 for success, otherwise 1.
 *
 * NOTES:
 *
 * ============================================================================
 * S_ALL                     E_DEVTOUSERDATA_RES           NO CHANGE
 * ----------------------------------------------------------------------------
 */
int sccpcc_sem_devicetouserdata_res (sem_event_t *event)
{
    sccpcc_cccb_t                     *cccb = (sccpcc_cccb_t *)(event->cb);
    sccpcc_msg_devicetouserdata_res_t *msg  =
        (sccpcc_msg_devicetouserdata_res_t *)(event->data);
    
    cccb->socket = sccpcm_get_primary_socket(cccb->sccpcb);
    if (cccb->socket == 0) {
        return (0);
    } 

    sccpmsg_send_device_to_user_data_response(&sccpcc_msgcb, cccb->socket,
                                              msg->data.data,
                                              msg->data.data_length,
                                              msg->data.application_id,
                                              msg->data.transaction_id,
                                              cccb->sccp_line,
                                              cccb->sccp_call_id);

    return (0);
}

/*
 * FUNCTION:    sccpcc_sem_init
 *
 * DESCRIPTION: Initialize sccpcc.
 *
 * PARAMETERS:  None.
 *
 * RETURNS:     0 for success, otherwise 1.
 */
int sccpcc_init (void)
{
    sccpcc_msgcb.cb       = NULL;
    sccpcc_msgcb.sem      = SCCP_SEM_CC;
    sccpcc_msgcb.sem_name = SCCPCC_ID;

    return (0);
}

/*
 * FUNCTION:    sccpcc_cleanup
 *
 * DESCRIPTION: Cleanup and free any resources associated with sccpcc.
 *
 * PARAMETERS:  None.
 *
 * RETURNS:     0 for success, otherwise 1.
 */
int sccpcc_cleanup (void)
{
    return (0);
}
