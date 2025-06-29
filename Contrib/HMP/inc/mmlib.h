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

#ifndef _MMLIB_H_
#define _MMLIB_H_

#include "mmtarget.h"
#include "mmerrs.h"
#include "mmevts.h"
#include "mmparms.h"

#pragma pack(push, 8)

/*** General purpose item types ***/
/* Chain definitions */
typedef enum tagEMM_ITEM
{
	EMM_ITEM_CONT,
	EMM_ITEM_LINK,
	EMM_ITEM_EOT
} eMM_ITEM;

/* Return codes */
typedef struct tagMM_RET_CODE
{
	unsigned int unVersion;
	unsigned int unRetCode;
} MM_RET_CODE, *PMM_RET_CODE;
typedef const MM_RET_CODE* CPMM_RET_CODE;

#define MM_RET_CODE_VER(v) (sizeof(MM_RET_CODE) << 16 | (0xFFFF & (v)))
#define MM_RET_CODE_VERSION_0 MM_RET_CODE_VER(0)

/*******************************************************************************
*  mm_Open definitions
*******************************************************************************/
typedef struct tagMM_OPEN_INFO
{
	unsigned int unVersion;
	unsigned int unRFU;
} MM_OPEN_INFO, *PMM_OPEN_INFO;
typedef const MM_OPEN_INFO* CPMM_OPEN_INFO;

#define MM_OPEN_INFO_VER(v) (sizeof(MM_OPEN_INFO) << 16 | (0xFFFF & (v)))
#define MM_OPEN_INFO_VERSION_0 MM_OPEN_INFO_VER(0)

typedef MM_RET_CODE MM_OPEN_RESULT, *PMM_OPEN_RESULT;
typedef const MM_RET_CODE* CPMM_OPEN_RESULT;

#define MM_OPEN_RESULT_VER(v) (sizeof(MM_OPEN_RESULT) << 16 | (0xFFFF & (v)))
#define MM_OPEN_RESULT_VERSION_0 MM_OPEN_RESULT_VER(0)

/*******************************************************************************
*  mm_Close definitions
*******************************************************************************/
typedef struct tagMM_CLOSE_INFO
{
	unsigned int unVersion;
	unsigned int unRFU;
} MM_CLOSE_INFO, *PMM_CLOSE_INFO;
typedef const MM_CLOSE_INFO* CPMM_CLOSE_INFO;

#define MM_CLOSE_INFO_VER(v) (sizeof(MM_CLOSE_INFO) << 16 | (0xFFFF & (v)))
#define MM_CLOSE_INFO_VERSION_0 MM_CLOSE_INFO_VER(0)

/*******************************************************************************
*  mm_Play and mm_Record definitions
*******************************************************************************/

typedef enum tagEMM_MEDIA_TYPE
{
	EMM_MEDIA_TYPE_VIDEO,
	EMM_MEDIA_TYPE_AUDIO,
	EMM_MEDIA_TYPE_TERM
} eMM_MEDIA_TYPE;

typedef enum tagEMM_VIDEO_CODING
{
	EMM_VIDEO_CODING_UNDEFINED
	, EMM_VIDEO_CODING_DEFAULT = EMM_VIDEO_CODING_UNDEFINED
	, EMM_VIDEO_CODING_H263 = 1
	, EMM_VIDEO_CODING_H263_1998 = 2
	, EMM_VIDEO_CODING_MP4V_ES = 3
} eMM_VIDEO_CODING;

typedef enum tagEMM_VIDEO_PROFILE
{
	EMM_VIDEO_PROFILE_UNDEFINED
	, EMM_VIDEO_PROFILE_DEFAULT = EMM_VIDEO_PROFILE_UNDEFINED
	, EMM_VIDEO_PROFILE_0 = 0
	, EMM_VIDEO_PROFILE_1 = 1
	, EMM_VIDEO_PROFILE_2 = 2
	, EMM_VIDEO_PROFILE_3 = 3
	, EMM_VIDEO_PROFILE_4 = 4
	, EMM_VIDEO_PROFILE_5 = 5
	, EMM_VIDEO_PROFILE_6 = 6
	, EMM_VIDEO_PROFILE_7 = 7
	, EMM_VIDEO_PROFILE_8 = 8
	, EMM_VIDEO_PROFILE_9 = 9
} eMM_VIDEO_PROFILE;

typedef enum tagEMM_VIDEO_LEVEL
{
	EMM_VIDEO_LEVEL_UNDEFINED
	, EMM_VIDEO_LEVEL_DEFAULT = EMM_VIDEO_LEVEL_UNDEFINED
	, EMM_VIDEO_LEVEL_10 = 10
	, EMM_VIDEO_LEVEL_20 = 20
	, EMM_VIDEO_LEVEL_30 = 30
	, EMM_VIDEO_LEVEL_40 = 40
	, EMM_VIDEO_LEVEL_45 = 45
	, EMM_VIDEO_LEVEL_50 = 50
	, EMM_VIDEO_LEVEL_60 = 60
	, EMM_VIDEO_LEVEL_70 = 70
} eMM_VIDEO_LEVEL;

typedef enum tagEMM_VIDEO_IMAGE_WIDTH
{
	EMM_VIDEO_IMAGE_WIDTH_UNDEFINED = 176     /* QCIF */
	, EMM_VIDEO_IMAGE_WIDTH_DEFAULT = EMM_VIDEO_IMAGE_WIDTH_UNDEFINED
	, EMM_VIDEO_IMAGE_WIDTH_128 = 128         /* Sub-QCIF */
	, EMM_VIDEO_IMAGE_WIDTH_176 = 176         /* QCIF */
	, EMM_VIDEO_IMAGE_WIDTH_352 = 352         /* CIF */
	, EMM_VIDEO_IMAGE_WIDTH_704 = 704         /* 4CIF */
	, EMM_VIDEO_IMAGE_WIDTH_1408 = 1408       /* 16CIF */
} eMM_VIDEO_IMAGE_WIDTH;

typedef enum tagEMM_VIDEO_IMAGE_HEIGHT
{
	EMM_VIDEO_IMAGE_HEIGHT_UNDEFINED = 144   /* QCIF */
	, EMM_VIDEO_IMAGE_HEIGHT_DEFAULT = EMM_VIDEO_IMAGE_HEIGHT_UNDEFINED
	, EMM_VIDEO_IMAGE_HEIGHT_96 = 96         /* Sub-QCIF */
	, EMM_VIDEO_IMAGE_HEIGHT_144 = 144       /* QCIF */
	, EMM_VIDEO_IMAGE_HEIGHT_288 = 288       /* CIF */
	, EMM_VIDEO_IMAGE_HEIGHT_576 = 576       /* 4CIF */
	, EMM_VIDEO_IMAGE_HEIGHT_1152 = 1152     /* 16CIF */
} eMM_VIDEO_IMAGE_HEIGHT;

typedef enum tagEMM_VIDEO_BITRATE
{
	EMM_VIDEO_BITRATE_UNDEFINED
	, EMM_VIDEO_BITRATE_DEFAULT = EMM_VIDEO_BITRATE_UNDEFINED
} eMM_VIDEO_BITRATE;

typedef enum tagEMM_VIDEO_FRAMESPERSEC
{
	EMM_VIDEO_FRAMESPERSEC_UNDEFINED = 0xf0000     /* 15 fps */
	, EMM_VIDEO_FRAMESPERSEC_DEFAULT = EMM_VIDEO_FRAMESPERSEC_UNDEFINED
	, EMM_VIDEO_FRAMESPERSEC_6 = 0x60000           /* 6 fps */
	, EMM_VIDEO_FRAMESPERSEC_15 = 0xf0000          /* 15 fps */
	, EMM_VIDEO_FRAMESPERSEC_25 = 0x190000         /* 25 fps */
	, EMM_VIDEO_FRAMESPERSEC_2997 = 0x1D0061       /* 29.97 fps */
	, EMM_VIDEO_FRAMESPERSEC_30 = 0x1e0000         /* 30 fps */
} eMM_VIDEO_FRAMESPERSEC;

typedef struct tagMM_VIDEO_CODEC
{
	unsigned int unVersion;
	eMM_VIDEO_CODING Coding;
	eMM_VIDEO_PROFILE Profile;
	eMM_VIDEO_LEVEL Level;
	eMM_VIDEO_IMAGE_WIDTH ImageWidth;
	eMM_VIDEO_IMAGE_HEIGHT ImageHeight;
	eMM_VIDEO_BITRATE BitRate;
	eMM_VIDEO_FRAMESPERSEC FramesPerSec;
} MM_VIDEO_CODEC, *PMM_VIDEO_CODEC;
typedef const MM_VIDEO_CODEC* CPMM_VIDEO_CODEC;

#define MM_VIDEO_CODEC_VER(v) (sizeof(MM_VIDEO_CODEC) << 16 | (0xFFFF & (v)))
#define MM_VIDEO_CODEC_VERSION_0 MM_VIDEO_CODEC_VER(0)

typedef struct tagMM_MEDIA_ACCESS_MEMORY
{
	unsigned int  unVersion;
	unsigned char *pBuffer;
	unsigned int unBufferSize;
} MM_MEDIA_ACCESS_MEMORY, *PMM_MEDIA_ACCESS_MEMORY;
typedef const MM_MEDIA_ACCESS_MEMORY *CPMM_MEDIA_ACCESS_MEMORY;

#define MM_MEDIA_ACCESS_MEMORY_VER(v) (sizeof(MM_MEDIA_ACCESS_MEMORY) << 16 | (0xFFFF & (v)))
#define MM_MEDIA_ACCESS_MEMORY_VERSION_0 MM_MEDIA_ACCESS_MEMORY_VER(0)

typedef struct tagMM_MEDIA_ACCESS_STREAM
{
	unsigned int unVersion;
	int nStreamHandle;
} MM_MEDIA_ACCESS_STREAM, *PMM_MEDIA_ACCESS_STREAM;
typedef const MM_MEDIA_ACCESS_STREAM *CPMM_MEDIA_ACCESS_STREAM; 

#define MM_MEDIA_ACCESS_STREAM_VER(v) (sizeof(MM_MEDIA_ACCESS_STREAM) << 16 | (0xFFFF & (v)))
#define MM_MEDIA_ACCESS_STREAM_VERSION_0 MM_MEDIA_ACCESS_STREAM_VER(0)

/*
 * unMode video definitions
 */
#define MM_MODE_VID_PAUSED              0x0002
#define MM_MODE_VID_BEEPINITIATED       0x0004
#define MM_MODE_VID_NOIFRMBEEPINITIATED 0x0008

/*
 * unAccessMode definitions
 */
#define MM_MEDIA_ACCESS_MODE_FILE         0x0000
#define MM_MEDIA_ACCESS_MODE_MEMORY       0x0001
#define MM_MEDIA_ACCESS_MODE_STREAM       0x0002

typedef struct tagMM_MEDIA_VIDEO
{
	unsigned int    unVersion;
	MM_VIDEO_CODEC  codec;
	unsigned int    unMode;
	union {
		const char*     szFileName;
		MM_MEDIA_ACCESS_STREAM stream;
		MM_MEDIA_ACCESS_MEMORY memory;
	};
	unsigned int    unAccessMode;
} MM_MEDIA_VIDEO, *PMM_MEDIA_VIDEO;
typedef const MM_MEDIA_VIDEO* CPMM_MEDIA_VIDEO;

#define MM_MEDIA_VIDEO_VER(v) (sizeof(MM_MEDIA_VIDEO) << 16 | (0xFFFF & (v)))
#define MM_MEDIA_VIDEO_VERSION_0 MM_MEDIA_VIDEO_VER(0)
#define MM_MEDIA_VIDEO_VERSION_1 MM_MEDIA_VIDEO_VER(1)

/*
 * Voice Sampling rate
 */
#define MM_DRT_6KHZ           0x30  /* 6KHz (RFU) */
#define MM_DRT_8KHZ           0x40  /* 8KHz */
#define MM_DRT_11KHZ          0x58  /* 11KHz (RFU) */

/*
 * Voice Data format
 */
#define MM_DATA_FORMAT_DIALOGIC_ADPCM                  0x1   /* OKI ADPCM (RFU) */
#define MM_DATA_FORMAT_ALAW                            0x3   /* alaw PCM (RFU) */
#define MM_DATA_FORMAT_G726                            0x4   /* G.726 (RFU) */
#define MM_DATA_FORMAT_MULAW                           0x7   /* mulaw PCM (RFU) */
#define MM_DATA_FORMAT_PCM                             0x8   /* PCM */
#define MM_DATA_FORMAT_G729A                           0x0C  /* CELP Coder (RFU) */ 
#define MM_DATA_FORMAT_GSM610                          0x0D  /* Microsoft GSM (RFU) */ 
#define MM_DATA_FORMAT_GSM610_MICROSOFT                0x0D  /* Microsoft GSM (RFU) */
#define MM_DATA_FORMAT_GSM610_ETSI                     0x0E  /* ETSI Standard Framing (RFU) */
#define MM_DATA_FORMAT_GSM610_TIPHON                   0x0F  /* ETSI TIPHON Bit Order (RFU) */
#define MM_DATA_FORMAT_TRUESPEECH                      0x10  /* TRUESPEECH Coder (RFU) */
#define MM_DATA_FORMAT_RFU1                            MM_DATA_FORMAT_TRUESPEECH /* Reserved 1 (RFU) */
#define MM_DATA_FORMAT_G711_ALAW                       MM_DATA_FORMAT_ALAW /* (RFU) */
#define MM_DATA_FORMAT_G711_ALAW_8BIT_REV              0x11  /* (RFU) */
#define MM_DATA_FORMAT_G711_ALAW_16BIT_REV             0x12  /* (RFU) */
#define MM_DATA_FORMAT_G711_MULAW                      MM_DATA_FORMAT_MULAW  /* (RFU) */
#define MM_DATA_FORMAT_G711_MULAW_8BIT_REV             0x13  /* (RFU) */
#define MM_DATA_FORMAT_G711_MULAW_16BIT_REV            0x14  /* (RFU) */
#define MM_DATA_FORMAT_G721                            0x15  /* (RFU) */
#define MM_DATA_FORMAT_G721_8BIT_REV                   0x16  /* (RFU) */
#define MM_DATA_FORMAT_G721_16BIT_REV                  0x17  /* (RFU) */
#define MM_DATA_FORMAT_G721_16BIT_REV_NIBBLE_SWAP      0x18  /* (RFU) */
#define MM_DATA_FORMAT_IMA_ADPCM                       0x19  /* (RFU) */
#define MM_DATA_FORMAT_FFT                             0xFF  /* fft data (RFU) */ 

/*
 * Masked DTMF termination/initiation equates
 */
#define  MM_DM_D        0x0001    /* Mask for DTMF d. */
#define  MM_DM_1        0x0002    /* Mask for DTMF 1. */
#define  MM_DM_2        0x0004    /* Mask for DTMF 2. */
#define  MM_DM_3        0x0008    /* Mask for DTMF 3. */
#define  MM_DM_4        0x0010    /* Mask for DTMF 4. */
#define  MM_DM_5        0x0020    /* Mask for DTMF 5. */
#define  MM_DM_6        0x0040    /* Mask for DTMF 6. */
#define  MM_DM_7        0x0080    /* Mask for DTMF 7. */
#define  MM_DM_8        0x0100    /* Mask for DTMF 8. */
#define  MM_DM_9        0x0200    /* Mask for DTMF 9. */
#define  MM_DM_0        0x0400    /* Mask for DTMF 0. */
#define  MM_DM_S        0x0800    /* Mask for DTMF *. */
#define  MM_DM_P        0x1000    /* Mask for DTMF #. */
#define  MM_DM_A        0x2000    /* Mask for DTMF a. */
#define  MM_DM_B        0x4000    /* Mask for DTMF b. */
#define  MM_DM_C        0x8000    /* Mask for DTMF c. */

typedef struct tagMM_AUDIO_CODEC
{
	unsigned int unVersion;
	unsigned int unCoding;         /* see DX_XPB wDataFormat */
	unsigned int unSampleRate;     /* see DX_XPB nSamplesPerSec */
	unsigned int unBitsPerSample;  /* see DX_XPB wBitsPerSample */
} MM_AUDIO_CODEC, *PMM_AUDIO_CODEC;
typedef const MM_AUDIO_CODEC* CPMM_AUDIO_CODEC;

#define MM_AUDIO_CODEC_VER(v) (sizeof(MM_AUDIO_CODEC) << 16 | (0xFFFF & (v)))
#define MM_AUDIO_CODEC_VERSION_0 MM_AUDIO_CODEC_VER(0)

/*
 * unMode audio definitions
 */
#define MM_MODE_AUD_PAUSED              0x0002
#define MM_MODE_AUD_BEEPINITIATED       0x0004
#define MM_MODE_AUD_FILE_TYPE_VOX       0x0020
#define MM_MODE_AUD_FILE_TYPE_WAVE      0x0040
#define MM_MODE_AUD_AGC_ON              0x0080

typedef struct tagMM_MEDIA_AUDIO
{
	unsigned int   unVersion;
	MM_AUDIO_CODEC codec;
	unsigned int   unMode;
	unsigned int   unOffset;
	union {
		const char*    szFileName;
		MM_MEDIA_ACCESS_STREAM stream;
		MM_MEDIA_ACCESS_MEMORY memory;
	};
	unsigned int    unAccessMode;
	} MM_MEDIA_AUDIO, *PMM_MEDIA_AUDIO;
typedef const MM_MEDIA_AUDIO* CPMM_MEDIA_AUDIO;

#define MM_MEDIA_AUDIO_VER(v) (sizeof(MM_MEDIA_AUDIO) << 16 | (0xFFFF & (v)))
#define MM_MEDIA_AUDIO_VERSION_0 MM_MEDIA_AUDIO_VER(0)
#define MM_MEDIA_AUDIO_VERSION_1 MM_MEDIA_AUDIO_VER(1)

typedef struct tagMM_MEDIA_TERM
{
	unsigned int   unVersion;
	unsigned int   unRfu;
} MM_MEDIA_TERM, *PMM_MEDIA_TERM;
typedef const MM_MEDIA_TERM* CPMM_MEDIA_TERM;

#define MM_MEDIA_TERM_VER(v) (sizeof(MM_MEDIA_TERM) << 16 | (0xFFFF & (v)))
#define MM_MEDIA_TERM_VERSION_0 MM_MEDIA_TERM_VER(0)

typedef union tagMM_MEDIA_ITEM
{
	unsigned int      unVersion;
	MM_MEDIA_VIDEO    video;
	MM_MEDIA_AUDIO    audio;
	MM_MEDIA_TERM     term;
} MM_MEDIA_ITEM, *PMM_MEDIA_ITEM;
typedef const MM_MEDIA_ITEM* CPMM_MEDIA_ITEM;

#define MM_MEDIA_ITEM_VER(v) (sizeof(MM_MEDIA_ITEM) << 16 | (0xFFFF & (v)))
#define MM_MEDIA_ITEM_VERSION_0 MM_MEDIA_ITEM_VER(0)

/* List of media items */
typedef struct tagMM_MEDIA_ITEM_LIST
{
	unsigned int unVersion;
	eMM_ITEM ItemChain;
	MM_MEDIA_ITEM item;
	struct tagMM_MEDIA_ITEM_LIST* next;
	struct tagMM_MEDIA_ITEM_LIST* prev;	/* optional */
} MM_MEDIA_ITEM_LIST, *PMM_MEDIA_ITEM_LIST;
typedef const MM_MEDIA_ITEM_LIST* CPMM_MEDIA_ITEM_LIST;

#define MM_MEDIA_ITEM_LIST_VER(v) (sizeof(MM_MEDIA_ITEM_LIST) << 16 | (0xFFFF & (v)))
#define MM_MEDIA_ITEM_LIST_VERSION_0 MM_MEDIA_ITEM_LIST_VER(0)

typedef enum tagEMM_FILE_FORMAT
{
	EMM_FILE_FORMAT_UNDEFINED,
	EMM_FILE_FORMAT_PROPRIETARY
} eMM_FILE_FORMAT;

typedef struct tagMM_PLAY_RECORD_LIST
{
	unsigned int unVersion;
	eMM_ITEM ItemChain;
	eMM_MEDIA_TYPE ItemType;
	CPMM_MEDIA_ITEM_LIST list;
	unsigned int unRFU;
	struct tagMM_PLAY_RECORD_LIST* next;
	struct tagMM_PLAY_RECORD_LIST* prev;	/* optional */
} MM_PLAY_RECORD_LIST, *PMM_PLAY_RECORD_LIST;
typedef const MM_PLAY_RECORD_LIST* CPMM_PLAY_RECORD_LIST;

#define MM_PLAY_RECORD_LIST_VER(v) (sizeof(MM_PLAY_RECORD_LIST) << 16 | (0xFFFF & (v)))
#define MM_PLAY_RECORD_LIST_VERSION_0 MM_PLAY_RECORD_LIST_VER(0)

typedef struct tagMM_PLAY_RECORD_INFO
{
	unsigned int unVersion;
	eMM_FILE_FORMAT eFileFormat;
	CPMM_PLAY_RECORD_LIST list;
} MM_PLAY_RECORD_INFO, *PMM_PLAY_RECORD_INFO;
typedef const MM_PLAY_RECORD_INFO* CPMM_PLAY_RECORD_INFO;

#define MM_PLAY_RECORD_INFO_VER(v) (sizeof(MM_PLAY_RECORD_INFO) << 16 | (0xFFFF & (v)))
#define MM_PLAY_RECORD_INFO_VERSION_0 MM_PLAY_RECORD_INFO_VER(0)

typedef MM_PLAY_RECORD_INFO MM_PLAY_INFO, *PMM_PLAY_INFO;
typedef CPMM_PLAY_RECORD_INFO CPMM_PLAY_INFO;

#define MM_PLAY_INFO_VER(v) MM_PLAY_RECORD_INFO_VER(v)
#define MM_PLAY_INFO_VERSION_0 MM_PLAY_RECORD_INFO_VERSION_0

typedef MM_PLAY_RECORD_INFO MM_RECORD_INFO, *PMM_RECORD_INFO;
typedef CPMM_PLAY_RECORD_INFO CPMM_RECORD_INFO;

#define MM_RECORD_INFO_VER(v) MM_PLAY_RECORD_INFO_VER(v)
#define MM_RECORD_INFO_VERSION_0 MM_PLAY_RECORD_INFO_VERSION_0

typedef enum tagEMM_TERMINATION_REASON
{
	EMM_TERM_NORTC,
	EMM_TERM_DIGMASK,
	EMM_TERM_DIGTYPE,
	EMM_TERM_MAXDTMF,
	EMM_TERM_MAXTIME,
	EMM_TERM_UNDEFINED = 0xff
} eMM_TERMINATION_REASON;

typedef enum tagEMM_TERMINATION_ACTION
{
	EMM_TA_AUDIO_STOP,
	EMM_TA_VIDEO_STOP,
	EMM_TA_AUDIO_VIDEO_STOP,
	EMM_TA_UNDEFINED = 0xff
} eMM_TERMINATION_ACTION;

typedef struct tagMM_RUNTIME_CONTROL
{
	unsigned int unVersion;
	eMM_TERMINATION_REASON Reason; 
	unsigned int unValue; 
	eMM_TERMINATION_ACTION Action; 
	struct tagMM_RUNTIME_CONTROL *next;
} MM_RUNTIME_CONTROL, *PMM_RUNTIME_CONTROL;
typedef const MM_RUNTIME_CONTROL* CPMM_RUNTIME_CONTROL;

#define MM_RUNTIME_CONTROL_VER(v) (sizeof(MM_RUNTIME_CONTROL) << 16 | (0xFFFF & (v)))
#define MM_RUNTIME_CONTROL_VERSION_0 MM_RUNTIME_CONTROL_VER(0)
#define MM_RUNTIME_CONTROL_VERSION_1 MM_RUNTIME_CONTROL_VER(1)

typedef MM_RET_CODE MM_PLAY_ACK, *PMM_PLAY_ACK;
typedef const MM_RET_CODE* CPMM_PLAY_ACK;

#define MM_PLAY_ACK_VER(v) (sizeof(MM_PLAY_ACK) << 16 | (0xFFFF & (v)))
#define MM_PLAY_ACK_VERSION_0 MM_PLAY_ACK_VER(0)

typedef MM_RET_CODE MM_RECORD_ACK, *PMM_RECORD_ACK;
typedef const MM_RET_CODE* CPMM_RECORD_ACK;

#define MM_RECORD_ACK_VER(v) (sizeof(MM_RECORD_ACK) << 16 | (0xFFFF & (v)))
#define MM_RECORD_ACK_VERSION_0 MM_RECORD_ACK_VER(0)

/*
 * MM_VIDEO_RECORD_STARTED::unStatus
 */
#define EMM_VIDEO_RCRD_IFRAME_DETECTED 0x1
#define EMM_VIDEO_RCRD_IFRAME_TIMEOUT 0x2

typedef struct tagMM_VIDEO_RECORD_STARTED
{
	unsigned int unVersion;
	unsigned int unStatus;
} MM_VIDEO_RECORD_STARTED, *PMM_VIDEO_RECORD_STARTED;
typedef const PMM_VIDEO_RECORD_STARTED* CPMM_VIDEO_RECORD_STARTED;

#define MM_VIDEO_RECORD_STARTED_VER(v) (sizeof(MM_VIDEO_RECORD_STARTED) << 16 | (0xFFFF & (v)))
#define MM_VIDEO_RECORD_STARTED_VERSION_0 MM_VIDEO_RECORD_STARTED_VER(0)

typedef enum tagEMM_CMPLT_PLAY_RECORD
{
	EMM_CMPLT_VIDEO_PLAY,
	EMM_CMPLT_VIDEO_RECORD,
	EMM_CMPLT_AUDIO_PLAY,
	EMM_CMPLT_AUDIO_RECORD,
	EMM_CMPLT_TONE,
	EMM_CMPLT_GET_DIGIT,
	EMM_CMPLT_UNDEFINED = 0xff
} eMM_CMPLT_PLAY_RECORD;

typedef enum tagEMM_CMPLT_PLAY_RECORD_REASON
{
	EMM_TR_MAX_DTMF_DIGIT,
	EMM_TR_CONT_SILON,
	EMM_TR_CONT_SILOFF,
	EMM_TR_ID_DELAY,
	EMM_TR_EOF,
	EMM_TR_DIGIT,
	EMM_TR_PATTERN_SILON_SILOFF,
	EMM_TR_USERSTOP,
	EMM_TR_TONEID,
	EMM_TR_ERROR,
	EMM_TR_DIGMASK,
	EMM_TR_DIGTYPE,
	EMM_TR_MAXTIME,
	EMM_TR_UNDEFINED = 0xff
} eMM_CMPLT_PLAY_RECORD_REASON;

typedef enum tagEMM_CMPLT_PLAY_RECORD_STATUS
{
    EMM_STATUS_SUCCESS,
    EMM_STATUS_RCRD_V_DRPD_FRAME_FULL_ERROR,
    EMM_STATUS_RCRD_V_PKTS_DROPD_FS_GT_MFS,
    EMM_STATUS_RCRD_A_DRPD_FRAME_FULL_ERROR,
    EMM_STATUS_PLAY_V_ERROR_FS_GT_MFS,
    EMM_STATUS_PLAY_A_FILEREAD_ERROR,
    EMM_STATUS_PLAY_V_FILEREAD_ERROR,
    EMM_STATUS_UNDEFINED = 0xff
} eMM_CMPLT_PLAY_RECORD_STATUS;

typedef struct tagMM_PLAY_RECORD_CMPLT_DETAILS
{
	unsigned int unVersion;
	eMM_CMPLT_PLAY_RECORD Complete;
	eMM_CMPLT_PLAY_RECORD_REASON Reason;
	unsigned int unDuration;
	unsigned int unNumberOfBytes;
	eMM_CMPLT_PLAY_RECORD_STATUS Status;
} MM_PLAY_RECORD_CMPLT_DETAILS, *PMM_PLAY_RECORD_CMPLT_DETAILS;
typedef const MM_PLAY_RECORD_CMPLT_DETAILS* CPMM_PLAY_RECORD_CMPLT_DETAILS;

#define MM_PLAY_RECORD_CMPLT_DETAILS_VER(v) (sizeof(MM_PLAY_RECORD_CMPLT_DETAILS) << 16 | (0xFFFF & (v)))
#define MM_PLAY_RECORD_CMPLT_DETAILS_VERSION_0 MM_PLAY_RECORD_CMPLT_DETAILS_VER(0)

#define MAX_PLAY_RECORD_CMPLT 16
typedef struct tagMM_PLAY_RECORD_CMPLT
{
	unsigned int unVersion;
	unsigned int unCount;
	MM_PLAY_RECORD_CMPLT_DETAILS details[MAX_PLAY_RECORD_CMPLT];
} MM_PLAY_RECORD_CMPLT, *PMM_PLAY_RECORD_CMPLT;
typedef const MM_PLAY_RECORD_CMPLT* CPMM_PLAY_RECORD_CMPLT;

#define MM_PLAY_RECORD_CMPLT_VER(v) (sizeof(MM_PLAY_RECORD_CMPLT) << 16 | (0xFFFF & (v)))
#define MM_PLAY_RECORD_CMPLT_VERSION_0 MM_PLAY_RECORD_CMPLT_VER(0)

typedef MM_PLAY_RECORD_CMPLT MM_PLAY_CMPLT, *PMM_PLAY_CMPLT;
typedef CPMM_PLAY_RECORD_CMPLT CPMM_PLAY_CMPLT;

typedef MM_PLAY_RECORD_CMPLT MM_RECORD_CMPLT, *PMM_RECORD_CMPLT;
typedef CPMM_PLAY_RECORD_CMPLT CPMM_RECORD_CMPLT;

/*******************************************************************************
*  mm_GetParm definitions
*******************************************************************************/
typedef struct tagMM_GET_PARM
{
	unsigned int unVersion;
	eMM_PARM eParm;
} MM_GET_PARM, *PMM_GET_PARM;
typedef const MM_GET_PARM* CPMM_GET_PARM;

#define MM_GET_PARM_VER(v) (sizeof(MM_GET_PARM) << 16 | (0xFFFF & (v)))
#define MM_GET_PARM_VERSION_0 MM_GET_PARM_VER(0)

typedef struct tagMM_GET_PARM_RESULT
{
	unsigned int unVersion;
	eMM_PARM eParm;
	unsigned int unParmValue;
} MM_GET_PARM_RESULT, *PMM_GET_PARM_RESULT;
typedef const MM_GET_PARM_RESULT* CPMM_GET_PARM_RESULT;

#define MM_GET_PARM_RESULT_VER(v) (sizeof(MM_GET_PARM_RESULT) << 16 | (0xFFFF & (v)))
#define MM_GET_PARM_RESULT_VERSION_0 MM_GET_PARM_RESULT_VER(0)

/*******************************************************************************
*  mm_SetParm definitions
*******************************************************************************/
typedef struct tagMM_SET_PARM
{
	unsigned int unVersion;
	eMM_PARM eParm;
	unsigned int unParmValue;
} MM_SET_PARM, *PMM_SET_PARM;
typedef const MM_SET_PARM* CPMM_SET_PARM;

#define MM_SET_PARM_VER(v) (sizeof(MM_SET_PARM) << 16 | (0xFFFF & (v)))
#define MM_SET_PARM_VERSION_0 MM_SET_PARM_VER(0)

typedef MM_RET_CODE MM_SET_PARM_RESULT, *PMM_SET_PARM_RESULT;
typedef const MM_RET_CODE* CPMM_SET_PARM_RESULT;

#define MM_SET_PARM_RESULT_VER(v) (sizeof(MM_SET_PARM_RESULT) << 16 | (0xFFFF & (v)))
#define MM_SET_PARM_RESULT_VERSION_0 MM_SET_PARM_RESULT_VER(0)

/*******************************************************************************
*  mm_GetChanState definitions
*******************************************************************************/
typedef struct tagMM_GET_CHAN_STATE
{
	unsigned int unVersion;
	unsigned int unRFU;
} MM_GET_CHAN_STATE, PMM_GET_CHAN_STATE;
typedef const MM_GET_CHAN_STATE* CPMM_GET_CHAN_STATE;

#define MM_GET_CHAN_STATE_VER(v) (sizeof(MM_GET_CHAN_STATE) << 16 | (0xFFFF & (v)))
#define MM_GET_CHAN_STATE_VERSION_0 MM_GET_CHAN_STATE_VER(0)

typedef enum tagEMM_CHAN_STATE
{
	EMM_STATE_IDLE,
	/* todo: define */
	EMM_STATE_UNDEFINED = 0xff
} eMM_CHAN_STATE;

typedef struct tagMM_CHAN_STATE
{
	unsigned int unVersion;
	eMM_CHAN_STATE ChanState;
} MM_CHAN_STATE, *PMM_CHAN_STATE;
typedef const MM_CHAN_STATE* CPMM_CHAN_STATE;

#define MM_CHAN_STATE_VER(v) (sizeof(MM_CHAN_STATE) << 16 | (0xFFFF & (v)))
#define MM_CHAN_STATE_VERSION_0 MM_CHAN_STATE_VER(0)

/*******************************************************************************
*  mm_Reset definitions
*******************************************************************************/
typedef struct tagMM_RESET
{
	unsigned int unVersion;
	unsigned int unRFU;
} MM_RESET, *PMM_RESET;
typedef const MM_RESET* CPMM_RESET;

#define MM_RESET_VER(v) (sizeof(MM_RESET) << 16 | (0xFFFF & (v)))
#define MM_RESET_VERSION_0 MM_RESET_VER(0)

typedef MM_RET_CODE MM_RESET_ACK, *PMM_RESET_ACK;
typedef const MM_RESET_ACK* CPMM_RESET_ACK;

#define MM_RESET_ACK_VER(v) (sizeof(MM_RESET_ACK) << 16 | (0xFFFF & (v)))
#define MM_RESET_ACK_VERSION_0 MM_RESET_ACK_VER(0)

typedef MM_RET_CODE MM_RESET_RESULT, *PMM_RESET_RESULT;
typedef const MM_RESET_RESULT* CPMM_RESET_RESULT;

#define MM_RESET_RESULT_VER(v) (sizeof(MM_RESET_RESULT) << 16 | (0xFFFF & (v)))
#define MM_RESET_RESULT_VERSION_0 MM_RESET_RESULT_VER(0)

/*******************************************************************************
*  mm_EnableEvents amd mm_DisableEvents definitions
*******************************************************************************/
typedef struct tagMM_EVENTS
{
	unsigned int unVersion;
	unsigned int unMask;
} MM_EVENTS, *PMM_EVENTS;
typedef const MM_EVENTS* CPMM_EVENTS;

#define MM_EVENTS_VER(v) (sizeof(MM_EVENTS) << 16 | (0xFFFF & (v)))
#define MM_EVENTS_VERSION_0 MM_EVENTS_VER(0)

typedef MM_RET_CODE MM_ENABLE_EVENTS_RESULT, *PMM_ENABLE_EVENTS_RESULT;
typedef const MM_RET_CODE* CPMM_ENABLE_EVENTS_RESULT;

typedef MM_RET_CODE MM_DISABLE_EVENTS_RESULT, *PMM_DISABLE_EVENTS_RESULT;
typedef const MM_RET_CODE* CPMM_DISABLE_EVENTS_RESULT;

/*******************************************************************************
*  mm_Stop definitions
*******************************************************************************/
typedef enum tagEMM_STOP
{
	EMM_STOP_VIDEO_PLAY,
	EMM_STOP_VIDEO_RECORD,
	EMM_STOP_AUDIO_PLAY,
	EMM_STOP_AUDIO_RECORD,
	EMM_STOP_AUDIO_PLAYTONE,
	EMM_STOP_GETDIGIT,
	EMM_STOP_UNDEFINED = 0xff
} eMM_STOP;

typedef struct tagMM_STOP_DETAILS
{
	unsigned int unVersion;
	unsigned int unRfu;
} MM_STOP_DETAILS, *PMM_STOP_DETAILS;
typedef const MM_STOP_DETAILS* CPMM_STOP_DETAILS;

#define MM_STOP_DETAILS_VER(v) (sizeof(MM_STOP_DETAILS) << 16 | (0xFFFF & (v)))
#define MM_STOP_DETAILS_VERSION_0 MM_STOP_DETAILS_VER(0)

typedef struct tagMM_STOP
{
	unsigned int unVersion;
	eMM_ITEM ItemChain;
	eMM_STOP ItemType;
	MM_STOP_DETAILS details;
	struct tagMM_STOP* next;
	struct tagMM_STOP* prev;	/* optional */
} MM_STOP, *PMM_STOP;
typedef const MM_STOP* CPMM_STOP;

#define MM_STOP_VER(v) (sizeof(MM_STOP) << 16 | (0xFFFF & (v)))
#define MM_STOP_VERSION_0 MM_STOP_VER(0)

typedef struct tagMM_STOP_ACK_DETAILS
{
	unsigned int unVersion;
	eMM_STOP ItemType;
	unsigned int unRetCode;
} MM_STOP_ACK_DETAILS, *PMM_STOP_ACK_DETAILS;
typedef const MM_STOP_ACK_DETAILS* CPMM_STOP_ACK_DETAILS;

#define MM_STOP_ACK_DETAILS_VER(v) (sizeof(MM_STOP_ACK_DETAILS) << 16 | (0xFFFF & (v)))
#define MM_STOP_ACK_DETAILS_VERSION_0 MM_STOP_ACK_DETAILS_VER(0)

#define MAX_STOP_ACK 16
typedef struct tagMM_STOP_ACK
{
	unsigned int unVersion;
	unsigned int unCount;
	MM_STOP_ACK_DETAILS details[MAX_STOP_ACK];
} MM_STOP_ACK, *PMM_STOP_ACK;
typedef const MM_STOP_ACK* CPMM_STOP_ACK;

#define MM_STOP_ACK_VER(v) (sizeof(MM_STOP_ACK) << 16 | (0xFFFF & (v)))
#define MM_STOP_ACK_VERSION_0 MM_STOP_ACK_VER(0)

/*******************************************************************************
*  mm_StreamOpen definitions
*******************************************************************************/
typedef enum tagEMM_STREAM_MODE
{
	EMM_SM_READ,
	EMM_SM_WRITE,
	EMM_SM_UNDEFINED = 0xff
} eMM_STREAM_MODE;

typedef struct tagMM_STREAM_OPEN_INFO
{
	unsigned int unVersion;
	unsigned int unBufferSize;
	eMM_STREAM_MODE BufferMode; // EMM_SM_READ or EMM_SM_WRITE
} MM_STREAM_OPEN_INFO, *PMM_STREAM_OPEN_INFO;
typedef const MM_STREAM_OPEN_INFO *CPMM_STREAM_OPEN_INFO;

#define MM_STREAM_OPEN_INFO_VER(v) (sizeof(MM_STREAM_OPEN_INFO) << 16 | (0xFFFF & (v)))
#define MM_STREAM_OPEN_INFO_VERSION_0 MM_STREAM_OPEN_INFO_VER(0)

/*******************************************************************************
*  mm_StreamGetStat definitions
*******************************************************************************/
typedef struct tagMM_STREAM_STAT
{
	unsigned int unVersion; // version of the structure
	unsigned int unBytesIn; // total number of bytes put into stream
	unsigned int unBytesOut; // total number of bytes sent to board
	unsigned char *pHeadPointer; // pointer to position in stream 
	unsigned char *pTailPointer; // pointer to position in stream buffer
	unsigned int unCurrentState; // idle, streaming etc.
	unsigned int unNumberOfBufferUnderruns;
	unsigned int unNumberOfBufferOverruns;
	unsigned int unBufferSize; // buffer size
	unsigned int unBufferMode; // read or write mode
	unsigned int unSpaceAvailable; // bytes available in stream buffer
	unsigned int unHighWaterMark; // high water mark for stream buffer
	unsigned int unLowWaterMark; // low water mark for stream buffer
} MM_STREAM_STAT, *PMM_STREAM_STAT;
typedef const MM_STREAM_STAT *CPMM_STREAM_STAT;

#define MM_STREAM_STAT_VER(v) (sizeof(MM_STREAM_STAT) << 16 | (0xFFFF & (v)))
#define MM_STREAM_STAT_VERSION_0 MM_STREAM_STAT_VER(0)

typedef enum tagEMM_STREAM_WATERMARK_LEVEL
{
	EMM_WM_HIGH,
	EMM_WM_LOW,
	EMM_WM_UNDEFINED = 0xff
} eMM_STREAM_WATERMARK_LEVEL;

/*******************************************************************************
*  mm_StreamSetWaterMark definitions
*******************************************************************************/
typedef struct tagMM_STREAM_WATERMARK_INFO
{
	unsigned int unVersion;
	eMM_STREAM_WATERMARK_LEVEL Level; // EMM_WM_HIGH or EMM_WM_LOW
	unsigned int unValue; // watermark value in bytes
} MM_STREAM_WATERMARK_INFO, *PMM_STREAM_WATERMARK_INFO;
typedef const MM_STREAM_WATERMARK_INFO *CPMM_STREAM_WATERMARK_INFO;

#define MM_STREAM_WATERMARK_INFO_VER(v) (sizeof(MM_STREAM_WATERMARK_INFO) << 16 | (0xFFFF & (v)))
#define MM_STREAM_WATERMARK_INFO_VERSION_0 MM_STREAM_WATERMARK_INFO_VER(0)

/*******************************************************************************
* MM_ERROR_RESULT
* Data block returned with MMEV_ERROR event
*******************************************************************************/
typedef struct tagMM_ERROR_RESULT
{
	unsigned int unVersion;
	unsigned int unErrorCode;
	unsigned int unErrorMsg;
	unsigned int unData[4];
} MM_ERROR_RESULT, *PMM_ERROR_RESULT;
typedef const MM_ERROR_RESULT* CPMM_ERROR_RESULT;

#define MM_ERROR_RESULT_VER(v) (sizeof(MM_ERROR_RESULT) << 16 | (0xFFFF & (v)))
#define MM_ERROR_RESULT_VERSION_0 MM_ERROR_RESULT_VER(0)


/*******************************************************************************
* MM_INFO
*******************************************************************************/
typedef struct tagMM_INFO
{
	unsigned int unVersion;
	int mmValue;
	const char* mmMsg;
	const char* additionalInfo;
} MM_INFO, *PMM_INFO;
typedef const MM_INFO* CPMM_INFO;

#define MM_INFO_VER(v) (sizeof(MM_INFO) << 16 | (0xFFFF & (v)))
#define MM_INFO_VERSION_0 MM_INFO_VER(0)

/*******************************************************************************
* MM_METAEVENT
*******************************************************************************/
typedef struct tagMM_METAEVENT
{
    unsigned int unVersion;
    /* application calls mm_GetMetaEvent() to fill in these fields */
    unsigned long    flags;              /* flags field */
                                         /* - possibly event data structure type */
                                         /* i.e. evtdata_struct_type */
    void*            evtdatap;           /* pointer to the event data block */
                                         /* other libraries to be determined */
                                         /* sr_getevtdatap() */
    long             evtlen;             /* event length */
                                         /* sr_getevtlen */
    long             evtdev;             /* sr_getevtdev */
    long             evttype;            /* Event type */
    void*            evtUserInfo;        /* User Info */

    int              rfu1;               /* for future use only */

} MM_METAEVENT, *PMM_METAEVENT;
typedef const MM_METAEVENT* CPMM_METAEVENT;

#define MM_METAEVENT_VER(v) (sizeof(MM_METAEVENT) << 16 | (0xFFFF & (v)))
#define MM_METAEVENT_VERSION_0 MM_METAEVENT_VER(0)

/* define(s) for flags field within MM_METAEVENT structure */
#define MMME_MM_EVENT               0x2  /* Event is an MM event */


/*******************************************************************************
* MM API methods
*******************************************************************************/
#ifdef __cplusplus
extern "C" {
#endif  /* __cplusplus */

MMLIB_API
int MMAPI_CONV mm_Open(
	const char*             szDevName,
	CPMM_OPEN_INFO          pOpenInfo,
	void*                   pUserInfo
	);
	
MMLIB_API
int MMAPI_CONV mm_Close(
	int                     nDeviceHandle,
	CPMM_CLOSE_INFO         pCloseInfo
	);

MMLIB_API
int MMAPI_CONV mm_Play(
	int                     nDeviceHandle,
	CPMM_PLAY_INFO          pPlayInfo,
	CPMM_RUNTIME_CONTROL    pRuntimeControl,
	void*                   pUserInfo
	);
	
MMLIB_API
int MMAPI_CONV mm_Record(
	int                     nDeviceHandle,
	CPMM_RECORD_INFO        pRecordInfo,
	CPMM_RUNTIME_CONTROL    pRuntimeControl,
	void*                   pUserInfo
	);
	
MMLIB_API
int MMAPI_CONV mm_Stop(
	int                     nDeviceHandle,
	CPMM_STOP               pStop,
	void*                   pUserInfo
	);

MMLIB_API
int MMAPI_CONV mm_GetParm(
	int                     nDeviceHandle,
	CPMM_GET_PARM           pGetParm,
	void*                   pUserInfo
	);
	
MMLIB_API
int MMAPI_CONV mm_SetParm(
	int                     nDeviceHandle,
	CPMM_SET_PARM           pSetParm,
	void*                   pUserInfo
	);
	
MMLIB_API
int MMAPI_CONV mm_GetChanState(
	int                     nDeviceHandle,
	CPMM_GET_CHAN_STATE     pGetChanState,
	void*                   pUserInfo
	);

MMLIB_API
int MMAPI_CONV mm_Reset(
	int                     nDeviceHandle,
	CPMM_RESET              pReset,
	void*                   pUserInfo
	);

MMLIB_API
int MMAPI_CONV mm_EnableEvents(
	int                     nDeviceHandle,
	CPMM_EVENTS             pEvents,
	void*                   pUserInfo
	);

MMLIB_API
int MMAPI_CONV mm_DisableEvents(
	int                     nDeviceHandle,
	CPMM_EVENTS             pEvents,
	void*                   pUserInfo
	);

MMLIB_API
int MMAPI_CONV mm_GetMetaEvent(
	PMM_METAEVENT           pMetaEvent
	);

#ifdef MMAPI_TARGET_WIN32
MMLIB_API
int MMAPI_CONV mm_GetMetaEventEx(
	PMM_METAEVENT         pMetaEvent,
	unsigned int          evt_handle
	);
#endif	/* ifdef MMAPI_TARGET_WIN32 */

MMLIB_API
int MMAPI_CONV mm_StreamOpen(
	CPMM_STREAM_OPEN_INFO pStreamOpenInfo,
	void                  *pUserInfo
	);

MMLIB_API
int MMAPI_CONV mm_StreamClose(
	int          nStreamHandle
	);

MMLIB_API
int MMAPI_CONV mm_StreamReset(
	int          nStreamHandle
	);

MMLIB_API
int MMAPI_CONV mm_StreamGetStat(
	int          nStreamHandle,
	PMM_STREAM_STAT       pStreamStat
	);

MMLIB_API
int MMAPI_CONV mm_StreamSetWaterMark(
	int               nStreamHandle,
	CPMM_STREAM_WATERMARK_INFO pStreamWaterMarkInfo
	);

MMLIB_API
int MMAPI_CONV mm_StreamRead(
	int          nStreamHandle,
	unsigned char         *pData,
	unsigned int          *pDataSize,
	unsigned int          *pEndFlag
	);

MMLIB_API
int MMAPI_CONV mm_StreamWrite(
	int          nStreamHandle,
	unsigned char         *pData,
	unsigned int          unDataSize,
	unsigned int          unEndFlag
	);

/* Retrieves information about MM events */
MMLIB_API
int MMAPI_CONV mm_ResultInfo(
	CPMM_METAEVENT          pMetaEvent,
	PMM_INFO                pInfo
	);

/* Provides thread-specific error information about a failed function */
MMLIB_API
int MMAPI_CONV mm_ErrorInfo(
	PMM_INFO                pInfo
	);

#ifdef __cplusplus
}
#endif  /* __cplusplus */

#pragma pack(pop)

#endif /* _MMLIB_H_ */

