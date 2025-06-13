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
 *     sapp.h
 *
 *  CREATION/MODIFICATION DATES
 *     Created:  November 2002, Sam Hague
 *     Modified: 
 *
 *  DESCRIPTION
 *     SCCP Application header file
 */
#ifndef __SAPP_H__
#define __SAPP_H__

/*sam
 * Had a problem including platform.h when compiling with MFC. The
 * MFC stuff includes everything in the world and does not like the user
 * including specific files such as winsock2.h.
 */
//#include "platform.h"

extern "C"
{
#include "gapi.h"
}

typedef enum sapp_states_e_ {
    SAPP_S_MIN = -1,    
    SAPP_S_IDLE,
    SAPP_S_OPENING,
    SAPP_S_OPENED,
    SAPP_S_REGISTERED,
    SAPP_S_RESETTING,
    SAPP_S_MAX
} sapp_states_e;

/*sam
 * This is a #define in platform.h but I can't get it - see above.
 */
#ifndef PLATFORM_WAIT_INFINITE
#define PLATFORM_WAIT_INFINITE (0xFFFFFFFF)
#endif

#define SAPP_ID "SAPP   "
#define SAPP_MAX_CALLS            (6)
//#define SAPP_MAX_SCCP_BUFFER_SIZE (2048)
#define SAPP_MAX_SCCP_BUFFER_SIZE (1400)
#define SAPP_SCCP_WAIT_TIMEOUT    (PLATFORM_WAIT_INFINITE) //2000 /* ms */
#define SAPP_MAX_CMS              (5)
#define SAPP_MAX_SOCKETS          (5 * SAPP_MAX_INFOS)
#define SAPP_NO_SOCKET            (-1)
#define SAPP_NO_CONN_ID           (-1)
#define SAPP_NO_SAPP_SOCKET       (0)
#define SAPP_DEFAULT_LINE         (1)
#define SAPP_MAX_INFOS            (1)


typedef enum sapp_cm_states_e_ {
    SAPP_CM_S_MIN = -1,
    SAPP_CM_S_CLOSED,
    SAPP_CM_S_CONNECTING,
    SAPP_CM_S_CONNECTED,
    SAPP_CM_S_REGISTERING,
    SAPP_CM_S_REGISTERED,
    SAPP_CM_S_MAX
} sapp_cm_states_e;

typedef struct sapp_cminfo_t_ {
    gapi_cmaddr_t    cmaddr;
    sapp_cm_states_e state;
} sapp_cminfo_t;

typedef struct sapp_status_data_info_t_ {
    int linecnt;
    int speeddialcnt;
    int featurecnt;
    int serviceurlcnt;
    int softkeycnt;
    int softkeysetcnt;
    gapi_line_t                   lines[GAPI_MAX_LINES];
    gapi_speeddial_t              speeddials[GAPI_MAX_SPEED_DIALS];
    gapi_feature_t                features[GAPI_MAX_FEATURES];
    gapi_serviceurl_t             serviceurls[GAPI_MAX_SERVICE_URLS];
    gapi_softkey_definition_t     softkeys[GAPI_MAX_SOFTKEY_DEFINITIONS];
    gapi_softkey_set_definition_t softkeysets[GAPI_MAX_SOFTKEY_SET_DEFINITIONS];
    gapi_versions_e version;
} sapp_status_data_info_t;

typedef struct sapp_info_t_ {
    int id;

    gapi_callbacks_t *sapp_sccp_cbs;
    void *sapp_sccp_handle;
    void *sapp_sapp_handle;
    sapp_states_e sapp_state;
    gapi_causes_e sapp_reset_cause;
    sapp_status_data_info_t sapp_session_data;
    sapp_cminfo_t sapp_cminfo[SAPP_MAX_CMS];

    char *sapp_mac_addr;
    void *sapp_sccp_event;
} sapp_info_t;

typedef enum sapp_direction_e_ {
    SAPP_DIRECTION_OUTGOING,
    SAPP_DIRECTION_INCOMING
} sapp_direction_e;

typedef struct sapp_call_t_ {
    int sapp_call_id;
    int sccp_conn_id;

    int sapp_line;
    int sccp_line;

    gapi_conninfo_t sccp_conninfo;
    gapi_conninfo_t sapp_conninfo;
    
    gapi_media_t sccp_media;
    gapi_media_t sapp_media;

    int gapi_waiting;

    sapp_info_t *sinfo;
} sapp_call_t;

typedef struct sapp_calls_t_ {
    void *mutex;
    int count;
    sapp_call_t calls[SAPP_MAX_CALLS];
} sapp_calls_t;


#define SAPP_DEBUGP if (0) printf
#define SAPP_DEBUG if (sapp_debug) printf
//#define SAPP_DEBUG() ((void)0)
void sapp_debug_entry(char *fname, int conn_id, int line);

int  sapp_init(void);
int  sapp_cleanup(void);
int  sapp_startup(void);
sapp_info_t *sapp_get_info(int id);

sapp_cminfo_t *sapp_get_cminfo(unsigned long addr, unsigned short port,
                               gapi_handle_t handle);
gapi_cmaddr_t *sapp_get_cmaddr_by_state(sapp_cm_states_e state,
                                        gapi_handle_t handle);
gapi_cmaddr_t *sapp_get_cmaddr_by_state2(sapp_cm_states_e state);
void *sapp_get_gapihandle (void);
int  sapp_get_num_lines(void);
char *sapp_get_line_name(int line);
int  sapp_get_conn_id(void);
int  sapp_get_line(void);
sapp_call_t *sapp_get_call_by_conn_id(int id);
sapp_call_t *sapp_get_call_by_call_id(int id);
int sapp_get_call_list(int *list);
void sapp_free_call(sapp_call_t *call);
int sapp_process_cc_event(void *data);
int sapp_is_this_sccp(void *data);
void sapp_debug_entry(char *fname, int conn_id, int line);
unsigned long sapp_local_addr();

gapi_setup_f               sapp_sapp_setup;
gapi_offhook_f             sapp_sapp_offhook;
gapi_setup_ack_f           sapp_sapp_setup_ack;
gapi_proceeding_f          sapp_sapp_proceeding;
gapi_alerting_f            sapp_sapp_alerting;
gapi_connect_ack_f         sapp_sapp_connect_ack;
gapi_connect_f             sapp_sapp_connect;
gapi_disconnect_f          sapp_sapp_disconnect;
gapi_release_f             sapp_sapp_release;
gapi_release_complete_f    sapp_sapp_release_complete;
gapi_starttone_f           sapp_sapp_starttone;
gapi_openrcv_req_f         sapp_sapp_openrcv_req;
gapi_closercv_f            sapp_sapp_closercv;
gapi_startxmit_f           sapp_sapp_startxmit;
gapi_stopxmit_f            sapp_sapp_stopxmit;
gapi_feature_req_f         sapp_sapp_feature_req;
gapi_feature_res_f         sapp_sapp_feature_res;
gapi_connectionstats_f     sapp_sapp_connectionstats;
gapi_reset_f               sapp_sapp_reset;
gapi_opensession_res_f     sapp_sapp_opensession_res;
gapi_sessionstatus_f       sapp_sapp_sessionstatus;
gapi_serviceurlstat_res_f  sapp_sapp_serviceurlstat_res;
gapi_featurestat_res_f     sapp_sapp_featurestat_res;
gapi_speeddialstat_res_f   sapp_sapp_speeddialstat_res;
gapi_linestat_res_f        sapp_sapp_linestat_res;
gapi_softkeytemplate_res_f sapp_sapp_softkeytemplate_res;
gapi_softkeyset_res_f      sapp_sapp_softkeyset_res;
gapi_resetsession_res_f    sapp_sapp_resetsession_res;
gapi_conninfo_f            sapp_sapp_conninfo;
gapi_passthru_f            sapp_sapp_passthru;

int sapp_setup(int call_id, int line,
               void *caller_id, char *digits, int numdigits,
               void *media, int alert_info, int privacy, int flag);
int sapp_setup_ack(int call_id, int line, void *media);
int sapp_proceeding(int call_id, int line);
int sapp_alerting(int call_id, int line);
int sapp_connect(int call_id, int line, void *media);
int sapp_connect_ack(int conn_id, int line);
int sapp_release(int conn_id, int line, int cause);
int sapp_release_complete(int conn_id, int line, int cause);
int sapp_digits(int call_id, int line, char *digits, int numdigits);
int sapp_feature_req(int call_id, int line, int feature, void *data,
                     gapi_causes_e cause);
int sapp_opensession_wrapper(char* lmac, unsigned long cm_addr, unsigned short cm_port);
int sapp_opensession_req(gapi_cmaddr_t *cms, char *mac, 
                         gapi_cmaddr_t *srsts, gapi_srst_modes_e srst_mode,
                         gapi_cmaddr_t *tftp, gapi_media_caps_t *media_caps,
                         gapi_opensession_values_t *values,
                         gapi_protocol_versions_e version, sapp_info_t *sinfo);
int sapp_resetsession_req(gapi_causes_e cause);
void sapp_showsession(void);

int sapp_sapp_openrcv_res(int conn_id, int line, gapi_media_t *media);

int sapp_sccp_main(void *data, int type);
int sapp_test(void);

#endif /* __SAPP_H__ */
