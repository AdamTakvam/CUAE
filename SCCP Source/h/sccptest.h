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
 *     sccptest.h
 *
 *  CREATION/MODIFICATION DATES
 *     Created:  November 2002, Sam Hague
 *     Modified: 
 *
 *  DESCRIPTION
 *     SCCP Test Application header file
 */
#ifndef _SCCPTEST_H_
#define _SCCPTEST_H_

#ifdef SCCP_SAPP
#ifdef __cplusplus
extern "C" {
#endif
#endif /*SCCP_SAPP */

//#include "sccp_ice_events.h"
#include "gapi.h"


int sccptest_get_new_call_id(void);
void sccptest_callback(int event, void *data, void *callback_data);
void sccptest_closesession_res(void);
void sccptest_sessionstatus(gapi_status_e status);
void sccptest_offhook(int conn_id, int line, int flag);
void sccptest_setup(int conn_id, int line, gapi_conninfo_t *conninfo);
void sccptest_setup_ack(int conn_id, int line, gapi_causes_e cause);
void sccptest_proceeding(int conn_id, int line);
void sccptest_alerting(int conn_id, int line);
void sccptest_connect(int conn_id, int line);
void sccptest_connect_ack(int conn_id, int line);
void sccptest_release(int conn_id, int line);
void sccptest_release_complete(int conn_id, int line);
void sccptest_feature_req(int conn_id, int line, int feature);
void sccptest_starttone(int conn_id, int line, int tone);

#ifdef SCCP_SAPP
#ifdef __cplusplus
}
#endif
#endif /* SCCP_SAPP */

#endif /* _SCCPTEST_H_ */
