#ifndef MMS_MEDIADEVICE_H
#define MMS_MEDIADEVICE_H
#ifdef MMS_WINPLATFORM
#pragma once
#endif
#include <time.h>
#include "mmsMedia.h"
#include "mmsConfig.h"

#define DEVICEFLAGS_OWNED             1
#define DEVICEFLAGS_IS_SELF_LISTENING 2
#define DEVICEFLAGS_IS_LOWBITRATE     4
#define DEVICEFLAGS_IS_CSP            8
#define DEVICEFLAGS_IS_STREAMING      16


struct SC_TSINFOEX: public SC_TSINFO {SC_TSINFOEX() {sc_numts=1; sc_tsarrayp=0;}};

 

class MmsMediaDevice
{
  public:
  struct deviceKey 
  { int board; 
    int card;
    void set(int b, int c) { board = b; card = c; }
    void set(const deviceKey& that) { board = that.board; card = that.card; }
  };

  struct OPENINFO                           // Media device open parameters
  { int       length;                        
    deviceKey key;                           
    void*     info;
    OPENINFO(int b, int c) { init(); key.set(b,c); }
    OPENINFO() { init(); }
    void init(){ memset(this,0,sizeof(OPENINFO)); length = sizeof(OPENINFO); }
    void set(int b, int c) { key.set(b,c); }
  }; 
                                            // Redefine HMP constants
  enum{SYNC = EV_SYNC, ASYNC = EV_ASYNC, PLAYTONE = RM_TONE};   
  enum{FULLDUPLEX, HALFDUPLEX};
                                            // Open media device
  virtual mmsDeviceHandle open(OPENINFO& openinfo, unsigned short mode=SYNC)=0;

  virtual void close()=0;                   // Close media device
  
  virtual int  listen(SC_TSINFO* slotinfo); // Start listening on timeslot
 
  virtual int  unlisten();                  // Stop listening on timeslot
                                            // Get transmit timeslot info
  virtual int  timeslot(SC_TSINFO* slotinfo);  

  long         timeslotNumber();            // Get transmit timeslot ID
                                            // Connect device with another
  virtual int  busConnect(MmsMediaDevice* device2, unsigned int mode = FULLDUPLEX);
                                            // Break bus device connection
  virtual int  busDisconnect(MmsMediaDevice* device2, unsigned int mode);

  virtual int  isListening();
                                            // Return (HMP) device handle
  mmsDeviceHandle handle() { return m_handle; }
                                            // Return IP, VOICE, CONF, etc
  int   type()             { return m_deviceType; }
  char* name()             { return devname; }
  int   ordinal()          { return m_ordinal; }

  enum DEVICETYPE{IP=1, VOICE, CONF, FAX, LSI, DTI, MSI};

  enum DEVICESTATE{CLOSED, AVAILABLE, IDLE, INUSE, TRANSITORY};
  enum MEDIASTATE {MEDIAIDLE, MEDIAWAITING, MEDIAACTIVE};

  enum{ATTRIBUTE_ERROR = 0x01000000};       // A sufficiently high bit 
  enum{NOAQUIRE_RESOURCE = 0x01000000};     // Ditto 
  enum{NOCLEAR = 0x02000000};
                                            // Ctor
  MmsMediaDevice(int deviceType, int ordinal, MmsConfig* config);           

  virtual ~MmsMediaDevice() { }
                                            // Device type convenience methods
  int  isIpDevice()         { return m_deviceType == IP; }
  int  isVoiceDevice()      { return m_deviceType == VOICE; }
  int  isConferenceDevice() { return m_deviceType == CONF; }
  int  isFaxDevice()        { return m_deviceType == FAX; }
  char* devTypeC();
                                            // Device state convenience methods
  int  isAvailable()    { return m_deviceState == AVAILABLE; }
  int  isIdle()         { return m_deviceState == IDLE; }
  int  isBusy()         { return m_deviceState == INUSE; }
  int  isClosed()       { return m_deviceState == CLOSED; }
  int  isLimbo()        { return m_deviceState == TRANSITORY; }
  int  isMediaIdle()    { return m_mediaState  == MEDIAIDLE; }
  int  isMediaWaiting() { return m_mediaState  == MEDIAWAITING; }
  int  isMediaActive()  { return m_mediaState  >= MEDIAWAITING; }
  time_t timeIdled()    { return m_timeIdled;} // Return time resource went idle
                                            
  void setAvailable();                      // Device state setters
  void setIdle();    
  void setBusy() { m_deviceState = INUSE; m_timeIdled = 0; }
  void setBusy(unsigned long n)  { m_owner = n; setBusy(); }  
  void setBusy(unsigned long n, unsigned long m) { this->owner(n,m); setBusy(); }  
  void setOwned(const int isOwned); 
  int  isOwned();    
  void setTransitory();   
  void timeIdled(int n)   { m_timeIdled = n; }
  unsigned long owner()   { return m_owner;  }
  unsigned long subowner(){ return m_subowner; }
  void owner(unsigned long n)   { m_owner = n; }
  void owner(unsigned long n, unsigned long m) { m_owner = n; m_subowner = m; }
  void logTimeslots(MmsMediaDevice* dev2);
  void logListening(SC_TSINFO*);
  void logUnlistening();

  unsigned int flags;

  // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
  protected: 
  // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - 
  mmsDeviceHandle m_handle;                 // HMP device handle in this case
  int             m_ordinal;                // Sequential ID
  int             m_deviceType;             // What type of device are we
  int             m_deviceState;            // Are we in use or what
  int             m_mediaState;             // Is async activity in progress
  time_t          m_timeIdled;              // Time device went idle (map key)
  SC_TSINFO       m_timeslotInfo;           // {int sc_numts, long* sc_tsarrayp}
  long            m_timeslotNumber;         // ID returned by get xmit slot
  unsigned long   m_owner;                  // Object currently hosting resource
  unsigned long   m_subowner;               // Owner subkey
  char            devname[16];              // Device name for logging purposes
  char            em[16];                   // Error message prefix
  deviceKey       m_key;
  MmsConfig*      m_config;

  MmsMediaDevice() { }
};


#endif // MMS_MEDIADEVICE_H