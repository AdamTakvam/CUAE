// logclient.cpp

#include <iostream>
#include "logclient/logclient.h"
#include "logclient/message.h"
#include "ace/message_queue_t.h"

using namespace Metreos;
using namespace Metreos::LogClient;

/////////////////////////////////////////////////////////////////////
#define DEFAULT_WAIT		1		// sec
#define DEFAULT_WATERMARK	1000    // log messages
#define DEFAULT_PORT		8400
#define DEFAULT_SERVER		"127.0.0.1"

#define MESSAGE_WRITEREQUEST			1001
#define MESSAGE_DISPOSEREQUEST			1002
#define MESSAGE_REFRESHREQUEST			1003
#define MESSAGE_INTRODUCTIONREQUEST		1004
#define MESSAGE_INTRODUCTIONRESPONSE	1005
#define MESSAGE_WRITERESPONSE			1006
#define MESSAGE_FLUSHREQUEST			1007

/////////////////////////////////////////////////////////////////////
LogServerClient* LogServerClient::instance = 0;

/////////////////////////////////////////////////////////////////////
LogServerClient::LogServerClient()
{
	bToConsole = false;
	bToFile = true;
	bConnected = false;
	bReady = false;
	bShutdown = false;
	hThreadConnectToServer = NULL;
	name = NULL;
    logLevel = Log_Info;
}


LogServerClient::~LogServerClient()
{
	if (!IsShutdown())
		close();

	if (name)
		free(name);
}

LogServerClient* LogServerClient::Instance()
{
    if(instance == 0)
    {
        instance = new LogServerClient();
    }

    return instance;
}

int LogServerClient::svc(void)
{
	ACE_Time_Value tv100 (0, 100*1000);		// 100 ms 
	ACE_Time_Value tv5	(0, 5*1000);		// 5 ms

	ACE_Message_Block* msg = 0;
	while(!IsShutdown())
	{
		if (IsReady())
		{
			if (msg_queue()->peek_dequeue_head(msg) != -1)
			{
				WriteMessage* wm = dynamic_cast<WriteMessage*>(msg);
				ACE_ASSERT(wm != 0);

				bool bSent = SendLog(wm);

				if (!bSent)
				{
					// server may not be ready, sleep 100 ms
					ACE_OS::sleep(tv100);
					continue;
				}

				// dequeue it
				getq(msg);
				wm = dynamic_cast<WriteMessage*>(msg);
				if (wm)
				{
					delete wm;
					wm = 0;
				}
			}
			else
			{
				ACE_OS::sleep(tv5);
			}
		}
		else
		{
			// nothing in queue yet, sleep 10 ms
			ACE_OS::sleep(tv100);
		}
	}

    return 0;
}

int LogServerClient::open(void *args)
{
	name = (char*)malloc(strlen((const char*)args)+1);
	strcpy(name, (const char*)args);
	
    if (-1 == ACE_Thread_Manager::instance()->spawn(
													ConnectToServerThreadFunc, this, THR_NEW_LWP | THR_JOINABLE,
													&hThreadConnectToServer))
	{
		return 0;
	}

	return 1;
}

int LogServerClient::close (u_long flags)
{
	bShutdown = true;
	bReady = false;

	if (IsConnected())
	{
		this->Disconnect();
		bConnected = false;
	}


    ACE_Thread_Manager *tm = ACE_Thread_Manager::instance();
    ACE_ASSERT(tm);

    if(tm->testresume(hThreadConnectToServer) == 1)
    {
        tm->join(hThreadConnectToServer);
    }

	return 1;
}

void LogServerClient::LogFormattedMsg(LogLevel level, const char* msg)
{
    if (logLevel < level)       // make sure the log level matches what we want.
        return;

	if (IsToConsole())
	{
        consoleWriteMutex.acquire();
		std::cout << msg << std::endl;
        consoleWriteMutex.release();
	}

	if (!IsToFile() || !IsReady())
	{
		return;
	}

	ACE_TCHAR* timeStamp = GetTimeStamp();

	WriteMessage* wm = 0;
	
	wm = new WriteMessage(level, msg, timeStamp);

	AddLog(wm);

	if (timeStamp)
		delete timeStamp;
}

void LogServerClient::WriteLog(LogLevel level, const char* fmt, ...)
{
    if (logLevel < level)       // make sure the log level matches what we want.
        return;

    ACE_TCHAR* msg = new ACE_TCHAR[1024];

    va_list ap;
    va_start(ap, fmt);
    ACE_OS::vsprintf(msg, fmt, ap);
    va_end(ap);

	if (IsToConsole())
	{
        consoleWriteMutex.acquire();
		std::cout << msg << std::endl;
        consoleWriteMutex.release();
	}

	if (!IsToFile() || !IsReady())
	{
		if (msg)
			delete msg;
		return;
	}

	ACE_TCHAR* timeStamp = GetTimeStamp();

	WriteMessage* wm = 0;
	
	wm = new WriteMessage(level, msg, timeStamp);

	AddLog(wm);

	if (msg)
		delete msg;

	if (timeStamp)
		delete timeStamp;
}

void LogServerClient::WriteLog(const char* fmt, ...)
{
    // This is info level log, if log level is Warning, Error, or Off then return.
    if (logLevel < Log_Info)
        return;

    ACE_TCHAR* msg = new ACE_TCHAR[1024];

    va_list ap;
    va_start(ap, fmt);
    ACE_OS::vsprintf(msg, fmt, ap);
    va_end(ap);

	if (IsToConsole())
	{
		consoleWriteMutex.acquire();
		std::cout << msg << std::endl;
        consoleWriteMutex.release();
	}

	if (!IsToFile() || !IsReady())
	{
		if (msg)
			delete msg;

		return;
	}

	ACE_TCHAR* timeStamp = GetTimeStamp();

	WriteMessage* wm = 0;
	
	wm = new WriteMessage(Log_Info, msg, timeStamp);

	AddLog(wm);

	if (msg)
		delete msg;

	if (timeStamp)
		delete timeStamp;
}

void LogServerClient::AddLog(WriteMessage* wm)
{
	if (msg_queue()->message_count() >= DEFAULT_WATERMARK)
	{
		ACE_Message_Block* msg = 0;
		getq(msg);
		if (msg != 0)
		{
			WriteMessage* wmx = NULL;
			wmx = dynamic_cast<WriteMessage*>(msg);
			if (wmx)
				delete wmx;
		}
	}

	putq(wm);
}

bool LogServerClient::ConnectToServer()
{
	bConnected = this->Connect(DEFAULT_SERVER, DEFAULT_PORT);

	return bConnected;
}

bool LogServerClient::SendIntroduction()
{
	bool ret = false;

	IntroductionMessage* im = new IntroductionMessage(name);
	FlatMapWriter writer;
	im->Create(writer);

	ret = this->Write(MESSAGE_INTRODUCTIONREQUEST, writer);

	if (im)
		delete im;

	return ret;
}

bool LogServerClient::SendLog(WriteMessage* wm)
{
	bool ret = false;

	if (!IsReady())
		return ret;

	FlatMapWriter writer;
	wm->Create(writer);

    ret = this->Write(MESSAGE_WRITEREQUEST, writer);

	return ret;
}

void LogServerClient::OnIncomingFlatMapMessage(const int messageType, const FlatMapReader& reader)
{
	switch(messageType)
	{
		case MESSAGE_INTRODUCTIONRESPONSE:
			//bReady = reader.find(IntroductionResponse::successKey, 1) > 0 ? true : false;
			bReady = true;
			break;

		case MESSAGE_WRITERESPONSE:
			break;
	}
}

void LogServerClient::OnSessionStop(int id)
{
	bReady = false;
	bConnected = false;
}

ACE_THR_FUNC_RETURN LogServerClient::ConnectToServerThreadFunc(void* data)
{
    LogServerClient* client = static_cast<LogServerClient*>(data);
    ACE_ASSERT(client != NULL);

    while(!client->IsShutdown())
    {
		while (!client->IsShutdown() && !client->IsConnected())
		{
			if (client->ConnectToServer())
				break;

			ACE_OS::sleep(DEFAULT_WAIT);
		}


		while (!client->IsShutdown() && client->IsConnected() && !client->IsReady())
		{
			client->SendIntroduction();

			ACE_OS::sleep(DEFAULT_WAIT);
		}

		ACE_OS::sleep(DEFAULT_WAIT);
	}

    return 0;
}

char* LogServerClient::GetTimeStamp()
{
    static char* masktimestamp = ACE_LIB_TEXT("%s %s.%03ld");

    ACE_TCHAR* timestamp = new ACE_TCHAR[32];	// 0123456789012345678901234 
    int  length = 0;							// Oct 18 14:25:36.000 2003<nul>

    ACE_Time_Value datetime = ACE_OS::gettimeofday(); 

    time_t now = datetime.sec();
    ACE_TCHAR ctp[32];
    ACE_OS::ctime_r(&now, ctp, sizeof ctp);

    ctp[19] = '\0';     // terminate after time
    ctp[24] = '\0';     // terminate after date

    ACE_OS::sprintf(timestamp, masktimestamp, ctp+20, ctp+4, (datetime.usec())/1000);    

	return timestamp;
}