// pcap-client.cpp : Defines the entry point for the console application.
//

#include "stdafx.h"
#include "Flatmap.h"
#include "ipc/FlatmapIpcClient.h"
#include "pcap-client.h"
#include "ace/Get_Opt.h"

using namespace Metreos;
using namespace Metreos::IPC;
using namespace Metreos::LogClient;

static int portbase = 23000;
static int portmax = 24000;
static int curport = portbase;
static int transId = 0;
static char pcapIpAddr[16];
static char destIpAddr[16];
static int requestInterval = 300;        // 300 secs = 5 mins
static int monitorAttempt = 0;
static int monitorSuccess = 0;

static u_int lastMonitoredCallId = 0;

PCapClient::PCapClient()
{
    bConnected = false;
    logger = LogServerClient::Instance();
}

static int GetNextTransId()
{
    transId++;
    return transId;
}

static int GetNextPort()
{
    curport+=2;
    if (curport >= portmax)
        curport = portbase;

    return curport;
}


void PCapClient::OnIncomingFlatMapMessage(const int messageType, const FlatMapReader& flatmap)
{
	FlatMapReader reader = flatmap;

    switch(messageType)
    {
        case 100:       // Active Call List
            {
            char* pNumEntries = NULL;
            int numEntries = 0;
            reader.find(1001, &pNumEntries);                // num entries
            if (pNumEntries)
                numEntries = *((int*)pNumEntries);
            logger->WriteLog(Log_Info, "Number of Active Calls = %d", numEntries);

            int nMyPick = 0;
            if (numEntries > 0)
            {
                nMyPick = (int)(numEntries * (rand() / ((double)RAND_MAX + 1)));
                nMyPick++;
                logger->WriteLog(Log_Info, "\tPick Call [%d] to monitor", nMyPick);
            }
            else
            {
                lastMonitoredCallId = 0;
            }

            for (int i=1; i<=numEntries; i++)
            {
                char* pCallData = NULL;
                u_int callIdentifier = 0;
                reader.find(1011, &pCallData, 0, i);                // identifier
                if (pCallData)
                {
                    char* p = strtok(pCallData, "&");
                    if (p)
                        callIdentifier = atoi(pCallData);
                    logger->WriteLog(Log_Info, "\tCall[%d] Identifier = %d", i, callIdentifier);

                    if (i == nMyPick)
                    {
                        // Send out monitor call request
                        FlatMapWriter map;
                        map.insert(1012, (int)GetNextTransId());
                        map.insert(1002, (int)callIdentifier);
                        map.insert(1003, 3, 11, destIpAddr); 
                        int dport1 = GetNextPort();
                        int dport2 = GetNextPort();
                        map.insert(1004, (int)dport1);
                        map.insert(1004, (int)dport2);
                        Write(103, map);     // Monitor Call
                        monitorAttempt++;
                        logger->WriteLog(Log_Info, "Send out monitor call request [%d]\n\tIdentifier = %d\n\tDestination=%s:%d:%d", 
                                        monitorAttempt, callIdentifier, destIpAddr, dport1, dport2);
                    }
                }
            }
            }
            break;

        case 104:       // Monitor Call Ack
            {
            char* pResultCode = NULL;
            int rc = 1;
            reader.find(1000, &pResultCode);                // result code
            if (pResultCode)
                rc = *((int*)pResultCode);            

            char* pIdentifier = NULL;
            u_int callIdentifier = 0;
            reader.find(1002, &pIdentifier);                // identifier
            if (pIdentifier)
            {
                callIdentifier = *((u_int*)pIdentifier);

                if (rc == 0)
                {
                    lastMonitoredCallId = callIdentifier;
                    monitorSuccess++;
                }
                logger->WriteLog(Log_Info, "Monitor request [%d] for Call Identifier = %d returns %d", monitorSuccess, callIdentifier, rc);
            }
            }
            break;

        default:
            break;
    }


}


void PCapClient::OnConnected()
{
    logger->WriteLog(Log_Info, "Connected to pcap-service.");
    bConnected = true;
}


void PCapClient::OnDisconnected()
{
    logger->WriteLog(Log_Info, "Disconnected from pcap-service.");
    bConnected = false;
}

void PCapClient::OnFailure()
{
    logger->WriteLog(Log_Error, "IPC failure.");
}

int main(int argc, char* argv[])
{    
    memset(&destIpAddr, 0, sizeof(destIpAddr));
    memset(&pcapIpAddr, 0, sizeof(pcapIpAddr));
    strcpy(destIpAddr, "127.0.0.1");
    strcpy(pcapIpAddr, "127.0.0.1");

    ACE_Get_Opt getOpt(argc, argv, "c:i:t:p:q:");

    char c;
    char* p = 0;
    while((c = getOpt()) != EOF)
    {
        switch(c)
        {
            case 'c':                                   // IPC server IP
                strcpy(pcapIpAddr, getOpt.optarg);
                break;

            case 'i':                                   // RTP detination IP
                strcpy(destIpAddr, getOpt.optarg);
                break;

            case 't':                                   // Request time interval
                p = getOpt.optarg;
                if (p != NULL)
                    requestInterval = atoi(p);
                break;

            case 'p':                                   // portbase LWM
                p = getOpt.optarg;
                if (p != NULL)
                {
                    portbase = atoi(p);
                    curport = portbase;
                }
                break;

            case 'q':                                   // portbae HWM
                p = getOpt.optarg;
                if (p != NULL)
                    portmax = atoi(p);
                break;
        }
    }

    PCapClient client;

	client.logger->LogToConsole(true);

	client.logger->open("PCapClient");

	client.logger->activate();

	ACE_OS::sleep(2);

    while (1)
    {
        if (!client.bConnected)
        {
            client.bConnected = client.Connect(pcapIpAddr, 8510);
            if (client.bConnected)
            {
                FlatMapWriter map;
                map.insert(1012, (int)GetNextTransId());        // trans id
                client.Write(100, map);                         // Active Call List
            }
        }
        else
        {
            FlatMapWriter map;
            map.insert(1012, (int)GetNextTransId());            // trans id
            client.Write(100, map);                             // Active Call List
        }
        ACE_OS::sleep(requestInterval);

        // Stop last monitored call
        if (lastMonitoredCallId != 0)
        {
            FlatMapWriter map;
            map.insert(1002, (int)lastMonitoredCallId);         // call identifier
            client.Write(105, map);                             // Stop monitor call
            client.logger->WriteLog(Log_Info, "Stop monitor call request for Call Identifier = %d", lastMonitoredCallId);
            // Rest a while, 5 secs
            ACE_OS::sleep(5);
        }
    }

    client.Disconnect();

	client.logger->msg_queue()->deactivate();
    client.logger->close();

	return 0;
}
