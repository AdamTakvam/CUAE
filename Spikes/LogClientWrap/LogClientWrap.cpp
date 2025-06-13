#include "stdafx.h"
#include <_vcclrit.h>
#include "LogClientWrap.h"

/////////////////////////////////////////////////////////////////////
static CMLogClient* pLogClient = NULL;

/////////////////////////////////////////////////////////////////////
// Special initialization/termination required for managed DLL.
// The __crt fns are in _vcclrit.h.
//
CMLogClientWrapInit::CMLogClientWrapInit()
{
   __crt_dll_initialize();
}

CMLogClientWrapInit::~CMLogClientWrapInit()
{
   __crt_dll_terminate();
}

/////////////////////////////////////////////////////////////////////
IMPLEMENT_WRAPPER(LogClient, Object)

/////////////////////////////////////////////////////////////////////
// Wrap construction of LogClient object in try/catch to wrap any exception before
// re-throwing it. Regex throws an ArgumentException if the regex is
// ill-formed.
//
LogClient* NewLogClient(CString name, CMLogClient::TraceLevel traceLevel)
{
	try {
		return new LogClient(name, traceLevel);
	} catch (Exception* e) {
		throw CMException(e);
	}
}

/////////////////////////////////////////////////////////////////////

CMLogClient::CMLogClient(LPCTSTR name, TraceLevel traceLevel) : CMObject(NewLogClient(name, traceLevel))
{
}

/////////////////////////////////////////////////////////////////////
void CMLogClient::WriteLog(TraceLevel traceLevel, LPCTSTR msg)
{
	return (*this)->WriteLog(traceLevel, msg);
}

/////////////////////////////////////////////////////////////////////
DLLEXPORT void InitLogClientWrapper()
{
	__crt_dll_initialize();
}

/////////////////////////////////////////////////////////////////////
DLLEXPORT void UninitLogClientWrapper()
{
	if (pLogClient != NULL)
		delete pLogClient;

	__crt_dll_terminate();
}

/////////////////////////////////////////////////////////////////////
DLLEXPORT void CreateLogClient(const char* name, int traceLevel)
{
	if (pLogClient == NULL)
		pLogClient = new CMLogClient(CString(name), (CMLogClient::TraceLevel)traceLevel);
}

/////////////////////////////////////////////////////////////////////
DLLEXPORT void WriteLog(int traceLevel, const char *msg)
{
	if (pLogClient==NULL)
		return;

	pLogClient->WriteLog((CMLogClient::TraceLevel)traceLevel, CString(msg));
}



