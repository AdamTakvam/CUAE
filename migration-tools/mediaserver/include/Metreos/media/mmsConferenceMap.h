//
// mmsConferenceMap.h 
//
// Thin wrapper for HMP conference and associated CDT, and conference collection 
//
#ifndef MMS_CONFERENCEMAP_H
#define MMS_CONFERENCEMAP_H
#ifdef MMS_WINPLATFORM
#pragma once
#endif
#pragma warning(disable:4786)
#include <map> 
#include <vector>
#include "mmsMediaDevice.h"
#define MMS_MAX_INITIAL_CONFEREES 16        // Delta for CDT re/allocation
struct MMS_CDT;


struct MmsHmpConference
{  
  // Represents an HMP conference identified by HMP conference ID.
  // Each object hosts a CDT conference descriptor table
    
  int  size()    { return m_conferees; } 
  int  publicCid()      { return m_publicConferenceID; }  // public conference id
  void publicCid(int n) { m_publicConferenceID = n; }     
  int  hmpCid()      { return m_hmpConferenceID; }        // hmp conference id
  void hmpCid(int n) { m_hmpConferenceID = n; }
  void teardown();
                                            // CDT activity
  MMS_CDT* setCdt(const int i, const int chanNum, const int chanAttr, const int mmsAttrs);
  MMS_CDT* getCdt(const mmsTimeslotHandle tsn);
  MMS_CDT* getCdt(const int confereeIndex);
  int  cdtAdd()  { return ++m_conferees;}
  int  cdtRemove(const int i);
  int  cdtRemove(MMS_CDT*);
  int  isConferee   (MmsMediaDevice* deviceIP);
  int  confereeIndex(MmsMediaDevice* deviceIP);
                                            // Monitor channel activity
  int  monitors()    {return m_monitorListeners.size();}
  void monitor(MmsMediaDevice* deviceIP) {m_monitorListeners.push_back(deviceIP);}
  int  isMonitor    (MmsMediaDevice* deviceIP);
  int  removeMonitor(MmsMediaDevice* deviceIP);
 
  void clearMonitorListeners() 
  { m_monitorListeners.erase(m_monitorListeners.begin(),m_monitorListeners.end()); 
  }    
                             
  mmsTimeslotHandle monitorListenTimeslot;  // Conference monitor channel

  // Conference descriptor table (CDT)
  // This table always has one entry for each conferee in the conference.
  // chan_num: SCbus TRANSMIT timeslot # of device to be included in conference
  // chan_sel: Defines meaning of chan_num: always MSPN_TS for current HMP vers
  // chan_attr For some HMP conferencing commands, a bitmask representing the 
  //           conferee's attributes, which can be any of:
  //           MSPA_NULL | MSPA_RO | MSPA_TARIFF | MSPA_COACH | MSPA_PUPIL
  //
  // chan_lts  Same location as chan_attr. Redefinition of chan_attr, for 
  //           commands in which this entry stores the bus LISTEN timeslot.
  //           When HMP adds a conferee to a conference, it places the conferee
  //           timeslot number here. When the conferee IP or voice device listens   
  //           to the conference, this is the timeslot it listens to. 
          
  enum{CDT_SIZE_DELTA = MMS_MAX_INITIAL_CONFEREES}; 
  MMS_CDT* m_cdt;                           // {int chan_num, chan_sel, chan_attr}
  int     m_cdtSize;  
  void    logcdt();
  void    cdtRealloc();

  MmsHmpConference(const int publicConfID);
  virtual ~MmsHmpConference();

  int  m_publicConferenceID;                // public conference id
  int  m_hmpConferenceID;                   // hmp conference id
  int  m_conferees; 
  std::vector<MmsMediaDevice*> m_monitorListeners; 
  MmsMediaDevice*  coach;
  MmsMediaDevice*  pupil; 
  ACE_Thread_Mutex xlock;
  char objname[5];
};



class MmsHmpConferenceMap
{
  // Tracks active HMP conferences by HMP conference ID
  protected:
  static MmsHmpConferenceMap* m_instance; 
  MmsHmpConferenceMap() { }
  MmsHmpConferenceMap& operator=(const MmsHmpConferenceMap&) { };
  std::map<int, MmsHmpConference*> conferences;

  public:
  static MmsHmpConferenceMap* instance()    // Singleton
  {
    if   (!m_instance) 
           m_instance = new MmsHmpConferenceMap;  
    return m_instance;
  }

  virtual ~MmsHmpConferenceMap();  

  int  put(MmsHmpConference* conference);   // Add an instantiated conference 

  int  remove(int conferenceID);            // Delete & remove conference

  void removeAll();                         // Delete & remove all conferences

  void destroy();                           // Delete singleton instance

  int  size() {return conferences.size();}  // Return # of conferences

  MMS_CDT* cdt(const int conferenceID, const mmsTimeslotHandle timeslotnumber);

  MmsHmpConference* get(int conferenceID);  // Return conference[publicConferenceID]
                                            // Find by internal conference ID
  MmsHmpConference* getByInternalID(const int internalConferenceID);  
                                            // Get internal ID given external ID
  int getPublicConferenceID(const int internalConferenceID);

  ACE_Thread_Mutex zlock;
}; 



struct MMS_CDT: public MS_CDT
{
  // Conference party attributes. We extend the HMP MS_CDT structure in order to
  // save per-conferee information we find useful. We store a duplicate of conferee
  // attribute bitflags in order to avoid use necessity for use of the dcb_getcde API.
  unsigned int confereeAttrs;

  MMS_CDT() { memset(this,0,sizeof(MMS_CDT)); }
};


#endif


