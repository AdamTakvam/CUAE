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
 *     sapp.c
 *
 *  CREATION/MODIFICATION DATES
 *     Created:  Nov 2002, Sam Hague
 *     Modified: 
 *
 *  DESCRIPTION
 *     SCCP Application platform independent implementation
 *
 *     There are some platform specifics in here, but for the most part,
 *     it uses the platform_xxx and sapp_xxx files to abstract the platform
 *     specifics.
 * 
 *  NOTES
 *     1. SAPP_ADD_RECURSION and SAPP_HAS_RECURSION are flags to control
 *        whether the software can use platform supplied recursion when
 *        calling mutexes. Use SAPP_HAS_RECURSION if the platform supports
 *        recursion. Otherwise, use SAPP_ADD_RECURSION so that the software
 *        implements its own version of recursion. ***SAPP_ADD_RECURSION has
 *        not been tested.
 */
#include "platform.h"
#include "sapp.h"
#include "gapi.h"

#if ((SAPP_APP == SAPP_APP_SCCPTEST) || (SAPP_APP == SAPP_APP_LOOPBACK))
#include "timer.h"
#endif

#include "ssapi.h"
#ifdef SAPP_PLATFORM_WIN32
#ifdef _DEBUG
#define _CRTDBG_MAP_ALLOC 
#include <crtdbg.h>
#endif
#endif


#ifdef SAPP_ADD_RECURSION
typedef struct sapp_mutex_t_ {
    void thread;
    void *mutex;
} sapp_mutext_t;
#endif

typedef struct sapp_socket_t_  {
    int id;
    int flags;
    platform_socket_t sapp_id;
    void *sinfo;
} sapp_socket_t;

typedef struct sapp_socket2_t_ {
    sapp_socket_t    sockets[SAPP_MAX_SOCKETS];
    void *mutex;
#ifdef SAPP_ADD_RECURSION
    sapp_mutex_t mutex;
#endif
    void *thread;
    void *event;
    int nready;
} sapp_socket2_t;

static char *sapp_cm_state_names[] = {
    "S_CLOSED",
    "S_CONNECTING",
    "S_CONNECTED",
    "S_REGISTERING",
    "S_REGISTERED"
};


static int sapp_flags = -1;

static unsigned int sapp_sccp_msg_id;
static int sapp_event_counter = 0;
int sapp_initialized = 0;

static int sapp_need_cleanup = 0;
int sapp_debug = 1;

sapp_info_t sapp_infos[SAPP_MAX_INFOS];

sapp_calls_t sapp_calls;

#if 0
gapi_callbacks_t *sapp_sccp_cbs;
void *sapp_sccp_handle;
void *sapp_sapp_handle;
sapp_states_e sapp_state = SAPP_S_IDLE;
gapi_causes_e sapp_reset_cause = GAPI_CAUSE_OK; 
int sapp_debug = 1;
sapp_status_data_info_t sapp_session_data;
sapp_cminfo_t sapp_cminfo[SAPP_MAX_CMS] =
{
    {{0, 0}, SAPP_CM_S_CLOSED}, {{0, 0}, SAPP_CM_S_CLOSED},
    {{0, 0}, SAPP_CM_S_CLOSED}, {{0, 0}, SAPP_CM_S_CLOSED},
    {{0, 0}, SAPP_CM_S_CLOSED}
};
#endif
gapi_cmaddr_t sapp_null_cmaddr = {0, 0};

#if 0
typedef struct sapp_event_t_ {
    int id;
    void *sinfo;
} sapp_event_t;
#endif
static sapp_socket2_t sapp_sockets2;
static void *sapp_sccp_thread;
static void *sapp_sccp_mutex;
static int  sapp_sccp_nready = 0;
static void *sapp_sccp_events[SAPP_MAX_INFOS];
static unsigned long sapp_sccp_tid;

static void *sapp_sccp_event;
#if 0
static char *sapp_mac_addr;
#endif
static char *SAPP_NAME1 = "SAPP1";
static char *SAPP_NAME2 = "SAPP2";


static unsigned long sapp_socket_getsockname(int socket);
static sapp_socket_t *sapp_get_socket2_by_id(int id);
static void sapp_tcp_thread_run(void *event);

static char *sapp_cm_state_name (int id)
{
    if ((id <= SAPP_CM_S_MIN) || (id >= SAPP_CM_S_MAX)) {
        return ("UNDEFINED");
    }

    return (sapp_cm_state_names[id]);
}

void sapp_debug_entry (char *fname, int conn_id, int line)
{
    SAPP_DEBUG("%s: %d:%d: %-25s\n", SAPP_ID, conn_id, line, fname);
}

#ifdef SAPP_ADD_RECURSION
static int sapp_mutex_lock (void *mutex)
{
    sapp_mutex_t *mtx = (sapp_mutex_t *)mutex;
    int rc;
    pthread_t thread_self;

    thread_self = pthread_self();
    if (pthread_equal(thread_self, mtx.thread) == 0) {
        rc = platform_mutex_lock(mtx.mutex, SAPP_SCCP_WAIT_TIMEOUT);
        if (rc != 0) {
            return (rc);
        }

        mtx.thread = thread_self;
    }

    return (0);
}
#endif

unsigned long sapp_local_addr ()
{
    unsigned long addr = 0;
    int i;

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

    return (addr);
}

#if 0 /* not used yet */
/*
 * FUNCTION:    sapp_get_new_call_id
 *
 * DESCRIPTION: Return a unique call_id.
 *
 * PARAMETERS:  None.
 *
 * RETURNS:     call_id
 */                    
static int sapp_get_new_call_id (void)
{
    static int id = 0;

    if (++id < 0) {
        id = 1;
    }

    return (id);
}
#endif

/*
 * FUNCTION:    sapp_free_call
 *
 * DESCRIPTION: Resets a call back to its initial values.
 *
 * PARAMETERS:  
 *     call: pointer to the call
 *
 * RETURNS:     None.
 */                    
void sapp_free_call (sapp_call_t *call)
{
    call->sapp_call_id = SAPP_NO_CONN_ID;
    call->sccp_conn_id = SAPP_NO_CONN_ID;
    
    call->sapp_line = SAPP_DEFAULT_LINE;
    call->sccp_line = SAPP_DEFAULT_LINE;

#if (SAPP_APP == SAPP_APP_GSM)
    call->sapp_conninfo.calling_name   = strlib_empty();
    call->sapp_conninfo.calling_number = strlib_empty();
    call->sapp_conninfo.called_name    = strlib_empty();
    call->sapp_conninfo.called_number  = strlib_empty();
#endif
#if ((SAPP_APP == SAPP_APP_SCCPTEST) || (SAPP_APP == SAPP_APP_LOOPBACK))
    memset(&(call->sccp_conninfo), 0, sizeof(call->sccp_conninfo));
    memset(&(call->sapp_conninfo), 0, sizeof(call->sapp_conninfo));
#endif
    memset(&(call->sccp_media), 0, sizeof(call->sccp_media));
    memset(&(call->sapp_media), 0, sizeof(call->sapp_media));

    call->gapi_waiting = 0;
}

/*
 * FUNCTION:    sapp_get_call_by_conn_id
 *
 * DESCRIPTION: Return the call identified by the conn_id.
 *
 * PARAMETERS:  
 *    id: connection identifier
 *
 * RETURNS:     pointer to the call if found, otherwise NULL
 */                    
sapp_call_t *sapp_get_call_by_conn_id (int id)
{
    sapp_call_t *call;
    sapp_call_t *call_found = NULL;
    sapp_info_t *sinfo = &(sapp_infos[0]);

#ifdef SAPP_HAS_RECURSION
    platform_mutex_lock(sapp_calls.mutex, SAPP_SCCP_WAIT_TIMEOUT);
#endif

    for (call = &(sapp_calls.calls[0]);
         call < &(sapp_calls.calls[SAPP_MAX_CALLS]);
         call++) {
        
        if (call->sccp_conn_id == id) {
            call_found = call;
            break;
        }
    }

#ifdef SAPP_HAS_RECURSION         
    platform_mutex_unlock(sapp_calls.mutex);
#endif

    return (call_found);
}

/*
 * FUNCTION:    sapp_get_call_by_call_id
 *
 * DESCRIPTION: Return the call identified by the call_id.
 *
 * PARAMETERS:  
 *    id: call identifier
 *
 * RETURNS:     pointer to the call if found, otherwise NULL
 */                    
sapp_call_t *sapp_get_call_by_call_id (int id)
{
    sapp_call_t *call;
    sapp_call_t *call_found = NULL;
    sapp_info_t *sinfo = &(sapp_infos[0]);

#ifdef SAPP_HAS_RECURSION
    platform_mutex_lock(sapp_calls.mutex, SAPP_SCCP_WAIT_TIMEOUT);
#endif

    for (call = &(sapp_calls.calls[0]);
         call < &(sapp_calls.calls[SAPP_MAX_CALLS]);
         call++) {
        
        if (call->sapp_call_id == id) {
            call_found = call;
            break;
        }
    }

#ifdef SAPP_HAS_RECURSION         
    platform_mutex_unlock(sapp_calls.mutex);
#endif

    return (call_found);
}

int sapp_get_call_list (int *list)
{
    sapp_call_t *call;
    int i = 0;    
    sapp_info_t *sinfo = &(sapp_infos[0]);

#ifdef SAPP_HAS_RECURSION
    platform_mutex_lock(sapp_calls.mutex, SAPP_SCCP_WAIT_TIMEOUT);
#endif

    for (call = &(sapp_calls.calls[0]);
         call < &(sapp_calls.calls[SAPP_MAX_CALLS]);
         call++) {
        
        if (call->sapp_call_id != SAPP_NO_CONN_ID) {
            list[i++] = call->sapp_call_id;
        }
    }

#ifdef SAPP_HAS_RECURSION         
    platform_mutex_unlock(sapp_calls.mutex);
#endif

    return (i);
}

static int sapp_sapp_all_streams_idle (void *handle)
{
    char *fname = "sapp_sapp_all_streams_idle";
    sapp_info_t *sinfo = (sapp_info_t *)handle;

    sapp_debug_entry(fname, 0, 0);

    SAPP_DEBUG("%s: %d-%d: %-25s: count= %d\n",
               SAPP_ID, 0, 0, fname, sapp_calls.count);

    return ((sapp_calls.count == 0) ? 1 : 0);
}

static int sapp_sapp_close_abandonded_streams (void *handle)
{
    char *fname = "sapp_sapp_close_abandonded_streams";
    sapp_info_t *sinfo = (sapp_info_t *)handle;

    sapp_debug_entry(fname, 0, 0);

    /*sam
    * maybe we should bomb the session data since this function is
    * only called when we try to find another primary.
    */

    sapp_calls.count = 0;

    return (sapp_calls.count);
}

/*
 * FUNCTION:    sapp_sccp_main2
 *
 * DESCRIPTION: Start function for the SCCP thread.
 *
 * PARAMETERS:  
 *    data: data passed to the thread at start
 *
 * RETURNS:     
 *    return code indicating exit.
 */                    
static platform_thread_func_t PLATFORM_CALLBACK sapp_sccp_main2 (void *data)
{
    int rc;
#ifdef SAPP_PLATFORM_WIN32
    sapp_info_t *sinfo;
    MSG msg;
    LPMSG lpmsg = &msg;
    HWND hwnd = NULL;
    int rc2;
#endif

    printf("%s: sccp task started\n", SAPP_ID);

#ifdef SAPP_PLATFORM_WIN32
    /*
     * Get the thread_id. The PostThreadMessage will need it.
     */
    sapp_sccp_tid = GetCurrentThreadId();

    /*
     * Force Windows to create a message queue.
     */
    PeekMessage(lpmsg, NULL, WM_USER, WM_USER, PM_NOREMOVE);
#endif

    /*
     * Sit in a loop processing events.
     */
    while (1) {
        platform_mutex_lock(sapp_sccp_mutex, SAPP_SCCP_WAIT_TIMEOUT);

        /*
         * Wait for events when we don't have anymore events to process.
         * The while loop was chosen instead of an if/then to correct
         * for spurious wakeups by the OS. We will always check the count
         * after the wakeup and go back to sleep if there are no more
         * events.
         */
        while (sapp_sccp_nready == 0) {
#ifdef SAPP_PLATFORM_UNIX
            /*
             * Unix releases the lock during the wait and then grabs
             * the lock again when the event is signaled.
             */
            platform_event_wait(sapp_sccp_event, (unsigned long)(sapp_sccp_mutex));
#endif
#ifdef SAPP_PLATFORM_WIN32
            platform_mutex_unlock(sapp_sccp_mutex);
            platform_event_wait(sapp_sccp_event, SAPP_SCCP_WAIT_TIMEOUT);

#if 0
            event = platform_event_wait_multiple(sapp_event_counter,
                                                 sapp_sccp_events,
                                                 SAPP_SCCP_WAIT_TIMEOUT);
            if (event == 0xFFFFFFFF) {
                printf(">>>>>>>>>>>> PROBLEM.\n");
            }
#endif
            platform_mutex_lock(sapp_sccp_mutex, SAPP_SCCP_WAIT_TIMEOUT);
#endif
        }

        /*
         * Check if we need to cleanup.
         */
        if (sapp_need_cleanup == 1) {
            platform_thread_exit(NULL, 0);
            return (0);
        }

#ifdef SAPP_PLATFORM_WIN32
        rc2 = GetMessage(lpmsg, hwnd, sapp_sccp_msg_id, sapp_sccp_msg_id);
#endif
        /*
         * Decrement the counter since we are going to process the
         * next event.
         */
        sapp_sccp_nready--;

        platform_mutex_unlock(sapp_sccp_mutex);

#ifdef SAPP_PLATFORM_WIN32
        sinfo = (sapp_info_t *)lpmsg->wParam;
#endif
        /*
         * Let the stack process the next event.
         */
        rc = sinfo->sapp_sccp_cbs->sccp_main(sinfo->sapp_sccp_handle, NULL);
    }
}

/*
 * FUNCTION:    sapp_free_socket2
 *
 * DESCRIPTION: Resets a socket2 back to its initial values.
 *
 * PARAMETERS:  
 *    socket2: pointer to the socket
 *
 * RETURNS:     None.
 */                    
static void sapp_free_socket2 (sapp_socket_t *socket2)
{
#ifdef SAPP_HAS_RECURSION
    platform_mutex_lock(sapp_sockets2.mutex, SAPP_SCCP_WAIT_TIMEOUT);
#endif

    socket2->id           = SAPP_NO_SOCKET;
    socket2->sapp_id      = SAPP_NO_SAPP_SOCKET;
    socket2->flags = 0;

#ifdef SAPP_HAS_RECURSION
    platform_mutex_unlock(sapp_sockets2.mutex);
#endif
}

/*
 * FUNCTION:    sapp_get_socket2_by_id
 *
 * DESCRIPTION: Return the socket2 identified by the id.
 *
 * PARAMETERS:  
 *    id: socket identifier
 *
 * RETURNS:     pointer to the socket2 if found, otherwise NULL
 */                    
static sapp_socket_t *sapp_get_socket2_by_id (int id)
{
    sapp_socket_t *socket2;
    sapp_socket_t *socket2_found = NULL;
    int i = 0;    

#ifdef SAPP_HAS_RECURSION
    platform_mutex_lock(sapp_sockets2.mutex, SAPP_SCCP_WAIT_TIMEOUT);
#endif

    for (socket2 = &(sapp_sockets2.sockets[0]);
         socket2 < &(sapp_sockets2.sockets[SAPP_MAX_SOCKETS]);
         socket2++) {
        
        SAPP_DEBUGP("%d: id= %d:%d\n", i++, socket2->id, id);
        
        if (socket2->id == id) {
            socket2_found = socket2;
            break;
        }
    }
    
#ifdef SAPP_HAS_RECURSION
    platform_mutex_unlock(sapp_sockets2.mutex);
#endif

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
    return (platform_get_local_mac_address(0));
}

/*
 * FUNCTION:    sapp_sccp_main2
 *
 * DESCRIPTION: Start function for the TCP thread.
 *
 * PARAMETERS:  
 *    data: data passed to the thread at start
 *
 * RETURNS:     
 *    return code indicating exit.
 */                    
static platform_thread_func_t PLATFORM_CALLBACK sapp_tcp_main (void *data)
{
    static char msg_buffer[SAPP_MAX_SCCP_BUFFER_SIZE];
    int  size = 0;
    int  error;
    fd_set readset;
    fd_set writeset;
    fd_set exceptset;
    platform_timeval_t timeout;
    platform_socket_t maxfdp1;
    int decrement;
    sapp_socket_t *sapp_socket2;
    sapp_socket_t *sapp_socket;
    int nready;
    sapp_info_t *sinfo;// = (sapp_info_t *)data;

    printf("%s: tcp task started\n", SAPP_ID);

    timeout.tv_sec  = 1;
    timeout.tv_usec = 0;

    /*
     * Wait until the user opens a socket. An event
     * will be sent to wake us up when the user connects a socket.
     */
#ifdef SAPP_PLATFORM_UNIX
    platform_mutex_lock(sapp_sockets2.mutex, SAPP_SCCP_WAIT_TIMEOUT);
    platform_event_wait(sapp_sockets2.event, (unsigned long)(sapp_sockets2.mutex));
    platform_mutex_unlock(sapp_sockets2.mutex);
#endif
#ifdef SAPP_PLATFORM_WIN32
    platform_event_wait(sapp_sockets2.event, SAPP_SCCP_WAIT_TIMEOUT);
#endif

    /*
     * Sit in a loop polling the sockets.
     */
    while (1) {
        /*
         * Check if we need to cleanup.
         */
        if (sapp_need_cleanup == 1) {
            platform_thread_exit(NULL, 0);
            return (0);
        }

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
#if 1
                /* 
                 * Skip sockets that have had an error. The TCP thread will
                 * keep reporting errors until the user closes the socket, so
                 * this flag will help us skip checking the socket the next
                 * time around.
                 */
                if (sapp_socket2->flags == 2) {
                    continue;
                }
#endif
                /*
                 * Remember to track the max fd.
                 */
                if (sapp_socket2->sapp_id > maxfdp1) {
                    maxfdp1 = sapp_socket2->sapp_id;
                }

                /*
                 * Add this socket to the set so that the select checks it
                 * for activity.
                 */
                FD_SET(sapp_socket2->sapp_id, &readset);

                /*
                 * Do we need to see if the socket was just connected? The
                 * flag is set when the socket is connected.
                 */
                if (sapp_socket2->flags == 1) {
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
#ifdef SAPP_PLATFORM_UNIX
            platform_mutex_lock(sapp_sockets2.mutex, SAPP_SCCP_WAIT_TIMEOUT);
            platform_event_wait(sapp_sockets2.event, (unsigned long)(sapp_sockets2.mutex));
            platform_mutex_unlock(sapp_sockets2.mutex);
#endif
#ifdef SAPP_PLATFORM_WIN32
            platform_event_wait(sapp_sockets2.event, SAPP_SCCP_WAIT_TIMEOUT);
#endif            

            continue;
        }

        /*
         * Remember to go one beyond the max fd.
         */
        maxfdp1++;

        /*
         * See if any sockets have activity.
         */
        nready = select(maxfdp1, &readset, &writeset, &exceptset, &timeout);
        
        SAPP_DEBUGP("tcp nready 1= %d\n", nready);
        
        /*
         * Did anything happen?
         */
        if (nready > 0) {
            platform_mutex_lock(sapp_sockets2.mutex, SAPP_SCCP_WAIT_TIMEOUT);
            
            /*
             * Find the sockets that had activity and process...
             */
            for (sapp_socket = &(sapp_sockets2.sockets[0]);
                 sapp_socket < &(sapp_sockets2.sockets[SAPP_MAX_SOCKETS]);
                 sapp_socket++) {
                
                SAPP_DEBUGP("tcp nready 1.5= %d, %d\n", nready, sapp_socket->id);
                
                /*
                 * nready indicates the number of sockets that had activity,
                 * so lets track each socket that is processed and decrement
                 * nready. This way we drop out of the loop without having to
                 * go through the whole list.
                 */
                decrement = 0;

                /*
                 * Skip sockets that have not been opened.
                 */
                if (sapp_socket->id == SAPP_NO_SOCKET) {
                    continue;
                }

                sinfo = sapp_socket->sinfo;

                /*
                 * Check for a successful connection. Sockets will report
                 * that the socket is writeable when the socket connects
                 * sucessfully.
                 */
                if ((sapp_socket->flags == 1) &&
                    (FD_ISSET(sapp_socket->sapp_id, &writeset))) {
                    sapp_socket->flags = 0;

                    SAPP_DEBUG("%s: socket= %d: connected: addr=0x%08x:%d\n",
                               SAPP_ID, sapp_socket->sapp_id,
                               htonl(platform_getsockname(sapp_socket->sapp_id)),
                               htons(platform_getsockport(sapp_socket->sapp_id)));

#if 1
					/* used to simulate a failed connection. */
                    sinfo->sapp_sccp_cbs->tcp_main(sinfo->sapp_sccp_handle, GAPI_MSG_TCP_EVENTS,
                                            sapp_socket->id,
                                            GAPI_TCP_EVENT_OPEN,
                                            NULL, 0);
#else /* test code */
                    sinfo->sapp_sccp_cbs->tcp_main(sinfo->sapp_sccp_handle, GAPI_MSG_TCP_EVENTS,
                                            sapp_socket->id,
                                            GAPI_TCP_EVENT_NACK,
                                            NULL, 0);
#endif

                    decrement = 1;
                }

                /*
                 * Check for a failed connection. Sockets will report 
                 * an exception if the connection fails.
                 */
                if ((sapp_socket->flags == 0) &&
                    (FD_ISSET(sapp_socket->sapp_id, &exceptset))) {
                    sapp_socket->flags = 0;
                    
                    SAPP_DEBUG("%s: socket= %d: except STATUS: failed connection to CM\n\n",
                               SAPP_ID, sapp_socket->id);

                    sinfo->sapp_sccp_cbs->tcp_main(sinfo->sapp_sccp_handle,
                                            GAPI_MSG_TCP_EVENTS,
                                            sapp_socket->id,
											GAPI_TCP_EVENT_NACK,
                                            //GAPI_TCP_EVENT_CLOSE,
                                            NULL, 0);
                    decrement = 1;
                }

                /*
                 * Check for any data that was received.
                 */
                if (FD_ISSET(sapp_socket->sapp_id, &readset)) {
                    memset(msg_buffer, 0, sizeof(msg_buffer));
                    size = recv(sapp_socket->sapp_id, msg_buffer,
                                SAPP_MAX_SCCP_BUFFER_SIZE, 0);
                    //printf("tcp_main[%d] size= %d\n", sapp_socket->sapp_id, size);

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
                            SAPP_DEBUG("\n%s: socket= %d: STATUS: Connection "
                                       "to CM disconnected\n\n",
                                       SAPP_ID, sapp_socket->id);

                            sinfo->sapp_sccp_cbs->tcp_main(sinfo->sapp_sccp_handle,
                                                    GAPI_MSG_TCP_EVENTS,
                                                    sapp_socket->id,
                                                    GAPI_TCP_EVENT_CLOSE,
                                                    NULL, 0);

#if 1
                            /*
                             * Remove the socket from the list so that we do
                             * not keep grabbing errors from it. The stack will
                             * (hopefully) close the socket which will remove
                             * it from the sapp_sockets list.
                             */
                            //FD_CLR(sapp_socket->sapp_id, &readset);
                            sapp_socket->flags = 2;
#endif
                        }
                    }
                    else if (size == 0) {
                        /*
                         * Connection has been closed - let the stack know.
                         */
                        SAPP_DEBUG("\n%s: socket= %d: STATUS: Connection to "
                                   "CM disconnected\n\n",
                                   SAPP_ID, sapp_socket->id);

                        sinfo->sapp_sccp_cbs->tcp_main(sinfo->sapp_sccp_handle,
                                                GAPI_MSG_TCP_EVENTS,
                                                sapp_socket->id,
                                                GAPI_TCP_EVENT_CLOSE,
                                                NULL, 0);
#if 1
                        sapp_socket->flags = 2;
#endif
                    }
                    else {
                        /*
                         * Got some good data - give it to the stack.
                         */
                        sinfo->sapp_sccp_cbs->tcp_main(sinfo->sapp_sccp_handle, 0,
                                                sapp_socket->id,
                                                GAPI_TCP_EVENT_RECV,
                                                msg_buffer, (int)size);
                    }

                    decrement = 1;
                } /* if (FD_ISSET(sapp_socket->sapp_id, &readset)) */
                /*
                 * Check if all sockets have been processed.
                 */
                if ((nready -= decrement) <= 0) {
                    break;
                }
            } /* for (i = 0; i < SAPP_MAX_SOCKETS; i++) */
            
            platform_mutex_unlock(sapp_sockets2.mutex);
        } /* if (error > 0)  */
    } /* while (1) */
}

#if 0 /* not used yet */
/*
 * FUNCTION:    sapp_get_new_socket_id
 *
 * DESCRIPTION: Return a unique socket identifier.
 *
 * PARAMETERS:  None.
 *
 * RETURNS:     socket id
 */                    
static int sapp_get_new_socket_id ()
{
    static int id = 0;
    
    if (++id < 0)  {
        id = 1;
    }
    
    return (id);    
}
#endif

static int sapp_socket_open (int *socket)
{
    char              *fname    = "sapp_socket_open";
    sapp_socket_t     *socket2;
    int               rc;
    platform_socket_t id = 0;

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
    socket2->id      = (int)id;
    socket2->sapp_id = id;
    socket2->sinfo   = (void *)(*socket);

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
    
    SAPP_DEBUG("%s: %-25s: socket= %d:%d, addr=0x%08x:%d\n",
               SAPP_ID, fname, *socket, id,
               htonl(platform_getsockname(id)),
               htons(platform_getsockport(id)));
#if 0 // test code
	return (1);
#endif
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
    char          *fname   = "sapp_socket_connect";
    int           rc;
    sapp_socket_t *socket2;

    platform_mutex_lock(sapp_sockets2.mutex, SAPP_SCCP_WAIT_TIMEOUT);
    
    socket2 = sapp_get_socket2_by_id(socket);
    if (socket2 == NULL)  {
        platform_mutex_unlock(sapp_sockets2.mutex);

        return (SSAPI_SOCKET_ERROR);
    }

    SAPP_DEBUG("%s: %-25s: socket= %d: addr=0x%08x:%d\n",
               SAPP_ID, fname, socket2->id,
               htonl(platform_getsockname(socket2->sapp_id)),
               htons(platform_getsockport(socket2->sapp_id)));

    rc = platform_connect(socket2->sapp_id, addr, port);
    if (rc  != 0) {
        switch (rc) {
#ifdef SAPP_PLATFORM_WIN32
        case (PLATFORM_EWOULDBLOCK):
#endif
#ifdef SAPP_PLATFORM_UNIX
        case (PLATFORM_EINPROGRESS):
#endif
            /*
             * Mark this socket so that we know to check it for 
             * connection completion later in the tcp thread select.
             */
            socket2->flags = 1;

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

    /*
     * Stoke the tcp thread so it can start checking the socket for activity.
     */
    sapp_tcp_thread_run(sapp_sockets2.event);
#if 0 // test code
	return (1);
#endif

    return (rc);
}

static int sapp_socket_send (int socket, char *buf, int len)
{
    char          *fname   = "sapp_socket_send";
    int           error;
    sapp_socket_t *socket2 = NULL;
    sapp_info_t *sinfo = &(sapp_infos[0]);

    platform_mutex_lock(sapp_sockets2.mutex, SAPP_SCCP_WAIT_TIMEOUT);
    
    socket2 = sapp_get_socket2_by_id(socket);
    if (socket2 == NULL)  {
        platform_mutex_unlock(sapp_sockets2.mutex);

        return (SSAPI_SOCKET_ERROR);
    }

    SAPP_DEBUG("%s: %-25s: socket= %d: addr=0x%08x:%d\n",
               SAPP_ID, fname, socket2->id,
               htonl(platform_getsockname(socket2->sapp_id)),
               htons(platform_getsockport(socket2->sapp_id)));

    error = send(socket2->sapp_id, buf, len, 0);
    if (error == PLATFORM_SOCKET_ERROR) {
        error = platform_get_last_error();
        SAPP_DEBUG("%s: %-25s: socket= %d: ERROR: %d:%s\n",
                   SAPP_ID, fname, socket2->id,
                   error, platform_get_last_error_string(error));
        /*sam
         * SAPP should close the socket and notify PSCCP.
         */
    }
    else if (error == 0) { 
        /*
        * Uh, oh, the socket closed. Let the stack know.
        */
        sinfo->sapp_sccp_cbs->tcp_main(sinfo->sapp_sccp_handle, GAPI_MSG_TCP_EVENTS,
                                socket,
                                GAPI_TCP_EVENT_CLOSE,
                                NULL, 0);
    }
    else {
        error = 0;
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

/*sam
 * Kind of dumb to just wrap these ntoh functions, but for some reason
 * when I tried to just assign the ntoh to ssapi_cbs->ntoh, it caused
 * crashes.
 */
static unsigned short sapp_ntohs (unsigned short data)
{
    return (ntohs(data));
}

static unsigned long sapp_ntohl (unsigned long data)
{
    return (ntohl(data));
}

#if 0
static int sapp_sccp_thread_attach (void *thread, gapi_handle_t *handle)
{
    sapp_info_t *sinfo;

    /*
     * Find sapp_info that matches thread.
     */
    for (sinfo = &(sapp_infos[0]); sinfo < &(sapp_infos[SAPP_MAX_INFOS]);
         sinfo++) {
        if (sinfo->sapp_sccp_handle == handle) {
            sinfo->sapp_sccp_thread = thread;
            return (0);
        }
    }

    return (1);
}
#endif

static void *sapp_sccp_thread_get (void)
{
    //return (sapp_infos[sapp_event_counter++].sapp_sccp_event);
    return (&(sapp_infos[sapp_event_counter++]));
}

static void sapp_sccp_thread_run (void *event)
{
    int rc;
    int rc2;

    SAPP_DEBUGP("sapp_sccp_thread_run: 1\n");

#ifdef SAPP_PLATFORM_UNIX
    platform_mutex_lock(sapp_sccp_mutex, SAPP_SCCP_WAIT_TIMEOUT);
    if (sapp_sccp_nready == 0) {
        platform_event_set(event);
    }
    sapp_sccp_nready++;
    platform_mutex_unlock(sapp_sccp_mutex);
#endif
#ifdef SAPP_PLATFORM_WIN32
    /*
     * Windows performs a thread-switch as soon as the event is set,
     * so we need to reorder things as opposed to Unix.
     */
    platform_mutex_lock(sapp_sccp_mutex, SAPP_SCCP_WAIT_TIMEOUT);
    SAPP_DEBUGP("sapp_sccp_thread_run: 1.3: %d\n", sapp_sccp_nready);
    sapp_sccp_nready++;
    rc = PostThreadMessage(sapp_sccp_tid, sapp_sccp_msg_id,
                           (unsigned long)event, 0);
    if (rc == 0) {
        rc2 = GetLastError();
    }
    platform_mutex_unlock(sapp_sccp_mutex);
    SAPP_DEBUGP("sapp_sccp_thread_run: 1.5: %d\n", sapp_sccp_nready);
    platform_event_set(sapp_sccp_event);
#endif

    SAPP_DEBUGP("sapp_sccp_thread_run: 2\n");
}

static void sapp_tcp_thread_run (void *event)
{
    SAPP_DEBUGP("sapp_tcp_thread_run: 1\n");
#ifdef SAPP_PLATFORM_UNIX
    platform_mutex_lock(sapp_sockets2.mutex, SAPP_SCCP_WAIT_TIMEOUT);
    platform_event_set(event);
    platform_mutex_unlock(sapp_sockets2.mutex);
#endif
#ifdef SAPP_PLATFORM_WIN32
    platform_event_set(event);
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

sapp_cminfo_t *sapp_get_cminfo (unsigned long addr, unsigned short port,
                                gapi_handle_t handle)
{
    sapp_cminfo_t *cminfo;
    sapp_cminfo_t *cminfo_found = NULL;
    sapp_info_t *sinfo = handle;

    for (cminfo = &(sinfo->sapp_cminfo[0]); cminfo < &(sinfo->sapp_cminfo[SAPP_MAX_CMS]);
        cminfo++) {
        if ((cminfo->cmaddr.addr == addr) &&
            (cminfo->cmaddr.port == port)) {
            cminfo_found = cminfo;
            break;
        }
    }

    return (cminfo_found);
}

#if 0 /* not used yet */
static sapp_free_cminfo (sapp_cminfo_t *cminfo)
{
    cminfo->cmaddr.addr = 0;
    cminfo->cmaddr.port = 0;
    cminfo->state = SAPP_CM_S_CLOSED;
}
#endif

gapi_cmaddr_t *sapp_get_cmaddr_by_state2 (sapp_cm_states_e state)
{
    return (sapp_get_cmaddr_by_state(state, &(sapp_infos[0])));
}

gapi_cmaddr_t *sapp_get_cmaddr_by_state (sapp_cm_states_e state,
                                         gapi_handle_t handle)
{
    sapp_cminfo_t *cminfo;
    sapp_cminfo_t *cminfo_found = NULL;
    gapi_cmaddr_t *cmaddr       = &sapp_null_cmaddr;
    sapp_info_t *sinfo = handle;

    for (cminfo = &(sinfo->sapp_cminfo[0]); cminfo < &(sinfo->sapp_cminfo[SAPP_MAX_CMS]);
         cminfo++) {
        if (cminfo->state == state) {
            cminfo_found = cminfo;
            break;
        }
    }

    if (cminfo_found != NULL) {
        cmaddr = &(cminfo_found->cmaddr);
    }

    return (cmaddr);
}

void sapp_showsession (void)
{
    int i, j;
    unsigned char *p;
    sapp_cminfo_t *cminfo;
    sapp_info_t *sinfo = &(sapp_infos[0]);

    printf("\n\n---- SAPP SESSION ---\n");

    printf("\naddress                    state\n");
    for (cminfo = &(sinfo->sapp_cminfo[0]); cminfo < &(sinfo->sapp_cminfo[SAPP_MAX_CMS]);
         cminfo++) {
        p = (unsigned char *)(&(cminfo->cmaddr.addr));
        printf("[%03d:%03d:%03d:%03d:%06d] - %s\n",
               p[0], p[1], p[2], p[3], ntohs(cminfo->cmaddr.port),
               sapp_cm_state_name(cminfo->state));
    }

    printf("\nstate= %d, call_count= %d\n", sinfo->sapp_state, sapp_calls.count);

    printf("\nlinecnt= %d\n",
           sinfo->sapp_session_data.linecnt);

    for (i = 0; i < sinfo->sapp_session_data.linecnt; i++) {
        printf("[%d] %s : %s : %s\n",
               sinfo->sapp_session_data.lines[i].instance,
               sinfo->sapp_session_data.lines[i].dn,
               sinfo->sapp_session_data.lines[i].fqdn,
               sinfo->sapp_session_data.lines[i].label);
    }

    printf("\nspeeddialcnt= %d\n",
           sinfo->sapp_session_data.speeddialcnt);

    for (i = 0; i < sinfo->sapp_session_data.speeddialcnt; i++) {
        printf("[%d] %s : %s\n",
               sinfo->sapp_session_data.speeddials[i].instance,
               sinfo->sapp_session_data.speeddials[i].dn,
               sinfo->sapp_session_data.speeddials[i].fqdn);
    }

    printf("\nfeaturecnt= %d\n",
           sinfo->sapp_session_data.featurecnt);

    for (i = 0; i < sinfo->sapp_session_data.featurecnt; i++) {
        printf("[%d] %s : %d:%d\n",
               sinfo->sapp_session_data.features[i].instance,
               sinfo->sapp_session_data.features[i].label,
               sinfo->sapp_session_data.features[i].id,
               sinfo->sapp_session_data.features[i].status);
    }

    printf("\nserviceurlcnt= %d\n",
           sinfo->sapp_session_data.serviceurlcnt);

    for (i = 0; i < sinfo->sapp_session_data.serviceurlcnt; i++) {
        printf("[%d] %s : %s\n",
               sinfo->sapp_session_data.serviceurls[i].instance,
               sinfo->sapp_session_data.serviceurls[i].url,
               sinfo->sapp_session_data.serviceurls[i].display_name);
    }

    printf("\nsoftkeycnt= %d",
           sinfo->sapp_session_data.softkeycnt);

    for (i = 0; i < sinfo->sapp_session_data.softkeycnt; i++) {
        if ((i % 4) == 0) {
            printf("\n");
        }

        printf("%04x:%-12.12s  ",
               sinfo->sapp_session_data.softkeys[i].event,
               sinfo->sapp_session_data.softkeys[i].label);
    }

    printf("\n\nsoftkeysetcnt= %d",
           sinfo->sapp_session_data.softkeysetcnt);

    for (i = 0; i < sinfo->sapp_session_data.softkeysetcnt; i++) {
        printf("\n[%02d] ", i);

        for (j = 0; j < GAPI_MAX_SOFTKEY_INDEXES; j++) {
            if ((j > 0) && ((j % 8) == 0)) {
                printf("\n     ");
            }

            printf("%02x:%04x  ",
                   sinfo->sapp_session_data.softkeysets[i].template_index[j],
                   sinfo->sapp_session_data.softkeysets[i].info_index[j]);
        }
    }

    printf("\n");
}

int sapp_get_conn_id (void)
{
    sapp_info_t *sinfo = &(sapp_infos[0]);

    return (sapp_calls.calls[0].sapp_call_id);
}

int sapp_get_line (void)
{
    sapp_info_t *sinfo = &(sapp_infos[0]);

    return (sapp_calls.calls[0].sapp_line);
}

void *sapp_get_gapihandle(void)
{
    sapp_info_t *sinfo = &(sapp_infos[0]);

    return (sinfo->sapp_sccp_handle);
}

int sapp_get_num_lines (void)
{
    sapp_info_t *sinfo = &(sapp_infos[0]);

    return (sinfo->sapp_session_data.linecnt);
}

char *sapp_get_line_name (int line)
{
    sapp_info_t *sinfo = &(sapp_infos[0]);

    return (sinfo->sapp_session_data.lines[line- 1].dn);
}

int sapp_get_active_conn_id (void)
{
    return (0);
}

int sapp_get_active_line (void)
{
    return (0);
}

void sapp_set_active_line (int line)
{

}

#ifdef SAPP_PLATFORM_79XXWIN
typedef void (*sapp_timer_callback)(void *timer_event, void *param1, void *param2);

/*
 * The 79xx implements the period ticks as 20ms ticks, so we need to adjust
 * our period by 20. The user passes periods in milliseconds.
 */
static void sapp_timer_initialize (void *timer, int period,
                                   sapp_timer_callback expiration,
                                   void *param1, void *param2)
{
    timer_event_initialize((timer_struct_type *)timer,
                           period / 20, expiration,
                           param1, param2);
}
#endif
#if 0 /* SAPP_DOCUMENTATION */
#if 0
#endif
/*
 *  Copyright (c) 2002, 2003, 2004 by Cisco Systems, Inc. All Rights Reserved.
 */
#include "gapi.h" 
#include "ssapi.h" 
   

typedef void *ssapi_thread_t;
typedef void *gapi_handle_t;

#if 0
typedef enum sapp_states_ {
    SAPP_S_IDLE,
    SAPP_S_REGISTERED
} sapp_states_e;


static sapp_states_e     sapp_state;            /* PSCCP session state */
#endif

static ssapi_thread_t    sapp_psccp_thread;     /* psccp thread handle */
static ssapi_thread_t    sapp_tcp_thread;       /* tcp thread handle */
static gapi_handle_t     sapp_sapp_handle2;     /* sapp callbacks handle */
static gapi_handle_t     sapp_psccp_handle;     /* psccp callbacks handle */
static gapi_callbacks_t  sapp_psccp_cbs;        /* psccp callbacks */

static int               sapp_ready_to_run;     /* event counter */
static char              sapp_tcp_buffer[1400]; /* tcp data buffer */
static int               sapp_conn_id;

/*
 * Define the application's callbacks. These are the callbacks that the 
 * stack will call.
 */
static ssapi_thread_t sapp_thread_get_cb (void)
{
    return (sapp_psccp_thread);
}

static void sapp_thread_run_cb (ssapi_thread_t *thread)
{
    /*
     * Schedule psccp thread to run.
     */
    sapp_ready_to_run++;
}

static int sapp_opensession_res_cb (gapi_handle_t handle,
                                    gapi_msgs_e msg_id,
                                    gapi_causes_e cause)
{
    if (cause == GAPI_CAUSE_OK) {
        sapp_state = SAPP_S_REGISTERED;
    }
    else {
        ; /* Do something. */
    }

    return (0);
}

int sapp_sessionstatus_cb (gapi_handle_t handle, int msg_id,
                           gapi_status_e status, 
                           gapi_status_data_types_e type,
                           gapi_status_data_u *data)
{
    return (0);
}

static int sapp_resetsession_res_cb (gapi_handle_t handle,
                                     gapi_msgs_e msg_id,
                                     gapi_causes_e cause)
{
    if (cause == GAPI_CAUSE_OK) {
        sapp_state = SAPP_S_IDLE;
    }
    else {
        ; /* Do something. */
    }

    return (0);
}

static int sapp_setup_cb (gapi_handle_t handle, int msg_id,
                          int conn_id, int line,
                          gapi_conninfo_t *conninfo, char *digit,
                          int numdigits,
                          gapi_media_t *media,
                          int alert_info, int privacy,
                          gapi_precedence_t *precedence)
{
    sapp_conn_id = conn_id;

    /*
     * Accept the incoming call.
     */
    sapp_psccp_cbs.setup_ack(sapp_psccp_handle, GAPI_MSG_SETUP_ACK,
                             conn_id, line,
                             NULL, NULL, GAPI_CAUSE_OK, NULL);

    sapp_psccp_cbs.proceeding(sapp_psccp_handle, GAPI_MSG_PROCEEDING,
                              conn_id, line, NULL,
                              NULL);

    sapp_psccp_cbs.alerting(sapp_psccp_handle, GAPI_MSG_ALERTING,
                            conn_id, line,
                            NULL, NULL, 0, NULL);

    /*
     * Answer the call.
     */
    sapp_psccp_cbs.connect(sapp_psccp_handle, GAPI_MSG_CONNECT,
                           conn_id, line,
                           NULL, NULL, NULL);

    return (0);
}

static int sapp_setup_ack_cb (gapi_handle_t handle, int msg_id,
                              int conn_id, int line,
                              gapi_conninfo_t *conninfo,
                              gapi_media_t *media,
                              gapi_causes_e cause,
                              gapi_precedence_t *precedence)
{
    /*
     * Call was accepted.
     */
    return (0);
}

static int sapp_proceeding_cb (gapi_handle_t handle, gapi_msgs_e msg_id,
                               int conn_id, int line,
                               gapi_conninfo_t *conninfo,
                               gapi_precedence_t *precedence)
{
    /*
     * Stop collecting digits.
     */
    return (0);
}

static int sapp_alerting_cb (gapi_handle_t handle, gapi_msgs_e msg_id,
                             int conn_id, int line,
                             gapi_conninfo_t *conninfo, gapi_media_t *media,
                             gapi_inband_e inband,
                             gapi_precedence_t *precedence)
{
    /*
     * Provide ringback to the user.
     */
    return (0);
}

static int sapp_connect_cb (gapi_handle_t handle, gapi_msgs_e msg_id,
                            int conn_id, int line,
                            gapi_conninfo_t *conninfo, gapi_media_t *media,
                            gapi_precedence_t *precedence)
{
    /*
     * Provide notification to the user.
     */
    return (0);
}

static int sapp_connect_ack_cb (gapi_handle_t handle,
                                int msg_id, int conn_id,
                                int line, gapi_conninfo_t *conninfo,
                                gapi_media_t *media,
                                gapi_precedence_t *precedence)
{
    /*
     * Provide notification to the user.
     */
    return (0);
}

static int sapp_release_cb (gapi_handle_t handle,
                            gapi_msgs_e msg_id,
                            int conn_id, int line,
                            gapi_causes_e cause,
                            gapi_precedence_t *precedence)
{
    sapp_psccp_cbs.release_complete(sapp_psccp_handle,
                                    GAPI_MSG_RELEASE_COMPLETE,
                                    sapp_conn_id, 1,
                                    GAPI_CAUSE_OK, NULL);

    /*
     * Free all resources related with this call.
     */
    sapp_conn_id = 0;

    return (0);
}

static int sapp_release_complete_cb (gapi_handle_t handle,
                                     gapi_msgs_e msg_id,
                                     int conn_id, int line,
                                     gapi_causes_e cause,
                                     gapi_precedence_t *precedence)
{
    /*
     * Free all resources related with this call.
     */
    sapp_conn_id = 0;

    return (0);
}

static int sapp_openrcv_req_cb (gapi_handle_t handle,
                                int msg_id, int conn_id,
                                int line, gapi_media_t *media)
{
    /*
     * Get an RTP to receive media.
     */
    media->sccp_media.addr = htonl(0x0101010a); /* 10.1.1.1 */
    media->sccp_media.port = htonl(16384);      /* random port */

    sapp_psccp_cbs.openrcv_res(sapp_psccp_handle, GAPI_MSG_OPENRCV_RES,
                               conn_id, line, media, GAPI_CAUSE_OK);

    return (0);
}

static int sapp_closercv_cb (gapi_handle_t handle,
                             int msg_id, int conn_id,
                             int line, gapi_media_t *media)
{
    /*
     * Close the receive RTP port.
     */
    return (0);
}

static int sapp_startxmit_cb (gapi_handle_t handle,
                              int msg_id, int conn_id,
                              int line, gapi_media_t *media)
{
    /*
     * Begin sending RTP information to the address and port
     * specfified in the media parameter.
     */
    return (0);
}

static int sapp_stopxmit_cb (gapi_handle_t handle,
                             int msg_id, int conn_id,
                             int line, gapi_media_t *media)
{
    /*
     * Stop sending RTP information.
     */
    return (0);
}

/* 
 * main function for the PSCCP thread.
 */
static void sapp_psccp_main (void)
{
    while (1) { /* loop until ready to run */
        if (sapp_ready_to_run > 0) {
            sapp_psccp_cbs.sccp_main(sapp_psccp_handle, NULL);
        }
        sapp_ready_to_run--;
    }
}

/* 
 * main function for the TCP thread.
 */
static void sapp_ptcp_main (void)
{
    int size;
    int socket = 1;

    while (1) {
        /* Wait for new sockets events, such as opened or closed connections
         * and data received.
         */

        /*
         * Sample case of receiving data.
         */
//        size = recv(socket, sapp_tcp_buffer, 0);
        sapp_psccp_cbs.tcp_main(sapp_sccp_handle, 0,
                                socket,
                                GAPI_TCP_EVENT_RECV,
                                sapp_tcp_buffer, 
                                size);
    }
}

void sapp_init_api (void)
{
    ssapi_callbacks_t ssapi_cbs;
    gapi_callbacks_t  sapp_cbs;
    gapi_callbacks_t  *psccp_cbs = NULL;
    int               rc = 0;


    /*
     * Init software. 
     */
    sapp_state = SAPP_S_IDLE;
    sapp_ready_to_run = 0;
    sapp_conn_id = 0;
    memset(sapp_tcp_buffer, 0, sizeof(sapp_tcp_buffer));

    /*
     * Setup SSAPI callbacks.
     */
    memset(&ssapi_cbs, 0, sizeof(ssapi_cbs));

    ssapi_cbs.thread_get = sapp_thread_get_cb;
    ssapi_cbs.thread_run = sapp_thread_run_cb;
    /*
     * Set the rest of the SSAPI callbacks.
     */

    /*
     * Bind SSAPI callbacks.
     */
    rc = ssapi_bind(&ssapi_cbs);
    if (rc != 0) {
        /*
         * Error!
         */
        ;/* Do something. */
    }


    /*
     * Setup GAPI callbacks. 
     */
    memset(&sapp_cbs, 0, sizeof(sapp_cbs));
    memset(&sapp_psccp_cbs, 0, sizeof(sapp_psccp_cbs));
    
    sapp_cbs.opensession_res  = sapp_opensession_res_cb;
    sapp_cbs.sessionstatus    = sapp_sessionstatus_cb;
    sapp_cbs.resetsession_res = sapp_resetsession_res_cb;
    sapp_cbs.setup            = sapp_setup_cb;
    sapp_cbs.setup_ack        = sapp_setup_ack_cb;
    sapp_cbs.proceeding       = sapp_proceeding_cb;
    sapp_cbs.alerting         = sapp_alerting_cb;
    sapp_cbs.connect          = sapp_connect_cb;
    sapp_cbs.connect_ack      = sapp_connect_ack_cb;
    sapp_cbs.release          = sapp_release_cb;
    sapp_cbs.release_complete = sapp_release_complete_cb;
    sapp_cbs.openrcv_req      = sapp_openrcv_req_cb;
    sapp_cbs.closercv         = sapp_closercv_cb;
    sapp_cbs.startxmit        = sapp_startxmit_cb;
    sapp_cbs.stopxmit         = sapp_stopxmit_cb;

    /*
     * Bind GAPI callbacks.
     */
    rc = gapi_bind(&sapp_cbs, &psccp_cbs, sapp_sapp_handle2,
                   &sapp_psccp_handle, "SAPP");
    if (rc != 0) {
        /*
         * Error!
         */
        ; /* Do something. */
    }

    /* 
     * Copy GAPI callbacks.
     */
    sapp_psccp_cbs = *psccp_cbs;
}

void sapp_open_session (void)
{
    gapi_cmaddr_t cms[5];
    gapi_media_caps_t caps;

    /*
     * Set the CCM addresses.
     */
    memset(cms, 0, sizeof(*cms) * 5);
    
    cms[0].addr = htonl(0x0101010a); /* 10.1.1.1 */
    cms[0].port = htons(2000);       /* 2000 */

    /*
     * Set the media capabilities.
     */
    caps.count = 1;
    caps.caps[0].payload = GAPI_PAYLOAD_TYPE_G711_ULAW_64K;
    caps.caps[0].milliseconds_per_packet = 20;

    sapp_psccp_cbs.opensession_req(sapp_psccp_handle,
                                   GAPI_MSG_OPENSESSION_REQ,
                                   cms, "0123456789ab", NULL,
                                   GAPI_SRST_MODE_DISABLE, NULL,
                                   &caps, NULL,
                                   GAPI_PROTOCOL_VERSION_PARCHE);    
}

void sapp_reset_session (void)
{
    sapp_psccp_cbs.resetsession_req(sapp_psccp_handle,
                                    GAPI_MSG_RESETSESSION_REQ,
                                    GAPI_CAUSE_CM_RESET);
}

void sapp_outgoing_call (void)
{
    sapp_conn_id = gapi_get_new_conn_id();

    sapp_psccp_cbs.setup(sapp_psccp_handle, GAPI_MSG_SETUP,
                         sapp_conn_id, 1,
                         NULL, NULL, 0,
                         NULL, 0, 0, NULL);

    sapp_psccp_cbs.digits(sapp_psccp_handle, GAPI_MSG_DIGITS,
                          sapp_conn_id, 1,
                          "5551212", 7);
    
    /* 
     * The rest of the call setup with be completed through callbacks.
     * The procedding, alerting, media establishment and connect
     * callbacks will be received.
     */

    /*
     * sleep for a few seconds.
     */

    /*
     * User hangs up.
     */
    sapp_psccp_cbs.release(sapp_psccp_handle, GAPI_MSG_RELEASE,
                           sapp_conn_id, 1,
                           GAPI_CAUSE_OK, NULL);

    /*
     * release_complete callback will be called by the stack and
     * all resources related with the call will be freed.
     */
}


int main (int argc, char **argv)
{
    /*
     * Initialize the SAPP software.
     */
    sapp_init_api();

    /*
     * Create a PSCCP thread.
     */
//    sapp_psccp_thread_handle = platform_create_thread(sapp_sccp_main);

    /*
     * Create a TCP/IP thread.
     */
//    sapp_tcp_thread_handle = platform_create_thread(sapp_tcp_main);

    /*
     * Open a stack session.
     */
    sapp_open_session();

    return (0);
}

#else /* SAPP_DOCUMENTATION */

sapp_info_t *sapp_get_info (int id)
{
    if ((id >= 1) && (id <= SAPP_MAX_INFOS)) {
        return (&(sapp_infos[id - 1]));
    }
    else {
        return (NULL);
    }
}
static int sapp_info_init (sapp_info_t *sinfo)
{
//    int rc;
    static int count = 1;

    sinfo->sapp_state = SAPP_S_IDLE;
    sinfo->sapp_reset_cause = GAPI_CAUSE_OK;
    memset(sinfo->sapp_cminfo, 0, sizeof(sinfo->sapp_cminfo));

#if 0
    rc = platform_event_create(0, 0, &(sinfo->sapp_sccp_event));
    if (rc != 0) {
        return (1);
    }
#endif
    sinfo->sapp_sapp_handle = sinfo;
    sinfo->id = count++;

    return (0);
}

int sapp_init (void)
{
    char              *fname    = "sapp_init";
    int               gapi_rc   = 0;
//  gapi_callbacks_t  *sccp_cbs = NULL;
    gapi_callbacks_t  sapp_cbs;
    ssapi_callbacks_t ssapi_cbs;
    sapp_socket_t     *socket2;
    sapp_call_t       *call;
    int               rc;
    int               i = 0;
    sapp_info_t       *sinfo;

    printf("%s: %s: initializing\n", SAPP_ID, fname);

#ifdef SAPP_PLATFORM_WIN32
    sapp_sccp_msg_id = RegisterWindowMessage("SCCPMSG");
#endif

    printf("%s: %s: initializing: sockets\n", SAPP_ID, fname);
    rc = platform_mutex_create(0, &(sapp_sockets2.mutex));
    if (rc != 0) {
        return (1);
    }

    rc = platform_event_create(0, 0, &(sapp_sockets2.event));
    if (rc != 0) {
        return (1);
    }

    for (socket2 = &(sapp_sockets2.sockets[0]);
         socket2 < &(sapp_sockets2.sockets[SAPP_MAX_SOCKETS]);
         socket2++) {
        sapp_free_socket2(socket2);
    }

    rc = platform_thread_create(sapp_tcp_main,
                                &(sapp_infos[0]),
                                &(sapp_sockets2.thread));
    if (rc != 0) {
        return (1);
    }

#if 0
    printf("%s: %s: initializing: sccp\n", SAPP_ID, fname);
    rc = platform_mutex_create(0, &sapp_sccp_mutex);
    if (rc != 0) {
        return (1);
    }

    rc = platform_thread_create(sapp_sccp_main2,
                                &(sapp_infos[0]),
                                &sapp_sccp_thread);
    if (rc != 0) {
        return (1);
    }
#endif

    printf("%s: %s: initializing: calls\n", SAPP_ID, fname);
    rc = platform_mutex_create(0, &(sapp_calls.mutex));
    if (rc != 0) {
        return (1);
    }

    sapp_calls.count = 0;
    for (call = &(sapp_calls.calls[0]);
         call < &(sapp_calls.calls[SAPP_MAX_CALLS]);
         call++) {

        sapp_free_call(call);
    }
#if 0
    sapp_sccp_cbs = (gapi_callbacks_t *)malloc(sizeof(gapi_callbacks_t));
    if (sapp_sccp_cbs == NULL) {
         return (1);
    }
#endif
    memset(&sapp_cbs, 0, sizeof(sapp_cbs));
//  memset(sapp_sccp_cbs, 0, sizeof(*sapp_sccp_cbs));

    /*
     * Setup the sccp application callbacks.
     */
    sapp_cbs.setup                    = sapp_sapp_setup;
    sapp_cbs.offhook                  = sapp_sapp_offhook;
    sapp_cbs.setup_ack                = sapp_sapp_setup_ack;
    sapp_cbs.proceeding               = sapp_sapp_proceeding;
    sapp_cbs.alerting                 = sapp_sapp_alerting;
    sapp_cbs.connect                  = sapp_sapp_connect;
    sapp_cbs.connect_ack              = sapp_sapp_connect_ack;
    sapp_cbs.disconnect               = sapp_sapp_disconnect;
    sapp_cbs.release                  = sapp_sapp_release;
    sapp_cbs.release_complete         = sapp_sapp_release_complete;

    sapp_cbs.openrcv_req              = sapp_sapp_openrcv_req;
    sapp_cbs.closercv                 = sapp_sapp_closercv;
    sapp_cbs.startxmit                = sapp_sapp_startxmit;
    sapp_cbs.stopxmit                 = sapp_sapp_stopxmit;
    
    sapp_cbs.feature_req              = sapp_sapp_feature_req;
    sapp_cbs.feature_res              = sapp_sapp_feature_res;
    sapp_cbs.connectionstats          = sapp_sapp_connectionstats;

    sapp_cbs.starttone                = sapp_sapp_starttone;

    sapp_cbs.reset                    = sapp_sapp_reset;
    sapp_cbs.opensession_res          = sapp_sapp_opensession_res;
    sapp_cbs.sessionstatus            = sapp_sapp_sessionstatus;
    sapp_cbs.resetsession_res         = sapp_sapp_resetsession_res;
    
    sapp_cbs.conninfo                 = sapp_sapp_conninfo;

    sapp_cbs.linestat_res             = sapp_sapp_linestat_res;
    sapp_cbs.speeddialstat_res        = sapp_sapp_speeddialstat_res;
    sapp_cbs.featurestat_res          = sapp_sapp_featurestat_res;
    sapp_cbs.serviceurlstat_res       = sapp_sapp_serviceurlstat_res;

    sapp_cbs.softkeytemplate_res      = sapp_sapp_softkeytemplate_res;
    sapp_cbs.softkeyset_res           = sapp_sapp_softkeyset_res;

    sapp_cbs.passthru                 = sapp_sapp_passthru;

    sapp_cbs.all_streams_idle         = sapp_sapp_all_streams_idle;
    sapp_cbs.close_abandonded_streams = sapp_sapp_close_abandonded_streams;

    
    /*
     * Setup the system services callbacks.
     */
#ifdef SAPP_PLATFORM_79XXWIN
    ssapi_cbs.timer_initialize    = sapp_timer_initialize;
#else
    ssapi_cbs.timer_initialize    = (ssapi_timer_initialize_f *)timer_event_initialize;
#if 0
#ifdef SAPP_PLATFORM_WIN32
    ssapi_cbs.timer_initialize    = (ssapi_timer_initialize_f *)timer_event_initialize;
#endif
#ifdef SAPP_PLATFORM_UNIX
    ssapi_cbs.timer_initialize    = timer_event_initialize;
#endif
#endif
#endif
    ssapi_cbs.timer_allocate      = (ssapi_timer_allocate_f *)timer_event_allocate;
    ssapi_cbs.timer_activate      = (ssapi_timer_activate_f *)timer_event_activate;
    ssapi_cbs.timer_cancel        = (ssapi_timer_cancel_f *)timer_event_cancel;
    ssapi_cbs.timer_free          = (ssapi_timer_free_f *)timer_event_free;

    ssapi_cbs.malloc              = malloc;
    ssapi_cbs.free                = free;
    ssapi_cbs.memset              = memset;
    ssapi_cbs.memcpy              = memcpy;

    ssapi_cbs.htons               = sapp_ntohs;
    ssapi_cbs.htonl               = sapp_ntohl;
    ssapi_cbs.ntohs               = sapp_ntohs;
    ssapi_cbs.ntohl               = sapp_ntohl;

    ssapi_cbs.strtime             = platform_strtime;

    ssapi_cbs.mutex_create        = (ssapi_mutex_create_f *)platform_mutex_create;
    ssapi_cbs.mutex_lock          = (ssapi_mutex_lock_f *)platform_mutex_lock;
    ssapi_cbs.mutex_unlock        = (ssapi_mutex_unlock_f *)platform_mutex_unlock;
    ssapi_cbs.mutex_delete        = (ssapi_mutex_delete_f *)platform_mutex_delete;

    ssapi_cbs.thread_get          = sapp_sccp_thread_get;
    ssapi_cbs.thread_run          = sapp_sccp_thread_run;
    
    ssapi_cbs.socket_open         = sapp_socket_open;
    ssapi_cbs.socket_close        = sapp_socket_close;
    ssapi_cbs.socket_connect      = sapp_socket_connect;
    ssapi_cbs.socket_send         = sapp_socket_send;
    ssapi_cbs.socket_recv         = sapp_socket_recv;
    ssapi_cbs.socket_getlasterror = platform_get_last_error;
    ssapi_cbs.socket_getsockname  = sapp_socket_getsockname;
    ssapi_cbs.socket_getmac       = sapp_socket_getmac; 

    ssapi_cbs.printf              = printf;
    ssapi_cbs.snprintf            = snprintf;
    ssapi_cbs.vsnprintf           = vsnprintf;
    ssapi_cbs.strncpy             = strncpy;

    gapi_rc = ssapi_bind(&ssapi_cbs);
    if (gapi_rc != 0) {
        return (12);
    }

    rc = platform_event_create(0, 0, &sapp_sccp_event);
    if (rc != 0) {
        return (1);
    }

    for (sinfo = &(sapp_infos[0]); sinfo < &(sapp_infos[SAPP_MAX_INFOS]); sinfo++) {
        if (sapp_info_init(sinfo) != 0) {
            return (20);
        }

        //sapp_sccp_events[i++] = sinfo->sapp_sccp_event;

        gapi_rc = gapi_bind(&sapp_cbs, &(sinfo->sapp_sccp_cbs), sinfo->sapp_sapp_handle,
                            &(sinfo->sapp_sccp_handle), SAPP_NAME1);

        if (gapi_rc != 0) {
            return (21);
        }

        if (sinfo->sapp_sccp_handle == NULL) {
            return (22);
        }

        memset(&(sinfo->sapp_session_data), 0, sizeof(sinfo->sapp_session_data));    
    }

//    memcpy(sapp_sccp_cbs, sccp_cbs, sizeof(gapi_callbacks_t));

    printf("%s: %s: initializing: sccp\n", SAPP_ID, fname);
    rc = platform_mutex_create(0, &sapp_sccp_mutex);
    if (rc != 0) {
        return (1);
    }

    rc = platform_thread_create(sapp_sccp_main2,
                                &(sapp_infos[0]),
                                &sapp_sccp_thread);
    if (rc != 0) {
        return (1);
    }

#if 0
    sapp_sccp_cbs->sccp_cleanup(sapp_sccp_handle, NULL);
    memset(sapp_sccp_cbs, 0, sizeof(gapi_callbacks_t));

    gapi_rc = gapi_bind(&sapp_cbs, &sccp_cbs, sapp_sapp_handle,
                        &sapp_sccp_handle, SAPP_NAME);
    if (gapi_rc != 0) {
        return (21);
    }

    if (sapp_sccp_handle == NULL) {
        return (22);
    }

    memcpy(sapp_sccp_cbs, sccp_cbs, sizeof(gapi_callbacks_t));
#endif

    /*
     * Other global connection related info was initialized at compile-time.
     */

    printf("%s: %s: initialized\n", SAPP_ID, fname);

    //_CrtMemDumpAllObjectsSince(NULL);

    sapp_initialized = 1;

    return (0);
}
#endif
int sapp_cleanup (void)
{
    int rc;
    sapp_info_t *sinfo = &(sapp_infos[0]);

    /*
     * Kill sccp and tcp threads. Set the cleanup flag and singnal
     * the thread to run. The threads will read the flag and cleanup.
     */
    sapp_need_cleanup = 1;
    
    /*
     * Poke the sccp and tcp threads so they will know to cleanup.
     */
    //sapp_sccp_thread_run(sinfo->sapp_sccp_event);
    sapp_sccp_thread_run(sapp_sccp_event);
    sapp_tcp_thread_run(sapp_sockets2.event);

    /*
     * Wait for the SCCP thread to exit and then delete it. Repeat for
     * the TCP thread.
     */
#ifdef SAPP_PLATFORM_UNIX
    platform_thread_wait(sapp_sccp_thread, SAPP_SCCP_WAIT_TIMEOUT);
#endif
    rc = platform_thread_delete(sapp_sccp_thread);

#ifdef SAPP_PLATFORM_UNIX
    platform_thread_wait(sapp_sockets2.thread, SAPP_SCCP_WAIT_TIMEOUT);
#endif
    platform_thread_delete(sapp_sockets2.thread);

    if (sapp_sockets2.mutex != NULL) {
        platform_mutex_delete(sapp_sockets2.mutex);   
    }
    sapp_sockets2.mutex = NULL;

    if (sapp_sccp_mutex != NULL) {
        platform_mutex_delete(sapp_sccp_mutex);
    }
    sapp_sccp_mutex = NULL;


    for (sinfo = &(sapp_infos[0]); sinfo < &(sapp_infos[SAPP_MAX_INFOS]); sinfo++) {
        if (sinfo->sapp_sccp_cbs != NULL) {
            free(sinfo->sapp_sccp_cbs);
        }
        sinfo->sapp_sccp_cbs = NULL;
    }

    if (sapp_sccp_event != NULL) {
        platform_event_delete(sapp_sccp_event);   
    }
    sapp_sccp_event = NULL;

    if (sapp_sockets2.event != NULL) {
        platform_event_delete(sapp_sockets2.event);   
    }
    sapp_sockets2.event = NULL;

    return (0);
}

void sapp_set_flags (int flags)
{
    sapp_flags = flags;
}

int sapp_get_test_flags (void)
{
    return (sapp_flags);
}

#ifdef SAPP_USE_PORTABLE_SNPRINTF
/*
 * snprintf.c - a portable implementation of snprintf
 *
 * AUTHOR
 *   Mark Martinec <mark.martinec@ijs.si>, April 1999.
 *
 *   Copyright 1999, Mark Martinec. All rights reserved.
 *
 * TERMS AND CONDITIONS
 *   This program is free software; you can redistribute it and/or modify
 *   it under the terms of the "Frontier Artistic License" which comes
 *   with this Kit.
 *
 *   This program is distributed in the hope that it will be useful,
 *   but WITHOUT ANY WARRANTY; without even the implied warranty
 *   of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.
 *   See the Frontier Artistic License for more details.
 *
 *   You should have received a copy of the Frontier Artistic License
 *   with this Kit in the file named LICENSE.txt .
 *   If not, I'll be glad to provide one.
 *
 * FEATURES
 * - careful adherence to specs regarding flags, field width and precision;
 * - good performance for large string handling (large format, large
 *   argument or large paddings). Performance is similar to system's sprintf
 *   and in several cases significantly better (make sure you compile with
 *   optimizations turned on, tell the compiler the code is strict ANSI
 *   if necessary to give it more freedom for optimizations);
 * - return value semantics per ISO/IEC 9899:1999 ("ISO C99");
 * - written in standard ISO/ANSI C - requires an ANSI C compiler.
 *
 * SUPPORTED CONVERSION SPECIFIERS AND DATA TYPES
 *
 * This snprintf only supports the following conversion specifiers:
 * s, c, d, u, o, x, X, p  (and synonyms: i, D, U, O - see below)
 * with flags: '-', '+', ' ', '0' and '#'.
 * An asterisk is supported for field width as well as precision.
 *
 * Length modifiers 'h' (short int), 'l' (long int),
 * and 'll' (long long int) are supported.
 * NOTE:
 *   If macro SNPRINTF_LONGLONG_SUPPORT is not defined (default) the
 *   length modifier 'll' is recognized but treated the same as 'l',
 *   which may cause argument value truncation! Defining
 *   SNPRINTF_LONGLONG_SUPPORT requires that your system's sprintf also
 *   handles length modifier 'll'.  long long int is a language extension
 *   which may not be portable.
 *
 * Conversion of numeric data (conversion specifiers d, u, o, x, X, p)
 * with length modifiers (none or h, l, ll) is left to the system routine
 * sprintf, but all handling of flags, field width and precision as well as
 * c and s conversions is done very carefully by this portable routine.
 * If a string precision (truncation) is specified (e.g. %.8s) it is
 * guaranteed the string beyond the specified precision will not be referenced.
 *
 * Length modifiers h, l and ll are ignored for c and s conversions (data
 * types wint_t and wchar_t are not supported).
 *
 * The following common synonyms for conversion characters are supported:
 *   - i is a synonym for d
 *   - D is a synonym for ld, explicit length modifiers are ignored
 *   - U is a synonym for lu, explicit length modifiers are ignored
 *   - O is a synonym for lo, explicit length modifiers are ignored
 * The D, O and U conversion characters are nonstandard, they are supported
 * for backward compatibility only, and should not be used for new code.
 *
 * The following is specifically NOT supported:
 *   - flag ' (thousands' grouping character) is recognized but ignored
 *   - numeric conversion specifiers: f, e, E, g, G and synonym F,
 *     as well as the new a and A conversion specifiers
 *   - length modifier 'L' (long double) and 'q' (quad - use 'll' instead)
 *   - wide character/string conversions: lc, ls, and nonstandard
 *     synonyms C and S
 *   - writeback of converted string length: conversion character n
 *   - the n$ specification for direct reference to n-th argument
 *   - locales
 *
 * It is permitted for str_m to be zero, and it is permitted to specify NULL
 * pointer for resulting string argument if str_m is zero (as per ISO C99).
 *
 * The return value is the number of characters which would be generated
 * for the given input, excluding the trailing null. If this value
 * is greater or equal to str_m, not all characters from the result
 * have been stored in str, output bytes beyond the (str_m-1) -th character
 * are discarded. If str_m is greater than zero it is guaranteed
 * the resulting string will be null-terminated.
 *
 * NOTE that this matches the ISO C99, OpenBSD, and GNU C library 2.1,
 * but is different from some older and vendor implementations,
 * and is also different from XPG, XSH5, SUSv2 specifications.
 * For historical discussion on changes in the semantics and standards
 * of snprintf see printf(3) man page in the Linux programmers manual.
 *
 * Routines asprintf and vasprintf return a pointer (in the ptr argument)
 * to a buffer sufficiently large to hold the resulting string. This pointer
 * should be passed to free(3) to release the allocated storage when it is
 * no longer needed. If sufficient space cannot be allocated, these functions
 * will return -1 and set ptr to be a NULL pointer. These two routines are a
 * GNU C library extensions (glibc).
 *
 * Routines asnprintf and vasnprintf are similar to asprintf and vasprintf,
 * yet, like snprintf and vsnprintf counterparts, will write at most str_m-1
 * characters into the allocated output string, the last character in the
 * allocated buffer then gets the terminating null. If the formatted string
 * length (the return value) is greater than or equal to the str_m argument,
 * the resulting string was truncated and some of the formatted characters
 * were discarded. These routines present a handy way to limit the amount
 * of allocated memory to some sane value.
 *
 * AVAILABILITY
 *   http://www.ijs.si/software/snprintf/
 *
 * REVISION HISTORY
 * 1999-04	V0.9  Mark Martinec
 *		- initial version, some modifications after comparing printf
 *		  man pages for Digital Unix 4.0, Solaris 2.6 and HPUX 10,
 *		  and checking how Perl handles sprintf (differently!);
 * 1999-04-09	V1.0  Mark Martinec <mark.martinec@ijs.si>
 *		- added main test program, fixed remaining inconsistencies,
 *		  added optional (long long int) support;
 * 1999-04-12	V1.1  Mark Martinec <mark.martinec@ijs.si>
 *		- support the 'p' conversion (pointer to void);
 *		- if a string precision is specified
 *		  make sure the string beyond the specified precision
 *		  will not be referenced (e.g. by strlen);
 * 1999-04-13	V1.2  Mark Martinec <mark.martinec@ijs.si>
 *		- support synonyms %D=%ld, %U=%lu, %O=%lo;
 *		- speed up the case of long format string with few conversions;
 * 1999-06-30	V1.3  Mark Martinec <mark.martinec@ijs.si>
 *		- fixed runaway loop (eventually crashing when str_l wraps
 *		  beyond 2^31) while copying format string without
 *		  conversion specifiers to a buffer that is too short
 *		  (thanks to Edwin Young <edwiny@autonomy.com> for
 *		  spotting the problem);
 *		- added macros PORTABLE_SNPRINTF_VERSION_(MAJOR|MINOR)
 *		  to snprintf.h
 * 2000-02-14	V2.0 (never released) Mark Martinec <mark.martinec@ijs.si>
 *		- relaxed license terms: The Artistic License now applies.
 *		  You may still apply the GNU GENERAL PUBLIC LICENSE
 *		  as was distributed with previous versions, if you prefer;
 *		- changed REVISION HISTORY dates to use ISO 8601 date format;
 *		- added vsnprintf (patch also independently proposed by
 *		  Caolan McNamara 2000-05-04, and Keith M Willenson 2000-06-01)
 * 2000-06-27	V2.1  Mark Martinec <mark.martinec@ijs.si>
 *		- removed POSIX check for str_m<1; value 0 for str_m is
 *		  allowed by ISO C99 (and GNU C library 2.1) - (pointed out
 *		  on 2000-05-04 by Caolan McNamara, caolan@ csn dot ul dot ie).
 *		  Besides relaxed license this change in standards adherence
 *		  is the main reason to bump up the major version number;
 *		- added nonstandard routines asnprintf, vasnprintf, asprintf,
 *		  vasprintf that dynamically allocate storage for the
 *		  resulting string; these routines are not compiled by default,
 *		  see comments where NEED_V?ASN?PRINTF macros are defined;
 *		- autoconf contributed by Caolan McNamara
 * 2000-10-06	V2.2  Mark Martinec <mark.martinec@ijs.si>
 *		- BUG FIX: the %c conversion used a temporary variable
 *		  that was no longer in scope when referenced,
 *		  possibly causing incorrect resulting character;
 *		- BUG FIX: make precision and minimal field width unsigned
 *		  to handle huge values (2^31 <= n < 2^32) correctly;
 *		  also be more careful in the use of signed/unsigned/size_t
 *		  internal variables - probably more careful than many
 *		  vendor implementations, but there may still be a case
 *		  where huge values of str_m, precision or minimal field
 *		  could cause incorrect behaviour;
 *		- use separate variables for signed/unsigned arguments,
 *		  and for short/int, long, and long long argument lengths
 *		  to avoid possible incompatibilities on certain
 *		  computer architectures. Also use separate variable
 *		  arg_sign to hold sign of a numeric argument,
 *		  to make code more transparent;
 *		- some fiddling with zero padding and "0x" to make it
 *		  Linux compatible;
 *		- systematically use macros fast_memcpy and fast_memset
 *		  instead of case-by-case hand optimization; determine some
 *		  breakeven string lengths for different architectures;
 *		- terminology change: 'format' -> 'conversion specifier',
 *		  'C9x' -> 'ISO/IEC 9899:1999 ("ISO C99")',
 *		  'alternative form' -> 'alternate form',
 *		  'data type modifier' -> 'length modifier';
 *		- several comments rephrased and new ones added;
 *		- make compiler not complain about 'credits' defined but
 *		  not used;
 */


/* Define HAVE_SNPRINTF if your system already has snprintf and vsnprintf.
 *
 * If HAVE_SNPRINTF is defined this module will not produce code for
 * snprintf and vsnprintf, unless PREFER_PORTABLE_SNPRINTF is defined as well,
 * causing this portable version of snprintf to be called portable_snprintf
 * (and portable_vsnprintf).
 */
/* #define HAVE_SNPRINTF */

/* Define PREFER_PORTABLE_SNPRINTF if your system does have snprintf and
 * vsnprintf but you would prefer to use the portable routine(s) instead.
 * In this case the portable routine is declared as portable_snprintf
 * (and portable_vsnprintf) and a macro 'snprintf' (and 'vsnprintf')
 * is defined to expand to 'portable_v?snprintf' - see file snprintf.h .
 * Defining this macro is only useful if HAVE_SNPRINTF is also defined,
 * but does does no harm if defined nevertheless.
 */
/* #define PREFER_PORTABLE_SNPRINTF */

/* Define SNPRINTF_LONGLONG_SUPPORT if you want to support
 * data type (long long int) and length modifier 'll' (e.g. %lld).
 * If undefined, 'll' is recognized but treated as a single 'l'.
 *
 * If the system's sprintf does not handle 'll'
 * the SNPRINTF_LONGLONG_SUPPORT must not be defined!
 *
 * This is off by default as (long long int) is a language extension.
 */
/* #define SNPRINTF_LONGLONG_SUPPORT */

/* Define NEED_SNPRINTF_ONLY if you only need snprintf, and not vsnprintf.
 * If NEED_SNPRINTF_ONLY is defined, the snprintf will be defined directly,
 * otherwise both snprintf and vsnprintf routines will be defined
 * and snprintf will be a simple wrapper around vsnprintf, at the expense
 * of an extra procedure call.
 */
/* #define NEED_SNPRINTF_ONLY */

/* Define NEED_V?ASN?PRINTF macros if you need library extension
 * routines asprintf, vasprintf, asnprintf, vasnprintf respectively,
 * and your system library does not provide them. They are all small
 * wrapper routines around portable_vsnprintf. Defining any of the four
 * NEED_V?ASN?PRINTF macros automatically turns off NEED_SNPRINTF_ONLY
 * and turns on PREFER_PORTABLE_SNPRINTF.
 *
 * Watch for name conflicts with the system library if these routines
 * are already present there.
 *
 * NOTE: vasprintf and vasnprintf routines need va_copy() from stdarg.h, as
 * specified by C99, to be able to traverse the same list of arguments twice.
 * I don't know of any other standard and portable way of achieving the same.
 * With some versions of gcc you may use __va_copy(). You might even get away
 * with "ap2 = ap", in this case you must not call va_end(ap2) !
 *   #define va_copy(ap2,ap) ap2 = ap
 */
/* #define NEED_ASPRINTF   */
/* #define NEED_ASNPRINTF  */
/* #define NEED_VASPRINTF  */
/* #define NEED_VASNPRINTF */


/* Define the following macros if desired:
 *   SOLARIS_COMPATIBLE, SOLARIS_BUG_COMPATIBLE,
 *   HPUX_COMPATIBLE, HPUX_BUG_COMPATIBLE, LINUX_COMPATIBLE,
 *   DIGITAL_UNIX_COMPATIBLE, DIGITAL_UNIX_BUG_COMPATIBLE,
 *   PERL_COMPATIBLE, PERL_BUG_COMPATIBLE,
 *
 * - For portable applications it is best not to rely on peculiarities
 *   of a given implementation so it may be best not to define any
 *   of the macros that select compatibility and to avoid features
 *   that vary among the systems.
 *
 * - Selecting compatibility with more than one operating system
 *   is not strictly forbidden but is not recommended.
 *
 * - 'x'_BUG_COMPATIBLE implies 'x'_COMPATIBLE .
 *
 * - 'x'_COMPATIBLE refers to (and enables) a behaviour that is
 *   documented in a sprintf man page on a given operating system
 *   and actually adhered to by the system's sprintf (but not on
 *   most other operating systems). It may also refer to and enable
 *   a behaviour that is declared 'undefined' or 'implementation specific'
 *   in the man page but a given implementation behaves predictably
 *   in a certain way.
 *
 * - 'x'_BUG_COMPATIBLE refers to (and enables) a behaviour of system's sprintf
 *   that contradicts the sprintf man page on the same operating system.
 *
 * - I do not claim that the 'x'_COMPATIBLE and 'x'_BUG_COMPATIBLE
 *   conditionals take into account all idiosyncrasies of a particular
 *   implementation, there may be other incompatibilities.
 */


/* ============================================= */
/* NO USER SERVICABLE PARTS FOLLOWING THIS POINT */
/* ============================================= */

#define PORTABLE_SNPRINTF_VERSION_MAJOR 2
#define PORTABLE_SNPRINTF_VERSION_MINOR 2

#if defined(NEED_ASPRINTF) || defined(NEED_ASNPRINTF) || defined(NEED_VASPRINTF) || defined(NEED_VASNPRINTF)
# if defined(NEED_SNPRINTF_ONLY)
# undef NEED_SNPRINTF_ONLY
# endif
# if !defined(PREFER_PORTABLE_SNPRINTF)
# define PREFER_PORTABLE_SNPRINTF
# endif
#endif

#if defined(SOLARIS_BUG_COMPATIBLE) && !defined(SOLARIS_COMPATIBLE)
#define SOLARIS_COMPATIBLE
#endif

#if defined(HPUX_BUG_COMPATIBLE) && !defined(HPUX_COMPATIBLE)
#define HPUX_COMPATIBLE
#endif

#if defined(DIGITAL_UNIX_BUG_COMPATIBLE) && !defined(DIGITAL_UNIX_COMPATIBLE)
#define DIGITAL_UNIX_COMPATIBLE
#endif

#if defined(PERL_BUG_COMPATIBLE) && !defined(PERL_COMPATIBLE)
#define PERL_COMPATIBLE
#endif

#if defined(LINUX_BUG_COMPATIBLE) && !defined(LINUX_COMPATIBLE)
#define LINUX_COMPATIBLE
#endif

#include <sys/types.h>
#include <string.h>
#include <stdlib.h>
#include <stdio.h>
#include <stdarg.h>
#include <assert.h>
#include <errno.h>

#ifdef isdigit
#undef isdigit
#endif
#define isdigit(c) ((c) >= '0' && (c) <= '9')

/* For copying strings longer or equal to 'breakeven_point'
 * it is more efficient to call memcpy() than to do it inline.
 * The value depends mostly on the processor architecture,
 * but also on the compiler and its optimization capabilities.
 * The value is not critical, some small value greater than zero
 * will be just fine if you don't care to squeeze every drop
 * of performance out of the code.
 *
 * Small values favor memcpy, large values favor inline code.
 */
#if defined(__alpha__) || defined(__alpha)
#  define breakeven_point   2	/* AXP (DEC Alpha)     - gcc or cc or egcs */
#endif
#if defined(__i386__)  || defined(__i386)
#  define breakeven_point  12	/* Intel Pentium/Linux - gcc 2.96 */
#endif
#if defined(__hppa)
#  define breakeven_point  10	/* HP-PA               - gcc */
#endif
#if defined(__sparc__) || defined(__sparc)
#  define breakeven_point  33	/* Sun Sparc 5         - gcc 2.8.1 */
#endif

/* some other values of possible interest: */
/* #define breakeven_point  8 */  /* VAX 4000          - vaxc */
/* #define breakeven_point 19 */  /* VAX 4000          - gcc 2.7.0 */

#ifndef breakeven_point
#  define breakeven_point   6	/* some reasonable one-size-fits-all value */
#endif

#if 0
#define fast_memcpy(d,s,n) \
  { register size_t nn = (size_t)(n); \
    if (nn >= breakeven_point) memcpy((d), (s), nn); \
    else if (nn > 0) { /* proc call overhead is worth only for large strings*/\
      register char *dd; register const char *ss; \
      for (ss=(s), dd=(d); nn>0; nn--) *dd++ = *ss++; } }

#define fast_memset(d,c,n) \
  { register size_t nn = (size_t)(n); \
    if (nn >= breakeven_point) memset((d), (int)(c), nn); \
    else if (nn > 0) { /* proc call overhead is worth only for large strings*/\
      register char *dd; register const int cc=(int)(c); \
      for (dd=(d); nn>0; nn--) *dd++ = cc; } }
#else
#define fast_memcpy(d,s,n) ssapi_cbs->memcpy(d, s, n)
#define fast_memset(d,c,n) ssapi_cbs->memset(d, c, n)
#endif
/* prototypes */

#if defined(NEED_ASPRINTF)
int asprintf   (char **ptr, const char *fmt, /*args*/ ...);
#endif
#if defined(NEED_VASPRINTF)
int vasprintf  (char **ptr, const char *fmt, va_list ap);
#endif
#if defined(NEED_ASNPRINTF)
int asnprintf  (char **ptr, size_t str_m, const char *fmt, /*args*/ ...);
#endif
#if defined(NEED_VASNPRINTF)
int vasnprintf (char **ptr, size_t str_m, const char *fmt, va_list ap);
#endif

#if defined(HAVE_SNPRINTF)
/* declare our portable snprintf  routine under name portable_snprintf  */
/* declare our portable vsnprintf routine under name portable_vsnprintf */
#else
/* declare our portable routines under names snprintf and vsnprintf */
#define portable_snprintf snprintf
#if !defined(NEED_SNPRINTF_ONLY)
#define portable_vsnprintf vsnprintf
#endif
#endif

#if !defined(HAVE_SNPRINTF) || defined(PREFER_PORTABLE_SNPRINTF)
int portable_snprintf(char *str, size_t str_m, const char *fmt, /*args*/ ...);
#if !defined(NEED_SNPRINTF_ONLY)
int portable_vsnprintf(char *str, size_t str_m, const char *fmt, va_list ap);
#endif
#endif

/* declarations */

static char credits[] = "";
#if 0
"\n\
@(#)snprintf.c, v2.2: Mark Martinec, <mark.martinec@ijs.si>\n\
@(#)snprintf.c, v2.2: Copyright 1999, Mark Martinec. Frontier Artistic License applies.\n\
@(#)snprintf.c, v2.2: http://www.ijs.si/software/snprintf/\n";
#endif
#if defined(NEED_ASPRINTF)
int asprintf(char **ptr, const char *fmt, /*args*/ ...) {
  va_list ap;
  size_t str_m;
  int str_l;

  *ptr = NULL;
  va_start(ap, fmt);                            /* measure the required size */
  str_l = portable_vsnprintf(NULL, (size_t)0, fmt, ap);
  va_end(ap);
  assert(str_l >= 0);        /* possible integer overflow if str_m > INT_MAX */
  *ptr = (char *) malloc(str_m = (size_t)str_l + 1);
  if (*ptr == NULL) { errno = ENOMEM; str_l = -1; }
  else {
    int str_l2;
    va_start(ap, fmt);
    str_l2 = portable_vsnprintf(*ptr, str_m, fmt, ap);
    va_end(ap);
    assert(str_l2 == str_l);
  }
  return str_l;
}
#endif

#if defined(NEED_VASPRINTF)
int vasprintf(char **ptr, const char *fmt, va_list ap) {
  size_t str_m;
  int str_l;

  *ptr = NULL;
  { va_list ap2;
    va_copy(ap2, ap);  /* don't consume the original ap, we'll need it again */
    str_l = portable_vsnprintf(NULL, (size_t)0, fmt, ap2);/*get required size*/
    va_end(ap2);
  }
  assert(str_l >= 0);        /* possible integer overflow if str_m > INT_MAX */
  *ptr = (char *) malloc(str_m = (size_t)str_l + 1);
  if (*ptr == NULL) { errno = ENOMEM; str_l = -1; }
  else {
    int str_l2 = portable_vsnprintf(*ptr, str_m, fmt, ap);
    assert(str_l2 == str_l);
  }
  return str_l;
}
#endif

#if defined(NEED_ASNPRINTF)
int asnprintf (char **ptr, size_t str_m, const char *fmt, /*args*/ ...) {
  va_list ap;
  int str_l;

  *ptr = NULL;
  va_start(ap, fmt);                            /* measure the required size */
  str_l = portable_vsnprintf(NULL, (size_t)0, fmt, ap);
  va_end(ap);
  assert(str_l >= 0);        /* possible integer overflow if str_m > INT_MAX */
  if ((size_t)str_l + 1 < str_m) str_m = (size_t)str_l + 1;      /* truncate */
  /* if str_m is 0, no buffer is allocated, just set *ptr to NULL */
  if (str_m == 0) {  /* not interested in resulting string, just return size */
  } else {
    *ptr = (char *) malloc(str_m);
    if (*ptr == NULL) { errno = ENOMEM; str_l = -1; }
    else {
      int str_l2;
      va_start(ap, fmt);
      str_l2 = portable_vsnprintf(*ptr, str_m, fmt, ap);
      va_end(ap);
      assert(str_l2 == str_l);
    }
  }
  return str_l;
}
#endif

#if defined(NEED_VASNPRINTF)
int vasnprintf (char **ptr, size_t str_m, const char *fmt, va_list ap) {
  int str_l;

  *ptr = NULL;
  { va_list ap2;
    va_copy(ap2, ap);  /* don't consume the original ap, we'll need it again */
    str_l = portable_vsnprintf(NULL, (size_t)0, fmt, ap2);/*get required size*/
    va_end(ap2);
  }
  assert(str_l >= 0);        /* possible integer overflow if str_m > INT_MAX */
  if ((size_t)str_l + 1 < str_m) str_m = (size_t)str_l + 1;      /* truncate */
  /* if str_m is 0, no buffer is allocated, just set *ptr to NULL */
  if (str_m == 0) {  /* not interested in resulting string, just return size */
  } else {
    *ptr = (char *) malloc(str_m);
    if (*ptr == NULL) { errno = ENOMEM; str_l = -1; }
    else {
      int str_l2 = portable_vsnprintf(*ptr, str_m, fmt, ap);
      assert(str_l2 == str_l);
    }
  }
  return str_l;
}
#endif

/*
 * If the system does have snprintf and the portable routine is not
 * specifically required, this module produces no code for snprintf/vsnprintf.
 */
#if !defined(HAVE_SNPRINTF) || defined(PREFER_PORTABLE_SNPRINTF)

#if !defined(NEED_SNPRINTF_ONLY)
int portable_snprintf(char *str, size_t str_m, const char *fmt, /*args*/ ...) {
  va_list ap;
  int str_l;

  va_start(ap, fmt);
  str_l = portable_vsnprintf(str, str_m, fmt, ap);
  va_end(ap);
  return str_l;
}
#endif

#if defined(NEED_SNPRINTF_ONLY)
int portable_snprintf(char *str, size_t str_m, const char *fmt, /*args*/ ...) {
#else
int portable_vsnprintf(char *str, size_t str_m, const char *fmt, va_list ap) {
#endif

#if defined(NEED_SNPRINTF_ONLY)
  va_list ap;
#endif
  size_t str_l = 0;
  const char *p = fmt;

/* In contrast with POSIX, the ISO C99 now says
 * that str can be NULL and str_m can be 0.
 * This is more useful than the old:  if (str_m < 1) return -1; */

#if defined(NEED_SNPRINTF_ONLY)
  va_start(ap, fmt);
#endif
  if (!p) p = "";
  while (*p) {
    if (*p != '%') {
   /* if (str_l < str_m) str[str_l++] = *p++;    -- this would be sufficient */
   /* but the following code achieves better performance for cases
    * where format string is long and contains few conversions */
      const char *q = strchr(p+1,'%');
      size_t n = !q ? strlen(p) : (q-p);
      if (str_l < str_m) {
        size_t avail = str_m-str_l;
        fast_memcpy(str+str_l, p, (n>avail?avail:n));
      }
      p += n; str_l += n;
    } else {
      const char *starting_p;
      size_t min_field_width = 0, precision = 0;
      int zero_padding = 0, precision_specified = 0, justify_left = 0;
      int alternate_form = 0, force_sign = 0;
      int space_for_positive = 1; /* If both the ' ' and '+' flags appear,
                                     the ' ' flag should be ignored. */
      char length_modifier = '\0';            /* allowed values: \0, h, l, L */
      char tmp[32];/* temporary buffer for simple numeric->string conversion */

      const char *str_arg;      /* string address in case of string argument */
      size_t str_arg_l;         /* natural field width of arg without padding
                                   and sign */
      unsigned char uchar_arg;
        /* unsigned char argument value - only defined for c conversion.
           N.B. standard explicitly states the char argument for
           the c conversion is unsigned */

      size_t number_of_zeros_to_pad = 0;
        /* number of zeros to be inserted for numeric conversions
           as required by the precision or minimal field width */

      size_t zero_padding_insertion_ind = 0;
        /* index into tmp where zero padding is to be inserted */

      char fmt_spec = '\0';
        /* current conversion specifier character */

      str_arg = credits;/* just to make compiler happy (defined but not used)*/
      str_arg = NULL;
      starting_p = p; p++;  /* skip '%' */
   /* parse flags */
      while (*p == '0' || *p == '-' || *p == '+' ||
             *p == ' ' || *p == '#' || *p == '\'') {
        switch (*p) {
        case '0': zero_padding = 1; break;
        case '-': justify_left = 1; break;
        case '+': force_sign = 1; space_for_positive = 0; break;
        case ' ': force_sign = 1;
     /* If both the ' ' and '+' flags appear, the ' ' flag should be ignored */
#ifdef PERL_COMPATIBLE
     /* ... but in Perl the last of ' ' and '+' applies */
                  space_for_positive = 1;
#endif
                  break;
        case '#': alternate_form = 1; break;
        case '\'': break;
        }
        p++;
      }
   /* If the '0' and '-' flags both appear, the '0' flag should be ignored. */

   /* parse field width */
      if (*p == '*') {
        int j;
        p++; j = va_arg(ap, int);
        if (j >= 0) min_field_width = j;
        else { min_field_width = -j; justify_left = 1; }
      } else if (isdigit((int)(*p))) {
        /* size_t could be wider than unsigned int;
           make sure we treat argument like common implementations do */
        unsigned int uj = *p++ - '0';
        while (isdigit((int)(*p))) uj = 10*uj + (unsigned int)(*p++ - '0');
        min_field_width = uj;
      }
   /* parse precision */
      if (*p == '.') {
        p++; precision_specified = 1;
        if (*p == '*') {
          int j = va_arg(ap, int);
          p++;
          if (j >= 0) precision = j;
          else {
            precision_specified = 0; precision = 0;
         /* NOTE:
          *   Solaris 2.6 man page claims that in this case the precision
          *   should be set to 0.  Digital Unix 4.0, HPUX 10 and BSD man page
          *   claim that this case should be treated as unspecified precision,
          *   which is what we do here.
          */
          }
        } else if (isdigit((int)(*p))) {
          /* size_t could be wider than unsigned int;
             make sure we treat argument like common implementations do */
          unsigned int uj = *p++ - '0';
          while (isdigit((int)(*p))) uj = 10*uj + (unsigned int)(*p++ - '0');
          precision = uj;
        }
      }
   /* parse 'h', 'l' and 'll' length modifiers */
      if (*p == 'h' || *p == 'l') {
        length_modifier = *p; p++;
        if (length_modifier == 'l' && *p == 'l') {   /* double l = long long */
#ifdef SNPRINTF_LONGLONG_SUPPORT
          length_modifier = '2';                  /* double l encoded as '2' */
#else
          length_modifier = 'l';                 /* treat it as a single 'l' */
#endif
          p++;
        }
      }
      fmt_spec = *p;
   /* common synonyms: */
      switch (fmt_spec) {
      case 'i': fmt_spec = 'd'; break;
      case 'D': fmt_spec = 'd'; length_modifier = 'l'; break;
      case 'U': fmt_spec = 'u'; length_modifier = 'l'; break;
      case 'O': fmt_spec = 'o'; length_modifier = 'l'; break;
      default: break;
      }
   /* get parameter value, do initial processing */
      switch (fmt_spec) {
      case '%': /* % behaves similar to 's' regarding flags and field widths */
      case 'c': /* c behaves similar to 's' regarding flags and field widths */
      case 's':
        length_modifier = '\0';          /* wint_t and wchar_t not supported */
     /* the result of zero padding flag with non-numeric conversion specifier*/
     /* is undefined. Solaris and HPUX 10 does zero padding in this case,    */
     /* Digital Unix and Linux does not. */
#if !defined(SOLARIS_COMPATIBLE) && !defined(HPUX_COMPATIBLE)
        zero_padding = 0;    /* turn zero padding off for string conversions */
#endif
        str_arg_l = 1;
        switch (fmt_spec) {
        case '%':
          str_arg = p; break;
        case 'c': {
          int j = va_arg(ap, int);
          uchar_arg = (unsigned char) j;   /* standard demands unsigned char */
          str_arg = (const char *) &uchar_arg;
          break;
        }
        case 's':
          str_arg = va_arg(ap, const char *);
          if (!str_arg) str_arg_l = 0;
       /* make sure not to address string beyond the specified precision !!! */
          else if (!precision_specified) str_arg_l = strlen(str_arg);
       /* truncate string if necessary as requested by precision */
          else if (precision == 0) str_arg_l = 0;
          else {
       /* memchr on HP does not like n > 2^31  !!! */
            const char *q = memchr(str_arg, '\0',
                             precision <= 0x7fffffff ? precision : 0x7fffffff);
            str_arg_l = !q ? precision : (q-str_arg);
          }
          break;
        default: break;
        }
        break;
      case 'd': case 'u': case 'o': case 'x': case 'X': case 'p': {
        /* NOTE: the u, o, x, X and p conversion specifiers imply
                 the value is unsigned;  d implies a signed value */

        int arg_sign = 0;
          /* 0 if numeric argument is zero (or if pointer is NULL for 'p'),
            +1 if greater than zero (or nonzero for unsigned arguments),
            -1 if negative (unsigned argument is never negative) */

        int int_arg = 0;  unsigned int uint_arg = 0;
          /* only defined for length modifier h, or for no length modifiers */

        long int long_arg = 0;  unsigned long int ulong_arg = 0;
          /* only defined for length modifier l */

        void *ptr_arg = NULL;
          /* pointer argument value -only defined for p conversion */

#ifdef SNPRINTF_LONGLONG_SUPPORT
        long long int long_long_arg = 0;
        unsigned long long int ulong_long_arg = 0;
          /* only defined for length modifier ll */
#endif
        if (fmt_spec == 'p') {
        /* HPUX 10: An l, h, ll or L before any other conversion character
         *   (other than d, i, u, o, x, or X) is ignored.
         * Digital Unix:
         *   not specified, but seems to behave as HPUX does.
         * Solaris: If an h, l, or L appears before any other conversion
         *   specifier (other than d, i, u, o, x, or X), the behavior
         *   is undefined. (Actually %hp converts only 16-bits of address
         *   and %llp treats address as 64-bit data which is incompatible
         *   with (void *) argument on a 32-bit system).
         */
#ifdef SOLARIS_COMPATIBLE
#  ifdef SOLARIS_BUG_COMPATIBLE
          /* keep length modifiers even if it represents 'll' */
#  else
          if (length_modifier == '2') length_modifier = '\0';
#  endif
#else
          length_modifier = '\0';
#endif
          ptr_arg = va_arg(ap, void *);
          if (ptr_arg != NULL) arg_sign = 1;
        } else if (fmt_spec == 'd') {  /* signed */
          switch (length_modifier) {
          case '\0':
          case 'h':
         /* It is non-portable to specify a second argument of char or short
          * to va_arg, because arguments seen by the called function
          * are not char or short.  C converts char and short arguments
          * to int before passing them to a function.
          */
            int_arg = va_arg(ap, int);
            if      (int_arg > 0) arg_sign =  1;
            else if (int_arg < 0) arg_sign = -1;
            break;
          case 'l':
            long_arg = va_arg(ap, long int);
            if      (long_arg > 0) arg_sign =  1;
            else if (long_arg < 0) arg_sign = -1;
            break;
#ifdef SNPRINTF_LONGLONG_SUPPORT
          case '2':
            long_long_arg = va_arg(ap, long long int);
            if      (long_long_arg > 0) arg_sign =  1;
            else if (long_long_arg < 0) arg_sign = -1;
            break;
#endif
          }
        } else {  /* unsigned */
          switch (length_modifier) {
          case '\0':
          case 'h':
            uint_arg = va_arg(ap, unsigned int);
            if (uint_arg) arg_sign = 1;
            break;
          case 'l':
            ulong_arg = va_arg(ap, unsigned long int);
            if (ulong_arg) arg_sign = 1;
            break;
#ifdef SNPRINTF_LONGLONG_SUPPORT
          case '2':
            ulong_long_arg = va_arg(ap, unsigned long long int);
            if (ulong_long_arg) arg_sign = 1;
            break;
#endif
          }
        }
        str_arg = tmp; str_arg_l = 0;
     /* NOTE:
      *   For d, i, u, o, x, and X conversions, if precision is specified,
      *   the '0' flag should be ignored. This is so with Solaris 2.6,
      *   Digital UNIX 4.0, HPUX 10, Linux, FreeBSD, NetBSD; but not with Perl.
      */
#ifndef PERL_COMPATIBLE
        if (precision_specified) zero_padding = 0;
#endif
        if (fmt_spec == 'd') {
          if (force_sign && arg_sign >= 0)
            tmp[str_arg_l++] = space_for_positive ? ' ' : '+';
         /* leave negative numbers for sprintf to handle,
            to avoid handling tricky cases like (short int)(-32768) */
#ifdef LINUX_COMPATIBLE
        } else if (fmt_spec == 'p' && force_sign && arg_sign > 0) {
          tmp[str_arg_l++] = space_for_positive ? ' ' : '+';
#endif
        } else if (alternate_form) {
          if (arg_sign != 0 && (fmt_spec == 'x' || fmt_spec == 'X') )
            { tmp[str_arg_l++] = '0'; tmp[str_arg_l++] = fmt_spec; }
         /* alternate form should have no effect for p conversion, but ... */
#ifdef HPUX_COMPATIBLE
          else if (fmt_spec == 'p'
         /* HPUX 10: for an alternate form of p conversion,
          *          a nonzero result is prefixed by 0x. */
#ifndef HPUX_BUG_COMPATIBLE
         /* Actually it uses 0x prefix even for a zero value. */
                   && arg_sign != 0
#endif
                  ) { tmp[str_arg_l++] = '0'; tmp[str_arg_l++] = 'x'; }
#endif
        }
        zero_padding_insertion_ind = str_arg_l;
        if (!precision_specified) precision = 1;   /* default precision is 1 */
        if (precision == 0 && arg_sign == 0
#if defined(HPUX_BUG_COMPATIBLE) || defined(LINUX_COMPATIBLE)
            && fmt_spec != 'p'
         /* HPUX 10 man page claims: With conversion character p the result of
          * converting a zero value with a precision of zero is a null string.
          * Actually HP returns all zeroes, and Linux returns "(nil)". */
#endif
        ) {
         /* converted to null string */
         /* When zero value is formatted with an explicit precision 0,
            the resulting formatted string is empty (d, i, u, o, x, X, p).   */
        } else {
          char f[5]; int f_l = 0;
          f[f_l++] = '%';    /* construct a simple format string for sprintf */
          if (!length_modifier) { }
          else if (length_modifier=='2') { f[f_l++] = 'l'; f[f_l++] = 'l'; }
          else f[f_l++] = length_modifier;
          f[f_l++] = fmt_spec; f[f_l++] = '\0';
          if (fmt_spec == 'p') str_arg_l += sprintf(tmp+str_arg_l, f, ptr_arg);
          else if (fmt_spec == 'd') {  /* signed */
            switch (length_modifier) {
            case '\0':
            case 'h': str_arg_l+=sprintf(tmp+str_arg_l, f, int_arg);  break;
            case 'l': str_arg_l+=sprintf(tmp+str_arg_l, f, long_arg); break;
#ifdef SNPRINTF_LONGLONG_SUPPORT
            case '2': str_arg_l+=sprintf(tmp+str_arg_l,f,long_long_arg); break;
#endif
            }
          } else {  /* unsigned */
            switch (length_modifier) {
            case '\0':
            case 'h': str_arg_l+=sprintf(tmp+str_arg_l, f, uint_arg);  break;
            case 'l': str_arg_l+=sprintf(tmp+str_arg_l, f, ulong_arg); break;
#ifdef SNPRINTF_LONGLONG_SUPPORT
            case '2': str_arg_l+=sprintf(tmp+str_arg_l,f,ulong_long_arg);break;
#endif
            }
          }
         /* include the optional minus sign and possible "0x"
            in the region before the zero padding insertion point */
          if (zero_padding_insertion_ind < str_arg_l &&
              tmp[zero_padding_insertion_ind] == '-') {
            zero_padding_insertion_ind++;
          }
          if (zero_padding_insertion_ind+1 < str_arg_l &&
              tmp[zero_padding_insertion_ind]   == '0' &&
             (tmp[zero_padding_insertion_ind+1] == 'x' ||
              tmp[zero_padding_insertion_ind+1] == 'X') ) {
            zero_padding_insertion_ind += 2;
          }
        }
        { size_t num_of_digits = str_arg_l - zero_padding_insertion_ind;
          if (alternate_form && fmt_spec == 'o'
#ifdef HPUX_COMPATIBLE                                  /* ("%#.o",0) -> ""  */
              && (str_arg_l > 0)
#endif
#ifdef DIGITAL_UNIX_BUG_COMPATIBLE                      /* ("%#o",0) -> "00" */
#else
              /* unless zero is already the first character */
              && !(zero_padding_insertion_ind < str_arg_l
                   && tmp[zero_padding_insertion_ind] == '0')
#endif
          ) {        /* assure leading zero for alternate-form octal numbers */
            if (!precision_specified || precision < num_of_digits+1) {
             /* precision is increased to force the first character to be zero,
                except if a zero value is formatted with an explicit precision
                of zero */
              precision = num_of_digits+1; precision_specified = 1;
            }
          }
       /* zero padding to specified precision? */
          if (num_of_digits < precision) 
            number_of_zeros_to_pad = precision - num_of_digits;
        }
     /* zero padding to specified minimal field width? */
        if (!justify_left && zero_padding) {
          int n = min_field_width - (str_arg_l+number_of_zeros_to_pad);
          if (n > 0) number_of_zeros_to_pad += n;
        }
        break;
      }
      default: /* unrecognized conversion specifier, keep format string as-is*/
        zero_padding = 0;  /* turn zero padding off for non-numeric convers. */
#ifndef DIGITAL_UNIX_COMPATIBLE
        justify_left = 1; min_field_width = 0;                /* reset flags */
#endif
#if defined(PERL_COMPATIBLE) || defined(LINUX_COMPATIBLE)
     /* keep the entire format string unchanged */
        str_arg = starting_p; str_arg_l = p - starting_p;
     /* well, not exactly so for Linux, which does something inbetween,
      * and I don't feel an urge to imitate it: "%+++++hy" -> "%+y"  */
#else
     /* discard the unrecognized conversion, just keep *
      * the unrecognized conversion character          */
        str_arg = p; str_arg_l = 0;
#endif
        if (*p) str_arg_l++;  /* include invalid conversion specifier unchanged
                                 if not at end-of-string */
        break;
      }
      if (*p) p++;      /* step over the just processed conversion specifier */
   /* insert padding to the left as requested by min_field_width;
      this does not include the zero padding in case of numerical conversions*/
      if (!justify_left) {                /* left padding with blank or zero */
        int n = min_field_width - (str_arg_l+number_of_zeros_to_pad);
        if (n > 0) {
          if (str_l < str_m) {
            size_t avail = str_m-str_l;
            fast_memset(str+str_l, (zero_padding?'0':' '), (n>(int)avail?avail:n));
          }
          str_l += n;
        }
      }
   /* zero padding as requested by the precision or by the minimal field width
    * for numeric conversions required? */
      if (number_of_zeros_to_pad <= 0) {
     /* will not copy first part of numeric right now, *
      * force it to be copied later in its entirety    */
        zero_padding_insertion_ind = 0;
      } else {
     /* insert first part of numerics (sign or '0x') before zero padding */
        int n = zero_padding_insertion_ind;
        if (n > 0) {
          if (str_l < str_m) {
            size_t avail = str_m-str_l;
            fast_memcpy(str+str_l, str_arg, (n>(int)avail?avail:n));
          }
          str_l += n;
        }
     /* insert zero padding as requested by the precision or min field width */
        n = number_of_zeros_to_pad;
        if (n > 0) {
          if (str_l < str_m) {
            size_t avail = str_m-str_l;
            fast_memset(str+str_l, '0', (n>(int)avail?avail:n));
          }
          str_l += n;
        }
      }
   /* insert formatted string
    * (or as-is conversion specifier for unknown conversions) */
      { int n = str_arg_l - zero_padding_insertion_ind;
        if (n > 0) {
          if (str_l < str_m) {
            size_t avail = str_m-str_l;
            fast_memcpy(str+str_l, str_arg+zero_padding_insertion_ind,
                        (n>(int)avail?avail:n));
          }
          str_l += n;
        }
      }
   /* insert right padding */
      if (justify_left) {          /* right blank padding to the field width */
        int n = min_field_width - (str_arg_l+number_of_zeros_to_pad);
        if (n > 0) {
          if (str_l < str_m) {
            size_t avail = str_m-str_l;
            fast_memset(str+str_l, ' ', (n>(int)avail?avail:n));
          }
          str_l += n;
        }
      }
    }
  }
#if defined(NEED_SNPRINTF_ONLY)
  va_end(ap);
#endif
  if (str_m > 0) { /* make sure the string is null-terminated
                      even at the expense of overwriting the last character
                      (shouldn't happen, but just in case) */
    str[str_l <= str_m-1 ? str_l : str_m-1] = '\0';
  }
  /* Return the number of characters formatted (excluding trailing null
   * character), that is, the number of characters that would have been
   * written to the buffer if it were large enough.
   *
   * The value of str_l should be returned, but str_l is of unsigned type
   * size_t, and snprintf is int, possibly leading to an undetected
   * integer overflow, resulting in a negative return value, which is illegal.
   * Both XSH5 and ISO C99 (at least the draft) are silent on this issue.
   * Should errno be set to EOVERFLOW and EOF returned in this case???
   */
  return (int) str_l;
}
#endif
#endif /* SAPP_USE_PORTABLE_SNPRINTF */
