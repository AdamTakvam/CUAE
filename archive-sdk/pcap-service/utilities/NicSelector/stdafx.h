// stdafx.h : include file for standard system include files,
// or project specific include files that are used frequently,
// but are changed infrequently

#pragma once

#ifndef VC_EXTRALEAN
#define VC_EXTRALEAN		// Exclude rarely-used stuff from Windows headers
#endif

// Modify the following defines if you have to target a platform prior to the ones specified below.
// Refer to MSDN for the latest info on corresponding values for different platforms.
#ifndef WINVER				// Allow use of features specific to Windows 95 and Windows NT 4 or later.
#define WINVER 0x0400		// Change this to the appropriate value to target Windows 98 and Windows 2000 or later.
#endif

#ifndef _WIN32_WINNT		// Allow use of features specific to Windows NT 4 or later.
#define _WIN32_WINNT 0x0400		// Change this to the appropriate value to target Windows 98 and Windows 2000 or later.
#endif						

#ifndef _WIN32_WINDOWS		// Allow use of features specific to Windows 98 or later.
#define _WIN32_WINDOWS 0x0410 // Change this to the appropriate value to target Windows Me or later.
#endif

#ifndef _WIN32_IE			// Allow use of features specific to IE 4.0 or later.
#define _WIN32_IE 0x0400	// Change this to the appropriate value to target IE 5.0 or later.
#endif

#define _ATL_CSTRING_EXPLICIT_CONSTRUCTORS	// some CString constructors will be explicit

// turns off MFC's hiding of some common and often safely ignored warning messages
#define _AFX_ALL_WARNINGS

#include <afxwin.h>         // MFC core and standard components
#include <afxext.h>         // MFC extensions
#include <afxdisp.h>        // MFC Automation classes

#include <afxdtctl.h>		// MFC support for Internet Explorer 4 Common Controls
#ifndef _AFX_NO_AFXCMN_SUPPORT
#include <afxcmn.h>			// MFC support for Windows Common Controls
#endif // _AFX_NO_AFXCMN_SUPPORT

#include <afxsock.h>		// MFC socket extensions

typedef	u_int   tcp_seq;



/* 6 bytes MAC address */
typedef struct mac_address
{
  u_char byte1;
  u_char byte2;
  u_char byte3;
  u_char byte4;
  u_char byte5;
  u_char byte6;
} mac_address;

/* 4 bytes IP address */
typedef struct ip_address
{
  u_char byte1;
  u_char byte2;
  u_char byte3;
  u_char byte4;
} ip_address;

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

#define SKINNY_PORT     2000

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

/* Ethernet header */
typedef struct ethernet_header 
{
    mac_address dest;
    mac_address src;
    u_short ether_type;
} ethernet_header;


