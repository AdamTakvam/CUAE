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
 *     ssapi.h
 *
 *  CREATION/MODIFICATION DATES
 *     Created:  November 2002, Sam Hague
 *     Modified: 
 *
 *  DESCRIPTION
 *     System Services API header
 */
#ifndef _SSAPI_H_
#define _SSAPI_H_

//#ifndef SCCP_PLATFORM_WINDOWS
//#define SCCP_PLATFORM_WINDOWS
//#endif
/*
 * Get winsock values from sscp_platform.h - it includes winsock2.h.
 */
//#include "sccp_platform.h"
             

#define SSAPI_SOCKET_ERROR    -1 //SOCKET_ERROR
#define SSAPI_SOCKET_INVALID  -1 //INVALID_SOCKET

//#define SCCP_PROTOTYPE_CHECKING 
#ifdef SCCP_PROTOTYPE_CHECKING
typedef struct ssapi_timer_  *ssapi_timer_t;
typedef struct ssapi_mutex_  *ssapi_mutex_t;
typedef struct ssapi_thread_ *ssapi_thread_t;
#else
typedef void *ssapi_timer_t;
typedef void *ssapi_mutex_t;
typedef void *ssapi_thread_t;
#endif

typedef void (ssapi_timer_callback_f)(ssapi_timer_t timer,
                                      void *param1, void *param2);
typedef ssapi_timer_t (ssapi_timer_allocate_f)(void);
typedef void (ssapi_timer_initialize_f)(ssapi_timer_t timer, int period,
                                        ssapi_timer_callback_f *expiration,
                                        void *param1, void *param2);
typedef void (ssapi_timer_activate_f)(ssapi_timer_t timer);
typedef void (ssapi_timer_cancel_f)(ssapi_timer_t timer);
typedef void (ssapi_timer_free_f)(ssapi_timer_t timer);

typedef void *(ssapi_malloc_f)(unsigned int size);
typedef void (ssapi_free_f)(void *ptr);
typedef void *(ssapi_memset_f)(void *dest, int c, unsigned int count);
typedef void *(ssapi_memcpy_f)(void *dest, const void *src,
                               unsigned int count);

typedef int (ssapi_socket_open_f)(int *socket);
typedef int (ssapi_socket_close_f)(int socket);
typedef int (ssapi_socket_connect_f)(int socket, unsigned long addr,
                                     unsigned short port);
typedef int (ssapi_socket_send_f)(int socket, char *buf, int len);
typedef int (ssapi_socket_recv_f)(int socket, char *buf, int len);
typedef int (ssapi_socket_getlasterror_f)(void);
typedef unsigned long (ssapi_socket_getsockname_f)(int socket);
typedef char *(ssapi_socket_getmac_f)(int socket);    
typedef unsigned short (ssapi_htons_f)(unsigned short data);
typedef unsigned long (ssapi_htonl_f)(unsigned long data);
typedef unsigned short (ssapi_ntohs_f)(unsigned short data);
typedef unsigned long (ssapi_ntohl_f)(unsigned long data);

typedef char *(ssapi_strtime_f)(char *buf);

typedef int (ssapi_mutex_create_f)(int owner, ssapi_mutex_t *mutex);
typedef int (ssapi_mutex_lock_f)(ssapi_mutex_t mutex, unsigned long timeout);
typedef int (ssapi_mutex_unlock_f)(ssapi_mutex_t mutex);
typedef int (ssapi_mutex_delete_f)(ssapi_mutex_t mutex);

typedef ssapi_thread_t (ssapi_thread_get_f)(void);
typedef void (ssapi_thread_run_f)(ssapi_thread_t thread);

#include <stdarg.h>
typedef int (ssapi_printf_f)(const char *fmt, /*args*/...);
typedef int (ssapi_snprintf_f)(char *buf, unsigned int count, const char *fmt, /*args*/...);
typedef int (ssapi_vsnprintf_f)(char *buf, unsigned int count, const char *fmt, va_list ap);
typedef char *(ssapi_strncpy_f)(char *dest, const char *src, unsigned int count);


typedef struct ssapi_callbacks_t_ {
#if 0 /* Used for code documentation */
    /*
     * FUNCTION:    timer_allocate
     *
     * DESCRIPTION: Allocates a timer.
     *
     * PARAMETERS:  None
     *
     * RETURNS:
     *     success: pointer to valid timer
     *     failure: NULL
     */                    
    void *(timer_allocate)(void);

    /*
     * FUNCTION:    timer_initialize
     *
     * DESCRIPTION: Initializes a timer.
     *
     * PARAMETERS:
     *     timer:      valid timer returned from timer_allocate
     *     period:     duration of timer in milliseconds
     *     expiration: callback function called when timer expires
     *     param1:     data returned in callback
     *     param2:     data returned in callback
     *
     * RETURNS:     None
     */                    
    void (timer_initialize)(ssapi_timer_t timer, int period,
                            ssapi_timer_callback expiration,
                            void *param1, void *param2);

    /*
     * FUNCTION:    timer_activate
     *
     * DESCRIPTION: Activates (starts) a timer.
     *
     * PARAMETERS:
     *     timer:   valid timer returned from timer_allocate
     *
     * RETURNS:     None
     */                    
    void (timer_activate)(ssapi_timer_t timer);

    /*
     * FUNCTION:    timer_cancel
     *
     * DESCRIPTION: Cancels (stops) a timer.
     *
     * PARAMETERS:
     *     timer:   valid timer returned from timer_allocate
     *
     * RETURNS:     None
     */                    
    void (timer_cancel)(ssapi_timer_t timer);

    /*
     * FUNCTION:    timer_free
     *
     * DESCRIPTION: Frees a timer.
     *
     * PARAMETERS:
     *     timer:   valid timer returned from timer_allocate
     *
     * RETURNS:     None
     */                    
    void (timer_free)(ssapi_timer_t timer);

    /*
     * FUNCTION:    malloc
     *
     * DESCRIPTION: Returns a pointer to a block of memory of at
     *              least <size> bytes.
     *
     * PARAMETERS:
     *     size:    bytes requested   
     *
     * RETURNS:     
     *     success: pointer to valid memory
     *     failure: NULL
     */                    
    void *(malloc)(unsigned int size);

    /*
     * FUNCTION:    free
     *
     * DESCRIPTION: Frees the memory pointed to by a previous malloc.
     *
     * PARAMETERS:
     *     ptr:     pointer to memory
     *
     * RETURNS:     None
     */                    
    void (free)(void *ptr);

    /*
     * FUNCTION:    memset
     *
     * DESCRIPTION: Sets the first <count> bytes of memory area <dest>
     *              to the value <c>.
     *
     * PARAMETERS:
     *     dest:    pointer to memory
     *     c:       value to assign
     *     count:   number of bytes to assign
     *
     * RETURNS:     None
     */                    
    void *(memset)(void *dest, int c, unsigned int count);

    /*
     * FUNCTION:    memcpy
     *
     * DESCRIPTION: Copies <count> bytes from memory area <src>
     *              to memory area <dest>.
     *
     * PARAMETERS:
     *     dest:    pointer to destination memory
     *     src:     pointer to source memory
     *     count:   number of bytes to copy
     *
     * RETURNS:     pointer to dest
     */                    
    void *(memcpy)(void *dest, const void *src, unsigned int count);

    /*
     * FUNCTION:    socket_open
     *
     * DESCRIPTION: Request to open a TCP socket.
     *
     * PARAMETERS:  None
     *
     * RETURNS:     0 for success, otherwise positive value indicating error
	 *
	 *     socket:  socket identifier, must be positive value greater than 0
     */                    
    int (socket_open)(void);

    /*
     * FUNCTION:    socket_close
     *
     * DESCRIPTION: Request to close a socket.
     *
     * PARAMETERS:  None
     *
     * RETURNS:     0 for success, otherwise positive value indicating error
	 */                    
    int (socket_close)(int socket);

    /*
     * FUNCTION:    socket_connect
     *
     * DESCRIPTION: Requests a connection on a socket.
     *
     * PARAMETERS:
     *     socket:  identifies a socket
     *     addr:    peer address of connection
     *     port:    peer port of connection
     *
     * RETURNS:     0 for success, otherwise positive value indicating error
	 *
	 * NOTES:       The application must return a successful return code
	 *              if non-blocking sockets are implemented. Normally a
	 *              return code of EWOULDBLOCK or EINPROGRESS is returned when
	 *              a connect is attempted and later an event is received that
	 *              indicates the status of the connection.
     */                    
    int (socket_connect)(int socket, unsigned long addr,
                         unsigned short port);

    /*
     * FUNCTION:    socket_send
     *
     * DESCRIPTION: Request to send data on a socket.
     *
     * PARAMETERS:
     *     socket:  identifies a socket
     *     buf:     data to send
     *     len:     length of data
     *
     * RETURNS:     0 for success, otherwise positive value indicating error
     */                    
    int (socket_send)(int socket, char *buf, int len);

    /*
     * FUNCTION:    socket_recv
     *
     * DESCRIPTION: Request to receive data on a socket.
     *
     * PARAMETERS:
     *     socket:  identifies a socket
     *     buf:     data to receive
     *     len:     length of data
     *
     * RETURNS:     0 for success, otherwise positive value indicating error
	 */                    
    int (socket_recv)(int socket, char *buf, int len);

    /*
     * FUNCTION:    socket_getlasterror
     *
     * DESCRIPTION: Returns error number of last network error.
     *
     * PARAMETERS:  None
     *
     * RETURNS:     Error number
     */                    
    int (socket_getlasterror)(void);

    /*
     * FUNCTION:    socket_getsockname
     *
     * DESCRIPTION: Returns address of socket.
     *
     * PARAMETERS:
     *     socket:  identifies a socket
     *
     * RETURNS:
     *     success: address of socket
     *     failure: -1
     */                    
    unsigned long (socket_getsockname(int socket);

    /*
     * FUNCTION:    socket_getmac
     *
     * DESCRIPTION: Returns mac address of socket.
     *
     * PARAMETERS:
     *     socket:  identifies a socket
     *
     * RETURNS:
     *     success: mac address of socket
     *     failure: -1
     */                    
    char *(socket_getmac)(int data);    

    /*
     * FUNCTION:    htons
     *
     * DESCRIPTION: Takes a 16 bit number in host order and
     *              returns a 16 bit number in network order.
     *
     * PARAMETERS:
     *     data:    number to be converted
     *
     * RETURNS:     converted number
     */                    
    unsigned short (htons)(unsigned short data);

    /*
     * FUNCTION:    htonl
     *
     * DESCRIPTION: Takes a 32 bit number in host order and
     *              returns a 32 bit number in network order.
     *
     * PARAMETERS:
     *     data:    number to be converted
     *
     * RETURNS:     converted number
     */                    
    unsigned long (htonl)(unsigned long data);
    
    /*
     * FUNCTION:    ntohs
     *
     * DESCRIPTION: Takes a 16 bit number in network order and
     *              returns a 16 bit number in host order.
     *
     * PARAMETERS:
     *     data:    number to be converted
     *
     * RETURNS:     converted number
     */                    
    unsigned short (ntohs)(unsigned short data);

    /*
     * FUNCTION:    ntohl
     *
     * DESCRIPTION: Takes a 32 bit number in network order and
     *              returns a 32 bit number in host order.
     *
     * PARAMETERS:
     *     data:    number to be converted
     *
     * RETURNS:     converted number
     */                    
    unsigned long (ntohl)(unsigned long data);
    
    /*
     * FUNCTION:    strtime
     *
     * DESCRIPTION: Returns a text string containing the current time in
     *              this format: HH:MM:SS
     *
     * PARAMETERS:
     *     buf:     buffer for returned string. Must be at least 9 bytes.
     *
     * RETURNS:     text string containing time.
     */                    
    char *(strtime)(char *buf);

    /*
     * FUNCTION:    mutex_create
     *
     * DESCRIPTION: Create a mutex.
     *
     * PARAMETERS:
     *     owner:   value user can assign to mutex
     *
     * RETURNS:     
     *     rc:      0 for success, otherwise 1
     *     mutex:   mutex
     */                    
    int (mutex_create)(int owner, ssapi_mutext_t *mutex);

    /*
     * FUNCTION:    mutex_lock
     *
     * DESCRIPTION: Lock a mutex.
     *
     * PARAMETERS:
     *     mutex:   mutex to be locked.
     *     timeout: maximum time to wait while waiting for mutex
     *
     * RETURNS:     
     *     rc:      0 for success, otherwise 1
     */                    
    int (mutex_lock)(ssapi_mutex_t mutex, unsigned long timeout);

    /*
     * FUNCTION:    mutex_unlock
     *
     * DESCRIPTION: Unlock a mutex.
     *
     * PARAMETERS:
     *     mutex:   mutex to be unlocked.
     *
     * RETURNS:     
     *     rc:      0 for success, otherwise 1
     */                    
    int (mutex_unlock)(ssapi_mutex_t mutex);

    /*
     * FUNCTION:    mutex_delete
     *
     * DESCRIPTION: Delete a mutex.
     *
     * PARAMETERS:
     *     mutex:   mutex to be deleted.
     *
     * RETURNS:     
     *     rc:      0 for success, otherwise 1
     */                    
    int (mutex_delete)(ssapi_mutex_t mutex);

    /*
     * FUNCTION:    thread_get
     *
     * DESCRIPTION: Returns a unique value that identifies the stack's thread.
     *
     * PARAMETERS:
     *
     * RETURNS:     
     *     thread:  thread id on success, otherwise NULL
     */                    
    void *(thread_get)(void);

    /*
     * FUNCTION:    thread_run
     *
     * DESCRIPTION: Request to run the thread. The stack will call this
     *              function when it needs to run.
     *
     * PARAMETERS:
     *     thread:  thread to run
     *
     * RETURNS:     None.
     */                    
    void (thread_run)(ssapi_thread_t thread);
#endif /* Used for code documentation. */

    ssapi_timer_allocate_f            *timer_allocate;
    ssapi_timer_initialize_f          *timer_initialize;
    ssapi_timer_activate_f            *timer_activate;
    ssapi_timer_cancel_f              *timer_cancel;
    ssapi_timer_free_f                *timer_free;

    ssapi_malloc_f                    *malloc;
    ssapi_free_f                      *free;
    ssapi_memset_f                    *memset;
    ssapi_memcpy_f                    *memcpy;

    ssapi_socket_open_f               *socket_open;
    ssapi_socket_close_f              *socket_close;
    ssapi_socket_connect_f            *socket_connect;
    ssapi_socket_send_f               *socket_send;
    ssapi_socket_recv_f               *socket_recv;
    ssapi_socket_getlasterror_f       *socket_getlasterror;
    ssapi_socket_getsockname_f        *socket_getsockname;
    ssapi_socket_getmac_f             *socket_getmac;
    ssapi_htons_f                     *htons;
    ssapi_htonl_f                     *htonl;
    ssapi_ntohs_f                     *ntohs;
    ssapi_ntohl_f                     *ntohl;

    ssapi_strtime_f                   *strtime;

    ssapi_mutex_create_f              *mutex_create;
    ssapi_mutex_lock_f                *mutex_lock;
    ssapi_mutex_unlock_f              *mutex_unlock;
    ssapi_mutex_delete_f              *mutex_delete;

    ssapi_thread_get_f                *thread_get;
    ssapi_thread_run_f                *thread_run;

    ssapi_printf_f                    *printf;
    ssapi_snprintf_f                  *snprintf;
    ssapi_vsnprintf_f                 *vsnprintf;
    ssapi_strncpy_f                   *strncpy;
} ssapi_callbacks_t;

extern ssapi_callbacks_t *ssapi_cbs;

/*
 * FUNCTION:    ssapi_bind
 *
 * DESCRIPTION: Registers the application's callbacks for
 *              system services.
 *
 * PARAMETERS:
 *     appcbs:  application callbacks
 *
 * RETURNS:
 *     success: 0
 *     failure: 1
 */                    
int ssapi_bind(ssapi_callbacks_t *appcbs);

/*
 * FUNCTION:    ssapi_cleanup
 *
 * DESCRIPTION: Clean up the SSAPI software.
 *              
 *
 * PARAMETERS:  None.
 *
 * RETURNS:
 *     success: 0
 *     failure: 1
 */                    
int ssapi_cleanup(void);

void sapp_set_flags(int flags);
int sapp_get_test_flags(void);

#endif /* _SSAPI_H_ */
