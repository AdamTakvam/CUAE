#if defined(HAVE_CONFIG_H)
#include "resip/stack/config.hxx"
#endif

#include <iostream>

#if defined(HAVE_SYS_SOCKIO_H)
#include <sys/sockio.h>
#endif

#include "rutil/compat.hxx"
#include "rutil/DnsUtil.hxx"
#include "rutil/Logger.hxx"
#include "rutil/Socket.hxx"
#include "rutil/ParseBuffer.hxx"

#include "resip/stack/ConnectionTerminated.hxx"
#include "resip/stack/Transport.hxx"
#include "resip/stack/SipMessage.hxx"
#include "resip/stack/TransportFailure.hxx"
#include "resip/stack/Helper.hxx"
#include "rutil/WinLeakCheck.hxx"

using namespace resip;
using namespace std;

#define RESIPROCATE_SUBSYSTEM Subsystem::TRANSPORT

Transport::Exception::Exception(const Data& msg, const Data& file, const int line) :
   BaseException(msg,file,line)
{
}

Transport::Transport(Fifo<TransactionMessage>& rxFifo,
                     const GenericIPAddress& address,
                     const Data& tlsDomain,
                     AfterSocketCreationFuncPtr socketFunc) :
   mTuple(address),
   mStateMachineFifo(rxFifo),
   mShuttingDown(false),
   mTlsDomain(tlsDomain),
   mSocketFunc(socketFunc)
{
   mInterface = Tuple::inet_ntop(mTuple);
}

Transport::Transport(Fifo<TransactionMessage>& rxFifo,
                     int portNum, 
                     IpVersion version,
                     const Data& intfc,
                     const Data& tlsDomain,
                     AfterSocketCreationFuncPtr socketFunc) :
   mInterface(intfc),
   mTuple(intfc, portNum, version),
   mStateMachineFifo(rxFifo),
   mShuttingDown(false),
   mTlsDomain(tlsDomain),
   mSocketFunc(socketFunc)
{
}

Transport::~Transport()
{
}

void
Transport::error(int e)
{
   switch (e)
   {
      case EAGAIN:
         //InfoLog (<< "No data ready to read" << strerror(e));
         break;
      case EINTR:
         InfoLog (<< "The call was interrupted by a signal before any data was read : " << strerror(e));
         break;
      case EIO:
         InfoLog (<< "I/O error : " << strerror(e));
         break;
      case EBADF:
         InfoLog (<< "fd is not a valid file descriptor or is not open for reading : " << strerror(e));
         break;
      case EINVAL:
         InfoLog (<< "fd is attached to an object which is unsuitable for reading : " << strerror(e));
         break;
      case EFAULT:
         InfoLog (<< "buf is outside your accessible address space : " << strerror(e));
         break;

#if defined(WIN32)
      case WSAENETDOWN: 
         InfoLog (<<" The network subsystem has failed.  ");
         break;
      case WSAEFAULT:
         InfoLog (<<" The buf or from parameters are not part of the user address space, "
                   "or the fromlen parameter is too small to accommodate the peer address.  ");
         break;
      case WSAEINTR: 
         InfoLog (<<" The (blocking) call was canceled through WSACancelBlockingCall.  ");
         break;
      case WSAEINPROGRESS: 
         InfoLog (<<" A blocking Windows Sockets 1.1 call is in progress, or the "
                   "service provider is still processing a callback function.  ");
         break;
      case WSAEINVAL: 
         InfoLog (<<" The socket has not been bound with bind, or an unknown flag was specified, "
                   "or MSG_OOB was specified for a socket with SO_OOBINLINE enabled, "
                   "or (for byte stream-style sockets only) len was zero or negative.  ");
         break;
      case WSAEISCONN : 
         InfoLog (<<"The socket is connected. This function is not permitted with a connected socket, "
                  "whether the socket is connection-oriented or connectionless.  ");
         break;
      case WSAENETRESET:
         InfoLog (<<" The connection has been broken due to the keep-alive activity "
                  "detecting a failure while the operation was in progress.  ");
         break;
      case WSAENOTSOCK :
         InfoLog (<<"The descriptor is not a socket.  ");
         break;
      case WSAEOPNOTSUPP:
         InfoLog (<<" MSG_OOB was specified, but the socket is not stream-style such as type "
                   "SOCK_STREAM, OOB data is not supported in the communication domain associated with this socket, "
                   "or the socket is unidirectional and supports only send operations.  ");
         break;
      case WSAESHUTDOWN:
         InfoLog (<<"The socket has been shut down; it is not possible to recvfrom on a socket after "
                  "shutdown has been invoked with how set to SD_RECEIVE or SD_BOTH.  ");
         break;
      case WSAEMSGSIZE:
         InfoLog (<<" The message was too large to fit into the specified buffer and was truncated.  ");
         break;
      case WSAETIMEDOUT: 
         InfoLog (<<" The connection has been dropped, because of a network failure or because the "
                  "system on the other end went down without notice.  ");
         break;
      case WSAECONNRESET : 
         InfoLog (<<"Connection reset ");
         break;

	   case WSAEWOULDBLOCK:
         DebugLog (<<"Would Block ");
         break;

      case WSAEHOSTUNREACH:
         InfoLog (<<"A socket operation was attempted to an unreachable host ");
         break;
      case WSANOTINITIALISED:
         InfoLog (<<"Either the application has not called WSAStartup or WSAStartup failed. "
                  "The application may be accessing a socket that the current active task does not own (that is, trying to share a socket between tasks),"
                  "or WSACleanup has been called too many times.  ");
         break;
      case WSAEACCES:
         InfoLog (<<"An attempt was made to access a socket in a way forbidden by its access permissions ");
         break;
      case WSAENOBUFS:
         InfoLog (<<"An operation on a socket could not be performed because the system lacked sufficient "
                  "buffer space or because a queue was full");
         break;
      case WSAENOTCONN:
         InfoLog (<<"A request to send or receive data was disallowed because the socket is not connected "
                  "and (when sending on a datagram socket using sendto) no address was supplied");
         break;
      case WSAECONNABORTED:
         InfoLog (<<"An established connection was aborted by the software in your host computer, possibly "
                  "due to a data transmission time-out or protocol error");
         break;
      case WSAEADDRNOTAVAIL:
         InfoLog (<<"The requested address is not valid in its context. This normally results from an attempt to "
                  "bind to an address that is not valid for the local computer");
         break;
      case WSAEAFNOSUPPORT:
         InfoLog (<<"An address incompatible with the requested protocol was used");
         break;
      case WSAEDESTADDRREQ:
         InfoLog (<<"A required address was omitted from an operation on a socket");
         break;
      case WSAENETUNREACH:
         InfoLog (<<"A socket operation was attempted to an unreachable network");
         break;
	  case WSAEADDRINUSE:
         InfoLog (<<"Address already in use");
         break;

#endif

      default:
         InfoLog (<< "Some other error (" << e << "): " << strerror(e));
         break;
   }
}

void
Transport::connectionTerminated(ConnectionId id)
{
   mStateMachineFifo.add(new ConnectionTerminated(this, id));
}
   
void
Transport::fail(const Data& tid)
{
   if (!tid.empty())
   {
      mStateMachineFifo.add(new TransportFailure(tid));
   }
}

/// @todo unify w/ tramsit
void 
Transport::send( const Tuple& dest, const Data& d, const Data& tid)
{
   assert(dest.getPort() != -1);
   DebugLog (<< "Adding message to tx buffer to: " << dest); // << " " << d.escaped());
   transmit(dest, d, tid); 
}

void
Transport::makeFailedResponse(const SipMessage& msg,
                              int responseCode,
                              const char * warning)
{
  if (msg.isResponse()) return;

  const Tuple& dest = msg.getSource();

  std::auto_ptr<SipMessage> errMsg(Helper::makeResponse(msg, 
                                                        responseCode, 
                                                        warning ? warning : "Original request had no Vias"));
  
  // make send data here w/ blank tid and blast it out.
  // encode message
  Data encoded;
  encoded.clear();
  DataStream encodeStream(encoded);
  errMsg->encode(encodeStream);
  encodeStream.flush();
  assert(!encoded.empty());

  InfoLog(<<"Sending response directly to " << dest << " : " << errMsg->brief() );
  transmit(dest, encoded, Data::Empty);
}


void
Transport::stampReceived(SipMessage* message)
{
   // set the received= and rport= parameters in the message if necessary !jf!
   if (message->isRequest() && message->exists(h_Vias) && !message->header(h_Vias).empty())
   {
      const Tuple& tuple = message->getSource();
	  Data received = Tuple::inet_ntop(tuple);
	  if(message->header(h_Vias).front().sentHost() != received)  // only add if received address is different from sent-by in Via
	  {
         message->header(h_Vias).front().param(p_received) = received;
	  }
      //message->header(h_Vias).front().param(p_received) = Tuple::inet_ntop(tuple);
      if (message->header(h_Vias).front().exists(p_rport))
      {
         message->header(h_Vias).front().param(p_rport).port() = tuple.getPort();
      }
   }
   DebugLog (<< "incoming from: " << message->getSource());
   StackLog (<< endl << *message);
}


bool
Transport::basicCheck(const SipMessage& msg)
{
   if (msg.isExternal())
   {
      try
      {
         try
         {
            if (!Helper::validateMessage(msg))
            {
               InfoLog(<<"Message Failed basicCheck :" << msg.brief());
               if (msg.isRequest())
               {
                  // this is VERY low-level b/c we don't have a transaction...
                  // here we make a response to warn the offending party.
                  makeFailedResponse(msg);
               }
               return false;
            }
            else if (mShuttingDown && msg.isRequest())
            {
               InfoLog (<< "Server has been shutdown, reject message with 503");
               // this is VERY low-level b/c we don't have a transaction...
               // here we make a response to warn the offending party.
               makeFailedResponse(msg, 503, "Server has been shutdown");
            }
         }
         catch (ParseBuffer::Exception& e)
         {
            InfoLog (<< "Parse exception in basic check: " << e);
            makeFailedResponse(msg);
         }
      }
      catch (BaseException& e)
      {
         InfoLog (<< "Cannot make failure response to badly constructed message: " << e);
         return false;
      }
   }
   return true;
}

bool 
Transport::operator==(const Transport& rhs) const
{
   return ( ( mTuple.isV4() == rhs.isV4()) &&
            ( port() == rhs.port()) &&
            ( memcmp(&boundInterface(),&rhs.boundInterface(),mTuple.length()) == 0) );
}
    
std::ostream& 
resip::operator<<(std::ostream& strm, const resip::Transport& rhs)
{
   strm << "Transport: " << rhs.mTuple;
   if (!rhs.mInterface.empty()) strm << " on " << rhs.mInterface;
   return strm;
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
