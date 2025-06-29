//=============================================================================
// Simple test of MMS memory mapped file mechanism for implementing synchronized
// inter-process communication using shared memory. This tests:
// (1) Writing shared memory in a process and reading it in a second process;
// (2) Creation of a lock in shared memory to synchronize the processes, and
// (3) Cleanup of shared memory. 
//
// To run this test, execute two copies of this executable. When prompted, 
// indicate whether each is the client or server process. Then when prompted,
// acquire or release locks. When one process has the lock, it writes to the
// shared object, and the other process should not be able to acquire the lock
// until you tell that process to release the lock.
//=============================================================================
#include "StdAfx.h"
#include "ace/PI_Malloc.h"
#include "ace/Process_Mutex.h"
#include "ace/Based_Pointer_T.h"
#include "ace/Synch.h"
#define WAITFORINPUT do{char c; cout << "Any character ... "; cin >> c;}while(0)
#define DESKTOPTEMPPATH "c:\\Documents and Settings\\james\\Desktop\\"
#define SERVER 1
#define CLIENT 2 
#define SERVERMASK   "SERVER%d"
#define CLIENTMASK   "CLIENT%d"
char msgmask[32], procname[32]; 
                                            // Get path to memory map file
int getTempfileFullpath(char* userbuf, const int buflen, const char* namepart)      
{                                            
  ACE_ASSERT(userbuf && buflen && namepart && *namepart); 
  char  localbuf[MAXPATHLEN+MAXPATHLEN];

  #ifdef DESKTOPTEMPPATH
  strcpy(localbuf, DESKTOPTEMPPATH);
  #else
  ACE_Lib_Find::get_temp_dir(localbuf, MAXPATHLEN);
  #endif

  ACE_OS::strcat(localbuf, namepart);
  const int userbuflen  = buflen - 1;
  const int localstrlen = ACE_OS::strlen(localbuf);
  const int copylen = localstrlen > userbuflen? userbuflen: localstrlen+1;
  ACE_OS::strncpy(userbuf, localbuf, copylen);
  return copylen;
}

int deleteMapFile(char* path)               // Delete memory map file
{
  #ifdef WIN32
  return remove(path);                      // unlink() does not work on this
  #else
											// file on Win32, perhaps due to
  return ACE_OS::unlink(path);              // lack of file extension?
  #endif
}


int getprocesstype()                        // Prompt user for whether this
{                                           // process is server or client
  char  c=0; 
  int   processtype=0;
  while(processtype == 0)
  {  ACE_DEBUG((LM_INFO,"Is this the server or client process (s|c)?")); 
     cin >> c;
     processtype = (c == 's')? SERVER: (c == 'c')? CLIENT: 0;
  }
  strcpy(msgmask,  processtype == SERVER? SERVERMASK: CLIENTMASK);
  return processtype;
}  

int gettimeout()                            // Prompt user for lock 
{                                           // acquisition timeout
   char s[2]={0,0};
   ACE_DEBUG((LM_INFO,"Lock timeout seconds (0 to block)?")); cin >> s[0];
   return ACE_OS::atoi(s);
}

#define BINDNAME     "foo"                  // Name to bind shared memory block
#define LOCKNAME     "foolock"              // Shared memory lock
#define OBJLOCKNAME  "objlock"              // Shared object lock
#define MMAPFILENAME "testmmap"             // Memory mapped file name
#define SHARED ((SHAREDOBJECT*)p)  
ACE_Process_Mutex* sharedmutex;
   

struct SHAREDOBJECT                         // Object written to shared memory
{ 
  enum {SIZE=64}; char c[SIZE];
  SHAREDOBJECT() { strcpy(c,"Initial state"); }
  ACE_Based_Pointer<ACE_Process_Mutex> lock;
};

                                            // Modify shared object
void writetosharedobject(SHAREDOBJECT* p, int n) 
{ 
  ACE_DEBUG ((LM_INFO,"acquired lock: found '%s'\n", p->c));
  ACE_OS::sprintf(p->c, msgmask, n); 
}

                                            // Shared memory allocator
typedef ACE_Malloc_T<ACE_LITE_MMAP_MEMORY_POOL, ACE_Process_Mutex,
                     ACE_PI_Control_Block> ALLOKATOR;

                                            // Map the shared memory
void* initSharedMemory(ALLOKATOR* allocator)
{
  SHAREDOBJECT sharedobject;                 
  void* p = allocator->malloc(sizeof(SHAREDOBJECT));
  memcpy(p, &sharedobject, sizeof(SHAREDOBJECT));
                                            // Allocate and attach lock                                            
  void* q = allocator->malloc(sizeof(ACE_Process_Mutex));
  ACE_Process_Mutex* mutex = new (q) ACE_Process_Mutex(OBJLOCKNAME);
  SHARED->lock = mutex;         
  sharedmutex  = mutex;

  allocator->bind(BINDNAME, p);              
  ACE_DEBUG((LM_INFO,"%x bound '%s'\n\n", p, BINDNAME));
  return p;
}
  


int main (int argc, char *argv[])
//-----------------------------------------------------------------------------
// main
//-----------------------------------------------------------------------------
{
  ACE_Trace::stop_tracing(); 
  char mmapPath[MAXPATHLEN];
  ACE_MMAP_Memory_Pool_Options options(ACE_DEFAULT_BASE_ADDR);
  void* p=0; 
   
  int processtype = getprocesstype();       // Are we server or client? 
  getTempfileFullpath(mmapPath, MAXPATHLEN, MMAPFILENAME);
                                      
  ACE_Time_Value timeoutsecs(gettimeout()); // Prompt for timeout
  ACE_Time_Value future;
 
  ALLOKATOR* allocator = new ALLOKATOR(mmapPath, LOCKNAME, &options);

  switch(processtype)                       // Look for shared memory:
  {
    case SERVER: 
         deleteMapFile(mmapPath);                  
         if (allocator->find(BINDNAME, p) == 0)
         {                                  // Found, not good
             ACE_DEBUG((LM_INFO,"found '%s': bailing\n", SHARED->c));
             allocator->free (p);           // Recover, maybe
             allocator->remove();
             delete allocator;
             WAITFORINPUT;
             return 0;
         }
                                            // Not found, good
         p = initSharedMemory(allocator);   // Install objects in shared mem
         break;

    case CLIENT:
         if  (allocator->find(BINDNAME, p) == 0)    
         {                                  // Found, good
              ACE_DEBUG((LM_INFO,"found shared '%s'\n", SHARED->c));
              sharedmutex = new ACE_Process_Mutex(OBJLOCKNAME);
              break;
         }                                  // Not found, bad
         ACE_DEBUG((LM_INFO,"shared memory not found: bailing\n"));

    default: 
         delete allocator;
         WAITFORINPUT;
         return 0;      
  }

  int n = 0; char c = 0, result = 0;

  while(c != 'q')                           // Loop manually creating
  {                                         // and releasing locks:
    ACE_DEBUG((LM_INFO,"acquire(a) try(t) release(r) quit(q)? ...")); 
    cin >> c;

    switch(c)
    { case 'a':
           future  = ACE_OS::gettimeofday();
           future += timeoutsecs;           // acquire uses absolute time  
           result  = timeoutsecs == ACE_Time_Value::zero?
                     sharedmutex->acquire():
                     sharedmutex->acquire(future);
           if  (result == 0)                // errno could be ETIMEDOUT
                writetosharedobject(SHARED,++n);
           else ACE_DEBUG((LM_INFO,"(%p\n", "acquire failed"));
           break;
      case 't':
           if  (sharedmutex->tryacquire () == 0)
                writetosharedobject(SHARED,++n);             
           else ACE_DEBUG((LM_INFO,"%p\n", "acquire failed"));
           break;
      case 'r':
           if  (sharedmutex->release () == 0)
                ACE_DEBUG((LM_INFO,"released OK\n"));
           else ACE_DEBUG((LM_INFO,"%p\n", "release failed"));
           break;         
      case 'q':
            ACE_DEBUG((LM_INFO,"final contents '%s'\n", SHARED->c));
            break;
      default: 
            ACE_DEBUG((LM_INFO,"not recognized\n"));
    }
  }

  if  (processtype == SERVER)
  {    allocator->free (p);
       allocator->remove();
  }
  else delete sharedmutex;

  delete allocator;
  WAITFORINPUT;
  return 0;
}    


