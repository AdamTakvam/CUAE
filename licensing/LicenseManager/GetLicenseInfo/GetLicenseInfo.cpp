// GetLicenseInfo.cpp : Defines the entry point for the console application.
//

#include "stdafx.h"
#include <windows.h>
#include <stdio.h>
#include <conio.h>
#include <string>
#include <iostream>
#include <errno.h>

#include "CUAELicMgr.h"

typedef int (*importFunction)(LicenseInformationCUAE *);
typedef int (*importFunction1)(LicenseInformationCUME *);
typedef int (*importFunction2)(char *);

void vPrintError(void)
{
		printf("\nPlease enter in the CUAE feature you would like license information on.\n");
			
		printf("-appservermax\n");
		printf("-maxrtp\n");
		printf("-maxvoice\n");
		printf("-maxenhanced\n");
		printf("-maxconf\n");
		printf("-maxspeech\n");
		printf("-maxtts\n");
		printf("-appserver\n");
		printf("-voiceports\n");
		printf("-rtpports\n");
		printf("-enhancedrtpports\n");
		printf("-conferenceports\n");
		printf("-speechports\n");
		printf("-tts\n");
		printf("-all\n");
		printf("-modeCUAE\n");
		printf("-modeCUME\n");
		fflush(stdout);
}



void vPrintMode(int nMode,char *pType)
{
		switch(nMode)
		{

			case EXPIRED:
				{
					printf("[%s]-Expired\n",pType);
				};
				break;

			case NOLIC:
				{
					printf("[%s]-No License\n",pType);
				};
				break;

			case SDK:
				{
					printf("[%s]-SDK\n",pType);
				};
				break;

			case SmallEnv: 
				{
					printf("[%s]-SmallEnv\n",pType);
				};
				break;

			case Standard: 
				{
					printf("[%s]-Standard\n",pType);
				};
				break;

			case Enhanced: 
				{
					printf("[%s]-Enhanced\n",pType);
				};
				break;

			case Premium:
				{
					printf("[%s]-Premium\n",pType);
				};
				break;

			case StdEnh:  
				{
					printf("[%s]-Standard Enhanced\n",pType);
				};
				break;

			case StdPrem: 
				{
					printf("[%s]-Standard Premium\n",pType);
				};
				break;

			case EnhPrem:
				{
					printf("[%s]-Enhanced Premium\n",pType);
				};
				break;
			
			default:
				{
					printf("[%s]-Unknown[%d]\n",pType,nMode);
				};
				break;

			}
}

int _tmain(int argc, char* argv[])
{

	if(argc != 2)
	{
		vPrintError();
		return 1;
	}

	std::string oStr(&argv[1][1]);

	importFunction licenseFunction;
	importFunction1 licenseFunction1;
	importFunction2 licenseFunction2;


	HINSTANCE hinstLib = LoadLibrary("..\\..\\LicenseManager\\Debug\\CUAELicMgr.dll");
    if (hinstLib == NULL)
	{
		hinstLib = LoadLibrary("..\\..\\Debug\\CUAELicMgr.dll");
		if (hinstLib == NULL)
		{
			hinstLib = LoadLibrary(".\\CUAELicMgr.dll");
			if (hinstLib == NULL)
			{
				hinstLib = LoadLibrary(".\\CUAELicMgr.dll");
				if (hinstLib == NULL)
				{
						printf("ERROR: unable to load DLL\n");
						return 1;
				}
			}
		}

    }


    LicenseInformationCUAE licInfo;
    memset(&licInfo, 0, sizeof(LicenseInformationCUAE));
  	licenseFunction = (importFunction)GetProcAddress(hinstLib, "getAppServerLicenseInfo");
    if (licenseFunction == NULL) {
            printf("ERROR: unable to find DLL function\n");
            FreeLibrary(hinstLib);
            return 1;
    }

    LicenseInformationCUME licInfo1;
    memset(&licInfo1, 0, sizeof(LicenseInformationCUME));
    licInfo1.length = sizeof(LicenseInformationCUME);
    licInfo1.signature = LI_SIG;
    licenseFunction1 = (importFunction1)GetProcAddress(hinstLib, "getMediaLicenseInfo");

    if (licenseFunction1 == NULL) {
            printf("ERROR: unable to find DLL function\n");
            FreeLibrary(hinstLib);
            return 1;
    }


    licenseFunction2 = (importFunction2)GetProcAddress(hinstLib, "old_validateLicenseFile");
    if (licenseFunction2 == NULL) {
           printf("ERROR: unable to find DLL function\n");
            FreeLibrary(hinstLib);
            return 1;
    }

	licenseFunction(&licInfo);
  	licenseFunction1(&licInfo1);

	int nCUAEMode = licInfo.nMode;
	int nCUMEMode = licInfo1.nMode;

	bool nFound = false;

	int nVal = (int)oStr.find("validate",0);
	if(nVal > -1)
	{
		int nVal1 = licenseFunction2("C:\\test.lic");
		
		int xxx = 0;

	}
	nVal = (int)oStr.find("all",0);
	if(nVal > -1)
	{

		printf("AppServerMax-%d\n",licInfo.licenseModeThreshold);
		printf("VoiceMax-%d\n",licInfo1.licenseLimitArray[VOICE_LIC_LIMIT]);
		printf("RTPMax-%d\n",licInfo1.licenseLimitArray[RTP_LIC_LIMIT]);
		printf("EnhancedMax-%d\n",licInfo1.licenseLimitArray[ENHANCED_LIC_LIMIT]);
		printf("ConfMax-%d\n",licInfo1.licenseLimitArray[CONF_LIC_LIMIT]);
		printf("SpeechMax-%d\n",licInfo1.licenseLimitArray[SPEECH_LIC_LIMIT]);
		printf("TTSMax-%d\n",licInfo1.licenseLimitArray[TTS_LIC_LIMIT]);

		printf("AppServer-%d\n",licInfo.scriptInstances);
		printf("VoicePorts-%d\n",licInfo1.voice);
		printf("RTP-%d\n",licInfo1.rtp);
		printf("EnhancedRTP-%d\n",licInfo1.enhancedRTP);
		printf("Conference-%d\n",licInfo1.conferencing);
		printf("Speech-%d\n",licInfo1.speechIntegration);
		printf("TTS-%d\n",licInfo1.tts);
		vPrintMode(nCUAEMode,"CUAE");
		vPrintMode(nCUMEMode,"CUME");

		nFound = true;
	}


	nVal = (int)oStr.find("validate",0);
	if(nVal > -1)
	{
		int nStatus = licenseFunction2("c:\\test.lic");
		printf("Validating file [%s]\n","C:\\test.lic");
		fflush(stdout);
		nFound = true;
	}

	nVal = (int)oStr.find("maxrtp",0);
	if(nVal > -1)
	{
		printf("%d\n",licInfo1.licenseLimitArray[RTP_LIC_LIMIT]);
		fflush(stdout);
		nFound = true;
	}

	nVal = (int)oStr.find("maxvoice",0);
	if(nVal > -1)
	{
		printf("%d\n",licInfo1.licenseLimitArray[VOICE_LIC_LIMIT]);
		fflush(stdout);
		nFound = true;
	}

	nVal = (int)oStr.find("maxenhanced",0);
	if(nVal > -1)
	{
		printf("%d\n",licInfo1.licenseLimitArray[ENHANCED_LIC_LIMIT]);
		fflush(stdout);
		nFound = true;
	}

	nVal = (int)oStr.find("maxconf",0);
	if(nVal > -1)
	{
		printf("%d\n",licInfo1.licenseLimitArray[CONF_LIC_LIMIT]);
		fflush(stdout);
		nFound = true;
	}

	nVal = (int)oStr.find("maxspeech",0);
	if(nVal > -1)
	{
		printf("%d\n",licInfo1.licenseLimitArray[SPEECH_LIC_LIMIT]);
		fflush(stdout);
		nFound = true;
	}

	nVal = (int)oStr.find("maxtts",0);
	if(nVal > -1)
	{
		printf("%d\n",licInfo1.licenseLimitArray[TTS_LIC_LIMIT]);
		fflush(stdout);
		nFound = true;
	}

	nVal = (int)oStr.find("appservermax",0);
	if(nVal > -1)
	{
		printf("%d",licInfo.licenseModeThreshold);
		fflush(stdout);
		nFound = true;
	}

	nVal = (int)oStr.find("appserver",0);
	if(nVal > -1  && !nFound)
	{
		printf("%d",licInfo.scriptInstances);
		fflush(stdout);
		nFound = true;
	}

	nVal = (int)oStr.find("voiceports",0);
	if(nVal > -1)
	{
		printf("%d",licInfo1.voice);
		fflush(stdout);
		nFound = true;
	}


	nVal = (int)oStr.find("enhancedrtpports",0);
	if(nVal > -1)
	{
		printf("%d",licInfo1.enhancedRTP);
		fflush(stdout);
		nFound = true;
	}

	nVal = (int)oStr.find("rtpports",0);
	if(nVal > -1 && !nFound)
	{
		printf("%d",licInfo1.rtp);
		fflush(stdout);
		nFound = true;
	}

	nVal = (int)oStr.find("conferenceports",0);
	if(nVal > -1)
	{
		printf("%d",licInfo1.conferencing);
		fflush(stdout);
		nFound = true;
	}

	nVal = (int)oStr.find("speechports",0);
	if(nVal > -1)
	{
		printf("%d",licInfo1.speechIntegration);
		fflush(stdout);
		nFound = true;
	}

	nVal = (int)oStr.find("tts",0);
	if(nVal > -1)
	{
		printf("%d",licInfo1.tts);
		fflush(stdout);
		nFound = true;
	}

	nVal = (int)oStr.find("modeCUAE",0);
	if(nVal > -1)
	{
		vPrintMode(nCUAEMode,"CUAE");
		fflush(stdout);
		nFound = true;
	}

	nVal = (int)oStr.find("modeCUME",0);
	if(nVal > -1)
	{
		vPrintMode(nCUMEMode,"CUME");
		fflush(stdout);
		nFound = true;
	}


	if(nFound == false)
	{
		vPrintError();
		return 1;
	}

	FreeLibrary(hinstLib);

	return 0;
}

