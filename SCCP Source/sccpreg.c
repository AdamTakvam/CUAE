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
 *     sccpreg.c
 *
 *  CREATION/MODIFICATION DATES
 *     Created:  November 2002, Sam Hague
 *     Modified: 
 *
 *  DESCRIPTION
 *     SCCP Registration implementation
 */
#include "sccp.h"
#include "sccp_debug.h"
#include "sem.h"
#include "am.h"
#include "ssapi.h"
#include "sccpmsg.h"


static sccpmsg_msgcb_t sccpreg_msgcb;


typedef enum sccpreg_states_t_ {
    SCCPREG_S_MIN = -1,
    SCCPREG_S_IDLE,
    SCCPREG_S_REGISTERING,
    SCCPREG_S_REGISTERED,
    SCCPREG_S_UNREGISTERING,
    SCCPREG_S_MAX
} sccpreg_states_t;

static char *sccpreg_state_names[] = {
    "S_IDLE",
    "S_REGISTERING",
    "S_REGISTERED",
    "S_UNREGISTERING"
};

static char *sccpreg_event_names[] = {
    "E_REG_REQ",
    "E_REGISTER_ACK",
    "E_REGISTER_REJ",
    "E_CAPABILITIES_REQ",
    "E_BUTTON_TEMPLATE_RES",
    "E_SOFTKEY_TEMPLATE_RES",
    "E_SOFTKEY_SET_RES",
    "E_SELECT_SOFTKEYS",
    "E_CONFIG_STAT_RES",    
    "E_LINE_STAT_RES",
    "E_SPEEDDIAL_STAT_RES",
    "E_FORWARD_STAT_RES",
    "E_FEATURE_STAT_RES",
    "E_SERVICEURL_STAT_RES",
    "E_VERSION_RES",
    "E_SERVER_RES",
    "E_UNREG_REQ",
    "E_UNREGISTER_ACK",
    "E_REGISTERED",
    "E_CLEANUP"
};


static int sccpreg_push_simple(sccp_sccpcb_t *sccpcb, int msg_id);
static char *sccpreg_sem_name(int id);
static char *sccpreg_state_name(int id);
static char *sccpreg_event_name(int id);
static char *sccpreg_function_name(int id);
static void sccpreg_debug_sem_entry(sem_event_t *event);
static int  sccpreg_validate_cb(void *sccpcb, void *cb);
static void sccpreg_change_state(sccpreg_regcb_t *regcb, int new_state,
                                 char *fname);

static int sccpreg_sem_default(sem_event_t *event);
static int sccpreg_sem_cleanup(sem_event_t *event);
static int sccpreg_sem_xxx_reg_req(sem_event_t *event);
static int sccpreg_sem_reg_req(sem_event_t *event);
static int sccpreg_sem_register_ack(sem_event_t *event);
static int sccpreg_sem_register_rej(sem_event_t *event);
static int sccpreg_sem_capabilities_req(sem_event_t *event);
static int sccpreg_sem_button_template_res(sem_event_t *event);
static int sccpreg_sem_softkey_template_res(sem_event_t *event);
static int sccpreg_sem_softkey_set_res(sem_event_t *event);
static int sccpreg_sem_line_stat_res(sem_event_t *event);
static int sccpreg_sem_speeddial_stat_res(sem_event_t *event);
static int sccpreg_sem_feature_stat_res(sem_event_t *event);
static int sccpreg_sem_serviceurl_stat_res(sem_event_t *event);
static int sccpreg_sem_unreg_req(sem_event_t *event);
static int sccpreg_sem_unregister_ack(sem_event_t *event);
static int sccpreg_sem_registered(sem_event_t *event);

typedef enum sccpreg_sem_functions_t_ {
    SCCPREG_SEM_MIN = -1,
    SCCPREG_SEM_DEFAULT,
    SCCPREG_SEM_CLEANUP,
    SCCPREG_SEM_XXX_REG_REQ,
    SCCPREG_SEM_REG_REQ,    
    SCCPREG_SEM_REGISTER_ACK,    
    SCCPREG_SEM_REGISTER_REJ,    
    SCCPREG_SEM_CAPABILITIES_REQ,    
    SCCPREG_SEM_BUTTON_TEMPLATE_RES,    
    SCCPREG_SEM_SOFTKEY_TEMPLATE_RES,    
    SCCPREG_SEM_SOFTKEY_SET_RES,    
    SCCPREG_SEM_LINE_STAT_RES,    
    SCCPREG_SEM_SPEEDDIAL_STAT_RES,
    SCCPREG_SEM_FEATURE_STAT_RES,
    SCCPREG_SEM_SERVICEURL_STAT_RES,
    SCCPREG_SEM_UNREG_REQ,
    SCCPREG_SEM_UNREGISTER_ACK,
    SCCPREG_SEM_REGISTERED,
    SCCPREG_SEM_MAX
} sccpreg_sem_functions_t;

static sem_function_t sccpreg_sem_functions[] =
{
    sccpreg_sem_default,
    sccpreg_sem_cleanup,
    sccpreg_sem_xxx_reg_req,
    sccpreg_sem_reg_req,
    sccpreg_sem_register_ack,
    sccpreg_sem_register_rej,
    sccpreg_sem_capabilities_req,
    sccpreg_sem_button_template_res,
    sccpreg_sem_softkey_template_res,
    sccpreg_sem_softkey_set_res,
    sccpreg_sem_line_stat_res,
    sccpreg_sem_speeddial_stat_res,
    sccpreg_sem_feature_stat_res,
    sccpreg_sem_serviceurl_stat_res,
    sccpreg_sem_unreg_req,
    sccpreg_sem_unregister_ack,
    sccpreg_sem_registered
};

static char *sccpreg_sem_function_names[] =
{
    "sem_default",
    "sccpreg_sem_cleanup",
    "sccpreg_sem_xxx_reg_req",
    "sem_reg_req",
    "sem_register_ack",
    "sem_register_rej",
    "sem_capabilities_req",
    "sem_button_template_res",
    "sem_softkey_template_res",
    "sem_softkey_set_res",
    "sem_line_stat_res",
    "sem_speeddial_stat_res",
    "sem_feature_stat_res",
    "sem_serviceurl_stat_res",
    "sem_unreg_req",
    "sem_unregister_ack",
    "sccpreg_sem_registered"

};

static sem_events_t sccpreg_sem_s_idle[] =
{
    {SCCPREG_E_REG_REQ,              SCCPREG_SEM_REG_REQ,              SCCPREG_S_REGISTERING},
    {SCCPREG_E_CLEANUP,              SCCPREG_SEM_CLEANUP,              SCCPREG_S_IDLE}
};

static sem_events_t sccpreg_sem_s_registering[] =
{
    {SCCPREG_E_REG_REQ,              SCCPREG_SEM_XXX_REG_REQ,          SCCPREG_S_REGISTERING},
    {SCCPREG_E_REGISTER_ACK,         SCCPREG_SEM_REGISTER_ACK,         SCCPREG_S_REGISTERING},
    {SCCPREG_E_REGISTER_REJ,         SCCPREG_SEM_REGISTER_REJ,         SCCPREG_S_REGISTERING},
    {SCCPREG_E_CAPABILITIES_REQ,     SCCPREG_SEM_CAPABILITIES_REQ,     SCCPREG_S_REGISTERING},
    {SCCPREG_E_BUTTON_TEMPLATE_RES,  SCCPREG_SEM_BUTTON_TEMPLATE_RES,  SCCPREG_S_REGISTERING},
    {SCCPREG_E_SOFTKEY_TEMPLATE_RES, SCCPREG_SEM_SOFTKEY_TEMPLATE_RES, SCCPREG_S_REGISTERING},
    {SCCPREG_E_SOFTKEY_SET_RES,      SCCPREG_SEM_SOFTKEY_SET_RES,      SCCPREG_S_REGISTERING},
    {SCCPREG_E_LINE_STAT_RES,        SCCPREG_SEM_LINE_STAT_RES,        SCCPREG_S_REGISTERING},
    {SCCPREG_E_SPEEDDIAL_STAT_RES,   SCCPREG_SEM_SPEEDDIAL_STAT_RES,   SCCPREG_S_REGISTERING},
    {SCCPREG_E_FEATURE_STAT_RES,     SCCPREG_SEM_FEATURE_STAT_RES,     SCCPREG_S_REGISTERING},
    {SCCPREG_E_SERVICEURL_STAT_RES,  SCCPREG_SEM_SERVICEURL_STAT_RES,  SCCPREG_S_REGISTERING},
    {SCCPREG_E_UNREG_REQ,            SCCPREG_SEM_UNREG_REQ,            SCCPREG_S_UNREGISTERING},
    {SCCPREG_E_REGISTERED,           SCCPREG_SEM_REGISTERED,           SCCPREG_S_REGISTERED},
    {SCCPREG_E_CLEANUP,              SCCPREG_SEM_CLEANUP,              SCCPREG_S_IDLE}
};

static sem_events_t sccpreg_sem_s_registered[] =
{
    {SCCPREG_E_REG_REQ,              SCCPREG_SEM_XXX_REG_REQ,          SCCPREG_S_REGISTERED},
    {SCCPREG_E_UNREG_REQ,            SCCPREG_SEM_UNREG_REQ,            SCCPREG_S_UNREGISTERING},
    {SCCPREG_E_CLEANUP,              SCCPREG_SEM_CLEANUP,              SCCPREG_S_IDLE}
};

static sem_events_t sccpreg_sem_s_unregistering[] =
{
    {SCCPREG_E_REG_REQ,              SCCPREG_SEM_XXX_REG_REQ,          SCCPREG_S_UNREGISTERING},
    {SCCPREG_E_UNREGISTER_ACK,       SCCPREG_SEM_UNREGISTER_ACK,       SCCPREG_S_UNREGISTERING},
    {SCCPREG_E_REGISTERED,           SCCPREG_SEM_REGISTERED,           SCCPREG_S_REGISTERED},
    {SCCPREG_E_CLEANUP,              SCCPREG_SEM_CLEANUP,              SCCPREG_S_IDLE}
};

static sem_states_t sccpreg_sem_states[] =
{
    {sccpreg_sem_s_idle,          sizeof(sccpreg_sem_s_idle)          / SEM_EVENTS_SIZE},
    {sccpreg_sem_s_registering,   sizeof(sccpreg_sem_s_registering)   / SEM_EVENTS_SIZE},
    {sccpreg_sem_s_registered,    sizeof(sccpreg_sem_s_registered)    / SEM_EVENTS_SIZE},
    {sccpreg_sem_s_unregistering, sizeof(sccpreg_sem_s_unregistering) / SEM_EVENTS_SIZE},
};

static sem_tbl_t sccpreg_sem_tbl = {
    sccpreg_sem_states, SCCPREG_S_MAX, SCCPREG_E_MAX
};

sem_table_t sccpreg_sem_table = 
{
    &sccpreg_sem_tbl,        sccpreg_sem_functions,
    sccpreg_state_name,      sccpreg_event_name,
    sccpreg_sem_name,        sccpreg_function_name,
    sccpreg_debug_sem_entry, sccpreg_validate_cb,
    (sem_change_state_f *)sccpreg_change_state
};


static char *sccpreg_sem_name (int id)
{
    return (SCCPREG_ID);
}

static char *sccpreg_state_name (int id)
{
    if ((id <= SCCPREG_S_MIN) || (id >= SCCPREG_S_MAX)) {
        return (SCCP_UNDEFINED);
    }

    return (sccpreg_state_names[id]);
}

static char *sccpreg_event_name (int id)
{
    if ((id <= SCCPREG_E_MIN) || (id >= SCCPREG_E_MAX)) {
        return (SCCP_UNDEFINED);
    }

    return (sccpreg_event_names[id]);
}

static char *sccpreg_function_name (int id)
{
    if ((id <= SCCPREG_SEM_MIN) || (id >= SCCPREG_SEM_MAX)) {
        return (SCCP_UNDEFINED);
    }

    return (sccpreg_sem_function_names[id]);
}

static void sccpreg_debug_sem_entry (sem_event_t *event)
{
    sccpreg_regcb_t *regcb = (sccpreg_regcb_t *)(event->cb);
    
    SCCP_DBG((sccpreg_debug, 9, "%s %-2d:%-2d: %-20s: %s <- %s\n",
                     SCCPREG_ID, regcb->sccpcb->id,
                     regcb->id,
                     event->function_name,
                     sccpreg_state_name(event->state_id),
                     sccpreg_event_name(event->event_id)));
}

static int sccpreg_validate_cb (void *scb, void *cb)
{
    return (((sccp_sccpcb_t *)scb)->regcb == cb ? 0 : 1);
}

static void sccpreg_change_state (sccpreg_regcb_t *regcb, int new_state,
                                  char *fname)
{
    SCCP_DBG((sccpreg_debug, 9, "%s %-2d:%-2d: %-20s: %s -> %s\n",
                     SCCPREG_ID, regcb->sccpcb->id, regcb->id, fname,
                     sccpreg_state_name(regcb->state),
                     sccpreg_state_name(new_state)));

    regcb->old_state = regcb->state;
    regcb->state = new_state;
}

static int sccpreg_get_new_regcb_id (void)
{
    static int id = 0;

    if (++id < 0) {
        id = 1;
    }

    return (id);
}

static void sccpreg_init_regcb (sccpreg_regcb_t *regcb, int id, 
                                sccp_sccpcb_t *sccpcb)
{
#if 0
    if (regcb->softkeys != NULL) {
        free(regcb->softkeys);
    }

    if (regcb->softkeysets != NULL) {
        free(regcb->softkeysets);
    }
#endif
    SCCP_MEMSET(regcb, 0, sizeof(*regcb));

    regcb->sccpcb = sccpcb;
    //regcb->id     = sccpreg_get_new_regcb_id();
    regcb->id     = sccpcb->id;
}

void sccpreg_free_regcb (sccpreg_regcb_t *regcb)
{
    if (regcb != NULL) {
        SCCP_FREE(regcb);
    }
}

void *sccpreg_get_new_regcb (sccp_sccpcb_t *sccpcb)

{
    char            *fname = "get_new_regcb";
    sccpreg_regcb_t *regcb;

    regcb = (sccpreg_regcb_t *)(SCCP_MALLOC(sizeof(*regcb)));
    if (regcb == NULL) {
        return (NULL);
    }

    SCCP_MEMSET(regcb, 0, sizeof(*regcb));

    regcb->sccpcb = sccpcb;
    regcb->id     = sccpreg_get_new_regcb_id();

    SCCP_DBG((sccpreg_debug, 9, "%s %-2d:%-2d: %-20s: regcb= 0x%08x\n",
             SCCPREG_ID, regcb->sccpcb->id, regcb->id, fname, regcb));
                      
    return (regcb);
}

gapi_status_data_info_t *sccpreg_get_session_data (sccp_sccpcb_t *sccpcb)
{
    return ((gapi_status_data_info_t *)(&(sccpcb->regcb->linecnt)));
}

/*
 * FUNCTION:    sccpreg_process_buttons
 *
 * DESCRIPTION: Determine whether to request more button information
 *              or to finish registration.
 * 
 * PARAMETERS:
 *     event:   event control block
 *
 * RETURNS:     0 for success, otherwise 1.
 *
 * NOTES:
 */
static void sccpreg_process_buttons (sem_event_t *event)
{
    sccpreg_regcb_t *regcb   = (sccpreg_regcb_t *)(event->cb);

    if (regcb->templinecnt < regcb->linecnt) {
        sccpmsg_send_line_stat_req(&sccpreg_msgcb, regcb->socket,
                                   ++(regcb->templinecnt));
    }
    else if (regcb->tempspeeddialcnt < regcb->speeddialcnt) {
        sccpmsg_send_speeddial_stat_req(&sccpreg_msgcb, regcb->socket,
                                        ++(regcb->tempspeeddialcnt));
    }
    else if (regcb->tempfeaturecnt < regcb->featurecnt) {
        sccpmsg_send_feature_stat_req(&sccpreg_msgcb, regcb->socket,
                                      ++(regcb->tempfeaturecnt));
    }
    else if (regcb->tempserviceurlcnt < regcb->serviceurlcnt) {
        sccpmsg_send_service_url_stat_req(&sccpreg_msgcb, regcb->socket,
                                          ++(regcb->tempserviceurlcnt));
    }
    else {
        sccpmsg_send_register_available_lines(&sccpreg_msgcb, regcb->socket,
                                              regcb->linecnt);

        sccpmsg_send_bare_message(&sccpreg_msgcb, regcb->socket,
                                  SCCPMSG_TIME_DATE_REQ);

        sccpcm_push_registered(regcb->sccpcb, SCCPCM_E_REGISTERED,
                               regcb->index);

        sccpreg_push_simple(regcb->sccpcb, SCCPREG_E_REGISTERED);
    }
}

static int sccpreg_push_simple (sccp_sccpcb_t *sccpcb, int msg_id)
{
    return (sccp_push_event(sccpcb, NULL, 0, msg_id,
                            &sccpreg_sem_table, sccpcb->regcb, 0));
}

int sccpreg_push_reg_req (sccp_sccpcb_t *sccpcb, int msg_id)
{
    return (sccp_push_event(sccpcb, NULL, 0, msg_id,
                            &sccpreg_sem_table, sccpcb->regcb, 0));
}

int sccpreg_push_unreg_req (sccp_sccpcb_t *sccpcb, int msg_id)
{
    return (sccp_push_event(sccpcb, NULL, 0, msg_id,
                            &sccpreg_sem_table, sccpcb->regcb, 0));
}

int sccpreg_push_unregister_ack (sccp_sccpcb_t *sccpcb, int msg_id,
                                 sccpmsg_unregister_status_t status)
{
    sccpmsg_general_t        *gen_msg;
    sccpmsg_unregister_ack_t *msg;

//    gen_msg = (sccpmsg_general_t *)SCCP_MALLOC(sizeof(*gen_msg));
    gen_msg = (sccpmsg_general_t *)SCCP_MALLOC(sizeof(*msg) +
                                               sizeof(sccpmsg_base_t));

    if (gen_msg == NULL) {
        return (2);
    }

//    SCCP_MEMSET(gen_msg, 0, sizeof(*gen_msg));
    SCCP_MEMSET(gen_msg, 0, sizeof(*msg) + sizeof(sccpmsg_base_t));

    msg = &(gen_msg->body.unregister_ack);

    msg->message_id = SCCPMSG_UNREGISTER_ACK;
    msg->status     = status;

    return (sccp_push_event(sccpcb, gen_msg, 0, msg_id,
                            &sccpreg_sem_table, sccpcb->regcb, 0));
}

/*
 * FUNCTION:    sccpreg_sem_default
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
 * S_ALL                     E_DONE                        NO CHANGE
 * ----------------------------------------------------------------------------
 */
static int sccpreg_sem_default (sem_event_t *event)
{
    return (0);
}

/*
 * FUNCTION:    sccpreg_sem_cleanup
 *
 * DESCRIPTION: Cleanup the sem.
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
static int sccpreg_sem_cleanup (sem_event_t *event)
{
    sccpreg_regcb_t *regcb = (sccpreg_regcb_t *)(event->cb);

    sccpreg_init_regcb(regcb, regcb->id, regcb->sccpcb);

    return (0);
}

/*
 * FUNCTION:    sccpreg_sem_xxx_reg_req
 *
 * DESCRIPTION: Request to register while in a non-idle state. Begin
 *              unregistration procedures and requeue the original reg_req
 *              to be processed after the unreg_req.
 * 
 * PARAMETERS:
 *     event:   event control block
 *
 * RETURNS:     0 for success, otherwise 1.
 *
 * NOTES:
 *
 * ============================================================================
 * S_REGISTERING             E_REG_REQ                     S_REGISTERING
 * ----------------------------------------------------------------------------
 * S_REGISTERED              E_REG_REQ                     S_REGISTERED  
 * ----------------------------------------------------------------------------
 * S_UNREGISTERING           E_REG_REQ                     S_UNREGISTERING
 * ----------------------------------------------------------------------------
 */
static int sccpreg_sem_xxx_reg_req (sem_event_t *event)
{
    sccpreg_regcb_t *regcb = (sccpreg_regcb_t *)(event->cb);

#if 0
    /*
	 * Push the unreg_req to sccpreg so we unregister.
	 */
    sccpreg_push_unreg_req(regcb->sccpcb, SCCPREG_E_UNREG_REQ);

	/*
	 * Simulate an unregister_ack from the CCM since we don't really
	 * want to wait for the ack.
	 */
    sccpreg_push_unregister_ack(regcb->sccpcb, SCCPREG_E_UNREGISTER_ACK,
                                SCCPMSG_UNREGISTER_STATUS_OK);

#if 0 /* this will be sent when the unregister_ack is received. */
    sccpcm_push_unreg_ack(regcb->sccpcb, SCCPCM_E_UNREG_ACK,
                          regcb->index, 0);
#endif

    sccpreg_push_simple(regcb->sccpcb, SCCPREG_E_CLEANUP);

	/*
	 * Requeue the reg_req.
	 */
    sccpreg_push_reg_req(regcb->sccpcb, SCCPREG_E_REG_REQ);
#endif
    
    sccpmsg_send_bare_message(&sccpreg_msgcb, regcb->socket,
                              SCCPMSG_UNREGISTER);

    sccpreg_push_simple(regcb->sccpcb, SCCPREG_E_CLEANUP);

	/*
	 * Requeue the reg_req.
	 */
    sccpreg_push_reg_req(regcb->sccpcb, SCCPREG_E_REG_REQ);

    return (0);
}

/*
 * FUNCTION:    sccpreg_sem_reg_req
 *
 * DESCRIPTION: Request to register. Make sure that there is a primary
 *              connection and begin registration.
 * 
 * PARAMETERS:
 *     event:   event control block
 *
 * RETURNS:     0 for success, otherwise 1.
 *
 * NOTES:
 *
 * ============================================================================
 * S_IDLE                    E_REG_REQ                     S_REGISTERING
 * ----------------------------------------------------------------------------
 */
static int sccpreg_sem_reg_req (sem_event_t *event)
{
    sccpreg_regcb_t *regcb = (sccpreg_regcb_t *)(event->cb);
    char            text[SCCPMSG_MAX_ALARM_TEXT_SIZE];
    char            *mac = sccprec_get_mac(regcb->sccpcb);
    char            device_name[SCCPMSG_DEVICE_NAME_SIZE];
    
	/*
	 * Make sure that we are still connected to a ccm.
	 */
    regcb->socket = sccpcm_get_primary_socket(regcb->sccpcb);
    if (regcb->socket == 0) {
        sccpcm_push_reg_nak(regcb->sccpcb, SCCPCM_E_REG_NAK,
                            regcb->index, NULL);

        sccpreg_push_simple(regcb->sccpcb, SCCPREG_E_CLEANUP);

        return (0);
    }

    regcb->index = sccprec_get_index(regcb->sccpcb);

    sccpmsg_create_alarm_string(text, regcb->sccpcb->reccb->tcp_close_cause,
                                mac);

    sccpmsg_send_alarm(&sccpreg_msgcb, regcb->socket,
                       SCCPMSG_ALARM_SEVERITY_INFORMATIONAL,
                       SCCPMSG_ALARM_PARAM1,
                       SCCP_SOCKET_GETSOCKNAME(regcb->socket),
                       text);

    SCCP_SNPRINTF((device_name, sizeof(device_name),
                   "%s%s",
                   SCCPMSG_FIRMWARE_STRING,
                   mac));

    sccpmsg_send_register(&sccpreg_msgcb, regcb->socket, device_name,
                          regcb->sccpcb->reccb->device_type,
                          SCCPMSG_PROTOCOL_VERSION_CURRENT,
                          SCCP_SOCKET_GETSOCKNAME(regcb->socket));

    if (regcb->sccpcb->version < SCCPMSG_VERSION_SEAVIEW) {
        sccpmsg_send_ip_port(&sccpreg_msgcb, regcb->socket, 0);
    }

    /*
     * Reset the tcp close cause. The value should be reset each alarm cycle.
     */
    sccprec_set_tcp_close_cause(regcb->sccpcb,
                                SCCPMSG_ALARM_LAST_INITIALIZED);

    return (0);
}

/*
 * FUNCTION:    sccpreg_sem_register_ack
 *
 * DESCRIPTION: Registration request has been accepted by the CCM. Inform the
 *              sccpcm and proceed with registration.
 * 
 * PARAMETERS:
 *     event:   event control block
 *
 * RETURNS:     0 for success, otherwise 1.
 *
 * NOTES:
 *
 * ============================================================================
 * S_REGISTERING             E_REGISTER_ACK                S_REGISTERING
 * ----------------------------------------------------------------------------
 */
static int sccpreg_sem_register_ack (sem_event_t *event)
{
    sccpreg_regcb_t        *regcb   = (sccpreg_regcb_t *)(event->cb);
    sccpmsg_general_t      *gen_msg = (sccpmsg_general_t *)(event->data);
    sccpmsg_register_ack_t *msg     = &(gen_msg->body.register_ack);
    
    sccpcm_push_reg_ack(regcb->sccpcb, SCCPCM_E_REG_ACK,
                        regcb->index, 0,
                        msg->keepalive_interval_1,
                        msg->keepalive_interval_2);

    if (regcb->sccpcb->version > SCCPMSG_VERSION_HAWKBILL) {
        sccpmsg_send_headset_status(&sccpreg_msgcb, regcb->socket,
                                    SCCPMSG_HEADSET_STATUS_OFF);
    }

    sccpmsg_send_bare_message(&sccpreg_msgcb, regcb->socket,
                              SCCPMSG_BUTTON_TEMPLATE_REQ);
  
    return (0);
}

/*
 * FUNCTION:    sccpreg_sem_register_rej
 *
 * DESCRIPTION: Registration request has been rejected by the CCM. Inform the 
 *              sccpcm and cleanup.
 * 
 * PARAMETERS:
 *     event:   event control block
 *
 * RETURNS:     0 for success, otherwise 1.
 *
 * NOTES:
 *
 * ============================================================================
 * S_REGISTERING             E_REGISTER_REJ                S_UNREGISTERING
 * ----------------------------------------------------------------------------
 */
static int sccpreg_sem_register_rej (sem_event_t *event)
{
    sccpreg_regcb_t *regcb = (sccpreg_regcb_t *)(event->cb);

    sccpcm_push_reg_nak(regcb->sccpcb, SCCPCM_E_REG_NAK,
                        regcb->index, NULL);

    sccpreg_push_simple(regcb->sccpcb, SCCPREG_E_CLEANUP);

    return (0);
}

/*
 * FUNCTION:    sccpreg_sem_capabilities_req
 *
 * DESCRIPTION: Return capabilities to CCM.
 * 
 * PARAMETERS:
 *     event:   event control block
 *
 * RETURNS:     0 for success, otherwise 1.
 *
 * NOTES:
 *
 * ============================================================================
 * S_REGISTERING             E_CAPABILITIES_REQ            S_REGISTERING
 * ----------------------------------------------------------------------------
 */
static int sccpreg_sem_capabilities_req (sem_event_t *event)
{
    sccpreg_regcb_t *regcb = (sccpreg_regcb_t *)(event->cb);
    
    sccpmsg_send_capabilities_response(&sccpreg_msgcb, regcb->socket,
                                       sccprec_get_media_caps(regcb->sccpcb));

    return (0);
}

/*
 * FUNCTION:    sccpreg_sem_button_template_res
 *
 * DESCRIPTION: Copy the button template.
 * 
 * PARAMETERS:
 *     event:   event control block
 *
 * RETURNS:     0 for success, otherwise 1.
 *
 * NOTES:
 *
 * ============================================================================
 * S_REGISTERING             E_BUTTON_TEMPLATE_RES         S_REGISTERING
 * ----------------------------------------------------------------------------
 */
static int sccpreg_sem_button_template_res (sem_event_t *event)
{
    sccpreg_regcb_t           *regcb   = (sccpreg_regcb_t *)(event->cb);
    sccpmsg_general_t         *gen_msg = (sccpmsg_general_t *)(event->data);
    sccpmsg_button_template_t *msg     = &(gen_msg->body.button_template);
    unsigned long             i;
    unsigned long             instance;
    
    /*
     * Copy each button description.
     */
    for (i = msg->button_template.button_offset;
         (msg->button_template.button_count)-- > 0; i++) {

         if (i >= msg->button_template.total_button_count) {
             break;
         }

        /*
         * Translate the instance to internal. instance starts at 1, but
         * internally it is 0.
         */
        instance = msg->button_template.buttons[i].instance - 1;

        /*
         * The button_template message includes both directory number
         * and speeddial values, so we need to look at each and copy them
         * to the right spot.
         * 
         * We also increment seperate counters for each type. These
         * counters will be used to track if all button definitions have
         * been received and later to request info for each button defined.
         */
        switch (msg->button_template.buttons[i].definition) {
        case SCCPMSG_DEVICE_STIMULUS_LINE:
            if (instance < SCCPMSG_MAX_LINES) {
//                regcb->lines[instance].instance = instance;
                regcb->linecnt++;
            }

            break;

        case SCCPMSG_DEVICE_STIMULUS_SPEED_DIAL:
            if (instance < SCCPMSG_MAX_LINES) {
//                regcb->speeddials[instance].instance = instance;
                regcb->speeddialcnt++;
            }
            
            break;

        case SCCPMSG_DEVICE_STIMULUS_PRIVACY:
            if (instance < SCCPMSG_MAX_FEATURES) {
//                regcb->features[instance].instance = instance;
                regcb->featurecnt++;
            }

            break;

        case SCCPMSG_DEVICE_STIMULUS_SERVICE_URL:
            if (instance < SCCPMSG_MAX_SERVICE_URLS) {
//                regcb->serviceurls[instance].instance = instance;
                regcb->serviceurlcnt++;
            }

            break;

        default:
            regcb->misccnt++;
            break;
        }
    }

    SCCP_DBG((sccpreg_debug, 9,
             "%s %-2d:%-2d: %-20s: linecnt= %d, speeddialcnt= %d, "
             " serviceeurlcnt= %d, featurecnt= %d, misccnt= %d\n",
             SCCPREG_ID, regcb->sccpcb->id, regcb->id, event->function_name,
             regcb->linecnt, regcb->speeddialcnt,
             regcb->serviceurlcnt, regcb->featurecnt, regcb->misccnt));

    /*
     * Request the softkey template only if we have received
     * all button definitions.
     */
    if ((regcb->linecnt + regcb->speeddialcnt +
         regcb->serviceurlcnt + regcb->featurecnt + regcb->misccnt) ==
        (int)(msg->button_template.total_button_count)) {

        sccpmsg_send_bare_message(&sccpreg_msgcb, regcb->socket,
                                  SCCPMSG_SOFTKEY_TEMPLATE_REQ);
    }

    return (0);
}

/*
 * FUNCTION:    sccpreg_sem_softkey_template_res
 *
 * DESCRIPTION: Copy the softkey template.
 * 
 * PARAMETERS:
 *     event:   event control block
 *
 * RETURNS:     0 for success, otherwise 1.
 *
 * NOTES:
 *
 * ============================================================================
 * S_REGISTERING             E_SOFTKEY_TEMPLATE_RES        S_REGISTERING
 * ----------------------------------------------------------------------------
 */
static int sccpreg_sem_softkey_template_res (sem_event_t *event)
{
    sccpreg_regcb_t                *regcb   = (sccpreg_regcb_t *)(event->cb);
    sccpmsg_general_t              *gen_msg = (sccpmsg_general_t *)(event->data);
    sccpmsg_softkey_template_res_t *msg     = &(gen_msg->body.softkey_template_res);
    //unsigned long                  i;
    //sccpmsg_softkey_template_definition_t *softkey = &(msg->softkey_template.definition[0]);

#if 0
    for (i = msg->softkey_template.offset;
         (msg->softkey_template.count)-- > 0; i++) {

        /*
         * Make sure we still have space for the data.
         */
        if ((i >= SCCPMSG_MAX_SOFTKEY_DEFINITIONS) ||
            (i >= msg->softkey_template.total_count)) {
            
            break;
        }

        SCCP_STRNCPY(regcb->softkeys[i].label,
                     softkey->label,
                     SCCPMSG_SOFTKEY_LABEL_SIZE);
        regcb->softkeys[i].label[SCCPMSG_SOFTKEY_LABEL_SIZE - 1] = '\0';

        regcb->softkeys[i].event = softkey->event;
        
        softkey++;
        regcb->softkeycnt++;
    }
#endif
    regcb->softkeycnt += msg->softkey_template.count;

    SCCP_SOFTKEYTEMPLATE_RES(regcb, GAPI_MSG_SOFTKEYTEMPLATE_RES,
                            (gapi_softkey_template_t *)(&(msg->softkey_template)));

    SCCP_DBG((sccpreg_debug, 9, "%s %-2d:%-2d: %-20s: softkeys= %d\n",
                     SCCPREG_ID, regcb->sccpcb->id, regcb->id, event->function_name,
                     regcb->softkeycnt));

    /*
     * Request the softkey set only if we have received all softkey
     * templates.
     */   
    if (regcb->softkeycnt == (int)(msg->softkey_template.total_count)) {
        sccpmsg_send_bare_message(&sccpreg_msgcb, regcb->socket,
                                  SCCPMSG_SOFTKEY_SET_REQ);
    }

    return (0);
}

/*
 * FUNCTION:    sccpreg_sem_softkey_set_res
 *
 * DESCRIPTION: Copy the softkey set.
 * 
 * PARAMETERS:
 *     event:   event control block
 *
 * RETURNS:     0 for success, otherwise 1.
 *
 * NOTES:
 *
 * ============================================================================
 * S_REGISTERING             E_SOFTKEY_SET_RES             S_REGISTERING
 * ----------------------------------------------------------------------------
 */
static int sccpreg_sem_softkey_set_res (sem_event_t *event)
{
    sccpreg_regcb_t           *regcb   = (sccpreg_regcb_t *)(event->cb);
    sccpmsg_general_t         *gen_msg = (sccpmsg_general_t *)(event->data);
    sccpmsg_softkey_set_res_t *msg     = &(gen_msg->body.softkey_set_res);
    //unsigned long             i, j;

#if 0
    for (i = msg->softkey_set.offset; (msg->softkey_set.count)-- > 0; i++) {
        /*
         * Make sure we still have space for the data.
         */
        if ((i >= SCCPMSG_MAX_SOFTKEY_SET_DEFINITIONS) ||
            (i >= (int)(msg->softkey_set.total_count))) {
            break;
        }

        for (j = 0; j < SCCPMSG_MAX_SOFTKEY_INDEXES; j++) {
            if (msg->softkey_set.definition[i].template_index[j] != 0) {
                regcb->softkeysets[i].template_index[j] =
                    msg->softkey_set.definition[i].template_index[j];

                regcb->softkeysets[i].info_index[j] =
                    msg->softkey_set.definition[i].info_index[j];

            }
        }

        regcb->softkeysetcnt++;
    }

#endif
    regcb->softkeysetcnt += msg->softkey_set.count;

    SCCP_SOFTKEYSET_RES(regcb, GAPI_MSG_SOFTKEYSET_RES,
                       (gapi_softkey_set_t *)(&(msg->softkey_set)));

    SCCP_DBG((sccpreg_debug, 9, "%s %-2d:%-2d: %-20s: softkeysets= %d\n",
                     SCCPREG_ID, regcb->sccpcb->id, regcb->id, event->function_name,
                     regcb->softkeysetcnt));

     /*
     * Request line enumeration only after we have all softkey sets.
     */
    if (regcb->softkeysetcnt == (int)(msg->softkey_set.total_count)) {
        sccpreg_process_buttons(event);
    }

    return (0);
}

/*
 * FUNCTION:    sccpreg_sem_line_stat_res
 *
 * DESCRIPTION: Copy the line stats.
 * 
 * PARAMETERS:
 *     event:   event control block
 *
 * RETURNS:     0 for success, otherwise 1.
 *
 * NOTES:
 *
 * ============================================================================
 * S_REGISTERING             E_LINE_STAT_RES               S_REGISTERING
 * ----------------------------------------------------------------------------
 */
static int sccpreg_sem_line_stat_res (sem_event_t *event)
{
    sccpreg_regcb_t     *regcb   = (sccpreg_regcb_t *)(event->cb);
    sccpmsg_general_t   *gen_msg = (sccpmsg_general_t *)(event->data);
    sccpmsg_line_stat_t *msg     = &(gen_msg->body.line_stat);
    unsigned long       line     = msg->line_number - 1;
//    gapi_line_t         gapi_line;
    
    /*
     * Make sure the line was defined in the template.
     */
    if ((int)line > regcb->linecnt) {
        sccpreg_process_buttons(event);

        return (0);
    }

#if 0
    gapi_line.instance = line;

    SCCP_STRNCPY(gapi_line.dn,
                 msg->line_directory_number,
                 SCCPMSG_DIRECTORY_NUMBER_SIZE);
    gapi_line.dn[SCCPMSG_DIRECTORY_NUMBER_SIZE - 1] = '\0';

    SCCP_STRNCPY(gapi_line.fqdn,
                 msg->line_fully_qualified_display_name,
                 SCCPMSG_DIRECTORY_NAME_SIZE);
    gapi_line.fqdn[SCCPMSG_DIRECTORY_NAME_SIZE - 1] = '\0';

    if (regcb->sccpcb->version < SCCPMSG_VERSION_HAWKBILL) {
        gapi_line.label[0] = '\0';
    }
    else {
        SCCP_STRNCPY(gapi_line.label,
                     msg->line_text_label,
                     SCCPMSG_DIRECTORY_NAME_SIZE);
        gapi_line.label[SCCPMSG_DIRECTORY_NAME_SIZE - 1] = '\0';
    }

    if (regcb->sccpcb->version < SCCPMSG_VERSION_PARCHE) {
        gapi_line.flags |= (SCCPMSG_DISPLAY_OPTIONS_ODN |
                            SCCPMSG_DISPLAY_OPTIONS_CLID |
                            SCCPMSG_DISPLAY_OPTIONS_CNID);
    }
    else {
        if (msg->display_options & SCCPMSG_DISPLAY_OPTIONS_ODN) {
            gapi_line.flags |= SCCPMSG_DISPLAY_OPTIONS_ODN;
        }

        if (msg->display_options & SCCPMSG_DISPLAY_OPTIONS_RDN) {
            gapi_line.flags |= SCCPMSG_DISPLAY_OPTIONS_RDN;
        }

        if (msg->display_options & SCCPMSG_DISPLAY_OPTIONS_CLID) {
            gapi_line.flags |= SCCPMSG_DISPLAY_OPTIONS_CLID;
        }
        
        if (msg->display_options & SCCPMSG_DISPLAY_OPTIONS_CNID) {
            gapi_line.flags |= SCCPMSG_DISPLAY_OPTIONS_CNID;
        }
    }
    
    SCCP_LINESTAT_RES(regcb, GAPI_MSG_LINESTAT_RES, &gapi_line);
#endif

    SCCP_DBG((sccpreg_debug, 9, "%s %-2d:%-2d: %-20s: %d: dn= %s, fqdn= %s\n",
             SCCPREG_ID, regcb->sccpcb->id, regcb->id, event->function_name,
             msg->line_directory_number,
             msg->line_fully_qualified_display_name));

    SCCP_LINESTAT_RES(regcb, GAPI_MSG_LINESTAT_RES,
                      (gapi_line_t *)(&(msg->line_number)));

    sccpreg_process_buttons(event);

    return (0);
}

/*
 * FUNCTION:    sccpreg_sem_speeddial_stat_res
 *
 * DESCRIPTION: Copy the speeddial stats.
 * 
 * PARAMETERS:
 *     event:   event control block
 *
 * RETURNS:     0 for success, otherwise 1.
 *
 * NOTES:
 *
 * ============================================================================
 * S_REGISTERING             E_SPEEDDIAL_STAT_RES          S_REGISTERING
 * ----------------------------------------------------------------------------
 */
static int sccpreg_sem_speeddial_stat_res (sem_event_t *event)
{
    sccpreg_regcb_t           *regcb   = (sccpreg_regcb_t *)(event->cb);
    sccpmsg_general_t         *gen_msg = (sccpmsg_general_t *)(event->data);
    sccpmsg_speeddial_stat_t  *msg     = &(gen_msg->body.speeddial_stat);
    unsigned long              line    = msg->speeddial_number - 1;
//    gapi_speeddial_t           gapi_speeddial;
    
    /*
     * Make sure the line was defined in the template.
     */
    if ((int)line > regcb->speeddialcnt) {
        sccpreg_process_buttons(event);

        return (0);
    }
#if 0
    gapi_speeddial.instance = line;

    SCCP_STRNCPY(gapi_speeddial.dn,
                 msg->speeddial_directory_number,
                 SCCPMSG_DIRECTORY_NUMBER_SIZE);
    gapi_speeddial.dn[SCCPMSG_DIRECTORY_NUMBER_SIZE - 1] = '\0';

    SCCP_STRNCPY(gapi_speeddial.fqdn,
                 msg->speeddial_display_name,
                 SCCPMSG_DIRECTORY_NAME_SIZE);
    gapi_speeddial.fqdn[SCCPMSG_DIRECTORY_NAME_SIZE - 1] = '\0';
    
    SCCP_SPEEDDIALSTAT_RES(regcb, GAPI_MSG_SPEEDDIALSTAT_RES, &gapi_speeddial);
#endif    
    SCCP_SPEEDDIALSTAT_RES(regcb, GAPI_MSG_SPEEDDIALSTAT_RES,
                           (gapi_speeddial_t *)(&(msg->speeddial_number)));

    sccpreg_process_buttons(event);

    return (0);
}

/*
 * FUNCTION:    sccpreg_sem_feature_stat_res
 *
 * DESCRIPTION: Copy the feature stats.
 * 
 * PARAMETERS:
 *     event:   event control block
 *
 * RETURNS:     0 for success, otherwise 1.
 *
 * NOTES:
 *
 * ============================================================================
 * S_REGISTERING             E_FEATURE_STAT_RES            S_REGISTERING
 * ----------------------------------------------------------------------------
 */
static int sccpreg_sem_feature_stat_res (sem_event_t *event)
{
    sccpreg_regcb_t        *regcb   = (sccpreg_regcb_t *)(event->cb);
    sccpmsg_general_t      *gen_msg = (sccpmsg_general_t *)(event->data);
    sccpmsg_feature_stat_t *msg     = &(gen_msg->body.feature_stat);
    unsigned long          line     = msg->feature_index - 1;
//    gapi_feature_t         gapi_feature;
    
    /*
     * Make sure the line was defined in the template.
     */
    if ((int)line > regcb->featurecnt) {
        sccpreg_process_buttons(event);

        return (0);
    }
#if 0
    gapi_feature.instance = line;

    SCCP_STRNCPY(gapi_feature.label,
                 msg->feture_text_label,
                 SCCPMSG_DIRECTORY_NUMBER_SIZE);
    gapi_feature.label[SCCPMSG_DIRECTORY_NUMBER_SIZE - 1] = '\0';

    gapi_feature.id     = msg->feature_id;
    gapi_feature.status = msg->feature_status;

    SCCP_FEATURESTAT_RES(regcb, GAPI_MSG_FEATURESTAT_RES, &gapi_feature);
#endif
    SCCP_FEATURESTAT_RES(regcb, GAPI_MSG_FEATURESTAT_RES,
                         (gapi_feature_t *)(&(msg->feature_index)));

    sccpreg_process_buttons(event);

    return (0);
}

/*
 * FUNCTION:    sccpreg_sem_serviceurl_stat_res
 *
 * DESCRIPTION: Copy the service_url stats.
 * 
 * PARAMETERS:
 *     event:   event control block
 *
 * RETURNS:     0 for success, otherwise 1.
 *
 * NOTES:
 *
 * ============================================================================
 * S_REGISTERING             E_SERVICEURL_STAT_RES         S_REGISTERING
 * ----------------------------------------------------------------------------
 */
static int sccpreg_sem_serviceurl_stat_res (sem_event_t *event)
{
    sccpreg_regcb_t            *regcb   = (sccpreg_regcb_t *)(event->cb);
    sccpmsg_general_t          *gen_msg = (sccpmsg_general_t *)(event->data);
    sccpmsg_service_url_stat_t *msg     = &(gen_msg->body.service_url_stat);
    unsigned long              line     = msg->service_url_index - 1;
//    gapi_serviceurl_t          gapi_serviceurl;
    
    /*
     * Make sure the line was defined in the template.
     */
    if ((int)line > regcb->serviceurlcnt) {
        sccpreg_process_buttons(event);

        return (0);
    }

#if 0
    gapi_serviceurl.instance = line;

    SCCP_STRNCPY(gapi_serviceurl.url,
                 msg->service_url,
                 SCCPMSG_MAX_SERVICE_URL_SIZE);
    gapi_serviceurl.url[SCCPMSG_MAX_SERVICE_URL_SIZE - 1] = '\0';

    SCCP_STRNCPY(gapi_serviceurl.display_name,
                 msg->service_url_display_name,
                 SCCPMSG_DIRECTORY_NAME_SIZE);
    gapi_serviceurl.display_name[SCCPMSG_DIRECTORY_NAME_SIZE - 1] = '\0';

    SCCP_SERVICEURLSTAT_RES(regcb, GAPI_MSG_SERVICEURLSTAT_RES, &gapi_serviceurl);
#endif
    
    SCCP_SERVICEURLSTAT_RES(regcb, GAPI_MSG_SERVICEURLSTAT_RES,
                            (gapi_serviceurl_t *)(&(msg->service_url_index)));

    sccpreg_process_buttons(event);

    return (0);
}

/*
 * FUNCTION:    sccpreg_sem_registered
 *
 * DESCRIPTION: Registration is complete. Move to the registered state.
 * 
 * PARAMETERS:
 *     event:   event control block
 *
 * RETURNS:     0 for success, otherwise 1.
 *
 * NOTES:
 *
 * ============================================================================
 * S_REGISTERING             E_REGISTERED                  S_REGISTERED
 * ----------------------------------------------------------------------------
 * S_UNREGISTERING           E_REGISTERED                  S_REGISTERED
 * ----------------------------------------------------------------------------
 */
static int sccpreg_sem_registered (sem_event_t *event)
{
    return (0);
}

/*
 * FUNCTION:    sccpreg_sem_unreg_req
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
 * S_ALL                     E_UNREG_REQ                   S_UNREGISTERING
 * ----------------------------------------------------------------------------
 */
static int sccpreg_sem_unreg_req (sem_event_t *event)
{
    sccpreg_regcb_t *regcb = (sccpreg_regcb_t *)(event->cb);

    /*
     * The socket may have already been closed. If so, then just pretend 
     * we received an unregister_ack and cleanup.
     */
    if (sccpmsg_send_bare_message(&sccpreg_msgcb, regcb->socket,
                                  SCCPMSG_UNREGISTER) != 0) {
        
        sccpcm_push_unreg_ack(regcb->sccpcb, SCCPCM_E_UNREG_ACK,
                              regcb->index, 0);

        sccpreg_push_simple(regcb->sccpcb, SCCPREG_E_CLEANUP);
    }
    
    return (0);
}

/*
 * FUNCTION:    sccpreg_sem_unregister_ack
 *
 * DESCRIPTION: CCM has acknowledged the unregister request.
 * 
 * PARAMETERS:
 *     event:   event control block
 *
 * RETURNS:     0 for success, otherwise 1.
 *
 * NOTES:
 *
 * ============================================================================
 * S_UNREGISTERING           E_UNREGISTER_ACK              S_UNREGISTERING
 * ----------------------------------------------------------------------------
 */
static int sccpreg_sem_unregister_ack (sem_event_t *event)
{
    sccpreg_regcb_t          *regcb   = (sccpreg_regcb_t *)(event->cb);
    sccpmsg_general_t        *gen_msg = (sccpmsg_general_t *)(event->data);
    sccpmsg_unregister_ack_t *msg     = &(gen_msg->body.unregister_ack);
    
    switch (msg->status) {
    case (SCCPMSG_UNREGISTER_STATUS_OK):
        sccpcm_push_unreg_ack(regcb->sccpcb, SCCPCM_E_UNREG_ACK,
                              regcb->index, GAPI_CAUSE_OK);

        sccpreg_push_simple(regcb->sccpcb, SCCPREG_E_CLEANUP);
        
        break;

    default:
        /*
         * CCM will not allow the unregistration, so go back to the
         * REGISTERED state.
         */
        sccpcm_push_unreg_nak(regcb->sccpcb, SCCPCM_E_UNREG_NAK,
                              regcb->index, GAPI_CAUSE_ERROR);

        sccpreg_push_simple(regcb->sccpcb, SCCPREG_E_REGISTERED);
        
        break;
    }

    return (0);
}

/*
 * FUNCTION:    sccpreg_init
 *
 * DESCRIPTION: Initialize sccpreg.
 * 
 * PARAMETERS:
 *     event:   event control block
 *
 * RETURNS:     0 for success, otherwise 1.
 *
 * NOTES:
 */
int sccpreg_init (void)
{
    sccpreg_msgcb.cb       = NULL;
    sccpreg_msgcb.sem      = SCCP_SEM_REG;
    sccpreg_msgcb.sem_name = SCCPREG_ID;

    return (0);
}

/*
 * FUNCTION:    sccpreg_cleanup
 *
 * DESCRIPTION: Cleanup sccpreg.
 * 
 * PARAMETERS:
 *     event:   event control block
 *
 * RETURNS:     0 for success, otherwise 1.
 *
 * NOTES:
 */
int sccpreg_cleanup (void)
{
    return (0);
}
