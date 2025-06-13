#ifndef UDPCLIENT_H
#define UDPCLIENT_H


BOOL StartClient();

BOOL StopClient();

BOOL ConnectToServer(char* pszServer, int iPort);

BOOL DisconnectFromServer();

BOOL SendToServer();

#endif
