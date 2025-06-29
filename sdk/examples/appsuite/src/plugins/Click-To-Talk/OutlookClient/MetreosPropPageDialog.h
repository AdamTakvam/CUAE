#ifndef __METREOS_PROP_PAGE_DIALOG_H__
#define __METREOS_PROP_PAGE_DIALOG_H__

#pragma once

#include "Resource.h"
#include "MetreosSettingsBase.h"

class CMetreosPropPageDialog : 
	public CAxDialogImpl<CMetreosPropPageDialog>,
    public MetreosSettingsBase
{
public:
	CMetreosPropPageDialog();
	virtual ~CMetreosPropPageDialog();


    ////////////////////////////////////////////////////////////
	// Event Handlers
    ////////////////////
	LRESULT OnInitDialog(UINT uMsg, WPARAM wParam, LPARAM lParam, BOOL& bHandled);
	LRESULT OnClickedOK(WORD wNotifyCode, WORD wID, HWND hWndCtl, BOOL& bHandled);
    LRESULT OnClickedApply(WORD wNotifyCode, WORD wID, HWND hWndCtl, BOOL& bHandled);
	LRESULT OnClickedCancel(WORD wNotifyCode, WORD wID, HWND hWndCtl, BOOL& bHandled);

    LRESULT OnBnClickedButtonValidate(WORD /*wNotifyCode*/, WORD /*wID*/, HWND /*hWndCtl*/, BOOL& /*bHandled*/);
    LRESULT OnEnChangeCmUsername(WORD /*wNotifyCode*/, WORD /*wID*/, HWND /*hWndCtl*/, BOOL& /*bHandled*/);
    LRESULT OnEnChangeCmPassword(WORD /*wNotifyCode*/, WORD /*wID*/, HWND /*hWndCtl*/, BOOL& /*bHandled*/);
    LRESULT OnEnChangeAppserver(WORD /*wNotifyCode*/, WORD /*wID*/, HWND /*hWndCtl*/, BOOL& /*bHandled*/);
    LRESULT OnBnClickedAlwaysRecord(WORD /*wNotifyCode*/, WORD /*wID*/, HWND /*hWndCtl*/, BOOL& /*bHandled*/);
    LRESULT OnEnChangeUseremailaddr(WORD /*wNotifyCode*/, WORD /*wID*/, HWND /*hWndCtl*/, BOOL& /*bHandled*/);


    ////////////////////////////////////////////////////////////
	// Designer Generated Code
    ////////////////////
	enum { IDD = IDD_METREOSPROPDIALOG };
    
    BEGIN_MSG_MAP(CDialSingleDialog)
	    MESSAGE_HANDLER(WM_INITDIALOG, OnInitDialog)
        COMMAND_HANDLER(IDC_BUTTON_VALIDATE, BN_CLICKED, OnBnClickedButtonValidate)
        COMMAND_HANDLER(IDC_CM_USERNAME, EN_CHANGE, OnEnChangeCmUsername)
        COMMAND_HANDLER(IDC_CM_PASSWORD, EN_CHANGE, OnEnChangeCmPassword)
        COMMAND_HANDLER(IDC_APPSERVER, EN_CHANGE, OnEnChangeAppserver)
        COMMAND_HANDLER(IDC_APPSERVER_PORT, EN_CHANGE, OnEnChangeAppserver)
        COMMAND_HANDLER(IDC_ALWAYS_RECORD, BN_CLICKED, OnBnClickedAlwaysRecord)
        COMMAND_HANDLER(IDC_USEREMAILADDR, EN_CHANGE, OnEnChangeUseremailaddr)
	    COMMAND_HANDLER(IDOK, BN_CLICKED, OnClickedOK)
        COMMAND_HANDLER(IDAPPLY, BN_CLICKED, OnClickedApply)
	    COMMAND_HANDLER(IDCANCEL, BN_CLICKED, OnClickedCancel)
	    CHAIN_MSG_MAP(CAxDialogImpl<CMetreosPropPageDialog>)
    END_MSG_MAP()

protected:
    CButton m_applyButton;
    CEdit   m_appServerEdit;
    CEdit   m_appServerPortEdit;
};

#endif // __METREOS_PROP_PAGE_DIALOG_H__
