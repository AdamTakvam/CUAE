/***********************************************************************
 *	  FILE: devmgmt.h
 * DESCRIPTION: Device Management library API
 *	  DATE: July 19, 2002
 *
 *   Copyright (c) 2002 Intel Corp. All Rights Reserved.
 *
 *   THIS IS UNPUBLISHED PROPRIETARY SOURCE CODE OF Intel Corp.
 *   The copyright notice above does not evidence any actual or
 *   intended publication of such source code.
 **********************************************************************/

#ifndef __DEVMGMTLIB_H__
#define __DEVMGMTLIB_H__

#include <srllib.h>
#define DEV_MAXERRMSGSIZE 80	// Maximum size of error message string

typedef enum {
   BRD_SUCCESS = 0,
   EBRD_FAILEDOPENINGDTILIB,
   EBRD_DEVICEMAPPERFAILED,
   EBRD_INVALIDPHYSICALNAME,
   EBRD_INVALIDVIRTUALNAME,
   EBRD_BUFFERTOOSMALL,
   EBRD_OUTOFMEMORY,
   EBRD_NULLPOINTERARGUMENT,
   EBRD_COMMANDNOTSUPPORTED,
   EBRD_INVALIDDEVICEHANDLE,
   EBRD_INVALIDINTERVAL,
   EBRD_INVALIDTHRESHOLD,
   EBRD_SENDALIVENOTENABLED,
   EBRD_NOTIMP,
   EBRD_IPMCONNECT_FAILED,
   EBRD_DEVBUSY,
   EBRD_INVALIDCONNTYPE,
   EBRD_IPMDISCONNECT_FAILED,
   EBRD_MAX
} T_BRDERRORVALUE;

typedef enum {
   DEV_SUCCESS = 0,
   EDEV_INVALIDDEVICEHANDLE,
   EDEV_INVALIDMODE,
   EDEV_EVENTTIMEOUT,
   EDEV_DEVICEBUSY,
   EDEV_INVALIDCONNTYPE,
   EDEV_IPM_SUBSYSTEMERR,
   EDEV_FAX_SUBSYSTEMERR,
   EDEV_IPM_INTERNALERR,
   EDEV_MM_SUBSYSTEMERR,
   EDEV_SUBSYSTEMERR,
   EDEV_INVALIDSTATE,
   EDEV_NOTCONNECTED,
   EDEV_MAX
} DEV_ERRORVALUE;

typedef struct errinfo {
	int dev_ErrValue;
	int dev_SubSystemErrValue;
	char dev_Msg[DEV_MAXERRMSGSIZE];
} DEV_ERRINFO;

typedef enum {
    DM_FULLDUP = 1,
    DM_HALFDUP
} eCONN_TYPE;

typedef enum {
    RESOURCE_TYPE_NONE,
    RESOURCE_IPM_LBR,
    RESOURCE_TYPE_MAX
} eDEV_RESOURCE_TYPE;

typedef struct getresourceinfo {
    int                 version;            // struct version
    eDEV_RESOURCE_TYPE  resourceType;       // Resource Type
    int                 curReserveCount;      // reservation count for dev
    int                 curReservePoolCount;  // current reserve pool count
    int                 maxReservePoolCount;  // max available pool count
} DEV_RESOURCE_RESERVATIONINFO;


#define BRD_FAILURE -1
#define DM3_TYPE    0x81

// dev_Connect events
#define DMEV_MASK                              0X9E00          // DML events start at 0x9E00
#define DMEV_CONNECT                           (DMEV_MASK | 0x01)
#define DMEV_CONNECT_FAIL                      (DMEV_MASK | 0x02)
#define DMEV_DISCONNECT                        (DMEV_MASK | 0x03)
#define DMEV_DISCONNECT_FAIL                   (DMEV_MASK | 0x04)
#define DMEV_RESERVE_RESOURCE                  (DMEV_MASK | 0x05)
#define DMEV_RESERVE_RESOURCE_FAIL             (DMEV_MASK | 0x06)
#define DMEV_RELEASE_RESOURCE                  (DMEV_MASK | 0x07)
#define DMEV_RELEASE_RESOURCE_FAIL             (DMEV_MASK | 0x08)
#define DMEV_GET_RESOURCE_RESERVATIONINFO      (DMEV_MASK | 0x09)
#define DMEV_GET_RESOURCE_RESERVATIONINFO_FAIL (DMEV_MASK | 0x0A)

#ifdef _WIN32


#ifdef LIBDM3DEVMGMT_EXPORTS
#define DEVMGMTLIB_API __declspec( dllexport )
#else
#define DEVMGMTLIB_API __declspec( dllimport )
#endif
#define DEVMGMT_CONV	__cdecl

#else /* !_WIN32 */

#define DEVMGMTLIB_API extern
#define DEVMGMT_CONV

#endif /* _WIN32 */

#ifdef __cplusplus
extern "C" {	// C Plus Plus function bindings
#endif

DEVMGMTLIB_API int DEVMGMT_CONV brd_Open(char *physical, long mode);
DEVMGMTLIB_API int DEVMGMT_CONV brd_VirtualToPhysicalName(char *virt, char *physical, int *len);
DEVMGMTLIB_API int DEVMGMT_CONV brd_SendAlive(int ddd, long mode);
DEVMGMTLIB_API int DEVMGMT_CONV brd_SendAliveEnable(int ddd, unsigned short interval, unsigned short threshold, long mode);
DEVMGMTLIB_API int DEVMGMT_CONV brd_SendAliveDisable(int ddd, long mode);
DEVMGMTLIB_API int DEVMGMT_CONV brd_GetAllPhysicalBoards(SRLDEVICEINFO *physicalDevs, int *count);
DEVMGMTLIB_API int DEVMGMT_CONV brd_Close(int ddd);
DEVMGMTLIB_API int DEVMGMT_CONV brd_ErrorValue( void );
DEVMGMTLIB_API char* DEVMGMT_CONV brd_ErrorMsg( void );
DEVMGMTLIB_API int DEVMGMT_CONV dev_ErrorInfo(DEV_ERRINFO *pErrInfo);
DEVMGMTLIB_API int DEVMGMT_CONV dev_Connect(int devHandle1, int devHandle2, eCONN_TYPE connType, unsigned short mode);
DEVMGMTLIB_API int DEVMGMT_CONV dev_Disconnect(int devHandle, unsigned short mode);
DEVMGMTLIB_API int DEVMGMT_CONV dev_ReserveResource(int devHandle, eDEV_RESOURCE_TYPE resType, unsigned short mode);
DEVMGMTLIB_API int DEVMGMT_CONV dev_ReleaseResource(int devHandle, eDEV_RESOURCE_TYPE resType, unsigned short mode);
DEVMGMTLIB_API int DEVMGMT_CONV dev_GetResourceReservationInfo(int devHandle, DEV_RESOURCE_RESERVATIONINFO *resInfo, unsigned short mode);

#ifdef __cplusplus
}		// C Plus Plus function bindings
#endif

#endif /* __DEVMGMTLIB_H__ */

