// FIFO_Recv.cpp
// FIFO_Recv.cpp,v 4.13 2003/07/27 20:48:24 dhinton Exp

#include "ace/FIFO_Recv.h"
#include "ace/Log_Msg.h"

#if defined (ACE_LACKS_INLINE_FUNCTIONS)
#include "ace/FIFO_Recv.i"
#endif

ACE_RCSID(ace, FIFO_Recv, "FIFO_Recv.cpp,v 4.13 2003/07/27 20:48:24 dhinton Exp")

ACE_ALLOC_HOOK_DEFINE(ACE_FIFO_Recv)

void
ACE_FIFO_Recv::dump (void) const
{
#if defined (ACE_HAS_DUMP)
  ACE_TRACE ("ACE_FIFO_Recv::dump");
  ACE_DEBUG ((LM_DEBUG, ACE_BEGIN_DUMP, this));
  ACE_FIFO::dump ();
  ACE_DEBUG ((LM_DEBUG,  ACE_LIB_TEXT ("aux_handle_ = %d"), this->aux_handle_));
  ACE_DEBUG ((LM_DEBUG, ACE_END_DUMP));
#endif /* ACE_HAS_DUMP */
}

int 
ACE_FIFO_Recv::close (void)
{
  ACE_TRACE ("ACE_FIFO_Recv::close");
  int result = ACE_FIFO::close ();

  if (this->aux_handle_ != ACE_INVALID_HANDLE)
    return ACE_OS::close (this->aux_handle_);
  else
    return result;
}

// Note that persistent means "open fifo for writing, as well as
// reading."  This ensures that the fifo never gets EOF, even if there
// aren't any writers at the moment!

int
ACE_FIFO_Recv::open (const ACE_TCHAR *fifo_name,
		     int flags,
		     int perms,
		     int persistent,
                     LPSECURITY_ATTRIBUTES sa)
{
  ACE_TRACE ("ACE_FIFO_Recv::open");

  if (ACE_FIFO::open (fifo_name, ACE_NONBLOCK | flags, perms, sa) == -1)
    return -1;
  else if (this->disable (ACE_NONBLOCK) == -1)
    return -1;
  else if (persistent 
	   && (this->aux_handle_ = ACE_OS::open (fifo_name, O_WRONLY, 0, sa)) == ACE_INVALID_HANDLE)
    return -1;
  else
    return this->get_handle () == ACE_INVALID_HANDLE ? -1 : 0;
}

ACE_FIFO_Recv::ACE_FIFO_Recv (void)
  : aux_handle_ (ACE_INVALID_HANDLE)
{
  ACE_TRACE ("ACE_FIFO_Recv::ACE_FIFO_Recv");
}

ACE_FIFO_Recv::ACE_FIFO_Recv (const ACE_TCHAR *fifo_name,
			      int flags,
			      int perms,
			      int persistent,
                              LPSECURITY_ATTRIBUTES sa)
  : aux_handle_ (ACE_INVALID_HANDLE)
{
  ACE_TRACE ("ACE_FIFO_Recv::ACE_FIFO_Recv");

  if (this->ACE_FIFO_Recv::open (fifo_name,
				 flags,
				 perms,
				 persistent,
                                 sa) == -1)
    ACE_ERROR ((LM_ERROR,  ACE_LIB_TEXT ("%p\n"),  ACE_LIB_TEXT ("ACE_FIFO_Recv")));
}
