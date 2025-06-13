// LogServerClientWrap.h

#pragma once
#include "ManWrap.h"
#ifdef _MANAGED
using namespace System::Runtime::InteropServices;
using namespace Metreos::LogServerClient;
#endif

class DLLEXPORT CMLogClientWrapInit 
{
public:
	CMLogClientWrapInit();
	~CMLogClientWrapInit();
};

class DLLEXPORT CMLogClient : public CMObject 
{
	DECLARE_WRAPPER(LogClient, Object);

public:
	enum TraceLevel 
	{
		Off = 0,
		Error = 1,
		Warning = 2,
		Info = 3,
		Verbose = 4,
	};

	CMLogClient(LPCTSTR name, TraceLevel traceLevel);
	void WriteLog(TraceLevel traceLevel, LPCTSTR msg);
};


extern "C" DLLEXPORT void InitLogClientWrapper();
extern "C" DLLEXPORT void UninitLogClientWrapper();
extern "C" DLLEXPORT void CreateLogClient(const char* name, int traceLevel);
extern "C" DLLEXPORT void WriteLog(int traceLevel, const char *msg);





