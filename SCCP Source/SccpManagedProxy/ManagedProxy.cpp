#include <stdlib.h>
#include <malloc.h>
#include <Winsock2.h>

#include "ManagedProxy.h"
#include "SccpCallbackProxy.h"
#include "timer.h"

using namespace System::Runtime::InteropServices;
using namespace Metreos::SCCP;
 
ManagedProxy::ManagedProxy()
{ 
	SccpCallbackProxy::m_MProxy = this;
}

bool ManagedProxy::Initialize(System::String* deviceId)
{
	m_DeviceId = deviceId;

	timer_event_system_init();
	if(sapp_init() != 0) { return false; }

	return InitializeWindowsSockets();

	//return true;
}

bool ManagedProxy::InitializeWindowsSockets()
{
	WSADATA             sock_data;
    unsigned short      lVersion;

    // Initialize windows socket system
    lVersion = MAKEWORD(2,2);
    WSAStartup(lVersion, &sock_data);
    if (LOBYTE(sock_data.wVersion) != 2 ||
        HIBYTE(sock_data.wVersion) != 2 ) {
        WSACleanup();
        return false; 
    }
	return true;
}

void ManagedProxy::Shutdown()
{
	sapp_cleanup();
}

//
// Actions from the app to the stack
//
int ManagedProxy::SendSetup(int callId, int line, ConnInfo* connInfo, System::String* digitStr, MediaInfo* mediaInfo)
{
	gapi_conninfo_t* insaneConnInfo = ConvertConnInfo(connInfo);
	gapi_media_t* insaneMediaInfo = ConvertMediaInfo(mediaInfo);

	int numDigits = digitStr->Length;

	System::IntPtr digitPtr = Marshal::StringToHGlobalAnsi(digitStr);
	char* digits = static_cast<char*>(digitPtr.ToPointer());

	return sapp_setup(callId, line, insaneConnInfo, digits, numDigits,
			       insaneMediaInfo, GAPI_ALERT_INFO_MIN, GAPI_PRIVACY_MIN, 0);
}

int ManagedProxy::SendSetupAck(int callId, int line, MediaInfo* mediaInfo)
{
	gapi_media_t* insaneMediaInfo = ConvertMediaInfo(mediaInfo);

	return sapp_setup_ack(callId, line, insaneMediaInfo);
}

int ManagedProxy::SendProceeding(int callId, int line)
{
	return sapp_proceeding(callId, line);
}

int ManagedProxy::SendAlerting(int callId, int line)
{
	return sapp_alerting(callId, line);
}

int ManagedProxy::SendConnect(int connId, int line, MediaInfo* mediaInfo)
{
	gapi_media_t* insaneMediaInfo = ConvertMediaInfo(mediaInfo);

	return sapp_connect(connId, line, insaneMediaInfo);
}

int ManagedProxy::SendConnectAck(int connId, int line)
{
	return sapp_connect_ack(connId, line);
}

int ManagedProxy::SendRelease(int connId, int line)
{
	return sapp_release(connId, line, GAPI_CAUSE_OK);
}

int ManagedProxy::SendReleaseComplete(int connId, int line)
{
	return sapp_release_complete(connId, line, GAPI_CAUSE_OK);
}

int ManagedProxy::SendDigits(int callId, int line, System::String* digitStr)
{
	int numDigits = digitStr->Length;

	System::IntPtr digitPtr = Marshal::StringToHGlobalAnsi(digitStr);
	char* digits = static_cast<char*>(digitPtr.ToPointer());

	return sapp_digits(callId, line, digits, numDigits);
}
 
int ManagedProxy::SendFeatureRequest(int callId, int line, int feature)
{
	return sapp_feature_req(callId, line, feature, NULL, GAPI_CAUSE_OK);
}

int ManagedProxy::SendOpenSessionRequest(System::String* cmAddress, unsigned short cmPort)
{
	System::IntPtr macPtr = Marshal::StringToHGlobalAnsi(m_DeviceId);
	char* mac_str = static_cast<char*>(macPtr.ToPointer());

	System::IntPtr cmAddressPtr = Marshal::StringToHGlobalAnsi(cmAddress);
	char* cm_addr_str = static_cast<char*>(cmAddressPtr.ToPointer());
	unsigned long cm_addr = static_cast<unsigned long>(inet_addr(cm_addr_str));
	
	unsigned short nuttyPort = htons(cmPort);

	return sapp_opensession_wrapper(mac_str, cm_addr, nuttyPort);
}

int ManagedProxy::SendResetSessionRequest()
{
	return sapp_resetsession_req(GAPI_CAUSE_OK);
}

int ManagedProxy::SendReceiveResponse(int callId, int line, MediaInfo* mediaInfo)
{
	gapi_media_t* insaneMediaInfo = ConvertMediaInfo(mediaInfo);

	return sapp_sapp_openrcv_res(callId, line, insaneMediaInfo);
}

System::String* ManagedProxy::GetExtension(int line)
{
	char* dn = sapp_get_line_name(line);
	if(dn == NULL) { return NULL; }

	return new System::String(dn);
}


//
// Callbacks from the stack to the app
//
void ManagedProxy::Setup(int call_id, int line, void *conninfo, int alert_info, 
					     int alerting_ring, int alerting_tone, void *media, int replaces)
{
	if(setupDelegate != NULL)
	{
		// The media info is bogus, drop it
		//   but the connection info is good
		ConnInfo* saneConnInfo = ConvertConnInfo(conninfo);
		setupDelegate(m_DeviceId, call_id, line, saneConnInfo);
	}
}

void ManagedProxy::Offhook(int call_id, int line)
{
	if(offhookDelegate != NULL)
	{
		offhookDelegate(m_DeviceId, line);
	}
}

void ManagedProxy::SetupAck(int call_id, int line, int cause, void *conninfo, void *media)
{
	if(setupAckDelegate != NULL)
	{
		ConnInfo* saneConnInfo = ConvertConnInfo(conninfo);
		MediaInfo* saneMedia = ConvertMediaInfo(media);
		setupAckDelegate(m_DeviceId, call_id, line, cause, saneConnInfo, saneMedia);
	}
}

void ManagedProxy::Proceeding(int call_id, int line, void *conninfo)
{
	if(proceedingDelegate != NULL)
	{
		ConnInfo* saneConnInfo = ConvertConnInfo(conninfo);
		proceedingDelegate(m_DeviceId, line, saneConnInfo);
	}
}

void ManagedProxy::Alerting(int call_id, int line, void *conninfo, void *media, int inband)
{
	if(alertingDelegate != NULL)
	{
		ConnInfo* saneConnInfo = ConvertConnInfo(conninfo);
		MediaInfo* saneMedia = ConvertMediaInfo(media);
		alertingDelegate(m_DeviceId, line, saneConnInfo, saneMedia, inband);
	}
}

void ManagedProxy::Connect(int call_id, int line, void *conninfo, void *media)
{
	if(connectDelegate != NULL)
	{
		ConnInfo* saneConnInfo = ConvertConnInfo(conninfo);
		MediaInfo* saneMedia = ConvertMediaInfo(media);
		connectDelegate(m_DeviceId, line, saneConnInfo, saneMedia);
	}
}

void ManagedProxy::ConnectAck(int call_id, int line, void *conninfo, void *media)
{
	if(connectAckDelegate != NULL)
	{
		ConnInfo* saneConnInfo = ConvertConnInfo(conninfo);
		MediaInfo* saneMedia = ConvertMediaInfo(media);
		connectAckDelegate(m_DeviceId, line, saneConnInfo, saneMedia);
	}
}

void ManagedProxy::Release(int call_id, int line)
{
	if(releaseDelegate != NULL)
	{
		releaseDelegate(m_DeviceId, line);
	}
}

void ManagedProxy::ReleaseComplete(int call_id, int line)
{
	if(releaseCompleteDelegate != NULL)
	{
		releaseCompleteDelegate(m_DeviceId, line);
	}
}

void ManagedProxy::FeatureRequest(int call_id, int line, int feature)
{
	if(featureRequestDelegate != NULL)
	{
		featureRequestDelegate(m_DeviceId, line, feature);
	}
}

void ManagedProxy::OpenReceiveRequest(int call_id, int line, gapi_media_t *media)
{
	if(openReceiveRequestDelegate != NULL)
	{
		MediaInfo* saneMedia = ConvertMediaInfo(media);
		openReceiveRequestDelegate(m_DeviceId, line, saneMedia);
	}
}

void ManagedProxy::StartTransmit(int call_id, int line, gapi_media_t *media)
{
	if(startTransmitDelegate != NULL)
	{
		MediaInfo* saneMedia = ConvertMediaInfo(media);
		startTransmitDelegate(m_DeviceId, line, saneMedia);
	}
}


//
// Private helper methods
//
ConnInfo* ManagedProxy::ConvertConnInfo(void* conninfo)
{
	if(conninfo == NULL) { return NULL; }

	ConnInfo* saneConnInfo = new ConnInfo();
	gapi_conninfo_t* gapi_conninfo = static_cast<gapi_conninfo_t*>(conninfo);

	saneConnInfo->calling_name = new System::String(gapi_conninfo->calling_name);
	saneConnInfo->calling_number = new System::String(gapi_conninfo->calling_number);
	saneConnInfo->called_name = new System::String(gapi_conninfo->called_name);
	saneConnInfo->called_number = new System::String(gapi_conninfo->called_number);
	saneConnInfo->original_name = new System::String(gapi_conninfo->original_name);
	saneConnInfo->original_number = new System::String(gapi_conninfo->original_number);
	saneConnInfo->redirected_name = new System::String(gapi_conninfo->redirected_name);
	saneConnInfo->redirected_number = new System::String(gapi_conninfo->redirected_number);

	return saneConnInfo;
}

gapi_conninfo_t* ManagedProxy::ConvertConnInfo(ConnInfo* connInfo)
{
	if(connInfo == NULL) { return NULL; }
	return NULL;
}

MediaInfo* ManagedProxy::ConvertMediaInfo(void* media)
{
	if(media == NULL) { return NULL; }

	MediaInfo* saneMediaInfo = new MediaInfo();
	gapi_sccp_media_t* gapi_media = static_cast<gapi_sccp_media_t*>(media);

	in_addr inAddr;
	inAddr.S_un.S_addr = gapi_media->addr;

	saneMediaInfo->addr = inet_ntoa(inAddr);
	saneMediaInfo->port = ntohl(gapi_media->port);
    saneMediaInfo->conference_id = gapi_media->conference_id;
    saneMediaInfo->passthruparty_id = gapi_media->passthruparty_id;
    saneMediaInfo->packet_size = gapi_media->packet_size;
    saneMediaInfo->echo_cancelation = gapi_media->echo_cancelation;
    saneMediaInfo->g723_bitrate = gapi_media->g723_bitrate; 
    saneMediaInfo->precedence = gapi_media->precedence;
    saneMediaInfo->silence_suppression = gapi_media->silence_suppression;
    saneMediaInfo->frames_per_packet = gapi_media->frames_per_packet;

	saneMediaInfo->payload_type = LookupManagedPayloadType(gapi_media->payload_type);

	return saneMediaInfo;
}

gapi_media_t* ManagedProxy::ConvertMediaInfo(MediaInfo* mediaInfo)
{
	if(mediaInfo == NULL) { return NULL; }

	IntPtr addrPtr = Marshal::StringToHGlobalAnsi(mediaInfo->addr);
	char* addrStr = static_cast<char*>(addrPtr.ToPointer());

	gapi_sccp_media_t insaneMediaInfo;
	insaneMediaInfo.addr = inet_addr(addrStr);
	insaneMediaInfo.port = htonl(mediaInfo->port);
    insaneMediaInfo.conference_id = mediaInfo->conference_id;
    insaneMediaInfo.passthruparty_id = mediaInfo->passthruparty_id;
    insaneMediaInfo.packet_size = mediaInfo->packet_size;
    insaneMediaInfo.echo_cancelation = mediaInfo->echo_cancelation;
    insaneMediaInfo.g723_bitrate = mediaInfo->g723_bitrate; 
    insaneMediaInfo.precedence = mediaInfo->precedence;
    insaneMediaInfo.silence_suppression = mediaInfo->silence_suppression;
    insaneMediaInfo.frames_per_packet = mediaInfo->frames_per_packet;

	insaneMediaInfo.payload_type = (gapi_payload_types_e)LookupManagedPayloadType(mediaInfo->payload_type);

	gapi_media_t* insaneMediaInfoParent = static_cast<gapi_media_t*>(malloc(sizeof(gapi_media_t)));
	insaneMediaInfoParent->sccp_media = insaneMediaInfo;

	return insaneMediaInfoParent;
}

//gapi_media_caps_t* GenerateMediaCaps()
//{
//	gapi_media_caps_t* media_caps = malloc(sizeof(gapi_media_caps_t));
//	media_caps->count = 1;
//    media_caps->caps[0].payload = GAPI_PAYLOAD_TYPE_G711_ULAW_64K;
//    media_caps->caps[0].milliseconds_per_packet = 20;
//
//	// At first I started writing a converter, then said 'fuck it'
//	/*
//	media_caps->count = mediaCaps->count;
//	
//	for(int i=0; i<mediaCaps->count; i++)
//	{
//		media_caps->caps[i]
//	}*/
//}

//gapi_cmaddr_t* ManagedProxy::ConvertAddress(Address* cmAddress)
//{
//	gapi_cmaddr_t* cms = malloc(sizeof(gapi_cmaddr_t));
//	cms->addr = cmAddress->addr;
//	cms->port = cmAddress->port;
//
//	return cms;
//}

//gapi_opensession_values_t* ManagedProxy::GenerateOpenSessionValues()
//{
//	gapi_opensession_values_t* values = malloc(sizeof(gapi_opensession_values_t));
//	values->device_poll_to = 180000;
//	values->device_type = GAPI_DEVICE_STATION_TELECASTER_MGR;
//
//	return values;
//}

// Horrible monkey-code
int ManagedProxy::LookupManagedPayloadType(PayloadTypes payloadType)
{
	switch(payloadType)
	{
	case PayloadTypes::GAPI_PAYLOAD_TYPE_NON_STANDARD:
		return 1;
	case PayloadTypes::GAPI_PAYLOAD_TYPE_G711_ALAW_64K:
		return 2;
	case PayloadTypes::GAPI_PAYLOAD_TYPE_G711_ALAW_56K:
		return 3;
	case PayloadTypes::GAPI_PAYLOAD_TYPE_G711_ULAW_64K:
		return 4;
	case PayloadTypes::GAPI_PAYLOAD_TYPE_G711_ULAW_56K:
		return 5;
	case PayloadTypes::GAPI_PAYLOAD_TYPE_G722_64K:
		return 6;
	case PayloadTypes::GAPI_PAYLOAD_TYPE_G722_56K:
		return 7;
	case PayloadTypes::GAPI_PAYLOAD_TYPE_G722_48K:
		return 8;
	case PayloadTypes::GAPI_PAYLOAD_TYPE_G7231:
		return 9;
	case PayloadTypes::GAPI_PAYLOAD_TYPE_G728:
		return 10;
	case PayloadTypes::GAPI_PAYLOAD_TYPE_G729:
		return 11;
	case PayloadTypes::GAPI_PAYLOAD_TYPE_G729_ANNEX_A:
		return 12;
	case PayloadTypes::GAPI_PAYLOAD_TYPE_IS11172_AUDIO_CAP:
		return 13;
	case PayloadTypes::GAPI_PAYLOAD_TYPE_IS13818_AUDIO_CAP:
		return 14;
	case PayloadTypes::GAPI_PAYLOAD_TYPE_G729_ANNEX_B:
		return 15;
	case PayloadTypes::GAPI_PAYLOAD_TYPE_G729_ANNEX_AW_ANNEX_B:
		return 17;
	case PayloadTypes::GAPI_PAYLOAD_TYPE_GSM_FULL_RATE:
		return 18;
	case PayloadTypes::GAPI_PAYLOAD_TYPE_GSM_HALF_RATE:
		return 19;
	case PayloadTypes::GAPI_PAYLOAD_TYPE_GSM_ENHANCED_FULL_RATE:
		return 20;
	case PayloadTypes::GAPI_PAYLOAD_TYPE_WIDE_BAND_256K:
		return 25;
	case PayloadTypes::GAPI_PAYLOAD_TYPE_DATA_64:
		return 32;
	case PayloadTypes::GAPI_PAYLOAD_TYPE_DATA_56:
		return 33;
	case PayloadTypes::GAPI_PAYLOAD_TYPE_GSM:
		return 80;
	case PayloadTypes::GAPI_PAYLOAD_TYPE_ACTIVE_VOICE:
		return 81;
	case PayloadTypes::GAPI_PAYLOAD_TYPE_G726_32K:
		return 82;
	case PayloadTypes::GAPI_PAYLOAD_TYPE_G726_24K:
		return 83;
	case PayloadTypes::GAPI_PAYLOAD_TYPE_G726_16K:
		return 84;
	case PayloadTypes::GAPI_PAYLOAD_TYPE_G729_B:
		return 85;
	case PayloadTypes::GAPI_PAYLOAD_TYPE_G729_B_LOW_COMPLEXITY:
		return 86;
	case PayloadTypes::GAPI_PAYLOAD_H261:
		return 100;
	case PayloadTypes::GAPI_PAYLOAD_H263:
		return 101;
	case PayloadTypes::GAPI_PAYLOAD_T120:
		return 105;
	case PayloadTypes::GAPI_PAYLOAD_H224:
		return 106;
	case PayloadTypes::GAPI_PAYLOAD_XV150_MR:
		return 111;
	case PayloadTypes::GAPI_PAYLOAD_RFC2833_DYN_PAYLOAD:
		return 257;
	default:
		return 0;
	}
}


// Horrible monkey-code x2
PayloadTypes ManagedProxy::LookupManagedPayloadType(int payloadType)
{
	switch(payloadType)
	{
	case 1:
		return PayloadTypes::GAPI_PAYLOAD_TYPE_NON_STANDARD;
	case 2:
		return PayloadTypes::GAPI_PAYLOAD_TYPE_G711_ALAW_64K;
	case 3:
		return PayloadTypes::GAPI_PAYLOAD_TYPE_G711_ALAW_56K;
	case 4:
		return PayloadTypes::GAPI_PAYLOAD_TYPE_G711_ULAW_64K;
	case 5:
		return PayloadTypes::GAPI_PAYLOAD_TYPE_G711_ULAW_56K;
	case 6:
		return PayloadTypes::GAPI_PAYLOAD_TYPE_G722_64K;
	case 7:
		return PayloadTypes::GAPI_PAYLOAD_TYPE_G722_56K;
	case 8:
		return PayloadTypes::GAPI_PAYLOAD_TYPE_G722_48K;
	case 9:
		return PayloadTypes::GAPI_PAYLOAD_TYPE_G7231;
	case 10:
		return PayloadTypes::GAPI_PAYLOAD_TYPE_G728;
	case 11:
		return PayloadTypes::GAPI_PAYLOAD_TYPE_G729;
	case 12:
		return PayloadTypes::GAPI_PAYLOAD_TYPE_G729_ANNEX_A;
	case 13:
		return PayloadTypes::GAPI_PAYLOAD_TYPE_IS11172_AUDIO_CAP;
	case 14:
		return PayloadTypes::GAPI_PAYLOAD_TYPE_IS13818_AUDIO_CAP;
	case 15:
		return PayloadTypes::GAPI_PAYLOAD_TYPE_G729_ANNEX_B;
	case 16:
		return PayloadTypes::GAPI_PAYLOAD_TYPE_G729_ANNEX_AW_ANNEX_B;
	case 18:
		return PayloadTypes::GAPI_PAYLOAD_TYPE_GSM_FULL_RATE;
	case 19:
		return PayloadTypes::GAPI_PAYLOAD_TYPE_GSM_HALF_RATE;
	case 20:
		return PayloadTypes::GAPI_PAYLOAD_TYPE_GSM_ENHANCED_FULL_RATE;
	case 25:
		return PayloadTypes::GAPI_PAYLOAD_TYPE_WIDE_BAND_256K;
	case 32:
		return PayloadTypes::GAPI_PAYLOAD_TYPE_DATA_64;
	case 33:
		return PayloadTypes::GAPI_PAYLOAD_TYPE_DATA_56;
	case 80:
		return PayloadTypes::GAPI_PAYLOAD_TYPE_GSM;
	case 81:
		return PayloadTypes::GAPI_PAYLOAD_TYPE_ACTIVE_VOICE;
	case 82:
		return PayloadTypes::GAPI_PAYLOAD_TYPE_G726_32K;
    case 83:
		return PayloadTypes::GAPI_PAYLOAD_TYPE_G726_24K;
	case 84:
		return PayloadTypes::GAPI_PAYLOAD_TYPE_G726_16K;
	case 85:
		return PayloadTypes::GAPI_PAYLOAD_TYPE_G729_B;
	case 86:
		return PayloadTypes::GAPI_PAYLOAD_TYPE_G729_B_LOW_COMPLEXITY;
	case 100:
		return PayloadTypes::GAPI_PAYLOAD_H261;
	case 101:
		return PayloadTypes::GAPI_PAYLOAD_H263;
	case 105:
		return PayloadTypes::GAPI_PAYLOAD_T120;
	case 106:
		return PayloadTypes::GAPI_PAYLOAD_H224;
	case 111:
		return PayloadTypes::GAPI_PAYLOAD_XV150_MR;
	case 257:
		return PayloadTypes::GAPI_PAYLOAD_RFC2833_DYN_PAYLOAD;
	default:
		return PayloadTypes::GAPI_PAYLOAD_TYPE_INVALID;
	}
}
