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
 *     gapi.h
 *
 *  CREATION/MODIFICATION DATES
 *     Created:  September 2002, Sam Hague
 *     Modified: 
 *
 *  DESCRIPTION
 *     Generic Call Control API header file
 * 
 *  NOTES:
 *     1. May want to add data to STATUS - i.e. the status for
 *        notifying app when trying to connect to a CM, pass the ip addr.
 */
#ifndef _GAPI_H_
#define _GAPI_H_

#define GAPI_MAX_MEDIA_TYPES             (15)
#define GAPI_NO_LINE                     (0)
#define GAPI_NO_DATA                     (NULL)

#define GAPI_DIRECTORY_NUMBER_SIZE       (24)
#define GAPI_DIRECTORY_NAME_SIZE         (40)
#define GAPI_DEVICE_NAME_SIZE            (16)
#define GAPI_DISPLAY_TEXT_SIZE           (32)
#define GAPI_PROMPT_STATUS_SIZE          (32)
#define GAPI_NOTIFY_SIZE                 (32)
#define GAPI_ALARM_TEXT_SIZE             (80)
#define GAPI_VERSION_SIZE                (16)
#define GAPI_DATE_TEMPLATE_SIZE          (6)
#define GAPI_BUTTON_TEMPLATE_SIZE        (42)
#define GAPI_MAX_SERVERS                 (5)
#define GAPI_MAX_LINES                   (40)
#define GAPI_MAX_SPEED_DIALS             (100)
#define GAPI_MAX_SERVICE_URLS            (40)
#define GAPI_MAX_SERVICE_URL_SIZE        (256)
#define GAPI_MAX_FEATURES                (40)
#define GAPI_SOFTKEY_LABEL_SIZE          (16)
#define GAPI_MAX_SOFTKEY_DEFINITIONS     (32)
#define GAPI_MAX_SOFTKEY_SET_DEFINITIONS (16)
#define GAPI_MAX_SOFTKEY_INDEXES         (16)
#define GAPI_MAX_CAPABILITIES            (18)
#define GAPI_MAX_PORTS                   (10)
#define GAPI_SERVER_NAME_SIZE            (48)
#define GAPI_MAX_DIRNUMS                 (1024)
#define GAPI_USER_DEVICE_DATA_SIZE       (1024)
#define GAPI_MAX_MAC_SIZE                (13)


#ifdef SCCP_PROTOTYPE_CHECKING
typedef struct gapi_handle_ *gapi_handle_t;
#else
typedef void *gapi_handle_t;
#endif

typedef enum gapi_msgs_e_ {
    GAPI_MSG_MIN = -1,
    GAPI_MSG_SETUP,
    GAPI_MSG_SETUP_ACK,
    GAPI_MSG_PROCEEDING,
    GAPI_MSG_ALERTING,
    GAPI_MSG_CONNECT,
    GAPI_MSG_CONNECT_ACK,
    GAPI_MSG_DISCONNECT,
    GAPI_MSG_RELEASE,
    GAPI_MSG_RELEASE_COMPLETE,

    GAPI_MSG_SOFTKEY,
    GAPI_MSG_FEATURE_REQ,
    GAPI_MSG_FEATURE_RES,

    GAPI_MSG_OPENRCV_REQ,
    GAPI_MSG_OPENRCV_RES,
    GAPI_MSG_CLOSERCV,
    GAPI_MSG_STARTXMIT,
    GAPI_MSG_STOPXMIT,

    GAPI_MSG_OFFHOOK,
    GAPI_MSG_ONHOOK,
    GAPI_MSG_HOOKFLASH,
    GAPI_MSG_DIGITS,
    GAPI_MSG_STARTTONE,
    GAPI_MSG_STOPTONE,
    GAPI_MSG_RINGER,
    GAPI_MSG_LAMP,
    GAPI_MSG_SPEAKER,
    GAPI_MSG_MICRO,
    GAPI_MSG_HEADSET,
    GAPI_MSG_TIMEDATE,
    GAPI_MSG_CONNINFO,
    GAPI_MSG_DISPLAY,
    GAPI_MSG_CLEARDISPLAY,
    GAPI_MSG_PROMPT,
    GAPI_MSG_CLEARPROMPT,
    GAPI_MSG_NOTIFY,
    GAPI_MSG_CLEARNOTIFY,
    GAPI_MSG_ACTIVATEPLANE,
    GAPI_MSG_DEACTIVATEPLANE,
    GAPI_MSG_CONNECTIONSTATS,
    GAPI_MSG_BACKSPACE_REQ,
    GAPI_MSG_SELECTSOFTKEYS,
    GAPI_MSG_DIALEDNUMBER,
    GAPI_MSG_DEVICETOUSERDATA_REQ,
    GAPI_MSG_DEVICETOUSERDATA_RES,
    GAPI_MSG_USERTODEVICEDATA,
    GAPI_MSG_PRIORITYNOTIFY,
    GAPI_MSG_CLEARPRIORITYNOTIFY,

    GAPI_MSG_RESET,
    GAPI_MSG_OPENSESSION_REQ,
    GAPI_MSG_OPENSESSION_RES,
    GAPI_MSG_RESETSESSION_REQ,
    GAPI_MSG_RESETSESSION_RES,

    /* NOT IMPLEMENTED */
    GAPI_MSG_KEEPALIVE_REQ,
    GAPI_MSG_KEEPALIVE_RES,
    GAPI_MSG_REGISTER_REQ,
    GAPI_MSG_REGISTER_RES,
    GAPI_MSG_ALARM,
    GAPI_MSG_FORWARDSTAT_REQ,
    GAPI_MSG_FORWARDSTAT_RES,
    GAPI_MSG_SERVICEURLSTAT_REQ,
    GAPI_MSG_SERVICEURLSTAT_RES,
    GAPI_MSG_FEATURESTAT_REQ,
    GAPI_MSG_FEATURESTAT_RES,
    GAPI_MSG_SPEEDDIALSTAT_REQ,
    GAPI_MSG_SPEEDDIALSTAT_RES,
    GAPI_MSG_LINESTAT_REQ,
    GAPI_MSG_LINESTAT_RES,
    GAPI_MSG_CONFIGSTAT_REQ,
    GAPI_MSG_CONFIGSTAT_RES,
    GAPI_MSG_TIMEDATE_REQ,
    GAPI_MSG_TIMEDATE_RES,
    GAPI_MSG_BUTTONTEMPLATE_REQ,
    GAPI_MSG_BUTTONTEMPLATE_RES,
    GAPI_MSG_VERSION_REQ,
    GAPI_MSG_VERSION_RES,
    GAPI_MSG_CAPABILITIES_REQ,
    GAPI_MSG_CAPABILITIES_RES,
    GAPI_MSG_SOFTKEYTEMPLATE_REQ,
    GAPI_MSG_SOFTKEYTEMPLATE_RES,
    GAPI_MSG_SOFTKEYSET_REQ,
    GAPI_MSG_SOFTKEYSET_RES,
    GAPI_MSG_AVAILABLELINES,

    GAPI_MSG_PASSTHRU,

    GAPI_MSG_STATUS,
    
    GAPI_MSG_TCP_EVENTS,
    
    GAPI_MSG_MAX
} gapi_msgs_e;

/*
    gapi_causes_e: possible return codes for GAPI requests.

    Only currently used causes are explained below:

    GAPI_CAUSE_OK: good response to GAPI requests.
    
    GAPI_CAUSE_ERROR: bad response to GAPI requests.
    
    GAPI_CAUSE_CM_CONNECT_FAILURE: not connected to CCM and SAPP tries
                                   to make a call. SAPP should OPENSESSION 
                                   or RESETSESSION to get things right
                                   before attempting another call.

    GAPI_CAUSE_CALL_ID_IN_USE: SAPP tried to initiate a call using an
                               already inuse conn_id. SAPP should do any
                               cleanup to get the call list correct.

    GAPI_CAUSE_NO_RESOURCE: PSCCP problem, most likely malloc failure.
                            SAPP should try agaain. If failures continue
                            then fix problem and RESETSESSION.

    GAPI_CAUSE_NO_CM_FOUND: PSCCP has exhausted all retries to connect
                            and register to the list of CCMs provided in
                            the OPENSESSION_REQ. SAPP should restart
                            and try again later.

    GAPI_CAUSE_CMS_UNDEFINED: SAPP attempts to OPENSESSION_REQ with a
                              cms list that does not have any CCMs defined.

    GAPI_CAUSE_CM_RESET: CCM requested RESET. SAPP should prepare for a
                         reset and then request RESET from PSCCP.

    GAPI_CAUSE_CM_RESTART: CCM requested RESTART. SAPP should prepare for a
                           reset and then request RESTART from PSCCP.


    GAPI_CAUSE_CM_REGISTER_REJECT: CCM has rejected the registration. SAPP
                                   can try again or restart.

    GAPI_CAUSE_SESSION_INVALID_DATA: SAPP attempts to OPENSESSION_REQ with
                                     invalid data, i.e. the mac or media
                                     data is invalid.

    GAPI_CAUSE_SESSION_ALREADY_OPENED: SAPP attemps OPENSESSION_REQ when
                                       PSCCP is already opened.
*/
typedef enum gapi_causes_e_ {
    GAPI_CAUSE_MIN = -1,
    GAPI_CAUSE_OK,
    GAPI_CAUSE_ERROR,
    GAPI_CAUSE_CM_CONNECT_FAILURE,
    GAPI_CAUSE_UNASSIGNED_NUM,
    GAPI_CAUSE_NO_RESOURCE,
    GAPI_CAUSE_NO_ROUTE,
    GAPI_CAUSE_NORMAL,
    GAPI_CAUSE_BUSY,
    GAPI_CAUSE_NO_USER_RESP,
    GAPI_CAUSE_NO_USER_ANS,
    GAPI_CAUSE_REJECT,
    GAPI_CAUSE_INVALID_NUMBER,
    GAPI_CAUSE_FACILITY_REJECTED,
    GAPI_CAUSE_CALL_ID_IN_USE,
    GAPI_CAUSE_XFER,
    GAPI_CAUSE_CONGESTION,
    GAPI_CAUSE_DND,
    GAPI_CAUSE_ANONYMOUS,
    GAPI_CAUSE_REDIRECT,
    GAPI_CAUSE_PAYLOAD_MISMATCH,
    GAPI_CAUSE_CONF,
    GAPI_CAUSE_REPLACE,
    GAPI_CAUSE_NO_REPLACE_CALL,
    
    GAPI_CAUSE_NO_CM_FOUND,
    GAPI_CAUSE_CMS_UNDEFINED,
    GAPI_CAUSE_TFTP_DEFAULT,

    GAPI_CAUSE_CM_CLOSE,
    GAPI_CAUSE_CM_RESET,
    GAPI_CAUSE_CM_RESTART,
    GAPI_CAUSE_CM_REGISTER_REJECT,
    GAPI_CAUSE_CMS_LOCKOUT,

    GAPI_CAUSE_SESSION_ALREADY_OPENED,
    GAPI_CAUSE_SESSION_INVALID_DATA,
    GAPI_CAUSE_MAX
} gapi_causes_e;

typedef enum gapi_status_e_ {
    GAPI_STATUS_MIN = -1,
    GAPI_STATUS_RESET_COMPLETE,
    GAPI_STATUS_CM_DOWN,
    GAPI_STATUS_CM_OPENING,
    GAPI_STATUS_CM_CONNECTED,
    GAPI_STATUS_CM_REGISTERED,
    GAPI_STATUS_CM_REGISTER_COMPLETE,
    GAPI_STATUS_MAX
} gapi_status_e;

typedef enum gapi_devices_t_ {
    GAPI_DEVICE_STATION_TELECASTER_MGR = 7,
    GAPI_DEVICE_IPSTE                  = 30035,
    GAPI_DEVICE_MAX
} gapi_devices_t;

typedef enum gapi_features_e_ {
    GAPI_FEATURE_MIN = -1,
    GAPI_FEATURE_NONE,
    GAPI_FEATURE_REDIAL,
    GAPI_FEATURE_NEWCALL,
    GAPI_FEATURE_HOLD,
    GAPI_FEATURE_TRANSFER,
    GAPI_FEATURE_CFWDALL,
    GAPI_FEATURE_CFWDBUSY,
    GAPI_FEATURE_CFWDNOANS,
    GAPI_FEATURE_BACKSPACE,
    GAPI_FEATURE_ENDCALL,
    GAPI_FEATURE_RESUME,
    GAPI_FEATURE_ANSWER,
    GAPI_FEATURE_INFO,
    GAPI_FEATURE_CONFERENCE,
    GAPI_FEATURE_PARK,
    GAPI_FEATURE_JOIN,
    GAPI_FEATURE_MEETMECNF,
    GAPI_FEATURE_CALLPU,
    GAPI_FEATURE_GRPCALLPU,
    GAPI_FEATURE_DROP,
    GAPI_FEATURE_CALLBACK,
    GAPI_FEATURE_BARGE,
    GAPI_FEATURE_SPEEDDIAL,
    GAPI_FEATURE_MAX
} gapi_features_e;

typedef union gapi_feature_data_u_ {
    int one;
    int two;
} gapi_feature_data_u;

typedef enum gapi_alert_info_e_ {
    GAPI_ALERT_INFO_MIN = -1,
    GAPI_ALERT_INFO_OFF,
    GAPI_ALERT_INFO_ON,
    GAPI_ALERT_INFO_MAX
} gapi_alert_info_e;

typedef enum gapi_privacy_e_ {
    GAPI_PRIVACY_MIN = -1,
    GAPI_PRIVACY_OFF,
    GAPI_PRIVACY_ON,
    GAPI_PRIVACY_MAX
} gapi_privacy_e;

typedef struct gapi_precedence_t_ {
    unsigned long level;
    unsigned long domain;
} gapi_precedence_t;

typedef enum gapi_inband_e_ {
    GAPI_INBAND_MIN = -1,
    GAPI_INBAND_OFF,
    GAPI_INBAND_ON,
    GAPI_INBAND_MAX
} gapi_inband_e;

typedef enum gapi_tone_directions_e_ {
    GAPI_TONE_DIRECTION_MIN     = -1,
    GAPI_TONE_DIRECTION_USER    = 0,
    GAPI_TONE_DIRECTION_NETWORK = 1,
    GAPI_TONE_DIRECTION_ALL     = 2,
    GAPI_TONE_DIRECTION_MAX,
} gapi_tone_directions_e;

typedef enum gapi_tones_e_ {
    GAPI_TONE_MIN = -1,
    GAPI_TONE_SILENCE     = 0x0,
    GAPI_TONE_DTMF1       = 0x1,
    GAPI_TONE_DTMF2       = 0x2,
    GAPI_TONE_DTMF3       = 0x3,
    GAPI_TONE_DTMF4       = 0x4,
    GAPI_TONE_DTMF5       = 0x5,
    GAPI_TONE_DTMF6       = 0x6,
    GAPI_TONE_DTMF7       = 0x7,
    GAPI_TONE_DTMF8       = 0x8,
    GAPI_TONE_DTMF9       = 0x9,
    GAPI_TONE_DTMF0       = 0xa,
    GAPI_TONE_DTMFSTAR    = 0xe,
    GAPI_TONE_DTMFPOUND   = 0xf,
    GAPI_TONE_DTMFA       = 0x10,
    GAPI_TONE_DTMFB       = 0x11,
    GAPI_TONE_DTMFC       = 0x12,
    GAPI_TONE_DTMFD       = 0x13,
    GAPI_TONE_INSIDEDIAL  = 0x21,
    GAPI_TONE_OUTSIDEDIAL = 0x22,
    GAPI_TONE_BUSY        = 0x23,
    GAPI_TONE_ALERTING    = 0x24,
    GAPI_TONE_REORDER     = 0x25,
    GAPI_TONE_CALLWAITING = 0x2d,
    GAPI_TONE_BEEPBONK    = 0x33,
    GAPI_TONE_HOLD        = 0x35,
    GAPI_TONE_MAX
} gapi_tones_e;

typedef enum gapi_ringers_e_ {
    GAPI_RINGER_MIN        = -1,
    GAPI_RINGER_OFF        = 0x1,
    GAPI_RINGER_INSIDE     = 0x2,
    GAPI_RINGER_OUTSIDE    = 0x3,
    GAPI_RINGER_FEATURE    = 0x4,
    GAPI_RINGER_FLASH_ONLY = 0x5,
    GAPI_RINGER_PRECEDENCE = 0x6,
    GAPI_RINGER_MAX
} gapi_ringers_e;

typedef enum gapi_duration_e_ {
    GAPI_DURATION_MIN = -1,
    GAPI_DURATION_NORMAL,
    GAPI_DURATION_SINGLE,
    GAPI_DURATION_MAX
} gapi_duration_e;

typedef enum gapi_speakers_e_ {
    GAPI_SPEAKER_MIN = -1,
    GAPI_SPEAKER_ON  = 0x1,
    GAPI_SPEAKER_OFF = 0x2,
    GAPI_SPEAKER_MAX
} gapi_speakers_e;

typedef enum gapi_mircophones_e_ {
    GAPI_MIRCOPHONES_MIN = -1,
    GAPI_MIRCOPHONES_ON  = 0x1,
    GAPI_MIRCOPHONES_OFF = 0x2,
    GAPI_MIRCOPHONES_MAX
} gapi_microphones_e;

typedef enum gapi_headsets_e_ {
    GAPI_HEADSET_MIN = -1,
    GAPI_HEADSET_ON  = 0x1,
    GAPI_HEADSET_OFF = 0x2,
    GAPI_HEADSET_MAX
} gapi_headsets_e;

typedef enum gapi_lamps_e_ {
    GAPI_LAMP_MIN  = -1,
    GAPI_LAMP_OFF   = 0x1,
    GAPI_LAMP_ON    = 0x2,
    GAPI_LAMP_WINK  = 0x3,
    GAPI_LAMP_FLASH = 0x4,
    GAPI_LAMP_BLINK = 0x5,
    GAPI_LAMP_MAX
} gapi_lamps_e;

typedef enum gapi_payload_types_e_ {
    GAPI_PAYLOAD_TYPE_NON_STANDARD           = 1,
    GAPI_PAYLOAD_TYPE_G711_ALAW_64K          = 2,
    GAPI_PAYLOAD_TYPE_G711_ALAW_56K          = 3,      // "RESTRICTED"
    GAPI_PAYLOAD_TYPE_G711_ULAW_64K          = 4,
    GAPI_PAYLOAD_TYPE_G711_ULAW_56K          = 5,      // "RESTRICTED"
    GAPI_PAYLOAD_TYPE_G722_64K               = 6,
    GAPI_PAYLOAD_TYPE_G722_56K               = 7,
    GAPI_PAYLOAD_TYPE_G722_48K               = 8,
    GAPI_PAYLOAD_TYPE_G7231                  = 9,
    GAPI_PAYLOAD_TYPE_G728                   = 10,
    GAPI_PAYLOAD_TYPE_G729                   = 11,
    GAPI_PAYLOAD_TYPE_G729_ANNEX_A           = 12,
    GAPI_PAYLOAD_TYPE_IS11172_AUDIO_CAP      = 13,
    GAPI_PAYLOAD_TYPE_IS13818_AUDIO_CAP      = 14,
    GAPI_PAYLOAD_TYPE_G729_ANNEX_B           = 15,
    GAPI_PAYLOAD_TYPE_G729_ANNEX_AW_ANNEX_B  = 16,
    GAPI_PAYLOAD_TYPE_GSM_FULL_RATE          = 18,
    GAPI_PAYLOAD_TYPE_GSM_HALF_RATE          = 19,
    GAPI_PAYLOAD_TYPE_GSM_ENHANCED_FULL_RATE = 20,
    GAPI_PAYLOAD_TYPE_WIDE_BAND_256K         = 25,
    GAPI_PAYLOAD_TYPE_DATA_64                = 32,
    GAPI_PAYLOAD_TYPE_DATA_56                = 33,
    GAPI_PAYLOAD_TYPE_GSM                    = 80,
    GAPI_PAYLOAD_TYPE_ACTIVE_VOICE           = 81,
    GAPI_PAYLOAD_TYPE_G726_32K               = 82,
    GAPI_PAYLOAD_TYPE_G726_24K               = 83,
    GAPI_PAYLOAD_TYPE_G726_16K               = 84,
    GAPI_PAYLOAD_TYPE_G729_B                 = 85,
    GAPI_PAYLOAD_TYPE_G729_B_LOW_COMPLEXITY  = 86,
    GAPI_PAYLOAD_H261                        = 100,
    GAPI_PAYLOAD_H263                        = 101,
    GAPI_PAYLOAD_T120                        = 105,
    GAPI_PAYLOAD_H224                        = 106,
    GAPI_PAYLOAD_XV150_MR                    = 111,
    GAPI_PAYLOAD_TYPE_INVALID,
    GAPI_PAYLOAD_RFC2833_DYN_PAYLOAD         = 257,
} gapi_payload_types_e;

#if 0 /* NOT USED YET */
typedef struct gapi_sdp_data_t_ {
    unsigned int         addr;
    unsigned int         port;
    gapi_payload_types_e media_types[GAPI_MAX_MEDIA_TYPES];
    int                  avt_payload_type;
} gapi_sdp_data_t;

typedef struct gapi_sdp_t_ {
    gapi_sdp_data_t local;
    gapi_sdp_data_t remote;
    gapi_sdp_data_t negotiated;
} gapi_sdp_t;
#endif
/*
 * port is defined as unsigned long to be consistent with SCCP.
 */
typedef struct gapi_sccp_media_t_ {
    unsigned long addr;
    unsigned long port;
    unsigned int conference_id;
    unsigned int passthruparty_id;
    unsigned int packet_size;
    gapi_payload_types_e payload_type;
    int echo_cancelation;
    int g723_bitrate; 
    int precedence;
    int silence_suppression;
    int frames_per_packet;
} gapi_sccp_media_t;

typedef struct gapi_media_t_ {
#if 0 /* NOT USED YET */
    gapi_sdp_t        sdp;
#endif
    gapi_sccp_media_t sccp_media;
} gapi_media_t;

typedef struct gapi_media_cap_t_ {
    gapi_payload_types_e payload;
    int milliseconds_per_packet;
} gapi_media_cap_t;

typedef struct gapi_media_caps_t_ {
    int count;
    gapi_media_cap_t caps[GAPI_MAX_CAPABILITIES];
} gapi_media_caps_t;

typedef struct gapi_conninfo_t_ {
    char *calling_name;
    char *calling_number;
    char *called_name;
    char *called_number;
    char *original_name;
    char *original_number;
    char *redirected_name;
    char *redirected_number;
} gapi_conninfo_t;

typedef struct gapi_cmaddr_t_ {
    unsigned long addr;
    unsigned short port;
} gapi_cmaddr_t;

typedef enum gapi_srst_modes_e_ {
    GAPI_SRST_MODE_MIN = -1,
    GAPI_SRST_MODE_DISABLE,
    GAPI_SRST_MODE_ENABLE,
    GAPI_SRST_MODE_MAX
} gapi_srst_modes_e;

typedef enum gapi_tcp_events_e_ {
    GAPI_TCP_EVENT_MIN = -1,
    GAPI_TCP_EVENT_ACK,
    GAPI_TCP_EVENT_NACK,    
    GAPI_TCP_EVENT_OPEN,
    GAPI_TCP_EVENT_CLOSE,
    GAPI_TCP_EVENT_RECV,
    GAPI_TCP_EVENT_MAX        
} gapi_tcp_events_e;

typedef enum gapi_display_options_e_ {
    GAPI_DISPLAY_OPTIONS_MIN  = -1,
    GAPI_DISPLAY_OPTIONS_ODN  = 0x1,
    GAPI_DISPLAY_OPTIONS_RDN  = 0x2,
    GAPI_DISPLAY_OPTIONS_CLID = 0x4,
    GAPI_DISPLAY_OPTIONS_CNID = 0x8,
    GAPI_DISPLAY_OPTIONS_MAX
} gapi_display_options_e;

typedef struct gapi_line_t_ {
    int  instance;
    char dn[GAPI_DIRECTORY_NUMBER_SIZE];
    char fqdn[GAPI_DIRECTORY_NAME_SIZE];
    char label[GAPI_DIRECTORY_NAME_SIZE];
    unsigned long flags;
} gapi_line_t;

typedef struct gapi_speeddial_t_ {
    int  instance;
    char dn[GAPI_DIRECTORY_NUMBER_SIZE];
    char fqdn[GAPI_DIRECTORY_NAME_SIZE];
} gapi_speeddial_t;

typedef struct gapi_feature_t_ {
    int  instance;
    unsigned long id;
    char label[GAPI_DIRECTORY_NAME_SIZE];
    unsigned long status;
} gapi_feature_t;

typedef struct gapi_serviceurl_t_ {
    int  instance;
    char url[GAPI_MAX_SERVICE_URL_SIZE];
    char display_name[GAPI_DIRECTORY_NAME_SIZE];
} gapi_serviceurl_t;

typedef struct gapi_softkey_definition_t_ {
    char          label[GAPI_SOFTKEY_LABEL_SIZE];
    unsigned long event;
} gapi_softkey_definition_t;

typedef struct gapi_softkey_template_t_ {
    unsigned long             offset;
    unsigned long             count;
    unsigned long             total_count;
    gapi_softkey_definition_t definition[GAPI_MAX_SOFTKEY_DEFINITIONS];
}gapi_softkey_template_t;

typedef struct gapi_softkey_set_definition_t_ {
    unsigned char  template_index[GAPI_MAX_SOFTKEY_INDEXES];
    unsigned short info_index[GAPI_MAX_SOFTKEY_INDEXES];
} gapi_softkey_set_definition_t;

typedef struct gapi_softkey_set_t_ {
    unsigned long       offset;
    unsigned long       count;
    unsigned long       total_count;
    gapi_softkey_set_definition_t 
                        definition[GAPI_MAX_SOFTKEY_SET_DEFINITIONS];
} gapi_softkey_set_t;

typedef enum gapi_versions_e_ {
    GAPI_VERSION_MIN = 0,
    GAPI_VERSION_SP30,
    GAPI_VERSION_BRAVO,
    GAPI_VERSION_HAWKBILL,
    GAPI_VERSION_SEAVIEW,
    GAPI_VERSION_PARCHE,
    GAPI_VERSION_MAX,
} gapi_versions_e;

typedef struct gapi_status_data_info_t_ {
    int linecnt;
    int speeddialcnt;
    int featurecnt;
    int serviceurlcnt;
    int softkeycnt;
    int softkeysetcnt;
    gapi_versions_e version;
} gapi_status_data_info_t;

typedef struct gapi_status_data_misc_t_ {
    gapi_cmaddr_t cmaddr;
} gapi_status_data_misc_t;

typedef enum gapi_status_data_types_e_ {
    GAPI_STATUS_DATA_MIN = -1,
    GAPI_STATUS_DATA_INFO,
    GAPI_STATUS_DATA_MISC,
    GAPI_STATUS_DATA_MAX
} gapi_status_data_types_e;

typedef union gapi_status_data_u_ {
    gapi_status_data_info_t info;
    gapi_status_data_misc_t misc;
} gapi_status_data_u;

typedef enum gapi_cc_mode_e_ {
    GAPI_CC_MODE_MIN = -1,
    GAPI_CC_MODE_NORMAL,
    GAPI_CC_MODE_PASSTHRU,
    GAPI_CC_MODE_MAX
} gapi_cc_mode_e;

typedef struct gapi_opensession_values_t_ {
    int call_end_to;
    int close_to;
    int connect_to;
    int default_keepalive_to;
    int device_poll_to;
    int nak_to_syn_retry_to;
    gapi_devices_t device_type;
    gapi_cc_mode_e cc_mode;
} gapi_opensession_values_t;

typedef enum gapi_protocol_versions_e_ {
    GAPI_PROTOCOL_VERSION_MIN = -1,
    GAPI_PROTOCOL_VERSION_RESERVED,
    GAPI_PROTOCOL_VERSION_SP30,
    GAPI_PROTOCOL_VERSION_BRAVO,
    GAPI_PROTOCOL_VERSION_HAWKBILL,
    GAPI_PROTOCOL_VERSION_SEAVIEW,
    GAPI_PROTOCOL_VERSION_PARCHE,
    GAPI_PROTOCOL_VERSION_MAX
} gapi_protocol_versions_e;

typedef enum gapi_stats_processing_e_ {
    GAPI_STATS_PROCESSING_CLEAR    = 0,
    GAPI_STATS_PROCESSING_NO_CLEAR = 1,
    GAPI_STATS_PROCESSING_INVALID
} gapi_stats_processing_e;

typedef struct gapi_connection_stats_t_ {
    char                    directory_number[GAPI_DIRECTORY_NUMBER_SIZE];
    unsigned long           call_identifier;
    gapi_stats_processing_e processing_mode;
    unsigned long           number_packets_sent;
    unsigned long           number_bytes_sent;
    unsigned long           number_packets_received;
    unsigned long           number_bytes_received;
    unsigned long           number_packets_lost;
    unsigned long           jitter;
    unsigned long           latency;
} gapi_connection_stats_t;

typedef struct gapi_user_and_device_data_t_ {
    unsigned long application_id;
    unsigned long line_number;
    unsigned long call_reference;
    unsigned long transaction_id;
    unsigned long data_length;
    char          data[GAPI_USER_DEVICE_DATA_SIZE];
} gapi_user_and_device_data_t;

typedef int (gapi_setup_f)(gapi_handle_t handle, gapi_msgs_e msg_id,
                           int conn_id, int line,
                           gapi_conninfo_t *conninfo,
                           char *digits, int numdigits,
                           gapi_media_t *media, gapi_alert_info_e alert_info,
                           gapi_privacy_e privacy,
                           gapi_precedence_t *precedence);

typedef int (gapi_setup_ack_f)(gapi_handle_t handle, gapi_msgs_e msg_id,
                               int conn_id, int line,
                               gapi_conninfo_t *conninfo, gapi_media_t *media,
                               gapi_causes_e cause,
                               gapi_precedence_t *precedence);

typedef int (gapi_proceeding_f)(gapi_handle_t handle, gapi_msgs_e msg_id,
                                int conn_id, int line,
                                gapi_conninfo_t *conninfo,
                                gapi_precedence_t *precedence);

typedef int (gapi_alerting_f)(gapi_handle_t handle, gapi_msgs_e msg_id,
                              int conn_id, int line,
                              gapi_conninfo_t *conninfo, gapi_media_t *media,
                              gapi_inband_e inband,
                              gapi_precedence_t *precedence);

typedef int (gapi_connect_f)(gapi_handle_t handle, gapi_msgs_e msg_id,
                             int conn_id, int line,
                             gapi_conninfo_t *conninfo, gapi_media_t *media,
                             gapi_precedence_t *precedence);

typedef int (gapi_connect_ack_f)(gapi_handle_t handle, gapi_msgs_e msg_id,
                                 int conn_id, int line,
                                 gapi_conninfo_t *conninfo,
                                 gapi_media_t *media,
                                 gapi_precedence_t *precedence);

typedef int (gapi_disconnect_f)(gapi_handle_t handle, gapi_msgs_e msg_id,
                                int conn_id, int line,
                                gapi_causes_e cause,
                                gapi_precedence_t *precedence);

typedef int (gapi_release_f)(gapi_handle_t handle, gapi_msgs_e msg_id,
                             int conn_id, int line,
                             gapi_causes_e cause,
                             gapi_precedence_t *precedence);

typedef int (gapi_release_complete_f)(gapi_handle_t handle,
                                      gapi_msgs_e msg_id,
                                      int conn_id, int line,
                                      gapi_causes_e cause,
                                      gapi_precedence_t *precedence);
    

typedef int (gapi_softkey_f)(gapi_handle_t handle, gapi_msgs_e msg_id,
                             int conn_id, int line, int softkey);

typedef int (gapi_feature_req_f)(gapi_handle_t handle, gapi_msgs_e msg_id,
                                 int conn_id, int line,
                                 gapi_features_e feature, 
                                 gapi_feature_data_u *data,
                                 gapi_causes_e cause);

typedef int (gapi_feature_res_f)(gapi_handle_t handle, gapi_msgs_e msg_id,
                                 int conn_id, int line,
                                 gapi_features_e feature,
                                 gapi_feature_data_u *data,
                                 gapi_causes_e cause);
    
typedef int (gapi_openrcv_req_f)(gapi_handle_t handle, gapi_msgs_e msg_id,
                                 int conn_id, int line,
                                 gapi_media_t *media);

typedef int (gapi_openrcv_res_f)(gapi_handle_t handle, gapi_msgs_e msg_id,
                                 int conn_id, int line,
                                 gapi_media_t *media, gapi_causes_e cause);

typedef int (gapi_closercv_f)(gapi_handle_t handle, gapi_msgs_e msg_id,
                              int conn_id, int line,
                              gapi_media_t *media);

typedef int (gapi_startxmit_f)(gapi_handle_t handle, gapi_msgs_e msg_id,
                               int conn_id, int line,
                               gapi_media_t *media);

typedef int (gapi_stopxmit_f)(gapi_handle_t handle, gapi_msgs_e msg_id,
                              int conn_id, int line,
                              gapi_media_t *media);

typedef int (gapi_offhook_f)(gapi_handle_t handle, gapi_msgs_e msg_id,
                             int conn_id, int line);

typedef int (gapi_onhook_f)(gapi_handle_t handle, gapi_msgs_e msg_id,
                            int conn_id, int line);

typedef int (gapi_hookflash_f)(gapi_handle_t handle, gapi_msgs_e msg_id,
                               int conn_id, int line);

typedef int (gapi_digits_f)(gapi_handle_t handle, gapi_msgs_e msg_id,
                            int conn_id, int line,
                            char *digits, int numdigits);

typedef int (gapi_starttone_f)(gapi_handle_t handle, gapi_msgs_e msg_id,
                               int conn_id, int line,
                               gapi_tones_e tone,
                               gapi_tone_directions_e direction);

typedef int (gapi_stoptone_f)(gapi_handle_t handle, gapi_msgs_e msg_id,
                              int conn_id, int line);

typedef int (gapi_ringer_f)(gapi_handle_t handle, gapi_msgs_e msg_id, 
                            int conn_id, int line,
                            gapi_ringers_e mode, gapi_duration_e duration);

typedef int (gapi_lamp_f)(gapi_handle_t handle, gapi_msgs_e msg_id, 
                          int conn_id, int line, int stimulus,
                          gapi_lamps_e mode);

typedef int (gapi_speaker_f)(gapi_handle_t handle, gapi_msgs_e msg_id,
                             int conn_id, int line,
                             gapi_speakers_e mode);

typedef int (gapi_micro_f)(gapi_handle_t handle, gapi_msgs_e msg_id,
                           int conn_id, int line,
                           gapi_microphones_e mode);

typedef int (gapi_headset_f)(gapi_handle_t handle, gapi_msgs_e msg_id,
                             int conn_id, int line,
                             gapi_headsets_e mode);

typedef int (gapi_timedate_f)(gapi_handle_t handle, gapi_msgs_e msg_id, 
                              int conn_id, int line,
                              int time);

typedef int (gapi_conninfo_f)(gapi_handle_t handle, gapi_msgs_e msg_id,
                              int conn_id, int line,
                              gapi_conninfo_t *conninfo);

typedef int (gapi_display_f)(gapi_handle_t handle, gapi_msgs_e msg_id, 
                             int conn_id, int line,
                             char *text);

typedef int (gapi_cleardisplay_f)(gapi_handle_t handle, gapi_msgs_e msg_id, 
                                  int conn_id, int line);

typedef int (gapi_prompt_f)(gapi_handle_t handle, gapi_msgs_e msg_id, 
                            int conn_id, int line,
                            char *text, unsigned int timeout);

typedef int (gapi_clearprompt_f)(gapi_handle_t handle, gapi_msgs_e msg_id,
                                 int conn_id, int line);

typedef int (gapi_notify_f)(gapi_handle_t handle, gapi_msgs_e msg_id,
                            int conn_id, int line,
                            char *text, unsigned int timeout);

typedef int (gapi_clearnotify_f)(gapi_handle_t handle, gapi_msgs_e msg_id, 
                                 int conn_id, int line);

typedef int (gapi_activateplane_f)(gapi_handle_t handle, gapi_msgs_e msg_id,
                                   int conn_id, int line);

typedef int (gapi_deactivateplane_f)(gapi_handle_t handle, gapi_msgs_e msg_id, 
                                     int conn_id, int line);

typedef int (gapi_connectionstats_f)(gapi_handle_t handle, gapi_msgs_e msg_id, 
                                     int conn_id, int line,
                                     gapi_connection_stats_t *stats);

typedef int (gapi_backspace_req_f)(gapi_handle_t handle, gapi_msgs_e msg_id, 
                                   int conn_id, int line);

typedef int (gapi_selectsoftkeys_f)(gapi_handle_t handle, gapi_msgs_e msg_id, 
                                    int conn_id, int line,
                                    unsigned long set, unsigned long mask);

typedef int (gapi_dialednumber_f)(gapi_handle_t handle, gapi_msgs_e msg_id, 
                                  int conn_id, int line,
                                  char *number);

typedef int (gapi_prioritynotify_f)(gapi_handle_t handle, gapi_msgs_e msg_id,
                                    int conn_id, int line, 
                                    char *text, unsigned int timeout,
                                    unsigned int priority);

typedef int (gapi_clearprioritynotify_f)(gapi_handle_t handle,
                                         gapi_msgs_e msg_id, 
                                         int conn_id, int line, 
                                         unsigned int priority);

typedef int (gapi_usertodevicedata_f)(gapi_handle_t handle,
                                      gapi_msgs_e msg_id, 
                                      int conn_id, int line,
                                      gapi_user_and_device_data_t *data);

typedef int (gapi_devicetouserdata_req_f)(gapi_handle_t handle,
                                          gapi_msgs_e msg_id, 
                                          int conn_id, int line,
                                          gapi_user_and_device_data_t *data);

typedef int (gapi_devicetouserdata_res_f)(gapi_handle_t handle,
                                          gapi_msgs_e msg_id, 
                                          int conn_id, int line,
                                          gapi_user_and_device_data_t *data);

typedef int (gapi_reset_f)(gapi_handle_t handle, gapi_msgs_e msg_id,
                           gapi_causes_e cause);    

typedef int (gapi_opensession_req_f)(gapi_handle_t handle, gapi_msgs_e msg_id,
                                     gapi_cmaddr_t *cmaddrs, char *mac,
                                     gapi_cmaddr_t *srsts,
                                     gapi_srst_modes_e srst_mode,
                                     gapi_cmaddr_t *tftp,
                                     gapi_media_caps_t *media_caps,
                                     gapi_opensession_values_t *values,
                                     gapi_protocol_versions_e version);

typedef int (gapi_opensession_res_f)(gapi_handle_t handle, gapi_msgs_e msg_id,
                                     gapi_causes_e cause);

typedef int (gapi_sessionstatus_f)(gapi_handle_t handle, gapi_msgs_e msg_id,
                                   gapi_status_e status,
                                   gapi_status_data_types_e type,
                                   gapi_status_data_u *data);

typedef int (gapi_resetsession_req_f)(gapi_handle_t handle,
                                      gapi_msgs_e msg_id,
                                      gapi_causes_e cause);    

typedef int (gapi_resetsession_res_f)(gapi_handle_t handle,
                                      gapi_msgs_e msg_id, 
                                      gapi_causes_e cause); 


typedef int (gapi_keepalive_req_f)(gapi_handle_t handle, gapi_msgs_e msg_id);

typedef int (gapi_keepalive_res_f)(gapi_handle_t handle, gapi_msgs_e msg_id);

typedef int (gapi_register_req_f)(gapi_handle_t handle, gapi_msgs_e msg_id);

typedef int (gapi_register_res_f)(gapi_handle_t handle, gapi_msgs_e msg_id,
                                  int status);

typedef int (gapi_alarm_f)(gapi_handle_t handle, gapi_msgs_e msg_id);

typedef int (gapi_forwardstat_req_f)(gapi_handle_t handle,
                                     gapi_msgs_e msg_id);
typedef int (gapi_forwardstat_res_f)(gapi_handle_t handle,
                                     gapi_msgs_e msg_id);

typedef int (gapi_serviceurlstat_req_f)(gapi_handle_t handle,
                                        gapi_msgs_e msg_id);

typedef int (gapi_serviceurlstat_res_f)(gapi_handle_t handle,
                                        gapi_msgs_e msg_id,
                                        gapi_serviceurl_t *serviceurl);

typedef int (gapi_featurestat_req_f)(gapi_handle_t handle,
                                     gapi_msgs_e msg_id);

typedef int (gapi_featurestat_res_f)(gapi_handle_t handle,
                                     gapi_msgs_e msg_id,
                                     gapi_feature_t *feature);

typedef int (gapi_speeddialstat_req_f)(gapi_handle_t handle,
                                       gapi_msgs_e msg_id);

typedef int (gapi_speeddialstat_res_f)(gapi_handle_t handle,
                                       gapi_msgs_e msg_id,
                                       gapi_speeddial_t *speeddial);

typedef int (gapi_linestat_req_f)(gapi_handle_t handle,
                                  gapi_msgs_e msg_id);

typedef int (gapi_linestat_res_f)(gapi_handle_t handle, gapi_msgs_e msg_id,
                                  gapi_line_t *line);

typedef int (gapi_configstat_req_f)(gapi_handle_t handle, gapi_msgs_e msg_id);

typedef int (gapi_configstat_res_f)(gapi_handle_t handle, gapi_msgs_e msg_id);

typedef int (gapi_timedate_req_f)(gapi_handle_t handle, gapi_msgs_e msg_id);

typedef int (gapi_timedate_res_f)(gapi_handle_t handle, gapi_msgs_e msg_id);

typedef int (gapi_buttontemplate_req_f)(gapi_handle_t handle,
                                        gapi_msgs_e msg_id);

typedef int (gapi_buttontemplate_res_f)(gapi_handle_t handle,
                                        gapi_msgs_e msg_id);

typedef int (gapi_version_req_f)(gapi_handle_t handle, gapi_msgs_e msg_id);

typedef int (gapi_version_res_f)(gapi_handle_t handle, gapi_msgs_e msg_id);

typedef int (gapi_capabilities_req_f)(gapi_handle_t handle,
                                      gapi_msgs_e msg_id);

typedef int (gapi_capabilities_res_f)(gapi_handle_t handle,
                                      gapi_msgs_e msg_id);

typedef int (gapi_softkeytemplate_req_f)(gapi_handle_t handle,
                                         gapi_msgs_e msg_id);

typedef int (gapi_softkeytemplate_res_f)(gapi_handle_t handle,
                                         gapi_msgs_e msg_id,
                                         gapi_softkey_template_t *softkey);

typedef int (gapi_softkeyset_req_f)(gapi_handle_t handle, gapi_msgs_e msg_id);

typedef int (gapi_softkeyset_res_f)(gapi_handle_t handle, gapi_msgs_e msg_id,
                                    gapi_softkey_set_t *softkeyset);


typedef int (gapi_all_streams_idle_f)(void *handle);    

typedef int (gapi_close_abandonded_streams_f)(void *handle);

typedef int (gapi_passthru_f)(gapi_handle_t handle, gapi_msgs_e msg_id,
                              void *msg, int len);

typedef int (gapi_sccp_cleanup_f)(gapi_handle_t handle, void *data);

typedef int (gapi_sccp_main_f)(gapi_handle_t handle, void *data);

typedef int (gapi_tcp_main_f)(gapi_handle_t handle, int msg_id, int socket,
                              gapi_tcp_events_e event, void *data, int size);

typedef struct gapi_callbacks_t_ {
#if 0 /* Used for code documentation */
    /*
     * FUNCTION:    setup
     *
     * DESCRIPTION: Initiates call setup.
     *
     * PARAMETERS:
     *     handle:     identifier used to identify the calling application
     *     msg_id:     message identifier
     *     conn_id:    connection identifier
     *     line:       identifies the line of the call
     *     conninfo:   identifies the calling and called parties
     *     digits:     digit(s) of the called party
     *     numdigits:  num of included digit(s)
     *     media:      describes the media attributes
     *     alert_info: identifies a network determined ringer
     *     privacy:    privacy release
     *     precedence: mlpp precedence information
     *
     * RETURNS:
     *     success: 0
     *     failure: 1
     */
    int (setup)(gapi_handle_t handle, gapi_msgs_e msg_id, int conn_id, int line,
                gapi_conninfo_t *conninfo, char *digits, int numdigits,
                gapi_media_t *media, gapi_alert_info_e alert_info,
                gapi_privacy_e privacy, gapi_precedence_t *precedence);

    /*
     * FUNCTION:    setup_ack
     *
     * DESCRIPTION: Indicates call establishment has been initiated, but
     *              additional information may be required.
     *
     * PARAMETERS:
     *     handle:     identifier used to identify the calling application
     *     msg_id:     message identifier
     *     conn_id:    connection identifier
     *     line:       identifies the line of the call
     *     conninfo:   identifies the calling and called parties
     *     media:      describes the media attributes
     *     cause:      indicates the status of the request
     *     precedence: mlpp precedence information
     *
     * RETURNS:     0 for sucess, otherwise 1
     */
    int (setup_ack)(gapi_handle_t handle, gapi_msgs_e msg_id, int conn_id, int line,
                    gapi_conninfo_t *conninfo, gapi_media_t *media,
                    gapi_causes_e cause, gapi_precedence_t *precedence);

    /*
     * FUNCTION:    proceeding
     * 
     * DESCRIPTION: Indicates call establishment has been initiated and no
     *              more additional information will be accepted.
     *
     * PARAMETERS:
     *     handle:     identifier used to identify the calling application
     *     msg_id:     message identifier
     *     conn_id:    connection identifier
     *     line:       identifies the line of the call
     *     conninfo:   identifies the calling and called parties
     *     precedence: mlpp precedence information
     *
     * RETURNS:     0 for sucess, otherwise 1
     */
    int (proceeding)(gapi_handle_t handle, gapi_msgs_e msg_id, int conn_id, int line,
                     gapi_conninfo_t *conninfo,
                     gapi_precedence_t *precedence);

    /*
     * FUNCTION:    alerting
     *
     * DESCRIPTION: Indicates that called user alerting has been initiated.
     *
     * PARAMETERS:
     *     handle:     identifier used to identify the calling application
     *     msg_id:     message identifier
     *     conn_id:    connection identifier
     *     line:       identifies the line of the call
     *     conninfo:   identifies the calling and called parties
     *     media:      describes the media attributes
     *     inband:     indicates if inband alerting is present
     *     precedence: mlpp precedence information
     *
     * RETURNS:
     *     success: 0
     *     failure: 1
     */
    int (alerting)(gapi_handle_t handle, gapi_msgs_e msg_id, int conn_id, int line,
                   gapi_conninfo_t *conninfo, gapi_media_t *media,
                   gapi_inband_e inband, gapi_precedence_t *precedence);

    /*
     * FUNCTION:    connect
     *
     * DESCRIPTION: Indicates call acceptance.
     *
     * PARAMETERS:
     *     handle:     identifier used to identify the calling application
     *     msg_id:     message identifier
     *     conn_id:    connection identifier
     *     line:       identifies the line of the call
     *     conninfo:   identifies the calling and called parties
     *     media:      describes the media attributes
     *     precedence: mlpp precedence information
     *
     * RETURNS:     0 for sucess, otherwise 1
     */    
    int (connect)(gapi_handle_t handle, gapi_msgs_e msg_id, int conn_id, int line,
                  gapi_conninfo_t *conninfo, gapi_media_t *media,
                  gapi_precedence_t *precedence);

    /*
     * FUNCTION:    connect_ack
     *
     * DESCRIPTION: Indicates that the user has been awarded the call.
     *
     * PARAMETERS:
     *     handle:     identifier used to identify the calling application
     *     msg_id:     message identifier
     *     conn_id:    connection identifier
     *     line:       identifies the line of the call
     *     conninfo:   identifies the calling and called parties
     *     media:      describes the media attributes
     *     precedence: mlpp precedence information
     *
     * RETURNS:     0 for sucess, otherwise 1
     */
    int (connect_ack)(gapi_handle_t handle, gapi_msgs_e msg_id, int conn_id, int line,
                      gapi_conninfo_t *conninfo, gapi_media_t *media,
                      gapi_precedence_t *precedence);

    /*
     * FUNCTION:    disconnect
     *
     * DESCRIPTION: Used to request call clearing.
     *
     * PARAMETERS:
     *     handle:     identifier used to identify the calling application
     *     msg_id:     message identifier
     *     conn_id:    connection identifier
     *     line:       identifies the line of the call
     *     cause:      indicates the reason for call clearing
     *     precedence: mlpp precedence information
     *
     * RETURNS:     0 for sucess, otherwise 1
     */    
    int (disconnect)(gapi_handle_t handle, gapi_msgs_e msg_id, int conn_id, int line,
                     gapi_causes_e cause, gapi_precedence_t *precedence);

    /*
     * FUNCTION:    release
     *
     * DESCRIPTION: Indicates that the sender has disconnected the call and
     *              intends to release all resources associated with the
     *              call.
     *
     * PARAMETERS:
     *     handle:     identifier used to identify the calling application
     *     msg_id:     message identifier
     *     conn_id:    connection identifier
     *     line:       identifies the line of the call
     *     cause:      indicates the reason for call clearing
     *     precedence: mlpp precedence information
     *
     * RETURNS:     0 for sucess, otherwise 1
     */    
    int (release)(gapi_handle_t handle, gapi_msgs_e msg_id, int conn_id, int line,
                  gapi_causes_e cause, gapi_precedence_t *precedence);

    /*
     * FUNCTION:    release_complete
     *
     * DESCRIPTION: Indicates that the sender has disconnected the call and
     *              released all resources associated with the call.
     *
     * PARAMETERS:
     *     handle:     identifier used to identify the calling application
     *     msg_id:     message identifier
     *     conn_id:    connection identifier
     *     line:       identifies the line of the call
     *     cause:      indicates the reason for call clearing
     *     precedence: mlpp precedence information
     *
     * RETURNS:     0 for sucess, otherwise 1
     */
    int (release_complete)(gapi_handle_t handle, gapi_msgs_e msg_id, int conn_id,
                           int line, gapi_causes_e cause,
                           gapi_precedence_t *precedence);    

    /*
     * FUNCTION:    softkey
     *
     * DESCRIPTION: Indicates that a specific softkey has been selected.
     *
     * PARAMETERS:
     *     handle:  identifier used to identify the calling application
     *     msg_id:  message identifier
     *     conn_id: connection identifier
     *     line:    identifies the line of the call
     *     softkey: identifies the softkey
     *
     * RETURNS:     0 for sucess, otherwise 1
     */        
    int (softkey)(gapi_handle_t handle, gapi_msgs_e msg_id, int conn_id,
                  int line, int softkey);

    /*
     * FUNCTION:    feature_req
     *
     * DESCRIPTION: Indicates that a specific feature has been requested.
     *
     * PARAMETERS:
     *     handle:  identifier used to identify the calling application
     *     msg_id:  message identifier
     *     conn_id: connection identifier
     *     line:    identifies the line of the call
     *     feature: identifies the feature
     *     data:    describes feature specific data
     *     cause:   indicates the reason for the request
     *
     * RETURNS:     0 for sucess, otherwise 1
     */        
    int (feature_req)(gapi_handle_t handle, gapi_msgs_e msg_id, int conn_id, int line,
                      int feature, void *data, gapi_causes_e cause);

    /*
     * FUNCTION:    feature_res
     *
     * DESCRIPTION: Response to a feature request.
     *
     * PARAMETERS:
     *     handle:  identifier used to identify the calling application
     *     msg_id:  message identifier
     *     conn_id: connection identifier
     *     line:    identifies the line of the call
     *     feature: identifies the feature
     *     data:    describes feature specific data
     *     cause:   indicates the status of the request
     *
     * RETURNS:     0 for sucess, otherwise 1
     */        
    int (feature_res)(gapi_handle_t handle, gapi_msgs_e msg_id, int conn_id, int line,
                      int feature, void *data, gapi_causes_e cause);

    /*
     * FUNCTION:    openrcv_req
     *
     * DESCRIPTION: Request to begin receiving a unicast RTP stream.
     *
     * PARAMETERS:
     *     handle:  identifier used to identify the calling application
     *     msg_id:  message identifier
     *     conn_id: connection identifier
     *     line:    identifies the line of the call
     *     media:   describes the media attributes
     *
     * RETURNS:     0 for sucess, otherwise 1
     */        
    int (openrcv_req)(gapi_handle_t handle, gapi_msgs_e msg_id, int conn_id, int line,
                      gapi_media_t *media);
    
    /*
     * FUNCTION:    openrcv_res
     *
     * DESCRIPTION: Response to an openrcv_req.
     *
     * PARAMETERS:
     *     handle:  identifier used to identify the calling application
     *     msg_id:  message identifier
     *     conn_id: connection identifier
     *     line:    identifies the line of the call
     *     media:   describes the media attributes
     *     cause:   indicates the status of the request
     *
     * RETURNS:     0 for sucess, otherwise 1
     */         
    int (openrcv_res)(gapi_handle_t handle, gapi_msgs_e msg_id, int conn_id, int line,
                      gapi_media_t *media, gapi_causes_e cause);

    /*
     * FUNCTION:    closercv
     *
     * DESCRIPTION: Request to terminate the reception of an RTP stream.
     *
     * PARAMETERS:
     *     handle:  identifier used to identify the calling application
     *     msg_id:  message identifier
     *     conn_id: connection identifier
     *
     * RETURNS:     0 for sucess, otherwise 1
     */            
    int (closercv)(gapi_handle_t handle, gapi_msgs_e msg_id, int conn_id, int line);

    /*
     * FUNCTION:    startxmit
     *
     * DESCRIPTION: Request to begin transemitting a unicast RTP stream.
     *
     * PARAMETERS:
     *     handle:  identifier used to identify the calling application
     *     msg_id:  message identifier
     *     conn_id: connection identifier
     *     line:    identifies the line of the call
     *     media:   describes the media attributes
     *
     * RETURNS:     0 for sucess, otherwise 1
     */        
    int (startxmit)(gapi_handle_t handle, gapi_msgs_e msg_id, int conn_id, int line,
                    gapi_media_t *media);

    /*
     * FUNCTION:    stopxmit
     *
     * DESCRIPTION: Request to stop transemitting a unicast RTP stream.
     *
     * PARAMETERS:
     *     handle:  identifier used to identify the calling application
     *     msg_id:  message identifier
     *     conn_id: connection identifier
     *     line:    identifies the line of the call
     *     media:   describes the media attributes
     *
     * RETURNS:     0 for sucess, otherwise 1
     */        
    int (stopxmit)(gapi_handle_t handle, gapi_msgs_e msg_id, int conn_id, int line);

    /*
     * FUNCTION:    offhook
     *
     * DESCRIPTION: Indicates that the client is in an offhook condition.
     *
     * PARAMETERS:
     *     handle:  identifier used to identify the calling application
     *     msg_id:  message identifier
     *     conn_id: connection identifier
     *     line:    identifies the line of the call
     *
     * RETURNS:     0 for sucess, otherwise 1
     */        
    int (offhook)(gapi_handle_t handle, gapi_msgs_e msg_id, int conn_id, int line);

    /*
     * FUNCTION:    onhook
     *
     * DESCRIPTION: Indicates that the client is in an onhook condition.
     *
     * PARAMETERS:
     *     handle:  identifier used to identify the calling application
     *     msg_id:  message identifier
     *     conn_id: connection identifier
     *     line:    identifies the line of the call
     *
     * RETURNS:     0 for sucess, otherwise 1
     */        
    int (onhook)(gapi_handle_t handle, gapi_msgs_e msg_id, int conn_id, int line);

    /*
     * FUNCTION:    hookflash
     *
     * DESCRIPTION: Indicates that a hookflash has occurred.
     *
     * PARAMETERS:
     *     handle:  identifier used to identify the calling application
     *     msg_id:  message identifier
     *     conn_id: connection identifier
     *     line:    identifies the line of the call
     *
     * RETURNS:     0 for sucess, otherwise 1
     */        
    int (hookflash)(gapi_handle_t handle, gapi_msgs_e msg_id, int conn_id, int line);

    /*
     * FUNCTION:    digits
     *
     * DESCRIPTION: Indicates user entered digit(s).
     *
     * PARAMETERS:
     *     handle:    identifier used to identify the calling application
     *     msg_id:    message identifier
     *     conn_id:   connection identifier
     *     line:      identifies the line of the call
     *     digits:    digit(s) of the called party
     *     numdigits: num of included digit(s)
     *
     * RETURNS:     0 for sucess, otherwise 1
     */        
    int (digits)(gapi_handle_t handle, gapi_msgs_e msg_id, int conn_id, int line,
                 char *digits, int numdigits);

    /*
     * FUNCTION:    starttone
     *
     * DESCRIPTION: Request to play a specific tone.
     *
     * PARAMETERS:
     *     handle:    identifier used to identify the calling application
     *     msg_id:    message identifier
     *     conn_id:   connection identifier
     *     line:      identifies the line of the call
     *     tone:      requested tone
     *     direction: play to the user or the network
     *
     * RETURNS:     0 for sucess, otherwise 1
     */        
    int (starttone)(gapi_handle_t handle, gapi_msgs_e msg_id, int conn_id, int line,
                    gapi_tones_e tone, gapi_tone_directions_e direction);

    /*
     * FUNCTION:    stoptone
     *
     * DESCRIPTION: Request to stop playing the current tone.
     *
     * PARAMETERS:
     *     handle:  identifier used to identify the calling application
     *     msg_id:  message identifier
     *     conn_id: connection identifier
     *     line:    identifies the line of the call
     *
     * RETURNS:     0 for sucess, otherwise 1
     */        
    int (stoptone)(gapi_handle_t handle, gapi_msgs_e msg_id, int conn_id, int line);

    /*
     * FUNCTION:    ringer
     *
     * DESCRIPTION: Request to play a specific ringer.
     *
     * PARAMETERS:
     *     handle:   identifier used to identify the calling application
     *     msg_id:   message identifier
     *     conn_id:  connection identifier
     *     line:     identifies the line of the call
     *     ringer:   requested ringer
     *     duration: requested duration
     *
     * RETURNS:     0 for sucess, otherwise 1
     */        
    int (ringer)(gapi_handle_t handle, gapi_msgs_e msg_id, int conn_id, int line,
                 gapi_ringers_e mode, gapi_duration_e duration);

    /*
     * FUNCTION:    lamp
     *
     * DESCRIPTION: Request for a specific lamp mode.
     *
     * PARAMETERS:
     *     handle:   identifier used to identify the calling application
     *     msg_id:   message identifier
     *     conn_id:  connection identifier
     *     line:     identifies the line of the call
     *     stimulus: stimulus lamp
     *     mode:     requested mode
     *
     * RETURNS:     0 for sucess, otherwise 1
     */        
    int (lamp)(gapi_handle_t handle, gapi_msgs_e msg_id, int conn_id, int line,
               int stimulus, gapi_lamps_e lamp);

    /*
     * FUNCTION:    speaker
     *
     * DESCRIPTION: Request to turn the speaker on or off.
     *
     * PARAMETERS:
     *     handle:  identifier used to identify the calling application
     *     msg_id:  message identifier
     *     conn_id: connection identifier
     *     line:    identifies the line of the call
     *     mode:    requested mode
     *
     * RETURNS:     0 for sucess, otherwise 1
     */        
    int (speaker)(gapi_handle_t handle, gapi_msgs_e msg_id, int conn_id, int line,
                  gapi_speakers_t mode);
    
    /*
     * FUNCTION:    micro
     *
     * DESCRIPTION: Request to turn the microphone on or off.
     *
     * PARAMETERS:
     *     handle:  identifier used to identify the calling application
     *     msg_id:  message identifier
     *     conn_id: connection identifier
     *     line:    identifies the line of the call
     *     mode:    requested mode
     *
     * RETURNS:     0 for sucess, otherwise 1
     */        
    int (micro)(gapi_handle_t handle, gapi_msgs_e msg_id, int conn_id, int line,
                gapi_microphones_t mode);

    /*
     * FUNCTION:    headset
     *
     * DESCRIPTION: Request to turn the headset on or off.
     *
     * PARAMETERS:
     *     handle:  identifier used to identify the calling application
     *     msg_id:  message identifier
     *     conn_id: connection identifier
     *     line:    identifies the line of the call
     *     mode:    requested mode
     *
     * RETURNS:     0 for sucess, otherwise 1
     */        
    int (headset)(gapi_handle_t handle, gapi_msgs_e msg_id, int conn_id, int line,
                  int mode);

    /*
     * FUNCTION:    timedate
     *
     * DESCRIPTION: Notify the client of the current time.
     *
     * PARAMETERS:
     *     handle:  identifier used to identify the calling application
     *     msg_id:  message identifier
     *     conn_id: connection identifier
     *     line:    identifies the line of the call
     *     time:    time
     *
     * RETURNS:     0 for sucess, otherwise 1
     */        
    int (timedate)(gapi_handle_t handle, gapi_msgs_e msg_id, int conn_id, int line,
                   int time);

    /*
     * FUNCTION:    conninfo
     *
     * DESCRIPTION: Notify the client of connection information.
     *
     * PARAMETERS:
     *     handle:   identifier used to identify the calling application
     *     msg_id:   message identifier
     *     conn_id:  connection identifier
     *     line:     identifies the line of the call
     *     conninfo: connection information
     *
     * RETURNS:     0 for sucess, otherwise 1
     */        
    int (conninfo)(gapi_handle_t handle, gapi_msgs_e msg_id, int conn_id, int line,
                   gapi_conninfo_t *conninfo);

    /*
     * FUNCTION:    display
     *
     * DESCRIPTION: Request to display text.
     *
     * PARAMETERS:
     *     handle:  identifier used to identify the calling application
     *     msg_id:  message identifier
     *     conn_id: connection identifier
     *     line:    identifies the line of the call
     *     text:    text to display
     *
     * RETURNS:     0 for sucess, otherwise 1
     *
     * NOTE:        The text is null terminated. The client is expected to
     *              pad the field with blanks.
     */        
    int (display)(gapi_handle_t handle, gapi_msgs_e msg_id, int conn_id, int line,
                  char *text);

    /*
     * FUNCTION:    cleardisplay
     *
     * DESCRIPTION: Request to clear display text.
     *
     * PARAMETERS:
     *     handle:  identifier used to identify the calling application
     *     msg_id:  message identifier
     *     conn_id: connection identifier
     *     line:    identifies the line of the call
     *
     * RETURNS:     0 for sucess, otherwise 1
     */        
    int (cleardisplaytext)(gapi_handle_t handle, gapi_msgs_e msg_id,
                           int conn_id, int line);

    /*
     * FUNCTION:    prompt
     *
     * DESCRIPTION: Request to display a prompt status.
     *
     * PARAMETERS:
     *     handle:  identifier used to identify the calling application
     *     msg_id:  message identifier
     *     conn_id: connection identifier
     *     line:    identifies the line of the call
     *     text:    text to display
     *
     * RETURNS:     0 for sucess, otherwise 1
     *
     * NOTE:        The text is null terminated. The client is expected to
     *              pad the field with blanks.
     */            
    int (prompt)(gapi_handle_t handle, gapi_msgs_e msg_id, int conn_id, int line,
                 char *text, unsigned int timeout);

    /*
     * FUNCTION:    clearprompt
     *
     * DESCRIPTION: Request to clear a prompt status.
     *
     * PARAMETERS:
     *     handle:  identifier used to identify the calling application
     *     msg_id:  message identifier
     *     conn_id: connection identifier
     *     line:    identifies the line of the call
     *
     * RETURNS:
     *     success: 0
     *     failure: 1
     */            
    int (clearprompt)(gapi_handle_t handle, gapi_msgs_e msg_id, int conn_id, int line);

    /*
     * FUNCTION:    notify
     *
     * DESCRIPTION: Request to display a notification for a period
     *              of time.
     *
     * PARAMETERS:
     *     handle:  identifier used to identify the calling application
     *     msg_id:  message identifier
     *     conn_id: connection identifier
     *     line:    identifies the line of the call
     *     text:    text to display
     *     timeout: timeout period
     *
     * RETURNS:     0 for sucess, otherwise 1
     *
     * NOTE:        The text is null terminated. The client is expected to
     *              pad the field with blanks.
     */                
    int (notify)(gapi_handle_t handle, gapi_msgs_e msg_id, int conn_id, int line,
                 char *text, unsigned int timeout);

    /*
     * FUNCTION:    clearnotify
     *
     * DESCRIPTION: Request to clear a notification.
     *
     * PARAMETERS:
     *     handle:  identifier used to identify the calling application
     *     msg_id:  message identifier
     *     conn_id: connection identifier
     *     line:    identifies the line of the call
     *
     * RETURNS:     0 for sucess, otherwise 1
     */            
    int (clearnotify)(gapi_handle_t handle, gapi_msgs_e msg_id,
                      int conn_id, int line);

    /*
     * FUNCTION:    activateplane
     *
     * DESCRIPTION: Request to activate a plane.
     *
     * PARAMETERS:
     *     handle:  identifier used to identify the calling application
     *     msg_id:  message identifier
     *     conn_id: connection identifier
     *     line:    identifies the line of the call
     *
     * RETURNS:     0 for sucess, otherwise 1
     */                
    int (activateplane)(gapi_handle_t handle, gapi_msgs_e msg_id,
                        int conn_id, int line);

    /*
     * FUNCTION:    deactivateplane
     *
     * DESCRIPTION: Request to deactivate a plane.
     *
     * PARAMETERS:
     *     handle:  identifier used to identify the calling application
     *     msg_id:  message identifier
     *     conn_id: connection identifier
     *     line:    identifies the line of the call
     *
     * RETURNS:     0 for sucess, otherwise 1
     */                    
    int (deactivateplane)(gapi_handle_t handle, gapi_msgs_e msg_id, int conn_id,
                          int line);
    
    /*
     * FUNCTION:    connectionstats
     *
     * DESCRIPTION: Request for connection statistics for the indicated
     *              connection.
     *
     * PARAMETERS:
     *     handle:  identifier used to identify the calling application
     *     msg_id:  message identifier
     *     conn_id: connection identifier
     *     line:    identifies the line of the call
     *     stats:   memory for the statistics. Application should only set
     *              the values after processing_mode.
     *
     * RETURNS:     0 for sucess, otherwise 1
     *     stats:   connection statistics
     */                    
    int (connectionstats)(gapi_handle_t handle, gapi_msgs_e msg_id, int conn_id,
                          int line, gapi_connection_stats_t *stats);

    /*
     * FUNCTION:    backspace_req
     *
     * DESCRIPTION: Request to backspace the last character ended
     *
     * PARAMETERS:
     *     handle:  identifier used to identify the calling application
     *     msg_id:  message identifier
     *     conn_id: connection identifier
     *     line:    identifies the line of the call
     *
     * RETURNS:     0 for sucess, otherwise 1
     */                    
    int (backspace_req)(gapi_handle_t handle, gapi_msgs_e msg_id, int conn_id,
                        int line);

    /*
     * FUNCTION:    selectsoftkeys
     *
     * DESCRIPTION: Request to deactivate a plane.
     *
     * PARAMETERS:
     *     handle:  identifier used to identify the calling application
     *     msg_id:  message identifier
     *     conn_id: connection identifier
     *     line:    identifies the line of the call
     *     set:     softkey set
     *     mask:    identifies keys in the set that are active
     *
     * RETURNS:     0 for sucess, otherwise 1
     */                    
    int (selectsoftkeys)(gapi_handle_t handle, gapi_msgs_e msg_id, int conn_id,
                         int line, unsigned long set, unsigned long mask);

    /*
     * FUNCTION:    dialednumber
     *
     * DESCRIPTION: Indication of the dialed number for an outgoing call.
     *
     * PARAMETERS:
     *     handle:  identifier used to identify the calling application
     *     msg_id:  message identifier
     *     conn_id: connection identifier
     *     line:    identifies the line of the call
     *     number:  dialed number
     *
     * RETURNS:     0 for sucess, otherwise 1
     */                    
    int (dialednumber)(gapi_handle_t handle, gapi_msgs_e msg_id, int conn_id,
                       int line, char *number);

    /*
     * FUNCTION:    usertodevicedata
     *
     * DESCRIPTION: User to device data.
     *
     * PARAMETERS:
     *     handle:  identifier used to identify the calling application
     *     msg_id:  message identifier
     *     conn_id: connection identifier
     *     line:    identifies the line of the call
     *     data:    data
     *
     * RETURNS:     0 for sucess, otherwise 1
     */                    
    int (usertodevicedata)(gapi_handle_t handle, gapi_msgs_e msg_id, int conn_id,
                           int line, gapi_user_and_device_data_t *data);

    /*
     * FUNCTION:    devicetouserdata_req
     *
     * DESCRIPTION: Data sent from a device to an application on the call
     *              controller.
     *
     * PARAMETERS:
     *     handle:  identifier used to identify the calling application
     *     msg_id:  message identifier
     *     conn_id: connection identifier
     *     line:    identifies the line of the call
     *     data:    data
     *
     * RETURNS:     0 for sucess, otherwise 1
     */                    
    int (devicetouserdata_req)(gapi_handle_t handle, gapi_msgs_e msg_id,
                               int conn_id, int line,
                               gapi_user_and_device_data_t *data);

    /*
     * FUNCTION:    devicetouserdata_res
     *
     * DESCRIPTION: Data sent by the device to acknowledge receipt of a
     *              user_to_device_data.
     *
     * PARAMETERS:
     *     handle:  identifier used to identify the calling application
     *     msg_id:  message identifier
     *     conn_id: connection identifier
     *     line:    identifies the line of the call
     *     data:    data
     *
     * RETURNS:     0 for sucess, otherwise 1
     */                    
    int (devicetouserdata_res)(gapi_handle_t handle, gapi_msgs_e msg_id,
                               int conn_id, int line,
                               gapi_user_and_device_data_t *data);

    /*
     * FUNCTION:    prioritynotify
     *
     * DESCRIPTION: Request to display a notification for a period
     *              of time and with priority.
     *
     * PARAMETERS:
     *     handle:  identifier used to identify the calling application
     *     msg_id:  message identifier
     *     conn_id: connection identifier
     *     line:    identifies the line of the call
     *     text:       text to display
     *     priority:   priority
     *
     * RETURNS:     0 for sucess, otherwise 1
     *
     * NOTE:        The text is null terminated. The client is expected to
     *              pad the field with blanks.
     */                
    int (prioritynotify)(gapi_handle_t handle, gapi_msgs_e msg_id, int conn_id,
                         int line, char *text, unsigned int timeout,
                         unsigned long priority);

    /*
     * FUNCTION:    clearprioritynotify
     *
     * DESCRIPTION: DESCRIPTION: Request to clear a priority notification.
     *
     * PARAMETERS:
     *     handle:   identifier used to identify the calling application
     *     msg_id:   message identifier
     *     conn_id:  connection identifier
     *     line:     identifies the line of the call
     *     priority: priority
     *
     * RETURNS:     0 for sucess, otherwise 1
     *
     * NOTE:        The text is null terminated. The client is expected to
     *              pad the field with blanks.
     */                
    int (clearprioritynotify)(gapi_handle_t handle, gapi_msgs_e msg_id, int conn_id,
                             int line, unsigned long priority);

    /*
     * FUNCTION:    reset
     *
     * DESCRIPTION: Indication from the stack to the application, that
     *              the application should request a reset. The application
     *              should request the reset as indicated in the reset
     *              indication. It is expected that the application will send
     *              another opensession_req after the reset is complete unless
     *              a restart was requested, in which case the stack will
     *              automatically restart.
     *
     * PARAMETERS:
     *     handle:  identifier used to identify the calling application
     *     msg_id:  message identifier
     *     cause:   indicates the cause for the indication
     *
     * RETURNS:     0 for sucess, otherwise 1
     */                    
    int (reset)(gapi_handle_t handle, gapi_msgs_e msg_id, 
                gapi_causes_e cause);

    /*
     * FUNCTION:    opensession_req
     *
     * DESCRIPTION: Request to open a session.
     *
     * PARAMETERS:
     *     handle:     identifier used to identify the calling application
     *     msg_id:     message identifier
     *     cmaddrs:    List of 5 CallManagers. All unused elements must
     *                 be initialized to 0.
     *     mac:        mac address to identify the stack. String must be NULL
     *                 terminated and length no longer than GAPI_MAX_MAC_SIZE.
     *     srsts:      List of 5 SRST CallManagers.  All unused elements must
     *                 be initialized to 0.
     *     srst_mode:  mode indicating SRST usage 
     *     tftp:       tftp CallManager
     *     media_caps: media capabilities supported by the client
     *     values:     configurable values
     *
     * RETURNS:     0 for sucess, otherwise 1
     */                
    int (opensession_req)(gapi_handle_t handle, gapi_msgs_e msg_id,
                          gapi_cmaddr_t *cmaddrs, char *mac,
                          gapi_cmaddr_t *srsts,
                          gapi_srst_modes_e srst_mode,
                          gapi_cmaddr_t *tftp,
                          gapi_media_caps_t *media_caps,
                          gapi_opensession_values_t *values);

    /*
     * FUNCTION:    opensession_res
     *
     * DESCRIPTION: Response to an an opensession_req
     *
     * PARAMETERS:
     *     handle:  identifier used to identify the calling application
     *     msg_id:  message identifier
     *     cause:   indicates the status of the request
     *
     * RETURNS:     0 for sucess, otherwise 1
     */                    
    int (opensession_res)(gapi_handle_t handle, gapi_msgs_e msg_id,
                          gapi_causes_e cause);

    /*
     * FUNCTION:    sessionstatus
     *
     * DESCRIPTION: Indicates the status of the session.
     *
     * PARAMETERS:
     *     handle:  identifier used to identify the calling application
     *     msg_id:  message identifier
     *     cause:   indicates the status of the session
     *
     * RETURNS:     0 for sucess, otherwise 1
     */                    
    int (sessionstatus)(gapi_handle_t handle, gapi_msgs_e msg_id,
                        gapi_status_e cause);

    /*
     * FUNCTION:    resetsession_req
     *
     * DESCRIPTION: Request to reset the session.
     *
     * PARAMETERS:
     *     handle:  identifier used to identify the calling application
     *     msg_id:  message identifier
     *     cause:   indicates the cause for the request
     *
     * RETURNS:     0 for sucess, otherwise 1
     */                    
    int (resetsession_req)(gapi_handle_t handle, gapi_msgs_e msg_id,
                           gapi_causes_e cause);

    /*
     * FUNCTION:    resetsession_res
     *
     * DESCRIPTION: Indicates the status of the resetsession request.
     *
     * PARAMETERS:
     *     handle:  identifier used to identify the calling application
     *     msg_id:  message identifier
     *     cause:   indicates the status of the request
     *
     * RETURNS:     0 for sucess, otherwise 1
     */                    
    int (resetsession_res)(gapi_handle_t handle, gapi_msgs_e msg_id,
                           gapi_causes_e cause);
    /*
     * FUNCTION:    all_streams_idle
     *
     * DESCRIPTION: Indicates if the application is idle - there are no active
     *              calls. The stack will normally call this function when 
     *              it wants to reset to verify that there are no active calls
     *              in progess.
     *
     * PARAMETERS:
     *     handle:  identifier used to identify the calling application
     *
     * RETURNS:     0 for sucess, otherwise 1
     */                    
    int (all_streams_idle)(void *handle);    

    /*
     * FUNCTION:    close_abandoned_streams
     *
     * DESCRIPTION: Request to close any media streams. The stack will
     *              normally call this function while resetting to allow
     *              the applpication to clean up any existing calls.
     *
     * PARAMETERS:
     *     handle:  identifier used to identify the calling application
     *
     * RETURNS:     0 for sucess, otherwise 1
     */                    
    int (close_abandonded_streams)(void *handle);    

    /*
     * FUNCTION:    sccp_cleanup
     *
     * DESCRIPTION: Cleanup function releases all resources in use by the
     *              stack.
     *
     * PARAMETERS:
     *     handle:  identifier used to identify the calling application
     *     data:    for future use
     *
     * RETURNS:     0 for sucess, otherwise 1
     */                    
    int (sccp_cleanup)(gapi_handle_t handle, void *data);

    /*
     * FUNCTION:    sccp_main
     *
     * DESCRIPTION: Main event handler for GAPI. The application will call
     *              this function when it detects that the stack has an event
     *              that needs processing.
     *
     * PARAMETERS:
     *     handle:  identifier used to identify the calling application
     *     data:    event 
     *
     * RETURNS:     0 for sucess, otherwise 1
     */                    
    int (sccp_main)(gapi_handle_t handle, void *data);

    /*
     * FUNCTION:    tcp_main
     *
     * DESCRIPTION: Main TCP/IP event handler for GAPI. The application will
     *              call this function when it detects data that has been 
     *              received on a TCP socket.
     *
     * PARAMETERS:
     *     handle:  identifier used to identify the calling application
     *     msg_id:  message identifier
     *     socket:  identifies the TCP/IP socket the message
     *              was received
     *     event:   identifies the event, ie. data, ack...
     *     data:    message
     *     size:    size of data
     *
     * RETURNS:     0 for sucess, otherwise 1
     */                    
    int (tcp_main)(gapi_handle_t handle, gapi_msgs_e msg_id, int socket,
                   gapi_tcp_events_e event, void *data, int size);

    /* not supported yet. */
    int (keepalive_req)(gapi_handle_t handle, gapi_msgs_e msg_id);
    int (keepalive_res)(gapi_handle_t handle, gapi_msgs_e msg_id);
    int (register_req)(gapi_handle_t handle, gapi_msgs_e msg_id);
    int (register_res)(gapi_handle_t handle, gapi_msgs_e msg_id, int status);
    int (alarm)(gapi_handle_t handle, gapi_msgs_e msg_id);
    int (forwardstat_req)(gapi_handle_t handle, gapi_msgs_e msg_id);
    int (forwardstat_res)(gapi_handle_t handle, gapi_msgs_e msg_id);
    int (speeddialstat_req)(gapi_handle_t handle, gapi_msgs_e msg_id);
    int (speeddialstat_res)(gapi_handle_t handle, gapi_msgs_e msg_id);
    int (linestat_req)(gapi_handle_t handle, gapi_msgs_e msg_id);
    int (linestat_res)(gapi_handle_t handle, gapi_msgs_e msg_id);
    int (configstat_req)(gapi_handle_t handle, gapi_msgs_e msg_id);
    int (configstat_res)(gapi_handle_t handle, gapi_msgs_e msg_id);
    int (timedate_req)(gapi_handle_t handle, gapi_msgs_e msg_id);
    int (timedate_res)(gapi_handle_t handle, gapi_msgs_e msg_id);
    int (buttontemplate_req)(gapi_handle_t handle, gapi_msgs_e msg_id);
    int (buttontemplate_res)(gapi_handle_t handle, gapi_msgs_e msg_id);
    int (version_req)(gapi_handle_t handle, gapi_msgs_e msg_id);
    int (version_res)(gapi_handle_t handle, gapi_msgs_e msg_id);
    int (capabilities_req)(gapi_handle_t handle, gapi_msgs_e msg_id);
    int (capabilities_res)(gapi_handle_t handle, gapi_msgs_e msg_id);
    int (softkeytemplate_req)(gapi_handle_t handle, gapi_msgs_e msg_id);
    int (softkeytemplate_res)(gapi_handle_t handle, gapi_msgs_e msg_id);
    int (softkeyset_req)(gapi_handle_t handle, gapi_msgs_e msg_id);
    int (softkeyset_res)(gapi_handle_t handle, gapi_msgs_e msg_id);
    int (availablelines)(gapi_handle_t handle, gapi_msgs_e msg_id);
    
    int (activecalls)(void *handle);

#endif /* Used for code documentation */

    gapi_setup_f                      *setup;
    gapi_setup_ack_f                  *setup_ack;
    gapi_proceeding_f                 *proceeding;
    gapi_alerting_f                   *alerting;
    gapi_connect_f                    *connect;
    gapi_connect_ack_f                *connect_ack;
    gapi_disconnect_f                 *disconnect;
    gapi_release_f                    *release;
    gapi_release_complete_f           *release_complete;
    
    gapi_softkey_f                    *softkey;
    gapi_feature_req_f                *feature_req;
    gapi_feature_res_f                *feature_res;

    gapi_openrcv_req_f                *openrcv_req;
    gapi_openrcv_res_f                *openrcv_res;
    gapi_closercv_f                   *closercv;
    gapi_startxmit_f                  *startxmit;
    gapi_stopxmit_f                   *stopxmit;
    
    gapi_offhook_f                    *offhook;
    gapi_onhook_f                     *onhook;
    gapi_hookflash_f                  *hookflash;
    gapi_digits_f                     *digits;
    gapi_starttone_f                  *starttone;
    gapi_stoptone_f                   *stoptone;
    gapi_ringer_f                     *ringer;
    gapi_lamp_f                       *lamp;
    gapi_speaker_f                    *speaker;
    gapi_micro_f                      *micro;
    gapi_headset_f                    *headset;
    gapi_timedate_f                   *timedate;
    gapi_conninfo_f                   *conninfo;
    gapi_display_f                    *display;
    gapi_cleardisplay_f               *cleardisplay;
    gapi_prompt_f                     *prompt;
    gapi_clearprompt_f                *clearprompt;
    gapi_notify_f                     *notify;
    gapi_clearnotify_f                *clearnotify;
    gapi_activateplane_f              *activateplane;
    gapi_deactivateplane_f            *deactivateplane;
    gapi_connectionstats_f            *connectionstats;
    gapi_backspace_req_f              *backspace_req;
    gapi_selectsoftkeys_f             *selectsoftkeys;
    gapi_dialednumber_f               *dialednumber;
    gapi_usertodevicedata_f           *usertodevicedata;
    gapi_devicetouserdata_req_f       *devicetouserdata_req;
    gapi_devicetouserdata_res_f       *devicetouserdata_res;
    gapi_prioritynotify_f             *prioritynotify;
    gapi_clearprioritynotify_f        *clearprioritynotify;
                              
    gapi_reset_f                      *reset;
    gapi_opensession_req_f            *opensession_req;
    gapi_opensession_res_f            *opensession_res;
    gapi_sessionstatus_f              *sessionstatus;

    gapi_resetsession_req_f           *resetsession_req;
    gapi_resetsession_res_f           *resetsession_res;

    gapi_keepalive_req_f              *keepalive_req;
    gapi_keepalive_res_f              *keepalive_res;
    gapi_register_req_f               *register_req;
    gapi_register_res_f               *register_res;
    gapi_alarm_f                      *alarm;
    gapi_forwardstat_req_f            *forwardstat_req;
    gapi_forwardstat_res_f            *forwardstat_res;
    gapi_serviceurlstat_req_f         *serviceurlstat_req;
    gapi_serviceurlstat_res_f         *serviceurlstat_res;
    gapi_featurestat_req_f            *featurestat_req;
    gapi_featurestat_res_f            *featurestat_res;
    gapi_speeddialstat_req_f          *speeddialstat_req;
    gapi_speeddialstat_res_f          *speeddialstat_res;
    gapi_linestat_req_f               *linestat_req;
    gapi_linestat_res_f               *linestat_res;
    gapi_configstat_req_f             *configstat_req;
    gapi_configstat_res_f             *configstat_res;
    gapi_timedate_req_f               *timedate_req;
    gapi_timedate_res_f               *timedate_res;
    gapi_buttontemplate_req_f         *buttontemplate_req;
    gapi_buttontemplate_res_f         *buttontemplate_res;
    gapi_version_req_f                *version_req;
    gapi_version_res_f                *version_res;
    gapi_capabilities_req_f           *capabilities_req;
    gapi_capabilities_res_f           *capabilities_res;
    gapi_softkeytemplate_req_f        *softkeytemplate_req;
    gapi_softkeytemplate_res_f        *softkeytemplate_res;
    gapi_softkeyset_req_f             *softkeyset_req;
    gapi_softkeyset_res_f             *softkeyset_res;

    gapi_passthru_f                   *passthru;
    
    gapi_all_streams_idle_f           *all_streams_idle;
    gapi_close_abandonded_streams_f   *close_abandonded_streams;

    gapi_sccp_cleanup_f               *sccp_cleanup;
    gapi_sccp_main_f                  *sccp_main;
    gapi_tcp_main_f                   *tcp_main;
} gapi_callbacks_t;

/*
 * FUNCTION:    gapi_bind
 *
 * DESCRIPTION: Registers the application's callbacks and handle. The function
 *              returns the stack's callbacks and handle.
 *
 * PARAMETERS:
 *     appcbs:    application callbacks
 *     apphandle: identifier used to identify the calling application
 *     name:      string identifier to identify the calling application
 *
 * RETURNS:
 *     success:    0
 *     failure:    1
 *
 *     gapicbs:    gapi (stack) callbacks
 *     gapihandle: identifier used to identify the gapi
 */
int gapi_bind(gapi_callbacks_t *appcbs, gapi_callbacks_t **gapicbs,
              void *apphandle, void **gapihandle, char *appname);

/*
 * FUNCTION:    gapi_cleanup
 *
 * DESCRIPTION: Clean up the GAPI software, including the underlying stack.
 *              
 *
 * PARAMETERS:  None.
 *
 * RETURNS:
 *     success: 0
 *     failure: 1
 */                    
int gapi_cleanup(void);

/*
 * FUNCTION:    gapi_get_new_conn_id
 *
 * DESCRIPTION: Returns a unique connection identifier. All gapi call control
 *              requests must have a conn_id.
 *              
 * PARAMETERS:  None.
 *
 * RETURNS:     unique conn_id
 */                    
int gapi_get_new_conn_id(void);

/*
 * FUNCTION:    gapi_check_and_set_callbacks
 *
 * DESCRIPTION: Validates the supplied GAPI callbacks. Provides a default
 *              callback for uninitialized values.
 *
 * PARAMETERS:  
 *     cbs:     callbacks
 *
 * RETURNS:     None.
 */                    
void gapi_check_and_set_callbacks(gapi_callbacks_t *cbs);
#endif
