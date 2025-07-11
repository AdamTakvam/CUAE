#include <cassert>

#include "resip/dum/DumFeature.hxx"
#include "resip/dum/DumFeatureChain.hxx"
#include "resip/dum/ServerAuthManager.hxx"
#include "resip/dum/DialogUsageManager.hxx"
#include "resip/dum/TargetCommand.hxx"
#include "rutil/Logger.hxx"
#include "resip/dum/UserAuthInfo.hxx"
#include "resip/stack/Helper.hxx"
#include "rutil/WinLeakCheck.hxx"

#define RESIPROCATE_SUBSYSTEM Subsystem::DUM

using namespace resip;
using namespace std;

ServerAuthManager::ServerAuthManager(DialogUsageManager& dum, TargetCommand::Target& target) :
   DumFeature(dum, target)
{
}


ServerAuthManager::~ServerAuthManager()
{
}

DumFeature::ProcessingResult 
ServerAuthManager::process(Message* msg)
{
   SipMessage* sipMsg = dynamic_cast<SipMessage*>(msg);

   if (sipMsg)
   {
      //!dcm! -- unecessary happens in handle
      switch ( handle(sipMsg) )
      {
         case ServerAuthManager::Challenged:
            InfoLog(<< "ServerAuth challenged request " << sipMsg->brief());
            return DumFeature::ChainDoneAndEventDone;            
         case ServerAuthManager::RequestedCredentials:
            InfoLog(<< "ServerAuth requested credentials " << sipMsg->brief());
            return DumFeature::EventTaken;
         case ServerAuthManager::Rejected:
            InfoLog(<< "ServerAuth rejected request " << sipMsg->brief());
            return DumFeature::ChainDoneAndEventDone;            
         default:
            return DumFeature::FeatureDone;            
            //break;
      }
   }

   UserAuthInfo* userAuth = dynamic_cast<UserAuthInfo*>(msg);
   if (userAuth)
   {
      //InfoLog(<< "Got UserAuthInfo");
      UserAuthInfo* userAuth = dynamic_cast<UserAuthInfo*>(msg);
      if (userAuth)
      {
         Message* result = handleUserAuthInfo(userAuth);
         if (result)
         {
            postCommand(auto_ptr<Message>(result));
            return FeatureDoneAndEventDone;
         }
         else
         {
            InfoLog(<< "ServerAuth rejected request " << *userAuth);
            return ChainDoneAndEventDone;            
         }
      }
   }
   return FeatureDone;   
}

SipMessage*
ServerAuthManager::handleUserAuthInfo(UserAuthInfo* userAuth)
{
   assert(userAuth);

   MessageMap::iterator it = mMessages.find(userAuth->getTransactionId());
   assert(it != mMessages.end());
   SipMessage* requestWithAuth = it->second;
   mMessages.erase(it);

   InfoLog( << "Checking for auth result in realm=" << userAuth->getRealm() 
            << " A1=" << userAuth->getA1());
         
   if (userAuth->getA1().empty())
   {
      InfoLog (<< "Account does not exist " << userAuth->getUser() << " in " << userAuth->getRealm());
      SharedPtr<SipMessage> response(new SipMessage);
      Helper::makeResponse(*response, *requestWithAuth, 404, "Account does not exist.");
      mDum.send(response);
      delete requestWithAuth;
      return 0;
   }
   else
   {
      //!dcm! -- need to handle stale/unit test advancedAuthenticateRequest
      //!dcm! -- delta? deal with.
      std::pair<Helper::AuthResult,Data> resPair = 
         Helper::advancedAuthenticateRequest(*requestWithAuth, 
                                             userAuth->getRealm(),
                                             userAuth->getA1(),
                                             3000);

      SharedPtr<SipMessage> response(new SipMessage);
      //SipMessage* challenge;

      switch (resPair.first) 
      {
         case Helper::Authenticated :
            if (authorizedForThisIdentity(userAuth->getUser(), userAuth->getRealm(), 
                                          requestWithAuth->header(h_From).uri()))
            {
               InfoLog (<< "Authorized request for " << userAuth->getRealm());
               return requestWithAuth;
            }
            else
            {
               // !rwm! The user is trying to forge a request.  Respond with a 403
               InfoLog (<< "User: " << userAuth->getUser() << " at realm: " << userAuth->getRealm() << 
                        " trying to forge request from: " << requestWithAuth->header(h_From).uri());

               Helper::makeResponse(*response, *requestWithAuth, 403, "Invalid user name provided");
               mDum.send(response);
               delete requestWithAuth;
               return 0;
            }
            break;
         case Helper::Failed :
            InfoLog (<< "Invalid password provided " << userAuth->getUser() << " in " << userAuth->getRealm());
            InfoLog (<< "  a1 hash of password from db was " << userAuth->getA1() );

            Helper::makeResponse(*response, *requestWithAuth, 403, "Invalid password provided");
            mDum.send(response);
            delete requestWithAuth;
            return 0;
            break;
         case Helper::BadlyFormed :
            InfoLog (<< "Authentication nonce badly formed for " << userAuth->getUser());

            Helper::makeResponse(*response, *requestWithAuth, 403, "Invalid nonce");
            mDum.send(response);
            delete requestWithAuth;
            return 0;
            break;
         case Helper::Expired :
         {
            InfoLog (<< "Nonce expired for " << userAuth->getUser());
            
            SharedPtr<SipMessage> challenge(Helper::makeProxyChallenge(*requestWithAuth, 
                                                                       requestWithAuth->header(h_RequestLine).uri().host(),
                                                                       useAuthInt(),
                                                                       true));

            InfoLog (<< "Sending challenge to " << requestWithAuth->brief());
            mDum.send(challenge);
            //delete challenge;
            delete requestWithAuth;
            return 0;
            break;
         }
         default :
            break;
      }
   }
   return 0;
}

            
bool
ServerAuthManager::useAuthInt() const
{
   return false;
}


bool 
ServerAuthManager::requiresChallenge(const SipMessage& msg)
{
   return true;  
}


bool
ServerAuthManager::authorizedForThisIdentity(const resip::Data &user, 
                                               const resip::Data &realm, 
                                                resip::Uri &fromUri)
{
   // !rwm! good enough for now.  TODO eventually consult a database to see what
   // combinations of user/realm combos are authorized for an identity
   return ((fromUri.user() == user) && (fromUri.host() == realm));
}


const Data& 
ServerAuthManager::getChallengeRealm(const SipMessage& msg)
{
   return msg.header(h_RequestLine).uri().host();
}


bool
ServerAuthManager::isMyRealm(const Data& realm)
{
   return mDum.isMyDomain(realm);
}


// return true if request has been consumed 
ServerAuthManager::Result
ServerAuthManager::handle(SipMessage* sipMsg)
{
   // Is challenge required for this message
   if(!requiresChallenge(*sipMsg))
   {
      return Skipped;
   }

   //InfoLog( << "trying to do auth" );
   if (sipMsg->isRequest() && 
       sipMsg->header(h_RequestLine).method() != ACK && 
       sipMsg->header(h_RequestLine).method() != CANCEL)  // Do not challenge ACKs or CANCELs
   {
      if (!sipMsg->exists(h_ProxyAuthorizations))
      {
         //assume TransactionUser has matched/repaired a realm
         SharedPtr<SipMessage> challenge(Helper::makeProxyChallenge(*sipMsg, 
                                                                    getChallengeRealm(*sipMsg),
                                                                    useAuthInt(),
                                                                    false /*stale*/));
         InfoLog (<< "Sending challenge to " << sipMsg->brief());
         mDum.send(challenge);
         //delete challenge;
         return Challenged;
      }
 
      try
      {
         for(Auths::iterator it = sipMsg->header(h_ProxyAuthorizations).begin();
             it  != sipMsg->header(h_ProxyAuthorizations).end(); it++)
         {
            if (isMyRealm(it->param(p_realm)))
            {
               InfoLog (<< "Requesting credential for " 
                        << it->param(p_username) << " @ " << it->param(p_realm));
               
               requestCredential(it->param(p_username),
                                 it->param(p_realm), 
                                 sipMsg->getTransactionId());
               mMessages[sipMsg->getTransactionId()] = sipMsg;
               return RequestedCredentials;
            }
         }

         InfoLog (<< "Didn't find matching realm ");
         SharedPtr<SipMessage> response(new SipMessage);
         Helper::makeResponse(*response, *sipMsg, 404, "Account does not exist");
         mDum.send(response);
         return Rejected;
      }
      catch(BaseException& e)
      {
         InfoLog (<< "Invalid auth header provided " << e);
         SharedPtr<SipMessage> response(new SipMessage);
         Helper::makeResponse(*response, *sipMsg, 400, "Invalid auth header");
         mDum.send(response);
         return Rejected;
      }
   }
   return Skipped;
}






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
