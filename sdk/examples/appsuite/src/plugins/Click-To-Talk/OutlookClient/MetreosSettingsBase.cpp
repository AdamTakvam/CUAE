#include "StdAfx.h"
#include "MetreosSettingsBase.h"
#include "RegistryHelpers.h"
#include "HttpHelpers.h"
#include "ValidateAppServerDialog.h"
const char* DLGTITLE_BADPARAM = "User Settings Parameter Invalid";


MetreosSettingsBase::MetreosSettingsBase()
{
    m_validatedFont.CreateFont(
        14,                        // nHeight
        0,                         // nWidth
        0,                         // nEscapement
        0,                         // nOrientation
        FW_NORMAL,                 // nWeight
        FALSE,                     // bItalic
        TRUE,                      // bUnderline
        0,                         // cStrikeOut
        ANSI_CHARSET,              // nCharSet
        OUT_DEFAULT_PRECIS,        // nOutPrecision
        CLIP_DEFAULT_PRECIS,       // nClipPrecision
        DEFAULT_QUALITY,           // nQuality
        DEFAULT_PITCH | FF_SWISS,  // nPitchAndFamily
        "Arial");                  // lpszFacename

    m_standardFont.CreateFont(
        14,                        // nHeight
        0,                         // nWidth
        0,                         // nEscapement
        0,                         // nOrientation
        FW_NORMAL,                 // nWeight
        FALSE,                     // bItalic
        FALSE,                     // bUnderline
        0,                         // cStrikeOut
        ANSI_CHARSET,              // nCharSet
        OUT_DEFAULT_PRECIS,        // nOutPrecision
        CLIP_DEFAULT_PRECIS,       // nClipPrecision
        DEFAULT_QUALITY,           // nQuality
        DEFAULT_PITCH | FF_SWISS,  // nPitchAndFamily
        "Arial");                  // lpszFacename
}


MetreosSettingsBase::~MetreosSettingsBase()
{
    m_validatedFont.DeleteObject();
    m_standardFont.DeleteObject();
}


bool MetreosSettingsBase::ValidateUserSettings(
    char* appServerStr,
    char* usernameStr,
    char* passwordStr,
    UINT appServerPort)
{
    bool validated = false;
	TCHAR msg[512];
	HINSTANCE hInst = _AtlModule.GetResourceInstance();

    if (strlen(usernameStr) == 0)
    {
		::LoadString(hInst, IDS_INVALID_USERNAME, msg, sizeof(msg)/sizeof(TCHAR)); 
        ::MessageBox(::GetActiveWindow(),
            msg, //"Could not validate ClickToTalk user settings",         
            DLGTITLE_BADPARAM, MB_ICONEXCLAMATION);
    }
    else if (strlen(appServerStr) == 0)
    {
		::LoadString(hInst, IDS_INVALID_SERVERADDRESS, msg, sizeof(msg)/sizeof(TCHAR)); 
        ::MessageBox(::GetActiveWindow(),
            msg, //"Application server network address missing or invalid",          
            DLGTITLE_BADPARAM, MB_ICONEXCLAMATION);
    }
    else if (appServerPort == 0)
    {
		::LoadString(hInst, IDS_INVALID_SERVERPORT, msg, sizeof(msg)/sizeof(TCHAR)); 
        ::MessageBox(::GetActiveWindow(),
            msg, //"Application server port number missing or invalid",           
            DLGTITLE_BADPARAM, MB_ICONEXCLAMATION);
    }
    else
    {
        CValidateAppServerDialog validateDlg;
        validated = validateDlg.Go(appServerStr, appServerPort, usernameStr, passwordStr);

        if (!validated)
        {
			::LoadString(hInst, IDS_INVALID_SETTINGS, msg, sizeof(msg)/sizeof(TCHAR)); 
            ::MessageBox(::GetActiveWindow(),
				msg,
                //"Could not validate ClickToTalk user settings.\n"
                //"Please verify that user name and password, and\n"
                //"application server IP address and port, are correct.", 
				DLGTITLE_BADPARAM, MB_ICONEXCLAMATION);               
        }
    }

    return validated;
}


void MetreosSettingsBase::LoadPropertyValuesFromRegistry()
{
    GetMetreosRegistryValues(
        m_usernameStr, 
        m_passwordStr, 
        m_emailAddrStr,
        m_appServerStr,
        m_appServerPort,
        m_alwaysRecord,
        STRING_PROPERTY_SIZE);
}


void MetreosSettingsBase::SavePropertyValuesToRegistry()
{
    SaveStringPropertyToRegistry(CM_USERNAME, m_usernameStr);
    SaveStringPropertyToRegistry(CM_PASSWORD, m_passwordStr);
    SaveStringPropertyToRegistry(APPSERVER, m_appServerStr);
    SaveStringPropertyToRegistry(USER_EMAIL_ADDRESS, m_emailAddrStr);
    SaveDwordPropertyToRegistry(APPSERVER_PORT, m_appServerPort);
    SaveDwordPropertyToRegistry(ALWAYS_RECORD, m_alwaysRecord);
}


void MetreosSettingsBase::UpdatePropertyValue(int key, char value[256])
{
    switch(key)
    {
    case IDC_CM_USERNAME:
        memset(m_usernameStr, 0, sizeof(m_usernameStr));
        memcpy(m_usernameStr, value, sizeof(m_usernameStr));
        break;

    case IDC_CM_PASSWORD:
        memset(m_passwordStr, 0, sizeof(m_passwordStr));
        memcpy(m_passwordStr, value, sizeof(m_passwordStr));
        break;

    case IDC_APPSERVER:
        memset(m_appServerStr, 0, sizeof(m_appServerStr));
        memcpy(m_appServerStr, value, sizeof(m_appServerStr));
        break;

    case IDC_USEREMAILADDR:
        memset(m_emailAddrStr, 0, sizeof(m_emailAddrStr));
        memcpy(m_emailAddrStr, value, sizeof(m_emailAddrStr));
    }
}


void MetreosSettingsBase::UpdatePropertyValue(int key, DWORD value)
{
    switch(key)
    {
    case IDC_APPSERVER_PORT:
        m_appServerPort = value;
        break;

    case IDC_ALWAYS_RECORD:
        m_alwaysRecord = (value == 0) ? false : true;
        break;
    }
}

bool MetreosSettingsBase::AlwaysRecordCalls()
{
    // Always Record
    DWORD dwAlwaysRecord = 0;
    GetRegistryDwordValue(HKEY_CURRENT_USER, METREOS_REGISTRY_ROOT,
        ALWAYS_RECORD, dwAlwaysRecord);
    return (dwAlwaysRecord == 0) ? false : true;
}
