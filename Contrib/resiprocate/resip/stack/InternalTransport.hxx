#if !defined(RESIP_INTERNAL_TRANSPORT_HXX)
#define RESIP_INTERNAL_TRANSPORT_HXX

#include <exception>

#include "rutil/BaseException.hxx"
#include "rutil/Data.hxx"
#include "rutil/Fifo.hxx"
#include "rutil/Socket.hxx"
#include "resip/stack/Message.hxx"
#include "resip/stack/Transport.hxx"
#include "resip/stack/Tuple.hxx"
#include "resip/stack/SendData.hxx"

namespace resip
{

class TransactionMessage;
class SipMessage;
class Connection;

class InternalTransport : public Transport
{
   public:
      // sendHost what to put in the Via:sent-by
      // portNum is the port to receive and/or send on
      InternalTransport(Fifo<TransactionMessage>& rxFifo, 
                        int portNum, 
                        IpVersion version,
                        const Data& interfaceObj,
						AfterSocketCreationFuncPtr socketFunc = 0,
						Socket fd = INVALID_SOCKET
                        );

      virtual ~InternalTransport();

      virtual bool isFinished() const;
      virtual bool hasDataToSend() const;

      virtual bool shareStackProcessAndSelect() const { return true; }
      virtual void startOwnProcessing() {}
      virtual void stopOwnProcessing() {}

      // shared by UDP, TCP, and TLS
      static Socket socket(TransportType type, IpVersion ipVer);
      void bind();      

      //used for epoll
      virtual int maxFileDescriptors() const { return 1; }
      virtual unsigned int getFifoSize() const;      
   protected:
      friend class SipStack;
      virtual void transmit(const Tuple& dest, const Data& pdata, const Data& tid);

      Socket mFd; // this is a unix file descriptor or a windows SOCKET
      Fifo<SendData> mTxFifo; // owned by the transport
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
