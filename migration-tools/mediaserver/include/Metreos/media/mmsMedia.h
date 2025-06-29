#ifndef MMS_MEDIA_H
#define MMS_MEDIA_H
#ifdef MMS_WINPLATFORM
#pragma once
#endif
#include <iostream>
#include "srllib.h"
#include "gclib.h"
#include "ipmlib.h"
#include "dtilib.h"
#include "dxxxlib.h"
#include <msilib.h>
#include <dcblib.h>
#include <errno.h>
#include "mms.h"


#define HMP_OK        0           
#define HMP_TIMEOUT   2
#define HMP_ERROR   (-1)

#define DEVICEIDSIZE 16                     // Masks for HMP device open calls             
#define CARDIDMASK "%sB%dC%d"
#define HMPCARDID(name,prefix,board,card) char name[DEVICEIDSIZE]; \
   sprintf(name,CARDIDMASK,prefix,board,card);
#define BOARDIDMASK "%sB%d"
#define HMPBOARDID(name,prefix,board) char name[DEVICEIDSIZE]; \
   sprintf(name,BOARDIDMASK,prefix,board);
#define DEVICEIDMASK "%sB%dD%d"
#define HMPDEVICEID(name,prefix,board,device) char name[DEVICEIDSIZE]; \
   sprintf(name,DEVICEIDMASK,prefix,board,device)
#define TIMESLOTIDMASK "%sB%dT%d"
#define HMPTIMESLOTID(name,prefix,board,device) char name[DEVICEIDSIZE]; \
   sprintf(name,TIMESLOTIDMASK,prefix,board,device)

#define HMP_KEEP_EVENT     1
#define HMP_RELEASE_EVENT  0

#define IS_VOICE_EVENT(n) (n > 0x80 && n <= 0xff)
#define IS_IP_EVENT(n)    (n > IPMEV_MASK)

#define MMS_MAX_INITIAL_IOTTS     8  

typedef long (*HMPEVENTHANDLER)(unsigned long);

typedef int  mmsDeviceHandle;
typedef int  mmsConfereeHandle;             // Conferee timeslot
typedef long mmsTimeslotHandle;
                                            // Arbitrary value for validation
#define MAXDEVICEHANDLE ((mmsDeviceHandle)(1023))                                                
                                            // HMP handle validation macros
#define isBadDeviceHandle(n)     (((n) <= 0) || ((n) >  MAXDEVICEHANDLE))
#define isValidDeviceHandle(n)   (((n) >  0) && ((n) <= MAXDEVICEHANDLE))
#define isValidConfereeHandle(n)  ((n) >= 0)
#define isBadConfereeHandle(n)    ((n) <  0)


class Hmp
{
  protected:
  static Hmp* m_instance;                   // Singleton 
  Hmp();
  Hmp& operator=(const Hmp&) { };
  static int isInitialized;

  public:
  static Hmp* instance()
  {
    if   (!m_instance) m_instance = new Hmp;  
    return m_instance;
  }

  virtual ~Hmp();

  int  init();
  void shutdown();

  static long defaultHmpEventHandler(long);
  int  registerDefaultEventHandler(bool reg=true); 
  int  raiseEvent(int handle, int eventType, int dataLen, char* eventData);
};


 
#endif