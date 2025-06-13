#include "stdafx.h"
#include "TestServiceRuntime.h"

TestServiceRuntime::TestServiceRuntime()
{
}

TestServiceRuntime::~TestServiceRuntime()
{
}

void TestServiceRuntime::OnStart(DWORD control)
{
}

void TestServiceRuntime::OnStop(DWORD control)
{
}

int main(int argc, char* argv[])
{
	TestServiceRuntime service;
    Metreos::Win32::ServiceRuntimeBase::Run(service);
    return 0;
}

