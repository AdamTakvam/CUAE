// PCapRuntime.cpp

#include "stdafx.h"

#ifdef WIN32
#ifdef PCAP_MEM_LEAK_DETECTION
#   define DEBUG_NEW new(_NORMAL_BLOCK, __FILE__, __LINE__)
#   define new DEBUG_NEW 
#endif
#endif

#include "PCapCommon.h"
#include "PCapRuntime.h"
#include "msgs/PCapMessageTypes.h"

#include "pcap.h"

#include "PCapPacket.h"

#include "Flatmap.h"

#include <fstream>
#include "g711.h"
//#include "wave.h"

using namespace Metreos;
using namespace Metreos::PCap;


PCapRuntime::PCapRuntime() :
    runtimeStartedMutex(),
    runtimeStoppedMutex(),
    m_LivePhonesMutex(),
    m_MonitoredCallsMutex(),
    m_MonitoredRtpStreamsMutex(),
    runtimeStarted(runtimeStartedMutex),
    runtimeStopped(runtimeStoppedMutex),
    ipcServer(this),
    bShuttingDown(false),
    portbaseLWM(DEFAULT_PORTBASE_LWM),
    portbaseHWM(DEFAULT_PORTBASE_HWM),
    portStep(DEFAULT_PORT_STEP),
    portbase(DEFAULT_PORTBASE_LWM),
    maxMonitorTime(DEFAULT_MAX_MONITOR_TIME),
    logLevel(Log_Info)
{
    params.activeMode = false;
    params.sendRTPToFile = false;
    params.sendRTPToIp = false;
    logger->SetLogLevel((LogClient::LogLevel)logLevel);
}

void PCapRuntime::WriteToIpc(const int messageType, FlatMapWriter& flatmap)
{
    this->ipcServer.Write(messageType, flatmap, m_sessionId);
}

int PCapRuntime::Startup()
{
    PCapMessage* pMsg = new PCapMessage;
    pMsg->type(Msgs::START);
    
    ACE_Time_Value timeout = ACE_OS::gettimeofday();
    timeout += 5; 

    this->runtimeStartedMutex.acquire();

    this->AddMessage(pMsg);

    int retValue = this->runtimeStarted.wait(&timeout);
    this->runtimeStartedMutex.release();

    return retValue;
}

int PCapRuntime::Shutdown()
{
    PCapMessage* pMsg = new PCapMessage;
    pMsg->type(Msgs::STOP);
    this->AddMessage(pMsg);
    
    ACE_Time_Value timeout = ACE_OS::gettimeofday();
    timeout += 5; 

    this->runtimeStoppedMutex.acquire();    
    int retValue = this->runtimeStopped.wait(&timeout);
    this->runtimeStoppedMutex.release();
    
    return retValue;
}

int PCapRuntime::svc(void)
{
    ACE_Message_Block* msg;
    while(!isShuttingDown() && getq(msg) != -1)
    {
        STAT_BEGIN(runtimeSvcExcTime);

        ACE_ASSERT(msg != 0);

        PCapMessage* pCapMsg = static_cast<PCapMessage*>(msg);
        ACE_ASSERT(pCapMsg != 0);

        switch(pCapMsg->type())
        {
            case Msgs::START:                       // Start runtime
                OnStart(*pCapMsg);
                break;

            case Msgs::STOP:                        // Stop runtime
            case Msgs::IPC_FAILED_START:            // IPC fail to start
            case Msgs::PCAP_FAILED_START:           // PCAP service failed to start
            case Msgs::PATROL_FAILED_START:         // Partol thread failed to start
                OnStop(*pCapMsg);
                break;

            case Msgs::SKINNY_CALL_DATA:            // All Skinny messages
                OnSkinnyCallData(*pCapMsg);
                break;

            case Msgs::RTP_PAYLOAD:                 // The RTP packet we want to monitor or record
                OnRTPPayload(*pCapMsg);
                break;

            case Msgs::ACTIVE_CALL_LIST:            // Calls in progress
                OnActiveCallList(*pCapMsg);
                break;

            case Msgs::MONITORED_CALL_LIST:         // Monitored calls
                OnMonitoredCallList(*pCapMsg);
                break;

            case Msgs::MONITORED_RTP_STREAMS:       // Monitored RTP streams
                OnMonitoredRTPStreams(*pCapMsg);
                break;

            case Msgs::MONITOR_CALL:                // Monitor call request
                OnMonitorCall(*pCapMsg);
                break;

            case Msgs::STOP_MONITOR_CALL:           // Stop monitor call
                OnStopMonitorCall(*pCapMsg);
                break;

            case Msgs::CONFIG_DATA:                 // Config data
                OnConfigData(*pCapMsg);
                break;

            case Msgs::START_RECORD:                // Start record
                OnStartRecord(*pCapMsg);
                break;

            case Msgs::START_RECORD_NOW:            // Start record now
                OnStartRecordNow(*pCapMsg);
                break;

            case Msgs::STOP_RECORD:                 // Stop record
                OnStopRecord(*pCapMsg);
                break;

            case Msgs::RECORD_CONFIG_DATA:          // Record config data
                OnRecordConfigData(*pCapMsg);
                break;

            case Msgs::HEART_BEAT:                  // Heartbeat
                OnHeartbeat(*pCapMsg);

            default:
                logger->WriteLog(Log_Warning, "Unknown msg received: %d", pCapMsg->type());
                break;
        }

        STAT_END(runtimeSvcExecTime);
        STAT(("STAT: RuntimeSvc --:: %d %d", pCapMsg->type(), runtimeSvcExecTime));

        if(pCapMsg->isPersistent() == false)       
            pCapMsg->release();

        msg = 0;
    }

    return 0;
}

void PCapRuntime::OnStart(PCapMessage& message)
{
    PCapPacketManager::Instance()->SetRuntime(this);

    if (!ipcServer.Start())
    {
        PCapMessage* pMsg = new PCapMessage();
        pMsg->type(Msgs::IPC_FAILED_START);
        this->AddMessage(pMsg);
        return;
    }

    if (-1 == ACE_Thread_Manager::instance()->spawn(PCapThreadFunc, this, THR_NEW_LWP | THR_JOINABLE,
													&hThreadPCap))
	{
        PCapMessage* pMsg = new PCapMessage();
        pMsg->type(Msgs::PCAP_FAILED_START);
        this->AddMessage(pMsg);
        return;
	}

    if (-1 == ACE_Thread_Manager::instance()->spawn(PatrolThreadFunc, this, THR_NEW_LWP | THR_JOINABLE,
													&hThreadPatrol))
	{
        PCapMessage* pMsg = new PCapMessage();
        pMsg->type(Msgs::PATROL_FAILED_START);
        this->AddMessage(pMsg);
        return;
	}

    this->runtimeStartedMutex.acquire();
    this->runtimeStarted.signal();
    this->runtimeStartedMutex.release();
}

void PCapRuntime::OnStop(PCapMessage& message)
{
    this->bShuttingDown = true;

    ipcServer.Stop();

    ACE_Thread_Manager *tm = ACE_Thread_Manager::instance();
    ACE_ASSERT(tm);

    if(tm->testresume(hThreadPCap) == 1)
    {
        tm->join(hThreadPCap);
    }

    if(tm->testresume(hThreadPatrol) == 1)
    {
        tm->join(hThreadPatrol);
    }

    this->runtimeStoppedMutex.acquire();
    this->runtimeStopped.signal();
    this->runtimeStoppedMutex.release();
}

void PCapRuntime::OnSkinnyCallData(PCapMessage& message)
{
    this->m_LivePhonesMutex.acquire();

    skinny_call_data* callData = NULL;

    // SKINNY_MSG_STATIONOPENRECEIVECHANNELACK does not include call id but pass thru party id.
    // Process this speical case first.
    if (message.callData()->msgId == SKINNY_MSG_STATIONOPENRECEIVECHANNELACK)
    {
        bool bFound = false;
        CallIdToCallDataMap_iterator iter;
        for(iter=livePhones.begin(); iter!= livePhones.end(); iter++)
        {   
            if  (NULL == (callData = iter->second)) 
                continue;

            if (callData->passThruPartyId == message.callData()->passThruPartyId)
            {
                callData->localIp = message.callData()->localIp;
                callData->localRTPPort = message.callData()->localRTPPort;
                bFound = true;
                break;
            }
        }

        // If the code is running in active mode then monitor it automatically.
        if (bFound)
        {
            if (this->params.activeMode  && HasCompletedCallData(callData))
            {
                if (callData->callType == SKINNY_OUTBOUND_CALL && !callData->active)
                    callData->active = true;
                AddMonitoredCall(callData);
                callData->monitored = true;
            }

            if (callData->hold && HasCompletedCallData(callData))
                ResumeMonitoredCall(callData);
        }
        this->m_LivePhonesMutex.release();
        return;
    }

    // check if this is a new call
    if(livePhones.find(message.callData()->callIdentifier) == livePhones.end())
    {
        callData = new skinny_call_data;
        memset(callData, 0, sizeof(skinny_call_data));
        memcpy(callData, message.callData(), sizeof(skinny_call_data));
        livePhones[message.callData()->callIdentifier] = callData;        
        logger->WriteLog(Log_Verbose, "New live phone added, number of live phones is %d", livePhones.size()); 
    }
    else
    {
        callData = livePhones[message.callData()->callIdentifier];
    }

    if (callData == NULL)
    {
        this->m_LivePhonesMutex.release();
        return;
    }

    callData->msgId = message.callData()->msgId;
            
    switch(message.callData()->msgId)
    {
        case SKINNY_MSG_STARTMEDIATRANSMISSION:
            callData->callIdentifier = message.callData()->callIdentifier;
            callData->passThruPartyId = message.callData()->passThruPartyId;
            callData->remoteIp = message.callData()->remoteIp;
            callData->remoteRTPPort = message.callData()->remoteRTPPort;
       
            // Id running in Active mode then monitor the call automatically
            if (this->params.activeMode  && HasCompletedCallData(callData))
            {
                AddMonitoredCall(callData);
                callData->monitored = true;
            }

            if (callData->hold && HasCompletedCallData(callData))
                ResumeMonitoredCall(callData);

            break;

        case SKINNY_MSG_STOPMEDIATRANSMISSION:
            callData->callIdentifier = message.callData()->callIdentifier;
            callData->passThruPartyId = message.callData()->passThruPartyId;
            if (callData->monitored && !callData->hold)
                HoldMonitoredCall(callData);
            break;

        case SKINNY_MSG_CALLINFO:
            callData->callIdentifier = message.callData()->callIdentifier;
            callData->stationIp = message.callData()->stationIp;
            callData->callType = message.callData()->callType;
            strcpy(callData->calleeDN, message.callData()->calleeDN);
            strcpy(callData->callerDN, message.callData()->callerDN);
            strcpy(callData->calleeName, message.callData()->calleeName);
            strcpy(callData->callerName, message.callData()->callerName);
            if (params.activeMode)
                UpdateCallStatus(callData->callIdentifier, 0, callData->callType, 
                                 callData->callerDN, callData->callerName, 
                                 callData->calleeDN, callData->calleeName);
            break;

        case SKINNY_MSG_OPENRECEIVECHANNEL:
            callData->callIdentifier = message.callData()->callIdentifier;
            callData->passThruPartyId = message.callData()->passThruPartyId;
            callData->payloadCapability = message.callData()->payloadCapability;
            break;

        case SKINNY_MSG_CLOSERECEIVECHANNEL:
            callData->callIdentifier = message.callData()->callIdentifier;
            callData->passThruPartyId = message.callData()->passThruPartyId;
            if (callData->monitored && !callData->hold)
                HoldMonitoredCall(callData);
            break;

        case SKINNY_MSG_CALLSTATE:
            callData->callIdentifier = message.callData()->callIdentifier;
            callData->callState = message.callData()->callState;

            switch(callData->callState)
            {
                case SKINNY_CONNECTED:
                    callData->active = true;
                    if (callData->hold && HasCompletedCallData(callData))
                        ResumeMonitoredCall(callData);
                    if (params.activeMode)
                        UpdateCallStatus(callData->callIdentifier, callData->callState, 0, 0, 0, 0, 0);
                    break;

                case SKINNY_HOLD:
                    // We should keep the monitored call but remove the monitored RTP streams
                    // When resume, two new RTP streams should add to the same monitored call.
                    if (callData->monitored && !callData->hold)
                        HoldMonitoredCall(callData);
                    if (params.activeMode)
                        UpdateCallStatus(callData->callIdentifier, callData->callState, 0, 0, 0, 0, 0);
                    break;

                case SKINNY_ONHOOK:
                    {
                    u_int callIdentifier = callData->callIdentifier;

                    if (params.activeMode)
                        UpdateCallStatus(callData->callIdentifier, SKINNY_PRE_ONHOOK, 0, 0, 0, 0, 0);

                    RemoveMonitoredCall(callData->callIdentifier, true);

                    // Make sure this update is after RemoveMonitoredCall, so we can be sure that we have all audios
                    // when ONHOOK is received on the client side.
                    if (params.activeMode)
                        UpdateCallStatus(callData->callIdentifier, callData->callState, 0, 0, 0, 0, 0);

                    // Remove it from active phone
                    delete callData;
                    callData = NULL;
                    livePhones.erase(callIdentifier);

                    logger->WriteLog(Log_Verbose, "Active phone removed, number of active phones = %d", livePhones.size());
                    }
                    break;

                default:
                    break;
            }
            break;

        default:
            break;
    }

    this->m_LivePhonesMutex.release();
}


void PCapRuntime::OnRTPPayload(PCapMessage& message)
{
    // We have a RTP payload come in, need to decide what to do with it.
    skinny_call_data* callData = message.callData();

    rtp_routing_params* params = NULL;
    // Is this stream currently monitored?
    m_MonitoredRtpStreamsMutex.acquire();
    if (monitoredRtpStreams.find(callData->callIdentifier /* this is actually RTP stream id */) == monitoredRtpStreams.end())
    {
        // new stream, make sure the call is still there
        m_MonitoredCallsMutex.acquire();

        bool bFound = false;
        rtp_routing_params* rrp = NULL;    
        CallIdToRtpRouteMap_iterator iter;

        for(iter=monitoredCalls.begin(); iter!= monitoredCalls.end(); iter++)
        {   
            rrp = NULL; 
            if  (NULL == (rrp = iter->second)) 
                continue;

            if ((callData->localRTPPort != rrp->localPort) && (callData->localRTPPort != rrp->remotePort))
                continue;

            if ((callData->remoteRTPPort != rrp->remotePort) && (callData->remoteRTPPort != rrp->localPort))
                continue;

            if (!IsIdenticalIpAddress(callData->localIp, rrp->localIp) && !IsIdenticalIpAddress(callData->localIp, rrp->remoteIp))
                continue;

            if (!IsIdenticalIpAddress(callData->remoteIp, rrp->remoteIp) && !IsIdenticalIpAddress(callData->remoteIp, rrp->localIp))
                continue;

            if (rrp->destPort1Taken && rrp->destPort2Taken)
            {
                logger->WriteLog(Log_Error, "Attempt to add more than 2 RTP streams into a monitored call - %d", rrp->callIdentifier);
                /*
                logger->WriteLog(Log_Error, "RTP Packet: StreamId=%d, Src=%s:%d, Dst=%s:%d", callData->callIdentifier, 
                        callData->localIp, callData->localRTPPort, callData->remoteIp, callData->remoteRTPPort);
                logger->WriteLog(Log_Error, "Matched Monitored Call: CallId=%d, Src=%s:[mms]%d[rtp]%d, Dst=%s:[mms]%d[rtp]%d", 
                        rrp->callIdentifier, 
                        rrp->localIp, rrp->localPort, 
                        rrp->remoteIp, rrp->remotePort,
                        rrp->portbase1, rrp->portbase2);
                */
                continue;       // we have two streams for this call already.
            }

            if (rrp->isOnHold)
                continue;       // we have two streams for this call already.
          
            // if we gets here then there is a match
            bFound = true;
            break;
        }        

        if (bFound && rrp != NULL)
        {
            rtp_routing_params* params = new rtp_routing_params;        // delete when call goes onhook
            memcpy(params, rrp, sizeof(rtp_routing_params));
            params->rtpStreamIdentifier = callData->callIdentifier;     // this is actually RTP stream id
            params->localIp = callData->localIp;                        // source ip
            params->localPort = callData->localRTPPort;                 // source port
            params->remoteIp = callData->remoteIp;                      // dest ip
            params->remotePort = callData->remoteRTPPort;               // dest port
            
            if (!rrp->destPort1Taken)
            {
                // if no port defined, just use remote port + 10 (TODO: Is it safe???)
                params->destPort1 = (rrp->destPort1 == 0) ? params->remotePort + 10 : rrp->destPort1;  
                rrp->destPort1 = params->destPort1;     // set it in case it was 0
                rrp->destPort1Taken = true;
            }
            else if (!rrp->destPort2Taken)
            {
                params->destPort1 = (rrp->destPort2 == 0) ? params->remotePort + 10 : rrp->destPort2;  
                rrp->destPort2 = params->destPort1;     // set it in case it was 0
                rrp->destPort2Taken = true;
            }
            else
            {
                // should not be here
                params->destPort1 = params->remotePort + 10; 
            }

            if (!rrp->portbase1Taken)
            {
                params->portbase1 = (rrp->portbase1 == 0) ? this->GetNextPortbase() : rrp->portbase1;  
                rrp->portbase1 = params->portbase1;     // set it in case it was 0
                rrp->portbase1Taken = true;
            }
            else if (!rrp->portbase2Taken)
            {
                params->portbase1 = (rrp->portbase2 == 0) ? this->GetNextPortbase() : rrp->portbase2; 
                rrp->portbase2 = params->portbase1;     // set it in case it was 0
                rrp->portbase2Taken = true;
            }
            else
            {
                // Should not be here
                params->portbase1 = this->GetNextPortbase();
            }

            m_MonitoredCallsMutex.release();

            if (params->sendRTPToIp)
            {
                params->sender = new PCapRTPSender();
                params->sender->CreateRTPSession(params->destIpAddress, params->destPort1, callData->payloadCapability, params->portbase1);
                logger->WriteLog(Log_Info, "CreateRTPSession dport=%d, bport=%d, id=%d", params->destPort1, params->portbase1, callData->callIdentifier);
            }
            if (params->sendRTPToFile)
            {
                params->writer = new PCapRTPWriter();
                params->writer->CreateRTPFile(this->GetParams().archiveFolder, params->callIdentifier, params->destPort1);
                logger->WriteLog(Log_Info, "CreateRTPFile in %s, and port = %d", this->GetParams().archiveFolder, params->destPort1);
            }

            params->monitorTimeLeft = this->maxMonitorTime;

            monitoredRtpStreams[callData->callIdentifier] = params;
            logger->WriteLog(Log_Info, "New monitored RTP stream added, number of monitored RTP streams is %d", monitoredRtpStreams.size()); 
        }
        else
        {
            // RPT packet does not match call
        }
    }
    else
    {
        // existing stream
        params = monitoredRtpStreams[callData->callIdentifier];
    }

    if (params == NULL)
    {
        m_MonitoredRtpStreamsMutex.release();
        return;
    }

    if (params->sendRTPToIp && params->sender)
    {
        params->sender->SendPacket(message.metreosData(), message.param());
    }

    if (params->sendRTPToFile && params->writer)
    {
        params->writer->WritePacket(message.metreosData(), message.param());
    }

    m_MonitoredRtpStreamsMutex.release();
}

ACE_THR_FUNC_RETURN PCapRuntime::PatrolThreadFunc(void* data)
{
    PCapRuntime* runtime = static_cast<PCapRuntime*>(data);
    ACE_ASSERT(runtime != NULL);

    while(!runtime->isShuttingDown())
    {
        ACE_OS::sleep(60);              // Sleep for 60 secs
        runtime->PatrolMonitoredCalls();
    }
    return 0;
}

ACE_THR_FUNC_RETURN PCapRuntime::PCapThreadFunc(void* data)
{
    PCapRuntime* runtime = static_cast<PCapRuntime*>(data);
    ACE_ASSERT(runtime != NULL);

    // Interact with WinPCap to capture packets on a pre-defined interface        
    // Retrieve the device list on the local machine
    pcap_if_t *alldevs;
    char errbuf[PCAP_ERRBUF_SIZE];

    if (pcap_findalldevs(&alldevs, errbuf) == -1)
    {
        logger->WriteLog(Log_Error, "Unable to find network interfaces - %s.", errbuf);
        PCapMessage* pMsg = new PCapMessage();
        pMsg->type(Msgs::PCAP_FAILED_START);
        runtime->AddMessage(pMsg);
        return 0;
    }
    
    // Go through each device to find the pre-defined interface
    int numNics = 0;
    pcap_if_t *d;
    for(d=alldevs; d; d=d->next)
        ++numNics;
    
    if(numNics==0)
    {
        logger->WriteLog(Log_Error, "No network interface found, make sure the software is properly installed.");
        PCapMessage* pMsg = new PCapMessage();
        pMsg->type(Msgs::PCAP_FAILED_START);
        runtime->AddMessage(pMsg);
        return 0;
    }

    int inum = runtime->GetParams().monnic;

    if (inum > numNics)
    {
        logger->WriteLog(Log_Error, "Network interface %d does not exist, there is only %d interfaces on this system.", inum, numNics);
        PCapMessage* pMsg = new PCapMessage();
        pMsg->type(Msgs::PCAP_FAILED_START);
        runtime->AddMessage(pMsg);
        return 0;
    }

    // Jump to the selected adapter
    int i = 0;
    for(d=alldevs, i=0; i<inum-1 ;d=d->next, i++);
    
    // Open the device
    pcap_t *adhandle;
    if ((adhandle= pcap_open_live( d->name,                         // name of the device
                                    65536,                          // portion of the packet to capture. 
                                                                    // 65536 guarantees that the whole packet will be captured on all the link layers
                                    1,                              // promiscuous mode
                                    1000,                           // read timeout
                                    errbuf                          // error buffer
                                    ) ) == NULL)
    {
        logger->WriteLog(Log_Error, "Unable to open adapter %s [%s].",  d->name, d->description);
        pcap_freealldevs(alldevs);
        PCapMessage* pMsg = new PCapMessage();
        pMsg->type(Msgs::PCAP_FAILED_START);
        runtime->AddMessage(pMsg);
        return 0;
    }

	// Check the link layer. We support only Ethernet for simplicity.
	if(pcap_datalink(adhandle) != DLT_EN10MB)
	{
        logger->WriteLog(Log_Error, "This program only supports Ethernet network.");
        pcap_freealldevs(alldevs);
        PCapMessage* pMsg = new PCapMessage();
        pMsg->type(Msgs::PCAP_FAILED_START);
        runtime->AddMessage(pMsg);
        return 0;
	}

    // Set WinPCap capture buffer to a larger value, WinPCap's default is 1MB.
    if (pcap_setbuff(adhandle, CAPTURE_BUFFER_SIZE) < 0)
    {
        logger->WriteLog(Log_Error, "Unable to set capture buffer size, default to 1MB.");
    }
	
	u_int netmask;
	if(d->addresses != NULL)
		// Retrieve the mask of the first address of the interface
		netmask = ((struct sockaddr_in *)(d->addresses->netmask))->sin_addr.S_un.S_addr;
	else
		// If the interface is without addresses we suppose to be in a C class network
		netmask=0xffffff; 

	//compile the filter
	//char packet_filter[] = "udp or (tcp port 2000)";
    //char packet_filter[] = "tcp port 2000";   // for testing purposes
    char packet_filter[] = "(udp or (tcp port 2000)) or (vlan and (udp or tcp port 2000))";
	struct bpf_program fcode;
	if (pcap_compile(adhandle, &fcode, packet_filter, 1, netmask) <0)
	{
        logger->WriteLog(Log_Error, "Failed to compile packet filter.");
        pcap_freealldevs(alldevs);
        PCapMessage* pMsg = new PCapMessage();
        pMsg->type(Msgs::PCAP_FAILED_START);
        runtime->AddMessage(pMsg);
        return 0;
	}

	//set the filter
	if (pcap_setfilter(adhandle, &fcode)<0)
	{
        logger->WriteLog(Log_Error, "Error setting packet filter.");
        pcap_freealldevs(alldevs);
        PCapMessage* pMsg = new PCapMessage();
        pMsg->type(Msgs::PCAP_FAILED_START);
        runtime->AddMessage(pMsg);
        return 0;
	}

    logger->WriteLog(Log_Info, "Listening on adapter %s [%s]...",  d->name, d->description);
    
    // At this point, we don't need any more the device list.
    pcap_freealldevs(alldevs);
    
    // Retrieve the packets for the life of the thread
    int res = -1;
    const u_char *pkt_data;
    struct pcap_pkthdr *header;
    while(!runtime->isShuttingDown() && (res = pcap_next_ex(adhandle, &header, &pkt_data)) >= 0)
    {    
        // Timeout elapsed        
        if(res == 0) 
            continue;

        char* packetData = new char[header->len];
        ACE_OS::memset(packetData, 0, header->len);
        ACE_OS::memcpy(packetData, pkt_data, header->len); 
        PCapMessage* pMsg = new PCapMessage(packetData, header->len);
        pMsg->type(Msgs::PACKET);
        pMsg->packetHeader(header);
        // Since ACE queue has size limitation (adjustable) and we are receiving large number of 
        // packet here.  Let's pre-process them here instead of putting them into ACE queue.
        PCapPacketManager::Instance()->ProcessPacket(*pMsg);

        if(pMsg->isPersistent() == false)       
            pMsg->release();

        pMsg = 0;
    }

    pcap_close(adhandle);
    
    if(res == -1)
    {
        logger->WriteLog(Log_Error, "Error reading the packets, error is %s.",  pcap_geterr(adhandle));
        return 0;
    }
    
    return 0;
}


bool PCapRuntime::PreParseUDPCheck(PCapMessage& message, int offset)
{
    return (monitoredCalls.size() > 0);

    /*
    bool bFound = false;

    m_MonitoredCallsMutex.acquire();

    if (monitoredCalls.size() == 0)
    {
        // No monitored call, ignore all RTP packets
        m_MonitoredCallsMutex.release();
        return bFound;
    }
        
    // Make sure this UDP packet is what we want by checking Source and Destination
    // IP addresses and ports.
    char* pkt_data = message.metreosData();

    // retrieve ip header
	ip_header* ih = (ip_header*) (pkt_data + offset);
    
    // retrieve udp header
	int ih_len = (ih->ver_ihl & 0xf) * 4;
	udp_header* uh = (udp_header*) ((u_char*)ih + ih_len);

	// convert from network byte order to host byte order
	u_short sport = ntohs(uh->sport);
	u_short dport = ntohs(uh->dport);

    ip_address saddr = ih->saddr;
    ip_address daddr = ih->daddr;

    CallIdToRtpRouteMap_iterator iter;
    for(iter=monitoredCalls.begin(); iter!= monitoredCalls.end(); iter++)
    {   
        rtp_routing_params* rrp = NULL;    
        if  (NULL == (rrp = iter->second)) 
            continue;

        if ((sport != rrp->localPort) && (sport != rrp->remotePort))
            continue;

        if ((dport != rrp->localPort) && (dport != rrp->remotePort))
            continue;

        if ((!IsIdenticalIpAddress(saddr, rrp->localIp)) && (!IsIdenticalIpAddress(saddr, rrp->remoteIp)))
            continue;

        if ((!IsIdenticalIpAddress(daddr, rrp->localIp)) && (!IsIdenticalIpAddress(daddr, rrp->remoteIp)))
            continue;

        // if we gets here then there is a match
        bFound = true;
        break;
    }
    
    m_MonitoredCallsMutex.release();

    return bFound;
    */
}

bool PCapRuntime::IsIdenticalIpAddress(ip_address ip1, ip_address ip2)
{
    // Assume the phones are under the same subnet, let's compare the last octet first.

    if (ip1.byte4 != ip2.byte4)
        return false;

    if (ip1.byte3 != ip2.byte3)
        return false;

    if (ip1.byte2 != ip2.byte2)
        return false;

    if (ip1.byte1 != ip2.byte1)
        return false;
    
    return true;
}

bool PCapRuntime::HasCompletedCallData(skinny_call_data* callData)
{
    if (callData == NULL)
        return false;
    
    if (callData->callIdentifier == 0)
        return false;

    // We do not need to look into all four octets.
    if (callData->localIp.byte1  == 0 || callData->localRTPPort == 0)
        return false;

    if (callData->remoteIp.byte1  == 0 || callData->remoteRTPPort == 0)
        return false;

    if (callData->stationIp.byte1 == 0)
        return false;

    // Do not allow monitoring a call if both caller DN and Callee DN are missing.
    if ((strlen(callData->callerDN) == 0) && (strlen(callData->calleeDN) == 0))
        return false;
    
    return true;
}

void PCapRuntime::AddMonitoredCall(skinny_call_data* callData, const char* daddr, 
                                   const u_short dport1, const u_short dport2,
                                   const u_short portbase1, const u_short portbase2)
{
    m_MonitoredCallsMutex.acquire();
    rtp_routing_params* rp = NULL;
    if (monitoredCalls.find(callData->callIdentifier) == monitoredCalls.end())
    {
        rp = new rtp_routing_params;        // will release it when onhook
        memset(rp, 0, sizeof(rtp_routing_params));

        rp->sendRTPToFile = this->params.sendRTPToFile;
        rp->sendRTPToIp = this->params.sendRTPToIp;
        rp->callIdentifier = callData->callIdentifier;
        rp->localIp = callData->localIp;
        rp->localPort = callData->localRTPPort;
        rp->remoteIp = callData->remoteIp;
        rp->remotePort = callData->remoteRTPPort;

        if (daddr)
            strcpy(rp->destIpAddress, daddr);
        else
            strcpy(rp->destIpAddress, this->params.destIpAddress);
        rp->destPort1 = dport1;
        rp->destPort2 = dport2;
        rp->portbase1 = portbase1;
        rp->portbase2 = portbase2;

        rp->monitorTimeLeft = this->maxMonitorTime;

        monitoredCalls[callData->callIdentifier] = rp;

        if (params.activeMode)
        {
            // Notify agent we have a new call
            NotifyNewCall(callData->callIdentifier, callData->callType, 
                          callData->callerDN, callData->callerName,
                          callData->calleeDN, callData->calleeName);
        }
    }
    else
    {
        rp = monitoredCalls[callData->callIdentifier];
        if (rp != NULL)
        {
            rp->localIp = callData->localIp;
            rp->localPort = callData->localRTPPort;
            rp->remoteIp = callData->remoteIp;
            rp->remotePort = callData->remoteRTPPort;

            if (!rp->isOnHold)
            {
                if (daddr)
                    strcpy(rp->destIpAddress, daddr);
                else
                    strcpy(rp->destIpAddress, this->params.destIpAddress);

                if (rp->destPort1 == 0) rp->destPort1 = dport1;
                if (rp->destPort2 == 0) rp->destPort2 = dport2;
                if (rp->portbase1 == 0) rp->portbase1 = portbase1;
                if (rp->portbase2 == 0) rp->portbase2 = portbase2;
            }
        }
    }

    logger->WriteLog(Log_Info, "New monitored call added, number of monitored calls = %d", monitoredCalls.size());
    m_MonitoredCallsMutex.release();

    if (this->params.activeMode)
    {
        // Notify that a call has been monitored

    }
}

void PCapRuntime::RemoveMonitoredCall(u_int callIdentifier, bool onHook)
{
    // remove monitored call
    bool bNeedNotifyFile = false;
    char szFilePath[MAX_FILE_PATH];
    m_MonitoredCallsMutex.acquire();
    if (monitoredCalls.find(callIdentifier) != monitoredCalls.end())
    {
        rtp_routing_params* rp = monitoredCalls[callIdentifier];
        if (rp)
        {
            if (rp->sendRTPToFile)
            {
                // Mix two RTP stream into one
                MixRtpStreams(rp->callIdentifier, rp->destPort1, rp->destPort2);
                memset(&szFilePath, 0, sizeof(szFilePath));
                wsprintf(szFilePath, "%s\\%d.au", params.archiveFolder, callIdentifier);
                bNeedNotifyFile = true;
            }
            delete rp;
            rp = NULL;
        }
        monitoredCalls.erase(callIdentifier);
    }
    m_MonitoredCallsMutex.release();

    RemoveMonitoredRTPStreams(callIdentifier);

    FlatMapWriter dataWriter;
    dataWriter.insert(Params::IDENTIFIER, (int)callIdentifier);         // Call identifier

    if (bNeedNotifyFile)
        dataWriter.insert(Params::RECORD_FILE, FlatMap::STRING, (int)ACE_OS::strlen(szFilePath)+1, szFilePath);

    this->WriteToIpc(Msgs::CALL_REMOVED, dataWriter); 

    logger->WriteLog(Log_Info, "Call removed, number of active phones = %d, monitored calls = %d, monitored streams = %d", 
            livePhones.size(),
            monitoredCalls.size(),
            monitoredRtpStreams.size());
}

void PCapRuntime::RemoveMonitoredRTPStreams(u_int callIdentifier)
{
    // remove monitored stream, one monitored call has two RTP streams, so do the best to find them.
    bool bFoundStream = false;
    m_MonitoredRtpStreamsMutex.acquire();
    do
    {
        bFoundStream = false;       // assume no more
        rtp_routing_params* rrp = NULL;    
        RtpStreamIdToRtpRouteMap_iterator iter;
        for(iter=monitoredRtpStreams.begin(); iter!= monitoredRtpStreams.end(); iter++)
        {   
            rrp = NULL;    
            if  (NULL == (rrp = iter->second)) 
                continue;

            if (rrp->callIdentifier == callIdentifier)
            {
                u_int streamId = rrp->rtpStreamIdentifier;

                if (rrp->sender)
                    delete rrp->sender;                

                if (rrp->writer)
                    delete rrp->writer;

                delete rrp;
                rrp = NULL;
                monitoredRtpStreams.erase(streamId);
                bFoundStream = true;
                break;
            }
        }        
    } while (bFoundStream);     // still have stream, try again
    m_MonitoredRtpStreamsMutex.release();
}

void PCapRuntime::ResumeMonitoredCall(skinny_call_data* callData)
{
    callData->hold = false;
    u_int callIdentifier = callData->callIdentifier;

    // Find monitored call
    m_MonitoredCallsMutex.acquire();
    if (monitoredCalls.find(callIdentifier) != monitoredCalls.end())
    {
        rtp_routing_params* rp = monitoredCalls[callIdentifier];
        if (rp)
        {
            rp->isOnHold = false;
            rp->localPort = callData->localRTPPort;
            rp->remotePort = callData->remoteRTPPort;

            logger->WriteLog(Log_Info, "Resume monitored call, RTP port1 = %d, port2 = %d", rp->localPort, rp->remotePort);
        }
    }
    m_MonitoredCallsMutex.release();
}

void PCapRuntime::PatrolMonitoredCalls()
{
    logger->WriteLog(Log_Verbose, "Patrol Monitored Calls to remove any out of sync calls and RTP streams.");

    // Search for expired call and remove them
    m_MonitoredCallsMutex.acquire();
    rtp_routing_params* rrp = NULL;    
    CallIdToRtpRouteMap_iterator iter;
    for(iter=monitoredCalls.begin(); iter!= monitoredCalls.end(); iter++)
    {
        rrp = NULL; 
        if  (NULL == (rrp = iter->second)) 
            continue;

        if (rrp->monitorTimeLeft < 0)
        {
            // remove the call, and mark the phone not monitored.
            u_int callIdentifier = rrp->callIdentifier;

            delete rrp;
            rrp = NULL;
            monitoredCalls.erase(callIdentifier);

            m_LivePhonesMutex.acquire();
            skinny_call_data* callData = livePhones[callIdentifier];
            if (callData != NULL)
                callData->monitored = false;
            m_LivePhonesMutex.release();
        }
        else
        {
            rrp->monitorTimeLeft--;     // decrease one minute
        }
    }
    m_MonitoredCallsMutex.release();    
}

void PCapRuntime::HoldMonitoredCall(skinny_call_data* callData)
{
    callData->hold = true;
    callData->localRTPPort = 0;
    callData->remoteRTPPort = 0;

    // Find monitored call
    u_int callIdentifier = callData->callIdentifier;
    bool bFoundMonitoredCall = false;
    m_MonitoredCallsMutex.acquire();
    if (monitoredCalls.find(callIdentifier) != monitoredCalls.end())
    {
        rtp_routing_params* rp = monitoredCalls[callIdentifier];
        if (rp)
        {
            bFoundMonitoredCall = true;
            rp->isOnHold = true;
            rp->destPort1Taken = false;
            rp->destPort2Taken = false;
            rp->portbase1Taken = false;
            rp->portbase2Taken = false;
        }
    }
    m_MonitoredCallsMutex.release();

    if (!bFoundMonitoredCall)
    {
        logger->WriteLog(Log_Verbose, "Try to hold a call which does not exist!!");
        return;
    }

    // remove monitored stream, one monitored call has two RTP streams, so do the best to find them.
    bool bFoundStream = false;
    m_MonitoredRtpStreamsMutex.acquire();
    do
    {
        bFoundStream = false;       // assume no more
        rtp_routing_params* rrp = NULL;    
        RtpStreamIdToRtpRouteMap_iterator iter;
        for(iter=monitoredRtpStreams.begin(); iter!= monitoredRtpStreams.end(); iter++)
        {   
            rrp = NULL;    
            if  (NULL == (rrp = iter->second)) 
                continue;

            if (rrp->callIdentifier == callIdentifier)
            {
                u_int streamId = rrp->rtpStreamIdentifier;

                if (rrp->sender)
                    delete rrp->sender;

                if (rrp->writer)
                    delete rrp->writer;

                delete rrp;
                rrp = NULL;
                monitoredRtpStreams.erase(streamId);
                bFoundStream = true;
                break;
            }
        }        
    } while (bFoundStream);     // still have stream, try again

    m_MonitoredRtpStreamsMutex.release();

    logger->WriteLog(Log_Info, "Monitored RTP streams removed [HOLD], number of active phones = %d, monitored calls = %d, monitored streams = %d", 
            livePhones.size(),
            monitoredCalls.size(),
            monitoredRtpStreams.size());
}

int PCapRuntime::AddMessage(PCapMessage* pMsg)
{
    if (this->isShuttingDown())
        return -1;

    // TODO: Remove this log when ACE queue overflow is not an issue.
    if (this->msg_queue()->is_full())
        logger->WriteLog(Log_Error, "CRITICAL ERROR!!!, message queue is FULL!  Please restart this service!!!");
 
    // Let's fail it even queue is full, ACE will stop runtime and its associated threads,
    // that means pcap-service must be restarted.  We will handle it if the traffic actually is that high.
    // TODO: We may need to queue up messages if ACE QUEUE is full.
    return this->putq(pMsg);
}

void PCapRuntime::OnActiveCallList(PCapMessage& message)
{
    logger->WriteLog(Log_Info, "[%d] >>>>> Receive active call list request", message.param());

    // Go through live phones and find the active calls
    FlatMapWriter dataWriter;
    int numActiveCalls = 0;
    skinny_call_data* callData = NULL;
    CallIdToCallDataMap_iterator iter;
    char szCallData[128];

    FlatMapReader reader(message.metreosData());
    char* p = NULL;
    u_int transactionId = 0;
    reader.find(Params::TRANSACTION_ID, &p);
    if (p)
        transactionId = *((u_int*)p);

    m_LivePhonesMutex.acquire();
    for(iter=livePhones.begin(); iter!= livePhones.end(); iter++)
    {   
        if  (NULL == (callData = iter->second)) 
            continue;

        if (!callData->active || callData->monitored)
            continue;

        memset(&szCallData, 0, sizeof(szCallData));
        if (callData->callType == SKINNY_INBOUND_CALL)
            wsprintf(szCallData, "%d&%s", 
                    callData->callIdentifier, 
                    callData->calleeDN);         // inbound, use callee
        else
            wsprintf(szCallData, "%d&%s", 
                    callData->callIdentifier, 
                    callData->callerDN);         // outbound, use caller
                 
        dataWriter.insert(Params::CALL_DATA, FlatMap::STRING, (int)ACE_OS::strlen(szCallData)+1, szCallData);

        logger->WriteLog(Log_Verbose, "[%d] <<<<< Active Call Data %s", message.param(), szCallData);

        numActiveCalls++;
    }
    m_LivePhonesMutex.release();

    dataWriter.insert(Params::NUM_ENTRIES, (int)numActiveCalls);
    dataWriter.insert(Params::TRANSACTION_ID, (int)transactionId);

    ipcServer.Write(Msgs::ACTIVE_CALL_LIST, dataWriter, message.param() /* client session id */);

    logger->WriteLog(Log_Info, "[%d] <<<<< Send back %d in-progress and non-monitored call(s).", message.param(), numActiveCalls);
}

void PCapRuntime::OnMonitoredCallList(PCapMessage& message)
{
}

void PCapRuntime::OnMonitoredRTPStreams(PCapMessage& message)
{
}

void PCapRuntime::OnMonitorCall(PCapMessage& message)
{
    // Client request to monitor an active call, we need to make sure the call is still active and send back result with ACK
    FlatMapReader reader(message.metreosData());
    char* p = NULL;

    // Transaction Id
    u_int transactionId = 0;
    reader.find(Params::TRANSACTION_ID, &p);
    if (p)
        transactionId = *((u_int*)p);

    // Call Identifier
    u_int callIdentifier = 0;
    reader.find(Params::IDENTIFIER, &p);
    if (p)
        callIdentifier = *((u_int*)p);

    // Destination IP Address
    char* daddr = NULL;
    int len = reader.find(Params::MONITORED_IP, &p);
    if (p != NULL)
    {
        daddr = new char[len+1];
        ACE_OS::strncpy(daddr, p, len+1);
        daddr[len] = 0;
    }

    // Destination Ports
    p = NULL;
    u_short dport1 = 0, dport2 = 0;
    len = reader.find(Params::MONITORED_PORT, &p, 0, 1);
    if (p != NULL)
        dport1 = *((u_short*)p);

    p = NULL;
    len = reader.find(Params::MONITORED_PORT, &p, 0, 2);
    if (p != NULL)
        dport2 = *((u_short*)p);

    logger->WriteLog(Log_Info, "[%d] >>>>> Receive monitor call request, Call Identifier = %d, Destination = %s:%d:%d",
            message.param(), callIdentifier, daddr, dport1, dport2);

    // Make sure the call is still active and non-monitored
    m_LivePhonesMutex.acquire();
    skinny_call_data* callData = NULL;

    callData = livePhones[callIdentifier];

    FlatMapWriter dataWriter;
    dataWriter.insert(Params::TRANSACTION_ID, (int)transactionId);

    if (callData && callData->active && !callData->monitored && HasCompletedCallData(callData))
    {
        u_short txPort1 = GetNextPortbase();
        u_short txPort2 = GetNextPortbase();
        AddMonitoredCall(callData, daddr, dport1, dport2, txPort1, txPort2);
        callData->monitored = true;
        // Ack Success
        dataWriter.insert(Params::IDENTIFIER, (int)callIdentifier);         // Call identifier
        dataWriter.insert(Params::RESULT_CODE, 0);                          // result code
        dataWriter.insert(Params::MONITORED_PORT, (int)txPort1);            // RTP port1
        dataWriter.insert(Params::MONITORED_PORT, (int)txPort2);            // RTP port2
        
        dataWriter.insert(Params::CALL_TYPE, (int)callData->callType);            // RTP port2

        char szDN[STATION_MAX_DN_SIZE];
        memset(&szDN, 0, sizeof(STATION_MAX_DN_SIZE));
        strcpy(szDN, callData->callerDN);
        dataWriter.insert(Params::CALLER_DN, FlatMap::STRING, (int)ACE_OS::strlen(szDN)+1, szDN);

        memset(&szDN, 0, sizeof(STATION_MAX_DN_SIZE));
        strcpy(szDN, callData->calleeDN);
        dataWriter.insert(Params::CALLEE_DN, FlatMap::STRING, (int)ACE_OS::strlen(szDN)+1, szDN);

        memset(&szDN, 0, sizeof(STATION_MAX_DN_SIZE));
        wsprintf(szDN, "%d.%d.%d.%d", callData->stationIp.byte1, callData->stationIp.byte2, callData->stationIp.byte3, callData->stationIp.byte4);
        dataWriter.insert(Params::PHONE_IP, FlatMap::STRING, (int)ACE_OS::strlen(szDN)+1, szDN);

        char szName[STATION_MAX_NAME_SIZE];
        memset(&szName, 0, sizeof(STATION_MAX_NAME_SIZE));
        strcpy(szName, callData->callerName);
        dataWriter.insert(Params::CALLER_NAME, FlatMap::STRING, (int)ACE_OS::strlen(szName)+1, szName);

        memset(&szName, 0, sizeof(STATION_MAX_NAME_SIZE));
        strcpy(szName, callData->calleeName);
        dataWriter.insert(Params::CALLEE_NAME, FlatMap::STRING, (int)ACE_OS::strlen(szName)+1, szName);

        logger->WriteLog(Log_Info, "[%d] <<<<< Send monitor call ack - SUCCESS for Call Identifier=%d, RTP Tx Port1=%d, Port2=%d, Call Type=%d, Caller DN=%s, Callee DN=%s, SIP=%s", 
                message.param(), callIdentifier, txPort1, txPort2, 
                callData->callType, callData->callerDN, callData->calleeDN, szDN);
    }
    else
    {
        // Ack failure
        dataWriter.insert(Params::IDENTIFIER, (int)callIdentifier);
        dataWriter.insert(Params::RESULT_CODE, 1);
        logger->WriteLog(Log_Info, "[%d] <<<<< Send monitor call ack - FAILURE for Call Identifier = %d", message.param(), callIdentifier);
    }

    m_LivePhonesMutex.release();

    ipcServer.Write(Msgs::MONITOR_CALL_ACK, dataWriter, message.param() /* client session id */);
}

void PCapRuntime::OnStopMonitorCall(PCapMessage& message)
{
    FlatMapReader reader(message.metreosData());

    // Call Identifier
    char* p = NULL;
    u_int callIdentifier = 0;
    reader.find(Params::IDENTIFIER, &p);
    if (p)
        callIdentifier = *((u_int*)p);

    FlatMapWriter dataWriter;
    if (callIdentifier != 0)
    {
        m_LivePhonesMutex.acquire();
        skinny_call_data* callData = livePhones[callIdentifier];
        if (callData)
            callData->monitored = false;
        m_LivePhonesMutex.release();

        RemoveMonitoredCall(callIdentifier);
        // Ack Success
        dataWriter.insert(Params::IDENTIFIER, (int)callIdentifier);
        dataWriter.insert(Params::RESULT_CODE, 0);
        logger->WriteLog(Log_Info, "[%d] <<<<< Send stop monitor call ack - SUCCESS for Call Identifier = %d", message.param(), callIdentifier);
    }
    else
    {
        // Ack Failure
        dataWriter.insert(Params::IDENTIFIER, (int)callIdentifier);
        dataWriter.insert(Params::RESULT_CODE, 1);
        logger->WriteLog(Log_Info, "[%d] <<<<< Send stop monitor call ack - FAILURE for Call Identifier = %d", message.param(), callIdentifier);
    }
    ipcServer.Write(Msgs::STOP_MONITOR_CALL_ACK, dataWriter, message.param() /* client session id */);
}

void PCapRuntime::OnConfigData(PCapMessage& message)
{
    FlatMapReader reader(message.metreosData());

    // Portbase LWM
    char* p = NULL;
    u_int pLWM = 0;
    reader.find(Params::PORTBASE_LWM, &p);
    if (p)
    {
        pLWM = *((u_int*)p);
        if (pLWM != 0)
        {
            this->portbaseLWM = pLWM;
            this->portbase = pLWM;
        }
    }

    // Portbase HWM
    u_int pHWM = 0;
    reader.find(Params::PORTBASE_HWM, &p);
    if (p)
    {
        pHWM = *((u_int*)p);
        if (pHWM != 0)
            this->portbaseHWM = pHWM;
    }

    // Port step
    u_int pStep = 0;
    reader.find(Params::PORT_STEP, &p);
    if (p)
    {
        pStep = *((u_int*)p);
        if (pStep != 0)
            this->portStep = pStep;
    }

    // Max monitor time
    u_int pMaxMonitorTime = 0;
    reader.find(Params::MAX_MONITOR_TIME, &p);
    if (p)
    {
        pMaxMonitorTime = *((u_int*)p);
        if (pMaxMonitorTime != 0)
            this->maxMonitorTime = pMaxMonitorTime;
    }

    // Log level
    u_int pLogLevel = 0;
    reader.find(Params::LOG_LEVEL, &p);
    if (p)
    {
        pLogLevel = *((u_int*)p);
        if (pLogLevel >= 0 && pLogLevel <= 4)
            logger->SetLogLevel((LogClient::LogLevel)pLogLevel);
    }

    logger->WriteLog(Log_Info, "[%d] >>>>> Receive config data, Port LWM = %d, Port HWM = %d, Port Step = %d, Max Monitor Time = %d, Log Level = %d",
            message.param(), pLWM, pHWM, pStep, pMaxMonitorTime, pLogLevel);
}

u_short PCapRuntime::GetNextPortbase()
{
    if (portbase >= portbaseHWM)
        portbase = portbaseLWM;
    else
        portbase += portStep;

    return portbase;
}

void PCapRuntime::OnStartRecord(PCapMessage& message)
{
    // Send back ack to confirm the recording
    FlatMapReader reader(message.metreosData());
    char* p = NULL;

    // Call Identifier
    u_int callIdentifier = 0;
    reader.find(Params::IDENTIFIER, &p);
    if (p)
        callIdentifier = *((u_int*)p);

    logger->WriteLog(Log_Info, "[%d] >>>>> Receive start record request, Call Identifier = %d",
                    message.param(), callIdentifier);

    // Make sure the call is still active and under monitored
    m_LivePhonesMutex.acquire();
    skinny_call_data* callData = NULL;

    callData = livePhones[callIdentifier];

    FlatMapWriter dataWriter;
    if (callData && callData->active && callData->monitored && HasCompletedCallData(callData))
    {
        // Ack Success
        dataWriter.insert(Params::IDENTIFIER, (int)callIdentifier);         // Call identifier
        dataWriter.insert(Params::RESULT_CODE, 0);                          // Result code
        logger->WriteLog(Log_Info, "[%d] <<<<< Send start record call ack - SUCCESS for Call Identifier=%d", message.param(), callIdentifier);
    }
    else
    {
        // Ack failure
        dataWriter.insert(Params::IDENTIFIER, (int)callIdentifier);
        dataWriter.insert(Params::RESULT_CODE, 1);
        logger->WriteLog(Log_Info, "[%d] <<<<< Send start record call ack - FAILURE for Call Identifier = %d", message.param(), callIdentifier);
    }

    m_LivePhonesMutex.release();

    ipcServer.Write(Msgs::START_RECORD_ACK, dataWriter, message.param() /* client session id */);
}

void PCapRuntime::OnStartRecordNow(PCapMessage& message)
{
    // Send back ack to confirm the recording, but discard the recorded data.
    FlatMapReader reader(message.metreosData());
    char* p = NULL;

    // Call Identifier
    u_int callIdentifier = 0;
    reader.find(Params::IDENTIFIER, &p);
    if (p)
        callIdentifier = *((u_int*)p);

    logger->WriteLog(Log_Info, "[%d] >>>>> Receive start record now request, Call Identifier = %d",
                    message.param(), callIdentifier);

    bool bFoundMonitoredCall = false;

    char szFilePath1[MAX_FILE_PATH], szFilePath2[MAX_FILE_PATH];
    memset(&szFilePath1, 0, sizeof(szFilePath1));
    memset(&szFilePath2, 0, sizeof(szFilePath2));

    m_MonitoredCallsMutex.acquire();
    if (monitoredCalls.find(callIdentifier) != monitoredCalls.end())
    {
        rtp_routing_params* rp = monitoredCalls[callIdentifier];
        if (rp)
        {
            bFoundMonitoredCall = true;

            wsprintf(szFilePath1, "%s\\%d-%d.rtp", params.archiveFolder, callIdentifier, rp->destPort1);
            wsprintf(szFilePath2, "%s\\%d-%d.rtp", params.archiveFolder, callIdentifier, rp->destPort2);
        }
    }
    m_MonitoredCallsMutex.release();

    FlatMapWriter dataWriter;
    if (bFoundMonitoredCall)
    {
        if (!IsFileEmpty(szFilePath1))
            DeleteFile(szFilePath1);

        if (!IsFileEmpty(szFilePath2))
            DeleteFile(szFilePath2);

        // Ack Success
        dataWriter.insert(Params::IDENTIFIER, (int)callIdentifier);         // Call identifier
        dataWriter.insert(Params::RESULT_CODE, 0);                          // Result code
        logger->WriteLog(Log_Info, "[%d] <<<<< Send start record call now ack - SUCCESS for Call Identifier=%d", message.param(), callIdentifier);
    }
    else
    {
        // Ack failure
        dataWriter.insert(Params::IDENTIFIER, (int)callIdentifier);
        dataWriter.insert(Params::RESULT_CODE, 1);
        logger->WriteLog(Log_Info, "[%d] <<<<< Send start record call now ack - FAILURE for Call Identifier = %d", message.param(), callIdentifier);
    }
    ipcServer.Write(Msgs::START_RECORD_ACK, dataWriter, message.param() /* client session id */);
}

void PCapRuntime::OnStopRecord(PCapMessage& message)
{
    // Send back ack and current recorded data location
    FlatMapReader reader(message.metreosData());
    char* p = NULL;

    // Call Identifier
    u_int callIdentifier = 0;
    reader.find(Params::IDENTIFIER, &p);
    if (p)
        callIdentifier = *((u_int*)p);

    logger->WriteLog(Log_Info, "[%d] >>>>> Receive start record now request, Call Identifier = %d",
                    message.param(), callIdentifier);

    bool bFoundMonitoredCall = false;

    u_int dPort1, dPort2;

    m_MonitoredCallsMutex.acquire();
    if (monitoredCalls.find(callIdentifier) != monitoredCalls.end())
    {
        rtp_routing_params* rp = monitoredCalls[callIdentifier];
        if (rp)
        {
            bFoundMonitoredCall = true;
            dPort1 = rp->destPort1;
            dPort2 = rp->destPort2;
        }
    }
    m_MonitoredCallsMutex.release();

    FlatMapWriter dataWriter;
    char szFilePath[MAX_FILE_PATH];
    if (bFoundMonitoredCall)
    {
        MixRtpStreams(callIdentifier, dPort1, dPort2);
        memset(&szFilePath, 0, sizeof(szFilePath));
        wsprintf(szFilePath, "%s\\%d.au", params.archiveFolder, callIdentifier);
        
        // Ack Success
        dataWriter.insert(Params::IDENTIFIER, (int)callIdentifier);         // Call identifier
        dataWriter.insert(Params::RECORD_FILE, FlatMap::STRING, (int)ACE_OS::strlen(szFilePath)+1, szFilePath);
        dataWriter.insert(Params::RESULT_CODE, 0);                          // Result code
        logger->WriteLog(Log_Info, "[%d] <<<<< Send stop record call ack - SUCCESS for Call Identifier=%d", message.param(), callIdentifier);
    }
    else
    {
        // It is possible that the call is not there anymore.
        memset(&szFilePath, 0, sizeof(szFilePath));
        wsprintf(szFilePath, "%s\\%d.au", params.archiveFolder, callIdentifier);
        if (!IsFileEmpty(szFilePath))
        {
            // Ack Success
            dataWriter.insert(Params::IDENTIFIER, (int)callIdentifier);         // Call identifier
            dataWriter.insert(Params::RECORD_FILE, FlatMap::STRING, (int)ACE_OS::strlen(szFilePath)+1, szFilePath);
            dataWriter.insert(Params::RESULT_CODE, 0);                          // Result code
            logger->WriteLog(Log_Info, "[%d] <<<<< Send stop record call ack - SUCCESS for Call Identifier=%d", message.param(), callIdentifier);
        }
        else
        {
            // Ack failure
            dataWriter.insert(Params::IDENTIFIER, (int)callIdentifier);
            dataWriter.insert(Params::RESULT_CODE, 1);
            logger->WriteLog(Log_Info, "[%d] <<<<< Send stop record call ack - FAILURE for Call Identifier = %d", message.param(), callIdentifier);
        }
    }
    ipcServer.Write(Msgs::STOP_RECORD_ACK, dataWriter, message.param() /* client session id */);
}

void PCapRuntime::OnRecordConfigData(PCapMessage& message)
{
    // Update agent config data
}

void PCapRuntime::OnHeartbeat(PCapMessage& message)
{
    // Heartbeat
}

void PCapRuntime::UpdateCallStatus(u_int callIdentifier, u_int callState, u_int callType,
                                    char* callerDN, char* callerName,
                                    char* calleeDN, char* calleeName)
{
    FlatMapWriter dataWriter;

    dataWriter.insert(Params::IDENTIFIER, (int)callIdentifier);
    dataWriter.insert(Params::CALL_STATE, (int)callState);
    dataWriter.insert(Params::CALL_TYPE, (int)callType);

    char szDN[STATION_MAX_DN_SIZE];
    if (callerDN)
    {
        memset(&szDN, 0, sizeof(STATION_MAX_DN_SIZE));
        strcpy(szDN, callerDN);
        dataWriter.insert(Params::CALLER_DN, FlatMap::STRING, (int)ACE_OS::strlen(szDN)+1, szDN);
    }

    if (calleeDN)
    {
        memset(&szDN, 0, sizeof(STATION_MAX_DN_SIZE));
        strcpy(szDN, calleeDN);
        dataWriter.insert(Params::CALLEE_DN, FlatMap::STRING, (int)ACE_OS::strlen(szDN)+1, szDN);
    }

    char szName[STATION_MAX_NAME_SIZE];
    if (callerName)
    {
        memset(&szName, 0, sizeof(STATION_MAX_NAME_SIZE));
        strcpy(szName, callerName);
        dataWriter.insert(Params::CALLER_NAME, FlatMap::STRING, (int)ACE_OS::strlen(szName)+1, szName);
    }

    if (calleeName)
    {
        memset(&szName, 0, sizeof(STATION_MAX_NAME_SIZE));
        strcpy(szName, calleeName);
        dataWriter.insert(Params::CALLEE_NAME, FlatMap::STRING, (int)ACE_OS::strlen(szName)+1, szName);
    }

    WriteToIpc(Msgs::CALL_STATUS_UPDATE, dataWriter);    
}

void PCapRuntime::NotifyNewCall(u_int callIdentifier, u_int callType, 
                                   char* callerDN, char* callerName,
                                   char* calleeDN, char* calleeName)
{
    FlatMapWriter dataWriter;

    dataWriter.insert(Params::IDENTIFIER, (int)callIdentifier);
    
    dataWriter.insert(Params::CALL_TYPE, (int)callType);

    char szDN[STATION_MAX_DN_SIZE];
    if (callerDN)
    {
        memset(&szDN, 0, sizeof(STATION_MAX_DN_SIZE));
        strcpy(szDN, callerDN);
        dataWriter.insert(Params::CALLER_DN, FlatMap::STRING, (int)ACE_OS::strlen(szDN)+1, szDN);
    }

    if (calleeDN)
    {
        memset(&szDN, 0, sizeof(STATION_MAX_DN_SIZE));
        strcpy(szDN, calleeDN);
        dataWriter.insert(Params::CALLEE_DN, FlatMap::STRING, (int)ACE_OS::strlen(szDN)+1, szDN);
    }

    char szName[STATION_MAX_NAME_SIZE];
    if (callerName)
    {
        memset(&szName, 0, sizeof(STATION_MAX_NAME_SIZE));
        strcpy(szName, callerName);
        dataWriter.insert(Params::CALLER_NAME, FlatMap::STRING, (int)ACE_OS::strlen(szName)+1, szName);
    }

    if (calleeName)
    {
        memset(&szName, 0, sizeof(STATION_MAX_NAME_SIZE));
        strcpy(szName, calleeName);
        dataWriter.insert(Params::CALLEE_NAME, FlatMap::STRING, (int)ACE_OS::strlen(szName)+1, szName);
    }

    WriteToIpc(Msgs::NEW_CALL, dataWriter);    
}

void PCapRuntime::MixRtpStreams(u_int callIdentifier, u_int dport1, u_int dport2)
{
    char szFilePath[MAX_FILE_PATH], szFilePath1[MAX_FILE_PATH], szFilePath2[MAX_FILE_PATH];
    memset(&szFilePath, 0, sizeof(szFilePath));
    memset(&szFilePath1, 0, sizeof(szFilePath1));
    memset(&szFilePath2, 0, sizeof(szFilePath2));

    wsprintf(szFilePath, "%s\\%d.au", params.archiveFolder, callIdentifier);
    wsprintf(szFilePath1, "%s\\%d-%d.rtp", params.archiveFolder, callIdentifier, dport1);
    wsprintf(szFilePath2, "%s\\%d-%d.rtp", params.archiveFolder, callIdentifier, dport2);

    bool bForward = false, bBackward = false, bDuplex = false;

    if (!IsFileEmpty(szFilePath1))
        bForward = true;

    if (!IsFileEmpty(szFilePath2))
        bBackward = true;

    if (bForward && bBackward)
    {
        bForward = false;
        bBackward = false;
        bDuplex = true;
    }

    if (!bForward && !bBackward && !bDuplex)
        return;     // nothing to convert of mix

    ofstream outfile;
    outfile.open(szFilePath, ios::out | ios::binary); 
    // write AU file header
    char c[1];
	*c = (unsigned char)0x2e;         
    outfile.write(c, 1);
	*c = (unsigned char)0x73;
    outfile.write(c, 1);
	*c = (unsigned char)0x6e;
    outfile.write(c, 1);
	*c = (unsigned char)0x64;
    outfile.write(c, 1);
	// header offset == 24 bytes
	*c = (unsigned char)0x00; 
    outfile.write(c, 1);
    outfile.write(c, 1);
    outfile.write(c, 1);
	*c = (unsigned char)0x18; 
    outfile.write(c, 1);
	// total length, it is permited to set this to 0xffffffff 
	*c = (unsigned char)0xff; 
    outfile.write(c, 1);
    outfile.write(c, 1);
    outfile.write(c, 1);
    outfile.write(c, 1);
	// encoding format == 8 bit ulaw
	*c = (unsigned char)0x00; 
    outfile.write(c, 1);
    outfile.write(c, 1);
    outfile.write(c, 1);
	*c = (unsigned char)0x01; 
    outfile.write(c, 1);
	// sample rate == 8000 Hz
	*c = (unsigned char)0x00; 
    outfile.write(c, 1);
    outfile.write(c, 1);
	*c = (unsigned char)0x1f; 
    outfile.write(c, 1);
	*c = (unsigned char)0x40; 
    outfile.write(c, 1);
	// channels == 1
	*c = (unsigned char)0x00; 
    outfile.write(c, 1);
    outfile.write(c, 1);
    outfile.write(c, 1);
	*c = (unsigned char)0x01; 
    outfile.write(c, 1);

    /*
	WaveFile wout;
    wout.OpenWrite("c:\\wout.wav");
    wout.SetupFormat(8000, 16, 1);
    */

    // convert and write data
    if (bForward)
    {
        ifstream f_file(szFilePath1, ios::in|ios::binary);
        char c;
        while(f_file.get(c))
        {
            int n = ulaw2linear((unsigned char)c);
            c = (unsigned char)linear2ulaw(n);
            outfile.write(&c, 1);
        }
        f_file.close();
    }
    else if (bBackward)
    {
        ifstream f_file(szFilePath2, ios::in|ios::binary);
        char c;
        while(f_file.get(c))
        {
            int n = ulaw2linear((unsigned char)c);
            c = (unsigned char)linear2ulaw(n);
            outfile.write(&c, 1);
        }
        f_file.close();
    }
    else if (bDuplex)
    {
        ifstream f_file(szFilePath1, ios::in|ios::binary);
        ifstream b_file(szFilePath2, ios::in|ios::binary);
        char c1, c2;
#ifdef LIMITED_RECORD_TIME
        int count = 0;
        while(f_file.get(c1) && b_file.get(c2) && count<MAX_TRIAL_FILE_SIZE)
#else
        while(f_file.get(c1) && b_file.get(c2))
#endif
        {
            int n1 = ulaw2linear((unsigned char)c1);
            int n2 = ulaw2linear((unsigned char)c2);
            /*
            short ns = (n1+n2)/2;
            wout.WriteSample(ns);
            */
            char c = (unsigned char)linear2ulaw((n1+n2)/2);
            outfile.write(&c, 1);
#ifdef LIMITED_RECORD_TIME
            count++;
#endif
        }
        f_file.close();
        b_file.close();
        /*
        wout.Close();
        */

        DeleteFile(szFilePath1);
        DeleteFile(szFilePath2);
    }
    
    outfile.close();

    
}

bool PCapRuntime::IsFileEmpty(char* pFilePath)
{
    long l,m;

    ifstream file (pFilePath, ios::in|ios::binary);
    l = file.tellg();
    file.seekg (0, ios::end);
    m = file.tellg();
    file.close();

    return ((m-l) == 0);
}





