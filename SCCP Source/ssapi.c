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
 *     ssapi.c
 *
 *  CREATION/MODIFICATION DATES
 *     Created:  November 2002, Sam Hague
 *     Modified: 
 *
 *  DESCRIPTION
 *     System Services API implementation
 */
#include <stdio.h>
#include "ssapi.h"


ssapi_callbacks_t *ssapi_cbs = NULL;
static int ssapi_initialized = 0;


static int ssapi_verify_app_callbacks (ssapi_callbacks_t *cbs)
{
    if (cbs == NULL) {
        return (1);
    }
    if ((cbs->timer_allocate      == NULL) ||
        (cbs->timer_initialize    == NULL) ||
        (cbs->timer_activate      == NULL) ||
        (cbs->timer_cancel        == NULL) ||
        (cbs->timer_free          == NULL) ||
        (cbs->malloc              == NULL) ||
        (cbs->free                == NULL) ||
        (cbs->memset              == NULL) ||
        (cbs->memcpy              == NULL) ||
        (cbs->socket_open         == NULL) ||
        (cbs->socket_close        == NULL) ||
        (cbs->socket_connect      == NULL) ||
        (cbs->socket_send         == NULL) ||
        (cbs->socket_recv         == NULL) ||
        (cbs->socket_getlasterror == NULL) ||
        (cbs->socket_getsockname  == NULL) ||
        (cbs->socket_getmac       == NULL) ||
        (cbs->htons               == NULL) ||
        (cbs->htonl               == NULL) ||
        (cbs->ntohs               == NULL) ||
        (cbs->ntohl               == NULL) ||
        (cbs->mutex_create        == NULL) ||
        (cbs->mutex_lock          == NULL) ||
        (cbs->mutex_unlock        == NULL) ||
        (cbs->thread_get          == NULL) ||
        (cbs->thread_run          == NULL) ||
        (cbs->printf              == NULL) ||
        (cbs->snprintf            == NULL) ||
        (cbs->vsnprintf           == NULL) ||
        (cbs->strncpy             == NULL) ||
        (cbs->strtime             == NULL)) {
        return (2);
    }

    return (0);
}

int ssapi_init (void)
{
    if (ssapi_initialized != 1) {
        ssapi_initialized = 1;
    }

    return (0);
}

int ssapi_cleanup (void)
{
    if (ssapi_initialized != 0) {
        if (ssapi_cbs != NULL) {
            ssapi_cbs->free(ssapi_cbs);
            ssapi_cbs = NULL;
        }
    }

    return (0);
}

int ssapi_bind (ssapi_callbacks_t *appcbs)
{
    int rc;

    ssapi_init();

    if (ssapi_initialized != 1) {
        return (1);
    }

    rc = ssapi_verify_app_callbacks(appcbs);
    if (rc != 0) {
        return (1);
    }

    ssapi_cbs = (ssapi_callbacks_t *)(appcbs->malloc(sizeof(*ssapi_cbs)));
    if (ssapi_cbs == NULL) {
        return (2);
    }

    appcbs->memcpy(ssapi_cbs, appcbs, sizeof(*ssapi_cbs));

    return (0);
}
