// -*- C++ -*-

//=============================================================================
/**
 *  @file   OS_NS_sys_msg.h
 *
 *  OS_NS_sys_msg.h,v 1.3 2003/11/01 23:42:24 dhinton Exp
 *
 *  @author Douglas C. Schmidt <schmidt@cs.wustl.edu>
 *  @author Jesper S. M|ller<stophph@diku.dk>
 *  @author and a cast of thousands...
 *
 *  Originally in OS.h.
 */
//=============================================================================

#ifndef ACE_OS_NS_SYS_MSG_H
# define ACE_OS_NS_SYS_MSG_H

# include /**/ "ace/pre.h"

# include "ace/config-all.h"

# if !defined (ACE_LACKS_PRAGMA_ONCE)
#  pragma once
# endif /* ACE_LACKS_PRAGMA_ONCE */

#include "ace/os_include/sys/os_msg.h"
#include "ace/ACE_export.h"

#if defined (ACE_EXPORT_MACRO)
#  undef ACE_EXPORT_MACRO
#endif
#define ACE_EXPORT_MACRO ACE_Export

namespace ACE_OS {

  //@{ @name A set of wrappers for System V message queues.
  ACE_NAMESPACE_INLINE_FUNCTION
  int msgctl (int msqid,
              int cmd,
              struct msqid_ds *);

  ACE_NAMESPACE_INLINE_FUNCTION
  int msgget (key_t key,
              int msgflg);

  ACE_NAMESPACE_INLINE_FUNCTION
  int msgrcv (int int_id,
              void *buf,
              size_t len,
              long type,
              int flags);

  ACE_NAMESPACE_INLINE_FUNCTION
  int msgsnd (int int_id,
              const void *buf,
              size_t len,
              int flags);
  //@}

} /* namespace ACE_OS */

# if defined (ACE_HAS_INLINED_OSCALLS)
#   if defined (ACE_INLINE)
#     undef ACE_INLINE
#   endif /* ACE_INLINE */
#   define ACE_INLINE inline
#   include "ace/OS_NS_sys_msg.inl"
# endif /* ACE_HAS_INLINED_OSCALLS */

# include /**/ "ace/post.h"
#endif /* ACE_OS_NS_SYS_MSG_H */
