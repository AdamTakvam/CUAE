// SPIPE_Addr.cpp
// SPIPE_Addr.cpp,v 4.15 2002/03/09 13:59:29 schmidt Exp

#include "ace/SPIPE_Addr.h"

#if !defined (__ACE_INLINE__)
#include "ace/SPIPE_Addr.i"
#endif /* __ACE_INLINE__ */

ACE_RCSID(ace, SPIPE_Addr, "SPIPE_Addr.cpp,v 4.15 2002/03/09 13:59:29 schmidt Exp")

ACE_ALLOC_HOOK_DEFINE(ACE_SPIPE_Addr)

void
ACE_SPIPE_Addr::dump (void) const
{
}

// Set a pointer to the address.
void 
ACE_SPIPE_Addr::set_addr (void *addr, int len)
{
  ACE_TRACE ("ACE_SPIPE_Addr::set_addr");

  this->ACE_Addr::base_set (AF_SPIPE, len);
  ACE_OS::memcpy ((void *) &this->SPIPE_addr_,
		  (void *) addr, 
		  len);
}

// Do nothing constructor. 

ACE_SPIPE_Addr::ACE_SPIPE_Addr (void)
  : ACE_Addr (AF_SPIPE, sizeof this->SPIPE_addr_)
{
  (void) ACE_OS::memset ((void *) &this->SPIPE_addr_, 
                         0, 
			 sizeof this->SPIPE_addr_);
}

// Transform the string into the current addressing format.

int
ACE_SPIPE_Addr::string_to_addr (const ACE_TCHAR *addr)
{
  return this->set (addr);
}

int
ACE_SPIPE_Addr::set (const ACE_SPIPE_Addr &sa)
{
  this->base_set (sa.get_type (), sa.get_size ());

  if (sa.get_type () == AF_ANY)
    (void) ACE_OS::memset ((void *) &this->SPIPE_addr_,
                           0,
                           sizeof this->SPIPE_addr_);
  else
    (void) ACE_OS::memcpy ((void *) &this->SPIPE_addr_, (void *)
                           &sa.SPIPE_addr_,
                           sa.get_size ()); 
  return 0;
}

// Copy constructor.

ACE_SPIPE_Addr::ACE_SPIPE_Addr (const ACE_SPIPE_Addr &sa)
  : ACE_Addr (AF_SPIPE, sizeof this->SPIPE_addr_)
{
  this->set (sa);
}

int
ACE_SPIPE_Addr::set (const ACE_TCHAR *addr,
		     gid_t gid, 
		     uid_t uid)
{
  int len = sizeof (this->SPIPE_addr_.uid_);
  len += sizeof (this->SPIPE_addr_.gid_);

#if defined (ACE_WIN32)
  const ACE_TCHAR *colonp = ACE_OS::strchr (addr, ':');
  ACE_TCHAR temp[BUFSIZ];

  if (colonp == 0) // Assume it's a local name.
    {
      ACE_OS::strcpy (temp, ACE_LIB_TEXT ( "\\\\.\\pipe\\"));
      ACE_OS::strcat (temp, addr);
    }
  else
    {
      
      if (ACE_OS::strncmp (addr,
                           ACE_LIB_TEXT ("localhost"),
                           ACE_OS::strlen ("localhost")) == 0)
        // change "localhost" to "."
        ACE_OS::strcpy (temp, ACE_LIB_TEXT ("\\\\."));
      else
        {
          ACE_OS::strcpy (temp, ACE_LIB_TEXT ("\\\\"));

          ACE_TCHAR *t;
          
          // We need to allocate a duplicate so that we can write a
          // NUL character into it.
          ACE_ALLOCATOR_RETURN (t, ACE_OS::strdup (addr), -1);

          t[colonp - addr] = ACE_LIB_TEXT ('\0');
          ACE_OS::strcat (temp, t);

          ACE_OS::free (t);
        }

      ACE_OS::strcat (temp, ACE_LIB_TEXT ("\\pipe\\"));
      ACE_OS::strcat (temp, colonp + 1);
    }
  this->ACE_Addr::base_set (AF_SPIPE, 
			    ACE_OS::strlen (temp) + len);

  ACE_OS::strcpy (this->SPIPE_addr_.rendezvous_, temp);
#else
  this->ACE_Addr::base_set (AF_SPIPE,
                            ACE_OS::strlen (addr) + 1 + len);
  ACE_OS::strsncpy (this->SPIPE_addr_.rendezvous_,
                    addr,
                    sizeof this->SPIPE_addr_.rendezvous_);
#endif /* ACE_WIN32 */
  this->SPIPE_addr_.gid_ = gid == 0 ? ACE_OS::getgid () : gid;
  this->SPIPE_addr_.uid_ = uid == 0 ? ACE_OS::getuid () : uid;
  return 0;
}

// Create a ACE_Addr from a ACE_SPIPE pathname. 

ACE_SPIPE_Addr::ACE_SPIPE_Addr (const ACE_TCHAR *addr,
				gid_t gid, 
				uid_t uid)
  : ACE_Addr (AF_SPIPE, sizeof this->SPIPE_addr_)
{
  this->set (addr, gid, uid);
}

