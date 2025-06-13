#ifndef PCAP_CLIENT_H
#define PCAP_CLIENT_H

#include "ipc/FlatmapIpcClient.h"
#include "logclient/logclient.h"

using namespace Metreos;
using namespace Metreos::IPC;
using namespace Metreos::LogClient;

class PCapClient : public Metreos::IPC::FlatMapIpcClient
{
public:
    PCapClient();

    virtual void OnIncomingFlatMapMessage(const int messageType, const FlatMapReader& flatmap);
    virtual void OnConnected();
    virtual void OnDisconnected();
	virtual void OnFailure();

    bool bConnected;
    LogServerClient* logger;
};

#endif // PCAP_CLIENT_H