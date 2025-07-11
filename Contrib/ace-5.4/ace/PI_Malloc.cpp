#ifndef ACE_PI_MALLOC_CPP
#define ACE_PI_MALLOC_CPP

#include "ace/PI_Malloc.h"

ACE_RCSID (ace,
           PI_Malloc,
           "PI_Malloc.cpp,v 4.11 2003/11/09 04:12:07 dhinton Exp")

#if (ACE_HAS_POSITION_INDEPENDENT_POINTERS == 1)

#if !defined (__ACE_INLINE__)
#include "ace/PI_Malloc.i"
#endif /* __ACE_INLINE__ */

#include "ace/Object_Manager.h"
#include "ace/Process_Mutex.h"
#include "ace/OS_NS_string.h"

void
ACE_PI_Control_Block::ACE_Malloc_Header::dump (void) const
{
#if defined (ACE_HAS_DUMP)
  ACE_TRACE ("ACE_PI_Control_Block::ACE_Malloc_Header::dump");

  ACE_DEBUG ((LM_DEBUG, ACE_BEGIN_DUMP, this));
  ACE_DEBUG ((LM_DEBUG, ACE_LIB_TEXT ("\nnext_block = %x"), (ACE_Malloc_Header *) this->next_block_));
  ACE_DEBUG ((LM_DEBUG, ACE_LIB_TEXT ("\nsize = %d\n"), this->size_));
  ACE_DEBUG ((LM_DEBUG, ACE_END_DUMP));
#endif /* ACE_HAS_DUMP */
}

void
ACE_PI_Control_Block::print_alignment_info (void)
{
#if defined (ACE_HAS_DUMP)
  ACE_TRACE ("ACE_PI_Control_Block::ACE_Control_Block::print_alignment_info");
  ACE_DEBUG ((LM_DEBUG, ACE_LIB_TEXT ("Start ---> ACE_PI_Control_Block::print_alignment_info:\n")));
  ACE_DEBUG ((LM_DEBUG,
              ACE_LIB_TEXT ("Sizeof ptr: %d\n")
              ACE_LIB_TEXT ("Sizeof size_t: %d\n")
              ACE_LIB_TEXT ("Sizeof long: %d\n")
              ACE_LIB_TEXT ("Sizeof double: %d\n")
              ACE_LIB_TEXT ("Sizeof ACE_MALLOC_ALIGN: %d\n")
              ACE_LIB_TEXT ("sizeof ACE_MALLOC_PADDING: %d\n")
              ACE_LIB_TEXT ("Sizeof ACE_MALLOC_HEADER_SIZE: %d\n")
              ACE_LIB_TEXT ("Sizeof ACE_PI_MALLOC_PADDING_SIZE: %d\n")
              ACE_LIB_TEXT ("Sizeof ACE_PI_CONTROL_BLOCK_SIZE: %d\n")
              ACE_LIB_TEXT ("Sizeof ACE_PI_CONTROL_BLOCK_ALIGN_BYTES: %d\n")
              ACE_LIB_TEXT ("Sizeof (MALLOC_HEADER): %d\n")
              ACE_LIB_TEXT ("Sizeof (CONTROL_BLOCK): %d\n"),
              sizeof (char *),
              sizeof (size_t),
              sizeof (long),
              sizeof (double),
              ACE_MALLOC_ALIGN,
              ACE_MALLOC_PADDING,
              ACE_MALLOC_HEADER_SIZE,
              ACE_PI_MALLOC_PADDING_SIZE,
              ACE_PI_CONTROL_BLOCK_SIZE,
              ACE_PI_CONTROL_BLOCK_ALIGN_BYTES,
              sizeof (ACE_Malloc_Header),
              sizeof (ACE_PI_Control_Block)
              ));
  ACE_DEBUG ((LM_DEBUG, ACE_LIB_TEXT ("End <--- ACE_PI_Control_Block::print_alignment_info:\n")));
#endif /* ACE_HAS_DUMP */
}

void
ACE_PI_Control_Block::dump (void) const
{
#if defined (ACE_HAS_DUMP)
  ACE_TRACE ("ACE_PI_Control_Block::dump");

  ACE_DEBUG ((LM_DEBUG, ACE_BEGIN_DUMP, this));
  ACE_DEBUG ((LM_DEBUG, ACE_LIB_TEXT ("Name Node:\n")));
  for (ACE_Name_Node *nextn = this->name_head_;
       nextn != 0;
       nextn = nextn->next_)
    nextn->dump ();

  ACE_DEBUG ((LM_DEBUG, ACE_LIB_TEXT ("freep_ = %x"), (ACE_Malloc_Header *) this->freep_));
  this->base_.dump ();

  ACE_DEBUG ((LM_DEBUG, ACE_LIB_TEXT ("\nMalloc Header:\n")));
  for (ACE_Malloc_Header *nexth = ((ACE_Malloc_Header *)this->freep_)->next_block_;
       nexth != 0 && nexth != &this->base_;
       nexth = nexth->next_block_)
    nexth->dump ();

  ACE_DEBUG ((LM_DEBUG, ACE_LIB_TEXT ("\n")));
  ACE_DEBUG ((LM_DEBUG, ACE_END_DUMP));
#endif /* ACE_HAS_DUMP */
}

ACE_PI_Control_Block::ACE_Name_Node::ACE_Name_Node (void)
{
  ACE_TRACE ("ACE_PI_Control_Block::ACE_Name_Node::ACE_Name_Node");
}

ACE_PI_Control_Block::ACE_Name_Node::ACE_Name_Node (const char *name,
                                                    char *name_ptr,
                                                    char *pointer,
                                                    ACE_Name_Node *next)
  : name_ (name_ptr),
    pointer_ (pointer),
    next_ (next),
    prev_ (0)
{
  ACE_TRACE ("ACE_PI_Control_Block::ACE_Name_Node::ACE_Name_Node");
  char *n = this->name_;
  ACE_OS::strcpy (n, name);
  if (next != 0)
    next->prev_ = this;
}

ACE_PI_Control_Block::ACE_Name_Node::ACE_Name_Node (const ACE_Name_Node &)
{
  ACE_TRACE ("ACE_PI_Control_Block::ACE_Name_Node::ACE_Name_Node");
  ACE_ASSERT (0); // not implemented!
}

const char *
ACE_PI_Control_Block::ACE_Name_Node::name (void) const
{
  const char *c = this->name_;
  return c;
}

void
ACE_PI_Control_Block::ACE_Name_Node::name (const char *)
{
  ACE_ASSERT (0); // not implemented yet.
}

ACE_PI_Control_Block::ACE_Malloc_Header::ACE_Malloc_Header (void)
  : next_block_ (0),
    size_ (0)
{
}

void
ACE_PI_Control_Block::ACE_Name_Node::dump (void) const
{
#if defined (ACE_HAS_DUMP)
  ACE_TRACE ("ACE_PI_Control_Block::ACE_Name_Node::dump");

  ACE_DEBUG ((LM_DEBUG, ACE_BEGIN_DUMP, this));
  ACE_DEBUG ((LM_DEBUG, ACE_LIB_TEXT("pointer = %x"), (const char *) this->pointer_));
  ACE_DEBUG ((LM_DEBUG, ACE_LIB_TEXT("\nnext_ = %x"), (ACE_Name_Node *) this->next_));
  ACE_DEBUG ((LM_DEBUG,
              ACE_LIB_TEXT("\nname_ = (%x, %s)"),
              (const char *) this->name_,
              (const char *) this->name_));
  ACE_DEBUG ((LM_DEBUG, ACE_LIB_TEXT("\n")));
  ACE_DEBUG ((LM_DEBUG, ACE_END_DUMP));
#endif /* ACE_HAS_DUMP */
}


#if defined (ACE_HAS_EXPLICIT_TEMPLATE_INSTANTIATION)
template class ACE_Based_Pointer_Basic<ACE_PI_Control_Block::ACE_Malloc_Header>;
template class ACE_Based_Pointer_Basic<ACE_PI_Control_Block::ACE_Name_Node>;
template class ACE_Based_Pointer_Basic<char>;
template class ACE_Based_Pointer<ACE_PI_Control_Block::ACE_Malloc_Header>;
template class ACE_Based_Pointer<ACE_PI_Control_Block::ACE_Name_Node>;
#elif defined (ACE_HAS_TEMPLATE_INSTANTIATION_PRAGMA)
#pragma instantiate ACE_Based_Pointer_Basic<ACE_PI_Control_Block::ACE_Malloc_Header>
#pragma instantiate ACE_Based_Pointer_Basic<ACE_PI_Control_Block::ACE_Name_Node>
#pragma instantiate ACE_Based_Pointer_Basic<char>
#pragma instantiate ACE_Based_Pointer<ACE_PI_Control_Block::ACE_Malloc_Header>
#pragma instantiate ACE_Based_Pointer<ACE_PI_Control_Block::ACE_Name_Node>
#endif /* ACE_HAS_EXPLICIT_TEMPLATE_INSTANTIATION */

#endif /* ACE_HAS_POSITION_INDEPENDENT_POINTERS == 1*/

#endif /* ACE_PI_MALLOC_CPP */
