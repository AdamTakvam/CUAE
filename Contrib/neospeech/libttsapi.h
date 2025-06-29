#include <windows.h>

/* tts api return values */
#define TTS_HOSTNAME_ERROR      -1
#define TTS_SOCKET_ERROR        -2
#define TTS_CONNECT_ERROR       -3
#define TTS_READWRITE_ERROR     -4
#define TTS_MEMORY_ERROR        -5
#define TTS_TEXT_ERROR			-6
#define TTS_VOICEFORMAT_ERROR   -7
#define TTS_PARAM_ERROR         -8
#define TTS_RESULT_ERROR        -9
#define TTS_SPEAKER_ERROR		-10
#define TTS_DISK_ERROR			-11
#define TTS_UNKNOWN_ERROR		-12
#define TTS_MAX_ERROR           -100
#define TTS_RESULT_CONTINUE     0
#define TTS_RESULT_SUCCESS      1

#define TTS_SERVICE_ON		1
#define TTS_SERVICE_OFF		0
#define TTS_SERVICE_PAUSED	-1

// TTS Speaker Id Info
#define TTS_JIHAE_DB	0
#define TTS_MINHO_DB	1
#define TTS_EUNJU_DB	2
#define TTS_JUNWOO_DB	3
#define TTS_SUNYOUNG_DB	6
#define TTS_SUJIN_DB	8
#define TTS_YUMI_DB		10
#define TTS_KATE_DB		100
#define TTS_PAUL_DB		101
#define TTS_LILY_DB		200
#define TTS_WANG_DB		201
#define TTS_MIYU_DB		300
#define TTS_SHOW_DB		301

// Voice Format Info
#define FORMAT_DEFAULT	0
#define FORMAT_WAV		1
#define FORMAT_PCM		2
#define FORMAT_MULAW	3
#define FORMAT_ALAW		4
#define FORMAT_ADPCM	5
#define FORMAT_ASF		6
#define FORMAT_WMA		7
#define FORMAT_32ADPCM	8
#define FORMAT_MP3		9
#define FORMAT_OGG		10
#define FORMAT_8BITWAV	11
#define FORMAT_AWAV		12
#define FORMAT_MUWAV	13
#define FORMAT_ADWAV	14
#define FORMAT_G726		15
#define FORMAT_8BITPCM	16

/* text sorts */
#define TEXT_NORMAL		0
#define TEXT_VXML		1
#define TEXT_HTML		2
#define TEXT_EMAIL		3
#define TEXT_7BIT		4

#ifndef DllExport
#define DllExport __declspec(dllexport) 
#endif

#ifndef DllImport
#define DllImport __declspec(dllimport) 
#endif

#if defined(__cplusplus)
	extern "C" {
#endif

DllImport int TTSRequestFile(char *szServer, int nPort, char *pText, int nTextLen, char *szSaveDir, char *szSaveFile, int nSpeakerID, int nVoiceFormat);
DllImport char *TTSRequestBuffer(SOCKET *sockfd, char *szServer, int nPort, char *pText, int nTextLen, int *nVoiceLen, int nSpeakerID, int nVoiceFormat, BOOL bFirst, BOOL bAll, int *nReturn);
DllImport int TTSRequestEffect(char *szServer, int nPort, char *pText, int nTextLen, char *szSaveDir, char *szSaveFile, char *szBackMusicFile, int nSpeakerID, int nVoiceFormat, int nEffectID);

// expanded format functions, for volume, speed, pitch, dictionary number
DllImport int TTSRequestFileEx(char *szServer, int nPort, char *pText, int nTextLen, char *szSaveDir, char *szSaveFile, int nSpeakerID, int nVoiceFormat, int nTextFormat, int nVolume, int nSpeed, int nPitch, int nDictIndex);
DllImport char *TTSRequestBufferEx(SOCKET *sockfd, char *szServer, int nPort, char *pText, int nTextLen, int *nVoiceLen, int nSpeakerID, int nVoiceFormat, int nTextFormat, int nVolume, int nSpeed, int nPitch, int nDictIndex, BOOL bFirst, BOOL bAll, int *nReturn);

DllImport void TTSBufferFree(char *pBuffer);
DllImport int TTSRequestStatus(char *szServer, int nPort);
DllImport void TTSSetTimeout(int nConnTime, int nReadWriteTime);

#if defined(__cplusplus)
	}
#endif
