/**********************************************************************
 *        File: cllib.h
 * Description: Header File for the Call Logging API (cllib.dll)
 *
 *  Copyright (C) 2000-2002 Intel Corporation
 *  All Rights Reserved
 *
 *  THIS IS UNPUBLISHED PROPRIETARY SOURCE CODE OF Intel Corporation
 *  The copyright notice above does not evidence any actual or
 *  intended publication of such source code.
 **********************************************************************/

#ifndef __CLLIB_H__
#define __CLLIB_H__

#ifdef _WIN32

#	ifdef CLLIB_EXPORTS
#		define CLLIB_API __declspec(dllexport)
#	else
#		define CLLIB_API __declspec(dllimport)
#	endif

#else /* _WIN32 */
#		define CLLIB_API
#endif /* _WIN32 */

#include <time.h>

#ifdef __cplusplus
extern "C" {
#endif


/* CL events */
#define CLEV_BASE		0x3000
#define CLEV_MESSAGE	(CLEV_BASE | 0x01)
#define CLEV_ALARM		(CLEV_BASE | 0x02)
#define CLEV_ERROR		(CLEV_BASE | 0x03)

/* CL result codes (reported in the CL_EVENTDATA.iResult field of CLEV_MESSAGE events) */
#define ECL_NOERR					0x00000000
#define ECL_FIRST_MESSAGE			0x00000001
#define ECL_CONNECT_MESSAGE			0x00000002
#define ECL_DISCONNECT_MESSAGE		0x00000004
#define ECL_LAST_MESSAGE			0x00000008
#define ECL_STATE_HAS_CHANGED		0x00000010
#define ECL_WRONG_FIRST_MESSAGE		0x00000020

/* CL error codes (reported in the CL_EVENTDATA.iResult field of CLEV_ERROR events) */
#define ECL_UNRECOGNIZED_L3MESSAGE	0x00000040
#define ECL_UNRECOGNIZED_L2FRAME	0x00000080
#define ECL_L2FRAMES_WERE_LOST		0x00000100
#define ECL_L2LAYER_WAS_RESTARTED	0x00000200
#define ECL_OUT_OF_MEMORY			0x80000000

/* CL error codes (reported by ATDV_LASTERR(), or in errno when cl_Open() fails) */
#define ECL_BASE				0x3000
#define ECL_FLAG_INSIDE_GC		0x0800	/* refer to gc_ErrorValue() for additional error description */
#define ECL_NOMEM				(ECL_BASE | 0x01)	/* out of memory */
#define ECL_NULLPARAMETER		(ECL_BASE | 0x02)	/* invalid NULL parameter */
#define ECL_INVALIDPARAMETER	(ECL_BASE | 0x03)	/* invalid parameter */
#define ECL_INVALIDCONTEXT		(ECL_BASE | 0x04)	/* invalid event context */
#define ECL_TRANSACTIONRELEASED	(ECL_BASE | 0x05)	/* transaction already released */
#define ECL_UNSUPPORTED			(ECL_BASE | 0x06)	/* function not supported */
#define ECL_FILEOPEN			(ECL_BASE | 0x07)	/* fopen failed */
#define ECL_FILECLOSE			(ECL_BASE | 0x08)	/* fclose failed */
#define ECL_FILEREAD			(ECL_BASE | 0x09)	/* fread failed */
#define ECL_FILEWRITE			(ECL_BASE | 0x0A)	/* fwrite failed */
#define ECL_TRACESTARTED		(ECL_BASE | 0x0B)	/* trace already started */
#define ECL_TRACENOTSTARTED		(ECL_BASE | 0x0C)	/* trace not started */
#define ECL_INVALIDDEVICE		(ECL_BASE | 0x0D)	/* invalid device handle */
#define ECL_INTERNAL			(ECL_BASE | 0x0E)	/* internal error */
#define ECL_GCOPENEX_NETWORK	(ECL_BASE | 0x0F | ECL_FLAG_INSIDE_GC)	/* gc_OpenEx failed on network-side */
#define ECL_GCOPENEX_USER		(ECL_BASE | 0x10 | ECL_FLAG_INSIDE_GC)	/* gc_OpenEx failed on user-side */
#define ECL_DTOPEN_BOARD		(ECL_BASE | 0x11)	/* dt_open failed */
#define ECL_ATDVSUBDEVS_BOARD	(ECL_BASE | 0x12)	/* ATDV_SUBDEVS failed */
#define ECL_HIZOPEN_CHANNEL		(ECL_BASE | 0x13)	/* hiz_open failed */

/* CL message source codes */
#define CL_SOURCE_NETWORK	1
#define CL_SOURCE_USER		2
#define CL_SOURCE_INVALID	(-1)

/* CL reference numbers */
#define CL_REFERENCE_MASK	0x00FFFFFF
#define CL_REFERENCE_USER	0xCE000000	// Customer Equipment reference number

/* CL event data base structure, as retrieved by sr_getevtdatap() or gc_GetMetaEvent() */
typedef struct CL_EVENTDATA_struct
{
	int		iResult;
	time_t	timeEvent;
} CL_EVENTDATA;

/* CL API */

CLLIB_API int __cdecl
cl_Open(long* phDevice, const char* pszDeviceName, void* pUsrAttr);
/*
// pszDeviceName: ":<key>_<value>"...
//   key = M (method used to get L2 frames, or to collect TSC CallState/CallInfo)
//     value = HDLC, FILE, TSC
//   key = P (protocol) - not supported with :M_TSC
//     value = ISDN, NET5, QSIGE1, 4ESS, 5ESS, DMS, NI2, NTT, QSIGT1
//   key = N (network-side trunk/board name, or list of HiZ device names) - ignored with :M_FILE
//     value = dtiBx, or comma-separated list of dtiBxTy, dtiBxTy-dtiBxTz, dtiBx or dtiBx-dtiBw
//   key = U (user-side trunk/board name) - ignored with :M_FILE, not supported with :M_TSC
//     value = dtiBx
//   all other keys are reserved for future use
// returns:
//   0 on success and call-logging device handle in *phDevice
//  -1 on failure
// remarks:
//   :P_ISDN can be used as a synonym for :P_NET5
*/

CLLIB_API int __cdecl
cl_Close(long hDevice);
/*
// returns: 0 on success, -1 on failure
*/

CLLIB_API int __cdecl
cl_SetUsrAttr(long hDevice, void* pUsrAttr);
/*
// returns: 0 on success, -1 on failure
*/

CLLIB_API int __cdecl
cl_GetUsrAttr(long hDevice, void** ppUsrAttr);
/*
// returns:
//   0 on success and pointer to call-logging device user attribute in *ppUsrAttr
//  -1 on failure
*/

CLLIB_API int __cdecl
cl_DecodeTrace(long hDevice, const char* pszFileName);
/*
// returns:
//   0 on success
//  -1 on failure
// remarks:
//   can only be called when FILE method specified in cl_Open/pszDeviceName
*/

CLLIB_API int __cdecl
cl_StartTrace(long hDevice, const char* pszFileName);
/*
// returns:
//   0 on success
//  -1 on failure
*/

CLLIB_API int __cdecl
cl_StopTrace(long hDevice);
/*
// returns:
//   0 on success
//  -1 on failure
*/

CLLIB_API int __cdecl
cl_GetSemanticsStateCount(long hDevice, int* piSemanticsStateCount);
/*
// returns:
//   0 on success and count of semantics states in *piSemanticsStateCount
//  -1 on failure
// remarks:
//   semantics states are indexed from 0 to (count - 1)
*/

CLLIB_API int __cdecl
cl_GetSemanticsStateName(	long hDevice, int iSemanticsStateIndex,
							char* pszSemanticsStateName, int iSemanticsStateNameSize );
/*
// returns:
//   0 on success and name of semantics state specified by iSemanticsStateIndex in pszSemanticsStateName
//  -1 on failure
*/

CLLIB_API int __cdecl
cl_GetSemanticsStateIndex(	long hDevice, int* piSemanticsStateIndex,
							const char* pszSemanticsStateName );
/*
// returns:
//   0 on success and index of semantics state specified by pszSemanticsStateName in *piSemanticsStateIndex
//  -1 on failure
*/

CLLIB_API int __cdecl
cl_GetTransaction(long hDevice, long* pidTransaction, CL_EVENTDATA* pclEventData);
/*
// returns:
//   0 on success and call-logging transaction ID in *pidTransaction
//  -1 on failure
//  -2 if call-logging transaction was already released
// remarks:
//   can only be called while processing a CLEV_MESSAGE event
*/

CLLIB_API int __cdecl
cl_GetTransactionDetails(	long hDevice, long* pidTransaction, CL_EVENTDATA* pclEventData,
							long* plReference,
							int* piSemanticsStateIndex,
							char* pszSemanticsStateName, int iSemanticsStateNameSize );
/*
// returns:
//   0 on success and call-logging transaction ID in *pidTransaction
//  -1 on failure
//  -2 if call-logging transaction was already released
// remarks:
//   can only be called while processing a CLEV_MESSAGE event
//   if some details are not needed, simply pass NULL as the related parameter
*/

CLLIB_API int __cdecl
cl_ReleaseTransaction(long hDevice, long idTransaction);
/*
// returns:
//   0 on success
//  -1 on failure
//  -2 if call-logging transaction was already released
*/

CLLIB_API int __cdecl
cl_SetTransactionUsrAttr(long hDevice, long idTransaction, void* pUsrAttr);
/*
// returns:
//   0 on success
//  -1 on failure
//  -2 if call-logging transaction was already released
*/

CLLIB_API int __cdecl
cl_GetTransactionUsrAttr(long hDevice, long idTransaction, void** ppUsrAttr);
/*
// returns:
//   0 on success and pointer to call-logging transaction user attribute in *ppUsrAttr
//  -1 on failure
//  -2 if call-logging transaction was already released
*/

CLLIB_API int __cdecl
cl_GetMessage(long hDevice, long* pidMessage, CL_EVENTDATA* pclEventData);
/*
// returns:
//   0 on success and message ID in *pidMessage
//  -1 on failure
//  -2 if call-logging transaction was already released
// remarks:
//   can only be called while processing a CLEV_MESSAGE event
*/

CLLIB_API int __cdecl
cl_GetMessageDetails(	long hDevice, long* pidMessage, CL_EVENTDATA* pclEventData,
						int* piSource,
						char* pszName, int iNameSize,
						char* pszTraceText, int iTraceTextSize );
/*
// returns:
//   0 on success and message ID in *pidMessage
//  -1 on failure
//  -2 if call-logging transaction was already released
// remarks:
//   can only be called while processing a CLEV_MESSAGE event
//   if some details are not needed, simply pass NULL as the related parameter
*/

CLLIB_API int __cdecl
cl_GetVariable(	long hDevice, CL_EVENTDATA* pclEventData, const char* pszVariableName,
				char* pszVariable, int iVariableSize );
/*
// returns:
//   0 on success and variable specified by pszVariableName in pszVariable
//  -1 on failure
//  -2 if call-logging transaction was already released
// remarks:
//   can only be called while processing a CLEV_MESSAGE event
*/

CLLIB_API int __cdecl
cl_GetCalling(	long hDevice, CL_EVENTDATA* pclEventData,
				char* pszCalling, int iCallingSize );
/*
// returns:
//   0 on success and calling-party number in pszCalling
//  -1 on failure
//  -2 if call-logging transaction was already released
// remarks:
//   can only be called while processing a CLEV_MESSAGE event
*/

CLLIB_API int __cdecl
cl_GetCalled(	long hDevice, CL_EVENTDATA* pclEventData,
				char* pszCalled, int iCalledSize );
/*
// returns:
//   0 on success and called-party number in pszCalled
//  -1 on failure
//  -2 if call-logging transaction was already released
// remarks:
//   can only be called while processing a CLEV_MESSAGE event
*/

CLLIB_API int __cdecl
cl_GetChannel(	long hDevice, CL_EVENTDATA* pclEventData,
				char* pszChannel, int iChannelSize );
/*
// returns:
//   0 on success and channel number in pszChannel
//  -1 on failure
//  -2 if call-logging transaction was already released
// remarks:
//   can only be called while processing a CLEV_MESSAGE event
*/

CLLIB_API int __cdecl
cl_GetOrdinalChannel(	long hDevice, CL_EVENTDATA* pclEventData,
						int* piOrdinalChannel );
/*
// returns:
//   0 on success and ordinal channel number in *piOrdinalChannel
//  -1 on failure
//  -2 if call-logging transaction was already released
// remarks:
//   can only be called while processing a CLEV_MESSAGE event
*/

CLLIB_API int __cdecl
cl_PeekVariable(	long hDevice, long idTransaction, const char* pszVariableName,
					char* pszVariable, int iVariableSize );
/*
// returns:
//   0 on success and variable specified by pszVariableName in pszVariable
//  -1 on failure
//  -2 if call-logging transaction was already released
*/

CLLIB_API int __cdecl
cl_PeekCalling(	long hDevice, long idTransaction,
				char* pszCalling, int iCallingSize );
/*
// returns:
//   0 on success and calling-party number in pszCalling
//  -1 on failure
//  -2 if call-logging transaction was already released
*/

CLLIB_API int __cdecl
cl_PeekCalled(	long hDevice, long idTransaction,
				char* pszCalled, int iCalledSize );
/*
// returns:
//   0 on success and called-party number in pszCalled
//  -1 on failure
//  -2 if call-logging transaction was already released
*/

CLLIB_API int __cdecl
cl_PeekChannel(	long hDevice, long idTransaction,
				char* pszChannel, int iChannelSize );
/*
// returns:
//   0 on success and channel number in pszChannel
//  -1 on failure
//  -2 if call-logging transaction was already released
*/

CLLIB_API int __cdecl
cl_PeekOrdinalChannel(	long hDevice, long idTransaction,
						int* piOrdinalChannel );
/*
// returns:
//   0 on success and ordinal channel number in *piOrdinalChannel
//  -1 on failure
//  -2 if call-logging transaction was already released
*/


#ifdef __cplusplus
}
#endif

#endif /* __CLLIB_H__ */
