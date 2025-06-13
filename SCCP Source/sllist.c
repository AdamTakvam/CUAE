/*
 *  Copyright (c) 2002, 2003, 2004 by Cisco Systems, Inc. All Rights Reserved.
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
 *     slist.c
 *
 *  CREATION/MODIFICATION DATES
 *     Created:  May 2002, Sam Hague
 *     Modified:
 *
 *  DESCRIPTION
 *     Linked list implementation
 */
#include "sccp_platform.h" /* defines malloc, ..., NULL */
#include "sllist.h"

#define sllist_malloc        SCCP_MALLOC
#define sllist_memset        SCCP_MEMSET
#define sllist_memcpy        SCCP_MEMCPY
#define sllist_free          SCCP_FREE
#define sllist_lock          SCCP_MUTEX_LOCK
#define sllist_unlock        SCCP_MUTEX_UNLOCK
#define sllist_create_lock   SCCP_MUTEX_CREATE
#define sllist_delete_lock   SCCP_MUTEX_DELETE


#ifdef SCCP_USE_POOL

#ifdef SCCP_USE_POOL5
#define SLLIST_POOL_LIST_CNT  (9)  /* make sure this holds all lists */
#else
/*
 * 1 sccpcb list
 * per sccpcb: 1 cccb list, 1 cmcb list, 1 event list
 * per pool: 4 pools
 * = 8 lists for a single insatnce
 *----------
 * Each additional instance adds three lists:
 *   each additional sccpcb adds the three lists per sccpcb.
 */
#define SLLIST_POOL_LIST_CNT  (8)  /* make sure this holds all lists */
#endif
#define SLLIST_POOL_LIST_SIZE (36) /* make sure this holds sllist_node_t */

static unsigned long sllist_pool_list[SLLIST_POOL_LIST_CNT][SLLIST_POOL_LIST_SIZE/4];
static int sllist_initialized = 0;

static void *sllist_malloc_head (unsigned int size)
{
    sllist_node_t *node;
    sllist_node_t *node_found = NULL; 

    for (node = (sllist_node_t *)sllist_pool_list;
         node < (sllist_node_t *)(&(sllist_pool_list[SLLIST_POOL_LIST_CNT][0]));
         node++) {

        if (node->id == 0) {
            node_found = node;
            break;
        }
    }

    return (node_found);
}

static void sllist_free_head (void *list)
{
    sllist_node_t *node;

    for (node = (sllist_node_t *)sllist_pool_list;
         node < (sllist_node_t *)(&(sllist_pool_list[SLLIST_POOL_LIST_CNT][0]));
         node++) {

        if (node == list) {
            node->id = 0;
            break;
        }
    }
}

#ifdef sllist_malloc
#undef sllist_malloc
#undef sllist_free
#define sllist_malloc sllist_malloc_head
#define sllist_free sllist_free_head
#endif
#endif /* SCCP_USE_POOL */

#define sllist_print_name(a, b, c) //void(0)
#if 0
void sllist_print_name (sllist_node_t *list, int which, char *fname)
{
    if (list->name[0] == 'e') {
        printf("\nsllist: %s: %d:%d:0x%08x\n",
               fname, which, list->size, list->next);
    }
}
#endif

void *sllist_create_list (sllist_free_f *free_func, char *name)
{
    sllist_node_t *list;
    int rc;

#ifdef SCCP_USE_POOL1
    list = sllist_malloc_head();
#else
    list = (sllist_node_t *)sllist_malloc(sizeof(sllist_node_t));
#endif

    if ((list != NULL) && (free_func != NULL)) {
        sllist_memset(list, 0, sizeof(*list));
        rc = sllist_create_lock(0, &(list->lock));
        if (rc != 0) {
#ifdef SCCP_USE_POOL1
            sllist_free_head(list);
#else
            sllist_free(list);
#endif
            list = NULL;

            return (NULL);
        }

        list->next = NULL;
        list->id   = -1;
        list->size = 0;
        list->curr = list;
        list->free = free_func;
        sllist_memcpy(list->name, name, sizeof(list->name));
        list->name[sizeof(list->name) - 1] = '\0';
    }

    return (list);
}

void *sllist_empty_list (void *sllist)
{
    sllist_node_t *list = sllist;
    sllist_node_t *curr = sllist;
    sllist_node_t *next;

    /*
     * Make sure the list is valid.
     */
    if (list == NULL) {
        return (NULL);
    }

    sllist_print_name(list, 0, "el");
    sllist_lock(list->lock, SCCP_MUTEX_TIMEOUT_INFINITE);

    /*
     * Traverse the list and free each node.
     */
    for (curr = curr->next; curr != NULL; curr = next) {
        /*
         * Make sure we remember the next node, since we will
         * free the current node and lose the pointer.
         */
        next = curr->next;

        list->free(curr);
    }

    list->size = 0;
    list->next = NULL;

    sllist_print_name(list, 1, "el");
    sllist_unlock(list->lock);
    
    return (list);
}

void *sllist_delete_list (void *sllist)
{
    sllist_node_t *list = sllist;
    sllist_node_t *curr = sllist;
    sllist_node_t *next;

    /*
     * Make sure the list is valid.
     */
    if (list == NULL) {
        return (NULL);
    }
    sllist_print_name(list, 0, "dl");
    sllist_lock(list->lock, SCCP_MUTEX_TIMEOUT_INFINITE);

    /*
     * Traverse the list and free each node.
     */
    for (curr = curr->next; curr != NULL; curr = next) {
        /*
         * Remember the next node, since the current node will be freed 
         * and the pointer to next will be lost.
         */
        next = curr->next;

        list->free(curr);
    }

    sllist_print_name(list, 1, "dl");
    sllist_unlock(list->lock);
    
    sllist_delete_lock(list->lock);
    
    /*
     * Free the head node of the list.
     */
#ifdef SCCP_USE_POOL1
    sllist_free_head(list);
#else
    sllist_free(list);
#endif

    return (NULL);
}

void *sllist_add_node (void *sllist, void *slnode, int front)
{
    sllist_node_t *list = sllist;
    sllist_node_t *curr = sllist;
    sllist_node_t *node = slnode;

    /*
     * Make sure the list and node are valid.
     */
    if ((list == NULL) || (slnode == NULL)) {
        return (NULL);
    }
    sllist_print_name(list, 0, "an");
    sllist_lock(list->lock, SCCP_MUTEX_TIMEOUT_INFINITE);

    list->size++;

    /*
     * Add to the front of the list or to the end.
     */
    if (front == 1) {
        /*
         * Insert at the front.
         */
        node->next = list->next;
        list->next = node;
    }
    else {
        /*
         * Insert at the end. Traverse the list to find the end.
         */
        while (curr->next != NULL) {
            curr = curr->next;
        }

        /*
         * Insert at the end.
         */
        node->next = NULL;
        curr->next = node;
    }
    sllist_print_name(list, 1, "an");
    sllist_unlock(list->lock);

    return (node);
}

void *sllist_find_node (void *sllist, void *slnode)
{
    sllist_node_t *list  = sllist;
    sllist_node_t *found = NULL;
    sllist_node_t *curr  = sllist;

    /*
     * Make sure the list and node are valid.
     */
    if ((list == NULL) || (slnode == NULL)) {
        return (NULL);
    }
    sllist_print_name(list, 0, "fn");
    sllist_lock(list->lock, SCCP_MUTEX_TIMEOUT_INFINITE);

    /*
     * Taverse the list and find the node.
     */
    for (curr = curr->next; curr != NULL; curr = curr->next) {
        if (curr == slnode) {
            found = curr;
            break;
        }
    }
    sllist_print_name(list, 1, "fn");
    sllist_unlock(list->lock);

    return (found);
}

void *sllist_delete_node (void *sllist, void *slnode)
{
    sllist_node_t *list = sllist;
    sllist_node_t *prev = sllist;
    sllist_node_t *curr = sllist;

    /*
     * Make sure the list and node are valid.
     */
    if ((list == NULL) || (slnode == NULL)) {
        return (NULL);
    }

    sllist_print_name(list, 0, "dn");
    sllist_lock(list->lock, SCCP_MUTEX_TIMEOUT_INFINITE);

    /*
     * Taverse the list and find the node.
     */
    for (curr = curr->next; curr != NULL; curr = curr->next) {
        if (curr == slnode) {
            list->size--;

            prev->next = curr->next;
        
            list->free(curr);

            break;
        }
        
        prev = curr;
    }

    sllist_print_name(list, 1, "dn");
    sllist_unlock(list->lock);

    return (NULL);
}

void *sllist_get_last_node (void *sllist)
{
    sllist_node_t *list = sllist;
    sllist_node_t *prev = NULL;
    sllist_node_t *curr = sllist;

    /*
     * Make sure the list is valid.
     */
    if (list == NULL) {
        return (NULL);
    }
    sllist_print_name(list, 0, "ln");
    sllist_lock(list->lock, SCCP_MUTEX_TIMEOUT_INFINITE);
    
    /*
     * Taverse the list and find the last node.
     */
    for (curr = curr->next; curr != NULL; curr = curr->next) {
        prev = curr;
    }
    sllist_print_name(list, 1, "ln");
    sllist_unlock(list->lock);

    return (prev);
}


void *sllist_get_node_by_id (void *sllist, int id)
{
    sllist_node_t *list = sllist;
    sllist_node_t *found = NULL;
    sllist_node_t *curr = sllist;

    /*
     * Make sure the list is valid.
     */
    if (list == NULL) {
        return (NULL);
    }
    sllist_print_name(list, 0, "gn");
    sllist_lock(list->lock, SCCP_MUTEX_TIMEOUT_INFINITE);
    
    /*
     * Taverse the list and find the node.
     */
    for (curr = curr->next; curr != NULL; curr = curr->next) {
        if (curr->id == id) {
            found = curr;
            break;
        }
    }
    sllist_print_name(list, 1, "gn");
    sllist_unlock(list->lock);

    return (found);
}

void *sllist_remove_node (void *sllist, int front)
{
    sllist_node_t *list = sllist;
    sllist_node_t *prev = sllist;
    sllist_node_t *curr = sllist;

    /*
     * Make sure the list is valid.
     */
    if (list == NULL) {
        return (NULL);
    }
    sllist_print_name(list, 0, "rn");
    sllist_lock(list->lock, SCCP_MUTEX_TIMEOUT_INFINITE);
    
    /*
     * Make sure the list is not empty.
     */
    if (list->next == NULL) {
        sllist_print_name(list, 2, "rn");
        sllist_unlock(list->lock);

        return (NULL);
    }

    list->size--;

    /*
     * Remove from the front of the list or the end.
     */
    if (front == 1) {
        curr = list->next;
        list->next = curr->next;
    }
    else {
        /*
         * Traverse the list to find the end.
         */
        for (curr = list->next; curr->next != NULL; curr = curr->next) {
            /*
             * Remember the previous node, because we will need to
             * mark it as the end.
             */
            prev = curr;
        }

        prev->next = NULL;
    }
    sllist_print_name(list, 1, "rn");
    sllist_unlock(list->lock);

    return (curr);
}

int sllist_get_list_size (void *sllist)
{
    sllist_node_t *list = sllist;
    int           size;

    /*
     * Make sure the list is valid.
     */
    if (list == NULL) {
        return (0);
    }
    sllist_print_name(list, 0, "gl");
    sllist_lock(list->lock, SCCP_MUTEX_TIMEOUT_INFINITE);
    
    size = list->size;
    sllist_print_name(list, 1, "gl");
    sllist_unlock(list->lock);

    return (size);
}

int sllist_get_new_id (void *sllist, int *id)
{
    sllist_node_t *list = sllist;

    /*
     * Make sure the list is valid.
     */
    if (list == NULL) {
        return (0);
    }
    sllist_print_name(list, 0, "gl");
    sllist_lock(list->lock, SCCP_MUTEX_TIMEOUT_INFINITE);
    
    if (++(*id) < 0) {
        *id = 1;
    }
    
    sllist_unlock(list->lock);

    return (*id);
}

int sllist_init (void)
{
#ifdef SCCP_USE_POOL
    if (sllist_initialized == 0) {
        sllist_memset(sllist_pool_list, 0, sizeof(sllist_pool_list));
        sllist_initialized  = 1;
    }
#endif
    return (0);
}

int sllist_cleanup (void)
{
#ifdef SCCP_USE_POOL
    int i, j;

    for (i = 0; i < SLLIST_POOL_LIST_CNT; i++) {
        for (j = 0; j < SLLIST_POOL_LIST_SIZE/4; j++) {
            sllist_pool_list[SLLIST_POOL_LIST_CNT][SLLIST_POOL_LIST_SIZE/4] = 0;
        }
    }
    //sllist_memset(sllist_pool_list, 0, sizeof(sllist_pool_list));
#endif
    return (0);
}
