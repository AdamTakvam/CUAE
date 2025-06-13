/*
 *  Copyright (c) 2001, 2002, 2003, 2004 by Cisco Systems, Inc. All Rights Reserved.
 *
 *  This work is subject to U.S. and international copyright laws and
 *  treaties. No part of this work may be used, practiced, performed,
 *  copied, distributed, revised, modified, translated, abridged, condensed,
 *  expanded, collected, compiled, linked, recast, transformed or adapted
 *  without the prior written consent of Cisco Systems, Inc. Any use or 
 *  exploitation of this work without authorization could subject the 
 *  perpetrator to criminal and civil liability.
 *
 *  FILENAME
 *     sem.h
 *
 *  CREATION/MODIFICATION DATES
 *     Created:  June 2000, Sam Hague
 *     Modified: 
 *
 *  DESCRIPTION
 *     State Machine header file
 */
#ifndef __SEM_H__
#define __SEM_H__


#define SM_SE_INVALID (-1)


struct sem_event_t_;

typedef int (*sem_function_t)(struct sem_event_t_ *event);
typedef char* sem_name_f(int id);
typedef void sem_debug_entry_f(struct sem_event_t_ *event);
typedef int sem_validate_cb_f(void *cb1, void *cb2);
typedef void sem_change_state_f(void *cb, int state, char *function_name);

typedef struct sem_events_t_ {
    int event_id;
    int function_id;
    int next_state_id;
} sem_events_t;

#define SEM_EVENTS_SIZE sizeof(sem_events_t)

typedef struct sem_states_t_ {
    sem_events_t *events;
    int          event_count;
} sem_states_t;

typedef struct sem_tbl_t_ {
    sem_states_t *states;
    int          max_state;
    int          max_event;
} sem_tbl_t;

typedef struct sem_table_t_ {
    sem_tbl_t          *table;
    sem_function_t     *functions;
    sem_name_f         *state_name;
    sem_name_f         *event_name;
    sem_name_f         *sem_name;    
    sem_name_f         *function_name;
    sem_debug_entry_f  *debug_entry;
    sem_validate_cb_f  *validate_cb;
    sem_change_state_f *change_state;
} sem_table_t;

typedef struct sem_event_t_ {
    struct sem_event_t_ *next;
    int  id;
    sem_table_t *table;
    void *cb;
    void *cb2;
    int  state_id;
    int  event_id;
    char *function_name;
    void *data;
} sem_event_t;

typedef struct sem_cbhdr_t_ {
    void        *next;
    int         id;
    int         state;
    int         old_state;
    sem_table_t *table;
} sem_cbhdr_t;

int sem_process_event(sem_event_t *event);

#endif /* __SEM_H__ */
