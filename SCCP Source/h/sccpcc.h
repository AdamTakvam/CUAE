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
 *     sccpcc.h
 *
 *  CREATION/MODIFICATION DATES
 *     Created:  March 2003, Sam Hague
 *     Modified: 
 *
 *  DESCRIPTION
 *     SCCP Call Control header file
 */
#ifndef _SCCPCC_H_
#define _SCCPCC_H_

#include "gapi.h"
#include "sem.h"
#include "sccpmsg.h"


#define SCCPCC_DEFAULT_ID (0)


struct sccp_sccpcb_t_;

typedef enum sccpcc_events_t_ {
    SCCPCC_E_MIN = -1,
    SCCPCC_E_SETUP,
    SCCPCC_E_SETUP_ACK,
    SCCPCC_E_PROCEEDING,
    SCCPCC_E_ALERTING,
    SCCPCC_E_CONNECT,
    SCCPCC_E_CONNECT_ACK,
    SCCPCC_E_DISCONNECT,
    SCCPCC_E_RELEASE,
    SCCPCC_E_RELEASE_COMPLETE,
    SCCPCC_E_OFFHOOK,
    SCCPCC_E_ONHOOK,
    SCCPCC_E_DIGITS,
    SCCPCC_E_FEATURE_REQ,
    SCCPCC_E_CALL_STATE,
    SCCPCC_E_CS_OFFHOOK,
    SCCPCC_E_CS_SETUP,
    SCCPCC_E_CS_PROCEEDING,
    SCCPCC_E_CS_ALERTING,
    SCCPCC_E_CS_CONNECTED,
    SCCPCC_E_CS_RELEASE,
    SCCPCC_E_UPDATE_UI,
    SCCPCC_E_MEDIA,
    SCCPCC_E_OPENRCV_RES,
    SCCPCC_E_TIMER,
    SCCPCC_E_CLEANUP,
    SCCPCC_E_DEVTOUSERDATA_REQ,
    SCCPCC_E_DEVTOUSERDATA_RES,
    SCCPCC_E_PASSTHRU,
    SCCPCC_E_MAX
} sccpcc_events_t;

typedef struct sccpcc_cccb_t_ {
    struct sccpcc_cccb_t_ *next;
    int id;
    int state;
    int old_state;

    struct sccp_sccpcb_t_ *sccpcb;

    unsigned int socket;
    int line;
    gapi_privacy_e privacy;
    int setup_ack;
    unsigned long sccp_call_id;
    unsigned long sccp_line;
    unsigned long passthru_party_id;
    unsigned long conference_id;
    int incoming;
    gapi_precedence_t precedence;
    char digits[GAPI_DIRECTORY_NUMBER_SIZE + 1];
    int numdigits;
} sccpcc_cccb_t;

typedef struct sccpcc_msg_cleanup_t_ {
    int msg_id;
    int conn_id;
    int line;
    gapi_causes_e cause;
} sccpcc_msg_cleanup_t;

typedef struct sccpcc_msg_setup_t_ {
    int msg_id;
    int conn_id;
    int line;
    gapi_conninfo_t conninfo;
    char digits[GAPI_DIRECTORY_NUMBER_SIZE + 1];
    int numdigits;
    gapi_media_t      media;
    gapi_alert_info_e alert_info;
    gapi_privacy_e    privacy;
    gapi_precedence_t precedence;
} sccpcc_msg_setup_t;

typedef struct sccpcc_msg_setup_ack_t_ {
    int msg_id;
    int conn_id;
    int line;
    gapi_conninfo_t   conninfo;
    gapi_media_t      media;
    gapi_causes_e     cause;
    gapi_precedence_t precedence;
} sccpcc_msg_setup_ack_t;

typedef struct sccpcc_msg_proceeding_t_ {
    int msg_id;
    int conn_id;
    int line;
    gapi_conninfo_t   conninfo;
    gapi_precedence_t precedence;
} sccpcc_msg_proceeding_t;

typedef struct sccpcc_msg_alerting_t_ {
    int msg_id;
    int conn_id;
    int line;
    gapi_conninfo_t   conninfo;
    gapi_media_t      media;
    gapi_inband_e     inband;
    gapi_precedence_t precedence;
} sccpcc_msg_alerting_t;

typedef struct sccpcc_msg_connect_t_ {
    int msg_id;
    int conn_id;
    int line;
    gapi_conninfo_t   conninfo;
    gapi_media_t      media;
    gapi_precedence_t precedence;
} sccpcc_msg_connect_t;

typedef struct sccpcc_msg_connect_ack_t_ {
    int msg_id;
    int conn_id;
    int line;
    gapi_conninfo_t   conninfo;
    gapi_media_t      media;
    gapi_precedence_t precedence;
} sccpcc_msg_connect_ack_t;

typedef struct sccpcc_msg_disconnect_t_ {
    int msg_id;
    int conn_id;
    int line;
    gapi_causes_e     cause;
    gapi_precedence_t precedence;
} sccpcc_msg_disconnect_t;

typedef struct sccpcc_msg_release_t_ {
    int msg_id;
    int conn_id;
    int line;
    gapi_causes_e     cause;
    gapi_precedence_t precedence;
} sccpcc_msg_release_t;

typedef struct sccpcc_msg_release_complete_t_ {
    int msg_id;
    int conn_id;
    int line;
    gapi_causes_e     cause;
    gapi_precedence_t precedence;
} sccpcc_msg_release_complete_t;

typedef struct sccpcc_msg_openrecv_res_t_ {
    int msg_id;
    int conn_id;
    int line;
    gapi_media_t media;
    gapi_causes_e cause;
} sccpcc_msg_openrcv_res_t;

typedef struct sccpcc_msg_feature_req_t_ {
    int msg_id;
    int conn_id;
    int line;
    gapi_features_e     feature;
    gapi_feature_data_u data;
    gapi_causes_e       cause;
} sccpcc_msg_feature_req_t;

typedef struct sccpcc_msg_offhook_t_ {
    int msg_id;
    int conn_id;
    int line;
} sccpcc_msg_offhook_t;

typedef struct sccpcc_msg_digits_t_ {
    int msg_id;
    int conn_id;
    int line;
    char digits[GAPI_DIRECTORY_NUMBER_SIZE + 1];
    int numdigits;
} sccpcc_msg_digits_t;

typedef struct sccpcc_msg_devicetouserdata_req_t_ {
    int msg_id;
    int conn_id;
    int line;
    gapi_user_and_device_data_t data;
} sccpcc_msg_devicetouserdata_req_t;

typedef struct sccpcc_msg_devicetouserdata_res_t_ {
    int msg_id;
    int conn_id;
    int line;
    gapi_user_and_device_data_t data;
} sccpcc_msg_devicetouserdata_res_t;

void sccpcc_free_cccb2(sccpcc_cccb_t *cccb);
void *sccpcc_get_new_cccb(struct sccp_sccpcb_t_ *sccpcb, int conn_id);
int sccpcc_init(void);
int sccpcc_cleanup(void);
sccpcc_cccb_t *sccpcc_get_cccb_by_sccpmsg(struct sccp_sccpcb_t_ *sccpcb,
                                          sccpmsg_general_t *msg);
int sccpcc_push_setup(struct sccp_sccpcb_t_ *sccpcb,
                      int msg_id, int conn_id, int line,
                      gapi_conninfo_t *conninfo, char *digits,
                      int numdigits, gapi_media_t *media,
                      gapi_alert_info_e alert_info, gapi_privacy_e privacy,
                      gapi_precedence_t *precedence);
int sccpcc_push_setup_ack(struct sccp_sccpcb_t_ *sccpcb,
                          int msg_id, int conn_id, int line,
                          gapi_conninfo_t *conninfo,
                          gapi_media_t *media, gapi_causes_e cause,
                          gapi_precedence_t *precedence);
int sccpcc_push_proceeding(struct sccp_sccpcb_t_ *sccpcb,
                           int msg_id, int conn_id, int line,
                           gapi_conninfo_t *conninfo,
                           gapi_precedence_t *precedence);
int sccpcc_push_alerting(struct sccp_sccpcb_t_ *sccpcb,
                         int msg_id, int conn_id, int line,
                         gapi_conninfo_t *conninfo,
                         gapi_media_t *media, gapi_inband_e inband,
                         gapi_precedence_t *precedence);
int sccpcc_push_connect(struct sccp_sccpcb_t_ *sccpcb,
                        int msg_id, int conn_id, int line,
                        gapi_conninfo_t *conninfo,
                        gapi_media_t *media,
                        gapi_precedence_t *precedence);
int sccpcc_push_connect_ack(struct sccp_sccpcb_t_ *sccpcb,
                            int msg_id, int conn_id, int line,
                            gapi_conninfo_t *conninfo,
                            gapi_media_t *media,
                            gapi_precedence_t *precedence);
int sccpcc_push_disconnect(struct sccp_sccpcb_t_ *sccpcb,
                           int msg_id, int conn_id, int line,
                           gapi_causes_e cause,
                           gapi_precedence_t *precedence);
int sccpcc_push_release(struct sccp_sccpcb_t_ *sccpcb,
                        int msg_id, int conn_id, int line,
                        gapi_causes_e cause,
                        gapi_precedence_t *precedence);
int sccpcc_push_release_complete(struct sccp_sccpcb_t_ *sccpcb,
                                 int msg_id, int conn_id, int line,
                                 gapi_causes_e cause,
                                 gapi_precedence_t *precedence);
int sccpcc_push_openrcv_res(struct sccp_sccpcb_t_ *sccpcb,
                            int msg_id, int conn_id, int line,
                            gapi_media_t *media, gapi_causes_e cause);
int sccpcc_push_feature_req(struct sccp_sccpcb_t_ *sccpcb,
                            int msg_id, int conn_id, int line,
                            gapi_features_e feature,
                            gapi_feature_data_u *data,
                            gapi_causes_e cause);
int sccpcc_push_feature_res(struct sccp_sccpcb_t_ *sccpcb,
                            int msg_id, int conn_id, int line,
                            gapi_features_e feature,
                            gapi_feature_data_u *data,
                            gapi_causes_e cause);
int sccpcc_push_offhook(struct sccp_sccpcb_t_ *sccpcb,
                        int msg_id, int conn_id, int line);
int sccpcc_push_digits(struct sccp_sccpcb_t_ *sccpcb,
                       int msg_id, int conn_id, int line,
                       char *digits, int numdigits);
int sccpcc_push_devicetouserdata_req(struct sccp_sccpcb_t_ *sccpcb,
                                     int msg_id, int conn_id, int line,
                                     gapi_user_and_device_data_t *data);
int sccpcc_push_devicetouserdata_res(struct sccp_sccpcb_t_ *sccpcb,
                                     int msg_id, int conn_id, int line,
                                     gapi_user_and_device_data_t *data);

#endif /* _SCCPCC_H_ */
