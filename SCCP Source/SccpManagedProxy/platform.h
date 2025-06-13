/*
 *  Copyright (c) 2003, 2004 by Cisco Systems, Inc. All Rights Reserved.
 */
#ifndef _PLATFORM_H_
#define _PLATFORM_H_

/*
 * define all the platform-specific #defines for the platform
 */
/* define the platform */
/*
 * SAPP_PLATFORM_79XXWIN for softphone - need SAPP_PLATFORM_WIN32 also
 * SAPP_PLATFORM_UNIX_WIN for unix, but compiling under windows - need 
 *                        SAPP_PLATFORM_UNIX also
 */
//#define SAPP_PLATFORM_WIN32
//#define SAPP_PLATFORM_79XX
//#define SAPP_PLATFORM_79XXWIN
//#define SAPP_PLATFORM_UNIX
//#define SAPP_PLATFORM_UNIX_WIN

/* define the sccp application */
//#define SAPP_SAPP_GSM
//#define SAPP_SAPP_SCCPTEST
//#define SAPP_SAPP_LOOPBACK
#define SAPP_APP_GSM 0
#define SAPP_APP_SCCPTEST 1
#define SAPP_APP_LOOPBACK 2

#ifdef SAPP_SAPP_GSM
#define SAPP_APP SAPP_APP_GSM
#endif
#ifdef SAPP_SAPP_SCCPTEST
#define SAPP_APP SAPP_APP_SCCPTEST
#endif
#ifdef SAPP_SAPP_LOOPBACK
#define SAPP_APP SAPP_APP_LOOPBACK
#endif

/* define other platform attributes */
//#define SAPP_HAS_RECURSION
//#define SAPP_ADD_RECURSION

//#include <memory.h>
//#include <malloc.h>


#define SCCP_SAPP
#ifdef SCCP_SAPP
#ifdef __cplusplus
extern "C" {
#endif
#endif /*SCCP_SAPP */


/*****************************************************************************/
/*****************************************************************************/
/*
 * SAPP_USE_PORTABLE_SNPRINTF: use the portable snprintf/vsnprintf routines
 *                             instead of platform routines.
 * HAVE_SNPRINTF: platform has snprintf
 * PREFER_PORTABLE_SNPRINTF: have platform snprintf, but prefer to use the
 *                           portable snprintf.
 */

/* add this is the platform does not have snprintf and vsnprintf */
//#define SAPP_USE_PORTABLE_SNPRINTF
#define HAVE_SNPRINTF
#define PREFER_PORTABLE_SNPRINTF

#ifdef  SAPP_USE_PORTABLE_SNPRINTF
#ifndef _PORTABLE_SNPRINTF_H_
#define _PORTABLE_SNPRINTF_H_

#include <stdarg.h>


#define PORTABLE_SNPRINTF_VERSION_MAJOR 2
#define PORTABLE_SNPRINTF_VERSION_MINOR 2

#ifdef HAVE_SNPRINTF
#include <stdio.h>
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
#endif /* SAPP_USE_PORTABLE_SNPRINTF */
/*****************************************************************************/
/*****************************************************************************/

/******************************************************************************/
/* SAPP_PLATFORM_UNIX                                                         */
/******************************************************************************/
#ifdef SAPP_PLATFORM_UNIX

#ifndef SAPP_PLATFORM_UNIX_WIN  // used when compiling the unix files under windows
#include <stdio.h>
#include <pthread.h>
#include <errno.h>
#include <stdlib.h>
#include <string.h>
#include <strings.h>
#include <netinet/in.h>

typedef int platform_socket_t;
typedef struct timeval platform_timeval_t;

#else /* SAPP_PLATFORM_UNIX_WIN */

#include <winsock2.h>
#include "stdio.h"

#if 0
typedef struct TIMEVAL_T {
    long tv_usec;
    long tv_sec;
} TIMEVAL;
#endif
typedef TIMEVAL platform_timeval_t;

#define sleep(a) Sleep((a) * 1000)
#define usleep sleep
typedef unsigned int  platform_socket_t;
typedef unsigned long pthread_mutex_t;
typedef unsigned long pthread_mutexattr_t;
typedef unsigned long pthread_cond_t;
typedef unsigned long pthread_t;
#define pthread_mutex_init(a, b) 0
#define pthread_mutex_destroy(a) 0
#define pthread_mutex_lock(a) 0
#define pthread_mutex_unlock(a) 0
#define pthread_mutexattr_init(a) 0
#define pthread_mutexattr_settype(a, b) 0
#define pthread_cond_init(a, b) 0
#define pthread_cond_signal(a) 0
#define pthread_cond_destroy(a) 0
#define pthread_cond_wait(a, b) 0
#define pthread_create(a, b, c, d) 0
#define pthread_exit(a) 0
#define pthread_join(a, b) 0
#define ioctl ioctlsocket
#define close closesocket

#ifndef snprintf
#define snprintf  _snprintf
#endif
#ifndef vsnprintf
#define vsnprintf _vsnprintf
#endif

#endif /* SAPP_PLATFORM_UNIX_WIN */

#define PLATFORM_INVALID_SOCKET (-1)
#define PLATFORM_SOCKET_ERROR   (-1)
#define PLATFORM_WAIT_INFINITE  (0xffffffff)
#define PLATFORM_EWOULDBLOCK    11 //(EWOULDBLOCK) //11 use when compiling under windows
#define PLATFORM_EINPROGRESS    150 //(EINPROGRESS)
#define PLATFORM_SD_SEND        (1)
#if 1
#define PLATFORM_CALLBACK /* __cdecl */
#else
#define PLATFORM_CALLBACK __cdecl
#endif 

typedef void *platform_thread_func_t;
typedef platform_thread_func_t (PLATFORM_CALLBACK *platform_thread_proc)(void *user_data);

#endif /* SAPP_PLATFORM_UNIX */


/******************************************************************************/
/* SAPP_PLATFORM_WIN32                                                        */
/******************************************************************************/
#ifdef SAPP_PLATFORM_WIN32

#ifdef SAPP_PLATFORM_79XXWIN
//#include "types.h"
#ifndef boolean
typedef unsigned short boolean;
#endif
#include "..\..\includes\timer.h"
extern int buginf( const char *_format, ...);
//extern int err_msg(const char *_format, ...);
#define printf buginf
#endif /* SAPP_PLATFORM_79XXWIN */

#define WIN32_LEAN_AND_MEAN     // Exclude rarely-used stuff from Windows headers
//#include <windows.h>
#include <winsock2.h>
#include <stdio.h>
#include <time.h>
#include <stdlib.h>

#if 0
/*
 * Sometimes the compiler blows up when including winsock2.h and MFC stuff
 * so we need to define the winsock stuff here.
 */
#define INFINITE       (0xFFFFFFFF)
#define INVALID_SOCKET (unsigned int)(~0)
#define SOCKET_ERROR   (-1)
#define WSAEWOULDBLOCK (10035)
#define WSAEINPROGRESS (10036)
#define SD_SEND        (0x01)
typedef struct TIMEVAL_T {
    long tv_usec;
    long tv_sec;
} TIMEVAL;
#endif

#ifndef snprintf
#define snprintf  _snprintf
#endif
#ifndef vsnprintf
#define vsnprintf _vsnprintf
#endif

/*
 * sapp expects sleep to be in seconds, but Windows has it in milliseconds.
 */
#define sleep(x) Sleep((x)*1000)

#define SCCP_WINDOWS_PLATFORM /* used for messing with TCHAR */
#define PLATFORM_WAIT_INFINITE  INFINITE//0xFFFFFFFF//INFINITE

//#include <windows.h>
//#include <tchar.h>

typedef TIMEVAL          platform_timeval_t;
//typedef struct timeval   platform_timeval_t;
typedef unsigned int     platform_socket_t;

#define PLATFORM_INVALID_SOCKET (INVALID_SOCKET)
#define PLATFORM_SOCKET_ERROR   (SOCKET_ERROR)
#define PLATFORM_EWOULDBLOCK    (WSAEWOULDBLOCK)
#define PLATFORM_EINPROGRESS    (WSAEINPROGRESS)
#define PLATFORM_SD_SEND        (SD_SEND)

typedef unsigned int platform_thread_func_t;
#define PLATFORM_CALLBACK __stdcall
typedef platform_thread_func_t (PLATFORM_CALLBACK *platform_thread_proc)(void *user_data);

#define platform_thread_wait   platform_event_wait
#define platform_thread_delete platform_event_delete
#endif /* SAPP_PLATFORM_WIN32 */


/******************************************************************************/
/* SAPP_PLATFORM_POCKET_PC                                                    */
/******************************************************************************/
#ifdef PLATFORM_POCKET_PC
#define WINDOWS_PLATFORM
#define PLATFORM_WAIT_INFINITE  INFINITE

#include <windows.h>

// Defines that are not included in the PocketPC 2002 winsock.h header
#define SD_RECEIVE      0x00
#define SD_SEND         0x01
#define SD_BOTH         0x02
// Redefine a function in Windows to a PocketPC equivalent
#define timeGetTime     GetTickCount

typedef unsigned long    platform_thread_t;

#endif /* PLATFORM_POCKET_PC */

#ifndef SAPP_PLATFORM_UNIX
#ifndef SCCP_WINDOWS_PLATFORM
#ifndef _UNICODE
#define _T(x) x
#endif
#define TCHAR char

// Uncomment the next line and set the define CALLBACK to the system callback
// convention type, usually one of the following:
//
// __cdecl
// __stdcall
// __fastcall
//
//#define CALLBACK  <calling convention>
#endif /* SCCP_WINDOWS_PLATFORM */
#endif /* SCCP_PLAFORM_UNIX */


int platform_mutex_create(int owner, void **mutex);
int platform_mutex_delete(void *mutex);
int platform_mutex_lock(void *mutex, unsigned long timeout);
int platform_mutex_unlock(void *mutex);

int platform_event_create(unsigned long signal_state,
                          unsigned long reset_type,
                          void **event);
int platform_event_delete(void *event);
int platform_event_set(void *event);
int platform_event_reset(void *event);
int platform_event_wait(void *event, unsigned long timeout);
int platform_event_wait_multiple(int count, void *events, unsigned long timeout);

int platform_thread_create(platform_thread_proc proc, void *user_data,
                           void **thread);
int platform_thread_exit(void *thread, unsigned int retval);
int platform_thread_wait(void *thread, unsigned long timeout);
int platform_thread_delete(void *thread);

long platform_get_time_sec(void);
char *platform_strtime(char *buf);

char *platform_get_local_mac_address(unsigned long adapter);
unsigned long platform_getsockname(platform_socket_t socket);
unsigned short platform_getsockport(platform_socket_t socket);
int platform_socket(platform_socket_t *socket_id);
int platform_ioctl(platform_socket_t socket);
int platform_shutdown(platform_socket_t socket);
int platform_close(platform_socket_t socket);
int platform_connect(platform_socket_t socket, unsigned long addr,
                     unsigned short port);

int platform_get_last_error(void);
char *platform_get_last_error_string(int error);

#ifdef SCCP_SAPP 
#ifdef __cplusplus
}
#endif
#endif /* SCCP_SAPP */


#if 0
/******************************************************************************
 * SCCP_PLATFORM_WINDOWS
 *****************************************************************************/
#ifdef SCCP_PLATFORM_WINDOWS

//#include <winsock2.h>

//#include <crtdbg.h>
#include <stdio.h>
#include <stdarg.h>
#include "string.h"

/* use this when compiling softphone */
#ifdef SAPP_SAPP_GSM /* SCCP_PLATFORM_79XX */
extern int buginf( const char *_format, ...);
extern int err_msg(const char *_format, ...);
#define printf buginf
#endif /* SAPP_SAPP_GSM */ /* SCCP_PLATFORM_79XX */

int sccp_platform_write_file(int which, char *buf);
void sccp_platform_memchk(int a);
//#define SCCP_PRINTF       printf
//#define SCCP_STRNCPY      strncpy

#if 0
#ifdef SAPP_USE_PORTABLE_SNPRINTF
#define SCCP_SNPRINTF     portable_snprintf
#define SCCP_VSNPRINTF    portable_vsnprintf
#else  /* SAPP_USE_PORTABLE_SNPRINTF */
#define SCCP_SNPRINTF     _snprintf
#define SCCP_VSNPRINTF    _vsnprintf
#endif /* SAPP_USE_PORTABLE_SNPRINTF */
#endif
#define SCCP_MEMCHK(a)    sccp_platform_memchk(a)

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

#include <util_ios_string.h>
extern int buginf(const char *_format, ...);
#define SCCP_PRINTF buginf /* use buginf so telnet display works */

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
#include <stdio.h>
#include <stdarg.h>
#include <string.h>
//#define Sleep sleep
#define SCCP_PRINTF     printf
#define SCCP_STRNCPY    strncpy

#ifdef SAPP_USE_PORTABLE_SNPRINTF
#define SCCP_SNPRINTF     portable_snprintf
#define SCCP_VSNPRINTF    portable_vsnprintf
#else  /* SAPP_USE_PORTABLE_SNPRINTF */
#define SCCP_SNPRINTF   snprintf
#define SCCP_VSNPRINTF  vsnprintf
#endif /* SAPP_USE_PORTABLE_SNPRINTF */

#define SCCP_MEMCHK(a)  

/*
 * The functions are backwards from the normal ntoh functions, because
 * the CM expects to receive messages in little-endian order. So, on 
 * big-endian platforms we have to convert the messages to little-endian.
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
#define SCCP_STRNCPY       strncpy

#ifdef SAPP_USE_PORTABLE_SNPRINTF
#define SCCP_SNPRINTF     portable_snprintf
#define SCCP_VSNPRINTF    portable_vsnprintf
#else  /* SAPP_USE_PORTABLE_SNPRINTF */
#define SCCP_SNPRINTF   snprintf
#define SCCP_VSNPRINTF  vsnprintf
#endif /* SAPP_USE_PORTABLE_SNPRINTF */

#define SCCP_MEMCHK(a)  

/*
 * The functions are backwards from the normal ntoh functions, because
 * the CM expects to receive messages in little-endian order. So, on 
 * big-endian platforms we have to convert the messages to little-endian.
 */
#define CMTOSS(a) ((((a)&0x00FF) << 8) + (((a)&0xFF00) >> 8 ))
#define CMTOSL(a) ((CMTOSS((a)&0x0000FFFF) << 16) + (CMTOSS(((a)&0xFFFF0000)>>16)))
#define STOCMS(a) CMTOSS(a)
#define STOCML(a) CMTOSL(a)

#define sccp_platform_write_file(which, buf)

#endif /* SCCP_PLATFORM_PSOS */

#endif

#endif /* _PLATFORM_H */
