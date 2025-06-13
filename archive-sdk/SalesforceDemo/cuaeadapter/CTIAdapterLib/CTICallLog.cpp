#include "stdafx.h"
#include "CTICallLog.h"
#include "CTILine.h"
#include "CTIUserInterface.h"

CCTICallLog::CCTICallLog(CCTIUserInterface* pUI, std::wstring sLabel, int nLineNumber, bool bOpen)
:CCTIIdLabelObject(L"",sLabel),m_pUI(pUI)
,m_whoNone(LOG_OBJECTTYPE_WHO,KEY_WHO_NONE,L"",L"",L"")
,m_whatNone(LOG_OBJECTTYPE_WHAT,KEY_WHAT_NONE,L"",L"",L"")
,m_bDirty(true)
,m_bCallIsActive(true)
{
	SetOpen(bOpen);
	SetLineNumber(nLineNumber);
	SetSaveOnEnd(false);
	SetVisible(false);
}

CCTICallLog::~CCTICallLog()
{
	//Synchronize this method
	AutoLock autoLock(this);

	for (WhoWhatMap::iterator itWho=m_mapWhos.begin();itWho!=m_mapWhos.end();itWho++) {
		delete itWho->second;
	}
	m_mapWhos.clear();

	for (WhoWhatMap::iterator itWhat=m_mapWhats.begin();itWhat!=m_mapWhats.end();itWhat++) {
		delete itWhat->second;
	}
	m_mapWhats.clear();
}

void CCTICallLog::SetLineNumber(int nLineNumber)
{
	m_nLineNumber = nLineNumber;
	SetAttribute(KEY_LINE,m_nLineNumber);
}

int CCTICallLog::GetLineNumber()
{
	return m_nLineNumber;
}

void CCTICallLog::SetOpen(bool bOpen)
{
	m_bOpen = bOpen;
	SetAttribute(KEY_OPEN,m_bOpen);
}

bool CCTICallLog::GetOpen()
{
	return m_bOpen;
}

void CCTICallLog::SetDirty(bool bDirty)
{
	m_bDirty = bDirty;
}

bool CCTICallLog::GetDirty()
{
	return m_bDirty;
}

void CCTICallLog::SetCallIsActive(bool bCallIsActive)
{
	m_bCallIsActive = bCallIsActive;
}

bool CCTICallLog::GetCallIsActive()
{
	return m_bCallIsActive;
}

bool CCTICallLog::IsAWho(std::wstring& sId) {
	//These first 3 characters tell us what type of object this ID refers to (the ID could also be KEY_WHO_NONE).
	std::wstring sEntityId = sId.substr(0,3);
	return (sEntityId==KEY_003 || sEntityId==KEY_00Q || sEntityId==KEY_WHO);
}

bool CCTICallLog::IsALead(std::wstring& sId) {
	return CCTIUtils::StringBeginsWith(sId,std::wstring(KEY_00Q));
}

bool CCTICallLog::IsAUser(std::wstring& sId) {
	return CCTIUtils::StringBeginsWith(sId,std::wstring(KEY_005));
}

MSXML2::IXMLDOMDocumentFragmentPtr CCTICallLog::SerializeToXML(MSXML2::IXMLDOMDocumentPtr pXMLDoc)
{
	//Synchronize this method
	AutoLock autoLock(this);

	MSXML2::IXMLDOMDocumentFragmentPtr pFragment = pXMLDoc->createDocumentFragment();
	if (GetVisible()) {
		MSXML2::IXMLDOMElementPtr pXmlLog = pXMLDoc->createElement(GetTag());

		AddAttributesToElement(pXMLDoc,pXmlLog);

		if (m_mapWhos.size()>0) {
			AddChildIfVisible(pXMLDoc,pXmlLog,&m_whoNone);
		}
		for (WhoWhatMap::iterator itWho=m_mapWhos.begin();itWho!=m_mapWhos.end();itWho++) {
			AddChildIfVisible(pXMLDoc,pXmlLog,itWho->second);
		}

		//If the who is a lead, then we can't have whats, so we'll hide them
		if (!IsALead(m_sSelectedWhoId)) {
			if (m_mapWhats.size()>0) {
				AddChildIfVisible(pXMLDoc,pXmlLog,&m_whatNone);
			}
			for (WhoWhatMap::iterator itWhat=m_mapWhats.begin();itWhat!=m_mapWhats.end();itWhat++) {
				AddChildIfVisible(pXMLDoc,pXmlLog,itWhat->second);
			}
		}

		MSXML2::IXMLDOMElementPtr pXmlComments = pXMLDoc->createElement("CTIComments");
		pXmlComments->setAttribute(KEY_VALUE,CCTIUtils::StringToBSTR(m_sComments));
		pXmlLog->appendChild(pXmlComments);
		
		pFragment->appendChild(pXmlLog);

		pXmlLog.Release();
	}
	return pFragment;
}

int CCTICallLog::AddObject(std::wstring& sId, std::wstring& sObjectLabel, std::wstring& sObjectName, std::wstring& sEntityName, bool bAttachToCall, bool bAutoSelect)
{
	//Synchronize this method
	AutoLock autoLock(this);

	int nObjectType = 0;
	//Just take the first 15 wchar_ts of the Id, which is all that actually counts
	sId = sId.substr(0,15);	

	if (!IsAUser(sId)) {
						
		if (IsAWho(sId)) {
			//It's a who
			CCTIWhoWhat* pObject = new CCTIWhoWhat(LOG_OBJECTTYPE_WHO,sId,sObjectLabel,sObjectName,sEntityName);

			//Delete the object if there's already one in there for some reason (the name might have changed, for example)...
			if (m_mapWhos.find(sId)!=m_mapWhos.end()) {
				delete m_mapWhos[sId];
				bAutoSelect = m_sSelectedWhoId==sId;
			}
			m_mapWhos[sId]=pObject;

			nObjectType = LOG_OBJECTTYPE_WHO;
		} else {
			//It's a what
			CCTIWhoWhat* pObject = new CCTIWhoWhat(LOG_OBJECTTYPE_WHAT,sId,sObjectLabel,sObjectName,sEntityName);

			//Delete the object if there's already one in there for some reason (the name might have changed, for example)...
			if (m_mapWhats.find(sId)!=m_mapWhats.end()) {
				bAutoSelect = m_sSelectedWhatId==sId;	
				delete m_mapWhats[sId];
			}
			m_mapWhats[sId]=pObject;

			nObjectType = LOG_OBJECTTYPE_WHAT;
		}
		if (bAutoSelect) {
			SelectObject(sId,bAttachToCall);
		}
	}

	return nObjectType;
}

void CCTICallLog::SetComments(std::wstring sComments) 
{ 
	m_sComments = sComments;

	if (GetCallIsActive()) {
		PARAM_MAP mapAttachedData;
		mapAttachedData[KEY_COMMENTS] = sComments;
		std::wstring sCallObjectId = m_pUI->GetCallObjectIdFromLine(GetLineNumber());
		m_pUI->CallAttachData(sCallObjectId,mapAttachedData);
	}

	SetDirty(true);
}

void CCTICallLog::AddRelatedObjects(RelObjSetList* listRelObjSets)
{
	int nWhos = 0;
	int nWhats = 0;

	//First see how many whos and whats we've got
	for (RelObjSetList::iterator itSet=listRelObjSets->begin();itSet!=listRelObjSets->end();itSet++) {
		CCTIRelatedObjectSet* pSet = *itSet;
		
		RelObjectList* pListRelObjects = pSet->GetRelatedObjects();
		for (RelObjectList::iterator itObj=pListRelObjects->begin();itObj!=pListRelObjects->end();itObj++) {
			CCTIRelatedObject* pObj = *itObj;
			std::wstring sId = pObj->GetId();
			if (IsAWho(sId)) {
				nWhos++;
				if (nWhos>2) break;
			} else {
				nWhats++;
				if (nWhats>2) break;
			}
		}
	}

	//If there's exactly one who or one what, or exactly one of each, we'll default-select the who and the what
	if ((nWhos+nWhats)==1 || (nWhos*nWhats)==1) {
		for (RelObjSetList::iterator itSet=listRelObjSets->begin();itSet!=listRelObjSets->end();itSet++) {
			CCTIRelatedObjectSet* pSet = *itSet;
			
			RelObjectList* pListRelObjects = pSet->GetRelatedObjects();
			for (RelObjectList::iterator itObj=pListRelObjects->begin();itObj!=pListRelObjects->end();itObj++) {
				CCTIRelatedObject* pObj = *itObj;
				std::wstring sId = pObj->GetId();

				AddObject(pObj->GetId(),pSet->GetSingularLabel(),pObj->GetLabel(),pSet->GetObjectName(),false);
			}
		}
		
		AttachLogObjectsToCall();
	} else {
		CCTILogger::Log(LOGLEVEL_MED,L"CCTICallLog::AddRelatedObjects: There are more related objects than can be associated with this call log.  Leaving call log blank.");
	}
}

void CCTICallLog::SelectObject(std::wstring sId, bool bAttachToCall)
{
	//Synchronize this method
	AutoLock autoLock(this);

	if (!sId.empty()) 
	{
		if (IsAWho(sId)) {
			//It's a who
			if (sId!=KEY_WHO_NONE) {
				CCTIWhoWhat* pWhoWhat = (m_sSelectedWhoId.length()>0)?m_mapWhos[m_sSelectedWhoId]:NULL;
				if (pWhoWhat) pWhoWhat->SetSelected(false);
				CCTIWhoWhat* pNewWhoWhat = m_mapWhos[sId];
				if (pNewWhoWhat) pNewWhoWhat->SetSelected(true);
				m_sSelectedWhoId = sId;

				//If it's a lead, deselect the what
				if (IsALead(sId)) {
					DeselectObject(LOG_OBJECTTYPE_WHAT);
				}
			} else {
				DeselectObject(LOG_OBJECTTYPE_WHO);
			}
		} else {
			//It's a what
			if (sId!=KEY_WHAT_NONE) {
				//If the who is a lead, first deselect the who
				if (IsALead(m_sSelectedWhoId)) DeselectObject(LOG_OBJECTTYPE_WHO);

				CCTIWhoWhat* pWhoWhat = (m_sSelectedWhatId.length()>0)?m_mapWhats[m_sSelectedWhatId]:NULL;
				if (pWhoWhat) pWhoWhat->SetSelected(false);
				CCTIWhoWhat* pNewWhoWhat = m_mapWhats[sId];
				if (pNewWhoWhat) pNewWhoWhat->SetSelected(true);
				m_sSelectedWhatId=sId;
			} else {
				DeselectObject(LOG_OBJECTTYPE_WHAT);
			}
		}

		if (bAttachToCall) AttachLogObjectsToCall();
		SetDirty(true);
	}
}

void CCTICallLog::DeselectObject(int nWhoOrWhat)
{
	//Synchronize this method
	AutoLock autoLock(this);

	if (nWhoOrWhat==LOG_OBJECTTYPE_WHO) {
		CCTIWhoWhat* pWhoWhat = (m_sSelectedWhoId.length()>0)?m_mapWhos[m_sSelectedWhoId]:NULL;
		if (pWhoWhat) {
			pWhoWhat->SetSelected(false);
			m_sSelectedWhoId=L"";
		}
	} else {
		CCTIWhoWhat* pWhoWhat = (m_sSelectedWhatId.length()>0)?m_mapWhats[m_sSelectedWhatId]:NULL;
		if (pWhoWhat)
		{
			pWhoWhat->SetSelected(false);
			m_sSelectedWhatId=L"";
		}
	}
	AttachLogObjectsToCall();
	SetDirty(true);
}

void CCTICallLog::AttachLogObjectsToCall()
{
	//If the ID is not empty then the call must have ended, which means we have no active call to attach this data to.
	if (GetCallIsActive()) {
		std::wstring sCallObjectId = m_pUI->GetCallObjectIdFromLine(GetLineNumber());

		CCTIWhoWhat* pSelectedWho = (m_sSelectedWhoId.length()>0)?m_mapWhos[m_sSelectedWhoId]:NULL;
		CCTIWhoWhat* pSelectedWhat = (m_sSelectedWhatId.length()>0)?m_mapWhats[m_sSelectedWhatId]:NULL;
		PARAM_MAP mapAttachedData;

		//We'll attach the related objects in a single attached item, separating the two IDs by a slash (which you are guaranteed never to see in an ID)
		if (pSelectedWho) {
			mapAttachedData[KEY_LOG_WHO] = pSelectedWho->GetObjectType() + L":" + m_sSelectedWhoId;
		} else m_pUI->CallUnattachData(sCallObjectId,KEY_LOG_WHO);

		if (pSelectedWhat) {
			mapAttachedData[KEY_LOG_WHAT] = pSelectedWhat->GetObjectType() + L":" + m_sSelectedWhatId;
		} else m_pUI->CallUnattachData(sCallObjectId,KEY_LOG_WHAT);

		m_pUI->CallAttachData(sCallObjectId,mapAttachedData);
	}
}

void CCTICallLog::SetCallObjectId(std::wstring sCallObjectId) 
{ 
	//We also set the attribute so it will be emitted in the XML
	SetAttribute(KEY_CALL_OBJECT_ID,sCallObjectId);
	m_sCallObjectId = sCallObjectId; 
}

std::wstring CCTICallLog::GetCallObjectId() 
{ 
	return m_sCallObjectId; 
}

CCTIWhoWhat::CCTIWhoWhat(int nWhoOrWhat,std::wstring sId,std::wstring sObjectLabel,std::wstring sObjectName,std::wstring sEntityName)
:m_nWhoOrWhat(nWhoOrWhat),m_bSelected(false)
{
	SetAttribute(KEY_ID,sId);
	SetAttribute(KEY_OBJECT_LABEL,sObjectLabel);
	SetAttribute(KEY_OBJECT_NAME,sObjectName);
	SetAttribute(KEY_ENTITY_NAME,sEntityName);
	SetVisible(true);
}

void CCTIWhoWhat::SetSelected(bool bSelected)
{
	m_bSelected = bSelected;
	SetAttribute(KEY_SELECTED,bSelected);
}
