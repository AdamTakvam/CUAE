/*
 *  Copyright (c) 2002-2004 by Cisco Systems, Inc. All Rights Reserved.
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
 *     sllist.h
 *
 *  CREATION/MODIFICATION DATES
 *     Created:  September 2001, Sam Hague
 *     Modified:
 *
 *  DESCRIPTION
 *     Single linked list header file
 */
#ifndef __SLLIST_H__
#define __SLLIST_H__

#include "ssapi.h"


typedef void (sllist_free_f)(void *);

typedef struct sllist_node_t_ {
    struct sllist_node_t_ *next;
    int                   id;
    int                   size;
    ssapi_mutex_t         lock;
    sllist_free_f         *free;
    struct sllist_node_t_ *curr;
    char                  name[9];
} sllist_node_t;


void *sllist_create_list(sllist_free_f *free_func, char *name);
void *sllist_delete_list(void *sllist);
void *sllist_empty_list(void *sllist);
void *sllist_add_node(void *sllist, void *node, int front);
void *sllist_find_node(void *sllist, void *slnode);
void *sllist_delete_node(void *sllist, void *node);
void *sllist_get_last_node(void *sllist);
void *sllist_get_node_by_id(void *sllist, int id);
void *sllist_remove_node(void *sllist, int front);
int  sllist_get_list_size(void *sllist);
int sllist_get_new_id(void *sllist, int *id);

int sllist_init(void);
int sllist_cleanup(void);

#endif /* __SLLIST_H__ */
