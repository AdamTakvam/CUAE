/* -*- C++ -*- */


//=============================================================================
/**
 *  @file    System_Time.h
 *
 *  System_Time.h,v 4.17 2002/05/27 06:06:18 jwillemsen Exp
 *
 *  @author Prashant Jain
 *  @author Tim H. Harrison and Douglas C. Schmidt
 */
//=============================================================================


#ifndef ACE_SYSTEM_TIME_H
#define ACE_SYSTEM_TIME_H
#include "ace/pre.h"

#include "ace/OS.h"

#if !defined (ACE_LACKS_PRAGMA_ONCE)
# pragma once
#endif /* ACE_LACKS_PRAGMA_ONCE */

#include "ace/Memory_Pool.h"
#include "ace/Malloc_T.h"

/**
 * @class ACE_System_Time
 *
 * @brief Defines the timer services of the OS interface to access the
 * system time either on the local host or on the central time
 * server in the network.
 */
class ACE_Export ACE_System_Time
{
public:
  /**
   * Enumeration types to specify mode of synchronization with master
   * clock.  Jump will set local system time directly (thus possibly
   * producing time gaps or ambiguous local system times.  Adjust will
   * smoothly slow down or speed up the local system clock to reach
   * the system time of the master clock.
   */
  enum Sync_Mode { Jump, Adjust };

  /// Default constructor.
  ACE_System_Time (const ACE_TCHAR *poolname = 0);

  /// Default destructor.
  ~ACE_System_Time (void);

  /// Get the local system time, i.e., the value returned by
  /// <ACE_OS::time>.
  static int get_local_system_time (ACE_UINT32 &time_out);

  /// Get the local system time, i.e., the value returned by
  /// <ACE_OS::time>.
  static int get_local_system_time (ACE_Time_Value &time_out);

  /// Get the system time of the central time server.
  int get_master_system_time (ACE_UINT32 &time_out);

  /// Get the system time of the central time server.
  int get_master_system_time (ACE_Time_Value &time_out);

  /// Synchronize local system time with the central time server using
  /// specified mode.
  int sync_local_system_time (ACE_System_Time::Sync_Mode mode);

private:
  typedef ACE_Malloc <ACE_MMAP_MEMORY_POOL, ACE_Null_Mutex> MALLOC;
  typedef ACE_Allocator_Adapter<MALLOC> ALLOCATOR;

  /// Our allocator (used for obtaining system time from shared memory).
  ALLOCATOR *shmem_;

  /// The name of the pool used by the allocator.
  ACE_TCHAR poolname_[MAXPATHLEN + 1];

  /// Pointer to delta time kept in shared memory.
  long *delta_time_;
};

#include "ace/post.h"
#endif /* ACE_SYSTEM_TIME_H */
