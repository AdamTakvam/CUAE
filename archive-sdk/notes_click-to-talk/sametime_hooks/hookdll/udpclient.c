// udpclient.c

#include <winsock2.h>
#include "udpclient.h"

/////////////////////////////////////////////////////////////////////
static WSADATA wsaData;
static SOCKET conn_socket;
#define SECURITY_CODE "METREOS"

/////////////////////////////////////////////////////////////////////
BOOL StartClient()
{
	int retval = 0;

    if ((retval = WSAStartup(0x202, &wsaData)) != 0) 
	{
        WSACleanup();
        return FALSE;
    }

	return TRUE;
}

/////////////////////////////////////////////////////////////////////
BOOL StopClient()
{
    int retval = WSACleanup();

	return (retval == 0 ? TRUE : FALSE);
}

/////////////////////////////////////////////////////////////////////
BOOL ConnectToServer(char* pszServer, int iport)
{
	int socket_type = SOCK_DGRAM;
	struct sockaddr_in server;
	struct hostent *hp;
	unsigned int addr;

    // Attempt to detect if we should call gethostbyname() or
    // gethostbyaddr()
	
	char server_name[256];
	memset(&server_name, 0, sizeof(server_name));
	lstrcpy(server_name, pszServer);

    if (isalpha(server_name[0])) 
	{   
		// server address is a name
        hp = gethostbyname(server_name);
    }
    else  
	{ 
		// Convert nnn.nnn address to a usable one
        addr = inet_addr(server_name);
        hp = gethostbyaddr((char *)&addr, 4, AF_INET);
    }
    if (hp == NULL ) 
	{
        WSACleanup();
		// TODO: Notify error
        return FALSE;
    }

    // Copy the resolved information into the sockaddr_in structure
    memset(&server, 0, sizeof(server));
    memcpy(&(server.sin_addr), hp->h_addr, hp->h_length);
    server.sin_family = hp->h_addrtype;
    server.sin_port = htons(iport);

	// Open a socket
    conn_socket = socket(AF_INET, socket_type, 0);
    if (conn_socket <0 ) 
	{
        WSACleanup();
		// TODO: Notify error
        return FALSE;
    }

    // Notice that nothing in this code is specific to whether we 
    // are using UDP or TCP.
    // We achieve this by using a simple trick.
    //    When connect() is called on a datagram socket, it does not 
    //    actually establish the connection as a stream (TCP) socket
    //    would. Instead, TCP/IP establishes the remote half of the
    //    ( LocalIPAddress, LocalPort, RemoteIP, RemotePort) mapping.
    //    This enables us to use send() and recv() on datagram sockets,
    //    instead of recvfrom() and sendto()
    if (connect(conn_socket, (struct sockaddr*)&server, sizeof(server)) == SOCKET_ERROR) 
	{
        WSACleanup();
		// TODO: Notify error
        return FALSE;
    }

	return TRUE;
}

/////////////////////////////////////////////////////////////////////
BOOL DisconnectFromServer()
{
	int retval = closesocket(conn_socket);

	return (retval == 0 ? TRUE : FALSE);
}

/////////////////////////////////////////////////////////////////////
BOOL SendToServer()
{
	char szData[256];
	memset(&szData, 0, sizeof(szData));
	wsprintf(szData, "%s", SECURITY_CODE);

    if (SOCKET_ERROR == send(conn_socket, szData, lstrlen(szData), 0))
		return FALSE;

	return TRUE;
}
