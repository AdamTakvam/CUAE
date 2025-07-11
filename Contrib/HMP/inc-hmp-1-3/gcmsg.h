/**********************************************************************
*
*	C Header:		gcmsg.h
*	Instance:		gc_1
*	Description:	Header file provides GC number-to-name transforms
*	%created_by:	jinb %
*	%date_created:	Tue Jul 20 11:11:55 2004 %
*
**********************************************************************/
/**********************************************************************
*  Copyright (C) 2000-2004 Intel Corporation.
*  All Rights Reserved
*
*  All names, products, and services mentioned herein are the 
*  trademarks or registered trademarks of their respective 
*  organizations and are the sole property of their respective owners.
**********************************************************************/

#ifndef _gc_1_gcmsg_h_H
#define _gc_1_gcmsg_h_H

#ifndef lint
static
#ifdef __cplusplus          
const             /* C++ needs const */     
#endif
char    *_gc_1_gcmsg_h = "@(#) %filespec: gcmsg.h-10 %  (%full_filespec: gcmsg.h-10:incl:gc#1 %)";
#endif

#ifdef __cplusplus
extern "C" {   /* C++ func bindings to enable funcs to be called from C */
#endif

/* CAUTION: in the following macro, if the event code is greater than 0x800 and less 
   than 0x900, then a GC event name is returned; otherwise, return message
   indicating that a non-GC event code was passed into the macro. */
#define GCEV_MSG(code) (((code >= 0x800) && (code < 0x900))? gcev_msg[code-0x800] : "NON-GLOBALCALL EVENT")

static
#ifdef __cplusplus          
const             /* C++ needs const */     
#endif
	char *gcev_msg[256] = { "GCEV_INVALID_0x800",
                        "GCEV_TASKFAIL",
                        "GCEV_ANSWERED",
                        "GCEV_CALLPROGRESS",
                        "GCEV_ACCEPT",
                        "GCEV_DROPCALL",
                        "GCEV_RESETLINEDEV",
                        "GCEV_CALLINFO",
                        "GCEV_REQANI",
                        "GCEV_SETCHANSTATE",
                        "GCEV_FACILITY_ACK",
                        "GCEV_FACILITY_REJ",
                        "GCEV_MOREDIGITS",
                        "GCEV_INVALID_0x80D",
                        "GCEV_SETBILLING",
                        "GCEV_ATTACH",
                        "GCEV_ATTACH_FAIL",
                        "GCEV_DETACH",
                        "GCEV_DETACH_FAIL",
                        "GCEV_MEDIA_REQ",
                        "GCEV_STOPMEDIA_REQ",
                        "GCEV_MEDIA_ACCEPT",
                        "GCEV_MEDIA_REJ",
                        "GCEV_OPENEX",
                        "GCEV_OPENEX_FAIL",
                        "GCEV_TRACEDATA",
                        "GCEV_INVALID_0x81A",
                        "GCEV_INVALID_0x81B",
                        "GCEV_INVALID_0x81C",
                        "GCEV_INVALID_0x81D",
                        "GCEV_INVALID_0x81E",
                        "GCEV_INVALID_0x81F",
                        "GCEV_INVALID_0x820",
                        "GCEV_ALERTING",
                        "GCEV_CONNECTED",
                        "GCEV_ERROR",
                        "GCEV_OFFERED",
                        "GCEV_INVALID_0x825",
                        "GCEV_DISCONNECTED",
                        "GCEV_PROCEEDING",
                        "GCEV_PROGRESSING",
                        "GCEV_USRINFO",
                        "GCEV_FACILITY",
                        "GCEV_CONGESTION",
                        "GCEV_INVALID_0x82C",
                        "GCEV_INVALID_0x82D",
                        "GCEV_D_CHAN_STATUS",
                        "GCEV_INVALID_0x82F",
                        "GCEV_NOUSRINFOBUF",
                        "GCEV_NOFACILITYBUF",
                        "GCEV_BLOCKED",
                        "GCEV_UNBLOCKED",
                        "GCEV_ISDNMSG",
                        "GCEV_NOTIFY",
                        "GCEV_L2FRAME",
                        "GCEV_L2BFFRFULL",
                        "GCEV_L2NOBFFR",
                        "GCEV_REQMOREINFO",
                        "GCEV_CALLSTATUS",
                        "GCEV_MEDIADETECTED",
                        "GCEV_INVALID_0x83C",
                        "GCEV_INVALID_0x83D",
                        "GCEV_INVALID_0x83E",
                        "GCEV_INVALID_0x83F",
                        "GCEV_DIVERTED",
                        "GCEV_HOLDACK",
                        "GCEV_HOLDCALL",
                        "GCEV_HOLDREJ",
                        "GCEV_RETRIEVEACK",
                        "GCEV_RETRIEVECALL",
                        "GCEV_RETRIEVEREJ",
                        "GCEV_NSI",
                        "GCEV_TRANSFERACK",
                        "GCEV_TRANSFERREJ",
                        "GCEV_TRANSIT",
                        "GCEV_RESTARTFAIL",
                        "GCEV_INVALID_0x84C",
                        "GCEV_INVALID_0x84D",
                        "GCEV_INVALID_0x84E",
                        "GCEV_INVALID_0x84F",
                        "GCEV_ACKCALL",
                        "GCEV_SETUPTRANSFER",
                        "GCEV_COMPLETETRANSFER",
                        "GCEV_SWAPHOLD",
                        "GCEV_BLINDTRANSFER",
                        "GCEV_LISTEN",
                        "GCEV_UNLISTEN",
                        "GCEV_DETECTED",
                        "GCEV_FATALERROR",
                        "GCEV_RELEASECALL",
                        "GCEV_RELEASECALL_FAIL",
                        "GCEV_INVALID_0x85B",
                        "GCEV_INVALID_0x85C",
                        "GCEV_INVALID_0x85D",
                        "GCEV_INVALID_0x85E",
                        "GCEV_INVALID_0x85F",
                        "GCEV_DIALTONE",
                        "GCEV_DIALING",
                        "GCEV_ALARM",
                        "GCEV_MOREINFO",
                        "GCEV_INVALID_0x864",
                        "GCEV_SENDMOREINFO",
                        "GCEV_CALLPROC",
                        "GCEV_NODYNMEM",
                        "GCEV_EXTENSION",
                        "GCEV_EXTENSIONCMPLT",
                        "GCEV_GETCONFIGDATA",
                        "GCEV_GETCONFIGDATA_FAIL",
                        "GCEV_SETCONFIGDATA",
                        "GCEV_SETCONFIGDATA_FAIL",
                        "GCEV_SERVICEREQ",
                        "GCEV_INVALID_0x86F",
                        "GCEV_SERVICERESP",
                        "GCEV_SERVICERESPCMPLT",
                        "GCEV_INVOKE_XFER_ACCEPTED",
                        "GCEV_INVOKE_XFER_REJ",
                        "GCEV_INVOKE_XFER",
                        "GCEV_INVOKE_XFER_FAIL",
                        "GCEV_REQ_XFER",
                        "GCEV_ACCEPT_XFER",
                        "GCEV_ACCEPT_XFER_FAIL",
                        "GCEV_REJ_XFER",
                        "GCEV_REJ_XFER_FAIL",
                        "GCEV_XFER_CMPLT",
                        "GCEV_XFER_FAIL",
                        "GCEV_INIT_XFER",
                        "GCEV_INIT_XFER_REJ",
                        "GCEV_INIT_XFER_FAIL",
                        "GCEV_REQ_INIT_XFER",
                        "GCEV_ACCEPT_INIT_XFER",
                        "GCEV_ACCEPT_INIT_XFER_FAIL",
                        "GCEV_REJ_INIT_XFER",
                        "GCEV_REJ_INIT_XFER_FAIL",
                        "GCEV_TIMEOUT",
                        "GCEV_REQ_MODIFY_CALL",
                        "GCEV_REQ_MODIFY_CALL_UNSUPPORTED",
                        "GCEV_MODIFY_CALL_ACK",
                        "GCEV_MODIFY_CALL_REJ",
                        "GCEV_MODIFY_CALL_FAIL",
                        "GCEV_MODIFY_CALL_CANCEL",
                        "GCEV_CANCEL_MODIFY_CALL",
                        "GCEV_CANCEL_MODIFY_CALL_FAIL",
                        "GCEV_ACCEPT_MODIFY_CALL",
                        "GCEV_ACCEPT_MODIFY_CALL_FAIL",
                        "GCEV_REJECT_MODIFY_CALL",
                        "GCEV_REJECT_MODIFY_CALL_FAIL",
                        "GCEV_SIP_ACK",
                        "GCEV_SIP_ACK_OK",
                        "GCEV_SIP_ACK_FAIL",
                        "GCEV_SIP_200OK",
                        "GCEV_INVALID_0x896",
                        "GCEV_INVALID_0x897",
                        "GCEV_INVALID_0x898",
                        "GCEV_INVALID_0x899",
                        "GCEV_INVALID_0x89A",
                        "GCEV_INVALID_0x89B",
                        "GCEV_INVALID_0x89C",
                        "GCEV_INVALID_0x89D",
                        "GCEV_INVALID_0x89E",
                        "GCEV_INVALID_0x89F",
                        "GCEV_INVALID_0x8A0",
                        "GCEV_INVALID_0x8A1",
                        "GCEV_INVALID_0x8A2",
                        "GCEV_INVALID_0x8A3",
                        "GCEV_INVALID_0x8A4",
                        "GCEV_INVALID_0x8A5",
                        "GCEV_INVALID_0x8A6",
                        "GCEV_INVALID_0x8A7",
                        "GCEV_INVALID_0x8A8",
                        "GCEV_INVALID_0x8A9",
                        "GCEV_INVALID_0x8AA",
                        "GCEV_INVALID_0x8AB",
                        "GCEV_INVALID_0x8AC",
                        "GCEV_INVALID_0x8AD",
                        "GCEV_INVALID_0x8AE",
                        "GCEV_INVALID_0x8AF",
                        "GCEV_INVALID_0x8B0",
                        "GCEV_INVALID_0x8B1",
                        "GCEV_INVALID_0x8B2",
                        "GCEV_INVALID_0x8B3",
                        "GCEV_INVALID_0x8B4",
                        "GCEV_INVALID_0x8B5",
                        "GCEV_INVALID_0x8B6",
                        "GCEV_INVALID_0x8B7",
                        "GCEV_INVALID_0x8B8",
                        "GCEV_INVALID_0x8B9",
                        "GCEV_INVALID_0x8BA",
                        "GCEV_INVALID_0x8BB",
                        "GCEV_INVALID_0x8BC",
                        "GCEV_INVALID_0x8BD",
                        "GCEV_INVALID_0x8BE",
                        "GCEV_INVALID_0x8BF",
                        "GCEV_INVALID_0x8C0",
                        "GCEV_INVALID_0x8C1",
                        "GCEV_INVALID_0x8C2",
                        "GCEV_INVALID_0x8C3",
                        "GCEV_INVALID_0x8C4",
                        "GCEV_INVALID_0x8C5",
                        "GCEV_INVALID_0x8C6",
                        "GCEV_INVALID_0x8C7",
                        "GCEV_INVALID_0x8C8",
                        "GCEV_INVALID_0x8C9",
                        "GCEV_INVALID_0x8CA",
                        "GCEV_INVALID_0x8CB",
                        "GCEV_INVALID_0x8CC",
                        "GCEV_INVALID_0x8CD",
                        "GCEV_INVALID_0x8CE",
                        "GCEV_INVALID_0x8CF",
                        "GCEV_INVALID_0x8D0",
                        "GCEV_INVALID_0x8D1",
                        "GCEV_INVALID_0x8D2",
                        "GCEV_INVALID_0x8D3",
                        "GCEV_INVALID_0x8D4",
                        "GCEV_INVALID_0x8D5",
                        "GCEV_INVALID_0x8D6",
                        "GCEV_INVALID_0x8D7",
                        "GCEV_INVALID_0x8D8",
                        "GCEV_INVALID_0x8D9",
                        "GCEV_INVALID_0x8DA",
                        "GCEV_INVALID_0x8DB",
                        "GCEV_INVALID_0x8DC",
                        "GCEV_INVALID_0x8DD",
                        "GCEV_INVALID_0x8DE",
                        "GCEV_INVALID_0x8DF",
                        "GCEV_INVALID_0x8E0",
                        "GCEV_INVALID_0x8E1",
                        "GCEV_INVALID_0x8E2",
                        "GCEV_INVALID_0x8E3",
                        "GCEV_INVALID_0x8E4",
                        "GCEV_INVALID_0x8E5",
                        "GCEV_INVALID_0x8E6",
                        "GCEV_INVALID_0x8E7",
                        "GCEV_INVALID_0x8E8",
                        "GCEV_INVALID_0x8E9",
                        "GCEV_INVALID_0x8EA",
                        "GCEV_INVALID_0x8EB",
                        "GCEV_INVALID_0x8EC",
                        "GCEV_INVALID_0x8ED",
                        "GCEV_INVALID_0x8EE",
                        "GCEV_INVALID_0x8EF",
                        "GCEV_INVALID_0x8F0",
                        "GCEV_INVALID_0x8F1",
                        "GCEV_INVALID_0x8F2",
                        "GCEV_INVALID_0x8F3",
                        "GCEV_INVALID_0x8F4",
                        "GCEV_INVALID_0x8F5",
                        "GCEV_INVALID_0x8F6",
                        "GCEV_INVALID_0x8F7",
                        "GCEV_INVALID_0x8F8",
                        "GCEV_INVALID_0x8F9",
                        "GCEV_INVALID_0x8FA",
                        "GCEV_INVALID_0x8FB",
                        "GCEV_INVALID_0x8FC",
                        "GCEV_INVALID_0x8FD",
                        "GCEV_INVALID_0x8FE",
                        "GCEV_FACILITYREQ"
};

#ifdef __cplusplus
}
#endif

#endif  /* _gc_1_gcmsg_h_H */
