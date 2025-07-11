#if !defined(RESIP_TCPBASETRANSPORT_HXX)
#define RESIP_TCPBASETRANSPORT_HXX

#include "resip/stack/InternalTransport.hxx"
#include "resip/stack/ConnectionManager.hxx"

namespace resip
{

class TransactionMessage;

class TcpBaseTransport : public InternalTransport
{
   public:
      enum  {MaxFileDescriptors = 100000};

     TcpBaseTransport(Fifo<TransactionMessage>& fifo, int portNum,  IpVersion version, const Data& interfaceName, Socket fd = INVALID_SOCKET );
      virtual  ~TcpBaseTransport();
      
      virtual void process(FdSet& fdset);
      virtual void buildFdSet( FdSet& fdset);
      virtual bool isReliable() const { return true; }
      virtual int maxFileDescriptors() const { return MaxFileDescriptors; }

      ConnectionManager& getConnectionManager() {return mConnectionManager;}
      const ConnectionManager& getConnectionManager() const {return mConnectionManager;}

   protected:
      virtual Connection* createConnection(Tuple& who, Socket fd, bool server=false)=0;
      
      void processSomeWrites(FdSet& fdset);
      void processSomeReads(FdSet& fdset);
      
      /** Forms a connection if one doesn't exist, moves requests to the
      appropriate connection's fifo.
      */
      void processAllWriteRequests(FdSet& fdset);
      void sendFromRoundRobin(FdSet& fdset);
      void processListen(FdSet& fdSet);

      static const size_t MaxWriteSize;
      static const size_t MaxReadSize;

   private:
      static const int MaxBufferSize;
      ConnectionManager mConnectionManager;
};

}

#endif

/* ====================================================================
 * The Vovida Software License, Version 1.0 
 * 
 * Copyright (c) 2000-2005 Vovida Networks, Inc.  All rights reserved.
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
