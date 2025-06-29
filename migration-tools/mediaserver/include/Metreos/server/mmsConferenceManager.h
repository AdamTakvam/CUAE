//
// mmsConferenceManager.h  
//
#ifndef MMS_CONFERENCEMGR_H
#define MMS_CONFERENCEMGR_H
#ifdef MMS_WINPLATFORM
#pragma once
#endif
#pragma warning(disable:4786)
#include "mmsSession.h"
#include "mmsDeviceConference.h"
#include <list> 
#include <map>

#define CLIENT_PROMOTE_OPT_NOT_SPECIFIED  0
#define CLIENT_PROMOTE_OPT_PROMOTE        1
#define CLIENT_PROMOTE_OPT_PROMOTE_DEMOTE 2
#define CLIENT_PROMOTE_OPT_DO_NOT_PROMOTE 4
	


class MmsConference
{
  // Represents a conference from a MMS perspective, in which a conferee is
  // a session. Tracks parties by session, including both full participants 
  // and monitor listeners. For informational and conferee status conferencing
  // functionality, we don't provide wrappers here, sessions will go direct
  // thru the HMP device MmsConferenceManager::device(session) for those.
  // HPIN: note we need to track down all such informational and status
  // calls, and intercept them if the conference is hairpinned
  public:
  int join(MmsSession::Op*, unsigned int attributes);

  int hairpin(MmsSession*, unsigned int attributes);

  int hairpin(const int getlock=1);

  int unhairpin(const int getlock=1);

  int leave(MmsSession*);

  int monitor(MmsSession::Op*);

  int unmonitor(MmsSession*);

  int promote();

  int promote(MmsSession*, char* flatmap=0);    

  int demote();

  int mutehairpinned(MmsSession*, const int isMuting=1);
 
  int size()      { return conferees.size(); }
  int nmonitors() { return monitors.size(); }
                                             
  int id()        { return conferenceID; }
  int teardown(int forceDevice=0, int release=0);
  int teardownHairpin();
  MmsSession* first();

  int isConferee (MmsSession*);       
  int isMonitor  (MmsSession*); 
  int isPromoting(MmsSession*, char* flatmap=0); 
  int isDemoting (MmsSession*, char* flatmap=0);
  int confereeAttributes(MmsSession*);
  int isMonitored() { return this->ismonitored; } 
  int isHairpinned(){ return this->ishairpin;   }
  int isactive;
  int internalConferenceID();

  MmsMediaDevice* listenDevice(MmsSession::Op*, const int acquire = TRUE);
  static unsigned int normalizeConfereeAttributes(const unsigned int);
  int isOnlyDroneSessions(MmsSession* thisSession, MmsSession** droneSessions);

  int isNonHairpinnableParty(const unsigned attrs)
  {
      return (attrs & (MSPA_COACH | MSPA_PUPIL)) != 0;
  } 
                                                     
  MmsConference(MmsDeviceConference*, const int confID, const int attrs, const int ishairpin); 
  ACE_Thread_Mutex dlock;                   // Destruct mutex  

  friend class MmsConferenceManager;

  struct HairpinInfo
  {
    MmsDeviceIP* ipA;
    MmsDeviceIP* ipB;
    MmsSession* sessionA;
    MmsSession* sessionB;
    HairpinInfo() { reset(); }
    void reset()  { ipA = ipB = NULL; sessionA = sessionB = NULL; }
  };

  protected:
  int conferenceID, ismonitored, ishairpin;
  unsigned int conferenceAttributes;
  MmsDeviceConference* deviceConference;
  std::list<MmsSession*> conferees;         // Full conferees by session 
  std::list<MmsSession*> monitors;          // Monitors by session 
  ACE_Thread_Mutex xlock;                   // Access mutex

  int  getHairpinInfo(HairpinInfo& info);
  void muteIfMuted();
  void muteIfMuted(MmsConference::HairpinInfo&);

  MmsConference() {} 
}; 



class MmsConferenceManager
{
  // Manages conferences from an MMS session perspective.
  public:

  int joinConference(MmsSession::Op*, const int conferenceID, const int flags=0,
      const unsigned int confereeAttrs=0, const unsigned int conferenceAttrs=0);

  int leaveConference(MmsSession*, const int confID);

  int teardownConference(MmsSession*, const int conferenceID);

  int joinUtilitySession(MmsSession*);

  int findByConferenceID(const int conferenceID, MmsConference** outconf, const int isLog=0);

  int findBySession(MmsSession* session, MmsConference** outconf);

  int removeConferenceByID(const int conferenceID);

  int isActiveConference(const int conferenceID);

  int size() { return conferences.size(); }

  int closeAll();

  static int conferenceDevice
    (mmsDeviceHandle*, MmsDeviceConference**, const int sid=0, const int isLog=0);

  MmsDeviceConference* device(MmsSession* session);

  MmsDeviceConference* device();

  static int isConfereeOptionsOkToHairpin(const unsigned int attrs);

  MmsConferenceManager
    (MmsConfig* config, HmpResourceManager* resourceMgr, MmsTask* serverMgr);
  virtual ~MmsConferenceManager();

  enum bitflags{MONITOR=1, UTILITY_SESSION=2};

  friend class MmsSessionManager;

  protected:

  int createConference(MmsSession::Op*, 
      unsigned int confereeAttrs,  const unsigned int conferenceAttrs);

  int validateConferenceParameters(MmsSession*, const int conferenceID, const int flags, 
      const unsigned int confereeAttrs, const unsigned int conferenceAttrs);

  int isHairpinning(MmsSession::Op*, 
      const unsigned int confereeAttrs, const unsigned int conferenceAttrs);

  int ensureHmpConference(const int conferenceID);

  MmsConferenceManager() { }
  MmsConfig* config;
  MmsTask*   serverMgr;
  static HmpResourceManager* resourceMgr;
  char objname[5];
                                             
  std::map<int, MmsConference*> conferences;// ConferenceID to MmsConference*
  ACE_Thread_Mutex slock;
};



#endif

