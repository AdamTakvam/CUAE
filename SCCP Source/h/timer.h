/*
 *------------------------------------------------------------------
 * timer.h  - Prototypes for asynchronous timer service.
 *
 *
 * Copyright (c) 2003 by Cisco Systems, Inc.
 * All rights reserved.
 *------------------------------------------------------------------
 */


#ifndef TIMER_H
#define TIMER_H

#include "timer_platform.h"


// Requires inclusion of "platform_timer.h"
#define SECONDS(x)  (TICKS_PER_SECOND*x)

extern unsigned long current_time(void);
extern void timer_event_activate(void *timer);
extern void *timer_event_allocate(void);
extern void timer_event_cancel(void *timer);
extern void timer_event_free(void *timer);
extern void timer_event_initialize(void *timer,
                                   int period,
                                   void (*expiration)(void *timer_event, 
                                                      void *param1,
                                                      void *param2),
                                   void *param1,
                                   void *param2);
extern void timer_event_process(void);
extern void timer_event_system_init(void);


#endif
