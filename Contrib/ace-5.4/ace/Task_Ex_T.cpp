// Task_Ex_T.cpp
// Task_Ex_T.cpp,v 1.2 2003/12/03 21:56:52 kobica Exp

#ifndef ACE_TASK_EX_T_C
#define ACE_TASK_EX_T_C

#include "ace/Task_Ex_T.h"

#if !defined (ACE_LACKS_PRAGMA_ONCE)
# pragma once
#endif /* ACE_LACKS_PRAGMA_ONCE */

#include "ace/Module.h"
#include "ace/Null_Condition.h"

#if !defined (__ACE_INLINE__)
#include "ace/Task_Ex_T.i"
#endif /* __ACE_INLINE__ */

ACE_RCSID(ace, Task_Ex_T, "Task_Ex_T.cpp,v 1.2 2003/12/03 21:56:52 kobica Exp")

template <ACE_SYNCH_DECL, class ACE_MESSAGE_TYPE> void
ACE_Task_Ex<ACE_SYNCH_USE, ACE_MESSAGE_TYPE>::dump (void) const
{
#if defined (ACE_HAS_DUMP)
  ACE_TRACE ("ACE_Task_Ex<ACE_SYNCH_USE, ACE_MESSAGE_TYPE>::dump");
  ACE_DEBUG ((LM_DEBUG, ACE_BEGIN_DUMP, this));
  ACE_DEBUG ((LM_DEBUG,  ACE_LIB_TEXT ("\nthr_mgr_ = %x"), this->thr_mgr_));
  this->msg_queue_->dump ();
  ACE_DEBUG ((LM_DEBUG,  ACE_LIB_TEXT ("delete_msg_queue_ = %d\n"), this->delete_msg_queue_));
  ACE_DEBUG ((LM_DEBUG,  ACE_LIB_TEXT ("\nflags = %x"), this->flags_));
  ACE_DEBUG ((LM_DEBUG,  ACE_LIB_TEXT ("\nmod_ = %x"), this->mod_));
  ACE_DEBUG ((LM_DEBUG,  ACE_LIB_TEXT ("\nnext_ = %x"), this->next_));
  ACE_DEBUG ((LM_DEBUG,  ACE_LIB_TEXT ("\ngrp_id_ = %d"), this->grp_id_));
  ACE_DEBUG ((LM_DEBUG,  ACE_LIB_TEXT ("\nthr_count_ = %d"), this->thr_count_));
#if defined (ACE_MT_SAFE) && (ACE_MT_SAFE != 0)
  this->lock_.dump ();
#endif /* ACE_MT_SAFE */

  ACE_DEBUG ((LM_DEBUG, ACE_END_DUMP));
#endif /* ACE_HAS_DUMP */
}

// If the user doesn't supply a ACE_Message_Queue_Ex pointer then we'll
// allocate one dynamically.  Otherwise, we'll use the one they give.

template<ACE_SYNCH_DECL, class ACE_MESSAGE_TYPE>
ACE_Task_Ex<ACE_SYNCH_USE, ACE_MESSAGE_TYPE>::ACE_Task_Ex (ACE_Thread_Manager *thr_man,
                                   ACE_Message_Queue_Ex<ACE_MESSAGE_TYPE, ACE_SYNCH_USE> *mq)
  : ACE_Task_Base (thr_man),
    msg_queue_ (0),
    delete_msg_queue_ (0),
    mod_ (0),
    next_ (0)
{
  ACE_TRACE ("ACE_Task_Ex<ACE_SYNCH_USE, ACE_MESSAGE_TYPE>::ACE_Task_Ex");

  if (mq == 0)
    {
      ACE_NEW (mq,
               (ACE_Message_Queue_Ex<ACE_MESSAGE_TYPE, ACE_SYNCH_USE>));
      this->delete_msg_queue_ = 1;
    }

  this->msg_queue_ = mq;
}

template<ACE_SYNCH_DECL, class ACE_MESSAGE_TYPE>
ACE_Task_Ex<ACE_SYNCH_USE, ACE_MESSAGE_TYPE>::~ACE_Task_Ex (void)
{
  ACE_TRACE ("ACE_Task_Ex<ACE_SYNCH_USE, ACE_MESSAGE_TYPE>::~ACE_Task_Ex");
  if (this->delete_msg_queue_)
    delete this->msg_queue_;

  // These assignments aren't strickly necessary but they help guard
  // against odd race conditions...
  this->delete_msg_queue_ = 0;
}

template<ACE_SYNCH_DECL, class ACE_MESSAGE_TYPE> ACE_Task<ACE_SYNCH_USE> *
ACE_Task_Ex<ACE_SYNCH_USE, ACE_MESSAGE_TYPE>::sibling (void)
{
  ACE_TRACE ("ACE_Task_Ex<ACE_SYNCH_USE, ACE_MESSAGE_TYPE>::sibling");
  /// @todo FIXME Need to impl ACE_Moudle to support ACE_Task as well.
  /// Now always return 0 for sibling
  return 0;  
/*
  if (this->mod_ == 0)
    return 0;
  else
    return this->mod_->sibling (this);
*/
}

template<ACE_SYNCH_DECL, class ACE_MESSAGE_TYPE> const ACE_TCHAR *
ACE_Task_Ex<ACE_SYNCH_USE, ACE_MESSAGE_TYPE>::name (void) const
{
  ACE_TRACE ("ACE_Task_Ex<ACE_SYNCH_USE, ACE_MESSAGE_TYPE>::name");
  if (this->mod_ == 0)
    return 0;
  else
    return this->mod_->name ();
}

template<ACE_SYNCH_DECL, class ACE_MESSAGE_TYPE> ACE_Module<ACE_SYNCH_USE> *
ACE_Task_Ex<ACE_SYNCH_USE, ACE_MESSAGE_TYPE>::module (void) const
{
  ACE_TRACE ("ACE_Task_Ex<ACE_SYNCH_USE, ACE_MESSAGE_TYPE>::module");
  return this->mod_;
}

#endif /* ACE_TASK_EX_T_C */
