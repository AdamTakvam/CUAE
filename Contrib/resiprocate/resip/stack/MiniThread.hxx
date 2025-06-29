#ifndef RESIP_MiniThread__hxx
#define RESIP_MiniThread__hxx

#include "rutil/ThreadIf.hxx"
#include "rutil/Socket.hxx"

namespace resip
{

class SipStack;

/** 
    This class is used to create a thread to run the SipStack in.  The
    thread provides cycles to the SipStack by calling process.  Process
    is called at least every 25ms, or sooner if select returns a signaled
    file descriptor.
*/
class MiniThread : public ThreadIf
{
   public:
      MiniThread(SipStack& stack);
	  MiniThread(SipStack& stack, int groupNumber);
      virtual ~MiniThread();
      
      virtual void thread();

   protected:
      virtual void buildFdSet(FdSet& fdset);
      virtual unsigned int getTimeTillNextProcessMS() const;

   private:
      SipStack& mStack;
	  int mGroup;
	  void miniThread();
};

}

#endif

