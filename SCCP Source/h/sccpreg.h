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
 *     sccpreg.h
 *
 *  CREATION/MODIFICATION DATES
 *     Created:  March 2003, Sam Hague
 *     Modified: 
 *
 *  DESCRIPTION
 *     SCCP Registration header file
 */
#ifndef _SCCPREG_H_
#define _SCCPREG_H_

#include "sem.h"
#include "sccpmsg.h"


struct sccp_sccpcb_t_;

typedef enum sccpreg_events_t_ {
    SCCPREG_E_MIN = -1,
    SCCPREG_E_REG_REQ,
    SCCPREG_E_REGISTER_ACK,
    SCCPREG_E_REGISTER_REJ,
    SCCPREG_E_CAPABILITIES_REQ,
    SCCPREG_E_BUTTON_TEMPLATE_RES,
    SCCPREG_E_SOFTKEY_TEMPLATE_RES,
    SCCPREG_E_SOFTKEY_SET_RES,
    SCCPREG_E_SELECT_SOFTKEYS,
    SCCPREG_E_CONFIG_STAT_RES,
    SCCPREG_E_LINE_STAT_RES,
    SCCPREG_E_SPEEDDIAL_STAT_RES,
    SCCPREG_E_FORWARD_STAT_RES,
    SCCPREG_E_FEATURE_STAT_RES,
    SCCPREG_E_SERVICEURL_STAT_RES,
    SCCPREG_E_VERSION_RES,
    SCCPREG_E_SERVER_RES,
    SCCPREG_E_UNREG_REQ,
    SCCPREG_E_UNREGISTER_ACK,
    SCCPREG_E_REGISTERED,
    SCCPREG_E_CLEANUP,
    SCCPREG_E_MAX
} sccpreg_events_t;

typedef struct sccpreg_line_t_ {
    int instance;
}sccpreg_line_t;

typedef struct sccpreg_regcb_t_ {
    struct sccpreg_regcb_t_ *next;
    int id;
    int state;
    int old_state;

    struct sccp_sccpcb_t_ *sccpcb;

    unsigned int socket;
    int index;

    int templinecnt;
    int tempspeeddialcnt;
    int tempfeaturecnt;
    int tempserviceurlcnt;

    int misccnt;

    /*
     * The following params must match with the gapi_status_data_info_t.
     */
    int linecnt;
    int speeddialcnt;
    int featurecnt;
    int serviceurlcnt;
    int softkeycnt;
    int softkeysetcnt;

#if 0
    sccpreg_line_t lines[GAPI_MAX_LINES];
    sccpreg_line_t speeddials[GAPI_MAX_SPEED_DIALS];
    sccpreg_line_t features[GAPI_MAX_FEATURES];
    sccpreg_line_t serviceurls[GAPI_MAX_SERVICE_URLS];

    gapi_line_t               lines[GAPI_MAX_LINES];
    gapi_speeddial_t          speeddials[GAPI_MAX_SPEED_DIALS];
    gapi_feature_t            features[GAPI_MAX_FEATURES];
    gapi_serviceurl_t         serviceurls[GAPI_MAX_SERVICE_URLS];
    gapi_softkey_definition_t softkeys[GAPI_MAX_SOFTKEY_DEFINITIONS];
    gapi_softkey_set_t        softkeysets[GAPI_MAX_SOFTKEY_SET_DEFINITIONS];
#endif
} sccpreg_regcb_t;

void sccpreg_free_regcb(sccpreg_regcb_t *regcb);
void *sccpreg_get_new_regcb(struct sccp_sccpcb_t_ *sccpcb);
int sccpreg_init(void);
int sccpreg_cleanup(void);
int sccpreg_push_reg_req(struct sccp_sccpcb_t_ *sccpcb, int msg_id);
int sccpreg_push_unreg_req(struct sccp_sccpcb_t_ *sccpcb, int msg_id);
int sccpreg_push_unregister_ack(struct sccp_sccpcb_t_ *sccpcb, int msg_id,
                                sccpmsg_unregister_status_t status);
gapi_status_data_info_t *sccpreg_get_session_data(struct sccp_sccpcb_t_ *sccpcb);

#endif /* _SCCPREG_H_ */
