#include "StdAfx.h"
#include "CUAEEventSink.h"
#include "CUAEUserInterface.h"
#include "CTIAppExchange.h"
#include "CUAEConfigDlg.h"

CUAEEventSink::CUAEEventSink(CUAEUserInterface* pUI)
{
	this->ui = pUI;
	this->monitoredDevice = "";
	this->cuaeIP = "";

	HKEY myKey;
	//DebugBreak();
	if(RegOpenKeyEx(HKEY_LOCAL_MACHINE, L"SOFTWARE\\Cisco Systems\\SalesForce.Com CUAE Connector", 0, KEY_ALL_ACCESS, &myKey) == ERROR_SUCCESS)
	{
		USES_CONVERSION;

		DWORD type = 0;
		DWORD size = 0; 

		// Retrieve monitored device name
		long result = RegQueryValueEx(myKey, L"MonitoredDeviceName", NULL, NULL, NULL, &size);
		if (result == ERROR_SUCCESS)
		{
			monitoredDevice = new char[size]; 

			if(RegQueryValueEx(myKey, L"MonitoredDeviceName", NULL, &type, (byte*) monitoredDevice, &size) == ERROR_SUCCESS)
			{
				LPWSTR L_monitoredDevice = (LPWSTR)monitoredDevice;
				this->monitoredDevice = W2A(L_monitoredDevice);
				char *deviceNameArray = new char [strlen(monitoredDevice) + 1];
				strcpy(deviceNameArray, monitoredDevice);

				this->monitoredDevice = deviceNameArray;
				this->ui->SetMonitoredDevice(deviceNameArray);
			}
		}

		type = 0;

		// Retrieve server IP
		result = RegQueryValueEx(myKey, L"CuaeIP", NULL, NULL,  NULL, &size);
		if (result == ERROR_SUCCESS)
		{
			cuaeIP = new char[size]; 

			if(RegQueryValueEx(myKey, L"CuaeIP", NULL, &type, (BYTE *)cuaeIP, &size) == ERROR_SUCCESS)
			{
				LPWSTR L_cuaeIP = (LPWSTR)cuaeIP;
				this->cuaeIP = W2A(L_cuaeIP);
				char *cuaeIPArray = new char [strlen(cuaeIP) + 1];
				strcpy(cuaeIPArray, cuaeIP);

				this->cuaeIP = cuaeIPArray;
				this->ui->SetCuaeIP(cuaeIPArray);
			}
		}

		RegCloseKey(myKey);
	}
	
}

CUAEEventSink::~CUAEEventSink(void)
{
}

void CUAEEventSink::HandleMenuMessage(std::wstring& sMenuCommand)
{
	USES_CONVERSION;

	if (sMenuCommand == L"MENU_CONFIGURE_CUAE") 
	{
		CUAEConfigDlg *dlg;
		dlg = new CUAEConfigDlg();
		dlg->SetDefaults(cuaeIP, monitoredDevice);
		dlg->DoModal();
 		if (this->cuaeIP != dlg->cuaeIP || this->monitoredDevice != dlg->deviceName)
		{
			this->cuaeIP = dlg->cuaeIP;
			this->monitoredDevice = dlg->deviceName;

			this->ui->SetCuaeIP(cuaeIP);
			this->ui->SetMonitoredDevice(monitoredDevice);
			//this->ui->ConnectToServer();

			// Convert to unicode
			LPWSTR L_monitoredDevice = A2W(this->monitoredDevice);
			LPWSTR L_cuaeIP = A2W(this->cuaeIP);

			HKEY myKey;
			RegCreateKey(HKEY_LOCAL_MACHINE, L"SOFTWARE\\Cisco Systems\\SalesForce.Com CUAE Connector", &myKey);
			RegSetValueEx(myKey, L"MonitoredDeviceName", 0, REG_SZ, (CONST BYTE *)(L_monitoredDevice), (int)strlen(this->monitoredDevice) * sizeof(TCHAR));
			RegSetValueEx(myKey, L"CuaeIP", 0, REG_SZ, (CONST BYTE *)(L_cuaeIP), (int)strlen(this->cuaeIP) * sizeof(TCHAR));
			RegCloseKey(myKey);
		}
	}
}
