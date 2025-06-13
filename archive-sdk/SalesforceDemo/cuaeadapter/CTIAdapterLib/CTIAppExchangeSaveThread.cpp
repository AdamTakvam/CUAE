#include "stdafx.h"
#include "CTIAppExchangeSaveThread.h"
#include "CTICallLog.h"
#include "CTIAppExchange.h"
#include "CTIUtils.h"
#include "CTIUserInterface.h"
#include <atlsafe.h>

//These numbers were chosen so as not to conflict with other thread commands floating around, not that it particularly matters.
#define	CMD_UPSERT_LOG			WM_USER + 2000
#define CMD_SAVE_USER_EXTENSION	WM_USER + 2001
#define CMD_SAVE_USER_PARAMS	WM_USER + 2002
#define CMD_EXIT				WM_USER + 2003
#define CMD_INITIALIZE_SAVE		WM_USER + 2004

#define WM_SAVE_RETURNED				WM_USER + 5000

#define SAVETHREAD_WINDOW_NAME L"SFDC_SaveThreadWnd"

bool CCTIAppExchangeSaveThread::m_bExiting = false;

CCTIAppExchangeSaveThread::CCTIAppExchangeSaveThread(CCTIAppExchange* pAppExchange)
:m_dwThreadId(0),m_pAppExchange(pAppExchange),m_hWnd(NULL)
{
	m_bExiting = false;

	//First create the hidden window
	WNDCLASS wc;
	ZeroMemory(&wc, sizeof(WNDCLASS));
	wc.style = 0;
	wc.lpfnWndProc = CCTIAppExchangeSaveThread::HiddenWindowProc;
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

		m_sHiddenWindowClassName = SAVETHREAD_WINDOW_NAME;
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

	HANDLE hThread = CreateThread(NULL, 0, CCTIAppExchangeSaveThread::RunThread, pAppExchange, 0, &m_dwThreadId);
	if (m_dwThreadId==0) {
		CCTILogger::Log(LOGLEVEL_LOW,L"CCTIAppExchangeSaveThread::CCTIAppExchangeSaveThread: Unable to create save thread.");
	}
}

CCTIAppExchangeSaveThread::~CCTIAppExchangeSaveThread()
{
	if(IsWindow(m_hWnd)) DestroyWindow(m_hWnd);
	HINSTANCE hi = GetModuleHandle(NULL);
	UnregisterClass(m_sHiddenWindowClassName.c_str(),hi);

	m_bExiting = true;
	ExitThread();
}

DWORD CCTIAppExchangeSaveThread::RunThread(LPVOID pThreadParam)
{
	CCTIAppExchange* pAppExchange = (CCTIAppExchange*)pThreadParam;
	//The thread initialization stuff goes here
	CoInitializeEx(NULL,COINIT_MULTITHREADED);
	ISForceSession4Ptr pSession = CCTIAppExchange::CreateOfficeToolkit();
	//Set the timeouts a little higher here because this is just a save thread, it won't affect the main search or anything
	//If it does get stuck for a few seconds, that's ok, subsequent saves will queue up as thread messages, so they'll still save.
	pSession->HTTPConnectTimeout = 45;
	pSession->HTTPReceiveTimeout = 45;

	bool bContinueRunning = true;

	CCTIAppExchange::SetSessionInstanceAndSid(pSession,pAppExchange->GetInstance(),pAppExchange->GetSid());
	ThreadInitializeSave(pSession, pAppExchange);
	
	//Run a message loop
	MSG	pMessage;
	while (bContinueRunning && GetMessage(&pMessage, 0, 0, 0) > 0)
	{
		TranslateMessage(&pMessage);
		switch(pMessage.message)
		{
			case CMD_UPSERT_LOG:
				{
					CSaveThreadInfo* pInfo = (CSaveThreadInfo*)pMessage.lParam;
					//Make sure the SID and instance are up to date
					CCTIAppExchange::SetSessionInstanceAndSid(pSession,pInfo->m_sInstance,pInfo->m_sSid);
					ThreadUpsertCallLog(pSession,pInfo);

					delete pInfo;
					break;
				}
			case CMD_SAVE_USER_EXTENSION:
				{
					CSaveThreadInfo* pInfo = (CSaveThreadInfo*)pMessage.lParam;
					//Make sure the SID and instance are up to date
					CCTIAppExchange::SetSessionInstanceAndSid(pSession,pInfo->m_sInstance,pInfo->m_sSid);
					ThreadSaveUserExtension(pSession,pInfo->m_sUserId,pInfo->m_sExtension);
					
					delete pInfo;
					break;
				}
			case CMD_SAVE_USER_PARAMS:
				{
					CSaveThreadInfo* pInfo = (CSaveThreadInfo*)pMessage.lParam;
					//Make sure the SID and instance are up to date
					CCTIAppExchange::SetSessionInstanceAndSid(pSession,pInfo->m_sInstance,pInfo->m_sSid);
					ThreadSaveUserParams(pSession,pInfo);
					
					delete pInfo;
					break;
				}
			case CMD_EXIT:
				{
					bContinueRunning = false;
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

void CCTIAppExchangeSaveThread::UpsertCallLog(CCTICallLog* pLog)
{
	if (m_dwThreadId!=0) {
		PARAM_MAP parameters;
		CSaveThreadInfo* pThreadInfo = new CSaveThreadInfo(m_pAppExchange,parameters,m_hWnd,pLog);
		PostThreadMessage(m_dwThreadId,CMD_UPSERT_LOG,0,(LPARAM)pThreadInfo);
	} else {
		CCTILogger::Log(LOGLEVEL_LOW,L"CCTIAppExchangeSaveThread::UpsertCallLog: Could not upsert call log; save thread does not exist.");
	}
}

void CCTIAppExchangeSaveThread::SaveUserExtension(std::wstring& sExtension)
{
	if (m_dwThreadId!=0) {
		PARAM_MAP parameters;
		CSaveThreadInfo* pThreadInfo = new CSaveThreadInfo(m_pAppExchange);
		pThreadInfo->m_sExtension = sExtension;
		PostThreadMessage(m_dwThreadId,CMD_SAVE_USER_EXTENSION,0,(LPARAM)pThreadInfo);
	} else {
		CCTILogger::Log(LOGLEVEL_LOW,L"CCTIAppExchangeSaveThread::UpsertCallLog: Could not upsert call log; save thread does not exist.");
	}
}

void CCTIAppExchangeSaveThread::SaveUserParamsToCustomSetup(PARAM_MAP& parameters)
{
	if (m_dwThreadId!=0) {
		CSaveThreadInfo* pThreadInfo = new CSaveThreadInfo(m_pAppExchange,parameters,m_hWnd);
		PostThreadMessage(m_dwThreadId,CMD_SAVE_USER_PARAMS,0,(LPARAM)pThreadInfo);
	} else {
		CCTILogger::Log(LOGLEVEL_LOW,L"CCTIAppExchangeSaveThread::SaveUserParamsToCustomSetup: Could not update call log; save thread does not exist.");
	}
}

void CCTIAppExchangeSaveThread::ExitThread()
{
	if (m_dwThreadId!=0) {
		PostThreadMessage(m_dwThreadId,CMD_EXIT,0,0);
		m_dwThreadId = 0;
	} else {
		CCTILogger::Log(LOGLEVEL_LOW,L"CCTIAppExchangeSaveThread::ExitThread: Could not exit thread; save thread does not exist.");
	}
}

void CCTIAppExchangeSaveThread::ThreadInitializeSave(ISForceSession4Ptr pSession, CCTIAppExchange* pAppExchange)
{
	//Get the proper task status for a completed task.
	bool bFound = false;
	_bstr_t bsQuery = "Select MasterLabel, IsClosed From TaskStatus";
	try
	{
		IQueryResultSet4Ptr	pQueryResults = NULL;

		pQueryResults = pSession->Query(bsQuery,VARIANT_FALSE);

		if (pQueryResults) {
			//Look for the first closed status
			BEGIN_FOREACH_QUERYRESULT(pQueryResults,pSObject)
				IField4Ptr pField = pSObject->Item(_variant_t("IsClosed"));
				if (pField) {
					_variant_t vtIsClosed = pField->Value;
					if (vtIsClosed.boolVal==VARIANT_TRUE) {
						std::wstring sMasterLabel = CCTIAppExchange::GetFieldValue(pSession,pSObject,L"MasterLabel");
						pAppExchange->SetTaskCompletedStatus(sMasterLabel);
						bFound=true;
						break;
					}
				}
			END_FOREACH_QUERYRESULT()

			pQueryResults.Release();
		}
	} catch(_com_error ce) {
		CCTILogger::Log(LOGLEVEL_LOW,L"CCTIAppExchangeSaveThread::ThreadInitializeSave: Error running task status query: %s.",(wchar_t*)pSession->GetErrorMessage());
	}

	if (!bFound) {
		//In case the query failed for some reason, it's better than nothing
		pAppExchange->SetTaskCompletedStatus(L"Completed");
	}

	CCTILogger::Log(LOGLEVEL_HIGH,L"CCTIAppExchangeSaveThread::ThreadInitializeSave: Using %s for task completed status.",pAppExchange->GetTaskCompletedStatus());

	bool bLabelFound = false;

	try {
		//The localizer function takes in an LCID (like 1033), not an IETF language string (like en-US), so we'll get the LCID from Windows
		LCID lcid = GetUserDefaultLCID();

		//Now we have to convert the number to a BSTR so we can send it to the Office Toolkit
		wchar_t lcidBuffer[10]={0};
		_bstr_t bsUserLocale = _ltow(lcid,lcidBuffer,10); 

		ILocalizationContext4Ptr pLC = pSession->GetLocalizationContext(CTI_APPLICATION_NAME,CTI_APPLICATION_VERSION,bsUserLocale);

		//We try to load the labels locally first, then we try to load them from the server
		if (pLC!=NULL && (pLC->LoadLocal()==VARIANT_TRUE || pLC->LoadAndCheck()==VARIANT_TRUE)) {
			_bstr_t bsSubjectLabel = pLC->GetLabel(L"SoftPhone",L"CallLogTaskSubject");
			if (bsSubjectLabel.length()>0) {
				bLabelFound = true;
				pAppExchange->SetTaskSubject(CCTIUtils::BSTRToString(bsSubjectLabel));
			}
		} else {
			CCTILogger::Log(LOGLEVEL_LOW,L"CCTIAppExchangeSaveThread::ThreadInitializeSave: Office Toolkit Error getting call log subject label: %s.  Will use default subject text.",(wchar_t*)pSession->GetErrorMessage());
		}
	} catch(_com_error ce) {
		CCTILogger::Log(LOGLEVEL_LOW,L"CCTIAppExchangeSaveThread::ThreadInitializeSave: COM Error getting call log subject label: %s.  Will use default subject text.",(wchar_t*)pSession->GetErrorMessage());
	}

	if (!bLabelFound) {
		pAppExchange->SetTaskSubject(L"Call {0} {1}");
		CCTILogger::Log(LOGLEVEL_LOW,L"CCTIAppExchangeSaveThread::ThreadInitializeSave: Will use default subject text.");
	}
}

void CCTIAppExchangeSaveThread::ThreadUpsertCallLog(ISForceSession4Ptr pSession,CSaveThreadInfo* pInfo)
{
	//If the call log has an ID already, then we should update the task, not make a new one
	bool bUpdating = (!pInfo->m_sLogId.empty());

	CCTILogger::Log(LOGLEVEL_MED,L"CCTIAppExchangeSaveThread::ThreadUpsertCallLog: %s call log...",bUpdating?L"Updating":L"Creating");

	//No sense updating the log if it hasn't changed
	if (pInfo->m_bLogDirty) {
		try {
			//A call log is actually a closed task
			ISObject4Ptr pSObject = pSession->CreateObject("Task");
			
			if (bUpdating) {
				//Set the ID if we're updating
				CCTIAppExchange::SetFieldValue(pSObject,"Id",pInfo->m_sLogId);
			}

			//We format this date North American-style for API insertion
			wchar_t lpDateStr[64];
			GetDateFormatW(
				NULL,               // locale
				0,             // options
				NULL,			// date
				L"MM/dd/yyyy",          // date format
				lpDateStr,          // formatted string buffer
				64                // size of buffer
			);

			//None of these fields can change if the call log is being updated
			CCTIAppExchange::SetFieldValue(pSObject,L"ActivityDate",lpDateStr);
			CCTIAppExchange::SetFieldValue(pSObject,L"Subject",pInfo->m_sLogLabel);
			CCTIAppExchange::SetFieldValue(pSObject,L"CallDurationInSeconds",CCTIUtils::IntToString(pInfo->m_nLogDuration));
			if (!pInfo->m_sLogDisposition.empty()) CCTIAppExchange::SetFieldValue(pSObject,L"CallDisposition",pInfo->m_sLogDisposition);
			CCTIAppExchange::SetFieldValue(pSObject,L"CallObject",pInfo->m_sLogCallObjectId);
			//CCTIAppExchange::SetFieldValue(pSObject,L"Priority", L"Log");

			std::wstring sCallType;
			switch (pInfo->m_nLogCallType) {
				case CALLTYPE_INTERNAL:
					sCallType = L"Internal";
					break;
				case CALLTYPE_INBOUND:
					sCallType = L"Inbound";
					break;
				case CALLTYPE_OUTBOUND:
					sCallType = L"Outbound";
					break;
			}

			CCTIAppExchange::SetFieldValue(pSObject,L"CallType",sCallType);
			CCTIAppExchange::SetFieldValue(pSObject,L"OwnerId",pInfo->m_sUserId);
			CCTIAppExchange::SetFieldValue(pSObject,L"Status",pInfo->m_sTaskCompletedStatus);

			if (!pInfo->m_sLogComments.empty()) {
				CCTIAppExchange::SetFieldValue(pSObject,L"Description",pInfo->m_sLogComments);
			}

			CCTIAppExchange::SetFieldValue(pSObject,L"WhoId",pInfo->m_sLogWhoId);
			if (!CCTICallLog::IsALead(pInfo->m_sLogWhatId)) {
				CCTIAppExchange::SetFieldValue(pSObject,L"WhatId",pInfo->m_sLogWhatId);
			} else {
				if (!pInfo->m_sLogWhatId.empty()) 
					CCTILogger::Log(LOGLEVEL_MED,L"CCTIAppExchangeSaveThread::ThreadUpsertCallLog: The Who was a lead, so the selected What must be ignored.");
			}

			if (!bUpdating) {
				pSObject->Create();
			} else {
				pSObject->Update();
			}

			if (pSObject->Error==NO_SF_ERROR) {
				CCTILogger::Log(LOGLEVEL_MED,L"CCTIAppExchangeSaveThread::ThreadUpsertCallLog: Call log %s successfully.",bUpdating?L"updated":L"created");
				
				std::wstring sId = CCTIAppExchange::GetFieldValue(pSession,pSObject,L"Id");
				CSaveThreadInfo* pSavedLogInfo = new CSaveThreadInfo(pInfo->m_pUI, pInfo->m_sLogCallObjectId,sId);
				
				if (IsWindow(pInfo->m_hHiddenWindow))
				{
					PostMessage(pInfo->m_hHiddenWindow, WM_SAVE_RETURNED, 0, (LPARAM)pSavedLogInfo);
				} else delete pSavedLogInfo;
				
			} else {
				CCTILogger::Log(LOGLEVEL_LOW,L"CCTIAppExchangeSaveThread::ThreadUpsertCallLog: Error %s call log: %s.",bUpdating?L"updating":L"creating", pSObject->GetErrorMessage());
			}
		} catch (_com_error ce) {
			CCTILogger::Log(LOGLEVEL_LOW,L"CCTIAppExchangeSaveThread::ThreadUpsertCallLog: COM Error %s call log: %s.",bUpdating?L"updating":L"creating",(wchar_t*)pSession->GetErrorMessage());
		}
	}
}

void CCTIAppExchangeSaveThread::ThreadSaveUserExtension(ISForceSession4Ptr pSession,std::wstring& sUserId,std::wstring& sExtension)
{
	try {
		ISObject4Ptr pUser = pSession->CreateObject("User");
		CCTIAppExchange::SetFieldValue(pUser,"Id",sUserId);
		CCTIAppExchange::SetFieldValue(pUser,"Extension",sExtension);
		pUser->Update();

		CCTILogger::Log(LOGLEVEL_MED,L"CCTIAppExchangeSaveThread::ThreadSaveUserExtension: Updated user's extension to %s.",sExtension);
	} catch (_com_error ce) {
		CCTILogger::Log(LOGLEVEL_LOW,L"CCTIAppExchangeSaveThread::ThreadSaveUserExtension: COM error updating user extension: %s.",(wchar_t*)pSession->GetErrorMessage());
	}
}

void CCTIAppExchangeSaveThread::ThreadSaveUserParams(ISForceSession4Ptr pSession,CSaveThreadInfo* pThreadInfo)
{
	//Make arrays to hold the custom setup rows for creation.
	CComSafeArray<IDispatch*> arrayCustomSetups;
	arrayCustomSetups.Create((ULONG)pThreadInfo->m_parameters.size());

	try {
		//Is there a password in here?
		std::wstring sPassword;
		if (pThreadInfo->m_parameters.find(KEY_PASSWORD)!=pThreadInfo->m_parameters.end()) 
		{
			//We need to encrypt the password prior to sending it.
			sPassword = pThreadInfo->m_parameters[KEY_PASSWORD];
			if (sPassword.size()>0) {
				CCTILogger::Log(LOGLEVEL_MED,L"CCTIAppExchangeSaveThread::ThreadSaveUserParams: Getting encryption code.");
				_bstr_t bsCodeUrl = CCTIUtils::StringToBSTR(pThreadInfo->m_sInstance+CODE_SERVLET_PATH);
				pSession->SetCookie(bsCodeUrl,L"sid",CCTIUtils::StringToBSTR(pThreadInfo->m_sSid));
				_bstr_t bsCode = pSession->MakeHttpRequest(bsCodeUrl,KEY_GET,L"",L"",false,false);

				if (bsCode.length()>0) {
					CCTILogger::Log(LOGLEVEL_MED,L"CCTIAppExchangeSaveThread::ThreadSaveUserParams: Encrypting password.");

					//Reverse the key because the user-specific component of it is at the end of the key
					std::wstring sKey = CCTIUtils::ReverseString(CCTIUtils::BSTRToString(bsCode));

					//Put the encrypted password in the map
					pThreadInfo->m_parameters[KEY_PASSWORD] = CCTIAppExchange::EncryptString(sPassword,sKey);
				} else {
					CCTILogger::Log(LOGLEVEL_LOW,L"CCTIAppExchangeSaveThread::ThreadSaveUserParams: Unable to get encryption code from OfflineCode servlet.  Will not save unencrypted password.");
					pThreadInfo->m_parameters.erase(KEY_PASSWORD);
				}
			}
		}

		int nIndex = 0;
		for (PARAM_MAP::iterator it=pThreadInfo->m_parameters.begin();it!=pThreadInfo->m_parameters.end();it++) {
			std::wstring sKeyPath = USER_KEY_PATH+it->first;

			//Now the custom setup row
			ISObject4Ptr pCustomSetup = pSession->CreateObject("CustomSetup");
			CCTIAppExchange::SetFieldValue(pCustomSetup,"KeyPath",sKeyPath);
			CCTIAppExchange::SetFieldValue(pCustomSetup,"ParentId",pThreadInfo->m_sUserId);
			CCTIAppExchange::SetFieldValue(pCustomSetup,"Value",it->second.c_str());
			arrayCustomSetups.SetAt(nIndex, pCustomSetup);

			nIndex++;
		}
	} catch (_com_error ce) {
		CCTILogger::Log(LOGLEVEL_LOW,L"CCTIAppExchangeSaveThread::ThreadSaveUserParams: COM error setting up custom setup objects: %s.",(wchar_t*)pSession->GetErrorMessage());
	}

	CCTILogger::Log(LOGLEVEL_MED,L"CCTIAppExchangeSaveThread::ThreadSaveUserParams: Created SObjects; saving login data to Custom Setup tables.");

	try {
		CComVariant vCustomSetups(arrayCustomSetups.m_psa);
		pSession->Create(vCustomSetups,VARIANT_FALSE);
	} catch (_com_error ce) {
		CCTILogger::Log(LOGLEVEL_MED,L"CCTIAppExchangeSaveThread::ThreadSaveUserParams: COM error creating custom setup rows: %s.",(wchar_t*)pSession->GetErrorMessage());

		for (LONG i=arrayCustomSetups.GetLowerBound();i<=arrayCustomSetups.GetUpperBound();i++) {
			IDispatch* pDispObject = arrayCustomSetups.GetAt(i);
			ISObject4Ptr pObject = pDispObject;
			if (pObject!=NULL && pObject->Error!=NO_SF_ERROR) {
				CCTILogger::Log(LOGLEVEL_MED,L"CCTIAppExchangeSaveThread::ThreadSaveUserParams: Object error: %s.",pObject->GetErrorMessage());
			}
		}
	}
	CCTILogger::Log(LOGLEVEL_MED,L"CCTIAppExchangeSaveThread::ThreadSaveUserParams: Save complete.");
}

LRESULT CALLBACK CCTIAppExchangeSaveThread::HiddenWindowProc(HWND hwnd, UINT msg, WPARAM wparam, LPARAM lparam)
{
	if (msg==WM_SAVE_RETURNED) {
		CSaveThreadInfo* pInfo = (CSaveThreadInfo*)lparam;
		if (pInfo!=NULL) {
			CCTIUserInterface* pUI = pInfo->m_pUI;
			if (pUI) {
				pUI->UIUpdateSavedCallLog(pInfo->m_sLogCallObjectId, pInfo->m_sLogId);
			}
			delete pInfo;
		}
	}
	return DefWindowProc(hwnd, msg, wparam, lparam);
}

CSaveThreadInfo::CSaveThreadInfo(CCTIAppExchange* pAppExchange)
:m_hHiddenWindow(NULL)
{
	m_sSid = pAppExchange->GetSid();
	m_sInstance = pAppExchange->GetInstance();
	m_sUserId = pAppExchange->GetUserId();
	m_pUI = pAppExchange->GetCTIUserInterface();
}

CSaveThreadInfo::CSaveThreadInfo(CCTIAppExchange* pAppExchange, PARAM_MAP& parameters, HWND hHiddenWindow, CCTICallLog* pCallLog) :
m_parameters(parameters),m_hHiddenWindow(hHiddenWindow)
{
	m_sSid = pAppExchange->GetSid();
	m_sInstance = pAppExchange->GetInstance();
	m_sUserId = pAppExchange->GetUserId();
	m_pUI = pAppExchange->GetCTIUserInterface();

	if (pCallLog) {
		m_sLogCallObjectId = pCallLog->GetCallObjectId();
		m_sLogId = pCallLog->GetId();
		m_sLogLabel = pCallLog->GetLabel();
		m_sLogComments = pCallLog->GetComments();
		m_sLogDisposition = pCallLog->GetCallDisposition();
		m_nLogDuration = pCallLog->GetCallDuration();
		m_nLogCallType = pCallLog->GetCallType();
		m_sTaskCompletedStatus = pAppExchange->GetTaskCompletedStatus();
		m_sLogWhoId = pCallLog->GetSelectedWhoId();
		m_sLogWhatId = pCallLog->GetSelectedWhatId();
		m_bLogDirty = pCallLog->GetDirty();
	}
}

CSaveThreadInfo::CSaveThreadInfo(CCTIUserInterface* pUI, std::wstring sCallObjectId, std::wstring sId)
:m_hHiddenWindow(NULL), m_pUI(pUI)
{
	m_sLogCallObjectId = sCallObjectId;
	m_sLogId = sId;
}