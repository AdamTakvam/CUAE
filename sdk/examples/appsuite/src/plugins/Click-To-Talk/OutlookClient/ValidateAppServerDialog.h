#ifndef __DIAL_SINGLE_DIALOG_H__
#define __DIAL_SINGLE_DIALOG_H__

#pragma once

#include "Resource.h"

const char VALIDATE_REQUEST_URI[]   = "http://%s:%u/click-to-talk/validate";

const char ValidateDialogInitializedString[] = "CValidateAppServerDialog";

class CValidateAppServerDialog : 
	public CAxDialogImpl<CValidateAppServerDialog>
{
public:
	CValidateAppServerDialog();
	virtual ~CValidateAppServerDialog();

    bool Go(char appServerStr[256], DWORD appServerPort, char usernameStr[256], char passwordStr[256]);

    ////////////////////////////////////////////////////////////
	// Event Handlers
    ////////////////////
	LRESULT OnInitDialog(UINT uMsg, WPARAM wParam, LPARAM lParam, BOOL& bHandled);
	LRESULT OnClickedOK(WORD wNotifyCode, WORD wID, HWND hWndCtl, BOOL& bHandled);
	LRESULT OnClickedCancel(WORD wNotifyCode, WORD wID, HWND hWndCtl, BOOL& bHandled);

    static DWORD WINAPI ThreadFunc(LPVOID lpParam);
    static bool WINAPI HttpStatusCallback(DWORD bytes, DWORD_PTR data);

    ////////////////////////////////////////////////////////////
	// Designer Generated Code
    ////////////////////
	enum { IDD = IDD_VALIDATE };
    
    BEGIN_MSG_MAP(CValidateAppServerDialog)
	    MESSAGE_HANDLER(WM_INITDIALOG, OnInitDialog)
	    COMMAND_HANDLER(IDOK, BN_CLICKED, OnClickedOK)
	    COMMAND_HANDLER(IDCANCEL, BN_CLICKED, OnClickedCancel)
	    CHAIN_MSG_MAP(CAxDialogImpl<CValidateAppServerDialog>)
    END_MSG_MAP()
    
protected:
    CButton m_closeButton;
    CProgressCtrl m_progressBar;
    CEdit m_statusBar;

    bool valid;
    char appServerStr[256];
    char username[256];
    char password[256];
    DWORD appServerPort;
};

#endif // __DIAL_SINGLE_DIALOG_H__
