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
 *     sccp_platform.h
 *
 *  CREATION/MODIFICATION DATES
 *     Created:  November 2002, Sam Hague
 *     Modified: 
 *
 *  DESCRIPTION
 *     SCCP platform dependent header file
 */
#ifndef _SCCP_PLATFORM_H_
#define _SCCP_PLATFORM_H_

#include <stdio.h>  /* define NULL */
#include <stdarg.h> /* variable arguments */
#include "ssapi.h"  /* ssapi callbacks */


/*
 * Define flags.
 */

/*
 * SCCP_MSG_DUMP: produce hex dump of SCCP messages.
 * SCCP_USE_POOL: use the static memory pool instead of platform dynamic
 *                memory routines.
 * SCCP_USE_POOL_STATS: use to debug the memory pool. Prints a list of the
 *                      current pool usage for each malloc or free.
 *
 * SCCP_PLATFORM_WIN32: the platform is win32
 * SCCP_PLATFORM_UNIX: the platform is unix
 * SCCP_PLATFORM_PSOS: the platform is psos
 * SCCP_PLATFORM_79XX: the platform is 79xx
 */
#define SCCP_MSG_DUMP
#define SCCP_USE_POOL
/*
 * Flip on if you want the pool stats.
 */
//#define SCCP_USE_POOL_STATS


//#define SCCP_PLATFORM_WINDOWS
//#define SCCP_PLATFORM_UNIX
//#define SCCP_PLATFORM_PSOS
//#define SCCP_PLATFORM_79XX

/*
 * Specify the device type used for registration.
 */
#define SCCP_DEVICE_TYPE SCCPMSG_DEVICE_TYPE_STATION_TELECASTER_MGR

/*
 * Use this #define to turn on/off debug.
 */
#ifdef _DEBUG
#define SCCP_DEBUG
#endif
#define SCCP_DEBUG


/*
 * Turn off debugging until printf works properly in PSOS.
 */
#ifdef SCCP_PLATFORM_PSOS
#undef SCCP_DEBUG
#endif

/* add this is the platform does not have snprintf and vsnprintf */
#ifdef SCCP_PLATFORM_PSOS
#define SCCP_USE_PORTABLE_SNPRINTF
#endif
#ifdef  SCCP_USE_PORTABLE_SNPRINTF
#ifndef _PORTABLE_SNPRINTF_H_
#define _PORTABLE_SNPRINTF_H_

#include <stdarg.h>

#define HAVE_SNPRINTF
#define PREFER_PORTABLE_SNPRINTF

#define PORTABLE_SNPRINTF_VERSION_MAJOR 2
#define PORTABLE_SNPRINTF_VERSION_MINOR 2

#ifdef HAVE_SNPRINTF

#include <stdarg.h>
#ifdef SCCP_PLATFORM_PSOS
#include "I:/microtec/tools/include/mcc68k/stdio.h"
#else
#include <stdio.h>
#endif

#else
extern int snprintf(char *, size_t, const char *, /*args*/ ...);
extern int vsnprintf(char *, size_t, const char *, va_list);
#endif

#if defined(HAVE_SNPRINTF) && defined(PREFER_PORTABLE_SNPRINTF)
extern int portable_snprintf(char *str, size_t str_m, const char *fmt, /*args*/ ...);
extern int portable_vsnprintf(char *str, size_t str_m, const char *fmt, va_list ap);
#define snprintf  portable_snprintf
#define vsnprintf portable_vsnprintf
#endif
#if 0
extern int asprintf  (char **ptr, const char *fmt, /*args*/ ...);
extern int vasprintf (char **ptr, const char *fmt, va_list ap);
extern int asnprintf (char **ptr, size_t str_m, const char *fmt, /*args*/ ...);
extern int vasnprintf(char **ptr, size_t str_m, const char *fmt, va_list ap);
#endif
#endif _PORTABLE_SNPRINTF_H_
#endif /* SCCP_USE_PORTABLE_SNPRINTF */

char *sccp_platform_strncpy(char *dest, const char *src, unsigned int count);

/*****************************************************************************/
/*****************************************************************************/
/*
 * These #defines are added for convenience. The user can replace them 
 * with direct calls instead of using the callbacks. Maybe it will
 * help with performance.
 */
#define SCCP_TIMER_ALLOCATE()                ssapi_cbs->timer_allocate()
#define SCCP_TIMER_INITIALIZE(a, b, c, d, e) ssapi_cbs->timer_initialize(a, b, c, d, e)
#define SCCP_TIMER_ACTIVATE(a)               ssapi_cbs->timer_activate(a)
#define SCCP_TIMER_CANCEL(a)                 ssapi_cbs->timer_cancel(a)
#define SCCP_TIMER_FREE(a)                   ssapi_cbs->timer_free(a)

/*
 * Decide which memoery routines to use - callbacks, debug version or ???.
 */
#ifdef SCCP_USE_POOL

#ifdef SCCP_USE_POOL_STATS
void *sccp_pool_malloc(int type, unsigned int size, char *fname, int line);
void sccp_pool_free(int type, void *ptr, char *fname, int line);
#define SCCP_MALLOC(a)                       sccp_pool_malloc(-1, a, __FILE__, __LINE__)
#define SCCP_FREE(a)                         sccp_pool_free(-1, a, __FILE__, __LINE__)
#else
void *sccp_pool_malloc(int type, unsigned int size);
void sccp_pool_free(int type, void *ptr);
#define SCCP_MALLOC(a)                       sccp_pool_malloc(-1, a)
#define SCCP_FREE(a)                         sccp_pool_free(-1, a)
#endif
//void *sccp_pool_malloc_debug(int type, unsigned int size, char *filename, int line);

//void sccp_pool_free_debug(int type, void *ptr, char *filename, int line);

//#define SCCP_MALLOC(a)                       sccp_pool_malloc_debug(-1, a, __FILE__, __LINE__)
//#define SCCP_FREE(a)                         sccp_pool_free_debug(-1, a, __FILE__, __LINE__)
#else
#ifdef _DEBUG1 /* _DEBUG */
/*
 * The _DEBUG flag allows the use of the windows debug stuff, adds caller names,
 * adds freed memory to a debug list for tracking...
 */
#include <crtdbg.h> /* definitions of debug versions */
#define SCCP_PLATFORM_MEMCHK
#define SCCP_MALLOC(a)                       _malloc_dbg((a), _NORMAL_BLOCK, __FILE__, __LINE__)
#define SCCP_FREE(a)                         _free_dbg((a), _NORMAL_BLOCK)
#else /* _DEBUG */
#define SCCP_MALLOC(a)                       ssapi_cbs->malloc(a)
#define SCCP_FREE(a)                         ssapi_cbs->free(a)
#endif /* _DEBUG */
#endif /* SCCP_USE_POOL */

#define SCCP_MEMSET(a, b, c)                 ssapi_cbs->memset(a, b, c)
#define SCCP_MEMCPY(a, b, c)                 ssapi_cbs->memcpy(a, b, c)
#define SCCP_SOCKET_OPEN(a)                  ssapi_cbs->socket_open(a)
#define SCCP_SOCKET_CLOSE(a)                 ssapi_cbs->socket_close(a)
#define SCCP_SOCKET_CONNECT(a, b, c)         ssapi_cbs->socket_connect(a, b, c)
#define SCCP_SOCKET_SEND(a, b, c)            ssapi_cbs->socket_send(a, b, c)
#define SCCP_SOCKET_RECV(a, b, c)            ssapi_cbs->socket_recv(a, b, c)
#define SCCP_SOCKET_GET_LAST_ERROR()         ssapi_cbs->socket_getlasterror()
#define SCCP_SOCKET_GETSOCKNAME(a)           ssapi_cbs->socket_getsockname(a)
#define SCCP_SOCKET_GETMAC(a)                ssapi_cbs->socket_getmac(a)
#define SCCP_HTONS(a)                        ssapi_cbs->htons(a)
#define SCCP_HTONL(a)                        ssapi_cbs->htonl(a)
#define SCCP_NTOHS(a)                        ssapi_cbs->ntohs(a)
#define SCCP_NTOHL(a)                        ssapi_cbs->ntohl(a)
#define SCCP_STRTIME(a)                      ssapi_cbs->strtime(a)
#define SCCP_MUTEX_CREATE(a, b)              ssapi_cbs->mutex_create(a, b)
#define SCCP_MUTEX_LOCK(a, b)                ssapi_cbs->mutex_lock(a, b)
#define SCCP_MUTEX_UNLOCK(a)                 ssapi_cbs->mutex_unlock(a)
#define SCCP_MUTEX_DELETE(a)                 ssapi_cbs->mutex_delete(a)
#define SCCP_THREAD_GET()                    ssapi_cbs->thread_get()
#define SCCP_THREAD_RUN(a)                   ssapi_cbs->thread_run(a)
#define SCCP_PRINTF(a)                       ssapi_cbs->printf a
#define SCCP_SNPRINTF(a)                     ssapi_cbs->snprintf a
#define SCCP_VSNPRINTF(a, b, c, d)           ssapi_cbs->vsnprintf(a, b, c, d)
#define SCCP_STRNCPY(a, b, c)                ssapi_cbs->strncpy(a, b, c)
//#define SCCP_STRNCPY(a, b, c)                sccp_platform_strncpy(a, b, c)
 
#define SCCP_APPCBS(cb)    ((cb)->sccpcb->appcbs)
#define SCCP_APPHANDLE(cb) ((cb)->sccpcb->apphandle)

#define SCCP_OPENSESSION_RES(a, b, c)               SCCP_APPCBS(a)->opensession_res(SCCP_APPHANDLE(a), (b), (c))
#define SCCP_RESETSESSION_RES(a, b, c)              SCCP_APPCBS(a)->resetsession_res(SCCP_APPHANDLE(a), (b), (c))
#define SCCP_RESET(a, b, c)                         SCCP_APPCBS(a)->reset(SCCP_APPHANDLE(a), (b), (c))
#define SCCP_SESSIONSTATUS(a, b, c, d, e)           SCCP_APPCBS(a)->sessionstatus(SCCP_APPHANDLE(a), (b), (c), (d), (e))
#define SCCP_SOFTKEYTEMPLATE_RES(a, b, c)           SCCP_APPCBS(a)->softkeytemplate_res(SCCP_APPHANDLE(a), (b), (c))
#define SCCP_SOFTKEYSET_RES(a, b, c)                SCCP_APPCBS(a)->softkeyset_res(SCCP_APPHANDLE(a), (b), (c))
#define SCCP_LINESTAT_RES(a, b, c)                  SCCP_APPCBS(a)->linestat_res(SCCP_APPHANDLE(a), (b), (c))
#define SCCP_SPEEDDIALSTAT_RES(a, b, c)             SCCP_APPCBS(a)->speeddialstat_res(SCCP_APPHANDLE(a), (b), (c))
#define SCCP_FEATURESTAT_RES(a, b, c)               SCCP_APPCBS(a)->featurestat_res(SCCP_APPHANDLE(a), (b), (c))
#define SCCP_SERVICEURLSTAT_RES(a, b, c)            SCCP_APPCBS(a)->serviceurlstat_res(SCCP_APPHANDLE(a), (b), (c))
#define SCCP_ALL_STREAMS_IDLE(a)                    SCCP_APPCBS(a)->all_streams_idle(SCCP_APPHANDLE(a))
#define SCCP_CLOSE_ABANDONDED_STREAMS(a)            SCCP_APPCBS(a)->close_abandonded_streams(SCCP_APPHANDLE(a))
#define SCCP_SETUP(a, b, c, d, e, f, g, h, i, j, k) SCCP_APPCBS(a)->setup(SCCP_APPHANDLE(a), (b), (c), (d), (e), (f), (g), (h), (i), (j), (k))
#define SCCP_SETUP_ACK(a, b, c, d, e, f, g, h)      SCCP_APPCBS(a)->setup_ack(SCCP_APPHANDLE(a), (b), (c), (d), (e), (f), (g), (h))
#define SCCP_PROCEEDING(a, b, c, d, e, f)           SCCP_APPCBS(a)->proceeding(SCCP_APPHANDLE(a), (b), (c), (d), (e), (f))
#define SCCP_ALERTING(a, b, c, d, e, f, g, h)       SCCP_APPCBS(a)->alerting(SCCP_APPHANDLE(a), (b), (c), (d), (e), (f), (g), (h))
#define SCCP_CONNECT(a, b, c, d, e, f, g)           SCCP_APPCBS(a)->connect(SCCP_APPHANDLE(a), (b), (c), (d), (e), (f), (g))
#define SCCP_CONNECT_ACK(a, b, c, d, e, f, g)       SCCP_APPCBS(a)->connect_ack(SCCP_APPHANDLE(a), (b), (c), (d), (e), (f), (g))
#define SCCP_DISCONNECT(a, b, c, d, e, f)           SCCP_APPCBS(a)->disconnect(SCCP_APPHANDLE(a), (b), (c), (d), (e), (f))
#define SCCP_RELEASE(a, b, c, d, e, f)              SCCP_APPCBS(a)->release(SCCP_APPHANDLE(a), (b), (c), (d), (e), (f))
#define SCCP_RELEASE_COMPLETE(a, b, c, d, e, f)     SCCP_APPCBS(a)->release_complete(SCCP_APPHANDLE(a), (b), (c), (d), (e), (f))
#define SCCP_OFFHOOK(a, b, c, d)                    SCCP_APPCBS(a)->offhook(SCCP_APPHANDLE(a), (b), (c), (d))
#define SCCP_FEATURE_REQ(a, b, c, d, e, f, g)       SCCP_APPCBS(a)->feature_req(SCCP_APPHANDLE(a), (b), (c), (d), (e), (f), (g))
#define SCCP_FEATURE_RES(a, b, c, d, e, f, g)       SCCP_APPCBS(a)->feature_res(SCCP_APPHANDLE(a), (b), (c), (d), (e), (f), (g))
#define SCCP_OPENRCV_REQ(a, b, c, d, e)             SCCP_APPCBS(a)->openrcv_req(SCCP_APPHANDLE(a), (b), (c), (d), (e))
#define SCCP_CLOSERCV(a, b, c, d, e)                SCCP_APPCBS(a)->closercv(SCCP_APPHANDLE(a), (b), (c), (d), (e))
#define SCCP_STARTXMIT(a, b, c, d, e)               SCCP_APPCBS(a)->startxmit(SCCP_APPHANDLE(a), (b), (c), (d), (e))
#define SCCP_STOPXMIT(a, b, c, d, e)                SCCP_APPCBS(a)->stopxmit(SCCP_APPHANDLE(a), (b), (c), (d), (e))
#define SCCP_STARTTONE(a, b, c, d, e, f)            SCCP_APPCBS(a)->starttone(SCCP_APPHANDLE(a), (b), (c), (d), (e), (f))
#define SCCP_STOPTONE(a, b, c, d)                   SCCP_APPCBS(a)->stoptone(SCCP_APPHANDLE(a), (b), (c), (d))
#define SCCP_RINGER(a, b, c, d, e, f)               SCCP_APPCBS(a)->ringer(SCCP_APPHANDLE(a), (b), (c), (d), (e), (f))
#define SCCP_LAMP(a, b, c, d, e, f)                 SCCP_APPCBS(a)->lamp(SCCP_APPHANDLE(a), (b), (c), (d), (e), (f))
#define SCCP_SPEAKER(a, b, c, d, e)                 SCCP_APPCBS(a)->speaker(SCCP_APPHANDLE(a), (b), (c), (d), (e))
#define SCCP_MICRO(a, b, c, d, e)                   SCCP_APPCBS(a)->micro(SCCP_APPHANDLE(a), (b), (c), (d), (e))
#define SCCP_CONNINFO(a, b, c, d, e)                SCCP_APPCBS(a)->conninfo(SCCP_APPHANDLE(a), (b), (c), (d), (e))
#define SCCP_TIMEDATE(a, b, c, d, e)                SCCP_APPCBS(a)->timedate(SCCP_APPHANDLE(a), (b), (c), (d), (e))
#define SCCP_DISPLAY(a, b, c, d, e)                 SCCP_APPCBS(a)->display(SCCP_APPHANDLE(a), (b), (c), (d), (e))
#define SCCP_CLEARDISPLAY(a, b, c, d)               SCCP_APPCBS(a)->cleardisplay(SCCP_APPHANDLE(a), (b), (c), (d))
#define SCCP_PROMPT(a, b, c, d, e, f)               SCCP_APPCBS(a)->prompt(SCCP_APPHANDLE(a), (b), (c), (d), (e), (f))
#define SCCP_CLEARPROMPT(a, b, c, d)                SCCP_APPCBS(a)->clearprompt(SCCP_APPHANDLE(a), (b), (c), (d))
#define SCCP_NOTIFY(a, b, c, d, e, f)               SCCP_APPCBS(a)->notify(SCCP_APPHANDLE(a), (b), (c), (d), (e), (f))
#define SCCP_CLEARNOTIFY(a, b, c, d)                SCCP_APPCBS(a)->clearnotify(SCCP_APPHANDLE(a), (b), (c), (d))
#define SCCP_CONNECTIONSTATS(a, b, c, d, e)         SCCP_APPCBS(a)->connectionstats(SCCP_APPHANDLE(a), (b), (c), (d), (e))
#define SCCP_ACTIVATEPLANE(a, b, c, d)              SCCP_APPCBS(a)->activateplane(SCCP_APPHANDLE(a), (b), (c), (d))
#define SCCP_DEACTIVATEPLANE(a, b, c, d)            SCCP_APPCBS(a)->deactivateplane(SCCP_APPHANDLE(a), (b), (c), (d))
#define SCCP_BACKSPACE_REQ(a, b, c, d)              SCCP_APPCBS(a)->backspace_req(SCCP_APPHANDLE(a), (b), (c), (d))
#define SCCP_DIALEDNUMBER(a, b, c, d, e)            SCCP_APPCBS(a)->dialednumber(SCCP_APPHANDLE(a), (b), (c), (d), (e))
#define SCCP_USERTODEVICEDATA(a, b, c, d, e)        SCCP_APPCBS(a)->usertodevicedata(SCCP_APPHANDLE(a), (b), (c), (d), (e))
#define SCCP_PRIORITYNOTIFY(a, b, c, d, e, f, g)    SCCP_APPCBS(a)->prioritynotify(SCCP_APPHANDLE(a), (b), (c), (d), (e), (f), (g))
#define SCCP_CLEARPRIORITYNOTIFY(a, b, c, d, e)     SCCP_APPCBS(a)->clearprioritynotify(SCCP_APPHANDLE(a), (b), (c), (d), (e))
#define SCCP_PASSTHRU(a, b, c, d)                   SCCP_APPCBS(a)->passthru(SCCP_APPHANDLE(a), (b), (c), (d))

#define SCCP_MUTEX_TIMEOUT_INFINITE 0xFFFFFFFF
/*****************************************************************************/
/*****************************************************************************/

/******************************************************************************
 * SCCP_PLATFORM_WINDOWS
 *****************************************************************************/
#ifdef SCCP_PLATFORM_WINDOWS

/*
 * Disable compiler warnings:
 * - 4100: unreferenced formal parameter
 */
#pragma warning(disable: 4100) 

int sccp_platform_write_file(int which, char *buf);

/*
 * No need for these macros since we are already little-endian.
 */
#define CMTOSS(x) (x)
#define CMTOSL(x) (x)
#define STOCMS(x) (x)
#define STOCML(x) (x)
#endif /* SCCP_PLATFORM_WINDOWS */


/******************************************************************************
 * SCCP_PLATFORM_79XX
 *****************************************************************************/
#ifdef SCCP_PLATFORM_79XX

#define sccp_platform_write_file(which, buf)

/*
 * No need for these macros since we are already little-endian.
 */
#define CMTOSS(x) (x)
#define CMTOSL(x) (x)
#define STOCMS(x) (x)
#define STOCML(x) (x)

#endif /* SCCP_PLATFORM_79XX */


/******************************************************************************
 * SCCP_PLATFORM_UNIX
 *****************************************************************************/
#ifdef SCCP_PLATFORM_UNIX

#define sccp_platform_write_file(which, buf)

/*
 * The functions are backwards from the normal ntoh functions, because
 * the CM expects to receive messages in little-endian order. So, on 
 * big-endian platforms we have to convert the messages to little-endian
 * when sending messages and convert to big-endian when receiving.
 */
#define CMTOSS(a) ((((a)&0x00FF) << 8) + (((a)&0xFF00) >> 8 ))
#define CMTOSL(a) ((CMTOSS((a)&0x0000FFFF) << 16) + (CMTOSS(((a)&0xFFFF0000)>>16)))
#define STOCMS(a) CMTOSS(a)
#define STOCML(a) CMTOSL(a)

#endif /* SCCP_PLATFORM_UNIX */


/******************************************************************************
 * SCCP_PLATFORM_PSOS
 *****************************************************************************/
#ifdef SCCP_PLATFORM_PSOS
//#include <stdio.h>
//#include <stdarg.h>
//#include <string.h>
//#define Sleep sleep
#define SCCP_PRINTF(a, b)  LIF_PutStr(b)
#define SCCP_STRNCPY       csw_strncpy

#ifdef SCCP_USE_PORTABLE_SNPRINTF
#define SCCP_SNPRINTF     portable_snprintf
#define SCCP_VSNPRINTF    portable_vsnprintf
#else  /* SCCP_USE_PORTABLE_SNPRINTF */
#define SCCP_SNPRINTF   snprintf
#define SCCP_VSNPRINTF  vsnprintf
#endif /* SCCP_USE_PORTABLE_SNPRINTF */

#define SCCP_MEMCHK(a)  
#define sccp_platform_write_file(which, buf)

/*
 * The functions are backwards from the normal ntoh functions, because
 * the CM expects to receive messages in little-endian order. So, on 
 * big-endian platforms we have to convert the messages to little-endian
 * when sending messages and convert to big-endian when receiving.
 */
#define CMTOSS(a) ((((a)&0x00FF) << 8) + (((a)&0xFF00) >> 8 ))
#define CMTOSL(a) ((CMTOSS((a)&0x0000FFFF) << 16) + (CMTOSS(((a)&0xFFFF0000)>>16)))
#define STOCMS(a) CMTOSS(a)
#define STOCML(a) CMTOSL(a)

#endif /* SCCP_PLATFORM_PSOS */


int sccp_platform_init(void);
int sccp_platform_cleanup(void);
void sccp_platform_printf(int level1, int level2, const char *_format, ...);

#endif /* _SCCP_PLATFORM_H_ */
