//
// mmsMediaEvent.h
//
// HMP event handling. Concurrent interface to global events
//
#ifndef MMS_MEDIAEVENT_H
#define MMS_MEDIAEVENT_H
#ifdef MMS_WINPLATFORM
#pragma once
#pragma warning(disable:4786)
#endif
#include "srllib.h"
#include <map>
#include "mmsTask.h"
#include "mmsMedia.h"
#include "mmsConfig.h"
#include <errno.h>
#include <time.h>



class MmsEventRegistry
{       
  // Implements a dispatch table which maps an HMP event (device + event type)
  // to an MmsTask instance expecting event messages (MmsTask* + event ID).
  // Consumer registers here for a recurring event, or a one-shot event.
  // When event fires, the DispatchMap* event table entry is posted to the
  // dispatchee task in a MMSM_MEDIAEVENT message. Dispatchee must free
  // this DispatchMap*, and must eventually unregister recurring events.
  public:
  static MmsEventRegistry* instance()
  {
    if (!m_instance) 
    {    m_instance = new MmsEventRegistry; 
         m_instance->m_config = NULL;
    } 
    return m_instance;
  }

  virtual ~MmsEventRegistry();

  #ifdef MMS_WINPLATFORM
  #pragma pack(1)
  #endif

  struct DispatchMap                        // Event registration information:
  { 
    long   eventType;                       // HMP event identifier eg TDX_PLAY                    
    int    eventID;                         // A unique id for this transaction
    int    sessionID;                       // MMS session object key
    int    operationID;                     // MMS session operation key
    time_t timestamp;                       // Time of registration
    void*  partyToNotify;                   // Instance pointer of dispatchee
    void*  returnData;                      // User or allocated return data buf
    int    returnDataLength;                // Length of user buf & of return
    unsigned int flags;                     // Bit flags
    enum bitflags
    { DELETED=1,                            // Event marked for deletion
      RECURRING=2,                          // Recurring event (not one-shot)
      NODATARETURN=4,                       // Dispatchee does not want data
      HDLR_ALLOCATED=8,                     // Handler allocated returnData 
      EVENT_ERROR=16,                       // Error event fired
      DATALENGTH_IS_ERRORCODE=32,           // Error code in returnDataLength
      INTERNAL_RETAIN=0x10000               // Internal temporary parameter
    }; 
             
    DispatchMap() { memset(this,0,sizeof(DispatchMap)); }
   ~DispatchMap() { conditionallyFreeDataBuffer(); }
    void stamp(int b=1) { if(b) time(&timestamp); else timestamp=0; }
    bool isElderThan(const int secs) {return difftime(time(0), timestamp) > secs;} 

    DispatchMap* clone() 
    { DispatchMap* p = new DispatchMap; memcpy(p,this,sizeof(DispatchMap));
      return p;
    }

    void conditionallyFreeDataBuffer() 
    { if (returnData && (flags & HDLR_ALLOCATED))
      {   
          delete[](unsigned char*)returnData;
          returnData = NULL;
      }
    }
  };
 
  #ifdef MMS_WINPLATFORM
  #pragma pack()
  #endif

                      
  int registerRecurringEvent(mmsDeviceHandle handle, const long eventType, 
      const int session, const long eventID, const void* dispatchee,
      const int returnBufferLength=0, void* returnBuffer=0); 

  int registerOneShotEvent(mmsDeviceHandle handle, const long eventType, 
      const int session, const long eventID, const void* dispatchee,
      const int returnBufferLength=0, void* returnBuffer=0); 

  int unregister(mmsDeviceHandle handle, const long eventType);

  int find(mmsDeviceHandle handle, const long eventType, DispatchMap* outEntry);

  int search(mmsDeviceHandle handle, DispatchMap* outEntry);

  int findUnregister(mmsDeviceHandle handle, const long eventType, DispatchMap* outEntry); 

  int searchUnregister(mmsDeviceHandle handle, DispatchMap* outEntry);

  int  open();

  void close();

  void cancelAll();

  void destroy();

  int  entries() { return eventTable.size(); }

  static int  registerForErrorEvents();

  static int  unregisterForErrorEvents();

  static long hmpEventHandler(unsigned long);

  static int  retrieveEventData(const void* data, const long length, DispatchMap* map);

  void   setConfig(MmsConfig* config) { m_config = config; }

  void dump(char* txt);

  protected:

  static MmsEventRegistry* m_instance;  
           
  MmsEventRegistry();

  MmsEventRegistry& operator=(const MmsEventRegistry&) { };

  int  internalInterface(const int action, mmsDeviceHandle handle, 
       const long eventType, const int session=0, const long eventID=0, 
       void* map=0, const int returnBufferLength=0, void* returnBuffer=0);  

  int  registerInternal(mmsDeviceHandle handle, const long eventType, 
       const int session, const long eventID, void* map,
       const int returnBufferLength, void* returnBuffer);  

  int  findInternal(mmsDeviceHandle handle, const long eventType, DispatchMap* outEntry);

  int  findUnregisterInternal(mmsDeviceHandle handle, const long eventType, 
       DispatchMap* outEntry);

  int  searchInternal(mmsDeviceHandle handle, DispatchMap* outEntry);

  int  searchUnregisterInternal(mmsDeviceHandle handle, DispatchMap* outEntry);

  int  unregisterInternal(mmsDeviceHandle handle, const long eventType);

  void prune(const int seconds);

  long assignEventID();

  ACE_Thread_Mutex m_mutex;
  ACE_Thread_Mutex m_atomicOperationLock;

  static int  m_totalRegistrations;
  static int  m_isOpen;
  static long m_eventID;                    // Global unique event ID number
  MmsConfig*  m_config;

  enum {PRUNE_EVERY_N = 256, THIRTY_MINUTES = 1800};
  enum {REGISTER, UNREGISTER, FIND, FIND_UNREGISTER, SEARCH, SEARCH_UNREGISTER};

  typedef std::map<_int64, DispatchMap> EventDispatchTable;
                                            
  EventDispatchTable eventTable;            // Global pending HMP event table
                                            // Make 64-bit key = handle|type
  #define EVENTKEY(x,h,t) _int64 x=h; x <<=32; x |= (t&0xffffffff) // for win64
                                            // Extract handle from 64-bit key
  #define EVENTKEY_HANDLE(k) ((mmsDeviceHandle)(k>>32))
  #define EVENTKEY_TYPE(k)   ((long)(k & 0xffffffff))

  // This could exist as a non-singleton; however each static event handler
  // must know of a static location to find the event table it needs.
  // So either (a) a given event handler is associated with a singleton class
  // instance of an event table (as it is now); (b) an event handler looks up
  // the non-static event table it needs, by accessing a static directory of
  // event tables; or (c) event table instances are global and extern.
};


 
#endif