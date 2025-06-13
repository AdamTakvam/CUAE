#include "StdAfx.h"
#include "ctiobject.h"
#include "ctiutils.h"

CCTIObject::CCTIObject()
:m_bVisible(true)
{
}

CCTIObject::~CCTIObject()
{
}

MSXML2::IXMLDOMDocumentFragmentPtr CCTIObject::SerializeToXML(MSXML2::IXMLDOMDocumentPtr pXMLDoc) 
{
	//Synchronize this method
	AutoLock autoLock(this);

	MSXML2::IXMLDOMDocumentFragmentPtr pFragment = pXMLDoc->createDocumentFragment();
	if (GetVisible()) {
		MSXML2::IXMLDOMElementPtr pXmlObject= pXMLDoc->createElement(GetTag());

		AddAttributesToElement(pXMLDoc,pXmlObject);

		pFragment->appendChild(pXmlObject);
		pXmlObject.Release();
	}
	return pFragment;
}

void CCTIObject::RemoveAttribute(std::wstring sName)
{
	//Synchronize this method
	AutoLock autoLock(this);

	m_mapAttributes.erase(sName);
}

void CCTIObject::SetAttribute(std::wstring sName, const wchar_t* sValue)
{
	SetAttribute(sName,std::wstring(sValue));
}

void CCTIObject::SetAttribute(std::wstring sName, std::wstring sValue)
{
	//Synchronize this method
	AutoLock autoLock(this);

	m_mapAttributes[sName]=sValue;
}

void CCTIObject::SetAttribute(std::wstring sName, bool bValue)
{
	SetAttribute(sName,bValue?KEY_TRUE:KEY_FALSE);
}

void CCTIObject::SetAttribute(std::wstring sName, int nValue)
{
	SetAttribute(sName,CCTIUtils::IntToString(nValue));
}

std::wstring CCTIObject::GetAttribute(std::wstring sName)
{
	//Synchronize this method
	AutoLock autoLock(this);

	std::wstring sReturnValue;
	sReturnValue = m_mapAttributes[sName];

	return sReturnValue;
}

void CCTIObject::SetVisible(bool bVisible)
{
	this->m_bVisible = bVisible;
}

bool CCTIObject::GetVisible()
{
	return m_bVisible;
}

void CCTIObject::AddChildIfVisible(MSXML2::IXMLDOMDocumentPtr pXMLDoc, MSXML2::IXMLDOMElementPtr pXmlElement, CCTIObject* pChild) 
{
	if (pChild && pChild->GetVisible()) {
		//Output the info field.
		MSXML2::IXMLDOMDocumentFragmentPtr pChildFragment = pChild->SerializeToXML(pXMLDoc);
		pXmlElement->appendChild(pChildFragment);
		pChildFragment.Release();
	}
}

void CCTIObject::AddAttributesToElement(MSXML2::IXMLDOMDocumentPtr pXMLDoc, MSXML2::IXMLDOMElementPtr pElement)
{
	//Synchronize this method
	AutoLock autoLock(this);

	for (PARAM_MAP::iterator it=m_mapAttributes.begin();it!=m_mapAttributes.end();it++) {
		pElement->setAttribute(CCTIUtils::StringToBSTR(it->first),CCTIUtils::StringToBSTR(it->second));
	}
}