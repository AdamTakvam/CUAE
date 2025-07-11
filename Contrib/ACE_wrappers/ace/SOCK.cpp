// SOCK.cpp
// SOCK.cpp,v 4.26 2001/12/03 21:30:09 shuston Exp

#include "ace/SOCK.h"
#include "ace/Log_Msg.h"

#if defined (ACE_LACKS_INLINE_FUNCTIONS)
#include "ace/SOCK.i"
#endif

ACE_RCSID(ace, SOCK, "SOCK.cpp,v 4.26 2001/12/03 21:30:09 shuston Exp")

ACE_ALLOC_HOOK_DEFINE(ACE_SOCK)

void
ACE_SOCK::dump (void) const
{
  ACE_TRACE ("ACE_SOCK::dump");
}

ACE_SOCK::ACE_SOCK (void)
{
  // ACE_TRACE ("ACE_SOCK::ACE_SOCK");
}

// Returns information about the remote peer endpoint (if there is
// one).

int
ACE_SOCK::get_remote_addr (ACE_Addr &sa) const
{
  ACE_TRACE ("ACE_SOCK::get_remote_addr");

  int len = sa.get_size ();
  sockaddr *addr = ACE_reinterpret_cast (sockaddr *,
                                         sa.get_addr ());

  if (ACE_OS::getpeername (this->get_handle (),
                           addr,
                           &len) == -1)
    return -1;
  
  sa.set_size (len);
  sa.set_type (addr->sa_family);
  return 0;
}

int
ACE_SOCK::get_local_addr (ACE_Addr &sa) const
{
  ACE_TRACE ("ACE_SOCK::get_local_addr");

  int len = sa.get_size ();
  sockaddr *addr = ACE_reinterpret_cast (sockaddr *,
                                         sa.get_addr ());

  if (ACE_OS::getsockname (this->get_handle (),
                           addr,
                           &len) == -1)
    return -1;

  sa.set_type (addr->sa_family);
  sa.set_size (len);
  return 0;
}

// Close down a ACE_SOCK.

int
ACE_SOCK::close (void)
{
  ACE_TRACE ("ACE_SOCK::close");
  int result = 0;

  if (this->get_handle () != ACE_INVALID_HANDLE)
    {
      result = ACE_OS::closesocket (this->get_handle ());
      this->set_handle (ACE_INVALID_HANDLE);
    }
  return result;
}

int
ACE_SOCK::open (int type, 
                int protocol_family, 
                int protocol,
                int reuse_addr)
{
  ACE_TRACE ("ACE_SOCK::open");
  int one = 1;

  this->set_handle (ACE_OS::socket (protocol_family,
                                    type,
                                    protocol));

  if (this->get_handle () == ACE_INVALID_HANDLE)
    return -1;
  else if (protocol_family != PF_UNIX 
           && reuse_addr 
           && this->set_option (SOL_SOCKET,
                                SO_REUSEADDR,
                                &one,
                                sizeof one) == -1)
    {
      this->close ();
      return -1;
    }
  return 0;
}

// General purpose constructor for performing server ACE_SOCK
// creation.

ACE_SOCK::ACE_SOCK (int type, 
                    int protocol_family, 
                    int protocol,
                    int reuse_addr)
{
  // ACE_TRACE ("ACE_SOCK::ACE_SOCK");
  if (this->open (type,
                  protocol_family, 
                  protocol,
                  reuse_addr) == -1)
    ACE_ERROR ((LM_ERROR,
                ACE_LIB_TEXT ("%p\n"),
                ACE_LIB_TEXT ("ACE_SOCK::ACE_SOCK")));
}

int
ACE_SOCK::open (int type, 
                int protocol_family, 
                int protocol,
                ACE_Protocol_Info *protocolinfo,
                ACE_SOCK_GROUP g,
                u_long flags,
                int reuse_addr)
{
  ACE_TRACE ("ACE_SOCK::open");

  this->set_handle (ACE_OS::socket (protocol_family,
                                    type,
                                    protocol,
                                    protocolinfo,
                                    g,
                                    flags));
  int one = 1;

  if (this->get_handle () == ACE_INVALID_HANDLE)
    return -1;
  else if (reuse_addr 
           && this->set_option (SOL_SOCKET,
                                SO_REUSEADDR,
                                &one,
                                sizeof one) == -1)
    {
      this->close ();
      return -1;
    }
  else
    return 0;
}
      
ACE_SOCK::ACE_SOCK (int type, 
                    int protocol_family, 
                    int protocol,
                    ACE_Protocol_Info *protocolinfo,
                    ACE_SOCK_GROUP g,
                    u_long flags,
                    int reuse_addr)
{
  // ACE_TRACE ("ACE_SOCK::ACE_SOCK");
  if (this->open (type,
                  protocol_family,
                  protocol,
                  protocolinfo,
                  g,
                  flags,
                  reuse_addr) == -1)
    ACE_ERROR ((LM_ERROR,
                ACE_LIB_TEXT ("%p\n"),
                ACE_LIB_TEXT ("ACE_SOCK::ACE_SOCK")));
}
