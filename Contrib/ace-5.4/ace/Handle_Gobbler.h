
//=============================================================================
/**
 *  @file    Handle_Gobbler.h
 *
 *  Handle_Gobbler.h,v 1.9 2003/11/01 11:15:12 dhinton Exp
 *
 *  @author Kirthika Parameswaran <kirthika@cs.wustl.edu>
 *  @author Irfan Pyarali <irfan@cs.wustl.edu>
 */
//=============================================================================


#ifndef ACE_HANDLE_GOBBLER_H
#define ACE_HANDLE_GOBBLER_H
#include /**/ "ace/pre.h"

#include "ace/Unbounded_Set.h"

#if !defined (ACE_LACKS_PRAGMA_ONCE)
# pragma once
#endif /* ACE_LACKS_PRAGMA_ONCE */

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

#include /**/ "ace/post.h"
#endif /* ACE_HANDLE_GOBBLER_H */
