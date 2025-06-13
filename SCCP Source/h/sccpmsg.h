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
 *     sccpmsg.c
 *
 *  CREATION/MODIFICATION DATES
 *     Created:  November 2002, Sam Hague
 *     Modified: 
 *
 *  DESCRIPTION
 *     Skinny Client Control Protocol Messaging header file
 */
#ifndef _SCCPMSG_H_
#define _SCCPMSG_H_

#include "gapi.h"


struct sccp_sccpcb_t_;


#define SCCPMSG_BASE_LEN                       sizeof(sccpmsg_base_t)
#define SCCPMSG_NO_CALL_REFERENCE              (0)
#define SCCPMSG_NO_LINE                        (0)
#define SCCPMSG_NO_CALL_NUMBER                 (0xFFFFFFFF)
#define SCCPMSG_NO_PRIVACY                     (0)
#define SCCPMSG_NO_PRECEDENCE_LEVEL            (0)
#define SCCPMSG_NO_PRECEDENCE_DOMAIN           (0)

#define SCCPMSG_DIRECTORY_NUMBER_SIZE          (24)
#define SCCPMSG_DIRECTORY_NAME_SIZE            (40)
#define SCCPMSG_DEVICE_NAME_SIZE               (16)
#define SCCPMSG_MAX_SPEEDDIALS                 (100)
#define SCCPMSG_MAX_SERVICE_URLS               (40)
#define SCCPMSG_MAX_SERVICE_URL_SIZE           (256)
#define SCCPMSG_MAX_FEATURES                   (40)
#define SCCPMSG_MAX_VERSION_SIZE               (16)
#define SCCPMSG_MAX_BUTTON_TEMPLATE_SIZE       (42)
#define SCCPMSG_DISPLAY_TEXT_SIZE              (32)
#define SCCPMSG_MAX_MEDIA_CAPABILITIES         (18)
#define SCCPMSG_DATE_TEMPLATE_SIZE             (6)
#define SCCPMSG_DISPLAY_NOTIFY_TEXT_SIZE       (32)
#define SCCPMSG_DISPLAY_PROMPT_TEXT_SIZE       (32)
#define SCCPMSG_MAX_ALARM_TEXT_SIZE            (80)
#define SCCPMSG_MAX_SERVERS                    (5)
#define SCCPMSG_MAX_LINES                      (42)
#define SCCPMSG_SOFTKEY_LABEL_SIZE             (16)
#define SCCPMSG_MAX_SOFTKEY_DEFINITIONS        (32)
#define SCCPMSG_MAX_SOFTKEY_SET_DEFINITIONS    (16)
#define SCCPMSG_MAX_SOFTKEY_INDEXES            (16)
#define SCCPMSG_USER_DEVICE_DATA_SIZE          (1024)
#define SCCPMSG_DEFAULT_SERVER_PORT            (2000)
#define SCCPMSG_FIRMWARE_STRING                "SEP"
#define SCCPMSG_MAX_BUFFER_SIZE                (1400)
#define SCCPMSG_MAX_RTP_STREAMS                (SCCPMSG_MAX_LINES)

#define SCCPMSG_KEY_SIZE                       (16)

#define SCCPMSG_DEFAULT_KEEPALIVE_TIMEOUT      (10000)     // Value in milliseconds
#define SCCPMSG_SECONDARY_KEEPALIVE_TIMEOUT    (30000)     // Value in milliseconds
#define SCCPMSG_DEFAULT_UNREGISTER_TIMEOUT     (10000)     // Value in milliseconds
#define SCCPMSG_DEFAULT_DISCONNECT_TIMEOUT     (2000)       // Value in milliseconds
#define SCCPMSG_DEFAULT_WAIT_TIMEOUT           (2000)       // Value in milliseconds
#define SCCPMSG_MAX_MONITOR_PARTIES            (16)
#define SCCPMSG_MAX_ANNOUNCEMENT_LIST          (32)

#define SCCPMSG_INTERNATIONALIZATION_FEATURE_MASK  0x80000000
#define SCCPMSG_PROTOCOL_VERSION_SP30     0x00000001
#define SCCPMSG_PROTOCOL_VERSION_BRAVO    0x00000002
#define SCCPMSG_PROTOCOL_VERSION_HAWKBILL 0x00000003
#define SCCPMSG_PROTOCOL_VERSION_SEAVIEW  0x00000004
#define SCCPMSG_PROTOCOL_VERSION_PARCHE   0x00000005
#define SCCPMSG_PROTOCOL_VERSION_CURRENT  (SCCPMSG_PROTOCOL_VERSION_PARCHE | SCCPMSG_INTERNATIONALIZATION_FEATURE_MASK)
//#define SCCPMSG_PROTOCOL_VERSION_CURRENT (SCCPMSG_PROTOCOL_VERSION_SEAVIEW | SCCPMSG_INTERNATIONALIZATION_FEATURE_MASK)
#define SCCPMSG_TFTP_FILE  0x00000800
#define SCCPMSG_FIXED_TFTP 0x00010000
#define SCCPMSG_ALARM_PARAM1 (SCCPMSG_TFTP_FILE | SCCPMSG_FIXED_TFTP)


typedef struct sccpmsg_msgcb_t_ {
    void *cb;
    int  sem;
    char *sem_name;
} sccpmsg_msgcb_t;


typedef enum sccpmsg_messages_t_ {
    /*-------------- To the CM --------------*/    
    SCCPMSG_KEEPALIVE                                   = 0x0,
    SCCPMSG_REGISTER                                    = 0x1,
    SCCPMSG_IP_PORT                                     = 0x2,
    SCCPMSG_KEYPAD_BUTTON                               = 0x3,
    SCCPMSG_ENBLOC_CALL                                 = 0x4,
    SCCPMSG_STIMULUS                                    = 0x5,
    SCCPMSG_OFFHOOK                                     = 0x6,
    SCCPMSG_ONHOOK                                      = 0x7,
    SCCPMSG_HOOK_FLASH                                  = 0x8,
    SCCPMSG_FORWARD_STAT_REQ                            = 0x9,
    SCCPMSG_SPEEDDIAL_STAT_REQ                          = 0xa,
    SCCPMSG_LINE_STAT_REQ                               = 0xb,
    SCCPMSG_CONFIG_STAT_REQ                             = 0xc,
    SCCPMSG_TIME_DATE_REQ                               = 0xd,
    SCCPMSG_BUTTON_TEMPLATE_REQ                         = 0xe,
    SCCPMSG_VERSION_REQ                                 = 0xf,
    SCCPMSG_CAPABILITIES_RES                            = 0x10,
    SCCPMSG_MEDIA_PORT_LIST                             = 0x11,
    SCCPMSG_SERVER_REQ                                  = 0x12,
    SCCPMSG_ALARM                                       = 0x20,
    SCCPMSG_MULTICAST_MEDIA_RECEPTION_ACK               = 0x21,
    SCCPMSG_OPEN_RECEIVE_CHANNEL_ACK                    = 0x22,
    SCCPMSG_CONNECTION_STATISTICS_RES                   = 0x23,
    SCCPMSG_OFFHOOK_WITH_CGPN                           = 0x24,
    SCCPMSG_SOFTKEY_SET_REQ                             = 0x25,
    SCCPMSG_SOFTKEY_EVENT                               = 0x26,
    SCCPMSG_UNREGISTER                                  = 0x27,
    SCCPMSG_SOFTKEY_TEMPLATE_REQ                        = 0x28,
    SCCPMSG_REGISTER_TOKEN_REQ                          = 0x29,
    SCCPMSG_MEDIA_TRANSMISSION_FAILURE                  = 0x2a,
    SCCPMSG_HEADSET_STATUS                              = 0x2b,
    SCCPMSG_MEDIA_RESOURCE_NOTIFICATION                 = 0x2c,
    SCCPMSG_REGISTER_AVAILABLE_LINES                    = 0x2d,
    SCCPMSG_DEVICE_TO_USER_DATA                         = 0x2e,
    SCCPMSG_DEVICE_TO_USER_DATA_RESPONSE                = 0x2f,
    SCCPMSG_UPDATE_CAPABILITIES                         = 0x30,
    SCCPMSG_OPEN_MULTIMEDIA_RECEIVE_CHANNEL_ACK         = 0x31,
    SCCPMSG_UPDATE_CLEAR_CONFERENCE                     = 0x32,
    SCCPMSG_SERVICE_URL_STAT_REQ                        = 0x33,
    SCCPMSG_FEATURE_STAT_REQ                            = 0x34,
    SCCPMSG_CREATE_CONFERENCE_RES                       = 0x35,
    SCCPMSG_DELETE_CONFERENCE_RES                       = 0x36,
    SCCPMSG_MODIFY_CONFERENCE_RES                       = 0x37,
    SCCPMSG_ADD_PARTICIPANT_RES                         = 0x38,
    SCCPMSG_AUDIT_CONFERENCE_RES                        = 0x39,
    SCCPMSG_AUDIT_PARTICIPANT_RES                       = 0x40,
    SCCPMSG_DEVICE_TO_USER_DATA_VERSION1                = 0x41,
    SCCPMSG_DEVICE_TO_USER_DATA_RESPONSE_VERSION1       = 0x42,

    /*-------------- From the CM --------------*/    
    SCCPMSG_REGISTER_ACK                                = 0x81,
    SCCPMSG_START_TONE                                  = 0x82,
    SCCPMSG_STOP_TONE                                   = 0x83,
    SCCPMSG_SET_RINGER                                  = 0x85,
    SCCPMSG_SET_LAMP                                    = 0x86,
    //SCCPMSG_SET_HKF_DETECT                              = 0x87,
    SCCPMSG_SET_SPEAKER_MODE                            = 0x88,
    SCCPMSG_SET_MICRO_MODE                              = 0x89,
    SCCPMSG_START_MEDIA_TRANSMISSION                    = 0x8a,
    SCCPMSG_STOP_MEDIA_TRANSMISSION                     = 0x8b,
    //SCCPMSG_START_MEDIA_RECEPTION                       = 0x8c,
    //SCCPMSG_STOP_MEDIA_RECEPTION                        = 0x8d,
    //SCCPMSG_RESERVED_FOR_FUTURE_USE                     = 0x8e,
    SCCPMSG_CALL_INFO                                   = 0x8f,
    SCCPMSG_FORWARD_STAT                                = 0x90,
    SCCPMSG_SPEEDDIAL_STAT                              = 0x91,
    SCCPMSG_LINE_STAT                                   = 0x92,
    SCCPMSG_CONFIG_STAT                                 = 0x93,
    SCCPMSG_DEFINE_TIME_DATE                            = 0x94,
    SCCPMSG_START_SESSION_TRANSMISSION                  = 0x95,
    SCCPMSG_STOP_SESSION_TRANSMISSION                   = 0x96,
    SCCPMSG_BUTTON_TEMPLATE                             = 0x97,
    SCCPMSG_VERSION                                     = 0x98,
    SCCPMSG_DISPLAY_TEXT                                = 0x99,
    SCCPMSG_CLEAR_DISPLAY                               = 0x9a,
    SCCPMSG_CAPABILITIES_REQ                            = 0x9b,
    //SCCPMSG_ENUNCIATOR_COMMAND                          = 0x9c,
    SCCPMSG_REGISTER_REJECT                             = 0x9d,
    SCCPMSG_SERVER_RES                                  = 0x9e,
    SCCPMSG_RESET                                       = 0x9f,
    SCCPMSG_KEEPALIVE_ACK                               = 0x100,
    SCCPMSG_START_MULTICAST_MEDIA_RECEPTION             = 0x101,
    SCCPMSG_START_MULTICAST_MEDIA_TRANSMISSION          = 0x102,
    SCCPMSG_STOP_MULTICAST_MEDIA_RECEPTION              = 0x103,
    SCCPMSG_STOP_MULTICAST_MEDIA_TRANSMISSION           = 0x104,
    SCCPMSG_OPEN_RECEIVE_CHANNEL                        = 0x105,
    SCCPMSG_CLOSE_RECEIVE_CHANNEL                       = 0x106,
    SCCPMSG_CONNECTION_STATISTICS_REQ                   = 0x107,
    SCCPMSG_SOFTKEY_TEMPLATE_RES                        = 0x108,
    SCCPMSG_SOFTKEY_SET_RES                             = 0x109,
    SCCPMSG_SELECT_SOFTKEYS                             = 0x110,
    SCCPMSG_CALL_STATE                                  = 0x111,
    SCCPMSG_DISPLAY_PROMPT_STATUS                       = 0x112,
    SCCPMSG_CLEAR_PROMPT_STATUS                         = 0x113,
    SCCPMSG_DISPLAY_NOTIFY                              = 0x114,
    SCCPMSG_CLEAR_NOTIFY                                = 0x115,
    SCCPMSG_ACTIVATE_CALL_PLANE                         = 0x116,
    SCCPMSG_DEACTIVATE_CALL_PLANE                       = 0x117,
    SCCPMSG_UNREGISTER_ACK                              = 0x118,
    SCCPMSG_BACKSPACE_REQ                               = 0x119,
    SCCPMSG_REGISTER_TOKEN_ACK                          = 0x11a,
    SCCPMSG_REGISTER_TOKEN_REJECT                       = 0x11b,
    SCCPMSG_START_MEDIA_FAILURE_DETECTION               = 0x11c,
    SCCPMSG_DIALED_NUMBER                               = 0x11d,
    SCCPMSG_USER_TO_DEVICE_DATA                         = 0x11e,
    SCCPMSG_FEATURE_STAT                                = 0x11f,
    SCCPMSG_DISPLAY_PRIORITY_NOTIFY                     = 0x120,
    SCCPMSG_CLEAR_PRIORITY_NOTIFY                       = 0x121,
    SCCPMSG_START_ANNOUNCEMENT                          = 0x122,
    SCCPMSG_STOP_ANNOUNCEMENT                           = 0x123,
    SCCPMSG_ANNOUNCEMENT_FINISH                         = 0x124,
    SCCPMSG_RECORD_INFO                                 = 0x125,
    SCCPMSG_RECORDER_ONHOOK                             = 0x126,
    SCCPMSG_NOTIFY_DTMF_TONE                            = 0x127,
    SCCPMSG_SEND_DTMF_TONE                              = 0x128,
    SCCPMSG_SUBSCRIBE_DTMF_PAYLOAD_REQ                  = 0x129,
    SCCPMSG_SUBSCRIBE_DTMF_PAYLOAD_RES                  = 0x12a,
    SCCPMSG_SUBSCRIBE_DTMF_PAYLOAD_ERR                  = 0x12b,
    SCCPMSG_UNSUBSCRIBE_DTMF_PAYLOAD_REQ                = 0x12c,
    SCCPMSG_UNSUBSCRIBE_DTMF_PAYLOAD_RES                = 0x12d,
    SCCPMSG_UNSUBSCRIBE_DTMF_PAYLOAD_ERR                = 0x12e,
    SCCPMSG_SERVICE_URL_STAT                            = 0x12f,
    SCCPMSG_CALL_SELECT_STAT                            = 0x130,
    SCCPMSG_OPEN_MULTI_MEDIA_CHANNEL                    = 0x131,
    SCCPMSG_START_MULTI_MEDIA_CHANNEL                   = 0x132,
    SCCPMSG_STOP_MULTI_MEDIA_CHANNEL                    = 0x133,
    SCCPMSG_MISCELLANEOUS_COMMAND                       = 0x134,
    SCCPMSG_FLOW_CONTROL_COMMAND                        = 0x135,
    SCCPMSG_CLOSE_MULTI_MEDIA_RECEIVE_CHANNEL           = 0x136,
    SCCPMSG_CREATE_CONFERENCE_REQ                       = 0x137,
    SCCPMSG_DELETE_CONFERENCE_REQ                       = 0x138,
    SCCPMSG_MODIFY_CONFERENCE_REQ                       = 0x139,
    SCCPMSG_ADD_PARTICIPANT_REQ                         = 0x13a,
    SCCPMSG_DROP_PARTICIPANT_REQ                        = 0x13b,
    SCCPMSG_AUDIT_CONFERENCE_REQ                        = 0x13c,
    SCCPMSG_AUDIT_PARTICIAPANT_REQ                      = 0x13d,
    SCCPMSG_USER_TO_DEVICE_DATA_VERSION1                = 0x13f,
    SCCPMSG_INVALID                                     = 0xffff
} sccpmsg_messages_t;

typedef enum sccpmsg_versions_t_ {
    SCCPMSG_VERSION_MIN = 0,
    SCCPMSG_VERSION_SP30,
    SCCPMSG_VERSION_BRAVO,
    SCCPMSG_VERSION_HAWKBILL,
    SCCPMSG_VERSION_SEAVIEW,
    SCCPMSG_VERSION_PARCHE,
    SCCPMSG_VERSION_MAX,
} sccpmsg_versions_t;

typedef enum sccpmsg_device_type_t_ {
    SCCPMSG_DEVICE_TYPE_STATION_30SPPLUS             = 1,
    SCCPMSG_DEVICE_TYPE_STATION_12SPPLUS             = 2,
    SCCPMSG_DEVICE_TYPE_STATION_12SP                 = 3,
    SCCPMSG_DEVICE_TYPE_STATION_12                   = 4,
    SCCPMSG_DEVICE_TYPE_STATION_30VIP                = 5,
    SCCPMSG_DEVICE_TYPE_STATION_TELECASTER           = 6,
    SCCPMSG_DEVICE_TYPE_STATION_TELECASTER_MGR       = 7,
    SCCPMSG_DEVICE_TYPE_STATION_TELECASTER_BUS       = 8,
    SCCPMSG_DEVICE_TYPE_STATION_POLYCOM              = 9,
    SCCPMSG_DEVICE_TYPE_VIRTUAL_130SPPLUS            = 20,
    SCCPMSG_DEVICE_TYPE_STATION_PHONE_APPLICATION    = 21,
    SCCPMSG_DEVICE_TYPE_ANALOG_ACCESS                = 30,
    SCCPMSG_DEVICE_TYPE_DIGITAL_ACCESS_TITAN1        = 40,
    SCCPMSG_DEVICE_TYPE_DIGITAL_ACCESS_TITAN2        = 42,
    SCCPMSG_DEVICE_TYPE_DIGITAL_ACCESS_LENNON        = 43,
    SCCPMSG_DEVICE_TYPE_ANALOG_ACCESS_ELVIS          = 47,
    SCCPMSG_DEVICE_TYPE_CONFERENCE_BRIDGE            = 50,
    SCCPMSG_DEVICE_TYPE_CONFERENCE_BRIDGE_YOKO       = 51,
    SCCPMSG_DEVICE_TYPE_H225                         = 60,
    SCCPMSG_DEVICE_TYPE_H323_PHONE                   = 61,
    SCCPMSG_DEVICE_TYPE_H323_TRUNK                   = 62,
    SCCPMSG_DEVICE_TYPE_MUSIC_ON_HOLD                = 70,
    SCCPMSG_DEVICE_TYPE_PILOT                        = 71,
    SCCPMSG_DEVICE_TYPE_TAPI_PORT                    = 72,
    SCCPMSG_DEVICE_TYPE_TAPI_ROUTE_POINT             = 73,
    SCCPMSG_DEVICE_TYPE_VOICE_INBOX                  = 80,
    SCCPMSG_DEVICE_TYPE_VOICE_INBOX_ADMIN            = 81,
    SCCPMSG_DEVICE_TYPE_LINE_ANNUNCIATOR             = 82,
    SCCPMSG_DEVICE_TYPE_SOFTWARE_MTP_DIXIELAND       = 83,
    SCCPMSG_DEVICE_TYPE_CISCO_MEDIA_SERVER           = 84,
    SCCPMSG_DEVICE_TYPE_ROUTE_LIST                   = 90,
    SCCPMSG_DEVICE_TYPE_LOAD_SIMULATOR               = 100,
    SCCPMSG_DEVICE_TYPE_IPSTE1                       = 107,
    SCCPMSG_DEVICE_TYPE_MEDIA_TERMINATION_POINT      = 110,
    SCCPMSG_DEVICE_TYPE_MEDIA_TERMINATION_POINT_YOKO = 111,
    SCCPMSG_DEVICE_TYPE_MEDIA_TERMINATION_POINT_DIXIELAND = 112,
    SCCPMSG_DEVICE_TYPE_MEDIA_TERMINATION_POINT_SUMMIT = 113,
    SCCPMSG_DEVICE_TYPE_MGCP_STATION                 = 120,
    SCCPMSG_DEVICE_TYPE_MGCP_TRUNK                   = 121,
    SCCPMSG_DEVICE_TYPE_RAS_PROXY                    = 122,
    //SCCPMSG_DEVICE_TYPE_H323_ANONYMOUS_GATEWAY       = 123,
    SCCPMSG_DEVICE_TYPE_TRUNK                        = 125,
    SCCPMSG_DEVICE_TYPE_ANNUNCIATOR                  = 126,
    SCCPMSG_DEVICE_TYPE_MONITOR_BRIDGE               = 127,
    SCCPMSG_DEVICE_TYPE_RECORDER                     = 128,
    SCCPMSG_DEVICE_TYPE_MONITOR_BRIDGE_YOKO          = 129,
    SCCPMSG_DEVICE_TYPE_UNKNOWN_MGCP_GATEWAY         = 254,
    SCCPMSG_DEVICE_TYPE_IPSTE2                       = 30035,
    SCCPMSG_DEVICE_TYPE_INVALID
} sccpmsg_device_type_t;

typedef enum sccpmsg_tone_t_ {
    SCCPMSG_TONE_SILENCE              = 0x00,
    SCCPMSG_TONE_DTMF_1               = 0x01,
    SCCPMSG_TONE_DTMF_2               = 0x02,
    SCCPMSG_TONE_DTMF_3               = 0x03,
    SCCPMSG_TONE_DTMF_4               = 0x04,
    SCCPMSG_TONE_DTMF_5               = 0x05,
    SCCPMSG_TONE_DMTF_6               = 0x06,
    SCCPMSG_TONE_DTMF_7               = 0x07,
    SCCPMSG_TONE_DTMF_8               = 0x08,
    SCCPMSG_TONE_DTMF_9               = 0x09,
    SCCPMSG_TONE_DTMF_0               = 0x0a,
    SCCPMSG_TONE_DTMF_STAR            = 0x0e,
    SCCPMSG_TONE_DTMF_POUND           = 0x0f,
    SCCPMSG_TONE_DTMF_A               = 0x10,
    SCCPMSG_TONE_DTMF_B               = 0x11,
    SCCPMSG_TONE_DTMF_C               = 0x12,
    SCCPMSG_TONE_DTMF_D               = 0x13,
    SCCPMSG_TONE_INSIDE_DIAL          = 0x21,
    SCCPMSG_TONE_OUTSIDE_DIAL         = 0x22,
    SCCPMSG_TONE_LINE_BUSY            = 0x23,
    SCCPMSG_TONE_ALERTING             = 0x24,
    SCCPMSG_TONE_REORDER              = 0x25,
    SCCPMSG_TONE_RECORDER_WARNING     = 0x26,
    SCCPMSG_TONE_RECORDER_DETECTED    = 0x27,
    SCCPMSG_TONE_REVERTING            = 0x28,
    SCCPMSG_TONE_RECEIVER_OFFHOOK     = 0x29,
    SCCPMSG_TONE_PARTIAL_DIAL         = 0x2A,
    SCCPMSG_TONE_NO_SUCH_NUMBER       = 0x2B,
    SCCPMSG_TONE_BUSY_VERIFICATION    = 0x2C,
    SCCPMSG_TONE_CALL_WAITING         = 0x2D,
    SCCPMSG_TONE_CONFIRMATION         = 0x2E,
    SCCPMSG_TONE_CAMP_ON_INDICATION   = 0x2F,
    SCCPMSG_TONE_RECALL_DIAL          = 0x30,
    SCCPMSG_TONE_ZIP_ZIP              = 0x31,
    SCCPMSG_TONE_ZIP                  = 0x32,
    SCCPMSG_TONE_BEEP_BONK            = 0x33,
    SCCPMSG_TONE_MUSIC                = 0x34,
    SCCPMSG_TONE_HOLD                 = 0x35,
    SCCPMSG_TONE_TEST                 = 0x36,
    SCCPMSG_TONE_ADD_CALL_WAITING     = 0x40,
    SCCPMSG_TONE_PRIORITY_CALL_WAIT   = 0x41,
    SCCPMSG_TONE_RECALL               = 0x42,
    SCCPMSG_TONE_BARG_IN              = 0x43,
    SCCPMSG_TONE_DISTINCT_ALERT       = 0x44,
    SCCPMSG_TONE_PRIORITY_ALERT       = 0x45,
    SCCPMSG_TONE_REMINDER_RING        = 0x46,
    SCCPMSG_TONE_PRECEDENCE_RING_BACK = 0x47,
    SCCPMSG_TONE_PREEMPTION           = 0x48,
    SCCPMSG_TONE_MF1                  = 0x50,
    SCCPMSG_TONE_MF2                  = 0x51,
    SCCPMSG_TONE_MF3                  = 0x52,
    SCCPMSG_TONE_MF4                  = 0x53,
    SCCPMSG_TONE_MF5                  = 0x54,
    SCCPMSG_TONE_MF6                  = 0x55,
    SCCPMSG_TONE_MF7                  = 0x56,
    SCCPMSG_TONE_MF8                  = 0x57,
    SCCPMSG_TONE_MF9                  = 0x58,
    SCCPMSG_TONE_MF0                  = 0x59,
    SCCPMSG_TONE_MFKP1                = 0x5a,
    SCCPMSG_TONE_MFST                 = 0x5b,
    SCCPMSG_TONE_MFKP2                = 0x5c,
    SCCPMSG_TONE_MFSTP                = 0x5d,
    SCCPMSG_TONE_MFST3P               = 0x5e,
    SCCPMSG_TONE_MILLIWATT            = 0x5f,
    SCCPMSG_TONE_MILLIWATT_TEST       = 0x60,
    SCCPMSG_TONE_HIGH                 = 0x61,
    SCCPMSG_TONE_FLASH_OVERRIDE       = 0x62,
    SCCPMSG_TONE_FLASH                = 0x63,
    SCCPMSG_TONE_PRIORITY             = 0x64,
    SCCPMSG_TONE_IMMEDIATE            = 0x65,
    SCCPMSG_TONE_PREAMP_WARN          = 0x66,
    SCCPMSG_TONE_2105HZ               = 0x67,
    SCCPMSG_TONE_2600HZ               = 0x68,
    SCCPMSG_TONE_440HZ                = 0x69,
    SCCPMSG_TONE_300HZ                = 0x6a,
    SCCPMSG_MLPP_PALA                 = 0x77,
    SCCPMSG_MLPP_ICA                  = 0x78,
    SCCPMSG_MLPP_VCA                  = 0x79,
    SCCPMSG_MLPP_BPA                  = 0x7a,
    SCCPMSG_MLPP_BNEA                 = 0x7b,
    SCCPMSG_MLPP_UPA                  = 0x7c,
    SCCPMSG_TONE_NONE                 = 0x7f,
    SCCPMSG_TONE_INVALID
} sccpmsg_tone_t;

typedef enum sccpmsg_device_stimulus_t_ {
    SCCPMSG_DEVICE_STIMULUS_LAST_NUMBER_REDIAL          = 0x01,
    SCCPMSG_DEVICE_STIMULUS_SPEED_DIAL                  = 0x02,
    SCCPMSG_DEVICE_STIMULUS_HOLD                        = 0x03,
    SCCPMSG_DEVICE_STIMULUS_TRANSFER                    = 0x04,
    SCCPMSG_DEVICE_STIMULUS_FORWARD_ALL                 = 0x05,
    SCCPMSG_DEVICE_STIMULUS_FORWARD_BUSY                = 0x06,
    SCCPMSG_DEVICE_STIMULUS_FORWARD_NO_ANSWER           = 0x07,
    SCCPMSG_DEVICE_STIMULUS_DISPLAY                     = 0x08,
    SCCPMSG_DEVICE_STIMULUS_LINE                        = 0x09,
    SCCPMSG_DEVICE_STIMULUS_T120_CHAT                   = 0x0a,
    SCCPMSG_DEVICE_STIMULUS_T120_WHITEBOARD             = 0x0b,
    SCCPMSG_DEVICE_STIMULUS_T120_APPLICATION_SHARING    = 0x0c,
    SCCPMSG_DEVICE_STIMULUS_T120_FILE_TRANSFER          = 0x0d,
    SCCPMSG_DEVICE_STIMULUS_VIDEO                       = 0x0e,
    SCCPMSG_DEVICE_STIMULUS_VOICEMAIL                   = 0x0f,
    SCCPMSG_DEVICE_STIMULUS_ANSWER_RELEASE              = 0x10,
    SCCPMSG_DEVICE_STIMULUS_AUTO_ANSWER                 = 0x11,
    SCCPMSG_DEVICE_STIMULUS_SELECT                      = 0x12,
    SCCPMSG_DEVICE_STIMULUS_PRIVACY                     = 0x13,
    SCCPMSG_DEVICE_STIMULUS_SERVICE_URL                 = 0x14,
    SCCPMSG_DEVICE_STIMULUS_MALICIOUS_CALL              = 0x1b,
    SCCPMSG_DEVICE_STIMULUS_GENERIC_APP_B1              = 0x21,
    SCCPMSG_DEVICE_STIMULUS_GENERIC_APP_B2              = 0x22,
    SCCPMSG_DEVICE_STIMULUS_GENERIC_APP_B3              = 0x23,
    SCCPMSG_DEVICE_STIMULUS_GENERIC_APP_B4              = 0x24,
    SCCPMSG_DEVICE_STIMULUS_GENERIC_APP_B5              = 0x25,
    //SCCPMSG_DEVICE_STIMULUS_MEET_ME_CONFERENCE          = 0x7b,
    //SCCPMSG_DEVICE_STIMULUS_CONFERENCE                  = 0x7d,
    //SCCPMSG_DEVICE_STIMULUS_CALL_PARK                   = 0x7e,
    //SCCPMSG_DEVICE_STIMULUS_CALL_PICKUP                 = 0x7f,
    //SCCPMSG_DEVICE_STIMULUS_GROUP_CALL_PICKUP           = 0x80,
    SCCPMSG_DEVICE_STIMULUS_INVALID
} sccpmsg_device_stimulus_t;

typedef enum sccpmsg_alarm_severity_t_ {
    SCCPMSG_ALARM_SEVERITY_CRITICAL      = 0,
    SCCPMSG_ALARM_SEVERITY_WARNING       = 1,
    SCCPMSG_ALARM_SEVERITY_INFORMATIONAL = 2,
    SCCPMSG_ALARM_SEVERITY_UNKNOWN       = 4,
    SCCPMSG_ALARM_SEVERITY_MAJOR         = 7,
    SCCPMSG_ALARM_SEVERITY_MINOR         = 8,
    SCCPMSG_ALARM_SEVERITY_MARGINAL      = 10,
    SCCPMSG_ALARM_SEVERITY_TRACE_INFO    = 20,
    SCCPMSG_ALARM_SEVERITY_INVALID
} sccpmsg_alarm_severity_t;

typedef enum sccpmsg_device_application_t_ {
    SCCPMSG_DEVICE_APPLICATION_ANALOG_CO_TRUNK              = 1,
    SCCPMSG_DEVICE_APPLICATION_DIGITAL_TO_ANALOG            = 2,
    SCCPMSG_DEVICE_APPLICATION_ANALOG_TIE_TRUNK             = 3,
    SCCPMSG_DEVICE_APPLICATION_DIGITAL_TO_DIGITAL_CO        = 4,
    SCCPMSG_DEVICE_APPLICATION_ISDN_STATION                 = 5,
    SCCPMSG_DEVICE_APPLICATION_ISDN_DIGITAL_TIE_TRUNK       = 6,
    SCCPMSG_DEVICE_APPLICATION_ISDN_TRUNK                   = 7,
    SCCPMSG_DEVICE_APPLICATION_ON_PREMISE_POTS_LINE         = 8,
    SCCPMSG_DEVICE_APPLICATION_OFF_PREMISE_POTS_LINE        = 9,
    SCCPMSG_DEVICE_APPLICATION_SATELLITE_ANANLOG_TIE_TRUNK  = 10,
    SCCPMSG_DEVICE_APPLICATION_SATELLITE_DIGITAL_TIE_TRUNK  = 11,
    SCCPMSG_DEVICE_APPLICATION_ANANLOG_TO_LL_TRUNK          = 12,
    SCCPMSG_DEVICE_APPLICATION_MAX                          = 12
} sccpmsg_device_application_t;

typedef enum sccpmsg_reset_type_t_ {
    SCCPMSG_RESET_TYPE_RESET   = 1,
    SCCPMSG_RESET_TYPE_RESTART = 2,
    SCCPMSG_RESET_TYPE_INVALID
} sccpmsg_reset_type_t;

typedef enum sccpmsg_unregister_status_t_ {
    SCCPMSG_UNREGISTER_STATUS_OK      =  0,
    SCCPMSG_UNREGISTER_STATUS_ERROR   =  1,
    SCCPMSG_UNREGISTER_STATUS_NAK     =  2,
    SCCPMSG_UNREGISTER_STATUS_INVALID
} sccpmsg_unregister_status_t;

typedef enum sccpmsg_alarms_t_ {
    SCCPMSG_ALARM_LAST_MIN = -1,
	SCCPMSG_ALARM_LOAD_REJECTED,
	SCCPMSG_ALARM_TFTP_SIZE_ERROR,
	SCCPMSG_ALARM_COMPRESSOR_ERROR,
	SCCPMSG_ALARM_VERSION_ERROR,
	SCCPMSG_ALARM_DISKFULL_ERROR,
	SCCPMSG_ALARM_CHECKSUM_ERROR,
	SCCPMSG_ALARM_FILE_NOT_FOUND,
	SCCPMSG_ALARM_TFTP_TIMEOUT,
	SCCPMSG_ALARM_TFTP_ACCESS_ERROR,
	SCCPMSG_ALARM_TFTP_ERROR,
	SCCPMSG_ALARM_LAST_TCP_TIMEOUT,
	SCCPMSG_ALARM_LAST_TCP_BAD_ACK,
	SCCPMSG_ALARM_LAST_CM_RESET_TCP,
    SCCPMSG_ALARM_LAST_CM_ABORT_TCP,
	SCCPMSG_ALARM_LAST_TCP_CLOSED, /* 14 */
    SCCPMSG_ALARM_LAST_ICMP,
    SCCPMSG_ALARM_LAST_CM_NAKED,
    SCCPMSG_ALARM_LAST_KA_TO,
    SCCPMSG_ALARM_LAST_FAILBACK,
    SCCPMSG_ALARM_LAST_DIAG,
    SCCPMSG_ALARM_LAST_KEYPAD,
    SCCPMSG_ALARM_LAST_RE_IP,
    SCCPMSG_ALARM_LAST_RESET,
    SCCPMSG_ALARM_LAST_RESTART,
    SCCPMSG_ALARM_LAST_REG_REJ,
    SCCPMSG_ALARM_LAST_INITIALIZED,
	SCCPMSG_ALARM_LAST_0X2X,
	SCCPMSG_ALARM_WAITING_FORS_FROMS,
	SCCPMSG_ALARM_WAITING_FOR_STATED_RESPONSE_FROMS,
	SCCPMSG_ALARM_WAITING_FORS_DSP_ALARMS,
	SCCPMSG_ALARM_LAST_PHONE_ABORT,
	SCCPMSG_ALARM_FILE_AUTH_FAIL,
    SCCPMSG_ALARM_LAST_KEYPAD_CLOSE,
    SCCPMSG_ALARM_LAST_KEYPAD_RESTART,
    SCCPMSG_ALARM_LAST_KEYPAD_RESET,
    SCCPMSG_ALARM_LAST_MAX
} sccpmsg_alarms_t;

typedef enum sccpmsg_media_payload_type_t_ {
    //SCCPMSG_MEDIA_PAYLOAD_NON_STANDARD              = 1,
    SCCPMSG_MEDIA_PAYLOAD_G711_ALAW_64K             = 2,
    SCCPMSG_MEDIA_PAYLOAD_G711_ALAW_56K             = 3,
    SCCPMSG_MEDIA_PAYLOAD_G711_ULAW_64K             = 4,
    SCCPMSG_MEDIA_PAYLOAD_G711_ULAW_56K             = 5,
    SCCPMSG_MEDIA_PAYLOAD_G722_64K                  = 6,
    SCCPMSG_MEDIA_PAYLOAD_G722_56K                  = 7,
    SCCPMSG_MEDIA_PAYLOAD_G722_48K                  = 8,
    SCCPMSG_MEDIA_PAYLOAD_G7231                     = 9,
    SCCPMSG_MEDIA_PAYLOAD_G728                      = 10,
    SCCPMSG_MEDIA_PAYLOAD_G729                      = 11,
    SCCPMSG_MEDIA_PAYLOAD_G729_ANNEX_A              = 12,
    //SCCPMSG_MEDIA_PAYLOAD_IS11172_AUDIO_CAP         = 13,
    //SCCPMSG_MEDIA_PAYLOAD_IS13818_AUDIO_CAP         = 14,
    SCCPMSG_MEDIA_PAYLOAD_G729_ANNEX_B              = 15,
    SCCPMSG_MEDIA_PAYLOAD_G729_ANNEX_AW_ANNEX_B     = 16,
    SCCPMSG_MEDIA_PAYLOAD_GSM_FULL_RATE             = 18,
    SCCPMSG_MEDIA_PAYLOAD_GSM_HALF_RATE             = 19,
    SCCPMSG_MEDIA_PAYLOAD_GSM_ENHANCED_FULL_RATE    = 20,
    SCCPMSG_MEDIA_PAYLOAD_WIDE_BAND_256K            = 25,
    SCCPMSG_MEDIA_PAYLOAD_DATA_64                   = 32,
    SCCPMSG_MEDIA_PAYLOAD_DATA_56                   = 33,
    SCCPMSG_MEDIA_PAYLOAD_GSM                       = 80,
    //SCCPMSG_MEDIA_PAYLOAD_ACTIVE_VOICE              = 81,
    SCCPMSG_MEDIA_PAYLOAD_G726_32K                  = 82,
    SCCPMSG_MEDIA_PAYLOAD_G726_24K                  = 83,
    SCCPMSG_MEDIA_PAYLOAD_G726_16K                  = 84,
    //SCCPMSG_MEDIA_PAYLOAD_G729_B                    = 85,
    //SCCPMSG_MEDIA_PAYLOAD_G729_B_LOW_COMPLEXITY     = 86,
    SCCPMSG_MEDIA_PAYLOAD_H261                      = 100,
    SCCPMSG_MEDIA_PAYLOAD_H263                      = 101,
    SCCPMSG_MEDIA_PAYLOAD_T120                      = 105,
    SCCPMSG_MEDIA_PAYLOAD_H224                      = 106,
    SCCPMSG_MEDIA_PAYLOAD_XV150_MR                  = 111,
    SCCPMSG_MEDIA_PAYLOAD_INVALID,
    SCCPMSG_MEDIA_PAYLOAD_RFC2833_DYN_PAYLOAD       = 257,
} sccpmsg_media_payload_type_t;

typedef enum sccpmsg_media_silence_supression_t_ {
    SCCPMSG_MEDIA_SILENCE_SUPRESSION_OFF     = 0,
    SCCPMSG_MEDIA_SILENCE_SUPRESSION_ON      = 1,
    SCCPMSG_MEDIA_SILENCE_SUPRESSION_INVALID
} sccpmsg_media_silence_supression_t;

typedef enum sccpmsg_media_echo_cancellation_t_ {
    SCCPMSG_MEDIA_ECHO_CANCELLATION_OFF     = 0,
    SCCPMSG_MEDIA_ECHO_CANCELLATION_ON      = 1,
    SCCPMSG_MEDIA_ECHO_CANCELLATION_INVALID
} sccpmsg_media_echo_cancellation_t;

typedef enum sccpmsg_g723_bit_rate_t_ {
    SCCPMSG_G723_BIT_RATE_5_3     = 1,
    SCCPMSG_G723_BIT_RATE_6_4     = 2,
    SCCPMSG_G723_BIT_RATE_INVALID
} sccpmsg_g723_bit_rate_t;

typedef struct sccpmsg_media_qualifier_incoming_t_ {
    sccpmsg_media_echo_cancellation_t echo_cancellation;
    sccpmsg_g723_bit_rate_t           g723_bit_rate;
} sccpmsg_media_qualifier_incoming_t;

typedef struct sccpmsg_media_qualifier_outgoing_t_ {
    unsigned long                      precedence;
    sccpmsg_media_silence_supression_t silence_supression;
    unsigned short                     max_frames_per_packet;
    sccpmsg_g723_bit_rate_t            g723_bit_rate;
} sccpmsg_media_qualifier_outgoing_t;

typedef struct sccpmsg_media_capabilities_t_ {
    sccpmsg_media_payload_type_t payload_type;
    unsigned long                milliseconds_per_packet;
    union {
        char                    future_use[8];
        sccpmsg_g723_bit_rate_t g723_bit_rate;
    } payload_params;
} sccpmsg_media_capabilities_t;

typedef enum sccpmsg_media_encryption_algorithm_t_ {
    SCCPMSG_MEDIA_ENCRYPTION_ALGORITHM_MIN = -1,
    SCCPMSG_MEDIA_ENCRYPTION_ALGORITHM_NONE,
    SCCPMSG_MEDIA_ENCRYPTION_ALGORITHM_AES_128_COUNTER,
    SCCPMSG_MEDIA_ENCRYPTION_ALGORITHM_MAX
} sccpmsg_media_encryption_algorithm_t;

typedef struct sccpmsg_media_key_data_t_ {
    char key[SCCPMSG_KEY_SIZE];
    char salt[SCCPMSG_KEY_SIZE];
} sccpmsg_media_key_data_t;

typedef struct sccpmsg_media_encryption_key_t_ {
    sccpmsg_media_encryption_algorithm_t algorithm;
    unsigned short                       key_len;
    unsigned short                       salt_len;
    sccpmsg_media_key_data_t             key_data;
} sccpmsg_media_encryption_key_t;

typedef enum sccpmsg_sequence_flag_t_ {
    SCCPMSG_SEQUENCE_FLAG_FIRST,
    SCCPMSG_SEQUENCE_FLAG_MORE,
    SCCPMSG_SEQUENCE_FLAG_LAST
} sccpmsg_sequence_flag_t;

typedef enum sccpmsg_session_type_t_ {
    SCCPMSG_SESSION_TYPE_CHAT                = 0x01,
    SCCPMSG_SESSION_TYPE_WHITEBOARD          = 0x02,
    SCCPMSG_SESSION_TYPE_APPLICATION_SHARING = 0x04,
    SCCPMSG_SESSION_TYPE_FILE_TRANSFER       = 0x08,
    SCCPMSG_SESSION_TYPE_VIDEO               = 0x10,
    SCCPMSG_SESSION_TYPE_INVALID
} sccpmsg_session_type_t;

typedef enum sccpmsg_tone_direction_t_ {
    SCCPMSG_TONE_DIRECTION_USER    = 0,
    SCCPMSG_TONE_DIRECTION_NETWORK = 1,
    SCCPMSG_TONE_DIRECTION_ALL     = 2,
    SCCPMSG_TONE_DIRECTION_INVALID
} sccpmsg_tone_direction_t;

typedef struct sccpmsg_station_time_t_ {
    unsigned long year;
    unsigned long month;
    unsigned long day_of_week;
    unsigned long day;
    unsigned long hour;
    unsigned long minute;
    unsigned long second;
    unsigned long milliseconds;
} sccpmsg_station_time_t;

typedef enum sccpmsg_keypad_buttons_t_ {
    SCCPMSG_KEYPAD_BUTTON_ZERO    = 0x0,
    SCCPMSG_KEYPAD_BUTTON_ONE     = 0x1,
    SCCPMSG_KEYPAD_BUTTON_TWO     = 0x2,
    SCCPMSG_KEYPAD_BUTTON_THREE   = 0x3,
    SCCPMSG_KEYPAD_BUTTON_FOUR    = 0x4,
    SCCPMSG_KEYPAD_BUTTON_FIVE    = 0x5,
    SCCPMSG_KEYPAD_BUTTON_SIX     = 0x6,
    SCCPMSG_KEYPAD_BUTTON_SEVEN   = 0x7,
    SCCPMSG_KEYPAD_BUTTON_EIGHT   = 0x8,
    SCCPMSG_KEYPAD_BUTTON_NINE    = 0x9,
    SCCPMSG_KEYPAD_BUTTON_A       = 0xa,
    SCCPMSG_KEYPAD_BUTTON_B       = 0xb,
    SCCPMSG_KEYPAD_BUTTON_C       = 0xc,
    SCCPMSG_KEYPAD_BUTTON_D       = 0xd,
    SCCPMSG_KEYPAD_BUTTON_STAR    = 0xe,
    SCCPMSG_KEYPAD_BUTTON_POUND   = 0xf,
    SCCPMSG_KEYPAD_BUTTON_INVALID 
} sccpmsg_keypad_buttons_t;

typedef enum sccpmsg_ring_mode_t_ {
    SCCPMSG_RING_MODE_OFF        = 0x1,
    SCCPMSG_RING_MODE_INSIDE     = 0x2,
    SCCPMSG_RING_MODE_OUTSIDE    = 0x3,
    SCCPMSG_RING_MODE_FEATURE    = 0x4,
    SCCPMSG_RING_MODE_FLASH_ONLY = 0x5,
    SCCPMSG_RING_MODE_PRECEDENCE = 0x6,
    SCCPMSG_RING_MODE_INVALID
} sccpmsg_ring_mode_t;

typedef enum sccpmsg_ring_duration_t_ {
    SCCPMSG_RING_NORMAL  = 0x1,
    SCCPMSG_RING_SINGLE  = 0x2,
    SCCPMSG_RING_INVALID 
} sccpmsg_ring_duration_t;

typedef enum sccpmsg_lamp_mode_t_ {
    SCCPMSG_LAMP_OFF     = 0x1,      // Off (on 0%)
    SCCPMSG_LAMP_ON      = 0x2,      // On (on 100%)
    SCCPMSG_LAMP_WINK    = 0x3,      // Hold, Wink (on 80%) = 448 ms on / 64 ms off
    SCCPMSG_LAMP_FLASH   = 0x4,      // E-Mail, Flash (on 50%) = 32 ms on / 32 ms off
    SCCPMSG_LAMP_BLINK   = 0x5,      // Blink (on 50%) = 512 ms on / 512 ms off
    SCCPMSG_LAMP_INVALID 
} sccpmsg_lamp_mode_t;

typedef enum sccpmsg_hookflash_detection_mode_t_ {
    SCCPMSG_HOOKFLASH_ON    = 0x01,
    SCCPMSG_HOOKFLASH_OFF   = 0x02,
    SCCPMSG_HOOKFLASH_INVALID
} sccpmsg_hookflash_detection_mode_t;

typedef enum sccpmsg_speaker_mode_t_ {
    SCCPMSG_SPEAKER_MODE_ON      = 1,
    SCCPMSG_SPEAKER_MODE_OFF     = 2,
    SCCPMSG_SPEAKER_MODE_INVALID 
} sccpmsg_speaker_mode_t;

typedef enum sccpmsg_headset_statuss_t_ {
    SCCPMSG_HEADSET_STATUS_ON      = 1,
    SCCPMSG_HEADSET_STATUS_OFF     = 2,
    SCCPMSG_HEADSET_STATUS_INVALID
} sccpmsg_headset_statuss_t;

typedef enum sccpmsg_microphone_mode_t_ {
    SCCPMSG_MICROPHONE_MODE_ON      = 1,
    SCCPMSG_MICROPHONE_MODE_OFF     = 2,
    SCCPMSG_MICROPHONE_MODE_INVALID 
} sccpmsg_microphone_mode_t;

typedef struct sccpmsg_sid_t_ {
    char          device_name[SCCPMSG_DEVICE_NAME_SIZE];
    unsigned long reserved;
    unsigned long instance;
} sccpmsg_sid_t;

typedef struct sccpmsg_button_definition_t_ {
    unsigned char instance;
    unsigned char definition;
} sccpmsg_button_definition_t;

typedef struct sccpmsg_button_template_definition_t_ {
    unsigned long               button_offset;
    unsigned long               button_count;
    unsigned long               total_button_count;
    sccpmsg_button_definition_t buttons[SCCPMSG_MAX_BUTTON_TEMPLATE_SIZE];
} sccpmsg_button_template_definition_t;

typedef enum sccpmsg_media_enunciation_type_t_ {
    SCCPMSG_MEDIA_ENUNCIATION_TYPE_NONE,
    SCCPMSG_MEDIA_ENUNCIATION_TYPE_CALL_PARK,
    SCCPMSG_MEDIA_ENUNCIATION_TYPE_INVALID 
} sccpmsg_media_enunciation_type_t;

typedef enum sccpmsg_stats_processing_t_ {
    SCCPMSG_STATS_PROCESSING_CLEAR    = 0,
    SCCPMSG_STATS_PROCESSING_NO_CLEAR = 1,
    SCCPMSG_STATS_PROCESSING_INVALID
} sccpmsg_stats_processing_t;

typedef enum sccpmsg_softkey_events_t_ {
    SCCPMSG_SOFTKEY_EVENT_MIN,
    SCCPMSG_SOFTKEY_EVENT_REDIAL,
    SCCPMSG_SOFTKEY_EVENT_NEWCALL,
    SCCPMSG_SOFTKEY_EVENT_HOLD,
    SCCPMSG_SOFTKEY_EVENT_TRNSFER,
    SCCPMSG_SOFTKEY_EVENT_CFWDALL,
    SCCPMSG_SOFTKEY_EVENT_CFWDBUSY,
    SCCPMSG_SOFTKEY_EVENT_CFWDNOANSWER,
    SCCPMSG_SOFTKEY_EVENT_BACKSPACE,
    SCCPMSG_SOFTKEY_EVENT_ENDCALL,
    SCCPMSG_SOFTKEY_EVENT_RESUME,
    SCCPMSG_SOFTKEY_EVENT_ANSWER,
    SCCPMSG_SOFTKEY_EVENT_INFO,
    SCCPMSG_SOFTKEY_EVENT_CONFRN,
    SCCPMSG_SOFTKEY_EVENT_PARK,
    SCCPMSG_SOFTKEY_EVENT_JOIN,
    SCCPMSG_SOFTKEY_EVENT_MEETMECONFRN,
    SCCPMSG_SOFTKEY_EVENT_CALLPICKUP,
    SCCPMSG_SOFTKEY_EVENT_GRPCALLPICKUP,
    SCCPMSG_SOFTKEY_EVENT_DROPLASTCONFEE,
    SCCPMSG_SOFTKEY_EVENT_CALLBACK,
    SCCPMSG_SOFTKEY_EVENT_BARGE,
    SCCPMSG_SOFTKEY_EVENT_MAX
} sccpmsg_softkey_events_t;

typedef struct sccpmsg_softkey_template_definition_t_ {
    char          label[SCCPMSG_SOFTKEY_LABEL_SIZE];
    unsigned long event;
} sccpmsg_softkey_template_definition_t;

typedef struct sccpmsg_softkey_template_t_ {
    unsigned long            offset;
    unsigned long            count;
    unsigned long            total_count;
    sccpmsg_softkey_template_definition_t
                             definition[SCCPMSG_MAX_SOFTKEY_DEFINITIONS];
} sccpmsg_softkey_template_t;

typedef struct sccpmsg_softkey_set_definition_t_ {
    unsigned char  template_index[SCCPMSG_MAX_SOFTKEY_INDEXES];
    unsigned short info_index[SCCPMSG_MAX_SOFTKEY_INDEXES];
} sccpmsg_softkey_set_definition_t;

typedef struct sccpmsg_softkey_set_t_ {
    unsigned long       offset;
    unsigned long       count;
    unsigned long       total_count;
    sccpmsg_softkey_set_definition_t 
                        definition[SCCPMSG_MAX_SOFTKEY_SET_DEFINITIONS];
} sccpmsg_softkey_set_t;

typedef struct sccpmsg_user_and_device_data_t_ {
    unsigned long application_id;
    unsigned long line_number;
    unsigned long call_reference;
    unsigned long transaction_id;
    unsigned long data_length;
    char          data[SCCPMSG_USER_DEVICE_DATA_SIZE];
} sccpmsg_user_and_device_data_t;

typedef struct sccpmsg_user_and_device_data_version1_t_ {
    unsigned long application_id;
    unsigned long line_number;
    unsigned long call_reference;
    unsigned long transaction_id;
    unsigned long data_length;
    sccpmsg_sequence_flag_t sequence_flag;
    unsigned long display_priority;
    unsigned long conference_id;
    unsigned long app_instance_id;
    unsigned long routing_id;
    char          data[SCCPMSG_USER_DEVICE_DATA_SIZE];
} sccpmsg_user_and_device_data_version1_t;

typedef enum sccpmsg_display_options_t_ {
    SCCPMSG_DISPLAY_OPTIONS_MIN  = -1,
    SCCPMSG_DISPLAY_OPTIONS_ODN  = 0x1,
    SCCPMSG_DISPLAY_OPTIONS_RDN  = 0x2,
    SCCPMSG_DISPLAY_OPTIONS_CLID = 0x4,
    SCCPMSG_DISPLAY_OPTIONS_CNID = 0x8,
    SCCPMSG_DISPLAY_OPTIONS_MAX
} sccpmsg_display_options_t;

typedef enum sccpmsg_mlpp_precedence_t_ {
    SCCPMSG_MLPP_PRECEDENCE_MIN = -1,
    SCCPMSG_MLPP_PRECEDENCE_HIGHEST,
    SCCPMSG_MLPP_PRECEDENCE_FLASH,
    SCCPMSG_MLPP_PRECEDENCE_IMMEDIATE,
    SCCPMSG_MLPP_PRECEDENCE_PRIORITY,
    SCCPMSG_MLPP_PRECEDENCE_ROUTINE,
    SCCPMSG_MLPP_PRECEDENCE_LOWEST = SCCPMSG_MLPP_PRECEDENCE_ROUTINE,
    SCCPMSG_MLPP_PRECEDENCE_MAX
} sccpmsg_mlpp_precedence_t;

typedef struct sccpmsg_precedence_t_ {
    unsigned long level;
    unsigned long domain;
} sccpmsg_precedence_t;

typedef enum sccpmsg_call_states_t_ {
    SCCPMSG_CALL_STATE_IDLE                  = 0,
    SCCPMSG_CALL_STATE_OFFHOOK               = 1,
    SCCPMSG_CALL_STATE_ONHOOK                = 2,
    SCCPMSG_CALL_STATE_RING_OUT              = 3,
    SCCPMSG_CALL_STATE_RING_IN               = 4,
    SCCPMSG_CALL_STATE_CONNECTED             = 5,
    SCCPMSG_CALL_STATE_BUSY                  = 6,
    SCCPMSG_CALL_STATE_CONGESTION            = 7,
    SCCPMSG_CALL_STATE_HOLD                  = 8,
    SCCPMSG_CALL_STATE_CALL_WAITING          = 9,
    SCCPMSG_CALL_STATE_CALL_TRANSFER         = 10,
    SCCPMSG_CALL_STATE_CALL_PARK             = 11,
    SCCPMSG_CALL_STATE_PROCEED               = 12,
    SCCPMSG_CALL_STATE_CALL_REMOTE_MULTILINE = 13,
    SCCPMSG_CALL_STATE_INVALID_NUMBER        = 14,
    SCCPMSG_CALL_STATE_INVALID
} sccpmsg_call_states_t;

typedef enum sccpmsg_call_types_t_ {
    SCCPMSG_CALL_TYPE_MIN,
    SCCPMSG_CALL_TYPE_INBOUND,
    SCCPMSG_CALL_TYPE_OUTBOUND,
    SCCPMSG_CALL_TYPE_FORWARD,
    SCCPMSG_CALL_TYPE_INVALID
} sccpmsg_call_types_t;

typedef enum sccpmsg_security_status_t_ {
    SCCPMSG_SECURITY_STATUS_MIN = -1,
    SCCPMSG_SECURITY_STATUS_UNKNOWN,
    SCCPMSG_SECURITY_STATUS_NOT_AUTHENTICATED,
    SCCPMSG_SECURITY_STATUS_AUTHENTICATED,
    SCCPMSG_SECURITY_STATUS_MAX
} sccpmsg_security_status_t;

typedef enum sccpmsg_restrict_info_t_ {
    SCCPMSG_RESTRICT_INFO_MIN      = -1,
    SCCPMSG_RESTRICT_INFO_CGPN     = 0x00000001,
    SCCPMSG_RESTRICT_INFO_CGPD     = 0x00000002,
    SCCPMSG_RESTRICT_INFO_CGP      = SCCPMSG_RESTRICT_INFO_CGPN | SCCPMSG_RESTRICT_INFO_CGPD,
    SCCPMSG_RESTRICT_INFO_CDPN     = 0x00000004,
    SCCPMSG_RESTRICT_INFO_CDPD     = 0x00000008,
    SCCPMSG_RESTRICT_INFO_CDP      = SCCPMSG_RESTRICT_INFO_CDPN | SCCPMSG_RESTRICT_INFO_CDPD,
    SCCPMSG_RESTRICT_INFO_OCGPN    = 0x00000010,
    SCCPMSG_RESTRICT_INFO_OCGPD    = 0x00000020,
    SCCPMSG_RESTRICT_INFO_OCGP     = SCCPMSG_RESTRICT_INFO_OCGPN | SCCPMSG_RESTRICT_INFO_OCGPD,
    SCCPMSG_RESTRICT_INFO_LCGPN    = 0x00000040,
    SCCPMSG_RESTRICT_INFO_LCGPD    = 0x00000080,
    SCCPMSG_RESTRICT_INFO_LCGP     = SCCPMSG_RESTRICT_INFO_LCGPN | SCCPMSG_RESTRICT_INFO_LCGPD,
    SCCPMSG_RESTRICT_INFO_RESERVED = ~(0x000000FF),
    SCCPMSG_RESTRICT_INFO_MAX
} sccpmsg_restrict_info_t;

/*-------------- To the CM --------------*/
typedef struct sccpmsg_register_token_req_t_ {
    unsigned long         message_id;
    sccpmsg_sid_t         sid;
    unsigned long         ip_address;
    sccpmsg_device_type_t device_type;
} sccpmsg_register_token_req_t;

typedef struct sccpmsg_register_t_ {
    unsigned long         message_id;
    sccpmsg_sid_t         sid;
    unsigned long         ip_address;
    sccpmsg_device_type_t device_type;
    unsigned long         max_streams;
    unsigned long         active_streams;
    unsigned long         protocol_version;
    unsigned long         max_conferences;
    unsigned long         active_conferences;
} sccpmsg_register_t;

typedef struct sccpmsg_unregister_t_ {
    unsigned long message_id;
} sccpmsg_unregister_t;

typedef struct sccpmsg_ip_port_t_ {
    unsigned long message_id;
    unsigned long port;
} sccpmsg_ip_port_t;

typedef struct sccpmsg_media_resource_notification_t_ {
    unsigned long         message_id;
    sccpmsg_device_type_t type;
    unsigned long         number_in_service_streams;
    unsigned long         max_streams_per_conference;
    unsigned long         number_out_of_service_streams;
} sccpmsg_media_resource_notification_t;

typedef struct sccpmsg_keypad_button_t_ {
    unsigned long            message_id;
    sccpmsg_keypad_buttons_t button;
    unsigned long            line_number;
    unsigned long            call_reference;
} sccpmsg_keypad_button_t;

typedef struct sccpmsg_enbloc_call_t_ {
    unsigned long message_id;
    char          called_party[SCCPMSG_DIRECTORY_NUMBER_SIZE];
    unsigned long line_number;
} sccpmsg_enbloc_call_t;

typedef struct sccpmsg_stimulus_t_ {
    unsigned long             message_id;
    sccpmsg_device_stimulus_t type;
    unsigned long             instance;
    unsigned long             call_reference;
} sccpmsg_stimulus_t;

typedef struct sccpmsg_offhook_t_ {
    unsigned long message_id;
    unsigned long line_number;
    unsigned long call_reference;
} sccpmsg_offhook_t;

typedef struct sccpmsg_onhook_t_ {
    unsigned long message_id;
    unsigned long line_number;
    unsigned long call_reference;
} sccpmsg_onhook_t;

typedef struct sccpmsg_hook_flash_t_ {
    unsigned long message_id;
    unsigned long line_number;
    unsigned long call_reference;
} sccpmsg_hook_flash_t;

typedef struct sccpmsg_forward_stat_req_t_ {
    unsigned long message_id;
    unsigned long line_number;
} sccpmsg_forward_stat_req_t;

typedef struct sccpmsg_speeddial_stat_req_t_ {
    unsigned long message_id;
    unsigned long speeddial_number;
} sccpmsg_speeddial_stat_req_t;

typedef struct sccpmsg_service_url_stat_req_t_ {
    unsigned long message_id;
    unsigned long service_url_index;
} sccpmsg_service_url_stat_req_t;

typedef struct sccpmsg_line_stat_req_t_ {
    unsigned long message_id;
    unsigned long line_number;
} sccpmsg_line_stat_req_t;

typedef struct sccpmsg_feature_stat_req_t_ {
    unsigned long message_id;
    unsigned long feature_index;
} sccpmsg_feature_stat_req_t;

typedef struct sccpmsg_config_stat_req_t_ {
    unsigned long message_id;
} sccpmsg_config_stat_req_t;

typedef struct sccpmsg_time_date_req_t_ {
    unsigned long message_id;
} sccpmsg_time_date_req_t;

typedef struct sccpmsg_button_template_req_t_ {
    unsigned long message_id;
} sccpmsg_button_template_req_t;

typedef struct sccpmsg_version_req_t_ {
    unsigned long message_id;
} sccpmsg_version_req_t;

typedef struct sccpmsg_keepalive_t_ {
    unsigned long message_id;
} sccpmsg_keepalive_t;

typedef struct sccpmsg_capabilities_res_t_ {
    unsigned long                message_id;
    unsigned long                capabilites_count;
    sccpmsg_media_capabilities_t capabilities[SCCPMSG_MAX_MEDIA_CAPABILITIES];
} sccpmsg_capabilities_res_t;

typedef struct sccpmsg_media_port_list_t_ {
    unsigned long message_id;
    //TODO:
    // No details on this structure available
} sccpmsg_media_port_list_t;

typedef struct sccpmsg_alarm_t_ {
    unsigned long            message_id;
    sccpmsg_alarm_severity_t alarm_severity;
    char                     text[SCCPMSG_MAX_ALARM_TEXT_SIZE];
    unsigned long            param1;
    unsigned long            param2;
} sccpmsg_alarm_t;

typedef struct sccpmsg_server_req_t_ {
    unsigned long           message_id;
} sccpmsg_server_req_t;

typedef struct sccpmsg_connection_statistics_res_t_ {
    unsigned long              message_id;
    char                       directory_number[SCCPMSG_DIRECTORY_NUMBER_SIZE + 1];
    unsigned long              call_reference;
    sccpmsg_stats_processing_t processing_mode;
    unsigned long              number_packets_sent;
    unsigned long              number_bytes_sent;
    unsigned long              number_packets_received;
    unsigned long              number_bytes_received;
    unsigned long              number_packets_lost;
    unsigned long              jitter;
    unsigned long              latency;
} sccpmsg_connection_statistics_res_t;

typedef struct sccpmsg_offhook_with_cgpn_t_ {
    unsigned long message_id;
    char          calling_party_number[SCCPMSG_DIRECTORY_NUMBER_SIZE];
    char          cgpn_voice_mailbox[SCCPMSG_DIRECTORY_NUMBER_SIZE];
    unsigned long line_number;
} sccpmsg_offhook_with_cgpn_t;

typedef struct sccpmsg_softkey_template_req_t_ {
    unsigned long message_id;
} sccpmsg_softkey_template_req_t;

typedef struct sccpmsg_softkey_set_req_t_ {
    unsigned long message_id;
} sccpmsg_softkey_set_req_t;

typedef struct sccpmsg_softkey_event_t_ {
    unsigned long message_id;
    unsigned long softkey_event;
    unsigned long line_number;
    unsigned long call_reference;
} sccpmsg_softkey_event_t;

typedef struct sccpmsg_media_transmission_failure_t_ {
    unsigned long message_id;
    unsigned long conference_id;
    unsigned long passthru_party_id;
    unsigned long remote_ip_address;
    unsigned long remote_port_number;
    unsigned long call_reference;
} sccpmsg_media_transmission_failure_t;

typedef struct sccpmsg_headset_status_t_ {
    unsigned long             message_id;
    sccpmsg_headset_statuss_t status;
} sccpmsg_headset_status_t;

typedef struct sccpmsg_register_available_lines_t_ {
    unsigned long message_id;
    unsigned long max_number_available_lines;
} sccpmsg_register_available_lines_t;

typedef struct sccpmsg_device_to_user_data_t_ {
    unsigned long                  message_id;
    sccpmsg_user_and_device_data_t data;
} sccpmsg_device_to_user_data_t;

typedef struct sccpmsg_device_to_user_data_response_t_ {
    unsigned long                  message_id;
    sccpmsg_user_and_device_data_t data;
} sccpmsg_device_to_user_data_response_t;

typedef struct sccpmsg_device_to_user_data_version1_t_ {
    unsigned long                           message_id;
    sccpmsg_user_and_device_data_version1_t data;
} sccpmsg_device_to_user_data_version1_t;

typedef struct sccpmsg_device_to_user_data_response_version1_t_ {
    unsigned long                                   message_id;
    sccpmsg_user_and_device_data_version1_t         data;
} sccpmsg_device_to_user_data_response_version1_t;


/*-------------- From the CM --------------*/

typedef struct sccpmsg_keepalive_ack_t_ {
    unsigned long message_id;
} sccpmsg_keepalive_ack_t;

typedef struct sccpmsg_register_ack_t_ {
    unsigned long message_id;
    unsigned long keepalive_interval_1;
    char          date_template[SCCPMSG_DATE_TEMPLATE_SIZE];
    unsigned long keepalive_interval_2;
    unsigned long max_protocol_version;
} sccpmsg_register_ack_t;

typedef struct sccpmsg_register_token_ack_t_ {
    unsigned long message_id;
} sccpmsg_register_token_ack_t;

typedef struct sccpmsg_register_token_reject_t_ {
    unsigned long message_id;
    unsigned long wait_time_before_next_reg;
} sccpmsg_register_token_reject_t;

typedef struct sccpmsg_unregister_ack_t_ {
    unsigned long               message_id;
    sccpmsg_unregister_status_t status;
} sccpmsg_unregister_ack_t;

typedef struct sccpmsg_register_reject_t_ {
    unsigned long message_id;
    char          text[SCCPMSG_DISPLAY_TEXT_SIZE];
} sccpmsg_register_reject_t;

typedef struct sccpmsg_start_tone_t_ {
    unsigned long            message_id;
    sccpmsg_tone_t           tone;
    sccpmsg_tone_direction_t direction;
    unsigned long            line_number;
    unsigned long            call_reference;
} sccpmsg_start_tone_t;

typedef struct sccpmsg_stop_tone_t_ {
    unsigned long message_id;
    unsigned long line_number;
    unsigned long call_reference;
} sccpmsg_stop_tone_t;

typedef struct sccpmsg_set_ringer_t_ {
    unsigned long           message_id;
    sccpmsg_ring_mode_t     ring_mode;
    sccpmsg_ring_duration_t ring_duration;
    unsigned long           line_number;
    unsigned long           call_reference;
} sccpmsg_set_ringer_t;

typedef struct sccpmsg_set_lamp_t_ {
    unsigned long             message_id;
    sccpmsg_device_stimulus_t stimulus;
    unsigned long             line_number;
    sccpmsg_lamp_mode_t       lamp_mode;
} sccpmsg_set_lamp_t;

typedef struct sccpmsg_set_speaker_mode_t_ {
    unsigned long          message_id;
    sccpmsg_speaker_mode_t mode;
} sccpmsg_set_speaker_mode_t;

typedef struct sccpmsg_set_micro_mode_t_ {
    unsigned long             message_id;
    sccpmsg_microphone_mode_t mode;
} sccpmsg_set_micro_mode_t;

typedef struct sccpmsg_start_session_transmission_t_ {
    unsigned long          message_id;
    unsigned long          remote_ip_address;
    sccpmsg_session_type_t session_type;
} sccpmsg_start_session_transmission_t;

typedef struct sccpmsg_stop_session_transmission_t_ {
    unsigned long          message_id;
    unsigned long          remote_ip_address;
    sccpmsg_session_type_t session_type;
} sccpmsg_stop_session_transmission_t;

typedef struct sccpmsg_start_media_transmission_t_ {
    unsigned long                      message_id;
    unsigned long                      conference_id;
    unsigned long                      passthru_party_id;
    unsigned long                      ip_address;
    unsigned long                      port;
    unsigned long                      ms_packet_size;
    sccpmsg_media_payload_type_t       payload;
    sccpmsg_media_qualifier_outgoing_t qualifier;
    unsigned long                      call_reference;
    sccpmsg_media_encryption_key_t     media_encryption;
} sccpmsg_start_media_transmission_t;

typedef struct sccpmsg_stop_media_transmission_t_ {
    unsigned long message_id;
    unsigned long conference_id;
    unsigned long passthru_party_id;
    unsigned long call_reference;
} sccpmsg_stop_media_transmission_t;

typedef struct sccpmsg_open_receive_channel_t_ {
    unsigned long                      message_id;
    unsigned long                      conference_id;
    unsigned long                      passthru_party_id;
    unsigned long                      ms_packet_size;
    sccpmsg_media_payload_type_t       payload;
    sccpmsg_media_qualifier_incoming_t qualifier;
    unsigned long                      call_reference;
    sccpmsg_media_encryption_key_t     media_encryption;
} sccpmsg_open_receive_channel_t;

typedef enum sccpmsg_orc_status_t_ {
    SCCPMSG_ORC_STATUS_OK      = 0,
    SCCPMSG_ORC_STATUS_ERROR   = 1,
    SCCPMSG_ORC_STATUS_INVALID
} sccpmsg_orc_status_t;

typedef struct sccpmsg_open_receive_channel_ack_t_ {
    unsigned long        message_id;
    sccpmsg_orc_status_t status;
    unsigned long        ip_address;
    unsigned long        port;
    unsigned long        passthru_party_id;
    unsigned long        call_reference;
} sccpmsg_open_receive_channel_ack_t;

typedef struct sccpmsg_close_receive_channel_t_ {
    unsigned long message_id;
    unsigned long conference_id;
    unsigned long passthru_party_id;
    unsigned long call_reference;
} sccpmsg_close_receive_channel_t;

typedef struct sccpmsg_feature_stat_t_ {
    unsigned long message_id;
    unsigned long feature_index;
    unsigned long feature_id;
    char          feture_text_label[SCCPMSG_DIRECTORY_NAME_SIZE];
    unsigned long feature_status;
} sccpmsg_feature_stat_t;

typedef struct sccpmsg_reset_t_ {
    unsigned long        message_id;
    sccpmsg_reset_type_t reset_type;
} sccpmsg_reset_t;

typedef struct sccpmsg_call_info_t_ {
    unsigned long             message_id;
    char                      calling_party_name[SCCPMSG_DIRECTORY_NAME_SIZE];
    char                      calling_party_number[SCCPMSG_DIRECTORY_NUMBER_SIZE];
    char                      called_party_name[SCCPMSG_DIRECTORY_NAME_SIZE];
    char                      called_party_number[SCCPMSG_DIRECTORY_NUMBER_SIZE];
    unsigned long             line_number;
    unsigned long             call_reference;
    sccpmsg_call_types_t      call_type;
    char                      original_called_party_name[SCCPMSG_DIRECTORY_NAME_SIZE];
    char                      original_called_party_number[SCCPMSG_DIRECTORY_NUMBER_SIZE];
    char                      last_redirecting_party_name[SCCPMSG_DIRECTORY_NAME_SIZE];
    char                      last_redirecting_party_number[SCCPMSG_DIRECTORY_NUMBER_SIZE];
    unsigned long             original_cdpn_redirect_reason;
    unsigned long             last_redirect_reason;
    char                      cgpn_voice_mailbox[SCCPMSG_DIRECTORY_NUMBER_SIZE];
    char                      cdpn_voice_mailbox[SCCPMSG_DIRECTORY_NUMBER_SIZE];
    char                      original_cdpn_voice_mailbox[SCCPMSG_DIRECTORY_NUMBER_SIZE];
    char                      last_redirecting_voice_mailbox[SCCPMSG_DIRECTORY_NUMBER_SIZE];
    unsigned long             call_instance;
    sccpmsg_security_status_t security_status;
    sccpmsg_restrict_info_t   restrict_info;
} sccpmsg_call_info_t;

typedef struct sccpmsg_call_select_stat_t_ {
    unsigned long message_id;
    unsigned long call_reference;
    unsigned long line_number;
} sccpmsg_call_select_stat_t;

typedef struct sccpmsg_dialed_number_t_ {
    unsigned long message_id;
    char          dialed_number[SCCPMSG_DIRECTORY_NUMBER_SIZE];
    unsigned long line_number;
    unsigned long call_reference;
} sccpmsg_dialed_number_t;

typedef struct sccpmsg_user_to_device_data_t_ {
    unsigned long                  message_id;
    sccpmsg_user_and_device_data_t data;
} sccpmsg_user_to_device_data_t;

typedef struct sccpmsg_user_to_device_data_version1_t_ {
    unsigned long                           message_id;
    sccpmsg_user_and_device_data_version1_t data;
} sccpmsg_user_to_device_data_version1_t;

typedef struct sccpmsg_forward_stat_t_ {
    unsigned long message_id;
    unsigned long active_forward;
    unsigned long line_number;
    unsigned long forward_all_active;
    char          forward_all_directory_number[SCCPMSG_DIRECTORY_NUMBER_SIZE];
    unsigned long forward_busy_active;
    char          forward_busy_directory_number[SCCPMSG_DIRECTORY_NUMBER_SIZE];
    unsigned long forward_no_answer_active;
    char          forward_no_answer_directory_number[SCCPMSG_DIRECTORY_NUMBER_SIZE];
} sccpmsg_forward_stat_t;

typedef struct sccpmsg_speeddial_stat_t_ {
    unsigned long message_id;
    unsigned long speeddial_number;
    char          speeddial_directory_number[SCCPMSG_DIRECTORY_NUMBER_SIZE];
    char          speeddial_display_name[SCCPMSG_DIRECTORY_NAME_SIZE];
} sccpmsg_speeddial_stat_t;

typedef struct sccpmsg_service_url_stat_t_ {
    unsigned long message_id;
    unsigned long service_url_index;
    char          service_url[SCCPMSG_MAX_SERVICE_URL_SIZE];
    char          service_url_display_name[SCCPMSG_DIRECTORY_NAME_SIZE];
} sccpmsg_service_url_stat_t;

typedef struct sccpmsg_line_stat_t_ {
    unsigned long message_id;
    unsigned long line_number;
    char          line_directory_number[SCCPMSG_DIRECTORY_NUMBER_SIZE];
    char          line_fully_qualified_display_name[SCCPMSG_DIRECTORY_NAME_SIZE];
    char          line_text_label[SCCPMSG_DIRECTORY_NAME_SIZE];
    unsigned long display_options;
} sccpmsg_line_stat_t;

typedef struct sccpmsg_config_stat_t_ {
    unsigned long message_id;
    sccpmsg_sid_t sid;
    char          user_name[SCCPMSG_DIRECTORY_NAME_SIZE];
    char          server_name[SCCPMSG_DIRECTORY_NAME_SIZE];
    unsigned long number_lines;
    unsigned long number_speed_dials;
} sccpmsg_config_stat_t;

typedef struct sccpmsg_define_time_date_t_ {
    unsigned long          message_id;
    sccpmsg_station_time_t station_time;
    unsigned long          system_time;
} sccpmsg_define_time_date_t;

typedef struct sccpmsg_button_template_t_ {
    unsigned long       message_id;
    sccpmsg_button_template_definition_t button_template;
} sccpmsg_button_template_t;

typedef struct sccpmsg_version_t_ {
    unsigned long message_id;
    char          version[SCCPMSG_MAX_VERSION_SIZE];
} sccpmsg_version_t;

typedef struct sccpmsg_display_text_t_ {
    unsigned long message_id;
    char          text[SCCPMSG_DISPLAY_TEXT_SIZE];
} sccpmsg_display_text_t;

typedef struct sccpmsg_clear_display_t_ {
    unsigned long message_id;
} sccpmsg_clear_display_t;

typedef struct sccpmsg_capabilities_req_t_ {
    unsigned long           message_id;
} sccpmsg_capabilities_req_t;

typedef struct sccpmsg_enunciator_command_t_ {
    unsigned long message_id;
    // TODO:
    // No details available
} sccpmsg_enunciator_command_t;

typedef struct sccpmsg_server_res_t_ {
    unsigned long message_id;
    char          server[SCCPMSG_DIRECTORY_NUMBER_SIZE][SCCPMSG_MAX_SERVERS];
    unsigned long port[SCCPMSG_MAX_SERVERS];
    unsigned long ip_address[SCCPMSG_MAX_SERVERS];
} sccpmsg_server_res_t;

typedef struct sccpmsg_connection_statistics_req_t_ {
    unsigned long              message_id;
    char                       directory_number[SCCPMSG_DIRECTORY_NUMBER_SIZE];
    unsigned long              call_reference;
    sccpmsg_stats_processing_t processing_mode;
} sccpmsg_connection_statistics_req_t;

typedef struct sccpmsg_start_multicast_media_reception_t_ {
    unsigned long                      message_id;
    unsigned long                      conference_id;
    unsigned long                      passthru_party_id;
    unsigned long                      ip_address;
    unsigned long                      port;
    unsigned long                      ms_packet_size;
    sccpmsg_media_payload_type_t       payload;
    sccpmsg_media_qualifier_incoming_t qualifier;
    unsigned long                      call_reference;
} sccpmsg_start_multicast_media_reception_t;

typedef enum sccpmsg_mmr_status_t_ {
    SCCPMSG_MMR_OK      = 0,
    SCCPMSG_MMR_ERROR   = 1,
    SCCPMSG_MMR_INVALID
} sccpmsg_mmr_status_t;

typedef struct sccpmsg_multicast_media_reception_ack_t_ {
    unsigned long        message_id;
    sccpmsg_mmr_status_t status;
    unsigned long        passthru_party_id;
    unsigned long        call_reference;
} sccpmsg_multicast_media_reception_ack_t;

typedef struct sccpmsg_start_multicast_media_transmission_t_ {
    unsigned long                      message_id;
    unsigned long                      conference_id;
    unsigned long                      passthru_party_id;
    unsigned long                      ip_address;
    unsigned long                      port;
    unsigned long                      ms_packet_size;
    sccpmsg_media_payload_type_t       payload;
    sccpmsg_media_qualifier_outgoing_t qualifier;
    unsigned long                      call_reference;
} sccpmsg_start_multicast_media_transmission_t;

typedef struct sccpmsg_stop_multicast_media_reception_t_ {
    unsigned long message_id;
    unsigned long conference_id;
    unsigned long passthru_party_id;
    unsigned long call_reference;
} sccpmsg_stop_multicast_media_reception_t;

typedef struct sccpmsg_stop_multicast_media_transmission_t_ {
    unsigned long message_id;
    unsigned long conference_id;
    unsigned long passthru_party_id;
    unsigned long call_reference;
} sccpmsg_stop_multicast_media_transmission_t;

typedef struct sccpmsg_softkey_template_res_t_ {
    unsigned long              message_id;
    sccpmsg_softkey_template_t softkey_template;
} sccpmsg_softkey_template_res_t;

typedef struct sccpmsg_softkey_set_res_t_ {
    unsigned long         message_id;
    sccpmsg_softkey_set_t softkey_set;
} sccpmsg_softkey_set_res_t;

typedef struct sccpmsg_select_softkeys_t {
    unsigned long message_id;
    unsigned long line_number;
    unsigned long reference;
    unsigned long softkey_set_index;
    unsigned long valid_key_mask;
} sccpmsg_select_softkeys_t;

typedef struct sccpmsg_call_state_t_ {
    unsigned long         message_id;
    sccpmsg_call_states_t call_state;
    unsigned long         line_number;
    unsigned long         call_reference;
    unsigned long         privacy;
    sccpmsg_precedence_t  precedence;
} sccpmsg_call_state_t;

typedef struct sccpmsg_display_prompt_status_t_ {
    unsigned long message_id;
    unsigned long timeout;
    char          text[SCCPMSG_DISPLAY_PROMPT_TEXT_SIZE];
    unsigned long line_number;
    unsigned long call_reference;
} sccpmsg_display_prompt_status_t;

typedef struct sccpmsg_clear_prompt_status_t_ {
    unsigned long message_id;
    unsigned long line_number;
    unsigned long call_reference;
} sccpmsg_clear_prompt_status_t;

typedef struct sccpmsg_display_notify_t_ {
    unsigned long message_id;
    unsigned long timeout;
    char          text[SCCPMSG_DISPLAY_NOTIFY_TEXT_SIZE];
} sccpmsg_display_notify_t;

typedef struct sccpmsg_display_priority_notify_t_ {
    unsigned long message_id;
    unsigned long timeout;
    unsigned long priority;
    char          text[SCCPMSG_DISPLAY_NOTIFY_TEXT_SIZE];
} sccpmsg_display_priority_notify_t;

typedef struct sccpmsg_clear_notify_t_ {
    unsigned long message_id;
} sccpmsg_clear_notify_t;

typedef struct sccpmsg_clear_priority_notify_t_ {
    unsigned long message_id;
    unsigned long priority;
} sccpmsg_clear_priority_notify_t;

typedef struct sccpmsg_activate_call_plane_t_ {
    unsigned long message_id;
    unsigned long line_number;
} sccpmsg_activate_call_plane_t;

typedef struct sccpmsg_deactivate_call_plane_t_ {
    unsigned long message_id;
} sccpmsg_deactivate_call_plane_t;

typedef struct sccpmsg_backspace_req_t_ {
    unsigned long message_id;
    unsigned long line_number;
    unsigned long call_reference;
} sccpmsg_backspace_req_t;

typedef struct sccpmsg_start_media_failure_detection_t_ {
    unsigned long                      message_id;
    unsigned long                      conference_id;
    unsigned long                      passthru_party_id;
    unsigned long                      ms_packet_size;
    sccpmsg_media_payload_type_t       payload;
    sccpmsg_media_qualifier_incoming_t qualifier;
    unsigned long                      call_reference;
} sccpmsg_start_media_failure_detection_t;

#ifdef SCCPMSG_PARCHE
typedef struct sccpmsg_record_info_t_ {
    unsigned long           message_id;
    char                    recording_party_name[SCCPMSG_DIRECTORY_NAME_SIZE];
    char                    recording_party[SCCPMSG_DIRECTORY_NUMBER_SIZE];
    char                    connected_party_name[SCCPMSG_DIRECTORY_NAME_SIZE];
    char                    connected_party[SCCPMSG_DIRECTORY_NUMBER_SIZE];
    unsigned long           call_reference;
    sccpmsg_call_types_t    call_type;
    char                    recording_party_voice_mailbox[SCCPMSG_DIRECTORY_NUMBER_SIZE];
    char                    recording_party_voice_mail_pilot_number[SCCPMSG_DIRECTORY_NUMBER_SIZE];
    unsigned long           conference_id;
} sccpmsg_record_info_t;

typedef enum sccpmsg_recorder_onhook_status_t_ {
    SCCPMSG_RECORDER_OK,
    SCCPMSG_RECORDER_ERR,
    SCCPMSG_RECORDER_FULL,
    SCCPMSG_RECORDER_MAX_TIME,
    SCCPMSG_RECORDER_LIMIT
} sccpmsg_recorder_onhook_status_t;

typedef struct sccpmsg_record_onhook_t_ {
    unsigned long                    message_id;
    unsigned long                    conference_id;
    sccpmsg_recorder_onhook_status_t status;
} sccpmsg_record_onhook_t;

typedef enum sccpmsg_play_ann_status_t_ {
    SCCPMSG_PLAY_TONE_OK,
    SCCPMSG_PLAY_TONE_ERR,
} sccpmsg_play_ann_status_t;

typedef struct sccpmsg_announcement_finish_t_ {
    unsigned long             message_id;
    unsigned long             conference_id;
    sccpmsg_play_ann_status_t status;
} sccpmsg_announcement_finish_t;

typedef enum sccpmsg_end_of_ann_ack_t_ {
    SCCPMSG_ANN_ACK_REQUIRED_NO_OK,
    SCCPMSG_ANN_ACK_REQUIRED_YES,
} sccpmsg_end_of_ann_ack_t;

typedef enum sccpmsg_ann_play_mode_t_ {
    SCCPMSG_ANN_XML_CONFIG_MODE,
    SCCPMSG_ANN_ONE_SHOT_MODE,
    SCCPMSG_ANN_CONTINUOUS_MODE,
} sccpmsg_ann_play_mode_t;

typedef struct sccpmsg_ann_play_list_t_ {
    unsigned long  locale;
    sccpmsg_tone_t tone;
} sccpmsg_ann_play_list_t;

typedef struct sccpmsg_start_announcement_t_ {
    unsigned long             message_id;
    sccpmsg_ann_play_mode_t   ann_list[SCCPMSG_MAX_ANNOUNCEMENT_LIST];
    sccpmsg_end_of_ann_ack_t  ann_ack_req;
    unsigned long             conference_id;
    unsigned long             matrix_conf_prty_id[SCCPMSG_MAX_MONITOR_PARTIES];
    unsigned long             hearing_conf_pary_mask;
    sccpmsg_ann_play_mode_t   ann_play_mode;
} sccpmsg_start_announcement_t;

typedef struct sccpmsg_stop_announcement_t_ {
    unsigned long             message_id;
    unsigned long             conference_id;
} sccpmsg_stop_announcement_t;

typedef struct sccpmsg_notify_dtmf_tone_t_ {
    unsigned long   message_id;
    sccpmsg_tone_t  tone;
    unsigned long   conference_id;
    unsigned long   passthru_party_id;
} sccpmsg_notify_dtmf_tone_t;

typedef struct sccpmsg_subscribe_dtmf_payload_req_t_ {
    unsigned long   message_id;
    unsigned long   payload_dtmf;
    unsigned long   conference_id;
    unsigned long   passthru_party_id;
} sccpmsg_subscribe_dtmf_payload_req_t;

typedef struct sccpmsg_subscribe_dtmf_payload_res_t_ {
    unsigned long   message_id;
    unsigned long   payload_dtmf;
    unsigned long   conference_id;
    unsigned long   passthru_party_id;
} sccpmsg_subscribe_dtmf_payload_res_t;

typedef struct sccpmsg_subscribe_dtmf_payload_err_t_ {
    unsigned long   message_id;
    unsigned long   payload_dtmf;
    unsigned long   conference_id;
    unsigned long   passthru_party_id;
} sccpmsg_subscribe_dtmf_payload_err_t;

typedef struct sccpmsg_unsubscribe_dtmf_payload_req_t_ {
    unsigned long   message_id;
    unsigned long   payload_dtmf;
    unsigned long   conference_id;
    unsigned long   passthru_party_id;
} sccpmsg_unsubscribe_dtmf_payload_req_t;

typedef struct sccpmsg_unsubscribe_dtmf_payload_res_t_ {
    unsigned long   message_id;
    unsigned long   payload_dtmf;
    unsigned long   conference_id;
    unsigned long   passthru_party_id;
} sccpmsg_unsubscribe_dtmf_payload_res_t;

typedef struct sccpmsg_unsubscribe_dtmf_payload_err_t_ {
    unsigned long   message_id;
    unsigned long   payload_dtmf;
    unsigned long   conference_id;
    unsigned long   passthru_party_id;
} sccpmsg_unsubscribe_dtmf_payload_err_t;

typedef struct sccpmsg_update_capabilities_t_ {
    unsigned long   message_id;
    unsigned long   audio_cap_count;
    unsigned long   video_capp_count;
    unsigned long   data_cap_count;
    unsigned long   rtp_payload_format;
    unsigned long   custom_picture_format_count;
    /** some more needs to be added **/
} sccpmsg_update_capabilities_t;
#endif
#if 0
typedef enum sccpmsg_stimulus_type_t_ {
    SCCPMSG_STIMULUS_TYPE_LAST_NUMBER_REDIAL       =  0x01,
    SCCPMSG_STIMULUS_TYPE_SPEED_DIAL               =  0x02,
    SCCPMSG_STIMULUS_TYPE_HOLD                     =  0x03,
    SCCPMSG_STIMULUS_TYPE_TRANSFER                 =  0x04,
    SCCPMSG_STIMULUS_TYPE_FORWARD_ALL              =  0x05,
    SCCPMSG_STIMULUS_TYPE_FORWARD_BUSY             =  0x06,
    SCCPMSG_STIMULUS_TYPE_FORWARD_NO_ANSWER        =  0x07,
    SCCPMSG_STIMULUS_TYPE_DISPLAY                  =  0x08,
    SCCPMSG_STIMULUS_TYPE_LINE                     =  0x09,
    SCCPMSG_STIMULUS_TYPE_T120_CHAT                =  0x0a,
    SCCPMSG_STIMULUS_TYPE_T120_WHITEBOARD          =  0x0b,
    SCCPMSG_STIMULUS_TYPE_T120_APPLICATION_SHARING =  0x0c,
    SCCPMSG_STIMULUS_TYPE_T120_FILE_TRANSFER       =  0x0d,
    SCCPMSG_STIMULUS_TYPE_VIDEO                    =  0x0e,
    SCCPMSG_STIMULUS_TYPE_VOICEMAIL                =  0x0f,
    SCCPMSG_STIMULUS_TYPE_ANSWER_RELEASE           =  0x10,
    SCCPMSG_STIMULUS_TYPE_AUTO_ANSWER              =  0x11,
    SCCPMSG_STIMULUS_TYPE_SELECT                   =  0x12,
    SCCPMSG_STIMULUS_TYPE_PRIVACY                  =  0x13,
    SCCPMSG_STIMULUS_TYPE_SERVICE_URL              =  0x14,
    SCCPMSG_STIMULUS_TYPE_GENERIC_APP_B1           =  0x21,
    SCCPMSG_STIMULUS_TYPE_GENERIC_APP_B2           =  0x22,
    SCCPMSG_STIMULUS_TYPE_GENERIC_APP_B3           =  0x23,
    SCCPMSG_STIMULUS_TYPE_GENERIC_APP_B4           =  0x24,
    SCCPMSG_STIMULUS_TYPE_GENERIC_APP_B5           =  0x25,
    SCCPMSG_STIMULUS_TYPE_GROUP_CALL_PICKUP        =  0x50,
    SCCPMSG_STIMULUS_TYPE_MEET_ME_CONFERENCE       =  0x7b,
    SCCPMSG_STIMULUS_TYPE_CONFERENCE               =  0x7d,
    SCCPMSG_STIMULUS_TYPE_CALL_PARK                =  0x7e,
    SCCPMSG_STIMULUS_TYPE_CALL_PICKUP              =  0x7f,
    SCCPMSG_STIMULUS_TYPE_INVALID
} sccpmsg_stimulus_type_t;
#endif
typedef enum sccpmsg_prompt_text_t_ {
    SCCPMSG_PROMPT_TEXT_OPTIONS         =  0x00,
    SCCPMSG_PROMPT_TEXT_OFFHOOK         =  0x01,
    SCCPMSG_PROMPT_TEXT_ONHOOK          =  0x02,
    SCCPMSG_PROMPT_TEXT_RING_OUT        =  0x03,
    SCCPMSG_PROMPT_TEXT_FROM            =  0x04,
    SCCPMSG_PROMPT_TEXT_CONNECTED       =  0x05,
    SCCPMSG_PROMPT_TEXT_BUSY            =  0x06,
    SCCPMSG_PROMPT_TEXT_LINE_IN_USE     =  0x07,
    SCCPMSG_PROMPT_TEXT_HOLD            =  0x08,
    SCCPMSG_PROMPT_TEXT_CALL_WAITING    =  0x09,
    SCCPMSG_PROMPT_TEXT_CALL_TRANSFER   =  0x0a,
    SCCPMSG_PROMPT_TEXT_CALL_PARK       =  0x0b,
    SCCPMSG_PROMPT_TEXT_CALL_PROCEED    =  0x0c,
    SCCPMSG_PROMPT_TEXT_IN_USE_REMOTELY =  0x0d,
    SCCPMSG_PROMPT_TEXT_INVALID
} sccpmsg_prompt_text_t;

typedef struct sccpmsg_base_t_ {
    unsigned long length;
    unsigned long type;
} sccpmsg_base_t;

typedef struct sccpmsg_message_id_t_ {
    unsigned long message_id;
} sccpmsg_message_id_t;

typedef struct sccpmsg_general_t_ {
    sccpmsg_base_t base;
    union {
        sccpmsg_message_id_t                         msg_id;
        
        /*-------------- To the CM --------------*/    
        sccpmsg_keepalive_t                          keepalive;
        sccpmsg_register_t                           reg;
        sccpmsg_ip_port_t                            ip_port;
        sccpmsg_keypad_button_t                      keypad_button;
        sccpmsg_enbloc_call_t                        enbloc_call;
        sccpmsg_stimulus_t                           stimulus;
        sccpmsg_offhook_t                            offhook;
        sccpmsg_onhook_t                             onhook;
        sccpmsg_hook_flash_t                         hook_flash; 
        sccpmsg_forward_stat_req_t                   forward_stat_req;
        sccpmsg_speeddial_stat_req_t                 speeddial_stat_req;
        sccpmsg_line_stat_req_t                      line_stat_req;
        sccpmsg_config_stat_req_t                    config_stat_req;
        sccpmsg_time_date_req_t                      time_date_req;
        sccpmsg_button_template_req_t                button_template_req;
        sccpmsg_version_req_t                        version_req;
        sccpmsg_capabilities_res_t                   capabilities_res;
        sccpmsg_media_port_list_t                    media_port_list;
        sccpmsg_server_req_t                         server_req;
        sccpmsg_alarm_t                              alarm;
        sccpmsg_multicast_media_reception_ack_t      multicast_media_reception_ack;
        sccpmsg_open_receive_channel_ack_t           open_receive_channel_ack;
        sccpmsg_connection_statistics_res_t          connection_statistics_res;
        sccpmsg_offhook_with_cgpn_t                  offhook_with_cgpn;
        sccpmsg_softkey_set_req_t                    softkey_set_req;
        sccpmsg_softkey_event_t                      softkey_event;
        sccpmsg_unregister_t                         unregister;
        sccpmsg_softkey_template_req_t               softkey_template_req;
        sccpmsg_register_token_req_t                 register_token_req;
        sccpmsg_media_transmission_failure_t         media_transmission_failure;
        sccpmsg_headset_status_t                     headset_status;
        sccpmsg_media_resource_notification_t        media_resource_notification;
        sccpmsg_register_available_lines_t           register_available_lines;
        sccpmsg_device_to_user_data_t                device_to_user;
        sccpmsg_device_to_user_data_response_t       device_to_user_response;
        sccpmsg_service_url_stat_req_t               service_url_stat_req; 
        sccpmsg_feature_stat_req_t                   feature_stat_req;
        sccpmsg_device_to_user_data_t                device_to_user_data;
        sccpmsg_device_to_user_data_response_t       device_to_user_data_response;

#ifdef SCCPMSG_PARCHE
        sccpmsg_update_capabilities_t                update_capabilities;
        sccpmsg_open_multimedia_receive_channel_ack  open_multimedia_receive_channel_ack;
        sccpmsg_update_clear_conference              update_clear_conference;
        sccpmsg_create_conference_res                create_conference_res;     
        sccpmsg_delete_conference_res                delete_conference_res;
        sccpmsg_modify_conference_res                modify_conference_res;
        sccpmsg_add_participant_res                  add_participant_res;
        sccpmsg_device_to_user_data_version1         device_to_user_data_version1;
        sccpmsg_device_to_user_data_response_version1 device_to_user_data_response_version1;
#endif   

        /*-------------- From the CM --------------*/    
        sccpmsg_register_ack_t                       register_ack;
        sccpmsg_start_tone_t                         start_tone;
        sccpmsg_stop_tone_t                          stop_tone;
        sccpmsg_set_ringer_t                         set_ringer;
        sccpmsg_set_lamp_t                           set_lamp;
        //sccpmsg_set_hkf_detect_t                     set_hkf_detect;
        sccpmsg_set_speaker_mode_t                   set_speaker_mode;
        sccpmsg_set_micro_mode_t                     set_micro_mode;
        sccpmsg_start_media_transmission_t           start_media_transmission;
        sccpmsg_stop_media_transmission_t            stop_media_transmission;
        //sccpmsg_start_media_reception_t              start_media_reception;
        //sccpmsg_stop_media_reception_t               stop_media_reception;
        //sccpmsg_reserved_for_future_use_t            reserverd;
        sccpmsg_call_info_t                          call_info;
        sccpmsg_forward_stat_t                       forward_stat;
        sccpmsg_speeddial_stat_t                     speeddial_stat;
        sccpmsg_line_stat_t                          line_stat;
        sccpmsg_config_stat_t                        config_stat;
        sccpmsg_define_time_date_t                   define_time_date;
        sccpmsg_start_session_transmission_t         start_session_transemissin;
        sccpmsg_stop_session_transmission_t          stop_session_transmission;
        sccpmsg_button_template_t                    button_template;
        sccpmsg_version_t                            version;
        sccpmsg_display_text_t                       display_text;
        sccpmsg_clear_display_t                      clear_display;
        sccpmsg_capabilities_req_t                   capabilities_req;
        sccpmsg_enunciator_command_t                 enunciator_command;
        sccpmsg_register_reject_t                    register_reject;
        sccpmsg_server_res_t                         server_res;
        sccpmsg_reset_t                              reset;
        sccpmsg_keepalive_ack_t                      keepalive_ack;
        sccpmsg_start_multicast_media_reception_t    start_multicast_media_reception;
        sccpmsg_start_multicast_media_transmission_t start_multicast_media_transmission;
        sccpmsg_stop_multicast_media_reception_t     stop_multicast_media_reception;
        sccpmsg_stop_multicast_media_transmission_t  stop_multicast_media_transmission;
        sccpmsg_open_receive_channel_t               open_receive_channel;
        sccpmsg_close_receive_channel_t              close_receive_channel;
        sccpmsg_connection_statistics_req_t          connection_statistics_req;
        sccpmsg_softkey_template_res_t               softkey_template_res;
        sccpmsg_softkey_set_res_t                    softkey_set_res;
        sccpmsg_select_softkeys_t                    select_softkeys;
        sccpmsg_call_state_t                         call_state;
        sccpmsg_display_prompt_status_t              display_prompt_status;
        sccpmsg_clear_prompt_status_t                clear_prompt_status;
        sccpmsg_display_notify_t                     display_notify;
        sccpmsg_clear_notify_t                       clear_notify;
        sccpmsg_activate_call_plane_t                activate_call_plane;
        sccpmsg_deactivate_call_plane_t              deactivate_call_plane;
        sccpmsg_unregister_ack_t                     unregister_ack;
        sccpmsg_backspace_req_t                      backspace_req;
        sccpmsg_register_token_ack_t                 register_token_ack;
        sccpmsg_register_token_reject_t              register_token_reject;
        sccpmsg_start_media_failure_detection_t      start_media_failure_detection;
        sccpmsg_dialed_number_t                      dialed_number;
        sccpmsg_display_priority_notify_t            display_priority_notify;
        sccpmsg_clear_priority_notify_t              clear_priority_notify;
        sccpmsg_user_to_device_data_t                user_to_device_data;
        sccpmsg_service_url_stat_t                   service_url_stat; 
        sccpmsg_feature_stat_t                       feature_stat;
        sccpmsg_call_select_stat_t                   call_select_stat;
    } body;
} sccpmsg_general_t;

void sccpmsg_create_alarm_string(char *text, int tcp_close_cause, char *mac);
//char *sccpmsg_alarm_name(int id);
const char *sccpmsg_get_message_text(unsigned long index);
void sccpmsg_get_sccp_msg_data(struct sccp_sccpcb_t_ *sccpcb,
                               sccpmsg_general_t *msg,
                               unsigned long *call_id, unsigned long *line);
int sccpmsg_parse_msg(sccpmsg_general_t *gen_msg, int header, int msgid,
                      int body, struct sccp_sccpcb_t_ *sccpcb);
#ifdef SCCP_MSG_DUMP
void sccpmsg_dump(sccpmsg_general_t *gen_msg);
#endif

int sccpmsg_send_message(sccpmsg_msgcb_t *msgcb, unsigned int socket,
                         sccpmsg_general_t *msg, unsigned long length);

int sccpmsg_send_bare_message(sccpmsg_msgcb_t *msgcb, unsigned int socket,
                              sccpmsg_messages_t msg);
int sccpmsg_send_alarm(sccpmsg_msgcb_t *msgcb, unsigned int socket,
                       sccpmsg_alarm_severity_t level,
                       unsigned long param1, unsigned long param2,
                       char *msg);
int sccpmsg_send_connection_stats(sccpmsg_msgcb_t *msgcb, unsigned int socket,
                                  char *dirnum, 
                                  unsigned long call_identifier,
                                  sccpmsg_stats_processing_t mode,
                                  unsigned long num_packets_sent,
                                  unsigned long num_bytes_sent,
                                  unsigned long num_packets_received,
                                  unsigned long num_bytes_received,
                                  unsigned long num_packets_lost,
                                  unsigned long jitter,
                                  unsigned long latency);
int sccpmsg_send_register(sccpmsg_msgcb_t *msgcb, unsigned int socket,
                          char *device_name,
                          unsigned long device_type,
                          unsigned long version, unsigned long addr);
int sccpmsg_send_ip_port(sccpmsg_msgcb_t *msgcb, unsigned int socket,
                         unsigned long port);
int sccpmsg_send_capabilities_response(sccpmsg_msgcb_t *msgcb,
                                       unsigned int socket,
                                       gapi_media_caps_t *caps);
int sccpmsg_send_offhook(sccpmsg_msgcb_t *msgcb, unsigned int socket,
                         unsigned long line_number,
                         unsigned long call_reference);
int sccpmsg_send_keypress(sccpmsg_msgcb_t *msgcb, unsigned int socket,
                          sccpmsg_keypad_buttons_t button,
                          unsigned long line_number,
                          unsigned long call_reference);
int sccpmsg_send_onhook(sccpmsg_msgcb_t *msgcb, unsigned int socket,
                        unsigned long line_number,
                        unsigned long call_reference);
int sccpmsg_send_enbloc(sccpmsg_msgcb_t *msgcb, unsigned int socket,
                        char *dialstring);
int sccpmsg_send_headset_status(sccpmsg_msgcb_t *msgcb, unsigned int socket,
                                sccpmsg_headset_statuss_t status);
int sccpmsg_send_register_available_lines(sccpmsg_msgcb_t *msgcb,
                                          unsigned int socket,
                                          unsigned long lines);
int sccpmsg_send_line_stat_req(sccpmsg_msgcb_t *msgcb,
                               unsigned int socket,
                               unsigned long line);
int sccpmsg_send_speeddial_stat_req(sccpmsg_msgcb_t *msgcb,
                                    unsigned int socket,
                                    unsigned long line);
int sccpmsg_send_feature_stat_req(sccpmsg_msgcb_t *msgcb,
                                      unsigned int socket,
                                      unsigned long line);
int sccpmsg_send_service_url_stat_req(sccpmsg_msgcb_t *msgcb,
                                      unsigned int socket,
                                      unsigned long line);
int sccpmsg_send_open_receive_channel_ack(sccpmsg_msgcb_t *msgcb,
                                          unsigned int socket,
                                          unsigned long addr,
                                          unsigned long port,
                                          sccpmsg_orc_status_t status,
                                          unsigned long passthru_party_id,
                                          unsigned long call_reference);
int sccpmsg_send_device_to_user_data(sccpmsg_msgcb_t *msgcb,
                                     unsigned int socket, char *user_data,
                                     unsigned long len,
                                     unsigned long application_id,
                                     unsigned long transaction_id,
                                     unsigned long line,
                                     unsigned long call_reference);
int sccpmsg_send_device_to_user_data_response(sccpmsg_msgcb_t *msgcb,
                                              unsigned int socket,
                                              char *user_data,
                                              unsigned long len,
                                              unsigned long application_id,
                                              unsigned long transaction_id,
                                              unsigned long line,
                                              unsigned long call_reference);
int sccpmsg_send_softkey_event(sccpmsg_msgcb_t *msgcb, unsigned int socket,
                               int event,
                               unsigned long line,
                               unsigned long call_reference);
int sccpmsg_send_register_token_req(sccpmsg_msgcb_t *msgcb,
                                    unsigned int socket, char *device_name,
                                    unsigned long device_type,
                                    unsigned long addr);

int sccpmsg_get_primary_socket(struct sccp_sccpcb_t_*sccpcb);

/*
 * Helper functions for translating SCCP enums to GAPi enums.
 * Currently there is a 1:1 mapping of SCCP to GAPI enums. Therefore, one
 * function can be used to just return the value;
 */
int sccpmsg_sccp_to_gapi_enum(int id);
//#define sccpmsg_gapi_to_sccp_feature(a)         sccpmsg_sccp_to_gapi_enum(a)
#define sccpmsg_gapi_to_sccp_feature(a)         (a)
#define sccpmsg_sccp_to_gapi_tone(a)            sccpmsg_sccp_to_gapi_enum(a)
#define sccpmsg_sccp_to_gapi_tone_direction(a)  sccpmsg_sccp_to_gapi_enum(a)
#define sccpmsg_sccp_to_gapi_ringer(a)          sccpmsg_sccp_to_gapi_enum(a)
#define sccpmsg_sccp_to_gapi_ring_duration(a)   sccpmsg_sccp_to_gapi_enum(a)
#define sccpmsg_sccp_to_gapi_speaker(a)         sccpmsg_sccp_to_gapi_enum(a)
#define sccpmsg_sccp_to_gapi_micro(a)           sccpmsg_sccp_to_gapi_enum(a)
#define sccpmsg_sccp_to_gapi_stimulus(a)        sccpmsg_sccp_to_gapi_enum(a)
#define sccpmsg_sccp_to_gapi_lamp_mode(a)       sccpmsg_sccp_to_gapi_enum(a)
#define sccpmsg_sccp_to_gapi_processing_mode(a) sccpmsg_sccp_to_gapi_enum(a)

gapi_user_and_device_data_t *sccpmsg_sccp_to_gapi_data(sccpmsg_user_and_device_data_t *data);

#if 0
typedef struct sccpmsg_set_hkf_detect_t_ {
    unsigned long message_id;
    // TODO:
    // No details available
} sccpmsg_set_hkf_detect_t;

typedef struct sccpmsg_start_media_reception_t_ {
    unsigned long message_id;
    // TODO:
    // No details available
} sccpmsg_start_media_reception_t;

typedef struct sccpmsg_stop_media_reception_t_ {
    unsigned long message_id;
    // TODO:
    // No details available
} sccpmsg_stop_media_reception_t;

typedef struct sccpmsg_reserved_for_future_use_t_ {
    unsigned long message_id;
} sccpmsg_reserved_for_future_use_t;
#endif

#endif /* _SCCPMSG_H_ */
