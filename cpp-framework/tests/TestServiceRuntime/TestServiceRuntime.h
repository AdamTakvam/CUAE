#ifndef TEST_SERVICE_RUNTIME_H
#define TEST_SERVICE_RUNTIME_H

#include "win32/ServiceRuntimeBase.h"

class TestServiceRuntime : public Metreos::Win32::ServiceRuntimeBase
{
public:
    TestServiceRuntime();
    virtual ~TestServiceRuntime();

    virtual void OnStart(DWORD control);
    virtual void OnStop(DWORD control);
};

#endif // TEST_SERVICE_RUNTIME_H