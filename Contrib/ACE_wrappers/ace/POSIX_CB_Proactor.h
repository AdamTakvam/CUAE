/* -*- C++ -*- */

//=============================================================================
/**
 *  @file    POSIX_CB_Proactor.h
 *
 *  POSIX_CB_Proactor.h,v 4.1 2002/04/25 19:48:19 shuston Exp
 *
 *  @author Alexander Libman <alibman@ihug.com.au>
 */
//=============================================================================

#ifndef ACE_POSIX_CB_PROACTOR_H
#define ACE_POSIX_CB_PROACTOR_H

#include "ace/config-all.h"

#if !defined (ACE_LACKS_PRAGMA_ONCE)
# pragma once
#endif /* ACE_LACKS_PRAGMA_ONCE */

#if defined (ACE_HAS_AIO_CALLS) && defined (__sgi)

#include "ace/POSIX_Proactor.h"

/**
 * @class ACE_POSIX_CB_Proactor
 *
 * @brief Implementation of SGI IRIX Proactor
 * };
 */
class ACE_Export ACE_POSIX_CB_Proactor : public ACE_POSIX_AIOCB_Proactor
{

public:
  virtual Proactor_Type get_impl_type (void);

  /// Destructor.
  virtual ~ACE_POSIX_CB_Proactor (void);

  /// Constructor defines max number asynchronous operations that can
  /// be started at the same time.
  ACE_POSIX_CB_Proactor (size_t max_aio_operations = ACE_AIO_DEFAULT_SIZE);

protected:

  static void aio_completion_func ( sigval_t cb_data );

  /**
   * Dispatch a single set of events.  If <wait_time> elapses before
   * any events occur, return 0.  Return 1 on success i.e., when a
   * completion is dispatched, non-zero (-1) on errors and errno is
   * set accordingly.
   */
  virtual int handle_events (ACE_Time_Value &wait_time);

  /**
   * Dispatch a single set of events.  If <milli_seconds> elapses
   * before any events occur, return 0. Return 1 if a completion is
   * dispatched. Return -1 on errors.
   */
  virtual int handle_events (u_long milli_seconds);

  /**
   * Block indefinitely until at least one event is dispatched.
   * Dispatch a single set of events.  If <wait_time> elapses before
   * any events occur, return 0.  Return 1 on success i.e., when a
   * completion is dispatched, non-zero (-1) on errors and errno is
   * set accordingly.
   */
  virtual int handle_events (void);


  /// Check AIO for completion, error and result status
  /// Return: 1 - AIO completed , 0 - not completed yet
  virtual int get_result_status ( ACE_POSIX_Asynch_Result* asynch_result,
                                   int & error_status,
                                   int & return_status );
 

 
  /// From ACE_POSIX_AIOCB_Proactor.
  /// Attempt to cancel running request
  virtual int cancel_aiocb (ACE_POSIX_Asynch_Result *result);
  virtual int cancel_aio (ACE_HANDLE handle);

  /// Find free slot to store result and aiocb pointer
  virtual int allocate_aio_slot (ACE_POSIX_Asynch_Result *result);

 /// Notify queue of "post_completed" ACE_POSIX_Asynch_Results
  /// called from post_completion method
  virtual int notify_completion (int sig_num);

 
  /// semaphore variable to notify
  /// used to wait the first AIO start
  ACE_SYNCH_SEMAPHORE sema_;
};

#if defined (__ACE_INLINE__)
#include "ace/POSIX_CB_Proactor.i"
#endif /* __ACE_INLINE__ */

#endif /* ACE_HAS_AIO_CALLS && __sgi */
#endif /* ACE_POSIX_CB_PROACTOR_H*/
