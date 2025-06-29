/**************************************************************
    Copyright (C) 2000-2003.  Intel Corporation.
 
    All Rights Reserved.  All names, products,
    and services mentioned herein are the trademarks
    or registered trademarks of their respective organizations
    and are the sole property of their respective owners.
 **************************************************************/
 
/*
*    AUTO-VERSIONING HEADER  DO NOT HAND MODIFY
*    ===================================================================
*    %name:          dlgsyserrors.h %
*    %version:       11 %
*    %instance:      hsw_1 %
*    %created_by:    klotzw %
*    %date_modified: Thu Mar 27 16:24:40 2003 %
*    ===================================================================
*/

#ifndef __DLG_SYS_ERROR_H__
#define __DLG_SYS_ERROR_H__

// Remove since it's not in dlgeventproxydef.h
#if 0

#define DLG_SYS_ERROR_BASE          0
#define DLG_SYS_ERROR_OFFSET        500

/* 0 - 499 */
#define DLG_GEN_MSG_ERR_BASE        DLG_SYS_ERROR_BASE
#define Dlg_FAIL                    DLG_GEN_MSG_ERR_BASE
#define Dlg_OK                      DLG_GEN_MSG_ERR_BASE + 1
#define Dlg_INVALID_POINTER         DLG_GEN_MSG_ERR_BASE + 2
#define Dlg_MEM_ALLOC_ERR           DLG_GEN_MSG_ERR_BASE + 3



/* 500 - 999 */
#define DLG_SYS_MONITOR_ERR_BASE                DLG_GEN_MSG_ERR_BASE + DLG_SYS_ERROR_OFFSET

/* 1000 - 1499 */
#define DLG_FAULT_DETECTOR_ERR_BASE             DLG_SYS_MONITOR_ERR_BASE + DLG_SYS_ERROR_OFFSET

/* 1500 - 1999 */
#define DLG_PnP_ERR_BASE                        DLG_FAULT_DETECTOR_ERR_BASE + DLG_SYS_ERROR_OFFSET

/* 2000 - 2499 */
#define DLG_DETECTOR_ERR_BASE                   DLG_PnP_ERR_BASE + DLG_SYS_ERROR_OFFSET

/* 2500 - 2999 */
#define DLG_DETECTOR_FACTORY_ERR_BASE           DLG_DETECTOR_ERR_BASE + DLG_SYS_ERROR_OFFSET

/* 3000 - 3499 */
#define DLG_DETECTOR_CONTROL_ERR_BASE           DLG_DETECTOR_FACTORY_ERR_BASE + DLG_SYS_ERROR_OFFSET

/* 3500 - 3999 */
#define DLG_SYSTEM_CONTROL_ERR_BASE             DLG_DETECTOR_CONTROL_ERR_BASE + DLG_SYS_ERROR_OFFSET

/* 4000 - 4499 */
#define DLG_DATA_MGR_ERR_BASE                   DLG_SYSTEM_CONTROL_ERR_BASE + DLG_SYS_ERROR_OFFSET

/* 4500 4999 */
#define DLG_CLOCK_DEAMON_ERR_BASE               DLG_DATA_MGR_ERR_BASE + DLG_SYS_ERROR_OFFSET


/* 5000 - 5499 */
#define DLG_EVENT_SERVER_ERR_BASE               DLG_CLOCK_DEAMON_ERR_BASE + DLG_SYS_ERROR_OFFSET
#define DlgEvent_READY                          DLG_EVENT_SERVER_ERR_BASE      //5000
#define DlgEvent_ERROR_INIT                     DLG_EVENT_SERVER_ERR_BASE + 1  //5001
#define DlgEvent_NOT_READY                      DLG_EVENT_SERVER_ERR_BASE + 2  //5002
#define DlgEvent_CHANNEL_ERROR                  DLG_EVENT_SERVER_ERR_BASE + 3  //5003



/* 5500 - 5999 */
#define DLG_TDM_BUSAGENT_ERR_BASE               DLG_EVENT_SERVER_ERR_BASE + DLG_SYS_ERROR_OFFSET

/* 6000 - 6499 */
#define DLG_INITIALIZER_SERVICE_ERR_BASE        DLG_TDM_BUSAGENT_ERR_BASE + DLG_SYS_ERROR_OFFSET

/* 6500 - 6999 */
#define DLG_INITIALIZER_FACTORY_ERR_BASE        DLG_INITIALIZER_SERVICE_ERR_BASE + DLG_SYS_ERROR_OFFSET


/* 7000 - 7499 */
#define DLG_INITIALIZER_CONTROLLER_ERR_BASE     DLG_TDM_BUSAGENT_ERR_BASE + DLG_SYS_ERROR_OFFSET

/* 7500 - 7999 */
#define DLG_TIMESLOT_MGR_ERR_BASE               DLG_INITIALIZER_CONTROLLER_ERR_BASE + DLG_SYS_ERROR_OFFSET

/* 8000 - 8499 */
#define DLG_TIMESLOT_ASSIGN_ERR_BASE            DLG_TIMESLOT_MGR_ERR_BASE + DLG_SYS_ERROR_OFFSET

/* 8500 - 8999 */
#define DLG_CLUSTER_ERR_BASE                    DLG_TIMESLOT_ASSIGN_ERR_BASE + DLG_SYS_ERROR_OFFSET

/* 9000 - 9499 */
#define DLG_DASI_ERR_BASE                       DLG_CLUSTER_ERR_BASE + DLG_SYS_ERROR_OFFSET

#endif  // if 0

#endif

