// Timer_Wheel.cpp,v 4.9 2003/08/09 00:59:29 dhinton Exp

#if !defined (ACE_TIMER_WHEEL_C)
#define ACE_TIMER_WHEEL_C

#include "ace/Timer_Wheel.h"
#include "ace/Synch_Traits.h"
#include "ace/Recursive_Thread_Mutex.h"

#if defined (ACE_HAS_BROKEN_HPUX_TEMPLATES)
#include "ace/Timer_Wheel_T.cpp"
#endif /* ACE_HAS_BROKEN_HPUX_TEMPLATES */

ACE_RCSID(ace, Timer_Wheel, "Timer_Wheel.cpp,v 4.9 2003/08/09 00:59:29 dhinton Exp")

#if defined (ACE_HAS_EXPLICIT_TEMPLATE_INSTANTIATION)
template class ACE_Timer_Wheel_T<ACE_Event_Handler *, ACE_Event_Handler_Handle_Timeout_Upcall<ACE_SYNCH_RECURSIVE_MUTEX>, ACE_SYNCH_RECURSIVE_MUTEX>;
template class ACE_Timer_Wheel_Iterator_T<ACE_Event_Handler *, ACE_Event_Handler_Handle_Timeout_Upcall<ACE_SYNCH_RECURSIVE_MUTEX>, ACE_SYNCH_RECURSIVE_MUTEX>;
#elif defined (ACE_HAS_TEMPLATE_INSTANTIATION_PRAGMA)
#pragma instantiate ACE_Timer_Wheel_T<ACE_Event_Handler *, ACE_Event_Handler_Handle_Timeout_Upcall<ACE_SYNCH_RECURSIVE_MUTEX>, ACE_SYNCH_RECURSIVE_MUTEX>
#pragma instantiate ACE_Timer_Wheel_Iterator_T<ACE_Event_Handler *, ACE_Event_Handler_Handle_Timeout_Upcall<ACE_SYNCH_RECURSIVE_MUTEX>, ACE_SYNCH_RECURSIVE_MUTEX>
#endif /* ACE_HAS_EXPLICIT_TEMPLATE_INSTANTIATION */


#endif /* ACE_TIMER_WHEEL_C */
