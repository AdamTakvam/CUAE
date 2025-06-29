//
// Copyright (C) 1998  Dialogic Corporation.  All Rights Reserved.
//
// This is the header for Dialogic D42 boards.


#ifndef D42_DEVSPEC_INCLUDED
#define D42_DEVSPEC_INCLUDED	1

#include "dlgspec.h"

#define D42TSP_EXTENSIONID0 0xBF819AC0
#define D42TSP_EXTENSIONID1 0x1021F498
#define D42TSP_EXTENSIONID2 0xC00056A4
#define D42TSP_EXTENSIONID3 0xE6D81D4F

#define D42_DATATYPE_GENERIC	0x0	
#define D42_DATATYPE_DISPLAY	0x1
#define D42_DATATYPE_LED		0x2
#define D42_DATATYPE_PROTIMS	0x4
#define D42_DATATYPE_DIASTRING	0x8	

typedef struct _d42pbxdatatag 
{
	DWORD	dwType;
	DWORD	dwLength;
	BYTE	pData[160];
}D42PBXDATA, *PD42PBXDATA;

//
// Device specific command codes
//
#define DEVSPEC_D42					0x00040000
#define SEND_PBXMSG					(DEVSPEC_D42 + 1)		// Send PBX message (AA-INFO)
#define SPD_KEY_PROG				(DEVSPEC_D42 + 2)		// Program Speed key
#define REC_BEEP_DUR				(DEVSPEC_D42 + 3)		// set record beep duration
#define REC_BEEP_REP				(DEVSPEC_D42 + 4)		// set record beep interval
#define REC_BEEP_TIMETOFIRSTBEEP	(DEVSPEC_D42 + 5)		// set first beep time delay
#define REC_BEEP_ALL				(DEVSPEC_D42 + 6)		// set all three parameters

typedef struct _d42recbeep
{
	int nRecBeepDur;
	int nRecBeepRep;
	int nTimeToFirstBeep;
} D42RECBEEP, *PD42RECBEEP;

#endif