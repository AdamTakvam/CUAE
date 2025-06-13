/*
 *  Copyright (c) 2002, 2003 by Cisco Systems, Inc. All Rights Reserved.
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
 *     am.h
 *
 *  CREATION/MODIFICATION DATES
 *     Created:  November 2002, Sam Hague
 *     Modified: 
 *
 *  DESCRIPTION
 *     Application Manager header file
 */
#ifndef _AM_H_
#define _AM_H_

#include "gapi.h"


gapi_callbacks_t *am_get_am_callbacks(void *handle);
int am_set_app_callbacks(void *amhandle, gapi_callbacks_t *appcbs,
                         void *apphandle);
int am_init(void);
int am_cleanup(void);
void *am_get_new_handle(gapi_callbacks_t *appcbs,
                        void *apphandle, char *appname);

#endif /* _AM_H_ */
