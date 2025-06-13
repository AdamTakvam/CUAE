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
 *     sccprec.c
 *
 *  CREATION/MODIFICATION DATES
 *     Created:  November 2002, Sam Hague
 *     Modified: 
 *
 *  DESCRIPTION
 *     SCCP Recovery implementation
 */
#include "sccp.h"
#include "sccp_debug.h"
#include "sem.h"
#include "am.h"
#include "sllist.h"
#include "sccpmsg.h"


#define SCCPREC_DBG_LEVEL_INFO1 (3)
#define SCCPREC_DBG_LEVEL_INFO2 (4)
#define SCCPREC_DBG_LEVEL_ERROR (5)
#define SCCPREC_DBG_LEVEL_SEM   (9)


static char sccprec_buf[32];


typedef enum sccprec_timers_t_ {
    SCCPREC_TIMER_MIN = -1,
    SCCPREC_TIMER_RETRY_NO_CM,
    SCCPREC_TIMER_RETRY_PRI,
    SCCPREC_TIMER_DEVICE_POLL,
    SCCPREC_TIMER_WF_CALL_END,
    SCCPREC_TIMER_WF_REG_ACK_KA,
    SCCPREC_TIMER_WF_CONNECT,
    SCCPREC_TIMER_WF_REGISTER,
    SCCPREC_TIMER_WF_UNREGISTER,
    SCCPREC_TIMER_WF_TOKEN,
    SCCPREC_TIMER_WF_RETRY_TOKEN_REQ,
    SCCPREC_TIMER_RETRY_UNREGISTER_TO,
    SCCPREC_TIMER_WF_CLOSE,
    SCCPREC_TIMER_REJECT,
    SCCPREC_TIMER_MAX
} sccprec_timers_t;

typedef enum sccprec_states_t_ {
    SCCPREC_S_MIN = -1,
    SCCPREC_S_IDLE,
    SCCPREC_S_PRI_CHECK,
    SCCPREC_S_FIND_PRI,
    SCCPREC_S_SEC_CHECK,
    SCCPREC_S_FIND_SEC,
    SCCPREC_S_PRI_OPTIMIZE,
    SCCPREC_S_MAKE_PRI_CON,
    SCCPREC_S_TOKEN_REQ,
    SCCPREC_S_PRI_UNREG_CM,
    SCCPREC_S_PRI_REG_CM,
    SCCPREC_S_SEC_OPTIMIZE,
    SCCPREC_S_MAKE_SEC_CON,
    SCCPREC_S_RESETTING,
    SCCPREC_S_MAX
} sccprec_states_t;

static char *sccprec_state_names[] = {
    "S_IDLE",
    "S_PRI_CHECK",
    "S_FIND_PRI",
    "S_SEC_CHECK",
    "S_FIND_SEC",
    "S_PRI_OPTIMIZE",
    "S_MAKE_PRI_CONN",
    "S_TOKEN_REQ",
    "S_PRI_UNREG_CMGR",
    "S_PRI_REG_CMGR",
    "S_SEC_OPTIMIZE",
    "S_MAKE_SEC_CONN",
    "S_RESETTING"
};

static char *sccprec_event_names[] = {
    "E_PRI_CHECK",
    "E_FIND_PRI",
    "E_TIMEOUT",
    "E_DEV_NOTIFY",
    "E_SEC_CHECK",
    "E_FIND_SEC",
    "E_PRI_OPTIMIZE",
    "E_TOKEN_REQ",
    "E_UNREG_REQ",
    "E_REG_REQ",
    "E_SEC_OPTIMIZE",
    "E_DONE",
    "E_OPEN",
    "E_CLOSE",
    "E_CON_RES",
    "E_REG_RES",
    "E_UNREG_RES",
    "E_RESET",
    "E_RESET_REQ",
};

static char *sccprec_timer_names[] = {
    "T_RETRY_NO_CM",
    "T_RETRY_PRI",
    "T_DEVICE_POLL",
    "T_WF_CALL_END",
    "T_WF_REG_ACK_KA",
    "T_WF_CONNECT",
    "T_WF_REGISTER",
    "T_WF_UNREGISTER",
    "T_WF_TOKEN",
    "T_WF_RETRY_TOKEN_REQ",
    "T_RETRY_UNREGISTER_TO",
    "T_WF_CLOSE",
    "T_REJECT"
};

static int  sccprec_push_timeout(sccp_sccpcb_t *sccpcb, int msg_id,
                                 int event);
static char *sccprec_sem_name(int id);
static char *sccprec_state_name(int id);
static char *sccprec_event_name(int id);
static char *sccprec_function_name(int id);
static void sccprec_debug_sem_entry(sem_event_t *event);
static int  sccprec_validate_cb(void *sccpcb, void *cb);
static void sccprec_change_state(sccprec_reccb_t *reccb, int new_state,
                                 char *fname);

static int sccprec_sem_default(sem_event_t *event);
static int sccprec_sem_done(sem_event_t *event);
static int sccprec_sem_pri_check(sem_event_t *event);
static int sccprec_sem_pri_check_find_pri(sem_event_t *event);
static int sccprec_sem_find_pri_dev_notify(sem_event_t *event);
static int sccprec_sem_find_pri_timeout(sem_event_t *event);
static int sccprec_sem_sec_check(sem_event_t *event);
static int sccprec_sem_sec_check_find_sec(sem_event_t *event);
static int sccprec_sem_find_sec_dev_notify(sem_event_t *event);
static int sccprec_sem_find_sec_timeout(sem_event_t *event);
static int sccprec_sem_pri_optimize(sem_event_t *event);
static int sccprec_sem_pri_opt_pri_optimize(sem_event_t *event);
static int sccprec_sem_make_pri_con_dev_notify(sem_event_t *event);
static int sccprec_sem_make_pri_con_timeout(sem_event_t *event);
static int sccprec_sem_make_pri_con_token_req(sem_event_t *event);
static int sccprec_sem_make_pri_con_unreg_req(sem_event_t *event);
static int sccprec_sem_token_req_dev_notify(sem_event_t *event);
static int sccprec_sem_token_req_timeout(sem_event_t *event);
static int sccprec_sem_token_req_unreg_req(sem_event_t *event);
static int sccprec_sem_pri_unreg_cm_dev_notify(sem_event_t *event);
static int sccprec_sem_pri_unreg_cm_timeout(sem_event_t *event);
static int sccprec_sem_pri_unreg_cm_reg_req(sem_event_t *event);
static int sccprec_sem_pri_reg_cm_dev_notify(sem_event_t *event);
static int sccprec_sem_pri_reg_cm_timeout(sem_event_t *event);
static int sccprec_sem_sec_optimize(sem_event_t *event);
static int sccprec_sem_sec_opt_sec_optimize(sem_event_t *event);
static int sccprec_sem_make_sec_con_dev_notify(sem_event_t *event);
static int sccprec_sem_make_sec_con_timeout(sem_event_t *event);
static int sccprec_sem_open(sem_event_t *event);
static int sccprec_sem_reset(sem_event_t *event);
static int sccprec_sem_reset_req(sem_event_t *event);
static int sccprec_sem_resetting_timeout(sem_event_t *event);
static int sccprec_sem_resetting_dev_notify(sem_event_t *event);
static int sccprec_sem_resetting_done(sem_event_t *event);

typedef enum sccprec_sem_functions_t_  {
    SCCPREC_SEM_MIN = -1,    
    SCCPREC_SEM_DEFAULT,
    SCCPREC_SEM_DONE,
    SCCPREC_SEM_PRI_CHECK,
    SCCPREC_SEM_PRI_CHECK_FIND_PRI,
    SCCPREC_SEM_FIND_PRI_DEV_NOTIFY,
    SCCPREC_SEM_FIND_PRI_TIMEOUT,
    SCCPREC_SEM_SEC_CHECK,
    SCCPREC_SEM_SEC_CHECK_FIND_SEC,
    SCCPREC_SEM_FIND_SEC_DEV_NOTIFY,
    SCCPREC_SEM_FIND_SEC_TIMEOUT,
    SCCPREC_SEM_PRI_OPTIMIZE,
    SCCPREC_SEM_PRI_OPT_PRI_OPTIMIZE,
    SCCPREC_SEM_MAKE_PRI_CON_DEV_NOTIFY,
    SCCPREC_SEM_MAKE_PRI_CON_TIMEOUT,
    SCCPREC_SEM_MAKE_PRI_CON_TOKEN_REQ,
    SCCPREC_SEM_MAKE_PRI_CON_UNREG_REQ,
    SCCPREC_SEM_TOKEN_REQ_DEV_NOTIFY,
    SCCPREC_SEM_TOKEN_REQ_TIMEOUT,
    SCCPREC_SEM_TOKEN_REQ_UNREG_REQ,
    SCCPREC_SEM_PRI_UNREG_CM_DEV_NOTIFY,
    SCCPREC_SEM_PRI_UNREG_CM_TIMEOUT,
    SCCPREC_SEM_PRI_UNREG_CM_REG_REQ,
    SCCPREC_SEM_PRI_REG_CM_DEV_NOTIFY,
    SCCPREC_SEM_PRI_REG_CM_TIMEOUT,
    SCCPREC_SEM_SEC_OPTIMIZE,
    SCCPREC_SEM_SEC_OPT_SEC_OPTIMIZE,
    SCCPREC_SEM_MAKE_SEC_CON_DEV_NOTIFY,
    SCCPREC_SEM_MAKE_SEC_CON_TIMEOUT,
    SCCPREC_SEM_OPEN,
    SCCPREC_SEM_RESET,
    SCCPREC_SEM_RESET_REQ,
    SCCPREC_SEM_RESETTING_TIMEOUT,
    SCCPREC_SEM_RESETTING_DEV_NOTIFY,
    SCCPREC_SEM_RESETTING_DONE,
    SCCPREC_SEM_MAX            
} sccprec_sem_functions_t;

static sem_function_t sccprec_sem_functions[] = 
{
    sccprec_sem_default,
    sccprec_sem_done,
    sccprec_sem_pri_check,
    sccprec_sem_pri_check_find_pri,
    sccprec_sem_find_pri_dev_notify,
    sccprec_sem_find_pri_timeout,
    sccprec_sem_sec_check,
    sccprec_sem_sec_check_find_sec,
    sccprec_sem_find_sec_dev_notify,
    sccprec_sem_find_sec_timeout,
    sccprec_sem_pri_optimize,
    sccprec_sem_pri_opt_pri_optimize,
    sccprec_sem_make_pri_con_dev_notify,
    sccprec_sem_make_pri_con_timeout,
    sccprec_sem_make_pri_con_token_req,
    sccprec_sem_make_pri_con_unreg_req,
    sccprec_sem_token_req_dev_notify,
    sccprec_sem_token_req_timeout,
    sccprec_sem_token_req_unreg_req,
    sccprec_sem_pri_unreg_cm_dev_notify,
    sccprec_sem_pri_unreg_cm_timeout,
    sccprec_sem_pri_unreg_cm_reg_req,
    sccprec_sem_pri_reg_cm_dev_notify,
    sccprec_sem_pri_reg_cm_timeout,
    sccprec_sem_sec_optimize,
    sccprec_sem_sec_opt_sec_optimize,
    sccprec_sem_make_sec_con_dev_notify,
    sccprec_sem_make_sec_con_timeout,
    sccprec_sem_open,
    sccprec_sem_reset,
    sccprec_sem_reset_req,
    sccprec_sem_resetting_timeout,
    sccprec_sem_resetting_dev_notify,
    sccprec_sem_resetting_done
};

static char *sccprec_sem_function_names[] = 
{
    "sem_default",
    "sem_done",
    "sem_pri_check",
    "sem_pri_check_find_pri",
    "sem_find_pri_dev_notify",
    "sem_find_pri_timeout",
    "sem_sec_check",
    "sem_sec_check_find_sec",
    "sem_find_sec_dev_notify",
    "sem_find_sec_timeout",
    "sem_pri_optimize",
    "sem_pri_opt_pri_optimize",
    "sem_make_pri_con_dev_notify",
    "sem_make_pri_con_timeout",
    "sem_make_pri_con_token_req",
    "sem_make_pri_con_unreg_req",
    "sem_token_req_dev_notify",
    "sem_token_req_timeout",
    "sem_token_req_unreg_req",
    "sem_pri_unreg_cm_dev_notify",
    "sem_pri_unreg_cm_timeout",
    "sem_pri_unreg_cm_reg_req",
    "sem_pri_reg_cm_dev_notify",
    "sem_pri_reg_cm_timeout",
    "sem_sec_optimize",
    "sem_sec_opt_sec_optimize",
    "sem_make_sec_con_dev_notify",
    "sem_make_sec_con_timeout",
    "sem_open",
    "sem_reset",
    "sem_reset_req",
    "sem_resetting_timeout",
    "sccprec_sem_resetting_dev_notify",
    "sccprec_sem_resetting_done"
};

static sem_events_t sccprec_sem_s_idle[] = 
{
    {SCCPREC_E_OPEN,         SCCPREC_SEM_OPEN,                     SCCPREC_S_IDLE},
    {SCCPREC_E_PRI_CHECK,    SCCPREC_SEM_PRI_CHECK,                SCCPREC_S_PRI_CHECK},
    {SCCPREC_E_TIMEOUT,      SCCPREC_SEM_PRI_CHECK,                SCCPREC_S_PRI_CHECK},
    {SCCPREC_E_DEV_NOTIFY,   SCCPREC_SEM_PRI_CHECK,                SCCPREC_S_PRI_CHECK},
    {SCCPREC_E_RESET,        SCCPREC_SEM_RESET,                    SCCPREC_S_IDLE},
    {SCCPREC_E_RESET_REQ,    SCCPREC_SEM_RESET_REQ,                SCCPREC_S_RESETTING},
    {SCCPREC_E_DONE,         SCCPREC_SEM_DONE,                     SCCPREC_S_IDLE}
};

static sem_events_t sccprec_sem_s_pri_check[] =
{
    {SCCPREC_E_FIND_PRI,     SCCPREC_SEM_PRI_CHECK_FIND_PRI,       SCCPREC_S_FIND_PRI},
    {SCCPREC_E_TIMEOUT,      SCCPREC_SEM_PRI_CHECK,                SCCPREC_S_PRI_CHECK},  
    {SCCPREC_E_SEC_CHECK,    SCCPREC_SEM_SEC_CHECK,                SCCPREC_S_SEC_CHECK},
    {SCCPREC_E_RESET,        SCCPREC_SEM_RESET,                    SCCPREC_S_PRI_CHECK},
    {SCCPREC_E_RESET_REQ,    SCCPREC_SEM_RESET_REQ,                SCCPREC_S_RESETTING},
    {SCCPREC_E_DONE,         SCCPREC_SEM_DONE,                     SCCPREC_S_IDLE}
};

static sem_events_t sccprec_sem_s_find_pri[] =
{
    {SCCPREC_E_TIMEOUT,      SCCPREC_SEM_FIND_PRI_TIMEOUT,         SCCPREC_S_FIND_PRI},  
    {SCCPREC_E_DEV_NOTIFY,   SCCPREC_SEM_FIND_PRI_DEV_NOTIFY,      SCCPREC_S_FIND_PRI},
    {SCCPREC_E_SEC_CHECK,    SCCPREC_SEM_SEC_CHECK,                SCCPREC_S_SEC_CHECK},
    {SCCPREC_E_RESET,        SCCPREC_SEM_RESET,                    SCCPREC_S_FIND_PRI},
    {SCCPREC_E_RESET_REQ,    SCCPREC_SEM_RESET_REQ,                SCCPREC_S_RESETTING},
    {SCCPREC_E_DONE,         SCCPREC_SEM_DONE,                     SCCPREC_S_IDLE}
};

static sem_events_t sccprec_sem_s_sec_check[] =
{
    {SCCPREC_E_FIND_SEC,     SCCPREC_SEM_SEC_CHECK_FIND_SEC,       SCCPREC_S_FIND_SEC},
    {SCCPREC_E_PRI_OPTIMIZE, SCCPREC_SEM_PRI_OPTIMIZE,             SCCPREC_S_PRI_OPTIMIZE},
    {SCCPREC_E_RESET,        SCCPREC_SEM_RESET,                    SCCPREC_S_SEC_CHECK},
    {SCCPREC_E_RESET_REQ,    SCCPREC_SEM_RESET_REQ,                SCCPREC_S_RESETTING},
    {SCCPREC_E_DONE,         SCCPREC_SEM_DONE,                     SCCPREC_S_IDLE}
};

static sem_events_t sccprec_sem_s_find_sec[] =
{
    {SCCPREC_E_TIMEOUT,      SCCPREC_SEM_FIND_SEC_TIMEOUT,         SCCPREC_S_FIND_SEC},  
    {SCCPREC_E_DEV_NOTIFY,   SCCPREC_SEM_FIND_SEC_DEV_NOTIFY,      SCCPREC_S_FIND_SEC},
    {SCCPREC_E_PRI_OPTIMIZE, SCCPREC_SEM_PRI_OPTIMIZE,             SCCPREC_S_PRI_OPTIMIZE},
    {SCCPREC_E_RESET,        SCCPREC_SEM_RESET,                    SCCPREC_S_FIND_SEC},
    {SCCPREC_E_RESET_REQ,    SCCPREC_SEM_RESET_REQ,                SCCPREC_S_RESETTING},
    {SCCPREC_E_DONE,         SCCPREC_SEM_DONE,                     SCCPREC_S_IDLE}
};

static sem_events_t sccprec_sem_s_pri_optimize[] =
{
    {SCCPREC_E_PRI_OPTIMIZE, SCCPREC_SEM_PRI_OPT_PRI_OPTIMIZE,     SCCPREC_S_MAKE_PRI_CON},    
    {SCCPREC_E_SEC_OPTIMIZE, SCCPREC_SEM_SEC_OPTIMIZE,             SCCPREC_S_SEC_OPTIMIZE},
    {SCCPREC_E_RESET,        SCCPREC_SEM_RESET,                    SCCPREC_S_PRI_OPTIMIZE},
    {SCCPREC_E_RESET_REQ,    SCCPREC_SEM_RESET_REQ,                SCCPREC_S_RESETTING},
    {SCCPREC_E_DONE,         SCCPREC_SEM_DONE,                     SCCPREC_S_IDLE}
};

static sem_events_t sccprec_sem_s_make_pri_connection[] =
{
    {SCCPREC_E_TIMEOUT,      SCCPREC_SEM_MAKE_PRI_CON_TIMEOUT,     SCCPREC_S_MAKE_PRI_CON},
    {SCCPREC_E_DEV_NOTIFY,   SCCPREC_SEM_MAKE_PRI_CON_DEV_NOTIFY,  SCCPREC_S_MAKE_PRI_CON},
    {SCCPREC_E_PRI_OPTIMIZE, SCCPREC_SEM_PRI_OPT_PRI_OPTIMIZE,     SCCPREC_S_MAKE_PRI_CON},    
    {SCCPREC_E_TOKEN_REQ,    SCCPREC_SEM_MAKE_PRI_CON_TOKEN_REQ,   SCCPREC_S_TOKEN_REQ},
    {SCCPREC_E_UNREG_REQ,    SCCPREC_SEM_MAKE_PRI_CON_UNREG_REQ,   SCCPREC_S_PRI_UNREG_CM},
    {SCCPREC_E_SEC_OPTIMIZE, SCCPREC_SEM_SEC_OPTIMIZE,             SCCPREC_S_SEC_OPTIMIZE},
    {SCCPREC_E_RESET,        SCCPREC_SEM_RESET,                    SCCPREC_S_MAKE_PRI_CON},
    {SCCPREC_E_RESET_REQ,    SCCPREC_SEM_RESET_REQ,                SCCPREC_S_RESETTING},
    {SCCPREC_E_DONE,         SCCPREC_SEM_DONE,                     SCCPREC_S_IDLE}
};

static sem_events_t sccprec_sem_s_token_req[] =
{
    {SCCPREC_E_TIMEOUT,      SCCPREC_SEM_TOKEN_REQ_TIMEOUT,        SCCPREC_S_TOKEN_REQ},
    {SCCPREC_E_DEV_NOTIFY,   SCCPREC_SEM_TOKEN_REQ_DEV_NOTIFY,     SCCPREC_S_TOKEN_REQ},
    {SCCPREC_E_UNREG_REQ,    SCCPREC_SEM_TOKEN_REQ_UNREG_REQ,      SCCPREC_S_PRI_UNREG_CM},
    {SCCPREC_E_SEC_OPTIMIZE, SCCPREC_SEM_SEC_OPTIMIZE,             SCCPREC_S_SEC_OPTIMIZE},
    {SCCPREC_E_RESET,        SCCPREC_SEM_RESET,                    SCCPREC_S_TOKEN_REQ},
    {SCCPREC_E_RESET_REQ,    SCCPREC_SEM_RESET_REQ,                SCCPREC_S_RESETTING},
    {SCCPREC_E_DONE,         SCCPREC_SEM_DONE,                     SCCPREC_S_IDLE}
};

static sem_events_t sccprec_sem_s_pri_unreg_cm[] =
{
    {SCCPREC_E_TIMEOUT,      SCCPREC_SEM_PRI_UNREG_CM_TIMEOUT,     SCCPREC_S_PRI_UNREG_CM},
    {SCCPREC_E_DEV_NOTIFY,   SCCPREC_SEM_PRI_UNREG_CM_DEV_NOTIFY,  SCCPREC_S_PRI_UNREG_CM},
    {SCCPREC_E_REG_REQ,      SCCPREC_SEM_PRI_UNREG_CM_REG_REQ,     SCCPREC_S_PRI_REG_CM},
    {SCCPREC_E_SEC_OPTIMIZE, SCCPREC_SEM_SEC_OPTIMIZE,             SCCPREC_S_SEC_OPTIMIZE},
    {SCCPREC_E_RESET,        SCCPREC_SEM_RESET,                    SCCPREC_S_PRI_UNREG_CM},
    {SCCPREC_E_RESET_REQ,    SCCPREC_SEM_RESET_REQ,                SCCPREC_S_RESETTING},
    {SCCPREC_E_DONE,         SCCPREC_SEM_DONE,                     SCCPREC_S_IDLE}
};

static sem_events_t sccprec_sem_s_pri_reg_cm[] =
{
    {SCCPREC_E_TIMEOUT,      SCCPREC_SEM_PRI_REG_CM_TIMEOUT,       SCCPREC_S_PRI_REG_CM},
    {SCCPREC_E_DEV_NOTIFY,   SCCPREC_SEM_PRI_REG_CM_DEV_NOTIFY,    SCCPREC_S_PRI_REG_CM},
    {SCCPREC_E_PRI_CHECK,    SCCPREC_SEM_PRI_CHECK,                SCCPREC_S_PRI_CHECK},
    {SCCPREC_E_SEC_CHECK,    SCCPREC_SEM_SEC_CHECK,                SCCPREC_S_SEC_CHECK},
    {SCCPREC_E_SEC_OPTIMIZE, SCCPREC_SEM_SEC_OPTIMIZE,             SCCPREC_S_SEC_OPTIMIZE},
    {SCCPREC_E_RESET,        SCCPREC_SEM_RESET,                    SCCPREC_S_PRI_REG_CM},
    {SCCPREC_E_RESET_REQ,    SCCPREC_SEM_RESET_REQ,                SCCPREC_S_RESETTING},
    {SCCPREC_E_DONE,         SCCPREC_SEM_DONE,                     SCCPREC_S_IDLE}
};

static sem_events_t sccprec_sem_s_sec_optimize[] =
{
    {SCCPREC_E_SEC_OPTIMIZE, SCCPREC_SEM_SEC_OPT_SEC_OPTIMIZE,     SCCPREC_S_MAKE_SEC_CON},
    {SCCPREC_E_RESET,        SCCPREC_SEM_RESET,                    SCCPREC_S_SEC_OPTIMIZE},
    {SCCPREC_E_RESET_REQ,    SCCPREC_SEM_RESET_REQ,                SCCPREC_S_RESETTING},
    {SCCPREC_E_DONE,         SCCPREC_SEM_DONE,                     SCCPREC_S_IDLE}
};

static sem_events_t sccprec_sem_s_make_sec_connection[] =
{
    {SCCPREC_E_TIMEOUT,      SCCPREC_SEM_MAKE_SEC_CON_TIMEOUT,     SCCPREC_S_MAKE_SEC_CON},
    {SCCPREC_E_DEV_NOTIFY,   SCCPREC_SEM_MAKE_SEC_CON_DEV_NOTIFY,  SCCPREC_S_MAKE_SEC_CON},
    {SCCPREC_E_SEC_OPTIMIZE, SCCPREC_SEM_SEC_OPT_SEC_OPTIMIZE,     SCCPREC_S_MAKE_SEC_CON},
    {SCCPREC_E_RESET,        SCCPREC_SEM_RESET,                    SCCPREC_S_MAKE_SEC_CON},
    {SCCPREC_E_RESET_REQ,    SCCPREC_SEM_RESET_REQ,                SCCPREC_S_RESETTING},
    {SCCPREC_E_DONE,         SCCPREC_SEM_DONE,                     SCCPREC_S_IDLE}
};

static sem_events_t sccprec_sem_s_resetting[] =
{
    {SCCPREC_E_TIMEOUT,      SCCPREC_SEM_RESETTING_TIMEOUT,        SCCPREC_S_RESETTING},
    {SCCPREC_E_DEV_NOTIFY,   SCCPREC_SEM_RESETTING_DEV_NOTIFY,     SCCPREC_S_RESETTING},
    {SCCPREC_E_DONE,         SCCPREC_SEM_RESETTING_DONE,           SCCPREC_S_IDLE}
};

static sem_states_t sccprec_sem_states[] =
{
    {sccprec_sem_s_idle,                sizeof(sccprec_sem_s_idle)                / SEM_EVENTS_SIZE},
    {sccprec_sem_s_pri_check,           sizeof(sccprec_sem_s_pri_check)           / SEM_EVENTS_SIZE},
    {sccprec_sem_s_find_pri,            sizeof(sccprec_sem_s_find_pri)            / SEM_EVENTS_SIZE},
    {sccprec_sem_s_sec_check,           sizeof(sccprec_sem_s_sec_check)           / SEM_EVENTS_SIZE},
    {sccprec_sem_s_find_sec,            sizeof(sccprec_sem_s_find_sec)            / SEM_EVENTS_SIZE},
    {sccprec_sem_s_pri_optimize,        sizeof(sccprec_sem_s_pri_optimize)        / SEM_EVENTS_SIZE},
    {sccprec_sem_s_make_pri_connection, sizeof(sccprec_sem_s_make_pri_connection) / SEM_EVENTS_SIZE},
    {sccprec_sem_s_token_req,           sizeof(sccprec_sem_s_token_req)           / SEM_EVENTS_SIZE},
    {sccprec_sem_s_pri_unreg_cm,        sizeof(sccprec_sem_s_pri_unreg_cm)        / SEM_EVENTS_SIZE},
    {sccprec_sem_s_pri_reg_cm,          sizeof(sccprec_sem_s_pri_reg_cm)          / SEM_EVENTS_SIZE},
    {sccprec_sem_s_sec_optimize,        sizeof(sccprec_sem_s_sec_optimize)        / SEM_EVENTS_SIZE},
    {sccprec_sem_s_make_sec_connection, sizeof(sccprec_sem_s_make_sec_connection) / SEM_EVENTS_SIZE}, 
    {sccprec_sem_s_resetting,           sizeof(sccprec_sem_s_resetting)           / SEM_EVENTS_SIZE}
};

static sem_tbl_t sccprec_sem_tbl = {
    sccprec_sem_states, SCCPREC_S_MAX, SCCPREC_E_MAX
};

sem_table_t sccprec_sem_table = 
{
    &sccprec_sem_tbl,        sccprec_sem_functions,
    sccprec_state_name,      sccprec_event_name,
    sccprec_sem_name,        sccprec_function_name,
    sccprec_debug_sem_entry, sccprec_validate_cb,
    (sem_change_state_f *)sccprec_change_state
};


static char *sccprec_sem_name (int id)
{
    return (SCCPREC_ID);
}

static char *sccprec_state_name (int id)
{
    if ((id <= SCCPREC_S_MIN) || (id >= SCCPREC_S_MAX)) {
        return (SCCP_UNDEFINED);
    }

    return (sccprec_state_names[id]);
}

static char *sccprec_event_name (int id)
{
    if ((id <= SCCPREC_E_MIN) || (id >= SCCPREC_E_MAX)) {
        return (SCCP_UNDEFINED);
    }

    return (sccprec_event_names[id]);
}

static char *sccprec_function_name (int id)
{
    if ((id <= SCCPREC_SEM_MIN) || (id >= SCCPREC_SEM_MAX)) {
        return (SCCP_UNDEFINED);
    }

    return (sccprec_sem_function_names[id]);
}

static char *sccprec_timer_name (int id)
{
    if ((id <= SCCPREC_TIMER_MIN) || (id >= SCCPREC_TIMER_MAX)) {
        return (SCCP_UNDEFINED);
    }

    return (sccprec_timer_names[id]);
}

static void sccprec_debug_sem_entry (sem_event_t *event)
{
    sccprec_reccb_t *reccb = (sccprec_reccb_t *)(event->cb);
    
    SCCP_DBG((sccprec_debug, SCCPREC_DBG_LEVEL_SEM,
             "%s %-2d:%-2d: %-20s: %s <- %s\n",
             SCCPREC_ID, reccb->sccpcb->id, reccb->id,
             event->function_name,
             sccprec_state_name(event->state_id),
             sccprec_event_name(event->event_id)));
}

static int sccprec_validate_cb (void *scb, void *cb)
{
    return (((sccp_sccpcb_t *)scb)->reccb == cb ? 0 : 1);
}

static void sccprec_change_state (sccprec_reccb_t *reccb, int new_state,
                                  char *fname)
{
    SCCP_DBG((sccprec_debug, SCCPREC_DBG_LEVEL_SEM, 
             "%s %-2d:%-2d: %-20s: %s -> %s\n",
             SCCPREC_ID, reccb->sccpcb->id, reccb->id, fname,
             sccprec_state_name(reccb->state),
             sccprec_state_name(new_state)));

    reccb->old_state = reccb->state;
    reccb->state = new_state;
}

static void sccprec_timeout_callback (void *timer_event, void *param1,
                                      void *param2)
{
    SCCP_DBG((sccprec_debug, SCCPREC_DBG_LEVEL_SEM,
             "%s %-2d:%-2d: %-20s: %s: 0x%08x\n",
             SCCPREC_ID,
             ((sccpcm_cmcb_t *)(param1))->sccpcb->id,
             ((sccpcm_cmcb_t *)(param1))->id, "timeout_callback",
             sccprec_timer_name((int)param2), timer_event));

    sccprec_push_timeout(((sccprec_reccb_t *)(param1))->sccpcb,
                         SCCPREC_E_TIMEOUT, (int)param2);
}

static void sccprec_start_timer (sccprec_reccb_t *cb,
                                 void *timer, int type,
                                 unsigned long period)
{
    char *fname = "start_timer";

    SCCP_TIMER_CANCEL(timer);

    SCCP_TIMER_INITIALIZE(timer,
                          period,
                          sccprec_timeout_callback,
                          (void *)cb,
                          (void *)type);

    SCCP_TIMER_ACTIVATE(timer);

    SCCP_DBG((sccprec_debug, SCCPREC_DBG_LEVEL_INFO2,
             "%s %-2d:%-2d: %-20s: %s: 0x%08x: %dms\n",
             SCCPREC_ID, cb->sccpcb->id, cb->id, fname,
             sccprec_timer_name(type), timer, period));
}

static void sccprec_stop_timer (sccprec_reccb_t *cb, void *timer)
{
    char *fname = "stop_timer";

    SCCP_TIMER_CANCEL(timer);

    SCCP_DBG((sccprec_debug, SCCPREC_DBG_LEVEL_INFO2,
             "%s %-2d:%-2d: %-20s: 0x%08x\n",
             SCCPREC_ID, cb->sccpcb->id, cb->id, fname, timer));
}

static void sccprec_stop_timers (sccprec_reccb_t *reccb)
{
    sccprec_stop_timer(reccb, reccb->misc_timer);
}

static void sccprec_free_timers (sccprec_reccb_t *reccb)
{
    SCCP_TIMER_FREE(reccb->misc_timer);
}

void sccprec_set_fallback (sccp_sccpcb_t *sccpcb, 
                           sccpcm_ccm_types_t ccm_type,
                           int registered)
{
    if (registered == 1) {
        if (ccm_type == SCCPCM_CCM_TYPE_SRST_FALLBACK) {
            sccpcb->reccb->registered_with_fallback = 1;
        }
        else {
            sccpcb->reccb->registered_with_fallback = 0;
        }
    }
    else {
        sccpcb->reccb->registered_with_fallback = 0;
    }
}

void sccprec_set_tcp_close_cause (sccp_sccpcb_t *sccpcb, int cause)
{
    /*
     * Only change the value if it still has it's default or if
     * the user is trying to set it to its default.
     */
    if ((sccpcb->reccb->tcp_close_cause == SCCPMSG_ALARM_LAST_INITIALIZED) || 
        (cause == SCCPMSG_ALARM_LAST_INITIALIZED)) {
        sccpcb->reccb->tcp_close_cause = cause;
    }
}

void sccprec_set_keepalive_timeout (sccp_sccpcb_t *sccpcb,
                                    unsigned long timeout_ms,
                                    int whichone)
{
    unsigned long *ka;
    unsigned long old_timeout;

    if (whichone == 1) {
        ka = &(sccpcb->reccb->keepalive_t1);
    }
    else {
        ka = &(sccpcb->reccb->keepalive_t2);
    }

    old_timeout = *ka;
#if 0
	/*
	 * Make sure that the timeout value is not less than the minimum.
	 */
    if (timeout_ms < (unsigned long)(sccpcb->reccb->default_keepalive_to)) {
        timeout_ms = (unsigned long)(sccpcb->reccb->default_keepalive_to);
    }
#endif
    if (timeout_ms == 0) {
        timeout_ms = (unsigned long)(sccpcb->reccb->default_keepalive_to);
    }

    *ka = timeout_ms;

    SCCP_DBG((sccprec_debug, SCCPREC_DBG_LEVEL_INFO2,
             "%s %-2d:%-2d: Changing keepalive timeout "
             "%d from %dms to: %dms\n", 
             SCCPREC_ID, sccpcb->id, sccpcb->reccb->id,
             whichone, old_timeout, *ka));
}

int sccprec_get_keepalive_to (sccp_sccpcb_t *sccpcb, int whichone)
{
    int timeout;

    switch (whichone) {
    case (0):
        timeout = sccpcb->reccb->default_keepalive_to;
        break;

    case (1):
        timeout = sccpcb->reccb->keepalive_t1;
        break;

    case (2):
        timeout = sccpcb->reccb->keepalive_t2;
        break;

    default:
        timeout = 0;
        break;
    }

    return (timeout);
}

void sccprec_set_free_events_flag (sccp_sccpcb_t *sccpcb, int flag)
{
    if ((sccpcb != NULL) && (sccpcb->reccb != NULL)) {
        sccpcb->reccb->free_events = flag;
    }
}


void sccprec_set_register_reject (sccp_sccpcb_t *sccpcb, int flag)
{
    if ((sccpcb != NULL) && (sccpcb->reccb != NULL)) {
        sccpcb->reccb->reject = flag;
    }
}

static int sccprec_get_new_reccb_id (void)
{
    static int id = 0;

    if (++id < 0) {
        id = 1;
    }

    return (id);
}

void sccprec_init_reccb (sccprec_reccb_t *reccb, sccprec_reset_type_t reset)
{
    int i;

    sccprec_stop_timers(reccb);

    /*
     * Resets require the application to start another session with ne data,
     * so clear out all the existing data.
     * 
     * Restarts will just get kicked off again with the same session
     * data so we don't reset it.
     */
    if (reset == SCCPREC_RESET_TYPE_RESET) {
        reccb->session_state = SCCPREC_SESSION_STATE_IDLE;

        for (i = 0; i < reccb->max_cm_index; i++) {
            reccb->cmcbs[i] = NULL;
        }

		reccb->max_cm_index = 0;

        reccb->call_end_to          = SCCPREC_WF_CALL_END_TO;
        reccb->close_to             = SCCPREC_WF_CLOSE_TO;
        reccb->connect_to           = SCCPREC_WF_CONNECT_TO;
        reccb->default_keepalive_to = SCCPREC_DEFAULT_CONNECT_KA_TO;
        reccb->device_poll_to       = SCCPREC_DEVICE_POLL_TO;
        reccb->nak_to_syn_retry_to  = SCCPREC_NAK_TO_SYN_RETRY_TO;
        reccb->device_type          = SCCPREC_DEVICE_TYPE;
    }
    
#if 0
    /*
     * Make sure the list is empty. This ensures that the list does not
     * contain events not processed from a previous session.
     */
    sllist_empty_list(reccb->sccpcb->events);
#endif
    reccb->pri_retries              = 0;
    reccb->cm_index                 = -1;
    reccb->start_index              = 0;
    reccb->cm_list_iterations       = 0;
    reccb->connect_keepalive_to     = 0;
    reccb->waiting_to_find_pri      = 0;
    reccb->keepalive_t1             = reccb->default_keepalive_to;
    reccb->keepalive_t2             = reccb->default_keepalive_to;
    reccb->registered_with_fallback = 0;
    //reccb->tcp_close_cause          = SCCPMSG_ALARM_LAST_MIN;
    reccb->reset_cause              = GAPI_CAUSE_OK;
    reccb->reject                   = 0;
    reccb->free_events              = 0;
}

void sccprec_free_reccb (sccprec_reccb_t *reccb)
{
    if (reccb != NULL) {
        sccprec_stop_timers(reccb);
        sccprec_free_timers(reccb);

        SCCP_FREE(reccb);
    }
}

void *sccprec_get_new_reccb (sccp_sccpcb_t *sccpcb)

{
    char            *fname = "get_new_reccb";
    sccprec_reccb_t *reccb;

    reccb = (sccprec_reccb_t *)(SCCP_MALLOC(sizeof(*reccb)));
    if (reccb == NULL) {
        return (NULL);
    }

    SCCP_MEMSET(reccb, 0, sizeof(*reccb));

    reccb->misc_timer = SCCP_TIMER_ALLOCATE();
    if (reccb->misc_timer == NULL) {
        SCCP_FREE(reccb);
        return (NULL);
    }

    reccb->sccpcb = sccpcb;
    //reccb->id     = sccprec_get_new_reccb_id();
    reccb->id     = sccpcb->id;

    sccprec_init_reccb(reccb, SCCPREC_RESET_TYPE_RESET);

    /*
     * We set this value outside of the init_reccb function because
     * the init function is called in other places and we do not
     * want to overwrite the value. But here, the reccb was just
     * created so we want to initialize the value.
     */
    reccb->tcp_close_cause = SCCPMSG_ALARM_LAST_INITIALIZED;

    SCCP_DBG((sccprec_debug, SCCPREC_DBG_LEVEL_INFO1,
             "%s %-2d:%-2d: %-20s: reccb= 0x%08x\n",
             SCCPREC_ID, reccb->sccpcb->id, reccb->id, fname, reccb));
                      
    return (reccb);
}

static int sccprec_push_simple (sccp_sccpcb_t *sccpcb, int msg_id)
{
    sccp_push_event(sccpcb, NULL, 0, msg_id, 
                    &sccprec_sem_table, sccpcb->reccb, 0);

    return (0);
}

int sccprec_push_dev_notify (sccp_sccpcb_t *sccpcb, int msg_id)
{
    return (sccprec_push_simple(sccpcb, msg_id));
}

int sccprec_push_opensession_req (sccp_sccpcb_t *sccpcb, int msg_id,
                                  gapi_cmaddr_t *cms, char *mac,
                                  gapi_cmaddr_t *srsts,
                                  gapi_srst_modes_e srst_mode,
                                  gapi_cmaddr_t *tftp,
                                  gapi_media_caps_t *media_caps,
                                  gapi_opensession_values_t *values,
                                  gapi_protocol_versions_e version)
{
    sccprec_msg_open_t *msg;
    int i;

    msg = (sccprec_msg_open_t *)SCCP_MALLOC(sizeof(*msg));
    if (msg == NULL) {
        return (2);
    }

    SCCP_MEMSET(msg, 0, sizeof(*msg));

    msg->msg_id = msg_id;

    if (cms != NULL) {
        for (i = 0; i < SCCPREC_MAX_CM_SERVERS; i++) {
            msg->cms[i].addr = cms[i].addr;
            msg->cms[i].port = cms[i].port;
        }
    }
    
    for (i = 0; i < GAPI_MAX_MAC_SIZE; i++) {
        msg->mac[i] = mac[i];
        msg->mac[GAPI_MAX_MAC_SIZE - 1] = '\0';
    }

    if (srsts != NULL) {
        for (i = 0; i < SCCPREC_MAX_SRST_SERVERS; i++) {
            msg->srsts[i].addr = srsts[i].addr;
            msg->srsts[i].port = srsts[i].port;
        }
    }

    msg->srst_mode = srst_mode;

    if (tftp != NULL) {
        msg->tftp.addr = tftp->addr;
        msg->tftp.port = tftp->port;
    }

    if (media_caps != NULL) {
        for (i = 0; i < media_caps->count; i++) {
            msg->media_caps.caps[i].payload = media_caps->caps[i].payload;
            msg->media_caps.caps[i].milliseconds_per_packet = media_caps->caps[i].milliseconds_per_packet;
        }
        msg->media_caps.count = media_caps->count;
    }

    if (values != NULL) {
        msg->values.call_end_to          = values->call_end_to;
        msg->values.close_to             = values->close_to;
        msg->values.connect_to           = values->connect_to;
        msg->values.default_keepalive_to = values->default_keepalive_to;
        msg->values.device_poll_to       = values->device_poll_to;
        msg->values.nak_to_syn_retry_to  = values->nak_to_syn_retry_to;
        msg->values.device_type          = values->device_type;
        msg->values.cc_mode              = values->cc_mode;
    }

    msg->version = version;

    sccp_push_event(sccpcb, msg, 0, SCCPREC_E_OPEN,
                    &sccprec_sem_table, sccpcb->reccb, 0);

    return (0);
}

int sccprec_push_resetsession_req (sccp_sccpcb_t *sccpcb, int msg_id,
                                   gapi_causes_e cause)
{
    sccprec_msg_reset_req_t *msg;

    msg = (sccprec_msg_reset_req_t *)(SCCP_MALLOC(sizeof(*msg)));
    if (msg == NULL) {
        return (2);
    }

    SCCP_MEMSET(msg, 0, sizeof(*msg));

    msg->msg_id = SCCPREC_E_RESET_REQ;
    msg->cause  = cause;

    sccp_push_event(sccpcb, msg, 0, SCCPREC_E_RESET_REQ,
                    &sccprec_sem_table, sccpcb->reccb, 0);

    return (0);
}

static int sccprec_push_timeout (sccp_sccpcb_t *sccpcb, int msg_id,
                                 int event)
{
    sccprec_msg_timeout_t *msg;

    msg = (sccprec_msg_timeout_t *)(SCCP_MALLOC(sizeof(*msg)));
    if (msg == NULL) {
        return (2);
    }

    SCCP_MEMSET(msg, 0, sizeof(*msg));

    msg->msg_id = msg_id;
    msg->event  = event;

    sccp_push_event(sccpcb, msg, 0, msg_id,
                    &sccprec_sem_table, sccpcb->reccb, 0);

    return (0);
}

int sccprec_push_reset (sccp_sccpcb_t *sccpcb, int msg_id,
                        sccpmsg_reset_type_t type)
{
    sccpmsg_general_t *gen_msg;
    sccpmsg_reset_t   *msg;

    //gen_msg = (sccpmsg_general_t *)SCCP_MALLOC(sizeof(*gen_msg));
    gen_msg = (sccpmsg_general_t *)SCCP_MALLOC(sizeof(*msg) +
                                               sizeof(sccpmsg_base_t));

    if (gen_msg == NULL) {
        return (2);
    }

    SCCP_MEMSET(gen_msg, 0, sizeof(*msg) + sizeof(sccpmsg_base_t));

    msg = &(gen_msg->body.reset);

    msg->message_id = SCCPMSG_RESET;
    msg->reset_type = type;

    return (sccp_push_event(sccpcb, gen_msg, 0, SCCPREC_E_RESET,
                            &sccprec_sem_table, sccpcb->regcb, 0));
}

/*
 * FUNCTION:    sccprec_cleanup_secondarys
 *
 * DESCRIPTION: Go through the CM list and close all but
 *              the best secondary connection to a CM. We only want need to 
 *              maintain one primary and one secondary.
 *
 * PARAMETERS:
 *     reccb:   Recovery control block
 *     fname:   Function name
 *
 * RETURNS:     Number of connections open with CMs after cleanup.
 */                    
static int sccprec_cleanup_secondarys (sccprec_reccb_t *reccb, char *fname)
{
    int i;
    int count = 0;
    int cmlist[SCCPREC_MAX_CM_SERVERS];

    SCCP_MEMSET(cmlist, -1, sizeof(cmlist));

    /*
     * Count total number of secondarys.
     */
    for (i = 0; i < reccb->max_cm_index; i++) {
        if (reccb->cmcbs[i] != NULL) {
            if (sccpcm_is_cmcb_connected(reccb->cmcbs[i]) == 1) {
                cmlist[i] = i;
                count++;
            }
        }
    }

    /*
     * Cleanup extra secondarys starting at the end of the list.
     * If we only have one good secondary, then this loop won't
     * run. The best secondary will be near the start of the list.
     */
    for (i = reccb->max_cm_index - 1; (i >= 0) && (count > 1); i--) {
        if (cmlist[i] != -1) {
            SCCP_DBG((sccprec_debug, SCCPREC_DBG_LEVEL_INFO2, 
                     "%s %-2d:%-2d: %-20s: %s: disconnecting secondary\n",
                     SCCPREC_ID, reccb->sccpcb->id, reccb->id, fname,
                     sccpcm_print_cm(reccb->cmcbs[i])));

            sccpcm_push_discon_req(reccb->sccpcb, SCCPCM_E_DISCON_REQ, i);

            count--;
        }
    }

    return (count);
}

/*
 * FUNCTION:    sccprec_count_list_iterations
 *
 * DESCRIPTION: Supervise the CM list iterations and request a reset
 *              if we have exceeded the max number of iterations.
 *
 * PARAMETERS:
 *     reccb:   Recovery control block
 *     fname:   Function name
 *
 * RETURNS:     0 if reset not requested, otherwise 1.
 */
static int sccprec_count_list_iterations (sccprec_reccb_t *reccb, char *fname)
{
    if (++(reccb->cm_list_iterations) >= SCCPREC_MAX_CM_LIST_ITERATIONS) {
        SCCP_DBG((sccprec_debug, SCCPREC_DBG_LEVEL_INFO2,
                 "%s %-2d:%-2d: %-20s: CM list interation count exceeded\n",
                 SCCPREC_ID, reccb->sccpcb->id, reccb->id, fname));

        SCCP_RESET(reccb, GAPI_MSG_RESET, GAPI_CAUSE_NO_CM_FOUND);

        return (1);
    } 

    return (0);
}

/*
 * FUNCTION:    sccprec_get_mac
 *
 * DESCRIPTION: Return the mac address for the client. This is the address
 *              supplied by the application when opening a seesion. The 
 *              buffer is already NULL terminated.
 *
 * PARAMETERS:
 *     sccpcb:  sccp control block
 *
 * RETURNS:     mac address
 */                    
char *sccprec_get_mac (sccp_sccpcb_t *sccpcb)
{
    return (sccpcb->reccb->mac);
}

/*
 * FUNCTION:    sccprec_get_media_caps
 *
 * DESCRIPTION: Return the media capabilities for the client. This is the
 *              media supplied by the application when opening a seesion.
 *
 * PARAMETERS:
 *     sccpcb:  sccp control block
 *
 * RETURNS:     media capabilities.
 */                    
gapi_media_caps_t *sccprec_get_media_caps (sccp_sccpcb_t *sccpcb)
{
    return (&(sccpcb->reccb->media_caps));
}

/*
 * FUNCTION:    sccprec_get_index
 *
 * DESCRIPTION: Return the cm index of the current cm that sccprec is
 *              working with.
 *
 * PARAMETERS:
 *     sccpcb:  sccp control block.
 *
 * RETURNS:     index
 */                    
int sccprec_get_index (sccp_sccpcb_t *sccpcb)
{
    return (sccpcb->reccb->cm_index);
}

/*
 * FUNCTION:    sccprec_connect_cm
 *
 * DESCRIPTION: Send a connect event to the requested cm.
 *
 * PARAMETERS:
 *     reccb:   sccprec control block
 *     index:   cm index
 *     fname:   function name
 *
 * RETURNS:     None.
 */                    
static void sccprec_connect_cm (sccprec_reccb_t *reccb, int index,
                                char *fname)
{
    SCCP_DBG((sccprec_debug, SCCPREC_DBG_LEVEL_INFO2, 
             "%s %-2d:%-2d: %-20s: %s: trying to connect\n",
             SCCPREC_ID, reccb->sccpcb->id, reccb->id, fname,
             sccpcm_print_cm(reccb->cmcbs[index])));

    sccpcm_push_con_req(reccb->sccpcb, SCCPCM_E_CON_REQ, index);

    /*
     * Start a supervision timer.
     */
    sccprec_start_timer(reccb, reccb->misc_timer,
                        SCCPREC_TIMER_WF_CONNECT,
                        reccb->connect_to + SCCPREC_FUDGE_TO);
}

/*
 * FUNCTION:    sccprec_register_cm
 *
 * DESCRIPTION: Send a register event to the requested cm.
 *
 * PARAMETERS:
 *     reccb:   sccprec control block
 *     index:   cm index
 *     fname:   function name
 *
 * RETURNS:     None.
 */                    
static void sccprec_register_cm (sccprec_reccb_t *reccb, int index,
                                 char *fname)
{
    SCCP_DBG((sccprec_debug, SCCPREC_DBG_LEVEL_INFO2,
             "%s %-2d:%-2d: %-20s: %s: trying to register\n",
             SCCPREC_ID, reccb->sccpcb->id, reccb->id, fname,
             sccpcm_print_cm(reccb->cmcbs[index])));

    /*
     * Reset the primary keepalive timeout. The value will later be set to the
     * the value returned in the registerAck.
     */
//    reccb->keepalive_t1 = reccb->default_keepalive_to;
//    reccb->keepalive_t2 = reccb->default_keepalive_to;
    sccprec_set_keepalive_timeout(reccb->sccpcb,
                                  reccb->default_keepalive_to, 2);

    sccpcm_push_reg_req(reccb->sccpcb, SCCPCM_E_REG_REQ, index);

    /*
     * Start a supervision timer.
     */
    sccprec_start_timer(reccb, reccb->misc_timer,
                        SCCPREC_TIMER_WF_REGISTER,
						//sccpcm_get_keepalive_timeout(reccb->cmcbs[index]) *
                        SCCPREC_WF_REGISTER_TO * 
                        SCCPREC_ACK_RETRIES_BEFORE_LOCKOUT +
                        SCCPREC_FUDGE_TO);
}

/*
 * FUNCTION:    sccprec_unregister_cm
 *
 * DESCRIPTION: Send an unregister event to the requested cm.
 *
 * PARAMETERS:
 *     reccb:   sccprec control block
 *     index:   cm index
 *     fname:   function name
 *
 * RETURNS:     None.
 */                    
static void sccprec_unregister_cm (sccprec_reccb_t *reccb,
                                   int index, char *fname)
{
    SCCP_DBG((sccprec_debug, SCCPREC_DBG_LEVEL_INFO2,
             "%s %-2d:%-2d: %-20s: %s: trying to unregister\n",
             SCCPREC_ID, reccb->sccpcb->id, reccb->id, fname,
             sccpcm_print_cm(reccb->cmcbs[index])));

    sccpcm_push_unreg_req(reccb->sccpcb, SCCPCM_E_UNREG_REQ, index);

    /*
     * Start a supervision timer.
     */
    sccprec_start_timer(reccb, reccb->misc_timer,
                        SCCPREC_TIMER_WF_UNREGISTER,
                        SCCPREC_WF_UNREGISTER_TO * SCCPREC_UNREG_ACK_RETRIES +
                        SCCPREC_FUDGE_TO);
}

#if 0 /* not used yet */
/*
 * FUNCTION:    sccprec_disconnect_cm
 *
 * DESCRIPTION: Send a disconnect event to the requested cm.
 *
 * PARAMETERS:
 *     reccb:   sccprec control block
 *     index:   cm index
 *     fname:   function name
 *
 * RETURNS:     None.
 */                    
static void sccprec_disconnect_cm (sccprec_reccb_t *reccb,
                                   int index, char *fname)
{
    SCCP_DBG((sccprec_debug, SCCPREC_DBG_LEVEL_INFO2,
             "%s %-2d:%-2d: %-20s: %s: trying to disconnect\n",
             SCCPREC_ID, reccb->sccpcb->id, reccb->id, fname,
             sccpcm_print_cm(reccb->cmcbs[index])));

    sccpcm_push_discon_req(reccb->sccpcb, SCCPCM_E_DISCON_REQ, index);

#if 0
    /*
     * Start a supervision timer.
     */
    sccprec_start_timer(reccb, reccb->misc_timer,
                        SCCPREC_TIMER_WF_UNREGISTER,
                        SCCPREC_WF_UNREGISTER_TO * SCCPREC_MAX_ACK_RETRY +
                        SCCPREC_FUDGE_TO);
#endif
}
#endif

/*
 * FUNCTION:    sccprec_close_cm
 *
 * DESCRIPTION: Send a close event to the requested cm.
 *
 * PARAMETERS:
 *     reccb:   sccprec control block
 *     index:   cm index
 *     fname:   function name
 *     error:   close type
 *
 * RETURNS:     None.
 */                    
static void sccprec_close_cm (sccprec_reccb_t *reccb,
                              int index, char *fname, int error)
{
    SCCP_DBG((sccprec_debug, SCCPREC_DBG_LEVEL_INFO2,
             "%s %-2d:%-2d: %-20s: %s: trying to close\n",
             SCCPREC_ID, reccb->sccpcb->id, reccb->id, fname,
             sccpcm_print_cm(reccb->cmcbs[index])));

    sccpcm_push_error(reccb->sccpcb, SCCPCM_E_ERROR, index,
                      error);
}

#if 0 /* not used yet */
/*
 * FUNCTION:    sccprec_shutdown_cms
 *
 * DESCRIPTION: Send shutdown events to all non-IDLE cms.
 *              Registered cms will receive unregister requests and connected
 *              cms will receive disonnects.
 *
 * PARAMETERS:
 *     reccb:   sccprec control block
 *     fname:   function name
 *
 * RETURNS:     None.
 */                    
static void sccprec_shutdown_cms (sccprec_reccb_t *reccb, char *fname)
{
    int i;

    /*sam - might be easier use ERROR for registered cms */
    for (i = 0; i < reccb->max_cm_index; i++) {        
        if (reccb->cmcbs[i] != NULL) {
            if (sccpcm_is_cmcb_registered(reccb->cmcbs[i]) == 1) {
                sccprec_unregister_cm(reccb, i, fname);
            }
            else if (sccpcm_is_cmcb_idle(reccb->cmcbs[i]) != 1) {
                sccprec_disconnect_cm(reccb, i, fname);
            }
        }
    }
}
#endif

/*
 * FUNCTION:    sccprec_close_cms
 *
 * DESCRIPTION: Send close events to all cms. This is accomplished by sending
 *              error events.
 *
 * PARAMETERS:
 *     reccb:   sccprec control block
 *     fname:   function name
 *
 * RETURNS:     Number of cms closed.
 */                    
static int sccprec_close_cms (sccprec_reccb_t *reccb, char *fname)
{
    int i;
    int count = 0;

    for (i = 0; i < reccb->max_cm_index; i++) {        
        if ((reccb->cmcbs[i] != NULL) &&
            (sccpcm_is_cmcb_idle(reccb->cmcbs[i]) != 1)) {
            sccprec_close_cm(reccb, i, fname, SCCPCM_ERROR_CLOSE_ALL_NOW);
            count++;
        }
    }

    return (count);
}

/*
 * FUNCTION:    sccprec_get_next_cm_index
 *
 * DESCRIPTION: Increments the cm index counter. Wraps to 0 if the end
 *              is reached.
 *
 * PARAMETERS:
 *     reccb:   sccprec control block
 *
 * RETURNS:     None.
 */                    
static void sccprec_get_next_cm_index (sccprec_reccb_t *reccb)
{
	if (++(reccb->cm_index) >= reccb->max_cm_index) {
        reccb->cm_index = 0;
    }
}

/*
 * FUNCTION:    sccprec_init
 *
 * DESCRIPTION: Initialize the sccprec software.
 *
 * PARAMETERS:  None.
 *
 * RETURNS:     0 for success, otherwise 1.
 */
int sccprec_init (void)
{
    return (0);
}

/*
 * FUNCTION:    sccprec_cleanup
 *
 * DESCRIPTION: Cleanup the sccprec software.
 *
 * PARAMETERS:  None.
 *
 * RETURNS:     0 for success, otherwise 1.
 */
int sccprec_cleanup (void)
{
    return (0);
}

/*
 * FUNCTION:    sccprec_open_init
 *
 * DESCRIPTION: Initializes the reccb data with the received data from
 *              an open request. This data needs to persist in the reccb
 *              because sccprec will reuse the data while running through
 *              it's state machine.
 *
 * PARAMETERS:
 *     event:   open event 
 *
 * RETURNS:     Count of valid CMs, otherwise -1 for error.
 */                    
static int sccprec_open_init (sem_event_t *event)
{
    int                i, j;
    sccprec_reccb_t    *reccb = (sccprec_reccb_t *)(event->cb);
    sccprec_msg_open_t *msg   = (sccprec_msg_open_t *)(event->data);
    int                count  = 0;
    sccpcm_cmcb_t      *cmcb;

#if 0 /* test code */
	//memset(msg->cms, 0, 5*sizeof(*cms));
	//msg->mac[0] = '\0';
	msg->media_caps.count = 0;
#endif

//    sccprec_init_reccb(reccb, SCCPREC_RESET_TYPE_RESTART);

    /*
     * Free up any existing cms. This function will also disconnect any
     * existing sockets.
     */
//    sccpcm_free_all_cmcbs(reccb->sccpcb);

    sccprec_set_free_events_flag(reccb->sccpcb, 0);

    /*
     * Verify that the app has supplied valid data.
     */
    if ((msg->mac[0] == '\0') ||
        (msg->media_caps.count <= 0)) {
        return (-1);
    }

    /*
     * Copy the mac address. Make sure to NULL terminate.
     */
    for (i = 0; i < GAPI_MAX_MAC_SIZE; i++) {
        reccb->mac[i] = msg->mac[i];
        reccb->mac[GAPI_MAX_MAC_SIZE - 1] = '\0';
    }

    /*
     * Copy the media.
     */
    for (i = 0; i < msg->media_caps.count; i++) {
        reccb->media_caps.caps[i].payload = msg->media_caps.caps[i].payload;
        reccb->media_caps.caps[i].milliseconds_per_packet = msg->media_caps.caps[i].milliseconds_per_packet;
    }
    reccb->media_caps.count = msg->media_caps.count;

    sccp_set_version(reccb->sccpcb, msg->version);

    /*
     * Copy the configurable values.
     */
    if (msg->values.call_end_to > 0) {
        reccb->call_end_to = msg->values.call_end_to;
    }
    if (msg->values.close_to > 0) {
        reccb->close_to = msg->values.close_to;
    }
    if (msg->values.connect_to > 0) {
        reccb->connect_to = msg->values.connect_to;
    }
    if (msg->values.default_keepalive_to > 0) {
        reccb->default_keepalive_to = msg->values.default_keepalive_to;
    }
    if (msg->values.device_poll_to > 0) {
        reccb->device_poll_to = msg->values.device_poll_to;
    }
    if (msg->values.nak_to_syn_retry_to > 0) {
        reccb->nak_to_syn_retry_to = msg->values.nak_to_syn_retry_to;
    }
    if (msg->values.device_type > 0) {
        reccb->device_type = msg->values.device_type;
    }
    reccb->cc_mode = msg->values.cc_mode;

    /*
     * Copy the list of call managers and create new cms.
     */
    for (i = 0; i < SCCPREC_MAX_CM_SERVERS; i++) {
        /*
         * Make sure we have a good address.
         */
        if ((msg->cms[i].addr != 0) && (msg->cms[i].port != 0)) { 
            /*
             * Grab a new cm to handle the address.
             */
            cmcb = sccpcm_get_new_cmcb(reccb->sccpcb, count);
            if (cmcb != NULL) {
                sccpcm_init_cmcb(reccb->sccpcb, count,
                                 SCCPCM_CCM_TYPE_STANDARD,
                                 msg->cms[i].addr,
                                 msg->cms[i].port);

                reccb->cmcbs[count] = cmcb;
                
                count++;
            }
        } /* if ((msg->cms[i].addr != 0) && (msg->cms[i].port != 0)) { */
    } /* for (i = 0; i < SCCPREC_MAX_CM_SERVERS; i++) { */

    /*
     * Default to the tftp server as the CM if we did not find any
     * valid addresses.
     */
    if (count == 0) {
        if ((msg->tftp.addr != 0) && (msg->tftp.port != 0)) {
            /*
             * Grab a new cm to handle the address.
             */
            cmcb = sccpcm_get_new_cmcb(reccb->sccpcb, count);
            if (cmcb != NULL) {
                sccpcm_init_cmcb(reccb->sccpcb, count,
                                 SCCPCM_CCM_TYPE_STANDARD,
                                 msg->tftp.addr,
                                 msg->tftp.port);

                reccb->cmcbs[count] = cmcb;

                count++;
            }
        }
    } /* if (count == 0) {*/

    /*
     * Add SRST as fallback CM if necessary.
     */
    if (msg->srst_mode == GAPI_SRST_MODE_ENABLE) {
        /*
         * Copy the list of srst's and create new cms.
         */
        for (i = 0; i < SCCPREC_MAX_SRST_SERVERS; i++) {
            /*
             * Make sure we have a good address.
             */
            if ((msg->srsts[i].addr != 0) && (msg->srsts[i].port != 0)) { 
                /*
                 * Make sure we still have space.
                 */
                if (count < SCCPREC_MAX_CM_SERVERS) {
                    /*
                     * Look through the list and make sure
                     * the srst is not already there.
                     */
                    for (j = 0; j < SCCPREC_MAX_CM_SERVERS; j++) {
                        if ((reccb->cmcbs[j]->addr == msg->srsts[i].addr) &&
                            (reccb->cmcbs[j]->port == msg->srsts[i].port)) {
                            break;
                        }
#if 0
                        /*
                         * Find an empty spot.
                         */
                        if ((reccb->cms[j].addr = 0) &&
                            (reccb->cms[j].port = 0) &&
                            (valid_entry == 0)) {
                            valid_entry = j;
                        }
#endif
                    }

                    /*
                     * Add this SRST cm to the list if we did not find
                     * it as a standard or tftp server.
                     */
                    if (j == SCCPREC_MAX_CM_SERVERS) {
                        /*
                         * Grab a new cm to handle the address.
                         */
                        cmcb = sccpcm_get_new_cmcb(reccb->sccpcb, count);
                        if (cmcb != NULL) {
                            sccpcm_init_cmcb(reccb->sccpcb, count,
                                             SCCPCM_CCM_TYPE_SRST_FALLBACK,
                                             msg->srsts[i].addr,
                                             msg->srsts[i].port);
                            
                            reccb->cmcbs[count] = cmcb;

                            count++;
                        }
                    }
                }
            } /* if ((msg->srsts[i].addr != 0) && (msg->srsts[i].port != 0)) { */
        } /* for (i = 0; i < SCCPREC_MAX_SRST_SERVERS; i++) { */
    } /* if (msg->srst_mode == GAPI_SRST_MODE_ENABLE) { */

	reccb->max_cm_index = count;

    return (count);
}

/*
 * FUNCTION:    sccprec_get_wait_before_unregister
 *
 * DESCRIPTION: Calculate a random amount of time to wait before
 *              unregistering.
 *
 * PARAMETERS:
 *     reccb:   sccprec control block
 *
 * RETURNS:     None.
 */                    
static int sccprec_get_wait_before_unregister (sccprec_reccb_t *reccb)
{
    int          wait = 0;
    unsigned int rnd;

    if (sccpcm_is_cm_registered_with_fallback(reccb->sccpcb) != 1) {
        SCCP_MEMCPY(&rnd, &(reccb->mac[2]), 4);

        rnd = (rnd % (SCCPREC_MAX_WAIT_FOR_UNREGISTER_TO -
                      SCCPREC_MIN_WAIT_FOR_UNREGISTER_TO)) +
              SCCPREC_MIN_WAIT_FOR_UNREGISTER_TO;
        wait = (int)(rnd + 1);
    }

    return (wait);
}

/*
 * ---------------------- State-Event Machine Functions -----------------------
 */

/*
 * FUNCTION:    sccprec_sem_default
 *
 * DESCRIPTION: Handles unrecognized events.
 *
 * PARAMETERS:
 *     event:   event control block
 *
 * RETURNS:     0 for success, otherwise 1.
 *
 * NOTES:
 *
 * ============================================================================
 * S_ALL                     E_ALL                         NO CHANGE
 * ----------------------------------------------------------------------------
*/
static int sccprec_sem_default (sem_event_t *event)
{
    return (0);
}

/*
 * FUNCTION:    sccprec_sem_done
 *
 * DESCRIPTION: Cleanup function. This function is called as a fincal catch
 *              for the sccprec sm after it has finished running though the 
 *              whole sm and during error cases. The function will try to
 *              ensure that a primary is defined and if not it will intiiate
 *              the proper procedures to find one.
 *
 * PARAMETERS:
 *     event:   event control block
 *
 * RETURNS:     0 for success, otherwise 1.
 *
 * NOTES:
 *
 * ============================================================================
 * S_ALL                     E_DONE                        S_IDLE
 * ----------------------------------------------------------------------------
*/
static int sccprec_sem_done (sem_event_t *event) 
{
    sccprec_reccb_t *reccb = (sccprec_reccb_t *)(event->cb);

    /*
     * Make sure a cm is defined.
     */
    if (sccpcm_get_cm_count(reccb->sccpcb) <= 0) {
        SCCP_DBG((sccprec_debug, SCCPREC_DBG_LEVEL_INFO2,
                 "%s %-2d:%-2d: %-20s: no CMs defined\n",
                 SCCPREC_ID, reccb->sccpcb->id, reccb->id,
                 event->function_name));
                    
        /*
         * Set a timer and try again. Maybe we will have some
         * cm's defined when the timer pops.
         */
        sccprec_start_timer(reccb, reccb->misc_timer,
                            SCCPREC_TIMER_RETRY_NO_CM,
                            SCCPREC_RETRY_NO_CMS_DEFINED_TO);
                
        return (0);                         
    }     

    /*
     * Check if all the cms are lockout. If so, let's request a reset
     * from the application so the reset cleans things up by
     * resetting the session and starting over.
     */
    if (sccpcm_are_all_cms_lockout(reccb->sccpcb) == 1) {
        SCCP_RESET(reccb, GAPI_MSG_RESET, GAPI_CAUSE_NO_CM_FOUND);

        return (0);
    }

    /*
     * Make sure we have a primary.
     */
    if (sccpcm_get_primary_index(reccb->sccpcb) == SCCPCM_NO_INDEX) {
        /*
         * Are we in retry mode? If not, then we can just go ahead and kick
         * of the sm again to find a primary.
         */
        if (reccb->pri_retries == 0) {
            SCCP_DBG((sccprec_debug, SCCPREC_DBG_LEVEL_INFO2,
                     "%s %-2d:%-2d: %-20s: no primary, will try now\n",
                     SCCPREC_ID, reccb->sccpcb->id, reccb->id,
                     event->function_name));

            sccprec_push_simple(reccb->sccpcb, SCCPREC_E_PRI_CHECK);
        
            return (0);
        }
        else {         
            /*
             * Start timer for next polling. We don't have a primary so
             * we wait a little while and start the process over.
             */
            SCCP_DBG((sccprec_debug, SCCPREC_DBG_LEVEL_INFO2,
                     "%s %-2d:%-2d: %-20s: no primary, in retry mode, "
                     "retries= %d, list iterations= %d\n",
                     SCCPREC_ID, reccb->sccpcb->id, reccb->id,
                     event->function_name,
                     reccb->pri_retries, reccb->cm_list_iterations));

            sccprec_start_timer(reccb, reccb->misc_timer,
                                SCCPREC_TIMER_RETRY_PRI,
                                SCCPREC_RETRY_PRI_TO);
                        
            /*
             * Reset the flag so that when we timeout and come back
             * to this function, we will enter the if part of this 
             * conditional and start looking for the primary again.
             */
            reccb->pri_retries = 0;
            
            return (0);
        }
    }
    
    /*
     * Clean up any secondarys.
     */
     sccprec_cleanup_secondarys(reccb, event->function_name);
     
    /*
     * Everything looks good. Start timer for next polling and wait.
     * This is the 10s pop where we keep starting the state machine to
     * check if we have everything optimized.
     */
    sccprec_start_timer(reccb, reccb->misc_timer,
                        SCCPREC_TIMER_DEVICE_POLL,
                        reccb->device_poll_to);

    return (0);                        
}

/*
 * FUNCTION:    sccprec_sem_pri_check
 *
 * DESCRIPTION: Ensure that a primary is defined and if not initiate the 
 *              proper procedures to find one.
 *
 * PARAMETERS:
 *     event:   event control block
 *
 * RETURNS:     0 for success, otherwise 1.
 *
 * NOTES:
 *
 * ============================================================================
 * S_IDLE                    E_PRI_CHECK                   S_PRI_CHECK
 * ----------------------------------------------------------------------------
 * S_IDLE                    E_TIMEOUT                     S_PRI_CHECK
 * ---------------------------------------------------------------------------- 
 * S_IDLE                    E_DEV_NOTIFY                  S_PRI_CHECK
 * ----------------------------------------------------------------------------
 * S_PRI_CHECK               E_TIMEOUT                     S_PRI_CHECK
 * ---------------------------------------------------------------------------- 
 * S_PRI_REG_CM              E_PRI_CHECK                   S_PRI_CHECK
 * ----------------------------------------------------------------------------
 */
static int sccprec_sem_pri_check (sem_event_t *event)
{
    sccprec_reccb_t *reccb = (sccprec_reccb_t *)(event->cb);

	/*
	 * Check if this was a real timeout to add debug. There are other functions
	 * which will call this function.
	 */
    if (event->event_id == SCCPREC_E_TIMEOUT) {
        sccprec_msg_timeout_t *msg = (sccprec_msg_timeout_t *)(event->data);

        SCCP_DBG((sccprec_debug, SCCPREC_DBG_LEVEL_SEM,
                 "%s %-2d:%-2d: %-20s: %s\n",
                 SCCPREC_ID, reccb->sccpcb->id, reccb->id,
                 event->function_name,
                 sccprec_timer_name(msg->event)));

	    /*
         * Toss extra T_WF_CLOSE timeouts. sccprec generates a timeout event
         * for each dev_notify it receives when resetting, but sccprec cleans 
         * up after receiving the first timeout so these extra timeouts are
         * still around.
         */
        if (msg->event == SCCPREC_TIMER_WF_CLOSE) {
            return (0);
        }
    }
    else {
        sccprec_stop_timer(reccb, reccb->misc_timer);
    }

    /*
     * Make sure a CM is defined.
     */
    if (sccpcm_get_cm_count(reccb->sccpcb) <= 0) {
        SCCP_DBG((sccprec_debug, SCCPREC_DBG_LEVEL_INFO2, 
                 "%s %-2d:%-2d: %-20s: Shutting down or no CMs defined\n",
                 SCCPREC_ID, reccb->sccpcb->id, reccb->id,
                 event->function_name));

        /*
         * Do nothing because there are no CMs defined.
         */
        sccprec_push_simple(reccb->sccpcb, SCCPREC_E_DONE);
        
        return (0);                         
    }     

    /*
     * Do we have a primary?
     */
    if (sccpcm_get_primary_index(reccb->sccpcb) == SCCPCM_NO_INDEX) {
        /*
         * Check if we can start looking for the primary.
         * We can only look if there are not active calls - we don't
         * want to mess up any active calls.
         */
        if (SCCP_ALL_STREAMS_IDLE(reccb) == 0) {
            /*
             * Let the application know that we are not connected
             * to a primary CM.
             */
            SCCP_SESSIONSTATUS(reccb, GAPI_MSG_STATUS,
                               GAPI_STATUS_CM_DOWN,
                               GAPI_STATUS_DATA_MIN, NULL);
                       
            /*
             * Increment this flag. The flag is used to know that we were
             * waiting for calls to complete and might need to do some
             * cleanup in the else part of this conditional.
             */
            reccb->waiting_to_find_pri++;
        
            /*
             * We will try to wait for the call to end before we try
             * again.
             */
            sccprec_start_timer(reccb, reccb->misc_timer,
                                SCCPREC_TIMER_WF_CALL_END,
                                reccb->call_end_to);
                    
            return (0);                            
        }
        else  {
            /*
             * We might need to do some cleanup if this flag is set.
             */
            if (reccb->waiting_to_find_pri != 0) {
                SCCP_CLOSE_ABANDONDED_STREAMS(reccb);
            
                reccb->waiting_to_find_pri = 0;
            }

            /*
             * Don't have a primary so we need to find a primary.
             */
            sccprec_push_simple(reccb->sccpcb, SCCPREC_E_FIND_PRI);
            
            return (0);                            
        }
    }
    /*
     * Already have a primary, so check for a secondary.
     */
    else  {
        sccprec_push_simple(reccb->sccpcb, SCCPREC_E_SEC_CHECK);
        
        return (0);                            
    }
}

/*
 * FUNCTION:    sccprec_sem_pri_check_find_pri
 *
 * DESCRIPTION: Find a primary from the cm list. First try to find the 
 *              highest priority connected cm and register with it. If one
 *              is not found, try to find an idle cm and connect with it.
 *
 * PARAMETERS:
 *     event:   event control block
 *
 * RETURNS:     0 for success, otherwise 1.
 *
 * NOTES:
 *
 * ============================================================================
 * S_PRI_CHECK               E_FIND_PRI                    S_FIND_PRI
 * ----------------------------------------------------------------------------
 */
static int sccprec_sem_pri_check_find_pri (sem_event_t *event) 
{
    sccprec_reccb_t *reccb = (sccprec_reccb_t *)(event->cb);
    int             i;
    
    /*
     * Search for a secondary that is already connected.
     * We use the term secondary, but don't think we are looking for
     * secondary CM. We are just trying to find a free CM that is
     * connected. Any CM that is connected and not registered is
     * a secondary.
     */

    /*
     * Start from the highest priority because this is
     * is the first time through the list.
     */
    for (i = 0; i < reccb->max_cm_index; i++) {
        /*
         * Look for connected and token cms.
         */
        if (sccpcm_is_cmcb_connected(reccb->cmcbs[i]) == 1) {
            /*
             * Got one that is connected, let's try and register with it.
             */

            /*
             * First time with this cm, so let's mark it so.
             */
            reccb->pri_retries = 0;

            /*
             * Save the working index.
             */
            reccb->cm_index = i;
            
            /*
             * Save starting index so we only look through
             * the list one time.
             */
            reccb->start_index = i;

            sccprec_register_cm(reccb, i, event->function_name);

            /*
             * Wait for notification.
             */
            return (0);                            
        } 
    }

    /*
     * No secondary. We need to find an idle cm, make a connection
     * and then register.
     */
     for (reccb->cm_index = 0; reccb->cm_index < reccb->max_cm_index;
          reccb->cm_index++) {
        if (sccpcm_is_cmcb_idle(reccb->cmcbs[reccb->cm_index]) == 1) {
            reccb->pri_retries = 0;        

            /*
             * Tell the CM to try and connect.
             */
            sccprec_connect_cm(reccb, reccb->cm_index,
                               event->function_name);

            /*
             * Save starting index so we only look through
             * the list one time.
             */
            reccb->start_index = reccb->cm_index;                                 

            /*
             * Wait around for the connect_ack.
             */

            return (0);                 
        } 
    }
    
    /*
     * No CM found - wait for next time and try again.
     */
    SCCP_DBG((sccprec_debug, SCCPREC_DBG_LEVEL_INFO2,
             "%s %-2d:%-2d: %-20s: No CM found to make primary\n",
             SCCPREC_ID, reccb->sccpcb->id, reccb->id, event->function_name));

    /*
     * Track that we are in retry mode. The flag will be checked after we 
     * go through the whole state machine.
     */
    reccb->pri_retries++;

    /*
     * Make sure we do not retry too many times.
     */
    if (sccprec_count_list_iterations(reccb, event->function_name) == 0) {
        /*
         * Didn't find a primary. Let's just let the cleanup function fix
         * everything.
         */
        sccprec_push_simple(reccb->sccpcb, SCCPREC_E_DONE);    
    }

    return (0);
}

/*
 * FUNCTION:    sccprec_sem_sec_check
 *
 * DESCRIPTION: Ensure that a secondary is defined and if not initiate the 
 *              proper procedures to find one.
 *
 * PARAMETERS:
 *     event:   event control block
 *
 * RETURNS:     0 for success, otherwise 1.
 *
 * NOTES:
 *
 * ============================================================================
 * S_PRI_CHECK               E_SEC_CHECK                   S_SEC_CHECK
 * ----------------------------------------------------------------------------
 * S_FIND_PRI                E_SEC_CHECK                   S_SEC_CHECK
 * ----------------------------------------------------------------------------
 * S_PRI_REG_CM              E_SEC_CHECK                   S_SEC_CHECK
 * ----------------------------------------------------------------------------
 */
static int sccprec_sem_sec_check (sem_event_t *event) 
{
    sccprec_reccb_t *reccb = (sccprec_reccb_t *)(event->cb);
    int i;

    /*
     * Make sure we have a primary. No need to look for a scondary if
     * don't have a primary.
     */
    if (sccpcm_get_primary_index(reccb->sccpcb) == SCCPCM_NO_INDEX) {
        /*
         * No primary. Go back and try to find a primary.
         */
        sccprec_push_simple(reccb->sccpcb, SCCPREC_E_PRI_OPTIMIZE);
        
        return (0);
    }

    /*
     * Make sure we can allocate a secondary. Gotta have at least
     * two CMs defined - one for the primary, one for secondary.
     */
    if (sccpcm_get_cm_count(reccb->sccpcb) < 2) {
        /*
         * Not enough CMs defined to have a secondary.
         * Goto next state.
         */
        SCCP_DBG((sccprec_debug, SCCPREC_DBG_LEVEL_INFO2, 
                 "%s %-2d:%-2d: %-20s: not enough CMs to have secondary\n",
                 SCCPREC_ID, reccb->sccpcb->id, reccb->id,
                 event->function_name));

        sccprec_push_simple(reccb->sccpcb, SCCPREC_E_PRI_OPTIMIZE);
        
        return (0);
    }

    /*
     * Check if we already have a secondary. The primary will
     * be in a REGISTERED state so the first other cm in a CONNECTED
     * state will be a secondary.
     */
    for (i = 0; i < reccb->max_cm_index; i++) {
        /*
         * Include connected and token cms.
         */
        if (sccpcm_is_cmcb_connected(reccb->cmcbs[i]) == 1) {
            /*
             * Found a secondary. Goto next state.
             */
            sccprec_push_simple(reccb->sccpcb, SCCPREC_E_PRI_OPTIMIZE);
        
            return (0);
        }
    }

    /*
     * Don't have a secondary, let's try to find one.
     */
    SCCP_DBG((sccprec_debug, SCCPREC_DBG_LEVEL_INFO2,
             "%s %-2d:%-2d: %-20s: no secondary, finding\n",
             SCCPREC_ID, reccb->sccpcb->id, reccb->id, event->function_name));

    sccprec_push_simple(reccb->sccpcb, SCCPREC_E_FIND_SEC);
    
    return (0);
}

/*
 * FUNCTION:    sccprec_sem_find_pri_dev_notify
 *
 * DESCRIPTION: Something has changed in a cm. Check if the cm is registered or
 *              connected. Move on to the next if neither.
 *
 * PARAMETERS:
 *     event:   event control block
 *
 * RETURNS:     0 for success, otherwise 1.
 *
 * NOTES:
 *
 * ============================================================================
 * S_FIND_PRI                E_DEV_NOTIFY                  S_FIND_PRI
 * ----------------------------------------------------------------------------
 */
static int sccprec_sem_find_pri_dev_notify (sem_event_t *event) 
{
    sccprec_reccb_t *reccb = (sccprec_reccb_t *)(event->cb);
    int             index  = reccb->cm_index;

    /*
     * The CCM has rejected the register request. Set a timer to retry.
     */
    if (reccb->reject == 1) {
        sccprec_start_timer(reccb, reccb->misc_timer, SCCPREC_TIMER_REJECT,
                            SCCPREC_REJECT_TO);

        return (0);
    }

    /*
     * Validate the index.
     */
    if (sccpcm_get_cm_by_index(reccb->sccpcb, index) == SCCPCM_NO_INDEX) {
        SCCP_DBG((sccprec_debug, SCCPREC_DBG_LEVEL_INFO2,
                 "%s %-2d:%-2d: %-20s: CM[%d]: NULL\n",
                 SCCPREC_ID, reccb->sccpcb->id, reccb->id,
                 event->function_name,
                 index));

        sccprec_push_simple(reccb->sccpcb, SCCPREC_E_DONE);

        return (0);
    }

    /*
     * Are we registered yet?
     */
    if (sccpcm_is_cmcb_registered(reccb->cmcbs[index]) == 1) {
        SCCP_DBG((sccprec_debug, SCCPREC_DBG_LEVEL_INFO2,
                 "%s %-2d:%-2d: %-20s: %s: registered\n",
                 SCCPREC_ID, reccb->sccpcb->id, reccb->id,
                 event->function_name,
                 sccpcm_print_cm(reccb->cmcbs[index])));

        sccprec_stop_timer(reccb, reccb->misc_timer);

        /*
         * Reset the cm list iteration count.
         */
        reccb->cm_list_iterations = 0;

        /*
         * Let the application know that we have a primary.
         */
        SCCP_OPENSESSION_RES(reccb, GAPI_MSG_OPENSESSION_RES, GAPI_CAUSE_OK);

        /*
         * Got a primary, let's look for secondary.
         */
        sccprec_push_simple(reccb->sccpcb, SCCPREC_E_SEC_CHECK);
    
        return (0);
    }

    /*
     * Try to register with this cm if it is still connected. Don't check
     * the token states.
     */
    if (sccpcm_is_cmcb_connected2(reccb->cmcbs[index]) == 1) {
        SCCP_DBG((sccprec_debug, SCCPREC_DBG_LEVEL_INFO2,
                 "%s %-2d:%-2d: %-20s: %s: connected\n",
                 SCCPREC_ID, reccb->sccpcb->id, reccb->id,
                 event->function_name,
                 sccpcm_print_cm(reccb->cmcbs[index])));

        sccprec_register_cm(reccb, index, event->function_name);

        return (0);
    }

    /*
     * If we are IDLE or LOCKOUT then get next.
     */
    if ((sccpcm_is_cmcb_idle(reccb->cmcbs[index]) == 1) ||
        (sccpcm_is_cmcb_lockout(reccb->cmcbs[index]) == 1)) {
        sccprec_stop_timer(reccb, reccb->misc_timer);

        SCCP_DBG((sccprec_debug, SCCPREC_DBG_LEVEL_INFO2,
                 "%s %-2d:%-2d: %-20s: %s: connect or register failed, "
                 "trying to find next\n",
                 SCCPREC_ID, reccb->sccpcb->id, reccb->id,
                 event->function_name,
                 sccpcm_print_cm(reccb->cmcbs[index])));

        /*
         * Try next.
         */

        /*
         * Set the function_name so that the debug is corect.
         */
		event->function_name = 
			sccprec_function_name(SCCPREC_SEM_FIND_PRI_TIMEOUT);

		sccprec_sem_find_pri_timeout(event);

        return (0);
    }

    /*
     * Just wait, another connection must have changed.
     */
    return (0);
}

/*
 * FUNCTION:    sccprec_sem_find_pri_timeout
 *
 * DESCRIPTION: Ensure that a primary is defined and if not initiate the 
 *              proper procedures to find one.
 *
 * PARAMETERS:
 *     event:   event control block
 *
 * RETURNS:     0 for success, otherwise 1.
 *
 * NOTES:
 *
 * ============================================================================
 * S_FIND_PRI                E_TIMEOUT                     S_FIND_PRI
 * ----------------------------------------------------------------------------
 */
static int sccprec_sem_find_pri_timeout (sem_event_t *event) 
{
    sccprec_reccb_t *reccb = (sccprec_reccb_t *)(event->cb);
    int             index  = reccb->cm_index;

	/*
	 * Check if this was a real timeout to add debug. There are other functions
	 * which will call this function.
	 */
    if (event->event_id == SCCPREC_E_TIMEOUT) {
		sccprec_msg_timeout_t *msg = (sccprec_msg_timeout_t *)(event->data);

        SCCP_DBG((sccprec_debug, SCCPREC_DBG_LEVEL_SEM,
                 "%s %-2d:%-2d: %-20s: %s\n",
                 SCCPREC_ID, reccb->sccpcb->id, reccb->id,
                 event->function_name,
                 sccprec_timer_name(msg->event)));

        if (msg->event == SCCPREC_TIMER_REJECT) {
            SCCP_RESET(reccb, GAPI_MSG_RESET, GAPI_CAUSE_CM_REGISTER_REJECT);

            return (0);
        }
    }

    /*
     * Validate the index.
     */
    if (sccpcm_get_cm_by_index(reccb->sccpcb, index) != SCCPCM_NO_INDEX) {
        /*
         * Are we registered yet?
         */
        if (sccpcm_is_cmcb_registered(reccb->cmcbs[index]) == 1) {
            sccprec_stop_timer(reccb, reccb->misc_timer);

            SCCP_DBG((sccprec_debug, SCCPREC_DBG_LEVEL_INFO2,
                     "%s %-2d:%-2d: %-20s: %s: registered\n",
                     SCCPREC_ID, reccb->sccpcb->id, reccb->id,
                     event->function_name,
                     sccpcm_print_cm(reccb->cmcbs[index])));

            reccb->cm_list_iterations = 0;

            /*
             * Found a primary, look for secondary.
             */
            sccprec_push_simple(reccb->sccpcb, SCCPREC_E_SEC_CHECK);
            
            return (0);
        }
    }

    /*
     * Let's try another one.
     */
    sccprec_get_next_cm_index(reccb);

    for (;;) {
        /*
         * Have we made a complete pass?
         */
        if (reccb->start_index == reccb->cm_index) {
            /*
             * No CM found - wait for next time and try again.
             */
            SCCP_DBG((sccprec_debug, SCCPREC_DBG_LEVEL_INFO2,
                     "%s %-2d:%-2d: %-20s: No CM found to make primary\n",
                     SCCPREC_ID, reccb->sccpcb->id, reccb->id,
                     event->function_name));
                     
            reccb->pri_retries++;

            /*
             * See if we have gone through the list too many times.
             */
            sccprec_count_list_iterations(reccb, event->function_name);

			/*
			 * Let's wait a bit and try again.
			 */
            sccprec_push_simple(reccb->sccpcb, SCCPREC_E_DONE);    

            return (0);
        }

        /*
         * Validate the index.
         */
        if (sccpcm_get_cm_by_index(reccb->sccpcb, reccb->cm_index) !=
            SCCPCM_NO_INDEX) {

            /*
             * Are we connected yet? Include token in the search.
             */
            if (sccpcm_is_cmcb_connected(reccb->cmcbs[reccb->cm_index]) == 1) {
                /*
                 * Got one that is connected, let's try and register with it.
                 */
                sccprec_register_cm(reccb, reccb->cm_index,
                                    event->function_name);

                /*
                 * Wait for cm to respond.
                 */
                return (0);
            }

            if (sccpcm_is_cmcb_idle(reccb->cmcbs[reccb->cm_index]) == 1) {
                /*
                 * Tell the CM to try and connect.
                 */
                sccprec_connect_cm(reccb, reccb->cm_index,
                                   event->function_name);

                /*
                 * Wait around for the connect_res.
                 */

                return (0);                 
            } 
        }

        /*
        * Let's try another one.
        */
        sccprec_get_next_cm_index(reccb);
    }
}

/*
 * FUNCTION:    sccprec_sem_sec_check_find_sec
 *
 * DESCRIPTION: Ensure that a secondary is defined and if not initiate the 
 *              proper procedures to find one.
 *
 * PARAMETERS:
 *     event:   event control block
 *
 * RETURNS:     0 for success, otherwise 1.
 *
 * NOTES:
 *
 * ============================================================================
 * S_SEC_CHECK               E_FIND_SEC                    S_FIND_SEC
 * ----------------------------------------------------------------------------
 */
static int sccprec_sem_sec_check_find_sec (sem_event_t *event)
{
    sccprec_reccb_t *reccb = (sccprec_reccb_t *)(event->cb);

    /*
     * Reset the working index.
     */
    reccb->cm_index = 0;

    /*
     * Look through all the CMs to find one that is idle
     * so we can try to connect with it. And of course, start
     * at the top since the list is descending priority.
     */
    for (;;) {
        if (sccpcm_is_cmcb_idle(reccb->cmcbs[reccb->cm_index]) == 1) {
            sccprec_connect_cm(reccb, reccb->cm_index,
                               event->function_name);

            /*
             * Save starting index so that we only look through 
             * the list one time.
             */
            reccb->start_index = reccb->cm_index;                                 

            /*
             * Wait for the ack.
             */
            return (0);                 

        }

        /*
         * No luck, move on to the next.
         */
        sccprec_get_next_cm_index(reccb);

        /*
         * Are we at the end of the list?
         */
        if (reccb->cm_index == 0) {
            SCCP_DBG((sccprec_debug, SCCPREC_DBG_LEVEL_INFO2,
                     "%s %-2d:%-2d: %-20s: no CMs found to connect to\n",
                     SCCPREC_ID, reccb->sccpcb->id, reccb->id,
                     event->function_name));

            break;
        }
    }

    /*
     * Could not find secondary, wait for timeout.
     */
    sccprec_start_timer(reccb, reccb->misc_timer,
                        SCCPREC_TIMER_DEVICE_POLL,
                        reccb->device_poll_to);

    return (0);
}

/*
 * FUNCTION:    sccprec_sem_pri_optimize
 *
 * DESCRIPTION: Ensure that the current primary is the highest priority and  
 *              if not, initiate the proper procedures to find a better one.
 *
 * PARAMETERS:
 *     event:   event control block
 *
 * RETURNS:     0 for success, otherwise 1.
 *
 * NOTES:
 *
 * ============================================================================
 * S_SEC_CHECK               E_PRI_OPTIMIZE                S_PRI_OPTIMIZE
 * ----------------------------------------------------------------------------
 * S_FIND_SEC                E_PRI_OPTIMIZE                S_PRI_OPTIMIZE
 * ----------------------------------------------------------------------------
 */
static int sccprec_sem_pri_optimize (sem_event_t *event) 
{
    sccprec_reccb_t *reccb = (sccprec_reccb_t *)(event->cb);
    int             pri_index;

    /*
     * Check if we have a primary.
     */
    pri_index = sccpcm_get_primary_index(reccb->sccpcb);
    if (pri_index != SCCPCM_NO_INDEX) {
        /*
         * Are we already optimized? Remember the CM list is in
         * descending order so index 0 will be optimal.
         */
        if (pri_index == 0) {
            SCCP_DBG((sccprec_debug, SCCPREC_DBG_LEVEL_INFO2,
                     "%s %-2d:%-2d: %-20s: %s: optimizing primary, found\n",
                     SCCPREC_ID, reccb->sccpcb->id, reccb->id,
                     event->function_name,
                     sccpcm_print_cm(reccb->cmcbs[pri_index])));

            /*
             * Yes, check for the optimal secondary.
             */
            sccprec_push_simple(reccb->sccpcb, SCCPREC_E_SEC_OPTIMIZE);

            return (0);
        }

        /*
         * Let's see if we can get a better primary, starting
         * with the optimal choice of 0. We will check all cms with a higher 
         * priority than our current primary.
         */
        reccb->start_index = SCCPREC_PRIMARY_INDEX;
       
        for (reccb->cm_index = SCCPREC_PRIMARY_INDEX; 
             ((reccb->cm_index < pri_index) && (reccb->cm_index < reccb->max_cm_index)); 
             reccb->cm_index++) {

            /*
             * Validate the index.
             */
            if (sccpcm_get_cm_by_index(reccb->sccpcb,reccb->cm_index) !=
                SCCPCM_NO_INDEX) {

                /*
                 * Find an available cm. Include token in the search.
                 */
                if ((sccpcm_is_cmcb_idle(reccb->cmcbs[reccb->cm_index]) == 1) ||
                    (sccpcm_is_cmcb_connected(reccb->cmcbs[reccb->cm_index]) == 1)) {
                    
                    /*
                     * Make sure we have no active streams on the current
                     * primary.
                     */
                    if (SCCP_ALL_STREAMS_IDLE(reccb) == 1) {
                        /*
                         * Let's try and switch.
                         */
                        SCCP_STRNCPY(sccprec_buf,
                                     sccpcm_print_cm(reccb->cmcbs[pri_index]),
                                     sizeof(sccprec_buf)) ;
                        SCCP_DBG((sccprec_debug, SCCPREC_DBG_LEVEL_INFO2,
                                 "%s %-2d:%-2d: %-20s: %s: no active streams, "
                                 "trying to switch to %s\n",
                                 SCCPREC_ID, reccb->sccpcb->id, reccb->id,
                                 event->function_name,
                                 sccprec_buf,
                                 sccpcm_print_cm(reccb->cmcbs[reccb->cm_index])));
   
                        sccprec_push_simple(reccb->sccpcb,
                                            SCCPREC_E_PRI_OPTIMIZE);

                        return (0);
                    }
                    else {
                        /*
                         * Active streams - can't switch. Move to the next
                         * state.
                         */
                        SCCP_STRNCPY(sccprec_buf,
                                     sccpcm_print_cm(reccb->cmcbs[pri_index]),
                                     sizeof(sccprec_buf)) ;

                        SCCP_DBG((sccprec_debug, SCCPREC_DBG_LEVEL_INFO2,
                                 "%s %-2d:%-2d: %-20s: %s: active streams, "
                                 "unable to switch to %s\n",
                                 SCCPREC_ID, reccb->sccpcb->id, reccb->id,
                                 event->function_name,
                                 sccprec_buf,
                                 sccpcm_print_cm(reccb->cmcbs[reccb->cm_index])));
                    }
                } /* if ((sccpcm_is_cm_idle(reccb->cm_index, reccb->sccpcb) == 1) || */
            } /* if (sccpcm_get_cm_by_index(reccb->sccpcb,reccb->cm_index) != */
        } /* for (; ((reccb->cm_index < pri_index) && */

        SCCP_DBG((sccprec_debug, SCCPREC_DBG_LEVEL_INFO2,
                 "%s %-2d:%-2d: %-20s: No connection to a better "
                 "primary, skipping\n",
                 SCCPREC_ID, reccb->sccpcb->id, reccb->id,
                 event->function_name));
    } /* if (pri_index != SCCPCM_NO_INDEX) { */
    else {
        SCCP_DBG((sccprec_debug, SCCPREC_DBG_LEVEL_INFO2,
                 "%s %-2d:%-2d: %-20s: No primary, skipping\n",
                 SCCPREC_ID, reccb->sccpcb->id, reccb->id,
                 event->function_name));
    }

    sccprec_push_simple(reccb->sccpcb, SCCPREC_E_SEC_OPTIMIZE);

    return (0);
}

/*
 * FUNCTION:    sccprec_sem_find_sec_timeout
 *
 * DESCRIPTION: Ensure that a secondary is defined and if not initiate the 
 *              proper procedures to find one.
 *
 * PARAMETERS:
 *     event:   event control block
 *
 * RETURNS:     0 for success, otherwise 1.
 *
 * NOTES:
 *
 * ============================================================================
 * S_FIND_SEC                E_TIMEOUT                     S_FIND_SEC
 * ----------------------------------------------------------------------------
 */
static int sccprec_sem_find_sec_timeout (sem_event_t *event) 
{
    sccprec_reccb_t *reccb = (sccprec_reccb_t *)(event->cb);
    int             index  = reccb->cm_index;
    
	/*
	 * Check if this was a real timeout to add debug. There are other functions
	 * which will call this function.
	 */
    if (event->event_id == SCCPREC_E_TIMEOUT) {
		sccprec_msg_timeout_t *msg = (sccprec_msg_timeout_t *)(event->data);

        SCCP_DBG((sccprec_debug, SCCPREC_DBG_LEVEL_SEM,
                 "%s %-2d:%-2d: %-20s: %s\n",
                 SCCPREC_ID, reccb->sccpcb->id, reccb->id,
                 event->function_name,
                 sccprec_timer_name(msg->event)));
    }

    /*
     * Make sure we have a primary.
     */
    if (sccpcm_get_primary_index(reccb->sccpcb) == SCCPCM_NO_INDEX) {
        /*
         * No primary. Go back and try to find a primary.
         */
        sccprec_push_simple(reccb->sccpcb, SCCPREC_E_PRI_OPTIMIZE);
        
        return (0);
    }

    /*
     * Validate the index.
     */
    if (sccpcm_get_cm_by_index(reccb->sccpcb, index) != SCCPCM_NO_INDEX) {
        /*
         * Are we connected yet? Include token.
         */
        if (sccpcm_is_cmcb_connected(reccb->cmcbs[index])) {
            SCCP_DBG((sccprec_debug, SCCPREC_DBG_LEVEL_INFO2,
                     "%s %-2d:%-2d: %-20s: %s: connected\n",
                     SCCPREC_ID, reccb->sccpcb->id, reccb->id,
                     event->function_name,
                     sccpcm_print_cm(reccb->cmcbs[index])));

            sccprec_stop_timer(reccb, reccb->misc_timer);

            /*
             * Good to go. Let's make sure we have the optimal
             * primary and secondary.
             */
            sccprec_push_simple(reccb->sccpcb, SCCPREC_E_PRI_OPTIMIZE);
            
            return (0);
        }
    }

    /*
     * Let's try another one.
     */
    sccprec_get_next_cm_index(reccb);

    for (;;) {
        /*
         * Validate the index.
         */
        if (sccpcm_get_cm_by_index(reccb->sccpcb,reccb->cm_index) !=
            SCCPCM_NO_INDEX) {

			/*
			 * Need an idle cm so that we can start a connection.
			 */
            if (sccpcm_is_cmcb_idle(reccb->cmcbs[reccb->cm_index]) == 1) {
                /*
                 * Tell the CM to try and connect.
                 */
                sccprec_connect_cm(reccb, reccb->cm_index,
                                   event->function_name);

                /*
                 * Wait around for the connect_res.
                 */

                return (0);                 
            } 
        }

        /*
        * Let's try another one.
        */
        sccprec_get_next_cm_index(reccb);

        /*
         * Have we made a complete pass?
         */
        if (reccb->start_index == reccb->cm_index) {
            /*
             * No CM found - wait for next time and try again.
             */
            SCCP_DBG((sccprec_debug, SCCPREC_DBG_LEVEL_INFO2, 
                     "%s %-2d:%-2d: %-20s: No CM found to make secondary\n",
                     SCCPREC_ID, reccb->sccpcb->id, reccb->id,
                     event->function_name));
                     
            sccprec_push_simple(reccb->sccpcb, SCCPREC_E_DONE);    

            return (0);
        }
    }
}

/*
 * FUNCTION:    sccprec_sem_find_sec_dev_notify
 *
 * DESCRIPTION: Something has changed in a cm. Ensure a secondary is defined
 *              and if not initiate procedures to find one.
 *
 * PARAMETERS:
 *     event:   event control block
 *
 * RETURNS:     0 for success, otherwise 1.
 *
 * NOTES:
 *
 * ============================================================================
 * S_FIND_SEC                E_DEV_NOTIFY                  S_FIND_SEC
 * ----------------------------------------------------------------------------
 */
static int sccprec_sem_find_sec_dev_notify (sem_event_t *event) 
{
    sccprec_reccb_t *reccb = (sccprec_reccb_t *)(event->cb);
    int             index  = reccb->cm_index;

    /*
     * Validate the index.
     */
    if (sccpcm_get_cm_by_index(reccb->sccpcb, index) == SCCPCM_NO_INDEX) {
        sccprec_stop_timer(reccb, reccb->misc_timer);

        SCCP_DBG((sccprec_debug, SCCPREC_DBG_LEVEL_INFO2, 
                 "%s %-2d:%-2d: %-20s: CM[%d]: NULL\n",
                 SCCPREC_ID, reccb->sccpcb->id, reccb->id,
                 event->function_name,
                 index));

        sccprec_push_simple(reccb->sccpcb, SCCPREC_E_PRI_OPTIMIZE);    

        return (0);
    }

    /*
     * Are we connected yet? If so, then we have our secondary.
     * Include token in the search.
     */
    if (sccpcm_is_cmcb_connected(reccb->cmcbs[index]) == 1) {
        sccprec_stop_timer(reccb, reccb->misc_timer);

        SCCP_DBG((sccprec_debug, SCCPREC_DBG_LEVEL_INFO2, 
                 "%s %-2d:%-2d: %-20s: %s: connected\n",
                 SCCPREC_ID, reccb->sccpcb->id, reccb->id,
                 event->function_name,
                 sccpcm_print_cm(reccb->cmcbs[index])));

        sccprec_push_simple(reccb->sccpcb, SCCPREC_E_PRI_OPTIMIZE);

        return (0);
    }

    /*
     * If we are IDLE or LOCKOUT then get next.
     */
    if ((sccpcm_is_cmcb_idle(reccb->cmcbs[index]) == 1) ||
        (sccpcm_is_cmcb_lockout(reccb->cmcbs[index]) == 1)) {
        sccprec_stop_timer(reccb, reccb->misc_timer);

        SCCP_DBG((sccprec_debug, SCCPREC_DBG_LEVEL_INFO2, 
                 "%s %-2d:%-2d: %-20s: %s: connect failed, "
                 "trying to find next\n",
                 SCCPREC_ID, reccb->sccpcb->id, reccb->id,
                 event->function_name,
                 sccpcm_print_cm(reccb->cmcbs[index])));

        /*
         * Try next.
         */
        event->function_name = 
            sccprec_function_name(SCCPREC_SEM_FIND_SEC_TIMEOUT);

        sccprec_sem_find_sec_timeout(event);

        return (0);
    }

    /*
     * Just wait, another connection must have changed.
     */
    return (0);
}

/*
 * FUNCTION:    sccprec_sem_sec_optimize
 *
 * DESCRIPTION: Ensure that the current secondary is the highest priority and  
 *              if not, initiate the proper procedures to find a better one.
 *
 * PARAMETERS:
 *     event:   event control block
 *
 * RETURNS:     0 for success, otherwise 1.
 *
 * NOTES:
 *
 * ============================================================================
 * S_PRI_OPTIMIZE            E_SEC_OPTIMIZE                S_SEC_OPTIMIZE
 * ----------------------------------------------------------------------------
 * S_MAKE_PRI_CONNECTION     E_SEC_OPTIMIZE                S_SEC_OPTIMIZE
 * ----------------------------------------------------------------------------
 * S_TOKEN_REQ               E_SEC_OPTIMIZE                S_SEC_OPTIMIZE
 * ----------------------------------------------------------------------------
 * S_PRI_UNREG_CM            E_SEC_OPTIMIZE                S_SEC_OPTIMIZE
 * ----------------------------------------------------------------------------
 * S_PRI_REG_CM              E_SEC_OPTIMIZE                S_SEC_OPTIMIZE
 * ----------------------------------------------------------------------------
 */
static int sccprec_sem_sec_optimize (sem_event_t *event) 
{
    sccprec_reccb_t *reccb = (sccprec_reccb_t *)(event->cb);
    int             sec_index;

    /*
     * Make sure we have a primary.
     */
    if (sccpcm_get_primary_index(reccb->sccpcb) == SCCPCM_NO_INDEX) {
        SCCP_DBG((sccprec_debug, SCCPREC_DBG_LEVEL_INFO2,
                 "%s %-2d:%-2d: %-20s: No primary, skipping\n",
                 SCCPREC_ID, reccb->sccpcb->id, reccb->id,
                 event->function_name));

        /*
         * No primary. The DONE event will restart the 
         * searching process.
         */
        sccprec_push_simple(reccb->sccpcb, SCCPREC_E_DONE);

        return (0);
    }

    /*
     * Make sure we have a secondary.
     */
    sec_index = sccpcm_get_secondary_index(reccb->sccpcb);
    if (sec_index == SCCPCM_NO_INDEX) {
        SCCP_DBG((sccprec_debug, SCCPREC_DBG_LEVEL_INFO2, 
                 "%s %-2d:%-2d: %-20s: No secondary, skipping\n",
                 SCCPREC_ID, reccb->sccpcb->id, reccb->id,
                 event->function_name));

        sccprec_push_simple(reccb->sccpcb, SCCPREC_E_DONE);
        
        return (0);
    }

    /*
     * Make sure we can allocate a secondary. Gotta have at least
     * two CMs defined - one for the primary, one for secondary.
     */
    if (sccpcm_get_cm_count(reccb->sccpcb) < 2) {
        /*
         * Not enough CMs defined to have a secondary.
         * Goto next state.
         */
        SCCP_DBG((sccprec_debug, SCCPREC_DBG_LEVEL_INFO2,
                 "%s %-2d:%-2d: %-20s: Not enough CMs to have secondary\n",
                 SCCPREC_ID, reccb->sccpcb->id, reccb->id,
                 event->function_name));

        /*
         * No biggie, we have a primary. The done event will
         * start the polling and maybe we will find a secondary.
         */
        sccprec_push_simple(reccb->sccpcb, SCCPREC_E_DONE);
        
        return (0);
    }

    /*
     * Check if our best/optimal secondary is secondary/connected.
     * Remember we want the highest priority CMs to be primary and
     * secondary.
     */
    reccb->cm_index    = SCCPREC_SECONDARY_INDEX;
    reccb->start_index = SCCPREC_SECONDARY_INDEX;

    /*
     * Quick check, index 1 would be the optimal secondary. Include
     * token in the search.
     */
    if (sccpcm_is_cmcb_connected(reccb->cmcbs[reccb->cm_index]) == 1) {
        SCCP_DBG((sccprec_debug, SCCPREC_DBG_LEVEL_INFO2,
                 "%s %-2d:%-2d: %-20s: %s: optimizing secondary, found\n",
                 SCCPREC_ID, reccb->sccpcb->id, reccb->id,
                 event->function_name,
                 sccpcm_print_cm(reccb->cmcbs[reccb->cm_index])));

        /*
         * We have an optimal/connected secondary.
         * We can finish up and wait for the next poll to restart
         * everything.
         */
        sccprec_push_simple(reccb->sccpcb, SCCPREC_E_DONE);
     
        return (0);
    }

    /*
     * Check if we can allocate secondary/connection.
     */
    if (sccpcm_is_cmcb_idle(reccb->cmcbs[reccb->cm_index]) == 1) {
        SCCP_DBG((sccprec_debug, SCCPREC_DBG_LEVEL_INFO2,
                 "%s %-2d:%-2d: %-20s: %s: optimizing secondary, trying\n",
                 SCCPREC_ID, reccb->sccpcb->id, reccb->id,
                 event->function_name,
                 sccpcm_print_cm(reccb->cmcbs[reccb->cm_index])));

        sccprec_push_simple(reccb->sccpcb, SCCPREC_E_SEC_OPTIMIZE);

        return (0);
    }

    /*
     * Start at the top of the list and work our way down to the
     * connected one. This will also help the primary optimize
     * if we connect to the first one.
     */
    reccb->start_index = SCCPREC_PRIMARY_INDEX;

    for (reccb->cm_index    = SCCPREC_PRIMARY_INDEX;
         ((reccb->cm_index < sec_index) && (reccb->cm_index < reccb->max_cm_index));
         reccb->cm_index++) {
        /*
         * Validate the index.
         */
        if (sccpcm_get_cm_by_index(reccb->sccpcb, reccb->cm_index) ==
            SCCPCM_NO_INDEX) {

            continue;
        }

        /*
         * Include connected and token.
         */
        if (sccpcm_is_cmcb_connected(reccb->cmcbs[reccb->cm_index]) == 1) {
            SCCP_DBG((sccprec_debug, SCCPREC_DBG_LEVEL_INFO2,
                     "%s %-2d:%-2d: %-20s: %s: optimizing secondary, found\n",
                     SCCPREC_ID, reccb->sccpcb->id, reccb->id,
                     event->function_name,
                     sccpcm_print_cm(reccb->cmcbs[reccb->cm_index])));

            sccprec_push_simple(reccb->sccpcb, SCCPREC_E_DONE);

            return (0);
        }

        /*
         * Check if we can allocate secondary/connection.
         */
        if (sccpcm_is_cmcb_idle(reccb->cmcbs[reccb->cm_index]) == 1) {
            SCCP_DBG((sccprec_debug, SCCPREC_DBG_LEVEL_INFO2,
                     "%s %-2d:%-2d: %-20s: %s: optimizing secondary, trying\n",
                     SCCPREC_ID, reccb->sccpcb->id, reccb->id,
                     event->function_name,
                     sccpcm_print_cm(reccb->cmcbs[reccb->cm_index])));

            sccprec_push_simple(reccb->sccpcb, SCCPREC_E_SEC_OPTIMIZE);

            return (0);
        }
    }
    
    /*
     * There must only be one CM defined or none available.
     */
    SCCP_DBG((sccprec_debug, SCCPREC_DBG_LEVEL_INFO2,
             "%s %-2d:%-2d: %-20s: No better secondary found, skipping\n",
             SCCPREC_ID, reccb->sccpcb->id, reccb->id,
             event->function_name));

    sccprec_push_simple(reccb->sccpcb, SCCPREC_E_DONE);

    return (0);
}

/*
 * FUNCTION:    sccprec_sem_pri_opt_pri_optimize
 *
 * DESCRIPTION: Ensure that the current primary is the highest priority and  
 *              if not, initiate the proper procedures to find a better one.
 *
 * PARAMETERS:
 *     event:   event control block
 *
 * RETURNS:     0 for success, otherwise 1.
 *
 * NOTES:
 *
 * ============================================================================
 * S_PRI_OPTIMIZE            E_PRI_OPTIMIZE                S_MAKE_PRI_CON
 * ----------------------------------------------------------------------------
 * S_MAKE_PRI_CON            E_PRI_OPTIMIZE                S_MAKE_PRI_CON
 * ----------------------------------------------------------------------------
 */
static int sccprec_sem_pri_opt_pri_optimize (sem_event_t *event) 
{
    sccprec_reccb_t *reccb = (sccprec_reccb_t *)(event->cb);

    /*
     * Make sure we have a primary.
     */
    if (sccpcm_get_primary_index(reccb->sccpcb) == SCCPCM_NO_INDEX) {
        SCCP_DBG((sccprec_debug, SCCPREC_DBG_LEVEL_INFO2,
                 "%s %-2d:%-2d: %-20s: No primary, skipping\n",
                 SCCPREC_ID, reccb->sccpcb->id, reccb->id,
                 event->function_name));

        /*
         * No primary. Let the secondary optimize code handle 
         * finding the primary.
         */
        sccprec_push_simple(reccb->sccpcb, SCCPREC_E_SEC_OPTIMIZE);

        return (0);
    }

    /*
     * Try to connect to higher priority secondary.
     */

    /*
     * Validate the index.
     */
    if (sccpcm_get_cm_by_index(reccb->sccpcb, reccb->cm_index) ==
        SCCPCM_NO_INDEX) {
        SCCP_DBG((sccprec_debug, SCCPREC_DBG_LEVEL_INFO2,
                 "%s %-2d:%-2d: %-20s: CM[%d]: NULL, skipping\n",
                 SCCPREC_ID, reccb->sccpcb->id, reccb->id,
                 event->function_name,
                 reccb->cm_index));

        sccprec_push_simple(reccb->sccpcb, SCCPREC_E_SEC_OPTIMIZE);    

        return (0);
    }

    /*
     * Check if the secondary is already connected. Don't look at those in
     * the TOKEN state, since that is the state we will be entering if
     * we try to switch to this cm.
     */
    if (sccpcm_is_cmcb_connected2(reccb->cmcbs[reccb->cm_index]) == 1) {
        /*
         * Got a higher priority secondary so we need to fallback to this
         * one. We need to grab a token before we can fallback.
         */
        SCCP_DBG((sccprec_debug, SCCPREC_DBG_LEVEL_INFO2, 
                 "%s %-2d:%-2d: %-20s: %s: connected, requesting token\n",
                 SCCPREC_ID, reccb->sccpcb->id, reccb->id,
                 event->function_name,
                 sccpcm_print_cm(reccb->cmcbs[reccb->cm_index])));

        sccprec_push_simple(reccb->sccpcb, SCCPREC_E_TOKEN_REQ);    

        return (0);
    }

    /*
     * If this secondary connection is idle...
     */
    if (sccpcm_is_cmcb_idle(reccb->cmcbs[reccb->cm_index]) == 1) {
        /*
         * Tell the cm to try and connect.
         */
        sccprec_connect_cm(reccb, reccb->cm_index,
                           event->function_name);

        return (0);
    }

    sccprec_push_simple(reccb->sccpcb, SCCPREC_E_SEC_OPTIMIZE);    

    return (0);
}

/*
 * FUNCTION:    sccprec_sem_make_pri_con_token_req
 *
 * DESCRIPTION: Request a token becaue fallback procedures have been
 *              initiated.
 *
 * PARAMETERS:
 *     event:   event control block
 *
 * RETURNS:     0 for success, otherwise 1.
 *
 * NOTES:
 *
 * ============================================================================
 * S_MAKE_PRI_CON            E_TOKEN_REQ                   S_TOKEN_REQ
 * ----------------------------------------------------------------------------
 */
static int sccprec_sem_make_pri_con_token_req (sem_event_t *event) 
{
    sccprec_reccb_t *reccb = (sccprec_reccb_t *)(event->cb);

    /*
    * Ask the CM to grab a token.
     */
    sccpcm_push_token_req(reccb->sccpcb, SCCPCM_E_SEND_TOKEN,
                          reccb->cm_index);

#if 1
    /*
     * Start a timer to supervise the token request. Use the secondary
     * timeout because this is a secondary connection that is requesting
     * the token.
     */
    sccprec_start_timer(reccb, reccb->misc_timer,
                        SCCPREC_TIMER_WF_TOKEN,
                        reccb->keepalive_t2 * 
                        SCCPREC_KA_TO_BEFORE_NO_TOKEN_REG +
                        SCCPREC_FUDGE_TO);
#endif
    /*
     * Wait for notification.
     */
    return (0);
}

/*
 * FUNCTION:    sccprec_sem_make_pri_con_unreg_req
 *
 * DESCRIPTION: Begin unregistration procedures.
 *
 * PARAMETERS:
 *     event:   event control block
 *
 * RETURNS:     0 for success, otherwise 1.
 *
 * NOTES:
 *
 * ============================================================================
 * S_MAKE_PRI_CON            E_UNREG_REQ                   S_PRI_UNREG_CM
 * ----------------------------------------------------------------------------
 */
static int sccprec_sem_make_pri_con_unreg_req (sem_event_t *event) 
{
    sccprec_reccb_t *reccb = (sccprec_reccb_t *)(event->cb);
    int             wait;

    /*
     * Save the connection that we just made.
     */
    reccb->start_index = reccb->cm_index;

    /*
     * Save the current primary.
     */
    reccb->cm_index = sccpcm_get_primary_index(reccb->sccpcb);

    /*
     * Make sure the primary is still valid.
     */
    if (reccb->cm_index == SCCPCM_NO_INDEX) {
        SCCP_DBG((sccprec_debug, SCCPREC_DBG_LEVEL_INFO2, 
                 "%s %-2d:%-2d: %-20s: CM[%d]: no primary, skipping\n",
                 SCCPREC_ID, reccb->sccpcb->id, reccb->id,
                 event->function_name,
                 reccb->cm_index));

        /*
         * The secondary optimize will find a way to get a primary.
         */
        sccprec_push_simple(reccb->sccpcb, SCCPREC_E_SEC_OPTIMIZE);    

        return (0);
    }
    
    /*
     * Use a random wait to help alleviate network overlaod. It is possible
     * that a link just came up and you don't want all the clients to start
     * banging away on the CMs at once.
     */
    wait = sccprec_get_wait_before_unregister(reccb);
    if (wait == 0) {
        sccprec_unregister_cm(reccb, reccb->cm_index, event->function_name);
    }
    else {
        /*
         * Start a timer to wait.
         */
        sccprec_start_timer(reccb, reccb->misc_timer,
                            SCCPREC_TIMER_RETRY_UNREGISTER_TO,
                            wait);

    }

    return (0);
}

/*
 * FUNCTION:    sccprec_sem_make_pri_con_dev_notify
 *
 * DESCRIPTION: This function ensures that we have the optimal primary. If
 *              not, then it will begin fallback procedures to find the 
 *              optimal primary.
 *
 * PARAMETERS:
 *     event:   event control block
 *
 * RETURNS:     0 for success, otherwise 1.
 *
 * NOTES:
 *
 * ============================================================================
 * S_MAKE_PRI_CON            E_DEV_NOTIFY                  S_MAKE_PRI_CON
 * ----------------------------------------------------------------------------
 */
static int sccprec_sem_make_pri_con_dev_notify (sem_event_t *event) 
{
    sccprec_reccb_t *reccb = (sccprec_reccb_t *)(event->cb);
    int             index  = reccb->cm_index;
    int             pri_index;

    /*
     * Make sure we have a primary.
     */
    pri_index = sccpcm_get_primary_index(reccb->sccpcb);
    if (pri_index == SCCPCM_NO_INDEX) {
        sccprec_stop_timer(reccb, reccb->misc_timer);

        SCCP_DBG((sccprec_debug, SCCPREC_DBG_LEVEL_INFO2,
                 "%s %-2d:%-2d: %-20s: No primary, skipping\n",
                 SCCPREC_ID, reccb->sccpcb->id, reccb->id,
                 event->function_name));

        /*
         * No primary. Let the secondary optimize code handle 
         * finding the primary.
         */
        sccprec_push_simple(reccb->sccpcb, SCCPREC_E_SEC_OPTIMIZE);

        return (0);
    }

    /*
     * Validate the index.
     */
    if (sccpcm_get_cm_by_index(reccb->sccpcb, index) == SCCPCM_NO_INDEX) {
        sccprec_stop_timer(reccb, reccb->misc_timer);

        SCCP_DBG((sccprec_debug, SCCPREC_DBG_LEVEL_INFO2,
                 "%s %-2d:%-2d: %-20s: CM[%d]: NULL, skipping\n",
                 SCCPREC_ID, reccb->sccpcb->id, reccb->id,
                 event->function_name,
                 index));

        sccprec_push_simple(reccb->sccpcb, SCCPREC_E_SEC_OPTIMIZE);    

        return (0);
    }

    /*
     * Unregister from the current primary if we get a token from the 
     * new higher priority cm.
     */
    if (sccpcm_is_cmcb_token(reccb->cmcbs[index]) == 1) {
        sccprec_stop_timer(reccb, reccb->misc_timer);

        SCCP_DBG((sccprec_debug, SCCPREC_DBG_LEVEL_INFO2,
                 "%s %-2d:%-2d: %-20s: %s: has token\n",
                 SCCPREC_ID, reccb->sccpcb->id, reccb->id,
                 event->function_name,
                 sccpcm_print_cm(reccb->cmcbs[index])));

        sccprec_push_simple(reccb->sccpcb, SCCPREC_E_UNREG_REQ);    

        return (0);
    }

    /*
     * Request a token if we are connected to the higher priority cm.
     */
    if (sccpcm_is_cmcb_connected(reccb->cmcbs[index]) == 1) {
        sccprec_stop_timer(reccb, reccb->misc_timer);

        SCCP_DBG((sccprec_debug, SCCPREC_DBG_LEVEL_INFO2, 
                 "%s %-2d:%-2d: %-20s: %s: connected, requesting token\n",
                 SCCPREC_ID, reccb->sccpcb->id, reccb->id,
                 event->function_name,
                 sccpcm_print_cm(reccb->cmcbs[index])));

        sccprec_push_simple(reccb->sccpcb, SCCPREC_E_TOKEN_REQ);    

        return (0);
    }

    /*
     * If we are IDLE or LOCKOUT then get next.
     */
    if ((sccpcm_is_cmcb_idle(reccb->cmcbs[index]) == 1) ||
        (sccpcm_is_cmcb_lockout(reccb->cmcbs[index]) == 1)) {
#if 0 /*sam why not? */
        sccprec_stop_timer(reccb, reccb->misc_timer);
#endif
        SCCP_DBG((sccprec_debug, SCCPREC_DBG_LEVEL_INFO2, 
                 "%s %-2d:%-2d: %-20s: %s: IDLE or LOCKOUT, skipping\n",
                 SCCPREC_ID, reccb->sccpcb->id, reccb->id,
                 event->function_name,
                 sccpcm_print_cm(reccb->cmcbs[index])));

        sccprec_get_next_cm_index(reccb);

        /*
         * Try to connect to the next best one and not past the 
         * current primary.
         */
        while ((reccb->cm_index < pri_index) &&
               (reccb->start_index != reccb->cm_index)) {
            /*
             * Validate the index.
             */
            if (sccpcm_get_cm_by_index(reccb->sccpcb, index) !=
                SCCPCM_NO_INDEX) {
                /*
                 * Do we have a token yet? If so, we can unregister the
                 * current primary.
                 */
                if (sccpcm_is_cmcb_token(reccb->cmcbs[reccb->cm_index]) == 1) {
                    sccprec_push_simple(reccb->sccpcb, SCCPREC_E_UNREG_REQ);    

                    return (0);
                }

                /*
                 * Are we connected yet? If so, we can try to request a token.
                 */
                if (sccpcm_is_cmcb_connected(reccb->cmcbs[reccb->cm_index]) == 1) {
                    sccprec_push_simple(reccb->sccpcb, SCCPREC_E_TOKEN_REQ);    

                    return (0);
                }

                /*
                 * Check if we can connect.
                 */
                SCCP_STRNCPY(sccprec_buf,
                             sccpcm_print_cm(reccb->cmcbs[pri_index]),
                             sizeof(sccprec_buf)) ;

                if (sccpcm_is_cmcb_idle(reccb->cmcbs[reccb->cm_index]) == 1) {
                    SCCP_DBG((sccprec_debug, SCCPREC_DBG_LEVEL_INFO2, 
                             "%s %-2d:%-2d: %-20s: %s: optimizing primary, to %s\n",
                             SCCPREC_ID, reccb->sccpcb->id, reccb->id, 
                             event->function_name,
                             sccprec_buf,
                             sccpcm_print_cm(reccb->cmcbs[reccb->cm_index])));

                    sccprec_push_simple(reccb->sccpcb,
                                        SCCPREC_E_PRI_OPTIMIZE);  

                    return (0);
                }
            } /* if (sccpcm_get_cm_by_index(reccb->sccpcb, index) != */

            sccprec_get_next_cm_index(reccb);
        } /* while ((reccb->cm_index < pri_index) && */

        /*
         * Goto to next state, unable to connect and no other
         * choices.
         */
        sccprec_push_simple(reccb->sccpcb,
                            SCCPREC_E_SEC_OPTIMIZE);  

        return (0);
    } /* if ((sccpcm_is_cm_idle(index, reccb->sccpcb) == 1) || */

    /*
     * Just wait, another connection must have changed.
     */
    return (0);
}

/*
 * FUNCTION:    sccprec_sem_make_pri_con_timeout
 *
 * DESCRIPTION: This function ensures that we have the optimal primary. If
 *              not, then it will begin fallback procedures to find the 
 *              optimal primary.
 *
 * PARAMETERS:
 *     event:   event control block
 *
 * RETURNS:     0 for success, otherwise 1.
 *
 * NOTES:
 *
 * ============================================================================
 * S_MAKE_PRI_CON            E_TIMEOUT                     S_MAKE_PRI_CON
 * ----------------------------------------------------------------------------
 */
static int sccprec_sem_make_pri_con_timeout (sem_event_t *event) 
{
    sccprec_reccb_t *reccb = (sccprec_reccb_t *)(event->cb);
    int             index  = reccb->cm_index;
    int             pri_index;
    sccprec_msg_timeout_t *msg = (sccprec_msg_timeout_t *)(event->data);

    SCCP_DBG((sccprec_debug, SCCPREC_DBG_LEVEL_SEM,
             "%s %-2d:%-2d: %-20s: %s\n",
             SCCPREC_ID, reccb->sccpcb->id, reccb->id, event->function_name,
             sccprec_timer_name(msg->event)));

    /*
     * Make sure we have a primary.
     */
    pri_index = sccpcm_get_primary_index(reccb->sccpcb);
    if (pri_index == SCCPCM_NO_INDEX) {
        sccprec_stop_timer(reccb, reccb->misc_timer);

        SCCP_DBG((sccprec_debug, SCCPREC_DBG_LEVEL_INFO2,
                 "%s %-2d:%-2d: %-20s: No primary, skipping\n",
                 SCCPREC_ID, reccb->sccpcb->id, reccb->id,
                 event->function_name));

        /*
         * No primary. Let the secondary optimize code handle 
         * finding the primary.
         */
        sccprec_push_simple(reccb->sccpcb, SCCPREC_E_SEC_OPTIMIZE);

        return (0);
    }

    /*
     * Validate the index.
     */
    if (sccpcm_get_cm_by_index(reccb->sccpcb, index) == SCCPCM_NO_INDEX) {
        sccprec_stop_timer(reccb, reccb->misc_timer);

        SCCP_DBG((sccprec_debug, SCCPREC_DBG_LEVEL_INFO2,
                 "%s %-2d:%-2d: %-20s: CM[%d]: NULL, skipping\n",
                 SCCPREC_ID, reccb->sccpcb->id, reccb->id,
                 event->function_name,
                 index));

        sccprec_push_simple(reccb->sccpcb, SCCPREC_E_SEC_OPTIMIZE);    

        return (0);
    }

    /*
     * We can unregister with the current primary if we got a token
     * from this higher priority cm.
     */
    if (sccpcm_is_cmcb_token(reccb->cmcbs[index]) == 1) {
        sccprec_stop_timer(reccb, reccb->misc_timer);

        SCCP_DBG((sccprec_debug, SCCPREC_DBG_LEVEL_INFO2, 
                 "%s %-2d:%-2d: %-20s: %s: has token\n",
                 SCCPREC_ID, reccb->sccpcb->id, reccb->id,
                 event->function_name,  
                 sccpcm_print_cm(reccb->cmcbs[index])));

        sccprec_push_simple(reccb->sccpcb, SCCPREC_E_UNREG_REQ);    

        return (0);
    }

    /*
     * We can request a token if we are connected to this higher
     * priority cm. Once we get the token we can register with it.
     */
    if (sccpcm_is_cmcb_connected(reccb->cmcbs[index]) == 1) {
        SCCP_DBG((sccprec_debug, SCCPREC_DBG_LEVEL_INFO2,
                 "%s %-2d:%-2d: %-20s: %s: connected, requesting token\n",
                 SCCPREC_ID, reccb->sccpcb->id, reccb->id,
                 event->function_name,
                 sccpcm_print_cm(reccb->cmcbs[index])));

        sccprec_push_simple(reccb->sccpcb, SCCPREC_E_TOKEN_REQ);    

        return (0);
    }

    /*
     * Or we can just try for another since this one didn't work.
     */
    sccprec_get_next_cm_index(reccb);

    /*
     * Try to connect to the next best one and not past the 
     * current primary.
     */
    while ((reccb->cm_index < pri_index) &&
           (reccb->start_index != reccb->cm_index)) {
        /*
         * Validate the index.
         */
        if (sccpcm_get_cm_by_index(reccb->sccpcb, index) != SCCPCM_NO_INDEX) {
            /*
             * Do we have a token yet? If so, we can unregister the
             * current primary.
             */
            if (sccpcm_is_cmcb_token(reccb->cmcbs[reccb->cm_index]) == 1) {
                sccprec_push_simple(reccb->sccpcb, SCCPREC_E_UNREG_REQ);    

                return (0);
            }

            /*
             * Are we connected yet? If so, then we can request a token for 
             * this higher priority cm.
             */
            if (sccpcm_is_cmcb_connected(reccb->cmcbs[reccb->cm_index]) == 1) {
                sccprec_push_simple(reccb->sccpcb, SCCPREC_E_TOKEN_REQ);    

                return (0);
            }

            /*
             * Check if we can connect.
             */
            if (sccpcm_is_cmcb_idle(reccb->cmcbs[reccb->cm_index]) == 1) {
                    SCCP_STRNCPY(sccprec_buf,
                                 sccpcm_print_cm(reccb->cmcbs[pri_index]),
                                 sizeof(sccprec_buf)) ;

                    SCCP_DBG((sccprec_debug, SCCPREC_DBG_LEVEL_INFO2,
                             "%s %-2d:%-2d: %-20s: %s: optimizing primary, to %s\n",
                             SCCPREC_ID, reccb->sccpcb->id, reccb->id,
                             event->function_name,
                             sccprec_buf,
                             sccpcm_print_cm(reccb->cmcbs[reccb->cm_index])));

                sccprec_push_simple(reccb->sccpcb,
                                    SCCPREC_E_PRI_OPTIMIZE);  

                return (0);
            }
        } /* if (sccpcm_get_cm_by_index(reccb->sccpcb, index) != */

        sccprec_get_next_cm_index(reccb);
    } /* while ((reccb->cm_index < pri_index) && */

    /*
     * Goto to next state, unable to connect and no other
     * choices.
     */
    sccprec_push_simple(reccb->sccpcb, SCCPREC_E_SEC_OPTIMIZE);  

    return (0);
}

/*
 * FUNCTION:    sccprec_sem_token_req_timeout
 *
 * DESCRIPTION: This function is called when requesting a token from a higher
 *              priority cm fails. The function will begin unregistration from
 *              the current cm.
 *
 * PARAMETERS:
 *     event:   event control block
 *
 * RETURNS:     0 for success, otherwise 1.
 *
 * NOTES:
 *
 * ============================================================================
 * S_TOKEN_REQ               E_TIMEOUT                     S_TOKEN_REQ
 * ----------------------------------------------------------------------------
 */
static int sccprec_sem_token_req_timeout (sem_event_t *event) 
{
    sccprec_reccb_t *reccb = (sccprec_reccb_t *)(event->cb);
    int             index  = reccb->cm_index;
    int             pri_index;
    sccprec_msg_timeout_t *msg = (sccprec_msg_timeout_t *)(event->data);

    SCCP_DBG((sccprec_debug, SCCPREC_DBG_LEVEL_SEM,
             "%s %-2d:%-2d: %-20s: %s\n",
             SCCPREC_ID, reccb->sccpcb->id, reccb->id, event->function_name,
             sccprec_timer_name(msg->event)));

    /*
     * Make sure we have a primary.
     */
    pri_index = sccpcm_get_primary_index(reccb->sccpcb);
    if (pri_index == SCCPCM_NO_INDEX) {
        SCCP_DBG((sccprec_debug, SCCPREC_DBG_LEVEL_INFO2,
                 "%s %-2d:%-2d: %-20s: No primary, skipping\n",
                 SCCPREC_ID, reccb->sccpcb->id, reccb->id,
                 event->function_name));

        /*
         * No primary. Let the secondary optimize code handle 
         * finding the primary.
         */
        sccprec_push_simple(reccb->sccpcb, SCCPREC_E_SEC_OPTIMIZE);

        return (0);
    }

    /*
     * Validate the index.
     */
    if (sccpcm_get_cm_by_index(reccb->sccpcb, index) == SCCPCM_NO_INDEX) {
        SCCP_DBG((sccprec_debug, SCCPREC_DBG_LEVEL_INFO2,
                 "%s %-2d:%-2d: %-20s: Null CM[%d], skipping\n",
                 SCCPREC_ID, reccb->sccpcb->id, reccb->id,
                 event->function_name,
                 index));

        sccprec_push_simple(reccb->sccpcb, SCCPREC_E_SEC_OPTIMIZE);    

        return (0);
    }

    if (sccpcm_is_cmcb_token(reccb->cmcbs[index]) == 1) {
        SCCP_DBG((sccprec_debug, SCCPREC_DBG_LEVEL_INFO2,
                 "%s %-2d:%-2d: %-20s: %s: has token\n",
                 SCCPREC_ID, reccb->sccpcb->id, reccb->id,
                 event->function_name,
                 sccpcm_print_cm(reccb->cmcbs[index])));
    }
    else {
        SCCP_DBG((sccprec_debug, SCCPREC_DBG_LEVEL_INFO2,
                 "%s %-2d:%-2d: %-20s: %s: timeout waiting for token\n",
                 SCCPREC_ID, reccb->sccpcb->id, reccb->id,
                 event->function_name,
                 sccpcm_print_cm(reccb->cmcbs[index])));
    }

    /*
     * Whether we have a token or not, we go on and unregister
     * since the CM may not support tokens.
     */
    sccprec_set_tcp_close_cause(reccb->sccpcb,
                                SCCPMSG_ALARM_LAST_FAILBACK);

    sccprec_push_simple(reccb->sccpcb, SCCPREC_E_UNREG_REQ);    

    return (0);
}

/*
 * FUNCTION:    sccprec_sem_token_req_dev_notify
 *
 * DESCRIPTION: This function is called while waiting for a token_req response
 *              and something had happened on a cm. The function will check if
 *              the notification came from the cm requesting the token. If so,
 *              the primary cm will be unregistered. Otherwise, wait for the 
 *              notification from the correct cm.
 *
 * PARAMETERS:
 *     event:   event control block
 *
 * RETURNS:     0 for success, otherwise 1.
 *
 * NOTES:
 *
 * ============================================================================
 * S_TOKEN_REQ               E_TIMEOUT                     S_TOKEN_REQ
 * ----------------------------------------------------------------------------
 */
static int sccprec_sem_token_req_dev_notify (sem_event_t *event) 
{
    sccprec_reccb_t *reccb = (sccprec_reccb_t *)(event->cb);
    int             index  = reccb->cm_index;
    int             pri_index;

    /*
     * Make sure we have a primary.
     */
    pri_index = sccpcm_get_primary_index(reccb->sccpcb);
    if (pri_index == SCCPCM_NO_INDEX) {
        sccprec_stop_timer(reccb, reccb->misc_timer);
        
        SCCP_DBG((sccprec_debug, SCCPREC_DBG_LEVEL_INFO2,
                 "%s %-2d:%-2d: %-20s: No primary, skipping\n",
                 SCCPREC_ID, reccb->sccpcb->id, reccb->id,
                 event->function_name));

        /*
         * No primary. Let the secondary optimize code handle 
         * finding the primary.
         */
        sccprec_push_simple(reccb->sccpcb, SCCPREC_E_SEC_OPTIMIZE);

        return (0);
    }

    /*
     * Validate the index.
     */
    if (sccpcm_get_cm_by_index(reccb->sccpcb, index) == SCCPCM_NO_INDEX) {
        sccprec_stop_timer(reccb, reccb->misc_timer);

        SCCP_DBG((sccprec_debug, SCCPREC_DBG_LEVEL_INFO2,
                 "%s %-2d:%-2d: %-20s: Null CM[%d], skipping\n",
                 SCCPREC_ID, reccb->sccpcb->id, reccb->id,
                 event->function_name,
                 index));

        sccprec_push_simple(reccb->sccpcb, SCCPREC_E_SEC_OPTIMIZE);    

        return (0);
    }

    /*
     * Unregister the primary cm if we have a token from this higher priority
     * cm.
     */
    if (sccpcm_is_cmcb_token(reccb->cmcbs[index]) == 1) {
        sccprec_stop_timer(reccb, reccb->misc_timer);

        SCCP_DBG((sccprec_debug, SCCPREC_DBG_LEVEL_INFO2,
                 "%s %-2d:%-2d: %-20s: %s: has token\n",
                 SCCPREC_ID, reccb->sccpcb->id, reccb->id,
                 event->function_name,
                 sccpcm_print_cm(reccb->cmcbs[index])));

        sccprec_push_simple(reccb->sccpcb, SCCPREC_E_UNREG_REQ);

        return (0);
    }

    /*
     * Did we lose our connection?
     */
    if (sccpcm_is_cmcb_connected(reccb->cmcbs[index]) == 0) {
        sccprec_stop_timer(reccb, reccb->misc_timer);

        SCCP_DBG((sccprec_debug, SCCPREC_DBG_LEVEL_INFO2,
                 "%s %-2d:%-2d: %-20s: %s: connection lost\n",
                 SCCPREC_ID, reccb->sccpcb->id, reccb->id,
                 event->function_name,
                 sccpcm_print_cm(reccb->cmcbs[index])));

        /*
         * We lost our connection. Go back to IDLE state and 
         * start over.
         */
        sccprec_push_simple(reccb->sccpcb, SCCPREC_E_DONE);    

        return (0);
    }

    /*
     * Just wait, another connection must have changed.
     */
    return (0);
}

/*
 * FUNCTION:    sccprec_sem_token_req_unreg_req
 *
 * DESCRIPTION: This function initiates unregistration.
 *
 * PARAMETERS:
 *     event:   event control block
 *
 * RETURNS:     0 for success, otherwise 1.
 *
 * NOTES:
 *
 * ============================================================================
 * S_TOKEN_REQ               E_UNREG                       S_PRI_UNREG_CM
 * ----------------------------------------------------------------------------
 */
static int sccprec_sem_token_req_unreg_req (sem_event_t *event) 
{
    sccprec_reccb_t *reccb = (sccprec_reccb_t *)(event->cb);
    int             index  = reccb->cm_index;

    /*
     * Save the connection that we just made. Remember, if we are here,
     * then we have just connected to a higher priority cm. Therefore, we
     * need to unregister from the current primary.
     */
    reccb->start_index = reccb->cm_index;

    /*
     * Make sure we have a primary.
     */
    reccb->cm_index = sccpcm_get_primary_index(reccb->sccpcb);
    if (reccb->cm_index == SCCPCM_NO_INDEX) {
        SCCP_DBG((sccprec_debug, SCCPREC_DBG_LEVEL_INFO2,
                 "%s %-2d:%-2d: %-20s: No primary, skipping\n",
                 SCCPREC_ID, reccb->sccpcb->id, reccb->id,
                 event->function_name));

        /*
         * No primary. Let the secondary optimize code handle 
         * finding the primary.
         */
        sccprec_push_simple(reccb->sccpcb, SCCPREC_E_SEC_OPTIMIZE);

        return (0);
    }

    /*
     * Validate the index.
     */
    if (sccpcm_get_cm_by_index(reccb->sccpcb, reccb->cm_index) == 
        SCCPCM_NO_INDEX) {
        SCCP_DBG((sccprec_debug, SCCPREC_DBG_LEVEL_INFO2,
                 "%s %-2d:%-2d: %-20s: Null CM[%d], skipping\n",
                 SCCPREC_ID, reccb->sccpcb->id, reccb->id,
                 event->function_name,
                 index));

        sccprec_push_simple(reccb->sccpcb, SCCPREC_E_SEC_OPTIMIZE);    

        return (0);
    }

    sccprec_unregister_cm(reccb, reccb->cm_index, event->function_name);

    return (0);
}

/*
 * FUNCTION:    sccprec_sem_pri_unreg_cm_reg_req
 *
 * DESCRIPTION: This function initiates registration procedures for the 
 *              current connected secondary.
 *
 * PARAMETERS:
 *     event:   event control block
 *
 * RETURNS:     0 for success, otherwise 1.
 *
 * NOTES:
 *
 * ============================================================================
 * S_PRI_UNREG_CM            E_REG_REQ                     S_PRI_REG_CM
 * ----------------------------------------------------------------------------
 */
static int sccprec_sem_pri_unreg_cm_reg_req (sem_event_t *event) 
{
    sccprec_reccb_t *reccb = (sccprec_reccb_t *)(event->cb);

    /*
     * Restore the connection that we just made. We modified cm_index while
     * unregistering the old primary.
     */
    reccb->cm_index = reccb->start_index;

    /*
     * Validate the index.
     */
    if (sccpcm_get_cm_by_index(reccb->sccpcb, reccb->cm_index) ==
        SCCPCM_NO_INDEX) {

        SCCP_DBG((sccprec_debug, SCCPREC_DBG_LEVEL_INFO2,
                 "%s %-2d:%-2d: %-20s: Null CM[%d], skipping\n",
                 SCCPREC_ID, reccb->sccpcb->id, reccb->id,
                 event->function_name,
                 reccb->cm_index));

        sccprec_push_simple(reccb->sccpcb, SCCPREC_E_SEC_OPTIMIZE);    

        return (0);
    }

    /*
     * Are we still connected? If so, then try to register.
     */
    if (sccpcm_is_cmcb_connected(reccb->cmcbs[reccb->cm_index]) == 1) {
#if 0
	static int once = 0;

        /*sam hack */
        if (once == 0) {
            once = 1;

            sccprec_change_state(reccb, SCCPREC_S_PRI_UNREG_CM,
                                 event->function_name);

            return (0);
        }
        else if (once == 1) {
            once = 2;

            sccprec_start_timer(reccb, reccb->misc_timer,
                                SCCPREC_TIMER_WF_REGISTER,
						        5000);

            sccprec_change_state(reccb, SCCPREC_S_PRI_UNREG_CM,
                                 event->function_name);

            return (0);
        }
        else {
            once = 0;
        }
#else
        /*
         * CCM not responding to fallback register requests, so don't
         * try to register on this connected socket. Start a timer,
         * which expires, kills the connection and starts the process over.
         */
        sccprec_start_timer(reccb, reccb->misc_timer,
                            SCCPREC_TIMER_WF_REGISTER,
						    500);

        return (0);

#endif
#if 0
        sccprec_register_cm(reccb, reccb->cm_index, event->function_name);

        /*
         * Wait for notification.
         */
        return (0);                            
#endif
    } 
    else {
        SCCP_DBG((sccprec_debug, SCCPREC_DBG_LEVEL_INFO2,
                 "%s %-2d:%-2d: %-20s: %s: connection lost\n",
                 SCCPREC_ID, reccb->sccpcb->id, reccb->id,
                 event->function_name,
                 sccpcm_print_cm(reccb->cmcbs[reccb->cm_index])));
    }

    sccprec_push_simple(reccb->sccpcb, SCCPREC_E_SEC_OPTIMIZE);    

    return (0);
}

/*
 * FUNCTION:    sccprec_sem_pri_unreg_cm_dev_notify
 *
 * DESCRIPTION: This function processes notifications from the cm.
 *
 * PARAMETERS:
 *     event:   event control block
 *
 * RETURNS:     0 for success, otherwise 1.
 *
 * NOTES:
 *
 * ============================================================================
 * S_PRI_UNREG_CM            E_DEV_NOTIFY                  S_PRI_UNREG_CM
 * ----------------------------------------------------------------------------
 */
static int sccprec_sem_pri_unreg_cm_dev_notify (sem_event_t *event) 
{
    sccprec_reccb_t *reccb = (sccprec_reccb_t *)(event->cb);
    int             index  = reccb->cm_index;

    /*
     * Validate the index.
     */
    if (sccpcm_get_cm_by_index(reccb->sccpcb, index) == SCCPCM_NO_INDEX) {
        sccprec_stop_timer(reccb, reccb->misc_timer);

        SCCP_DBG((sccprec_debug, SCCPREC_DBG_LEVEL_INFO2,
                 "%s %-2d:%-2d: %-20s: Null CM[%d], skipping\n",
                 SCCPREC_ID, reccb->sccpcb->id, reccb->id,
                 event->function_name,
                 index));

        sccprec_push_simple(reccb->sccpcb, SCCPREC_E_SEC_OPTIMIZE);    

        return (0);
    }

    /*
     * Are we connected? Or in this case unregistered? If so, then we can 
     * try to register with the new primary.
     */
    if (sccpcm_is_cmcb_connected(reccb->cmcbs[reccb->cm_index]) == 1) {
        sccprec_stop_timer(reccb, reccb->misc_timer);

        SCCP_DBG((sccprec_debug, SCCPREC_DBG_LEVEL_INFO2, 
                 "%s %-2d:%-2d: %-20s: %s: unregistered\n",
                 SCCPREC_ID, reccb->sccpcb->id, reccb->id,
                 event->function_name,
                 sccpcm_print_cm(reccb->cmcbs[index])));

        sccprec_push_simple(reccb->sccpcb, SCCPREC_E_REG_REQ);    

        return (0);
    } 

    /*
     * Oh well, we still have to register since our current primary has died.
     */
    if ((sccpcm_is_cmcb_idle(reccb->cmcbs[index]) == 1) ||
        (sccpcm_is_cmcb_lockout(reccb->cmcbs[index]) == 1)) {
        sccprec_stop_timer(reccb, reccb->misc_timer);

        SCCP_DBG((sccprec_debug, SCCPREC_DBG_LEVEL_INFO2,
                 "%s %-2d:%-2d: %-20s: %s: IDLE or LOCKOUT, not unregistered\n",
                 SCCPREC_ID, reccb->sccpcb->id, reccb->id,
                 event->function_name,
                 sccpcm_print_cm(reccb->cmcbs[index])));

        sccprec_push_simple(reccb->sccpcb, SCCPREC_E_REG_REQ);    

        return (0);
    }

    /*
     * Just wait, another connection must have changed.
     */
    return (0);
}

/*
 * FUNCTION:    sccprec_sem_pri_unreg_cm_timeout
 *
 * DESCRIPTION: This function processes timeouts while waiting for the primary
 *              to unregister.
 *
 * PARAMETERS:
 *     event:   event control block
 *
 * RETURNS:     0 for success, otherwise 1.
 *
 * NOTES:
 *
 * ============================================================================
 * S_PRI_UNREG_CM            E_TIMEOUT                     S_PRI_UNREG_CM
 * ----------------------------------------------------------------------------
 */
static int sccprec_sem_pri_unreg_cm_timeout (sem_event_t *event) 
{
    sccprec_reccb_t       *reccb = (sccprec_reccb_t *)(event->cb);
    sccprec_msg_timeout_t *msg   = (sccprec_msg_timeout_t *)(event->data);

    SCCP_DBG((sccprec_debug, SCCPREC_DBG_LEVEL_SEM,
             "%s %-2d:%-2d: %-20s: %s\n",
             SCCPREC_ID, reccb->sccpcb->id, reccb->id, event->function_name,
             sccprec_timer_name(msg->event)));

    /*
     * Can't wait all day to unregister. Let's just let the optimize code
     * handle finding a primary.
     */
    sccprec_push_simple(reccb->sccpcb, SCCPREC_E_SEC_OPTIMIZE);    

    return (0);
}

/*
 * FUNCTION:    sccprec_sem_pri_reg_cm_dev_notify
 *
 * DESCRIPTION: This function processes device notifications from the cm while
 *              waiting for a registration response.
 *
 * PARAMETERS:
 *     event:   event control block
 *
 * RETURNS:     0 for success, otherwise 1.
 *
 * NOTES:
 *
 * ============================================================================
 * S_PRI_REG_CM              E_DEV_NOTIFY                  S_PRI_REG_CM
 * ----------------------------------------------------------------------------
 */
static int sccprec_sem_pri_reg_cm_dev_notify (sem_event_t *event) 
{
    sccprec_reccb_t *reccb = (sccprec_reccb_t *)(event->cb);
    int             index  = reccb->cm_index;

    /*
     * Validate the index.
     */
    if (sccpcm_get_cm_by_index(reccb->sccpcb, index) == SCCPCM_NO_INDEX) {
        sccprec_stop_timer(reccb, reccb->misc_timer);

        SCCP_DBG((sccprec_debug, SCCPREC_DBG_LEVEL_INFO2,
                 "%s %-2d:%-2d: %-20s: Null CM[%d], skipping\n",
                 SCCPREC_ID, reccb->sccpcb->id, reccb->id,
                 event->function_name,
                 index));

        sccprec_push_simple(reccb->sccpcb, SCCPREC_E_PRI_CHECK);    

        return (0);
    }

    /*
     * Are we registered? If so, then we can move on.
     */
    if (sccpcm_is_cmcb_registered(reccb->cmcbs[reccb->cm_index]) == 1) {
        sccprec_stop_timer(reccb, reccb->misc_timer);

        SCCP_DBG((sccprec_debug, SCCPREC_DBG_LEVEL_INFO2,
                 "%s %-2d:%-2d: %-20s: %s: registered\n",
                 SCCPREC_ID, reccb->sccpcb->id, reccb->id,
                 event->function_name,
                 sccpcm_print_cm(reccb->cmcbs[index])));

        /*
         * Reset the list counter because we have a priamry.
         */
        reccb->cm_list_iterations = 0;

        /*
         * Now we need to disconnect the highest connected.
         */
        if (sccprec_cleanup_secondarys(reccb, event->function_name) == 0) {
            /*
             * We need a new secondary.
             */
            SCCP_DBG((sccprec_debug, SCCPREC_DBG_LEVEL_INFO2,
                     "%s %-2d:%-2d: %-20s: Need a new secondary",
                     SCCPREC_ID, reccb->sccpcb->id, reccb->id,
                     event->function_name));

            sccprec_push_simple(reccb->sccpcb, SCCPREC_E_SEC_CHECK);    

            return (0);
        }

        sccprec_push_simple(reccb->sccpcb, SCCPREC_E_SEC_OPTIMIZE);    

        return (0);
    } 

    /*
     * If we are IDLE or LOCKOUT then get next.
     */
    if ((sccpcm_is_cmcb_idle(reccb->cmcbs[index]) == 1) ||
        (sccpcm_is_cmcb_lockout(reccb->cmcbs[index]) == 1)) {
        sccprec_stop_timer(reccb, reccb->misc_timer);

        SCCP_DBG((sccprec_debug, SCCPREC_DBG_LEVEL_INFO2,
                 "%s %-2d:%-2d: %-20s: %s: IDLE or LOCKOUT, skipping\n",
                 SCCPREC_ID, reccb->sccpcb->id, reccb->id,
                 event->function_name,
                 sccpcm_print_cm(reccb->cmcbs[index])));

        sccprec_push_simple(reccb->sccpcb, SCCPREC_E_PRI_CHECK);    

        return (0);
    }

    /*
     * Just wait, another connection must have changed.
     */
    return (0);
}

/*
 * FUNCTION:    sccprec_sem_pri_reg_cm_timeout
 *
 * DESCRIPTION: This function processes timeouts while waiting for the primary
 *              to register.
 *
 * PARAMETERS:
 *     event:   event control block
 *
 * RETURNS:     0 for success, otherwise 1.
 *
 * NOTES:
 *
 * ============================================================================
 * S_PRI_REG_CM              E_TIMEOUT                     S_PRI_REG_CM
 * ----------------------------------------------------------------------------
 */
static int sccprec_sem_pri_reg_cm_timeout (sem_event_t *event) 
{
    sccprec_reccb_t       *reccb = (sccprec_reccb_t *)(event->cb);
    int                   index  = reccb->cm_index;
    sccprec_msg_timeout_t *msg   = (sccprec_msg_timeout_t *)(event->data);

    SCCP_DBG((sccprec_debug, SCCPREC_DBG_LEVEL_SEM,
             "%s %-2d:%-2d: %-20s: %s\n",
             SCCPREC_ID, reccb->sccpcb->id, reccb->id, event->function_name,
             sccprec_timer_name(msg->event)));

    /*
     * Validate the index.
     */
    if (sccpcm_get_cm_by_index(reccb->sccpcb, index) != SCCPCM_NO_INDEX) {
        /*
         * Are we registered?
         */
        if (sccpcm_is_cmcb_registered(reccb->cmcbs[reccb->cm_index]) == 1) {
            sccprec_stop_timer(reccb, reccb->misc_timer);

            SCCP_DBG((sccprec_debug, SCCPREC_DBG_LEVEL_INFO2,
                     "%s %-2d:%-2d: %-20s: %s: registered\n",
                     SCCPREC_ID, reccb->sccpcb->id, reccb->id,
                     event->function_name,
                     sccpcm_print_cm(reccb->cmcbs[index])));

            reccb->cm_list_iterations = 0;

            /*
             * Now we need to disconnect the highest connected.
             */
            if (sccprec_cleanup_secondarys(reccb, event->function_name) == 0) {
                /*
                 * We need a new secondary.
                 */
                SCCP_DBG((sccprec_debug, SCCPREC_DBG_LEVEL_INFO2, 
                         "%s %-2d:%-2d: %-20s: Need a new secondary",
                         SCCPREC_ID, reccb->sccpcb->id, reccb->id,
                         event->function_name));

                sccprec_push_simple(reccb->sccpcb, SCCPREC_E_SEC_CHECK);    

                return (0);
            }

            sccprec_push_simple(reccb->sccpcb, SCCPREC_E_SEC_OPTIMIZE);    

            return (0);
        } 
    }

    /*
     * We need a new primary.
     */
    SCCP_DBG((sccprec_debug, SCCPREC_DBG_LEVEL_INFO2, "%s %-2d:%-2d: %-20s: %s: "
             "hasn't completed registration, need new primary\n",
             SCCPREC_ID, reccb->sccpcb->id, reccb->id, event->function_name,
             sccpcm_print_cm(reccb->cmcbs[index])));

    /*
     * Close the connection for the current CM before we start a new one.
     */
    sccpcm_push_error(reccb->sccpcb, SCCPCM_E_ERROR,
                      reccb->cm_index, SCCPCM_ERROR_DR_PRI_REG_TO);

    /*sam
     * may need to check for timing problems if the cm we disconnect does
     * not go to idle before we start with the pri_check.
     */
    sccprec_push_simple(reccb->sccpcb, SCCPREC_E_PRI_CHECK);    

    return (0);
}

/*
 * FUNCTION:    sccprec_sem_sec_opt_sec_optimize
 *
 * DESCRIPTION: This function ensures that a primary is available and that
 *              a secondary is available. If not, then the required procedures
 *              to find them will be initiated.
 *
 * PARAMETERS:
 *     event:   event control block
 *
 * RETURNS:     0 for success, otherwise 1.
 *
 * NOTES:
 *
 * ============================================================================
 * S_SEC_OPTIMIZE            E_SEC_OPTIMIZE                S_MAKE_SEC_CON
 * ----------------------------------------------------------------------------
 * S_MAKE_SEC_CON            E_SEC_OPTIMIZE                S_MAKE_SEC_CON
 * ----------------------------------------------------------------------------
 */
static int sccprec_sem_sec_opt_sec_optimize (sem_event_t *event) 
{
    sccprec_reccb_t *reccb = (sccprec_reccb_t *)(event->cb);
    int             index  = reccb->cm_index;
    int             i;

    /*
     * Make sure we have a primary.
     */
    if (sccpcm_get_primary_index(reccb->sccpcb) == SCCPCM_NO_INDEX) {
        SCCP_DBG((sccprec_debug, SCCPREC_DBG_LEVEL_INFO2,
                 "%s %-2d:%-2d: %-20s: No primary, skipping\n",
                 SCCPREC_ID, reccb->sccpcb->id, reccb->id,
                 event->function_name));

        /*
         * No primary. Go back to IDLE and start over.
         */
        sccprec_push_simple(reccb->sccpcb, SCCPREC_E_DONE);

        return (0);
    }

    /*
     * Check if we have a better secondary. It is possible that we fell back
    * to a higher priority cm and the previous primary might be in a
     * connected state. We can make that the highest priority secondary.
     */
    for (i = 0; i < reccb->cm_index; i++) {
        /*
         * Check if the cm is already connected.
         */
        if (sccpcm_is_cmcb_connected(reccb->cmcbs[reccb->cm_index]) == 1) {
            SCCP_STRNCPY(sccprec_buf,
                         sccpcm_print_cm(reccb->cmcbs[reccb->cm_index]),
                         sizeof(sccprec_buf)) ;

            SCCP_DBG((sccprec_debug, SCCPREC_DBG_LEVEL_INFO2,
                     "%s %-2d:%-2d: %-20s: %s: skipping, %s: better secondary\n",
                     SCCPREC_ID, reccb->sccpcb->id, reccb->id,
                     event->function_name,
                     sccprec_buf,
                     sccpcm_print_cm(reccb->cmcbs[i])));

            sccprec_push_simple(reccb->sccpcb, SCCPREC_E_DONE);    

            return (0);
        }
    }

    /*
     * Validate the index.
     */
    if (sccpcm_get_cm_by_index(reccb->sccpcb, index) == SCCPCM_NO_INDEX) {
        SCCP_DBG((sccprec_debug, SCCPREC_DBG_LEVEL_INFO2,
                 "%s %-2d:%-2d: %-20s: Null CM[%d], skipping\n",
                 SCCPREC_ID, reccb->sccpcb->id, reccb->id,
                 event->function_name,
                 index));

        sccprec_push_simple(reccb->sccpcb, SCCPREC_E_DONE);    

        return (0);
    }

    /*
     * Check if cm is secondary/connected.
     */
    sccprec_connect_cm(reccb, reccb->cm_index,
                       event->function_name);

    return (0);
}

/*
 * FUNCTION:    sccprec_sem_make_sec_con_dev_notify
 *
 * DESCRIPTION: This function processes device notifications from the cm. 
 *              The function ensures that a primary is available and that
 *              a secondary is available. If not, then the required procedures
 *              to find them will be initiated.
 *
 * PARAMETERS:
 *     event:   event control block
 *
 * RETURNS:     0 for success, otherwise 1.
 *
 * NOTES:
 *
 * ============================================================================
 * S_MAKE_SEC_CON            E_DEV_NOTIFY                  S_MAKE_SEC_CON
 * ----------------------------------------------------------------------------
 */
static int sccprec_sem_make_sec_con_dev_notify (sem_event_t *event) 
{
    sccprec_reccb_t *reccb = (sccprec_reccb_t *)(event->cb);
    int             index  = reccb->cm_index;

    /*
     * Make sure we have a primary.
     */
    if (sccpcm_get_primary_index(reccb->sccpcb) == SCCPCM_NO_INDEX) {
        sccprec_stop_timer(reccb, reccb->misc_timer);

        SCCP_DBG((sccprec_debug, SCCPREC_DBG_LEVEL_INFO2,
                 "%s %-2d:%-2d: %-20s: No primary, skipping\n",
                 SCCPREC_ID, reccb->sccpcb->id, reccb->id,
                 event->function_name));

        /*
         * No primary. Go back to IDLE and start over.
         */
        sccprec_push_simple(reccb->sccpcb, SCCPREC_E_DONE);

        return (0);
    }

    /*
     * Validate the index.
     */
    if (sccpcm_get_cm_by_index(reccb->sccpcb, index) == SCCPCM_NO_INDEX) {
        sccprec_stop_timer(reccb, reccb->misc_timer);

        SCCP_DBG((sccprec_debug, SCCPREC_DBG_LEVEL_INFO2,
                 "%s %-2d:%-2d: %-20s: Null CM[%d], skipping\n",
                 SCCPREC_ID, reccb->sccpcb->id, reccb->id,
                 event->function_name,
                 index));

        sccprec_push_simple(reccb->sccpcb, SCCPREC_E_DONE);    

        return (0);
    }

    /*
     * Did the cm connect? If so we are done.
     */
    if (sccpcm_is_cmcb_connected(reccb->cmcbs[index]) == 1) {
        sccprec_stop_timer(reccb, reccb->misc_timer);

        SCCP_DBG((sccprec_debug, SCCPREC_DBG_LEVEL_INFO2,
                 "%s %-2d:%-2d: %-20s: %s: connected\n",
                 SCCPREC_ID, reccb->sccpcb->id, reccb->id,
                 event->function_name,
                 sccpcm_print_cm(reccb->cmcbs[index])));

        sccprec_push_simple(reccb->sccpcb, SCCPREC_E_DONE);    

        return (0);
    }

    /*
     * Just wait, another connection must have changed.
     */
    return (0);
}

/*
 * FUNCTION:    sccprec_sem_make_sec_con_timeout
 *
 * DESCRIPTION: This function processes timeouts while waiting for a cm to
 *              connect. The function ensures that a primary is available
 *              and that a secondary is available. If not, then the required
 *              procedures to find them will be initiated.
 *
 * PARAMETERS:
 *     event:   event control block
 *
 * RETURNS:     0 for success, otherwise 1.
 *
 * NOTES:
 *
 * ============================================================================
 * S_MAKE_SEC_CON            E_TIMEOUT                     S_MAKE_SEC_CON
 * ----------------------------------------------------------------------------
 */
static int sccprec_sem_make_sec_con_timeout (sem_event_t *event) 
{
    sccprec_reccb_t       *reccb = (sccprec_reccb_t *)(event->cb);
    int                   index  = reccb->cm_index;
    int                   pri_index;
    int                   sec_index;
    sccprec_msg_timeout_t *msg = (sccprec_msg_timeout_t *)(event->data);

    SCCP_DBG((sccprec_debug, SCCPREC_DBG_LEVEL_SEM,
             "%s %-2d:%-2d: %-20s: %s\n",
             SCCPREC_ID, reccb->sccpcb->id, reccb->id, event->function_name,
             sccprec_timer_name(msg->event)));

    /*
     * Make sure we have a primary.
     */
    pri_index = sccpcm_get_primary_index(reccb->sccpcb);
    if (pri_index == SCCPCM_NO_INDEX) {
        sccprec_stop_timer(reccb, reccb->misc_timer);

        SCCP_DBG((sccprec_debug, SCCPREC_DBG_LEVEL_INFO2, 
                 "%s %-2d:%-2d: %-20s: no primary, skipping\n",
                 SCCPREC_ID, reccb->sccpcb->id, reccb->id,
                 event->function_name));

        /*
         * No primary. Start over.
         */
        sccprec_push_simple(reccb->sccpcb, SCCPREC_E_DONE);

        return (0);
    }

    /*
     * Make sure we have a secondary.
     */
    sec_index = sccpcm_get_secondary_index(reccb->sccpcb);
    if (sec_index == SCCPCM_NO_INDEX) {
        sccprec_stop_timer(reccb, reccb->misc_timer);

        SCCP_DBG((sccprec_debug, SCCPREC_DBG_LEVEL_INFO2,
                 "%s %-2d:%-2d: %-20s: no secondary, skipping\n",
                 SCCPREC_ID, reccb->sccpcb->id, reccb->id,
                 event->function_name));

        /*
         * No secondary. Start over.
         */
        sccprec_push_simple(reccb->sccpcb, SCCPREC_E_DONE);

        return (0);
    }

    /*
     * Validate the index.
     */
    if (sccpcm_get_cm_by_index(reccb->sccpcb, index) == SCCPCM_NO_INDEX) {
        sccprec_stop_timer(reccb, reccb->misc_timer);

        SCCP_DBG((sccprec_debug, SCCPREC_DBG_LEVEL_INFO2,
                 "%s %-2d:%-2d: %-20s: Null CM[%d], skipping\n",
                 SCCPREC_ID, reccb->sccpcb->id, reccb->id,
                 event->function_name,
                 index));

        sccprec_push_simple(reccb->sccpcb, SCCPREC_E_DONE);    

        return (0);
    }

    /*
     * Are we connected yet? If so, then we are done.
     */
    if (sccpcm_is_cmcb_connected(reccb->cmcbs[index]) == 1) {
        SCCP_DBG((sccprec_debug, SCCPREC_DBG_LEVEL_INFO2, 
                 "%s %-2d:%-2d: %-20s: %s: connected\n",
                 SCCPREC_ID, reccb->sccpcb->id, reccb->id,
                 event->function_name,
                 sccpcm_print_cm(reccb->cmcbs[index])));

        sccprec_push_simple(reccb->sccpcb, SCCPREC_E_DONE);    

        return (0);
    }

    /*
     * Let's try another one.
     */
    sccprec_get_next_cm_index(reccb);

    /*
     * Try to connect to the next best one and not past the 
     * current secondary.
     */
    while ((reccb->cm_index < sec_index) &&
           (reccb->start_index != reccb->cm_index)) {
        /*
         * Validate the index.
         */
        if (sccpcm_get_cm_by_index(reccb->sccpcb, reccb->cm_index) !=
            SCCPCM_NO_INDEX) {
            /*
             * Are we connected yet?
             */
            if (sccpcm_is_cmcb_connected(reccb->cmcbs[reccb->cm_index]) == 1) {
                SCCP_STRNCPY(sccprec_buf,
                             sccpcm_print_cm(reccb->cmcbs[sec_index]),
                             sizeof(sccprec_buf)) ;

                SCCP_DBG((sccprec_debug, SCCPREC_DBG_LEVEL_INFO2,
                         "%s %-2d:%-2d: %-20s: %s: optimizing secondary, "
                         "to connected %s\n",
                         SCCPREC_ID, reccb->sccpcb->id, reccb->id,
                         event->function_name,
                         sccprec_buf,
                         sccpcm_print_cm(reccb->cmcbs[reccb->cm_index])));

                /*
                 * DONE will cleanup any outstanding connections.
                 */
                sccprec_push_simple(reccb->sccpcb, SCCPREC_E_DONE);    

                return (0);
            }

            /*
             * Check if we can connect.
             */
            if (sccpcm_is_cmcb_idle(reccb->cmcbs[reccb->cm_index]) == 1) {
                SCCP_STRNCPY(sccprec_buf,
                             sccpcm_print_cm(reccb->cmcbs[sec_index]),
                             sizeof(sccprec_buf)) ;

                SCCP_DBG((sccprec_debug, SCCPREC_DBG_LEVEL_INFO2,
                         "%s %-2d:%-2d: %-20s: %s: optimizing secondary, "
                         "to try connecting %s\n",
                         SCCPREC_ID, reccb->sccpcb->id, reccb->id,
                         event->function_name,
                         sccprec_buf,
                         sccpcm_print_cm(reccb->cmcbs[reccb->cm_index])));
                
                sccprec_push_simple(reccb->sccpcb, SCCPREC_E_SEC_OPTIMIZE);  

                return (0);
            }
        } /* if (sccpcm_get_cm_by_index(reccb->sccpcb, index) != */

        sccprec_get_next_cm_index(reccb);
    } /* while ((reccb->cm_index < sec_index) && */

    /*
     * Goto to next state, unable to connect and no other
     * choices.
     */
    sccprec_push_simple(reccb->sccpcb, SCCPREC_E_DONE);  

    return (0);
}

/*
 * FUNCTION:    sccprec_sem_reset
 *
 * DESCRIPTION: Process reset request. Notify the application that it needs to
 *              reset.
 *
 * PARAMETERS:
 *     event:   event control block
 *
 * RETURNS:     0 for success, otherwise 1.
 *
 * NOTES:
 *
 * ============================================================================
 * S_ALL                     E_RESET                       NO CHANGE
 * ----------------------------------------------------------------------------
 */
static int sccprec_sem_reset (sem_event_t *event) 
{
    sccprec_reccb_t   *reccb   = (sccprec_reccb_t *)(event->cb);
    sccpmsg_general_t *gen_msg = (sccpmsg_general_t *)(event->data);
    sccpmsg_reset_t   *msg     = &(gen_msg->body.reset);
    gapi_causes_e     cause;

    /*
     * Cancel any timers. There is no need to do the polling
     * looking for primary or secondary cms.
     */
    sccprec_stop_timer(reccb, reccb->misc_timer);

	SCCP_DBG((sccprec_debug, SCCPREC_DBG_LEVEL_SEM,
			 "%s %-2d:%-2d: %-20s: type= %d\n",
			 SCCPREC_ID, reccb->sccpcb->id, reccb->id, event->function_name,
			 msg->reset_type));

    /*
     * Inform the application that we need a reset.
     */
    switch (msg->reset_type) {
    case (SCCPMSG_RESET_TYPE_RESET):
        cause = GAPI_CAUSE_CM_RESET;
        
        break;

    case (SCCPMSG_RESET_TYPE_RESTART):
        cause = GAPI_CAUSE_CM_RESTART;

        break;

    case (SCCPMSG_RESET_TYPE_INVALID):
    default:
        cause = GAPI_CAUSE_NO_CM_FOUND;

        break;
    
    }

    /* 
     * Let the application know that is needs to reset.
     */
    SCCP_RESET(reccb, GAPI_MSG_RESET, cause);

    /*
     * Wait around for the reset_req. The application will send a 
     * reset_req when it wants the reset to continue.
     */
    return (0);
}

/*
 * FUNCTION:    sccprec_sem_reset_req
 *
 * DESCRIPTION: Process reset request from the application. Set the TCP close
 *              cause and close all cms. The application should only call this
 *              api when all streams are idle.
 *
 * PARAMETERS:
 *     event:   event control block
 *
 * RETURNS:     0 for success, otherwise 1.
 *
 * NOTES:
 *
 * ============================================================================
 * S_ALL                     E_RESET_REQ                   S_RESETTING
 * ----------------------------------------------------------------------------
 */
static int sccprec_sem_reset_req (sem_event_t *event) 
{
    sccprec_reccb_t         *reccb = (sccprec_reccb_t *)(event->cb);
    sccprec_msg_reset_req_t *msg   = (sccprec_msg_reset_req_t *)(event->data);
    int                     count;

    /*
     * Cancel any timers. There is no need to do the polling
     * to look for cms because we are going to reset.
     */
    sccprec_stop_timer(reccb, reccb->misc_timer);

    reccb->reset_cause = msg->cause;

    switch (msg->cause) {
    case (GAPI_CAUSE_CM_RESTART):
        sccprec_set_tcp_close_cause(reccb->sccpcb,
                                    SCCPMSG_ALARM_LAST_RESTART);

        break;

    case (GAPI_CAUSE_CM_RESET):
        sccprec_set_tcp_close_cause(reccb->sccpcb,
                                    SCCPMSG_ALARM_LAST_RESET);

        break;

    case (GAPI_CAUSE_CM_CLOSE):
    default:
        sccprec_set_tcp_close_cause(reccb->sccpcb,
                                    SCCPMSG_ALARM_LAST_KEYPAD);

        break;
    }

    count = sccprec_close_cms(reccb, event->function_name);

    /*
     * Start a timer to wait for the connections to close.
     */
    if (count > 0) {
        sccprec_start_timer(reccb, reccb->misc_timer,
                            SCCPREC_TIMER_WF_CLOSE,
                            reccb->close_to);
    }
    else {
        sccprec_push_timeout(reccb->sccpcb, SCCPREC_E_TIMEOUT,
                             SCCPREC_TIMER_WF_CLOSE);
    }

    return (0);
}

/*
 * FUNCTION:    sccprec_sem_resetting_timeout
 *
 * DESCRIPTION: This function handles the timeout that occurs while waiting
 *              for cms to close. sccprec closes all cms during a reset and
 *              must wait until all the cms are closed before resetting itself.
 *
 * PARAMETERS:
 *     event:   event control block
 *
 * RETURNS:     0 for success, otherwise 1.
 *
 * NOTES:
 *
 * ============================================================================
 * S_RESETTING               E_TIMEOUT                     S_RESETTING
 * ----------------------------------------------------------------------------
 */
static int sccprec_sem_resetting_timeout (sem_event_t *event) 
{
    sccprec_reccb_t       *reccb = (sccprec_reccb_t *)(event->cb);

	/*
	 * Check if this was a real timeout to add debug. There are other functions
	 * which will call this function.
	 */
	if (event->event_id == SCCPREC_E_TIMEOUT) {
		sccprec_msg_timeout_t *msg = (sccprec_msg_timeout_t *)(event->data);

		SCCP_DBG((sccprec_debug, SCCPREC_DBG_LEVEL_SEM,
				 "%s %-2d:%-2d: %-20s: %s - 0x%08x\n",
				 SCCPREC_ID, reccb->sccpcb->id, reccb->id,
                 event->function_name,
				 sccprec_timer_name(msg->event), event));
	}

    sccprec_push_simple(reccb->sccpcb, SCCPREC_E_DONE);    

    return (0);
}

/*
 * FUNCTION:    sccprec_sem_resetting_dev_notify
 *
 * DESCRIPTION: This function handles device notifications from the cms. This
 *              state is waiting for the cms to close.
 *
 * PARAMETERS:
 *     event:   event control block
 *
 * RETURNS:     0 for success, otherwise 1.
 *
 * NOTES:
 *
 * ============================================================================
 * S_RESETTING               E_DEV_NOTIFY                  S_RESETTING
 * ----------------------------------------------------------------------------
 */
static int sccprec_sem_resetting_dev_notify (sem_event_t *event) 
{
    sccprec_reccb_t *reccb = (sccprec_reccb_t *)(event->cb);

    /*
     * Go ahead and finish the reset if all cms are idle. No need to
     * wait for the timer to pop.
     */
    if (sccpcm_are_all_cms_idle(reccb->sccpcb) == 1) {
        sccprec_stop_timer(reccb, reccb->misc_timer);

        sccprec_push_simple(reccb->sccpcb, SCCPREC_E_DONE);    
    }

    return (0);
}

/*
 * FUNCTION:    sccprec_sem_resetting_done
 *
 * DESCRIPTION: Reset is complete. Go to the idle state and wait or restart
 *              depending on the original reset request.
 *
 * PARAMETERS:
 *     event:   event control block
 *
 * RETURNS:     0 for success, otherwise 1.
 *
 * NOTES:
 *
 * ============================================================================
 * S_RESETTING               E_DONE                        S_IDLE
 * ----------------------------------------------------------------------------
 */
static int sccprec_sem_resetting_done (sem_event_t *event) 
{
    sccprec_reccb_t *reccb = (sccprec_reccb_t *)(event->cb);

    switch (reccb->reset_cause) {
    case (GAPI_CAUSE_CM_RESTART):
        /*
         * Goto IDLE and restart the whole thing...
         */
        sccprec_init_reccb(reccb, SCCPREC_RESET_TYPE_RESTART);        
        //sllist_empty_list(reccb->sccpcb->cmcbs);
        
        sccprec_push_simple(reccb->sccpcb, SCCPREC_E_DONE);    

        break;

    case (GAPI_CAUSE_CM_RESET):
    case (GAPI_CAUSE_CM_CLOSE):
    default:
        /*
         * Just cleanup - the application will open another session.
         */
        sccprec_init_reccb(reccb, SCCPREC_RESET_TYPE_RESET);
        sllist_empty_list(reccb->sccpcb->cmcbs);

        /*
         * Ensure that all events are flushed.
         */
        sccprec_set_free_events_flag(reccb->sccpcb, 1);

        break;
    }

    SCCP_SESSIONSTATUS(reccb, GAPI_MSG_STATUS,
                       GAPI_STATUS_RESET_COMPLETE,
                       GAPI_STATUS_DATA_MIN, NULL);

    SCCP_RESETSESSION_RES(reccb, GAPI_MSG_RESETSESSION_RES, GAPI_CAUSE_OK);
 
    return (0);
}

/*
 * FUNCTION:    sccprec_sem_open
 *
 * DESCRIPTION: This function opens a new session. The function will verify
 *              the supplied data and start the session.
 *
 * PARAMETERS:
 *     event:   event control block
 *
 * RETURNS:     0 for success, otherwise 1.
 *
 * NOTES:
 *
 * ============================================================================
 * S_IDLE                    E_OPEN                        S_IDLE
 * ----------------------------------------------------------------------------
 */
static int sccprec_sem_open (sem_event_t *event)
{
    sccprec_reccb_t *reccb = (sccprec_reccb_t *)(event->cb);
    int             count  = 0;
    
    /*
     * Make sure we are not already open.
     */
    if (reccb->session_state != SCCPREC_SESSION_STATE_IDLE) {
        /*
         * Already open - just let the app know and ignore the request.
         */
        SCCP_OPENSESSION_RES(reccb, GAPI_MSG_OPENSESSION_RES,
                             GAPI_CAUSE_SESSION_ALREADY_OPENED);

        return (0);
    }

    reccb->session_state = SCCPREC_SESSION_STATE_OPEN;

    count = sccprec_open_init(event);

    /*
     * Check if we got some good data.
     */
    if (count < 0) {
        SCCP_OPENSESSION_RES(reccb, GAPI_MSG_OPENSESSION_RES,
                             GAPI_CAUSE_SESSION_INVALID_DATA);

        sccprec_init_reccb(reccb, SCCPREC_RESET_TYPE_RESET);
    }
    else if (count > 0) { 
        /*
         * Got some valid addresses, so let's look for a primary cm.
         */
        sccprec_push_simple(reccb->sccpcb, SCCPREC_E_PRI_CHECK);
    }
    else {
        /*
         * No valid addresses, so let's punt and notify the application.
         */
        SCCP_OPENSESSION_RES(reccb, GAPI_MSG_OPENSESSION_RES,
                             GAPI_CAUSE_CMS_UNDEFINED);

        sccprec_init_reccb(reccb, SCCPREC_RESET_TYPE_RESET);
    }
    
    return (0);
}
