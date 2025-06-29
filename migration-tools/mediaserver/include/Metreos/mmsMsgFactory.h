//=============================================================================
// mmsMsgFactory.h 
//=============================================================================
// A message factory allocates or accesses a message and returns it.
// This factory will be useful if and when we implement static messages.
// If we don't see a need for static messages, we should possibly go back to
// new'ing the MmsMsgs in the postMessage calls. However, even so, let's
// develop the server using the factory, which lets us localize the message
// allocation and deletion in a global context. If message release() is instead
// spread around in the various threads which host postMessage() it complicates
// debugging very considerably. Once the server is in production shape, we can
// revert if necessary to the postMessage new() and message loop release();
// to do so, remove the #define MMS_USING_MESSAGE_FACTORY from mms.h.   
//
// Note that a MmsMsg is 56 bytes. 40 static messages are ~ 2k
// We'll have to change the addref to a nop for static messages too.
//
// Each task could have its own message factory reference. That way if 
// a single static factory is a bottleneck, we can use additional factories.
//
// A thread should be able to get a reference to a static message, no  
// matter the type. The thread then is free to store the reference and 
// send the message as many times as needed, using putq instead of postMessage 
//
// A message can be both persistent and static
// If persistent, it is not deleted until end, but may be modified
// If static, it may not be modified.

// STATIC
// will have public dtor
// param(n) disabled
// type(n) disabled
// release() releasex() disabled
// duplicate() disabled
// deleted, never released

// PERSISTENT
// no restrictions, merely checked
// releasex(force) releases persistent message
// - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
#ifndef MMS_MSGFACTORY_H
#define MMS_MSGFACTORY_H
#ifdef MMS_WINPLATFORM
#pragma once
#endif
#include "StdAfx.h"
#include "mmsMsg.h"
//#pragma warning(disable:4786)
//#include <map>



class mmsMsgFactory
{ 
  public: 

  static mmsMsgFactory* instance()
  {
    if  (!singleton) singleton = new mmsMsgFactory;  
    return singleton;
  }

  virtual ~mmsMsgFactory() {if (singleton) delete singleton; }
  enum {MAX_PRIORITY_MESSAGES = 16};

  MmsMsg* get(MmsMsg::MMSMSGPARAMS* params);

  MmsMsg* get(MmsMsg::MMSMSGPARAMS* params, const char *data, size_t size=0);

  MmsMsg* get(MmsMsg::MMSMSGPARAMS* params, size_t size, 
    ACE_Message_Block::ACE_Message_Type type=ACE_Message_Block::MB_DATA, 
    ACE_Message_Block* cont=0, const char* data=0);

  MmsMsg* get(MmsMsg::MMSMSGPARAMS* params, const ACE_Message_Block &mb, size_t align);

  MmsMsg* release(MmsMsg* msg);
  
  MmsMsg* setPriority(MmsMsg* msg, MmsMsg::MMSMSGPARAMS* params);
  int  getMessagePriority(const int msgnum) const;

  protected:
  static mmsMsgFactory* singleton;
  mmsMsgFactory() {}
           
  // std::map<unsigned int,MmsMsg*> staticMessages;

  ACE_Recursive_Thread_Mutex getMutex;
  ACE_Recursive_Thread_Mutex relMutex;
  typedef ACE_Guard<ACE_Recursive_Thread_Mutex> msgFactoryGuard;
};



#endif

