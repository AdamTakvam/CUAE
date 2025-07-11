/* -*- C++ -*- */
// config-irix6.x-kcc.h,v 4.8 2003/07/19 19:04:14 dhinton Exp

// The following configuration file is designed to work for the SGI
// Indigo2EX running Irix 6.5 platform using the KAI C++ compiler.

#ifndef ACE_CONFIG_H
#define ACE_CONFIG_H
#include /**/ "ace/pre.h"

#include "ace/config-kcc-common.h"


// Platform supports the very odd IRIX 6.2 threads...
#define ACE_HAS_THREADS
#if !defined (ACE_MT_SAFE)
# define ACE_MT_SAFE 1
#endif /* !ACE_MT_SAFE */
#define ACE_HAS_IRIX62_THREADS

// IRIX 6.2 supports a variant of POSIX Pthreads, supposedly POSIX 1c
#define ACE_HAS_PTHREADS
#define ACE_HAS_PTHREADS_STD

#include "ace/config-irix6.x-common.h"

// Needed for the threading stuff?
#include /**/ <sched.h>
#include /**/ <task.h>

#define PTHREAD_MIN_PRIORITY PX_PRIO_MIN
#define PTHREAD_MAX_PRIORITY PX_PRIO_MAX

// Compiler/platform has thread-specific storage
#define ACE_HAS_THREAD_SPECIFIC_STORAGE

// The pthread_cond_timedwait call does not reset the timer.
#define ACE_LACKS_COND_TIMEDWAIT_RESET 1

#define ACE_HAS_STRING_CLASS
#ifndef IRIX6
# define IRIX6
#endif

// Denotes that GNU has cstring.h as standard
// which redefines memchr()
#define ACE_HAS_GNU_CSTRING_H

// Compiler/platform supports SVR4 signal typedef.
#define ACE_HAS_IRIX_53_SIGNALS

#define ACE_HAS_CONSISTENT_SIGNAL_PROTOTYPES

// Compiler/platform supports sys_siglist array.
//#define ACE_HAS_SYS_SIGLIST

// This gets defined in config-irix6.x-common.h, but kcc does support this.
#undef ACE_LACKS_PLACEMENT_OPERATOR_DELETE

// Shift this --- otherwise, it lands on top of libKCC.
#define ACE_DEFAULT_BASE_ADDR ((char*) 0x0500000)

// kcc 4.0d miscompiles memchr_emulation on irix at +K3 optimization.
// So just use the library version instead.
#define ACE_HAS_MEMCHR

#include /**/ "ace/post.h"
#endif /* ACE_CONFIG_H */
