// udpsvr.cpp : Defines the entry point for the console application.
//

#include "stdafx.h"

#define WIN32_LEAN_AND_MEAN
#include <winsock2.h>
#include <stdlib.h>
#include <stdio.h>
#include <string.h>

#define DEFAULT_PORT 8080
#define DEFAULT_PROTO SOCK_DGRAM

int main(int argc, char **argv) 
{
    char Buffer[1024];
    int retval;
    int fromlen;
    int socket_type = DEFAULT_PROTO;
    unsigned short port = DEFAULT_PORT;
    struct sockaddr_in local, from;
    WSADATA wsaData;
    SOCKET listen_socket, msgsock;

    if ((retval = WSAStartup(0x202, &wsaData)) != 0) 
	{
        fprintf(stderr, "WSAStartup failed with error %d\n", retval);
        WSACleanup();
        return -1;
    }
    
    local.sin_family = AF_INET;
    local.sin_addr.s_addr = htonl(INADDR_ANY);

    local.sin_port = htons(port);

    listen_socket = socket(AF_INET, socket_type, 0); 
    
    if (listen_socket == INVALID_SOCKET)
	{
        fprintf(stderr,"socket() failed with error %d\n",WSAGetLastError());
        WSACleanup();
        return -1;
    }

	local.sin_family = AF_INET;
	local.sin_addr.s_addr = htonl(INADDR_ANY);
	local.sin_port = htons(DEFAULT_PORT);
    if (bind(listen_socket, (struct sockaddr*)&local, sizeof(local)) == SOCKET_ERROR) 
	{
        fprintf(stderr,"bind() failed with error %d\n",WSAGetLastError());
        WSACleanup();
        return -1;
    }

    printf("Listening on port %d\n", port);

    while(1) 
	{
        fromlen = sizeof(from);

        msgsock = listen_socket;

		memset(&Buffer, 0, sizeof(Buffer));

        retval = recvfrom(msgsock, Buffer, sizeof(Buffer), 0, (struct sockaddr *)&from, &fromlen);
            
        if (retval == SOCKET_ERROR) 
		{
            fprintf(stderr, "recv() failed: error %d\n", WSAGetLastError());
            closesocket(msgsock);
			Sleep(1000);
            continue;
        }

        printf("Received %d bytes, data [%s] from %s\n", retval, Buffer, inet_ntoa(from.sin_addr));
        continue;
    }
}