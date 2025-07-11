// Lock.cpp,v 4.2 2003/10/20 16:38:37 dhinton Exp

#include "ace/Lock.h"

#if !defined (__ACE_INLINE__)
#include "ace/Lock.inl"
#endif /* __ACE_INLINE__ */

ACE_RCSID(ace, Lock, "Lock.cpp,v 4.2 2003/10/20 16:38:37 dhinton Exp")

ACE_Lock::~ACE_Lock (void)
{
}

ACE_Adaptive_Lock::ACE_Adaptive_Lock (void)
  : lock_ (0)
{
}

ACE_Adaptive_Lock::~ACE_Adaptive_Lock (void)
{
}

int
ACE_Adaptive_Lock::remove (void)
{
  return this->lock_->remove ();
}

int
ACE_Adaptive_Lock::acquire (void)
{
  return this->lock_->acquire ();
}

int
ACE_Adaptive_Lock::tryacquire (void)
{
  return this->lock_->tryacquire ();
}

int
ACE_Adaptive_Lock::release (void)
{
  return this->lock_->release ();
}

int
ACE_Adaptive_Lock::acquire_read (void)
{
  return this->lock_->acquire_read ();
}

int
ACE_Adaptive_Lock::acquire_write (void)
{
  return this->lock_->acquire_write ();
}

int
ACE_Adaptive_Lock::tryacquire_read (void)
{
  return this->lock_->tryacquire_read ();
}

int
ACE_Adaptive_Lock::tryacquire_write (void)
{
  return this->lock_->tryacquire_write ();
}

int
ACE_Adaptive_Lock::tryacquire_write_upgrade (void)
{
  return this->lock_->tryacquire_write_upgrade ();
}

void
ACE_Adaptive_Lock::dump (void) const
{
#if defined (ACE_HAS_DUMP)
  //  return this->lock_->dump ();
#endif /* ACE_HAS_DUMP */
}
