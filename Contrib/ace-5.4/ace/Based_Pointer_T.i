/* -*- C++ -*- */
// Based_Pointer_T.i,v 4.7 2003/07/30 21:15:58 dhinton Exp

#define ACE_COMPUTE_BASED_POINTER(P) (((char *) (P) - (P)->base_offset_) + (P)->target_)
#include "ace/Global_Macros.h"

template <class CONCRETE> ACE_INLINE CONCRETE *
ACE_Based_Pointer<CONCRETE>::operator->(void)
{
  ACE_TRACE ("ACE_Based_Pointer<CONCRETE>::operator->");
  return (CONCRETE *)(ACE_COMPUTE_BASED_POINTER (this));
}

template <class CONCRETE> ACE_INLINE void
ACE_Based_Pointer_Basic<CONCRETE>::operator = (CONCRETE *rhs)
{
  ACE_TRACE ("ACE_Based_Pointer_Basic<CONCRETE>::operator =");
  if (rhs == 0)
    // Store a value of <target_> that indicate "NULL" pointer.
    this->target_ = -1;
  else
    this->target_ = ((char *) rhs
                     - ((char *) this - this->base_offset_));
}

template <class CONCRETE> ACE_INLINE void
ACE_Based_Pointer<CONCRETE>::operator = (CONCRETE *rhs)
{
  ACE_TRACE ("ACE_Based_Pointer<CONCRETE>::operator =");
  if (rhs == 0)
    // Store a value of <target_> that indicate "NULL" pointer.
    this->target_ = -1;
  else
    this->target_ = ((char *) rhs
                     - ((char *) this - this->base_offset_));
}

template <class CONCRETE> ACE_INLINE CONCRETE 
ACE_Based_Pointer_Basic<CONCRETE>::operator *(void) const
{
  ACE_TRACE ("ACE_Based_Pointer_Basic<CONCRETE>::operator *");
  return *ACE_reinterpret_cast (CONCRETE *,
                                ACE_COMPUTE_BASED_POINTER (this));
}

template <class CONCRETE> ACE_INLINE CONCRETE *
ACE_Based_Pointer_Basic<CONCRETE>::addr (void) const
{
  ACE_TRACE ("ACE_Based_Pointer_Basic<CONCRETE>::addr");

  if (this->target_ == -1)
    return 0;
  else
    return ACE_reinterpret_cast (CONCRETE *,
                                 ACE_COMPUTE_BASED_POINTER (this));
}

template <class CONCRETE> ACE_INLINE
ACE_Based_Pointer_Basic<CONCRETE>::operator CONCRETE *() const
{
  ACE_TRACE ("ACE_Based_Pointer_Basic<CONCRETE>::operator CONCRETE *()");

  return this->addr ();
}

template <class CONCRETE> ACE_INLINE CONCRETE
ACE_Based_Pointer_Basic<CONCRETE>::operator [] (int index) const
{
  ACE_TRACE ("ACE_Based_Pointer_Basic<CONCRETE>::operator []");
  CONCRETE *c = ACE_reinterpret_cast (CONCRETE *,
                                      ACE_COMPUTE_BASED_POINTER (this));
  return c[index];
}

template <class CONCRETE> ACE_INLINE void
ACE_Based_Pointer_Basic<CONCRETE>::operator += (int index)
{
  ACE_TRACE ("ACE_Based_Pointer_Basic<CONCRETE>::operator +=");
  this->base_offset_ += (index * sizeof (CONCRETE));
}

template <class CONCRETE> ACE_INLINE int 
ACE_Based_Pointer_Basic<CONCRETE>::operator == (const ACE_Based_Pointer_Basic<CONCRETE> &rhs) const
{
  ACE_TRACE ("ACE_Based_Pointer_Basic<CONCRETE>::operator ==");
  return ACE_COMPUTE_BASED_POINTER (this) == ACE_COMPUTE_BASED_POINTER (&rhs);
}

template <class CONCRETE> ACE_INLINE int 
ACE_Based_Pointer_Basic<CONCRETE>::operator != (const ACE_Based_Pointer_Basic<CONCRETE> &rhs) const
{
  ACE_TRACE ("ACE_Based_Pointer_Basic<CONCRETE>::operator !=");
  return !(*this == rhs);
}

template <class CONCRETE> ACE_INLINE int 
ACE_Based_Pointer_Basic<CONCRETE>::operator < (const ACE_Based_Pointer_Basic<CONCRETE> &rhs) const
{
  ACE_TRACE ("ACE_Based_Pointer_Basic<CONCRETE>::operator <");
  return ACE_COMPUTE_BASED_POINTER (this) < ACE_COMPUTE_BASED_POINTER (&rhs);
}

template <class CONCRETE> ACE_INLINE int 
ACE_Based_Pointer_Basic<CONCRETE>::operator <= (const ACE_Based_Pointer_Basic<CONCRETE> &rhs) const
{
  ACE_TRACE ("ACE_Based_Pointer_Basic<CONCRETE>::operator <=");
  return ACE_COMPUTE_BASED_POINTER (this) <= ACE_COMPUTE_BASED_POINTER (&rhs);
}

template <class CONCRETE> ACE_INLINE int 
ACE_Based_Pointer_Basic<CONCRETE>::operator > (const ACE_Based_Pointer_Basic<CONCRETE> &rhs) const
{
  ACE_TRACE ("ACE_Based_Pointer_Basic<CONCRETE>::operator >");
  return ACE_COMPUTE_BASED_POINTER (this) > ACE_COMPUTE_BASED_POINTER (&rhs);
}

template <class CONCRETE> ACE_INLINE int 
ACE_Based_Pointer_Basic<CONCRETE>::operator >= (const ACE_Based_Pointer_Basic<CONCRETE> &rhs) const
{
  ACE_TRACE ("ACE_Based_Pointer_Basic<CONCRETE>::operator >=");
  return ACE_COMPUTE_BASED_POINTER (this) >= ACE_COMPUTE_BASED_POINTER (&rhs);
}

template <class CONCRETE> ACE_INLINE void
ACE_Based_Pointer_Basic<CONCRETE>::operator= (const ACE_Based_Pointer_Basic<CONCRETE> &rhs)
{
  ACE_TRACE ("ACE_Based_Pointer_Basic<CONCRETE>::operator=");
  *this = rhs.addr ();
}

template <class CONCRETE> ACE_INLINE void
ACE_Based_Pointer<CONCRETE>::operator= (const ACE_Based_Pointer<CONCRETE> &rhs)
{
  ACE_TRACE ("ACE_Based_Pointer<CONCRETE>::operator=");
  *this = rhs.addr ();
}

