/*
 *------------------------------------------------------------------
 * timer_platform_windows.c  - Generic timer support for windows based systems
 *
 * Copyright (c)2003 by Cisco Systems, Inc.
 * All rights reserved.
 *------------------------------------------------------------------
 */
#include <stdio.h>
#include "timer_platform.h"
#include "timer.h"
#include <windows.h>
#include <process.h>    /* _beginthread, _endthread */


static unsigned long ticker = 0;        /* Timer ticker */
static CRITICAL_SECTION critical_section;     /* Mutex for critical section */


void platform_critical_enter()
{
    EnterCriticalSection(&critical_section);
}

void platform_critical_exit()
{
    LeaveCriticalSection(&critical_section);
}

int platform_ms_to_ticks(int milliseconds)
{
    // Round up to insure each timeout is at least 1 tick
    return((milliseconds + (TICK_INTERVAL - 1)) / TICK_INTERVAL);
}

void TimerTask(void *param)
{

    printf("\nTimer task started\n");

    while (1)
    {
      Sleep(TICK_INTERVAL);
      ticker++;
      platform_critical_enter();
      timer_event_process();  /* Platform independent timer processing */
      platform_critical_exit();
    }
}

unsigned long current_time(void)
{
    return(ticker);
}


/* Called by timer_event_system_init() to init platform timer stuff */
void platform_timer_init (void)
{
    ticker = 0;

    InitializeCriticalSection(&critical_section);

    // Startup timer task
    _beginthread(TimerTask,0,NULL);

    

}
