#include "StdAfx.h"
#include <iostream>
#include <minmax.h>
#include "mmsConfig.h"
#include "mmsMediaResourceMgr.h"
#include "mmsDeviceConference.h"
#include "mmsTask.h"

MmsConfig* config = NULL;
HmpResourceManager* resmgr = NULL;
int configMax[4]; 
char* rname[] ={"ALL","IP","VOICE","CONF"};
const int CONFEREE_LIMIT = 0;               // Limit for conf test; zero == max
const int MAXWAITSECONDS = 5;               // Max wait for a voice resource




// - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
// Resource acquisition and release
// - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

int testGetAndReleaseAllDevice(const int devicetype, unsigned int options)
{
  // Obtains all resources for specified resource type, up to point of error.
  // Then returns them all to resource pool. Note the resource manager will
  // limit available resources based on configuration info. So while an HMP
  // license may be 64/64, if the config limits connections to 16, we'll only
  // see 16 IP and vox resources here. 
   
  char* resname = rname[devicetype];

  int  devicecount = resmgr->deviceInventory().getDeviceCount(devicetype);
  if  (devicecount < 1)
  {
       MMSLOG((LM_INFO,"MAIN resource counts unavailable for %s\n",resname));  
       return -1;
  }

  if  (devicecount != configMax[devicetype])
       MMSLOG((LM_INFO,"MAIN available %s resources are limited by config to %d\n",
               resname, configMax[devicetype]));

  mmsDeviceHandle* handles = new mmsDeviceHandle[devicecount];
  memset(handles,0,devicecount * sizeof(mmsDeviceHandle*));
  int devices = 0, result = -1, released =0;

  while(1)
  {
    mmsDeviceHandle handle = resmgr->getResource(devicetype);

    if  (handle == -1)
    {
         MMSLOG((LM_INFO,"MAIN %d %s successfuly obtained\n",devices,resname));
         if  (devicecount > 0) result = 0;
         break;
    }

    if  (devices == devicecount)
    {
      MMSLOG((LM_INFO,"MAIN %s devices exceeds devicecount %d\n",resname,devicecount));  
      break;
    }

    handles[devices++] = handle; 
    
    // A conferencing device is not a resource; the relation is one to many.
    // Therefore the getResource() metaphor does not exhaust devices for
    // conferencing devices. We may wish to modify the test in the future, but
    // for the present we'll stop after looking at one conference device.
    // getResource() is actually a misnomer for a conferencing device, since
    // only joining or adding a monitor reduce resources. 
 
    if  (devicetype == MmsMediaDevice::CONF) 
    {    MMSLOG((LM_INFO,"MAIN %d %s successfuly obtained\n",devices,resname));
         result = 0;
         break;
    }

  } // while(1)

    // Again, the conferencing device is not a conferencing resource, so we do
    // not have to release it. These tests should be rewritten so that we do 
    // not need to hack the logic like this.

  if  (devicetype != MmsMediaDevice::CONF)
  for(int j = 0; j < devices; j++)
  {
     mmsDeviceHandle handle = handles[j];
     int  result = resmgr->releaseResource(handle);
     if  (result == -1)
          MMSLOG((LM_INFO,"MAIN could not releaseResource %s %d\n",resname,j));
     else released++; 
  }

  MMSLOG((LM_INFO,"MAIN %d %s successfuly released\n",devices,resname));
  delete[] handles;
  return result;
}



int testGetAndReleaseAllAll(int ntimes=1, unsigned int options=0)
{
  // Obtains and releases all resources of all types, ntimes in succession.
  // This test ensures that we can exhaust resources, return them to the
  // available pool, and then acquire them all again.
  // NOTE that this test acquires devices until failure, not merely until
  // device count is reached. Therefore for voice resource this will also
  // test the wait for voice resource timeout, if config file indicates 
  // should wait for a resource to become available -- config entries are:
  // Server.reassignIdleVoiceResources   
  // Server.waitForVoiceResourceSeconds           
  // Server.waitForVoiceResourceMsecs 
  // If reassignIdleVoiceResources = 1, we will wait for a resource.
  // waitForVoiceResourceSeconds and waitForVoiceResourceMsecs together
  // specify the wait timeout -- if both are zero, the wait times out
  // immediately. Obviously once we have acquired all voice resources in 
  // this test, no resource will become available, so this will only test
  // that the wait times out corrrectly after the configured secs/msecs.
  // The multithreaded testWaitAndAcquireIdleVoiceResource will test the
  // wait for voice resource feature more fully.

  int  result, finalresult;
  int  reassignIdleVoiceResources  = config->serverParams.reassignIdleVoiceResources;
  int  waitForVoiceResourceSeconds = config->serverParams.waitForVoiceResourceSeconds;          
  int  waitForVoiceResourceMsecs   = config->serverParams.waitForVoiceResourceMsecs;  
  if  (reassignIdleVoiceResources && 
      (waitForVoiceResourceSeconds + waitForVoiceResourceMsecs))        
  {
       MMSLOG((LM_INFO,"MAIN also testing voice resource wait of (%d,%d)\n", 
               waitForVoiceResourceSeconds,waitForVoiceResourceMsecs ));
  }

  for(int i=0; i < ntimes; i++)
  {
    result = testGetAndReleaseAllDevice(MmsMediaDevice::IP, options);
    finalresult = result;

    result = testGetAndReleaseAllDevice(MmsMediaDevice::VOICE, options);
    if  (result == -1) finalresult = -1; 

    result = testGetAndReleaseAllDevice(MmsMediaDevice::CONF, options); 
    if  (result == -1) finalresult = -1;
  }
      
                                            // Verify available pool correct
  for(i=MmsMediaDeviceInventory::IP; i <= MmsMediaDevice::CONF; i++)
  {
    int  availablePoolCount = resmgr->resourcePoolAvailableCount(i);
    int  expectedCount 
            = min(resmgr->deviceInventory().getDeviceCount(i),
                  configMax[i]);

    if ((availablePoolCount != expectedCount)
     || (resmgr->resourcePoolBusyCount(i) != 0))
    {
      MMSLOG((LM_INFO,"MAIN %s available pool out of sync\n", rname[i]));
      finalresult = -1;
    }
  }
                                            
  if ((resmgr->resourcePoolIdleCount(MmsMediaDevice::VOICE) != 0)
   || (resmgr->idleTimesCount()   != 0)
   || (resmgr->idleMappingCount() != 0))
  {
       MMSLOG((LM_INFO,"MAIN idle voice resource pool out of sync\n"));
       finalresult = -1;
  }

  return finalresult;
}



int getResources(int resourceType, int* handles, int numhandles)
{
  // We are less rigorous here since we've done this already in a prior test

  memset(handles,0,numhandles * sizeof(mmsDeviceHandle*));

  for(int i=0; i < numhandles; i++)
  {
    mmsDeviceHandle handle = resmgr->getResource(resourceType);
    if  (handle == -1)  break;
    handles[i] = handle;
  } 

  return i; 
}  


int releaseResources(int* handles, int numhandles)
{
  // We are less rigorous here since we've done this already in a prior test
  int finalresult=0;

  for(int i=0; i < numhandles; i++)
  {
    int  result = resmgr->releaseResource(handles[i]);
    if  (result < finalresult) finalresult = result;
  } 

  return finalresult; 
}



// - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
// Voice connections
// - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

int testAllVoiceIpConnections()
{
  int  result, finalresult=0, ipdevicecount=0, voxdevicecount=0, commoncount=0;
  mmsDeviceHandle* iphandles=NULL, *voxhandles=NULL;

  do 
  {                                         // Acquire all IP and voice resources
    ipdevicecount = resmgr->deviceInventory().getDeviceCount(MmsMediaDevice::IP);
    if  (ipdevicecount < 1) { finalresult = -1; break; }

    iphandles = new mmsDeviceHandle[ipdevicecount];
    result = getResources(MmsMediaDevice::IP, iphandles, ipdevicecount);
    if  (result == -1) { finalresult = -1; break; }

    voxdevicecount = resmgr->deviceInventory().getDeviceCount(MmsMediaDevice::VOICE);
    if  (voxdevicecount < 1) { finalresult = -1; break; }

    voxhandles = new mmsDeviceHandle[voxdevicecount];
    result = getResources(MmsMediaDevice::VOICE, voxhandles, voxdevicecount);
    if  (result == -1) { finalresult = -1; break; }


    commoncount = min(ipdevicecount, voxdevicecount);
    commoncount = min(commoncount, configMax[MmsMediaDevice::IP]);
    commoncount = min(commoncount, configMax[MmsMediaDevice::VOICE]);

    for(int i=0; i < commoncount; i++)      // For each IP/voice pair loaded ...
    {
      mmsDeviceHandle hIP  = iphandles[i];
      mmsDeviceHandle hVox = voxhandles[i];
      MmsMediaDevice* ip   = resmgr->getDevice(hIP);
      MmsMediaDevice* vox  = resmgr->getDevice(hVox);
      if  (ip == NULL || vox == NULL) { finalresult = -1; break; }
                                            // ... listen on each others' timeslot
      finalresult = vox->busConnect(ip, MmsMediaDevice::FULLDUPLEX);    

      if  (finalresult == -1)
           MMSLOG((LM_INFO,"MAIN unsuccessful mutual listen for vox/ip pair %d\n",i));
      else                                  // ... and immediately disconnect
      {    MMSLOG((LM_INFO,"MAIN successful mutual listen for vox/ip pair %d\n",i)); 
           finalresult  = vox->busDisconnect(ip, MmsMediaDevice::FULLDUPLEX);
           if  (finalresult == -1)   
                MMSLOG((LM_INFO,"MAIN unsuccessful mutual unlisten for vox/ip pair %d\n",i));
      }
    }

  } while(0);


  if  (iphandles) 
  {    result = releaseResources(iphandles, configMax[MmsMediaDevice::IP]);
       if  (result < finalresult) finalresult = result;
       delete[] iphandles;
  }
  if  (voxhandles) 
  {    result = releaseResources(voxhandles, configMax[MmsMediaDevice::VOICE]);
       if  (result < finalresult) finalresult = result;
       delete[] voxhandles;
  }

  return finalresult;
}


// - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
// Conferencing
// - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

int testAllConferenceIpConnections(int ntimes)
{
  int  result, i, finalresult=0, ipdevicecount=0, confdevicecount=0;
  int  obtainedCountIP=0, conferenceID=0;
  mmsDeviceHandle* iphandles=NULL, *confhandles=NULL;
  MmsDeviceConference* deviceConf = NULL;
                                            // Acquire all IP and conf resources
  ipdevicecount = resmgr->deviceInventory().getDeviceCount(MmsMediaDevice::IP);
  if  (ipdevicecount < 1) return -1;

  iphandles = new mmsDeviceHandle[ipdevicecount];
  obtainedCountIP = getResources(MmsMediaDevice::IP, iphandles, ipdevicecount);
  if  (obtainedCountIP == -1) return -1;

  confdevicecount = resmgr->deviceInventory().getDeviceCount(MmsMediaDevice::CONF);
  if  (confdevicecount < 1) return -1;

  confhandles = new mmsDeviceHandle[confdevicecount];
  result = getResources(MmsMediaDevice::CONF, confhandles, confdevicecount);
  if  (result == -1) return -1;


  for(int j=0; j < ntimes; j++) 
  {       
    deviceConf = (MmsDeviceConference*)resmgr->getDevice(confhandles[0]);
    if  (NULL == deviceConf) break; 
    MmsMediaDevice* deviceIP = resmgr->getDevice(iphandles[0]); 
    if  (NULL == deviceIP)   break; 
                                            // Create conference of 1
    result = deviceConf->create(&conferenceID, deviceIP);  
    if  (result == -1)
    {    MMSLOG((LM_INFO,"MAIN conf->create() failed\n"));                              
         break;
    }

    int conferees = 1;
    const int confereeLimit = CONFEREE_LIMIT? CONFEREE_LIMIT: ipdevicecount;

    for(i=1; i < confereeLimit; i++)        // Add conferees to conference
    {
       int  confResourcesLeft = deviceConf->resourcesRemaining();
       if  (confResourcesLeft < 1) break;

       deviceIP = resmgr->getDevice(iphandles[i]);
       if  (NULL == deviceIP) break; 

       result = deviceConf->join(conferenceID, deviceIP);  
       if  (result == -1)
       {    MMSLOG((LM_INFO,"MAIN conf->join() failed\n"));                              
            break;
       }

       conferees++;
    }

    MMSLOG((LM_INFO,"MAIN created conference (ID %d) parties %d\n",conferenceID,conferees));

    for(i = conferees; i; i--)              // Remove conferees from conference
    {
       deviceIP = resmgr->getDevice(iphandles[i-1]);
       if  (NULL == deviceIP) break; 

       result = deviceConf->leave(conferenceID, deviceIP);  
       if  (result == -1)
       {    MMSLOG((LM_INFO,"MAIN conf->leave() failed\n"));                              
            break;
       }
    }

    if  (i == 0)
         MMSLOG((LM_INFO,"MAIN removed all participants from conference %d\n",
                 conferenceID));

    // Note that if we have removed the last conferee above, the following
    // teardown does nothing, since resource manager will have already done
    // it when final conferee was detected. If on the other hand the above
    // code has been changed to not leave() all conferees, the the teardown
    // will remove all remaining conferees and destroy the conference.

    result = deviceConf->teardown(conferenceID);
    if  (result == -1) MMSLOG((LM_INFO,"MAIN conf->teardown() failed\n"));
  }    // for(int j ...


  if  (iphandles) 
  {    result = releaseResources(iphandles, obtainedCountIP);
       if  (result < finalresult) finalresult = result;
       delete[] iphandles;
  }

  if  (confhandles)  
  {    
       delete[] confhandles;
  }

  return finalresult;
}



int testMultipleConferences(const int numConferences)
{
  const int MAXCONFERENCES = 16;
  const int NUMCONFERENCES = numConferences < 1 || numConferences > MAXCONFERENCES?
            MAXCONFERENCES:  numConferences;
  int  result,i,finalresult=-1,ipdevicecount=0,confdevicecount=0,obtainedCountIP=0;
  int* conferenceID = new int[NUMCONFERENCES]; 
  memset(conferenceID, 0, sizeof(int) * NUMCONFERENCES);
  mmsDeviceHandle* iphandles=NULL, *confhandles=NULL;
  MmsDeviceConference* deviceConf = NULL;
  mmsDeviceHandle* conferences[MAXCONFERENCES];
  memset(conferences, 0, sizeof(mmsDeviceHandle) * MAXCONFERENCES);

  do {         // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - 

  confdevicecount = resmgr->deviceInventory().getDeviceCount(MmsMediaDevice::CONF);
  if  (confdevicecount < 1) break;

  confhandles = new mmsDeviceHandle[confdevicecount];
  result = getResources(MmsMediaDevice::CONF, confhandles, confdevicecount);
  if  (result == -1) break;

  deviceConf = (MmsDeviceConference*)resmgr->getDevice(confhandles[0]);
  if  (NULL == deviceConf) break; 
                                            // Acquire all IP resources
  ipdevicecount = resmgr->deviceInventory().getDeviceCount(MmsMediaDevice::IP);
  if  (ipdevicecount < 1) break;

  iphandles = new mmsDeviceHandle[ipdevicecount];
  obtainedCountIP = getResources(MmsMediaDevice::IP, iphandles, ipdevicecount);
  if  (obtainedCountIP == -1) break;

  int maxPartiesPerConference 
   = (obtainedCountIP / NUMCONFERENCES) + ((obtainedCountIP % NUMCONFERENCES) != 0);
  
  for(i=0; i < NUMCONFERENCES; i++)
      conferences[i] = new mmsDeviceHandle[maxPartiesPerConference];

  int* currentID = &conferenceID[0];
  int  currentParty = 0;
                                            // For each requested conference ...
  for(i=0; i < NUMCONFERENCES; i++, currentID++)         
  {
    mmsDeviceHandle party    = iphandles[currentParty++];  
    MmsMediaDevice* deviceIP = resmgr->getDevice(party); 
    ACE_ASSERT(deviceIP);
                                            // ... create conference of one
    result = deviceConf->create(currentID, deviceIP);  
    if  (result == -1)
    {    MMSLOG((LM_INFO,"MAIN conf->create() failed\n"));                              
         break;
    }
    else MMSLOG((LM_INFO,"MAIN created conference ID=%d\n", *currentID));
                                            // Register first party
    mmsDeviceHandle* conference = conferences[i];
    conference[0] = party; 
  }

  if  (result == -1) break;


  while(currentParty < obtainedCountIP)     // While more IPs ...
  {
    mmsDeviceHandle party = iphandles[currentParty]; 
                                            // Alternate conferences
    int  whichConference  = (currentParty) % NUMCONFERENCES; 

    MmsMediaDevice* deviceIP = resmgr->getDevice(party); 
    ACE_ASSERT(deviceIP);
                                            // Add IP to a conference
    result = deviceConf->join(conferenceID[whichConference], deviceIP); 
    if  (result == -1)
    {    MMSLOG((LM_INFO,"MAIN conference resources exhausted after %d\n",
                 currentParty)); 
         result = 0;                            
         break;
    }
    else MMSLOG((LM_INFO,"MAIN IP %d joins conference %d\n",
                 party, conferenceID[whichConference])); 
                                            // Register this party
    mmsDeviceHandle* conference = conferences[whichConference];
    int whichParty = currentParty / NUMCONFERENCES;
    conference[whichParty] = party;

    currentParty++;
  }

                                            // Remove parties from conferences
  for(i = maxPartiesPerConference-1; i>=0; i--)  
  {
    for(int j = 0; j < NUMCONFERENCES; j++)
    {
      mmsDeviceHandle* conference = conferences[j];
     
      mmsDeviceHandle  party = conference[i];
      if  (party <= 0) continue;

      MmsMediaDevice* deviceIP = resmgr->getDevice(party); 
      ACE_ASSERT(deviceIP);

      result = deviceConf->leave(conferenceID[j], deviceIP);  
      if  (result == -1)
           MMSLOG((LM_INFO,"MAIN conf->leave() failed for conf %d IP %d\n",                             
                   conferenceID[j], party)); 
      else MMSLOG((LM_INFO,"MAIN IP %d leaves conference %d\n",                             
                   party, conferenceID[j])); 
    }
  }

  if  (result == 0)
       MMSLOG((LM_INFO,"MAIN removed all participants from %d conferences\n",
               NUMCONFERENCES));

  for(i = 0; i < NUMCONFERENCES; i++)
  {
    // Note that if we have removed the last conferee above, the following
    // teardown does nothing, since resource manager will have already done
    // it when final conferee was detected. If on the other hand the above
    // code has been changed to not leave() all conferees, the the teardown
    // will remove all remaining conferees and destroy the conference.
    result = deviceConf->teardown(conferenceID[i]);
    if  (result == -1) 
         MMSLOG((LM_INFO,"MAIN conf->teardown() failed for %d\n",
                 conferenceID[i]));
  }
 
  if  (result == 0) finalresult = 0;

  } while(0);  // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -


  if  (conferenceID) delete[] conferenceID;

  if  (iphandles) 
  {    result = releaseResources(iphandles, obtainedCountIP);
       if  (result < finalresult) finalresult = result;
       delete[] iphandles;
  }

  if  (confhandles)  
  {    
       delete[] confhandles;
  }

  for(i=0; i < NUMCONFERENCES; i++)
      if  (conferences[i]) delete[] conferences[i];

  return finalresult;
}



int testMultipleConferences(const int numConferences, int ntimes)
{
  int  result = 0;
  if  (ntimes < 1) ntimes = 1;

  for(int i=0; i < ntimes; i++)
  {
     result = testMultipleConferences(numConferences); 
     if  (result == -1) break;
  } 

  return result;   
}



int testMaxConferences()
{
  // Note that as of HMP 1.1, attempt to create more conferences than the
  // 16 simultaneous conferences supported by HMP, hoses HMP conferencing
  // state, requiring a restart of HMP. Resource manager is supposed to be
  // aware of the limit and not attempt to exceed it, so we test that here.

  int  result,i,finalresult=-1,ipdevicecount=0,confdevicecount=0,obtainedCountIP=0;
  int* conferenceID = NULL, NUMCONFERENCES = 0; 
  mmsDeviceHandle* iphandles=NULL, confhandle = NULL;
  MmsDeviceConference* deviceConf = NULL;

  do {         // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - 

  confdevicecount = 1;

  result = getResources(MmsMediaDevice::CONF, &confhandle, confdevicecount);
  if  (result == -1) break;

  deviceConf = (MmsDeviceConference*)resmgr->getDevice(confhandle);
  if  (NULL == deviceConf) break; 
                                            // Acquire all IP resources
  ipdevicecount = resmgr->deviceInventory().getDeviceCount(MmsMediaDevice::IP);
  if  (ipdevicecount < 1) break;

  iphandles = new mmsDeviceHandle[ipdevicecount];
  obtainedCountIP = getResources(MmsMediaDevice::IP, iphandles, ipdevicecount);
  if  (obtainedCountIP == -1) break;

  NUMCONFERENCES = ipdevicecount;           // Try same # of conferences
  conferenceID = new int[NUMCONFERENCES];   // although HMP max is 16
  memset(conferenceID, 0, sizeof(int) * NUMCONFERENCES);

  int* currentID = &conferenceID[0];

                                            // For each requested conference ...
  for(i=0; i < NUMCONFERENCES; i++, currentID++)         
  {
    mmsDeviceHandle party    = iphandles[i];  
    MmsMediaDevice* deviceIP = resmgr->getDevice(party); 
    ACE_ASSERT(deviceIP);
                                            // ... create conference of one
    result = deviceConf->create(currentID, deviceIP);  
    if  (result == -1)
    {    MMSLOG((LM_INFO,"MAIN conf->create() failed\n"));                              
         break;
    }
    else MMSLOG((LM_INFO,"MAIN created conference ID=%d\n", *currentID));
  }

  if  (result == -1)
       if  (i < NUMCONFERENCES)             // We reached conference max
            NUMCONFERENCES = i;             // so set count to that number
       else break;                          // HMP dcb_estconf error
                                            // Remove parties from conferences
  for(i=0; i < NUMCONFERENCES; i++)         
  {
    mmsDeviceHandle iphandle = iphandles[i];
    if  (iphandle <= 0) continue;

    MmsMediaDevice* deviceIP = resmgr->getDevice(iphandle); 
    ACE_ASSERT(deviceIP);  

    int confID = conferenceID[i];

    result = deviceConf->leave(confID, deviceIP);  
    if  (result == -1)
         MMSLOG((LM_INFO,"MAIN conf->leave() failed for conf %d IP %d\n", confID, iphandle)); 
    else MMSLOG((LM_INFO,"MAIN IP %d leaves conference %d\n", iphandle, confID)); 
  }

  if  (result == 0)
  {    MMSLOG((LM_INFO,"MAIN removed all participants from %d conferences\n", NUMCONFERENCES));
       finalresult = 0;
  }

  } while(0);  // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -


  if  (conferenceID) delete[] conferenceID;

  if  (iphandles) 
  {    result = releaseResources(iphandles, obtainedCountIP);
       if  (result < finalresult) finalresult = result;
       delete[] iphandles;
  }

  return finalresult;
}





// - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
// Idle resource appropriation
// - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

                                            // Waiter thread class
class VoiceResourceWaiter: public MmsBasicTask
{ 
  public:
  int  handleMessage(MmsMsg*);               
  VoiceResourceWaiter(MmsTask::InitialParams* p): MmsBasicTask(p) { }
  virtual ~VoiceResourceWaiter() { }
  void onInitTask(MmsMsg* msg); 
};


                                            // Waiter thread message handler
int VoiceResourceWaiter::handleMessage(MmsMsg* msg)
{ 
  switch(msg->type())
  { 
    case MMSM_INITTASK:
         onInitTask(msg);
         break; 
    
    case MMSM_QUIT:
         MMSDEBUG((LM_INFO,"%s thread exit\n",taskName));                                            
         break;

    default: return 0;
  } 

  return 1;
}


   
void VoiceResourceWaiter::onInitTask(MmsMsg* msg)
{
  // Waiter thread wait for resource, acquire, and release logic. Waits
  // to acquire a voice resource. Presumably the main thread has acquired 
  // all available voice resources, forcing us to wait for one. We expect 
  // the main thread to sleep a couple of seconds and then idle one of 
  // the voice resources it holds. The appearance of resource in the
  // idle pool should cause the resource mamager to signal our waiting
  // thread, terminating our wait and returning to us the handle of the
  // newly idled voice resource. Presumably the main thread will then
  // itself wait for a voice resource. We will sleep a couple of seconds
  // and release the resource we just acquired. This should in turn permit
  // the main thread wait to terminate, allowing the main thread to
  // reacquire that voice resource. 
  MMSLOG((LM_INFO,"%s waiting up to %d secs for a voice resource ...\n",
          taskName,MAXWAITSECONDS));
                                            // Wait for voice resource
  mmsDeviceHandle handle = resmgr->getResource(MmsMediaDevice::VOICE);

  if  (handle == -1)
  {                                         // Wait timed out
       MMSLOG((LM_INFO,"%s getResource(VOICE) timed out\n",taskName)); 
       MMSLOG((LM_INFO,"%s idle voice resource acquisition test failure\n",taskName)); 
  }        
  else
  {                                         // Got the resource
       MMSLOG((LM_INFO,"%s successfully usurped idle voice resource\n",taskName));
       MmsMediaDevice* voiceDevice = resmgr->getDevice(handle);
       if  (resmgr->resourcePoolBusy(handle) == -1)
            MMSLOG((LM_INFO,"%s error during resourcePoolBusy\n",taskName));
       mmsSleep(4); 
       MMSLOG((LM_INFO,"%s returning voice resource to available pool\n",taskName));
       resmgr->releaseResource(handle);
  } 
}



int testWaitAndAcquireIdleVoiceResource()
{
  // Tests a thread's ability to wait for an idle voice resource to come
  // available, acquire the resource, and return the resource while the
  // main thread is waiting for the resource to again become available.

  // First acquire all the voice resources so none are available
  int  voxdevicecount = resmgr->deviceInventory().getDeviceCount(MmsMediaDevice::VOICE);
  if  (voxdevicecount < 1) return -1;
  int* voxhandles = new mmsDeviceHandle[voxdevicecount];
  int  result = getResources(MmsMediaDevice::VOICE, voxhandles, voxdevicecount);
  if  (result == -1) { delete voxhandles; return -1; }

  // Get current config settings
  int olduseidle = config->serverParams.reassignIdleVoiceResources;
  int oldsec     = config->serverParams.waitForVoiceResourceSeconds;

  // Configure to wait up to MAXWAITSECONDS seconds for a voice resource
  config->serverParams.reassignIdleVoiceResources  = 1;
  config->serverParams.waitForVoiceResourceSeconds = MAXWAITSECONDS;

  // Launch the thread which will wait for a resource and acquire the resource
  // once we idle the resource.
  MmsTask::InitialParams params(0,0,0);
  strcpy(params.taskName,"WAIT");
  VoiceResourceWaiter* waiter = new VoiceResourceWaiter(&params);
  MMSLOG((LM_INFO,"MAIN launching waiter thread\n"));
  waiter->start();
 
  // The waiter thread should now be waiting for a voice resource to become
  // available. We'll wait a couple of seconds and idle one of the resources
  mmsSleep(2);

  MMSLOG((LM_INFO,"MAIN idling a voice resource\n"));
  if  (resmgr->resourcePoolIdle(voxhandles[0]) == -1)
       MMSLOG((LM_INFO,"MAIN error during resourcePoolIdle\n"));

  mmsSleep(2);

  // Wait for waiter thread to release the voice resource 
  MMSLOG((LM_INFO,"MAIN waiting to reacquire the voice resource\n"));
  mmsDeviceHandle handle = resmgr->getResource(MmsMediaDevice::VOICE);

  if  (handle == -1)
  {
       MMSLOG((LM_INFO,"MAIN getResource(VOICE) timed out\n")); 
       MMSLOG((LM_INFO,"MAIN voice resource reacquisition failure\n"));
       result = -1; 
  }        
  else
  {
       MMSLOG((LM_INFO,"MAIN successfully reacquired voice resource\n"));
       MmsMediaDevice* voiceDevice = resmgr->getDevice(handle);
       result = 0;
  } 
 
                                            // Shut down waiter thread
  MMSLOG((LM_INFO,"MAIN cancelling waiter thread ...\n"));
  waiter->postMessage(MMSM_QUIT);
  mmsSleep(1); 

                                            // Restore config settings
  config->serverParams.reassignIdleVoiceResources  = olduseidle;
  config->serverParams.waitForVoiceResourceSeconds = oldsec;

                                            // Verify resource pool state
  if ((resmgr->resourcePoolIdleCount(MmsMediaDevice::VOICE) != 0)
   || (resmgr->idleTimesCount()   != 0)
   || (resmgr->idleMappingCount() != 0))
  {
       MMSLOG((LM_INFO,"MAIN idle voice resource pool out of sync\n"));
       result = -1;
  }
  
  if  (voxhandles)                          // Release all voice resources
  {    releaseResources(voxhandles, voxdevicecount);
       delete[] voxhandles;
  }

  return result;
}




int main (int argc, char *argv[])
//-----------------------------------------------------------------------------
// main
//-----------------------------------------------------------------------------
{
  ACE_Trace::stop_tracing(); 
                                              
  const int doAcquireReleaseTest                  = 1; // iterations
  const int doAllVoiceIpConnectionsTest           = 0; // boolean
  const int doWaitAndAcquireIdleVoiceResourceTest = 0; // boolean
  const int doSingleConferenceTest                = 0; // iterations
  const int doMultipleConferenceTest              = 0; // iterations
  const int doMaxConferencesTest                  = 0; // boolean

  do {         // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

  config = new MmsConfig;                   // Read config info
  if  (-1 == config->readLocalConfigFile())  
  {    std::cout << "Could not read/open config file\n"; 
       break;
  }

  resmgr = new HmpResourceManager(config);  // Instantiate resource manager        
  if  (NULL == resmgr) break; 

  int  result  = resmgr->init();
  if  (result == -1) break;
                                            // Get configured resource max
  configMax[MmsMediaDevice::IP]    = config->calculated.maxMediaResourcesIP;
  configMax[MmsMediaDevice::VOICE] = config->calculated.maxMediaResourcesVoice; 
  configMax[MmsMediaDevice::CONF]  = config->calculated.maxMediaResourcesConf;

       
  if (doAcquireReleaseTest) 
  {   
      MMSLOG((LM_INFO,"\nMAIN begin acquire/release test %d iterations\n",
              doAcquireReleaseTest));
      if  (-1 == testGetAndReleaseAllAll(doAcquireReleaseTest))  
      {    MMSLOG((LM_INFO,"MAIN acquire/release test failed\n"));
           break;
      }
      MMSLOG((LM_INFO,"MAIN end acquire/release test\n"));
  }



  if (doAllVoiceIpConnectionsTest)
  {                                         
      MMSLOG((LM_INFO,"\nMAIN begin voice/IP connection test\n"));
      if  (-1 == testAllVoiceIpConnections())  
      {    MMSLOG((LM_INFO,"MAIN voice/IP connection test failed\n"));
           break;
      }
      MMSLOG((LM_INFO,"MAIN end voice/IP connection test\n"));
  }



  if (doWaitAndAcquireIdleVoiceResourceTest)
  {                                           
      MMSLOG((LM_INFO,"\nMAIN begin WaitAndAcquireIdleVoiceResource test\n"));
      if  (-1 == testWaitAndAcquireIdleVoiceResource())  
      {    MMSLOG((LM_INFO,"MAIN WaitAndAcquireIdleVoiceResource test failed\n"));
           break;
      }
      MMSLOG((LM_INFO,"MAIN end WaitAndAcquireIdleVoiceResource test\n"));
  }



  if (doSingleConferenceTest)
  {                                             
      MMSLOG((LM_INFO,"\nMAIN begin testAllConferenceIpConnections test %d iterations\n",
              doSingleConferenceTest));
      if  (-1 == testAllConferenceIpConnections(doSingleConferenceTest))  
      {    MMSLOG((LM_INFO,"MAIN testAllConferenceIpConnections test failed\n"));
           break;
      }
      MMSLOG((LM_INFO,"MAIN end testAllConferenceIpConnections test\n"));
  }



  if (doMultipleConferenceTest)
  {                                             
      MMSLOG((LM_INFO,"\nMAIN begin testMultipleConferences test %d iterations\n",
              doMultipleConferenceTest));
      if  (-1 == testMultipleConferences(0, doMultipleConferenceTest))  
      {    MMSLOG((LM_INFO,"MAIN testMultipleConferences test failed\n"));
           break;
      }
      MMSLOG((LM_INFO,"MAIN end testMultipleConferences test\n"));
  }



  if (doMaxConferencesTest)
  {                                             
      MMSLOG((LM_INFO,"\nMAIN begin max conferences test\n"));
      if  (-1 == testMaxConferences())  
      {    MMSLOG((LM_INFO,"MAIN max conferences test failed\n"));
           break;
      }
      MMSLOG((LM_INFO,"MAIN end max conferences test\n"));
  }


  } while(0);  // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

  if  (resmgr) resmgr->shutdown();
  if  (resmgr) delete resmgr;
  if  (config) delete config;
  WAITFORINPUT;
  return 0;
}    

