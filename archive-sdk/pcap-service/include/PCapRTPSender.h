// PCapRTPSender.h

/**
 * RTP sender implementation based on JRTPLIB v3.1.0, a free open source RTP stack
 * developed at Expertise Centre for Digital Media (EDM), a research institute of 
 * the Limburgs Universitair Centrum (LUC).
 * http://research.edm.luc.ac.be/jori/jrtplib/jrtplib.html
 *
 * The sole purpose for PCapRTPSession is to package RTP payload to a pre-defined destination.
 */ 

#ifndef PCAP_RTPSENDER_H
#define PCAP_RTPSENDER_H

#ifdef PCAP_MEM_LEAK_DETECTION
#define DEBUG_NEW new(_NORMAL_BLOCK, __FILE__, __LINE__)
#define new DEBUG_NEW 
#endif

#include "PCapCommon.h" 

// includes for JRTPLib
#include "RTPSession.h"
#include "RTPIPv4Address.h"


namespace Metreos
{

namespace PCap
{

class PCapRTPSender
{
public:
    PCapRTPSender();
    virtual ~PCapRTPSender();

    bool CreateRTPSession(const char* daddr, const u_int dport, const u_int pt, const u_int portbase = 25000);

    bool SendPacket(const void* payload, int len);

protected:
    int ReportError(int errorCode);

private:
    RTPSession rtpSession;      // RTP session
    RTPIPv4Address rtpAddr;     // RTP destination address
    bool bEnabled;              // Is session enabled?
};  // PCapRTPSender

}   // namespace PCap
}   // namespace Metreos

#endif // PCAP_RTPSENDER_H