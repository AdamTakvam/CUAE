
//=============================================================================
/**
 *  @file    Thread_Hook.h
 *
 *  Thread_Hook.h,v 4.6 2003/11/06 18:19:39 dhinton Exp
 *
 *  @author Carlos O'Ryan <coryan@uci.edu>
 */
//=============================================================================


#ifndef ACE_THREAD_HOOK_H
#define ACE_THREAD_HOOK_H
#include /**/ "ace/pre.h"

#include "ace/config-all.h"

#if !defined (ACE_LACKS_PRAGMA_ONCE)
# pragma once
#endif /* ACE_LACKS_PRAGMA_ONCE */

#include "ace/ACE_export.h"

/**
 * @class ACE_Thread_Hook
 *
 * @brief This class makes it possible to provide user-defined "start"
 * hooks that are called before the thread entry point function
 * is invoked.
 */
class ACE_Export ACE_Thread_Hook
{

public:
  /**
   * This method can be overridden in a subclass to customize this
   * pre-function call "hook" invocation that can perform
   * initialization processing before the thread entry point <func>
   * method is called back.  The @a func and @a arg passed into the
   * start hook are the same as those passed by the application that
   * spawned the thread.
   */
  virtual ACE_THR_FUNC_RETURN start (ACE_THR_FUNC func,
                                     void *arg);

  /// sets the system wide thread hook, returns the previous thread
  /// hook or 0 if none is set.
  static ACE_Thread_Hook *thread_hook (ACE_Thread_Hook *hook);

  /// Returns the current system thread hook.
  static ACE_Thread_Hook *thread_hook (void);
};

#include /**/ "ace/post.h"
#endif /* ACE_THREAD_HOOK_H */
