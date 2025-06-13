#ifndef __MANAGED_PROXY_H__
#define __MANAGED_PROXY_H__

#pragma once
#pragma managed

#include <vcclr.h>

// Forward declare unmanaged callback proxy.
__nogc class SccpCallbackProxy;

// Forward declare stack structs
__nogc struct gapi_conninfo_t_;
typedef gapi_conninfo_t_ gapi_conninfo_t;

__nogc struct gapi_media_t_;
typedef gapi_media_t_ gapi_media_t;

namespace Metreos
{
namespace SCCP
{

[System::FlagsAttribute]
public __value enum PayloadTypes
{
    GAPI_PAYLOAD_TYPE_NON_STANDARD           = 1,
    GAPI_PAYLOAD_TYPE_G711_ALAW_64K          = 2,
    GAPI_PAYLOAD_TYPE_G711_ALAW_56K          = 3,      // "RESTRICTED"
    GAPI_PAYLOAD_TYPE_G711_ULAW_64K          = 4,
    GAPI_PAYLOAD_TYPE_G711_ULAW_56K          = 5,      // "RESTRICTED"
    GAPI_PAYLOAD_TYPE_G722_64K               = 6,
    GAPI_PAYLOAD_TYPE_G722_56K               = 7,
    GAPI_PAYLOAD_TYPE_G722_48K               = 8,
    GAPI_PAYLOAD_TYPE_G7231                  = 9,
    GAPI_PAYLOAD_TYPE_G728                   = 10,
    GAPI_PAYLOAD_TYPE_G729                   = 11,
    GAPI_PAYLOAD_TYPE_G729_ANNEX_A           = 12,
    GAPI_PAYLOAD_TYPE_IS11172_AUDIO_CAP      = 13,
    GAPI_PAYLOAD_TYPE_IS13818_AUDIO_CAP      = 14,
    GAPI_PAYLOAD_TYPE_G729_ANNEX_B           = 15,
    GAPI_PAYLOAD_TYPE_G729_ANNEX_AW_ANNEX_B  = 16,
    GAPI_PAYLOAD_TYPE_GSM_FULL_RATE          = 18,
    GAPI_PAYLOAD_TYPE_GSM_HALF_RATE          = 19,
    GAPI_PAYLOAD_TYPE_GSM_ENHANCED_FULL_RATE = 20,
    GAPI_PAYLOAD_TYPE_WIDE_BAND_256K         = 25,
    GAPI_PAYLOAD_TYPE_DATA_64                = 32,
    GAPI_PAYLOAD_TYPE_DATA_56                = 33,
    GAPI_PAYLOAD_TYPE_GSM                    = 80,
    GAPI_PAYLOAD_TYPE_ACTIVE_VOICE           = 81,
    GAPI_PAYLOAD_TYPE_G726_32K               = 82,
    GAPI_PAYLOAD_TYPE_G726_24K               = 83,
    GAPI_PAYLOAD_TYPE_G726_16K               = 84,
    GAPI_PAYLOAD_TYPE_G729_B                 = 85,
    GAPI_PAYLOAD_TYPE_G729_B_LOW_COMPLEXITY  = 86,
    GAPI_PAYLOAD_H261                        = 100,
    GAPI_PAYLOAD_H263                        = 101,
    GAPI_PAYLOAD_T120                        = 105,
    GAPI_PAYLOAD_H224                        = 106,
    GAPI_PAYLOAD_XV150_MR                    = 111,
    GAPI_PAYLOAD_RFC2833_DYN_PAYLOAD         = 257,
	GAPI_PAYLOAD_TYPE_INVALID
};

public __gc struct ConnInfo
{
	System::String* calling_name;
    System::String* calling_number;
    System::String* called_name;
    System::String* called_number;
    System::String* original_name;
    System::String* original_number;
    System::String* redirected_name;
    System::String* redirected_number;
};

public __gc struct MediaInfo
{
	System::String* addr;
    unsigned long port;
    unsigned int conference_id;
    unsigned int passthruparty_id;
    unsigned int packet_size;
    PayloadTypes payload_type;
    int echo_cancelation;
    int g723_bitrate; 
    int precedence;
    int silence_suppression;
    int frames_per_packet;
};

//public __gc struct MediaCapabilities 
//{
//    int count;
//    MediaCapability caps[MAX_CAPABILITIES];
//};
//
//public __gc struct MediaCapability
//{
//    PayloadTypes payload;
//    int milliseconds_per_packet;
//};
//
//public __gc struct Address 
//{
//    unsigned long addr;
//    unsigned short port;
//};



public __delegate void SetupDelegate(System::String* deviceId, int call_id, int line, ConnInfo* conninfo);
public __delegate void OffhookDelegate(System::String* deviceId, int line);
public __delegate void SetupAckDelegate(System::String* deviceId, int call_id, int line, int cause, ConnInfo* conninfo, MediaInfo* media);
public __delegate void ProceedingDelegate(System::String* deviceId, int line, ConnInfo* conninfo);
public __delegate void AlertingDelegate(System::String* deviceId, int line, ConnInfo* conninfo, MediaInfo* media, int inband);
public __delegate void ConnectDelegate(System::String* deviceId, int line, ConnInfo* conninfo, MediaInfo* media);
public __delegate void ConnectAckDelegate(System::String* deviceId, int line, ConnInfo* conninfo, MediaInfo* media);
public __delegate void ReleaseDelegate(System::String* deviceId, int line);
public __delegate void ReleaseCompleteDelegate(System::String* deviceId, int line);
public __delegate void FeatureRequestDelegate(System::String* deviceId, int line, int feature);
public __delegate void OpenReceiveRequestDelegate(System::String* deviceId, int line, MediaInfo* saneMedia);
public __delegate void StartTransmitDelegate(System::String* deviceId, int line, MediaInfo* saneMedia);

public __gc class ManagedProxy 
{
public:
    ManagedProxy();
	bool Initialize(System::String* deviceId);
	void Shutdown();

	// Callbacks from the stack
	void Setup(int call_id, int line, void *conninfo, int alert_info, 
			   int alerting_ring, int alerting_tone, void *media, int replaces);
    void Offhook(int call_id, int line);
    void SetupAck(int call_id, int line, int cause, void *conninfo, void *media);
    void Proceeding(int call_id, int line, void *conninfo);
    void Alerting(int call_id, int line, void *conninfo, void *media, int inband);
    void Connect(int call_id, int line, void *conninfo, void *media);
    void ConnectAck(int call_id, int line, void *conninfo, void *media);
    void Release(int call_id, int line);
    void ReleaseComplete(int call_id, int line);
    void FeatureRequest(int call_id, int line, int feature);
	void OpenReceiveRequest(int call_id, int line, gapi_media_t *media);
	void StartTransmit(int call_id, int line, gapi_media_t *media);

	// Calls into the stack
	int SendSetup(int callId, int line, ConnInfo* connInfo, System::String* digits, MediaInfo* mediaInfo);
	int SendSetupAck(int callId, int line, MediaInfo* mediaInfo);
	int SendProceeding(int call_id, int line);
	int SendAlerting(int callId, int line);
	int SendConnect(int connId, int line, MediaInfo* mediaInfo);
	int SendConnectAck(int connId, int line);
	int SendRelease(int connId, int line);
	int SendReleaseComplete(int connId, int line);
	int SendDigits(int callId, int line, System::String* digits);
	int SendFeatureRequest(int callId, int line, int feature);
	int SendOpenSessionRequest(System::String* cmAddress, unsigned short cmPort);
	System::String* GetExtension(int line);
	int SendResetSessionRequest();
	int SendReceiveResponse(int callId, int line, MediaInfo* mediaInfo);

public:
	SetupDelegate* setupDelegate;
	OffhookDelegate* offhookDelegate;
	SetupAckDelegate* setupAckDelegate;
	ProceedingDelegate* proceedingDelegate;
	AlertingDelegate* alertingDelegate;
	ConnectDelegate* connectDelegate;
	ConnectAckDelegate* connectAckDelegate;
	ReleaseDelegate* releaseDelegate;
	ReleaseCompleteDelegate* releaseCompleteDelegate;
	FeatureRequestDelegate* featureRequestDelegate;
	OpenReceiveRequestDelegate* openReceiveRequestDelegate;
	StartTransmitDelegate* startTransmitDelegate;

protected:
	SccpCallbackProxy __nogc* m_Proxy;
	System::String* m_DeviceId;

private:
	bool InitializeWindowsSockets();
	ConnInfo* ConvertConnInfo(void* conninfo);
	gapi_conninfo_t* ConvertConnInfo(ConnInfo* connInfo);
	MediaInfo* ConvertMediaInfo(void* media);
	gapi_media_t* ConvertMediaInfo(MediaInfo* mediaInfo);
	PayloadTypes LookupManagedPayloadType(int payloadType);
	int LookupManagedPayloadType(PayloadTypes payloadType);
};

} // namespace SCCP
} // namespace Metreos

#endif // __MANAGED_PROXY_H__