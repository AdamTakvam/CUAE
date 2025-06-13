/*
 *------------------------------------------------------------------
 * windows_platform_timer.c  - Generic timer support for windows based systems
 *
 * Copyright (c)2003 by Cisco Systems, Inc.
 * All rights reserved.
 *------------------------------------------------------------------
 */
//#define _REENTRANT
#include <stdio.h>
#include "timer_platform.h"
#include "timer.h"
#include "platform.h"
//#include <errno.h>

extern int errno;

#ifndef SAPP_PLATFORM_UNIX_WIN  // used when compiling under windows
#include <pthread.h>
#include <unistd.h>
#endif

static unsigned long ticker = 0;        /* Timer ticker */
static pthread_mutex_t critical_section;
static pthread_t tid;

void platform_critical_enter()
{
    pthread_mutex_lock(&critical_section);
}

void platform_critical_exit()
{
    pthread_mutex_unlock(&critical_section);
}

int platform_ms_to_ticks(int milliseconds)
{

    // Round up to insure each timeout is at least 1 tick
    return((milliseconds + (TICK_INTERVAL - 1)) / TICK_INTERVAL);
}

void *TimerTask(void *param)
{

    printf("\nTimer task started\n");

    while (1)
    {
      usleep(TICK_INTERVAL*1000 - 100); /* microseconds */
      ticker++;
      timer_event_process();  /* Platform independent timer processing */
    }

    return (NULL);
}

unsigned long current_time(void)
{
    return(ticker);
}

/* Called by timer_event_system_init() to init platform timer stuff */
void platform_timer_init (void)
{
    int rc;

    ticker = 0;

    pthread_mutex_init(&critical_section, NULL);

    rc = pthread_create(&tid, NULL, TimerTask, NULL);
    if (rc != 0) {
        printf("platform_timer_init: ERROR: rc= %d\n", rc);
    }
}
