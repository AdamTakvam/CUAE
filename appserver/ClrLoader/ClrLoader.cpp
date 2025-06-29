#include "windows.h"
#include "stdio.h"
#include "mscoree.h"

#import  <mscorlib.tlb> raw_interfaces_only high_property_prefixes("_get","_put","_putref")

using namespace mscorlib;

int wmain(int argc, WCHAR **argv)
{
    if (argc != 3) {
        printf("Usage: HostDemo HostConfigFile AppToExecute\n");
        return 1;
    }

    ICorRuntimeHost *pHost = NULL;
    IUnknown *pAppDomainThunk = NULL;
        _AppDomain *pAppDomain = NULL;

    // let's host CLR
    HRESULT hr = CorBindToRuntimeHost(  L"v1.1.4322",   // pwzVersion
                                L"svr",   // pwzBuildFlavor
                                argv[1],  // host config file
                                NULL,   // reversed
                                0,      // startup flag,
                                CLSID_CorRuntimeHost,
                                IID_ICorRuntimeHost,
                                (VOID**)&pHost);

    if (FAILED(hr)) {
        printf("CorBindToRuntimeHost failed with hr=0x%x.\n", hr);
        return 2;
    }

    // Start the runtime
    hr = pHost->Start();
    if (FAILED(hr)) {
        printf("ICorRuntimeHost->Start failed with hr=0x%x.\n", hr);
        return 3;
    }

    // Get the default appdomain.
    hr = pHost->GetDefaultDomain(&pAppDomainThunk);
    if (FAILED(hr)) {
        printf("ICorRuntimeHost->GetDefaultDomain failed with hr=0x%x.\n", hr);
        return 4;
    }

    // Get System::_AppDomain interface
    hr = pAppDomainThunk->QueryInterface(__uuidof(_AppDomain),
                                        (void**) &pAppDomain);
    if (FAILED(hr)) {
        printf("Can't get System::_AppDomain interface\n");
        return 5;
    }

    long lRet= 0;
    // call System::_AppDomain::ExecuteAssembly(String)
    hr = pAppDomain->ExecuteAssembly_2(_bstr_t(argv[2]), &lRet);
    if (FAILED(hr)) {
        printf("_AppDomain::ExecuteAssembly_2 failed with hr=0x%x.\n", hr);
        return 6;
    }

    return 0;
}