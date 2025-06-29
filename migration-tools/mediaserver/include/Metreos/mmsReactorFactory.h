// mmsReactorFactory.h 
// in ACE apps, frequent errors are the result of a reactor going out of scope
// prior to an event handler which has a reference to it. We find that having  
// a static reactor factory which allocates reactor instances on request and
// deletes them only as the application goes out of scope, prevents this 
// problem from occurring.
// 
#ifndef MMS_REACTORFACTORY_H
#define MMS_REACTORFACTORY_H
#ifdef MMS_WINPLATFORM
#pragma once
#endif
#include "StdAfx.h"
#include "mms.h"
#include <vector>



class ReactorFactory    
{ 
  public:
  ReactorFactory(): maxSize(0) { }
  virtual ~ReactorFactory() { this->removeAll(); }

  ACE_Reactor* create()
  {
    ACE_Guard<ACE_Thread_Mutex> x(theMutex);  
 
    ACE_Reactor* reactor = new ACE_Reactor;
    activeReactors.push_back(reactor);

    if ((int)activeReactors.size() > maxSize) maxSize = activeReactors.size(); 
    return reactor;
  }

  int remove(ACE_Reactor* p)
  {
    ACE_Guard<ACE_Thread_Mutex> x(theMutex);  
 
    int i = this->findByObjectPtr(p);
    if (i >= 0)  
    {   activeReactors.erase(activeReactors.begin() + i);
        p->close();
        delete p;
    }
    return i;
  }

  int removeAll()
  {
    ACE_Guard<ACE_Thread_Mutex> x(theMutex); 
  
    for(int i=0; i < (int)activeReactors.size(); i++)
    { ACE_Reactor* p = activeReactors[i];
      if (p)
         {p->close();
          delete p;
         }
    }  

    activeReactors.erase(activeReactors.begin(), activeReactors.end());
    return 0;
  }

  int findByObjectPtr(ACE_Reactor* p)
  { for(int i=0; i < (int)activeReactors.size(); i++)
        if  (activeReactors[i] == p) return i;
    return -1;
  }

  protected:
  std::vector<ACE_Reactor*> activeReactors;
  int  maxSize;
  ACE_Thread_Mutex theMutex;
};
    
#endif
