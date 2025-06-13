#include "stdafx.h"
#include "Flatmap.h"
#include "TestIpcServer.h"

using namespace Metreos;
using namespace Metreos::IPC;

TestIpcServer::TestIpcServer(int port) :
    IpcServer(port),
    mutex(),
    waiter(mutex)
{
} 

TestIpcServer::~TestIpcServer()
{
}

void TestIpcServer::OnIncomingData(const char* data, int id)
{
    std::cout << "Received: " << data << std::endl;

    char msg[] = "response response";
    Write(msg, sizeof(msg), id);
}

int main(int argc, char* argv[])
{
    std::cout << "Press 'q' to quit" << std::endl;

    TestIpcServer server(3434);
    server.Start();

    char c;
    std::cin >> c;

    server.Stop();
    
	return 0;
}

