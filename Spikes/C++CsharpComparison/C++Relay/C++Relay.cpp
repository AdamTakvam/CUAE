// This is the main project file for VC++ application project 
// generated using an Application Wizard.

#include "stdafx.h"
#include "winsock2.h"
#include "process.h"

#using <mscorlib.dll>

static SOCKET socketOut;
static SOCKET socketIn;
static sockaddr_in addressOut;
static sockaddr_in addressIn;

static HANDLE thread;

static DWORD WINAPI RelayData(void *dummy)
{
	if (connect(socketOut, (SOCKADDR *)&addressOut, sizeof addressOut) == SOCKET_ERROR)
	{
		puts("Failed to connect.");
	}
	else
	{
		if (bind(socketIn, (SOCKADDR *)&addressIn, sizeof addressIn) == SOCKET_ERROR)
		{
			puts("bind() failed.");
		}
		else
		{
			static char buffer[1024];
			while (true)
			{
				int bytesRead = recv(socketIn, buffer, sizeof buffer, 0);
				if (bytesRead > 0)
				{
//					static char integer[128];
//					puts(itoa(*((int *)&buffer), integer, 10));
					send(socketOut, buffer, bytesRead, 0);
				}
				else
				{
					puts("recv error");
				}
			}
		}
	}

	closesocket(socketOut);
	closesocket(socketIn);
	WSACleanup();

	return 0;
}

int main(int argc, char *argv[])
{
	if (argc < 4)
	{
		printf("Invalid command-line arguments (%d)", argc);
	}
	else
	{
		addressOut.sin_family = AF_INET;
		addressOut.sin_addr.s_addr = inet_addr(argv[2]);
		int portOut = atoi(argv[3]);
		addressOut.sin_port = htons(portOut);

		addressIn.sin_family = AF_INET;
		addressIn.sin_addr.s_addr = inet_addr("0.0.0.0");
		int portIn = atoi(argv[1]);
		addressIn.sin_port = htons(portIn);

		static WSADATA wsaData;
		int iResult = WSAStartup(MAKEWORD(2,2), &wsaData);
		if (iResult != NO_ERROR)
		{
			puts("Error at WSAStartup()");
		}
		else
		{
			socketOut = socket(AF_INET, SOCK_DGRAM, IPPROTO_UDP);
			socketIn = socket(AF_INET, SOCK_DGRAM, IPPROTO_UDP);

			DWORD threadId;
			thread = CreateThread(NULL, 0, RelayData, NULL, 0, &threadId);
			WaitForSingleObject(thread, INFINITE);
			CloseHandle(thread);
		}
	}

	return 0;
}
