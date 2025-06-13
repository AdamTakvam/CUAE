// message.cpp

#include <stdlib.h>
#include <string.h>
#include "logclient/message.h"

using namespace Metreos;
using namespace Metreos::LogClient;

/////////////////////////////////////////////////////////////////////
const int IntroductionMessage::nameKey = 1000;
const int WriteMessage::logLevelKey = 1000;
const int WriteMessage::messageKey = 1001;
const int WriteMessage::timeStampKey = 1002;
const int RefreshMessage::logLevelKey = 1000;
const int RefreshMessage::messageKey = 1001;
const int IntroductionResponse::successKey = 1000;


/////////////////////////////////////////////////////////////////////
IntroductionMessage::IntroductionMessage(const char* pName)
{
	if (pName)
	{
		name = (char*)malloc(strlen(pName)+1);
		strcpy(name, pName);
	}
	else
	{
		strcpy(name, "Log Client");
	}
}

IntroductionMessage::~IntroductionMessage()
{
	if (name)
		free(name);
}

void IntroductionMessage::Create(FlatMapWriter& writer)
{
	writer.insert(nameKey, FlatMap::STRING, strlen(name)+1, name);
}

/////////////////////////////////////////////////////////////////////
WriteMessage::WriteMessage(const LogLevel level, const char* pMsg, const char* pTimeStamp)
{
	traceLevel = level;

	if (pMsg)
	{
		message = (char*)malloc(strlen(pMsg)+1);
		strcpy(message, pMsg);
	}
	else
		message = 0;

	if (pTimeStamp)
	{
		timeStamp = (char*)malloc(strlen(pTimeStamp)+1);
		strcpy(timeStamp, pTimeStamp);
	}
	else
		timeStamp = 0;
}

WriteMessage::~WriteMessage()
{
	if (message)
		free(message);

	if (timeStamp)
		free(timeStamp);
}

void WriteMessage::Create(FlatMapWriter& writer)
{
	char buffer[8];

	_itoa(traceLevel, buffer, 10);
	writer.insert(logLevelKey, FlatMap::STRING, strlen(buffer)+1, buffer);
	if (message)
		writer.insert(messageKey, FlatMap::STRING, strlen(message)+1, message);
	if (timeStamp)
		writer.insert(timeStampKey, FlatMap::STRING, strlen(timeStamp)+1, timeStamp);
}

/////////////////////////////////////////////////////////////////////
RefreshMessage::RefreshMessage(const LogLevel level, const char* pMsg)
{
	traceLevel = level;
	if (message)
	{
		message = (char*)malloc(strlen(pMsg)+1);
		strcpy(message, pMsg);
	}
	else
		message = 0;
}

RefreshMessage::~RefreshMessage()
{
	if (message)
		free(message);
}

void RefreshMessage::Create(FlatMapWriter& writer)
{
	char buffer[8];

	_itoa(traceLevel, buffer, 10);
	writer.insert(logLevelKey, FlatMap::STRING, strlen(buffer)+1, buffer);
	if (message)
		writer.insert(messageKey, FlatMap::STRING, strlen(message)+1, message);
}

/////////////////////////////////////////////////////////////////////
IntroductionResponse::IntroductionResponse(const bool bSuccess)
{
	success = bSuccess;
}

void IntroductionResponse::Create(FlatMapWriter& writer)
{
	if (success)
		writer.insert(successKey, FlatMap::STRING, 2, "1");
	else
		writer.insert(successKey, FlatMap::STRING, 2, "0");
}


