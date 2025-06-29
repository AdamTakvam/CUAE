//
// mmsMediaEvent.cpp
//
#include "StdAfx.h"
#include "mms.h"
#if defined(MMS_WINPLATFORM) 
#pragma warning(disable:4786)
#endif
#include "mmsMediaEvent.h"

#ifdef MMS_ENABLE_MEMORY_LEAK_DETECTION
#define new MMS_MEMLEAK_NEW
#endif // MMS_ENABLE_MEMORY_LEAK_DETECTION

static char objid[]="EVNT";
MmsEventRegistry* MmsEventRegistry::m_instance = 0;
int  MmsEventRegistry::m_totalRegistrations = 0;
int  MmsEventRegistry::m_isOpen  = 0;
long MmsEventRegistry::m_eventID = 0;


                                            
long MmsEventRegistry::hmpEventHandler(unsigned long)
{            
  // One and only HMP event handler. HMP accepts a function pointer of this
  // signature in sr_enbhdlr. We look up the task which registered the fired
  // event, and notify that task that the event has fired. Party notified must
  // delete the DispatchMap* sent as the param() of the notification message.

  mmsDeviceHandle handle = sr_getevtdev();  // Device handle of event firing
  long  eventType  = sr_getevttype();       // Type of event firing
  int   eventError =(eventType == TDX_ERROR) || (eventType == IPMEV_ERROR);
  long  dataLength = eventError? 0: sr_getevtlen();         
  void* eventData  = dataLength? sr_getevtdatap(): NULL; 
  if  (!m_isOpen) return HMP_KEEP_EVENT;
  // ACE_OS::printf("EVNT event %d e=%d\n",eventType,eventError);   

  if (eventType == EVT_RFC2833)
  {   // Temporary code to determine if we are seeing RFC2833 events
      // See MmsDeviceIP.init(), config.media.rfc2833Enable 
      ACE_OS::printf("EVNT RFC2833 digit event detected\n"); 
      return HMP_KEEP_EVENT;
  }
  
  DispatchMap dispatchMap;
  dispatchMap.flags |= DispatchMap::INTERNAL_RETAIN;                       
                                             
  MmsEventRegistry* thisx = MmsEventRegistry::instance(); 
                                            // Locate firing event in event     
  int  result = eventError?                 // table & unregister if one-shot
       thisx->searchUnregister(handle, &dispatchMap):  
       thisx->findUnregister(handle, eventType, &dispatchMap); 

  if  (result == -1) return HMP_KEEP_EVENT;                              
                                            // Get party registered for event
  MmsTask* task = (MmsTask*)dispatchMap.partyToNotify;
  if  (task == NULL) return HMP_KEEP_EVENT;
              
  DispatchMap* clone = dispatchMap.clone(); // Make a copy for them

  if  (eventError)                          // If error event ...
  {                                           
       clone->flags |=                      // ... return error code
         (DispatchMap::EVENT_ERROR | DispatchMap::DATALENGTH_IS_ERRORCODE); 
 
       clone->returnDataLength = ATDV_LASTERR(handle); 

       ACE_OS::printf("EVNT HMP reports '%s' on device %d\n",
               ATDV_ERRMSGP(handle), handle);
  }
  else
  if  (eventData)                           // Retrieve event data if any
       retrieveEventData(eventData, dataLength, clone);    
                                            // Notify dispatchee of event
  task->postMessage(MMSM_MEDIAEVENT, (long)clone); 
 
  return HMP_RELEASE_EVENT;                 // Indicate we handled the event                       
}



int MmsEventRegistry::retrieveEventData
( const void* data, const long dataLength, DispatchMap* map) 
{
  // Retrieve event data, such as collected digits. Returns data length.
  // If dispatchee has requested data return, but has not supplied a buffer
  // for the data, we allocate one and set dispatchmap.flags | HDLR_ALLOCATED;
  // This memory is automatically freed as dispatchee frees the DispatchMap*

  ACE_ASSERT((map->returnDataLength &&  map->returnData)
          ||(!map->returnDataLength && !map->returnData)); 
                                            // Ensure dispatchee wants data
  if  (map->flags & DispatchMap::NODATARETURN) return 0;
                                            // Ensure sufficient buffer 
  if ((map->returnDataLength  > 0  && map->returnDataLength >= dataLength)     
   || (map->returnDataLength == 0));         
  else return 0;                               
                                                
  if  (map->returnDataLength == 0)          // Or we allocate return buffer
  {    map->returnData = new unsigned char[dataLength];
       map->flags |= DispatchMap::HDLR_ALLOCATED;
  }
                                            
  memcpy(map->returnData, data, dataLength);// Copy event data
  map->returnDataLength = dataLength;       // Return actual data length

  return map->returnDataLength;
}




int MmsEventRegistry::registerOneShotEvent(mmsDeviceHandle handle, 
  const long eventType, const int session, const long eventID, const void* dispatchee,
  const int returnBufferLength, void* returnBuffer)      
{  
  // Register for a one-shot event. When a one-shot event fires, 
  // the event is removed from the dispatch table. Parameters are:
  // eventType:  Type of event as known to HMP, such as TDX_PLAY.
  // sessionID:  Ordinal of the MMS session which is waiting on the event.
  // eventID:    A unique identifier assigned by this->assignEventID(). 
  // dispatchee: MmsTask to receive MMSM_MEDIAEVENT when event fires.
  // returnBufferLength and returnBuffer. Optional, see following.
  // 
  // If returnBufferLength is positive, it specifies the length of the 
  // dispatchee buffer area, pointed to by returnBuffer, which is to receive
  // event data, if any, such as DTMF digits. If returnBufferLength is 
  // passed as -1, this means that dispatchee does not wish to receive
  // event data associated with the firing event. If returnBufferLength is
  // zero, and there is data associated with the firing event, returnBuffer
  // will be allocated by the event handler. The event handler will reset 
  // returnBufferLength to the actual length of the event data returned.
  // Dispatchee is responsible for deleting the DispatchMap* sent via a 
  // MMSM_MEDIAEVENT. If event handler allocated an event data buffer
  // as described above, this memory will be automatically freed  
  // at the time dispatchee deletes the DispatchMap*. 

  // Recent mod: event ID assigned internally, and returned from register()
  // We should eventually remove the eventID parameter from the class methods.          
                                        
  return internalInterface(REGISTER, handle, eventType, session, eventID, 
         (void*)dispatchee, returnBufferLength, returnBuffer);
}



int MmsEventRegistry::registerRecurringEvent(mmsDeviceHandle handle, 
  const long eventType, const int session, const long eventID, const void* dispatchee, 
  const int returnBufferLength, void* returnBuffer)                
{                                           
  // Register for a recurring event. A recurring event remains in the dispatch
  // table until unregistered, meaning dispatchee will continue to be notified
  // each time the event fires. See registerOneShot above for parameters.  
                                              
  return internalInterface(REGISTER, handle, eventType, 0 - session, eventID,
         (void*)dispatchee, returnBufferLength, returnBuffer);
}                                           // Session negative means recurring



int MmsEventRegistry::unregister(mmsDeviceHandle handle, const long eventType)   
{                                              
  return internalInterface(UNREGISTER, handle, eventType);
}



int MmsEventRegistry::find(mmsDeviceHandle handle, const long eventType, DispatchMap* out)
{                                              
  return internalInterface(FIND, handle, eventType, 0, 0, out);
}



int MmsEventRegistry::search(mmsDeviceHandle handle, DispatchMap* out)
{                                              
  return internalInterface(SEARCH, handle, 0, 0, 0, out);
}



int MmsEventRegistry::findUnregister(mmsDeviceHandle handle, 
    const long eventType, DispatchMap* out)
{                                              
  return internalInterface(FIND_UNREGISTER, handle, eventType, 0, 0, out);
}



int MmsEventRegistry::searchUnregister(mmsDeviceHandle handle, DispatchMap* out)
{                                              
  return internalInterface(SEARCH_UNREGISTER, handle, 0, 0, 0, out);
}



int MmsEventRegistry::internalInterface(const int action, mmsDeviceHandle handle, 
    const long eventType, const int session, const long eventID, void* map,
    const int returnBufferLength, void* returnBuffer)              
{   
  // Synchronized interface to all possibly-concurrent activity 
  ACE_Guard<ACE_Thread_Mutex> x(m_mutex);
  int result = -1;
  if (m_config && m_config->diagnostics.flags & MMS_DIAG_LOG_EVENT_TABLE) dump("bef"); 

  switch(action)
  { case REGISTER: 
         result = registerInternal(handle, eventType, session, eventID, map,
                                   returnBufferLength, returnBuffer);
                                            
         if ((++m_totalRegistrations % PRUNE_EVERY_N) == 0) 
              this->prune(THIRTY_MINUTES);  // Prune orphans periodically 
         break;

    case UNREGISTER:
         result = unregisterInternal(handle, eventType);
         break;

    case FIND_UNREGISTER:
         result = findUnregisterInternal(handle, eventType, (DispatchMap*)map);
         break;

    case SEARCH_UNREGISTER:
         result = searchUnregisterInternal(handle, (DispatchMap*)map);
         break;

    case FIND:
         result = findInternal(handle, eventType, (DispatchMap*)map);
         break;

    case SEARCH:
         result = searchInternal(handle, (DispatchMap*)map);
         break;
  }
      
  if (m_config && m_config->diagnostics.flags & MMS_DIAG_LOG_EVENT_TABLE) dump("aft"); 
                                                             
  return result;                            
}



int MmsEventRegistry::registerInternal(mmsDeviceHandle handle, 
  const long eventType, const int session, const long eventid, void* dispatchee,
  const int returnBufferLength, void* returnBuffer)        
{                                           
  // Register for an event, inserting the event into dispatch table.
  // If this is to be a recurring event, caller will have passed the 
  // session ID parameter to us signed negative. 

  int isRecurringEvent = session < 0;
  int sessionID        = session < 0? 0 - session: session;

  int eventID = eventid? eventid: this->assignEventID();   
                                             
  DispatchMap dispatchMap;                  // Build event dispatch map  
  dispatchMap.eventType = eventType;       
  dispatchMap.eventID   = eventID; 
  dispatchMap.sessionID = sessionID;                                                                                    
  dispatchMap.partyToNotify = dispatchee; 
                                            // Return length -1 means
  if  (returnBufferLength == -1)            // do not return any event data
       dispatchMap.flags |= DispatchMap::NODATARETURN;
  else
  if  (returnBuffer && (returnBufferLength > 0))
  {                                         // Dispatchee owns return buffer
       dispatchMap.returnData       = returnBuffer;
       dispatchMap.returnDataLength = returnBufferLength;
  } 

  dispatchMap.stamp();

  if  (isRecurringEvent)                     
       dispatchMap.flags |= DispatchMap::RECURRING;

  EVENTKEY(eventKey,handle,eventType);      // Build 64-bit event key
     
  eventTable[eventKey] = dispatchMap;       // First register for MMS dispatch
                                            // Then register with HMP
  if  (-1 == sr_enbhdlr(handle, eventType, this->hmpEventHandler))
  {                                
       MMSLOG((LM_ERROR,"EVNT sr_enbhdlr h=%d k=%d t=%d: %s\n",
               handle, eventKey, eventType, ATDV_ERRMSGP(handle)));

       this->dump("entry");
          
       eventTable.erase(eventTable.find(eventKey));                           
       return -1;                              
  }
                                                                       
  return eventID;                            
}



int MmsEventRegistry::findInternal(mmsDeviceHandle handle, const long eventType, DispatchMap* out)
{
  // Finds event matching handle and type; if found, copies event data to caller
  // Depends upon caller synchronization, however may be synchronized,
  // since table entry is copied to caller in scope

  EVENTKEY(eventKey,handle,eventType);

  EventDispatchTable::iterator entry = eventTable.find(eventKey);

  if  (entry == eventTable.end()) return -1;

  DispatchMap& dispatchMap = entry->second;

  memcpy(out, &dispatchMap, sizeof(DispatchMap));

  return 0;
}



int MmsEventRegistry::searchInternal(mmsDeviceHandle handle, DispatchMap* out)
{
  // Locates event matching handle. All comments in searchInternal apply. 
  EventDispatchTable::iterator i; 
               
  for(i = eventTable.begin(); i != eventTable.end(); i++)
  {  
      const _int64 eventKey  = i->first;
      mmsDeviceHandle registeredHandle = EVENTKEY_HANDLE(eventKey);
      if  (handle  == registeredHandle) break;
  }

  if  (i == eventTable.end()) return -1;

  DispatchMap& dispatchMap = i->second;

  return 0;
}



int MmsEventRegistry::unregisterInternal(mmsDeviceHandle handle, const long eventType)
{ 
  // Removes event matching handle and type from dispatch table.
  // Depends upon caller synchronization, however is synchronizable
  EVENTKEY(eventKey,handle,eventType);

  EventDispatchTable::iterator entry = eventTable.find(eventKey);
  if  (entry == eventTable.end()) return -1;

  DispatchMap& dispatchMap = entry->second;

  sr_dishdlr(handle, dispatchMap.eventType, this->hmpEventHandler); 

  eventTable.erase(entry);                  // Could omit and wait for prune

  return 0;
}



int MmsEventRegistry::findUnregisterInternal(mmsDeviceHandle handle, 
    const long eventType, DispatchMap* out)
{
  // Locates event matching handle and type. If found, copies event data 
  // to caller and deletes event from the dispatch table.
  // Depends upon caller synchronization, however can be synchronized.
  // If this method is invoked from the event handler, a bitflag
  // parameter will have been passed to us in the output DispatchMap.
  // When this is the case, the unregister part of the call is conditional
  // on whether the registered event is a recurring event or a one-shot.

  EVENTKEY(eventKey,handle,eventType);

  EventDispatchTable::iterator entry = eventTable.find(eventKey);

  if  (entry == eventTable.end()) return -1;

  DispatchMap& dispatchMap = entry->second; 
                                            // Parameters may have been
  int  isinvokedFromEventHandler            // passed us in the out map
     = out->flags & DispatchMap::INTERNAL_RETAIN;

  memcpy(out, &dispatchMap, sizeof(DispatchMap));
                                            // Unless firing recurring event ...
  if  (isinvokedFromEventHandler && (dispatchMap.flags & DispatchMap::RECURRING));
  else                                      // ... unregister event 
  {    sr_dishdlr(handle, eventType, this->hmpEventHandler); 
                                            // Note: we could omit the erase
       eventTable.erase(entry);             // here and let prune() pick it up
  }

  return 0;
}



int MmsEventRegistry::searchUnregisterInternal(mmsDeviceHandle handle, DispatchMap* out)
{
  // Locates event matching handle. All comments in findUnregisterInternal apply.
  if  (eventTable.size() == 0) return 0; 
  EventDispatchTable::iterator i; 
               
  for(i = eventTable.begin(); i != eventTable.end(); i++)
  {  
      const _int64 eventKey  = i->first;
      mmsDeviceHandle registeredHandle = EVENTKEY_HANDLE(eventKey);
      if  (handle  == registeredHandle) break;
  }

  if  (i == eventTable.end()) return -1;

  DispatchMap& dispatchMap = i->second;
                                            // Parameters may have been
  int  isinvokedFromEventHandler            // passed us in the out map
     = out->flags & DispatchMap::INTERNAL_RETAIN;

  memcpy(out, &dispatchMap, sizeof(DispatchMap));
                                            // Unless firing recurring event ...
  if  (isinvokedFromEventHandler && (dispatchMap.flags & DispatchMap::RECURRING));
  else                                      // ... unregister event 
  {    sr_dishdlr(handle, dispatchMap.eventType, this->hmpEventHandler); 
                                            // We could omit the erase here
       eventTable.erase(i);                 // and let prune() pick it up
  }

  return 0;
}



int MmsEventRegistry::registerForErrorEvents()
{
  return (-1 == sr_enbhdlr(EV_ANYDEV, TDX_ERROR,   hmpEventHandler))
      || (-1 == sr_enbhdlr(EV_ANYDEV, IPMEV_ERROR, hmpEventHandler))?                                 
          -1: 0;                              
} 



int MmsEventRegistry::unregisterForErrorEvents() 
{
  return (-1 == sr_dishdlr(EV_ANYDEV, TDX_ERROR,   hmpEventHandler))
      || (-1 == sr_dishdlr(EV_ANYDEV, IPMEV_ERROR, hmpEventHandler))?                                 
          -1: 0;  
}


                                            
long MmsEventRegistry::assignEventID()      // Return a unique ID for an event
{
  ACE_Guard<ACE_Thread_Mutex> x(m_atomicOperationLock);
  return ++m_eventID;
}



int MmsEventRegistry::open()
{ 
  if (m_isOpen) return -1;
  this->eventTable.clear();
  m_isOpen = 1;

  //efine PRUNE_TEST    // Register a bogus event
  #ifdef  PRUNE_TEST    // which will get pruned asap
  this->registerOneShotEvent(9999, 8888, 7777, 0, 0); 
  MMSLOG((LM_NOTICE,"EVNT registered bogus event for session 777 et 8888, dev 9999\n"));
  this->dump("aft");
  #endif

  return 0;
} 



void MmsEventRegistry::close()
{ 
  if  (!m_isOpen) return;
  cancelAll();
} 



void MmsEventRegistry::cancelAll()
{  
  // Cancels all outstanding events registered herein
  // Assumed to be shutdown condition so does not clear the dispatch table. 

  if  (!m_isOpen || eventTable.size() == 0) return;
  unregisterForErrorEvents();
  EventDispatchTable::iterator i;  
              
  for(i = eventTable.begin(); i != eventTable.end(); i++)
  { 
    DispatchMap& dispatchMap = i->second; 
    dispatchMap.timestamp    = 0;
    dispatchMap.partyToNotify = NULL;

    const _int64 eventKey  = i->first;
    mmsDeviceHandle handle = EVENTKEY_HANDLE(eventKey);

    int result = sr_dishdlr(handle, dispatchMap.eventType, hmpEventHandler);
  }
} 



void MmsEventRegistry::prune(const int seconds)
{ 
  // Removes from event table one-shot events elder than specified number of 
  // seconds, or any events which have been marked for deletion.
  // Not synchronized, assumes call within scope of existing critical section

  long registeredEventCount = 0, eventType = 0;
  int  erResult = 0, result = 0, sessionID = 0;
  _int64 eventKey = 0;
  mmsDeviceHandle handle = 0;
  EventDispatchTable::iterator i;

  // When app server client is restarted, MMS removes its orphaned events here
  // shortly afterward. There may have been an access violation during one
  // such prune, so we have wrapped the entire process in a try/catch to attempt
  // to pinpoint the problem, if there is one.

  try
  {
    while(1)
    {
      registeredEventCount = eventTable.size();
      if (registeredEventCount == 0) break;

      for(i = eventTable.begin(); i != eventTable.end(); i++)
      {           
          DispatchMap& dispatchMap = i->second;
          const _int64 eventKey    = i->first;

          if ((dispatchMap.flags & DispatchMap::DELETED) || 
            (((dispatchMap.flags & DispatchMap::RECURRING) == 0) && 
              dispatchMap.isElderThan(seconds))) 
              break;                      // Expired entry found to delete
      }
                                             
      if (i == eventTable.end()) break;    // If no expired entry found, done
      
      DispatchMap& dispatchMap = i->second;
      eventKey = i->first;

      handle = EVENTKEY_HANDLE(eventKey);
      eventType = dispatchMap.eventType;
      sessionID = dispatchMap.sessionID;

      MMSLOG((LM_INFO,"EVNT removing expired registration for session dev %d et %d\n",
              sessionID, handle, eventType)); 

      const int srResult = sr_dishdlr(handle, eventType, hmpEventHandler);

      if (srResult < 0)                 // Unregister event with HMP
          MMSLOG((LM_ERROR,"EVNT sr_dishdlr error pruning sid %d dev %d et %d\n",
                  dispatchMap.sessionID, handle, eventType));
     
      erResult = -1;
      eventTable.erase(i);              // Remove expired event table entry
      erResult = 0;
    }  // while(1)
  }    // try
  catch(...) { result = -1; }

  if (erResult < 0)
      MMSLOG((LM_ERROR,"EVNT exception pruning entry sid %d dev %d et %ld\n",
              sessionID, handle, eventType)); 

  if (result < 0)
      MMSLOG((LM_ERROR,"EVNT exception while pruning event sid %d dev %d et %ld\n",
              sessionID, handle, eventType));  
}



void MmsEventRegistry::dump(char* txt)
{  
  if  (eventTable.size() == 0) 
  {
       MMSLOG((LM_DEBUG,"EVNT %s empty\n",txt));
       return;
  }

  EventDispatchTable::iterator i; 
  int j=0;
               
  for(i = eventTable.begin(); i != eventTable.end(); i++)
  {  
      const _int64 eventKey = i->first;
      DispatchMap& dm       = i->second;
      mmsDeviceHandle dh    = EVENTKEY_HANDLE(eventKey);
     
      MMSLOG((LM_NOTICE,"EVNT %s %d: dh=%d, et=%d, eid=%d, sid=%d, fl=%d\n",
              txt, j++, dh, dm.eventType, dm.eventID, dm.sessionID, dm.flags));      
  } 
}



void MmsEventRegistry::destroy()
{
  if  (m_instance)
       delete m_instance;
  m_instance = NULL;
}


                                            
MmsEventRegistry::MmsEventRegistry()        // Private ctor
{           
  m_isOpen  = 0;
  m_eventID = 0;
  m_totalRegistrations = 0;
} 



MmsEventRegistry::~MmsEventRegistry()       // Dtor
{ 
  // Singleton - destroy explicitly
}

         
// Note: no reason to include an event registry record lock, since each entry 
// belongs to a single session object, and a session does not permit multiple
// service threads to operate on it 


