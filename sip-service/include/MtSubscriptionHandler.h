#ifndef MtSubscriptionHandler_H_LOADED
#define MtSubscriptionHandler_H_LOADED

#include "resip/dum/SubscriptionHandler.hxx"

using namespace resip;

namespace Metreos
{

namespace Sip
{
class MtSipStackRuntime;
class MtClientSubscriptionHandler : public ClientSubscriptionHandler
{
  public:
	  MtClientSubscriptionHandler(MtSipStackRuntime *pr) : m_pRuntime(pr) { }
      virtual ~MtClientSubscriptionHandler() { }

      virtual void onRefreshRejected(ClientSubscriptionHandle, const SipMessage& rejection);

      //Client must call acceptUpdate or rejectUpdate for any onUpdateFoo
      virtual void onUpdatePending(ClientSubscriptionHandle, const SipMessage& notify, bool outOfOrder);
      virtual void onUpdateActive(ClientSubscriptionHandle, const SipMessage& notify, bool outOfOrder);
      //unknown Subscription-State value
      virtual void onUpdateExtension(ClientSubscriptionHandle, const SipMessage& notify, bool outOfOrder);      

      virtual int onRequestRetry(ClientSubscriptionHandle, int retrySeconds, const SipMessage& notify);
      
      //subscription can be ended through a notify or a failure response.
      virtual void onTerminated(ClientSubscriptionHandle, const SipMessage& msg);   
      //not sure if this has any value.
      virtual void onNewSubscription(ClientSubscriptionHandle, const SipMessage& notify);

private:
	MtSipStackRuntime	*m_pRuntime;
	static Data DIGITS_TAG;
};

class MtServerSubscriptionHandler : public ServerSubscriptionHandler
{
  public:   
      MtServerSubscriptionHandler() {}
      virtual ~MtServerSubscriptionHandler() {}

      virtual void onNewSubscription(ServerSubscriptionHandle h, const SipMessage& sub);
 
      //called when this usage is destroyed for any reason. One of the following
      //three methods will always be called before this, but this is the only
      //method that MUST be implemented by a handler
      virtual void onTerminated(ServerSubscriptionHandle h);

      //will be called when a NOTIFY is not delivered(with a usage terminating
      //statusCode), or the Dialog is destroyed
      virtual void onError(ServerSubscriptionHandle, const SipMessage& msg);      

	  virtual void onReadyToSend(ServerSubscriptionHandle h, SipMessage& msg);
};

}

}

#endif
