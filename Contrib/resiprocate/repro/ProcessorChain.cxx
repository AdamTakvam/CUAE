#if defined(HAVE_CONFIG_H)
#include "resip/stack/config.hxx"
#endif

#include <iostream>

#include "repro/ProcessorChain.hxx"
#include "resip/stack/SipMessage.hxx"
#include "repro/ChainTraverser.hxx"
#include "repro/RequestContext.hxx"

#include "rutil/Logger.hxx"
#include "rutil/Inserter.hxx"

#define RESIPROCATE_SUBSYSTEM resip::Subsystem::REPRO

using namespace resip;
using namespace repro;
using namespace std;


repro::ProcessorChain::ProcessorChain(Processor::ChainType type)
{
   DebugLog(<< "Instantiating new monkey chain " << this );
   mType=type;
}

repro::ProcessorChain::~ProcessorChain()
{
   //DebugLog (<< "Deleting Chain: " << this);
   for (Chain::iterator i = mChain.begin(); i != mChain.end(); ++i)
   {
      //DebugLog (<< "Deleting RP: " << *i << " : " << **i);
      delete *i;
   }
   mChain.clear();
}

void
repro::ProcessorChain::addProcessor(auto_ptr<Processor> rp)
{
   DebugLog(<< "Adding new monkey to chain: " << *(rp.get()));
   rp->pushAddress(mChain.size());
   rp->pushAddress(mAddress);
   rp->setChainType(mType);
   mChain.push_back(rp.release());
}

repro::Processor::processor_action_t
repro::ProcessorChain::process(RequestContext &rc)
{
   //DebugLog(<< "Monkey handling request: " << *this << "; reqcontext = " << rc);

   processor_action_t action;
   unsigned int position=0;

   resip::Message* msg = rc.getCurrentEvent();

   if(!msg)
   {
      return SkipAllChains;
   }

   repro::ChainTraverser* proc = dynamic_cast<ChainTraverser*>(msg);
   
   if(proc)
   {
      position=proc->popAddr();      
   }
   
   
   for (; (position >=0 && position < mChain.size()); ++position)
   {
      DebugLog(<< "Chain invoking monkey: " << *(mChain[position]));

      action = mChain[position]->process(rc);

      if (action == SkipAllChains)
      {
         DebugLog(<< "Monkey aborted all chains: " << *(mChain[position]));
         return SkipAllChains;
      }

      if (action == WaitingForEvent)
      {
         DebugLog(<< "Monkey waiting for async response: " << *(mChain[position]));
         return WaitingForEvent;
      }

      if (action == SkipThisChain)
      {
         DebugLog(<< "Monkey skipping current chain: " << *(mChain[position]));
         return Continue;
      }

   }
   //DebugLog(<< "Monkey done processing: " << *(mChain[position]));
   return Continue;
}

void
ProcessorChain::pushAddress(const std::vector<short>& address)
{
   Processor::pushAddress(address);
   for(std::vector<Processor*>::iterator i=mChain.begin();i!=mChain.end();++i)
   {
      (**i).pushAddress(address);
   }
}

void
ProcessorChain::pushAddress(const short address)
{
   Processor::pushAddress(address);
   for(std::vector<Processor*>::iterator i=mChain.begin();i!=mChain.end();++i)
   {
      (**i).pushAddress(address);
   }   
}


void
ProcessorChain::setChainType(ChainType type)
{
   mType=type;
   std::vector<Processor*>::iterator i;
   for(i=mChain.begin();i!=mChain.end();++i)
   {
      (*i)->setChainType(type);
   }
}

void
ProcessorChain::dump(std::ostream &os) const
{
   os << "Monkey Chain!" << Inserter(mChain);
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
