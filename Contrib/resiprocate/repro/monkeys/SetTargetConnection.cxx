#if defined(HAVE_CONFIG_H)
#include "resip/stack/config.hxx"
#endif

#include "resip/stack/SipMessage.hxx"
#include "resip/stack/ExtensionParameter.hxx"
#include "repro/monkeys/SetTargetConnection.hxx"
#include "repro/RequestContext.hxx"
#include "repro/Proxy.hxx"
#include <ostream>

#include "rutil/Logger.hxx"
#define RESIPROCATE_SUBSYSTEM resip::Subsystem::REPRO

using namespace resip;
using namespace repro;
using namespace std;

SetTargetConnection::SetTargetConnection()
{}

SetTargetConnection::~SetTargetConnection()
{}

Processor::processor_action_t
SetTargetConnection::process(RequestContext& context)
{
   DebugLog(<< "Monkey handling request: " << *this 
            << "; reqcontext = " << context);

   // !jf! this is to get tcp connections to go over the correct connection id

   SipMessage& request = context.getOriginalRequest();
   const Uri& route = context.getTopRoute().uri();
   
   static ExtensionParameter p_cid("cid");
   static ExtensionParameter p_cid1("cid1");
   static ExtensionParameter p_cid2("cid2");
   
   ConnectionId cid1 = route.exists(p_cid1) ? route.param(p_cid1).convertUnsignedLong() : 0;
   ConnectionId cid2 = route.exists(p_cid2) ? route.param(p_cid2).convertUnsignedLong() : 0;
   if (request.getSource().connectionId != 0 && 
       request.getSource().connectionId == cid1)
   {
      context.setTargetConnection(cid2);
   }
   else if (request.getSource().connectionId != 0 && 
            request.getSource().connectionId == cid2)
   {
      context.setTargetConnection(cid1);
   }
   return Processor::Continue;
}
   

void
SetTargetConnection::dump(std::ostream &os) const
{
  os << "Set Target Connection Monkey" << std::endl;
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
