// Typed_SV_Message.cpp
// Typed_SV_Message.cpp,v 4.4 2003/07/27 20:48:28 dhinton Exp

#ifndef ACE_TYPED_SV_MESSAGE_C
#define ACE_TYPED_SV_MESSAGE_C
#include "ace/Typed_SV_Message.h"

#if !defined (ACE_LACKS_PRAGMA_ONCE)
# pragma once
#endif /* ACE_LACKS_PRAGMA_ONCE */

#if !defined (__ACE_INLINE__)
#include "ace/Typed_SV_Message.i"
#endif /* __ACE_INLINE__ */

ACE_RCSID(ace, Typed_SV_Message, "Typed_SV_Message.cpp,v 4.4 2003/07/27 20:48:28 dhinton Exp")

ACE_ALLOC_HOOK_DEFINE(ACE_Typed_SV_Message)

template <class T> void
ACE_Typed_SV_Message<T>::dump (void) const
{
#if defined (ACE_HAS_DUMP)
  ACE_TRACE ("ACE_Typed_SV_Message<T>::dump");
#endif /* ACE_HAS_DUMP */
}

#endif /* ACE_TYPED_SV_MESSAGE_C */
