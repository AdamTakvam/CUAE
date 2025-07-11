/* -*- C++ -*- */
// TLI_Stream.i,v 4.7 2002/05/02 05:33:17 irfan Exp

// TLI_Stream.i

#include "ace/TLI_Stream.h"

ACE_INLINE
void
ACE_TLI_Stream::set_rwflag (int value)
{
  ACE_TRACE ("ACE_TLI_Stream::set_rwflag");
  this->rwflag_ = value;
}

ACE_INLINE
int
ACE_TLI_Stream::get_rwflag (void)
{
  ACE_TRACE ("ACE_TLI_Stream::get_rwflag");
  return this->rwflag_;
}
