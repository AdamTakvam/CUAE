// TSS_T.cpp,v 4.9 2003/11/12 00:16:13 bala Exp

#ifndef ACE_TSS_T_C
#define ACE_TSS_T_C

#include "ace/TSS_T.h"

#if !defined (ACE_LACKS_PRAGMA_ONCE)
# pragma once
#endif /* ACE_LACKS_PRAGMA_ONCE */

ACE_RCSID(ace, TSS_T, "TSS_T.cpp,v 4.9 2003/11/12 00:16:13 bala Exp")

#if !defined (__ACE_INLINE__)
#include "ace/TSS_T.inl"
#endif /* __ACE_INLINE__ */

#include "ace/Thread.h"
#include "ace/Log_Msg.h"
#include "ace/Guard_T.h"
#include "ace/OS_NS_stdio.h"

#if defined (ACE_HAS_THR_C_DEST)
#  include "ace/TSS_Adapter.h"
#endif /* ACE_HAS_THR_C_DEST */

ACE_ALLOC_HOOK_DEFINE(ACE_TSS)

template <class TYPE>
ACE_TSS<TYPE>::~ACE_TSS (void)
{
  // We can't call <ACE_OS::thr_keyfree> until *all* of the threads
  // that are using that key have done an <ACE_OS::thr_key_detach>.
  // Otherwise, we'll end up with "dangling TSS pointers."
  ACE_OS::thr_key_detach (this);
}

template <class TYPE> TYPE *
ACE_TSS<TYPE>::operator-> () const
{
  return this->ts_get ();
}

template <class TYPE>
ACE_TSS<TYPE>::operator TYPE *(void) const
{
  return this->ts_get ();
}

template <class TYPE> TYPE *
ACE_TSS<TYPE>::make_TSS_TYPE (void) const
{
  TYPE *temp = 0;
  ACE_NEW_RETURN (temp,
                  TYPE,
                  0);
  return temp;
}

template <class TYPE> void
ACE_TSS<TYPE>::dump (void) const
{
#if defined (ACE_HAS_DUMP)
// ACE_TRACE ("ACE_TSS<TYPE>::dump");
#if defined (ACE_HAS_THREADS) && (defined (ACE_HAS_THREAD_SPECIFIC_STORAGE) || defined (ACE_HAS_TSS_EMULATION))
  ACE_DEBUG ((LM_DEBUG, ACE_BEGIN_DUMP, this));
  this->keylock_.dump ();
  ACE_DEBUG ((LM_DEBUG, ACE_LIB_TEXT ("key_ = %d\n"), this->key_));
  ACE_DEBUG ((LM_DEBUG, ACE_LIB_TEXT ("\nonce_ = %d"), this->once_));
  ACE_DEBUG ((LM_DEBUG, ACE_LIB_TEXT ("\n")));
  ACE_DEBUG ((LM_DEBUG, ACE_END_DUMP));
#endif /* defined (ACE_HAS_THREADS) && (defined (ACE_HAS_THREAD_SPECIFIC_STORAGE) || defined (ACE_HAS_TSS_EMULATION)) */
#endif /* ACE_HAS_DUMP */
}

#if defined (ACE_HAS_THREADS) && (defined (ACE_HAS_THREAD_SPECIFIC_STORAGE) || defined (ACE_HAS_TSS_EMULATION))
#if defined (ACE_HAS_THR_C_DEST)
extern "C" void ACE_TSS_C_cleanup(void *); // defined in Synch.cpp
#endif /* ACE_HAS_THR_C_DEST */

template <class TYPE> void
ACE_TSS<TYPE>::cleanup (void *ptr)
{
  // Cast this to the concrete TYPE * so the destructor gets called.
  delete (TYPE *) ptr;
}

template <class TYPE> int
ACE_TSS<TYPE>::ts_init (void) const
{
  // Insure that we are serialized!
  ACE_GUARD_RETURN (ACE_Thread_Mutex, ace_mon, (ACE_Thread_Mutex &) this->keylock_, 0);

  // Use the Double-Check pattern to make sure we only create the key
  // once!
  if (this->once_ == 0)
    {
      if (ACE_Thread::keycreate (ACE_const_cast (ACE_thread_key_t *, &this->key_),
#if defined (ACE_HAS_THR_C_DEST)
                                 &ACE_TSS_C_cleanup,
#else
                                 &ACE_TSS<TYPE>::cleanup,
#endif /* ACE_HAS_THR_C_DEST */
                                 (void *) this) != 0)
        return -1; // Major problems, this should *never* happen!
      else
        {
          // This *must* come last to avoid race conditions!  Note that
          // we need to "cast away const..."
          * ACE_const_cast (int*, &this->once_) = 1;
          return 0;
        }
    }

  return 0;
}

template <class TYPE>
ACE_TSS<TYPE>::ACE_TSS (TYPE *ts_obj)
  : once_ (0),
    key_ (ACE_OS::NULL_key)
{
  // If caller has passed us a non-NULL TYPE *, then we'll just use
  // this to initialize the thread-specific value.  Thus, subsequent
  // calls to operator->() will return this value.  This is useful
  // since it enables us to assign objects to thread-specific data
  // that have arbitrarily complex constructors!

  if (ts_obj != 0)
    {
      if (this->ts_init () == -1)
        {
          // Save/restore errno.
          ACE_Errno_Guard error (errno);
          // What should we do if this call fails?!
#if defined (ACE_HAS_WINCE)
          ::MessageBox (0,
                        L"ACE_Thread::keycreate() failed!",
                        L"ACE_TSS::ACE_TSS",
                        MB_OK);
#else
          ACE_OS::fprintf (stderr,
                           "ACE_Thread::keycreate() failed!");
#endif /* ACE_HAS_WINCE */
          return;
        }

#if defined (ACE_HAS_THR_C_DEST)
      // Encapsulate a ts_obj and it's destructor in an
      // ACE_TSS_Adapter.
      ACE_TSS_Adapter *tss_adapter;
      ACE_NEW (tss_adapter,
               ACE_TSS_Adapter ((void *) ts_obj,
                                ACE_TSS<TYPE>::cleanup));

      // Put the adapter in thread specific storage
      if (ACE_Thread::setspecific (this->key_,
                                   (void *) tss_adapter) != 0)
        {
          delete tss_adapter;
          ACE_ERROR ((LM_ERROR,
                      ACE_LIB_TEXT ("%p\n"),
                      ACE_LIB_TEXT ("ACE_Thread::setspecific() failed!")));
        }
#else
      if (ACE_Thread::setspecific (this->key_,
                                   (void *) ts_obj) != 0)
        ACE_ERROR ((LM_ERROR,
                    ACE_LIB_TEXT ("%p\n"),
                    ACE_LIB_TEXT ("ACE_Thread::setspecific() failed!")));
#endif /* ACE_HAS_THR_C_DEST */
    }
}

template <class TYPE> TYPE *
ACE_TSS<TYPE>::ts_get (void) const
{
  if (this->once_ == 0)
    {
      // Create and initialize thread-specific ts_obj.
      if (this->ts_init () == -1)
        // Seriously wrong..
        return 0;
    }

  TYPE *ts_obj = 0;

#if defined (ACE_HAS_THR_C_DEST)
  ACE_TSS_Adapter *tss_adapter = 0;

  // Get the adapter from thread-specific storage
  if (ACE_Thread::getspecific (this->key_,
                               (void **) &tss_adapter) == -1)
    return 0; // This should not happen!

  // Check to see if this is the first time in for this thread.
  if (tss_adapter == 0)
#else
  // Get the ts_obj from thread-specific storage.  Note that no locks
  // are required here...
  if (ACE_Thread::getspecific (this->key_,
                               (void **) &ts_obj) == -1)
    return 0; // This should not happen!

  // Check to see if this is the first time in for this thread.
  if (ts_obj == 0)
#endif /* ACE_HAS_THR_C_DEST */
    {
      // Allocate memory off the heap and store it in a pointer in
      // thread-specific storage (on the stack...).

      ts_obj = this->make_TSS_TYPE ();

      if (ts_obj == 0)
        return 0;

#if defined (ACE_HAS_THR_C_DEST)
      // Encapsulate a ts_obj and it's destructor in an
      // ACE_TSS_Adapter.
      ACE_NEW_RETURN (tss_adapter,
                      ACE_TSS_Adapter (ts_obj,
                                       ACE_TSS<TYPE>::cleanup), 0);

      // Put the adapter in thread specific storage
      if (ACE_Thread::setspecific (this->key_,
                                   (void *) tss_adapter) != 0)
        {
          delete tss_adapter;
          delete ts_obj;
          return 0; // Major problems, this should *never* happen!
        }
#else
      // Store the dynamically allocated pointer in thread-specific
      // storage.
      if (ACE_Thread::setspecific (this->key_,
                                   (void *) ts_obj) != 0)
        {
          delete ts_obj;
          return 0; // Major problems, this should *never* happen!
        }
#endif /* ACE_HAS_THR_C_DEST */
    }

#if defined (ACE_HAS_THR_C_DEST)
  // Return the underlying ts object.
  return (TYPE *) tss_adapter->ts_obj_;
#else
  return ts_obj;
#endif /* ACE_HAS_THR_C_DEST */
}

// Get the thread-specific object for the key associated with this
// object.  Returns 0 if the ts_obj has never been initialized,
// otherwise returns a pointer to the ts_obj.

template <class TYPE> TYPE *
ACE_TSS<TYPE>::ts_object (void) const
{
  // Ensure that we are serialized!
  ACE_GUARD_RETURN (ACE_Thread_Mutex, ace_mon, (ACE_Thread_Mutex &) this->keylock_, 0);

  if (this->once_ == 0) // Return 0 if we've never been initialized.
    return 0;

  TYPE *ts_obj = 0;

#if defined (ACE_HAS_THR_C_DEST)
  ACE_TSS_Adapter *tss_adapter = 0;

  // Get the tss adapter from thread-specific storage
  if (ACE_Thread::getspecific (this->key_,
                               (void **) &tss_adapter) == -1)
    return 0; // This should not happen!
  else if (tss_adapter != 0)
    // Extract the real TS object.
    ts_obj = (TYPE *) tss_adapter->ts_obj_;
#else
  if (ACE_Thread::getspecific (this->key_,
                               (void **) &ts_obj) == -1)
    return 0; // This should not happen!
#endif /* ACE_HAS_THR_C_DEST */

  return ts_obj;
}

template <class TYPE> TYPE *
ACE_TSS<TYPE>::ts_object (TYPE *new_ts_obj)
{
  // Note, we shouldn't hold the keylock at this point because
  // <ts_init> does it for us and we'll end up with deadlock
  // otherwise...
  if (this->once_ == 0)
    {
      // Create and initialize thread-specific ts_obj.
      if (this->ts_init () == -1)
        return 0;
    }

  // Ensure that we are serialized!
  ACE_GUARD_RETURN (ACE_Thread_Mutex, ace_mon, this->keylock_, 0);

  TYPE *ts_obj = 0;

#if defined (ACE_HAS_THR_C_DEST)
  ACE_TSS_Adapter *tss_adapter = 0;

  if (ACE_Thread::getspecific (this->key_,
                               (void **) &tss_adapter) == -1)
    return 0; // This should not happen!

  if (tss_adapter != 0)
    {
      ts_obj = (TYPE *) tss_adapter->ts_obj_;
      delete tss_adapter;       // don't need this anymore
    }

  ACE_NEW_RETURN (tss_adapter,
                  ACE_TSS_Adapter ((void *) new_ts_obj,
                                   ACE_TSS<TYPE>::cleanup),
                  0);

  if (ACE_Thread::setspecific (this->key_,
                               (void *) tss_adapter) == -1)
    {
      delete tss_adapter;
      return ts_obj; // This should not happen!
    }
#else
  if (ACE_Thread::getspecific (this->key_,
                               (void **) &ts_obj) == -1)
    return 0; // This should not happen!
  if (ACE_Thread::setspecific (this->key_,
                               (void *) new_ts_obj) == -1)
    return ts_obj; // This should not happen!
#endif /* ACE_HAS_THR_C_DEST */

  return ts_obj;
}

ACE_ALLOC_HOOK_DEFINE(ACE_TSS_Guard)

template <class ACE_LOCK> void
ACE_TSS_Guard<ACE_LOCK>::dump (void) const
{
#if defined (ACE_HAS_DUMP)
// ACE_TRACE ("ACE_TSS_Guard<ACE_LOCK>::dump");

  ACE_DEBUG ((LM_DEBUG, ACE_BEGIN_DUMP, this));
  ACE_DEBUG ((LM_DEBUG, ACE_LIB_TEXT ("key_ = %d"), this->key_));
  ACE_DEBUG ((LM_DEBUG, ACE_LIB_TEXT ("\n")));
  ACE_DEBUG ((LM_DEBUG, ACE_END_DUMP));
#endif /* ACE_HAS_DUMP */
}

template <class ACE_LOCK> void
ACE_TSS_Guard<ACE_LOCK>::init_key (void)
{
// ACE_TRACE ("ACE_TSS_Guard<ACE_LOCK>::init_key");

  this->key_ = ACE_OS::NULL_key;
  ACE_Thread::keycreate (&this->key_,
#if defined (ACE_HAS_THR_C_DEST)
                         &ACE_TSS_C_cleanup,
#else
                         &ACE_TSS_Guard<ACE_LOCK>::cleanup,
#endif /* ACE_HAS_THR_C_DEST */
                         (void *) this);
}

template <class ACE_LOCK>
ACE_TSS_Guard<ACE_LOCK>::ACE_TSS_Guard (void)
{
// ACE_TRACE ("ACE_TSS_Guard<ACE_LOCK>::ACE_TSS_Guard");
  this->init_key ();
}

template <class ACE_LOCK> int
ACE_TSS_Guard<ACE_LOCK>::release (void)
{
// ACE_TRACE ("ACE_TSS_Guard<ACE_LOCK>::release");

  ACE_Guard<ACE_LOCK> *guard = 0;

#if defined (ACE_HAS_THR_C_DEST)
  ACE_TSS_Adapter *tss_adapter = 0;
  ACE_Thread::getspecific (this->key_,
                           (void **) &tss_adapter);
  guard = (ACE_Guard<ACE_LOCK> *)tss_adapter->ts_obj_;
#else
  ACE_Thread::getspecific (this->key_,
                           (void **) &guard);
#endif /* ACE_HAS_THR_C_DEST */

  return guard->release ();
}

template <class ACE_LOCK> int
ACE_TSS_Guard<ACE_LOCK>::remove (void)
{
// ACE_TRACE ("ACE_TSS_Guard<ACE_LOCK>::remove");

  ACE_Guard<ACE_LOCK> *guard = 0;

#if defined (ACE_HAS_THR_C_DEST)
  ACE_TSS_Adapter *tss_adapter = 0;
  ACE_Thread::getspecific (this->key_,
                           (void **) &tss_adapter);
  guard = (ACE_Guard<ACE_LOCK> *) tss_adapter->ts_obj_;
#else
  ACE_Thread::getspecific (this->key_,
                           (void **) &guard);
#endif /* ACE_HAS_THR_C_DEST */

  return guard->remove ();
}

template <class ACE_LOCK>
ACE_TSS_Guard<ACE_LOCK>::~ACE_TSS_Guard (void)
{
// ACE_TRACE ("ACE_TSS_Guard<ACE_LOCK>::~ACE_TSS_Guard");

  ACE_Guard<ACE_LOCK> *guard = 0;

#if defined (ACE_HAS_THR_C_DEST)
  ACE_TSS_Adapter *tss_adapter = 0;
  ACE_Thread::getspecific (this->key_,
                           (void **) &tss_adapter);
  guard = (ACE_Guard<ACE_LOCK> *) tss_adapter->ts_obj_;
#else
  ACE_Thread::getspecific (this->key_,
                           (void **) &guard);
#endif /* ACE_HAS_THR_C_DEST */

  // Make sure that this pointer is NULL when we shut down...
  ACE_Thread::setspecific (this->key_, 0);
  ACE_Thread::keyfree (this->key_);
  // Destructor releases lock.
  delete guard;
}

template <class ACE_LOCK> void
ACE_TSS_Guard<ACE_LOCK>::cleanup (void *ptr)
{
// ACE_TRACE ("ACE_TSS_Guard<ACE_LOCK>::cleanup");

  // Destructor releases lock.
  delete (ACE_Guard<ACE_LOCK> *) ptr;
}

template <class ACE_LOCK>
ACE_TSS_Guard<ACE_LOCK>::ACE_TSS_Guard (ACE_LOCK &lock, int block)
{
// ACE_TRACE ("ACE_TSS_Guard<ACE_LOCK>::ACE_TSS_Guard");

  this->init_key ();
  ACE_Guard<ACE_LOCK> *guard;
  ACE_NEW (guard,
           ACE_Guard<ACE_LOCK> (lock,
                                block));

#if defined (ACE_HAS_THR_C_DEST)
  ACE_TSS_Adapter *tss_adapter;
  ACE_NEW (tss_adapter,
           ACE_TSS_Adapter ((void *) guard,
                            ACE_TSS_Guard<ACE_LOCK>::cleanup));
  ACE_Thread::setspecific (this->key_,
                           (void *) tss_adapter);
#else
  ACE_Thread::setspecific (this->key_,
                           (void *) guard);
#endif /* ACE_HAS_THR_C_DEST */
}

template <class ACE_LOCK> int
ACE_TSS_Guard<ACE_LOCK>::acquire (void)
{
// ACE_TRACE ("ACE_TSS_Guard<ACE_LOCK>::acquire");

  ACE_Guard<ACE_LOCK> *guard = 0;

#if defined (ACE_HAS_THR_C_DEST)
  ACE_TSS_Adapter *tss_adapter = 0;
  ACE_Thread::getspecific (this->key_,
                           (void **) &tss_adapter);
  guard = (ACE_Guard<ACE_LOCK> *) tss_adapter->ts_obj_;
#else
  ACE_Thread::getspecific (this->key_,
                           (void **) &guard);
#endif /* ACE_HAS_THR_C_DEST */

  return guard->acquire ();
}

template <class ACE_LOCK> int
ACE_TSS_Guard<ACE_LOCK>::tryacquire (void)
{
// ACE_TRACE ("ACE_TSS_Guard<ACE_LOCK>::tryacquire");

  ACE_Guard<ACE_LOCK> *guard = 0;

#if defined (ACE_HAS_THR_C_DEST)
  ACE_TSS_Adapter *tss_adapter = 0;
  ACE_Thread::getspecific (this->key_,
                           (void **) &tss_adapter);
  guard = (ACE_Guard<ACE_LOCK> *) tss_adapter->ts_obj_;
#else
  ACE_Thread::getspecific (this->key_,
                           (void **) &guard);
#endif /* ACE_HAS_THR_C_DEST */

  return guard->tryacquire ();
}

template <class ACE_LOCK>
ACE_TSS_Write_Guard<ACE_LOCK>::ACE_TSS_Write_Guard (ACE_LOCK &lock,
                                                    int block)
{
// ACE_TRACE ("ACE_TSS_Write_Guard<ACE_LOCK>::ACE_TSS_Write_Guard");

  this->init_key ();
  ACE_Guard<ACE_LOCK> *guard;
  ACE_NEW (guard,
           ACE_Write_Guard<ACE_LOCK> (lock,
                                      block));

#if defined (ACE_HAS_THR_C_DEST)
  ACE_TSS_Adapter *tss_adapter;
  ACE_NEW (tss_adapter,
           ACE_TSS_Adapter ((void *) guard,
                            ACE_TSS_Guard<ACE_LOCK>::cleanup));
  ACE_Thread::setspecific (this->key_,
                           (void *) tss_adapter);
#else
  ACE_Thread::setspecific (this->key_,
                           (void *) guard);
#endif /* ACE_HAS_THR_C_DEST */
}

template <class ACE_LOCK> int
ACE_TSS_Write_Guard<ACE_LOCK>::acquire (void)
{
// ACE_TRACE ("ACE_TSS_Write_Guard<ACE_LOCK>::acquire");

  ACE_Write_Guard<ACE_LOCK> *guard = 0;

#if defined (ACE_HAS_THR_C_DEST)
  ACE_TSS_Adapter *tss_adapter = 0;
  ACE_Thread::getspecific (this->key_,
                           (void **) &tss_adapter);
  guard = (ACE_Guard<ACE_LOCK> *) tss_adapter->ts_obj_;
#else
  ACE_Thread::getspecific (this->key_,
                           (void **) &guard);
#endif /* ACE_HAS_THR_C_DEST */

  return guard->acquire_write ();
}

template <class ACE_LOCK> int
ACE_TSS_Write_Guard<ACE_LOCK>::tryacquire (void)
{
// ACE_TRACE ("ACE_TSS_Write_Guard<ACE_LOCK>::tryacquire");

  ACE_Write_Guard<ACE_LOCK> *guard = 0;

#if defined (ACE_HAS_THR_C_DEST)
  ACE_TSS_Adapter *tss_adapter = 0;
  ACE_Thread::getspecific (this->key_,
                           (void **) &tss_adapter);
  guard = (ACE_Guard<ACE_LOCK> *) tss_adapter->ts_obj_;
#else
  ACE_Thread::getspecific (this->key_,
                           (void **) &guard);
#endif /* ACE_HAS_THR_C_DEST */

  return guard->tryacquire_write ();
}

template <class ACE_LOCK> int
ACE_TSS_Write_Guard<ACE_LOCK>::acquire_write (void)
{
// ACE_TRACE ("ACE_TSS_Write_Guard<ACE_LOCK>::acquire_write");

  return this->acquire ();
}

template <class ACE_LOCK> int
ACE_TSS_Write_Guard<ACE_LOCK>::tryacquire_write (void)
{
// ACE_TRACE ("ACE_TSS_Write_Guard<ACE_LOCK>::tryacquire_write");

  return this->tryacquire ();
}

template <class ACE_LOCK> void
ACE_TSS_Write_Guard<ACE_LOCK>::dump (void) const
{
#if defined (ACE_HAS_DUMP)
// ACE_TRACE ("ACE_TSS_Write_Guard<ACE_LOCK>::dump");
  ACE_TSS_Guard<ACE_LOCK>::dump ();
#endif /* ACE_HAS_DUMP */
}

template <class ACE_LOCK>
ACE_TSS_Read_Guard<ACE_LOCK>::ACE_TSS_Read_Guard (ACE_LOCK &lock, int block)
{
// ACE_TRACE ("ACE_TSS_Read_Guard<ACE_LOCK>::ACE_TSS_Read_Guard");

  this->init_key ();
  ACE_Guard<ACE_LOCK> *guard;
  ACE_NEW (guard,
           ACE_Read_Guard<ACE_LOCK> (lock,
                                     block));
#if defined (ACE_HAS_THR_C_DEST)
  ACE_TSS_Adapter *tss_adapter;
  ACE_NEW (tss_adapter,
           ACE_TSS_Adapter ((void *)guard,
                            ACE_TSS_Guard<ACE_LOCK>::cleanup));
  ACE_Thread::setspecific (this->key_,
                           (void *) tss_adapter);
#else
  ACE_Thread::setspecific (this->key_,
                           (void *) guard);
#endif /* ACE_HAS_THR_C_DEST */
}

template <class ACE_LOCK> int
ACE_TSS_Read_Guard<ACE_LOCK>::acquire (void)
{
// ACE_TRACE ("ACE_TSS_Read_Guard<ACE_LOCK>::acquire");

  ACE_Read_Guard<ACE_LOCK> *guard = 0;

#if defined (ACE_HAS_THR_C_DEST)
  ACE_TSS_Adapter *tss_adapter = 0;
  ACE_Thread::getspecific (this->key_,
                           (void **) &tss_adapter);
  guard = (ACE_Guard<ACE_LOCK> *) tss_adapter->ts_obj_;
#else
  ACE_Thread::getspecific (this->key_,
                           (void **) &guard);
#endif /* ACE_HAS_THR_C_DEST */

  return guard->acquire_read ();
}

template <class ACE_LOCK> int
ACE_TSS_Read_Guard<ACE_LOCK>::tryacquire (void)
{
// ACE_TRACE ("ACE_TSS_Read_Guard<ACE_LOCK>::tryacquire");

  ACE_Read_Guard<ACE_LOCK> *guard = 0;

#if defined (ACE_HAS_THR_C_DEST)
  ACE_TSS_Adapter *tss_adapter = 0;
  ACE_Thread::getspecific (this->key_,
                           (void **) &tss_adapter);
  guard = (ACE_Guard<ACE_LOCK> *) tss_adapter->ts_obj_;
#else
  ACE_Thread::getspecific (this->key_,
                           (void **) &guard);
#endif /* ACE_HAS_THR_C_DEST */

  return guard->tryacquire_read ();
}

template <class ACE_LOCK> int
ACE_TSS_Read_Guard<ACE_LOCK>::acquire_read (void)
{
// ACE_TRACE ("ACE_TSS_Read_Guard<ACE_LOCK>::acquire_read");

  return this->acquire ();
}

template <class ACE_LOCK> int
ACE_TSS_Read_Guard<ACE_LOCK>::tryacquire_read (void)
{
// ACE_TRACE ("ACE_TSS_Read_Guard<ACE_LOCK>::tryacquire_read");

  return this->tryacquire ();
}

template <class ACE_LOCK> void
ACE_TSS_Read_Guard<ACE_LOCK>::dump (void) const
{
#if defined (ACE_HAS_DUMP)
// ACE_TRACE ("ACE_TSS_Read_Guard<ACE_LOCK>::dump");
  ACE_TSS_Guard<ACE_LOCK>::dump ();
#endif /* ACE_HAS_DUMP */
}

#endif /* defined (ACE_HAS_THREADS) && (defined (ACE_HAS_THREAD_SPECIFIC_STORAGE) || defined (ACE_HAS_TSS_EMULATION)) */

#endif /* ACE_TSS_T_C */
