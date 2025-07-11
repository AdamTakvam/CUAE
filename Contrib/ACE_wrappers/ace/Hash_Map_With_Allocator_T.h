/* -*- C++ -*- */
//=============================================================================
/**
 *  @file   Hash_Map_With_Allocator_T.h
 *
 *  Hash_Map_With_Allocator_T.h,v 4.8 2002/02/18 23:55:01 shuston Exp
 *
 *  @author Marina Spivak <marina@cs.wustl.edu>
 *  @author Irfan Pyarali <irfan@cs.wustl.edu>
 */
//=============================================================================

#ifndef ACE_HASH_MAP_WITH_ALLOCATOR_T_H
#define ACE_HASH_MAP_WITH_ALLOCATOR_T_H
#include "ace/pre.h"

#include "ace/Hash_Map_Manager.h"
#include "ace/Synch.h"

#if !defined (ACE_LACKS_PRAGMA_ONCE)
# pragma once
#endif /* ACE_LACKS_PRAGMA_ONCE */

/**
 * @class ACE_Hash_Map_With_Allocator
 *
 * @brief This class is a thin wrapper around ACE_Hash_Map_Manager,
 *     which comes handy when ACE_Hash_Map_Manager is to be used with a
 *     non-nil ACE_Allocator.  This wrapper insures that the appropriate
 *     allocator is in place for every operation that accesses or
 *     updates the hash map.
 *
 *     If we use ACE_Hash_Map_Manager with a shared memory allocator
 *     (or memory-mapped file allocator, for example), the allocator
 *     pointer used by ACE_Hash_Map_Manager gets stored with it, in
 *     shared memory (or memory-mapped file).  Naturally, this will
 *     cause horrible problems, since only the first process to set
 *     that pointer will be guaranteed the address of the allocator
 *     is meaningful!  That is why we need this wrapper, which
 *     insures that appropriate allocator pointer is in place for
 *     each call.
 */
template <class EXT_ID, class INT_ID>
class ACE_Hash_Map_With_Allocator :
  public ACE_Hash_Map_Manager<EXT_ID, INT_ID, ACE_Null_Mutex>
{
public:
  /// Constructor.
  ACE_Hash_Map_With_Allocator (ACE_Allocator *alloc);

  /// Constructor that specifies hash table size.
  ACE_Hash_Map_With_Allocator (size_t size,
                               ACE_Allocator *alloc);

  // = The following methods are Proxies to the corresponding methods
  // in <ACE_Hash_Map_Manager>.  Each method sets the allocator to
  // the one specified by the invoking entity, and then calls the
  // corresponding method in <ACE_Hash_Map_Manager> to do the
  // actual work.

  int bind (const EXT_ID &,
            const INT_ID &,
            ACE_Allocator *alloc);

  int unbind (const EXT_ID &,
              INT_ID &,
              ACE_Allocator *alloc);

  int unbind (const EXT_ID &,
              ACE_Allocator *alloc);

  int rebind (const EXT_ID &,
              const INT_ID &,
              EXT_ID &,
              INT_ID &,
              ACE_Allocator *alloc);

  int find (const EXT_ID &,
            INT_ID &,
            ACE_Allocator *alloc);

  /// Returns 0 if the <ext_id> is in the mapping, otherwise -1.
  int find (const EXT_ID &,
            ACE_Allocator *alloc);

  int close (ACE_Allocator *alloc);
};

#if defined (__ACE_INLINE__)
#include "ace/Hash_Map_With_Allocator_T.i"
#endif /* __ACE_INLINE__ */

#if defined (ACE_TEMPLATES_REQUIRE_SOURCE)
#include "ace/Hash_Map_With_Allocator_T.cpp"
#endif /* ACE_TEMPLATES_REQUIRE_SOURCE */

#if defined (ACE_TEMPLATES_REQUIRE_PRAGMA)
#pragma implementation ("Hash_Map_With_Allocator_T.cpp")
#endif /* ACE_TEMPLATES_REQUIRE_PRAGMA */


#include "ace/post.h"
#endif /* ACE_HASH_MAP_WITH_ALLOCATOR_T_H */
