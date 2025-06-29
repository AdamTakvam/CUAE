/**
 * @file  cnflib.h
 * @brief Conferencing API (Application Programming Interface)
 * @date  Nov. 22, 2005
 *
 * INTEL CONFIDENTIAL	
 * Copyright 2005 Intel Corporation All Rights Reserved.
 * 
 * The source code contained or described herein and all documents related to 
 * the source code ("Material") are owned by Intel Corporation or its suppliers
 * or licensors.  Title to the Material remains with Intel Corporation or its 
 * suppliers and licensors.  The Material contains trade secrets and proprietary
 * and confidential information of Intel or its suppliers and licensors.  The
 * Material is protected by worldwide copyright and trade secret laws and treaty
 * provisions.  No part of the Material may be used, copied, reproduced, 
 * modified, published, uploaded, posted, transmitted, distributed, or disclosed
 * in any way without Intel's prior express written permission.
 * 
 * No license under any patent, copyright, trade secret or other intellectual 
 * property right is granted to or conferred upon you by disclosure or delivery
 * of the Materials, either expressly, by implication, inducement, estoppel or
 * otherwise.  Any license under such intellectual property rights must be
 * express and approved by Intel in writing.
 * 
 * Unless otherwise agreed by Intel in writing, you may not remove or alter this
 * notice or any other notice embedded in Materials by Intel or Intel's 
 * suppliers or licensors in any way.
 */

#ifndef _CNFLIB_H_
#define _CNFLIB_H_

#include "hrt_target.h"
#include "cnfevts.h"
#include "cnferrs.h"
#include "srllib.h"

#if ((!defined EV_SYNC) && (!defined(EV_ASYNC)))
#define EV_SYNC   0x0000
#define EV_ASYNC  0x8000
#endif

typedef int CNF_RETURN; 

#define CNF_SUCCESS  0              ///< Indicates success
#define CNF_ERROR    -1             ///< Indicates failure

#define CNF_VER(v,s) (sizeof(s) << 16 | (0xFFFF & (v)))

/**
 * @enum ECNF_ATTR_STATE
 * Enumerations used to when setting and getting of device attributes.
 */
typedef enum tagECNF_ATTR_STATE
{
   ECNF_ATTR_STATE_DISABLED = 0,
   ECNF_ATTR_STATE_ENABLED	 = 1
} ECNF_ATTR_STATE;

#define CNF_BRD_ATTR_BASE  1

/**
 * @enum ECNF_BRD_ATTR
 * Enumerated list of board device attributes.
 * @sa Refer to ECNF_ATTR_STATE for values used in setting these attributes. 
 */
typedef enum tagECNF_BRD_ATTR
{
   ECNF_BRD_ATTR_ACTIVE_TALKER	= 1,
   ECNF_BRD_ATTR_TONE_CLAMPING	= 2,
   ECNF_BRD_ATTR_NOTIFY_INTERVAL	= 3,
   ECNF_BRD_ATTR_END_OF_LIST
} ECNF_BRD_ATTR;

#define CNF_CONF_ATTR_BASE  101

/**
 * @enum ECNF_CONF_ATTR
 * Enumerated list of conference device attributes.
 * @sa Refer to ECNF_ATTR_STATE for values used in setting these attributes.
 */
typedef enum tagECNF_CONF_ATTR
{
   ECNF_CONF_ATTR_TONE_CLAMPING = 101,
   ECNF_CONF_ATTR_DTMF_MASK     = 102,
   ECNF_CONF_ATTR_NOTIFY        = 103,
   ECNF_CONF_ATTR_END_OF_LIST
} ECNF_CONF_ATTR;

#define CNF_PARTY_ATTR_BASE  201

/**
 * @enum ECNF_PARTY_ATTR
 * Enumerated list of party device attributes.
 * @sa Refer to ECNF_ATTR_STATE for values used in setting these attributes.
 */
typedef enum tagECNF_PARTY_ATTR
{
   ECNF_PARTY_ATTR_TARIFF_TONE 	= 201,
   ECNF_PARTY_ATTR_COACH			= 202,
   ECNF_PARTY_ATTR_PUPIL			= 203,
   ECNF_PARTY_ATTR_AGC				= 204,
   ECNF_PARTY_ATTR_ECHO_CANCEL	= 205,
   ECNF_PARTY_ATTR_BROADCAST		= 206,
   ECNF_PARTY_ATTR_TONE_CLAMPING	= 207,
   ECNF_PARTY_ATTR_END_OF_LIST
} ECNF_PARTY_ATTR;

#define CNF_BRD_EVT_BASE  301

/**
 * @enum ECNF_BRD_EVT
 * Enumerated list of board device notification events.
 */
typedef enum tagECNF_BRD_EVT
{
   ECNF_BRD_EVT_CONF_OPENED		= 301,	///< Enable/Disable conference opened
														///< notification event
   ECNF_BRD_EVT_CONF_CLOSED		= 302,   ///< Enable/Disable conference closed
														///< notification event
   ECNF_BRD_EVT_PARTY_ADDED		= 303,   ///< Enalbe/Disable party added 
														///< notification event
   ECNF_BRD_EVT_PARTY_REMOVED		= 304,   ///< Enable/Disable party removed
														///< notification event
   ECNF_BRD_EVT_ACTIVE_TALKER    = 305		///< Enable/Disable active talker 
														///< notification event
} ECNF_BRD_EVT;

#define CNF_CONF_EVT_BASE  401

/**
 * @enum ECNF_CONF_EVT
 * Enumerated list of conference device events.
 */
typedef enum tagECNF_CONF_EVT
{
   ECNF_CONF_EVT_PARTY_ADDED		= 401,   ///< Enable/Disable party added
														///< notification event
   ECNF_CONF_EVT_PARTY_REMOVED	= 402,   ///< Enable/Disable party removed
														///< notification event
   ECNF_CONF_EVT_DTMF_DETECTION	= 403,   ///< Enable/Disable DTMF detected 
														///< notification event
	ECNF_CONF_EVT_ACTIVE_TALKER	= 404		///< Enalbe/Disable active talker
														///< notification event
} ECNF_CONF_EVT;

/**
 * @enum ECNF_DTMF_DIGIT
 * @brief Enumerated list of DTMF digits.
 * 
 * These digits can be used by the cnf_SetDTMFControl API and can also be
 * logically OR'ed with the ECNF_DTMF_MASK_OP in order to set the 
 * ECNF_CONF_ATTR_DTMF_MASK attribute.
 */
typedef enum tagECNF_DTMF_DIGIT
{
   ECNF_DTMF_DIGIT_1             = 0x0001,     ///< DTMF 1
   ECNF_DTMF_DIGIT_2             = 0x0002,     ///< DTMF 2
   ECNF_DTMF_DIGIT_3             = 0x0004,     ///< DTMF 3
   ECNF_DTMF_DIGIT_4				   = 0x0008,     ///< DTMF 4
   ECNF_DTMF_DIGIT_5             = 0x0010,     ///< DTMF 5
   ECNF_DTMF_DIGIT_6             = 0x0020,     ///< DTMF 6
   ECNF_DTMF_DIGIT_7             = 0x0040,     ///< DTMF 7
   ECNF_DTMF_DIGIT_8             = 0x0080,     ///< DTMF 8
   ECNF_DTMF_DIGIT_9             = 0x0100,     ///< DTMF 9
   ECNF_DTMF_DIGIT_0             = 0x0200,     ///< DTMF 0
   ECNF_DTMF_DIGIT_STAR          = 0x0400,     ///< DTMF *
   ECNF_DTMF_DIGIT_POUND         = 0x0800,     ///< DTMF #
   ECNF_DTMF_DIGIT_A             = 0x1000,     ///< DTMF A
   ECNF_DTMF_DIGIT_B             = 0x2000,     ///< DTMF B
   ECNF_DTMF_DIGIT_C             = 0x4000,     ///< DTMF C
   ECNF_DTMF_DIGIT_D             = 0x8000      ///< DTMF D
} ECNF_DTMF_DIGIT;

/**
 * @enum ECNF_DTMF_MASK_OP
 * @brief Enumerated list of DTMF MASK operations.
 * These operations can be logically OR's with the ECNF_DTMF_DIGIT values in
 * in order to add, remove, clear, or set the ECNF_CONF_ATTR_DTMF_MASK attribute.
 */
typedef enum tagECNF_DTMF_MASK_OP
{
   ECNF_DTMF_MASK_OP_CLEAR       = 0x00000000, ///< Clear all digits operation.
   ECNF_DTMF_MASK_OP_ADD         = 0x00100000, ///< Add digits operation.
   ECNF_DTMF_MASK_OP_REMOVE      = 0x00200000, ///< Remove digits operation.
   ECNF_DTMF_MASK_OP_SET         = 0x00400000  ///< Set digits operation.
} ECNF_DTMF_MASK_OP;

/**
 * @struct CNF_OPEN_INFO
 * Open information structure. Reserved for future use.
 */
typedef struct tagCNF_OPEN_INFO
{
   unsigned int unVersion;          ///< Structure version
} CNF_OPEN_INFO, *PCNF_OPEN_INFO;
typedef const CNF_OPEN_INFO * CPCNF_OPEN_INFO;

#define CNF_OPEN_INFO_VERSION_0 CNF_VER(0, CNF_OPEN_INFO)

/**
 * @struct CNF_CLOSE_INFO
 * Close information structure. Reserved for future use.
 */
typedef struct tagCNF_CLOSE_INFO
{
   unsigned int unVersion;          ///< Structure version
} CNF_CLOSE_INFO, *PCNF_CLOSE_INFO;
typedef const CNF_CLOSE_INFO * CPCNF_CLOSE_INFO;

#define CNF_CLOSE_INFO_VERSION_0 CNF_VER(0, CNF_CLOSE_INFO)

/**
 * @struct CNF_OPEN_CONF_INFO
 * Open conference information structure. Reserved for future use.
 */
typedef struct tagCNF_OPEN_CONF_INFO
{
   unsigned int unVersion;          ///< Structure version
} CNF_OPEN_CONF_INFO, *PCNF_OPEN_CONF_INFO;
typedef const CNF_OPEN_CONF_INFO * CPCNF_OPEN_CONF_INFO;

#define CNF_OPEN_CONF_INFO_VERSION_0 CNF_VER(0, CNF_OPEN_CONF_INFO)

/**
 * @struct CNF_CLOSE_CONF_INFO
 * Close conference information structure. Reserved for future use.
 */
typedef struct tagCNF_CLOSE_CONF_INFO
{
   unsigned int unVersion;          ///< Structure version
} CNF_CLOSE_CONF_INFO, *PCNF_CLOSE_CONF_INFO;
typedef const CNF_CLOSE_CONF_INFO * CPCNF_CLOSE_CONF_INFO;

#define CNF_CLOSE_CONF_INFO_VERSION_0 CNF_VER(0. CNF_CLOSE_CONF_INFO)

/**
 * @struct CNF_OPEN_PARTY_INFO
 * Open party information structure. Reserved for future use.
 */
typedef struct tagCNF_OPEN_PARTY_INFO
{
   unsigned int unVersion;          ///< Structure version
} CNF_OPEN_PARTY_INFO, *PCNF_OPEN_PARTY_INFO;
typedef const CNF_OPEN_PARTY_INFO * CPCNF_OPEN_PARTY_INFO;

#define CNF_OPEN_PARTY_INFO_VERSION_0 CNF_VER(0, CNF_OPEN_PARTY_INFO)

/**
 * @struct CNF_CLOSE_PARTY_INFO
 * Close party information structure. Reserved for future use.
 */
typedef struct tagCNF_CLOSE_PARTY_INFO
{
   unsigned int unVersion;          ///< Structure version
} CNF_CLOSE_PARTY_INFO, *PCNF_CLOSE_PARTY_INFO;
typedef const CNF_CLOSE_PARTY_INFO * CPCNF_CLOSE_PARTY_INFO;

#define CNF_CLOSE_PARTY_INFO_VERSION_0 CNF_VER(0, CNF_CLOSE_PARTY_INFO)

/**
 * @struct CNF_PARTY_INFO
 * Party information structure. Structure used to provide the party information
 * when adding and removing parties, and will also be returned as the data to
 * several events.
 */
typedef struct tagCNF_PARTY_INFO
{
   unsigned int        unVersion;      ///< Structure version
   unsigned int        unPartyCount;   ///< Number of party handles in list
   SRL_DEVICE_HANDLE * pPartyList;     ///< Pointer to list of party handles
} CNF_PARTY_INFO, *PCNF_PARTY_INFO;
typedef const CNF_PARTY_INFO * CPCNF_PARTY_INFO;

#define CNF_PARTY_INFO_VERSION_0 CNF_VER(0, CNF_PARTY_INFO)

/**
 * @struct CNF_ACTIVE_TALKER_INFO
 * Active talker information structure. Structure used to provide the active
 * talker information when notified of any active talker event.
 */
typedef struct tagCNF_ACTIVE_TALKER_INFO
{
   unsigned int        unVersion;      ///< Structure version
   unsigned int        unPartyCount;   ///< Number of party handles in list
   SRL_DEVICE_HANDLE * pPartyList;     ///< Pointer to list of party handles
} CNF_ACTIVE_TALKER_INFO, *PCNF_ACTIVE_TALKER_INFO;
typedef const CNF_ACTIVE_TALKER_INFO * CPCNF_ACTIVE_TALKER_INFO;

#define CNF_ACTIVE_TALKER_INFO_VERSION_0 CNF_VER(0, CNF_ACTIVE_TALKER_INFO)


/**
 * @struct CNF_ATTR
 * Attribute structure. Structure representing a attribute and its value.
 * @sa CNF_ATTR_INFO
 */
typedef struct tagCNF_ATTR
{
   unsigned int unVersion;       ///< Structure version
   unsigned int unAttribute;     ///< Attribute type
   unsigned int unValue;         ///< Attribute value
} CNF_ATTR, *PCNF_ATTR;
typedef const CNF_ATTR * CPCNF_ATTR;

#define CNF_ATTR_VERSION_0 CNF_VER(0, CNF_ATTR)

/**
 * @struct CNF_ATTR_INFO
 * Attribute information structure. Structure used to provide attribute
 * information when getting and setting attributes.
 */
typedef struct tagCNF_ATTR_INFO
{
   unsigned int unVersion;       ///< Structure version
   unsigned int unAttrCount;     ///< Number of attribute structures in list
   PCNF_ATTR    pAttrList;       ///< Pointer to attribute structure list
} CNF_ATTR_INFO, *PCNF_ATTR_INFO;
typedef const CNF_ATTR_INFO * CPCNF_ATTR_INFO;

#define CNF_ATTR_INFO_VERSION_0 CNF_VER(0, CNF_ATTR_INFO)

/**
 * @struct CNF_EVENT_INFO
 * Event information structure. Structure used to provide event information
 * when enabling and disabling events.
 */
typedef struct tagCNF_EVENT_INFO
{
   unsigned int   unVersion;        ///< Structure version
   unsigned int   unEventCount;     ///< Number of events in list
   unsigned int * punEventList;     ///< Pointer to event list
} CNF_EVENT_INFO, *PCNF_EVENT_INFO;
typedef const CNF_EVENT_INFO * CPCNF_EVENT_INFO;

#define CNF_EVENT_INFO_VERSION_0 CNF_VER(0, CNF_EVENT_INFO)

/** 
 * @struct CNF_DTMF_CONTROL_INFO
 * DTMF contol information structure. Structure used to set and get the DTMF
 * control information via the cnf_SetDTMFControl and cnf_GetDTMFControl API's.
 * @sa Refer to ECNF_ATTR_STATE, and ECNF_DTMF_DIGIT enumerations for valid 
 * values.
 */
typedef struct tagCNF_DTMF_CONTROL_INFO
{
   unsigned int    unVersion;             ///< Structure version
   ECNF_ATTR_STATE eDTMFControlState;     ///< Enable/Disable DTMF control
   ECNF_DTMF_DIGIT eVolumeUpDigit;        ///< Volume up digit
   ECNF_DTMF_DIGIT eVolumeDownDigit;      ///< Volume down digit
   ECNF_DTMF_DIGIT eVolumeResetDigit;     ///< Volume reset digit
} CNF_DTMF_CONTROL_INFO, *PCNF_DTMF_CONTROL_INFO;
typedef const CNF_DTMF_CONTROL_INFO * CPCNF_DTMF_CONTROL_INFO;

#define CNF_DTMF_CONTROL_INFO_VERSION_0 CNF_VER(0, CNF_DTMF_CONTROL_INFO)

/**
 * @struct CNF_DEVICE_COUNT_INFO
 * Device count information structure. Structure used to get the device count
 * information via the cnf_GetDeviceCount API.
 */
typedef struct tagCNF_DEVICE_COUNT_INFO
{
   unsigned int unVersion;             ///< Structure version
   unsigned int unFreePartyCount;      ///< Number of free parties
   unsigned int unMaxPartyCount;       ///< Number of maximum parties
   unsigned int unFreeConfCount;       ///< Number of free conferences
   unsigned int unMaxConfCount;        ///< Number of maximum conferences
} CNF_DEVICE_COUNT_INFO, *PCNF_DEVICE_COUNT_INFO;
typedef const CNF_DEVICE_COUNT_INFO * CPCNF_DEVICE_COUNT_INFO;

#define CNF_DEVICE_COUNT_INFO_VERSION_0 CNF_VER(0, CNF_DEVICE_COUNT_INFO)

/**
 * @struct CNF_CONF_OPENED_EVENT_INFO
 * Conference opened event information structure. Structure used to provide 
 * information when recieving the CNFEV_CONF_OPENED notification event.
 */
typedef struct tagCNF_CONF_OPENED_EVENT_INFO
{
   unsigned int      unVersion;        ///< Structure version
   SRL_DEVICE_HANDLE ConfHandle;       ///< Conference device handle
   const char *      szConfName;       ///< Conference device name
} CNF_CONF_OPENED_EVENT_INFO, *PCNF_CONF_OPENED_EVENT_INFO;
typedef const CNF_CONF_OPENED_EVENT_INFO * CPCNF_CONF_OPENED_EVENT_INFO;

#define CNF_CONF_OPENED_EVENT_INFO_VERSION_0 CNF_VER(0, CNF_CONF_OPENED_EVENT_INFO)

/**
 * @struct CNF_CONF_CLOSED_EVENT_INFO
 * Conference closed event information structure. Structure used to provide 
 * information when recieving the CNFEV_CONF_CLOSED notification event.
 */
typedef struct tagCNF_CONF_CLOSED_EVENT_INFO
{
   unsigned int unVersion;       ///< Structure version
   const char * szConfName;      ///< Conference device name
} CNF_CONF_CLOSED_EVENT_INFO, *PCNF_CONF_CLOSED_EVENT_INFO;
typedef const CNF_CONF_CLOSED_EVENT_INFO * CPCNF_CONF_CLOSED_EVENT_INFO;

#define CNF_CONF_CLOSED_EVENT_INFO_VERSION_0 CNF_VER(0, CNF_CONF_CLOSED_EVENT_INFO)

/**
 * @struct CNF_PARTY_ADDED_EVENT_INFO
 * Party added event information structure. Structure used to provide 
 * information when recieving the CNFEV_PARTY_ADDED notification event.
 */
typedef struct tagCNF_PARTY_ADDED_EVENT_INFO
{
   unsigned int      unVersion;        ///< Structure version
   SRL_DEVICE_HANDLE ConfHandle;       ///< Conference device handle
   const char *      szConfName;       ///< Conference device name
   SRL_DEVICE_HANDLE PartyHandle;      ///< Party device handle
   const char *      szPartyName;      ///< Party device name
} CNF_PARTY_ADDED_EVENT_INFO, *PCNF_PARTY_ADDED_EVENT_INFO;
typedef const CNF_PARTY_ADDED_EVENT_INFO * CPCNF_PARTY_ADDED_EVENT_INFO;

#define CNF_PARTY_ADDED_EVENT_INFO_VERSION_0 CNF_VER(0, CNF_PARTY_ADDED_EVENT_INFO)

/**
 * @struct CNF_PARTY_REMOVED_EVENT_INFO
 * Party removed event information structure. Structure used to provide 
 * information when recieving the CNFEV_PARTY_REMOVED notification event.
 */
typedef struct tagCNF_PARTY_REMOVED_EVENT_INFO
{
   unsigned int      unVersion;        ///< Structure version
   SRL_DEVICE_HANDLE ConfHandle;       ///< Conference device handle
   const char *      szConfName;       ///< Conference device name
   SRL_DEVICE_HANDLE PartyHandle;      ///< Party device handle
   const char *      szPartyName;      ///< Party device name
} CNF_PARTY_REMOVED_EVENT_INFO, *PCNF_PARTY_REMOVED_EVENT_INFO;
typedef const CNF_PARTY_REMOVED_EVENT_INFO * CPCNF_PARTY_REMOVED_EVENT_INFO;

#define CNF_PARTY_REMOVED_EVENT_INFO_VERSION_0 CNF_VER(0, CNF_PARTY_REMOVED_EVENT_INFO)

/**
 * @struct CNF_DTMF_EVENT_INFO
 * DTMF event detected information structure. Structure used to provide the 
 * information when recieving the CNFEV_DTMF_EVENT notification event.
 */
typedef struct tagCNF_DTMF_EVENT_INFO
{
   unsigned int      unVersion;     ///< Structure version
   SRL_DEVICE_HANDLE PartyHandle;   ///< Party device handle
   ECNF_DTMF_DIGIT   eDigit;        ///< Detected DTMF digit
} CNF_DTMF_EVENT_INFO, *PCNF_DTMF_EVENT_INFO;
typedef const CNF_DTMF_EVENT_INFO * CPCNF_DTMF_EVENT_INFO;

#define CNF_DTMF_EVENT_INFO_VERSION_0 CNF_VER(0, CNF_DTMF_EVENT_INFO)

/**
 * @struct CNF_ERROR_INFO
 * Error information structure. Structure used to provide error information when
 * a API call fails.
 */
typedef struct tagCNF_ERROR_INFO
{
   unsigned int unVersion;          ///< Structure version
   unsigned int unErrorCode;        ///< Error code
   const char * szErrorString;      ///< Error string
   const char * szAdditionalInfo;   ///< Additional error information string
} CNF_ERROR_INFO, *PCNF_ERROR_INFO;
typedef const CNF_ERROR_INFO * CPCNF_ERROR_INFO;

#define CNF_ERROR_INFO_VERSION_0 CNF_VER(0, CNF_ERROR_INFO)

/**
 * @struct CNF_OPEN_CONF_RESULT
 * Open conference result information structure. Structure containing result
 * information returned with the CNFEV_OPEN_CONF event.
 */
typedef struct tagCNF_OPEN_CONF_RESULT
{
   unsigned int      unVersion;     ///< Structure version
   const char *      szConfName;    ///< Conference device name
   SRL_DEVICE_HANDLE ConfHandle;    ///< Conference device handle
} CNF_OPEN_CONF_RESULT, *PCNF_OPEN_CONF_RESULT;
typedef const CNF_OPEN_CONF_RESULT * CPCNF_OPEN_CONF_RESULT;

#define CNF_OPEN_CONF_RESULT_VERSION_0 CNF_VER(0, CNF_OPEN_CONF_RESULT)

/**
 * @struct CNF_CLOSE_CONF_RESULT
 * Close conference result information structure. Structure containing result
 * information returned with the CNFEV_CLOSE_CONF event.
 */
typedef struct tagCNF_CLOSE_CONF_RESULT
{
   unsigned int unVersion;          ///< Structure version
   const char * szConfName;         ///< Conference device name
} CNF_CLOSE_CONF_RESULT, *PCNF_CLOSE_CONF_RESULT;
typedef const CNF_CLOSE_CONF_RESULT * CPCNF_CLOSE_CONF_RESULT;

#define CNF_CLOSE_CONF_RESULT_VERSION_0 CNF_VER(0, CNF_CLOSE_CONF_RESULT)

/**
 * @struct CNF_OPEN_PARTY_RESULT
 * Open party result information structure. Structure containing result
 * information returned with the CNFEV_OPEN_PARTY event.
 */
typedef struct tagCNF_OPEN_PARTY_RESULT
{
   unsigned int      unVersion;     ///< Structure version
   const char *      szPartyName;   ///< Party device name
   SRL_DEVICE_HANDLE PartyHandle;   ///< Party device handle
} CNF_OPEN_PARTY_RESULT, *PCNF_OPEN_PARTY_RESULT;
typedef const CNF_OPEN_PARTY_RESULT * CPCNF_OPEN_PARTY_RESULT;

#define CNF_OPEN_PARTY_RESULT_VERSION_0 CNF_VER(0, CNF_OPEN_PARTY_RESULT)

/**
 * @struct CNF_CLOSE_PARTY_RESULT
 * Close party result information structure. Structure containing result
 * information returned with the CNFEV_CLOSE_PARTY event.
 */
typedef struct tagCNF_CLOSE_PARTY_RESULT
{
   unsigned int unVersion;          ///< Structure version
   const char * szPartyName;        ///< Party device name
} CNF_CLOSE_PARTY_RESULT, *PCNF_CLOSE_PARTY_RESULT;
typedef const CNF_CLOSE_PARTY_RESULT * CPCNF_CLOSE_PARTY_RESULT;

#define CNF_CLOSE_PARTY_RESULT_VERSION_0 CNF_VER(0, CNF_CLOSE_PARTY_RESULT)

/**
 * @struct CNF_RESET_DEVICES_INFO
 * Reset devices information structure. Reserved for future use.
 */
typedef struct tagCNF_RESET_DEVICES_INFO
{
   unsigned int unVersion;          ///< Structure version
} CNF_RESET_DEVICES_INFO, *PCNF_RESET_DEVICES_INFO;
typedef const CNF_RESET_DEVICES_INFO * CPCNF_RESET_DEVICES_INFO;

#define CNF_RESET_DEVICES_INFO_VERSION_0 CNF_VER(0, CNF_RESET_DEVICES_INFO)

/**
 * @struct CNF_RESET_DEVICES_RESULT
 * Reset devices result structure. Reserved for future use.
 */
typedef struct tagCNF_RESET_DEVICES_RESULT
{
   unsigned int unVersion;          ///< Structure version
} CNF_RESET_DEVICES_RESULT, *PCNF_RESET_DEVICES_RESULT;
typedef const CNF_RESET_DEVICES_RESULT * CPCNF_RESET_DEVICES_RESULT;

#define CNF_RESET_DEVICES_RESULT_VERSION_0 CNF_VER(0, CNF_RESET_DEVICES_RESULT)


/*******************************************************************************
 * CNF API methods
 ******************************************************************************/
#ifdef __cplusplus
extern "C" {
#endif  /* __cplusplus */


/**
 * @brief Add one or more parties to a conference.
 * @param [in] a_CnfHandle - Conference device handle.
 * @param [in] a_pPtyInfo  - Pointer to party information structure.
 * @param [in] a_pUserInfo - Pointer to user defined data.
 * @return CNF_SUCCESS if successful, else CNF_ERROR.
 * @sa cnf_RemoveParty
 */
HRT_API
CNF_RETURN HRT_CONV cnf_AddParty (
   SRL_DEVICE_HANDLE a_CnfHandle,
   CPCNF_PARTY_INFO  a_pPtyInfo,
   void *            a_pUserInfo
   );

   
/**
 * @brief Close a board device.
 * @param [in] a_BrdHandle  - Board device handle.
 * @param [in] a_pCloseInfo - Pointer to close info structure.
 * @return CNF_SUCCESS if successful, else CNF_ERROR.
 * @sa cnf_Open
 */
HRT_API 
CNF_RETURN HRT_CONV cnf_Close (
   SRL_DEVICE_HANDLE a_BrdHandle, 
   CPCNF_CLOSE_INFO  a_pCloseInfo
   );
   
   
/**
 * @brief Close a conference device.
 * @param [in] a_CnfHandle  - Conference device handle.
 * @param [in] a_pCloseInfo - Pointer to close conference information structure.
 * @return CNF_SUCCESS if successful, else CNF_ERROR.
 * @sa cnf_OpenConference
 */
HRT_API 
CNF_RETURN HRT_CONV cnf_CloseConference (
   SRL_DEVICE_HANDLE     a_CnfHandle,
   CPCNF_CLOSE_CONF_INFO a_pCloseInfo
   );
   
   
/**
 * @brief Close specified party device.
 * @param [in] a_PtyHandle  - Party device handle.
 * @param [in] a_pCloseInfo - Pointer to close party information structure.
 * @return CNF_SUCCESS if successful, else CNF_ERROR.
 * @sa cnf_OpenParty
 */
HRT_API
CNF_RETURN HRT_CONV cnf_CloseParty (
   SRL_DEVICE_HANDLE      a_PtyHandle,
   CPCNF_CLOSE_PARTY_INFO a_pCloseInfo
   );
   
   
/**
 * @brief Disable one or more device events.
 * @param [in] a_DevHandle  - Device handle.
 * @param [in] a_pEventInfo - Pointer to event information structure.
 * @param [in] a_pUserInfo  - Pointer to user defined data.
 * @return CNF_SUCCESS if successful, else CNF_ERROR.
 * @sa cnf_EnableEvents
 */
HRT_API
CNF_RETURN HRT_CONV cnf_DisableEvents (
   SRL_DEVICE_HANDLE a_DevHandle,
   CPCNF_EVENT_INFO  a_pEventInfo,
   void *            a_pUserInfo
   );
   
   
/**
 * @brief Enable one or more device events.
 * @param [in] a_DevHandle  - Device handle.
 * @param [in] a_pEventInfo - Pointer to event information structure.
 * @param [in] a_pUserInfo  - Pointer to user defined data.
 * @return CNF_SUCCESS if successful, else CNF_ERROR.
 * @sa cnf_DisableEvents
 */
HRT_API
CNF_RETURN HRT_CONV cnf_EnableEvents (
   SRL_DEVICE_HANDLE a_DevHandle,
   CPCNF_EVENT_INFO  a_pEventInfo,
   void *            a_pUserInfo
   );
   
   
/**
 * @brief Get list of active talkers.
 * @param [in] a_DevHandle - Device handle.
 * @param [in] a_pUserInfo - Pointer to user defined data.
 * @return CNF_SUCCESS if successful, else CNF_ERROR.
 */
HRT_API
CNF_RETURN HRT_CONV cnf_GetActiveTalkerList (
   SRL_DEVICE_HANDLE a_DevHandle,
   void *            a_pUserInfo
   );
   

/**
 * @brief Get one or more device attributes.
 * @param [in] a_DevHandle - Device handle.
 * @param [in] a_pAttrInfo - Pointer to attribute information structure.
 * @param [in] a_pUserInfo - Pointer to user defined data.
 * @return CNF_SUCCESS if successful, else CNF_ERROR.
 * @sa cnf_SetAttribute
 */
HRT_API
CNF_RETURN HRT_CONV cnf_GetAttributes (
   SRL_DEVICE_HANDLE a_DevHandle,
   CPCNF_ATTR_INFO   a_pAttrInfo,
   void *            a_pUserInfo
   );
   
   
/**
 * @brief Get device count information.
 * @param [in] a_BrdHandle - Board device handle.
 * @param [in] a_pUserInfo - Pointer to user defined data.
 * @return CNF_SUCCESS if successful, else CNF_ERROR.
 */
HRT_API
CNF_RETURN HRT_CONV cnf_GetDeviceCount (
   SRL_DEVICE_HANDLE a_BrdHandle,
   void *            a_pUserInfo
   );
   
   
/**
 * @brief Get DTMF control information.
 * @param [in] a_BrdHandle - Board device handle.
 * @param [in] a_pUserInfo - Pointer to user defined data.
 * @return CNF_SUCCESS if successful, else CNF_ERROR.
 * @sa cnf_SetDTMFControl
 */
HRT_API
CNF_RETURN HRT_CONV cnf_GetDTMFControl (
   SRL_DEVICE_HANDLE a_BrdHandle,
   void *            a_pUserInfo
   );
   
   
/**
 * @brief Get error information for the last error.
 * @param [out] a_pErrorInfo - Pointer to error information structure.
 * @return CNF_SUCCESS if successful, else CNF_ERROR.
 */
HRT_API
CNF_RETURN HRT_CONV cnf_GetErrorInfo(PCNF_ERROR_INFO a_pErrorInfo);


/**
 * @brief Get list of added parties in a conference.
 * @param [in] a_CnfHandle - Conference device handle.
 * @param [in] a_pUserInfo - Pointer to user defined data.
 * @return CNF_SUCCESS if successful, else CNF_ERROR.
 */
HRT_API
CNF_RETURN HRT_CONV cnf_GetPartyList (
   SRL_DEVICE_HANDLE a_CnfHandle,
   void *            a_pUserInfo
   );


/**
 * @brief Open a board device.
 *
 * This function will open the specified conferencing board device and return
 * the handle to the board device. However, the handle is not valid until the
 * CNFEV_OPEN event is recieved due to the fact that this function is 
 * asynchronous only.
 *
 * @param [in] a_szBrdName - Board device name.
 * @param [in] a_pOpenInfo - Pointer to open info structure.
 * @param [in] a_pUserInfo - Pointer to user defined data.
 *
 * @return Board device handle if successful, else CNF_ERROR.
 * @sa cnf_Close
 */ 
HRT_API 
SRL_DEVICE_HANDLE HRT_CONV cnf_Open (
   const char *    a_szBrdName, 
   CPCNF_OPEN_INFO a_pOpenInfo, 
   void *          a_pUserInfo
   );


/**
 * @brief Open a board device.
 *
 * This function will open the specified conferencing board device and return
 * the handle to the board device. The returned device handle will be valid only
 * if the synchronous mode is specified, if the asynchronous mode is specified,
 * the handle will not be valid until the CNFEV_OPEN event is recieved.
 *
 * @param [in] a_szBrdName - Board device name.
 * @param [in] a_pOpenInfo - Pointer to open info structure.
 * @param [in] a_pUserInfo - Pointer to user defined data.
 * @param [in] a_usMode    - Synchronous/Asynchronous mode specifier.
 *
 * @return Board device handle if successful, else CNF_ERROR.
 * @sa cnf_Close
 */ 
HRT_API 
SRL_DEVICE_HANDLE HRT_CONV cnf_OpenEx (
   const char *    a_szBrdName, 
   CPCNF_OPEN_INFO a_pOpenInfo,
   void *          a_pUserInfo,
   unsigned short  a_usMode
   );
   
   
/**
 * @brief Open a conference device.
 * @param [in] a_BrdHandle - Board device handle.
 * @param [in] a_szCnfName - Conference device name.
 * @param [in] a_pOpenInfo - Pointer to open conference information structure.
 * @param [in] a_pUserInfo - Pointer to user defined data.
 * @return Conference device handle if successful, else CNF_ERROR.
 * @sa cnf_CloseConference
 */
HRT_API 
SRL_DEVICE_HANDLE HRT_CONV cnf_OpenConference (
   SRL_DEVICE_HANDLE    a_BrdHandle,
   const char *         a_szCnfName,
   CPCNF_OPEN_CONF_INFO a_pOpenInfo,
   void *               a_pUserInfo
   );

   
/**
 * @brief Open a party device.
 * @param [in] a_BrdHandle - Board device handle.
 * @param [in] a_szPtyName - Party device name.
 * @param [in] a_pOpenInfo - Pointer to open party information structure.
 * @param [in] a_pUserInfo - Pointer to user defined data.
 * @return Party device handle if successful, else CNF_ERROR.
 * @sa cnf_CloseParty
 */
HRT_API
SRL_DEVICE_HANDLE HRT_CONV cnf_OpenParty (
   SRL_DEVICE_HANDLE     a_BrdHandle,
   const char *          a_szPtyName,
   CPCNF_OPEN_PARTY_INFO a_pOpenInfo,
   void *                a_pUserInfo
   );
   
  
/**
 * @brief Remove one or more parties from a conference.
 * @param [in] a_CnfHandle - Conference device handle.
 * @param [in] a_pPtyInfo  - Pointer to party information structure.
 * @param [in] a_pUserInfo - Pointer to user defined data.
 * @return CNF_SUCCESS if successful, else CNF_ERROR.
 * @sa cnf_AddParty
 */
HRT_API
CNF_RETURN HRT_CONV cnf_RemoveParty (
   SRL_DEVICE_HANDLE a_CnfHandle,
   CPCNF_PARTY_INFO  a_pPtyInfo,
   void *            a_pUserInfo
   );


/**
 * @brief Reset all devices on specified board device.
 *
 * This function is used to reset all board devices which may have been opened
 * and not closed by a previous process. This function is mainly used to recover
 * board devices after a abnormal shutdown and should not be used otherwise.  
 *
 * @param [in] a_BrdHandle  - Board device handle.
 * @param [in] a_pResetInfo - Pointer to reset information structure.
 * @param [in] a_pUserInfo  - Pointer to user defined data.
 * @return CNF_SUCCESS if successful, else CNF_ERROR.
 */
HRT_API
CNF_RETURN HRT_CONV cnf_ResetDevices (
   SRL_DEVICE_HANDLE        a_BrdHandle,
   CPCNF_RESET_DEVICES_INFO a_pResetInfo,
   void *                   a_pUserInfo
   );
   
   
/**
 * @brief Set one or more device attributes.
 * @param [in] a_DevHandle - Device handle.
 * @param [in] a_pAttrInfo - Pointer to attribute information structure.
 * @param [in] a_pUserInfo - Pointer to user defined data.
 * @return CNF_SUCCESS if successful, else CNF_ERROR.
 * @sa cnf_GetAttributes
 */
HRT_API
CNF_RETURN HRT_CONV cnf_SetAttributes (
   SRL_DEVICE_HANDLE a_DevHandle,
   CPCNF_ATTR_INFO   a_pAttrInfo,
   void *            a_pUserInfo
   );
   
   
/**
 * @brief Set DTMF control information.
 * @param [in] a_BrdHandle - Board device handle.
 * @param [in] a_pDTMFInfo - Pointer to DTMF information structure.
 * @param [in] a_pUserInfo - Pointer to user defined data.
 * @return CNF_SUCCESS if successful, else CNF_ERROR.
 * @sa cnf_GetDTMFControl
 */
HRT_API
CNF_RETURN HRT_CONV cnf_SetDTMFControl (
   SRL_DEVICE_HANDLE       a_BrdHandle,
   CPCNF_DTMF_CONTROL_INFO a_pDTMFInfo,
   void *                  a_pUserInfo
   );
   
                                     
#ifdef __cplusplus
}
#endif  /* __cplusplus */


#endif /* _CNFLIB_H_ */

