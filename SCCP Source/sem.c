/*
 *  Copyright (c) 2000, 2001, 2002, 2003, 2004 by Cisco Systems, Inc. All Rights Reserved.
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
 *     sem.c
 *
 *  CREATION/MODIFICATION DATES
 *     Created:  June 2000, Sam Hague
 *     Modified: 
 *
 *  DESCRIPTION
 *     State Machine implementation
 */
#include "sccp.h"
#include "sem.h"
#include "sccp_debug.h"


int sem_process_event (sem_event_t *event)
{
    int          state_id = event->state_id;
    int          event_id = event->event_id;
    sem_table_t  *table   = event->table;
    sem_events_t *ev;
    sem_states_t *st;
    sem_tbl_t    *tbl;
    sem_cbhdr_t  *cb      = event->cb;
    
    /*
     * Make sure the table is valid.
     */
    if ((table == NULL) || (table->table == NULL)){
        return (1);
    }

    tbl = table->table;

    /*
     * Make sure the state and event are valid.
     */
    if (((event_id < 0) || (event_id >= tbl->max_event)) ||
        ((state_id < 0) || (state_id >= tbl->max_state))) {
        return (2);
    }

    st = &(tbl->states[state_id]);

    for (ev = st->events; ev < &(st->events[st->event_count]); ev++) {

        /*
         * Find the matching event entry
         */
        if (ev->event_id == event_id) {
            /*
             * Make sure we have a function.
             */
            if (table->functions[ev->function_id] != NULL) {
                event->function_name = table->function_name(ev->function_id);

                if ((sccp_debug_show(table, -1) == 1) || (1)) {
                    SCCP_DBG((sem_debug, 9, "\n"));
                    SCCP_DBG((sem_debug, 9, "%s %-5d: [%s] [%s] [%s]\n",
                             table->sem_name(0), cb->id,
                             table->state_name(state_id), table->event_name(event_id),
                             table->state_name(ev->next_state_id)));
                }

                /*
                 * Set the new state.
                 */
                cb->old_state = cb->state;
                cb->state     = ev->next_state_id;

                /*
                 * Call the sem function.
                 */
                table->functions[ev->function_id](event);

                return (0);
            }
            else {
                SCCP_DBG((sem_debug, 9,
                         "%s %-5d: NULL function: [%s] [%s]\n",
                         table->sem_name(0), cb->id,
                         table->state_name(state_id),
                         table->event_name(event_id)));
                    
                return (11);      
            }
        }
    }

    SCCP_DBG((sem_debug, 9,
             "%s %-5d: not handled: [%s] [%s]\n",
             table->sem_name(0), cb->id,
             table->state_name(state_id),
             table->event_name(event_id)));

    return (21);
}
