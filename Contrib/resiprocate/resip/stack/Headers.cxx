#if defined(HAVE_CONFIG_H)
#include "resip/stack/config.hxx"
#endif

#include "rutil/Data.hxx"
#include "resip/stack/Headers.hxx"
#include "resip/stack/Symbols.hxx"
#include "resip/stack/SipMessage.hxx"

// GPERF generated external routines
#include "resip/stack/HeaderHash.hxx"

#include <iostream>
using namespace std;

//int strcasecmp(const char*, const char*);
//int strncasecmp(const char*, const char*, int len);

using namespace resip;

Data Headers::HeaderNames[MAX_HEADERS+1];
bool Headers::CommaTokenizing[] = {false};
bool Headers::CommaEncoding[] = {false};

bool 
Headers::isCommaTokenizing(Type type)
{
   return CommaTokenizing[type+1];
}

bool 
Headers::isCommaEncoding(Type type)
{
   return CommaEncoding[type+1];
}

const Data&
Headers::getHeaderName(int type)
{
   return HeaderNames[type+1];
}

#define defineHeader(_enum, _name, _type, _reference)                                                                   \
Headers::Type                                                                                                           \
H_##_enum::getTypeNum() const {return Headers::_enum;}                                                                  \
                                                                                                                        \
void H_##_enum::merge(SipMessage& target, const SipMessage& embedded)                                                   \
{                                                                                                                       \
   if (embedded.exists(*this))                                                                                          \
   {                                                                                                                    \
      target.header(*this) = embedded.header(*this);                                                                    \
   }                                                                                                                    \
}                                                                                                                       \
                                                                                                                        \
H_##_enum::H_##_enum()                                                                                                  \
{                                                                                                                       \
   Headers::CommaTokenizing[Headers::_enum+1] = bool(Type::commaHandling & ParserCategory::CommasAllowedOutputMulti);   \
   Headers::CommaEncoding[Headers::_enum+1] = bool(Type::commaHandling & 2);                                            \
   Headers::HeaderNames[Headers::_enum+1] = _name;                                                                      \
}                                                                                                                       \
                                                                                                                        \
_type&                                                                                                                  \
H_##_enum::knownReturn(ParserContainerBase* container)                                                                  \
{                                                                                                                       \
   return dynamic_cast<ParserContainer<_type>*>(container)->front();                                                    \
}                                                                                                                       \
                                                                                                                        \
H_##_enum resip::h_##_enum

#define defineMultiHeader(_enum, _name, _type, _reference)                                                                              \
   Headers::Type                                                                                                                        \
H_##_enum##s::getTypeNum() const {return Headers::_enum;}                                                                               \
                                                                                                                                        \
void H_##_enum##s::merge(SipMessage& target, const SipMessage& embedded)                                                                \
{                                                                                                                                       \
   if (embedded.exists(*this))                                                                                                          \
   {                                                                                                                                    \
      target.header(*this).append(embedded.header(*this));                                                                              \
   }                                                                                                                                    \
}                                                                                                                                       \
                                                                                                                                        \
H_##_enum##s::H_##_enum##s()                                                                                                            \
{                                                                                                                                       \
   Headers::CommaTokenizing[Headers::_enum+1] = bool(Type::value_type::commaHandling & ParserCategory::CommasAllowedOutputMulti);       \
   Headers::CommaEncoding[Headers::_enum+1] = bool(Type::value_type::commaHandling & 2);                                                \
   Headers::HeaderNames[Headers::_enum+1] = _name;                                                                                      \
}                                                                                                                                       \
                                                                                                                                        \
ParserContainer<_type>&                                                                                                                 \
H_##_enum##s::knownReturn(ParserContainerBase* container)                                                                               \
{                                                                                                                                       \
   return *dynamic_cast<ParserContainer<_type>*>(container);                                                                            \
}                                                                                                                                       \
                                                                                                                                        \
H_##_enum##s resip::h_##_enum##s

//====================
// Token
//====================

defineHeader(ContentDisposition, "Content-Disposition", Token, "RFC 3261");
defineHeader(ContentEncoding, "Content-Encoding", Token, "RFC 3261");
defineHeader(ContentTransferEncoding, "Content-Transfer-Encoding", StringCategory, "RFC ?"); // !dlb! defineMultiHeader
defineHeader(MIMEVersion, "MIME-Version", Token, "RFC 3261");
defineHeader(Priority, "Priority", Token, "RFC 3261");
defineHeader(Event, "Event", Token, "RFC 3265");
defineHeader(SubscriptionState, "Subscription-State", Token, "RFC 3265");

defineHeader(SIPETag, "SIP-ETag", Token, "RFC 3903");
defineHeader(SIPIfMatch, "SIP-If-Match", Token, "RFC 3903");
defineHeader(ContentId, "Content-ID", Token, "RFC 2045");

defineHeader(Identity, "Identity", StringCategory, "draft-sip-identity-03");

defineMultiHeader(AllowEvents, "Allow-Events", Token, "RFC 3265");
// explicitly declare to avoid h_AllowEventss, ugh
H_AllowEventss resip::h_AllowEvents;

defineMultiHeader(AcceptEncoding, "Accept-Encoding", Token, "RFC 3261");
defineMultiHeader(AcceptLanguage, "Accept-Language", Token, "RFC 3261");
defineMultiHeader(Allow, "Allow", Token, "RFC 3261");
defineMultiHeader(ContentLanguage, "Content-Language", Token, "RFC 3261");
defineMultiHeader(ProxyRequire, "Proxy-Require", Token, "RFC 3261");
defineMultiHeader(Require, "Require", Token, "RFC 3261");
defineMultiHeader(Supported, "Supported", Token, "RFC 3261");
defineMultiHeader(Unsupported, "Unsupported", Token, "RFC 3261");
defineMultiHeader(SecurityClient, "Security-Client", Token, "RFC 3329");
defineMultiHeader(SecurityServer, "Security-Server", Token, "RFC 3329");
defineMultiHeader(SecurityVerify, "Security-Verify", Token, "RFC 3329");
// explicitly declare to avoid h_SecurityVerifys, ugh
H_SecurityVerifys resip::h_SecurityVerifies;

defineMultiHeader(RequestDisposition, "Request-Disposition", Token, "RFC 3841");
defineMultiHeader(Reason, "Reason", Token, "RFC 3326");
defineMultiHeader(Privacy, "Privacy", Token, "RFC 3323");
// explicitly declare to avoid h_Privacys
H_Privacys resip::h_Privacies;

defineMultiHeader(PMediaAuthorization, "P-Media-Authorization", Token, "RFC 3313");
defineHeader(ReferSub, "Refer-Sub", Token, "draft-ietf-sip-refer-with-norefersub-03");

//====================
// Mime
//====================
typedef ParserContainer<Mime> Mimes;

defineMultiHeader(Accept, "Accept", Mime, "RFC 3261");
defineHeader(ContentType, "Content-Type", Mime, "RFC 3261");

//====================
// GenericUris:
//====================
defineHeader(IdentityInfo, "Identity-Info", GenericUri, "draft-sip-identity-03");

typedef ParserContainer<GenericUri> GenericUris;
defineMultiHeader(CallInfo, "Call-Info", GenericUri, "RFC 3261");
defineMultiHeader(AlertInfo, "Alert-Info", GenericUri, "RFC 3261");
defineMultiHeader(ErrorInfo, "Error-Info", GenericUri, "RFC 3261");

//====================
// NameAddr:
//====================
typedef ParserContainer<NameAddr> NameAddrs;

defineMultiHeader(RecordRoute, "Record-Route", NameAddr, "RFC 3261");
defineMultiHeader(Route, "Route", NameAddr, "RFC 3261");
defineMultiHeader(Contact, "Contact", NameAddr, "RFC 3261");
defineHeader(From, "From", NameAddr, "RFC 3261");
defineHeader(To, "To", NameAddr, "RFC 3261"); 
defineHeader(ReplyTo, "Reply-To", NameAddr, "RFC 3261");
defineHeader(ReferTo, "Refer-To", NameAddr, "RFC 3515");
defineHeader(ReferredBy, "Referred-By", NameAddr, "RFC 3892");

defineMultiHeader(Path, "Path", NameAddr, "RFC 3327");
defineMultiHeader(AcceptContact, "Accept-Contact", NameAddr, "RFC 3841");
defineMultiHeader(RejectContact, "Reject-Contact", NameAddr, "RFC 3841");
defineMultiHeader(PPreferredIdentity, "P-Preferred-Identity", NameAddr, "RFC 3325");
// explicitly declare to avoid h_PAssertedIdentitys
H_PPreferredIdentitys resip::h_PPreferredIdentities;

defineMultiHeader(PAssertedIdentity, "P-Asserted-Identity", NameAddr, "RFC 3325");
// explicitly declare to avoid h_PAssertedIdentitys
H_PAssertedIdentitys resip::h_PAssertedIdentities;


defineHeader(PCalledPartyId, "P-Called-Party-ID", NameAddr, "RFC 3455");
defineMultiHeader(PAssociatedUri, "P-Associated-URI", NameAddr, "RFC 3455");
defineMultiHeader(ServiceRoute, "Service-Route", NameAddr, "RFC 3608");

//====================
// String:
//====================
typedef ParserContainer<StringCategory> StringCategories;

defineHeader(Organization, "Organization", StringCategory, "RFC 3261");
defineHeader(Server, "Server", StringCategory, "RFC 3261");
defineHeader(Subject, "Subject", StringCategory, "RFC 3261");
defineHeader(UserAgent, "User-Agent", StringCategory, "RFC 3261");
defineHeader(Timestamp, "Timestamp", StringCategory, "RFC 3261");

//====================
// Integer:
//====================
typedef ParserContainer<IntegerCategory> IntegerCategories;

// !dlb! not clear this needs to be exposed
defineHeader(ContentLength, "Content-Length", IntegerCategory, "RFC 3261");
defineHeader(MaxForwards, "Max-Forwards", IntegerCategory, "RFC 3261");
defineHeader(MinExpires, "Min-Expires", IntegerCategory, "RFC 3261");
defineHeader(RSeq, "RSeq", IntegerCategory, "RFC 3262");

// !dlb! this one is not quite right -- can have (comment) after field value
// !rwm!  WHO CARES!!!! Comments are evil
defineHeader(RetryAfter, "Retry-After", IntegerCategory, "RFC 3261");
defineHeader(Expires, "Expires", ExpiresCategory, "RFC 3261");
defineHeader(SessionExpires, "Session-Expires", ExpiresCategory, "RFC 4028");
defineHeader(MinSE, "Min-SE", ExpiresCategory, "RFC 4028");

//====================
// CallId:
//====================
defineHeader(CallID, "Call-ID", CallId, "RFC 3261");
H_CallId resip::h_CallId; // code convention compatible
defineHeader(Replaces, "Replaces", CallId, "RFC 3261");
defineHeader(InReplyTo, "In-Reply-To", CallId, "RFC 3261");
defineHeader(Join, "Join", CallId, "RFC 3911");
defineHeader(TargetDialog, "Target-Dialog", CallId, "Target Dialog draft");


//====================
// Auth:
//====================
defineHeader(AuthenticationInfo, "Authentication-Info", Auth, "RFC 3261");
defineMultiHeader(Authorization, "Authorization", Auth, "RFC 3261");
defineMultiHeader(ProxyAuthenticate, "Proxy-Authenticate", Auth, "RFC 3261");
defineMultiHeader(ProxyAuthorization, "Proxy-Authorization", Auth, "RFC 3261");
defineMultiHeader(WWWAuthenticate, "WWW-Authenticate", Auth, "RFC 3261");

//====================
// CSeqCategory:
//====================
defineHeader(CSeq, "CSeq", CSeqCategory, "RFC 3261");

//====================
// DateCategory:
//====================
defineHeader(Date, "Date", DateCategory, "RFC 3261");

//====================
// WarningCategory:
//====================
defineMultiHeader(Warning, "Warning", WarningCategory, "RFC 3261");

//====================
// RAckCategory
//====================
defineHeader(RAck, "RAck", RAckCategory, "RFC 3262");

defineMultiHeader(Via, "Via", Via, "RFC 3261");

RequestLineType resip::h_RequestLine;
StatusLineType resip::h_StatusLine;

Headers::Type
Headers::getType(const char* name, int len)
{
   struct headers* p;
   p = HeaderHash::in_word_set(name, len);
   return p ? Headers::Type(p->type) : Headers::UNKNOWN;
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
