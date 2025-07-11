// -*- C++ -*-

//==========================================================================
/**
 *  @file    Null_Barrier.h
 *
 *  Null_Barrier.h,v 4.2 2003/12/18 17:56:22 elliott_c Exp
 *
 *   Moved from Synch.h.
 *
 *  @author Douglas C. Schmidt <schmidt@cs.wustl.edu>
 */
//==========================================================================

#ifndef ACE_NULL_BARRIER_H
#define ACE_NULL_BARRIER_H
#include /**/ "ace/pre.h"

// All methods in this class are inline, so there is no
// need to import or export on Windows. -- CAE 12/18/2003

/**
 * @class ACE_Null_Barrier
 *
 * @brief Implements "NULL barrier synchronization".
 */
class ACE_Null_Barrier
{
public:
  /// Initialize the barrier to synchronize <count> threads.
  ACE_Null_Barrier (unsigned int,
                    const char * = 0,
                    void * = 0) {};

  /// Default dtor.
  ~ACE_Null_Barrier (void) {};

  /// Block the caller until all <count> threads have called <wait> and
  /// then allow all the caller threads to continue in parallel.
  int wait (void) { return 0; };

  /// Dump the state of an object.
  void dump (void) const {};

  /// Declare the dynamic allocation hooks.
  //ACE_ALLOC_HOOK_DECLARE;

private:
  // = Prevent assignment and initialization.
  void operator= (const ACE_Null_Barrier &);
  ACE_Null_Barrier (const ACE_Null_Barrier &);
};

#include /**/ "ace/post.h"
#endif /* ACE_NULL_BARRIER_H */
