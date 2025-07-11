// UPIPE_Connector.cpp
// UPIPE_Connector.cpp,v 4.11 2003/07/27 20:48:28 dhinton Exp

#include "ace/UPIPE_Connector.h"

ACE_RCSID(ace, UPIPE_Connector, "UPIPE_Connector.cpp,v 4.11 2003/07/27 20:48:28 dhinton Exp")

#if defined (ACE_HAS_THREADS)

#if defined (ACE_LACKS_INLINE_FUNCTIONS)
#include "ace/UPIPE_Connector.i"
#endif

ACE_ALLOC_HOOK_DEFINE(ACE_UPIPE_Connector)

void
ACE_UPIPE_Connector::dump (void) const
{
#if defined (ACE_HAS_DUMP)
  ACE_TRACE ("ACE_UPIPE_Connector::dump");
#endif /* ACE_HAS_DUMP */
}

ACE_UPIPE_Connector::ACE_UPIPE_Connector (void)
{
  ACE_TRACE ("ACE_UPIPE_Connector::ACE_UPIPE_Connector");
}

int
ACE_UPIPE_Connector::connect (ACE_UPIPE_Stream &new_stream,
                              const ACE_UPIPE_Addr &addr,
                              ACE_Time_Value *timeout,
                              const ACE_Addr & /* local_sap */,
                              int /* reuse_addr */,
                              int flags,
                              int perms)
{
  ACE_TRACE ("ACE_UPIPE_Connector::connect");
  ACE_ASSERT (new_stream.get_handle () == ACE_INVALID_HANDLE);

  ACE_HANDLE handle = ACE_Handle_Ops::handle_timed_open (timeout,
                                                         addr.get_path_name (),
                                                         flags, perms);

  if (handle == ACE_INVALID_HANDLE)
    return -1;
#if !defined (ACE_WIN32)
  else if (ACE_OS::isastream (handle) != 1)
    return -1;
#endif
  else // We're connected!
    {
      ACE_MT (ACE_GUARD_RETURN (ACE_Thread_Mutex, ace_mon, new_stream.lock_, -1));

      ACE_UPIPE_Stream *ustream = &new_stream;

      new_stream.set_handle (handle);
      new_stream.remote_addr_ = addr; // class copy.
      new_stream.reference_count_++;

      // Now send the address of our ACE_UPIPE_Stream over this pipe
      // to our corresponding ACE_UPIPE_Acceptor, so he may link the
      // two streams.
      ssize_t result = ACE_OS::write (handle,
				      (const char *) &ustream,
				      sizeof ustream);
      if (result == -1)
	ACE_ERROR ((LM_ERROR,
		    ACE_LIB_TEXT ("ACE_UPIPE_Connector %p\n"),
		    ACE_LIB_TEXT ("write to pipe failed")));

      // Wait for confirmation of stream linking.
      ACE_Message_Block *mb_p = 0;

      // Our part is done, wait for server to confirm connection.
      result = new_stream.recv (mb_p, 0);

      // Do *not* coalesce the following two checks for result == -1.
      // They perform different checks and cannot be merged.
      if (result == -1)
	  ACE_ERROR ((LM_ERROR,
                      ACE_LIB_TEXT ("ACE_UPIPE_Connector %p\n"),
		      ACE_LIB_TEXT ("no confirmation from server")));
      else
	// Close down the new_stream at this point in order to
	// conserve handles.  Note that we don't need the SPIPE
	// connection anymore since we're linked via the Message_Queue
	// now.
	new_stream.ACE_SPIPE::close ();
      return result;
    }
}
#endif /* ACE_HAS_THREADS */
