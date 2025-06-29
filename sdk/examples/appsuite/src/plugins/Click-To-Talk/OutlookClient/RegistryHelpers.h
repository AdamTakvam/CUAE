#ifndef __REGISTERY_HELPERS_H__
#define __REGISTERY_HELPERS_H__

#include <string>

const LPCSTR METREOS_REGISTRY_ROOT  = "Software\\Metreos\\Click-to-Talk\\Properties";

const LPCSTR CM_USERNAME            = "CmUsername";
const LPCSTR CM_PASSWORD            = "CmPassword";
const LPCSTR APPSERVER              = "AppServer";
const LPCSTR APPSERVER_PORT         = "AppServerPort";
const LPCSTR ALWAYS_RECORD          = "AlwaysRecord";
const LPCSTR USER_EMAIL_ADDRESS     = "EmailAddress";

void GetMetreosRegistryValues(char* username,
                              char* password,
                              char* emailAddr,
                              char* appServer,
                              DWORD& appServerPort,
                              bool& alwaysRecord,
                              ULONG maxStrLen);

void GetRegistryStringValue(HKEY parent, 
                            const std::string& root, 
                            const std::string& key, 
                            char* valueBuf, 
                            ULONG* size);

void GetRegistryDwordValue(HKEY parent, 
                           const std::string& root, 
                           const std::string& key, 
                           DWORD& valueBuf);

void SaveStringPropertyToRegistry(LPCSTR propName, LPCSTR propValue);

void SaveDwordPropertyToRegistry(LPCSTR propName, DWORD propValue);

#endif // __REGISTERY_HELPERS_H__