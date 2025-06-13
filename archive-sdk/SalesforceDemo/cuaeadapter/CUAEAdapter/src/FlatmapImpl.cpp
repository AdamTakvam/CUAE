// FlatmapImlp.cpp

// Outgoing Messages
#define MESSAGE_LOGIN					1000					/* Login message sent */
#define MESSAGE_LOGOUT					1001					/* Logout message sent */
#define MESSAGE_MAKECALL				1002					/* Make call message sent */
#define MESSAGE_HANGUP					1003					/* Hangup message sent, ALSO INCOMINg */
#define MESSAGE_ANSWER					1004					/* Answer message sent */

// Incoming Message
#define MESSAGE_INCOMING				1050					/* Incoming call message received */
#define MESSAGE_CALLACTIVE				1051					/* Call active message received */
#define MESSAGE_CALLINACTIVE			1052					/* Call inactive message received */
#define MESSAGE_INITIATE				1053					/* An ack for a CallInitiate */
#define MESSAGE_LOGIN_ACK				1054					/* An ack for login request */

// Message Parameters
#define PARAM_DEVICENAME				2000
#define PARAM_TO						2001
#define PARAM_FROM						2002
#define PARAM_ORIGINALTO				2003
#define PARAM_CALLID					2004
#define PARAM_INUSE						2005
#define PARAM_CAUSE						2006
#define PARAM_LINEDN_COUNT				2007

#define PARAM_LINEDN_START				3000

#include "FlatmapImpl.h"
#include "IPC/FlatmapIpcclient.h"

using namespace Metreos;
using namespace Metreos::IPC;

IncomingCallNotifier icn = 0;		// IncomingCall Callback
CallActiveNotifier can = 0;			// Call Active Callback
CallInactiveNotifier cin = 0;		// Call Inactive Callback 
HangupNotifier hn = 0;				// Hangup Callback
InitiateCallNotifier ican = 0;		// InitiateCall Callback
LoginNotifier ln = 0;				// Login Notifier Callback
LogCallback lc = 0;					// Log callback
/////////////////////////////////////////////////////////////////////
// Assign callback function pointer 
void assignIncomingCallNotifier(IncomingCallNotifier pFunc)
{
	if (icn != 0)
		return;

	icn = pFunc;
}

/////////////////////////////////////////////////////////////////////
// Assign callback function pointer 
void assignCallActiveNotifier(CallActiveNotifier pFunc)
{
	if (can != 0)
		return;

	can = pFunc;
}

/////////////////////////////////////////////////////////////////////
// Assign callback function pointer 
void assignCallInactiveNotifier(CallInactiveNotifier pFunc)
{
	if (cin != 0)
		return;

	cin = pFunc;
}

/////////////////////////////////////////////////////////////////////
// Assign callback function pointer 
void assignHangupNotifier(HangupNotifier pFunc)
{
	if (hn != 0)
		return;

	hn = pFunc;
}

/////////////////////////////////////////////////////////////////////
// Assign callback function pointer 
void assignInitiateCallNotifier(InitiateCallNotifier pFunc)
{
	if (ican != 0)
		return;

	ican = pFunc;
}

/////////////////////////////////////////////////////////////////////
// Assign callback function pointer 
void assignLoginNotifier(LoginNotifier pFunc)
{
	if (ln != 0)
		return;

	ln = pFunc;
}

/////////////////////////////////////////////////////////////////////
// Assign callback function pointer 
void assignLogCallback(LogCallback pFunc)
{
	if (lc != 0)
		return;

	lc = pFunc;
}

/////////////////////////////////////////////////////////////////////
// The C wrapper class for IPC client
class CWFlatMapIpcClient : public FlatMapIpcClient
{
public:
	virtual void OnIncomingFlatMapMessage(const int messageType, const FlatMapReader& flatmap);
	virtual void OnConnected();
	virtual void OnDisconnected();
	virtual void OnFailure();
};


/////////////////////////////////////////////////////////////////////
static CWFlatMapIpcClient* pClient = NULL;          // The one and only flatmap client
static BOOL bConnected = FALSE;                     // Connection status flag
static char szFlatmapIpcServer[32] = {0};           // IPC server IP address
static unsigned int uFlatmapIpcPort = 10000;         // Default IPC port
static HANDLE hMutexReceive = NULL;                 // Receive mutex object
static HANDLE hMutexSend = NULL;                    // Send mutex object
static void* parent;						// CUAE Interface reference to pass back in callbacks

/////////////////////////////////////////////////////////////////////
// Incoming flatmap data handler
void CWFlatMapIpcClient::OnIncomingFlatMapMessage(const int messageType, const FlatMapReader& flatmap)
{
	if (hMutexReceive == NULL)
	{
		hMutexReceive = CreateMutex(NULL, FALSE, L"Flatmap Calback");
	}

    // Gaining the permission to read data
    DWORD dwWaitResult = WaitForSingleObject(hMutexReceive, 30000L);

	//if (dwWaitResult != WAIT_OBJECT_0)
	//	return;

	FlatMapReader r = flatmap;

	int stringType = (int)FlatMap::STRING;
	int intType = (int)FlatMap::INT;

	char* to = NULL;
	char* from = NULL;
	char* originalTo = NULL;
	char* callId = NULL;
	char* inUse = NULL;
	char* cause = NULL;
	char* deviceName = NULL;
	char* lineDnCount = NULL;
	char lineDns[5][25];

	switch(messageType)
	{
		case MESSAGE_INCOMING:
			
			r.find(PARAM_TO, &to);
			r.find(PARAM_FROM, &from);
			r.find(PARAM_ORIGINALTO, &originalTo);
			r.find(PARAM_CALLID, &callId);
			r.find(PARAM_DEVICENAME, &deviceName);

			// Check for nulls of parameters
			if(to == NULL || from == NULL || originalTo == NULL || callId == NULL || deviceName == NULL)
			{
				// Unable to process this incoming call
			}
			else
			{
				icn(to, from, originalTo, callId, parent);
			}

			break;

		case MESSAGE_CALLACTIVE:
			
			r.find(PARAM_TO, &to);
			r.find(PARAM_CALLID, &callId);
			r.find(PARAM_DEVICENAME, &deviceName);

			// Check for nulls of parameters
			if(to == NULL || callId == NULL || deviceName == NULL)
			{
				// Unable to process this call active message
			}
			else
			{
				can(to, callId, parent);
			}

			break;

		case MESSAGE_CALLINACTIVE:
			
			r.find(PARAM_INUSE, &inUse);
			r.find(PARAM_CALLID, &callId);
			r.find(PARAM_DEVICENAME, &deviceName);

			// Check for nulls of parameters
			if(inUse == NULL || callId == NULL || deviceName == NULL)
			{
				// Unable to process this call inactive message
			}
			else
			{
				cin(*(int *)inUse, callId, parent);
			}

			break;

		case MESSAGE_INITIATE:
			
			r.find(PARAM_TO, &to);
			r.find(PARAM_FROM, &from);
			r.find(PARAM_CALLID, &callId);
			r.find(PARAM_DEVICENAME, &deviceName);

			// Check for nulls of parameters
			if(to == NULL || callId == NULL || deviceName == NULL || from == NULL)
			{
				// Unable to process this call active message
			}
			else
			{
				ican(callId, to, from, parent);
			}

			break;

		case MESSAGE_HANGUP:
			
			r.find(PARAM_CAUSE, &cause);
			r.find(PARAM_CALLID, &callId);
			r.find(PARAM_DEVICENAME, &deviceName);

			// Check for nulls of parameters
			if(cause == NULL || callId == NULL || deviceName == NULL)
			{
				// Unable to process this hangup message
			}
			else
			{
				hn(cause, callId, parent);
			}

			break;

		case MESSAGE_LOGIN_ACK:
			
			r.find(PARAM_LINEDN_COUNT, &lineDnCount);
			r.find(PARAM_DEVICENAME, &deviceName);

			int numLineDns = *(int *) lineDnCount;

			for(int i = 0; i < numLineDns; i++)
			{
				char* lineDn = NULL;
				r.find(PARAM_LINEDN_START + i, &lineDn);

				strcpy(lineDns[i], lineDn);
			}

			// Check for nulls of parameters
			if(deviceName == NULL || lineDnCount == NULL)
			{
				// Unable to process this login message
			}
			else
			{
				ln(deviceName, numLineDns, lineDns, parent);
			}

			break;

	}

	ReleaseMutex(hMutexReceive);
}

/////////////////////////////////////////////////////////////////////
// Session disconect handler
void CWFlatMapIpcClient::OnDisconnected()
{
	bConnected = FALSE;
	if (pClient)
	{
		pClient->Disconnect();
		delete pClient;
		pClient = NULL;
	}
}

/////////////////////////////////////////////////////////////////////
// Session connect handler
void CWFlatMapIpcClient::OnConnected()
{
    // noop
}

/////////////////////////////////////////////////////////////////////
// socket failure failure handler
void CWFlatMapIpcClient::OnFailure()
{
    // noop
}

/////////////////////////////////////////////////////////////////////
// Create a new flatmap IPC object
int createFlatmapIpcClient(void* userInterface)
{
	parent = userInterface;

	BOOL bRet = FALSE;

	if (pClient)
		return bRet;

	pClient = new CWFlatMapIpcClient();

	bRet = pClient != NULL ? TRUE : FALSE;

	return bRet;
}

/////////////////////////////////////////////////////////////////////
// Clean up IPC client object and resources 
void destroyFlatmapIpcClient()
{
	if (!pClient)
		return;

	if (bConnected)
	{
		pClient->Disconnect();
		bConnected = false;
	}

	delete pClient;
}

/////////////////////////////////////////////////////////////////////
// Is the connection object still valid?
int isFlatmapIpcClientAlive()
{
	return (pClient != NULL);
}

/////////////////////////////////////////////////////////////////////
// Connect to IPC server
int connectToFlatmapIpcServer(const char* pszServer, const unsigned int port)
{
	BOOL bRet = false;

    // If the client object is not available then create a new one
	if (!pClient)
		createFlatmapIpcClient(parent);

	if (bConnected)
		bRet = true;
	else
	{
		memset(&szFlatmapIpcServer, 0, sizeof(szFlatmapIpcServer));
		if (strlen(pszServer) > 0)
			strcpy(szFlatmapIpcServer, pszServer);
		else
			strcpy(szFlatmapIpcServer, "127.0.0.1");
		uFlatmapIpcPort = port;
		bRet = pClient->Connect(szFlatmapIpcServer, uFlatmapIpcPort);
	}

	bConnected = bRet;

	return bRet;
}

/////////////////////////////////////////////////////////////////////
// Disconnect from IPC server
void disconnectFromFlatmapIpcServer()
{
	if (!pClient || !bConnected)
		return;

	pClient->Disconnect();

	bConnected = false;
}

/////////////////////////////////////////////////////////////////////
// Utility function to return connection status
int isFlatmapIpcClientConnected()
{
	return bConnected;
}

/////////////////////////////////////////////////////////////////////
// Send Login Request
int sendLogin(const char* deviceName)
{
	BOOL bRet = FALSE;
	BOOL bNeedDisconnect = FALSE;
	FlatMapWriter map;

	if (hMutexSend == NULL)
	{
		hMutexSend = CreateMutex(NULL, FALSE, L"Flatmap Send");
	}

    // Gaining permission to send data
    DWORD dwWaitResult = WaitForSingleObject(hMutexSend, 30000L);

	/*if (dwWaitResult != WAIT_OBJECT_0)
		return bRet;*/

	if (!pClient || !bConnected)
	{
        // If the IPC link is broken then try to reconnect
		bRet = connectToFlatmapIpcServer(szFlatmapIpcServer, uFlatmapIpcPort);
		bConnected = bRet;
	}

	if (bConnected)
	{
		(&map)->insert((int)PARAM_DEVICENAME, FlatMap::STRING, (int)strlen(deviceName) + 1 , deviceName);
		bRet = pClient->Write(MESSAGE_LOGIN, map);
		bConnected = bRet;          // Well, somthing is wrong there, we may need to reconnect again
		bNeedDisconnect = TRUE;		// only reconnect when a connection was there from last try.
	}

	if (!bConnected)
	{
        // Try to disconnect then reconnect since the existing IPC client object is no longer valid
		if (pClient && bNeedDisconnect)
		{
			pClient->Disconnect();
			delete pClient;
			pClient = NULL;
		}

		// Second chance
		bRet = connectToFlatmapIpcServer(szFlatmapIpcServer, uFlatmapIpcPort);
		bConnected = bRet;

		if (bConnected)
		{
            // We do need a new writer object, otherwise, IPC client may fail to write 
	        FlatMapWriter mapx;
			(&mapx)->insert((int)PARAM_DEVICENAME, FlatMap::STRING, (int)strlen(deviceName) + 1 , deviceName);
			bRet = pClient->Write(MESSAGE_LOGIN, mapx);
		}
	}

	ReleaseMutex(hMutexSend);
	return bRet;
}

/////////////////////////////////////////////////////////////////////
// Send Logout Request
int sendLogout(const char* deviceName)
{
	BOOL bRet = FALSE;
	BOOL bNeedDisconnect = FALSE;
	FlatMapWriter map;

	if (hMutexSend == NULL)
	{
		hMutexSend = CreateMutex(NULL, FALSE, L"Flatmap Send");
	}

    // Gaining permission to send data
    DWORD dwWaitResult = WaitForSingleObject(hMutexSend, 30000L);

	/*if (dwWaitResult != WAIT_OBJECT_0)
		return bRet;*/

	if (!pClient || !bConnected)
	{
        // If the IPC link is broken then try to reconnect
		bRet = connectToFlatmapIpcServer(szFlatmapIpcServer, uFlatmapIpcPort);
		bConnected = bRet;
	}

	if (bConnected)
	{
		(&map)->insert((int)PARAM_DEVICENAME, FlatMap::STRING, (int)strlen(deviceName) + 1 , deviceName);
		bRet = pClient->Write(MESSAGE_LOGOUT, map);
		bConnected = bRet;          // Well, somthing is wrong there, we may need to reconnect again
		bNeedDisconnect = TRUE;		// only reconnect when a connection was there from last try.
	}

	if (!bConnected)
	{
        // Try to disconnect then reconnect since the existing IPC client object is no longer valid
		if (pClient && bNeedDisconnect)
		{
			pClient->Disconnect();
			delete pClient;
			pClient = NULL;
		}

		// Second chance
		bRet = connectToFlatmapIpcServer(szFlatmapIpcServer, uFlatmapIpcPort);
		bConnected = bRet;

		if (bConnected)
		{
            // We do need a new writer object, otherwise, IPC client may fail to write 
	        FlatMapWriter mapx;
			(&mapx)->insert((int)PARAM_DEVICENAME, FlatMap::STRING, (int)strlen(deviceName) + 1 , deviceName);
			bRet = pClient->Write(MESSAGE_LOGOUT, mapx);
		}
	}

	ReleaseMutex(hMutexSend);
	return bRet;
}

/////////////////////////////////////////////////////////////////////
// Send MakeCall Request
int sendMakeCall(const char* deviceName, const char* to)
{
	BOOL bRet = FALSE;
	BOOL bNeedDisconnect = FALSE;
	FlatMapWriter map;

	if (hMutexSend == NULL)
	{
		hMutexSend = CreateMutex(NULL, FALSE, L"Flatmap Send");
	}

    // Gaining permission to send data
    DWORD dwWaitResult = WaitForSingleObject(hMutexSend, 30000L);

	/*if (dwWaitResult != WAIT_OBJECT_0)
		return bRet;*/

	if (!pClient || !bConnected)
	{
        // If the IPC link is broken then try to reconnect
		bRet = connectToFlatmapIpcServer(szFlatmapIpcServer, uFlatmapIpcPort);
		bConnected = bRet;
	}

	if (bConnected)
	{
		(&map)->insert((int)PARAM_DEVICENAME, FlatMap::STRING, (int)strlen(deviceName) + 1, deviceName);
		(&map)->insert((int)PARAM_TO, FlatMap::STRING, (int)strlen(to) + 1, to);
		bRet = pClient->Write(MESSAGE_MAKECALL, map);
		bConnected = bRet;          // Well, somthing is wrong there, we may need to reconnect again
		bNeedDisconnect = TRUE;		// only reconnect when a connection was there from last try.
	}

	if (!bConnected)
	{
        // Try to disconnect then reconnect since the existing IPC client object is no longer valid
		if (pClient && bNeedDisconnect)
		{
			pClient->Disconnect();
			delete pClient;
			pClient = NULL;
		}

		// Second chance
		bRet = connectToFlatmapIpcServer(szFlatmapIpcServer, uFlatmapIpcPort);
		bConnected = bRet;

		if (bConnected)
		{
            // We do need a new writer object, otherwise, IPC client may fail to write 
	        FlatMapWriter mapx;
			(&mapx)->insert((int)PARAM_DEVICENAME, FlatMap::STRING, (int)strlen(deviceName) + 1, deviceName);
			(&mapx)->insert((int)PARAM_TO, FlatMap::STRING, (int)strlen(to) + 1, to);
			bRet = pClient->Write(MESSAGE_MAKECALL, mapx);
		}
	}

	ReleaseMutex(hMutexSend);
	return bRet;
}

/////////////////////////////////////////////////////////////////////
// Send MakeCall Request
int sendAnswerCall(const char* deviceName)
{
	BOOL bRet = FALSE;
	BOOL bNeedDisconnect = FALSE;
	FlatMapWriter map;

	if (hMutexSend == NULL)
	{
		hMutexSend = CreateMutex(NULL, FALSE, L"Flatmap Send");
	}

    // Gaining permission to send data
    DWORD dwWaitResult = WaitForSingleObject(hMutexSend, 30000L);

	/*if (dwWaitResult != WAIT_OBJECT_0)
		return bRet;*/

	if (!pClient || !bConnected)
	{
        // If the IPC link is broken then try to reconnect
		bRet = connectToFlatmapIpcServer(szFlatmapIpcServer, uFlatmapIpcPort);
		bConnected = bRet;
	}

	if (bConnected)
	{
		(&map)->insert((int)PARAM_DEVICENAME, FlatMap::STRING, (int)strlen(deviceName) + 1 , deviceName);
		bRet = pClient->Write(MESSAGE_MAKECALL, map);
		bConnected = bRet;          // Well, somthing is wrong there, we may need to reconnect again
		bNeedDisconnect = TRUE;		// only reconnect when a connection was there from last try.
	}

	if (!bConnected)
	{
        // Try to disconnect then reconnect since the existing IPC client object is no longer valid
		if (pClient && bNeedDisconnect)
		{
			pClient->Disconnect();
			delete pClient;
			pClient = NULL;
		}

		// Second chance
		bRet = connectToFlatmapIpcServer(szFlatmapIpcServer, uFlatmapIpcPort);
		bConnected = bRet;

		if (bConnected)
		{
            // We do need a new writer object, otherwise, IPC client may fail to write 
	        FlatMapWriter mapx;
			(&mapx)->insert((int)PARAM_DEVICENAME, FlatMap::STRING, (int)strlen(deviceName) + 1 , deviceName);
			bRet = pClient->Write(MESSAGE_MAKECALL, mapx);
		}
	}

	ReleaseMutex(hMutexSend);
	return bRet;
}

/////////////////////////////////////////////////////////////////////
// Send Hangup Request
int sendHangup(const char* deviceName, const char* callId)
{
	BOOL bRet = FALSE;
	BOOL bNeedDisconnect = FALSE;
	FlatMapWriter map;

	if (hMutexSend == NULL)
	{
		hMutexSend = CreateMutex(NULL, FALSE, L"Flatmap Send");
	}

    // Gaining permission to send data
    DWORD dwWaitResult = WaitForSingleObject(hMutexSend, 30000L);

	/*if (dwWaitResult != WAIT_OBJECT_0)
		return bRet;*/

	if (!pClient || !bConnected)
	{
        // If the IPC link is broken then try to reconnect
		bRet = connectToFlatmapIpcServer(szFlatmapIpcServer, uFlatmapIpcPort);
		bConnected = bRet;
	}

	if (bConnected)
	{
		(&map)->insert((int)PARAM_DEVICENAME, FlatMap::STRING, (int)strlen(deviceName) + 1 , deviceName);
		(&map)->insert((int)PARAM_CALLID, FlatMap::STRING, (int)strlen(callId) + 1 , callId);
		bRet = pClient->Write(MESSAGE_HANGUP, map);
		bConnected = bRet;          // Well, somthing is wrong there, we may need to reconnect again
		bNeedDisconnect = TRUE;		// only reconnect when a connection was there from last try.
	}

	if (!bConnected)
	{
        // Try to disconnect then reconnect since the existing IPC client object is no longer valid
		if (pClient && bNeedDisconnect)
		{
			pClient->Disconnect();
			delete pClient;
			pClient = NULL;
		}

		// Second chance
		bRet = connectToFlatmapIpcServer(szFlatmapIpcServer, uFlatmapIpcPort);
		bConnected = bRet;

		if (bConnected)
		{
            // We do need a new writer object, otherwise, IPC client may fail to write 
	        FlatMapWriter mapx;
			(&mapx)->insert((int)PARAM_DEVICENAME, FlatMap::STRING, (int)strlen(deviceName) + 1 , deviceName);
			(&mapx)->insert((int)PARAM_CALLID, FlatMap::STRING, (int)strlen(callId) + 1 , callId);
			bRet = pClient->Write(MESSAGE_HANGUP, mapx);
		}
	}

	ReleaseMutex(hMutexSend);
	return bRet;
}