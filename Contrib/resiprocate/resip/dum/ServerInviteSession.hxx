#if !defined(RESIP_SERVERINVITESESSION_HXX)
#define RESIP_SERVERINVITESESSION_HXX

#include "resip/dum/InviteSession.hxx"
#include "resip/stack/SipMessage.hxx"

#include <deque>

namespace resip
{

class ServerInviteSession: public InviteSession
{
   public:
      typedef Handle<ServerInviteSession> ServerInviteSessionHandle;
      ServerInviteSessionHandle getHandle();

      /** send a 3xx */
      void redirect(const NameAddrs& contacts, int code=302);

      /** send a 1xx - provisional response */
      void provisional(int code=180);
      
      /** Called to set the offer that will be used in the next message that
          sends an offer. If possible, this will synchronously send the
          appropriate request or response. In some cases, the UAS might have to
          call accept in order to cause the message to be sent. */
      virtual void provideOffer(const SdpContents& offer);
      virtual void provideOffer(const SdpContents& offer, DialogUsageManager::EncryptionLevel level, const SdpContents* alternative);

      /** Similar to provideOffer - called to set the answer to be signalled to
          the peer. May result in message being sent synchronously depending on
          the state. */
      virtual void provideAnswer(const SdpContents& answer);

      /** Makes the specific dialog end. Will send a BYE (not a CANCEL) */
      virtual void end(EndReason reason);
      virtual void end();

      /** Rejects an offer at the SIP level. So this can send a 488 to a
          reINVITE or UPDATE */
      virtual void reject(int statusCode, WarningCategory *warning = 0);

      /** accept an initial invite (2xx) - this is only applicable to the UAS */
      virtual void accept(int statusCode=200);
            
   private:
      friend class Dialog;

      virtual void dispatch(const SipMessage& msg);
      virtual void dispatch(const DumTimeout& timer);

      void dispatchStart(const SipMessage& msg);
      void dispatchOfferOrEarly(const SipMessage& msg);
      void dispatchAccepted(const SipMessage& msg);
      void dispatchWaitingToOffer(const SipMessage& msg);
      void dispatchAcceptedWaitingAnswer(const SipMessage& msg);
      void dispatchOfferReliable(const SipMessage& msg);
      void dispatchNoOfferReliable(const SipMessage& msg);
      void dispatchFirstSentOfferReliable(const SipMessage& msg);
      void dispatchFirstEarlyReliable(const SipMessage& msg);
      void dispatchEarlyReliable(const SipMessage& msg);
      void dispatchSentUpdate(const SipMessage& msg);
      void dispatchSentUpdateAccepted(const SipMessage& msg);
      void dispatchReceivedUpdate(const SipMessage& msg);
      void dispatchReceivedUpdateWaitingAnswer(const SipMessage& msg);
      void dispatchWaitingToTerminate(const SipMessage& msg);
      void dispatchWaitingToHangup(const SipMessage& msg);

      void dispatchCancel(const SipMessage& msg);
      void dispatchBye(const SipMessage& msg);
      void dispatchUnknown(const SipMessage& msg);

      // utilities
      void startRetransmit1xxTimer();
      void sendAccept(int code, Contents* sdp); // sends 2xxI
      void sendProvisional(int code);
      void sendUpdate(const SdpContents& sdp);

      ServerInviteSession(DialogUsageManager& dum, Dialog& dialog, const SipMessage& msg);

      // disabled
      ServerInviteSession(const ServerInviteSession&);
      ServerInviteSession& operator=(const ServerInviteSession&);

      // stores the original request
      const SipMessage mFirstRequest;
      SharedPtr<SipMessage> m1xx; // for 1xx retransmissions
      unsigned long mCurrentRetransmit1xx;
      
      //std::deque<SipMessage> mUnacknowledgedProvisionals; // all of them
      //SipMessage m200; // for retransmission
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
