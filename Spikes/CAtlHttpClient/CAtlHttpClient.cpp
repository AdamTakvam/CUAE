// CAtlHttpClient.cpp : Defines the entry point for the console application.
//

#include "stdafx.h"

using namespace ATL;

bool __stdcall SendStatusCallback(DWORD a, DWORD b)
{
    std::cout << "s";
    return true;
}

bool __stdcall ReceiveStatusCallback(DWORD a, DWORD b)
{
    std::cout << "r";
    return true;
}

void Worker(void* url)
{
    unsigned char postBody[] = "Body body body body body";
    char contentType[] = "text/xml";

    CAtlNavigateData navData;
    CAtlHttpClient httpClient;

    navData.SetSendStatusCallback((PFNATLSTATUSCALLBACK)SendStatusCallback, 0);
    navData.SetReadStatusCallback((PFNATLSTATUSCALLBACK)ReceiveStatusCallback, 0);

    navData.SetMethod(ATL_HTTP_METHOD_POST);
    navData.SetPostData(postBody, sizeof(postBody), contentType);

    httpClient.Navigate((char*)url, &navData);

    char* responseBody;
    long responseBodyLen = httpClient.GetResponseLength();

    responseBody = new char[responseBodyLen + 1];
    memcpy(responseBody, httpClient.GetResponse(), responseBodyLen);
    responseBody[responseBodyLen] = 0;

    int statusCode = httpClient.GetStatus();
    std::cout << std::endl;
    std::cout << "Status Code: " << statusCode << std::endl;
 
    std::cout << "Body Length: " << responseBodyLen << std::endl;
   // std::cout << "Body:" << std::endl << responseBody << std::endl;

    httpClient.Close();
}

int _tmain(int argc, _TCHAR* argv[])
{
    if(argc != 2)
    {
        std::cout << "Usage: CAtlHttpClient <URL>" << std::endl;
        return 0;
    }
    
    HANDLE hThread = (HANDLE) _beginthread(Worker, 0, argv[1]);

    WaitForSingleObject(hThread, 5000);
}