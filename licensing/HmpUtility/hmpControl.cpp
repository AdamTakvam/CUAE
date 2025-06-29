//
// hmpControl.h -- HMP service startup, shutdown, restore, and register
//
#include "StdAfx.h"
#include "hmpControl.h"
#include "OEMLicense.h"
#include "hmpregisterdll.h"

char gszPCDFile[512];

// the official location of the dll and its name
const char* libraryName = "C:\\Program Files\\Cisco Systems\\Unified Application Environment\\LicenseServer\\CUAEUtl1.dll";
const char* functionName= "utilityfunc";

int hmpStart()
{        
  // Start the hmp service
                                             
  int serviceState = hmpGetServiceState();  // Check if stopped
  if (serviceState < 0) return serviceState;
                                            // Start unless already started?
  if (serviceState == MMS_HMP_SERVICERUN) return 0;

  NCM_SetDlgSrvStartupMode(NCM_DLGSRV_MANUAL); 
  int result = 0, elapsed = 0;
  const int maxWaitSecs = 100;//config->hmp.startSvcMaxWaitSecs;


  if (serviceState != MMS_HMP_SERVICESTOP)  // Stop unless already stopped
  {
      result = hmpStopService();
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

 
  if (1)                               // Detect boards
  {
      result = hmpDetectBoards();
      if (result < 0) return MMS_HMP_ERROR_BOARDDETECT;
  }


  result = hmpStartService();        // Initiate service start
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

int hmpStop()
{
  const int maxWaitSecs = 100;
  int serviceState = hmpGetServiceState();  // Check if stopped
  if (serviceState < 0) return serviceState;                                            
  if (serviceState == MMS_HMP_SERVICESTOP) return 0;

  int result = 0, elapsed = 0;

  result = hmpStopService();
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
 
  ACE_OS::printf("\n");
  return serviceState == MMS_HMP_SERVICESTOP? 0: MMS_HMP_ERROR_STOPSERVICE; 
}



int hmpStartService()
{
  // Start the hmp service

  int result = 0;                           // No crlf
  ACE_OS::printf("MAIN starting firmware service ");

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

int hmpStopService()
{
  // Stop the hmp service

  int result = 0;                           // No crlf
  ACE_OS::printf("MAIN stopping firmware service ");

  NCMRetCode rc = NCM_StopDlgSrv();

  if  (rc != NCM_SUCCESS)
  {
       result = MMS_HMP_ERROR_STOPSERVICE;
       NCMErrorMsg* msg = NULL;
       NCM_GetErrorMsg(rc, &msg);
       if (msg) 
       {   if (msg->name) ACE_OS::printf("MAIN %s\n", msg->name);
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
  //SERVICE_STATUS state;// = null;

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

  NCMRetCode rc = NCM_DetectBoardsEx(&info, &dresult);

  if  (rc != NCM_SUCCESS)
  {
       NCMErrorMsg* msg = NULL;
       NCM_GetErrorMsg(rc, &msg);
       if (msg) 
       {   if (msg->name) ACE_OS::printf("MAIN %s\n", msg->name);
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

int CallBackFunc(UINT uipercent, const char *message)
{
	ACE_OS::printf("%d percent complete \n Status message: %s \n", uipercent, message);
	return TRUE;
}

int GetPCDFile(NCMFileInfo *fileList, int NumFiles, NCMDevInfo devInfo, int *index)
{
	int i = 0;
	int id = 0;

	// Try to match command line input for pcd file
	if (strlen(gszPCDFile) > 0)
	{
		for (i=0; i<NumFiles; i++)
		{
			if (!stricmp(fileList[i].fileName, gszPCDFile))
			{
				*index = i; 
				return *index;
			}
		}
	}

	// No good info from caller, list all PCD files on the system if there are more than 2
	for (i=0; i<NumFiles; i++)
	{
		ACE_OS::printf ("Index %d, file name = %s \n", i, fileList[i].fileName);
		if (strcmp(fileList[i].fileName, "240r240v200e240c240s240i_pur.pcd") == 0) id = i;
	}

	ACE_OS::printf("Please select file index: %s\n", fileList[id].fileName);
	//scanf("%d", &id);
	*index = id;
	return *index;
}

void RestoreDefaults()
{
	NCMFamily ncmFamily = { DEVICE_FAMILY, NULL };		// device family
	NCMDevice ncmDevice = { DEVICE_NAME, NULL };		// device name

	NCMRetCode ncmRc = NCM_SUCCESS;
	NCM_DETECTION_INFO detectionInfo;
	detectionInfo.structSize = sizeof(NCM_DETECTION_INFO);
	detectionInfo.callbackFcn = (NCM_CALLBACK_FCN*)CallBackFunc;					// progress callback function
	detectionInfo.pcdFileSelectionFcn = (NCM_PCDFILE_SELECTION_FCN*)GetPCDFile;	// PCD file selection callback function

	NCMDevice * pncmNewDevice = NULL;

	ncmRc = NCM_ReconfigBoard(&ncmFamily, &ncmDevice, &detectionInfo, &pncmNewDevice);
	if (ncmRc != NCM_SUCCESS)
	{
		NCMErrorMsg *pncmErrorMsg = NULL;
		ncmRc = NCM_GetErrorMsg(ncmRc, &pncmErrorMsg);
		if (ncmRc == NCM_SUCCESS)
		{
			ACE_OS::printf ("NCM_ReconfigureBoard() returns error: %s\n", pncmErrorMsg->name);
		}
		else
		{
			ACE_OS::printf("NCM_ReconfigureBoard() returns unknown error\n");
			NCM_Dealloc(pncmErrorMsg);
		}
	}
	else
	{
		ACE_OS::printf ("NCM_ReconfigureBoard() returns success\n");
	}
	NCM_Dealloc(pncmNewDevice);
}

void  registerDLL()
{
	if ( OEMLicenseRetCode_Success == OEMLicense_RegisterOEMLib(libraryName, functionName) )
		ACE_OS::printf("\nSuccess");
	else {
		ACE_OS::printf("\nFailure");
		ACE_OS::printf("\nCheck %s", libraryName);
	}
}