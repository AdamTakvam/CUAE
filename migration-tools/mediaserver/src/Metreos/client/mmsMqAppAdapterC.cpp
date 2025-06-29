//
// MmsMqAppAdapter.cpp 
//
// App server msmq protocol adapter
//
// Media server command return handlers
//
#include "StdAfx.h"
#pragma warning(disable:4786)
#include "mmsMqAppAdapter.h"

#ifdef MMS_ENABLE_MEMORY_LEAK_DETECTION
#define new MMS_MEMLEAK_NEW
#endif // MMS_ENABLE_MEMORY_LEAK_DETECTION



void MmsMqAppAdapter::onReturnConnect(char* flatmap)
{
  // Return result of a connect command to client. Note that a server connect
  // is not sent to the media server, and it therefore does not arrive here 

  MmsAppMessage* outxml = this->writeStandardClientMessageContent
                  (flatmap, MmsAppMessageX::MMSMSG_CONNECT);                                            
  if  (outxml == NULL) return;              // Provisional response, we're done
                                            
  const int mapparam = getFlatmapParam (flatmap);
  const int wasError = isFlatmapFlagSet(flatmap, MmsServerCmdHeader::IS_ERROR);

  if  (mapparam)                            
  {          
       // Return newly-assigned conference ID, munged with our serverID
       const int serverID     = getFlatmapServerID(flatmap);
       const int conferenceID = this->insertServerIdExcludeZero(mapparam, serverID);
       outxml->putParameter(MmsAppMessageX::CONFERENCE_ID, conferenceID);
  }

  if (!wasError)                            // Unless connect did not take ...
  {                                         // Determine if the connect was an
      int  isConferenceOnly                 // existing connection to conference 
        = (isFlatmapFlagSet(flatmap, MmsServerCmdHeader::IS_CONFERENCE)  
       && (isFlatmapFlagSet(flatmap, MmsServerCmdHeader::IS_NOSTARTMEDIA)  
       && (isFlatmapFlagSet(flatmap, MmsServerCmdHeader::IS_EXISTING_CONNECTION))));

      #if(0)
      int  isHalfConnect                     
        = (isFlatmapFlagSet(flatmap, MmsServerCmdHeader::IS_NOSTARTMEDIA)  
      && !(isFlatmapFlagSet(flatmap, MmsServerCmdHeader::IS_EXISTING_CONNECTION)));
      #endif

      if  (!isConferenceOnly)               // Unless conference-only hookup ...                                        
      {                                     // ... return local ip and port
           char* pport = NULL, *pip = NULL;
           int   port = 0;
           MmsFlatMapReader map(flatmap);  

           map.find(MMSP_PORT_NUMBER, &pport);
           map.find(MMSP_IP_ADDRESS,  &pip);

           if  (pport) port = *((int*)pport);
           if  (port)  outxml->putParameter(MmsAppMessageX::PORT, port);
           if  (pip)   outxml->putParameter(MmsAppMessageX::IP_ADDRESS, pip);
      }
                                            // Add updated resource counts to result
      this->formatAvailableMediaResources(outxml);  
  }
  else // ensure server ID present when both connection ID and server ID were zero  
  if (!outxml->getvaluefor(MmsAppMessageX::paramnames[MmsAppMessageX::SERVER_ID]))   
       outxml->putServerID(getFlatmapServerID(flatmap));
   
                                            // Send xml to client via msmq
  this->postClientReturnMessage(&outxml, flatmap);
}



void MmsMqAppAdapter::onReturnDisconnect(char* flatmap)
{
  // Note that a server disconnect is not sent to the media server  
  // and will therefore never arrive here

  MmsAppMessage* outxml 
    = this->writeStandardClientMessageContent(flatmap, MmsAppMessageX::MMSMSG_DISCONNECT);

  if  (outxml)
  {
       this->formatAvailableMediaResources(outxml);  

       this->postClientReturnMessage(&outxml, flatmap);
  }
}                                



void MmsMqAppAdapter::onReturnPlay(char* flatmap)
{ 
  this->onGenericReturn(flatmap, MmsAppMessageX::MMSMSG_PLAY); 
}



void MmsMqAppAdapter::onReturnPlaytone(char* flatmap)
{
  this->onGenericReturn(flatmap, MmsAppMessageX::MMSMSG_PLAYTONE); 
}


void MmsMqAppAdapter::onReturnVoiceRec(char* flatmap)
{
  if (config->serverParams.pointerValidationLevel > 1)
  {   
    if (!Mms::isFlatmapReferenced(flatmap,34)) 
      return;          
  }

  MmsAppMessage* outxml = this->writeStandardClientMessageContent(flatmap, 
                                                                  MmsAppMessageX::MMSMSG_VOICEREC);
  if (outxml == NULL) 
    return;                             // Provisional response, done

  MmsFlatMapReader map(flatmap);        // Retrieve VR meaning from session flatmap and fill it into return xml
  char* buf = NULL;
  map.find(MMSP_VR_MEANING, &buf);
  if (buf) 
    outxml->putParameter(MmsAppMessageX::paramnames[MmsAppMessageX::VR_MEANING], buf);
                                        // Retrieve VR score from session flatmap and fill it into return xml
  map.find(MMSP_VR_SCORE, &buf);
  if (buf) 
    outxml->putParameter(MmsAppMessageX::paramnames[MmsAppMessageX::VR_SCORE], *(int*)buf);

  this->postClientReturnMessage(&outxml, flatmap);
}


void MmsMqAppAdapter::onReturnRecord(char* flatmap)
{
  MmsAppMessage* outxml = this->writeStandardClientMessageContent
                (flatmap, MmsAppMessageX::MMSMSG_RECORD);
  if  (outxml == NULL) return;              // Provisional response, done

  this->insertFilename(outxml, flatmap);    // Insert recorded file path

  this->postClientReturnMessage(&outxml, flatmap);
}



void MmsMqAppAdapter::onReturnRecordTransaction(char* flatmap)
{
  #if 0
  this->onGenericReturn(flatmap, MmsAppMessageX::MMSMSG_RECORDTRANS); 
  #endif
}



void MmsMqAppAdapter::onReturnReceiveDigits(char* flatmap)
{
  if (!Mms::isFlatmapReferenced(flatmap,34)) return;          

  MmsAppMessage* outxml = this->writeStandardClientMessageContent
      (flatmap, MmsAppMessageX::MMSMSG_GETDIGITS);
  if  (outxml == NULL) return;              // Provisional response, done

  MmsFlatMapReader map(flatmap);            // Return digit string
  char* buf = NULL;

  map.find(MMSP_RECEIVE_DIGITS_RETURN_BUFFER, &buf);
  if  (buf) 
      outxml->putParameter(MmsAppMessageX::paramnames[MmsAppMessageX::DIGITS], buf);

  this->postClientReturnMessage(&outxml, flatmap);
}



void MmsMqAppAdapter::onReturnSendDigits(char* flatmap)
{
  this->onGenericReturn(flatmap, MmsAppMessageX::MMSMSG_SENDDIGITS);           
}



void MmsMqAppAdapter::onReturnStopMediaOperation(char* flatmap)
{
  this->onGenericReturn(flatmap, MmsAppMessageX::MMSMSG_STOPMEDIA);           
}



void MmsMqAppAdapter::onReturnAdjustPlay(char* flatmap)
{   
  this->onGenericReturn(flatmap, MmsAppMessageX::MMSMSG_ADJUST_PLAY);
}



void MmsMqAppAdapter::onReturnConferenceResourcesRemaining(char* flatmap)
{
  #if 0
  MmsAppMessage* outxml = this->writeStandardClientMessageContent
                (flatmap, MmsAppMessageX::MMSMSG_CONFRESREMAINING);
  if  (outxml == NULL) return;              
                                            // Resources remaining returned 
  int  mapparam = getFlatmapParam (flatmap);// in map param
  outxml->putParameter(MMS_RETURNRES_NAME, mapparam);

  this->postClientReturnMessage(&outxml, flatmap);
  #endif
}



void MmsMqAppAdapter::onReturnAdjustments(char* flatmap)
{   
  #if 0
  int commandno = 0;
  switch(getFlatmapCommand(flatmap))
  { case COMMANDTYPE_ASSIGN_VOLADJ_DIGIT:   commandno = MMSMSG_ASSIGNVOLDIGIT;  break;
    case COMMANDTYPE_ASSIGN_SPEEDADJ_DIGIT: commandno = MMSMSG_ASSIGNSPEEDDIGIT;break; 
    case COMMANDTYPE_ADJUST_VOLUME:         commandno = MMSMSG_ADJUSTVOL;       break;
    case COMMANDTYPE_ADJUST_SPEED:          commandno = MMSMSG_ADJUSTSPEED;     break;
    case COMMANDTYPE_CLEAR_VS_ADJUSTMENTS:  commandno = MMSMSG_CLEARVOLSPEED;   break;
  }

  if (commandno) 
      this->onGenericReturn(flatmap, commandno);
  #endif
}



void MmsMqAppAdapter::onReturnConfereeSetAttribute(char* flatmap)
{
  this->onGenericReturn(flatmap, MmsAppMessageX::MMSMSG_CONFEREESETATTR); 
}



void MmsMqAppAdapter::onReturnMonitorCallState(char* flatmap)
{
  this->onGenericReturn(flatmap, MmsAppMessageX::MMSMSG_MONITOR_CALL_STATE); 
}



void MmsMqAppAdapter::onReturnConfereeEnableVolumeControl(char* flatmap)
{
  #if 0
  this->onGenericReturn(flatmap, MmsAppMessageX::MMSMSG_CONFENABLEVOLCONTROL); 
  #endif
}



void MmsMqAppAdapter::onGenericReturn(char* flatmap, int commandno)
{
  // Return the result of a server command to client

  MmsAppMessage* outxml 
    = this->writeStandardClientMessageContent(flatmap, commandno);

  if  (outxml)
       this->postClientReturnMessage(&outxml, flatmap);
} 


