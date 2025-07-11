/* -*- C++ -*- */
// SOCK_Dgram_Mcast.i,v 4.13 2002/06/25 23:25:58 crodrigu Exp

ASYS_INLINE
ACE_SOCK_Dgram_Mcast::~ACE_SOCK_Dgram_Mcast (void)
{
}

ASYS_INLINE int
ACE_SOCK_Dgram_Mcast::set_option (int option, 
				  char optval) 
{ 
  ACE_TRACE ("ACE_SOCK_Dgram_Mcast::set_option");
#if defined (ACE_WIN32)
  int sock_opt = optval;
  return this->ACE_SOCK::set_option (IPPROTO_IP,
                                     option, 
				     &sock_opt,
                                     sizeof (sock_opt));
#else
  return this->ACE_SOCK::set_option (IPPROTO_IP,
                                     option, 
				     &optval,
                                     sizeof (optval));
#endif /* !ACE_WIN32 */  
}

ASYS_INLINE ssize_t
ACE_SOCK_Dgram_Mcast::send (const void *buf, 
			    size_t n, 
                            int flags) const
{
  ACE_TRACE ("ACE_SOCK_Dgram_Mcast::send");
  return this->ACE_SOCK_Dgram::send (buf, 
                                     n, 
				     this->mcast_addr_, 
                                     flags);
}

ASYS_INLINE ssize_t
ACE_SOCK_Dgram_Mcast::send (const iovec iov[], 
			    size_t n, 
                            int flags) const
{
  ACE_TRACE ("ACE_SOCK_Dgram_Mcast::send");
  return this->ACE_SOCK_Dgram::send (iov, 
                                     n, 
				     this->mcast_addr_, 
                                     flags);
}

ASYS_INLINE ssize_t
ACE_SOCK_Dgram_Mcast::send (const void *buf, 
			    size_t n, 
                            const ACE_Addr &addr, 		
                            int flags) const
{
  ACE_TRACE ("ACE_SOCK_Dgram_Mcast::send");
  return this->ACE_SOCK_Dgram::send (buf, 
                                     n, 
				     addr, 
                                     flags);
}

ASYS_INLINE ssize_t
ACE_SOCK_Dgram_Mcast::send (const iovec iov[], 
			    size_t n, 
                            const ACE_Addr &addr, 		
                            int flags) const
{
  ACE_TRACE ("ACE_SOCK_Dgram_Mcast::send");
  return this->ACE_SOCK_Dgram::send (iov, 
                                     n, 
				     addr, 
                                     flags);
}


