#ifndef __DIAL_MULTIPLE_DIALOG_H__
#define __DIAL_MULTIPLE_DIALOG_H__

#pragma once

#include "Resource.h"
#include "MetreosToolbar.h"
#include "DialDialogBase.h"


////////////////////////////////////////////////////////////
// class CDialMultipleDialog
//
// Dialog for creating ad-hoc conference calls with up to
// four parties selected.
//
////////////////////
class CDialMultipleDialog : 
	public CAxDialogImpl<CDialMultipleDialog>,
    public DialDialogBase
{
public:
    CDialMultipleDialog();
    virtual ~CDialMultipleDialog();

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
	enum { IDD = IDD_DIALMULTIPLEDIALOG };

    BEGIN_MSG_MAP(CDialMultipleDialog)
	    MESSAGE_HANDLER(WM_INITDIALOG, OnInitDialog)
        MESSAGE_HANDLER(WM_DESTROY, OnDestroy);
	    COMMAND_HANDLER(IDOK, BN_CLICKED, OnClickedStartCall)
	    COMMAND_HANDLER(IDCANCEL, BN_CLICKED, OnClickedCancel)
        COMMAND_HANDLER(IDC_BTN_STOP_RECORD, BN_CLICKED, OnBnClickedBtnRecord)
        CHAIN_MSG_MAP(CAxDialogImpl<CDialMultipleDialog>)
    END_MSG_MAP()

protected:
    ////////////////////////////////////////////////////////////
	// Utility Methods 
    ////////////////////
    void InitDialogData();
    void PopulateNumbersCombo(int index, CComBSTR numberStr, CComBSTR desc);

    DialParticipant     m_pParticipants[4];
    CComboBox           m_dialNumCombo[4];
    CEdit               m_editBoxes[4];

    int                 m_numParticipants;
};

// Handler prototypes (for reference):
//  LRESULT MessageHandler(UINT uMsg, WPARAM wParam, LPARAM lParam, BOOL& bHandled);
//  LRESULT CommandHandler(WORD wNotifyCode, WORD wID, HWND hWndCtl, BOOL& bHandled);
//  LRESULT NotifyHandler(int idCtrl, LPNMHDR pnmh, BOOL& bHandled);

#endif // __DIAL_MULTIPLE_DIALOG_H__
