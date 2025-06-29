//
// mmsException.cpp 
//
// Unhandled exception handler (windows platform)
// 
#include "StdAfx.h"
#include "mmsException.h"

#ifdef MMS_WINPLATFORM

#ifdef MMS_ENABLE_MEMORY_LEAK_DETECTION
#define new MMS_MEMLEAK_NEW
#endif // MMS_ENABLE_MEMORY_LEAK_DETECTION


char MmsUnhandledExcpTrap::buf[MAX_PATH + 64];
char MmsUnhandledExcpTrap::dumpBasePath[MAX_PATH];
BOOL MmsUnhandledExcpTrap::isOverwriteDumpFile = 1;
MmsUnhandledExcpTrap* MmsUnhandledExcpTrap::singleton = NULL;


MmsUnhandledExcpTrap* MmsUnhandledExcpTrap::instance()
{
  if (!singleton)       
       singleton = new MmsUnhandledExcpTrap; 
 
  return singleton;
}



BOOL MmsUnhandledExcpTrap::setFilter(BOOL overwriteDumpFile, const char* dumpBasePath)
{
  MmsUnhandledExcpTrap::isOverwriteDumpFile = overwriteDumpFile != 0;
	ACE_OS::strcpy(MmsUnhandledExcpTrap::dumpBasePath, dumpBasePath);

  BOOL result = NULL != ::SetUnhandledExceptionFilter(excpFilter);

  return result;
}



long MmsUnhandledExcpTrap::excpFilter(EXCEPTION_POINTERS* info)
{
  // Top-level exception filter. Generates a .dmp file which can be read
  // and interpreted using visual studio .net.

  // This code will not be executed if a debugger is attached to the process.
  // If it becomes necessary to debug this method, we'll need to display 
  // state in some other manner. 
                               
  static const char* NODUMPMSG  = "- no dump taken";
  static const char* STOPMSG    = "EXCP emergency stop ...";

  do {
           
  HMODULE hmod = MmsUnhandledExcpTrap::getDbgHelpDll(); 
                                        
	if (!hmod)                                // Find dll with minidump export
  {   wsprintf(buf,"EXCP missing dump library %s\n", NODUMPMSG);  
      out(buf);
      break;
  }
 
	MINIDUMPWRITEDUMP pmdwd =                  
 (MINIDUMPWRITEDUMP)GetProcAddress(hmod, "MiniDumpWriteDump");

	if (!pmdwd)                               // Dll version too old     
  {   wsprintf(buf,"EXCP wrong dump library %s\n", NODUMPMSG); 
      out(buf);
      break;
  }

  makeDumpFilename();                       // File name -> buf

	char path[MAX_PATH];                       
	if (ACE_OS::strlen(MmsUnhandledExcpTrap::dumpBasePath) == 0)
	{
	  if (!Mms::getTempfileFullpath(path, MAX_PATH, buf))  
		{   wsprintf(buf,"EXCP path error %s\n", NODUMPMSG);
				out(buf);
				break;
		}
	}
	else
	{
		ACE_OS::strcpy(path, MmsUnhandledExcpTrap::dumpBasePath);
		if (*path) ensureTrailingSlash(path);    
		ACE_OS::strcat(path, buf);
	}
	
	// Open dump file
	HANDLE hfile = CreateFile(path, GENERIC_WRITE, FILE_SHARE_WRITE, NULL, 
                 CREATE_ALWAYS, FILE_ATTRIBUTE_NORMAL, NULL);

	if (INVALID_HANDLE_VALUE == hfile) 
  {   wsprintf(buf,"EXCP open error on %s %s\n", path, NODUMPMSG);
      out(buf);
      break;
  }

				 
	MINIDUMP_EXCEPTION_INFORMATION xinfo;
	xinfo.ThreadId = GetCurrentThreadId();
	xinfo.ExceptionPointers = info;
	xinfo.ClientPointers = NULL;
                                            // Invoke MiniDumpWriteDump
	BOOL result = pmdwd(GetCurrentProcess(), GetCurrentProcessId(), hfile, 
                MiniDumpWithHandleData, &xinfo, NULL, NULL);

  CloseHandle(hfile);

  if  (result)  
       wsprintf(buf,"%s dump written to %s\n", STOPMSG, path);
  else 
  {
    wsprintf(buf,"%s no dump could be written\n", STOPMSG);
    DeleteFile(path);
  }
  out(buf);

  } while(0);
                                            // Kill process
  ExitProcess(-1);                          // This takes 30 seconds or so

  return 0;                                 // Not reached
}
    


HMODULE MmsUnhandledExcpTrap::getDbgHelpDll()
{
  // Locate dbghelp.dll. With media server on Windows 2000, we keep the XP
  // version of the dll, having the needed exports, in the exe directory.
	HMODULE hmod = NULL;
	char path[MAX_PATH];

	if (GetModuleFileName(NULL, path, sizeof(path)))
	{                                         // Check home directory
		  char *p = _tcsrchr(path,'\\');
	  	if (p)
		  {  _tcscpy(p+1, DBGHELP_DLL);
			    hmod = LoadLibrary(path);
		  }
	}

	if (hmod == NULL)	 
		  hmod = LoadLibrary(DBGHELP_DLL);      // Check elsewhere
	                                         
	return hmod;
}



void MmsUnhandledExcpTrap::makeDumpFilename()
{
  if  (MmsUnhandledExcpTrap::isOverwriteDumpFile)
       wsprintf(buf, STATIC_DUMP_FILENAME);
  else
  {    long   l; time(&l);
       struct tm* t = ACE_OS::localtime(&l);
       wsprintf(buf, "%s%04d%02d%02d%02d%02d", "mms",
                t->tm_year+1900, t->tm_mon+1, t->tm_mday, t->tm_hour, t->tm_min);
  }

  ACE_OS::strcat(buf, MMS_DUMPFILE_EXTENSION);
}



int MmsUnhandledExcpTrap::out(char* msg, int rc)
{ 
  printf(buf);
  OutputDebugString(buf); 
  return rc;    
}



void MmsUnhandledExcpTrap::destroy()
{
  if (MmsUnhandledExcpTrap::singleton)
  {
      ::SetUnhandledExceptionFilter(NULL);
      delete MmsUnhandledExcpTrap::singleton;
      MmsUnhandledExcpTrap::singleton = NULL;
  }
}


#endif
