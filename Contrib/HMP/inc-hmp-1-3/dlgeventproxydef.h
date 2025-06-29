/**************************************************************
    Copyright (C) 2000-2002.  Intel Corporation.
 
    All Rights Reserved.  All names, products,
    and services mentioned herein are the trademarks
    or registered trademarks of their respective organizations
    and are the sole property of their respective owners.
 **************************************************************/
 
/*
*    AUTO-VERSIONING HEADER  DO NOT HAND MODIFY
*    ===================================================================
*    %name:          dlgeventproxydef.h %
*    %version:       14 %
*    %instance:      hsw_1 %
*    %created_by:    klotzw %
*    %date_modified: Wed Oct 16 16:14:38 2002 %
*    ===================================================================
*/
 
#ifndef H_DLGEVENTPROXY_DEF_H_
#define H_DLGEVENTPROXY_DEF_H_

#include <wchar.h>

/* 0 - 499 */
#define Dlg_FAIL                    0
#define Dlg_OK                      1
#define Dlg_INVALID_POINTER         2
#define Dlg_MEM_ALLOC_ERR           3

/* 5000 - 5499 */
#define DlgEvent_READY              5000             
#define DlgEvent_ERROR_INIT         5001 
#define DlgEvent_NOT_READY          5002 
#define DlgEvent_CHANNEL_ERROR      5003 

typedef void*           DlgEvent_THANDLE;
typedef void*           ClientDataType;
typedef unsigned long	DlgFilterType;
typedef unsigned char	PayloadDataType;
typedef char*           IpAddressStringType;
typedef wchar_t*        SupplierNameType;
typedef long            DlgEvent_TRESULT;

typedef struct AdminCallbackMsg
{
    unsigned long       msgId;          // Message identifier	
    wchar_t*            supplierName;   // The name of the supplier of this msg
    char*               node;           // IP address of the supplier 
    int                 payLoadLen;     // Size of actual msg
    unsigned char*      pPayLoad;       // Serialized msg
    int                 conversion;     // Use to indicate conversion is needed
    char*               description;    // Event Description
    char*               date;           // Date Event Sent
    char*               time;           // Time Event Sent
} AdminCallbackMsgType;		

typedef AdminCallbackMsgType	DlgEventMsgType;
typedef AdminCallbackMsgType*	DlgEventMsgTypePtr;
typedef DlgEvent_TRESULT (*ClientCallbackFunc)(AdminCallbackMsgType* pMsg, ClientDataType clientData) ;

typedef enum 
{
    DlgEvent_DISABLE=0,
    DlgEvent_ENABLE
} DlgEvent_FilterControl;

typedef struct ProxyFilterCallbackAssoc 
{
    ClientCallbackFunc     callback;
    void*                  clientData;
    unsigned long          filter;      //only one filter allow at this time
    DlgEvent_FilterControl enable;      // true if filter enable 
} ProxyFilterCallbackAssocType;


#define ORBInitRefArg    "-ORBInitRef"
#define EvDefPortArg      3699
#define EvChanFactoryArg "EventChannelFactory=corbaloc:iiop:%s:%d/DefaultEventChannelFactory"
#define EvServiceArg     "EventService=corbaloc:iiop:%s:%d/DefaultEventChannel"


#define DEFAULT_HOST      NULL
#define DEFAULT_PORT      0

#define DLG_CONVERT         1
#define DLG_DONT_CONVERT    0

#endif

