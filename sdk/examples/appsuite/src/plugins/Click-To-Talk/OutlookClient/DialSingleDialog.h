#ifndef __DIAL_SINGLE_DIALOG_H__
#define __DIAL_SINGLE_DIALOG_H__

#pragma once

#include "Resource.h"
#include "MetreosToolbar.h"
#include "DialDialogBase.h"

class CDialSingleDialog : 
	public CAxDialogImpl<CDialSingleDialog>,
    public DialDialogBase
{
public:
	CDialSingleDialog();
	virtual ~CDialSingleDialog();

    virtual void SetDialInformation(const DialParticipant_list& numbers);
    virtual void BuildControlMessage(ControlMessage& controlMsg);

    ////////////////////////////////////////////////////////////
	// Event Handlers
    ////////////////////
	LRESULT OnInitDialog(UINT uMsg, WPARAM wParam, LPARAM lParam, BOOL& bHandled);
    LRESULT OnDestroy(UINT uMsg, WPARAM wParam, LPARAM lParam, BOOL& bHandled);

    LRESULT OnClickedStartCall(WORD wNotifyCode, WORD wID, HWND hWndCtl, BOOL& bHandled);
	LRESULT OnClickedCancel(WORD wNotifyCode, WORD wID, HWND hWndCtl, BOOL& bHandled);
    LRESULT OnBnClickedBtnRecord(WORD wNotifyCode, WORD wID, HWND hWndCtl, BOOL& bHandled);


    ////////////////////////////////////////////////////////////
	// Designer Generated Code
    ////////////////////
	enum { IDD = IDD_DIALSINGLEDIALOG };
    
    BEGIN_MSG_MAP(CDialSingleDialog)
	    MESSAGE_HANDLER(WM_INITDIALOG, OnInitDialog)
        MESSAGE_HANDLER(WM_DESTROY, OnDestroy);
	    COMMAND_HANDLER(IDOK, BN_CLICKED, OnClickedStartCall)
	    COMMAND_HANDLER(IDCANCEL, BN_CLICKED, OnClickedCancel)
        COMMAND_HANDLER(IDC_BTN_STOP_RECORD, BN_CLICKED, OnBnClickedBtnRecord)
        CHAIN_MSG_MAP(CAxDialogImpl<CDialSingleDialog>)
    END_MSG_MAP()

protected:
    ////////////////////////////////////////////////////////////
	// Utility Methods 
    ////////////////////
    void InitDialogData();
    void PopulateNumbersCombo(CComBSTR numberStr, CComBSTR desc);

    CEdit               m_contactName;
    CComboBox           m_dialNumCombo;
    DialParticipant     m_pParticipant;
};

#endif // __DIAL_SINGLE_DIALOG_H__
