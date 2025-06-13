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
 *     sapp.c
 *
 *  CREATION/MODIFICATION DATES
 *     Created:  November 2002, Sam Hague
 *     Modified: 
 *
 *  DESCRIPTION
 *     SCCP Application platform dependent implementation
 *     (win32, unix)
 */

#include "platform.h"
#include <stdio.h>
#include <malloc.h>
#include <memory.h>
#include "sapp.h"
#include "gapi.h"
//#include "timer.h"
//#include "ssapi.h"
//#include "sccp.h"
#include "sccptest.h"


#if 0
#ifdef SCCP_PLATFORM_WINDOWS
#define SAPP_USE_SCCPTEST
#include <winsock2.h>
#include <rpc.h>
#include <time.h>
#endif
#endif

#if 0
#ifdef SCCP_PLATFORM_UNIX
#define SAPP_USE_SCCPTEST
//#define SAPP_ADD_RECURSION   /* used when recursive mutexes are not supported by the platform. */
#ifndef SCCP_PLATFORM_UNIX_WIN  // used when compiling under windows
#include <sys/socket.h>
#include <sys/time.h>
#else
#include <winsock2.h>
#endif
#endif
#endif


extern sapp_calls_t sapp_calls;
extern gapi_callbacks_t *sapp_sccp_cbs;
extern void *sapp_sccp_handle;
extern void *sapp_sapp_handle;
extern sapp_resetting;
extern int sapp_registered;
extern int sapp_debug;
extern gapi_status_data_info_t sapp_session_data;


#if 0
void sapp_set_new_call_data (sapp_call_t *call, int direction,
                             int conn_id, int call_id, int line)
{
//    platform_mutex_lock(sapp_calls.mutex, SAPP_SCCP_WAIT_TIMEOUT);

    switch (direction) {
    case (0): /* outgoing */
        call->sapp_call_id = call_id;
        call->sccp_conn_id = gapi_get_new_conn_id();

        call->sapp_line = line;
        call->sccp_line = line;

        break;

    case (1): /* incoming */
        call->sapp_call_id = sccptest_get_new_call_id();
        call->sccp_conn_id = conn_id;

        call->sapp_line = line;
        call->sccp_line = line;

    }

//    platform_mutex_unlock(sapp_calls.mutex);
}
#endif
static void sapp_set_new_call_data (sapp_call_t *call, sapp_direction_e direction,
                                    int sccp_conn_id, int sapp_call_id,
                                    int sccp_line, int sapp_line,
                                    void *sccp_media, void *sapp_media,
                                    void *sccp_conninfo, void *sapp_conninfo)
{
#ifdef SCCP_HAS_RECURSION
    platform_mutex_lock(sapp_calls.mutex, SAPP_SCCP_WAIT_TIMEOUT);
#endif

    sapp_calls.count++;

    switch (direction) {
    case (SAPP_DIRECTION_OUTGOING):
        call->sccp_conn_id = gapi_get_new_conn_id();
        call->sapp_call_id = sapp_call_id;

        call->sccp_line = sccp_line;
        call->sapp_line = sapp_line;

        break;

    case (SAPP_DIRECTION_INCOMING):
        call->sccp_conn_id = sccp_conn_id;
        call->sapp_call_id = sccptest_get_new_call_id();

        call->sccp_line = sccp_line;
        call->sapp_line = sapp_line;

        break;
    }

#ifdef SCCP_HAS_RECURSION
    platform_mutex_unlock(sapp_calls.mutex);
#endif
}

/*
 * Application wrapper functions. The application uses these functions to
 * access GAPI.
 */
int sapp_setup (int call_id, int line,
                gapi_conninfo_t *conninfo, char *digit, int numdigits,
                gapi_media_t *media, int alert_info, int privacy)
{
    char *fname = "sapp_setup";
    sapp_call_t *call;

    sapp_debug_entry(fname, call_id, line);

    platform_mutex_lock(sapp_calls.mutex, SAPP_SCCP_WAIT_TIMEOUT);
    
    call = sapp_get_call_by_call_id(SAPP_NO_CONN_ID);
    if (call == NULL) {
        platform_mutex_unlock(sapp_calls.mutex);
        sccptest_release_complete(call_id, line, GAPI_CAUSE_NO_RESOURCE);
    }

    sapp_set_new_call_data(call, SAPP_DIRECTION_OUTGOING,
                           SAPP_NO_CONN_ID, call_id, line, line,
                           &(call->sccp_media), media,
                           &(call->sccp_conninfo), &(call->sapp_conninfo));

    sapp_sccp_cbs->setup(sapp_sccp_handle, GAPI_MSG_SETUP,
                         call->sccp_conn_id, call->sccp_line,
                         conninfo, digit, numdigits,
                         media, alert_info, privacy);


    platform_mutex_unlock(sapp_calls.mutex);

    return (0);
}

int sapp_setup_ack (int call_id, int line, cc_sdp_t *sdp)
{
    char *fname = "sapp_setup_ack";
    sapp_call_t *call;

    sapp_debug_entry(fname, call_id, line);

    platform_mutex_lock(sapp_calls.mutex, SAPP_SCCP_WAIT_TIMEOUT);
    
    call = sapp_get_call_by_call_id(call_id);
    if (call == NULL) {
        platform_mutex_unlock(sapp_calls.mutex);
        sccptest_release_complete(call_id, line, GAPI_CAUSE_NO_RESOURCE);
        return (0);
    }

    call->sapp_line = line;

#if 0
    /*
     * GSM sends the local sdp info, so save it away for when
     * SCCP requests the open_rcv.
     */
    sapp_copy_sdp_to_media(&(call->sccp_media), sdp);
#endif
    sapp_sccp_cbs->setup_ack(sapp_sccp_handle, GAPI_MSG_SETUP_ACK,
                             call->sccp_conn_id, call->sccp_line,
                             NULL, NULL, GAPI_CAUSE_OK);

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
        sccptest_release_complete(call_id, line, GAPI_CAUSE_NO_RESOURCE);
    }

    sapp_sccp_cbs->proceeding(sapp_sccp_handle, GAPI_MSG_PROCEEDING,
                              call->sccp_conn_id, call->sccp_line, NULL);

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
        sccptest_release_complete(call_id, line, GAPI_CAUSE_NO_RESOURCE);
        return (0);
    }

    sapp_sccp_cbs->alerting(sapp_sccp_handle, GAPI_MSG_ALERTING,
                            call->sccp_conn_id, call->sccp_line,
                            NULL, NULL, 0);

    platform_mutex_unlock(sapp_calls.mutex);

    return (0);
}

int sapp_connect (int call_id, int line, cc_sdp_t *sdp)
{
    char *fname = "sapp_connect";
    sapp_call_t *call;

    sapp_debug_entry(fname, call_id, line);

    platform_mutex_lock(sapp_calls.mutex, SAPP_SCCP_WAIT_TIMEOUT);
    
    call = sapp_get_call_by_call_id(call_id);
    if (call == NULL) {
        platform_mutex_unlock(sapp_calls.mutex);
        cc_release_complete(CC_SRC_SIP, call_id, line, CC_CAUSE_NORMAL);    
        return (0);
    }
    /*
     * GSM sends the local sdp info, so save it away for when
     * SCCP requests the open_rcv.
     */
    sapp_copy_sdp_to_media(&(call->sccp_media), sdp);

    sapp_sccp_cbs->connect(sapp_sccp_handle, GAPI_MSG_CONNECT,
                           call->sccp_conn_id, call->sccp_line,
                           NULL, NULL);

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
        sccptest_release_complete(call_id, line, GAPI_CAUSE_NO_RESOURCE);
    }

    sapp_sccp_cbs->connect_ack(sapp_sccp_handle, GAPI_MSG_CONNECT_ACK,
                               call->sccp_conn_id, call->sccp_line,
                               NULL, NULL);
    
    platform_mutex_unlock(sapp_calls.mutex);

    return (0);
}

int sapp_release (int call_id, int line, gapi_causes_e cause)
{
    char *fname = "sapp_release";
    sapp_call_t *call;

    sapp_debug_entry(fname, call_id, line);

    platform_mutex_lock(sapp_calls.mutex, SAPP_SCCP_WAIT_TIMEOUT);
    
    call = sapp_get_call_by_call_id(call_id);
    if (call == NULL) {
        platform_mutex_unlock(sapp_calls.mutex);
        sccptest_release_complete(call_id, line, GAPI_CAUSE_NO_RESOURCE);
    }

    sapp_sccp_cbs->release(sapp_sccp_handle, GAPI_MSG_RELEASE,
                           call->sccp_conn_id, call->sccp_line, cause);
    
    platform_mutex_unlock(sapp_calls.mutex);
    
    return (0);
}

int sapp_release_complete (int call_id, int line, gapi_causes_e cause)
{
    char *fname = "sapp_release_complete";
    sapp_call_t *call;

    sapp_debug_entry(fname, call_id, line);

    platform_mutex_lock(sapp_calls.mutex, SAPP_SCCP_WAIT_TIMEOUT);
    
    call = sapp_get_call_by_call_id(call_id);
    if (call == NULL) {
        platform_mutex_unlock(sapp_calls.mutex);
        sccptest_release_complete(call_id, line, GAPI_CAUSE_NO_RESOURCE);
    }

    sapp_sccp_cbs->release_complete(sapp_sccp_handle,
                                    GAPI_MSG_RELEASE_COMPLETE,
                                    call->sccp_conn_id, call->sccp_line, cause);

    sapp_free_call(call);

    platform_mutex_unlock(sapp_calls.mutex);

    return (0);
}

int sapp_feature_req (int call_id, int line, gapi_features_e feature,
                      gapi_feature_data_u *data)
{
    char *fname = "sapp_feature_req";
    sapp_call_t *call;

    sapp_debug_entry(fname, call_id, line);

    platform_mutex_lock(sapp_calls.mutex, SAPP_SCCP_WAIT_TIMEOUT);
    
    call = sapp_get_call_by_call_id(call_id);
    if (call == NULL) {
        platform_mutex_unlock(sapp_calls.mutex);
        sccptest_release_complete(call_id, line, GAPI_CAUSE_NO_RESOURCE);
    }

    sapp_sccp_cbs->feature_req(sapp_sccp_handle,
                               GAPI_MSG_FEATURE_REQ,
                               call->sccp_conn_id, call->sccp_line,
                               feature, data);

    platform_mutex_unlock(sapp_calls.mutex);

    return (0);
}

int sapp_opensession_req (gapi_cmaddr_t *cms, char *mac,
                          gapi_cmaddr_t *srsts, gapi_srst_modes_e srst_mode,
                          gapi_cmaddr_t *tftp, gapi_media_caps_t *media_caps)
{
    char *fname = "sapp_opensession_req";

    sapp_debug_entry(fname, 0, 0);

    /*
     * Check if we are already resetting. If so, then don't let
     * the app try to open until the reset is complete.
     */
    if (sapp_resetting == 1) {
        return (0);
    }

    sapp_sccp_cbs->opensession_req(sapp_sccp_handle,
                                   GAPI_MSG_OPENSESSION_REQ,
                                   cms, mac, srsts, srst_mode,
                                   tftp, media_caps);

    return (0);
}

int sapp_closesession_req (void)
{
    char *fname = "sapp_closesession_req";

    sapp_debug_entry(fname, 0, 0);

    /*
     * Check if we are already resettijng. If so, then don't let
     * the app try to reset.
     */
    if (sapp_resetting == 1) {
        return (0);
    }

    sapp_resetting = 1;

    sapp_sccp_cbs->closesession_req(sapp_sccp_handle,
                                    GAPI_MSG_CLOSESESSION_REQ);

    return (0);
}

int sapp_resetsession_req (char *mode)
{
    char          *fname = "sapp_resetsession_req";
    gapi_causes_e cause;

    sapp_debug_entry(fname, 0, 0);

    /*
     * Check if we are already resettijng. If so, then don't let
     * the app try to reset.
     */
    if (sapp_resetting == 1) {
        return (0);
    }

    sapp_resetting = 1;

    if (mode[0] == '1') {
        cause = GAPI_CAUSE_CM_RESET;
    }
    else {
        cause = GAPI_CAUSE_CM_RESTART;
    }

    sapp_sccp_cbs->resetsession_req(sapp_sccp_handle,
                                    GAPI_MSG_RESETSESSION_REQ,
                                    cause);

    return (0);
}

static int sapp_get_new_call_id (void)
{
    static int id = 0;

    if (++id < 0) {
        id = 1;
    }

    return (id);
}

static sapp_copy_conninfo_to_gapi (gapi_conninfo_t *conninfo, void *info,
                                   int direction)
{
    gapi_conninfo_t *gapiinfo = (gapi_conninfo_t *)info;

    if (direction == 0) { /* outgoing */
        if (conninfo != NULL) {
            strncpy(gapiinfo->called_number, conninfo->called_number,
                    GAPI_DIRECTORY_NUMBER_SIZE);
        }
    }
}

/*
 * SAPP callbacks. These are the callbacks that SAPP gives to the stack.
 */
static int sapp_sapp_setup (void *handle, int msg_id, int conn_id, int line,
                            gapi_conninfo_t *conninfo, char *digit,
                            int numdigits,
                            gapi_media_t *media, int alert_info, int privacy)
{
    char *fname = "sapp_sapp_setup";
#ifdef SAPP_USE_SCCPTEST    
//    sapp_call_t *call;
#endif

    sapp_debug_entry(fname, conn_id, line);

    sapp_call_count++;

    sapp_sccp_conn_id = conn_id;
    //sapp_sapp_call_id = sapp_get_new_call_id();
    sapp_sapp_call_id = conn_id;

    sapp_copy_conninfo_to_gapi(conninfo, &sapp_sapp_conninfo2, 0);

    sapp_sccp_cbs->setup_ack(sapp_sccp_handle, GAPI_MSG_SETUP_ACK,
                             sapp_sccp_conn_id, sapp_sccp_line, conninfo,
                             NULL, GAPI_CAUSE_OK);

    sapp_sccp_cbs->proceeding(sapp_sccp_handle, GAPI_MSG_PROCEEDING,
                              sapp_sccp_conn_id, sapp_sccp_line, conninfo);

    sapp_sccp_cbs->alerting(sapp_sccp_handle, GAPI_MSG_ALERTING,
                            sapp_sccp_conn_id, sapp_sccp_line, conninfo,
                            media, 0);

    sapp_sccp_cbs->connect(sapp_sccp_handle, GAPI_MSG_CONNECT,
                           sapp_sccp_conn_id, sapp_sccp_line, conninfo, media);

    return (0);
}

static int sapp_sapp_setup_ack (void *handle, int msg_id, int conn_id, int line,
                                gapi_conninfo_t *conninfo, gapi_media_t *media,
                                gapi_causes_e cause)
{
    char *fname = "sapp_sapp_setup_ack";
#ifdef SAPP_USE_SCCPTEST    
    sapp_call_t *call;
#endif

    sapp_debug_entry(fname, conn_id, line);

//    sapp_sccp_conn_id = conn_id;

#ifdef SAPP_USE_SCCPTEST    
    platform_mutex_lock(sapp_calls.mutex, SAPP_SCCP_WAIT_TIMEOUT);
    
    call = sapp_get_call_by_conn_id(conn_id);
    if (call == NULL) {
        platform_mutex_unlock(sapp_calls.mutex);
        sapp_sccp_cbs->release_complete(sapp_sccp_handle, GAPI_MSG_RELEASE,
                                        conn_id, line,
                                        GAPI_CAUSE_NO_RESOURCE);
    }

    sccptest_setup_ack(call->sapp_call_id, call->sapp_line, cause);

    platform_mutex_unlock(sapp_calls.mutex);
#else
    if (cause == GAPI_CAUSE_OK) {

        SAPP_DEBUG("%s: %d-%d: %s: sapp_sccp_conn_id= %d\n",
                   SAPP_ID, conn_id, line, fname, sapp_sccp_conn_id);
    }
    else  {
        sapp_sccp_cbs->release(sapp_sccp_handle, GAPI_MSG_RELEASE, conn_id,
                              line, cause);
    }
#endif
    return (0);
}

static int sapp_sapp_proceeding (void *handle, int msg_id, int conn_id, int line,
                                 gapi_conninfo_t *conninfo)
{
    char *fname = "sapp_sapp_proceeding";
#ifdef SAPP_USE_SCCPTEST    
    sapp_call_t *call;
#endif

    sapp_debug_entry(fname, conn_id, line);

#ifdef SAPP_USE_SCCPTEST        
    platform_mutex_lock(sapp_calls.mutex, SAPP_SCCP_WAIT_TIMEOUT);
    
    call = sapp_get_call_by_conn_id(conn_id);
    if (call == NULL) {
        platform_mutex_unlock(sapp_calls.mutex);
        sapp_sccp_cbs->release_complete(sapp_sccp_handle, GAPI_MSG_RELEASE,
                                        conn_id, line,
                                        GAPI_CAUSE_NO_RESOURCE);
    }

    sccptest_proceeding(call->sapp_call_id, call->sapp_line, 0);

    platform_mutex_unlock(sapp_calls.mutex);
#endif

    return (0);
}

static int sapp_sapp_alerting (void *handle, int msg_id, int conn_id, int line,
                               gapi_conninfo_t *conninfo, gapi_media_t *media,
                               int inband)
{
    char *fname = "sapp_sapp_alerting";
#ifdef SAPP_USE_SCCPTEST    
    sapp_call_t *call;
#endif

    sapp_debug_entry(fname, conn_id, line);

#ifdef SAPP_USE_SCCPTEST    
    platform_mutex_lock(sapp_calls.mutex, SAPP_SCCP_WAIT_TIMEOUT);
    
    call = sapp_get_call_by_conn_id(conn_id);
    if (call == NULL) {
        platform_mutex_unlock(sapp_calls.mutex);
        sapp_sccp_cbs->release_complete(sapp_sccp_handle, GAPI_MSG_RELEASE,
                                        conn_id, line,
                                        GAPI_CAUSE_NO_RESOURCE);
    }

    sccptest_alerting(call->sapp_call_id, call->sapp_line, 0);

    platform_mutex_unlock(sapp_calls.mutex);
#endif

    return (0);
}

static int sapp_sapp_connect (void *handle, int msg_id, int conn_id, int line,
                              gapi_conninfo_t *conninfo, gapi_media_t *media)
{             
    char *fname = "sapp_sapp_connect";
#ifdef SAPP_USE_SCCPTEST    
    sapp_call_t *call;
#endif

    sapp_debug_entry(fname, conn_id, line);

#ifdef SAPP_USE_SCCPTEST
    platform_mutex_lock(sapp_calls.mutex, SAPP_SCCP_WAIT_TIMEOUT);
    
    call = sapp_get_call_by_conn_id(conn_id);
    if (call == NULL) {
        platform_mutex_unlock(sapp_calls.mutex);
        sapp_sccp_cbs->release_complete(sapp_sccp_handle, GAPI_MSG_RELEASE,
                                        conn_id, line,
                                        GAPI_CAUSE_NO_RESOURCE);
    }

    sccptest_connect(call->sapp_call_id, call->sapp_line, 0);

    platform_mutex_unlock(sapp_calls.mutex);
#else
    sapp_sccp_cbs->connect_ack(sapp_sccp_handle, GAPI_MSG_CONNECT_ACK,
                              sapp_sccp_conn_id, sapp_sccp_line,
                              NULL, NULL);
#endif

    return (0);
}

static int sapp_sapp_connect_ack (void *handle, int msg_id, int conn_id,
                                  int line, gapi_conninfo_t *conninfo,
                                  gapi_media_t *media)
{
    char *fname = "sapp_sapp_connect_ack";
#ifdef SAPP_USE_SCCPTEST    
    sapp_call_t *call;
#endif

    sapp_debug_entry(fname, conn_id, line);

#ifdef SAPP_USE_SCCPTEST
    platform_mutex_lock(sapp_calls.mutex, SAPP_SCCP_WAIT_TIMEOUT);
    
    call = sapp_get_call_by_conn_id(conn_id);
    if (call == NULL) {
        platform_mutex_unlock(sapp_calls.mutex);
        sapp_sccp_cbs->release_complete(sapp_sccp_handle, GAPI_MSG_RELEASE,
                                        conn_id, line,
                                        GAPI_CAUSE_NO_RESOURCE);
    }

    sccptest_connect_ack(call->sapp_call_id, call->sapp_line, 0);

    platform_mutex_unlock(sapp_calls.mutex);
#endif

    return (0);
}

static int sapp_sapp_disconnect (void *handle, int msg_id, int conn_id, int line,
                                 gapi_causes_e cause)
{
    char *fname = "sapp_sapp_disconnect";

    sapp_debug_entry(fname, conn_id, line);
    
    return (0);
}

static int sapp_sapp_release (void *handle, int msg_id, int conn_id, int line,
                              gapi_causes_e cause)
{
    char *fname = "sapp_sapp_release";
#ifdef SAPP_USE_SCCPTEST    
    sapp_call_t *call;
#endif

    sapp_debug_entry(fname, conn_id, line);

    sapp_call_count++;

#ifdef SAPP_USE_SCCPTEST
    platform_mutex_lock(sapp_calls.mutex, SAPP_SCCP_WAIT_TIMEOUT);
    
    call = sapp_get_call_by_conn_id(conn_id);
    if (call == NULL) {
        platform_mutex_unlock(sapp_calls.mutex);
        sapp_sccp_cbs->release_complete(sapp_sccp_handle, GAPI_MSG_RELEASE,
                                        conn_id, line,
                                        GAPI_CAUSE_NO_RESOURCE);
    }

    sccptest_release(call->sapp_call_id, call->sapp_line, cause);

    platform_mutex_unlock(sapp_calls.mutex);
#else
    sapp_sccp_cbs->release_complete(sapp_sccp_handle, GAPI_MSG_RELEASE, conn_id,
                                   line, cause);
#endif
    
    return (0);
}

static int sapp_sapp_release_complete (void *handle, int msg_id, int conn_id,
                                       int line, gapi_causes_e cause)
{
    char *fname = "sapp_sapp_release_complete";
#ifdef SAPP_USE_SCCPTEST    
    sapp_call_t *call;
#endif

    sapp_debug_entry(fname, conn_id, line);

    sapp_call_count--;

#ifdef SAPP_USE_SCCPTEST
    platform_mutex_lock(sapp_calls.mutex, SAPP_SCCP_WAIT_TIMEOUT);
    
    call = sapp_get_call_by_conn_id(conn_id);
    if (call == NULL) {
        platform_mutex_unlock(sapp_calls.mutex);
        sapp_sccp_cbs->release_complete(sapp_sccp_handle, GAPI_MSG_RELEASE,
                                        conn_id, line,
                                        GAPI_CAUSE_NO_RESOURCE);
    }

    sccptest_release_complete(call->sapp_call_id, call->sapp_line, cause);

    platform_mutex_unlock(sapp_calls.mutex);
#endif

    return (0);
}

static int sapp_sapp_openrcv_req (void *handle, int msg_id, int conn_id,
                                  int line, gapi_media_t *media)
{
    char          *fname = "sapp_sapp_openrcv_req";
    unsigned long addr   = 0;
    int           i;
    
    sapp_debug_entry(fname, conn_id, line);

    /*
     * Just grab the sockname for any socket that is open,
     * since this will identify our local address.
     */
    platform_mutex_lock(sapp_sockets2.mutex, SAPP_SCCP_WAIT_TIMEOUT);

    for (i = 0; i < SAPP_MAX_SOCKETS; i++) {
        if (sapp_sockets2.sockets[i].id != 0) {
            addr = sapp_socket_getsockname(sapp_sockets2.sockets[i].id);
            break;
        }
    }
    
    platform_mutex_unlock(sapp_sockets2.mutex);

    media->sccp_media.addr = addr;
    media->sccp_media.port = htons(16384);

    sapp_sccp_cbs->openrcv_res(sapp_sccp_handle, GAPI_MSG_OPENRCV_RES, conn_id,
                               line, media, GAPI_CAUSE_OK);

    return (0);
}

static int sapp_sapp_closercv (void *handle, int msg_id, int conn_id,
                               int line, gapi_media_t *media)
{
    char *fname = "sapp_sapp_closercv";

    sapp_debug_entry(fname, conn_id, line);

    return (0);
}

static int sapp_sapp_startxmit (void *handle, int msg_id, int conn_id,
                                int line, gapi_media_t *media)
{
    char *fname = "sapp_sapp_startxmit";
    
    sapp_debug_entry(fname, conn_id, line);

    return (0);
}

static int sapp_sapp_stopxmit (void *handle, int msg_id, int conn_id,
                               int line, gapi_media_t *media)
{
    char *fname = "sapp_sapp_stopxmit";

    sapp_debug_entry(fname, conn_id, line);

    return (0);
}

static int sapp_sapp_feature_res (void *handle, int msg_id, int conn_id,
                                  int line, gapi_features_e feature,
                                  gapi_feature_data_u *data, gapi_causes_e cause)
{
    char *fname = "sapp_sapp_feature_res";
    
    sapp_debug_entry(fname, conn_id, line);

    return (0);
}

static int sapp_sapp_opensession_res (void *handle, int msg_id,
                                      gapi_causes_e cause)
{
    char *fname = "sapp_sapp_opensession_res";

    sapp_debug_entry(fname, 0, 0);

    SAPP_DEBUG("%s: 0-0: %-25s: cause= %d\n", SAPP_ID, fname, cause);
    
    return (0);
}

static int sapp_sapp_closesession_res (void *handle, int msg_id,
                                       gapi_causes_e cause)
{
    char *fname = "sapp_sapp_closesession_res";

    sapp_debug_entry(fname, 0, 0);

    SAPP_DEBUG("%s: 0-0: %-25s: cause= %d\n", SAPP_ID, fname, cause);

    sapp_resetting = 0;

    sccptest_closesession_res();
  
    return (0);
}

static int sapp_sapp_sessionstatus (void *handle, int msg_id,
                                    gapi_status_e status, 
                                    gapi_status_data_types_e type,
                                    gapi_status_data_u *data)
{
    char *fname = "sapp_sapp_sessionstatus";

    sapp_debug_entry(fname, 0, 0);

    SAPP_DEBUG("%s: 0-0: %-25s: status= %d\n", SAPP_ID, fname, status);
    
    switch (status) {
    case (GAPI_STATUS_RESET_COMPLETE):
        sapp_resetting = 0;
        memset(&sapp_session_data, 0, sizeof(sapp_session_data));
        break;

    case (GAPI_STATUS_REGISTERED):
        if ((type == GAPI_STATUS_DATA_INFO) && (data != NULL)) {
            memcpy(&sapp_session_data, &(data->info),
                   sizeof(sapp_session_data));
        }

        sccptest_sessionstatus();
        
        break;

    default:
        break;
    }

    return (0);
}

static int sapp_sapp_resetsession_req (void *handle, int msg_id,
                                       gapi_causes_e cause)
{
    char *fname = "sapp_sapp_resetsession_req";
    sapp_debug_entry(fname, 0, 0);

    SAPP_DEBUG("%s: 0-0: %-25s: cause= %d\n", SAPP_ID, fname, cause);

    sapp_resetting = 1;

    sapp_sccp_cbs->resetsession_res(sapp_sccp_handle,
                                    GAPI_MSG_RESETSESSION_RES, cause);
    
    return (0);
}

static int sapp_sapp_resetsession_res (void *handle, int msg_id,
                                       gapi_causes_e cause)
{
    char *fname = "sapp_sapp_resetsession_res";

    sapp_debug_entry(fname, 0, 0);

    SAPP_DEBUG("%s: 0-0: %-25s: cause= %d\n", SAPP_ID, fname, cause);

    sapp_resetting = 0;
    
    return (0);
}

static int sapp_sapp_conninfo (void *handle, int msg_id, int conn_id,
                               int line, gapi_conninfo_t *conninfo)
{
    char *fname = "sapp_sapp_conninfo";

    sapp_debug_entry(fname, conn_id, line);

    SAPP_DEBUG("    calling_name: %s, calling_number: %s,\n"
               "    called_name: %s, called_number: %s,\n"
               "    connected_name: %s, connected_number: %s.\n",
               conninfo->calling_name, conninfo->calling_number,
               conninfo->called_name, conninfo->called_number,
               conninfo->connected_name, conninfo->connected_number);

    return (0);
}

static int sapp_sapp_all_streams_idle (void *handle)
{
    char *fname = "sapp_sapp_all_streams_idle";

    sapp_debug_entry(fname, 0, 0);

    return ((sapp_call_count == 0) ? 1 : sapp_call_count);
}

static int sapp_sapp_close_abandonded_streams (void *handle)
{
    char *fname = "sapp_sapp_close_abandonded_streams";

    sapp_debug_entry(fname, 0, 0);

    sapp_call_count = 0;

    return (sapp_call_count);
}

static platform_thread_func_t PLATFORM_CALLBACK sapp_sccp_main2 (void *user_data)
{
    int rc;

    printf("%s: sccp task started\n", SAPP_ID);

    while (1) {
        //platform_wait_event(sapp_sccp_event, SAPP_SCCP_WAIT_TIMEOUT);
        SAPP_DEBUGP("sapp_sccp_main2: 1\n");

#ifdef SCCP_PLATFORM_UNIX
        platform_mutex_lock(sapp_sccp_mutex, SAPP_SCCP_WAIT_TIMEOUT);
        while (sapp_sccp_nready == 0) {
            platform_wait_event(sapp_sccp_event, (unsigned long)(sapp_sccp_mutex));
        }
        sapp_sccp_nready--;
        platform_mutex_unlock(sapp_sccp_mutex);
//        do {
            rc = sapp_sccp_cbs->sccp_main(sapp_sccp_handle, NULL);
        //while (rc != 0);
        SAPP_DEBUGP("sapp_sccp_main2: 2\n");
#endif
#ifdef SCCP_PLATFORM_WINDOWS
        platform_mutex_lock(sapp_sccp_mutex, SAPP_SCCP_WAIT_TIMEOUT);
        SAPP_DEBUGP("sapp_sccp_main2: 1.3: %d\n", sapp_sccp_nready);
        while (sapp_sccp_nready == 0) {
            SAPP_DEBUGP("sapp_sccp_main2: 1.4: %d\n", sapp_sccp_nready);
            platform_mutex_unlock(sapp_sccp_mutex);
            platform_wait_event(sapp_sccp_event, SAPP_SCCP_WAIT_TIMEOUT);
            
            platform_mutex_lock(sapp_sccp_mutex, SAPP_SCCP_WAIT_TIMEOUT);
            SAPP_DEBUGP("sapp_sccp_main2: 1.6: %d\n", sapp_sccp_nready);
        }

        sapp_sccp_nready--;
        SAPP_DEBUGP("sapp_sccp_main2: 1.8: %d\n", sapp_sccp_nready);
        platform_mutex_unlock(sapp_sccp_mutex);

        rc = sapp_sccp_cbs->sccp_main(sapp_sccp_handle, NULL);
        SAPP_DEBUGP("sapp_sccp_main2: 2\n");
#endif
    }

    return (0);
}

#if 0
static void *sapp_queue_create (ssapi_queue_cleanup cleanup)
{
    sapp_sccp_queue = mt_queue_create();
    if (sapp_sccp_queue == NULL) {
        return (NULL);
    }

    mt_queue_init(sapp_sccp_queue, cleanup);

    sapp_sccp_thread = platform_create_thread(sapp_sccp_main2,
                                              sapp_sccp_queue,
                                              &sapp_sccp_thread_id);
    if (sapp_sccp_thread == 0) {
        mt_queue_destroy(sapp_sccp_queue);
        return (NULL);
    }

    return (sapp_sccp_queue);
}

static int sapp_queue_push (void *queue, void *data)
{
    return (mt_queue_push(queue, data));
}
#endif

static void sapp_free_socket2 (sapp_socket_t *socket2)
{
//    platform_mutex_lock(sapp_sockets2.mutex, SAPP_SCCP_WAIT_TIMEOUT);
    
    socket2->id           = SAPP_NO_SOCKET;
    socket2->sapp_id      = SAPP_NO_SAPP_SOCKET;
    socket2->checkconnect = 0;

//    platform_mutex_unlock(sapp_sockets2.mutex);
}

static sapp_socket_t *sapp_get_socket2_by_id (int id)
{
    sapp_socket_t *socket2;
    sapp_socket_t *socket2_found = NULL;
    int i = 0;    

    //platform_mutex_lock(sapp_sockets2.mutex, SAPP_SCCP_WAIT_TIMEOUT);

    for (socket2 = &(sapp_sockets2.sockets[0]);
         socket2 < &(sapp_sockets2.sockets[SAPP_MAX_SOCKETS]);
         socket2++) {
        
        SAPP_DEBUGP("%d: id= %d:%d\n", i++, socket2->id, id);
        
        if (socket2->id == id) {
            socket2_found = socket2;
            break;
        }
    }
    
    //platform_mutex_unlock(sapp_sockets2.mutex);

    return (socket2_found);
}

static unsigned long sapp_socket_getsockname (int socket)
{
    sapp_socket_t *socket2;
    unsigned long sockname;

    platform_mutex_lock(sapp_sockets2.mutex, SAPP_SCCP_WAIT_TIMEOUT);

    socket2 = sapp_get_socket2_by_id(socket);
    if (socket2 == NULL)  {
        platform_mutex_unlock(sapp_sockets2.mutex);

        return (SSAPI_SOCKET_ERROR);
    }

    sockname = platform_getsockname(socket2->sapp_id);

    platform_mutex_unlock(sapp_sockets2.mutex);

    /*
     * Return an IPv4 address in network byte order.
     */
    return (sockname);
}

static char *sapp_socket_getmac (int socket)
{
    return (sapp_mac_addr);
}

static platform_thread_func_t PLATFORM_CALLBACK sapp_tcp_main (void *user_data)
{
    char msg_buffer[SAPP_MAX_SCCP_BUFFER_SIZE];
    int  size = 0;
    int  error;
    fd_set readset;
    fd_set writeset;
    fd_set exceptset;
    //TIMEVAL timeout;
    platform_timeval_t timeout;
    platform_socket_t maxfdp1;
    int decrement;
    sapp_socket_t *sapp_socket2;
    sapp_socket_t *sapp_socket;
    int nready;

    printf("%s: tcp task started\n", SAPP_ID);

    timeout.tv_sec  = 1;
    timeout.tv_usec = 0;

#ifdef SCCP_PLATFORM_UNIX
    platform_mutex_lock(sapp_sockets2.mutex, SAPP_SCCP_WAIT_TIMEOUT);
//  while (sapp_sockets2.nready == 0) {
    platform_wait_event(sapp_sockets2.event, (unsigned long)(sapp_sockets2.mutex));
//    sapp_sockets2.nready--;
    //}
    platform_mutex_unlock(sapp_sockets2.mutex);
#endif

#ifdef SCCP_PLATFORM_WINDOWS
    platform_wait_event(sapp_sockets2.event, SAPP_SCCP_WAIT_TIMEOUT);
#endif
    
    while (1) {
        FD_ZERO(&readset);
        FD_ZERO(&writeset);
        FD_ZERO(&exceptset);
        maxfdp1 = 0;
        
        platform_mutex_lock(sapp_sockets2.mutex, SAPP_SCCP_WAIT_TIMEOUT);

        /*
         * Set the control structure with the sockets that we need to 
         * check.
         */
        for (sapp_socket2 = &(sapp_sockets2.sockets[0]);
             sapp_socket2 < &(sapp_sockets2.sockets[SAPP_MAX_SOCKETS]);
             sapp_socket2++) {

            /*
             * Only check the sockets that have been opened.
             */
            if (sapp_socket2->id != SAPP_NO_SOCKET) {
#if 0
                /* 
                 * Skip sockets that have had an error.
                 */
                if (sapp_socket2->checkconnect == 2) {
                    continue;
                }
#endif
                if (sapp_socket2->sapp_id > maxfdp1) {
                    maxfdp1 = sapp_socket2->sapp_id;
                }
                FD_SET(sapp_socket2->sapp_id, &readset);

                /*
                 * Do we need to see if the socket was just connected?
                 */
                if (sapp_socket2->checkconnect == 1) {
                    FD_SET(sapp_socket2->sapp_id, &writeset);
                    FD_SET(sapp_socket2->sapp_id, &exceptset);
                }
            }
        }

        platform_mutex_unlock(sapp_sockets2.mutex);

        /*
         * Skip the rest if we don't have any sockets.
         */
        if (maxfdp1 == 0) {
            /*
             * No sockets, so let's just wait until the stack opens
             * a socket.
             */
#ifdef SCCP_PLATFORM_UNIX
            platform_mutex_lock(sapp_sockets2.mutex, SAPP_SCCP_WAIT_TIMEOUT);
            platform_wait_event(sapp_sockets2.event, (unsigned long)(sapp_sockets2.mutex));
            platform_mutex_unlock(sapp_sockets2.mutex);
#endif
#ifdef SCCP_PLATFORM_WINDOWS
            platform_wait_event(sapp_sockets2.event, SAPP_SCCP_WAIT_TIMEOUT);
#endif            
            continue;
        }

        maxfdp1++;

        nready = select(maxfdp1, &readset, &writeset, &exceptset, &timeout);
        
        SAPP_DEBUGP("tcp nready 1= %d\n", nready);
        
        if (nready > 0) {
            platform_mutex_lock(sapp_sockets2.mutex, SAPP_SCCP_WAIT_TIMEOUT);
            
            for (sapp_socket = &(sapp_sockets2.sockets[0]);
                 sapp_socket < &(sapp_sockets2.sockets[SAPP_MAX_SOCKETS]);
                 sapp_socket++) {
                
                SAPP_DEBUGP("tcp nready 1.5= %d, %d\n", nready, sapp_socket->id);
                
                decrement = 0;

                /*
                 * Skip sockets that have not been opened.
                 */
                if (sapp_socket->id == SAPP_NO_SOCKET) {
                    continue;
                }

                /*
                 * Check for a successful connection.
                 */
                if ((sapp_socket->checkconnect == 1) &&
                    (FD_ISSET(sapp_socket->sapp_id, &writeset))) {
                    sapp_socket->checkconnect = 0;

                    SAPP_DEBUG("%s: socket= %d: write STATUS: sucessful connection to CM\n\n",
                               SAPP_ID, sapp_socket->id);
                    
                    sapp_sccp_cbs->tcp_main(sapp_sccp_handle, GAPI_MSG_TCP_EVENTS,
                                            sapp_socket->id,
                                            GAPI_TCP_EVENT_OPEN,
                                            NULL, 0);

                    decrement = 1;
                }

                /*
                 * Check for a failed connection.
                 */
                if ((sapp_socket->checkconnect = 0) &&
                    (FD_ISSET(sapp_socket->sapp_id, &exceptset))) {
                    sapp_socket->checkconnect = 0;
                    
                    SAPP_DEBUG("%s: socket= %d: except STATUS: failed connection to CM\n\n",
                               SAPP_ID, sapp_socket->id);

                    sapp_sccp_cbs->tcp_main(sapp_sccp_handle,
                                            GAPI_MSG_TCP_EVENTS,
                                            sapp_socket->id,
                                            GAPI_TCP_EVENT_CLOSE,
                                            NULL, 0);
                    decrement = 1;
                }

                /*
                 * Check for any data that was received.
                 */
                if (FD_ISSET(sapp_socket->sapp_id, &readset)) {
                    size = recv(sapp_socket->sapp_id, msg_buffer,
                                SAPP_MAX_SCCP_BUFFER_SIZE, 0);

                    //TODO:
                    // This receive procedure does not guarantee that a message is not spanned
                    // across more than 1 TCP messages.  There are 2 cases for this.
                    // 1) The message we received is incomplete.  We would need to keep the
                    // data around until we read the next TCP message to reconstruct the
                    // correct SCCP message
                    // 2) We received a partial message at the end of some valid messages.
                    // Again, we need to keep this data around until next time.
                    if (size == PLATFORM_SOCKET_ERROR) {
                        int rc = 0;

                        error = platform_get_last_error();
                        switch(error) {
                        default:
                            SAPP_DEBUG("\n%s: socket= %d: recv ERROR= %d:%s\n",
                                       SAPP_ID, sapp_socket->id, error,
                                       platform_get_last_error_string(error));
                            rc = 1;
                            break;
                        }

                        if (rc > 0) {
                            /*
                             * Connection has been closed - let the stack know.
                             */
                            SAPP_DEBUG("\n%s: socket= %d: STATUS: Connection to CM disconnected\n\n",
                                       SAPP_ID, sapp_socket->id);

                            sapp_sccp_cbs->tcp_main(sapp_sccp_handle,
                                                    GAPI_MSG_TCP_EVENTS,
                                                    sapp_socket->id,
                                                    GAPI_TCP_EVENT_CLOSE,
                                                    NULL, 0);

#if 0
                            /*
                             * Remove the socket from the list so that we do
                             * not keep grabbing errors from it. The stack will
                             * (hopefully) close the socket which will remove
                             * it from the sapp_sockets list.
                             */
                            //FD_CLR(sapp_socket->sapp_id, &readset);
                            sapp_socket2->checkconnect = 2;
#endif
                        }
                    }
                    else if (size == 0) {
                        /*
                         * Connection has been closed - let the stack know.
                         */
                        SAPP_DEBUG("\n%s: socket= %d: STATUS: Connection to CM disconnected\n\n",
                                   SAPP_ID, sapp_socket->id);

                        sapp_sccp_cbs->tcp_main(sapp_sccp_handle,
                                                GAPI_MSG_TCP_EVENTS,
                                                sapp_socket->id,
                                                GAPI_TCP_EVENT_CLOSE,
                                                NULL, 0);
#if 0
                        sapp_socket2->checkconnect = 2;
#endif
                    }
                    else {
                        /*
                         * Got some good data - give it to the stack.
                         */
                        sapp_sccp_cbs->tcp_main(sapp_sccp_handle, 0,
                                                sapp_socket->id,
                                                GAPI_TCP_EVENT_RECV,
                                                msg_buffer, (int)size);
                    }

                    decrement = 1;
                } /* if (FD_ISSET(sapp_socket->sapp_id, &readset)) */
                SAPP_DEBUGP("tcp nready 2= %d\n", nready);
                /*
                 * Check if all sockets have been processed.
                 */
                if ((nready -= decrement) <= 0) {
                    SAPP_DEBUGP("tcp nready 3= %d\n", nready);
                    break;
                }
                SAPP_DEBUGP("tcp nready 4= %d\n", nready);
            } /* for (i = 0; i < SAPP_MAX_SOCKETS; i++) */
            
            platform_mutex_unlock(sapp_sockets2.mutex);
        } /* if (error > 0)  */
    } /* while (1) */

    return (0);
}

static int sapp_get_new_socket_id ()
{
    static int id = 0;
    
    if (++id < 0)  {
        id = 1;
    }
    
    return (id);    
}

static int sapp_socket_open (int *socket)
{
    char              *fname    = "sapp_socket_open";
    sapp_socket_t     *socket2;
    int               rc;
    platform_socket_t id;

    platform_mutex_lock(sapp_sockets2.mutex, SAPP_SCCP_WAIT_TIMEOUT);

    /*
     * Grab a free socket2.
     */
    socket2 = sapp_get_socket2_by_id(-1);
    if (socket2 == NULL)  {
        platform_mutex_unlock(sapp_sockets2.mutex);

        return (SSAPI_SOCKET_ERROR);
    }
    
    /*
     * Grab a socket from the OS.
     */
    rc = platform_socket(&id);
    if (rc != 0) {
        platform_mutex_unlock(sapp_sockets2.mutex);
        return (rc);
    }
    socket2->id      = (int)id; //sapp_get_new_socket_id();
    socket2->sapp_id = id;
    *socket          = socket2->id;

    /*
     * Make this a non-blocking socket.
     */
    rc = platform_ioctl(id);
    if (rc != 0) {
        platform_close(id);
        sapp_free_socket2(socket2);
        
        platform_mutex_unlock(sapp_sockets2.mutex);

        return (rc);
    }

    platform_mutex_unlock(sapp_sockets2.mutex);
    
    SAPP_DEBUG("%s: %-25s: socket= %d:%d\n",
               SAPP_ID, fname, *socket, id);

    return (0);
}

static int sapp_socket_close (int socket)
{
    char          *fname    = "sapp_socket_close";
    sapp_socket_t *socket2;    
    int           error;
    int           size;
    char          msg_buffer[SAPP_MAX_SCCP_BUFFER_SIZE];

    platform_mutex_lock(sapp_sockets2.mutex, SAPP_SCCP_WAIT_TIMEOUT);

    socket2 = sapp_get_socket2_by_id(socket);
    if (socket2 == NULL)  {
        platform_mutex_unlock(sapp_sockets2.mutex);

        return (SSAPI_SOCKET_ERROR);
    }

    /*
     * Finish sending data ???.
     */

    /*
     * Gracefully shutdown the socket. Disable sending.
     */
    platform_shutdown(socket2->sapp_id);

    /*
     * Empty the socket.
     */
    while (1) {
        size = recv(socket2->sapp_id, msg_buffer,
                    SAPP_MAX_SCCP_BUFFER_SIZE, 0);
        if ((size == PLATFORM_SOCKET_ERROR) || (size == 0)) {
            break;
        }
        else {
            SAPP_DEBUG("%s: %-25s: received %d bytes after shutdown.\n",
                       SAPP_ID, fname, size);
        }
    }

    /*
     * Ok, go ahead and close it.
     */
    error = platform_close(socket2->sapp_id);

    sapp_free_socket2(socket2);

    platform_mutex_unlock(sapp_sockets2.mutex);

    SAPP_DEBUG("%s: %-25s: socket= %d->%d\n",
               SAPP_ID, fname, socket, error);

    return (error);
}

static int sapp_socket_connect (int socket, unsigned long addr,
                                unsigned short port)
{
    char               *fname   = "sapp_socket_connect";
    int                rc;
    sapp_socket_t      *socket2;

    SAPP_DEBUGP("%s: 1, socket= %d\n", fname, socket);
    platform_mutex_lock(sapp_sockets2.mutex, SAPP_SCCP_WAIT_TIMEOUT);
    
    socket2 = sapp_get_socket2_by_id(socket);
    if (socket2 == NULL)  {
        platform_mutex_unlock(sapp_sockets2.mutex);

        return (SSAPI_SOCKET_ERROR);
    }

    //addr = htonl(addr);
    //cmtocs(&port);
    SAPP_DEBUGP("%s: addr/port= %08x:%04x\n", fname, addr, port);
    rc = platform_connect(socket2->sapp_id, addr, port);
    if (rc  != 0) {
        switch (rc) {
        case (PLATFORM_EWOULDBLOCK):
        case (PLATFORM_EINPROGRESS):
            /*
             * Mark this socket so that we know to check it for 
             * connection completion.
             */
            socket2->checkconnect = 1;

            /*
             * Set the return code to success, because we will be hiding 
             * the setting up of the sockets from the user.
             */
            rc = 0;
            break;
 
        default:
            platform_mutex_unlock(sapp_sockets2.mutex);

            return (rc);//(SSAPI_SOCKET_ERROR);
        }
    }
    
    platform_mutex_unlock(sapp_sockets2.mutex);

    sapp_tcp_thread_run(sapp_sockets2.event);

    /*
     * Notify the user when the connection is complete.
     */

    return (rc);
}

static int sapp_socket_send (int socket, char *buf, int len)
{
    char *fname   = "sapp_socket_send";
    int  error;
    sapp_socket_t *socket2 = NULL;

    platform_mutex_lock(sapp_sockets2.mutex, SAPP_SCCP_WAIT_TIMEOUT);
    
    socket2 = sapp_get_socket2_by_id(socket);
    if (socket2 == NULL)  {
        platform_mutex_unlock(sapp_sockets2.mutex);

        return (SSAPI_SOCKET_ERROR);
    }

    error = send(socket2->sapp_id, buf, len, 0);

    if (error == PLATFORM_SOCKET_ERROR) {
        error = platform_get_last_error();
        SAPP_DEBUG("%s: %-25s: socket= %d: ERROR: %d:%s\n",
                   SAPP_ID, fname, socket2->id,
                   error, platform_get_last_error_string(error));
    }
    else if (error == 0) { /* socket closed. */
        sapp_sccp_cbs->tcp_main(sapp_sccp_handle, GAPI_MSG_TCP_EVENTS,
                                socket,
                                GAPI_TCP_EVENT_CLOSE,
                                NULL, 0);
    }
    /*sam
     * Should we check that we sent all the bytes?
     */
    
    platform_mutex_unlock(sapp_sockets2.mutex);
    
    return (error);
}

static int sapp_socket_recv (int socket, char *buf, int len)
{
    return (0);
}

#if 0
void *sapp_timer_allocate (void)
{
    return (timer_event_allocate());
}

typedef void (*sapp_timer_callback)(void *timer_event, void *param1,
                                    void *param2);
static void sapp_timer_initialize (void *timer, int period,
                            sapp_timer_callback expiration,
                            void *param1, void *param2)
{
    timer_event_initialize(timer,
                           period, expiration,
                           param1, param2);
}

static void sapp_timer_activate (void *timer)
{
    SAPP_DEBUG("%s: %-25s: 0x%p.\n", SAPP_ID,
               "sapp_timer_activate", timer);

    timer_event_activate(timer);
}

static void sapp_timer_cancel (void *timer)
{
    SAPP_DEBUG("%s: %-25s: 0x%p.\n", SAPP_ID,
               "sapp_timer_cancel", timer);

    timer_event_cancel(timer);
}

static void sapp_timer_free (void *timer)
{
    timer_event_free(timer);
}

static char *sapp_strtime (char *buf)
{
    return (platform_strtime(buf));
}

static void *sapp_init_critical_section (void)
{
    return (platform_init_critical_section());
}

static void sapp_enter_critical_section (void *critical_section)
{
    platform_enter_critical_section(critical_section);
}

static void sapp_leave_critical_section (void *critical_section)
{
    platform_leave_critical_section(critical_section);
}
#endif

static unsigned short sapp_ntohs (unsigned short data)
{
    return (ntohs(data));
}

static unsigned long sapp_ntohl (unsigned long data)
{
    return (ntohl(data));
}

static void *sapp_sccp_thread_get (void)
{
    return (sapp_sccp_event);
}

static void sapp_sccp_thread_run (void *event)
{
    SAPP_DEBUGP("sapp_sccp_thread_run: 1\n");

#ifdef SCCP_PLATFORM_UNIX
    platform_mutex_lock(sapp_sccp_mutex, SAPP_SCCP_WAIT_TIMEOUT);
        if (sapp_sccp_nready == 0) {
        platform_set_event(event);
    }
    sapp_sccp_nready++;
    platform_mutex_unlock(sapp_sccp_mutex);
#endif
#ifdef SCCP_PLATFORM_WINDOWS
    /*
     * Windows performs a thread-switch as soon as the event is set,
     * so we need to reorder things as opposed to Unix.
     */
    platform_mutex_lock(sapp_sccp_mutex, SAPP_SCCP_WAIT_TIMEOUT);
    SAPP_DEBUGP("sapp_sccp_thread_run: 1.3: %d\n", sapp_sccp_nready);
    sapp_sccp_nready++;
    platform_mutex_unlock(sapp_sccp_mutex);
    SAPP_DEBUGP("sapp_sccp_thread_run: 1.5: %d\n", sapp_sccp_nready);
    platform_set_event(event);
#endif

    SAPP_DEBUGP("sapp_sccp_thread_run: 2\n");
}

static void sapp_tcp_thread_run (void *event)
{
    SAPP_DEBUGP("sapp_tcp_thread_run: 1\n");
#ifdef SCCP_PLATFORM_UNIX
    platform_mutex_lock(sapp_sockets2.mutex, SAPP_SCCP_WAIT_TIMEOUT);
//    if (sapp_sockets2.nready == 0) {
        platform_set_event(event);
//    }
//    sapp_sockets2.nready++;
    platform_mutex_unlock(sapp_sockets2.mutex);
#endif
#ifdef SCCP_PLATFORM_WINDOWS
    platform_set_event(event);
#endif
    SAPP_DEBUGP("sapp_tcp_thread_run: 2\n");
}

#if 0
static int sapp_cmd (int argc, const char *argv[])
{
    if ((argc == 1) ||
        ((argc == 2) && (argv[1][0] == '0')))  {
        if (sapp_debug == 0)  {
            sapp_debug = 1;
        }
        else  {
            sapp_debug = 0;
        }
    
        am_debug      = sapp_debug;
        sccp_debug    = sapp_debug;            
        sccpmsg_debug = sapp_debug;    
        sccpcc_debug  = sapp_debug;
        sccpcm_debug  = sapp_debug;
        sccprec_debug = sapp_debug;
        sccpreg_debug = sapp_debug;            
    }
    else if ((argc == 2) && (argv[1][0] == '1'))  {
        sapp_test();
    }
    
    return (0);
}
#endif

void sapp_showsession (void)
{
    int i, j;

    printf("\n\n---- SAPP SESSION ---\n");

    printf("sccp_conn_id/line= %d/%d, sapp_call_id/line = %d/%d\n",
           sapp_sccp_conn_id, sapp_sccp_line,
           sapp_sapp_call_id, sapp_sapp_line);

    printf("\nlinecnt= %d\n",
           sapp_session_data.linecnt);

    for (i = 0; i < sapp_session_data.linecnt; i++) {
        printf("[%d] %s : %s : %s\n",
               sapp_session_data.lines[i].instance,
               sapp_session_data.lines[i].dn,
               sapp_session_data.lines[i].fqdn,
               sapp_session_data.lines[i].label);
    }

    printf("\nspeeddialcnt= %d\n",
           sapp_session_data.speeddialcnt);

    for (i = 0; i < sapp_session_data.speeddialcnt; i++) {
        printf("[%d] %s : %s\n",
               sapp_session_data.speeddials[i].instance,
               sapp_session_data.speeddials[i].dn,
               sapp_session_data.speeddials[i].fqdn);
    }

    printf("\nsoftkeycnt= %d",
           sapp_session_data.softkeycnt);

    for (i = 0; i < sapp_session_data.softkeycnt; i++) {
        if ((i % 4) == 0) {
            printf("\n");
        }

        printf("%04x:%-12s  ",
               sapp_session_data.softkeys[i].event,
               sapp_session_data.softkeys[i].label);
    }

    printf("\n\nsoftkeysetcnt= %d",
           sapp_session_data.softkeysetcnt);

    for (i = 0; i < sapp_session_data.softkeysetcnt; i++) {
        printf("\n[%02d] ", i);

        for (j = 0; j < GAPI_MAX_SOFTKEY_INDEXES; j++) {
            if ((j > 0) && ((j % 8) == 0)) {
                printf("\n     ");
            }

            printf("%02x:%04x  ",
                   sapp_session_data.softkeysets[i].template_index[j],
                   sapp_session_data.softkeysets[i].info_index[j]);
        }
    }

    printf("\n");
}

void *sapp_get_gapihandle(void)
{
    return (sapp_sccp_handle);
}

int sapp_get_conn_id (void)
{
    return (sapp_sccp_conn_id);
}

int sapp_get_num_lines (void)
{
    return (sapp_session_data.linecnt);
}

char *sapp_get_line_name (int line)
{
    return (sapp_session_data.lines[line- 1].dn);
}

int sapp_get_line (void)
{
    return (sapp_sccp_line);
}

int sapp_get_active_conn_id (void)
{
    return (0);
}

int sapp_get_active_line (void)
{
    return (sapp_sapp_line);
}

void sapp_set_active_line (int line)
{
    sapp_sapp_line = line;
}
#if 0
int sapp_test (void)
{
    gapi_cmaddr_t cms[5];
    
    memset(cms, 0 , sizeof(*cms) * 5);

    /*
     * Set the addresses in host order. The stack will change it to network
     * order on the way out.
     */
    cms[0].addr = 0x40665774; /* shague-cm10 - 64.102.878.116 */
    cms[0].port = 0x07d0;     /* 2000 */
    
    sapp_opensession_req(cms, NULL, NULL, GAPI_SRST_MODE_DISABLE, NULL, NULL); 
    
    return (1);   
}
#endif
static int sapp_mutex_create (int owner, void **mutex)
{
    return (platform_create_mutex(owner, mutex));
}

static int sapp_mutex_wait (void *mutex, unsigned long timeout)
{
    return (platform_mutex_lock(mutex, PLATFORM_WAIT_INFINITE));
}

static int sapp_mutex_release (void *mutex)
{
    return (platform_mutex_unlock(mutex));
}

static int sapp_mutex_destroy (void *mutex)
{
    return (platform_destroy_mutex(mutex));
}

int sapp_init (void)
{
    char *fname = "sapp_init";
    int gapi_rc = 0;
    gapi_callbacks_t  *sccp_cbs = NULL;
    gapi_callbacks_t  sapp_cbs;
    ssapi_callbacks_t ssapi_cbs;
    sapp_socket_t     *socket2;
    sapp_call_t       *call;
    int rc;

    printf("%s: initializing\n", SAPP_ID);
    
    printf("%s: initializing: sockets\n", SAPP_ID);
    rc = platform_create_mutex(0, &(sapp_sockets2.mutex));
    if (rc != 0) {
        return (1);
    }

    rc = platform_create_event(0, 0, &(sapp_sockets2.event));
    if (rc != 0) {
        return (1);
    }

    for (socket2 = &(sapp_sockets2.sockets[0]);
         socket2 < &(sapp_sockets2.sockets[SAPP_MAX_SOCKETS]);
         socket2++) {
        sapp_free_socket2(socket2);
    }

    rc = platform_create_thread(sapp_tcp_main,
                                NULL,
                                &(sapp_sockets2.thread));
    if (rc != 0) {
        return (1);
    }

    printf("%s: initializing: sccp\n", SAPP_ID);
    rc = platform_create_mutex(0, &sapp_sccp_mutex);
    if (rc != 0) {
        return (1);
    }

    rc = platform_create_event(0, 0, &sapp_sccp_event);
    if (rc != 0) {
        return (1);
    }

    rc = platform_create_thread(sapp_sccp_main2,
                                NULL,
                                &sapp_sccp_thread);
    if (rc != 0) {
        return (1);
    }

    printf("%s: initializing: calls\n", SAPP_ID);
    rc = platform_create_mutex(0, &(sapp_calls.mutex));
    if (rc != 0) {
        return (1);
    }

    sapp_calls.count = 0;
    for (call = &(sapp_calls.calls[0]);
         call < &(sapp_calls.calls[SAPP_MAX_CALLS]);
         call++) {
        sapp_free_call(call);
    }

    //    sapp_mac_addr = local_mac;
    sapp_sccp_cbs = (gapi_callbacks_t *)malloc(sizeof(gapi_callbacks_t));
    if (sapp_sccp_cbs == NULL) {
         return (1);
    }

    memset(sapp_sccp_cbs, 0, sizeof(gapi_callbacks_t));

    /*
     * Setup the sccp application callbacks.
     */
    sapp_cbs.setup            = sapp_sapp_setup;
    sapp_cbs.setup_ack        = sapp_sapp_setup_ack;
    sapp_cbs.proceeding       = sapp_sapp_proceeding;
    sapp_cbs.alerting         = sapp_sapp_alerting;
    sapp_cbs.connect          = sapp_sapp_connect;
    sapp_cbs.connect_ack      = sapp_sapp_connect_ack;
    sapp_cbs.disconnect       = sapp_sapp_disconnect;
    sapp_cbs.release          = sapp_sapp_release;
    sapp_cbs.release_complete = sapp_sapp_release_complete;

    sapp_cbs.openrcv_req      = sapp_sapp_openrcv_req;
    sapp_cbs.closercv         = sapp_sapp_closercv;
    sapp_cbs.startxmit        = sapp_sapp_startxmit;
    sapp_cbs.stopxmit         = sapp_sapp_stopxmit;
    
    sapp_cbs.feature_res      = sapp_sapp_feature_res;    
    
    sapp_cbs.opensession_res  = sapp_sapp_opensession_res;
    sapp_cbs.closesession_res = sapp_sapp_closesession_res;
    sapp_cbs.sessionstatus    = sapp_sapp_sessionstatus;    
    sapp_cbs.resetsession_req = sapp_sapp_resetsession_req;    
    sapp_cbs.resetsession_res = sapp_sapp_resetsession_res;    
    
    sapp_cbs.conninfo         = sapp_sapp_conninfo;
        
    sapp_cbs.all_streams_idle         = sapp_sapp_all_streams_idle;
    sapp_cbs.close_abandonded_streams = sapp_sapp_close_abandonded_streams;

    
    /*
     * Setup the system services callbacks.
     */
    ssapi_cbs.timer_allocate   = timer_event_allocate;
    ssapi_cbs.timer_initialize = timer_event_initialize;
    ssapi_cbs.timer_activate   = timer_event_activate;
    ssapi_cbs.timer_cancel     = timer_event_cancel;
    ssapi_cbs.timer_free       = timer_event_free;

    ssapi_cbs.malloc = malloc;
    ssapi_cbs.free   = free;
    ssapi_cbs.memset = memset;
    ssapi_cbs.memcpy = memcpy;

    ssapi_cbs.htons = sapp_ntohs;
    ssapi_cbs.htonl = sapp_ntohl;
    ssapi_cbs.ntohs = sapp_ntohs;
    ssapi_cbs.ntohl = sapp_ntohl;

    ssapi_cbs.strtime = platform_strtime;

    ssapi_cbs.mutex_create = sapp_mutex_create;
    ssapi_cbs.mutex_lock   = sapp_mutex_wait;
    ssapi_cbs.mutex_unlock = sapp_mutex_release;
    ssapi_cbs.mutex_delete = sapp_mutex_destroy;

    ssapi_cbs.thread_get = sapp_sccp_thread_get;
    ssapi_cbs.thread_run = sapp_sccp_thread_run;
    
    ssapi_cbs.socket_open         = sapp_socket_open;
    ssapi_cbs.socket_close        = sapp_socket_close;
    ssapi_cbs.socket_connect      = sapp_socket_connect;
    ssapi_cbs.socket_send         = sapp_socket_send;
    ssapi_cbs.socket_recv         = sapp_socket_recv;
    ssapi_cbs.socket_getlasterror = platform_get_last_error;
    ssapi_cbs.socket_getsockname  = sapp_socket_getsockname;
    ssapi_cbs.socket_getmac       = sapp_socket_getmac; 

    gapi_rc = ssapi_bind(&ssapi_cbs);
    if (gapi_rc != 0) {
        return (12);
    }

    gapi_rc = gapi_bind(&sapp_cbs, &sccp_cbs, sapp_sapp_handle,
                        &sapp_sccp_handle, SAPP_NAME);
    if (gapi_rc != 0) {
        return (21);
    }

    if (sapp_sccp_handle == NULL) {
        return (22);
    }

    memcpy(sapp_sccp_cbs, sccp_cbs, sizeof(gapi_callbacks_t));

    memset(&sapp_session_data, 0, sizeof(sapp_session_data));

    /*
     * Other global connection related info was initialized at compile-time.
     */

    printf("%s: initialized\n", SAPP_ID);

    return (0);
}

int sapp_cleanup (void)
{
    platform_destroy_mutex(sapp_sockets2.mutex);   
    platform_destroy_mutex(sapp_sccp_mutex);

    platform_destroy_event(sapp_sccp_event);   
    platform_destroy_event(sapp_sockets2.event);   

    /*sam
     * Kill threads?
     */
    free(sapp_sccp_cbs);

    return (0);
}
