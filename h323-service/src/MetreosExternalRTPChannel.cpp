/**
 * $Id: MetreosExternalRTPChannel.cpp 19103 2006-01-04 21:57:35Z jdliau $
 */

#include "stdafx.h"

#ifdef WIN32
#ifdef H323_MEM_LEAK_DETECTION
#   define DEBUG_NEW new(_NORMAL_BLOCK, __FILE__, __LINE__)
#   define new DEBUG_NEW 
#endif
#endif

#include "H323Common.h"

#include "MetreosConnection.h"
#include "MetreosExternalRTPChannel.h"

using namespace Metreos::H323;


BOOL MetreosExternalRTPChannel::OnReceivedAckPDU(const H245_H2250LogicalChannelAckParameters& param)
{
    logger->WriteLog(Log_Info, "%s: OnReceivedAckPDU", (const char*)this->connection.GetCallToken());
    bool result = H323_ExternalRTPChannel::OnReceivedAckPDU(param);

    if(result == false) 
        return false;

    MetreosConnection& conx = static_cast<MetreosConnection&>(this->connection);

    if(remoteMediaAddress.IsEmpty())
    {
        logger->WriteLog(Log_Error, "No remote media address");
        logger->WriteLog(Log_Error, "dir: %d  ses: %d", GetDirection(), GetSessionID());
        
        return false;
    }

    PIPSocket::Address remoteIp;
    unsigned short remotePort;

    remoteMediaAddress.GetIpAndPort(remoteIp, remotePort);
    
    conx.OnReceivedRemotePartyMediaParams(remoteIp.AsString(), remotePort);

    return result;
}
