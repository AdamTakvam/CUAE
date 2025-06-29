#include "StdAfx.h"
#include "RegistryHelpers.h"

void GetMetreosRegistryValues(char* username,
                              char* password,
                              char* emailAddr,
                              char* appServer,
                              DWORD& appServerPort,
                              bool& alwaysRecord,
                              ULONG maxStrLen)
{
    // CallManager Username
    ULONG tempMaxStrLen = maxStrLen;
    memset(username,  0, sizeof(username));
    GetRegistryStringValue(HKEY_CURRENT_USER, METREOS_REGISTRY_ROOT, CM_USERNAME, username, &tempMaxStrLen);

    // CallManager Password
    tempMaxStrLen = maxStrLen;
    memset(password,  0, sizeof(password));
    GetRegistryStringValue(HKEY_CURRENT_USER, METREOS_REGISTRY_ROOT, CM_PASSWORD, password, &tempMaxStrLen);

    // User Email Address
    tempMaxStrLen = maxStrLen;
    memset(emailAddr,  0, sizeof(emailAddr));
    GetRegistryStringValue(HKEY_CURRENT_USER, METREOS_REGISTRY_ROOT, USER_EMAIL_ADDRESS, emailAddr, &tempMaxStrLen);

    // AppServer Machine Name/IP Address
    tempMaxStrLen = maxStrLen;
    memset(appServer,  0, sizeof(appServer));
    GetRegistryStringValue(HKEY_CURRENT_USER, METREOS_REGISTRY_ROOT, APPSERVER, appServer, &tempMaxStrLen);

    // AppServer Port
    GetRegistryDwordValue(HKEY_CURRENT_USER, METREOS_REGISTRY_ROOT, APPSERVER_PORT, appServerPort);

    // Always Record
    DWORD dwAlwaysRecord = 0;
    GetRegistryDwordValue(HKEY_CURRENT_USER, METREOS_REGISTRY_ROOT, ALWAYS_RECORD, dwAlwaysRecord);
    alwaysRecord = dwAlwaysRecord == 0 ? false : true;
}

void GetRegistryStringValue(HKEY parent, 
                            const std::string& root, 
                            const std::string& key, 
                            char* valueBuf, 
                            ULONG* size)
{
    CRegKey regKey;
    regKey.Open(parent, root.c_str());

    memset(valueBuf,  0, sizeof(*size));
    regKey.QueryStringValue(key.c_str(), valueBuf, size);

    regKey.Close();
}

void GetRegistryDwordValue(HKEY parent, 
                           const std::string& root, 
                           const std::string& key, 
                           DWORD& valueBuf)
{
    CRegKey regKey;
    regKey.Open(parent, root.c_str());

    regKey.QueryDWORDValue(key.c_str(), valueBuf);

    regKey.Close();
}

void SaveStringPropertyToRegistry(LPCSTR propName, LPCSTR propValue)
{
    CRegKey regKey;
    regKey.Open(HKEY_CURRENT_USER, METREOS_REGISTRY_ROOT);

    regKey.SetStringValue(propName, propValue);

    regKey.Close();
}

void SaveDwordPropertyToRegistry(LPCSTR propName, DWORD propValue)
{
    CRegKey regKey;
    regKey.Open(HKEY_CURRENT_USER, METREOS_REGISTRY_ROOT);

    regKey.SetDWORDValue(propName, propValue);

    regKey.Close();
}
