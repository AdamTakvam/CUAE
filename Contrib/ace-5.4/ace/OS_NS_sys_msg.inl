// -*- C++ -*-
// OS_NS_sys_msg.inl,v 1.5 2004/01/08 16:50:40 bala Exp

#include "ace/OS_NS_errno.h"

ACE_INLINE int
ACE_OS::msgctl (int msqid, int cmd, struct msqid_ds *val)
{
  ACE_OS_TRACE ("ACE_OS::msgctl");
#if defined (ACE_HAS_SYSV_IPC)
  ACE_OSCALL_RETURN (::msgctl (msqid, cmd, val), int, -1);
#else
  ACE_UNUSED_ARG (msqid);
  ACE_UNUSED_ARG (cmd);
  ACE_UNUSED_ARG (val);

  ACE_NOTSUP_RETURN (-1);
#endif /* ACE_HAS_SYSV_IPC */
}

ACE_INLINE int
ACE_OS::msgget (key_t key, int msgflg)
{
  ACE_OS_TRACE ("ACE_OS::msgget");
#if defined (ACE_HAS_SYSV_IPC)
  ACE_OSCALL_RETURN (::msgget (key, msgflg), int, -1);
#else
  ACE_UNUSED_ARG (key);
  ACE_UNUSED_ARG (msgflg);

  ACE_NOTSUP_RETURN (-1);
#endif /* ACE_HAS_SYSV_IPC */
}

ACE_INLINE int
ACE_OS::msgrcv (int int_id, void *buf, size_t len,
                long type, int flags)
{
  ACE_OS_TRACE ("ACE_OS::msgrcv");
#if defined (ACE_HAS_SYSV_IPC)
# if defined (ACE_LACKS_POSIX_PROTOTYPES) || defined (ACE_LACKS_SOME_POSIX_PROTOTYPES)
  ACE_OSCALL_RETURN (::msgrcv (int_id, (msgbuf *) buf, len, type, flags),
                     int, -1);
# else
  ACE_OSCALL_RETURN (::msgrcv (int_id, buf, len, type, flags),
                     int, -1);
# endif /* ACE_LACKS_POSIX_PROTOTYPES */
#else
  ACE_UNUSED_ARG (int_id);
  ACE_UNUSED_ARG (buf);
  ACE_UNUSED_ARG (len);
  ACE_UNUSED_ARG (type);
  ACE_UNUSED_ARG (flags);

  ACE_NOTSUP_RETURN (-1);
#endif /* ACE_HAS_SYSV_IPC */
}

ACE_INLINE int
ACE_OS::msgsnd (int int_id, const void *buf, size_t len, int flags)
{
  ACE_OS_TRACE ("ACE_OS::msgsnd");
#if defined (ACE_HAS_SYSV_IPC)
# if defined (ACE_HAS_NONCONST_MSGSND)
  ACE_OSCALL_RETURN (::msgsnd (int_id, (void *) buf, len, flags), int, -1);
# elif defined (ACE_LACKS_POSIX_PROTOTYPES) || defined (ACE_LACKS_SOME_POSIX_PROTOTYPES)
  ACE_OSCALL_RETURN (::msgsnd (int_id, (msgbuf *) buf, len, flags), int, -1);
# else
  ACE_OSCALL_RETURN (::msgsnd (int_id, ACE_const_cast (void *, buf), len, flags), int, -1);
# endif /* ACE_LACKS_POSIX_PROTOTYPES || ACE_HAS_NONCONST_MSGSND */
#else
  ACE_UNUSED_ARG (int_id);
  ACE_UNUSED_ARG (buf);
  ACE_UNUSED_ARG (len);
  ACE_UNUSED_ARG (flags);

  ACE_NOTSUP_RETURN (-1);
#endif /* ACE_HAS_SYSV_IPC */
}

