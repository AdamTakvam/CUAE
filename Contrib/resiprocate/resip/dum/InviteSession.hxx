#if !defined(RESIP_INVITESESSION_HXX)
#define RESIP_INVITESESSION_HXX

#include "resip/stack/SipMessage.hxx"
#include "resip/stack/SdpContents.hxx"
#include "resip/dum/DialogUsage.hxx"
#include "resip/dum/DialogUsageManager.hxx"

#include <map>

namespace resip
{

class SdpContents;

/** Base class for class ClientInviteSession and class ServerInviteSession.
    Implements common attributes and behavior (i.e.t post connected) of the two
    classes.
*/
class InviteSession : public DialogUsage
{
   public:
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

      /** Called to request that the far end provide an offer.  This will cause a 
          reinvite with no sdp to be sent.  */
      virtual void requestOffer();

      enum EndReason
      {
         NotSpecified=0,
         UserHangup,
         AppRejectedSdp,
         IllegalNegotiation,
         AckNotReceived,
         SessionExpired,
         ENDREASON_MAX
      };

      /** Makes the specific dialog end. Will send a BYE (not a CANCEL) */
      virtual void end(EndReason reason);
      virtual void end();      

      /** Rejects an offer at the SIP level.  Can also be used to 
          send a 488 to a reINVITE or UPDATE */
      virtual void reject(int statusCode, WarningCategory *warning = 0);

      /** will resend the current sdp in an UPDATE or reINVITE */
      virtual void targetRefresh(const NameAddr& localUri);

      // Following methods are for sending requests within a dialog

      /** sends a refer request */
      virtual void refer(const NameAddr& referTo, bool referSub = true);

      /** sends a refer request with a replaces header */
      virtual void refer(const NameAddr& referTo, InviteSessionHandle sessionToReplace, bool referSub = true);

      /** sends an info request */
      virtual void info(const Contents& contents);

      /** sends a message request 

          @warning From RFC3428 - The authors recognize that there may be valid reasons to 
                                  send MESSAGE requests in the context of a dialog.  For 
                                  example, one participant in a voice session may wish to 
                                  send an IM to another participant, and associate that IM 
                                  with the session.  But implementations SHOULD NOT create 
                                  dialogs for the primary purpose of associating MESSAGE 
                                  requests with one another. 
      */
      virtual void message(const Contents& contents);

      /** accepts an INFO or MESSAGE request with a 2xx and an optional contents */
      virtual void acceptNIT(int statusCode = 200, const Contents * contents = 0);

      /** rejects an INFO or MESSAGE request with an error status code */
      virtual void rejectNIT(int statusCode = 488);

      virtual void acceptReferNoSub(int statusCode = 200);
      virtual void rejectReferNoSub(int responseCode);

      // Convenience methods for accessing attributes of a dialog. 
      const NameAddr& myAddr() const;
      const NameAddr& peerAddr() const;
      const SdpContents& getLocalSdp() const;
      const SdpContents& getRemoteSdp() const;
      const Data& getDialogId() const;
      
      bool isConnected() const;
      bool isTerminated() const;
      bool isEarly() const;     // UAC Early states
      bool isAccepted() const;  // UAS States before accept is called
      
      virtual std::ostream& dump(std::ostream& strm) const;
      InviteSessionHandle getSessionHandle();

      typedef enum
      {
         None, // means no Offer or Answer (may have SDP)
         Offer,
         Answer
      } OfferAnswerType;

   protected:

      typedef enum
      {
         Undefined,                 // Not used
         Connected,
         SentUpdate,                // Sent an UPDATE
         SentUpdateGlare,           // got a 491
         SentReinvite,              // Sent a reINVITE
         SentReinviteGlare,         // Got a 491
         SentReinviteNoOffer,       // Sent a reINVITE with no offer (requestOffer)
         SentReinviteAnswered,      // Sent a reINVITE no offer and received a 200-offer
         SentReinviteNoOfferGlare,  // Got a 491
         ReceivedUpdate,            // Received an UPDATE
         ReceivedReinvite,          // Received a reINVITE
         ReceivedReinviteNoOffer,   // Received a reINVITE with no offer
         ReceivedReinviteSentOffer, // Sent a 200 to a reINVITE with no offer
         Answered,
         WaitingToOffer,
         WaitingToRequestOffer,
         WaitingToTerminate,        // Waiting for 2xx response before sending BYE
         WaitingToHangup,           // Waiting for ACK before sending BYE
         Terminated,                // Ended. waiting to delete

         UAC_Start,
         UAC_Early,
         UAC_EarlyWithOffer,
         UAC_EarlyWithAnswer,
         UAC_Answered,
         UAC_SentUpdateEarly,
         UAC_SentUpdateConnected,
         UAC_ReceivedUpdateEarly,
         UAC_SentAnswer,
         UAC_QueuedUpdate,
         UAC_Cancelled,

         UAS_Start,
         UAS_Offer, 
         UAS_OfferProvidedAnswer,
         UAS_EarlyOffer,
         UAS_EarlyProvidedAnswer, 

         UAS_NoOffer, 
         UAS_ProvidedOffer, 
         UAS_EarlyNoOffer, 
         UAS_EarlyProvidedOffer, 
         UAS_Accepted, 
         UAS_WaitingToOffer, 

         UAS_AcceptedWaitingAnswer, 
         UAS_OfferReliable,
         UAS_NoOfferReliable,
         UAS_FirstSentOfferReliable,
         UAS_FirstEarlyReliable,
         UAS_EarlyReliable,
         UAS_SentUpdate,
         UAS_SentUpdateAccepted,
         UAS_ReceivedUpdate,
         UAS_ReceivedUpdateWaitingAnswer,
         UAS_WaitingToTerminate,
         UAS_WaitingToHangup
      } State;

      typedef enum
      {
         OnRedirect, // 3xx
         OnGeneralFailure, // 481 or 408
         OnInvite, // UAS
         OnInviteOffer,// UAS
         OnInviteReliableOffer, // UAS
         OnInviteReliable, // UAS
         OnCancel, // UAS
         OnBye, 
         On200Bye, 
         On1xx, // UAC
         On1xxEarly, // UAC
         On1xxOffer, // UAC
         On1xxAnswer, // UAC
         On2xx, 
         On2xxOffer,
         On2xxAnswer,
         On422Invite,
         On487Invite,
         On489Invite,
         On491Invite,
         OnInviteFailure,
         OnAck,
         OnAckAnswer,
         On200Cancel, // UAC
         OnCancelFailure, // UAC
         OnUpdate,
         OnUpdateOffer,
         OnUpdateRejected,
         On422Update,
         On491Update,
         On489Update,
         On200Update,
         OnPrack, // UAS
         On200Prack, // UAC
         Unknown
      } Event;

      typedef enum
      {
         NitComplete,
         NitProceeding
      } NitState;

      InviteSession(DialogUsageManager& dum, Dialog& dialog);
      virtual ~InviteSession();
      virtual void dialogDestroyed(const SipMessage& msg);
      virtual void onReadyToSend(SipMessage& msg);

      virtual void dispatch(const SipMessage& msg);
      virtual void dispatch(const DumTimeout& timer);

      // Utility methods (one for each State)
      void dispatchConnected(const SipMessage& msg);
      void dispatchSentUpdate(const SipMessage& msg);
      void dispatchSentReinvite(const SipMessage& msg);
      void dispatchSentReinviteNoOffer(const SipMessage& msg);
      void dispatchSentReinviteAnswered(const SipMessage& msg);
      void dispatchGlare(const SipMessage& msg);
      void dispatchReinviteNoOfferGlare(const SipMessage& msg);
      void dispatchReceivedUpdateOrReinvite(const SipMessage& msg);
      void dispatchReceivedReinviteSentOffer(const SipMessage& msg);
      void dispatchAnswered(const SipMessage& msg);
      void dispatchWaitingToOffer(const SipMessage& msg);
      void dispatchWaitingToRequestOffer(const SipMessage& msg);
      void dispatchWaitingToTerminate(const SipMessage& msg);
      void dispatchWaitingToHangup(const SipMessage& msg);
      void dispatchTerminated(const SipMessage& msg);

      void startRetransmit200Timer();
      void start491Timer();

      void setSessionTimerHeaders(SipMessage& msg);
      void sessionRefresh();
      void setSessionTimerPreferences();
      void startSessionTimer();
      void handleSessionTimerResponse(const SipMessage& msg);
      void handleSessionTimerRequest(SipMessage &response, const SipMessage& request);

      static Data toData(State state);
      void transition(State target);

      std::auto_ptr<SdpContents> getSdp(const SipMessage& msg);
      bool isReliable(const SipMessage& msg);
      static std::auto_ptr<SdpContents> makeSdp(const SdpContents& sdp);
      static std::auto_ptr<Contents> makeSdp(const SdpContents& sdp, const SdpContents* alternative);
      static void setSdp(SipMessage& msg, const SdpContents& sdp, const SdpContents* alternative = 0);
      static void setSdp(SipMessage& msg, const Contents* sdp);
      void provideProposedOffer();

      void storePeerCapabilities(const SipMessage& msg);
      bool updateMethodSupported() const;

      void sendAck(const SdpContents *sdp=0);
      void sendBye();

      DialogUsageManager::EncryptionLevel getEncryptionLevel(const SipMessage& msg);
      void setCurrentLocalSdp(const SipMessage& msg);
      void referNoSub(const SipMessage& msg);

      Tokens mPeerSupportedMethods;
      Tokens mPeerSupportedOptionTags;
      Mimes mPeerSupportedMimeTypes;
      Tokens mPeerSupportedEncodings;
      Tokens mPeerSupportedLanguages;
      Tokens mPeerAllowedEvents;

      Event toEvent(const SipMessage& msg, const SdpContents* sdp);
      
      State mState;
      NitState mNitState;

      std::auto_ptr<SdpContents> mCurrentLocalSdp;
      std::auto_ptr<Contents> mProposedLocalSdp;

      std::auto_ptr<SdpContents> mCurrentRemoteSdp;
      std::auto_ptr<SdpContents> mProposedRemoteSdp;

      SharedPtr<SipMessage> mLastLocalSessionModification; // last UPDATE or reINVITE sent
      SharedPtr<SipMessage> mLastRemoteSessionModification; // last UPDATE or reINVITE received
      SharedPtr<SipMessage> mInvite200;               // 200 OK for reINVITE for retransmissions
      SharedPtr<SipMessage> mLastNitResponse;         // 
                                                      //?dcm? -- ptr, delete when not needed?

      SipMessage  mLastReferNoSubRequest;
      
      unsigned long mCurrentRetransmit200;

      // Session Timer settings
      int  mSessionInterval;
      int  mMinSE;
      bool mSessionRefresher;
      int  mSessionTimerSeq;
      bool mSessionRefreshReInvite;      

      bool mSentRefer;
      bool mReferSub;

      DialogUsageManager::EncryptionLevel mCurrentEncryptionLevel;
      DialogUsageManager::EncryptionLevel mProposedEncryptionLevel; // UPDATE or RE-INVITE

      EndReason mEndReason;   

      // Used to respond to 2xx retransmissions.
      typedef std::map<int, SharedPtr<SipMessage> > AckMap;
      AckMap mAcks;
      
   private:
      friend class Dialog;
      friend class DialogUsageManager;

      // disabled
      InviteSession(const InviteSession&);
      InviteSession& operator=(const InviteSession&);

      // Utility methods for handling particular methods
      void dispatchOthers(const SipMessage& msg);
      void dispatchUnhandledInvite(const SipMessage& msg);
      void dispatchPrack(const SipMessage& msg);
      void dispatchCancel(const SipMessage& msg);
      void dispatchBye(const SipMessage& msg);
      void dispatchInfo(const SipMessage& msg);
      void dispatchMessage(const SipMessage& msg);

 
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

