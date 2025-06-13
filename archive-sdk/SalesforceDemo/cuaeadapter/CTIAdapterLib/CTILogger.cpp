#include "StdAfx.h"
#include ".\ctilogger.h"

#define MAX_LOGLENGTH 2048

CCTILogger* CCTILogger::theLogger = NULL;

CCTILogger::CCTILogger(int nLogType)
:m_nLogType(nLogType),
m_nLogLevel(LOGLEVEL_LOW),
m_pLogFileStream(NULL),
m_nMaxLogFileSize(500000),
m_nMaxLogFiles(10)
{
	LoadLogInfoFromRegistry();
}

CCTILogger::~CCTILogger(void)
{
	if (m_pLogFileStream) {
		fclose(m_pLogFileStream);
	}
}

void CCTILogger::CreateLogger(int nLogType) 
{
	theLogger = new CCTILogger(nLogType);
}

void CCTILogger::DestroyLogger() 
{
	delete theLogger;
	theLogger = NULL;
}

CCTILogger* CCTILogger::GetLogger()
{
	ATLASSERT(theLogger!=NULL);
	return theLogger;
}

void CCTILogger::LoadLogInfoFromRegistry()
{
	//If there are no values currently in the registry, then we'll save the defaults when we're done here.
	bool bSave = false;
	HKEY key;

	wchar_t sPathBuffer[MAX_PATH];

	if (RegOpenKeyEx(HKEY_CURRENT_USER,L"Software\\Salesforce.com\\CTI",0,KEY_QUERY_VALUE,&key)==ERROR_SUCCESS) {
		DWORD dwType;
		DWORD dwLogLevel;
		DWORD dwMaxLogSize;
		//The buffer size is set by RegQueryValueEx, so we have to set it back after each call.
		DWORD dwBufferSize = sizeof(DWORD);
		if (RegQueryValueEx(key,L"LogLevel",NULL,&dwType,(LPBYTE)&dwLogLevel,&dwBufferSize)==ERROR_SUCCESS) 
		{
			if (dwLogLevel>=LOGLEVEL_LOW && dwLogLevel<=LOGLEVEL_HIGH)
			{
				m_nLogLevel = dwLogLevel;
			}
		} else {
			m_nLogLevel = LOGLEVEL_LOW;
			bSave = true;
		}

		dwBufferSize = sizeof(DWORD);
		if (RegQueryValueEx(key,L"MaxLogFileSize",NULL,&dwType,(LPBYTE)&dwMaxLogSize,&dwBufferSize)==ERROR_SUCCESS) 
		{
			m_nMaxLogFileSize = dwMaxLogSize;
		}
		dwBufferSize = MAX_PATH * sizeof(wchar_t);
		if (RegQueryValueEx(key,L"BrowserConnectorLogFile",NULL,&dwType,(LPBYTE)&sPathBuffer,&dwBufferSize)==ERROR_SUCCESS) 
		{
			m_sBrowserConnectorFilePath = sPathBuffer;
		}

		dwBufferSize = MAX_PATH * sizeof(wchar_t);
		if (RegQueryValueEx(key,L"CtiConnectorLogFile",NULL,&dwType,(LPBYTE)&sPathBuffer,&dwBufferSize)==ERROR_SUCCESS) 
		{
			m_sCtiConnectorFilePath = sPathBuffer;
		}
	}

	if (m_sBrowserConnectorFilePath.empty()) {
		GetCurrentDirectoryW(MAX_PATH,sPathBuffer);
		m_sBrowserConnectorFilePath = sPathBuffer;
		m_sBrowserConnectorFilePath += L"\\browser_connector.log";
		bSave = true;
	}
		
	if (m_sCtiConnectorFilePath.empty()) {
		GetCurrentDirectoryW(MAX_PATH,sPathBuffer);
		m_sCtiConnectorFilePath = sPathBuffer;
		m_sCtiConnectorFilePath += L"\\cti_connector.log";
		bSave = true;
	}

	if (bSave) SaveLogInfoToRegistry();
}
	
void CCTILogger::SaveLogInfoToRegistry()
{
	HKEY key;
	DWORD dwDisposition;
	DWORD dwLogLevel = m_nLogLevel;
	if (RegCreateKeyEx(HKEY_CURRENT_USER,L"Software\\Salesforce.com\\CTI",0,
						NULL,REG_OPTION_NON_VOLATILE,KEY_WRITE,NULL,
						&key,&dwDisposition)==ERROR_SUCCESS) 
	{
		RegSetValueEx(key,L"LogLevel",NULL,REG_DWORD,(const BYTE*)(&dwLogLevel),sizeof(DWORD));
		RegSetValueEx(key,L"BrowserConnectorLogFile",NULL,
						REG_SZ,(const BYTE*)m_sBrowserConnectorFilePath.c_str(),
						(DWORD)m_sBrowserConnectorFilePath.length()*sizeof(wchar_t));
		RegSetValueEx(key,L"CtiConnectorLogFile",NULL,
						REG_SZ,(const BYTE*)m_sCtiConnectorFilePath.c_str(),
						(DWORD)m_sCtiConnectorFilePath.length()*sizeof(wchar_t));
	}
}

void CCTILogger::WriteLogEntry(int nLogLevel, const wchar_t* sLogText, ...) {
	AutoLock autoLock(this);

#ifndef _DEBUG
	//If we're not in debug mode, there's no sense doing all this work if we're not going to log anything
	//But in debug mode, even if we don't log it, we want to trace it, so we should do the work.
	if (nLogLevel<=m_nLogLevel) {
#endif

	//First trim whitespace wchar_ts off of sLogText
	size_t len = wcslen(sLogText);

	//The maximum length of a log entry is MAX_LOGLENGTH wchar_ts
	if (len>MAX_LOGLENGTH) len=MAX_LOGLENGTH;
	for (size_t index = len-1; index>=0; index--) {
		if (!iswcntrl(sLogText[index]) && !iswspace(sLogText[index])) 
		{
			break;
		}
	}
	wchar_t sTrimmedLogText[MAX_LOGLENGTH+1];

	wcsncpy(sTrimmedLogText,sLogText,index+1);
	sTrimmedLogText[index+1]=NULL;

	va_list arglist;

	wchar_t sLogBuffer[MAX_LOGLENGTH+1];
	va_start( arglist, sLogText );
	//Format the log like printf (this is the buffer-overflow-protected version of vsprintf)
	_vsnwprintf(sLogBuffer,MAX_LOGLENGTH,sTrimmedLogText,arglist);
	va_end( arglist );
	sLogBuffer[MAX_LOGLENGTH]=NULL;

	wchar_t sDateStr[64];
	GetDateFormat(
		NULL,               // locale
		0,					// options
		NULL,				// date
		L"MM/dd/yyyy",       // date format
		sDateStr,          // formatted string buffer
		64					// size of buffer
	);

	wchar_t sTimeStr[64];
	GetTimeFormat(
		LOCALE_USER_DEFAULT,// locale
		0,					// options
		NULL,				// time
		L"hh:mm:ss tt",      // time format string
		sTimeStr,			// formatted string buffer
		64					// size of string buffer
	);

	//Now trim the output too (in case the input params had a bunch of whitespace in them)
	len = wcslen(sLogBuffer);
	for (size_t index = len-1; index>=0; index--) {
		if (!iswcntrl(sLogBuffer[index]) && !iswspace(sLogBuffer[index]))
		{
			break;
		}
	}
	wchar_t sTrimmedOutput[MAX_LOGLENGTH+1];
	wcsncpy(sTrimmedOutput,sLogBuffer,index+1);
	sTrimmedOutput[index+1]=NULL;

	if (nLogLevel<=m_nLogLevel) {
		//Lock on the file resource -- it'll unlock when this AutoLock goes out of scope
		FILE* pLogFileStream = GetLogFile();

		if (pLogFileStream!=NULL) {
			//If it's still null at this point, then we have a serious problem -- the log file could not be opened.
			fwprintf(pLogFileStream,L"%s %s: %s\n",sDateStr,sTimeStr,sTrimmedOutput);
			fflush(pLogFileStream);	
		} else {
			ATLTRACE(L"Unable to write to log file.\n");
		}
	}

	//Always log it to std debug, regardless of the log level
	ATLTRACE(L"%s %s: %s\n",sDateStr,sTimeStr,sTrimmedOutput);

#ifndef _DEBUG
	}
#endif
}

std::wstring CCTILogger::GetLogFilePath(int nLogType)
{
	if (GetLogger()) {
		if (nLogType==LOGTYPE_BROWSER_CONNECTOR) {
			return GetLogger()->m_sBrowserConnectorFilePath;
		} else {
			return GetLogger()->m_sCtiConnectorFilePath;
		}
	}
	return L"";
}

void CCTILogger::SetLogFilePaths(std::wstring sBrowserConnectorFilePath, std::wstring sCtiConnectorFilePath)
{
	if (GetLogger()) {
		GetLogger()->SetLogFiles(sBrowserConnectorFilePath,sCtiConnectorFilePath);
	}
}

FILE* CCTILogger::GetLogFile()
{
	if (m_pLogFileStream==NULL) {
		//Open the log file if it's not been opened
		if (m_nLogType==LOGTYPE_BROWSER_CONNECTOR) {
			m_pLogFileStream = _wfopen(m_sBrowserConnectorFilePath.c_str(),L"a");
		} else {
			m_pLogFileStream = _wfopen(m_sCtiConnectorFilePath.c_str(),L"a");
		}
	} // if (m_pLogFileStream!=NULL)
	return m_pLogFileStream;
}

void CCTILogger::SetLogFiles(std::wstring& sBrowserConnectorFilePath, std::wstring& sCtiConnectorFilePath)
{
	//If this log's file has changed, we have to close the old file (the new one will be opened when we have something to write).
	bool bSave = false;
	if (m_sBrowserConnectorFilePath!=sBrowserConnectorFilePath) {
		m_sBrowserConnectorFilePath=sBrowserConnectorFilePath;
		bSave = true;
		if (m_nLogType==LOGTYPE_BROWSER_CONNECTOR) {
			//Lock on the file resource -- it'll unlock when this AutoLock goes out of scope
			AutoLock autoLock(this);
			if (m_pLogFileStream!=NULL) {
				fclose(m_pLogFileStream);
				m_pLogFileStream=NULL;
			}
		}
	}
	if (m_sCtiConnectorFilePath!=sCtiConnectorFilePath) {
		m_sCtiConnectorFilePath=sCtiConnectorFilePath;
		bSave = true;
		if (m_nLogType==LOGTYPE_CTI_CONNECTOR) {
			//Lock on the file resource -- it'll unlock when this AutoLock goes out of scope
			AutoLock autoLock(this);
			if (m_pLogFileStream!=NULL) {
				fclose(m_pLogFileStream);
				m_pLogFileStream=NULL;
			}
		}
	}

	if (bSave) SaveLogInfoToRegistry();
}
