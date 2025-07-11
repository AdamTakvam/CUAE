// -*- C++ -*-
// OS_NS_arpa_inet.inl,v 1.4 2004/01/07 20:57:06 shuston Exp

#include "ace/OS_NS_string.h"
#include "ace/OS_NS_errno.h"
#include "ace/OS_NS_stdio.h"

ACE_INLINE unsigned long
ACE_OS::inet_addr (const char *name)
{
  ACE_OS_TRACE ("ACE_OS::inet_addr");
#if defined (VXWORKS) || defined (ACE_PSOS)

  u_long ret = 0;
  u_int segment;
  u_int valid = 1;

  for (u_int i = 0; i < 4; ++i)
    {
      ret <<= 8;
      if (*name != '\0')
        {
          segment = 0;

          while (*name >= '0'  &&  *name <= '9')
            {
              segment *= 10;
              segment += *name++ - '0';
            }
          if (*name != '.' && *name != '\0')
            {
              valid = 0;
              break;
            }

          ret |= segment;

          if (*name == '.')
            {
              ++name;
            }
        }
    }
  return valid ? htonl (ret) : INADDR_NONE;
#elif defined (ACE_HAS_NONCONST_GETBY)
  return ::inet_addr ((char *) name);
#else
  return ::inet_addr (name);
#endif /* ACE_HAS_NONCONST_GETBY */
}

// For pSOS, this function is in OS.cpp
#if !defined (ACE_PSOS)
ACE_INLINE char *
ACE_OS::inet_ntoa (const struct in_addr addr)
{
  ACE_OS_TRACE ("ACE_OS::inet_ntoa");
  ACE_OSCALL_RETURN (::inet_ntoa (addr),
                     char *,
                     0);
}
#endif /* defined (ACE_PSOS) */

ACE_INLINE const char *
ACE_OS::inet_ntop (int family, const void *addrptr, char *strptr, size_t len)
{
  ACE_OS_TRACE ("ACE_OS::inet_ntop");

#if defined (ACE_HAS_IPV6) && !defined (ACE_WIN32)
  ACE_OSCALL_RETURN (::inet_ntop (family, addrptr, strptr, len), const char *, 0);
#else
  const u_char *p =
    ACE_reinterpret_cast (const u_char *, addrptr);

  if (family == AF_INET)
    {
      char temp[INET_ADDRSTRLEN];

      // Stevens uses snprintf() in his implementation but snprintf()
      // doesn't appear to be very portable.  For now, hope that using
      // sprintf() will not cause any string/memory overrun problems.
      ACE_OS::sprintf (temp,
                       "%d.%d.%d.%d",
                       p[0], p[1], p[2], p[3]);

      if (ACE_OS::strlen (temp) >= len)
        {
          errno = ENOSPC;
          return 0; // Failure
        }

      ACE_OS::strcpy (strptr, temp);
      return strptr;
    }

  ACE_NOTSUP_RETURN(0);
#endif /* ACE_HAS_IPV6 */
}
ACE_INLINE int
ACE_OS::inet_pton (int family, const char *strptr, void *addrptr)
{
  ACE_OS_TRACE ("ACE_OS::inet_pton");

#if defined (ACE_HAS_IPV6) && !defined (ACE_WIN32)
  ACE_OSCALL_RETURN (::inet_pton (family, strptr, addrptr), int, -1);
#else
  if (family == AF_INET)
    {
      struct in_addr in_val;

      if (ACE_OS::inet_aton (strptr, &in_val))
        {
          ACE_OS::memcpy (addrptr, &in_val, sizeof (struct in_addr));
          return 1; // Success
        }

      return 0; // Input is not a valid presentation format
    }

  ACE_NOTSUP_RETURN(-1);
#endif  /* ACE_HAS_IPV6 */
}

