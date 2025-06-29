#include "StdAfx.h"
#include "ace/Hash_Map_Manager.h"
#define NUMSESSIONS 6
#define UUIDSIZE 36
// A simple test of the ACE hash map in a string to integer situation.
// We do a little more work here than necessary in order to parallel
// our real-world use case, which will involve allocating guids on the
// heap, storing the pointer with the corresponding session, mapping
// the guid to session ID, eventually unmapping same and deleting the
// guid allocation.                          
struct  session { int id; char* uuid; };
session sessions[NUMSESSIONS];
char*   uuids[NUMSESSIONS];

void load(int i, char* s) 
{ session* p = &sessions[i]; 
  p->id = i; 
  p->uuid = new char[UUIDSIZE+1];           // Alloc on heap to emulate
  strcpy(p->uuid, s);                       // the mms app situation

  uuids[i] = s;                             // Save the stack copy for lookups
}

void initSessions()
{
  load(0, "44EC053A-400F-11D0-9DCD-00A0C90391D3");
  load(1, "B6EA2051-048A-11D1-82B9-00C04FB9942E");
  load(2, "93BB06B6-B6DA-43C8-BC9B-E32DB49AA6F7");
  load(3, "6E871954-50AD-11D0-883E-080000185165");
  load(4, "21448B92-0788-11d0-8144-00A0C91BBEE3");
  load(5, "314111a0-a502-11d2-bbca-00c04f8ec294");
}
                                            // UUID to session ID hash
typedef ACE_Hash_Map_Entry<const char*, int> CONXID_HASHENTRY;

typedef ACE_Hash_Map_Manager_Ex<const char*, int, 
        ACE_Hash<const char*>, ACE_Equal_To<const char*>, ACE_Null_Mutex>
        CONXID_HASHMAP;


int main(int argc, char* argv[])
{
  initSessions();
                                            // Specify map size = session pool size
  CONXID_HASHMAP* mymap = new CONXID_HASHMAP(NUMSESSIONS);

  for(int i=0; i < NUMSESSIONS; i++)
  {
      if (mymap->bind(uuids[i], i) == 0) continue;
      printf("Error in bind\n");
      return -1;
  }
    
          
  for(i=0; i < NUMSESSIONS; i++)
  {
      int   internalID = -1;
      char* uuid = uuids[i];

      if (mymap->find(uuid, internalID) == -1) 
      {
          printf("Error in find\n");
          return -1;
      }
      printf("Matched %s with %d\n", uuid, internalID); 
  }

   
  printf("Map size is %d\n", mymap->current_size());


  for(i=0; i < NUMSESSIONS; i++)
  {
      session* thisSession = &sessions[i];
      int n = -1;
      
      if ((mymap->unbind(thisSession->uuid, n) == -1) || (n < 0)) 
      {
          printf("Error in unbind\n");
          return -1;
      }

      printf("Unbound %s\n", thisSession->uuid); 
      delete[] thisSession->uuid;
      thisSession->uuid = NULL;
  }

  printf("Map size is %d\n", mymap->current_size());
  delete mymap;
  return 0;
}
