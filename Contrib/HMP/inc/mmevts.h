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

#ifndef _MMEVTS_H_
#define _MMEVTS_H_

/* -----------------------------------------------------------------------------
 * Define MM events
 * -----------------------------------------------------------------------------
 */
 
/* Mask for all MM events */
#define MMEV_MASK                       0xA000L

/* Mask for all MM error events */
#define MMEV_ERROR_MASK                 (MMEV_MASK | 0x0800)

/*
 * Define success events
 */
#define MMEV_OPEN                       (MMEV_MASK | 0x01)  /* device opened */
#define MMEV_PLAY_ACK                   (MMEV_MASK | 0x02)  /* play initiated */
#define MMEV_RECORD_ACK                 (MMEV_MASK | 0x03)  /* record initiated */
#define MMEV_STOP_ACK                   (MMEV_MASK | 0x04)  /* stop initiated */
#define MMEV_RESET_ACK                  (MMEV_MASK | 0x05)  /* device reset initiated */
#define MMEV_ENABLEEVENTS               (MMEV_MASK | 0x06)  /* enabled notification events */
#define MMEV_DISABLEEVENTS              (MMEV_MASK | 0x07)  /* disabled notification events */
#define MMEV_PLAY                       (MMEV_MASK | 0x08)  /* play completed */
#define MMEV_RECORD                     (MMEV_MASK | 0x09)  /* record completed */
#define MMEV_RESET                      (MMEV_MASK | 0x0A)  /* device reset */
#define MMEV_GET_CHAN_STATE             (MMEV_MASK | 0x0B)  /* get channel state completed */
#define MMEV_SETPARM                    (MMEV_MASK | 0x0C)  /* parameter set */
#define MMEV_GETPARM                    (MMEV_MASK | 0x0D)  /* parameter retrieved */
#define MMEV_PLAY_VIDEO_LOWWATER        (MMEV_MASK | 0x0E)  /* video play low watermark hit */
#define MMEV_PLAY_VIDEO_HIGHWATER       (MMEV_MASK | 0x0F)  /* video play high watermark hit */
#define MMEV_PLAY_AUDIO_LOWWATER        (MMEV_MASK | 0x10)  /* audio play low watermark hit */
#define MMEV_PLAY_AUDIO_HIGHWATER       (MMEV_MASK | 0x11)  /* audio play high watermark hit */
#define MMEV_RECORD_VIDEO_LOWWATER      (MMEV_MASK | 0x12)  /* video record low watermark hit */
#define MMEV_RECORD_VIDEO_HIGHWATER     (MMEV_MASK | 0x13)  /* video record high watermark hit */
#define MMEV_RECORD_AUDIO_LOWWATER      (MMEV_MASK | 0x14)  /* audio record low watermark hit */
#define MMEV_RECORD_AUDIO_HIGHWATER     (MMEV_MASK | 0x15)  /* audio record high watermark hit */

/*
 * Define optional events
 */
#define MMEV_VIDEO_RECORD_STARTED       (MMEV_MASK | 0x30)  /* video recording started */

/* last event code (not real event) */
#define MMEV_LAST_EVENT                 (MMEV_MASK | 0x7F)

/* -----------------------------------------------------------------------------
 * General failure event
 * Indicates general failure during response processing
 * It may be anything ranging from memory errors to implementation assertions
 * May be generated as a response to any request
 * -----------------------------------------------------------------------------
 */
#define MMEV_ERROR                      (MMEV_ERROR_MASK | 0xff)

/* -----------------------------------------------------------------------------
 * Define specific failure events
 * Generated when an error occured during processing of a particular request 
 * -----------------------------------------------------------------------------
 */
#define MMEV_OPEN_FAIL                  (MMEV_ERROR_MASK | 0x01)
#define MMEV_PLAY_ACK_FAIL              (MMEV_ERROR_MASK | 0x02)
#define MMEV_RECORD_ACK_FAIL            (MMEV_ERROR_MASK | 0x03)
#define MMEV_STOP_ACK_FAIL              (MMEV_ERROR_MASK | 0x04)
#define MMEV_RESET_ACK_FAIL             (MMEV_ERROR_MASK | 0x05)
#define MMEV_ENABLEEVENTS_FAIL          (MMEV_ERROR_MASK | 0x06)
#define MMEV_DISABLEEVENTS_FAIL         (MMEV_ERROR_MASK | 0x07)
#define MMEV_PLAY_FAIL                  (MMEV_ERROR_MASK | 0x08)
#define MMEV_RECORD_FAIL                (MMEV_ERROR_MASK | 0x09)
#define MMEV_RESET_FAIL                 (MMEV_ERROR_MASK | 0x0A)
#define MMEV_GET_CHAN_STATE_FAIL        (MMEV_ERROR_MASK | 0x0B)
#define MMEV_SETPARM_FAIL               (MMEV_ERROR_MASK | 0x0C)
#define MMEV_GETPARM_FAIL               (MMEV_ERROR_MASK | 0x0D)

/*
 * Define optional failure events
 */
#define MMEV_VIDEO_RECORD_STARTED_FAIL  (MMEV_ERROR_MASK | 0x30)

/* Last error event code (not real error) */
#define MMEV_LAST_EVENT_ERROR           (MMEV_ERROR_MASK | 0x7F)

/*
 * Enable/Disable event masks
 */
#define MMR_EVENT_VIDEO_RECORD_STARTED_POSITION	0

#define MMR_EVENT_ALL (~0L)
#define MMR_EVENT_VIDEO_RECORD_STARTED (1 << MMR_EVENT_VIDEO_RECORD_STARTED_POSITION)

#endif /* _MMEVTS_H_ */
