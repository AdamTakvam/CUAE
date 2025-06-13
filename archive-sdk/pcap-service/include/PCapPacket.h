// PCapPacket.h

/**
 * The core parser implementation for the following types of packets:
 * 1. TCP, to identify Skinny packets.
 * 2. UDP, to identify RTP packets.
 * 3. Skinny, to gather call control related events, data, and commands.
 * 4. RTP, to retrieve RTP payload and RTP session related information.
 */

#ifndef PCAP_PACKET_H
#define PCAP_PACKET_H

#include "PCapMessage.h"
#include "PCapRuntime.h"

#ifdef WIN32
#ifdef PCAP_MEM_LEAK_DETECTION
#   define DEBUG_NEW new(_NORMAL_BLOCK, __FILE__, __LINE__)
#   define new DEBUG_NEW 
#endif
#endif

#ifndef PROTO_TCP
#define PROTO_TCP   6
#endif

#ifndef PROTO_UDP
#define PROTO_UDP   17
#endif

#ifndef SKINNY_PORT
#define SKINNY_PORT 2000
#endif

#ifndef CAPTURE_BUFFER_SIZE
#define CAPTURE_BUFFER_SIZE     20*1024*1024     // 20MB
#endif

#ifndef SKINNY_INBOUND_CALL
#define SKINNY_INBOUND_CALL    1
#endif

#ifndef SKINNY_OUTBOUND_CALL
#define SKINNY_OUTBOUND_CALL    2
#endif

// Skinny messages which we care about
#define SKINNY_MSG_STATIONOPENRECEIVECHANNELACK     0x22
#define SKINNY_MSG_STARTMEDIATRANSMISSION           0x8a
#define SKINNY_MSG_STOPMEDIATRANSMISSION            0x8b
#define SKINNY_MSG_CALLINFO                         0x8f
#define SKINNY_MSG_OPENRECEIVECHANNEL               0x105
#define SKINNY_MSG_CLOSERECEIVECHANNEL              0x106
#define SKINNY_MSG_CALLSTATE                        0x111

// Skinny Call Types
#define SKINNY_INBOUND_CALL                         1
#define SKINNY_OUTBOUND_CALL                        2
#define SKINNY_FORWARD_CALL                         3

// Skinny Call States
#define SKINNY_OFFHOOK                              1
#define SKINNY_ONHOOK                               2
#define SKINNY_RINGOUT                              3
#define SKINNY_RINGIN                               4
#define SKINNY_CONNECTED                            5
#define SKINNY_BUSY                                 6
#define SKINNY_CONGESTION                           7
#define SKINNY_HOLD                                 8
#define SKINNY_CALLWAITING                          9
#define SKINNY_CALLTRANSFER                         10
#define SKINNY_CALLPARK                             11
#define SKINNY_PROCEED                              12
#define SKINNY_CALLREMOTEMULTILINE                  13
#define SKINNY_INVALIDNUMBER                        14
#define SKINNY_PRE_ONHOOK                           102   

#define get_letohl(p)   ((u_int)*((const u_char *)(p)+3)<<24|  \
                        (u_int)*((const u_char *)(p)+2)<<16|  \
                        (u_int)*((const u_char *)(p)+1)<<8|   \
                        (u_int)*((const u_char *)(p)+0)<<0)                             // From WinDump
        
typedef	u_int   tcp_seq;

/* IPv4 header */
typedef struct ip_header
{
  u_char ver_ihl;               // Version (4 bits) + Internet header length (4 bits)
  u_char tos;			        // Type of service 
  u_short tlen;			        // Total length 
  u_short identification;       // Identification
  u_short flags_fo;		        // Flags (3 bits) + Fragment offset (13 bits)
  u_char ttl;			        // Time to live
  u_char proto;			        // Protocol
  u_short crc;			        // Header checksum
  ip_address saddr;		        // Source address
  ip_address daddr;		        // Destination address
  u_int op_pad;			        // Option + Padding
} ip_header;

/* TCP header */
typedef struct tcp_header 
{
    u_short	sport;		        // source port
    u_short	dport;		        // destination port
    tcp_seq	seq;			    // sequence number
    tcp_seq	ack;			    // acknowledgement number
    u_char	offx2;		        // data offset, rsvd
#define TH_OFF(th)	(((th)->offx2 & 0xf0) >> 4)
	u_char	flags;
#define	TH_FIN	0x01
#define	TH_SYN	0x02
#define	TH_RST	0x04
#define	TH_PUSH	0x08
#define	TH_ACK	0x10
#define	TH_URG	0x20
#define TH_ECNECHO	0x40	    // ECN Echo
#define TH_CWR		0x80	    // ECN Cwnd Reduced
	u_short	win;			    // window 
	u_short	sum;			    // checksum 
	u_short	urp;			    // urgent pointer
} tcp_header;

/* UDP header*/
typedef struct udp_header
{
	u_short sport;              // Source port
	u_short dport;			    // Destination port
	u_short len;			    // Datagram length
	u_short crc;			    // Checksum
} udp_header;

/* Skinny common header */
typedef struct skinny_common_header 
{
    u_int size;
    u_int reserved;
} skinny_common_header;

/* Skinny message header */
typedef struct skinny_message_header 
{
    u_int msgId;
} skinny_message_header;


using namespace Metreos;
using namespace Metreos::PCap;

namespace Metreos
{

namespace PCap
{
class PCapPacketManager
{
public:
	PCapPacketManager();
	~PCapPacketManager();

    static PCapPacketManager* Instance();

    void SetRuntime(PCapRuntime* r) { runtime = r; }

    void ProcessPacket(PCapMessage& message);
    void ProcessTCPPacket(PCapMessage& message, int offset);
    void ProcessUDPPacket(PCapMessage& message, int offset);
    void ProcessRTPPacket(PCapMessage& message, int offset, int ih_len, int uh_len,
                          ip_address saddr, ip_address daddr, u_short sport, u_short dport);
    void ProcessSkinnyPacket(PCapMessage& message, int offset, int ih_len, int th_len);

protected:
    static PCapPacketManager*  instance;

    int ProcessSkinnyMessage(const ip_address saddr, const ip_address daddr, const char *pptr);

    PCapRuntime* runtime;
};  // PCapPacketManager

}   // PCap

}   // Metreos

#endif

