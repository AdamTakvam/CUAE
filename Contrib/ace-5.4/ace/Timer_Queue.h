// -*- C++ -*-

//=============================================================================
/**
 *  @file    Timer_Queue.h
 *
 *  Timer_Queue.h,v 4.36 2003/11/05 23:30:47 shuston Exp
 *
 *  @author Douglas C. Schmidt <schmidt@cs.wustl.edu>
 *  @author Irfan Pyarali <irfan@cs.wustl.edu>
 */
//=============================================================================

#ifndef ACE_TIMER_QUEUE_H
#define ACE_TIMER_QUEUE_H

#include /**/ "ace/pre.h"

#include "ace/Synch_Traits.h"

#if !defined (ACE_LACKS_PRAGMA_ONCE)
# pragma once
#endif /* ACE_LACKS_PRAGMA_ONCE */

#include "ace/Timer_Queuefwd.h"
#include "ace/Timer_Queue_T.h"
#if defined (ACE_HAS_THREADS)
#  include "ace/Recursive_Thread_Mutex.h"
#else
#  include "ace/Null_Mutex.h"
#endif /* ACE_HAS_THREADS */

// The following typedef are here for ease of use and backward
// compatibility.
typedef ACE_Timer_Node_Dispatch_Info_T<ACE_Event_Handler *>
        ACE_Timer_Node_Dispatch_Info;

typedef ACE_Timer_Node_T<ACE_Event_Handler *>
        ACE_Timer_Node;

typedef ACE_Timer_Queue_Iterator_T<ACE_Event_Handler *,
                                   ACE_Event_Handler_Handle_Timeout_Upcall<ACE_SYNCH_RECURSIVE_MUTEX>,
                                   ACE_SYNCH_RECURSIVE_MUTEX>
        ACE_Timer_Queue_Iterator;

#include /**/ "ace/post.h"

#endif /* ACE_TIMER_QUEUE_H */
