//
// Allocates, serves, manages, and deletes HMP resources
// These are IP, voice, and conferencing resources
//
#include "StdAfx.h"
#include "mms.h"
#ifdef  MMS_WINPLATFORM
#pragma warning(disable:4786)
#include <minmax.h>
#endif
#include "mmsConfig.h"
#include "mmsMediaResourceMgr.h"
#include "mmsDeviceIP.h"
#include "mmsDeviceVoice.h"
#include "mmsDeviceConference.h"
#include "mmsBuild.h"
#include "CUAELicMgr.h"                     // OEM license manager DLL

#ifdef MMS_ENABLE_MEMORY_LEAK_DETECTION
#define new MMS_MEMLEAK_NEW
#endif // MMS_ENABLE_MEMORY_LEAK_DETECTION

static const char* ltwng = "RESX warning: available %s %s (%d) less than licensed %s (%d)\n";
static const char* cdevx = "devices", *cslots = "slots", *cresx = "resources";

HmpResourceManager* HmpResourceManager::m_instance = 0;


   
                                            // Get available resx of specified type
mmsDeviceHandle HmpResourceManager::getResource(const int resourceType, const int flags)
{ 
  static MmsTime voiceTimeout(0);
  mmsDeviceHandle handle = -1; 
  MmsTime* timeout = NULL;
  int useIdleResource = (flags & GAD_USEIDLE) != 0;

  this->setResourceAcquisitionParameters 
       (resourceType, &timeout, voiceTimeout, &useIdleResource);
     
  handle = resourcePoolGetAvailableDevice(resourceType, timeout, flags);

  return handle;
} 


                                            // Get device object
MmsMediaDevice* HmpResourceManager::getDevice(const mmsDeviceHandle handle)
{
  // Given a device handle, returns a pointer to device object

  if  (!isValidDeviceHandle(handle)) return NULL;

  MmsMediaDeviceTable::iterator entry = mediaResourceTable.find(handle);
  if (entry == mediaResourceTable.end()) return NULL;

  MmsMediaDevice* device = entry->second;

  return device;
}  


                                            // Return resource to pool
int HmpResourceManager::releaseResource(mmsDeviceHandle handle) 
{ 
  // Releases resources for indicated device, returning it to available pool
  return this->resourcePoolAvailable(handle);
}


                                            // Get resource from pool
mmsDeviceHandle HmpResourceManager::resourcePoolGetAvailableDevice
( int resourceType, MmsTime* timeout, const int flags) 
{
  // Acquires a resource of the specified type, removing it from the available
  // resource pool if other than a conferencing device. It the requested type
  // is a voice resource, and useidle is TRUE, and none are available, we can
  // attempt to commandeer an idle voice resource, waiting up to (timeout) 
  // for a resource to become available. If requested type is a conferencing 
  // resource, the resource remains available, and if there are more than one,
  // the device having the most remaining conferencing resources is selected.

  // To do: idle pool is not currently used. replace idle pool wait with a 
  // wait on a device of the requested type entering the available pool.

  mmsDeviceHandle handle = -1;
  const int useidle = (flags & HmpResourceManager::GAD_USEIDLE) != 0;
                       
  this->resourceLock.acquire();             // Synchronize this method


  switch(resourceType)
  { 
    case MEDIA_RESOURCE_TYPE_IP:
         if  (mediaResourceTableIPAvailable.size() > 0)
              handle = mediaResourceTableIPAvailable.front();
         break;

    case MEDIA_RESOURCE_TYPE_VOICE:

         if  (useidle && (mediaResourceTableVoiceAvailable.size() ==0))
              handle = this->getAvailableVoiceResource(timeout);
         else
         if  (config->media.asrEnable)          
              handle = this->getCapableVoxDevice(flags & GAD_CSP_CAPABLE? 
                  MMSRM_DEMAND_CSP_CAPABLE: MMSRM_PREFER_NON_CSP_CAPABLE);
         else      
         if  (mediaResourceTableVoiceAvailable.size() > 0)
              handle = mediaResourceTableVoiceAvailable.front(); 
         break;

    case MEDIA_RESOURCE_TYPE_CONFERENCE: 
         handle = this->selectConferencingDevice();
         break;
  }


  if  (isValidDeviceHandle(handle))        
  {    
       MmsMediaDevice* device = this->getDevice(handle);
       if  (NULL == device)
            handle = -1;                     
       else 
       if  (resourceType != MEDIA_RESOURCE_TYPE_CONFERENCE)                                          
       {    this->resourcePoolUnavailable(handle);    
            device->setBusy();              // Remove device from available pool 
       }
  }

  this->resourceLock.release();
  return handle;
}


                                            // Move resource to/from available pool
int HmpResourceManager::resourcePoolAvailable(mmsDeviceHandle handle, int bAvailable)
{
  // Moves device in or out of available pool. If made unavailable, 
  // device->state() will remain in limbo until reset elsewhere.

  MmsMediaDevice* device = this->getDevice(handle);
  if  (NULL == device) return -1; 

  switch(device->type())
  {
    case MmsMediaDevice::IP:
         if  (bAvailable)
              mediaResourceTableIPAvailable.push_back(handle);
         else mediaResourceTableIPAvailable.remove(handle);
         break;

    case MmsMediaDevice::VOICE:
         this->voiceResourceLock.acquire();        

         if  (bAvailable)
         {
              if (this->isVoxUnavailable(handle))
              {                    
                  mediaResourceTableVoiceAvailable.push_back(handle);
                                            // Clear possible idle mapping
                  resourcePoolUnidle(handle,0);  
              }
         }                                   
         else mediaResourceTableVoiceAvailable.remove(handle);

         this->voiceResourceLock.release(); 
         break;

    case MmsMediaDevice::CONF:
         if  (bAvailable)
              mediaResourceTableConferenceAvailable.push_back(handle);
         else mediaResourceTableConferenceAvailable.remove(handle);
         break;
  }


  if  (bAvailable)
  {    
       device->setAvailable();              // Reset state of device object 
                                            // Wake thread waiting on resource
       if  (device->isVoiceDevice() && this->isWaitingForVoiceResource())
            this->signalVoiceResourceAvailable();
  }
  else device->setTransitory();

  return 0;
}


                                            // Make device unavailable
int HmpResourceManager::resourcePoolUnavailable(mmsDeviceHandle handle)
{
  return this->resourcePoolAvailable(handle, FALSE);
}


                                            // Make device unavailable and busy
int HmpResourceManager::resourcePoolBusy(mmsDeviceHandle handle)
{
  MmsMediaDevice* device = this->getDevice(handle);
  if  (NULL == device) return -1;

  this->resourcePoolUnavailable(handle);
  device->setBusy();

  return 0;
}



int HmpResourceManager::isVoxUnavailable(mmsDeviceHandle handle)
{
  const int result = this->isVoxAvailable(handle);
  return result == 0;
}



int HmpResourceManager::isVoxAvailable(mmsDeviceHandle handle)
{
  if (mediaResourceTableVoiceAvailable.size() == 0) return 0;
                           
  MmsMediaDeviceList::iterator item     
    = std::find(mediaResourceTableVoiceAvailable.begin(),
                mediaResourceTableVoiceAvailable.end(), handle);

  return item != mediaResourceTableVoiceAvailable.end();
}


                                            // Return available count for type
int HmpResourceManager::resourcePoolAvailableCount(int resourceType, int useidle)
{
  int count = 0;

  switch(resourceType)
  { 
    case MEDIA_RESOURCE_TYPE_IP:
         count = mediaResourceTableIPAvailable.size();
         break;

    case MEDIA_RESOURCE_TYPE_VOICE:
         this->voiceResourceLock.acquire();
         count = mediaResourceTableVoiceAvailable.size();
         if  (useidle)
              count += mediaResourceVoiceIdleTimes.size();
         this->voiceResourceLock.release();
         break;

    case MEDIA_RESOURCE_TYPE_CONFERENCE: 
         count = mediaResourceTableConferenceAvailable.size();
         break;
  }

  return count;
}


                                            // Return busy count for type
int HmpResourceManager::resourcePoolBusyCount(int resourceType)
{
  int count = 0;

  switch(resourceType)
  { 
    case MEDIA_RESOURCE_TYPE_IP:
         count = mediaResourceTableIP.size() 
               - mediaResourceTableIPAvailable.size();
         break;

    case MEDIA_RESOURCE_TYPE_VOICE:
         this->voiceResourceLock.acquire();
         count = mediaResourceTableVoice.size()
               - mediaResourceTableVoiceAvailable.size() 
               - mediaResourceVoiceIdleTimes.size(); 
         this->voiceResourceLock.release();
         break;

    case MEDIA_RESOURCE_TYPE_CONFERENCE: 
         count = mediaResourceTableConference.size() 
               - mediaResourceTableConferenceAvailable.size();
         break;
  }

  return count;
}
   

                                            // Return idle count for type
int HmpResourceManager::resourcePoolIdleCount(int resourceType)
{
  int count = 0;

  switch(resourceType)
  {                                         // Only voice resources  
    case MEDIA_RESOURCE_TYPE_VOICE:         // have an idle pool
         count = mediaResourceVoiceIdleTimes.size();
         break;
  }

  return count;
}



mmsDeviceHandle HmpResourceManager::selectConferencingDevice()
{
  // Selects the conferencing device with the greatest number of conferencing
  // resources. Conferencing devices differ metaphorically from other media
  // resources, since with the others, the device essentially is the resource, 
  // whereas with conferencing, the device has multiple resources.
  // Note that at this writing there exists only the one device dcbB1D1

  switch(mediaResourceTableConferenceAvailable.size())
  { case 0: return -1;
    case 1: return mediaResourceTableConferenceAvailable.front();
  }

  int maxconfresources = 0;
  MmsMediaDeviceList::iterator x = mediaResourceTableConferenceAvailable.end();
  MmsMediaDeviceList::iterator i = mediaResourceTableConferenceAvailable.begin();

  for(; i != mediaResourceTableConferenceAvailable.end(); i++)
  {
    MmsDeviceConference* device = (MmsDeviceConference*)this->getDevice(*i);
    ACE_ASSERT(device);

    int  resourcesremaining = device->resourcesRemaining();
    if  (resourcesremaining > maxconfresources)
    {
         maxconfresources = resourcesremaining;
         x = i;
    }
  }

  return x == mediaResourceTableConferenceAvailable.end()? -1: *x;
}



mmsDeviceHandle HmpResourceManager::getConferencingResource()
{
  return this->selectConferencingDevice();
}


                                            
MmsMediaDevice* HmpResourceManager::getConferencingDevice()     
{ 
  // Convenience method to retrieve the conference device
  mmsDeviceHandle handle = this->selectConferencingDevice();
  MmsMediaDevice* device = this->getDevice(handle);
  return device;
}


// - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
// Begin methods relating to waiting on and acquiring a voice resource
// - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
// Configuration file items serverParams.reassignIdleVoiceResources,
// serverParams.waitForVoiceResourceSeconds, and waitForVoiceResourceMsecs 
// determine whether this->getResource(MEDIA_RESOURCE_TYPE_VOICE) will attempt 
// to commandeer an idle voice resource (assigned to another session but not 
// currently in use); if we will wait to acquire the idle resource, and if so 
// how long. Config item serverParams.idleDeviceSelectionStrategy indicates 
// the stragegy to be used in selecting an idle resource from among multiple 
// idle resources

 
mmsDeviceHandle HmpResourceManager::getAvailableVoiceResource(MmsTime* timeout)
{
  // Wait (timeout) secs/msecs for an available or idle voice resource to 
  // become available. Timeout value TIMEOUT_BLOCK is not accepted here.  
  // If passed as such (timeout == NULL), we convert it to immediate timeout.
  // If timeout is not immediate, this method should be executed in a service 
  // thread context, since the executing thread will block until timeout
  // or device acquisition.

  mmsDeviceHandle handle = -1;
  int  result=0, wasIdle = 0;               // First check if we don't need
                                            // to wait for a voice resource
  handle = this->getVoiceResourceImmediate(&wasIdle);
  if  (isValidDeviceHandle(handle)) return handle;  
  MMSLOG((LM_DEBUG,"RESX waiting for available vox resource\n"));       
                                            // Convert timeout to absolute time
  MmsTime expirationtime = ACE_OS::gettimeofday();
  if (timeout) expirationtime += *timeout;  // If null, timeout is immediate

  // Create the wait condition. We would not want isWaitingForVoiceResource()
  // to intervene after we instantiated the condition but before we set the 
  // condition lock, so we do those operations atomically.

  this->atomicOperationLock.acquire(); 
  this->isVoiceResourceAvailable = FALSE;  
  this->voiceResourceAvailable               
      = new ACE_Condition<ACE_Thread_Mutex>(this->voiceResourceAvailableLock);
  this->voiceResourceAvailableLock.acquire(); 
  this->atomicOperationLock.release(); 

  // Wait on the condition variable isVoiceResourceAvailable to become true,
  // or timeout to expire. Note that condition.wait() releases the condition
  // mutex (permitting the condition variable to be set elsewhere), and 
  // reacquires that mutex immediately on return from wait().  
                                             
  result = voiceResourceAvailable->wait(&expirationtime); 
                                            // Timed out?
  if ((result != -1 ) && isVoiceResourceAvailable)
                                            // No: get newly-available resource
       handle = this->getVoiceResourceImmediate(&wasIdle);
                                  
  if  (isValidDeviceHandle(handle))
       MMSLOG((LM_DEBUG,"RESX vox resource %d acquired\n",handle));
  else MMSLOG((LM_DEBUG,"RESX vox resource unavailable\n"));

  // Finally discard the wait condition object. Since the existence of the
  // condition object, plus voiceResourceAvailableLock unlocked, are the 
  // indicators for signalVoiceResourceAvailable() that it should signal
  // the wait condition, we'll undo those two items atomically to ensure 
  // another thread can't signal the same condition again before we're done.
                                              
  this->atomicOperationLock.acquire();                                            
  this->voiceResourceAvailableLock.release();
  delete this->voiceResourceAvailable;        
  this->voiceResourceAvailable = NULL;       
  this->atomicOperationLock.release(); 

  return handle;
}


                                             
int HmpResourceManager::isWaitingForVoiceResource()
{ 
  // Indicates if a thread is currently blocked waiting for a voice resource

  ACE_Guard<ACE_Thread_Mutex> x(this->atomicOperationLock);
  return this->voiceResourceAvailable != NULL; 
}



int HmpResourceManager::signalVoiceResourceAvailable(int broadcast)
{
  // Set value of the condition variable that getAvailableVoiceResource()
  // is waiting on, and wake up one or all waiting threads. Invoked
  // by methods which insert resources into available or idle pools.
  // We ensure that we do not signal the same condition more than once 
  // by failing if we can't acquire the voiceResourceAvailableLock, 
  // since if another thread has the lock it means that the wait condition
  // still exists but has just expired.

  if  (!this->isWaitingForVoiceResource()) return -1;
  if  (this->voiceResourceAvailableLock.tryacquire() == -1) return -1;

  this->isVoiceResourceAvailable = TRUE;    // Condition variable
  if  (broadcast)
       voiceResourceAvailable->broadcast(); // Signal all threads
  else voiceResourceAvailable->signal();    // Signal one thread
                                            // End condition.wait():  
  this->voiceResourceAvailableLock.release();
  return 0;
}



mmsDeviceHandle HmpResourceManager::getVoiceResourceImmediate(int* wasidle)
{
  // Get an available or idle voice resource if one is immediately available 

  mmsDeviceHandle handle = -1; 

  if  (mediaResourceTableVoiceAvailable.size() 
     + mediaResourceVoiceIdleTimes.size() == 0)
       return handle;

  int  wasIdle = 0;             
  this->voiceResourceLock.acquire();        // First check if we don't need 
                                            // to wait for a resource
  if  (mediaResourceTableVoiceAvailable.size() > 0)
       handle  = mediaResourceTableVoiceAvailable.front();
  else
  if  (mediaResourceVoiceIdleTimes.size() > 0)
  {                                         // Get & unmap idle resource
       int strategy = config->serverParams.idleDeviceSelectionStrategy;

       handle  = resourcePoolGetIdleDevice(strategy, TRUE);

       wasIdle = TRUE;
       MMSLOG((LM_DEBUG,"RESX vox %d reassigned\n",handle));                                       
  }


  this->voiceResourceLock.release(); 
  if  (isBadDeviceHandle(handle)) handle = -1; 
                                        
  if  (isValidDeviceHandle(handle) & wasIdle)         
       *wasidle = wasIdle;                  // Return idle indicator to caller
       
  return handle;
}


                                            // Return handle to an idle device
mmsDeviceHandle HmpResourceManager::resourcePoolGetIdleDevice
( const int strategy, const int unidle)
{
  // Select and return a (voice) resource from the idle pool, based on a 
  // specified selection strategy. Strategies are currently limited to 
  // IDLED_MOST_RECENTLY and IDLE_LONGEST (mmsConfig.h). If removal from idle 
  // pool is requested (unidle TRUE), device state will remain in limbo 
  // until reset by caller. Caller should hold the voiceResourceLock
  // NOTE: A resource which is in media state MEDIAWAITING should not be
  // idled unless the async operation is projected to take a long time;
  // and if we were to idle such a resource, these selection strategies
  // should take into account the estimated time remaining prior to
  // reassigning the resource.

  time_t idledTime = 0;
  mmsDeviceHandle handle = -1;
  if  (this->mediaResourceVoiceIdleTimes.size() == 0) return handle;

  switch(strategy)                          
  {
    case MMS_STRATEGY_IDLED_MOST_RECENTLY:  // Select entry with greatest time
         idledTime = *this->mediaResourceVoiceIdleTimes.rbegin();
         break;

    case MMS_STRATEGY_IDLE_LONGEST:         // Select entry with lowest time
         idledTime = *this->mediaResourceVoiceIdleTimes.begin();
         break;
  }
                                            // Get resource idled at that time
  TimeToMmsDeviceMap ::iterator entry = mediaResourceTableVoiceIdle.find(idledTime);
  if  (entry != this->mediaResourceTableVoiceIdle.end())
       handle = entry->second;   
                                            // Remove resource from idle pool
  if  (isValidDeviceHandle(handle) && unidle)
       this->resourcePoolUnidle(handle, FALSE);

  return handle;
}


                                            // Set voice resource as idle
int HmpResourceManager::resourcePoolIdle(mmsDeviceHandle handle, int uselock)
{   
  // Device state is set to idle, device idled time is set as now, and an
  // ordered mapping is created from the idled time to device, such that 
  // a device may subsequently be selected based upon relative time idled.
  // If caller does not hold the voiceResourceLock, uselock should be TRUE
  MmsMediaDevice* device = this->getDevice(handle);
  if  (device && device->isVoiceDevice()); else return -1;         
  if  (uselock) 
       this->voiceResourceLock.acquire();  
                              
  device->setIdle();                                                                     
  time_t idletime = device->timeIdled();    // Create ordered mapping
                                            // from idle time to handle
  time_t idletimeused = this->createIdleTimeMapping(handle, idletime);
  if  (idletimeused  != idletime)           // Ensure keys still match
       device->timeIdled(idletimeused);

  if  (uselock) 
       this->voiceResourceLock.release(); 

  MMSLOG((LM_DEBUG,"RESX vox %d is idle\n",handle));  
                                            // If a thread is waiting for a 
  if  (this->isWaitingForVoiceResource())   // voice resource, wake it up
       this->signalVoiceResourceAvailable();
                                     
  return 0;
}


                                            // Unmap idle voice resource
int HmpResourceManager::resourcePoolUnidle(mmsDeviceHandle handle, int uselock)
{  
  // Remove any mapping from idle time to device handle. If device is indeed
  // in idle state at time of call, device state will remain in limbo until
  // reset elsewhere. 

  MmsMediaDevice* device = this->getDevice(handle);
  if  (device && device->isVoiceDevice()); else return -1;
  if  (uselock) 
       this->voiceResourceLock.acquire();

  time_t idletime = device->timeIdled();   
  if  (device->isIdle())                   // Set state to other than idle
       device->setTransitory();            // Caller must reset as necessary

  if  (idletime)
  {
       mediaResourceVoiceIdleTimes.erase(idletime);

			 TimeToMmsDeviceMap ::iterator entry = mediaResourceTableVoiceIdle.find(idletime);
			 if  (entry != this->mediaResourceTableVoiceIdle.end())
			 {
			     mediaResourceTableVoiceIdle.erase(entry);
			 }
  }

  if  (uselock) 
       this->voiceResourceLock.release(); 

  // if  (idletime) MMSLOG((LM_DEBUG,"RESX vox %d unidled\n",handle));                                       
  return 0;
}



time_t HmpResourceManager::createIdleTimeMapping
( mmsDeviceHandle handle, time_t idledtime)
{
  // Creates the ordered mapping from idle time to media device handle.
  // A <set> orders the idle times, and a <map> maps times to handles.
  // The extra logic here ensures that the time key is unique.
  // Caller must ensure that device handle refers to a voice device.
  // The actual time used for the mapping is returned.

  time_t idletime = idledtime;

  while(1)                                  // Ensure unique key
  {
    if  (mediaResourceVoiceIdleTimes.size() == 0) break;  

    if  (mediaResourceVoiceIdleTimes.find(idletime) 
      == mediaResourceVoiceIdleTimes.end()) 
         break;

    ++idletime;
  }    
                                            // Do the mapping
  mediaResourceVoiceIdleTimes.insert(idletime);
  mediaResourceTableVoiceIdle[idletime] = handle;
  return idletime;                          // Return key used
}



int HmpResourceManager::setResourceAcquisitionParameters
( const int resourceType, MmsTime** timeout, MmsTime& voiceTimeout, int* useIdle)
{
  // Invoked by this->getResource to set wait timeout, and to indicate if we
  // are to commandeer an idle resource if no unassigned resource is available 

  int  useIdleResource = 0;

  if  (resourceType == MEDIA_RESOURCE_TYPE_VOICE)
  {    // If voice request, and we are configured to move voice devices around,
       // set the timeout value for waiting on an available voice device
       useIdleResource = config->serverParams.reassignIdleVoiceResources;

       if  (useIdleResource)
       {                                    // Set relative timeout seconds
            int timeoutSecs  = config->serverParams.waitForVoiceResourceSeconds; 
            int timeoutMsecs = config->serverParams.waitForVoiceResourceMsecs;
                                            // Convert msecs to usecs
            voiceTimeout.set(timeoutSecs, timeoutMsecs * 1000);

            ACE_ASSERT(timeout && useIdle);
            *timeout = &voiceTimeout;       // Make that the timeout used
            *useIdle = useIdleResource;     // Notify caller to use idle resources
       }
  }

  return 0;
}



mmsDeviceHandle HmpResourceManager::getCapableVoxDevice(const int caps)
{
  // Retrieve voice device with requested capabilities. This is an interim
  // solution, as the lookup is sequential. We'll want to at some point include
  // this facility into the wait mechanism such that we can wait for a resource
  // with the specified capabilities; however at this writing we invoke this
  // sequential search only when voice recognition support is configured, and 
  // as that is not commonly done, we can postpone implementation of a more
  // efficient wait-enabled solution.

  // Caller is assumed to hold this->resourceLock.

  if (mediaResourceTableVoiceAvailable.size() == 0) return 0;
  MmsDeviceVoice* selectedDevice = NULL;
                           
  MmsMediaDeviceList::iterator i = mediaResourceTableVoiceAvailable.begin(); 

  for(; i != mediaResourceTableVoiceAvailable.end(); i++)
  {
      mmsDeviceHandle hvox = *i;
      MmsDeviceVoice* thisDevice = (MmsDeviceVoice*)this->getDevice(hvox);
      if (thisDevice == NULL) continue;

      if (caps == MMSRM_PREFER_NON_CSP_CAPABLE)
          if  (thisDevice->isCspDevice());
          else selectedDevice = thisDevice;
      else
      if  (caps == MMSRM_DEMAND_CSP_CAPABLE) 
           if  (thisDevice->isCspDevice()) 
                selectedDevice = thisDevice;
           else;
      else selectedDevice = thisDevice;

      if (selectedDevice) break;      
  }    

  const int selectedHandle = selectedDevice? selectedDevice->handle(): 0;

  return selectedHandle? selectedHandle: 
        (caps == MMSRM_DEMAND_CSP_CAPABLE)? 0:
         mediaResourceTableVoiceAvailable.front(); 
}

// - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
// End methods relating to waiting on and acquiring a voice resource
// - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -



// - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
// Begin methods relating to media shutdown
// - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -


void HmpResourceManager::shutdown()              
{
  // Shut down the resource manager, closing and clearing all devices
  // Caveat: this method should not be invoked from this->destructor

  MMSLOG((LM_INFO,"RESX shutdown requested\n"));

  closeAll();                               // First do an ordered shutdown

  if  (mediaResourceTable.size() == 0) return;

  MmsMediaDeviceTable::iterator i;          // Finally delete all device objects

  for(i = mediaResourceTable.begin(); i != mediaResourceTable.end(); i++)
  {
    MmsMediaDevice* device = i->second;

    if (device)       
        delete device;
  }

  mediaResourceTable.erase(mediaResourceTable.begin(),mediaResourceTable.end());
}



void HmpResourceManager::closeAll()  
{ 
  // Closes all active resources in order of resource type: IP, vox, conference
  // Note: since we may have shut down the logger as the server manager
  // shuts down, prior to shutdown of the resource manager, we've forced the 
  // device shutdown messages to the console below so that we can have a visual
  // indication of the ordered shutdown as it occurs.
   
  if  (mediaResourceTableIP.size())
       ACE_OS::printf("RESX closing %d IP devices\n",
        mediaResourceTableIP.size());
  while(mediaResourceTableIP.size() > 0)
        this->removeResource(*mediaResourceTableIP.begin());
   
  if  (mediaResourceTableVoice.size())
       ACE_OS::printf("RESX closing %d voice devices\n",
        mediaResourceTableVoice.size());
  while(mediaResourceTableVoice.size() > 0)
        this->removeResource(*mediaResourceTableVoice.begin());

  if  (mediaResourceTableConference.size())
       ACE_OS::printf("RESX closing %d conference device(s)\n",
        mediaResourceTableConference.size());
  while(mediaResourceTableConference.size() > 0)
        this->removeResource(*mediaResourceTableConference.begin());
}



int HmpResourceManager::removeResource(mmsDeviceHandle handle) 
{ 
  // Closes indicated device and removes it from resource pool

  this->resourcePoolUnavailable(handle);    // Ensure out of available pool

  MmsMediaDevice* device = this->getDevice(handle);
  if  (NULL == device) return -1; 

  switch(device->type())                    // Remove from specific type pool
  {
    case MmsMediaDevice::IP:
         mediaResourceTableIP.erase(mediaResourceTableIP.find(handle));
         break;

    case MmsMediaDevice::VOICE:
         mediaResourceTableVoice.erase(mediaResourceTableVoice.find(handle));   
         break;

    case MmsMediaDevice::CONF:
         mediaResourceTableConference.erase(mediaResourceTableConference.find(handle));   
         break;
  }
                                            // Force to console - see comments above
  // ACE_OS::printf("RESX closing device %d\n",device->handle());
  device->close();   
  return 0;
}



HmpResourceManager::~HmpResourceManager()   // Dtor
{
  // Note that this destructor may not invoke this->shutdown(). Instead,   
  // the resource manager host should invoke resource manger's shutdown(), 
  // to ensure that all resources are cleaned up.

  if (this-> conferencingResourcesTotal)
      delete conferencingResourcesTotal;

  if (this->hmp)
      this->hmp->shutdown();
}


void HmpResourceManager::destroy()
{
  if (m_instance) delete m_instance;
  m_instance = NULL;
}

// - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
// End methods relating to media shutdown
// - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -



// - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
// Begin methods relating to media loading
// - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -


HmpResourceManager* HmpResourceManager::instance()
{
   if(!m_instance) 
   {   m_instance = new HmpResourceManager;  
       m_instance->hmp = Hmp::instance();
       m_instance->config = NULL;
       m_instance->reporterQueue = NULL;
       m_instance->voiceResourceAvailable = NULL;
       m_instance->conferencingResourcesTotal = NULL;
   } 

   return m_instance;
}



int HmpResourceManager::init(MmsConfig* config)
{
  this->config = config;

  const int flags = disableLogTimestamps();

  int result  = this->getDeviceCounts();

  if (result != -1)
      result  = this->setResourceCounts();
     
  if (result != -1)
  {   
      ACE_OS::printf("RESX opening media devices ...\n");

      result = this->loadDevicePool();

      if  (result == -1) 
           ACE_OS::printf("RESX media devices could not be opened\n");
      else ACE_OS::printf("RESX media devices open\n");
  }

  restoreLogTimestamps(flags);
  return result;
}



int HmpResourceManager::setResourceCounts()
{
  // Set the actual numbers of connection and media resources  
  // available to this server deployment.

  // Post-OEM-Licensing, change the comment below -- it no longer applies:
  // The ceiling on connections is MEDIASERVER_CUSTOMER_CONCURRENCY_LICENSES
  // defined in mmsBuild.h. This is the maximum number of connections that
  // this server deployment is licensed for. MMS_MAXIMUM_AVAILABLE_RESOURCES
  // may have been specified as the value, and if so we'll calculate the
  // limit from other sources, most likely from IP resource availability.

  int  absoluteMaxConnections = MEDIASERVER_CUSTOMER_CONCURRENCY_LICENSES;
  ACE_ASSERT(absoluteMaxConnections > 0);

  // Server admin can configure for fewer connections
  int configfileMaxConnections = max(config->hmp.maxConnections, 1); 
  int configuredMaxConnections = min(configfileMaxConnections, absoluteMaxConnections); 

  // Maximum possible number of connections is ultimately dependent upon the
  // number of IP resources available, previously read from the HMP firmware
  int ipResources = m_deviceCounts.getDeviceCount(MEDIA_RESOURCE_TYPE_IP);
                                            
  // Obviously we don't need more connections than IP resources and vice-versa
  ipResources = min(ipResources, configuredMaxConnections);
  config->calculated.maxMediaResourcesIP = ipResources;

  // This is the final word on connections, which also sets session pool size
  config->calculated.maxConnections = ipResources;


  // Voice licenses can be similarly limited, however this is unlikely.
  // This number should usually come in as MMS_MAXIMUM_AVAILABLE_RESOURCES
  int  maxVoiceConnections = MEDIASERVER_CUSTOMER_VOICE_LICENSES;
  ACE_ASSERT(maxVoiceConnections > 0);
   
  // We'll take all the voice resources we can get ...
  config->calculated.maxMediaResourcesVoice    
        = min(m_deviceCounts.getDeviceCount(MEDIA_RESOURCE_TYPE_VOICE),
              maxVoiceConnections);

  // ... but we currently have no use for more voice than IP resources
  // For OEM licensing this is no longer true; however we only use these 
  // config.calculated numbers if we are bypassing licensing (internal override)
    
  config->calculated.maxMediaResourcesVoice    
        = min(config->calculated.maxMediaResourcesVoice,
              config->calculated.maxMediaResourcesIP);
  

  // Conferencing licenses can be similarly limited
  // This figure is not useful, since there is only one conference *device*
  int  maxConferenceConnections = MEDIASERVER_CUSTOMER_CONFERENCE_LICENSES;
  ACE_ASSERT(maxConferenceConnections > 0);
   
  config->calculated.maxMediaResourcesConf    
        = min(m_deviceCounts.getDeviceCount(MEDIA_RESOURCE_TYPE_CONFERENCE),
              maxConferenceConnections);
         
  resourcesInitial[MEDIA_RESOURCE_TYPE_ALL] = 0; 


  // We first do the old calculation for the number of IP, voice, and
  // conference resources to be opened. We then calculate the same limits 
  // and more using information obtained from the licensing service. 
  // If licensing is not available or if we have overridden licensing, 
  // we'll use the counts derived from the config; otherwise we will use 
  // the licensed counts.

  int  configuredIntialResourcesIP          // Get configured initial IP
     = config->hmp.maxInitialResourcesIP == MMS_USE_MAX_IP_RESOURCES?
       config->calculated.maxMediaResourcesIP:
       config->hmp.maxInitialResourcesIP;

  resourcesInitial[MEDIA_RESOURCE_TYPE_IP]  // Calculate initital IP
        = min(configuredIntialResourcesIP,
              config->calculated.maxMediaResourcesIP);

  int  configuredIntialResourcesVoice       // Get configured initial voice
     = config->hmp.maxInitialResourcesVoice == MMS_USE_MAX_VOICE_RESOURCES?
       config->calculated.maxMediaResourcesVoice:
       config->hmp.maxInitialResourcesVoice;
                                            // Calculate initial voice
  resourcesInitial[MEDIA_RESOURCE_TYPE_VOICE] 
        = min(configuredIntialResourcesVoice,
              config->calculated.maxMediaResourcesVoice); 

  int  configuredIntialResourcesConf       // Get configured initial conference
     = config->hmp.maxInitialResourcesConf == MMS_USE_MAX_CONF_RESOURCES?
       config->calculated.maxMediaResourcesConf:
       config->hmp.maxInitialResourcesConf;
                                           // Calculate initial conference
  resourcesInitial[MEDIA_RESOURCE_TYPE_CONFERENCE] 
        = min(configuredIntialResourcesConf,
              config->calculated.maxMediaResourcesConf); 


  if (this->isInternalLicensingOverridePresent()) 
  { 
    // Private config keys found indicating use configured resource limits
    this->assignOverrideResourceLimits(); 
  }
  else
  { // Get licenses for this server and set resource limits accordingly
    const int resultLRC = this->getLicensedResourceCounts();
    
    this->isLicensingServiceAvailable = resultLRC == 0;  

    if (!this->isLicensingServiceAvailable && !config->hmp.assumeSdkOnLicenseFailure)
    {
        // There was no license facility present or operable, we did not find 
        // an internal license bypass config, and we're not configured to revert to 
        // SDK mode in such a case, so we return failure to terminate this server.
        ACE_OS::printf("RESX license facility unavailable - terminating server\n");
        return -1;
    }
  }


  // Calculate thread pool size from max licensed G711.
  // Service thread pool pool size is 1/4, 3/8, 1/2, or 5/8 the session pool
  // size, but at least 25%. User specifies 1-4, as is done for utility pool.
  config->calculated.threadPoolSize = this->calculateThreadPoolSize(MmsAs::maxG711);


  // Calculate utility session slots from max licensed G711.
  // Utility session pool size is 1/8, 1/4, 3/8, or 1/2 the session pool
  // size, but at least 4.
  config->calculated.utilityPoolSize = this->calculateUtilityPoolSize(MmsAs::maxG711);

  return 0;
}



int HmpResourceManager::calculateThreadPoolSize(const int requestedMaxconnections)
{
  // st int calcMaxConnections = config->calculated.maxConnections;
  const int calcMaxConnections = MmsAs::maxG711;
  
  const int connections 
      = requestedMaxconnections < 1 || requestedMaxconnections > calcMaxConnections? 
        calcMaxConnections: requestedMaxconnections;

  // Service thread pool pool size is 1/4, 3/8, 1/2, or 5/8 the session pool
  // size, but at least 25%. User specifies 1-4, as is done for utility pool.
                                             
  int specifiedSizeFactor = config->serverParams.serviceThreadPoolSizeFactor;
  if (specifiedSizeFactor < 1) specifiedSizeFactor = 1; else
  if (specifiedSizeFactor > 4) specifiedSizeFactor = 4; 
  specifiedSizeFactor++;                    // Range is 2/8 to 5/8

  const int calcThreadPoolSize = (int)((specifiedSizeFactor / 8.0) * (double) connections); 
                                            // Pool is always at least 4 threads
  const int threadPoolSize = max(calcThreadPoolSize, 4); 
  return threadPoolSize;             
}



int HmpResourceManager::calculateUtilityPoolSize(const int requestedMaxconnections)
{
  // st int calcMaxConnections = config->calculated.maxConnections;
  const int calcMaxConnections = MmsAs::maxG711;

  const int connections 
      = requestedMaxconnections < 1 || requestedMaxconnections > calcMaxConnections? 
        calcMaxConnections: requestedMaxconnections;

  // Calculate number of utility session slots to be allocated.
  // Utility session pool size is 1/8, 1/4, 3/8, or 1/2 the session pool
  // size, but at least 4.

  int  specifiedSizeFactor = config->media.utilityPoolSizeFactor;
  if  (specifiedSizeFactor < 1) specifiedSizeFactor = 1; else
  if  (specifiedSizeFactor > 4) specifiedSizeFactor = 4; 

  const int calcPoolSize = (int)((specifiedSizeFactor / 8.0) * (double) connections); 
                                            // Pool is always at least 4 slots
  const int utilityPoolSize = max(calcPoolSize, 4);  

  return utilityPoolSize;           
}



int HmpResourceManager::getLicensedResourceCounts()   
{
  // Find licensing library and if found, read the licensed resource counts from it
  // and calculate any overage permitted. If licensing was not found, assign hard
  // SDK resource limits if so configured (we do not calculate an overage on SDK).
  // Returns 0 if OK, 1 if internal override in effect, -1 otherwise.

  static char* revsdkmask = "RESX licensing unavailable - assuming SDK per config\n";
  static char* overdmask  = "RESX using configured resources per internal override \n";
  static char* nolicmask  = "RESX licensing unavailable and no alternative configured\n";

  // Get resource limits for this HMP installation, taken from HMP license file
  int result = this->getMaxHmpResourceCounts(&hmpResourceCounts);

  result = this->readLicensingService();

  if (result == -1) 
  {   // We could not access the licensing facility   

      if (this->isInternalLicensingOverridePresent()) 
      {
          // An internal (developer) license override was configured.
          // Two separate unpublished config entries must be properly set.
          printf(overdmask);
          this->assignOverrideResourceLimits();
          result = 1;
      }
      else
      if (config->hmp.assumeSdkOnLicenseFailure)
      {   // Configured to revert to SDK licensing profile when no license server
          // is available. This config option is unpublished and is normally off.
          printf(revsdkmask);
          this->assignSdkResourceLimits();
          result = 0;
      }
      else printf(nolicmask);
 
      return result;
  }

  // Add headroom if applicable and so configured 
  const int hrPct = config->hmp.licenseHeadroomPercent;

  if (!config->hmp.disallowLicenseHeadroom || !this->isSdkLicenseProfile && hrPct > 0) 
      this->setResourceHeadroom(hrPct);

  return 0;
}



int HmpResourceManager::readLicensingService()   
{
  // Access licensing service for resource limits, or assign SDK limits
   
  LicenseInformationCUME licenseInfo; 
  memset(&licenseInfo, 0, sizeof(LicenseInformationCUME));
  licenseInfo.length = sizeof(LicenseInformationCUME);
  licenseInfo.signature = LI_SIG;

  #ifdef MMS_IS_DIALOGIC_OEM_LICENSING

  int result = this->loadLicenseManager(&licenseInfo);

  if (result != 0)
  {   // Some error contacting licensing server - revert to SDK limits     
      this->assignSdkResourceLimits();
  }
  else
  {  // Copy license limits from returned licensing server info
     this->assignLicensedResourceLimits(&licenseInfo);
  }

  #else // #ifdef MMS_IS_DIALOGIC_OEM_LICENSING

  this->assignSdkResourceLimits();

  #endif // #ifdef MMS_IS_DIALOGIC_OEM_LICENSING
       
  return result;
}



void HmpResourceManager::assignLicensedResourceLimits(void* licenseStruct)
{
  // Assign media resource limits for a license-constrained media engine
  LicenseInformationCUME licenseInfo;
  memcpy(&licenseInfo, licenseStruct, sizeof(LicenseInformationCUME));
  const int ceiling  = licenseInfo.licenseLimit, MAXCEILING = 2048;

  if  (config->hmp.disableCeiling)   
       printf("RESX ceiling disabled per config\n");
  else
  if  (ceiling < MMS_SDK_RESXS_G711 || ceiling > MAXCEILING) 
       printf("RESX no ceiling in effect\n");

  else this->applyResourceCeiling(&licenseInfo, ceiling);

  // Copy license limits from licensing server info
  MmsAs::licG711 = MmsAs::maxG711 = MmsAs::avlG711 = MmsAs::insG711 = licenseInfo.rtp;
  MmsAs::licVox  = MmsAs::maxVox  = MmsAs::avlVox  = MmsAs::insVox  = licenseInfo.voice;
  MmsAs::licG729 = MmsAs::maxG729 = MmsAs::avlG729 = MmsAs::insG729 = licenseInfo.enhancedRTP;
  MmsAs::licConf = MmsAs::maxConf = MmsAs::avlConf = MmsAs::insConf = licenseInfo.conferencing;
  MmsAs::licTTS  = MmsAs::maxTTS  = MmsAs::avlTTS  = MmsAs::insTTS  = licenseInfo.tts;
  MmsAs::licASR  = MmsAs::maxASR  = MmsAs::avlASR  = MmsAs::insASR  = licenseInfo.voiceRecognitionEnginePorts; 
  MmsAs::licCSP  = MmsAs::maxCSP  = MmsAs::avlCSP  = MmsAs::insCSP  = licenseInfo.speechIntegration;

  this->adjustResourcesLicensedToActual();
} 



void HmpResourceManager::assignSdkResourceLimits()          
{
  // Assign media resource limits for the SDK version of the media engine
  this-> isSdkLicenseProfile = TRUE;

  MmsAs::licG711 = MmsAs::maxG711 = MmsAs::avlG711 = MmsAs::insG711 = MMS_SDK_RESXS_G711;
  MmsAs::licVox  = MmsAs::maxVox  = MmsAs::avlVox  = MmsAs::insVox  = MMS_SDK_RESXS_VOX;
  MmsAs::licG729 = MmsAs::maxG729 = MmsAs::avlG729 = MmsAs::insG729 = MMS_SDK_RESXS_G729;
  MmsAs::licConf = MmsAs::maxConf = MmsAs::avlConf = MmsAs::insConf = MMS_SDK_RESXS_CONF;
  MmsAs::licTTS  = MmsAs::maxTTS  = MmsAs::avlTTS  = MmsAs::insTTS  = MMS_SDK_RESXS_TTS;
  MmsAs::licASR  = MmsAs::maxASR  = MmsAs::avlASR  = MMS_SDK_RESXS_ASR; 
  MmsAs::licCSP  = MmsAs::maxCSP  = MmsAs::avlCSP  = MMS_SDK_RESXS_CSP;
}



void HmpResourceManager::assignOverrideResourceLimits()     
{
  // Assign media resource limits for internal developers bypassing licensing.
  // This is essentially the pre-licensing-integration mode of operation.
  this->isInternalLicenseProfile = TRUE;

  const int configuredMaxG711 = config->calculated.maxConnections;

  MmsAs::licG711 = MmsAs::maxG711 = MmsAs::avlG711 = MmsAs::insG711 = configuredMaxG711;
  MmsAs::licVox  = MmsAs::maxVox  = MmsAs::avlVox  = MmsAs::insVox  = configuredMaxG711;
  MmsAs::licG729 = MmsAs::maxG729 = MmsAs::avlG729 = MmsAs::insG729 = config->media.enhancedRTP; 
  MmsAs::licConf = MmsAs::maxConf = MmsAs::avlConf = MmsAs::insConf = configuredMaxG711;
  MmsAs::licTTS  = MmsAs::maxTTS  = MmsAs::avlTTS  = MmsAs::insTTS  = 1;
  MmsAs::licASR  = MmsAs::maxASR  = MmsAs::avlASR  = MmsAs::insASR  = 0; // todo: a config entry 
  MmsAs::licCSP  = MmsAs::maxCSP  = MmsAs::avlCSP  = MmsAs::insCSP  = MmsAs::licASR;
}



void HmpResourceManager::applyResourceCeiling(void* licenseStruct, const int ceiling)
{
  // Apply the license mode-determined ceiling to available resource counts
  LicenseInformationCUME* licenseInfo = (LicenseInformationCUME*) licenseStruct;
  MmsAs::licensedResourceCeiling = ceiling;
  licenseInfo->rtp          = min(licenseInfo->rtp, ceiling); 
  licenseInfo->voice        = min(licenseInfo->voice, ceiling); 
  licenseInfo->enhancedRTP  = min(licenseInfo->enhancedRTP, ceiling); 
  licenseInfo->conferencing = min(licenseInfo->conferencing, ceiling); 
  licenseInfo->tts          = min(licenseInfo->tts, ceiling); 
  licenseInfo->voiceRecognitionEnginePorts = min(licenseInfo->voiceRecognitionEnginePorts, ceiling); 
  licenseInfo->speechIntegration = min(licenseInfo->speechIntegration, ceiling); 
}        



void HmpResourceManager::setResourceHeadroom(const int hrPct)
{
  // Add a percentage of resources to resource counts in order to prevent hard cutoff
  MmsAs::maxG711 = MmsAs::avlG711 = this->calculateResourceHeadroom(MmsAs::licG711, hrPct);
  MmsAs::maxVox  = MmsAs::avlVox  = this->calculateResourceHeadroom(MmsAs::licVox,  hrPct);
  MmsAs::maxG729 = MmsAs::avlG729 = this->calculateResourceHeadroom(MmsAs::licG729, hrPct);
  MmsAs::maxConf = MmsAs::avlConf = this->calculateResourceHeadroom(MmsAs::licConf, hrPct);
  MmsAs::maxTTS  = MmsAs::avlTTS  = this->calculateResourceHeadroom(MmsAs::licTTS,  hrPct);
  MmsAs::maxASR  = MmsAs::avlASR  = this->calculateResourceHeadroom(MmsAs::licASR,  hrPct);
  MmsAs::maxCSP  = MmsAs::avlCSP  = this->calculateResourceHeadroom(MmsAs::licCSP,  hrPct);
}



void HmpResourceManager::showLicensedLimits(const int islog)
{
  static const char* hdg  = "RESX licensed resources follow:\n";
  static const char* g711 = "RESX G711 connections  . . . . . %d\n";
  static const char* g729 = "RESX G729 low-bitrate resources  %d\n";
  static const char* vox  = "RESX voice resources . . . . . . %d\n";
  static const char* conf = "RESX conference resources  . . . %d\n";
  static const char* tts  = "RESX text-to-speech ports  . . . %d\n";
  // tic const char* asr  = "RESX speech recognition ports    %d\n";
  static const char* csp  = "RESX continuous speech resources %d\n";

  if (islog)
  {
      MMSLOG((LM_INFO, hdg));
      MMSLOG((LM_INFO, g711, MmsAs::insG711));
      MMSLOG((LM_INFO, g729, MmsAs::insG729));
      MMSLOG((LM_INFO, vox,  MmsAs::insVox));
      MMSLOG((LM_INFO, conf, MmsAs::insConf));
      MMSLOG((LM_INFO, tts,  MmsAs::avlTTS));
      // LOG((LM_INFO, asr,  MmsAs::insASR));
      MMSLOG((LM_INFO, csp,  MmsAs::insCSP));
  }
  else
  {   printf(hdg);
      printf(g711, MmsAs::insG711);
      printf(g729, MmsAs::insG729);
      printf(vox,  MmsAs::insVox);
      printf(conf, MmsAs::insConf);
      printf(tts,  MmsAs::avlTTS);
      // ntf(asr,  MmsAs::insASR);
      printf(csp,  MmsAs::insCSP);
  }
}



int HmpResourceManager::loadLicenseManager(void* pInfoStruct) 
{
  // Load the license manager dll and retrieve licensing info from it.
  // The dll is expected to be in the same directory as our executable.

  const static char* excpmsg = "RESX exception thrown by licensing facility\n"; 
  const static char* exermsg = "RESX license manager export error\n"; 
  const static char* notpmsg = "RESX license facility not present\n";
  const static char* licenok = "RESX license facility accessed\n"; 
  #define LICMGR_DLLNAME    "cuaelicmgr"   
  #define LICMGR_METHODNAME "getMediaLicenseInfo"  
  char dllpath[MAXPATHLEN];
  typedef int (*GETLICINFO)(void*);
  int result = -1;

  #ifdef MMS_WINPLATFORM                     

  result = ::GetModuleFileName(NULL, dllpath, sizeof(dllpath));
  if (result < 1) return -1;

  char* backslashpos = strrchr(dllpath, '\\');  
  if (backslashpos == NULL) return -1;

  *++backslashpos = '\0';
  strcat(dllpath, LICMGR_DLLNAME);
  strcat(dllpath, ".dll");

  ::HMODULE hDLL = ::LoadLibrary(dllpath); 

  do 
  {
    if (hDLL == NULL) 
    {   // License manager DLL not found is not necessarily an error condition.
        // It can be interpreted as an error or not by the caller.
        printf(notpmsg);
        MMSLOG((LM_INFO, notpmsg));
        break;
    }
 
    GETLICINFO funcaddr = (GETLICINFO)::GetProcAddress(hDLL, LICMGR_METHODNAME);

    if (funcaddr == NULL)
    {   // Expected method not exported from DLL
        printf(exermsg);
        MMSLOG((LM_ERROR, exermsg));
        break;
    }

    try // Invoke expected method in licensing DLL
    { 
      result = (funcaddr) (pInfoStruct);    // Success returns zero
    }
    catch(...) { printf(excpmsg); }

  } while(0);

  
  // Note: do not call FreeLibrary here. The FlexLM client (this dll) checks out
  // these licenses from the FlexLM server, and keeps them checked out until the
  // client exits (media server shutdown). The FlexLM client spawns a thread on
  // which to listen for heartbeats from the FlexLM server. If we FreeLibrary
  // here, the FlexLM server will attempt to execute code on the listener thread,
  // which would no longer exists if we freed the library, causing a crash.

  if (result == 0)
  {    
      printf(licenok); 
  }  
  else
  if (result == CUAE_UNHANDLED_EXCEPTION)
  {   
      printf(excpmsg);
      MMSLOG((LM_ERROR, excpmsg)); 
  }

  #endif // #ifdef MMS_WINPLATFORM

  return result == 0? 0: -1;
}



int HmpResourceManager::calculateResourceHeadroom
( const int resourceCount, const int headroomPct)
{
  // Add a fudge factor of (headroomPct) to resourceCount. The purpose of this
  // is to avoid cutting off resources at the license limit, but rather permit
  // exceeding the licensed limit by some presumably small number of resources. 
  double percent  = headroomPct / 100.0;
  double headroom = (double)resourceCount * percent;
  const int extraResourceCount = (int)(headroom + 0.5); // Round up
  return resourceCount + extraResourceCount;
}



int HmpResourceManager::loadDevicePool()
{ 
  // Loader for media device pool. Preloads and opens all available resources 
  // of each type, unless maxInitialResourcesIP, etc, was configured to limit 
  // initial quantity. Each is stored in the media device pools by hmp handle. 

  MmsMediaDevice* resource;
  MmsMediaDevice::OPENINFO openinfo;
  int resourcesLoaded[MMS_NUM_RESOURCE_TYPES] = {0,0,0,0};
  int board, card, ordinal=0;   

  int ipResourceLimit = 0, voxResourceLimit = 0, confResourceLimit = 0;
  const int isLicenseOverride = this->isInternalLicensingOverridePresent();

  if (isLicenseOverride)      
  {    // Use configured resource limits
      ipResourceLimit   = resourcesInitial[MEDIA_RESOURCE_TYPE_IP];
      voxResourceLimit  = resourcesInitial[MEDIA_RESOURCE_TYPE_VOICE];
      confResourceLimit = resourcesInitial[MEDIA_RESOURCE_TYPE_CONFERENCE];
  }
  else // Use licensed resource limits provided by licensing mechanism
  {   ipResourceLimit   = MmsAs::maxG711;
      voxResourceLimit  = MmsAs::maxVox;
      confResourceLimit = MmsAs::maxConf;
  }
 

  for(board=1; board <= m_deviceCounts.getBoardCount(MEDIA_RESOURCE_TYPE_IP); board++)
  { 
      if (resourcesLoaded [MEDIA_RESOURCE_TYPE_IP] >= ipResourceLimit)
          break;

      for(card=1; card <= m_deviceCounts.getCardCount(MEDIA_RESOURCE_TYPE_IP, board); card++)
      {  
        resource = new MmsDeviceIP(++ordinal, config);
        openinfo.set(board,card);
         
        mmsDeviceHandle handle = resource->open(openinfo); // Open IP (G.711) resource

        if (isBadDeviceHandle(handle))
        {    
            delete resource;
            return -1;
        } 
                                              // Track last key assigned
        currentKey[MEDIA_RESOURCE_TYPE_IP].set(openinfo.key);

                                              // Insert to resource pool
        mediaResourceTable[handle] = resource;
        mediaResourceTableIP.insert(handle);
        mediaResourceTableIPAvailable.push_back(handle);

        if (++resourcesLoaded [MEDIA_RESOURCE_TYPE_IP] >= ipResourceLimit)
          break;
    }
  }


  if (!isLicenseOverride && (MmsAs::openG711 < MmsAs::maxG711))
  {
      // Licensing facility reported n G711 licenses, but fewer than n IP resources  
      // were actually opened. Report the condition and adjust available resources.
      MMSLOG((LM_ERROR,ltwng, "IP", cdevx, MmsAs::openG711, "G711", MmsAs::maxG711));
      printf(ltwng, "IP", cdevx, MmsAs::openG711, "G711", MmsAs::maxG711);
      MmsAs::maxG711 = MmsAs::insG711 = MmsAs::avlG711 = MmsAs::openG711;
      // ALARM TODO fire alarm "cannot satisfy request from licensing for n G711 resources" 
  }
   

  ordinal = 0;
  for(board=1; board <= m_deviceCounts.getBoardCount(MEDIA_RESOURCE_TYPE_VOICE); board++)
  {
      if (resourcesLoaded [MEDIA_RESOURCE_TYPE_VOICE] >= voxResourceLimit)
          break;

      for(card=1; card <= m_deviceCounts.getCardCount(MEDIA_RESOURCE_TYPE_VOICE, board); card++)
      {  
          resource = new MmsDeviceVoice(++ordinal, config);
          openinfo.set(board,card);

          mmsDeviceHandle handle = resource->open(openinfo);
          if (isBadDeviceHandle(handle))
          {    
              delete resource;
              return -1;
          } 
                                                // Track last key assigned
          currentKey[MEDIA_RESOURCE_TYPE_VOICE].set(openinfo.key);
                                                // Insert to resource pool
          mediaResourceTable[handle] = resource;
          mediaResourceTableVoice.insert(handle);
          mediaResourceTableVoiceAvailable.push_back(handle);

          if (++resourcesLoaded [MEDIA_RESOURCE_TYPE_VOICE] >= voxResourceLimit)
            break;
      }
  }

  if (!isLicenseOverride && (MmsAs::openVox < MmsAs::maxVox))
  {
      // Licensing facility reported n voice licenses, but fewer than n voice resources  
      // were actually opened. Report the condition and adjust available resources.
      MMSLOG((LM_ERROR, ltwng, "vox", cdevx, MmsAs::openVox, "vox", MmsAs::maxVox));
      printf(ltwng, "vox", cdevx, MmsAs::openVox, "vox", MmsAs::maxVox);
      MmsAs::maxVox = MmsAs::insVox = MmsAs::avlVox = MmsAs::openVox;
      // ALARM TODO fire alarm "cannot satisfy request from licensing for n voice resources" 
  }


  ordinal = 0;
  MmsFlatMapWriter conferencingResourceCountsMap(128);
   
  for(board=1; board <= m_deviceCounts.getBoardCount(MEDIA_RESOURCE_TYPE_CONFERENCE); board++)
  {
    // Note that we wrote this code for the possibility of multiple conference devices,
    // but such a situation cannot occur with HMP, only with hardware boards. So there
    // will always be exactly one "board" and "card" and thus one pass through this loop.

    if (resourcesLoaded [MEDIA_RESOURCE_TYPE_CONFERENCE] >= confResourceLimit)
        break;

    for(card=1; card <= m_deviceCounts.getCardCount(MEDIA_RESOURCE_TYPE_CONFERENCE, board); card++)
    {   
      resource = new MmsDeviceConference(ordinal, config);
      openinfo.set(board,card);

      mmsDeviceHandle handle = resource->open(openinfo);

      if (isBadDeviceHandle(handle))
      {    
          delete resource;
          return -1;
      } 

      int  confResourcesOnBoard             // Track total conferencing resources
        = ((MmsDeviceConference*)resource)->resourcesRemaining();

      if  (confResourcesOnBoard > 0)
           conferencingResourceCountsMap.insert(handle, confResourcesOnBoard);

      if (!isLicenseOverride && (confResourcesOnBoard < MmsAs::maxConf))
      {
          // Licensing facility reported n conference licenses, but fewer than n conference 
          // resources were actually available. Report the condition and adjust available resources.
          MMSLOG((LM_ERROR, ltwng, "conf", cslots, confResourcesOnBoard, "conf", MmsAs::maxConf));
          printf(ltwng, "conf", cslots, confResourcesOnBoard, "conf", MmsAs::maxConf);
          MmsAs::maxConf = MmsAs::insConf = MmsAs::avlConf = confResourcesOnBoard;
          // ALARM TODO fire alarm "cannot satisfy request from licensing for n conference resources" 
      }
                                            // Track last key assigned
      currentKey[MEDIA_RESOURCE_TYPE_CONFERENCE].set(openinfo.key);

      mediaResourceTable[handle] = resource;// Insert to resource pool
      mediaResourceTableConference.insert(handle);
      mediaResourceTableConferenceAvailable.push_back(handle);

      if (++resourcesLoaded [MEDIA_RESOURCE_TYPE_CONFERENCE] >= confResourceLimit)
          break;
    }
  }
                                            // Commit the conf resx map
  if  (conferencingResourceCountsMap.size() > 0)
  {    
       const int mapbufsize = conferencingResourceCountsMap.length();
       this->conferencingResourcesTotal = new char[mapbufsize];
       conferencingResourceCountsMap.marshal(this->conferencingResourcesTotal);
  }
  
  return 0;
}



int HmpResourceManager::adjustResourcesLicensedToActual()
{
  // When actual HMP license is more limited than what is reported by licensing facility,
  // bring the available LBR and CSP resources down in line with available RTP, to avoid
  // the situation where for example we are showing 64 of RTP, voice, and conference,
  // but 256 of low bitrate and CSP resources (since we don't actually *open* LBR and
  // CSP, we have no way of knowing the number actually available other than by reading
  // the license).

  const int actualG729 = this->hmpResourceCounts.lobitrateResources;
  const int actualCSP  = this->hmpResourceCounts.cspResources;
  const int actualRTP  = this->hmpResourceCounts.ipResources;

  if (actualG729 < MmsAs::licG729)     // Adjust LBR to actual
  {     
      ACE_OS::printf(ltwng, "G729", cresx, actualG729, "G729", MmsAs::licG729);
      MmsAs::maxG729 = MmsAs::insG729 = MmsAs::avlG729 = actualG729;
  }

  if (MmsAs::insG729 > actualRTP)      // Adjust LBR to to RTP    
      MmsAs::maxG729 = MmsAs::insG729 = MmsAs::avlG729 = actualRTP;

  if (MmsAs::insConf > actualRTP)      // Adjust conf to to RTP    
      MmsAs::maxConf = MmsAs::insConf = MmsAs::avlConf = actualRTP;

  if (actualCSP < MmsAs::licCSP)       // Adjust CSP to actual
  {     
      ACE_OS::printf(ltwng, "CSP", cresx, actualCSP, "CSP", MmsAs::licCSP);
      MmsAs::maxCSP = MmsAs::insCSP = MmsAs::avlCSP = actualCSP;
  }

  if (MmsAs::avlASR > MmsAs::avlCSP)   // Align ASR with CSP     
      MmsAs::maxASR = MmsAs::insASR = MmsAs::avlASR = MmsAs::avlCSP;

  if (MmsAs::avlTTS > actualRTP)       // Align TTS with RTP      
      MmsAs::maxTTS = MmsAs::insTTS = MmsAs::avlTTS = actualRTP;

  return 0;
}



int HmpResourceManager::isInternalLicensingOverridePresent()  
{
  // Determine if we are operating without licensing facilities and restrictions
  int isOverridePresent  
      = config->calculated.isValidDevkey 
    && (config->hmp.internalLicensingMode != CISCO_LICENSING_MODE_NORMAL);

  return isOverridePresent;    
}



int HmpResourceManager::getDeviceCounts()
{
  // Establishes available device counts for all HMP device types.
  const static char* noipmsg = "RESX error: no IP resources\n";
  const static char* novcmsg = "RESX error: no vox/conf resources\n";

  if  (-1 == this->getDeviceCountIP())         return -1;
  if  (-1 == this->getDeviceCountVoice())      return -1;
  if  (-1 == this->getDeviceCountConference()) return -1;

  if  (0 == m_deviceCounts.getDeviceCount(MEDIA_RESOURCE_TYPE_IP))
  {
       MMSLOG((LM_ERROR,noipmsg));
       ACE_OS::printf(noipmsg);
       return -1;
  }

  if ((0 == m_deviceCounts.getDeviceCount(MEDIA_RESOURCE_TYPE_VOICE)) 
   && (0 == m_deviceCounts.getDeviceCount(MEDIA_RESOURCE_TYPE_CONFERENCE)))
  {
       MMSLOG((LM_ERROR,novcmsg));
       ACE_OS::printf(novcmsg);
       return -1;
  }

  if  (0 == m_deviceCounts.getDeviceCount(MEDIA_RESOURCE_TYPE_VOICE))
  {
       MMSLOG((LM_NOTICE, NOVOXWARNING));
       ACE_OS::printf(NOVOXWARNING);
  }
  else config->calculated.isVoiceEnabled = 1;

  if  (0 == m_deviceCounts.getDeviceCount(MEDIA_RESOURCE_TYPE_CONFERENCE))
  {
       MMSLOG((LM_NOTICE, NOCONFWARNING));
       ACE_OS::printf(NOCONFWARNING);
  }
  else config->calculated.isConferencingEnabled = 1;

  return 0;
}



int HmpResourceManager::getDeviceCountIP()
{ 
  // Fetches IP device counts from HMP firmware
  int boardCount, cardCount; 
  int result = sr_getboardcnt("ipm", &boardCount);           
  if (result == -1) return -1;  
  const static char* openerr = "RESX error during ipm_Open %s\n";

  m_deviceCounts.setBoardCount(MEDIA_RESOURCE_TYPE_IP, boardCount); 

  for (int i=1; i <= boardCount; i++)
  { 
    HMPBOARDID(deviceID,"ipm",i);
    mmsDeviceHandle handle = ipm_Open(deviceID, NULL, EV_SYNC);

    if  (isBadDeviceHandle(handle)) 
    {
         MMSLOG((LM_ERROR,openerr,deviceID));
         ACE_OS::printf(openerr,deviceID);
         return -1;
    }

    // Set board-wide RTP port base 
    int rtpPortBaseOverride = config->hmp.rtpPortBase;

    if (rtpPortBaseOverride)
    {
        IPM_PARM_INFO info;
        info.eParm = (eIPM_PARM)0x10002; // PARMBD_RTPAUDIO_PORT_BASE
        info.pvParmValue = &rtpPortBaseOverride;
        if (-1 == ipm_SetParm(handle, &info, EV_SYNC))
            MMSLOG((LM_WARNING,"RESX could not set RTP port base - using default\n"));
    }
    else MMSLOG((LM_NOTICE,"RESX no RTP port base override supplied - using default\n"));

    cardCount = ATDV_SUBDEVS(handle);
    ipm_Close(handle, 0);
    if  (cardCount == AT_FAILURE) return -1;
    
    m_deviceCounts.setCardCount(MEDIA_RESOURCE_TYPE_IP, i, cardCount);
  }
   
  return 0;
}


	
int HmpResourceManager::getDeviceCountVoice()
{ 
  // Fetches voice device counts from HMP firmware

  int boardCount, cardCount;  
  int result  = sr_getboardcnt(DEV_CLASS_VOICE, &boardCount);  
  if (result == -1) return -1; 
  const static char* openerr = "RESX error during dx_Open %s\n"; 

  m_deviceCounts.setBoardCount(MEDIA_RESOURCE_TYPE_VOICE, boardCount); 

  for (int i=1; i <= boardCount; i++)
  { 
    HMPBOARDID(deviceID,"dxxx",i);
    mmsDeviceHandle handle = dx_open(deviceID, NULL);
    if  (isBadDeviceHandle(handle))  
    {
         MMSLOG((LM_ALERT,openerr,deviceID));
         ACE_OS::printf(openerr,deviceID);
         return -1;
    }

    cardCount = ATDV_SUBDEVS(handle);
    dx_close(handle);
    if  (cardCount == AT_FAILURE) return -1;
    
    m_deviceCounts.setCardCount(MEDIA_RESOURCE_TYPE_VOICE, i, cardCount);
  }
   
  return 0;
}



int HmpResourceManager::getDeviceCountConference()
{ 
  // Fetches conferencing device counts from HMP firmware. There is no subdevice
  // metaphor here as with IP and voice resources, so we'll set that to 1.

  int moreboards = 1, moredevices, rescount;
  this->firmwareTotalLicensedConferees = 0;
  const static char* openerr = "RESX error during dcb_open %s\n";

  for(int board = 1; moreboards; board++) 
  {
    moredevices = 1;

    for(int device = 1; moredevices; device++)
    {
        HMPDEVICEID(deviceID,"dcb",board,device);

        mmsDeviceHandle handle = dcb_open(deviceID, 0);

        if  (isBadDeviceHandle(handle)) 
        {
             if  (board == 1 && device == 1)
             {    MMSLOG((LM_WARNING,openerr,deviceID));
                  ACE_OS::printf(openerr,deviceID);
             }

             if  (device == 1)
                  moreboards = 0;           // Outer loop predicate

             moredevices = 0;               // Inner loop predicate
             break; 
        }

        m_deviceCounts.setBoardCount(MEDIA_RESOURCE_TYPE_CONFERENCE, board); 
                                              
        m_deviceCounts.setCardCount (MEDIA_RESOURCE_TYPE_CONFERENCE, board, device);

        rescount = 0;                       // Get conferee count for device
        if  (-1 != dcb_dsprescount(handle, &rescount))
             this->firmwareTotalLicensedConferees += rescount;

        dcb_close(handle); 
    }
  }

  return 0;
}



int HmpResourceManager::getResourceCountConference(const mmsDeviceHandle hConf)
{
  // A conferencing device is not "the" resource, as is the case for
  // other HMP device types, but instead it hosts conferencing resources.
  // Therefore we supply this method to return the total number of
  // resources installed on the device. The currently available conference
  // resources can be had using confdevice->resourcesRemaining().

  mmsDeviceHandle handle = hConf? hConf: this->selectConferencingDevice();  
  MmsFlatMapReader map(this->conferencingResourcesMap());
  char* pResourceCount = NULL; int resourceCount = 0;

  map.find(handle, &pResourceCount);
  if  (pResourceCount != NULL)
        resourceCount  = *(int*)pResourceCount;

  return resourceCount;
}



int HmpResourceManager::getMaxHmpResourceCounts(mmsHmpRegistryResourceCounts* counts)
{
  // Determine maximum number of each type of licensable HMP resource available
  // on this HMP installation.

  // If this is a windows box, we'll look at Dialogic registry hive for the
  // string which describes all licensed resources. If this is not successful,
  // we'll get the hard-coded override from the config file. Eventually we'll
  // want to code this to get the resources on a Linux box. Another way to
  // do this may be at the time a media server license is applied, we can read
  // the license file and store the licensed resource counts in a file at a 
  // location where we know to look for it here.

  char* resxString = NULL;
  const char resxStringDelimiter = '|';

  const char* ipKey         = "RTP ";
  const char* voiceKey      = "Voice ";
  const char* lobitrateKey  = "Enhanced RTP ";
  const char* conferenceKey = "Conference ";
  const char* cspKey        = "CSP ";

  const char* resxKeys[] = { voiceKey, lobitrateKey, cspKey, conferenceKey, ipKey };

  const int NUMCOUNTS = sizeof(mmsHmpRegistryResourceCounts) / sizeof(int);

  int* pcounts[NUMCOUNTS] = { &counts->voiceResources, &counts->lobitrateResources, 
       &counts->cspResources, &counts->conferenceResources, &counts->ipResources };

  #ifdef MMS_WINPLATFORM                    // If windows ...

  HKEY  hkey;                               // ... try to get licensed resources
  DWORD dwResult, dwDataSize=0, dwDataType; // string from registry
                                             
  dwResult = RegOpenKeyEx(HKEY_LOCAL_MACHINE, 
             MMS_HMP_LICENSED_RESOURCES_LOC, 0, KEY_ALL_ACCESS, &hkey);
                                            
  dwResult = RegQueryValueEx(hkey, MMS_HMP_LICENSED_RESOURCES_SUBKEY, 
             NULL, NULL, NULL, &dwDataSize);
 
  if (dwDataSize)
  {
      resxString = new char[dwDataSize];

      dwResult = RegQueryValueEx(hkey, MMS_HMP_LICENSED_RESOURCES_SUBKEY, 
                 NULL, &dwDataType, (BYTE*)resxString, &dwDataSize);
  }

  RegCloseKey(hkey);

  #endif // #ifdef MMS_WINPLATFORM


  if (resxString)                            
  {   // Resource string will look something like the following:
      // Voice 64|Enhanced RTP 64|Fax 64|CSP 64|Conference 64|RTP 64 

      try {  
      // The try/catch is lame, but in case Dialogic throws us a curveball
      // in the way of data we haven't anticipated, best to avoid a crash.    

      // For each resource count we are interested in ...
      for(int i = 0; i < NUMCOUNTS; i++)
      {
          // Find this resource name in the registry string
          char *thiskey = (char*)resxKeys[i];
          char *p = strstr(resxString, thiskey);
          if (!p) continue;

          // If delimiter is present, terminate string at that point
          char* q = strchr(p, resxStringDelimiter), *nullterm = 0;
          
          if  (q) 
               nullterm = q;                                          
          else                    // Last item -- either no delimiter
          {    q = p + strlen(p); // or trailing spaces
               char* r = p + strlen(thiskey);
               while((q > r) && !isdigit(*q)) q--;             
               if (isdigit(*q))
                   nullterm = q + 1;
          }

          if (nullterm) *nullterm = '\0';   // Temporarily terminate

          // String now looks something like "Enhanced RTP 64" 
          // Back up to resource count digits and get the digits  
          q = strrchr(p, ' ');
          
          if (q) // Set the max count for this resource
            *(pcounts[i]) = atoi(++q);

          // Undo the null terminator we set above
          if (nullterm) *nullterm = resxStringDelimiter;           
      }

      } catch(...) { }

      delete[] resxString;
  }  

  return 0;
}    


// - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
// End methods relating to media loading
// - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

