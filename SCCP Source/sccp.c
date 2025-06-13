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
 *     sccp.c
 *
 *  CREATION/MODIFICATION DATES
 *     Created:  November 2002, Sam Hague
 *     Modified: 
 *
 *  DESCRIPTION
 *     Skinny Client Control Protocol implementation
 */
#include "sccp_platform.h"
#include "sccp.h"
#include "sccp_debug.h"
#include "sem.h"
#include "ssapi.h"
#include "sllist.h"
#include "sccpmsg.h"


extern sem_table_t sccpcc_sem_table;
extern sem_table_t sccpcm_sem_table;
extern sem_table_t sccprec_sem_table;
extern sem_table_t sccpreg_sem_table;

/*
 * This is the temp buffer used when reassembling a fragmented message,
 * therefore, make sure that this matches the maximum size of a received message.
 */
#define SCCP_CHUNK_DATA_BUFFER_SIZE 1024

#define SCCP_EVENT_LIST_NAME  "events"
#define SCCP_CCCB_LIST_NAME   "cccbs"
#define SCCP_CMCB_LIST_NAME   "cmcbs"
#define SCCP_SCCPCB_LIST_NAME "sccpcbs"

#ifdef SCCP_USE_POOL

typedef struct sccp_pool_table_element_t_ {
    unsigned int size;
    int  cnt;
    void *list;
    char *name;
    unsigned long *pool_start;
    unsigned long *pool_end;
} sccp_pool_table_element_t;

typedef enum sccp_pools_t_ {
    SCCP_POOL_MIN = -1,
    SCCP_POOL_1,
    SCCP_POOL_2,
    SCCP_POOL_3,
    SCCP_POOL_4,
#ifdef SCCP_USE_POOL5
    SCCP_POOL_5,
#endif
    SCCP_POOL_MAX
} sccp_pools_t;

#define SCCP_POOL_1_LIST_NAME "pool1"
#define SCCP_POOL_2_LIST_NAME "pool2"
#define SCCP_POOL_3_LIST_NAME "pool3"
#define SCCP_POOL_4_LIST_NAME "pool4"
#ifdef SCCP_USE_POOL5
#define SCCP_POOL_5_LIST_NAME "pool5"
#endif

/*
 * 1-36: per sccpcb: n events, n sccp messages, n internal messages
 * 2-100: per sccpcb: 5 cmcbs, 1 regcb, n cccbs, n sccp messages, n internal msgs
 * 3-352: 1 sccpcallbacks, per sccpcb: 1 sccpcb, 1 reccb, 1 appcallbacks
 * 4-1024: per sccpcb: 1 sccp message (softkey, softkeyset)
 *
 * NOTE: - need to add a 2048 size if the user_data messages are used.
 *       - shouldn't need sizes > 1024 (actually just need 792 for the softkey
 *         message) because SAPP passes a pointer to the whole packet
 *         (which can be up to 1400). PSCCP then copies data in SCCP message
 *         sizes of which the largest is 792.
 */
#define SCCP_MULT        (1) /* 25 for 60 sessions */
#define SCCP_POOL_1_CNT  (20 * SCCP_MULT) /* 720  */
#define SCCP_POOL_2_CNT  (15 * SCCP_MULT) /* 1500 */
#define SCCP_POOL_3_CNT  (7  * SCCP_MULT)  /* 2464 */
#define SCCP_POOL_4_CNT  (5  * SCCP_MULT)  /* 5120 */
#ifdef SCCP_USE_POOL5
#define SCCP_POOL_5_CNT  (4  * SCCP_MULT)  /* 9664 */
#endif

#define SCCP_POOL_1_SIZE 36
#define SCCP_POOL_2_SIZE 100
#define SCCP_POOL_3_SIZE 348
#define SCCP_POOL_4_SIZE 1024
#ifdef SCCP_USE_POOL5
#define SCCP_POOL_5_SIZE 2416
#endif

static unsigned long sccp_pool_1[SCCP_POOL_1_CNT][SCCP_POOL_1_SIZE / sizeof(unsigned long)];
static unsigned long sccp_pool_2[SCCP_POOL_2_CNT][SCCP_POOL_2_SIZE / sizeof(unsigned long)];
static unsigned long sccp_pool_3[SCCP_POOL_3_CNT][SCCP_POOL_3_SIZE / sizeof(unsigned long)];
static unsigned long sccp_pool_4[SCCP_POOL_4_CNT][SCCP_POOL_4_SIZE / sizeof(unsigned long)];
#ifdef SCCP_USE_POOL5
static unsigned long sccp_pool_5[SCCP_POOL_5_CNT][SCCP_POOL_5_SIZE / sizeof(unsigned long)];
#endif

static sccp_pool_table_element_t sccp_pool_table[SCCP_POOL_MAX] =
{
    {
        SCCP_POOL_1_SIZE, SCCP_POOL_1_CNT, NULL, SCCP_POOL_1_LIST_NAME,
        (unsigned long *)sccp_pool_1,
        (unsigned long *)sccp_pool_1 + (SCCP_POOL_1_CNT - 1) * (SCCP_POOL_1_SIZE / sizeof(unsigned long))
    },
    {
        SCCP_POOL_2_SIZE, SCCP_POOL_2_CNT, NULL, SCCP_POOL_2_LIST_NAME,
        (unsigned long *)sccp_pool_2,
        (unsigned long *)sccp_pool_2 + (SCCP_POOL_2_CNT - 1) * (SCCP_POOL_2_SIZE / sizeof(unsigned long))
    },
    {
        SCCP_POOL_3_SIZE, SCCP_POOL_3_CNT, NULL, SCCP_POOL_3_LIST_NAME,
        (unsigned long *)sccp_pool_3,
        (unsigned long *)sccp_pool_3 + (SCCP_POOL_3_CNT - 1) * (SCCP_POOL_3_SIZE / sizeof(unsigned long))
    },
    {
        SCCP_POOL_4_SIZE, SCCP_POOL_4_CNT, NULL, SCCP_POOL_4_LIST_NAME,
        (unsigned long *)sccp_pool_4,
        (unsigned long *)sccp_pool_4 + (SCCP_POOL_4_CNT - 1) * (SCCP_POOL_4_SIZE / sizeof(unsigned long))
    }
#ifdef SCCP_USE_POOL5
    ,
    {
        SCCP_POOL_5_SIZE, SCCP_POOL_5_CNT, NULL, SCCP_POOL_5_LIST_NAME,
        (unsigned long *)sccp_pool_5,
        (unsigned long *)sccp_pool_5 + (SCCP_POOL_5_CNT - 1) * (SCCP_POOL_5_SIZE / sizeof(unsigned long))
    }
#endif
};
#endif

typedef struct sccp_sccp_event_t_ {
    sccpmsg_messages_t msg;
    sccp_sems_t        sem;
    int                event;
    int                size;
} sccp_sccp_event_t;


static int sccp_initialized = 0;
static int              sccp_sccpcb_id = 0;
static sccp_sccpcb_t    *sccp_sccpcbs   = NULL;
static gapi_callbacks_t *sccp_callbacks = NULL;

/*
 * This table defines all the SCCP messages that can be received by PSCCP from the 
 * network. The messages that can only be sent by PSCCP to the CCM are included, but
 * ifdef'ed out.
 */
static sccp_sccp_event_t sccp_sccp_event_table[] = 
{
    {SCCPMSG_KEEPALIVE,                          SCCP_SEM_CM,  SCCPCM_E_KEEPALIVE,              sizeof(sccpmsg_keepalive_t)},
#if 0
    {SCCPMSG_REGISTER,                           SCCP_SEM_REG, SCCPREG_E_MIN,                   sizeof(sccpmsg_register_t)},
    {SCCPMSG_IP_PORT,                            SCCP_SEM_REG, SCCPREG_E_MIN,                   sizeof(sccpmsg_ip_port_t)},
#endif
    {SCCPMSG_KEYPAD_BUTTON,                      SCCP_SEM_CC,  SCCPCC_E_MIN,                    sizeof(sccpmsg_keypad_button_t)},
#if 0
    {SCCPMSG_ENBLOC_CALL,                        SCCP_SEM_CC,  SCCPCC_E_MIN,                    sizeof(sccpmsg_enbloc_call_t)},
    {SCCPMSG_STIMULUS,                           SCCP_SEM_CC,  SCCPCC_E_MIN,                    sizeof(sccpmsg_stimulus_t)},
#endif
    {SCCPMSG_OFFHOOK,                            SCCP_SEM_CC,  SCCPCC_E_OFFHOOK,                sizeof(sccpmsg_offhook_t)},
    {SCCPMSG_ONHOOK,                             SCCP_SEM_CC,  SCCPCC_E_ONHOOK,                 sizeof(sccpmsg_onhook_t)},
#if 0
    {SCCPMSG_HOOK_FLASH,                         SCCP_SEM_CC,  SCCPCC_E_MIN,                    sizeof(sccpmsg_hook_flash_t)},
    {SCCPMSG_FORWARD_STAT_REQ,                   SCCP_SEM_REG, SCCPREG_E_MIN,                   sizeof(sccpmsg_forward_stat_req_t)},
    {SCCPMSG_SPEEDDIAL_STAT_REQ,                 SCCP_SEM_REG, SCCPREG_E_MIN,                   sizeof(sccpmsg_speeddial_stat_req_t)},
    {SCCPMSG_LINE_STAT_REQ,                      SCCP_SEM_REG, SCCPREG_E_MIN,                   sizeof(sccpmsg_line_stat_req_t)},
    {SCCPMSG_CONFIG_STAT_REQ,                    SCCP_SEM_REG, SCCPREG_E_MIN,                   sizeof(sccpmsg_config_stat_req_t)},
    {SCCPMSG_TIME_DATE_REQ,                      SCCP_SEM_REG, SCCPREG_E_MIN,                   sizeof(sccpmsg_time_date_req_t)},
    {SCCPMSG_BUTTON_TEMPLATE_REQ,                SCCP_SEM_REG, SCCPREG_E_MIN,                   sizeof(sccpmsg_button_template_req_t)},
    {SCCPMSG_VERSION_REQ,                        SCCP_SEM_REG, SCCPREG_E_MIN,                   sizeof(sccpmsg_version_req_t)},
    {SCCPMSG_CAPABILITIES_RES,                   SCCP_SEM_REG, SCCPREG_E_MIN,                   sizeof(sccpmsg_capabilities_res_t)},
    {SCCPMSG_MEDIA_PORT_LIST,                    SCCP_SEM_REG, SCCPCC_E_MIN,                    sizeof(sccpmsg_media_port_list_t)},
    {SCCPMSG_SERVER_REQ,                         SCCP_SEM_REG, SCCPREG_E_MIN,                   sizeof(sccpmsg_server_req_t)},
    {SCCPMSG_ALARM,                              SCCP_SEM_REG, SCCPREG_E_MIN,                   sizeof(sccpmsg_alarm_t)},
    {SCCPMSG_MULTICAST_MEDIA_RECEPTION_ACK,      SCCP_SEM_CC,  SCCPCC_E_MIN,                    sizeof(sccpmsg_multicast_media_reception_ack_t)},
    {SCCPMSG_OPEN_RECEIVE_CHANNEL_ACK,           SCCP_SEM_CC,  SCCPCC_E_MIN,                    sizeof(sccpmsg_open_receive_channel_ack_t)},
    {SCCPMSG_CONNECTION_STATISTICS_RES,          SCCP_SEM_CC,  SCCPCC_E_MIN,                    sizeof(sccpmsg_connection_statistics_req_t)},
    {SCCPMSG_OFFHOOK_WITH_CGPN,                  SCCP_SEM_CC,  SCCPCC_E_MIN,                    sizeof(sccpmsg_offhook_with_cgpn_t)},
    {SCCPMSG_SOFTKEY_SET_REQ,                    SCCP_SEM_REG, SCCPREG_E_MIN,                   sizeof(sccpmsg_softkey_set_req_t)},
    {SCCPMSG_SOFTKEY_EVENT,                      SCCP_SEM_CC,  SCCPCC_E_MIN,                    sizeof(sccpmsg_softkey_event_t)},
    {SCCPMSG_UNREGISTER,                         SCCP_SEM_REG, SCCPREG_E_MIN,                   sizeof(sccpmsg_unregister_t)},
    {SCCPMSG_SOFTKEY_TEMPLATE_REQ,               SCCP_SEM_REG, SCCPREG_E_MIN,                   sizeof(sccpmsg_softkey_template_req_t)},
    {SCCPMSG_REGISTER_TOKEN_REQ,                 SCCP_SEM_CM,  SCCPCM_E_MIN,                    sizeof(sccpmsg_register_token_req_t)},
    {SCCPMSG_MEDIA_TRANSMISSION_FAILURE,         SCCP_SEM_CC,  SCCPCC_E_MIN,                    sizeof(sccpmsg_media_transmission_failure_t)},
    {SCCPMSG_HEADSET_STATUS,                     SCCP_SEM_CC,  SCCPCC_E_MIN,                    sizeof(sccpmsg_headset_status_t)},
    {SCCPMSG_MEDIA_RESOURCE_NOTIFICATION,        SCCP_SEM_CC,  SCCPCC_E_MIN,                    sizeof(sccpmsg_media_resource_notification_t)},
    {SCCPMSG_REGISTER_AVAILABLE_LINES,           SCCP_SEM_REG, SCCPREG_E_MIN,                   sizeof(sccpmsg_register_available_lines_t)},
    {SCCPMSG_DEVICE_TO_USER_DATA,                SCCP_SEM_CC,  SCCPCC_E_MIN,                    sizeof(sccpmsg_device_to_user_data_t)},
    {SCCPMSG_DEVICE_TO_USER_DATA_RESPONSE,       SCCP_SEM_CC,  SCCPCC_E_MIN,                    sizeof(sccpmsg_device_to_user_data_response_t)},
#endif    
    {SCCPMSG_REGISTER_ACK,                       SCCP_SEM_REG, SCCPREG_E_REGISTER_ACK,          sizeof(sccpmsg_register_ack_t)},
    {SCCPMSG_START_TONE,                         SCCP_SEM_CC,  SCCPCC_E_UPDATE_UI,              sizeof(sccpmsg_start_tone_t)},
    {SCCPMSG_STOP_TONE,                          SCCP_SEM_CC,  SCCPCC_E_UPDATE_UI,              sizeof(sccpmsg_stop_tone_t)},
    {SCCPMSG_SET_RINGER,                         SCCP_SEM_CC,  SCCPCC_E_UPDATE_UI,              sizeof(sccpmsg_set_ringer_t)},
    {SCCPMSG_SET_LAMP,                           SCCP_SEM_CC,  SCCPCC_E_UPDATE_UI,              sizeof(sccpmsg_set_lamp_t)},
//    {SCCPMSG_SET_HKF_DETECT,                     SCCP_SEM_CC,  SCCPCC_E_UPDATE_UI,              sizeof(sccpmsg_set_hkf_detect_t)},
    {SCCPMSG_SET_SPEAKER_MODE,                   SCCP_SEM_CC,  SCCPCC_E_UPDATE_UI,              sizeof(sccpmsg_set_speaker_mode_t)},
    {SCCPMSG_SET_MICRO_MODE,                     SCCP_SEM_CC,  SCCPCC_E_UPDATE_UI,              sizeof(sccpmsg_set_micro_mode_t)},
    {SCCPMSG_START_MEDIA_TRANSMISSION,           SCCP_SEM_CC,  SCCPCC_E_MEDIA,                  sizeof(sccpmsg_start_media_transmission_t)},
    {SCCPMSG_STOP_MEDIA_TRANSMISSION,            SCCP_SEM_CC,  SCCPCC_E_MEDIA,                  sizeof(sccpmsg_stop_media_transmission_t)},
//    {SCCPMSG_START_MEDIA_RECEPTION,              SCCP_SEM_CC,  SCCPCC_E_UPDATE_UI,              sizeof(sccpmsg_start_media_reception_t)},
//    {SCCPMSG_STOP_MEDIA_RECEPTION,               SCCP_SEM_CC,  SCCPCC_E_UPDATE_UI,              sizeof(sccpmsg_stop_media_reception_t)},
//    {SCCPMSG_RESERVED_FOR_FUTURE_USE,            SCCP_SEM_REG, SCCPREG_E_MIN                    sizeof(sccpmsg__t)},
    {SCCPMSG_CALL_INFO,                          SCCP_SEM_CC,  SCCPCC_E_UPDATE_UI,              sizeof(sccpmsg_call_info_t)},
    {SCCPMSG_FORWARD_STAT,                       SCCP_SEM_REG, SCCPREG_E_FORWARD_STAT_RES,      sizeof(sccpmsg_forward_stat_t)},
    {SCCPMSG_SPEEDDIAL_STAT,                     SCCP_SEM_REG, SCCPREG_E_SPEEDDIAL_STAT_RES,    sizeof(sccpmsg_speeddial_stat_t)},
    {SCCPMSG_LINE_STAT,                          SCCP_SEM_REG, SCCPREG_E_LINE_STAT_RES,         sizeof(sccpmsg_line_stat_t)},
    {SCCPMSG_CONFIG_STAT,                        SCCP_SEM_REG, SCCPREG_E_CONFIG_STAT_RES,       sizeof(sccpmsg_config_stat_t)},
    {SCCPMSG_DEFINE_TIME_DATE,                   SCCP_SEM_CC,  SCCPCC_E_UPDATE_UI,              sizeof(sccpmsg_define_time_date_t)},
    {SCCPMSG_START_SESSION_TRANSMISSION,         SCCP_SEM_CC,  SCCPCC_E_MEDIA,                  sizeof(sccpmsg_start_session_transmission_t)},
    {SCCPMSG_STOP_SESSION_TRANSMISSION,          SCCP_SEM_CC,  SCCPCC_E_MEDIA,                  sizeof(sccpmsg_stop_session_transmission_t)},
    {SCCPMSG_BUTTON_TEMPLATE,                    SCCP_SEM_REG, SCCPREG_E_BUTTON_TEMPLATE_RES,   sizeof(sccpmsg_button_template_t)},
    {SCCPMSG_VERSION,                            SCCP_SEM_REG, SCCPREG_E_VERSION_RES,           sizeof(sccpmsg_version_t)},
    {SCCPMSG_DISPLAY_TEXT,                       SCCP_SEM_CC,  SCCPCC_E_UPDATE_UI,              sizeof(sccpmsg_display_text_t)},
    {SCCPMSG_CLEAR_DISPLAY,                      SCCP_SEM_CC,  SCCPCC_E_UPDATE_UI,              sizeof(sccpmsg_clear_display_t)},
    {SCCPMSG_CAPABILITIES_REQ,                   SCCP_SEM_REG, SCCPREG_E_CAPABILITIES_REQ,      sizeof(sccpmsg_capabilities_req_t)},
//    {SCCPMSG_ENUNCIATOR_COMMAND,                 SCCP_SEM_CC,  SCCPCC_E_MIN,                    sizeof(sccpmsg__t)},
    {SCCPMSG_REGISTER_REJECT,                    SCCP_SEM_REG, SCCPREG_E_REGISTER_REJ,          sizeof(sccpmsg_register_reject_t)},
    {SCCPMSG_SERVER_RES,                         SCCP_SEM_REG, SCCPREG_E_SERVER_RES,            sizeof(sccpmsg_server_res_t)},
    {SCCPMSG_RESET,                              SCCP_SEM_REC, SCCPREC_E_RESET,                 sizeof(sccpmsg_reset_t)},
    {SCCPMSG_KEEPALIVE_ACK,                      SCCP_SEM_CM,  SCCPCM_E_KEEPALIVE_ACK,          sizeof(sccpmsg_keepalive_ack_t)},
    {SCCPMSG_START_MULTICAST_MEDIA_RECEPTION,    SCCP_SEM_CC,  SCCPCC_E_MEDIA,                  sizeof(sccpmsg_start_multicast_media_reception_t)},
    {SCCPMSG_START_MULTICAST_MEDIA_TRANSMISSION, SCCP_SEM_CC,  SCCPCC_E_MEDIA,                  sizeof(sccpmsg_start_multicast_media_transmission_t)},
    {SCCPMSG_STOP_MULTICAST_MEDIA_RECEPTION,     SCCP_SEM_CC,  SCCPCC_E_MEDIA,                  sizeof(sccpmsg_stop_multicast_media_reception_t)},
    {SCCPMSG_STOP_MULTICAST_MEDIA_TRANSMISSION,  SCCP_SEM_CC,  SCCPCC_E_MEDIA,                  sizeof(sccpmsg_stop_multicast_media_transmission_t)},
    {SCCPMSG_OPEN_RECEIVE_CHANNEL,               SCCP_SEM_CC,  SCCPCC_E_MEDIA,                  sizeof(sccpmsg_open_receive_channel_t)},
    {SCCPMSG_CLOSE_RECEIVE_CHANNEL,              SCCP_SEM_CC,  SCCPCC_E_MEDIA,                  sizeof(sccpmsg_close_receive_channel_t)},
    {SCCPMSG_CONNECTION_STATISTICS_REQ,          SCCP_SEM_CC,  SCCPCC_E_UPDATE_UI,              sizeof(sccpmsg_connection_statistics_req_t)},
    {SCCPMSG_SOFTKEY_TEMPLATE_RES,               SCCP_SEM_REG, SCCPREG_E_SOFTKEY_TEMPLATE_RES,  sizeof(sccpmsg_softkey_template_res_t)},
    {SCCPMSG_SOFTKEY_SET_RES,                    SCCP_SEM_REG, SCCPREG_E_SOFTKEY_SET_RES,       sizeof(sccpmsg_softkey_set_res_t)},
    {SCCPMSG_SELECT_SOFTKEYS,                    SCCP_SEM_CC,  SCCPCC_E_UPDATE_UI,              sizeof(sccpmsg_select_softkeys_t)},
    {SCCPMSG_CALL_STATE,                         SCCP_SEM_CC,  SCCPCC_E_CALL_STATE,             sizeof(sccpmsg_call_state_t)},
    {SCCPMSG_DISPLAY_PROMPT_STATUS,              SCCP_SEM_CC,  SCCPCC_E_UPDATE_UI,              sizeof(sccpmsg_display_prompt_status_t)},
    {SCCPMSG_CLEAR_PROMPT_STATUS,                SCCP_SEM_CC,  SCCPCC_E_UPDATE_UI,              sizeof(sccpmsg_clear_prompt_status_t)},
    {SCCPMSG_DISPLAY_NOTIFY,                     SCCP_SEM_CC,  SCCPCC_E_UPDATE_UI,              sizeof(sccpmsg_display_notify_t)},
    {SCCPMSG_CLEAR_NOTIFY,                       SCCP_SEM_CC,  SCCPCC_E_UPDATE_UI,              sizeof(sccpmsg_clear_notify_t)},
    {SCCPMSG_ACTIVATE_CALL_PLANE,                SCCP_SEM_CC,  SCCPCC_E_UPDATE_UI,              sizeof(sccpmsg_activate_call_plane_t)},
    {SCCPMSG_DEACTIVATE_CALL_PLANE,              SCCP_SEM_CC,  SCCPCC_E_UPDATE_UI,              sizeof(sccpmsg_deactivate_call_plane_t)},
    {SCCPMSG_UNREGISTER_ACK,                     SCCP_SEM_REG, SCCPREG_E_UNREGISTER_ACK,        sizeof(sccpmsg_unregister_ack_t)},
    {SCCPMSG_BACKSPACE_REQ,                      SCCP_SEM_CC,  SCCPCC_E_UPDATE_UI,              sizeof(sccpmsg_backspace_req_t)},
    {SCCPMSG_REGISTER_TOKEN_ACK,                 SCCP_SEM_CM,  SCCPCM_E_RECV_TOKEN,             sizeof(sccpmsg_register_token_ack_t)},
    {SCCPMSG_REGISTER_TOKEN_REJECT,              SCCP_SEM_CM,  SCCPCM_E_TOKEN_REJ,              sizeof(sccpmsg_register_token_reject_t)},
    {SCCPMSG_START_MEDIA_FAILURE_DETECTION,      SCCP_SEM_CC,  SCCPCC_E_MEDIA,                  sizeof(sccpmsg_start_media_failure_detection_t)},
    {SCCPMSG_DIALED_NUMBER,                      SCCP_SEM_CC,  SCCPCC_E_UPDATE_UI,              sizeof(sccpmsg_dialed_number_t)},
    {SCCPMSG_USER_TO_DEVICE_DATA,                SCCP_SEM_CC,  SCCPCC_E_UPDATE_UI,              sizeof(sccpmsg_user_to_device_data_t)},
    {SCCPMSG_FEATURE_STAT,                       SCCP_SEM_REG, SCCPREG_E_FEATURE_STAT_RES,      sizeof(sccpmsg_feature_stat_t)},
    {SCCPMSG_DISPLAY_PRIORITY_NOTIFY,            SCCP_SEM_CC,  SCCPCC_E_UPDATE_UI,              sizeof(sccpmsg_display_priority_notify_t)},
    {SCCPMSG_CLEAR_PRIORITY_NOTIFY,              SCCP_SEM_CC,  SCCPCC_E_UPDATE_UI,              sizeof(sccpmsg_clear_priority_notify_t)},
    {SCCPMSG_SERVICE_URL_STAT,                   SCCP_SEM_REG, SCCPREG_E_SERVICEURL_STAT_RES,   sizeof(sccpmsg_service_url_stat_t)},
    {SCCPMSG_CALL_SELECT_STAT,                   SCCP_SEM_CC,  SCCPCC_E_UPDATE_UI,              sizeof(sccpmsg_call_select_stat_t)},
    {SCCPMSG_INVALID,                            SCCP_SEM_MIN, SCCPREG_E_MIN,                   0}
};

#ifdef SCCP_PRINT_TABLE
/* debug code to show message sizes. */
static sccp_print_table (void)
{
    sccp_sccp_event_t *entry = sccp_sccp_event_table;
    sccp_sccp_event_t *entry_found = NULL;

    if (entry != NULL) {
        while ((entry != NULL) && (entry->msg != SCCPMSG_INVALID)) {
            SCCP_DBG((sccp_debug, 5,
                     "%04x: %04d: %s\n",
                     entry->msg, entry->size,
                     sccpmsg_get_message_text(entry->msg)));
            entry++;
        }
    }
}
#endif /* SCCP_PRINT_TABLE */

static sccp_sccp_event_t *sccp_sccp_to_sccp_event(int id)
{
    sccp_sccp_event_t *entry = sccp_sccp_event_table;
    sccp_sccp_event_t *entry_found = NULL;

    if (entry != NULL) {
        while ((entry != NULL) && (entry->msg != SCCPMSG_INVALID)) {
            if (id == entry->msg) {
                entry_found = entry;
                break;
            }
            entry++;
        }
    }

    return (entry_found);
}

static sem_table_t *sccp_get_sem_table (int sem)
{
    sem_table_t *sem_table;
    
    switch (sem) {
    case (SCCP_SEM_CC):
        sem_table = &sccpcc_sem_table;
        break;

    case (SCCP_SEM_CM) :
        sem_table = &sccpcm_sem_table;
        break;

    case (SCCP_SEM_REC) :
        sem_table = &sccprec_sem_table;
        break;

    case (SCCP_SEM_REG) :
        sem_table = &sccpreg_sem_table;
        break;

    default:
        sem_table = NULL;
        break;
    }

    return (sem_table);
}

char *sccp_get_sem_name (unsigned long id)
{
    sccp_sccp_event_t *sccp_event;
    sem_table_t       *table;

    sccp_event = sccp_sccp_to_sccp_event(id);
    if (sccp_event == NULL) {
        return (SCCP_UNDEFINED);
    }

    table = sccp_get_sem_table(sccp_event->sem);
    if (table == NULL) {
        return (SCCP_UNDEFINED);
    }

    return (table->sem_name(0));
}

#ifdef SCCP_USE_POOL
/*
 * Flip on if you want the pool stats.
 */
//#define SCCP_USE_POOL_STATS
#ifdef SCCP_USE_POOL_STATS
#define SCCP_MAX_POOL_STATS 5120
typedef struct sccp_pool_stats_t_ {
    unsigned int size;
    int count[SCCP_POOL_MAX];
    char timebuf[9];
} sccp_pool_stats_t;
sccp_pool_stats_t sccp_pool_stats[SCCP_MAX_POOL_STATS];
static int sccp_pool_stats_count = 0;

static void sccp_set_pool_stats (unsigned int size, char *fname, int line,
                                 void *ptr)
{
    sccp_pool_stats_t *stats = &(sccp_pool_stats[sccp_pool_stats_count]);
    static char buf[128];
    sccp_pool_table_element_t *element;
    int i = 0;
    static int counter = 0;

    if (sccp_pool_stats_count >= SCCP_MAX_POOL_STATS) {
        return;
    }

    stats->size = size;

    for (element = sccp_pool_table;
         element < &(sccp_pool_table[SCCP_POOL_MAX]);
         element++){
        
        stats->count[i++] = sllist_get_list_size(element->list);
    }

    /* 
     * Add a timestamp because the write_file routine does not add timestamps.
     */
    SCCP_STRTIME(stats->timebuf);

    SCCP_SNPRINTF((buf, sizeof(buf), "[%s] % 4d % 4d  [% 4d  % 4d  % 4d  % 4d]  "
                   "0x%08x: (% 5d)%s\n", 
                   stats->timebuf, ++counter, stats->size, stats->count[0],
                   stats->count[1], stats->count[2], stats->count[3],
                   ptr, line, fname+30)); /* +30 removes most of the path */
    
#ifdef SCCP_PLATFORM_WINDOWS
    sccp_platform_write_file(1, buf);
#endif
    /*
     * Skip the first 11 chars because the SCCP_DBG will add another
     * timestamp.
     */
    SCCP_DBG((sccp_debug, 9, buf + 11, "%s", buf));

    sccp_pool_stats_count++;
}
#endif

#if 0
static void sccp_set_pool_stats_debug (void *ptr, unsigned int size,
                                       char *filename, int line)
{
    static char buf[64];
    static counter = 0;

    sccp_set_pool_stats(size);

    SCCP_SNPRINTF((buf, sizeof(buf), "      %04d 0x%08x: %s: %d\n",
                  ++counter, ptr, filename + 30, line));

#ifdef SCCP_PLATFORM_WINDOWS
    sccp_platform_write_file(1, buf);
#endif
    SCCP_DBG((sccp_debug, 9, buf, "%s\n", buf));
}
#endif

static void *sccp_pool_get_list_by_type (int type)
{
    void *list = NULL;
    sccp_pool_table_element_t *element;

    for (element = sccp_pool_table;
         element < &(sccp_pool_table[SCCP_POOL_MAX]);
         element++){

        if (type == (int )(element->size)) {
            list = element->list;
            break;
        }
    }

    return (list);
}

static void *sccp_pool_get_list_by_addr (unsigned long *addr)
{
    void *list = NULL;
    sccp_pool_table_element_t *element;

    for (element = sccp_pool_table;
         element < &(sccp_pool_table[SCCP_POOL_MAX]);
         element++){

        if ((addr >= element->pool_start) &&
            (addr <= element->pool_end)) {
            
            list = element->list;
            break;
        }
    }

    return (list);
}

static void *sccp_pool_get_list_by_size (unsigned int size)
{
    void *list = NULL;
    sccp_pool_table_element_t *element;

    for (element = sccp_pool_table;
         element < &(sccp_pool_table[SCCP_POOL_MAX]);
         element++) {

        if (size <= element->size) {
            list = element->list;
            break;
        }
    }

    return (list);
}

static void *sccp_pool_get_list_by_next (void *list)
{
    void *list_found = NULL;
    sccp_pool_table_element_t *element;

    /*
     * Check all elements except the last. There is not a next after last.
     */
    for (element = sccp_pool_table;
         element < &(sccp_pool_table[SCCP_POOL_MAX - 1]);
         element++) {

        if (list == element->list) {
            list_found = (++element)->list;
            break;
        }
    }

    return (list_found);
}

#ifdef SCCP_USE_POOL_STATS
void *sccp_pool_malloc (int type, unsigned int size, char *fname, int line)
#else
void *sccp_pool_malloc (int type, unsigned int size)
#endif
{
    void *list = NULL;
    void *node = NULL;

    if (type != SCCP_POOL_MIN) {
        list = sccp_pool_get_list_by_type(type);
        node = sllist_remove_node(list, 1);
    }
    else {
        /*
         * Try to grab a node from the smallest sized list that matches
         * the requested size. If none available, then try next size up
         * until all lists are exhausted.
         */
        list = sccp_pool_get_list_by_size(size);
        while (list != NULL) {
            node = sllist_remove_node(list, 1);
            if (node != NULL) {
                break;
            }

            list = sccp_pool_get_list_by_next(list);
        } 
    }
    
#ifdef SCCP_USE_POOL_STATS
    sccp_set_pool_stats(size, fname, line, node);
#endif
    return (node);
}

#if 0
void *sccp_pool_malloc_debug (int type, unsigned int size,
                              char *filename, int line)
{
    void *ptr;

    ptr = sccp_pool_malloc(type, size);
    sccp_set_pool_stats_debug(ptr, size, filename, line);

    return (ptr);
}
#endif
#ifdef SCCP_USE_POOL_STATS
void sccp_pool_free (int type, void *ptr, char *fname, int line)
#else
void sccp_pool_free (int type, void *ptr)
#endif
{
    void *list = NULL;

    if (type != SCCP_POOL_MIN) {
        list = sccp_pool_get_list_by_type(type);
    }
    else {
        list = sccp_pool_get_list_by_addr(ptr);
    }

    if (list == NULL) {
        SCCP_DBG((sccp_debug, 9, "%s: Invalid address= %p\n",
                 SCCP_ID, ptr));

        return;
    }

    /*
     * Put the node back on the front of the list.
     */
    sllist_add_node(list, ptr, 1);

#ifdef SCCP_USE_POOL_STATS
    sccp_set_pool_stats(0, fname, line, ptr);
#endif
}

#if 0
void sccp_pool_free_debug (int type, void *ptr, char *filename, int line)
{
    sccp_pool_free(type, ptr);
    sccp_set_pool_stats_debug(ptr, 0, filename, line);
}
#endif 

void sccp_pool_delete_all ()
{
    int i;

    for (i = 0; i < SCCP_POOL_MAX; i++) {
        sccp_pool_table[i].list = sllist_delete_list(sccp_pool_table[i].list);
    }
}

static void sccp_free_pool2 (void *node)
{
}

#endif

static void sccp_free_event (sem_event_t *event)
{
    if ((event != NULL) && (event->cb2 != NULL)) {
        sllist_delete_node(((sccp_sccpcb_t *)(event->cb2))->events, event);
    }
}

/* node has already been removed from list so just free the memory*/
static void sccp_free_event2 (sem_event_t *event)
{
    if (event != NULL) {
        if (event->data != NULL) {
            SCCP_FREE(event->data);
        }
    
        SCCP_FREE(event);
    }
}

#if 0
static int sccp_get_new_event_id (int *id)
{
//    static int id = 0;

    if (++(*id) < 0) {
        *id = 1;
    }

    return (*id);
}
#endif
static sem_event_t *sccp_get_new_event (sccp_sccpcb_t *sccpcb, int front)
{
    char        *fname = "get_new_event";
    sem_event_t *event = NULL;

    event = (sem_event_t *)(SCCP_MALLOC(sizeof(*event)));
    if (event == NULL) {
        SCCP_DBG((sccp_debug, 5,
                 "%s %-5d: %-20s: Unable to allocate memory for queue event.\n",
                 SCCP_ID, sccpcb->id, fname));
        return (NULL);
    }
    SCCP_MEMSET(event, 0, sizeof(*event));
    
    if (sllist_add_node(sccpcb->events, event, front) == NULL) {
        SCCP_DBG((sccp_debug, 5,
                 "%s %-5d: %-20s: Unable to add queue event to list.\n",
                 SCCP_ID, sccpcb->id, fname));
        sccp_free_event2(event);
        return (NULL);
    }

    event->id = sllist_get_new_id(sccpcb->events, &(sccpcb->event_id));
    //event->id = sccp_get_new_event_id(sccpcb->events, &(sccpcb->event_id));
    event->cb2 = sccpcb;

    return (event);
}

static int sccp_push_event_setup (sccp_sccpcb_t *sccpcb, sem_table_t *table,
                                  sem_event_t **event, int event_id,
                                  char *fname, int front)
{
    *event = sccp_get_new_event(sccpcb, front);
    if (*event == NULL) {
        return (21);
    }

    return (0);
}

int sccp_debug_show (sem_table_t *table, int msg_id)
{
    int show = 0;

    if (table != NULL) {
        if ((table == &sccpcc_sem_table) && (sccpcc_debug > 0)) {
            show = 1;
        }
        else if ((table == &sccpcm_sem_table) && (sccpcm_debug > 0)) {
            show = 1;
        }
        else if ((table == &sccprec_sem_table) && (sccprec_debug > 0)) {
            show = 1;
        }
        else if ((table == &sccpreg_sem_table) && (sccpreg_debug > 0)) {
            show = 1;
        }
    }
    else if (msg_id > -1) {
        sccp_sccp_event_t *sccp_event;

        sccp_event = sccp_sccp_to_sccp_event(msg_id);
        if (sccp_event == NULL) {
            return (show);
        }

        if ((sccp_event->sem == SCCP_SEM_CC) && (sccpcc_debug > 0)) {
            show = 1;
        }
        else if ((sccp_event->sem == SCCP_SEM_CM) && (sccpcm_debug > 0)) {
            show = 1;
        }
        else if ((sccp_event->sem == SCCP_SEM_REC) && (sccprec_debug > 0)) {
            show = 1;
        }
        else if ((sccp_event->sem == SCCP_SEM_REG) && (sccpreg_debug > 0)) {
            show = 1;
        }
    }

    return (show);
}

/*sam add #define SCCP_DONT_COPY (-1) so users can use it instead of passing
*     0 as the copy_len. Then change if to check for SCCP_DONT_COPY.
*/
int sccp_push_event (sccp_sccpcb_t *sccpcb, void *data, int copy_len,
                     int event_id, sem_table_t *table, void *cb, int front)
{
    char        *fname = "push_event";
    sem_event_t *event = NULL;
    int         rc;

    if ((cb == NULL) || (table == NULL) || (sccpcb == NULL)) {
        SCCP_DBG((sccp_debug, 9, "oops\n"));
        return (1);
    }

    rc = sccp_push_event_setup(sccpcb, table, &event, event_id, fname, front);
    if (rc != 0) {
        return (rc);
    }

    /*
     * Do we need to copy the data?
     */
    if ((copy_len > 0) && (data != NULL)) {
        event->data = SCCP_MALLOC(copy_len);
        if (event->data == NULL) {
            SCCP_DBG((sccp_debug, 5,
                     "%s %-5d: %-20s: Unable to allocate "
                     "memory for queue event data.\n",
                     SCCP_ID, sccpcb->id, fname));
            sccp_free_event(event);
            return (32);
        }
        SCCP_MEMSET(event->data, 0, copy_len);
        SCCP_MEMCPY(event->data, data, copy_len);
    }
    else {
        event->data = data;
    }

    if ((sccp_debug_show(table, -1) == 1) || (1)){
        SCCP_DBG((sccp_debug, 5,
                 "%s %-5d: %-20s: %s <- %s.\n",
                 SCCP_ID, sccpcb->id, fname, table->sem_name(0),
                 table->event_name(event_id)));
    }

    event->table    = table;
    event->cb       = cb;
    event->event_id = event_id;

    SCCP_THREAD_RUN(sccpcb->event_run);

    return (0);
}

/*
 * FUNCTION:    sccp_event_main
 *
 * DESCRIPTION: Main event hadnler for PSCCP.
 *
 * PARAMETERS:
 *     handle:  identifes this instance
 *     data:    user supplied data (not used)
 *
 * RETURNS:     0 for success, otherwise 1.
 *
 * NOTES:
 */
int sccp_event_main (void *handle, void *data)
{
//    char          *fname = "event_main";
    sem_cbhdr_t   *cbhdr;
    sem_event_t   *event;
    sccp_sccpcb_t *sccpcb = handle;

    if (sccpcb == NULL) {
        return (1);
    }

    event = sllist_remove_node(sccpcb->events, 1);
    if (event == NULL) {
        return (0);//sllist_get_list_size(sccpcb->events));
    }

    cbhdr = (sem_cbhdr_t *)(event->cb);
    if (cbhdr == NULL) {
        sccp_free_event2(event); /* node already removed, so just free it */
        return (0);//sllist_get_list_size(sccpcb->events));
    }
    /*
     * Validate the cb. It is possible that the cb has been removed from
     * the list. Here is one scenario:
     * - release_complete is received from the app,
     * - thread switch to TCP task - ccm sends a bunch of other sccp messages
     *   that the tcp task receives and dumps onto the queue - which means the
     *   cccb is found
     * - thread switch back to SCCP task - stack frees the cccb since it
     *   received the release_complete
     * - we are here and try to run with the invalid cccb which was copied 
     *   to the events generated with the bunch of messages the ccm sent.
     *
     * Pass the cc events to the default cccb.
     */
    if ((event->table != NULL) && (event->table->validate_cb != NULL) &&
        (event->table->validate_cb(sccpcb, cbhdr) != 0)) {

        /*
         * Pass cc events to the default cccb if the cccb that was
         * handling them was freed.
         */
        if (event->table == &sccpcc_sem_table) {
            event->cb = sccpcb->cccbs->next;
            cbhdr = (sem_cbhdr_t *)(event->cb);
            if (cbhdr == NULL) {
                sccp_free_event2(event); /* node already removed, so just free it */
                return (0);//sllist_get_list_size(sccpcb->events));
            }
        }
        else {
            sccp_free_event2(event); /* node already removed, so just free it */
            return (0);//sllist_get_list_size(sccpcb->events));
        }
    }

    event->state_id = cbhdr->state;
#if 0 /* test21 *//* test22 *//* test23 *//* test24 *//* test25 */
      /* test71 */
    {
        int flags = -1;
        static int flags2 = -1;
        int toss = 0;
        sccpcm_cmcb_t *cmcb;

        flags = sapp_get_test_flags();
        if (flags != -1) {
            if (event->table == &sccpcm_sem_table) {
                cmcb = (sccpcm_cmcb_t *)(event->cb);

                if (cmcb->index == flags) {
                    switch (event->event_id) {
                    case (SCCPCM_E_KEEPALIVE_ACK):
                    //case (SCCPCM_E_CON_ACK):
                    //case (SCCPCM_E_REG_ACK):
                        toss = 1;
                        break;

                    //case (SCCPCM_E_REG_REQ):
                        if (flags2 != -1) {
                            toss = 1;
                            flags2 = -1;
                        }
                        break;

                    //case (SCCPCM_E_RECV_TOKEN):
                        flags2 = 0;
                        break;
                    }
                }
            }
        }
        else {
            flags2 = -1;
        }

        if (toss == 1) {
            SCCP_DBG((sccp_debug, 5,
                     "%s %-5d: %-20s: %s: tossing event= %s\n",
                     SCCPCM_ID, cmcb->id, fname,
                     sccpcm_print_cm(cmcb),
                     sccpcm_sem_table.event_name(event->event_id)));
            
            sccp_free_event2(event);
            return (0);//sllist_get_list_size(sccpcb->events));
        }
    }
#endif

    sem_process_event(event);

    sccp_free_event2(event); /* node already removed, so just free it */

    /*
     * Check if the sccprec wanted to cleanup the event list. This ensures
     * that the list does not contain events not processed from
     * a previous session. This happens for reset-resets and session closing.
     */
    if (sccpcb->reccb->free_events == 1) {
        sllist_empty_list(sccpcb->events);
    }

    return (0);//sllist_get_list_size(sccpcb->events));
}

static int sccp_push_tcp (sccp_sccpcb_t *sccpcb, sccpmsg_general_t *msg,
                          int msgsize, unsigned long socket)
{
    char              *fname       = "push_tcp";
    sccp_sccp_event_t *sccp_event;
    void              *cb          = NULL;
    sem_table_t       *table;
    sccpmsg_general_t *msg2;

    if (msg == NULL) {
        SCCP_DBG((sccp_debug, 5, "\n"));
        SCCP_DBG((sccp_debug, 5,
                 "%s %-5d: %-20s: ERROR: SCCP message received with no data.\n",
                 SCCP_ID, sccpcb->id, fname));
        return (1);
    }

#if 0
    /*
     * Make sure the msgsize matches what we think the 
     * message size should be.
     */
    if ((unsigned int )msgsize < (sccp_event->size + sizeof(sccpmsg_base_t))) {
        SCCP_MEMCPY(msg2, msg, msgsize);
    }
    else {
        SCCP_MEMCPY(msg2, msg, (sccp_event->size + sizeof(sccpmsg_base_t)));
    }
#endif

    /*
     * message_id has not been converted yet.
     */
    sccp_event = sccp_sccp_to_sccp_event((int)(CMTOSL(msg->body.msg_id.message_id)));
    if (sccp_event == NULL) {
        return (3);
    }

    msg2 = (sccpmsg_general_t *)SCCP_MALLOC(sccp_event->size +
                                            sizeof(sccpmsg_base_t));
    if (msg2 == NULL) {
        return (4);
    }

    SCCP_MEMCPY(msg2, msg, (sccp_event->size + sizeof(sccpmsg_base_t)));

    /*
     * Perform any big-endian/little-endian conversions.
     */
    if (sccpmsg_parse_msg(msg2, 0, 1, 1, sccpcb) == 0) {
        if (sccp_debug_show(NULL, (int)(msg2->body.msg_id.message_id)) == 1) {
            SCCP_DBG((sccp_debug, 9, "\n"));
            SCCP_DBG((sccp_debug, 9, "%s %-5d: %-20s: recv[%i] <- %s[0x%x] "
                                    "%d:%d:%d\n",
                     SCCP_ID, sccpcb->id, fname, socket,
                     sccpmsg_get_message_text(msg2->body.msg_id.message_id),
                     msg2->body.msg_id.message_id,
                     msg2->base.length, 0 , msgsize));
        }
#ifdef SCCP_MSG_DUMP
        sccpmsg_dump(msg2);
#endif
    }
    else {
        SCCP_FREE(msg2);
        return (5);
    }

    switch (sccp_event->sem) {
    case (SCCP_SEM_CC):
        /*
         * Check the mode - does the application want to use GAPI or the
         * raw SCCP messages for call contrlo.
         */
        if (sccpcb->reccb->cc_mode != GAPI_CC_MODE_PASSTHRU) {
            cb = sccpcc_get_cccb_by_sccpmsg(sccpcb, msg2);
        }
        else {
            /*
             * Just give the message to the application.
             */
            SCCP_PASSTHRU(sccpcb->reccb, GAPI_MSG_PASSTHRU, msg2,
                          sccp_event->size + sizeof(sccpmsg_base_t));
            SCCP_FREE(msg2);
            return (0);
        }

        break;

    case (SCCP_SEM_CM):
        cb = sccpcm_get_cmcb_by_socket(sccpcb, socket);
#if 0 /* test21 */
        if (sccp_event->event == SCCPCM_E_KEEPALIVE_ACK) {
            return (4);
        }
#endif
        break;

    case (SCCP_SEM_REC):
        cb = sccpcb->reccb;
        break;

    case (SCCP_SEM_REG):
        cb = sccpcb->regcb;
        break;

    default:
        break;
    }

    if (cb == NULL) {
        SCCP_FREE(msg2);
        return (11);
    }

    table = sccp_get_sem_table(sccp_event->sem);
    if (table == NULL) {
        SCCP_FREE(msg2);
        return (21);
    }
    
    if (sccp_push_event(sccpcb, msg2, 0, sccp_event->event,
                        table, cb, 0) != 0) {

        SCCP_FREE(msg2);
    }

    return (0);
}

static void sccp_init_chunk_data (sccpcm_chunk_data_t *chunk_data)
{
    chunk_data->presidual_buffer = chunk_data->residual_buffer;
    chunk_data->pnext_chunk = NULL;
    chunk_data->pending_bytes = 0;
    chunk_data->bytes_left = 0;
}

static void *sccp_next_chunk (sccpcm_chunk_data_t *chunk_data,
                              unsigned char *msg, unsigned int msglen)
{
    unsigned char     *preturn_chunk = NULL;
    int               chunk_length = 0;
    sccpmsg_general_t *gen_msg;

    /*
     * Make sure we have a residual buffer.
     */
    if (chunk_data->residual_buffer == NULL) {
        chunk_data->residual_buffer =
            (unsigned char *)(SCCP_MALLOC(SCCP_CHUNK_DATA_BUFFER_SIZE));
    
        if (chunk_data->residual_buffer == NULL) {
            return (NULL);
        }

        sccp_init_chunk_data(chunk_data);
    }

    /*
     * If this is a new message then check to see if the last message
     * ended on an even chunk boundary. If not, then combine this message
     * with what is leftover from the last one.
     */
    if (msg != NULL) {
        /*
         * If pending bytes is negative, then we need to get the rest of the
         * length field before we can get the next chunk. This might happen
         * if there was a message spread across two packets.
         */
        if (chunk_data->pending_bytes < 0) {
            gen_msg = (sccpmsg_general_t *)(chunk_data->residual_buffer);
            
            SCCP_MEMCPY(chunk_data->presidual_buffer, msg,
                        -(chunk_data->pending_bytes));

            gen_msg->base.length = CMTOSL(gen_msg->base.length);
#if 0
            chunk_data->pending_bytes = sizeof(sccpmsg_base_t) + 
                                        gen_msg->base.length -
                                        (int)(sizeof(gen_msg->base.length)) +
                                        chunk_data->pending_bytes;

#else
            chunk_data->pending_bytes = sizeof(sccpmsg_base_t) + 
                                        gen_msg->base.length -
                                        ((int)(sizeof(gen_msg->base.length)) +
                                         chunk_data->pending_bytes);
#endif
        } /* if (chunk_data->pending_bytes < 0) { */

        /*
         * Check if we had any residual data left from a previous packet.
         */
        if (chunk_data->pending_bytes > 0) {
            /*
             * Does this message have all of the pending bytes?
             */
            if (chunk_data->pending_bytes <= (int)msglen) {
                /*
                 * Copy the remaining bytes to the return buffer.
                 */
                SCCP_MEMCPY(chunk_data->presidual_buffer, msg,
                            chunk_data->pending_bytes);

                preturn_chunk = chunk_data->residual_buffer;
                chunk_data->pnext_chunk = &(msg[chunk_data->pending_bytes]);

                /*
                 * Reset the buffer pointer.
                 */
                chunk_data->presidual_buffer = chunk_data->residual_buffer;
                chunk_data->bytes_left = msglen - chunk_data->pending_bytes;
                chunk_data->pending_bytes = 0;
            }
            /*
             * Not enough bytes to finish the message. Copy what we have and
             * update the pending bytes count and residual buffer.
             */
            else {
                SCCP_MEMCPY(chunk_data->presidual_buffer, msg, msglen);

                preturn_chunk = NULL; /* don't have a full message yet. */
                chunk_data->presidual_buffer += msglen;
                chunk_data->pending_bytes -= msglen;
                chunk_data->pnext_chunk = NULL;
                chunk_data->bytes_left = 0;
            }
        } /* if (chunk_data->pending_bytes > 0) { */
        /*
         * New message, but no residual data. We assume that there are at least
         * four bytes included (for the message length).
         */
        else {
            gen_msg = (sccpmsg_general_t *)msg;
            preturn_chunk = msg;
            
            gen_msg->base.length = CMTOSL(gen_msg->base.length);
            chunk_length = sizeof(sccpmsg_base_t) + gen_msg->base.length;

            /*
             * Check if we got a whole message in this packet. If so,
             * we can return it.
             */
            if (chunk_length <= (int)msglen) {
                chunk_data->pending_bytes = 0;
                chunk_data->bytes_left = msglen - chunk_length;
                chunk_data->pnext_chunk = &(preturn_chunk[chunk_length]);
            }
            /*
             * Didn't get the whole message. Copy what is there and wait
             * for the next packet to get the rest.
             */
            else {
                chunk_data->pending_bytes = chunk_length - msglen;
                SCCP_MEMCPY(chunk_data->residual_buffer, msg, msglen);
                chunk_data->presidual_buffer = &(chunk_data->residual_buffer[msglen]);
                chunk_data->bytes_left = 0; /* nothing left in this packet. */
                preturn_chunk = NULL; /* don't have a full message yet. */
                chunk_data->pnext_chunk = NULL; /* nothing left in this packet. */
            } /* if (chunk_length <= msglen) { */
        }
    } /* if (msg != NULL) {*/
    /*
     * This is data left over from a packet that had more than one 
     * message.
     */
    else {
        /*
         * As long as the number of message bytes left is >= the size of the
         * length field, we know that the length will get copied into the
         * residual buffer so we don't have to do anything special.
         */
        gen_msg = (sccpmsg_general_t *)(chunk_data->pnext_chunk);

        if (chunk_data->bytes_left >= sizeof(gen_msg->base.length)) {
            
            /*
             * First time looking at this message in the packet, so go
             * ahead and fix the length.
             */
            gen_msg->base.length = CMTOSL(gen_msg->base.length);
            
            chunk_length = sizeof(sccpmsg_base_t) + gen_msg->base.length;

            /*
             * Check if we got a whole message. If so,
             * we can return it.
             */
            if (chunk_length <= chunk_data->bytes_left) {
                preturn_chunk = chunk_data->pnext_chunk;
                chunk_data->bytes_left -= chunk_length;
                chunk_data->pnext_chunk = &(preturn_chunk[chunk_length]);
                chunk_data->pending_bytes = 0;
            }
            /*
             * Didn't get the whole message. Copy what is there and wait
             * for the next packet to get the rest.
             */
            else {
                chunk_data->pending_bytes = chunk_length -
                                            chunk_data->bytes_left;

                SCCP_MEMCPY(chunk_data->residual_buffer,
                            chunk_data->pnext_chunk, chunk_data->bytes_left);

                chunk_data->presidual_buffer = &(chunk_data->residual_buffer[chunk_data->bytes_left]);
                preturn_chunk = NULL; /* don't have a full message yet. */
                chunk_data->pnext_chunk = NULL; /* nothing left in this packet. */
                chunk_data->bytes_left = 0; /* used all the bytes */
            } /* if (chunk_length <= chunk_data->bytes_left) { */
        }
        /*
         * Don't have a complete length field, so we copy what we have
         * and set the pending bytes to a negative number to indicate that we
         * need to get the rest of the length field before we get the next 
         * chunk.
         */
        else {
            if (chunk_data->bytes_left > 0) {
                chunk_data->pending_bytes = chunk_data->bytes_left - 
                                            (int)(sizeof(gen_msg->base.length));

                SCCP_MEMCPY(chunk_data->residual_buffer,
                            chunk_data->pnext_chunk, chunk_data->bytes_left);

                chunk_data->presidual_buffer = &(chunk_data->residual_buffer[chunk_data->bytes_left]);
                chunk_data->pnext_chunk = NULL; /* nothing left in this packet. */
                chunk_data->bytes_left = 0; /* used all the bytes */
            }

            preturn_chunk = NULL; /* don't have a complete message yet. */
        } /* if (chunk_data->bytes_left >= */
    } /* if (msg != NULL) { */

    return (preturn_chunk);
}

#if 0
static int sccp_tcp_recv (sccp_sccpcb_t *sccpcb, void *data,
                          unsigned long size, unsigned int socket)
{
    char              *fname         = "tcp_recv";
    unsigned long     i;
    unsigned long     total_msg_size = 0;
    char              *msg_buffer    = (char *)data;
    sccpmsg_general_t *msg           = (sccpmsg_general_t *)msg_buffer;

    if (msg == NULL) {
        SCCP_DBG((sccp_debug,5, 
                 "%s %-5d: %-20s: ERROR: msg is NULL, size = %d\n",
                 SCCP_ID, sccpcb->id, fname, size));
        return (1);
    }

    if (size < sizeof(sccpmsg_base_t)) {
        SCCP_DBG((sccp_debug, 5, 
                 "%s %-5d: %-20s: ERROR: size < base, size = %d\n",
                 SCCP_ID, sccpcb->id, fname, size));
        return (2);
    }

    if (size > SCCPMSG_MAX_BUFFER_SIZE) {
        SCCP_DBG((sccp_debug, 5,
                 "%s %-5d: %-20s: ERROR: size > buffer, size = %d\n",
                 SCCP_ID, sccpcb->id, fname, size));
        return (3);
    }

    msg->base.length = CMTOSL(msg->base.length);
    if (size < (msg->base.length + sizeof(sccpmsg_base_t))) { /* add the header */
        SCCP_DBG((sccp_debug, 5,
                 "%s %-5d: %-20s: ERROR: size < msg, size = %d\n",
                 SCCP_ID, sccpcb->id, fname, size));
        return (4);
    }

    /*
     * Check for the number of messages in the buffer
     * We could have 1 or more than 1
     */
    for (i = 0; i < size;) {
        total_msg_size = msg->base.length + sizeof(sccpmsg_base_t);

        /*sam
         * simulate a keepalive_ack, since a message was received.
         */
        /*sam
         * may want to allocate the size of the specific message here. Then
         * the parse can always assume the latest protocol messages are
         * available and not have to worry about running off the end of the 
         * message.
         */
        msg->body.msg_id.message_id = CMTOSL(msg->body.msg_id.message_id);

        if (sccp_debug_show(NULL, (int)(msg->body.msg_id.message_id)) == 1) {
            SCCP_DBG((sccp_debug, 9, "\n"));
            SCCP_DBG((sccp_debug, 9, "%s %-5d: %-20s: recv[%i] <- %s[0x%x] "
                                    "%d:%d:%d:%d\n",
                     SCCP_ID, sccpcb->id, fname, socket,
                     sccpmsg_get_message_text(msg->body.msg_id.message_id),
                     msg->body.msg_id.message_id,
                     msg->base.length, total_msg_size, i, size));
        }

        sccp_push_tcp(sccpcb, msg, total_msg_size, socket);

        /*
         * Try to parse the rest of the data if there is any left.
         */
        i += total_msg_size;

        if (i >= size) {
            break;
        }

        SCCP_MEMCHK(110);
        
        if (sccp_tcp_recv(sccpcb, msg_buffer + i, size - i, socket) != 0) {
            return (10);
        }

        break;

    }
    
    return (0);
}
#endif
static int sccp_tcp_recv (sccp_sccpcb_t *sccpcb, void *data,
                          unsigned long size, unsigned int socket)
{
    char *fname = "tcp_recv";
    void *temp;
    sccpcm_chunk_data_t *chunk_data;

    /*
     * Make sure we received some data.
     */
    if (data == NULL) {
        SCCP_DBG((sccp_debug,5, 
                 "%s %-5d: %-20s: ERROR: msg is NULL, size = %d\n",
                 SCCP_ID, sccpcb->id, fname, size));
        return (1);
    }

    /*
     * Make sure the received data will not overflow our buffer.
     */
    if (size > SCCPMSG_MAX_BUFFER_SIZE) {
        SCCP_DBG((sccp_debug, 5,
                 "%s %-5d: %-20s: ERROR: size > buffer, size = %d\n",
                 SCCP_ID, sccpcb->id, fname, size));
        return (3);
    }

    /*
     * Get the chunk data for this socket since each socket has
     * their own chunk data.
     */
    chunk_data = sccpcm_get_chunk_data(sccpcb, socket);
    if (chunk_data == NULL) {
        return (11);
    }

    temp = sccp_next_chunk(chunk_data, 
                           (unsigned char *)data, (unsigned int)size);

    while (temp != NULL) {
        sccp_push_tcp(sccpcb, temp, size, socket);

        temp = sccp_next_chunk(chunk_data, NULL, (unsigned int)size);
    }

    return (0);
}

int sccp_tcp_main (void *handle, int msg_id, int socket,
                   gapi_tcp_events_e event, void *data, int size)
{
    char          *fname  = "tcp_main";
    sccp_sccpcb_t *sccpcb = handle;

    if (sccpcb == NULL) {
        SCCP_DBG((sccp_debug, 5,
                 "%s %-5d: %-20s: invalid sccpcb= 0x%08p\n",
                 SCCP_ID, 0, fname, sccpcb));
            
        return (11);                   
    }

    switch (event) {
    case (GAPI_TCP_EVENT_RECV):
#if 0 /* test201-206 */
        /* using a register_ack message, chop the message into the
         * right pieces for the test.
         */
        {
            int flags = -1;

            flags = sapp_get_test_flags();
            if (flags != -1) {
                switch (flags) {
                    /* register_ack is 32 bytes */
                case (1):
                    break;
                
                case (2): /* test 202 */
                    sccp_tcp_recv(sccpcb, data, size, socket);
                    sccp_tcp_recv(sccpcb, (char *)data+size/2, size/2, socket);
                    break;

                case (3): /* test 203 */
                    sccp_tcp_recv(sccpcb, data, 8, socket);
                    sccp_tcp_recv(sccpcb, (char *)data+8, 8, socket);
                    sccp_tcp_recv(sccpcb, (char *)data+16, 16, socket);
                    break;
                
                case (4): /* test 204 */
                    *((unsigned long *)data) = 24;
                    sccp_tcp_recv(sccpcb, data, size, socket);
                    break;
                
                case (5): /* test 205 */
                    *((unsigned long *)data) = 24;
                    sccp_tcp_recv(sccpcb, data, 33, socket);
                    sccp_tcp_recv(sccpcb, (char *)data+33, 31, socket);
                    break;

                case (6): /* test 206 */
                    *((unsigned long *)data) = 24;
                    sccp_tcp_recv(sccpcb, data, 34, socket);
                    sccp_tcp_recv(sccpcb, (char *)data+34, 20, socket);
                    sccp_tcp_recv(sccpcb, (char *)data+54, 10, socket);
                    break;
                }
            }
            else {
                sccp_tcp_recv(sccpcb, data, size, socket);
            }
        }
#else
        sccp_tcp_recv(sccpcb, data, size, socket);
#endif
        break;

    default:
        sccpcm_push_tcp_event(sccpcb, msg_id,
                              socket, event, data, size);
        break; 
    }

    return (0);
}

static int sccp_sccp_cleanup (sccp_sccpcb_t *sccpcb, void *data)
{
    sccp_free_sccpcb(sccpcb);

    return (0);
}

void sccp_set_callbacks (gapi_callbacks_t *cbs)
{
    SCCP_MEMSET(cbs, 0, sizeof(*cbs));

    cbs->setup                   = (gapi_setup_f *)sccpcc_push_setup;
    cbs->setup_ack               = (gapi_setup_ack_f *)sccpcc_push_setup_ack;
    cbs->proceeding              = (gapi_proceeding_f *)sccpcc_push_proceeding;
    cbs->alerting                = (gapi_alerting_f *)sccpcc_push_alerting;
    cbs->connect                 = (gapi_connect_f *)sccpcc_push_connect;
    cbs->connect_ack             = (gapi_connect_ack_f *)sccpcc_push_connect_ack;
    cbs->disconnect              = (gapi_disconnect_f *)sccpcc_push_disconnect;
    cbs->release                 = (gapi_release_f *)sccpcc_push_release;
    cbs->release_complete        = (gapi_release_complete_f *)sccpcc_push_release_complete;
    cbs->openrcv_res             = (gapi_openrcv_res_f *)sccpcc_push_openrcv_res;
    cbs->opensession_req         = (gapi_opensession_req_f *)sccprec_push_opensession_req;
    cbs->resetsession_req        = (gapi_resetsession_req_f *)sccprec_push_resetsession_req;
    cbs->digits                  = (gapi_digits_f *)sccpcc_push_digits;
    cbs->feature_req             = (gapi_feature_req_f *)sccpcc_push_feature_req;
    cbs->feature_res             = (gapi_feature_res_f *)sccpcc_push_feature_res;
    cbs->devicetouserdata_req    = (gapi_devicetouserdata_req_f *)sccpcc_push_devicetouserdata_req;
    cbs->devicetouserdata_res    = (gapi_devicetouserdata_res_f *)sccpcc_push_devicetouserdata_res;
    cbs->sccp_cleanup            = (gapi_sccp_cleanup_f *)sccp_sccp_cleanup;
    cbs->sccp_main               = (gapi_sccp_main_f *)sccp_event_main;
    cbs->tcp_main                = (gapi_tcp_main_f *)sccp_tcp_main;
}

#if 0
static int sccp_get_new_sccpcb_id (void)
{
    static int id = 0;

    if (++id < 0) {
        id = 1;
    }

    return (id);
}
#endif

void sccp_free_sccpcb (sccp_sccpcb_t *sccpcb)
{
    sllist_delete_node(sccp_sccpcbs, sccpcb);
}

void sccp_free_sccpcb2 (sccp_sccpcb_t *sccpcb)
{
    if (sccpcb == NULL) {
        return;
    }

    sccpcb->events = sllist_delete_list(sccpcb->events);

    sccpcb->cccbs = sllist_delete_list(sccpcb->cccbs);
    
    sccpcb->cmcbs = sllist_delete_list(sccpcb->cmcbs);
    
    sccprec_free_reccb(sccpcb->reccb);
    sccpcb->reccb = NULL;
    
    sccpreg_free_regcb(sccpcb->regcb);
    sccpcb->regcb = NULL;

    /*
     * Free the appcbs last because one of the other sems might use them 
     * during cleanup, like sccpcm_cm_disconnect calls sessionstatus to report
     * that the CM is down while disconnecting.
     */
    if (sccpcb->appcbs) {
        SCCP_FREE(sccpcb->appcbs);
        sccpcb->appcbs = NULL;
    }

    SCCP_FREE(sccpcb);
}

void *sccp_get_new_sccpcb (void)
{
    char          *fname   = "get_new_sccpcb";
    sccp_sccpcb_t *sccpcb;
    sccpcc_cccb_t *cccb;
    unsigned long endian   = 1;
   
    sccpcb = (sccp_sccpcb_t *)(SCCP_MALLOC(sizeof(*sccpcb)));
    if (sccpcb == NULL) {
        return (NULL);
    }

    SCCP_MEMSET(sccpcb, 0, sizeof(*sccpcb));

    sccpcb->id = sllist_get_new_id(sccp_sccpcbs, &sccp_sccpcb_id);

    if (sllist_add_node(sccp_sccpcbs, sccpcb, 0) == NULL) {
        sccp_free_sccpcb2(sccpcb);
        return (NULL);
    }

    sccpcb->appcbs = (gapi_callbacks_t *)(SCCP_MALLOC(sizeof(*(sccpcb->appcbs))));
    if (sccpcb->appcbs == NULL) {
        sccp_free_sccpcb(sccpcb);
        return (NULL);
    }

    sccpcb->events = sllist_create_list((sllist_free_f *)sccp_free_event2,
                                        SCCP_EVENT_LIST_NAME);

    if (sccpcb->events == NULL) {
        SCCP_DBG((sccp_debug, 5,
                 "%s: %-20s: events list create failed.", 
                 SCCP_ID, fname));
        sccp_free_sccpcb(sccpcb);
        return (NULL);
    }

    sccpcb->cccbs = sllist_create_list((sllist_free_f *)sccpcc_free_cccb2,
                                       SCCP_CCCB_LIST_NAME);

    if (sccpcb->cccbs == NULL) {
        SCCP_DBG((sccp_debug, 5,
                 "%s: %-20s: cccbs list create failed.",
                 SCCP_ID, fname));
        sccp_free_sccpcb(sccpcb);
        return (NULL);
    }
    /*
     * Grab a default cccb to handle any sccp messages
     * that are not for a call or that we can not map to a call.
     * Assign 0 to the id to identify it.
     */
    cccb = sccpcc_get_new_cccb(sccpcb, 0);
    if (cccb == NULL) {
        sccp_free_sccpcb(sccpcb);
        return (NULL);
    }

    sccpcb->cmcbs = sllist_create_list((sllist_free_f *)sccpcm_free_cmcb2,
                                       SCCP_CMCB_LIST_NAME);

    if (sccpcb->cmcbs == NULL) {
        SCCP_DBG((sccp_debug, 5,
                 "%s: %-20s: cmcbs list create failed.",
                 SCCP_ID, fname));
        sccp_free_sccpcb(sccpcb);
        return (NULL);
    }

    sccpcb->reccb = sccprec_get_new_reccb(sccpcb);
    if (sccpcb->reccb == NULL) {
        sccp_free_sccpcb(sccpcb);
        return (NULL);
    }

    sccpcb->regcb = sccpreg_get_new_regcb(sccpcb);
    if (sccpcb->regcb == NULL) {
        sccp_free_sccpcb(sccpcb);
        return (NULL);
    }

    //sccpcb->id   = sccp_get_new_sccpcb_id();
    sccpcb->event_run = SCCP_THREAD_GET();
    if (endian != SCCP_NTOHL(endian)) {
        sccpcb->endian = SCCP_ENDIAN_LITTLE;
    }
    //sccpcb->version = SCCPMSG_VERSION_PARCHE;

    SCCP_DBG((sccp_debug, 3,
             "%s %-5d: %-20s: cb= 0x%08x\n",
             SCCP_ID, sccpcb->id, fname, sccpcb));

    return (sccpcb);
}

int sccp_init (void)
{
    int rc;

    if (sccp_initialized != 0) {
        return (0);;
    }

    if (sccp_platform_init() != 0) {
        return (1);
    }

#ifdef SCCP_PRINT_TABLE
/* debug code to show message sizes. */
    sccp_print_table();
#endif

    if (sllist_init() != 0) {
        return (1);
    }

    sccp_sccpcbs = sllist_create_list((sllist_free_f *)sccp_free_sccpcb2,
                                      SCCP_SCCPCB_LIST_NAME);

    if (sccp_sccpcbs == NULL) {
        return (21);
    }

#ifdef SCCP_USE_POOL
    {
        int i;
        sccp_pool_table_element_t *element;
        unsigned long *p;

        /*
         * Setup the static memory pools.
         */
        for (element = sccp_pool_table;
             element < &(sccp_pool_table[SCCP_POOL_MAX]);
             element++){

            /*
             * Create the pool lists.
             */
            element->list = sllist_create_list((sllist_free_f *)sccp_free_pool2,
                                               element->name);

            if (element->list == NULL) {
                sccp_sccpcbs = sllist_delete_list(sccp_sccpcbs);
                sccp_pool_delete_all();
                return (22);
            }

            /*
             * Link all the nodes in the pool.
             */
            p = element->pool_start;
            for (i = 0; i < element->cnt; i++) {
                if (sllist_add_node(element->list, p, 1) == NULL) {
                    sccp_pool_delete_all();

                    return (22);
                }

                p += (element->size / sizeof(unsigned long));
            }
        } /* for (element = sccp_pool_table; */
    }
#endif /* SCCP_USE_POOL */

    sccp_callbacks = (gapi_callbacks_t *)(SCCP_MALLOC(sizeof(*sccp_callbacks)));
    if (sccp_callbacks == NULL) {
        sccp_sccpcbs = sllist_delete_list(sccp_sccpcbs);

#ifdef SCCP_USE_POOL
        sccp_pool_delete_all();
#endif
        return (23);
    }

    sccp_set_callbacks(sccp_callbacks);

    rc = sccpcc_init();
    if (rc != 0) {
        return (31);
    }

    rc = sccpcm_init();
    if (rc != 0) {
        return (32);
    }

    rc = sccprec_init();
    if (rc != 0) {
        return (33);
    }
    
    rc = sccpreg_init();
    if (rc != 0) {
        return (34);
    }

    sccp_initialized = 1;

    return (0);
}

int sccp_cleanup ()
{
    sccpcc_cleanup();
    sccpcm_cleanup();
    sccprec_cleanup();
    sccpreg_cleanup();
    
    sccp_platform_cleanup();

    sccp_sccpcbs = sllist_delete_list(sccp_sccpcbs);
    sccp_sccpcb_id = 0;

    SCCP_FREE(sccp_callbacks);
    sccp_callbacks = NULL;

    sccp_pool_delete_all();

    sllist_cleanup();

    sccp_initialized = 0;

    return (0);
}

gapi_callbacks_t *sccp_get_callbacks (sccp_sccpcb_t *sccpcb)
{
    return (sccp_callbacks);
}

void sccp_set_version (sccp_sccpcb_t *sccpcb, int version)
{
    if (sccpcb != NULL) {
        sccpcb->version = version;
    }
}
#if 0
sccp_sccpcb_t *sccp_get_sccpcb (void)
{
    return (sccp_sccpcbs->next);
}
#endif

/*
 * Flip this on to buld only PSCCP and not SAPP.
 */
#ifdef SCCPMAIN

#ifdef SCCP_PLATFORM_WINDOWS
int __stdcall WinMain(int a, int b, char* c, int d);
int __stdcall WinMain(int a, int b, char* c, int d)
{
    return 0;
}
#endif /* SCCP_PLATFORM_WINDOWS */

int main (int argc, char **argv)
{
    return (0);
}

#endif /* SCCPMAIN */
