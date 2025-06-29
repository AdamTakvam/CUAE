#include "StdAfx.h"
#include <srllib.h>
#include <gclib.h>
#include <ipmlib.h>
#include <dtilib.h>
#include <dxxxlib.h>
#include <msilib.h>
#include <dcblib.h>
#include <errno.h>
#include <iostream>
#define WAITFORINPUT do{char c;std::cout << "Any character ...";std::cin >> c;}while(0)

int  result, handleIP, handleConf, conferenceID;
long iptimeslotnum, listentimeslot;
IPM_MEDIA_INFO mediaInfoLocal;
SC_TSINFO      slotinfo; 
MS_CDT         cdt; 
char* ipKey="ipmB1C1", *confKey="dcbB1D1";
typedef long (*HMPEVENTHANDLER)(unsigned long);
long hmpEventHandler(long whatever) { return 0; }

int registerEventHandler(bool b)        
{ 
  return b?                         
  sr_enbhdlr(EV_ANYDEV, EV_ANYEVT, (HMPEVENTHANDLER)hmpEventHandler):     
  sr_dishdlr(EV_ANYDEV, EV_ANYEVT, (HMPEVENTHANDLER)hmpEventHandler);  
}

int init()
{ 
  // Make sure we can do some benign stuff before the heavy lifting
  registerEventHandler(true);
  if  (sr_getboardcnt("ipm", &result) == -1) return -1;
  if  (sr_getboardcnt(DEV_CLASS_VOICE, &result) == -1) return -1;
  if  (sr_getboardcnt("dcb", &result) == -1) return -1;
  if  (-1 == (handleIP = ipm_Open("ipmB1",0,EV_SYNC))) return -1;
  ipm_Close(handleIP, EV_SYNC);
  if  (-1 == (handleConf = dcb_open("dcbB1",0))) return -1;
  dcb_close(handleConf);
  return 0;
}

int err(int h,char*s){printf("err during %s %s\n",s,ATDV_ERRMSGP(h));return -1;}



int main(int argc, char* argv[])
{
  do {  
  if (-1 == init()) {printf("Could not init HMP\n"); break;}
      
  handleIP = ipm_Open(ipKey,0,EV_SYNC);              
  if  (handleIP == -1) return err(0,"ipm_Open");

  result = ipm_GetLocalMediaInfo(handleIP, &mediaInfoLocal, EV_SYNC);
  if  (result == -1) return err(handleIP,"ipm_GetLocalMediaInfo");

  handleConf = dcb_open(confKey, 0); 
  if  (handleConf == -1) return err(0,"dcb_open");

  slotinfo.sc_numts = 1;
  slotinfo.sc_tsarrayp = &iptimeslotnum;

  if  (ipm_GetXmitSlot(handleIP, &slotinfo, EV_SYNC) == -1) 
       return err(handleIP,"ipm_GetXmitSlot");        

  cdt.chan_num  = iptimeslotnum;
  cdt.chan_sel  = MSPN_TS;
  cdt.chan_attr = MSPA_NULL;
                                             
  result = dcb_estconf(handleConf, &cdt, 1, 0, &conferenceID);
  if  (result == -1) return err(handleConf,"dcb_estconf"); 
  printf("1-party conference %d established\n",conferenceID);

  listentimeslot = cdt.chan_lts;   
  slotinfo.sc_tsarrayp = &listentimeslot;

  if  (ipm_Listen(handleIP, &slotinfo, EV_SYNC) == -1) 
       return err(handleIP,"ipm_Listen"); 

  if  (ipm_UnListen(handleIP, EV_SYNC) == -1) err(handleIP,"ipm_UnListen"); 

  if  (ipm_Stop(handleIP, STOP_ALL, EV_SYNC) == -1) err(handleIP,"ipm_Stop"); 
   
  if  (dcb_delconf(handleConf, conferenceID) == -1) 
       err(handleConf,"dcb_delconf");
  printf("conference %d deleted\n",conferenceID); 

  if  (dcb_close(handleConf)  == -1)  err(handleConf,"dcb_close"); 
  if  (ipm_Close(handleIP, 0) == -1)  err(handleIP,"ipm_Close"); 
  } while(0); 

  WAITFORINPUT;
  return result;
}
