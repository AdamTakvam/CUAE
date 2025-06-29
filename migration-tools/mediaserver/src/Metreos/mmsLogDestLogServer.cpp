//
// mmsLogDestLogServer.cpp
//
// Logger pluggable destination for Metreos Log Server
//
#include "StdAfx.h"
#include "mmsLogger.h"
#include "mmsLogDestination.h"
#include "logclient/logclient.h"

#ifdef MMS_ENABLE_MEMORY_LEAK_DETECTION
#define new MMS_MEMLEAK_NEW
#endif // MMS_ENABLE_MEMORY_LEAK_DETECTION


using namespace Metreos::LogClient;

MmsLogDestinationLogServer::MmsLogDestinationLogServer():
MmsLogDestination(MmsLogDestination::LOGSERVER, "LOGSERVER")
{   
  this->activate();
  m_isLoggedInline = m_isOpen = true; 
}

MmsLogDestinationLogServer::~MmsLogDestinationLogServer() 
{
  this->deactivate();                             
}

int MmsLogDestinationLogServer::printToDestination(const ACE_TCHAR* buf, const void* param)
{
  if  (NULL == buf) 
		return 0;

  int length = (int)param;                  // Length includes null term

  #ifdef MMS_POINTER_VALIDATION_ENABLED
  #ifdef MMS_WINPLATFORM 
  if  (IsBadReadPtr(buf, length? length : 16))
  {    
		ACE_OS::printf("LOGD >>> invalid pointer at printToDestination\n");
		return 0;
  }
  #endif
  #endif

	LogServerClient::Instance()->WriteLog(buf);

  return 1;
}

int MmsLogDestinationLogServer::activate()
{
	LogServerClient::Instance()->open("MediaServer");
	LogServerClient::Instance()->activate();
	return 1;
}

int MmsLogDestinationLogServer::deactivate()
{
	LogServerClient::Instance()->msg_queue()->deactivate();
	LogServerClient::Instance()->close();
	return 1;
}

