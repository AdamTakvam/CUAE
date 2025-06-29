#include "stdafx.h"
#include <srllib.h>
#include <gclib.h>
#include <ipmlib.h>
#include <dtilib.h>
#include <dxxxlib.h>
#include <msilib.h>
#include <dcblib.h>
#include <errno.h>
#include <conio.h>
#include <fcntl.h>

#define CARDIDMASK "%sB%dC%d"
#define HMPCARDID(name,prefix,board,card) char name[16]; \
   sprintf(name,CARDIDMASK,prefix,board,card);
#define DEVICEIDMASK "%sB%dD%d"
#define HMPDEVICEID(name,prefix,board,device) char name[16]; \
   sprintf(name,DEVICEIDMASK,prefix,board,device)

int  result, handleIP, handleConf, handleVox, conferenceID, remaining;
long iptimeslotnum, listentimeslot, voiceHandles[16], ipHandles[16];

SC_TSINFO      slotinfo; 
MS_CDT         cdt; 
DX_IOTT        iott[16];
DX_XPB         xpb[16]; 
IPM_MEDIA_INFO mediaInfoLocal[16];
IPM_MEDIA_INFO mediaInfoRemote[16]; 
char c, *ipKey="ipmB1C1", *confKey="dcbB1D1";
typedef long (*HMPEVENTHANDLER)(unsigned long);

#define rtpInfoLocal(pmi)   pmi->MediaData[0]
#define rtcpInfoLocal(pmi)  pmi->MediaData[1]
#define rtpInfoRemote(pmi)  pmi->MediaData[0]
#define rtcpInfoRemote(pmi) pmi->MediaData[1]
#define codecRemote(pmi)    pmi->MediaData[2]
#define codecLocal(pmi)     pmi->MediaData[3]

char* localIP(IPM_MEDIA_INFO* pmi)     { return rtpInfoLocal(pmi).mediaInfo.PortInfo.cIPAddress;}
int   localPort(IPM_MEDIA_INFO* pmi)   { return rtpInfoLocal(pmi).mediaInfo.PortInfo.unPortId;  }
int   localCoder(IPM_MEDIA_INFO* pmi)  { return codecLocal(pmi).mediaInfo.CoderInfo.eCoderType; } 
int   localFrsize(IPM_MEDIA_INFO* pmi) { return codecLocal(pmi).mediaInfo.CoderInfo.eFrameSize; } 
int   localVad(IPM_MEDIA_INFO* pmi)    { return codecLocal(pmi).mediaInfo.CoderInfo.eVadEnable; } 

char* remoteIP(IPM_MEDIA_INFO* pmi)    { return rtpInfoRemote(pmi).mediaInfo.PortInfo.cIPAddress;}
int   remotePort(IPM_MEDIA_INFO* pmi)  { return rtpInfoRemote(pmi).mediaInfo.PortInfo.unPortId;  }
int   remoteCoder(IPM_MEDIA_INFO* pmi) { return codecRemote(pmi).mediaInfo.CoderInfo.eCoderType; } 
int   remoteFrsize(IPM_MEDIA_INFO* pmi){ return codecRemote(pmi).mediaInfo.CoderInfo.eFrameSize; } 
int   remoteVad(IPM_MEDIA_INFO* pmi)   { return codecRemote(pmi).mediaInfo.CoderInfo.eVadEnable; }

int err(int h,char*s) { printf("err on %s %s\n",s,ATDV_ERRMSGP(h)); return -1; }

long hmpEventHandler(unsigned long)
{            
  long  devhandle  = sr_getevtdev();  // Device handle of event firing
  long  eventType  = sr_getevttype(); // Type of event firing
  int   eventError =(eventType == TDX_ERROR) || (eventType == IPMEV_ERROR);
  long  dataLength = eventError? 0: sr_getevtlen();         
  void* eventData  = dataLength? sr_getevtdatap(): NULL;

  printf("EVNT dev=%d, et=%d, dl=%d\n", devhandle, eventType, dataLength);

  if (eventError) 
      printf("EVNT HMP error '%s' on device %d\n",ATDV_ERRMSGP(devhandle),devhandle);  

  if (eventType == IPMEV_SEND_DIGITS)
      printf("Received IPMEV_SEND_DIGITS for device = %sat %d\n", ATDV_NAMEP(devhandle), GetTickCount());

  if (eventType == 129)
  {
      printf("Received 129 for device = %s at %d, term reason=%d\n", ATDV_NAMEP(devhandle), GetTickCount(), ATDX_TERMMSK(devhandle));
      dx_clrdigbuf(devhandle);
  }

  return 0; // Indicate we handled the event                       
}


int registerEventHandler(bool b)        
{ 
  return b?                         
  sr_enbhdlr(EV_ANYDEV, EV_ANYEVT, (HMPEVENTHANDLER)hmpEventHandler):     
  sr_dishdlr(EV_ANYDEV, EV_ANYEVT, (HMPEVENTHANDLER)hmpEventHandler);  
}


int init()
{ 
  // Make sure hmp is operable
  registerEventHandler(true);
  if  (sr_getboardcnt("ipm", &result) == -1) return -1;
  if  (sr_getboardcnt(DEV_CLASS_VOICE, &result) == -1) return -1;
  if  (sr_getboardcnt("dcb", &result) == -1) return -1;
  if  (-1 == (handleIP = ipm_Open("ipmB1",0,EV_SYNC))) return -1;
  ipm_Close(handleIP, EV_SYNC);
  if  (-1 == (handleConf = dcb_open("dcbB1",0))) return -1;
  dcb_close(handleConf); 
  if  (-1 == (handleVox = dx_open("dxxxB1", NULL))) return -1; 
  dx_close(handleVox);
  return 0;
}


int openVoiceDevice(int ordinal) 
{  
  int voiceDeviceHandle = -1;
  HMPCARDID(deviceID,"dxxx", 1, ordinal);
  if  (-1 == (voiceDeviceHandle = dx_open(deviceID, 0))) 
       printf("error on dx_open\n");         
  else printf("opened %s as %d\n",deviceID,voiceDeviceHandle); 
  return voiceDeviceHandle;
}


int closeVoiceDevice(const int whichDevice) 
{     
  const int devhandle = voiceHandles[whichDevice]; 
  const int result = dx_close(devhandle);
  return result;
}


int openIpDevice(const int whichDevice, const int ordinal) 
{     
  int ipDeviceHandle = -1;                                        
  HMPCARDID(deviceID, "ipm", 1, ordinal);
  if  (-1 == (ipDeviceHandle = ipm_Open(deviceID, 0, EV_SYNC))) 
  {    printf("error on ipm_Open\n"); 
       return -1;
  }        
  printf("opened %s as %d\n",deviceID,ipDeviceHandle); 
  ipHandles[whichDevice] = ipDeviceHandle;

  IPM_MEDIA_INFO* mil = &mediaInfoLocal[whichDevice];
  int  result = ipm_GetLocalMediaInfo(ipDeviceHandle, mil, EV_SYNC);
  if  (result == -1) 
       err(ipDeviceHandle, "ipm_GetLocalMediaInfo");
  else printf("IP dvc %d %s port %d\n", ipDeviceHandle, localIP(mil), localPort(mil));

  return result;
}


long ipTimeslot(const int whichDevice, SC_TSINFO* slotinfo) 
{
  const int devhandle = ipHandles[whichDevice]; 
  slotinfo->sc_numts = 1;
  long timeslotNumber=0;
  slotinfo->sc_tsarrayp = &timeslotNumber;

  int result = ipm_GetXmitSlot(devhandle, slotinfo, EV_SYNC);         
  if (result == -1) 
  {   printf("error on ipm_GetXmitSlot %d\n",devhandle);  
      return -1;
  }
  printf("ip %d xmit slot is %d\n", devhandle, timeslotNumber);   
  return result == -1? -1: timeslotNumber;
} 


int vxTimeslot(const int whichDevice, SC_TSINFO* slotinfo) 
{
  const int devhandle = voiceHandles[whichDevice]; 
  slotinfo->sc_numts = 1;
  long timeslotNumber=0;
  slotinfo->sc_tsarrayp = &timeslotNumber;

  int result = dx_getxmitslot(devhandle, slotinfo);
  if (result == -1) 
  {   printf("error on dx_getxmitslot %d\n",devhandle); 
      return -1;
  }
  printf("vx %d xmit slot is %d\n", devhandle, timeslotNumber);
  return timeslotNumber;
} 


int listenIpToVoice(const int whichIP, const int whichVoice)
{
  const int iphandle = ipHandles[whichIP]; 
  const int vxhandle = voiceHandles[whichVoice]; 
  SC_TSINFO tsip;
  SC_TSINFO tsdx;

  long ipxmitslot = ipTimeslot(whichIP, &tsip);
  long vxtimeslot = vxTimeslot(whichVoice, &tsdx);

  tsdx.sc_numts = 1;
  tsdx.sc_tsarrayp = &vxtimeslot;
  result = dx_getxmitslot(vxhandle, &tsdx);

  result = ipm_Listen(iphandle, &tsdx, EV_SYNC);
  if (result != 0) printf("error on ipm_Listen %s\n",ATDV_ERRMSGP(iphandle));

  tsip.sc_numts = 1;
  tsip.sc_tsarrayp = &ipxmitslot;
  result = ipm_GetXmitSlot(iphandle, &tsip, EV_SYNC);

  int result = dx_listen(vxhandle, &tsip);
  if (result != 0) printf("error on dx_listen %s\n",ATDV_ERRMSGP(vxhandle));

  /*
  int result = dx_listen(vxhandle, &tsdx);
  if (result != 0) printf("error on dx_listen %s\n",ATDV_ERRMSGP(vxhandle));

  tsip.sc_numts = 1;
  tsip.sc_tsarrayp = &ipxmitslot;

  result = ipm_GetXmitSlot(iphandle, &tsip, EV_SYNC);         

  result = ipm_Listen(iphandle, &tsip, EV_SYNC);
  if (result != 0) printf("error on ipm_Listen %s\n",ATDV_ERRMSGP(iphandle));
  */

  return result;
}


int unlistenIpToVoice(const int whichIP, const int whichVoice)
{
  const int iphandle = ipHandles[whichIP]; 
  const int vxhandle = voiceHandles[whichVoice];
  int result = ipm_UnListen(iphandle, EV_SYNC);
  if (result == 0) result = dx_unlisten(vxhandle);

  return result;
}


int initRemoteMediaInfo(const int whichDevice, const char* ip, unsigned int port) 
{ 
  // Set remote and local connectivity selections. Set media properties.
  IPM_MEDIA_INFO* mir = &mediaInfoRemote[whichDevice];
  mir->unCount = 4; 
                                             
  rtpInfoRemote(mir).eMediaType = MEDIATYPE_REMOTE_RTP_INFO;
  if (port & 1) printf("???? remote port (%d) not modulo 2\n",port);
  rtpInfoRemote(mir).mediaInfo.PortInfo.unPortId = port;     
  strcpy(rtpInfoRemote(mir).mediaInfo.PortInfo.cIPAddress, ip);
                                             
  rtcpInfoRemote(mir).eMediaType = MEDIATYPE_REMOTE_RTCP_INFO;                                            
  rtcpInfoRemote(mir).mediaInfo.PortInfo.unPortId = port + 1;// RTCP port = RTP port+1
  strcpy(rtcpInfoRemote(mir).mediaInfo.PortInfo.cIPAddress, ip); 

  IPM_MEDIA* remoteInfo = &codecRemote(mir); // symbols for the debugger  
  IPM_MEDIA* localInfo  = &codecLocal(mir);  
  IPM_MEDIA* rtpInfo    = &rtpInfoRemote(mir);
  IPM_MEDIA* rtcpInfo   = &rtcpInfoRemote(mir);    
  
  codecRemote(mir).eMediaType = MEDIATYPE_REMOTE_CODER_INFO;
  codecLocal(mir).eMediaType  = MEDIATYPE_LOCAL_CODER_INFO;

  codecRemote(mir).mediaInfo.CoderInfo.eCoderType = CODER_TYPE_G711ULAW64K;      
  codecLocal(mir).mediaInfo.CoderInfo.eCoderType   
        = codecRemote(mir).mediaInfo.CoderInfo.eCoderType;

  codecRemote(mir).mediaInfo.CoderInfo.eFrameSize = CODER_FRAMESIZE_20;          
  codecLocal(mir).mediaInfo.CoderInfo.eFrameSize    
        = codecRemote(mir).mediaInfo.CoderInfo.eFrameSize; 
 
  codecRemote(mir).mediaInfo.CoderInfo.unFramesPerPkt = 1; 
  codecLocal(mir).mediaInfo.CoderInfo.unFramesPerPkt  
        = codecRemote(mir).mediaInfo.CoderInfo.unFramesPerPkt;  
  return 0;
} 


int startIpSession(const int whichDevice, char* ip, const int port) 
{     
  int  devhandle = ipHandles[whichDevice]; 
  IPM_MEDIA_INFO* mir = &mediaInfoRemote[whichDevice];
  eIPM_DATA_DIRECTION dataflowDirection = DATA_IP_TDM_BIDIRECTIONAL;
                                      
  if (0 == (result = initRemoteMediaInfo(whichDevice, ip, port)))
  {   result = ipm_StartMedia(devhandle, mir, dataflowDirection, EV_SYNC);
      if (result != 0) printf("error on ipm_StartMedia %s\n",ATDV_ERRMSGP(devhandle)); 
  }

  return result;
}


int closeIpSession(const int whichDevice) 
{     
  int devhandle = ipHandles[whichDevice]; 
  int result = ipm_Stop(devhandle, STOP_ALL, EV_SYNC); 
  if (result == -1)
  {   printf("error on ipm_Stop %s\n",ATDV_ERRMSGP(devhandle)); 
      return -1;                     
  }                                 
  return result;
}

int openfilePlay(const int whichDevice, char* filepath)
{
  int  devhandle = voiceHandles[whichDevice];
  int  fhandle = dx_fileopen(filepath, O_BINARY|O_RDONLY); 
  if  (fhandle == -1)
  {    err(devhandle,"dx_fileopen");    
       return -1;                     
  } 
                 
  DX_IOTT* iottentry = &iott[whichDevice]; // test program assumes one IOTT entry per device
  iottentry->io_fhandle = fhandle;
  iottentry->io_type    = IO_DEV | IO_EOT;
  iottentry->io_offset  = 0;           
  iottentry->io_length  = -1; // -1 = play to eof or a term cond
  return 0;
}


int playfile(const int whichDevice, char* path)
{
  if (-1 == openfilePlay(whichDevice, path)) return -1;

  const int devhandle = voiceHandles[whichDevice];

  DX_XPB* xpbentry = &xpb[whichDevice];
  xpbentry->wBitsPerSample = 8;
  xpbentry->wFileFormat = FILE_FORMAT_WAVE;
  xpbentry->wDataFormat = DATA_FORMAT_DIALOGIC_ADPCM;          
  xpbentry->wBitsPerSample = 4;
  xpbentry->nSamplesPerSec = DRT_8KHZ;  

  DX_IOTT* iottentry = &iott[whichDevice]; // test program assumes one IOTT entry per device                    
  DV_TPT tpt; 
  memset(&tpt, 0, sizeof(tpt));
  tpt.tp_type   = IO_EOT;  
  tpt.tp_termno = DX_MAXDTMF;  
  tpt.tp_length = 1;  
  tpt.tp_flags  = TF_MAXDTMF;  
             
  dx_clrdigbuf(devhandle);

  int result = dx_playiottdata(devhandle, iottentry, &tpt, xpbentry, EV_ASYNC);

  if (result == -1)
  {   printf("error on dx_playiottdata %s\n",ATDV_ERRMSGP(devhandle)); 
      return -1;                     
  } 
  return 0;
}

int main(int argc, char* argv[])
{
  do {  
  if (-1 == init()) { printf("Could not initialize HMP\n"); break; }

  if ((handleConf = dcb_open("dcbB1D1",0)) == -1)
  {    err(0, "dcb_open");
       break;
  } 

  if (-1 == (voiceHandles[0] = openVoiceDevice(1))) break;
  if (-1 == (voiceHandles[1] = openVoiceDevice(2))) break;
  if (-1 == (voiceHandles[2] = openVoiceDevice(3))) break;

  if (-1 == openIpDevice(0, 1)) break;

  if (-1 == startIpSession(0, "10.1.12.153", 20480)) break;

  int result = listenIpToVoice(0, 0);

  printf("\nplaying 4-second wav file at %d\n", GetTickCount());
  playfile(0, "foursecond.wav");

  IPM_DIGIT_INFO myDigitInfo; 
  memset(&myDigitInfo, 0, sizeof(myDigitInfo));
  myDigitInfo.eDigitType       = DIGIT_ALPHA_NUMERIC;
  myDigitInfo.eDigitDirection  = DIGIT_TDM;
  memcpy(myDigitInfo.cDigits, "1", 1);
  myDigitInfo.unNumberOfDigits = 1; 

  /*
  if(ipm_SendDigits(ipHandles[0], &myDigitInfo, EV_ASYNC) == -1)
  {
      printf("ipm_SendDigits failed for device name = %s with error = %d\n", 
             ATDV_NAMEP(ipHandles[0]), ATDV_LASTERR(ipHandles[0]));
  }
  else
  {
      printf("Digit sent ASYNC at %d\n", GetTickCount());
      Sleep(3000);
  }
  */

  printf("Digit before send SYNC at %d\n", GetTickCount());
  if(ipm_SendDigits(ipHandles[0], &myDigitInfo, EV_SYNC) == -1)
  {
      printf("ipm_SendDigits failed for device name = %s with error = %d\n", 
             ATDV_NAMEP(ipHandles[0]), ATDV_LASTERR(ipHandles[0]));
  }
  else
  {
      printf("Digit sent SYNC at %d\n", GetTickCount());
  }

  printf("Digit before send SYNC at %d\n", GetTickCount());
  if(ipm_SendDigits(ipHandles[0], &myDigitInfo, EV_SYNC) == -1)
  {
      printf("ipm_SendDigits failed for device name = %s with error = %d\n", 
             ATDV_NAMEP(ipHandles[0]), ATDV_LASTERR(ipHandles[0]));
  }
  else
  {
      printf("Digit sent SYNC at %d\n", GetTickCount());
  }

  result = listenIpToVoice(0, 1);
  printf("\nplaying 4-second wav file to second voice at %d\n", GetTickCount());
  playfile(1, "foursecond.wav");

  result = listenIpToVoice(0, 2);
  printf("\nplaying 4-second wav file to third voice at %d\n", GetTickCount());
  playfile(2, "foursecond.wav");

  Sleep(30000);

  unlistenIpToVoice(0, 0);
  unlistenIpToVoice(0, 1);
  unlistenIpToVoice(0, 2);
  closeVoiceDevice(0);
  closeVoiceDevice(1);
  closeVoiceDevice(2);
  closeIpSession(0);
  } while(0);

  dcb_close(handleConf);
  for(int i=0; i < 16; i++) if (voiceHandles[i] > 0) dx_close(voiceHandles[i]);
  for(int j=0; j < 16; j++) if (ipHandles[i] > 0) ipm_Close(ipHandles[i], EV_SYNC);
  printf("any key ..."); while(!c) c = _getch(); printf("\n");
  return result;
}
