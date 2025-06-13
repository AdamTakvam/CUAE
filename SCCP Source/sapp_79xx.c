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
 *     sapp_79xx.c
 *
 *  CREATION/MODIFICATION DATES
 *     Created:  November 2002, Sam Hague
 *     Modified: 
 *
 *  DESCRIPTION
 *     SCCP Application implementation (79xx platform)
 */
#include <stdio.h>
#include <memory.h>
#include "sapp.h"
#include "gapi.h"
#include "timer.h"
#include "ssapi.h"
#include "sccp.h"
#include "sccpmsg.h"
#include "sccp_debug.h"
#include "task.h"
#include "network.h"
#include "cfgfile_utils.h"
#include "prot_configmgr.h"
#include "emuiface.h"
#include "ccapi.h"
#include "phone.h"


#define SAPP_SCCP_WAIT_TIMEOUT 10 //2000 /* ms */
#define SAPP_MAX_SOCKETS  5
#define SAPP_NO_SOCKET    -1
#define SAPP_NO_CONN_ID   -1
#define SAPP_DEFAULT_LINE 1


typedef struct sapp_socket_t_  {
    int id;
    Socket socket;
    unsigned char sapp_id;
} sapp_socket_t;

static sapp_socket_t sapp_sockets2[SAPP_MAX_SOCKETS];


static int sapp_sccp_conn_id = SAPP_NO_CONN_ID;
static int sapp_sapp_call_id = CC_NO_CALL_ID;

static int sapp_sccp_line = SAPP_DEFAULT_LINE;
static int sapp_sapp_line = SAPP_DEFAULT_LINE;

static gapi_conninfo_t sapp_sccp_conninfo;
static cc_caller_id_t  sapp_sapp_conninfo2;

static gapi_media_t sapp_sccp_media;
static cc_sdp_t     sapp_sapp_sdp;

static int sapp_call_direction = 0;
static unsigned long  sapp_rtp_addr = 0;
static unsigned short sapp_rtp_port = 0;

static int sapp_gapi_waiting = 0;
static unsigned long sapp_queue;

gapi_callbacks_t *sapp_sccp_cbs;
void *sapp_sccp_handle;
void *sapp_sapp_handle;
void *sapp_sccp_queue = &SIP_List;
unsigned long sapp_tcp_thread;
unsigned long sapp_tcp_thread_id;
void *sapp_tcp_thread_run;
void *sapp_tcp_thread_reading;
void *sapp_tcp_thread_shutdown;
static unsigned long sapp_socket_getsockname(int socket);
static sapp_socket_t *sapp_get_socket2_by_id(int id);

static const char *SAPP_NAME = "SAPP";

static int sapp_debug = 1;
#define SAPP_DEBUG if (sapp_debug) buginf

static void sapp_debug_entry (char *fname, int conn_id, int line)
{
    SAPP_DEBUG("%s: %d-%d: %s\n", SAPP_ID, conn_id, line, fname);
}

void sapp_copy_conninfo_to_caller_id (gapi_conninfo_t *conninfo,
                                      cc_caller_id_t *caller_id,
                                      int direction)
{
    caller_id->called_name = strlib_update(caller_id->called_name,
                                           conninfo->called_name);

    caller_id->called_number = strlib_update(caller_id->called_number,
                                             conninfo->called_number);                      

    caller_id->calling_name = strlib_update(caller_id->calling_name,
                                            conninfo->calling_name);   

    caller_id->calling_number = strlib_update(caller_id->calling_number,
                                              conninfo->calling_number);   
}

void sapp_copy_caller_id_to_conninfo (gapi_conninfo_t *conninfo,
                                      cc_caller_id_t *caller_id)
{
    conninfo->calling_name   = (char *)(caller_id->calling_name);
    conninfo->calling_number = (char *)(caller_id->calling_number);
    conninfo->called_name    = (char *)(caller_id->called_name);
    conninfo->called_number  = (char *)(caller_id->called_number);
}

void sapp_copy_media_to_sdp (gapi_media_t *media, cc_sdp_t *sdp)
{
    int i;
    
    memset(sdp, 0, sizeof(*sdp));

    if (media != NULL)  {
        sdp->remote_addr.addr = media->sccp_media.addr;
        sdp->remote_addr.port = media->sccp_media.port;        
    }    
    
    sdp->remote_avt_payload_type = RTP_NONE;
    for (i = 0; i < CC_MAX_MEDIA_TYPES; i++)  {
        sdp->remote_media_types[i] = RTP_NONE;
    }
    sdp->remote_media_types[0] = RTP_PCMU; 
}

void sapp_copy_sdp_to_media (gapi_media_t *media, cc_sdp_t *sdp)
{
    media->sccp_media.addr = sdp->local_addr.addr;
    media->sccp_media.port = sdp->local_addr.port;
}

int sapp_setup (int conn_id, int line,
                gapi_conninfo_t *conninfo, char *digit, int numdigits,
                gapi_media_t *media, int alert_info, int privacy)
{
    char *fname = "sapp_setup";

    sapp_debug_entry(fname, conn_id, line);

    sapp_sccp_cbs->setup(sapp_sccp_handle, GAPI_MSG_SETUP, conn_id, line, NULL,
                         digit, numdigits, NULL, 0, 0);

    return (0);
}

int sapp_release (int conn_id, int line, gapi_causes_t cause)
{
    char *fname = "sapp_release";

    sapp_debug_entry(fname, conn_id, line);

    sapp_sccp_cbs->release(sapp_sccp_handle, GAPI_MSG_RELEASE, conn_id,
                           line, cause);

    return (0);
}

int sapp_opensession_req (gapi_cmaddr_t *cms, char *mac,
                          gapi_cmaddr_t *srsts, gapi_srst_modes_t srst_mode,
                          gapi_cmaddr_t *tftp)
{
    char *fname = "sapp_opensession_req";

    sapp_debug_entry(fname, 0, 0);

    sapp_sccp_cbs->opensession_req(sapp_sccp_handle,
                                   GAPI_MSG_OPENSESSION_REQ,
                                   cms, mac, srsts, srst_mode, tftp);

    return (0);
}

int sapp_closesession_req (void)
{
    char *fname = "sapp_closesession_req";

    sapp_debug_entry(fname, 0, 0);

    sapp_sccp_cbs->closesession_req(sapp_sccp_handle, GAPI_MSG_CLOSESESSION_REQ);

    return (0);
}

static int sapp_sapp_sessionstatus (void *handle, int msg_id,
                                    gapi_status_t status)
{
    char *fname = "sapp_sapp_sessionstatus";

    sapp_debug_entry(fname, 0, 0);

    SAPP_DEBUG("%s: 0-0: %s: status= %d\n", SAPP_ID, fname, status);
    
    return (0);
}

static int sapp_sapp_resetsession (void *handle, int msg_id,
                                   gapi_causes_t cause)
{
    char *fname = "sapp_sapp_resetsession";

    sapp_debug_entry(fname, 0, 0);

    SAPP_DEBUG("%s: 0-0: %s: cause= %d\n", SAPP_ID, fname, cause);
    
    return (0);
}


static int sapp_sapp_setup (void *handle, int msg_id, int conn_id, int line,
                         gapi_conninfo_t *conninfo, char *digit, int numdigits,
                         gapi_media_t *media, int alert_info, int privacy)
{
    char *fname = "sapp_sapp_setup";

    sapp_debug_entry(fname, conn_id, line);

    sapp_call_direction = 1;
    
    sapp_sccp_conn_id = conn_id;
    sapp_sapp_call_id = cc_get_new_call_id();

    /*
     * Stick a fake port into the remote sdp. SCCP sends the
     * open_rcv before the start_xmit so we never know what the
     * remote port is before we send up a connect. GSM will kill
     * the call if we don't give it some port during the connect.
     */
    sapp_copy_media_to_sdp(NULL, &sapp_sapp_sdp);    
    sapp_sapp_sdp.remote_addr.addr = 0x0a50210d; /* 10.80.33.13 */
    sapp_sapp_sdp.remote_addr.port = 0x4000;     /* 16384 */        

    sapp_gapi_waiting = 1;

    return (0);
}

static int sapp_sapp_setup_ack (void *handle, int msg_id, int conn_id, int line,
                             gapi_conninfo_t *conninfo, gapi_media_t *media,
                             gapi_causes_t cause)
{
    char *fname = "sapp_sapp_setup_ack";

    sapp_debug_entry(fname, conn_id, line);

    sapp_sccp_conn_id = conn_id;
    
    if (cause == GAPI_CAUSE_OK) {

        SAPP_DEBUG("%s: %d-%d: %s: sapp_sccp_conn_id= %d\n",
                   SAPP_ID, conn_id, line, fname, sapp_sccp_conn_id);

        cc_setup_ack(CC_SRC_SIP, sapp_sapp_call_id, sapp_sapp_line,
                     &sapp_sapp_conninfo2, &sapp_sapp_sdp);
    }
    else  {
        cc_release(CC_SRC_SIP, sapp_sapp_call_id, sapp_sapp_line,
                   CC_CAUSE_ERROR, NULL);
    }

    return (0);
}

static int sapp_sapp_proceeding (void *handle, int msg_id, int conn_id, int line,
                              gapi_conninfo_t *conninfo)
{
    char *fname = "sapp_sapp_proceeding";

    sapp_debug_entry(fname, conn_id, line);
    
    return (0);
}

static int sapp_sapp_alerting (void *handle, int msg_id, int conn_id, int line,
                            gapi_conninfo_t *conninfo, gapi_media_t *media,
                            int inband)
{
    char *fname = "sapp_sapp_alerting";

    sapp_debug_entry(fname, conn_id, line);

    cc_alerting(CC_SRC_SIP, sapp_sapp_call_id, sapp_sapp_line,
                &sapp_sapp_conninfo2, NULL, 0);    
                       
    return (0);
}

static int sapp_sapp_connect (void *handle, int msg_id, int conn_id, int line,
                           gapi_conninfo_t *conninfo, gapi_media_t *media)
{             
    char *fname = "sapp_sapp_connect";

    sapp_debug_entry(fname, conn_id, line);

    cc_connected(CC_SRC_SIP, sapp_sapp_call_id, sapp_sapp_line,
                 &sapp_sapp_conninfo2, &sapp_sapp_sdp);        

    return (0);
}

static int sapp_sapp_connect_ack (void *handle, int msg_id, int conn_id, int line,
                               gapi_conninfo_t *conninfo, gapi_media_t *media)
{
    char *fname = "sapp_sapp_connect_ack";

    sapp_debug_entry(fname, conn_id, line);

    cc_connected_ack(CC_SRC_SIP, sapp_sapp_call_id, sapp_sapp_line,
                     &sapp_sapp_conninfo2,
                     &sapp_sapp_sdp);    

    return (0);
}

static int sapp_sapp_disconnect (void *handle, int msg_id, int conn_id, int line,
                                 gapi_causes_t cause)
{
    char *fname = "sapp_sapp_disconnect";

    sapp_debug_entry(fname, conn_id, line);
    
    return (0);
}

static int sapp_sapp_release (void *handle, int msg_id, int conn_id, int line,
                           gapi_causes_t cause)
{
    char *fname = "sapp_sapp_release";

    sapp_debug_entry(fname, conn_id, line);

    cc_release(CC_SRC_SIP, sapp_sapp_call_id, sapp_sapp_line,
               CC_CAUSE_NORMAL, NULL);
                  
    return (0);
}

static int sapp_sapp_release_complete (void *handle, int msg_id, int conn_id,
                                    int line, gapi_causes_t cause)
{
    char *fname = "sapp_sapp_release_complete";

    sapp_debug_entry(fname, conn_id, line);

    cc_release_complete(CC_SRC_SIP, sapp_sapp_call_id, sapp_sapp_line,
                        CC_CAUSE_NORMAL);    

    return (0);
}

static int sapp_sapp_openrcv_req (void *handle, int msg_id, int conn_id,
                               int line, gapi_media_t *media)
{
    char *fname = "sapp_sapp_openrcv_req";

    sapp_debug_entry(fname, conn_id, line);

    /*
     * Reuse the incoming media in the outgoing message. Their are
     * a bunch of fields in the incoming media that we need
     * on the way out. And set the local addr and port that we
     * squirreled away after either a setup for an outgoing call
     * or a setup_ack for an incoming call.
     */
    media->sccp_media.addr = sapp_sccp_media.sccp_media.addr;
    media->sccp_media.port = sapp_sccp_media.sccp_media.port;

    sapp_sccp_cbs->openrcv_res(sapp_sccp_handle, GAPI_MSG_OPENRCV_RES,
                               conn_id, line, media, GAPI_CAUSE_OK);
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
    char              *fname = "sapp_sapp_startxmit";
    cc_feature_data_t data;
    
    sapp_debug_entry(fname, conn_id, line);

    sapp_copy_media_to_sdp(media, &(data.resume.sdp));
                     
    cc_feature(CC_SRC_SIP, sapp_sapp_call_id, sapp_sapp_line,
               CC_FEATURE_MEDIA, &data);    

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
                                  int line, gapi_features_t feature,
                                  gapi_feature_data_t *data,
                                  gapi_causes_t cause)
{
    char *fname = "sapp_sapp_feature_res";
    
    sapp_debug_entry(fname, conn_id, line);

    cc_feature_ack(CC_SRC_SIP, sapp_sapp_call_id, sapp_sapp_line,
                   (cc_features_t )feature, NULL, (cc_causes_t)cause);    

    return (0);
}

static int sapp_sapp_opensession_res (void *handle, int msg_id,
                                      gapi_causes_t cause)
{
    char *fname = "sapp_sapp_opensession_res";

    sapp_debug_entry(fname, 0, 0);

    SAPP_DEBUG("%s: 0-0: %s: cause= %d\n", fname, cause);
    
    return (0);
}

static int sapp_sapp_closesession_res (void *handle, int msg_id,
                                    gapi_causes_t cause)
{
    char *fname = "sapp_sapp_closesession_res";

    sapp_debug_entry(fname, 0, 0);

    SAPP_DEBUG("%s: 0-0: %s: cause= %d\n", SAPP_ID, fname, cause);
    
    return (0);
}

static int sapp_sapp_reset (void *handle, int msg_id,
                         gapi_causes_t cause)
{
    char *fname = "sapp_sapp_reset";

    sapp_debug_entry(fname, 0, 0);

    SAPP_DEBUG("%s: 0-0: %s: cause= %d\n", SAPP_ID, fname, cause);
    
    return (0);
}

static int sapp_sapp_conninfo (void *handle, int msg_id, int conn_id, int line,
                            gapi_conninfo_t *conninfo)
{
    char *fname = "sapp_sapp_conninfo";

    sapp_debug_entry(fname, conn_id, line);

    SAPP_DEBUG("calling_name: %s, calling_number: %s,\n"
               "called_name: %s, called_number: %s,\n"
               "connected_name: %s, connected_number: %s.\n",
               conninfo->calling_name, conninfo->calling_number,
               conninfo->called_name, conninfo->called_number,
               conninfo->connected_name, conninfo->connected_number);

    sapp_copy_conninfo_to_caller_id(conninfo,
                                    &sapp_sapp_conninfo2,
                                    0);

    if (sapp_gapi_waiting == 1)  {
        sapp_gapi_waiting = 0;
        
        cc_setup(CC_SRC_SIP, sapp_sapp_call_id, sapp_sapp_line,
                 &sapp_sapp_conninfo2, 0, &sapp_sapp_sdp, NULL, 0);
    }        
    else if (sapp_gapi_waiting == 2) {
        sapp_gapi_waiting = 0;
            
        cc_proceeding(CC_SRC_SIP, sapp_sapp_call_id, sapp_sapp_line,
                      &sapp_sapp_conninfo2);
    }
    else if (sapp_gapi_waiting == 3) {
        sapp_gapi_waiting = 0;
            
        cc_alerting(CC_SRC_SIP, sapp_sapp_call_id, sapp_sapp_line,
                    &sapp_sapp_conninfo2, NULL, 0);    
    }

    return (0);
}

static void *sapp_malloc (unsigned int size)
{
    return (malloc(size));
}

static void sapp_free (void *ptr)
{
    free(ptr);
}

static int sapp_queue_push (void *queue, void *data)
{
    SysHdr *psm;
    unsigned long *pdata;
   
    psm = (SysHdr *)IRXLstGet(&SysBuf_List, IRXNWAIT, NULL);
    if (psm == NULL)  {
        return (1);
    }
    
    psm->Cmd = 0xdddd;
    psm->Data = (uchar *)psm + sizeof(SysHdr);
    pdata = (unsigned long *)(psm->Data);
    *pdata = (unsigned long)data;    

    IRXLstPut(queue, (irx_buf_ptr)psm);
    
    return (0);
}

static void *sapp_queue_create (ssapi_queue_cleanup cleanup)
{
    return (sapp_sccp_queue);
}

static int sapp_queue_destroy (void)
{
    return (0);
}

static int sapp_socket_getlasterror (void)
{
//    return (WSAGetLastError());
    return (0);
}

static void sapp_free_socket2 (sapp_socket_t *socket2)
{
    socket2->id = -1;
    socket2->sapp_id = 0;
}

static sapp_socket_t *sapp_get_socket2_by_id (int id)
{
    int i;
    
    for (i = 0; i < SAPP_MAX_SOCKETS; i++) {
        if (sapp_sockets2[i].id == id) {
            return (&(sapp_sockets2[i]));
        }
    }
    
    return (NULL);
}

static unsigned long sapp_socket_getsockname (int socket)
{
    unsigned long addr;

    addr = sip_config_get_net_device_ipaddr();

    return (addr);
}

static char *sapp_socket_getmac (int data)
{
    static char mac_address[6];
    
    memcpy(mac_address, pDHCPInfo->my_mac_addr, 6);
    
    return (mac_address);
}

#if 0
static char *sapp_socket_getmacstr (char *buf, int buflen)
{
    char mac[6];

    memset(buf, 0, buflen);
    
    memcpy(mac, pDHCPInfo->my_mac_addr, 6);
        
    snprintf(buf, buflen, "%.4x%.4x%.4x",
             mac[0]*256+mac[1], mac[2]*256+mac[3], mac[4]*256+mac[5]);
  
    return (buf);
}

static long sapp_recv_data (void *data, unsigned long size,
                            unsigned int socket)
{
    sccpmsg_general_t *msg = NULL;
    sccpmsg_general_t *new_msg;
    unsigned long i;
    unsigned long total_msg_size = 0;
    long err;
    char *msg_buffer = (char *)data;
    
    if (size < sizeof(sccpmsg_base_t)) {
        SAPP_DEBUG("%s: ERROR : size < base, size = %d\n", SAPP_ID, size);
        return (1);
    }

    if (size > SCCPMSG_MAX_BUFFER_SIZE) {
        SAPP_DEBUG("%s: ERROR : size > buffer, size = %d\n", SAPP_ID, size);
        return (2);
    }

    msg = (sccpmsg_general_t *)msg_buffer;
    if (size < msg->base.length) {
        SAPP_DEBUG("%s: ERROR : size < buffer\n", SAPP_ID);
        return (3);
    }

    // Check for the number of messages in the buffer
    // We could have 1 or more than 1
    for (i = 0; i < size;) {
        total_msg_size = msg->base.length + sizeof(sccpmsg_base_t);

        /* sam
         * simulate a keepalive_ack, since a messages was received.
         */
        sccp_tcp_main(sapp_get_gapihandle(), msg, total_msg_size,
                      socket);

        i   += total_msg_size;

        msg = (sccpmsg_general_t *)(msg_buffer + i);
    }
    
    return (0);
}

static long sapp_tcp_wait_connected(void *thread)
{
    long result;

//    result = platform_wait_event(thread, PLATFORM_WAIT_INFINITE);

#if 0
    switch(result) {
    case WAIT_OBJECT_0:
        return (0);

    case WAIT_TIMEOUT:
        return (2);

    case WAIT_ABANDONED:
    case WAIT_FAILED:
    default:
        break;
    }
#endif
    return (1);
}

static long sapp_tcp_check_shutdown ()
{
    long status;

#if 0    
    status = platform_wait_event(sapp_tcp_thread_shutdown, 0);

    return (status == WAIT_TIMEOUT ? 0:1);
#endif
    return (0);
}

static unsigned int sapp_tcp_thread_handler (void *user_data)
{
    long wait_result;;

#if 0
    while (sapp_tcp_check_shutdown(/*sapp_tcp_thread_shutdown*/) == 0)
    {
        wait_result = sapp_tcp_wait_connected(sapp_tcp_thread_run);
        if (wait_result == 0) {
            sapp_recv_data(sapp_sockets[0]);
        }
        else if (wait_result == 0) {
            // Wait timed out
            SAPP_DEBUG("%s: ERROR : TCP Thread Handler timed out"
                       " waiting for connection to CM.\n",
                       SAPP_ID);
        }
    }
#endif
    return(0);
}
#endif

static int sapp_get_new_socket_id ()
{
    static int id = 0;
    
    if (++id < 0)  {
        id = 1;
    }
    
    return (id);    
}

static int sapp_socket_open (void)
{
    char *fname = "sapp_socket_open";
    sapp_socket_t *socket2;

    socket2 = sapp_get_socket2_by_id(0);
    if (socket2 == NULL)  {
        return (SSAPI_SOCKET_ERROR);
    }
    
    memset(socket2, 0, sizeof(*socket2));    
    
    socket2->id = sapp_get_new_socket_id();

    SAPP_DEBUG("%s: %s: socket= %d\n",
               SAPP_ID, fname, socket2->id);

    return (socket2->id);
}

static int sapp_socket_close (int socket)
{
    char *fname = "sapp_socket_close";
    sapp_socket_t *socket2;    

    socket2 = sapp_get_socket2_by_id(socket);
    if (socket2 == NULL)  {
        return (SSAPI_SOCKET_ERROR);
    }

    sapp_free_socket2(socket2);

    SAPP_DEBUG("%s: %s: socket= %d\n", SAPP_ID, fname, 0);

    return (0);
}

static int sapp_socket_connect (int socket, unsigned long addr,
                                unsigned short port)
{
    char *fname = "sapp_socket_connect";
    unsigned char rc;
    sapp_socket_t *socket2 = NULL;

    socket2 = sapp_get_socket2_by_id(socket);
    if (socket2 == NULL)  {
        return (SSAPI_SOCKET_ERROR);
    }
    
    socket2->socket.fhost     = ntohl(addr);
    socket2->socket.fsock     = ntoh(port);
    socket2->socket.pTaskList = &SIP_List;//&SOC_List;
    socket2->socket.taskId    = SIPid;//SOCid;
    socket2->socket.tos       = 0;

    rc = TCPOpen(&(socket2->socket));

    if (rc == 0xFF) {
        SAPP_DEBUG("%s: %s: ERROR: TCP Open: %d\n", SAPP_ID, fname, rc);
        return (SSAPI_SOCKET_INVALID);
    }
    
//    socket2->id = (int)rc;
    socket2->sapp_id = rc; 

    return (0);
}

static int sapp_socket_send (int socket, char *buf, int len)
{
//    char *fname = "sapp_socket_send";
    SysHdr *psm;
    sapp_socket_t *socket2 = NULL;    

//    return (0);
    
    socket2 = sapp_get_socket2_by_id(socket);
    if (socket2 == NULL)  {
        return (SSAPI_SOCKET_ERROR);
    }
    
    psm = (SysHdr *)IRXLstGet(&SysBuf_List, IRXNWAIT, NULL);
    if (psm == NULL)  {
        return (1);
    }
    
    psm->Cmd     = HTTP_RCV;
    psm->Data    = (uchar *)psm + APP_DATA_OFFSET;
    psm->Len     = len;
//    psm->Usr.UsrInfo = HTTP_RCV;
    psm->Usr.UsrPtr = NULL;
    memcpy(psm->Data, buf, len);
    psm->Misc[0] = socket2->sapp_id;
    psm->RetID   = SYS_BUFFER;
    
    TCPRcv(psm);    
    
    return (0);
}

static int sapp_socket_recv (int socket, char *buf, int len)
{
    return (0);
}

void *sapp_timer_allocate (void)
{
    return (timer_event_allocate());
}

typedef void (*sapp_timer_callback)(void *timer_event, void *param1, void *param2);
static void sapp_timer_initialize (void *timer, int period,
                                   sapp_timer_callback expiration,
                                   void *param1, void *param2)
{
    timer_event_initialize((timer_struct_type *)timer,
                           period / 20, expiration,
                           param1, param2);
}

static void sapp_timer_activate (void *timer)
{
    timer_event_activate((timer_struct_type *)timer);
}

static void sapp_timer_cancel (void *timer)
{
    timer_event_cancel((timer_struct_type *)timer);
}

static void sapp_timer_free (void *timer)
{
    timer_event_free((timer_struct_type *)timer);
}

static unsigned short sapp_ntohs (unsigned short data)
{
    return (ntohs(data));
}

static unsigned long sapp_ntohl (unsigned long data)
{
    return (ntohl(data));
}

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
        sem_debug     = sapp_debug;                    
    }
    else if ((argc == 2) && (argv[1][0] == '1'))  {
        sapp_test();
    }
    
    return (0);
}

static void *sapp_get_gapihandle(void)
{
    return (sapp_sccp_handle);
}

static int sapp_get_conn_id (void)
{
    return (sapp_sccp_conn_id);
}

static int sapp_get_line (void)
{
    return (sapp_sccp_line);
}

int sapp_init (void)
{
    char *fname = "sapp_init";
    int gapi_rc = 0;
    gapi_callbacks_t  *sccp_cbs = NULL;
    gapi_callbacks_t  sapp_cbs;
    ssapi_callbacks_t ssapi_cbs;
    int i;

    sapp_sccp_cbs = (gapi_callbacks_t *)malloc(sizeof(gapi_callbacks_t));
    if (sapp_sccp_cbs == NULL) {
         return (1);
    }

    memset(sapp_sccp_cbs, 0, sizeof(gapi_callbacks_t));

    /*
     * Setup the application and system callbacks.
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
    sapp_cbs.opensession_res  = sapp_sapp_opensession_res;
    sapp_cbs.closesession_res = sapp_sapp_closesession_res;
    sapp_cbs.sessionstatus    = sapp_sapp_sessionstatus;    
    sapp_cbs.resetsession     = sapp_sapp_resetsession;    
    sapp_cbs.conninfo         = sapp_sapp_conninfo;
    sapp_cbs.feature_res      = sapp_sapp_feature_res;    

    ssapi_cbs.timer_allocate   = sapp_timer_allocate;
    ssapi_cbs.timer_initialize = sapp_timer_initialize;
    ssapi_cbs.timer_activate   = sapp_timer_activate;
    ssapi_cbs.timer_cancel     = sapp_timer_cancel;
    ssapi_cbs.timer_free       = sapp_timer_free;

    ssapi_cbs.malloc = sapp_malloc;
    ssapi_cbs.free   = sapp_free;
    ssapi_cbs.memset = memset;
    ssapi_cbs.memcpy = memcpy;

    ssapi_cbs.htons = sapp_ntohs;
    ssapi_cbs.htonl = sapp_ntohl;
    
    ssapi_cbs.queue_create = sapp_queue_create;    
    ssapi_cbs.queue_push   = sapp_queue_push;

    ssapi_cbs.socket_open         = sapp_socket_open;
    ssapi_cbs.socket_close        = sapp_socket_close;
    ssapi_cbs.socket_connect      = sapp_socket_connect;
    ssapi_cbs.socket_send         = sapp_socket_send;
    ssapi_cbs.socket_recv         = sapp_socket_recv;
    ssapi_cbs.socket_getlasterror = sapp_socket_getlasterror;
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

    for (i = 0; i < SAPP_MAX_SOCKETS; i++) {
        sapp_free_socket2(&(sapp_sockets2[i]));
    }

    sapp_tcp_thread = 0; //platform_create_thread(sapp_tcp_thread_handler, NULL,
                           //                  &sapp_tcp_thread_id);
    if (sapp_tcp_thread == 0) {
        SAPP_DEBUG("%s: %s: ERROR: tcp_thread error.\n", SAPP_ID, fname);
    }

    sapp_tcp_thread_reading = 0; //platform_create_mutex(0);

    bind_show_keyword("sapp", sapp_cmd);
    
    /*
     * Other global connection related infos were initialized at compile-time.
     */
    sapp_sapp_conninfo2.calling_name   = strlib_empty();
    sapp_sapp_conninfo2.calling_number = strlib_empty();
    sapp_sapp_conninfo2.called_name    = strlib_empty();
    sapp_sapp_conninfo2.called_number  = strlib_empty();
    
    memset(&sapp_sccp_media, 0, sizeof(sapp_sccp_media));

    debug_bind_keyword("sapp",    &sapp_debug);        
    debug_bind_keyword("am",      &am_debug);            
    debug_bind_keyword("sccp",    &sccp_debug);        
    debug_bind_keyword("sccpmsg", &sccpmsg_debug);            
    debug_bind_keyword("sccpcc",  &sccpcc_debug);        
    debug_bind_keyword("sccpcm",  &sccpcm_debug);        
    debug_bind_keyword("sccprec", &sccprec_debug);        
    debug_bind_keyword("sccpreg", &sccpreg_debug);        
    debug_bind_keyword("sem",     &sem_debug);                
    
    return (0);
}

static gapi_tcp_events_t sapp_sapp_to_gapi_tcp_event(unsigned char type)
{
    gapi_tcp_events_t event;
    
    switch (type) {
    case (RCV_CMD):
        event = GAPI_TCP_EVENT_RECV;
        break;

    case (OPEN_CMD):
        event = GAPI_TCP_EVENT_OPEN;
        break;

    case (CLOSE_CMD):
        event = GAPI_TCP_EVENT_CLOSE;
        break;

    case (ACK_CMD):
        event = GAPI_TCP_EVENT_ACK;
        break;

    case (NACK_CMD):
        event = GAPI_TCP_EVENT_NACK;
        break;
        
    default:
        event = GAPI_TCP_EVENT_MIN;
        break;
    }
    
    return (event);
}

int sapp_sccp_main (void *data, int type)
{
    char *fname = "sapp_sccp_main";
    unsigned long *addr;
    SysHdr *psm = (SysHdr *)data;    
    Socket *socket;

    switch (type)  {
    case (0):           
        addr = (unsigned long *)(psm->Data);
        sapp_sccp_cbs->sccp_main(sapp_sccp_handle, (void *)(*addr));        
//        sccp_event_main((void *)(*addr));
        break;

    case (1):           
        socket = (Socket *)(psm->Usr.UsrPtr);

        SAPP_DEBUG("%s: %s: handle= %d: fhost= %x: fsock= %x:"
                   " type= %x\n: len= %d.\n",
                   SAPP_ID, fname,
                   socket->handle, socket->fhost,
                   socket->fsock, socket->msgType,
                   psm->Len);

        if (socket->msgType == RCV_CMD) {           
//            sapp_recv_data(psm->Data, psm->Len, sapp_sockets2[0].id);
            sapp_sccp_cbs->tcp_main(sapp_sccp_handle, 0,
                                    sapp_sockets2[0].id,
                                    GAPI_TCP_EVENT_RECV,
                                    psm->Data, psm->Len);
        }
        else  {
            sapp_sccp_cbs->tcp_main(sapp_sccp_handle, GAPI_MSG_TCP_EVENTS,
                                    sapp_sockets2[0].id,
                                    sapp_sapp_to_gapi_tcp_event(socket->msgType),
                                    NULL, 0);
        }       
        
        break;
    }
   
    return (0);
}

int sapp_test (void)
{
    gapi_cmaddr_t cms[5];
    
//    return (0);
    
    memset(cms, 0 , sizeof(*cms) * 5);

    /*
     * Set the addresses in host order. The stack will change it to network
     * order on the way out.
     */
//    cms[0].addr = 0x4066578a;//0x40665774; //0x4066578a;
    cms[0].addr = sip_config_get_proxy_ipaddr(3);
    cms[0].port = 0x07d0;
    
    sapp_opensession_req(cms, "123456789AB", NULL, 0, NULL); 
    
    return (1);   
}

void sapp_copy_caller_id (cc_caller_id_t *caller_id)
{
    sapp_sapp_conninfo2.calling_name =
        strlib_update(sapp_sapp_conninfo2.calling_name,
                      caller_id->calling_name);
    sapp_sapp_conninfo2.calling_number =
        strlib_update(sapp_sapp_conninfo2.calling_number,
                      caller_id->calling_number);
    sapp_sapp_conninfo2.called_name = 
        strlib_update(sapp_sapp_conninfo2.called_name,
                      caller_id->called_name);
    sapp_sapp_conninfo2.called_number = 
        strlib_update(sapp_sapp_conninfo2.called_number,
                      caller_id->called_number);        
}

static gapi_features_t sapp_cc_feature_to_gapi (int feature)
{
    return ((gapi_features_t )feature);
}

int sapp_process_cc_event (void *data)
{
    char *fname = "sapp_process_cc_event";
    cc_msg_t *msg = (cc_msg_t *)data;
    int rc = 1;
    
    switch (msg->msg.setup.msg_id)  {
    case (CC_MSG_SETUP):
        /*
         * Copy or set data.
         * sapp_sccp_conn_id will be set from setup_ack.
         */
        sapp_sapp_call_id = msg->msg.setup.call_id;
        sapp_sapp_line = msg->msg.setup.line;
        sapp_sccp_line = sapp_sapp_line;
        sapp_copy_caller_id_to_conninfo(&sapp_sccp_conninfo,
                                        &(msg->msg.setup.caller_id));
        
        sapp_call_direction = 0;

        /*
         * GSM sends the local sdp info, so save it away for when
         * SCCP requests the open_rcv.
         */
        sapp_copy_sdp_to_media(&sapp_sccp_media, &(msg->msg.setup.sdp));

        /*
         * Stick a fake port into the remote sdp. SCCP sends the
         * open_rcv before the start_xmit so we never know what the
         * remote port is before we send up a connect. GSM will kill
         * the call if we don't give it some port during the connect.
         */
        sapp_copy_media_to_sdp(NULL, &sapp_sapp_sdp);        
        sapp_sapp_sdp.remote_addr.addr = 0x0a50210d; /* 10.80.33.13 */
        sapp_sapp_sdp.remote_addr.port = 0x4000;     /* 16384 */        
        
        sapp_sccp_cbs->setup(sapp_sccp_handle, GAPI_MSG_SETUP,
                             sapp_sccp_conn_id, sapp_sccp_line, NULL,
                             (char *)(msg->msg.setup.caller_id.called_number),
                             strlen(msg->msg.setup.caller_id.called_number),
                             NULL, 0, 0);
    
        break;
        
    case (CC_MSG_SETUP_ACK):
        sapp_sapp_line = msg->msg.setup_ack.line;
        sapp_sccp_line = sapp_sapp_line;

        /*
         * GSM sends the local sdp info, so save it away for when
         * SCCP requests the open_rcv.
         */
        sapp_copy_sdp_to_media(&sapp_sccp_media, &(msg->msg.setup_ack.sdp));

        sapp_copy_caller_id_to_conninfo(&sapp_sccp_conninfo,
                                        &(msg->msg.setup_ack.caller_id));

        rc = sapp_sccp_cbs->setup_ack(sapp_sccp_handle, GAPI_MSG_SETUP_ACK,
                                      sapp_sccp_conn_id, sapp_sccp_line,
                                      NULL, NULL, GAPI_CAUSE_OK);
        
        break;                                      

    case (CC_MSG_PROCEEDING):
        rc = sapp_sccp_cbs->proceeding(sapp_sccp_handle, GAPI_MSG_PROCEEDING,
                                       sapp_sccp_conn_id, sapp_sccp_line, NULL);
        
        break;                                      

    case (CC_MSG_ALERTING):        
        rc = sapp_sccp_cbs->alerting(sapp_sccp_handle, GAPI_MSG_ALERTING,
                                     sapp_sccp_conn_id, sapp_sccp_line,
                                     NULL, NULL, 0);
        
        break;                                    

    case (CC_MSG_CONNECTED):
         sapp_copy_sdp_to_media(&sapp_sccp_media, &(msg->msg.connected.sdp));
         sapp_sccp_cbs->connect(sapp_sccp_handle, GAPI_MSG_CONNECT,
                                sapp_sccp_conn_id, sapp_sccp_line, NULL, NULL);

         break;
                
    case (CC_MSG_CONNECTED_ACK):
         sapp_sccp_cbs->connect_ack(sapp_sccp_handle, GAPI_MSG_CONNECT_ACK,
                                    sapp_sccp_conn_id, sapp_sccp_line,
                                    NULL, NULL);

        break;
                                           
    case (CC_MSG_RELEASE):
        rc = sapp_sccp_cbs->release(sapp_sccp_handle, GAPI_MSG_RELEASE,
                                    sapp_sccp_conn_id, sapp_sccp_line,
                                    GAPI_CAUSE_OK);
        
        break;                                      

    case (CC_MSG_RELEASE_COMPLETE):
        rc = sapp_sccp_cbs->release_complete(sapp_sccp_handle,
                                             GAPI_MSG_RELEASE_COMPLETE,
                                             sapp_sccp_conn_id, sapp_sccp_line,
                                             GAPI_CAUSE_OK);
        
        break;            
        
    case (CC_MSG_FEATURE):
        rc = sapp_sccp_cbs->feature_req(sapp_sccp_handle,
                                        GAPI_MSG_FEATURE_REQ,
                                        sapp_sccp_conn_id, sapp_sccp_line,
                                        sapp_cc_feature_to_gapi(msg->msg.feature.feature_id),
                                        NULL);
        
        break;            
        
    default:
        SAPP_DEBUG("%s: %s: Tossing unused cc event.\n", SAPP_ID, fname);    
    }

    SAPP_DEBUG("%s: %s: rc= %d.\n", SAPP_ID, fname, rc);

    free(data);
    
    return (rc);
}

int sapp_is_this_sccp (void *data)
{
    cc_msg_t *msg = (cc_msg_t *)data;
    char name[32];
    
    if (msg->msg.setup.msg_id == CC_MSG_SETUP)  {
        /*
         * Check who is making the call.
         */
        config_get_string(CFGID_LINE1_NAME, name);
         
        if (strcmp(name, msg->msg.setup.caller_id.calling_number) == 0) {
            return (0);
        }
        else {
            return (1);
        }
    }
    else if (msg->msg.setup.call_id == sapp_sapp_call_id)  {
        return (0);
    }
    else {
        return (1);
    }
}
