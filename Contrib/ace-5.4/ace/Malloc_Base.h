/* -*- C++ -*- */

//=============================================================================
/**
 *  @file    Malloc_Base.h
 *
 *  Malloc_Base.h,v 4.16 2003/07/19 19:04:12 dhinton Exp
 *
 *  @author Doug Schmidt and Irfan Pyarali
 */
//=============================================================================


#ifndef ACE_MALLOC_BASE_H
#define ACE_MALLOC_BASE_H
#include /**/ "ace/pre.h"

#include "ace/ACE_export.h"

#if !defined (ACE_LACKS_PRAGMA_ONCE)
# pragma once
#endif /* ACE_LACKS_PRAGMA_ONCE */

#include "ace/os_include/sys/os_types.h"
#include "ace/os_include/sys/os_mman.h"
#include "ace/os_include/sys/os_types.h"

// The definition of this class is located in Malloc.cpp.

/**
 * @class ACE_Allocator
 *
 * @brief Interface for a dynamic memory allocator that uses inheritance
 * and dynamic binding to provide extensible mechanisms for
 * allocating and deallocating memory.
 */
class ACE_Export ACE_Allocator
{
public:
  // = Memory Management

  /// Get pointer to a default <ACE_Allocator>.
  static ACE_Allocator *instance (void);

  /// Set pointer to a process-wide <ACE_Allocator> and return existing
  /// pointer.
  static ACE_Allocator *instance (ACE_Allocator *);

  /// Delete the dynamically allocated Singleton
  static void close_singleton (void);

  /// "No-op" constructor (needed to make certain compilers happy).
  ACE_Allocator (void);

  /// Virtual destructor
  virtual ~ACE_Allocator (void);

  /// Allocate <nbytes>, but don't give them any initial value.
  virtual void *malloc (size_t nbytes) = 0;

  /// Allocate <nbytes>, giving them <initial_value>.
  virtual void *calloc (size_t nbytes, char initial_value = '\0') = 0;

  /// Allocate <n_elem> each of size <elem_size>, giving them
  /// <initial_value>.
  virtual void *calloc (size_t n_elem,
                        size_t elem_size,
                        char initial_value = '\0') = 0;

  /// Free <ptr> (must have been allocated by <ACE_Allocator::malloc>).
  virtual void free (void *ptr) = 0;

  /// Remove any resources associated with this memory manager.
  virtual int remove (void) = 0;

  // = Map manager like functions

  /**
   * Associate <name> with <pointer>.  If <duplicates> == 0 then do
   * not allow duplicate <name>/<pointer> associations, else if
   * <duplicates> != 0 then allow duplicate <name>/<pointer>
   * assocations.  Returns 0 if successfully binds (1) a previously
   * unbound <name> or (2) <duplicates> != 0, returns 1 if trying to
   * bind a previously bound <name> and <duplicates> == 0, else
   * returns -1 if a resource failure occurs.
   */
  virtual int bind (const char *name, void *pointer, int duplicates = 0) = 0;

  /**
   * Associate <name> with <pointer>.  Does not allow duplicate
   * <name>/<pointer> associations.  Returns 0 if successfully binds
   * (1) a previously unbound <name>, 1 if trying to bind a previously
   * bound <name>, or returns -1 if a resource failure occurs.  When
   * this call returns <pointer>'s value will always reference the
   * void * that <name> is associated with.  Thus, if the caller needs
   * to use <pointer> (e.g., to free it) a copy must be maintained by
   * the caller.
   */
  virtual int trybind (const char *name, void *&pointer) = 0;

  /// Locate <name> and pass out parameter via pointer.  If found,
  /// return 0, returns -1 if failure occurs.
  virtual int find (const char *name, void *&pointer) = 0;

  /// Returns 0 if the name is in the mapping. -1, otherwise.
  virtual int find (const char *name) = 0;

  /// Unbind (remove) the name from the map.  Don't return the pointer
  /// to the caller
  virtual int unbind (const char *name) = 0;

  /// Break any association of name.  Returns the value of pointer in
  /// case the caller needs to deallocate memory.
  virtual int unbind (const char *name, void *&pointer) = 0;

  // = Protection and "sync" (i.e., flushing memory to persistent
  // backing store).

  /**
   * Sync <len> bytes of the memory region to the backing store
   * starting at <this->base_addr_>.  If <len> == -1 then sync the
   * whole region.
   */
  virtual int sync (ssize_t len = -1, int flags = MS_SYNC) = 0;

  /// Sync <len> bytes of the memory region to the backing store
  /// starting at <addr_>.
  virtual int sync (void *addr, size_t len, int flags = MS_SYNC) = 0;

  /**
   * Change the protection of the pages of the mapped region to <prot>
   * starting at <this->base_addr_> up to <len> bytes.  If <len> == -1
   * then change protection of all pages in the mapped region.
   */
  virtual int protect (ssize_t len = -1, int prot = PROT_RDWR) = 0;

  /// Change the protection of the pages of the mapped region to <prot>
  /// starting at <addr> up to <len> bytes.
  virtual int protect (void *addr, size_t len, int prot = PROT_RDWR) = 0;

#if defined (ACE_HAS_MALLOC_STATS)
  /// Dump statistics of how malloc is behaving.
  virtual void print_stats (void) const = 0;
#endif /* ACE_HAS_MALLOC_STATS */

  /// Dump the state of the object.
  virtual void dump (void) const = 0;
private:
  // DO NOT ADD ANY STATE (DATA MEMBERS) TO THIS CLASS!!!!  See the
  // <ACE_Allocator::instance> implementation for explanation.

  /// Pointer to a process-wide <ACE_Allocator> instance.
  static ACE_Allocator *allocator_;

  /// Must delete the <allocator_> if non-0.
  static int delete_allocator_;
};

#include /**/ "ace/post.h"
#endif /* ACE_MALLOC_BASE_H */
