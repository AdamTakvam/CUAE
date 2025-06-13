// PCapCommon.h

/**
 * Definitions for commonly used Macros, and data structures.
 */

#ifndef PCAP_COMMON_H
#define PCAP_COMMON_H

#ifdef WIN32
#   pragma warning(disable : 4786) // Identifier was truncated in debug information
#endif // WIN32

#include <iostream>
#include <sstream>

#include "ptlib.h"

/* * * * * * * * * * * * * * * * * * * * * * * * * * * * *
 * Common ACE includes.
 */
#include "ace/OS.h"
#include "ace/ACE.h"
#include "ace/Task.h"
#include "ace/Synch.h"
#include "ace/Time_Value.h"

#include "logclient/logclient.h"
using namespace Metreos::LogClient;

extern LogServerClient* logger;

#define PCAPLOG(X) do { logger->WriteLog X; } while(0)

#ifndef STATION_MAX_NAME_SIZE
#define STATION_MAX_NAME_SIZE   40
#endif

#ifndef STATION_MAX_DN_SIZE
#define STATION_MAX_DN_SIZE     24
#endif

/* 4 bytes IP address */
typedef struct ip_address
{
  u_char byte1;
  u_char byte2;
  u_char byte3;
  u_char byte4;
} ip_address;

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

/* Ethernet header */
typedef struct ethernet_header 
{
    mac_address dest;
    mac_address src;
    u_short ether_type;
} ethernet_header;

/* Skinny call data */
typedef struct skinny_call_data
{
    bool active;                                        // Active call
    bool monitored;                                     // being monitored now
    bool hold;                                          // is on hold
    u_int msgId;                                        // skinny message id
    u_int callIdentifier;                               // call identifier or conference identifier (for bookkeeping)
    u_int passThruPartyId;                              // Pass through party id for RTP stream reference
    u_int callType;                                     // what kind of call?   (inbound, outbound, or ???)
    u_int callState;                                    // call state           (determine call status)
    u_int payloadCapability;                            // RTP audio type       (reference between RTP and call)
    ip_address localIp;                                 // local ip address     (reference between RTP and call)
    u_int localRTPPort;                                 // local RTP port       (reference between RTP and call)
    ip_address remoteIp;                                // remote ip address    (reference between RTP and call)
    u_int remoteRTPPort;                                // remote RTP port      (reference between RTP and call)
    
    ip_address stationIp;
    char callerDN[STATION_MAX_DN_SIZE];
    char calleeDN[STATION_MAX_DN_SIZE];
    char callerName[STATION_MAX_NAME_SIZE];
    char calleeName[STATION_MAX_NAME_SIZE];
} skinny_call_data;

/* * * * * * * * * * * * * * * * * * * * * * * * * * * * *
 * Microsoft CRT memory leak detection.  Memory leak
 * detection is compiled in or compiled out. To turn on 
 * memory leak detection define "PCAP_MEM_LEAK_DETECTION" 
 * and rebuild the solution.  Run the execution and when
 * the process terminates a list of possible leaks
 * will be printed to the debug window.  Use DebugView
 * to view the leak output.
 */
//#define PCAP_MEM_LEAK_DETECTION
#ifdef WIN32
#ifdef PCAP_MEM_LEAK_DETECTION
#   define _CRTDBG_MAP_ALLOC
#   include <stdlib.h>
#   include <crtdbg.h>
#endif
#endif


/* * * * * * * * * * * * * * * * * * * * * * * * * * * * *
 * Macros used to gather timing statistics around various
 * portions of the code.  These are compiled in or compiled
 * out based on whether "GATHER_TIMING_STATS" is defined.
 * To use these macros, do the following in code:
 * 
 *   STAT_BEGIN(someStatisticName);
 *   ... do something that takes time ...
 *   STAT_END(someStatisticName);
 *   STAT("My statistic is: %d", someStatisticName);
 *
 * That will generate a log file with the result of the timing
 * statistic.
 */
//#define GATHER_TIMING_STATS
#ifdef GATHER_TIMING_STATS
#   define STAT_BEGIN(X)                                \
        ACE_Time_Value X_time = ACE_OS::gettimeofday(); \
        long X;                                         \
        long X_startMs = X_time.msec();

#   define STAT_END(X)                                  \
        X_time = ACE_OS::gettimeofday();                \
        X = X_time.msec() - X_startMs;                  

#   define STAT(X)    do { LogServerClient::Instance()->WriteLog X; } while(0)
#else
#   define STAT_BEGIN(X)
#   define STAT_END(X)
#   define STAT(X)
#endif

#endif // PCAP_COMMON_H