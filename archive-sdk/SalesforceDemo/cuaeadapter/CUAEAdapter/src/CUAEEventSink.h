#pragma once
#include <string>
#include <map>
#include <set>

class CUAEUserInterface;

class CUAEEventSink
{
protected:
	char * monitoredDevice;
	char * cuaeIP;

	CUAEUserInterface* ui;

public:
	CUAEEventSink(CUAEUserInterface* pUI);
	~CUAEEventSink(void);

	void HandleMenuMessage(std::wstring& sMenuCommand);


};
