/* -*- C++ -*- */
// Unbounded_Set_Ex.inl,v 4.2 2003/05/18 19:17:34 dhinton Exp

#include "ace/Global_Macros.h"

template <class T> ACE_INLINE int
ACE_Unbounded_Set_Ex<T>::is_empty (void) const
{
  ACE_TRACE ("ACE_Unbounded_Set_Ex<T>::is_empty");
  return this->head_ == this->head_->next_;
}

template <class T> ACE_INLINE int
ACE_Unbounded_Set_Ex<T>::is_full (void) const
{
  ACE_TRACE ("ACE_Unbounded_Set_Ex<T>::is_full");
  return 0; // We should implement a "node of last resort for this..."
}
