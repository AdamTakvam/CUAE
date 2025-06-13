#include "stdafx.h"

#include "Flatmap.h"

#include "TestIpcClient.h"

using namespace Metreos;
using namespace Metreos::IPC;

int main(int argc, char* argv[])
{    
    IpcClient client;

    if(client.Connect("127.0.0.1", 3434) != 1)
        return 0;
    
    char msg[] = "test test test";
    client.Write(msg, sizeof(msg));

    ACE_OS::sleep(1);

    client.Disconnect();

	return 0;
}

