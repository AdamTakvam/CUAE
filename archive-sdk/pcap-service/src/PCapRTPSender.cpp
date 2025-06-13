// PCapRTPSender.cpp

#include "stdafx.h"

#ifdef WIN32
#ifdef PCAP_MEM_LEAK_DETECTION
#   define DEBUG_NEW new(_NORMAL_BLOCK, __FILE__, __LINE__)
#   define new DEBUG_NEW 
#endif
#endif

#include "PCapCommon.h"
#include "PCapRTPSender.h"

// includes for JRTPLib
#include "RTPSessionParams.h"
#include "RTPUDPv4Transmitter.h"

using namespace Metreos;
using namespace Metreos::PCap;


PCapRTPSender::PCapRTPSender()
: bEnabled(false)
{
}

PCapRTPSender::~PCapRTPSender()
{
    rtpSession.DeleteDestination(rtpAddr);
    rtpSession.Destroy();  
    bEnabled = false;
}

bool PCapRTPSender::CreateRTPSession(const char* daddr, const u_int dport, const u_int pt, const u_int portbase)
{
    // Assume the the Audio is 8KHz.
    double timeStampUnit = 1.0/8000.0;

    // Timestamp increment value: As an example, for fixed-rate audio the timestamp clock would likely increment 
    // by one for each sampling period. If an audio application reads blocks covering 160 sampling periods from 
    // the input device, the timestamp would be increased by 160 for each such block, regardless of whether the 
    // block is transmitted in a packet or dropped as silent.
    int tsi = 160;

	RTPSessionParams sessParams;
	sessParams.SetOwnTimestampUnit(timeStampUnit);
    //sessParams.SetAcceptOwnPackets(true);
	RTPUDPv4TransmissionParams transParams;
	transParams.SetPortbase(portbase);
	int status = rtpSession.Create(sessParams, &transParams);
	if (ReportError(status))
		return false;

    rtpSession.SetDefaultPayloadType(pt);
    rtpSession.SetDefaultMark(false);
	rtpSession.SetDefaultTimestampIncrement(tsi);

	unsigned long intIP = inet_addr(daddr);
	ACE_ASSERT(intIP != INADDR_NONE);
	intIP = ntohl(intIP); //put in host byte order
    rtpAddr.SetIP(intIP);
    rtpAddr.SetPort(dport);
	status = rtpSession.AddDestination(rtpAddr);
	if (ReportError(status))
        return false;

    bEnabled = true;

    return true;
}

bool PCapRTPSender::SendPacket(const void* payload, int len)
{
    if (!bEnabled)
        return false;

    int status = rtpSession.SendPacket(payload, len);
	if (ReportError(status))
        return false;

    return true;
}

int PCapRTPSender::ReportError(int errorCode)
{
	int isErr = (errorCode < 0);
	if (isErr) 
    {
		std::string stdErrStr = RTPGetErrorString(errorCode);
        logger->WriteLog(Log_Error, "Send RTP payload failed: %s", stdErrStr.c_str());
	}
	return isErr;
}


