/* -*- C++ -*- */
//=============================================================================
/**
 *  @file    Obstack_T.h
 *
 *  Obstack_T.h,v 4.2 2002/06/13 14:51:05 schmidt Exp
 *
 *  @author Doug Schmidt <schmidt@cs.wustl.edu> and Nanbor Wang <nanbor@cs.wustl.edu>
 */
//=============================================================================

#ifndef ACE_OBSTACK_T_H
#define ACE_OBSTACK_T_H
#include "ace/pre.h"

#include "ace/Obchunk.h"

#if !defined (ACE_LACKS_PRAGMA_ONCE)
# pragma once
#endif /* ACE_LACKS_PRAGMA_ONCE */


/**
 * @class ACE_Obstack
 *
 * @brief Define a simple "mark and release" memory allocation utility.
 *
 * The implementation is similar to the GNU obstack utility,
 * which is used extensively in the GCC compiler.
 */
template <class CHAR>
class ACE_Obstack_T
{
public:
  // = Initialization and termination methods.
  ACE_Obstack_T (size_t size = (4096 * sizeof (CHAR)) - sizeof (ACE_Obchunk),
                 ACE_Allocator *allocator_strategy = 0);
  ~ACE_Obstack_T (void);

  /// Request Obstack to prepare a block at least @a len long for building
  /// a new string.  Return -1 if fail, 0 if success.
  int request (size_t len);

  /// Inserting a new CHAR \a c into the current building
  /// block without freezing (null terminating) the block.
  /// This function will create new chunk by checking the
  /// boundary of current Obchunk.  Return
  /// the location \a c gets inserted to, or 0 if error.
  CHAR *grow (CHAR c);

  /// Inserting a new CHAR \a c into the current building
  /// block without freezing (null terminating) the block and without
  /// checking for out-of-bound error.
  void grow_fast (CHAR c);

  /// Freeze the current building block by null terminating it.
  /// Return the starting address of the current building block, 0
  /// if error occurs.
  CHAR *freeze (void);

  /// Copy the data into the current Obchunk and freeze the current
  /// block.  Return the starting address of the current building
  /// block, 0 if error occurs.  @a len specify the string length,
  /// not the actually data size.
  CHAR *copy (const CHAR *data,
              size_t len);

  /// Return the maximum @a length or @a size of a string that can be put into
  /// this Obstack.  @a size = @a length * sizeof (CHAR).
  size_t length (void) const;
  size_t size (void) const;

  /// "Release" the entire stack of Obchunks, putting it back on the
  /// free list.
  void release (void);

  /// Dump the state of an object.
  void dump (void) const;

  /// Declare the dynamic allocation hooks.
  ACE_ALLOC_HOOK_DECLARE;

protected:
  class ACE_Obchunk *new_chunk (void);

  /// Pointer to the allocator used by this Obstack.
  ACE_Allocator *allocator_strategy_;

  /// Current size of the Obstack;
  size_t size_;

  // Don't change the order of the following two fields.
  /// Head of the Obchunk chain.
  class ACE_Obchunk *head_;

  /// Pointer to the current Obchunk.
  class ACE_Obchunk *curr_;
};

#if defined (__ACE_INLINE__)
#include "ace/Obstack_T.i"
#endif /* __ACE_INLINE__ */

#if defined (ACE_TEMPLATES_REQUIRE_SOURCE)
#include "ace/Obstack_T.cpp"
#endif /* ACE_TEMPLATES_REQUIRE_SOURCE */

#if defined (ACE_TEMPLATES_REQUIRE_PRAGMA)
#pragma implementation ("Obstack_T.cpp")
#endif /* ACE_TEMPLATES_REQUIRE_PRAGMA */

#include "ace/post.h"
#endif /* ACE_OBSTACK_T_H */
