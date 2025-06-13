// FlatmapIpcClientWrapper.cpp

#include "FlatmapIpcClientWrapper.h"
#include "IPC/FlatmapIpcclient.h"

using namespace Metreos;
using namespace Metreos::IPC;

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
static bool populateFlatmapWriter(FlatMapWriter* writer, const flatmap_data* flatmap);

/////////////////////////////////////////////////////////////////////
static bool populateFlatmapData(flatmap_data* flatmap, const FlatMapReader& reader);

/////////////////////////////////////////////////////////////////////
static CWFlatMapIpcClient* pClient = NULL;          // The one and only flatmap client
static BOOL bConnected = FALSE;                     // Connection status flag
static char szFlatmapIpcServer[32] = {0};           // IPC server IP address
static unsigned int uFlatmapIpcPort = 9434;         // Default IPC port
static HANDLE hMutexReceive = NULL;                 // Receive mutex object
static HANDLE hMutexSend = NULL;                    // Send mutex object
MessageNotifier mn = NULL;                          // Call back function pointer

/////////////////////////////////////////////////////////////////////
// Incoming flatmap data handler
void CWFlatMapIpcClient::OnIncomingFlatMapMessage(const int messageType, const FlatMapReader& flatmap)
{
    // If there is no callback function, stops here 
	if (!mn)
		return;

	if (hMutexReceive == NULL)
	{
		hMutexReceive = CreateMutex(NULL, FALSE, "Flatmap Calback");
	}

    // Gaining the permission to read data
    DWORD dwWaitResult = WaitForSingleObject(hMutexReceive, 30000L);

	if (dwWaitResult != WAIT_OBJECT_0)
		return;

	FlatMapReader r = flatmap;
	char* pUuid = NULL;
	int key = 100;			// HTTP_UUID
	int type = 3;			// FLATMAP_STRING
	int len = r.find(key, &pUuid, &type, 0);

	flatmap_data fd;
	memset(&fd, 0, sizeof(flatmap_data));
	fd.uuid_len = len;
	memcpy(&fd.uuid, pUuid, len);
	populateFlatmapData(&fd, flatmap);
	mn(fd);

	if (fd.data)
		delete []fd.data;

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
int createFlatmapIpcClient()
{
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
		createFlatmapIpcClient();

	if (bConnected)
		bRet = true;
	else
	{
		memset(&szFlatmapIpcServer, 0, sizeof(szFlatmapIpcServer));
		if (lstrlen(pszServer) > 0)
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
// Send HTTP request data to HTTP provider on Application server side
int sendReguestMessage(const flatmap_data* flatmap)
{
	BOOL bRet = FALSE;
	BOOL bNeedDisconnect = FALSE;
	FlatMapWriter map;

	if (hMutexSend == NULL)
	{
		hMutexSend = CreateMutex(NULL, FALSE, "Flatmap Send");
	}

    // Gaining permission to send data
    DWORD dwWaitResult = WaitForSingleObject(hMutexSend, 30000L);

	if (dwWaitResult != WAIT_OBJECT_0)
		return bRet;

	if (!pClient || !bConnected)
	{
        // If the IPC link is broken then try to reconnect
		bRet = connectToFlatmapIpcServer(szFlatmapIpcServer, uFlatmapIpcPort);
		bConnected = bRet;
	}

	if (bConnected)
	{
        // If the connection is there, try to send data
		if (populateFlatmapWriter(&map, flatmap))
		{
			bRet = pClient->Write(MESSAGE_APACHE_MODULE, map);
			bConnected = bRet;          // Well, somthing is wrong there, we may need to reconnect again
			bNeedDisconnect = TRUE;		// only reconnect when a connection was there from last try.
		}
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
			if (populateFlatmapWriter(&mapx, flatmap))
			{
				bRet = pClient->Write(MESSAGE_APACHE_MODULE, mapx);
			}
		}
	}

	ReleaseMutex(hMutexSend);
	return bRet;
}

/////////////////////////////////////////////////////////////////////
// Assign callback function pointer 
void assignMessageNotifier(MessageNotifier pFunc)
{
	if (mn != NULL)
		return;

	mn = pFunc;
}

/////////////////////////////////////////////////////////////////////
// Populate outgoing data from internal data structure shared by this wrapper and Apache module
// to flatmap writer
static bool populateFlatmapWriter(FlatMapWriter* writer, const flatmap_data* flatmap)
{
	char* p = flatmap->data;
	writer->insert((int)HTTP_UUID, FlatMap::STRING, flatmap->uuid_len, flatmap->uuid);

	for (int i=0; i<flatmap->count; i++)
	{
		flatmap_data_header header;
		memcpy(&header, p, sizeof(flatmap_data_header));
		p += sizeof(flatmap_data_header);
		switch(header.flatmap_data_type)
		{
			case FLATMAP_INT:
				{
				int value;
				memcpy(&value, p, sizeof(int));
				writer->insert(header.http_data_type, value);
				p += sizeof(int);
				}
				break;

			case FLATMAP_BYTE:
				writer->insert(header.http_data_type, header.flatmap_data_type, header.data_size, p);
				p += header.data_size;
				break;

			case FLATMAP_STRING:
				{
				writer->insert(header.http_data_type, header.flatmap_data_type, header.data_size, p);
				p += header.data_size;
				}
				break;

			case FLATMAP_FLATMAP:
				writer->insert(header.http_data_type, header.flatmap_data_type, header.data_size, p);
				p += header.data_size;
				break;

			case FLATMAP_LONG:
				{
				long value;
				memcpy(&value, p, sizeof(long));
				writer->insert(header.http_data_type, value);
				p += sizeof(long);
				}
				break;

			case FLATMAP_DOUBLE:
				{
				double value;
				memcpy(&value, p, sizeof(double));
				writer->insert(header.http_data_type, value);
				p += sizeof(double);
				}
				break;
		}
	}
			
	return true;	
}

/////////////////////////////////////////////////////////////////////
// Populate incoming flatmap data to the internal data structure shared by this wrapper and Apache module 
static bool populateFlatmapData(flatmap_data* flatmap, const FlatMapReader& reader)
{
	char* pData = NULL;
	int key = 0;
	int type = 0;
	int len = 0;
	int total_len = 0;

	FlatMapReader r = reader;

	flatmap->count = r.size();

	for (int i=0; i<flatmap->count; i++)
	{
		len = r.get(i, &key, &pData, &type);
		total_len += (sizeof(flatmap_data_header) + len);
	}

    // This piece is going to freed after data being passed to Apache module 
	flatmap->data = new char[total_len];

	char* p = flatmap->data;

	for (i=0; i<flatmap->count; i++)
	{
		len = r.get(i, &key, &pData, &type);
		
		flatmap_data_header header;
		header.data_size = len;
		header.flatmap_data_type = type;
		header.http_data_type = key;

		memcpy(p, &header, sizeof(flatmap_data_header));
		p += sizeof(flatmap_data_header);

		memcpy(p, pData, len);
		p += len;
	}

	return true;
}
