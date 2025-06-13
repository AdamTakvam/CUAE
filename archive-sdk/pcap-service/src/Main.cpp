// Main.cpp

/**
 * Entry point for pcap-service, also set up Windows Exception trap for error handling.
 * The program can run in either "console" or "service" mode, the following command line 
 * options are valid:
 * -s: Run as Windows service.
 * -d: Run as debug mode.
 * -a: Auto mode to enable RTP packet routing or recording automatically when a call gets connected.
 * -m: Specify what network interface to monitor
 * -f: Write RTP payload to file, argument is "archive folder".
 * -i: Route RTP payload to another IP address, argument is "destination IP address".
 * -c: config file, argument is the full path of config file
 * -N: List available network interfaces then exit
 */ 

#include "stdafx.h"

#ifdef WIN32
#ifdef PCAP_MEM_LEAK_DETECTION
#   define DEBUG_NEW new(_NORMAL_BLOCK, __FILE__, __LINE__)
#   define new DEBUG_NEW 
#endif
#endif

#include "PCapCommon.h"
#include "PCapRuntime.h"
#include "win32/PCapService.h"

#include "ace/Get_Opt.h"
#include "ace/Trace.h"

#include "pcap.h"

#include "win32/Exception.h"

#include "logclient/logclient.h"

using namespace Metreos::Win32;
using namespace Metreos::PCap;
using namespace Metreos::PCap::Win32;
using namespace Metreos::LogClient;

#define DEFAULT_CONFIG_FILE_PATH        "C:/METREOS/PCAPSERVICE/PCAP-SERVICE.CONFIG"
#define DEFAULT_CONFIG_FILE             "PCAP-SERVICE.CONFIG"
#define ETHERNET_INTERFACE_ID_TOKEN     "ETHERNET_INTERFACE_ID="
#define MAX_CONFIG_LINE_SIZE            255
#define DEFAULT_ARCHIVE_BASE_PATH       "archive"


// Declare the required PProcess class.
PDECLARE_PROCESS(MetreosPProcess, PProcess, "Metreos", "MCE PCap Service", 2, 0, ReleaseCode, 1)

static void SetExceptionTrap();

static u_int GetMonNicFromConfig(char* pConfigFilePath);

static void SetArchiveFolder(runtime_params& params);

static u_int GetMonNicFromDefaultConfig();

LogServerClient* logger = LogServerClient::Instance();

/**
 * Executable entry point.
 *
 * Arguments:
 *    --service - Start in Win32 service mode.
 *    --debug   - Break after process start for debugger (only for console mode).
 */
int main(int argc, char* argv[])
{
#ifdef PCAP_MEM_LEAK_DETECTION
    //_CrtSetBreakAlloc(15565);
    _CrtSetDbgFlag(_CRTDBG_ALLOC_MEM_DF|_CRTDBG_LEAK_CHECK_DF);
#endif

    // Initialize WinSock library for JRTPLIB
#ifdef WIN32
	WSADATA wsaData;
	WORD wVersionRequested = MAKEWORD(2, 2);
	WSAStartup(wVersionRequested, &wsaData);
#endif

    MetreosPProcess* process = new MetreosPProcess;
    
    bool serviceEnabled     = false;
    bool debugBreakEnabled  = false;
    bool listNic            = false;

#ifndef PCAP_MEM_LEAK_DETECTION
    // Set Win32 exception handler to generate a mini-dump if we crash.
    SetExceptionTrap();
#endif
    
    ACE_Trace::stop_tracing();

    ACE_Log_Msg::instance()->clr_flags(ACE_Log_Msg::LOGGER | ACE_Log_Msg::OSTREAM | ACE_Log_Msg::MSG_CALLBACK | ACE_Log_Msg::SYSLOG);
    ACE_Log_Msg::instance()->set_flags(ACE_Log_Msg::STDERR); 

    // Determine what diagnostic info is written to the console.
    ACE_LOG_MSG->priority_mask(
        LM_INFO|        // Messages that contain information normally of use only when debugging a program
        LM_NOTICE|      // Conditions that are not error conditions but that may require special handling
        LM_WARNING|     // Warning messages
        LM_ERROR|       // Error messages
        LM_CRITICAL|    // Critical conditions, such as hard device errors
        LM_ALERT|       // A condition that should be corrected immediately, such as a corrupted system database
        LM_EMERGENCY,   // A panic condition, normally broadcast to all users
        ACE_Log_Msg::PROCESS);

    ACE_Get_Opt getOpt(argc, argv, "sdaNfi:m:c:");

    getOpt.long_option("service", 's');
    getOpt.long_option("debug",   'd');
    getOpt.long_option("active",  'a');
    getOpt.long_option("rtp2file",  'f');
    getOpt.long_option("rtp2ip",  'i');
    getOpt.long_option("monnic",  'm');
    getOpt.long_option("config", 'c');

    getOpt.long_option("nics",  'N');

    runtime_params params;
    memset(&params, 0, sizeof(runtime_params));

    SetArchiveFolder(params);
    params.monnic = GetMonNicFromDefaultConfig();

    char c;
    char *p = NULL;
    while((c = getOpt()) != EOF)
    {
        switch(c)
        {
            case 's': 
                serviceEnabled = true;
                break;

            case 'd':
                debugBreakEnabled = true;
                break;

            case 'a':
                params.activeMode = true;
                break;

            case 'f':
                params.sendRTPToFile = true;
                break;

            case 'i':
                params.sendRTPToIp = true;
                p = getOpt.optarg;
                if (p != NULL)
                    strcpy(params.destIpAddress, p);
                break;

            case 'm':
                p = getOpt.optarg;
                if (p != NULL)
                    params.monnic = atoi(p);
                break;

            case 'c':
                p = getOpt.optarg;
                if (p != NULL)
                {
                    u_int monnic = GetMonNicFromConfig(p);
                    if (monnic > 0)
                        params.monnic = monnic;
                }
                break;

            case 'N':
                listNic = true;
                break;
        }
    }

	// Log to console if running in console mode.
	if (!serviceEnabled)
		LogServerClient::Instance()->LogToConsole(true);
    LogServerClient::Instance()->open("PCap");
    LogServerClient::Instance()->activate();

	// Waiting for message queue to get ready.
	ACE_OS::sleep(1);

    logger->WriteLog(Log_Info, "Default archive folder is %s", params.archiveFolder);
    logger->WriteLog(Log_Info, "Default monitoring NIC card index is %d", params.monnic);

    // List available network interfaces.
    // The reason why we need to do this every time is because WinPCap fails to 
    // detect any interfaces in worker thread on some machines.
    pcap_if_t *alldevs;
    char errbuf[PCAP_ERRBUF_SIZE];

    if (pcap_findalldevs(&alldevs, errbuf) == -1)
    {
        logger->WriteLog(Log_Error, "Unable to find network interfaces - %s.", errbuf);
    }
    else
    {
        int i = 0;
        pcap_if_t *d;
        for(d=alldevs; d; d=d->next)
        {
            if (d->description)
                logger->WriteLog(Log_Info, "%d. %s, %s", ++i, d->name, d->description);
            else
                logger->WriteLog(Log_Info, "%d. %s", ++i, d->name);
        }
        
        if(i==0)
        {
            logger->WriteLog(Log_Error, "No network interface found, make sure the software is properly installed.");
        }
        // We don't need any more the device list. Free it
        pcap_freealldevs(alldevs);
    }

    if(serviceEnabled == false)
    {
        if(debugBreakEnabled == true)
        {
            char d;
            logger->WriteLog(Log_Info, "Debug break. Press any key.");
            std::cin.get(d);
        }

        // If user only wants to list interfaces then return right away
        if (!listNic)
        {
            logger->WriteLog(Log_Info, "Activating Runtime");
            Metreos::PCap::PCapRuntime runtime;
            runtime.msg_queue()->low_water_mark(ACE_QUEUE_LWN);
            runtime.msg_queue()->high_water_mark(ACE_QUEUE_HWN);
            runtime.SetParams(params);
            runtime.open();
            runtime.activate();

            if(runtime.Startup() == -1)
            {
                logger->WriteLog(Log_Error, "Runtime failed to start.  Exiting.");
                return 0;
            }
            
            logger->WriteLog(Log_Info, "Runtime activated in CONSOLE mode.");
            logger->WriteLog(Log_Info, "Press 'q' to quit.");

            char c;
            std::cin.get(c);
            while(c != 'q' && c != 'Q')
            {
                logger->WriteLog(Log_Info, "Press 'q' to quit.");
                std::cin.get(c);
            }

            if(runtime.Shutdown() == -1)
            {
                logger->WriteLog(Log_Warning, "Runtime failed to shutdown cleanly.  Exiting.");
            }

            runtime.msg_queue()->deactivate();
            runtime.close();

            logger->WriteLog(Log_Info, "Runtime De-Activated.");
        }
    }
    else
    {
        // if there is no ethernet interface id defined, try to find it in default config file.
        if (params.monnic == 0)
        {
            u_int monnic = GetMonNicFromConfig(DEFAULT_CONFIG_FILE_PATH);
            params.monnic = monnic > 0 ? monnic : 1;
        }
        PCapService service;
        service.SetParams(params);
        Metreos::Win32::ServiceRuntimeBase::Run(service);
    }

    LogServerClient::Instance()->msg_queue()->deactivate();
    LogServerClient::Instance()->close();

    delete process;

#ifdef WIN32
	WSACleanup();
#endif

    return 0;
}


// Set exception trap if Windows platform. Catches otherwise elusive crashes.
void SetExceptionTrap()
{
#ifdef WIN32
    UnhandledExcpTrap* excpHandler = UnhandledExcpTrap::instance();

    if (excpHandler != NULL)
    {
        bool ok;

        ok = excpHandler->setFilter(UnhandledExcpTrap::CreateNewDumpFile);

// To test mini-dump facility, force exception
#if 0
        if (ok)
        {
            char *dummy;

            dummy = NULL;
            *dummy = 'b';
        }
#endif

    }

#endif // WIN32 
}


u_int GetMonNicFromConfig(char* pConfigFilePath)
{
    u_int monnic = 0;
    char line [MAX_CONFIG_LINE_SIZE+1];          // Config file line buffer


    FILE* f = fopen(pConfigFilePath, "r");
    if (!f) 
        return monnic;

    while(1)                                  // While not eof on config file ...
    {
        if  (NULL == ACE_OS::fgets(line, MAX_CONFIG_LINE_SIZE, f)) 
            break;  

        char* p = ACE_OS::strstr(line, ETHERNET_INTERFACE_ID_TOKEN);
        if (p!=NULL)
        {
            // To simpliy things, we do not allow any space after =.
            p += 22; //sizeof(ETHERNET_INTERFACE_ID_TOKEN)
            if (p!=NULL)
                monnic = ACE_OS::atoi(p);
            break;
        }

    }

    return monnic;
}

static u_int GetMonNicFromDefaultConfig()
{
#ifdef WIN32
	char path[MAX_PATH];
	memset(&path, 0, sizeof(path));
    if (GetModuleFileName(NULL, path, sizeof(path)))
    {                                         // Check home directory
        char *p = _tcsrchr(path,'\\');
        if (p)
        {
            _tcscpy(p+1, DEFAULT_CONFIG_FILE);
			DWORD attr = GetFileAttributes(path); 
			if(0xFFFFFFFF != attr) 
			{ 
				// config file existed
                return GetMonNicFromConfig(path);
			} 
		}
	}

    return 0;
#endif
}

static void SetArchiveFolder(runtime_params& params)
{
#ifdef WIN32
	char path[MAX_PATH];
	memset(&path, 0, sizeof(path));
    if (GetModuleFileName(NULL, path, sizeof(path)))
    {                                         // Check home directory
        char *p = _tcsrchr(path,'\\');
        if (p)
        {
            _tcscpy(p+1, DEFAULT_ARCHIVE_BASE_PATH);
			DWORD attr = GetFileAttributes(path); 
			if((0xFFFFFFFF != attr) && (0 != (FILE_ATTRIBUTE_DIRECTORY & attr))) 
			{ 
				// directory existed
				ACE_OS::sprintf(params.archiveFolder, "%s", path);
			} 
			else 
			{ 
				if (CreateDirectory(path, NULL))
				{
					ACE_OS::sprintf(params.archiveFolder, "%s", path);
				}
		    }
		}
	}
#endif
}


///////////////////////////////////////
//
// MetreosPProcess::Main()
//
// MetreosPProcess class is delcared above with
// the PDECLARE_PROCESS macro. 
//
void MetreosPProcess::Main()
{
    // Do nothing
}