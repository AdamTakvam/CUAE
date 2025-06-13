#include "stdafx.h"
#include "TestFlatmapIpcServer.h"
#define CUAE_STAT_SERVER_PORT 9201

using namespace Metreos;
using namespace Metreos::IPC;
void show(int key, int len, int type, char* val);
void walkMapSequential(FlatMapReader& map);
void walkMapSequential(char* flatmapbuf);
int  seqno;


TestFlatMapIpcServer::TestFlatMapIpcServer(const int port) : FlatMapIpcServer(port)
{
}

void TestFlatMapIpcServer::OnIncomingFlatMapMessage(const int messageType, 
  const FlatMapReader& flatmap, const char* data, size_t length, int sessionId)
{
  printf("TEST incoming flatmap %d for IPC session %d (%s) map type %d ...\n",
         ++seqno, sessionId, this->getRemoteAddress(sessionId), messageType);
  FlatMapReader r = flatmap;	
  walkMapSequential((FlatMapReader&)r);
  printf("\n");

  // char* pData = NULL;
  // int type;
  // int len = r.find(100, &pData, &type, 0);        // jld removed for stats testing

  FlatMapWriter map;
	// map.insert(100, FlatMap::STRING, len, pData); // jld removed for stats testing
  map.insert(100, FlatMap::BYTE, length, data);      // jld added for stats testing

  ACE_OS::sleep(1);

  Write(50, map, sessionId);
}



void TestFlatMapIpcServer::OnClientConnected(int sessionId)
{
   printf("TEST flatmap client %d (%s) connected\n",sessionId, this->getRemoteAddress(sessionId));
}



void TestFlatMapIpcServer::OnClientDisconnected(int sessionId)
{
   printf("TEST flatmap client %d (%s) disconnected\n",sessionId, this->getRemoteAddress(sessionId));
}



void TestFlatMapIpcServer::OnSocketFailure(int errorNumber, int id)
{
   printf("TEST socket failure on IPC client %d error %d\n",id,errorNumber);
}



char* TestFlatMapIpcServer::getRemoteAddress(const int ipcSessionID) 
{
  // Get IPC client's IP
  const static char* unknownIP = "unknown IP";
  char* fromIP = NULL;

  IpcSession* session = this->activeSessions[ipcSessionID];

  if (session)       
      fromIP = (char*)session->GetPeerAddr().get_host_addr();

  return fromIP? fromIP: (char*)unknownIP;
}



void TestFlatMapIpcServer::walkMapRandom(char* flatmap, int* keys, int numkeys)
{                                            
  FlatMapReader map(flatmap);
  char* data;
  int   type, len, key;
                                            
  for(int i=0; i < numkeys; i++)              
  { 
    key = keys[i];
    len = map.find(key, &data, &type);
    show(key, len, type, data);
  }
}



void TestFlatMapIpcServer::walkMapSequential(FlatMapReader& map)
{
  char* data;
  int   type, len, key;
                                            
  for(int i=0; i < map.size(); i++)
  {
    len = map.get(i, &key, &data, &type);
    show(key, len, type, data);
  }
}



void TestFlatMapIpcServer::walkMapSequential(char* flatmap)
{
  FlatMapReader map(flatmap);
  walkMapSequential((FlatMapReader&)map);
}



void TestFlatMapIpcServer::show(int key, int len, int type, char* val)
{
  if  (len == 0) { printf("TEST key %d not found\n", key); return; }
  char    buf[512]; 
  int     ival; 
  __int64 lval;
  double  dval; 

  switch(type)
  { case FlatMap::datatype::BYTE:
         memset(buf,0,sizeof(buf));
         memcpy(buf,val,len);
         printf("TEST key %04d value %s\n", key, buf);
         break;

    case FlatMap::datatype::INT:
         ival = *((int*)val);
         printf("TEST key %04d value %d\n", key, ival);
         break;

    case FlatMap::datatype::STRING:
         printf("TEST key %04d value %s\n", key, val);
         break;

    case FlatMap::datatype::FLATMAP:
         printf("TEST key %04d is embedded flatmap, recursing ...\n",key);
         walkMapSequential(val);  
         break;

    case FlatMap::datatype::LONG:
         lval = *((__int64*)val);
         printf("TEST key %04d value %I64\n", key, lval);      
         break;

    case FlatMap::datatype::DOUBLE:
         dval = *((double*)val);
         printf("TEST key %04d value %6.5f\n", key, dval);
         break;
  }
}



int main(int argc, char* argv[])
{
    printf("press 'q' to quit\n");

    TestFlatMapIpcServer server(CUAE_STAT_SERVER_PORT);
    // server.Start("127.0.0.1");  // jld removed for stats testing
    server.Start(NULL);            // jld added for stats testing

    char c;
    std::cin >> c;

    server.Stop();    
	return 0;
}

