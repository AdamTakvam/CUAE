/*
 * INTEL CONFIDENTIAL	
 * Copyright 2004 Intel Corporation All Rights Reserved.
 * 
 * The source code contained or described herein and all documents related to the
 * source code ("Material") are owned by Intel Corporation or its suppliers or
 * licensors.  Title to the Material remains with Intel Corporation or its suppliers
 * and licensors.  The Material contains trade secrets and proprietary and
 * confidential information of Intel or its suppliers and licensors.  The Material is 
 * protected by worldwide copyright and trade secret laws and treaty provisions. No
 * part of the Material may be used, copied, reproduced, modified, published,
 * uploaded, posted, transmitted, distributed, or disclosed in any way without Intel's
 * prior express written permission.
 * 
 * No license under any patent, copyright, trade secret or other intellectual property
 * right is granted to or conferred upon you by disclosure or delivery of the
 * Materials,  either expressly, by implication, inducement, estoppel or otherwise.
 * Any license under such intellectual property rights must be express and approved by
 * Intel in writing.
 * 
 * Unless otherwise agreed by Intel in writing, you may not remove or alter this notice
 * or any other notice embedded in Materials by Intel or Intel's suppliers or licensors
 * in any way.
 */

#ifndef _MMERRS_H_
#define _MMERRS_H_

/* -----------------------------------------------------------------------------
 * Error codes
 * Returned when a call fails
 * -----------------------------------------------------------------------------
 */
#define EMM_NOERROR           0     /* No Errors */
#define EMM_SYSTEM            1     /* System Error */
#define EMM_FWERROR           2     /* Firmware Error */
#define EMM_TIMEOUT           3     /* Function Timed Out */
#define EMM_RESERVED1         4     /* Reserved 1 */
#define EMM_RESERVED2         5     /* Reserved 2 */
#define EMM_BADPARM           6     /* Invalid Parameter in Function Call */
#define EMM_BADDEV            7     /* Invalid Device Descriptor */
#define EMM_BADPROD           8     /* Function Not Supported on this Board */
#define EMM_BUSY              9     /* Device is Already Busy */
#define EMM_IDLE              10    /* Device is Idle */
#define EMM_NOSUPPORT         14    /* Data format not supported */
#define EMM_NOTIMP            15    /* Function not implemented */

/* -----------------------------------------------------------------------------
 * MM function call return codes
 * -----------------------------------------------------------------------------
 */
#define EMM_SUCCESS           0     /* MM call succeeded */
#define EMM_ERROR             -1    /* MM call failed */
#define EMM_STREAM_EMPTY      -2    /* MM call failed - Stream full */
#define EMM_STREAM_FULL       -3    /* MM call failed - Stream empty */

/* -----------------------------------------------------------------------------
 * MM RetCode values
 * -----------------------------------------------------------------------------
 */
#define EMMRC_OK                       0
#define EMMRC_INVALIDARG               1
#define EMMRC_INVALID_FILEFORMAT       2
#define EMMRC_A_INVALID_STATE          3
#define EMMRC_V_INVALID_STATE          4
#define EMMRC_AV_INVALID_STATE         5
#define EMMRC_A_FILE_OPEN_FAILED       6
#define EMMRC_V_FILE_OPEN_FAILED       7
#define EMMRC_UNSUPPORTED_MODE         8
#define EMMRC_ALREADYSTOPPED           9
#define EMMRC_MEMALLOC_ERROR          10
#define EMMRC_MEMALLOC_POOLNOTFOUND   11
#define EMMRC_INVALIDSTATE_ERROR      12
#define EMMRC_FILEREAD_FAILED         13
#define EMMRC_NOT_VIDEO_FILE          14
#define EMMRC_UNKNOWN_ERROR           15
#define EMMRC_FAILED                  16

#endif /* _MMERRS_H_ */

