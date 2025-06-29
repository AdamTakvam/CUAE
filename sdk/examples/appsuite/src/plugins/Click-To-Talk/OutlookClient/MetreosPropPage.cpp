#include "StdAfx.h"
#include "MetreosPropPage.h"
#include "RegistryHelpers.h"

HRESULT __stdcall CMetreosPropPage::SetClientSite(IOleClientSite* pClientSite)
{
    HRESULT result;

    // Call default ATL implementation
    result = IOleObjectImpl<CMetreosPropPage>::SetClientSite(pClientSite);
    if (result != S_OK) { return result; }

    // pClientSite may be NULL when container has being destructed
    if (pClientSite != NULL) 
    {
        CComQIPtr<Outlook::PropertyPageSite> pPropertyPageSite(pClientSite);
        result = pPropertyPageSite.CopyTo(&m_pPropPageSite);
    }

    m_fDirty = false;
    return result;
}

HRESULT __stdcall CMetreosPropPage::Apply()
{
    char propDataStr[256];
    UINT propDataInt = 0;

    memset(propDataStr,  0, sizeof(propDataStr));
    GetDlgItemText(IDC_CM_USERNAME, propDataStr, sizeof(propDataStr));
    UpdatePropertyValue(IDC_CM_USERNAME, propDataStr);

    memset(propDataStr,  0, sizeof(propDataStr));
    GetDlgItemText(IDC_CM_PASSWORD, propDataStr, sizeof(propDataStr));
    UpdatePropertyValue(IDC_CM_PASSWORD, propDataStr);

    memset(propDataStr,  0, sizeof(propDataStr));
    GetDlgItemText(IDC_APPSERVER, propDataStr, sizeof(propDataStr));
    UpdatePropertyValue(IDC_APPSERVER, propDataStr);

    memset(propDataStr,  0, sizeof(propDataStr));
    GetDlgItemText(IDC_USEREMAILADDR, propDataStr, sizeof(propDataStr));
    UpdatePropertyValue(IDC_USEREMAILADDR, propDataStr);

    propDataInt = GetDlgItemInt(IDC_APPSERVER_PORT);
    UpdatePropertyValue(IDC_APPSERVER_PORT, propDataInt);

    propDataInt = IsDlgButtonChecked(IDC_ALWAYS_RECORD);
    UpdatePropertyValue(IDC_ALWAYS_RECORD, propDataInt);

    SavePropertyValuesToRegistry();

    return S_OK;
}

HRESULT __stdcall CMetreosPropPage::GetPageInfo(BSTR* HelpFile, long* HelpContext)
{
    return E_NOTIMPL;
}

HRESULT __stdcall CMetreosPropPage::get_Dirty(VARIANT_BOOL* Dirty)
{
    *Dirty = m_fDirty.boolVal;
    return S_OK;
}

LRESULT CMetreosPropPage::OnInitDialog(UINT uMsg, WPARAM wParam, LPARAM lParam, BOOL& bHandled)
{
    m_appServerEdit.Attach(GetDlgItem(IDC_APPSERVER));
    m_appServerPortEdit.Attach(GetDlgItem(IDC_APPSERVER_PORT));

    // Load the values from registry
    LoadPropertyValuesFromRegistry();

    SetDlgItemText(IDC_CM_USERNAME, m_usernameStr);
    SetDlgItemText(IDC_CM_PASSWORD, m_passwordStr);
    SetDlgItemText(IDC_APPSERVER, m_appServerStr);
    SetDlgItemInt(IDC_APPSERVER_PORT, m_appServerPort);
    SetDlgItemText(IDC_USEREMAILADDR, m_emailAddrStr);
    CheckDlgButton(IDC_ALWAYS_RECORD, m_alwaysRecord ? 1 : 0);

    m_fDirty = false;
    m_pPropPageSite->OnStatusChange();

    return 0;
}

LRESULT CMetreosPropPage::OnBnClickedButtonValidate(WORD /*wNotifyCode*/, WORD /*wID*/, HWND /*hWndCtl*/, BOOL& /*bHandled*/)
{
    char appServerStr[256];
    char usernameStr[256];
    char passwordStr[256];
    UINT appServerPort = 0;

    memset(appServerStr,  0, sizeof(appServerStr));
    GetDlgItemText(IDC_APPSERVER, appServerStr, sizeof(appServerStr));
    
    memset(usernameStr,  0, sizeof(usernameStr));
    GetDlgItemText(IDC_CM_USERNAME, usernameStr, sizeof(usernameStr));
    
    memset(passwordStr,  0, sizeof(passwordStr));
    GetDlgItemText(IDC_CM_PASSWORD, passwordStr, sizeof(passwordStr));

    appServerPort = GetDlgItemInt(IDC_APPSERVER_PORT);

    if(ValidateUserSettings(appServerStr, usernameStr, passwordStr, appServerPort) == true)
    {
        m_appServerEdit.SetFont(&m_validatedFont);
        m_appServerPortEdit.SetFont(&m_validatedFont);
    }
    else
    {
        m_appServerEdit.SetFont(&m_standardFont);
        m_appServerPortEdit.SetFont(&m_standardFont);
    }

    return 0;
}

LRESULT CMetreosPropPage::OnEnChangeCmUsername(WORD /*wNotifyCode*/, WORD /*wID*/, HWND /*hWndCtl*/, BOOL& /*bHandled*/)
{
    m_fDirty = true;
    m_pPropPageSite->OnStatusChange();
    
    return 0;
}

LRESULT CMetreosPropPage::OnEnChangeCmPassword(WORD /*wNotifyCode*/, WORD /*wID*/, HWND /*hWndCtl*/, BOOL& /*bHandled*/)
{
    m_fDirty = true;
    m_pPropPageSite->OnStatusChange();
    
    return 0;
}

LRESULT CMetreosPropPage::OnEnChangeAppserver(WORD /*wNotifyCode*/, WORD /*wID*/, HWND /*hWndCtl*/, BOOL& /*bHandled*/)
{
    m_appServerEdit.SetFont(&m_standardFont);
    m_appServerPortEdit.SetFont(&m_standardFont);

    m_fDirty = true;
    m_pPropPageSite->OnStatusChange();
    
    return 0;
}

LRESULT CMetreosPropPage::OnBnClickedAlwaysRecord(WORD /*wNotifyCode*/, WORD /*wID*/, HWND /*hWndCtl*/, BOOL& /*bHandled*/)
{
    m_fDirty = true;
    m_pPropPageSite->OnStatusChange();
    
    return 0;
}

LRESULT CMetreosPropPage::OnEnChangeUseremailaddr(WORD /*wNotifyCode*/, WORD /*wID*/, HWND /*hWndCtl*/, BOOL& /*bHandled*/)
{
    m_fDirty = true;
    m_pPropPageSite->OnStatusChange();
    
    return 0;
}

