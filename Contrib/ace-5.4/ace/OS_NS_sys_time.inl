// -*- C++ -*-
// OS_NS_sys_time.inl,v 1.5 2003/11/12 16:31:27 dhinton Exp

#include "ace/os_include/sys/os_time.h"
#include "ace/os_include/os_errno.h"

#if defined (VXWORKS) || defined (CHORUS) || defined (ACE_PSOS)
#  include "ace/OS_NS_time.h"
#endif /* VXWORKS || CHORUS || ACE_PSOS */

ACE_INLINE ACE_Time_Value
ACE_OS::gettimeofday (void)
{
  // ACE_OS_TRACE ("ACE_OS::gettimeofday");

#if !defined (ACE_HAS_WINCE)&& !defined (ACE_WIN32)
  timeval tv;
  int result = 0;
#endif // !defined (ACE_HAS_WINCE)&& !defined (ACE_WIN32)

#if (0)
  struct timespec ts;

  ACE_OSCALL (ACE_OS::clock_gettime (CLOCK_REALTIME, &ts), int, -1, result);
  tv.tv_sec = ts.tv_sec;
  tv.tv_usec = ts.tv_nsec / 1000L;  // timespec has nsec, but timeval has usec

#elif defined (ACE_HAS_WINCE)
  SYSTEMTIME tsys;
  FILETIME   tfile;
  ::GetSystemTime (&tsys);
  ::SystemTimeToFileTime (&tsys, &tfile);
  return ACE_Time_Value (tfile);
#elif defined (ACE_WIN32)
  FILETIME   tfile;
  ::GetSystemTimeAsFileTime (&tfile);
  return ACE_Time_Value (tfile);
#if 0
  // From Todd Montgomery...
  struct _timeb tb;
  ::_ftime (&tb);
  tv.tv_sec = tb.time;
  tv.tv_usec = 1000 * tb.millitm;
#endif /* 0 */
#elif defined (ACE_HAS_AIX_HI_RES_TIMER)
  timebasestruct_t tb;

  ::read_real_time (&tb, TIMEBASE_SZ);
  ::time_base_to_time (&tb, TIMEBASE_SZ);

  tv.tv_sec = tb.tb_high;
  tv.tv_usec = tb.tb_low / 1000L;
#else
# if defined (ACE_HAS_TIMEZONE_GETTIMEOFDAY) || \
  (defined (ACE_HAS_SVR4_GETTIMEOFDAY) && !defined (m88k) && !defined (SCO))
  ACE_OSCALL (::gettimeofday (&tv, 0), int, -1, result);
# elif defined (VXWORKS) || defined (CHORUS) || defined (ACE_PSOS)
  // Assumes that struct timespec is same size as struct timeval,
  // which assumes that time_t is a long: it currently (VxWorks
  // 5.2/5.3) is.
  struct timespec ts;

  ACE_OSCALL (ACE_OS::clock_gettime (CLOCK_REALTIME, &ts), int, -1, result);
  tv.tv_sec = ts.tv_sec;
  tv.tv_usec = ts.tv_nsec / 1000L;  // timespec has nsec, but timeval has usec
# else
  ACE_OSCALL (::gettimeofday (&tv), int, -1, result);
# endif /* ACE_HAS_SVR4_GETTIMEOFDAY */
#endif /* 0 */
#if !defined (ACE_HAS_WINCE)&& !defined (ACE_WIN32)
  if (result == -1)
    return -1;
  else
    return ACE_Time_Value (tv);
#endif // !defined (ACE_HAS_WINCE)&& !defined (ACE_WIN32)
}
