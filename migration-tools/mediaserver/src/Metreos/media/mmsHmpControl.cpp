//
// mmsHmpControl.h -- HMP service startup and shutdown
//
#include "StdAfx.h"
#include "mmsHmpControl.h"

#ifdef MMS_ENABLE_MEMORY_LEAK_DETECTION
#define new MMS_MEMLEAK_NEW
#endif // MMS_ENABLE_MEMORY_LEAK_DETECTION



int hmpStart(MmsConfig* config, int verify, int detect, int verbose)
{        
  // Start the hmp service
                                             
  int serviceState = hmpGetServiceState();  // Check if stopped
  if (serviceState < 0) return serviceState;
                                            // Start unless already started?
  if (verify && (serviceState == MMS_HMP_SERVICERUN)) return 0;

  NCM_SetDlgSrvStartupMode(NCM_DLGSRV_MANUAL); 
  int result = 0, elapsed = 0;
  const int maxWaitSecs = config->hmp.startSvcMaxWaitSecs;


  if (serviceState != MMS_HMP_SERVICESTOP)  // Stop unless already stopped
  {
      result = hmpStopService(verbose);
      if (result < 0) return MMS_HMP_ERROR_STOPSERVICE;

      serviceState = hmpGetServiceState();
                                            // Wait until stopped ...
      while(serviceState != MMS_HMP_SERVICESTOP && elapsed < maxWaitSecs) 
      {       
        mmsSleep(1);
        elapsed++;
        ACE_OS::printf(".");
        serviceState = hmpGetServiceState();
      } 

      if (elapsed) ACE_OS::printf("\n");
      if (serviceState != MMS_HMP_SERVICESTOP) return MMS_HMP_ERROR_STOPSERVICE; 
  }

 
  if (detect)                               // Detect boards
  {
      result = hmpDetectBoards();
      if (result < 0) return MMS_HMP_ERROR_BOARDDETECT;
  }


  result = hmpStartService(verbose);        // Initiate service start
  if (result < 0) return MMS_HMP_ERROR_STARTSERVICE;          

  serviceState = hmpGetServiceState();
  elapsed = 0;
                                            // Wait until started ...
  while(serviceState != MMS_HMP_SERVICERUN && elapsed < maxWaitSecs) 
  {    
    mmsSleep(1);
    elapsed++;
    ACE_OS::printf(".");
    serviceState = hmpGetServiceState();
  }  

  if (elapsed) ACE_OS::printf("\n");
  return serviceState == MMS_HMP_SERVICERUN? 0: MMS_HMP_ERROR_STARTSERVICE;
}



int hmpStop(int waitSecs, int verbose)
{
  int serviceState = hmpGetServiceState();  // Check if stopped
  if (serviceState < 0) return serviceState;                                            
  if (serviceState == MMS_HMP_SERVICESTOP) return 0;

  int result = 0, elapsed = 0;

  result = hmpStopService(verbose);
  if (result < 0) return MMS_HMP_ERROR_STOPSERVICE;

  serviceState = hmpGetServiceState();
                                            // Wait until stopped ...
  while(serviceState != MMS_HMP_SERVICESTOP && elapsed < waitSecs) 
  {       
    mmsSleep(1);
    elapsed++;
    ACE_OS::printf(".");
    serviceState = hmpGetServiceState();
  }
 
  ACE_OS::printf("\n");
  return serviceState == MMS_HMP_SERVICESTOP? 0: MMS_HMP_ERROR_STOPSERVICE; 
}



int hmpStartService(int verbose)
{
  // Start the hmp service

  int result = 0;                           // No crlf
  if (verbose) ACE_OS::printf("MAIN starting firmware service ");

  NCMRetCode rc = NCM_StartDlgSrv();

  if  (rc != NCM_SUCCESS)
  {
       result = MMS_HMP_ERROR_STARTSERVICE;
       NCMErrorMsg* msg = NULL;
       NCM_GetErrorMsg(rc, &msg);
       if (msg) 
       {   if (msg->name) ACE_OS::printf("MAIN %s\n", msg->name);
           NCM_Dealloc(msg); 
       } 
  }

  return result;
}



int hmpStopService(int verbose)
{
  // Stop the hmp service

  int result = 0;                           // No crlf
  if (verbose) ACE_OS::printf("MAIN stopping firmware service ");

  NCMRetCode rc = NCM_StopDlgSrv();

  if  (rc != NCM_SUCCESS)
  {
       result = MMS_HMP_ERROR_STOPSERVICE;
       NCMErrorMsg* msg = NULL;
       NCM_GetErrorMsg(rc, &msg);
       if (msg) 
       {   if (verbose && msg->name) ACE_OS::printf("MAIN %s\n", msg->name);
           NCM_Dealloc(msg); 
       }       
  }

  return result;
}



int hmpGetServiceState()
{
  // Get state of HMP service, whether started, stopped, or other (pending etc)
  int result = 0;
  NCMDlgSrvState state = 0;

  NCMRetCode rc = NCM_GetDlgSrvState(&state);

  if  (rc != NCM_SUCCESS) 
       result = MMS_HMP_ERROR_SERVICESTATE; 

  else switch(state)
  {
    case SERVICE_RUNNING: result = MMS_HMP_SERVICERUN;  break;
    case SERVICE_STOPPED: result = MMS_HMP_SERVICESTOP; break;
    default: result = MMS_HMP_SERVICEOTHER;
  }

  return result;
}



int hmpDetectBoards()
{
  // Detect boards installed -- must be done prior to NCM_StartDlgSrv
  // In an HMP environment the returned board count should always be 1

  NCM_DETECTION_INFO info;
  info.structSize  = sizeof(NCM_DETECTION_INFO);
  info.callbackFcn = (NCM_CALLBACK_FCN*) hmpServiceStartPctCompleteCallback;
  info.pcdFileSelectionFcn = (NCM_PCDFILE_SELECTION_FCN*)hmpGetPcdFile;

  NCM_DETECTION_RESULT dresult; 
  dresult.structSize = sizeof(NCM_DETECTION_RESULT);
  int result = 0;

  // As of HMP 3.0 SU 103, this does not work. However, after it fails and mms exits,
  // the next time mms is started and executes this code NCM_DetectBoardsEx will work 
  // as expected. I presume this is an issue that will be fixed in another SU.
  NCMRetCode rc = NCM_DetectBoardsEx(&info, &dresult);

  if  (rc != NCM_SUCCESS)
  {
       result = -1;
       NCMErrorMsg* msg = NULL;
       NCM_GetErrorMsg(rc, &msg);
       if (msg) 
       {   if (msg->name) 
               ACE_OS::printf("MAIN error '%s' on NCM_DetectBoardsEx\n", msg->name);
           NCM_Dealloc(msg); 
       }
  }
  else result = dresult.totalDetectedBoards;

  return result;
}



int hmpServiceStartPctCompleteCallback(unsigned int pct, const char* msg)
{
  // Not necessary to supply this callback since there is essentially
  // no wait -- we could set parameter 1 of NCM_DetectBoardsEx to NULL  
  // ACE_OS::printf("%d pct complete for '%'\n", pct, msg);

  return 1;
}



int hmpGetPcdFile(NCMFileInfo* flist, int count, NCMDevInfo info, int* ndx)
{
  // Not necessary to supply this NCM_DetectBoardsEx callback since it appears 
  // not to be invoked in the HMP environment

  return 0;
}


