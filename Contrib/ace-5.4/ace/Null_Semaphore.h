// -*- C++ -*-

//==========================================================================
/**
 *  @file    Null_Semaphore.h
 *
 *  Null_Semaphore.h,v 4.3 2003/12/18 17:56:22 elliott_c Exp
 *
 *   Moved from Synch.h.
 *
 *  @author Douglas C. Schmidt <schmidt@cs.wustl.edu>
 */
//==========================================================================

#ifndef ACE_NULL_SEMAPHORE_H
#define ACE_NULL_SEMAPHORE_H
#include /**/ "ace/pre.h"

// All methods in this class are inline, so there is no
// need to import or export on Windows. -- CAE 12/18/2003
#include "ace/os_include/os_errno.h"

#if !defined (ACE_LACKS_PRAGMA_ONCE)
# pragma once
#endif /* ACE_LACKS_PRAGMA_ONCE */

class ACE_Time_Value;

/**
 * @class ACE_Null_Semaphore
 *
 * @brief Implement a do nothing <ACE_Semaphore>, i.e., all the methods are
 * no ops.
 *
 * Although the methods are no-ops, the return values are different for
 * the blocking as opposed to timed acquires.  The blocking version of
 * acquire() is often used to serialize access to a critical section,
 * whereas the timed version is often used to wait for another thread
 * to update some condition or change some shared state.  When using an
 * ACE_Null_Semaphore, however, there's no other thread involved to
 * change a state or condition (otherwise, a null semaphore would be
 * inappropriate).  Returning an error value signifies that the
 * state or condition has not been (and can't be) changed, which is
 * consistent with the behavior of the threaded case where a timeout
 * occurs before the state or condition is changed.
 */
class ACE_Null_Semaphore
{
public:
  ACE_Null_Semaphore (unsigned int = 1,
                       int = 0,
                       const ACE_TCHAR * = 0,
                       void * = 0,
                       int = 0x7fffffff) {}
  ~ACE_Null_Semaphore (void) {}
  /// Return 0.
  int remove (void) {return 0;}

  /// Return 0.
  int acquire (void) {return 0;}

  /// Return -1 with <errno> == <ETIME>.
  int acquire (ACE_Time_Value &) {errno = ETIME; return -1;}

  /// Return -1 with <errno> == <ETIME>.
  int acquire (ACE_Time_Value *) {errno = ETIME; return -1;}

  /// Return 0.
  int tryacquire (void) {return 0;}

  /// Return 0.
  int release (void) {return 0;}

  /// Return 0.
  int release (size_t) {return 0;}

  /// Return 0.
  int acquire_write (void) {return 0;}

  /// Return 0.
  int tryacquire_write (void) {return 0;}

  /// Return 0.
  int tryacquire_write_upgrade (void) {return 0;}

  /// Return 0.
  int acquire_read (void) {return 0;}

  /// Return 0.
  int tryacquire_read (void) {return 0;}

  /// Dump the state of an object.
  void dump (void) const {}

  /// Declare the dynamic allocation hooks.
  //ACE_ALLOC_HOOK_DECLARE;
};

#include /**/ "ace/post.h"
#endif /* ACE_NULL_SEMAPHORE_H */
