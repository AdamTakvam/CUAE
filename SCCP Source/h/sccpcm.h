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
 *     sccpcm.h
 *
 *  CREATION/MODIFICATION DATES
 *     Created:  March 2003, Sam Hague
 *     Modified: 
 *
 *  DESCRIPTION
 *     SCCP Call Manager header file
 */
#ifndef _SCCPCM_H_
#define _SCCPCM_H_

#include "sem.h"


#define SCCPCM_NO_CMCB  (NULL)
#define SCCPCM_NO_INDEX (-1)


struct sccp_sccpcb_t_;

typedef enum sccpcm_events_t_ {
    SCCPCM_E_MIN = -1,
    SCCPCM_E_CON_REQ,
    SCCPCM_E_CNTREG,
    SCCPCM_E_KEEPALIVE,
    SCCPCM_E_ERROR,
    SCCPCM_E_DISCON_REQ,
    SCCPCM_E_SEND_TOKEN,
    SCCPCM_E_RECV_TOKEN,
    SCCPCM_E_REG_REQ,
    SCCPCM_E_REGISTERED,
    SCCPCM_E_UNREG_REQ,
    SCCPCM_E_TIMEOUT,
    SCCPCM_E_LOCKOUT,
    SCCPCM_E_CON_ACK,
    SCCPCM_E_CON_RETRY,
    SCCPCM_E_UNREG_ACK,
    SCCPCM_E_UNREG_NAK,
    SCCPCM_E_TCP_EVENTS,
    SCCPCM_E_REG_ACK,
    SCCPCM_E_REG_NAK,
    SCCPCM_E_KEEPALIVE_ACK,
    SCCPCM_E_RESET,
    SCCPCM_E_CONNECTED,
    SCCPCM_E_TOKEN_REJ,
    SCCPCM_E_TOKEN_RETRY_TO,
    SCCPCM_E_LOCKOUT_TO,
    SCCPCM_E_KA_TO,
    SCCPCM_E_WF_REG_RES_TO,
    SCCPCM_E_MAX
} sccpcm_events_t;

typedef enum sccpcm_ccm_types_t_ {
    SCCPCM_CCM_TYPE_MIN = -1,
    SCCPCM_CCM_TYPE_STANDARD,
    SCCPCM_CCM_TYPE_STANDARD_TFTP,
    SCCPCM_CCM_TYPE_SRST_FALLBACK,
    SCCPCM_CCM_TYPE_MAX
} sccpcm_ccm_types_t;

typedef enum sccpcm_errors_t_ {
    SCCPCM_ERROR_MIN = -1,
    SCCPCM_ERROR_DR_PRI_REG_TO,
    SCCPCM_ERROR_CANT_SEND_KA,
    SCCPCM_ERROR_NULL_ADDR_PORT,
    SCCPCM_ERROR_CANT_OPEN_TCP,
    SCCPCM_ERROR_TCP_NAK,
    SCCPCM_ERROR_CANT_SEND_TOKEN,
    SCCPCM_ERROR_NO_KA_ACK,
    SCCPCM_ERROR_SHUTDOWN,
    SCCPCM_ERROR_NO_UNREG_ACK,
    SCCPCM_ERROR_CLOSE_ALL_NOW,
    SCCPCM_ERROR_TCP_CLOSE,
    SCCPCM_ERROR_REGISTER_REJECT,
    SCCPCM_ERROR_REGISTER_TO,
    SCCPCM_ERROR_MAX
} sccpcm_errors_t;

typedef struct sccpcm_chunk_data_t_ {
    unsigned char *presidual_buffer;
    unsigned char *pnext_chunk;
    int pending_bytes; /* bytes still needed to complete a message */
    int bytes_left;    /* bytes left to process from a packet */
    unsigned char *residual_buffer;
} sccpcm_chunk_data_t;

typedef struct sccpcm_cmcb_t_ {
    struct sccpcm_cmcb_t_ *next;
    int id;
    int state;
    int old_state;

    struct sccp_sccpcb_t_ *sccpcb;

    ssapi_timer_t misc_timer;
    ssapi_timer_t keepalive_timer;
    int socket;
    unsigned long addr;
    unsigned short port;
    int index;
    sccpcm_ccm_types_t ccm_type;
    int connection_retries;
    int max_connection_retries;
    int ack_rsp_retries;
    int timeouts_before_register;
    int ms_before_send_message;
    int lockout_time_ms;
    int shutdown;
    int ka_count;
    
    sccpcm_chunk_data_t chunk_data;
} sccpcm_cmcb_t;

typedef struct sccpcm_msg_tcp_events_t_ {
    int msg_id;
    gapi_tcp_events_e event;
    void *data;
    int data_len;
} sccpcm_msg_tcp_events_t;

typedef struct sccpcm_msg_reg_ack_t_ {
    int msg_id;
    int cause;
    unsigned long keepalive1;
    unsigned long keepalive2;
} sccpcm_msg_reg_ack_t;

typedef struct sccpcm_msg_reg_nak_t_ {
    int msg_id;
    char text[SCCPMSG_DISPLAY_TEXT_SIZE + 1];
} sccpcm_msg_reg_nak_t;

typedef struct sccpcm_msg_timeout_t_ {
    int msg_id;
    int event;
} sccpcm_msg_timeout_t;

typedef struct sccpcm_msg_error_t_ {
    int msg_id;
    int error;
} sccpcm_msg_error_t;


//void sccpcm_free_cmcb(sccpcm_cmcb_t *cmcb);
void sccpcm_free_cmcb2(sccpcm_cmcb_t *cmcb);
void *sccpcm_get_new_cmcb(struct sccp_sccpcb_t_ *sccpcb, int index);
void sccpcm_init_cmcb(struct sccp_sccpcb_t_ *sccpcb, int index,
                      sccpcm_ccm_types_t ccm_type,
                      unsigned long addr, unsigned short port);
char *sccpcm_print_cm(sccpcm_cmcb_t *cmcb);
#if 0
void *sccpcm_get_new_cmcb(struct sccp_sccpcb_t_ *sccpcb, int index,
                          sccpcm_ccm_types_t ccm_type,
                          unsigned long addr, unsigned short port);
#endif
int sccpcm_init(void);
int sccpcm_cleanup(void);
int sccpcm_get_keepalive_timeout(sccpcm_cmcb_t *cmcb);
int sccpcm_get_primary_index (struct sccp_sccpcb_t_ *sccpcb);
int sccpcm_get_secondary_index(struct sccp_sccpcb_t_ *sccpcb);
int sccpcm_get_connected_index(struct sccp_sccpcb_t_ *sccpcb);
int sccpcm_get_cm_by_index(struct sccp_sccpcb_t_ *sccpcb, int index);
int sccpcm_are_all_cms_idle(struct sccp_sccpcb_t_ *sccpcb);
int sccpcm_are_all_cms_lockout(struct sccp_sccpcb_t_ *sccpcb);
int sccpcm_is_cm_registered_with_fallback(struct sccp_sccpcb_t_ *sccpcb);
int sccpcm_is_cmcb_idle(sccpcm_cmcb_t *cmcb);
int sccpcm_is_cmcb_connected(sccpcm_cmcb_t *cmcb);
int sccpcm_is_cmcb_connected2(sccpcm_cmcb_t *cmcb);
int sccpcm_is_cmcb_registered(sccpcm_cmcb_t *cmcb);
int sccpcm_is_cmcb_token(sccpcm_cmcb_t *cmcb);
int sccpcm_is_cmcb_lockout(sccpcm_cmcb_t *cmcb);

int sccpcm_get_cm_count(struct sccp_sccpcb_t_ *sccpcb);
sccpcm_cmcb_t *sccpcm_get_primary_cmcb(struct sccp_sccpcb_t_ *sccpcb);
unsigned int sccpcm_get_primary_socket(struct sccp_sccpcb_t_ *sccpcb);

sccpcm_cmcb_t *sccpcm_get_cmcb_by_sccpmsg(struct sccp_sccpcb_t_ *sccpcb,
                                         sccpmsg_general_t *msg,
                                         unsigned long socket);
sccpcm_cmcb_t *sccpcm_get_cmcb_by_socket(struct sccp_sccpcb_t_ *sccpcb,
                                         int socket);
sccpcm_chunk_data_t *sccpcm_get_chunk_data(struct sccp_sccpcb_t_ *sccpcb,
                                           unsigned int socket);
int sccpcm_cm_disconnect_all(struct sccp_sccpcb_t_ *sccpcb);
int sccpcm_free_all_cmcbs(struct sccp_sccpcb_t_ *sccpcb);

int sccpcm_push_tcp_event(struct sccp_sccpcb_t_ *sccpcb, int msg_id,
                          unsigned int socket,
                          gapi_tcp_events_e event, void *data, int size);
int sccpcm_push_con_req(struct sccp_sccpcb_t_ *sccpcb, int msg_id, int index);
int sccpcm_push_discon_req(struct sccp_sccpcb_t_ *sccpcb, int msg_id,
                           int index);
int sccpcm_push_reg_req(struct sccp_sccpcb_t_ *sccpcb, int msg_id,
                         int index);
int sccpcm_push_reg_ack(struct sccp_sccpcb_t_ *sccpcb, int msg_id,
                        int index, int cause,
                        unsigned long keepalive1,
                        unsigned long keepalive2);
int sccpcm_push_reg_nak(struct sccp_sccpcb_t_ *sccpcb, int msg_id, int index,
                        char *text);
int sccpcm_push_registered(struct sccp_sccpcb_t_ *sccpcb, int msg_id,
                           int index);
int sccpcm_push_unreg_req(struct sccp_sccpcb_t_ *sccpcb, int msg_id,
                          int index);
int sccpcm_push_unreg_ack(struct sccp_sccpcb_t_ *sccpcb, int msg_id,
                          int index, int cause);
int sccpcm_push_unreg_nak(struct sccp_sccpcb_t_ *sccpcb, int msg_id,
                          int index, int cause);
int sccpcm_push_token_req(struct sccp_sccpcb_t_ *sccpcb, int msg_id,
                          int index);
int sccpcm_push_error(struct sccp_sccpcb_t_ *sccpcb, int msg_id,
                      int index, int error);

#endif /* _SCCPCM_H_ */
