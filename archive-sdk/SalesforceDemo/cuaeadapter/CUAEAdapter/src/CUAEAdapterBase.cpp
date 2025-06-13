#include "stdafx.h"
#include "CUAEAdapterBase.h"
#include "CUAEUserInterface.h"
#include "CTIUtils.h"

STDMETHODIMP CUAEAdapterBase::UIAction(BSTR message)
{
	m_pUI->UIParseIncomingXMLMessage(message);

	return S_OK;
}

STDMETHODIMP CUAEAdapterBase::GetAdapterName(BSTR* bsName)
{
	*bsName = SysAllocString(L"Cisco CUAE Salesforce.com CTI Adapter");

	return S_OK;
}

STDMETHODIMP CUAEAdapterBase::GetAdapterVersion(BSTR* bsName)
{
	*bsName = SysAllocString(L"1.0");

	return S_OK;
}

void CUAEAdapterBase::SendUIRefreshEvent(_bstr_t xml)
{
	CCTILogger::Log(LOGLEVEL_HIGH,L"Sending XML (len %d): %s",xml.length(),(wchar_t*)xml);
	_ISalesforceCTIAdapterEvents_UIRefresh(xml);
}

HRESULT CUAEAdapterBase::FinalConstruct()
{
	m_pUI = new CUAEUserInterface(this);
	m_pUI->Initialize();

	return S_OK;
}
	
void CUAEAdapterBase::FinalRelease() 
{
	m_pUI->Uninitialize();
}

void CUAEAdapterBase::SendUpdateTrayMenuEvent(_bstr_t bsMenu)
{
	CCTILogger::Log(LOGLEVEL_MED,L"Sending updated menu.");

	_ISalesforceCTIAdapterEvents_UpdateTrayMenu(bsMenu);
}