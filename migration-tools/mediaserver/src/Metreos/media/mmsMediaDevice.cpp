//
// mmsMediaDevice.cpp
//
#include "StdAfx.h"
#include "mmsMediaDevice.h"
static char* dt[] ={"??","IP","VX","CF"};

#ifdef MMS_ENABLE_MEMORY_LEAK_DETECTION
#define new MMS_MEMLEAK_NEW
#endif // MMS_ENABLE_MEMORY_LEAK_DETECTION


                                                                                   
int MmsMediaDevice::busConnect(MmsMediaDevice* device2, unsigned int mode)
{   
  // Connect two devices on the SC bus.
  // For example: voiceResource->busConnect(ipResource, FULLDUPLEX);
  // 1. Make IP resource listen to timeslot that voice resource is xmitting on
  // 2. Make voice resource listen to timeslot that ip resource is xmitting on 
  SC_TSINFO tsinfo;                         
                                            
  if  (this->timeslot (&tsinfo) == -1)      // Get bus timeslot connected to
       return -1;                           // the xmit of this device
                                            // Make other device listen to slot
  if  (device2->listen(&tsinfo) == -1)      // that this device is xmitting on
       return -1;               

  if  (mode == FULLDUPLEX)                  // Get bus timeslot connected to
  {    int result = 0;                      // the xmit of the other device

       if  (device2->timeslot(&tsinfo) == -1) 
            result = -1;                                                                   
       else                                     
       if  (this->listen(&tsinfo) == -1)    // Make this device listen to slot
            result = -1;                    // the other device is xmitting on             
  }

  if  (m_config->diagnostics.flags & MMS_DIAG_LOG_TIMESLOTS)
       this->logTimeslots(device2); 
    
  return 0;
}



int MmsMediaDevice::busDisconnect(MmsMediaDevice* device2, unsigned int mode)
{                                           // Disconnect rcv of device2 
  int  result = device2->unlisten();        // from the bus timeslot
                                    
  if  (mode == HALFDUPLEX) return result;  
                                            // Disconnect rcv of this device 
  if  (-1 == this->unlisten()) result = -1; // from the bus timeslot

  return result;
}



int MmsMediaDevice::listen(SC_TSINFO*)   
{  
  return -1;
}



int MmsMediaDevice::unlisten()   
{  
  return -1;
}



int MmsMediaDevice::timeslot(SC_TSINFO*) 
{
  return -1;
}



int MmsMediaDevice::isListening() 
{
  return 0;
}



long MmsMediaDevice::timeslotNumber()
{
  SC_TSINFO tsinfo;
  return this->timeslot(&tsinfo) == -1? -1: *(tsinfo.sc_tsarrayp);
} 



void MmsMediaDevice::setAvailable() 
{
  m_deviceState = AVAILABLE; 
  m_mediaState  = MEDIAIDLE; 
  m_timeIdled = 0; 
  m_owner = 0;  
  this->flags &= ~(DEVICEFLAGS_OWNED);
}



void MmsMediaDevice::setIdle() 
{
  m_deviceState = IDLE;
  m_mediaState  = MEDIAIDLE;  
  m_timeIdled   = time(0); 
}



void MmsMediaDevice::setTransitory() 
{ 
  m_deviceState = TRANSITORY; 
  m_timeIdled = 0;
}



void MmsMediaDevice::logTimeslots(MmsMediaDevice* dev2)
{
  const  int   nt1  = this->type() < 4? this->type(): 0;
  const  int   nt2  = dev2->type() < 4? dev2->type(): 0;
  const  char* dt1  = dt[nt1], *dt2 = dt[nt2];
  const  int   hnd1 = this->handle(), hnd2 = dev2->handle();
  const  int   tsn1 = this->timeslotNumber(), tsn2 = dev2->timeslotNumber();
  
  MMSLOG((LM_DEBUG,"%s timeslots %s%d/%s%d %d/%d\n",
          devname,dt1,hnd1,dt2,hnd2,tsn1,tsn2));
}



void MmsMediaDevice::logListening(SC_TSINFO* slotinfo)
{
  const  int   nt1  = this->type() < 4? this->type(): 0;
  const  char* dt1  = dt[nt1];
  const  int   hnd1 = this->handle();
  
  MMSLOG((LM_DEBUG,"%s %s%d listening to slot %d\n",
          devname,dt1,hnd1,*(slotinfo->sc_tsarrayp)));
}



void MmsMediaDevice::logUnlistening()
{
  const  int   nt1  = this->type() < 4? this->type(): 0;
  const  char* dt1  = dt[nt1];
  const  int   hnd1 = this->handle();
  
  MMSLOG((LM_DEBUG,"%s %s%d unlistens\n", devname, dt1, hnd1));
}



void MmsMediaDevice::setOwned(const int isOwned)
{
  if  (isOwned) // Indicate that device must remain with session
       this->flags |=   DEVICEFLAGS_OWNED;
  else this->flags &= ~(DEVICEFLAGS_OWNED);
}



int MmsMediaDevice::isOwned()
{
  return (this->flags & DEVICEFLAGS_OWNED) != 0;
}



char* MmsMediaDevice::devTypeC()
{
  static char *ip = "ip", *vox = "vox", *cfx = "cfx", *oth = "oth";

  switch(m_deviceType)
  { case IP:    return ip;
    case VOICE: return vox;
    case CONF:  return cfx;
  }
  return oth;
}


                                            
MmsMediaDevice::MmsMediaDevice(int deviceType, int ordinal, MmsConfig* config) 
{ 
  m_deviceType  = deviceType;               // Ctor
  m_deviceState = CLOSED;
  m_mediaState  = MEDIAIDLE;
  m_ordinal     = ordinal;
  m_timeIdled   = 0;
  m_owner       = 0;
  m_config      = config;
  this->flags   = 0;
  *devname = '\0';
  ACE_OS::strcpy(this->em,"error during"); 
} 
   