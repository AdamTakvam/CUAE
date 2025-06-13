/*
 *------------------------------------------------------------------
 * timer.c - Generic asynchronous timer event service
 *
 *
 * Copyright (c) 2003 by Cisco Systems, Inc.
 * All rights reserved.
 *------------------------------------------------------------------
 */

#include <stdio.h>
#include "timer_platform.h"
#include "timer.h"


/* Local defines and data structures.  
 * These are private to this file and should not be made externally
 * accessable.
*/
#define TIMER_FREE 0x1              /* Indicates timer is free */
#define TIMER_INITIALIZED 0x2       /* Indicates timer is initialized */
#define TIMER_ACTIVE 0x4            /* Indicates timer is in list */

/* Timer event structure */
typedef struct timer_struct
{
  unsigned int expiration_time;     /* Expiration time */
  int interval;                     /* Timer period */
  void *parameter;                  /* Timer expiration callback param */
  void *parameter2;                 /* Second timer expiration callback param */
  void (*expiration_callback)                /* Expiry handler */
             (void *timer,                   /* Handle for expired timer */
              void *parameter2,              /* Timer param 1 */
              void *paramerer2               /* Timer param 2 */
              );
  int flags;                        /* Debugging flags */
  struct timer_struct *pred;        /* List predecessor */
  struct timer_struct *next;        /* List successor */
} timer_struct_type;

/* Local Data */
static timer_struct_type timers[MAX_TIMERS];    /* Timers */
static timer_struct_type *timer_list;           /* Pending timer list */
static timer_struct_type *timer_free;           /* Timer free list */

/* Local data debug info */
static int expired_count;                   /* Number of timers that expired */
static int removed_count;                   /* Number of timers removed. */
static int inserted_count;                  /* Number of timers inserted. */


/* Local Routines */


/*-------------------------------------------------------------------------------
 *  NAME:       timer_insert(*timer)
 *
 *  PARAMETERS: 
 *   timer - Timer to be inserted into timer event list. 
 *           Must be initilialized and have timeout value set.
 *
 *  RETURNS:    
 *   void
 *
 *  DESCRIPTION:
 *   Insert timer entry into doubly linked timer list sorted in
 *   ascending order by timeout expiration.
 *
 *----------------------------------------------------------------------------*/
static void timer_insert(timer_struct_type *timer)
{

  timer_struct_type *element;       /* Timer element new timer is inserted in front of */
  timer_struct_type *pred;          /* Timer element new timer is inserted behind */
  int insert_at_front;              /* Indicates new timer inserted in front of list */

  platform_critical_enter();

  if (timer_list == NULL) {
    insert_at_front = 1;
  }
  else if (timer->expiration_time <= timer_list->expiration_time) {  
    insert_at_front = 1;
  }
  else {
    insert_at_front = 0;
  }
    

  if (insert_at_front) {
    /* Insert timer into front of timer list */
    if (timer_list != NULL) {
      timer_list->pred = timer;
    }
    timer->next = timer_list;
    timer->pred = NULL;
    timer_list = timer;
  }
  else {
    /* Traverse list looking for place to insert */
    pred = timer_list;
    element = timer_list->next;

    /* Find position for timer insert */
    while (element != NULL) {
      if (timer->expiration_time <= element->expiration_time) {
          /* Found insertion point */
          break;
      }
      else {
        /* Check next element */
        pred = element;
        element = element->next;
      }
    }

    /* Insert timer in front of element */
    timer->pred = pred;
    timer->next = pred->next;
    pred->next = timer;

    if (element != NULL) {
      element->pred = timer;
    }
  }

  platform_critical_exit();
}


/*-------------------------------------------------------------------------------
 *  NAME:   timer_remove(*timer)
 *
 *  PARAMETERS:
 *   timer - Timer to be removed from active timer list.
 *
 *  RETURNS:
 *   void
 *
 *  DESCRIPTION:
 *   Removes specified timer from the doubly linked
 *   timer list to cancel timer.
 *
 *----------------------------------------------------------------------------*/
static void timer_remove(timer_struct_type *timer)
{
 
//  platform_critical_enter();
   
  if (timer_list == timer) {
    /* Element is at front of list */
    timer_list = timer->next;
    if (timer->next != NULL) {
      timer->next->pred = NULL;
    }
  }
  else {
    /* Timer is in the middle of the list */
    timer->pred->next = timer->next;
    if (timer->next != NULL) {
       timer->next->pred = timer->pred;
    }
  }
  timer->next = NULL;
  timer->pred = NULL;
  
//  platform_critical_exit();
}


/*   Global Routines */


/*-------------------------------------------------------------------------------
 *  NAME:   timer_event_activate(*timer)
 *
 *  PARAMETERS:
 *   timer - timer to be activated.  Must be initalized.
 *
 *  RETURNS:
 *   void
 *
 *  DESCRIPTION:
 *   Place previously initialized timer into the active timer list.
 *
 *----------------------------------------------------------------------------*/
//void timer_event_activate(timer_struct_type *timer)
void timer_event_activate(void *timer2)
{
  timer_struct_type *timer = timer2;

  if (timer == NULL) {
    return;
  }
    
  if (timer->flags & TIMER_FREE){
    err_msg("!timer_event_activate: %x %s\n", timer,
       "Attempting to activate free timer\n");
  }
  else if (timer->flags & TIMER_ACTIVE) {
    err_msg("!timer_event_activate: %x %s\n", timer,
       "Attempting to activate active timer");
  }
  else if ((timer->flags & TIMER_INITIALIZED) == 0)
  {
    err_msg("!timer_event_activate: %x %s\n", timer,
       "Attempting to activate unitialized timer");
  }
  else
  {
    /* Timer is safe to activate */
    timer->expiration_time = current_time() + timer->interval;
    
    /* Insert into timer list */
    timer_insert(timer);

    timer->flags |= TIMER_ACTIVE;
  }
}

/*-------------------------------------------------------------------------------
 *  NAME:   timer_event_allocate()
 *
 *  PARAMETERS:
 *   None.
 *
 *  RETURNS:
 *   timer - if timer is available.
 *   0 - otherwise.
 *
 *  DESCRIPTION:
 *   Allocate a timer from free list of timers.
 *
 *----------------------------------------------------------------------------*/
void *timer_event_allocate(void)
{
  timer_struct_type *new_timer;

  platform_critical_enter();
  
  new_timer = timer_free;
  if (new_timer != NULL) 
  {
    timer_free = new_timer->next;
    new_timer->flags &= ~TIMER_FREE;
    platform_critical_exit();
  }
  else 
  {
    platform_critical_exit();
    /* Out of timers */
    err_msg("!timer_event_allocate:  Could not allocate timer\n");
  }
  

  return(new_timer);
}

/*-------------------------------------------------------------------------------
 *  NAME:   timer_event_cancel(timer)
 *
 *  PARAMETERS:
 *   timer - timer to be canceled
 *
 *  RETURNS:
 *   void
 *
 *  DESCRIPTION:
 *   Remove active timer from timer list.
 *
 *----------------------------------------------------------------------------*/
//void timer_event_cancel(timer_struct_type *timer)
void timer_event_cancel(void *timer2)
{
  timer_struct_type *timer = timer2;
  
  if (timer == NULL) {
    return;
  }

  if (timer->flags & TIMER_ACTIVE)
  {
    /* Timer is active.  Cancel it */
    timer_remove(timer);
    timer->flags &= ~TIMER_ACTIVE;
  }
  else
  {
    /* Timer is not active.  Ignore request to cancel */
  }
  
  removed_count++;              /* Debug counter */
 
}

/*-------------------------------------------------------------------------------
 *  NAME:   timer_event_free(*timer)
 *
 *  PARAMETERS:
 *   timer - timer to be freed.  Must not be in active timer list.
 *
 *  RETURNS:
 *   void
 *
 *  DESCRIPTION:
 *   Return specified timer entry to timer free list.
 *
 *----------------------------------------------------------------------------*/
//void timer_event_free(timer_struct_type *timer)
void timer_event_free(void *timer2)
{
  timer_struct_type *timer = timer2;

  if (timer == NULL) {
      return;
  }

  if (timer->flags & TIMER_FREE)
  {
    err_msg("!timer_event_free: %x %s\n", timer,
       "Attempting to free timer that is already free");
  }
  else if (timer->flags & TIMER_ACTIVE) 
  {
    err_msg("!timer_event_free: %x %s\n", timer,
       "Attempting to free active timer");
  }
  else 
  {
    platform_critical_enter();
    /* Timer is ok to free.  Insert at front of list */
    timer->flags = TIMER_FREE;
    timer->next = timer_free;
    timer_free = timer;
    platform_critical_exit();
  }

}


/*-------------------------------------------------------------------------------
 *  NAME:   timer_event_initialize( see below)
 *
 *  PARAMETERS:
 *   timer - timer to be initialized.
 *   period - Number of system ticks representing timer period.
 *   expiration - Timer expiration handler.
 *   parameter - Parameter to pass to timer expiration routine.
 *   parameter2 - Second parameter to pass to timer expiration routine.
 *
 *  RETURNS:
 *   void
 *
 *  DESCRIPTION:
 *   Initilizes a timer entry with timer period, expiry handler, and
 *   expiration parameter.
 *
 *----------------------------------------------------------------------------*/
//void timer_event_initialize(timer_struct_type *timer,
void timer_event_initialize(void *timer2,
                            int period,
                            void (*expiration)(void *timer_event, 
                                               void *param,
                                               void *param2),                                           
                            void *param,
                            void *param2)
{
  timer_struct_type *timer = timer2;

  if (timer == NULL) {
    err_msg("!timer_event_initialize:  %s\n",
            "Null timer\n");

  }
  else if (timer->flags & TIMER_FREE)
  {
    err_msg("!timer_event_initialize: %x %s\n", timer,
       "Attempting to initialize free timer\n");
  }
  else if (timer->flags & TIMER_ACTIVE) 
  {
    err_msg("!timer_event_initialize: %x %s\n", timer,
       "Attempting to initilize active timer");
  }
  else { 
    platform_critical_enter();
    /* Setup timer */
    timer->interval = platform_ms_to_ticks(period);
    timer->expiration_callback = expiration;
    timer->parameter = param;
    timer->parameter2 = param2;
    timer->flags |= TIMER_INITIALIZED;
    platform_critical_exit();
  }
}

/*-------------------------------------------------------------------------------
 *  NAME:   timer_event_process()
 *
 *  PARAMETERS:
 *   None.
 *
 *  RETURNS:
 *   void
 *
 *  DESCRIPTION:
 *   This is called to process the timer event list.  The current time
 *   is compared with the sorted list of entries in the timer list.  
 *   When a timer is found to have expired, it is removed from the list
 *   and its expiration routine is called.
 *   
 *----------------------------------------------------------------------------*/
void timer_event_process(void)
{
  unsigned int now;     /* Current time */
  timer_struct_type *timer; /* User to travers timer list */

  now = current_time(); 
  
  platform_critical_enter();
  timer = timer_list;

  while (timer != NULL) {
    if (timer->expiration_time <= now){

      /* Timer has expired. */

      timer_remove(timer);  /* Remove from active list */
      timer->flags &= ~TIMER_ACTIVE;

      expired_count++;      /* Debug counter */

      /* Call expiration routine */
      timer->expiration_callback(timer, 
                                 timer->parameter,
                                 timer->parameter2);
                                 

      /* Timer expiration function may have manipulated the
       * timer list.  Need to start over at front of timer list looking
       * for expired timers.
      */
      timer = timer_list;   /* Start at front of list */
    }
    else {
      /* No more expired timers. Bail */
      break;
    }
  }
  platform_critical_exit();
}      

/*-------------------------------------------------------------------------------
 *  NAME:   timer_event_system_init()
 *
 *  PARAMETERS:
 *   None.
 *
 *  RETURNS:
 *   void
 *
 *  DESCRIPTION:
 *   Initialize timer system.  Create timer pool, initilize lists, call
 *   routine to initialize timer interrupt  ticker.
 *
 *----------------------------------------------------------------------------*/
void timer_event_system_init(void)
{
  int i;    /* Index */

  /* Create timer lists and free buffer pool */
  timer_list = NULL;
  timer_free = NULL;

  for (i = 0; i < MAX_TIMERS; i++){
    timers[i].next = timer_free;
    timers[i].flags = TIMER_FREE;
    timer_free = &timers[i];
  }

  expired_count = 0;
  removed_count = 0;
  inserted_count = 0;

  platform_timer_init();
}


#ifdef TIMER_DEBUG
static int free_list_count()
{
   timer_struct_type *timer;
   int count;
 
   count = 0;

   platform_critical_enter();
   timer = timer_free;
 
   while (timer != NULL)
   {
      count++;
      timer=timer->next;
   }
   platform_critical_exit();
   return(count);
}
 

static void display_timer(timer_struct_type *timer)
{
  err_msg("TIMER: %08p\n",timer);
  err_msg("expiration: %d, interval: %d, parameter: 0x%08p, flags: 0x%08x\n",
     timer->expiration_time, timer->interval, timer->parameter,
     timer->flags);
  err_msg("call_back: 0x%08p, pred: %08p, next: %08p\n\n",
          timer->expiration_callback, timer->pred, timer->next);
}



int display_active_timers()
{
  timer_struct_type *timer;

  err_msg("\n\nTimer List - Current Time: %d \n",current_time());
  err_msg("Expired: %d, Canceled: %d, Started: %d, Free List: %d\n\n",
         expired_count, removed_count, inserted_count, free_list_count());
  timer = timer_list;
  
  while (timer != NULL)
  {
    display_timer(timer);
    timer = timer->next;
  }

  expired_count = 0;
  removed_count = 0;
  inserted_count = 0;
  return(0);
}


#endif // TIMER_DEBUG
