//
// mmsMediaResourceMgr.h
//
#ifndef MMS_MEDIARESOURCEMGR_H
#define MMS_MEDIARESOURCEMGR_H
#ifdef MMS_WINPLATFORM
#pragma once
#endif
#include <map>
#include <set>
#include <list>
#include <algorithm>
#include "mmsMediaDevice.h"
#include "mmsFlatMap.h"
#include "mmsTask.h"



class MmsMediaDeviceInventory               // Maintains device board/card counts
{  
  public:
  enum deviceTypes {IP=1, VOICE, CONFERENCE}; 
  enum{NUMDEVICES = CONFERENCE};

  void setBoardCount(const int deviceType, const int n)
  { ACE_ASSERT(deviceType > 0 && deviceType <= NUMDEVICES);
    deviceCount[deviceType].setBoardCount(n);
  }

  int getBoardCount(const int deviceType)
  { ACE_ASSERT(deviceType > 0 && deviceType <= NUMDEVICES);
    return deviceCount[deviceType].getBoardCount();
  }

  void setCardCount(const int deviceType, const int i, const int n)
  { ACE_ASSERT(deviceType > 0 && deviceType <= NUMDEVICES);
    deviceCount[deviceType].setCardCount(i, n);
  }

  int getCardCount(const int deviceType, const int i)
  { ACE_ASSERT(deviceType > 0 && deviceType <= NUMDEVICES);
    return deviceCount[deviceType].getCardCount(i);
  }

  int getDeviceCount(const int deviceType)
  { ACE_ASSERT(deviceType > 0 && deviceType <= NUMDEVICES);
    int totalDevices= 0;
    for(int i=1; i <= this->deviceCount[deviceType].boardCount; i++)
        totalDevices += this->deviceCount[deviceType].cardCount[i];
    return totalDevices;
  }

  virtual ~MmsMediaDeviceInventory() { }

  protected:

  struct hmpDeviceCount                     // Data structure consisting of an 
  { int  boardCount;                        // HMP board count, each with a 
    int* cardCount;                         // variable number of card counts

    hmpDeviceCount(): cardCount(NULL), boardCount(0) { }
    virtual ~hmpDeviceCount() { if (cardCount) delete[] cardCount; }

    void setBoardCount(const int n) 
    { ACE_ASSERT(boardCount == 0);
      boardCount = n; 
      cardCount  = new int[n+1];            // Board & card indexes are 1-based
      memset(cardCount, 0, sizeof(int) * (n+1)); 
    }
    void setCardCount (const int i, const int n) { cardCount[i] = n; }
    int  getBoardCount() { return boardCount; }
    int  getCardCount (const int i) { return cardCount? cardCount[i]: 0; }
  };
  
  hmpDeviceCount deviceCount[NUMDEVICES+1]; // IP=[1], voice=[2], conf=[3] 
};



class HmpResourceManager                    // Manages HMP devices
{ 
  // The terminology is no longer correct here. An HMP resource and an HMP device  
  // are not the same. Wherever we mention "resource" here, think "device".

  protected:
  static HmpResourceManager* m_instance;    // Singleton 
  HmpResourceManager() { }
  HmpResourceManager& operator=(const HmpResourceManager&) { };

  public:
  static HmpResourceManager* instance();
 
  enum resourceType
  { MEDIA_RESOURCE_TYPE_ALL, 
    MEDIA_RESOURCE_TYPE_IP    = MmsMediaDevice::IP,
    MEDIA_RESOURCE_TYPE_VOICE = MmsMediaDevice::VOICE,
    MEDIA_RESOURCE_TYPE_CONFERENCE = MmsMediaDevice::CONF,
    MMS_NUM_RESOURCE_TYPES,          
  };

  enum gadBitflags { GAD_USEIDLE = 1, GAD_CSP_CAPABLE = 2 };
                                            // Assign resource of specified type
  mmsDeviceHandle getResource(const int resourceType, const int flags=0);    
                                            // Given handle return device object              
  MmsMediaDevice* getDevice(const mmsDeviceHandle handle);
  int  releaseResource(mmsDeviceHandle);    // Return resource to resource pool
  int  removeResource (mmsDeviceHandle);    // Remove resource from pool
                                             
  mmsDeviceHandle getConferencingResource();// Convenience methods
  MmsMediaDevice* getConferencingDevice();  

  int  init(MmsConfig* config);             // Initialize resource pool
  void shutdown();                          // Shut down resource pool
  void closeAll();                          // Close all open resources

  virtual ~HmpResourceManager();            // Dtor
  void destroy();                           // Delete singleton instance

  int  resourcePoolAvailableCount(int resourceType, int useidle=0);
  int  resourcePoolBusyCount(int resourceType);
  int  resourcePoolIdleCount(int resourceType);
  int  idleTimesCount()   { return mediaResourceVoiceIdleTimes.size(); }
  int  idleMappingCount() { return mediaResourceTableVoiceIdle.size(); }

  int  resourcePoolAvailable(mmsDeviceHandle, int bAvailable=TRUE);
  int  resourcePoolUnavailable(mmsDeviceHandle);
  int  resourcePoolBusy(mmsDeviceHandle);
  int  resourcePoolIdle(mmsDeviceHandle,   int getlock=0);
  int  resourcePoolUnidle(mmsDeviceHandle, int getlock=0);

  MmsMediaDeviceInventory& deviceInventory() { return m_deviceCounts; }
  int  getDeviceCount(int devType) {return m_deviceCounts.getDeviceCount(devType);}
  int  getResourceCountConference(const mmsDeviceHandle=0);
  int  getMaxHmpResourceCounts(mmsHmpRegistryResourceCounts*);
  int  isVoxUnavailable(mmsDeviceHandle);
  int  isVoxAvailable(mmsDeviceHandle);
  int  isInternalLicensingOverridePresent(); // LICX
  int  calculateThreadPoolSize (const int maxconnections);
  int  calculateUtilityPoolSize(const int maxconnections);
  mmsDeviceHandle getCapableVoxDevice(const int caps);
  int  adjustResourcesLicensedToActual();
  static void showLicensedLimits(const int islog=0);

  MmsConfig* config;
  MmsTask*   reporterQueue;
  mmsHmpRegistryResourceCounts hmpResourceCounts;
  char* conferencingResourcesMap() { return conferencingResourcesTotal; }

  mmsDeviceHandle selectConferencingDevice();

  friend class MmsSessionManager;            

  protected:

  int  getDeviceCounts();
  int  getDeviceCountIP(); 
  int  getDeviceCountVoice();
  int  getDeviceCountConference();
  int  setResourceCounts();

  int  getLicensedResourceCounts();    // LICX
  int  readLicensingService();         
  int  loadLicenseManager(void* info); 
  void assignOverrideResourceLimits();
  void assignSdkResourceLimits();
  void assignLicensedResourceLimits(void*);
  void applyResourceCeiling(void*, const int ceiling);
  void setResourceHeadroom(const int pct);
  int  calculateResourceHeadroom(const int count, const int pct);

  int  loadDevicePool();

  mmsDeviceHandle resourcePoolGetAvailableDevice
    (int resourceType, MmsTime* timeout=0, const int flags=0); 

  mmsDeviceHandle resourcePoolGetIdleDevice(const int strategy, const int unidle=0);

  mmsDeviceHandle getAvailableVoiceResource(MmsTime* timeout);

  mmsDeviceHandle getVoiceResourceImmediate(int* wasIdle);

  int setResourceAcquisitionParameters(const int resourceType, 
      MmsTime** timeout, MmsTime& voicetimeout, int* useIdle);
                                            // Map time idled to device
  time_t createIdleTimeMapping(mmsDeviceHandle, time_t);

  MmsMediaDeviceInventory m_deviceCounts;
  int isLicensingServiceAvailable;
  int isSdkLicenseProfile, isInternalLicenseProfile;
  int firmwareTotalLicensedConferees;
  int resourcesInitial[MMS_NUM_RESOURCE_TYPES];
  MmsMediaDevice::deviceKey currentKey[MMS_NUM_RESOURCE_TYPES];

  Hmp* hmp; 
                                            // Media resources keyed by handle
  typedef std::map<mmsDeviceHandle, MmsMediaDevice*> MmsMediaDeviceTable;
                                            // Resource handles keyed by time
  typedef std::map<time_t, mmsDeviceHandle> TimeToMmsDeviceMap;
  typedef std::set<time_t> OrderedTimes;    // Times ordered  
                                            // Dynamic device collection sequ    
  typedef std::list<mmsDeviceHandle> MmsMediaDeviceList;
                                            // Static device collection random
  typedef std::set <mmsDeviceHandle> MmsMediaDeviceLookup;

                                            
  MmsMediaDeviceTable  mediaResourceTable;  // Resource inventory lookup table
                                            // Static resource lists by type
  MmsMediaDeviceLookup mediaResourceTableIP;// with fast lookup
  MmsMediaDeviceLookup mediaResourceTableVoice;
  MmsMediaDeviceLookup mediaResourceTableConference;
                                            // Dynamic lists of available resx
  MmsMediaDeviceList   mediaResourceTableIPAvailable;
  MmsMediaDeviceList   mediaResourceTableVoiceAvailable;
  MmsMediaDeviceList   mediaResourceTableConferenceAvailable;
                                            // Idle voice devices by time idled
  OrderedTimes         mediaResourceVoiceIdleTimes;
  TimeToMmsDeviceMap   mediaResourceTableVoiceIdle;

  ACE_Thread_Mutex     resourceLock;        // Lock to acquire media resource
  ACE_Thread_Mutex     voiceResourceLock;   // Lock on voice resources
                                            // Queue to wait on avail. resource
  ACE_Thread_Mutex     voiceResourceAvailableLock; 
  ACE_Condition<ACE_Thread_Mutex>* voiceResourceAvailable;

  ACE_Thread_Mutex     atomicOperationLock;
  int   isVoiceResourceAvailable;
  int   signalVoiceResourceAvailable(int broadcast=0);
  int   isWaitingForVoiceResource();
  char* conferencingResourcesTotal;         // Flatmap of handle to resx count
}; 



#define MMSRM_DEMAND_CSP_CAPABLE     1
#define MMSRM_PREFER_NON_CSP_CAPABLE 2


#define NOVOXWARNING \
 "RESX alert: no voice resources; configuring as conferencing server only\n"
#define NOCONFWARNING \
 "RESX alert: no conference resources; conferencing unavailable on this server\n"
                    
    
 
#endif