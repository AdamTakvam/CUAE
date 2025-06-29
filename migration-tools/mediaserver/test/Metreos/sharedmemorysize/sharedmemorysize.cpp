//=============================================================================
// Test capacity of MMS memory mapped file mechanism.
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
                                            // Specify maximum shared memory   
                                            // usage you want to test for here:
#define MAXPAYLOADS  256                    // Number of concurrent allocations
#define PAYLOADSIZE 4096                    // Size of each allocation chunk

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



#define BINDNAME     "foobar"               // Name to bind shared memory block
#define LOCKNAME     "foobarlock"           // Shared memory lock
#define OBJLOCKNAME  "foobarobjlock"        // Shared object lock
#define MMAPFILENAME "foobar"               // Memory mapped file name
#define SHARED ((SHAREDMEMORYMAP*)p)  
ACE_Process_Mutex* sharedmutex;



struct SHAREDOBJECT                         // Shared memory paylaod object 
{ 
  char c[PAYLOADSIZE];
  SHAREDOBJECT() { memset(c, 0, PAYLOADSIZE); strcpy(c,"Initial state"); }
};



struct SHAREDMEMORYMAP                      // Shared memory map object    
{ 
  SHAREDMEMORYMAP() { memset(this,0,sizeof(SHAREDMEMORYMAP)); currentObject = -1; }
  int currentObject;
  ACE_Based_Pointer<ACE_Process_Mutex> lock;
                                            // Array of payloads
  ACE_Based_Pointer<SHAREDOBJECT> sharedobject[MAXPAYLOADS];
};
   


                                            // Modify shared object
void writetosharedobject(SHAREDOBJECT* p, int n) 
{ 
  //ACE_DEBUG((LM_INFO,"acquired lock: found '%s'\n", p->c));
  memset(p, 0, PAYLOADSIZE);                // Verify write of entire object
  ACE_OS::sprintf(p->c, msgmask, n);        // Write ID string
}

                                            // MMS shared memory allocator
typedef ACE_Malloc_T<ACE_LITE_MMAP_MEMORY_POOL, ACE_Process_Mutex,
                     ACE_PI_Control_Block> ALLOKATOR;


                                            // Map the shared memory
void* initSharedMemory(ALLOKATOR* allocator)
{
  SHAREDMEMORYMAP map;                 
  void*  p = allocator->malloc(sizeof(SHAREDMEMORYMAP));
  memcpy(p, &map, sizeof(SHAREDMEMORYMAP));
                                            // Allocate and attach lock                                            
  void*  q = allocator->malloc(sizeof(ACE_Process_Mutex));
  ACE_Process_Mutex* mutex = new (q) ACE_Process_Mutex(OBJLOCKNAME);
  SHARED->lock = mutex;         
  sharedmutex  = mutex;
  int totalbytes = 0;

  for(int i = 0; i < MAXPAYLOADS; i++)
  {
    ACE_DEBUG((LM_INFO,"total allocation is %d, allocating %d\n",totalbytes,i));

    q  = allocator->malloc(sizeof(SHAREDOBJECT));
    if  (q < (void*)1)
    {
         ACE_DEBUG((LM_INFO,"allocation failed after %d bytes\n",totalbytes));
         return NULL;
    }

    totalbytes += sizeof(SHAREDOBJECT);

    SHAREDOBJECT* x = new(q) SHAREDOBJECT;
    if  (x < (void*)1)
    {
         ACE_DEBUG((LM_INFO,"allocation failed after %d bytes\n",totalbytes));
         return NULL;
    }

    SHARED->sharedobject[i] = x;
  }

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
  void* sharedmem = 0; 
  int start, end;
   
  int processtype = getprocesstype();       // Are we server or client? 
  getTempfileFullpath(mmapPath, MAXPATHLEN, MMAPFILENAME);
                                      
  //ACE_Time_Value timeoutsecs(gettimeout()); // Prompt for timeout
  //ACE_Time_Value future;
 
  ALLOKATOR* allocator = new ALLOKATOR(mmapPath, LOCKNAME, &options);

  switch(processtype)                       // Look for shared memory:
  {
    case SERVER:                             
         if (allocator->find(BINDNAME, sharedmem) == 0)
         {                                  // Found, not good
             ACE_DEBUG((LM_INFO,"found bind name: bailing\n"));
             allocator->free(sharedmem);    // Recover, maybe
             allocator->remove();
             delete allocator;
             WAITFORINPUT;
             return 0;
         }                                  // Not found, good
                                            // Install max objects in shared mem
         sharedmem = initSharedMemory(allocator);   
         if (sharedmem == NULL)
         {
             ACE_DEBUG((LM_INFO,"shared memory allocation failed: bailing\n"));
             allocator->free(sharedmem);    // Recover, maybe
             allocator->remove();
             delete allocator;
             WAITFORINPUT;
             return 0;
         }

         start = 0; end = MAXPAYLOADS-1;
         break;

    case CLIENT:
         if  (allocator->find(BINDNAME, sharedmem) == 0)    
         {                                  // Found, good
              ACE_DEBUG((LM_INFO,"client found shared memory %x\n",sharedmem));
              sharedmutex = new ACE_Process_Mutex(OBJLOCKNAME);
              start = MAXPAYLOADS-1; end = 0;
              break;
         }                                  // Not found, bad
         ACE_DEBUG((LM_INFO,"shared memory not found: bailing\n"));

    default: 
         delete allocator;
         WAITFORINPUT;
         return 0;      
  }

  int i = start;
  SHAREDMEMORYMAP* sharedmap = (SHAREDMEMORYMAP*)sharedmem;

  while(1)
  {
    if  (sharedmutex->acquire() == -1)
        {ACE_DEBUG ((LM_INFO, "%p\n", "acquire failed ... bailing"));
         break;
        }

    SHAREDOBJECT* obj = sharedmap->sharedobject[i];

    ACE_DEBUG((LM_INFO,"Found %s at %d\n",obj->c, i));

    writetosharedobject(obj, i);

    if  (sharedmutex->release() == -1)
        {ACE_DEBUG ((LM_INFO, "%p\n", "release failed ... bailing"));
         break;
        }

    if  (processtype == SERVER)
         i++;
    else i--;
    if  (i == end) break;
  }


  if  (processtype == SERVER)
  {    WAITFORINPUT;
       allocator->free(sharedmem);
       allocator->remove();
  }
  else delete sharedmutex;

  delete allocator;
  WAITFORINPUT;
  return 0;
}    


