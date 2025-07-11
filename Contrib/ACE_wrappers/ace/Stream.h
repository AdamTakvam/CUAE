// -*- C++ -*-

//==========================================================================
/**
 *  @file    Stream.h
 *
 *  Stream.h,v 4.27 2002/05/02 04:08:14 ossama Exp
 *
 *  @author Douglas C. Schmidt <schmidt@uci.edu>
 */
//==========================================================================

#ifndef ACE_STREAM_H
#define ACE_STREAM_H

#include "ace/pre.h"

#include "ace/config-all.h"

#if !defined (ACE_LACKS_PRAGMA_ONCE)
# pragma once
#endif /* ACE_LACKS_PRAGMA_ONCE */

#include "ace/IO_Cntl_Msg.h"
#include "ace/Message_Block.h"
#include "ace/Time_Value.h"
#include "ace/Module.h"

// Forward decls.
template<ACE_SYNCH_DECL> class ACE_Stream_Iterator;

/**
 * @class ACE_Stream
 *
 * @brief This class is the primary abstraction for the ASX framework.
 * It is moduled after System V Stream.
 *
 * A Stream consists of a stack of <ACE_Modules>, each of which
 * contains two <ACE_Tasks>.  Even though the methods in this
 * class are virtual, this class isn't really intended for
 * subclassing unless you know what you are doing.  In
 * particular, the <ACE_Stream> destructor calls <close>, which
 * won't be overridden properly unless you call it in a subclass
 * destructor.
 */
template <ACE_SYNCH_DECL>
class ACE_Stream
{
public:
  friend class ACE_Stream_Iterator<ACE_SYNCH_USE>;

  enum
  {
    /// Indicates that <close> deletes the Tasks.  Don't change this
    /// value without updating the same enum in class ACE_Module...
    M_DELETE = 3
  };

  // = Initializatation and termination methods.
  /**
   * Create a Stream consisting of <head> and <tail> as the Stream
   * head and Stream tail, respectively.  If these are 0 then the
   * <ACE_Stream_Head> and <ACE_Stream_Tail> are used, respectively.
   * <arg> is the value past in to the <open> methods of the tasks.
   */
  ACE_Stream (void *arg = 0,
              ACE_Module<ACE_SYNCH_USE> *head = 0,
              ACE_Module<ACE_SYNCH_USE> *tail = 0);

  /**
   * Create a Stream consisting of <head> and <tail> as the Stream
   * head and Stream tail, respectively.  If these are 0 then the
   * <ACE_Stream_Head> and <ACE_Stream_Tail> are used, respectively.
   * <arg> is the value past in to the <open> methods of the tasks.
   */
  virtual int open (void *arg,
                    ACE_Module<ACE_SYNCH_USE> *head = 0,
                    ACE_Module<ACE_SYNCH_USE> *tail = 0);

  /// Close down the stream and release all the resources.
  virtual int close (int flags = M_DELETE);

  /// Close down the stream and release all the resources.
  virtual ~ACE_Stream (void);

  // = ACE_Stream plumbing operations

  /// Add a new module <mod> right below the Stream head.
  virtual int push (ACE_Module<ACE_SYNCH_USE> *mod);

  /// Remove the <mod> right below the Stream head and close it down.
  virtual int pop (int flags = M_DELETE);

  /// Return the top module on the stream (right below the stream
  /// head).
  virtual int top (ACE_Module<ACE_SYNCH_USE> *&mod);

  /// Insert a new module <mod> below the named module <prev_name>.
  virtual int insert (const ACE_TCHAR *prev_name,
                      ACE_Module<ACE_SYNCH_USE> *mod);

  /// Replace the named module <replace_name> with a new module <mod>.
  virtual int replace (const ACE_TCHAR *replace_name,
                       ACE_Module<ACE_SYNCH_USE> *mod,
                       int flags = M_DELETE);

  /// Remove the named module <mod> from the stream.  This bypasses the
  /// strict LIFO ordering of <push> and <pop>.
  virtual int remove (const ACE_TCHAR *mod,
                      int flags = M_DELETE);

  /// Return current stream head.
  virtual ACE_Module<ACE_SYNCH_USE> *head (void);

  /// Return current stream tail.
  virtual ACE_Module<ACE_SYNCH_USE> *tail (void);

  /// Find a particular ACE_Module.
  virtual ACE_Module<ACE_SYNCH_USE> *find (const ACE_TCHAR *mod);

  /// Create a pipe between two Streams.
  virtual int link (ACE_Stream<ACE_SYNCH_USE> &);

  /// Remove a pipe formed between two Streams.
  virtual int unlink (void);

  // = Blocking data transfer operations
  /**
   * Send the message <mb> down the stream, starting at the Module
   * below the Stream head.  Wait for upto <timeout> amount of
   * absolute time for the operation to complete (or block forever if
   * <timeout> == 0).
   */
  virtual int put (ACE_Message_Block *mb,
                   ACE_Time_Value *timeout = 0);

  /**
   * Read the message <mb> that is stored in the stream head.
   * Wait for upto <timeout> amount of absolute time for the operation
   * to complete (or block forever if <timeout> == 0).
   */
  virtual int get (ACE_Message_Block *&mb,
                   ACE_Time_Value *timeout = 0);

  /// Send control message down the stream.
  virtual int control (ACE_IO_Cntl_Msg::ACE_IO_Cntl_Cmds cmd,
                       void *args);

  /// Synchronize with the final close of the stream.
  virtual int wait (void);

  /// Dump the state of an object.
  virtual void dump (void) const;

  /// Declare the dynamic allocation hooks.
  ACE_ALLOC_HOOK_DECLARE;

private:
  /// Actually perform the unlinking of two Streams (must be called
  /// with locks held).
  int unlink_i (void);

  /// Actually perform the linking of two Streams (must be called with
  /// locks held).
  int link_i (ACE_Stream<ACE_SYNCH_USE> &);

  /// Must a new module onto the Stream.
  int push_module (ACE_Module<ACE_SYNCH_USE> *,
                   ACE_Module<ACE_SYNCH_USE> * = 0,
                   ACE_Module<ACE_SYNCH_USE> * = 0);

  /// Pointer to the head of the stream.
  ACE_Module<ACE_SYNCH_USE> *stream_head_;

  /// Pointer to the tail of the stream.
  ACE_Module<ACE_SYNCH_USE> *stream_tail_;

  /// Pointer to an adjoining linked stream.
  ACE_Stream<ACE_SYNCH_USE> *linked_us_;

  // = Synchronization objects used for thread-safe streams.
  /// Protect the stream against race conditions.
  ACE_SYNCH_MUTEX_T lock_;

  /// Use to tell all threads waiting on the close that we are done.
  ACE_SYNCH_CONDITION_T final_close_;
};

/**
 * @class ACE_Stream_Iterator
 *
 * @brief Iterate through an <ACE_Stream>.
 */
template <ACE_SYNCH_DECL>
class ACE_Stream_Iterator
{
public:
  // = Initialization method.
  ACE_Stream_Iterator (const ACE_Stream<ACE_SYNCH_USE> &sr);

  // = Iteration methods.

  /// Pass back the <next_item> that hasn't been seen in the set.
  /// Returns 0 when all items have been seen, else 1.
  int next (const ACE_Module<ACE_SYNCH_USE> *&next_item);

  /// Returns 1 when all items have been seen, else 0.
  int done (void) const;

  /// Move forward by one element in the set.  Returns 0 when all the
  /// items in the set have been seen, else 1.
  int advance (void);

private:
  /// Next <Module> that we haven't yet seen.
  ACE_Module<ACE_SYNCH_USE> *next_;
};

#if defined (__ACE_INLINE__)
#include "ace/Stream.i"
#endif /* __ACE_INLINE__ */

#if defined (ACE_TEMPLATES_REQUIRE_SOURCE)
#include "ace/Stream.cpp"
#endif /* ACE_TEMPLATES_REQUIRE_SOURCE */

#if defined (ACE_TEMPLATES_REQUIRE_PRAGMA)
#pragma implementation ("Stream.cpp")
#endif /* ACE_TEMPLATES_REQUIRE_PRAGMA */

#include "ace/post.h"

#endif /* ACE_STREAM_H */
