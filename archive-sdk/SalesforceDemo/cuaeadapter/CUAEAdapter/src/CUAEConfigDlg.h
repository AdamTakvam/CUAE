// CUAEConfigDlg.h : Declaration of the CUAEConfigDlg

#pragma once

#include "resource.h"       // main symbols
#include <atlhost.h>
#include "atlcontrols.h"
#include "CUAEEventSink.h"

// CUAEConfigDlg

class CUAEConfigDlg : 
	public CAxDialogImpl<CUAEConfigDlg>
{
private:
	ATLControls::CEdit cuaeIPEdit;
	ATLControls::CEdit deviceNameEdit;

public:
	char* cuaeIP;
	char* deviceName;

	CUAEConfigDlg()
	{
		cuaeIP = "";
		deviceName = "";
	}

	~CUAEConfigDlg()
	{
	}

	void SetDefaults(char* setCuaeIP, char* setDeviceName)
	{
		cuaeIP = setCuaeIP;
		deviceName = setDeviceName;
	}

	enum { IDD = IDD_CUAECONFIGDLG };

BEGIN_MSG_MAP(CUAEConfigDlg)
	MESSAGE_HANDLER(WM_INITDIALOG, OnInitDialog)
	COMMAND_HANDLER(IDOK, BN_CLICKED, OnClickedOK)
	COMMAND_HANDLER(IDCANCEL, BN_CLICKED, OnClickedCancel)
	CHAIN_MSG_MAP(CAxDialogImpl<CUAEConfigDlg>)
END_MSG_MAP()

// Handler prototypes:
//  LRESULT MessageHandler(UINT uMsg, WPARAM wParam, LPARAM lParam, BOOL& bHandled);
//  LRESULT CommandHandler(WORD wNotifyCode, WORD wID, HWND hWndCtl, BOOL& bHandled);
//  LRESULT NotifyHandler(int idCtrl, LPNMHDR pnmh, BOOL& bHandled);

	LRESULT OnInitDialog(UINT uMsg, WPARAM wParam, LPARAM lParam, BOOL& bHandled)
	{
		USES_CONVERSION;

		CAxDialogImpl<CUAEConfigDlg>::OnInitDialog(uMsg, wParam, lParam, bHandled);

		// Populate dialogs from registry, if exists
		cuaeIPEdit = GetDlgItem(IDC_EDIT_CUAE_IP);
		deviceNameEdit = GetDlgItem(IDC_EDIT_DEVICE_NAME);

		cuaeIPEdit.AppendText(A2W(cuaeIP));
		deviceNameEdit.AppendText(A2W(deviceName));

		return 1;  // Let the system set the focus
	}

	LRESULT OnClickedOK(WORD wNotifyCode, WORD wID, HWND hWndCtl, BOOL& bHandled)
	{
		USES_CONVERSION;

		LPTSTR cuaeIPStore = new TCHAR[255];
		LPTSTR deviceNameStore = new TCHAR[255];

		cuaeIPEdit.GetLine(0, cuaeIPStore);
		deviceNameEdit.GetLine(0, deviceNameStore);

		cuaeIP = W2A(cuaeIPStore);
		char *cuaeIPArray = new char [strlen(cuaeIP) + 1];
		strcpy(cuaeIPArray, cuaeIP);
		cuaeIP = cuaeIPArray;

		deviceName = W2A(deviceNameStore);
		char *deviceNameArray = new char [strlen(deviceName) + 1];
		strcpy(deviceNameArray, deviceName);
		deviceName = deviceNameArray;


		EndDialog(wID);
		return 0;
	}

	LRESULT OnClickedCancel(WORD wNotifyCode, WORD wID, HWND hWndCtl, BOOL& bHandled)
	{
		EndDialog(wID);
		return 0;
	}


};


