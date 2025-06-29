#include "windows.h"
#include "stdio.h"
#include "mscoree.h"

#import  <mscorlib.tlb> raw_interfaces_only high_property_prefixes("_get","_put","_putref")
#import "x:\build\AppServer\AppServer.tlb" raw_interfaces_only high_property_prefixes("_get","_put","_putref")

using namespace mscorlib;

int wmain(int argc, WCHAR **argv)
{
    ICorRuntimeHost *pHost = NULL;
    IUnknown *pAppDomainThunk = NULL;
        _AppDomain *pAppDomain = NULL;

    // let's host CLR
    HRESULT hr = CorBindToRuntimeEx(  L"v1.1.4322",   // pwzVersion
                                L"svr",   // pwzBuildFlavor
                                STARTUP_LOADER_OPTIMIZATION_SINGLE_DOMAIN | STARTUP_CONCURRENT_GC,
                                CLSID_CorRuntimeHost,
                                IID_ICorRuntimeHost,
                                (VOID**)&pHost);

    if (FAILED(hr)) {
        printf("CorBindToRuntimeHost failed with hr=0x%x.\n", hr);
        return 2;
    }

	printf(".NET CLR v1.1.4322 loaded...\n");

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

    _ObjectHandle *pObjHandle = NULL;
	hr = pAppDomain->CreateInstance(
			_bstr_t("AppServer"),
			_bstr_t("Metreos.AppServer.ConsoleRuntime.ConsoleMain"), 
			&pObjHandle); 
    if (FAILED(hr)) {
        printf("_AppDomain::CreateInstance failed with hr=0x%x.\n", hr);
        return 6;
    }

	printf("AppServer core runtime created...\n");

//    VARIANT v;
//	VariantInit(&v);
//	hr = pObjHandle->Unwrap(&v);
//	if (FAILED(hr)) {
//        printf("_ObjectHandle::Unwrap failed with hr=0x%x.\n", hr);
//        return 7;
//    }

//    _ConsoleMain *pMgdHost = NULL;
//	hr = v.pdispVal->QueryInterface(__uuidof(_ConsoleMain), 
//                                (void**) &pMgdHost);
//	if (FAILED(hr)) {
//        printf("Unable to obtain pointer to managed code (hr=0x%x).\n", hr);
//        return 8;
//    }

//	pMgdHost->RunUserCode(_bstr_t("AppServerDomain")); //_bstr_t("MyDomain"));

    return 0;
}