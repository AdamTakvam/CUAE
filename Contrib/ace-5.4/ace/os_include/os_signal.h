// -*- C++ -*-

//=============================================================================
/**
 *  @file    os_signal.h
 *
 *  signals
 *
 *  os_signal.h,v 1.10 2003/12/09 15:25:41 elliott_c Exp
 *
 *  @author Don Hinton <dhinton@dresystems.com>
 *  @author This code was originally in various places including ace/OS.h.
 */
//=============================================================================

#ifndef ACE_OS_INCLUDE_OS_SIGNAL_H
#define ACE_OS_INCLUDE_OS_SIGNAL_H

#include /**/ "ace/pre.h"

#include "ace/config-all.h"

#if !defined (ACE_LACKS_PRAGMA_ONCE)
# pragma once
#endif /* ACE_LACKS_PRAGMA_ONCE */

#include "ace/os_include/sys/os_types.h"
#include "ace/os_include/os_time.h"

#if !defined (ACE_LACKS_SIGNAL_H)
   extern "C" {
#  include /**/ <signal.h>
   }
#endif /* !ACE_LACKS_SIGNAL_H */

// This must come after signal.h is #included.
#if defined (SCO)
#  define SIGIO SIGPOLL
#  include /**/ <sys/regset.h>
#endif /* SCO */

#if defined (ACE_HAS_SIGINFO_T)
#  if !defined (ACE_LACKS_SIGINFO_H)
#    if defined (__QNX__) || defined (__OpenBSD__)
#      include /**/ <sys/siginfo.h>
#    else  /* __QNX__ || __OpenBSD__ */
#      include /**/ <siginfo.h>
#    endif /* __QNX__ || __OpenBSD__ */
#  endif /* ACE_LACKS_SIGINFO_H */
#endif /* ACE_HAS_SIGINFO_T */

#if defined (VXWORKS)
#  include /**/ <sigLib.h>
#endif /* VXWORKS */

// should this be extern "C" {}?
#if defined (CHORUS)
#     if  !defined(CHORUS_4)
typedef void (*__sighandler_t)(int); // keep Signal compilation happy
#     endif
#endif /* CHORUS */

// Place all additions (especially function declarations) within extern "C" {}
#ifdef __cplusplus
extern "C"
{
#endif /* __cplusplus */

#if defined (ACE_SIGINFO_IS_SIGINFO_T)
   typedef struct siginfo siginfo_t;
#endif /* ACE_LACKS_SIGINFO_H */

#if defined (ACE_LACKS_SIGSET)
#  if !defined(__MINGW32__)
     typedef u_int sigset_t;
#  endif /* !__MINGW32__*/
#endif /* ACE_LACKS_SIGSET */

#if defined (ACE_HAS_SIG_MACROS)
#  undef sigemptyset
#  undef sigfillset
#  undef sigaddset
#  undef sigdelset
#  undef sigismember
#endif /* ACE_HAS_SIG_MACROS */

// This must come after signal.h is #included.  It's to counteract
// the sigemptyset and sigfillset #defines, which only happen
// when __OPTIMIZE__ is #defined (really!) on Linux.
#if defined (linux) && defined (__OPTIMIZE__)
#  undef sigemptyset
#  undef sigfillset
#endif /* linux && __OPTIMIZE__ */

#if !defined (ACE_HAS_SIG_ATOMIC_T)
   typedef int sig_atomic_t;
#endif /* !ACE_HAS_SIG_ATOMIC_T */

# if !defined (SA_SIGINFO)
#   define SA_SIGINFO 0
# endif /* SA_SIGINFO */

# if !defined (SA_RESTART)
#   define SA_RESTART 0
# endif /* SA_RESTART */

#if !defined (SIGHUP)
#  define SIGHUP 0
#endif /* SIGHUP */

#if !defined (SIGINT)
#  define SIGINT 0
#endif /* SIGINT */

#if !defined (SIGSEGV)
#  define SIGSEGV 0
#endif /* SIGSEGV */

#if !defined (SIGIO)
#  define SIGIO 0
#endif /* SIGSEGV */

#if !defined (SIGUSR1)
#  define SIGUSR1 0
#endif /* SIGUSR1 */

#if !defined (SIGUSR2)
#  define SIGUSR2 0
#endif /* SIGUSR2 */

#if !defined (SIGCHLD)
#  define SIGCHLD 0
#endif /* SIGCHLD */

#if !defined (SIGCLD)
#  define SIGCLD SIGCHLD
#endif /* SIGCLD */

#if !defined (SIGQUIT)
#  define SIGQUIT 0
#endif /* SIGQUIT */

#if !defined (SIGPIPE)
#  define SIGPIPE 0
#endif /* SIGPIPE */

#if !defined (SIGALRM)
#  define SIGALRM 0
#endif /* SIGALRM */

#if !defined (SIG_DFL)
#  if defined (ACE_PSOS_DIAB_MIPS) || defined (ACE_PSOS_DIAB_PPC)
#    define SIG_DFL ((void *) 0)
#  else
#    define SIG_DFL ((__sighandler_t) 0)
#  endif
#endif /* SIG_DFL */

#if !defined (SIG_IGN)
#  if defined (ACE_PSOS_DIAB_MIPS) || defined (ACE_PSOS_DIAB_PPC)
#    define SIG_IGN ((void *) 1)     /* ignore signal */
#  else
#    define SIG_IGN ((__sighandler_t) 1)     /* ignore signal */
#  endif
#endif /* SIG_IGN */

#if !defined (SIG_ERR)
#  if defined (ACE_PSOS_DIAB_MIPS) || defined (ACE_PSOS_DIAB_PPC)
#    define SIG_ERR ((void *) -1)    /* error return from signal */
#  else
#    define SIG_ERR ((__sighandler_t) -1)    /* error return from signal */
#  endif
#endif /* SIG_ERR */

// These are used by the <ACE_IPC_SAP::enable> and
// <ACE_IPC_SAP::disable> methods.  They must be unique and cannot
// conflict with the value of <ACE_NONBLOCK>.  We make the numbers
// negative here so they won't conflict with other values like SIGIO,
// etc.
# define ACE_SIGIO -1
# define ACE_SIGURG -2
# define ACE_CLOEXEC -3

#if defined (ACE_PSOS)
#  if !defined (ACE_PSOSIM)
     typedef void (* ACE_SignalHandler) (void);
     typedef void (* ACE_SignalHandlerV) (void);
#    if !defined(SIG_DFL)
#      define SIG_DFL (ACE_SignalHandler) 0
#    endif  /* philabs */
#  endif /* !ACE_PSOSIM */
#  if ! defined (NSIG)
#    define NSIG 32
#  endif /* NSIG */
#endif /* ACE_PSOS && !ACE_PSOSIM */

#if defined (VXWORKS)
#  define ACE_NSIG (_NSIGS + 1)
#elif defined (__Lynx__)
   // LynxOS Neutrino sets NSIG to the highest-numbered signal.
#  define ACE_NSIG (NSIG + 1)
#elif defined (__rtems__)
#  define ACE_NSIG (SIGRTMAX)
#elif defined(__BORLANDC__) && (__BORLANDC__ >= 0x600)
#  define ACE_NSIG _NSIG
#else
   // All other platforms set NSIG to one greater than the
   // highest-numbered signal.
#  define ACE_NSIG NSIG
#endif /* __Lynx__ */

#if defined (ACE_HAS_CONSISTENT_SIGNAL_PROTOTYPES)
   // Prototypes for both signal() and struct sigaction are consistent..
  //#  if defined (ACE_HAS_SIG_C_FUNC)
  //   extern "C" {
  //#  endif /* ACE_HAS_SIG_C_FUNC */
#  if !defined (ACE_PSOS)
     typedef void (*ACE_SignalHandler)(int);
     typedef void (*ACE_SignalHandlerV)(int);
#  endif /* !defined (ACE_PSOS) */
  //#  if defined (ACE_HAS_SIG_C_FUNC)
  //   }
  //#  endif /* ACE_HAS_SIG_C_FUNC */
#elif defined (ACE_HAS_LYNXOS_SIGNALS)
   typedef void (*ACE_SignalHandler)(...);
   typedef void (*ACE_SignalHandlerV)(...);
#elif defined (ACE_HAS_TANDEM_SIGNALS)
   typedef void (*ACE_SignalHandler)(...);
   typedef void (*ACE_SignalHandlerV)(...);
#elif defined (ACE_HAS_IRIX_53_SIGNALS)
   typedef void (*ACE_SignalHandler)(...);
   typedef void (*ACE_SignalHandlerV)(...);
#elif defined (ACE_HAS_SPARCWORKS_401_SIGNALS)
   typedef void (*ACE_SignalHandler)(int, ...);
   typedef void (*ACE_SignalHandlerV)(int,...);
#elif defined (ACE_HAS_SUNOS4_SIGNAL_T)
   typedef void (*ACE_SignalHandler)(...);
   typedef void (*ACE_SignalHandlerV)(...);
#elif defined (ACE_HAS_SVR4_SIGNAL_T)
   // SVR4 Signals are inconsistent (e.g., see struct sigaction)..
   typedef void (*ACE_SignalHandler)(int);
#  if !defined (m88k)     /*  with SVR4_SIGNAL_T */
     typedef void (*ACE_SignalHandlerV)(void);
#  else
     typedef void (*ACE_SignalHandlerV)(int);
#  endif  /*  m88k */       /*  with SVR4_SIGNAL_T */
#elif defined (ACE_WIN32)
   typedef void (__cdecl *ACE_SignalHandler)(int);
   typedef void (__cdecl *ACE_SignalHandlerV)(int);
#elif defined (ACE_HAS_UNIXWARE_SVR4_SIGNAL_T)
   typedef void (*ACE_SignalHandler)(int);
   typedef void (*ACE_SignalHandlerV)(...);
#elif defined (INTEGRITY)
   typedef void (*ACE_SignalHandler)();
   typedef void (*ACE_SignalHandlerV)(int);
#else /* This is necessary for some older broken version of cfront */
#  if defined (SIG_PF)
#    define ACE_SignalHandler SIG_PF
#  else
     typedef void (*ACE_SignalHandler)(int);
#  endif /* SIG_PF */
   typedef void (*ACE_SignalHandlerV)(...);
#endif /* ACE_HAS_CONSISTENT_SIGNAL_PROTOTYPES */

#if defined (ACE_LACKS_SIGACTION)
   struct sigaction
   {
     int sa_flags;
     ACE_SignalHandlerV sa_handler;
     sigset_t sa_mask;
   };
#endif /* ACE_LACKS_SIGACTION */

// Defining POSIX4 real-time signal range.
#if defined(ACE_HAS_POSIX_REALTIME_SIGNALS)
#define ACE_SIGRTMIN SIGRTMIN
#define ACE_SIGRTMAX SIGRTMAX

#else /* !ACE_HAS_POSIX_REALTIME_SIGNALS */

#ifndef ACE_SIGRTMIN
#define ACE_SIGRTMIN 0
#endif /* ACE_SIGRTMIN */

#ifndef ACE_SIGRTMAX
#define ACE_SIGRTMAX 0
#endif /* ACE_SIGRTMAX */

#endif /* ACE_HAS_POSIX_REALTIME_SIGNALS */

#if defined (DIGITAL_UNIX)
   // sigwait is yet another macro on Digital UNIX 4.0, just causing
   // trouble when introducing member functions with the same name.
   // Thanks to Thilo Kielmann" <kielmann@informatik.uni-siegen.de> for
   // this fix.
#  if defined  (__DECCXX_VER)
#    undef sigwait
     // cxx on Digital Unix 4.0 needs this declaration.  With it,
     // <::_Psigwait> works with cxx -pthread.  g++ does _not_ need
     // it.
     int _Psigwait __((const sigset_t *set, int *sig));
#  elif defined (__KCC)
#    undef sigwait
     inline int sigwait (const sigset_t* set, int* sig)
       { return _Psigwait (set, sig); }
#  endif /* __DECCXX_VER */
#elif !defined (ACE_HAS_SIGWAIT)
#  if defined(__rtems__)
     int sigwait (const sigset_t *set, int *sig);
#  else
     int sigwait (sigset_t *set);
#  endif /* __rtems__ */
#endif /* ! DIGITAL_UNIX && ! ACE_HAS_SIGWAIT */

  int pthread_sigmask(int, const sigset_t *, sigset_t *);

#ifdef __cplusplus
}
#endif /* __cplusplus */

#include "ace/os_include/os_ucontext.h"

#include /**/ "ace/post.h"
#endif /* ACE_OS_INCLUDE_OS_SIGNAL_H */
