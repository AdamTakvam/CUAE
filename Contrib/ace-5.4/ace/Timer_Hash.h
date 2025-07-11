/* -*- C++ -*- */

//=============================================================================
/**
 *  @file    Timer_Hash.h
 *
 *  Timer_Hash.h,v 1.10 2003/07/19 19:04:14 dhinton Exp
 *
 *  @author Darrell Brunsch <brunsch@cs.wustl.edu>
 */
//=============================================================================


#ifndef ACE_TIMER_HASH_H
#define ACE_TIMER_HASH_H
#include /**/ "ace/pre.h"

#include "ace/Timer_Hash_T.h"

#if !defined (ACE_LACKS_PRAGMA_ONCE)
# pragma once
#endif /* ACE_LACKS_PRAGMA_ONCE */

#include "ace/Timer_Heap_T.h"
#include "ace/Timer_List_T.h"

// The following typedef are here for ease of use

typedef ACE_Timer_Hash_Upcall <ACE_Event_Handler *,
                               ACE_Event_Handler_Handle_Timeout_Upcall<ACE_SYNCH_RECURSIVE_MUTEX>,
                               ACE_SYNCH_RECURSIVE_MUTEX>
        ACE_Hash_Upcall;

typedef ACE_Timer_List_T <ACE_Event_Handler *,
                          ACE_Hash_Upcall,
                          ACE_Null_Mutex>
        ACE_Hash_Timer_List;

typedef ACE_Timer_Heap_T <ACE_Event_Handler *,
                          ACE_Hash_Upcall,
                          ACE_Null_Mutex>
        ACE_Hash_Timer_Heap;


typedef ACE_Timer_Hash_T<ACE_Event_Handler *,
                        ACE_Event_Handler_Handle_Timeout_Upcall<ACE_SYNCH_RECURSIVE_MUTEX>,
                        ACE_SYNCH_RECURSIVE_MUTEX,
                        ACE_Hash_Timer_List>

        ACE_Timer_Hash;

typedef ACE_Timer_Hash_Iterator_T<ACE_Event_Handler *,
                                  ACE_Event_Handler_Handle_Timeout_Upcall<ACE_SYNCH_RECURSIVE_MUTEX>,
                                  ACE_SYNCH_RECURSIVE_MUTEX,
                                  ACE_Hash_Timer_List>
        ACE_Timer_Hash_Iterator;

typedef ACE_Timer_Hash_T<ACE_Event_Handler *,
                        ACE_Event_Handler_Handle_Timeout_Upcall<ACE_SYNCH_RECURSIVE_MUTEX>,
                        ACE_SYNCH_RECURSIVE_MUTEX,
                        ACE_Hash_Timer_Heap>
        ACE_Timer_Hash_Heap;

typedef ACE_Timer_Hash_Iterator_T<ACE_Event_Handler *,
                                  ACE_Event_Handler_Handle_Timeout_Upcall<ACE_SYNCH_RECURSIVE_MUTEX>,
                                  ACE_SYNCH_RECURSIVE_MUTEX,
                                  ACE_Hash_Timer_Heap>
        ACE_Timer_Hash_Heap_Iterator;

#include /**/ "ace/post.h"
#endif /* ACE_TIMER_HASH_H */
