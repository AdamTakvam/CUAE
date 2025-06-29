#ifndef METREOS_EXTERNAL_RTP_CHANNEL_H
#define METREOS_EXTERNAL_RTP_CHANNEL_H

#ifdef H323_MEM_LEAK_DETECTION
#define DEBUG_NEW new(_NORMAL_BLOCK, __FILE__, __LINE__)
#define new DEBUG_NEW 
#endif

#include "H323Common.h"

namespace Metreos
{

namespace H323
{

///////////////////////////////////////
//
// MetreosExternalRTPChannel
//
// A sub-class of the provided H323_ExternalRTPChannel. 
// We sub-class to override certain methods to enable 
// us to determine when the logical channels have
// been opened.
//
class MetreosExternalRTPChannel : public H323_ExternalRTPChannel
{

public:
    MetreosExternalRTPChannel(
        H323Connection& connection,                             // Connection to endpoint for channel
        const H323Capability& cap,                              // Capability channel is using
        Directions direction,                                   // Direction of channel
        unsigned sessionID                                      // Session ID for channel
    ) : H323_ExternalRTPChannel(connection, cap, direction, sessionID)
    {}

    MetreosExternalRTPChannel(
        H323Connection& connection,                             // Connection to endpoint for channel
        const H323Capability& cap,                              // Capability channel is using
        Directions direction,                                   // Direction of channel
        unsigned sessionID,                                     // Session ID for channel
        const H323TransportAddress& data,                       // Data address
        const H323TransportAddress& control                     // Control address
    ) : H323_ExternalRTPChannel(connection, cap, direction, sessionID, data, control)
    {}

    MetreosExternalRTPChannel(
        H323Connection& connection,                             // Connection to endpoint for channel
        const H323Capability& cap,                              // Capability channel is using
        Directions direction,                                   // Direction of channel
        unsigned sessionID,                                     // Session ID for channel
        const PIPSocket::Address& ip,                           // IP address of media server
        WORD dataPort                                           // Data port (control is dataPort+1)
    ) : H323_ExternalRTPChannel(connection, cap, direction, sessionID, ip, dataPort)
     {}

    //
    // Indicates that our sending logical channel (the channel that terminates
    // to the remote party) has been acknowledged by the remote party and opened.
    //
    virtual BOOL OnReceivedAckPDU(
        const H245_H2250LogicalChannelAckParameters& param      // Acknowledgement PDU
    );

   // virtual BOOL 	OnSendingPDU (H245_OpenLogicalChannel &openPDU) const;
};

} // namespace H323
} // namespace Metreos

#endif // METREOS_EXTERNAL_RTP_CHANNEL_H
