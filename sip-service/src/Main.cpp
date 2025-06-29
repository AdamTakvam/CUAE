/**
 * $Id: Main.cpp 15735 2005-11-20 21:26:59Z marascio $
 */

#include "stdafx.h"

#ifdef WIN32
#ifdef SIP_MEM_LEAK_DETECTION
#   define DEBUG_NEW new(_NORMAL_BLOCK, __FILE__, __LINE__)
#   define new DEBUG_NEW 
#endif
#endif

#include "SipCommon.h"

#include "ace/Get_Opt.h"
#include "ace/Trace.h"

#include "win32/Exception.h"

#include "MtSipStackRuntime.h"
#include "win32/MetreosSipStackService.h"

#include "logclient/logclient.h"
#include "stack/SipStack.hxx"
#include "stack/StackThread.hxx"
#include "dum/DialogUsageManager.hxx"
#include "dum/DumThread.hxx"
#include "stack/SdpContents.hxx"

using namespace Metreos::Win32;
using namespace Metreos::Sip;
using namespace Metreos::Sip::Win32;
using namespace Metreos::LogClient;

static void SetExceptionTrap();

/**
 * Executable entry point.
 *
 * Arguments:
 *    --service - Start in Win32 service mode.
 *    --debug   - Break after process start for debugger (only for console mode).
 */
int main(int argc, char* argv[])
{
#ifdef SIP_MEM_LEAK_DETECTION
    //_CrtSetBreakAlloc(15565);
    _CrtSetDbgFlag(_CRTDBG_ALLOC_MEM_DF|_CRTDBG_LEAK_CHECK_DF);
#endif

    bool serviceEnabled     = false;
    bool debugBreakEnabled  = false;

#ifndef SIP_MEM_LEAK_DETECTION
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

    ACE_Get_Opt getOpt(argc, argv, "sd");

    getOpt.long_option("service", 's');
    getOpt.long_option("debug",   'd');

    char c;
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
        }
    }

	// Log to console if running in console mode.
	if (!serviceEnabled)
		LogServerClient::Instance()->LogToConsole(true);
    LogServerClient::Instance()->open("Sip");
    LogServerClient::Instance()->activate();
	LogServerClient::Instance()->SetLogLevel(Log_Verbose);

	// Waiting for message queue to get ready.
	ACE_OS::sleep(1);

    if(serviceEnabled == false)
    {
        if(debugBreakEnabled == true)
        {
            char d;
            SIPLOG((Log_Info, "Debug break. Press any key."));
            std::cin.get(d);
        }

        SIPLOG((Log_Info, "Activating Runtime"));
        Metreos::Sip::MtSipStackRuntime runtime;
        runtime.open();
        runtime.activate();
        if(runtime.PostStartupMsg() == -1)
        {
            SIPLOG((Log_Error, "Runtime failed to start.  Exiting."));
            return 0;
        }
        
        SIPLOG((Log_Info, "Runtime activated in CONSOLE mode."));
        SIPLOG((Log_Info, "Press 'q' to quit."));

        char c;
        std::cin.get(c);
        while(c != 'q' && c != 'Q')
        {
            SIPLOG((Log_Info, "Press 'q' to quit."));
            std::cin.get(c);
        }

        if(runtime.PostShutdownMsg() == -1)
            SIPLOG((Log_Warning, "Runtime failed to shutdown cleanly.  Exiting."));

        runtime.msg_queue()->deactivate();
        runtime.close();

        SIPLOG((Log_Info, "Runtime De-Activated."));
    }
    else
    {
        MetreosSipStackService service;
        Metreos::Win32::ServiceRuntimeBase::Run(service);
    }

    LogServerClient::Instance()->msg_queue()->deactivate();
    LogServerClient::Instance()->close();

	// Waiting for message queue to shutdown.
	ACE_OS::sleep(3);
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