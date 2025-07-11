#if defined(HAVE_CONFIG_H)
#include "resip/stack/config.hxx"
#endif

#include "resip/stack/Pkcs8Contents.hxx"
#include "resip/stack/SipMessage.hxx"
#include "rutil/Logger.hxx"
#include "rutil/ParseBuffer.hxx"
#include "rutil/WinLeakCheck.hxx"

using namespace resip;
using namespace std;

#define RESIPROCATE_SUBSYSTEM Subsystem::SIP

const Pkcs8Contents Pkcs8Contents::Empty;

static bool invokePkcs8ContentsInit = Pkcs8Contents::init();

bool
Pkcs8Contents::init()
{
   static ContentsFactory<Pkcs8Contents> factory;
   (void)factory;
   return true;
}

Pkcs8Contents::Pkcs8Contents()
   : Contents(getStaticType()),
     mText()
{
}

Pkcs8Contents::Pkcs8Contents(const Data& txt)
   : Contents(getStaticType()),
     mText(txt)
{
}

Pkcs8Contents::Pkcs8Contents(const Data& txt, const Mime& contentsType)
   : Contents(contentsType),
     mText(txt)
{
}
 
Pkcs8Contents::Pkcs8Contents(HeaderFieldValue* hfv, const Mime& contentsType)
   : Contents(hfv, contentsType),
     mText()
{
}
 
Pkcs8Contents::Pkcs8Contents(const Pkcs8Contents& rhs)
   : Contents(rhs),
     mText(rhs.mText)
{
}

Pkcs8Contents::~Pkcs8Contents()
{
}

Pkcs8Contents&
Pkcs8Contents::operator=(const Pkcs8Contents& rhs)
{
   if (this != &rhs)
   {
      Contents::operator=(rhs);
      mText = rhs.mText;
   }
   return *this;
}

Contents* 
Pkcs8Contents::clone() const
{
   return new Pkcs8Contents(*this);
}

const Mime& 
Pkcs8Contents::getStaticType() 
{
   static Mime type("application", "pkcs8");
   return type;
}

std::ostream& 
Pkcs8Contents::encodeParsed(std::ostream& str) const
{
   //DebugLog(<< "Pkcs8Contents::encodeParsed " << mText);
   str << mText;
   return str;
}


void 
Pkcs8Contents::parse(ParseBuffer& pb)
{
   const char* anchor = pb.position();
   pb.skipToEnd();
   pb.data(mText, anchor);
}

Data 
Pkcs8Contents::getBodyData() const
{
   checkParsed();
   return mText;
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
