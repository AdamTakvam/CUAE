/*
 *  Copyright (c) 2003 by Cisco Systems, Inc. All Rights Reserved.
 */
#include "platform.h" 
#include <malloc.h>

#ifdef SAPP_PLATFORM_WIN32
#include <process.h>
#include <time.h>
#include <winsock2.h>
#endif

#ifdef PLATFORM_POCKET_PC
#include <Afx.h>
#include <winbase.h>
#include <winsock.h>
#endif

#ifdef SAPP_PLATFORM_79XXWIN
#ifdef __cplusplus
extern "C" {
#endif

extern int buginf( const char *_format, ...);
extern int err_msg(const char *_format, ...);
#define printf buginf
#ifdef __cplusplus
}
#endif
#endif /* SAPP_PLATFORM_79XXWIN */

#define PLAT_ID "PLAT   "

static int plat_debug = 1;
#define PLAT_DEBUG if (plat_debug) printf

#define PLAT_DEBUGP if (0) printf

static char *platform_mac = "000347b98077";

//#define PLATFORM_WHO
#if (PLATFORM_WHO)
typedef struct platform_whos_t {
    int first;
    void *tcp;
    void *sccp;
    void *sllist;
} platform_whos_t;

static platform_whos_t platform_mutexes = {0, NULL, NULL};
static platform_whos_t platform_events = {0, NULL, NULL};

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

static platform_set_who (void *handle, int which)
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

static char * platform_strerror(unsigned long error)
{
    static char msg[256];

    FormatMessage(FORMAT_MESSAGE_FROM_SYSTEM, NULL, error, 0,
                  msg, 256, NULL);

    return (msg);
}

//int platform_mutex_create (int owner, platform_mutex_t *mutex)
int platform_mutex_create (int owner, void **mutex)
{
    unsigned long rc = 0;
    HANDLE handle;

    *mutex = NULL;

    handle = CreateMutex(0, owner, NULL);
    if (handle == NULL) {
        rc = GetLastError();

        PLAT_DEBUG("%s: %-25s: ERROR= %d:%s\n",
                   PLAT_ID, "mutex_create", rc, platform_strerror(rc));
    }
    else {
        *mutex = handle;
    }

    PLAT_DEBUGP("platform_mutex_create: %p\n", *mutex);

    platform_set_who(*mutex, 0);

    return ((int)rc);
}

int platform_mutex_delete (void *mutex)
{
    unsigned long rc;

    rc = CloseHandle((HANDLE)mutex);
    if (rc == 0) {
        rc = GetLastError();

        PLAT_DEBUG("%s: %-25s: ERROR= %d:%s\n",
                   PLAT_ID, "mutex_delete", rc, platform_strerror(rc));
    }

    return ((int)rc);
}

int platform_mutex_lock (void *mutex, unsigned long timeout)
{
    unsigned long rc = 0;

    PLAT_DEBUGP("platform_mutex_lock: 1: %s: %p\n", platform_who(mutex, 0), mutex);

    if (mutex != NULL) {
        rc = WaitForSingleObject((HANDLE)mutex, timeout);
        if (rc == WAIT_FAILED) {
            rc = GetLastError();

            PLAT_DEBUG("%s: %-25s: ERROR= %d:%s\n",
                       PLAT_ID, "mutex_lock", rc, platform_strerror(rc));
        }
    }

    PLAT_DEBUGP("platform_mutex_lock: 2: %s: %p\n", platform_who(mutex, 0), mutex);

    return ((int)rc);
}

int platform_mutex_unlock (void *mutex)
{
    unsigned long rc = 0;

    PLAT_DEBUGP("platform_mutex_unlock: 1: %s: %p\n", platform_who(mutex, 0), mutex);

    if (mutex != NULL) {
        rc = ReleaseMutex((HANDLE)mutex);
        if (rc == 0) {
            rc = GetLastError();

            PLAT_DEBUG("%s: %-25s: ERROR= %d:%s\n",
                       PLAT_ID, "mutex_unlock", rc, platform_strerror(rc));
        }
    }

    PLAT_DEBUGP("platform_mutex_unlock: 2: %s: %p\n", platform_who(mutex, 0), mutex);

    return ((int)rc);
}

int platform_event_create (unsigned long signal_state,
                           unsigned long reset_type,
                           void **event)
{
    unsigned long rc = 0;
    HANDLE        handle;

    *event = NULL;

    handle = CreateEvent(NULL, reset_type, signal_state, NULL);
    if (handle == NULL) {
        rc = GetLastError();

        PLAT_DEBUG("%s: %-25s: ERROR= %d:%s\n",
                   PLAT_ID, "event_create", rc, platform_strerror(rc));
    }
    else {
        *event = handle;
    }

    platform_set_who(*event, 1);

    return ((int)rc);
}

int platform_event_delete (void *event)
{
    unsigned long rc = 0;

    if (event != NULL) {
        rc = CloseHandle((HANDLE)event);
        if (rc == 0) {
            rc = GetLastError();

            PLAT_DEBUG("%s: %-25s: ERROR= %d:%s\n",
                       PLAT_ID, "event_delete", rc, platform_strerror(rc));
        }
    }

    return ((int)rc);
}

int platform_event_set (void *event)
{
    unsigned long rc = 0;

    if (event != NULL) {
        rc = SetEvent((HANDLE)event);
        if (rc == 0) {
            rc = GetLastError();

            PLAT_DEBUG("%s: %-25s: ERROR= %d:%s\n",
                       PLAT_ID, "event_set", rc, platform_strerror(rc));
        }
    }

    return ((int)rc);
}

int platform_event_reset (void *event)
{
    unsigned long rc = 0;

    if (event != NULL) {
        rc = ResetEvent((HANDLE)event);
        if (rc == 0) {
            rc = GetLastError();

            PLAT_DEBUG("%s: %-25s: ERROR= %d:%s\n",
                       PLAT_ID, "event_reset", rc, platform_strerror(rc));
        }
    }

    return ((int)rc);
}

int platform_event_wait (void *event, unsigned long timeout)
{
    unsigned long rc = 0;

    if (event != NULL) {
        rc = WaitForSingleObject((HANDLE)event, timeout);
        if (rc == WAIT_FAILED) {
            rc = GetLastError();

            PLAT_DEBUG("%s: %-25s: ERROR= %d:%s\n",
                       PLAT_ID, "event_wait", rc, platform_strerror(rc));
        }
    }
    
    return ((int)rc);
}

#if 0
int platform_event_wait_multiple (int count, void *events, unsigned long timeout)
{
    unsigned long rc = 0;

    if (events != NULL) {
        rc = WaitForMultipleObjects(count, (CONST HANDLE *)events, 0, timeout);
        if (rc == 0xFFFFFFFF) {
            rc = GetLastError();

            PLAT_DEBUG("%s: %-25s: ERROR= %d:%s\n",
                       PLAT_ID, "event_wait", rc, platform_strerror(rc));
        }
    }
    
    return ((int)(rc - WAIT_OBJECT_0));
}
#endif
char *platform_get_local_mac_address (unsigned long adapter)
{
    return (platform_mac);
}

int platform_thread_create (platform_thread_proc proc, void *user_data,
                            void **thread)
{
    int          rc = 0;
    unsigned int id;

#ifdef SAPP_PLATFORM_WIN32
    *thread = (void *)_beginthreadex(NULL, 0, proc, user_data, 0, &id);
#endif
#ifdef PLATFORM_POCKET_PC
    *thread = (platform_thread_t)CreateThread(NULL, 0,
                                              (LPTHREAD_START_ROUTINE)proc,
                                              user_data, 0,
                                              &id);
#endif

    if (*thread == 0) {
         rc = WSAGetLastError();
    }

    return (rc);
}


int platform_thread_exit (void *thread, unsigned int retval)
{
    _endthreadex(retval);

    return (0);
}

long platform_get_time_sec (void)
{
#ifdef SAPP_PLATFORM_WIN32
    return (time(NULL));
#endif

#ifdef PLATFORM_POCKET_PC
    return ((CTime::GetCurrentTime()).GetTime());
#endif
}

char *platform_strtime (char *buf)
{
    return (_strtime(buf));
}

unsigned short platform_getsockport (platform_socket_t socket)
{
    struct sockaddr_in ipv4;
    long               size = sizeof(ipv4);

    getsockname(socket, (struct sockaddr *)&ipv4, (int *)&size);

#if 0
    printf("port= %04x - %d\n",
           ntohs(ipv4.sin_port), ntohs(ipv4.sin_port));
#endif
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
//    return (htonl(ipv4.sin_addr.S_un.S_addr));
    return (ipv4.sin_addr.S_un.S_addr);
}

int platform_socket (platform_socket_t *socket_id)
{
    int rc = 0;

    *socket_id = socket(AF_INET, SOCK_STREAM, IPPROTO_TCP);
    if (*socket_id == INVALID_SOCKET) {
        rc = WSAGetLastError(); 
        
        PLAT_DEBUG("%s: %-25s: socket= %d: ERROR= %d:%s\n",
                   PLAT_ID, "socket", -1, rc,
                   platform_get_last_error_string(rc));
    }

    return (rc);
}

int platform_ioctl (platform_socket_t socket)
{
    unsigned long arg  = 1;
    int rc;

    rc = ioctlsocket(socket, FIONBIO, &arg);
    if (rc != 0) {
        rc = WSAGetLastError();

        PLAT_DEBUG("%s: %-25s: socket= %d: ERROR= %d:%s\n",
                   PLAT_ID, "ioctl", -1, rc,
                   platform_get_last_error_string(rc));
    }

    return (rc);
}

int platform_close (platform_socket_t socket)
{
    int rc;

    rc = closesocket(socket);
    if (rc != 0) {
        rc = WSAGetLastError();

        PLAT_DEBUG("%s: %-25s: socket= %d: ERROR= %d:%s\n",
                   PLAT_ID, "close", -1, rc, 
                   platform_get_last_error_string(rc));
    }

    return (rc);
}

int platform_shutdown (platform_socket_t socket)
{
    int rc;
    
    rc = shutdown(socket, PLATFORM_SD_SEND);
    if (rc != 0) {
        rc = WSAGetLastError();

        PLAT_DEBUG("%s: %-25s: socket= %d: ERROR= %d:%s\n",
                   PLAT_ID, "shutdown", -1, rc,
                   platform_get_last_error_string(rc));
    }

    return (rc);
}

int platform_connect (platform_socket_t socket, unsigned long addr,
                      unsigned short port)
{
    struct sockaddr_in dest;
    int rc;

    memset(&dest, 0, sizeof(dest));
    dest.sin_family = AF_INET;
    dest.sin_port = port;
    dest.sin_addr.S_un.S_addr = addr;

    rc = connect(socket, (struct sockaddr *)(&dest), sizeof(dest));
    printf("connect: rc= %d\n", rc);
    if (rc != 0) {
        rc = WSAGetLastError();

        PLAT_DEBUG("%s: %-25s: socket= %d: ERROR= %d:%s\n",
                   PLAT_ID, "connect", -1, rc,
                   platform_get_last_error_string(rc));
    }

    return (rc);
}

int platform_get_last_error (void)
{
    return (WSAGetLastError());
}

#if 0
static struct ErrorEntry {
	int nID;
	const char* pcMessage;
} gaErrorList[] = {
	{ 0,                     "No error" },
	{ WSAEINTR,              "Interrupted system call" },
	{ WSAEBADF,              "Bad file number" },
	{ WSAEACCES,             "Permission denied" },
	{ WSAEFAULT,             "Bad address" },
	{ WSAEINVAL,             "Invalid argument" },
	{ WSAEMFILE,             "Too many open sockets" },
	{ WSAEWOULDBLOCK,        "Operation would block" },
	{ WSAEINPROGRESS,        "Operation now in progress" },
	{ WSAEALREADY,           "Operation already in progress" },
	{ WSAENOTSOCK,           "Socket operation on non-socket" },
	{ WSAEDESTADDRREQ,       "Destination address required" },
	{ WSAEMSGSIZE,           "Message too long" },
	{ WSAEPROTOTYPE,         "Protocol wrong type for socket" },
	{ WSAENOPROTOOPT,        "Bad protocol option" },
	{ WSAEPROTONOSUPPORT,    "Protocol not supported" },
	{ WSAESOCKTNOSUPPORT,    "Socket type not supported" },
	{ WSAEOPNOTSUPP,         "Operation not supported on socket" },
	{ WSAEPFNOSUPPORT,       "Protocol family not supported" },
	{ WSAEAFNOSUPPORT,       "Address family not supported" },
	{ WSAEADDRINUSE,         "Address already in use" },
	{ WSAEADDRNOTAVAIL,      "Can't assign requested address" },
	{ WSAENETDOWN,           "Network is down" },
	{ WSAENETUNREACH,        "Network is unreachable" },
	{ WSAENETRESET,          "Net connection reset" },
	{ WSAECONNABORTED,       "Software caused connection abort" },
	{ WSAECONNRESET,         "Connection reset by peer" },
	{ WSAENOBUFS,            "No buffer space available" },
	{ WSAEISCONN,            "Socket is already connected" },
	{ WSAENOTCONN,           "Socket is not connected" },
	{ WSAESHUTDOWN,          "Can't send after socket shutdown" },
	{ WSAETOOMANYREFS,       "Too many references, can't splice" },
	{ WSAETIMEDOUT,          "Connection timed out" },
	{ WSAECONNREFUSED,       "Connection refused" },
	{ WSAELOOP,              "Too many levels of symbolic links" },
	{ WSAENAMETOOLONG,       "File name too long" },
	{ WSAEHOSTDOWN,          "Host is down" },
	{ WSAEHOSTUNREACH,       "No route to host" },
	{ WSAENOTEMPTY,          "Directory not empty" },
	{ WSAEPROCLIM,           "Too many processes" },
	{ WSAEUSERS,             "Too many users" },
	{ WSAEDQUOT,             "Disc quota exceeded" },
	{ WSAESTALE,             "Stale NFS file handle" },
	{ WSAEREMOTE,            "Too many levels of remote in path" },
	{ WSASYSNOTREADY,        "Network subsystem is unavailable" },
	{ WSAVERNOTSUPPORTED,    "Winsock version not supported" },
	{ WSANOTINITIALISED,     "Winsock not yet initialized" },
	{ WSAHOST_NOT_FOUND,     "Host not found" },
	{ WSATRY_AGAIN,          "Non-authoritative host not found" },
	{ WSANO_RECOVERY,        "Non-recoverable errors" },
	{ WSANO_DATA,            "Valid name, no data record of requested type" },
	{ WSAEDISCON,            "Graceful disconnect in progress" },
	{ WSASYSCALLFAILURE,     "System call failure" },
	{ WSA_NOT_ENOUGH_MEMORY, "Insufficient memory available" },
	{ WSA_OPERATION_ABORTED, "Overlapped operation aborted" },
	{ WSA_IO_INCOMPLETE,  	 "Overlapped I/O object not signalled" },
	{ WSA_IO_PENDING,        "Overlapped I/O will complete later" },
	//{ WSAINVALIDPROCTABLE,   "Invalid proc. table from service provider" },
	//{ WSAINVALIDPROVIDER,    "Invalid service provider version number" },
	//{ WSAPROVIDERFAILEDINIT, "Unable to init service provider" },
	{ WSA_INVALID_PARAMETER, "One or more parameters are invalid" },
	{ WSA_INVALID_HANDLE,    "Event object handle not valid" }
};
const int kNumMessages = sizeof(gaErrorList) / sizeof(ErrorEntry);


//// WSAGetLastErrorMessage ////////////////////////////////////////////
// A function similar in spirit to Unix's perror() that tacks a canned 
// interpretation of the value of WSAGetLastError() onto the end of a
// passed string, separated by a ": ".  Generally, you should implement
// smarter error handling than this, but for default cases and simple
// programs, this function is sufficient.
//
// This function returns a pointer to an internal static buffer, so you
// must copy the data from this function before you call it again.  It
// follows that this function is also not thread-safe.

const char* WSAGetLastErrorMessage(const char* pcMessagePrefix)
{
	// Build basic error string
	static char acErrorBuffer[256];
	ostrstream outs(acErrorBuffer, sizeof(acErrorBuffer));
	outs << pcMessagePrefix << ": ";

	// Tack appropriate canned message onto end of supplied message 
	// prefix -- if you want to make this faster, sort the table above
	// and do a binary search here instead.
	int nLastError = WSAGetLastError();
	int i;
	for (i = 0; i < kNumMessages; ++i) {
		if (gaErrorList[i].nID == nLastError) {
			outs << gaErrorList[i].pcMessage;
			break;
		}
	}
	if (i == kNumMessages) {
		// Didn't find error in list, so make up a generic one
		outs << "unknown error";
	}
	outs << " (" << nLastError << ")";

	// Finish error message off and return it.
	outs << ends;
	acErrorBuffer[sizeof(acErrorBuffer) - 1] = '\0';
	return acErrorBuffer;
}

#endif
typedef struct ErrorEntry_t {
	int  nID;
	char *pcMessage;
} ErrorEntry;

ErrorEntry gaErrorList[] = {
	{ 0,                     "No error" },
	{ WSAEINTR,              "Interrupted system call" },
	{ WSAEBADF,              "Bad file number" },
	{ WSAEACCES,             "Permission denied" },
	{ WSAEFAULT,             "Bad address" },
	{ WSAEINVAL,             "Invalid argument" },
	{ WSAEMFILE,             "Too many open sockets" },
	{ WSAEWOULDBLOCK,        "Operation would block" },
	{ WSAEINPROGRESS,        "Operation now in progress" },
	{ WSAEALREADY,           "Operation already in progress" },
	{ WSAENOTSOCK,           "Socket operation on non-socket" },
	{ WSAEDESTADDRREQ,       "Destination address required" },
	{ WSAEMSGSIZE,           "Message too long" },
	{ WSAEPROTOTYPE,         "Protocol wrong type for socket" },
	{ WSAENOPROTOOPT,        "Bad protocol option" },
	{ WSAEPROTONOSUPPORT,    "Protocol not supported" },
	{ WSAESOCKTNOSUPPORT,    "Socket type not supported" },
	{ WSAEOPNOTSUPP,         "Operation not supported on socket" },
	{ WSAEPFNOSUPPORT,       "Protocol family not supported" },
	{ WSAEAFNOSUPPORT,       "Address family not supported" },
	{ WSAEADDRINUSE,         "Address already in use" },
	{ WSAEADDRNOTAVAIL,      "Can't assign requested address" },
	{ WSAENETDOWN,           "Network is down" },
	{ WSAENETUNREACH,        "Network is unreachable" },
	{ WSAENETRESET,          "Net connection reset" },
	{ WSAECONNABORTED,       "Software caused connection abort" },
	{ WSAECONNRESET,         "Connection reset by peer" },
	{ WSAENOBUFS,            "No buffer space available" },
	{ WSAEISCONN,            "Socket is already connected" },
	{ WSAENOTCONN,           "Socket is not connected" },
	{ WSAESHUTDOWN,          "Can't send after socket shutdown" },
	{ WSAETOOMANYREFS,       "Too many references, can't splice" },
	{ WSAETIMEDOUT,          "Connection timed out" },
	{ WSAECONNREFUSED,       "Connection refused" },
	{ WSAELOOP,              "Too many levels of symbolic links" },
	{ WSAENAMETOOLONG,       "File name too long" },
	{ WSAEHOSTDOWN,          "Host is down" },
	{ WSAEHOSTUNREACH,       "No route to host" },
	{ WSAENOTEMPTY,          "Directory not empty" },
	{ WSAEPROCLIM,           "Too many processes" },
	{ WSAEUSERS,             "Too many users" },
	{ WSAEDQUOT,             "Disc quota exceeded" },
	{ WSAESTALE,             "Stale NFS file handle" },
	{ WSAEREMOTE,            "Too many levels of remote in path" },
	{ WSASYSNOTREADY,        "Network subsystem is unavailable" },
	{ WSAVERNOTSUPPORTED,    "Winsock version not supported" },
	{ WSANOTINITIALISED,     "Winsock not yet initialized" },
	{ WSAHOST_NOT_FOUND,     "Host not found" },
	{ WSATRY_AGAIN,          "Non-authoritative host not found" },
	{ WSANO_RECOVERY,        "Non-recoverable errors" },
	{ WSANO_DATA,            "Valid name, no data record of requested type" },
	{ WSAEDISCON,            "Graceful disconnect in progress" },
	{ WSASYSCALLFAILURE,     "System call failure" },
	{ WSA_NOT_ENOUGH_MEMORY, "Insufficient memory available" },
	{ WSA_OPERATION_ABORTED, "Overlapped operation aborted" },
	{ WSA_IO_INCOMPLETE,  	 "Overlapped I/O object not signalled" },
	{ WSA_IO_PENDING,        "Overlapped I/O will complete later" },
	//{ WSAINVALIDPROCTABLE,   "Invalid proc. table from service provider" },
	//{ WSAINVALIDPROVIDER,    "Invalid service provider version number" },
	//{ WSAPROVIDERFAILEDINIT, "Unable to init service provider" },
	{ WSA_INVALID_PARAMETER, "One or more parameters are invalid" },
	{ WSA_INVALID_HANDLE,    "Event object handle not valid" }
};
const int kNumMessages = sizeof(gaErrorList) / sizeof(ErrorEntry);


//// WSAGetLastErrorMessage ////////////////////////////////////////////
// A function similar in spirit to Unix's perror() that tacks a canned 
// interpretation of the value of WSAGetLastError() onto the end of a
// passed string, separated by a ": ".  Generally, you should implement
// smarter error handling than this, but for default cases and simple
// programs, this function is sufficient.
//
// This function returns a pointer to an internal static buffer, so you
// must copy the data from this function before you call it again.  It
// follows that this function is also not thread-safe.

//const char* WSAGetLastErrorMessage(const char* pcMessagePrefix)
char *platform_get_last_error_string (int error)
{
	int i;

    for (i = 0; i < kNumMessages; ++i) {
		if (gaErrorList[i].nID == error) {
			return (gaErrorList[i].pcMessage);
		}
	}

    return ("unknown error");
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
