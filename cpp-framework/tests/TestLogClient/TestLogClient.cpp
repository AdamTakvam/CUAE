#include "stdafx.h"

#include "logclient/logclient.h"

using namespace Metreos::LogClient;

int main(int argc, char* argv[])
{    

	ACE_Time_Value tv10 (0, 10*1000);		// 10 ms

    LogServerClient* client = LogServerClient::Instance();

	client->LogToConsole(true);

	if (!client->open("Hello"))
		return 0;

	client->activate();

	ACE_OS::sleep(2);

    ACE_Time_Value t1 = ACE_OS::gettimeofday();

	for (int i=0; i<10000; i++)
	{
		client->WriteLog(Log_Info, "%d --- I am here", i);

		ACE_OS::sleep(tv10);
	}

    ACE_Time_Value t2 = ACE_OS::gettimeofday();

	ACE_Time_Value td = t2 - t1;

	ACE_DEBUG ((LM_DEBUG,
				ACE_TEXT ("Test run time = %d millisec\n"), td.msec()));

	client->msg_queue()->deactivate();
    client->close();

	ACE_DEBUG ((LM_DEBUG,
				ACE_TEXT ("Press any key to exit.")));
	char c;
	scanf(&c);

	return 0;
}

