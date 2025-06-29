//
// CUAELicMgr.h -- licensing manager interface
//
#include "lm_attr.h"
#include "lmclient.h"
#include "time.h"
#include <iostream>

#ifdef  OEMFUNC_EXPORTS
#define OEMFUNC_API __declspec(dllexport)

#else
#define OEMFUNC_API __declspec(dllimport)
#endif

#define DEFAULT_LICPATH "@localhost"

// number of license modes supported by each product, excluding SDK mode, which is implied by a lack of license.
#define CUME_LICENSE_MODE_COUNT		2
#define CUAE_LICENSE_MODE_COUNT		7

enum CUME_LIC_LIMIT
{
	RTP_LIC_LIMIT = 0,
	VOICE_LIC_LIMIT,
	CONF_LIC_LIMIT,
	SPEECH_LIC_LIMIT,
	TTS_LIC_LIMIT,
	ENHANCED_LIC_LIMIT
};

#define CUME_RTP "RTP-CUME"
#define CUME_VOICE "Voice-CUME"
#define CUME_CONFERENCING "Conferencing-CUME"
#define CUME_SPEECH_INTEGRATION "SpeechIntegration-CUME"
#define CUME_ENHANCED_RTP "EnhancedRTP-CUME"
#define CUME_TTS_ENGINE_PORTS "TTS-CUME"
#define CUME_VOICE_REC_ENG_PORTS "VoiceRecognitionEnginePorts-CUME"
#define CUME_LICENSE_MODE_STANDARD "Media"
#define CUME_LICENSE_MODE_SMALLENV "MediaSmallEnv"
#define CUME_DEFAULT_SDK_RESOURCE_COUNT 6

#define CUME_MODE_LIMIT_PREMIUM  (1000)
#define CUME_MODE_LIMIT_SDK        6
#define CUME_MODE_SMALLENV        16
#define CUME_MODE_LIMIT_MEDIA    240
#define CUME_MODE_LIMIT_ENHANCED 240

#define CUAE_MODE_LIMIT_SMALLENV	16
#define CUAE_MODE_LIMIT_SDK			6
#define CUAE_MODE_LIMIT_STANDARD	50
#define CUAE_MODE_LIMIT_ENHANCED	150
#define CUAE_MODE_LIMIT_PREMIUM		(9999)

#define CUME_FEATURE_VERSION "2.9"
#define CUME_COUNTED_FEATURE_COUNT 6

#define CUAE_LICENSE_MODE_SDK      "SDK"
#define CUAE_LICENSE_MODE_SMALLENV "SmallEnv"
#define CUAE_LICENSE_MODE_STANDARD "Standard"
#define CUAE_LICENSE_MODE_ENHANCED "Enhanced"
#define CUAE_LICENSE_MODE_PREMIUM  "Premium"
#define CUAE_LICENSE_MODE_STDENH   "StdEnh"
#define CUAE_LICENSE_MODE_STDPREM  "StdPrem"
#define CUAE_LICENSE_MODE_ENHPREM  "EnhPrem"
#define CUAE_SCRIPT_INSTANCES "ScriptInstances"
#define LICENSE_MODE_NOLIC "No License"
#define LICENSE_MODE_EXPIRED "Expired"

#define CUAE_FEATURE_VERSION "2.9"


#define CUAE_UNHANDLED_EXCEPTION (-32768)
#define LI_SIG 0xa1b2c3d4

enum OEMFUNC_API Products
{
    CUAE,
    CUME
};

enum OEMFUNC_API LicenseModes
{
    SDK			= 0,
	SmallEnv	= 1,
    Standard	= 2,
    Enhanced	= 3,
	StdEnh		= 4,
	StdPrem		= 5,
	EnhPrem		= 6,
	Premium		= 7,
	NOLIC		= 8,
	EXPIRED		= 9,
 };

extern "C" 
{
    struct OEMFUNC_API LicenseInformationCUME
    {
        int length;
        int signature;
        int licenseLimit;
	    int rtp;
        int voice;
        int conferencing;
        int speechIntegration;
        int enhancedRTP;
        int tts;
        int voiceRecognitionEnginePorts;
        int errorCode; 
		int nMode;
		int licenseLimitArray[6];

    };

    struct OEMFUNC_API LicenseInformationCUAE
    {
        char** licenseMode;
		int licenseModeThreshold;
        int scriptInstances;
        int errorCode;
		int nMode;
    };

    OEMFUNC_API int getMediaLicenseInfo(LicenseInformationCUME*);

    OEMFUNC_API int getAppServerLicenseInfo(LicenseInformationCUAE*);

    OEMFUNC_API int validateLicenseFile(char*);
	
	OEMFUNC_API int old_validateLicenseFile(char*);

	
}
