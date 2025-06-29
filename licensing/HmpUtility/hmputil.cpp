#include "stdafx.h"
#include "hmpControl.h"
#include "ace/Trace.h"

int startHmpService();
int stopHmpService();
void usage();

using namespace System;

int main (int argc, char *argv[])
{
	if (argc == 0) {
		usage();
	} else {
		if (stricmp(argv[1], "-start") == 0) {
			startHmpService();
		} else if (stricmp(argv[1], "-stop") == 0) {
			stopHmpService();
		} else if (stricmp(argv[1], "-restore") == 0) {
			RestoreDefaults();
		} else if (stricmp(argv[1], "-register") == 0) {
			registerDLL();
		} else {
			usage();
		}
	}
	return 0;
}

int startHmpService()
{
	int result = hmpStart();
	if (result < 0) ACE_OS::printf("MAIN could not start service\n");
	return result;
}

int stopHmpService()
{
	int result = hmpStop();
	if (result < 0) ACE_OS::printf("MAIN could not stop service\n");
	return result;
}

bool DeleteTheFile( LPCTSTR lpFileName )
{
	//Deletes the file
	bool rc = DeleteFile( lpFileName );
	
	if ( rc )
		printf( _T("Successfully deleted.\n") );
	else
		printf( _T("Couldn't delete. Error = %d\n"), GetLastError() );

	return rc;
}

void usage() {
	ACE_OS::printf("Usage: hmputil -start, hmputil -stop, hmputil -restore or hmputil -register\n");
}