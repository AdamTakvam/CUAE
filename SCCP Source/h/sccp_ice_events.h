/*
 *  Copyright (c) 2003, 2004 by Cisco Systems, Inc. All Rights Reserved.
 */
#ifndef _SCCP_EVENTS_H_
#define _SCCP_EVENTS_H_


#include <sccp_ice_constants.h>


enum SCCP_FEATURE
{
	SCCP_FEATURE_NONE,
	SCCP_FEATURE_DIGIT,
	SCCP_FEATURE_HOLD,
	SCCP_FEATURE_RESUME,
	SCCP_FEATURE_CONFERENCE,
	SCCP_FEATURE_ANSWER,
	SCCP_FEATURE_END_CALL,
	SCCP_FEATURE_REDIAL,
	SCCP_FEATURE_CALL_FORWARD_ALL,
	SCCP_FEATURE_DEVICE_DATA,
	SCCP_FEATURE_USER_DATA_RESPONSE,
    SCCP_FEATURE_SPEEDDIAL,
    SCCP_FEATURE_STARTTONE,
    SCCP_FEATURE_TRANSFER,
	SCCP_FEATURE_INVALID
};
#ifdef SCCP_SAPP
typedef enum SCCP_FEATURE SCCP_FEATURE;
#endif


enum SCCP_STACK_EVENT
{
	SCCP_STACK_EVENT_CM_CONNECTED,
	SCCP_STACK_EVENT_CM_DISCONNECTED,
	SCCP_STACK_EVENT_OFFHOOK,
	SCCP_STACK_EVENT_CALL_SETUP,
	SCCP_STACK_EVENT_CALL_SETUP_ACK,
	SCCP_STACK_EVENT_CALL_RELEASE,
	SCCP_STACK_EVENT_CALL_ALERTING,
	SCCP_STACK_EVENT_CALL_PROCEEDING,
	SCCP_STACK_EVENT_CALL_CONNECTED,
	SCCP_STACK_EVENT_CALL_STATE,
	SCCP_STACK_EVENT_CALL_INFO,
	SCCP_STACK_EVENT_FEATURE,
	SCCP_STACK_EVENT_MEDIA_RECEIVE_START,
	SCCP_STACK_EVENT_MEDIA_RECEIVE_STOP,
	SCCP_STACK_EVENT_MEDIA_TRANSMIT_START,
	SCCP_STACK_EVENT_MEDIA_TRANSMIT_STOP,
	SCCP_STACK_EVENT_INVALID
};
#ifdef SCCP_SAPP
typedef enum SCCP_STACK_EVENT SCCP_STACK_EVENT;
#endif

enum SCCP_APP_EVENT
{
	SCCP_APP_EVENT_CALL_SETUP,
	SCCP_APP_EVENT_CALL_RELEASE,
	SCCP_APP_EVENT_OFFHOOK,
	SCCP_APP_EVENT_FEATURE,
	SCCP_APP_EVENT_SELECT_LINE,
	SCCP_APP_EVENT_MEDIA_RECEIVE_START_ACK,
	SCCP_APP_EVENT_MEDIA_RECEIVE_STOP_ACK,
	SCCP_APP_EVENT_MEDIA_TRANSMIT_START_ACK,
	SCCP_APP_EVENT_MEDIA_TRANSMIT_STOP_ACK,
	SCCP_APP_EVENT_INVALID
};


struct sccp_setup_data
{
	char								dialstring[SCCP_MAX_DIRECTORY_NUMBER_SIZE];
};
#ifdef SCCP_SAPP
typedef struct sccp_setup_data sccp_setup_data;
#endif

struct sccp_feature_user_data
{
	unsigned long						data_len;
	char								data[SCCP_MAX_USER_DEVICE_DATA_SIZE];
	unsigned long						application_id;
	unsigned long						transaction_id;
};
#ifdef SCCP_SAPP
typedef struct sccp_feature_user_data sccp_feature_user_data;
#endif


struct sccp_media_info
{
	unsigned long						ip_address;
	unsigned short						port;
	SCCP_MEDIA_PAYLOAD_TYPE				payload;
	unsigned long						packetization_size_bytes;
	unsigned long						packetization_samples;
	unsigned long						packetization_time_ms;

};
#ifdef SCCP_SAPP
typedef struct sccp_media_info sccp_media_info;
#endif

struct sccp_event_data
{
	unsigned long						line_number;
	unsigned long						call_reference;
	SCCP_FEATURE						type;
	union
	{
		//SCCP_KEYPAD_BUTTON				digit_data;
        char                            digit_data;  
		sccp_feature_user_data			user_data;
		sccp_setup_data					setup_data;
		sccp_media_info					media_info;
		unsigned long					activate_line;
        int                             tone_data;
	} body;
};

#ifdef SCCP_SAPP
#endif

#endif