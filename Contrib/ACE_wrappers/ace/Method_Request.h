// -*- C++ -*-

//=============================================================================
/**
 *  @file    Method_Request.h
 *
 *  Method_Request.h,v 4.12 2002/04/17 22:42:55 shuston Exp
 *
 *  @author Andres Kruse <Andres.Kruse@cern.ch>
 *  @author Douglas C. Schmidt <schmidt@cs.wustl.edu>
 */
//=============================================================================


#ifndef ACE_METHOD_REQUEST_H
#define ACE_METHOD_REQUEST_H

#include "ace/pre.h"

#include "ace/ACE_export.h"

#if !defined (ACE_LACKS_PRAGMA_ONCE)
# pragma once
#endif /* ACE_LACKS_PRAGMA_ONCE */

#include "Global_Macros.h"

/**
 * @class ACE_Method_Request
 *
 * @brief Reifies a method into a request.  Subclasses must provide
 * the necessary state and behavior.
 *
 * An ACE_Method_Request is inserted in an ACE_Activation_Queue,
 * where it is subsequently removed by a scheduler object (often
 * derived from ACE_Task), which invokes the @c call() method.
 *
 * This class is discussed in depth in the Active Object chapter
 * of POSA2.
 *
 * @sa ACE_Activation_Queue
 */
class ACE_Export ACE_Method_Request
{
public:
  // = Initialization and termination methods.
  /// Constructor.
  ACE_Method_Request (unsigned long priority = 0);

  /// Destructor.
  virtual ~ACE_Method_Request (void);

  // = Accessors.
  /// Get priority.
  unsigned long priority (void) const;

  /// Set priority.
  /**
   * Priority values are user-defined. The default (set in the constructor)
   * is 0. The priority value is used in the ACE_Activation_Queue::enqueue()
   * method to order the method requests in the queue by priority.
   * 0 is the lowest priority.
   *
   * @arg prio   unsigned long, the new priority value for this object.
   *
   * @sa ACE_Activation_Queue::enqueue
   */
  void priority (unsigned long);

  // = Invocation method (must be overridden by subclasses).
  /// Invoked by the scheduler to execute the request.
  /**
   * This method must be implemented by the subclass to perform the
   * desired actions.
   *
   * @return int; not interpreted by ACE. The scheduler class must
   *         decide the meaning of this return value and act on it
   *         if needed.
   */
  virtual int call (void) = 0;

protected:
  /// The priority of the request.
  unsigned long priority_;

private:
  ACE_UNIMPLEMENTED_FUNC (void operator= (const ACE_Method_Request &))
  ACE_UNIMPLEMENTED_FUNC (ACE_Method_Request (const ACE_Method_Request &))
};


#include "ace/post.h"
#endif /* ACE_METHOD_REQUEST_H */
