/**
 *
 * @file  cnferrs.h
 * @brief Conferencing API error code defnitions
 * @date  Nov. 23, 2005
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

#ifndef _CNFERRS_H_
#define _CNFERRS_H_

/*
 * Error code definitions.
 */
 
#define ECNF_NOERROR             0  ///< No error
#define ECNF_INVALID_DEVICE      1  ///< Invalid device
#define ECNF_INVALID_HANDLE      2  ///< Invalid device handle
#define ECNF_INVALID_NAME        3  ///< Invalid device name
#define ECNF_INVALID_PARM        4  ///< Invalid parameter in function call
#define ECNF_INVALID_ATTR			5	///< Invalid attribute
#define ECNF_INVALID_EVENT			6	///< Invalid event
#define ECNF_INVALID_STATE       7  ///< Invalid state for current operation
#define ECNF_SUBSYSTEM           8  ///< Internal subsystem error
#define ECNF_FIRMWARE            9  ///< Firmware error
#define ECNF_LIBRARY            10  ///< Library error
#define ECNF_SYSTEM             11  ///< System error
#define ECNF_MEMORY_ALLOC       12  ///< Memory allocation error
#define ECNF_UNSUPPORTED_API    13  ///< API not supported
#define ECNF_UNSUPPORTED_TECH   14  ///< Technology not supported
#define ECNF_UNSUPPORTED_FUNC   15  ///< Functionality not supported

#endif /* _CNFERRS_H_ */

