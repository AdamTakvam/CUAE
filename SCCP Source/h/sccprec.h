/*
 *  Copyright (c) 2003, 2004 by Cisco Systems, Inc. All Rights Reserved.
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
 *     sccprec.h
 *
 *  CREATION/MODIFICATION DATES
 *     Created:  March 2003, Sam Hague
 *     Modified: 
 *
 *  DESCRIPTION
 *     SCCP Recovery header file
 */
#ifndef _SCCPREC_H_
#define _SCCPREC_H_

#include "gapi.h"
#include "sem.h"

#define SCCPREC_DEVICE_TYPE                SCCP_DEVICE_TYPE
#define SCCPREC_MAX_CM_SERVERS             5
#define SCCPREC_MAX_SRST_SERVERS           5
#define SCCPREC_PRIMARY_INDEX              0
#define SCCPREC_SECONDARY_INDEX            1
#define SCCPREC_MAX_CM_LIST_ITERATIONS     4 //2//4
#define SCCPREC_ACK_RETRIES_BEFORE_LOCKOUT 3 //1//3
#define SCCPREC_CCM_CON_RETRIES            2
#define SCCPREC_SRST_CON_RETRIES           0
#define SCCPREC_UNREG_ACK_RETRIES          1
#define SCCPREC_KA_TO_BEFORE_NO_TOKEN_REG  3
#define SCCPREC_CM_STUCK_ALARM_COUNT       6

/*
 * All timeouts (TO) are in milliseconds.
 */
#define SCCPREC_FUDGE_TO                3000
#define SCCPREC_RETRY_TOKEN_REQ_TO      5000
#define SCCPREC_RETRY_NO_CMS_DEFINED_TO 60000
#define SCCPREC_REJECT_TO               60000
#define SCCPREC_RETRY_PRI_TO            10000
#define SCCPREC_DEVICE_POLL_TO          10000 //180000 //10000/*samtest*/
#define SCCPREC_WF_CALL_END_TO          1000
#define SCCPREC_WF_CLOSE_TO             5000
#define SCCPREC_DEFAULT_CONNECT_KA_TO   60000
#define SCCPREC_WF_RESET_RES            5000
#define SCCPREC_MIN_WAIT_FOR_UNREGISTER_TO ((unsigned)(0.5 * 60000))
#define SCCPREC_MAX_WAIT_FOR_UNREGISTER_TO ((unsigned)(5.5 * 60000))
#define SCCPREC_NAK_TO_SYN_RETRY_TO    250
#define SCCPREC_LOCKOUT_TO              (10 * 60 * 1000)//(1 * 60 * 1000)//(10 * 60 * 1000)/*samtest*/
#define SCCPREC_ALL_LOCKOUT_DHCP_TO     (5 * 60 * 1000)
#define SCCPREC_ALL_LOCKOUT_TO          5000
#define SCCPREC_WF_CONNECT_TO           5000
#define SCCPREC_WF_REG_ACK_KA_TO        30000
#define SCCPREC_WF_UNREGISTER_TO        10000
#define SCCPREC_WF_REGISTER_TO          (2 * SCCPREC_WF_REG_ACK_KA_TO)


struct sccp_sccpcb_t_;

typedef enum sccprec_events_t_ {
    SCCPREC_E_MIN = -1,
    SCCPREC_E_PRI_CHECK,
    SCCPREC_E_FIND_PRI,
    SCCPREC_E_TIMEOUT,
    SCCPREC_E_DEV_NOTIFY,
    SCCPREC_E_SEC_CHECK,
    SCCPREC_E_FIND_SEC,
    SCCPREC_E_PRI_OPTIMIZE,
    SCCPREC_E_TOKEN_REQ,
    SCCPREC_E_UNREG_REQ,
    SCCPREC_E_REG_REQ,
    SCCPREC_E_SEC_OPTIMIZE,
    SCCPREC_E_DONE,
    SCCPREC_E_OPEN,
    SCCPREC_E_CLOSE,
    SCCPREC_E_CON_ACK,
    SCCPREC_E_REG_ACK,
    SCCPREC_E_UNREG_ACK,
    SCCPREC_E_RESET,
    SCCPREC_E_RESET_REQ,
    SCCPREC_E_MAX
} sccprec_events_t;

typedef enum sccprec_reset_source_t_ {
    SCCPREC_RESET_SOURCE_MIN = -1,
    SCCPREC_RESET_SOURCE_CM,
    SCCPREC_RESET_SOURCE_RESTART,
    SCCPREC_RESET_SOURCE_RESET,
    SCCPREC_RESET_SOURCE_CLOSE
} sccprec_reset_source_t;

typedef enum sccprec_session_states_t_ {
    SCCPREC_SESSION_STATE_MIN = -1,
    SCCPREC_SESSION_STATE_IDLE,
    SCCPREC_SESSION_STATE_OPEN,
    SCCPREC_SESSION_STATE_MAX
} sccprec_session_states_t;

typedef enum sccprec_reset_type_t_ {
    SCCPREC_RESET_TYPE_MIN = -1,
    SCCPREC_RESET_TYPE_RESTART,
    SCCPREC_RESET_TYPE_RESET,
    SCCPREC_RESET_TYPE_MAX
} sccprec_reset_type_t;

typedef struct sccprec_cm_t_ {
    unsigned long addr;
    unsigned short port;
} sccprec_cm_t;

typedef struct sccprec_reccb_t_ {
    struct sccprec_reccb_t_ *next;
    int id;
    int state;
    int old_state;

    struct sccp_sccpcb_t_ *sccpcb;
    
    sccpcm_cmcb_t *cmcbs[SCCPREC_MAX_CM_SERVERS];
    char mac[GAPI_MAX_MAC_SIZE];
    int pri_retries; /* tracks when we enter retry mode */
    int cm_index; /* the cm currently being processed */
    int start_index; /* starting cm index */
	int max_cm_index;
    int cm_list_iterations; /* interations over cm list */
    ssapi_timer_t misc_timer;
    int connect_keepalive_to;
    int waiting_to_find_pri;
    unsigned long keepalive_t1;
    unsigned long keepalive_t2;
    int registered_with_fallback;
    int tcp_close_cause;
    int reject;
    int free_events;
    gapi_causes_e reset_cause;
    gapi_media_caps_t media_caps;

    int version;

    enum sccprec_session_states_t_ session_state;

    /* configurable parameters */
    int default_keepalive_to;
    int device_poll_to;
    int call_end_to;
    int close_to;
    int nak_to_syn_retry_to;
    int connect_to;
    gapi_devices_t device_type;
    gapi_cc_mode_e cc_mode;
} sccprec_reccb_t;

typedef struct sccprec_msg_open_t_ {
    int msg_id;
    gapi_cmaddr_t cms[SCCPREC_MAX_CM_SERVERS];
    gapi_cmaddr_t srsts[SCCPREC_MAX_CM_SERVERS];
    gapi_srst_modes_e srst_mode;
    char mac[14];
    gapi_cmaddr_t tftp;
    gapi_media_caps_t media_caps;
    gapi_opensession_values_t values;
    gapi_protocol_versions_e version;
} sccprec_msg_open_t;

typedef struct sccprec_msg_timeout_t_ {
    int msg_id;
    int event;
} sccprec_msg_timeout_t;

typedef struct sccprec_msg_reset_req_t_ {
    int msg_id;
    gapi_causes_e cause;
} sccprec_msg_reset_req_t;

void sccprec_free_reccb(sccprec_reccb_t *reccb);
void *sccprec_get_new_reccb(struct sccp_sccpcb_t_ *sccpcb);
int sccprec_init(void);
int sccprec_cleanup(void);
char *sccprec_get_mac(struct sccp_sccpcb_t_ *sccpcb);
gapi_media_caps_t *sccprec_get_media_caps(struct sccp_sccpcb_t_ *sccpcb);
int sccprec_get_index(struct sccp_sccpcb_t_ *sccpcb);
void sccprec_set_fallback(struct sccp_sccpcb_t_ *sccpcb,
                          sccpcm_ccm_types_t ccm_type,
                          int registered);
void sccprec_set_register_reject(struct sccp_sccpcb_t_ *sccpcb, int flag);
void sccprec_set_tcp_close_cause(struct sccp_sccpcb_t_ *sccpcb, int cause);
int sccprec_get_keepalive_to(struct sccp_sccpcb_t_ *sccpcb, int whichone);
void sccprec_set_keepalive_timeout(struct sccp_sccpcb_t_ *sccpcb,
                                   unsigned long timeout_ms,
                                   int whichone);
int sccprec_push_opensession_req(struct sccp_sccpcb_t_ *sccpcb, int msg_id,
                                 gapi_cmaddr_t *cms, char *mac,
                                 gapi_cmaddr_t *srsts,
                                 gapi_srst_modes_e srst_mode,
                                 gapi_cmaddr_t *tftp,
                                 gapi_media_caps_t *media_caps,
                                 gapi_opensession_values_t *values,
                                 gapi_protocol_versions_e version);
int sccprec_push_resetsession_req(struct sccp_sccpcb_t_ *sccpcb, int msg_id,
                                  gapi_causes_e cause);
int sccprec_push_dev_notify(struct sccp_sccpcb_t_ *sccpcb, int msg_id);

#endif /* _SCCPREC_H_ */
