/* -*- C++ -*- */

//=============================================================================
/**
 *  @file   Connection_Recycling_Strategy.h
 *
 *  Connection_Recycling_Strategy.h,v 4.1 2001/12/10 21:44:35 bala Exp
 *
 *  @author Doug Schmidt
 */
//=============================================================================
#ifndef ACE_CONNECTION_RECYCLING_STRATEGY_H
#define ACE_CONNECTION_RECYCLING_STRATEGY_H
#include "ace/pre.h"

#include "ace/Recyclable.h"

#if !defined (ACE_LACKS_PRAGMA_ONCE)
# pragma once
#endif /* ACE_LACKS_PRAGMA_ONCE */




/**
 * @class ACE_Connection_Recycling_Strategy
 *
 * @brief Defines the interface for a connection recycler.
 */

class ACE_Export ACE_Connection_Recycling_Strategy
{
public:
  /// Virtual Destructor
  virtual ~ACE_Connection_Recycling_Strategy (void);

  /// Remove from cache.
  virtual int purge (const void *recycling_act) = 0;

  /// Add to cache.
  virtual int cache (const void *recycling_act) = 0;

  virtual int recycle_state (const void *recycling_act,
                             ACE_Recyclable_State new_state) = 0;

  /// Get/Set <recycle_state>.
  virtual ACE_Recyclable_State recycle_state (const void *recycling_act) const = 0;

  /// Mark as closed.
  virtual int mark_as_closed (const void *recycling_act) = 0;

  /// Mark as closed.(non-locking version)
  virtual int mark_as_closed_i (const void *recycling_act) = 0;

  /// Cleanup hint and reset <*act_holder> to zero if <act_holder != 0>.
  virtual int cleanup_hint (const void *recycling_act,
                            void **act_holder = 0) = 0;

protected:
  /// Default ctor.
  ACE_Connection_Recycling_Strategy (void);
};



#include "ace/post.h"
#endif /*ACE_CONNECTION_RECYCLING_STRATEGY*/
