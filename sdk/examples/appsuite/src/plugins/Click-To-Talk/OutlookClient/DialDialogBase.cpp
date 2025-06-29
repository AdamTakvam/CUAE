#include "StdAfx.h"
#include "DialDialogBase.h"
#include "HttpHelpers.h"
#include "RegistryHelpers.h"

DialDialogBase::DialDialogBase() :
    m_conferenceId(0)
{}

void DialDialogBase::DialogDestroy()
{
    if(m_conferenceId != 0)
    {
        delete m_conferenceId;
        m_conferenceId = 0;
    }
}

bool DialDialogBase::MakeCall(
    char* appServer, 
    DWORD appServerPort,
    bool record,
    ControlMessage& controlMsg)
{
    bool makeCallError = false;
	TCHAR msg[512], title[256];
	HINSTANCE hInst = _AtlModule.GetResourceInstance();

    std::string xmlControlMsg = controlMsg.ToXmlString();

    UpdateStatusBar(IDS_STATUS_DIALING /*"Dialing..."*/);
    if(SendMakeCallHttpRequest(appServer, appServerPort, xmlControlMsg, &m_conferenceId) == false)
    {
        UpdateStatusBar(IDS_STATUS_ERROR_MAKE_CALL /*"ERROR: Unable to initiate call"*/);

		::LoadString(hInst, IDS_ERROR_MAKE_CALL, msg, sizeof(msg)/sizeof(TCHAR)); 		
		::LoadString(hInst, IDS_ERROR_MAKE_CALL_TITLE, title, sizeof(title)/sizeof(TCHAR)); 		        
        ::MessageBox(
            ::GetActiveWindow(),
            msg, //"Your call could not be initiated.  Please verify your settings and try again.", 
            title, //"Unable to Initiate Call",
            MB_ICONEXCLAMATION);

        m_conferenceId = 0;

        makeCallError = true;
    }
    else
    {
        UpdateStatusBar(IDS_STATUS_CALL_INPROGRESS /*"Call in progress..."*/);

        if(record == true)
        {
            m_stopRecordButton.EnableWindow(1);
        }
    }

    return makeCallError;
}

void DialDialogBase::UpdateStatusBar(LPCSTR statusText)
{
    m_statusBar.SetWindowText(statusText);
    m_statusBar.UpdateWindow();
}

void DialDialogBase::UpdateStatusBar(UINT nID)
{
	TCHAR msg[512];
	HINSTANCE hInst = _AtlModule.GetResourceInstance();

	::LoadString(hInst, nID, msg, sizeof(msg)/sizeof(TCHAR)); 		

    m_statusBar.SetWindowText(msg);
    m_statusBar.UpdateWindow();
}

bool DialDialogBase::HandleStartCall()
{
    USES_CONVERSION;

    // Registry values.
    char emailAddrStr[256];
    char usernameStr[256];
    char passwordStr[256];
    char appServerStr[256];
    DWORD appServerPort;

    ControlMessage controlMsg;
    bool makeCallError = false;
    bool record;

    m_dialButton.EnableWindow(0);

    GetMetreosRegistryValues(usernameStr, passwordStr, emailAddrStr, appServerStr, appServerPort, record, 256);

    record = IsDlgButtonChecked(::GetActiveWindow(), IDC_CHECK_RECORDCALL) == BST_CHECKED ? true : false;

    BuildControlMessage(controlMsg);

    controlMsg.SetEmail(emailAddrStr);
    controlMsg.SetAuth(usernameStr, passwordStr);
    controlMsg.SetRecord(record);

    makeCallError = MakeCall(appServerStr, appServerPort, record, controlMsg);

    return record;
}

void DialDialogBase::HandleStopRecord()
{
    // Registry values.
    char emailAddrStr[256];
    char usernameStr[256];
    char passwordStr[256];
    char appServerStr[256];
    DWORD appServerPort;
    bool record;

    m_stopRecordButton.EnableWindow(0);

    GetMetreosRegistryValues(usernameStr, passwordStr, emailAddrStr, appServerStr, appServerPort, record, 256);

    UpdateStatusBar(IDS_STATUS_STOP_RECORDING /*"Attempting to stop the recording..."*/);
    if(SendStopRecordHttpRequest(appServerStr, appServerPort, m_conferenceId) == false)
    {
        UpdateStatusBar(IDS_STATUS_ERROR_STOP_RECORDING /*"Unable to stop recording. The call may no longer be active."*/);
        m_stopRecordButton.EnableWindow(1);
    }
    else
    {
        UpdateStatusBar(IDS_STATUS_RECORDING_STOPPED /*"Recording stopped, call still in progress..."*/);
    }
}