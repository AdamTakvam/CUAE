// Malloc_Allocator.cpp,v 4.6 2002/08/01 16:17:18 bala Exp

// FILENAME
//   Malloc_Allocator.cpp
//
// AUTHOR
//   Priyanka Gontla <pgontla@uci.edu>
//
// ==========================================================================

#if !defined (ACE_MALLOC_ALLOCATOR_CPP)
#define ACE_MALLOC_ALLOCATOR_CPP

#include "ace/Malloc_Allocator.h"
#include "ace/Object_Manager.h"

#if !defined (__ACE_INLINE__)
#include "ace/Malloc_Allocator.i"
#endif /* __ACE_INLINE__ */

#include "ace/Synch_T.h"

ACE_RCSID (ace, Malloc_Allocator, "Malloc_Allocator.cpp,v 4.6 2002/08/01 16:17:18 bala Exp")

ACE_Allocator *
ACE_Allocator::instance (void)
{
  //  ACE_TRACE ("ACE_Allocator::instance");

  if (ACE_Allocator::allocator_ == 0)
    {
      // Perform Double-Checked Locking Optimization.
      ACE_MT (ACE_GUARD_RETURN (ACE_Recursive_Thread_Mutex, ace_mon,
                                *ACE_Static_Object_Lock::instance (), 0));

      if (ACE_Allocator::allocator_ == 0)
        {
          // Have a seat.  We want to avoid ever having to delete the
          // ACE_Allocator instance, to avoid shutdown order
          // dependencies.  ACE_New_Allocator never needs to be
          // destroyed:  its destructor is empty and its instance
          // doesn't have any state.  Therefore, sizeof
          // ACE_New_Allocator is equal to sizeof void *.  It's
          // instance just contains a pointer to its virtual function
          // table.
          //
          // So, we allocate space for the ACE_New_Allocator instance
          // in the data segment.  Because its size is the same as
          // that of a pointer, we allocate it as a pointer so that it
          // doesn't get constructed statically.  We never bother to
          // destroy it.
          static void *allocator_instance = 0;

          // Check this critical assumption.  We put it in a variable
          // first to avoid stupid compiler warnings that the
          // condition may always be true/false.
#         if !defined (ACE_NDEBUG)
          int assertion = (sizeof allocator_instance ==
                           sizeof (ACE_New_Allocator));
          ACE_ASSERT (assertion);
#         endif /* !ACE_NDEBUG */

          // Initialize the allocator_instance by using a placement
          // new.
          ACE_Allocator::allocator_ =
            new (&allocator_instance) ACE_New_Allocator;
        }
    }

  return ACE_Allocator::allocator_;
}

ACE_Allocator *
ACE_Allocator::instance (ACE_Allocator *r)
{
  ACE_TRACE ("ACE_Allocator::instance");
  ACE_MT (ACE_GUARD_RETURN (ACE_Recursive_Thread_Mutex, ace_mon,
                            *ACE_Static_Object_Lock::instance (), 0));
  ACE_Allocator *t = ACE_Allocator::allocator_;

  // We can't safely delete it since we don't know who created it!
  ACE_Allocator::delete_allocator_ = 0;

  ACE_Allocator::allocator_ = r;
  return t;
}

void
ACE_Allocator::close_singleton (void)
{
  ACE_TRACE ("ACE_Allocator::close_singleton");

  ACE_MT (ACE_GUARD (ACE_Recursive_Thread_Mutex, ace_mon,
                     *ACE_Static_Object_Lock::instance ()));

  if (ACE_Allocator::delete_allocator_)
    {
      // This should never be executed....  See the
      // ACE_Allocator::instance (void) method for an explanation.
      delete ACE_Allocator::allocator_;
      ACE_Allocator::allocator_ = 0;
      ACE_Allocator::delete_allocator_ = 0;
    }
}

ACE_Allocator::~ACE_Allocator (void)
{
  ACE_TRACE ("ACE_Allocator::~ACE_Allocator");
}

ACE_Allocator::ACE_Allocator (void)
{
  ACE_TRACE ("ACE_Allocator::ACE_Allocator");
}

void
ACE_Static_Allocator_Base::dump (void) const
{
  ACE_TRACE ("ACE_Static_Allocator_Base::dump");

  ACE_DEBUG ((LM_DEBUG, ACE_BEGIN_DUMP, this));
  ACE_DEBUG ((LM_DEBUG, ACE_LIB_TEXT ("\noffset_ = %d"), this->offset_));
  ACE_DEBUG ((LM_DEBUG, ACE_LIB_TEXT ("\nsize_ = %d\n"), this->size_));
  ACE_HEX_DUMP ((LM_DEBUG, this->buffer_, this->size_));
  ACE_DEBUG ((LM_DEBUG, ACE_LIB_TEXT ("\n")));

  ACE_DEBUG ((LM_DEBUG, ACE_END_DUMP));
}



#endif /* ACE_MALLOC_ALLOCATOR_CPP */
