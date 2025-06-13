#include "InitDll.h"

#include <windows.h>
#include "_vcclrit.h"

namespace Metreos
{
namespace SCCP
{

void InitDll::Initialize()
{
	__crt_dll_initialize();
}

void InitDll::Terminate()
{
	__crt_dll_terminate();
}

}
}

BOOL WINAPI DllMain(HINSTANCE hModule, DWORD dwReason, LPVOID lpvReserved) 
{
	return TRUE;
}
