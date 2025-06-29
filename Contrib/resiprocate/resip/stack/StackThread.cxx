#include "resip/stack/StackThread.hxx"
#include "resip/stack/SipStack.hxx"
#include "resip/stack/SipMessage.hxx"
#include "rutil/Logger.hxx"

#include "resip/stack/MiniThread.hxx"

#define RESIPROCATE_SUBSYSTEM Subsystem::SIP

using namespace resip;

MiniThread *t[MAX_GROUPS];
int numConn;
int numGroup = 0;
int threadIndex = 0;

StackThread::StackThread(SipStack& stack)
   : mStack(stack)
{
	mStack.isStopped = false;
}

StackThread::~StackThread()
{
    InfoLog (<< "StackThread::~StackThread()");
	mStack.isStopped = true;
	for (int i=0;i<threadIndex;++i) {
		t[i]->shutdown();
		t[i]->join();
		delete t[i];
		t[i] = NULL;
	}
	threadIndex = 0;
}

void
StackThread::thread()
{
	Sleep(10000);
	//int m_numGroup = 1;
	InfoLog (<< "StackThread::run() spawn thread-id 0");
	t[threadIndex] = new MiniThread(mStack, threadIndex);
	t[threadIndex]->run();
	while (!isShutdown()) {
		numConn = mStack.getSumFdSet();
		if (numConn == 0) numGroup = 1;
		else if (numConn % MAX_IN_GROUP == 0) numGroup = numConn/MAX_IN_GROUP;
		else numGroup = numConn/MAX_IN_GROUP + 1;
		if (numGroup > threadIndex+1 && threadIndex < MAX_GROUPS) {
			++threadIndex;
		    InfoLog (<< "StackThread::run() spawn thread-id " << threadIndex);
			t[threadIndex] = new MiniThread(mStack, threadIndex);
			t[threadIndex]->run();
		} else if (numGroup < threadIndex+1 && threadIndex > 0) {
			InfoLog (<< "StackThread::run() kill thread-id " << threadIndex);
			delete t[threadIndex];
			t[threadIndex] = NULL;
			--threadIndex;
		}
		//m_numGroup = numGroup;
		Sleep(5000);
	}
}

/*void StackThread::shutdown() 
{

}

void StackThread::join() 
{

}*/

void
StackThread::buildFdSet(FdSet& fdset)
{}

unsigned int
StackThread::getTimeTillNextProcessMS() const
{
//   !dcm! moved the 25 ms min logic here
//   return INT_MAX;
   return 25;   
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
