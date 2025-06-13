// Flatmap.h

#ifndef _FLATMAPIPCCLIENTWRAPPER_H_
#define _FLATMAPIPCCLIENTWRAPPER_H_

#include "cwrapper.h"


/////////////////////////////////////////////////////////////////////
// Callback function prototype for IPC data notifier
typedef int (*IncomingCallNotifier)	(const char* to, const char* from, const char* originalTo, const char* callId, void* userInterface);
typedef int (*CallActiveNotifier)	(const char* to, const char* callId, void* userInterface);
typedef int (*CallInactiveNotifier)	(int inUse, const char* callId, void* userInterface);
typedef int (*HangupNotifier)		(const char* cause, const char* callId, void* userInterface);
typedef int (*InitiateCallNotifier)	(const char* callId, const char* to, const char* from, void* userInterface);
typedef int (*LoginNotifier)		(const char* deviceName, int lineDnCount, char lineDns[5][25], void* userInterface);
typedef int (*LogCallback)			(const char* log);

/////////////////////////////////////////////////////////////////////
#ifdef __cplusplus
extern "C" {
#endif

CWRAPPER_API int createFlatmapIpcClient(void* userInterface);
CWRAPPER_API void destroyFlatmapIpcClient();
CWRAPPER_API int isFlatmapIpcClientAlive();
CWRAPPER_API int connectToFlatmapIpcServer(const char* pszServer, const unsigned int port);
CWRAPPER_API void disconnectFromFlatmapIpcServer();
CWRAPPER_API int isFlatmapIpcClientConnected();

CWRAPPER_API void assignIncomingCallNotifier(IncomingCallNotifier pFunc);
CWRAPPER_API void assignCallActiveNotifier(CallActiveNotifier pFunc);
CWRAPPER_API void assignCallInactiveNotifier(CallInactiveNotifier pFunc);
CWRAPPER_API void assignHangupNotifier(HangupNotifier pFunc);
CWRAPPER_API void assignInitiateCallNotifier(InitiateCallNotifier pFunc);
CWRAPPER_API void assignLoginNotifier(LoginNotifier pFunc);
CWRAPPER_API void assignLogCallback(LogCallback pFunc);

CWRAPPER_API int sendLogin(const char* deviceName);
CWRAPPER_API int sendLogout(const char* deviceName);
CWRAPPER_API int sendMakeCall(const char* deviceName, const char* to);
CWRAPPER_API int sendHangup(const char* deviceName, const char* callId);



#ifdef __cplusplus
}
#endif

#endif
