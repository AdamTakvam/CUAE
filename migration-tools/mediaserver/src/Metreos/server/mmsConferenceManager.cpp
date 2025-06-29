// 
// mmsConferenceManager.cpp
//
// Manages MmsConference media server conference abstraction. This is distinct   
// from HMP conferences which are managed as MmsHmpConference objects elsewhere.
//
#include "StdAfx.h"
#ifdef MMS_WINPLATFORM
#pragma warning(disable:4786)
#include <minmax.h>
#endif
#include "mmsConferenceManager.h"

#ifdef MMS_ENABLE_MEMORY_LEAK_DETECTION
#define new MMS_MEMLEAK_NEW
#endif // MMS_ENABLE_MEMORY_LEAK_DETECTION

HmpResourceManager* MmsConferenceManager::resourceMgr = 0;
const char* sname = "session", *sutil = "utility session";
const char *hmpconf = "conference", *hpinconf = "hairpin conference";

#include "mmsSessionManager.h"    


int MmsConferenceManager::joinConference
( MmsSession::Op* operation, const int conferenceID, const int flags, 
  const unsigned int confereeAttrs, const unsigned int conferenceAttrs)
{
  // Add session to an existing conference as a full particpant, monitor, or
  // utility session, or create a new conference with session as its first party 

  MmsSession* session = operation->Session(); if (!session) return -1;
  const int sessionID = session->sessionID();
  const int isUtilitySession = (flags & UTILITY_SESSION) != 0;

  if (-1 == this->validateConferenceParameters
     (session, conferenceID, flags, confereeAttrs, conferenceAttrs))
      return -1;

  // Attempt to ensure that we do not complete a conference create/join which was
  // in the pipeline before its client declared intention to disconnect. MMS-195.
  if (operation->isClientDisconnecting()) return -1;  

  if (conferenceID == 0)                    // Create new conference if requested                                           
      return this->createConference(operation, confereeAttrs, conferenceAttrs);

  MmsConference* conference = NULL;
                                            // Look up existing conference
  int result = findByConferenceID(conferenceID, &conference, TRUE);
  if (conference == NULL) return -1;

	if (session->isInConference() && conferenceID != session->conferenceId())
	{					
      MMSLOG((LM_ERROR,"CMGR session %d already conferenced\n", sessionID));
		  return -1;  		
	}

  ACE_Guard<ACE_Thread_Mutex> x(conference->xlock);                                      
                                            // Join existing conference ...
  result = flags & MONITOR?                 // ... as either a full participant
       conference->monitor(operation):      // ... or a monitor
       conference->join(operation, confereeAttrs);

  if  (result == -1) return -1;

  const char* s =  isUtilitySession? sutil: sname;
  const char* t = (flags & MONITOR)? "monitors": "joins";
  const char* c = conference->isHairpinned()? hpinconf: hmpconf;
  MMSLOG((LM_INFO,"CMGR %s %d %s %s %d\n", s, sessionID, t, c, conferenceID)); 

  return 0;   
}



int MmsConferenceManager::leaveConference(MmsSession* session, const int conferenceID) 
{
  // Either leave or unmonitor, depending on session 
  // See mmsNotes.txt for conference teardown logic diagram

  MmsConference* conference = NULL;

  // Maintenance note: we should perhaps have the capability to *pass* a lock pointer
  // to findByConferenceID and findBySession. These methods would set the lock if
  // present. In that way we could get the xlock here atomically, whereas currently
  // we would seem to have a potential race condition between the time we find the
  // conference by ID, and the next line where we acquire the xlock.
                                            // Look up conference
  int  result = findByConferenceID(conferenceID, &conference);
  if  (!conference || !conference->isactive) return -1;                           

  conference->xlock.acquire();

  MmsSession* droneSessions[2] = { NULL, NULL };
  int droneCount = 0;
  int confereesRemaining   = conference->size();
  const int confid         = conferenceID & 0xffff;
  const int sessionid      = session->sessionID();
  const int isDroneSession = session->isUtilitySession();
  const int isMonitor      = session->isConferenceMonitor();
  const int isLastParty    = !isMonitor && (confereesRemaining == 1); 
  // If we're removing a monitor, conferee count 1 is not the last
  // party, since monitors do not contribute to conferee count 

  const char* s = isDroneSession? sutil: sname;
  const char* t = isMonitor? "unmonitors": "leaving";
  const char* c = conference->isHairpinned()? hpinconf: hmpconf;

  MMSLOG((LM_INFO,"CMGR %s %d %s %s %d\n", s, sessionid, t, c, confid)); 
  session->markExitLogged();

  if (!isLastParty)  // Postpone leave if last party  
       result = isMonitor?
                conference->unmonitor(session):  
                conference->leave(session);  

  confereesRemaining = conference->size();       

  // Permit utility session disconnect to terminate conference.
  // Previously we returned at this point if above conference->leave failed. 
  // We cannot simply return here if the conference leave() was unsuccessful,
  // the reason being that if the command is a disco on the lone remaining 
  // conference party, and that party is a voice utility session, the disco
  // will have been previously identified as a barge-in, and the voice operation
  // canceled at that time. Since there is now no IP or voice device in the
  // session, the normal device leave was short-circuited prior to the point
  // where it would normally have terminated the conference (since the listen
  // device is used to find a CDT entry), so we force the conference closed. 

  int  isEmptyConference = confereesRemaining == 0; // Should no longer occur

  if  (confereesRemaining == 1 || confereesRemaining == 2)
  {
       droneCount = conference->isOnlyDroneSessions(NULL, droneSessions);

       if (droneCount > 0)
       {
         // Handle the case where one or two drone sessions are active in this
         // conference, and the last "real" conferee has left the conference.
         // To avoid forcing client to manually close the voice utility session
         // (handled above -- see notes), we manual cancel the voice operations
         // (play, record) on each such drone session, which closes the session. 
 
         for(int i=0; i < droneCount; i++) 
         {
             MmsSession* session = droneSessions[i];
             if (session) 
             {   // We tell stopOperation to not remove utility session from
                 // conference, which would call here recursively. That would
                 // work, but we'll remove all the drones at once at teardown
                 // below, instead.
                 session->markExitSupressed();
                 session->stopAllOperations(this->serverMgr); 
             }
         }
           
         isEmptyConference = TRUE;  
         conference->xlock.release(); 
       } 
       else conference->xlock.release();            
  }
  else conference->xlock.release();     
  
  if  (isLastParty || isEmptyConference) 
  {    
       result = this->teardownConference(session, conferenceID);

        for(int i=0; i < droneCount; i++) 
        {
            // Here we close drone sessions which were just removed from conference
            MmsSession* session = droneSessions[i];
            if (!session || session->isSessionClosed()) continue;   // UTLS
            session->onSessionEnd();        // Return to pool unbinds connection ID
            session->sessionManager()->sessionPool()->returnSessionToAvailablePool(session);   
         }
  }

  return result;
}



int MmsConferenceManager::teardownConference(MmsSession* session, const int confID)   
{
  MmsConference* conference = NULL;

  int  result = findByConferenceID(confID, &conference);
  if  (result == -1 || !conference) return -1;

  // Session.reconnectToConference acquires this lock, preventing us  
  // from proceeding until the reconnect can run its course. 
  conference->dlock.acquire();

  conference->isactive = FALSE;
  const int isHairpin  = conference->isHairpinned();

  MmsDeviceIP* deviceIP = session->ipDevice();
  if (deviceIP && deviceIP->isListening())   
      deviceIP->unlisten();                 // Unlisten last party  
       
  result = conference->teardown(FALSE, FALSE); 

  MMSLOG((LM_INFO,"CMGR conference %d closed\n", confID));    
                                            
  result = removeConferenceByID(confID);    // Unmap conference ID 

  // We've now postponed all destruction of the HMP conference  
  // until after all other aspects of the teardown are complete  

  if (!isHairpin && conference->deviceConference)
      result = conference->deviceConference->leaveHost(confID);             

  conference->dlock.release();
  delete conference;                         

  return result;
}



int MmsConferenceManager::createConference(MmsSession::Op* operation,  
  unsigned int confereeAttrs, const unsigned int conferenceAttrs)
{
  // Create a new conference with session as its first and only participant.

  // If we are configured to start conferences out as a pair of hairpinned
  // connections, we will not create an HMP conference object until such time
  // as a third conferee joins and we promote the conference to HMP. Hairpins
  // exist as MmsConference objects only. If caller has specified conference
  // attributes, (sound tone on join, sound no tone if joining mute), we will
  // still create the conference as a hairpin; however no tone will sound until
  // such time as the conference is promoted.

  MmsSession* session = operation->Session(); if (!session) return -1;
  MmsDeviceConference* deviceConf = NULL;
  mmsConfereeHandle confereeHandle = 0;
  mmsDeviceHandle deviceHandle = 0;
  const int sessionID = session->sessionID();
                                            // Get new public ID for the conference
  const int conferenceID = Mms::getNewPublicConferenceID(); 

  confereeAttrs = MmsConference::normalizeConfereeAttributes(confereeAttrs);     
                                            // Does conference begin as hairpin
  const int isHairpin = this->isHairpinning(operation, confereeAttrs, conferenceAttrs);                                                                                    
                                            
  if (!isHairpin)                           // If not hairpinning ...
  {                                         // ... create an HMP conference of one:
      conferenceDevice(&deviceHandle, &deviceConf, sessionID, TRUE);
      if (deviceConf == NULL) return -1;
     
      confereeHandle = deviceConf->create   // Create HMP conference
         (conferenceID, session->ipDevice(), conferenceAttrs, confereeAttrs);

      if (confereeHandle == -1) return -1;  // No conference resources remain                                   
  }

  MMSLOG((LM_INFO,"CMGR session %d create %s %d\n",
          sessionID, isHairpin? hpinconf: hmpconf, conferenceID)); 
                                            
  MmsConference* conf = new MmsConference   // Create new MMS conference object ...
    (deviceConf, conferenceID, conferenceAttrs, isHairpin);
  
  this->slock.acquire();                    // ... and map it to conference ID
  conferences[conferenceID] = conf;         // We'll get the lock since HMP
  this->slock.release();                    // conference IDs are reused
                                
  MmsSession::ConfInfo& confinfo = session->confinfo();
  confinfo.id = conferenceID;               // Mark session a conferenced session
  confinfo.handle = confereeHandle;         // Conferee handle (timeslot)
  confinfo.confResx = deviceHandle;         // Conference device handle
  confinfo.sethairpin(isHairpin);           // Flag as hairpinned or not
  if (confereeAttrs & MSPA_TARIFF)          // If tariff-toned conferee ...
      confinfo.setttone();                  // ... mark as such
  if (confereeAttrs & MSPA_RO)              // If muted conferee ...
      confinfo.setmuted();                  // ... mark as such
                                             
  conf->conferees.push_back(session);       // Add first conferee to conference

  return 0;
}



int MmsConferenceManager::findByConferenceID
(const int conferenceID, MmsConference** outconf, const int isLog)
{   
  // Look up conference object identified by conferenceID. Return -1 if not found.
  // If caller supplies an out parameter, return conference object so located.

  std::map<int, MmsConference*>::iterator i;
  ACE_Guard<ACE_Thread_Mutex> x(this->slock);

  if ((conferences.size() == 0)  
    || conferences.end()  == (i = conferences.find(conferenceID))) 
  {
      if (isLog) MMSLOG((LM_ERROR,"CMGR no entry for conf ID %d\n", conferenceID));
      return -1; 
  }                            
  
  MmsConference* conference = i->second;    
  if  (outconf) *outconf = conference;
  return 0;
}



int MmsConferenceManager::findBySession(MmsSession* session, MmsConference** outconf) 
{
  // Searches all conferences for a conference hosting supplied session

  *outconf = NULL;
  ACE_Guard<ACE_Thread_Mutex> x(this->slock);
  if  (conferences.size() == 0) return -1;

  std::map<int, MmsConference*>::iterator i = conferences.begin();
  for(; i != conferences.end(); i++)
  {
    MmsConference* conference = i->second;
    if  (conference->isConferee(session) || conference->isMonitor(session))
    {    *outconf = conference;
         return 0;
    }
  }

  return -1;
}



int MmsConferenceManager::removeConferenceByID(const int conferenceID)
{   
  // Look up conference object identified by conferenceID. Return -1 if not found.
  // Otherwise remove conference from conferences map

  std::map<int, MmsConference*>::iterator i;
  ACE_Guard<ACE_Thread_Mutex> x(this->slock);

  if ((conferences.size() == 0)  
    || conferences.end()  == (i = conferences.find(conferenceID))) 
       return -1;                             
  
  conferences.erase(i);    
  return 0;
}

  

int MmsConferenceManager::isActiveConference(const int conferenceID)
{ 
  MmsConference* conference = NULL;
  int result = this->findByConferenceID(conferenceID, &conference);
  return conference && conference->isactive;
}


                  
MmsDeviceConference* MmsConferenceManager::device(MmsSession* session)
{
  MmsMediaDevice* dev = resourceMgr->getDevice(session->confinfo().confResx);
  return (MmsDeviceConference*)dev;
}



MmsDeviceConference* MmsConferenceManager::device()
{   
  // Convenience method to get conference's HMP conference device
  MmsMediaDevice* dev = resourceMgr->getConferencingDevice();
  return (MmsDeviceConference*)dev;
}



int MmsConferenceManager::conferenceDevice(mmsDeviceHandle* phandle, 
  MmsDeviceConference** pdevice, const int sessionID, const int isLog)
{
  // Gets handle to, and reference to, the one and only HMP conferencing device.
  // Note that the device will not exist if there are no licensed resources.

  if  (*phandle = resourceMgr->getResource(MmsMediaDevice::CONF))
       *pdevice = (MmsDeviceConference*)resourceMgr->getDevice(*phandle);
  else 
  {    *pdevice = NULL;
        if (isLog) MMSLOG((LM_ERROR,
           "CMGR session %d firmware conferencing unavailable\n", sessionID)); 
         return -1;                         // No HMP conferences licensed
  }

  return 0;
}



int MmsConferenceManager::joinUtilitySession(MmsSession* session)
{
  // Connect a utility session to a conference. A utility session is a 
  // non-IP session used as context for play/record of voice to a conference.
  // It appears as a conferee to HMP, in that it occupies a CDT entry;
  // however HMP listens to the session voice resource instead of the IP.
  // A utility session exists only for the duration of the voice operation;
  // therefore it will be removed from conference upon play or record
  // termination. 

  char* map = NULL; 
  MmsSession::Op* operation  = session->first(); 
  if (operation != NULL) map = operation->flatmap();  
  if (map == NULL) return -1;                           

  const int conferenceID = getFlatmapParam(map);
  if (conferenceID == 0) return -1;

  int result = ensureHmpConference(conferenceID);
  if (result != 0) return result;

  result = this->joinConference(operation, conferenceID, UTILITY_SESSION, 0, 0);
  return result;
}



int MmsConferenceManager::ensureHmpConference(const int conferenceID)
{
  // Check if conference[conferenceID] is a hairpin, and if so, promote it
  MmsConference* conference = NULL;

  int result = findByConferenceID(conferenceID, &conference, TRUE);
  if (!conference || result == -1) return -1;

  if (conference->isHairpinned())
  {
      if (-1 == conference->promote())
          return MMS_ERROR_RESOURCE_UNAVAILABLE;
  }

  return 0;
}



int MmsConferenceManager::validateConferenceParameters
(  MmsSession* session, const int conferenceID, const int flags, 
   const unsigned int confereeAttrs, const unsigned int conferenceAttrs) 
{
  int result = 0;
  const int sessionID = session->sessionID();

  if ((flags & UTILITY_SESSION) != 0)
  {
      if (conferenceID == 0)
      {
          MMSLOG((LM_ERROR,"CMGR utility session %d may not host a conference\n", sessionID));
          result = -1;  
      }
  }
  else    // not a utility session
  {                                         
      MmsDeviceIP* deviceIP = session->ipDevice();
      if (!deviceIP) return -1;

      if (!deviceIP->isStarted())
      {
          MMSLOG((LM_ERROR,"CMGR half-connected session %d may not conference\n", sessionID));
          result = -1;  
      }
  }

  return result;
}



int MmsConferenceManager::isConfereeOptionsOkToHairpin(const unsigned int attrs) 
{
  // Indicate if we will hairpin a conferee given conferee options specified by 
  // client, or if we must promote the conference. Read only (mute) and tariff
  // tone options are OK to hairpin. Other options (coach, pupil) are not.
  unsigned int hairpinnableAttrs = MSPA_RO | MSPA_TARIFF;
  unsigned int attrsMinusHairpinnableAttrs = attrs & ~hairpinnableAttrs;
  return attrsMinusHairpinnableAttrs == 0;
}


                                            
MmsConferenceManager::MmsConferenceManager
( MmsConfig* config, HmpResourceManager* resmgr, MmsTask* serverMgr) 
{
  this->config = config;                    // Ctor
  this->resourceMgr = resmgr;
  this->serverMgr = serverMgr;
  ACE_OS::strcpy(objname,"CMGR");
}



int MmsConferenceManager::closeAll()
{
  ACE_Guard<ACE_Thread_Mutex> x(this->slock);

  const int n = conferences.size();
  if  (n < 1) return 0;

  for(int i=0; i < n; i++)
  {                                          
      MmsConference* conference = conferences[i]; 
  
      if  (conference)
      {     
           conference->teardown(FALSE, TRUE);
           delete conference;
           conference = NULL;
      }
  } 

  conferences.clear();
  return n; 
}


                                            
MmsConferenceManager::~MmsConferenceManager()
{
  // Dtor: tear down and delete any remaining conferences
  this->closeAll();
}



// - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - 
// MmsConference
// - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - 


int MmsConference::join(MmsSession::Op* operation, unsigned int attrs)
{
  // Join a conferee to conference. Assumed that caller has the xlock.

  MmsSession* session = operation->Session(); if (!session) return -1;
  mmsConfereeHandle confereeHandle = 0;

  // MMS-137: do not join conference while teardown in progress, 
  // and do not permit teardown to begin while join in progress.
  ACE_Guard<ACE_Thread_Mutex> x(this->dlock);  

  if (this->ishairpin)                      // If currently hairpinned ...
  {
      // If 2 parties currently in hairpin, or if new party 2 comes with an
      // attribute not available to hairpinned conferees, then promote hairpin 
      // to an HMP conference and continue. Otherwise, hairpin this second party 
      // to the current one-party "hairpin" and return.
      
      if  (conferees.size() == 2 || this->isNonHairpinnableParty(attrs))       
           if  (this->promote(session, operation->flatmap()) == -1) 
                return -1;
           else;       
      else return this->hairpin(session, attrs);       
  }
      
  if (!this->ishairpin)                     // If not or no longer hairpinned ...      
  {                        
      MmsMediaDevice* deviceListen = this->listenDevice(operation);
      if (deviceListen == NULL || deviceConference == NULL) return -1;

      attrs = MmsConference::normalizeConfereeAttributes(attrs);   

      confereeHandle = deviceConference->join(this->conferenceID, deviceListen, attrs);

      #if(0) // Code to check actual conferee attributes ...      
      int hmpAttrs = deviceConference->getConfereeAttributes(conferenceID, deviceListen); 
      #endif 
  }

  if (confereeHandle != -1)
  {    
      MmsSession::ConfInfo& confinfo = session->confinfo();
      confinfo.id = this->conferenceID;
      confinfo.handle   = confereeHandle;   // Conferee timeslot 
      confinfo.confResx = deviceConference->handle();
      confinfo.sethairpin(this->ishairpin); 
      if (attrs & MSPA_TARIFF) confinfo.setttone();                  
      if (attrs & MSPA_RO) confinfo.setmuted();
      conferees.push_back(session);
  }

  return confereeHandle;
}



int MmsConference::leave(MmsSession* session)
{
  // Leave a conferee from this conference. Assumed that caller has the xlock.

  int result = 0;
  MmsMediaDevice* deviceListen = NULL;

  // We should add some more parameters such as flags to leaveConference(),
  // to indicate for example that we do or don't want to access the session
  // parameter map (e.g. after a utility session cancel); however for now
  // we will assume that we do not. 
  MmsSession::Op* operation = session->first();
  char* flatmap = session->isUtilitySession()? NULL:
                  operation? operation->flatmap(): 
                  NULL;

  if (!this->ishairpin)
  {   
      // Note that utility sessions must come through this code *before* their
      // voice operation is closed, since such a session's CDT (HMP conference
      // descriptor table) entry uses the voice device handle as its key, and
      // after the operation is closed, the voice device is gone.
      MmsMediaDevice* deviceListen = operation?
          this->listenDevice(operation, FALSE): NULL;

      if (deviceListen)  
          result = deviceConference->leave(this->conferenceID, deviceListen);    
  }
   
  if (this->ishairpin && conferees.size() == 2)
      this->unhairpin(FALSE);               // Unhairpin the 2 parties

  session->confinfo().clear();

  conferees.remove(session);
    
  if (flatmap && (conferees.size() == 2) && this->isDemoting(session, flatmap))
      this->demote();                       // Demote to hairpin if configured

  return result;
}



int MmsConference::monitor(MmsSession::Op* operation)
{
  // Add a monitor to this conference. Assumed that caller has the xlock.

  MmsSession* session = operation->Session(); if (!session) return -1;
                                            // Promote if hairpinned
  if  (this->ishairpin && this->promote(session, operation->flatmap()) == -1) return -1;
  int  result = 0;

  if  (!this->isMonitored())
  {
       result = deviceConference->addMonitorChannel(this->conferenceID);
       if (result == -1) return -1;
       this->ismonitored = TRUE;
  }

  result = deviceConference->listenOnMonitorChannel
      (this->conferenceID, session->ipDevice());

  if  (result != -1)
  {   
       session->confinfo().id = this->conferenceID;
       session->confinfo().setmonitor();    // Mark session as a conf monitor
       session->confinfo().confResx = deviceConference->handle();
       monitors.push_back(session);
  }

  return result;
}



int MmsConference::unmonitor(MmsSession* session)
{
  // Remove this monitor from this conference. Assumed that caller has the xlock.
  if  (!this->isMonitored()) return -1;

  int  result = deviceConference->unlistenOnMonitorChannel
      (this->conferenceID, session->ipDevice());

  session->confinfo().clear();
  monitors.remove(session);

  if  (monitors.size() == 0)
  {
       int n = deviceConference->removeMonitorChannel(this->conferenceID);
       this->ismonitored = FALSE;
  }

  return result;
}



void MmsConference::muteIfMuted()
{
  // Mute any hairpinned party who is marked as muted

  MmsConference::HairpinInfo hi;
  this->muteIfMuted(hi);
}



void MmsConference::muteIfMuted(MmsConference::HairpinInfo& hi)
{
  // Mute any hairpinned party who is marked as muted

  if (hi.sessionA && hi.sessionA->isMutedConferee())
      this->mutehairpinned(hi.sessionB);
  if (hi.sessionB && hi.sessionB->isMutedConferee())
      this->mutehairpinned(hi.sessionA);
}



int MmsConference::isConferee(MmsSession* session)
{
  if (conferees.size() == 0) return FALSE;
  int result = 0;
  std::list<MmsSession*>::iterator i = conferees.begin();

  for(; i != conferees.end(); i++)
      if (*i == session) { result = TRUE; break; }

  return result;
}



int MmsConference::isMonitor (MmsSession* session)
{
  if (monitors.size() == 0) return FALSE;
  int result = 0;
  std::list<MmsSession*>::iterator i = monitors.begin();

  for(; i != monitors.end(); i++)
      if (*i == session) { result = TRUE; break; }

  return result;
} 



MmsSession* MmsConference::first()
{
  return this->size()? *this->conferees.begin(): NULL;
}
  


unsigned int MmsConference::normalizeConfereeAttributes(const unsigned int attrs)
{
    // Set conferee attributes when passed with the conference join request.
    // We ensure that the combined attributes are a legal combination, however
    // we do not throw an error when a combo is invalid, we simply make it valid
    // by assuming attribute priorities: coach, pupil, tone, read-only
    
    unsigned int newattrs = 0;

    if  (attrs & MSPA_COACH)                // When coach specified ...
         newattrs = MSPA_COACH;             // ... coach can be the only attribute
    else                                    // When pupil specified ...
    if  (attrs & MSPA_PUPIL)                // ... only other valid attr is tone
         newattrs =(attrs & MSPA_TARIFF)? MSPA_PUPIL | MSPA_TARIFF: MSPA_PUPIL; 
    else newattrs = attrs;

    return newattrs;
}



int MmsConference::internalConferenceID()
{
    // Given external conference ID, return the internal (HMP) conference ID
    MmsHmpConference* conference = deviceConference->getConference(conferenceID);
    const int internalConfID = conference? conference->hmpCid(): 0;
    return internalConfID;
}



int MmsConference::confereeAttributes(MmsSession* session)
{
  // Retrieve HMP conferee attributes for conferee represented by IP session

  int attrs = MmsMediaDevice::ATTRIBUTE_ERROR;
  const int internalConfID = this->internalConferenceID();
  if (internalConfID == 0) return attrs;

  MmsDeviceIP* deviceIP = session? session->ipDevice(): NULL;
  if (deviceIP == NULL) return attrs;

  attrs = this->deviceConference->getConfereeAttributes(internalConfID, deviceIP);
  return attrs;
}



int MmsConference::isOnlyDroneSessions(MmsSession* thisSession, MmsSession** droneSessions)
{
  // Determines if the only conferees remaining in this conference, other than the
  // current session, if specified, are utility (drone) sessions. If this is the case, 
  // the drone count is returned, if not, zero is returned. If sessions* is non-null, 
  // the drone sessions so identified are returned in that table, which must be at least 
  // 2 deep (since there can be at most 2 drones).

  const int confereeCount = this->conferees.size();
  if (confereeCount == 0) return 0;
  int droneCount = 0;

  std::list<MmsSession*>::iterator i = this->conferees.begin();

  for(; i != this->conferees.end(); i++)
  {
      MmsSession* session = *i; 
      if (session == NULL || session == thisSession) continue;

      if (!session->isUtilitySession())  
      {   droneCount = 0;
          break;
      }
      if (droneSessions && (droneCount < 2)) 
          droneSessions[droneCount] = session;
      droneCount++;
  }

  return droneCount;
}



MmsMediaDevice* MmsConference::listenDevice(MmsSession::Op* operation, const int acquire)
{
  // Conference usually listens to IP, but in the case of a play to
  // conference via utility session, conference listens to voice 

  MmsSession* session = operation->Session(); if (!session) return NULL;

  MmsMediaDevice* deviceListen = session->isUtilitySession()? 
     (MmsMediaDevice*)operation->voiceDevice(acquire, acquire): 
     (MmsMediaDevice*)session->ipDevice();

  return deviceListen;
}




int MmsConference::teardown(int forceDevice, int release)
{ 
  if (this->ishairpin) return this->teardownHairpin();
  int result = 0; 

  // 12/03 override to ensure the HMP conference is terminated even if the
  // conference is empty. The forceDevice parameter was added for the case
  // where a utility session was the final conferee and was removed, but since
  // voice had already been canceled and there was no IP device, the device was
  // not able to get a CDT entry on the final conferee and thus the identifi-
  // cation of final conferee and subsequent termination of the conference was
  // short-circuited.

  // 6/04 we now postpone removal of final conferee and deletion of HMP
  // conference until here at the end of the teardown process, so the 
  // synchronization problem mentioned above should no longer be an issue.
                                             
                                            // 12/03 see comments above                                            
  if  (release && (this->size() > 0 || forceDevice))      
       result = deviceConference->leaveHost(this->conferenceID);  

  std::list<MmsSession*>::iterator i;  
  ACE_Guard<ACE_Thread_Mutex> x(this->xlock);

  if  (conferees.size())                    // Clear conference info from
  {                                         // all participating sessions
       for(i = conferees.begin(); i != conferees.end(); i++)
       {
           MmsSession* session = *i;
           if  (session)
           {    
                session->confinfo().clear();

                // When we postpone removal of a voice utility session when
                // that session is the final session in the conference, the
                // exit may not have been logged elsewhere, so log it now.  
                if (session->isUtilitySession() && !session->isExitLogged()) 
                    MMSLOG((LM_INFO,"CMGR utility session %d leaving conference %d\n", 
                      session->sessionID(), this->conferenceID & 0xffff)); 
           }
       }
  }
      
  if  (monitors.size())
  {
       for(i = monitors.begin(); i != monitors.end(); i++)
       {
           MmsSession* session = *i;
           if  (session)
                session->confinfo().clear();
       }
  }

  return result;
}


// - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
// conference hairpinning logic
// - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - 


int MmsConference::hairpin(MmsSession* sessionB, unsigned int attrs)
{
  // Join a second conferee to existing hairpin conference  
 
  MmsConference::HairpinInfo hi;
  this->getHairpinInfo(hi);
  MmsDeviceIP* ipB = NULL;

  if (hi.sessionA && sessionB && conferees.size() < 2)     
      ipB = sessionB->ipDevice();
   
  if (!hi.ipA || !ipB)
  {
      MMSLOG((LM_ERROR,"CMGR session %d invalid hairpin on conf %d\n", 
              sessionB->sessionID(), this->conferenceID));
      return -1;
  }
         
  if (hi.ipA->busConnect(ipB, MmsMediaDevice::FULLDUPLEX) == -1)
  {
      MMSLOG((LM_ERROR,"CMGR could not hairpin sessions %d + %d\n", 
              sessionB->sessionID(), hi.sessionA->sessionID()));
      return -1;
  }
   
  MmsSession::ConfInfo& confinfo = sessionB->confinfo();
  confinfo.id = this->conferenceID;
  confinfo.handle = 0;       // Hairpins have handle zero
  confinfo.confResx = 0;     // deviceConference->handle();
  confinfo.sethairpin();
  if (attrs & MSPA_RO)
  {   confinfo.setmuted();
      this->mutehairpinned(sessionB);
  }

  conferees.push_back(sessionB);
  return 0;
}



int MmsConference::hairpin(const int getlock)
{
  // Hairpin two existing conferees

  int result = -1;
  if (getlock) this->xlock.acquire();

  if (conferees.size() == 2)
  {
      MmsConference::HairpinInfo hi;
      this->getHairpinInfo(hi);

      if (hi.ipA && hi.ipB)                  
      {                                      
          result = hi.ipA->busConnect(hi.ipB, MmsMediaDevice::FULLDUPLEX); 
                                             
          if (result == 0) this->muteIfMuted(hi); 
      }         
  }

  if (getlock) this->xlock.release();             
  return result;
}



int MmsConference::unhairpin(const int getlock)
{
  // Unhook a hairpinned connection, if the connection exists

  int result = 0;
  if (getlock) this->xlock.acquire();

  if (conferees.size() > 0)  
  {
      MmsConference::HairpinInfo hi;
      this->getHairpinInfo(hi);

      if (hi.ipA && hi.ipB)  
          result = hi.ipA->busDisconnect(hi.ipB, MmsMediaDevice::FULLDUPLEX); 
  }

  if (getlock) this->xlock.release();             
  return result;
} 



int MmsConference::getHairpinInfo(HairpinInfo& info)
{
  // Get the session and IP device objects for the one or two conference parties

  info.reset();
  if (conferees.size() == 0) return -1;
  std::list<MmsSession*>::iterator i = conferees.begin(); 
  if (i != conferees.end()) 
  {   info.sessionA = *i;
      if (info.sessionA) info.ipA = info.sessionA->ipDevice();
      if (++i != conferees.end()) 
      {   info.sessionB = *i;
          if (info.sessionB) info.ipB = info.sessionB->ipDevice();
      }
  }
  return 0;
}



int MmsConferenceManager::isHairpinning
( MmsSession::Op* operation, const unsigned int cfreeAttrs, const unsigned int confxAttrs)
{
  // Determine if a new conference is to start off as a hairpin 

  // Config file will specify hairpinning options as follows
  // Server.hairpinOpts = 0: hairpinning off unless overridden by client param
  // Server.hairpinOpts = 1: hairpinning on  unless overridden by client param
  // Server.hairpinOpts = 2: hairpinning off regardless of client param 

  // Client may specify hairpinning overide via a connect parameter as follows,
  // with absence of the parameter assumed to indicate that the config setting
  // should be used to determine whether to hairpin.
  // hairpin = 0: Do not hairpin
  // hairpin = 1: Hairpin

  MmsSession* session = operation->Session(); if (!session) return 0;
  MmsConfig* config   = session->Config();
  const int sessionID = session->sessionID();
  const int configHairpinParam = config->serverParams.hairpinOpts;
 
  MmsFlatMapReader map(operation->flatmap()); 
  
  char* phairpinOpts = 0;
  const int length = map.find(MMSP_HAIRPIN, &phairpinOpts);
  const int hairpinParam = phairpinOpts? *(int*)(phairpinOpts): 0;
  const int isClientHairpinOptionSpecified = (phairpinOpts != NULL);
  const int isClientHairpinOverrideOff = isClientHairpinOptionSpecified? 
            hairpinParam == 0: 0;
  const int isClientHairpinOverrideOn  = isClientHairpinOptionSpecified? 
            hairpinParam  > 0: 0;
                                            // config off and client did not override?
  if (configHairpinParam == MMS_HAIRPIN_OFF_UNLESS_OVERRIDE && !isClientHairpinOverrideOn)
      return 0;
                                            // config on but client override?
  if (configHairpinParam == MMS_HAIRPIN_ON_UNLESS_OVERRIDE && isClientHairpinOverrideOff)
      return 0;

  if (!isConfereeOptionsOkToHairpin(cfreeAttrs)) 
  {
      // We will not create a hairpin conference when certain HMP conferee options 
      // (coach, pupil, etc) are specified; however we permit the receive only option
      MMSLOG((LM_INFO,"CMGR will not hairpin session %d due to conferee options\n", 
              sessionID));
      return 0;
  }

  if (configHairpinParam >= MMS_HAIRPIN_NEVER)  
  {
      // Client specifically requested hairpin but config set to never hairpin?
      if (isClientHairpinOverrideOn)
          MMSLOG((LM_INFO,"CMGR will not hairpin session %d due to config\n", sessionID));

      return 0;
  }

  return 1;
}



int MmsConference::promote()
{
  // Promote conference from hairpinned to HMP conference

  if (!this->isHairpinned()) return -1;     

  return this->promote(this->first(), NULL);
}



int MmsConference::promote(MmsSession* newsession, char* flatmap)
{
  // Promote conference from hairpinned to HMP conference
  // Assumed that caller has the xlock prior to entry

  if (!newsession || !this->isPromoting(newsession, flatmap)) return -1;

  mmsConfereeHandle hConferee  = 0;
  mmsDeviceHandle deviceHandle = 0;
  const int sessionID = newsession->sessionID();

  MmsConferenceManager::conferenceDevice(&deviceHandle, &deviceConference, sessionID, 1);
  if (deviceConference == NULL) return -1;

  MmsConference::HairpinInfo hi;
  this->getHairpinInfo(hi);
  
  this->unhairpin();
  this->ishairpin = FALSE;
    
  if (hi.ipA)
  {   
      // Create an HMP conference with first party as initial member
      MmsSession::ConfInfo& confinfo = hi.sessionA->confinfo();   
      confinfo.sethairpin(FALSE);
      unsigned int confereeAttrs = 0;
      if (confinfo.flags & MmsSession::ConfInfo::ISMUTED) confereeAttrs |= MSPA_RO;

      if (this->conferenceID) 
          hConferee = this->deviceConference->create
              (conferenceID, hi.ipA, this->conferenceAttributes, confereeAttrs);

      confinfo.handle = hConferee; 
  }

  if (hConferee == 0)  
  {   // If we could not promote the conference, try to re-hairpin
      MMSLOG((LM_ERROR,"CONF conference %d was not promoted\n", conferenceID));

      if  (this->hairpin() == 0)
           MMSLOG((LM_INFO, "CONF conference %d rehairpinned\n", conferenceID));
      else MMSLOG((LM_ERROR,"CONF conference %d was not rehairpinned\n", conferenceID));

      return -1;
  }

  if (hi.ipB)
  {   // Join second party to conference
      MmsSession::ConfInfo& confinfo = hi.sessionB->confinfo(); 
      confinfo.sethairpin(FALSE);
      unsigned int conferenceAttrs = 0, confereeAttrs = 0;
      if (confinfo.flags & MmsSession::ConfInfo::ISMUTED) confereeAttrs |= MSPA_RO;
      if (confinfo.flags & MmsSession::ConfInfo::ISTTONE) confereeAttrs |= MSPA_TARIFF;

      hConferee = deviceConference->join(conferenceID, hi.ipB, confereeAttrs);
  }

  if (hConferee == 0)  
  {   // If second party failed to join, report and return error
      MMSLOG((LM_ERROR,"CONF conference %d promoted but without session %d\n", 
              conferenceID, hi.sessionB->sessionID()));
      return -1;
  }

  MMSLOG((LM_INFO,"CONF conference %d promoted\n", conferenceID));
  return 0;
}



int MmsConference::isPromoting(MmsSession* session, char* flatmap)
{
  // Determine if we are configured to promote conference 
  // Assumed that caller has the xlock prior to entry

  // Config file will specify promotion options as follows
  // Server.hairpinPromotionOpts = 0: promotion off unless overridden by client param
  // Server.hairpinPromotionOpts = 1: promotion on  unless overridden by client param
  // Server.hairpinPromotionOpts = 2: demotion  on  unless overridden by client param
  // Server.hairpinPromotionOpts = 4: promotion off regardless of client param 
  // Server.hairpinPromotionOpts = 8: demotion  off regardless of client param 

  // Client may specify promotions options via a connect parameter as follows:
  // hairpinPromote = 0: Not specified
  // hairpinPromote = 1: Promote if config permits
  // hairpinPromote = 2: Promote and demote if config permits
  // hairpinPromote = 4: Do not promote regardless of config

  const int sessionID = session->sessionID();
  MmsConfig* config   = session->Config();
  const int configPromoteOpts  = config->serverParams.hairpinPromotionOpts;
  int   clientPromoteOpts = 0;

  if (flatmap)
  {   char* ppromoteOpts = 0;
      MmsFlatMapReader map(flatmap);
      map.find(MMSP_HAIRPIN_PROMOTE, &ppromoteOpts);
      if (ppromoteOpts) clientPromoteOpts = *(int*)(ppromoteOpts);
  }

  const int isClientPromoteOverride 
          = clientPromoteOpts == CLIENT_PROMOTE_OPT_PROMOTE || 
            clientPromoteOpts == CLIENT_PROMOTE_OPT_PROMOTE_DEMOTE;

  const int isClientNeverPromote  
          = clientPromoteOpts  > CLIENT_PROMOTE_OPT_PROMOTE_DEMOTE;
 
  if  ((configPromoteOpts & MMS_HPIN_PROMOTE_NEVER) 
   || ((configPromoteOpts & MMS_HPIN_PROMOTE_OFF_UNLESS_OVERRIDE) 
        && !isClientPromoteOverride)) 
  {
       MMSLOG((LM_INFO,"CONF session %d conf %d will not promote per config\n", 
               sessionID, this->conferenceID)); 
       return 0;
  }

  const int isConfigPromoteOn 
    = (configPromoteOpts & MMS_HPIN_PROMOTE_ON_UNLESS_OVERRIDE) != 0;

  if (isClientNeverPromote)
  {
      if (isConfigPromoteOn)
          MMSLOG((LM_INFO,"CONF session %d conf %d will not promote per client\n", 
                  sessionID, this->conferenceID)); 
      return 0;
  }

  return isConfigPromoteOn;
}



int MmsConference::demote()
{
  // Demote HMP conference to hairpin

  if (this->ishairpin) return 0;
  this->ishairpin = TRUE;
  int result = 0;
  MmsConference::HairpinInfo hi;
  this->getHairpinInfo(hi);

  if (hi.sessionB) 
  {   // Session B leave HMP conference, but not MMS conference
      result = deviceConference->leave(conferenceID, hi.ipB);
      MmsSession::ConfInfo& confinfo = hi.sessionB->confinfo();
      confinfo.handle = 0;  
      confinfo.sethairpin();
  }

  if (hi.sessionA)  
  {   // Session A leave HMP conference, but not MMS conference
      result = deviceConference->leave(conferenceID, hi.ipA);
      MmsSession::ConfInfo& confinfo = hi.sessionA->confinfo();
      confinfo.handle = 0;  
      confinfo.sethairpin();
  }

  if  (conferees.size() == 2)
  {
       result = this->hairpin();
  
       if (hi.sessionB->isMutedConferee())
           this->mutehairpinned(hi.sessionB);

       if (hi.sessionA->isMutedConferee())
           this->mutehairpinned(hi.sessionA);
  }
       
  MMSLOG((LM_INFO,"CONF conference %d demoted to hairpin\n", conferenceID));
  return result;
}



int MmsConference::isDemoting(MmsSession* session, char* flatmap)
{
  // Determine if we are configured to demote conference 
  // Assumed that caller has the xlock prior to entry

  // Config file will specify promotion options as follows
  // Server.hairpinPromotionOpts = 0: promotion off unless overridden by client param
  // Server.hairpinPromotionOpts = 1: promotion on  unless overridden by client param
  // Server.hairpinPromotionOpts = 2: demotion  on  unless overridden by client param
  // Server.hairpinPromotionOpts = 4: promotion off regardless of client param 
  // Server.hairpinPromotionOpts = 8: demotion  off regardless of client param 

  // Client may specify promotions options via a connect parameter as follows:
  // hairpinPromote = 0: Not specified
  // hairpinPromote = 1: Promote if config permits
  // hairpinPromote = 2: Promote and demote if config permits
  // hairpinPromote = 4: Do not promote regardless of config

  if (!flatmap || !session || !session->isBusy()) return FALSE;

  // No reason to demote when session or conference is being abandoned
  if (session->Flags() & MMS_SESSION_FLAG_ABANDON_IN_PROGRESS) return FALSE;

  MmsConfig* config = session->Config();
  const int configPromoteOpts = config? config->serverParams.hairpinPromotionOpts: 0;
  if (configPromoteOpts & MMS_HPIN_DEMOTE_NEVER) return FALSE;

  int clientPromoteOpts = 0;

  if (Mms::isFlatmapReferenced(flatmap,21))
  {
      MmsFlatMapReader map(flatmap); 
      char* ppromoteOpts = 0;

      map.find(MMSP_HAIRPIN_PROMOTE, &ppromoteOpts);
      if (ppromoteOpts) 
          clientPromoteOpts = *(int*)(ppromoteOpts);
  }

  const int isClientNeverDemote  
    = clientPromoteOpts  > CLIENT_PROMOTE_OPT_PROMOTE_DEMOTE;
  if (isClientNeverDemote) return FALSE;

  const int isClientDemoteRequested 
    = clientPromoteOpts == CLIENT_PROMOTE_OPT_PROMOTE_DEMOTE;
  if (isClientDemoteRequested) return TRUE;

  const int isConfigDemoteOK 
    = (configPromoteOpts & MMS_HPIN_DEMOTE_ON_UNLESS_OVERRIDE) != 0;

  return isConfigDemoteOK;
}



int MmsConference::mutehairpinned(MmsSession* session, const int isMuting)
{
  // Mute or unmute this end of the hairpin by (un)listening the other end from/to this end

  MmsConference::HairpinInfo hi;
  this->getHairpinInfo(hi);
  session->confinfo().setmuted(isMuting);
  int result = 0;

  MmsDeviceIP* otherIP = session == hi.sessionA? hi.ipA: hi.ipB;
  MmsDeviceIP* thisIP  = session->ipDevice();
  if (!otherIP || !otherIP->isListening()) return 0;

  if  (isMuting)       
       result = thisIP->busDisconnect(otherIP, MmsMediaDevice::HALFDUPLEX);  
  else result = thisIP->busConnect   (otherIP, MmsMediaDevice::HALFDUPLEX);
  
  return result;
}



int MmsConference::teardownHairpin()
{ 
  // Tear down a hairpinned "conference". Conference may be marked as a hairpin
  // but could in reality have only one member at this point

  const int result = this->unhairpin();
  ACE_Guard<ACE_Thread_Mutex> x(this->xlock);
  this->conferees.clear(); 
  return 0;
}

