
/************************************************************************
 **   FILENAME:          ipmlib.h
 **   CREATED:           1/1/2001
 **   LAST MODIFIED:     07/16/2003

 **   Copyright (c) 2002 Intel Corporation
 **   All Rights Reserved

 **	
************************************************************************/

#ifndef __IPMLIB_H__
#define __IPMLIB_H__

#ifdef _WIN32

#ifdef IPMEDIALIB_EXPORTS
#define IPMEDIALIB_API __declspec(dllexport)
#else
#define IPMEDIALIB_API __declspec(dllimport)
#endif
#else

#define IPMEDIALIB_API 
#endif

#if defined (__GNUC__ )
#include <stdint.h> 
typedef uint64_t UINT64; 
#elif defined (_MSC_VER) 
#include <windows.h>   /* will get definition of UINT64 */
#else 
#pragma error "Unable to define UINT64 - unknown compiler"
#endif 

#include "ctinfo.h"
#include "ipmedia_codecs.h"

/* > JAF CR10803 */
#include <stdlib.h>
#include <malloc.h>
/* < JAF CR10803 */

/* Constants */
#if ((!defined EV_ASYNC) && (!defined(EV_SYNC)))
#define EV_ASYNC              0x8000
#define EV_SYNC               0x0000
#endif

#define MAX_IPM_DIGITS						0x20 /*32*/
#define MAX_MEDIA_INFO						0x14/*20*/
#define IP_ADDR_SIZE						0x20 /*32*/
#define MAX_QOS_SESSION						0xd /*13*/
#define MAX_EVENT							0x20 /*32*/
#define MAX_ALARM							0xd /*13*/
#define MAX_QOS_THRESHOLD					0xd /*13*/
#define MAX_T38INFOFRAME					0x14 /*20*/
#define MAX_T38HDLCFRAME					0x20 /*32*/
#define MAX_ENET_IPADDR						0x08 /*8*/


/* Event list */
#define IPMEV_MASK							0x9000
#define IPMEV_ERROR_MASK					(IPMEV_MASK | 0x0800)

#define IPMEV_OPEN							(IPMEV_MASK | 0x01)
#define IPMEV_STARTMEDIA					(IPMEV_MASK | 0x02)
#define IPMEV_STOPPED						(IPMEV_MASK | 0x03)
#define IPMEV_GET_LOCAL_MEDIA_INFO			(IPMEV_MASK | 0x04)
#define IPMEV_GET_SESSION_INFO				(IPMEV_MASK | 0x05)
#define IPMEV_LISTEN						(IPMEV_MASK | 0x06)
#define IPMEV_UNLISTEN						(IPMEV_MASK | 0x07)
#define IPMEV_GET_XMITTS_INFO				(IPMEV_MASK | 0x08)
#define IPMEV_SEND_DIGITS					(IPMEV_MASK | 0x09)
#define IPMEV_RECEIVE_DIGITS				(IPMEV_MASK | 0x0a)
#define IPMEV_DIGITS_RECEIVED				(IPMEV_MASK | 0x0b)
#define IPMEV_EVENT_ENABLED					(IPMEV_MASK | 0x0c)
#define IPMEV_EVENT_DISABLED				(IPMEV_MASK | 0x0d)
#define IPMEV_GET_QOS_ALARM_STATUS			(IPMEV_MASK | 0x0e)
#define IPMEV_RESET_QOS_ALARM_STATUS		(IPMEV_MASK | 0x0f)
#define IPMEV_SET_QOS_THRESHOLD_INFO		(IPMEV_MASK | 0x10)
#define IPMEV_GET_QOS_THRESHOLD_INFO		(IPMEV_MASK | 0x11)
#define IPMEV_QOS_ALARM						(IPMEV_MASK | 0x12)
#define IPMEV_SET_PARM						(IPMEV_MASK | 0x13)
#define IPMEV_GET_PARM						(IPMEV_MASK | 0x14)
#define IPMEV_PING							(IPMEV_MASK | 0x15)
#define IPMEV_SEND_SIGNAL_TO_IP				(IPMEV_MASK | 0x16)
#define IPMEV_TELEPHONY_EVENT				(IPMEV_MASK | 0x17)		// RFC2833
#define IPMEV_FAXTONE						(IPMEV_MASK | 0x18)
#define IPMEV_T38CALLSTATE					(IPMEV_MASK | 0x19)
#define IPMEV_T38CAPFRAME_STATUS			(IPMEV_MASK | 0x1a)
#define IPMEV_T38INFOFRAME_STATUS			(IPMEV_MASK | 0x1b)
#define IPMEV_T38HDLCFRAME_STATUS			(IPMEV_MASK | 0x1c)
#define IPMEV_T38POSTPGCODE_STATUS			(IPMEV_MASK | 0x1d)
#define IPMEV_ERROR							(IPMEV_MASK | 0x1e)	// for compatibility
#define IPMEV_T38POSTPGRESP_STATUS			(IPMEV_MASK | 0x1f)
#define IPMEV_GET_CTINFO					(IPMEV_MASK | 0x20)
#define IPMEV_MODIFYMEDIA					(IPMEV_MASK | 0x21)
#define IPMEV_GET_CAPABILITIES				(IPMEV_MASK | 0x22)

/* > JAF CR10803 */
#define IPMEV_INIT_SEND						(IPMEV_MASK | 0x23)
#define IPMEV_INIT_RESPONSE_SEND			(IPMEV_MASK | 0x24)
#define IPMEV_INIT_RECEIVED					(IPMEV_MASK | 0x25)
#define IPMEV_INIT_COMPLETE					(IPMEV_MASK | 0x26)
#define IPMEV_INIT_FAILED					(IPMEV_MASK | 0x27)
#define IPMEV_NACK_SENT						(IPMEV_MASK | 0x28)
/* < JAF CR10803 */
#define IPMEV_SEC_NOTIFY_EXPIRE_KEY_AUDIO	(IPMEV_MASK | 0x23)
#define IPMEV_SEC_NOTIFY_EXPIRE_KEY_VIDEO	(IPMEV_MASK | 0x24)

// New paradigm for the failure events
#define IPMEV_MODIFYMEDIA_FAIL				(IPMEV_ERROR_MASK | IPMEV_MODIFYMEDIA)
#define IPMEV_GET_CAPABILITIES_FAIL			(IPMEV_ERROR_MASK | IPMEV_GET_CAPABILITIES)
/* > JAF CR10803 */
#define IPMEV_INIT_SEND_FAIL				(IPMEV_ERROR_MASK | IPMEV_INIT_SEND)
#define IPMEV_INIT_RESPONSE_SEND_FAIL		(IPMEV_ERROR_MASK | IPMEV_INIT_RESPONSE_SEND)
/* < JAF CR10803 */

/******************************** T.38 ******************************/

typedef enum 
{
	T38_PPGC_TX	= 0x0,
	T38_PPGC_RX	= 0x1
} eIPM_T38POSTPGCODE_STATUS;

typedef enum 
{
	T38_PPGR_TX	= 0x0,
	T38_PPGR_RX	= 0x1
} eIPM_T38POSTPGRESP_STATUS;

typedef enum
{
	T38_HDLCFRAME_TX	= 0x0,
	T38_HDLCFRAME_RX	= 0x1
} eIPM_T38HDLCFRAME_STATUS;

typedef struct ipm_t38hdlcframe_status_info_tag
{
	eIPM_T38HDLCFRAME_STATUS		eHDLCFrameType;
	unsigned int						unHDLCFrameOctets;
	unsigned int						unHDLCFrameArray[MAX_T38HDLCFRAME];
}IPM_T38HDLCFRAME_STATUS_INFO, *PIPM_T38HDLCFRAME_STATUS_INFO;

typedef enum
{
	T38INFOFRAME_TX_SUB		= 0x0,
	T38INFOFRAME_RX_SUB		= 0x1,
	T38INFOFRAME_TX_SEP		= 0x2,
	T38INFOFRAME_RX_SEP		= 0x3,
	T38INFOFRAME_TX_PWD		= 0x4,
	T38INFOFRAME_RX_PWD		= 0x5,
	T38INFOFRAME_TX_TSI		= 0x6,
	T38INFOFRAME_RX_TSI		= 0x7,
	T38INFOFRAME_TX_CSI		= 0x8,
	T38INFOFRAME_RX_CSI		= 0x9,
	T38INFOFRAME_TX_CIG		= 0xa,
	T38INFOFRAME_RX_CIG		= 0xb
} eIPM_T38INFOFRAME_STATUS;

typedef	struct ipm_t38infoframe_info_tag
{
	eIPM_T38INFOFRAME_STATUS	eInfoFrameType;
	unsigned int				unInfoFrameOctets[MAX_T38INFOFRAME];
}IPM_T38INFOFRAME_STATUS_INFO, *PIPM_T38INFOFRAME_STATUS_INFO;

typedef enum
{
	T38CAPFRAME_TX_DIS_DTC	= 0x0,
	T38CAPFRAME_TX_DCS		= 0x1,
	T38CAPFRAME_TX_CTC		= 0x2,
	T38CAPFRAME_RX_DIX_DTC	= 0x3,
	T38CAPFRAME_RX_DCS		= 0x4,
	T38CAPFRAME_RX_CTC		= 0x5
} eIPM_T38CAPFRAME_STATUS;

typedef struct ipm_t38capframe_status_info_tag
{
	eIPM_T38CAPFRAME_STATUS		eCapFrameType;
	unsigned int				unVertResolution;
	unsigned int				unBitRate;
	unsigned int				unPageWidth;
	unsigned int				unPageLength;
	unsigned int				unDataFormat;
	unsigned int				unECM;
	unsigned int				unBFT;
	unsigned int				unScanTime;
	unsigned int				unJPEG;
} IPM_T38CAPFRAME_STATUS_INFO, *PIPM_T38CAPFRAME_STATUS_INFO;


typedef enum
{
	T38CALLSTATE_DISCONNECT,
	T38CALLSTATE_SWITCH_TO_VOICE
} eIPM_T38CALLSTATE;

/***************************** Events **********************************/

typedef enum
{
	/*Channel specific events */
	EVT_DTMFDISCARDED			= 0x0001,
	EVT_LOSTPACKETS				= 0x0002,
	EVT_JITTER					= 0x0003,
	EVT_ROUNDTRIPLATENCY		= 0x0004,
	EVT_RTCPTIMEOUT				= 0x0005,
	EVT_RTPTIMEOUT				= 0x0006,
	EVT_SEC_AUTH_FAIL_AUDIO		= 0x0020,		/* Authentication failure on Audio packets */
	EVT_SEC_AUTH_FAIL_VIDEO		= 0x0021,		/* Authentication failure on Video packets */
	EVT_SEC_PKT_REPLAY_AUDIO	= 0x0022,		/* Audio Packet Replay monitoring */
	EVT_SEC_PKT_REPLAY_VIDEO	= 0x0023,		/* Video Packet Replay monitoring */
	EVT_SEC_MKI_NOMATCH_AUDIO	= 0x0024,		/* no matching MKI on Audio packets */
	EVT_SEC_MKI_NOMATCH_VIDEO	= 0x0025,		/* no matching MKI on Video packets */
	EVT_SEC_NOTIFY_EXPIRE_KEY_AUDIO = 0x0026,	/* PreNotification for expiring key for audio */
	EVT_SEC_NOTIFY_EXPIRE_KEY_VIDEO	= 0x0027,	/* PreNotification for expiring key for video */
	EVT_FAXTONE					= 0x0100,		/* from TDM */
	EVT_TELEPHONY				= 0x0200,
	EVT_T38CALLSTATE			= 0x0300,
	EVT_T38FAXTONE				= 0x0301,
	EVT_T38PPC_STATUS			= 0x0302,
	EVT_T38PPR_STATUS			= 0x0303,
	EVT_T38CAPFRAME_STATUS		= 0x0304,
	EVT_T38HDLCFRAME_STATUS		= 0x0305,
	EVT_T38INFOFRAME_STATUS		= 0x0306,

/* < JAF CR15300 */
	EVT_INIT_RECEIVED			= 0x0307,	/* Required messages for RTP encapsupation protocols (NBUP, etc) */
	EVT_PROCEDURE_DONE			= 0x0308,
	EVT_SEND_FAILED				= 0x0309,
	EVT_NACK_SENT				= 0x030A,
/* > JAF CR15300 */

	/*Board specific events */
	EVT_NETWORKFAILURE			= 0x1001

} eIPM_EVENT;


/***************************** QOS **********************************/


typedef enum
{
	QOS_EVENT_DTMFDISCARDED		= 0x01,
	QOS_EVENT_LOSTPACKETS		= 0x02,
	QOS_EVENT_JITTER			= 0x03,
	QOS_EVENT_ROUNDTRIPLATENCY	= 0x04,
	QOS_EVENT_RTCPTIMEOUT		= 0x05,
	QOS_EVENT_RTPTIMEOUT		= 0x06
} eIPM_QOS_EVENT;


typedef enum
{
	ALARM_STATE_OFF		= 0x0,
	ALARM_STATE_ON		= 0x1
} eIPM_ALARM_STATE;

typedef enum
{
	/*Channel specific QoS */
	QOSTYPE_DTMFDISCARDED		= 0x01,
	QOSTYPE_LOSTPACKETS			= 0x02,
	QOSTYPE_JITTER				= 0x03,
	QOSTYPE_ROUNDTRIPLATENCY	= 0x04,
	QOSTYPE_RTCPTIMEOUT			= 0x05,
	QOSTYPE_RTPTIMEOUT			= 0x06,
	QOSTYPE_SEC_AUTH_FAIL_AUDIO		= 0x20,
	QOSTYPE_SEC_AUTH_FAIL_VIDEO		= 0x21,
	QOSTYPE_SEC_PKT_REPLAY_AUDIO	= 0x22,
	QOSTYPE_SEC_PKT_REPLAY_VIDEO	= 0x23,
	QOSTYPE_SEC_MKI_NOMATCH_AUDIO	= 0x24,
	QOSTYPE_SEC_MKI_NOMATCH_VIDEO	= 0x25,

	/*Board specific QoS */
	QOSTYPE_NETWORKFAILURE		= 0x1001
} eIPM_QOS_TYPE;

typedef struct ipm_qos_alarm_data_tag
{
   eIPM_QOS_TYPE		eQoSType;			/*The QoS parameter type */
   eIPM_ALARM_STATE		eAlarmState;		/*indicate if On/Off */
} IPM_QOS_ALARM_DATA, *PIPM_QOS_ALARM_DATA;

typedef struct ipm_qos_alarm_status_tag
{
   unsigned int unAlarmCount;
   IPM_QOS_ALARM_DATA	QoSData[MAX_ALARM];
} IPM_QOS_ALARM_STATUS, *PIPM_QOS_ALARM_STATUS;

typedef struct ipm_qos_threshold_data_tag
{
    eIPM_QOS_TYPE	eQoSType;     
    unsigned int	unTimeInterval;
    unsigned int	unDebounceOn;
    unsigned int	unDebounceOff;
    unsigned int	unFaultThreshold;
    unsigned int	unPercentSuccessThreshold;
    unsigned int	unPercentFailThreshold;
} IPM_QOS_THRESHOLD_DATA, *PIPM_QOS_THRESHOLD_DATA;

typedef struct ipm_qos_threshold_info_tag
{
   unsigned int				unCount;
   IPM_QOS_THRESHOLD_DATA	QoSThresholdData[MAX_QOS_THRESHOLD];
} IPM_QOS_THRESHOLD_INFO, *PIPM_QOS_THRESHOLD_INFO;

typedef struct ipm_event_info_tag
{
	unsigned int		unCount;                /* number of following structures */
	unsigned int		unEventData[MAX_EVENT]; /* Data associated with the event */
} IPM_EVENT_INFO, *PIPM_EVENT_INFO;

typedef struct ipm_qos_session_info_tag
{
	  eIPM_QOS_TYPE		eQoSType;
	  unsigned int		unData;	
} IPM_QOS_SESSION_INFO, *PIPM_QOS_SESSION_INFO;


/**************************** DTMF ******************************/

typedef enum
{
	DIGIT_ALPHA_NUMERIC		=	0x00,
	DIGIT_SIGNAL			=	0x01,
	DIGIT_SIGNAL_UPDATE		=	0x02
} eIPM_DIGIT_TYPE;

typedef enum
{
	DIGIT_IP_INBAND			=	0x00,
	DIGIT_TDM				=	0x01
} eIPM_DIGIT_DIRECTION;

typedef struct ipm_digit_info_tag
{
	eIPM_DIGIT_TYPE			eDigitType;					/* Type of digits - DTMF, ALPHA-NUMERIC */
	eIPM_DIGIT_DIRECTION	eDigitDirection;			/* The direction of flow of digits */
	char					cDigits[MAX_IPM_DIGITS];	/* the digits */
	unsigned int			unNumberOfDigits;			/* Number of digits */
	unsigned int			unTimeStamp;
	unsigned int			unExpirationTime;
	unsigned int			unDuration;
} IPM_DIGIT_INFO, *PIPM_DIGIT_INFO;

/************************  Session Operations ********************/


typedef struct ipm_rtcp_session_info_tag
{
   unsigned int	unLocalSR_TimeStamp;
   unsigned int	unLocalSR_TxPackets;
   unsigned int	unLocalSR_TxOctets;
   unsigned int	unLocalSR_SendIndication;
   unsigned int	unLocalRR_FractionLost;
   unsigned int	unLocalRR_CumulativeLost;
   unsigned int	unLocalRR_SeqNumber;
   unsigned int	unLocalRR_ValidInfo;
   unsigned int	unRemoteSR_TimeStamp;
   unsigned int	unRemoteSR_TxPackets;
   unsigned int	unRemoteSR_TxOctets;
   unsigned int	unRemoteSR_SendIndication;
   unsigned int	unRemoteRR_FractionLost;
   unsigned int	unRemoteRR_CumulativeLost;
   unsigned int	unRemoteRR_SeqNumber;
   unsigned int	unRemoteRR_ValidInfo;

} IPM_RTCP_SESSION_INFO, *PIPM_RTCP_SESSION_INFO;

typedef struct ipm_session_info_tag
{
	IPM_RTCP_SESSION_INFO  RtcpInfo;
	unsigned int           unQoSInfoCount;
	IPM_QOS_SESSION_INFO   QoSInfo[MAX_QOS_SESSION];
} IPM_SESSION_INFO, *PIPM_SESSION_INFO;


/******************************** SRTP *******************************/

typedef enum 
{
    IPM_SECURITY_KEYTYPE_BINARY = 1,
    IPM_SECURITY_KEYTYPE_BASE64 = 2
} eIPM_SECURITY_KEY_TYPE;

typedef enum
{
    IPM_SECURITY_INFO_MODE_IMMEDIATE           = 1,
    IPM_SECURITY_INFO_MODE_WAITFOREXPIRATION   = 2
} eIPM_SECURITY_INFO_MODE;

typedef enum 
{
    IPM_CRYPTO_AES_CM_128_HMAC_SHA1_80 = 1,
    IPM_CRYPTO_AES_CM_128_HMAC_SHA1_32 = 2 
} eIPM_CRYPTO_SUITE;

typedef struct ipm_security_binary_key_tag
{
    unsigned int    unVersion;          /* version of this structure */
    char            *pcMasterKey;       /* binary master key */
    char            *pcMasterSaltKey;   /* binary master salt key */
} IPM_SECURITY_BINARY_KEY, *PIPM_SECURITY_BINARY_KEY;

typedef struct ipm_security_base64_key_tag
{
    unsigned int    unVersion;          /* version of this structure */
    char            *pcMasterBase64Key; /* base64 encoded master key and master salt key */
} IPM_SECURITY_BASE64_KEY, *PIPM_SECURITY_BASE64_KEY;

typedef struct ipm_security_key_tag
{
    UINT64                  unSRTPLifeTime;             /* max no of SRTP packets using master key */
    unsigned int            unVersion;                  /* version of this structure */
    unsigned int            unSRTCPLifeTime;            /* max no of SRTCP packets using master key  */
    unsigned int            unMkiLength;                /* number of bytes of MKI */
    unsigned int            unMkiValue;                 /* MKI value */
    unsigned int            unMasterKeyLength;          /* number of master key bits */
    unsigned int            unMasterSaltKeyLength;      /* number of master salt key bits */
    eIPM_SECURITY_KEY_TYPE  eKeyType;                   /* type of key that pvMasterKey points to */
    void                    *pvMasterKey;               /* pointer to the master key structure */
} IPM_SECURITY_KEY,         *PIPM_SECURITY_KEY;

typedef struct ipm_srtp_parm_tag {
    unsigned int            unVersion;                  /* version of this structure */
    eIPM_CRYPTO_SUITE       eCryptoSuite;               /* cryptosuite */
    unsigned short          usKeyDerivationRate;        /* key derivation rate */
    unsigned short          usWindowSizeHint;           /* windows size hint */
    unsigned short          usSrtpUnEncryptedFlag;      /* 1 => SRTP messages are NOT encrypted */
    unsigned short          usSrtcpUnEncryptedFlag;     /* 1 => SRTCP messages are NOT encrypted */
    unsigned short          usSrtpUnAuthenticatedFlag;  /* 1 => SRTP messages are NOT authenticated */
    unsigned int            unSsrc;                     /* RTP SSRC */
    unsigned int            unRoc;                      /* roll over counter */
    unsigned int            unSeqNum;                   /* RTP start sequence number */
} IPM_SRTP_PARMS, * PIPM_SRTP_PARMS;

typedef struct ipm_security_info_tag {
    unsigned int            unVersion;      /* version of this structure */
    unsigned int            unNumKeys;      /* number of keys pointed to by pKeys */
    eIPM_SECURITY_INFO_MODE eInfoMode;      /* mode of security info */
    IPM_SECURITY_KEY        *pKeys;         /* pointer to array of key structures */
    IPM_SRTP_PARMS          *pParms;        /* SRTP parameters */
} IPM_SECURITY_INFO, *PIPM_SECURITY_INFO;

#define IPM_SECURITY_KEY_VERSION            ((0x1 & 0xFFFF) | (sizeof(IPM_SECURITY_KEY) << 16))
#define IPM_SECURITY_BINARY_KEY_VERSION     ((0x1 & 0xFFFF) | (sizeof(IPM_SECURITY_BINARY_KEY) << 16))
#define IPM_SECURITY_BASE64_KEY_VERSION     ((0x1 & 0xFFFF) | (sizeof(IPM_SECURITY_BASE64_KEY) << 16))
#define IPM_SECURITY_INFO_VERSION           ((0x1 & 0xFFFF) | (sizeof(IPM_SECURITY_INFO) << 16))
#define IPM_SRTP_PARMS_VERSION              ((0x1 & 0xFFFF) | (sizeof(IPM_SRTP_PARMS) << 16))

#define IPM_CRYPTO_SUITE_DEFAULT            IPM_CRYPTO_AES_CM_128_HMAC_SHA1_80
#define IPM_WINDOW_SIZE_HINT_DEFAULT        64
#define IPM_KDR_DEFAULT                     0
#define IPM_SRTP_UNENCRYPTED_DEFAULT        0  /* SRTP messages are encrypted */
#define IPM_SRTCP_UNENCRYPTED_DEFAULT       0  /* SRTCP messages are encrypted */
#define IPM_SRTP_UNAUTHENTICATED_DEFAULT    0  /* SRTP messages are authenticated */
#define IPM_SSRC_DEFAULT                    0
#define IPM_ROC_DEFAULT                     0
#define IPM_SEQNUM_DEFAULT                  0xFFFFFFFF

#define IPM_SECURITY_MAX_NUM_KEYS           10
#define IPM_SECURITY_MAX_TOTAL_NUM_KEYS     20

static __inline void INIT_IPM_SECURITY_KEY(IPM_SECURITY_KEY *pSecKey) {
    pSecKey->unVersion              = IPM_SECURITY_KEY_VERSION;
    pSecKey->unSRTPLifeTime         = 0x80000000;           /* 2^31 */
    pSecKey->unSRTCPLifeTime        = 0x80000000;           /* 2^31 */
    pSecKey->unMkiLength            = 0;
    pSecKey->unMasterKeyLength      = 128;
    pSecKey->unMasterSaltKeyLength  = 112;
    pSecKey->eKeyType               = IPM_SECURITY_KEYTYPE_BINARY;
    pSecKey->pvMasterKey            = 0;
}

static __inline void INIT_IPM_SECURITY_BINARY_KEY(IPM_SECURITY_BINARY_KEY *pSecKey) {
    pSecKey->unVersion              = IPM_SECURITY_BINARY_KEY_VERSION;
    pSecKey->pcMasterKey            = 0;
    pSecKey->pcMasterSaltKey        = 0;
}

static __inline void INIT_IPM_SECURITY_BASE64_KEY(IPM_SECURITY_BASE64_KEY *pSecKey) {
    pSecKey->unVersion              = IPM_SECURITY_BASE64_KEY_VERSION;
    pSecKey->pcMasterBase64Key      = 0;
}

static __inline void INIT_IPM_SECURITY_INFO(IPM_SECURITY_INFO *pSecInfo) {
    pSecInfo->unVersion             = IPM_SECURITY_INFO_VERSION;
    pSecInfo->unNumKeys             = 0;
    pSecInfo->eInfoMode             = IPM_SECURITY_INFO_MODE_IMMEDIATE;
    pSecInfo->pKeys                 = 0;
    pSecInfo->pParms                = 0;
}

static __inline void INIT_IPM_SRTP_PARMS(IPM_SRTP_PARMS *pSrtpParms) {
    pSrtpParms->unVersion                   = IPM_SRTP_PARMS_VERSION;
    pSrtpParms->eCryptoSuite                = IPM_CRYPTO_SUITE_DEFAULT;
    pSrtpParms->usKeyDerivationRate         = IPM_KDR_DEFAULT;
    pSrtpParms->usWindowSizeHint            = IPM_WINDOW_SIZE_HINT_DEFAULT;
    pSrtpParms->usSrtpUnEncryptedFlag       = IPM_SRTP_UNENCRYPTED_DEFAULT;
    pSrtpParms->usSrtcpUnEncryptedFlag      = IPM_SRTCP_UNENCRYPTED_DEFAULT;
    pSrtpParms->usSrtpUnAuthenticatedFlag   = IPM_SRTP_UNAUTHENTICATED_DEFAULT;
    pSrtpParms->unSsrc                      = IPM_SSRC_DEFAULT;
    pSrtpParms->unRoc                       = IPM_ROC_DEFAULT;
    pSrtpParms->unSeqNum                    = IPM_SEQNUM_DEFAULT;
}

/************************* Media Operations ***************************/


typedef enum
{
	TONE_NONE	= 0x00,
	TONE_CED	= 0x01,
	TONE_CNG	= 0x02
} eIPM_TONE;

/* Flow direction of IP packets */
typedef enum
{

	DATA_IP_RECEIVEONLY			= 0x00,		// rcv RTP + unaffect RTCP
	DATA_IP_SENDONLY			= 0x01,		// send RTP + unaffect RTCP
	DATA_IP_TDM_BIDIRECTIONAL	= 0x02,		// send/rcv RTP and RTCP
	DATA_MULTICAST_SERVER		= 0x03,
	DATA_MULTICAST_CLIENT		= 0x04,
	DATA_IP_INACTIVE			= 0x05,		// Hold: Stop RTP. RTCP continues
	DATA_IP_DIR_NONE			= 0x06		// No change to direction

} eIPM_DATA_DIRECTION;

/* > JAF CR10803 */

/*
 * RTP encapsulation protocol support (NBUP, etc)
 */
typedef enum
{
	RTP_PROTOCOL_NBUP			= 0x00,
} eIPM_RTP_PROTOCOL;

typedef enum
{
	NBUP_INIT_RESPONSE_ACK		= 0x00,
	NBUP_INIT_RESPONSE_NACK
} eIPM_NBUP_INIT_RESPONSE_TYPE;

typedef enum
{
	NBUP_PROFILE_3G324M			= 0x00,
} eIPM_NBUP_PROFILE_TYPE;

typedef enum
{
	NBUP_FLOW_SIZE_0_BITS		= 0x00,
	NBUP_FLOW_SIZE_320_BITS
} eIPM_NBUP_FLOW_SIZE;

typedef enum
{
	NBUP_NACK_INITIALIZE		= 0x00,
	NBUP_NACK_RATE_CONTROL,
	NBUP_NACK_TIME_ALIGNMENT,
} eIPM_NBUP_NACK_PROCEDURE;

/*
 * NBUP Profile data structure
 */
typedef struct ipm_nbup_profile_info_tag
{
	unsigned int				unVersion;
	eIPM_NBUP_PROFILE_TYPE		eProfileType;		/* Type of NBUP profile */
	unsigned char				ucTxPLT;
	unsigned char				ucRxPLT;
} IPM_NBUP_PROFILE_INFO, *PIPM_NBUP_PROFILE_INFO;

/* Structure version and initialization routine */
#define IPM_NBUP_PROFILE_INFO_VERSION	1

static __inline void INIT_IPM_NBUP_PROFILE_INFO(PIPM_NBUP_PROFILE_INFO pStruct)
{
	pStruct->unVersion = (IPM_NBUP_PROFILE_INFO_VERSION << 16) | sizeof (IPM_NBUP_PROFILE_INFO);
}

/*
 * NBUP Init message data structure
 */
typedef struct IPM_NBUP_SUBFLOW_INFO_tag
{
	unsigned int				unVersion;
	eIPM_NBUP_FLOW_SIZE			eFlowSize;
} IPM_NBUP_SUBFLOW_INFO, *PIPM_NBUP_SUBFLOW_INFO;

/* Structure version and initialization routine */
#define IPM_NBUP_SUBFLOW_INFO_VERSION	1

static __inline void INIT_IPM_NBUP_SUBFLOW_INFO(PIPM_NBUP_SUBFLOW_INFO pStruct)
{
	pStruct->unVersion = (IPM_NBUP_SUBFLOW_INFO_VERSION << 16) | sizeof (IPM_NBUP_SUBFLOW_INFO);
}

/*
 * NBUP Init message data structure
 */
typedef struct ipm_NBUP_RFCI_INFO_tag
{
	unsigned int				unVersion;
	unsigned char				ucID;
	IPM_NBUP_SUBFLOW_INFO		*pSubFlows;
} IPM_NBUP_RFCI_INFO, *PIPM_NBUP_RFCI_INFO;

/* Structure version and initialization routine */
#define IPM_NBUP_RFCI_INFO_VERSION	1

static __inline void
INIT_IPM_NBUP_RFCI_INFO(
	PIPM_NBUP_RFCI_INFO pStruct,
	unsigned int unSubFlowCount)
{
	unsigned int iIndx;
	pStruct->unVersion = (IPM_NBUP_RFCI_INFO_VERSION << 16) | sizeof (IPM_NBUP_RFCI_INFO);
	pStruct->pSubFlows = (PIPM_NBUP_SUBFLOW_INFO)malloc(unSubFlowCount * sizeof (IPM_NBUP_SUBFLOW_INFO));

	for (iIndx = 0; iIndx < unSubFlowCount; iIndx++)
	{
		INIT_IPM_NBUP_SUBFLOW_INFO(&(pStruct->pSubFlows[iIndx]));
		pStruct->pSubFlows[iIndx].eFlowSize = NBUP_FLOW_SIZE_320_BITS;
	}
}
static __inline void
FREE_IPM_NBUP_RFCI_INFO(
	PIPM_NBUP_RFCI_INFO pStruct)
{
	if (pStruct->pSubFlows != NULL)
	{
		free(pStruct->pSubFlows);
		pStruct->pSubFlows = NULL;
	}
}

/*
 * NBUP Init message data structure
 */
typedef struct ipm_NBUP_INIT_SEND_tag
{
	unsigned int				unVersion;
	unsigned int				unSubFlowCount;		/* Number of subflows per RFCI */
	unsigned int				unRFCICount;		/* Number of RFCI structures */
	IPM_NBUP_RFCI_INFO			*pRFCIs;			/* RFCI structures */
} IPM_NBUP_INIT_SEND, *PIPM_NBUP_INIT_SEND;

/* Structure version and initialization routine */
#define IPM_NBUP_INIT_SEND_VERSION	1

static __inline void
INIT_IPM_NBUP_INIT_SEND(
	PIPM_NBUP_INIT_SEND pStruct,
	unsigned int unSubFlowCount,
	unsigned int unRFCICount)
{
	unsigned int iIndx;
	pStruct->unVersion = (IPM_NBUP_INIT_SEND_VERSION << 16) | sizeof (IPM_NBUP_INIT_SEND);
	pStruct->unSubFlowCount = unSubFlowCount;
	pStruct->unRFCICount = unRFCICount;
	pStruct->pRFCIs = (PIPM_NBUP_RFCI_INFO)malloc(unRFCICount * sizeof (IPM_NBUP_RFCI_INFO));

	for (iIndx = 0; iIndx < unRFCICount; iIndx++)
	{
		INIT_IPM_NBUP_RFCI_INFO(&(pStruct->pRFCIs[iIndx]), unSubFlowCount);
	}
}
static __inline void
FREE_IPM_NBUP_INIT_SEND(
	PIPM_NBUP_INIT_SEND pStruct)
{
	unsigned int iIndx;
	
	if (pStruct->pRFCIs != NULL)
	{
		for (iIndx = 0; iIndx < pStruct->unRFCICount; iIndx++)
		{
			FREE_IPM_NBUP_RFCI_INFO(&(pStruct->pRFCIs[iIndx]));
		}

		free(pStruct->pRFCIs);
		pStruct->pRFCIs = NULL;
	}
}

/* Typedef, since the init received structure
   uses the same items */
#define IPM_NBUP_INIT_RECEIVED			IPM_NBUP_INIT_SEND
#define INIT_IPM_NBUP_INIT_RECEIVED		INIT_IPM_NBUP_INIT_SEND
#define FREE_IPM_NBUP_INIT_RECEIVED		FREE_IPM_NBUP_INIT_SEND

/*
 * NBUP Initialization response structure
 */
typedef struct IPM_NBUP_INIT_RESPONSE_tag
{
	unsigned int					unVersion;
	eIPM_NBUP_INIT_RESPONSE_TYPE	eResponse;
	unsigned char					ucCause;
} IPM_NBUP_INIT_RESPONSE, *PIPM_NBUP_INIT_RESPONSE;

/* Structure version and initialization routine */
#define IPM_NBUP_INIT_RESPONSE_VERSION	1

static __inline void INIT_IPM_NBUP_INIT_RESPONSE(PIPM_NBUP_INIT_RESPONSE pStruct)
{
	pStruct->unVersion = (IPM_NBUP_INIT_RESPONSE_VERSION << 16) | sizeof (IPM_NBUP_INIT_RESPONSE);
}

/*
 * Generic Init message data structure
 */
typedef struct IPM_INIT_SEND_tag
{
	unsigned int				unVersion;
	eIPM_RTP_PROTOCOL			eProtocol;
	union
	{
		PIPM_NBUP_INIT_SEND		pNBUP;
	} data;
} IPM_INIT_SEND, *PIPM_INIT_SEND;

/* Structure version and initialization routine */
#define IPM_INIT_SEND_VERSION	1

static __inline void
INIT_IPM_INIT_SEND(PIPM_INIT_SEND pStruct)
{
	pStruct->unVersion = (IPM_INIT_SEND_VERSION << 16) | sizeof (IPM_INIT_SEND);
	pStruct->data.pNBUP = NULL;
}

/*
 *  Generic Initialization response structure
 */
typedef struct IPM_INIT_RESPONSE_tag
{
	unsigned int				unVersion;
	eIPM_RTP_PROTOCOL			eProtocol;
	union
	{
		PIPM_NBUP_INIT_RESPONSE	pNBUP;
	} data;
} IPM_INIT_RESPONSE, *PIPM_INIT_RESPONSE;

/* Structure version and initialization routine */
#define IPM_INIT_RESPONSE_VERSION	1

static __inline void INIT_IPM_INIT_RESPONSE(PIPM_INIT_RESPONSE pStruct)
{
	pStruct->unVersion = (IPM_INIT_RESPONSE_VERSION << 16) | sizeof (IPM_INIT_RESPONSE);
	pStruct->data.pNBUP = NULL;
}

/*
 *  Init request structure
 */
typedef struct IPM_INIT_RECEIVED_tag
{
	unsigned int				unVersion;
	eIPM_RTP_PROTOCOL			eProtocol;

	union
	{
		IPM_NBUP_INIT_RECEIVED	*pNBUP;
	} data;
} IPM_INIT_RECEIVED, *PIPM_INIT_RECEIVED;

/* Structure version and initialization routine */
#define IPM_INIT_RECEIVED_VERSION	1

static __inline void INIT_IPM_INIT_RECEIVED(PIPM_INIT_RECEIVED pStruct)
{
	pStruct->unVersion = (IPM_INIT_RECEIVED_VERSION << 16) | sizeof (IPM_INIT_RECEIVED);
	pStruct->data.pNBUP = NULL;
}

/*
 * NBUP Init Failure
 */
typedef struct IPM_NBUP_INIT_FAILURE_tag
{
	unsigned int				unVersion;
	unsigned int				unCause;
} IPM_NBUP_INIT_FAILURE, *PIPM_NBUP_INIT_FAILURE;

/* Structure version and initialization routine */
#define IPM_NBUP_INIT_FAILURE_VERSION	1

static __inline void INIT_IPM_NBUP_INIT_FAILURE(PIPM_NBUP_INIT_FAILURE pStruct)
{
	pStruct->unVersion = (IPM_NBUP_INIT_FAILURE_VERSION << 16) | sizeof (IPM_NBUP_INIT_FAILURE);
}

/*
 * NBUP NACK sent
 */
typedef struct IPM_NBUP_NACK_SENT_tag
{
	unsigned int				unVersion;
	eIPM_NBUP_NACK_PROCEDURE	eProcedure;
	unsigned int				unCause;
} IPM_NBUP_NACK_SENT, *PIPM_NBUP_NACK_SENT;

/* Structure version and initialization routine */
#define IPM_NBUP_NACK_SENT_VERSION	1

static __inline void INIT_IPM_NBUP_NACK_SENT(PIPM_NBUP_NACK_SENT pStruct)
{
	pStruct->unVersion = (IPM_NBUP_NACK_SENT_VERSION << 16) | sizeof (IPM_NBUP_NACK_SENT);
}

/*
 *  Generic Init Failed response structure
 */
typedef struct IPM_INIT_FAILURE_tag
{
	unsigned int				unVersion;
	eIPM_RTP_PROTOCOL			eProtocol;
	union
	{
		PIPM_NBUP_INIT_FAILURE	pNBUP;
	} data;
} IPM_INIT_FAILURE, *PIPM_INIT_FAILURE;

/* Structure version and initialization routine */
#define IPM_INIT_FAILURE_VERSION	1

static __inline void INIT_IPM_INIT_FAILURE(PIPM_INIT_FAILURE pStruct)
{
	pStruct->unVersion = (IPM_INIT_FAILURE_VERSION << 16) | sizeof (IPM_INIT_FAILURE);
	pStruct->data.pNBUP = NULL;
}

/*
 *  Generic NACK sent structure
 */
typedef struct IPM_NACK_SENT_tag
{
	unsigned int				unVersion;
	eIPM_RTP_PROTOCOL			eProtocol;
	union
	{
		PIPM_NBUP_NACK_SENT	pNBUP;
	} data;
} IPM_NACK_SENT, *PIPM_NACK_SENT;

/* Structure version and initialization routine */
#define IPM_NACK_SENT_VERSION	1

static __inline void INIT_IPM_NACK_SENT(PIPM_NACK_SENT pStruct)
{
	pStruct->unVersion = (IPM_NACK_SENT_VERSION << 16) | sizeof (IPM_NACK_SENT);
	pStruct->data.pNBUP = NULL;
}

/* Error info structure
 *
 */
typedef struct tagIPM_INFO
{
	unsigned int	unVersion;
	int				ipmValue;
	const char		*ipmMsg;
	const char		*pAdditionalInfo;
} IPM_INFO, *PIPM_INFO;

/* Structure version and initialization routine */
#define IPM_INFO_VERSION	1

static __inline void INIT_IPM_INFO(PIPM_INFO pStruct)
{
   pStruct->unVersion = (IPM_INFO_VERSION << 16) | sizeof (IPM_INFO);
}

/* < JAF CR10803 */

/* NBUP-Specific error cause values
 */
#ifndef __NBUP_ERROR_CAUSE_VALUES__
#define __NBUP_ERROR_CAUSE_VALUES__
 
 #define NBUP_ERR_CRCHDR                   (0)
 #define NBUP_ERR_CRCPYLD                  (1)
 #define NBUP_ERR_UNEXP_FRAMENUM           (2)
 #define NBUP_ERR_FRAMELOSS                (3)
 #define NBUP_ERR_UNKNOWN_PDUTYPE          (4)
 #define NBUP_ERR_UNKNOWN_PROCEDURE        (5)
 #define NBUP_ERR_UNKNOWN_RESERVED         (6)
 #define NBUP_ERR_UNKNOWN_FIELD            (7)
 #define NBUP_ERR_FRAMESHORT               (8)
 #define NBUP_ERR_MISSING_FIELDS           (9)
 #define NBUP_ERR_UNEXP_PDUTYPE            (16)
 #define NBUP_ERR_UNEXP_PROCEDURE          (18)
 #define NBUP_ERR_UNEXP_RFCI               (19)
 #define NBUP_ERR_UNEXP_VALUE              (20)
 #define NBUP_ERR_INITIALISATION_0         (42)
 #define NBUP_ERR_INITIALISATION_1         (43)
 #define NBUP_ERR_INITIALISATION_2         (44)
 #define NBUP_ERR_RATECONTROL_FAILURE      (45)
 #define NBUP_ERR_ERROREVENT_FAILURE       (46)
 #define NBUP_ERR_NOTSUP_TIMEALIGN         (47)
 #define NBUP_ERR_TIMEALIGN_ERROR          (48)
 #define NBUP_ERR_NOTSUP_VERSION           (49)
 
#endif /*__NBUP_ERROR_CAUSE_VALUES__*/


typedef enum
{
	//AUDIO
	MEDIATYPE_AUDIO_REMOTE_RTP_INFO,
	MEDIATYPE_AUDIO_LOCAL_RTP_INFO,		
	MEDIATYPE_AUDIO_REMOTE_RTCP_INFO,		
	MEDIATYPE_AUDIO_LOCAL_RTCP_INFO,		
	MEDIATYPE_AUDIO_REMOTE_CODER_INFO,	
	MEDIATYPE_AUDIO_LOCAL_CODER_INFO,

	//FAX
	MEDIATYPE_LOCAL_UDPTL_T38_INFO,
	MEDIATYPE_REMOTE_UDPTL_T38_INFO,
    MEDIATYPE_FAX_SIGNAL,

	//VIDEO
	MEDIATYPE_VIDEO_REMOTE_RTP_INFO ,
	MEDIATYPE_VIDEO_LOCAL_RTP_INFO,		
	MEDIATYPE_VIDEO_REMOTE_RTCP_INFO,		
	MEDIATYPE_VIDEO_LOCAL_RTCP_INFO,		
	MEDIATYPE_VIDEO_REMOTE_CODER_INFO,	
	MEDIATYPE_VIDEO_LOCAL_CODER_INFO,
	MEDIATYPE_VIDEO_REMOTE_SECURITY_INFO,	
	MEDIATYPE_VIDEO_LOCAL_SECURITY_INFO,

	MEDIATYPE_AUDIO_REMOTE_SECURITY_INFO,	
	MEDIATYPE_AUDIO_LOCAL_SECURITY_INFO,
	MEDIATYPE_AUDIO_REMOTE_CODER_OPTIONS_INFO,	
	MEDIATYPE_AUDIO_LOCAL_CODER_OPTIONS_INFO,
/* > JAF CR10803 */
	// NBUP
	MEDIATYPE_NBUP_REMOTE_RTP_INFO,
	MEDIATYPE_NBUP_LOCAL_RTP_INFO,		
	MEDIATYPE_NBUP_PROFILE_INFO, /* NBUP is mutually exclusive with Audio/Video coders */
								/* DATA_IP_TDM_BIDIRECTIONAL must be specified for NBUP */
/* < JAF CR10803 */

} eIPM_MEDIA_TYPE;

#define CODER_TYPE_NATIVE					0x8000

typedef enum
{
	CODER_TYPE_NONSTANDARD				= CODEC_AUDIO_NONSTANDARD,
	CODER_TYPE_G711ALAW64K				= CODEC_AUDIO_G711ALAW64K,
	CODER_TYPE_G711ALAW64K_NATIVE		= CODEC_AUDIO_G711ALAW64K | CODER_TYPE_NATIVE,
	CODER_TYPE_G711ALAW56K				= CODEC_AUDIO_G711ALAW56K,
	CODER_TYPE_G711ALAW56K_NATIVE		= CODEC_AUDIO_G711ALAW56K | CODER_TYPE_NATIVE,
	CODER_TYPE_G711ULAW64K				= CODEC_AUDIO_G711ULAW64K,
	CODER_TYPE_G711ULAW64K_NATIVE		= CODEC_AUDIO_G711ULAW64K | CODER_TYPE_NATIVE,
	CODER_TYPE_G711ULAW56K				= CODEC_AUDIO_G711ULAW56K,
	CODER_TYPE_G711ULAW56K_NATIVE		= CODEC_AUDIO_G711ULAW56K | CODER_TYPE_NATIVE,
	CODER_TYPE_G721ADPCM				= CODEC_AUDIO_G721ADPCM,
	CODER_TYPE_G721ADPCM_NATIVE			= CODEC_AUDIO_G721ADPCM | CODER_TYPE_NATIVE,
	CODER_TYPE_G722_48K					= CODEC_AUDIO_G722_48K,
	CODER_TYPE_G722_48K_NATIVE			= CODEC_AUDIO_G722_48K | CODER_TYPE_NATIVE,
	CODER_TYPE_G722_56K					= CODEC_AUDIO_G722_56K,
	CODER_TYPE_G722_56K_NATIVE			= CODEC_AUDIO_G722_56K | CODER_TYPE_NATIVE,
	CODER_TYPE_G722_64K					= CODEC_AUDIO_G722_64K,
	CODER_TYPE_G722_64K_NATIVE			= CODEC_AUDIO_G722_64K | CODER_TYPE_NATIVE,
	CODER_TYPE_G7231_5_3K				= CODEC_AUDIO_G7231_5_3K,
	CODER_TYPE_G7231_5_3K_NATIVE		= CODEC_AUDIO_G7231_5_3K | CODER_TYPE_NATIVE,
	CODER_TYPE_G7231_6_3K				= CODEC_AUDIO_G7231_6_3K,
	CODER_TYPE_G7231_6_3K_NATIVE		= CODEC_AUDIO_G7231_6_3K | CODER_TYPE_NATIVE,
	CODER_TYPE_G7231ANNEXCCAP			= CODEC_AUDIO_G7231ANNEXCCAP,
	CODER_TYPE_G7231ANNEXCCAP_NATIVE	= CODEC_AUDIO_G7231ANNEXCCAP | CODER_TYPE_NATIVE,
	CODER_TYPE_G726_16K					= CODEC_AUDIO_G726_16K,
	CODER_TYPE_G726_16K_NATIVE			= CODEC_AUDIO_G726_16K | CODER_TYPE_NATIVE,
	CODER_TYPE_G726_24K					= CODEC_AUDIO_G726_24K,
	CODER_TYPE_G726_24K_NATIVE			= CODEC_AUDIO_G726_24K | CODER_TYPE_NATIVE,
	CODER_TYPE_G726_32K					= CODEC_AUDIO_G726_32K,
	CODER_TYPE_G726_32K_NATIVE			= CODEC_AUDIO_G726_32K | CODER_TYPE_NATIVE,
	CODER_TYPE_G726_40K					= CODEC_AUDIO_G726_40K,
	CODER_TYPE_G726_40K_NATIVE			= CODEC_AUDIO_G726_40K | CODER_TYPE_NATIVE,
	CODER_TYPE_G728						= CODEC_AUDIO_G728,
	CODER_TYPE_G728_NATIVE				= CODEC_AUDIO_G728 | CODER_TYPE_NATIVE,
	CODER_TYPE_G729						= CODEC_AUDIO_G729,
	CODER_TYPE_G729_NATIVE				= CODEC_AUDIO_G729 | CODER_TYPE_NATIVE,
	CODER_TYPE_G729ANNEXA				= CODEC_AUDIO_G729ANNEXA,
	CODER_TYPE_G729ANNEXA_NATIVE		= CODEC_AUDIO_G729ANNEXA | CODER_TYPE_NATIVE,
	CODER_TYPE_G729WANNEXB				= CODEC_AUDIO_G729WANNEXB,
	CODER_TYPE_G729WANNEXB_NATIVE		= CODEC_AUDIO_G729WANNEXB | CODER_TYPE_NATIVE,
	CODER_TYPE_G729ANNEXAWANNEXB		= CODEC_AUDIO_G729ANNEXAWANNEXB,
	CODER_TYPE_G729ANNEXAWANNEXB_NATIVE	= CODEC_AUDIO_G729ANNEXAWANNEXB | CODER_TYPE_NATIVE,
	CODER_TYPE_GSMFULLRATE				= CODEC_AUDIO_GSMFULLRATE,
	CODER_TYPE_GSMFULLRATE_NATIVE		= CODEC_AUDIO_GSMFULLRATE | CODER_TYPE_NATIVE,
	CODER_TYPE_GSMHALFRATE				= CODEC_AUDIO_GSMHALFRATE,
	CODER_TYPE_GSMHALFRATE_NATIVE		= CODEC_AUDIO_GSMHALFRATE | CODER_TYPE_NATIVE,
	CODER_TYPE_GSMEFR					= CODEC_AUDIO_GSMEFULLRATE,
	CODER_TYPE_GSMEFR_NATIVE			= CODEC_AUDIO_GSMEFULLRATE | CODER_TYPE_NATIVE,
	CODER_TYPE_IS11172					= CODEC_AUDIO_IS11172,
	CODER_TYPE_IS11172_NATIVE			= CODEC_AUDIO_IS11172 | CODER_TYPE_NATIVE,
	CODER_TYPE_IS13818					= CODEC_AUDIO_IS13818,
	CODER_TYPE_IS13818_NATIVE			= CODEC_AUDIO_IS13818 | CODER_TYPE_NATIVE,
	CODER_TYPE_AMRNB_4_75k				= CODEC_AUDIO_AMRNB_4_75k,						/*rate is 4.75k*/
	CODER_TYPE_AMRNB_4_75k_NATIVE		= CODEC_AUDIO_AMRNB_4_75k | CODER_TYPE_NATIVE,	/*rate is 4.75k*/
	CODER_TYPE_AMRNB_5_15k				= CODEC_AUDIO_AMRNB_5_15k,						/*rate is 5.15k*/
	CODER_TYPE_AMRNB_5_15k_NATIVE		= CODEC_AUDIO_AMRNB_5_15k | CODER_TYPE_NATIVE,	/*rate is 5.15k*/
	CODER_TYPE_AMRNB_5_9k				= CODEC_AUDIO_AMRNB_5_9k,						/*rate is 5.9k*/
	CODER_TYPE_AMRNB_5_9k_NATIVE		= CODEC_AUDIO_AMRNB_5_9k | CODER_TYPE_NATIVE,	/*rate is 5.9k*/
	CODER_TYPE_AMRNB_6_7k				= CODEC_AUDIO_AMRNB_6_7k,						/*rate is 6.7k*/
	CODER_TYPE_AMRNB_6_7k_NATIVE		= CODEC_AUDIO_AMRNB_6_7k | CODER_TYPE_NATIVE,	/*rate is 6.7k*/
	CODER_TYPE_AMRNB_7_4k				= CODEC_AUDIO_AMRNB_7_4k,						/*rate is 7.4k*/
	CODER_TYPE_AMRNB_7_4k_NATIVE		= CODEC_AUDIO_AMRNB_7_4k | CODER_TYPE_NATIVE,	/*rate is 7.4k*/
	CODER_TYPE_AMRNB_7_95k				= CODEC_AUDIO_AMRNB_7_95k,						/*rate is 7.95k*/
	CODER_TYPE_AMRNB_7_95k_NATIVE		= CODEC_AUDIO_AMRNB_7_95k | CODER_TYPE_NATIVE,	/*rate is 7.95k*/
	CODER_TYPE_AMRNB_10_2k				= CODEC_AUDIO_AMRNB_10_2k,						/*rate is 10.2k*/
	CODER_TYPE_AMRNB_10_2k_NATIVE		= CODEC_AUDIO_AMRNB_10_2k | CODER_TYPE_NATIVE,	/*rate is 10.2k*/
	CODER_TYPE_AMRNB_12_2k				= CODEC_AUDIO_AMRNB_12_2k,						/*rate is 12.2k*/
	CODER_TYPE_AMRNB_12_2k_NATIVE		= CODEC_AUDIO_AMRNB_12_2k | CODER_TYPE_NATIVE,	/*rate is 12.2k*/
	CODER_TYPE_AMRNB_NONE				= CODEC_AUDIO_AMRNB_NONE,						/*rate is no preference */
	CODER_TYPE_EVRC_1_2k				= CODEC_AUDIO_EVRC_1_2k,						/*rate is 1.2k*/
	CODER_TYPE_EVRC_1_2k_NATIVE			= CODEC_AUDIO_EVRC_1_2k | CODER_TYPE_NATIVE,	/*rate is 1.2k*/
	CODER_TYPE_EVRC_4_8k				= CODEC_AUDIO_EVRC_4_8k,						/*rate is 4.8k*/
	CODER_TYPE_EVRC_4_8k_NATIVE			= CODEC_AUDIO_EVRC_4_8k | CODER_TYPE_NATIVE,	/*rate is 4.8k*/
	CODER_TYPE_EVRC_9_6k				= CODEC_AUDIO_EVRC_9_6k,						/*rate is 9.6k*/
	CODER_TYPE_EVRC_9_6k_NATIVE			= CODEC_AUDIO_EVRC_9_6k | CODER_TYPE_NATIVE,	/*rate is 9.6k*/
	CODER_TYPE_G729ANNEXE				= CODEC_AUDIO_G729ANNEXE,
	CODER_TYPE_G729ANNEXE_NATIVE		= CODEC_AUDIO_G729ANNEXE | CODER_TYPE_NATIVE,
	CODER_TYPE_AMRWB_6_6K				= CODEC_AUDIO_AMRWB_6_6K,
	CODER_TYPE_AMRWB_6_6K_NATIVE		= CODEC_AUDIO_AMRWB_6_6K | CODER_TYPE_NATIVE,
	CODER_TYPE_AMRWB_8_85K				= CODEC_AUDIO_AMRWB_8_85K,
	CODER_TYPE_AMRWB_8_85K_NATIVE		= CODEC_AUDIO_AMRWB_8_85K | CODER_TYPE_NATIVE,
	CODER_TYPE_AMRWB_12_65K				= CODEC_AUDIO_AMRWB_12_65K,
	CODER_TYPE_AMRWB_12_65K_NATIVE		= CODEC_AUDIO_AMRWB_12_65K | CODER_TYPE_NATIVE,
	CODER_TYPE_AMRWB_14_25K				= CODEC_AUDIO_AMRWB_14_25K,
	CODER_TYPE_AMRWB_14_25K_NATIVE		= CODEC_AUDIO_AMRWB_14_25K | CODER_TYPE_NATIVE,
	CODER_TYPE_AMRWB_15_85K				= CODEC_AUDIO_AMRWB_15_85K,
	CODER_TYPE_AMRWB_15_85K_NATIVE		= CODEC_AUDIO_AMRWB_15_85K | CODER_TYPE_NATIVE,
	CODER_TYPE_AMRWB_18_25K				= CODEC_AUDIO_AMRWB_18_25K,
	CODER_TYPE_AMRWB_18_25K_NATIVE		= CODEC_AUDIO_AMRWB_18_25K | CODER_TYPE_NATIVE,
	CODER_TYPE_AMRWB_19_85K				= CODEC_AUDIO_AMRWB_19_85K,
	CODER_TYPE_AMRWB_19_85K_NATIVE		= CODEC_AUDIO_AMRWB_19_85K | CODER_TYPE_NATIVE,
	CODER_TYPE_AMRWB_23_05K				= CODEC_AUDIO_AMRWB_23_05K,
	CODER_TYPE_AMRWB_23_05K_NATIVE		= CODEC_AUDIO_AMRWB_23_05K | CODER_TYPE_NATIVE,
	CODER_TYPE_AMRWB_23_85K				= CODEC_AUDIO_AMRWB_23_85K,
	CODER_TYPE_AMRWB_23_85K_NATIVE		= CODEC_AUDIO_AMRWB_23_85K | CODER_TYPE_NATIVE,
	CODER_TYPE_ILBC_13_33K				= CODEC_AUDIO_ILBC_13_33K,
	CODER_TYPE_ILBC_13_33K_NATIVE		= CODEC_AUDIO_ILBC_13_33K | CODER_TYPE_NATIVE,
	CODER_TYPE_ILBC_15_2K				= CODEC_AUDIO_ILBC_15_2K,
	CODER_TYPE_ILBC_15_2K_NATIVE		= CODEC_AUDIO_ILBC_15_2K | CODER_TYPE_NATIVE,

	/*Video*/
	CODER_TYPE_H263						= CODEC_VIDEO_H263,
	CODER_TYPE_H263_1998				= CODEC_VIDEO_H263_1998,
	CODER_TYPE_MP4V_ES					= CODEC_VIDEO_MP4V_ES
} eIPM_CODER_TYPE;

typedef enum
{
	CODER_FRAMESIZE_5	= 0x05,
	CODER_FRAMESIZE_10	= 0xa,
	CODER_FRAMESIZE_20	= 0x14,
	CODER_FRAMESIZE_30	= 0x1e 
} eIPM_CODER_FRAMESIZE;

typedef enum
{
	CODER_VAD_DISABLE	= 0x00,
	CODER_VAD_ENABLE	= 0x01
} eIPM_CODER_VAD;

typedef struct ipm_video_coder_info_tag
{
	unsigned int			unVersion;		  /*structure version*/
	eIPM_CODER_TYPE		eCoderType;		  /* The coder Type - e.g. CODER_TYPE_H263*/
	unsigned int			unCoderPayloadType;	/* Static or dynamic range of  96-127*/
} IPM_VIDEO_CODER_INFO, *PIPM_VIDEO_CODER_INFO;

#define IPM_VIDEO_CODER_INFO_VER(v) \
	sizeof(IPM_VIDEO_CODER_INFO) << 16 | (0xFFFF & v)

#define IPM_VIDEO_CODER_INFO_VERSION_0  IPM_VIDEO_CODER_INFO_VER(0)


typedef struct ipm_audio_coder_info_tag
{
	   eIPM_CODER_TYPE		eCoderType;				/* The coder Type */
	   eIPM_CODER_FRAMESIZE	eFrameSize;				/* Frame size supported*/
	   unsigned int			unFramesPerPkt;			/* No. of Frames per packet*/
	   eIPM_CODER_VAD			eVadEnable;				/* Flag indicating if VAD is enabled/disabled */
	   unsigned int			unCoderPayloadType;		/* Type of coder payload supported*/
	   unsigned int			unRedPayloadType;		/* Type of Redundancy Payload supported*/
}IPM_AUDIO_CODER_INFO, *PIPM_AUDIO_CODER_INFO;

typedef 	IPM_AUDIO_CODER_INFO		IPM_CODER_INFO;
typedef 	PIPM_AUDIO_CODER_INFO		PIPM_CODER_INFO;

typedef struct ipm_audio_coder_options_info_tag
{
	unsigned int		unVersion;
	unsigned int		unCoderOptions; /* Specifies options for a particular coder */
	unsigned int        unParm1;  
    int                 nValue1;  
    unsigned int        unParm2;
    int                 nValue2;	   
}IPM_AUDIO_CODER_OPTIONS_INFO, *PIPM_AUDIO_CODER_OPTIONS_INFO;

#define IPM_AUDIO_CODER_OPTIONS_INFO_VER(v) \
	sizeof(IPM_AUDIO_CODER_OPTIONS_INFO) << 16 | (0xFFFF & v)

#define IPM_AUDIO_CODER_OPTIONS_INFO_VERSION  IPM_AUDIO_CODER_OPTIONS_INFO_VER(0)

typedef struct ipm_port_info_tag
{
   unsigned int   unPortId;                  /* The Port ID */
   char           cIPAddress[IP_ADDR_SIZE];  /* IP Address  */

} IPM_PORT_INFO, *PIPM_PORT_INFO;

typedef struct ToneDetection
{
	eIPM_TONE		eToneType;
	unsigned short	unDuration;
} IPM_TONE_DETECTED, *PIPM_TONE_DETECTED;

typedef struct ipm_fax_signal_tag 
{
	eIPM_TONE		eToneType;
	unsigned int	unToneDuration;
} IPM_FAX_SIGNAL, *PIPM_FAX_SIGNAL;

typedef struct ipm_media_tag
{
	eIPM_MEDIA_TYPE eMediaType;			
	union
	{
		IPM_PORT_INFO				PortInfo;		/* RTP Port Information*/
		IPM_AUDIO_CODER_INFO		AudioCoderInfo; /* Audio Coder Information  */
		IPM_FAX_SIGNAL				FaxSignal;		/* Fax Signal information*/
		IPM_VIDEO_CODER_INFO		VideoCoderInfo; /* Video Coder Information  */
		IPM_SECURITY_INFO		    SecurityInfo;   /* Security Information  */
		IPM_AUDIO_CODER_OPTIONS_INFO		AudioCoderOptionsInfo;  /* Extended Audio Coder Information  */	
		/* > JAF CR10803 */
		IPM_NBUP_PROFILE_INFO		NBUPProfileInfo; /* NBUP Profile Information */
		/* < JAF CR10803 */
	}mediaInfo;
} IPM_MEDIA, *PIPM_MEDIA;

typedef struct ipm_media_info_tag
{
	unsigned int	unCount;
	IPM_MEDIA		MediaData[MAX_MEDIA_INFO];   
} IPM_MEDIA_INFO, *PIPM_MEDIA_INFO;


typedef struct ipm_stop_info_tag
{
	int nRFU;
}IPM_STOP_INFO, * PIPM_STOP_INFO;

typedef enum
{
	STOP_SEND_DIGITS			= 0x01,
	STOP_RECEIVE_DIGITS			= 0x02,
	STOP_RECEIVE_DIGITS_RFC2833	= 0x03,
	STOP_PLAY					= 0x04,
	STOP_RECORD					= 0x05,
	STOP_MEDIA					= 0x06,
	STOP_ALL					= 0x07
} eIPM_STOP_OPERATION;

/******************************** Parameters *******************************/

typedef enum
{
	/* channel parameters */
   PARMCH_ECHOTAIL				= 0x1000,	/* Echo Tail*/
   PARMCH_RFC2833REDLEVEL		= 0x1001,	/* Redundancy level*/
   PARMCH_RFC2833GEN_TO_TDM		= 0x1002,	/* Convert RFC2833 to signal*/
   PARMCH_RFC2833GEN_TO_IP		= 0x1003,	/* Send RFC2833 to IP */
   PARMCH_DTMFXFERMODE			= 0x1004,	/* DTMF Transfer Mode*/
   PARMCH_ECACTIVE				= 0x1005,	/* Echo cancellation active or not*/
   PARMCH_AGCACTIVE				= 0x1006,	/* Automatic Gain Control active or not*/
   PARMCH_TOS					= 0x1007,	/* type of service; values 0-255.*/
   PARMCH_RFC2833EVT_TX_PLT		= 0x1008,	/* RFC2833 named event Xmit payload*/
   PARMCH_RFC2833EVT_RX_PLT		= 0x1009,	/* RFC2833 named event Receive payload*/
   PARMCH_RFC2833TONE_TX_PLT	= 0x100a,	/* RFC2833 tone Xmit payload*/
   PARMCH_RFC2833TONE_RX_PLT	= 0x100b,	/* RFC2833 tone Receive payload*/
   PARMCH_RFC2833MUTE_AUDIO		= 0x100c,	/* RFC2833 mute tone audio*/
   PARMCH_IPVERSION				= 0x100d,	/* T38 fax version*/
   PARMCH_TCFMETHOD				= 0x100e,	/* TCF signal*/
   PARMCH_ELLIPSISFEATURE		= 0x100f,	/* specifies whether ext to T38 packet format is allowed*/
   PARMCH_HSECM_REDLEVEL		= 0x1010,	/* HS ECM data - specify btwn range of 0:7 with default=1*/
   PARMCH_HS_REDLEVEL			= 0x1011,	/* HS non-ECM - specify btwn range of 0:7 with default=2*/
   PARMCH_HSEOF_REDLEVEL		= 0x1012,	/* EOF marker - specify btwn range of 0:31 with default=2*/
   PARMCH_HSEOT_REDLEVEL		= 0x1013,	/* EOT marker - specify btwn range of 0:31 with default=7*/
   PARMCH_IND_REDLEVEL			= 0x1014,	/* fax indicator packets - specify btwn range of 0:31 with default=7*/
   PARMCH_V21_RED				= 0x1015,	/* V.21 LS HDLC data - specify btwn range of 0:31 with default=7*/
   PARMCH_PACKETRATE			= 0x1016,	/* fax packet generation rate*/
   PARMCH_HSPACKETRATE			= 0x1017,	/* base packet multiplier for HS fax sessions*/
   PARMCH_JITTERCOMP			= 0x1018,	/* specify btwn range of 0:1000 with default of 100*/
   PARMCH_IPTRANSPORT			= 0x1019,	/* transport to be used for the T.38 media session*/
   PARMCH_TTL					= 0x101a,	/* Time to Live - specify btwn range of 0:255 with default = 1*/
   PARMCH_TX_ADJVOLUME			= 0x101b,	/* Volume Level Adjustment on Outbound IP */
   PARMCH_RX_ADJVOLUME			= 0x101c,	/* Volume Level Adjustment on Inbound IP */
   PARMCH_LOCAL_IPADDR			= 0x101d,   /* Active IP address index (read-only)*/
   PARMCH_ECNLP_ACTIVE			= 0x101e,       /* NLP */
   PARMCH_EC_ADAPT_COEFF		= 0x101f,
   PARMCH_EC_RESET			    = 0x1020,
   PARMCH_TX_TIMESLOT			= 0x1021,	/* Transmit TimeSlot */
   PARMCH_NOTIFY_EXPIRE_KEY_AUDIO	= 0x1022,	/* PreNotification time, in ms, for expiring Encryption key for audio */
   PARMCH_NOTIFY_EXPIRE_KEY_VIDEO	= 0x1023,	/* PreNotification time, in ms, for expiring Encryption key or video */

   PARMCH_LATENCYALG_AUDIO		= 0x1024,	/* Algorithm for Jitter Buffer */
   PARMCH_LATENCYFRAMEINIT_AUDIO	= 0x1025,
   PARMCH_LATENCYFRAMEMAX_AUDIO		= 0x1026,
/* > JAF CR10803 */
   PARMCH_NBUP_MODE_VERSION		= 0x1027,	/* NBUP Mode version */
/* < JAF CR10803 */

	/*Board parameters*/
   PARMBD_IPADDR_LIST			= 0x10000,	/* List of IP Addr */
   PARMBD_RTPAUDIO_PORT_RANGE	= 0x10001,	/* RTP Port Range */
   PARMBD_RTPAUDIO_PORT_BASE	= 0x10002,	/* RTP Port Base */
   PARMBD_RTCPAUDIO_INTERVAL	= 0x10003,	/* Interval for RTCP stats */

/* > JAF CR10803 */
   PARMCH_NBUP_SUPPORTED_MODE_VERS = 0x10004, /* Supported NBUP mode versions */
/* < JAF CR10803 */

} eIPM_PARM;



// Parameters Values for Volume Adjustment
typedef enum
{
	PARM_TX_ADJVOL_MIN			= -32,
	PARM_TX_ADJVOL_DEFAULT		= 0,
	PARM_TX_ADJVOL_MAX			= 31
} eIPM_PARM_TX_VOL;

typedef enum
{
	PARM_RX_ADJVOL_MIN			= -32,
	PARM_RX_ADJVOL_DEFAULT		= 0,
	PARM_RX_ADJVOL_MAX			= 31
} eIPM_PARM_RX_VOL;

//

typedef enum
{
	TRANSPORT_UDPTL_UDP		= 0x00,
	TRANSPORT_TPKT_TCP		= 0x01
} eIPM_TRANSPORT_PROTOCOL;

typedef enum
{
	HS_PACKET_X1			= 0x00,
	HS_PACKET_X2			= 0x01,
	HS_PACKET_X4			= 0x02,
	HS_PACKET_X8			= 0x03
} eIPM_HS_PACKET_RATE;

typedef enum
{
	PACKET_10MSEC			= 0x00,
	PACKET_20MSEC			= 0x01,
	PACKET_30MSEC			= 0x02
} eIPM_PACKET_RATE;

typedef enum
{
	ELLIPSIS_OFF			= 0x00,
	ELLIPSIS_ON				= 0x01
} eIPM_ELLIPSIS;

typedef enum
{
	TCF_METHOD_1			= 0x00,		/* recommended for UDPTL, although either method may be used */
	TCF_METHOD_2			= 0x01		/* recommended for TPKT, although either method may be used */
} eIPM_TCF_METHOD_TYPE;

typedef enum
{
   ECHO_TAIL_NONE			= 0x00,
   ECHO_TAIL_8				= 0x01,
   ECHO_TAIL_16				= 0x02,
   ECHO_TAIL_24				= 0x08,
   ECHO_TAIL_32				= 0x03,
   ECHO_TAIL_48				= 0x07,
   ECHO_TAIL_64				= 0x04,
   ECHO_TAIL_96				= 0x06,
   ECHO_TAIL_128			= 0x05
} eIPM_ECHO_TAIL_LENGTH;

typedef enum
{
   RFC2833REDLEVEL_0		= 0x0,
   RFC2833REDLEVEL_1		= 0x1,
   RFC2833REDLEVEL_2		= 0x2,
   RFC2833REDLEVEL_3		= 0x3,
   RFC2833REDLEVEL_4		= 0x4,
   RFC2833REDLEVEL_5		= 0x5
} eIPM_RFC2833_REDLEVEL;

typedef enum
{
   RFC2833GEN_TO_TDM_OFF	= 0x0,
   RFC2833GEN_TO_TDM_ON		= 0x1
} eIPM_RFC2833GEN_TO_TDM;

typedef enum
{
   RFC2833GEN_TO_IP_OFF		= 0x0,
   RFC2833GEN_TO_IP_ON		= 0x1
} eIPM_RFC2833GEN_TO_IP;

typedef enum
{
   DTMFXFERMODE_INBAND		= 0x1,
   DTMFXFERMODE_OUTOFBAND	= 0x2,
   DTMFXFERMODE_RFC2833		= 0x3
} eIPM_DTMFXFERMODE;

typedef enum
{
   ECACTIVE_OFF				= 0x0,
   ECACTIVE_ON				= 0x1
} eIPM_ECACTIVE;

typedef enum
{
   AGCACTIVE_OFF			= 0x0,
   AGCACTIVE_ON				= 0x1
} eIPM_AGCACTIVE;

typedef enum
{
	RFC2833MUTE_AUDIO_ON	= 0x0,
	RFC2833MUTE_AUDIO_OFF	= 0x1
} eIPM_RFC2833MUTE_AUDIO;

typedef enum
{
	RTCPINTERVAL_AUDIO_MIN		= 0x1,
	RTCPINTERVAL_AUDIO_DEFAULT	= 0x5,
	RTCPINTERVAL_AUDIO_MAX		= 0xf
} eRTCPINTERVAL_AUDIO;						/* seconds */

typedef enum
{
	RTPPORTBASE_AUDIO_MIN		= 0,
	RTPPOSTBASE_AUDIO_DEFAULT	= 0xc000,	/* 49152 */
	RTPPORTBASE_AUDIO_MAX		= 0xffff
} eRTPPORTBASE_AUDIO;

typedef enum
{
	RTPPORTBASE_VIDEO_MIN		= 0,
	RTPPORTBASE_VIDEO_DEFAULT	= 0xe000,	/* 57344 */
	RTPPORTBASE_VIDEO_MAX		= 0xffff
} eRTPPORTBASE_VIDEO;

#ifndef defined_eIPM_CODER_AMRNB_OPTIONS
#define defined_eIPM_CODER_AMRNB_OPTIONS
typedef enum
{
	/* CODER_OPT_AMR_CMR_TRACK and CODER_OPT_AMR_CMR_LIMIT 
	   are mutually exclusive */
	CODER_OPT_AMR_CMR_TRACK = 0x1,
	CODER_OPT_AMR_CMR_LIMIT = 0x2,
	/* CODER_OPT_AMR_OCTET and CODER_OPT_AMR_EFFICIENT 
	   are mutually exclusive */
	CODER_OPT_AMR_OCTET = 0x4,
	CODER_OPT_AMR_EFFICIENT = 0x8
} eIPM_CODER_AMRNB_OPTIONS;
#endif /* defined_eIPM_CODER_AMRNB_OPTIONS */

#ifndef defined_eIPM_CODER_EVRC_OPTIONS
#define defined_eIPM_CODER_EVRC_OPTIONS

typedef enum
{
	CODER_OPT_EVRC_SIGNALLING_OFF = 0x0,
	CODER_OPT_EVRC_SIGNALLING_DIM_AND_BURST_HALF_RATE = 0x1,
	CODER_OPT_EVRC_SIGNALLING_BLANK = 0x2

} eIPM_CODER_EVRC_OPTIONS_SIGNALLING;



typedef enum
{
	CODER_OPT_INTERLEAVE_LENGTH = 0x1
} eIPM_CODER_OPTION_PARMS;

#endif

typedef struct ipm_parm_info_tag
{
   eIPM_PARM		eParm;
   void				*pvParmValue;
}IPM_PARM_INFO, *PIPM_PARM_INFO;


/********************************* Misc ********************************/

#ifndef __SC_TSINFO__
#define __SC_TSINFO__
typedef struct sc_tsinfo 
{
   unsigned long	sc_numts;
   long				*sc_tsarrayp;
} SC_TSINFO;
#endif

typedef struct ipm_open_info_tag
{
	int nRFU;
}IPM_OPEN_INFO, *PIPM_OPEN_INFO;

typedef struct ipm_close_info_tag
{
	int nRFU;
}IPM_CLOSE_INFO, *PIPM_CLOSE_INFO;


typedef struct ipm_enet_ipaddr_info_tag
{
unsigned int	unVersion;
   unsigned int   unCount;  
   char IPAddrList[MAX_ENET_IPADDR][IP_ADDR_SIZE];  

} IPM_ENET_IPADDR_INFO, *PIPM_ENET_IPADDR_INFO;

#define IPM_ENET_IPADDR_INFO_VER(v) \
	sizeof(IPM_ENET_IPADDR_INFO) << 16 | (0xFFFF & v)

#define IPM_ENET_IPADDR_INFO_VERSION_0  IPM_ENET_IPADDR_INFO_VER(0)

typedef struct ipm_ping_parameter_tag
{
	char			cRemoteIPAddress[IP_ADDR_SIZE];	/* Destination IP Address */
	char			cLocalIPAddress[IP_ADDR_SIZE];	/* Local PMAC/IP Address */	
	unsigned long	ulNumOfPings;					/* RFU - Number of Echo Requests to send */
	unsigned long	ulPacketSize;					/* RFU - Number of data bytes to be sent */	
	unsigned long	ulTimeout;						/* RFU - mSec Timeout to wait for each reply */	
}IPM_PING_PARM, * PIPM_PING_PARM;


typedef  struct ipm_ping_info_tag
{
	unsigned int unPacketsSent;
	unsigned int unPacketsReceived;
	unsigned int unPacketsLost;
	
	float fRoundTripMin;					/* Time values in mSec */
	float fRoundTripAvg;
	float fRoundTripMax;

}IPM_PING_INFO, * PIPM_PING_INFO ;



typedef enum
{	
	SIGNAL_ID_EVENT_DTMF_0							= 0x0,
	SIGNAL_ID_EVENT_DTMF_1							= 0x1,
	SIGNAL_ID_EVENT_DTMF_2							= 0x2,
	SIGNAL_ID_EVENT_DTMF_3							= 0x3,
	SIGNAL_ID_EVENT_DTMF_4							= 0x4,
	SIGNAL_ID_EVENT_DTMF_5							= 0x5,
	SIGNAL_ID_EVENT_DTMF_6							= 0x6,
	SIGNAL_ID_EVENT_DTMF_7							= 0x7,
	SIGNAL_ID_EVENT_DTMF_8							= 0x8,
	SIGNAL_ID_EVENT_DTMF_9							= 0x9,
	SIGNAL_ID_EVENT_DTMF_STAR						= 0xa,
	SIGNAL_ID_EVENT_DTMF_POUND						= 0xb,
	SIGNAL_ID_EVENT_DTMF_A							= 0xc,
	SIGNAL_ID_EVENT_DTMF_B							= 0xd,
	SIGNAL_ID_EVENT_DTMF_C							= 0xe,
	SIGNAL_ID_EVENT_DTMF_D							= 0xf,
	SIGNAL_ID_EVENT_DTMF_FLASH						= 0x10,
	SIGNAL_ID_EVENT_LINE_OFFHOOK					= 0x40,
	SIGNAL_ID_EVENT_LINE_ONHOOK						= 0x41,
	SIGNAL_ID_EVENT_LINE_DIAL_TONE					= 0x42,
	SIGNAL_ID_EVENT_LINE_PABX_INTERNALDIAL_TONE		= 0x43,
	SIGNAL_ID_EVENT_LINE_SPECIALDIAL_TONE			= 0x44,
	SIGNAL_ID_EVENT_LINE_SECONDDIAL_TONE			= 0x45,
	SIGNAL_ID_EVENT_LINE_RINGING_TONE				= 0x46,
	SIGNAL_ID_EVENT_LINE_SPECIALRINGING_TONE		= 0x47,
	SIGNAL_ID_EVENT_LINE_BUSY_TONE					= 0x48,
	SIGNAL_ID_EVENT_LINE_CONGESTION_TONE			= 0x49,
	SIGNAL_ID_EVENT_LINE_SPECIALINFORMATION_TONE	= 0x4a,
	SIGNAL_ID_EVENT_LINE_COMFORT_TONE				= 0x4b,
	SIGNAL_ID_EVENT_LINE_HOLD_TONE					= 0x4c,
	SIGNAL_ID_EVENT_LINE_RECORD_TONE				= 0x4d,
	SIGNAL_ID_EVENT_LINE_CALLERWAITING_TONE			= 0x4e,
	SIGNAL_ID_EVENT_LINE_CALLWAITING_TONE			= 0x4f,
	SIGNAL_ID_EVENT_LINE_PAY_TONE					= 0x50,
	SIGNAL_ID_EVENT_LINE_POSITIVEINDICATION_TONE	= 0x51,
	SIGNAL_ID_EVENT_LINE_NEGATIVEINDICATION_TONE	= 0x52,
	SIGNAL_ID_EVENT_LINE_WARNING_TONE				= 0x53,
	SIGNAL_ID_EVENT_LINE_INTRUSION_TONE				= 0x54,
	SIGNAL_ID_EVENT_LINE_CALLINGCARDSERVICE_TONE	= 0x55,
	SIGNAL_ID_EVENT_LINE_PAYPHONERECOGNITION_TONE	= 0x56,
	SIGNAL_ID_EVENT_LINE_CPE_ALERTINGSIGNAL_ID_CAS	= 0x57,
	SIGNAL_ID_EVENT_LINE_OFFHOOKWARNING_TONE		= 0x58,
	SIGNAL_ID_EVENT_LINE_RING						= 0x59,
	SIGNAL_ID_EVENT_LINE_ACCEPTANCE_TONE			= 0x60,
	SIGNAL_ID_EVENT_LINE_CONFIRMATION_TONE			= 0x61,
	SIGNAL_ID_EVENT_LINE_DIALTONE_RECALL			= 0x62,
	SIGNAL_ID_EVENT_LINE_ENDOFTHREEPARTYSERVICE_TONE= 0x63,
	SIGNAL_ID_EVENT_LINE_FACILITIES_TONE			= 0x64,
	SIGNAL_ID_EVENT_LINE_LINELOCKOUT_TONE			= 0x65,
	SIGNAL_ID_EVENT_LINE_NUMBERUNOBTAINABLE_TONE	= 0x66,
	SIGNAL_ID_EVENT_LINE_OFFERING_TONE				= 0x67,
	SIGNAL_ID_EVENT_LINE_PERMANENTSIGNAL_ID_TONE	= 0x68,
	SIGNAL_ID_EVENT_LINE_PREEMPTION_TONE			= 0x69,
	SIGNAL_ID_EVENT_LINE_QUEUE_TONE					= 0x6a,
	SIGNAL_ID_EVENT_LINE_REFUSAL_TONE				= 0x6b,
	SIGNAL_ID_EVENT_LINE_ROUTE_TONE					= 0x6c,
	SIGNAL_ID_EVENT_LINE_VALID_TONE					= 0x6d,
	SIGNAL_ID_EVENT_LINE_WAITING_TONE				= 0x6e,
	SIGNAL_ID_EVENT_LINE_WARNING_TONE_ENDOFPERIOD	= 0x6f,
	SIGNAL_ID_EVENT_LINE_WARNING_TONE_PIPTONE		= 0x70,
	SIGNAL_ID_EVENT_TRUNK_MF_0						= 0x80,
	SIGNAL_ID_EVENT_TRUNK_MF_1						= 0x81,
	SIGNAL_ID_EVENT_TRUNK_MF_2						= 0x82,
	SIGNAL_ID_EVENT_TRUNK_MF_3						= 0x83,
	SIGNAL_ID_EVENT_TRUNK_MF_4						= 0x84,
	SIGNAL_ID_EVENT_TRUNK_MF_5						= 0x85,
	SIGNAL_ID_EVENT_TRUNK_MF_6						= 0x86,
	SIGNAL_ID_EVENT_TRUNK_MF_7						= 0x87,
	SIGNAL_ID_EVENT_TRUNK_MF_8						= 0x88,
	SIGNAL_ID_EVENT_TRUNK_MF_9						= 0x89,
	SIGNAL_ID_EVENT_TRUNK_MF_K0_OR_KP_STARTOFPULSING= 0x8a,
	SIGNAL_ID_EVENT_TRUNK_MF_K1						= 0x8b,
	SIGNAL_ID_EVENT_TRUNK_MF_K2						= 0x8c,
	SIGNAL_ID_EVENT_TRUNK_MF_S0_TO_ST_ENDOFPULSING	= 0x8d,
	SIGNAL_ID_EVENT_TRUNK_MF_S1						= 0x8e,
	SIGNAL_ID_EVENT_TRUNK_MF_S2						= 0x8f,
	SIGNAL_ID_EVENT_TRUNK_ABCD_SIGNALING_0			= 0x90,
	SIGNAL_ID_EVENT_TRUNK_ABCD_SIGNALING_1			= 0x91,
	SIGNAL_ID_EVENT_TRUNK_ABCD_SIGNALING_2			= 0x92,
	SIGNAL_ID_EVENT_TRUNK_ABCD_SIGNALING_3			= 0x93,
	SIGNAL_ID_EVENT_TRUNK_ABCD_SIGNALING_4			= 0x94,
	SIGNAL_ID_EVENT_TRUNK_ABCD_SIGNALING_5			= 0x95,
	SIGNAL_ID_EVENT_TRUNK_ABCD_SIGNALING_6			= 0x96,
	SIGNAL_ID_EVENT_TRUNK_ABCD_SIGNALING_7			= 0x97,
	SIGNAL_ID_EVENT_TRUNK_ABCD_SIGNALING_8			= 0x98,
	SIGNAL_ID_EVENT_TRUNK_ABCD_SIGNALING_9			= 0x99,
	SIGNAL_ID_EVENT_TRUNK_ABCD_SIGNALING_10			= 0x9a,
	SIGNAL_ID_EVENT_TRUNK_ABCD_SIGNALING_11			= 0x9b,
	SIGNAL_ID_EVENT_TRUNK_ABCD_SIGNALING_12			= 0x9c,
	SIGNAL_ID_EVENT_TRUNK_ABCD_SIGNALING_13			= 0x9d,
	SIGNAL_ID_EVENT_TRUNK_ABCD_SIGNALING_14			= 0x9e,
	SIGNAL_ID_EVENT_TRUNK_ABCD_SIGNALING_15			= 0x9f,
	SIGNAL_ID_EVENT_TRUNK_WINK						= 0xa0,
	SIGNAL_ID_EVENT_TRUNK_WINKOFF					= 0xa1,
	SIGNAL_ID_EVENT_TRUNK_INCOMING_SEIZURE			= 0xa2,
	SIGNAL_ID_EVENT_TRUNK_SEIZURE					= 0xa3,
	SIGNAL_ID_EVENT_TRUNK_UNSEIZE_CIRCUIT			= 0xa4,
	SIGNAL_ID_EVENT_TRUNK_CONTINUITY_TEST			= 0xa5,
	SIGNAL_ID_EVENT_TRUNK_DEFAULT_CONTINUITY_TONE	= 0xa6,
	SIGNAL_ID_EVENT_TRUNK_CONTINUITY_TONE_SINGLETONE= 0xa7,
	SIGNAL_ID_EVENT_TRUNK_CONTINUITY_TESTSEND		= 0xa8,
	SIGNAL_ID_EVENT_TRUNK_CONTINUITY_VERIFIED		= 0xa9,
	SIGNAL_ID_EVENT_TRUNK_LOOPBACK					= 0xaa,
	SIGNAL_ID_EVENT_TRUNK_OLD_MILLIWATT_TONE		= 0xab,
	SIGNAL_ID_EVENT_TRUNK_NEW_MILLIWATT_TONE		= 0xac
} eIPM_TELEPHONY_EVENT_ID;

/************Obsolete RFC2833 parameters*********************/
#define eIPM_RFC2833_SIGNAL_ID		eIPM_TELEPHONY_EVENT_ID

typedef enum
{
	SIGNAL_STATE_OFF	= 0x0,
	SIGNAL_STATE_ON		= 0x1
} eIPM_SIGNAL_STATE;

typedef struct ipm_rfc2833_signalid_info_tag
{
	eIPM_RFC2833_SIGNAL_ID	eSignalID;
	eIPM_SIGNAL_STATE		eState;
}IPM_RFC2833_SIGNALID_INFO;

#define IPMEV_RFC2833SIGNAL_RECEIVED IPMEV_TELEPHONY_EVENT

#define EVT_RFC2833	EVT_TELEPHONY	
/***********************************************************/

typedef enum
{
   TEL_INFOTYPE_EVENT,
   TEL_INFOTYPE_TONE
} eIPM_TELEPHONY_INFO_TYPE;

typedef struct ipm_telephony_event_info_tag
{
	unsigned int  			unVersion;          /* Structure version for library use only */      
	eIPM_TELEPHONY_EVENT_ID eTelephonyEventID;  /* The named event usually DTMF named event */ 
	short           		sVolume;  			/* The power level for the DTMF event tone*/
	unsigned short			usDuration;			/* Duration for the DTMF digit in ms*/	
} IPM_TELEPHONY_EVENT_INFO, *PIPM_TELEPHONY_EVENT_INFO;

typedef struct ipm_telephony_tone_info_tag
{
	unsigned int  			unVersion;          /* Structure version for library use only */     
	eIPM_TELEPHONY_EVENT_ID eTelephonyEventID;  /* The named event usually DTMF named event */
	unsigned short 	        usModulationFreq;  /* The modulation frequency in Hz         */
	unsigned char			ucModFreqDivBy3;	/* If set to 1 the modulation frequency is to be divided by 3 */
	short					sVolume;			/* Power level of tone */
	unsigned  short			usDuration;    		/* Duration of tone in ms   */
	unsigned short 			usFreq1;			/* The frequency of tone 1 to be added */
	unsigned short 			usFreq2;			/* The frequency of tone 2 to be added */
	unsigned short          usFreq3;            /* The frequency of tone 3 to be added */
	unsigned short          usFreq4;            /* The frequency of tone 4 to be added */	
} IPM_TELEPHONY_TONE_INFO, *PIPM_TELEPHONY_TONE_INFO;


typedef struct ipm_telephony_info_tag
{
   unsigned int  unVersion;                  /* Structure version for library use only */
   eIPM_TELEPHONY_INFO_TYPE eTelInfoType;    /* RFC2833 Info type - named event or tone */
   union
   {
      IPM_TELEPHONY_EVENT_INFO	TelEvtInfo;   /* RFC2833 named event info eg. DTMF digit */
      IPM_TELEPHONY_TONE_INFO   TelToneInfo;  /* RFC2833 non-standard tone signal Information */
   }TelephonyInfo;
} IPM_TELEPHONY_INFO, *PIPM_TELEPHONY_INFO;


/*********************FR2356 Get Capabilities **************************/

#ifndef defined_CAPABILITY_TYPE
#define defined_CAPABILITY_TYPE
typedef enum CAPABILITY_TYPE
{
   CAPABILITY_CODERLIST
} eCAPABILITY_TYPE;
#endif /* defined_CAPABILITY_TYPE */

#define IPM_CAPABILITIES_VERSION 1

typedef struct ipm_capabilities_tag
{
   unsigned int version;
   union 
   {
      IPM_CODER_INFO Coder;
      // Future types here.
   };
}IPM_CAPABILITIES;

/******************************* IPML Functions ************************/

#if (defined(__BORLAND__) || defined(__cplusplus) || defined(__STDC__))

#if defined(__cplusplus)
extern "C" {
#endif

IPMEDIALIB_API 
int 
ipm_Close(
	int nDeviceHandle, 
	IPM_CLOSE_INFO* pCloseInfo 
	);

IPMEDIALIB_API 
int 
ipm_DisableEvents(
	int nDeviceHandle, 
	eIPM_EVENT *pEvents,
	unsigned int unNumofEvents, 
	unsigned short usMode
	);

IPMEDIALIB_API 
int 
ipm_EnableEvents(
	int nDeviceHandle,
	eIPM_EVENT *pEvents,
	unsigned int unNumofEvents,
	unsigned short usMode
	);

IPMEDIALIB_API 
int 
ipm_GetCTInfo(
   int Dev, 
   CT_DEVINFO *pCTInfo,
   unsigned short usMode
   );

IPMEDIALIB_API 
int 
ipm_GetLocalMediaInfo(
	int nDeviceHandle, 
	IPM_MEDIA_INFO *pMediaInfo,
	unsigned short usMode
	);

IPMEDIALIB_API 
int 
ipm_GetParm(
   int Dev, 
   IPM_PARM_INFO *pParmInfo,
   unsigned short usMode  
   );
IPMEDIALIB_API 
int 
ipm_GetCapabilities(
	int	a_nDeviceHandle,
	eCAPABILITY_TYPE	a_CapType,
	unsigned int a_num,
	IPM_CAPABILITIES a_CapabilitiesArray[],
	unsigned short a_usMode);

IPMEDIALIB_API 
int 
ipm_GetQoSAlarmStatus(
	int nDeviceHandle, 
	IPM_QOS_ALARM_STATUS  *pQoSAlarmInfo,
	unsigned short usMode
	);

IPMEDIALIB_API 
int 
ipm_GetQoSThreshold(
	int nDeviceHandle,
	IPM_QOS_THRESHOLD_INFO *pQoSThresholdInfo,
	unsigned short usMode
	);

IPMEDIALIB_API 
int 
ipm_GetSessionInfo(
	int nDeviceHandle, 
	IPM_SESSION_INFO *pSessionInfo,
	unsigned short usMode
	);

IPMEDIALIB_API 
int 
ipm_GetXmitSlot(
	int nDeviceHandle, 
	SC_TSINFO *pTimeslotInfo,
	unsigned short usMode
	);

IPMEDIALIB_API 
int 
ipm_Listen(
	int nDeviceHandle, 
	SC_TSINFO *pTimeslotInfo,
	unsigned short usMode
	);

IPMEDIALIB_API 
int 
ipm_Open(
	const char *szDevName, 
	IPM_OPEN_INFO *pOpenInfo, 
	unsigned short usMode
	);

IPMEDIALIB_API 
int 
ipm_ReceiveDigits(
	int nDeviceHandle, 
	IPM_DIGIT_INFO *pDigitInfo,
	unsigned short usMode
	);

IPMEDIALIB_API 
int 
ipm_ResetQoSAlarmStatus(
	int nDeviceHandle,
	IPM_QOS_ALARM_STATUS *pQoSAlarmInfo,
	unsigned short usMode
	);

IPMEDIALIB_API 
int 
ipm_SendDigits(
	int nDeviceHandle, 
	IPM_DIGIT_INFO *pDigitInfo,
	unsigned short usMode
	);

IPMEDIALIB_API 
int 
ipm_SendRFC2833SignalIDToIP(
	int nDeviceHandle, 
	IPM_RFC2833_SIGNALID_INFO *pSignalInfo,
	unsigned short usMode
	);

IPMEDIALIB_API 
int 
ipm_SetParm(
   int Dev, 
   IPM_PARM_INFO *pParmInfo,
   unsigned short usMode  
   );

IPMEDIALIB_API
int 
ipm_SetQoSThreshold(
	int nDeviceHandle,
	IPM_QOS_THRESHOLD_INFO *pQoSThresholdInfo,
	unsigned short usMode
	);

IPMEDIALIB_API 
int 
ipm_StartMedia(
	int nDeviceHandle, 
	IPM_MEDIA_INFO* pMediaInfo,
	eIPM_DATA_DIRECTION eDirection,
	unsigned short usMode
	);

IPMEDIALIB_API 
int 
ipm_ModifyMedia(
	int nDeviceHandle, 
	IPM_MEDIA_INFO* pMediaInfo,
	eIPM_DATA_DIRECTION eDirection,
	unsigned short usMode
	);

IPMEDIALIB_API 
int 
ipm_Stop(
	int nDeviceHandle, 
	eIPM_STOP_OPERATION eOperation, 
	unsigned short usMode
	);

IPMEDIALIB_API 
int 
ipm_UnListen(
	int nDeviceHandle, 
	unsigned short usMode
	);


IPMEDIALIB_API 
int 
ipm_Ping(
	int nDeviceHandle, 
	IPM_PING_PARM *pPingParameter,
   IPM_PING_INFO *pPingInfo,
   unsigned short usMode
	);

/*  ipm_SetRemoteMediaInfo is replaced by ipm_StartMedia.
	This function is being maintained for backward compatibility. */
IPMEDIALIB_API 
int 
ipm_SetRemoteMediaInfo(
	int nDeviceHandle, 
	IPM_MEDIA_INFO* pMediaInfo,
	eIPM_DATA_DIRECTION eDirection,
	unsigned short usMode
	);

IPMEDIALIB_API 
int 
ipm_SecurityGenMasterKeys(
	int nDeviceHandle, 
	IPM_SECURITY_KEY	*pKeys,
	unsigned short usNumKeys,
	unsigned short usMode
	);

/* > JAF CR10803 */
/*
 * RTP encapsulation protocol support (NBUP, etc)
 */
IPMEDIALIB_API 
int 
ipm_InitSend(
	int a_nDeviceHandle,
	const IPM_INIT_SEND *a_pInitInfo
	);

IPMEDIALIB_API 
int 
ipm_InitResponseSend(
	int a_nDeviceHandle,
	const IPM_INIT_RESPONSE *a_pInitRsp
	);

/* < JAF CR10803 */

IPMEDIALIB_API 
int 
ipm_set_lasterr(int uDeviceHandle, 
					 long lErrorCode);

#if defined(__cplusplus)
}
#endif

#else /*(defined(__BORLAND__) || defined(__cplusplus) || defined(__STDC__))*/

IPMEDIALIB_API int ipm_Close();
IPMEDIALIB_API int ipm_DisableEvents();
IPMEDIALIB_API int ipm_EnableEvents();
IPMEDIALIB_API int ipm_GetCTInfo();
IPMEDIALIB_API int ipm_GetLocalMediaInfo();
IPMEDIALIB_API int ipm_GetParm();
IPMEDIALIB_API int ipm_GetQoSAlarmStatus();
IPMEDIALIB_API int ipm_GetQoSThreshold();
IPMEDIALIB_API int ipm_GetSessionInfo();
IPMEDIALIB_API int ipm_GetXmitSlot();
IPMEDIALIB_API int ipm_Listen();
IPMEDIALIB_API int ipm_Open();
IPMEDIALIB_API int ipm_ReceiveDigits();
IPMEDIALIB_API int ipm_ResetQoSAlarmStatus();
IPMEDIALIB_API int ipm_SendDigits();
IPMEDIALIB_API int ipm_SendRFC2833SignalIDToIP();
IPMEDIALIB_API int ipm_SetParm();
IPMEDIALIB_API int ipm_SetQoSThreshold();
IPMEDIALIB_API int ipm_StartMedia();
IPMEDIALIB_API int ipm_ModifyMedia();
IPMEDIALIB_API int ipm_Stop();
IPMEDIALIB_API int ipm_UnListen();
IPMEDIALIB_API int ipm_Ping();
IPMEDIALIB_API int ipm_SecurityGenMasterKeys();
IPMEDIALIB_API int ipm_set_lasterr();
IPMEDIALIB_API int ipm_GetCapabilities();

/*  ipm_SetRemoteMediaInfo is replaced by ipm_StartMedia.
	This function is being maintained for backward compatibility. */
IPMEDIALIB_API int ipm_SetRemoteMediaInfo();

#endif  /*(defined(__BORLAND__) || defined(__cplusplus) || defined(__STDC__))*/

#define IPMEV_SET_REMOTE_MEDIA_INFO IPMEV_STARTMEDIA
#define STOP_REMOTEMEDIA STOP_MEDIA

#define MEDIATYPE_REMOTE_RTP_INFO	MEDIATYPE_AUDIO_REMOTE_RTP_INFO
#define MEDIATYPE_LOCAL_RTP_INFO	MEDIATYPE_AUDIO_LOCAL_RTP_INFO       
#define MEDIATYPE_REMOTE_RTCP_INFO	MEDIATYPE_AUDIO_REMOTE_RTCP_INFO     
#define MEDIATYPE_LOCAL_RTCP_INFO	MEDIATYPE_AUDIO_LOCAL_RTCP_INFO      
#define MEDIATYPE_REMOTE_CODER_INFO	MEDIATYPE_AUDIO_REMOTE_CODER_INFO    
#define MEDIATYPE_LOCAL_CODER_INFO	MEDIATYPE_AUDIO_LOCAL_CODER_INFO
#define CoderInfo					AudioCoderInfo
#endif /*__IPMLIB_H__*/
