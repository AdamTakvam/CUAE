#include "StdAfx.h"
#include "ctirelatedobjectset.h"
#include "CTIRelatedObject.h"

CCTIRelatedObjectSet::CCTIRelatedObjectSet()
{
	SetVisible(true);
}

CCTIRelatedObjectSet::CCTIRelatedObjectSet(std::wstring sId, std::wstring sSingularLabel, std::wstring sPluralLabel):
CCTIIdLabelObject(sId,L""),
m_sSingularLabel(sSingularLabel),
m_sPluralLabel(sPluralLabel)
{
}

CCTIRelatedObjectSet::~CCTIRelatedObjectSet(void)
{
	for (RelObjectList::iterator it=m_listRelatedObjects.begin();it!=m_listRelatedObjects.end();it++) {
		CCTIRelatedObject* pRelatedObject = *it;
		delete pRelatedObject;
	}
	m_listRelatedObjects.clear();
	m_mapRelatedObjects.clear();
}

MSXML2::IXMLDOMDocumentFragmentPtr CCTIRelatedObjectSet::SerializeToXML(MSXML2::IXMLDOMDocumentPtr pXMLDoc) 
{
	MSXML2::IXMLDOMDocumentFragmentPtr pFragment = pXMLDoc->createDocumentFragment();
	if (GetVisible()) {
		MSXML2::IXMLDOMElementPtr pXmlSet= pXMLDoc->createElement("CTIRelatedObjectSet");

		//Set the label to plural or singular as necessary first
		if (m_listRelatedObjects.size()>1) {
			SetLabel(m_sPluralLabel);
		} else {
			SetLabel(m_sSingularLabel);
		}

		AddAttributesToElement(pXMLDoc,pXmlSet);

		for (RelObjectList::iterator it=m_listRelatedObjects.begin();it!=m_listRelatedObjects.end();it++) {
			CCTIRelatedObject* pRelatedObject = *it;
			AddChildIfVisible(pXMLDoc, pXmlSet, pRelatedObject);
		}

		pFragment->appendChild(pXmlSet);
		pXmlSet.Release();
	}
	return pFragment;
}

void CCTIRelatedObjectSet::AddRelatedObject(CCTIRelatedObject* pRelatedObject)
{
	m_listRelatedObjects.push_back(pRelatedObject);
	m_mapRelatedObjects[pRelatedObject->GetId().substr(0,15)] = pRelatedObject;
}

void CCTIRelatedObjectSet::Merge(CCTIRelatedObjectSet* pSet)
{
	if (pSet->GetObjectName()==GetObjectName()) {
		RelObjectList listDupes; //A list of duplicate objects to "give back" to the merged set, which will probably be deleted later anyway
		RelObjectList* pSetObjects = pSet->GetRelatedObjects();
		for (RelObjectList::iterator it=pSetObjects->begin();it!=pSetObjects->end();) {
			CCTIRelatedObject* pRelatedObject = *it;
			if (!ContainsRelatedObject(pRelatedObject->GetId())) {
				//Take the related object from the other set
				AddRelatedObject(pRelatedObject);
				//This will move the iterator to the next member of the set
				it = pSetObjects->erase(it);
			} else {
				//Leave it in the merged set
				it++;
			}
		}
	}
}

CCTIRelatedObject* CCTIRelatedObjectSet::GetFirstObject()
{
	if (!m_listRelatedObjects.empty()) {
		RelObjectList::iterator it = m_listRelatedObjects.begin();
		return *it;
	} else return NULL;
}

bool CCTIRelatedObjectSet::ContainsRelatedObject(std::wstring sId)
{
	return (m_mapRelatedObjects.find(sId.substr(0,15))!=m_mapRelatedObjects.end());
}
