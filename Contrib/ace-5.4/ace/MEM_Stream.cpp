// MEM_Stream.cpp
// MEM_Stream.cpp,v 4.7 2003/07/27 20:48:25 dhinton Exp

#include "ace/MEM_Stream.h"

#if (ACE_HAS_POSITION_INDEPENDENT_POINTERS == 1)
#if defined (ACE_LACKS_INLINE_FUNCTIONS)
#include "ace/MEM_Stream.i"
#endif

ACE_RCSID(ace, MEM_Stream, "MEM_Stream.cpp,v 4.7 2003/07/27 20:48:25 dhinton Exp")

ACE_ALLOC_HOOK_DEFINE(ACE_MEM_Stream)

void
ACE_MEM_Stream::dump (void) const
{
#if defined (ACE_HAS_DUMP)
  ACE_TRACE ("ACE_MEM_Stream::dump");
#endif /* ACE_HAS_DUMP */
}

int
ACE_MEM_Stream::close (void)
{
  this->send ((char *)0, 0);

  this->fini ();

#if defined (ACE_WIN32)
  // We need the following call to make things work correctly on
  // Win32, which requires use to do a <close_writer> before doing the
  // close in order to avoid losing data.  Note that we don't need to
  // do this on UNIX since it doesn't have this "feature".  Moreover,
  // this will cause subtle problems on UNIX due to the way that
  // fork() works.
  this->close_writer ();
#endif /* ACE_WIN32 */
  // Close down the socket.
  return ACE_SOCK::close ();
}

#endif /* ACE_HAS_POSITION_INDEPENDENT_POINTERS == 1 */
