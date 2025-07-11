// Shared_Memory_MM.cpp
// Shared_Memory_MM.cpp,v 4.6 2003/07/27 20:48:27 dhinton Exp

#include "ace/Shared_Memory_MM.h"

#if !defined (__ACE_INLINE__)
#include "ace/Shared_Memory_MM.i"
#endif /* __ACE_INLINE__ */

ACE_RCSID(ace, Shared_Memory_MM, "Shared_Memory_MM.cpp,v 4.6 2003/07/27 20:48:27 dhinton Exp")

ACE_ALLOC_HOOK_DEFINE(ACE_Shared_Memory_MM)

void
ACE_Shared_Memory_MM::dump (void) const
{
#if defined (ACE_HAS_DUMP)
  ACE_TRACE ("ACE_Shared_Memory_MM::dump");
#endif /* ACE_HAS_DUMP */
}

// Creates a shared memory segment of SIZE bytes.

ACE_Shared_Memory_MM::ACE_Shared_Memory_MM (ACE_HANDLE handle,
                                            int length,
                                            int prot,
                                            int share,
                                            char *addr,
                                            off_t pos)
  : shared_memory_ (handle, length, prot, share, addr, pos)
{
  ACE_TRACE ("ACE_Shared_Memory_MM::ACE_Shared_Memory_MM");
}

ACE_Shared_Memory_MM::ACE_Shared_Memory_MM (const ACE_TCHAR *file_name,
                                            int len,
                                            int flags,
                                            int mode,
                                            int prot,
                                            int share,
                                            char *addr,
                                            off_t pos)
  : shared_memory_ (file_name, len, flags, mode,
                    prot, share, addr, pos)
{
  ACE_TRACE ("ACE_Shared_Memory_MM::ACE_Shared_Memory_MM");
}

// The "do-nothing" constructor.

ACE_Shared_Memory_MM::ACE_Shared_Memory_MM (void)
{
  ACE_TRACE ("ACE_Shared_Memory_MM::ACE_Shared_Memory_MM");
}
