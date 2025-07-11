
//=============================================================================
/**
 *  @file    Thread_Adapter.h
 *
 *  Thread_Adapter.h,v 4.8 2002/07/02 19:56:49 shuston Exp
 *
 *  @author Carlos O'Ryan <coryan@uci.edu>
 */
//=============================================================================

#ifndef ACE_THREAD_ADAPTER_H
#define ACE_THREAD_ADAPTER_H
#include "ace/pre.h"

#include "ace/ACE_export.h"

#if !defined (ACE_LACKS_PRAGMA_ONCE)
# pragma once
#endif /* ACE_LACKS_PRAGMA_ONCE */

#include "ace/Base_Thread_Adapter.h"

// Forward decl.
class ACE_Thread_Manager;
class ACE_Thread_Descriptor;

/**
 * @class ACE_Thread_Adapter
 *
 * @brief Converts a C++ function into a function that
 * can be called from a thread creation routine
 * (e.g., pthread_create() or _beginthreadex()) that expects an
 * extern "C" entry point.  This class also makes it possible to
 * transparently provide hooks to register a thread with an
 * ACE_Thread_Manager.
 *
 * This class is used in ACE_OS::thr_create().  In general, the
 * thread that creates an object of this class is different from
 * the thread that calls @c invoke() on this object.  Therefore,
 * the @c invoke() method is responsible for deleting itself.
 */
class ACE_Export ACE_Thread_Adapter : public ACE_Base_Thread_Adapter
{
public:
  /// Constructor.
  ACE_Thread_Adapter (ACE_THR_FUNC user_func,
                      void *arg,
                      ACE_THR_C_FUNC entry_point = (ACE_THR_C_FUNC) ace_thread_adapter,
                      ACE_Thread_Manager *thr_mgr = 0,
                      ACE_Thread_Descriptor *td = 0
# if defined (ACE_HAS_WIN32_STRUCTURAL_EXCEPTIONS)
                      , ACE_SEH_EXCEPT_HANDLER selector = 0,
                      ACE_SEH_EXCEPT_HANDLER handler = 0
# endif /* ACE_HAS_WIN32_STRUCTURAL_EXCEPTIONS */
                      );

  /**
   * Execute the <user_func_> with the <arg>.  This function deletes
   * <this>, thereby rendering the object useless after the call
   * returns.
   */
  virtual ACE_THR_FUNC_RETURN invoke (void);

  /// Accessor for the optional <Thread_Manager>.
  ACE_Thread_Manager *thr_mgr (void);

private:
  /// Ensure that this object must be allocated on the heap.
  ~ACE_Thread_Adapter (void);

  /// Called by invoke, mainly here to separate the SEH stuff because
  /// SEH on Win32 doesn't compile with local vars with destructors.
  virtual ACE_THR_FUNC_RETURN invoke_i (void);

private:
  /// Optional thread manager.
  ACE_Thread_Manager *thr_mgr_;

  /// Friend declaration to avoid compiler warning:  only defines a private
  /// destructor and has no friends.
  friend class ACE_Thread_Adapter_Has_Private_Destructor;
};

# if defined (ACE_HAS_INLINED_OSCALLS)
#   if defined (ACE_INLINE)
#     undef ACE_INLINE
#   endif /* ACE_INLINE */
#   define ACE_INLINE inline
#   include "ace/Thread_Adapter.inl"
# endif /* ACE_HAS_INLINED_OSCALLS */

#include "ace/post.h"
#endif /* ACE_THREAD_ADAPTER_H */
