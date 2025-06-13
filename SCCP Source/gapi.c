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
 *     gapi.c
 *
 *  CREATION/MODIFICATION DATES
 *     Created:  November 2002, Sam Hague
 *     Modified: 
 *
 *  DESCRIPTION
 *     Generic Call Control API implementation
 */
#include "sccp_platform.h"
#include "gapi.h"
#include "am.h"


static int gapi_initialized = 0;

int gapi_init (void)
{
    int gapi_rc = 0;

    if (gapi_initialized != 1) {
        gapi_rc = am_init();
        if (gapi_rc != 0) {
            return (12);
        }

        gapi_initialized = 1;
    }

    return (0);
}

int gapi_cleanup (void)
{
    if (gapi_initialized != 0) {
        am_cleanup();

        gapi_initialized = 0;
    }

    return (0);
}

int gapi_bind (gapi_callbacks_t *appcbs, gapi_callbacks_t **gapicbs,
               void *apphandle, void **gapihandle, char *appname)
{
    gapi_init();

    if (gapi_initialized != 1) {
        return (1);
    }

    *gapihandle = am_get_new_handle(appcbs, apphandle, appname);
    if (*gapihandle == NULL) {
        return (1);
    }

    /*
     * Copy the gapi callbacks.
     */
    *gapicbs = am_get_am_callbacks(*gapihandle);
    if (*gapicbs == NULL) {
        return (11);
    }

    return (0);
}

int gapi_get_new_conn_id (void)
{
    static int id = 0;

    if (++id < 0) {
        id = 1;
    }

    return (id);
}

static int gapi_null_callback (void)
{
    return (0);
}

void gapi_check_and_set_callbacks (gapi_callbacks_t *cbs)
{
    unsigned long *func = (unsigned long *)cbs;
    int numfuncs = sizeof(*cbs) / sizeof(unsigned long *);
    int i;

    for (i = 0; i < numfuncs; i++) {
        if (*func == (unsigned long)NULL) {
            *func = (unsigned long)gapi_null_callback;
        }

        func++;
    }
#if 0
    if (cbs->setup                      == NULL) cbs->setup                     = (gapi_setup_f *)gapi_null_callback;
    if (cbs->setup_ack                  == NULL) cbs->setup_ack                 = (gapi_setup_ack_f *)gapi_null_callback;
    if (cbs->proceeding                 == NULL) cbs->proceeding                = (gapi_proceeding_f *)gapi_null_callback;
    if (cbs->alerting                   == NULL) cbs->alerting                  = (gapi_alerting_f *)gapi_null_callback;
    if (cbs->connect                    == NULL) cbs->connect                   = (gapi_connect_f *)gapi_null_callback;
    if (cbs->connect_ack                == NULL) cbs->connect_ack               = (gapi_connect_ack_f *)gapi_null_callback;
    if (cbs->disconnect                 == NULL) cbs->disconnect                = (gapi_disconnect_f *)gapi_null_callback;
    if (cbs->release                    == NULL) cbs->release                   = (gapi_release_f *)gapi_null_callback;
    if (cbs->release_complete           == NULL) cbs->release_complete          = (gapi_release_complete_f *)gapi_null_callback;

    if (cbs->softkey                    == NULL) cbs->softkey                   = (gapi_softkey_f *)gapi_null_callback;       
    if (cbs->feature_req                == NULL) cbs->feature_req               = (gapi_feature_req_f *)gapi_null_callback;
    if (cbs->feature_res                == NULL) cbs->feature_res               = (gapi_feature_res_f *)gapi_null_callback;

    if (cbs->openrcv_req                == NULL) cbs->openrcv_req               = (gapi_openrcv_req_f *)gapi_null_callback;
    if (cbs->openrcv_res                == NULL) cbs->openrcv_res               = (gapi_openrcv_res_f *)gapi_null_callback;
    if (cbs->closercv                   == NULL) cbs->closercv                  = (gapi_closercv_f *)gapi_null_callback;
    if (cbs->startxmit                  == NULL) cbs->startxmit                 = (gapi_startxmit_f *)gapi_null_callback;
    if (cbs->stopxmit                   == NULL) cbs->stopxmit                  = (gapi_stopxmit_f *)gapi_null_callback;

    if (cbs->offhook                    == NULL) cbs->offhook                   = (gapi_offhook_f *)gapi_null_callback;
    if (cbs->onhook                     == NULL) cbs->onhook                    = (gapi_onhook_f *)gapi_null_callback;
    if (cbs->hookflash                  == NULL) cbs->hookflash                 = (gapi_hookflash_f *)gapi_null_callback;
    if (cbs->digits                     == NULL) cbs->digits                    = (gapi_digits_f *)gapi_null_callback;
    if (cbs->starttone                  == NULL) cbs->starttone                 = (gapi_starttone_f *)gapi_null_callback;
    if (cbs->stoptone                   == NULL) cbs->stoptone                  = (gapi_stoptone_f *)gapi_null_callback;
    if (cbs->ringer                     == NULL) cbs->ringer                    = (gapi_ringer_f *)gapi_null_callback;
    if (cbs->lamp                       == NULL) cbs->lamp                      = (gapi_lamp_f *)gapi_null_callback;
    if (cbs->speaker                    == NULL) cbs->speaker                   = (gapi_speaker_f *)gapi_null_callback;
    if (cbs->micro                      == NULL) cbs->micro                     = (gapi_micro_f *)gapi_null_callback;
    if (cbs->headset                    == NULL) cbs->headset                   = (gapi_headset_f *)gapi_null_callback;
    if (cbs->timedate                   == NULL) cbs->timedate                  = (gapi_timedate_f *)gapi_null_callback;
    if (cbs->conninfo                   == NULL) cbs->conninfo                  = (gapi_conninfo_f *)gapi_null_callback;
    if (cbs->display                    == NULL) cbs->display                   = (gapi_display_f *)gapi_null_callback;
    if (cbs->cleardisplay               == NULL) cbs->cleardisplay              = (gapi_cleardisplay_f *)gapi_null_callback;
    if (cbs->prompt                     == NULL) cbs->prompt                    = (gapi_prompt_f *)gapi_null_callback;
    if (cbs->clearprompt                == NULL) cbs->clearprompt               = (gapi_clearprompt_f *)gapi_null_callback;
    if (cbs->notify                     == NULL) cbs->notify                    = (gapi_notify_f *)gapi_null_callback;      
    if (cbs->clearnotify                == NULL) cbs->clearnotify               = (gapi_clearnotify_f *)gapi_null_callback;
    if (cbs->activateplane              == NULL) cbs->activateplane             = (gapi_activateplane_f *)gapi_null_callback;
    if (cbs->deactivateplane            == NULL) cbs->deactivateplane           = (gapi_deactivateplane_f *)gapi_null_callback;
    if (cbs->connectionstats            == NULL) cbs->connectionstats           = (gapi_connectionstats_f *)gapi_null_callback;
    if (cbs->backspace_req              == NULL) cbs->backspace_req             = (gapi_backspace_req_f *)gapi_null_callback;
    if (cbs->selectsoftkeys             == NULL) cbs->selectsoftkeys            = (gapi_selectsoftkeys_f *)gapi_null_callback;
    if (cbs->dialednumber               == NULL) cbs->dialednumber              = (gapi_dialednumber_f *)gapi_null_callback;
    if (cbs->usertodevicedata           == NULL) cbs->usertodevicedata          = (gapi_usertodevicedata_f *)gapi_null_callback;
    if (cbs->devicetouserdata_req       == NULL) cbs->devicetouserdata_req      = (gapi_devicetouserdata_req_f *)gapi_null_callback;
    if (cbs->devicetouserdata_res       == NULL) cbs->devicetouserdata_res      = (gapi_devicetouserdata_res_f *)gapi_null_callback;
    if (cbs->prioritynotify             == NULL) cbs->prioritynotify            = (gapi_prioritynotify_f *)gapi_null_callback;
    if (cbs->clearprioritynotify        == NULL) cbs->clearprioritynotify       = (gapi_clearprioritynotify_f *)gapi_null_callback;

    if (cbs->reset                      == NULL) cbs->reset                     = (gapi_reset_f *)gapi_null_callback;
    if (cbs->opensession_req            == NULL) cbs->opensession_req           = (gapi_opensession_req_f *)gapi_null_callback;
    if (cbs->opensession_res            == NULL) cbs->opensession_res           = (gapi_opensession_res_f *)gapi_null_callback;
    if (cbs->sessionstatus              == NULL) cbs->sessionstatus             = (gapi_sessionstatus_f *)gapi_null_callback;  
    if (cbs->resetsession_req           == NULL) cbs->resetsession_req          = (gapi_resetsession_req_f *)gapi_null_callback;
    if (cbs->resetsession_req           == NULL) cbs->resetsession_res          = (gapi_resetsession_res_f *)gapi_null_callback;

    if (cbs->keepalive_req              == NULL) cbs->keepalive_req             = (gapi_keepalive_req_f *)gapi_null_callback;
    if (cbs->keepalive_res              == NULL) cbs->keepalive_res             = (gapi_keepalive_res_f *)gapi_null_callback;
    if (cbs->register_req               == NULL) cbs->register_req              = (gapi_register_req_f *)gapi_null_callback;
    if (cbs->register_res               == NULL) cbs->register_res              = (gapi_register_res_f *)gapi_null_callback;
    if (cbs->alarm                      == NULL) cbs->alarm                     = (gapi_alarm_f *)gapi_null_callback;
    if (cbs->forwardstat_req            == NULL) cbs->forwardstat_req           = (gapi_forwardstat_req_f *)gapi_null_callback;
    if (cbs->forwardstat_res            == NULL) cbs->forwardstat_res           = (gapi_forwardstat_res_f *)gapi_null_callback;
    if (cbs->serviceurlstat_req         == NULL) cbs->serviceurlstat_req        = (gapi_serviceurlstat_req_f *)gapi_null_callback;
    if (cbs->serviceurlstat_res         == NULL) cbs->serviceurlstat_res        = (gapi_serviceurlstat_res_f *)gapi_null_callback;
    if (cbs->featurestat_req            == NULL) cbs->featurestat_req           = (gapi_featurestat_req_f *)gapi_null_callback;
    if (cbs->featurestat_res            == NULL) cbs->featurestat_res           = (gapi_featurestat_res_f *)gapi_null_callback;
    if (cbs->speeddialstat_req          == NULL) cbs->speeddialstat_req         = (gapi_speeddialstat_req_f *)gapi_null_callback;
    if (cbs->speeddialstat_res          == NULL) cbs->speeddialstat_res         = (gapi_speeddialstat_res_f *)gapi_null_callback;
    if (cbs->linestat_req               == NULL) cbs->linestat_req              = (gapi_linestat_req_f *)gapi_null_callback;
    if (cbs->linestat_res               == NULL) cbs->linestat_res              = (gapi_linestat_res_f *)gapi_null_callback;
    if (cbs->configstat_req             == NULL) cbs->configstat_req            = (gapi_configstat_req_f *)gapi_null_callback;
    if (cbs->configstat_res             == NULL) cbs->configstat_res            = (gapi_configstat_res_f *)gapi_null_callback;
    if (cbs->timedate_req               == NULL) cbs->timedate_req              = (gapi_timedate_req_f *)gapi_null_callback;
    if (cbs->timedate_res               == NULL) cbs->timedate_res              = (gapi_timedate_res_f *)gapi_null_callback;
    if (cbs->buttontemplate_req         == NULL) cbs->buttontemplate_req        = (gapi_buttontemplate_req_f *)gapi_null_callback;
    if (cbs->buttontemplate_res         == NULL) cbs->buttontemplate_res        = (gapi_buttontemplate_res_f *)gapi_null_callback;
    if (cbs->version_req                == NULL) cbs->version_req               = (gapi_version_req_f *)gapi_null_callback;
    if (cbs->version_res                == NULL) cbs->version_res               = (gapi_version_res_f *)gapi_null_callback;
    if (cbs->capabilities_req           == NULL) cbs->capabilities_req          = (gapi_capabilities_req_f *)gapi_null_callback;
    if (cbs->capabilities_res           == NULL) cbs->capabilities_res          = (gapi_capabilities_res_f *)gapi_null_callback;
    if (cbs->softkeytemplate_req        == NULL) cbs->softkeytemplate_req       = (gapi_softkeytemplate_req_f *)gapi_null_callback;
    if (cbs->softkeytemplate_res        == NULL) cbs->softkeytemplate_res       = (gapi_softkeytemplate_res_f *)gapi_null_callback;
    if (cbs->softkeyset_req             == NULL) cbs->softkeyset_req            = (gapi_softkeyset_req_f *)gapi_null_callback;    
    if (cbs->softkeyset_res             == NULL) cbs->softkeyset_res            = (gapi_softkeyset_res_f *)gapi_null_callback;        

    if (cbs->pasthru                    == NULL) cbs->passthru                  = (gapi_passthru_f *)gapi_null_callback;        

    if (cbs->all_streams_idle           == NULL) cbs->all_streams_idle          = (gapi_all_streams_idle_f *)gapi_null_callback;
    if (cbs->close_abandonded_streams   == NULL) cbs->close_abandonded_streams  = (gapi_close_abandonded_streams_f *)gapi_null_callback;

    if (cbs->tcp_main                   == NULL) cbs->tcp_main                  = (gapi_tcp_main_f *)gapi_null_callback;                
    if (cbs->sccp_main                  == NULL) cbs->sccp_main                 = (gapi_sccp_main_f *)gapi_null_callback;
    if (cbs->sccp_cleanup               == NULL) cbs->sccp_cleanup              = (gapi_sccp_cleanup_f *)gapi_null_callback;               
#endif
}
