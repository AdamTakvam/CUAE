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
 *     sapp_win79xx.c
 *
 *  CREATION/MODIFICATION DATES
 *     Created:  May 2003, Sam Hague
 *     Modified: 
 *
 *  DESCRIPTION
 *     SCCP Application platform dependent implementation
 *     (win32, 79xx softphone)
 */
#include "platform.h"
#include <stdio.h>
#include "sapp.h"

extern "C"
{
#include "gapi.h"
#include "sccpmsg.h"
}

#include "SccpCallbackProxy.h"

sccpmsg_msgcb_t sapp_msgcb = {NULL, 0, "SAPP"};

#ifdef _DEBUG
#define _CRTDBG_MAP_ALLOC 
#include <crtdbg.h>
#endif

#define SAPP_CAUSE_NORMAL GAPI_CAUSE_NORMAL
#define SAPP_CAUSE_ERROR  GAPI_CAUSE_ERROR

extern int sapp_initialized;

extern sapp_info_t sapp_infos[SAPP_MAX_INFOS];
extern sapp_calls_t sapp_calls;
extern int sapp_debug;

/*
 * sapp_app_xxx
 *
 * These are wrapper functions around the application functions.
 */
//static void sapp_app_setup (int call_id, int line, void *conninfo,
//                            int alert_info, int alerting_ring,
//                            int alerting_tone, void *media,
//                            void *redirect, int replaces)
//{
//}

//static void sapp_app_offhook (int call_id, int line)
//{
//}

//static void sapp_app_setup_ack (int call_id, int line, int cause,
//                                void *conninfo, void *media)
//{
//}

//static void sapp_app_proceeding (int call_id, int line, 
//                                 void *conninfo)
//{
//}
//
//static void sapp_app_alerting (int call_id, int line,
//                               void *conninfo, void *media, int inband)
//{
//}
//
//static void sapp_app_connect (int call_id, int line,
//                              void *conninfo, void *media)
//{
//}
//
//static void sapp_app_connect_ack (int call_id, int line,
//                                  void *conninfo, void *media)
//{
//}
//
//static void sapp_app_release (int call_id, int line, int cause)
//{
//}
//
//static void sapp_app_release_complete (int call_id, int line, int cause)
//{
//}
//
//static void sapp_app_feature_req (int call_id, int line, int feature)
//{
//}

static int sapp_sapp_to_gapi_feature (int feature)
{
    return (feature);
}

static void sapp_sccp_to_sapp_conninfo (gapi_conninfo_t *sccp_conninfo,
                                        void *sapp_conninfo,
                                        int direction)
{
    // *** Following was #ifdef'd out. Looks important? ***
    //
    //gapi_conninfo_t *sapp_info = (gapi_conninfo_t *)sapp_conninfo;

    //if (direction == 0) { /* outgoing */
    //    if (sccp_conninfo != NULL) {
    //        strncpy(sapp_info->called_number, sccp_conninfo->called_number,
    //                GAPI_DIRECTORY_NUMBER_SIZE);
    //    }
    //}
}

static void sapp_sapp_to_sccp_conninfo (gapi_conninfo_t *sccp_conninfo,
                                        void *sapp_conninfo)
{
}


/*
 * CCAPI expects addr and port in host order
 */
static void sapp_sccp_to_sapp_media (gapi_media_t *sccp_media, void *sapp_media)
{
    gapi_media_t *media  = (gapi_media_t*)sapp_media;
    
    memset(media, 0, sizeof(*media));

    if (sccp_media != NULL)  {
        media->sccp_media.addr = ntohl(sccp_media->sccp_media.addr);
        media->sccp_media.port = ntohl(sccp_media->sccp_media.port);        
        media->sccp_media.payload_type = sccp_media->sccp_media.payload_type;
    }
    else {
        /*
         * Stick a fake port into the remote sdp. SCCP sends the
         * open_rcv before the start_xmit so we never know what the
         * remote port is before we send up a connect. GSM will kill
         * the call if we don't give it some port during the connect.
         */
        media->sccp_media.addr = sapp_local_addr();
        media->sccp_media.port = 0x4000; /* 16384 */        
        media->sccp_media.payload_type = GAPI_PAYLOAD_TYPE_G711_ULAW_64K;
    }
}

/*
 * GAPI expects addr and port in network order.
 */
static void sapp_sapp_to_sccp_media (gapi_media_t *sccp_media, void *sapp_media)
{
    gapi_media_t *media = (gapi_media_t*)sapp_media;

    if (media != NULL) {
        sccp_media->sccp_media.addr = htonl(media->sccp_media.addr);
        sccp_media->sccp_media.port = htonl(media->sccp_media.port);
        sccp_media->sccp_media.payload_type = media->sccp_media.payload_type;
    }
    else {
        sccp_media->sccp_media.addr = sapp_local_addr();
        sccp_media->sccp_media.port = htonl(16384);
        sccp_media->sccp_media.payload_type = GAPI_PAYLOAD_TYPE_G711_ULAW_64K;
    }
}

//static int sccptest_get_new_call_id()
//{
//    static int id = 0;
//
//    if (++id < 0) {
//        id = 1;
//    }
//
//    return (id);
//}

static void sapp_set_new_call_data (sapp_call_t *call, sapp_direction_e direction,
                                    int sccp_conn_id, int sapp_call_id,
                                    int sccp_line, int sapp_line,
                                    void *sccp_media, void *sapp_media,
                                    void *sccp_conninfo, void *sapp_conninfo,
                                    sapp_info_t *sinfo)
{
#ifdef SAPP_HAS_RECURSION
    platform_mutex_lock(sapp_calls.mutex, SAPP_SCCP_WAIT_TIMEOUT);
#endif

    sapp_calls.count++;

    switch (direction) {
    case (SAPP_DIRECTION_OUTGOING):
        call->sccp_conn_id = gapi_get_new_conn_id();
        call->sapp_call_id = sapp_call_id;

        call->sccp_line = sccp_line;
        call->sapp_line = sapp_line;

        sapp_sapp_to_sccp_media(&(call->sccp_media), sapp_media);
        sapp_sccp_to_sapp_media(NULL, &(call->sapp_media));        

        sapp_sapp_to_sccp_conninfo(&(call->sccp_conninfo), sapp_conninfo);

        call->gapi_waiting = 2;

        break;

    case (SAPP_DIRECTION_INCOMING):
        call->sccp_conn_id = sccp_conn_id;

		// This may or may not be important
        //call->sapp_call_id = sccptest_get_new_call_id();

        call->sccp_line = sccp_line;
        call->sapp_line = sapp_line;

        sapp_sapp_to_sccp_media(&(call->sccp_media), NULL);
        sapp_sccp_to_sapp_media(NULL, &(call->sapp_media));

        call->gapi_waiting = 1;

        break;
    }

    call->sinfo = sinfo;

#ifdef SAPP_HAS_RECURSION
    platform_mutex_unlock(sapp_calls.mutex);
#endif
}

/*
 * Application wrapper functions. The application uses these functions to
 * access GAPI through SAPP.
 */
int sapp_setup (int call_id, int line,
                void *conninfo, char *digit, int numdigits,
                void *media, int alert_info, int privacy, int flag)
{
    char *fname = "sapp_setup";
    sapp_call_t *call;
    sapp_info_t *sinfo = &(sapp_infos[0]);

    sapp_debug_entry(fname, call_id, line);

    if (sinfo->sapp_state != SAPP_S_REGISTERED) {
        SccpCallbackProxy::sapp_app_release_complete(call_id, line, SAPP_CAUSE_NORMAL);
        return (0);
    }

    platform_mutex_lock(sapp_calls.mutex, SAPP_SCCP_WAIT_TIMEOUT);
    
    call = sapp_get_call_by_call_id(SAPP_NO_CONN_ID);
    if (call == NULL) {
        platform_mutex_unlock(sapp_calls.mutex);
        SccpCallbackProxy::sapp_app_release_complete(call_id, line, SAPP_CAUSE_NORMAL);
        return (0);
    }

    sapp_set_new_call_data(call, SAPP_DIRECTION_OUTGOING,
                           SAPP_NO_CONN_ID, call_id, line, line,
                           &(call->sccp_media), media,
                           &(call->sccp_conninfo), conninfo, sinfo);

    if (flag > 0) {
        call->sccp_conn_id = flag;
        call->sinfo->sapp_sccp_cbs->digits(call->sinfo->sapp_sccp_handle, GAPI_MSG_DIGITS,
                              call->sccp_conn_id, call->sccp_line,
                              digit, numdigits);

        platform_mutex_unlock(sapp_calls.mutex);
        return (0);
    }

#if 1
    call->sinfo->sapp_sccp_cbs->setup(call->sinfo->sapp_sccp_handle, GAPI_MSG_SETUP,
                         call->sccp_conn_id, call->sccp_line,
                         &(call->sccp_conninfo), digit, numdigits,
                         &(call->sccp_media), (gapi_alert_info_e)alert_info, 
                         (gapi_privacy_e)privacy, NULL);
#else /* passthru test */
    sccpmsg_send_offhook(&sapp_msgcb,
                         sccpmsg_get_primary_socket(call->sinfo->sapp_sccp_handle),
                         0, 0);

#endif
    platform_mutex_unlock(sapp_calls.mutex);

    return (0);
}

int sapp_setup_ack (int call_id, int line, void *media)
{
    char *fname = "sapp_setup_ack";
    sapp_call_t *call;

    sapp_debug_entry(fname, call_id, line);

    platform_mutex_lock(sapp_calls.mutex, SAPP_SCCP_WAIT_TIMEOUT);
    
    call = sapp_get_call_by_call_id(call_id);
    if (call == NULL) {
        platform_mutex_unlock(sapp_calls.mutex);
        SccpCallbackProxy::sapp_app_release_complete(call_id, line, SAPP_CAUSE_NORMAL);
        return (0);
    }

    call->sapp_line = line;

    /*
     * GSM sends the local sdp info, so save it away for when
     * SCCP requests the open_rcv.
     */
    sapp_sapp_to_sccp_media(&(call->sccp_media), media);

    call->sinfo->sapp_sccp_cbs->setup_ack(call->sinfo->sapp_sccp_handle, GAPI_MSG_SETUP_ACK,
                             call->sccp_conn_id, call->sccp_line,
                             NULL, NULL, GAPI_CAUSE_OK, NULL);

    platform_mutex_unlock(sapp_calls.mutex);

    return (0);
}

int sapp_proceeding (int call_id, int line)
{
    char *fname = "sapp_proceeding";
    sapp_call_t *call;

    sapp_debug_entry(fname, call_id, line);

    platform_mutex_lock(sapp_calls.mutex, SAPP_SCCP_WAIT_TIMEOUT);
    
    call = sapp_get_call_by_call_id(call_id);
    if (call == NULL) {
        platform_mutex_unlock(sapp_calls.mutex);
        SccpCallbackProxy::sapp_app_release_complete(call_id, line, SAPP_CAUSE_NORMAL);
        return (0);
    }

    call->sinfo->sapp_sccp_cbs->proceeding(call->sinfo->sapp_sccp_handle, GAPI_MSG_PROCEEDING,
                              call->sccp_conn_id, call->sccp_line, NULL,
                              NULL);

    platform_mutex_unlock(sapp_calls.mutex);

    return (0);
}

int sapp_alerting (int call_id, int line)
{
    char *fname = "sapp_alerting";
    sapp_call_t *call;

    sapp_debug_entry(fname, call_id, line);

    platform_mutex_lock(sapp_calls.mutex, SAPP_SCCP_WAIT_TIMEOUT);
    
    call = sapp_get_call_by_call_id(call_id);
    if (call == NULL) {
        platform_mutex_unlock(sapp_calls.mutex);
        SccpCallbackProxy::sapp_app_release_complete(call_id, line, SAPP_CAUSE_NORMAL);
        return (0);
    }

    call->sinfo->sapp_sccp_cbs->alerting(call->sinfo->sapp_sccp_handle, GAPI_MSG_ALERTING,
                            call->sccp_conn_id, call->sccp_line,
                            NULL, NULL, (gapi_inband_e)0, NULL);

    platform_mutex_unlock(sapp_calls.mutex);

    return (0);
}

int sapp_connect (int call_id, int line, void *media)
{
    char *fname = "sapp_connect";
    sapp_call_t *call;

    sapp_debug_entry(fname, call_id, line);

    platform_mutex_lock(sapp_calls.mutex, SAPP_SCCP_WAIT_TIMEOUT);
    
    call = sapp_get_call_by_call_id(call_id);
    if (call == NULL) {
        platform_mutex_unlock(sapp_calls.mutex);
        SccpCallbackProxy::sapp_app_release_complete(call_id, line, SAPP_CAUSE_NORMAL);    
        return (0);
    }
    /*
     * GSM sends the local sdp info, so save it away for when
     * SCCP requests the open_rcv.
     */
    sapp_sapp_to_sccp_media(&(call->sccp_media), media);

    call->sinfo->sapp_sccp_cbs->connect(call->sinfo->sapp_sccp_handle, GAPI_MSG_CONNECT,
                           call->sccp_conn_id, call->sccp_line,
                           NULL, NULL, NULL);

    platform_mutex_unlock(sapp_calls.mutex);

    return (0);
}

int sapp_connect_ack (int call_id, int line)
{
    char *fname = "sapp_connect_ack";
    sapp_call_t *call;

    sapp_debug_entry(fname, call_id, line);

    platform_mutex_lock(sapp_calls.mutex, SAPP_SCCP_WAIT_TIMEOUT);
    
    call = sapp_get_call_by_call_id(call_id);
    if (call == NULL) {
        platform_mutex_unlock(sapp_calls.mutex);
        SccpCallbackProxy::sapp_app_release_complete(call_id, line, SAPP_CAUSE_NORMAL);    
        return (0);
    }

    call->sinfo->sapp_sccp_cbs->connect_ack(call->sinfo->sapp_sccp_handle, GAPI_MSG_CONNECT_ACK,
                               call->sccp_conn_id, call->sccp_line,
                               NULL, NULL, NULL);
    
    platform_mutex_unlock(sapp_calls.mutex);

    return (0);
}

int sapp_release (int call_id, int line, int cause)
{
    char *fname = "sapp_release";
    sapp_call_t *call;

    sapp_debug_entry(fname, call_id, line);

    platform_mutex_lock(sapp_calls.mutex, SAPP_SCCP_WAIT_TIMEOUT);
    
    call = sapp_get_call_by_call_id(call_id);
    if (call == NULL) {
        platform_mutex_unlock(sapp_calls.mutex);
        SccpCallbackProxy::sapp_app_release_complete(call_id, line, SAPP_CAUSE_NORMAL);    
        return (0);
    }

    call->sinfo->sapp_sccp_cbs->release(call->sinfo->sapp_sccp_handle, GAPI_MSG_RELEASE,
                           call->sccp_conn_id, call->sccp_line,
                           GAPI_CAUSE_OK, NULL);
    
    platform_mutex_unlock(sapp_calls.mutex);
    
    return (0);
}

int sapp_release_complete (int call_id, int line, int cause)
{
    char        *fname = "sapp_release_complete";
    sapp_call_t *call;

    sapp_debug_entry(fname, call_id, line);

    platform_mutex_lock(sapp_calls.mutex, SAPP_SCCP_WAIT_TIMEOUT);
    
    call = sapp_get_call_by_call_id(call_id);
    if (call == NULL) {
        platform_mutex_unlock(sapp_calls.mutex);
        SccpCallbackProxy::sapp_app_release_complete(call_id, line, SAPP_CAUSE_NORMAL);    
        return (0);
    }

    call->sinfo->sapp_sccp_cbs->release_complete(call->sinfo->sapp_sccp_handle,
                                    GAPI_MSG_RELEASE_COMPLETE,
                                    call->sccp_conn_id, call->sccp_line,
                                    (gapi_causes_e)cause, NULL);

    sapp_free_call(call);

    sapp_calls.count--;
    
    platform_mutex_unlock(sapp_calls.mutex);

    if ((sapp_calls.count == 0) && (call->sinfo->sapp_state == SAPP_S_RESETTING)) {
        call->sinfo->sapp_sccp_cbs->resetsession_req(call->sinfo->sapp_sccp_handle,
                                        GAPI_MSG_RESETSESSION_RES,
                                        call->sinfo->sapp_reset_cause);
    }

    return (0);
}

int sapp_digits (int call_id, int line, char *digits, int numdigits)
{
    char *fname = "sapp_digit";
    sapp_call_t *call;

    sapp_debug_entry(fname, call_id, line);

    if (sapp_infos[0].sapp_state != SAPP_S_REGISTERED) {
        SccpCallbackProxy::sapp_app_release_complete(call_id, line, SAPP_CAUSE_NORMAL);
        return (0);
    }

    platform_mutex_lock(sapp_calls.mutex, SAPP_SCCP_WAIT_TIMEOUT);
    
    call = sapp_get_call_by_call_id(call_id);
    if (call == NULL) {
        platform_mutex_unlock(sapp_calls.mutex);
        SccpCallbackProxy::sapp_app_release_complete(call_id, line, SAPP_CAUSE_NORMAL);
        return (0);
    }

    call->sinfo->sapp_sccp_cbs->digits(call->sinfo->sapp_sccp_handle, GAPI_MSG_DIGITS,
                          call->sccp_conn_id, call->sccp_line,
                          digits, numdigits);

    platform_mutex_unlock(sapp_calls.mutex);

    return (0);
}

int sapp_feature_req (int call_id, int line, int feature, void *data,
                      gapi_causes_e cause)
{
    char *fname = "sapp_feature_req";
    sapp_call_t *call;

    sapp_debug_entry(fname, call_id, line);

    platform_mutex_lock(sapp_calls.mutex, SAPP_SCCP_WAIT_TIMEOUT);
    
    call = sapp_get_call_by_call_id(call_id);
    if (call == NULL) {
        if ((feature == GAPI_FEATURE_REDIAL) ||
            (feature == GAPI_FEATURE_SPEEDDIAL)) {
            call = sapp_get_call_by_call_id(SAPP_NO_CONN_ID);
            if (call == NULL) {
                platform_mutex_unlock(sapp_calls.mutex);
                SccpCallbackProxy::sapp_app_release_complete(call_id, line, SAPP_CAUSE_NORMAL);
                return (0);
            }

            sapp_set_new_call_data(call, SAPP_DIRECTION_OUTGOING,
                                   SAPP_NO_CONN_ID, call_id, line, line,
                                   &(call->sccp_media), NULL,
                                   &(call->sccp_conninfo), NULL,
                                   &(sapp_infos[0]));
        }
        else {
            platform_mutex_unlock(sapp_calls.mutex);
            SccpCallbackProxy::sapp_app_release_complete(call_id, line, SAPP_CAUSE_NORMAL);    
            return (0);
        }
    }

    call->sinfo->sapp_sccp_cbs->feature_req((gapi_handle_t)call->sinfo->sapp_sccp_handle,
                               GAPI_MSG_FEATURE_REQ,
                               call->sccp_conn_id, call->sccp_line,
                               (gapi_features_e)sapp_sapp_to_gapi_feature(feature),
                               NULL, cause);

    platform_mutex_unlock(sapp_calls.mutex);

    return (0);
}

// This code is insane. It was written by blind monkeys. 
// Don't even try to figure it out
int sapp_opensession_wrapper(char* lmac, unsigned long cm_addr, unsigned short cm_port)
{
	static int info_offset = 0;

	gapi_cmaddr_t cms[5];
    int i;
    int count = 0;
    gapi_media_caps_t caps;
    gapi_opensession_values_t values;

    memset(cms, 0, sizeof(*cms) * 5);
    
    for (i = 0; i < 5; i++) {
        cms[i].addr = cm_addr;
        cms[i].port = cm_port;
    }

    caps.count = 1;
    caps.caps[0].payload = GAPI_PAYLOAD_TYPE_G711_ULAW_64K;
    caps.caps[0].milliseconds_per_packet = 20;

    memset(&values, 0, sizeof(values));
    values.device_poll_to = 180000;
    values.device_type = GAPI_DEVICE_STATION_TELECASTER_MGR;

	SAPP_DEBUG("Registering MAC: %s\n", lmac);

    sapp_opensession_req(cms, lmac, NULL,
                            GAPI_SRST_MODE_DISABLE, NULL, &caps, &values,
                            GAPI_PROTOCOL_VERSION_PARCHE, &(sapp_infos[info_offset]));
	info_offset++;

	return 0;
}

int sapp_opensession_req (gapi_cmaddr_t *cms, char *mac,
                          gapi_cmaddr_t *srsts, gapi_srst_modes_e srst_mode,
                          gapi_cmaddr_t *tftp, gapi_media_caps_t *media_caps,
                          gapi_opensession_values_t *values,
                          gapi_protocol_versions_e version, sapp_info_t *sinfo)
{
    char *fname = "sapp_opensession_req";

    sapp_debug_entry(fname, 0, 0);

    /*
     * Check if we are already resetting. If so, then don't let
     * the app try to open until the reset is complete.
     */
    if (sinfo->sapp_state != SAPP_S_IDLE) {
        return (0);
    }

    sinfo->sapp_state = SAPP_S_OPENING;

#if 0 //test code
	//memset(cms, 0, 5*sizeof(*cms));
	//mac[0] = '\0';
	media_caps->count = 0;
#endif
#if (SAPP_APP == SAPP_APP_LOOPBACK)
    {
        char *lmac = "000000000000";
        itoa(sinfo->id, lmac+11, 10);

        mac = lmac;
    }
#endif
    sinfo->sapp_sccp_cbs->opensession_req(sinfo->sapp_sccp_handle,
                                   GAPI_MSG_OPENSESSION_REQ,
                                   cms, mac, srsts, srst_mode,
                                   tftp, media_caps, values, version);

    return (0);
}

int sapp_resetsession_req (gapi_causes_e cause)
{
    char          *fname = "sapp_resetsession_req";
    sapp_info_t *sinfo = &(sapp_infos[0]);

    sapp_debug_entry(fname, 0, 0);

    /*
     * Check if we are already resetting. If so, then don't let
     * the app try to reset.
     */
    if ((sinfo->sapp_state == SAPP_S_IDLE) || (sinfo->sapp_state == SAPP_S_RESETTING)) {
        return (0);
    }

    sinfo->sapp_state = SAPP_S_RESETTING;
    
    if (sinfo->sapp_reset_cause == GAPI_CAUSE_OK) {
        sinfo->sapp_reset_cause = cause;
    }

    if (sapp_calls.count > 0) {
        int i;

        for (i = 0; i < SAPP_MAX_CALLS; i++) {
            if (sapp_calls.calls[i].sccp_conn_id != SAPP_NO_CONN_ID) {
                sapp_release(sapp_calls.calls[i].sapp_call_id,
                             sapp_calls.calls[i].sapp_line, GAPI_CAUSE_OK);
            }
        }
    }
    else {
        sinfo->sapp_sccp_cbs->resetsession_req(sinfo->sapp_sccp_handle,
                                        GAPI_MSG_RESETSESSION_REQ,
                                        cause);
    }

    return (0);
}

/*
 * SAPP callbacks. These are the callbacks that SAPP gives to the stack.
 */

//int sapp_sapp_passthru (gapi_handle_t handle, int msg_id, void *msg, int len)
int sapp_sapp_passthru (void* handle, gapi_msgs_e msg_id, void *msg, int len)
{
    char *fname = "passthru";
    sccpmsg_general_t *gen_msg = (sccpmsg_general_t *)msg;

    sapp_debug_entry(fname, 0, 0);

    SAPP_DEBUG("%s: %s\n", SAPP_ID,
               sccpmsg_get_message_text(gen_msg->body.msg_id.message_id));

    return (0);
}


int sapp_sapp_setup (void* handle, gapi_msgs_e msg_id, int conn_id, int line,
                     gapi_conninfo_t *conninfo, char *digit,
                     int numdigits,
                     gapi_media_t *media, gapi_alert_info_e alert_info, gapi_privacy_e privacy,
                     gapi_precedence_t *precedence)
{
    char *fname = "sapp_setup";
    sapp_call_t *call;
    sapp_info_t *sinfo = (sapp_info_t *)handle;

    sapp_debug_entry(fname, conn_id, line);

    if (sinfo->sapp_state != SAPP_S_REGISTERED) {
        sinfo->sapp_sccp_cbs->release_complete(sinfo->sapp_sccp_handle,
                                        GAPI_MSG_RELEASE_COMPLETE,
                                        conn_id, line,
                                        GAPI_CAUSE_NO_RESOURCE, NULL);

        return (0);
    }

    platform_mutex_lock(sapp_calls.mutex, SAPP_SCCP_WAIT_TIMEOUT);

    call = sapp_get_call_by_call_id(SAPP_NO_CONN_ID);
    if (call == NULL) {
        platform_mutex_unlock(sapp_calls.mutex);
        sinfo->sapp_sccp_cbs->release_complete(sinfo->sapp_sccp_handle,
                                        GAPI_MSG_RELEASE_COMPLETE,
                                        conn_id, line,
                                        GAPI_CAUSE_NO_RESOURCE, NULL);
        return (0);
    }

    sapp_set_new_call_data(call, SAPP_DIRECTION_INCOMING,
                           conn_id, SAPP_NO_CONN_ID, line, line,
                           media, &(call->sapp_media),
                           conninfo, &(call->sapp_conninfo), sinfo);

    /*
     * We don't have the called_number yet, so we can't send cc_setup
     * to application. Wait until we receive the conn_info and grab the data
     * from that. The gapi_waiting flag was set by sapp_set_new_call_data
     * and will be used when the conn_info message is received to indicate
     * that the cc_setup needs to be sent.
     */
    platform_mutex_unlock(sapp_calls.mutex);

    return (0);
}

int sapp_sapp_offhook (void* handle, gapi_msgs_e msg_id, int conn_id, int line)
{
    char *fname = "sapp_offhook";
//    sapp_call_t *call;
    sapp_info_t *sinfo = (sapp_info_t *)handle;

    sapp_debug_entry(fname, conn_id, line);

    if (sinfo->sapp_state != SAPP_S_REGISTERED) {
        sinfo->sapp_sccp_cbs->release_complete(sinfo->sapp_sccp_handle,
                                        GAPI_MSG_RELEASE_COMPLETE,
                                        conn_id, line,
                                        GAPI_CAUSE_NO_RESOURCE, NULL);

        return (0);
    }

#if 0
    platform_mutex_lock(sapp_calls.mutex, SAPP_SCCP_WAIT_TIMEOUT);

    call = sapp_get_call_by_call_id(SAPP_NO_CONN_ID);
    if (call == NULL) {
        platform_mutex_unlock(sapp_calls.mutex);
        sinfo->sapp_sccp_cbs->release_complete(sinfo->sapp_sccp_handle,
                                        GAPI_MSG_RELEASE_COMPLETE,
                                        conn_id, line,
                                        GAPI_CAUSE_NO_RESOURCE, NULL);
        return (0);
    }

    sapp_set_new_call_data(call, SAPP_DIRECTION_OUTGOING,
                           conn_id, SAPP_NO_CONN_ID, line, line,
                           NULL, &(call->sapp_media),
                           NULL, &(call->sapp_conninfo), sinfo);
#endif
    //sccptest_offhook(conn_id, line, 0);
    SccpCallbackProxy::sapp_app_offhook(conn_id, line);
                           
    /*
     * We don't have the called_number yet, so we can't send cc_setup
     * to application. Wait until we receive the conn_info and grab the data
     * from that. The gapi_waiting flag was set by sapp_set_new_call_data
     * and will be used when the conn_info message is received to indicate
     * that the cc_setup needs to be sent.
     */
//    platform_mutex_unlock(sapp_calls.mutex);

    return (0);
}

int sapp_sapp_setup_ack (void* handle, gapi_msgs_e msg_id, int conn_id, int line,
                         gapi_conninfo_t *conninfo, gapi_media_t *media,
                         gapi_causes_e cause, gapi_precedence_t *precedence)
{
    char *fname = "sapp_setup_ack";
    sapp_call_t *call;
    sapp_info_t *sinfo = (sapp_info_t *)handle;

    sapp_debug_entry(fname, conn_id, line);

    platform_mutex_lock(sapp_calls.mutex, SAPP_SCCP_WAIT_TIMEOUT);
    
    call = sapp_get_call_by_conn_id(conn_id);
    if (call == NULL) {
        platform_mutex_unlock(sapp_calls.mutex);
        sinfo->sapp_sccp_cbs->release_complete(sinfo->sapp_sccp_handle,
                                        GAPI_MSG_RELEASE_COMPLETE,
                                        conn_id, line,
                                        GAPI_CAUSE_NO_RESOURCE, NULL);
        return (0);
    }

    if (cause == GAPI_CAUSE_OK) {

        SAPP_DEBUG("%s: %d-%d: %s: call->sccp_conn_id= %d\n",
                   SAPP_ID, conn_id, line, fname, call->sccp_conn_id);

        SccpCallbackProxy::sapp_app_setup_ack(call->sapp_call_id, call->sapp_line,
                           SAPP_CAUSE_NORMAL,
                           &(call->sapp_conninfo), &(call->sapp_media));
    }
    else  {
        SccpCallbackProxy::sapp_app_release(call->sapp_call_id, call->sapp_line,
                         SAPP_CAUSE_ERROR);
    }

    platform_mutex_unlock(sapp_calls.mutex);

    return (0);
}

int sapp_sapp_proceeding (void* handle, gapi_msgs_e msg_id, int conn_id, int line,
                          gapi_conninfo_t *conninfo,
                          gapi_precedence_t *precedence)
{
    char *fname = "sapp_proceeding";

    sapp_debug_entry(fname, conn_id, line);

    /*
     * Don't send the message to ccapi yet. We need to wait for the
     * call_info message so that we can grab the called_number. The cc_setup
     * requires a called_number.
     */

    return (0);
}

int sapp_sapp_alerting (void* handle, gapi_msgs_e msg_id, int conn_id, int line,
                        gapi_conninfo_t *conninfo, gapi_media_t *media,
                        gapi_inband_e inband, gapi_precedence_t *precedence)
{
    char *fname = "sapp_alerting";
    sapp_call_t *call;
    sapp_info_t *sinfo = (sapp_info_t *)handle;

    sapp_debug_entry(fname, conn_id, line);

    platform_mutex_lock(sapp_calls.mutex, SAPP_SCCP_WAIT_TIMEOUT);
    
    call = sapp_get_call_by_conn_id(conn_id);
    if (call == NULL) {
        platform_mutex_unlock(sapp_calls.mutex);
        sinfo->sapp_sccp_cbs->release_complete(sinfo->sapp_sccp_handle,
                                        GAPI_MSG_RELEASE_COMPLETE,
                                        conn_id, line,
                                        GAPI_CAUSE_NO_RESOURCE, NULL);
        return (0);
    }

    SccpCallbackProxy::sapp_app_alerting(call->sapp_call_id, call->sapp_line,
                      &(call->sapp_conninfo), NULL, 0);    

    platform_mutex_unlock(sapp_calls.mutex);

    return (0);
}

int sapp_sapp_connect (void* handle, gapi_msgs_e msg_id, int conn_id, int line,
                       gapi_conninfo_t *conninfo, gapi_media_t *media,
                       gapi_precedence_t *precedence)
{             
    char *fname = "sapp_connect";
    sapp_call_t *call;
    sapp_info_t *sinfo = (sapp_info_t *)handle;

    sapp_debug_entry(fname, conn_id, line);

    platform_mutex_lock(sapp_calls.mutex, SAPP_SCCP_WAIT_TIMEOUT);
    
    call = sapp_get_call_by_conn_id(conn_id);
    if (call == NULL) {
        platform_mutex_unlock(sapp_calls.mutex);
        sinfo->sapp_sccp_cbs->release_complete(sinfo->sapp_sccp_handle,
                                        GAPI_MSG_RELEASE_COMPLETE,
                                        conn_id, line,
                                        GAPI_CAUSE_NO_RESOURCE, NULL);
        return (0);
    }

    SccpCallbackProxy::sapp_app_connect(call->sapp_call_id, call->sapp_line,
                     &(call->sapp_conninfo), &(call->sapp_media));        

    platform_mutex_unlock(sapp_calls.mutex);

    return (0);
}

int sapp_sapp_connect_ack (void* handle, gapi_msgs_e msg_id, int conn_id,
                           int line, gapi_conninfo_t *conninfo,
                           gapi_media_t *media,
                           gapi_precedence_t *precedence)
{
    char *fname = "sapp_connect_ack";
    sapp_call_t *call;
    sapp_info_t *sinfo = (sapp_info_t *)handle;

    sapp_debug_entry(fname, conn_id, line);

    platform_mutex_lock(sapp_calls.mutex, SAPP_SCCP_WAIT_TIMEOUT);
    
    call = sapp_get_call_by_conn_id(conn_id);
    if (call == NULL) {
        platform_mutex_unlock(sapp_calls.mutex);
        sinfo->sapp_sccp_cbs->release_complete(sinfo->sapp_sccp_handle,
                                        GAPI_MSG_RELEASE_COMPLETE,
                                        conn_id, line,
                                        GAPI_CAUSE_NO_RESOURCE, NULL);
        return (0);
    }

    SccpCallbackProxy::sapp_app_connect_ack(call->sapp_call_id, call->sapp_line,
                         &(call->sapp_conninfo),
                         &(call->sapp_media));    

    platform_mutex_unlock(sapp_calls.mutex);

    return (0);
}

int sapp_sapp_disconnect (void* handle, gapi_msgs_e msg_id, int conn_id, int line,
                          gapi_causes_e cause, gapi_precedence_t *precedence)
{
    char *fname = "sapp_disconnect";

    sapp_debug_entry(fname, conn_id, line);
    
    return (0);
}

int sapp_sapp_release (void* handle, gapi_msgs_e msg_id, int conn_id, int line,
                       gapi_causes_e cause, gapi_precedence_t *precedence)
{
    char *fname = "sapp_release";
    sapp_call_t *call;
    sapp_info_t *sinfo = (sapp_info_t *)handle;

    sapp_debug_entry(fname, conn_id, line);

    platform_mutex_lock(sapp_calls.mutex, SAPP_SCCP_WAIT_TIMEOUT);
    
    call = sapp_get_call_by_conn_id(conn_id);
    if (call == NULL) {
        platform_mutex_unlock(sapp_calls.mutex);
        sinfo->sapp_sccp_cbs->release_complete(sinfo->sapp_sccp_handle,
                                        GAPI_MSG_RELEASE_COMPLETE,
                                        conn_id, line,
                                        GAPI_CAUSE_NO_RESOURCE, NULL);
        return (0);
    }

    SccpCallbackProxy::sapp_app_release(call->sapp_call_id, call->sapp_line,
                     SAPP_CAUSE_NORMAL);

    platform_mutex_unlock(sapp_calls.mutex);

    return (0);
}

int sapp_sapp_release_complete (void* handle, gapi_msgs_e msg_id, int conn_id,
                                int line, gapi_causes_e cause, 
                                gapi_precedence_t *precedence)
{
    char *fname = "sapp_release_complete";
    sapp_call_t *call;
    sapp_info_t *sinfo = (sapp_info_t *)handle;

    sapp_debug_entry(fname, conn_id, line);

    platform_mutex_lock(sapp_calls.mutex, SAPP_SCCP_WAIT_TIMEOUT);
    
    call = sapp_get_call_by_conn_id(conn_id);
    if (call == NULL) {
        platform_mutex_unlock(sapp_calls.mutex);
        sinfo->sapp_sccp_cbs->release_complete(sinfo->sapp_sccp_handle, 
                                        GAPI_MSG_RELEASE_COMPLETE,
                                        conn_id, line,
                                        GAPI_CAUSE_NO_RESOURCE, NULL);
        return (0);
    }

    SccpCallbackProxy::sapp_app_release_complete(call->sapp_call_id, call->sapp_line,
                              SAPP_CAUSE_NORMAL);    

    sapp_free_call(call);
    
    sapp_calls.count--;

    platform_mutex_unlock(sapp_calls.mutex);

    /*
     * Did the stack request a reset? If so and this is the last call then 
     * let's start the reset.
     */
    if ((sapp_calls.count == 0) && (sinfo->sapp_state == SAPP_S_RESETTING)) {
        sinfo->sapp_sccp_cbs->resetsession_req(sinfo->sapp_sccp_handle,
                                        GAPI_MSG_RESETSESSION_RES,
                                        sinfo->sapp_reset_cause);
    }

    return (0);
}

int sapp_sapp_openrcv_req (void* handle, gapi_msgs_e msg_id, int conn_id,
                           int line, gapi_media_t *media)
{
	SccpCallbackProxy::sapp_sapp_openrcv_req(conn_id, line, media);

    //char        *fname = "sapp_sapp_openrcv_req";
    //sapp_call_t *call;
    //sapp_info_t *sinfo = (sapp_info_t *)handle;
    //
    //sapp_debug_entry(fname, conn_id, line);

    //platform_mutex_lock(sapp_calls.mutex, SAPP_SCCP_WAIT_TIMEOUT);
    //
    //call = sapp_get_call_by_conn_id(conn_id);
    //if (call == NULL) {
    //    platform_mutex_unlock(sapp_calls.mutex);
    //    sinfo->sapp_sccp_cbs->release_complete(sinfo->sapp_sccp_handle, 
    //                                    GAPI_MSG_RELEASE_COMPLETE,
    //                                    conn_id, line,
    //                                    GAPI_CAUSE_NO_RESOURCE, NULL);
    //    return (0);
    //}

    //media->sccp_media.addr = call->sccp_media.sccp_media.addr;
    //media->sccp_media.port = call->sccp_media.sccp_media.port;

    //sinfo->sapp_sccp_cbs->openrcv_res(sinfo->sapp_sccp_handle, GAPI_MSG_OPENRCV_RES, conn_id,
    //                           line, media, GAPI_CAUSE_OK);
    //platform_mutex_unlock(sapp_calls.mutex);

    return (0);
}

int sapp_sapp_openrcv_res(int conn_id, int line, gapi_media_t *media)
{
	sapp_info_t sinfo = sapp_infos[0];

	sinfo.sapp_sccp_cbs->openrcv_res(sinfo.sapp_sccp_handle, GAPI_MSG_OPENRCV_RES, conn_id,
                               line, media, GAPI_CAUSE_OK);

	return (0);
}

int sapp_sapp_closercv (void* handle, gapi_msgs_e msg_id, int conn_id,
                        int line, gapi_media_t *media)
{
    char *fname = "sapp_closercv";

    sapp_debug_entry(fname, conn_id, line);

    return (0);
}

int sapp_sapp_startxmit (void* handle, gapi_msgs_e msg_id, int conn_id,
                         int line, gapi_media_t *media)
{
    char *fname = "sapp_startxmit";
#if (SAPP_APP == SAPP_APP_GSM)
    sapp_call_t *call;
    cc_feature_data_t data;
    sapp_info_t *sinfo = (sapp_info_t *)handle;

    sapp_debug_entry(fname, conn_id, line);

    platform_mutex_lock(sapp_calls.mutex, SAPP_SCCP_WAIT_TIMEOUT);
    
    call = sapp_get_call_by_conn_id(conn_id);
    if (call == NULL) {
        platform_mutex_unlock(sapp_calls.mutex);
        sinfo->sapp_sccp_cbs->release_complete(sinfo->sapp_sccp_handle,
                                        GAPI_MSG_RELEASE_COMPLETE,
                                        conn_id, line,
                                        GAPI_CAUSE_NO_RESOURCE, NULL);
        return (0);
    }

    sapp_sccp_to_sapp_media(media, &(call->sapp_media));
    memcpy(&(data.resume.sdp), &(call->sapp_media), sizeof(data.resume.sdp));
    //sapp_sccp_to_sapp_media(media, &(data.resume.sdp));
                     
    cc_feature(CC_SRC_SIP, call->sapp_call_id, call->sapp_line,
               CC_FEATURE_MEDIA, &data);    

    platform_mutex_unlock(sapp_calls.mutex);
#endif
#if (SAPP_APP == SAPP_APP_SCCPTEST)
    sapp_debug_entry(fname, conn_id, line);

	SccpCallbackProxy::sapp_sapp_startxmit(conn_id, line, media);
#endif
 
    return (0);
}

int sapp_sapp_stopxmit (void* handle, gapi_msgs_e msg_id, int conn_id,
                        int line, gapi_media_t *media)
{
    char *fname = "sapp_stopxmit";
#if (SAPP_APP == SAPP_APP_GSM)
    sapp_call_t *call;
    cc_feature_data_t data;
    sapp_info_t *sinfo = (sapp_info_t *)handle;

    sapp_debug_entry(fname, conn_id, line);

    platform_mutex_lock(sapp_calls.mutex, SAPP_SCCP_WAIT_TIMEOUT);
    
    call = sapp_get_call_by_conn_id(conn_id);
    if (call == NULL) {
        platform_mutex_unlock(sapp_calls.mutex);
        sinfo->sapp_sccp_cbs->release_complete(sinfo->sapp_sccp_handle,
                                        GAPI_MSG_RELEASE_COMPLETE,
                                        conn_id, line,
                                        GAPI_CAUSE_NO_RESOURCE, NULL);
        return (0);
    }

    sapp_sccp_to_sapp_media(media, &(data.resume.sdp));
                     
    cc_feature(CC_SRC_SIP, call->sapp_call_id, call->sapp_line,
               CC_FEATURE_MEDIA, &data);    

    platform_mutex_unlock(sapp_calls.mutex);
#endif
#if (SAPP_APP == SAPP_APP_SCCPTEST)
    sapp_debug_entry(fname, conn_id, line);
#endif

    return (0);
}

int sapp_sapp_feature_req (void* handle, gapi_msgs_e msg_id, int conn_id,
                           int line, gapi_features_e feature,
                           gapi_feature_data_u *data, gapi_causes_e cause)
{
    char *fname = "sapp_feature_req";
    sapp_call_t *call;
    sapp_info_t *sinfo = (sapp_info_t *)handle;

    sapp_debug_entry(fname, conn_id, line);

    platform_mutex_lock(sapp_calls.mutex, SAPP_SCCP_WAIT_TIMEOUT);
    
    call = sapp_get_call_by_conn_id(conn_id);
    if (call == NULL) {
        platform_mutex_unlock(sapp_calls.mutex);
        sinfo->sapp_sccp_cbs->release_complete(sinfo->sapp_sccp_handle,
                                        GAPI_MSG_RELEASE_COMPLETE,
                                        conn_id, line,
                                        GAPI_CAUSE_NO_RESOURCE, NULL);
        return (0);
    }

    SccpCallbackProxy::sapp_app_feature_req(call->sapp_call_id, call->sapp_line, feature);

    sinfo->sapp_sccp_cbs->feature_res(sinfo->sapp_sccp_handle,
                               GAPI_MSG_FEATURE_RES,
                               call->sccp_conn_id, call->sccp_line,
                               feature,
                               NULL, GAPI_CAUSE_OK);

    platform_mutex_unlock(sapp_calls.mutex);

    return (0);
}

int sapp_sapp_feature_res (void* handle, gapi_msgs_e msg_id, int conn_id,
                           int line, gapi_features_e feature,
                           gapi_feature_data_u *data, gapi_causes_e cause)
{
    char *fname = "sapp_feature_res";
#if (SAPP_APP == SAPP_APP_GSM)
    sapp_call_t *call;
    sapp_info_t *sinfo = (sapp_info_t *)handle;

    sapp_debug_entry(fname, conn_id, line);

    platform_mutex_lock(sapp_calls.mutex, SAPP_SCCP_WAIT_TIMEOUT);
    
    call = sapp_get_call_by_conn_id(conn_id);
    if (call == NULL) {
        platform_mutex_unlock(sapp_calls.mutex);
        sinfo->sapp_sccp_cbs->release_complete(sinfo->sapp_sccp_handle,
                                        GAPI_MSG_RELEASE_COMPLETE,
                                        conn_id, line,
                                        GAPI_CAUSE_NO_RESOURCE, NULL);
        return (0);
    }

    cc_feature_ack(CC_SRC_SIP, call->sapp_call_id, call->sapp_line,
                   (cc_features_t )feature, NULL, (cc_causes_t)cause);    

    platform_mutex_unlock(sapp_calls.mutex);
#endif
#if (SAPP_APP == SAPP_APP_SCCPTEST)
    sapp_debug_entry(fname, conn_id, line);
#endif

    return (0);
}

int sapp_sapp_connectionstats (void* handle, gapi_msgs_e msg_id, int conn_id,
                               int line, gapi_connection_stats_t *stats)
{
    char *fname = "sapp_connection_stats";

    sapp_debug_entry(fname, conn_id, line);

    stats->number_packets_sent     = 0;
    stats->number_bytes_sent       = 0;
    stats->number_packets_received = 0;
    stats->number_bytes_received   = 0;
    stats->number_packets_lost     = 0;
    stats->jitter                  = 0;
    stats->latency                 = 0;

    return (0);
}

int sapp_sapp_reset (void* handle, gapi_msgs_e msg_id,
                     gapi_causes_e cause)
{
    char *fname = "sapp_reset";
    sapp_info_t *sinfo = (sapp_info_t *)handle;

    sapp_debug_entry(fname, 0, 0);

    SAPP_DEBUG("%s: 0-0: %-25s: cause= %d\n", SAPP_ID, fname, cause);

    sinfo->sapp_state = SAPP_S_RESETTING;
    
    sinfo->sapp_reset_cause = cause;

    /*
     * Check the reset type which can be either reset or restart. In either
     * case, SAPP should not send the resetsession_req until it has
     * completed whatever it needs to complete.
     */

    /* 
     * The reset type may also be from a register_reject or unable_to_find_cm.
     * In these cases, the application should reply with 
     * the resetsession to put the session back to the idle state. The
     * application should then fix the data and restart the session with
     * another opensession. Or the application can just change the reset 
     * type to GAPI_CAUSE_CM_RESTART and PSCCP will automatically try to
     * restart the session.
     */
#if 0
    /*
     * Force the stack to restart by setting the reset type to restart. 
     */
    switch (cause) {
    case (GAPI_CAUSE_NO_CM_FOUND):
    case (GAPI_CAUSE_CM_REGISTER_REJECT):
        cause = GAPI_CAUSE_CM_RESTART;
    }
#endif
    /*
     * Only start the reset if there are no calls. We will start the 
     * reset after all calls have cleared.
     */
    if (sapp_calls.count == 0) {
        sinfo->sapp_sccp_cbs->resetsession_req(sinfo->sapp_sccp_handle,
                                        GAPI_MSG_RESETSESSION_REQ,
                                        cause);
    }
    
    return (0);
}

int sapp_sapp_opensession_res (void* handle, gapi_msgs_e msg_id,
                               gapi_causes_e cause)
{
    char *fname = "sapp_opensession_res";
    sapp_info_t *sinfo = (sapp_info_t *)handle;

    sapp_debug_entry(fname, 0, 0);

	switch (cause) {
	case (GAPI_CAUSE_OK):
	    sinfo->sapp_state = SAPP_S_OPENED;
		break;

	default:
		sinfo->sapp_state = SAPP_S_IDLE;
#if (SAPP_APP == SAPP_APP_SCCPTEST)
        //sccptest_sessionstatus(GAPI_STATUS_RESET_COMPLETE);
#endif
		break;
	}

    SAPP_DEBUG("%s: 0-0: %-25s: cause= %d\n", SAPP_ID, fname, cause);
    
    //_CrtMemDumpAllObjectsSince(NULL);

    return (0);
}

int sapp_sapp_sessionstatus (void* handle, gapi_msgs_e msg_id,
                             gapi_status_e status, 
                             gapi_status_data_types_e type,
                             gapi_status_data_u *data)
{
    char          *fname = "sapp_sessionstatus";
    unsigned char *p;
    sapp_cminfo_t *cminfo = NULL;
    gapi_cmaddr_t *cmaddr;
    sapp_info_t *sinfo = (sapp_info_t *)handle;

    sapp_debug_entry(fname, 0, 0);

    SAPP_DEBUG("%s: 0-0: %-25s: status= %d\n", SAPP_ID, fname, status);
    
    switch (status) {
    case (GAPI_STATUS_RESET_COMPLETE):
        sinfo->sapp_state = SAPP_S_IDLE;
        sinfo->sapp_reset_cause = GAPI_CAUSE_OK;

        memset(&(sinfo->sapp_session_data), 0, sizeof(sinfo->sapp_session_data));

#if (SAPP_APP == SAPP_APP_SCCPTEST)
//        sccptest_sessionstatus(status);
#endif

        break;

    case (GAPI_STATUS_CM_DOWN):
        if (data == NULL) {
            SAPP_DEBUG("%s: 0-0: %-25s: need to kill calls to start reset.\n",
                       SAPP_ID, fname);
        }
        else {
            cminfo = sapp_get_cminfo(data->misc.cmaddr.addr,
                                     data->misc.cmaddr.port,
                                     handle);
            if (cminfo != NULL) {
                /*
                 * Reset the session_data if we lost the registered CCM.
                 */
                if (cminfo->state == SAPP_CM_S_REGISTERED) {
                    memset(&(sinfo->sapp_session_data), 0, sizeof(sinfo->sapp_session_data));
                }

                cminfo->state = SAPP_CM_S_CLOSED;
            }
            
            p = (unsigned char *)(&(data->misc.cmaddr.addr));
            SAPP_DEBUG("%s: 0-0: %-25s: CM[%d:%d:%d:%d:%d]: state= %d\n",
                       SAPP_ID, fname,
                       p[0], p[1], p[2], p[3], ntohs(data->misc.cmaddr.port),
                       cminfo != NULL ? cminfo->state : SAPP_CM_S_CLOSED);
        }

        break;

    case (GAPI_STATUS_CM_OPENING):
        if (data != NULL) {
            cminfo = sapp_get_cminfo(data->misc.cmaddr.addr,
                                     data->misc.cmaddr.port,
                                     handle);
            if (cminfo == NULL) {
                cminfo = sapp_get_cminfo(0, 0, handle);
                if (cminfo != NULL) {
                    cminfo->cmaddr.addr = data->misc.cmaddr.addr;
                    cminfo->cmaddr.port = data->misc.cmaddr.port;
                    cminfo->state = SAPP_CM_S_CONNECTING;
                }
            }

            p = (unsigned char *)(&(data->misc.cmaddr.addr));
            SAPP_DEBUG("%s: 0-0: %-25s: CM[%d:%d:%d:%d:%d]: opening\n",
                       SAPP_ID, fname,
                       p[0], p[1], p[2], p[3], ntohs(data->misc.cmaddr.port));
        }
        break;

    case (GAPI_STATUS_CM_CONNECTED):
        if (data != NULL) {
            cminfo = sapp_get_cminfo(data->misc.cmaddr.addr,
                                     data->misc.cmaddr.port, handle);
            if (cminfo != NULL) {
                cminfo->state = SAPP_CM_S_CONNECTED;
            }

            p = (unsigned char *)(&(data->misc.cmaddr.addr));
            SAPP_DEBUG("%s: 0-0: %-25s: CM[%d:%d:%d:%d:%d]: connected\n",
                       SAPP_ID, fname,
                       p[0], p[1], p[2], p[3], ntohs(data->misc.cmaddr.port));
        }
        break;

    case (GAPI_STATUS_CM_REGISTERED):
        if (data != NULL) {
            cminfo = sapp_get_cminfo(data->misc.cmaddr.addr,
                                     data->misc.cmaddr.port, handle);
            if (cminfo != NULL) {
                cminfo->state = SAPP_CM_S_REGISTERING;
            }

            p = (unsigned char *)(&(data->misc.cmaddr.addr));
            SAPP_DEBUG("%s: 0-0: %-25s: CM[%d:%d:%d:%d:%d]: registered\n",
                       SAPP_ID, fname,
                       p[0], p[1], p[2], p[3], ntohs(data->misc.cmaddr.port));
        }
        break;

    case (GAPI_STATUS_CM_REGISTER_COMPLETE):
        SAPP_DEBUG("%s: 0-0: %-25s: CM: register complete\n",
                   SAPP_ID, fname);

        cmaddr = sapp_get_cmaddr_by_state(SAPP_CM_S_REGISTERING, handle);
        if (cmaddr != NULL) {
            cminfo = sapp_get_cminfo(cmaddr->addr, cmaddr->port, handle);
            if (cminfo != NULL) {
                cminfo->state = SAPP_CM_S_REGISTERED;
            }
        }

        if ((type == GAPI_STATUS_DATA_INFO) && (data != NULL)) {
            /*
             * Make sure all the enumerations match up.
             */
            if ((sinfo->sapp_session_data.linecnt       == data->info.linecnt) &&
                (sinfo->sapp_session_data.speeddialcnt  == data->info.speeddialcnt) &&
                (sinfo->sapp_session_data.featurecnt    == data->info.featurecnt) &&
                (sinfo->sapp_session_data.serviceurlcnt == data->info.serviceurlcnt) &&
                (sinfo->sapp_session_data.softkeycnt    == data->info.softkeycnt) &&
                (sinfo->sapp_session_data.softkeysetcnt == data->info.softkeysetcnt)) {
                
                SAPP_DEBUG("%s: 0-0: %-25s: CM: enumeration complete\n",
                           SAPP_ID, fname);
            }
            else {
                SAPP_DEBUG("%s: 0-0: %-25s: CM: enumeration incomplete\n",
                           SAPP_ID, fname);
            }
        }

        sinfo->sapp_state = SAPP_S_REGISTERED;

#if (SAPP_APP == SAPP_APP_SCCPTEST)
        //sccptest_sessionstatus(status);
#endif
        
        break;

    default:
        break;
    }

    return (0);
}

int sapp_sapp_resetsession_res (void* handle, gapi_msgs_e msg_id,
                                gapi_causes_e cause)
{
    char *fname = "sapp_resetsession_res";

    sapp_debug_entry(fname, 0, 0);

    SAPP_DEBUG("%s: 0-0: %-25s: cause= %d\n", SAPP_ID, fname, cause);

#if (SAPP_APP == SAPP_APP_SCCPTEST)
        //sccptest_sessionstatus(GAPI_STATUS_RESET_COMPLETE);
#endif
    
    return (0);
}

int sapp_sapp_linestat_res (void* handle, gapi_msgs_e msg_id,
                            gapi_line_t *line)
{
    char *fname = "sapp_linestat_res";
    sapp_info_t *sinfo = (sapp_info_t *)handle;

    sapp_debug_entry(fname, 0, 0);

    if (sinfo->sapp_session_data.linecnt < GAPI_MAX_LINES) {
        memcpy(&(sinfo->sapp_session_data.lines[sinfo->sapp_session_data.linecnt]), line,
               sizeof(gapi_line_t));
        
        sinfo->sapp_session_data.linecnt++;
    }

    return (0);
}

int sapp_sapp_speeddialstat_res (void* handle, gapi_msgs_e msg_id,
                                 gapi_speeddial_t *speeddial)
{
    char *fname = "sapp_speeddialstat_res";
    sapp_info_t *sinfo = (sapp_info_t *)handle;

    sapp_debug_entry(fname, 0, 0);

    if (sinfo->sapp_session_data.speeddialcnt < GAPI_MAX_SPEED_DIALS) {
        memcpy(&(sinfo->sapp_session_data.speeddials[sinfo->sapp_session_data.speeddialcnt]),
               speeddial, sizeof(gapi_speeddial_t));

        sinfo->sapp_session_data.speeddialcnt++;
    }

    return (0);
}

int sapp_sapp_featurestat_res (void* handle, gapi_msgs_e msg_id,
                               gapi_feature_t *feature)
{
    char *fname = "sapp_featurestat_res";
    sapp_info_t *sinfo = (sapp_info_t *)handle;

    sapp_debug_entry(fname, 0, 0);

    if (sinfo->sapp_session_data.featurecnt < GAPI_MAX_FEATURES) {
        memcpy(&(sinfo->sapp_session_data.features[sinfo->sapp_session_data.featurecnt]),
               feature, sizeof(gapi_feature_t));

        sinfo->sapp_session_data.featurecnt++;
    }

    return (0);
}

int sapp_sapp_serviceurlstat_res (void* handle, gapi_msgs_e msg_id,
                                  gapi_serviceurl_t *serviceurl)
{
    char *fname = "sapp_serviceurlstat_res";
    sapp_info_t *sinfo = (sapp_info_t *)handle;

    sapp_debug_entry(fname, 0, 0);

    if (sinfo->sapp_session_data.serviceurlcnt < GAPI_MAX_SERVICE_URLS) {
        memcpy(&(sinfo->sapp_session_data.lines[sinfo->sapp_session_data.serviceurlcnt]),
               serviceurl, sizeof(gapi_serviceurl_t));

        sinfo->sapp_session_data.serviceurlcnt++;
    }

    return (0);
}

int sapp_sapp_softkeytemplate_res (void* handle, gapi_msgs_e msg_id,
                                   gapi_softkey_template_t *softkey)
{
    char *fname = "sapp_softkeytemplate_res";
    unsigned long             i;
    gapi_softkey_definition_t *definition;
    sapp_info_t *sinfo = (sapp_info_t *)handle;

    sapp_debug_entry(fname, 0, 0);

    definition = (gapi_softkey_definition_t *)(softkey->definition);

    for (i = softkey->offset; softkey->count-- > 0; i++) {
        /*
         * Make sure we still have space for the data.
         */
        if ((i >= GAPI_MAX_SOFTKEY_DEFINITIONS) ||
            (i >= softkey->total_count)) {
            
            break;
        }

        memcpy(&(sinfo->sapp_session_data.softkeys[i]), definition,
               sizeof(gapi_softkey_definition_t));

        sinfo->sapp_session_data.softkeycnt++;
        definition++;
    }

    return (0);
}

int sapp_sapp_softkeyset_res (void* handle, gapi_msgs_e msg_id,
                              gapi_softkey_set_t *softkeyset)
{
    char *fname = "sapp_softkeyset_res";
    unsigned long             i;
    gapi_softkey_set_definition_t *definition;
    sapp_info_t *sinfo = (sapp_info_t *)handle;

    sapp_debug_entry(fname, 0, 0);

    definition = (gapi_softkey_set_definition_t *)(softkeyset->definition);

    for (i = softkeyset->offset; softkeyset->count-- > 0; i++) {
        /*
         * Make sure we still have space for the data.
         */
        if ((i >= GAPI_MAX_SOFTKEY_SET_DEFINITIONS) ||
            (i >= softkeyset->total_count)) {
            
            break;
        }

        memcpy(&(sinfo->sapp_session_data.softkeysets[i]), definition,
               sizeof(gapi_softkey_definition_t));

        sinfo->sapp_session_data.softkeysetcnt++;
        definition++;
    }

    return (0);
}

int sapp_sapp_conninfo (void* handle, gapi_msgs_e msg_id, int conn_id,
                        int line, gapi_conninfo_t *conninfo)
{
    char *fname = "sapp_conninfo";
    sapp_call_t *call;
    sapp_info_t *sinfo = (sapp_info_t *)handle;

    sapp_debug_entry(fname, conn_id, line);

#if 0
    SAPP_DEBUG("    calling_name: %s, calling_number: %s,\n"
               "    called_name: %s, called_number: %s,\n"
               "    connected_name: %s, connected_number: %s.\n",
               conninfo->calling_name, conninfo->calling_number,
               conninfo->called_name, conninfo->called_number,
               conninfo->connected_name, conninfo->connected_number);
#else
    SAPP_DEBUG("    calling_name: %s, calling_number: %s,\n"
               "    called_name: %s, called_number: %s,\n",
               conninfo->calling_name, conninfo->calling_number,
               conninfo->called_name, conninfo->called_number);
#endif

    platform_mutex_lock(sapp_calls.mutex, SAPP_SCCP_WAIT_TIMEOUT);
    
    call = sapp_get_call_by_conn_id(conn_id);
    if (call == NULL) {
        platform_mutex_unlock(sapp_calls.mutex);
        sinfo->sapp_sccp_cbs->release_complete(sinfo->sapp_sccp_handle,
                                        GAPI_MSG_RELEASE_COMPLETE,
                                        conn_id, line,
                                        GAPI_CAUSE_NO_RESOURCE, NULL);
        return (0);
    }

    sapp_sccp_to_sapp_conninfo(conninfo,
                               &(call->sapp_conninfo),
                               0);

    /*
     * Check if we had to hold up sending the call_state messages because
     * we were waiting for the call_info.
     */

	call->sapp_call_id = conn_id;

    if (call->gapi_waiting == 1)  {
        call->gapi_waiting = 0;
        SccpCallbackProxy::sapp_app_setup(call->sapp_call_id, call->sapp_line,
                       conninfo, 0, 0, 0, &(call->sapp_media),
                       NULL, 0);
    }        
    else if (call->gapi_waiting == 2) {
        call->gapi_waiting = 0;

        SccpCallbackProxy::sapp_app_proceeding(call->sapp_call_id, call->sapp_line,
                            &(call->sapp_conninfo));
    }
    else if (call->gapi_waiting == 3) {
        call->gapi_waiting = 0;

        SccpCallbackProxy::sapp_app_alerting(call->sapp_call_id, call->sapp_line,
                          &(call->sapp_conninfo), NULL, 0);    
    }

    platform_mutex_unlock(sapp_calls.mutex);

    return (0);
}

int sapp_sapp_starttone (void* handle, gapi_msgs_e msg_id, int conn_id,
                         int line, gapi_tones_e tone,
                         gapi_tone_directions_e direction)
{
    char *fname = "sapp_starttone";
    sapp_call_t *call;
    sapp_info_t *sinfo = (sapp_info_t *)handle;

    sapp_debug_entry(fname, conn_id, line);

    platform_mutex_lock(sapp_calls.mutex, SAPP_SCCP_WAIT_TIMEOUT);
    
    call = sapp_get_call_by_conn_id(conn_id);
    if (call == NULL) {
        platform_mutex_unlock(sapp_calls.mutex);
        sinfo->sapp_sccp_cbs->release_complete(sinfo->sapp_sccp_handle,
                                        GAPI_MSG_RELEASE_COMPLETE,
                                        conn_id, line,
                                        GAPI_CAUSE_NO_RESOURCE, NULL);
        return (0);
    }

#if (SAPP_APP == SAPP_APP_GSM)
    if ((tone == GAPI_TONE_BUSY) || (tone == GAPI_TONE_REORDER)) {
        sapp_app_release(call->sapp_call_id, call->sapp_line,
                         SAPP_CAUSE_ERROR);
    }
#endif
#if (SAPP_APP == SAPP_APP_SCCPTEST)
        //sccptest_starttone(call->sccp_conn_id, call->sapp_line, tone);
#endif
    platform_mutex_unlock(sapp_calls.mutex);

    return (0);
}

#if (SAPP_APP == SAPP_APP_GSM)

/*
 * sapp_config_get_ccm_port()
 *
 * Get table entry from the table string and option number
 */
static int sapp_config_get_ccm_port (int ccm)
{
    int port;

    config_get_value((SAPP_CFGID_CCM1_PORT + ccm - 1), &port, sizeof(port));
    
    if (port == 0) {
        config_get_value((SAPP_CFGID_CCM1_PORT), &port, sizeof(port));
    }

    return (port);    
} 

/*
 * sapp_config_get_ccm_addr()
 *
 * Get table entry from the table string and option number
 */
void sapp_config_get_ccm_addr (int ccm, char *buffer)
{
    config_get_string((SAPP_CFGID_CCM1_ADDRESS + ccm - 1), buffer);

#if 0
    if ((strcmp(buffer, "UNPROVISIONED") == 0) ||
        (buffer[0] == '\0')) {
        config_get_string((SAPP_CFGID_CCM1_ADDRESS), buffer);
    }
#endif
}

/*
 * sapp_config_get_ccm_ipaddr()
 *
 * Perform DNS lookup on the primary proxy name and
 * return its IP address. 
 *
 * Note: the IP Address is returned in the non-Telecaster
 *       SIP format, which is not byte reversed.
 *       Eg. 0xac2c33f8 = 161.44.51.248
 */
unsigned long sapp_config_get_ccm_ipaddr (int ccm)
{
    unsigned long IPAddress = 0;
    unsigned short port;
    t_SrvHandle srv_order = NULL;
    int dnsErrorCode = 0;
    char addr[MAX_IPADDR_STR_LEN];
    
    sapp_config_get_ccm_addr(ccm, addr);

    if ((strcmp(addr, "UNPROVISIONED") == 0) ||
        (addr[0] == '\0')) {
        return (0);
    }
    
    dnsErrorCode = sip_dns_gethostbysrv(addr, &IPAddress, &port, &srv_order);
    if (srv_order) {
       dns_free_call_order(srv_order);
    }
    if (dnsErrorCode != 0){//DNS_OK){
       dnsErrorCode = dns_gethostbyname(addr, &IPAddress, 100, 1);
    }
    return (ntohl(IPAddress));
}

static int sapp_line_protocol[6] = {0, 0, 0, 0, 0, 0};

int sapp_test (void)
{
    gapi_cmaddr_t cms[5];
    unsigned char *p;
    char *mac;
    gapi_media_caps_t caps;
    gapi_opensession_values_t values;
    int i;
    int value;

    /*
     * Make sure SAPP has been initialized. Otherwise, initialize it now.
     */
    if (sapp_initialized != 1) {
        sapp_init();
        sapp_initialized = 1;
    }

    for (i = 0; i < 6; i++) {
        config_get_value(SAPP_CFGID_LINE1_PROTOCOL + i, &value, sizeof(value));

        if (value != 0) {
            sapp_line_protocol[i] = 1;
        }
    }

    memset(cms, 0, sizeof(*cms) * 5);

#if 0
    mac = "000347B98077";

    /*
     * Stack expects address and port to be in network order.
     */
    cms[0].addr = htonl(0xac12bc3c); /* 172.18.188.60 */
    cms[0].port = htons(2000);
#else

    {
        char ethernet_mac[16];
        unsigned char ethernet_mac_raw[6];
        char name[32];

        /*
         * Check if the mac address has been provisioned in
         * line 6 displayname. If not, grab the real mac.
         */
        sip_config_get_display_name(6, name);
        if (strcmp(name, "UNPROVISIONED") == 0) {
            mac = ethernet_mac;
            if (getAdapterAddr(ethernet_mac, ethernet_mac_raw) < 0) {
                /*
                 * Couldn't get the real mac so just stick one
                 * in there.
                 */
                strncpy(ethernet_mac, "000347B98077", 16);
            }
        }
        else {
            mac = name;
        }
    }

#if 0
    cms[0].addr = htonl(sapp_config_get_proxy_ipaddr(5));
    cms[0].port = htons((unsigned short)sapp_config_get_proxy_port(5));
    cms[1].addr = htonl(sapp_config_get_proxy_ipaddr(6));
    cms[1].port = htons((unsigned short)sapp_config_get_proxy_port(6));

    cms[4].addr = htonl(sapp_config_get_proxy_ipaddr(1));
    cms[4].port = htons((unsigned short)sapp_config_get_proxy_port(1));

    /*
     * Reset the second cm addr if it is the same as the first. This happens
     * if the user did not configure line 6. The sip_config routine will
     * grab the default line's proxy_addr.
     */
    if ((cms[1].addr == cms[4].addr) && (cms[1].addr == cms[4].addr)) {
        cms[1].addr = 0;
        cms[1].port = 0;
    }

    cms[4].addr = 0;
    cms[4].port = 0;
#endif

    for (i = 0; i < 5; i++) {
        cms[i].addr = htonl(sapp_config_get_ccm_ipaddr(i + 1));
        cms[i].port = htons((unsigned short)sapp_config_get_ccm_port(i + 1));
    }

#endif
    for (i = 0; i < 2; i++) {
        printf("0x%08x:%04x\n", cms[i].addr, cms[i].port);
    
        p = (unsigned char *)(&(cms[i].addr));
        printf("CM[%d] [%s] %d.%d.%d.%d:%d\n",
               i, mac, p[0], p[1], p[2], p[3], ntohs(cms[i].port));
    }

    caps.count = 1;
    caps.caps[0].payload = GAPI_PAYLOAD_TYPE_G711_ULAW_64K;
    caps.caps[0].milliseconds_per_packet = 20;

    memset(&values, 0, sizeof(values));

    //values.connect_to = 10000;
    printf("TEST: Opening SCCP session...\n");
    sapp_opensession_req(cms, mac, NULL,
                         GAPI_SRST_MODE_DISABLE, NULL, &caps, &values,
                         GAPI_PROTOCOL_VERSION_SEAVIEW);

    return (0);
}
#if 0
int sapp_check_if_sccp (int line)
{
    int rc = 0;
    int i;

    for (i = 0; i < 6; i++) {
        if ((sapp_line_protocol[i] = 1) || //CFG_PROTOCOL_SEAVIEW || CFG_PROTOCOL_PARCHE
            (sapp_line_protocol[i] = 2))
    }

    return (rc);
}
#endif
int sapp_is_this_sccp (void *data)
{
    cc_msg_t    *msg = (cc_msg_t *)data;
    sapp_call_t *call;

    if (sapp_initialized != 1) {
        return (0);
    }
    
    /*
     * Grab messages for the SCCP lines.
     */
    //if ((msg->msg.setup.line == 1) || (msg->msg.setup.line == 2)) {
    //if (msg->msg.setup.line == 1) {
    if (sapp_line_protocol[msg->msg.setup.line - 1] == 1) {
        /*
         * Pass incoming calls to a SCCP line to the SIP stack if the call
         * originated as a SIP call. This will be the case if there is not a 
         * sapp call associated with this call_id.
         */
        if (msg->msg.setup.msg_id != CC_MSG_SETUP) {
            platform_mutex_lock(sapp_calls.mutex, SAPP_SCCP_WAIT_TIMEOUT);
    
            call = sapp_get_call_by_call_id(msg->msg.setup.call_id);
            if (call == NULL) {
                platform_mutex_unlock(sapp_calls.mutex);
                return (0);
            }
            platform_mutex_unlock(sapp_calls.mutex);
        }

        return (1);
    }
    else {
        return (0);
    }
}

int sapp_process_cc_event (void *data)
{
    char *fname = "sapp_process_cc_event";
    cc_msg_t *msg = (cc_msg_t *)data;

    if (sapp_is_this_sccp(data) == 0) {
        return (1);
    }

    switch (msg->msg.setup.msg_id)  {
    case (CC_MSG_SETUP):
        if (sapp_state != SAPP_S_REGISTERED) {
            cc_release(CC_SRC_SIP, msg->msg.setup.call_id,
                       msg->msg.setup.line,
                       CC_CAUSE_NORMAL, NULL);

#if 0
            cc_release_complete(CC_SRC_SIP, msg->msg.setup.call_id,
                                msg->msg.setup.line,
                                CC_CAUSE_NORMAL);
#endif
            break;
        }

        sapp_setup(msg->msg.setup.call_id, msg->msg.setup.line,
                   &(msg->msg.setup.caller_id),
                   (char *)(msg->msg.setup.caller_id.called_number),
                   strlen(msg->msg.setup.caller_id.called_number),
                   &(msg->msg.setup.sdp), 
                   msg->msg.setup.alert_info, 0);

        break;
        
    case (CC_MSG_SETUP_ACK):
        sapp_setup_ack(msg->msg.setup_ack.call_id, msg->msg.setup_ack.line,
                       &(msg->msg.setup_ack.sdp));
        break;                                      

    case (CC_MSG_PROCEEDING):
        sapp_proceeding(msg->msg.proceeding.call_id, msg->msg.proceeding.line);

        break;                                      

    case (CC_MSG_ALERTING):        
        sapp_alerting(msg->msg.alerting.call_id, msg->msg.alerting.line);

        break;                                    

    case (CC_MSG_CONNECTED):
        sapp_connect(msg->msg.connected.call_id, msg->msg.connected.line,
                     &(msg->msg.connected.sdp));

        break;

    case (CC_MSG_CONNECTED_ACK):
        sapp_connect_ack(msg->msg.setup.call_id, msg->msg.setup.line);

        break;
                                           
    case (CC_MSG_RELEASE):

        sapp_release(msg->msg.release.call_id, msg->msg.release.line,
                     msg->msg.release.cause);

        break;                                      

    case (CC_MSG_RELEASE_COMPLETE):
        sapp_release_complete(msg->msg.release_complete.call_id,
                              msg->msg.release_complete.line,
                              msg->msg.release_complete.cause);

        break;            
        
    case (CC_MSG_FEATURE):
        sapp_feature_req(msg->msg.feature.call_id, msg->msg.feature.line,
                         msg->msg.feature.feature_id, NULL, CC_CAUSE_OK);

        break;            

    default:
        SAPP_DEBUG("%s: %s: Tossing unused cc event.\n", SAPP_ID, fname);    
    }

    SAPP_DEBUG("%s: %s: rc= %d.\n", SAPP_ID, fname, 0);

    free(data);
    
    return (0);
}
#endif /* #if (SAPP_APP == SAPP_APP_GSM) */