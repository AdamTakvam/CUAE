#ifndef MMS_DEVICEIP_H
#define MMS_DEVICEIP_H
#ifdef MMS_WINPLATFORM
#pragma once
#endif

#include "mmsMediaDevice.h"
#define DSCP_EXPEDITE_FORWARD_VALUE 0xb8  // Value of TCP DSCP EF byte 

// #define MMS_HMP_1X_IPM_HEADERS         // Remove once we no longer need headers prior 2.0
  


class MmsDeviceIP: public MmsMediaDevice
{
  public:
                                            // Open IP resource 
  virtual mmsDeviceHandle open(OPENINFO& openinfo, unsigned short mode);

  virtual void close();                     // Close IP resource

  MmsDeviceIP(int ordinal, MmsConfig*);     // Ctor
  virtual ~MmsDeviceIP();
                                               
  virtual int listen(SC_TSINFO* slotinfo);  // Connect to specified timeslot
 
  virtual int unlisten();                   // Disconnect from timeslot

  virtual int timeslot(SC_TSINFO* slotinfo);// Get transmit timeslot 

  int   selflisten();                       // Listen to our own xmit timeslot
                                            // Start RTP session
  int   start(const char* ip, unsigned int port, const unsigned int remoteattrs=0, 
              const unsigned int localattrs=0, const int mode=SYNC);                     
                                            // Disconnect from timeslot & more
  int   stop(eIPM_STOP_OPERATION stopwhat = STOP_ALL);
                                            // Return SCBus timeslot info
  int   startReceiveDigits();               // Prepare to receive DTMF 

  int   startReceiveDigits(IPM_DIGIT_INFO* info);

  int   stopReceiveDigits();                // Terminate DTMF reception

  int   sendDigits(const char* digitlist, const int numdigits, const int mode=SYNC);

  int   toggleRfc2833Events(const int enabling);
                                            // Return prior session's stats
  int   getSessionStats(IPM_SESSION_INFO* info); 

  int   resourcesRemaining();               // Return resource count

  int   isStopped()  { return m_mediaState == IPMEDIASTOPPED; }
  int   isStarted()  { return m_mediaState >= IPMEDIASTARTED; }
  int   isListening(){ return m_mediaState == IPMEDIALISTENING; }
  int   isSelfListening() { return (flags & DEVICEFLAGS_IS_SELF_LISTENING) != 0; }
  int   isLowBitrateConnection() { return (flags & DEVICEFLAGS_IS_LOWBITRATE) != 0; }
  int   isLowBitrateCoder(IPM_CODER_INFO& coderInfo);

  int   initRemoteMediaInfo(const char* ip, unsigned int port, 
           const unsigned int remoteattrs, const unsigned int localattrs, int m);
  int   verifyCoderAvailabilityEx(const int mode, const int isLog=0);
  int   raiseLowBitrateExhaustedAlarm();
  static int  getDigitFromRfc2833EventData(void* eventdata);
  static long getRfc2833EventType();

  char* localIP();                          // Convenience accessors
  char* remoteIP();
  int   localPort();
  int   remotePort();
  int   localCoder();
  int   remoteCoder();
  int   localFrsize();
  int   remoteFrsize();
  int   localFPP();
  int   remoteFPP();
  int   localVad();
  int   remoteVad();
  int   dataflowDirection();

  protected:
  IPM_MEDIA_INFO m_mediaInfoLocal;
  IPM_MEDIA_INFO m_mediaInfoRemote;
 
  eIPM_DATA_DIRECTION m_dataflowDirection;

  enum ipMediaStates                        // MmsMediaDevice::mediaState
  { IPMEDIASTOPPED   = MEDIAIDLE,
    IPMEDIASTARTED   = MEDIAWAITING,
    IPMEDIALISTENING = MEDIAACTIVE
  };

  int  getLocalMediaInfo();
  int  initLocalMediaInfo();
  int  startMedia(const int mode=SYNC);

  int  init();
  void setCoderInfo(IPM_CODER_INFO&, const unsigned int attrs, const int isLocal=0);
  int  setDeviceParam(IPM_PARM_INFO&);
  void logMediaInfo(IPM_MEDIA*, IPM_MEDIA*, IPM_MEDIA*, IPM_MEDIA*);
  void logCoderInfo(IPM_MEDIA*, const int isLocal=0); 
  void trackLowBitrateUsage(const int minus=0, const int mode=0);
  MmsConfig* config;
};


#define rtpInfoLocal   m_mediaInfoLocal.MediaData[0]
#define rtcpInfoLocal  m_mediaInfoLocal.MediaData[1]

#define rtpInfoRemote  m_mediaInfoRemote.MediaData[0]
#define rtcpInfoRemote m_mediaInfoRemote.MediaData[1]
#define codecRemote    m_mediaInfoRemote.MediaData[2]
#define codecLocal     m_mediaInfoRemote.MediaData[3]


#endif