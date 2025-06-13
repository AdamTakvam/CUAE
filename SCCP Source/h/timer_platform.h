/*
 *------------------------------------------------------------------
 * timer_platform.h - platform specific defines used by generic
 *                    asynchronous timer service.  
 *
 *
 * Copyright (c) 2003, 2004 by Cisco Systems, Inc.
 * All rights reserved.
 *------------------------------------------------------------------
 */


// Macro for concurrency protection when entering critical area
//#define platform_critical_enter() 
extern void platform_critical_enter(void);

// Macro for exiting critical area
//#define platform_critical_exit()  
extern void platform_critical_exit(void);

// Macro to start timer tick running on current platform
//#define platform_timer_init()
extern void platform_timer_init(void);

// Macro for returning the current elapsed time.  This should return
// the time in "TICKS_PER_SECOND" units.
//#define current_time() (1)
extern unsigned long current_time(void);

// Convert millisecond count into system tick count
extern int platform_ms_to_ticks(int milliseconds);

// Define for error message printing. Printf paramaters assumed
#define err_msg printf

#if 1 /* WINDOWS */
// Number of timer ticks per second
#define TICKS_PER_SECOND    1           

#define TICK_INTERVAL       1000        /* Milliseconds per each tick interval */

#else /* PSOS */

// Number of timer ticks per second
#define TICKS_PER_SECOND    20           

#define TICK_INTERVAL       50          /* Milliseconds per each tick interval */

#endif

// Maximum number of timers supported
#define MAX_TIMERS  20 * 1                      
