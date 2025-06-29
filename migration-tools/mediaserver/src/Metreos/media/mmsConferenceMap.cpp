//
// mmsConferenceMap.cpp 
//
// Thin wrapper for HMP conference and associated CDT, and conference collection 
//
#include "StdAfx.h"
#ifdef MMS_WINPLATFORM
#pragma warning(disable:4786)
#endif
#include "mmsConferenceMap.h"

#ifdef MMS_ENABLE_MEMORY_LEAK_DETECTION
#define new MMS_MEMLEAK_NEW
#endif // MMS_ENABLE_MEMORY_LEAK_DETECTION

MmsHmpConferenceMap* MmsHmpConferenceMap::m_instance = 0;
    
                                           
                                            
int MmsHmpConferenceMap::put(MmsHmpConference* conference)     
{ 
  // Add an instantiated conference 
   
  ACE_Guard<ACE_Thread_Mutex> x(zlock); 
  int result = 0;

  const int isPresent = (conferences.size() == 0)?  FALSE:
            conferences.end() != conferences.find(conference->publicCid());
 
  if  (isPresent)
       result = -1;
  else conferences[conference->publicCid()] = conference;

  return result;
}


                                            
MmsHmpConference* MmsHmpConferenceMap::get(const int conferenceID)   
{
  // Return conference[externalConferenceID]

  ACE_Guard<ACE_Thread_Mutex> x(zlock);      
  if  (conferences.size() < 1) return NULL;

  std::map<int, MmsHmpConference*>::iterator i = conferences.find(conferenceID);

  return (i == conferences.end())? NULL: i->second;
}



MmsHmpConference* MmsHmpConferenceMap::getByInternalID(const int internalConferenceID)
{
  // Return conference having the supplied internal (HMP) ID

  ACE_Guard<ACE_Thread_Mutex> x(zlock); 

  const int publicConferenceID = this->getPublicConferenceID(internalConferenceID);
  if (internalConferenceID == -1) return NULL;
  
  return this->get(publicConferenceID);
}



int MmsHmpConferenceMap::remove(int conferenceID)
{
  ACE_Guard<ACE_Thread_Mutex> x(zlock); 
  int  result = 0;     
  
  MmsHmpConference* conference = this->get(conferenceID);

  if  (conference)
  {
       delete conference;
       conferences.erase(conferences.find(conferenceID));
       result = 1;
  }  

  return result; 
}



void MmsHmpConferenceMap::removeAll()
{
   ACE_Guard<ACE_Thread_Mutex> x(zlock);      

   if  (conferences.size() == 0) return;
   std::map<int, MmsHmpConference*>::iterator i = conferences.begin();

   for(; i != conferences.end(); i++)
   {
       if  (i->second)
            delete i->second;
   }

   conferences.erase(conferences.begin(),conferences.end());
}



void MmsHmpConferenceMap::destroy()
{
  if  (m_instance)
       delete m_instance;

  m_instance = NULL;
}



int MmsHmpConferenceMap::getPublicConferenceID(const int internalConferenceID)
{
  // Return public conference ID of conference having the supplied internal ID,
  // or -1 if a conference having that internal ID does not exist.

  ACE_Guard<ACE_Thread_Mutex> x(zlock); 

  if  (conferences.size() == 0) return -1;
  int  returnedID = -1;
  std::map<int, MmsHmpConference*>::iterator i = conferences.begin();

  for(; i != conferences.end(); i++)
  {
      MmsHmpConference* conference = i->second;

      if (conference && conference->hmpCid() == internalConferenceID)
      {   returnedID = conference->publicCid();
          break;
      }     
  }

  return returnedID;
}



MMS_CDT* MmsHmpConferenceMap::cdt(const int conferenceID, const mmsTimeslotHandle timeslotnumber)
{
  MMS_CDT* cdtentry = NULL; MmsHmpConference* conf = NULL;

  if  (NULL != (conf = this->get(conferenceID)))
       cdtentry = conf->getCdt(timeslotnumber);

  return cdtentry;
}



MmsHmpConferenceMap::~MmsHmpConferenceMap()  
{
  // Singleton instance is destroyed explicitly
}



// - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - 
// MmsHmpConference
// - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - 

MmsHmpConference::MmsHmpConference(const int publicConfID)  
{                                           // Ctor
  m_publicConferenceID = publicConfID? publicConfID: Mms::getNewPublicConferenceID();
  m_hmpConferenceID = 0;

  ACE_OS::sprintf(objname,"C%03d",m_publicConferenceID);
  m_conferees = 0;
  monitorListenTimeslot = 0;
  coach = pupil = NULL;

  m_monitorListeners.erase(m_monitorListeners.begin(),m_monitorListeners.end()); 

  enum{CDT_SIZE_DELTA = MMS_MAX_INITIAL_CONFEREES}; 
  m_cdtSize = CDT_SIZE_DELTA;               
                                            // CDT starts out with 
  m_cdt = new MMS_CDT[m_cdtSize];           // CDT_SIZE_DELTA entries
  // Now that we extend HMP's CDT struct, each CDT entry now inits itself to zero  
  // memset(m_cdt, 0, sizeof(MMS_CDT) * CDT_SIZE_DELTA);
}



MmsHmpConference::~MmsHmpConference()       // Dtor
{
  if (m_cdt)
      delete[] m_cdt;
}



MMS_CDT* MmsHmpConference::setCdt
( const int i, const int chanNum, const int chanAttr, const int mmsAttrs)
{
  // Sets the i'th entry in the conference's conference descriptor table

  ACE_ASSERT(i < m_cdtSize + CDT_SIZE_DELTA-1);  
  if (i >= m_cdtSize)                       // If CDT at capacity ...
      cdtRealloc();                         // ... increase CDT size

  MMS_CDT* cdtEntry   = &m_cdt[i];           
  cdtEntry->chan_num  = chanNum;            // Xmit timeslot# of conferee device 
  cdtEntry->chan_sel  = MSPN_TS;            // Constant 0x10
  cdtEntry->chan_attr = chanAttr;           // Conferee attrib bits (or listen TS#)   
  cdtEntry->confereeAttrs = mmsAttrs;       // Our copy of conferee attrib bits    

  // chan_num: SCbus TRANSMIT timeslot # of device to be included in conference
  // chan_sel: Defines meaning of chan_num: must be MSPN_TS for current HMP ver
  // chan_attr Describes conferee's attributes (see pp.34) chosen from among:
  //           MSPA_NULL | MSPA_RO | MSPA_TARIFF | MSPA_COACH | MSPA_PUPIL
  // chan_lts  Redefinition of chan_attr, for the situations where this
  //           location is used to store the SCBus LISTEN timeslot number, 
  //           which in conjuction with the chan_num transmit timeslot number, 
  //           comprises a bus connection
  //
  // confereeAttrs: An extension to HMP's MS_CDT struct added by Cisco/Metreos. 
  //           These 32 bits hold the conferee attribute bits as described above.
  //           Since chan_attr can be overwritten by other API calls, we save
  //           the conferee attribute state here to avoid having to invoke
  //           dcb_getcde each time we need to query a conferee attribute.

  return cdtEntry;
}


                                            
MMS_CDT* MmsHmpConference::getCdt(const mmsTimeslotHandle timeslotnumber)
{ 
  // Return CDT entry matching timeslot
  MMS_CDT* p = m_cdt;

  for(int i = 0; i < m_conferees; i++, p++)
      if (p->chan_num == timeslotnumber) return p;
      
  return NULL;
}


                                            
MMS_CDT* MmsHmpConference::getCdt(const int confereeIndex)
{ 
  // Return CDT entry matching timeslot
  MMS_CDT* cdt = confereeIndex >= 0 && confereeIndex < m_cdtSize?
          &m_cdt[confereeIndex]: NULL;  
  return cdt;
}



void MmsHmpConference::cdtRealloc()
{   
  // Increases size of conference descriptor table by CDT_SIZE_DELTA entries                                        
  const int saveLength = sizeof(MMS_CDT) * m_cdtSize;
  unsigned char* saveBuf = new unsigned char[saveLength];
  memcpy(saveBuf, m_cdt, saveLength);
  delete[] m_cdt;
  
  m_cdt = new MMS_CDT[m_cdtSize += CDT_SIZE_DELTA]; 
  memset(m_cdt, 0, sizeof(MMS_CDT) * m_cdtSize);
  memcpy(m_cdt, saveBuf, saveLength);
  delete[] saveBuf;
  MMSLOG((LM_DEBUG,"%s CDT reallocated to %d\n",objname,m_cdtSize));
}



void MmsHmpConference::logcdt()             // Diagnostic log of CDT entries
{
  MMS_CDT* p = m_cdt;                         
  for(int i = 0; i < m_cdtSize; i++, p++)    
  {   if (p->chan_num + p->chan_sel + p->chan_lts == 0) break;
      MMSLOG((LM_INFO,"%s CDT%d %d %d %d\n",objname, i,
              p->chan_num, p->chan_sel, p->chan_lts));
  }
}


                                            
int MmsHmpConference::cdtRemove(MMS_CDT* cdtEntry)
{ 
  // Removes entry from CDT    
  MMS_CDT* p = m_cdt;                                       
  for(int i = 0; i < m_conferees; i++, p++)
      if  (memcmp(cdtEntry, p, sizeof(MMS_CDT)) == 0)
           return this->cdtRemove(i);
            
  return -1;     
}


                                            
int MmsHmpConference::cdtRemove(const int confereeIndex)
{  
  // Removes entry[i] from CDT                                          
  ACE_ASSERT(confereeIndex >= 0 && confereeIndex < m_conferees);

  for(int i = confereeIndex; i < m_conferees; i++)
      m_cdt[i] = m_cdt[i+1];
  
  memset(&m_cdt[m_conferees-1], 0, sizeof(MMS_CDT));

  m_conferees--;

  return 0;
}



int MmsHmpConference::isMonitor(MmsMediaDevice* deviceIP)
{
  for(int i=0; i < (int)m_monitorListeners.size(); i++)
      if (m_monitorListeners[i] == deviceIP) return TRUE;

  return FALSE;
}


                                            
int MmsHmpConference::isConferee(MmsMediaDevice* deviceIP)
{ // Is IP a conferee?     
  return this->confereeIndex(deviceIP) != -1;
}


                                            
int MmsHmpConference::confereeIndex(MmsMediaDevice* deviceIP)
{ 
  // Return CDT index of conferee
  MMS_CDT* p = m_cdt;
  mmsTimeslotHandle ipTimeslot = deviceIP->timeslotNumber();

  for(int i = 0; i < m_cdtSize; i++, p++)
  {
      #if 0   // debugging display of conference descriptor table entries
      MMSLOG((LM_DEBUG,"---- confereeIndex %d: %d %d\n",i,ipTimeslot,p->chan_num));
      #endif

      if (p->chan_num == ipTimeslot) return i;
  }
      
  return -1;
}



int MmsHmpConference::removeMonitor(MmsMediaDevice* deviceIP)
{
  for(int i=0; i < (int)m_monitorListeners.size(); i++)
  {                                         // Remove entry from monitors list
      if  (m_monitorListeners[i] == deviceIP)
      {    m_monitorListeners.erase(m_monitorListeners.begin() + i);
           return 0;
      }
  }

  return -1;
}



void MmsHmpConference::teardown()
{                                           
  if  (m_monitorListeners.size())           // Disconnect all monitors
  {
       for(int i=0; i < (int)m_monitorListeners.size(); i++)
           m_monitorListeners[i]->unlisten();

       clearMonitorListeners(); 
  }
                                          
  m_conferees = 0;
}


