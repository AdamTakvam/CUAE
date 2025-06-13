// LogClient.h

#ifndef _LOGCLIENT_LOGCLIENT_H_
#define _LOGCLIENT_LOGCLIENT_H_

#ifdef WIN32
#   pragma warning(disable:4786)
#   pragma warning(disable:4251)
#endif

#include "cpp-core.h"
#include "ace/Task.h"
#include "ace/Thread_Manager.h"
#include "ipc/FlatmapIpcclient.h"
#include "Flatmap.h"
#include "message.h"

using namespace Metreos::IPC;
using namespace Metreos::LogClient;

namespace Metreos
{

namespace LogClient
{

class CPPCORE_API LogServerClient : public FlatMapIpcClient, public ACE_Task<ACE_MT_SYNCH>
{
public:
	LogServerClient();
	~LogServerClient();

    static LogServerClient* Instance();

	// From ACE_Task
	virtual int open(void *args = 0);
	virtual int close (u_long flags = 0);
	virtual int svc(void);

	void LogFormattedMsg(LogLevel level, const char* msg);
	void WriteLog(LogLevel level, const char* fmt, ...);
	void WriteLog(const char* fmt, ...);
    void SetLogLevel(LogLevel l) { logLevel = l; }

	inline bool IsConnected() { return bConnected; }
	inline bool IsShutdown() { return bShutdown; }
	inline bool IsReady() { return bReady; }
	inline bool IsToConsole() { return bToConsole; }
	inline bool IsToFile() { return bToFile; }

	inline void LogToConsole(bool b) { bToConsole = b; }
	inline void LogToFile(bool b) { bToFile = b; }

protected:
	// From FlatMapIpcClient
    virtual void OnIncomingFlatMapMessage(const int messageType, const FlatMapReader& reader);
    virtual void OnSessionStop(int id);

    static ACE_THR_FUNC_RETURN ConnectToServerThreadFunc(void* data);
	bool ConnectToServer();
	bool SendIntroduction();
	bool SendLog(WriteMessage* wm);
	void AddLog(WriteMessage* wm);
	char* GetTimeStamp();

    static LogServerClient*  instance;

private:
	char* name;
	bool bToConsole;
	bool bToFile;
	bool bConnected;
	bool bReady;
	bool bShutdown;
    LogLevel logLevel;
    ACE_thread_t hThreadConnectToServer;

    ACE_Thread_Mutex consoleWriteMutex;

};

}	// LogClient

}	// Metreos


#endif
