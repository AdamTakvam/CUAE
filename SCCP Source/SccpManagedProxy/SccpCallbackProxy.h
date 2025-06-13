#ifndef __SCCP_CALLBACK_PROXY_H__
#define __SCCP_CALLBACK_PROXY_H__

#pragma once

#include "sapp.h"
#include <vcclr.h>


using namespace System;

namespace Metreos
{
namespace SCCP
{
	public __gc class ManagedProxy;
} // namespace SCCP
} // namespace Metreos


// Unmanaged proxy
__nogc class SccpCallbackProxy
{
public:
    static void sapp_app_setup(int call_id, int line, void *conninfo,
                            int alert_info, int alerting_ring,
                            int alerting_tone, void *media,
                            void *redirect, int replaces);
    static void sapp_app_offhook (int call_id, int line);
    static void sapp_app_setup_ack (int call_id, int line, int cause, void *conninfo, void *media);
    static void sapp_app_proceeding (int call_id, int line, void *conninfo);
    static void sapp_app_alerting (int call_id, int line, void *conninfo, void *media, int inband);
    static void sapp_app_connect (int call_id, int line, void *conninfo, void *media);
    static void sapp_app_connect_ack (int call_id, int line, void *conninfo, void *media);
    static void sapp_app_release (int call_id, int line, int cause);
    static void sapp_app_release_complete (int call_id, int line, int cause);
    static void sapp_app_feature_req (int call_id, int line, int feature);
	static void sapp_sapp_openrcv_req (int call_id, int line, gapi_media_t *media);
	static void sapp_sapp_startxmit (int call_id, int line, gapi_media_t *media);

public:
	static gcroot<Metreos::SCCP::ManagedProxy*> m_MProxy;
};

#endif // __SCCP_CALLBACK_PROXY_H__