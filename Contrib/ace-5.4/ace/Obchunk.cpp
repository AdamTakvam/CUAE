// Obchunk.cpp,v 4.3 2003/07/27 20:48:26 dhinton Exp

#include "ace/Obchunk.h"

#if !defined (__ACE_INLINE__)
#include "ace/Obchunk.i"
#endif /* __ACE_INLINE__ */

ACE_RCSID(ace, Obchunk, "Obchunk.cpp,v 4.3 2003/07/27 20:48:26 dhinton Exp")

ACE_ALLOC_HOOK_DEFINE(ACE_Obchunk)

void
ACE_Obchunk::dump (void) const
{
#if defined (ACE_HAS_DUMP)
  ACE_TRACE ("ACE_Obchunk::dump");

  ACE_DEBUG ((LM_DEBUG, ACE_BEGIN_DUMP, this));
  ACE_DEBUG ((LM_DEBUG,  ACE_LIB_TEXT ("end_ = %x\n"), this->end_));
  ACE_DEBUG ((LM_DEBUG,  ACE_LIB_TEXT ("cur_ = %x\n"), this->cur_));
  ACE_DEBUG ((LM_DEBUG, ACE_END_DUMP));
#endif /* ACE_HAS_DUMP */
}

ACE_Obchunk::ACE_Obchunk (size_t size)
  : end_ (contents_ + size),
    block_ (contents_),
    cur_ (contents_),
    next_ (0)
{
}
