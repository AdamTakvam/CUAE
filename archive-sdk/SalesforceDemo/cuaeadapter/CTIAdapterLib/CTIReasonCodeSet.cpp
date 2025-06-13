#include "StdAfx.h"
#include "ctireasoncodeset.h"
#include "ctiutils.h"

CCTIReasonCodeSet::CCTIReasonCodeSet(void)
{
}

CCTIReasonCodeSet::~CCTIReasonCodeSet(void)
{
}

CCTIReasonCodeSet::CCTIReasonCodeSet(std::wstring sId, std::wstring sLabel)
:CCTIIdLabelObject(sId,sLabel)
{

}

MSXML2::IXMLDOMDocumentFragmentPtr CCTIReasonCodeSet::SerializeToXML(MSXML2::IXMLDOMDocumentPtr pXMLDoc)
{
	MSXML2::IXMLDOMDocumentFragmentPtr pFragment = pXMLDoc->createDocumentFragment();
	if (GetVisible()) {
		MSXML2::IXMLDOMElementPtr pXmlSet= pXMLDoc->createElement("CTIReasonCodeSet");

		AddAttributesToElement(pXMLDoc,pXmlSet);

		for (PARAM_MAP::iterator it=m_mapReasonCodes.begin();it!=m_mapReasonCodes.end();it++) {
			MSXML2::IXMLDOMElementPtr pXmlReasonCode= pXMLDoc->createElement("CTIReasonCode");

			MSXML2::IXMLDOMAttributePtr pAttribute = pXMLDoc->createAttribute(KEY_ID);
			//The javascript totally barfs if there's an apostrophe in there, so we'll escape it here
			//It'll get unescaped later for display...
			std::wstring sId = CCTIUtils::SearchAndReplace(it->first,L"'",L"\\'");
			pAttribute->value = CCTIUtils::StringToBSTR(sId);
			pXmlReasonCode->setAttributeNode(pAttribute);
			pAttribute.Release();

			pAttribute = pXMLDoc->createAttribute(KEY_LABEL);
			pAttribute->value = CCTIUtils::StringToBSTR(it->second);
			pXmlReasonCode->setAttributeNode(pAttribute);
			pAttribute.Release();

			if (!m_sSelectedReasonCode.empty()) {
				pAttribute = pXMLDoc->createAttribute(KEY_SELECTED);
				pAttribute->value = CCTIUtils::StringToBSTR(m_sSelectedReasonCode==it->first?KEY_TRUE:KEY_FALSE);
				pXmlReasonCode->setAttributeNode(pAttribute);
				pAttribute.Release();
			}

			pXmlSet->appendChild(pXmlReasonCode);
			pXmlReasonCode.Release();
		}

		pFragment->appendChild(pXmlSet);
		pXmlSet.Release();
	}
	return pFragment;
}

void CCTIReasonCodeSet::SetReasonCodes(PARAM_MAP& mapReasonCodes)
{
	m_mapReasonCodes = mapReasonCodes;
	SetSelectedReasonCode(L"");
}

void CCTIReasonCodeSet::SelectFirstReasonCode()
{
	if (!m_mapReasonCodes.empty()) 
	{
		PARAM_MAP::iterator it = m_mapReasonCodes.begin();
		SetSelectedReasonCode(it->first);
	}
}