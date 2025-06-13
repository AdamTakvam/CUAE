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
 *     sccpcm.c
 *
 *  CREATION/MODIFICATION DATES
 *     Created:  November 2002, Sam Hague
 *     Modified: 
 *
 *  DESCRIPTION
 *     SCCP CallManager implementation
 */
#include "sccp.h"
#include "sccp_debug.h"
#include "sem.h"
#include "am.h"
#include "sllist.h"
#include "sccpmsg.h"


static sccpmsg_msgcb_t sccpcm_msgcb;


typedef enum sccpcm_states_t_ {
    SCCPCM_S_MIN = -1,
    SCCPCM_S_IDLE,
    SCCPCM_S_WF_CON_RES,
    SCCPCM_S_CONNECTED,
    SCCPCM_S_TOKEN,
    SCCPCM_S_WF_REG_RES,
    SCCPCM_S_WF_CON_RETRY,
    SCCPCM_S_REGISTERED,
    SCCPCM_S_WF_UNREG_RES,
    SCCPCM_S_LOCKOUT,
    SCCPCM_S_MAX
} sccpcm_states_t;

static char *sccpcm_state_names[] = {
    "S_IDLE",
    "S_WF_CON_RES",
    "S_CONNECTED",
    "S_TOKEN",
    "S_WF_REG_RES",
    "S_WF_CON_RETRY",
    "S_REGISTERED",
    "S_WF_UNREG_RES",
    "S_LOCKOUT"
};

static char *sccpcm_event_names[] = {
    "E_CON_REQ",
    "E_CNTREG",
    "E_KEEPALIVE",
    "E_ERROR",
    "E_DISCON_REQ",
    "E_SEND_TOKEN",
    "E_RECV_TOKEN",
    "E_REG_REQ",
    "E_REGISTERED",
    "E_UNREG_REQ",
    "E_TIMEOUT",
    "E_LOCKOUT",
    "E_CON_ACK",
    "E_CON_RETRY",
    "E_UNREG_ACK",
    "E_UNREG_NAK",
    "E_TCP_EVENTS",
    "E_REG_ACK",
    "E_REG_NAK",
    "E_KEEPALIVE_ACK",
    "E_RESET",
    "E_CONNECTED",
    "E_TOKEN_REJ",
    "E_TOKEN_RETRY_TO",
    "E_LOCKOUT_TO",
    "E_KA_TO",
    "E_WF_REG_RES_TO"
};

static char *sccpcm_error_names[] = {
    "ERROR_DR_PRI_REG_TO",
    "ERROR_CANT_SEND_KA",
    "ERROR_NULL_ADDR_PORT",
    "ERROR_CANT_OPEN_TCP",
    "ERROR_TCP_NAK",
    "ERROR_CANT_SEND_TOKEN",
    "ERROR_NO_KA_ACK",
    "ERROR_SHUTDOWN",
    "ERROR_NO_UNREG_ACK",
    "ERROR_CLOSE_ALL_NOW",
    "ERROR_TCP_CLOSE",
    "ERROR_REGISTER_REJECT",
    "ERROR_REGISTER_TO",
};

typedef enum sccpcm_timers_t_ {
    SCCPCM_TIMER_MIN = -1,
    SCCPCM_TIMER_WF_CON_RES,
    SCCPCM_TIMER_WF_CON_RETRY,
    SCCPCM_TIMER_WF_KA_ACK,
    SCCPCM_TIMER_WF_REG_RES,
    SCCPCM_TIMER_WF_UNREG_RES,
    SCCPCM_TIMER_LOCKOUT,
    SCCPCM_TIMER_TOKEN_RETRY,
    SCCPCM_TIMER_MAX
} sccpcm_timers_t;

static char *sccpcm_timer_names[] = {
    "T_WF_CON_RES",
    "T_WF_CON_RETRY",
    "T_WF_KA_ACK",
    "T_WF_REG_RES",
    "T_WF_UNREG_RES",
    "T_LOCKOUT",
    "T_TOKEN_RETRY"
};


static int  sccpcm_push_timeout(sccp_sccpcb_t *sccpcb, int msg_id,
                                int index, int event);
static int sccpcm_push_simple(sccp_sccpcb_t *sccpcb, int msg_id,
                               int index);

static char *sccpcm_sem_name(int id);
static char *sccpcm_state_name(int id);
static char *sccpcm_event_name(int id);
static char *sccpcm_function_name(int id);
static void sccpcm_debug_sem_entry(sem_event_t *event);
static int  sccpcm_validate_cb(void *sccpcb, void *cb);
static void sccpcm_change_state(sccpcm_cmcb_t *cmcb, int new_state,
                                char *fname);

static int sccpcm_sem_default(sem_event_t *event);
static int sccpcm_sem_connected_discon_req(sem_event_t *event);
static int sccpcm_sem_connected_lockout(sem_event_t *event);
static int sccpcm_sem_connected_recv_token(sem_event_t *event);
static int sccpcm_sem_connected_reg_req(sem_event_t *event);
static int sccpcm_sem_connected_send_token(sem_event_t *event);
static int sccpcm_sem_connected_timeout(sem_event_t *event);
static int sccpcm_sem_connected_ka_ack(sem_event_t *event);
static int sccpcm_sem_do_nothing(sem_event_t *event);
static int sccpcm_sem_idle_con_req(sem_event_t *event);
static int sccpcm_sem_registered_error(sem_event_t *event);
static int sccpcm_sem_registered_lockout(sem_event_t *event);
static int sccpcm_sem_registered_timeout(sem_event_t *event);
static int sccpcm_sem_token_unreg_nak(sem_event_t *event);
static int sccpcm_sem_unreg_req(sem_event_t *event);
static int sccpcm_sem_wf_con_res_con_ack(sem_event_t *event);
static int sccpcm_sem_wf_con_res_con_retry(sem_event_t *event);
static int sccpcm_sem_wf_con_res_error(sem_event_t *event);
static int sccpcm_sem_wf_con_res_timeout(sem_event_t *event);
static int sccpcm_sem_wf_con_retry_con_req(sem_event_t *event);
static int sccpcm_sem_wf_con_retry_timeout(sem_event_t *event);
static int sccpcm_sem_wf_reg_res_reg_ack(sem_event_t *event);
static int sccpcm_sem_wf_reg_res_reg_nak(sem_event_t *event);
static int sccpcm_sem_wf_reg_res_ka_ack(sem_event_t *event);
static int sccpcm_sem_wf_reg_res_timeout(sem_event_t *event);
static int sccpcm_sem_wf_reg_res_wf_reg_res_to(sem_event_t *event);
static int sccpcm_sem_wf_unreg_res_error(sem_event_t *event);
static int sccpcm_sem_wf_unreg_res_timeout(sem_event_t *event);
static int sccpcm_sem_wf_unreg_res_unreg_ack(sem_event_t *event);
static int sccpcm_sem_wf_unreg_res_unreg_nak(sem_event_t *event);
static int sccpcm_sem_tcp_events(sem_event_t *event);
static int sccpcm_sem_keepalive(sem_event_t *event);
static int sccpcm_sem_lockout_timeout_ka(sem_event_t *event);
static int sccpcm_sem_lockout_timeout_lockout(sem_event_t *event);
static int sccpcm_sem_lockout_ka_ack(sem_event_t *event);
static int sccpcm_sem_registered(sem_event_t *event);
static int sccpcm_sem_token_rej(sem_event_t *event);
static int sccpcm_sem_token_retry_to(sem_event_t *event);


typedef enum sccpcm_sem_functions_t_ {
    SCCPCM_SEM_MIN = -1,
    SCCPCM_SEM_DEFAULT,
    SCCPCM_SEM_CONNECTED_DISCON_REQ,
    SCCPCM_SEM_CONNECTED_LOCKOUT,
    SCCPCM_SEM_CONNECTED_RECV_TOKEN,
    SCCPCM_SEM_CONNECTED_REG_REQ,
    SCCPCM_SEM_CONNECTED_SEND_TOKEN,
    SCCPCM_SEM_CONNECTED_TIMEOUT,
    SCCPCM_SEM_CONNECTED_KA_ACK,
    SCCPCM_SEM_DO_NOTHING,
    SCCPCM_SEM_IDLE_CON_REQ,
    SCCPCM_SEM_REGISTERED_ERROR,
    SCCPCM_SEM_REGISTERED_LOCKOUT,
    SCCPCM_SEM_REGISTERED_TIMEOUT,
    SCCPCM_SEM_TOKEN_UNREG_NAK,
    SCCPCM_SEM_UNREG_REQ,
    SCCPCM_SEM_WF_CON_RES_CON_ACK,
    SCCPCM_SEM_WF_CON_RES_CON_RETRY,
    SCCPCM_SEM_WF_CON_RES_ERROR,
    SCCPCM_SEM_WF_CON_RES_TIMEOUT,
    SCCPCM_SEM_WF_CON_RETRY_CON_REQ,
    SCCPCM_SEM_WF_CON_RETRY_TIMEOUT,
    SCCPCM_SEM_WF_REG_RES_REG_ACK,
    SCCPCM_SEM_WF_REG_RES_REG_NAK,
    SCCPCM_SEM_WF_REG_RES_KA_ACK,
    SCCPCM_SEM_WF_REG_RES_TIMEOUT,
    SCCPCM_SEM_WF_REG_RES_WF_REG_RES_TO,
    SCCPCM_SEM_WF_UNREG_RES_ERROR,
    SCCPCM_SEM_WF_UNREG_RES_TIMEOUT,
    SCCPCM_SEM_WF_UNREG_RES_UNREG_ACK,
    SCCPCM_SEM_WF_UNREG_RES_UNREG_NAK,
    SCCPCM_SEM_TCP_EVENTS,
    SCCPCM_SEM_KEEPALIVE,
    SCCPCM_SEM_LOCKOUT_TIMEOUT_KA,
    SCCPCM_SEM_LOCKOUT_TIMEOUT_LOCKOUT,
    SCCPCM_SEM_LOCKOUT_KA_ACK,
    SCCPCM_SEM_REGISTERED,
    SCCPCM_SEM_TOKEN_REJ,
    SCCPCM_SEM_TOKEN_RETRY_TO,
    SCCPCM_SEM_MAX    
} sccpcm_sem_functions_t;

static sem_function_t sccpcm_sem_functions[] = 
{
    sccpcm_sem_default,
    sccpcm_sem_connected_discon_req,
    sccpcm_sem_connected_lockout,
    sccpcm_sem_connected_recv_token,
    sccpcm_sem_connected_reg_req,
    sccpcm_sem_connected_send_token,
    sccpcm_sem_connected_timeout,
    sccpcm_sem_connected_ka_ack,
    sccpcm_sem_do_nothing,
    sccpcm_sem_idle_con_req,
    sccpcm_sem_registered_error,
    sccpcm_sem_registered_lockout,
    sccpcm_sem_registered_timeout,
    sccpcm_sem_token_unreg_nak,
    sccpcm_sem_unreg_req,
    sccpcm_sem_wf_con_res_con_ack,
    sccpcm_sem_wf_con_res_con_retry,
    sccpcm_sem_wf_con_res_error,
    sccpcm_sem_wf_con_res_timeout,
    sccpcm_sem_wf_con_retry_con_req,
    sccpcm_sem_wf_con_retry_timeout,
    sccpcm_sem_wf_reg_res_reg_ack,
    sccpcm_sem_wf_reg_res_reg_nak,
    sccpcm_sem_wf_reg_res_ka_ack,
    sccpcm_sem_wf_reg_res_timeout,
    sccpcm_sem_wf_reg_res_wf_reg_res_to,
    sccpcm_sem_wf_unreg_res_error,
    sccpcm_sem_wf_unreg_res_timeout,
    sccpcm_sem_wf_unreg_res_unreg_ack,
    sccpcm_sem_wf_unreg_res_unreg_nak,
    sccpcm_sem_tcp_events,
    sccpcm_sem_keepalive,
    sccpcm_sem_lockout_timeout_ka,
    sccpcm_sem_lockout_timeout_lockout,
    sccpcm_sem_lockout_ka_ack,
    sccpcm_sem_registered,
    sccpcm_sem_token_rej,
    sccpcm_sem_token_retry_to
};

static char *sccpcm_sem_function_names[] = 
{
    "sem_default",
    "sem_connected_discon_req",
    "sem_connected_lockout",
    "sem_connected_recv_token",
    "sem_connected_reg_req",
    "sem_connected_send_token",
    "sem_connected_timeout",
    "sem_connected_ka_ack",
    "sem_do_nothing",
    "sem_idle_con_req",
    "sem_registered_error",
    "sem_registered_lockout",
    "sem_registered_timeout",
    "sem_token_unreg_nak",
    "sem_unreg_req",
    "sem_wf_con_res_con_ack",
    "sem_wf_con_res_con_retry",
    "sem_wf_con_res_error",
    "sem_wf_con_res_timeout",
    "sem_wf_con_retry_con_req",
    "sem_wf_con_retry_timeout",
    "sem_wf_reg_res_reg_ack",
    "sem_wf_reg_res_reg_nak",
    "sem_wf_reg_res_timeout",
    "sem_wf_reg_res_wf_reg_res_to",
    "sem_wf_unreg_res_error",
    "sem_wf_unreg_res_timeout",
    "sem_wf_unreg_res_unreg_ack",
    "sem_wf_unreg_res_unreg_nak",
    "sem_wf_reg_res_ka_ack",
    "sem_tcp_events",
    "sem_keepalive",
    "sem_lockout_timeout_ka",
    "sem_lockout_timeout_lockout",
    "sccpcm_sem_lockout_ka_ack",
    "sem_registered",
    "sem_token_rej",
    "sem_token_retry_to"
};

static sem_events_t sccpcm_sem_s_idle[] = 
{
    {SCCPCM_E_CON_REQ,         SCCPCM_SEM_IDLE_CON_REQ,               SCCPCM_S_WF_CON_RES}
};

static sem_events_t sccpcm_sem_s_wf_con_res[] = 
{
    {SCCPCM_E_CON_ACK,         SCCPCM_SEM_WF_CON_RES_CON_ACK,         SCCPCM_S_CONNECTED},
    {SCCPCM_E_CON_RETRY,       SCCPCM_SEM_WF_CON_RES_CON_RETRY,       SCCPCM_S_WF_CON_RETRY},
    {SCCPCM_E_TCP_EVENTS,      SCCPCM_SEM_TCP_EVENTS,                 SCCPCM_S_WF_CON_RES},
    {SCCPCM_E_TIMEOUT,         SCCPCM_SEM_WF_CON_RES_TIMEOUT,         SCCPCM_S_IDLE},            
    {SCCPCM_E_ERROR,           SCCPCM_SEM_WF_CON_RES_ERROR,           SCCPCM_S_IDLE},    
};

static sem_events_t sccpcm_sem_s_connected[] =     
{
    {SCCPCM_E_SEND_TOKEN,      SCCPCM_SEM_CONNECTED_SEND_TOKEN,       SCCPCM_S_CONNECTED},
    {SCCPCM_E_RECV_TOKEN,      SCCPCM_SEM_CONNECTED_RECV_TOKEN,       SCCPCM_S_TOKEN},
    {SCCPCM_E_DISCON_REQ,      SCCPCM_SEM_CONNECTED_DISCON_REQ,       SCCPCM_S_IDLE},
    {SCCPCM_E_REG_REQ,         SCCPCM_SEM_CONNECTED_REG_REQ,          SCCPCM_S_WF_REG_RES},
    {SCCPCM_E_KEEPALIVE_ACK,   SCCPCM_SEM_CONNECTED_KA_ACK,           SCCPCM_S_CONNECTED},
    {SCCPCM_E_LOCKOUT,         SCCPCM_SEM_CONNECTED_LOCKOUT,          SCCPCM_S_LOCKOUT},
    {SCCPCM_E_TCP_EVENTS,      SCCPCM_SEM_TCP_EVENTS,                 SCCPCM_S_CONNECTED},
    {SCCPCM_E_TOKEN_REJ,       SCCPCM_SEM_TOKEN_REJ,                  SCCPCM_S_CONNECTED},
    {SCCPCM_E_TIMEOUT,         SCCPCM_SEM_CONNECTED_TIMEOUT,          SCCPCM_S_CONNECTED},
    {SCCPCM_E_TOKEN_RETRY_TO,  SCCPCM_SEM_TOKEN_RETRY_TO,             SCCPCM_S_CONNECTED},
    {SCCPCM_E_ERROR,           SCCPCM_SEM_WF_CON_RES_ERROR,           SCCPCM_S_IDLE},    
    {SCCPCM_E_KEEPALIVE,       SCCPCM_SEM_KEEPALIVE,                  SCCPCM_S_CONNECTED}
};

static sem_events_t sccpcm_sem_s_token[] = 
{
    {SCCPCM_E_DISCON_REQ,      SCCPCM_SEM_CONNECTED_DISCON_REQ,       SCCPCM_S_IDLE},
    {SCCPCM_E_REG_REQ,         SCCPCM_SEM_CONNECTED_REG_REQ,          SCCPCM_S_WF_REG_RES},
    {SCCPCM_E_UNREG_NAK,       SCCPCM_SEM_TOKEN_UNREG_NAK,            SCCPCM_S_CONNECTED},
    {SCCPCM_E_KEEPALIVE_ACK,   SCCPCM_SEM_CONNECTED_KA_ACK,           SCCPCM_S_TOKEN},
    {SCCPCM_E_LOCKOUT,         SCCPCM_SEM_CONNECTED_LOCKOUT,          SCCPCM_S_LOCKOUT},
    {SCCPCM_E_TCP_EVENTS,      SCCPCM_SEM_TCP_EVENTS,                 SCCPCM_S_TOKEN},
    {SCCPCM_E_TIMEOUT,         SCCPCM_SEM_CONNECTED_TIMEOUT,          SCCPCM_S_TOKEN},
    {SCCPCM_E_ERROR,           SCCPCM_SEM_WF_CON_RES_ERROR,           SCCPCM_S_IDLE},
    {SCCPCM_E_KEEPALIVE,       SCCPCM_SEM_KEEPALIVE,                  SCCPCM_S_TOKEN}
};

static sem_events_t sccpcm_sem_s_wf_reg_res[] = 
{
    {SCCPCM_E_UNREG_REQ,       SCCPCM_SEM_UNREG_REQ,                  SCCPCM_S_WF_UNREG_RES},
    {SCCPCM_E_REG_ACK,         SCCPCM_SEM_WF_REG_RES_REG_ACK,         SCCPCM_S_REGISTERED},
    {SCCPCM_E_REG_NAK,         SCCPCM_SEM_WF_REG_RES_REG_NAK,         SCCPCM_S_WF_REG_RES},
    {SCCPCM_E_KEEPALIVE_ACK,   SCCPCM_SEM_WF_REG_RES_KA_ACK,          SCCPCM_S_WF_REG_RES},
    {SCCPCM_E_TCP_EVENTS,      SCCPCM_SEM_TCP_EVENTS,                 SCCPCM_S_WF_REG_RES},
    {SCCPCM_E_TIMEOUT,         SCCPCM_SEM_CONNECTED_TIMEOUT,          SCCPCM_S_WF_REG_RES},
    {SCCPCM_E_WF_REG_RES_TO,   SCCPCM_SEM_WF_REG_RES_WF_REG_RES_TO,   SCCPCM_S_WF_REG_RES},
    {SCCPCM_E_ERROR,           SCCPCM_SEM_WF_CON_RES_ERROR,           SCCPCM_S_IDLE},            
    {SCCPCM_E_KEEPALIVE,       SCCPCM_SEM_KEEPALIVE,                  SCCPCM_S_WF_REG_RES}
};

static sem_events_t sccpcm_sem_s_wf_con_retry[] = 
{
    {SCCPCM_E_CON_REQ,         SCCPCM_SEM_WF_CON_RETRY_CON_REQ,       SCCPCM_S_WF_CON_RES},
    {SCCPCM_E_TCP_EVENTS,      SCCPCM_SEM_TCP_EVENTS,                 SCCPCM_S_WF_CON_RETRY},
    {SCCPCM_E_TIMEOUT,         SCCPCM_SEM_WF_CON_RETRY_TIMEOUT,       SCCPCM_S_WF_CON_RETRY},
    {SCCPCM_E_ERROR,           SCCPCM_SEM_WF_CON_RES_ERROR,           SCCPCM_S_IDLE},            
    {SCCPCM_E_KEEPALIVE,       SCCPCM_SEM_KEEPALIVE,                  SCCPCM_S_WF_CON_RETRY}    
};

static sem_events_t sccpcm_sem_s_registered[] =     
{
    {SCCPCM_E_UNREG_REQ,       SCCPCM_SEM_UNREG_REQ,                  SCCPCM_S_WF_UNREG_RES},
    {SCCPCM_E_REGISTERED,      SCCPCM_SEM_REGISTERED,                 SCCPCM_S_REGISTERED},
    {SCCPCM_E_LOCKOUT,         SCCPCM_SEM_REGISTERED_LOCKOUT,         SCCPCM_S_LOCKOUT},  
    {SCCPCM_E_KEEPALIVE_ACK,   SCCPCM_SEM_CONNECTED_KA_ACK,           SCCPCM_S_REGISTERED},
    {SCCPCM_E_TCP_EVENTS,      SCCPCM_SEM_TCP_EVENTS,                 SCCPCM_S_REGISTERED},
    {SCCPCM_E_TIMEOUT,         SCCPCM_SEM_CONNECTED_TIMEOUT,          SCCPCM_S_REGISTERED},            
    {SCCPCM_E_ERROR,           SCCPCM_SEM_REGISTERED_ERROR,           SCCPCM_S_WF_UNREG_RES},            
    {SCCPCM_E_KEEPALIVE,       SCCPCM_SEM_KEEPALIVE,                  SCCPCM_S_REGISTERED}
};

static sem_events_t sccpcm_sem_s_wf_unreg_res[] = 
{
    {SCCPCM_E_UNREG_ACK,       SCCPCM_SEM_WF_UNREG_RES_UNREG_ACK,     SCCPCM_S_CONNECTED},
    {SCCPCM_E_UNREG_NAK,       SCCPCM_SEM_WF_UNREG_RES_UNREG_NAK,     SCCPCM_S_REGISTERED},
    {SCCPCM_E_TCP_EVENTS,      SCCPCM_SEM_TCP_EVENTS,                 SCCPCM_S_WF_UNREG_RES},
    {SCCPCM_E_TIMEOUT,         SCCPCM_SEM_WF_UNREG_RES_TIMEOUT,       SCCPCM_S_WF_UNREG_RES},            
    {SCCPCM_E_ERROR,           SCCPCM_SEM_WF_UNREG_RES_ERROR,         SCCPCM_S_IDLE},            
    {SCCPCM_E_KEEPALIVE,       SCCPCM_SEM_KEEPALIVE,                  SCCPCM_S_WF_UNREG_RES}
};

static sem_events_t sccpcm_sem_s_lockout[] = 
{
    {SCCPCM_E_DISCON_REQ,      SCCPCM_SEM_CONNECTED_DISCON_REQ,       SCCPCM_S_IDLE},
    {SCCPCM_E_UNREG_ACK,       SCCPCM_SEM_DO_NOTHING,                 SCCPCM_S_LOCKOUT},
    {SCCPCM_E_KEEPALIVE_ACK,   SCCPCM_SEM_LOCKOUT_KA_ACK,             SCCPCM_S_CONNECTED},
    {SCCPCM_E_TCP_EVENTS,      SCCPCM_SEM_TCP_EVENTS,                 SCCPCM_S_LOCKOUT},
    {SCCPCM_E_KA_TO,           SCCPCM_SEM_LOCKOUT_TIMEOUT_KA,         SCCPCM_S_LOCKOUT},
    {SCCPCM_E_LOCKOUT_TO,      SCCPCM_SEM_LOCKOUT_TIMEOUT_LOCKOUT,    SCCPCM_S_LOCKOUT},
	{SCCPCM_E_ERROR,           SCCPCM_SEM_WF_CON_RES_ERROR,           SCCPCM_S_IDLE},            
    {SCCPCM_E_KEEPALIVE,       SCCPCM_SEM_KEEPALIVE,                  SCCPCM_S_LOCKOUT}
};

static sem_states_t sccpcm_sem_states[] =
{ 
    {sccpcm_sem_s_idle,              sizeof(sccpcm_sem_s_idle)              / SEM_EVENTS_SIZE},
    {sccpcm_sem_s_wf_con_res,        sizeof(sccpcm_sem_s_wf_con_res)        / SEM_EVENTS_SIZE},
    {sccpcm_sem_s_connected,         sizeof(sccpcm_sem_s_connected)         / SEM_EVENTS_SIZE},
    {sccpcm_sem_s_token,             sizeof(sccpcm_sem_s_token)             / SEM_EVENTS_SIZE},
    {sccpcm_sem_s_wf_reg_res,        sizeof(sccpcm_sem_s_wf_reg_res)        / SEM_EVENTS_SIZE},
    {sccpcm_sem_s_wf_con_retry,      sizeof(sccpcm_sem_s_wf_con_retry)      / SEM_EVENTS_SIZE},
    {sccpcm_sem_s_registered,        sizeof(sccpcm_sem_s_registered)        / SEM_EVENTS_SIZE},
    {sccpcm_sem_s_wf_unreg_res,      sizeof(sccpcm_sem_s_wf_unreg_res)      / SEM_EVENTS_SIZE},
    {sccpcm_sem_s_lockout,           sizeof(sccpcm_sem_s_lockout)           / SEM_EVENTS_SIZE}
};

static sem_tbl_t sccpcm_sem_tbl = {
    sccpcm_sem_states, SCCPCM_S_MAX, SCCPCM_E_MAX
};

sem_table_t sccpcm_sem_table = 
{
    &sccpcm_sem_tbl,        sccpcm_sem_functions,
    sccpcm_state_name,      sccpcm_event_name,
    sccpcm_sem_name,        sccpcm_function_name,
    sccpcm_debug_sem_entry, sccpcm_validate_cb,
    (sem_change_state_f *)sccpcm_change_state
};


static char *sccpcm_sem_name (int id)
{
    return (SCCPCM_ID);
}

static char *sccpcm_state_name (int id)
{
    if ((id <= SCCPCM_S_MIN) || (id >= SCCPCM_S_MAX)) {
        return (SCCP_UNDEFINED);
    }

    return (sccpcm_state_names[id]);
}

static char *sccpcm_event_name (int id)
{
    if ((id <= SCCPCM_E_MIN) || (id >= SCCPCM_E_MAX)) {
        return (SCCP_UNDEFINED);
    }

    return (sccpcm_event_names[id]);
}

static char *sccpcm_error_name (int id)
{
    if ((id <= SCCPCM_ERROR_MIN) || (id >= SCCPCM_ERROR_MAX)) {
        return (SCCP_UNDEFINED);
    }

    return (sccpcm_error_names[id]);
}

static char *sccpcm_function_name (int id)
{
    if ((id <= SCCPCM_SEM_MIN) || (id >= SCCPCM_SEM_MAX)) {
        return (SCCP_UNDEFINED);
    }

    return (sccpcm_sem_function_names[id]);
}

static char *sccpcm_timer_name (int id)
{
    if ((id <= SCCPCM_TIMER_MIN) || (id >= SCCPCM_TIMER_MAX)) {
        return (SCCP_UNDEFINED);
    }

    return (sccpcm_timer_names[id]);
}

static void sccpcm_sessionstatus (sccpcm_cmcb_t *cmcb, gapi_status_e status)
{
    gapi_status_data_u data;

    data.misc.cmaddr.addr = cmcb->addr;
    data.misc.cmaddr.port = cmcb->port;

    SCCP_SESSIONSTATUS(cmcb, GAPI_MSG_STATUS,
                       status, GAPI_STATUS_DATA_MISC, &data);
}

/*
 * We know the addr and port are in network order. Convert the port to 
 * host order for the printf.
 */
char *sccpcm_print_cm (sccpcm_cmcb_t *cmcb)
{
    unsigned char  *p;
    static char    str[48];
    unsigned long  addr  = 0;
    unsigned short port  = 0;
    int            index = -1;

    if (cmcb != NULL) {
        p = (unsigned char *)&(cmcb->addr);
        port = SCCP_NTOHS(cmcb->port);
        index = cmcb->index;
    }
    else {
        p = (unsigned char *)&addr;
    }

    SCCP_SNPRINTF((str, sizeof(str), "CM[%d] %d.%d.%d.%d:%d",
                   index, p[0], p[1], p[2], p[3],
                   port));

    return (str);
}

static void sccpcm_debug_sem_entry (sem_event_t *event)
{
    sccpcm_cmcb_t *cmcb = (sccpcm_cmcb_t *)(event->cb);
    
    SCCP_DBG((sccpcm_debug, 9,
             "%s %-2d:%-2d: %-20s: %s <- %s\n",
             SCCPCM_ID, cmcb->sccpcb->id, cmcb->id,
             event->function_name,
             sccpcm_state_name(event->state_id),
             sccpcm_event_name(event->event_id)));
}

static int sccpcm_validate_cb (void *cb1, void *cb2)
{
    return (sllist_find_node(((sccp_sccpcb_t *)(cb1))->cmcbs, cb2) == NULL ? 1 : 0);
}

static void sccpcm_change_state (sccpcm_cmcb_t *cmcb, int new_state,
                                 char *fname)
{
    SCCP_DBG((sccpcm_debug, 9,
             "%s %-2d:%-2d: %-20s: %s -> %s\n",
             SCCPCM_ID, cmcb->sccpcb->id, cmcb->id, fname,
             sccpcm_state_name(cmcb->state),
             sccpcm_state_name(new_state)));

    cmcb->old_state = cmcb->state;
    cmcb->state = new_state;
}

int sccpcm_get_cm_count (sccp_sccpcb_t *sccpcb)
{
    return (sllist_get_list_size(sccpcb->cmcbs));
}

static void sccpcm_print_cmcb (sccpcm_cmcb_t *cmcb, char *fname)
{
    SCCP_DBG((sccpcm_debug, 3, "%s %-2d:%-2d: %-20s: cmcb= 0x%08p\n",
             SCCPCM_ID,
             cmcb == NULL ? 0 : cmcb->sccpcb->id,
             cmcb == NULL ? 0 : cmcb->id,
             fname, cmcb));
}

sccpcm_cmcb_t *sccpcm_get_primary_cmcb (sccp_sccpcb_t *sccpcb)
{
    char          *fname     = "get_primary_cmcb";
    sccpcm_cmcb_t *cmcb      = NULL;
    sccpcm_cmcb_t *curr_cmcb;

    if ((sccpcb != NULL) && (sccpcb->cmcbs != NULL)) {
        for (curr_cmcb = ((sccpcm_cmcb_t *)(sccpcb->cmcbs))->next;
             curr_cmcb != NULL; curr_cmcb = curr_cmcb->next) {

             if (curr_cmcb->state == SCCPCM_S_REGISTERED) {
                cmcb = curr_cmcb;
                break;
            }
        }
    }

    sccpcm_print_cmcb(cmcb, fname);

    return (cmcb);
}

unsigned int sccpcm_get_primary_socket (sccp_sccpcb_t *sccpcb)
{
    char          *fname     = "get_primary_socket";
    sccpcm_cmcb_t *cmcb      = NULL;
    sccpcm_cmcb_t *curr_cmcb;

    if ((sccpcb != NULL) && (sccpcb->cmcbs != NULL)) {
        for (curr_cmcb = ((sccpcm_cmcb_t *)(sccpcb->cmcbs))->next;
             curr_cmcb != NULL; curr_cmcb = curr_cmcb->next) {

            if ((curr_cmcb->state == SCCPCM_S_REGISTERED) ||
                (curr_cmcb->state == SCCPCM_S_WF_REG_RES)) {
                cmcb = curr_cmcb;
                break;
            }
        }
    }

    if (cmcb != NULL) {
        if (cmcb->shutdown == 1) {
            cmcb = NULL;
        }
    }

    sccpcm_print_cmcb(cmcb, fname);

    return ((cmcb == NULL) ? (0) : (cmcb->socket));
}

int sccpcm_get_primary_index (sccp_sccpcb_t *sccpcb)
{
    char          *fname = "get_primary_index";
    sccpcm_cmcb_t *cmcb;

    cmcb = sccpcm_get_primary_cmcb(sccpcb);

    sccpcm_print_cmcb(cmcb, fname);

    return (cmcb == NULL ? SCCPCM_NO_INDEX : cmcb->index);
}

sccpcm_cmcb_t *sccpcm_get_secondary_cmcb (sccp_sccpcb_t *sccpcb)
{
    char          *fname     = "get_secondary_cmcb";
    sccpcm_cmcb_t *cmcb      = NULL;
    sccpcm_cmcb_t *curr_cmcb;

    if ((sccpcb != NULL) && (sccpcb->cmcbs != NULL)) {
        for (curr_cmcb = ((sccpcm_cmcb_t *)(sccpcb->cmcbs))->next;
             curr_cmcb != NULL; curr_cmcb = curr_cmcb->next) {

            if ((curr_cmcb->state == SCCPCM_S_CONNECTED) || 
                (curr_cmcb->state == SCCPCM_S_TOKEN)) {
                cmcb = curr_cmcb;
                break;
            }
        }
    }

    sccpcm_print_cmcb(cmcb, fname);

    return (cmcb);
}

int sccpcm_get_secondary_index (sccp_sccpcb_t *sccpcb)
{
    char          *fname     = "get_secondary_index";
    sccpcm_cmcb_t *cmcb      = NULL;

    cmcb = sccpcm_get_secondary_cmcb(sccpcb);

    sccpcm_print_cmcb(cmcb, fname);

    return (cmcb == NULL ? SCCPCM_NO_INDEX : cmcb->index);
}

sccpcm_cmcb_t *sccpcm_get_cmcb_by_socket (sccp_sccpcb_t *sccpcb,
                                          int socket)
{
    char          *fname = "get_cmcb_by_socket";
    sccpcm_cmcb_t *cmcb  = NULL;
    sccpcm_cmcb_t *curr_cmcb;

    if ((sccpcb != NULL) && (sccpcb->cmcbs != NULL)) {
        for (curr_cmcb = ((sccpcm_cmcb_t *)(sccpcb->cmcbs))->next;
             curr_cmcb != NULL; curr_cmcb = curr_cmcb->next) {

             if (curr_cmcb->socket == socket) { 
                cmcb = curr_cmcb;
                break;
            }
        }
    }

    sccpcm_print_cmcb(cmcb, fname);

    return (cmcb);
}

#if 0
sccpcm_cmcb_t *sccpcm_get_cmcb_by_socket2 (unsigned int socket)
{
    char          *fname = "get_cmcb_by_socket2";
    sccpcm_cmcb_t *cmcb  = NULL;
    sccp_sccpcb_t *sccpcb;

    sccpcb = sccp_get_sccpcb();

    cmcb = sccpcm_get_cmcb_by_socket(sccpcb, socket);

    return (cmcb);
}
#endif

sccpcm_cmcb_t *sccpcm_get_cmcb_by_index (sccp_sccpcb_t *sccpcb,
                                         int index)
{
    char          *fname = "get_cmcb_by_index";
    sccpcm_cmcb_t *cmcb  = NULL;
    sccpcm_cmcb_t *curr_cmcb;

    if ((sccpcb != NULL) && (sccpcb->cmcbs != NULL)) {
        for (curr_cmcb = ((sccpcm_cmcb_t *)(sccpcb->cmcbs))->next;
             curr_cmcb != NULL; curr_cmcb = curr_cmcb->next) {

             if (curr_cmcb->index == index) { 
                cmcb = curr_cmcb;
                break;
            }
        }
    }

    sccpcm_print_cmcb(cmcb, fname);

    return (cmcb);
}

sccpcm_chunk_data_t *sccpcm_get_chunk_data (sccp_sccpcb_t *sccpcb,
                                            unsigned int socket)
{
    sccpcm_cmcb_t *cmcb;

    cmcb = sccpcm_get_cmcb_by_socket(sccpcb, socket);

    if (cmcb != NULL) {
        return (&(cmcb->chunk_data));
    }
    else {
        return (NULL);
    }
}

int sccpcm_get_cm_by_index (sccp_sccpcb_t *sccpcb, int index)
{
    sccpcm_cmcb_t *cmcb;

    cmcb = sccpcm_get_cmcb_by_index(sccpcb, index);

    return (cmcb == NULL ? SCCPCM_NO_INDEX : cmcb->id);
}

#if 0
int sccpcm_is_state (int index, sccp_sccpcb_t *sccpcb, sccpcm_states_t state)
{
    sccpcm_cmcb_t *cmcb;
    int           rc = 0;

    cmcb = sccpcm_get_cmcb_by_index(sccpcb, index);
    if (cmcb == NULL) {
        return (0);
    }

    switch (state) {
    case (SCCPCM_S_IDLE):
        if (cmcb->state == SCCPCM_S_IDLE) {
            rc = 1;
        }

        break;

    case (SCCPCM_S_CONNECTED):
        if (cmcb->state == SCCPCM_S_CONNECTED) {
            rc = 1;
        }

        break;

    case (SCCPCM_S_REGISTERED):
        if (cmcb->state == SCCPCM_S_REGISTERED) {
            rc = 1;
        }

        break;

    case (SCCPCM_S_TOKEN):
        if (cmcb->state == SCCPCM_S_TOKEN) {
            rc = 1;
        }

        break;

    case (SCCPCM_S_LOCKOUT):
        if (cmcb->state == SCCPCM_S_LOCKOUT) {
            rc = 1;
        }

        break;

    default:
        rc = 0;
        break;
    }

    return (rc);
}
#endif
int sccpcm_are_all_cms_idle (sccp_sccpcb_t *sccpcb)
{
    sccpcm_cmcb_t *curr_cmcb;
    int           rc = 1;

    if ((sccpcb != NULL) && (sccpcb->cmcbs != NULL)) {
        for (curr_cmcb = ((sccpcm_cmcb_t *)(sccpcb->cmcbs))->next;
             curr_cmcb != NULL; curr_cmcb = curr_cmcb->next) {

             if (curr_cmcb->state != SCCPCM_S_IDLE) { 
                rc = 0;
                break;
            }
        }
    }

    return (rc);
}

int sccpcm_are_all_cms_lockout (sccp_sccpcb_t *sccpcb)
{
    sccpcm_cmcb_t *curr_cmcb;
    int           rc = 1;

    if ((sccpcb != NULL) && (sccpcb->cmcbs != NULL)) {
        for (curr_cmcb = ((sccpcm_cmcb_t *)(sccpcb->cmcbs))->next;
             curr_cmcb != NULL; curr_cmcb = curr_cmcb->next) {

             if (curr_cmcb->state != SCCPCM_S_LOCKOUT) { 
                rc = 0;
                break;
            }
        }
    }

    return (rc);
}

int sccpcm_is_cmcb_idle (sccpcm_cmcb_t *cmcb)
{
    int rc = 0;

    if (cmcb != NULL) {
        if (cmcb->state == SCCPCM_S_IDLE) {
            rc = 1;
        }
    }

    return (rc);
}

int sccpcm_is_cmcb_connected (sccpcm_cmcb_t *cmcb)
{
    int rc = 0 ;
    
    if (cmcb != NULL) {
        if ((cmcb->state == SCCPCM_S_CONNECTED) || 
            (cmcb->state == SCCPCM_S_TOKEN)) {
            rc = 1;
        }
    }

    return (rc);
}

int sccpcm_is_cmcb_connected2 (sccpcm_cmcb_t *cmcb)
{
    int rc = 0;

    if (cmcb != NULL) {
        if (cmcb->state == SCCPCM_S_CONNECTED) {
            rc = 1;
        }
    }

    return (rc);
}

int sccpcm_is_cmcb_registered (sccpcm_cmcb_t *cmcb)
{
    int rc = 0;

    if (cmcb != NULL) {
        if (cmcb->state == SCCPCM_S_REGISTERED) {
            rc = 1;
        }
    }

    return (rc);
}

int sccpcm_is_cmcb_registered2 (sccpcm_cmcb_t *cmcb)
{
    int rc = 0;

    if (cmcb != NULL) {
        if ((cmcb->state == SCCPCM_S_REGISTERED) ||
            (cmcb->state == SCCPCM_S_WF_REG_RES)) {
            rc = 1;
        }
    }

    return (rc);
}

int sccpcm_is_cmcb_token (sccpcm_cmcb_t *cmcb)
{
    int rc = 0;

    if (cmcb != NULL) {
        if (cmcb->state == SCCPCM_S_TOKEN) {
            rc = 1;
        }
    }

    return (rc);
}

int sccpcm_is_cmcb_lockout (sccpcm_cmcb_t *cmcb)
{
    int rc = 0;

    if (cmcb != NULL) {
        if (cmcb->state == SCCPCM_S_LOCKOUT) {
            rc = 1;
        }
    }

    return (rc);
}

int sccpcm_is_cm_registered_with_fallback (sccp_sccpcb_t *sccpcb)
{
    int flag = 0;

    if ((sccpcb != NULL) && (sccpcb->reccb != NULL)) {
        flag = sccpcb->reccb->registered_with_fallback;
    }

    return (flag);
}

int sccpcm_get_connected_index (sccp_sccpcb_t *sccpcb)
{
    sccpcm_cmcb_t *cmcb;

    cmcb = sccpcm_get_secondary_cmcb(sccpcb);

    return (cmcb == NULL ? SCCPCM_NO_INDEX : cmcb->index);
}

static int sccpcm_get_active_cm (sccp_sccpcb_t *sccpcb)
{
    int           index  = SCCPCM_NO_INDEX;
    sccpcm_cmcb_t *curr_cmcb;

    if ((sccpcb != NULL) && (sccpcb->cmcbs != NULL)) {
        for (curr_cmcb = ((sccpcm_cmcb_t *)(sccpcb->cmcbs))->next;
             curr_cmcb != NULL; curr_cmcb = curr_cmcb->next) {

            if ((curr_cmcb->state == SCCPCM_S_WF_REG_RES)        ||
                (curr_cmcb->state == SCCPCM_S_REGISTERED)        || 
                (curr_cmcb->state == SCCPCM_S_WF_UNREG_RES)) {
                index = curr_cmcb->index;

                break;
            }
        }
    }

    return (index);
}

static void sccpcm_timeout_callback (void *timer_event, void *param1,
                                     void *param2)
{
    sccpcm_cmcb_t *cmcb = (sccpcm_cmcb_t *)param1;
    int           event = (int)param2;

    SCCP_DBG((sccpcm_debug, 9,
             "%s %-2d:%-2d: %-20s: %s: 0x%08x\n",
             SCCPCM_ID, ((sccpcm_cmcb_t *)(param1))->sccpcb->id,
             ((sccpcm_cmcb_t *)(param1))->id, "timeout_callback",
             sccpcm_timer_name((int)param2), timer_event));

    switch (event) {
    case (SCCPCM_TIMER_WF_KA_ACK):
        if (cmcb->state == SCCPCM_S_LOCKOUT) {
            sccpcm_push_simple(cmcb->sccpcb, SCCPCM_E_KA_TO,
                               cmcb->index);
        }
        else {
            sccpcm_push_timeout(cmcb->sccpcb, SCCPCM_E_TIMEOUT, 
                                cmcb->index, event);
        }

        break;
    
    case (SCCPCM_TIMER_WF_REG_RES):
#if 0
        sccpcm_push_reg_nak(cmcb->sccpcb, SCCPCM_E_REG_NAK,
                            cmcb->index, NULL);
#endif
        sccpcm_push_simple(cmcb->sccpcb, SCCPCM_E_WF_REG_RES_TO,
                           cmcb->index);

        break;

    case (SCCPCM_TIMER_LOCKOUT):
        sccpcm_push_simple(cmcb->sccpcb, SCCPCM_E_LOCKOUT_TO,
                           cmcb->index);

        break;
    
    case (SCCPCM_TIMER_TOKEN_RETRY):
        sccpcm_push_simple(cmcb->sccpcb, SCCPCM_E_TOKEN_RETRY_TO,
                           cmcb->index);
        break;

    default:
        sccpcm_push_timeout(cmcb->sccpcb, SCCPCM_E_TIMEOUT, 
                            cmcb->index, event);
    }
}

static void sccpcm_stop_timer (sccpcm_cmcb_t *cb, void *timer)
{
    char *fname = "stop_timer";

    SCCP_TIMER_CANCEL(timer);

    SCCP_DBG((sccpcm_debug, 9,
             "%s %-2d:%-2d: %-20s: 0x%08x\n",
             SCCPCM_ID, cb->sccpcb->id, cb->id, fname, timer));
}

static void sccpcm_stop_timers (sccpcm_cmcb_t *cmcb)
{
    SCCP_TIMER_CANCEL(cmcb->misc_timer);
    SCCP_TIMER_CANCEL(cmcb->keepalive_timer);
}

static void sccpcm_free_timers (sccpcm_cmcb_t *cmcb)
{
    SCCP_TIMER_FREE(cmcb->misc_timer);
    SCCP_TIMER_FREE(cmcb->keepalive_timer);
}

static void sccpcm_start_timer (sccpcm_cmcb_t *cmcb,
                                void *timer, int type,
                                unsigned long period)
{
    char *fname = "start_timer";

    SCCP_TIMER_CANCEL(timer);

    SCCP_TIMER_INITIALIZE(timer,
                          period,
                          sccpcm_timeout_callback,
                          (void *)cmcb,
                          (void *)type);

    SCCP_TIMER_ACTIVATE(timer);

    SCCP_DBG((sccpcm_debug, 9,
             "%s %-2d:%-2d: %-20s: %s: 0x%08x: %dms\n",
             SCCPCM_ID, cmcb->sccpcb->id, cmcb->id, fname,
             sccpcm_timer_name(type), timer, period));
}

int sccpcm_get_keepalive_timeout (sccpcm_cmcb_t *cmcb)
{
    int timeout = 0;

    switch (cmcb->state) {
    case (SCCPCM_S_WF_CON_RES):
        /*sam
         * maybe change this to the default 10s since we know that 
         * we are trying to get to a primary.
         */
    case (SCCPCM_S_CONNECTED):
    case (SCCPCM_S_TOKEN):
    case (SCCPCM_S_WF_REG_RES):

		/*
         * Secondary connections use the secondary timeout.
         */
        timeout = sccprec_get_keepalive_to(cmcb->sccpcb, 2);
        break;

    case (SCCPCM_S_REGISTERED):
    case (SCCPCM_S_LOCKOUT):
        /*
         * Primary connections use the primary timeout.
         */
        timeout = sccprec_get_keepalive_to(cmcb->sccpcb, 1);
        break;

    default:
        timeout = 0;
        break;
    }

    return (timeout);
}

static void sccpcm_start_keepalive_timer (sccpcm_cmcb_t *cmcb)
{
    int period;

	period = sccpcm_get_keepalive_timeout(cmcb);

    if (period != 0) {
        sccpcm_start_timer(cmcb, cmcb->keepalive_timer,
			               SCCPCM_TIMER_WF_KA_ACK, period);
    }

    return;
}

static void sccpcm_start_misc_timer (sccpcm_cmcb_t *cmcb)
{
    int period;
    int event;

    switch (cmcb->state) {
#if 0
	/* 
	 * keepalive timer is used for this timeout. This allows the misc
	 * timer to be used for the nak_to_syn timeout because we will need
	 * two misc timers for that scenario.
     */
    case (SCCPCM_S_WF_CON_RES):
        period = cmcb->sccpcb->reccb->connect_to;
        event  = SCCPCM_TIMER_WF_CON_RES;
        break;
#endif
    case (SCCPCM_S_WF_CON_RETRY):
        period = cmcb->sccpcb->reccb->nak_to_syn_retry_to;
        event  = SCCPCM_TIMER_WF_CON_RETRY;
        break;

    case (SCCPCM_S_WF_REG_RES):
        period = SCCPREC_WF_REGISTER_TO * SCCPREC_ACK_RETRIES_BEFORE_LOCKOUT;
        event  = SCCPCM_TIMER_WF_REG_RES;
        break;

    case (SCCPCM_S_WF_UNREG_RES):
        period = SCCPREC_WF_UNREGISTER_TO;
        event  = SCCPCM_TIMER_WF_UNREG_RES;
        break;

    case (SCCPCM_S_LOCKOUT):    
        period = SCCPREC_LOCKOUT_TO;
        event  = SCCPCM_TIMER_LOCKOUT;
        break;

    default:
        period = 0;
        event = 0;
        break;
    }

    if (period != 0) {
        sccpcm_start_timer(cmcb, cmcb->misc_timer, event, period);
    }

    return;
}

static int sccpcm_cm_connect (sccpcm_cmcb_t *cmcb)
{
    int error = 0;

    if (cmcb == NULL) {
        return (1);
    }

    /*
     * Let the application know that we are trying to open a
     * connection.
     */
    sccpcm_sessionstatus(cmcb, GAPI_STATUS_CM_OPENING);

    (void *)cmcb->socket = cmcb->sccpcb->apphandle;

    error = SCCP_SOCKET_OPEN(&(cmcb->socket));

#if 0 /* test02 */ /* test32 */
    /* primary: 0, secondary: 1 */
    if (cmcb->index == 1) {
        return (1);
    }
#endif

    if ((error != 0) || (cmcb->socket <= 0)) {
        //cmcb->socket = 0;
        return (error);
    }

    error = SCCP_SOCKET_CONNECT(cmcb->socket, cmcb->addr, cmcb->port);

#if 0 /* test02 */ /* test33 */
    /* primary: 0, secondary: 1 */
    if (cmcb->index == 1) {
        return (1);
    }
#endif

    if (error != 0) {
        //cmcb->socket = 0;
        return (error);
    }

    return (0);
}

static int sccpcm_cm_disconnect (sccpcm_cmcb_t *cmcb)
{
    int error = 0;

    if (cmcb == NULL) {
        return (1);
    }

    if (cmcb->socket != 0) {
        error = SCCP_SOCKET_CLOSE(cmcb->socket);
        cmcb->socket = 0;

        /*
         * Let the application know that we are not connected
         * to a primary CM.
         */
        sccpcm_sessionstatus(cmcb, GAPI_STATUS_CM_DOWN);

        /*
         * Don't need this buffer anymore since we are not connected
         * and can't receive messages.
         */
        if (cmcb->chunk_data.residual_buffer != NULL) {
            SCCP_FREE(cmcb->chunk_data.residual_buffer);
            cmcb->chunk_data.residual_buffer = NULL;
        }
    }


    return (error);
}

#if 0 /* not used yet */
static int sccpcm_cm_disconnect_active (sccpcm_cmcb_t *cmcb)
{
    /*
     * Let the application know that we are not connected
     * to a primary CM.
     */
    sccpcm_sessionstatus(cmcb, GAPI_STATUS_CM_DOWN);            

    return (sccpcm_cm_disconnect(cmcb));
}
#endif
int sccpcm_cm_disconnect_all (sccp_sccpcb_t *sccpcb)
{
    sccpcm_cmcb_t *curr_cmcb;

    if ((sccpcb != NULL) && (sccpcb->cmcbs != NULL)) {
        for (curr_cmcb = ((sccpcm_cmcb_t *)(sccpcb->cmcbs))->next;
             curr_cmcb != NULL; curr_cmcb = curr_cmcb->next) {

             if (curr_cmcb->socket != 0) {
                sccpcm_cm_disconnect(curr_cmcb);
                break;
            }
        }
    }

    return (0);
}

#if 0
static int sccpcm_get_new_cmcb_id (void)
{
    static int id = 0;

    if (++id < 0) {
        id = 1;
    }

    return (id);
}
#endif

void sccpcm_free_cmcb (sccpcm_cmcb_t *cmcb)
{
    if ((cmcb != NULL) && (cmcb->sccpcb != NULL)) {
        sllist_delete_node(cmcb->sccpcb->cmcbs, cmcb);
    }
}

void sccpcm_free_cmcb2 (sccpcm_cmcb_t *cmcb)
{
    if (cmcb != NULL) {
        sccpcm_stop_timers(cmcb);
        sccpcm_free_timers(cmcb);

        sccpcm_cm_disconnect(cmcb);

        if (cmcb->chunk_data.residual_buffer != NULL) {
            SCCP_FREE(cmcb->chunk_data.residual_buffer);
            cmcb->chunk_data.residual_buffer = NULL;
        }

        SCCP_FREE(cmcb);
    }
}

void sccpcm_init_cmcb (sccp_sccpcb_t *sccpcb, int index,
                       sccpcm_ccm_types_t ccm_type,
                       unsigned long addr, unsigned short port)
{
    sccpcm_cmcb_t *cmcb;

    cmcb = sccpcm_get_cmcb_by_index(sccpcb, index);
    if (cmcb == NULL) {
        return;
    }

    cmcb->addr     = addr;
    cmcb->port     = port;
    cmcb->ccm_type = ccm_type;

    switch (ccm_type) {
    case (SCCPCM_CCM_TYPE_STANDARD):
    case (SCCPCM_CCM_TYPE_STANDARD_TFTP):
        cmcb->max_connection_retries = SCCPREC_CCM_CON_RETRIES;
		break;

    case (SCCPCM_CCM_TYPE_SRST_FALLBACK):
        cmcb->max_connection_retries = SCCPREC_SRST_CON_RETRIES;
		break;

    default:
        cmcb->max_connection_retries = 0;
		break;
    }
}

void *sccpcm_get_new_cmcb (sccp_sccpcb_t *sccpcb, int index)
{
    sccpcm_cmcb_t *cmcb;

    if (sccpcm_get_cm_count(sccpcb) >= SCCPMSG_MAX_SERVERS) {
        return (NULL);
    }

    cmcb = (sccpcm_cmcb_t *)(SCCP_MALLOC(sizeof(*cmcb)));
    if (cmcb == NULL) {
        return (NULL);
    }

    SCCP_MEMSET(cmcb, 0, sizeof(*cmcb));

    if (sllist_add_node(sccpcb->cmcbs, cmcb, 0) == NULL) {
        sccpcm_free_cmcb2(cmcb);
        return (NULL);
    }


    cmcb->misc_timer = SCCP_TIMER_ALLOCATE();
    if (cmcb->misc_timer == NULL) {
        sccpcm_free_cmcb(cmcb);
        return (NULL);
    }

    cmcb->keepalive_timer = SCCP_TIMER_ALLOCATE();
    if (cmcb->keepalive_timer == NULL) {
        sccpcm_free_cmcb(cmcb);
        return (NULL);
    }

    cmcb->sccpcb = sccpcb;
    //cmcb->id     = sccpcm_get_new_cmcb_id();
    cmcb->id     = sllist_get_new_id(sccpcb->cmcbs, &(sccpcb->cmcb_id));
    cmcb->index  = index;

    return (cmcb);
}

static void sccpcm_reset_cmcb (sccpcm_cmcb_t *cmcb)
{
    SCCP_TIMER_CANCEL(cmcb->misc_timer);
    SCCP_TIMER_CANCEL(cmcb->keepalive_timer);

    cmcb->connection_retries       = 0;
    cmcb->ack_rsp_retries          = 0;
    cmcb->timeouts_before_register = -1;
    cmcb->shutdown                 = 0;
    cmcb->ka_count                 = 0;
}

/*
 * Check if the CCM is stuck. Send an alarm for every six keepalives
 * that the CCM does not respond to a register.
 */
static void sccpcm_check_cm_stuck (sccpcm_cmcb_t *cmcb)
{
    char text[SCCPMSG_MAX_ALARM_TEXT_SIZE];
    char *mac = sccprec_get_mac(cmcb->sccpcb);

    if (cmcb->state != SCCPCM_S_WF_REG_RES) {
        return;
    }

    if (++(cmcb->ka_count) >= SCCPREC_CM_STUCK_ALARM_COUNT) {
        sccpmsg_create_alarm_string(text, cmcb->sccpcb->reccb->tcp_close_cause,
                                    mac);

        sccpmsg_send_alarm(&sccpcm_msgcb, cmcb->socket,
                           SCCPMSG_ALARM_SEVERITY_INFORMATIONAL,
                           SCCPMSG_ALARM_PARAM1,
                           SCCP_SOCKET_GETSOCKNAME(cmcb->socket),
                           text);

        cmcb->ka_count = 0;
    }
}

static int sccpcm_push_simple (sccp_sccpcb_t *sccpcb, int msg_id,
                               int index)
{
    sccpcm_cmcb_t *cmcb;

    cmcb = sccpcm_get_cmcb_by_index(sccpcb, index);
    if (cmcb == NULL) {
        return (11);
    }

    sccp_push_event(sccpcb, NULL, 0, msg_id, &sccpcm_sem_table, cmcb, 0);

    return (0);
}

int sccpcm_push_tcp_event (sccp_sccpcb_t *sccpcb, int msg_id,
                           unsigned int socket,
                           gapi_tcp_events_e event, void *data, int size)
{
    sccpcm_cmcb_t *cmcb;
    sccpcm_msg_tcp_events_t *msg;

    cmcb = sccpcm_get_cmcb_by_socket(sccpcb, socket);
    if (sccpcb == NULL)  {
        return (1);
    }

    msg = (sccpcm_msg_tcp_events_t *)(SCCP_MALLOC(sizeof(*msg)));
    if (msg == NULL) {
        return (2);
    }

    SCCP_MEMSET(msg, 0, sizeof(*msg));

    msg->msg_id = SCCPCM_E_TCP_EVENTS;
    msg->event  = event;
    
    sccp_push_event(sccpcb, msg, 0, SCCPCM_E_TCP_EVENTS,
                    &sccpcm_sem_table, cmcb, 0);

    return (0);
}

int sccpcm_push_con_req (sccp_sccpcb_t *sccpcb, int msg_id, int index)
{
    return (sccpcm_push_simple(sccpcb, SCCPCM_E_CON_REQ, index));
}

int sccpcm_push_connected (sccp_sccpcb_t *sccpcb, int msg_id, int index)
{
    return (sccpcm_push_simple(sccpcb, SCCPCM_E_CONNECTED, index));
}

int sccpcm_push_discon_req (sccp_sccpcb_t *sccpcb, int msg_id, int index)
{
    return (sccpcm_push_simple(sccpcb, SCCPCM_E_DISCON_REQ, index));
}

int sccpcm_push_reg_req (sccp_sccpcb_t *sccpcb, int msg_id, int index)
{
    return (sccpcm_push_simple(sccpcb, SCCPCM_E_REG_REQ, index));
}

int sccpcm_push_reg_ack (sccp_sccpcb_t *sccpcb, int msg_id,
                         int index, int cause,
                         unsigned long keepalive1,
                         unsigned long keepalive2)
{
    sccpcm_cmcb_t        *cmcb;
    sccpcm_msg_reg_ack_t *msg;

    cmcb = sccpcm_get_cmcb_by_index(sccpcb, index);
    if (cmcb == NULL) {
        return (11);
    }

    msg = (sccpcm_msg_reg_ack_t *)(SCCP_MALLOC(sizeof(*msg)));
    if (msg == NULL) {
        return (21);
    }

    SCCP_MEMSET(msg, 0, sizeof(*msg));

    msg->msg_id     = msg_id;
    msg->cause      = cause;
    msg->keepalive1 = keepalive1;
    msg->keepalive2 = keepalive2;

    sccp_push_event(sccpcb, msg, 0, SCCPCM_E_REG_ACK,
                    &sccpcm_sem_table, cmcb, 0);

    return (0);
}

int sccpcm_push_reg_nak (sccp_sccpcb_t *sccpcb, int msg_id, int index,
                         char *text)
{
    sccpcm_cmcb_t        *cmcb;
    sccpcm_msg_reg_nak_t *msg;

    cmcb = sccpcm_get_cmcb_by_index(sccpcb, index);
    if (cmcb == NULL) {
        return (11);
    }

    msg = (sccpcm_msg_reg_nak_t *)(SCCP_MALLOC(sizeof(*msg)));
    if (msg == NULL) {
        return (21);
    }

    SCCP_MEMSET(msg, 0, sizeof(*msg));

    msg->msg_id = msg_id;
	if (text != NULL) {
		SCCP_STRNCPY(msg->text, text, sizeof(msg->text));
        msg->text[SCCPMSG_DISPLAY_TEXT_SIZE] = '\0';
	}

    sccp_push_event(sccpcb, msg, 0, SCCPCM_E_REG_NAK,
                    &sccpcm_sem_table, cmcb, 0);

    return (0);
}

int sccpcm_push_registered (sccp_sccpcb_t *sccpcb, int msg_id, int index)
{
    return (sccpcm_push_simple(sccpcb, SCCPCM_E_REGISTERED, index));
}

int sccpcm_push_unreg_req (sccp_sccpcb_t *sccpcb, int msg_id, int index)
{
    return (sccpcm_push_simple(sccpcb, SCCPCM_E_UNREG_REQ, index));
}

int sccpcm_push_unreg_ack (sccp_sccpcb_t *sccpcb, int msg_id, int index,
                           int cause)
{
    return (sccpcm_push_simple(sccpcb, SCCPCM_E_UNREG_ACK, index));
}

int sccpcm_push_unreg_nak (sccp_sccpcb_t *sccpcb, int msg_id, int index,
                           int cause)
{
    return (sccpcm_push_simple(sccpcb, SCCPCM_E_UNREG_NAK, index));
}

int sccpcm_push_token_req (sccp_sccpcb_t *sccpcb, int msg_id, int index)
{
    return (sccpcm_push_simple(sccpcb, SCCPCM_E_SEND_TOKEN, index));
}

int sccpcm_push_keepalive_ack (sccp_sccpcb_t *sccpcb, int msg_id, int index)
{
    return (sccpcm_push_simple(sccpcb, SCCPCM_E_KEEPALIVE_ACK, index));
}

int sccpcm_push_error (sccp_sccpcb_t *sccpcb, int msg_id,
                       int index, int error)
{
    sccpcm_cmcb_t      *cmcb;
    sccpcm_msg_error_t *msg;

    if (sccpcb == NULL)  {
        return (1);
    }

    cmcb = sccpcm_get_cmcb_by_index(sccpcb, index);
    if (cmcb == NULL) {
        return (11);
    }

    msg = (sccpcm_msg_error_t *)(SCCP_MALLOC(sizeof(*msg)));
    if (msg == NULL) {
        return (21);
    }

    SCCP_MEMSET(msg, 0, sizeof(*msg));

    msg->msg_id = msg_id;
    msg->error  = error;

    sccp_push_event(sccpcb, msg, 0, SCCPCM_E_ERROR,
                    &sccpcm_sem_table, cmcb, 0);

    return (0);
}

static int sccpcm_push_timeout (sccp_sccpcb_t *sccpcb, int msg_id,
                                int index, int event)
{
    sccpcm_cmcb_t        *cmcb;
    sccpcm_msg_timeout_t *msg;

    if (sccpcb == NULL)  {
        return (1);
    }

    cmcb = sccpcm_get_cmcb_by_index(sccpcb, index);
    if (cmcb == NULL) {
        return (11);
    }

    msg = (sccpcm_msg_timeout_t *)(SCCP_MALLOC(sizeof(*msg)));
    if (msg == NULL) {
        return (2);
    }

    SCCP_MEMSET(msg, 0, sizeof(*msg));

    msg->msg_id = msg_id;
    msg->event  = event;

    sccp_push_event(sccpcb, msg, 0, msg_id,
                    &sccpcm_sem_table, cmcb, 0);

    return (0);
}

/*
 * FUNCTION:    sccpcm_sem_default
 *
 * DESCRIPTION: Default handler for invalid events.
 *
 * PARAMETERS:
 *     event:   event control block
 *
 * RETURNS:     0 for success, otherwise 1.
 *
 * NOTES:
 *
 * ============================================================================
 * S_ALL                     E_DEFAULT                     NO CHANGE
 * ----------------------------------------------------------------------------
 */
static int sccpcm_sem_default (sem_event_t *event)
{
    sccpcm_cmcb_t *cmcb = (sccpcm_cmcb_t *)(event->cb);

    SCCP_DBG((sccpcm_debug, 9,
             "%s %-2d:%-2d: %-20s: do nothing\n",
             SCCPCM_ID, cmcb->sccpcb->id, cmcb->id, event->function_name));

    return (0);
}

/*
 * FUNCTION:    sccpcm_sem_tcp_events
 *
 * DESCRIPTION: Process tcp events from the application. The function will
 *              translate the tcp event into a more usable sccpcm event.
 *
 * PARAMETERS:
 *     event:   event control block
 *
 * RETURNS:     0 for success, otherwise 1.
 *
 * NOTES:
 *
 * ============================================================================
 * S_ALL                     E_TCP_EVENT                   NO CHANGE
 * ----------------------------------------------------------------------------
 */
static int sccpcm_sem_tcp_events (sem_event_t *event)
{
    sccpcm_cmcb_t           *cmcb = (sccpcm_cmcb_t *)(event->cb);
    sccpcm_msg_tcp_events_t *msg  = (sccpcm_msg_tcp_events_t *)(event->data);        
    gapi_tcp_events_e       tcp_event = msg->event;

    SCCP_DBG((sccpcm_debug, 9,
             "%s %-2d:%-2d: %-20s: event= %d\n",
             SCCPCM_ID, cmcb->sccpcb->id, cmcb->id, event->function_name, tcp_event));

#if 0 /* test4 */ /* test34 */
    /* primary: 0, secondary: 1 */
    if (cmcb->index == 1) {
        tcp_event = GAPI_TCP_EVENT_NACK;
    }
#endif

    switch (tcp_event) {
    case (GAPI_TCP_EVENT_ACK):
    case (GAPI_TCP_EVENT_OPEN):
        sccpcm_push_simple(cmcb->sccpcb, SCCPCM_E_CON_ACK, cmcb->index);
        
        break;
        
    case (GAPI_TCP_EVENT_CLOSE):
        sccpcm_push_error(cmcb->sccpcb, SCCPCM_E_ERROR, cmcb->index,
                          SCCPCM_ERROR_TCP_CLOSE);
        
        break;

    case (GAPI_TCP_EVENT_NACK):
        if ((cmcb->state == SCCPCM_S_WF_CON_RES) &&
            (cmcb->connection_retries < cmcb->max_connection_retries)) {

            cmcb->connection_retries++;

            sccpcm_push_simple(cmcb->sccpcb, SCCPCM_E_CON_RETRY, cmcb->index);
        }
        else {
            sccpcm_push_error(cmcb->sccpcb, SCCPCM_E_ERROR, cmcb->index,
                              SCCPCM_ERROR_TCP_NAK);
        }

        break;
        
    case (GAPI_TCP_EVENT_RECV):
    default:
        break;
    }
    
    return (0);        
}

/*
 * FUNCTION:    sccpcm_sem_idle_con_req
 *
 * DESCRIPTION: Connect to a CiscoCallManager.
 *
 * PARAMETERS:
 *     event:   event control block
 *
 * RETURNS:     0 for success, otherwise 1.
 *
 * NOTES:
 *
 * ============================================================================
 * S_IDLE                    E_CON_REQ                     S_WF_CON_RES
 * ----------------------------------------------------------------------------
 */
static int sccpcm_sem_idle_con_req (sem_event_t *event)
{
    sccpcm_cmcb_t *cmcb = (sccpcm_cmcb_t *)(event->cb);
    int           rc    = 0;

    cmcb->connection_retries = 0;
    cmcb->ack_rsp_retries = 0;

#if 0 /*sam not needed - the sccprec checks to make sure the data ia valid. */
    /*
     * Make sure we have a valid address and port.
     */
    if ((cmcb->addr == 0) || (cmcb->port == 0)) {
        sccpcm_push_error(cmcb->sccpcb, SCCPCM_E_ERROR,
                          cmcb->index, SCCPCM_ERROR_NULL_ADDR_PORT);

        return (0);
    }
#endif

    /*
     * Close existing connections.
     */
    sccpcm_cm_disconnect(cmcb);

    rc = sccpcm_cm_connect(cmcb);
    if (rc == 0) {
        /*
         * Local side of the connect worked. Start a timer to wait
         * for a syn-ack from the far side.
         */
        /*
        * I know this looks funny since we are using the
        * keepalive_timer. We normally use the misc_timer
        * for any timer other than keepalive timers, but we need
        * another timer because the misc_timer will be used if we
        * have to go into the connect retry code. This timer here
        * will be killed when:
        * 1. timeout,
        * 2. connect suceeds - the real keepalive timer will be
        *    started which always cancels itself before starting,
        * 3. connect fails - the cmcb will be cleared which cancels
        *    all timers.
        */
        sccpcm_start_timer(cmcb, cmcb->keepalive_timer,
                           SCCPCM_TIMER_WF_CON_RES,
                           cmcb->sccpcb->reccb->connect_to);
#if 0 /* see note above */
        sccpcm_start_misc_timer(cmcb);
#endif
    }
    else {
        SCCP_DBG((sccpcm_debug, 8,
                 "%s %-2d:%-2d: %-20s: %s: unable to connect\n",
                 SCCPCM_ID, cmcb->sccpcb->id, cmcb->id,
                 event->function_name,
                 sccpcm_print_cm(cmcb)));

        sccpcm_push_error(cmcb->sccpcb, SCCPCM_E_ERROR,
                          cmcb->index, SCCPCM_ERROR_CANT_OPEN_TCP);
    }    

    return (0);
}

/*
 * FUNCTION:    sccpcm_sem_wf_con_res_con_ack
 *
 * DESCRIPTION: Handle the response for a successful socket connect. The
 *              function will start the keepalive timers and notify the 
 *              sccprec of the successful connection.
 *
 * PARAMETERS:
 *     event:   event control block
 *
 * RETURNS:     0 for success, otherwise 1.
 *
 * NOTES:
 *
 * ============================================================================
 * S_WF_CON_RES              E_CON_ACK                     S_CONNECTED
 * ----------------------------------------------------------------------------
 */
static int sccpcm_sem_wf_con_res_con_ack (sem_event_t *event)
{
    sccpcm_cmcb_t *cmcb = (sccpcm_cmcb_t *)(event->cb);

    /*
     * Grab space for the chunk messaging. We wait until.
     */
    /*
     * Start timer to wait for the keepalive_ack. This also cancels the timer
     * that was waiting for the con_ack - wf_con_res_to. The ka_ack_to was
     * reused during this time so that the misc timer would be available for
     * user if con_retry was started.
     */
    sccpcm_start_keepalive_timer(cmcb);

    SCCP_DBG((sccpcm_debug, 3,
             "%s %-2d:%-2d: %-20s: %s: connected\n",
             SCCPCM_ID, cmcb->sccpcb->id, cmcb->id,
             event->function_name,
             sccpcm_print_cm(cmcb)));

    sccpcm_sessionstatus(cmcb, GAPI_STATUS_CM_CONNECTED);

    sccprec_push_dev_notify(cmcb->sccpcb, SCCPREC_E_DEV_NOTIFY);

    return (0);
}

/*
 * FUNCTION:    sccpcm_sem_wf_con_res_con_retry
 *
 * DESCRIPTION: The socket connect to a CM has failed. Start retry procedures.
 *              Start a short timer to retry the connect.
 *
 * PARAMETERS:
 *     event:   event control block
 *
 * RETURNS:     0 for success, otherwise 1.
 *
 * NOTES:
 *
 * ============================================================================
 * S_WF_CON_RES              E_CON_RETRY                   S_WF_CON_RETRY
 * ----------------------------------------------------------------------------
 */
static int sccpcm_sem_wf_con_res_con_retry (sem_event_t *event)
{
    sccpcm_cmcb_t *cmcb = (sccpcm_cmcb_t *)(event->cb);

    /*
     * Start a timer so we can retry the connect. We need to wait before
     * we retry.
     */
    sccpcm_start_misc_timer(cmcb);

    return (0);
}

/*
 * FUNCTION:    sccpcm_sem_wf_con_res_timeout
 *
 * DESCRIPTION: The socket connect to a CM has timed out. Disconnect the socket
 *              and notify sccprec.
 *
 * PARAMETERS:
 *     event:   event control block
 *
 * RETURNS:     0 for success, otherwise 1.
 *
 * NOTES:
 *
 * ============================================================================
 * S_WF_CON_RES              E_TIMEOUT                     S_IDLE
 * ----------------------------------------------------------------------------
 */
static int sccpcm_sem_wf_con_res_timeout (sem_event_t *event)
{
    sccpcm_cmcb_t *cmcb = (sccpcm_cmcb_t *)(event->cb);

    /*
     * No reply from the other side, so let's just go to IDLE
     * and let the sccprec machine fix things.
     */
    sccpcm_cm_disconnect(cmcb);

    sccpcm_reset_cmcb(cmcb);

    sccprec_push_dev_notify(cmcb->sccpcb, SCCPREC_E_DEV_NOTIFY);

    return (0);
}

/*
 * FUNCTION:    sccpcm_sem_wf_con_res_error
 *
 * DESCRIPTION: Error has occurred while trying to connect to a CM.
 *              Disconnect the socket and notify sccprec.
 *
 * PARAMETERS:
 *     event:   event control block
 *
 * RETURNS:     0 for success, otherwise 1.
 *
 * NOTES:
 *
 * ============================================================================
 * S_ALL                     E_ERROR                       S_IDLE
 * ----------------------------------------------------------------------------
 */
static int sccpcm_sem_wf_con_res_error (sem_event_t *event)
{
    sccpcm_cmcb_t      *cmcb = (sccpcm_cmcb_t *)(event->cb);
    sccpcm_msg_error_t *msg  = (sccpcm_msg_error_t *)(event->data);

    SCCP_DBG((sccpcm_debug, 9,
             "%s %-2d:%-2d: %-20s: %s: error= %s\n",
             SCCPCM_ID, cmcb->sccpcb->id, cmcb->id,
             event->function_name,
             sccpcm_print_cm(cmcb),
             sccpcm_error_name(msg->error)));

    sccpcm_cm_disconnect(cmcb);

    sccpcm_reset_cmcb(cmcb);

    sccprec_push_dev_notify(cmcb->sccpcb, SCCPREC_E_DEV_NOTIFY);

    return (0);
}

/*
 * FUNCTION:    sccpcm_sem_wf_con_retry_con_req
 *
 * DESCRIPTION: Retry connect to a CiscoCallManager.
 *
 * PARAMETERS:
 *     event:   event control block
 *
 * RETURNS:     0 for success, otherwise 1.
 *
 * NOTES:
 *
 * ============================================================================
 * S_WF_CON_RETRY            E_CON_REQ                     S_WF_CON_RES
 * ----------------------------------------------------------------------------
 */
static int sccpcm_sem_wf_con_retry_con_req (sem_event_t *event)
{
    sccpcm_cmcb_t *cmcb = (sccpcm_cmcb_t *)(event->cb);
    int           rc    = 0;

    cmcb->ack_rsp_retries = 0;

#if 0 /*sam not needed - the sccprec checks to make sure the data ia valid. */
    /*
     * Make sure we have a valid address and port.
     */
    if ((cmcb->addr == 0) || (cmcb->port == 0)) {
        sccpcm_push_error(cmcb->sccpcb, SCCPCM_E_ERROR,
                          cmcb->index, SCCPCM_ERROR_NULL_ADDR_PORT);

        return (0);
    }
#endif
    if (sccpcm_get_active_cm(cmcb->sccpcb) == SCCPCM_NO_INDEX) {
        sccprec_set_tcp_close_cause(cmcb->sccpcb,
                                    SCCPMSG_ALARM_LAST_PHONE_ABORT);
    }

    /*
     * Close existing connections.
     */
    sccpcm_cm_disconnect(cmcb);

    rc = sccpcm_cm_connect(cmcb);
    if (rc == 0) {
        /*
         * Local side of the connect worked. Start a timer to wait
         * for a syn-ack from the far side.
         */
        /*sam
        * I know this looks funny since we are using the
        * keepalive_timer. We normally use the misc_timer
        * for any timer other than keepalive timers, but we need
        * another timer because the misc_timer will be used if we
        * have to go into the connect retry code. This timer here
        * will be killed when:
        * 1. timeout,
        * 2. connect suceeds - the real keepalive timer will be
        *    started which always cancels itself before starting,
        * 3. connect fails - the cmcb will be cleared which cancels
        *    all timers.
        */
#if 0 /* timer already started in idle_con_req */
        sccpcm_start_timer(cmcb, cmcb->keepalive_timer,
                           SCCPCM_TIMER_WF_CON_RES,
                           cmcb->sccpcb->reccb->connect_to);
#endif
#if 0 /* see above note */
        sccpcm_start_misc_timer(cmcb);
#endif
    }
    else {
        SCCP_DBG((sccpcm_debug, 8,
                 "%s %-2d:%-2d: %-20s: %s: unable to connect\n",
                 SCCPCM_ID, cmcb->sccpcb->id, cmcb->id,
                 event->function_name,
                 sccpcm_print_cm(cmcb)));

        sccpcm_push_error(cmcb->sccpcb, SCCPCM_E_ERROR,
                          cmcb->index, SCCPCM_ERROR_CANT_OPEN_TCP);
    }    

    return (0);
}

/*
 * FUNCTION:    sccpcm_sem_wf_con_retry_timeout
 *
 * DESCRIPTION: Timeout has occurred while waiting to retry a socket connect.
 *              Proceed with a connect retry.
 *
 * PARAMETERS:
 *     event:   event control block
 *
 * RETURNS:     0 for success, otherwise 1.
 *
 * NOTES:
 *     - this function can do what sccpcm_sem_wf_con_retry_con_req instead
 *       of sending a con_req event to get to it.
 *
 * ============================================================================
 * S_WF_CON_RETRY            E_TIMEOUT                     S_WF_CON_RETRY
 * ----------------------------------------------------------------------------
 */
static int sccpcm_sem_wf_con_retry_timeout (sem_event_t *event)
{
    sccpcm_cmcb_t *cmcb = (sccpcm_cmcb_t *)(event->cb);
    
    sccpcm_push_con_req(cmcb->sccpcb, SCCPCM_E_CON_REQ, cmcb->index);

    return (0);
}

/*
 * FUNCTION:    sccpcm_sem_connected_send_token
 *
 * DESCRIPTION: Start the token request processing. Request a token from the
 *              CM.
 *
 * PARAMETERS:
 *     event:   event control block
 *
 * RETURNS:     0 for success, otherwise 1.
 *
 * NOTES:
 *
 * ============================================================================
 * S_CONNECTED               E_SEND_TOKEN                  S_CONNECTED
 * ----------------------------------------------------------------------------
 */
static int sccpcm_sem_connected_send_token (sem_event_t *event)
{
    sccpcm_cmcb_t *cmcb = (sccpcm_cmcb_t *)(event->cb);

    if (sccpmsg_send_register_token_req(&sccpcm_msgcb, cmcb->socket,
                                        sccprec_get_mac(cmcb->sccpcb),
                                        SCCPMSG_DEVICE_TYPE_STATION_TELECASTER_MGR,
                                        cmcb->addr) == 0) {
        /*
         * Set the number of keepalive timeouts that we can have before we
         * receive a token. If we have too many timeouts then we will just
         * assume that we have a token and try to register with the CM.
         * Add 1 since we don't know how much longer we have before
         * the next timeout. 
         */
        cmcb->timeouts_before_register = SCCPREC_KA_TO_BEFORE_NO_TOKEN_REG + 1;
    }
    else {
        sccpcm_push_error(cmcb->sccpcb, SCCPCM_E_ERROR,
                          cmcb->index, SCCPCM_ERROR_CANT_SEND_TOKEN);
    }

    return (0);
}    

/*
 * FUNCTION:    sccpcm_sem_connected_recv_token
 *
 * DESCRIPTION: A token has been received. Notify sccprec.
 *
 * PARAMETERS:
 *     event:   event control block
 *
 * RETURNS:     0 for success, otherwise 1.
 *
 * NOTES:
 *
 * ============================================================================
 * S_CONNECTED               E_RECV_TOKEN                  S_TOKEN
 * ----------------------------------------------------------------------------
 */
static int sccpcm_sem_connected_recv_token (sem_event_t *event)
{
    sccpcm_cmcb_t *cmcb = (sccpcm_cmcb_t *)(event->cb);

    cmcb->timeouts_before_register = -1;

    /*
     * Make sure the wf_retry_to is cancelled.
     */
    sccpcm_stop_timer(cmcb, cmcb->misc_timer);
    //cmcb->ms_before_send_message   = -1;

    sccprec_set_tcp_close_cause(cmcb->sccpcb,
                                SCCPMSG_ALARM_LAST_FAILBACK);

    sccprec_push_dev_notify(cmcb->sccpcb, SCCPREC_E_DEV_NOTIFY);

    return (0);
}

/*
 * FUNCTION:    sccpcm_sem_token_rej
 *
 * DESCRIPTION: Register token request has been rejected. Start a timer
 *              to retry the request.
 *
 * PARAMETERS:
 *     event:   event control block
 *
 * RETURNS:     0 for success, otherwise 1.
 *
 * NOTES:
 *
 * ============================================================================
 * S_CONNECTED               E_TOKEN_REJ                   S_CONNECTED
 * ----------------------------------------------------------------------------
 */
static int sccpcm_sem_token_rej (sem_event_t *event)
{
    sccpcm_cmcb_t                   *cmcb    = (sccpcm_cmcb_t *)(event->cb);
    sccpmsg_general_t               *gen_msg = (sccpmsg_general_t *)(event->data);
    sccpmsg_register_token_reject_t *msg     = &(gen_msg->body.register_token_reject);
    unsigned long                   wait;

    /*
     * Convert seconds to milliseconds.
     */
    wait = msg->wait_time_before_next_reg * 1000;

    /*
     * Make sure the wait time is at least the minimum.
     */
    if (wait < SCCPREC_RETRY_TOKEN_REQ_TO) {
        wait = SCCPREC_RETRY_TOKEN_REQ_TO;
    }

    sccpcm_start_timer(cmcb, cmcb->misc_timer, SCCPCM_TIMER_TOKEN_RETRY,
		               wait);

    /*
     * Need to cancel the sccprec timer that is waiting for the token stuff
     * to finish.
     */
    /*
     * We would need to start the sccrec misc_timer again when this timeout
     * here occurs. Then we would need to track the number of times we get
     * a token_reject and just go ahead and register. So, let's just keep
     * the sccprec misc_timer going, let it timeout and force us to
     * register.
     */
    //sccprec_stop_timer(cmcb->sccpcb->reccb, cmcb->sccpcb->reccb->misc_timer);

    cmcb->timeouts_before_register = -1;

    return (0);
}    

/*
 * FUNCTION:    sccpcm_sem_connected_reg_req
 *
 * DESCRIPTION: Begin registration procedures.
 *
 * PARAMETERS:
 *     event:   event control block
 *
 * RETURNS:     0 for success, otherwise 1.
 *
 * NOTES:
 *
 * ============================================================================
 * S_CONNECTED               E_REG_REG                     S_WF_RES_RES
 * ----------------------------------------------------------------------------
 * S_TOKEN                   E_REG_REG                     S_WF_RES_RES
 * ----------------------------------------------------------------------------
 */
static int sccpcm_sem_connected_reg_req (sem_event_t *event)
{
    sccpcm_cmcb_t *cmcb = (sccpcm_cmcb_t *)(event->cb);

    cmcb->ack_rsp_retries          = 0;
    cmcb->timeouts_before_register = -1;
    //cmcb->ms_before_send_message   = -1; //timer_cancel
    sccpcm_stop_timer(cmcb, cmcb->misc_timer);

    /*
     * Connected - go ahead and fire off the registration state machine.
     */

    sccpreg_push_reg_req(cmcb->sccpcb, SCCPREG_E_REG_REQ);


#if 0
    /*sam - keepalive timer should have already been started
     * when the socket connected successfully.
     */

    /*
     * Start timer to wait for the keepalive_ack.
     */
    sccpcm_start_keepalive_timer(cmcb);
#endif
#if 1 //sccprec starts timer
    /*
     * Start timer to wait for a register_ack.
     */
    sccpcm_start_misc_timer(cmcb);
#endif

    return (0);
}

/*
 * FUNCTION:    sccpcm_sem_connected_discon_req
 *
 * DESCRIPTION: Disconnect from the CM.
 *
 * PARAMETERS:
 *     event:   event control block
 *
 * RETURNS:     0 for success, otherwise 1.
 *
 * NOTES:
 *
 * ============================================================================
 * S_CONNECTED               E_DISCON_REQ                  S_IDLE
 * ----------------------------------------------------------------------------
 * S_TOKEN                   E_DISCON_REQ                  S_IDLE
 * ----------------------------------------------------------------------------
 * S_LOCKOUT                 E_DISCON_REQ                  S_IDLE
 * ----------------------------------------------------------------------------
 */
static int sccpcm_sem_connected_discon_req (sem_event_t *event)
{
    sccpcm_cmcb_t *cmcb = (sccpcm_cmcb_t *)(event->cb);

    sccpcm_cm_disconnect(cmcb);

    sccpcm_reset_cmcb(cmcb);

    sccprec_push_dev_notify(cmcb->sccpcb, SCCPREC_E_DEV_NOTIFY);

    return (0);
}

/*
 * FUNCTION:    sccpcm_sem_connected_ka_ack
 *
 * DESCRIPTION: keepalive_ack received. Clear the retry count and wait for
 *              timeout to send another keepalive.
 *
 * PARAMETERS:
 *     event:   event control block
 *
 * RETURNS:     0 for success, otherwise 1.
 *
 * NOTES:
 *
 * ============================================================================
 * S_CONNECTED               E_KEEPALIVE_ACK               S_CONNECTED
 * ----------------------------------------------------------------------------
 * S_TOKEN                   E_KEEPALIVE_ACK               S_TOKEN      
 * ----------------------------------------------------------------------------
 * S_REGISTERED              E_KEEPALIVE_ACK               S_REGISTERED
 * ----------------------------------------------------------------------------
*/
static int sccpcm_sem_connected_ka_ack (sem_event_t *event)
{
    sccpcm_cmcb_t *cmcb = (sccpcm_cmcb_t *)(event->cb);

    /*
     * Clear the retry count.
     */
    cmcb->ack_rsp_retries = 0;
#if 0    
    /*
     * Check if the CM is having problems while we were trying to request
     * a token.
     */
    if (cmcb->timeouts_before_register == 0) {
        /*
         * Maybe the CM is overloaded right now. Just simulate that a token
         * was received and see if that helps.
         */
        cmcb->timeouts_before_register = -1;

        sccpcm_push_simple(cmcb->sccpcb, SCCPCM_E_RECV_TOKEN, cmcb->index);
    }
#endif
    return (0);
}

/*
 * FUNCTION:    sccpcm_sem_connected_timeout
 *
 * DESCRIPTION: Timeout has occurred. Send a keepalive and start another
 *              timer to wait for the keepalive_ack.
 *
 * PARAMETERS:
 *     event:   event control block
 *
 * RETURNS:     0 for success, otherwise 1.
 *
 * NOTES:
 *
 * ============================================================================
 * S_CONNECTED               E_TIMEOUT                     S_CONNECTED
 * ----------------------------------------------------------------------------
 * S_TOKEN                   E_TIMEOUT                     S_TOKEN
 * ----------------------------------------------------------------------------
 * S_WF_REG_RES              E_TIMEOUT                     S_WF_REG_RES
 * ----------------------------------------------------------------------------
 * S_REGISTERED              E_TIMEOUT                     S_REGISTERED
 * ----------------------------------------------------------------------------
 */
static int sccpcm_sem_connected_timeout (sem_event_t *event)
{
    sccpcm_cmcb_t *cmcb = (sccpcm_cmcb_t *)(event->cb);

    sccpcm_check_cm_stuck(cmcb);

    /*
     * Increment the retry count, since we did not receive
     * a keepalive_ack.
     */
    cmcb->ack_rsp_retries++;

    SCCP_DBG((sccpcm_debug, 9,
             "%s %-2d:%-2d: %-20s: %s: retry= %d\n",
             SCCPCM_ID, cmcb->sccpcb->id, cmcb->id,
             event->function_name,
             sccpcm_print_cm(cmcb),
             cmcb->ack_rsp_retries));

    /*
     * Mark this cm as bad if we have missed too many keepalive_acks.
     */
    if (cmcb->ack_rsp_retries > SCCPREC_ACK_RETRIES_BEFORE_LOCKOUT) {
        /*
         * Bomb the cmcb if this was the primary or we were trying to make
         * it the primary, otherwise lockout.
         */
        if (cmcb->state == SCCPCM_S_WF_REG_RES) {
#if 0
            sccpcm_push_simple(cmcb->sccpcb, SCCPCM_E_WF_REG_RES_TO,
                               cmcb->index);
#else
            event->function_name = 
                sccpcm_function_name(SCCPCM_SEM_WF_REG_RES_WF_REG_RES_TO);
            sccpcm_sem_wf_reg_res_wf_reg_res_to(event);
#endif
        }
        else if (cmcb->state == SCCPCM_S_REGISTERED) {
            sccpcm_push_error(cmcb->sccpcb, SCCPCM_E_ERROR,
                              cmcb->index, SCCPCM_ERROR_NO_KA_ACK);
        }
        else {
            sccpcm_push_simple(cmcb->sccpcb, SCCPCM_E_LOCKOUT, cmcb->index);
        }

        return (0);
    }
    else {
        /*
         * Let's try again.
         */
        if (sccpmsg_send_bare_message(&sccpcm_msgcb, cmcb->socket,
                                      SCCPMSG_KEEPALIVE) != 0) {

            sccpcm_push_error(cmcb->sccpcb, SCCPCM_E_ERROR,
                              cmcb->index, SCCPCM_ERROR_CANT_SEND_KA);

            return (0);
        }
        else {
            /*
             * Start timer to wait for the keepalive_ack.
             */
            sccpcm_start_keepalive_timer(cmcb);

		    /*
		     * Simulate that a keepalive was received. The CM will not respond
		     * to a keepalive request unless the client has registered.
		     * This cm is only connected and not registered, therefore,
		     * we just pretend that an ack was received to keep the state
		     * machine happy.
		     */
            /* test code
             * comment out the push below to simulate keepalive_ack failures.
             */
            
            /*
             * This is all that needs to be done for cms in the registered state.
             */
            if (cmcb->state == SCCPCM_S_REGISTERED) {
                return (0);
            }
    
            sccpcm_push_simple(cmcb->sccpcb, SCCPCM_E_KEEPALIVE_ACK, cmcb->index);
        }
    }

    /*
     * Track the number of timeouts if we are trying to 
     * receive a token. If we have too many timeouts then we will just
     * assume that we have a token and try to register with the CM.
     * Add 1 since we don't know how much longer we have before
     * the next timeout. 
     */
    
    if (cmcb->timeouts_before_register != -1) {
        cmcb->timeouts_before_register--;

        /*
         * Check if the CM is having problems while we were trying to request
         * a token.
         */
        if (cmcb->timeouts_before_register == 0) {
            /*
             * Maybe the CM is overloaded right now. Simulate that a token
             * was received and see if that helps.
             */
            cmcb->timeouts_before_register = -1;

            sccpcm_push_simple(cmcb->sccpcb, SCCPCM_E_RECV_TOKEN, cmcb->index);
        }
    }

    return (0);
}

/*
 * FUNCTION:    sccpcm_sem_connected_lockout
 *
 * DESCRIPTION: Lockout has occurred. The CM has missed responding with a 
 *              keepalive_ack for three intervals. Notify the sccprec.
 *
 * PARAMETERS:
 *     event:   event control block
 *
 * RETURNS:     0 for success, otherwise 1.
 *
 * NOTES:
 *
 * ============================================================================
 * S_CONNECTED               E_LOCKOUT                     S_LOCKOUT
 * ----------------------------------------------------------------------------
 * S_TOKEN                   E_LOCKOUT                     S_LOCKOUT
 * ----------------------------------------------------------------------------
 */
static int sccpcm_sem_connected_lockout (sem_event_t *event)
{
    sccpcm_cmcb_t *cmcb = (sccpcm_cmcb_t *)(event->cb);

    cmcb->ack_rsp_retries = 0;
    cmcb->timeouts_before_register = -1;
    //cmcb->ms_before_send_message   = -1; //timer_cancel
//    sccpcm_stop_timer(cmcb, cmcb->misc_timer);

    /*
     * Start a timer to supervise the lockout. We will keep
     * the connection up and send keepalives until
     * this timeout.
     */
    sccpcm_start_misc_timer(cmcb);
    sccpcm_start_keepalive_timer(cmcb);

    sccprec_push_dev_notify(cmcb->sccpcb, SCCPREC_E_DEV_NOTIFY);

    return (0);
}

/*
 * FUNCTION:    sccpcm_sem_token_unreg_nak
 *
 * DESCRIPTION: Handle unreg_ack while holding token. Move cm to connected
 *              state. This happens when fallback procedures have been 
 *              initiated and the CM we tried to unregister will not
 *              unregister. That sccpcm will send unreg_nak to this sccpcm
 *              to move it to the connected state. This ensures that everything
 *              is back to the way it was before the fallback and allow sccprec
 *              to start over.
 *
 * PARAMETERS:
 *     event:   event control block
 *
 * RETURNS:     0 for success, otherwise 1.
 *
 * NOTES:
 *
 * ============================================================================
 * S_TOKEN                   E_UNREG_NAK                   S_CONNECTED
 * ----------------------------------------------------------------------------
 */
static int sccpcm_sem_token_unreg_nak (sem_event_t *event)
{
    return (0);
}

/*
 * FUNCTION:    sccpcm_sem_wf_reg_res_reg_ack
 *
 * DESCRIPTION: Handle register_ack while waiting for register_ack. Copy 
 *              the relevant register information and notify sccprec.
 *
 * PARAMETERS:
 *     event:   event control block
 *
 * RETURNS:     0 for success, otherwise 1.
 *
 * NOTES:
 *
 * ============================================================================
 * S_WF_REG_RES              E_REG_ACK                     S_REGISTERED
 * ----------------------------------------------------------------------------
 */
static int sccpcm_sem_wf_reg_res_reg_ack (sem_event_t *event)
{
    sccpcm_cmcb_t        *cmcb  = (sccpcm_cmcb_t *)(event->cb);
    sccpcm_msg_reg_ack_t *msg   = (sccpcm_msg_reg_ack_t *)(event->data);
    int                  period;
    char                 *mac;

    /*
     * Stop the wf_reg_res_to.
     */
    sccpcm_stop_timer(cmcb, cmcb->misc_timer);

    /*
     * Reset the ack counter since we received an ack.
     */
    cmcb->ack_rsp_retries = 0;
    cmcb->ka_count = 0;

#if 0 /*sam not needed - the sccprec checks to make sure the data ia valid
        before it creates the new cmcb. */
    /*
     * Make sure we have a valid address and port.
     */
    if ((cmcb->addr == 0) || (cmcb->port == 0)) {
        sccpcm_push_error(cmcb->sccpcb, SCCPCM_E_ERROR,
                          cmcb->index, SCCPCM_ERROR_NULL_ADDR_PORT);

        return (0);
    }
#endif
    SCCP_DBG((sccpcm_debug, 8,
             "%s %-2d:%-2d: %-20s: %s: registered\n",
             SCCPCM_ID, cmcb->sccpcb->id, cmcb->id,
             event->function_name,
             sccpcm_print_cm(cmcb)));

    sccprec_set_fallback(cmcb->sccpcb, cmcb->ccm_type, 1);

    /*
     * Convert keepalive seconds to milliseconds.
     */
    sccprec_set_keepalive_timeout(cmcb->sccpcb,
                                  msg->keepalive1 * 1000, 1);

    sccprec_set_keepalive_timeout(cmcb->sccpcb,
                                  msg->keepalive2 * 1000, 2);

    /*
     * Randomize first timeout between 1 and 17 seconds. We already
     * started a keepalive timer when the connection was first connected
     * for 10s, but that one will be canceled when this timeout is set.
     */
    mac = sccprec_get_mac(cmcb->sccpcb);
    period = ((mac[5] * 100) % 17) + 1;
    sccpcm_start_timer(cmcb, cmcb->keepalive_timer,
                       SCCPCM_TIMER_WF_KA_ACK,
                       period * 1000); /* change to ms */

    sccprec_push_dev_notify(cmcb->sccpcb, SCCPREC_E_DEV_NOTIFY);

    /*
     * Notify the application that this CM is registered.
     */
    sccpcm_sessionstatus(cmcb, GAPI_STATUS_CM_REGISTERED);

    return (0);
}

/*
 * FUNCTION:    sccpcm_sem_wf_reg_res_reg_nak
 *
 * DESCRIPTION: Handle register_nak while waiting for register_ack. Issue
 *              error to cleanup sccpcm.
 *
 * PARAMETERS:
 *     event:   event control block
 *
 * RETURNS:     0 for success, otherwise 1.
 *
 * NOTES:
 *
 * ============================================================================
 * S_WF_REG_RES              E_REG_NAK                     S_WF_REG_RES
 * ----------------------------------------------------------------------------
 */
static int sccpcm_sem_wf_reg_res_reg_nak (sem_event_t *event)
{
    sccpcm_cmcb_t        *cmcb  = (sccpcm_cmcb_t *)(event->cb);
    sccpcm_msg_reg_nak_t *msg   = (sccpcm_msg_reg_nak_t *)(event->data);

    SCCP_DBG((sccpcm_debug, 9,
             "%s %-2d:%-2d: %-20s: %s: register rejected= %s\n",
             SCCPCM_ID, cmcb->sccpcb->id, cmcb->id,
             event->function_name,
             sccpcm_print_cm(cmcb), msg->text == NULL ? "" : msg->text));

    /*
     * Stop the wf_reg_res_to.
     */
    sccpcm_stop_timer(cmcb, cmcb->misc_timer);

    sccprec_set_register_reject(cmcb->sccpcb, 1);

    sccprec_set_tcp_close_cause(cmcb->sccpcb,
                                SCCPMSG_ALARM_LAST_REG_REJ);

    sccpcm_push_error(cmcb->sccpcb, SCCPCM_E_ERROR, cmcb->index,
                      SCCPCM_ERROR_REGISTER_REJECT);

    return (0);
}

/*
 * FUNCTION:    sccpcm_sem_wf_reg_res_timeout
 *
 * DESCRIPTION: Timeout for a keepalive_ack has occurred.
 *
 * PARAMETERS:
 *     event:   event control block
 *
 * RETURNS:     0 for success, otherwise 1.
 *
 * NOTES:
 *
 * ============================================================================
 * S_WF_REG_RES              E_TIMEOUT                     S_WF_REG_RES_KA_ACK
 * ----------------------------------------------------------------------------
 */
static int sccpcm_sem_wf_reg_res_timeout (sem_event_t *event)
{
    sccpcm_cmcb_t *cmcb = (sccpcm_cmcb_t *)(event->cb);

    if (sccpmsg_send_bare_message(&sccpcm_msgcb, cmcb->socket,
                                  SCCPMSG_KEEPALIVE) != 0) {

        sccpcm_push_error(cmcb->sccpcb, SCCPCM_E_ERROR,
                          cmcb->index, SCCPCM_ERROR_CANT_SEND_KA);
    }
    else {
        /*
         * Start timer to wait for the keepalive_ack.
         */
        sccpcm_start_keepalive_timer(cmcb);
    }

    /*
     * Check if the CCM is stuck. Send an alarm for every six keepalives
     * that the CCM does not respond to a register.
     */
    sccpcm_check_cm_stuck(cmcb);

    return (0);
}

/*
 * FUNCTION:    sccpcm_sem_wf_reg_res_wf_reg_res_to
 *
 * DESCRIPTION: Timeout while waiting for reg_res. Unregister sccpreg and
 *              error the sccpcm to cleanup.
 *
 * PARAMETERS:
 *     event:   event control block
 *
 * RETURNS:     0 for success, otherwise 1.
 *
 * NOTES:
 *
 * ============================================================================
 * S_WF_REG_RES              E_WF_REG_RES_TO               S_WF_REG_RES
 * ----------------------------------------------------------------------------
 */
static int sccpcm_sem_wf_reg_res_wf_reg_res_to (sem_event_t *event)
{
    sccpcm_cmcb_t *cmcb = (sccpcm_cmcb_t *)(event->cb);

    /*
     * Unregister the sccpreg to clean it up.
     */
    sccpreg_push_unreg_req(cmcb->sccpcb, SCCPREG_E_UNREG_REQ);

    /*
     * Push unregister_ack because we probably will not receive one from
     * the CCM since we never registered with it.
     */
    sccpreg_push_unregister_ack(cmcb->sccpcb, SCCPREG_E_UNREGISTER_ACK,
                                SCCPMSG_UNREGISTER_STATUS_OK);

    sccprec_set_tcp_close_cause(cmcb->sccpcb,
                                SCCPMSG_ALARM_LAST_CM_ABORT_TCP);

    sccpcm_push_error(cmcb->sccpcb, SCCPCM_E_ERROR, cmcb->index,
                      SCCPCM_ERROR_REGISTER_TO);

    return (0);
}

/*
 * FUNCTION:    sccpcm_sem_wf_reg_res_ka_ack
 *
 * DESCRIPTION: Handle keepalive_ack.
 *
 * PARAMETERS:
 *     event:   event control block
 *
 * RETURNS:     0 for success, otherwise 1.
 *
 * NOTES:
 *
 * ============================================================================
 * S_WF_REG_RES              E_KEEPALIVE_ACK               S_WF_REG_RES
 * ----------------------------------------------------------------------------
 */
/*sam make sure a keepalive timer is running*/
static int sccpcm_sem_wf_reg_res_ka_ack (sem_event_t *event)
{
    sccpcm_cmcb_t *cmcb = (sccpcm_cmcb_t *)(event->cb);

    /*
     * Might need to scale back the keepalive timeout if we are still
     * set to the minimum default.
     */
	if (sccprec_get_keepalive_to(cmcb->sccpcb, 2) == 
        sccprec_get_keepalive_to(cmcb->sccpcb, 0)) {

		/*
		 * This is the first keepalive_ack returned and before the
		 * register_ack was received, so let's scale back to the
		 * wf_reg_ack_to so that we do not pound the CM.
		 */
#if 0
		 sccprec_set_keepalive_timeout(cmcb->sccpcb,
									  SCCPREC_WF_REG_ACK_KA_TO, 1);
#endif
		 sccprec_set_keepalive_timeout(cmcb->sccpcb,
									   SCCPREC_WF_REG_ACK_KA_TO, 2);
	}
    
    /*
     * ACK received, reset the counter.
     */
    cmcb->ack_rsp_retries = 0;

    return (0);
}

/*
 * FUNCTION:    sccpcm_sem_unreg_req
 *
 * DESCRIPTION: Handle unregister request from sccprec. Push unreg_reg to
 *              sccpreg and wait for response.
 *
 * PARAMETERS:
 *     event:   event control block
 *
 * RETURNS:     0 for success, otherwise 1.
 *
 * NOTES:
 *
 * ============================================================================
 * S_WF_REG_RES              E_UNREG_REQ                   S_WF_UNREG_RES
 * ----------------------------------------------------------------------------
 * S_REGISTERED              E_UNREG_REQ                   S_WF_UNREG_RES
 * ----------------------------------------------------------------------------
 */
static int sccpcm_sem_unreg_req (sem_event_t *event)
{
    sccpcm_cmcb_t *cmcb = (sccpcm_cmcb_t *)(event->cb);

    /*
     * Reset the retry count.
     */
    cmcb->ack_rsp_retries = 0;
    //cmcb->ms_before_send_message = -1;
    //sccpcm_stop_timer(cmcb, cmcb->misc_timer);

    sccpreg_push_unreg_req(cmcb->sccpcb, SCCPREG_E_UNREG_REQ);

	sccpcm_start_misc_timer(cmcb);

    return (0);
}

/*
 * FUNCTION:    sccpcm_sem_registered_error
 *
 * DESCRIPTION: Handle error condition. Unregister from CM and disconnect.
 *
 * PARAMETERS:
 *     event:   event control block
 *
 * RETURNS:     0 for success, otherwise 1.
 *
 * NOTES:
 *
 * ============================================================================
 * S_REGISTERED              E_ERROR                       S_WF_UNREG_RES
 * ----------------------------------------------------------------------------
 */
static int sccpcm_sem_registered_error (sem_event_t *event)
{
    sccpcm_cmcb_t      *cmcb = (sccpcm_cmcb_t *)(event->cb);
    sccpcm_msg_error_t *msg  = (sccpcm_msg_error_t *)(event->data);

    SCCP_DBG((sccpcm_debug, 9,
             "%s %-2d:%-2d: %-20s: %s: error= %s\n",
             SCCPCM_ID, cmcb->sccpcb->id, cmcb->id,
             event->function_name,
             sccpcm_print_cm(cmcb),
             sccpcm_error_name(msg->error)));

    sccpcm_reset_cmcb(cmcb);
    sccprec_set_fallback(cmcb->sccpcb, cmcb->ccm_type, 0);

    if (msg->error == SCCPCM_ERROR_TCP_CLOSE) {
        sccprec_set_tcp_close_cause(cmcb->sccpcb,
                                    SCCPMSG_ALARM_LAST_CM_RESET_TCP);
    }

    /* 
     * Unregister from the CM, do not wait for the ACK.
     */
    sccpreg_push_unreg_req(cmcb->sccpcb, SCCPREG_E_UNREG_REQ);

    /*
     * Set the shutdown flag so that we will issue a discon_req later
     * down the road.
     */
//    cmcb->shutdown = 1;

    sccpreg_push_unregister_ack(cmcb->sccpcb, SCCPREG_E_UNREGISTER_ACK,
                                SCCPMSG_UNREGISTER_STATUS_OK);

    /*
     * Don't really care if we get a response to the unreg_req, so let's
     * just issue one now to keep the cm state machine happy.
     */
    sccpcm_push_unreg_ack(cmcb->sccpcb, SCCPCM_E_UNREG_ACK,
                          cmcb->index, GAPI_CAUSE_OK);

    sccpcm_push_discon_req(cmcb->sccpcb, SCCPCM_E_DISCON_REQ,
                           cmcb->index);

    sccprec_push_dev_notify(cmcb->sccpcb, SCCPREC_E_DEV_NOTIFY);

    return (0);
}

/*
 * FUNCTION:    sccpcm_sem_registered_timeout
 *
 * DESCRIPTION: Handle timeout. Send keepalive and wait for keepalive_ack.
 *
 * PARAMETERS:
 *     event:   event control block
 *
 * RETURNS:     0 for success, otherwise 1.
 *
 * NOTES:
 *
 * ============================================================================
 * S_REGISTERED              E_TIMEOUT                     S_REGISTERED
 * ----------------------------------------------------------------------------
 */
static int sccpcm_sem_registered_timeout (sem_event_t *event)
{
    sccpcm_cmcb_t *cmcb = (sccpcm_cmcb_t *)(event->cb);

    if (sccpmsg_send_bare_message(&sccpcm_msgcb, cmcb->socket,
                                  SCCPMSG_KEEPALIVE) != 0) {

        sccpcm_push_error(cmcb->sccpcb, SCCPCM_E_ERROR,
                          cmcb->index, SCCPCM_ERROR_CANT_SEND_KA);
    }
    else {
        /*
         * Start timer to wait for the keepalive_ack.
         */
        sccpcm_start_keepalive_timer(cmcb);
    }

    return (0);
}

/*
 * FUNCTION:    sccpcm_sem_registered_lockout
 *
 * DESCRIPTION: Perform lockout procedures.
 *
 * PARAMETERS:
 *     event:   event control block
 *
 * RETURNS:     0 for success, otherwise 1.
 *
 * NOTES:
 *
 * ============================================================================
 * S_REGISTERED              E_LOCKOUT                     S_LOCKOUT
 * ----------------------------------------------------------------------------
 */
static int sccpcm_sem_registered_lockout (sem_event_t *event)
{
    sccpcm_cmcb_t *cmcb = (sccpcm_cmcb_t *)(event->cb);

    //cmcb->ms_before_send_message = -1; //timer_cancel
    sccpcm_stop_timer(cmcb, cmcb->misc_timer);

    sccprec_set_fallback(cmcb->sccpcb, cmcb->ccm_type, 0);

    sccprec_set_tcp_close_cause(cmcb->sccpcb, SCCPMSG_ALARM_LAST_KA_TO);

    /*
     * Let the application know that we are not connected
     * to a primary CM.
     */
    sccpcm_sessionstatus(cmcb, GAPI_STATUS_CM_DOWN);

    /*
     * Unregister from the CM, do not wait for the ACK.
     */
    sccpreg_push_unreg_req(cmcb->sccpcb, SCCPREG_E_UNREG_REQ);

    sccpreg_push_unregister_ack(cmcb->sccpcb, SCCPREG_E_UNREGISTER_ACK,
                                SCCPMSG_UNREGISTER_STATUS_OK);

    /*
     * Don't really care if we get a response to the unreg_req, so let's
     * just issue one now to keep the cm state machine happy.
     */
    sccpcm_push_unreg_ack(cmcb->sccpcb, SCCPCM_E_UNREG_ACK,
                          cmcb->index, GAPI_CAUSE_OK);

    sccprec_push_dev_notify(cmcb->sccpcb, SCCPREC_E_DEV_NOTIFY);

    return (0);
}

/*
 * FUNCTION:    sccpcm_sem_wf_unreg_res_unreg_ack
 *
 * DESCRIPTION: Handle unregister_ack. Set fallback and notify the application
 *              that the CM is down.
 *
 * PARAMETERS:
 *     event:   event control block
 *
 * RETURNS:     0 for success, otherwise 1.
 *
 * NOTES:
 *
 * ============================================================================
 * S_WF_UNREG_RES            E_UNREG_ACK                   S_CONNECTED
 * ----------------------------------------------------------------------------
 */
static int sccpcm_sem_wf_unreg_res_unreg_ack (sem_event_t *event)
{
    sccpcm_cmcb_t *cmcb = (sccpcm_cmcb_t *)(event->cb);

    sccprec_set_fallback(cmcb->sccpcb, cmcb->ccm_type, 0);

    /*
     * Let the application know that we are not connected
     * to a primary CM.
     */
    sccpcm_sessionstatus(cmcb, GAPI_STATUS_CM_DOWN);

#if 0
    /*
     * Disconnect the CM if we are shutting-down.
     */
    if (cmcb->shutdown == 1) {
        sccpcm_push_discon_req(cmcb->sccpcb, SCCPCM_E_DISCON_REQ,
                               cmcb->index);

        return (0);
    }
#endif
    sccprec_push_dev_notify(cmcb->sccpcb, SCCPREC_E_DEV_NOTIFY);

    return (0);
}

/*
 * FUNCTION:    sccpcm_sem_wf_unreg_res_unreg_nak
 *
 * DESCRIPTION: Handle unregister_nak. Go back to the registered state and
 *              let sccprec timeout to fix things.
 *
 * PARAMETERS:
 *     event:   event control block
 *
 * RETURNS:     0 for success, otherwise 1.
 *
 * NOTES:
 *
 * ============================================================================
 * S_WF_UNREG_RES            E_UNREG_NAK                   S_REGISTERED
 * ----------------------------------------------------------------------------
 */
static int sccpcm_sem_wf_unreg_res_unreg_nak (sem_event_t *event)
{
    sccpcm_cmcb_t *cmcb = (sccpcm_cmcb_t *)(event->cb);
    sccpcm_cmcb_t *standbycmcb;

    /*
     * If we receive an UNREGISTER_NAK, we go back to the REGISTERED
     * state and let the sccprec state machine timeout and move to the
     * secondary optimize. Eventually, we detect a better primary
     * available again and try the unregister again.
     */
    /*sam not sure if this is right
     * Check if we were in the process of falling back to another CM. This is
     * the case if we have a standby with a token. We cannot unregister this
     * CM, so we need to go back to the token CM and push it to the connected
     * state. This will put everything back to how it was before we started
     * the fallback procedures. sccprec can then start the whole process over
     * again when it times out.
     */
    standbycmcb = sccpcm_get_secondary_cmcb(cmcb->sccpcb);
    if (standbycmcb != NULL) {
        if (sccpcm_is_cmcb_token(standbycmcb) == 1) {
            sccpcm_push_unreg_nak(standbycmcb->sccpcb, SCCPCM_E_UNREG_NAK,
                                  standbycmcb->index, GAPI_CAUSE_ERROR);
        }
    }

    return (0);
}

/*
 * FUNCTION:    sccpcm_sem_wf_unreg_res_timeout
 *
 * DESCRIPTION: Handle timeout. Check ack retry.
 *
 * PARAMETERS:
 *     event:   event control block
 *
 * RETURNS:     0 for success, otherwise 1.
 *
 * NOTES:
 *
 * ============================================================================
 * S_WF_UNREG_RES            E_TIMEOUT                     S_WF_UNREG_RES
 * ----------------------------------------------------------------------------
 */
static int sccpcm_sem_wf_unreg_res_timeout (sem_event_t *event)
{
    sccpcm_cmcb_t *cmcb = (sccpcm_cmcb_t *)(event->cb);

    /*
     * Increment the retry count, since we did not receive
     * a keepalive_ack.
     */
    cmcb->ack_rsp_retries++;

    SCCP_DBG((sccpcm_debug, 9,
             "%s %-2d:%-2d: %-20s: %s: retry= %d\n",
             SCCPCM_ID, cmcb->sccpcb->id, cmcb->id,
             event->function_name,
             sccpcm_print_cm(cmcb),
             cmcb->ack_rsp_retries));

    /*
     * Check for max retries.
     */
    if (cmcb->ack_rsp_retries >= SCCPREC_UNREG_ACK_RETRIES) {
        sccpcm_push_error(cmcb->sccpcb, SCCPCM_E_ERROR, cmcb->index,
                          SCCPCM_ERROR_NO_UNREG_ACK);
    }
    else {
        /*
         * Resend the unregister.
         */
        sccpreg_push_unreg_req(cmcb->sccpcb, SCCPREG_E_UNREG_REQ);
		sccpcm_start_misc_timer(cmcb);
    }

    return (0);
}

/*
 * FUNCTION:    sccpcm_sem_wf_unreg_res_error
 *
 * DESCRIPTION: Handle error.
 *
 * PARAMETERS:
 *     event:   event control block
 *
 * RETURNS:     0 for success, otherwise 1.
 *
 * NOTES:
 *
 * ============================================================================
 * S_WF_UNREG_RES            E_ERROR                       S_IDLE
 * ----------------------------------------------------------------------------
 */
static int sccpcm_sem_wf_unreg_res_error (sem_event_t *event)
{
    sccpcm_cmcb_t      *cmcb = (sccpcm_cmcb_t *)(event->cb);
    sccpcm_msg_error_t *msg  = (sccpcm_msg_error_t *)(event->data);

    SCCP_DBG((sccpcm_debug, 9,
             "%s %-2d:%-2d: %-20s: %s: %s\n",
             SCCPCM_ID, cmcb->sccpcb->id, cmcb->id,
             event->function_name,
             sccpcm_print_cm(cmcb),
             sccpcm_error_name(msg->error)));

    sccprec_set_fallback(cmcb->sccpcb, cmcb->ccm_type, 0);

    /*
     * Make sure the sccpreg sem is cleaned up.
     */
    sccpreg_push_unregister_ack(cmcb->sccpcb, SCCPREG_E_UNREGISTER_ACK,
                                SCCPMSG_UNREGISTER_STATUS_OK);

    /*
     * Try to disconnect.
     */
    //sccpcm_cm_disconnect_active(cmcb);
    sccpcm_cm_disconnect(cmcb);

    sccpcm_reset_cmcb(cmcb);

    sccprec_push_dev_notify(cmcb->sccpcb, SCCPREC_E_DEV_NOTIFY);

    return (0);
}

/*
 * FUNCTION:    sccpcm_sem_lockout_timeout_ka
 *
 * DESCRIPTION: Handle keepalive timeout. Send keepalive and start timer to 
 *              wait for keepalive_ack.
 *
 * PARAMETERS:
 *     event:   event control block
 *
 * RETURNS:     0 for success, otherwise 1.
 *
 * NOTES:
 *
 * ============================================================================
 * S_LOCKOUT                 E_KA_TO                       S_LOCKOUT
 * ----------------------------------------------------------------------------
 */
static int sccpcm_sem_lockout_timeout_ka (sem_event_t *event)
{
    sccpcm_cmcb_t *cmcb = (sccpcm_cmcb_t *)(event->cb);

    if (sccpmsg_send_bare_message(&sccpcm_msgcb, cmcb->socket,
                                  SCCPMSG_KEEPALIVE) != 0) {
        sccpcm_push_error(cmcb->sccpcb, SCCPCM_E_ERROR,
                          cmcb->index, SCCPCM_ERROR_CANT_SEND_KA);
    }
    else {
        /*
         * Start timer to wait for the keepalive_ack.
         */
        sccpcm_start_keepalive_timer(cmcb);
    }

    return (0);
}

/*
 * FUNCTION:    sccpcm_sem_lockout_timeout_lockout
 *
 * DESCRIPTION: Handle lockout timeout. Start CM disconnect procedures.
 *
 * PARAMETERS:
 *     event:   event control block
 *
 * RETURNS:     0 for success, otherwise 1.
 *
 * NOTES:
 *
 * ============================================================================
 * S_LOCKOUT                 E_LOCKOUT_TO                  S_LOCKOUT
 * ----------------------------------------------------------------------------
 */
static int sccpcm_sem_lockout_timeout_lockout (sem_event_t *event)
{
    sccpcm_cmcb_t *cmcb = (sccpcm_cmcb_t *)(event->cb);

    /*
     * keepalive timer is probably running, so let's stop it.
     */
    sccpcm_stop_timer(cmcb, cmcb->keepalive_timer);

    sccpcm_push_discon_req(cmcb->sccpcb, SCCPCM_E_DISCON_REQ, cmcb->index);

    return (0);
}
#if 0
/*
 * FUNCTION:    sccpcm_sem_lockout_discon_req
 *
 * DESCRIPTION: Begin CM diconnect procedures.
 *
 * PARAMETERS:
 *     event:   event control block
 *
 * RETURNS:     0 for success, otherwise 1.
 *
 * NOTES:
 *
 * ============================================================================
 * S_LOCKOUT                 E_DISCON_REQ                  S_IDLE
 * ----------------------------------------------------------------------------
 */
static int sccpcm_sem_lockout_discon_req (sem_event_t *event)
{
    sccpcm_cmcb_t *cmcb = (sccpcm_cmcb_t *)(event->cb);

    sccpcm_cm_disconnect(cmcb);

    sccpcm_reset_cmcb(cmcb);

    sccprec_push_dev_notify(cmcb->sccpcb, SCCPREC_E_DEV_NOTIFY);

    return (0);
}
#endif
/*
 * FUNCTION:    sccpcm_sem_do_nothing
 *
 * DESCRIPTION: Begin CM disconnect procedures.
 *
 * PARAMETERS:
 *     event:   event control block
 *
 * RETURNS:     0 for success, otherwise 1.
 *
 * NOTES:
 *
 * ============================================================================
 * S_LOCKOUT                 E_UNREG_ACK                   S_LOCKOUT
 * ----------------------------------------------------------------------------
 */
static int sccpcm_sem_do_nothing (sem_event_t *event)
{
    return (0);
}

/*
 * FUNCTION:    sccpcm_sem_lockout_ka_ack
 *
 * DESCRIPTION: keepalive_ack was received while in lockout. Go back to the
 *              to the connected state, requeue the keepalive_ack and everything
 *              should be back to normal.
 *
 * PARAMETERS:
 *     event:   event control block
 *
 * RETURNS:     0 for success, otherwise 1.
 *
 * NOTES:
 *
 * ============================================================================
 * S_LOCKOUT                 E_KEEPALIVE_ACK               S_CONNECTED
 * ----------------------------------------------------------------------------
 */
static int sccpcm_sem_lockout_ka_ack (sem_event_t *event)
{
    sccpcm_cmcb_t *cmcb = (sccpcm_cmcb_t *)(event->cb);

    /*
     * Stop the lockout timer.
     */
    sccpcm_stop_timer(cmcb, cmcb->misc_timer);

    /*
     * We want the keepalive_ack timer to still be running.
     */
    sccpcm_push_keepalive_ack(cmcb->sccpcb, SCCPCM_E_KEEPALIVE_ACK,
                              cmcb->index);

    return (0);
}

/*
 * FUNCTION:    sccpcm_sem_registered
 *
 * DESCRIPTION: Registration is complete - the full registration beyond just
 *              the register_ack. Notify the application.
 *
 * PARAMETERS:
 *     event:   event control block
 *
 * RETURNS:     0 for success, otherwise 1.
 *
 * NOTES:
 *
 * ============================================================================
 * S_REGISTERED              E_REGISTERED                  S_REGISTERED
 * ----------------------------------------------------------------------------
 */
static int sccpcm_sem_registered (sem_event_t *event)
{
    sccpcm_cmcb_t *cmcb = (sccpcm_cmcb_t *)(event->cb);
    gapi_status_data_u data;

    /*
     * Package the session data to send to the application.
     */
    SCCP_MEMCPY(&(data.info), sccpreg_get_session_data(cmcb->sccpcb),
                sizeof(gapi_status_data_info_t));
    data.info.version = cmcb->sccpcb->version;

    SCCP_SESSIONSTATUS(cmcb, GAPI_MSG_STATUS,
                       GAPI_STATUS_CM_REGISTER_COMPLETE,
                       GAPI_STATUS_DATA_INFO,
                       &data);
    
    return (0);
}

/*
 * FUNCTION:    sccpcm_sem_token_retry_to
 *
 * DESCRIPTION: Token retry timer has expired. Try another 
 *              register_token_request.
 *
 * PARAMETERS:
 *     event:   event control block
 *
 * RETURNS:     0 for success, otherwise 1.
 *
 * NOTES:
 *
 * ============================================================================
 * S_CONNECTED               E_TOKEN_RETRY_TO              S_CONNECTED
 * ----------------------------------------------------------------------------
 */
static int sccpcm_sem_token_retry_to (sem_event_t *event)
{
    sccpcm_cmcb_t *cmcb = (sccpcm_cmcb_t *)(event->cb);

    /*
     * Only do things for these states.
     */
    if (sccpcm_is_cmcb_connected2(cmcb) == 1) {
        sccpcm_push_token_req(cmcb->sccpcb, SCCPCM_E_SEND_TOKEN, cmcb->index);
    }
#if 0
    else if ((sccpcm_is_cmcb_registered(cmcb) == 1) {
        sccpcm_push_unreg_req(cmcb->sccpcb, SCCPCM_E_UNREG_REQ, cmcb->index);
    }
#endif
    return (0);
}

/*
 * FUNCTION:    sccpcm_sem_keepalive
 *
 * DESCRIPTION: keepalive has been received. Reply with a keepalive_ack.
 *
 * PARAMETERS:
 *     event:   event control block
 *
 * RETURNS:     0 for success, otherwise 1.
 *
 * NOTES:
 *
 * ============================================================================
 * S_ALL                     E_KEEPALIVE                   NO CHANGE
 * ----------------------------------------------------------------------------
 */
static int sccpcm_sem_keepalive (sem_event_t *event)
{
    sccpcm_cmcb_t *cmcb = (sccpcm_cmcb_t *)(event->cb);

    sccpmsg_send_bare_message(&sccpcm_msgcb, cmcb->socket,
                              SCCPMSG_KEEPALIVE_ACK);

    return (0);
}

int sccpcm_init (void)
{
    sccpcm_msgcb.cb       = NULL;
    sccpcm_msgcb.sem      = SCCP_SEM_CM;
    sccpcm_msgcb.sem_name = SCCPCM_ID;

    return (0);
}

int sccpcm_cleanup (void)
{
    return (0);
}

