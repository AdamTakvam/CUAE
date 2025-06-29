#include "StdAfx.h"
#include "ValidateAppServerDialog.h"
#include "ControlMessage.h"
#include "RegistryHelpers.h"
#include "HttpHelpers.h"
#include "MetreosSettingsBase.h"

CValidateAppServerDialog::CValidateAppServerDialog()
{}

CValidateAppServerDialog::~CValidateAppServerDialog()
{
    m_progressBar.Detach();
    m_statusBar.Detach();
    m_closeButton.Detach();
}

LRESULT CValidateAppServerDialog::OnInitDialog(UINT uMsg, WPARAM wParam, LPARAM lParam, BOOL& bHandled)
{
	CAxDialogImpl<CValidateAppServerDialog>::OnInitDialog(uMsg, wParam, lParam, bHandled);

    m_progressBar.Attach(GetDlgItem(IDC_PROGRESS1));
    m_statusBar.Attach(GetDlgItem(IDC_STATUSBAR));
    m_closeButton.Attach(GetDlgItem(IDCANCEL));

    m_closeButton.EnableWindow(0);

    // Set event to tell thread function that the dialog is now initialized.
    HANDLE hEvent = OpenEvent(EVENT_ALL_ACCESS, false, ValidateDialogInitializedString);
    // assert(hEvent != NULL);
    if (hEvent != NULL)
    {
        SetEvent(hEvent);
        CloseHandle(hEvent);
    }

	return 1;  // Let the system set the focus
}

LRESULT CValidateAppServerDialog::OnClickedOK(WORD wNotifyCode, WORD wID, HWND hWndCtl, BOOL& bHandled)
{
    USES_CONVERSION;

    EndDialog(wID);
	return 0;
}

LRESULT CValidateAppServerDialog::OnClickedCancel(WORD wNotifyCode, WORD wID, HWND hWndCtl, BOOL& bHandled)
{
	EndDialog(wID);
	return 0;
}

bool CValidateAppServerDialog::Go(char appServerStr[256], DWORD appServerPort, char usernameStr[256], char passwordStr[256])
{
    bool validated = false;

    HANDLE hThread;
    DWORD dwThreadId;

    memset(this->appServerStr, 0, 256);
    memcpy(this->appServerStr, appServerStr, 256);

    memset(this->username, 0, 256);
    memcpy(this->username, usernameStr, 256);

    memset(this->password, 0, 256);
    memcpy(this->password, passwordStr, 256);

    this->appServerPort = appServerPort;

    // Create event for thread function to wait for dialog to be initialized
    HANDLE hEvent = CreateEvent(NULL, true, false, ValidateDialogInitializedString);
    // assert(hEvent != NULL);
    if (hEvent != NULL)
    {
        hThread = CreateThread(
            NULL,
            0,
            ThreadFunc,
            this,
            0,
            &dwThreadId);

        if(hThread != NULL)
        {
            this->DoModal();

            WaitForSingleObject(hThread, 5000);

            CloseHandle(hThread);

            validated = valid;
        }

        CloseHandle(hEvent);
    }

    return validated;
}


DWORD WINAPI CValidateAppServerDialog::ThreadFunc(LPVOID lpParam)
{
    // Wait for dialog to be initialized before proceeding.
    HANDLE hEvent = OpenEvent(EVENT_ALL_ACCESS, false, ValidateDialogInitializedString);
    // assert(hEvent != NULL);
    if (hEvent != NULL)
    {
        WaitForSingleObject(hEvent, INFINITE);
        CloseHandle(hEvent);

        CAtlNavigateData navData;
        CAtlHttpClient httpClient;
        char requestUri[512];
        ControlMessage controlMsg;
        std::string xmlControlMsg;
        char* response = 0;
        long responseLen = 0;
        int statusCode = -1;

        CValidateAppServerDialog* dlg = (CValidateAppServerDialog*)lpParam;

        dlg->SetDlgItemText(IDC_STATUSBAR, "Initializing");

        // Create the request URI
        memset(requestUri, 0, sizeof(requestUri));
        _snprintf(requestUri, sizeof(requestUri) - 1, VALIDATE_REQUEST_URI, dlg->appServerStr, dlg->appServerPort);

        // Build an emtpy control message with just the user's credentials.
        controlMsg.SetAuth(dlg->username, dlg->password);
        controlMsg.SetEmail("");
        xmlControlMsg = controlMsg.ToXmlString();

        // Add status call backs for the HTTP request
        navData.SetSendStatusCallback((PFNATLSTATUSCALLBACK)HttpStatusCallback, (DWORD)dlg);
        navData.SetReadStatusCallback((PFNATLSTATUSCALLBACK)HttpStatusCallback, (DWORD)dlg);

        // Set the method to POST and add our POST data to the body of the message
        navData.SetMethod(ATL_HTTP_METHOD_POST);
        navData.SetPostData((unsigned char*)xmlControlMsg.c_str(), (DWORD)xmlControlMsg.length(), CONTENT_TYPE);
        
        dlg->SetDlgItemText(IDC_STATUSBAR, "Connecting...");
        dlg->m_progressBar.StepIt();

        // Do the HTTP POST request
        httpClient.Navigate(requestUri, &navData);

        responseLen = GetResponseFromClient(httpClient, &statusCode, &response);

        dlg->SetDlgItemText(IDC_STATUSBAR, "Connected, Reading Status Code");
        dlg->m_progressBar.StepIt();

        if(statusCode == 200)
        {
            // Status code 200 received, successful validation
            dlg->m_progressBar.StepIt();
            dlg->valid = true;
            dlg->SetDlgItemText(IDC_STATUSBAR, "Successful");
            
        }
        else if(statusCode == -1)
        {
            // No status code, we didn't connect or didn't get a response
            dlg->m_progressBar.StepIt();        
            dlg->SetDlgItemText(IDC_STATUSBAR, "Failed to Connect");
            dlg->valid = false;
        }
        else
        {
            // Connected, but validation failed for some other reason
            dlg->m_progressBar.StepIt();
            dlg->SetDlgItemText(IDC_STATUSBAR, "Validation failed");
            dlg->valid = false;
        }

        dlg->m_progressBar.SetPos(100);
        httpClient.Close();

        dlg->m_closeButton.EnableWindow(1);

		if (response != 0)
			delete response;
    }

    return 0;
}

bool WINAPI CValidateAppServerDialog::HttpStatusCallback(DWORD bytes, DWORD_PTR data)
{
    CValidateAppServerDialog* dlg = (CValidateAppServerDialog*)((void*)data);
    dlg->m_progressBar.StepIt();

    return true;
}