#ifndef HOOKSVC_H
#define HOOKSVC_H

void ServiceMain(int argc, char** argv); 

void ControlHandler(DWORD request);

int InitService();

int StartHooks();

int StopHooks();

BOOL InstallService();

BOOL DeleteService();

int WriteToLog(char* str);

#endif
