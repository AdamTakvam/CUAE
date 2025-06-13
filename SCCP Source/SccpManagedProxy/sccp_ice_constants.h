/*
 *  Copyright (c) 2003 by Cisco Systems, Inc. All Rights Reserved.
 */
#ifndef _SCCP_CONSTANTS_H_
#define _SCCP_CONSTANTS_H_


// SCCP Specific
#define SCCP_MAX_LINES								(8)
#define SCCP_MAX_DIRECTORY_NUMBER_SIZE				(24)
#define SCCP_MAX_USER_DEVICE_DATA_SIZE				(1024)


enum SCCP_KEYPAD_BUTTON
{
    SCCP_KEYPAD_BUTTON_ZERO                         = 0x0,
    SCCP_KEYPAD_BUTTON_ONE                          = 0x1,
    SCCP_KEYPAD_BUTTON_TWO                          = 0x2,
    SCCP_KEYPAD_BUTTON_THREE                        = 0x3,
    SCCP_KEYPAD_BUTTON_FOUR                         = 0x4,
    SCCP_KEYPAD_BUTTON_FIVE                         = 0x5,
    SCCP_KEYPAD_BUTTON_SIX                          = 0x6,
    SCCP_KEYPAD_BUTTON_SEVEN                        = 0x7,
    SCCP_KEYPAD_BUTTON_EIGHT                        = 0x8,
    SCCP_KEYPAD_BUTTON_NINE                         = 0x9,
    SCCP_KEYPAD_BUTTON_A                            = 0xa,
    SCCP_KEYPAD_BUTTON_B                            = 0xb,
    SCCP_KEYPAD_BUTTON_C                            = 0xc,
    SCCP_KEYPAD_BUTTON_D                            = 0xd,
    SCCP_KEYPAD_BUTTON_STAR                         = 0xe,
    SCCP_KEYPAD_BUTTON_POUND                        = 0xf,
    SCCP_KEYPAD_BUTTON_INVALID
};
#ifdef SCCP_SAPP
typedef enum SCCP_KEYPAD_BUTTON SCCP_KEYPAD_BUTTON;
#endif

enum SCCP_MEDIA_PAYLOAD_TYPE
{
    SCCP_MEDIA_PAYLOAD_NON_STANDARD                 = 1,
    SCCP_MEDIA_PAYLOAD_G711_ALAW_64K                = 2,
    SCCP_MEDIA_PAYLOAD_G711_ALAW_56K                = 3,      // "RESTRICTED"
    SCCP_MEDIA_PAYLOAD_G711_ULAW_64K                = 4,
    SCCP_MEDIA_PAYLOAD_G711_ULAW_56K                = 5,      // "RESTRICTED"
    SCCP_MEDIA_PAYLOAD_G722_64K                     = 6,
    SCCP_MEDIA_PAYLOAD_G722_56K                     = 7,
    SCCP_MEDIA_PAYLOAD_G722_48K                     = 8,
    SCCP_MEDIA_PAYLOAD_G7231                        = 9,
    SCCP_MEDIA_PAYLOAD_G728                         = 10,
    SCCP_MEDIA_PAYLOAD_G729                         = 11,
    SCCP_MEDIA_PAYLOAD_G729_ANNEX_A                 = 12,
    SCCP_MEDIA_PAYLOAD_IS11172_AUDIO_CAP            = 13,
    SCCP_MEDIA_PAYLOAD_IS13818_AUDIO_CAP            = 14,
    SCCP_MEDIA_PAYLOAD_G729_ANNEX_B                 = 15,
    SCCP_MEDIA_PAYLOAD_G729_ANNEX_AW_ANNEX_B        = 16,
    SCCP_MEDIA_PAYLOAD_GSM_FULL_RATE                = 18,
    SCCP_MEDIA_PAYLOAD_GSM_HALF_RATE                = 19,
    SCCP_MEDIA_PAYLOAD_GSM_ENHANCED_FULL_RATE       = 20,
    SCCP_MEDIA_PAYLOAD_WIDE_BAND_256K               = 25,
    SCCP_MEDIA_PAYLOAD_DATA_64                      = 32,
    SCCP_MEDIA_PAYLOAD_DATA_56                      = 33,
    SCCP_MEDIA_PAYLOAD_GSM                          = 80,
    SCCP_MEDIA_PAYLOAD_ACTIVE_VOICE                 = 81,
    SCCP_MEDIA_PAYLOAD_G726_32K                     = 82,
    SCCP_MEDIA_PAYLOAD_G726_24K                     = 83,
    SCCP_MEDIA_PAYLOAD_G726_16K                     = 84,
    SCCP_MEDIA_PAYLOAD_G729_B                       = 85,
    SCCP_MEDIA_PAYLOAD_G729_B_LOW_COMPLEXITY        = 86,
    SCCP_MEDIA_PAYLOAD_INVALID
};
#ifdef SCCP_SAPP
typedef enum SCCP_MEDIA_PAYLOAD_TYPE SCCP_MEDIA_PAYLOAD_TYPE;
#endif


#endif