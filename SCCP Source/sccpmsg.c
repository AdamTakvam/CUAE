/*
 *  Copyright (c) 2002, 2003 by Cisco Systems, Inc. All Rights Reserved.
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
 *     Skinny Client Control Protocol Messaging implementation
 *
 *  NOTES
 *     1. Version: application will specify the version it desires.
 *        The CCM will return a version it expects for the client, that is:
 *        less - client should assume this version and ignore any messages
 *               not meant for the version.
 *        same - no problems,
 *        or greater - client can ignore or upgrade. Our client will ignore.
 */
#include "sccp.h"
#include "sccpmsg.h"
#include "sccp_debug.h"


#define SCCPMSG_RESERVED            (0x00000000)
#define SCCPMSG_SID_RESERVED        (0x00000000)
#define SCCPMSG_SID_INSTANCE        (0x00000001)
#define SCCPMSG_INTERNAL_VERSION(a) (0x000000ff & (a))


#if 0 /* Only use if a single instance of PSCCP is used. */
/*
 * sccpmsg_gen_msg is a single buffer that will be used to send all
 * messages. This implies, that there is only one task that is calling
 * the send routines. Currently, this is the case, since all event
 * processing is completed by the SCCP task.
 */
static sccpmsg_general_t sccpmsg_gen_msg;
#endif

char *sccpmsg_alarm_names[] = {
	"Load Rejected HC",
	"TFTP Size Error",
	"Compression Type Error",
    "Version Error",
	"Disk Full Error",
	"ROM Checksum Error",
	"File Not Found",
	"TFTP Timeout",
	"TFTP Access Error",
	"TFTP Error",
	"TCP-timeout",
	"TCP-Bad-ACK",
	"CM-reset-TCP",
	"CM-aborted-TCP",
	"CM-closed-TCP",
    "CM-ICMP-Unreach",
    "CM-NAKed",
    "KeepaliveTO",
    "Failback",
    "Phone-Diag",
    "Phone-Keypad",
    "Phone-Re-IP",
    "Reset-Reset",
    "Reset-Restart",
    "Phone-Reg-Rej",
    "Initialized",
	"Last=0x%02X",
	"Waiting for %s From %s",
	"Waiting for state %d response from %s",
	"DSP Error",
	"Phone-Abort",
	"FileAuthError"
};

typedef struct sccpmsg_enum_to_text_entry_t_ {
    unsigned long id;
    const char    *text;
} sccpmsg_enum_to_text_entry_t;


static sccpmsg_enum_to_text_entry_t sccpmsg_message_text_table[] =
{
    {SCCPMSG_KEEPALIVE,                          "KEEPALIVE"},
    {SCCPMSG_REGISTER,                           "REGISTER"},
    {SCCPMSG_IP_PORT,                            "IP_PORT"},
    {SCCPMSG_KEYPAD_BUTTON,                      "KEYPAD_BUTTON"},
    {SCCPMSG_ENBLOC_CALL,                        "ENBLOC_CALL"},
    {SCCPMSG_STIMULUS,                           "STIMULUS"},
    {SCCPMSG_OFFHOOK,                            "OFFHOOK"},
    {SCCPMSG_ONHOOK,                             "ONHOOK"},
    {SCCPMSG_HOOK_FLASH,                         "HOOK_FLASH"},
    {SCCPMSG_FORWARD_STAT_REQ,                   "FORWARD_STAT_REQ"},
    {SCCPMSG_SPEEDDIAL_STAT_REQ,                 "SPEEDDIAL_STAT_REQ"},
    {SCCPMSG_LINE_STAT_REQ,                      "LINE_STAT_REQ"},
    {SCCPMSG_CONFIG_STAT_REQ,                    "CONFIG_STAT_REQ"},
    {SCCPMSG_TIME_DATE_REQ,                      "TIME_DATE_REQ"},
    {SCCPMSG_BUTTON_TEMPLATE_REQ,                "BUTTON_TEMPLATE_REQ"},
    {SCCPMSG_VERSION_REQ,                        "VERSION_REQ"},
    {SCCPMSG_CAPABILITIES_RES,                   "CAPABILITIES_RES"},
    {SCCPMSG_MEDIA_PORT_LIST,                    "MEDIA_PORT_LIST"},
    {SCCPMSG_SERVER_REQ,                         "SERVER_REQ"},
    {SCCPMSG_ALARM,                              "ALARM"},
    {SCCPMSG_MULTICAST_MEDIA_RECEPTION_ACK,      "MULTICAST_MEDIA_RECV_ACK"},
    {SCCPMSG_OPEN_RECEIVE_CHANNEL_ACK,           "OPEN_RECV_CHANNEL_ACK"},
    {SCCPMSG_CONNECTION_STATISTICS_RES,          "CONNECTION_STATISTICS_RES"},
    {SCCPMSG_OFFHOOK_WITH_CGPN,                  "OFFHOOK_WITH_CGPN"},
    {SCCPMSG_SOFTKEY_SET_REQ,                    "SOFTKEY_SET_REQ"},
    {SCCPMSG_SOFTKEY_EVENT,                      "SOFTKEY_EVENT"},
    {SCCPMSG_UNREGISTER,                         "UNREGISTER"},
    {SCCPMSG_SOFTKEY_TEMPLATE_REQ,               "SOFTKEY_TEMPLATE_REQ"},
    {SCCPMSG_REGISTER_TOKEN_REQ,                 "REGISTER_TOKEN_REQ"},
    {SCCPMSG_MEDIA_TRANSMISSION_FAILURE,         "MEDIA_XMIT_FAILURE"},
    {SCCPMSG_HEADSET_STATUS,                     "HEADSET_STATUS"},
    {SCCPMSG_MEDIA_RESOURCE_NOTIFICATION,        "MEDIA_RESOURCE_NOTIFICATION"},
    {SCCPMSG_REGISTER_AVAILABLE_LINES,           "REGISTER_AVAILABLE_LINES"},
    {SCCPMSG_DEVICE_TO_USER_DATA,                "DEVICE_TO_USER_DATA"},
    {SCCPMSG_DEVICE_TO_USER_DATA_RESPONSE,       "DEVICE_TO_USER_DATA_RESP"},

    {SCCPMSG_SERVICE_URL_STAT_REQ,               "SERVICE_URL_STAT_REQ"},
    {SCCPMSG_FEATURE_STAT_REQ,                   "FEATURE_STAT_REQ"},

    {SCCPMSG_REGISTER_ACK,                       "REGISTER_ACK"},
    {SCCPMSG_START_TONE,                         "START_TONE"},
    {SCCPMSG_STOP_TONE,                          "STOP_TONE"},
    {SCCPMSG_SET_RINGER,                         "SET_RINGER"},
    {SCCPMSG_SET_LAMP,                           "SET_LAMP"},
//    {SCCPMSG_SET_HKF_DETECT,                     "SET_HKF_DETECT"},
    {SCCPMSG_SET_SPEAKER_MODE,                   "SET_SPEAKER_MODE"},
    {SCCPMSG_SET_MICRO_MODE,                     "SET_MICRO_MODE"},
    {SCCPMSG_START_MEDIA_TRANSMISSION,           "START_MEDIA_XMIT"},
    {SCCPMSG_STOP_MEDIA_TRANSMISSION,            "STOP_MEDIA_XMIT"},
//    {SCCPMSG_START_MEDIA_RECEPTION,              "START_MEDIA_RECV"},
//    {SCCPMSG_STOP_MEDIA_RECEPTION,               "STOP_MEDIA_RECV"},
//    {SCCPMSG_RESERVED_FOR_FUTURE_USE,            "RESERVED_FOR_FUTURE_USE"},
    {SCCPMSG_CALL_INFO,                          "CALL_INFO"},
    {SCCPMSG_FORWARD_STAT,                       "FORWARD_STAT"},
    {SCCPMSG_SPEEDDIAL_STAT,                     "SPEEDDIAL_STAT"},
    {SCCPMSG_LINE_STAT,                          "LINE_STAT"},
    {SCCPMSG_CONFIG_STAT,                        "CONFIG_STAT"},
    {SCCPMSG_DEFINE_TIME_DATE,                   "DEFINE_TIME_DATE"},
    {SCCPMSG_START_SESSION_TRANSMISSION,         "START_SESSION_XMIT"},
    {SCCPMSG_STOP_SESSION_TRANSMISSION,          "STOP_SESSION_XMIT"},
    {SCCPMSG_BUTTON_TEMPLATE,                    "BUTTON_TEMPLATE"},
    {SCCPMSG_VERSION,                            "VERSION"},
    {SCCPMSG_DISPLAY_TEXT,                       "DISPLAY_TEXT"},
    {SCCPMSG_CLEAR_DISPLAY,                      "CLEAR_DISPLAY"},
    {SCCPMSG_CAPABILITIES_REQ,                   "CAPABILITIES_REQ"},
//    {SCCPMSG_ENUNCIATOR_COMMAND,                 "ENUNCIATOR_COMMAND"},
    {SCCPMSG_REGISTER_REJECT,                    "REGISTER_REJECT"},
    {SCCPMSG_SERVER_RES,                         "SERVER_RES"},
    {SCCPMSG_RESET,                              "RESET"},
    {SCCPMSG_KEEPALIVE_ACK,                      "KEEPALIVE_ACK"},
    {SCCPMSG_START_MULTICAST_MEDIA_RECEPTION,    "START_MULTICAST_MEDIA_RECV"},
    {SCCPMSG_START_MULTICAST_MEDIA_TRANSMISSION, "START_MULTICAST_MEDIA_XMIT"},
    {SCCPMSG_STOP_MULTICAST_MEDIA_RECEPTION,     "STOP_MULTICAST_MEDIA_RECV"},
    {SCCPMSG_STOP_MULTICAST_MEDIA_TRANSMISSION,  "STOP_MULTICAST_MEDIA_XMIT"},
    {SCCPMSG_OPEN_RECEIVE_CHANNEL,               "OPEN_RECEIVE_CHANNEL"},
    {SCCPMSG_CLOSE_RECEIVE_CHANNEL,              "CLOSE_RECEIVE_CHANNEL"},
    {SCCPMSG_CONNECTION_STATISTICS_REQ,          "CONNECTION_STATISTICS_REQ"},
    {SCCPMSG_SOFTKEY_TEMPLATE_RES,               "SOFTKEY_TEMPLATE_RES"},
    {SCCPMSG_SOFTKEY_SET_RES,                    "SOFTKEY_SET_RES"},
    {SCCPMSG_SELECT_SOFTKEYS,                    "SELECT_SOFTKEYS"},
    {SCCPMSG_CALL_STATE,                         "CALL_STATE"},
    {SCCPMSG_DISPLAY_PROMPT_STATUS,              "DISPLAY_PROMPT_STATUS"},
    {SCCPMSG_CLEAR_PROMPT_STATUS,                "CLEAR_PROMPT_STATUS"},
    {SCCPMSG_DISPLAY_NOTIFY,                     "DISPLAY_NOTIFY"},
    {SCCPMSG_CLEAR_NOTIFY,                       "CLEAR_NOTIFY"},
    {SCCPMSG_ACTIVATE_CALL_PLANE,                "ACTIVATE_CALL_PLANE"},
    {SCCPMSG_DEACTIVATE_CALL_PLANE,              "DEACTIVATE_CALL_PLANE"},
    {SCCPMSG_UNREGISTER_ACK,                     "UNREGISTER_ACK"},
    {SCCPMSG_BACKSPACE_REQ,                      "BACKSPACE_REQ"},
    {SCCPMSG_REGISTER_TOKEN_ACK,                 "REGISTER_TOKEN_ACK"},
    {SCCPMSG_REGISTER_TOKEN_REJECT,              "REGISTER_TOKEN_REJECT"},
    {SCCPMSG_START_MEDIA_FAILURE_DETECTION,      "START_MEDIA_FAILURE_DETECTION"},
    {SCCPMSG_DIALED_NUMBER,                      "DIALED_NUMBER"},
    {SCCPMSG_USER_TO_DEVICE_DATA,                "USER_TO_DEVICE_DATA"},
    {SCCPMSG_DISPLAY_PRIORITY_NOTIFY,            "DISPLAY_PRIORITY_NOTIFY"},
    {SCCPMSG_CLEAR_PRIORITY_NOTIFY,              "CLEAR_PRIORITY_NOTIFY"},
    {SCCPMSG_FEATURE_STAT,                       "FEATURE_STAT"},
    {SCCPMSG_SERVICE_URL_STAT,                   "SERVICE_URL_STAT"},
    {SCCPMSG_INVALID,                            NULL}
};

static const char *sccpmsg_get_table_text (sccpmsg_enum_to_text_entry_t *table,
                                           unsigned long index)
{
    sccpmsg_enum_to_text_entry_t *walker = table;

    while ((walker != NULL) && (walker->text != NULL)) {
        if (walker->id == index) {
            return (walker->text);
        }

        walker++;
    }

    return ("");
}

const char *sccpmsg_get_message_text (unsigned long index)
{
    return (sccpmsg_get_table_text(sccpmsg_message_text_table, index));
}

char *sccpmsg_alarm_name (int id)
{
    if ((id <= SCCPMSG_ALARM_LAST_MIN) || (id >= SCCPMSG_ALARM_LAST_MAX)) {
        return (SCCP_UNDEFINED);
    }

    /*
     * We map the internal keypad causes to the one external keypad string.
     */
    if ((id >= SCCPMSG_ALARM_LAST_KEYPAD_CLOSE) &&
        (id <= SCCPMSG_ALARM_LAST_KEYPAD_RESET)) {
        id = SCCPMSG_ALARM_LAST_KEYPAD;
    }

    return (sccpmsg_alarm_names[id]);
}

#ifdef SCCP_MSG_DUMP
void sccpmsg_dump (sccpmsg_general_t *gen_msg)
{
    int i, j;
    int msg_len;
    unsigned char *c;

    //if ((sccpmsg_debug <= 0) || (5 < sccpmsg_debug)) {
    if ((sccpmsg_debug <= 0) || (0 < sccpmsg_debug)) {
        return;
    }

    j = 0;
    c = (unsigned char *)gen_msg;
    
    if (CMTOSL(gen_msg->base.length) > 100) {
        msg_len = 100;
    }
    else {
        msg_len = (int)(CMTOSL(gen_msg->base.length));
    }

    for (i = 0; i < msg_len + (int)(sizeof(gen_msg->base)); i++) {
        printf("%02x", *c++&0xFF);
        
        ++j;

        if ((j % 32) == 0) {
            printf("\n");
        }
        else if ((j % 16) == 0) {
            printf("--");
        }

        else if ((j % 4) == 0) {
            printf(" ");
        }
    }
    printf("\n");
#if 0
    switch (gen_msg->body.msg_id.message_id) {
    case (SCCPMSG_BUTTON_TEMPLATE): {
        sccpmsg_button_template_t *msg = &(gen_msg->body.button_template);
        unsigned long i;
        
        printf("\n\n    button_offset= %d, button_count= %d, "
               "total_button_count= %d\n",
               msg->button_template.button_offset,
               msg->button_template.button_count,
               msg->button_template.total_button_count);
                
        for (i = 0; i < msg->button_template.button_count; i++) {
            if ((i % 8) == 0) {
                printf("\n    ");
            }

            printf("%02d:%02d  ",
                   msg->button_template.buttons[i].instance,
                   msg->button_template.buttons[i].definition);
        }
        printf("\n");

        break;
    }

    case (SCCPMSG_SOFTKEY_TEMPLATE_RES): {
        sccpmsg_softkey_template_res_t *msg = &(gen_msg->body.softkey_template_res);
        unsigned long i;
        
        printf("\n\n    offset= %d, count= %d, total_count= %d\n",
               msg->softkey_template.offset, msg->softkey_template.count,
               msg->softkey_template.total_count);
                
        for (i = 0; i < msg->softkey_template.count; i++) {
            if ((i % 4) == 0) {
                printf("\n    ");
            }

            printf("%04x:%-12s  ",
                   msg->softkey_template.definition[i].event,
                   msg->softkey_template.definition[i].label);
        }
        printf("\n");

        break;
    }

    case (SCCPMSG_SOFTKEY_SET_RES): {
        sccpmsg_softkey_set_res_t *msg = &(gen_msg->body.softkey_set_res);
        unsigned long i, j;
        
        printf("    offset= %d, count= %d, "
               "total_count= %d",
               msg->softkey_set.offset, msg->softkey_set.count,
               msg->softkey_set.total_count);
                
        for (i = 0; i < msg->softkey_set.count; i++) {
            printf("\n    [%02d] ", i);

            for (j = 0; j < SCCPMSG_MAX_SOFTKEY_INDEXES; j++) {
                if ((j > 0) && ((j % 8) == 0)) {
                    printf("\n         ");
                }

                printf("%02x:%04x  ",
                       msg->softkey_set.definition[i].template_index[j],
                       msg->softkey_set.definition[i].info_index[j]);
            }  

        }
        printf("\n");

        break;
    }

    default:
        break;
    }
#endif
}
#endif
#if 0 /* NOT_USED */
char *sccpmsg_get_mac_addr_str (char *addr)
{
    char mac[6];
    
    memcpy(mac, SCCP_SOCKET_GETMAC(0), 6);
    
    sprintf(addr, "%.4x%.4x%.4x",
            mac[0]*256+mac[1], mac[2]*256+mac[3], mac[4]*256+mac[5]);
    
    return addr;        
}
#endif
int sccpmsg_send_message (sccpmsg_msgcb_t *msgcb, unsigned int socket,
                          sccpmsg_general_t *msg, unsigned long length)
{
    char *fname = "send_message";
    int  err    = 0;

    if (socket == 0) {
        SCCP_DBG((sccpmsg_debug, 9, "\n"));
        SCCP_DBG((sccpmsg_debug, 9,
                 "%s      : %-20s: [%d] STATUS: Invalid socket - "
                 "No message will be sent.\n",
                 SCCPMSG_ID, fname, socket));
    
        SCCP_FREE(msg);

        return (1);
    }

    if (sccp_debug_show(NULL, STOCML(msg->body.msg_id.message_id)) == 1) {
        SCCP_DBG((sccpmsg_debug, 9,
                 "%s      : %-20s: [%d] %-8s -> %-25s\n",
                 SCCPMSG_ID, fname, socket,
                 msgcb->sem_name,
                 sccpmsg_get_message_text(STOCML(msg->body.msg_id.message_id))));
    }

#ifdef SCCP_MSG_DUMP
    sccpmsg_dump(msg);
#endif

    err = SCCP_SOCKET_SEND(socket,
                           (char *)msg,
                           length + sizeof(sccpmsg_base_t));
    if (err != 0) {
        SCCP_DBG((sccpmsg_debug, 9,
                 "%s      : %-20s: [%d] ERROR= %d\n",
                 SCCPMSG_ID, fname, socket, err));
#if 0
        {
            sccpcm_cmcb_t *cmcb;
            /*
             * Notify sccpcm of the error.
             */
            cmcb = sccpcm_get_cmcb_by_socket2(socket);
            if (cmcb != NULL) {
                sccpcm_push_error(cmcb->sccpcb, SCCPCM_E_ERROR,
                                  cmcb->index, SCCPCM_ERROR_TCP_CLOSE);
            }
        }
#endif
    }
    else {
        err = 0;
    }

    SCCP_FREE(msg);

    return (err);
}

sccpmsg_general_t *sccpmsg_get_gen_msg (sccpmsg_msgcb_t *msgcb,
                                        unsigned int length,
                                        sccpmsg_messages_t msg_id)
{
    sccpmsg_general_t *gen_msg;

    gen_msg = (sccpmsg_general_t *)SCCP_MALLOC(length + SCCPMSG_BASE_LEN);
    if (gen_msg != NULL) {
        /* Do we need to memset it? Shouldn't need to. */

        gen_msg->base.length = STOCML(length);
        gen_msg->base.type   = STOCML(SCCPMSG_RESERVED);
        gen_msg->body.msg_id.message_id = STOCML(msg_id);
    }

    return (gen_msg);
}
                       
#if 0
int sccpmsg_send_bare_message (sccpmsg_msgcb_t *msgcb, unsigned int socket,
                               sccpmsg_messages_t msg_id)
{
    sccpmsg_message_id_t *data = &(sccpmsg_gen_msg.body.msg_id);

    sccpmsg_gen_msg.base.length = STOCML(sizeof(*data));
    sccpmsg_gen_msg.base.type   = STOCML(SCCPMSG_RESERVED);
    
    data->message_id = STOCML(msg_id);

    return (sccpmsg_send_message(msgcb, socket, &sccpmsg_gen_msg, sizeof(*data)));
}
#endif
int sccpmsg_send_bare_message (sccpmsg_msgcb_t *msgcb, unsigned int socket,
                               sccpmsg_messages_t msg_id)
{
    sccpmsg_general_t *gen_msg;

    gen_msg = sccpmsg_get_gen_msg(msgcb, sizeof(sccpmsg_message_id_t), msg_id);
    if (gen_msg == NULL) {
        return (1);
    }

    return (sccpmsg_send_message(msgcb, socket, gen_msg,
            sizeof(sccpmsg_message_id_t)));
}

int sccpmsg_send_register_token_req (sccpmsg_msgcb_t *msgcb,
                                     unsigned int socket,
                                     char *device_name,
                                     unsigned long device_type,
                                     unsigned long addr)
{
    sccpmsg_general_t            *gen_msg;
    sccpmsg_register_token_req_t *data;

    gen_msg = sccpmsg_get_gen_msg(msgcb, sizeof(sccpmsg_register_token_req_t),
                                  SCCPMSG_REGISTER_TOKEN_REQ);
    if (gen_msg == NULL) {
        return (1);
    }

    data = &(gen_msg->body.register_token_req);
    
    data->sid.reserved = STOCML(SCCPMSG_SID_RESERVED);
    data->sid.instance = STOCML(SCCPMSG_SID_INSTANCE);
    data->ip_address   = addr;
    data->device_type  = STOCML(device_type);
    
    SCCP_STRNCPY(data->sid.device_name, device_name,
                 sizeof(data->sid.device_name));
    data->sid.device_name[sizeof(data->sid.device_name) - 1] = '\0';
    
    return (sccpmsg_send_message(msgcb, socket, gen_msg, sizeof(*data)));
}

/*
 * addr goes out in network order. IP addresses are always passed in
 * big-endian through the stack, so there is no need to convert it.
 */
int sccpmsg_send_register (sccpmsg_msgcb_t *msgcb, unsigned int socket,
                           char *device_name,
                           unsigned long device_type,
                           unsigned long version, unsigned long addr)
{
    sccpmsg_general_t  *gen_msg;
    sccpmsg_register_t *data;

    gen_msg = sccpmsg_get_gen_msg(msgcb, sizeof(sccpmsg_register_t),
                                  SCCPMSG_REGISTER);
    if (gen_msg == NULL) {
        return (1);
    }

    data = &(gen_msg->body.reg);

    data->sid.reserved       = STOCML(SCCPMSG_RESERVED);
    data->sid.instance       = STOCML(SCCPMSG_SID_INSTANCE);
    data->ip_address         = addr;
    data->device_type        = STOCML(device_type);
    data->max_streams        = STOCML(SCCPMSG_MAX_RTP_STREAMS);
    data->active_streams     = STOCML(0);
    data->protocol_version   = STOCML(version);
    data->max_conferences    = STOCML(0);
    data->active_conferences = STOCML(0);

    SCCP_STRNCPY(data->sid.device_name, device_name,
                 sizeof(data->sid.device_name));
    data->sid.device_name[sizeof(data->sid.device_name) - 1] = '\0';
    
    return (sccpmsg_send_message(msgcb, socket, gen_msg, sizeof(*data)));
}

int sccpmsg_send_ip_port (sccpmsg_msgcb_t *msgcb, unsigned int socket,
                          unsigned long port)
{
    sccpmsg_general_t *gen_msg;
    sccpmsg_ip_port_t *data;

    gen_msg = sccpmsg_get_gen_msg(msgcb, sizeof(sccpmsg_ip_port_t),
                                  SCCPMSG_IP_PORT);
    if (gen_msg == NULL) {
        return (1);
    }

    data = &(gen_msg->body.ip_port);

    data->port = STOCML(port);
    
    return (sccpmsg_send_message(msgcb, socket, gen_msg, sizeof(*data)));
}

int sccpmsg_send_keypress (sccpmsg_msgcb_t *msgcb, unsigned int socket,
                           sccpmsg_keypad_buttons_t button,
                           unsigned long line_number,
                           unsigned long call_reference)
{
    sccpmsg_general_t       *gen_msg;
    sccpmsg_keypad_button_t *data;

    gen_msg = sccpmsg_get_gen_msg(msgcb, sizeof(sccpmsg_keypad_button_t),
                                  SCCPMSG_KEYPAD_BUTTON);
    if (gen_msg == NULL) {
        return (1);
    }

    data = &(gen_msg->body.keypad_button);

    data->button         = STOCML(button);
    data->line_number    = STOCML(line_number);
    data->call_reference = STOCML(call_reference);
    
    return (sccpmsg_send_message(msgcb, socket, gen_msg, sizeof(*data)));
}

int sccpmsg_send_enbloc (sccpmsg_msgcb_t *msgcb, unsigned int socket,
                         char *dialstring)
{
    sccpmsg_general_t     *gen_msg;
    sccpmsg_enbloc_call_t *data;

    gen_msg = sccpmsg_get_gen_msg(msgcb, sizeof(sccpmsg_enbloc_call_t),
                                  SCCPMSG_ENBLOC_CALL);
    if (gen_msg == NULL) {
        return (1);
    }

    data = &(gen_msg->body.enbloc_call);

#if 0
    if (dialstring == NULL) {
        return (1);        
    }
#endif
    SCCP_STRNCPY(data->called_party, dialstring, sizeof(data->called_party));
    data->called_party[sizeof(data->called_party) - 1] = '\0';
        
    return (sccpmsg_send_message(msgcb, socket, gen_msg, sizeof(*data)));
}

int sccpmsg_send_stimulus (sccpmsg_msgcb_t *msgcb, unsigned int socket,
                           sccpmsg_device_stimulus_t type,
                           unsigned long instance,
                           unsigned long call_reference)
{
    sccpmsg_general_t  *gen_msg;
    sccpmsg_stimulus_t *data;

    gen_msg = sccpmsg_get_gen_msg(msgcb, sizeof(sccpmsg_stimulus_t),
                                  SCCPMSG_STIMULUS);
    if (gen_msg == NULL) {
        return (1);
    }

    data = &(gen_msg->body.stimulus);

    data->type           = STOCML(type);
    data->instance       = STOCML(instance);
    data->call_reference = STOCML(call_reference);
    
    return (sccpmsg_send_message(msgcb, socket, gen_msg, sizeof(*data)));
}

int sccpmsg_send_offhook (sccpmsg_msgcb_t *msgcb, unsigned int socket,
                          unsigned long line_number,
                          unsigned long call_reference)
{
    sccpmsg_general_t *gen_msg;
    sccpmsg_offhook_t *data;

    gen_msg = sccpmsg_get_gen_msg(msgcb, sizeof(sccpmsg_offhook_t),
                                  SCCPMSG_OFFHOOK);
    if (gen_msg == NULL) {
        return (1);
    }

    data = &(gen_msg->body.offhook);

    data->line_number    = STOCML(line_number);
    data->call_reference = STOCML(call_reference);
    
    return (sccpmsg_send_message(msgcb, socket, gen_msg, sizeof(*data)));
}

int sccpmsg_send_onhook (sccpmsg_msgcb_t *msgcb, unsigned int socket,
                         unsigned long line_number,
                         unsigned long call_reference)
{
    sccpmsg_general_t *gen_msg;
    sccpmsg_onhook_t  *data;

    gen_msg = sccpmsg_get_gen_msg(msgcb, sizeof(sccpmsg_onhook_t),
                                  SCCPMSG_ONHOOK);
    if (gen_msg == NULL) {
        return (1);
    }

    data = &(gen_msg->body.onhook);

    data->line_number    = STOCML(line_number);
    data->call_reference = STOCML(call_reference);
    
    return (sccpmsg_send_message(msgcb, socket, gen_msg, sizeof(*data)));
}

int sccpmsg_send_hook_flash (sccpmsg_msgcb_t *msgcb, unsigned int socket,
                             unsigned long line_number,
                             unsigned long call_reference)
{
    sccpmsg_general_t    *gen_msg;
    sccpmsg_hook_flash_t *data;

    gen_msg = sccpmsg_get_gen_msg(msgcb, sizeof(sccpmsg_hook_flash_t),
                                  SCCPMSG_HOOK_FLASH);
    if (gen_msg == NULL) {
        return (1);
    }

    data = &(gen_msg->body.hook_flash);

    data->line_number    = STOCML(line_number);
    data->call_reference = STOCML(call_reference);
    
    return (sccpmsg_send_message(msgcb, socket, gen_msg, sizeof(*data)));
}

int sccpmsg_send_forward_stat_req (sccpmsg_msgcb_t *msgcb,
                                   unsigned int socket,
                                   unsigned long line)
{
    sccpmsg_general_t      *gen_msg;
    sccpmsg_forward_stat_t *data;

    gen_msg = sccpmsg_get_gen_msg(msgcb, sizeof(sccpmsg_forward_stat_t),
                                  SCCPMSG_FORWARD_STAT_REQ);
    if (gen_msg == NULL) {
        return (1);
    }

    data = &(gen_msg->body.forward_stat);

    data->line_number = STOCML(line);
    
    return (sccpmsg_send_message(msgcb, socket, gen_msg, sizeof(*data)));
}

int sccpmsg_send_speeddial_stat_req (sccpmsg_msgcb_t *msgcb,
                                     unsigned int socket,
                                     unsigned long line)
{
    sccpmsg_general_t            *gen_msg;
    sccpmsg_speeddial_stat_req_t *data;

    gen_msg = sccpmsg_get_gen_msg(msgcb, sizeof(sccpmsg_speeddial_stat_req_t),
                                  SCCPMSG_SPEEDDIAL_STAT_REQ);
    if (gen_msg == NULL) {
        return (1);
    }

    data     = &(gen_msg->body.speeddial_stat_req);

    data->speeddial_number = STOCML(line);
    
    return (sccpmsg_send_message(msgcb, socket, gen_msg, sizeof(*data)));
}

int sccpmsg_send_line_stat_req (sccpmsg_msgcb_t *msgcb,
                                unsigned int socket,
                                unsigned long line)
{
    sccpmsg_general_t       *gen_msg;
    sccpmsg_line_stat_req_t *data;

    gen_msg = sccpmsg_get_gen_msg(msgcb, sizeof(sccpmsg_line_stat_req_t),
                                  SCCPMSG_LINE_STAT_REQ);
    if (gen_msg == NULL) {
        return (1);
    }

    data = &(gen_msg->body.line_stat_req);

    data->line_number = STOCML(line);
    
    return (sccpmsg_send_message(msgcb, socket, gen_msg, sizeof(*data)));
}

int sccpmsg_send_feature_stat_req (sccpmsg_msgcb_t *msgcb,
                                   unsigned int socket,
                                   unsigned long line)
{
    sccpmsg_general_t          *gen_msg;
    sccpmsg_feature_stat_req_t *data;

    gen_msg = sccpmsg_get_gen_msg(msgcb, sizeof(sccpmsg_feature_stat_req_t),
                                  SCCPMSG_FEATURE_STAT_REQ);
    if (gen_msg == NULL) {
        return (1);
    }

    data = &(gen_msg->body.feature_stat_req);

    data->feature_index = STOCML(line);
    
    return (sccpmsg_send_message(msgcb, socket, gen_msg, sizeof(*data)));
}

int sccpmsg_send_service_url_stat_req (sccpmsg_msgcb_t *msgcb,
                                       unsigned int socket,
                                       unsigned long line)
{
    sccpmsg_general_t              *gen_msg;
    sccpmsg_service_url_stat_req_t *data;

    gen_msg = sccpmsg_get_gen_msg(msgcb, sizeof(sccpmsg_service_url_stat_req_t),
                                  SCCPMSG_SERVICE_URL_STAT_REQ);
    if (gen_msg == NULL) {
        return (1);
    }

    data = &(gen_msg->body.service_url_stat_req);

    data->service_url_index = STOCML(line);
    
    return (sccpmsg_send_message(msgcb, socket, gen_msg, sizeof(*data)));
}

int sccpmsg_send_capabilities_response (sccpmsg_msgcb_t *msgcb,
                                        unsigned int socket,
                                        gapi_media_caps_t *caps)
{
    sccpmsg_general_t          *gen_msg;
    sccpmsg_capabilities_res_t *data;
    int                        i;

    gen_msg = sccpmsg_get_gen_msg(msgcb, sizeof(sccpmsg_capabilities_res_t),
                                  SCCPMSG_CAPABILITIES_RES);
    if (gen_msg == NULL) {
        return (1);
    }

    data = &(gen_msg->body.capabilities_res);

    data->capabilites_count = STOCML(caps->count);
    for (i = 0; i < caps->count; i++) {
        data->capabilities[i].payload_type            = STOCML(caps->caps[i].payload);
        data->capabilities[i].milliseconds_per_packet = STOCML(caps->caps[i].milliseconds_per_packet);
        SCCP_MEMSET(data->capabilities[i].payload_params.future_use, 0, 8);
    }

    return (sccpmsg_send_message(msgcb, socket, gen_msg, sizeof(*data)));
}

/*
 * NOTE: user must ensure that text is NULL-terminated.
 * param2 is the ip address and goes out in network order
 */
int sccpmsg_send_alarm (sccpmsg_msgcb_t *msgcb, unsigned int socket,
                        sccpmsg_alarm_severity_t level,
                        unsigned long param1, unsigned long param2,
                        char *text)
{
    sccpmsg_general_t *gen_msg;
    sccpmsg_alarm_t   *data;

    gen_msg = sccpmsg_get_gen_msg(msgcb, sizeof(sccpmsg_alarm_t),
                                  SCCPMSG_ALARM);
    if (gen_msg == NULL) {
        return (1);
    }

    data = &(gen_msg->body.alarm);
    
    data->alarm_severity = STOCML(level);
    data->param1         = STOCML(param1);
    data->param2         = param2; //STOCML(param2);
    SCCP_STRNCPY(data->text, text, sizeof(data->text));
    data->text[sizeof(data->text) - 1] = '\0';

    return (sccpmsg_send_message(msgcb, socket, gen_msg, sizeof(*data)));
}

int sccpmsg_send_connection_stats (sccpmsg_msgcb_t *msgcb, unsigned int socket,
                                   char *dirnum, 
                                   unsigned long call_reference,
                                   sccpmsg_stats_processing_t mode,
                                   unsigned long num_packets_sent,
                                   unsigned long num_bytes_sent,
                                   unsigned long num_packets_received,
                                   unsigned long num_bytes_received,
                                   unsigned long num_packets_lost,
                                   unsigned long jitter,
                                   unsigned long latency)
{
    sccpmsg_general_t                   *gen_msg;
    sccpmsg_connection_statistics_res_t *data;

    gen_msg = sccpmsg_get_gen_msg(msgcb, sizeof(sccpmsg_connection_statistics_res_t),
                                  SCCPMSG_CONNECTION_STATISTICS_RES);
    if (gen_msg == NULL) {
        return (1);
    }

    data = &(gen_msg->body.connection_statistics_res);

    data->call_reference          = STOCML(call_reference);
    data->processing_mode         = STOCML(mode);
    data->number_packets_sent     = STOCML(num_packets_sent);
    data->number_bytes_sent       = STOCML(num_bytes_sent);
    data->number_packets_received = STOCML(num_packets_received);
    data->number_bytes_received   = STOCML(num_bytes_received);
    data->number_packets_lost     = STOCML(num_packets_lost);
    data->jitter                  = STOCML(jitter);
    data->latency                 = STOCML(latency);

    return (sccpmsg_send_message(msgcb, socket, gen_msg, sizeof(*data)));
}
                                
int sccpmsg_send_softkey_event (sccpmsg_msgcb_t *msgcb, unsigned int socket,
                                int event,
                                unsigned long line,
                                unsigned long call_reference)
{
    sccpmsg_general_t       *gen_msg;
    sccpmsg_softkey_event_t *data;

    gen_msg = sccpmsg_get_gen_msg(msgcb, sizeof(sccpmsg_softkey_event_t),
                                  SCCPMSG_SOFTKEY_EVENT);
    if (gen_msg == NULL) {
        return (1);
    }

    data = &(gen_msg->body.softkey_event);

    data->softkey_event  = STOCML(event);
    data->line_number    = STOCML(line);
    data->call_reference = STOCML(call_reference);
    
    return (sccpmsg_send_message(msgcb, socket, gen_msg, sizeof(*data)));
}
                                
int sccpmsg_send_headset_status (sccpmsg_msgcb_t *msgcb, unsigned int socket,
                                 sccpmsg_headset_statuss_t status)
{
    sccpmsg_general_t        *gen_msg;
    sccpmsg_headset_status_t *data;

    gen_msg = sccpmsg_get_gen_msg(msgcb, sizeof(sccpmsg_headset_status_t),
                                  SCCPMSG_HEADSET_STATUS);
    if (gen_msg == NULL) {
        return (1);
    }

    data = &(gen_msg->body.headset_status);

    data->status     = STOCML(status);
    
    return (sccpmsg_send_message(msgcb, socket, gen_msg, sizeof(*data)));
}
                                
int sccpmsg_send_register_available_lines (sccpmsg_msgcb_t *msgcb,
                                           unsigned int socket,
                                           unsigned long lines)
{
    sccpmsg_general_t                  *gen_msg;
    sccpmsg_register_available_lines_t *data;

    gen_msg = sccpmsg_get_gen_msg(msgcb, sizeof(sccpmsg_register_available_lines_t),
                                  SCCPMSG_REGISTER_AVAILABLE_LINES);
    if (gen_msg == NULL) {
        return (1);
    }

    data = &(gen_msg->body.register_available_lines);

    data->max_number_available_lines = STOCML(lines);
    
    return (sccpmsg_send_message(msgcb, socket, gen_msg, sizeof(*data)));
}

/*
 * address and port are stored in big-endian order by the stack.
 * address goes out in network order and port goes out in CM order. The stack
 * will convert the port to host before calling sccpmsg.
 */
int sccpmsg_send_open_receive_channel_ack (sccpmsg_msgcb_t *msgcb,
                                           unsigned int socket,
                                           unsigned long addr,
                                           unsigned long port,
                                           sccpmsg_orc_status_t status,
                                           unsigned long passthru_party_id,
                                           unsigned long call_reference)
{
    sccpmsg_general_t                  *gen_msg;
    sccpmsg_open_receive_channel_ack_t *data;

    gen_msg = sccpmsg_get_gen_msg(msgcb, sizeof(sccpmsg_open_receive_channel_ack_t),
                                  SCCPMSG_OPEN_RECEIVE_CHANNEL_ACK);
    if (gen_msg == NULL) {
        return (1);
    }

    data = &(gen_msg->body.open_receive_channel_ack);

    data->status            = STOCML(status);
    data->ip_address        = addr;
    data->port              = STOCML(port);
    data->passthru_party_id = STOCML(passthru_party_id);
    data->call_reference    = STOCML(call_reference);
    
    return (sccpmsg_send_message(msgcb, socket, gen_msg, sizeof(*data)));
}

int sccpmsg_send_device_to_user_data (sccpmsg_msgcb_t *msgcb,
                                      unsigned int socket,
                                      char *user_data,
                                      unsigned long len,
                                      unsigned long application_id,
                                      unsigned long transaction_id,
                                      unsigned long line,
                                      unsigned long call_reference)

{
    sccpmsg_general_t             *gen_msg;
    sccpmsg_device_to_user_data_t *data;

    gen_msg = sccpmsg_get_gen_msg(msgcb, sizeof(sccpmsg_device_to_user_data_t),
                                  SCCPMSG_DEVICE_TO_USER_DATA);
    if (gen_msg == NULL) {
        return (1);
    }

    data = &(gen_msg->body.device_to_user_data);

    SCCP_MEMCPY(data->data.data, user_data, len);
    data->data.data_length    = STOCML(len);
    data->data.line_number    = STOCML(line);
    data->data.call_reference = STOCML(call_reference);
    data->data.application_id = STOCML(application_id);
    data->data.transaction_id = STOCML(transaction_id);
    
    return (sccpmsg_send_message(msgcb, socket, gen_msg, sizeof(*data)));
}

int sccpmsg_send_device_to_user_data_response (sccpmsg_msgcb_t *msgcb,
                                               unsigned int socket,
                                               char *user_data,
                                               unsigned long len,
                                               unsigned long application_id,
                                               unsigned long transaction_id,
                                               unsigned long line,
                                               unsigned long call_reference)

{
    sccpmsg_general_t                      *gen_msg;
    sccpmsg_device_to_user_data_response_t *data;

    gen_msg = sccpmsg_get_gen_msg(msgcb, sizeof(sccpmsg_device_to_user_data_response_t),
                                  SCCPMSG_DEVICE_TO_USER_DATA_RESPONSE);
    if (gen_msg == NULL) {
        return (1);
    }

    data = &(gen_msg->body.device_to_user_data_response);

    SCCP_MEMCPY(data->data.data, user_data, len);
    data->data.data_length    = STOCML(len);
    data->data.line_number    = STOCML(line);
    data->data.call_reference = STOCML(call_reference);
    data->data.application_id = STOCML(application_id);
    data->data.transaction_id = STOCML(transaction_id);
    
    return (sccpmsg_send_message(msgcb, socket, gen_msg, sizeof(*data)));
}

#ifdef SCCPMSG_PARCHE
int sccpmsg_send_feature_stat_req (sccpmsg_msgcb_t *msgcb,
                                   unsigned int socket,
                                   unsigned long index)
{
    sccpmsg_general_t          *gen_msg;
    sccpmsg_feature_stat_req_t *data;

    gen_msg = sccpmsg_get_gen_msg(msgcb, sizeof(sccpmsg_feature_stat_req_t),
                                  SCCPMSG_FEATURE_STAT_REQ);
    if (gen_msg == NULL) {
        return (1);
    }

    data = &(gen_msg->body.feature_stat_req);

    data->feature_index = STOCML(index);
    
    return (sccpmsg_send_message(msgcb, socket, gen_msg, sizeof(*data)));
}
#endif
           
int sccpmsg_parse_msg (sccpmsg_general_t *gen_msg, int header, int msgid,
                       int body, sccp_sccpcb_t *sccpcb)
{
    /*
     * Check the endian-ness of the platform to determine if the message
     * really needs to be parsed.
     *
     * Little-endian platforms do not need to convert the messages since
     * the CM sends messages in little-endian format.
     *
     * But, there are some values that need to be set for earlier versions
     * of the SCCP protocol that are not 0. The msg structure is initialized
     * to all 0's so only parameters that are not 0 need to be set here.
     */
    if (sccpcb->endian == SCCP_ENDIAN_LITTLE) {
        /*
         * Get the version because other messages may be received before
         * we have a chance to process the register_ack in the sccpreg sem.
         */
        switch (gen_msg->body.msg_id.message_id) {
        case (SCCPMSG_REGISTER_ACK): {
            sccpmsg_register_ack_t *data = &(gen_msg->body.register_ack);

            /*
             * max_protocol_version was added after Bravo, so let's do a
             * little  magic to set the version. Make sure the sizeof
             * sccpmsg_register_ack is the size of a register_ack for all
             * versions greater than Bravo.
             */
            if (gen_msg->base.length < sizeof(sccpmsg_register_ack_t)) {
                sccp_set_version(sccpcb, SCCPMSG_VERSION_BRAVO);
                data->max_protocol_version = SCCPMSG_VERSION_BRAVO;
            }
            else {
                sccp_set_version(sccpcb, 
                                 SCCPMSG_INTERNAL_VERSION(data->max_protocol_version));
            }

            break;
        }

        case (SCCPMSG_SET_RINGER): {
            sccpmsg_set_ringer_t *data = &(gen_msg->body.set_ringer);

            if (sccpcb->version < SCCPMSG_VERSION_SEAVIEW) {
                data->ring_duration = SCCPMSG_RING_NORMAL;
                data->line_number   = 1; /*sam not sure why? */
            }

            break;
        }

        case (SCCPMSG_LINE_STAT): {
            sccpmsg_line_stat_t *data = &(gen_msg->body.line_stat);

            if (sccpcb->version < SCCPMSG_VERSION_HAWKBILL) {
                data->line_text_label[0] = '\0';
            }

            if (sccpcb->version < SCCPMSG_VERSION_PARCHE) {
                data->display_options = SCCPMSG_DISPLAY_OPTIONS_CNID |
                                        SCCPMSG_DISPLAY_OPTIONS_CLID |
                                        SCCPMSG_DISPLAY_OPTIONS_ODN;
            }

            break;
        }

        case (SCCPMSG_CALL_STATE): {
            sccpmsg_call_state_t *data = &(gen_msg->body.call_state);

            if (sccpcb->version < SCCPMSG_VERSION_HAWKBILL) {
                data->privacy = SCCPMSG_NO_PRIVACY;
            }

            if (sccpcb->version < SCCPMSG_VERSION_PARCHE) {
                data->precedence.level  = SCCPMSG_MLPP_PRECEDENCE_ROUTINE;
                data->precedence.domain = SCCPMSG_NO_PRECEDENCE_DOMAIN;
            }

            break;
        }

        default:
            break;
        }

        return (0);
    }

    /*
     * The platform is big-endian if the function is here. Therefore,
     * parse the requested portions of the message.
     */

    /*
     * Parse the header?
     */
    if (header == 1) {
        gen_msg->base.length            = CMTOSL(gen_msg->base.length);
        gen_msg->base.type              = CMTOSL(gen_msg->base.type);
    }

    /*
     * Parse the message id?
     */
    if (msgid == 1) {
        gen_msg->body.msg_id.message_id = CMTOSL(gen_msg->body.msg_id.message_id);
    }

    /*
     * Parse the body?
     */
    if (body == 0) {
        return (0);
    }

    switch (gen_msg->body.msg_id.message_id) {
    case (SCCPMSG_REGISTER_ACK): {
        sccpmsg_register_ack_t *data = &(gen_msg->body.register_ack);

        data->keepalive_interval_1 = CMTOSL(data->keepalive_interval_1);
        data->keepalive_interval_2 = CMTOSL(data->keepalive_interval_2);

        /*
         * max_protocol_version was added after Bravo, so let's do a
         * little  magic to set the version. Make sure the sizeof
         * sccpmsg_register_ack is the size of a register_ack for all
         * versions greater than Bravo.
         */
        if (gen_msg->base.length < sizeof(sccpmsg_register_ack_t)) {
            sccp_set_version(sccpcb, SCCPMSG_VERSION_BRAVO);
            data->max_protocol_version = SCCPMSG_VERSION_BRAVO;
        }
        else {
            data->max_protocol_version = CMTOSL(data->max_protocol_version);
            sccp_set_version(sccpcb,
                             SCCPMSG_INTERNAL_VERSION(data->max_protocol_version));
        }

        break;
    }
    
    case (SCCPMSG_REGISTER_TOKEN_REJECT): {
        sccpmsg_register_token_reject_t *data = &(gen_msg->body.register_token_reject);

        data->wait_time_before_next_reg = CMTOSL(data->wait_time_before_next_reg);

        break;
    }

    case (SCCPMSG_UNREGISTER_ACK): {
        sccpmsg_unregister_ack_t *data = &(gen_msg->body.unregister_ack);

        data->status = CMTOSL(data->status);

        break;
    }

    case (SCCPMSG_START_TONE): {
        sccpmsg_start_tone_t *data = &(gen_msg->body.start_tone);

        data->tone           = CMTOSL(data->tone);
        if (sccpcb->version < SCCPMSG_VERSION_HAWKBILL) {
            data->direction = SCCPMSG_TONE_DIRECTION_USER;
        }
        else {
            data->direction = CMTOSL(data->direction);
        }

        if (sccpcb->version < SCCPMSG_VERSION_PARCHE) {
            data->line_number    = SCCPMSG_NO_LINE;
            data->call_reference = SCCPMSG_NO_CALL_REFERENCE;
        }
        else {
            data->line_number    = CMTOSL(data->line_number);
            data->call_reference = CMTOSL(data->call_reference);
        }

        break;
    }

    case (SCCPMSG_STOP_TONE): {
        sccpmsg_stop_tone_t *data = &(gen_msg->body.stop_tone);

        if (sccpcb->version < SCCPMSG_VERSION_PARCHE) {
            data->line_number    = SCCPMSG_NO_LINE;
            data->call_reference = SCCPMSG_NO_CALL_REFERENCE;
        }
        else {
            data->line_number    = CMTOSL(data->line_number);
            data->call_reference = CMTOSL(data->call_reference);
        }

        break;
    }

    case (SCCPMSG_KEYPAD_BUTTON): {
        sccpmsg_keypad_button_t *data = &(gen_msg->body.keypad_button);

        data->button         = CMTOSL(data->button);
        data->line_number    = CMTOSL(data->line_number);
        data->call_reference = CMTOSL(data->call_reference);
        
        break;
    }

    case (SCCPMSG_SET_RINGER): {
        sccpmsg_set_ringer_t *data = &(gen_msg->body.set_ringer);

        data->ring_mode      = CMTOSL(data->ring_mode);
        if (sccpcb->version < SCCPMSG_VERSION_SEAVIEW) {
            data->ring_duration  = SCCPMSG_RING_NORMAL;
            data->line_number    = 1; /*sam not sure why? */
        }
        else {
            data->ring_duration  = CMTOSL(data->ring_duration);
            data->line_number    = CMTOSL(data->line_number);
        }

        if (sccpcb->version < SCCPMSG_VERSION_PARCHE) {
            data->call_reference = SCCPMSG_NO_CALL_REFERENCE;
        }
        else {
            data->call_reference    = CMTOSL(data->call_reference);
        }

        break;
    }

    case (SCCPMSG_SET_LAMP): {
        sccpmsg_set_lamp_t *data = &(gen_msg->body.set_lamp);

        data->stimulus    = CMTOSL(data->stimulus);
        data->line_number = CMTOSL(data->line_number);
        data->lamp_mode   = CMTOSL(data->lamp_mode);

        break;
    }
    
    case (SCCPMSG_SET_SPEAKER_MODE): {
        sccpmsg_set_speaker_mode_t *data = &(gen_msg->body.set_speaker_mode);

        data->mode = CMTOSL(data->mode);

        break;
    }
    
    case (SCCPMSG_SET_MICRO_MODE): {
        sccpmsg_set_micro_mode_t *data = &(gen_msg->body.set_micro_mode);

        data->mode = CMTOSL(data->mode);

        break;
    }
    
    /*
     * ip_address is in network order and port is in CM order in the 
     * received message, so flip the port. The port is received in CM order,
     * so we will convert it from CM order. Then the stack will convert it
     * to network order.
     */
    case (SCCPMSG_START_MEDIA_TRANSMISSION): {
        sccpmsg_start_media_transmission_t *data = &(gen_msg->body.start_media_transmission);

        data->conference_id     = CMTOSL(data->conference_id);
        data->passthru_party_id = CMTOSL(data->passthru_party_id);
        /* data->ip_address        = data->; stack will convert */
        data->port              = CMTOSL(data->port);
        data->ms_packet_size    = CMTOSL(data->ms_packet_size);
        data->payload           = CMTOSL(data->payload);
        if (sccpcb->version < SCCPMSG_VERSION_PARCHE) {
            data->call_reference = SCCPMSG_NO_CALL_REFERENCE;
        }
        else {
            data->call_reference    = CMTOSL(data->call_reference);
            data->media_encryption.algorithm      = CMTOSL(data->media_encryption.algorithm);
            data->media_encryption.key_len        = CMTOSS(data->media_encryption.key_len);
            data->media_encryption.salt_len       = CMTOSS(data->media_encryption.salt_len);
            /* data->media_encryption.key_data.key[]  = ; */
            /* data->media_encryption.key_data.salt[] = ; */
        }
        data->qualifier.precedence            = CMTOSL(data->qualifier.precedence);
        data->qualifier.silence_supression    = CMTOSL(data->qualifier.silence_supression);
        data->qualifier.max_frames_per_packet = CMTOSS(data->qualifier.max_frames_per_packet);
        data->qualifier.g723_bit_rate         = CMTOSL(data->qualifier.g723_bit_rate);

        break;
    }

    case (SCCPMSG_STOP_MEDIA_TRANSMISSION): {
        sccpmsg_stop_media_transmission_t *data = &(gen_msg->body.stop_media_transmission);

        data->conference_id     = CMTOSL(data->conference_id);
        data->passthru_party_id = CMTOSL(data->passthru_party_id);
        if (sccpcb->version < SCCPMSG_VERSION_PARCHE) {
            data->call_reference = SCCPMSG_NO_CALL_REFERENCE;
        }
        else {
            data->call_reference = CMTOSL(data->call_reference);
        }

        break;
    }

    case (SCCPMSG_OPEN_RECEIVE_CHANNEL): {
        sccpmsg_open_receive_channel_t *data = &(gen_msg->body.open_receive_channel);

        data->conference_id     = CMTOSL(data->conference_id);
        data->passthru_party_id = CMTOSL(data->passthru_party_id);
        data->ms_packet_size    = CMTOSL(data->ms_packet_size);
        data->payload           = CMTOSL(data->payload);
        if (sccpcb->version < SCCPMSG_VERSION_PARCHE) {
            data->call_reference = SCCPMSG_NO_CALL_REFERENCE;
        }
        else {
            data->call_reference = CMTOSL(data->call_reference);
            data->media_encryption.algorithm      = CMTOSL(data->media_encryption.algorithm);
            data->media_encryption.key_len        = CMTOSS(data->media_encryption.key_len);
            data->media_encryption.salt_len       = CMTOSS(data->media_encryption.salt_len);
            /* data->media_encryption.key_data.key[]  = ; */
            /* data->media_encryption.key_data.salt[] = ; */
        }
        data->qualifier.echo_cancellation = CMTOSL(data->qualifier.echo_cancellation);
        data->qualifier.g723_bit_rate     = CMTOSL(data->qualifier.g723_bit_rate);
        
        break;
    }

    case (SCCPMSG_CLOSE_RECEIVE_CHANNEL): {
        sccpmsg_close_receive_channel_t *data = &(gen_msg->body.close_receive_channel);

        data->conference_id     = CMTOSL(data->conference_id);
        data->passthru_party_id = CMTOSL(data->passthru_party_id);
        if (sccpcb->version < SCCPMSG_VERSION_PARCHE) {
            data->call_reference = SCCPMSG_NO_CALL_REFERENCE;
        }
        else {
            data->call_reference = CMTOSL(data->call_reference);
        }

        break;
    }

    case (SCCPMSG_RESET): {
        sccpmsg_reset_t *data = &(gen_msg->body.reset);

        data->reset_type = CMTOSL(data->reset_type);

        break;
    }
    
    case (SCCPMSG_CALL_INFO): {
        sccpmsg_call_info_t *data = &(gen_msg->body.call_info);

        /* data->calling_party_name[] = ; */
        /* data->calling_party_number[] = ; */
        /* data->called_party_name[] = ; */
        /* data->called_party_number[] = ; */
        /* data->original_called_party_name[] = ; */
        /* data->original_called_party_number[] = ; */
        /* data->last_redirecting_party_name[] = ; */
        /* data->last_redirecting_party_number[] = ; */
        /* data->cgpn_voice_mailbox[] = ; */
        /* data->cdpn_voice_mailbox[] = ; */
        /* data->original_cdpn_voice_mailbox[] = ; */
        /* data->last_redirecting_voice_mailbox[] = ; */
        data->line_number                   = CMTOSL(data->line_number);
        data->call_reference                = CMTOSL(data->call_reference);
        data->call_type                     = CMTOSL(data->call_type);
        data->original_cdpn_redirect_reason = CMTOSL(data->original_cdpn_redirect_reason);
        data->last_redirect_reason          = CMTOSL(data->last_redirect_reason);
        if (sccpcb->version < SCCPMSG_VERSION_PARCHE) {
            data->call_instance             = SCCPMSG_NO_CALL_NUMBER;
        }
        else {
            data->call_instance             = CMTOSL(data->call_instance);
        }

        break;
    }

    case (SCCPMSG_CALL_SELECT_STAT): {
        sccpmsg_call_select_stat_t *data = &(gen_msg->body.call_select_stat);

        data->call_reference = CMTOSL(data->call_reference);
        data->line_number    = CMTOSL(data->line_number);

        break;
    }

    case (SCCPMSG_DIALED_NUMBER): {
        sccpmsg_dialed_number_t *data = &(gen_msg->body.dialed_number);

        /* data->dialed_number[] = ; */
        data->line_number    = CMTOSL(data->line_number);
        data->call_reference = CMTOSL(data->call_reference);
        
        break;
    }

    case (SCCPMSG_USER_TO_DEVICE_DATA): {
        sccpmsg_user_to_device_data_t *data = &(gen_msg->body.user_to_device_data);

        data->data.application_id = CMTOSL(data->data.application_id);
        data->data.line_number    = CMTOSL(data->data.line_number);        
        data->data.call_reference = CMTOSL(data->data.call_reference);
        data->data.transaction_id = CMTOSL(data->data.transaction_id);
        data->data.data_length    = CMTOSL(data->data.data_length);
        /* data->data.data[] = ; */

        break;
    }

    case (SCCPMSG_FORWARD_STAT): {
        sccpmsg_forward_stat_t *data = &(gen_msg->body.forward_stat);

        data->active_forward           = CMTOSL(data->active_forward);
        data->line_number              = CMTOSL(data->line_number);
        data->forward_all_active       = CMTOSL(data->forward_all_active);
        /* data->forward_all_directory_number[] = ; */
        data->forward_busy_active      = CMTOSL(data->forward_busy_active);
        /* data->forward_busy_directory_number[] = ; */
        data->forward_no_answer_active = CMTOSL(data->forward_no_answer_active);
        /* data->forward_no_answer_directory_number[] = ; */

        break;
    }

    case (SCCPMSG_SPEEDDIAL_STAT): {
        sccpmsg_speeddial_stat_t *data = &(gen_msg->body.speeddial_stat);

        data->speeddial_number = CMTOSL(data->speeddial_number);
        /* data->speed_dial_directory_number[] = ; */
        /* data->speed_dial_display_name[] = ; */

        break;
    }

    case (SCCPMSG_LINE_STAT): {
        sccpmsg_line_stat_t *data = &(gen_msg->body.line_stat);

        data->line_number = CMTOSL(data->line_number);
        /* data->line_directory_number[] = ; */
        /* data->line_fully_qualified_display_name[] = ; */
        if (sccpcb->version < SCCPMSG_VERSION_HAWKBILL) {
            data->line_text_label[0] = '\0';
        }
        if (sccpcb->version < SCCPMSG_VERSION_PARCHE) {
            data->display_options = SCCPMSG_DISPLAY_OPTIONS_CNID |
                                    SCCPMSG_DISPLAY_OPTIONS_CLID |
                                    SCCPMSG_DISPLAY_OPTIONS_ODN;
        }
        else {
            data->display_options = CMTOSL(data->display_options);
        }

        break;
    }

    case (SCCPMSG_SERVICE_URL_STAT): {
        sccpmsg_service_url_stat_t *data = &(gen_msg->body.service_url_stat);

        data->service_url_index = CMTOSL(data->service_url_index);
        /* data->service_url[] = ; */
        /* data->service_url_display_name[] = ; */

        break;
    }

    case (SCCPMSG_FEATURE_STAT): {
        sccpmsg_feature_stat_t *data = &(gen_msg->body.feature_stat);

        data->feature_index = CMTOSL(data->feature_index);
        data->feature_id = CMTOSL(data->feature_id);
        /* data->feature_text_label[] = ; */
        data->feature_status = CMTOSL(data->feature_status);

        break;
    }

    case (SCCPMSG_DEFINE_TIME_DATE): {
        sccpmsg_define_time_date_t *data = &(gen_msg->body.define_time_date);

        data->station_time.year         = CMTOSL(data->station_time.year);
        data->station_time.month        = CMTOSL(data->station_time.month);
        data->station_time.day_of_week  = CMTOSL(data->station_time.day_of_week);
        data->station_time.day          = CMTOSL(data->station_time.day);
        data->station_time.hour         = CMTOSL(data->station_time.hour);
        data->station_time.minute       = CMTOSL(data->station_time.minute);
        data->station_time.second       = CMTOSL(data->station_time.second);
        data->station_time.milliseconds = CMTOSL(data->station_time.milliseconds);
        data->system_time  = CMTOSL(data->system_time);

        break;
    }

    case (SCCPMSG_BUTTON_TEMPLATE): {
        sccpmsg_button_template_t *data = &(gen_msg->body.button_template);

        data->button_template.button_offset      = CMTOSL(data->button_template.button_offset);
        data->button_template.button_count       = CMTOSL(data->button_template.button_count);
        data->button_template.total_button_count = CMTOSL(data->button_template.total_button_count);
        /* data->buttons = CMTOSL(data->buttons) ; */
        
        break;
    }

    case (SCCPMSG_CONNECTION_STATISTICS_REQ): {
        sccpmsg_connection_statistics_req_t *data = &(gen_msg->body.connection_statistics_req);

        /* data->directory_number[] = ; */
        data->call_reference = CMTOSL(data->call_reference);
        data->processing_mode = CMTOSL(data->processing_mode);

        break;
    }

    case (SCCPMSG_SOFTKEY_TEMPLATE_RES): {
        sccpmsg_softkey_template_res_t *data = &(gen_msg->body.softkey_template_res);
        unsigned long i;

        data->softkey_template.offset      = CMTOSL(data->softkey_template.offset);
        data->softkey_template.count       = CMTOSL(data->softkey_template.count);
        data->softkey_template.total_count = CMTOSL(data->softkey_template.total_count);

        for (i = 0; i < data->softkey_template.count; i++) {
            /* data->softkey_template.label[] = ; */
            data->softkey_template.definition[i].event = CMTOSL(data->softkey_template.definition[i].event);
        }

        break;
    }

    case (SCCPMSG_SOFTKEY_SET_RES): {
        sccpmsg_softkey_set_res_t *data = &(gen_msg->body.softkey_set_res);
        unsigned long i, j;

        data->softkey_set.offset      = CMTOSL(data->softkey_set.offset);
        data->softkey_set.count       = CMTOSL(data->softkey_set.count);
        data->softkey_set.total_count = CMTOSL(data->softkey_set.total_count);

        for (i = 0; i < data->softkey_set.count; i++) {
            for (j = 0; j < SCCPMSG_MAX_SOFTKEY_INDEXES; j++) {
                /* data->softkey_set.definition[i].template_index[j] = ; */
                data->softkey_set.definition[i].info_index[j] = CMTOSS(data->softkey_set.definition[i].info_index[j]);
            }
        }

        break;
    }

    case (SCCPMSG_SELECT_SOFTKEYS): {
        sccpmsg_select_softkeys_t *data = &(gen_msg->body.select_softkeys);

        data->line_number       = CMTOSL(data->line_number);
        data->reference         = CMTOSL(data->reference);
        data->softkey_set_index = CMTOSL(data->softkey_set_index);
        data->valid_key_mask    = CMTOSL(data->valid_key_mask);

        break;
    }

    case (SCCPMSG_CALL_STATE): {
        sccpmsg_call_state_t *data = &(gen_msg->body.call_state);

        data->call_state        = CMTOSL(data->call_state);
        data->line_number       = CMTOSL(data->line_number);
        data->call_reference    = CMTOSL(data->call_reference);
        if (sccpcb->version < SCCPMSG_VERSION_HAWKBILL) {
            data->privacy       = SCCPMSG_NO_PRIVACY;
        }
        else {
            data->privacy       = CMTOSL(data->privacy);
        }
        if (sccpcb->version < SCCPMSG_VERSION_PARCHE) {
            data->precedence.level  = SCCPMSG_MLPP_PRECEDENCE_ROUTINE;
            data->precedence.domain = SCCPMSG_NO_PRECEDENCE_DOMAIN;
        }
        else {
            data->precedence.level  = CMTOSL(data->precedence.level);
            data->precedence.domain = CMTOSL(data->precedence.domain);
        }

        break;
    }

    case (SCCPMSG_DISPLAY_PROMPT_STATUS): {
        sccpmsg_display_prompt_status_t *data = &(gen_msg->body.display_prompt_status);

        data->timeout        = CMTOSL(data->timeout);
        /* data->text[]         = ; */
        data->line_number    = CMTOSL(data->line_number);
        data->call_reference = CMTOSL(data->call_reference);

        break;
    }

    case (SCCPMSG_CLEAR_PROMPT_STATUS): {
        sccpmsg_clear_prompt_status_t *data = &(gen_msg->body.clear_prompt_status);

        data->line_number    = CMTOSL(data->line_number);
        data->call_reference = CMTOSL(data->call_reference);

        break;
    }

    case (SCCPMSG_DISPLAY_NOTIFY): {
        sccpmsg_display_notify_t *data = &(gen_msg->body.display_notify);

        data->timeout = CMTOSL(data->timeout);
        /* data->text[] = ; */

        break;
    }

    case (SCCPMSG_DISPLAY_PRIORITY_NOTIFY): {
        sccpmsg_display_priority_notify_t *data = &(gen_msg->body.display_priority_notify);

        data->timeout  = CMTOSL(data->timeout);
        data->priority = CMTOSL(data->priority);
        /* data->text[] = ; */

        break;
    }

    case (SCCPMSG_CLEAR_PRIORITY_NOTIFY): {
        sccpmsg_clear_priority_notify_t *data = &(gen_msg->body.clear_priority_notify);

        data->priority = CMTOSL(data->priority);

        break;
    }

    case (SCCPMSG_ACTIVATE_CALL_PLANE): {
        sccpmsg_activate_call_plane_t *data = &(gen_msg->body.activate_call_plane);

        data->line_number = CMTOSL(data->line_number);

        break;
    }

    case (SCCPMSG_BACKSPACE_REQ): {
        sccpmsg_backspace_req_t *data = &(gen_msg->body.backspace_req);

        data->line_number    = CMTOSL(data->line_number);
        data->call_reference = CMTOSL(data->call_reference);

        break;
    }

#if 0
    case (SCCPMSG_): {
        sccpmsg__t *data = &(gen_msg->body.);

        data-> = CMTOSL(data->);

        break;
    }

#endif

    default:
        return (0);
    }

    return (0);
}

void sccpmsg_get_sccp_msg_data (sccp_sccpcb_t *sccpcb, sccpmsg_general_t *msg,
                                unsigned long *call_id, unsigned long *line)
{
    char *fname = "get_sccp_msg_data";

    *call_id = SCCPMSG_NO_CALL_REFERENCE;
    *line    = SCCPMSG_NO_LINE;

    if (msg == NULL) {
        return;
    }

    switch (msg->body.msg_id.message_id) {
    case (SCCPMSG_KEYPAD_BUTTON):
        if (sccpcb->version > SCCPMSG_VERSION_SEAVIEW) {
            *call_id = msg->body.keypad_button.call_reference;
            *line    = msg->body.keypad_button.line_number;
        }

        break;

    case (SCCPMSG_DEFINE_TIME_DATE):
    case (SCCPMSG_DISPLAY_NOTIFY):
    case (SCCPMSG_CLEAR_NOTIFY):    
        break;

    case (SCCPMSG_START_TONE):
        if (sccpcb->version > SCCPMSG_VERSION_SEAVIEW) {
            *call_id = msg->body.start_tone.call_reference;
            *line    = msg->body.start_tone.line_number;
        }

        break;

    case (SCCPMSG_STOP_TONE):
        if (sccpcb->version > SCCPMSG_VERSION_SEAVIEW) {
            *call_id = msg->body.stop_tone.call_reference;
            *line    = msg->body.stop_tone.line_number;
        }

        break;

    case (SCCPMSG_SET_RINGER):
        *line = msg->body.set_ringer.line_number;

        if (sccpcb->version > SCCPMSG_VERSION_SEAVIEW) {
            *call_id = msg->body.set_ringer.call_reference;
        }

        break;

    case (SCCPMSG_SET_LAMP):
        *line = msg->body.set_lamp.line_number;
        break;

    case (SCCPMSG_OPEN_RECEIVE_CHANNEL):
        if (sccpcb->version > SCCPMSG_VERSION_SEAVIEW) {
            *call_id = msg->body.open_receive_channel.call_reference;
        }

        break;

    case (SCCPMSG_CLOSE_RECEIVE_CHANNEL):
        if (sccpcb->version > SCCPMSG_VERSION_SEAVIEW) {
            *call_id = msg->body.close_receive_channel.call_reference;
        }

        break;

    case (SCCPMSG_START_MEDIA_TRANSMISSION):
        if (sccpcb->version > SCCPMSG_VERSION_SEAVIEW) {
            *call_id = msg->body.start_media_transmission.call_reference;
        }

        break;

    case (SCCPMSG_STOP_MEDIA_TRANSMISSION):
        if (sccpcb->version > SCCPMSG_VERSION_SEAVIEW) {
            *call_id = msg->body.stop_media_transmission.call_reference;
        }

        break;

    case (SCCPMSG_CALL_INFO):
        *call_id = msg->body.call_info.call_reference;
        *line    = msg->body.call_info.line_number;
        break;

    case (SCCPMSG_SELECT_SOFTKEYS):
        *call_id = msg->body.select_softkeys.reference;
        *line    = msg->body.select_softkeys.line_number;
        break;

    case (SCCPMSG_CALL_STATE):
        *call_id = msg->body.call_state.call_reference;
        *line    = msg->body.call_state.line_number;
        break;

    case (SCCPMSG_DISPLAY_PROMPT_STATUS):
        *call_id = msg->body.display_prompt_status.call_reference;
        *line    = msg->body.display_prompt_status.line_number;
        break;

    case (SCCPMSG_CLEAR_PROMPT_STATUS):
        *call_id = msg->body.clear_prompt_status.call_reference;
        *line    = msg->body.clear_prompt_status.line_number;
        break;

    case (SCCPMSG_ACTIVATE_CALL_PLANE):
        *line = msg->body.activate_call_plane.line_number;
        break;

    case (SCCPMSG_BACKSPACE_REQ):
        *call_id = msg->body.backspace_req.call_reference;
        *line    = msg->body.backspace_req.line_number;
        break;

    case (SCCPMSG_DIALED_NUMBER):
        *call_id = msg->body.dialed_number.call_reference;
        *line    = msg->body.dialed_number.line_number;
        break;

    case (SCCPMSG_USER_TO_DEVICE_DATA):
        *call_id = msg->body.user_to_device_data.data.call_reference;
        *line    = msg->body.user_to_device_data.data.line_number;
        break;

    case (SCCPMSG_CALL_SELECT_STAT):
        *call_id = msg->body.call_select_stat.call_reference;
        *line    = msg->body.call_select_stat.line_number;
        break;

    default:
        break;
    }

    SCCP_DBG((sccpmsg_debug, 5,
             "%s %-5d: %-20s: call_id= 0x%x, line= %d\n",
             SCCPMSG_ID, sccpcb->id, fname, *call_id, *line));
}
#if 0
/*
 * FUNCTION:    sccpmsg_gapi_to_sccp_feature
 *
 * DESCRIPTION: Translate a GAPI feature to SCCP feature.
 *
 * PARAMETERS:
 *     event:   event control block
 *
 * RETURNS:     0 for success, otherwise 1.
 *
 * NOTES:
 */
static sccpmsg_softkey_events_t sccpmsg_gapi_to_sccp_feature (gapi_features_e feature)
{
    /*
     * The mappings are currently one to one.
     */
    return (feature);
}

/*
 * FUNCTION:    sccpmsg_sccp_to_gapi_tone
 *
 * DESCRIPTION: Translate SCCP tone to GAPI tone.
 *
 * PARAMETERS:
 *     event:   event control block
 *
 * RETURNS:     0 for success, otherwise 1.
 *
 * NOTES:
 */
static gapi_tones_e sccpmsg_sccp_to_gapi_tone (sccpmsg_tones_t tone)
{
    /*
     * The mappings are currently one to one.
     */
    return (tone);
}

/*
 * FUNCTION:    sccpmsg_sccp_to_gapi_tone_direction
 *
 * DESCRIPTION: Translate SCCP tone direction to GAPI tone direction.
 *
 * PARAMETERS:
 *     event:   event control block
 *
 * RETURNS:     0 for success, otherwise 1.
 *
 * NOTES:
 */
static gapi_tone_directions_e sccpmsg_sccp_to_gapi_tone_direction (sccpmsg_tone_directions_t direction)
{
    /*
     * The mappings are currently one to one.
     */
    return (direction);
}

/*
 * FUNCTION:    sccpmsg_sccp_to_gapi_ringer
 *
 * DESCRIPTION: Translate SCCP ring mode to GAPI ringer.
 *
 * PARAMETERS:
 *     event:   event control block
 *
 * RETURNS:     0 for success, otherwise 1.
 *
 * NOTES:
 */
static gapi_ringers_t sccpmsg_sccp_to_gapi_ringer (sccpmsg_ring_mode_t mode)
{
    /*
     * The mappings are currently one to one.
     */
    return (mode);
}

/*
 * FUNCTION:    sccpmsg_sccp_to_gapi_ring_duration
 *
 * DESCRIPTION: Translate SCCP ring duration to GAPI ring duration.
 *
 * PARAMETERS:
 *     event:   event control block
 *
 * RETURNS:     0 for success, otherwise 1.
 *
 * NOTES:
 */
static gapi_ringers_t sccpmsg_sccp_to_gapi_ring_duration (sccpmsg_ring_duration_t duration)
{
    /*
     * The mappings are currently one to one.
     */
    return (duration);
}
#endif

int sccpmsg_sccp_to_gapi_enum (int id)
{
    return (id);
}

gapi_user_and_device_data_t *sccpmsg_sccp_to_gapi_data (sccpmsg_user_and_device_data_t *data)
{
    return ((gapi_user_and_device_data_t *)data);
}

void sccpmsg_create_alarm_string (char *text, int tcp_close_cause, char *mac)
{
    if (text == NULL) {
        return;
    }

    SCCP_SNPRINTF((text, SCCPMSG_MAX_ALARM_TEXT_SIZE,
                   "%d: Name=%s%s Load=3.3(2.0) Last=%s", 
                   tcp_close_cause,
                   SCCPMSG_FIRMWARE_STRING, mac,
                   sccpmsg_alarm_name(tcp_close_cause)));
}

int sccpmsg_get_primary_socket (sccp_sccpcb_t *sccpcb)
{
    return (sccpcm_get_primary_socket(sccpcb));
}