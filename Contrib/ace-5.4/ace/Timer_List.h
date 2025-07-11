/* -*- C++ -*- */

//=============================================================================
/**
 *  @file    Timer_List.h
 *
 *  Timer_List.h,v 4.23 2003/07/19 19:04:14 dhinton Exp
 *
 *  @author Doug Schmidt
 */
//=============================================================================


#ifndef ACE_TIMER_LIST_H
#define ACE_TIMER_LIST_H
#include /**/ "ace/pre.h"

#include "ace/Timer_List_T.h"

#if !defined (ACE_LACKS_PRAGMA_ONCE)
# pragma once
#endif /* ACE_LACKS_PRAGMA_ONCE */

// The following typedef are here for ease of use and backward
// compatibility.

typedef ACE_Timer_List_T<ACE_Event_Handler *,
                         ACE_Event_Handler_Handle_Timeout_Upcall<ACE_SYNCH_RECURSIVE_MUTEX>,
                         ACE_SYNCH_RECURSIVE_MUTEX>
        ACE_Timer_List;

typedef ACE_Timer_List_Iterator_T<ACE_Event_Handler *,
                                  ACE_Event_Handler_Handle_Timeout_Upcall<ACE_SYNCH_RECURSIVE_MUTEX>,
                                  ACE_SYNCH_RECURSIVE_MUTEX>
        ACE_Timer_List_Iterator;

#include /**/ "ace/post.h"
#endif /* ACE_TIMER_LIST_H */
