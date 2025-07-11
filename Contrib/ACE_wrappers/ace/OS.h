// -*- C++ -*-

//=============================================================================
/**
 *  @file   OS.h
 *
 *  OS.h,v 4.1158 2002/07/17 05:33:16 dhinton Exp
 *
 *  @author Douglas C. Schmidt <schmidt@cs.wustl.edu>
 *  @author Jesper S. M|ller<stophph@diku.dk>
 *  @author and a cast of thousands...
 */
//=============================================================================

#ifndef ACE_OS_H
#define ACE_OS_H

#include "ace/pre.h"

#include "ace/config-all.h"

#if defined (ACE_HAS_VIRTUAL_TIME)
#include /**/ <sys/times.h>
#endif /*ACE_HAS_VIRTUAL_TIME*/

#if !defined (ACE_LACKS_PRAGMA_ONCE)
# pragma once
#endif /* ACE_LACKS_PRAGMA_ONCE */

// Include the split up ACE_OS classes
#include "ace/OS_Dirent.h"
#include "ace/OS_String.h"
#include "ace/OS_Memory.h"
#include "ace/OS_TLI.h"
#include "ace/OS_Errno.h"

#include "ace/Time_Value.h"

class ACE_Timeout_Manager;

#if !defined (_SC_AIO_MAX)
#define _SC_AIO_MAX 1
#endif /* _SC_AIO_MAX */

// Do not change these values wantonly since GPERF depends on them..
#define ACE_ASCII_SIZE 128
#define ACE_EBCDIC_SIZE 256

#if 'a' < 'A'
#define ACE_HAS_EBCDIC
#define ACE_STANDARD_CHARACTER_SET_SIZE 256
#else
#define ACE_HAS_ASCII
#define ACE_STANDARD_CHARACTER_SET_SIZE 128
#endif /* 'a' < 'A' */

# if defined (ACE_PSOS_TM)
typedef long long longlong_t;
typedef long      id_t;
# endif /* ACE_PSOS_TM */

// Deal with MSVC++ insanity for CORBA...
# if defined (ACE_HAS_BROKEN_NAMESPACES)
#   define ACE_CORBA_1(NAME) CORBA_##NAME
#   define ACE_CORBA_2(TYPE, NAME) CORBA_##TYPE##_##NAME
#   define ACE_CORBA_3(TYPE, NAME) CORBA_##TYPE::NAME
#   define ACE_NESTED_CLASS(TYPE, NAME) NAME
# else  /* ! ACE_HAS_BROKEN_NAMESPACES */
#   define ACE_CORBA_1(NAME) CORBA::NAME
#   define ACE_CORBA_2(TYPE, NAME) CORBA::TYPE::NAME
#   define ACE_CORBA_3(TYPE, NAME) CORBA::TYPE::NAME
#   define ACE_NESTED_CLASS(TYPE, NAME) TYPE::NAME
# endif /* ! ACE_HAS_BROKEN_NAMESPACES */

// Here are all ACE-specific default constants, needed throughout ACE
// and its applications.  The values can be over written by user
// specific values in config.h files.
#include "ace/Default_Constants.h"

# if defined (ACE_HAS_4_4BSD_SENDMSG_RECVMSG)
    // Control message size to pass a file descriptor.
#   define ACE_BSD_CONTROL_MSG_LEN sizeof (struct cmsghdr) + sizeof (ACE_HANDLE)
#   if defined (ACE_LACKS_CMSG_DATA_MACRO)
#     if defined (ACE_LACKS_CMSG_DATA_MEMBER)
#       define CMSG_DATA(cmsg) ((unsigned char *) ((struct cmsghdr *) (cmsg) + 1))
#     else
#       define CMSG_DATA(cmsg) ((cmsg)->cmsg_data)
#     endif /* ACE_LACKS_CMSG_DATA_MEMBER */
#   endif /* ACE_LACKS_CMSG_DATA_MACRO */
# endif /* ACE_HAS_4_4BSD_SENDMSG_RECVMSG */


// Default size of the ACE Reactor.
# if defined (FD_SETSIZE)
int const ACE_FD_SETSIZE = FD_SETSIZE;
# else
#   define ACE_FD_SETSIZE FD_SETSIZE
# endif /* ACE_FD_SETSIZE */

# if !defined (ACE_DEFAULT_SELECT_REACTOR_SIZE)
#   define ACE_DEFAULT_SELECT_REACTOR_SIZE ACE_FD_SETSIZE
# endif /* ACE_DEFAULT_SELECT_REACTOR_SIZE */


// Here are all ACE-specific global declarations needed throughout
// ACE.
#include "ace/Global_Macros.h"

#if !defined (ACE_WIN32)
#define ACE_MAX_USERID L_cuserid
#endif /*!ACE_WIN32*/

// include the ACE min()/max() functions.
# include "ace/Min_Max.h"


// These hooks enable ACE to have all dynamic memory management
// automatically handled on a per-object basis.

# if defined (ACE_LACKS_KEY_T)
#   if defined (ACE_WIN32)
   // Win32 doesn't use numeric values to name its semaphores, it uses
   // strings!
typedef char *key_t;
#   else
typedef int key_t;
#   endif /* ACE_WIN32 */
# endif /* ACE_LACKS_KEY_T */

///////////////////////////////////////////
//                                       //
// NOTE: Please do not add any #includes //
//       before this point.  On VxWorks, //
//       vxWorks.h must be #included     //
//       first!                          //
//                                       //
///////////////////////////////////////////

# if defined (ACE_PSOS)

#   if defined (ACE_LACKS_ASSERT_MACRO)
#     define assert(expr)
#   endif

#   if defined (ACE_PSOSIM)

#     include /**/ "ace/sys_conf.h" /* system configuration file */
#     include /**/ <psos.h>         /* pSOS+ system calls                */
#     include /**/ <pna.h>          /* pNA+ TCP/IP Network Manager calls */

    /* In the *simulator* environment, use unsigned int for size_t */
#     define size_t  unsigned int


    /*   include <rpc.h>       pRPC+ Remote Procedure Call Library calls   */
    /*                         are not supported by pSOSim                 */
    /*                                                                     */
    /*   include <phile.h>     pHILE+ file system calls are not supported  */
    /*                         by pSOSim *so*, for the time being, we make */
    /*                         use of UNIX file system headers and then    */
    /*                         when we have time, we wrap UNIX file system */
    /*                         calls w/ pHILE+ wrappers, and modify ACE to */
    /*                         use the wrappers under pSOSim               */

    /* put includes for necessary UNIX file system calls here */
#     include /**/ <sys/stat.h>
#     include /**/ <sys/ioctl.h>
#     include /**/ <sys/sockio.h>
#     include /**/ <netinet/tcp.h>

#     define TCP_
#     if ! defined (BUFSIZ)
#       define BUFSIZ 1024
#     endif  /* ! defined (BUFSIZ) */


#   else

#     if defined (ACE_PSOS_CANT_USE_SYS_TYPES)
      // these are missing from the pSOS types.h file, and the compiler
      // supplied types.h file collides with the pSOS version.
#       if !defined (ACE_SHOULD_NOT_DEFINE_SYS_TYPES)
      typedef unsigned char     u_char;
      typedef unsigned short    u_short;
#       endif /* ACE_SHOULD_NOT_DEFINE_SYS_TYPES */
      typedef unsigned int      u_int;
#       if !defined (ACE_SHOULD_NOT_DEFINE_SYS_TYPES)
      typedef unsigned long     u_long;
#       endif /* ACE_SHOULD_NOT_DEFINE_SYS_TYPES */

      // These are defined in types.h included by (among others) pna.h
#       if 0
      typedef unsigned char     uchar_t;
      typedef unsigned short    ushort_t;
      typedef unsigned int      uint_t;
      typedef unsigned long     ulong_t;
#       endif /* 0 */
      typedef char *            caddr_t;

#       if defined (ACE_PSOS_DIAB_PPC)
      // pid_t is defined in sys/types.h
#         if 0
      typedef unsigned long pid_t;
#         endif /* 0 */
#     define ACE_INVALID_PID ((pid_t) ~0)
#       else /* !defined (ACE_PSOS_DIAB_PPC) */
      typedef long pid_t;
#     define ACE_INVALID_PID ((pid_t) -1)
#       endif /* defined (ACE_PSOS_DIAB_PPC) */

//      typedef unsigned char wchar_t;
#     endif /* ACE_PSOS_CANT_USE_SYS_TYPES */

#     include /**/ "ace/sys_conf.h" /* system configuration file */
#     include /**/ <configs.h>   /* includes all pSOS headers */
//    #include /**/ <psos.h>    /* pSOS system calls */
#     include /**/ <pna.h>      /* pNA+ TCP/IP Network Manager calls */
#     include /**/ <phile.h>     /* pHILE+ file system calls */
//    #include /**/ <prepccfg.h>     /* pREPC+ file system calls */
#     if defined (ACE_PSOS_DIAB_MIPS)
#       if defined (ACE_PSOS_USES_DIAB_SYS_CALLS)
#         include /**/ <unistd.h>    /* Diab Data supplied file system calls */
#       else
#         include /**/ <prepc.h>
#       endif /* ACE_PSOS_USES_DIAB_SYS_CALLS */
#       include /**/ <sys/wait.h>    /* Diab Data supplied header file */
#     endif /* ACE_PSOS_DIAB_MIPS */

// This collides with phile.h
//    #include /**/ <sys/stat.h>    /* Diab Data supplied header file */

// Some versions have missing preprocessor definitions
#     if !defined (AF_UNIX)
#       define AF_UNIX 0x1
#     endif /* AF_UNIX */
#     define PF_UNIX AF_UNIX
#     define PF_INET AF_INET
#     if !defined (AF_MAX)
#       define AF_MAX AF_INET
#     endif /* AF_MAX */
#     if !defined (IFF_LOOPBACK)
#       define IFF_LOOPBACK IFF_EXTLOOPBACK
#     endif /* IFF_LOOPBACK */

  typedef long fd_mask;
#     define IPPORT_RESERVED       1024
#     define IPPORT_USERRESERVED   5000

#     if !defined (howmany)
#       define howmany(x, y) (((x)+((y)-1))/(y))
#     endif /* howmany */

  extern "C"
  {
    typedef void (* ACE_SignalHandler) (void);
    typedef void (* ACE_SignalHandlerV) (void);
  }

#     if !defined(SIG_DFL)
#       define SIG_DFL (ACE_SignalHandler) 0
#     endif  /* philabs */

#   endif /* defined (ACE_PSOSIM) */

// Some versions of pSOS do not define error numbers, but newer
// versions do. So, include errno.h and then see which ones are not
// yet defined.
#   include /**/ <errno.h>

#   if !defined (EPERM)
#     define EPERM        1        /* Not super-user                        */
#   endif /* EPERM */
#   if !defined (ENOENT)
#     define ENOENT       2        /* No such file or directory             */
#   endif /* ENOENT */
#   if !defined (ESRCH)
#     define ESRCH        3        /* No such process                       */
#   endif /* ESRCH */
#   if ! defined (EINTR)
#     define EINTR        4        /* interrupted system call               */
#   endif /* EINTR */
#   if !defined (EBADF)
#     define EBADF        9        /* Bad file number                       */
#   endif /* EBADF */
#   if !defined (EAGAIN)
#     define EAGAIN       11       /* Resource temporarily unavailable      */
#   endif /* EAGAIN */
#   if !defined (EWOULDBLOCK)
#     define EWOULDBLOCK  EAGAIN   /* Blocking resource request would block */
#   endif /* EWOULDBLOCK */
#   if !defined (ENOMEM)
#     define ENOMEM       12       /* Not enough core                       */
#   endif /* ENOMEM */
#   if !defined (EACCES)
#     define EACCES      13       /* Permission denied                     */
#   endif /* EACCES */
#   if !defined (EFAULT)
#     define EFAULT       14       /* Bad access                            */
#   endif /* EFAULT */
#   if !defined (EEXIST)
#     define EEXIST       17       /* File exists                           */
#   endif /* EEXIST */
#   if !defined (ENOSPC)
#     define ENOSPC       28       /* No space left on device               */
#   endif /* ENOSPC */
#   if !defined (EPIPE)
#     define EPIPE        32       /* Broken pipe                           */
#   endif /* EPIPE */
#   if !defined (ETIME)
#     define ETIME        62       /* timer expired                         */
#   endif /* ETIME */
#   if !defined (ENAMETOOLONG)
#     define ENAMETOOLONG 78       /* path name is too long                 */
#   endif /* ENAMETOOLONG */
#   if !defined (ENOSYS)
#     define ENOSYS       89       /* Unsupported file system operation     */
#   endif /* ENOSYS */
#   if !defined (EADDRINUSE)
#     define EADDRINUSE   125      /* Address already in use                */
#   endif /* EADDRINUSE */
#   if !defined (ENETUNREACH)
#     define ENETUNREACH  128      /* Network is unreachable                */
#   endif /* ENETUNREACH */
#   if !defined (EISCONN)
#     define EISCONN      133      /* Socket is already connected           */
#   endif /* EISCONN */
#   if !defined (ESHUTDOWN)
#     define ESHUTDOWN    143      /* Can't send after socket shutdown      */
#   endif /* ESHUTDOWN */
#   if !defined (ECONNREFUSED)
#     define ECONNREFUSED 146      /* Connection refused                    */
#   endif /* ECONNREFUSED */
#   if !defined (EINPROGRESS)
#     define EINPROGRESS  150      /* operation now in progress             */
#   endif /* EINPROGRESS */
#   if !defined (ERRMAX)
#     define ERRMAX       151      /* Last error number                     */
#   endif /* ERRMAX */

#   if ! defined (NSIG)
#     define NSIG 32
#   endif /* NSIG */

#   if ! defined (TCP_NODELAY)
#     define TCP_NODELAY  1
#   endif /* TCP_NODELAY */

// For general purpose portability

#   define ACE_BITS_PER_ULONG (8 * sizeof (u_long))

typedef u_long ACE_idtype_t;
typedef u_long ACE_id_t;
#   define ACE_SELF (0)
typedef u_long ACE_pri_t;

// pHILE+ calls the DIR struct XDIR instead
#    if !defined (ACE_PSOS_DIAB_PPC)
typedef XDIR ACE_DIR;
#    endif /* !defined (ACE_PSOS_DIAB_PPC) */

// Use pSOS semaphores, wrapped . . .
typedef struct
{
  /// Semaphore handle.  This is allocated by pSOS.
  u_long sema_;

  /// Name of the semaphore: really a 32 bit number to pSOS
  char name_[4];
} ACE_sema_t;

// Used for dynamic linking.
#   if !defined (ACE_DEFAULT_SVC_CONF)
#     if (ACE_USES_CLASSIC_SVC_CONF == 1)
#       define ACE_DEFAULT_SVC_CONF "./svc.conf"
#     else
#       define ACE_DEFAULT_SVC_CONF "./svc.conf.xml"
#     endif /* ACE_USES_CLASSIC_SVC_CONF ==1 */
#   endif /* ACE_DEFAULT_SVC_CONF */

#   if !defined (ACE_DEFAULT_SEM_KEY)
#     define ACE_DEFAULT_SEM_KEY 1234
#   endif /* ACE_DEFAULT_SEM_KEY */

#   define ACE_STDIN 0
#   define ACE_STDOUT 1
#   define ACE_STDERR 2

#   define ACE_DIRECTORY_SEPARATOR_STR_A "/"
#   define ACE_DIRECTORY_SEPARATOR_CHAR_A '/'
#   define ACE_PLATFORM_A "pSOS"
#   define ACE_PLATFORM_EXE_SUFFIX_A ""

#   define ACE_DLL_SUFFIX ACE_LIB_TEXT (".so")
#   define ACE_DLL_PREFIX ACE_LIB_TEXT ("lib")
#   define ACE_LD_SEARCH_PATH ACE_LIB_TEXT ("LD_LIBRARY_PATH")
#   define ACE_LD_SEARCH_PATH_SEPARATOR_STR ACE_LIB_TEXT (":")
#   define ACE_LOGGER_KEY ACE_LIB_TEXT ("/tmp/server_daemon")

#   define ACE_MAX_DEFAULT_PORT 65535

#   if ! defined(MAXPATHLEN)
#     define MAXPATHLEN  1024
#   endif /* MAXPATHLEN */

#   if ! defined(MAXNAMLEN)
#     define MAXNAMLEN   255
#   endif /* MAXNAMLEN */

#   if defined (ACE_LACKS_MMAP)
#     define PROT_READ 0
#     define PROT_WRITE 0
#     define PROT_EXEC 0
#     define PROT_NONE 0
#     define PROT_RDWR 0
#     define MAP_PRIVATE 0
#     define MAP_SHARED 0
#     define MAP_FIXED 0
#   endif /* ACE_LACKS_MMAP */

typedef int ACE_exitcode;

typedef ACE_HANDLE ACE_SHLIB_HANDLE;
#   define ACE_SHLIB_INVALID_HANDLE ACE_INVALID_HANDLE
#   define ACE_DEFAULT_SHLIB_MODE 0

#   define ACE_INVALID_SEM_KEY -1

struct  hostent {
  char    *h_name;        /* official name of host */
  char    **h_aliases;    /* alias list */
  int     h_addrtype;     /* host address type */
  int     h_length;       /* address length */
  char    **h_addr_list;  /* (first, only) address from name server */
#   define h_addr h_addr_list[0]   /* the first address */
};

struct  servent {
  char     *s_name;    /* official service name */
  char    **s_aliases; /* alias list */
  int       s_port;    /* port # */
  char     *s_proto;   /* protocol to use */
};

#   define ACE_SEH_TRY if (1)
#   define ACE_SEH_EXCEPT(X) while (0)
#   define ACE_SEH_FINALLY if (1)

#   if !defined (LPSECURITY_ATTRIBUTES)
#     define LPSECURITY_ATTRIBUTES int
#   endif /* !defined LPSECURITY_ATTRIBUTES */
#   if !defined (GENERIC_READ)
#     define GENERIC_READ 0
#   endif /* !defined GENERIC_READ */
#   if !defined (FILE_SHARE_READ)
#     define FILE_SHARE_READ 0
#   endif /* !defined FILE_SHARE_READ */
#   if !defined (OPEN_EXISTING)
#     define OPEN_EXISTING 0
#   endif /* !defined OPEN_EXISTING */
#   if !defined (FILE_ATTRIBUTE_NORMAL)
#     define FILE_ATTRIBUTE_NORMAL 0
#   endif /* !defined FILE_ATTRIBUTE_NORMAL */
#   if !defined (MAXIMUM_WAIT_OBJECTS)
#     define MAXIMUM_WAIT_OBJECTS 0
#   endif /* !defined MAXIMUM_WAIT_OBJECTS */
#   if !defined (FILE_FLAG_OVERLAPPED)
#     define FILE_FLAG_OVERLAPPED 0
#   endif /* !defined FILE_FLAG_OVERLAPPED */
#   if !defined (FILE_FLAG_SEQUENTIAL_SCAN)
#     define FILE_FLAG_SEQUENTIAL_SCAN 0
#   endif /* !defined FILE_FLAG_SEQUENTIAL_SCAN */

struct ACE_OVERLAPPED
{
  u_long Internal;
  u_long InternalHigh;
  u_long Offset;
  u_long OffsetHigh;
  ACE_HANDLE hEvent;
};

#   if !defined (USER_INCLUDE_SYS_TIME_TM)
#     if defined (ACE_PSOS_DIAB_PPC)
typedef struct timespec timespec_t;
#     else /* ! defined (ACE_PSOS_DIAB_PPC) */
typedef struct timespec
{
  time_t tv_sec; // Seconds
  long tv_nsec; // Nanoseconds
} timespec_t;
#     endif /* defined (ACE_PSOS_DIAB_PPC) */
#   endif /*  !defined (USER_INCLUDE_SYS_TIME_TM) */

#if defined (ACE_PSOS_HAS_TIME)

// Use pSOS time, wrapped . . .
class ACE_OS_Export ACE_PSOS_Time_t
{
public:
    /// default ctor: date, time, and ticks all zeroed.
  ACE_PSOS_Time_t (void);

    /// ctor from a timespec_t
  ACE_PSOS_Time_t (const timespec_t& t);

    /// type cast operator (to a timespec_t)
  operator timespec_t ();

    /// static member function to get current system time
  static u_long get_system_time (ACE_PSOS_Time_t& t);

    /// static member function to set current system time
  static u_long set_system_time (const ACE_PSOS_Time_t& t);

#   if defined (ACE_PSOSIM)
    /// static member function to initialize system time, using UNIX calls
  static u_long init_simulator_time (void);
#   endif /* ACE_PSOSIM */

    /// max number of ticks supported in a single system call
  static const u_long max_ticks;
private:
  // = Constants for prying info out of the pSOS time encoding.
  static const u_long year_mask;
  static const u_long month_mask;
  static const u_long day_mask;
  static const u_long hour_mask;
  static const u_long minute_mask;
  static const u_long second_mask;
  static const int year_shift;
  static const int month_shift;
  static const int hour_shift;
  static const int minute_shift;
  static const int year_origin;
  static const int month_origin;

  // error codes
  static const u_long err_notime;   // system time not set
  static const u_long err_illdate;  // date out of range
  static const u_long err_illtime;  // time out of range
  static const u_long err_illticks; // ticks out of range

  /// date : year in bits 31-16, month in bits 15-8, day in bits 7-0
   u_long date_;

  /// time : hour in bits 31-16, minutes in bits 15-8, seconds in bits 7-0
  u_long time_;

  /// ticks: number of system clock ticks (KC_TICKS2SEC-1 max)
  u_long ticks_;
} ;
#endif /* ACE_PSOS_HAS_TIME */

# endif /* defined (ACE_PSOS) */

# if defined (ACE_HAS_CHARPTR_SPRINTF)
#   define ACE_SPRINTF_ADAPTER(X) ::strlen (X)
# else
#   define ACE_SPRINTF_ADAPTER(X) X
# endif /* ACE_HAS_CHARPTR_SPRINTF */

// Default address for shared memory mapped files and SYSV shared memory
// (defaults to 64 M).
# if !defined (ACE_DEFAULT_BASE_ADDR)
#   define ACE_DEFAULT_BASE_ADDR ((char *) (64 * 1024 * 1024))
# endif /* ACE_DEFAULT_BASE_ADDR */

// This fudge factor can be overriden for timers that need it, such as on
// Solaris, by defining the ACE_TIMER_SKEW symbol in the appropriate config
// header.
#if !defined (ACE_TIMER_SKEW)
#  define ACE_TIMER_SKEW 0
#endif /* ACE_TIMER_SKEW */

// This needs to go here *first* to avoid problems with AIX.
# if defined (ACE_HAS_PTHREADS)
extern "C" {
#   define ACE_DONT_INCLUDE_ACE_SIGNAL_H
#     include /**/ <signal.h>
#   undef ACE_DONT_INCLUDE_ACE_SIGNAL_H
#   include /**/ <pthread.h>
#   if defined (DIGITAL_UNIX)
#     define pthread_self __pthread_self
extern "C" pthread_t pthread_self (void);
#   endif /* DIGITAL_UNIX */
}
#   if defined (HPUX_10)
//    HP-UX 10 needs to see cma_sigwait, and since _CMA_NOWRAPPERS_ is defined,
//    this header does not get included from pthreads.h.
#     include /**/ <dce/cma_sigwait.h>
#   endif /* HPUX_10 */
# endif /* ACE_HAS_PTHREADS */

// There are a lot of threads-related macro definitions in the config files.
// They came in at different times and from different places and platform
// requirements as threads evolved.  They are probably not all needed - some
// overlap or are otherwise confused.  This is an attempt to start
// straightening them out.
# if defined (ACE_HAS_PTHREADS_STD)    /* POSIX.1c threads (pthreads) */
  // ... and 2-parameter asctime_r and ctime_r
#   if !defined (ACE_HAS_2_PARAM_ASCTIME_R_AND_CTIME_R) && \
       !defined (ACE_HAS_STHREADS)
#     define ACE_HAS_2_PARAM_ASCTIME_R_AND_CTIME_R
#   endif
# endif /* ACE_HAS_PTHREADS_STD */

// By default we perform no tracing on the OS layer, otherwise the
// coupling between the OS layer and Log_Msg is too tight.  But the
// application can override the default if they wish to.
# if !defined(ACE_OS_TRACE)
#  define ACE_OS_TRACE(X)
# endif /* ACE_OS_TRACE */

# if defined (ACE_USES_STD_NAMESPACE_FOR_STDC_LIB) && \
             (ACE_USES_STD_NAMESPACE_FOR_STDC_LIB != 0)
using std::time_t;
using std::tm;
# if defined (ACE_WIN32)
using std::_timezone;
# else
using std::timezone;
# endif
using std::difftime;
# endif /* ACE_USES_STD_NAMESPACE_FOR_STDC_LIB */

# if !defined (ACE_HAS_CLOCK_GETTIME) && !(defined (_CLOCKID_T_) || defined (_CLOCKID_T))
typedef int clockid_t;
#   if !defined (CLOCK_REALTIME)
#     define CLOCK_REALTIME 0
#   endif /* CLOCK_REALTIME */
# endif /* ! ACE_HAS_CLOCK_GETTIME && ! _CLOCKID_T_ */

#if !defined (E2BIG)
#  define E2BIG 7
#endif /* E2BIG */

/**
 * @class ACE_Countdown_Time
 *
 * @brief Keeps track of the amount of elapsed time.
 *
 * This class has a side-effect on the <max_wait_time> -- every
 * time the <stop> method is called the <max_wait_time> is
 * updated.
 */
class ACE_OS_Export ACE_Countdown_Time
{
public:
  // = Initialization and termination methods.
  /// Cache the <max_wait_time> and call <start>.
  ACE_Countdown_Time (ACE_Time_Value *max_wait_time);

  /// Call <stop>.
  ~ACE_Countdown_Time (void);

  /// Cache the current time and enter a start state.
  int start (void);

  /// Subtract the elapsed time from max_wait_time_ and enter a stopped
  /// state.
  int stop (void);

  /// Calls stop and then start.  max_wait_time_ is modified by the
  /// call to stop.
  int update (void);

  /// Returns 1 if we've already been stopped, else 0.
  int stopped (void) const;

private:
  /// Maximum time we were willing to wait.
  ACE_Time_Value *max_wait_time_;

  /// Beginning of the start time.
  ACE_Time_Value start_time_;

  /// Keeps track of whether we've already been stopped.
  int stopped_;
};

# if defined (ACE_HAS_USING_KEYWORD)
#   define ACE_USING using
# else
#   define ACE_USING
# endif /* ACE_HAS_USING_KEYWORD */

# if defined (ACE_HAS_TYPENAME_KEYWORD)
#   define ACE_TYPENAME typename
# else
#   define ACE_TYPENAME
# endif /* ACE_HAS_TYPENAME_KEYWORD */

# if defined (ACE_HAS_STD_TEMPLATE_SPECIALIZATION)
#   define ACE_TEMPLATE_SPECIALIZATION template<>
# else
#   define ACE_TEMPLATE_SPECIALIZATION
# endif /* ACE_HAS_STD_TEMPLATE_SPECIALIZATION */

# if defined (ACE_HAS_STD_TEMPLATE_METHOD_SPECIALIZATION)
#   define ACE_TEMPLATE_METHOD_SPECIALIZATION template<>
# else
#   define ACE_TEMPLATE_METHOD_SPECIALIZATION
# endif /* ACE_HAS_STD_TEMPLATE_SPECIALIZATION */

// The following is necessary since many C++ compilers don't support
// typedef'd types inside of classes used as formal template
// arguments... ;-(.  Luckily, using the C++ preprocessor I can hide
// most of this nastiness!

# if defined (ACE_HAS_TEMPLATE_TYPEDEFS)

// Handle ACE_Message_Queue.
#   define ACE_SYNCH_DECL class _ACE_SYNCH
#   define ACE_SYNCH_USE _ACE_SYNCH
#   define ACE_SYNCH_MUTEX_T ACE_TYPENAME _ACE_SYNCH::MUTEX
#   define ACE_SYNCH_CONDITION_T ACE_TYPENAME _ACE_SYNCH::CONDITION
#   define ACE_SYNCH_SEMAPHORE_T ACE_TYPENAME _ACE_SYNCH::SEMAPHORE

// Handle ACE_Malloc*
#   define ACE_MEM_POOL_1 class _ACE_MEM_POOL
#   define ACE_MEM_POOL_2 _ACE_MEM_POOL
#   define ACE_MEM_POOL _ACE_MEM_POOL
#   define ACE_MEM_POOL_OPTIONS ACE_TYPENAME _ACE_MEM_POOL::OPTIONS

// Handle ACE_Svc_Handler
#   define ACE_PEER_STREAM_1 class _ACE_PEER_STREAM
#   define ACE_PEER_STREAM_2 _ACE_PEER_STREAM
#   define ACE_PEER_STREAM _ACE_PEER_STREAM
#   define ACE_PEER_STREAM_ADDR ACE_TYPENAME _ACE_PEER_STREAM::PEER_ADDR

// Handle ACE_Acceptor
#   define ACE_PEER_ACCEPTOR_1 class _ACE_PEER_ACCEPTOR
#   define ACE_PEER_ACCEPTOR_2 _ACE_PEER_ACCEPTOR
#   define ACE_PEER_ACCEPTOR _ACE_PEER_ACCEPTOR
#   define ACE_PEER_ACCEPTOR_ADDR ACE_TYPENAME _ACE_PEER_ACCEPTOR::PEER_ADDR

// Handle ACE_Connector
#   define ACE_PEER_CONNECTOR_1 class _ACE_PEER_CONNECTOR
#   define ACE_PEER_CONNECTOR_2 _ACE_PEER_CONNECTOR
#   define ACE_PEER_CONNECTOR _ACE_PEER_CONNECTOR
#   define ACE_PEER_CONNECTOR_ADDR ACE_TYPENAME _ACE_PEER_CONNECTOR::PEER_ADDR
#   if !defined(ACE_HAS_TYPENAME_KEYWORD)
#     define ACE_PEER_CONNECTOR_ADDR_ANY ACE_PEER_CONNECTOR_ADDR::sap_any
#   else
    //
    // If the compiler supports 'typename' we cannot use
    //
    // PEER_CONNECTOR::PEER_ADDR::sap_any
    //
    // because PEER_CONNECTOR::PEER_ADDR is not considered a type. But:
    //
    // typename PEER_CONNECTOR::PEER_ADDR::sap_any
    //
    // will not work either, because now we are declaring sap_any a
    // type, further:
    //
    // (typename PEER_CONNECTOR::PEER_ADDR)::sap_any
    //
    // is considered a casting expression. All I can think of is using a
    // typedef, I tried PEER_ADDR but that was a source of trouble on
    // some platforms. I will try:
    //
#     define ACE_PEER_CONNECTOR_ADDR_ANY ACE_PEER_ADDR_TYPEDEF::sap_any
#   endif /* ACE_HAS_TYPENAME_KEYWORD */

// Handle ACE_SOCK_*
#   define ACE_SOCK_ACCEPTOR ACE_SOCK_Acceptor
#   define ACE_SOCK_CONNECTOR ACE_SOCK_Connector
#   define ACE_SOCK_STREAM ACE_SOCK_Stream

// Handle ACE_MEM_*
#   define ACE_MEM_ACCEPTOR ACE_MEM_Acceptor
#   define ACE_MEM_CONNECTOR ACE_MEM_Connector
#   define ACE_MEM_STREAM ACE_MEM_Stream

// Handle ACE_LSOCK_*
#   define ACE_LSOCK_ACCEPTOR ACE_LSOCK_Acceptor
#   define ACE_LSOCK_CONNECTOR ACE_LSOCK_Connector
#   define ACE_LSOCK_STREAM ACE_LSOCK_Stream

// Handle ACE_TLI_*
#   define ACE_TLI_ACCEPTOR ACE_TLI_Acceptor
#   define ACE_TLI_CONNECTOR ACE_TLI_Connector
#   define ACE_TLI_STREAM ACE_TLI_Stream

// Handle ACE_SPIPE_*
#   define ACE_SPIPE_ACCEPTOR ACE_SPIPE_Acceptor
#   define ACE_SPIPE_CONNECTOR ACE_SPIPE_Connector
#   define ACE_SPIPE_STREAM ACE_SPIPE_Stream

// Handle ACE_UPIPE_*
#   define ACE_UPIPE_ACCEPTOR ACE_UPIPE_Acceptor
#   define ACE_UPIPE_CONNECTOR ACE_UPIPE_Connector
#   define ACE_UPIPE_STREAM ACE_UPIPE_Stream

// Handle ACE_FILE_*
#   define ACE_FILE_CONNECTOR ACE_FILE_Connector
#   define ACE_FILE_STREAM ACE_FILE_IO

// Handle ACE_*_Memory_Pool.
#   define ACE_MMAP_MEMORY_POOL ACE_MMAP_Memory_Pool
#   define ACE_LITE_MMAP_MEMORY_POOL ACE_Lite_MMAP_Memory_Pool
#   define ACE_SBRK_MEMORY_POOL ACE_Sbrk_Memory_Pool
#   define ACE_SHARED_MEMORY_POOL ACE_Shared_Memory_Pool
#   define ACE_LOCAL_MEMORY_POOL ACE_Local_Memory_Pool
#   define ACE_PAGEFILE_MEMORY_POOL ACE_Pagefile_Memory_Pool

# else /* TEMPLATES are broken in some form or another (i.e., most C++ compilers) */

// Handle ACE_Message_Queue.
#   if defined (ACE_HAS_OPTIMIZED_MESSAGE_QUEUE)
#     define ACE_SYNCH_DECL class _ACE_SYNCH_MUTEX_T, class _ACE_SYNCH_CONDITION_T, class _ACE_SYNCH_SEMAPHORE_T
#     define ACE_SYNCH_USE _ACE_SYNCH_MUTEX_T, _ACE_SYNCH_CONDITION_T, _ACE_SYNCH_SEMAPHORE_T
#   else
#     define ACE_SYNCH_DECL class _ACE_SYNCH_MUTEX_T, class _ACE_SYNCH_CONDITION_T
#     define ACE_SYNCH_USE _ACE_SYNCH_MUTEX_T, _ACE_SYNCH_CONDITION_T
#   endif /* ACE_HAS_OPTIMIZED_MESSAGE_QUEUE */
#   define ACE_SYNCH_MUTEX_T _ACE_SYNCH_MUTEX_T
#   define ACE_SYNCH_CONDITION_T _ACE_SYNCH_CONDITION_T
#   define ACE_SYNCH_SEMAPHORE_T _ACE_SYNCH_SEMAPHORE_T

// Handle ACE_Malloc*
#   define ACE_MEM_POOL_1 class _ACE_MEM_POOL, class _ACE_MEM_POOL_OPTIONS
#   define ACE_MEM_POOL_2 _ACE_MEM_POOL, _ACE_MEM_POOL_OPTIONS
#   define ACE_MEM_POOL _ACE_MEM_POOL
#   define ACE_MEM_POOL_OPTIONS _ACE_MEM_POOL_OPTIONS

// Handle ACE_Svc_Handler
#   define ACE_PEER_STREAM_1 class _ACE_PEER_STREAM, class _ACE_PEER_ADDR
#   define ACE_PEER_STREAM_2 _ACE_PEER_STREAM, _ACE_PEER_ADDR
#   define ACE_PEER_STREAM _ACE_PEER_STREAM
#   define ACE_PEER_STREAM_ADDR _ACE_PEER_ADDR

// Handle ACE_Acceptor
#   define ACE_PEER_ACCEPTOR_1 class _ACE_PEER_ACCEPTOR, class _ACE_PEER_ADDR
#   define ACE_PEER_ACCEPTOR_2 _ACE_PEER_ACCEPTOR, _ACE_PEER_ADDR
#   define ACE_PEER_ACCEPTOR _ACE_PEER_ACCEPTOR
#   define ACE_PEER_ACCEPTOR_ADDR _ACE_PEER_ADDR

// Handle ACE_Connector
#   define ACE_PEER_CONNECTOR_1 class _ACE_PEER_CONNECTOR, class _ACE_PEER_ADDR
#   define ACE_PEER_CONNECTOR_2 _ACE_PEER_CONNECTOR, _ACE_PEER_ADDR
#   define ACE_PEER_CONNECTOR _ACE_PEER_CONNECTOR
#   define ACE_PEER_CONNECTOR_ADDR _ACE_PEER_ADDR
#   define ACE_PEER_CONNECTOR_ADDR_ANY ACE_PEER_CONNECTOR_ADDR::sap_any

// Handle ACE_SOCK_*
#   define ACE_SOCK_ACCEPTOR ACE_SOCK_Acceptor, ACE_INET_Addr
#   define ACE_SOCK_CONNECTOR ACE_SOCK_Connector, ACE_INET_Addr
#   define ACE_SOCK_STREAM ACE_SOCK_Stream, ACE_INET_Addr

// Handle ACE_MEM_*
#   define ACE_MEM_ACCEPTOR ACE_MEM_Acceptor, ACE_MEM_Addr
#   define ACE_MEM_CONNECTOR ACE_MEM_Connector, ACE_INET_Addr
#   define ACE_MEM_STREAM ACE_MEM_Stream, ACE_INET_Addr

// Handle ACE_LSOCK_*
#   define ACE_LSOCK_ACCEPTOR ACE_LSOCK_Acceptor, ACE_UNIX_Addr
#   define ACE_LSOCK_CONNECTOR ACE_LSOCK_Connector, ACE_UNIX_Addr
#   define ACE_LSOCK_STREAM ACE_LSOCK_Stream, ACE_UNIX_Addr

// Handle ACE_TLI_*
#   define ACE_TLI_ACCEPTOR ACE_TLI_Acceptor, ACE_INET_Addr
#   define ACE_TLI_CONNECTOR ACE_TLI_Connector, ACE_INET_Addr
#   define ACE_TLI_STREAM ACE_TLI_Stream, ACE_INET_Addr

// Handle ACE_SPIPE_*
#   define ACE_SPIPE_ACCEPTOR ACE_SPIPE_Acceptor, ACE_SPIPE_Addr
#   define ACE_SPIPE_CONNECTOR ACE_SPIPE_Connector, ACE_SPIPE_Addr
#   define ACE_SPIPE_STREAM ACE_SPIPE_Stream, ACE_SPIPE_Addr

// Handle ACE_UPIPE_*
#   define ACE_UPIPE_ACCEPTOR ACE_UPIPE_Acceptor, ACE_SPIPE_Addr
#   define ACE_UPIPE_CONNECTOR ACE_UPIPE_Connector, ACE_SPIPE_Addr
#   define ACE_UPIPE_STREAM ACE_UPIPE_Stream, ACE_SPIPE_Addr

// Handle ACE_FILE_*
#   define ACE_FILE_CONNECTOR ACE_FILE_Connector, ACE_FILE_Addr
#   define ACE_FILE_STREAM ACE_FILE_IO, ACE_FILE_Addr

// Handle ACE_*_Memory_Pool.
#   define ACE_MMAP_MEMORY_POOL ACE_MMAP_Memory_Pool, ACE_MMAP_Memory_Pool_Options
#   define ACE_LITE_MMAP_MEMORY_POOL ACE_Lite_MMAP_Memory_Pool, ACE_MMAP_Memory_Pool_Options
#   define ACE_SBRK_MEMORY_POOL ACE_Sbrk_Memory_Pool, ACE_Sbrk_Memory_Pool_Options
#   define ACE_SHARED_MEMORY_POOL ACE_Shared_Memory_Pool, ACE_Shared_Memory_Pool_Options
#   define ACE_LOCAL_MEMORY_POOL ACE_Local_Memory_Pool, ACE_Local_Memory_Pool_Options
#   define ACE_PAGEFILE_MEMORY_POOL ACE_Pagefile_Memory_Pool, ACE_Pagefile_Memory_Pool_Options
# endif /* ACE_HAS_TEMPLATE_TYPEDEFS */

// These two are only for backward compatibility. You should avoid
// using them if not necessary.
# define ACE_SYNCH_1 ACE_SYNCH_DECL
# define ACE_SYNCH_2 ACE_SYNCH_USE

// For Win32 compatibility...
# if !defined (ACE_WSOCK_VERSION)
#   define ACE_WSOCK_VERSION 0, 0
# endif /* ACE_WSOCK_VERSION */

# if defined (ACE_HAS_BROKEN_CTIME)
#   undef ctime
# endif /* ACE_HAS_BROKEN_CTIME */

/// Service Objects, i.e., objects dynamically loaded via the service
/// configurator, must provide a destructor function with the
/// following prototype to perform object cleanup.
typedef void (*ACE_Service_Object_Exterminator)(void *);

/** @name Service Configurator macros
 *
 * The following macros are used to define helper objects used in
 * ACE's Service Configurator.  This is an implementation of the
 * Service Configurator pattern:
 *
 * http://www.cs.wustl.edu/~schmidt/PDF/SvcConf.pdf
 *
 * The intent of this pattern is to allow developers to dynamically
 * load and configure services into a system.  With a little help from
 * this macros statically linked services can also be dynamically
 * configured.
 *
 * More details about this component are available in the documentation
 * of the ACE_Service_Configurator class and also
 * ACE_Dynamic_Service.
 *
 * Notice that in all the macros the SERVICE_CLASS parameter must be
 * the name of a class derived from ACE_Service_Object.
 */
//@{
/// Declare a the data structure required to register a statically
/// linked service into the service configurator.
/**
 * The macro should be used in the header file where the service is
 * declared, its only argument is usually the name of the class that
 * implements the service.
 *
 * @param SERVICE_CLASS The name of the class implementing the
 *   service.
 */
# define ACE_STATIC_SVC_DECLARE(SERVICE_CLASS) \
extern ACE_Static_Svc_Descriptor ace_svc_desc_##SERVICE_CLASS ;

/// As ACE_STATIC_SVC_DECLARE, but using an export macro for NT
/// compilers.
/**
 * NT compilers require the use of explicit directives to export and
 * import symbols from a DLL.  If you need to define a service in a
 * dynamic library you should use this version instead.
 * Normally ACE uses a macro to inject the correct export/import
 * directives on NT.  Naturally it also the macro expands to a blank
 * on platforms that do not require such directives.
 * The first argument (EXPORT_NAME) is the prefix for this export
 * macro, the full name is formed by appending _Export.
 * ACE provides tools to generate header files that define the macro
 * correctly on all platforms, please see
 * $ACE_ROOT/bin/generate_export_file.pl
 *
 * @param EXPORT_NAME The export macro name prefix.
 * @param SERVICE_CLASS The name of the class implementing the service.
 */
#define ACE_STATIC_SVC_DECLARE_EXPORT(EXPORT_NAME,SERVICE_CLASS) \
extern EXPORT_NAME##_Export ACE_Static_Svc_Descriptor ace_svc_desc_##SERVICE_CLASS;

/// Define the data structure used to register a statically linked
/// service into the Service Configurator.
/**
 * The service configurator requires several arguments to build and
 * control an statically linked service, including its name, the
 * factory function used to construct the service, and some flags.
 * All those parameters are configured in a single structure, an
 * instance of this structure is statically initialized using the
 * following macro.
 *
 * @param SERVICE_CLASS The name of the class that implements the
 *    service, must be derived (directly or indirectly) from
 *    ACE_Service_Object.
 * @param NAME The name for this service, this name is used by the
 *    service configurator to match configuration options provided in
 *    the svc.conf file.
 * @param TYPE The type of object.  Objects can be streams or service
 *    objects.  Please read the ACE_Service_Configurator and ASX
 *    documentation for more details.
 * @param FN The name of the factory function, usually the
 *    ACE_SVC_NAME macro can be used to generate the name.  The
 *    factory function is often defined using ACE_FACTORY_DECLARE and
 *    ACE_FACTORY_DEFINE.
 * @param FLAGS Flags to control the ownership and lifecycle of the
 *    object. Please read the ACE_Service_Configurator documentation
 *    for more details.
 * @param ACTIVE If not zero then a thread will be dedicate to the
 *    service. Please read the ACE_Service_Configurator documentation
 *    for more details.
 */
#define ACE_STATIC_SVC_DEFINE(SERVICE_CLASS, NAME, TYPE, FN, FLAGS, ACTIVE) \
ACE_Static_Svc_Descriptor ace_svc_desc_##SERVICE_CLASS = { NAME, TYPE, FN, FLAGS, ACTIVE };

/// Automatically register a service with the service configurator
/**
 * In some applications the services must be automatically registered
 * with the service configurator, before main() starts.
 * The ACE_STATIC_SVC_REQUIRE macro defines a class whose constructor
 * register the service, it also defines a static instance of that
 * class to ensure that the service is registered before main.
 *
 * On platforms that lack adequate support for static C++ objects the
 * macro ACE_STATIC_SVC_REGISTER can be used to explicitly register
 * the service.
 *
 * @todo One class per-Service_Object seems wasteful.  It should be
 *   possible to define a single class and re-use it for all the
 *   service objects, just by passing the Service_Descriptor as an
 *   argument to the constructor.
 */
#if defined(ACE_LACKS_STATIC_CONSTRUCTORS)
# define ACE_STATIC_SVC_REQUIRE(SERVICE_CLASS)\
class ACE_Static_Svc_##SERVICE_CLASS {\
public:\
  ACE_Static_Svc_##SERVICE_CLASS() { \
    ACE_Service_Config::static_svcs ()->insert (\
         &ace_svc_desc_##SERVICE_CLASS); \
  } \
};
#define ACE_STATIC_SVC_REGISTER(SERVICE_CLASS)\
ACE_Static_Svc_##SERVICE_CLASS ace_static_svc_##SERVICE_CLASS

#else /* !ACE_LACKS_STATIC_CONSTRUCTORS */

# define ACE_STATIC_SVC_REQUIRE(SERVICE_CLASS)\
class ACE_Static_Svc_##SERVICE_CLASS {\
public:\
  ACE_Static_Svc_##SERVICE_CLASS() { \
    ACE_Service_Config::static_svcs ()->insert (\
         &ace_svc_desc_##SERVICE_CLASS); \
    } \
};\
static ACE_Static_Svc_##SERVICE_CLASS ace_static_svc_##SERVICE_CLASS;
#define ACE_STATIC_SVC_REGISTER(SERVICE_CLASS) do {} while (0)

#endif /* !ACE_LACKS_STATIC_CONSTRUCTORS */

/// Declare the factory method used to create dynamically loadable
/// services.
/**
 * Once the service implementation is dynamically loaded the Service
 * Configurator uses a factory method to create the object.
 * This macro declares such a factory function with the proper
 * interface and export macros.
 * Normally used in the header file that declares the service
 * implementation.
 *
 * @param CLS must match the prefix of the export macro used for this
 *        service.
 * @param SERVICE_CLASS must match the name of the class that
 *        implements the service.
 *
 */
#define ACE_FACTORY_DECLARE(CLS,SERVICE_CLASS) \
extern "C" CLS##_Export ACE_Service_Object *\
_make_##SERVICE_CLASS (ACE_Service_Object_Exterminator *);

/// Define the factory method (and destructor) for a dynamically
/// loadable service.
/**
 * Use with arguments matching ACE_FACTORY_DECLARE.
 * Normally used in the .cpp file that defines the service
 * implementation.
 *
 * This macro defines both the factory method and the function used to
 * cleanup the service object.
 *
 * If this macro is used to define a factory function that need not be
 * exported (for example, in a static service situation), CLS can be
 * specified as ACE_Local_Service.
 */
# define ACE_Local_Service_Export

# define ACE_FACTORY_DEFINE(CLS,SERVICE_CLASS) \
void _gobble_##SERVICE_CLASS (void *p) { \
  ACE_Service_Object *_p = ACE_static_cast (ACE_Service_Object *, p); \
  ACE_ASSERT (_p != 0); \
  delete _p; } \
extern "C" CLS##_Export ACE_Service_Object *\
_make_##SERVICE_CLASS (ACE_Service_Object_Exterminator *gobbler) \
{ \
  ACE_TRACE (#SERVICE_CLASS); \
  if (gobbler != 0) \
    *gobbler = (ACE_Service_Object_Exterminator) _gobble_##SERVICE_CLASS; \
  return new SERVICE_CLASS; \
}

/// The canonical name for a service factory method
#define ACE_SVC_NAME(SERVICE_CLASS) _make_##SERVICE_CLASS

/// The canonical way to invoke (i.e. construct) a service factory
/// method.
#define ACE_SVC_INVOKE(SERVICE_CLASS) _make_##SERVICE_CLASS (0)

//@}

/** @name Helper macros for services defined in the netsvcs library.
 *
 * The ACE services defined in netsvcs use this helper macros for
 * simplicity.
 *
 */
//@{
# define ACE_SVC_FACTORY_DECLARE(X) ACE_FACTORY_DECLARE (ACE_Svc, X)
# define ACE_SVC_FACTORY_DEFINE(X) ACE_FACTORY_DEFINE (ACE_Svc, X)
//@}

# if defined (ACE_HAS_THREADS) && (defined (ACE_HAS_THREAD_SPECIFIC_STORAGE) || defined (ACE_HAS_TSS_EMULATION))
#   define ACE_TSS_TYPE(T) ACE_TSS< T >
#   if defined (ACE_HAS_BROKEN_CONVERSIONS)
#     define ACE_TSS_GET(I, T) (*(I))
#   else
#     define ACE_TSS_GET(I, T) ((I)->operator T * ())
#   endif /* ACE_HAS_BROKEN_CONVERSIONS */
# else
#   define ACE_TSS_TYPE(T) T
#   define ACE_TSS_GET(I, T) (I)
# endif /* ACE_HAS_THREADS && (ACE_HAS_THREAD_SPECIFIC_STORAGE || ACE_HAS_TSS_EMULATIOND) */

# if defined (ACE_LACKS_MODE_MASKS)
// MODE MASKS

// the following macros are for POSIX conformance.

#   if !defined (ACE_HAS_USER_MODE_MASKS)
#     define S_IRWXU 00700         /* read, write, execute: owner. */
#     define S_IRUSR 00400         /* read permission: owner. */
#     define S_IWUSR 00200         /* write permission: owner. */
#     define S_IXUSR 00100         /* execute permission: owner. */
#   endif /* ACE_HAS_USER_MODE_MASKS */
#   define S_IRWXG 00070           /* read, write, execute: group. */
#   define S_IRGRP 00040           /* read permission: group. */
#   define S_IWGRP 00020           /* write permission: group. */
#   define S_IXGRP 00010           /* execute permission: group. */
#   define S_IRWXO 00007           /* read, write, execute: other. */
#   define S_IROTH 00004           /* read permission: other. */
#   define S_IWOTH 00002           /* write permission: other. */
#   define S_IXOTH 00001           /* execute permission: other. */

# endif /* ACE_LACKS_MODE_MASKS */

# if defined (ACE_LACKS_SEMBUF_T)
struct sembuf
{
  /// semaphore #
  unsigned short sem_num;

  /// semaphore operation
  short sem_op;

  /// operation flags
  short sem_flg;
};
# endif /* ACE_LACKS_SEMBUF_T */

# if defined (ACE_LACKS_MSGBUF_T)
struct msgbuf {};
# endif /* ACE_LACKS_MSGBUF_T */

# if defined (ACE_LACKS_STRRECVFD)
struct strrecvfd {};
# endif /* ACE_LACKS_STRRECVFD */

# if defined (ACE_HAS_PROC_FS)
#   include /**/ <sys/procfs.h>
# endif /* ACE_HAS_PROC_FS */

# if defined(__rtems__)
struct iovec {
  /// Base address.
  char *iov_base;
  /// Length.
  size_t iov_len;
};
# endif

# if defined (ACE_HAS_BROKEN_WRITEV)
typedef struct iovec ACE_WRITEV_TYPE;
# else
typedef const struct iovec ACE_WRITEV_TYPE;
# endif /* ACE_HAS_BROKEN_WRITEV */

# if defined (ACE_HAS_BROKEN_READV)
typedef const struct iovec ACE_READV_TYPE;
# else
typedef struct iovec ACE_READV_TYPE;
# endif /* ACE_HAS_BROKEN_READV */

# if defined (ACE_HAS_BROKEN_SETRLIMIT)
typedef struct rlimit ACE_SETRLIMIT_TYPE;
# else
typedef const struct rlimit ACE_SETRLIMIT_TYPE;
# endif /* ACE_HAS_BROKEN_SETRLIMIT */

# if defined (ACE_MT_SAFE) && (ACE_MT_SAFE != 0)
#   define ACE_MT(X) X
#   if !defined (_REENTRANT)
#     define _REENTRANT
#   endif /* _REENTRANT */
# else
#   define ACE_MT(X)
# endif /* ACE_MT_SAFE */

# if !defined (ACE_DEFAULT_THREAD_PRIORITY)
#   define ACE_DEFAULT_THREAD_PRIORITY (-0x7fffffffL - 1L)
# endif /* ACE_DEFAULT_THREAD_PRIORITY */

# if defined (ACE_HAS_POSIX_SEM)
#   include /**/ <semaphore.h>
#   if !defined (SEM_FAILED) && !defined (ACE_LACKS_NAMED_POSIX_SEM)
#     define SEM_FAILED ((sem_t *) -1)
#   endif  /* !SEM_FAILED */

typedef struct
{
  /// Pointer to semaphore handle.  This is allocated by ACE if we are
  /// working with an unnamed POSIX semaphore or by the OS if we are
  /// working with a named POSIX semaphore.
  sem_t *sema_;

  /// Name of the semaphore (if this is non-NULL then this is a named
  /// POSIX semaphore, else its an unnamed POSIX semaphore).
  char *name_;

#   if defined (ACE_LACKS_NAMED_POSIX_SEM)
  /// this->sema_ doesn't always get created dynamically if a platform
  /// doesn't support named posix semaphores.  We use this flag to
  /// remember if we need to delete <sema_> or not.
  int new_sema_;
#   endif /* ACE_LACKS_NAMED_POSIX_SEM */
} ACE_sema_t;
# endif /* ACE_HAS_POSIX_SEM */

struct cancel_state
{
  /// e.g., PTHREAD_CANCEL_ENABLE, PTHREAD_CANCEL_DISABLE,
  /// PTHREAD_CANCELED.
  int cancelstate;

  /// e.g., PTHREAD_CANCEL_DEFERRED and PTHREAD_CANCEL_ASYNCHRONOUS.
  int canceltype;
};

# if defined (ACE_HAS_WINCE)
#   include /**/ <types.h>

//typedef DWORD  nlink_t;

// CE's add-on for c-style fstat/stat functionalities.  This struct is
// by no mean complete compared to what you usually find in UNIX
// platforms.  Only members that have direct conversion using Win32's
// BY_HANDLE_FILE_INFORMATION are defined so that users can discover
// non-supported members at compile time.  Time values are of type
// ACE_Time_Value for easy comparison.

// Since CE does not have _stat by default as NT/2000 does, the 'stat'
// struct defined here will be used.  Also note that CE file system
// struct is only for the CE 3.0 or later.
// Refer to the WCHAR.H from Visual C++ and WIBASE.H from eVC 3.0.

typedef unsigned int dev_t;

struct stat
{
    /// always 0 on Windows platforms
    dev_t st_dev;

    /// always 0 on Windows platforms
    dev_t st_rdev;

    /// file attribute
    unsigned short st_mode;

    /// number of hard links
    short st_nlink;

    /// time of last access
    ACE_Time_Value st_atime;

    /// time of last data modification
    ACE_Time_Value st_mtime;

    /// time of creation
    ACE_Time_Value st_ctime;

    /// file size, in bytes
    off_t st_size;

    // Following members do not have direct conversion in Window platforms.
//    u_long st_blksize;        // optimal blocksize for I/O
//    u_long st_flags;          // user defined flags for file
};

# else /* ! ACE_HAS_WINCE */
#   if defined (ACE_LACKS_SYS_TYPES_H)
#     if ! defined (ACE_PSOS)
  typedef unsigned char u_char;
  typedef unsigned short u_short;
  typedef unsigned int u_int;
  typedef unsigned long u_long;

  typedef unsigned char uchar_t;
  typedef unsigned short ushort_t;
  typedef unsigned int  uint_t;
  typedef unsigned long ulong_t;
#     endif /* ! defined (ACE_PSOS) */
#   else
#     include /**/ <sys/types.h>
#   endif  /* ACE_LACKS_SYS_TYPES_H */

#   if ! defined (ACE_PSOS)
#     include /**/ <sys/stat.h>
#   endif
# endif /* ACE_HAS_WINCE */


#if defined (ACE_HAS_NO_THROW_SPEC)
#   define ACE_THROW_SPEC(X)
#else
# if defined (ACE_HAS_EXCEPTIONS)
#   define ACE_THROW_SPEC(X) throw X
#   if defined (ACE_WIN32) && defined(_MSC_VER) && !defined (ghs)
// @@ MSVC "supports" the keyword but doesn't implement it (Huh?).
//    Therefore, we simply supress the warning for now.
#     pragma warning( disable : 4290 )
#   endif /* ACE_WIN32 */
# else  /* ! ACE_HAS_EXCEPTIONS */
#   define ACE_THROW_SPEC(X)
# endif /* ! ACE_HAS_EXCEPTIONS */
#endif /*ACE_HAS_NO_THROW_SPEC*/

#if !defined (ACE_LACKS_UNISTD_H)
#  include /**/ <unistd.h>
#endif /* ACE_LACKS_UNISTD_H */

#if defined (ACE_HAS_PRIOCNTL)
  // Need to #include thread.h before #defining THR_BOUND, etc.,
  // when building without threads on SunOS 5.x.
#  if defined (sun)
#    include /**/ <thread.h>
#  endif /* sun */

  // Need to #include these before #defining USYNC_PROCESS on SunOS 5.x.
# include /**/ <sys/rtpriocntl.h>
# include /**/ <sys/tspriocntl.h>
#endif /* ACE_HAS_PRIOCNTL */

# if defined (ACE_HAS_THREADS)

#   if defined (ACE_HAS_STHREADS)
#     include /**/ <synch.h>
#     include /**/ <thread.h>
#     define ACE_SCOPE_PROCESS P_PID
#     define ACE_SCOPE_LWP P_LWPID
#     define ACE_SCOPE_THREAD (ACE_SCOPE_LWP + 1)
#   else
#     define ACE_SCOPE_PROCESS 0
#     define ACE_SCOPE_LWP 1
#     define ACE_SCOPE_THREAD 2
#   endif /* ACE_HAS_STHREADS */

#   if !defined (ACE_HAS_PTHREADS)
#     define ACE_SCHED_OTHER 0
#     define ACE_SCHED_FIFO 1
#     define ACE_SCHED_RR 2
#   endif /* ! ACE_HAS_PTHREADS */

#   if defined (ACE_HAS_PTHREADS)
#     define ACE_SCHED_OTHER SCHED_OTHER
#     define ACE_SCHED_FIFO SCHED_FIFO
#     define ACE_SCHED_RR SCHED_RR

// Definitions for mapping POSIX pthreads draft 6 into 1003.1c names

#     if defined (ACE_HAS_PTHREADS_DRAFT6)
#       define PTHREAD_SCOPE_PROCESS           PTHREAD_SCOPE_LOCAL
#       define PTHREAD_SCOPE_SYSTEM            PTHREAD_SCOPE_GLOBAL
#       define PTHREAD_CREATE_UNDETACHED       0
#       define PTHREAD_CREATE_DETACHED         1
#       define PTHREAD_CREATE_JOINABLE         0
#       define PTHREAD_EXPLICIT_SCHED          0
#       define PTHREAD_MIN_PRIORITY            0
#       define PTHREAD_MAX_PRIORITY            126
#     endif /* ACE_HAS_PTHREADS_DRAFT6 */

// Definitions for THREAD- and PROCESS-LEVEL priorities...some
// implementations define these while others don't.  In order to
// further complicate matters, we don't redefine the default (*_DEF)
// values if they've already been defined, which allows individual
// programs to have their own ACE-wide "default".

// PROCESS-level values
#     if defined (_POSIX_PRIORITY_SCHEDULING) && \
         !defined(_UNICOS) && !defined(UNIXWARE_7_1)
#       define ACE_PROC_PRI_FIFO_MIN  (sched_get_priority_min(SCHED_FIFO))
#       define ACE_PROC_PRI_RR_MIN    (sched_get_priority_min(SCHED_RR))
#       if defined (HPUX)
          // HP-UX's other is the SCHED_HPUX class, which uses historical
          // values that have reverse semantics from POSIX (low value is
          // more important priority). To use these in pthreads calls,
          // the values need to be converted. The other scheduling classes
          // don't need this special treatment.
#         define ACE_PROC_PRI_OTHER_MIN \
                      (sched_get_priority_min(SCHED_OTHER))
#       else
#         define ACE_PROC_PRI_OTHER_MIN (sched_get_priority_min(SCHED_OTHER))
#       endif /* HPUX */
#     else /* UNICOS is missing a sched_get_priority_min() implementation,
              SCO too */
#       define ACE_PROC_PRI_FIFO_MIN  0
#       define ACE_PROC_PRI_RR_MIN    0
#       define ACE_PROC_PRI_OTHER_MIN 0
#     endif

#     if defined (_POSIX_PRIORITY_SCHEDULING) && !defined(UNIXWARE_7_1)
#       define ACE_PROC_PRI_FIFO_MAX  (sched_get_priority_max(SCHED_FIFO))
#       define ACE_PROC_PRI_RR_MAX    (sched_get_priority_max(SCHED_RR))
#       if defined (HPUX)
#         define ACE_PROC_PRI_OTHER_MAX \
                      (sched_get_priority_max(SCHED_OTHER))
#       else
#         define ACE_PROC_PRI_OTHER_MAX (sched_get_priority_max(SCHED_OTHER))
#       endif /* HPUX */
#     else /* SCO missing sched_get_priority_max() implementation */
#       define ACE_PROC_PRI_FIFO_MAX  59
#       define ACE_PROC_PRI_RR_MAX    59
#       define ACE_PROC_PRI_OTHER_MAX 59
#     endif

#     if !defined(ACE_PROC_PRI_FIFO_DEF)
#       define ACE_PROC_PRI_FIFO_DEF (ACE_PROC_PRI_FIFO_MIN + (ACE_PROC_PRI_FIFO_MAX - ACE_PROC_PRI_FIFO_MIN)/2)
#     endif
#     if !defined(ACE_PROC_PRI_RR_DEF)
#       define ACE_PROC_PRI_RR_DEF (ACE_PROC_PRI_RR_MIN + (ACE_PROC_PRI_RR_MAX - ACE_PROC_PRI_RR_MIN)/2)
#     endif
#     if !defined(ACE_PROC_PRI_OTHER_DEF)
#       define ACE_PROC_PRI_OTHER_DEF (ACE_PROC_PRI_OTHER_MIN + (ACE_PROC_PRI_OTHER_MAX - ACE_PROC_PRI_OTHER_MIN)/2)
#     endif

// THREAD-level values
#     if defined(PRI_FIFO_MIN) && defined(PRI_FIFO_MAX) && defined(PRI_RR_MIN) && defined(PRI_RR_MAX) && defined(PRI_OTHER_MIN) && defined(PRI_OTHER_MAX)
#       if !defined (ACE_THR_PRI_FIFO_MIN)
#         define ACE_THR_PRI_FIFO_MIN  (long) PRI_FIFO_MIN
#       endif /* !ACE_THR_PRI_FIFO_MIN */
#       if !defined (ACE_THR_PRI_FIFO_MAX)
#         define ACE_THR_PRI_FIFO_MAX  (long) PRI_FIFO_MAX
#       endif /* !ACE_THR_PRI_FIFO_MAX */
#       if !defined (ACE_THR_PRI_RR_MIN)
#         define ACE_THR_PRI_RR_MIN    (long) PRI_RR_MIN
#       endif /* !ACE_THR_PRI_RR_MIN */
#       if !defined (ACE_THR_PRI_RR_MAX)
#         define ACE_THR_PRI_RR_MAX    (long) PRI_RR_MAX
#       endif /* !ACE_THR_PRI_RR_MAX */
#       if !defined (ACE_THR_PRI_OTHER_MIN)
#         define ACE_THR_PRI_OTHER_MIN (long) PRI_OTHER_MIN
#       endif /* !ACE_THR_PRI_OTHER_MIN */
#       if !defined (ACE_THR_PRI_OTHER_MAX)
#         define ACE_THR_PRI_OTHER_MAX (long) PRI_OTHER_MAX
#       endif /* !ACE_THR_PRI_OTHER_MAX */
#     elif defined (AIX)
        // AIX's priority range is 1 (low) to 127 (high). There aren't
        // any preprocessor macros I can find. PRIORITY_MIN is for
        // process priorities, as far as I can see, and does not apply
        // to thread priority. The 1 to 127 range is from the
        // pthread_attr_setschedparam man page (Steve Huston, 18-May-2001).
#       if !defined (ACE_THR_PRI_FIFO_MIN)
#         define ACE_THR_PRI_FIFO_MIN  (long) 1
#       endif /* !ACE_THR_PRI_FIFO_MIN */
#       if !defined (ACE_THR_PRI_FIFO_MAX)
#         define ACE_THR_PRI_FIFO_MAX  (long) 127
#       endif /* !ACE_THR_PRI_FIFO_MAX */
#       if !defined (ACE_THR_PRI_RR_MIN)
#         define ACE_THR_PRI_RR_MIN    (long) 1
#       endif /* !ACE_THR_PRI_RR_MIN */
#       if !defined (ACE_THR_PRI_RR_MAX)
#         define ACE_THR_PRI_RR_MAX    (long) 127
#       endif /* !ACE_THR_PRI_RR_MAX */
#       if !defined (ACE_THR_PRI_OTHER_MIN)
#         define ACE_THR_PRI_OTHER_MIN (long) 1
#       endif /* !ACE_THR_PRI_OTHER_MIN */
#       if !defined (ACE_THR_PRI_OTHER_MAX)
#         define ACE_THR_PRI_OTHER_MAX (long) 127
#       endif /* !ACE_THR_PRI_OTHER_MAX */
#     elif defined (sun)
#       if !defined (ACE_THR_PRI_FIFO_MIN)
#         define ACE_THR_PRI_FIFO_MIN  (long) 0
#       endif /* !ACE_THR_PRI_FIFO_MIN */
#       if !defined (ACE_THR_PRI_FIFO_MAX)
#         define ACE_THR_PRI_FIFO_MAX  (long) 59
#       endif /* !ACE_THR_PRI_FIFO_MAX */
#       if !defined (ACE_THR_PRI_RR_MIN)
#         define ACE_THR_PRI_RR_MIN    (long) 0
#       endif /* !ACE_THR_PRI_RR_MIN */
#       if !defined (ACE_THR_PRI_RR_MAX)
#         define ACE_THR_PRI_RR_MAX    (long) 59
#       endif /* !ACE_THR_PRI_RR_MAX */
#       if !defined (ACE_THR_PRI_OTHER_MIN)
#         define ACE_THR_PRI_OTHER_MIN (long) 0
#       endif /* !ACE_THR_PRI_OTHER_MIN */
#       if !defined (ACE_THR_PRI_OTHER_MAX)
#         define ACE_THR_PRI_OTHER_MAX (long) 127
#       endif /* !ACE_THR_PRI_OTHER_MAX */
#     else
#       if !defined (ACE_THR_PRI_FIFO_MIN)
#         define ACE_THR_PRI_FIFO_MIN  (long) ACE_PROC_PRI_FIFO_MIN
#       endif /* !ACE_THR_PRI_FIFO_MIN */
#       if !defined (ACE_THR_PRI_FIFO_MAX)
#         define ACE_THR_PRI_FIFO_MAX  (long) ACE_PROC_PRI_FIFO_MAX
#       endif /* !ACE_THR_PRI_FIFO_MAX */
#       if !defined (ACE_THR_PRI_RR_MIN)
#         define ACE_THR_PRI_RR_MIN    (long) ACE_PROC_PRI_RR_MIN
#       endif /* !ACE_THR_PRI_RR_MIN */
#       if !defined (ACE_THR_PRI_RR_MAX)
#         define ACE_THR_PRI_RR_MAX    (long) ACE_PROC_PRI_RR_MAX
#       endif /* !ACE_THR_PRI_RR_MAX */
#       if !defined (ACE_THR_PRI_OTHER_MIN)
#         define ACE_THR_PRI_OTHER_MIN (long) ACE_PROC_PRI_OTHER_MIN
#       endif /* !ACE_THR_PRI_OTHER_MIN */
#       if !defined (ACE_THR_PRI_OTHER_MAX)
#         define ACE_THR_PRI_OTHER_MAX (long) ACE_PROC_PRI_OTHER_MAX
#       endif /* !ACE_THR_PRI_OTHER_MAX */
#     endif
#     if !defined(ACE_THR_PRI_FIFO_DEF)
#       define ACE_THR_PRI_FIFO_DEF  ((ACE_THR_PRI_FIFO_MIN + ACE_THR_PRI_FIFO_MAX)/2)
#     endif
#     if !defined(ACE_THR_PRI_RR_DEF)
#       define ACE_THR_PRI_RR_DEF ((ACE_THR_PRI_RR_MIN + ACE_THR_PRI_RR_MAX)/2)
#     endif
#     if !defined(ACE_THR_PRI_OTHER_DEF)
#       define ACE_THR_PRI_OTHER_DEF ((ACE_THR_PRI_OTHER_MIN + ACE_THR_PRI_OTHER_MAX)/2)
#     endif

// Typedefs to help compatibility with Windows NT and Pthreads.
typedef pthread_t ACE_hthread_t;
typedef pthread_t ACE_thread_t;

#     if defined (ACE_HAS_TSS_EMULATION)
        typedef pthread_key_t ACE_OS_thread_key_t;
        typedef u_long ACE_thread_key_t;
#     else  /* ! ACE_HAS_TSS_EMULATION */
        typedef pthread_key_t ACE_thread_key_t;
#     endif /* ! ACE_HAS_TSS_EMULATION */

#     if !defined (ACE_LACKS_COND_T)
typedef pthread_mutex_t ACE_mutex_t;
typedef pthread_cond_t ACE_cond_t;
typedef pthread_condattr_t ACE_condattr_t;
typedef pthread_mutexattr_t ACE_mutexattr_t;
#     endif /* ! ACE_LACKS_COND_T */
typedef pthread_mutex_t ACE_thread_mutex_t;

#     if !defined (PTHREAD_CANCEL_DISABLE)
#       define PTHREAD_CANCEL_DISABLE      0
#     endif /* PTHREAD_CANCEL_DISABLE */

#     if !defined (PTHREAD_CANCEL_ENABLE)
#       define PTHREAD_CANCEL_ENABLE       0
#     endif /* PTHREAD_CANCEL_ENABLE */

#     if !defined (PTHREAD_CANCEL_DEFERRED)
#       define PTHREAD_CANCEL_DEFERRED     0
#     endif /* PTHREAD_CANCEL_DEFERRED */

#     if !defined (PTHREAD_CANCEL_ASYNCHRONOUS)
#       define PTHREAD_CANCEL_ASYNCHRONOUS 0
#     endif /* PTHREAD_CANCEL_ASYNCHRONOUS */

#     define THR_CANCEL_DISABLE      PTHREAD_CANCEL_DISABLE
#     define THR_CANCEL_ENABLE       PTHREAD_CANCEL_ENABLE
#     define THR_CANCEL_DEFERRED     PTHREAD_CANCEL_DEFERRED
#     define THR_CANCEL_ASYNCHRONOUS PTHREAD_CANCEL_ASYNCHRONOUS

#     if !defined (PTHREAD_CREATE_JOINABLE)
#       if defined (PTHREAD_CREATE_UNDETACHED)
#         define PTHREAD_CREATE_JOINABLE PTHREAD_CREATE_UNDETACHED
#       else
#         define PTHREAD_CREATE_JOINABLE 0
#       endif /* PTHREAD_CREATE_UNDETACHED */
#     endif /* PTHREAD_CREATE_JOINABLE */

#     if !defined (PTHREAD_CREATE_DETACHED)
#       define PTHREAD_CREATE_DETACHED 1
#     endif /* PTHREAD_CREATE_DETACHED */

#     if !defined (PTHREAD_PROCESS_PRIVATE) && !defined (ACE_HAS_PTHREAD_PROCESS_ENUM)
#       if defined (PTHREAD_MUTEXTYPE_FAST)
#         define PTHREAD_PROCESS_PRIVATE PTHREAD_MUTEXTYPE_FAST
#       else
#         define PTHREAD_PROCESS_PRIVATE 0
#       endif /* PTHREAD_MUTEXTYPE_FAST */
#     endif /* PTHREAD_PROCESS_PRIVATE */

#     if !defined (PTHREAD_PROCESS_SHARED) && !defined (ACE_HAS_PTHREAD_PROCESS_ENUM)
#       if defined (PTHREAD_MUTEXTYPE_FAST)
#         define PTHREAD_PROCESS_SHARED PTHREAD_MUTEXTYPE_FAST
#       else
#         define PTHREAD_PROCESS_SHARED 1
#       endif /* PTHREAD_MUTEXTYPE_FAST */
#     endif /* PTHREAD_PROCESS_SHARED */

#     if defined (ACE_HAS_PTHREADS_DRAFT4)
#       if defined (PTHREAD_PROCESS_PRIVATE)
#         if !defined (USYNC_THREAD)
#         define USYNC_THREAD    PTHREAD_PROCESS_PRIVATE
#         endif /* ! USYNC_THREAD */
#       else
#         if !defined (USYNC_THREAD)
#         define USYNC_THREAD    MUTEX_NONRECURSIVE_NP
#         endif /* ! USYNC_THREAD */
#       endif /* PTHREAD_PROCESS_PRIVATE */

#       if defined (PTHREAD_PROCESS_SHARED)
#         if !defined (USYNC_PROCESS)
#         define USYNC_PROCESS   PTHREAD_PROCESS_SHARED
#         endif /* ! USYNC_PROCESS */
#       else
#         if !defined (USYNC_PROCESS)
#         define USYNC_PROCESS   MUTEX_NONRECURSIVE_NP
#         endif /* ! USYNC_PROCESS */
#       endif /* PTHREAD_PROCESS_SHARED */
#     elif !defined (ACE_HAS_STHREADS)
#       if !defined (USYNC_THREAD)
#       define USYNC_THREAD PTHREAD_PROCESS_PRIVATE
#       endif /* ! USYNC_THREAD */
#       if !defined (USYNC_PROCESS)
#       define USYNC_PROCESS PTHREAD_PROCESS_SHARED
#       endif /* ! USYNC_PROCESS */
#     endif /* ACE_HAS_PTHREADS_DRAFT4 */

/* MM-Graz:  prevent warnings */
#undef THR_BOUND
#undef THR_NEW_LWP
#undef THR_DETACHED
#undef THR_SUSPENDED
#undef THR_DAEMON

#     define THR_BOUND               0x00000001
#     if defined (CHORUS)
#       define THR_NEW_LWP             0x00000000
#     else
#       define THR_NEW_LWP             0x00000002
#     endif /* CHORUS */
#     define THR_DETACHED            0x00000040
#     define THR_SUSPENDED           0x00000080
#     define THR_DAEMON              0x00000100
#     define THR_JOINABLE            0x00010000
#     define THR_SCHED_FIFO          0x00020000
#     define THR_SCHED_RR            0x00040000
#     define THR_SCHED_DEFAULT       0x00080000

#     if defined (ACE_HAS_IRIX62_THREADS)
#     define THR_SCOPE_SYSTEM        0x00100000
#     else
#     define THR_SCOPE_SYSTEM        THR_BOUND
#endif /*ACE_HAS_IRIX62_THREADS*/

#     define THR_SCOPE_PROCESS       0x00200000
#     define THR_INHERIT_SCHED       0x00400000
#     define THR_EXPLICIT_SCHED      0x00800000
#     define THR_SCHED_IO            0x01000000

#     if !defined (ACE_HAS_STHREADS)
#       if !defined (ACE_HAS_POSIX_SEM)
/**
 * @class ACE_sema_t
 *
 * @brief This is used to implement semaphores for platforms that support
 * POSIX pthreads, but do *not* support POSIX semaphores, i.e.,
 * it's a different type than the POSIX <sem_t>.
 */
class ACE_OS_Export ACE_sema_t
{
friend class ACE_OS;
protected:
  /// Serialize access to internal state.
  ACE_mutex_t lock_;

  /// Block until there are no waiters.
  ACE_cond_t count_nonzero_;

  /// Count of the semaphore.
  u_long count_;

  /// Number of threads that have called <ACE_OS::sema_wait>.
  u_long waiters_;
};
#       endif /* !ACE_HAS_POSIX_SEM */

#       if defined (ACE_LACKS_PTHREAD_YIELD) && defined (ACE_HAS_THR_YIELD)
        // If we are on Solaris we can just reuse the existing
        // implementations of these synchronization types.
#         if !defined (ACE_LACKS_RWLOCK_T)
#           include /**/ <synch.h>
typedef rwlock_t ACE_rwlock_t;
#         endif /* !ACE_LACKS_RWLOCK_T */
#         include /**/ <thread.h>
#       endif /* (ACE_LACKS_PTHREAD_YIELD) && defined (ACE_HAS_THR_YIELD) */

#     else
#       if !defined (ACE_HAS_POSIX_SEM)
typedef sema_t ACE_sema_t;
#       endif /* !ACE_HAS_POSIX_SEM */
#     endif /* !ACE_HAS_STHREADS */
#   elif defined (ACE_HAS_STHREADS)
// Solaris threads, without PTHREADS.
// Typedefs to help compatibility with Windows NT and Pthreads.
typedef thread_t ACE_thread_t;
typedef thread_key_t ACE_thread_key_t;
typedef mutex_t ACE_mutex_t;
#     if !defined (ACE_LACKS_RWLOCK_T)
typedef rwlock_t ACE_rwlock_t;
#     endif /* !ACE_LACKS_RWLOCK_T */
#     if !defined (ACE_HAS_POSIX_SEM)
typedef sema_t ACE_sema_t;
#     endif /* !ACE_HAS_POSIX_SEM */

typedef cond_t ACE_cond_t;
struct ACE_OS_Export ACE_condattr_t
{
  int type;
};
struct ACE_OS_Export ACE_mutexattr_t
{
  int type;
};
typedef ACE_thread_t ACE_hthread_t;
typedef ACE_mutex_t ACE_thread_mutex_t;

#     define THR_CANCEL_DISABLE      0
#     define THR_CANCEL_ENABLE       0
#     define THR_CANCEL_DEFERRED     0
#     define THR_CANCEL_ASYNCHRONOUS 0
#     define THR_JOINABLE            0
#     define THR_SCHED_FIFO          0
#     define THR_SCHED_RR            0
#     define THR_SCHED_DEFAULT       0

#   elif defined (ACE_PSOS)

// Some versions of pSOS provide native mutex support.  For others,
// implement ACE_thread_mutex_t and ACE_mutex_t using pSOS semaphores.
// Either way, the types are all u_longs.
typedef u_long ACE_mutex_t;
typedef u_long ACE_thread_mutex_t;
typedef u_long ACE_thread_t;
typedef u_long ACE_hthread_t;

#if defined (ACE_PSOS_HAS_COND_T)
typedef u_long ACE_cond_t;
typedef u_long ACE_condattr_t;
struct ACE_OS_Export ACE_mutexattr_t
{
  int type;
};
#endif /* ACE_PSOS_HAS_COND_T */


// TCB registers 0-7 are for application use
#     define PSOS_TASK_REG_TSS 0
#     define PSOS_TASK_REG_MAX 7

#     define PSOS_TASK_MIN_PRIORITY   1
#     define PSOS_TASK_MAX_PRIORITY 239

// Key type: the ACE TSS emulation requires the key type be unsigned,
// for efficiency.  Current POSIX and Solaris TSS implementations also
// use unsigned int, so the ACE TSS emulation is compatible with them.
// Native pSOS TSD, where available, uses unsigned long as the key type.
#     if defined (ACE_PSOS_HAS_TSS)
typedef u_long ACE_thread_key_t;
#     else
typedef u_int ACE_thread_key_t;
#     endif /* ACE_PSOS_HAS_TSS */

#     define THR_CANCEL_DISABLE      0  /* thread can never be cancelled */
#     define THR_CANCEL_ENABLE       0      /* thread can be cancelled */
#     define THR_CANCEL_DEFERRED     0      /* cancellation deferred to cancellation point */
#     define THR_CANCEL_ASYNCHRONOUS 0      /* cancellation occurs immediately */

#     define THR_BOUND               0
#     define THR_NEW_LWP             0
#     define THR_DETACHED            0
#     define THR_SUSPENDED           0
#     define THR_DAEMON              0
#     define THR_JOINABLE            0

#     define THR_SCHED_FIFO          0
#     define THR_SCHED_RR            0
#     define THR_SCHED_DEFAULT       0
#     define USYNC_THREAD            T_LOCAL
#     define USYNC_PROCESS           T_GLOBAL

/* from psos.h */
/* #define T_NOPREEMPT     0x00000001   Not preemptible bit */
/* #define T_PREEMPT       0x00000000   Preemptible */
/* #define T_TSLICE        0x00000002   Time-slicing enabled bit */
/* #define T_NOTSLICE      0x00000000   No Time-slicing */
/* #define T_NOASR         0x00000004   ASRs disabled bit */
/* #define T_ASR           0x00000000   ASRs enabled */

/* #define SM_GLOBAL       0x00000001  1 = Global */
/* #define SM_LOCAL        0x00000000  0 = Local */
/* #define SM_PRIOR        0x00000002  Queue by priority */
/* #define SM_FIFO         0x00000000  Queue by FIFO order */

/* #define T_NOFPU         0x00000000   Not using FPU */
/* #define T_FPU           0x00000002   Using FPU bit */

#   elif defined (VXWORKS)
// For mutex implementation using mutual-exclusion semaphores (which
// can be taken recursively).
#     include /**/ <semLib.h>

#     include /**/ <envLib.h>
#     include /**/ <hostLib.h>
#     include /**/ <ioLib.h>
#     include /**/ <remLib.h>
#     include /**/ <selectLib.h>
#     include /**/ <sigLib.h>
#     include /**/ <sockLib.h>
#     include /**/ <sysLib.h>
#     include /**/ <taskLib.h>
#     include /**/ <taskHookLib.h>

extern "C"
struct sockaddr_un {
  short sun_family;    // AF_UNIX.
  char  sun_path[108]; // path name.
};

#     define MAXPATHLEN  1024
#     define MAXNAMLEN   255
#     define NSIG (_NSIGS + 1)

// task options:  the other options are either obsolete, internal, or for
// Fortran or Ada support
#     define VX_UNBREAKABLE        0x0002  /* breakpoints ignored */
#     define VX_FP_TASK            0x0008  /* floating point coprocessor */
#     define VX_PRIVATE_ENV        0x0080  /* private environment support */
#     define VX_NO_STACK_FILL      0x0100  /* do not stack fill for
                                              checkstack () */

#     define THR_CANCEL_DISABLE      0
#     define THR_CANCEL_ENABLE       0
#     define THR_CANCEL_DEFERRED     0
#     define THR_CANCEL_ASYNCHRONOUS 0
#     define THR_BOUND               0
#     define THR_NEW_LWP             0
#     define THR_DETACHED            0
#     define THR_SUSPENDED           0
#     define THR_DAEMON              0
#     define THR_JOINABLE            0
#     define THR_SCHED_FIFO          0
#     define THR_SCHED_RR            0
#     define THR_SCHED_DEFAULT       0
#     define THR_INHERIT_SCHED       0
#     define THR_EXPLICIT_SCHED      0
#     define THR_SCHED_IO            0
#     define THR_SCOPE_SYSTEM        0
#     define THR_SCOPE_PROCESS       0
#     define USYNC_THREAD            0
#     define USYNC_PROCESS           1 /* It's all global on VxWorks
                                          (without MMU option). */

#     if !defined (ACE_DEFAULT_SYNCH_TYPE)
 // Types include these options: SEM_Q_PRIORITY, SEM_Q_FIFO,
 // SEM_DELETE_SAFE, and SEM_INVERSION_SAFE.  SEM_Q_FIFO is
 // used as the default because that is VxWorks' default.
#       define ACE_DEFAULT_SYNCH_TYPE SEM_Q_FIFO
#     endif /* ! ACE_DEFAULT_SYNCH_TYPE */

typedef SEM_ID ACE_mutex_t;
// Implement ACE_thread_mutex_t with ACE_mutex_t because there's just
// one process . . .
typedef ACE_mutex_t ACE_thread_mutex_t;
#     if !defined (ACE_HAS_POSIX_SEM)
// Use VxWorks semaphores, wrapped ...
typedef struct
{
  /// Semaphore handle.  This is allocated by VxWorks.
  SEM_ID sema_;

  /// Name of the semaphore:  always NULL with VxWorks.
  char *name_;
} ACE_sema_t;
#     endif /* !ACE_HAS_POSIX_SEM */
typedef char * ACE_thread_t;
typedef int ACE_hthread_t;
// Key type: the ACE TSS emulation requires the key type be unsigned,
// for efficiency.  (Current POSIX and Solaris TSS implementations also
// use u_int, so the ACE TSS emulation is compatible with them.)
typedef u_int ACE_thread_key_t;

      // Marker for ACE_Thread_Manager to indicate that it allocated
      // an ACE_thread_t.  It is placed at the beginning of the ID.
#     define ACE_THR_ID_ALLOCATED '\022'

#   elif defined (ACE_HAS_WTHREADS)

typedef CRITICAL_SECTION ACE_thread_mutex_t;

typedef struct
{
  /// Either USYNC_THREAD or USYNC_PROCESS
  int type_;
  union
  {
    HANDLE proc_mutex_;
    CRITICAL_SECTION thr_mutex_;
  };
} ACE_mutex_t;

// Wrapper for NT Events.
typedef HANDLE ACE_event_t;

#   if defined (ACE_WIN32)
//@@ ACE_USES_WINCE_SEMA_SIMULATION is used to debug
//   semaphore simulation on WinNT.  It should be
//   changed to ACE_USES_HAS_WINCE at some later point.
#     if !defined (ACE_USES_WINCE_SEMA_SIMULATION)
typedef HANDLE ACE_sema_t;
#     else
/**
 * @class ACE_sema_t
 *
 * @brief Semaphore simulation for Windows CE.
 */
class ACE_OS_Export ACE_sema_t
{
public:
  /// Serializes access to <count_>.
  ACE_thread_mutex_t lock_;

  /// This event is signaled whenever the count becomes non-zero.
  ACE_event_t count_nonzero_;

  /// Current count of the semaphore.
  u_int count_;
};

#     endif /* ACE_USES_WINCE_SEMA_SIMULATION */
#   endif /* defined (ACE_WIN32) */

// These need to be different values, neither of which can be 0...
#     define USYNC_THREAD 1
#     define USYNC_PROCESS 2

#     define THR_CANCEL_DISABLE      0
#     define THR_CANCEL_ENABLE       0
#     define THR_CANCEL_DEFERRED     0
#     define THR_CANCEL_ASYNCHRONOUS 0
#     define THR_DETACHED            0x02000000 /* ignore in most places */
#     define THR_BOUND               0          /* ignore in most places */
#     define THR_NEW_LWP             0          /* ignore in most places */
#     define THR_DAEMON              0          /* ignore in most places */
#     define THR_JOINABLE            0          /* ignore in most places */
#     define THR_SUSPENDED   CREATE_SUSPENDED
#     define THR_USE_AFX             0x01000000
#     define THR_SCHED_FIFO          0
#     define THR_SCHED_RR            0
#     define THR_SCHED_DEFAULT       0
#     define THR_SCOPE_PROCESS       0
#     define THR_SCOPE_SYSTEM        0
#   endif /* ACE_HAS_PTHREADS / STHREADS / PSOS / VXWORKS / WTHREADS */

// If we're using PACE then we don't want this class (since PACE
// takes care of it) unless we're on Windows. Win32 mutexes, semaphores,
// and condition variables are not yet supported in PACE.
#   if defined (ACE_LACKS_COND_T)
/**
 * @class ACE_cond_t
 *
 * @brief This structure is used to implement condition variables on
 * platforms that lack it natively, such as VxWorks, pSoS, and
 * Win32.
 *
 * At the current time, this stuff only works for threads
 * within the same process.
 */
class ACE_OS_Export ACE_cond_t
{
public:
  friend class ACE_OS;

  /// Returns the number of waiters.
  long waiters (void) const;

protected:
  /// Number of waiting threads.
  long waiters_;

  /// Serialize access to the waiters count.
  ACE_thread_mutex_t waiters_lock_;

  /// Queue up threads waiting for the condition to become signaled.
  ACE_sema_t sema_;

#     if defined (VXWORKS) || defined (ACE_PSOS)
  /**
   * A semaphore used by the broadcast/signal thread to wait for all
   * the waiting thread(s) to wake up and be released from the
   * semaphore.
   */
  ACE_sema_t waiters_done_;
#     elif defined (ACE_WIN32)
  /**
   * An auto reset event used by the broadcast/signal thread to wait
   * for the waiting thread(s) to wake up and get a chance at the
   * semaphore.
   */
  HANDLE waiters_done_;
#     else
#       error "Please implement this feature or check your config.h file!"
#     endif /* VXWORKS || ACE_PSOS */

  /// Keeps track of whether we were broadcasting or just signaling.
  size_t was_broadcast_;
};

struct ACE_OS_Export ACE_condattr_t
{
  int type;
};

struct ACE_OS_Export ACE_mutexattr_t
{
  int type;
};
#   endif /* ACE_LACKS_COND_T */

#   if defined (ACE_LACKS_RWLOCK_T) && !defined (ACE_HAS_PTHREADS_UNIX98_EXT)

/**
 * @class ACE_rwlock_t
 *
 * @brief This is used to implement readers/writer locks on NT,
 *     VxWorks, and POSIX pthreads.
 *
 *     At the current time, this stuff only works for threads
 *     within the same process.
 */
struct ACE_OS_Export ACE_rwlock_t
{
protected:
  friend class ACE_OS;

  ACE_mutex_t lock_;
  // Serialize access to internal state.

  ACE_cond_t waiting_readers_;
  // Reader threads waiting to acquire the lock.

  int num_waiting_readers_;
  // Number of waiting readers.

  ACE_cond_t waiting_writers_;
  // Writer threads waiting to acquire the lock.

  int num_waiting_writers_;
  // Number of waiting writers.

  int ref_count_;
  // Value is -1 if writer has the lock, else this keeps track of the
  // number of readers holding the lock.

  int important_writer_;
  // indicate that a reader is trying to upgrade

  ACE_cond_t waiting_important_writer_;
  // condition for the upgrading reader
};
#   elif defined (ACE_HAS_PTHREADS_UNIX98_EXT)
typedef pthread_rwlock_t ACE_rwlock_t;
#   elif defined (ACE_HAS_STHREADS)
#     include /**/ <synch.h>
typedef rwlock_t ACE_rwlock_t;
#   endif /* ACE_LACKS_RWLOCK_T */

// Define some default thread priorities on all threaded platforms, if
// not defined above or in the individual platform config file.
// ACE_THR_PRI_FIFO_DEF should be used by applications for default
// real-time thread priority.  ACE_THR_PRI_OTHER_DEF should be used
// for non-real-time priority.
#   if !defined(ACE_THR_PRI_FIFO_DEF)
#     if defined (ACE_WTHREADS)
      // It would be more in spirit to use THREAD_PRIORITY_NORMAL.  But,
      // using THREAD_PRIORITY_ABOVE_NORMAL should give preference to the
      // threads in this process, even if the process is not in the
      // REALTIME_PRIORITY_CLASS.
#       define ACE_THR_PRI_FIFO_DEF THREAD_PRIORITY_ABOVE_NORMAL
#     else  /* ! ACE_WTHREADS */
#       define ACE_THR_PRI_FIFO_DEF 0
#     endif /* ! ACE_WTHREADS */
#   endif /* ! ACE_THR_PRI_FIFO_DEF */

#   if !defined(ACE_THR_PRI_OTHER_DEF)
#     if defined (ACE_WTHREADS)
      // It would be more in spirit to use THREAD_PRIORITY_NORMAL.  But,
      // using THREAD_PRIORITY_ABOVE_NORMAL should give preference to the
      // threads in this process, even if the process is not in the
      // REALTIME_PRIORITY_CLASS.
#       define ACE_THR_PRI_OTHER_DEF THREAD_PRIORITY_NORMAL
#     else  /* ! ACE_WTHREADS */
#       define ACE_THR_PRI_OTHER_DEF 0
#     endif /* ! ACE_WTHREADS */
#   endif /* ! ACE_THR_PRI_OTHER_DEF */

#if defined (ACE_HAS_RECURSIVE_MUTEXES)
typedef ACE_thread_mutex_t ACE_recursive_thread_mutex_t;
#else
/**
 * @class ACE_recursive_thread_mutex_t
 *
 * @brief Implement a thin C++ wrapper that allows nested acquisition
 * and release of a mutex that occurs in the same thread.
 *
 * This implementation is based on an algorithm sketched by Dave
 * Butenhof <butenhof@zko.dec.com>.  Naturally, I take the
 * credit for any mistakes ;-)
 */
class ACE_recursive_thread_mutex_t
{
public:
  /// Guards the state of the nesting level and thread id.
  ACE_thread_mutex_t nesting_mutex_;

  /// This condition variable suspends other waiting threads until the
  /// mutex is available.
  ACE_cond_t lock_available_;

  /// Current nesting level of the recursion.
  int nesting_level_;

  /// Current owner of the lock.
  ACE_thread_t owner_id_;
};
#endif /* ACE_HAS_RECURSIVE_MUTEXES */

# else /* !ACE_HAS_THREADS, i.e., the OS/platform doesn't support threading. */

// Give these things some reasonable value...
#   define ACE_SCOPE_PROCESS 0
#   define ACE_SCOPE_LWP 1
#   define ACE_SCOPE_THREAD 2
#   define ACE_SCHED_OTHER 0
#   define ACE_SCHED_FIFO 1
#   define ACE_SCHED_RR 2
#   if !defined (THR_CANCEL_DISABLE)
#     define THR_CANCEL_DISABLE      0
#   endif /* ! THR_CANCEL_DISABLE */
#   if !defined (THR_CANCEL_ENABLE)
#     define THR_CANCEL_ENABLE       0
#   endif /* ! THR_CANCEL_ENABLE */
#   if !defined (THR_CANCEL_DEFERRED)
#     define THR_CANCEL_DEFERRED     0
#   endif /* ! THR_CANCEL_DEFERRED */
#   if !defined (THR_CANCEL_ASYNCHRONOUS)
#     define THR_CANCEL_ASYNCHRONOUS 0
#   endif /* ! THR_CANCEL_ASYNCHRONOUS */
#   if !defined (THR_JOINABLE)
#     define THR_JOINABLE    0     /* ignore in most places */
#   endif /* ! THR_JOINABLE */
#   if !defined (THR_DETACHED)
#     define THR_DETACHED    0     /* ignore in most places */
#   endif /* ! THR_DETACHED */
#   if !defined (THR_DAEMON)
#     define THR_DAEMON      0     /* ignore in most places */
#   endif /* ! THR_DAEMON */
#   if !defined (THR_BOUND)
#     define THR_BOUND       0     /* ignore in most places */
#   endif /* ! THR_BOUND */
#   if !defined (THR_NEW_LWP)
#     define THR_NEW_LWP     0     /* ignore in most places */
#   endif /* ! THR_NEW_LWP */
#   if !defined (THR_SUSPENDED)
#     define THR_SUSPENDED   0     /* ignore in most places */
#   endif /* ! THR_SUSPENDED */
#   if !defined (THR_SCHED_FIFO)
#     define THR_SCHED_FIFO 0
#   endif /* ! THR_SCHED_FIFO */
#   if !defined (THR_SCHED_RR)
#     define THR_SCHED_RR 0
#   endif /* ! THR_SCHED_RR */
#   if !defined (THR_SCHED_DEFAULT)
#     define THR_SCHED_DEFAULT 0
#   endif /* ! THR_SCHED_DEFAULT */
#   if !defined (USYNC_THREAD)
#     define USYNC_THREAD 0
#   endif /* ! USYNC_THREAD */
#   if !defined (USYNC_PROCESS)
#     define USYNC_PROCESS 0
#   endif /* ! USYNC_PROCESS */
#   if !defined (THR_SCOPE_PROCESS)
#     define THR_SCOPE_PROCESS 0
#   endif /* ! THR_SCOPE_PROCESS */
#   if !defined (THR_SCOPE_SYSTEM)
#     define THR_SCOPE_SYSTEM 0
#   endif /* ! THR_SCOPE_SYSTEM */

// These are dummies needed for class OS.h
typedef int ACE_cond_t;
struct ACE_OS_Export ACE_condattr_t
{
  int type;
};
struct ACE_OS_Export ACE_mutexattr_t
{
  int type;
};
typedef int ACE_mutex_t;
typedef int ACE_thread_mutex_t;
typedef int ACE_recursive_thread_mutex_t;
#   if !defined (ACE_HAS_POSIX_SEM) && !defined (ACE_PSOS)
typedef int ACE_sema_t;
#   endif /* !ACE_HAS_POSIX_SEM && !ACE_PSOS */
typedef int ACE_rwlock_t;
typedef int ACE_thread_t;
typedef int ACE_hthread_t;
typedef u_int ACE_thread_key_t;

// Ensure that ACE_THR_PRI_FIFO_DEF and ACE_THR_PRI_OTHER_DEF are
// defined on non-threaded platforms, to support application source
// code compatibility.  ACE_THR_PRI_FIFO_DEF should be used by
// applications for default real-time thread priority.
// ACE_THR_PRI_OTHER_DEF should be used for non-real-time priority.
#   if !defined(ACE_THR_PRI_FIFO_DEF)
#     define ACE_THR_PRI_FIFO_DEF 0
#   endif /* ! ACE_THR_PRI_FIFO_DEF */
#   if !defined(ACE_THR_PRI_OTHER_DEF)
#     define ACE_THR_PRI_OTHER_DEF 0
#   endif /* ! ACE_THR_PRI_OTHER_DEF */

# endif /* ACE_HAS_THREADS */

# if defined (ACE_PSOS)

// Wrapper for NT events on pSOS.
class ACE_OS_Export ACE_event_t
{
  friend class ACE_OS;

protected:

  /// Protect critical section.
  ACE_mutex_t lock_;

  /// Keeps track of waiters.
  ACE_cond_t condition_;

  /// Specifies if this is an auto- or manual-reset event.
  int manual_reset_;

  /// "True" if signaled.
  int is_signaled_;

  /// Number of waiting threads.
  u_long waiting_threads_;
};

# endif /* ACE_PSOS */

// Standard C Library includes
// NOTE: stdarg.h must be #included before stdio.h on LynxOS.
# include /**/ <stdarg.h>
# if !defined (ACE_HAS_WINCE)
#   include /**/ <assert.h>
#   include /**/ <stdio.h>
// this is a nasty hack to get around problems with the
// pSOS definition of BUFSIZ as the config table entry
// (which is valued using the LC_BUFSIZ value anyway)
#   if defined (ACE_PSOS)
#     if defined (BUFSIZ)
#       undef BUFSIZ
#     endif /* defined (BUFSIZ) */
#     define BUFSIZ LC_BUFSIZ
#   endif /* defined (ACE_PSOS) */

#if defined (ACE_PSOS_DIAB_MIPS)
#undef size_t
typedef unsigned int size_t;
#endif

#   if !defined (ACE_LACKS_NEW_H)
#     if defined (ACE_USES_STD_NAMESPACE_FOR_STDCPP_LIB)
#       include /**/ <new>
#     else
#       include /**/ <new.h>
#     endif /* ACE_USES_STD_NAMESPACE_FOR_STDCPP_LIB */
#   endif /* ! ACE_LACKS_NEW_H */

#   if !defined (ACE_PSOS_DIAB_MIPS)  &&  !defined (VXWORKS)
#   define ACE_DONT_INCLUDE_ACE_SIGNAL_H
#     include /**/ <signal.h>
#   undef ACE_DONT_INCLUDE_ACE_SIGNAL_H
#   endif /* ! ACE_PSOS_DIAB_MIPS && ! VXWORKS */

#   if ! defined (ACE_PSOS_DIAB_MIPS)
#   include /**/ <fcntl.h>
#   endif /* ! ACE_PSOS_DIAB_MIPS */
# endif /* ACE_HAS_WINCE */

# include /**/ <limits.h>
# include /**/ <ctype.h>
# if ! defined (ACE_PSOS_DIAB_MIPS)
# include /**/ <string.h>
# include /**/ <stdlib.h>
# endif /* ! ACE_PSOS_DIAB_MIPS */
# include /**/ <float.h>

// This is defined by XOPEN to be a minimum of 16.  POSIX.1g
// also defines this value.  platform-specific config.h can
// override this if need be.
# if !defined (IOV_MAX)
#  define IOV_MAX 16
# endif /* IOV_MAX */

# if !defined (ACE_IOV_MAX)
#define ACE_IOV_MAX IOV_MAX
# endif /* ACE_IOV_MAX */

# if defined (ACE_PSOS_SNARFS_HEADER_INFO)
  // Header information snarfed from compiler provided header files
  // that are not included because there is already an identically
  // named file provided with pSOS, which does not have this info
  // from compiler supplied stdio.h
  extern FILE *fdopen(int, const char *);
  extern int getopt(int, char *const *, const char *);
  extern char *tempnam(const char *, const char *);
  extern "C" int fileno(FILE *);

//  #define fileno(stream) ((stream)->_file)

  // from compiler supplied string.h
  extern char *strdup (const char *);

  // from compiler supplied stat.h
  extern mode_t umask (mode_t);
  extern int mkfifo (const char *, mode_t);
  extern int mkdir (const char *, mode_t);

  // from compiler supplied stdlib.h
  extern int putenv (char *);

  int isatty (int h);

# endif /* ACE_PSOS_SNARFS_HEADER_INFO */

# if defined (ACE_NEEDS_SCHED_H)
#   include /**/ <sched.h>
# endif /* ACE_NEEDS_SCHED_H */

#if !defined (ACE_OSTREAM_TYPE)
# if defined (ACE_LACKS_IOSTREAM_TOTALLY)
#   define ACE_OSTREAM_TYPE FILE
# else  /* ! ACE_LACKS_IOSTREAM_TOTALLY */
#   define ACE_OSTREAM_TYPE ostream
# endif /* ! ACE_LACKS_IOSTREAM_TOTALLY */
#endif /* ! ACE_OSTREAM_TYPE */

#if !defined (ACE_DEFAULT_LOG_STREAM)
# if defined (ACE_LACKS_IOSTREAM_TOTALLY)
#   define ACE_DEFAULT_LOG_STREAM 0
# else  /* ! ACE_LACKS_IOSTREAM_TOTALLY */
#   define ACE_DEFAULT_LOG_STREAM (&cerr)
# endif /* ! ACE_LACKS_IOSTREAM_TOTALLY */
#endif /* ! ACE_DEFAULT_LOG_STREAM */

// If the user wants minimum IOStream inclusion, we will just include
// the forward declarations
# if defined (ACE_HAS_MINIMUM_IOSTREAMH_INCLUSION)
// Forward declaration for streams
#   include "ace/iosfwd.h"
# else /* ACE_HAS_MINIMUM_IOSTREAMH_INCLUSION */
// Else they will get all the stream header files
#   include "ace/streams.h"
# endif /* ACE_HAS_MINIMUM_IOSTREAMH_INCLUSION */

# if !defined (ACE_HAS_WINCE)
#   if ! defined (ACE_PSOS_DIAB_MIPS)
#   include /**/ <fcntl.h>
#   endif /* ! ACE_PSOS_DIAB_MIPS */
# endif /* ACE_HAS_WINCE */

// This must come after signal.h is #included.
# if defined (SCO)
#   define SIGIO SIGPOLL
#   include /**/ <sys/regset.h>
# endif /* SCO */

# if defined ACE_HAS_BYTESEX_H
#   include /**/ <bytesex.h>
# endif /* ACE_HAS_BYTESEX_H */
# include "ace/Basic_Types.h"

/* This should work for linux, solaris 5.6 and above, IRIX, OSF */
# if defined (ACE_HAS_LLSEEK) || defined (ACE_HAS_LSEEK64)
#   if ACE_SIZEOF_LONG == 8
      typedef off_t ACE_LOFF_T;
#   elif defined (__sgi) || defined (AIX) || defined (HPUX) \
    || defined (__QNX__)
      typedef off64_t ACE_LOFF_T;
#   elif defined (__sun)
      typedef offset_t ACE_LOFF_T;
#   elif defined (WIN32) //Add by Nick Lin -- for win32 llseek
      typedef __int64  ACE_LOFF_T;  //Add by Nick Lin -- for win32 llseek
#   else
      typedef loff_t ACE_LOFF_T;
#   endif
# endif /* ACE_HAS_LLSEEK || ACE_HAS_LSEEK64 */

// Type-safe, and unsigned.
static const ACE_UINT32 ACE_U_ONE_SECOND_IN_MSECS = 1000U;
static const ACE_UINT32 ACE_U_ONE_SECOND_IN_USECS = 1000000U;
static const ACE_UINT32 ACE_U_ONE_SECOND_IN_NSECS = 1000000000U;

# if defined (ACE_HAS_SIG_MACROS)
#   undef sigemptyset
#   undef sigfillset
#   undef sigaddset
#   undef sigdelset
#   undef sigismember
# endif /* ACE_HAS_SIG_MACROS */

// This must come after signal.h is #included.  It's to counteract
// the sigemptyset and sigfillset #defines, which only happen
// when __OPTIMIZE__ is #defined (really!) on Linux.
# if defined (linux) && defined (__OPTIMIZE__)
#   undef sigemptyset
#   undef sigfillset
# endif /* linux && __OPTIMIZE__ */

// Prototypes should come after ace/Basic_Types.h since some types may
// be used in the prototypes.

#if defined (ACE_LACKS_GETPGID_PROTOTYPE) && \
    !defined (_XOPEN_SOURCE) && !defined (_XOPEN_SOURCE_EXTENDED)
extern "C" pid_t getpgid (pid_t pid);
#endif  /* ACE_LACKS_GETPGID_PROTOTYPE &&
           !_XOPEN_SOURCE && !_XOPEN_SOURCE_EXTENDED */

#if defined (ACE_LACKS_STRPTIME_PROTOTYPE) && !defined (_XOPEN_SOURCE)
extern "C" char *strptime (const char *s, const char *fmt, struct tm *tp);
#endif  /* ACE_LACKS_STRPTIME_PROTOTYPE */

#if defined (ACE_LACKS_STRTOK_R_PROTOTYPE) && !defined (_POSIX_SOURCE)
extern "C" char *strtok_r (char *s, const char *delim, char **save_ptr);
#endif  /* ACE_LACKS_STRTOK_R_PROTOTYPE */

#if !defined (_LARGEFILE64_SOURCE)
# if defined (ACE_LACKS_LSEEK64_PROTOTYPE) && \
     defined (ACE_LACKS_LLSEEK_PROTOTYPE)
#   error Define either ACE_LACKS_LSEEK64_PROTOTYPE or ACE_LACKS_LLSEEK_PROTOTYPE, not both!
# elif defined (ACE_LACKS_LSEEK64_PROTOTYPE)
extern "C" ACE_LOFF_T lseek64 (int fd, ACE_LOFF_T offset, int whence);
# elif defined (ACE_LACKS_LLSEEK_PROTOTYPE)
extern "C" ACE_LOFF_T llseek (int fd, ACE_LOFF_T offset, int whence);
# endif
#endif  /* _LARGEFILE64_SOURCE */

#if defined (ACE_LACKS_PREAD_PROTOTYPE) && (_XOPEN_SOURCE - 0) < 500
// _XOPEN_SOURCE == 500    Single Unix conformance
// It seems that _XOPEN_SOURCE == 500 means that the prototypes are
// already defined in the system headers.
extern "C" ssize_t pread (int fd,
                          void *buf,
                          size_t nbytes,
                          off_t offset);

extern "C" ssize_t pwrite (int fd,
                           const void *buf,
                           size_t n,
                           off_t offset);
#endif  /* ACE_LACKS_PREAD_PROTOTYPE && (_XOPEN_SOURCE - 0) < 500 */

# if defined (ACE_LACKS_UALARM_PROTOTYPE)
extern "C" u_int ualarm (u_int usecs, u_int interval);
# endif /* ACE_LACKS_UALARM_PROTOTYPE */

# if defined (ACE_HAS_BROKEN_SENDMSG)
typedef struct msghdr ACE_SENDMSG_TYPE;
# else
typedef const struct msghdr ACE_SENDMSG_TYPE;
# endif /* ACE_HAS_BROKEN_SENDMSG */

# if defined (ACE_HAS_BROKEN_RANDR)
// The SunOS 5.4.X version of rand_r is inconsistent with the header
// files...
typedef u_int ACE_RANDR_TYPE;
extern "C" int rand_r (ACE_RANDR_TYPE seed);
# else
#   if defined (HPUX_10)
// HP-UX 10.x's stdlib.h (long *) doesn't match that man page (u_int *)
typedef long ACE_RANDR_TYPE;
#   else
typedef u_int ACE_RANDR_TYPE;
#   endif /* HPUX_10 */
# endif /* ACE_HAS_BROKEN_RANDR */

# if defined (ACE_HAS_UTIME)
#   include /**/ <utime.h>
# endif /* ACE_HAS_UTIME */

# if !defined (ACE_HAS_MSG) && !defined (SCO)
struct msghdr {};
# endif /* ACE_HAS_MSG */

# if defined (ACE_HAS_MSG) && defined (ACE_LACKS_MSG_ACCRIGHTS)
#   if !defined (msg_accrights)
#     undef msg_control
#     define msg_accrights msg_control
#   endif /* ! msg_accrights */

#   if !defined (msg_accrightslen)
#     undef msg_controllen
#     define msg_accrightslen msg_controllen
#   endif /* ! msg_accrightslen */
# endif /* ACE_HAS_MSG && ACE_LACKS_MSG_ACCRIGHTS */

# if !defined (ACE_HAS_SIG_ATOMIC_T)
typedef int sig_atomic_t;
# endif /* !ACE_HAS_SIG_ATOMIC_T */

# if defined (ACE_HAS_CONSISTENT_SIGNAL_PROTOTYPES)
// Prototypes for both signal() and struct sigaction are consistent..
#   if defined (ACE_HAS_SIG_C_FUNC)
extern "C" {
#   endif /* ACE_HAS_SIG_C_FUNC */
#   if !defined (ACE_PSOS)
typedef void (*ACE_SignalHandler)(int);
typedef void (*ACE_SignalHandlerV)(int);
#   endif /* !defined (ACE_PSOS) */
#   if defined (ACE_HAS_SIG_C_FUNC)
}
#   endif /* ACE_HAS_SIG_C_FUNC */
# elif defined (ACE_HAS_LYNXOS_SIGNALS)
typedef void (*ACE_SignalHandler)(...);
typedef void (*ACE_SignalHandlerV)(...);
# elif defined (ACE_HAS_TANDEM_SIGNALS)
typedef void (*ACE_SignalHandler)(...);
typedef void (*ACE_SignalHandlerV)(...);
# elif defined (ACE_HAS_IRIX_53_SIGNALS)
typedef void (*ACE_SignalHandler)(...);
typedef void (*ACE_SignalHandlerV)(...);
# elif defined (ACE_HAS_SPARCWORKS_401_SIGNALS)
typedef void (*ACE_SignalHandler)(int, ...);
typedef void (*ACE_SignalHandlerV)(int,...);
# elif defined (ACE_HAS_SUNOS4_SIGNAL_T)
typedef void (*ACE_SignalHandler)(...);
typedef void (*ACE_SignalHandlerV)(...);
# elif defined (ACE_HAS_SVR4_SIGNAL_T)
// SVR4 Signals are inconsistent (e.g., see struct sigaction)..
typedef void (*ACE_SignalHandler)(int);
#   if !defined (m88k)     /*  with SVR4_SIGNAL_T */
typedef void (*ACE_SignalHandlerV)(void);
#   else
typedef void (*ACE_SignalHandlerV)(int);
#   endif  /*  m88k */       /*  with SVR4_SIGNAL_T */
# elif defined (ACE_WIN32)
typedef void (__cdecl *ACE_SignalHandler)(int);
typedef void (__cdecl *ACE_SignalHandlerV)(int);
# elif defined (ACE_HAS_UNIXWARE_SVR4_SIGNAL_T)
typedef void (*ACE_SignalHandler)(int);
typedef void (*ACE_SignalHandlerV)(...);
# else /* This is necessary for some older broken version of cfront */
#   if defined (SIG_PF)
#     define ACE_SignalHandler SIG_PF
#   else
typedef void (*ACE_SignalHandler)(int);
#   endif /* SIG_PF */
typedef void (*ACE_SignalHandlerV)(...);
# endif /* ACE_HAS_CONSISTENT_SIGNAL_PROTOTYPES */

# if defined (BUFSIZ)
#   define ACE_STREAMBUF_SIZE BUFSIZ
# else
#   define ACE_STREAMBUF_SIZE 1024
# endif /* BUFSIZ */

# if defined (ACE_WIN32)
// Turn off warnings for /W4
// To resume any of these warning: #pragma warning(default: 4xxx)
// which should be placed after these defines

#   if !defined (ALL_WARNINGS) && defined(_MSC_VER) && !defined(ghs) && !defined(__MINGW32__)
// #pragma warning(disable: 4101)  // unreferenced local variable
#     pragma warning(disable: 4127)  /* constant expression for TRACE/ASSERT */
#     pragma warning(disable: 4134)  /* message map member fxn casts */
#     pragma warning(disable: 4511)  /* private copy constructors are good to have */
#     pragma warning(disable: 4512)  /* private operator= are good to have */
#     pragma warning(disable: 4514)  /* unreferenced inlines are common */
#     pragma warning(disable: 4710)  /* private constructors are disallowed */
#     pragma warning(disable: 4705)  /* statement has no effect in optimized code */
// #pragma warning(disable: 4701)  // local variable *may* be used without init
// #pragma warning(disable: 4702)  // unreachable code caused by optimizations
#     pragma warning(disable: 4791)  /* loss of debugging info in retail version */
// #pragma warning(disable: 4204)  // non-constant aggregate initializer
#     pragma warning(disable: 4275)  /* deriving exported class from non-exported */
#     pragma warning(disable: 4251)  /* using non-exported as public in exported */
#     pragma warning(disable: 4786)  /* identifier was truncated to '255' characters in the browser information */
#     pragma warning(disable: 4097)  /* typedef-name used as synonym for class-name */
#   endif /* !ALL_WARNINGS && _MSV_VER && !ghs && !__MINGW32__ */

// STRICT type checking in WINDOWS.H enhances type safety for Windows
// programs by using distinct types to represent all the different
// HANDLES in Windows. So for example, STRICT prevents you from
// mistakenly passing an HPEN to a routine expecting an HBITMAP.
// Note that we only use this if we
#   if defined (ACE_HAS_STRICT) && (ACE_HAS_STRICT != 0)
#     if !defined (STRICT)   /* may already be defined */
#       define STRICT
#     endif /* !STRICT */
#   endif /* ACE_HAS_STRICT */

#   if !defined (ACE_HAS_WINCE)
#     include /**/ <sys/timeb.h>
#   endif /* ACE_HAS_WINCE */

// Need to work around odd glitches with NT.
#   if !defined (ACE_MAX_DEFAULT_PORT)
#     define ACE_MAX_DEFAULT_PORT 65535
#   endif /* ACE_MAX_DEFAULT_PORT */

// We're on WinNT or Win95
#   define ACE_PLATFORM_A "Win32"
#   define ACE_PLATFORM_EXE_SUFFIX_A ".exe"

// Used for dynamic linking
#   if !defined (ACE_DEFAULT_SVC_CONF)
#     if (ACE_USES_CLASSIC_SVC_CONF == 1)
#       define ACE_DEFAULT_SVC_CONF ACE_LIB_TEXT (".\\svc.conf")
#     else
#       define ACE_DEFAULT_SVC_CONF ACE_LIB_TEXT (".\\svc.conf.xml")
#     endif /* ACE_USES_CLASSIC_SVC_CONF ==1 */
#   endif /* ACE_DEFAULT_SVC_CONF */

// The following are #defines and #includes that are specific to
// WIN32.
#   if defined (ACE_HAS_WINCE)
#     define ACE_STDIN  _fileno (stdin)
#     define ACE_STDOUT _fileno (stdout)
#     define ACE_STDERR _fileno (stderr)
#   else
#   define ACE_STDIN GetStdHandle (STD_INPUT_HANDLE)
#   define ACE_STDOUT GetStdHandle (STD_OUTPUT_HANDLE)
#   define ACE_STDERR GetStdHandle (STD_ERROR_HANDLE)
#   endif  // ACE_HAS_WINCE

// Default semaphore key and mutex name
#   if !defined (ACE_DEFAULT_SEM_KEY)
#     define ACE_DEFAULT_SEM_KEY "ACE_SEM_KEY"
#   endif /* ACE_DEFAULT_SEM_KEY */

#   define ACE_INVALID_SEM_KEY 0

#   if defined (ACE_HAS_WINCE)
// @@ WinCE probably doesn't have structural exception support
//    But I need to double check to find this out
#     define ACE_SEH_TRY if (1)
#     define ACE_SEH_EXCEPT(X) while (0)
#     define ACE_SEH_FINALLY if (1)
#   else
#     if !defined (ACE_HAS_WIN32_STRUCTURAL_EXCEPTIONS)
#       define ACE_SEH_TRY if (1)
#       define ACE_SEH_EXCEPT(X) while (0)
#       define ACE_SEH_FINALLY if (1)
#     elif defined(__BORLANDC__)
#       if (__BORLANDC__ >= 0x0530) /* Borland C++ Builder 3.0 */
#         define ACE_SEH_TRY try
#         define ACE_SEH_EXCEPT(X) __except(X)
#         define ACE_SEH_FINALLY __finally
#       else
#         define ACE_SEH_TRY if (1)
#         define ACE_SEH_EXCEPT(X) while (0)
#         define ACE_SEH_FINALLY if (1)
#       endif
#     elif defined (__IBMCPP__) && (__IBMCPP__ >= 400)
#         define ACE_SEH_TRY if (1)
#         define ACE_SEH_EXCEPT(X) while (0)
#         define ACE_SEH_FINALLY if (1)
#     else
#       define ACE_SEH_TRY __try
#       define ACE_SEH_EXCEPT(X) __except(X)
#       define ACE_SEH_FINALLY __finally
#     endif /* __BORLANDC__ */
#   endif /* ACE_HAS_WINCE */

// The "null" device on Win32.
#   define ACE_DEV_NULL "nul"

// Define the pathname separator characters for Win32 (ugh).
#   define ACE_DIRECTORY_SEPARATOR_STR_A "\\"
#   define ACE_DIRECTORY_SEPARATOR_CHAR_A '\\'
#   define ACE_LD_SEARCH_PATH ACE_LIB_TEXT ("PATH")
#   define ACE_LD_SEARCH_PATH_SEPARATOR_STR ACE_LIB_TEXT (";")
#   define ACE_DLL_SUFFIX ACE_LIB_TEXT (".dll")
#   if defined (__MINGW32__)
#     define ACE_DLL_PREFIX ACE_LIB_TEXT ("lib")
#   else /* __MINGW32__ */
#     define ACE_DLL_PREFIX ACE_LIB_TEXT ("")
#   endif /* __MINGW32__ */

// This will help until we figure out everything:
#   define NFDBITS 32 /* only used in unused functions... */
// These two may be used for internal flags soon:
#   define MAP_PRIVATE 1
#   define MAP_SHARED  2
#   define MAP_FIXED   4

#   define RUSAGE_SELF 1

struct shmaddr { };
struct msqid_ds {};

/// Fake the UNIX rusage structure.  Perhaps we can add more to this
/// later on?
struct rusage
{
  FILETIME ru_utime;
  FILETIME ru_stime;
};

// MMAP flags
#   define PROT_READ PAGE_READONLY
#   define PROT_WRITE PAGE_READWRITE
#   define PROT_RDWR PAGE_READWRITE
/* If we can find suitable use for these flags, here they are:
PAGE_WRITECOPY
PAGE_EXECUTE
PAGE_EXECUTE_READ
PAGE_EXECUTE_READWRITE
PAGE_EXECUTE_WRITECOPY
PAGE_GUARD
PAGE_NOACCESS
PAGE_NOCACHE  */

#   if defined (ACE_HAS_WINSOCK2) && (ACE_HAS_WINSOCK2 != 0)
#     include /**/ <ws2tcpip.h>
#   endif /* ACE_HAS_WINSOCK2 */

// error code mapping
#   define ETIME                   ERROR_SEM_TIMEOUT
#   define EWOULDBLOCK             WSAEWOULDBLOCK
#   define EINPROGRESS             WSAEINPROGRESS
#   define EALREADY                WSAEALREADY
#   define ENOTSOCK                WSAENOTSOCK
#   define EDESTADDRREQ            WSAEDESTADDRREQ
#   define EMSGSIZE                WSAEMSGSIZE
#   define EPROTOTYPE              WSAEPROTOTYPE
#   define ENOPROTOOPT             WSAENOPROTOOPT
#   define EPROTONOSUPPORT         WSAEPROTONOSUPPORT
#   define ESOCKTNOSUPPORT         WSAESOCKTNOSUPPORT
#   define EOPNOTSUPP              WSAEOPNOTSUPP
#   define EPFNOSUPPORT            WSAEPFNOSUPPORT
#   define EAFNOSUPPORT            WSAEAFNOSUPPORT
#   define EADDRINUSE              WSAEADDRINUSE
#   define EADDRNOTAVAIL           WSAEADDRNOTAVAIL
#   define ENETDOWN                WSAENETDOWN
#   define ENETUNREACH             WSAENETUNREACH
#   define ENETRESET               WSAENETRESET
#   define ECONNABORTED            WSAECONNABORTED
#   define ECONNRESET              WSAECONNRESET
#   define ENOBUFS                 WSAENOBUFS
#   define EISCONN                 WSAEISCONN
#   define ENOTCONN                WSAENOTCONN
#   define ESHUTDOWN               WSAESHUTDOWN
#   define ETOOMANYREFS            WSAETOOMANYREFS
#   define ETIMEDOUT               WSAETIMEDOUT
#   define ECONNREFUSED            WSAECONNREFUSED
#   define ELOOP                   WSAELOOP
#   define EHOSTDOWN               WSAEHOSTDOWN
#   define EHOSTUNREACH            WSAEHOSTUNREACH
#   define EPROCLIM                WSAEPROCLIM
#   define EUSERS                  WSAEUSERS
#   define EDQUOT                  WSAEDQUOT
#   define ESTALE                  WSAESTALE
#   define EREMOTE                 WSAEREMOTE
// Grrr! These two are already defined by the horrible 'standard'
// library.
// #define ENAMETOOLONG            WSAENAMETOOLONG
// #define ENOTEMPTY               WSAENOTEMPTY

#   if !defined (ACE_HAS_WINCE)
#     include /**/ <time.h>
#     include /**/ <direct.h>
#     include /**/ <process.h>
#     include /**/ <io.h>
#   endif /* ACE_HAS_WINCE */

#   if defined (__BORLANDC__)
#     include /**/ <fcntl.h>
#     define _chdir chdir
#     define _ftime ftime
#     undef _access
#     define _access access
#     if (__BORLANDC__ <= 0x540)
#       define _getcwd getcwd
#       define _stat stat
#     endif
#     define _isatty isatty
#     define _umask umask
#     define _fstat fstat
#     define _stricmp stricmp
#     define _strnicmp strnicmp

#     define _timeb timeb

#     define _O_CREAT O_CREAT
#     define _O_EXCL  O_EXCL
#     define _O_TRUNC O_TRUNC
      // 0x0800 is used for O_APPEND.  0x08 looks free.
#     define _O_TEMPORARY 0x08 /* see fcntl.h */
#     define _O_RDWR   O_RDWR
#     define _O_WRONLY O_WRONLY
#     define _O_RDONLY O_RDONLY
#     define _O_APPEND O_APPEND
#     define _O_BINARY O_BINARY
#     define _O_TEXT   O_TEXT
#   endif /* __BORLANDC__ */

typedef OVERLAPPED ACE_OVERLAPPED;

typedef DWORD ACE_thread_t;
#   if !defined(__MINGW32__)
typedef long pid_t;
#   endif /* __MINGW32__ */
typedef HANDLE ACE_hthread_t;

#define ACE_INVALID_PID ((pid_t) -1)
#   if defined (ACE_HAS_TSS_EMULATION)
      typedef DWORD ACE_OS_thread_key_t;
      typedef u_int ACE_thread_key_t;
#   else  /* ! ACE_HAS_TSS_EMULATION */
      typedef DWORD ACE_thread_key_t;
#   endif /* ! ACE_HAS_TSS_EMULATION */

#   if !defined (ACE_LACKS_LONGLONG_T)
// 64-bit quad-word definitions.
typedef unsigned __int64 ACE_QWORD;
typedef unsigned __int64 ACE_hrtime_t;
inline ACE_QWORD ACE_MAKE_QWORD (DWORD lo, DWORD hi) { return ACE_QWORD (lo) | (ACE_QWORD (hi) << 32); }
inline DWORD ACE_LOW_DWORD  (ACE_QWORD q) { return (DWORD) q; }
inline DWORD ACE_HIGH_DWORD (ACE_QWORD q) { return (DWORD) (q >> 32); }
#   else
// Can't find ANY place that ACE_QWORD is used, but hrtime_t is.
typedef ACE_UINT64 ACE_hrtime_t;
#   endif // ACE_LACKS_LONGLONG_T

// Win32 dummies to help compilation.

#   if !defined (__BORLANDC__)
typedef DWORD nlink_t;
#       if !defined(__MINGW32__)
typedef u_short mode_t;
#       endif /* !__MINGW32__ */
typedef long uid_t;
typedef long gid_t;
#   endif /* __BORLANDC__ */
typedef char *caddr_t;

typedef DWORD ACE_exitcode;
#   define ACE_SYSCALL_FAILED 0xFFFFFFFF

// Needed to map calls to NT transparently.
#   define MS_ASYNC 0
#   define MS_INVALIDATE 0

// Reliance on CRT - I don't really like this.

#   define O_NDELAY    1
#   if !defined (MAXPATHLEN)
#     define MAXPATHLEN  _MAX_PATH
#   endif /* !MAXPATHLEN */
#   define MAXNAMLEN   _MAX_FNAME
#   define EADDRINUSE WSAEADDRINUSE

/// The ordering of the fields in this struct is important.  It has to
/// match those in WSABUF.
struct iovec
{
  /// byte count to read/write
  size_t iov_len;
  /// data to be read/written
  char *iov_base;

  // WSABUF is a Winsock2-only type.
#if defined (ACE_HAS_WINSOCK2) && (ACE_HAS_WINSOCK2 != 0)
  operator WSABUF &(void) { return *((WSABUF *) this); }
#endif /* defined (ACE_HAS_WINSOCK2) && (ACE_HAS_WINSOCK2 != 0) */
};

struct msghdr
{
  /// Optional address
  sockaddr * msg_name;

  /// Size of address
  int msg_namelen;

  /// Scatter/gather array
  iovec *msg_iov;

  /// # elements in msg_iov
  int msg_iovlen;

  /// Access rights sent/received
  caddr_t msg_accrights;

  int msg_accrightslen;
};

typedef int ACE_idtype_t;
typedef DWORD ACE_id_t;
#   define ACE_SELF (0)
typedef int ACE_pri_t;

// Dynamic loading-related types - used for dlopen and family.
typedef HINSTANCE ACE_SHLIB_HANDLE;
#   define ACE_SHLIB_INVALID_HANDLE 0
#   define ACE_DEFAULT_SHLIB_MODE 0

# elif defined (ACE_PSOS)

typedef ACE_UINT64 ACE_hrtime_t;

#   if defined (ACE_SIGINFO_IS_SIGINFO_T)
  typedef struct siginfo siginfo_t;
#   endif /* ACE_LACKS_SIGINFO_H */

# else /* !defined (ACE_WIN32) && !defined (ACE_PSOS) */

#   if defined (m88k)
#     define RUSAGE_SELF 1
#   endif  /*  m88k */

// Default port is MAX_SHORT.
#   define ACE_MAX_DEFAULT_PORT 65535

// Default semaphore key
#   if !defined (ACE_DEFAULT_SEM_KEY)
#     define ACE_DEFAULT_SEM_KEY 1234
#   endif /* ACE_DEFAULT_SEM_KEY */

#   define ACE_INVALID_SEM_KEY -1

// Define the pathname separator characters for UNIX.
#   define ACE_DIRECTORY_SEPARATOR_STR_A "/"
#   define ACE_DIRECTORY_SEPARATOR_CHAR_A '/'

// We're some kind of UNIX...
#   define ACE_PLATFORM_A "UNIX"
#   define ACE_PLATFORM_EXE_SUFFIX_A ""

#   if !defined (ACE_LD_SEARCH_PATH)
#     define ACE_LD_SEARCH_PATH "LD_LIBRARY_PATH"
#   endif /* ACE_LD_SEARCH_PATH */
#   if !defined (ACE_LD_SEARCH_PATH_SEPARATOR_STR)
#     define ACE_LD_SEARCH_PATH_SEPARATOR_STR ":"
#   endif /* ACE_LD_SEARCH_PATH_SEPARATOR_STR */

#   if !defined (ACE_DLL_SUFFIX)
#     define ACE_DLL_SUFFIX ".so"
#   endif /* ACE_DLL_SUFFIX */
#   if !defined (ACE_DLL_PREFIX)
#     define ACE_DLL_PREFIX "lib"
#   endif /* ACE_DLL_PREFIX */

// Used for dynamic linking.
#   if !defined (ACE_DEFAULT_SVC_CONF)
#     if (ACE_USES_CLASSIC_SVC_CONF == 1)
#       define ACE_DEFAULT_SVC_CONF ACE_LIB_TEXT ("./svc.conf")
#     else
#       define ACE_DEFAULT_SVC_CONF ACE_LIB_TEXT ("./svc.conf.xml")
#     endif /* ACE_USES_CLASSIC_SVC_CONF ==1 */
#   endif /* ACE_DEFAULT_SVC_CONF */

// The following are #defines and #includes that are specific to UNIX.

#   define ACE_STDIN 0
#   define ACE_STDOUT 1
#   define ACE_STDERR 2

// Be consistent with Winsock naming
typedef int ACE_exitcode;
#   define ACE_INVALID_HANDLE -1
#   define ACE_SYSCALL_FAILED -1

#   define ACE_SEH_TRY if (1)
#   define ACE_SEH_EXCEPT(X) while (0)
#   define ACE_SEH_FINALLY if (1)

# if !defined (ACE_INVALID_PID)
# define ACE_INVALID_PID ((pid_t) -1)
# endif /* ACE_INVALID_PID */

// The "null" device on UNIX.
#   define ACE_DEV_NULL "/dev/null"

/**
 * @class ACE_event_t
 *
 * @brief Wrapper for NT events on UNIX.
 */
class ACE_OS_Export ACE_event_t
{
  friend class ACE_OS;
protected:
  /// Protect critical section.
  ACE_mutex_t lock_;

  /// Keeps track of waiters.
  ACE_cond_t condition_;

  /// Specifies if this is an auto- or manual-reset event.
  int manual_reset_;

  /// "True" if signaled.
  int is_signaled_;

  /// Number of waiting threads.
  u_long waiting_threads_;
};

struct ACE_OVERLAPPED
{
  u_long Internal;
  u_long InternalHigh;
  u_long Offset;
  u_long OffsetHigh;
  ACE_HANDLE hEvent;
};


// Add some typedefs and macros to enhance Win32 conformance...
#   if !defined (LPSECURITY_ATTRIBUTES)
#     define LPSECURITY_ATTRIBUTES int
#   endif /* !defined LPSECURITY_ATTRIBUTES */
#   if !defined (GENERIC_READ)
#     define GENERIC_READ 0
#   endif /* !defined GENERIC_READ */
#   if !defined (FILE_SHARE_READ)
#     define FILE_SHARE_READ 0
#   endif /* !defined FILE_SHARE_READ */
#   if !defined (OPEN_EXISTING)
#     define OPEN_EXISTING 0
#   endif /* !defined OPEN_EXISTING */
#   if !defined (FILE_ATTRIBUTE_NORMAL)
#     define FILE_ATTRIBUTE_NORMAL 0
#   endif /* !defined FILE_ATTRIBUTE_NORMAL */
#   if !defined (MAXIMUM_WAIT_OBJECTS)
#     define MAXIMUM_WAIT_OBJECTS 0
#   endif /* !defined MAXIMUM_WAIT_OBJECTS */
#   if !defined (FILE_FLAG_OVERLAPPED)
#     define FILE_FLAG_OVERLAPPED 0
#   endif /* !defined FILE_FLAG_OVERLAPPED */
#   if !defined (FILE_FLAG_SEQUENTIAL_SCAN)
#     define FILE_FLAG_SEQUENTIAL_SCAN 0
#   endif   /* FILE_FLAG_SEQUENTIAL_SCAN */

#   if defined (ACE_HAS_BROKEN_IF_HEADER)
struct ifafilt;
#   endif /* ACE_HAS_BROKEN_IF_HEADER */

#   if defined (ACE_HAS_AIX_BROKEN_SOCKET_HEADER)
#     undef __cplusplus
#     include /**/ <sys/socket.h>
#     define __cplusplus
#   else
#     include /**/ <sys/socket.h>
#   endif /* ACE_HAS_AIX_BROKEN_SOCKET_HEADER */

extern "C"
{
#   if defined (VXWORKS)
  struct  hostent {
    char    *h_name;        /* official name of host */
    char    **h_aliases;    /* aliases:  not used on VxWorks */
    int     h_addrtype;     /* host address type */
    int     h_length;       /* address length */
    char    **h_addr_list;  /* (first, only) address from name server */
#     define h_addr h_addr_list[0]   /* the first address */
  };
#   elif defined (ACE_HAS_CYGWIN32_SOCKET_H)
#     include /**/ <cygwin32/socket.h>
#   else
#     if defined (ACE_HAS_STL_QUEUE_CONFLICT)
#       define queue _Queue_
#     endif /* ACE_HAS_STL_QUEUE_CONFLICT */
#     include /**/ <netdb.h>
#     if defined (ACE_HAS_STL_QUEUE_CONFLICT)
#       undef queue
#     endif /* ACE_HAS_STL_QUEUE_CONFLICT */
#   endif /* VXWORKS */


// This part if to avoid STL name conflict with the map structure
// in net/if.h.
#   if defined (ACE_HAS_STL_MAP_CONFLICT)
#     define map _Resource_Allocation_Map_
#   endif /* ACE_HAS_STL_MAP_CONFLICT */
#   include /**/ <net/if.h>
#   if defined (ACE_HAS_STL_MAP_CONFLICT)
#     undef map
#   endif /* ACE_HAS_STL_MAP_CONFLICT */

#   if defined (ACE_HAS_STL_QUEUE_CONFLICT)
#     define queue _Queue_
#   endif /* ACE_HAS_STL_QUEUE_CONFLICT */
#   include /**/ <netinet/in.h>
#   if defined (ACE_HAS_STL_QUEUE_CONFLICT)
#     undef queue
#   endif /* ACE_HAS_STL_QUEUE_CONFLICT */

#   if defined (VXWORKS)
      // Work around a lack of ANSI prototypes for these functions on VxWorks.
      unsigned long  inet_addr (const char *);
      char           *inet_ntoa (const struct in_addr);
      struct in_addr inet_makeaddr (const int, const int);
      unsigned long  inet_network (const char *);
#   else  /* ! VXWORKS */
#     include /**/ <arpa/inet.h>
#   endif /* ! VXWORKS */
}
#   if !defined (ACE_LACKS_TCP_H)
#     if defined(ACE_HAS_CONFLICTING_XTI_MACROS)
#       if defined(TCP_NODELAY)
#         undef TCP_NODELAY
#       endif
#       if defined(TCP_MAXSEG)
#         undef TCP_MAXSEG
#       endif
#     endif
#     include /**/ <netinet/tcp.h>
#   endif /* ACE_LACKS_TCP_H */

#   if defined (__Lynx__)
#     ifndef howmany
#       define howmany(x, y)   (((x)+((y)-1))/(y))
#     endif /* howmany */
#   endif /* __Lynx__ */

#   if defined (CHORUS)
#     include /**/ <chorus.h>
#     if !defined(CHORUS_4)
#       include /**/ <cx/select.h>
#     else
#       include /**/ <stdio.h>
#     endif
#     include /**/ <sys/uio.h>
#     include /**/ <time.h>
#     include /**/ <stdfileio.h>
#     include /**/ <am/afexec.h>
#     include /**/ <sys/types.h>
#     include /**/ <sys/signal.h>
#     include /**/ <sys/wait.h>
#     include /**/ <pwd.h>
#     include /**/ <timer/chBench.h>
extern_C int      getgid          __((void));
extern_C int      getuid          __((void));
extern_C char*    getcwd          __((char* buf, size_t size));
extern_C int      pipe            __((int* fildes));
extern_C int      gethostname     __((char*, size_t));

// This must come after limits.h is included
#     define MAXPATHLEN _POSIX_PATH_MAX

#     if  !defined(CHORUS_4)
typedef cx_fd_mask fd_mask;
typedef void (*__sighandler_t)(int); // keep Signal compilation happy
#     endif
#     ifndef howmany
#       define howmany(x, y)   (((x)+((y)-1))/(y))
#     endif /* howmany */
#   elif defined (CYGWIN32)
#     include /**/ <sys/uio.h>
#     include /**/ <sys/file.h>
#     include /**/ <sys/time.h>
#     include /**/ <sys/resource.h>
#     include /**/ <sys/wait.h>
#     include /**/ <pwd.h>
#   elif defined (__QNX__)
#     include /**/ <sys/uio.h>
#     include /**/ <sys/ipc.h>
#     include /**/ <sys/sem.h>
#     include /**/ <sys/time.h>
#     include /**/ <sys/wait.h>
#     include /**/ <sys/resource.h>
#     include /**/ <pwd.h>
      // sets O_NDELAY
#     include /**/ <unix.h>
#     include /**/ <sys/param.h> /* for NBBY */
      typedef long fd_mask;
#     if !defined (NFDBITS)
#       define NFDBITS (sizeof(fd_mask) * NBBY)        /* bits per mask */
#     endif /* ! NFDBITS */
#     if !defined (howmany)
#       define howmany(x, y)   (((x)+((y)-1))/(y))
#     endif /* ! howmany */
#   elif defined(__rtems__)
#     include /**/ <sys/file.h>
#     include /**/ <sys/resource.h>
#     include /**/ <sys/fcntl.h>
#     include /**/ <sys/time.h>
#     include /**/ <sys/utsname.h>
#     include /**/ <sys/wait.h>
#     include /**/ <pwd.h>

extern "C"
{
  int select (int n, fd_set *readfds, fd_set *writefds,
              fd_set *exceptfds, const struct timeval *timeout);
};
#   elif ! defined (VXWORKS)
#     include /**/ <sys/uio.h>
#     include /**/ <sys/ipc.h>
#     if !defined(ACE_LACKS_SYSV_SHMEM)
// No reason to #include this if the platform lacks support for SHMEM
#       include /**/ <sys/shm.h>
#     endif /* ACE_LACKS_SYSV_SHMEM */
#     if ! defined (__MACOSX__)
#       include /**/ <sys/sem.h>
#     endif /* ! defined (__MACOSX__) */
#     include /**/ <sys/file.h>
#     include /**/ <sys/time.h>
#     include /**/ <sys/resource.h>
#     include /**/ <sys/wait.h>
#     include /**/ <pwd.h>
#   endif /* ! VXWORKS */
#   include /**/ <sys/ioctl.h>

// IRIX5 defines bzero() in this odd file...
#   if defined (ACE_HAS_BSTRING)
#     include /**/ <bstring.h>
#   endif /* ACE_HAS_BSTRING */

// AIX defines bzero() in this odd file...
#   if defined (ACE_HAS_STRINGS)
#     include /**/ <strings.h>
#   endif /* ACE_HAS_STRINGS */

#   if defined (ACE_HAS_TERM_IOCTLS)
#     if defined (__QNX__)
#       include /**/ <termios.h>
#     else  /* ! __QNX__ */
#       include /**/ <sys/termios.h>
#     endif /* ! __QNX__ */
#     if defined (HPUX)
#       include /**/ <sys/modem.h>
#     endif /* HPUX */
#   endif /* ACE_HAS_TERM_IOCTLS */

#if !defined (VMIN)
#define ACE_VMIN 4
#else
#define ACE_VMIN VMIN
#endif /* VMIN */

#if !defined (VTIME)
#define ACE_VTIME 5
#else
#define ACE_VTIME VTIME
#endif /* VTIME */

#   if defined (ACE_HAS_AIO_CALLS)
#     include /**/ <aio.h>
#   endif /* ACE_HAS_AIO_CALLS */

#   if !defined (ACE_LACKS_PARAM_H)
#     include /**/ <sys/param.h>
#   endif /* ACE_LACKS_PARAM_H */

#   if !defined (ACE_LACKS_UNIX_DOMAIN_SOCKETS) && !defined (VXWORKS)
#     include /**/ <sys/un.h>
#   endif /* ACE_LACKS_UNIX_DOMAIN_SOCKETS */

#   if defined (ACE_HAS_SIGINFO_T)
#     if !defined (ACE_LACKS_SIGINFO_H)
#       if defined (__QNX__)
#         include /**/ <sys/siginfo.h>
#       elif defined(__rtems__)
#         include /**/ <signal.h>
#       else  /* ! __QNX__ */
#         include /**/ <siginfo.h>
#       endif /* ! __QNX__ */
#     endif /* ACE_LACKS_SIGINFO_H */
#     if !defined (ACE_LACKS_UCONTEXT_H)
#       include /**/ <ucontext.h>
#     endif /* ACE_LACKS_UCONTEXT_H */
#   endif /* ACE_HAS_SIGINFO_T */

#   if defined (ACE_HAS_POLL)
#     include /**/ <poll.h>
#   endif /* ACE_HAS_POLL */

#   if defined (ACE_HAS_STREAMS)
#     if defined (AIX)
#       if !defined (_XOPEN_EXTENDED_SOURCE)
#         define _XOPEN_EXTENDED_SOURCE
#       endif /* !_XOPEN_EXTENDED_SOURCE */
#       include /**/ <stropts.h>
#       undef _XOPEN_EXTENDED_SOURCE
#     else
#       include /**/ <stropts.h>
#     endif /* AIX */
#   endif /* ACE_HAS_STREAMS */

#   if defined (DIGITAL_UNIX)
  // sigwait is yet another macro on Digital UNIX 4.0, just causing
  // trouble when introducing member functions with the same name.
  // Thanks to Thilo Kielmann" <kielmann@informatik.uni-siegen.de> for
  // this fix.
#     if defined  (__DECCXX_VER)
#       undef sigwait
        // cxx on Digital Unix 4.0 needs this declaration.  With it,
        // <::_Psigwait> works with cxx -pthread.  g++ does _not_ need
        // it.
        extern "C" int _Psigwait __((const sigset_t *set, int *sig));
#     elif defined (__KCC)
#       undef sigwait
        inline int sigwait (const sigset_t* set, int* sig)
          { return _Psigwait (set, sig); }
#     endif /* __DECCXX_VER */
#   elif !defined (ACE_HAS_SIGWAIT)
#      if defined(__rtems__)
  extern "C" int sigwait (const sigset_t *set, int *sig);
#      else
   extern "C" int sigwait (sigset_t *set);
#      endif /* __rtems__ */
#   endif /* ! DIGITAL_UNIX && ! ACE_HAS_SIGWAIT */

#   if defined (ACE_HAS_SELECT_H)
#     include /**/ <sys/select.h>
#   endif /* ACE_HAS_SELECT_H */

#   if defined (ACE_HAS_ALLOCA_H)
#     include /**/ <alloca.h>
#   endif /* ACE_HAS_ALLOCA_H */

/* Set the proper handle type for dynamically-loaded libraries. */
/* Also define a default 'mode' for loading a library - the names and values */
/* differ between OSes, so if you write code that uses the mode, be careful */
/* of the platform differences. */
#   if defined (ACE_HAS_SVR4_DYNAMIC_LINKING)
#    if defined (ACE_HAS_DLFCN_H_BROKEN_EXTERN_C)
extern "C" {
#     endif /* ACE_HAS_DLFCN_H_BROKEN_EXTERN_C */
#     include /**/ <dlfcn.h>
#     if defined (ACE_HAS_DLFCN_H_BROKEN_EXTERN_C)
}
#     endif /* ACE_HAS_DLFCN_H_BROKEN_EXTERN_C */
  typedef void *ACE_SHLIB_HANDLE;
#   define ACE_SHLIB_INVALID_HANDLE 0
#   if defined (__KCC) && defined(RTLD_GROUP) && defined(RTLD_NODELETE)
#   define ACE_DEFAULT_SHLIB_MODE RTLD_LAZY | RTLD_GROUP | RTLD_NODELETE
#   else
#   define ACE_DEFAULT_SHLIB_MODE RTLD_LAZY
#   endif /* KCC */
#   elif defined (__hpux)
#     if defined(__GNUC__) || __cplusplus >= 199707L
#       include /**/ <dl.h>
#     else
#       include /**/ <cxxdl.h>
#     endif /* (g++ || HP aC++) vs. HP C++ */
  typedef shl_t ACE_SHLIB_HANDLE;
#   define ACE_SHLIB_INVALID_HANDLE 0
#   define ACE_DEFAULT_SHLIB_MODE BIND_DEFERRED
#   else
  typedef void *ACE_SHLIB_HANDLE;
#   define ACE_SHLIB_INVALID_HANDLE 0
#   define ACE_DEFAULT_SHLIB_MODE RTLD_LAZY

#   endif /* ACE_HAS_SVR4_DYNAMIC_LINKING */

#if !defined (RTLD_LAZY)
#define RTLD_LAZY 1
#endif /* !RTLD_LAZY */

#if !defined (RTLD_NOW)
#define RTLD_NOW 2
#endif /* !RTLD_NOW */

#if !defined (RTLD_GLOBAL)
#define RTLD_GLOBAL 3
#endif /* !RTLD_GLOBAL */

#   if defined (ACE_HAS_SOCKIO_H)
#     include /**/ <sys/sockio.h>
#   endif /* ACE_HAS_SOCKIO_ */

// There must be a better way to do this...
#   if !defined (RLIMIT_NOFILE)
#     if defined (linux) || defined (AIX) || defined (SCO)
#       if defined (RLIMIT_OFILE)
#         define RLIMIT_NOFILE RLIMIT_OFILE
#       else
#         define RLIMIT_NOFILE 200
#       endif /* RLIMIT_OFILE */
#     endif /* defined (linux) || defined (AIX) || defined (SCO) */
#   endif /* RLIMIT_NOFILE */

#   if defined (ACE_LACKS_MMAP)
#     define PROT_READ 0
#     define PROT_WRITE 0
#     define PROT_EXEC 0
#     define PROT_NONE 0
#     define PROT_RDWR 0
#     define MAP_PRIVATE 0
#     define MAP_SHARED 0
#     define MAP_FIXED 0
#   endif /* ACE_LACKS_MMAP */

// Fixes a problem with HP/UX.
#   if defined (ACE_HAS_BROKEN_MMAP_H)
extern "C"
{
#     include /**/ <sys/mman.h>
}
#   elif !defined (ACE_LACKS_MMAP)
#     include /**/ <sys/mman.h>
#   endif /* ACE_HAS_BROKEN_MMAP_H */

// OSF1 has problems with sys/msg.h and C++...
#   if defined (ACE_HAS_BROKEN_MSG_H)
#     define _KERNEL
#   endif /* ACE_HAS_BROKEN_MSG_H */
#   if !defined (ACE_LACKS_SYSV_MSG_H)
#     include /**/ <sys/msg.h>
#   endif /* ACE_LACKS_SYSV_MSG_H */
#   if defined (ACE_HAS_BROKEN_MSG_H)
#     undef _KERNEL
#   endif /* ACE_HAS_BROKEN_MSG_H */

#   if defined (ACE_LACKS_SYSV_MSQ_PROTOS)
extern "C"
{
  int msgget (key_t, int);
  int msgrcv (int, void *, size_t, long, int);
  int msgsnd (int, const void *, size_t, int);
  int msgctl (int, int, struct msqid_ds *);
}
#   endif /* ACE_LACKS_SYSV_MSQ_PROTOS */

#   if defined (ACE_HAS_PRIOCNTL)
#     include /**/ <sys/priocntl.h>
#   endif /* ACE_HAS_PRIOCNTL */

#   if defined (ACE_HAS_IDTYPE_T)
  typedef idtype_t ACE_idtype_t;
#   else
  typedef int ACE_idtype_t;
#   endif /* ACE_HAS_IDTYPE_T */

#   if defined (ACE_HAS_STHREADS) || defined (DIGITAL_UNIX)
#     if defined (ACE_LACKS_PRI_T)
    typedef int pri_t;
#     endif /* ACE_LACKS_PRI_T */
  typedef id_t ACE_id_t;
#     define ACE_SELF P_MYID
  typedef pri_t ACE_pri_t;
#   else  /* ! ACE_HAS_STHREADS && ! DIGITAL_UNIX */
  typedef long ACE_id_t;
#     define ACE_SELF (-1)
  typedef short ACE_pri_t;
#   endif /* ! ACE_HAS_STHREADS && ! DIGITAL_UNIX */

#   if defined (ACE_HAS_HI_RES_TIMER) &&  !defined (ACE_LACKS_LONGLONG_T)
  /* hrtime_t is defined on systems (Suns) with ACE_HAS_HI_RES_TIMER */
  typedef hrtime_t ACE_hrtime_t;
#   else  /* ! ACE_HAS_HI_RES_TIMER  ||  ACE_LACKS_LONGLONG_T */
  typedef ACE_UINT64 ACE_hrtime_t;
#   endif /* ! ACE_HAS_HI_RES_TIMER  ||  ACE_LACKS_LONGLONG_T */

# endif /* !defined (ACE_WIN32) && !defined (ACE_PSOS) */

// Define the Wide character and normal versions of some of the string macros
#   if defined (ACE_HAS_WCHAR)
#     define ACE_DIRECTORY_SEPARATOR_STR_W ACE_TEXT_WIDE(ACE_DIRECTORY_SEPARATOR_STR_A)
#     define ACE_DIRECTORY_SEPARATOR_CHAR_W ACE_TEXT_WIDE(ACE_DIRECTORY_SEPARATOR_CHAR_A)
#     define ACE_PLATFORM_W ACE_TEXT_WIDE(ACE_PLATFORM_A)
#     define ACE_PLATFORM_EXE_SUFFIX_W ACE_TEXT_WIDE(ACE_PLATFORM_EXE_SUFFIX_A)
#   endif /* ACE_HAS_WCHAR */

#   define ACE_DIRECTORY_SEPARATOR_STR ACE_LIB_TEXT (ACE_DIRECTORY_SEPARATOR_STR_A)
#   define ACE_DIRECTORY_SEPARATOR_CHAR ACE_LIB_TEXT (ACE_DIRECTORY_SEPARATOR_CHAR_A)
#   define ACE_PLATFORM ACE_LIB_TEXT (ACE_PLATFORM_A)
#   define ACE_PLATFORM_EXE_SUFFIX ACE_LIB_TEXT (ACE_PLATFORM_EXE_SUFFIX_A)

// Theses defines are used by the ACE Name Server.
#   if !defined (ACE_DEFAULT_LOCALNAME_A)
#     define ACE_DEFAULT_LOCALNAME_A "localnames"
#   endif /* ACE_DEFAULT_LOCALNAME_A */
#   if !defined (ACE_DEFAULT_GLOBALNAME_A)
#     define ACE_DEFAULT_GLOBALNAME_A "globalnames"
#   endif /* ACE_DEFAULT_GLOBALNAME_A */

// ACE_DEFAULT_NAMESPACE_DIR is for legacy mode apps.  A better
// way of doing this is something like ACE_Lib_Find::get_temp_dir, since
// this directory may not exist
#   if defined (ACE_LEGACY_MODE)
#     if defined (ACE_WIN32)
#       define ACE_DEFAULT_NAMESPACE_DIR_A "C:\\temp"
#     else /* ACE_WIN32 */
#       define ACE_DEFAULT_NAMESPACE_DIR_A "/tmp"
#     endif /* ACE_WIN32 */
#     if defined (ACE_HAS_WCHAR)
#       define ACE_DEFAULT_NAMESPACE_DIR_W ACE_TEXT_WIDE(ACE_DEFAULT_NAMESPACE_DIR_A)
#     endif /* ACE_HAS_WCHAR */
#     define ACE_DEFAULT_NAMESPACE_DIR ACE_LIB_TEXT(ACE_DEFAULT_NAMESPACE_DIR_A)
#   endif /* ACE_LEGACY_MODE */

#   if defined (ACE_HAS_WCHAR)
#     define ACE_DEFAULT_LOCALNAME_W ACE_TEXT_WIDE(ACE_DEFAULT_LOCALNAME_A)
#     define ACE_DEFAULT_GLOBALNAME_W ACE_TEXT_WIDE(ACE_DEFAULT_GLOBALNAME_A)
#   endif /* ACE_HAS_WCHAR */

#   define ACE_DEFAULT_LOCALNAME ACE_LIB_TEXT (ACE_DEFAULT_LOCALNAME_A)
#   define ACE_DEFAULT_GLOBALNAME ACE_LIB_TEXT (ACE_DEFAULT_GLOBALNAME_A)

// defined Win32 specific macros for UNIX platforms
# if !defined (O_BINARY)
#   define O_BINARY 0
# endif /* O_BINARY */
# if !defined (_O_BINARY)
#   define _O_BINARY O_BINARY
# endif /* _O_BINARY */
# if !defined (O_TEXT)
#   define O_TEXT 0
# endif /* O_TEXT */
# if !defined (_O_TEXT)
#   define _O_TEXT O_TEXT
# endif /* _O_TEXT */
# if !defined (O_RAW)
#   define O_RAW 0
# endif /* O_RAW */
# if !defined (_O_RAW)
#   define _O_RAW O_RAW
# endif /* _O_RAW */

# if !defined (ACE_DEFAULT_SYNCH_TYPE)
#   define ACE_DEFAULT_SYNCH_TYPE USYNC_THREAD
# endif /* ! ACE_DEFAULT_SYNCH_TYPE */

# if !defined (ACE_MAP_PRIVATE)
#   define ACE_MAP_PRIVATE MAP_PRIVATE
# endif /* ! ACE_MAP_PRIVATE */

# if !defined (ACE_MAP_SHARED)
#   define ACE_MAP_SHARED MAP_SHARED
# endif /* ! ACE_MAP_SHARED */

# if !defined (ACE_MAP_FIXED)
#   define ACE_MAP_FIXED MAP_FIXED
# endif /* ! ACE_MAP_FIXED */

#if defined (ACE_LACKS_UTSNAME_T)
#   if !defined (SYS_NMLN)
#     define SYS_NMLN 257
#   endif /* SYS_NMLN */
#   if !defined (_SYS_NMLN)
#     define _SYS_NMLN SYS_NMLN
#   endif /* _SYS_NMLN */
struct ACE_utsname
{
  ACE_TCHAR sysname[_SYS_NMLN];
  ACE_TCHAR nodename[_SYS_NMLN];
  ACE_TCHAR release[_SYS_NMLN];
  ACE_TCHAR version[_SYS_NMLN];
  ACE_TCHAR machine[_SYS_NMLN];
};
# else
#   include /**/ <sys/utsname.h>
typedef struct utsname ACE_utsname;
# endif /* ACE_LACKS_UTSNAME_T */

// Increase the range of "address families".  Please note that this
// must appear _after_ the include of sys/socket.h, for the AF_FILE
// definition on Linux/glibc2.
#if !defined (AF_ANY)
# define AF_ANY (-1)
#endif /* AF_ANY */

# define AF_SPIPE (AF_MAX + 1)
# if !defined (AF_FILE)
#   define AF_FILE (AF_MAX + 2)
# endif /* ! AF_FILE */
# define AF_DEV (AF_MAX + 3)
# define AF_UPIPE (AF_SPIPE)

# if defined (ACE_SELECT_USES_INT)
typedef int ACE_FD_SET_TYPE;
# else
typedef fd_set ACE_FD_SET_TYPE;
# endif /* ACE_SELECT_USES_INT */

# if !defined (MAXNAMELEN)
#   if defined (FILENAME_MAX)
#     define MAXNAMELEN FILENAME_MAX
#   else
#     define MAXNAMELEN 256
#   endif /* FILENAME_MAX */
# endif /* MAXNAMELEN */

# if !defined(MAXHOSTNAMELEN)
#   define MAXHOSTNAMELEN  256
# endif /* MAXHOSTNAMELEN */

// Define INET loopback address constant if it hasn't been defined
// Dotted Decimal 127.0.0.1 == Hexidecimal 0x7f000001
# if !defined (INADDR_LOOPBACK)
#   define INADDR_LOOPBACK ((ACE_UINT32) 0x7f000001)
# endif /* INADDR_LOOPBACK */

// The INADDR_NONE address is generally 255.255.255.255.
# if !defined (INADDR_NONE)
#   define INADDR_NONE ((ACE_UINT32) 0xffffffff)
# endif /* INADDR_NONE */

// Define INET string length constants if they haven't been defined
//
// for IPv4 dotted-decimal
# if !defined (INET_ADDRSTRLEN)
#   define INET_ADDRSTRLEN 16
# endif /* INET_ADDRSTRLEN */
//
// for IPv6 hex string
# if !defined (INET6_ADDRSTRLEN)
#   define INET6_ADDRSTRLEN 46
# endif /* INET6_ADDRSTRLEN */

#if defined (ACE_HAS_IPV6)

#  if defined (ACE_USES_IPV4_IPV6_MIGRATION)
#    define ACE_ADDRESS_FAMILY_INET  AF_UNSPEC
#    define ACE_PROTOCOL_FAMILY_INET PF_UNSPEC
#  else
#    define ACE_ADDRESS_FAMILY_INET AF_INET6
#    define ACE_PROTOCOL_FAMILY_INET PF_INET6
#  endif /* ACE_USES_IPV4_IPV6_MIGRATION */

#else
#  define ACE_ADDRESS_FAMILY_INET AF_INET
#  define ACE_PROTOCOL_FAMILY_INET PF_INET
#endif

# if defined (ACE_LACKS_SIGSET)
#    if !defined(__MINGW32__)
typedef u_int sigset_t;
#    endif /* !__MINGW32__*/
# endif /* ACE_LACKS_SIGSET */

# if defined (ACE_LACKS_SIGACTION)
struct sigaction
{
  int sa_flags;
  ACE_SignalHandlerV sa_handler;
  sigset_t sa_mask;
};
# endif /* ACE_LACKS_SIGACTION */

# if !defined (SIGHUP)
#   define SIGHUP 0
# endif /* SIGHUP */

# if !defined (SIGINT)
#   define SIGINT 0
# endif /* SIGINT */

# if !defined (SIGSEGV)
#   define SIGSEGV 0
# endif /* SIGSEGV */

# if !defined (SIGIO)
#   define SIGIO 0
# endif /* SIGSEGV */

# if !defined (SIGUSR1)
#   define SIGUSR1 0
# endif /* SIGUSR1 */

# if !defined (SIGUSR2)
#   define SIGUSR2 0
# endif /* SIGUSR2 */

# if !defined (SIGCHLD)
#   define SIGCHLD 0
# endif /* SIGCHLD */

# if !defined (SIGCLD)
#   define SIGCLD SIGCHLD
# endif /* SIGCLD */

# if !defined (SIGQUIT)
#   define SIGQUIT 0
# endif /* SIGQUIT */

# if !defined (SIGPIPE)
#   define SIGPIPE 0
# endif /* SIGPIPE */

# if !defined (SIGALRM)
#   define SIGALRM 0
# endif /* SIGALRM */

# if !defined (SIG_DFL)
#   if defined (ACE_PSOS_DIAB_MIPS) || defined (ACE_PSOS_DIAB_PPC)
#     define SIG_DFL ((void *) 0)
#   else
#     define SIG_DFL ((__sighandler_t) 0)
#   endif
# endif /* SIG_DFL */

# if !defined (SIG_IGN)
#   if defined (ACE_PSOS_DIAB_MIPS) || defined (ACE_PSOS_DIAB_PPC)
#     define SIG_IGN ((void *) 1)     /* ignore signal */
#   else
#     define SIG_IGN ((__sighandler_t) 1)     /* ignore signal */
#   endif
# endif /* SIG_IGN */

# if !defined (SIG_ERR)
#   if defined (ACE_PSOS_DIAB_MIPS) || defined (ACE_PSOS_DIAB_PPC)
#     define SIG_ERR ((void *) -1)    /* error return from signal */
#   else
#     define SIG_ERR ((__sighandler_t) -1)    /* error return from signal */
#   endif
# endif /* SIG_ERR */

# if !defined (O_NONBLOCK)
#   define O_NONBLOCK  1
# endif /* O_NONBLOCK  */

# if !defined (SIG_BLOCK)
#   define SIG_BLOCK   1
# endif /* SIG_BLOCK   */

# if !defined (SIG_UNBLOCK)
#   define SIG_UNBLOCK 2
# endif /* SIG_UNBLOCK */

# if !defined (SIG_SETMASK)
#   define SIG_SETMASK 3
# endif /* SIG_SETMASK */

# if !defined (IPC_CREAT)
#   define IPC_CREAT 0
# endif /* IPC_CREAT */

# if !defined (IPC_NOWAIT)
#   define IPC_NOWAIT 0
# endif /* IPC_NOWAIT */

# if !defined (IPC_RMID)
#   define IPC_RMID 0
# endif /* IPC_RMID */

# if !defined (IPC_EXCL)
#   define IPC_EXCL 0
# endif /* IPC_EXCL */

# if !defined (IP_DROP_MEMBERSHIP)
#   define IP_DROP_MEMBERSHIP 0
# endif /* IP_DROP_MEMBERSHIP */

# if !defined (IP_ADD_MEMBERSHIP)
#   define IP_ADD_MEMBERSHIP 0
#   define ACE_LACKS_IP_ADD_MEMBERSHIP
# endif /* IP_ADD_MEMBERSHIP */

# if !defined (IP_DEFAULT_MULTICAST_TTL)
#   define IP_DEFAULT_MULTICAST_TTL 0
# endif /* IP_DEFAULT_MULTICAST_TTL */

# if !defined (IP_DEFAULT_MULTICAST_LOOP)
#   define IP_DEFAULT_MULTICAST_LOOP 0
# endif /* IP_DEFAULT_MULTICAST_LOOP */

# if !defined (IP_MULTICAST_IF)
#   define IP_MULTICAST_IF 0
#endif /* IP_MULTICAST_IF */

# if !defined (IP_MULTICAST_TTL)
#   define IP_MULTICAST_TTL 1
#endif /* IP_MULTICAST_TTL */

# if !defined (IP_MAX_MEMBERSHIPS)
#   define IP_MAX_MEMBERSHIPS 0
# endif /* IP_MAX_MEMBERSHIP */

# if !defined (SIOCGIFBRDADDR)
#   define SIOCGIFBRDADDR 0
# endif /* SIOCGIFBRDADDR */

# if !defined (SIOCGIFADDR)
#   define SIOCGIFADDR 0
# endif /* SIOCGIFADDR */

# if !defined (IPC_PRIVATE)
#   define IPC_PRIVATE ACE_INVALID_SEM_KEY
# endif /* IPC_PRIVATE */

# if !defined (IPC_STAT)
#   define IPC_STAT 0
# endif /* IPC_STAT */

# if !defined (GETVAL)
#   define GETVAL 0
# endif /* GETVAL */

# if !defined (F_GETFL)
#   define F_GETFL 0
# endif /* F_GETFL */

# if !defined (SETVAL)
#   define SETVAL 0
# endif /* SETVAL */

# if !defined (GETALL)
#   define GETALL 0
# endif /* GETALL */

# if !defined (SETALL)
#   define SETALL 0
# endif /* SETALL */

# if !defined (SEM_UNDO)
#   define SEM_UNDO 0
# endif /* SEM_UNDO */

# if defined (__Lynx__)
    // LynxOS Neutrino sets NSIG to the highest-numbered signal.
#   define ACE_NSIG (NSIG + 1)
# elif defined (__rtems__)
#   define ACE_NSIG (SIGRTMAX)
# else
  // All other platforms set NSIG to one greater than the
  // highest-numbered signal.
#   define ACE_NSIG NSIG
# endif /* __Lynx__ */

# if !defined (R_OK)
#   define R_OK    04      /* Test for Read permission. */
# endif /* R_OK */

# if !defined (W_OK)
#   define W_OK    02      /* Test for Write permission. */
# endif /* W_OK */

# if !defined (X_OK)
#   define X_OK    01      /* Test for eXecute permission. */
# endif /* X_OK */

# if !defined (F_OK)
#   define F_OK    0       /* Test for existence of File. */
# endif /* F_OK */

# if !defined (ESUCCESS)
#   define ESUCCESS 0
# endif /* !ESUCCESS */

# if !defined (EIDRM)
#   define EIDRM 0
# endif /* !EIDRM */

# if !defined (ENFILE)
#   define ENFILE EMFILE /* No more socket descriptors are available. */
# endif /* !ENOSYS */

# if !defined (ECOMM)
    // Not the same, but ECONNABORTED is provided on NT.
#   define ECOMM ECONNABORTED
# endif /* ECOMM */

# if !defined (WNOHANG)
#   define WNOHANG 0100
# endif /* !WNOHANG */

# if !defined (EDEADLK)
#   define EDEADLK 1000 /* Some large number.... */
# endif /* !EDEADLK */

# if !defined (MS_SYNC)
#   define MS_SYNC 0x0
# endif /* !MS_SYNC */

# if !defined (PIPE_BUF)
#   define PIPE_BUF 5120
# endif /* PIPE_BUF */

# if !defined (PROT_RDWR)
#   define PROT_RDWR (PROT_READ|PROT_WRITE)
# endif /* PROT_RDWR */

# if defined (ACE_HAS_POSIX_NONBLOCK)
#   define ACE_NONBLOCK O_NONBLOCK
# else
#   define ACE_NONBLOCK O_NDELAY
# endif /* ACE_HAS_POSIX_NONBLOCK */

// These are used by the <ACE_IPC_SAP::enable> and
// <ACE_IPC_SAP::disable> methods.  They must be unique and cannot
// conflict with the value of <ACE_NONBLOCK>.  We make the numbers
// negative here so they won't conflict with other values like SIGIO,
// etc.
# define ACE_SIGIO -1
# define ACE_SIGURG -2
# define ACE_CLOEXEC -3

# define LOCALNAME 0
# define REMOTENAME 1

# if !defined (ETIMEDOUT) && defined (ETIME)
#   define ETIMEDOUT ETIME
# endif /* ETIMEDOUT */

# if !defined (ETIME) && defined (ETIMEDOUT)
#   define ETIME ETIMEDOUT
# endif /* ETIMED */

# if !defined (EBUSY)
#   define EBUSY ETIME
# endif /* EBUSY */

# if !defined (_SC_TIMER_MAX)
#   define _SC_TIMER_MAX 44
# endif /* _SC_TIMER_MAX */

// Default number of <ACE_Event_Handler>s supported by
// <ACE_Timer_Heap>.
# if !defined (ACE_DEFAULT_TIMERS)
#   define ACE_DEFAULT_TIMERS _SC_TIMER_MAX
# endif /* ACE_DEFAULT_TIMERS */

# if defined (ACE_HAS_STRUCT_NETDB_DATA)
typedef char ACE_HOSTENT_DATA[sizeof(struct hostent_data)];
typedef char ACE_SERVENT_DATA[sizeof(struct servent_data)];
typedef char ACE_PROTOENT_DATA[sizeof(struct protoent_data)];
# else
#   if !defined ACE_HOSTENT_DATA_SIZE
#     define ACE_HOSTENT_DATA_SIZE (4*1024)
#   endif /*ACE_HOSTENT_DATA_SIZE */
#   if !defined ACE_SERVENT_DATA_SIZE
#     define ACE_SERVENT_DATA_SIZE (4*1024)
#   endif /*ACE_SERVENT_DATA_SIZE */
#   if !defined ACE_PROTOENT_DATA_SIZE
#     define ACE_PROTOENT_DATA_SIZE (2*1024)
#   endif /*ACE_PROTOENT_DATA_SIZE */
typedef char ACE_HOSTENT_DATA[ACE_HOSTENT_DATA_SIZE];
typedef char ACE_SERVENT_DATA[ACE_SERVENT_DATA_SIZE];
typedef char ACE_PROTOENT_DATA[ACE_PROTOENT_DATA_SIZE];
# endif /* ACE_HAS_STRUCT_NETDB_DATA */

# if !defined (ACE_HAS_SEMUN) || (defined (__GLIBC__) && defined (_SEM_SEMUN_UNDEFINED))
union semun
{
  /// value for SETVAL
  int val;
  /// buffer for IPC_STAT & IPC_SET
  struct semid_ds *buf;
  /// array for GETALL & SETALL
  u_short *array;
};
# endif /* !ACE_HAS_SEMUN || (defined (__GLIBC__) && defined (_SEM_SEMUN_UNDEFINED)) */


// Create some useful typedefs.

typedef const char **SYS_SIGLIST;

# if !defined (MAP_FAILED) || defined (ACE_HAS_BROKEN_MAP_FAILED)
#   undef MAP_FAILED
#   define MAP_FAILED ((void *) -1)
# elif defined (ACE_HAS_LONG_MAP_FAILED)
#   undef MAP_FAILED
#   define MAP_FAILED ((void *) -1L)
# endif /* !MAP_FAILED || ACE_HAS_BROKEN_MAP_FAILED */

# if defined (ACE_HAS_CHARPTR_DL)
typedef ACE_TCHAR * ACE_DL_TYPE;
# else
typedef const ACE_TCHAR * ACE_DL_TYPE;
# endif /* ACE_HAS_CHARPTR_DL */

# if !defined (ACE_HAS_SIGINFO_T)
struct ACE_OS_Export siginfo_t
{
  siginfo_t (ACE_HANDLE handle);
  siginfo_t (ACE_HANDLE *handles);      // JCEJ 12/23/96

  /// Win32 HANDLE that has become signaled.
  ACE_HANDLE si_handle_;

  /// Array of Win32 HANDLEs all of which have become signaled.
  ACE_HANDLE *si_handles_;
};
# endif /* ACE_HAS_SIGINFO_T */

// Typedef for the null handler func.
extern "C"
{
  typedef void (*ACE_SIGNAL_C_FUNC)(int,siginfo_t*,void*);
}

# if !defined (ACE_HAS_UCONTEXT_T)
typedef int ucontext_t;
# endif /* ACE_HAS_UCONTEXT_T */

# if !defined (SA_SIGINFO)
#   define SA_SIGINFO 0
# endif /* SA_SIGINFO */

# if !defined (SA_RESTART)
#   define SA_RESTART 0
# endif /* SA_RESTART */

# if defined (ACE_HAS_TIMOD_H)
#   if defined (ACE_HAS_STL_QUEUE_CONFLICT)
#     define queue _Queue_
#   endif /* ACE_HAS_STL_QUEUE_CONFLICT */
#   include /**/ <sys/timod.h>
#   if defined (ACE_HAS_STL_QUEUE_CONFLICT)
#     undef queue
#   endif /* ACE_HAS_STL_QUEUE_CONFLICT */
# elif defined (ACE_HAS_OSF_TIMOD_H)
#   include /**/ <tli/timod.h>
# endif /* ACE_HAS_TIMOD_H */

/**
 * @class ACE_Thread_ID
 *
 * @brief Defines a platform-independent thread ID.
 */
class ACE_OS_Export ACE_Thread_ID
{
public:
  // = Initialization method.
  ACE_Thread_ID (ACE_thread_t, ACE_hthread_t);
  ACE_Thread_ID (const ACE_Thread_ID &id);

  // = Set/Get the Thread ID.
  ACE_thread_t id (void);
  void id (ACE_thread_t);

  // = Set/Get the Thread handle.
  ACE_hthread_t handle (void);
  void handle (ACE_hthread_t);

  // != Comparison operator.
  int operator== (const ACE_Thread_ID &) const;
  int operator!= (const ACE_Thread_ID &) const;

private:
  /// Identify the thread.
  ACE_thread_t thread_id_;

  /// Handle to the thread (typically used to "wait" on Win32).
  ACE_hthread_t thread_handle_;
};

// Type of the extended signal handler.
typedef void (*ACE_Sig_Handler_Ex) (int, siginfo_t *siginfo, ucontext_t *ucontext);

// = The ACE_Sched_Priority type should be used for platform-
//   independent thread and process priorities, by convention.
//   int should be used for OS-specific priorities.
typedef int ACE_Sched_Priority;

// forward declaration
class ACE_Sched_Params;

# if defined (ACE_LACKS_FILELOCKS)
#   if ! defined (VXWORKS) && ! defined (ACE_PSOS) && ! defined (__rtems__)
// VxWorks defines struct flock in sys/fcntlcom.h.  But it doesn't
// appear to support flock ().  RTEMS defines struct flock but
// currently does not support locking.
struct flock
{
  short l_type;
  short l_whence;
  off_t l_start;
  off_t l_len;          /* len == 0 means until end of file */
  long  l_sysid;
  pid_t l_pid;
  long  l_pad[4];               /* reserve area */
};
#   endif /* ! VXWORKS */
# endif /* ACE_LACKS_FILELOCKS */

# if !defined (ACE_HAS_IP_MULTICAST)  &&  defined (ACE_LACKS_IP_ADD_MEMBERSHIP)
  // Even if ACE_HAS_IP_MULTICAST is not defined, if IP_ADD_MEMBERSHIP
  // is defined, assume that the ip_mreq struct is also defined
  // (presumably in netinet/in.h).
  struct ip_mreq
  {
    /// IP multicast address of group
    struct in_addr imr_multiaddr;
    /// Local IP address of interface
    struct in_addr imr_interface;
  };
# endif /* ! ACE_HAS_IP_MULTICAST  &&  ACE_LACKS_IP_ADD_MEMBERSHIP */

# if !defined (ACE_HAS_STRBUF_T)
struct strbuf
{
  /// No. of bytes in buffer.
  int maxlen;
  /// No. of bytes returned.
  int len;
  /// Pointer to data.
  void *buf;
};
# endif /* ACE_HAS_STRBUF_T */

/**
 * @class ACE_Str_Buf
 *
 * @brief Simple wrapper for STREAM pipes strbuf.
 */
class ACE_OS_Export ACE_Str_Buf : public strbuf
{
public:
  // = Initialization method
  /// Constructor.
  ACE_Str_Buf (void *b = 0, int l = 0, int max = 0);

  /// Constructor.
  ACE_Str_Buf (strbuf &);
};

# if defined (ACE_HAS_BROKEN_BITSHIFT)
  // This might not be necessary any more:  it was added prior to the
  // (fd_mask) cast being added to the version below.  Maybe that cast
  // will fix the problem on tandems.    Fri Dec 12 1997 David L. Levine
#   define ACE_MSB_MASK (~(ACE_UINT32 (1) << ACE_UINT32 (NFDBITS - 1)))
# else
  // This needs to go here to avoid overflow problems on some compilers.
#   if defined (ACE_WIN32)
    //  Does ACE_WIN32 have an fd_mask?
#     define ACE_MSB_MASK (~(1 << (NFDBITS - 1)))
#   else  /* ! ACE_WIN32 */
#     define ACE_MSB_MASK (~((fd_mask) 1 << (NFDBITS - 1)))
#   endif /* ! ACE_WIN32 */
# endif /* ACE_HAS_BROKEN_BITSHIFT */

// Signature for registering a cleanup function that is used by the
// <ACE_Object_Manager> and the <ACE_Thread_Manager>.
# if defined (ACE_HAS_SIG_C_FUNC)
extern "C" {
# endif /* ACE_HAS_SIG_C_FUNC */
typedef void (*ACE_CLEANUP_FUNC)(void *object, void *param) /* throw () */;
# if defined (ACE_HAS_SIG_C_FUNC)
}
# endif /* ACE_HAS_SIG_C_FUNC */

# if defined (ACE_WIN32)
// Default WIN32 structured exception handler.
int ACE_SEH_Default_Exception_Selector (void *);
int ACE_SEH_Default_Exception_Handler (void *);
# endif /* ACE_WIN32 */

/**
 * @class ACE_Cleanup
 *
 * @brief Base class for objects that are cleaned by ACE_Object_Manager.
 */
class ACE_OS_Export ACE_Cleanup
{
public:
  /// No-op constructor.
  ACE_Cleanup (void);

  /// Destructor.
  virtual ~ACE_Cleanup (void);

  /// Cleanup method that, by default, simply deletes itself.
  virtual void cleanup (void *param = 0);
};

// Adapter for cleanup, used by ACE_Object_Manager.
extern "C" ACE_OS_Export
void ace_cleanup_destroyer (ACE_Cleanup *, void *param = 0);

/**
 * @class ACE_Cleanup_Info
 *
 * @brief Hold cleanup information for thread/process
 */
class ACE_OS_Export ACE_Cleanup_Info
{
public:
  /// Default constructor.
  ACE_Cleanup_Info (void);

  /// Equality operator.
  int operator== (const ACE_Cleanup_Info &o) const;

  /// Inequality operator.
  int operator!= (const ACE_Cleanup_Info &o) const;

  /// Point to object that gets passed into the <cleanup_hook_>.
  void *object_;

  /// Cleanup hook that gets called back.
  ACE_CLEANUP_FUNC cleanup_hook_;

  /// Parameter passed to the <cleanup_hook_>.
  void *param_;
};

class ACE_Cleanup_Info_Node;

/**
 * @class ACE_OS_Exit_Info
 *
 * @brief Hold Object Manager cleanup (exit) information.
 *
 * For internal use by the ACE library, only.
 */
class ACE_OS_Export ACE_OS_Exit_Info
{
public:
  /// Default constructor.
  ACE_OS_Exit_Info (void);

  /// Destructor.
  ~ACE_OS_Exit_Info (void);

  /// Use to register a cleanup hook.
  int at_exit_i (void *object, ACE_CLEANUP_FUNC cleanup_hook, void *param);

  /// Look for a registered cleanup hook object.  Returns 1 if already
  /// registered, 0 if not.
  int find (void *object);

  /// Call all registered cleanup hooks, in reverse order of
  /// registration.
  void call_hooks ();

private:
  /**
   * Keeps track of all registered objects.  The last node is only
   * used to terminate the list (it doesn't contain a valid
   * ACE_Cleanup_Info).
   */
  ACE_Cleanup_Info_Node *registered_objects_;
};

class ACE_Base_Thread_Adapter;
class ACE_Thread_Hook;

# if defined (ACE_HAS_PHARLAP_RT)
#define ACE_IPPROTO_TCP SOL_SOCKET
# else
#define ACE_IPPROTO_TCP IPPROTO_TCP
# endif /* ACE_HAS_PHARLAP_RT */

# if defined (ACE_LACKS_FLOATING_POINT)
typedef ACE_UINT32 ACE_timer_t;
# else
typedef double ACE_timer_t;
# endif /* ACE_LACKS_FLOATING_POINT */

# if defined (ACE_HAS_PRUSAGE_T)
    typedef prusage_t ACE_Rusage;
# elif defined (ACE_HAS_GETRUSAGE)
    typedef rusage ACE_Rusage;
# else
    typedef int ACE_Rusage;
# endif /* ACE_HAS_PRUSAGE_T */

# if !defined (ACE_WIN32) && !defined (ACE_LACKS_UNIX_SYSLOG)
# include /**/ <syslog.h>
# endif /* !defined (ACE_WIN32) && !defined (ACE_LACKS_UNIX_SYSLOG) */

# if defined (ACE_HAS_SYS_FILIO_H)
# include /**/ <sys/filio.h>
# endif /* ACE_HAS_SYS_FILIO_H */

# if defined (ACE_WIN32) && !defined (ACE_HAS_WINCE) && !defined (__BORLANDC__)
      typedef struct _stat ACE_stat;
# else
      typedef struct stat ACE_stat;
# endif /* ACE_WIN32 */

// We need this for MVS...
extern "C" {
  typedef int (*ACE_COMPARE_FUNC)(const void *, const void *);
}



/// Helper for the ACE_OS::timezone() function
/**
 * We put all the timezone stuff that used to be in ACE_OS::timezone()
 * here because on some platforms "timezone" is a macro.  Because of this,
 * the name ACE_OS::timezone will cause errors.  So in order to use the
 * macro as it is defined but also keep the name ACE_OS::timezone, we
 * use timezone first here in this inline function, and then undefine
 * timezone.
 */
inline long ace_timezone()
{
#if !defined (ACE_HAS_WINCE) && !defined (VXWORKS) && !defined (ACE_PSOS) \
    && !defined (CHORUS)
# if defined (ACE_WIN32)
  return _timezone;  // For Win32.
# elif defined (__Lynx__) || defined (__FreeBSD__) || defined (ACE_HAS_SUNOS4_GETTIMEOFDAY)
  long result = 0;
  struct timeval time;
  struct timezone zone;
  ACE_UNUSED_ARG (result);
  ACE_OSCALL (::gettimeofday (&time, &zone), int, -1, result);
  return zone.tz_minuteswest * 60;
# else  /* __Lynx__ || __FreeBSD__ ... */
  return timezone;
# endif /* __Lynx__ || __FreeBSD__ ... */
#else
  ACE_NOTSUP_RETURN (0);
#endif /* !ACE_HAS_WINCE && !VXWORKS && !ACE_PSOS */
}


#if !defined (ACE_LACKS_DIFFTIME)
/// Helper for the ACE_OS::difftime() function
/**
 * We moved the difftime code that used to be in ACE_OS::difftime()
 * here because on some platforms "difftime" is a macro.  Because of this,
 * the name ACE_OS::difftime will cause errors.  So in order to use the
 * macro as it is defined but also keep the name ACE_OS::difftime, we
 * use difftime first here in this inline function, and then undefine
 * it.
 */
inline double ace_difftime(time_t t1, time_t t0)
{
# if defined (ACE_PSOS) && ! defined (ACE_PSOS_HAS_TIME)
  // simulate difftime ; just subtracting ; ACE_PSOS case
  return ((double)t1) - ((double)t0);
# else
  return difftime (t1, t0);
# endif /* ACE_PSOS !ACE_PSOS_HAS_TIME */
}
#endif /* !ACE_LACKS_DIFFTIME */


/// Helper for the ACE_OS::cuserid() function
/**
 * On some platforms cuserid is a macro.  Defining ACE_OS::cuserid()
 * becomes really hard, as there is no way to save the macro
 * definition using the pre-processor.
 * This inline function achieves the same effect, without namespace
 * pollution or performance penalties.
 *
 * @todo We maybe should move a lot of the code in ACE_OS::cuserid here so
 *       it is treated the same as the above ace_difftime and ace_timezone.
 *       But since there is a good deal more code in ACE_OS::cuserid, we
 *       probably need to move some of it off into some sort of emulation
 *       function.
 */
#if !defined (ACE_LACKS_CUSERID) && !defined(ACE_HAS_ALT_CUSERID) \
    && !defined(ACE_WIN32) && !defined (VXWORKS)
inline char *ace_cuserid(char *user)
{
  return cuserid(user);
}
#endif /* !ACE_LACKS_CUSERID && !ACE_HAS_ALT_CUSERID && ... */

#if defined (SD_RECEIVE)
#define ACE_SHUTDOWN_READ SD_RECEIVE
#elif defined (SHUT_RD)
#define ACE_SHUTDOWN_READ SHUT_RD
#else
#define ACE_SHUTDOWN_READ 0
#endif /* SD_RECEIVE */

#if defined (SD_SEND)
#define ACE_SHUTDOWN_WRITE SD_SEND
#elif defined (SHUT_WR)
#define ACE_SHUTDOWN_WRITE SHUT_WR
#else
#define ACE_SHUTDOWN_WRITE 1
#endif /* SD_RECEIVE */

#if defined (SD_BOTH)
#define ACE_SHUTDOWN_BOTH SD_BOTH
#elif defined (SHUT_RDWR)
#define ACE_SHUTDOWN_BOTH SHUT_RDWR
#else
#define ACE_SHUTDOWN_BOTH 2
#endif /* SD_RECEIVE */

#if !defined (ACE_HAS_WINCE)
// forward declarations of QoS data structures
class ACE_QoS;
class ACE_QoS_Params;
class ACE_Accept_QoS_Params;
#endif  // ACE_HAS_WINCE

#if defined (ACE_HAS_WINSOCK2) && (ACE_HAS_WINSOCK2 != 0)
typedef WSAPROTOCOL_INFO ACE_Protocol_Info;

// Callback function that's used by the QoS-enabled <ACE_OS::ioctl>
// method.
typedef LPWSAOVERLAPPED_COMPLETION_ROUTINE ACE_OVERLAPPED_COMPLETION_FUNC;
typedef GROUP ACE_SOCK_GROUP;
#else  /*  (ACE_HAS_WINSOCK2) && (ACE_HAS_WINSOCK2 != 0) */
struct ACE_Protocol_Info
{
  u_long dwServiceFlags1;
  int iAddressFamily;
  int iProtocol;
  char szProtocol[255+1];
};

// Callback function that's used by the QoS-enabled <ACE_OS::ioctl>
// method.
typedef void (*ACE_OVERLAPPED_COMPLETION_FUNC) (u_long error,
                                                u_long bytes_transferred,
                                                ACE_OVERLAPPED *overlapped,
                                                u_long flags);
typedef u_long ACE_SOCK_GROUP;

#endif /* (ACE_HAS_WINSOCK2) && (ACE_HAS_WINSOCK2 != 0) */

/**
 * @class ACE_OS
 *
 * @brief This class defines an OS independent programming API that
 *     shields developers from nonportable aspects of writing
 *     efficient system programs on Win32, POSIX and other versions
 *     of UNIX, and various real-time operating systems.
 *
 * This class encapsulates the differences between various OS
 * platforms.  When porting ACE to a new platform, this class is
 * the place to focus on.  Once this file is ported to a new
 * platform, pretty much everything else comes for "free."  See
 * <www.cs.wustl.edu/~schmidt/ACE_wrappers/etc/ACE-porting.html>
 * for instructions on porting ACE.  Please see the README file
 * in this directory for complete information on the meaning of
 * the various macros.
 */
class ACE_OS_Export ACE_OS
  : public ACE_OS_Dirent,
    public ACE_OS_String,
    public ACE_OS_Memory,
    public ACE_OS_TLI
{

  ACE_CLASS_IS_NAMESPACE (ACE_OS);
public:
  friend class ACE_Timeout_Manager;

# if defined (CHORUS) && !defined (CHORUS_4)
  // We must format this code as follows to avoid confusing OSE.
  enum ACE_HRTimer_Op
    {
      ACE_HRTIMER_START = K_BSTART,
      ACE_HRTIMER_INCR = K_BPOINT,
      ACE_HRTIMER_STOP = K_BSTOP,
      ACE_HRTIMER_GETTIME = 0xFFFF
    };
# else  /* ! CHORUS */
  enum ACE_HRTimer_Op
    {
      ACE_HRTIMER_START = 0x0,  // Only use these if you can stand
      ACE_HRTIMER_INCR = 0x1,   // for interrupts to be disabled during
      ACE_HRTIMER_STOP = 0x2,   // the timed interval!!!!
      ACE_HRTIMER_GETTIME = 0xFFFF
    };
# endif /* ! CHORUS */

  /**
   * @class ace_flock_t
   *
   * @brief OS file locking structure.
   */
  class ACE_OS_Export ace_flock_t
  {
  public:
  /// Dump state of the object.
    void dump (void) const;

# if defined (ACE_WIN32)
    ACE_OVERLAPPED overlapped_;
# else
    struct flock lock_;
# endif /* ACE_WIN32 */

    /// Name of this filelock.
    const ACE_TCHAR *lockname_;

    /// Handle to the underlying file.
    ACE_HANDLE handle_;

# if defined (CHORUS)
    /// This is the mutex that's stored in shared memory.  It can only
    /// be destroyed by the actor that initialized it.
    ACE_mutex_t *process_lock_;
# endif /* CHORUS */
  };

# if defined (ACE_WIN32)
  // = Default Win32 Security Attributes definition.
  static LPSECURITY_ATTRIBUTES default_win32_security_attributes (LPSECURITY_ATTRIBUTES);

  // = Win32 OS version determination function.
  /// Return the win32 OSVERSIONINFO structure.
  static const OSVERSIONINFO &get_win32_versioninfo (void);

  // = A pair of functions for modifying ACE's Win32 resource usage.
  /// Return the handle of the module containing ACE's resources. By
  /// default, for a DLL build of ACE this is a handle to the ACE DLL
  /// itself, and for a static build it is a handle to the executable.
  static HINSTANCE get_win32_resource_module (void);

  /// Allow an application to modify which module contains ACE's
  /// resources. This is mainly useful for a static build of ACE where
  /// the required resources reside somewhere other than the executable.
  static void set_win32_resource_module (HINSTANCE);

# endif /* ACE_WIN32 */

  // = A set of wrappers for miscellaneous operations.
  static int atoi (const char *s);

# if defined (ACE_HAS_WCHAR)
  static int atoi (const wchar_t *s);
# endif /* ACE_HAS_WCHAR */

  static void *atop (const char *s);

# if defined (ACE_HAS_WCHAR)
  static void *atop (const wchar_t *s);
# endif /* ACE_HAS_WCHAR */

  /// This method computes the largest integral value not greater than x.
  static double floor (double x);

  /// This method computes the smallest integral value not less than x.
  static double ceil (double x);

  static char *getenv (const char *symbol);
#   if defined (ACE_HAS_WCHAR) && defined (ACE_WIN32)
  static wchar_t *getenv (const wchar_t *symbol);
#   endif /* ACE_HAS_WCHAR && ACE_WIN32 */
  static int putenv (const ACE_TCHAR *string);
  static ACE_TCHAR *strenvdup (const ACE_TCHAR *str);
  static ACE_TCHAR *getenvstrings (void);

  static int getopt (int argc,
                     char *const *argv,
                     const char *optstring);
  static int argv_to_string (ACE_TCHAR **argv,
                             ACE_TCHAR *&buf,
                             int substitute_env_args = 1);
  static int string_to_argv (ACE_TCHAR *buf,
                             size_t &argc,
                             ACE_TCHAR **&argv,
                             int substitute_env_args = 1);
  static long sysconf (int);

  //@{ @name A set of wrappers for condition variables.
  static int condattr_init (ACE_condattr_t &attributes,
                            int type = ACE_DEFAULT_SYNCH_TYPE);
  static int condattr_destroy (ACE_condattr_t &attributes);
  static int cond_broadcast (ACE_cond_t *cv);
  static int cond_destroy (ACE_cond_t *cv);
  static int cond_init (ACE_cond_t *cv,
                        short type = ACE_DEFAULT_SYNCH_TYPE,
                        const char *name = 0,
                        void *arg = 0);
  static int cond_init (ACE_cond_t *cv,
                        ACE_condattr_t &attributes,
                        const char *name = 0,
                        void *arg = 0);
# if defined (ACE_HAS_WCHAR)
  static int cond_init (ACE_cond_t *cv,
                        short type,
                        const wchar_t *name,
                        void *arg = 0);
  static int cond_init (ACE_cond_t *cv,
                        ACE_condattr_t &attributes,
                        const wchar_t *name,
                        void *arg = 0);
# endif /* ACE_HAS_WCHAR */
  static int cond_signal (ACE_cond_t *cv);
  static int cond_timedwait (ACE_cond_t *cv,
                             ACE_mutex_t *m,
                             ACE_Time_Value *);
  static int cond_wait (ACE_cond_t *cv,
                        ACE_mutex_t *m);
# if defined (ACE_WIN32) && defined (ACE_HAS_WTHREADS)
  static int cond_timedwait (ACE_cond_t *cv,
                             ACE_thread_mutex_t *m,
                             ACE_Time_Value *);
  static int cond_wait (ACE_cond_t *cv,
                        ACE_thread_mutex_t *m);
# endif /* ACE_WIN32 && ACE_HAS_WTHREADS */
  //@}


  //@{ @name Wrappers to obtain the current user id
# if !defined (ACE_LACKS_CUSERID)
#if defined(cuserid)
# undef cuserid
#endif /* cuserid */
  static char *cuserid (char *user,
                        size_t maxlen = ACE_MAX_USERID);

#   if defined (ACE_HAS_WCHAR)
  static wchar_t *cuserid (wchar_t *user,
                           size_t maxlen = ACE_MAX_USERID);
#   endif /* ACE_HAS_WCHAR */
# endif /* ACE_LACKS_CUSERID */
  //@}

  //@{ @name Wrappers to obtain configuration info
  static int uname (ACE_utsname *name);
  static long sysinfo (int cmd,
                       char *buf,
                       long count);
  static int hostname (char *name,
                       size_t maxnamelen);

#if defined (ACE_HAS_WCHAR)
  static int hostname (wchar_t *name,
                       size_t maxnamelen);
#endif /* ACE_HAS_WCHAR */
  //@}

  //@{ @name A set of wrappers for explicit dynamic linking.
  static int dlclose (ACE_SHLIB_HANDLE handle);

  static ACE_TCHAR *dlerror (void);
  static ACE_SHLIB_HANDLE dlopen (const ACE_TCHAR *filename,
                                  int mode = ACE_DEFAULT_SHLIB_MODE);
  static void *dlsym (ACE_SHLIB_HANDLE handle,
                      const ACE_TCHAR *symbol);
  //@}

  //{@ @name A set of wrappers for stdio file operations.
  static int last_error (void);
  static void last_error (int);
  static int set_errno_to_last_error (void);
  static int set_errno_to_wsa_last_error (void);
  static int fclose (FILE *fp);
  static int fcntl (ACE_HANDLE handle,
                    int cmd,
                    long arg = 0);
  static int fdetach (const char *file);

  static int fsync (ACE_HANDLE handle);

# if defined (ACE_USES_WCHAR)
  // If fp points to the Unicode format file, the file pointer will be moved right next
  // to the Unicode header (2 types).  Otherwise, file pointer will be at the beginning.
  static void checkUnicodeFormat (FILE* fp);
# endif  // ACE_USES_WCHAR

  static FILE *fopen (const ACE_TCHAR *filename, const ACE_TCHAR *mode);
  static FILE *freopen (const ACE_TCHAR *filename, const ACE_TCHAR *mode, FILE* stream);
# if defined (fdopen)
#   undef fdopen
# endif /* fdopen */
  static FILE *fdopen (ACE_HANDLE handle, const ACE_TCHAR *mode);
  static ACE_TCHAR *fgets (ACE_TCHAR *buf, int size, FILE *fp);
  static int stat (const ACE_TCHAR *file, ACE_stat *);
  static int truncate (const ACE_TCHAR *filename, off_t length);

  static int fprintf (FILE *fp, const char *format, ...);
  static int sprintf (char *buf, const char *format, ...);
  static int vsprintf (char *buffer, const char *format, va_list argptr);
  static int printf (const char *format, ...);
# if defined (ACE_HAS_WCHAR)
  static int sprintf (wchar_t *buf, const wchar_t *format, ...);
  static int fprintf (FILE *fp, const wchar_t *format, ...);
  static int vsprintf (wchar_t *buffer, const wchar_t *format, va_list argptr);
# endif /* ACE_HAS_WCHAR */

  static void perror (const ACE_TCHAR *s);

  // The old gets () which directly maps to the evil, unprotected
  // gets () has been deprecated.  If you really need gets (),
  // consider the following one.

  // A better gets ().
  //   If n == 0, input is swallowed, but NULL is returned.
  //   Otherwise, reads up to n-1 bytes (not including the newline),
  //              then swallows rest up to newline
  //              then swallows newline
  static char *gets (char *str, int n = 0);
  static int puts (const ACE_TCHAR *s);
  static int fputs (const ACE_TCHAR *s,
                    FILE *stream);

  static int fflush (FILE *fp);
  static size_t fread (void *ptr,
                       size_t size,
                       size_t nelems,
                       FILE *fp);
  static int fseek (FILE *fp,
                    long offset,
                    int ptrname);
  static long ftell (FILE* fp);
  static int  fgetpos (FILE* fp, fpos_t* pos);
  static int  fsetpos (FILE* fp, fpos_t* pos);
  static int fstat (ACE_HANDLE,
                    ACE_stat *);
  static int lstat (const char *,
                    ACE_stat *);
  static int ftruncate (ACE_HANDLE,
                        off_t);
  static size_t fwrite (const void *ptr,
                        size_t size,
                        size_t nitems,
                        FILE *fp);
  static void rewind (FILE *fp);
  //@}

  //@{ @name Wrappers for searching and sorting.
  static void *bsearch (const void *key,
                        const void *base,
                        size_t nel,
                        size_t size,
                        ACE_COMPARE_FUNC);
  static void qsort (void *base,
                     size_t nel,
                     size_t width,
                     ACE_COMPARE_FUNC);
  //@}

  //@{ @name A set of wrappers for file locks.
  static int flock_init (ACE_OS::ace_flock_t *lock,
                         int flags = 0,
                         const ACE_TCHAR *name = 0,
                         mode_t perms = 0);
  static int flock_destroy (ACE_OS::ace_flock_t *lock,
                            int unlink_file = 1);
# if defined (ACE_WIN32)
  static void adjust_flock_params (ACE_OS::ace_flock_t *lock,
                                   short whence,
                                   off_t &start,
                                   off_t &len);
# endif /* ACE_WIN32 */
  static int flock_rdlock (ACE_OS::ace_flock_t *lock,
                           short whence = 0,
                           off_t start = 0,
                           off_t len = 0);
  static int flock_tryrdlock (ACE_OS::ace_flock_t *lock,
                              short whence = 0,
                              off_t start = 0,
                              off_t len = 0);
  static int flock_trywrlock (ACE_OS::ace_flock_t *lock,
                              short whence = 0,
                              off_t start = 0,
                              off_t len = 0);
  static int flock_unlock (ACE_OS::ace_flock_t *lock,
                           short whence = 0,
                           off_t start = 0,
                           off_t len = 0);
  static int flock_wrlock (ACE_OS::ace_flock_t *lock,
                           short whence = 0,
                           off_t start = 0,
                           off_t len = 0);
  //@}

  //@{ @name A set of wrappers for low-level process operations.
  static int atexit (ACE_EXIT_HOOK func);
  static int execl (const char *path,
                    const char *arg0, ...);
  static int execle (const char *path,
                     const char *arg0, ...);
  static int execlp (const char *file,
                     const char *arg0, ...);
  static int execv (const char *path,
                    char *const argv[]);
  static int execvp (const char *file,
                     char *const argv[]);
  static int execve (const char *path,
                     char *const argv[],
                     char *const envp[]);
  static void _exit (int status = 0);
  static void exit (int status = 0);
  static void abort (void);
  static pid_t fork (void);

  //@{
  /// Forks and exec's a process in a manner that works on Solaris and
  /// NT.  argv[0] must be the full path name to the executable.
  static pid_t fork (const ACE_TCHAR *program_name);
  static pid_t fork_exec (ACE_TCHAR *argv[]);
  //@}

  static int getpagesize (void);
  static int allocation_granularity (void);

  static gid_t getgid (void);
  static int setgid (gid_t);
  static pid_t getpid (void);
  static pid_t getpgid (pid_t pid);
  static pid_t getppid (void);
  static uid_t getuid (void);
  static int setuid (uid_t);
  static pid_t setsid (void);
  static int setpgid (pid_t pid, pid_t pgid);
  static int setreuid (uid_t ruid, uid_t euid);
  static int setregid (gid_t rgid, gid_t egid);
  static int system (const ACE_TCHAR *s);
  //@}

  /**
   * Calls <::waitpid> on UNIX/POSIX platforms and <::await> on
   * Chorus.  Does not work on Vxworks, or pSoS.
   * On Win32, <pid> is ignored if the <handle> is not equal to 0.
   * Passing the process <handle> is prefer on Win32 because using
   * <pid> to wait on the project doesn't always work correctly
   * if the waited process has already terminated.
   */
  static pid_t waitpid (pid_t pid,
                        ACE_exitcode *status = 0,
                        int wait_options = 0,
                        ACE_HANDLE handle = 0);

  /**
   * Calls <::WaitForSingleObject> on Win32 and <ACE::waitpid>
   * otherwise.  Returns the passed in <pid_t> on success and -1 on
   * failure.
   * On Win32, <pid> is ignored if the <handle> is not equal to 0.
   * Passing the process <handle> is prefer on Win32 because using
   * <pid> to wait on the project doesn't always work correctly
   * if the waited process has already terminated.
   */
  static pid_t wait (pid_t pid,
                     ACE_exitcode *status,
                     int wait_options = 0,
                     ACE_HANDLE handle = 0);

  /// Calls OS <::wait> function, so it's only portable to UNIX/POSIX
  /// platforms.
  static pid_t wait (int * = 0);

  //@{ @name A set of wrappers for timers and resource stats.
  static u_int alarm (u_int secs);
  static u_int ualarm (u_int usecs,
                       u_int interval = 0);
  static u_int ualarm (const ACE_Time_Value &tv,
                       const ACE_Time_Value &tv_interval = ACE_Time_Value::zero);
  static ACE_hrtime_t gethrtime (const ACE_HRTimer_Op = ACE_HRTIMER_GETTIME);
# if defined (ACE_HAS_POWERPC_TIMER) && (defined (ghs) || defined (__GNUG__))
  static void readPPCTimeBase (u_long &most,
                               u_long &least);
# endif /* ACE_HAS_POWERPC_TIMER  &&  (ghs or __GNUG__) */
  static int clock_gettime (clockid_t,
                            struct timespec *);
  static ACE_Time_Value gettimeofday (void);
  static int getrusage (int who,
                        struct rusage *rusage);
  static int getrlimit (int resource,
                        struct rlimit *rl);
  static int setrlimit (int resource,
                        ACE_SETRLIMIT_TYPE *rl);
  static int sleep (u_int seconds);
  static int sleep (const ACE_Time_Value &tv);
  static int nanosleep (const struct timespec *requested,
                        struct timespec *remaining = 0);

# if defined (ACE_HAS_BROKEN_R_ROUTINES)
#   undef ctime_r
#   undef asctime_r
#   undef rand_r
#   undef getpwnam_r
# endif /* ACE_HAS_BROKEN_R_ROUTINES */
  //@}

  //@{ @name A set of wrappers for operations on time.
# if !defined (ACE_HAS_WINCE)
  static time_t mktime (struct tm *timeptr);
# endif /* !ACE_HAS_WINCE */

  // wrapper for time zone information.
  static void tzset (void);

# if defined (timezone)
#   undef timezone
# endif /* timezone */
  static long timezone (void);

# if defined (difftime)
#   undef difftime
# endif /* difftime */
  static double difftime (time_t t1,
                          time_t t0);
  static time_t time (time_t *tloc = 0);
  static struct tm *localtime (const time_t *clock);
  static struct tm *localtime_r (const time_t *clock,
                                 struct tm *res);
  static struct tm *gmtime (const time_t *clock);
  static struct tm *gmtime_r (const time_t *clock,
                              struct tm *res);
  static char *asctime (const struct tm *tm);
  static char *asctime_r (const struct tm *tm,
                          char *buf, int buflen);
  static ACE_TCHAR *ctime (const time_t *t);
  static ACE_TCHAR *ctime_r (const time_t *clock, ACE_TCHAR *buf, int buflen);
  static size_t strftime (char *s,
                          size_t maxsize,
                          const char *format,
                          const struct tm *timeptr);
  //@}

  //@{ @name A set of wrappers for System V message queues.
  static int msgctl (int msqid,
                     int cmd,
                     struct msqid_ds *);
  static int msgget (key_t key,
                     int msgflg);
  static int msgrcv (int int_id,
                     void *buf,
                     size_t len,
                     long type,
                     int flags);
  static int msgsnd (int int_id,
                     const void *buf,
                     size_t len,
                     int flags);
  //@}

  //@{ @name A set of wrappers for memory mapped files.
  static int madvise (caddr_t addr,
                      size_t len,
                      int advice);
  static void *mmap (void *addr,
                     size_t len,
                     int prot,
                     int flags,
                     ACE_HANDLE handle,
                     off_t off = 0,
                     ACE_HANDLE *file_mapping = 0,
                     LPSECURITY_ATTRIBUTES sa = 0,
                     const ACE_TCHAR *file_mapping_name = 0);
  static int mprotect (void *addr,
                       size_t len,
                       int prot);
  static int msync (void *addr,
                    size_t len,
                    int sync);
  static int munmap (void *addr,
                     size_t len);
  //@}

  //@{ @name A set of wrappers for recursive mutex locks.
  static int recursive_mutex_init (ACE_recursive_thread_mutex_t *m,
                                   const ACE_TCHAR *name = 0,
                                   ACE_mutexattr_t *arg = 0,
                                   LPSECURITY_ATTRIBUTES sa = 0);
  static int recursive_mutex_destroy (ACE_recursive_thread_mutex_t *m);
  static int recursive_mutex_lock (ACE_recursive_thread_mutex_t *m);
  static int recursive_mutex_trylock (ACE_recursive_thread_mutex_t *m);
  static int recursive_mutex_unlock (ACE_recursive_thread_mutex_t *m);
  //@}

  //@{ @name A set of wrappers for mutex locks.
  static int mutex_init (ACE_mutex_t *m,
                         int type = ACE_DEFAULT_SYNCH_TYPE,
                         const char *name = 0,
                         ACE_mutexattr_t *arg = 0,
                         LPSECURITY_ATTRIBUTES sa = 0);
#if defined (ACE_HAS_WCHAR)
  static int mutex_init (ACE_mutex_t *m,
                         int type,
                         const wchar_t *name,
                         ACE_mutexattr_t *arg = 0,
                         LPSECURITY_ATTRIBUTES sa = 0);
#endif /* ACE_HAS_WCHAR */
  static int mutex_destroy (ACE_mutex_t *m);

  /// Win32 note: Abandoned mutexes are not treated differently. 0 is
  /// returned since the calling thread does get the ownership.
  static int mutex_lock (ACE_mutex_t *m);

  /// This method is only implemented for Win32.  For abandoned
  /// mutexes, <abandoned> is set to 1 and 0 is returned.
  static int mutex_lock (ACE_mutex_t *m,
                         int &abandoned);

  /**
   * This method attempts to acquire a lock, but gives up if the lock
   * has not been acquired by the given time.  If the lock is not
   * acquired within the given amount of time, then this method
   * returns -1 with an <ETIME> errno on platforms that actually
   * support timed mutexes.  The timeout should be an absolute time.
   * Note that the mutex should not be a recursive one, i.e., it
   * should only be a standard mutex or an error checking mutex.
   */
  static int mutex_lock (ACE_mutex_t *m,
                         const ACE_Time_Value &timeout);

  /**
   * If <timeout> == 0, calls <ACE_OS::mutex_lock(m)>.  Otherwise,
   * this method attempts to acquire a lock, but gives up if the lock
   * has not been acquired by the given time, in which case it returns
   * -1 with an <ETIME> errno on platforms that actually support timed
   * mutexes.  The timeout should be an absolute time.  Note that the
   * mutex should not be a recursive one, i.e., it should only be a
   * standard mutex or an error checking mutex.
   */
  static int mutex_lock (ACE_mutex_t *m,
                         const ACE_Time_Value *timeout);

  /// Win32 note: Abandoned mutexes are not treated differently. 0 is
  /// returned since the calling thread does get the ownership.
  static int mutex_trylock (ACE_mutex_t *m);

  /// This method is only implemented for Win32.  For abandoned
  /// mutexes, <abandoned> is set to 1 and 0 is returned.
  static int mutex_trylock (ACE_mutex_t *m,
                            int &abandoned);

  static int mutex_unlock (ACE_mutex_t *m);
  //@}

  //@{ @name A set of wrappers for mutex locks that only work within a single process.
  static int thread_mutex_init (ACE_thread_mutex_t *m,
                                int type = ACE_DEFAULT_SYNCH_TYPE,
                                const char *name = 0,
                                ACE_mutexattr_t *arg = 0);
#if defined (ACE_HAS_WCHAR)
  static int thread_mutex_init (ACE_thread_mutex_t *m,
                                int type,
                                const wchar_t *name,
                                ACE_mutexattr_t *arg = 0);
#endif /* ACE_HAS_WCHAR */
  static int thread_mutex_destroy (ACE_thread_mutex_t *m);
  static int thread_mutex_lock (ACE_thread_mutex_t *m);
  static int thread_mutex_lock (ACE_thread_mutex_t *m,
                                const ACE_Time_Value &timeout);
  static int thread_mutex_lock (ACE_thread_mutex_t *m,
                                const ACE_Time_Value *timeout);
  static int thread_mutex_trylock (ACE_thread_mutex_t *m);
  static int thread_mutex_unlock (ACE_thread_mutex_t *m);
  //@}

  //@{ @name A set of wrappers for low-level file operations.
  static int access (const char *path, int amode);
#if defined (ACE_HAS_WCHAR)
  static int access (const wchar_t *path, int amode);
#endif /* ACE_HAS_WCHAR */

  static int close (ACE_HANDLE handle);
  static ACE_HANDLE creat (const ACE_TCHAR *filename,
                           mode_t mode);
  static ACE_HANDLE dup (ACE_HANDLE handle);
  static int dup2 (ACE_HANDLE oldfd,
                   ACE_HANDLE newfd);
  static int fattach (int handle,
                      const char *path);
  static long filesize (ACE_HANDLE handle);
  static long filesize (const ACE_TCHAR *handle);
  static int getmsg (ACE_HANDLE handle,
                     struct strbuf *ctl,
                     struct strbuf
                     *data, int *flags);
  static int getpmsg (ACE_HANDLE handle,
                      struct strbuf *ctl,
                      struct strbuf
                      *data,
                      int *band,
                      int *flags);

  /// UNIX-style <ioctl>.
  static int ioctl (ACE_HANDLE handle,
                    int cmd,
                    void * = 0);

#if !defined (ACE_HAS_WINCE)
  /// QoS-enabled <ioctl>.
  static int ioctl (ACE_HANDLE socket,
                    u_long io_control_code,
                    void *in_buffer_p,
                    u_long in_buffer,
                    void *out_buffer_p,
                    u_long out_buffer,
                    u_long *bytes_returned,
                    ACE_OVERLAPPED *overlapped,
                    ACE_OVERLAPPED_COMPLETION_FUNC func);

  /// QoS-enabled <ioctl> when the I/O control code is either
  /// SIO_SET_QOS or SIO_GET_QOS.
  static int ioctl (ACE_HANDLE socket,
                    u_long io_control_code,
                    ACE_QoS &ace_qos,
                    u_long *bytes_returned,
                    void *buffer_p = 0,
                    u_long buffer = 0,
                    ACE_OVERLAPPED *overlapped = 0,
                    ACE_OVERLAPPED_COMPLETION_FUNC func = 0);
#endif  // ACE_HAS_WINCE

  static int isastream (ACE_HANDLE handle);
  static int isatty (int handle);
#if defined (ACE_WIN32)
  static int isatty (ACE_HANDLE handle);
#endif /* ACE_WIN32 */
  static off_t lseek (ACE_HANDLE handle,
                      off_t offset,
                      int whence);
#if defined (ACE_HAS_LLSEEK) || defined (ACE_HAS_LSEEK64)
  static ACE_LOFF_T llseek (ACE_HANDLE handle, ACE_LOFF_T offset, int whence);
#endif /* ACE_HAS_LLSEEK */

  // It used to be that the <perms> argument default was 0 on all
  // platforms. Further, the ACE_OS::open implementations ignored <perms>
  // for Win32 and always supplied read|write|delete. To preserve
  // backward compatibility and allow users to pass in values
  // that are used as desired, the defaults are now what the default
  // action used to be on Win32. The implementation now obeys what is passed.
#if defined (ACE_WIN32)
#  if defined (ACE_HAS_WINNT4) && (ACE_HAS_WINNT4 == 1)
#    define ACE_DEFAULT_OPEN_PERMS (FILE_SHARE_READ | FILE_SHARE_WRITE | \
                                    FILE_SHARE_DELETE)
#  else
#    define ACE_DEFAULT_OPEN_PERMS (FILE_SHARE_READ | FILE_SHARE_WRITE)
#  endif /* ACE_HAS_WINCE */
#else
#  define ACE_DEFAULT_OPEN_PERMS 0
#endif  /* ACE_WIN32 */

  /// The O_APPEND flag is only partly supported on Win32. If you specify
  /// O_APPEND, then the file pointer will be positioned at the end of
  /// the file initially during open, but it is not re-positioned at
  /// the end prior to each write, as specified by POSIX.  This
  /// is generally good enough for typical situations, but it is ``not
  /// quite right'' in its semantics.
  static ACE_HANDLE open (const char *filename,
                          int mode,
                          int perms = ACE_DEFAULT_OPEN_PERMS,
                          LPSECURITY_ATTRIBUTES sa = 0);
#if defined (ACE_HAS_WCHAR)
  static ACE_HANDLE open (const wchar_t *filename,
                          int mode,
                          int perms = ACE_DEFAULT_OPEN_PERMS,
                          LPSECURITY_ATTRIBUTES sa = 0);
#endif /* ACE_HAS_WCHAR */
  static int putmsg (ACE_HANDLE handle,
                     const struct strbuf *ctl,
                     const struct strbuf *data,
                     int flags);
  static int putpmsg (ACE_HANDLE handle,
                      const struct strbuf *ctl,
                      const struct strbuf *data,
                      int band,
                      int flags);
  static ssize_t read (ACE_HANDLE handle,
                       void *buf,
                       size_t len);
  static ssize_t read (ACE_HANDLE handle,
                       void *buf,
                       size_t len,
                       ACE_OVERLAPPED *);
  /**
   * Receive <len> bytes into <buf> from <handle> (uses the
   * <ACE_OS::read> call, which uses the <read> system call on UNIX
   * and the <ReadFile> call on Win32). If errors occur, -1 is
   * returned.  If EOF occurs, 0 is returned.  Whatever data has been
   * transmitted will be returned to the caller through
   * <bytes_transferred>.
   */
  static ssize_t read_n (ACE_HANDLE handle,
                         void *buf,
                         size_t len,
                         size_t *bytes_transferred = 0);

  static int readlink (const char *path,
                       char *buf,
                       size_t bufsiz);
  static ssize_t pread (ACE_HANDLE handle,
                        void *buf,
                        size_t nbyte,
                        off_t offset);
  static int recvmsg (ACE_HANDLE handle,
                      struct msghdr *msg,
                      int flags);
  static int sendmsg (ACE_HANDLE handle,
                      const struct msghdr *msg,
                      int flags);
  static ssize_t write (ACE_HANDLE handle,
                        const void *buf,
                        size_t nbyte);
  static ssize_t write (ACE_HANDLE handle,
                        const void *buf,
                        size_t nbyte,
                        ACE_OVERLAPPED *);

  /**
   * Send <len> bytes from <buf> to <handle> (uses the <ACE_OS::write>
   * calls, which is uses the <write> system call on UNIX and the
   * <WriteFile> call on Win32).  If errors occur, -1 is returned.  If
   * EOF occurs, 0 is returned.  Whatever data has been transmitted
   * will be returned to the caller through <bytes_transferred>.
   */
  static ssize_t write_n (ACE_HANDLE handle,
                          const void *buf,
                          size_t len,
                          size_t *bytes_transferred = 0);

  static ssize_t pwrite (ACE_HANDLE handle,
                         const void *buf,
                         size_t nbyte,
                         off_t offset);
  static ssize_t readv (ACE_HANDLE handle,
                        iovec *iov,
                        int iovlen);
  static ssize_t writev (ACE_HANDLE handle,
                         const iovec *iov,
                         int iovcnt);
  static ssize_t recvv (ACE_HANDLE handle,
                        iovec *iov,
                        int iovlen);
  static ssize_t sendv (ACE_HANDLE handle,
                        const iovec *iov,
                        int iovcnt);
  //@}

  //@{ @name A set of wrappers for event demultiplexing and IPC.
  static int select (int width,
                     fd_set *rfds,
                     fd_set *wfds = 0,
                     fd_set *efds = 0,
                     const ACE_Time_Value *tv = 0);
  static int select (int width,
                     fd_set *rfds,
                     fd_set *wfds,
                     fd_set *efds,
                     const ACE_Time_Value &tv);
  static int poll (struct pollfd *pollfds,
                   u_long len,
                   const ACE_Time_Value *tv = 0);
  static int poll (struct pollfd *pollfds,
                   u_long len,
                   const ACE_Time_Value &tv);
  static int pipe (ACE_HANDLE handles[]);

  static ACE_HANDLE shm_open (const ACE_TCHAR *filename,
                              int mode,
                              int perms = 0,
                              LPSECURITY_ATTRIBUTES sa = 0);
  static int shm_unlink (const ACE_TCHAR *path);
  //@}

  //@{ @name A set of wrappers for directory operations.
  static mode_t umask (mode_t cmask);

#if !defined (ACE_LACKS_CHDIR)
  static int chdir (const char *path);

#if defined (ACE_HAS_WCHAR)
  static int chdir (const wchar_t *path);
#endif /* ACE_HAS_WCHAR */
#endif /* ACE_LACKS_CHDIR */

  static int mkdir (const ACE_TCHAR *path,
                    mode_t mode = ACE_DEFAULT_DIR_PERMS);
  static int mkfifo (const ACE_TCHAR *file,
                     mode_t mode = ACE_DEFAULT_FILE_PERMS);
  static ACE_TCHAR *mktemp (ACE_TCHAR *t);
  static ACE_HANDLE mkstemp (ACE_TCHAR *t);
  static ACE_TCHAR *getcwd (ACE_TCHAR *, size_t);
  static int rename (const ACE_TCHAR *old_name,
                     const ACE_TCHAR *new_name,
                     int flags = -1);
  static int unlink (const ACE_TCHAR *path);
  static ACE_TCHAR *tempnam (const ACE_TCHAR *dir = 0,
                             const ACE_TCHAR *pfx = 0);
  //@}

  //@{ @name A set of wrappers for random number operations.
  static int rand (void);
  static int rand_r (ACE_RANDR_TYPE &seed);
  static void srand (u_int seed);
  //@}

  //@{ @name A set of wrappers for readers/writer locks.
  static int rwlock_init (ACE_rwlock_t *rw,
                          int type = ACE_DEFAULT_SYNCH_TYPE,
                          const ACE_TCHAR *name = 0,
                          void *arg = 0);
  static int rwlock_destroy (ACE_rwlock_t *rw);
  static int rw_rdlock (ACE_rwlock_t *rw);
  static int rw_wrlock (ACE_rwlock_t *rw);
  static int rw_tryrdlock (ACE_rwlock_t *rw);
  static int rw_trywrlock (ACE_rwlock_t *rw);
  static int rw_trywrlock_upgrade (ACE_rwlock_t *rw);
  static int rw_unlock (ACE_rwlock_t *rw);
  //@}

  //@{ @name A set of wrappers for auto-reset and manual events.
  static int event_init (ACE_event_t *event,
                         int manual_reset = 0,
                         int initial_state = 0,
                         int type = ACE_DEFAULT_SYNCH_TYPE,
                         const char *name = 0,
                         void *arg = 0,
                         LPSECURITY_ATTRIBUTES sa = 0);
# if defined (ACE_HAS_WCHAR)
  static int event_init (ACE_event_t *event,
                         int manual_reset,
                         int initial_state,
                         int type,
                         const wchar_t *name,
                         void *arg = 0,
                         LPSECURITY_ATTRIBUTES sa = 0);
# endif /* ACE_HAS_WCHAR */
  static int event_destroy (ACE_event_t *event);
  static int event_wait (ACE_event_t *event);
  static int event_timedwait (ACE_event_t *event,
                              ACE_Time_Value *timeout,
                              int use_absolute_time = 1);
  static int event_signal (ACE_event_t *event);
  static int event_pulse (ACE_event_t *event);
  static int event_reset (ACE_event_t *event);
  //@}

  //@{ @name A set of wrappers for semaphores.
  static int sema_destroy (ACE_sema_t *s);
  static int sema_init (ACE_sema_t *s,
                        u_int count,
                        int type = ACE_DEFAULT_SYNCH_TYPE,
                        const char *name = 0,
                        void *arg = 0,
                        int max = 0x7fffffff,
                        LPSECURITY_ATTRIBUTES sa = 0);
# if defined (ACE_HAS_WCHAR)
  static int sema_init (ACE_sema_t *s,
                        u_int count,
                        int type,
                        const wchar_t *name,
                        void *arg = 0,
                        int max = 0x7fffffff,
                        LPSECURITY_ATTRIBUTES sa = 0);
# endif /* ACE_HAS_WCHAR */
  static int sema_post (ACE_sema_t *s);
  static int sema_post (ACE_sema_t *s,
                        size_t release_count);
  static int sema_trywait (ACE_sema_t *s);
  static int sema_wait (ACE_sema_t *s);
  static int sema_wait (ACE_sema_t *s,
                        ACE_Time_Value &tv);
  static int sema_wait (ACE_sema_t *s,
                        ACE_Time_Value *tv);
  //@}

  //@{ @name A set of wrappers for System V semaphores.
  static int semctl (int int_id,
                     int semnum,
                     int cmd,
                     semun);
  static int semget (key_t key,
                     int nsems,
                     int flags);
  static int semop (int int_id,
                    struct sembuf *sops,
                    size_t nsops);
  //@}

  //@{ @name Thread scheduler interface.
  /// Set scheduling parameters.  An id of ACE_SELF indicates, e.g.,
  /// set the parameters on the calling thread.
  static int sched_params (const ACE_Sched_Params &, ACE_id_t id = ACE_SELF);
  //@}

  //@{ @name A set of wrappers for System V shared memory.
  static void *shmat (int int_id,
                      void *shmaddr,
                      int shmflg);
  static int shmctl (int int_id,
                     int cmd,
                     struct shmid_ds *buf);
  static int shmdt (void *shmaddr);
  static int shmget (key_t key,
                     int size,
                     int flags);
  ///@}

  //@{ @name A set of wrappers for Signals.
  static int kill (pid_t pid,
                   int signum);
  static int sigaction (int signum,
                        const struct sigaction *nsa,
                        struct sigaction *osa);
  static int sigaddset (sigset_t *s,
                        int signum);
  static int sigdelset (sigset_t *s,
                        int signum);
  static int sigemptyset (sigset_t *s);
  static int sigfillset (sigset_t *s);
  static int sigismember (sigset_t *s,
                          int signum);
  static ACE_SignalHandler signal (int signum,
                                   ACE_SignalHandler);
  static int sigsuspend (const sigset_t *set);
  static int sigprocmask (int how,
                          const sigset_t *nsp,
                          sigset_t *osp);

  static int pthread_sigmask (int how,
                              const sigset_t *nsp,
                              sigset_t *osp);
  //@}

  //@{ @name A set of wrappers for sockets.
  /// BSD-style <accept> (no QoS).
  static ACE_HANDLE accept (ACE_HANDLE handle,
                            struct sockaddr *addr,
                            int *addrlen);

#if !defined (ACE_HAS_WINCE)
  /**
   * QoS-enabled <accept>, which passes <qos_params> to <accept>.  If
   * the OS platform doesn't support QoS-enabled <accept> then the
   * <qos_params> are ignored and the BSD-style <accept> is called.
   */
  static ACE_HANDLE accept (ACE_HANDLE handle,
                            struct sockaddr *addr,
                            int *addrlen,
                            const ACE_Accept_QoS_Params &qos_params);
#endif  // ACE_HAS_WINCE

  /// BSD-style <connect> (no QoS).
  static int connect (ACE_HANDLE handle,
                      struct sockaddr *addr,
                      int addrlen);

#if !defined (ACE_HAS_WINCE)
  /**
   * QoS-enabled <connect>, which passes <qos_params> to <connect>.
   * If the OS platform doesn't support QoS-enabled <connect> then the
   * <qos_params> are ignored and the BSD-style <connect> is called.
   */
  static int connect (ACE_HANDLE handle,
                      const sockaddr *addr,
                      int addrlen,
                      const ACE_QoS_Params &qos_params);
#endif  // ACE_HAS_WINCE

  static int bind (ACE_HANDLE s,
                   struct sockaddr *name,
                   int namelen);

  static int closesocket (ACE_HANDLE s);
  static struct hostent *gethostbyaddr (const char *addr,
                                        int length,
                                        int type);
  static struct hostent *gethostbyname (const char *name);
  static struct hostent *getipnodebyname (const char *name, int family,
                                          int flags = 0);
  static struct hostent *getipnodebyaddr (const void *src, size_t len,
                                          int family);
  static struct hostent *gethostbyaddr_r (const char *addr,
                                          int length,
                                          int type,
                                          struct hostent *result,
                                          ACE_HOSTENT_DATA buffer,
                                          int *h_errnop);
  static struct hostent *gethostbyname_r (const char *name,
                                          struct hostent *result,
                                          ACE_HOSTENT_DATA buffer,
                                          int *h_errnop);
  static int getpeername (ACE_HANDLE handle,
                          struct sockaddr *addr,
                          int *addrlen);
  static struct protoent *getprotobyname (const char *name);
  static struct protoent *getprotobyname_r (const char *name,
                                            struct protoent *result,
                                            ACE_PROTOENT_DATA buffer);
  static struct protoent *getprotobynumber (int proto);
  static struct protoent *getprotobynumber_r (int proto,
                                              struct protoent *result,
                                              ACE_PROTOENT_DATA buffer);
  static struct servent *getservbyname (const char *svc,
                                        const char *proto);
  static struct servent *getservbyname_r (const char *svc,
                                          const char *proto,
                                          struct servent *result,
                                          ACE_SERVENT_DATA buf);
  static int getsockname (ACE_HANDLE handle,
                          struct sockaddr *addr,
                          int *addrlen);
  static int getsockopt (ACE_HANDLE handle,
                         int level,
                         int optname,
                         char *optval,
                         int *optlen);
  static unsigned long inet_addr (const char *name);
  static char *inet_ntoa (const struct in_addr addr);
  static int inet_aton (const char *strptr,
                        struct in_addr *addr);
  static const char *inet_ntop (int family,
                                const void *addrptr,
                                char *strptr,
                                 size_t len);
  static int inet_pton (int family,
                        const char *strptr,
                        void *addrptr);
  /// Retrieve information about available transport protocols
  /// installed on the local machine.
  static int enum_protocols (int *protocols,
                             ACE_Protocol_Info *protocol_buffer,
                             u_long *buffer_length);

#if !defined (ACE_HAS_WINCE)
  /// Joins a leaf node into a QoS-enabled multi-point session.
  static ACE_HANDLE join_leaf (ACE_HANDLE socket,
                               const sockaddr *name,
                               int namelen,
                               const ACE_QoS_Params &qos_params);
#endif  // ACE_HAS_WINCE

  static int listen (ACE_HANDLE handle,
                     int backlog);
  static int recv (ACE_HANDLE handle,
                   char *buf,
                   size_t len,
                   int flags = 0);
  static int recvfrom (ACE_HANDLE handle,
                       char *buf,
                       size_t len,
                       int flags,
                       struct sockaddr *addr,
                       int *addrlen);
  static int recvfrom (ACE_HANDLE handle,
                       iovec *buffers,
                       int buffer_count,
                       size_t &number_of_bytes_recvd,
                       int &flags,
                       struct sockaddr *addr,
                       int *addrlen,
                       ACE_OVERLAPPED *overlapped,
                       ACE_OVERLAPPED_COMPLETION_FUNC func);
  static int send (ACE_HANDLE handle,
                   const char *buf,
                   size_t len,
                   int flags = 0);
  static int sendto (ACE_HANDLE handle,
                     const char *buf,
                     size_t len,
                     int flags,
                     const struct sockaddr *addr,
                     int addrlen);
  static int sendto (ACE_HANDLE handle,
                     const iovec *buffers,
                     int buffer_count,
                     size_t &number_of_bytes_sent,
                     int flags,
                     const struct sockaddr *addr,
                     int addrlen,
                     ACE_OVERLAPPED *overlapped,
                     ACE_OVERLAPPED_COMPLETION_FUNC func);

  /// Manipulate the options associated with a socket.
  static int setsockopt (ACE_HANDLE handle,
                         int level,
                         int optname,
                         const char *optval,
                         int optlen);
  static int shutdown (ACE_HANDLE handle,
                       int how);

  /// Create a BSD-style socket (no QoS).
  static ACE_HANDLE socket (int protocol_family,
                            int type,
                            int proto);

  /// Create a QoS-enabled socket.  If the OS platform doesn't support
  /// QoS-enabled <socket> then the BSD-style <socket> is called.
  static ACE_HANDLE socket (int protocol_family,
                            int type,
                            int proto,
                            ACE_Protocol_Info *protocolinfo,
                            ACE_SOCK_GROUP g,
                            u_long flags);

  static int socketpair (int domain,
                         int type,
                         int protocol,
                         ACE_HANDLE sv[2]);

  /// Initialize WinSock before first use (e.g., when a DLL is first
  /// loaded or the first use of a socket() call.
  static int socket_init (int version_high = 1,
                          int version_low = 1);

  /// Finalize WinSock after last use (e.g., when a DLL is unloaded).
  static int socket_fini (void);
  //@}

  //@{ @name A set of wrappers for password routines.
  static void setpwent (void);
  static void endpwent (void);
  static struct passwd *getpwent (void);
  static struct passwd *getpwnam (const char *user);
  static struct passwd *getpwnam_r (const char *name,
                                    struct passwd *pwent,
                                    char *buffer,
                                    int buflen);
  //@}

  //@{ @name A set of wrappers for regular expressions.
  static char *compile (const char *instring,
                        char *expbuf,
                        char *endbuf);
  static int step (const char *str,
                   char *expbuf);
  //@}

  //@{ @name Wide-character strings
  // @@ UNICODE: (brunsch) Can this be handled better?
  // The following WChar typedef and functions are used by TAO.  TAO
  // does not use wchar_t because the size of wchar_t is
  // platform-dependent. These are to be used for all
  // manipulate\ions of CORBA::WString.
  // @@ (othman) IMHO, it is the lesser of two evils to use the
  //    correct type for the platform rather than (forcibly) assume
  //    that all wide characters are 16 bits.
#ifdef ACE_HAS_WCHAR
  typedef wchar_t WChar;
#else
  typedef ACE_UINT16 WChar;
#endif
  static u_int wslen (const WChar *);
  static WChar *wscpy (WChar *,
                       const WChar *);
  static int wscmp (const WChar *,
                    const WChar *);
  static int wsncmp (const WChar *,
                     const WChar *,
                     size_t len);
  //@}

# if 0
  //@{ @name A set of wrappers for threads (these are portable since they use the ACE_Thread_ID).
  static int thr_continue (const ACE_Thread_ID &thread);
  static int thr_create (ACE_THR_FUNC,
                         void *args,
                         long flags,
                         ACE_Thread_ID *,
                         long priority = ACE_DEFAULT_THREAD_PRIORITY,
                         void *stack = 0,
                         size_t stacksize = 0);
  static int thr_getprio (ACE_Thread_ID thr_id,
                          int &prio,
                          int *policy = 0);
  static int thr_join (ACE_Thread_ID waiter_id,
                       ACE_THR_FUNC_RETURN *status);
  static int thr_kill (ACE_Thread_ID thr_id,
                       int signum);
  static ACE_Thread_ID thr_self (void);
  static int thr_setprio (ACE_Thread_ID thr_id,
                          int prio);
  static int thr_setprio (const ACE_Sched_Priority prio);
  static int thr_suspend (ACE_Thread_ID target_thread);
  static int thr_cancel (ACE_Thread_ID t_id);
  //@}
# endif /* 0 */

  //@{ @name A set of wrappers for threads

  // These are non-portable since they use ACE_thread_t and
  // ACE_hthread_t and will go away in a future release.
  static int thr_continue (ACE_hthread_t target_thread);

  /*
   * Creates a new thread having <flags> attributes and running <func>
   * with <args> (if <thread_adapter> is non-0 then <func> and <args>
   * are ignored and are obtained from <thread_adapter>).  <thr_id>
   * and <t_handle> are set to the thread's ID and handle (?),
   * respectively.  The thread runs at <priority> priority (see
   * below).
   *
   * The <flags> are a bitwise-OR of the following:
   * = BEGIN<INDENT>
   * THR_CANCEL_DISABLE, THR_CANCEL_ENABLE, THR_CANCEL_DEFERRED,
   * THR_CANCEL_ASYNCHRONOUS, THR_BOUND, THR_NEW_LWP, THR_DETACHED,
   * THR_SUSPENDED, THR_DAEMON, THR_JOINABLE, THR_SCHED_FIFO,
   * THR_SCHED_RR, THR_SCHED_DEFAULT, THR_EXPLICIT_SCHED,
   * THR_SCOPE_SYSTEM, THR_SCOPE_PROCESS
   * = END<INDENT>
   *
   * By default, or if <priority> is set to
   * ACE_DEFAULT_THREAD_PRIORITY, an "appropriate" priority value for
   * the given scheduling policy (specified in <flags}>, e.g.,
   * <THR_SCHED_DEFAULT>) is used.  This value is calculated
   * dynamically, and is the median value between the minimum and
   * maximum priority values for the given policy.  If an explicit
   * value is given, it is used.  Note that actual priority values are
   * EXTREMEMLY implementation-dependent, and are probably best
   * avoided.
   *
   * Note that <thread_adapter> is always deleted by <thr_create>,
   * therefore it must be allocated with global operator new.
   */
  static int thr_create (ACE_THR_FUNC func,
                         void *args,
                         long flags,
                         ACE_thread_t *thr_id,
                         ACE_hthread_t *t_handle = 0,
                         long priority = ACE_DEFAULT_THREAD_PRIORITY,
                         void *stack = 0,
                         size_t stacksize = 0,
                         ACE_Base_Thread_Adapter *thread_adapter = 0);

  static int thr_getprio (ACE_hthread_t thr_id,
                          int &prio);
  static int thr_join (ACE_hthread_t waiter_id,
                       ACE_THR_FUNC_RETURN *status);
  static int thr_join (ACE_thread_t waiter_id,
                       ACE_thread_t *thr_id,
                       ACE_THR_FUNC_RETURN *status);
  static int thr_kill (ACE_thread_t thr_id,
                       int signum);
  static ACE_thread_t thr_self (void);
  static void thr_self (ACE_hthread_t &);
  static int thr_setprio (ACE_hthread_t thr_id,
                          int prio);
  static int thr_setprio (const ACE_Sched_Priority prio);
  static int thr_suspend (ACE_hthread_t target_thread);
  static int thr_cancel (ACE_thread_t t_id);

  static int thr_cmp (ACE_hthread_t t1,
                      ACE_hthread_t t2);
  static int thr_equal (ACE_thread_t t1,
                        ACE_thread_t t2);
  static void thr_exit (ACE_THR_FUNC_RETURN status = 0);
  static int thr_getconcurrency (void);
  static int lwp_getparams (ACE_Sched_Params &);
# if defined (ACE_HAS_TSS_EMULATION) && defined (ACE_HAS_THREAD_SPECIFIC_STORAGE)
  static int thr_getspecific (ACE_OS_thread_key_t key,
                              void **data);
# endif /* ACE_HAS_TSS_EMULATION && ACE_HAS_THREAD_SPECIFIC_STORAGE */
  static int thr_getspecific (ACE_thread_key_t key,
                              void **data);
  static int thr_keyfree (ACE_thread_key_t key);
  static int thr_key_detach (void *inst);
# if defined (ACE_HAS_THR_C_DEST)
#   if defined (ACE_HAS_TSS_EMULATION) && defined (ACE_HAS_THREAD_SPECIFIC_STORAGE)
  static int thr_keycreate (ACE_OS_thread_key_t *key,
                            ACE_THR_C_DEST,
                            void *inst = 0);
#   endif /* ACE_HAS_TSS_EMULATION && ACE_HAS_THREAD_SPECIFIC_STORAGE */
  static int thr_keycreate (ACE_thread_key_t *key,
                            ACE_THR_C_DEST,
                            void *inst = 0);
# else
#   if defined (ACE_HAS_TSS_EMULATION) && defined (ACE_HAS_THREAD_SPECIFIC_STORAGE)
  static int thr_keycreate (ACE_OS_thread_key_t *key,
                            ACE_THR_DEST,
                            void *inst = 0);
#   endif /* ACE_HAS_TSS_EMULATION && ACE_HAS_THREAD_SPECIFIC_STORAGE */
  static int thr_keycreate (ACE_thread_key_t *key,
                            ACE_THR_DEST,
                            void *inst = 0);
# endif /* ACE_HAS_THR_C_DEST */
  static int thr_key_used (ACE_thread_key_t key);
  static size_t thr_min_stack (void);
  static int thr_setconcurrency (int hint);
  static int lwp_setparams (const ACE_Sched_Params &);
# if defined (ACE_HAS_TSS_EMULATION) && defined (ACE_HAS_THREAD_SPECIFIC_STORAGE)
  static int thr_setspecific (ACE_OS_thread_key_t key,
                              void *data);
# endif /* ACE_HAS_TSS_EMULATION && ACE_HAS_THREAD_SPECIFIC_STORAGE */
  static int thr_setspecific (ACE_thread_key_t key,
                              void *data);
  static int thr_sigsetmask (int how,
                             const sigset_t *nsm,
                             sigset_t *osm);
  static int thr_setcancelstate (int new_state,
                                 int *old_state);
  static int thr_setcanceltype (int new_type,
                                int *old_type);
  static int sigwait (sigset_t *set,
                      int *sig = 0);
  static int sigtimedwait (const sigset_t *set,
                           siginfo_t *info,
                           const ACE_Time_Value *timeout);
  static void thr_testcancel (void);
  static void thr_yield (void);

  /**
   * This method uses process id and object pointer to come up with a
   * machine wide unique name.  The process ID will provide uniqueness
   * between processes on the same machine. The "this" pointer of the
   * <object> will provide uniqueness between other "live" objects in
   * the same process. The uniqueness of this name is therefore only
   * valid for the life of <object>.
   */
  static void unique_name (const void *object,
                           ACE_TCHAR *name,
                           size_t length);

  /// This is necessary to deal with POSIX pthreads and their use of
  /// structures for thread ids.
  static ACE_thread_t NULL_thread;

  /// This is necessary to deal with POSIX pthreads and their use of
  /// structures for thread handles.
  static ACE_hthread_t NULL_hthread;

  /// This is necessary to deal with POSIX pthreads and their use of
  /// structures for TSS keys.
  static ACE_thread_key_t NULL_key;

# if defined (CHORUS)
  /// This is used to map an actor's id into a KnCap for killing and
  /// waiting actors.
  static KnCap actorcaps_[ACE_CHORUS_MAX_ACTORS];
# endif /* CHORUS */
  //@}

# if defined (ACE_WIN32)
  /// Keeps track of whether we've already initialized WinSock...
  static int socket_initialized_;
# endif /* ACE_WIN32 */

  /// Handle asynchronous thread cancellation cleanup.
  static void mutex_lock_cleanup (void *mutex);

  /**
   * Call TSS destructors for the current thread.  If the current
   * thread is the main thread, then the argument must be 1.
   * For private use of ACE_Object_Manager and ACE_Thread_Adapter only.
   */
  static void cleanup_tss (const u_int main_thread);

# if defined (ACE_MT_SAFE) && (ACE_MT_SAFE != 0) && defined (ACE_LACKS_NETDB_REENTRANT_FUNCTIONS)
  static int netdb_acquire (void);
  static int netdb_release (void);
# endif /* defined (ACE_MT_SAFE) && ACE_LACKS_NETDB_REENTRANT_FUNCTIONS */

  /// Find the schedling class ID that corresponds to the class name.
  static int scheduling_class (const char *class_name, ACE_id_t &);

  /// Friendly interface to <priocntl>(2).
  static int set_scheduling_params (const ACE_Sched_Params &,
                                    ACE_id_t id = ACE_SELF);

  /// Low-level interface to <priocntl>(2).
  /**
   * Can't call the following priocntl, because that's a macro on
   * Solaris.
   */
  static int priority_control (ACE_idtype_t, ACE_id_t, int, void *);

#if defined (ACE_HAS_STRPTIME)
  static char *strptime (char *buf,
                         const char *format,
                         struct tm *tm);

# if defined (ACE_LACKS_NATIVE_STRPTIME)
  static int strptime_getnum (char *buf, int *num, int *bi,
                              int *fi, int min, int max);
# endif /* ACE_LACKS_NATIVE_STRPTIME */
#endif /* ACE_HAS_STRPTIME */

private:

#if defined (ACE_LACKS_WRITEV)
  static int writev_emulation (ACE_HANDLE handle,
                               ACE_WRITEV_TYPE *iov,
                               int iovcnt);
#endif /* ACE_LACKS_WRITEV */

#if defined (ACE_LACKS_READV)
  static ssize_t readv_emulation (ACE_HANDLE handle,
                                  ACE_READV_TYPE *iov,
                                  int iovcnt);
#endif /* ACE_LACKS_READV */

  /// Function that is called by <ACE_OS::exit>, if non-null.
  static ACE_EXIT_HOOK exit_hook_;

  /// For use by ACE_Object_Manager only, to register its exit hook..
  static ACE_EXIT_HOOK set_exit_hook (ACE_EXIT_HOOK hook);

  /// Allow the ACE_OS_Object_Manager to call set_exit_hook.
  friend class ACE_OS_Object_Manager;

# if defined (ACE_WIN32)
#   if defined (ACE_HAS_WINCE)
  /// Supporting data for ctime and ctime_r functions on WinCE.
  static const wchar_t *day_of_week_name[7];
  static const wchar_t *month_name[12];
#   endif /* ACE_HAS_WINCE */

  /// Translate fopen's mode char to open's mode.  This helper function
  /// is here to avoid maintaining several pieces of identical code.
  static void fopen_mode_to_open_mode_converter (ACE_TCHAR x, int &hmode);

  static OSVERSIONINFO win32_versioninfo_;

  static HINSTANCE win32_resource_module_;

# endif /* ACE_WIN32 */

#if defined (ACE_HAS_VIRTUAL_TIME)
  static clock_t times (struct tms *buf);
#endif /* ACE_HAS_VIRTUAL_TIME */

  //changed for ACE_HAS_VIRTUAL_TIME changes.

  static int cond_timedwait_i (ACE_cond_t *cv,
                       ACE_mutex_t *m,
                       ACE_Time_Value *);

  static u_int alarm_i (u_int secs);

  static u_int ualarm_i (u_int usecs, u_int interval = 0);

  static u_int ualarm_i (const ACE_Time_Value &tv,
                         const ACE_Time_Value &tv_interval = ACE_Time_Value::zero);

  static int sleep_i (u_int seconds);

  static int sleep_i (const ACE_Time_Value &tv);

  static int nanosleep_i (const struct timespec *requested,
                          struct timespec *remaining = 0);

  static int select_i (int width,
                       fd_set *rfds,
                       fd_set *wfds,
                       fd_set *efds,
                       const ACE_Time_Value *tv = 0);

  static int select_i (int width,
                       fd_set *rfds,
                       fd_set *wfds,
                       fd_set *efds,
                       const ACE_Time_Value &tv);

  static int poll_i (struct pollfd *pollfds,
                     u_long len,
                     const ACE_Time_Value *tv = 0);

  static int poll_i (struct pollfd *pollfds,
                     u_long len,
                     const ACE_Time_Value &tv);

  static int sema_wait_i (ACE_sema_t *s);

  static int sema_wait_i (ACE_sema_t *s,
                          ACE_Time_Value &tv);

  static int sigtimedwait_i (const sigset_t *set,
                             siginfo_t *info,
                             const ACE_Time_Value *timeout);

  static ACE_Time_Value gettimeofday_i (void);
};

/**
 * @class ACE_Object_Manager_Base
 *
 * @brief Base class for ACE_Object_Manager(s).
 *
 * Encapsulates the most useful ACE_Object_Manager data structures.
 */
class ACE_OS_Export ACE_Object_Manager_Base
{
# if (defined (ACE_PSOS) && defined (__DIAB))  || \
     (defined (__DECCXX_VER) && __DECCXX_VER < 60000000)
  // The Diab compiler got confused and complained about access rights
  // if this section was protected (changing this to public makes it happy).
  // Similarly, DEC CXX 5.6 needs the methods to be public.
public:
# else  /* ! (ACE_PSOS && __DIAB)  ||  ! __DECCXX_VER < 60000000 */
protected:
# endif /* ! (ACE_PSOS && __DIAB)  ||  ! __DECCXX_VER < 60000000 */
  /// Default constructor.
  ACE_Object_Manager_Base (void);

  /// Destructor.
  virtual ~ACE_Object_Manager_Base (void);

public:
  /**
   * Explicitly initialize.  Returns 0 on success, -1 on failure due
   * to dynamic allocation failure (in which case errno is set to
   * ENOMEM), or 1 if it had already been called.
   */
  virtual int init (void) = 0;

  /**
   * Explicitly destroy.  Returns 0 on success, -1 on failure because
   * the number of fini () calls hasn't reached the number of init ()
   * calls, or 1 if it had already been called.
   */
  virtual int fini (void) = 0;

  enum Object_Manager_State
    {
      OBJ_MAN_UNINITIALIZED = 0,
      OBJ_MAN_INITIALIZING,
      OBJ_MAN_INITIALIZED,
      OBJ_MAN_SHUTTING_DOWN,
      OBJ_MAN_SHUT_DOWN
    };

protected:
  /**
   * Returns 1 before ACE_Object_Manager_Base has been constructed.
   * This flag can be used to determine if the program is constructing
   * static objects.  If no static object spawns any threads, the
   * program will be single-threaded when this flag returns 1.  (Note
   * that the program still might construct some static objects when
   * this flag returns 0, if ACE_HAS_NONSTATIC_OBJECT_MANAGER is not
   * defined.)
   */
  int starting_up_i (void);

  /**
   * Returns 1 after ACE_Object_Manager_Base has been destroyed.  This
   * flag can be used to determine if the program is in the midst of
   * destroying static objects.  (Note that the program might destroy
   * some static objects before this flag can return 1, if
   * ACE_HAS_NONSTATIC_OBJECT_MANAGER is not defined.)
   */
  int shutting_down_i (void);

  /// State of the Object_Manager;
  Object_Manager_State object_manager_state_;

  /**
   * Flag indicating whether the ACE_Object_Manager was dynamically
   * allocated by ACE.  (If is was dynamically allocated by the
   * application, then the application is responsible for destroying
   * it.)
   */
  u_int dynamically_allocated_;

  /// Link to next Object_Manager, for chaining.
  ACE_Object_Manager_Base *next_;
private:
  // Disallow copying by not implementing the following . . .
  ACE_Object_Manager_Base (const ACE_Object_Manager_Base &);
  ACE_Object_Manager_Base &operator= (const ACE_Object_Manager_Base &);
};

extern "C"
void
ACE_OS_Object_Manager_Internal_Exit_Hook (void);


// @@ This forward declaration should go away.
class ACE_Log_Msg;

class ACE_OS_Export ACE_OS_Object_Manager : public ACE_Object_Manager_Base
{
public:
  /// Explicitly initialize.
  virtual int init (void);

  /// Explicitly destroy.
  virtual int fini (void);

  /**
   * Returns 1 before the <ACE_OS_Object_Manager> has been
   * constructed.  See <ACE_Object_Manager::starting_up> for more
   * information.
   */
  static int starting_up (void);

  /// Returns 1 after the <ACE_OS_Object_Manager> has been destroyed.
  /// See <ACE_Object_Manager::shutting_down> for more information.
  static int shutting_down (void);

  /// Unique identifiers for preallocated objects.
  enum Preallocated_Object
    {
# if defined (ACE_MT_SAFE) && (ACE_MT_SAFE != 0)
      ACE_OS_MONITOR_LOCK,
      ACE_TSS_CLEANUP_LOCK,
      ACE_LOG_MSG_INSTANCE_LOCK,
#   if defined (ACE_HAS_TSS_EMULATION)
      ACE_TSS_KEY_LOCK,
#     if defined (ACE_HAS_THREAD_SPECIFIC_STORAGE)
      ACE_TSS_BASE_LOCK,
#     endif /* ACE_HAS_THREAD_SPECIFIC_STORAGE */
#   endif /* ACE_HAS_TSS_EMULATION */
# else
      // Without ACE_MT_SAFE, There are no preallocated objects.  Make
      // sure that the preallocated_array size is at least one by
      // declaring this dummy . . .
      ACE_OS_EMPTY_PREALLOCATED_OBJECT,
# endif /* ACE_MT_SAFE */

      /// This enum value must be last!
      ACE_OS_PREALLOCATED_OBJECTS
    };

  /// Accesses a default signal set used, for example, in
  /// <ACE_Sig_Guard> methods.
  static sigset_t *default_mask (void);

  /// Returns the current thread hook for the process.
  static ACE_Thread_Hook *thread_hook (void);

  /// Returns the existing thread hook and assign a <new_thread_hook>.
  static ACE_Thread_Hook *thread_hook (ACE_Thread_Hook *new_thread_hook);

#if defined (ACE_HAS_WIN32_STRUCTURAL_EXCEPTIONS)
  /// Get/Set TSS exception action.
  static ACE_SEH_EXCEPT_HANDLER seh_except_selector (void);
  static ACE_SEH_EXCEPT_HANDLER seh_except_selector (ACE_SEH_EXCEPT_HANDLER);

  static ACE_SEH_EXCEPT_HANDLER seh_except_handler (void);
  static ACE_SEH_EXCEPT_HANDLER seh_except_handler (ACE_SEH_EXCEPT_HANDLER);
#endif /* ACE_HAS_WIN32_STRUCTURAL_EXCEPTIONS */

public:
  // = Applications shouldn't use these so they're hidden here.

  // They're public so that the <ACE_Object_Manager> can be
  // constructed/destructed in <main> with
  // <ACE_HAS_NONSTATIC_OBJECT_MANAGER>.
  /// Constructor.
  ACE_OS_Object_Manager (void);

  /// Destructor.
  ~ACE_OS_Object_Manager (void);

private:
  /// Accessor to singleton instance.
  static ACE_OS_Object_Manager *instance (void);

  /// Singleton instance pointer.
  static ACE_OS_Object_Manager *instance_;

  /// Table of preallocated objects.
  static void *preallocated_object[ACE_OS_PREALLOCATED_OBJECTS];

  /// Default signal set used, for example, in ACE_Sig_Guard.
  sigset_t *default_mask_;

  /// Thread hook that's used by this process.
  ACE_Thread_Hook *thread_hook_;

  /// For at_exit support.
  ACE_OS_Exit_Info exit_info_;

#if defined (ACE_HAS_WIN32_STRUCTURAL_EXCEPTIONS)
  /// These handlers determine how a thread handles win32 structured
  /// exception.
  ACE_SEH_EXCEPT_HANDLER seh_except_selector_;
  ACE_SEH_EXCEPT_HANDLER seh_except_handler_;
#endif /* ACE_HAS_WIN32_STRUCTURAL_EXCEPTIONS */

  /// For <ACE_OS::atexit> support.
  int at_exit (ACE_EXIT_HOOK func);

  /// For use by init () and fini (), to consolidate error reporting.
  static void print_error_message (u_int line_number, const ACE_TCHAR *message);

  /// This class is for internal use by ACE_OS, etc., only.
  friend class ACE_OS;
  friend class ACE_Object_Manager;
  friend class ACE_OS_Object_Manager_Manager;
  friend class ACE_TSS_Cleanup;
  friend class ACE_TSS_Emulation;
  friend class ACE_Log_Msg;
  friend void ACE_OS_Object_Manager_Internal_Exit_Hook ();
};

# if defined (ACE_LACKS_TIMEDWAIT_PROTOTYPES)
extern "C" ssize_t recv_timedwait (ACE_HANDLE handle,
                                   char *buf,
                                   int len,
                                   int flags,
                                   struct timespec *timeout);
extern "C" ssize_t read_timedwait (ACE_HANDLE handle,
                                   char *buf,
                                   size_t n,
                                   struct timespec *timeout);
extern "C" ssize_t recvmsg_timedwait (ACE_HANDLE handle,
                                      struct msghdr *msg,
                                      int flags,
                                      struct timespec *timeout);
extern "C" ssize_t recvfrom_timedwait (ACE_HANDLE handle,
                                       char *buf,
                                       int len,
                                       int flags,
                                       struct sockaddr *addr,
                                       int
                                       *addrlen,
                                       struct timespec *timeout);
extern "C" ssize_t readv_timedwait (ACE_HANDLE handle,
                                    iovec *iov,
                                    int iovcnt,
                                    struct timespec* timeout);
extern "C" ssize_t send_timedwait (ACE_HANDLE handle,
                                   const char *buf,
                                   int len,
                                   int flags,
                                   struct timespec *timeout);
extern "C" ssize_t write_timedwait (ACE_HANDLE handle,
                                    const void *buf,
                                    size_t n,
                                    struct timespec *timeout);
extern "C" ssize_t sendmsg_timedwait (ACE_HANDLE handle,
                                      ACE_SENDMSG_TYPE *msg,
                                      int flags,
                                      struct timespec *timeout);
extern "C" ssize_t sendto_timedwait (ACE_HANDLE handle,
                                     const char *buf,
                                     int len,
                                     int flags,
                                     const struct sockaddr *addr,
                                     int addrlen,
                                     struct timespec *timeout);
extern "C" ssize_t writev_timedwait (ACE_HANDLE handle,
                                     ACE_WRITEV_TYPE *iov,
                                     int iovcnt,
                                     struct timespec *timeout);
# endif /* ACE_LACKS_TIMEDWAIT_PROTOTYPES */

# if defined (ACE_HAS_TSS_EMULATION)
    // Allow config.h to set the default number of thread keys.
#   if !defined (ACE_DEFAULT_THREAD_KEYS)
#     define ACE_DEFAULT_THREAD_KEYS 64
#   endif /* ! ACE_DEFAULT_THREAD_KEYS */

// forward declaration
class ACE_TSS_Keys;

/**
 * @class ACE_TSS_Emulation
 *
 * @brief Thread-specific storage emulation.
 *
 * This provides a thread-specific storage implementation.
 * It is intended for use on platforms that don't have a
 * native TSS, or have a TSS with limitations such as the
 * number of keys or lack of support for removing keys.
 */
class ACE_OS_Export ACE_TSS_Emulation
{
public:
  typedef void (*ACE_TSS_DESTRUCTOR)(void *value) /* throw () */;

  /// Maximum number of TSS keys allowed over the life of the program.
  enum { ACE_TSS_THREAD_KEYS_MAX = ACE_DEFAULT_THREAD_KEYS };

  /// Returns the total number of keys allocated so far.
  static u_int total_keys ();

  /// Sets the argument to the next available key.  Returns 0 on success,
  /// -1 if no keys are available.
  static int next_key (ACE_thread_key_t &key);

  /// Release a key that was used. This way the key can be given out in a
  /// new request. Returns 0 on success, 1 if the key was not reserved.
  static int release_key (ACE_thread_key_t key);

  /// Returns the exit hook associated with the key.  Does _not_ check
  /// for a valid key.
  static ACE_TSS_DESTRUCTOR tss_destructor (const ACE_thread_key_t key);

  /// Associates the TSS destructor with the key.  Does _not_ check
  /// for a valid key.
  static void tss_destructor (const ACE_thread_key_t key,
                              ACE_TSS_DESTRUCTOR destructor);

  /// Accesses the object referenced by key in the current thread's TSS array.
  /// Does _not_ check for a valid key.
  static void *&ts_object (const ACE_thread_key_t key);

  /**
   * Setup an array to be used for local TSS.  Returns the array
   * address on success.  Returns 0 if local TSS had already been
   * setup for this thread.  There is no corresponding tss_close ()
   * because it is not needed.
   * NOTE: tss_open () is called by ACE for threads that it spawns.
   * If your application spawns threads without using ACE, and it uses
   * ACE's TSS emulation, each of those threads should call tss_open
   * ().  See the ace_thread_adapter () implementation for an example.
   */
  static void *tss_open (void *ts_storage[ACE_TSS_THREAD_KEYS_MAX]);

  /// Shutdown TSS emulation.  For use only by ACE_OS::cleanup_tss ().
  static void tss_close ();

private:
  // Global TSS structures.
  /// Contains the possible value of the next key to be allocated. Which key
  /// is actually allocated is based on the tss_keys_used
  static u_int total_keys_;

  /// Array of thread exit hooks (TSS destructors) that are called for each
  /// key (that has one) when the thread exits.
  static ACE_TSS_DESTRUCTOR tss_destructor_ [ACE_TSS_THREAD_KEYS_MAX];

  /// TSS_Keys instance to administrate whether a specific key is in used
  /// or not
  static ACE_TSS_Keys tss_keys_used_;

#   if defined (ACE_HAS_THREAD_SPECIFIC_STORAGE)
  /// Location of current thread's TSS array.
  static void **tss_base (void* ts_storage[] = 0, u_int *ts_created = 0);
#   else  /* ! ACE_HAS_THREAD_SPECIFIC_STORAGE */
  /// Location of current thread's TSS array.
  static void **&tss_base ();
#   endif /* ! ACE_HAS_THREAD_SPECIFIC_STORAGE */

#   if defined (ACE_HAS_THREAD_SPECIFIC_STORAGE)
  // Rely on native thread specific storage for the implementation,
  // but just use one key.
  static ACE_OS_thread_key_t native_tss_key_;

  // Used to indicate if native tss key has been allocated
  static int key_created_;
#   endif /* ACE_HAS_THREAD_SPECIFIC_STORAGE */
};

# else   /* ! ACE_HAS_TSS_EMULATION */
#   if defined (TLS_MINIMUM_AVAILABLE)
    // WIN32 platforms define TLS_MINIMUM_AVAILABLE natively.
#     define ACE_DEFAULT_THREAD_KEYS TLS_MINIMUM_AVAILABLE
#   endif /* TSL_MINIMUM_AVAILABLE */

# endif /* ACE_HAS_TSS_EMULATION */

// moved ACE_TSS_Ref, ACE_TSS_Info, and ACE_TSS_Keys class
// declarations from OS.cpp so they are visible to the single
// file of template instantiations.
# if defined (ACE_WIN32) || defined (ACE_HAS_TSS_EMULATION) || (defined (ACE_PSOS) && defined (ACE_PSOS_HAS_TSS))
/**
 * @class ACE_TSS_Ref
 *
 * @brief "Reference count" for thread-specific storage keys.
 *
 * Since the <ACE_Unbounded_Stack> doesn't allow duplicates, the
 * "reference count" is the identify of the thread_id.
 */
class ACE_TSS_Ref
{
public:
  /// Constructor
  ACE_TSS_Ref (ACE_thread_t id);

  /// Default constructor
  ACE_TSS_Ref (void);

  /// Check for equality.
  int operator== (const ACE_TSS_Ref &) const;

  /// Check for inequality.
  int operator!= (const ACE_TSS_Ref &) const;

// private:

  /// ID of thread using a specific key.
  ACE_thread_t tid_;
};

/**
 * @class ACE_TSS_Info
 *
 * @brief Thread Specific Key management.
 *
 * This class maps a key to a "destructor."
 */
class ACE_TSS_Info
{
public:
  /// Constructor
  ACE_TSS_Info (ACE_thread_key_t key,
                void (*dest)(void *) = 0,
                void *tss_inst = 0);

  /// Default constructor
  ACE_TSS_Info (void);

  /// Returns 1 if the key is in use, 0 if not.
  int key_in_use (void) const { return thread_count_ != -1; }

  /// Mark the key as being in use if the flag is non-zero, or
  /// not in use if the flag is 0.
  void key_in_use (int flag) { thread_count_ = flag == 0  ?  -1  :  1; }

  /// Check for equality.
  int operator== (const ACE_TSS_Info &) const;

  /// Check for inequality.
  int operator!= (const ACE_TSS_Info &) const;

  /// Dump the state.
  void dump (void);

private:
  /// Key to the thread-specific storage item.
  ACE_thread_key_t key_;

  /// "Destructor" that gets called when the item is finally released.
  void (*destructor_)(void *);

  /// Pointer to ACE_TSS<xxx> instance that has/will allocate the key.
  void *tss_obj_;

  /// Count of threads that are using this key.  Contains -1 when the
  /// key is not in use.
  int thread_count_;

  friend class ACE_TSS_Cleanup;
};

/**
 * @class ACE_TSS_Keys
 *
 * @brief Collection of in-use flags for a thread's TSS keys.
 * For internal use only by ACE_TSS_Cleanup; it is public because
 * some compilers can't use nested classes for template instantiation
 * parameters.
 *
 * Wrapper around array of whether each key is in use.  A simple
 * typedef doesn't work with Sun C++ 4.2.
 */
class ACE_TSS_Keys
{
public:
  /// Default constructor, to initialize all bits to zero (unused).
  ACE_TSS_Keys (void);

  /// Mark the specified key as being in use, if it was not already so marked.
  /// Returns 1 if the had already been marked, 0 if not.
  int test_and_set (const ACE_thread_key_t key);

  /// Mark the specified key as not being in use, if it was not already so
  /// cleared.  Returns 1 if the key had already been cleared, 0 if not.
  int test_and_clear (const ACE_thread_key_t key);

  /// Return whether the specific key is marked as in use.
  /// Returns 1 if the key is been marked, 0 if not.
  int is_set (const ACE_thread_key_t key) const;

private:
  /// For a given key, find the word and bit number that represent it.
  static void find (const u_int key, u_int &word, u_int &bit);

  enum
    {
#   if ACE_SIZEOF_LONG == 8
      ACE_BITS_PER_WORD = 64,
#   elif ACE_SIZEOF_LONG == 4
      ACE_BITS_PER_WORD = 32,
#   else
#     error ACE_TSS_Keys only supports 32 or 64 bit longs.
#   endif /* ACE_SIZEOF_LONG == 8 */
      ACE_WORDS = (ACE_DEFAULT_THREAD_KEYS - 1) / ACE_BITS_PER_WORD + 1
    };

  /// Bit flag collection.  A bit value of 1 indicates that the key is in
  /// use by this thread.
  u_long key_bit_words_[ACE_WORDS];
};

# endif /* defined (ACE_WIN32) || defined (ACE_HAS_TSS_EMULATION) */

// Support non-scalar thread keys, such as with some POSIX
// implementations, e.g., MVS.
# if defined (ACE_HAS_NONSCALAR_THREAD_KEY_T)
#   define ACE_KEY_INDEX(OBJ,KEY) \
  u_int OBJ; \
  ACE_OS::memcpy (&OBJ, &KEY, sizeof (u_int))
# else
#   define ACE_KEY_INDEX(OBJ,KEY) u_int OBJ = KEY
# endif /* ACE_HAS_NONSCALAR_THREAD_KEY_T */

// Some useful abstrations for expressions involving
// ACE_Allocator.malloc ().  The difference between ACE_NEW_MALLOC*
// with ACE_ALLOCATOR* is that they call constructors also.

# define ACE_ALLOCATOR_RETURN(POINTER,ALLOCATOR,RET_VAL) \
   do { POINTER = ALLOCATOR; \
     if (POINTER == 0) { errno = ENOMEM; return RET_VAL; } \
   } while (0)
# define ACE_ALLOCATOR(POINTER,ALLOCATOR) \
   do { POINTER = ALLOCATOR; \
     if (POINTER == 0) { errno = ENOMEM; return; } \
   } while (0)
# define ACE_ALLOCATOR_NORETURN(POINTER,ALLOCATOR) \
   do { POINTER = ALLOCATOR; \
     if (POINTER == 0) { errno = ENOMEM; } \
   } while (0)

# define ACE_NEW_MALLOC_RETURN(POINTER,ALLOCATOR,CONSTRUCTOR,RET_VAL) \
   do { POINTER = ALLOCATOR; \
     if (POINTER == 0) { errno = ENOMEM; return RET_VAL;} \
     else { new (POINTER) CONSTRUCTOR; } \
   } while (0)
# define ACE_NEW_MALLOC(POINTER,ALLOCATOR,CONSTRUCTOR) \
   do { POINTER = ALLOCATOR; \
     if (POINTER == 0) { errno = ENOMEM; return;} \
     else { new (POINTER) CONSTRUCTOR; } \
   } while (0)
# define ACE_NEW_MALLOC_NORETURN(POINTER,ALLOCATOR,CONSTRUCTOR) \
   do { POINTER = ALLOCATOR; \
     if (POINTER == 0) { errno = ENOMEM;} \
     else { new (POINTER) CONSTRUCTOR; } \
   } while (0)

# define ACE_NOOP(x)

# if defined (ACE_HAS_THR_C_FUNC)
// This is necessary to work around nasty problems with MVS C++.
extern "C" ACE_OS_Export void ace_mutex_lock_cleanup_adapter (void *args);
#   define ACE_PTHREAD_CLEANUP_PUSH(A) pthread_cleanup_push (ace_mutex_lock_cleanup_adapter, (void *) A);
#   define ACE_PTHREAD_CLEANUP_POP(A) pthread_cleanup_pop(A)
# elif defined (ACE_HAS_PTHREADS) && !defined (ACE_LACKS_PTHREAD_CLEANUP)
// Though we are defining a extern "C" function to match the prototype of
// pthread_cleanup_push, it is undone by the Solaris header file
// /usr/include/pthread.h. So this macro generates a warning under Solaris
// with SunCC. This is a bug in the Solaris header file.
extern "C" ACE_OS_Export void ace_mutex_lock_cleanup_adapter (void *args);
#   define ACE_PTHREAD_CLEANUP_PUSH(A) pthread_cleanup_push (ace_mutex_lock_cleanup_adapter, (void *) A);
#   define ACE_PTHREAD_CLEANUP_POP(A) pthread_cleanup_pop(A)
# else
#   define ACE_PTHREAD_CLEANUP_PUSH(A)
#   define ACE_PTHREAD_CLEANUP_POP(A)
# endif /* ACE_HAS_THR_C_FUNC */

# if !defined (ACE_DEFAULT_MUTEX_A)
#   define ACE_DEFAULT_MUTEX_A "ACE_MUTEX"
# endif /* ACE_DEFAULT_MUTEX_A */

# if defined (ACE_HAS_WCHAR)
#   define ACE_DEFAULT_MUTEX_W ACE_TEXT_WIDE(ACE_DEFAULT_MUTEX_A)
# endif /* ACE_HAS_WCHAR */

# define ACE_DEFAULT_MUTEX ACE_LIB_TEXT (ACE_DEFAULT_MUTEX_A)

# if !defined (ACE_MAIN)
#   define ACE_MAIN main
# endif /* ! ACE_MAIN */

# if !defined (ACE_WMAIN)
#   define ACE_WMAIN wmain
# endif /* ! ACE_WMAIN */

# if defined (ACE_WIN32) && defined (ACE_USES_WCHAR)
#   define ACE_TMAIN wmain
# else
#   define ACE_TMAIN main
# endif

# if defined (ACE_DOESNT_INSTANTIATE_NONSTATIC_OBJECT_MANAGER)
#   if !defined (ACE_HAS_NONSTATIC_OBJECT_MANAGER)
#     define ACE_HAS_NONSTATIC_OBJECT_MANAGER
#   endif /* ACE_HAS_NONSTATIC_OBJECT_MANAGER */
# endif /* ACE_DOESNT_INSTANTIATE_NONSTATIC_OBJECT_MANAGER */

# if defined (ACE_HAS_NONSTATIC_OBJECT_MANAGER) && !defined (ACE_DOESNT_INSTANTIATE_NONSTATIC_OBJECT_MANAGER)

#   if !defined (ACE_HAS_MINIMAL_ACE_OS)
#     include "ace/Object_Manager.h"
#   endif /* ! ACE_HAS_MINIMAL_ACE_OS */

// Rename "main ()" on platforms that don't allow it to be called "main ()".

// Also, create ACE_Object_Manager static instance(s) in "main ()".
// ACE_MAIN_OBJECT_MANAGER defines the ACE_Object_Manager(s) that will
// be instantiated on the stack of main ().  Note that it is only used
// when compiling main ():  its value does not affect the contents of
// ace/OS.o.
#   if !defined (ACE_MAIN_OBJECT_MANAGER)
#     define ACE_MAIN_OBJECT_MANAGER \
        ACE_OS_Object_Manager ace_os_object_manager; \
        ACE_Object_Manager ace_object_manager;
#   endif /* ! ACE_MAIN_OBJECT_MANAGER */

#   if defined (ACE_PSOSIM)
// PSOSIM root lacks the standard argc, argv command line parameters,
// create dummy argc and argv in the "real" main  and pass to "user" main.
// NOTE: ACE_MAIN must be defined to give the return type as well as the
// name of the entry point.
#     define main \
ace_main_i (int, char *[]);                /* forward declaration */ \
ACE_MAIN ()   /* user's entry point, e.g., "main" w/out argc, argv */ \
{ \
  int argc = 1;                            /* dummy arg count */ \
  char *argv[] = {"psosim"};               /* dummy arg list */ \
  ACE_MAIN_OBJECT_MANAGER \
  int ret_val = -1; /* assume the worst */ \
  if (ACE_PSOS_Time_t::init_simulator_time ()) /* init simulator time */ \
  { \
    ACE_ERROR((LM_ERROR, "init_simulator_time failed\n"));  /* report */ \
  } \
  else \
  { \
    ret_val = ace_main_i (argc, argv);   /* call user main, save result */ \
  } \
  ACE_OS::exit (ret_val);                /* pass code to simulator exit */ \
} \
int \
ace_main_i
#   elif defined (ACE_PSOS) && defined (ACE_PSOS_LACKS_ARGC_ARGV)
// PSOS root lacks the standard argc, argv command line parameters,
// create dummy argc and argv in the "real" main  and pass to "user" main.
// Ignore return value from user main as well.  NOTE: ACE_MAIN must be
// defined to give the return type as well as the name of the entry point
#     define main \
ace_main_i (int, char *[]);               /* forward declaration */ \
ACE_MAIN ()   /* user's entry point, e.g., "main" w/out argc, argv */ \
{ \
  int argc = 1;                           /* dummy arg count */ \
  char *argv[] = {"root"};                /* dummy arg list */ \
  ACE_MAIN_OBJECT_MANAGER \
  ace_main_i (argc, argv);                /* call user main, ignore result */ \
} \
int \
ace_main_i
#   elif defined (ACE_HAS_WINCE)
/**
 * @class ACE_CE_ARGV
 *
 * @brief This class is to hash input parameters, argc and argv, for WinCE platform.
 *
 * Since WinCE only supports wchar_t as an input from OS, some implementation detail,
 * especially for CORBA spec, will not support ACE_TCHAR (wchar_t) type parameter.
 * Moreover, WinCE's input parameter type is totally different than any other OS;
 * all command line parameters will be stored in a single wide-character string with
 * each unit parameter divided by blank space, and it does not provide the name of
 * executable (generally known as argv[0]).
 * This class is to convert CE's command line parameters and simulate as in the same
 * manner as other general platforms, adding 'root' as a first argc, which is for the
 * name of executable in other OS.
 */
class ACE_OS_Export ACE_CE_ARGV
{
public:
    /**
     * Ctor accepts CE command line as a paramter.
     */
    ACE_CE_ARGV(ACE_TCHAR* cmdLine);

    /**
     * Default Dtor that deletes any memory allocated for the converted string.
     */
    ~ACE_CE_ARGV(void);

    /**
     * Returns the number of command line paramters, same as argc on Unix.
     */
    int argc(void);

    /**
     * Returns the 'char**' that contains the converted command line parameters.
     */
    ACE_TCHAR** const argv(void);

private:
    /**
     * Copy Ctor is not allowed.
     */
    ACE_CE_ARGV(void);

    /**
     * Copy Ctor is not allowed.
     */
    ACE_CE_ARGV(ACE_CE_ARGV&);

    /**
     * Pointer of converted command line paramters.
     */
    ACE_TCHAR** ce_argv_;

    /**
     * Integer that is same as argc on other OS's.
     */
    int ce_argc_;
};
#     if defined (ACE_TMAIN)  // Use WinMain on CE; others give warning/error.
#       undef ACE_TMAIN
#     endif  // ACE_TMAIN

// Support for ACE_TMAIN, which is a recommended way.
#     define ACE_TMAIN \
ace_main_i (int, ACE_TCHAR *[]);  /* forward declaration */ \
int WINAPI WinMain (HINSTANCE hInstance, HINSTANCE hPrevInstance, LPWSTR lpCmdLine, int nCmdShow) \
{ \
  ACE_CE_ARGV ce_argv(lpCmdLine); \
  ACE::init(); \
  ACE_MAIN_OBJECT_MANAGER \
  int i = ace_main_i (ce_argv.argc(), ce_argv.argv()); \
  ACE::fini(); \
  return i; \
} \
int ace_main_i

// Support for wchar_t but still can't fit to CE because of the command line parameters.
#     define wmain \
ace_main_i (int, ACE_TCHAR *[]);  /* forward declaration */ \
int WINAPI WinMain (HINSTANCE hInstance, HINSTANCE hPrevInstance, LPWSTR lpCmdLine, int nCmdShow) \
{ \
  ACE_CE_ARGV ce_argv(lpCmdLine); \
  ACE::init(); \
  ACE_MAIN_OBJECT_MANAGER \
  int i = ace_main_i (ce_argv.argc(), ce_argv.argv()); \
  ACE::fini(); \
  return i; \
} \
int ace_main_i

// Supporting legacy 'main' is A LOT easier for users than changing existing code on WinCE.
#     define main \
ace_main_i (int, char *[]);  /* forward declaration */ \
#include <ace/Argv_Type_Converter.h> \
int WINAPI WinMain (HINSTANCE hInstance, HINSTANCE hPrevInstance, LPWSTR lpCmdLine, int nCmdShow) \
{ \
  ACE_CE_ARGV ce_argv(lpCmdLine); \
  ACE::init(); \
  ACE_MAIN_OBJECT_MANAGER \
  ACE_Argv_Type_Converter command_line(ce_argv.argc(), ce_argv.argv()); \
  int i = ace_main_i (command_line.get_argc(), command_line.get_ASCII_argv()); \
  ACE::fini(); \
  return i; \
} \
int ace_main_i

#   else
#     define main \
ace_main_i (int, char *[]);                  /* forward declaration */ \
int \
ACE_MAIN (int argc, char *argv[]) /* user's entry point, e.g., main */ \
{ \
  ACE_MAIN_OBJECT_MANAGER \
  return ace_main_i (argc, argv);           /* what the user calls "main" */ \
} \
int \
ace_main_i
#     if defined (ACE_WIN32)
#     define wmain \
ace_main_i (int, ACE_TCHAR *[]);                  /* forward declaration */ \
int \
ACE_WMAIN (int argc, ACE_TCHAR *argv[]) /* user's entry point, e.g., main */ \
{ \
  ACE_MAIN_OBJECT_MANAGER \
  return ace_main_i (argc, argv);           /* what the user calls "main" */ \
} \
int \
ace_main_i
#     endif /* ACE_WIN32 && UNICODE */
#   endif   /* ACE_PSOSIM */
# endif /* ACE_HAS_NONSTATIC_OBJECT_MANAGER && !ACE_HAS_WINCE && !ACE_DOESNT_INSTANTIATE_NONSTATIC_OBJECT_MANAGER */

# if defined (ACE_WIN32) && ! defined (ACE_HAS_WINCE) \
                         && ! defined (ACE_HAS_PHARLAP)
typedef TRANSMIT_FILE_BUFFERS ACE_TRANSMIT_FILE_BUFFERS;
typedef LPTRANSMIT_FILE_BUFFERS ACE_LPTRANSMIT_FILE_BUFFERS;
typedef PTRANSMIT_FILE_BUFFERS ACE_PTRANSMIT_FILE_BUFFERS;

#   define ACE_INFINITE INFINITE
#   define ACE_STATUS_TIMEOUT STATUS_TIMEOUT
#   define ACE_WAIT_FAILED WAIT_FAILED
#   define ACE_WAIT_TIMEOUT WAIT_TIMEOUT
# else /* ACE_WIN32 */
struct ACE_TRANSMIT_FILE_BUFFERS
{
  void *Head;
  size_t HeadLength;
  void *Tail;
  size_t TailLength;
};
typedef ACE_TRANSMIT_FILE_BUFFERS* ACE_PTRANSMIT_FILE_BUFFERS;
typedef ACE_TRANSMIT_FILE_BUFFERS* ACE_LPTRANSMIT_FILE_BUFFERS;

#   if !defined (ACE_INFINITE)
#     define ACE_INFINITE LONG_MAX
#   endif /* ACE_INFINITE */
#   define ACE_STATUS_TIMEOUT LONG_MAX
#   define ACE_WAIT_FAILED LONG_MAX
#   define ACE_WAIT_TIMEOUT LONG_MAX
# endif /* ACE_WIN32 */

# if !defined (ACE_HAS_MINIMAL_ACE_OS)
#   include "ace/Trace.h"
# endif /* ! ACE_HAS_MINIMAL_ACE_OS */

# if defined (ACE_HAS_INLINED_OSCALLS)
#   if defined (ACE_INLINE)
#     undef ACE_INLINE
#   endif /* ACE_INLINE */
#   define ACE_INLINE inline
#   include "ace/OS.i"
# endif /* ACE_HAS_INLINED_OSCALLS */

// Byte swapping macros to deal with differences between little endian
// and big endian machines.  Note that "long" here refers to 32 bit
// quantities.
# define ACE_SWAP_LONG(L) ((ACE_SWAP_WORD ((L) & 0xFFFF) << 16) \
            | ACE_SWAP_WORD(((L) >> 16) & 0xFFFF))
# define ACE_SWAP_WORD(L) ((((L) & 0x00FF) << 8) | (((L) & 0xFF00) >> 8))

# if defined (ACE_LITTLE_ENDIAN)
#   define ACE_HTONL(X) ACE_SWAP_LONG (X)
#   define ACE_NTOHL(X) ACE_SWAP_LONG (X)
#   define ACE_IDL_NCTOHL(X) (X)
#   define ACE_IDL_NSTOHL(X) (X)
# else
#   define ACE_HTONL(X) X
#   define ACE_NTOHL(X) X
#   define ACE_IDL_NCTOHL(X) (X << 24)
#   define ACE_IDL_NSTOHL(X) ((X) << 16)
# endif /* ACE_LITTLE_ENDIAN */

# if defined (ACE_LITTLE_ENDIAN)
#   define ACE_HTONS(x) ACE_SWAP_WORD(x)
#   define ACE_NTOHS(x) ACE_SWAP_WORD(x)
# else
#   define ACE_HTONS(x) x
#   define ACE_NTOHS(x) x
# endif /* ACE_LITTLE_ENDIAN */

# if defined (ACE_HAS_AIO_CALLS)
  // = Giving unique ACE scoped names for some important
  // RTSignal-Related constants. Becuase sometimes, different
  // platforms use different names for these constants.

  // Number of realtime signals provided in the system.
  // _POSIX_RTSIG_MAX is the upper limit on the number of real time
  // signals supported in a posix-4 compliant system.
#   if defined (_POSIX_RTSIG_MAX)
#     define ACE_RTSIG_MAX _POSIX_RTSIG_MAX
#   else /* not _POSIX_RTSIG_MAX */
  // POSIX-4 compilant system has to provide atleast 8 RT signals.
  // @@ Make sure the platform does *not* define this constant with
  // some other name. If yes, use that instead of 8.
#     define ACE_RTSIG_MAX 8
#   endif /* _POSIX_RTSIG_MAX */
# endif /* ACE_HAS_AIO_CALLS */

  // Wrapping around wait status <wstat> macros for platforms that
  // lack them.

  // Evaluates to a non-zero value if status was returned for a child
  // process that terminated normally.  0 means status wasn't
  // returned.
#if !defined (WIFEXITED)
#   define WIFEXITED(stat) 1
#endif /* WIFEXITED */

  // If the value of WIFEXITED(stat) is non-zero, this macro evaluates
  // to the exit code that the child process exit(3C), or the value
  // that the child process returned from main.  Peaceful exit code is
  // 0.
#if !defined (WEXITSTATUS)
#   define WEXITSTATUS(stat) stat
#endif /* WEXITSTATUS */

  // Evaluates to a non-zero value if status was returned for a child
  // process that terminated due to the receipt of a signal.  0 means
  // status wasnt returned.
#if !defined (WIFSIGNALED)
#   define WIFSIGNALED(stat) 0
#endif /* WIFSIGNALED */

  // If the value of  WIFSIGNALED(stat)  is non-zero,  this macro
  // evaluates to the number of the signal that  caused  the
  // termination of the child process.
#if !defined (WTERMSIG)
#   define WTERMSIG(stat) 0
#endif /* WTERMSIG */

#if !defined (WIFSTOPPED)
#   define WIFSTOPPED(stat) 0
#endif /* WIFSTOPPED */

#if !defined (WSTOPSIG)
#   define WSTOPSIG(stat) 0
#endif /* WSTOPSIG */

#if !defined (WIFCONTINUED)
#   define WIFCONTINUED(stat) 0
#endif /* WIFCONTINUED */

#if !defined (WCOREDUMP)
#   define WCOREDUMP(stat) 0
#endif /* WCOREDUMP */

// Stuff used by the ACE CDR classes.
#if defined ACE_LITTLE_ENDIAN
#  define ACE_CDR_BYTE_ORDER 1
// little endian encapsulation byte order has value = 1
#else  /* ! ACE_LITTLE_ENDIAN */
#  define ACE_CDR_BYTE_ORDER 0
// big endian encapsulation byte order has value = 0
#endif /* ! ACE_LITTLE_ENDIAN */

/**
 * @name Default values to control CDR classes memory allocation strategies
 */
//@{

/// Control the initial size of all CDR buffers, application
/// developers may want to optimize this value to fit their request
/// size
#if !defined (ACE_DEFAULT_CDR_BUFSIZE)
#  define ACE_DEFAULT_CDR_BUFSIZE 512
#endif /* ACE_DEFAULT_CDR_BUFSIZE */

/// Stop exponential growth of CDR buffers to avoid overallocation
#if !defined (ACE_DEFAULT_CDR_EXP_GROWTH_MAX)
#  define ACE_DEFAULT_CDR_EXP_GROWTH_MAX 65536
#endif /* ACE_DEFAULT_CDR_EXP_GROWTH_MAX */

/// Control CDR buffer growth after maximum exponential growth is
/// reached
#if !defined (ACE_DEFAULT_CDR_LINEAR_GROWTH_CHUNK)
#  define ACE_DEFAULT_CDR_LINEAR_GROWTH_CHUNK 65536
#endif /* ACE_DEFAULT_CDR_LINEAR_GROWTH_CHUNK */
//@}

/// Control the zero-copy optimizations for octet sequences
/**
 * Large octet sequences can be sent without any copies by chaining
 * them in the list of message blocks that represent a single CDR
 * stream.  However, if the octet sequence is too small the zero copy
 * optimizations actually hurt performance.  Octet sequences smaller
 * than this value will be copied.
 */
#if !defined (ACE_DEFAULT_CDR_MEMCPY_TRADEOFF)
#define ACE_DEFAULT_CDR_MEMCPY_TRADEOFF 256
#endif /* ACE_DEFAULT_CDR_MEMCPY_TRADEOFF */

/**
 * In some environments it is useful to swap the bytes on write, for
 * instance: a fast server can be feeding a lot of slow clients that
 * happen to have the wrong byte order.
 * Because this is a rarely used feature we disable it by default to
 * minimize footprint.
 * This macro enables the functionality, but we still need a way to
 * activate it on a per-connection basis.
 */
// #define ACE_ENABLE_SWAP_ON_WRITE

/**
 * In some environements we never need to swap bytes when reading, for
 * instance embebbed systems (such as avionics) or homogenous
 * networks.
 * Setting this macro disables the capabilities to demarshall streams
 * in the wrong byte order.
 */
// #define ACE_DISABLE_SWAP_ON_READ


//@{
/**
 * @name Efficiently compute aligned pointers to powers of 2 boundaries.
 */

/**
 * Efficiently align "value" up to "alignment", knowing that all such
 * boundaries are binary powers and that we're using two's complement
 * arithmetic.
 *
 * Since the alignment is a power of two its binary representation is:
 *
 * alignment      = 0...010...0
 *
 * hence
 *
 * alignment - 1  = 0...001...1 = T1
 *
 * so the complement is:
 *
 * ~(alignment - 1) = 1...110...0 = T2
 *
 * Notice that there is a multiple of <alignment> in the range
 * [<value>,<value> + T1], also notice that if
 *
 * X = ( <value> + T1 ) & T2
 *
 * then
 *
 * <value> <= X <= <value> + T1
 *
 * because the & operator only changes the last bits, and since X is a
 * multiple of <alignment> (its last bits are zero) we have found the
 * multiple we wanted.
 */
/// Return the next integer aligned to a required boundary
/**
 * @param ptr the base pointer
 * @param alignment the required alignment
 */
#define ACE_align_binary(ptr, alignment) \
    ((ptr + ((ptr_arith_t)((alignment)-1))) & (~((ptrdiff_t)((alignment)-1))))

/// Return the next address aligned to a required boundary
#define ACE_ptr_align_binary(ptr, alignment) \
        ((char *) ACE_align_binary (((ptrdiff_t) (ptr)), (alignment)))
//@}

// Defining POSIX4 real-time signal range.
#if defined ACE_HAS_AIO_CALLS
#define ACE_SIGRTMIN SIGRTMIN
#define ACE_SIGRTMAX SIGRTMAX
#else /* !ACE_HAS_AIO_CALLS */
#define ACE_SIGRTMIN 0
#define ACE_SIGRTMAX 0
#endif /* ACE_HAS_AIO_CALLS */

# if defined (ACE_LACKS_SYS_NERR)
extern ACE_OS_Export int sys_nerr;
# endif /* ACE_LACKS_SYS_NERR */

#if defined (ACE_LEGACY_MODE)
# include "ace/Log_Msg.h"
# include "ace/Thread_Hook.h"
# include "ace/Thread_Adapter.h"
# include "ace/Thread_Exit.h"
# include "ace/Thread_Control.h"
#endif  /* ACE_LEGACY_MODE */

#include "ace/post.h"
#endif  /* ACE_OS_H */
