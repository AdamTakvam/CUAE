//=============================================================================
// 
//=============================================================================
#include "StdAfx.h"
#include <iostream>
#include "C:\workspace\metreos-mediaserver\include\Metreos\mmsConfig.h"
//#define WAITFORINPUT do{char c; std::cout << "Any character ... "; std::cin >> c;}while(0)

  

int main (int argc, char *argv[])
//-----------------------------------------------------------------------------
// main
//-----------------------------------------------------------------------------
{
  ACE_Trace::stop_tracing(); 
  MmsConfig* config = new MmsConfig;        // Get config defaults

  int  itemcount = config->readLocalConfigFile();
  if  (itemcount == -1) 
       std::cout << "Could not read/open config file, path likely wrong\n"; 
  else std::cout << itemcount << " config file items recognized\n";  
  delete config;
  WAITFORINPUT;
  return 0;
}    


