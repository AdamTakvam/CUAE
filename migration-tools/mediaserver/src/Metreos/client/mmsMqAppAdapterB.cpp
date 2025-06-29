//
// MmsMqAppAdapter.cpp 
//
// App server msmq protocol adapter
//
// Command handling and dispatch
//
#include "StdAfx.h"
#pragma warning(disable:4786)
#include "mmsMqAppAdapter.h"
#define NOT_SERVERCONNECT (-1)

#ifdef MMS_ENABLE_MEMORY_LEAK_DETECTION
#define new MMS_MEMLEAK_NEW
#endif // MMS_ENABLE_MEMORY_LEAK_DETECTION



void MmsMqAppAdapter::onCommandConnect(void* IpcData)
{ 
  // Parse connnection request xml, extracting parameters, building parameter
  // map, and submitting command to media server 

  this->xmlmsg = (MmsAppMessage*)IpcData;
  do {

  char* ip      = xmlmsg->findparam(MmsAppMessageX::paramnames[MmsAppMessageX::IP_ADDRESS]); 
  char* port    = xmlmsg->findparam(MmsAppMessageX::paramnames[MmsAppMessageX::PORT]);
  char* conxID  = xmlmsg->findparam(MmsAppMessageX::paramnames[MmsAppMessageX::CONNECTION_ID]);
  char* confID  = xmlmsg->findparam(MmsAppMessageX::paramnames[MmsAppMessageX::CONFERENCE_ID]);
  char* rexmit  = xmlmsg->findparam(MmsAppMessageX::paramnames[MmsAppMessageX::REXMIT_CONNECTION]);  
  char* modify  = xmlmsg->findparam(MmsAppMessageX::paramnames[MmsAppMessageX::MODIFY]);
  char* rguid   = xmlmsg->findparam(MmsAppMessageX::paramnames[MmsAppMessageX::ROUTING_GUID]);

  int resultcode = 0; 
           
  // IP address and port must be supplied for a normal connection; however
  // if neither is supplied, and this does not appear to be a request to
  // move a connection into conference, we assume a server connect.

  if  (ip == NULL || port == NULL)          // No IP/port so ...  
  {                                           
       if  (conxID && confID)               // Legal conference connect
            resultcode = NOT_SERVERCONNECT; // ... so we'll want to continue
       else
       if  (conxID || confID)
            resultcode = MMS_ERROR_TOO_FEW_PARAMETERS;
       else              
       if  (ip == NULL && port == NULL)     // Is it a server connect?
                                            // Yes: open a new client queue
            resultcode = this->registerClient(TRUE, xmlmsg);

       else resultcode = NOT_SERVERCONNECT; 

                                            
       if  (resultcode !=NOT_SERVERCONNECT) // If server connect or error ...
       {                                    // ... return result now. 
            this->turnServerConnectMessageAround(resultcode, 0, FALSE);                
            break;                           
       }
  }
                                            // At this point it's a well-
  if (!this->isConnected())                 // formed, non-initial connect
  {    this->turnMqMessageAround(MMS_ERROR_NOT_CONNECTED, 0, 0);
       break;
  }                                         // Is client inititating multi-
  else                                      // client ID token passing?
  if  (xmlmsg->findparam(MmsAppMessageX::paramnames[MmsAppMessageX::CLIENT_ID])) 
       this->isExpectingClientToken = TRUE; // From now on, expect client ID

  MmsFlatMapWriter map;                     // Build server parameter map 
  int length=0, value=0, isConference=0, isModifyConnection=0, isRexmit=0; 
  if  (confID) isConference = TRUE;
  
  if (rguid)                                // Process routing guid
  {
      length = xmlmsg->isolateParameterValue(rguid);
      map.insert(MMSP_ROUTING_GUID, MmsFlatMap::STRING, length+1, xmlmsg->paramValue());
  }

  // If port is supplied nonzero, this implies a full connect, where the
  // remote session is started, in which case the remote IP address is also
  // expected. If port is zero, this is a half connect, in which the local
  // port and IP are returned without completing the connection. In the latter
  // case, we do not pass an IP address, an empty entry formatted instead 
  // which will be used to return the local IP.

  if  (port)                                // If port specified ...
  {                                         // ... pass IP and port: 
       length = xmlmsg->isolateParameterValue(port);
       value  = ACE_OS::atoi(xmlmsg->paramValue());
       map.insert(MMSP_PORT_NUMBER, value);

       // We're always going to return local port and ip address in the same
       // map entries as we pass remote port and ip address, so we need to 
       // preallocate the map IP entry at the maximum length of an IP address

       char* mapIP = map.format(MMSP_IP_ADDRESS, 
                                MmsFlatMap::STRING, MMS_SIZEOF_IPADDRESS);
       if  (ip)                             // If IP address was specified ...
       {                                    // ... copy IP address into map
            length = xmlmsg->isolateParameterValue(ip);
            ACE_OS::strcpy(mapIP, xmlmsg->paramValue());
       }
  } 

  if  (modify)                              
  {    //"modify" flag indicates if modifying attributes of existing connection 
       length = xmlmsg->isolateParameterValue(modify);
       value  = ACE_OS::atoi(xmlmsg->paramValue());

       if  (value == 0 || value == 1)        // Edit as boolean integer
            isModifyConnection = value;
       else
       {    this->turnMqMessageAround(MMS_ERROR_PARAMETER_VALUE, 0, 0);
            break;
       } 
  }

  if  (rexmit)                               
  {    //"retransmit" flag indicates if connection will initially listen to itself 
       length = xmlmsg->isolateParameterValue(rexmit);
       value  = ACE_OS::atoi(xmlmsg->paramValue());
       if (value != 0) isRexmit = 1;     
  }

                                         
  MmsServerCmdHeader commandHeader;         // Trans,conx,conf IDs; cmd timeout
  int  commonresult  = this->buildCommonParameters(map, xmlmsg, commandHeader); 
  int  cparamresult  = this->buildConnectionParameters(map, xmlmsg);
  resultcode = commonresult? commonresult: cparamresult? cparamresult: 0;  

  if  (resultcode)                          // Parameter format error?
  {    this->turnMqMessageAround(resultcode, 0, 0); 
       break;
  }   
   
  value = 0;                                // Session timeout parameter
  if  (xmlmsg->getvaluefor(MmsAppMessageX::paramnames[MmsAppMessageX::SESSION_TIMEOUT])) 
       value = ACE_OS::atoi(xmlmsg->paramValue());
  if  (value)
       map.insert(MMSP_SESSION_TIMEOUT_SECS, value);
                                            // Conference/conferee attributes
  if  (isConference && this->buildConferenceParameters(map, xmlmsg) == -1)
  {    this->turnMqMessageAround(MMS_ERROR_PARAMETER_VALUE, 0, 0); 
       break;
  }   
                                            // Commit the parameter map
  char* flatmap = this->mapComplete(map, commandHeader, COMMANDTYPE_CONNECT);

  if  (isConference)                        // Indicate if conference connection
       setFlatmapFlag(flatmap, MmsServerCmdHeader::IS_CONFERENCE);

  if  (isModifyConnection)                  // Indicate if modifying connection
       setFlatmapFlag(flatmap, MmsServerCmdHeader::IS_MODIFY_CONNECT);

  if  (isRexmit)                            // Indicate if connection self-listens
       setFlatmapFlag(flatmap, MmsServerCmdHeader::IS_REXMIT_CONNECTION);  

                                            // Attach transaction ID ...
  this->postServerCommand(flatmap);         // ... and send command to server

  } while(0);

  if (this->xmlmsg) delete this->xmlmsg;    // Free xml message object
  this->xmlmsg = NULL; 
}  
   


void MmsMqAppAdapter::onCommandDisconnect(void* IpcData)
{  
  // Parse xml for a disconnect request and submit command.
  // If conference ID is supplied, this is assumed to be a disconnect from 
  // conference only; if only connection ID is supplied, an IP disconnect is
  // assumed with a conference disconnect implied if IP is in conference.
  // If only conference ID is supplied, this an abandonment of conference.
  // If neither is supplied, this is a request from application to 
  // disconnect from media server.

  char* conxID = xmlmsg->findparam(MmsAppMessageX::paramnames[MmsAppMessageX::CONNECTION_ID]); 
  char* confID = xmlmsg->findparam(MmsAppMessageX::paramnames[MmsAppMessageX::CONFERENCE_ID]);

  if  (conxID == NULL && confID == NULL)
  {                                         // It's a server disco, so ...
       this->turnMqMessageAround(0, 0, 0);  // ... send acknowledgement ...
       this->registerClient(FALSE, xmlmsg); // ... then close client queue

       delete xmlmsg; xmlmsg = NULL;        // Free xml message object
  }
      
  else this->onGenericCommand(IpcData, COMMANDTYPE_DISCONNECT);
}



void MmsMqAppAdapter::onCommandPlay(void* IpcData)
{ 
  // Parse IPC data for a media play request, extracting parameters and
  // creating the parameter map. 

  // Play can have multiple filespec parameters, and multiple termination 
  // condition parameters. Parameters are represented as a map of maps of maps.
  // A filespec can include not only the filename (and relative path), but also
  // under certain condtions the file offset and segment length.
  // Volume/speed parameters of the adjustPlay command can accompany Play as well.

  this->xmlmsg = (MmsAppMessage*)IpcData;

  MmsFlatMapWriter map(1024);               // Build parameter map 
  MmsLocaleParams localeData;
  MmsServerCmdHeader commandHeader;  
                                            // All the IDs plus timeouts
  const int commonresult = this->buildCommonParameters(map, xmlmsg, commandHeader);
                                            // Termination conditions
  this->buildTerminationConditionParameters(map, xmlmsg);
                                            // Encoding, bitrate
  const int faresult  = this->buildAudioFileAttributeParameters(map, xmlmsg);
                                            // Locale for subdirectory determination
  const int ldresult  = this->buildLocaleDirectoryParameters(map, xmlmsg, localeData);
                                            // File(s) to play
  const int filecount = this->buildFileListParameters(map, xmlmsg, localeData, COMMANDTYPE_PLAY);
                                            // Volume/speed adjustments
  const int vsresult  = this->buildAdjustPlayParameters(map, xmlmsg, FALSE); 

  int  resultcode = commonresult? commonresult: faresult? faresult: 
                    vsresult? vsresult: ldresult? ldresult: 0;  

  if  (filecount == 0) resultcode = MMS_ERROR_TOO_FEW_PARAMETERS;

  if  (resultcode)
       this->turnMqMessageAround(resultcode, 0, 0);
  else                                      // Commit map 
  {    char* flatmap = this->mapComplete(map, commandHeader, COMMANDTYPE_PLAY);                                            
       this->postServerCommand(flatmap);    // Send command to server
  }

  delete this->xmlmsg; this->xmlmsg = NULL;
}



void MmsMqAppAdapter::onCommandPlaytone(void* IpcData)
{ 
  // Parse protocol data for a media play tone request, extracting parameters 
  // and creating the parameter map. Playtone can have zero to two frequency/
  // amplitude parameters, and multiple termination condition parameters. 
  // Parameters are represented as a map of maps.

  this->xmlmsg = (MmsAppMessage*)IpcData;

  MmsFlatMapWriter map;                     // Build parameter map 
  MmsServerCmdHeader commandHeader;  
                                            // All the IDs plus timeouts
  int  commonresult = this->buildCommonParameters(map, xmlmsg, commandHeader);
                                            // Termination conditions
  this->buildTerminationConditionParameters(map, xmlmsg);
                                            // Frequency. amplitude, duration
  int  batprresult  = this->buildAudioToneAttributeParameters(map, xmlmsg);                                          

  int  resultcode = commonresult? commonresult: batprresult? batprresult: 0;  

  if  (resultcode)
       this->turnMqMessageAround(resultcode, 0, 0);
  else                                      // Commit map 
  {    char* flatmap = this->mapComplete(map, commandHeader, COMMANDTYPE_PLAYTONE);                                            
       this->postServerCommand(flatmap);    // Send command to server
  }

  delete this->xmlmsg; this->xmlmsg = NULL;
}



void MmsMqAppAdapter::onCommandRecord(void* IpcData)
{ 
  // Parse protocol data for a media record request, extracting parameters and
  // creating the parameter map. See onCommandPlay for additional comments

  this->xmlmsg = (MmsAppMessage*)IpcData;

  MmsFlatMapWriter map;                     // Build parameter map 
  MmsLocaleParams localeData;
  MmsServerCmdHeader commandHeader;         // All the IDs plus timeouts     
  int  commonresult = this->buildCommonParameters(map, xmlmsg, commandHeader);
                                            // Termination conditions
  this->buildTerminationConditionParameters(map, xmlmsg);
                                            // Encoding, bitrate
  const int  fattrResult = this->buildAudioFileAttributeParameters(map, xmlmsg);
                                            // Locale for subdirectory determination
  const int  ldresult    = this->buildLocaleDirectoryParameters(map, xmlmsg, localeData);
                                            // File path if any
  const int  filecount   = this->buildFileListParameters(map, xmlmsg, localeData, COMMANDTYPE_RECORD);
                                            // File expiration 
  if  (xmlmsg->getvaluefor(MmsAppMessageX::paramnames[MmsAppMessageX::EXPIRES])) 
  {    int expirationDays = ACE_OS::atoi(xmlmsg->paramValue());
       map.insert(MMSP_EXPIRES, expirationDays); 
  }

  int  resultcode = commonresult? commonresult: fattrResult? fattrResult: 
                    ldresult? ldresult: 0; 
  if  (filecount == 0)                      // Unlikely since we provide one here 
       resultcode = MMS_ERROR_TOO_FEW_PARAMETERS;

  if  (resultcode)
       this->turnMqMessageAround(resultcode, 0, 0);
  else                                      // Commit map
  {    char* flatmap = this->mapComplete(map, commandHeader, COMMANDTYPE_RECORD);                                            
       this->postServerCommand(flatmap);    // Send command to server
  }

  delete this->xmlmsg; this->xmlmsg = NULL;
}



void MmsMqAppAdapter::onCommandRecordTransaction(void* IpcData)
{ 
  // Parse protocol data for a media record transaction request, extracting  
  // parameters and creating the parameter map. This is not yet in the test
  // mix. In order to test it, we'll want to set up a command sequence in which
  // we create two connections, then use the connectionID of the second
  // connection to create a record transaction package on the session of the 
  // first connection.
}



void MmsMqAppAdapter::onCommandReceiveDigits(void* IpcData)
{  
  // Parse XML for a receive digits request, extracting parameters, 
  // creating the parameter map. and submitting command to server 

  this->xmlmsg = (MmsAppMessage*)IpcData;

  MmsFlatMapWriter map;                     // Build parameter map 

  MmsServerCmdHeader commandHeader;              
  int  commonResult = this->buildCommonParameters(map, xmlmsg, commandHeader);

  this->buildTerminationConditionParameters(map, xmlmsg);
                                            // Allocate digit buffer
  map.format(MMSP_RECEIVE_DIGITS_RETURN_BUFFER, MmsFlatMap::STRING, 
       MMS_SIZEOF_RECEIVE_DIGITS_RETURN_BUFFER);  

  if  (commonResult != 0)
       this->turnMqMessageAround(commonResult, 0, 0);
  else                                      // Commit map                       
  {    char* flatmap = this->mapComplete(map, commandHeader, COMMANDTYPE_RECEIVE_DIGITS);                                            
       this->postServerCommand(flatmap);    // Send command to server
  }

  delete this->xmlmsg; this->xmlmsg = NULL;
}



void MmsMqAppAdapter::onCommandSendDigits(void* IpcData)
{  
  // Parse XML for a send digits request, extracting parameters, 
  // creating the parameter map. and submitting command to server 

  this->xmlmsg = (MmsAppMessage*)IpcData;

  MmsFlatMapWriter map;                     // Build parameter map 

  MmsServerCmdHeader commandHeader;              
  int  editresult = this->buildCommonParameters(map, xmlmsg, commandHeader); 
                                            
  if  (xmlmsg->getvaluefor(MmsAppMessageX::paramnames[MmsAppMessageX::DIGITS])) 
                                            // Insert digit list parameter
       map.insert(MMSP_DIGITLIST, MmsFlatMap::STRING, 
                  xmlmsg->paramLength()+1, xmlmsg->paramValue()); 

  else editresult = MMS_ERROR_TOO_FEW_PARAMETERS;

  if  (editresult != 0)
       this->turnMqMessageAround(editresult, 0, 0);
  else                                      // Commit map                       
  {    char* flatmap = this->mapComplete(map, commandHeader, COMMANDTYPE_SEND_DIGITS);                                            
       this->postServerCommand(flatmap);    // Send command to server
  }

  delete this->xmlmsg; this->xmlmsg = NULL; 
}

  
                      
void MmsMqAppAdapter::onCommandMonitorCallState(void* IpcData)   
{
  // Parse XML for a monitor call state request, extracting parameters, 
  // creating the parameter map. and submitting command to server 

  this->xmlmsg = (MmsAppMessage*)IpcData;

  MmsFlatMapWriter map;                     // Build parameter map 

  MmsServerCmdHeader commandHeader;              
  int  editresult = this->buildCommonParameters(map, xmlmsg, commandHeader); 
                                            // Silence, nonsilence duration
  int  cspresult  = this->buildCallStateParameters(map, xmlmsg);

  int  result = editresult != 0? editresult: cspresult != 0? cspresult: 0;

  if  (result != 0)
       this->turnMqMessageAround(result, 0, 0);
  else                                      // Commit map                       
  {    char* flatmap = this->mapComplete(map, commandHeader, COMMANDTYPE_MONITOR_CALL_STATE);                                            
       this->postServerCommand(flatmap);    // Send command to server
  }

  delete this->xmlmsg; this->xmlmsg = NULL; 
}



void MmsMqAppAdapter::onCommandStopMediaOperation(void* IpcData)
{
  // Server does not generate a return package on a stop media operation,
  // so this command is launched and forgotten. Adapter will receive the 
  // termination event for the media operation which was canceled.

  this->xmlmsg = (MmsAppMessage*)IpcData;

  MmsFlatMapWriter map;                     // Build parameter map 

  MmsServerCmdHeader commandHeader;              
  int  editresult = this->buildCommonParameters(map, xmlmsg, commandHeader); 

  char* attrvalue = NULL;                          
  char* paramname = MmsAppMessageX::paramnames[MmsAppMessageX::BLOCK]; 

  if  (NULL != (attrvalue = (xmlmsg->getvaluefor(paramname))))
  {
       const int block = ACE_OS::atoi(attrvalue);
       if (block)                           // Unless user asks to stop media 
           map.insert(MMSP_BLOCK, 1);       // synchronously, it runs async 
  }   

  attrvalue = NULL;                          
  paramname = MmsAppMessageX::paramnames[MmsAppMessageX::OPERATION_ID]; 

  if  (NULL != (attrvalue = (xmlmsg->getvaluefor(paramname))))
  {
       const int operationID = ACE_OS::atoi(attrvalue);
       if (operationID)                     // User asks to stop specific operation
           map.insert(MMSP_OPERATION_ID, operationID);        
  }  
                                            // Commit map                       
  char* flatmap = this->mapComplete(map, commandHeader, COMMANDTYPE_STOP_OPERATION);                                            
  this->postServerCommand(flatmap);         // Send command to server
  delete this->xmlmsg; this->xmlmsg = NULL; 
}



void MmsMqAppAdapter::onCommandAdjustPlay(void* IpcData)
{
  // Parse XML for an adjustPlay command, extracting parameters, 
  // creating the parameter map. and submitting command to server 

  this->xmlmsg = (MmsAppMessage*)IpcData;

  MmsFlatMapWriter map;                      
  MmsServerCmdHeader commandHeader;         // Build parameter map

  const int comresult = this->buildCommonParameters(map, xmlmsg, commandHeader);                                             
  const int apresult  = this->buildAdjustPlayParameters(map, xmlmsg, TRUE);
  const int result    = comresult? comresult: apresult? apresult: 0; 

  if  (result != 0)
       this->turnMqMessageAround(result, 0, 0);
  else
  {    char* flatmap = this->mapComplete(map, commandHeader, COMMANDTYPE_ADJUST_PLAY);                                            
       this->postServerCommand(flatmap);    // Send command to server
  }

  delete this->xmlmsg; this->xmlmsg = NULL; 
}



void MmsMqAppAdapter::onCommandVoiceRec(void* IpcData)
{
  // Parse protocol data for voice recognition request, extracting parameters 
  // and creating the parameter map. 
  // Voice recognition request include a list of voice prompt files, grammar files
  // and termination conditions.

  this->xmlmsg = (MmsAppMessage*)IpcData;

  MmsFlatMapWriter map;                     // Build parameter map 
  MmsLocaleParams localeData;
  MmsServerCmdHeader commandHeader;  
                                            // All the IDs plus timeouts
  int  commonresult = this->buildCommonParameters(map, xmlmsg, commandHeader);
                                            // Termination conditions
  this->buildTerminationConditionParameters(map, xmlmsg);
                                            // Encoding, bitrate
  const int fattrresult  = this->buildAudioFileAttributeParameters(map, xmlmsg);
                                            // Locale for subdirectory determination
  const int ldresult     = this->buildLocaleDirectoryParameters(map, xmlmsg, localeData);
                                            // File(s) to play
  const int filecount    = this->buildFileListParameters(map, xmlmsg, localeData, COMMANDTYPE_PLAY);
                                            // Grammar file(s)
  const int grammarcount = this->buildGrammarListParameters(map, xmlmsg);

  char* p = xmlmsg->findparam(MmsAppMessageX::paramnames[MmsAppMessageX::VOICE_BARGEIN]);

  if (p && *p == '1')
  {
      map.format(MMSP_VOICE_BARGEIN, MmsFlatMap::STRING, 2); 
      map.insert(MMSP_VOICE_BARGEIN, MmsFlatMap::STRING, 2, "1");    
  }

  p = xmlmsg->findparam(MmsAppMessageX::paramnames[MmsAppMessageX::CANCEL_ON_DIGIT]);

  if (p && *p == '1')
  {
      map.format(MMSP_CANCEL_ON_DIGIT, MmsFlatMap::STRING, 2); 
      map.insert(MMSP_CANCEL_ON_DIGIT, MmsFlatMap::STRING, 2, "1");    
  }

  int  resultcode = commonresult? commonresult: fattrresult? fattrresult: 0;  
  if  (grammarcount == 0) resultcode = MMS_ERROR_TOO_FEW_PARAMETERS;

                                            // Allocate buffer for returned vr meaning
  map.format(MMSP_VR_MEANING, MmsFlatMap::STRING, MMS_SIZEOF_VR_MEANING);  
                                            // Put in a dummy VR score
  map.insert(MMSP_VR_SCORE, 0);

  if (resultcode)
      this->turnMqMessageAround(resultcode, 0, 0);
  else                                      
  {                                         // Commit map 
      char* flatmap = this->mapComplete(map, commandHeader, COMMANDTYPE_VOICEREC);                                            
      this->postServerCommand(flatmap);     // Send command to server
  }

  delete this->xmlmsg; this->xmlmsg = NULL;
}



void MmsMqAppAdapter::onCommandConferenceResourcesRemaining(void* IpcData)
{
}



void MmsMqAppAdapter::onCommandAssignVolumeAdjustmentDigit(void* IpcData)
{
}



void MmsMqAppAdapter::onCommandAssignSpeedAdjustmentDigit(void* IpcData)
{
}



void MmsMqAppAdapter::onCommandAdjustVolume(void* IpcData) 
{
}


 
void MmsMqAppAdapter::onCommandAdjustSpeed(void* IpcData)
{
}



void MmsMqAppAdapter::onCommandClearVolumeSpeedAdjustments(void* IpcData)
{                     
}              
     


void MmsMqAppAdapter::onCommandConfereeSetAttribute(void* IpcData) 
{
  // Parse protocol data for a conferee attribute request, extracting 
  // parameters and creating the parameter map. 
  // We're expecting a single attribute name specified as a parameter,
  // e.g. <field name="receiveOnly>1</field>, with the value a boolean
  // indicating set or reset

  this->xmlmsg = (MmsAppMessage*)IpcData;

  MmsFlatMapWriter map;                     // Build parameter map: 
  MmsServerCmdHeader cmdHeader;  
  unsigned int editflags = 0;
                                            // Parse common params 
  int  commonresult = this->buildCommonParameters(map, xmlmsg, cmdHeader, &editflags);
                                            // Ensure conference ID specified
  if ((commonresult == 0) && ((editflags & CONFERENCEID_PRESENT) == 0))
       commonresult = MMS_ERROR_TOO_FEW_PARAMETERS;

  int  attrresult = this->buildConfereeAttributeParameter(map, xmlmsg);

  int  resultcode = commonresult? commonresult: attrresult? attrresult: 0; 
  if  (resultcode)
       this->turnMqMessageAround(resultcode, 0, 0);        
  else                                       
  {    char* flatmap = this->mapComplete    // Commit parameter map
            (map, cmdHeader, COMMANDTYPE_CONFEREE_SETATTRIBUTE);                                            
       this->postServerCommand(flatmap);    // Send command to server
  }

  delete this->xmlmsg; this->xmlmsg = NULL;
}


                                              
void MmsMqAppAdapter::onCommandConfereeEnableVolumeControl(void* IpcData)
{
}



void MmsMqAppAdapter::onGenericCommand(void* IpcData, int command)
{
  // Launch a server command that accepts standard parameter set 

  this->xmlmsg = (MmsAppMessage*)IpcData;

  MmsFlatMapWriter map;                     // Build parameter map  

  MmsServerCmdHeader commandHeader;            
  int  commonresult  = this->buildCommonParameters(map, xmlmsg, commandHeader);

  if  (commonresult)
       this->turnMqMessageAround(commonresult, 0, 0);
  else                                      // Commit map
  {    char* flatmap = this->mapComplete(map, commandHeader, command);                                            
       this->postServerCommand(flatmap);    // Send command to server
  }

  delete this->xmlmsg; this->xmlmsg = NULL;
}

