#include "StdAfx.h"
#include "ctirelatedobject.h"
#include "CTIField.h"

CCTIRelatedObject::CCTIRelatedObject(std::wstring sId, std::wstring sLabel)
:CCTIIdLabelObject(sId,sLabel)
{
	SetVisible(true);
}

CCTIRelatedObject::~CCTIRelatedObject(void)
{
	
}

void CCTIRelatedObject::AddField(CCTIField* pField)
{
	if (pField) m_listFields.push_back(pField);
}

std::wstring CCTIRelatedObject::GetFieldValue(std::wstring sFieldId)
{
	for (FieldList::iterator it=m_listFields.begin();it!=m_listFields.end();it++) {
		CCTIField* pField = *it;
		if (pField->GetId()==sFieldId) return pField->GetValue();
	}
	return L"";
}

MSXML2::IXMLDOMDocumentFragmentPtr CCTIRelatedObject::SerializeToXML(MSXML2::IXMLDOMDocumentPtr pXMLDoc) 
{
	MSXML2::IXMLDOMDocumentFragmentPtr pFragment = pXMLDoc->createDocumentFragment();
	if (GetVisible()) {
		MSXML2::IXMLDOMElementPtr pXmlObject = pXMLDoc->createElement("CTIRelatedObject");

		AddAttributesToElement(pXMLDoc,pXmlObject);

		for (std::list<CCTIField*>::iterator it=m_listFields.begin();it!=m_listFields.end();it++) {
			CCTIField* pField = *it;
			AddChildIfVisible(pXMLDoc, pXmlObject, pField);
		}

		pFragment->appendChild(pXmlObject);
		pXmlObject.Release();
	}
	return pFragment;
}