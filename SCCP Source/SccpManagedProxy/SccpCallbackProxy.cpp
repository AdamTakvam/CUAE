#include "SccpCallbackProxy.h"
#include "ManagedProxy.h"

using namespace Metreos::SCCP;

gcroot<Metreos::SCCP::ManagedProxy*> SccpCallbackProxy::m_MProxy = 0;


//
// Signals coming up from the stack
//
void SccpCallbackProxy::sapp_app_setup(int call_id, int line, void *conninfo,
                                       int alert_info, int alerting_ring,
                                       int alerting_tone, void *media,
                                       void *redirect, int replaces)
{
	m_MProxy->Setup(call_id, line, conninfo, alert_info, alerting_ring, alerting_tone, media, replaces);
}

void SccpCallbackProxy::sapp_app_offhook(int call_id, int line)
{
	m_MProxy->Offhook(call_id, line);
}

void SccpCallbackProxy::sapp_app_setup_ack(int call_id, int line, int cause, 
                                            void *conninfo, void *media)
{
	m_MProxy->SetupAck(call_id, line, cause, conninfo, media);
}

void SccpCallbackProxy::sapp_app_proceeding(int call_id, int line, void *conninfo)
{
	m_MProxy->Proceeding(call_id, line, conninfo);
}

void SccpCallbackProxy::sapp_app_alerting(int call_id, int line, void *conninfo, void *media, int inband)
{
	m_MProxy->Alerting(call_id, line, conninfo, media, inband);
}

void SccpCallbackProxy::sapp_app_connect(int call_id, int line, void *conninfo, void *media)
{
	m_MProxy->Connect(call_id, line, conninfo, media);
}

void SccpCallbackProxy::sapp_app_connect_ack(int call_id, int line, void *conninfo, void *media)
{
	m_MProxy->ConnectAck(call_id, line, conninfo, media);
}

void SccpCallbackProxy::sapp_app_release(int call_id, int line, int cause)
{
	m_MProxy->Release(call_id, line);
}

void SccpCallbackProxy::sapp_app_release_complete(int call_id, int line, int cause)
{
	m_MProxy->ReleaseComplete(call_id, line);
}

void SccpCallbackProxy::sapp_app_feature_req(int call_id, int line, int feature)
{
	m_MProxy->FeatureRequest(call_id, line, feature);
}

void SccpCallbackProxy::sapp_sapp_openrcv_req(int call_id, int line, gapi_media_t *media)
{
	m_MProxy->OpenReceiveRequest(call_id, line, media);
}

void SccpCallbackProxy::sapp_sapp_startxmit(int call_id, int line, gapi_media_t *media)
{
	m_MProxy->StartTransmit(call_id, line, media);
}