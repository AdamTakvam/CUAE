/* -*- C++ -*- */
// Profile_Timer.i,v 4.11 2003/11/01 11:15:16 dhinton Exp

#include "ace/OS_NS_sys_time.h"
#include "ace/OS_NS_sys_resource.h"
#include "ace/Global_Macros.h"

#if (defined (ACE_HAS_PRUSAGE_T) || defined (ACE_HAS_GETRUSAGE)) && !defined (ACE_WIN32)

#  if defined (ACE_HAS_PRUSAGE_T)
ACE_INLINE int
ACE_Profile_Timer::start (void)
{
  ACE_TRACE ("ACE_Profile_Timer::start");
  return ACE_OS::ioctl (this->proc_handle_,
                        PIOCUSAGE,
                        &this->begin_usage_);
}

ACE_INLINE int
ACE_Profile_Timer::stop (void)
{
  ACE_TRACE ("ACE_Profile_Timer::stop");
  this->last_usage_ = this->end_usage_;
  return ACE_OS::ioctl (this->proc_handle_,
                    PIOCUSAGE,
                    &this->end_usage_);
}
#  elif defined (ACE_HAS_GETRUSAGE)
ACE_INLINE int
ACE_Profile_Timer::start (void)
{
  ACE_TRACE ("ACE_Profile_Timer::start");
  this->begin_time_ = ACE_OS::gettimeofday ();
  ACE_OS::getrusage (RUSAGE_SELF,
                     &this->begin_usage_);
  return 0;
}

ACE_INLINE int
ACE_Profile_Timer::stop (void)
{
  ACE_TRACE ("ACE_Profile_Timer::stop");
  this->last_time_ = this->end_time_;
  this->end_time_ = ACE_OS::gettimeofday ();
  this->last_usage_ = this->end_usage_;
  ACE_OS::getrusage (RUSAGE_SELF,
                     &this->end_usage_);
  return 0;
}

#  endif /* ACE_HAS_PRUSAGE_T */

#elif defined (ACE_WIN32)

ACE_INLINE
ACE_Profile_Timer::~ACE_Profile_Timer (void)
{
}

ACE_INLINE int
ACE_Profile_Timer::start (void)
{
  ACE_TRACE ("ACE_Profile_Timer::start");
#  if defined (ACE_HAS_GETRUSAGE)
  ACE_OS::getrusage (RUSAGE_SELF,
                     &this->begin_usage_);
#  endif /* ACE_HAS_GETRUSAGE */
  this->timer_.start ();
  return 0;
}

ACE_INLINE int
ACE_Profile_Timer::stop (void)
{
  ACE_TRACE ("ACE_Profile_Timer::stop");
  this->timer_.stop ();
#  if defined (ACE_HAS_GETRUSAGE)
  this->last_usage_ = this->end_usage_;
  ACE_OS::getrusage (RUSAGE_SELF, &this->end_usage_);
#  endif /* ACE_HAS_GETRUSAGE */
  return 0;
}

#else

ACE_INLINE int
ACE_Profile_Timer::start (void)
{
  ACE_TRACE ("ACE_Profile_Timer::start");
  this->timer_.start ();
  return 0;
}

ACE_INLINE int
ACE_Profile_Timer::stop (void)
{
  ACE_TRACE ("ACE_Profile_Timer::stop");
  this->timer_.stop ();
  return 0;
}

ACE_INLINE
ACE_Profile_Timer::~ACE_Profile_Timer (void)
{
}

#endif /* defined (ACE_HAS_PRUSAGE_T) || defined (ACE_HAS_GETRUSAGE) */
