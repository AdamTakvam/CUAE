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
 *     am.c
 *
 *  CREATION/MODIFICATION DATES
 *     Created:  November 2002, Sam Hague
 *     Modified: 
 *
 *  DESCRIPTION
 *     Application Manager implementation
 */
#include "sccp.h"
#include "sccp_debug.h"
#include "am.h"
#include "gapi.h"


int am_init (void)
{
    int rc;

    rc = sccp_init(); 
    if (rc != 0) {
        SCCP_DBG((am_debug, 9, 
                 "%s:am_init: ERROR: sccp_init: rc= %d\n", "yo"));
        return (11);
    }

    return (0);
}

int am_cleanup (void)
{
    sccp_cleanup();

    return (0);
}

gapi_callbacks_t *am_get_am_callbacks (void *handle)
{
    return (sccp_get_callbacks(handle));
}

static int am_verify_app_callbacks (gapi_callbacks_t *cbs)
{
    if (cbs == NULL) {
        return (1);
    }

    return (0);
}

void am_free_handle (void *handle)
{
    sccp_free_sccpcb(handle);
}

void *am_get_new_handle (gapi_callbacks_t *appcbs,
                         void *apphandle, char *appname)
{
    sccp_sccpcb_t *sccpcb;
    int           rc = -1;

    rc = am_verify_app_callbacks(appcbs);
    if (rc != 0) {
        return (NULL);
    }

    sccpcb = sccp_get_new_sccpcb();
    if (sccpcb == NULL) {
        return (NULL);
    }

    SCCP_MEMCPY(sccpcb->appcbs, appcbs, sizeof(*(sccpcb->appcbs)));

    SCCP_STRNCPY(sccpcb->appname, appname, sizeof(sccpcb->appname));
    sccpcb->appname[SCCP_APPNAME_SIZE] = '\0';

    gapi_check_and_set_callbacks(sccpcb->appcbs);

    sccpcb->apphandle = apphandle;

    return (sccpcb);
}
