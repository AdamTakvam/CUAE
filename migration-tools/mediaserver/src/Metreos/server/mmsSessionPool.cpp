//
// MmsSessionPool.cpp
// Session (connection) table and associated tables
//
#include "StdAfx.h"
#pragma warning(disable:4786)
#include "mmsSession.h"
#include "mmsReporter.h"
#include "mmsBuild.h"

#ifdef MMS_ENABLE_MEMORY_LEAK_DETECTION
#define new MMS_MEMLEAK_NEW
#endif // MMS_ENABLE_MEMORY_LEAK_DETECTION

const char* cs = "session", *cus = "utility session";
#define MMS_MIN_CIDMAPSIZE 4



MmsSessionPool::MmsSessionPool(MmsConfig* config): connectionIDs(0) 
{
  ACE_OS::strcpy(this->objname,"SESP");
                                            // Assign size of session pool
  this->poolSizeMax       = MmsAs::maxG711; // config->calculated.maxConnections;
  this->utilityTableSize  = config->calculated.utilityPoolSize;
  this->sessionTableSize  = this->poolSizeMax + this->utilityTableSize;
  this->utilityPoolActive = 0;
                                            // Allocate session table
  this->sessions = new MmsSession[this->sessionTableSize];

  MmsSession* session = this->sessions;     // Initialize session objects
  for(int i=1; i <= this->sessionTableSize; i++, session++)
      session->init(i, config);             // Assign the 1-based session ID
                                            // Allocate connection ID map
  connectionIDs = new CONNECTIONID_HASHMAP(max(sessionTableSize, MMS_MIN_CIDMAPSIZE));
  mruSession = NULL;
  conxID = 0;
}



MmsSessionPool::~MmsSessionPool()           // Dtor
{
  if  (connectionIDs) delete connectionIDs;

  if  (sessions) delete[] sessions;
}



int MmsSessionPool::bindSessionToConnection(int sessionID, const void* clientID)
{
  MmsSession* session = this->findBySessionID(sessionID);
  return session? this->bindSessionToConnection(session, clientID): -1;
}


                                             
int MmsSessionPool::bindSessionToConnection(MmsSession* session, const void* clientID)
{         
  // Maps this session object to a new connection ID                                 
  // We must always ensure that the session's connection id cannot be modififed
  // before we can undo the mapping at disconnect.
  
  const int sessionID = session->sessionID();       

  if  (session->onSessionStart() == -1)   
  {
       MMSLOG((LM_ERROR,"%s ERROR unexpected state on session %d\n", objname, sessionID));       
       return -1;
  }         
                                                                          
  int connectionID = 0, attempts = 0;       
  const static int maxConxIdAttempts = 500;

  while(1)                                  // Get a connection ID 
  {
      if (attempts++ == maxConxIdAttempts)
      {
          MMSLOG((LM_ERROR,"%s ERROR no connection ID available\n", objname));       
          return -1;
      }

      connectionID = this->generateConnectionID();

      if (connectionIDs->find(connectionID) == -1) break;

      MMSLOG((LM_INFO,"%s warning conxID %d has been active for an entire cycle\n",
              objname, connectionID));       
  }

  session->conxID = connectionID;
                                            // Map conx ID to session ID
  if  (connectionIDs->bind(connectionID, sessionID) == -1) 
  {
       MMSLOG((LM_ERROR,"%s ERROR binding conxID\n",objname));       
       return -1;
  } 

  MMSLOG((LM_INFO,"%s connection ID %d bound to client %x %s %d\n", objname,
          connectionID, clientID, session->isUtilitySession()? cus: cs, sessionID));
                    
  session->markInUse();   
                                             
  activeSessions.insert(sessionID);         // Update active sessions list

  return sessionID;
}

             
                                            // Look up an active session object
MmsSession* MmsSessionPool::findByConnectionID(const int connectionID)
{
  int sessionID = -1, result;
                                            // sessionID passed out by reference
  result = connectionIDs->find(connectionID, sessionID);

  return result == -1? NULL: this->findBySessionID(sessionID);
}
  


MmsSession* MmsSessionPool::findBySessionID(const int sessionID)
{                                           
  return (sessionID < 1 || sessionID > this->size())? NULL:
          &sessions [sessionID - 1];        // session ID is a one-based ordinal
}



MmsSession* MmsSessionPool::findAvailableSessionEx(char* map)
{      
  // Return an available session object from either the IP session pool,
  // or the utility session pool, depending on IS_UTILITY_SESSION flag.
  // This version is a circular search of the session pool, beginning
  // after the previously-found session in the pool. The search ends
  // when an unused session if located which has been idle for at least
  // as long as the configured minimum idle time. The session returned
  // is the session thus located, if indeed one was located; failing
  // that, the session idle the longest in the pool; or NULL if no
  // session was idle.

  ACE_Guard<ACE_Thread_Mutex> x(this->sessionPoolLock());  
                             
  if (this->isUtilitySession(map))  
      return this->findAvailableUtilitySession();

  if (MmsAs::g711(MmsAs::RESX_ISZERO))      // LICX G711OUT
      return this->raiseG711RtpExhaustedAlarm();      

  MmsSession* session = this->sessions;
  MmsSession* selectedSession = NULL;
  MmsSession* eldestSession   = NULL;
  const int poolsize  = this->poolSizeMax;
  int priorSessionID  = session->ordinal;
  int startIndex = 0;
  double minsecs = MMS_SESSION_AVAILABLE_AGE_THRESHOLD_MS / 1000.0;
  double maxSessionAgeSecs = 0.0, sessionAgeSecs = 0.0;
  time_t now; time(&now);

  if (this->mruSession)                     // If previously-found session ...
  {                                         // ... start search there
      priorSessionID = this->mruSession->ordinal;
      
      if (priorSessionID < this->poolSizeMax)
      {                                     // Session ID is 1-based ordinal
          startIndex = priorSessionID;      // one greater than its table index
          session = &this->sessions[startIndex];
          maxSessionAgeSecs = 0.0;

          for(int i = startIndex; i < poolsize; i++, session++)
          {
              if (session->isInUse()) continue;
                    
              sessionAgeSecs = difftime(now, session->timestamp);

              if (sessionAgeSecs >= maxSessionAgeSecs)
              {   
                  maxSessionAgeSecs = sessionAgeSecs;
                  eldestSession = session;
                            
                  if (sessionAgeSecs > minsecs)
                  {   selectedSession = session;
                      break;                // End search if "old enough"
                  }
              }                            
          }   // for(int i ...
      }       // if (priorSessionID ... 
  }           // if (this->mruSession ...
   

  if (!selectedSession)                     // If not found yet ...
  {                                         // ... search from beginning
      const int endID = this->mruSession? priorSessionID: poolsize;   
      session = this->sessions;             
      maxSessionAgeSecs = 0.0;

      for(int i = 0; i < endID; i++, session++)
      {     
          if (session->isInUse()) continue;
           
          sessionAgeSecs = difftime(now, session->timestamp);

          if (sessionAgeSecs >= maxSessionAgeSecs)
          {   
              maxSessionAgeSecs = sessionAgeSecs;
              eldestSession = session;
              if (sessionAgeSecs > minsecs)
              {   selectedSession = session;
                  break;                    // End search if "old enough"
              }
          }    
      }  
  }     
 
  this->mruSession = selectedSession? selectedSession: eldestSession; 
  return this->mruSession;
}


                                             
MmsSession* MmsSessionPool::findAvailableUtilitySession()
{    
  // Return an available session object from the utility session pool.                                       
  // Utility sessions are allocated after the IP sessions in the session pool.
  // poolSizeMax is the IP session pool size, so we start after that.

  MmsSession* session = &this->sessions[this->poolSizeMax];                 
  const int poolsize  =  this->utilityTableSize;                    

  for(int i = 0; i < poolsize; i++, session++)
      if  (!session->isInUse())
            return session;

  this->raiseUtilityPoolExhaustedAlarm();
  return NULL;
}



MmsSession* MmsSessionPool::raiseG711RtpExhaustedAlarm()
{
  // Fire an alarm indicating G711 ports exhausted, also log the condition

  char alarmDescription[MmsAlarmParams::maxTextLength+1];
  sprintf(alarmDescription, MmsAs::szResxExhausted, "G.711 RTP");

  MMSLOG((LM_ERROR, "%s %s\n", objname, alarmDescription)); 

  MmsAlarmParams* params = new MmsAlarmParams(MmsReporter::NDX_NO_MORE_CONNECTIONS, 
     MMS_STAT_CATEGORY_RTP, MmsReporter::severityTypeSevere);
  params->settext(alarmDescription);

  MmsReporter::instance()->postMessage(MMSM_ALARMREQUEST, (long)params);
  return NULL;
}



MmsSession* MmsSessionPool::raiseUtilityPoolExhaustedAlarm()
{
  // Fire an alarm indicating utility session pool full, also log the condition

  char alarmDescription[MmsAlarmParams::maxTextLength+1];
  strncpy(alarmDescription, "utility pool full (increase Media.utilityPoolSizeFactor)", 
          MmsAlarmParams::maxTextLength);

  MMSLOG((LM_ERROR, "%s %s\n", objname, alarmDescription)); 

  MmsAlarmParams* params = new MmsAlarmParams(MmsReporter::NDX_UNEXPECTED_CONDITION, 
     MMS_STAT_CATEGORY_SERVER, MmsReporter::severityTypeCaution);
  params->settext(alarmDescription);

  MmsReporter::instance()->postMessage(MMSM_ALARMREQUEST, (long)params);
  return NULL;
}



int MmsSessionPool::returnSessionToAvailablePool(int sessionID)
{
  MmsSession* session = this->findBySessionID(sessionID);
  return session? this->returnSessionToAvailablePool(session): -1;
}


                                            
int MmsSessionPool::returnSessionToAvailablePool(MmsSession* session)
{ 
  // De-assign session/connection
  activeSessions.erase(session->sessionID()); 
  const int connectionID = session->connectionID();
  MMSLOG((LM_DEBUG,"%s unbind connection ID %d\n",objname,connectionID));
                                             
  int result = connectionIDs->unbind(connectionID);
  if (result == -1)                         // Undo the conxID/session mapping
      MMSLOG((LM_ERROR,"%s conxID unbind error\n",objname));       
   
  session->conxID = 0;
  // session->clear(); superfluous here
  return result;
}



int MmsSessionPool::generateOperationID() 
{
  static int opid;
  ACE_Guard<ACE_Thread_Mutex> x(atomicOperationLock);
  if  (++opid > 0xffffff) opid = 1;         // We leave upper byte empty
  return opid;                              // to contain server ID  
}



int MmsSessionPool::generateConnectionID() 
{
  ACE_Guard<ACE_Thread_Mutex> x(atomicOperationLock);
  if  (++conxID > 0xffff) conxID = 1;       // We leave upper word empty
  return conxID;                            // to contain server ID etc
}



void MmsSessionPool::dump(const int ifCountLT)
{
  // Debug dump of session table contents
  static int dumpcount;
  if (ifCountLT && dumpcount++ >= ifCountLT) return;

  ACE_Guard<ACE_Thread_Mutex> x(this->sessionPoolLock());                             
                         
  MmsSession* session = this->sessions;
  const int poolsize  = this->poolSizeMax;  
  static char* xstate[] = {"down","free","idle","busy","pend"};
  MMSLOG((LM_INFO,"%s session table dump follows\n",objname));

  for(int i = 0; i < poolsize; i++, session++)
  {
      MmsSession::Op* op = session->optable;
      const int opcount  = session->opcount;
      char* sname  = session->objname;
      char* sstate = xstate[session->state];

      MMSLOG((LM_INFO,"%s %s ops %d\n", sname, sstate, opcount)); 

      for(int j=0; j < opcount; j++, op++)
          if (!op->isIdle())  
              MMSLOG((LM_INFO,"%s   op %d cmd %d state %d\n", 
                      sname, op->opID(), op->cmdID(), op->State()));
  }      
}

