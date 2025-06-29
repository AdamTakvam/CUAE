//
// mmsDeviceIP.cpp
//
#include "StdAfx.h"
#ifdef  MMS_WINPLATFORM
#pragma warning(disable:4786)
#endif
#include "mmsDeviceIP.h"
#include "mmsReporter.h"
#include "mmsParameterMap.h"
#include "mmsServerCmdHeader.h"

#ifdef MMS_ENABLE_MEMORY_LEAK_DETECTION
#define new MMS_MEMLEAK_NEW
#endif // MMS_ENABLE_MEMORY_LEAK_DETECTION


                                            // Open IP resource
mmsDeviceHandle MmsDeviceIP::open(OPENINFO& openinfo, unsigned short mode) 
{                                             
  static char *maskError = "%s %s ipm_Open %s\n";
  static char *maskOpen  = "%s opened %s as %d\n";
  const int isLoggingOpenOk = config->serverLogger.globalMessageLevel >= LM_INFO;

  HMPCARDID(deviceID, "ipm", openinfo.key.board, openinfo.key.card);

  if (-1 == (m_handle = ipm_Open(deviceID, 0, EV_SYNC))) 
  {
      ACE_OS::printf  (maskError,devname,em,deviceID);  
      MMSLOG((LM_ERROR,maskError,devname,em,deviceID));     
      return -1;                     
  } 

  MmsAs::openG711++; 
  if (isLoggingOpenOk) printf(maskOpen,devname,deviceID,m_handle);

  if (-1 == this->getLocalMediaInfo())  return -1;

  if (-1 == this->initLocalMediaInfo()) return -1;

  this->init();
  m_deviceState = AVAILABLE;
  m_key = openinfo.key;
  return m_handle;
}


                                            
int MmsDeviceIP::start(const char* ip,  unsigned int port, 
  const unsigned int remoteattrs, const unsigned int localattrs, const int mode)
{
  // Start RTP session
  if (-1 == initRemoteMediaInfo(ip, port, remoteattrs, localattrs, mode)) 
      return -1;

  if (-1 == this->verifyCoderAvailabilityEx(mode, TRUE))
      return MMS_ERROR_RESOURCE_UNAVAILABLE;

  if (-1 == startMedia(mode)) 
      return -1;

  return 0;
}


                                             
int MmsDeviceIP::stop(eIPM_STOP_OPERATION stopwhat)
{
  // Stop operation(s) on this channel. Calls ipm_UnListen internally
  int result = ipm_Stop(m_handle, stopwhat, EV_SYNC);
  if (result == -1)
      MMSLOG((LM_ERROR,"%s %s ipm_Stop(%d) %s\n",
              devname,em,stopwhat,ATDV_ERRMSGP(m_handle)));  

  m_mediaState = IPMEDIASTOPPED; 
 
  this->trackLowBitrateUsage(MmsAs::RESX_INC);  
        
  return result;
}                    


                                            
int MmsDeviceIP::listen(SC_TSINFO* slotInfo)   // Listen on timeslot
{
  // Connect device receive channel to specified SCBus timeslot
  // allowing media to stream between timeslot and IP network
  int  result = ipm_Listen(m_handle, slotInfo, EV_SYNC);

  if  (result == -1)
       MMSLOG((LM_ERROR,"%s %s ipm_Listen %s\n",
               devname,em,ATDV_ERRMSGP(m_handle)));
  else 
  {    m_mediaState = IPMEDIALISTENING;

       if  (m_config->diagnostics.flags & MMS_DIAG_LOG_LISTENS)
            this->logListening(slotInfo);
  }
   
  return result;
}



int MmsDeviceIP::unlisten()                 // Stop listening on timeslot 
{  
  int  result = ipm_UnListen(m_handle, EV_SYNC);

  if  (result == -1)
       MMSLOG((LM_ERROR,"%s %s ipm_UnListen %s\n",devname,em,ATDV_ERRMSGP(m_handle)));
  else
  if ((m_config->diagnostics.flags & MMS_DIAG_LOG_LISTENS)
    && m_mediaState == IPMEDIALISTENING)
       this->logUnlistening();
                                            // Clear self-listen state
  this->flags &= ~DEVICEFLAGS_IS_SELF_LISTENING;
 
  m_mediaState = IPMEDIASTARTED;            // Revert to idle 

  return result;
}



int MmsDeviceIP::startMedia(const int mode) // Start RTP streaming
{ 
  const int syncmode = mode & MmsMediaDevice::ASYNC? EV_ASYNC: EV_SYNC;
  
  int  result = ipm_StartMedia
               (m_handle, &m_mediaInfoRemote, m_dataflowDirection, syncmode);

  if  (result != 0)
       MMSLOG((LM_ERROR,"%s %s ipm_StartMedia %s\n",devname,em,ATDV_ERRMSGP(m_handle))); 
  else 
  {    m_mediaState = IPMEDIASTARTED;  

       this->trackLowBitrateUsage(MmsAs::RESX_DEC, mode);     
  }
 
  return result;
} 



void MmsDeviceIP::close() 
{  
  if (m_handle > 0)                         // Stop invokes unlisten fyi  
  {   
      if (!this->isStopped())
           this->stop();

      ipm_Close(m_handle, NULL); 
  }

  m_mediaState  = IPMEDIASTOPPED;
  m_deviceState = CLOSED;
  m_handle = 0;
}



int MmsDeviceIP::timeslot(SC_TSINFO* slotinfo) 
{
  slotinfo->sc_numts = 1;
  slotinfo->sc_tsarrayp = &m_timeslotNumber;

  int  result = ipm_GetXmitSlot(m_handle, slotinfo, EV_SYNC);         

  if  (result == -1)
       MMSLOG((LM_ERROR,"%s %s ipm_GetXmitSlot %d\n",devname,em,m_handle));     
    
  return result;
} 


                                             
int MmsDeviceIP::selflisten()               // Listen on our own timeslot
{
  // Connect our receive channel to our own SCBus xmit timeslot

  SC_TSINFO tsinfo;   

  int result  = this->timeslot(&tsinfo);    // Get our xmit timeslot 

  if (result != -1)                                                                            
      result  = this->listen(&tsinfo);      // Listen to it

  if (result != -1)
      this->flags |= DEVICEFLAGS_IS_SELF_LISTENING;
   
  return result;
}



int MmsDeviceIP::getLocalMediaInfo()
{ 
  // Retrieves properties for the local media channel, including the port and
  // IP address that HMP is listening on (assigned during firmware download)

  int  result = ipm_GetLocalMediaInfo(m_handle, &m_mediaInfoLocal, EV_SYNC);

  if  (result == -1) 
       MMSLOG((LM_ERROR,"%s %s ipm_GetLocalMediaInfo\n",devname,em));  

  else MMSLOG((LM_DEBUG,"%s IP dvc %d %s port %d\n",
       devname, m_handle, localIP(), localPort()));

  return result;
}



int MmsDeviceIP::initRemoteMediaInfo(const char* ip, unsigned int port, 
  const unsigned int remoteattrs, const unsigned int localattrs, int mode)
{ 
  // Set remote and local connectivity selections. Set media properties.
  m_mediaInfoRemote.unCount = 4; 
                                            // Set up RTP information
  rtpInfoRemote.eMediaType = MEDIATYPE_REMOTE_RTP_INFO;

  if  (m_config->media.verifyRemotePortMod2 && (port & 1))
  {    MMSLOG((LM_ERROR,"%s remote port (%d) not modulo 2\n",devname,port));
       return -1;
  }

  rtpInfoRemote.mediaInfo.PortInfo.unPortId = port; 
    
  strncpy(rtpInfoRemote.mediaInfo.PortInfo.cIPAddress, ip, IP_ADDR_SIZE-1);
                                            // Set up RTCP info
  rtcpInfoRemote.eMediaType = MEDIATYPE_REMOTE_RTCP_INFO;
                                            // Convention: RTCP port = RTP port+1
  rtcpInfoRemote.mediaInfo.PortInfo.unPortId = port + 1;
  strncpy(rtcpInfoRemote.mediaInfo.PortInfo.cIPAddress, ip, IP_ADDR_SIZE-1); 

  IPM_MEDIA* remoteInfo = &codecRemote;     // symbols for the debugger  
  IPM_MEDIA* localInfo  = &codecLocal;  
  IPM_MEDIA* rtpInfo    = &rtpInfoRemote;
  IPM_MEDIA* rtcpInfo   = &rtcpInfoRemote;    
  
  // Set remote coder parameters:
  codecRemote.eMediaType = MEDIATYPE_REMOTE_CODER_INFO;

  this->setCoderInfo(codecRemote.mediaInfo.CoderInfo, remoteattrs, FALSE);

  codecRemote.mediaInfo.CoderInfo.unCoderPayloadType 
     = MMS_CODER_PAYLOAD_TYPE_GET(remoteattrs);
                                             
  #if 0
  codecRemote.mediaInfo.CoderInfo.unRedPayloadType   
     = MMS_REDUNDANCY_PAYLOAD_TYPE_GET(remoteattrs);
  #else

  // We initially had only one data flow direction type. Now we have 5.
  // Rather than define and edit an extra media server parameter, we're
  // using these bits previously reserved for redundancy payload type,
  // since we currently have no use for that. If at some point we have a
  // use for redundancy payload type, we should probably define an extra 
  // parameter in which to pass data flow direction.

  codecRemote.mediaInfo.CoderInfo.unRedPayloadType = 0;  

  switch(MMS_DATAFLOW_DIRECTION_GET(remoteattrs))
  { case MMS_DIRECTION_IPRO: m_dataflowDirection = DATA_IP_RECEIVEONLY;   break;     
    case MMS_DIRECTION_IPSO: m_dataflowDirection = DATA_IP_SENDONLY;      break;     
    case MMS_DIRECTION_MCS:  m_dataflowDirection = DATA_MULTICAST_SERVER; break;
    case MMS_DIRECTION_MCC:  m_dataflowDirection = DATA_MULTICAST_CLIENT; break; 
    case MMS_DIRECTION_IPBI:  
    default: m_dataflowDirection = DATA_IP_TDM_BIDIRECTIONAL;
  }

  #endif


  // Set local coder parameters:
  codecLocal.eMediaType = MEDIATYPE_LOCAL_CODER_INFO;

  // We're assuming here that any local coder parameters not explicitly
  // specified, will assume the values of their remote counterparts.

  // First set values for coder type, framesize, and frames per packet 
  // for the case where nonzero values were specified for any of these.
  this->setCoderInfo(codecLocal.mediaInfo.CoderInfo, localattrs, TRUE);
                 
  // Then use the corresponding value for the remote coder when any was zero
  int wasLocalCoderSpecified = TRUE;

  switch(MMS_GET_CODER_BITS(localattrs))
  {
    case 0:
    case CODER_TYPE_NONSTANDARD: 
         wasLocalCoderSpecified = FALSE;

         codecLocal.mediaInfo.CoderInfo.eCoderType   
           = codecRemote.mediaInfo.CoderInfo.eCoderType;

         codecLocal.mediaInfo.CoderInfo.eVadEnable 
           = codecRemote.mediaInfo.CoderInfo.eVadEnable; 
  }

  if (!wasLocalCoderSpecified || MMS_GET_FRAMESIZE_BITS(localattrs) == 0)
  {   
      codecLocal.mediaInfo.CoderInfo.eFrameSize   
        = codecRemote.mediaInfo.CoderInfo.eFrameSize; 
 
      codecLocal.mediaInfo.CoderInfo.unFramesPerPkt  
        = codecRemote.mediaInfo.CoderInfo.unFramesPerPkt;   
  }  

  codecLocal.mediaInfo.CoderInfo.unCoderPayloadType 
    = MMS_CODER_PAYLOAD_TYPE_GET(localattrs)?
      MMS_CODER_PAYLOAD_TYPE_GET(localattrs):
      codecRemote.mediaInfo.CoderInfo.unCoderPayloadType;

  codecLocal.mediaInfo.CoderInfo.unRedPayloadType  
    = codecRemote.mediaInfo.CoderInfo.unRedPayloadType;

                                            // Do extended logging if configured
  if  (m_config->diagnostics.flags & MMS_DIAG_LOG_CONNECT_ATTRS)
       logMediaInfo(remoteInfo, localInfo, rtpInfo, rtcpInfo);

  return 0;
} 



void MmsDeviceIP::setCoderInfo
( IPM_CODER_INFO& coderInfo, const unsigned int attrs, const int isLocal)
{
  // Set up coder information. See IP media docs for IPM_CODER_INFO.
  // For G.711, frames per packet is fixed at 1, and framesize can be 10, 20, 30
  // For G.723 and G.729, framesize is fixed at 30 and fpp can be 2, 3, or 4.
  // However we are using framesize on the front end to indicate frames per 
  // packet for the low bitrate coders, as follows:
  // for G.723, framesize 30 translates to frames per packet 2
  //                ""    60        ""           ""          3
  // for G.729, framesize 20 translates to frames per packet 2
  //                ""    30        ""           ""          3
  //                ""    40        ""           ""          4

  // HMP coder type constants for reference:
  // CODER_TYPE_NONSTANDARD			// CODER_TYPE_G7231ANNEXCCAP	 
	// CODER_TYPE_G711ALAW64K			// CODER_TYPE_G726_16K 
	// CODER_TYPE_G711ALAW56K			// CODER_TYPE_G726_24K 
	// CODER_TYPE_G711ULAW64K			// CODER_TYPE_G726_32K 
	// CODER_TYPE_G711ULAW56K			// CODER_TYPE_G726_40K 
	// CODER_TYPE_G721ADPCM			  // CODER_TYPE_G728
	// CODER_TYPE_G722_48K				// CODER_TYPE_G729 
	// CODER_TYPE_G722_56K			  // CODER_TYPE_G729ANNEXA	
	// CODER_TYPE_G722_64K				// CODER_TYPE_G729WANNEXB 
	// CODER_TYPE_G7231_5_3K			// CODER_TYPE_G729ANNEXAWANNEXB 
	// CODER_TYPE_G7231_6_3K	
		 
  // MMS coder type constants for reference:
  // MMS_CODER_G711ALAW64K      // MMS_CODER_G729 
  // MMS_CODER_G711ULAW64K      // MMS_CODER_G729ANNEXA        
  // MMS_CODER_G7231_5_3K       // MMS_CODER_G729ANNEXB        
  // MMS_CODER_G7231_6_3K       // MMS_CODER_G729ANNEXAB   

  // We assume that the specified coder has been vetted and defaulted   
  // at the adapter such that if we get this far, coder type is valid. 

  // It is assumed that caller sets remote coder attributes before
  // local, and that it is desired that local attributes, if not
  // specified, will eventually default to corresponding remote attributes.

  // Note that the logged diagnostic coder info will show the
  // framesize we send to HMP, which for LBR coders is not the same
  // as the framesize specified by media server client. As explained
  // above, for LBR coders, client framesize is used to map to frames per 
  // packet, but HMP framesize is constant. So for example G.729 will
  // always show as framesize 10 in the log, which is the framesize
  // expected by HMP, whereas the client specified framesize 20, 30,
  // or 40, which we used to map to fpp 2, 3, or 4. 
        
  coderInfo.eCoderType 
     = attrs & MMS_CODER_G7231_5_3K?  CODER_TYPE_G7231_5_3K:
       attrs & MMS_CODER_G7231_6_3K?  CODER_TYPE_G7231_6_3K:
       attrs & MMS_CODER_G729?        CODER_TYPE_G729:
       attrs & MMS_CODER_G729ANNEXA?  CODER_TYPE_G729ANNEXA:
       attrs & MMS_CODER_G729ANNEXB?  CODER_TYPE_G729WANNEXB:
       attrs & MMS_CODER_G729ANNEXAB? CODER_TYPE_G729ANNEXAWANNEXB:
       attrs & MMS_CODER_G711ULAW64K? CODER_TYPE_G711ULAW64K: 
       attrs & MMS_CODER_G711ALAW64K? CODER_TYPE_G711ALAW64K: 
       isLocal? CODER_TYPE_NONSTANDARD:
       CODER_TYPE_G711ULAW64K;

  switch(coderInfo.eCoderType)
  {
      case CODER_TYPE_G711ULAW64K:     // G.711
      case CODER_TYPE_G711ALAW64K:

           coderInfo.unFramesPerPkt = 1;

           coderInfo.eFrameSize  
             = attrs & MMS_CODER_G711_FRAMESIZE_30? CODER_FRAMESIZE_30:
               attrs & MMS_CODER_G711_FRAMESIZE_20? CODER_FRAMESIZE_20:
               attrs & MMS_CODER_G711_FRAMESIZE_10? CODER_FRAMESIZE_10:
	             CODER_FRAMESIZE_20; 

           coderInfo.eVadEnable = CODER_VAD_DISABLE; 
           break;

      case CODER_TYPE_G7231_5_3K:      // G.723
      case CODER_TYPE_G7231_6_3K:
                                       // Frame size fixed at 30 per spec
           coderInfo.eFrameSize = CODER_FRAMESIZE_30;
                                       // Client's frame size specification
           coderInfo.unFramesPerPkt    // is used only to indicate fpp
             = attrs & MMS_CODER_G723_FRAMESIZE_30? 2:
               attrs & MMS_CODER_G723_FRAMESIZE_60? 3:
	             2;  // default (CCM default is fs 30)

           coderInfo.eVadEnable   // per IPM_CODER_INFO docs
                 = attrs & MMS_CODER_VAD_ENABLE? 
                     CODER_VAD_ENABLE: CODER_VAD_DISABLE;
           break;

      case CODER_TYPE_NONSTANDARD:     // Not specified
           break;

      default:                         // G.729  
                                       // Frame size fixed at 10 per spec
           coderInfo.eFrameSize = CODER_FRAMESIZE_10;
                                       // Client's frame size specification
           coderInfo.unFramesPerPkt    // is used only to indicate fpp
             = attrs & MMS_CODER_G729_FRAMESIZE_20? 2:
               attrs & MMS_CODER_G729_FRAMESIZE_30? 3:
               attrs & MMS_CODER_G729_FRAMESIZE_40? 4:
	             2;  // default (CCM default is fs 20)

           coderInfo.eVadEnable   // per IPM_CODER_INFO docs
              = coderInfo.eCoderType == CODER_TYPE_G729ANNEXA?
                     CODER_VAD_DISABLE: CODER_VAD_ENABLE;                    
  }
}



int MmsDeviceIP::verifyCoderAvailabilityEx(const int mode, const int isLog)
{
  // Verifies that requested coder is available.
  // Sets and reports unavailability if requested.
  // Modified for OEM licensing enforcement

  if  (mode & MmsMediaDevice::NOAQUIRE_RESOURCE); // Pre-reserved?
  else
  if  (this->isLowBitrateCoder(codecRemote.mediaInfo.CoderInfo)
    || this->isLowBitrateCoder(codecLocal.mediaInfo.CoderInfo))
  {     
       if (MmsAs::g729(MmsAs::RESX_ISZERO))
           return this->raiseLowBitrateExhaustedAlarm();
  }

  return 0; 
}



int MmsDeviceIP::raiseLowBitrateExhaustedAlarm()
{
  // Fire an alarm indicating LBR ports exhausted, also log the condition

  char alarmDescription[MmsAlarmParams::maxTextLength+1];
  sprintf(alarmDescription, MmsAs::szResxExhausted, "low bitrate");

  MMSLOG((LM_ERROR,"%s %s\n", devname, alarmDescription));

  MmsAlarmParams* params = new MmsAlarmParams(MmsReporter::NDX_NO_MORE_G729_ETC, 
     MMS_STAT_CATEGORY_G729, MmsReporter::severityTypeSevere);
  params->settext(alarmDescription);

  MmsReporter::instance()->postMessage(MMSM_ALARMREQUEST, (long)params);

  return -1;
}



void MmsDeviceIP::trackLowBitrateUsage(const int incOrDec, const int mode)
{
  // Tracks whether this connection uses a low bitrate resource.
  // Also keeps track of total low bit rate resources currently in use.
  // The LBR resource may have been reserved at half-connect time;
  // if so the LBR availability count was updated with the reservation,
  // and we will note that fact and not update the count again here.

  if   (this->isLowBitrateCoder(codecRemote.mediaInfo.CoderInfo)
     || this->isLowBitrateCoder(codecLocal.mediaInfo.CoderInfo))
  {     
    if  (incOrDec == MmsAs::RESX_DEC)       // Decrement available LBR resources
    {                                        
         this->flags |= DEVICEFLAGS_IS_LOWBITRATE;

         const int wasLbrResourcePreviouslyReserved 
             = (mode & MmsMediaDevice::NOAQUIRE_RESOURCE) != 0;
                                            // LICX G729- (2 of 2)
         const int result = wasLbrResourcePreviouslyReserved? 0:
                   Mms::modifyLbrAvailableCount(-1);                 
    }
    else                                    // Increment available LBR resources
    {    this->flags &= ~DEVICEFLAGS_IS_LOWBITRATE;
                                            // LICX G729+ (2 of 2)
         const int result = Mms::modifyLbrAvailableCount(+1);       
    }
  }
  else this->flags &= ~DEVICEFLAGS_IS_LOWBITRATE; 
}



int MmsDeviceIP::isLowBitrateCoder(IPM_CODER_INFO& coderInfo)
{
  switch(coderInfo.eCoderType)
  {
    case CODER_TYPE_G711ULAW64K: 
    case CODER_TYPE_G711ALAW64K: 
         return false;
  }

  return true;
}

                                            // Convenience accessors
char* MmsDeviceIP::localIP()     { return rtpInfoLocal.mediaInfo.PortInfo.cIPAddress;}
int   MmsDeviceIP::localPort()   { return rtpInfoLocal.mediaInfo.PortInfo.unPortId; }
int   MmsDeviceIP::localCoder()  { return codecLocal.mediaInfo.CoderInfo.eCoderType; } 
int   MmsDeviceIP::localFrsize() { return codecLocal.mediaInfo.CoderInfo.eFrameSize; } 
int   MmsDeviceIP::localVad()    { return codecLocal.mediaInfo.CoderInfo.eVadEnable; } 

char* MmsDeviceIP::remoteIP()    { return rtpInfoRemote.mediaInfo.PortInfo.cIPAddress;}
int   MmsDeviceIP::remotePort()  { return rtpInfoRemote.mediaInfo.PortInfo.unPortId; }
int   MmsDeviceIP::remoteCoder() { return codecRemote.mediaInfo.CoderInfo.eCoderType; } 
int   MmsDeviceIP::remoteFrsize(){ return codecRemote.mediaInfo.CoderInfo.eFrameSize; } 
int   MmsDeviceIP::remoteVad()   { return codecRemote.mediaInfo.CoderInfo.eVadEnable; }
int   MmsDeviceIP::dataflowDirection() { return m_dataflowDirection; }
 


int MmsDeviceIP::initLocalMediaInfo()
{ 	
  return 0;
}


int MmsDeviceIP::startReceiveDigits()
{  
  // By default HMP uses G.711 codec, but if user wants to use compressed
  // audio (G.723, G.729a) they may want to collect encode DTMF over the 
  // IP channel instead of voice. Our config option to switch this on will
  // be something that means "Use RFC2833 RTP Payload for DTMF Transport".
  // Since we do not have the termination conditions happening on the IP
  // device, we'll need to accept a number of digits and a timeout in the
  // call from client. Session will accumulate digits until timeout or
  // number of digits is reached, and then call this->stopReceiveDigits
  // Note that as of this writing, HMP is not supporting ipm_ReceiveDigits,
  // however we have it included in the API and will support as soon as
  // Dialogic does.
  IPM_DIGIT_INFO digitInfo;
  digitInfo.eDigitType = DIGIT_ALPHA_NUMERIC;
  digitInfo.eDigitDirection = DIGIT_TDM;

  return this->startReceiveDigits(&digitInfo);
} 



int MmsDeviceIP::startReceiveDigits(IPM_DIGIT_INFO* info)
{ 
  // NOTE that the session will register for the IPMEV_DIGITS_RECEIVED 
  // event prior to calling here.

  // Prepare IP channel to receive encoded digits. Actual digits received async
  if  (-1 == ipm_ReceiveDigits(m_handle, info, EV_SYNC)) 
  {
       MMSLOG((LM_ERROR,"%s %s ipm_ReceiveDigits\n",devname,em));     
       return -1;                     
  }  
   
  return 0;
} 



int MmsDeviceIP::stopReceiveDigits()
{
  // It seems to be the case that ipm_Stop invokes unlisten, and so we
  // probably must set up a listen again after calling here by doing a
  // voiceResource->busConnect(this, FULLDUPLEX);
  
  const int result = ipm_Stop(m_handle, STOP_RECEIVE_DIGITS, EV_SYNC);
  if  (result == -1) 
       MMSLOG((LM_ERROR,"%s %s ipm_Stop(STOP_RECEIVE_DIGITS)\n",devname,em));     

  return result;
}



int MmsDeviceIP::sendDigits(const char* digitlist, const int numdigits, const int mode)
{ 
  IPM_DIGIT_INFO info; memset(&info, 0, sizeof(IPM_DIGIT_INFO));
  info.eDigitType       = DIGIT_ALPHA_NUMERIC;  
  info.eDigitDirection  = DIGIT_TDM; 
  info.unNumberOfDigits = numdigits;

  // Intel support: ipm_SendDigits(), by default, generates a 200 ms DTMF and for the 
  // command to be processed by the firmware takes another 30 ms. By setting the duration 
  // to a shorter value, HMP will generate a shorter PCM wav form for the DTMF tone.
  // JDL 03/21/06 set DTMF digit duration per dialogic's suggestion: 200ms is default

  info.unDuration = 40 * numdigits;       
  memcpy(info.cDigits, digitlist, numdigits);

  const int result = ipm_SendDigits(m_handle, &info, mode);

  if  (result == -1)
       MMSLOG((LM_ERROR,"%s %s ipm_SendDigits %s\n",devname,em,ATDV_ERRMSGP(m_handle))); 
  else // JDL per dialogic support give firmware time to process digit
       mmsSleep(MMS_N_MS(100)); 

  return result; 
} 



int MmsDeviceIP::toggleRfc2833Events(const int isEnabling)
{  
  #ifdef MMS_HMP_1X_IPM_HEADERS

  eIPM_EVENT events[1] = { EVT_RFC2833 }; 

  #else  // MMS_HMP_1X_IPM_HEADERS

  // For HMP 3.0, the preferred method for enabling RFC2833 DTMF info is to 
  // ipm_EnableEvents() with EVT_TELEPHONY. When RFC2833 info is received by HMP,
  // an IPMEV_TELEPHONY_EVENT will be generated, and not IPMEV_RFC2833SIGNALRECEIVED
  // as previously. 

  eIPM_EVENT events[1] = { EVT_TELEPHONY };

  #endif // MMS_HMP_1X_IPM_HEADERS
  
  static const char* cen = "ipm_EnableEvents", *cdis = "ipm_DisableEvents";

  int result = isEnabling?
               ipm_EnableEvents (m_handle, events, 1, EV_ASYNC):
               ipm_DisableEvents(m_handle, events, 1, EV_ASYNC);

  if (result == -1)
      MMSLOG((LM_ERROR,"%s %s %s %s\n", devname, em, 
              isEnabling? cen: cdis, ATDV_ERRMSGP(m_handle)));

  return result;
}



int MmsDeviceIP::getDigitFromRfc2833EventData(void* eventdata)
{
  // Extract digit from data arriving with RFC2833 signal event.
  // Returns the digit itself, or -1 to indicate QoS signal state is not OFF,
  // event is not an RFC2833 event, or event data is null.

  if (eventdata == NULL) return -1;

  #ifdef MMS_HMP_1X_IPM_HEADERS   
  
  // This code is used when we are building against HMP 1.3 headers, but HMP itself
  // is using the HMP 3.0 header, that is, RFC2833 event data is IPM_TELEPHONY_INFO
  // but we do not have a header including that structure definition.
  // Following is a hack to point to the correct location of the RFC2833 digit code.
  // Note that to activate inbound DTMF, add config item "Media.rfc2833Enable = 1"

  IPM_RFC2833_SIGNALID_INFO* sinfo = (IPM_RFC2833_SIGNALID_INFO*) eventdata;
  int* pdigitcode = (int*)sinfo+3;          // Offset hack described above
  const int digitCode = *pdigitcode;
  const int isSignalStateOff = sinfo->eState == 0;
  if (!isSignalStateOff) return -1;         // According to dialogic sample code

  #else  // #ifdef MMS_HMP_1X_IPM_HEADERS 

  // This code is used when we are building against the correct HMP 3.0+ headers

  IPM_TELEPHONY_INFO* tinfo = (IPM_TELEPHONY_INFO*) eventdata;

  if (tinfo->eTelInfoType  != TEL_INFOTYPE_EVENT) return -1;

  IPM_TELEPHONY_EVENT_INFO* einfo = &tinfo->TelephonyInfo.TelEvtInfo;
  const int digitCode = einfo->eTelephonyEventID;

  #endif // #ifdef MMS_HMP_1X_IPM_HEADERS 
  
  int digit = 0;
  
  if (digitCode < 0 || digitCode > SIGNAL_ID_EVENT_DTMF_POUND)
      digit = -1;                              // Bogus data
  else                                         // see ipmlib.h in HMP/inc
  if (digitCode <= SIGNAL_ID_EVENT_DTMF_9)     // digits 0 - 9 (0x0 thru 0x9)
      digit = digitCode + 48;                  
  else 
  if (digitCode >= SIGNAL_ID_EVENT_DTMF_A)     // digits A, B, C, D (0xc thru 0xf)
      digit = digitCode + 53;
  else 
  if (digitCode == SIGNAL_ID_EVENT_DTMF_STAR)  // digit * (0xa)
      digit = 42;
  else 
  if (digitCode == SIGNAL_ID_EVENT_DTMF_POUND) // digit # (0xb)
      digit = 35;

  return digit;
}



long MmsDeviceIP::getRfc2833EventType()
{
  // Return HMP event type for RFC2833 event, depending on HMP version

  #ifdef MMS_HMP_1X_IPM_HEADERS
  const static long rfc2833eventType = IPMEV_RFC2833SIGNAL_RECEIVED; // HMP 1.3 
  #else
  const static long rfc2833eventType = IPMEV_TELEPHONY_EVENT;        // HMP 2.0+
  #endif

  return rfc2833eventType;
}



int MmsDeviceIP::getSessionStats(IPM_SESSION_INFO* info)
{
  const int result = ipm_GetSessionInfo(m_handle, info, EV_SYNC);
  if  (result == -1) 
       MMSLOG((LM_ERROR,"%s %s ipm_GetSessionInfo\n",devname,em));     
  return result;
}



void MmsDeviceIP::logMediaInfo
( IPM_MEDIA* remote, IPM_MEDIA* local, IPM_MEDIA* rtp, IPM_MEDIA* rtcp)
{
  // Write current media parameter diagnostics to log
  if  (remote) logCoderInfo(remote, FALSE);
  if  (local)  logCoderInfo(local,  TRUE);
  if  (!rtp || !rtcp) return;
  int   rtpport  = rtp ->mediaInfo.PortInfo.unPortId;
  char* rtpip    = rtp ->mediaInfo.PortInfo.cIPAddress;
  int   rtcpport = rtcp->mediaInfo.PortInfo.unPortId;
  char* rtcpip   = rtcp->mediaInfo.PortInfo.cIPAddress;

  char* dir = "bi";
  switch(m_dataflowDirection)
  { case DATA_IP_RECEIVEONLY:   dir = "ro"; break;
    case DATA_IP_SENDONLY:      dir = "so"; break;
    case DATA_MULTICAST_SERVER: dir = "ms"; break;
    case DATA_MULTICAST_CLIENT: dir = "mc"; break;
  }

  static const char mask[] ="%s rtp %s/%d rtcp %s/%d dir %s\n";
  MMSLOG((LM_DEBUG,mask,devname,rtpip,rtpport,rtcpip,rtcpport,dir));      
}



void MmsDeviceIP::logCoderInfo(IPM_MEDIA* info, const int isLocal)
{
  // Write current coder parameter diagnostics to log
  static const char mask[] ="%s %s %s framsz %d fpp %d vad %d\n"; 
  const  char* which = isLocal? "local ":"remote";
  int framesize = info->mediaInfo.CoderInfo.eFrameSize;

  char* type = "?";
  switch(info->mediaInfo.CoderInfo.eCoderType)
  { case CODER_TYPE_G711ULAW64K: type = "g711u";   break;
    case CODER_TYPE_G711ALAW64K: type = "g711a";   break;
    case CODER_TYPE_G7231_5_3K:  type = "g723/5.3";break;
    case CODER_TYPE_G7231_6_3K:  type = "g723/6.3";break;
    case CODER_TYPE_G729ANNEXA:  type = "g729a";   break;
    case CODER_TYPE_G729WANNEXB: type = "g729b";   break;
    case CODER_TYPE_G729ANNEXAWANNEXB: type = "g729ab"; 
  }

  framesize = framesize == CODER_FRAMESIZE_20? 20:
              framesize == CODER_FRAMESIZE_10? 10:
              framesize == CODER_FRAMESIZE_30? 30: 0; 
                   
  const int fpp = info->mediaInfo.CoderInfo.unFramesPerPkt;
  const int vad = info->mediaInfo.CoderInfo.eVadEnable == CODER_VAD_ENABLE;

  MMSLOG((LM_DEBUG,mask,devname,which,type,framesize,fpp,vad));     
}
 


int MmsDeviceIP::init()
{ 
  // Initialize this object
  unsigned long parmval = 0; 
  IPM_PARM_INFO info;
  info.pvParmValue = &parmval;

  if (m_config->media.rfc2833Enable)
  {
      info.eParm = PARMCH_DTMFXFERMODE;
      parmval    = DTMFXFERMODE_RFC2833;
      this->setDeviceParam(info);
  }

  if (m_config->media.agcDisableIP)  
  {
      info.eParm = PARMCH_AGCACTIVE;
      parmval    = AGCACTIVE_OFF;
      this->setDeviceParam(info);
  }

  if (m_config->media.toneClampDisable)
  {
      info.eParm = PARMCH_RFC2833MUTE_AUDIO;
      parmval    = RFC2833MUTE_AUDIO_OFF;
      this->setDeviceParam(info);
  }

  if (m_config->hmp.setDscpExpediteForward)
  {   // Mark voice packets with DSCP expedite forward if so configured 
      info.eParm = PARMCH_TOS;
      parmval    = DSCP_EXPEDITE_FORWARD_VALUE;
      this->setDeviceParam(info);
  }

  return 0;
}



int MmsDeviceIP::setDeviceParam(IPM_PARM_INFO& info)
{
  const int result = ipm_SetParm(m_handle, &info, EV_SYNC);

  if (result < 0)
      MMSLOG((LM_ERROR,"%s %s ipm_SetParm %s\n",devname,em,ATDV_ERRMSGP(m_handle))); 

  return result;
}


                                            // Ctor
MmsDeviceIP::MmsDeviceIP(int ordinal, MmsConfig* config): 
  MmsMediaDevice(DEVICETYPE::IP, ordinal, config)
{ 
  ACE_OS::strcpy(devname,"DEVI"); 
  this->config = config;
  this->flags = 0;
}



MmsDeviceIP::~MmsDeviceIP()                 // Dtor
{   
}


