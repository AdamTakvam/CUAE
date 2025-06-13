// message.h

#ifndef _LOGCLIENT_MESSAGE_H_
#define _LOGCLIENT_MESSAGE_H_

#include "ace/Message_Block.h"

#include "Flatmap.h"

namespace Metreos
{

namespace LogClient
{

/////////////////////////////////////////////////////////////////////
typedef enum 
{
	Log_Off = 0,
	Log_Error = 1,
	Log_Warning = 2,
	Log_Info = 3,
	Log_Verbose = 4,
} LogLevel;

/////////////////////////////////////////////////////////////////////
class IntroductionMessage 
{
public:
	IntroductionMessage(const char* pName);
	~IntroductionMessage();

	void Create(FlatMapWriter& writer);

	static const int nameKey;

private:
	char* name; 
};

/////////////////////////////////////////////////////////////////////
class WriteMessage : public ACE_Message_Block
{
public:
	WriteMessage(const LogLevel level, const char* pMsg, const char* pTimeStamp);
	~WriteMessage();

	void Create(FlatMapWriter& writer);

	static const int logLevelKey;
	static const int messageKey;
	static const int timeStampKey;

private:
	LogLevel traceLevel;
	char* message;
	char* timeStamp;
};


/////////////////////////////////////////////////////////////////////
class RefreshMessage
{
public:
	RefreshMessage(const LogLevel level, const char* pMsg);
	~RefreshMessage();

	void Create(FlatMapWriter& writer);

	static const int logLevelKey;
	static const int messageKey;

private:
	LogLevel traceLevel;
	char* message;
};

/////////////////////////////////////////////////////////////////////
class DisposeMessage
{
public:
	DisposeMessage() {};
};

/////////////////////////////////////////////////////////////////////
class WriteResponse
{
public:
	WriteResponse() {};
};

/////////////////////////////////////////////////////////////////////
class IntroductionResponse
{
public:
	IntroductionResponse(const bool bSuccess);

	void Create(FlatMapWriter& writer);

	static const int successKey;

private:
	bool success;
};

}	// LogClient

}	// Metreos

#endif