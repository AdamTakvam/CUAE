// mmsMsg.h 
#ifndef MMS_MSG_H
#define MMS_MSG_H
#ifdef MMS_WINPLATFORM
#pragma once
#endif
#include "mms.h"
#include "mmsMsgTypes.h"



class MmsMsg: public ACE_Message_Block
//-----------------------------------------------------------------------------
// class MmsMsg: subset of ACE message utilized by the media server
//-----------------------------------------------------------------------------
{ public:                                   
                                            // Indicate if data block present 
  int isEmpty() const   {return this->base() == NULL;}
                                            
  ACE_TCHAR* safeText()                     // Get non-null pointer to payload
  { return this->base() == NULL? this->defaultText: this->base();
  }
                                            // Common ctor intialization
  // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
  // constructors: subset of ACE_Message_Block ctors applicable to our code
  // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
  MmsMsg(ACE_Allocator* a=0): ACE_Message_Block(a) 
  { this->msgInit();
  }

  MmsMsg(const char *data, size_t size=0): ACE_Message_Block(data, size) 
  { this->msgInit();
  }

  MmsMsg(size_t size, ACE_Message_Type type=MB_DATA, ACE_Message_Block* cont=0, 
         const char* data=0): ACE_Message_Block(size, type, cont, data) 
  { this->msgInit();
  }
         
  MmsMsg(const ACE_Message_Block &mb, size_t align): ACE_Message_Block(mb,align) 
  { this->msgInit();
  }

  void  msgInit()  
  { this->msgType = this->userParam = this->flags = 0; 
    *defaultText = '\0'; 
    sig = SIGNATURE_VALUE;
  }
                                            

  MmsMsg* releasex()                        // Ref count--, delete if zero
  {  
    if  (this->isPersistent()) return this; 
                                            // Sanity check 1
    if  (this->signature() != SIGNATURE_VALUE)
    {    MMSDEBUG((LM_EMERGENCY, ">>>> (%t) MMSMSG SIGNATURE bad (synch problem)\n"));
         ACE_ASSERT(this->signature() == SIGNATURE_VALUE);
         return this;
    }
    if  (this->reference_count() < 1)      // Sanity check 2
    {    MMSDEBUG((LM_EMERGENCY, ">>>> (%t) REFCOUNT already 0 (synch problem)\n"));
         ACE_ASSERT(this->reference_count() > 0);
         return this;
    }
    return (MmsMsg*)release();
  } 

  struct MMSMSGPARAMS                       // Params for message factory
  { unsigned int type;                       
    long         param;                      
    MMSMSGPARAMS(int t, long p) {type=t; param=p;}
    MMSMSGPARAMS() {memset(this, 0, sizeof(MMSMSGPARAMS));}
  }; 
            
  // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
  // MmsMsg instance data
  // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
  unsigned long signature() { return sig; }
  enum bitflags {PERSISTENT=1,STATIC=2,};
  enum {SIGNATURE_VALUE = 0x1e2d3b4a };
 
  protected:
  unsigned int  msgType;                    // Message type ala Windows WM_XXXX
  long          userParam;                  // Arbitrary message parameter 
  unsigned long sig;                        // Perhaps lose this once stable 

  #ifdef MMS_USING_MESSAGE_FACTORY 
  friend class mmsMsgFactory;
  #endif

  public:
  long param()          {return this->userParam;}     
  void param(long n)    {this->userParam = n;}  
  unsigned int type()   {return this->msgType;}
  void type(unsigned int n)    {this->msgType = n;}
  void makePersistent() {this->flags |= PERSISTENT; }
  int  isPersistent()   {return (this->flags & PERSISTENT) != 0;}
  int  isStatic()       {return (this->flags & STATIC) != 0;}

  private: 
  unsigned int flags;                       
  ACE_TCHAR defaultText[1];
 ~MmsMsg() {}                               // Ensure heap allocation  
};



#endif
