
//=============================================================================
/**
 *  @file    Handle_Gobbler.h
 *
 *  Handle_Gobbler.h,v 1.7 2001/10/28 19:15:11 mk1 Exp
 *
 *  @author Kirthika Parameswaran <kirthika@cs.wustl.edu>
 *  @author Irfan Pyarali <irfan@cs.wustl.edu>
 */
//=============================================================================


#ifndef ACE_HANDLE_GOBBLER_H
#define ACE_HANDLE_GOBBLER_H
#include "ace/pre.h"

#include "ace/OS.h"

#if !defined (ACE_LACKS_PRAGMA_ONCE)
# pragma once
#endif /* ACE_LACKS_PRAGMA_ONCE */

#include "ace/Unbounded_Set.h"

/**
 * @class ACE_Handle_Gobbler
 *
 * @brief This class gobbles up handles.
 *
 * This is useful when we need to control the number of handles
 * available for a process.  This class is mostly used for
 * testing purposes.
 */
class ACE_Handle_Gobbler
{
public:

  /// Destructor.  Cleans up any remaining handles.
  inline ~ACE_Handle_Gobbler (void);

  /**
   * Handles are opened continously until the process runs out of
   * them, and then <n_handles_to_keep_available> handles are closed
   * (freed) thereby making them usable in the future.
   */
  inline int consume_handles (size_t n_handles_to_keep_available);

  /// Free up <n_handles>.
  inline int free_handles (size_t n_handles);

  /// All remaining handles are closed.
  inline void close_remaining_handles (void);

private:

  typedef ACE_Unbounded_Set<ACE_HANDLE> HANDLE_SET;

  /// The container which holds the open descriptors.
  HANDLE_SET handle_set_;
};

#include "ace/Handle_Gobbler.i"

#include "ace/post.h"
#endif /* ACE_HANDLE_GOBBLER_H */
