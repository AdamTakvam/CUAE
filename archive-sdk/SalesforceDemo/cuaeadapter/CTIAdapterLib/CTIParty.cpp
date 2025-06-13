#include "StdAfx.h"
#include ".\ctiparty.h"
#include "CTIUserInterface.h"
#include <algorithm>

CCTIParty::CCTIParty(CCTIUserInterface* pUI,std::wstring sANI, int nPartyType)
:m_pUI(pUI)
{
	SetANI(sANI);
	SetType(nPartyType);
	CCTIInfoField* pANI = new CCTIInfoField(KEY_ANI);
	pANI->SetValue(sANI);
	AddInfoField(pANI);
}

CCTIParty::~CCTIParty(void)
{
	ClearInfoFields();
	ClearRelatedObjectSets();
}

MSXML2::IXMLDOMDocumentFragmentPtr CCTIParty::SerializeToXML(MSXML2::IXMLDOMDocumentPtr pXMLDoc) 
{
	MSXML2::IXMLDOMDocumentFragmentPtr pFragment = pXMLDoc->createDocumentFragment();
	if (GetVisible()) {
		MSXML2::IXMLDOMElementPtr pXmlParty= pXMLDoc->createElement("CTIParty");

		AddAttributesToElement(pXMLDoc,pXmlParty);

		for (InfoFieldList::iterator it=m_listInfoFields.begin();it!=m_listInfoFields.end();it++) {
			CCTIInfoField* pInfoField = *it;
			AddChildIfVisible(pXMLDoc, pXmlParty, pInfoField);
		}

		for (RelObjSetList::iterator it=m_listRelatedObjectSets.begin();it!=m_listRelatedObjectSets.end();it++) {
			CCTIRelatedObjectSet* pSet = *it;
			AddChildIfVisible(pXMLDoc, pXmlParty, pSet);
		}

		pFragment->appendChild(pXmlParty);
		pXmlParty.Release();
	}
	return pFragment;
}

std::wstring CCTIParty::GetInfoFieldValue(std::wstring sId)
{
	for (InfoFieldList::iterator it = m_listInfoFields.begin();it!=m_listInfoFields.end();it++) {
		CCTIInfoField* pInfoField = *it;
		if (pInfoField->GetId()==sId) {
			return pInfoField->GetValue();
		}
	}
	return L"";
}

bool CCTIParty::SetInfoFieldValue(std::wstring sId, std::wstring sValue)
{
	for (InfoFieldList::iterator it = m_listInfoFields.begin();it!=m_listInfoFields.end();it++) {
		CCTIInfoField* pInfoField = *it;
		if (pInfoField->GetId()==sId) {
			pInfoField->SetValue(sValue);
			return true;
		}
	}
	return false;
}

void CCTIParty::AddInfoField(CCTIInfoField* pInfoField)
{
	//If the info field already exists, just set its value
	if (SetInfoFieldValue(pInfoField->GetId(),pInfoField->GetValue())) {
		//It already exists and we set the new value, so just delete the input info field
		delete pInfoField;
	} else {
		//It doesn't already exist, so add it
		pInfoField->SetLabel(m_pUI->GetInfoFieldLabel(pInfoField->GetId()));
		m_listInfoFields.push_back(pInfoField);
	}
}

void CCTIParty::SetInfoFields(PARAM_MAP& mapInfoFields, StringList* pListLayoutInfoFields)
{
	ClearInfoFields();

	//Add the info fields in the order that the layout specifies them in
	for (StringList::iterator itList = pListLayoutInfoFields->begin(); itList!= pListLayoutInfoFields->end(); itList++) {
		std::wstring sInfoFieldId = *itList;
		PARAM_MAP::iterator itMap = mapInfoFields.find(sInfoFieldId);
		if (itMap!=mapInfoFields.end()) {
			//It's a standard field so we don't need to get the label -- it's built in
			CCTIInfoField* pInfoField = new CCTIInfoField(itMap->first,L"");
			pInfoField->SetValue(itMap->second);
			m_listInfoFields.push_back(pInfoField);
		}
	}

	//Now add any info fields that are not in the layout, but that we have labels for 
	//(which indicates that they are custom info fields that could not possibly be in the layout)
	//(we always show custom info fields)
	for (PARAM_MAP::iterator it=mapInfoFields.begin();it!=mapInfoFields.end();it++) {
		std::wstring sLabel = m_pUI->GetInfoFieldLabel(it->first);
		if (!sLabel.empty()) {
			CCTIInfoField* pInfoField = new CCTIInfoField(it->first,sLabel);
			pInfoField->SetValue(it->second);
			m_listInfoFields.push_back(pInfoField);
		}
	}
}

void CCTIParty::ClearInfoFields()
{
	for (InfoFieldList::iterator it=m_listInfoFields.begin();it!=m_listInfoFields.end();it++) {
		CCTIInfoField* pInfoField = *it;
		delete pInfoField;
	}
	m_listInfoFields.clear();
}

void CCTIParty::AddRelatedObjectSets(RelObjSetList& listRelatedObjectSets)
{
	for (RelObjSetList::iterator it=listRelatedObjectSets.begin();it!=listRelatedObjectSets.end();it++) {
		CCTIRelatedObjectSet* pSet = *it;
		AddRelatedObjectSet(pSet);
	}
}

void CCTIParty::AddRelatedObjectSet(CCTIRelatedObjectSet* pSet)
{
	//See if there's an existing related object set in this party for this type of object
	CCTIRelatedObjectSet* pExistingSet = GetRelatedObjectSet(pSet->GetObjectName());

	if (pExistingSet==NULL) {
		//No existing set -- just push in this one
		m_listRelatedObjectSets.push_back(pSet);
	} else {
		//There's an existing set -- merge the contents of the old set into the new one, then delete the old one, keeping the new one
		pSet->Merge(pExistingSet);
		m_listRelatedObjectSets.remove(pExistingSet);
		m_listRelatedObjectSets.push_back(pSet);
		delete pExistingSet;
	}
}

CCTIRelatedObjectSet*  CCTIParty::GetRelatedObjectSet(std::wstring& sEntityName)
{
	for (RelObjSetList::iterator it=m_listRelatedObjectSets.begin();it!=m_listRelatedObjectSets.end();it++) {
		CCTIRelatedObjectSet* pSet = *it;
		if (pSet->GetObjectName()==sEntityName) {
			return pSet;
		}
	}
	return NULL;
}

void CCTIParty::ClearRelatedObjectSets()
{
	for (RelObjSetList::iterator it=m_listRelatedObjectSets.begin();it!=m_listRelatedObjectSets.end();it++) {
		CCTIRelatedObjectSet* pSet = *it;
		delete pSet;
	}
	m_listRelatedObjectSets.clear();
}

void CCTIParty::SetType(int nType)
{
	m_nType = nType;
	switch (m_nType) {
		case PARTYTYPE_NONE:
			{
				RemoveAttribute(KEY_TYPE);
				break;
			}
		case PARTYTYPE_ORIGINAL_CALLER:
			{
				SetAttribute(KEY_TYPE,KEY_ORIGINAL_CALLER);
				break;
			}
		case PARTYTYPE_TRANSFERRED_FROM:
			{
				SetAttribute(KEY_TYPE,KEY_TRANSFERRED_FROM);
				break;
			}
		case PARTYTYPE_CONFERENCED_FROM:
			{
				SetAttribute(KEY_TYPE,KEY_CONFERENCED_FROM);
				break;
			}
		case PARTYTYPE_CONFERENCED_PARTY:
			{
				SetAttribute(KEY_TYPE,KEY_CONFERENCED_PARTY);
				break;
			}
	}
}