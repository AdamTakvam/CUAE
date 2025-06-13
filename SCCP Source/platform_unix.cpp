/*
 *  Copyright (c) 2003, 2004 by Cisco Systems, Inc. All Rights Reserved.
 */
#include <platform.h>
#include <malloc.h>
#include "sccp_debug.h"
#include "platform.h"
#ifndef SAPP_PLATFORM_UNIX_WIN // used when compiling on windows
//#include "/usr/include/pthread.h"
#include <pthread.h>
#include <sys/socket.h>
#include <netinet/in.h>
#include <errno.h>
#include <stdlib.h>
#include <time.h>
#include <string.h>
#include <sys/ioctl.h>
#include <unistd.h>
#include <sys/filio.h>
#include <stdio.h>
#else
#include <process.h>
#include <time.h>
#include <winsock2.h>
#endif


#define PLAT_ID "PLAT   "

static int plat_debug = 1;
#define PLAT_DEBUG if (plat_debug) printf

#define PLAT_DEBUGP if (0) printf

static char *platform_mac = "000347b98077";
extern int errno;

//#define PLATFORM_WHO
#if (PLATFORM_WHO)
typedef struct platform_whos_t {
    int first;
    void *tcp;
    void *sccp;
    void *sllist;
} platform_whos_t;

typedef struct platform_threads_t {
    int first;
    pthread_t tcp;
    pthread_t sccp;
} platform_threads_t;

static platform_threads_t platform_threads = {0, NULL, NULL};
static platform_whos_t platform_mutexes = {0, NULL, NULL};
static platform_whos_t platform_events = {0, NULL, NULL};

#if 0
char *platform_who (void)
{
    pthread_t thread;
    char *name = "yo";

#if 1
    thread = pthread_self();
    if (pthread_equal(platform_threads.tcp, thread) == 0) {
        name = "sccp";
    }
    else {
        name = "tcp";
    }
#endif
    return (name);
}
#endif
char *platform_who (void *handle, int which)
{
    char *name = "yo";
    platform_whos_t *who;

    if (which == 0) {
        who = &platform_mutexes;
    }
    else {
        who = &platform_events;
    }

    if (handle == who->tcp) {
        name = "tcp";
    }
    else if (handle == who->sccp) {
        name = "sccp";
    }
    else if (handle == who->sllist) {
        name = "sllist";
    }

    return (name);
}

static void platform_set_who (void *handle, int which)
{
    platform_whos_t *who;

    if (which == 0) {
        who = &platform_mutexes;
    }
    else {
        who = &platform_events;
    }

    switch (who->first) {
    case (0):
        who->tcp   = handle;
        who->first = 1;
        break;

    case (1):
        who->sccp  = handle;
        who->first = 2;
        break;

    case (2):
        who->sllist = handle;
        who->first  = 3;
        break;

    }
}
#else
#define platform_who(a, b)     "who"
#define platform_set_who(a, b) ((void)0)
#endif

int platform_mutex_create (int owner, void **mutex)
{
    int rc = 0;
    pthread_mutexattr_t *attr = NULL;

#ifdef  SAPP_HAS_RECURSION
    attr = (pthread_mutexattr_t *)malloc(sizeof(pthread_mutexattr_t));
    if (attr != NULL) {
        pthread_mutexattr_init(attr);
        pthread_mutexattr_settype(attr, PTHREAD_MUTEX_RECURSIVE);
    }
    else {
        PLAT_DEBUG("%s: %-25s: ERROR= %d:%s\n",
                   PLAT_ID, "mutex_create", -1, "malloc");

        return (1);
    }
#endif

    *mutex = malloc(sizeof(pthread_mutex_t));
    if (*mutex != NULL) {
        rc = pthread_mutex_init((pthread_mutex_t *)(*mutex), attr);
        if (rc != 0) {
            free(*mutex);
            *mutex = NULL;
            PLAT_DEBUG("%s: %-25s: ERROR= %d:%s\n",
                       PLAT_ID, "mutex_create", rc, strerror(rc));
        }
    }
    else {
        PLAT_DEBUG("%s: %-25s: ERROR= %d:%s\n",
           PLAT_ID, "mutex_create", -1, "malloc");
    }

#ifdef SAPP_HAS_RECURSION
    free(attr);
#endif

    PLAT_DEBUGP("platform_mutex_create: %p\n", *mutex);
 
    return (rc);
}

int platform_mutex_delete (void *mutex)
{
    int rc = 0;

    if (mutex != NULL) {
        rc = pthread_mutex_destroy((pthread_mutex_t *)mutex);
        if (rc != 0) {
            PLAT_DEBUG("%s: %-25s: ERROR= %d:%s\n",
                       PLAT_ID, "mutex_delete", rc, strerror(rc));
        }

        free(mutex);
    }

    return (rc);
}

int platform_mutex_lock (void *mutex, unsigned long timeout)
{
    int rc = 0;

    PLAT_DEBUGP("platform_mutex_lock: 1: %s: %p\n", platform_who(mutex, 0), mutex);

    if (mutex != NULL) {
        rc = pthread_mutex_lock((pthread_mutex_t *)mutex);
        if (rc != 0) {
            PLAT_DEBUG("%s: %-25s: ERROR= %d:%s\n",
                       PLAT_ID, "mutex_lock", rc, strerror(rc));
        }
    }

    PLAT_DEBUGP("platform_mutex_lock: 2: %s: %p\n", platform_who(mutex, 0), mutex);

    return (rc);
}

int platform_mutex_unlock (void *mutex)
{
    int rc = 0;

    PLAT_DEBUGP("platform_mutex_unlock: 1: %s: %p\n", platform_who(mutex, 0), mutex);

    if (mutex != NULL) {
        rc = pthread_mutex_unlock((pthread_mutex_t *)mutex);
        if (rc != 0) {
            PLAT_DEBUG("%s: %-25s: ERROR= %d:%s\n",
                       PLAT_ID, "mutex_unlock", rc, strerror(rc));
        }
    }

    PLAT_DEBUGP("platform_mutex_unlock: 2: %s: %p\n", platform_who(mutex, 0), mutex);

    return (rc);
}

int platform_event_create (unsigned long signal_state, unsigned long reset_type,
                           void **event)
{
    int rc = 0;

    *event = malloc(sizeof(pthread_cond_t));
    if (*event != NULL) {
        rc = pthread_cond_init((pthread_cond_t *)(*event), NULL);
        if (rc != 0) {
            free(*event);
            *event = NULL;

            PLAT_DEBUG("%s: %-25s: ERROR= %d:%s\n",
                       PLAT_ID, "event_create", rc, strerror(rc));
        }
    }
    else {
        PLAT_DEBUG("%s: %-25s: ERROR= %d:%s\n",
           PLAT_ID, "event_create", -1, "malloc");
    }

    PLAT_DEBUGP("platform_event_create: %p\n", *event);

    platform_set_who(*event, 1);

    return (rc);
}

int platform_event_delete (void *event)
{
    if (event != NULL) {
        pthread_cond_destroy((pthread_cond_t *)(event));
        free(event);
    }

    return (0);
}

int platform_event_set (void *event)
{
    int rc = 0;

    PLAT_DEBUGP("platform_event_set: 1: %s: %p\n", platform_who(event, 1), event);

    if (event != NULL) {
        rc = pthread_cond_signal((pthread_cond_t *)event);
        if (rc != 0) {
            PLAT_DEBUG("%s: %-25s: ERROR= %d:%s\n",
                       PLAT_ID, "event_set", rc, strerror(rc));
        }
    }

    PLAT_DEBUGP("platform_event_set: 2: %s: %p\n", platform_who(event, 1), event);
    
    return (rc);
}

int platform_event_wait (void *event, unsigned long timeout)
{
    int rc = 0;

    PLAT_DEBUGP("platform_event_wait: 1: %s: %p\n", platform_who(event, 1), event);

    if (event != NULL) {
        rc = pthread_cond_wait((pthread_cond_t *)event, (pthread_mutex_t *)timeout);
        if (rc != 0) {
            PLAT_DEBUG("%s: %-25s: ERROR= %d:%s\n",
                       PLAT_ID, "event_set", rc, strerror(rc));
        }
    }

    PLAT_DEBUGP("platform_event_wait: 2: %s: %p\n", platform_who(event, 1), event);

    return (rc);
}

char *platform_get_local_mac_address (unsigned long adapter)
{
    return (platform_mac);
}

int platform_thread_create (platform_thread_proc proc, void *user_data,
                            void **thread)
{
    int rc = 0;

    *thread = malloc(sizeof(pthread_t));
    if (*thread != NULL) {
        rc = pthread_create((pthread_t *)(*thread), NULL, proc, NULL);
        if (rc != 0) {
            free(*thread);
            *thread = NULL;

            PLAT_DEBUG("%s: %-25s: ERROR= %d:%s\n",
                       PLAT_ID, "thread_create", rc, strerror(rc));
        }
    }
    else {
        PLAT_DEBUG("%s: %-25s: ERROR= %d:%s\n",
           PLAT_ID, "thread_create", -1, "malloc");
    }

#if 0
    if (platform_threads.first == 0) {
        platform_threads.tcp = *(pthread_t *)(*thread);
        platform_threads.first = 1;
    }
    else {
        platform_threads.sccp = *(pthread_t *)(*thread);
    }
#endif
    return (rc);
}

int platform_thread_exit (void *thread, unsigned int retval)
{
    if (thread != NULL) {
        pthread_exit(NULL);
    }

    return (0);
}

int platform_thread_wait (void *thread, unsigned long timeout)
{
    if (thread != NULL) {
        pthread_join(*((pthread_t *)thread), NULL);
    }

    return (0);
}

int platform_thread_delete (void *thread)
{
    if (thread != NULL) {
        free(thread);
    }

    return (0);
}

long platform_get_time_sec (void)
{
    return (0);
}

char *platform_strtime (char *buf)
{
    time_t timer;

    timer = time(NULL);

    strftime(buf, 256, "%H:%M:%S", localtime(&timer));

    return (buf);
}

int platform_get_last_error (void)
{
    return (errno);
}

char *platform_get_last_error_string (int error)
{
    return (strerror(error));
}

unsigned short platform_getsockport (platform_socket_t socket)
{
    struct sockaddr_in ipv4;
    long               size = sizeof(ipv4);

    getsockname(socket, (struct sockaddr *)&ipv4, (int *)&size);

    /*
     * Return an IPv4 address in network byte order.
     */
    //return (htonl(ipv4.sin_addr.S_un.S_addr));
    return (ipv4.sin_port);
}

unsigned long platform_getsockname (platform_socket_t socket)
{
    struct sockaddr_in ipv4;
    long               size = sizeof(ipv4);

    getsockname(socket, (struct sockaddr *)&ipv4, (int *)&size);

    /*
     * Return an IPv4 address in network byte order.
     */
    //return (htonl(ipv4.sin_addr.S_un.S_addr));
    return (ipv4.sin_addr.S_un.S_addr);
}

int platform_socket (platform_socket_t *socket_id)
{
    int rc = 0;

    *socket_id = socket(AF_INET, SOCK_STREAM, IPPROTO_TCP);
    if (*socket_id == -1) {
        rc = errno;
        PLAT_DEBUG("%s: %-25s: socket= %d: ERROR= %d:%s\n",
                   PLAT_ID, "socket", *socket_id, rc, strerror(rc));
    }

    return (rc);
}

int platform_ioctl (platform_socket_t socket)
{
    unsigned long arg  = 1;
    int rc;

    rc = ioctl(socket, FIONBIO, &arg);
    if (rc == -1) {
        rc = errno;
        PLAT_DEBUG("%s: %-25s: socket= %d: ERROR= %d:%s\n",
                   PLAT_ID, "socket", socket, rc, strerror(rc));
    }
    else {
        rc = 0;
    }

    return (rc);
}

int platform_close (platform_socket_t socket)
{
    int rc;

    rc = close(socket);
    if (rc == -1) {
        rc = errno;
        PLAT_DEBUG("%s: %-25s: socket= %d: ERROR= %d:%s\n",
                   PLAT_ID, "socket", socket, rc, strerror(rc));
    }

    return (rc);
}

int platform_shutdown (platform_socket_t socket)
{
    int rc;

    rc = shutdown(socket, PLATFORM_SD_SEND);
    if (rc == -1) {
        rc = errno;
        PLAT_DEBUG("%s: %-25s: socket= %d: ERROR= %d:%s\n",
                   PLAT_ID, "socket", socket, rc, strerror(rc));
    }

    return (rc);
}

int platform_connect (platform_socket_t socket, unsigned long addr,
                      unsigned short port)
{
    int                rc;
    struct sockaddr_in dest;

    PLAT_DEBUGP("platform_connect: addr/port= 0x%08lx:%04x\n", addr, port);
    
    memset(&dest, 0, sizeof(dest));
    dest.sin_family = AF_INET;
    dest.sin_port = port;
    dest.sin_addr.S_un.S_addr = addr;

    rc = connect(socket, (struct sockaddr *)(&dest), sizeof(dest));
    if (rc == -1) {
        rc = errno;
        perror(NULL);
        PLAT_DEBUG("%s: %-25s: socket= %d: ERROR= %d:%s\n",
                   PLAT_ID, "connect", socket, rc, strerror(rc));
    }

    return (rc);
}

#if 0
/*
 * convert 16bit field in message received from call manager
 * from little endian format (used on call manager platform) 
 * to big endian format (used on the chopin card)
 */
void 
cmtocs(ushort *y) 
{
  ushort x;
  ushort us;

  x = *y;
  us = ((x & 0x00ff) << SCCP_LEFT_SHIFT_BYTE) | ((x & 0xff00) >> SCCP_RIGHT_SHIFT_BYTE);

  *y = us;

  return;
}

#define ctocms( x ) cmtocs( x )

/*
 * convert 32bit int field in message received from call manager
 * from little endian format (used on call manager platform)
 * to big endian format (used on the chopin card)
 */
void cmtoci (int *y)
{
  int i;
  int x;

  /* As of now we are that the int is of size 4. If this needs
   * to ported in different machine in which the int size is 
   * different they have add for that size here.
   */
  if (sizeof(int) != SCCP_STANDARD_INT_SIZE) {
      SCCP_ERR_BUGINF("\n%s: this int size not implemented...",
                      __FUNCTION__);
      return;
  }
 
  x = *y;
 
  i = ((x & 0x000000ff ) <<  SCCP_LEFT_SHIFT_THREE_BYTES);
  i |= ((x & 0x0000ff00 ) <<  SCCP_LEFT_SHIFT_BYTE);
  i |= ((x & 0x00ff0000 ) >>  SCCP_RIGHT_SHIFT_BYTE);
  i |= ((x & 0xff000000) >> SCCP_RIGHT_SHIFT_THREE_BYTES);
 
  *y = i;
 
  return;
}


/*
 * convert 32bit field in message received from call manager
 * from little endian format (used on call manager platform) 
 * to big endian format (used on the chopin card)
 */
void cmtocl
(ulong *y) 
{
  ulong ul;
  ulong x;

  x = *y;

  ul = ((x & 0x000000ff) <<  SCCP_LEFT_SHIFT_THREE_BYTES);
  ul |= ((x & 0x0000ff00) <<  SCCP_LEFT_SHIFT_BYTE);
  ul |= ((x & 0x00ff0000) >>  SCCP_RIGHT_SHIFT_BYTE);
  ul |= ((x & 0xff000000) >> SCCP_RIGHT_SHIFT_THREE_BYTES);

  *y = ul;

  return;
}

#define ctocml(x) cmtocl(x)

#endif

