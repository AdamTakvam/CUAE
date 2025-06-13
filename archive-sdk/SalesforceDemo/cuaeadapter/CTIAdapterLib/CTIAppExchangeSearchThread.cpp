#include "stdafx.h"
#include "CTIAppExchangeSearchThread.h"

//These numbers were chosen so as not to conflict with other thread commands floating around
#define CMD_SEARCH						WM_USER + 4000
#define CMD_SEARCH_EXIT					WM_USER + 4001

#define WM_SEARCH_RETURNED				WM_USER + 4500
#define WM_ADD_LOG_OBJECT				WM_USER + 4501

#define SEARCHTHREAD_WINDOW_NAME		L"SFDC_SearchThreadWnd"

bool CCTIAppExchangeSearchThread::m_bExiting = false;

CCTIAppExchangeSearchThread::CCTIAppExchangeSearchThread(CCTIAppExchange* pAppExchange)
:m_dwThreadId(0),m_pAppExchange(pAppExchange)
{
	m_bExiting = false;

	//First create the hidden window
	WNDCLASS wc;
	ZeroMemory(&wc, sizeof(WNDCLASS));
	wc.style = 0;
	wc.lpfnWndProc = CCTIAppExchangeSearchThread::HiddenWindowProc;
	HINSTANCE hi = GetModuleHandle(NULL);
	wc.hInstance = hi;

	GUID vGUID;
	HRESULT hr = CoCreateGuid(&vGUID);
	if ( FAILED( hr ) ) {
		CCTILogger::Log(LOGLEVEL_LOW,L"CCTIAppExchangeSaveThread::CCTIAppExchangeSaveThread: Unable to create GUID for save hidden window.");
	} else {
		// convert the GUID into a string
		WCHAR szDSGUID[39];
		StringFromGUID2(vGUID, szDSGUID, 39);

		m_sHiddenWindowClassName = SEARCHTHREAD_WINDOW_NAME;
		m_sHiddenWindowClassName += szDSGUID;
		
		wc.lpszClassName = m_sHiddenWindowClassName.c_str();
		ATOM atomWindow = RegisterClass(&wc);
		
		if (atomWindow) {
			m_hWnd = CreateWindow(m_sHiddenWindowClassName.c_str(), m_sHiddenWindowClassName.c_str(), WS_POPUP, 0, 0, 0, 0, NULL,NULL, hi, NULL);
		}

		if (!IsWindow(m_hWnd)) {
			CCTILogger::Log(LOGLEVEL_LOW,L"CCTIAppExchangeSaveThread::CCTIAppExchangeSaveThread: Unable to create save hidden window.");
		}
	}

	//Now fire up the thread
	HANDLE hThread = CreateThread(NULL, 0, CCTIAppExchangeSearchThread::RunThread, (LPVOID)pAppExchange, 0, &m_dwThreadId);
	if (m_dwThreadId==0) {
		CCTILogger::Log(LOGLEVEL_LOW,L"CCTIAppExchangeSearchThread::CCTIAppExchangeSearchThread: Unable to create search thread.");
	}
}

CCTIAppExchangeSearchThread::~CCTIAppExchangeSearchThread()
{
	if(IsWindow(m_hWnd)) DestroyWindow(m_hWnd);

	HINSTANCE hi = GetModuleHandle(NULL);
	UnregisterClass(m_sHiddenWindowClassName.c_str(),hi);

	m_bExiting = true;
	ExitThread();
}

DWORD CCTIAppExchangeSearchThread::RunThread(LPVOID pThreadParam)
{
	CCTIAppExchange* pAppExchange = (CCTIAppExchange*)pThreadParam;
	//The thread initialization stuff goes here
	CoInitializeEx(NULL,COINIT_MULTITHREADED);
	ISForceSession4Ptr pSession = CCTIAppExchange::CreateOfficeToolkit();

	CCTIAppExchange::SetSessionInstanceAndSid(pSession,pAppExchange->GetInstance(),pAppExchange->GetSid());

	bool bContinueRunning = true;
	
	//Run a message loop
	MSG	pMessage;
	while (bContinueRunning && GetMessage(&pMessage, 0, 0, 0) > 0)
	{
		TranslateMessage(&pMessage);
		switch(pMessage.message)
		{
			case CMD_SEARCH:
				{
					CSearchThreadInfo* pInfo = (CSearchThreadInfo*)pMessage.lParam;
					CCTIAppExchange::SetSessionInstanceAndSid(pSession,pInfo->m_sInstance,pInfo->m_sSid);
					if (pInfo) {
						ThreadSearch(pSession, pInfo);
						delete pInfo;
					}
					break;
				}
			case CMD_SEARCH_EXIT:
				{
					bContinueRunning = false;
					break;
				}

			default:
				DispatchMessage(&pMessage);
				break;
		}
	}

	//The thread cleanup code goes here
	pSession.Release();

	CoUninitialize();
	return 0;
}

LRESULT CALLBACK CCTIAppExchangeSearchThread::HiddenWindowProc(HWND hwnd, UINT msg, WPARAM wp, LPARAM lp)
{
	if (msg==WM_SEARCH_RETURNED) {
		CSearchThreadInfo* pInfo = (CSearchThreadInfo*)lp;
		if (pInfo) {
				CCTIUserInterface* pUI = pInfo->m_pUI;
				std::wstring sCallObjectId = pInfo->m_sCallObjectId;
				//Add the search results to the party of the line
				CCTILine* pLine = pUI->GetLineByCallId(sCallObjectId);
				if (pLine) {
					if (!pInfo->m_listRelObjSets.empty()) {
						CCTIParty* pParty = pLine->GetPartyByANI(pInfo->m_sPartyId);
						if (pParty) {
							pParty->AddRelatedObjectSets(pInfo->m_listRelObjSets);
						}

						CCTICallLog* pLog = pUI->GetCallLogForCallId(sCallObjectId);
						if (pLog) {
							pLog->AddRelatedObjects(&(pInfo->m_listRelObjSets));
						}
					} else {
						pLine->ShowLineStatusBar(false,KEY_NO_RESULTS_FOUND);
					}
						
					pUI->UIRefresh();
				}

			delete pInfo;
		}
	} else if (msg==WM_ADD_LOG_OBJECT) {
		CSearchThreadInfo* pInfo = (CSearchThreadInfo*)lp;
		if (pInfo) {
			CCTIUserInterface* pUI = pInfo->m_pUI;
			std::wstring sCallObjectId = pInfo->m_sCallObjectId;

			//Take the info gathered from the thread and add it to the call log with that call object ID			
			CCTICallLog* pLog = pUI->GetCallLogForCallId(sCallObjectId);
			if (pLog) {
				pLog->AddObject(pInfo->m_sLogId,pInfo->m_sLogLabel,pInfo->m_sLogName,pInfo->m_sLogEntityName,true);
			}

			pUI->UIRefresh();

			delete pInfo;
		}
	}
	return DefWindowProc(hwnd, msg, wp, lp);
}

void CCTIAppExchangeSearchThread::ExitThread()
{
	if (m_dwThreadId!=0) {
		PostThreadMessage(m_dwThreadId,CMD_SEARCH_EXIT,0,0);
		m_dwThreadId = 0;
	} else {
		CCTILogger::Log(LOGLEVEL_LOW,L"CCTIAppExchangeSearchThread::ExitThread: Could not exit thread; search thread does not exist.");
	}
}

void CCTIAppExchangeSearchThread::Search(int nLayout, std::wstring sCallObjectId, std::wstring sPartyId, 
										 std::wstring sCallLogQueryWho, std::wstring sCallLogQueryWhat, 
										 BSTRList& listIVRQueries, _bstr_t bsANIQuery)
{
	CSearchThreadInfo* pThreadInfo = new CSearchThreadInfo(m_pAppExchange, m_hWnd, nLayout, 
															sCallObjectId, sPartyId, sCallLogQueryWho, 
															sCallLogQueryWhat, listIVRQueries, bsANIQuery);
	::PostThreadMessage(m_dwThreadId,CMD_SEARCH,0,(WPARAM)pThreadInfo);
}

void CCTIAppExchangeSearchThread::ThreadSearch(ISForceSession4Ptr pSession, CSearchThreadInfo* pInfo)
{
	CCTILogger::Log(LOGLEVEL_MED,L"CCTIAppExchangeSearchThread::ThreadSearch: Asynchronous search started.");
	//Make a container for the related object sets (which the main thread will own after this thread sends it out)
	RelObjSetList listRelObjSets;

	//First run the log queries for the who and the what, if we got any
	ThreadRunLogQuery(pSession,pInfo,pInfo->m_sCallLogQueryWho,listRelObjSets);
	ThreadRunLogQuery(pSession,pInfo,pInfo->m_sCallLogQueryWhat,listRelObjSets);

	if (listRelObjSets.empty()) {
		//We got nothing from the logs -- try the IVR search
		for (BSTRList::iterator itQuery=pInfo->m_listIVRQueries.begin();itQuery!=pInfo->m_listIVRQueries.end();itQuery++) {
			_bstr_t bsQuery = *itQuery;
			try
			{
				IQueryResultSet4Ptr pQueryResults = pSession->Query(bsQuery,VARIANT_FALSE);
				CCTIAppExchange::PopulateQueryResults(pSession,pQueryResults,pInfo->m_mapLayout,listRelObjSets,false);
			}
			catch(_com_error ce)
			{
				CCTILogger::Log(LOGLEVEL_LOW,L"CCTIAppExchangeSearchThread::ThreadSearch: Error running IVR query: %s.",(wchar_t*)pSession->GetErrorMessage());
			}
		}
	}

	if (listRelObjSets.empty() && pInfo->m_bsANIQuery.length()>0) {
		//Still nothing from the logs and IVR -- try the ANI search
		try
		{
			IQueryResultSet4Ptr pQueryResults = pSession->Search(pInfo->m_bsANIQuery,VARIANT_FALSE);
			CCTIAppExchange::PopulateQueryResults(pSession,pQueryResults,pInfo->m_mapLayout,listRelObjSets,true);
		}
		catch(_com_error ce)
		{
			CCTILogger::Log(LOGLEVEL_LOW,L"CCTIAppExchangeSearchThread::ThreadSearch: Error running ANI query: %s.",(wchar_t*)pSession->GetErrorMessage());
		}
	}

	//Send the related object sets to the main thread to be added to the UI.
	if (IsWindow(pInfo->m_hWnd))
	{
		CSearchThreadInfo* pReturnInfo = new CSearchThreadInfo(pInfo,listRelObjSets);
		PostMessage(pInfo->m_hWnd, WM_SEARCH_RETURNED, 0, (LPARAM)pReturnInfo);
	} else {
		//The window's gone -- clean up the list of related objects so we don't make a memory leak
		for (RelObjSetList::iterator it = listRelObjSets.begin(); it!=listRelObjSets.end(); it++) {
			CCTIRelatedObjectSet* pSet = *it;
			delete pSet;
		}
		listRelObjSets.clear();
	}

	CCTILogger::Log(LOGLEVEL_MED,L"CCTIAppExchangeSearchThread::ThreadSearch: Asynchronous search complete.");
}

void CCTIAppExchangeSearchThread::ThreadRunLogQuery(ISForceSession4Ptr pSession, CSearchThreadInfo* pInfo, std::wstring sCallLogQuery, RelObjSetList& listRelObjSets)
{
	std::wstring sObjectType;
	std::wstring sId;

	if (!sCallLogQuery.empty()) {
		size_t nColon = sCallLogQuery.find(':');
		if (nColon!=std::wstring::npos && nColon!=sCallLogQuery.length()-1) {
			sObjectType = sCallLogQuery.substr(0,nColon);
			sId = sCallLogQuery.substr(nColon+1,sCallLogQuery.length()-nColon);
		} else {
			CCTILogger::Log(LOGLEVEL_LOW,L"CCTIAppExchangeSearchThread::ThreadSearch: Error: no object type found in call log value %s.",sCallLogQuery);
		}
	}

	if (!sObjectType.empty() && !sId.empty()) {
		ObjectFieldMap mapLayout = pInfo->m_mapLayout;

		//First, make sure this object is valid for this user (and get its fields)
		StringList* pListObjectFields = CCTIAppExchange::QueryObjectInfo(pSession, sObjectType);

		//Now see if this object is in the current layout
		StringList* listFields = mapLayout[sObjectType];

		//If it's not in the layout, look it up anyway so we can add it to the call log...
		bool bInLayout = (listFields!=NULL);

		//But only if the user can actually see this object.
		if (pListObjectFields!=NULL) {
			std::wstring sNameFieldsList;

			if (!bInLayout) {
				StringList* pListNameFields = CCTIAppExchange::GetNameFields(sObjectType);
				if (pListNameFields!=NULL) {
					size_t nIndex = 1;
					size_t nNumFields = pListNameFields->size();
					for (StringList::iterator it = pListNameFields->begin(); it!=pListNameFields->end(); it++) {
						sNameFieldsList += *it;
						if (nIndex<nNumFields) {
							//Add a comma if this is not the last name field
							sNameFieldsList += L",";
						}
						nIndex++;
					}
				}
			}

			//Create the query for this object for this layout
			std::wstring sQuery = L"Select ";
			sQuery += bInLayout?CCTIAppExchange::GetFieldListForObject(mapLayout,sObjectType):sNameFieldsList;
			sQuery += L" From "+sObjectType;
			sQuery += L" Where Id='" + sId +L"'";

			CCTILogger::Log(LOGLEVEL_MED,L"CCTIAppExchange::RunLogQuery: Generated query: %s.",sQuery);

			_bstr_t bsQuery = CCTIUtils::StringToBSTR(sQuery);
			IQueryResultSet4Ptr	pQueryResults = NULL;

			try
			{
				pQueryResults = pSession->Query(bsQuery,VARIANT_FALSE);
			}
			catch(_com_error ce)
			{
				CCTILogger::Log(LOGLEVEL_LOW,L"CCTIAppExchange::RunLogQuery: Error running query: %s.",(wchar_t*)pSession->GetErrorMessage());
			}

			if (pQueryResults) {
				if (bInLayout) {
					CCTIAppExchange::PopulateQueryResults(pSession,pQueryResults,mapLayout,listRelObjSets,false);
				}
				
				//No matter what, we add the object to the log
				BEGIN_FOREACH_QUERYRESULT(pQueryResults,pSObject)
					std::wstring sObjectType = CCTIUtils::FirstCharToUpperCase(CCTIUtils::BSTRToString(pSObject->ObjectType));

					//Now get the name
					std::wstring sName = CCTIAppExchange::GetSObjectName(pSession, pSObject);

					//And the object label (like "Contact")
					std::wstring sObjectLabel = CCTIUtils::BSTRToString(pSObject->Label);

					//Post a message back to the hidden window to add this object to the log
					if (IsWindow(pInfo->m_hWnd))
					{
						CSearchThreadInfo* pLogMessageInfo = new CSearchThreadInfo(pInfo,sId,sName,sObjectLabel,sObjectType);
						PostMessage(pInfo->m_hWnd, WM_ADD_LOG_OBJECT, 0, (LPARAM)pLogMessageInfo);
					}

					//pLog->AddObject(sId,sObjectLabel,sName,sObjectType,false);
				END_FOREACH_QUERYRESULT()

				pQueryResults.Release();
			}
		} else {
			CCTILogger::Log(LOGLEVEL_MED,L"CCTIAppExchange::GenerateLogQuery: The object %s is not valid for this user; could not create query with that object.",sObjectType);
		}
	}
}

CSearchThreadInfo::CSearchThreadInfo(CSearchThreadInfo* pOriginalInfo,RelObjSetList& listRelObjSets):
m_listRelObjSets(listRelObjSets)
{
	m_pUI = pOriginalInfo->m_pUI;
	m_sCallObjectId = pOriginalInfo->m_sCallObjectId;
	m_hWnd = pOriginalInfo->m_hWnd;
	m_sPartyId = pOriginalInfo->m_sPartyId;
}

CSearchThreadInfo::CSearchThreadInfo(CSearchThreadInfo* pOriginalInfo,std::wstring sId,std::wstring sName,std::wstring sLabel,std::wstring sEntityName):
m_sLogId(sId),
m_sLogName(sName),
m_sLogLabel(sLabel),
m_sLogEntityName(sEntityName)
{
	m_pUI = pOriginalInfo->m_pUI;
	m_sCallObjectId = pOriginalInfo->m_sCallObjectId;
	m_hWnd = pOriginalInfo->m_hWnd;
}

CSearchThreadInfo::CSearchThreadInfo(CCTIAppExchange* pAppExchange, HWND hWnd, int nLayout, 
									 std::wstring sCallObjectId, std::wstring sPartyId, 
									 std::wstring sCallLogQueryWho, std::wstring sCallLogQueryWhat,
									 BSTRList& listIVRQueries, _bstr_t bsANIQuery) :
m_hWnd(hWnd),
m_sCallObjectId(sCallObjectId), 
m_sPartyId(sPartyId),
m_sCallLogQueryWho(sCallLogQueryWho), 
m_sCallLogQueryWhat(sCallLogQueryWhat), 
m_listIVRQueries(listIVRQueries),
m_bsANIQuery(bsANIQuery)
{
	m_pUI = pAppExchange->GetCTIUserInterface();
	m_mapLayout = pAppExchange->GetMapForLayout(nLayout);
	m_sSid = pAppExchange->GetSid();
	m_sInstance = pAppExchange->GetInstance();
}