#include "stdafx.h"
#include <sstream>
#include "MtSipLogger.h"
#include "logclient/logclient.h"
#include "MtSipStackRuntime.h"

using namespace Metreos::Sip;
using namespace std;

MtSipLogger::~MtSipLogger()
{
}

bool MtSipLogger::operator()(	Log::Level level,
								const Subsystem& subsystem, 
								const Data& appName,
								const char* file,
								int line,
								const Data& message,
								const Data& messageWithHeaders)
{
	//first translate the log level to metreos log level
	//then format the message
	ostringstream os;
	if (level >= Log::Debug)
		os <<subsystem <<" " <<appName <<" " <<file <<":" <<line <<" ";

	os <<message; 

//	if (level >= Log::Debug)
//		os <<endl <<messageWithHeaders;


	LogLevel l = LogClient::Log_Warning;	//default to warning
	switch(level)
	{
	case Log::None:
		l = LogClient::Log_Off;
		break;

	case Log::Crit:
	case Log::Err:
		l = LogClient::Log_Error;
		break;

	case Log::Warning:
		l = LogClient::Log_Warning;
		break;

	case Log::Info:
		l = LogClient::Log_Info;
		break;

	case Log::Debug:
		l = LogClient::Log_Verbose;
		break;
	}

	LogServerClient::Instance()->LogFormattedMsg(l, os.str().c_str());

	return true;
}