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


#include "ace/Get_Opt.h"
#include "ace/Trace.h"

#include "win32/Exception.h"

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

int main(int argc, char** argv)
{
	ostringstream os;
	os<<"testtsetsets"<<flush;
	SipStack *m_pSipStack = new SipStack();
	//create the thread for sip stack
	StackThread *m_pStackThread = new StackThread(*m_pSipStack);
	//add transport??
	m_pSipStack->addTransport(UDP, 12115);
	m_pStackThread->run();

	for(int i = 0; i < 100; i++)
		Sleep(1000);

	return 0;
}
