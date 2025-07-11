// -*- C++ -*-

//=============================================================================
/**
 *  @file    os_semaphore.h
 *
 *  semaphores (REALTIME)
 *
 *  os_semaphore.h,v 1.3 2003/07/19 19:04:15 dhinton Exp
 *
 *  @author Don Hinton <dhinton@dresystems.com>
 *  @author This code was originally in various places including ace/OS.h.
 */
//=============================================================================

#ifndef ACE_OS_INCLUDE_OS_SEMAPHORE_H
#define ACE_OS_INCLUDE_OS_SEMAPHORE_H

#include /**/ "ace/pre.h"

#include "ace/config-all.h"

#if !defined (ACE_LACKS_PRAGMA_ONCE)
# pragma once
#endif /* ACE_LACKS_PRAGMA_ONCE */

#include "ace/os_include/os_time.h"

#if !defined (ACE_LACKS_SEMAPHORE_H)
# include /**/ <semaphore.h>
#endif /* !ACE_LACKS_SEMAPHORE_H */

#if defined (VXWORKS)
#  include /**/ <semLib.h>
#endif /* VXWORKS */

// Place all additions (especially function declarations) within extern "C" {}
#ifdef __cplusplus
extern "C"
{
#endif /* __cplusplus */

#if defined (ACE_HAS_POSIX_SEM)
#  if !defined (SEM_FAILED) && !defined (ACE_LACKS_NAMED_POSIX_SEM)
#    define SEM_FAILED ((sem_t *) -1)
#  endif  /* !SEM_FAILED */

   typedef struct
   {
     /// Pointer to semaphore handle.  This is allocated by ACE if we are
     /// working with an unnamed POSIX semaphore or by the OS if we are
     /// working with a named POSIX semaphore.
     sem_t *sema_;

     /// Name of the semaphore (if this is non-NULL then this is a named
     /// POSIX semaphore, else its an unnamed POSIX semaphore).
     char *name_;

#  if defined (ACE_LACKS_NAMED_POSIX_SEM)
     /// this->sema_ doesn't always get created dynamically if a platform
     /// doesn't support named posix semaphores.  We use this flag to
     /// remember if we need to delete <sema_> or not.
     int new_sema_;
#  endif /* ACE_LACKS_NAMED_POSIX_SEM */
   } ACE_sema_t;
#endif /* ACE_HAS_POSIX_SEM */

#ifdef __cplusplus
}
#endif /* __cplusplus */

#include /**/ "ace/post.h"
#endif /* ACE_OS_INCLUDE_OS_SEMAPHORE_H */
