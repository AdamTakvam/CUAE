#include "resip/stack/MiniThread.hxx"
#include "resip/stack/SipStack.hxx"
#include "resip/stack/SipMessage.hxx"
#include "rutil/Logger.hxx"

#define RESIPROCATE_SUBSYSTEM Subsystem::SIP

using namespace resip;

MiniThread::MiniThread(SipStack& stack)
   : mStack(stack)
{}

MiniThread::MiniThread(SipStack& stack, int groupNumber)
   : mStack(stack), mGroup(groupNumber)
{
}

MiniThread::~MiniThread()
{
}

void
MiniThread::thread()
{
	miniThread();
}

void
MiniThread::miniThread()
{
   while (!isShutdown() && mStack.isStopped == false)
   {
	   mStack.acquireCriticalSection();

      try
      {
		 //InfoLog ( << "---------------------------------------------" << mId);
		 //InfoLog ( << "[StackThread] keep running group: " << mGroup);
		 //InfoLog ( << "[StackThread] # transport: " << mStack.getSumFdSet());
		 //InfoLog ( << "---------------------------------------------");  
		 //in case there are junk transports wating to be removed
		 mStack.removeJunkTransports();

         resip::FdSet fdset;
         buildFdSet(fdset);
         mStack.buildFdSet(fdset, mGroup);
		 int ret = fdset.selectMilliSeconds(resipMin(mStack.getTimeTillNextProcessMS(),
                                                     getTimeTillNextProcessMS()));

		 //InfoLog (<< "StackThread ret=" << ret);

		 //after stack starts, without adding any transport to the stack, 
		 //mStack.buildFdSet will return with an empty fdset. when fdset is 
		 //empty, fdset.selectMilliSeconds will return with -1. At this point, 
		 //if there is any command message in mStateMacFifo waiting to be processed,
		 //and if we keep the following check, the message will be stuck forever.
		 //         if (ret >= 0)
         {
            // .dlb. use return value to peak at the message to see if it is a
            // shutdown, and call shutdown if it is
            mStack.process(fdset);
         }
      }
      catch (BaseException& e)
      {
         InfoLog (<< "Unhandled exception: " << e);
      }
	  mStack.releaseCriticalSection();
	  Sleep(10);
   }
   WarningLog (<< "Shutting down stack thread");
}

void
MiniThread::buildFdSet(FdSet& fdset)
{}

unsigned int
MiniThread::getTimeTillNextProcessMS() const
{
//   !dcm! moved the 25 ms min logic here
//   return INT_MAX;
   return 25;   
}
