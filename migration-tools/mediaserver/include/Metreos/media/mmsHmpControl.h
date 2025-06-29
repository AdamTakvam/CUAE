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


int hmpStart(MmsConfig* config, int verify=0, int detect=1, int verbose=1); 

int hmpStop(int waitSecs, int verbose=1);

int hmpStartService(int verbose=1);

int hmpStopService (int verbose=1);

int hmpDetectBoards();

int hmpGetServiceState();

int hmpServiceStartPctCompleteCallback(unsigned int pct, const char* msg);

int hmpGetPcdFile(NCMFileInfo* flist, int count, NCMDevInfo info, int* ndx);


#endif
