#pragma once
#include "CTIAppExchange.h"
#include "CTIConstants.h"

class CSearchThreadInfo;

/**
 * @version 1.0
 * 
 * This class encapsulates a thread whose purpose is to search for items in the Salesforce.com database via
 * the AppExchange API.  One such use is searching for ANIs and IVR data.
 *
 * This thread exists so that data can be searched in the background without freezing the user interface.
 * The search thread is entirely managed by an instance of this class, which means that the thread gets 
 * created and started in the constructor of this class, and stopped and destroyed in the destructor.
 */
class CCTIAppExchangeSearchThread
{
protected:
	static bool m_bExiting; /**< A static flag that indicates to the save thread, if it's true, that the adapter is exiting, so it shouldn't make any calls to memory outside its thread space. */
	DWORD m_dwThreadId; /**< The thread ID of the search thread. */
	CCTIAppExchange* m_pAppExchange; /**< The parent CCTIAppExchange of this object. */
	HWND m_hWnd; /**< The handle of the hidden window used to send messages from the search thread to the main thread. */
	std::wstring m_sHiddenWindowClassName; /**< The class name of the hidden window.  It is generated dynamically before the window is created. */
	
	/**
	 * Launches the save thread.  This should only be called by this class's constructor
	 * so that there is a CCTIAppExchangeSaveThread instance that encapsulates the thread.
	 *
	 * @param pThreadParam 
	 * @return Always 0.
	 */
	static DWORD WINAPI RunThread(LPVOID pThreadParam);

	/**
	 * Causes the save thread encapsulated by this object to exit.  Should not be called directly;
	 * this method is called by this object's destructor, which is the preferred method of exiting the thread.
	 */
	virtual void ExitThread();

	/**
	 * Performs a background search, first using the call log information, then using the IVR query
	 * if it's available, and finally falling back on a SOSL search on the ANI.
	 *
	 * @param pSession The session to search with.
	 * @param pInfo The search thread info.
	 */
	static void ThreadSearch(ISForceSession4Ptr pSession,CSearchThreadInfo* pInfo);

	static void ThreadRunLogQuery(ISForceSession4Ptr pSession, CSearchThreadInfo* pInfo, std::wstring sCallLogQuery, RelObjSetList& listRelObjSets);
public:
	/**
	 * Creates an instance of CCTIAppExchangeSearchThread and starts the save thread.
	 */
	CCTIAppExchangeSearchThread(CCTIAppExchange* pAppExchange);

	/**
	 * Destroys this instance of CCTIAppExchangeSearchThread and stops the save thread.
	 */
	~CCTIAppExchangeSearchThread();

	/**
	 * The WindowProc method that handles calls from the hidden window that this class
	 * uses to communicate with the main thread.
	 *
	 * @param hwnd The window handle.
	 * @param msg The message.
	 * @param wp The word param.
	 * @param lp The long param.
	 * @return Always 0.
	 */
	static LRESULT CALLBACK HiddenWindowProc(HWND hwnd, UINT msg, WPARAM wp, LPARAM lp);

	/**
	 * Instructs the search thread to perform an asynchronous search.  It will first search based
	 * on the log information specified, then using the IVR queries, and finally using the ANI search.
	 * If any of the three searches produce results, it will notify the main thread to update
	 * the UI to reflect the search results.
	 *
	 * Note that if the call log data, IVR query list, or ANI query is empty, they will silently be ignored.
	 *
	 * @param nLayout The ID of the layout.
	 * @param sCallObjectId The call object ID of the call we're performing a search for.
	 * @param sPartyId The ANI of the party to which this search applies.
	 * @param sCallLogQueryWho The attached Who data.
	 * @param sCallLogQueryWhat The attached What data.
	 * @param listIVRQueries The list of IVR queries to run.
	 * @param bsANIQuery The ANI query to run.
	 */
	virtual void Search(int nLayout, std::wstring sCallObjectId, std::wstring sPartyId, 
						std::wstring sCallLogQueryWho, std::wstring sCallLogQueryWhat, 
						BSTRList& listIVRQueries, _bstr_t bsANIQuery);
};

/**
 * This class exists solely to provide information to CCTIAppExchangeSearchThread when commands are
 * issued to its underlying thread and when commands are sent back from the thread to the hidden window.  
 * It generally does not need to be instantiated by classes other than CCTIAppExchangeSearchThread.
 *
 * @version 1.0
 */
class CSearchThreadInfo
{
public:
	CSearchThreadInfo(CSearchThreadInfo* pOriginalInfo,RelObjSetList& listRelObjSets);

	CSearchThreadInfo(CSearchThreadInfo* pOriginalInfo,std::wstring sId,std::wstring sName,std::wstring sLabel,std::wstring sEntityName);

	CSearchThreadInfo(CCTIAppExchange* pAppExchange, HWND hWnd, int nLayout, std::wstring sCallObjectId, std::wstring sPartyId, std::wstring sCallLogQueryWho, std::wstring sCallLogQueryWhat, BSTRList& listIVRQueries, _bstr_t bsANIQuery);
	
	std::wstring m_sSid;
	std::wstring m_sInstance;
	ObjectFieldMap m_mapLayout;
	HWND m_hWnd;
	std::wstring m_sCallObjectId;
	std::wstring m_sPartyId;
	std::wstring m_sCallLogQueryWho;
	std::wstring m_sCallLogQueryWhat;
	BSTRList m_listIVRQueries;
	_bstr_t m_bsANIQuery;
	CCTIUserInterface* m_pUI;

	std::wstring m_sLogId;
	std::wstring m_sLogName;
	std::wstring m_sLogLabel;
	std::wstring m_sLogEntityName;

	RelObjSetList m_listRelObjSets;
};