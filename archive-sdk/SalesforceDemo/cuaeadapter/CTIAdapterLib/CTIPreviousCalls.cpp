#include "stdafx.h"
#include "CTIPreviousCalls.h"
#include <Windows.h>

#define MAX_PREVIOUS_CALLS 3

CCTIPreviousCalls::CCTIPreviousCalls()
{
	SetVisible(false);
	SetOpen(false);
}

CCTIPreviousCalls::~CCTIPreviousCalls()
{
	Reset();
}

void CCTIPreviousCalls::Reset()
{
	AutoLock autoLock(this);
	for (CallLogList::iterator it=m_listCallLogs.begin();it!=m_listCallLogs.end();it++)
	{
		CCTICallLog* pLog = *it;
		delete pLog;
	}
	m_listCallLogs.clear();
	SetOpen(false);
}

void CCTIPreviousCalls::SetOpen(bool bOpen)
{
	m_bOpen = bOpen;
	SetAttribute(KEY_OPEN,m_bOpen);
}

bool CCTIPreviousCalls::GetOpen()
{
	return m_bOpen;
}

void CCTIPreviousCalls::AddCallLog(CCTICallLog* pLog)
{
	AutoLock autoLock(this);
	//We'll keep 2 more of the previous logs than we can actually show, just in case a log has to be picked back out again due to wrap-up mode
	if (m_listCallLogs.size()>=MAX_PREVIOUS_CALLS+2) {
		CCTICallLog* pOldLog = m_listCallLogs.back();
		m_listCallLogs.pop_back();
		delete pOldLog;
	}

	m_listCallLogs.push_front(pLog);
	pLog->SetVisible(true);
}

void CCTIPreviousCalls::RemoveCallLog(CCTICallLog* pLog)
{
	AutoLock autoLock(this);
	m_listCallLogs.remove(pLog);
}

MSXML2::IXMLDOMDocumentFragmentPtr CCTIPreviousCalls::SerializeToXML(MSXML2::IXMLDOMDocumentPtr pXMLDoc) 
{
	AutoLock autoLock(this);
	MSXML2::IXMLDOMDocumentFragmentPtr pFragment = pXMLDoc->createDocumentFragment();
	if (GetVisible()) {
		MSXML2::IXMLDOMElementPtr pXmlCalls= pXMLDoc->createElement(GetTag());

		AddAttributesToElement(pXMLDoc,pXmlCalls);

		SYSTEMTIME st;
        GetLocalTime(&st);
		SetReportDate(st.wYear, st.wMonth, st.wDay);

		int nCallLogNumber = 1;
		for (CallLogList::iterator it=m_listCallLogs.begin();it!=m_listCallLogs.end();it++)
		{
			CCTICallLog* pLog = *it;
			
			AddChildIfVisible(pXMLDoc, pXmlCalls, pLog);

			//We only want to show up to MAX_PREVIOUS_CALLS, even if we have more in memory
			nCallLogNumber++;
			if (nCallLogNumber>MAX_PREVIOUS_CALLS) break;
		}

		pFragment->appendChild(pXmlCalls);
		pXmlCalls.Release();
	}
	return pFragment;
}

CCTICallLog* CCTIPreviousCalls::GetCallLogByCallId(std::wstring& sCallObjectId)
{
	AutoLock autoLock(this);
	for (CallLogList::iterator it=m_listCallLogs.begin();it!=m_listCallLogs.end();it++)
	{
		CCTICallLog* pLog = *it;
		if (pLog->GetCallObjectId()==sCallObjectId) return pLog;
	}
	return NULL;
}