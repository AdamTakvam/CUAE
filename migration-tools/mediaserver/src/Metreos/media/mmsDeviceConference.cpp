//
// mmsDeviceConference.cpp
// HMP conference object
//
#include "StdAfx.h"
#ifdef  MMS_WINPLATFORM
#pragma warning(disable:4786)
#endif
#include "mmsDeviceConference.h"
#include "mmsMediaResourceMgr.h"
#include "mmsReporter.h"

#ifdef MMS_ENABLE_MEMORY_LEAK_DETECTION
#define new MMS_MEMLEAK_NEW
#endif // MMS_ENABLE_MEMORY_LEAK_DETECTION

const static char* xis = "is now", *xisnot = "removes", *resxcatcnfx = "CNFX";


                                             
MmsDeviceConference::MmsDeviceConference(int ordinal, MmsConfig* config): 
 MmsMediaDevice(DEVICETYPE::CONF, ordinal, config)
{ 
  ACE_OS::strcpy(devname,"DEVC"); 
  conferences = MmsHmpConferenceMap::instance();
}


                                            
mmsDeviceHandle MmsDeviceConference::open(OPENINFO& openinfo, unsigned short mode) 
{  
  int deviceOrdinal = openinfo.key.card, unusedparam = 0;
  HMPDEVICEID(deviceID, "dcb", openinfo.key.board, deviceOrdinal);

  if (-1 == (m_handle = dcb_open(deviceID, unusedparam))) 
  {
      MMSLOG((LM_ERROR,"DEVC %s dcb_open %s %s\n",em,deviceID,ATDV_ERRMSGP(m_handle)));     
      return -1;                     
  } 
  else MMSLOG((LM_INFO,"DEVC opened %s as %d\n",deviceID,m_handle));

  this->init();

  m_deviceState = AVAILABLE;
  m_key = openinfo.key;
  return m_handle;
}

  

void MmsDeviceConference::close()           // Close conferencing resource
{  
  // Reset handle and states before closing the device.  We do this because 
  // HMP calls are time consuming and some other thread may try to access 
  // the device while dcb_close is in progress, causing problems.
  mmsDeviceHandle h = m_handle;

  m_mediaState  = MEDIAIDLE;
  m_deviceState = CLOSED;
  m_handle = 0;

  conferences->instance()->removeAll();

  if (h > 0)                  
      dcb_close(h);
}


                                             
mmsConfereeHandle MmsDeviceConference::create
( const int publicConfID, MmsMediaDevice* deviceIP, 
  const unsigned int conferenceAttrs, const unsigned int partyAttrs)
{
  // Establish a conference: Initially establish a conference of 1 party
  // Returns conferee 1 handle (xmit timeslot), and HMP conference ID; or -1
  // Conference attributes:
  // MSCA_NULL: No attributes
  // MSCA_ND:   Sound tone always when participant added or removed
  // MSCA_ND | MSCA_NN: No tone for receive-only or monitor participants 
  // Conferee attributes: MSPA_NULL|MSPA_RO|MSPA_TARIFF|MSPA_COACH|MSPA_PUPIL

  // Due to HMP imposition of a maximum on number of conferences, and that
  // HMP state becomes permanently hosed if we attempt to exceed the maximum,  
  // we must synchronize the creation of conferences.

  ACE_ASSERT(m_handle > 0); 
  unsigned int confereeAttrs = partyAttrs;

  if (m_config->media.agcDisableConferee)    
      confereeAttrs |= MSPA_NOAGC;          // Disable gain control per config
   
  MmsHmpConference* conference = new MmsHmpConference(publicConfID);
  BUSCONNECTPARAMS params(0, 0, conferenceAttrs, confereeAttrs, conference);
  mmsConfereeHandle confereeHandle = -1;

  conferences->zlock.acquire();

  const int maxConferenceResources = MmsAs::maxConf; 
  int isConferenceResourcesExhausted = TRUE, isConferenceResourceReserved = FALSE;

  const int activeConferences      = conferences->size();
  const int maxConferences         = min(MmsAs::insG711 / 2, MmsAs::insConf / 2);
  const int availableConferences   = maxConferences - activeConferences;
  const int isConferencesExhausted = availableConferences <= 0; 

  if  (isConferencesExhausted)              
  {
       this->raiseConferencesExhaustedAlarm();
  }    // LICX CONFOUT 1 of 2, CONF- 1 of 2: 
  else // Reserve a conferencing (== conferee) resource, if available:
       isConferenceResourcesExhausted = MmsAs::conf(MmsAs::RESX_ISZERO, MmsAs::RESX_RESERVE);

  if  (isConferenceResourcesExhausted) 
  {
       this->raiseConferenceResourcesExhaustedAlarm();
  }
  else
  {    isConferenceResourceReserved = TRUE;
       // Listen up the initial conferee 
       confereeHandle = this->busConnect(deviceIP, (int)&params);
  }                                         

  conferences->zlock.release();

  if  (isBadConfereeHandle(confereeHandle)) 
  {    
       if (isConferenceResourceReserved)    // LICX CONF+ 1 of 7
           MmsAs::conf(MmsAs::RESX_INC);    // Un-reserve the resource license

       delete conference;
       return -1;
  }

  conference->hmpCid(params.hmpCid);
  conference->cdtAdd(); 

  this->logCoachPupil(conference, deviceIP, confereeAttrs);
 
  conferences->put(conference);             // Insert to conferences table 

  // Log and publish conference (not conferee) stats
  MmsAs::logResxUsageActivity(resxcatcnfx, MmsAs::RESX_DEC, availableConferences-1);
  MmsAs::raiseStatsEvent(MMS_STAT_CATEGORY_CONFERENCES, MmsAs::RESX_DEC, 
         activeConferences, activeConferences+1);                                              

  return confereeHandle;
}


                                            
mmsConfereeHandle MmsDeviceConference::join
( int publicCid, MmsMediaDevice* deviceIP, const unsigned int partyAttrs)
{
  // Join a conference; returns new conferee handle (conferee xmit timeslot) or -1 
  // Conferee attributes: MSPA_NULL|MSPA_RO|MSPA_TARIFF|MSPA_COACH|MSPA_PUPIL
  // Note that with the advent of utility sessions, the media device parameter  
  // referenced here as deviceIP, might actually be a voice device. 

  ACE_ASSERT(m_handle > 0);                  
  unsigned int confereeAttrs = partyAttrs;

  if (m_config->media.agcDisableConferee)   
      confereeAttrs |= MSPA_NOAGC;

  MmsHmpConference* conference = conferences->get(publicCid);
  if (NULL == conference) return -1;
  int result = 0;   
                                            // Get exclusive on the conference
  ACE_Guard<ACE_Thread_Mutex> x(conference->xlock);       
  
  // When coach or pupil is specified and we already have one, we replace                                          
  if ((partyAttrs & MSPA_COACH) && conference->coach)   
       result = this->setCoach(publicCid, conference->coach, FALSE);   
  else
  if ((partyAttrs & MSPA_PUPIL) && conference->pupil)
       result = this->setPupil(publicCid, conference->pupil, FALSE);
  
  if (result == -1) return -1;


  BUSCONNECTPARAMS params   
   (conference->hmpCid(), conference->size(), 0, confereeAttrs, conference);

  // Hook the conferee device up (listen) to the conference. If the conferenced
  // session is a utility session, the device listening to the conference,
  // referenced here as deviceIP, will actually be a voice device (this is 
  // the case when we are playing to or recording a conference)   

  mmsConfereeHandle confereeHandle = -1;
  const int isConferenceSlotsExhausted 
           = (conference->size() >= HMP_MAX_PARTIES_PER_CONFERENCE);
  int isConferenceResourcesExhausted = TRUE, isConferenceResourceReserved = FALSE;

  if  (isConferenceSlotsExhausted)
       MMSLOG((LM_ERROR,"DEVC conference is at maximum %s participants\n",
               HMP_MAX_PARTIES_PER_CONFERENCE));
       // Reserve a conference resource if available: LICX CONFOUT 2 of 2, CONF- 2 of 2
  else isConferenceResourcesExhausted = MmsAs::conf(MmsAs::RESX_ISZERO, MmsAs::RESX_RESERVE);
  
  if  (isConferenceResourcesExhausted) 
  {
       this->raiseConferenceResourcesExhaustedAlarm();
  }
  else
  {    isConferenceResourceReserved = TRUE;
       // Listen up the new conferee 
       confereeHandle = this->busConnect(deviceIP, (int)&params);
  }   

  if  (isBadConfereeHandle(confereeHandle)) 
  {    
       if (isConferenceResourceReserved)    // LICX CONF+ 2 of 7
           MmsAs::conf(MmsAs::RESX_INC);    // Un-reserve the resource license

       return -1;
  }

  conference->cdtAdd(); 

  this->logCoachPupil(conference, deviceIP, confereeAttrs); 
 
  return confereeHandle;
}


                                            
int MmsDeviceConference::leave(int publicCid, MmsMediaDevice* deviceIP)
{
  // Remove the specified party from conference.
  // Note that the last party in conference does not come through this code.
  // Note that with the advent of utility sessions, the media device referenced  
  // here as deviceIP, might actually be a voice device. 

  ACE_ASSERT(m_handle > 0);         

  MmsHmpConference* conference = conferences->get(publicCid);
  if  (NULL == conference) return -1; 

  // Unhook the conferee device (unlisten) from the conference. If the conferenced
  // session is a utility session, the device unlistened from the conference,
  // referenced here as deviceIP, will actually be a voice device (this is 
  // the case when we are playing to or recording a conference)   
                                             
  this->busDisconnect(deviceIP, 0);         // Unlisten before leave  

  if  (conference->size() == 1)             // Last party?
       return leaveHost(publicCid);          
    
  conference->xlock.acquire();              // Get exclusive on the conference
                                            // Get conferee descriptor 
  MMS_CDT* cdtEntry = conference->getCdt(deviceIP->timeslotNumber()); 
  if  (NULL == cdtEntry) 
  {    conference->xlock.release();                          
       return -1;  
  }   
                                            // Update coach/pupil status
  this->updateConferenceAttrsOnLeave(conference, publicCid, deviceIP);                                                                                 
                                            // Remove conferee 
  int  result = dcb_remfromconf(m_handle, conference->hmpCid(), cdtEntry);

  if  (result == -1) 
       MMSLOG((LM_ERROR,"DEVC %s dcb_remfromconf %s\n",
               em, ATDV_ERRMSGP(m_handle))); 
                                  
  conference->cdtRemove(cdtEntry);          // Decrement conferees
                                            // LICX CONF+ (3 of 7)
  MmsAs::conf(MmsAs::RESX_INC);             // Increment available licenses   

  conference->xlock.release(); 
  return result;
}



int MmsDeviceConference::leaveHost(int publicCid)
{
  // Remove final party, tear down and delete HMP conference. Dialogic requires
  // destruction of a conference to effect removal of its final party. See next:
  // Note that the preceeding comment is no longer, or is not, true. 
  // There is (now) an API to remove the lone conferee from the conference shell.
  // Ideally the conference logic would be built from the ground up to utilize
  // this feature -- retrofitting it will be somewhat difficult, as all logic
  // assumes destruction of the conference and final conferee simultaneously.

  MmsHmpConference* conference = conferences->get(publicCid);
  if  (NULL == conference) return -1; 

  conference->xlock.acquire();
 
  // Get some counts for license and resource tracking purposes
  const int maxConferences = min(MmsAs::insG711 / 2, MmsAs::insConf / 2); 
  const int conferencesInUsePriorTeardown = conferences->size();
  const int conferencesInUseAfterTeardown = conferencesInUsePriorTeardown - 1;
  const int conferencesAvailablePriorTeardown = maxConferences - conferencesInUsePriorTeardown;
  const int conferencesAvailableAfterTeardown = conferencesAvailablePriorTeardown + 1;
  const int confereesRemainingPriorTeardown = conference->size(); 
             
  conference->teardown();                   // Remove any monitor listeners

  conference->xlock.release();  

  if (confereesRemainingPriorTeardown > 1)
      MMSLOG((LM_INFO,"DEVC %d parties abandoned conference %d\n", 
              confereesRemainingPriorTeardown, publicCid));      
   
  // Delete conference object, object references, and HMP conference
  const int result = this->teardown(publicCid, conference->hmpCid()); 

  // Log and publish conference (not conferee) stats
  MmsAs::logResxUsageActivity(resxcatcnfx, MmsAs::RESX_INC,
         conferencesAvailableAfterTeardown);

  MmsAs::raiseStatsEvent(MMS_STAT_CATEGORY_CONFERENCES, MmsAs::RESX_INC, 
         conferencesInUsePriorTeardown, conferencesInUseAfterTeardown); 

  // This is a spot to watch for potential license leaks. If client has abandoned 
  // a conference, there will be multiple conferees on entry to this method. 
  // If client is tearing down a conference one by one, there will be one conferee 
  // on entry. There could be a drone conferee in the mix as well. We must assume
  // here that the above teardown did not do any license count adjustment, since
  // we are doing it all here.  
         
  // Un-burn license(s) for conferencing resource(s) (== conferee(s)) remaining 
  // prior to teardown. Of course, the conferencing resource stats and logging 
  // happen here also.
  if  (confereesRemainingPriorTeardown == 1)// LICX CONF+ (4 of 7)                                              
       MmsAs::conf(MmsAs::RESX_INC);        // LICX CONF+ (5 of 7)
  else MmsAs::conf(MmsAs::RESX_INC, confereesRemainingPriorTeardown); 

  return result;    
}



int MmsDeviceConference::busConnect(MmsMediaDevice* deviceOther, int mode)
{
  // Connects conferee to another (IP or voice) resource on the bus.
  // Returns conferee SCBus transmit timeslot number (conferee handle), or -1
  //
  // Background notes on hooking up a conferee on the bus:
  //
  // 1. A conference object contains a MS_CDT struct for each conferee
  //
  // 2. We first assign ip session[i] to the new conferee.
  //
  // 3. We then join the new conferee to a conference by passing join() the 
  //    session's deviceOther->timeslot(), (specifically the sc_tsarrayp member 
  //    of the IP resource's SC_TSINFO). We do this by placing the IP timeslot 
  //    ID of the new conferee into conference.CDT[confereeindex].chan_num
  //    (If other device is voice, not IP, replace IP with voice in the above)
  //
  // 4. HMP->join() hooks up this IP timeslot to a conference timeslot, 
  //    places the conference timeslot into CDT[confereeindex].chan_lts 
  //    (lts means listen timeslot), and returns the listen timeslot
  //
  //  How to provide an analogy to the IP-to-voice busConnect()? 
  //  Example: confResource->busConnect(confereeSession->deviceOther);
  //                                        // Get bus timeslot connected to
  //                                        // the xmit of the IP device
  //  1. CDT[conferee].chan_num = deviceOther->timeslotNumber();
  //                                        // Make CONF device listen to slot 
  //  2. dcb_addtoconf(CDT[conferee]);      // the IP device is xmitting on
  //                                        // Get bus timeslot now connected 
  //                                        // to the xmit of this CONF device
  //  3. timeslotinfo.sc_tsarrayp = &(CDT[conferee].chan_lts);
  //                                        // Make IP device listen to slot 
  //                                        // this CONF device is xmitting on 
  //  4. confereeSession->deviceOther->listen(&timeslotinfo);
  //
  // Notes on reconnecting IP to conference after intervening unlisten.
  // 1. Recall that CDT entry chan_lts contains the conferee timeslot that the
  //    IP is to listen to.
  // 2. Recall that chan_lts is a #define of CDT entry chan_attr.
  // 3. After the initial hookup, any setting of conferee attributes will
  //    overwrite the timeslot entry with the attributes mask.
  // 4. Therefore in order to reconnect IP to conference, we must have saved  
  //    the conferee timeslot, using it to set the conferee CDT entry chan_lts
  // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
  BUSCONNECTPARAMS* p = (BUSCONNECTPARAMS*)mode; 
  int  result = 0;

  MmsHmpConference* conference = p->conference? p->conference : NULL;
                                  
  if  (NULL == conference) 
  {    MMSLOG((LM_ERROR,"DEVC no cmap entry for ID %d\n",p->hmpCid));
       return -1; 
  }

  // Get bus timeslot connected to transmit slot of the other device
  MMS_CDT* cdtEntry = NULL;                
                                                                                       
  if  (p->flags & BUSCONNECTPARAMS::LISTENONLY)
       cdtEntry = conference->getCdt(p->confereeIndex);
  else 
  if  (deviceOther)
       cdtEntry = conference->setCdt(p->confereeIndex,      
          (int)deviceOther->timeslotNumber(), p->confereeAttrs, p->confereeAttrs); 

  if  (cdtEntry == NULL) 
  {    MMSLOG((LM_ERROR,"DEVC no CDT entry for party %d slot %d\n",
               p->confereeIndex, deviceOther->timeslotNumber()));
       return -1; 
  }
                           
  // Unless this call is to listen only (such as if the existing conference  
  // is to listen to the session's voice resource) move the IP session into 
  // the indicated conference (if an HMP conferencing resource is available).
  // Note that if we are doing a listen only, the timeslot to which the  
  // device we are adding to the conference is to listen, is passed in the 
  // confereeAttrs BUSCONNECTPARAMS parameter.
                                           
  if  (p->flags & BUSCONNECTPARAMS::LISTENONLY)
  {                                         // Conferee timeslot passed
       cdtEntry->chan_lts = p->confereeAttrs;  
  }
  else
  if  (this->resourcesRemaining() == 0)
  {   
       MMSLOG((LM_ERROR,"DEVC conference resources exhausted\n"));
       return -1;
  }                      
  else
  if  (p->confereeIndex == 0)               // New conference?
  {
       this->handleConferenceAttributes(p, cdtEntry, TRUE); 

       result = dcb_estconf(m_handle, cdtEntry, 1, p->conferenceAttrs, &p->hmpCid); 
  }                                          
  else                                      // Existing conference
  {    this->handleConferenceAttributes(p, cdtEntry, FALSE); 

       result = dcb_addtoconf(m_handle, p->hmpCid, cdtEntry);
  }

  if  (result == -1)                        // Create or join error?
  {    const char* which = p->confereeIndex? "dcb_addtoconf": "dcb_estconf";
       MMSLOG((LM_ERROR, "DEVC %s %s %s\n", em, which, ATDV_ERRMSGP(m_handle)));
       return -1;
  } 

  if  (m_config->diagnostics.flags & MMS_DIAG_LOG_CDT)
       conference->logcdt();                                        
            
                                            // Get bus timeslot now connected
  SC_TSINFOEX tsinfo;                       // to the conference xmit  
  mmsTimeslotHandle*   pConfXmitSlot = (mmsTimeslotHandle*)&cdtEntry->chan_lts; 
  tsinfo.sc_tsarrayp = pConfXmitSlot;
                                            // Tell the other device to listen    
  result = deviceOther->listen(&tsinfo);    // to the conference xmit timeslot

  if  (result != -1)                        // Listened OK?
  {   
       if (m_config->diagnostics.flags & MMS_DIAG_LOG_TIMESLOTS)
           this->logConfereeTimeslots(deviceOther, cdtEntry); 
  }
                                            // Return conferee timeslot 
  return result == -1? -1: cdtEntry->chan_lts;
}


                                             
int MmsDeviceConference::busDisconnect(MmsMediaDevice* deviceOther, unsigned int)
{
  return deviceOther? deviceOther->unlisten(): -1;
}



void MmsDeviceConference::handleConferenceAttributes
( BUSCONNECTPARAMS* bcp, MMS_CDT* cdt, const int isNew) 
{
  // Do whatever we need to do with the conference and conferee attribute 
  // bitflags prior to creating a conference and/or a conferee

  if  (bcp == NULL || cdt == NULL) return;

  // Save locally the conferee attributes supplied with the create or join.
  // However if the join is a listen only, the confereeAttrs parameter holds
  // the listen timeslot, the listen only conferee attribute is implicit,
  // and we have no explicit conferee attributes.
  if ((bcp->flags & BUSCONNECTPARAMS::LISTENONLY) == 0)
  {    
       cdt->confereeAttrs = bcp->confereeAttrs; 

       if (m_config->diagnostics.flags & MMS_DIAG_LOG_CONFX_ATTRS)
           this->logConferenceAttributes(bcp, isNew, FALSE);
  }
}



MmsHmpConference* MmsDeviceConference::getConference(int publicCid) 
{
  return this->conferences->get(publicCid);
}



int MmsDeviceConference::connectResource(const int publicCid, const int timeslot,
  MmsMediaDevice* deviceIP, MmsMediaDevice* newdevice, MmsMediaDevice* olddevice)
{
  // Connect an HMP resource to a conference. The session IP resource is
  // is by default connected to the conference as the session joins the 
  // conference; however if we want the conference to listen to the voice
  // resource of a session which is a conference party, we unhook the IP 
  // resource and connect the session vox resource. We can subsequently do 
  // the reverse, if we wish to reconnect the session to the conference. 

  // Device identified here as deviceIP could in fact be a voice device 
  // if we are connecting voice device to conference in a utility session

  MmsHmpConference* conference = conferences->get(publicCid);
  if  (NULL == conference) 
  {    MMSLOG((LM_ERROR,"DEVC lookup failed for confx %d\n", publicCid));
       return -1; 
  }  

  ACE_Guard<ACE_Thread_Mutex> x(conference->xlock); 

  const int hip = deviceIP? deviceIP->handle(): -1;  
  if  (!isValidDeviceHandle(hip)) 
  {    MMSLOG((LM_ERROR,"DEVC invalid IP device %d\n", hip));
       return -1;
  }     

  const int confereeIndex = conference->confereeIndex(deviceIP);
  if  (confereeIndex == -1) 
  {    MMSLOG((LM_INFO,"DEVC lookup failed for party IP%d\n", hip));
       return -1;
  }       

  if  (olddevice) this->busDisconnect(olddevice, 0);

  BUSCONNECTPARAMS params(conference->hmpCid(), confereeIndex, 0, timeslot, conference);
  params.flags |= BUSCONNECTPARAMS::LISTENONLY;

  const int timeslotnum = this->busConnect(newdevice, (int)&params);

  return timeslotnum;
}



int MmsDeviceConference::addMonitorChannel(int publicCid)
{
  // Adds a monitor to the conference. This is a timeslot on which kibitzers
  // may listen but not participate. The monitor channel uses one conferencing
  // resource, but not a CDT entry. Each monitor consumes only an IP resource.

  MmsHmpConference* conference = conferences->get(publicCid);
  if  (NULL == conference) return -1;   
  ACE_Guard<ACE_Thread_Mutex> x(conference->xlock);
     
  if  (conference->monitorListenTimeslot) return -1;

  int isConferenceResourceReserved = FALSE, result = -1;

  const int isConferenceResourcesExhausted  // Check for out of resx, reserve if not
      = MmsAs::conf(MmsAs::RESX_ISZERO, MmsAs::RESX_RESERVE);

  if  (isConferenceResourcesExhausted) 
  {
       this->raiseConferenceResourcesExhaustedAlarm();
  }
  else
  {    isConferenceResourceReserved = TRUE;
       // Add monitor resource to conference 
       result = dcb_monconf(m_handle, conference->hmpCid(), &conference->monitorListenTimeslot); 

       if (result == -1) MMSLOG((LM_ERROR,"DEVC %s dcb_monconf %s\n",em,ATDV_ERRMSGP(m_handle)));
  }  

  if  (result == -1)
  {
       if (isConferenceResourceReserved)    // LICX CONF+ 6 of 7
           MmsAs::conf(MmsAs::RESX_INC);    // Un-reserve the resource license
  }

  conference->clearMonitorListeners(); 

  return result;
}



int MmsDeviceConference::removeMonitorChannel(int publicCid)
{
  // Remove the monitor component from the conference, freeing one resource.
  // We must unlistenOnMonitorChannel(ip) for each monitor before doing this

  MmsHmpConference* conference = conferences->get(publicCid);
  if  (NULL == conference) return -1;        
  ACE_Guard<ACE_Thread_Mutex> x(conference->xlock);

  if  (!conference->monitorListenTimeslot || conference->monitors()) return -1; 
    
  int  result = dcb_unmonconf(m_handle, conference->hmpCid());         

  if  (result == -1)
       MMSLOG((LM_ERROR,"DEVC %s dcb_unmonconf %s\n",em,ATDV_ERRMSGP(m_handle)));

  MmsAs::conf(MmsAs::RESX_INC);             // LICX CONF+ (7 of 7)

  conference->monitorListenTimeslot = 0;

  return 0;
}


                                            
int MmsDeviceConference::listenOnMonitorChannel
( int publicCid, MmsMediaDevice* deviceIP)
{
  // Add a monitor party to conference (no conference resource consumed)

  MmsHmpConference* conference = conferences->get(publicCid);
  if  (NULL == conference) return -1;  
  ACE_Guard<ACE_Thread_Mutex> x(conference->xlock);      
  if (!conference->monitorListenTimeslot) return -1;

  SC_TSINFOEX tsinfo;                        
  tsinfo.sc_tsarrayp = &conference->monitorListenTimeslot;
                                            // Make the IP device listen to  
  int  result = deviceIP->listen(&tsinfo);  // conference monitor timeslot

  if  (result == -1)
       MMSLOG((LM_ERROR,"DEVC %s IP listen\n",em));
   
  else conference->monitor(deviceIP);       // Add to list 

  return result;
}


                                            
int MmsDeviceConference::unlistenOnMonitorChannel
( int publicCid, MmsMediaDevice* deviceIP)
{
  // Remove a monitor party from conference (no conference resource involved)

  MmsHmpConference* conference = conferences->get(publicCid);
  if  (NULL == conference) return -1;        
  ACE_Guard<ACE_Thread_Mutex> x(conference->xlock);

  mmsTimeslotHandle ipTimeslot = deviceIP->timeslotNumber();

  int result = deviceIP->unlisten();        // Stop listening to monitor channel

  if (result == -1)
      MMSLOG((LM_ERROR,"DEVC %s IP unlisten\n",em));

  conference->removeMonitor(deviceIP);

  return result;
}



int MmsDeviceConference::raiseConferenceResourcesExhaustedAlarm()
{
  // Fire an alarm indicating conference resources exhausted, also log the condition

  char alarmDescription[MmsAlarmParams::maxTextLength+1];
  sprintf(alarmDescription, MmsAs::szResxExhausted, "conferencing");

  MMSLOG((LM_ERROR,"DEVC %s\n", alarmDescription));

  MmsAlarmParams* params = new MmsAlarmParams(MmsReporter::NDX_NO_MORE_CONFRESX_IN_SVC, 
     MMS_STAT_CATEGORY_CONFRESX, MmsReporter::severityTypeSevere);
  params->settext(alarmDescription);

  MmsReporter::instance()->postMessage(MMSM_ALARMREQUEST, (long)params);

  return -1;
}



int MmsDeviceConference::raiseConferencesExhaustedAlarm()
{
  // Fire an alarm indicating conferences exhausted, also log the condition.
  // Dialogic establishes max number of conferences as RTP resources / 2.

  char alarmDescription[MmsAlarmParams::maxTextLength+1];
  sprintf(alarmDescription, MmsAs::szResxExhausted, "conferences");

  MMSLOG((LM_ERROR,"DEVC %s\n",alarmDescription));

  MmsAlarmParams* params = new MmsAlarmParams(MmsReporter::NDX_NO_MORE_CONFERENCES, 
     MMS_STAT_CATEGORY_CONFERENCES, MmsReporter::severityTypeSevere);
  params->settext(alarmDescription);

  MmsReporter::instance()->postMessage(MMSM_ALARMREQUEST, (long)params);

  return -1;
}



int MmsDeviceConference::raiseConferenceSlotsExhaustedAlarm()
{
  // Fire an alarm indicating conferences slots exhausted, also log the condition.
  // Dialogic establishes max number of conference participants per conference at 254.

  char alarmDescription[MmsAlarmParams::maxTextLength+1];
  sprintf(alarmDescription, MmsAs::szResxExhausted, "conference party");

  MMSLOG((LM_ERROR,"DEVC %s\n",alarmDescription));

  MmsAlarmParams* params = new MmsAlarmParams(MmsReporter::NDX_NO_MORE_SLOTS_IN_CONF, 
     MMS_STAT_CATEGORY_CONFSLOTS, MmsReporter::severityTypeSevere);
  params->settext(alarmDescription);

  MmsReporter::instance()->postMessage(MMSM_ALARMREQUEST, (long)params);

  return -1;
}



int MmsDeviceConference::teardown(int publicCid, int hmpCid)
{
  // Tear down and delete indicated conference. Note that conference may 
  // already have been removed. due for example to removing the last conferee. 

  // Changed to not dcb_delconf if we don't have a record of the conference ID.
  // In the unlikely event we want to back this out, revert to an unconditional 
  // dcb_delconf here, and remove this->teardown(conferenceID) from 
  // this->leaveHost(), replacing it with conferences->remove(id); 

  int  result = 0;

  int  count  = conferences->remove(publicCid);
                                             
  if  (count)                               // Free all conf resources in use
       result = dcb_delconf(m_handle, hmpCid);

  if  (result == -1) 
       MMSLOG((LM_ERROR,"DEVC %s dcb_delconf %d %s\n",em,publicCid,ATDV_ERRMSGP(m_handle)));
  else MMSLOG((LM_INFO, "DEVC conference %d released\n",publicCid & 0xffff));

  return result;
}



// - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
// Support methods
// - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
                                                                                        
int MmsDeviceConference::isConferee(int publicCid, MmsMediaDevice* deviceIP)
{
  // Is specified IP a conferee in the specified conference?

  MmsHmpConference*  conference = conferences->get(publicCid);
  return conference? conference->isConferee(deviceIP): 0;
}


                                            
int MmsDeviceConference::isActiveConferenceID(int publicCid, MmsMediaDevice* deviceIP)
{
  // Is specified conference ID in use?

  MmsHmpConference*  conference = conferences->get(publicCid);
  if (conference == NULL) return 0;

  int attrs = this->getConfereeAttributes(conference->hmpCid(), deviceIP, FALSE);
  const int result = (attrs & ATTRIBUTE_ERROR) == 0;
  return result;
}



int MmsDeviceConference::getConfereeAttributes(const int externalID, MmsMediaDevice* deviceIP)
{
  // Retrieves conferee attribute bits given external conference ID and conferee's IP

  int attrs = ATTRIBUTE_ERROR; 
  MmsHmpConference* conference = conferences->get(externalID);    
  if (NULL == conference) return attrs; 
        
  MMS_CDT* cdtEntry = conference->getCdt(deviceIP->timeslotNumber());
  if (NULL == cdtEntry) return attrs; 

  #ifdef MMS_USE_DCB_GETCDE
  return this->getConfereeAttributes(conference->hmpCid(), (MMS_CDT*)cdtEntry, TRUE);
  #else  // MMS_USE_DCB_GETCDE
  return cdtEntry->confereeAttrs;
  #endif // MMS_USE_DCB_GETCDE
}


                                            
int MmsDeviceConference::getConfereeAttributes(int hmpCid, MmsMediaDevice* deviceIP, int alert)
{
  // Retrieves conferee attribute bits given internal conference ID and conferee's IP

  MmsHmpConference* conference = conferences->getByInternalID(hmpCid);
  if (conference == NULL) return ATTRIBUTE_ERROR;  // No such conference

  MMS_CDT* cdtEntry = conference->getCdt(deviceIP->timeslotNumber());
  if (cdtEntry == NULL)   return ATTRIBUTE_ERROR;  // No such conferee

  #ifdef MMS_USE_DCB_GETCDE
  return this->getConfereeAttributes(hmpCid, (MMS_CDT*)cdtEntry, TRUE);
  #else  // MMS_USE_DCB_GETCDE
  return cdtEntry->confereeAttrs;
  #endif // MMS_USE_DCB_GETCDE
}  


                                            
int MmsDeviceConference::getConfereeAttributes(int hmpCid, MMS_CDT* pCdtEntry, int alert)
{
  // Retrieves conferee attribute bits given internal conference ID and conferee's CDT entry.
  // If MMS_USE_DCB_GETCDE is defined we get these attributes from HMP. Otherwise we return
  // our internal copy of the attributes.
  
  int attrs  = 0;

  #ifdef MMS_USE_DCB_GETCDE  // Note that this is not #defined

  const int result = dcb_getcde(m_handle, hmpCid, pCdtEntry);

  if  (result == -1)
  {   
       attrs |= ATTRIBUTE_ERROR;
       if (alert)
           MMSLOG((LM_ERROR,"DEVC %s dcb_getcde %s\n",em,ATDV_ERRMSGP(m_handle)));
  }
  else attrs = pCdtEntry->chan_attr;

  #else    // #ifdef MMS_USE_DCB_GETCDE

  MMS_CDT* cdtEntry = NULL;

  MmsHmpConference* conference = conferences->getByInternalID(hmpCid);

  if  (conference)
       cdtEntry = conference->getCdt(pCdtEntry->chan_num);

  if  (cdtEntry)   
       attrs = pCdtEntry->chan_attr = cdtEntry->confereeAttrs;   
  else attrs |= ATTRIBUTE_ERROR;

  #endif   // #ifdef MMS_USE_DCB_GETCDE

  return attrs;
} 


                                            
int MmsDeviceConference::setConfereeAttributes(int publicCid, MmsMediaDevice* deviceIP, int attrs)
{
  // Sets conferee attribute bits with both HMP and our internal state. We save these flags 
  // internally to avoid the overhead of asking HMP for conferee attributes each time we
  // need to access them.

  MmsHmpConference* conference = conferences->get(publicCid);
  if (NULL == conference) return -1;       // Caller should have lock

  MMS_CDT* cdtEntry = conference->getCdt(deviceIP->timeslotNumber());
  if (NULL == cdtEntry)   return -1;

  if (m_config->diagnostics.flags & MMS_DIAG_LOG_CONFX_ATTRS)
      this->logConferenceAttributes(0, attrs, FALSE);

  return this->setHmpConfereeAttributes(conference->hmpCid(), cdtEntry, attrs);
} 


                                            
int MmsDeviceConference::setHmpConfereeAttributes(int hmpCid, MMS_CDT* cdtentry, int attrs)
{
  // Sets conferee attribute bits with both HMP and our internal state. We save these flags 
  // internally to avoid the overhead of asking HMP for conferee attributes each time we
  // need to access them.

  cdtentry->chan_attr = attrs;

  if  (-1 == dcb_setcde(m_handle, hmpCid, cdtentry))
  {   
       MMSLOG((LM_ERROR,"DEVC %s dcb_setcde %s\n", em, ATDV_ERRMSGP(m_handle)));
       return -1;  
  }

  cdtentry->confereeAttrs = attrs;          // Local copy of attrib bits
  return 0;
} 



int MmsDeviceConference::getLocalConfereeAttributes
( MmsHmpConference* conference, MmsMediaDevice* deviceIP)
{
  // Gets local copy of conferee attribute bits. We keep these flags internally 
  // to avoid the overhead of asking HMP for conferee attributes.

  if (conference == NULL || deviceIP == NULL) return -1;
  MMS_CDT* cdtEntry = conference->getCdt(deviceIP->timeslotNumber());
  return (cdtEntry == NULL)? -1: cdtEntry->confereeAttrs;
} 



int MmsDeviceConference::setLocalConfereeAttributes
( MmsHmpConference* conference, MmsMediaDevice* deviceIP, const unsigned attrs)
{
  // Sets local copy of conferee attribute bits. We keep these flags internally 
  // to avoid the overhead of asking HMP for conferee attributes.

  if (conference == NULL || deviceIP == NULL) return -1;
  MMS_CDT* cdtEntry = conference->getCdt(deviceIP->timeslotNumber());
  if (cdtEntry == NULL) return -1;
  cdtEntry->confereeAttrs = attrs;
  return 0;
} 



void MmsDeviceConference::updateConferenceAttrsOnLeave
( MmsHmpConference* conference, const int externalID, MmsMediaDevice* deviceIP)
{
  // As a party leaves conference, adjust conference's coach/pupil state.
  // Not to be confused with conferee coach/pupil state - in the conference object
  // we maintain the timeslots of both the single conference coach, and the single
  // conference pupil. These will be null if a coach or pupil is not in effect for
  // the conference. Thus, when a conferee leaves a conference, we must check if
  // that conferee was either the coach or pupil, and adjust our state accordingly.

  if (conference->coach && (conference->coach->handle() == deviceIP->handle()))
  {
      MMSLOG((LM_DEBUG,"DEVC conference %d coach leaves\n", externalID));
      conference->coach = NULL; 
  }
  else
  if (conference->pupil && (conference->pupil->handle() == deviceIP->handle()))
  {
      MMSLOG((LM_DEBUG,"DEVC conference %d pupil leaves\n", externalID));
      conference->pupil = NULL;                         
  }               
}


                                             
int MmsDeviceConference::isReceiveOnly(int publicCid, MmsMediaDevice* deviceIP)   
{
  // Indicates if specified conferee is a receive-only participant, but not if its
  // IP resource is merely listening on the monitor timeslot. Use this->isMonitor()  
  // to determine the latter condition. 

  MmsHmpConference*  conference = conferences->get(publicCid);
  if (conference == NULL) return 0;

  return (getConfereeAttributes(conference->hmpCid(), deviceIP, TRUE) & MSPA_RO) != 0;
}                     


                                             
int MmsDeviceConference::isReceivingTariffTone(int publicCid, MmsMediaDevice* deviceIP)   
{
  // Indicates if specified conferee is configured to hear a periodic "tariff tone"

  MmsHmpConference*  conference = conferences->get(publicCid);
  if (conference == NULL) return 0;

  return (getConfereeAttributes(conference->hmpCid(), deviceIP, 1) & MSPA_TARIFF) != 0;
}  



int MmsDeviceConference::setCoach
( int publicCid, MmsMediaDevice* deviceIP, const int onOrOff)   
{
  // Set specified conferee to be the one conference coach
  // The only attribute a coach can have is coach, so we turn off any other
  // attributes this conferee may currently have

  MmsHmpConference* conference = conferences->get(publicCid);
  if  (NULL == conference) return -1; 
  ACE_Guard<ACE_Thread_Mutex> x(conference->xlock);

  if ((conference->coach &&  onOrOff) ||    // We already have a coach ...
     (!conference->coach && !onOrOff)) return -1;  // ... or we don't

  int  attrs = getConfereeAttributes(conference->hmpCid(), deviceIP, 1);
  if  (attrs & ATTRIBUTE_ERROR) return -1;  // Not a conferee probably                                            

  const int result = setConfereeAttributes
           (publicCid, deviceIP, onOrOff? MSPA_COACH: MSPA_NULL);      
  if (result < 0) return result;
 
  MMSLOG((LM_DEBUG,"DEVC conference %d IP%d %s coach\n",   
          publicCid, deviceIP->ordinal(), onOrOff? xis: xisnot));
                                            // If new coach was previously
  if (onOrOff && (attrs & MSPA_PUPIL))      // pupil, indicate no pupil
      if (conference->pupil == deviceIP) 
      {   conference->pupil = NULL;
          setLocalConfereeAttributes(conference, deviceIP, attrs &= ~MSPA_PUPIL);
      }

  conference->coach = onOrOff? deviceIP: NULL;
  return 0;
}   



int MmsDeviceConference::setPupil
( int publicCid, MmsMediaDevice* deviceIP, const int onOrOff)   
{
  // Set specified conferee to be the one conference pupil

  MmsHmpConference* conference = conferences->get(publicCid);
  if  (NULL == conference) return -1;
  ACE_Guard<ACE_Thread_Mutex> x(conference->xlock);

  if ((conference->pupil &&  onOrOff) ||    // We already have a pupil ...
     (!conference->pupil && !onOrOff)) return -1;  // ... or we don't 

  int  attrs = getConfereeAttributes(conference->hmpCid(), deviceIP, 1);
  if  (attrs & ATTRIBUTE_ERROR) return -1;  // Not a conferee probably                                            
                                            // Only other valid attr is tone 
  int  newAttrs = onOrOff?  MSPA_PUPIL: MSPA_NULL;                
  if  (attrs & MSPA_TARIFF) newAttrs |= MSPA_TARIFF;

  const int result = setConfereeAttributes(publicCid, deviceIP, newAttrs);
  if (result < 0) return result;
    
  MMSLOG((LM_DEBUG,"DEVC conference %d IP%d %s pupil\n",   
          publicCid, deviceIP->ordinal(), onOrOff? xis: xisnot));
                                            // If new pupil was previously
  if (onOrOff && (attrs & MSPA_COACH))      // coach, indicate no coach
      if (conference->coach == deviceIP)  
      {   conference->coach = NULL;
          setLocalConfereeAttributes(conference, deviceIP, attrs &= ~MSPA_COACH);
      }

  conference->pupil = onOrOff? deviceIP: NULL;
  return 0;
}  



int MmsDeviceConference::setTariffTone
( int publicCid, MmsMediaDevice* deviceIP, const int onOrOff)   
{
  // Configure specified conferee to receive the periodic tariff tone

  MmsHmpConference* conference = conferences->get(publicCid);
  if  (NULL == conference) return -1;

  int  attrs = getConfereeAttributes(conference->hmpCid(), deviceIP, 1);
  if ((attrs & ATTRIBUTE_ERROR) ||          // Not a conferee probably                                             
      (onOrOff && (attrs & MSPA_COACH)))    // Coach can't receive tone 
       return -1;

  if  (onOrOff)
       attrs |=  MSPA_TARIFF;
  else attrs &= ~MSPA_TARIFF;

  return setConfereeAttributes(publicCid, deviceIP, attrs);
}  



int MmsDeviceConference::setReceiveOnly
( int publicCid, MmsMediaDevice* deviceIP, const int onOrOff)   
{
  // Set specified conferee to be a receive-only participant

  MmsHmpConference* conference = conferences->get(publicCid);
  if  (NULL == conference) return -1;

  int  attrs = getConfereeAttributes(conference->hmpCid(), deviceIP, 1);
  if ((attrs & ATTRIBUTE_ERROR) ||          // Not a conferee probably                                             
      (onOrOff && (attrs & MSPA_COACH)) ||  // Coach can't be muted 
      (onOrOff && (attrs & MSPA_PUPIL)))    // Ditto pupil
       return -1;

  if  (onOrOff)
       attrs |=  MSPA_RO;
  else attrs &= ~MSPA_RO;

  return setConfereeAttributes(publicCid, deviceIP, attrs);
}



int MmsDeviceConference::isCoach(int publicCid, MmsMediaDevice* ip)
{
  // Determine if specified conferee is the conference coach

  MmsHmpConference* conference = conferences->get(publicCid);
  return conference? ip == conference->coach: 0; 
}



int MmsDeviceConference::isPupil(int publicCid, MmsMediaDevice* ip)
{  
  // Determine if specified conferee is the conference pupil

  MmsHmpConference* conference = conferences->get(publicCid);
  return conference? ip == conference->pupil: 0; 
}
  


MmsMediaDevice* MmsDeviceConference::coach(int publicCid)
{
  // Return conferee device (e.g. MmsDeviceIP*) of the conference coach

  MmsHmpConference* conference = conferences->get(publicCid);
  return conference? conference->coach: NULL; 
}


   
MmsMediaDevice* MmsDeviceConference::pupil(int publicCid)
{  
  // Return conferee device (e.g. MmsDeviceIP*) of the conference pupil

  MmsHmpConference* conference = conferences->get(publicCid);
  return conference?  conference->pupil: NULL; 
}
  

                                            
int MmsDeviceConference::resourcesRemaining()   
{  
  // Return HMP's current conference resource count

  static int priorrescount;
  int  resourcesRemaining = priorrescount;   

  if  (m_handle <= 0);
  else
  if  (-1 == dcb_dsprescount(m_handle, &resourcesRemaining))
       MMSLOG((LM_ERROR,"DEVC dcb_dsprescount failed\n"));   
  else priorrescount = resourcesRemaining;   

  return resourcesRemaining;
}



int MmsDeviceConference::enableActiveTalkerMonitoring(const int onOrOff)   
{
  // We currently do not use active talker functionality - it is resource intensive

  int  result = this->activeTalkersEnable(onOrOff);
  if  (result == -1)
       MMSLOG((LM_ERROR,"DEVC could not %s active talkers\n",
               onOrOff? "enable":"disable"));          
  else MMSLOG((LM_NOTICE, "DEVC active talkers monitoring is %s\n",
               onOrOff? "on":"off"));
  return result;
}


                                             
int MmsDeviceConference::activeTalkersEnable(const int onOrOff)   
{  
  // We need to open and save the board level handle (e.g. dcbB1), 
  // and use that handle to enable active talkers etc. Until then,
  // this is not functional.
  const int onOffParam = onOrOff? ACTID_ON: ACTID_OFF;
  const int result = dcb_setbrdparm(m_handle, MSG_ACTID, (void*)&onOffParam);
  return result;
}


                                             
int MmsDeviceConference::isActiveTalkerMonitoringEnabled()   
{  
  // Determine if active talker feature is enabled

  int  result = 0;                          // If error, returns false                                          
  dcb_getbrdparm(m_handle, MSG_ACTID, &result);
  return result;
}



int MmsDeviceConference::getActiveTalkers
( int publicCid, mmsTimeslotHandle* timeslotlist, int listsize)
{
  // Return a list of active talkers. Caller must allocate and supply a list of 
  // adequate size, and must indicate the maximum number of mmsTimeslotHandle*
  // entries that list can hold. Caller must match up the returned list of
  // timeslot handles with his own list of IP devices, using ip->timeslotNumber().
  // Returns active talkers count, or -1.

  // This is not that useful as it stands. If we need this feature, we
  // should translate from timeslot handle to connection ID internally
  // and return the list of connection IDs to client.
  
  MmsHmpConference* conference = conferences->get(publicCid);
  if  (NULL == conference) return -1;

  MMS_CDT* cdt = new MMS_CDT[conference->size()];
  memset(cdt,0,sizeof(cdt));       
  int  activeTalkersCount;

  int  result = dcb_gettalkers(m_handle, conference->hmpCid(), &activeTalkersCount, cdt);
  if  (listsize < activeTalkersCount)
  {    delete[] cdt;
       return -1;
  }

  mmsTimeslotHandle* callerslist = timeslotlist;
  MMS_CDT* cdti = cdt;
  for(int i=0; i < activeTalkersCount; i++, cdti++, callerslist++)
      *callerslist = cdti->chan_num; 
 
  delete[] cdt;
  return activeTalkersCount;
} 



int MmsDeviceConference::isVolumeControlEnabled()   
{  
  // If need to query volume control digits in effect, use similar code to this
  MS_VOL volControlInfo; // {UCHAR vol_control;vol_up;vol_reset;vol_down}
  int returnvalue = (-1 == dcb_getbrdparm(m_handle, MSG_VOLDIG, &volControlInfo))?
      -1: volControlInfo.vol_control != 0x00;
  return returnvalue;
}




int MmsDeviceConference::enableVolumeControl
( const int onOrOff, int digitUp, int digitReset, int digitDown)   
{ 
  // Enable volume control for the application. A conferee controls individual 
  // volume by pressing one of three digits. The digits to be used are 
  // specified in this call (if we're turning volume control on). We do not
  // receive the digit presses -- they are passed on through to the firmware.

  MS_VOL volControlInfo;
  volControlInfo.vol_control = onOrOff? 0x01: 0x00;
  volControlInfo.vol_up      = digitUp;
  volControlInfo.vol_reset   = digitReset;
  volControlInfo.vol_down    = digitDown;

  return dcb_setbrdparm(m_handle, MSG_VOLDIG, &volControlInfo);
}



void MmsDeviceConference::logCoachPupil
( MmsHmpConference* conference, MmsMediaDevice* deviceIP, const unsigned int confereeAttrs)
{
  // Write coach/pupil state changes to log

  if (confereeAttrs & MSPA_COACH) 
  {   
      conference->coach = deviceIP;  
          
      MMSLOG((LM_DEBUG,"DEVC conference %d IP%d is now coach\n",  
          conference->m_publicConferenceID, deviceIP->ordinal()));
  }
  else
  if (confereeAttrs & MSPA_PUPIL) 
  {   
      conference->pupil = deviceIP;      

      MMSLOG((LM_DEBUG,"DEVC conference %d IP%d is now pupil\n",  
          conference->m_publicConferenceID, deviceIP->ordinal()));
  }
}



void MmsDeviceConference::logConfereeTimeslots(MmsMediaDevice* deviceIP, MMS_CDT* cdtEntry)
{
  // Log xmit and receive timeslots for this conferee to log

  const int hip = deviceIP->handle(), hcf = 1;

  MMSLOG((LM_DEBUG,"DEVC timeslots IP%d/CF%d %d/%d\n",
          hip, hcf, cdtEntry->chan_num, cdtEntry->chan_lts));
}



void MmsDeviceConference::logConferenceAttributes
( const BUSCONNECTPARAMS* params, const int isNew, const int isSkipConferee)
{
  this->logConferenceAttributes
       (params->conferenceAttrs, params->confereeAttrs, isNew, isSkipConferee);
}



void MmsDeviceConference::logConferenceAttributes
( const int confxattrs, const int confereeattrs, const int isNew, const int isSkipConferee)
{
  if (isSkipConferee)
  {
       if  (isNew)
            MMSLOG((LM_DEBUG,"DEVC conference attrs %d\n", confxattrs));
       else MMSLOG((LM_DEBUG,"DEVC conferee attrs listen only\n"));
  }
  else
  if  (isNew)
       MMSLOG((LM_DEBUG,"DEVC conference attrs %d conferee %d\n",
               confxattrs, confereeattrs));
  else MMSLOG((LM_DEBUG,"DEVC conferee attrs %d\n", confereeattrs));
}



int MmsDeviceConference::init()
{ 
  const int isEnableActiveTalkersFeature    // Check config for active talkers 
   = (m_config->media.conferenceActiveTalkersEnabled != 0);
                                            // Set if indicated; on error reset
  if (isEnableActiveTalkersFeature && this->enableActiveTalkerMonitoring() == -1) 
      m_config->media.conferenceActiveTalkersEnabled = 0;

  return 0;
}



MmsDeviceConference::~MmsDeviceConference() // Dtor
{  
  conferences->instance()->removeAll();
  conferences->destroy();
}


