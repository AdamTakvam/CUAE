// Sched_Params.cpp,v 4.30 2003/11/01 11:15:17 dhinton Exp

// ============================================================================
//
// = LIBRARY
//    ACE
//
// = FILENAME
//    Sched_Params.cpp
//
// = CREATION DATE
//    28 January 1997
//
// = AUTHOR
//    David Levine
//
// ============================================================================

#include "ace/Sched_Params.h"

#if !defined (__ACE_INLINE__)
#include "ace/Sched_Params.i"
#endif /* __ACE_INLINE__ */

#if defined (ACE_HAS_PRIOCNTL) && defined (ACE_HAS_STHREADS)
#  include "ace/OS_NS_string.h"
#  include /**/ <sys/priocntl.h>
#endif /* ACE_HAS_PRIOCNTL && ACE_HAS_THREADS */

ACE_RCSID(ace, Sched_Params, "Sched_Params.cpp,v 4.30 2003/11/01 11:15:17 dhinton Exp")

int
ACE_Sched_Params::priority_min (const Policy policy,
                                const int scope)
{
#if defined (ACE_HAS_PRIOCNTL) && defined (ACE_HAS_STHREADS)
  ACE_UNUSED_ARG (scope);

  // Assume that ACE_SCHED_OTHER indicates TS class, and that other
  // policies indicate RT class.

  // Call ACE_OS::priority_control only for processes (lightweight
  // or otherwise). Calling ACE_OS::priority_control for thread
  // priorities gives incorrect results.
  if (scope == ACE_SCOPE_PROCESS || scope == ACE_SCOPE_LWP)
    {
      if (policy == ACE_SCHED_OTHER)
        {
          // Get the priority class ID and attributes.
          pcinfo_t pcinfo;
          // The following is just to avoid Purify warnings about unitialized
          // memory reads.
          ACE_OS::memset (&pcinfo, 0, sizeof pcinfo);
          ACE_OS::strcpy (pcinfo.pc_clname, "TS");

          if (ACE_OS::priority_control (P_ALL /* ignored */,
                                        P_MYID /* ignored */,
                                        PC_GETCID,
                                        (char *) &pcinfo) == -1)
            // Just hope that priority range wasn't configured from -1
            // .. 1
            return -1;

          // OK, now we've got the class ID in pcinfo.pc_cid.  In
          // addition, the maximum configured time-share priority is in
          // ((tsinfo_t *) pcinfo.pc_clinfo)->ts_maxupri.  The minimum
          // priority is just the negative of that.

          return -((tsinfo_t *) pcinfo.pc_clinfo)->ts_maxupri;
        }
      else
        return 0;
    }
  else
    {
      // Here we handle the case for ACE_SCOPE_THREAD. Calling
      // ACE_OS::priority_control for thread scope gives incorrect
      // results.
      switch (policy)
        {
        case ACE_SCHED_FIFO:
          return ACE_THR_PRI_FIFO_MIN;
        case ACE_SCHED_RR:
          return ACE_THR_PRI_RR_MIN;
        case ACE_SCHED_OTHER:
        default:
          return ACE_THR_PRI_OTHER_MIN;
        }
    }
#elif defined (ACE_HAS_PTHREADS) && !defined(ACE_LACKS_SETSCHED)

  switch (scope)
    {
    case ACE_SCOPE_THREAD:
      switch (policy)
        {
          case ACE_SCHED_FIFO:
            return ACE_THR_PRI_FIFO_MIN;
          case ACE_SCHED_RR:
            return ACE_THR_PRI_RR_MIN;
#if !defined (CHORUS)   // SCHED_OTHRE and SCHED_RR have same value
           case ACE_SCHED_OTHER:
#endif /* CHORUS */
          default:
            return ACE_THR_PRI_OTHER_MIN;
        }

    case ACE_SCOPE_PROCESS:
    default:
      switch (policy)
        {
          case ACE_SCHED_FIFO:
            return ACE_PROC_PRI_FIFO_MIN;
          case ACE_SCHED_RR:
            return ACE_PROC_PRI_RR_MIN;
#if !defined (CHORUS)   // SCHED_OTHRE and SCHED_RR have same value
           case ACE_SCHED_OTHER:
#endif /* CHORUS */
          default:
            return ACE_PROC_PRI_OTHER_MIN;
        }
    }

#elif defined (ACE_HAS_WTHREADS)
  ACE_UNUSED_ARG (policy);
  ACE_UNUSED_ARG (scope);
  return THREAD_PRIORITY_IDLE;
#elif defined (VXWORKS)
  ACE_UNUSED_ARG (policy);
  ACE_UNUSED_ARG (scope);
  return 255;
#else
  ACE_UNUSED_ARG (policy);
  ACE_UNUSED_ARG (scope);
  ACE_NOTSUP_RETURN (-1);
#endif /* ACE_HAS_PRIOCNTL && defined (ACE_HAS_STHREADS) */
}

int
ACE_Sched_Params::priority_max (const Policy policy,
                                const int scope)
{
#if defined (ACE_HAS_PRIOCNTL) && defined (ACE_HAS_STHREADS)
  ACE_UNUSED_ARG (scope);

  // Call ACE_OS::priority_control only for processes (lightweight
  // or otherwise). Calling ACE_OS::priority_control for thread
  // priorities gives incorrect results.
  if (scope == ACE_SCOPE_PROCESS || scope == ACE_SCOPE_LWP)
    {
      // Assume that ACE_SCHED_OTHER indicates TS class, and that other
      // policies indicate RT class.

      // Get the priority class ID and attributes.
      pcinfo_t pcinfo;
      // The following is just to avoid Purify warnings about unitialized
      // memory reads.
      ACE_OS::memset (&pcinfo, 0, sizeof pcinfo);
      ACE_OS::strcpy (pcinfo.pc_clname,
                      policy == ACE_SCHED_OTHER  ?  "TS"  :  "RT");

      if (ACE_OS::priority_control (P_ALL /* ignored */,
                                    P_MYID /* ignored */,
                                    PC_GETCID,
                                    (char *) &pcinfo) == -1)
        return -1;

      // OK, now we've got the class ID in pcinfo.pc_cid.  In addition,
      // the maximum configured real-time priority is in ((rtinfo_t *)
      // pcinfo.pc_clinfo)->rt_maxpri, or similarly for the TS class.

      return policy == ACE_SCHED_OTHER
        ? ((tsinfo_t *) pcinfo.pc_clinfo)->ts_maxupri
        : ((rtinfo_t *) pcinfo.pc_clinfo)->rt_maxpri;
    }
  else
    {
      // Here we handle the case for ACE_SCOPE_THREAD. Calling
      // ACE_OS::priority_control for thread scope gives incorrect
      // results.
      switch (policy)
        {
        case ACE_SCHED_FIFO:
          return ACE_THR_PRI_FIFO_MAX;
        case ACE_SCHED_RR:
          return ACE_THR_PRI_RR_MAX;
        case ACE_SCHED_OTHER:
        default:
          return ACE_THR_PRI_OTHER_MAX;
        }
    }
#elif defined(ACE_HAS_PTHREADS) && !defined(ACE_LACKS_SETSCHED)

  switch (scope)
    {
    case ACE_SCOPE_THREAD:
      switch (policy)
        {
          case ACE_SCHED_FIFO:
            return ACE_THR_PRI_FIFO_MAX;
          case ACE_SCHED_RR:
            return ACE_THR_PRI_RR_MAX;
#if !defined (CHORUS)   // SCHED_OTHRE and SCHED_RR have same value
           case ACE_SCHED_OTHER:
#endif /* CHORUS */
          default:
            return ACE_THR_PRI_OTHER_MAX;
        }

    case ACE_SCOPE_PROCESS:
    default:
      switch (policy)
        {
          case ACE_SCHED_FIFO:
            return ACE_PROC_PRI_FIFO_MAX;
          case ACE_SCHED_RR:
            return ACE_PROC_PRI_RR_MAX;
#if !defined (CHORUS)   // SCHED_OTHRE and SCHED_RR have same value
           case ACE_SCHED_OTHER:
#endif /* CHORUS */
          default:
            return ACE_PROC_PRI_OTHER_MAX;
        }
    }

#elif defined (ACE_HAS_WTHREADS)
  ACE_UNUSED_ARG (policy);
  ACE_UNUSED_ARG (scope);
  return THREAD_PRIORITY_TIME_CRITICAL;
#elif defined (VXWORKS)
  ACE_UNUSED_ARG (policy);
  ACE_UNUSED_ARG (scope);
  return 0;
#else
  ACE_UNUSED_ARG (policy);
  ACE_UNUSED_ARG (scope);
  ACE_NOTSUP_RETURN (-1);
#endif /* ACE_HAS_PRIOCNTL && defined (ACE_HAS_STHREADS) */
}

int
ACE_Sched_Params::next_priority (const Policy policy,
                                 const int priority,
                                 const int scope)
{
#if defined (VXWORKS)
  return priority > priority_max (policy, scope)
           ?  priority - 1
           :  priority_max (policy, scope);
#elif defined (ACE_HAS_WTHREADS)
  ACE_UNUSED_ARG (policy);
  ACE_UNUSED_ARG (scope);
  switch (priority)
    {
      case THREAD_PRIORITY_IDLE:
        return THREAD_PRIORITY_LOWEST;
      case THREAD_PRIORITY_LOWEST:
        return THREAD_PRIORITY_BELOW_NORMAL;
      case THREAD_PRIORITY_BELOW_NORMAL:
        return THREAD_PRIORITY_NORMAL;
      case THREAD_PRIORITY_NORMAL:
        return THREAD_PRIORITY_ABOVE_NORMAL;
      case THREAD_PRIORITY_ABOVE_NORMAL:
        return THREAD_PRIORITY_HIGHEST;
      case THREAD_PRIORITY_HIGHEST:
        return THREAD_PRIORITY_TIME_CRITICAL;
      case THREAD_PRIORITY_TIME_CRITICAL:
        return THREAD_PRIORITY_TIME_CRITICAL;
      default:
        return priority;  // unknown priority:  should never get here
    }
#elif defined(ACE_HAS_THREADS) && !defined(ACE_LACKS_SETSCHED)
  // including STHREADS, and PTHREADS
  const int max = priority_max (policy, scope);
  return priority < max  ?  priority + 1  :  max;
#else
  ACE_UNUSED_ARG (policy);
  ACE_UNUSED_ARG (scope);
  ACE_UNUSED_ARG (priority);
  ACE_NOTSUP_RETURN (-1);
#endif /* ACE_HAS_THREADS */
}

int
ACE_Sched_Params::previous_priority (const Policy policy,
                                     const int priority,
                                     const int scope)
{
#if defined (VXWORKS)
  return priority < priority_min (policy, scope)
           ?  priority + 1
           :  priority_min (policy, scope);
#elif defined (ACE_HAS_WTHREADS)
  ACE_UNUSED_ARG (policy);
  ACE_UNUSED_ARG (scope);
  switch (priority)
    {
      case THREAD_PRIORITY_IDLE:
        return THREAD_PRIORITY_IDLE;
      case THREAD_PRIORITY_LOWEST:
        return THREAD_PRIORITY_IDLE;
      case THREAD_PRIORITY_BELOW_NORMAL:
        return THREAD_PRIORITY_LOWEST;
      case THREAD_PRIORITY_NORMAL:
        return THREAD_PRIORITY_BELOW_NORMAL;
      case THREAD_PRIORITY_ABOVE_NORMAL:
        return THREAD_PRIORITY_NORMAL;
      case THREAD_PRIORITY_HIGHEST:
        return THREAD_PRIORITY_ABOVE_NORMAL;
      case THREAD_PRIORITY_TIME_CRITICAL:
        return THREAD_PRIORITY_HIGHEST;
      default:
        return priority;  // unknown priority:  should never get here
    }
#elif defined (ACE_HAS_THREADS) && !defined(ACE_LACKS_SETSCHED)
  // including STHREADS and PTHREADS
  const int min = priority_min (policy, scope);

  return priority > min ? priority - 1 : min;
#else
  ACE_UNUSED_ARG (policy);
  ACE_UNUSED_ARG (scope);
  ACE_UNUSED_ARG (priority);
  ACE_NOTSUP_RETURN (-1);
#endif /* ACE_HAS_THREADS */
}
