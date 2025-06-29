//
// mmsMedia.cpp
//
#include "StdAfx.h"
#include "mms.h"
#include "mmsMedia.h"
#include "mmsLogger.h"

#ifdef MMS_ENABLE_MEMORY_LEAK_DETECTION
#define new MMS_MEMLEAK_NEW
#endif // MMS_ENABLE_MEMORY_LEAK_DETECTION

Hmp* Hmp::m_instance    = 0;
int  Hmp::isInitialized = 0;



Hmp::Hmp()                                  // Private ctor
{ 
} 



Hmp::~Hmp() 
{ 
  // Singleton - cleanup must be explicit, not implicit
}



void Hmp::shutdown()
{ 
  if  (m_instance) delete m_instance;
  m_instance = NULL; 
}



int Hmp::registerDefaultEventHandler(bool RegisterOrUnregister)        
{ 
  return RegisterOrUnregister?                         
         sr_enbhdlr(EV_ANYDEV, EV_ANYEVT, (HMPEVENTHANDLER)defaultHmpEventHandler):     
         sr_dishdlr(EV_ANYDEV, EV_ANYEVT, (HMPEVENTHANDLER)defaultHmpEventHandler);  
}



int Hmp::raiseEvent(int deviceHandle, int eventType, int dataLen, char* eventData)
{
  int result = sr_putevt((long)deviceHandle,
        (unsigned long)eventType, (long)dataLen, (void*)eventData, 0);
  return result;
}



int Hmp::init()
{
  if  (isInitialized) return -1;
  //   int logflags = disableLogTimestamps(); 
  //   MMSLOG((LM_INFO,"HMPX startup\n"));  // This semi-hack needed in HMP 1.0                                          
  if  (-1 == registerDefaultEventHandler()) // in which sr_waitevt requires a 
  {    MMSLOG((LM_ERROR,"HMPX could not initialize\n"));
       return -1;                           // handler to be enabled once ...
  }                                         // which we do here ...   
  registerDefaultEventHandler(false);       // and then immediately disable
  isInitialized = 1;
  return 0; 
}


  
long Hmp::defaultHmpEventHandler(long)    
{ 
  // This event handler is not used. We use it to register, and immediately
  // unregister, an event handler, to work around a sr_waitevt glitch.
  return 0;                            
}
