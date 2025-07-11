/* -*- C++ -*- */
//=============================================================================
/**
 *  @file    Obchunk.h
 *
 *  Obchunk.h,v 4.2 2003/07/19 19:04:12 dhinton Exp
 *
 *  @author Doug Schmidt
 */
//=============================================================================


#ifndef ACE_OBCHUNK_H
#define ACE_OBCHUNK_H
#include /**/ "ace/pre.h"

#include "ace/Malloc.h"

#if !defined (ACE_LACKS_PRAGMA_ONCE)
# pragma once
#endif /* ACE_LACKS_PRAGMA_ONCE */

/**
 * @class ACE_Obchunk
 *
 * @brief Defines the state that represents a "chunk" of memory.
 *        Evenything in this class is public because it is designed
 *        as an internal structure of Obstack_T and users are
 *        not supposed to use this class directly.
 * @sa ACE_Obstack_T
 */
class ACE_Export ACE_Obchunk
{
public:
  /// Constructor.
  ACE_Obchunk (size_t size);

  /// Dtor.
  ~ACE_Obchunk (void);

  /// Dump the state of an object.
  void dump (void) const;

  /// Declare the dynamic allocation hooks.
  ACE_ALLOC_HOOK_DECLARE;

  /// Pointer to the end of the chunk.
  char *end_;

  /// Pointer to the head of the current building block.
  char *block_;

  /// Pointer to the current location in the chunk.
  char *cur_;

  /// Next chunk in the chain.
  ACE_Obchunk *next_;

  /**
   * Pointer to the beginning contents of this chunk.  This field is
   * actually overlayed by the memory allocated by
   * <ACE_Obstack::new_chunk>.  Therefore, it *must* come last.
   */
  char contents_[4];
};

#if defined (__ACE_INLINE__)
#include "ace/Obchunk.i"
#endif /* __ACE_INLINE__ */

#include /**/ "ace/post.h"
#endif /* ACE_OBCHUNK_H */
