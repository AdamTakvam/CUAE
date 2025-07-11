#ifndef MMS_HMPCONTROL_H
#define MMS_HMPCONTROL_H
#ifdef MMS_WINPLATFORM
#pragma once
#endif
#include "mms.h"
#include "mmsConfig.h"
#include "NCMApi.h"
#include "ace/OS.h"   
#include "ace/ACE.h"

#define MMS_HMP_ERROR_STARTSERVICE (-1)
#define MMS_HMP_ERROR_BOARDDETECT  (-2)
#define MMS_HMP_ERROR_STOPSERVICE  (-3)
#define MMS_HMP_ERROR_SERVICESTATE (-4)

#define MMS_HMP_SERVICESTOP  0
#define MMS_HMP_SERVICERUN   1
#define MMS_HMP_SERVICEOTHER 2

#define DEVICE_FAMILY	"DM3"									
#define DEVICE_NAME		"HMP_Software #0 in slot 0/65535"	// default device name if there is only one
#define IGNORE_PCD		"240r240v200e240c240s240i_pur.pcd"				// the pcd file generated by default 1 port license

int hmpStart(); 
int hmpStop();
int hmpStartService();
int hmpStopService ();
int hmpDetectBoards();
int hmpGetServiceState();
int hmpServiceStartPctCompleteCallback(unsigned int pct, const char* msg);
int hmpGetPcdFile(NCMFileInfo* flist, int count, NCMDevInfo info, int* ndx);
void RestoreDefaults();
void registerDLL();

#endif
