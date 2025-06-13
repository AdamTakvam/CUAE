#include "stdafx.h"
#include "TestFlatmapIpcClient.h"
#include "Flatmap.h"
#include "ipc/FlatmapIpcClient.h"

using namespace Metreos;
using namespace Metreos::IPC;

static bool bConnected = false;

TestFlatMapIpcClient::TestFlatMapIpcClient()
{
}

void TestFlatMapIpcClient::OnIncomingFlatMapMessage(const int messageType, const FlatMapReader& flatmap)
{
    std::cout << "Flatmap message: " << messageType << std::endl;

	FlatMapReader r;
	
	r = flatmap;

	
	char* pData = NULL;
	int type = 3;
	int len = r.find(100, &pData, &type, 0);

    FlatMapWriter map;
	map.insert(100, FlatMap::STRING, len, pData);

    ACE_OS::sleep(1);

    Write(50, map);
}


void TestFlatMapIpcClient::OnConnected()
{
    std::cout << "Flatmap client session is connected!" << std::endl;
    bConnected = true;
}


void TestFlatMapIpcClient::OnDisconnected()
{
    std::cout << "Flatmap client session has been disconnected!" << std::endl;
    bConnected = false;
}

void TestFlatMapIpcClient::OnFailure()
{
    std::cout << "Flatmap client failure!" << std::endl;
}

int main(int argc, char* argv[])
{    
    TestFlatMapIpcClient client;

    while (1)
    {
        if (!bConnected)
        {
            bConnected = client.Connect("127.0.0.1", 3434);
            if (bConnected)
            {
                FlatMapWriter map;
                map.insert(100, FlatMap::STRING, 5, "Hello");
                client.Write(50, map);
            }
        }
        ACE_OS::sleep(1);
    }

    client.Disconnect();

	return 0;
}

