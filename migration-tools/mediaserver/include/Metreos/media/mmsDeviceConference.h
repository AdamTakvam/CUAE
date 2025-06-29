#ifndef MMS_DEVICECONFERENCE_H
#define MMS_DEVICECONFERENCE_H
#ifdef MMS_WINPLATFORM
#pragma once 
#endif
#include "mmsMediaDevice.h"
#include "mmsConferenceMap.h"
#include <vector>

// Whether or not to use dcb_getcde API for conferee attribute state.
// If not defined we maintain and use local conferee attribute state.
// #define MMS_USE_DCB_GETCDE

// Intel issue# 36098 2/2005 dcb_addtoconf times out on 400 channels.
// Recommendation is to use no more than 254 conferees per conference.
// Note that as of HMP 3.0, 254 conferees max is still the recommendation.
#define HMP_MAX_PARTIES_PER_CONFERENCE 254



class MmsDeviceConference: public MmsMediaDevice
{
  public:
                                            // Establish an HMP conference
  mmsConfereeHandle create(const int publicConfID, MmsMediaDevice* deviceIP, 
     const unsigned int conferenceAttrs = MSCA_ND | MSCA_NN,
     const unsigned int confereeAttrs   = MSPA_NULL);
                                            // Join an HMP conference
  mmsConfereeHandle join(int publicCid, MmsMediaDevice* deviceIP, 
     const unsigned int confereeAttrs = MSPA_NULL);
                                            // Leave an HMP conference
  int leave(int publicCid, MmsMediaDevice* deviceIP);  
                                            
  int leaveHost(int publicCid);             // Last party leave and destroy

  int teardown (int publiCid, int hmpCid);  // Dismantle the HMP conference   
                                            // Open resource
  virtual mmsDeviceHandle open(OPENINFO& openinfo, unsigned short mode);

  virtual void close();                     // Close resource
                                            // Ctor
  MmsDeviceConference(int ordinal, MmsConfig* config);         
  virtual ~MmsDeviceConference();
                                            // Conference resource methods
  int  handle() { return m_handle; }
  int  resourcesRemaining();                 
                                            // Monitor timeslot methods:
  int  addMonitorChannel(int publicCid);    // Add monitor timeslot
  int  removeMonitorChannel(int publicCid); // Disable monitoring
  int  listenOnMonitorChannel  (int publicCid, MmsMediaDevice* deviceIP);
  int  unlistenOnMonitorChannel(int publicCid, MmsMediaDevice* deviceIP);

                                            // Connect device with another
  virtual int busConnect(MmsMediaDevice* deviceIP, int busconnectparams);
                                            // Break bus device connection
  virtual int busDisconnect(MmsMediaDevice* deviceIP, unsigned int index);

  int  connectResource(const int publicCid, const int timeslot, 
       MmsMediaDevice* devIP, MmsMediaDevice* newdev, MmsMediaDevice* olddev);

  MmsHmpConference* getConference(const int publicCid);
                                            // Conferee control:
  int  isConferee(int publicCid, MmsMediaDevice* ip); 
  int  isActiveConferenceID(int publicCid, MmsMediaDevice* ip);     
  int  isReceiveOnly(int publicCid, MmsMediaDevice* ip);   
  int  isReceivingTariffTone(int publicCid, MmsMediaDevice* ip);
  int  setReceiveOnly(int publicCid, MmsMediaDevice* ip, const int onOrOff);
  int  setTariffTone (int publicCid, MmsMediaDevice* ip, const int onOrOff); 
  int  isCoach (int publicCid, MmsMediaDevice* ip);  
  int  isPupil (int publicCid, MmsMediaDevice* ip);  
  int  setCoach(int publicCid, MmsMediaDevice* ip, const int onOrOff);        
  int  setPupil(int publicCid, MmsMediaDevice* ip, const int onOrOff); 
  void logCoachPupil(MmsHmpConference*, MmsMediaDevice*, const unsigned int confereeAttrs);
  void updateConferenceAttrsOnLeave(MmsHmpConference*, const int cid, MmsMediaDevice*);
  MmsMediaDevice* coach(int publicCid);    
  MmsMediaDevice* pupil(int publicCid);  
       
  int  isActiveTalkerMonitoringEnabled();   // Conference control
  int  enableActiveTalkerMonitoring(const int onOrOff=1); 
  int  activeTalkersEnable(const int onOrOff=1);                     
  int  getActiveTalkers(int publicCid, mmsTimeslotHandle* tslotlist, int tslotlistsize); 
  int  isVolumeControlEnabled(); 
  int  enableVolumeControl(const int onOrOff, int up=0, int reset=0, int down=0);

  int  MmsDeviceConference::getConfereeAttributes(const int externalID, MmsMediaDevice* deviceIP);
  int  getConfereeAttributes(int hmpCid, MmsMediaDevice* deviceIP, int alert); 

  protected:    
  MmsHmpConferenceMap* conferences;

  struct BUSCONNECTPARAMS
  { int  hmpCid;            // hmp conference id
    int  confereeIndex; 
    unsigned int conferenceAttrs;
    unsigned int confereeAttrs;
    unsigned int flags;            
    MmsHmpConference* conference; 
    enum bitflags{HALFDUPLEX = 1, LISTENONLY = 2, NEEDLOCK = 4};
    BUSCONNECTPARAMS(int cid, int cix, unsigned int ca1, unsigned int ca2, MmsHmpConference* conf) 
    { memset(this, 0, sizeof(BUSCONNECTPARAMS));
      hmpCid=cid; confereeIndex=cix; conferenceAttrs=ca1; confereeAttrs=ca2; 
      conference = conf;
    }
  };

  int  init();

  int  getConfereeAttributes(int hmpCid, MMS_CDT* cdtentry, int alert); 
  int  setConfereeAttributes(int publicCid, MmsMediaDevice* deviceIP, int attrs); 
  int  setHmpConfereeAttributes(int hmpCid, MMS_CDT* cdtentry, int attrs);
  int  getLocalConfereeAttributes(MmsHmpConference*, MmsMediaDevice*);
  int  setLocalConfereeAttributes(MmsHmpConference*, MmsMediaDevice*, const unsigned int);
  int  raiseConferenceResourcesExhaustedAlarm();
  int  raiseConferenceSlotsExhaustedAlarm(); 
  int  raiseConferencesExhaustedAlarm();   

  void logConfereeTimeslots(MmsMediaDevice* deviceIP, MMS_CDT* cdtEntry); 
  void logConferenceAttributes(const BUSCONNECTPARAMS* params, 
          const int isNew, const int isSkipConferee=0);   
  void MmsDeviceConference::logConferenceAttributes
      (const int confxa, const int confereea, const int isNew, const int isSkipConferee=0); 
  void handleConferenceAttributes(BUSCONNECTPARAMS*, MMS_CDT*, const int isNew);                     
};



#endif