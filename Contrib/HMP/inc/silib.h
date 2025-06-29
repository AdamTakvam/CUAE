/********************************************************************************
 * Copyright (c) 1990-2004 Intel Corporation
 * All Rights Reserved.  All names, products, and services mentioned herein 
 * are the trademarks or registered trademarks of their respective organizations 
 * and are the sole property of their respective owners
 ********************************************************************************/

/******************************************************************************** 
 * FILENAME:    silib.h
 * DESCRIPTION: Station Interface Library header file 
 ********************************************************************************/

#ifndef __SILIB_H__
#define __SILIB_H__

#define SI_SUCCESS							(0)
#define SI_FAILURE							(-1)

/* Synchronization mode:
 */
typedef enum
{
	Enum_SyncMode,
	Enum_AsyncMode
} TEnumSyncMode;


/*
 * Event Defines
 */
typedef enum
{
    Enum_SiEvInvalid						= 0,

    Enum_SiEvStartSuccess					= 1,

    Enum_SiEvOpenSuccess					= 2,
    Enum_SiEvSetRingerSuccess				= 3,
    Enum_SiEvGetRingerSuccess				= 4,
    Enum_SiEvSendAlertSuccess	 			= 5,
    Enum_SiEvSetVolumeSuccess				= 6,
    Enum_SiEvGetVolumeSuccess				= 7,
    Enum_SiEvSetSensitivitySuccess			= 8,
    Enum_SiEvGetSensitivitySuccess			= 9,
    Enum_SiEvSetIndicatorSuccess			= 10,
    Enum_SiEvGetKeyStateSuccess				= 11,
    Enum_SiEvGetSoftKeyTextSuccess			= 12,
    Enum_SiEvSetSoftKeyTextSuccess			= 13,
    Enum_SiEvSetLocalAudioRouteSuccess		= 14,
    Enum_SiEvGetLocalAudioRouteSuccess		= 15,
    Enum_SiEvDisplayClearSuccess			= 16,
    Enum_SiEvSetDisplayActivePageSuccess	= 17,
    Enum_SiEvSetDisplayTextSuccess			= 18,
    Enum_SiEvGetDisplayTextSuccess			= 19,
    Enum_SiEvGetDisplayCursorPosSuccess		= 20,
    Enum_SiEvSetCallerIdSuccess				= 21,
    Enum_SiEvSetParmSuccess					= 22,
    Enum_SiEvGetParmSuccess					= 23,
    Enum_SiEvSetStationStateSuccess			= 24,
    Enum_SiEvGetStationStateSuccess			= 25,
    Enum_SiEvListenSuccess					= 26,
    Enum_SiEvUnlistenSuccess				= 27,
    Enum_SiEvSetCallTimerSuccess			= 28,
    Enum_SiEvGetIndicatorSuccess			= 29,
    Enum_SiEvEndSuccess						= 30,

    Enum_SiEvStartFail						= 31,
    Enum_SiEvOpenFail						= 32,
    Enum_SiEvSetRingerFail					= 33,
    Enum_SiEvGetRingerFail					= 34,
    Enum_SiEvSendAlertFail					= 35,
    Enum_SiEvSetVolumeFail					= 36,
    Enum_SiEvGetVolumeFail					= 37,
    Enum_SiEvSetSensitivityFail				= 38,
    Enum_SiEvGetSensitivityFail				= 39,
    Enum_SiEvSetIndicatorFail				= 40,
    Enum_SiEvGetKeyStateFail				= 41,
    Enum_SiEvGetSoftKeyTextFail				= 42,
    Enum_SiEvSetSoftKeyTextFail				= 43,
    Enum_SiEvSetLocalAudioRouteFail			= 44,
    Enum_SiEvGetLocalAudioRouteFail			= 45,
    Enum_SiEvDisplayClearFail				= 46,
    Enum_SiEvSetDisplayActivePageFail		= 47,
    Enum_SiEvSetDisplayTextFail				= 48,
    Enum_SiEvGetDisplayTextFail				= 49,
    Enum_SiEvGetDisplayCursorPosFail		= 50,
    Enum_SiEvSetCallerIdFail				= 51,
    Enum_SiEvSetParmFail					= 52,
    Enum_SiEvGetParmFail					= 53,
    Enum_SiEvSetStationStateFail			= 54,
    Enum_SiEvGetStationStateFail			= 55,
    Enum_SiEvListenFail						= 56,
    Enum_SiEvUnlistenFail					= 57,
    Enum_SiEvSetCallTimerFail				= 58,
    Enum_SiEvGetIndicatorFail				= 59,
    Enum_SiEvEndFail						= 60,

    Enum_SiEvStartUnsolicited				= 61,
    Enum_SiEvKeyState						= 62,
    Enum_SiEvDeviceServiceIn				= 63,
    Enum_SiEvDeviceServiceOut				= 64,
    Enum_SiEvEndUnsolicited					= 65,

    Enum_SiEvUser							= 1000,

} TEnumSiEventId;

typedef enum
{
	Enum_SiEventStateDisable	= 0,
	Enum_SiEventStateEnable		= 1,
} TEnumSiEventState;

/* 
 *	SI Capabilities Types 
 */
typedef enum 
{
	Enum_SiCapabilitiesInvalid = 0,
	Enum_SiCapabilitiesMain,					/* retrieves TSSiCapabilitiesMain */
	Enum_SiCapabilitiesKeys,					/* retrieves array of TSSiCapabilitiesKey */
	Enum_SiCapabilitiesIndicators,				/* retrieves array of TSSiCapabilitiesIndicator */
	Enum_SiCapabilitiesVolumeDevices,			/* retrieves array of TSSiCapabilitiesVolumeDevice */
	Enum_SiCapabilitiesSensitivityDevices,		/* retrieves array of TSSiCapabilitiesSensitivityDevice */
	Enum_SiCapabilitiesAudioDevices,			/* retrieves array of TSSiCapabilitiesAudioDevice */
	Enum_SiCapabilitiesSoftKeys					/* retrieves array of TSSiCapabilitiesSoftKey */
} TEnumSiCapabilities;

/*
 * Function Capabilities 
 */
typedef enum
{
	Enum_SiFunction1Invalid					= 0,
	Enum_SiFunction1None					= 0x80000000,	
	Enum_SiFunction1Open					= 0x00000001,
	Enum_SiFunction1Close					= 0x00000002,
	Enum_SiFunction1SetRinger				= 0x00000004,
	Enum_SiFunction1GetRinger				= 0x00000008,
	Enum_SiFunction1SendAlert				= 0x00000010,
	Enum_SiFunction1SetVolume				= 0x00000020,
	Enum_SiFunction1GetVolume				= 0x00000040,
	Enum_SiFunction1SetSensitivity			= 0x00000080,
	Enum_SiFunction1GetSensitivity			= 0x00000100,
	Enum_SiFunction1SetIndicator			= 0x00000200,
	Enum_SiFunction1GetIndicator			= 0x00000400,
	Enum_SiFunction1GetKeyState				= 0x00000800,
	Enum_SiFunction1SetLocalAudioRoute		= 0x00001000,
	Enum_SiFunction1GetLocalAudioRoute		= 0x00002000,
	Enum_SiFunction1DisplayClear			= 0x00004000,
	Enum_SiFunction1SetDisplayText			= 0x00008000,
	Enum_SiFunction1GetDisplayText			= 0x00010000,
	Enum_SiFunction1SetParm					= 0x00020000,
	Enum_SiFunction1GetParm					= 0x00040000,
	Enum_SiFunction1GetStationState			= 0x00080000,
	Enum_SiFunction1SetStationState			= 0x00100000,
	Enum_SiFunction1SetCallerId				= 0x00200000,
	Enum_SiFunction1SetCallTimer			= 0x00400000,
	Enum_SiFunction1SetSoftKeyText			= 0x00800000,
	Enum_SiFunction1GetSoftKeyText			= 0x01000000,
	Enum_SiFunction1SetDisplayActivePage	= 0x02000000,
	Enum_SiFunction1GetDisplayCursorPos		= 0x04000000,
} TEnumSiFunction1;

typedef enum
{
	Enum_SiFunction2Invalid				= 0,
	Enum_SiFunction2None				= 0x80000000,
} TEnumSiFunction2;

/* 
 * Alert Types
 */
typedef enum
{
	Enum_SiAlertTypeInvalid		= 0,
	Enum_SiAlertTypeNone		= 0x80000000,
	Enum_SiAlertTypeBeep		= 0x00000001,
	Enum_SiAlertTypeCallWaiting	= 0x00000002,
	Enum_SiAlertTypeOther1		= 0x00100000,
	Enum_SiAlertTypeOther2		= 0x00200000,
	Enum_SiAlertTypeOther3		= 0x00400000,
} TEnumSiAlertType;

/* 
 * Volume Control Devices
 */
typedef enum
{
    Enum_SiVolumeDeviceInvalid				= 0,
    Enum_SiVolumeDeviceNone					= 0x80000000,
	Enum_SiVolumeDeviceRinger				= 0x00000001,
	Enum_SiVolumeDeviceHandsetSpeaker		= 0x00000002,
	Enum_SiVolumeDeviceIntercomSpeaker		= 0x00000004,
	Enum_SiVolumeDeviceSpeakerphoneSpeaker	= 0x00000008,
	Enum_SiVolumeDeviceHeadsetSpeaker		= 0x00000010,
} TEnumSiVolumeDevice;

/* 
 * Sensitivity Control Devices
 */
typedef enum
{
    Enum_SiSensitivityDeviceInvalid			= 0,
    Enum_SiSensitivityDeviceNone			= 0x80000000,
	Enum_SiSensitivityDeviceHandsetMic		= 0x00000001,
	Enum_SiSensitivityDeviceSpeakerphoneMic	= 0x00000002,
	Enum_SiSensitivityDeviceHeadsetMic		= 0x00000004,
} TEnumSiSensitivityDevice;

/* 
 * Sub-Indicators
 */
typedef enum
{
    Enum_SiSubIndicatorInvalid		= 0,
    Enum_SiSubIndicatorNone			= 0x80000000,	/* may not be combined with other TEnumSiSubIndicator values */
	Enum_SiSubIndicatorIndicator	= 0x00000001,	/* indicator (may be multiples on each indicator) */
	Enum_SiSubIndicatorSelector		= 0x00000002,	/* selector part (used as an indicator selector) */
} TEnumSiSubIndicator;

/* 
 * Indicator Types
 */
typedef enum
{
    Enum_SiIndicatorIdInvalid		= 0,
	Enum_SiIndicatorIdNone			= 0x80000000,
	
   	Enum_SiIndicatorIdHookswitch	= (0x0011),	/* hook-switch */
    Enum_SiIndicatorIdHold			= (0x0012),	/* hold */
    Enum_SiIndicatorIdConference	= (0x0013),	/* conference */
    Enum_SiIndicatorIdTransfer		= (0x0014),	/* transfer */

    Enum_SiIndicatorIdLine1			= (0x0015),	/* line indicators */
    Enum_SiIndicatorIdLine999		= (0x03FB),	
    Enum_SiIndicatorIdFunc1			= (0x03FC),	/* assignable function/feature keys */
    Enum_SiIndicatorIdFunc999		= (0x07E2),	

    Enum_SiIndicatorIdSpeaker		= (0x0800),	/* speaker */
    Enum_SiIndicatorIdMsg			= (0x0801),	/* message */
    Enum_SiIndicatorIdItcm			= (0x0802),	/* intercom */
    Enum_SiIndicatorIdRelease		= (0x0803),	/* drop/release */

    Enum_SiIndicatorIdSoft1			= (0x0900),	/* soft-keys */
    Enum_SiIndicatorIdSoft999		= (0x0CE6),	

    Enum_SiIndicatorIdMute			= (0x0D00),	/* mute */
    Enum_SiIndicatorIdVolumeUp		= (0x0D01),	/* volume up */
    Enum_SiIndicatorIdVolumeDown	= (0x0D02),	/* volume down */

    Enum_SiIndicatorIdOther1		= (0x00100000),	/* extensions */
    Enum_SiIndicatorIdOther2		= (0x00200000),	/* extensions */
    Enum_SiIndicatorIdOther3		= (0x00400000),	/* extensions */
} TEnumSiIndicatorId;

/* 
 * Indicator Colors
 */
typedef enum
{
	Enum_SiIndicatorColorInvalid	= 0,
	Enum_SiIndicatorColorNone		= 0x80000000,
	Enum_SiIndicatorColorRed		= 0x00000001,
	Enum_SiIndicatorColorGreen		= 0x00000002,
	Enum_SiIndicatorColorYellow		= 0x00000004,
	Enum_SiIndicatorColorOrange		= 0x00000008,
	Enum_SiIndicatorColorBlue		= 0x00000010,
	Enum_SiIndicatorColorBlack		= 0x00000020,
	Enum_SiIndicatorColorOther1		= 0x00100000,
	Enum_SiIndicatorColorOther2		= 0x00200000,
	Enum_SiIndicatorColorOther3		= 0x00400000,
} TEnumSiIndicatorColor;

/*
 * Indicator States
 */
typedef enum
{
	Enum_SiIndicatorStateInvalid	= 0,
	Enum_SiIndicatorStateNone		= 0x80000000,
	Enum_SiIndicatorStateOff		= 0x00000001,
	Enum_SiIndicatorStateSteady		= 0x00000002,
	Enum_SiIndicatorStateBlink		= 0x00000004,
	Enum_SiIndicatorStateFastBlink	= 0x00000008,
	Enum_SiIndicatorStateSlowBlink	= 0x00000010,
	Enum_SiIndicatorStateOther1		= 0x00100000,
	Enum_SiIndicatorStateOther2		= 0x00200000,
	Enum_SiIndicatorStateOther3		= 0x00400000,
} TEnumSiIndicatorState;


/*
 * Key Types
 */
typedef enum
{
    Enum_SiKeyIdInvalid		= 0,
    Enum_SiKeyIdNone		= 0x80000000,
	Enum_SiKeyId0			= (0x0001),	/* key-pad '0' */
	Enum_SiKeyId1			= (0x0002),	/* key-pad '1' */
	Enum_SiKeyId2			= (0x0003),	/* key-pad '2' */
	Enum_SiKeyId3			= (0x0004),	/* key-pad '3' */
	Enum_SiKeyId4			= (0x0005),	/* key-pad '4' */
	Enum_SiKeyId5			= (0x0006),	/* key-pad '5' */
	Enum_SiKeyId6			= (0x0007),	/* key-pad '6' */
	Enum_SiKeyId7			= (0x0008),	/* key-pad '7' */
	Enum_SiKeyId8			= (0x0009),	/* key-pad '8' */
	Enum_SiKeyId9			= (0x000A),	/* key-pad '9' */
	Enum_SiKeyIdStar		= (0x000B),	/* key-pad '*' */
	Enum_SiKeyIdPound		= (0x000C),	/* key-pad '#' */
	Enum_SiKeyIdA			= (0x000D),	/* key-pad 'A' or 'a' */
	Enum_SiKeyIdB			= (0x000E),	/* key-pad 'B' or 'b' */
	Enum_SiKeyIdC			= (0x000F),	/* key-pad 'C' or 'c' */
	Enum_SiKeyIdD			= (0x0010),	/* key-pad 'D' or 'd' */

	Enum_SiKeyIdHookswitch	= (0x0011),	/* hook-switch */
	Enum_SiKeyIdHold		= (0x0012),	/* hold */
	Enum_SiKeyIdConference	= (0x0013),	/* conference */
	Enum_SiKeyIdTransfer	= (0x0014),	/* transfer */

	Enum_SiKeyIdLine1		= (0x0015),	/* line keys */
	Enum_SiKeyIdLine999		= (0x03FB),	
	Enum_SiKeyIdFunc1		= (0x03FC),	/* assignable function/feature keys */
	Enum_SiKeyIdFunc999		= (0x07E2),

	Enum_SiKeyIdSpeaker		= (0x0800),	/* speaker */
	Enum_SiKeyIdMsg			= (0x0801),	/* message*/
	Enum_SiKeyIdItcm		= (0x0802),	/* intercom*/
	Enum_SiKeyIdRelease		= (0x0803),	/* drop/release */

	Enum_SiKeyIdSoft1		= (0x0900),	/* soft-keys */
	Enum_SiKeyIdSoft999		= (0x0CE6),	/* soft-keys */

    Enum_SiKeyIdMute		= (0x0D00),	/* mute */
    Enum_SiKeyIdVolumeUp	= (0x0D01),	/* volume up */
    Enum_SiKeyIdVolumeDown	= (0x0D02),	/* volume down */

    Enum_SiKeyIdOther1		= (0x0010000),	/* extensions*/
    Enum_SiKeyIdOther2		= (0x0020000),	/* extensions*/
    Enum_SiKeyIdOther3		= (0x0040000),	/* extensions*/
} TEnumSiKeyId;

/*
 * Key States
 */
typedef enum
{
	Enum_SiKeyStateInvalid	= 0,
	Enum_SiKeyStateNone		= 0x80000000,
	Enum_SiKeyStateUp		= 0x00000001,
	Enum_SiKeyStateDown		= 0x00000002,
} TEnumSiKeyState;

/* 
 * Local Audio Modes
 */
typedef enum
{
    Enum_SiAudioModeInvalid					= (0),
    Enum_SiAudioModeNone					= (0x80000000),
	Enum_SiAudioModeBidirection				= (0x0001),
	Enum_SiAudioModeMuteFromStation			= (0x0002),
} TEnumSiAudioMode;

/* 
 * Local Audio Route Devices
 */
typedef enum
{
    Enum_SiAudioDeviceInvalid				= (0),
    Enum_SiAudioDeviceNone					= (0x80000000),
	Enum_SiAudioDeviceHandset				= (0x0001),
	Enum_SiAudioDeviceSpeakerphone			= (0x0002),
	Enum_SiAudioDeviceHeadset				= (0x0004),
} TEnumSiAudioDevice;

/*
 * Display Text Attributes
 */
typedef enum
{
	Enum_SiTextDisplayAttribInvalid		= 0,
	Enum_SiTextDisplayAttribNone		= 0x80000000,
	Enum_SiTextDisplayAttribBlink		= 0x00000001,
	Enum_SiTextDisplayAttribInvert		= 0x00000002,
	Enum_SiTextDisplayAttribUnderline	= 0x00000004,
} TEnumSiTextDisplayAttrib;

/*
 * Call Timer Actions
 */
typedef enum
{
	Enum_SiCallTimerActionInvalid	= 0,
	Enum_SiCallTimerActionNone		= 0x80000000,	
	Enum_SiCallTimerActionStart		= 0x00000001,
	Enum_SiCallTimerActionStop		= 0x00000002,
	Enum_SiCallTimerActionRestart	= 0x00000004,
} TEnumSiCallTimerAction;

/*
 * Call Timer Formats
 */
typedef enum
{
	Enum_SiCallTimerFormatInvalid	= 0,
	Enum_SiCallTimerFormatNone		= 0x80000000,
	Enum_SiCallTimerFormatDefault	= 0x00000001,	/* "HH:MM:SS" - 12 hour counter   */
	Enum_SiCallTimerFormatHHMMSS	= 0x00000002,	/* "HH:MM:SS" - 24 hour counter   */
	Enum_SiCallTimerFormatMSS		= 0x00000004,	/* "MM:SS"    - 60 minute counter */
} TEnumSiCallTimerFormat;

/*
 * Call Timer Locations
 */
typedef enum
{
	Enum_SiCallTimerLocationInvalid		= 0,
	Enum_SiCallTimerLocationNone		= 0x80000000,
	Enum_SiCallTimerLocationTopLeft		= 0x00000001,	
	Enum_SiCallTimerLocationTopRight	= 0x00000002,	
	Enum_SiCallTimerLocationBottomLeft	= 0x00000004,	
	Enum_SiCallTimerLocationBottomRight	= 0x00000008,	
} TEnumSiCallTimerLocation;

/*
 * Set/Get Parms
 */
typedef enum
{
	Enum_SiParmInvalid		= 0,
	Enum_SiParmNone			= 0x80000000,
	Enum_SiParmDigitMode	= 0x00000001,
	Enum_SiParmUserContext	= 0x00000002,
} TEnumSiParm;

/*
 * Dial Digit Modes
 */
typedef enum
{
	Enum_SiDigitModeInvalid		= 0,
	Enum_SiDigitModeNone		= 0x80000000,
	Enum_SiDigitModeDtmf		= 0x00000001,
	Enum_SiDigitModeOutOfBand	= 0x00000002,
} TEnumSiDigitMode;

/*
 * Station States
 */
typedef enum
{
	Enum_SiStationStateInvalid		= 0,
	Enum_SiStationStateNone			= 0x80000000,
	Enum_SiStationStateEnabled		= 0x00000001,
	Enum_SiStationStateDisabled		= 0x00000002,
} TEnumSiStationState;

typedef enum
{
	Enum_SiServiceStateInvalid		= 0,
	Enum_SiServiceStateNone			= 0x80000000,
	Enum_SiServiceStateIn			= 0x00000001,
	Enum_SiServiceStateOut			= 0x00000002,
} TEnumSiServiceState;

typedef enum
{
	Enum_SiErrorInvalid	= 0,
	Enum_SiErrorInvLen,
	Enum_SiErrorBadBrd,
	Enum_SiErrorBadExtTs,
	Enum_SiErrorBadLclTs,
	Enum_SiErrorBadType,
	Enum_SiErrorBadVal,
	Enum_SiErrorFieldOutOfRange,
	Enum_SiErrorFwErr,
	Enum_SiErrorInvBd,
	Enum_SiErrorInvColor,
	Enum_SiErrorInvKeyId,
	Enum_SiErrorInvIndicatorId,
	Enum_SiErrorInvSubIndicator,
	Enum_SiErrorInvIndicatorState,
	Enum_SiErrorInvParm,
	Enum_SiErrorInvName,
	Enum_SiErrorInvState,
	Enum_SiErrorInvTs,
	Enum_SiErrorNoCT,
	Enum_SiErrorNoEvents,
	Enum_SiErrorNoMem,
	Enum_SiErrorNotSupported,
	Enum_SiErrorSystem,
	Enum_SiErrorTmoErr,
	Enum_SiErrorCallTimerEnabled,
	Enum_SiErrorSysNotStarted,
	Enum_SiErrorInvEventState,
	Enum_SiErrorInvEventId,
} TEnumSiError;

/*
 * Ensure all structures are packed.
 */
#pragma pack(1)

/*
 * Ringer Control
 */
typedef struct SSiCapabilitiesRinger
{
	int							iNumCadences;			/* num ring cadences (including 0-off) */
	int							iNumTones;				/* num ring tones */
} TSSiCapabilitiesRinger;

typedef struct SSiRingerState
{
	int							iCadence;	  			/* ring cadence to set (0-off) */
	int							iTone;		  			/* ring tone */
} TSSiRingerState;

/*
 * Volume Control
 */
typedef struct SSiCapabilitiesVolumeDevice
{
	TEnumSiVolumeDevice			eDevice;				/* Enum_SiVolumeDeviceXXX */
	int							iMinVolume;				/* min device volume */
	int							iMaxVolume;				/* max device volume */
} TSSiCapabilitiesVolumeDevice;

typedef struct SSiVolumeDeviceState
{
	TEnumSiVolumeDevice			eDevice;				/* specifies the volume device */
	int							iVolume;				/* specifies the volume level of the device */
} TSSiVolumeDeviceState;

/*
 * Sensitivity Control
 */
typedef struct SSiCapabilitiesSensitivityDevice
{
	TEnumSiSensitivityDevice	eDevice;				/* Enum_SiSensitivityDeviceXXX */
	int							iMinSensitivity;		/* min device sensitivity */
	int							iMaxSensitivity;		/* max device sensitivity */
} TSSiCapabilitiesSensitivityDevice;

typedef struct SSiSensitivityDeviceState
{
	TEnumSiSensitivityDevice	eDevice;				/* Enum_SiSensitivityDeviceXXX */
	int							iSensitivity;			/* device sensitivity */
} TSSiSensitivityDeviceState;

/*
 * Indicator Control
 */
typedef struct SSiCapabiltiesSubIndicator
{
	TEnumSiSubIndicator			eSubIndicator;		/* Enum_SiSubIndicatorXXX */
	unsigned long				ulStates; 			/* Enum_SiIndicatorStateXXX flags */
	unsigned long				ulColors; 			/* Enum_SiIndicatorColorXXX flags */
} TSSiCapabiltiesSubIndicator;

#define SI_MAXNUM_SUBINDICATORS	4
typedef struct SSiCapabilitiesIndicator
{
	unsigned long				ulIndicatorId;		/* Enum_SiIndicatorIdXXX */
	int							iNumSubIndicators;	/* number of sub-indicators */
	TSSiCapabiltiesSubIndicator	capSubIndicator[SI_MAXNUM_SUBINDICATORS];	/* indicator device capabilities */
} TSSiCapabilitiesIndicator;

typedef struct SSiIndicatorState
{
	unsigned long				ulIndicatorId;		/* Enum_SiIndicatorIdXXX */
	TEnumSiSubIndicator			eSubIndicator;		/* Enum_SiSubIndicatorXXX */
	TEnumSiIndicatorState		eState; 			/* Enum_SiIndicatorStateXXX */
	TEnumSiIndicatorColor		eColor; 			/* Enum_SiIndicatorColorXXX */
} TSSiIndicatorState;

/*
 * Key Control
 */
typedef struct SSiCapabilitiesKey
{
	unsigned long				ulKeyId;				/* Enum_SiKeyIdXXX */
	unsigned long				ulStates;				/* Enum_SiKeyStateXXX flags */
} TSSiCapabilitiesKey;

typedef struct SSiKeyState
{
	unsigned long				ulKeyId;				/* Enum_SiKeyIdXXX */
	TEnumSiKeyState				eState;					/* Enum_SiKeyState*/
	int							iMsDuration;			/* ms duration of down (only on Enum_SiKeyStateUp) */
} TSSiKeyState;

/*
 * Soft-Key Control
 */
typedef struct SSiCapabilitiesSoftKey
{
	unsigned long				ulMaxTextLen;			/* maximum text length */
} TSSiCapabilitiesSoftKey;

typedef struct SSiSoftKeyText
{
	unsigned long				ulKeyId;				/* Enum_SiKeyIdXXX */
	int							iLen;					/* length of pszText buffer */
	char*						pszText;				/* pointer to application buffer of soft-key text */
} TSSiSoftKeyText;

/*
 * Media Device Control
 */
typedef struct SSiCapabilitiesAudioDevice
{
	TEnumSiAudioDevice			eDevice;				/* TEnumSiAudioDevice device type */
	unsigned long				ulModes;				/* TEnumSiAudioMode flags that can be controlled */
} TSSiCapabilitiesAudioDevice;

typedef struct SSiAudioRoute
{
	TEnumSiAudioDevice			eDevice;				/* Enum_SiAudioDeviceXXX device type */
	TEnumSiAudioMode			eMode;					/* Enum_SiAudioModeXXX audio route mode */
} TSSiAudioRoute;

/*
 * Text Display Capabilities
 */
typedef struct SSiCapabilitiesTextDisplay
{
	int					iNumPages;						/* num pages */
	int					iNumRows;						/* num rows */
	int					iNumCols;						/* num columns */
	unsigned long		ulAttributes;					/* Enum_SiTextDisplayAttribXXX flags */

	unsigned long		ulCallTimerActions;				/* call timer actions supported */
	unsigned long		ulCallTimerFormats;				/* call timer formats supported */
	unsigned long		ulCallTimerLocations;			/* call timer location in display */
} TSSiCapabilitiesTextDisplay;

typedef struct SSiCallTimerState
{
	TEnumSiCallTimerAction		eAction;				/* action to execute on call timer */
	TEnumSiCallTimerFormat		eFormat;				/* format (used if eAction = Enum_SiCallTimerActionStart) */
	TEnumSiCallTimerLocation	eLocation;				/* location (used if eAction = Enum_SiCallTimerActionStart ) */
	unsigned long				ulTimeSec;				/* specifies current call timer time, in seconds (0 - 86399) */
} TSSiCallTimerState;

typedef struct SSiDisplayText
{
	int					iPage;							/* display page to set/get (1-based) */
	int					iRow;							/* starting row of text to set/get (1-based) */
	int					iCol;							/* starting col of text to set/get (1-based) */
	unsigned long		ulAttribute;					/* text attribute */
	int					iLen;							/* length of pszText buffer */
	char*				pszText;						/* pointer to application buffer */
} TSSiDisplayText;

typedef struct SSiDisplayCursorPosition
{
	int					iPage;							/* display page (1-based) */
	int					iCurrentRow;					/* current row position of cursor on the page (1-based) */
	int					iCurrentCol;					/* current col position of cursor on the page (1-based) */
} TSSiDisplayCursorPosition;

/*
 * Caller-ID Control
 */
#define SI_CALLERID_TIME_LEN		16
#define SI_CALLERID_DIRECTORY_LEN	64
typedef struct SSiCallerId
{
	char						month[SI_CALLERID_TIME_LEN];		/* month */
	char						day[SI_CALLERID_TIME_LEN];			/* day of the month */
	char						hour[SI_CALLERID_TIME_LEN]; 		/* hour in local military time */
	char						minute[SI_CALLERID_TIME_LEN];		/* minutes after the hour */
	char						name[SI_CALLERID_DIRECTORY_LEN]; 	/* calling party's directory name ("O", "P") */
	char						number[SI_CALLERID_DIRECTORY_LEN];	/* calling party's directory number ("O", "P") */
} TSSiCallerId;

/*
 * Parameter Control
 */
typedef struct SSiParameter
{
	TEnumSiParm					eParameter;				/* parameter to set/retrieve */
	int							iLen;					/* length of buffer pointed to by pvData */
	const void*					pvData;					/* pointer to application buffer containing parameter structure */
} TSSiParameter;

/*
 * Dial-Digit Mode Parameter
 */
typedef struct SSiDigitMode
{
	TEnumSiDigitMode			eDigitMode;				/* Digit Mode */
} TSSiDigitMode;

/*
 * Station State Control
 */
typedef struct SSiStationState
{
	TEnumSiStationState			eState;					/* specifies if station is enabled/disabled */
	TEnumSiServiceState			eService;				/* specifies if station is in service or not */
} TSSiStationState;

/* 
 *	SI Capabilities Retrieval 
 */
#define SI_CAP_MAIN_STATIONTYPE_LEN		128
#define SI_CAP_MAIN_STATIONMODEL_LEN	128
typedef struct SSiCapabilitiesMain
{
	char						szType[SI_CAP_MAIN_STATIONTYPE_LEN];	/* station type */
	char						szModel[SI_CAP_MAIN_STATIONMODEL_LEN];	/* station model */
	unsigned long				ulStationStates;				/* TEnumSiStationState flags */
	unsigned long				ulFunctions1;					/* TEnumSiFunction1 API functions supported */
	unsigned long				ulFunctions2;					/* TEnumSiFunction2 API functions supported */
	TSSiCapabilitiesTextDisplay	DisplayCapabilities;			/* text display capabilities */
	TSSiCapabilitiesRinger		RingerCapabilities;				/* ringer capabilities */
	unsigned long				ulAlerts;						/* supported alerts (TEnumSiAlertType) */
	int							iNumKeys;						/* number of supported keys */
	int							iNumIndicators;					/* number of supported indicators */
	int							iNumVolumeDevices;				/* number of devices with volume control */
	int							iNumSensitivityDevices;			/* num sensitivity control devices */
	int							iNumAudioDevices;				/* number of media devices */
	int							iNumSoftKeys;					/* number of soft-keys */
	unsigned long				ulParameters;					/* supported parameters TEnumSiParm flags */
} TSSiCapabilitiesMain;


/*
 * Restore default structures packing.
 */
#pragma pack()

/*
 *	Useful declaration specification.
 */
#if defined(__USING_DEF_FILE__)

#ifdef DllExport
#undef DllExport
#endif

#ifdef DllImport
#undef DllImport
#endif

#ifdef DllLinkage
#undef DllLinkage
#endif

#define DllExport
#define DllImport
#define DllLinkage extern

#else	/* __USING_DEF_FILE__	*/

#ifndef DllExport
#define DllExport	__declspec( dllexport )
#endif	/*	Dllexport	*/

#ifndef DllImport
#define DllImport __declspec( dllimport )
#endif	/*	Dllimport	*/

#ifdef _SI_DLL

#ifndef  DllLinkage
#define	DllLinkage	DllExport
#endif

#else

#ifndef DllLinkage
#define DllLinkage	DllImport
#endif

#endif   /* End _SI_DLL */

#endif	/* __USING_DEF_FILE__	*/

#ifdef __CROSS_COMPAT_LIB__
#undef  DllLinkage
#define DllLinkage    
#endif


/*
/*
 * Prototypes for MSI/SC library functions.
 */
#ifdef __cplusplus
extern "C" {   // C++ func bindings to enable C funcs to be called from C++
#define extern
#endif

#if (defined(__STDC__) || defined(__cplusplus) || defined(__BORLANDC__))

#ifdef _SILIB_NOSRL_
/* apis to support stand-alone version - no SRL */
DllLinkage    int	__cdecl si_WaitEvent(const int* a_pSiDevs, int a_iHandleCnt, long a_lTimeout, long* a_plEvent, TEnumSiError* a_peError);
DllLinkage    long	__cdecl si_GetEvtDev(long a_lEvent);
DllLinkage    long	__cdecl si_GetEvtType(long a_lEvent);
DllLinkage    long	__cdecl si_GetEvtLen(long a_lEvent);
DllLinkage    void*	__cdecl si_GetEvtDataP(long a_lEvent);
DllLinkage    void* __cdecl si_GetUserContext(long a_lEvent);
DllLinkage    int	__cdecl si_PutEvent(int	a_iHandle, unsigned long a_ulEventType, long a_lEventLen, const void* a_pEventData, long a_lErrorCode);
DllLinkage    long	__cdecl SI_LASTERR(int a_iDevHandle);
DllLinkage    char* __cdecl SI_ERRMSGP(int a_iDevHandle);
DllLinkage    char* __cdecl SI_NAMEP(int a_iDevHandle);
#else
// These macro redefinitions must be left in so that we transparently support the SRL
// std attribute retrieval functionality.
#define SI_LASTERR	ATDV_LASTERR
#define SI_ERRMSGP	ATDV_ERRMSGP
#define SI_NAMEP	ATDV_NAMEP
#endif

DllLinkage    int __cdecl  si_GetChannelCount(const char* a_szBoardName, int* a_piNumChannels, TEnumSiError* a_peError);
DllLinkage    int __cdecl  si_GetBoardName(int a_iBoardIndex, char *a_pszBoardName, int* a_piBoardNameLen, TEnumSiError* a_peError);
DllLinkage    int __cdecl  si_GetBoardCount(int* a_piNumBoards, TEnumSiError* a_peError);
DllLinkage    int __cdecl  si_Open(int* a_piHandle, const char* a_szDevName, TEnumSyncMode a_eSyncMode, const void* a_pvUserContext);
DllLinkage    int __cdecl  si_Close(int a_iHandle);
DllLinkage    int __cdecl  si_GetStationCapabilities(const char* a_szDevName, TEnumSiCapabilities a_eCapability, void* a_pCapBlk, int a_iLen, TEnumSiError* a_peError);
DllLinkage	  int __cdecl  si_SetEventMask(int a_iHandle, const TEnumSiEventId* a_peEventList, int a_iNumEntries, TEnumSiEventState a_eEventEnable);
DllLinkage    int __cdecl  si_SetRinger(int a_iHandle, const TSSiRingerState* a_pRingState, TEnumSyncMode a_eSyncMode);
DllLinkage    int __cdecl  si_GetRinger(int a_iHandle, TSSiRingerState* a_pRingState, TEnumSyncMode a_eSyncMode);
DllLinkage    int __cdecl  si_SendAlert(int a_iHandle, TEnumSiAlertType a_eAlert, TEnumSyncMode a_eSyncMode);
DllLinkage    int __cdecl  si_SetVolume(int a_iHandle, TSSiVolumeDeviceState* a_pVolumeDeviceState, TEnumSyncMode a_eSyncMode);
DllLinkage    int __cdecl  si_GetVolume(int a_iHandle, TSSiVolumeDeviceState* a_pVolumeDeviceState, TEnumSyncMode a_eSyncMode);
DllLinkage    int __cdecl  si_SetSensitivity(int a_iHandle, TSSiSensitivityDeviceState* a_pSensitivityDeviceState, TEnumSyncMode a_eSyncMode);
DllLinkage    int __cdecl  si_GetSensitivity(int a_iHandle, TSSiSensitivityDeviceState* a_pSensitivityDeviceState, TEnumSyncMode a_eSyncMode);
DllLinkage    int __cdecl  si_SetIndicator(int a_iHandle, const TSSiIndicatorState* a_pIndicatorState, TEnumSyncMode a_eSyncMode);
DllLinkage    int __cdecl  si_GetIndicator(int a_iHandle, TSSiIndicatorState* a_pIndicatorState, TEnumSyncMode a_eSyncMode);
DllLinkage    int __cdecl  si_GetKeyState(int a_iHandle, TSSiKeyState* a_pKeyState, TEnumSyncMode a_eSyncMode);
DllLinkage    int __cdecl  si_SetSoftKeyText(int a_iHandle, TSSiSoftKeyText* a_pSoftKeyText, TEnumSyncMode a_eSyncMode);
DllLinkage    int __cdecl  si_GetSoftKeyText(int a_iHandle, TSSiSoftKeyText* a_pSoftKeyText, TEnumSyncMode a_eSyncMode);
DllLinkage    int __cdecl  si_SetLocalAudioRoute(int a_iHandle, TSSiAudioRoute* a_pAudioRoute, TEnumSyncMode a_eSyncMode);
DllLinkage    int __cdecl  si_GetLocalAudioRoute(int a_iHandle, TSSiAudioRoute* a_pAudioRoute, TEnumSyncMode a_eSyncMode);
DllLinkage    int __cdecl  si_DisplayClear(int a_iHandle, int a_iPage, int a_iRow, TEnumSyncMode a_eSyncMode);
DllLinkage    int __cdecl  si_SetDisplayActivePage(int a_iHandle, int a_iPage, TEnumSyncMode a_eSyncMode);
DllLinkage    int __cdecl  si_SetDisplayText(int a_iHandle, TSSiDisplayText* a_pDisplayText, TEnumSyncMode a_eSyncMode);
DllLinkage    int __cdecl  si_GetDisplayText(int a_iHandle, TSSiDisplayText* a_pDisplayText, TEnumSyncMode a_eSyncMode);
DllLinkage    int __cdecl  si_GetDisplayCursorPos(int a_iHandle, TSSiDisplayCursorPosition* a_pCursorPosition, TEnumSyncMode a_eSyncMode);
DllLinkage    int __cdecl  si_SetCallTimer(int a_iHandle, TSSiCallTimerState* a_pCallTimerState, TEnumSyncMode a_eSyncMode);
DllLinkage    int __cdecl  si_SetCallerId(int a_iHandle, const TSSiCallerId* a_pCallerId, TEnumSyncMode a_eSyncMode);
DllLinkage    int __cdecl  si_SetParm(int a_iHandle, TSSiParameter* a_pParameter, TEnumSyncMode a_eSyncMode);
DllLinkage    int __cdecl  si_GetParm(int a_iHandle, TSSiParameter* a_pParameter, TEnumSyncMode a_eSyncMode);
DllLinkage    int __cdecl  si_SetStationState(int a_iHandle, TSSiStationState* a_pStationState, TEnumSyncMode a_eSyncMode);
DllLinkage    int __cdecl  si_GetStationState(int a_iHandle, TSSiStationState* a_pStationState, TEnumSyncMode a_eSyncMode);
DllLinkage    int __cdecl  si_GetXmtSlot(int a_iHandle, unsigned long* a_pulTimeSlot);
DllLinkage    int __cdecl  si_Listen(int a_iHandle, unsigned long a_ulTimeSlot, TEnumSyncMode a_eSyncMode);
DllLinkage    int __cdecl  si_UnListen(int a_iHandle, TEnumSyncMode a_eSyncMode);

#else

#ifdef _SILIB_NOSRL_
/* apis to support stand-alone version - no SRL */
DllLinkage    int	__cdecl si_WaitEvent();
DllLinkage    long	__cdecl si_GetEvtDev();
DllLinkage    long	__cdecl si_GetEvtType();
DllLinkage    long	__cdecl si_GetEvtLen();
DllLinkage    void* __cdecl si_GetEvtDataP();
DllLinkage    void* __cdecl si_GetUserContext();
DllLinkage    int	__cdecl si_PutEvent();
DllLinkage    long	__cdecl SI_LASTERR();
DllLinkage    char* __cdecl SI_ERRMSGP();
DllLinkage    char* __cdecl SI_NAMEP();
#else
// These macro redefinitions must be left in so that we transparently support the SRL
// std attribute retrieval functionality.
#define SI_LASTERR	ATDV_LASTERR
#define SI_ERRMSGP	ATDV_ERRMSGP
#define SI_NAMEP	ATDV_NAMEP
#endif

DllLinkage    int __cdecl  si_GetChannelCount();
DllLinkage    int __cdecl  si_GetBoardName();
DllLinkage    int __cdecl  si_GetBoardCount();
DllLinkage    int __cdecl  si_Open();
DllLinkage    int __cdecl  si_Close();
DllLinkage    int __cdecl  si_GetStationCapabilities();
DllLinkage	  int __cdecl  si_SetEventMask();
DllLinkage    int __cdecl  si_SetRinger();
DllLinkage    int __cdecl  si_GetRinger();
DllLinkage    int __cdecl  si_SendAlert();
DllLinkage    int __cdecl  si_SetVolume();
DllLinkage    int __cdecl  si_GetVolume();
DllLinkage    int __cdecl  si_SetSensitivity();
DllLinkage    int __cdecl  si_GetSensitivity();
DllLinkage    int __cdecl  si_SetIndicator();
DllLinkage    int __cdecl  si_GetIndicator();
DllLinkage    int __cdecl  si_GetKeyState();
DllLinkage    int __cdecl  si_GetSoftKeyText();
DllLinkage    int __cdecl  si_SetSoftKeyText();
DllLinkage    int __cdecl  si_SetLocalAudioRoute();
DllLinkage    int __cdecl  si_GetLocalAudioRoute();
DllLinkage    int __cdecl  si_DisplayClear();
DllLinkage    int __cdecl  si_SetDisplayActivePage();
DllLinkage    int __cdecl  si_SetDisplayText();
DllLinkage    int __cdecl  si_GetDisplayText();
DllLinkage    int __cdecl  si_GetDisplayCursorPos();
DllLinkage    int __cdecl  si_SetCallTimer();
DllLinkage    int __cdecl  si_SetCallerId();
DllLinkage    int __cdecl  si_SetParm();
DllLinkage    int __cdecl  si_GetParm();
DllLinkage    int __cdecl  si_SetStationState();
DllLinkage    int __cdecl  si_GetStationState();
DllLinkage    int __cdecl  si_GetXmtSlot();
DllLinkage    int __cdecl  si_Listen();
DllLinkage    int __cdecl  si_UnListen();
#endif

#ifdef __cplusplus
}
#undef extern
#endif

#endif /* __SILIB_H__ */


