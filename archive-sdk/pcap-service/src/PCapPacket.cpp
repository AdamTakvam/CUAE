// PCapPacket.cpp

#include "stdafx.h"

#ifdef WIN32
#ifdef PCAP_MEM_LEAK_DETECTION
#   define DEBUG_NEW new(_NORMAL_BLOCK, __FILE__, __LINE__)
#   define new DEBUG_NEW 
#endif
#endif

#include "PCapCommon.h"
#include "PCapPacket.h"
#include "msgs\PCapMessageTypes.h"

using namespace Metreos;
using namespace Metreos::PCap;

/////////////////////////////////////////////////////////////////////
PCapPacketManager* PCapPacketManager::instance = 0;

/////////////////////////////////////////////////////////////////////
PCapPacketManager::PCapPacketManager()
{
}


PCapPacketManager::~PCapPacketManager()
{
}

PCapPacketManager* PCapPacketManager::Instance()
{
    if(instance == 0)
    {
        instance = new PCapPacketManager();
    }

    return instance;
}

void PCapPacketManager::ProcessPacket(PCapMessage& message)
{
    //char timestr[16];
    //struct tm *ltime;
    pcap_pkthdr* header = message.packetHeader();

    //ltime = localtime(&header->ts.tv_sec);
    //strftime(timestr, sizeof timestr, "%H:%M:%S", ltime);           

    char* pkt_data = message.metreosData();

    int el = 0;
    ethernet_header* eh = (ethernet_header*)pkt_data;
    if (eh->ether_type == 8)            // 0x08
        el = 14;            // IP
    else if (eh->ether_type == 129)     // 0x81
        el = 18;            // VLAN
    else
        return;

	ip_header* ih = (ip_header*) (pkt_data + el);     //length of MAC header, Dest Addr(6), Src Addr(6), Type(2)
    switch(ih->proto)
    {
        case PROTO_TCP:     // TCP
            PCapPacketManager::Instance()->ProcessTCPPacket(message, el);           // el is the length of MAC header + VLAN tag
            break;

        case PROTO_UDP:     // UDP
            if (runtime->PreParseUDPCheck(message, el))
            {
                PCapPacketManager::Instance()->ProcessUDPPacket(message, el);       // el is the length of MAC header + VLAN tag
            }
            break;

        default:
            break;
    } 
}

void PCapPacketManager::ProcessTCPPacket(PCapMessage& message, int offset)
{
    char* pkt_data = message.metreosData();

    // retrieve ip header
	ip_header* ih = (ip_header*) (pkt_data + offset);
    
    // retrieve tcp header
	int ih_len = (ih->ver_ihl & 0xf) * 4;
	tcp_header* th = (tcp_header*) ((u_char*)ih + ih_len);

    int th_len = TH_OFF(th) * 4;

	// convert from network byte order to host byte order
	u_short sport = ntohs(th->sport);
	u_short dport = ntohs(th->dport);

    if (sport == SKINNY_PORT || dport == SKINNY_PORT)
    {
        ProcessSkinnyPacket(message, offset, ih_len, th_len);    
    }
}

void PCapPacketManager::ProcessUDPPacket(PCapMessage& message, int offset)
{
    char* pkt_data = message.metreosData();

    // retrieve ip header
	ip_header* ih = (ip_header*) (pkt_data + offset);
    
    // retrieve udp header
	int ih_len = (ih->ver_ihl & 0xf) * 4;
	udp_header* uh = (udp_header*) ((u_char*)ih + ih_len);

    int uh_len = sizeof(udp_header);

	// convert from network byte order to host byte order
	u_short sport = ntohs(uh->sport);
	u_short dport = ntohs(uh->dport);

    ProcessRTPPacket(message, offset, ih_len, uh_len, ih->saddr, ih->daddr, sport, dport);    
}

/* The general structure of a packet: {IP-Header|TCP-Header|n*SKINNY}
* SKINNY-Packet: {Header(Size, Reserved)|Data(MessageID, Message-Data)}
*/

/*
 * Skinny message header
 *
 *  0                   1                   2                   3
 *  0 1 2 3 4 5 6 7 8 9 0 1 2 3 4 5 6 7 8 9 0 1 2 3 4 5 6 7 8 9 0 1
 * +-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+
 * |                     Packet Size                               |
 * +-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+
 * |                     Reserved (must be 0)                      |
 * +-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+
 * |                     Msg ID                                    |
 * +-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+
 * |                                                               |
 * +                                                               +
 * |                     Msg Data                                  |
 * +                                                               +
 * |                                                               |
 * +-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+
 *
 */
void PCapPacketManager::ProcessSkinnyPacket(PCapMessage& message, int offset, int ih_len, int th_len)
{
    // Skipped header length
    int skip_len = offset + ih_len + th_len;

    // Calculate the length belongs to Skinny itself
    int len = message.packetHeader()->len - skip_len;

    // Find Src and Dst IP Address
    char* pkt_data = message.metreosData();
    // retrieve ip header
	ip_header* ih = (ip_header*) (pkt_data + offset);
    
    // Point to where skinny starts
    char* pSkinny = message.metreosData();
    pSkinny += skip_len;

    int processed;
    int l = sizeof(const struct skinny_common_header);
    while (len > l)
    {
        processed = ProcessSkinnyMessage(ih->saddr, ih->daddr, pSkinny);
        if (processed == 0)
            return;     // Done!
        len -= processed;
        pSkinny += processed;
    }
}

//              Real Time Transport Protocol

// 0                   1                   2                   3
// 0 1 2 3 4 5 6 7 8 9 0 1 2 3 4 5 6 7 8 9 0 1 2 3 4 5 6 7 8 9 0 1
//+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+
//|V=2|P|X|  CC   |M|     PT      |       sequence number         |
//+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+
//|                           timestamp                           |
//+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+
//|           synchronization source (SSRC) identifier            |
//+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+
//|            contributing source (CSRC) identifiers             |
//|                             ....                              |
//+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+

void PCapPacketManager::ProcessRTPPacket(PCapMessage& message, int offset, int ih_len, int uh_len,
                                         ip_address saddr, ip_address daddr, u_short sport, u_short dport)
{
    // Since all the UDP packets are coming here, let's inpect the first two octets and accept only RTP.

    // Skip header length
    int skip_len = offset + ih_len + uh_len;

    // Calculate the length belongs to RTP itself
    int len = message.packetHeader()->len - skip_len;

    if (len <= 12)
    {
        // No reason to go forward, cannot even hold RTP header
        return;
    }

    // Point to where RTP starts
    char* pRtp = message.metreosData();
    pRtp += skip_len;

	u_char* i0 = (u_char*)pRtp;

    u_short version = (*i0 >> 6);

	if (version == 1) 
    {
		// RTP v1
	} 
    else  if (version == 2)
    {
        // RTP
	}
    else
    {
        return;     // Not RTP
    }

    // look into CC filed and determine the size of CSRC
    u_char i1 = (*i0 << 4);
    u_short csrc_len = (i1 >> 4);

    //0 PCMU Audio 8000 1 RFC 3551 
    //1 1016 Audio 8000 1 RFC 3551 
    //2 G721 Audio 8000 1 RFC 3551 
    //3 GSM Audio 8000 1 RFC 3551 
    //4 G723 Audio 8000 1   
    //5 DVI4 Audio 8000 1 RFC 3551 
    //6 DVI4 Audio 16000 1 RFC 3551 
    //7 LPC Audio 8000 1 RFC 3551 
    //8 PCMA Audio 8000 1 RFC 3551 
    //9 G722 Audio 8000 1 RFC 3551 
    //10 L16 Audio 44100 2 RFC 3551 
    //11 L16 Audio 44100 1 RFC 3551 
    //12 QCELP Audio 8000 1   
    //13 CN Audio 8000 1 RFC 3389 
    //14 MPA Audio 90000  RFC 2250, RFC 3551 
    //15 G728 Audio 8000 1 RFC 3551 
    //16 DVI4 Audio 11025 1   
    //17 DVI4 Audio 22050 1   
    //18 G729 Audio 8000 1   
    //19 reserved Audio       
    //20
    //-
    //24           
    //25 CellB Video 90000   RFC 2029 
    //26 JPEG Video 90000   RFC 2435 
    //27           
    //28 nv Video 90000   RFC 3551 
    //29
    //30           
    //31 H261 Video 90000   RFC 2032 
    //32 MPV Video 90000   RFC 2250 
    //33 MP2T Audio/Video 90000   RFC 2250 
    //34 H263 Video 90000

    pRtp += 1;      // move to second octet

    i0 = (u_char*)pRtp;
    i1 = (*i0 << 1);
    u_short payload_type = (i1 >> 1);

    if (payload_type > 34)
    {
        //logger->WriteLog(Log_Verbose, "RTP -->  Invalid RTP, may not be RTP at all, payload type = %d", payload_type);
        return;     // Invalid Payload Type
    }

    pRtp += 7;      // skip M, PT (1), Sequence Number (2), Time Stamp (4)
    u_int* i2 = (u_int*)pRtp;
    u_int i3 = (*i2 << 8);
    u_int ssrc = (i3 >> 8);

    // We want to send this packet back to runtime for RTP routing.
    // Find the actual payload data from this RTP packet
    pRtp += 4;     // Skip RTP header (Should be 12 bytes but we move to end of TimeStamp earlier)    
    // Calculate payload length
    len -= 12;

    // Skip csrc if CC (csrc length) is not 0
    if (csrc_len > 0)
    {
        pRtp += csrc_len;
        len -= csrc_len;
    }

    if (len <= 0)
    {
        //logger->WriteLog(Log_Verbose, "RTP -->   No Payload data!!!");
        return;
    }

    char* packetData = new char[len];
    ACE_OS::memset(packetData, 0, len);
    ACE_OS::memcpy(packetData, pRtp, len); 
    PCapMessage* pMsg = new PCapMessage(packetData, len);
    pMsg->param(len);
    pMsg->type(Msgs::RTP_PAYLOAD);  
    // Use skinny call data to put some ip and port information
    skinny_call_data scd;
    memset(&scd, 0, sizeof(skinny_call_data));
    // The following port and ip naming may be misleading.  Since we do not have call type
    // info here, make sure we remember source=local and dest=remote in this case.
    scd.localIp = saddr;                    // source IP address
    scd.remoteIp = daddr;                   // destination ip address
    scd.localRTPPort = sport;               // source port
    scd.remoteRTPPort = dport;              // destination port
    scd.callIdentifier = ssrc;              // primary key to identify RTP stream in the same session
    scd.payloadCapability = payload_type;   // what kind of payload type
    pMsg->callData(&scd);

    runtime->AddMessage(pMsg);
}

int PCapPacketManager::ProcessSkinnyMessage(const ip_address saddr, const ip_address daddr, const char *pptr) 
{
    const struct skinny_common_header *skinny_com_header;
    const struct skinny_message_header *skinny_msg_header;
    const char *tptr;
    u_int pdu_len, reserved, msg_id;

    tptr = pptr;
    skinny_com_header = (const struct skinny_common_header *)pptr;

    // sanity checking of the header, make sure it is Skinny.
    pdu_len = skinny_com_header->size;
    reserved = skinny_com_header->reserved;
    if (pdu_len < 4 || reserved != 0)       // we need to have at least the length of message id
    {
	    //logger->WriteLog(Log_Verbose, "Not a Skinny packet!!!, packet size = %d, reserved value = %d", pdu_len, reserved);
        return 0;
    }

    // ok, lets fully decode it
    tptr += sizeof(const struct skinny_common_header);

    // we are sure the data size is larger than 4 from earlier check
    skinny_msg_header = (const struct skinny_message_header *)tptr;
    msg_id = skinny_msg_header->msgId;

    // move data pointer to skinny message payload
    tptr += sizeof(const struct skinny_message_header);

    switch(msg_id)
    {
        // cases that do not need to be decoded
        case 0x0 :              /* keepAlive */
            break;

        case 0x6 :              /* offHook */
            break;

        case 0x7 :              /* onHook */
            break;

        case 0x8 :              /* hookFlash */
            break;

        case 0xc :              /* configStateReqMessage */
            break;

        case 0xd :              /* timeDateReqMessage */
            break;

        case 0xe :              /* buttoneTemplateReqMessage */
            break;

        case 0xf :              /* stationVersionReqMessage */
            break;

        case 0x12 :             /* stationServerReqMessage */
            break;

        case 0x25 :             /* softKeySetReqMessage */
            break;

        case 0x27 :             /* unregisterMessage */
            break;

        case 0x28 :             /* softKeyTemplateRequest */
            break;

        case 0x83 :             /* stopTone */
            break;

        case 0x9a :             /* clearDisplay */
            break;

        case 0x9b :             /* capabilitiesReqMessage */
            break;

        case 0x100 :            /* keepAliveAck */
            break;

        case 0x115 :            /* clearNotifyDisplay */
            break;

        case 0x117 :            /* deactivateCallPlane */
            break;

        case 0x11a :            /* registerTokenAck */
            break;

        case 0x13C :            /* AuditConferenceReqMessage */
            break;

        // cases that need decode
        case 0x1 :   /* register message */
            break;

        case 0x2 :  /* ipPortMessage */
            break;

        case 0x3 :  /* keyPadButtonMessage */
            break;

        case 0x4 :  /* stationEnblocCallMessage -- This decode NOT verified*/
            break;

        case 0x5 : /* stationStimulusMessage */
            break;

        case 0x9  : /* stationForwardStatReqMessage */
            break;

        case 0xa :  /* speedDialStatReqMessage */
            break;

        case 0xb :  /* LineStatReqMessage */
            break;

        case 0x10 :  /* capabilitiesResMessage  - VERIFIED AS IS*/
            break;

        case 0x11 : /* mediaPortList */
            break;

        case 0x20 :   /* stationAlarmMessage */
            break;

        case 0x21 : /* stationMulticastMediaReceptionAck - This decode NOT verified*/
            break;

        case 0x22 : /* stationOpenReceiveChannelAck */
            if (pdu_len >= 16)   // 16 is the length of 4 data entries we may want to retrieve
            {
                //openReceiveChannelStatus
                //localIpAddress
                //localIpPort     
                //passThruPartyId

                tptr += 4;          // skip openReceiveChannelStatus

                const struct ip_address *localIpAddress;
                localIpAddress = (const struct ip_address *)tptr;
                tptr += 4;

                if (IsBadReadPtr(tptr, 4)) goto trunc;
                int localPortNumber = get_letohl(tptr);
                tptr += 4;      

                if (IsBadReadPtr(tptr, 4)) goto trunc;
                int passThruPartyId = get_letohl(tptr);

                logger->WriteLog(Log_Verbose, "SKINNY->stationOpenReceiveChannelAck, Pass Through ID = %d, Loal IP = %d.%d.%d.%d:%d", 
                        passThruPartyId,
                        localIpAddress->byte1,
                        localIpAddress->byte2,
                        localIpAddress->byte3,
                        localIpAddress->byte4,
                        localPortNumber);

                skinny_call_data scd;
                memset(&scd, 0, sizeof(skinny_call_data));
                scd.msgId = msg_id;
                scd.passThruPartyId = passThruPartyId;
                scd.localIp = *localIpAddress;
                scd.localRTPPort = localPortNumber;
                PCapMessage* pMsg = new PCapMessage();
                pMsg->type(Msgs::SKINNY_CALL_DATA);                
                pMsg->callData(&scd);
                runtime->AddMessage(pMsg);        
            }
            break;

        case 0x23    :  /* stationConnectionStatisticsRes */
            break;

        case 0x24 : /* offHookWithCgpn */
            break;

        case 0x26 :  /* softKeyEventMessage */
            break;

        case 0x29 : /* registerTokenREq */
            break;

        case 0x2A : /* MediaTransmissionFailure */
            break;

        case 0x2B : /* HeadsetStatusMessage */
            break;

        case 0x2C : /* MediaResourceNotification */
            break;

        case 0x2D : /* RegisterAvailableLinesMessage */
            break;

        case 0x2E : /* DeviceToUserDataMessage */
            break;

        case 0x2F : /* DeviceToUserDataResponseMessage */
            break;

        case 0x30 : /* UpdateCapabilitiesMessage */
            break;

        case 0x31 : /* OpenMultiMediaReceiveChannelAckMessage */
            break;

        case 0x32 : /* ClearConferenceMessage */
            break;

        case 0x33 : /* ServiceURLStatReqMessage */
            break;

        case 0x34 : /* FeatureStatReqMessage */
            break;

        case 0x35 : /* CreateConferenceResMessage */
            break;

        case 0x36 : /* DeleteConferenceResMessage */
            break;

        case 0x37 : /* ModifyConferenceResMessage */
            break;

        case 0x38 : /* AddParticipantResMessage */
            break;

        case 0x39 : /* AuditConferenceResMessage */
            break;

        case 0x40 : /* AuditParticipantResMessage */
            break;

        case 0x41 : /* DeviceToUserDataVersion1Message */
            break;

        case 0x42 : /* DeviceToUserDataResponseVersion1Message */
            break;

        //  Call manager -> client messages start here(ish)
        case 0x81 :  /* registerAck */
            break;

        case 0x82 :  /* startTone */
            break;

        case 0x85 : /* setRingerMessage */
            break;

        case 0x86 : /* setLampMessage */
            break;

        case 0x87 : /* stationHookFlashDetectMode */
            break;

        case 0x88 : /* setSpeakerMode */
            break;

        case 0x89 : /* setMicroMode */
            break;

        case 0x8a : /* startMediaTransmistion */
            if (pdu_len >= 40)   // 40 is the length of 10 data entries we may want to retrieve
            {
                //conferenceID        
                //passThruPartyID
                //remoteIpAddr       
                //remotePortNumber  
                //millisecondPacketSize
                //payloadCapability
                //precedenceValue     
                //silenceSuppression
                //maxFramesPerPacket
                //g723BitRate

                if (IsBadReadPtr(tptr, 4)) goto trunc;
                int conferenceId = get_letohl(tptr);
                tptr += 4;

                if (IsBadReadPtr(tptr, 4)) goto trunc;
                int passThruPartyId = get_letohl(tptr);
                tptr += 4;

                const struct ip_address *remoteIpAddress;
                remoteIpAddress = (const struct ip_address *)tptr;
                tptr += 4;

                if (IsBadReadPtr(tptr, 4)) goto trunc;
                int remotePortNumber = get_letohl(tptr);
                tptr += 8;      // remotePortNumber + millisecondPacketSize

                if (IsBadReadPtr(tptr, 4)) goto trunc;
                int payloadCapability = get_letohl(tptr);                        

                logger->WriteLog(Log_Verbose, "SKINNY->startMediaTransmistion, Conference ID = %d, Pass Through ID = %d, Payload Capability = %d, Remote IP = %d.%d.%d.%d:%d", 
                        conferenceId, 
                        passThruPartyId,
                        payloadCapability,
                        remoteIpAddress->byte1,
                        remoteIpAddress->byte2,
                        remoteIpAddress->byte3,
                        remoteIpAddress->byte4,
                        remotePortNumber);

                skinny_call_data scd;
                memset(&scd, 0, sizeof(skinny_call_data));
                scd.msgId = msg_id;
                scd.callIdentifier = conferenceId;
                scd.passThruPartyId = passThruPartyId;
                scd.remoteIp = *remoteIpAddress;
                scd.remoteRTPPort = remotePortNumber;
                PCapMessage* pMsg = new PCapMessage();
                pMsg->type(Msgs::SKINNY_CALL_DATA);                
                pMsg->callData(&scd);
                runtime->AddMessage(pMsg);        
            }
            break;

        case 0x8b :  /* stopMediaTransmission */
            if (pdu_len >= 8)   // 8 is the lenth of conferenceId and passThruPartyId
            {
                //conferenceID
                //passThruPartyID

                if (IsBadReadPtr(tptr, 4)) goto trunc;
                int conferenceId = get_letohl(tptr);
                tptr += 4;

                if (IsBadReadPtr(tptr, 4)) goto trunc;
                int passThruPartyId = get_letohl(tptr);

                logger->WriteLog(Log_Verbose, "SKINNY->stopMediaTransmission, Conference ID = %d, Pass Through ID = %d", conferenceId, passThruPartyId);

                skinny_call_data scd;
                memset(&scd, 0, sizeof(skinny_call_data));
                scd.msgId = msg_id;
                scd.callIdentifier = conferenceId;
                scd.passThruPartyId = passThruPartyId;
                PCapMessage* pMsg = new PCapMessage();
                pMsg->type(Msgs::SKINNY_CALL_DATA);                
                pMsg->callData(&scd);
                runtime->AddMessage(pMsg);        
            }
            break;

        case 0x8c : /* startMediaReception */
            break;

        case 0x8d : /* stopMediaReception */
            break;

        case 0x8e : /* reservered */
            break;

        case 0x8f : /* callInfo */
            if (pdu_len >= 384)   // we have 20 entries, and the possible length is 384
            {
                //callingPartyName            40
                //callingParty                24
                //calledPartyName             40
                //calledParty                 24
                //lineInstance                4
                //callIdentifier              4
                //callType                    4
                //originalCalledPartyName     40
                //originalCalledParty         24
                //lastRedirectingPartyName    40
                //lastRedirectingParty        24
                //originalCdpnRedirectReason  4
                //lastRedirectingReason       4
                //cgpnVoiceMailbox            24
                //cdpnVoiceMailbox            24
                //originalCdpnVoiceMailbox    24
                //lastRedirectingVoiceMailbox 24
                //callInstance                4
                //callSecurityStatus          4
                //partyPIRestrictionBits      4

                if (IsBadReadPtr(tptr, STATION_MAX_NAME_SIZE)) goto trunc;
                char CallingPartyName[STATION_MAX_NAME_SIZE+1];
                memset(&CallingPartyName, 0, sizeof(CallingPartyName));
                memcpy(CallingPartyName, tptr, STATION_MAX_NAME_SIZE);
                CallingPartyName[STATION_MAX_NAME_SIZE] = 0;
                tptr += STATION_MAX_NAME_SIZE;

                if (IsBadReadPtr(tptr, STATION_MAX_DN_SIZE)) goto trunc;
                char CallingParty[STATION_MAX_DN_SIZE+1];
                memset(&CallingParty, 0, sizeof(CallingParty));
                memcpy(CallingParty, tptr, STATION_MAX_DN_SIZE);
                CallingParty[STATION_MAX_DN_SIZE] = 0;
                tptr += STATION_MAX_DN_SIZE;

                if (IsBadReadPtr(tptr, STATION_MAX_NAME_SIZE)) goto trunc;
                char CalledPartyName[STATION_MAX_NAME_SIZE+1];
                memset(&CalledPartyName, 0, sizeof(CalledPartyName));
                memcpy(CalledPartyName, tptr, STATION_MAX_NAME_SIZE);
                CalledPartyName[STATION_MAX_NAME_SIZE] = 0;
                tptr += STATION_MAX_NAME_SIZE;

                if (IsBadReadPtr(tptr, STATION_MAX_DN_SIZE)) goto trunc;
                char CalledParty[STATION_MAX_DN_SIZE+1];
                memset(&CalledParty, 0, sizeof(CalledParty));
                memcpy(CalledParty, tptr, STATION_MAX_DN_SIZE);
                CalledParty[STATION_MAX_DN_SIZE] = 0;
                tptr += (STATION_MAX_DN_SIZE+4);        // Also skip lineInstance

                if (IsBadReadPtr(tptr, 4)) goto trunc;
                int callIdentifier = get_letohl(tptr);
                tptr += 4;

                if (IsBadReadPtr(tptr, 4)) goto trunc;
                int callType = get_letohl(tptr);

                if (callType != 1 && callType != 2)
                {
                    //logger->WriteLog(Log_Verbose, "Invalid call info packet!!!");
                    break;
                }

                skinny_call_data scd;
                memset(&scd, 0, sizeof(skinny_call_data));
                scd.msgId = msg_id;
                scd.callIdentifier = callIdentifier;
                scd.stationIp = daddr;
                strcpy(scd.callerDN, CallingParty);
                strcpy(scd.calleeDN, CalledParty);
                strcpy(scd.callerName, CallingPartyName);
                strcpy(scd.calleeName, CalledPartyName);
                scd.callType = callType;
                PCapMessage* pMsg = new PCapMessage();
                pMsg->type(Msgs::SKINNY_CALL_DATA);                
                pMsg->callData(&scd);

                logger->WriteLog(Log_Verbose, "SKINNY->callInfo, Call ID = %d, Call Type = %d, From %s:%s To %s:%s, Station IP = %d.%d.%d.%d", 
                        scd.callIdentifier, 
                        scd.callType,
                        CallingPartyName,
                        scd.callerDN,
                        CalledPartyName,
                        scd.calleeDN,
                        scd.stationIp.byte1, scd.stationIp.byte2, scd.stationIp.byte3, scd.stationIp.byte4);

                runtime->AddMessage(pMsg);        
            }
            break;

        case 0x90 : /* forwardStat */
            break;

        case 0x91 : /* speedDialStatMessage */
            break;

        case 0x92 : /* lineStatMessage */
            break;

        case 0x93 : /* configStat */
            break;

        case 0x94 : /* stationDefineTimeDate */
            break;

        case 0x95 : /* startSessionTransmission */
            break;

        case 0x96 : /* stopSessionTransmission */
            break;

        case 0x97 :  /* buttonTemplateMessage  */
            break;

        case 0x98 : /* version */
            break;

        case 0x99 :  /* displayTextMessage */
            break;

        case 0x9c : /* enunciatorCommand */
            break;

        case 0x9d : /* stationRegisterReject */
            break;

        case 0x9e : /* serverRes */
            break;

        case 0x9f :   /* reset */
            break;

        case 0x101 : /* startMulticastMediaReception*/
            break;

        case 0x102 : /* startMulticateMediaTermination*/
            break;

        case 0x103 : /* stopMulticastMediaReception*/
            break;

        case 0x104 : /* stopMulticastMediaTermination*/
            break;

        case 0x105 : /* open receive channel */
            if (pdu_len >= 24)   // 24 is length of 6 data entries we may want to retrieve
            {
                //conferenceID
                //passThruPartyID
                //millisecondPacketSize
                //payloadCapability
                //echoCancelType
                //g723BitRate

                if (IsBadReadPtr(tptr, 4)) goto trunc;
                int conferenceId = get_letohl(tptr);
                tptr += 4;

                if (IsBadReadPtr(tptr, 4)) goto trunc;
                int passThruPartyId = get_letohl(tptr);
                tptr += 8;      // passThruPartyId + millisecondPacketSize

                if (IsBadReadPtr(tptr, 4)) goto trunc;
                int payloadCapability = get_letohl(tptr);                        

                logger->WriteLog(Log_Verbose, "SKINNY->open receive channel, Conference ID = %d, Pass Through ID = %d, Payload Capability = %d", 
                        conferenceId, 
                        passThruPartyId,
                        payloadCapability);

                skinny_call_data scd;
                memset(&scd, 0, sizeof(skinny_call_data));
                scd.msgId = msg_id;
                scd.callIdentifier = conferenceId;
                scd.passThruPartyId = passThruPartyId;
                scd.payloadCapability = payloadCapability;
                PCapMessage* pMsg = new PCapMessage();
                pMsg->type(Msgs::SKINNY_CALL_DATA);                
                pMsg->callData(&scd);
                runtime->AddMessage(pMsg);        
            }
            break;

        case 0x106 :  /* closeReceiveChannel */
            if (pdu_len >= 8)   // 8 is the lenth of conferenceId and passThruPartyId
            {
                //conferenceID
                //passThruPartyID

                if (IsBadReadPtr(tptr, 4)) goto trunc;
                int conferenceId = get_letohl(tptr);
                tptr += 4;

                if (IsBadReadPtr(tptr, 4)) goto trunc;
                int passThruPartyId = get_letohl(tptr);

                logger->WriteLog(Log_Verbose, "SKINNY->closeReceiveChannel, Conference ID = %d, Pass Through ID = %d", conferenceId, passThruPartyId);

                skinny_call_data scd;
                memset(&scd, 0, sizeof(skinny_call_data));
                scd.msgId = msg_id;
                scd.callIdentifier = conferenceId;
                scd.passThruPartyId = passThruPartyId;
                PCapMessage* pMsg = new PCapMessage();
                pMsg->type(Msgs::SKINNY_CALL_DATA);                
                pMsg->callData(&scd);
                runtime->AddMessage(pMsg);        
            }
            break;

        case 0x107 :  /* connectionStatisticsReq */
            break;

        case 0x108 :   /* softkeyTemplateResMessage */
            break;

        case 0x109 : /* softkeysetres */
            break;

        case 0x110 : /* selectSoftKeys */
            break;

        case 0x111 : /* callState */
            if (pdu_len >= 12)   // 12 is the length of 3 data entries we may want to retrieve
            {
                //callState
                //lineInstance
                //callIdentifier

                if (IsBadReadPtr(tptr, 4)) goto trunc;
                int callState = get_letohl(tptr);
                tptr += 8;      // 8 is callState + lineInstance

                if (IsBadReadPtr(tptr, 4)) goto trunc;
                int callIdentifier = get_letohl(tptr);

                logger->WriteLog(Log_Verbose, "SKINNY->callState, Call State = %d, Call ID = %d", callState, callIdentifier);

                skinny_call_data scd;
                memset(&scd, 0, sizeof(skinny_call_data));
                scd.msgId = msg_id;
                scd.callIdentifier = callIdentifier;
                scd.callState = callState;
                PCapMessage* pMsg = new PCapMessage();
                pMsg->type(Msgs::SKINNY_CALL_DATA);                
                pMsg->callData(&scd);
                runtime->AddMessage(pMsg);        
            }
            break;

        case 0x112 : /* displayPromptStatus */
            break;

        case 0x113: /* clearPrompt */
            break;

        case 0x114 : /* displayNotify */
            break;

        case 0x116 : /* activateCallPlane */
            break;

        case 0x118 :    /* unregisterAckMessage */
            break;

        case 0x119 : /* backSpaceReq */
            break;

        case 0x11B : /* registerTokenReject */
            break;

        case 0x11C : /* StartMediaFailureDetection */
            break;

        case 0x11D : /* DialedNumberMessage */
            break;

        case 0x11E : /* UserToDeviceDataMessage */
            break;

        case 0x11F : /* FeatureStatMessage */
            break;

        case 0x120 : /* DisplayPriNotifyMessage */
            break;

        case 0x121 : /* ClearPriNotifyMessage */
            break;

        case 0x122 : /* StartAnnouncementMessage */
            break;

        case 0x123 : /* StopAnnouncementMessage */
            break;

        case 0x124 : /* AnnouncementFinishMessage */
            break;

        case 0x127 : /* NotifyDtmfToneMessage */
            break;

        case 0x128 : /* SendDtmfToneMessage */
            break;

        case 0x129 : /* SubscribeDtmfPayloadReqMessage */
            break;

        case 0x12A : /* SubscribeDtmfPayloadResMessage */
            break;

        case 0x12B : /* SubscribeDtmfPayloadErrMessage */
            break;

        case 0x12C : /* UnSubscribeDtmfPayloadReqMessage */
            break;

        case 0x12D : /* UnSubscribeDtmfPayloadResMessage */
            break;

        case 0x12E : /* UnSubscribeDtmfPayloadErrMessage */
            break;

        case 0x12F : /* ServiceURLStatMessage */
            break;

        case 0x130 : /* CallSelectStatMessage */
            break;

        case 0x131 : /* OpenMultiMediaChannelMessage */
            break;

        case 0x132 : /* StartMultiMediaTransmission */
            break;

        case 0x133 : /* StopMultiMediaTransmission */
            break;

        case 0x134 : /* MiscellaneousCommandMessage */
            break;

        case 0x135 : /* FlowControlCommandMessage */
            break;

        case 0x136 : /* CloseMultiMediaReceiveChannel */
            break;

        case 0x137 : /* CreateConferenceReqMessage */
            break;

        case 0x138 : /* DeleteConferenceReqMessage */
            break;

        case 0x139 : /* ModifyConferenceReqMessage */
            break;

        case 0x13A : /* AddParticipantReqMessage */
            break;

        case 0x13B : /* DropParticipantReqMessage */
            break;

        case 0x13D : /* AuditParticipantReqMessage */
            break;

        case 0x13F : /* UserToDeviceDataVersion1Message */
            break;

        default:
            logger->WriteLog(Log_Warning, "SKINNY->UKNOWN MESSAGE ID, %d", msg_id);
            break;
    }

    return (pdu_len + sizeof(struct skinny_common_header));

trunc:
    return 0;
}
