// SPIPE_Connector.cpp
// SPIPE_Connector.cpp,v 4.14 2002/06/09 22:09:18 schmidt Exp

#include "ace/SPIPE_Connector.h"
#include "ace/Log_Msg.h"

#if defined (ACE_LACKS_INLINE_FUNCTIONS)
#include "ace/SPIPE_Connector.i"
#endif

ACE_RCSID(ace, SPIPE_Connector, "SPIPE_Connector.cpp,v 4.14 2002/06/09 22:09:18 schmidt Exp")

ACE_ALLOC_HOOK_DEFINE(ACE_SPIPE_Connector)

// Creates a Local ACE_SPIPE.

ACE_SPIPE_Connector::ACE_SPIPE_Connector (ACE_SPIPE_Stream &new_io,
					  const ACE_SPIPE_Addr &remote_sap,
					  ACE_Time_Value *timeout,
					  const ACE_Addr & local_sap,
					  int reuse_addr,
					  int flags,
					  int perms,
                                          LPSECURITY_ATTRIBUTES sa)
{
  ACE_TRACE ("ACE_SPIPE_Connector::ACE_SPIPE_Connector");
  if (this->connect (new_io, remote_sap, timeout, local_sap,
		     reuse_addr, flags, perms, sa) == -1
      && timeout != 0 && !(errno == EWOULDBLOCK || errno == ETIME))
    ACE_ERROR ((LM_ERROR, ACE_LIB_TEXT ("address %s, %p\n"),
	       remote_sap.get_path_name (), ACE_LIB_TEXT ("ACE_SPIPE_Connector")));
}

void
ACE_SPIPE_Connector::dump (void) const
{
  ACE_TRACE ("ACE_SPIPE_Connector::dump");
}

ACE_SPIPE_Connector::ACE_SPIPE_Connector (void)
{
  ACE_TRACE ("ACE_SPIPE_Connector::ACE_SPIPE_Connector");
}

int
ACE_SPIPE_Connector::connect (ACE_SPIPE_Stream &new_io,
                              const ACE_SPIPE_Addr &remote_sap,
                              ACE_Time_Value *timeout,
                              const ACE_Addr & /* local_sap */,
                              int /* reuse_addr */,
                              int flags,
                              int perms,
                              LPSECURITY_ATTRIBUTES sa)
{
  ACE_TRACE ("ACE_SPIPE_Connector::connect");

  // Make darn sure that the O_CREAT flag is not set!
#if ! defined (ACE_PSOS_DIAB_MIPS)
  ACE_CLR_BITS (flags, O_CREAT);
# endif /* !ACE_PSOS_DIAB_MIPS */
  ACE_HANDLE handle = ACE_Handle_Ops::handle_timed_open (timeout,
                                                         remote_sap.get_path_name (),
                                                         flags, perms, sa);
  new_io.set_handle (handle);
  new_io.remote_addr_ = remote_sap; // class copy.

#if defined (ACE_WIN32) && !defined (ACE_HAS_PHARLAP)
  DWORD pipe_mode = PIPE_READMODE_MESSAGE | PIPE_WAIT;

  // Set named pipe mode and buffering characteristics.
  if (handle != ACE_INVALID_HANDLE)
    return ::SetNamedPipeHandleState (handle,
                                      &pipe_mode,
                                      0,
                                      0);
#endif
  return handle == ACE_INVALID_HANDLE ? -1 : 0;
}
