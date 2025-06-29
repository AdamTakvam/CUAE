// 
// MmsMq.h 
//
// MMS MSMQ message queue object
// 
#ifndef MMS_MQ_H
#define MMS_MQ_H
#ifdef MMS_WINPLATFORM
#pragma once
#endif
#include "mms.h"
#include <wtypes.h>
#include <objidl.h>
#include <mq.h>
#include <time.h>




class MmsMq
{ 
  public:
  struct  MmsMqFormatName
  { char  szFormatName [MAX_PATH];
    WCHAR wszFormatName[MAX_PATH];
  };
                                            // Construct MSMQ format name
  static LPWSTR mqFormatName
  (const char* machinename, const char* queuename, struct MmsMqFormatName* p)
  {
    wsprintf(p->szFormatName,"DIRECT=OS:%s\\Private$\\%s",machinename,queuename);

    MultiByteToWideChar(CP_ACP, 0, p->szFormatName, strlen(p->szFormatName)+1,
         p->wszFormatName, sizeof(p->wszFormatName) / sizeof(p->wszFormatName[0]));

    return p->wszFormatName;   
  }

  protected:
  int  openQueue(const char* mn, const char* qn, const DWORD mode);

  static QUEUEHANDLE openq(const char* mn, const char* qn, DWORD mode, MmsMqFormatName*);
 
  static int createq(const char* mn,  const char* qn, PSECURITY_DESCRIPTOR, 
                     const char* lab, const int isIgnoreError=0);  

  static int deleteq(const char* mn, const char* qn);

  QUEUEHANDLE hq;
  int  queuetype;
  int  disallowCreate;
  MmsMqFormatName formatname;

                                            // Construct MSMQ path name
  static LPWSTR mqPathName
  (const char* machinename, const char* queuename, MmsMqFormatName* p)
  {                                          
    // If local, OK to specify machinename as "."
    wsprintf(p->szFormatName,"%s\\Private$\\%s",machinename,queuename);

    MultiByteToWideChar(CP_ACP, 0, p->szFormatName, strlen(p->szFormatName)+1,
         p->wszFormatName, sizeof(p->wszFormatName) / sizeof(p->wszFormatName[0])); 

    return p->wszFormatName; 
  }

  char* cachedMachineName;
  char* cachedQueueName;

  public:
  MmsMq(const int flags=0);
  virtual ~MmsMq(); 
  virtual  char* label(const DWORD mode);
  enum modebits{RECEIVEACCESS = 1, SENDACCESS = 2, CREATEIF = 16};
  enum ctorbits{READER = 1, WRITER = 2, NOCREATE = 4};
  QUEUEHANDLE handle() { return this->hq; }

  int  openQueue(const char* mn, const char* qn);
  int  closeQueue();
  int  deleteQueue();

  int isMatchQueueName(const char* otherQueueName)
  {                                         // 626
    return cachedQueueName && stricmp(cachedQueueName, otherQueueName) == 0;
  }

  static void showresult(HRESULT hr);
  char* machineName() { return cachedMachineName; }
  char* queueName()   { return cachedQueueName;   }
};




#endif
