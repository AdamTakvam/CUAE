#pragma once

#include <string>
#include "CTIUtils.h"

#define LOGLEVEL_LOW					1	//This log level is for errors only
#define LOGLEVEL_MED					2	//This one should contain errors plus some informational stuff
#define LOGLEVEL_HIGH					3	//This logs everything that goes on

#define LOGTYPE_BROWSER_CONNECTOR		0
#define LOGTYPE_CTI_CONNECTOR			1

/**
 * This class contains a bunch of static methods that allow us to log events to a file, classifying them
 * by level (low, medium or high).
 *
 * The "low" level should be for errors only.  The "med" level is for informational messages, and the
 * "high" level is for very granular info (like the specific XML sent back and forth in messages and UIs).
 *
 * @version 1.0
 */
class CCTILogger : CCriticalSection
{
protected:
	/**
	 * Writes the log entry to the file (if necessary) and to ATLTRACE
	 *
	 * @param nLogLevel The current log level
	 * @param sLogText The text to log
	 * @param ... The VARARGS containing the printf-style log parameters that will be filled in
	 */
	virtual void WriteLogEntry(int nLogLevel, const wchar_t* sLogText, ...);
	/**
	 * Constructor for CCTILogger.
	 *
	 * @param nLogType The log type.  Must be one of LOGTYPE_BROWSER_CONNECTOR or LOGTYPE_CTI_CONNECTOR.
	 */
	CCTILogger(int nLogType);

	int m_nLogType; /**< The log type.  Must be one of LOGTYPE_BROWSER_CONNECTOR or LOGTYPE_CTI_CONNECTOR. */
	int m_nLogLevel; /**< The current log level.  Should be between 1 and 3. */
	int m_nMaxLogFileSize; /**< The maximum log file size.  Defaults to 500kb unless otherwise set in the registry. */
	int m_nMaxLogFiles; /**< The maximum number of log files.  Defaults to 10 unless otherwise set in the registry. */
	std::wstring m_sCtiConnectorFilePath; /**< The log file path for adapters. */
	std::wstring m_sBrowserConnectorFilePath; /**< The log file path for the browser controller. */
	FILE* m_pLogFileStream; /**< The log file stream. */

	/**
	 * Loads the log level from the Windows registry.
	 */
	void LoadLogInfoFromRegistry();
	/**
	 * Saves the log level to the Windows registry.
	 */
	void SaveLogInfoToRegistry();

	/**
	 * Gets the file to write log entries to, creating a new file if necessary.
	 *
	 * @return The file pointer to the file.
	 */
	FILE* GetLogFile();

	/**
	 * Sets the log file paths.  This is the non-static method that is called by the static method SetLogFilePaths.
	 *
	 * @param sBrowserConnectorFilePath The browser connector log file path.
	 * @param sCtiConnectorFilePath The adapter log file path.
	 */
	void SetLogFiles(std::wstring& sBrowserConnectorFilePath, std::wstring& sCtiConnectorFilePath);

	static CCTILogger* GetLogger(); /**< Gets the one and only logger object, or NULL if none have been created. */

	static CCTILogger* theLogger; /**< The one and only logger object. */
public:
	virtual ~CCTILogger(void);

	/**
	 * Creates a static instance of the logger.
	 *
	 * @param nLogType The log type.  Must be one of LOGTYPE_BROWSER_CONNECTOR or LOGTYPE_CTI_CONNECTOR.
	 */
	static void CreateLogger(int nLogType);

	/**
	 * Destroys the static instance of the logger.
	 */
	static void DestroyLogger();

	/**
	 * All these Log methods are to handle the various permutations of strings and numbers that might be passed in.
	 * This could have been done with a varargs method (like WriteLogEntry above), but due to the lack of type safety inherent
	 * in varargs in C++, that would be especially prone to crashes if someone passes in something (say, an std::wstring instead of
	 * a wchar_t*) that is unexpected.
	 */
	static void Log(int nLogLevel,const wchar_t* sLogText, const wchar_t* sData1=L"", const wchar_t* sData2=L"", const wchar_t* sData3=L"",const wchar_t* sData4=L"")
	{
		if (GetLogger()) GetLogger()->WriteLogEntry(nLogLevel,sLogText,sData1,sData2,sData3,sData4);
	};

	static void Log(int nLogLevel,const wchar_t* sLogText, long nData1, const wchar_t* sData2=L"", const wchar_t* sData3=L"")
	{
		if (GetLogger()) GetLogger()->WriteLogEntry(nLogLevel,sLogText,nData1,sData2,sData3);
	};

	static void Log(int nLogLevel,const wchar_t* sLogText, long nData1, long nData2, const wchar_t* sData3=L"")
	{
		if (GetLogger()) GetLogger()->WriteLogEntry(nLogLevel,sLogText,nData1,nData2,sData3);
	};

	static void Log(int nLogLevel,const wchar_t* sLogText, const wchar_t* sData1, long nData2, const wchar_t* sData3=L"")
	{
		if (GetLogger()) GetLogger()->WriteLogEntry(nLogLevel,sLogText,sData1,nData2,sData3);
	};

	static void Log(int nLogLevel,const wchar_t* sLogText, const std::wstring& sData1)
	{
		Log(nLogLevel,sLogText,sData1.c_str());
	};

	static void Log(int nLogLevel,const wchar_t* sLogText, const std::wstring& sData1, const std::wstring& sData2)
	{
		Log(nLogLevel,sLogText,sData1.c_str(),sData2.c_str());
	};

	static void Log(int nLogLevel,const wchar_t* sLogText, const std::wstring& sData1, const wchar_t* sData2)
	{
		Log(nLogLevel,sLogText,sData1.c_str(),sData2);
	};

	static void Log(int nLogLevel,const wchar_t* sLogText, const std::wstring& sData1, const std::wstring& sData2, const std::wstring& sData3)
	{
		Log(nLogLevel,sLogText,sData1.c_str(),sData2.c_str(),sData3.c_str());
	};

	static void Log(int nLogLevel,const wchar_t* sLogText, const std::wstring& sData1, const std::wstring& sData2, const std::wstring& sData3, const std::wstring& sData4)
	{
		Log(nLogLevel,sLogText,sData1.c_str(),sData2.c_str(),sData3.c_str(),sData4.c_str());
	};

	static void Log(int nLogLevel,const wchar_t* sLogText, long nData1, const std::wstring& sData2)
	{
		Log(nLogLevel,sLogText,nData1,sData2.c_str());
	};

	static void Log(int nLogLevel,const wchar_t* sLogText, const std::wstring& sData1, long nData2)
	{
		Log(nLogLevel,sLogText,sData1.c_str(),nData2);
	};

	/**
	 * Gets the log file path for the specified type of log.
	 *
	 * @param nLogType The log type.  Must be one of LOGTYPE_BROWSER_CONNECTOR or LOGTYPE_CTI_CONNECTOR.
	 * @return The log file path for the specified type of log.
	 */
	static std::wstring GetLogFilePath(int nLogType);

	/**
	 * Sets the log file path for the two possible types of log.
	 *
	 * @param sBrowserConnectorFilePath The log file path for a browser connector log.
	 * @param sCtiConnectorFilePath The log file path for a CTI connector log.
	 */
	static void SetLogFilePaths(std::wstring sBrowserConnectorFilePath, std::wstring sCtiConnectorFilePath);

	/**
	 * Sets the log level and saves it to the registry.
	 *
	 * @param nLogLevel The log level.  Must be one of LOGLEVEL_LOW, LOGLEVEL_MED, or LOGLEVEL_HIGH.
	 */
	static void SetLogLevel(int nLogLevel)
	{
		if (GetLogger()) {
			GetLogger()->m_nLogLevel = nLogLevel;
			GetLogger()->SaveLogInfoToRegistry();
			CCTILogger::Log(LOGLEVEL_MED,L"Log level set to %d.",nLogLevel);
		}
	};

	/**
	 * Gets the current log level.
	 *
	 * @return The current log level.
	 */
	static int GetLogLevel() { return GetLogger()->m_nLogLevel; };
};
