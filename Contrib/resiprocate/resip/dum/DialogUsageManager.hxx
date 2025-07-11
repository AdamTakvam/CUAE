#if !defined(RESIP_DIALOGUSAGEMANAGER_HXX)
#define RESIP_DIALOGUSAGEMANAGER_HXX

#include <vector>
#include <set>
#include <map>

#include "resip/stack/Headers.hxx"
#include "resip/dum/EventDispatcher.hxx"
#include "resip/dum/DialogSet.hxx"
#include "resip/dum/DumTimeout.hxx"
#include "resip/dum/HandleManager.hxx"
#include "resip/dum/Handles.hxx"
#include "resip/dum/MergedRequestKey.hxx"
#include "resip/dum/RegistrationPersistenceManager.hxx"
#include "resip/dum/EncryptionManager.hxx"
#include "resip/dum/ServerSubscription.hxx"
#include "rutil/BaseException.hxx"
#include "rutil/SharedPtr.hxx"
#include "resip/stack/SipStack.hxx"
#include "resip/stack/TransactionUser.hxx"
#include "resip/dum/DumFeature.hxx"
#include "resip/dum/DumFeatureChain.hxx"
#include "resip/dum/DumFeatureMessage.hxx"
#include "resip/dum/TargetCommand.hxx"
#include "resip/dum/ClientSubscriptionFunctor.hxx"
#include "resip/dum/ServerSubscriptionFunctor.hxx"

namespace resip 
{

class Security;
class SipStack;
class FdSet;
class MasterProfile;
class RedirectManager;
class ClientAuthManager;
class ServerAuthManager;
class Uri;
class SdpContents;

class ClientRegistrationHandler;
class ServerRegistrationHandler;
class InviteSessionHandler;
class ClientSubscriptionHandler;
class ServerSubscriptionHandler;
class ClientPublicationHandler;
class ServerPublicationHandler;
class ClientPagerMessageHandler;
class ServerPagerMessageHandler;
class OutOfDialogHandler;
class RedirectHandler;
class DialogSetHandler;

class Dialog;
class InviteSessionCreator;

class AppDialogSetFactory;
class DumShutdownHandler;

class KeepAliveManager;
class HttpGetMessage;

class ConnectionTerminated;

class DialogUsageManager : public HandleManager, public TransactionUser
{
   public:
      class Exception : public BaseException
      {
         public:
            Exception(const Data& msg,
                      const Data& file,
                      int line)
               : BaseException(msg, file, line)
            {}
            
            virtual const char* name() const {return "DialogUsageManager::Exception";}
      };

      typedef enum
      {
         None = 0,
         Sign,
         Encrypt,
         SignAndEncrypt
      } EncryptionLevel;
  
      // If createDefaultFeatures is true dum will construct a
      // IdentityHandler->EncryptionManager chain.
      DialogUsageManager(SipStack& stack, bool createDefaultFeatures=false);
      virtual ~DialogUsageManager();
            
      void shutdown(DumShutdownHandler*, unsigned long giveUpSeconds=0);
      void shutdownIfNoUsages(DumShutdownHandler*, unsigned long giveUpSeconds=0);
      void forceShutdown(DumShutdownHandler*);

      void addTransport( TransportType protocol,
                         int port=0, 
                         IpVersion version=V4,
                         const Data& ipInterface = Data::Empty, 
                         const Data& sipDomainname = Data::Empty, // only used
                                                                  // for TLS
                                                                  // based stuff 
                         const Data& privateKeyPassPhrase = Data::Empty,
                         SecurityTypes::SSLType sslType = SecurityTypes::TLSv1,
						 Socket fd = INVALID_SOCKET
						 );

      void removeTransport( TransportType protocol,
                         int port, 
                         IpVersion version,
                         const Data& ipInterface);

      SipStack& getSipStack();
      Security* getSecurity();
      
      Data getHostAddress();

      void setAppDialogSetFactory(std::auto_ptr<AppDialogSetFactory>);

      void setMasterProfile(const SharedPtr<MasterProfile>& masterProfile);
      SharedPtr<MasterProfile>& getMasterProfile();
      SharedPtr<UserProfile>& getMasterUserProfile();
      
      //optional handler to track the progress of DialogSets
      void setDialogSetHandler(DialogSetHandler* handler);

      void setKeepAliveManager(std::auto_ptr<KeepAliveManager> keepAlive);

      //There is a default RedirectManager.  Setting one may cause the old one
      //to be deleted. 
      void setRedirectManager(std::auto_ptr<RedirectManager> redirect);
      //informational, so a RedirectHandler is not required
      void setRedirectHandler(RedirectHandler* handler);      
      RedirectHandler* getRedirectHandler();      

      /// If there is no ClientAuthManager, when the client receives a 401/407,
      /// pass it up through the normal BaseUsageHandler
      void setClientAuthManager(std::auto_ptr<ClientAuthManager> client);

      /// If there is no ServerAuthManager, the server does not authenticate requests
      void setServerAuthManager(resip::SharedPtr<ServerAuthManager> server);

      /// If there is no such handler, calling makeInviteSession will throw and
      /// receiving an INVITE as a UAS will respond with 405 Method Not Allowed
      void setInviteSessionHandler(InviteSessionHandler*);
      
      /// If there is no such handler, calling makeRegistration will throw
      void setClientRegistrationHandler(ClientRegistrationHandler*);

      /// If no such handler, UAS will respond to REGISTER with 405 Method Not Allowed
      void setServerRegistrationHandler(ServerRegistrationHandler*);

      /// If there is no such handler, calling makeSubscription will throw
      void addClientSubscriptionHandler(const Data& eventType, ClientSubscriptionHandler*);

      /// If there is no such handler, calling makePublication will throw
      void addClientPublicationHandler(const Data& eventType, ClientPublicationHandler*);
      
      void addServerSubscriptionHandler(const Data& eventType, ServerSubscriptionHandler*);
      void addServerPublicationHandler(const Data& eventType, ServerPublicationHandler*);
      
      void addOutOfDialogHandler(MethodTypes, OutOfDialogHandler*);

      void setClientPagerMessageHandler(ClientPagerMessageHandler*);
      void setServerPagerMessageHandler(ServerPagerMessageHandler*);

      /// Sets a manager to handle storage of registration state
      void setRegistrationPersistenceManager(RegistrationPersistenceManager*);

      void setRemoteCertStore(std::auto_ptr<RemoteCertStore> store);
      
      // The message is owned by the underlying datastructure and may go away in
      // the future. If the caller wants to keep it, it should make a copy. The
      // memory will exist at least up until the point where the application
      // calls DialogUsageManager::send(msg);
      SharedPtr<SipMessage> makeInviteSession(const NameAddr& target, const SharedPtr<UserProfile>& userProfile, const SdpContents* initialOffer, AppDialogSet* ads = 0);
      SharedPtr<SipMessage> makeInviteSession(const NameAddr& target, const SdpContents* initialOffer, AppDialogSet* ads = 0);
      SharedPtr<SipMessage> makeInviteSession(const NameAddr& target, const SharedPtr<UserProfile>& userProfile, const SdpContents* initialOffer, EncryptionLevel level, const SdpContents* alternative = 0, AppDialogSet* ads = 0);
      SharedPtr<SipMessage> makeInviteSession(const NameAddr& target, const SdpContents* initialOffer, EncryptionLevel level, const SdpContents* alternative = 0, AppDialogSet* ads = 0);

      
      //will send a Notify(100)...currently can be decorated through the
      //OnReadyToSend callback.  Probably will change it's own callback/handler soon
      SharedPtr<SipMessage> makeInviteSessionFromRefer(const SipMessage& refer, ServerSubscriptionHandle, 
                                                       const SdpContents* initialOffer, AppDialogSet* = 0);
      SharedPtr<SipMessage> makeInviteSessionFromRefer(const SipMessage& refer, ServerSubscriptionHandle, 
                                                       const SdpContents* initialOffer, EncryptionLevel level, const SdpContents* alternative, AppDialogSet* = 0);
      
      SharedPtr<SipMessage> makeSubscription(const NameAddr& target, const SharedPtr<UserProfile>& userProfile, const Data& eventType, AppDialogSet* = 0);
      SharedPtr<SipMessage> makeSubscription(const NameAddr& target, const SharedPtr<UserProfile>& userProfile, const Data& eventType, 
                                             int subscriptionTime, AppDialogSet* = 0);
      SharedPtr<SipMessage> makeSubscription(const NameAddr& target, const SharedPtr<UserProfile>& userProfile, const Data& eventType, 
                                             int subscriptionTime, int refreshInterval, AppDialogSet* = 0);
      SharedPtr<SipMessage> makeSubscription(const NameAddr& target, const Data& eventType, AppDialogSet* = 0);
      SharedPtr<SipMessage> makeSubscription(const NameAddr& target, const Data& eventType, int subscriptionTime, AppDialogSet* = 0);
      SharedPtr<SipMessage> makeSubscription(const NameAddr& target, const Data& eventType, 
                                             int subscriptionTime, int refreshInterval, AppDialogSet* = 0);

      //unsolicited refer
      SharedPtr<SipMessage> makeRefer(const NameAddr& target, const SharedPtr<UserProfile>& userProfile, const H_ReferTo::Type& referTo, AppDialogSet* = 0);
      SharedPtr<SipMessage> makeRefer(const NameAddr& target, const H_ReferTo::Type& referTo, AppDialogSet* = 0);

      SharedPtr<SipMessage> makePublication(const NameAddr& target, 
                                            const SharedPtr<UserProfile>& userProfile, 
                                            const Contents& body, 
                                            const Data& eventType, 
                                            unsigned expiresSeconds, 
                                            AppDialogSet* = 0);
      SharedPtr<SipMessage> makePublication(const NameAddr& target, 
                                            const Contents& body, 
                                            const Data& eventType, 
                                            unsigned expiresSeconds, 
                                            AppDialogSet* = 0);

      SharedPtr<SipMessage> makeRegistration(const NameAddr& target, const SharedPtr<UserProfile>& userProfile, AppDialogSet* = 0);
      SharedPtr<SipMessage> makeRegistration(const NameAddr& target, const SharedPtr<UserProfile>& userProfile, int registrationTime, AppDialogSet* = 0);
      SharedPtr<SipMessage> makeRegistration(const NameAddr& target, AppDialogSet* = 0);
      SharedPtr<SipMessage> makeRegistration(const NameAddr& target, int registrationTime, AppDialogSet* = 0);

      SharedPtr<SipMessage> makeOutOfDialogRequest(const NameAddr& target, const SharedPtr<UserProfile>& userProfile, const MethodTypes meth, AppDialogSet* = 0);
      SharedPtr<SipMessage> makeOutOfDialogRequest(const NameAddr& target, const MethodTypes meth, AppDialogSet* = 0);

      ClientPagerMessageHandle makePagerMessage(const NameAddr& target, const SharedPtr<UserProfile>& userProfile, AppDialogSet* = 0);
      ClientPagerMessageHandle makePagerMessage(const NameAddr& target, AppDialogSet* = 0);
      
      void end(DialogSetId invSessionId);
      void send(SharedPtr<SipMessage> request);
      //void send(SipMessage& request, EncryptionLevel level);
      
      // give dum an opportunity to handle its events. If process() returns true
      // there are more events to process. 
      bool process();

      InviteSessionHandle findInviteSession(DialogId id);
      //if the handle is inValid, int represents the errorcode
      std::pair<InviteSessionHandle, int> findInviteSession(CallId replaces);

      std::vector<ClientSubscriptionHandle> findClientSubscriptions(DialogId id);
      std::vector<ClientSubscriptionHandle> findClientSubscriptions(DialogSetId id);
      std::vector<ClientSubscriptionHandle> findClientSubscriptions(const DialogId& id, const Data& eventType);
      std::vector<ClientSubscriptionHandle> findClientSubscriptions(const DialogSetId& id, const Data& eventType);
      std::vector<ClientSubscriptionHandle> findClientSubscriptions(DialogSetId id, const Data& eventType, const Data& subId);
      ServerSubscriptionHandle findServerSubscription(DialogId id);
      ClientPublicationHandle findClientPublication(DialogId id);
      ServerPublicationHandle findServerPublication(DialogId id);
      std::vector<ClientOutOfDialogReqHandle> findClientOutOfDialog(DialogId id);
      ServerOutOfDialogReqHandle findServerOutOfDialog(DialogId id);

      ClientPublicationHandler* getClientPublicationHandler(const Data& eventType);
      ServerPublicationHandler* getServerPublicationHandler(const Data& eventType);

      ClientSubscriptionHandler* getClientSubscriptionHandler(const Data& eventType);
      ServerSubscriptionHandler* getServerSubscriptionHandler(const Data& eventType);

      // will apply the specified functor(which takes a
      //ServerSubscriptionHandle) to each matching ServerSubscription.  
      //Returns the functor after the last application.
      template<typename UnaryFunction>
      UnaryFunction applyToServerSubscriptions(const Data& aor, 
                                               const Data& eventType, 
                                               UnaryFunction applyFn)
      {
         Data key = eventType + aor;
         std::pair<ServerSubscriptions::iterator,ServerSubscriptions::iterator> 
            range = mServerSubscriptions.equal_range(key);
         
         for (ServerSubscriptions::iterator i=range.first; i!=range.second; ++i)
         {
            ServerSubscriptionHandle h = i->second->getHandle();
            applyFn(h);
         }
         return applyFn;         
      }

      //DUM will delete features in its destructor. Feature manipulation should
      //be done before any processing starts.
      //ServerAuthManager is now a DumFeature; setServerAuthManager is a special
      //case of addFeature; the ServerAuthManager should always be the first
      //feature in the chain.
      void addIncomingFeature(resip::SharedPtr<DumFeature> feat);
      void addOutgoingFeature(resip::SharedPtr<DumFeature> feat);

      void setOutgoingMessageInterceptor(resip::SharedPtr<DumFeature> feat);

      TargetCommand::Target& dumIncomingTarget() 
      {
         return *mIncomingTarget;
      }

      TargetCommand::Target& dumOutgoingTarget()
      {
         return *mOutgoingTarget;
      }

      //exposed so DumThread variants can be written
      Message* getNext(int ms) { return mFifo.getNext(ms); }
      void internalProcess(std::auto_ptr<Message> msg);

      void applyToAllClientSubscriptions(ClientSubscriptionFunctor*);
      void applyToAllServerSubscriptions(ServerSubscriptionFunctor*);

      void registerForConnectionTermination(Postable*);
      void unRegisterForConnectionTermination(Postable*);

   protected:
      virtual void onAllHandlesDestroyed();      
      //TransactionUser virtuals
      virtual const Data& name() const;
      friend class DumThread;

      DumFeatureChain::FeatureList mIncomingFeatureList;
      DumFeatureChain::FeatureList mOutgoingFeatureList;
      
      SharedPtr<DumFeature> mOutgoingMessageInterceptor;

      typedef std::map<Data, DumFeatureChain*> FeatureChainMap;
      FeatureChainMap mIncomingFeatureChainMap;
      FeatureChainMap mOutgoingFeatureChainMap;
  
   private:     
      friend class Dialog;
      friend class DialogSet;

      friend class ClientInviteSession;
      friend class ClientOutOfDialogReq;
      friend class ClientPublication;
      friend class ClientRegistration;
      friend class ClientSubscription;
      friend class InviteSession;
      friend class ServerInviteSession;
      friend class ServerOutOfDialogReq;
      friend class ServerPublication;
      friend class ServerRegistration;
      friend class ServerSubscription;
      friend class BaseUsage;
      friend class ClientPagerMessage;
      friend class ServerPagerMessage;
      friend class KeepAliveAssociation;
      friend class NetworkAssociation;

      friend class MergedRequestRemovalCommand;

      class IncomingTarget : public TargetCommand::Target
      {
         public:
            IncomingTarget(DialogUsageManager& dum) : TargetCommand::Target(dum) 
            {
            }

            virtual void post(std::auto_ptr<Message> msg)
            {
               mDum.incomingProcess(msg);
            }
      };
      
      class OutgoingTarget : public TargetCommand::Target
      {
         public:
            OutgoingTarget(DialogUsageManager& dum) : TargetCommand::Target(dum) 
            {
            }

            virtual void post(std::auto_ptr<Message> msg)
            {
               mDum.outgoingProcess(msg);
            }
      };

      DialogSet* makeUacDialogSet(BaseCreator* creator, AppDialogSet* appDs);
      SharedPtr<SipMessage> makeNewSession(BaseCreator* creator, AppDialogSet* appDs);

      // makes a proto response to a request
      void makeResponse(SipMessage& response, 
                        const SipMessage& request, 
                        int responseCode, 
                        const Data& reason = Data::Empty) const;
      // May call a callback to let the app adorn
      void sendResponse(const SipMessage& response);

      void sendUsingOutboundIfAppropriate(UserProfile& userProfile, std::auto_ptr<SipMessage> msg);

      void addTimer(DumTimeout::Type type,
                    unsigned long durationSeconds,
                    BaseUsageHandle target, 
                    int seq, 
                    int altseq=-1);

      void addTimerMs(DumTimeout::Type type,
                      unsigned long duration,
                      BaseUsageHandle target, 
                      int seq, 
                      int altseq=-1);

      Dialog& findOrCreateDialog(const SipMessage* msg);
      Dialog* findDialog(const DialogId& id);
      DialogSet* findDialogSet(const DialogSetId& id);
      
      // return 0, if no matching BaseCreator
      BaseCreator* findCreator(const DialogId& id);

      void processRequest(const SipMessage& request);
      void processResponse(const SipMessage& response);
      bool validateRequestURI(const SipMessage& request);
      bool validateRequiredOptions(const SipMessage& request);
      bool validateContent(const SipMessage& request);
      bool validateAccept(const SipMessage& request);
      bool validateTo(const SipMessage& request);
      bool mergeRequest(const SipMessage& request);

      void processPublish(const SipMessage& publish);

      void removeDialogSet(const DialogSetId& );      

      bool checkEventPackage(const SipMessage& request);

      bool queueForIdentityCheck(SipMessage* msg);
      void processIdentityCheckResponse(const HttpGetMessage& msg);

      void incomingProcess(std::auto_ptr<Message> msg);
      void outgoingProcess(std::auto_ptr<Message> msg);

      // For delayed delete of a Usage
      void destroy(const BaseUsage* usage);
      void destroy(DialogSet*);
      void destroy(Dialog*);

      void requestMergedRequestRemoval(const MergedRequestKey&);
      void removeMergedRequest(const MergedRequestKey&);

      typedef std::set<MergedRequestKey> MergedRequests;
      MergedRequests mMergedRequests;
            
      typedef std::map<Data, DialogSet*> CancelMap;
      CancelMap mCancelMap;
      
      typedef HashMap<DialogSetId, DialogSet*> DialogSetMap;
      DialogSetMap mDialogSetMap;

      SharedPtr<MasterProfile> mMasterProfile;
      SharedPtr<UserProfile> mMasterUserProfile;
      std::auto_ptr<RedirectManager>   mRedirectManager;
      std::auto_ptr<ClientAuthManager> mClientAuthManager;
      //std::auto_ptr<ServerAuthManager> mServerAuthManager;  
    
      InviteSessionHandler* mInviteSessionHandler;
      ClientRegistrationHandler* mClientRegistrationHandler;
      ServerRegistrationHandler* mServerRegistrationHandler;      
      RedirectHandler* mRedirectHandler;
      DialogSetHandler* mDialogSetHandler;      

      RegistrationPersistenceManager *mRegistrationPersistenceManager;

	  OutOfDialogHandler* getOutOfDialogHandler(const MethodTypes type);

      std::map<Data, ClientSubscriptionHandler*> mClientSubscriptionHandlers;
      std::map<Data, ServerSubscriptionHandler*> mServerSubscriptionHandlers;
      std::map<Data, ClientPublicationHandler*> mClientPublicationHandlers;
      std::map<Data, ServerPublicationHandler*> mServerPublicationHandlers;
      std::map<MethodTypes, OutOfDialogHandler*> mOutOfDialogHandlers;
      std::auto_ptr<KeepAliveManager> mKeepAliveManager;
      bool mIsDefaultServerReferHandler;

      ClientPagerMessageHandler* mClientPagerMessageHandler;
      ServerPagerMessageHandler* mServerPagerMessageHandler;

      std::auto_ptr<AppDialogSetFactory> mAppDialogSetFactory;

      SipStack& mStack;
      DumShutdownHandler* mDumShutdownHandler;
      typedef enum 
      {
         Running,
         ShutdownRequested, // while ending usages
         RemovingTransactionUser, // while removing TU from stack
         Shutdown,  // after TU has been removed from stack
         Destroying // while calling destructor
      } ShutdownState;
      ShutdownState mShutdownState;

      // from ETag -> ServerPublication
      typedef std::map<Data, ServerPublication*> ServerPublications;
      ServerPublications mServerPublications;
      typedef std::map<Data, SipMessage*> RequiresCerts;
      RequiresCerts mRequiresCerts;      
      // from Event-Type+document-aor -> ServerSubscription
      // Managed by ServerSubscription
      typedef std::multimap<Data, ServerSubscription*> ServerSubscriptions;
      ServerSubscriptions mServerSubscriptions;

      IncomingTarget* mIncomingTarget;
      OutgoingTarget* mOutgoingTarget;

      EventDispatcher<ConnectionTerminated> mConnectionTerminatedEventDispatcher;
};

}

#endif

/* ====================================================================
 * The Vovida Software License, Version 1.0 
 * 
 * Copyright (c) 2000 Vovida Networks, Inc.  All rights reserved.
 * 
 * Redistribution and use in source and binary forms, with or without
 * modification, are permitted provided that the following conditions
 * are met:
 * 
 * 1. Redistributions of source code must retain the above copyright
 *    notice, this list of conditions and the following disclaimer.
 * 
 * 2. Redistributions in binary form must reproduce the above copyright
 *    notice, this list of conditions and the following disclaimer in
 *    the documentation and/or other materials provided with the
 *    distribution.
 * 
 * 3. The names "VOCAL", "Vovida Open Communication Application Library",
 *    and "Vovida Open Communication Application Library (VOCAL)" must
 *    not be used to endorse or promote products derived from this
 *    software without prior written permission. For written
 *    permission, please contact vocal@vovida.org.
 *
 * 4. Products derived from this software may not be called "VOCAL", nor
 *    may "VOCAL" appear in their name, without prior written
 *    permission of Vovida Networks, Inc.
 * 
 * THIS SOFTWARE IS PROVIDED "AS IS" AND ANY EXPRESSED OR IMPLIED
 * WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE IMPLIED WARRANTIES
 * OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE, TITLE AND
 * NON-INFRINGEMENT ARE DISCLAIMED.  IN NO EVENT SHALL VOVIDA
 * NETWORKS, INC. OR ITS CONTRIBUTORS BE LIABLE FOR ANY DIRECT DAMAGES
 * IN EXCESS OF $1,000, NOR FOR ANY INDIRECT, INCIDENTAL, SPECIAL,
 * EXEMPLARY, OR CONSEQUENTIAL DAMAGES (INCLUDING, BUT NOT LIMITED TO,
 * PROCUREMENT OF SUBSTITUTE GOODS OR SERVICES; LOSS OF USE, DATA, OR
 * PROFITS; OR BUSINESS INTERRUPTION) HOWEVER CAUSED AND ON ANY THEORY
 * OF LIABILITY, WHETHER IN CONTRACT, STRICT LIABILITY, OR TORT
 * (INCLUDING NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE
 * USE OF THIS SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF SUCH
 * DAMAGE.
 * 
 * ====================================================================
 * 
 * This software consists of voluntary contributions made by Vovida
 * Networks, Inc. and many individuals on behalf of Vovida Networks,
 * Inc.  For more information on Vovida Networks, Inc., please see
 * <http://www.vovida.org/>.
 *
 */
