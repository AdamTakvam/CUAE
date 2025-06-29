//
// mmsException.h 
//
// Unhandled exception handler (windows platform)
// 
#ifndef MMS_EXCEPTION_H
#define MMS_EXCEPTION_H
#ifdef MMS_WINPLATFORM
#pragma once
#endif
#include "mms.h"

#ifdef MMS_WINPLATFORM

#include <windows.h>
#include <tchar.h>
#define DBGHELP_DLL            "dbghelp.dll"
#define STATIC_DUMP_FILENAME   "mmsdump"
#define MMS_DUMPFILE_EXTENSION ".dmp"



class MmsUnhandledExcpTrap
{
  public:
  
  static MmsUnhandledExcpTrap* instance();

  BOOL setFilter(BOOL overwriteDumpFile=1, const char* dumpBasePath = NULL);

  virtual ~MmsUnhandledExcpTrap() { }

  static void destroy();

  protected:
  static MmsUnhandledExcpTrap* singleton;
  MmsUnhandledExcpTrap() {}
  static BOOL isOverwriteDumpFile;
  static void makeDumpFilename();

  static long WINAPI excpFilter(EXCEPTION_POINTERS* info);
  static HMODULE getDbgHelpDll();
  static int  out(char* msg, int rc=0);
  static char buf[MAX_PATH + 64];
	static char dumpBasePath[MAX_PATH];
};


// - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
// excerpts from XP dbghelp.h 
// - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -


typedef enum _MINIDUMP_TYPE 
{ MiniDumpNormal                         = 0x00000000,
  MiniDumpWithDataSegs                   = 0x00000001,
  MiniDumpWithFullMemory                 = 0x00000002,
  MiniDumpWithHandleData                 = 0x00000004,
  MiniDumpFilterMemory                   = 0x00000008,    
  MiniDumpScanMemory                     = 0x00000010,  
  MiniDumpWithUnloadedModules            = 0x00000020,
  MiniDumpWithIndirectlyReferencedMemory = 0x00000040,
  MiniDumpFilterModulePaths              = 0x00000080,
  MiniDumpWithProcessThreadData          = 0x00000100,
  MiniDumpWithPrivateReadWriteMemory     = 0x00000200,
  MiniDumpWithoutOptionalData            = 0x00000400,
  MiniDumpWithFullMemoryInfo             = 0x00000800,
  MiniDumpWithThreadInfo                 = 0x00001000,
  MiniDumpWithCodeSegs                   = 0x00002000
} MINIDUMP_TYPE;


struct MINIDUMP_EXCEPTION_INFORMATION 
{                                           // dbghelp.h
  DWORD ThreadId;
  EXCEPTION_POINTERS* ExceptionPointers;    // winbase.h
  BOOL  ClientPointers;
};


struct MINIDUMP_USER_STREAM
{
  unsigned int  Type;
  unsigned long BufferSize;
  void*         Buffer;
};


struct MINIDUMP_USER_STREAM_INFORMATION 
{
  unsigned long UserStreamCount;
  MINIDUMP_USER_STREAM* UserStreamArray;
};


struct MINIDUMP_THREAD_CALLBACK 
{
  unsigned long ThreadId;
  HANDLE  ThreadHandle;
  CONTEXT Context;  // winnt.h
  unsigned long SizeOfContext;
  unsigned __int64 StackBase;
  unsigned __int64 StackEnd;
};


struct MINIDUMP_THREAD_EX_CALLBACK 
{
  unsigned long ThreadId;
  HANDLE  ThreadHandle;
  CONTEXT Context;
  unsigned long SizeOfContext;
  unsigned __int64 StackBase;
  unsigned __int64 StackEnd;
  unsigned __int64 BackingStoreBase;
  unsigned __int64 BackingStoreEnd;
};


struct MINIDUMP_MODULE_CALLBACK
{
  WCHAR* FullPath;
  unsigned __int64 BaseOfImage;
  unsigned long SizeOfImage;
  unsigned long CheckSum;
  unsigned long TimeDateStamp;
  VS_FIXEDFILEINFO VersionInfo;
  void* CvRecord; 
  unsigned long SizeOfCvRecord;
  void* MiscRecord;
  unsigned long SizeOfMiscRecord;
};



struct MINIDUMP_INCLUDE_THREAD_CALLBACK 
{
  unsigned long ThreadId;
};


struct MINIDUMP_INCLUDE_MODULE_CALLBACK 
{
  unsigned __int64 BaseOfImage;
};



struct MINIDUMP_CALLBACK_INPUT 
{
  unsigned long ProcessId;
  HANDLE ProcessHandle;
  unsigned long CallbackType;
  union 
  { MINIDUMP_THREAD_CALLBACK Thread;
    MINIDUMP_THREAD_EX_CALLBACK ThreadEx;
    MINIDUMP_MODULE_CALLBACK Module;
    MINIDUMP_INCLUDE_THREAD_CALLBACK IncludeThread;
    MINIDUMP_INCLUDE_MODULE_CALLBACK IncludeModule;
  };
};


struct MINIDUMP_CALLBACK_OUTPUT 
{ union 
  { unsigned long ModuleWriteFlags;
    unsigned long ThreadWriteFlags;
  };
};


typedef BOOL (WINAPI *MINIDUMP_CALLBACK_ROUTINE) (void*,
              const MINIDUMP_CALLBACK_INPUT*, MINIDUMP_CALLBACK_OUTPUT*);


struct MINIDUMP_CALLBACK_INFORMATION 
{
  MINIDUMP_CALLBACK_ROUTINE CallbackRoutine;
  void* CallbackParam;
};


typedef BOOL (WINAPI *MINIDUMPWRITEDUMP)(HANDLE, DWORD, HANDLE, MINIDUMP_TYPE,
							const MINIDUMP_EXCEPTION_INFORMATION*,
							const MINIDUMP_USER_STREAM_INFORMATION*,
							const MINIDUMP_CALLBACK_INFORMATION*);

   
#endif
#endif

