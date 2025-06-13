using System;

namespace Metreos.Providers.SccpProxy.RtpRelay
{
	/// <summary>RTP relay flatmap message definitions</summary>
	public abstract class MsgApi
	{
        public enum MsgTypes
        {
            StartReq        = 1001,
            StartResp 	    = 1002,
            ChangeReq 	    = 1003,
            ChangeResp 	    = 1004,
            StopReq 	    = 1005,
            StopResp 	    = 1006,
            EchoReq 	    = 1007,
            EchoResp 	    = 1008,
            StopEchoReq 	= 1009,
            StopEchoResp 	= 1010,
            ListRelaysReq 	= 1011,
            ListRelaysResp 	= 1012
        }

        public enum Fields
        {
            aEchoIp         = 101,      // ip socket "a" should echo to
            aEchoPort 	    = 102,      // port socket "a" should echo to
            age 	        = 103, 	    // age of relay in seconds
            aIdle 	        = 104, 	    // time in seconds since last packet received at socket "a"
            aIp 	        = 105, 	    // ip socket "a" is connected to
            aPort 	        = 106, 	    // port socket "a" is connected to
            aRelayIp        = 107, 	    // ip socket "a" is listening on
            aRelayIpSel     = 108, 	    // ip selector for socket "a"
            aRelayPort      = 109, 	    // port socket "a" is listening on
            bEchoIp 	    = 110, 	    // ip socket "b" should echo to
            bEchoPort 	    = 111, 	    // port socket "b" should echo to
            bIdle 	        = 112, 	    // time in seconds since last packet received at socket "b"
            bIp 	        = 113, 	    // ip socket "b" is connected to
            bPort 	        = 114, 	    // port socket "b" is connected to
            bRelayIp        = 115, 	    // ip socket "b" is listening on
            bRelayIpSel     = 116, 	    // ip selector for socket "b"
            bRelayPort      = 117, 	    // port socket "b" is listening on
            count 	        = 118, 	    // number of responses
            index 	        = 119, 	    // index of response
            maxPacketSize   = 120, 	    // size in bytes of largest packet to relay
            relayId 	    = 121, 	    // unique id of a relay
            requestId 	    = 1000000106, // id of a request that is simply returned in a response
            resultCode 	    = 123, 	    // code which indicates the result of a request (0 means success)
            tos 	        = 124 	    // type of service for a relay (rfc 1349)
        }

        public enum ToS
        {
            LowCost         = 2,        // minimize cost
            Reliability 	= 4, 	    // maximize reliability
            Throughput 	    = 8, 	    // maximize throughput
            LowDelay 	    = 16 	    // minimize delay
        }

        public enum Interfaces
        {
            External        = 1,
            Internal        = 2,
            DMZ             = 3
        }

        public enum ResultCodes
        {
            Success         = 0, 	    // No problem, everything ok
            couldNotOpenA 	= 1, 	    // the "a" socket could not be opened or connected
            couldNotOpenB 	= 2, 	    // the "b" socket could not be opened or connected
            relayNotFound 	= 3, 	    // the relay could not be found
            notImplemented 	= 4, 	    // that feature is not implemented
            dupAEcho 	    = 5, 	    // socket "a" already has an echo matching those parameters
            aEchoNotFound 	= 6,	    // socket "a" does not have an echo matching those parameters
            couldNotChangeA = 7, 	    // the "a" socket could not be changed
            couldNotChangeB = 8, 	    // the "b" socket could not be changed
            dupBEcho 	    = 9, 	    // socket "b" already has an echo matching those parameters
            bEchoNotFound 	= 10,	    // socket "b" does not have an echo matching those parameters
			tooManyRelays   = 11        // the number of available relays has been exhausted.
        }
	}
}
