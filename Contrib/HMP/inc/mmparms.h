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

#ifndef _MMPARMS_H_
#define _MMPARMS_H_

#define MM_PM_LIB      0x10000000L          /* Library Parameter */

#define MM_PM_CH       0x00000000L		    /* Channel Level Parameter */
#define MM_PM_BD       0x00800000L		    /* Board Level Parameter */

#define MM_PM_TYPE_BYTE    0x00000000L      /* Byte */
#define MM_PM_TYPE_USHORT  0x20000000L      /* Unsigned Short */
#define MM_PM_TYPE_UINT    0x40000000L      /* Unsigned Int */
#define MM_PM_TYPE_STR     0x80000000L      /* String */

#define MM_PM_STR_MAXLEN   128   /* Max Length for String Parms */

#define MM_PM_MMXXX     0x05000000L   /* MM Parameter */

#define MM_LBCPRPM_BYTE     (MM_PM_LIB | MM_PM_CH | MM_PM_MMXXX | MM_PM_TYPE_BYTE)
#define MM_LBCPRPM_USHORT   (MM_PM_LIB | MM_PM_CH | MM_PM_MMXXX | MM_PM_TYPE_USHORT)
#define MM_LBCPRPM_UINT     (MM_PM_LIB | MM_PM_CH | MM_PM_MMXXX | MM_PM_TYPE_UINT)
#define MM_LBCPRPM_STR      (MM_PM_LIB | MM_PM_CH | MM_PM_MMXXX | MM_PM_TYPE_STR)

/* Sample parameters */
typedef enum {
	/* Channel-level Parameters */	
	EMM_REC_IFRAME_TIMEOUT,
	
	/* Board-level Parameters */	
	EMM_BEEP_DURATION,
	EMM_BEEP_FREQUENCY,
	EMM_BEEP_AMPLITUDE
} eMM_PARM;

#endif /* _MMPARMS_H_ */
