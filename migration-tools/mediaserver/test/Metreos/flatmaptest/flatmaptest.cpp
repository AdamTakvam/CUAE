#include "StdAfx.h" 
#pragma warning(disable:4786)
#include <iostream>
#include "C:\workspace\metreos-mediaserver\include\Metreos\mmsFlatMap.h"
#include "C:\workspace\metreos-mediaserver\include\Metreos\mmsParameterMap.h"
#define WAITFORINPUT do{char c; std::cout << "Any character ... "; std::cin >> c;}while(0)  
void walkMapSequential(char*);

// 1. Tests flatmap construction and access, both random and sequential.
// 2. Also tests duplicate key (multimap)
// 3. Also tests writing into a map parameter after map has been committed,
//    illustrating the method by which data will be returned in the same 
//    parameter map as was sent.
// 4. Also tests a map flatmap datatype of flatmap, that is, embedding a
//    flatmap within a flatmap; and a subsequent recursive walk of the maps.

#define S1024 "One-thousand twenty-four"    // Test data to be mapped
#define S0001 "One and only one"
#define S0200 "Two-hundred even"
#define SX200 "Second occurrence of 200"
#define D0500 "Five hundred not a string"
#define I0300 300
#define I0999 999
#define M0512 512
#define DPI   3.14159
#define LBIL  1000000000000
#define S400 "Embedded map four hundred"
#define S401 "Embedded map four-oh-one"


void show(int key, int len, int type, char* val)
{
  if  (len == 0) { printf("key %d not found\n", key); return; }
  char    buf[512]; 
  int     ival; 
  __int64 lval;
  double  dval; 

  switch(type)
  { case MmsFlatMap::datatype::BYTE:
         memset(buf,0,sizeof(buf));
         memcpy(buf,val,len);
         printf("key %04d value %s\n", key, buf);
         break;

    case MmsFlatMap::datatype::INT:
         ival = *((int*)val);
         printf("key %04d value %d\n", key, ival);
         break;

    case MmsFlatMap::datatype::STRING:
         printf("key %04d value %s\n", key, val);
         break;

    case MmsFlatMap::datatype::FLATMAP:
         printf("Key %04d is embedded flatmap, recursing ...\n",key);
         walkMapSequential(val);  
         break;

    case MmsFlatMap::datatype::LONG:
         lval = *((__int64*)val);
         printf("key %04d value %I64\n", key, lval);      
         break;

    case MmsFlatMap::datatype::DOUBLE:
         dval = *((double*)val);
         printf("key %04d value %6.5f\n", key, dval);
         break;
  }
}



void walkMapRandom(char* flatmap, int* keys, int numkeys)
{                                            
  MmsFlatMapReader map(flatmap);
  char* data;
  int   type, len, key;
                                            
  for(int i=0; i < numkeys; i++)              
  { 
    key = keys[i];
    len = map.find(key, &data, &type);
    show(key, len, type, data);
  }
}



void walkMapSequential(char* flatmap)
{
  MmsFlatMapReader map(flatmap);
  char* data;
  int   type, len, key;
                                            
  for(int i=0; i < map.size(); i++)
  {
    len = map.get(i, &key, &data, &type);
    show(key, len, type, data);
  }
}




int main(int argc, char* argv[])
{                                           
  char* data;
  int   type, len, key;

  MmsFlatMapWriter mapw2;                   // First build a small map to embed
  mapw2.insert(400, MmsFlatMap::datatype::STRING, sizeof(S400), S400);
  mapw2.insert(401, MmsFlatMap::datatype::STRING, sizeof(S401), S401);
  int embeddedMapLength = mapw2.length();

                                            // Build the main map
  MmsFlatMapWriter* mapw = new MmsFlatMapWriter;

  mapw->insert(1024, MmsFlatMap::datatype::STRING, sizeof(S1024),   S1024);
  mapw->insert(500 , MmsFlatMap::datatype::BYTE,   sizeof(D0500)-1, D0500);
  mapw->insert(200 , MmsFlatMap::datatype::STRING, sizeof(S0200),   S0200);
  mapw->insert(300 , I0300);
  mapw->insert(1   , MmsFlatMap::datatype::STRING, sizeof(S0001),   S0001);
  mapw->insert(200 , MmsFlatMap::datatype::STRING, sizeof(SX200),   SX200);
  mapw->insert(999 , I0999);

  mapw->insert(8, DPI);
  mapw->insert(16, LBIL);
                                            // Preformat a block for the
  char* embeddedMapArea = mapw->format(512, // small embedded flatmap parameter 
        MmsFlatMap::datatype::FLATMAP, embeddedMapLength);
                                            // Commit the small map into that
  mapw2.marshal(embeddedMapArea);           // preformatted area of the main map

  char* buf = new char[mapw->length()];     // Commit the main flatmap
  mapw->marshal(buf);

  delete mapw;
                                            // Access the map randomly
  printf("\n- -  Random access test  - -\n");                                    
  int keys[]={300,1,200,1024, 16, 8, 512,999,500};
  walkMapRandom(buf, keys, 7);
                                           
  MmsFlatMapReader map(buf);
  key = 200;
  len = map.find(key, &data, &type, 2);    // Find second occurrence of key 200
  show(key, len, type, data);
                                           // Rewrite data for that parameter
  initMapParameter(data, sizeof(SX200), ' ', 1);
  strncpy(data, "New data for second 200", len-1); 
                                           // Access the map sequentially
  printf("\n- -  Sequential access test  - -\n");
  walkMapSequential(buf);

  WAITFORINPUT;
  return 0;
}
