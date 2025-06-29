#ifndef	__BRD_STAT_H__
#define	__BRD_STAT_H__

//	Change History
//	==============
//
//	25Dec97/J.D.Patel
//	- original entry	
//	

#if	(!defined(NO_BOARD_STATUS) || !defined(NO_HOTPLUG_HOTSWAP))

// Board status requests supported
#define	BOARD_STATUS_ALL					0xffff	// report all status events
#define	BOARD_STATUS_SHUTDOWN				0x0001
#define	BOARD_STATUS_STARTED				0x0002
#define	BOARD_STATUS_QUIESCED				0x0004
#define	BOARD_STATUS_NO_RESPONSE			0x0008
#define	BOARD_STATUS_ERROR					0x0010
#define	BOARD_STATUS_RESET					0x0020
#define	BOARD_STATUS_ERROR_ACCESS			0x0040	//	ThakkarD : March 2001 : SRAM corruption

// for Hot-plug / Hot-swap
#define	BOARD_STATUS_POWER_ON_WARNING		0x0100
#define	BOARD_STATUS_POWER_ON				0x0200
#define	BOARD_STATUS_POWER_OFF_WARNING		0x0400
#define	BOARD_STATUS_POWER_OFF				0x0800
#define	BOARD_STATUS_POWER_FAULT			0x1000
#define	BOARD_STATUS_SIMULATED_FAILURE		0x2000
#define	BOARD_STATUS_NORMAL_OPERATION		0x4000
#define BOARD_SRAM_CORRUPTION				0x1111

#define	BOARD_STATUS_INVALID				0x0000	// invalid request/status


// structures used by BOARD_STATUS structure
typedef struct _BOARD_STATUS_IN {
	// Input Fields
	UCHAR	ucBoardNumber;
	BOOLEAN	fConsume;		// notification data to be consumed from system Q or not
	USHORT	usRequest;		// bit-wise ORing of request
							// whichever happens first will be delivered
	LONG	lTimeout;		// timeout in seconds
							// timeout = 0  will return the result upon function's return
							// timeout = -1 will let request pending until it's completed
} BOARD_STATUS_IN, *PBOARD_STATUS_IN;


typedef struct _BOARD_STATUS_OUT {
	// Output Fields
	LARGE_INTEGER	liTime;			// time-stamp when this data was recorded
	USHORT			usResult;		// which request is being satisfied
	ULONG			ulData[64];		// notification data, if needed
									// TODO - mod for var length
} BOARD_STATUS_OUT, *PBOARD_STATUS_OUT;


// Board status request data structure
// Application needs to allocate space for it and must initialize all Input fields
// before calling mntGetBoardStatus() with its pointer.
typedef struct _BOARD_STATUS {
	BOARD_STATUS_IN		In;
	BOARD_STATUS_OUT	Out;
} BOARD_STATUS, *PBOARD_STATUS, *LPBOARD_STATUS;


#ifndef	NO_HOTPLUG_HOTSWAP
// slot event types for performing hot-plug / hot-swap
typedef enum {
	HPHS_UNKNOWN,
	HPHS_NORMAL_OPERATION,
	HPHS_SIMULATED_FAILURE,
	HPHS_POWER_FAULT,
	HPHS_POWER_OFF_WARNING,
	HPHS_POWER_OFF,
	HPHS_POWER_ON_WARNING,
	HPHS_POWER_ON,
	HPHS_RESET_WARNING,
	HPHS_RESET
} HPHS_SLOT_EVENT;
#endif	// NO_HOTPLUG_HOTSWAP


#endif // (!defined(NO_BOARD_STATUS) || !defined(NO_HOTPLUG_HOTSWAP))

#endif // __BRD_STAT_H__
