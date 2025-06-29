#include "StdAfx.h"
#include "MetreosPropPageDialog.h"
#include "ValidateAppServerDialog.h"

CMetreosPropPageDialog::CMetreosPropPageDialog()
{}

CMetreosPropPageDialog::~CMetreosPropPageDialog()
{   
    m_appServerEdit.Detach();
    m_appServerPortEdit.Detach();
    m_applyButton.Detach();
}

LRESULT CMetreosPropPageDialog::OnInitDialog(UINT uMsg, WPARAM wParam, LPARAM lParam, BOOL& bHandled)
{
    CAxDialogImpl<CMetreosPropPageDialog>::OnInitDialog(uMsg, wParam, lParam, bHandled);

    m_applyButton.Attach(GetDlgItem(IDAPPLY));
    m_appServerEdit.Attach(GetDlgItem(IDC_APPSERVER));
    m_appServerPortEdit.Attach(GetDlgItem(IDC_APPSERVER_PORT));

    LoadPropertyValuesFromRegistry();

    SetDlgItemText(IDC_CM_USERNAME, m_usernameStr);
    SetDlgItemText(IDC_CM_PASSWORD, m_passwordStr);
    SetDlgItemText(IDC_APPSERVER, m_appServerStr);
    SetDlgItemInt(IDC_APPSERVER_PORT, m_appServerPort);
    SetDlgItemText(IDC_USEREMAILADDR, m_emailAddrStr);
    CheckDlgButton(IDC_ALWAYS_RECORD, m_alwaysRecord ? 1 : 0);

    m_applyButton.EnableWindow(0);

    return 1;  // Let the system set the focus
}

LRESULT CMetreosPropPageDialog::OnClickedOK(WORD wNotifyCode, WORD wID, HWND hWndCtl, BOOL& bHandled)
{
    OnClickedApply(wNotifyCode, wID, hWndCtl, bHandled);
    EndDialog(wID);

    return 0;
}

LRESULT CMetreosPropPageDialog::OnClickedApply(WORD wNotifyCode, WORD wID, HWND hWndCtl, BOOL& bHandled)
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

    m_applyButton.EnableWindow(0);

    return 0;
}

LRESULT CMetreosPropPageDialog::OnClickedCancel(WORD wNotifyCode, WORD wID, HWND hWndCtl, BOOL& bHandled)
{
    EndDialog(wID);
    return 0;
}

LRESULT CMetreosPropPageDialog::OnBnClickedButtonValidate(WORD /*wNotifyCode*/, WORD /*wID*/, HWND /*hWndCtl*/, BOOL& /*bHandled*/)
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

LRESULT CMetreosPropPageDialog::OnEnChangeCmUsername(WORD /*wNotifyCode*/, WORD /*wID*/, HWND /*hWndCtl*/, BOOL& /*bHandled*/)
{ 
    m_applyButton.EnableWindow(1);
    return 0;
}

LRESULT CMetreosPropPageDialog::OnEnChangeCmPassword(WORD /*wNotifyCode*/, WORD /*wID*/, HWND /*hWndCtl*/, BOOL& /*bHandled*/)
{
    m_applyButton.EnableWindow(1);
    return 0;
}

LRESULT CMetreosPropPageDialog::OnEnChangeAppserver(WORD /*wNotifyCode*/, WORD /*wID*/, HWND /*hWndCtl*/, BOOL& /*bHandled*/)
{
    m_appServerEdit.SetFont(&m_standardFont);
    m_appServerPortEdit.SetFont(&m_standardFont);
    m_applyButton.EnableWindow(1);
    return 0;
}

LRESULT CMetreosPropPageDialog::OnBnClickedAlwaysRecord(WORD /*wNotifyCode*/, WORD /*wID*/, HWND /*hWndCtl*/, BOOL& /*bHandled*/)
{ 
    m_applyButton.EnableWindow(1);
    return 0;
}

LRESULT CMetreosPropPageDialog::OnEnChangeUseremailaddr(WORD /*wNotifyCode*/, WORD /*wID*/, HWND /*hWndCtl*/, BOOL& /*bHandled*/)
{
    m_applyButton.EnableWindow(1);
    return 0;
}


