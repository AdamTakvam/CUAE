//
// CUAELicMgr.cpp -- licensing manager interface
//
#include "stdafx.h"
#include <string>
#include <iostream>
#include <errno.h>
#include <map>
#include <tchar.h>
#include "CUAELicMgr.h"
#include <sys\types.h> 
#include <sys\stat.h> 

VENDORCODE vendorCodeCUAE;
LM_HANDLE *lm_job_CUAE = NULL;

VENDORCODE vendorCodeCUME;
LM_HANDLE *lm_job_CUME = NULL;

std::map<std::string, int> availableFeatureCounts;

// arrays of LicenseMode strings, used throughout to iterate over possible license modes for a product while retrieving
// license mode information from the license server
char* mediaLicenseModes[] = { CUME_LICENSE_MODE_STANDARD, CUME_LICENSE_MODE_SMALLENV };
char* appserverLicenseModes[] = { CUAE_LICENSE_MODE_SMALLENV, CUAE_LICENSE_MODE_STANDARD, CUAE_LICENSE_MODE_ENHANCED, CUAE_LICENSE_MODE_PREMIUM, CUAE_LICENSE_MODE_STDENH, CUAE_LICENSE_MODE_STDPREM, CUAE_LICENSE_MODE_ENHPREM };


//#define CUAE_TEST   // comment this out when not testing app server
//#define CUME_TEST
//#define CUAE_NETPATH "@10.89.31.60" // Comment this out if FLEXlm server is localhost

///////////////////////////////////////////////////////////
//  DllMain
//  
//  DLL Main
//  
//
///////////////////////////////////////////////////////////
BOOL APIENTRY DllMain( HANDLE hModule, 
                       DWORD  ul_reason_for_call, 
                       LPVOID lpReserved
					 )
{
	switch (ul_reason_for_call)
	{
	  case DLL_PROCESS_ATTACH:
	  case DLL_THREAD_ATTACH:
	  case DLL_THREAD_DETACH:
	  case DLL_PROCESS_DETACH:
		break;
	}
    return TRUE;
}

///////////////////////////////////////////////////////////
//  WriteLog License Manager
//  
//  Write Log
//  
//
///////////////////////////////////////////////////////////
void WriteLogLicManager(char* pFile, char* pMsg) 
{
return;
	try {
		SYSTEMTIME oT;
		GetLocalTime(&oT);
		FILE* pLog = fopen(pFile,"a");
		fprintf(pLog, "\n%02d:%02d:%02d\n    %s",oT.wHour,oT.wMinute,oT.wSecond,pMsg);
		fclose(pLog);
	} catch(...) {}
}

int nGetExpiredCUAE(char *featurename)
{
	VENDORCODE* vendor		= NULL;
	LM_HANDLE *jobHandle	= NULL;
	vendor = &vendorCodeCUAE;
	jobHandle = lm_job_CUAE;

	int nStatus = -1;
	nStatus = lc_checkout(jobHandle,featurename,CUAE_FEATURE_VERSION,1,LM_CO_WAIT, vendor, LM_DUP_NONE);
	CONFIG *conf = 0;
	CONFIG *pos = 0;
	conf = lc_next_conf(jobHandle,featurename,&pos);
	
	int nDays = lc_expire_days(jobHandle,conf);

	lc_checkin(jobHandle,featurename,LM_CO_WAIT);

return nDays;
}

int nGetExpiredCUME(char *featurename)
{
	VENDORCODE* vendor		= NULL;
	LM_HANDLE *jobHandle	= NULL;
	vendor = &vendorCodeCUME;
	jobHandle = lm_job_CUME;

	int nStatus = -1;
	nStatus = lc_checkout(jobHandle,featurename,CUME_FEATURE_VERSION,1,LM_CO_WAIT, vendor, LM_DUP_NONE);
	CONFIG *conf = 0;
	CONFIG *pos = 0;
	conf = lc_next_conf(jobHandle,featurename,&pos);
	
	int nDays = lc_expire_days(jobHandle,conf);

	lc_checkin(jobHandle,featurename,LM_CO_WAIT);

return nDays;
}


///////////////////////////////////////////////////////////
//  populateDefaultLicenseCUAE
//  
//  Populate Default License CUAE
//  
//
///////////////////////////////////////////////////////////
void populateDefaultLicenseCUAE(LicenseInformationCUAE* licenseInfo)
{
    licenseInfo->licenseMode = (char**) CUAE_LICENSE_MODE_SDK ;
    licenseInfo->licenseModeThreshold = CUAE_MODE_LIMIT_SDK;
    licenseInfo->errorCode = LM_NOERROR;
}

///////////////////////////////////////////////////////////
//  populateDefaultLicenseCUME
//  
//  Populate Default License CUME
//  
//
///////////////////////////////////////////////////////////
void populateDefaultLicenseCUME(LicenseInformationCUME* licenseInfo)
{
    licenseInfo->licenseLimit					= CUME_MODE_LIMIT_SDK;
    licenseInfo->rtp							= CUME_MODE_LIMIT_SDK;
    licenseInfo->voice							= CUME_MODE_LIMIT_SDK;
    licenseInfo->conferencing					= CUME_MODE_LIMIT_SDK;
    licenseInfo->tts							= 1;
    licenseInfo->speechIntegration				= 0;
    licenseInfo->enhancedRTP					= 0;
    licenseInfo->voiceRecognitionEnginePorts	= 0;
    licenseInfo->errorCode						= LM_NOERROR;
}


///////////////////////////////////////////////////////////
//  getCountedFeature
//  
//  Get Feature
//  
//
///////////////////////////////////////////////////////////
int getCountedFeature(bool bCheckIn,LM_HANDLE* job, VENDORCODE* vendorCode, 
   LM_CHAR_PTR featureName, LM_CHAR_PTR featureVersion, int desiredInstances)
{
    int status = LM_NOERROR;

    try
    {
      status = lc_checkout(job, featureName, featureVersion, desiredInstances, LM_CO_WAIT, vendorCode, LM_DUP_NONE);
	  
	  if(bCheckIn == true)
	  {
		lc_checkin(job, featureName,LM_CO_WAIT);
	  }

	}
    catch(...)
	{ 
		status = CUAE_UNHANDLED_EXCEPTION; 
	}

    return status;
}


///////////////////////////////////////////////////////////
//  setDefaultJobAttributes
//  
//  Set Default Job Attrib
//  
//
///////////////////////////////////////////////////////////
int setDefaultJobAttributes(LM_HANDLE *job)
{
  int result = LM_NOERROR;

  try
  {
    lc_set_attr(job, LM_A_APP_DISABLE_CACHE_READ, (LM_A_VAL_TYPE) 1);
    #ifdef CUAE_NETPATH
    lc_set_attr(job, LM_A_LICENSE_DEFAULT, (LM_A_VAL_TYPE) CUAE_NETPATH);
    #else
    lc_set_attr(job, LM_A_LICENSE_DEFAULT, (LM_A_VAL_TYPE) DEFAULT_LICPATH);
    #endif
    lc_set_attr(job, LM_A_WINDOWS_MODULE_HANDLE, (LM_A_VAL_TYPE) 1);
    lc_set_attr(job, LM_A_PROMPT_FOR_FILE, (LM_A_VAL_TYPE) 0);
    lc_set_attr(job, LM_A_PERROR_MSGBOX , (LM_A_VAL_TYPE) 0);
  }
  catch(...) { result = CUAE_UNHANDLED_EXCEPTION; }

  return result;
}


///////////////////////////////////////////////////////////
//  mapAvailableFeatures
//  
//  Map Avail Features
//  
//
///////////////////////////////////////////////////////////
int mapAvailableFeatures()
{
    char **myFeatures; 
    LM_VD_FEATURE_INFO fi; 
    LM_HANDLE *lm_job;
    VENDORCODE vendorCode;

    int status = lc_new_job(NULL, NULL, &vendorCode, &lm_job);            
    if (status != LM_NOERROR) return status;

    // if there was no error, we set a bunch of attributes
    int result = setDefaultJobAttributes(lm_job);
    if (result != LM_NOERROR) return result;   

    // get a list of all available features 
    try
    {
       myFeatures = lc_feat_list(lm_job,LM_FLIST_ALL_FILES,NULL);
       result = myFeatures? LM_NOERROR: LM_BADPARAM;
           
    }
    catch(...) { result = CUAE_UNHANDLED_EXCEPTION; }
   
    if (result != LM_NOERROR) 
	{
		return result;   
	}

    int i = 0;

    // for each member of the list 
    while (myFeatures[i] != NULL) 
    { 
        CONFIG *pos  = 0; 
        CONFIG *conf = 0;
        int feat_count = 0, result = LM_NOERROR; 

        //get the config struct of this feature 
        while((conf = lc_next_conf(lm_job,myFeatures[i],&pos)) != NULL) 
        { 
            //fi.feat = lc_get_config(lm_job,myFeatures[i]); 
            fi.feat = conf; 

            //get generic feature info for this config struct 
            try
            {
              result = lc_get_attr(lm_job, LM_A_VD_FEATURE_INFO, (short *)&fi);

              if (result == LM_NOERROR)
			  {
				availableFeatureCounts[myFeatures[i]] = fi.num_lic; 
			  }
              else
              {   //couldn't get info for this config - probably a duplicate 
              } 
            }
            catch(...) { result = CUAE_UNHANDLED_EXCEPTION; }
        }

        if (result == CUAE_UNHANDLED_EXCEPTION) break;
        i++;
    } 

	

    return result;
}


// initializes the job belonging to the CUAE/CUME product
///////////////////////////////////////////////////////////
//  Initialize
//  
//  Init
//  
//
///////////////////////////////////////////////////////////
int initialize(Products product)
{
    int status = LM_NOERROR;

    switch (product)
    {
        case CUAE : 
            {
                if (lm_job_CUAE == NULL)
                {
                    try
                    {
                      status = lc_new_job(NULL, NULL, &vendorCodeCUAE, &lm_job_CUAE);
                    }
                    catch(...) { status = CUAE_UNHANDLED_EXCEPTION; }
            
                    // if there was no error, we set a bunch of attributes
                    if (status == LM_NOERROR)
					{
                        setDefaultJobAttributes(lm_job_CUAE);
					}
                }
            }
        case CUME : 
            {
                if (lm_job_CUME == NULL)
                {
                    try
                    {
                      status = lc_new_job(NULL, NULL, &vendorCodeCUME, &lm_job_CUME);
                    }
                    catch(...) { status = CUAE_UNHANDLED_EXCEPTION; }
            
                    // if there was no error, we set a bunch of attributes
                    if (status == LM_NOERROR)
					{
                        setDefaultJobAttributes(lm_job_CUME);
					}
                } 
            }

        default:    break;
    }

    if (status != CUAE_UNHANDLED_EXCEPTION) 
	{
        status = mapAvailableFeatures();
	}

    return status;
}


///////////////////////////////////////////////////////////
//  setCountedMediaLicenseValue
//  
//  Set Counted Media
//  
//
///////////////////////////////////////////////////////////
void setCountedMediaLicenseValue(LicenseInformationCUME *lic, LM_CHAR_PTR featureName, int value)
{
	//Ok if I am over the lic limit
	if(value > lic->licenseLimit)
	{
		value = lic->licenseLimit;
	}

    if (strcmp(featureName, CUME_RTP) == 0)
	{   
		lic->rtp = value;
		lic->licenseLimitArray[RTP_LIC_LIMIT] = lic->licenseLimit;
	}
	else if (strcmp(featureName, CUME_VOICE) == 0)
	{     
		lic->voice = value;
		lic->licenseLimitArray[VOICE_LIC_LIMIT] = lic->licenseLimit;
	}
	else if (strcmp(featureName, CUME_CONFERENCING) == 0)
	{     
		lic->conferencing = value;
		lic->licenseLimitArray[CONF_LIC_LIMIT] = lic->licenseLimit;
	}
	else if (strcmp(featureName, CUME_SPEECH_INTEGRATION) == 0)
	{     
		lic->speechIntegration = value;
		lic->licenseLimitArray[SPEECH_LIC_LIMIT] = lic->licenseLimit;
	}
	else if (strcmp(featureName, CUME_ENHANCED_RTP) == 0)
	{     
		lic->enhancedRTP = value;
		lic->licenseLimitArray[ENHANCED_LIC_LIMIT] = lic->licenseLimit;
	}
	else if (strcmp(featureName, CUME_TTS_ENGINE_PORTS) == 0)
	{     
		lic->tts = value;
		
		
		if(lic->nMode == SmallEnv)
		{
			if(lic->tts > 16)
			{
				lic->tts = 16;
			}

			lic->licenseLimitArray[TTS_LIC_LIMIT] = 16;
		}
		else
		{
			if(lic->tts > 60)
			{
				lic->tts = 60;
			}

			lic->licenseLimitArray[TTS_LIC_LIMIT] = 60;
		}



	}
	else if (strcmp(featureName, CUME_VOICE_REC_ENG_PORTS) == 0)
	{     
		lic->voiceRecognitionEnginePorts = value;
	}
}


///////////////////////////////////////////////////////////
//  loadCumeTestData
//  
//  Load Test Data
//  
//
///////////////////////////////////////////////////////////
int loadCumeTestData(LicenseInformationCUME *licenseInfo)
{
  #ifdef CUME_TEST
	licenseInfo->licenseLimit						= 0;
	licenseInfo->rtp								= 24;
	licenseInfo->voice								= 32;
	licenseInfo->conferencing						= 24;
	licenseInfo->speechIntegration					= 4;
	licenseInfo->enhancedRTP						= 8;
	licenseInfo->tts								= 4;
	licenseInfo->voiceRecognitionEnginePorts		= 4;
  return (licenseInfo->errorCode = LM_NOERROR);
  #endif
	
  return 0;
}

///////////////////////////////////////////////////////////
//  loadCuaeTestData
//  
//  Load Test Data
//  
//
///////////////////////////////////////////////////////////
int loadCuaeTestData(LicenseInformationCUAE *licenseInfo)
{
  #ifdef CUAE_TEST
  licenseInfo->scriptInstances = 1000;
  licenseInfo->licenseMode = (char**) CUAE_LICENSE_MODE_PREMIUM;
  licenseInfo->licenseModeThreshold = CUAE_MODE_LIMIT_PREMIUM;
  return (licenseInfo->errorCode = LM_NOERROR);
  #endif
	
  return 0;
}

///////////////////////////////////////////////////////////
//  getLicenseModeValue
//  
//  Get License Model Value
//  
//
///////////////////////////////////////////////////////////
int getLicenseModeValue(LM_CHAR_PTR featureName)
{
	//printf("here 1 [%s]\n",featureName);
	//fflush(stdout);

	int retVal = SDK;
	if (strcmp(featureName, CUME_LICENSE_MODE_SMALLENV) == 0)
	{
        retVal = SmallEnv;
	}
	else if (strcmp(featureName, CUAE_LICENSE_MODE_SMALLENV) == 0)
	{
        retVal = SmallEnv;
	}
	else if (strcmp(featureName, CUAE_LICENSE_MODE_STANDARD) == 0 || strcmp(featureName, CUME_LICENSE_MODE_STANDARD) == 0)
	{
        retVal = Standard;
	}
	else if (strcmp(featureName, CUAE_LICENSE_MODE_ENHANCED) == 0)
	{
        retVal = Enhanced;
	}
	else if (strcmp(featureName, CUAE_LICENSE_MODE_PREMIUM) == 0)
	{
        retVal = Premium;
	}
	else if (strcmp(featureName, CUAE_LICENSE_MODE_STDENH) == 0)
	{
        retVal = StdEnh;
	}
	else if (strcmp(featureName, CUAE_LICENSE_MODE_STDPREM) == 0)
	{
        retVal = StdPrem;
	}
	else if (strcmp(featureName, CUAE_LICENSE_MODE_ENHPREM) == 0)
	{
        retVal = EnhPrem;
	}

	return retVal;
}

///////////////////////////////////////////////////////////
//  getLicenseMode
//  
//  Get Mode
//  
//
///////////////////////////////////////////////////////////
int getLicenseMode(Products product, int* errorCode)
{
	VENDORCODE* vendor		= NULL;
	LM_HANDLE *jobHandle	= NULL;
	*errorCode				= LM_NOERROR;

	int licModeValue = SDK;

	if (&availableFeatureCounts == NULL || availableFeatureCounts.empty())
	{
		return licModeValue;
	}

	char** modesToRetrieve = NULL;
	int numberOfModes		= 0;

	switch (product)
	{
		case CUAE: 
			{
				modesToRetrieve = appserverLicenseModes;
				numberOfModes = CUAE_LICENSE_MODE_COUNT;
				vendor = &vendorCodeCUAE;
				jobHandle = lm_job_CUAE;
			}
			break;			
		case CUME: 
			{
				modesToRetrieve = mediaLicenseModes;
				numberOfModes = CUME_LICENSE_MODE_COUNT;
				vendor = &vendorCodeCUME;
				jobHandle = lm_job_CUME;
			}
			break;
		default:
			break;
	}



	if (modesToRetrieve == NULL)
	{
		return SDK;
	}

int nBestMode = -1;

		for (int c = 0; c < numberOfModes; c++)
		{
			if (availableFeatureCounts.find(modesToRetrieve[c]) != availableFeatureCounts.end())
			{
				int status = -1;
				status = getCountedFeature(true,jobHandle, vendor, modesToRetrieve[c], CUAE_FEATURE_VERSION, availableFeatureCounts[modesToRetrieve[c]]);
				if (status == LM_NOERROR)
				{
					int nVal = getLicenseModeValue(modesToRetrieve[c]);

					if(nVal > nBestMode)
					{
						licModeValue = nVal;
						nBestMode = licModeValue;
					}

					//printf("LicModeVale = [%d][%d]\n",c,licModeValue);
					//fflush(stdout);
				}
			}	
		}
	

    return licModeValue;     
}

///////////////////////////////////////////////////////////
//  getMediaLicenseInfo
//  
//  Get Media Lic
//  
//
///////////////////////////////////////////////////////////
int getMediaLicenseInfo(LicenseInformationCUME *licenseInfo)
{

	WriteLogLicManager("CUAELiceMgr.log","Enter getMediaLicenseInfo\n");

    if (licenseInfo == NULL // nullczyk
     || licenseInfo->length != sizeof(LicenseInformationCUME)
     || licenseInfo->signature != LI_SIG)
	{
        return -1;
	}

    #ifdef CUME_TEST
    return loadCumeTestData(licenseInfo);
    #endif

    int status  = initialize(CUME);
    if (status != LM_NOERROR)
    {
        licenseInfo->errorCode = status;
        return status;
    }
    else
	{
        licenseInfo->errorCode = LM_NOERROR;
	}


	WriteLogLicManager("CUAELiceMgr.log","After Init\n");

	int nDays = 1;
	int nVal = getLicenseMode(CUME, &licenseInfo->errorCode);
	int xxx = 0;
    switch (nVal)
    {
        case Standard: 
			{
				nDays = nGetExpiredCUME("Media");
				WriteLogLicManager("CUAELiceMgr.log","Standard Mode\n");

				licenseInfo->licenseLimit = CUME_MODE_LIMIT_MEDIA; 
				licenseInfo->nMode = Standard;
			}
			break;

        case SmallEnv:
			{
				nDays = nGetExpiredCUME("MediaSmallEnv");
				WriteLogLicManager("CUAELiceMgr.log","SmallEnv Mode\n");

				licenseInfo->licenseLimit = CUME_MODE_SMALLENV;
				licenseInfo->nMode = SmallEnv;
			}
			break;


		case SDK:
        default: 
			{			

				int nError = 0;
				int nMode = getLicenseMode(CUAE, &nError);

				//We are both SDK
				if (nMode != SDK)
				{
					WriteLogLicManager("CUAELiceMgr.log","No License Mode\n");

					licenseInfo->nMode = NOLIC;
					licenseInfo->licenseLimit					= 0;
					licenseInfo->rtp							= 0;
					licenseInfo->voice							= 0;
					licenseInfo->conferencing					= 0;
					licenseInfo->tts							= 0;
					licenseInfo->speechIntegration				= 0;
					licenseInfo->enhancedRTP					= 0;
					licenseInfo->voiceRecognitionEnginePorts	= 0;
					licenseInfo->errorCode						= LM_NOERROR;
				}
				else
				{

					WriteLogLicManager("CUAELiceMgr.log","SDK Mode\n");

					licenseInfo->nMode = SDK;
					licenseInfo->licenseLimit					= CUME_MODE_LIMIT_SDK;
					licenseInfo->rtp							= CUME_MODE_LIMIT_SDK;
					licenseInfo->voice							= CUME_MODE_LIMIT_SDK;
					licenseInfo->conferencing					= CUME_MODE_LIMIT_SDK;
					licenseInfo->tts							= 1;
					licenseInfo->speechIntegration				= 0;
					licenseInfo->enhancedRTP					= 0;
					licenseInfo->voiceRecognitionEnginePorts	= 0;
					licenseInfo->errorCode						= LM_NOERROR;

					licenseInfo->licenseLimitArray[RTP_LIC_LIMIT]		= 6;
					licenseInfo->licenseLimitArray[VOICE_LIC_LIMIT]		= 6;
					licenseInfo->licenseLimitArray[CONF_LIC_LIMIT]		= 6;
					licenseInfo->licenseLimitArray[SPEECH_LIC_LIMIT]	= 0;
					licenseInfo->licenseLimitArray[TTS_LIC_LIMIT]		= 1;
					licenseInfo->licenseLimitArray[ENHANCED_LIC_LIMIT]	= 0;
				}
				
				WriteLogLicManager("CUAELiceMgr.log","Exit getMediaLicenseInfo SDK\n");
				return LM_NOERROR;
			}
    }

    const LM_CHAR_PTR featuresToRetrieve[] = {CUME_RTP,CUME_VOICE,CUME_CONFERENCING,CUME_SPEECH_INTEGRATION,CUME_ENHANCED_RTP,CUME_TTS_ENGINE_PORTS}; 
    int featureCount = 0;

	if(nDays <= 0)
	{
		licenseInfo->nMode							= EXPIRED;
		licenseInfo->licenseLimit					= 0;
		licenseInfo->rtp							= 0;
		licenseInfo->voice							= 0;
		licenseInfo->conferencing					= 0;
		licenseInfo->tts							= 0;
		licenseInfo->speechIntegration				= 0;
		licenseInfo->enhancedRTP					= 0;
		licenseInfo->voiceRecognitionEnginePorts	= 0;
		licenseInfo->errorCode						= LM_NOERROR;
	}

    for (int c = 0; c < CUME_COUNTED_FEATURE_COUNT; c++)
    {
        featureCount = availableFeatureCounts[featuresToRetrieve[c]];

        status = getCountedFeature(true,lm_job_CUME, &vendorCodeCUME, featuresToRetrieve[c],CUME_FEATURE_VERSION, featureCount);
	
        if (status == LM_NOERROR)
        {
            setCountedMediaLicenseValue(licenseInfo, featuresToRetrieve[c], featureCount);
        }
        else
        {
            licenseInfo->errorCode = status;
            setCountedMediaLicenseValue(licenseInfo, featuresToRetrieve[c], 0);
        }
    }

	WriteLogLicManager("CUAELiceMgr.log","Exit getMediaLicenseInfo\n");


    return status;     
}

///////////////////////////////////////////////////////////
//  getAppServerLicenseInfo
//  
//  Get App Server Lic Info
//  
//
///////////////////////////////////////////////////////////
OEMFUNC_API int getAppServerLicenseInfo(LicenseInformationCUAE *licenseInfo)
{
	WriteLogLicManager("CUAELiceMgr.log","Enter getAppServerLicenseInfo\n");

	#ifdef CUAE_TEST
    return loadCuaeTestData(licenseInfo);
    #endif
    int status = initialize(CUAE);

    if (status != LM_NOERROR)
    {
        licenseInfo->errorCode = status;
        return status;
    }
    licenseInfo->errorCode = LM_NOERROR;

	WriteLogLicManager("CUAELiceMgr.log","After Init\n");

	VENDORCODE* vendor		= NULL;
	LM_HANDLE *jobHandle	= NULL;
	vendor = &vendorCodeCUAE;
	jobHandle = lm_job_CUAE;

	int nDays = 1;
	int nVal = getLicenseMode(CUAE, &licenseInfo->errorCode);
    // obtain license mode
    switch (nVal)
    {
        case SmallEnv: 
			{
				nDays = nGetExpiredCUAE("SmallEnv");
				WriteLogLicManager("CUAELiceMgr.log","SmallEnv\n");

				licenseInfo->licenseMode = (char**) CUAE_LICENSE_MODE_SMALLENV; 
				licenseInfo->licenseModeThreshold = CUAE_MODE_LIMIT_SMALLENV; 
				licenseInfo->nMode = SmallEnv;
			}
			break;

		case Standard: 
			{
				nDays = nGetExpiredCUAE("Standard");
				licenseInfo->licenseMode = (char**) CUAE_LICENSE_MODE_STANDARD; 
				licenseInfo->licenseModeThreshold = CUAE_MODE_LIMIT_STANDARD; 
				licenseInfo->nMode = Standard;
			}
			break;

        case Enhanced: 
			{
				nDays = nGetExpiredCUAE("Enhanced");
				licenseInfo->licenseMode = (char**) CUAE_LICENSE_MODE_ENHANCED; 
				licenseInfo->licenseModeThreshold = CUAE_MODE_LIMIT_ENHANCED; 
				licenseInfo->nMode = Enhanced;
			}
			break;

        case Premium:
			{
				nDays = nGetExpiredCUAE("Premium");
				licenseInfo->licenseMode = (char**) CUAE_LICENSE_MODE_PREMIUM; 
				licenseInfo->licenseModeThreshold = CUAE_MODE_LIMIT_PREMIUM;
				licenseInfo->nMode = Premium;
			}
			break;
	
        case StdEnh:
			{
				nDays = nGetExpiredCUAE("StdEnh");
				int nStatus = -1;
				nStatus = lc_checkout(jobHandle,"Standard",CUAE_FEATURE_VERSION,1,LM_CO_WAIT, vendor, LM_DUP_NONE);
				lc_checkin(jobHandle,"Standard",LM_CO_WAIT);
				if(nStatus == 0)
				{
					licenseInfo->licenseMode = (char**) CUAE_LICENSE_MODE_STDENH; 
					licenseInfo->licenseModeThreshold = CUAE_MODE_LIMIT_ENHANCED;
					licenseInfo->nMode = StdEnh;
				}
				else
				{
					licenseInfo->nMode = SDK;
					licenseInfo->licenseMode = (char**) CUAE_LICENSE_MODE_SDK;
					licenseInfo->licenseModeThreshold	= CUAE_MODE_LIMIT_SDK; 
					licenseInfo->scriptInstances		= CUAE_MODE_LIMIT_SDK;
					licenseInfo->errorCode				= LM_NOERROR;
				}
			}
			break;

        case StdPrem:
			{
				nDays = nGetExpiredCUAE("StdPrem");
				int nHaveStd = -1;
				nHaveStd = lc_checkout(jobHandle,"Standard",CUAE_FEATURE_VERSION,1,LM_CO_WAIT, vendor, LM_DUP_NONE);
				lc_checkin(jobHandle,"Standard",LM_CO_WAIT);

				int nHaveStdPrem = -1;
				nHaveStdPrem = lc_checkout(jobHandle,"StdPrem",CUAE_FEATURE_VERSION,1,LM_CO_WAIT, vendor, LM_DUP_NONE);
				lc_checkin(jobHandle,"StdPrem",LM_CO_WAIT);

				//if I have Enh or StdEnh annd EnhPrm
				if( nHaveStd == 0 && nHaveStdPrem == 0)
				{ 
					licenseInfo->licenseMode = (char**) CUAE_LICENSE_MODE_STDPREM; 
					licenseInfo->licenseModeThreshold = CUAE_MODE_LIMIT_PREMIUM;
					licenseInfo->nMode = EnhPrem;
				}
				else
				{
					licenseInfo->nMode = SDK;
					licenseInfo->licenseMode = (char**) CUAE_LICENSE_MODE_SDK;
					licenseInfo->licenseModeThreshold	= CUAE_MODE_LIMIT_SDK; 
					licenseInfo->scriptInstances		= CUAE_MODE_LIMIT_SDK;
					licenseInfo->errorCode				= LM_NOERROR;
				}
			}
			break;

        case EnhPrem:
			{
				nDays = nGetExpiredCUAE("EnhPrem");
				int nHaveEnh = -1;
				nHaveEnh = lc_checkout(jobHandle,"Enhanced",CUAE_FEATURE_VERSION,1,LM_CO_WAIT, vendor, LM_DUP_NONE);
				lc_checkin(jobHandle,"Enhanced",LM_CO_WAIT);

				int nHaveEnPrem = -1;
				nHaveEnPrem = lc_checkout(jobHandle,"EnhPrem",CUAE_FEATURE_VERSION,1,LM_CO_WAIT, vendor, LM_DUP_NONE);
				lc_checkin(jobHandle,"EnhPrem",LM_CO_WAIT);

				int nHaveStdEnh = -1;
				nHaveStdEnh = lc_checkout(jobHandle,"StdEnh",CUAE_FEATURE_VERSION,1,LM_CO_WAIT, vendor, LM_DUP_NONE);
				lc_checkin(jobHandle,"StdEnh",LM_CO_WAIT);

				//if I have Enh or StdEnh annd EnhPrm
				if( nHaveEnPrem == 0 && (nHaveEnh == 0 || nHaveStdEnh == 0))
				{ 
					licenseInfo->licenseMode = (char**) CUAE_LICENSE_MODE_ENHPREM; 
					licenseInfo->licenseModeThreshold = CUAE_MODE_LIMIT_PREMIUM;
					licenseInfo->nMode = EnhPrem;
				}
				else
				{
					licenseInfo->nMode = SDK;
					licenseInfo->licenseMode = (char**) CUAE_LICENSE_MODE_SDK;
					licenseInfo->licenseModeThreshold	= CUAE_MODE_LIMIT_SDK; 
					licenseInfo->scriptInstances		= CUAE_MODE_LIMIT_SDK;
					licenseInfo->errorCode				= LM_NOERROR;
				}
			}
			break;

        case SDK:
        default: 
			{
				int nError = 0;
				int nMode = getLicenseMode(CUME, &nError);

				if (nMode != SDK)
				{
					licenseInfo->nMode = NOLIC;
					licenseInfo->licenseMode = (char**) LICENSE_MODE_NOLIC;
					licenseInfo->licenseModeThreshold	= 0; 
					licenseInfo->scriptInstances		= 0;
					licenseInfo->errorCode				= LM_NOERROR;
				}
				else
				{
					licenseInfo->nMode = SDK;
					licenseInfo->licenseMode = (char**) CUAE_LICENSE_MODE_SDK;
					licenseInfo->licenseModeThreshold	= CUAE_MODE_LIMIT_SDK; 
					licenseInfo->scriptInstances		= CUAE_MODE_LIMIT_SDK;
					licenseInfo->errorCode				= LM_NOERROR;
				}
				return LM_NOERROR;
			}
    }

    int featureCount = availableFeatureCounts[CUAE_SCRIPT_INSTANCES];
    status = getCountedFeature(true,lm_job_CUAE, &vendorCodeCUAE, CUAE_SCRIPT_INSTANCES, CUAE_FEATURE_VERSION, featureCount);

    if (status == LM_NOERROR)
    {

		//Ok if I am over the lic limit
		if(featureCount > licenseInfo->licenseModeThreshold)
		{
			 licenseInfo->scriptInstances = licenseInfo->licenseModeThreshold;
		}
		else
		{
			licenseInfo->scriptInstances = featureCount;
		}
    }
    else
    {
        licenseInfo->errorCode = status;
		// TODO: examine if this is proper
        licenseInfo->scriptInstances = 0;
	
	}

	if(nDays <= 0)
	{
		licenseInfo->nMode = EXPIRED;
		licenseInfo->licenseMode = (char**) LICENSE_MODE_EXPIRED;
		licenseInfo->licenseModeThreshold	= 0; 
		licenseInfo->scriptInstances		= 0;
		licenseInfo->errorCode				= LM_NOERROR;
	}

    return status;
}

///////////////////////////////////////////////////////////
//  FileSize64
//  
//  Helper Function
//  
//
///////////////////////////////////////////////////////////
__int64 FileSize64( const char * szFileName ) 
{ 
  struct __stat64 fileStat; 
  int err = _stat64( szFileName, &fileStat ); 
  if (0 != err) return 0; 
  return fileStat.st_size; 
}

// validates that all features at the provided license path, given in either
// FLEXlm server path format or a filesystem pathname, have valid signatures.
// This validates that the license file was not tampered with.
OEMFUNC_API int old_validateLicenseFile(char* licensePath)
{
    char **myFeatures = 0; 
    LM_HANDLE *lm_job = 0;
    VENDORCODE vendorCode;
	LM_CHAR_PTR pData = 0;

    int status = lc_new_job(NULL, NULL, &vendorCode, &lm_job);            
    if (status != LM_NOERROR)
	{
		return status;
	}

    // if there was no error, we set a bunch of attributes
    int result = setDefaultJobAttributes(lm_job);
    lc_set_attr(lm_job, LM_A_LICENSE_DEFAULT, (LM_A_VAL_TYPE) licensePath);
    if (result != LM_NOERROR)
	{
		return result;   
	}

    // get a list of all available features 
    try
    {
       myFeatures = lc_feat_list(lm_job,LM_FLIST_ALL_FILES,NULL);
       result = myFeatures? LM_NOERROR: LM_BADPARAM;
    }
    catch(...) { result = CUAE_UNHANDLED_EXCEPTION; }
   
    if (result != LM_NOERROR)
	{
		return result;   
	}

    int i = 0;

    // for each member of the list 
    while (myFeatures[i] != NULL) 
    { 
        CONFIG *pos  = 0; 
        CONFIG *conf = 0;
        result = LM_NOERROR; 

        //get the config struct of this feature 
        while((conf = lc_next_conf(lm_job,myFeatures[i],&pos)) != NULL) 
        { 
            //get generic feature info for this config struct 
            try
            {
              result = lc_check_key(lm_job, conf, &vendorCode);
              if (result != LM_NOERROR)
			  {
				  lm_perror(pData);
                  return result;
			  }
            }
            catch(...) { result = CUAE_UNHANDLED_EXCEPTION; }
        }

        if (result == CUAE_UNHANDLED_EXCEPTION) break;

        i++;
    } 
 
    return result;
}


///////////////////////////////////////////////////////////
//  validateLicenseFile
//  
//  Keyword Validate Lic File
//  
//
///////////////////////////////////////////////////////////
OEMFUNC_API int validateLicenseFile(char* licensePath)
{
	FILE *fp = 0;
	fp = fopen(licensePath,"r+");
	
	if(fp == 0)
	{
		return 0;
	}

	size_t nLen = (size_t)FileSize64(licensePath);

	char *pBuffer = new char[nLen+1];
	memset(pBuffer,0,nLen+1);
	fread(pBuffer,nLen,1,fp);
	fclose(fp);

	std::string oStr(pBuffer);
	delete [] pBuffer;

	int nVal = (int)oStr.find("SERVER",0);
	if(nVal < 0)
	{
		return 1;
	}

	nVal = (int)oStr.find("VENDOR",0);
	if(nVal < 0)
	{
		return 1;
	}

	nVal = (int)oStr.find("<LicFileID>",0);
	if(nVal < 0)
	{
		return 1;
	}

	nVal = (int)oStr.find("<PAK>",0);
	if(nVal < 0)
	{
		return 1;
	}

	nVal = (int)oStr.find("SIGN=",0);
	if(nVal < 0)
	{
		return 1;
	}

return 0;
}


