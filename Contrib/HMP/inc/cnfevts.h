/**
 *        
 * @file  cnfevts.h
 * @brief Conferencing API event definitions
 * @date  Nov. 17, 2005
 *
 * INTEL CONFIDENTIAL	
 * Copyright 2005 Intel Corporation All Rights Reserved.
 * 
 * The source code contained or described herein and all documents related to
 * the source code ("Material") are owned by Intel Corporation or its suppliers
 * or licensors. Title to the Material remains with Intel Corporation or its
 * suppliers and licensors. The Material contains trade secrets and proprietary
 * and confidential information of Intel or its suppliers and licensors. The
 * Material is protected by worldwide copyright and trade secret laws and treaty
 * provisions. No part of the Material may be used, copied, reproduced, modified
 * published, uploaded, posted, transmitted, distributed, or disclosed in any
 * way without Intel's prior express written permission.
 * 
 * No license under any patent, copyright, trade secret or other intellectual
 * property right is granted to or conferred upon you by disclosure or delivery
 * of the Materials, either expressly, by implication, inducement, estoppel or
 * otherwise. Any license under such intellectual property rights must be 
 * express and approved by Intel in writing.
 * 
 * Unless otherwise agreed by Intel in writing, you may not remove or alter this
 * notice or any other notice embedded in Materials by Intel or Intel's 
 * suppliers or licensors in any way.
 *
 */

#ifndef  _CNFEVTS_H_
#define  _CNFEVTS_H_


#define CNFEV_BASE             0xC000L                   ///< Start of success
                                                         ///< conference events
                                                         ///< range.

#define CNFEV_LAST_EVENT       (CNFEV_BASE | 0xFF)       ///< End of success
                                                         ///< conference events
                                                         ///< range.

#define CNFEV_ERROR_BASE       (CNFEV_BASE | 0x0100)     ///< Start of failure
                                                         ///< conference events
                                                         ///< range.

#define CNFEV_LAST_EVENT_ERROR (CNFEV_ERROR_BASE | 0xFF) ///< End of failure
                                                         ///< conference events
                                                         ///< range.

/*
 * Success event definitions
 */
 
#define CNFEV_OPEN                   (CNFEV_BASE | 0x01) ///< Board opened
#define CNFEV_OPEN_CONF              (CNFEV_BASE | 0x02) ///< Conference opened
#define CNFEV_OPEN_PARTY             (CNFEV_BASE | 0x04) ///< Party opened
#define CNFEV_ADD_PARTY              (CNFEV_BASE | 0x06) ///< Party added
#define CNFEV_REMOVE_PARTY           (CNFEV_BASE | 0x07) ///< Party removed
#define CNFEV_GET_ATTRIBUTE          (CNFEV_BASE | 0x08) ///< Got attributes
#define CNFEV_SET_ATTRIBUTE          (CNFEV_BASE | 0x09) ///< Attributes set
#define CNFEV_ENABLE_EVENT           (CNFEV_BASE | 0x0A) ///< Events enabled
#define CNFEV_DISABLE_EVENT          (CNFEV_BASE | 0x0B) ///< Events disabled
#define CNFEV_GET_DTMF_CONTROL       (CNFEV_BASE | 0x0C) ///< DTMF control retrieved
#define CNFEV_SET_DTMF_CONTROL       (CNFEV_BASE | 0x0D) ///< DTMF control set
#define CNFEV_GET_ACTIVE_TALKER      (CNFEV_BASE | 0x0E) ///< Active talkers retrieved
#define CNFEV_GET_PARTY_LIST         (CNFEV_BASE | 0x0F) ///< Party list retrieved
#define CNFEV_GET_DEVICE_COUNT       (CNFEV_BASE | 0x10) ///< Device count retrieved
#define CNFEV_RESET_DEVICES          (CNFEV_BASE | 0x11) ///< Devices reset

/*
 * notification event definitions
 */

#define CNFEV_CONF_OPENED            (CNFEV_BASE | 0x30) ///< Conference opened notification event
#define CNFEV_CONF_CLOSED            (CNFEV_BASE | 0x31) ///< Conference closed notification event
#define CNFEV_PARTY_ADDED            (CNFEV_BASE | 0x32) ///< Party added notification event
#define CNFEV_PARTY_REMOVED          (CNFEV_BASE | 0x33) ///< Party removed notification event
#define CNFEV_DTMF_DETECTED          (CNFEV_BASE | 0x34) ///< DTMF detected notification event
#define CNFEV_ACTIVE_TALKER          (CNFEV_BASE | 0x35) ///< Active talker notification event 

/*
 * Error event definitions
 */
 
#define CNFEV_ERROR                  (CNFEV_ERROR_BASE | 0x00) ///< General error
#define CNFEV_OPEN_FAIL              (CNFEV_ERROR_BASE | 0x01) ///< Open board failure
#define CNFEV_OPEN_CONF_FAIL         (CNFEV_ERROR_BASE | 0x02) ///< Open conference failure
#define CNFEV_OPEN_PARTY_FAIL        (CNFEV_ERROR_BASE | 0x04) ///< Open party failure
#define CNFEV_ADD_PARTY_FAIL         (CNFEV_ERROR_BASE | 0x06) ///< Add party failure
#define CNFEV_REMOVE_PARTY_FAIL      (CNFEV_ERROR_BASE | 0x07) ///< Remove party failure
#define CNFEV_GET_ATTRIBUTE_FAIL     (CNFEV_ERROR_BASE | 0x08) ///< Get attributes failure
#define CNFEV_SET_ATTRIBUTE_FAIL     (CNFEV_ERROR_BASE | 0x09) ///< Set attributes failure
#define CNFEV_ENABLE_EVENT_FAIL      (CNFEV_ERROR_BASE | 0x0A) ///< Enable events failure
#define CNFEV_DISABLE_EVENT_FAIL     (CNFEV_ERROR_BASE | 0x0B) ///< Disable events failure
#define CNFEV_GET_DTMF_CONTROL_FAIL  (CNFEV_ERROR_BASE | 0x0C) ///< Get DTMF control failure
#define CNFEV_SET_DTMF_CONTROL_FAIL  (CNFEV_ERROR_BASE | 0x0D) ///< Set DTMF control failure
#define CNFEV_GET_ACTIVE_TALKER_FAIL (CNFEV_ERROR_BASE | 0x0E) ///< Get active talker failure
#define CNFEV_GET_PARTY_LIST_FAIL    (CNFEV_ERROR_BASE | 0x0F) ///< Get party list failure
#define CNFEV_GET_DEVICE_COUNT_FAIL  (CNFEV_ERROR_BASE | 0x10) ///< Get device count failure
#define CNFEV_RESET_DEVICES_FAIL     (CNFEV_ERROR_BASE | 0x11) ///< Reset devices failure
 
#endif /* _CNFEVTS_H_ */
