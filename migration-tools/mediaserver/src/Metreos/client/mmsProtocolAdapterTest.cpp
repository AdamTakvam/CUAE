// MmsProtocolAdapterTest.cpp  
#include "StdAfx.h"
#pragma warning(disable:4786)
#include "mmsProtocolAdapterTest.h"
#include "dxtables.h"


void MmsTestProtocolAdapter::onHeartbeat() 
{ 
}

void MmsTestProtocolAdapter::onServerPush(MmsMsg* msg) 
{ 
}

void MmsTestProtocolAdapter::onStartAdapter()
{
  // This is not indicative of how a normal adapter would start, since here we
  // are launching a self-contained state machine rather than being driven by 
  // network events.

  onCommandComplete(0);
}



int MmsTestProtocolAdapter::preparseCommand(void* protocolData)
{
  // Inspect data received over the network and determine server command
  // represented by that data. Return the command ID to base class which
  // will dispatch the appropriate command handler.
  // 
  // In this server test scenario, "pre-parsed" test data is generated in
  // onCommandTestComplete and passed to this method in lieu of live data.

  MmsBogusProtocolData* testData = (MmsBogusProtocolData*)protocolData;
  std::string command = testData->getCommandName();
  MMSLOG((LM_INFO,"%s begin media server %s test\n",taskName, command.c_str())); 

  if  (command.compare(BOGUSNAME_CONNECT) == 0)    
       return COMMANDTYPE_CONNECT;
  if  (command.compare(BOGUSNAME_DISCONNECT) == 0) 
       return COMMANDTYPE_DISCONNECT;
  if  (command.compare(BOGUSNAME_PLAY) == 0)       
       return COMMANDTYPE_PLAY;
  if  (command.compare(BOGUSNAME_PLAYTONE) == 0)   
       return COMMANDTYPE_PLAYTONE;
  if  (command.compare(BOGUSNAME_RECORD) == 0)     
       return COMMANDTYPE_RECORD;
  if  (command.compare(BOGUSNAME_RECEIVE_DIGITS) == 0)      
       return COMMANDTYPE_RECEIVE_DIGITS;
  if  (command.compare(BOGUSNAME_RECORD_TRANSACTION) == 0) 
       return COMMANDTYPE_RECORD_TRANSACTION;
  if  (command.compare(BOGUSNAME_STOP_MEDIA_OPERATION)== 0) 
       return COMMANDTYPE_STOP_OPERATION;
  if  (command.compare(BOGUSNAME_ASSIGN_VOLADJ_DIGIT) == 0) 
       return COMMANDTYPE_ASSIGN_VOLADJ_DIGIT;
  if  (command.compare(BOGUSNAME_ASSIGN_SPDADJ_DIGIT) == 0) 
       return COMMANDTYPE_ASSIGN_SPEEDADJ_DIGIT;
  if  (command.compare(BOGUSNAME_ADJUST_VOLUME) == 0) 
       return COMMANDTYPE_ADJUST_VOLUME;
  if  (command.compare(BOGUSNAME_ADJUST_SPEED) == 0) 
       return COMMANDTYPE_ADJUST_SPEED;
  if  (command.compare(BOGUSNAME_CLEAR_ADJUSTMENTS) == 0) 
       return COMMANDTYPE_CLEAR_VS_ADJUSTMENTS;
  if  (command.compare(BOGUSNAME_SET_ATTRIBUTES) == 0)    
       return COMMANDTYPE_CONFEREE_SETATTRIBUTE;
  if  (command.compare(BOGUSNAME_ENABLE_VOLCONTROL) == 0) 
       return COMMANDTYPE_CONFEREE_ENABLE_VOL;
  if  (command.compare(BOGUSNAME_CONFERENCE_RESOURCES) == 0) 
       return COMMANDTYPE_CONFERENCE_RESOURCES;
  if  (command.compare(BOGUSNAME_VOICEREC) == 0) 
       return COMMANDTYPE_VOICEREC;

  MMSLOG((LM_INFO,"%s unrecognized command string'%s'\n",taskName,command));
  return 0;
}



void MmsTestProtocolAdapter::onCommandComplete(char* flatmap)
{ 
  // Overrides base class version in order to implement a state machine to 
  // route control to the next command in the media server test sequence. 
  // We'll still want to invoke the base class version in order to free the 
  // completed command's parameter map memory. Ordinarily a protocol adapter
  // would have no need to override this method,  each onReturnXxxxx() method 
  // invoking the base class version instead. Ordinarily the protocol would of
  // course include the current connection ID, conf ID, etc, in any subsequent 
  // request on a call, but since we're generating test data, we need to copy 
  // those data from the previous command parameter map, prior to freeing the 
  // map memory.

  int   commandID = 0, connectionID = 0;
  char* confID = NULL;
  MmsBogusProtocolData* protocolData = NULL;

  if  (flatmap)
  {
       commandID    = getFlatmapCommand(flatmap);
       connectionID = getFlatmapConnectionID(flatmap);
       MmsFlatMapReader map(flatmap);
       map.find(MMSP_CONFERENCE_ID, &confID);
  }
                                            // Pause between commands 
  int pauseMs = MMS_SERVERTEST_INTERCOMMAND_PAUSE_SECONDS * 1000;   
  pauseMs = (rand() % pauseMs) + (pauseMs / 2);           
  mmsSleep(MmsTime(0, pauseMs * 1000));      


  switch(commandID)                         // Fire off next command:
  {  
    case 0:                                 // Initial command is connect 
         protocolData = new MmsBogusProtocolDataCONNECT;
         break;

    case COMMANDTYPE_CONNECT:  
         protocolData = new MmsBogusProtocolDataPLAY(connectionID);
         break;

    case COMMANDTYPE_DISCONNECT: 
         break;
      
    case COMMANDTYPE_PLAY: 
         protocolData = new MmsBogusProtocolDataPLAYTONE(connectionID);                 
         break;
              
    case COMMANDTYPE_RECORD: 
         protocolData = new MmsBogusProtocolDataRECEIVEDIGITS(connectionID);                              
         break;
              
    case COMMANDTYPE_RECORD_TRANSACTION:     
         break;
              
    case COMMANDTYPE_PLAYTONE:  
         protocolData = new MmsBogusProtocolDataRECORD(connectionID);              
         break;
              
    case COMMANDTYPE_RECEIVE_DIGITS: 
         protocolData = new MmsBogusProtocolDataDISCONNECT(connectionID);        
         break;

    case COMMANDTYPE_STOP_OPERATION:
         protocolData = new MmsBogusProtocolData
                       (BOGUSNAME_STOP_MEDIA_OPERATION,connectionID);        
         break;
              
    case COMMANDTYPE_ASSIGN_VOLADJ_DIGIT: 
         protocolData = new MmsBogusProtocolDataASSIGNVOLDIGIT(connectionID);           
         break;
              
    case COMMANDTYPE_ASSIGN_SPEEDADJ_DIGIT:
         protocolData = new MmsBogusProtocolDataASSIGNSPEEDDIGIT(connectionID);         
         break;
              
    case COMMANDTYPE_ADJUST_VOLUME:
         protocolData = new MmsBogusProtocolDataADJUSTVOLUME(connectionID);                  
         break;
              
    case COMMANDTYPE_ADJUST_SPEED: 
         protocolData = new MmsBogusProtocolDataADJUSTSPEED(connectionID);                  
         break;
              
    case COMMANDTYPE_CLEAR_VS_ADJUSTMENTS:
         protocolData = new MmsBogusProtocolDataCLEARADJUSTMENTS(connectionID);           
         break;              
              
    case COMMANDTYPE_CONFERENCE_RESOURCES:
         protocolData = new MmsBogusProtocolDataCONFRESOURCES(connectionID);              
         break;                           
              
    case COMMANDTYPE_CONFEREE_SETATTRIBUTE:
         protocolData = new MmsBogusProtocolDataCONFEREEATTRS(connectionID);      
         break;
              
    case COMMANDTYPE_CONFEREE_ENABLE_VOL:
         protocolData = new MmsBogusProtocolDataCONFEREEVOLENABLE(connectionID);    
         break;

    case COMMANDTYPE_VOICEREC:
      break;
  } 

                                            // Invoke base class to free mem
  MmsIpcAdapter::onCommandComplete(flatmap);
  m_iterations++;
                                            // If more commands in script ...
  if  (protocolData)                        // ... fire off next command
       this->onProtocolDataReceived(protocolData); 
  else                                      // If looping test script ...
  if  (this->config->clientParams.serverTestAdapterIterations != 1)
  {                                         // If more iterations ... 
       int  reqIters = this->config->clientParams.serverTestAdapterIterations;
       if  (reqIters == 0 || (m_iterations < reqIters))
            this->onCommandComplete(0);     // ... start over
       else this->shutdownMediaServer();    // ... or shut down
  }                                         
  else this->shutdownMediaServer();         // Shut down if one iteration         
}                                            


// - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
// Command parsers/launchers
// - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -


void MmsTestProtocolAdapter::onCommandConnect(void* protocolData)
{ 
  // Parse protocol data for a connnection request, extracting parameters and
  // creating the parameter map. 
  //
  // In this test scenario, we merely pretend to parse the data presented to us,
  // since our test data comes "pre-parsed"  We include most parameters here,
  // where clients might normally accept configured defaults. 

  MmsBogusProtocolDataCONNECT* testData = 
 (MmsBogusProtocolDataCONNECT*)protocolData;

  MmsFlatMapWriter map;                     // Build parameter map                                           
  map.insert(MMSP_IP_ADDRESS, MmsFlatMap::STRING, 
             testData->getIpLength()+1, testData->getIP());
  map.insert(MMSP_PORT_NUMBER, testData->getPort());
                                          
  unsigned int remoteConxAttrs              // Pack the attributes doubleword: 
    = MMS_CODER_G711ALAW64K | MMS_CODER_FRAMESIZE_20;

  MMS_CODER_PAYLOAD_TYPE_PUT(remoteConxAttrs, 
      testData->getRemoteCoderPayloadType());

  MMS_REDUNDANCY_PAYLOAD_TYPE_PUT(remoteConxAttrs, 
      testData->getRemoteRedundancyPayloadType());

  map.insert(MMSP_REMOTE_CONX_ATTRIBUTES, (int)remoteConxAttrs);

  map.insert(MMSP_SESSION_TIMEOUT_SECS, testData->getSessionTimeoutSecs());
  map.insert(MMSP_COMMAND_TIMEOUT_MS,   testData->getCommandTimeoutMs());

  char* paramatermap = this->mapComplete(map, COMMANDTYPE_CONNECT);

  setFlatmapTransID(paramatermap, GetTickCount());
  setFlatmapConnectionID(paramatermap, testData->getConnectionID());
  delete protocolData;                      // Free the test data
                                            
  this->postServerCommand(paramatermap);    // Send command to server
}
   


void MmsTestProtocolAdapter::onCommandDisconnect(void* protocolData)
{  
  // Parse protocol data for a disconnnection request, extracting parameters 
  // and creating the parameter map. 
 
  MmsBogusProtocolDataDISCONNECT* testData = 
 (MmsBogusProtocolDataDISCONNECT*)protocolData;

  MmsFlatMapWriter map;                     // Build parameter map 
  this->mapInit(map, testData);

  char* paramatermap = this->mapComplete(map, COMMANDTYPE_DISCONNECT);

  setFlatmapTransID(paramatermap, GetTickCount());
  setFlatmapConnectionID(paramatermap, testData->getConnectionID());
  delete protocolData;                      // Free the test data
                                             
  this->postServerCommand(paramatermap);    // Send command to server
}



void MmsTestProtocolAdapter::onCommandPlay(void* protocolData)
{ 
  // Parse protocol data for a media play request, extracting parameters and
  // creating the parameter map. 
  //
  // In this test scenario, we merely pretend to parse the data presented to us,
  // since our test data comes "pre-parsed"  We include most parameters here,
  // where users might normally accept the configured defaults. 

  // Play can have multiple filespec parameters, and multiple termination 
  // condition parameters. Parameters are represented as a map of maps of maps.
  // A filespec can include not only the filename (and relative path), but also
  // under certain condtions the file offset and segment length, which we do
  // not bother to look for here, but should in real life.

  MmsBogusProtocolDataPLAY* testData = 
 (MmsBogusProtocolDataPLAY*)protocolData;

  MmsFlatMapWriter map(1024);                      
  this->mapInit(map, testData);

  MMS_BOGUS_FILELIST& filelist = testData->files;
  this->buildFileListParameters(map, filelist);
                                       
  MMS_BOGUS_TERMINATION_CONDITIONS& terminations = testData->terminations;
  this->buildTerminationConditionParameters(map,terminations); 

  int dataformat = testData->format;
  int datarate   = testData->rate;
  unsigned int attrs = this->buildPlayRecordParameters(dataformat, datarate);                                 
  map.insert(MMSP_PLAY_RECORD_ATTRIBUTES, (int)attrs);                

  char* paramatermap = this->mapComplete(map, COMMANDTYPE_PLAY);

  setFlatmapTransID(paramatermap, GetTickCount());
  setFlatmapConnectionID(paramatermap, testData->getConnectionID());
  delete protocolData;                      // Free the test data
                                             
  this->postServerCommand(paramatermap);    // Send command to server
}



void MmsTestProtocolAdapter::onCommandPlaytone(void* protocolData)
{ 
  // Parse protocol data for a media play tone request, extracting parameters 
  // and creating the parameter map.Playone can have zero to two frequency/
  // amplitude parameters, and multiple termination condition parameters. 
  // Parameters are represented as a map of maps.

  MmsBogusProtocolDataPLAYTONE* testData = 
 (MmsBogusProtocolDataPLAYTONE*)protocolData;

  MmsFlatMapWriter map;                      
  this->mapInit(map, testData);
                                            // Frequency/amplitude
  int  f1 = testData->f1, f2 = testData->f2, a1 = testData->a1, a2 = testData->a2;
  int  duration = testData->duration;

  if  (f1 || a1)
  {
    MmsFlatMapWriter faMap(128); 
    faMap.insert(MMSP_FREQUENCY, f1);
    faMap.insert(MMSP_AMPLITUDE, a1);
    char* buf = map.format(MMSP_FREQUENCY_AMPLITUDE, MmsFlatMap::FLATMAP, faMap.length()); 
    faMap.marshal(buf);
  }

  if  (f2 || a2)
  {
    MmsFlatMapWriter faMap(128); 
    faMap.insert(MMSP_FREQUENCY, f2);
    faMap.insert(MMSP_AMPLITUDE, a2);
    char* buf = map.format(MMSP_FREQUENCY_AMPLITUDE, MmsFlatMap::FLATMAP, faMap.length()); 
    faMap.marshal(buf);
  }

  if  (duration)
       map.insert(MMSP_DURATION, duration);                                          

                                            // Termination conditions
  MMS_BOGUS_TERMINATION_CONDITIONS& terminations = testData->terminations;
  this->buildTerminationConditionParameters(map,terminations); 
                 
  char* paramatermap = this->mapComplete(map, COMMANDTYPE_PLAYTONE);

  setFlatmapTransID(paramatermap, GetTickCount());
  setFlatmapConnectionID(paramatermap, testData->getConnectionID());
  delete protocolData;                      // Free the test data
                                             
  this->postServerCommand(paramatermap);    // Send command to server
}

void MmsTestProtocolAdapter::onCommandRecord(void* protocolData)
{
}

void MmsTestProtocolAdapter::onCommandVoiceRec(void* protocolData)
{ 
  // Parse protocol data for a media record request, extracting parameters and
  // creating the parameter map. See onCommandPlay for additional comments

  MmsBogusProtocolDataRECORD* testData = 
 (MmsBogusProtocolDataRECORD*)protocolData;

  MmsFlatMapWriter map(1024);                      
  this->mapInit(map, testData);

  MMS_BOGUS_FILELIST& filelist = testData->files;
  this->buildFileListParameters(map, filelist);
                                        
  MMS_BOGUS_TERMINATION_CONDITIONS& terminations = testData->terminations;
  this->buildTerminationConditionParameters(map,terminations); 

  int dataformat = testData->format;
  int datarate   = testData->rate;
  unsigned int attrs = this->buildPlayRecordParameters(dataformat, datarate);                                
  map.insert(MMSP_PLAY_RECORD_ATTRIBUTES, (int)attrs);                 

  char* paramatermap = this->mapComplete(map, COMMANDTYPE_RECORD);

  setFlatmapTransID(paramatermap, GetTickCount());
  setFlatmapConnectionID(paramatermap, testData->getConnectionID());
  delete protocolData;                      // Free the test data
                                             
  this->postServerCommand(paramatermap);    // Send command to server
}



void MmsTestProtocolAdapter::onCommandRecordTransaction(void* protocolData)
{ 
  // Parse protocol data for a media record transaction request, extracting  
  // parameters and creating the parameter map. This is not yet in the test
  // mix. In order to test it, we'll want to set up a command sequence in which
  // we create two connections, then use the connectionID of the second
  // connection to create a record transaction package on the session of the 
  // first connection.

  MmsBogusProtocolDataRECORDTRANSACTION* testData = 
 (MmsBogusProtocolDataRECORDTRANSACTION*)protocolData;

  MmsFlatMapWriter map(1024);                      
  this->mapInit(map, testData);

  MMS_BOGUS_FILELIST& filelist = testData->files;
  this->buildFileListParameters(map, filelist);
                                        
  MMS_BOGUS_TERMINATION_CONDITIONS& terminations = testData->terminations;
  this->buildTerminationConditionParameters(map,terminations); 

  int dataformat = testData->format;
  int datarate   = testData->rate;
  unsigned int attrs = this->buildPlayRecordParameters(dataformat, datarate);                                
  map.insert(MMSP_PLAY_RECORD_ATTRIBUTES, (int)attrs);  
                                            // Insert the second connection ID
  int secondConnectionID = testData->connectionID2;
  map.insert(MMSP_CONNECTION_ID, secondConnectionID);               

  char* paramatermap = this->mapComplete(map, COMMANDTYPE_RECORD_TRANSACTION);

  setFlatmapTransID(paramatermap, GetTickCount());
  setFlatmapConnectionID(paramatermap, testData->getConnectionID());
  delete protocolData;                      // Free the test data
                                             
  this->postServerCommand(paramatermap);    // Send command to server
}



void MmsTestProtocolAdapter::onCommandReceiveDigits(void* protocolData)
{  
  // Parse protocol data for a receive digits request, extracting parameters 
  // and creating the parameter map. 
 
  MmsBogusProtocolDataRECEIVEDIGITS* testData = 
 (MmsBogusProtocolDataRECEIVEDIGITS*)protocolData;

  MmsFlatMapWriter map;                     // Build parameter map 
  this->mapInit(map, testData);

  MMS_BOGUS_TERMINATION_CONDITIONS& terminations = testData->terminations;
  this->buildTerminationConditionParameters(map,terminations); 
                                            // Commit parameter map 
  char* paramatermap = this->mapComplete(map, COMMANDTYPE_RECEIVE_DIGITS);

  setFlatmapTransID(paramatermap, GetTickCount());
  setFlatmapConnectionID(paramatermap, testData->getConnectionID());
  delete protocolData;                      // Free the test data

  this->postServerCommand(paramatermap);    // Send command to server
}



void MmsTestProtocolAdapter::onCommandStopMediaOperation(void* protocolData)
{
  // Server does not generate a return package on a stop media, so this
  // command is launched and forgotten. Adapter will receive the termination 
  // event for the media operation which was canceled.

  this->onGenericCommand
      ((MmsBogusProtocolData*)protocolData, COMMANDTYPE_STOP_OPERATION); 
}



void MmsTestProtocolAdapter::onCommandConferenceResourcesRemaining(void* protocolData)
{
  this->onGenericCommand
      ((MmsBogusProtocolData*)protocolData, COMMANDTYPE_CONFERENCE_RESOURCES); 
}



void MmsTestProtocolAdapter::onCommandAssignVolumeAdjustmentDigit(void* protocolData)
{
  MmsBogusProtocolData* testData = (MmsBogusProtocolData*)protocolData; 
  MmsFlatMapWriter map;                     // Build parameter map                       
  this->mapInit(map, testData);
  
  unsigned int adjval = 0;                  // Request that the digit 9 will 
  MMS_PUT_ADJUSTMENT_VALUE(adjval, 1);      // adjust volume up 1 tick (2dB)
  MMS_PUT_ADJUSTMENT_DIGIT(adjval,'9');
  map.insert(MMSP_VOLSPEED_DIGIT, (int)adjval);
                                             
  char* paramatermap = this->mapComplete(map, COMMANDTYPE_ASSIGN_VOLADJ_DIGIT);

  setFlatmapTransID(paramatermap, GetTickCount());
  setFlatmapConnectionID(paramatermap, testData->getConnectionID());
  delete protocolData;                      // Free the test data

  this->postServerCommand(paramatermap);    // Send command to server
}



void MmsTestProtocolAdapter::onCommandAssignSpeedAdjustmentDigit(void* protocolData)
{
  MmsBogusProtocolData* testData = (MmsBogusProtocolData*)protocolData; 
  MmsFlatMapWriter map;                     // Build parameter map                       
  this->mapInit(map, testData);
  
  unsigned int adjval = 0;                  // Request that the digit 8 will 
  MMS_PUT_ADJUSTMENT_VALUE(adjval,-1);      // adjust speed down 1 tick (10%)
  MMS_PUT_ADJUSTMENT_DIGIT(adjval,'8');
  map.insert(MMSP_VOLSPEED_DIGIT, (int)adjval);

  char* paramatermap = this->mapComplete(map, COMMANDTYPE_ASSIGN_SPEEDADJ_DIGIT);

  setFlatmapTransID(paramatermap, GetTickCount());
  setFlatmapConnectionID(paramatermap, testData->getConnectionID());
  delete protocolData;                      // Free the test data

  this->postServerCommand(paramatermap);    // Send command to server
}



void MmsTestProtocolAdapter::onCommandAdjustVolume(void* protocolData) 
{
  MmsBogusProtocolData* testData = (MmsBogusProtocolData*)protocolData; 
  MmsFlatMapWriter map;                     // Build parameter map                       
  this->mapInit(map, testData);
  
  unsigned int adjval = 0;                  // Request that the volume be 
  MMS_PUT_ADJUSTMENT_VALUE(adjval, -2);     // adjusted down 2 ticks (-4dB)
  MMS_PUT_ADJUSTMENT_DIGIT(adjval, MMS_ADJUST_TO_RELATIVE_POSITION);
  map.insert(MMSP_VOLSPEED_ADJUSTMENT, (int)adjval);

  char* paramatermap = this->mapComplete(map, COMMANDTYPE_ADJUST_VOLUME);

  setFlatmapTransID(paramatermap, GetTickCount());
  setFlatmapConnectionID(paramatermap, testData->getConnectionID());
  delete protocolData;                      // Free the test data

  this->postServerCommand(paramatermap);    // Send command to server
}


 
void MmsTestProtocolAdapter::onCommandAdjustSpeed(void* protocolData)
{
  MmsBogusProtocolData* testData = (MmsBogusProtocolData*)protocolData; 
  MmsFlatMapWriter map;                     // Build parameter map                       
  this->mapInit(map, testData);
  
  unsigned int adjval = 0;                  // Request that the speed be 
  MMS_PUT_ADJUSTMENT_VALUE(adjval, 2);      // adjusted up 2 ticks (20%)
  MMS_PUT_ADJUSTMENT_DIGIT(adjval, MMS_ADJUST_TO_RELATIVE_POSITION);
  map.insert(MMSP_VOLSPEED_ADJUSTMENT, (int)adjval);

  char* paramatermap = this->mapComplete(map, COMMANDTYPE_ADJUST_SPEED);

  setFlatmapTransID(paramatermap, GetTickCount());
  setFlatmapConnectionID(paramatermap, testData->getConnectionID());
  delete protocolData;                      // Free the test data

  this->postServerCommand(paramatermap);    // Send command to server
}



void MmsTestProtocolAdapter::onCommandClearVolumeSpeedAdjustments(void* protocolData)
{
  this->onGenericCommand
        ((MmsBogusProtocolData*)protocolData, COMMANDTYPE_CLEAR_VS_ADJUSTMENTS);                      
}              
     


void MmsTestProtocolAdapter::onCommandConfereeSetAttribute(void* protocolData) 
{
  // R/O, coach, pupil, tariff tone. Bitflags are MMS_CONFEREE_RECEIVE_ONLY,  
  // MMS_CONFEREE_TARIFF_TONE, MMS_CONFEREE_COACH, MMS_CONFEREE_PUPIL  
  // Set one at a time, setting MMS_CONFEREE_ATTRIBUTE_OFF bit if attribute
  // is to be turned off               

  MmsBogusProtocolData* testData = (MmsBogusProtocolData*)protocolData; 
  MmsFlatMapWriter map;                     // Build parameter map                       
  this->mapInit(map, testData);
  
  unsigned int confereeAttrs = 0; 
  confereeAttrs |= MMS_CONFEREE_TARIFF_TONE;           
  map.insert(MMSP_CONFEREE_ATTRIBUTES, (int)confereeAttrs);
                                             
  char* paramatermap = this->mapComplete(map, COMMANDTYPE_CONFEREE_SETATTRIBUTE);
  setFlatmapTransID(paramatermap, GetTickCount());
  setFlatmapConnectionID(paramatermap, testData->getConnectionID());
  delete protocolData;                      // Free the test data

  this->postServerCommand(paramatermap);    // Send command to server
}


                                              
void MmsTestProtocolAdapter::onCommandConfereeEnableVolumeControl(void* protocolData)
{
  MmsBogusProtocolData* testData = (MmsBogusProtocolData*)protocolData; 
  MmsFlatMapWriter map;                     // Build parameter map                       
  this->mapInit(map, testData);  
  unsigned int volcontrol = 0; 

  MMS_SET_ONOROFF(volcontrol, 1);           // Enable
  MMS_SET_VOLRESET_DIGIT(volcontrol,'0');
  MMS_SET_VOLUP_DIGIT(volcontrol,'1');
  MMS_SET_VOLDOWN_DIGIT(volcontrol,'2');

  map.insert(MMSP_CONFERENCE_VOLCONTROL, (int)volcontrol);

  char* paramatermap = this->mapComplete(map, COMMANDTYPE_CONFEREE_ENABLE_VOL);
  setFlatmapTransID(paramatermap, GetTickCount());
  setFlatmapConnectionID(paramatermap, testData->getConnectionID());
  delete protocolData;                      // Free the test data

  this->postServerCommand(paramatermap);    // Send command to server
}



void MmsTestProtocolAdapter::onGenericCommand(MmsBogusProtocolData* testData, int command)
{
  // Launch server command that takes no map parameters other than conxID
  MmsFlatMapWriter map;                     // Build parameter map 
  this->mapInit(map, testData);
                                            // Commit map
  char* paramatermap = this->mapComplete(map, command);

  setFlatmapTransID(paramatermap, GetTickCount());
  setFlatmapConnectionID(paramatermap, testData->getConnectionID());
  delete testData;                          // Free the test data
                                            
  this->postServerCommand(paramatermap);    // Send command to server
}


// - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
// Command return handlers
// - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -


void MmsTestProtocolAdapter::onReturnConnect(char* flatmap)
{
  // Here we return the result of a connect command to the protocol.
  // We must always call onCommandComplete(map) when we're done, so that 
  // the map's memory can be released.
  //
  // In this test situation we have no client to which to return the
  // result. We merely check the result and fire off the next test,
  // via our overridden onCommandComplete();

  int  retcode  = getFlatmapRetcode(flatmap);
  int  wasError = isFlatmapFlagSet (flatmap, MmsServerCmdHeader::IS_ERROR);
  MMSLOG((LM_INFO,"%s connect result %d\n",taskName, retcode));
  
  if  (!wasError)                           // Ensure connection ID was assigned
  {                                            
       MmsFlatMapReader map(flatmap); 
       int  connectionID  = getFlatmapConnectionID(flatmap);       
       if (!connectionID) 
            wasError = TRUE;                 
  }
   
  if  (wasError)
  {    
       MMSLOG((LM_INFO,"%s connection error - signaling shutdown\n",taskName));
       this->shutdownMediaServer();
       MmsIpcAdapter::onCommandComplete(flatmap);
  } 
  else this->onCommandComplete(flatmap);    // Free mem and fire off next test
}



void MmsTestProtocolAdapter::onReturnDisconnect(char* flatmap)
{
  this->onGenericReturn(flatmap, "disconnect");
}



void MmsTestProtocolAdapter::onReturnPlay(char* flatmap)
{
  this->onGenericReturn(flatmap, "play");
}



void MmsTestProtocolAdapter::onReturnPlaytone(char* flatmap)
{
  this->onGenericReturn(flatmap, "playtone");
}

void MmsTestProtocolAdapter::onReturnVoiceRec(char* flatmap)
{
  this->onGenericReturn(flatmap, "voiceRec");
}

void MmsTestProtocolAdapter::onReturnRecord(char* flatmap)
{
  this->onGenericReturn(flatmap, "record");
}



void MmsTestProtocolAdapter::onReturnRecordTransaction(char* flatmap)
{
  this->onGenericReturn(flatmap, "record trans");
}



void MmsTestProtocolAdapter::onReturnReceiveDigits(char* flatmap)
{
  int  retcode  = getFlatmapRetcode(flatmap);
  int  wasError = isFlatmapFlagSet(flatmap, MmsServerCmdHeader::IS_ERROR);
  MMSLOG((LM_INFO,"%s receive digits result %d\n",taskName, retcode));
  if  (wasError)
       MMSLOG((LM_INFO,"%s WARNING: digits error\n",taskName));

  MmsFlatMapReader map;
  char* digitbuf = NULL, numdigits = 0;
  int length = map.find(MMSP_RECEIVE_DIGITS_RETURN_BUFFER, &digitbuf);
  if  (digitbuf)
       numdigits = length? length-1: 0;
  MMSLOG((LM_INFO,"%s %d digits collected\n",taskName,numdigits));

  this->showTerminationReasons(flatmap);

  this->onCommandComplete(flatmap);          
}



void MmsTestProtocolAdapter::onReturnStopMediaOperation(char* flatmap)
{
  // Server does not generate a return package on a stop media,
  // so this is unused. Adapter will receive the termination event
  // for the media operation which was canceled.
  this->onGenericReturn(flatmap, "stop media");
}



void MmsTestProtocolAdapter::onReturnConferenceResourcesRemaining(char* flatmap)
{
  // Resources remaining returned in map param
  int resourcesRemaining = getFlatmapParam(flatmap);
  MMSLOG((LM_INFO,"%s conference resources remaining: %d\n",
          taskName, resourcesRemaining));

  this->onGenericReturn(flatmap, "conf resx remaining");
}



void MmsTestProtocolAdapter::onReturnAdjustments(char* flatmap)
{
  char*  text = NULL;
  switch(getFlatmapCommand(flatmap))
  { case COMMANDTYPE_ASSIGN_VOLADJ_DIGIT:   text = "asgn voladj"; break;
    case COMMANDTYPE_ASSIGN_SPEEDADJ_DIGIT: text = "asgn spdadj"; break; 
    case COMMANDTYPE_ADJUST_VOLUME:         text = "adj vol";     break;
    case COMMANDTYPE_ADJUST_SPEED:          text = "adj spd";     break;
    case COMMANDTYPE_CLEAR_VS_ADJUSTMENTS:  text = "clear adj";   break;
  }
  if (text) onGenericReturn(flatmap, text);
}



void MmsTestProtocolAdapter::onReturnConfereeSetAttribute(char* flatmap)
{
  this->onGenericReturn(flatmap, "conferee attr");
}



void MmsTestProtocolAdapter::onReturnConfereeEnableVolumeControl(char* flatmap)
{
  this->onGenericReturn(flatmap, "conferee volctrl");
}



void MmsTestProtocolAdapter::onGenericReturn(char* flatmap, char* text)
{
  // Here we return the result of a standard command to the protocol.
  // We must always call onCommandComplete(map) when we're done, so that 
  // the map's memory can be released.
  //
  // In this test situation we have no client to which to return the
  // result. We merely check the result and fire off the next test,
  // via our overridden onCommandComplete();

  int  retcode  = getFlatmapRetcode(flatmap);
  int  wasError = isFlatmapFlagSet (flatmap, MmsServerCmdHeader::IS_ERROR);
  MMSLOG((LM_INFO,"%s %s result %d\n",taskName, text, retcode));
  if  (wasError)
       MMSLOG((LM_INFO,"%s WARNING: %s error\n",taskName, text));

  this->showTerminationReasons(flatmap);

  this->onCommandComplete(flatmap);         // Free mem and fire off next test
} 


// - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
// Support methods
// - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -


void MmsTestProtocolAdapter::buildTerminationConditionParameters
( MmsFlatMapWriter& map, MMS_BOGUS_TERMINATION_CONDITIONS& terminations)
{
  // Insert each termination condition into a map; embed each of those maps
  // into the aggregate map

  MmsFlatMapWriter termcondsMap;
  int termcount = terminations.count();

  for(int i=0; i < termcount; i++)
  {                                         
    MMS_BOGUS_TERMINATION_CONDITION* cond = terminations.get(i);
    MmsFlatMapWriter condMap(200);

    int  type = 0;
    switch(cond->condition)
    { case MMS_BOGUS_TERMINATION_CONDITION::DIGIT:     type = DX_DIGTYPE;  break;
      case MMS_BOGUS_TERMINATION_CONDITION::TIMEOUT:   type = DX_MAXTIME;  break;
      case MMS_BOGUS_TERMINATION_CONDITION::CONDEOF:   type = 0;           break;
      case MMS_BOGUS_TERMINATION_CONDITION::MAXDIGITS: type = DX_MAXDTMF;  break;
      case MMS_BOGUS_TERMINATION_CONDITION::NONSILENCE:type = DX_MAXNOSIL; break;
      case MMS_BOGUS_TERMINATION_CONDITION::SILENCE:   type = DX_MAXSIL;   break;
    }

    if  (type == 0) continue;
    condMap.insert(MMSP_TERMINATION_CONDITION_TYPE,   type); 
    condMap.insert(MMSP_TERMINATION_CONDITION_LENGTH, cond->length);   
    condMap.insert(MMSP_TERMINATION_CONDITION_DATA1,  cond->value1); 
    condMap.insert(MMSP_TERMINATION_CONDITION_DATA2,  cond->value2);
    const int condmaplen = condMap.length();

    // Note that we also need to provide for a DX_DIGMASK condition. For this 
    // we'll need to write a MMSP_DIGITLIST parameter to the condition, with a 
    // digit string. If there is only one digit, it is merely DX_DIGTYPE.

    char* p = termcondsMap.format(MMSP_TERMINATION_CONDITION, 
                                  MmsFlatMap::FLATMAP, condmaplen); 
    condMap.marshal(p);
  }

  if  (termcondsMap.size())                 // Embed termconds map into main map
  {
       const int len = termcondsMap.length();
       char* p = map.format(MMSP_TERMINATION_CONDITIONS, MmsFlatMap::FLATMAP, len);
       termcondsMap.marshal(p);
  }
}



void MmsTestProtocolAdapter::buildFileListParameters
( MmsFlatMapWriter& map, MMS_BOGUS_FILELIST& filelist)
{
  MmsFlatMapWriter filelistMap;

  while(1)
  {                                          
    char* filepath = filelist.nextfile();    
    if   (filepath == NULL) break;          // Create filepsec map
    const int mapPathlen = ACE_OS::strlen(filepath) + 1;          
                                              
    MmsFlatMapWriter filespecMap(256);       
    filespecMap.insert(MMSP_FILENAME, MmsFlatMap::STRING, mapPathlen, filepath);
                                            // Embed filespec map to filelist map 
    char* p = filelistMap.format(MMSP_FILESPEC, MmsFlatMap::FLATMAP, filespecMap.length()); 
    filespecMap.marshal(p);
  }
                                            // Embed filelist map into main map
  char* p = map.format(MMSP_FILELIST, MmsFlatMap::FLATMAP, filelistMap.length());
  filelistMap.marshal(p);
}



unsigned int MmsTestProtocolAdapter::buildPlayRecordParameters
( const int dataformat, const int datarate)
{ 
  unsigned int attrs = 0;

  switch(dataformat)
  { case MmsBogusProtocolDataPLAY::MULAW: attrs |= MMS_FORMAT_MULAW; break; 
    case MmsBogusProtocolDataPLAY::ALAW:  attrs |= MMS_FORMAT_ALAW;  break; 
    case MmsBogusProtocolDataPLAY::PCM:   attrs |= MMS_FORMAT_PCM;   break; 
    case MmsBogusProtocolDataPLAY::ADPCM: attrs |= MMS_FORMAT_ADPCM; break; 
  }

  switch(datarate)
  { case MmsBogusProtocolDataPLAY::KHZ6:  attrs |= MMS_RATE_KHZ_6;   break; 
    case MmsBogusProtocolDataPLAY::KHZ8:  attrs |= MMS_RATE_KHZ_8;   break; 
    case MmsBogusProtocolDataPLAY::KHZ11: attrs |= MMS_RATE_KHZ_11;  break;
  }

  return attrs; 
}


                                          
void MmsTestProtocolAdapter::setCommandHeader(MmsServerCmdHeader* cmdHdr, int cmd)
{
  cmdHdr->command  = cmd;
  cmdHdr->sender   = this;
}



void MmsTestProtocolAdapter::mapInit(MmsFlatMapWriter& map, MmsBogusProtocolData* testData)
{
}



void MmsTestProtocolAdapter::shutdownMediaServer()
{
  MMSLOG((LM_INFO,"%s media server test complete, shutting down\n",taskName));
  this->postServerMessage(MMSM_SERVERCONTROL, MMS_SERVERCTRL_SHUTDOWN);
}

