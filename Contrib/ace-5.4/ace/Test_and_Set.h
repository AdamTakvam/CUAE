
//=============================================================================
/**
 *  @file   Test_and_Set.h
 *
 *  Test_and_Set.h,v 4.5 2003/07/19 19:04:14 dhinton Exp
 *
 *  @author Priyanka Gontla <pgontla@ece.uci.edu>
 */
//=============================================================================


#ifndef ACE_TEST_AND_SET_H
#define ACE_TEST_AND_SET_H

#include /**/ "ace/pre.h"
#include "ace/Event_Handler.h"

#if !defined (ACE_LACKS_PRAGMA_ONCE)
# pragma once
#endif /* ACE_LACKS_PRAGMA_ONCE */

/**
 * @class ACE_Test_and_Set
 *
 * @brief Implements the classic ``test and set'' operation.
 *
 *
 *     This class keeps track of the status of <is_set_>, which can
 *     be set based on various events (such as receipt of a
 *     signal).  This class is derived from <ACE_Event_Handler> so
 *     that it can be "signaled" by a Reactor when a signal occurs.
 *     We assume that <TYPE> is a data type that can be assigned the
 *     value 0 or 1.
 */
template <class ACE_LOCK, class TYPE>
class ACE_Test_and_Set : public ACE_Event_Handler
{
public:
  ACE_Test_and_Set (TYPE initial_value = 0);

  /// Returns true if we are set, else false.
  TYPE is_set (void) const;

  /// Sets the <is_set_> status, returning the original value of
  /// <is_set_>.
  TYPE set (TYPE);

  /// Called when object is signaled by OS (either via UNIX signals or
  /// when a Win32 object becomes signaled).
  virtual int handle_signal (int signum,
                             siginfo_t * = 0,
                             ucontext_t * = 0);

private:
  /// Keeps track of our state.
  TYPE is_set_;

  /// Protect the state from race conditions.
  ACE_LOCK lock_;
};

#if defined (__ACE_INLINE__)
#include "ace/Test_and_Set.i"
#endif /* __ACE_INLINE__ */

#if defined (ACE_TEMPLATES_REQUIRE_SOURCE)
#include "ace/Test_and_Set.cpp"
#endif /* ACE_TEMPLATES_REQUIRE_SOURCE */

#if defined (ACE_TEMPLATES_REQUIRE_PRAGMA)
#pragma implementation ("Test_and_Set.cpp")
#endif /* ACE_TEMPLATES_REQUIRE_PRAGMA */

#include /**/ "ace/post.h"
#endif /* ACE_TEST_AND_SET_H */
