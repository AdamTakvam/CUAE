// -*- C++ -*-
// OS_NS_netdb.inl,v 1.5 2004/01/07 20:57:06 shuston Exp

#include "ace/OS_NS_macros.h"
#include "ace/OS_NS_string.h"
#include "ace/OS_NS_errno.h"

#if defined (ACE_LACKS_NETDB_REENTRANT_FUNCTIONS)
# if defined (ACE_MT_SAFE) && (ACE_MT_SAFE != 0)
#   define ACE_NETDBCALL_RETURN(OP,TYPE,FAILVALUE,TARGET,SIZE) \
  do \
  { \
    if (ACE_OS::netdb_acquire ())  \
      return FAILVALUE; \
    else \
      { \
        TYPE ace_result_; \
        ACE_OSCALL (OP, TYPE, FAILVALUE, ace_result_); \
        if (ace_result_ != FAILVALUE) \
          ACE_OS::memcpy (TARGET, \
                    ace_result_, \
                    SIZE < sizeof (TYPE) ? SIZE : sizeof (TYPE)); \
        ACE_OS::netdb_release (); \
        return ace_result_; \
      } \
  } while(0)
# else /* ! (ACE_MT_SAFE && ACE_MT_SAFE != 0) */
#   define ACE_NETDBCALL_RETURN(OP,TYPE,FAILVALUE,TARGET,SIZE) \
  do \
  { \
        TYPE ace_result_; \
        ACE_OSCALL(OP,TYPE,FAILVALUE,ace_result_); \
        if (ace_result_ != FAILVALUE) \
          ACE_OS::memcpy (TARGET, \
                    ace_result_, \
                    SIZE < sizeof (TYPE) ? SIZE : sizeof (TYPE)); \
        return ace_result_; \
  } while(0)
# endif /* ACE_MT_SAFE && ACE_MT_SAFE != 0 */
#endif /* ACE_LACKS_NETDB_REENTRANT_FUNCTIONS */

#if !defined (VXWORKS)

ACE_INLINE struct hostent *
ACE_OS::gethostbyaddr (const char *addr, int length, int type)
{
  ACE_OS_TRACE ("ACE_OS::gethostbyaddr");
# if defined (ACE_PSOS)
  ACE_UNUSED_ARG (addr);
  ACE_UNUSED_ARG (length);
  ACE_UNUSED_ARG (type);
  ACE_NOTSUP_RETURN (0);
# elif defined (ACE_HAS_NONCONST_GETBY)
  ACE_SOCKCALL_RETURN (::gethostbyaddr (ACE_const_cast (char *, addr),
                                        (ACE_SOCKET_LEN) length,
                                        type),
                       struct hostent *,
                       0);
# else
  ACE_SOCKCALL_RETURN (::gethostbyaddr (addr,
                                        (ACE_SOCKET_LEN) length,
                                        type),
                       struct hostent *,
                       0);
# endif /* ACE_HAS_NONCONST_GETBY */
}

ACE_INLINE struct hostent *
ACE_OS::gethostbyaddr_r (const char *addr,
                         int length,
                         int type,
                         struct hostent *result,
                         ACE_HOSTENT_DATA buffer,
                         int *h_errnop)
{
  ACE_OS_TRACE ("ACE_OS::gethostbyaddr_r");
# if defined (ACE_PSOS)
  ACE_UNUSED_ARG (addr);
  ACE_UNUSED_ARG (length);
  ACE_UNUSED_ARG (type);
  ACE_UNUSED_ARG (result);
  ACE_UNUSED_ARG (buffer);
  ACE_UNUSED_ARG (h_errnop);
  ACE_NOTSUP_RETURN (0);
# elif defined (ACE_HAS_REENTRANT_FUNCTIONS) && !defined (UNIXWARE)
#   if defined (AIX) || defined (DIGITAL_UNIX) || defined (HPUX_10)
  ACE_OS::memset (buffer, 0, sizeof (ACE_HOSTENT_DATA));

  if (::gethostbyaddr_r ((char *) addr, length, type, result,
                         (struct hostent_data *) buffer)== 0)
    return result;
  else
    {
      *h_errnop = h_errno;
      return (struct hostent *) 0;
    }
# elif defined (__GLIBC__)
  // GNU C library has a different signature
  ACE_OS::memset (buffer, 0, sizeof (ACE_HOSTENT_DATA));

  if (::gethostbyaddr_r ((char *) addr,
                         length,
                         type,
                         result,
                         buffer,
                         sizeof (ACE_HOSTENT_DATA),
                         &result,
                         h_errnop) == 0)
    return result;
  else
    return (struct hostent *) 0;
#   else
#     if defined(ACE_LACKS_NETDB_REENTRANT_FUNCTIONS)
  ACE_UNUSED_ARG (result);
  ACE_UNUSED_ARG (h_errnop);
  ACE_NETDBCALL_RETURN (::gethostbyaddr (addr, (ACE_SOCKET_LEN) length, type),
                        struct hostent *, 0,
                        buffer, sizeof (ACE_HOSTENT_DATA));
#     else
  ACE_SOCKCALL_RETURN (::gethostbyaddr_r (addr, length, type, result,
                                          buffer, sizeof (ACE_HOSTENT_DATA),
                                          h_errnop),
                       struct hostent *, 0);
#     endif /* ACE_LACKS_NETDB_REENTRANT_FUNCTIONS */
#   endif /* defined (AIX) || defined (DIGITAL_UNIX) */
# elif defined (ACE_HAS_NONCONST_GETBY)
  ACE_UNUSED_ARG (result);
  ACE_UNUSED_ARG (buffer);
  ACE_UNUSED_ARG (h_errnop);
  ACE_SOCKCALL_RETURN (::gethostbyaddr (ACE_const_cast (char *, addr),
                                        (ACE_SOCKET_LEN) length,
                                        type),
                       struct hostent *,
                       0);
# else
  ACE_UNUSED_ARG (h_errnop);
  ACE_UNUSED_ARG (buffer);
  ACE_UNUSED_ARG (result);

  ACE_SOCKCALL_RETURN (::gethostbyaddr (addr,
                                        (ACE_SOCKET_LEN) length,
                                        type),
                       struct hostent *,
                       0);
# endif /* defined (ACE_HAS_REENTRANT_FUNCTIONS) && !defined (UNIXWARE) */
}

ACE_INLINE struct hostent *
ACE_OS::gethostbyname (const char *name)
{
  ACE_OS_TRACE ("ACE_OS::gethostbyname");
# if defined (ACE_PSOS)
  ACE_UNUSED_ARG (name);
  ACE_NOTSUP_RETURN (0);
# elif defined (ACE_HAS_NONCONST_GETBY)
  ACE_SOCKCALL_RETURN (::gethostbyname (ACE_const_cast (char *, name)),
                       struct hostent *,
                       0);
# else
  ACE_SOCKCALL_RETURN (::gethostbyname (name),
                       struct hostent *,
                       0);
# endif /* ACE_HAS_NONCONST_GETBY */
}

ACE_INLINE struct hostent *
ACE_OS::gethostbyname_r (const char *name,
                         struct hostent *result,
                         ACE_HOSTENT_DATA buffer,
                         int *h_errnop)
{
  ACE_OS_TRACE ("ACE_OS::gethostbyname_r");
#if defined (ACE_PSOS)
  ACE_UNUSED_ARG (name);
  ACE_UNUSED_ARG (result);
  ACE_UNUSED_ARG (buffer);
  ACE_UNUSED_ARG (h_errnop);
  ACE_NOTSUP_RETURN (0);
# elif defined (ACE_HAS_REENTRANT_FUNCTIONS) && !defined (UNIXWARE)
#   if defined (DIGITAL_UNIX) || \
       (defined (ACE_AIX_MINOR_VERS) && (ACE_AIX_MINOR_VERS > 2))
  ACE_UNUSED_ARG (result);
  ACE_UNUSED_ARG (buffer);
  ACE_UNUSED_ARG (h_errnop);

  // gethostbyname returns thread-specific storage on Digital Unix and
  // AIX 4.3
  ACE_SOCKCALL_RETURN (::gethostbyname (name), struct hostent *, 0);
#   elif defined (AIX) || defined (HPUX_10)
  ACE_OS::memset (buffer, 0, sizeof (ACE_HOSTENT_DATA));

  if (::gethostbyname_r (name, result, (struct hostent_data *) buffer) == 0)
    return result;
  else
    {
      *h_errnop = h_errno;
      return (struct hostent *) 0;
    }
# elif defined (__GLIBC__)
  // GNU C library has a different signature
  ACE_OS::memset (buffer, 0, sizeof (ACE_HOSTENT_DATA));

  if (::gethostbyname_r (name,
                         result,
                         buffer,
                         sizeof (ACE_HOSTENT_DATA),
                         &result,
                         h_errnop) == 0)
    return result;
  else
    return (struct hostent *) 0;
#   else
#     if defined(ACE_LACKS_NETDB_REENTRANT_FUNCTIONS)
  ACE_UNUSED_ARG (result);
  ACE_UNUSED_ARG (h_errnop);
  ACE_NETDBCALL_RETURN (::gethostbyname (name),
                        struct hostent *, 0,
                        buffer, sizeof (ACE_HOSTENT_DATA));
#     else
  ACE_SOCKCALL_RETURN (::gethostbyname_r (name, result, buffer,
                                          sizeof (ACE_HOSTENT_DATA),
                                          h_errnop),
                       struct hostent *,
                       0);
#     endif /* ACE_LACKS_NETDB_REENTRANT_FUNCTIONS */
#   endif /* defined (AIX) || defined (DIGITAL_UNIX) */
# elif defined (ACE_HAS_NONCONST_GETBY)
  ACE_UNUSED_ARG (result);
  ACE_UNUSED_ARG (buffer);
  ACE_UNUSED_ARG (h_errnop);
  ACE_SOCKCALL_RETURN (::gethostbyname (ACE_const_cast (char *, name)),
                       struct hostent *,
                       0);
# else
  ACE_UNUSED_ARG (result);
  ACE_UNUSED_ARG (buffer);
  ACE_UNUSED_ARG (h_errnop);

  ACE_SOCKCALL_RETURN (::gethostbyname (name),
                       struct hostent *,
                       0);
# endif /* defined (ACE_HAS_REENTRANT_FUNCTIONS) && !defined (UNIXWARE) */
}

ACE_INLINE struct hostent *
ACE_OS::getipnodebyaddr (const void *src, size_t len, int family)
{
#if defined (ACE_HAS_IPV6) && !defined (ACE_WIN32)
#  if defined (__GLIBC__)
  ACE_UNUSED_ARG (src);
  ACE_UNUSED_ARG (len);
  ACE_UNUSED_ARG (family);
  ACE_NOTSUP_RETURN (0);
#  else
  struct hostent *hptr;
  int errnum;
  if ((hptr = ::getipnodebyaddr (src, len, family, &errnum)) == 0)
    {
      errno = errnum;
    }
  return hptr;
#  endif /* whatever_doesnt_have_getipnodebyname */
#else
  // IPv4-only implementation
  if (family == AF_INET)
    return ACE_OS::gethostbyaddr (ACE_static_cast (const char *, src),
                                  ACE_static_cast (int, len),
                                  family);

  ACE_NOTSUP_RETURN (0);
# endif /* ACE_PSOS */
}

ACE_INLINE struct hostent *
ACE_OS::getipnodebyname (const char *name, int family, int flags)
{
  ACE_OS_TRACE ("ACE_OS::getipnodebyname");
# if defined (ACE_PSOS)
  ACE_UNUSED_ARG (name);
  ACE_UNUSED_ARG (family);
  ACE_UNUSED_ARG (flags);
  ACE_NOTSUP_RETURN (0);
# elif defined (ACE_HAS_IPV6) && !defined (ACE_WIN32)
#   if defined (__GLIBC__)
  ACE_UNUSED_ARG (flags);
#     if defined (ACE_HAS_NONCONST_GETBY)
  ACE_SOCKCALL_RETURN (::gethostbyname2 (ACE_const_cast (char *, name),
                                         family),
                       struct hostent *, 0);
#     else
  ACE_SOCKCALL_RETURN (::gethostbyname2 (name, family),
                       struct hostent *, 0);
#     endif /* ACE_HAS_NONCONST_GETBY */
#   else
  struct hostent *hptr;
  int errnum;
  if ((hptr = ::getipnodebyname (name, family, flags, &errnum)) == 0)
    {
      errno = errnum;
    }
  return hptr;
#   endif /* __GLIBC__ */
# else
  // IPv4-only implementation
  ACE_UNUSED_ARG (flags);
  if (family == AF_INET)
    return ACE_OS::gethostbyname (name);

  ACE_NOTSUP_RETURN (0);
# endif /* ACE_PSOS */
}

#endif /* ! VXWORKS */

ACE_INLINE struct protoent *
ACE_OS::getprotobyname (const char *name)
{
#if defined (VXWORKS) || defined (ACE_HAS_WINCE) || (defined (ghs) && defined (__Chorus)) || defined (ACE_PSOS)
  ACE_UNUSED_ARG (name);
  ACE_NOTSUP_RETURN (0);
#elif defined (ACE_HAS_NONCONST_GETBY)
  ACE_SOCKCALL_RETURN (::getprotobyname (ACE_const_cast (char *, name)),
                       struct protoent *,
                       0);
#else
  ACE_SOCKCALL_RETURN (::getprotobyname (name),
                       struct protoent *,
                       0);
#endif /* VXWORKS */
}

ACE_INLINE struct protoent *
ACE_OS::getprotobyname_r (const char *name,
                          struct protoent *result,
                          ACE_PROTOENT_DATA buffer)
{
#if defined (VXWORKS) || defined (ACE_HAS_WINCE) || (defined (ghs) && defined (__Chorus)) || defined (ACE_PSOS)
  ACE_UNUSED_ARG (name);
  ACE_UNUSED_ARG (result);
  ACE_UNUSED_ARG (buffer);
  ACE_NOTSUP_RETURN (0);
#elif defined (ACE_HAS_REENTRANT_FUNCTIONS) && !defined (UNIXWARE)
# if defined (AIX) || defined (DIGITAL_UNIX) || defined (HPUX_10)
  if (::getprotobyname_r (name, result, (struct protoent_data *) buffer) == 0)
    return result;
  else
    return 0;
# elif defined (__GLIBC__)
  // GNU C library has a different signature
  if (::getprotobyname_r (name,
                          result,
                          buffer,
                          sizeof (ACE_PROTOENT_DATA),
                          &result) == 0)
    return result;
  else
    return 0;
# else
#   if defined(ACE_LACKS_NETDB_REENTRANT_FUNCTIONS)
  ACE_UNUSED_ARG (result);
  ACE_NETDBCALL_RETURN (::getprotobyname (name),
                        struct protoent *, 0,
                        buffer, sizeof (ACE_PROTOENT_DATA));
#   else
    ACE_SOCKCALL_RETURN (::getprotobyname_r (name,
                                             result,
                                             buffer,
                                             sizeof (ACE_PROTOENT_DATA)),
                       struct protoent *, 0);
#   endif /* ACE_LACKS_NETDB_REENTRANT_FUNCTIONS */
# endif /* defined (AIX) || defined (DIGITAL_UNIX) */
#elif defined (ACE_HAS_NONCONST_GETBY)
  ACE_UNUSED_ARG (result);
  ACE_UNUSED_ARG (buffer);
  ACE_SOCKCALL_RETURN (::getprotobyname (ACE_const_cast (char *, name)),
                       struct protoent *, 0);
#else
  ACE_UNUSED_ARG (buffer);
  ACE_UNUSED_ARG (result);

  ACE_SOCKCALL_RETURN (::getprotobyname (name),
                       struct protoent *,
                       0);
#endif /* defined (ACE_HAS_REENTRANT_FUNCTIONS) !defined (UNIXWARE) */
}

ACE_INLINE struct protoent *
ACE_OS::getprotobynumber (int proto)
{
#if defined (VXWORKS) || defined (ACE_HAS_WINCE) || (defined (ghs) && defined (__Chorus)) || defined (ACE_PSOS)
  ACE_UNUSED_ARG (proto);
  ACE_NOTSUP_RETURN (0);
#else
  ACE_SOCKCALL_RETURN (::getprotobynumber (proto),
                       struct protoent *, 0);
#endif /* VXWORKS */
}

ACE_INLINE struct protoent *
ACE_OS::getprotobynumber_r (int proto,
                            struct protoent *result,
                            ACE_PROTOENT_DATA buffer)
{
#if defined (VXWORKS) || defined (ACE_HAS_WINCE) || (defined (ghs) && defined (__Chorus)) || defined (ACE_PSOS)
  ACE_UNUSED_ARG (proto);
  ACE_UNUSED_ARG (result);
  ACE_UNUSED_ARG (buffer);
  ACE_NOTSUP_RETURN (0);
#elif defined (ACE_HAS_REENTRANT_FUNCTIONS) && !defined (UNIXWARE)
# if defined (AIX) || defined (DIGITAL_UNIX) || defined (HPUX_10)
  if (::getprotobynumber_r (proto, result, (struct protoent_data *) buffer) == 0)
    return result;
  else
    return 0;
# elif defined (__GLIBC__)
  // GNU C library has a different signature
  if (::getprotobynumber_r (proto,
                            result,
                            buffer,
                            sizeof (ACE_PROTOENT_DATA),
                            &result) == 0)
    return result;
  else
    return 0;
# else
#   if defined(ACE_LACKS_NETDB_REENTRANT_FUNCTIONS)
  ACE_UNUSED_ARG (result);
  ACE_NETDBCALL_RETURN (::getprotobynumber (proto),
                        struct protoent *, 0,
                        buffer, sizeof (ACE_PROTOENT_DATA));
#   else
  ACE_SOCKCALL_RETURN (::getprotobynumber_r (proto, result, buffer, sizeof (ACE_PROTOENT_DATA)),
                       struct protoent *, 0);
#   endif /* ACE_LACKS_NETDB_REENTRANT_FUNCTIONS */
# endif /* defined (AIX) || defined (DIGITAL_UNIX) */
#else
  ACE_UNUSED_ARG (buffer);
  ACE_UNUSED_ARG (result);

  ACE_SOCKCALL_RETURN (::getprotobynumber (proto),
                       struct protoent *, 0);
#endif /* defined (ACE_HAS_REENTRANT_FUNCTIONS) && !defined (UNIXWARE) */
}

ACE_INLINE struct servent *
ACE_OS::getservbyname (const char *svc, const char *proto)
{
  ACE_OS_TRACE ("ACE_OS::getservbyname");
#if defined (ACE_LACKS_GETSERVBYNAME)
  ACE_UNUSED_ARG (svc);
  ACE_UNUSED_ARG (proto);
  ACE_NOTSUP_RETURN (0);
#elif defined (ACE_HAS_NONCONST_GETBY)
  ACE_SOCKCALL_RETURN (::getservbyname (ACE_const_cast (char *, svc),
                                        ACE_const_cast (char *, proto)),
                       struct servent *,
                       0);
#else
  ACE_SOCKCALL_RETURN (::getservbyname (svc,
                                        proto),
                       struct servent *,
                       0);
#endif /* ACE_HAS_NONCONST_GETBY */
}

ACE_INLINE struct servent *
ACE_OS::getservbyname_r (const char *svc,
                         const char *proto,
                         struct servent *result,
                         ACE_SERVENT_DATA buf)
{
  ACE_OS_TRACE ("ACE_OS::getservbyname_r");
#if defined (ACE_LACKS_GETSERVBYNAME)
  ACE_UNUSED_ARG (svc);
  ACE_UNUSED_ARG (proto);
  ACE_UNUSED_ARG (result);
  ACE_UNUSED_ARG (buf);
  ACE_NOTSUP_RETURN (0);
#elif defined (ACE_HAS_REENTRANT_FUNCTIONS) && !defined (UNIXWARE)
# if defined (AIX) || defined (DIGITAL_UNIX) || defined (HPUX_10)
  ACE_OS::memset (buf, 0, sizeof (ACE_SERVENT_DATA));

  if (::getservbyname_r (svc, proto, result, (struct servent_data *) buf) == 0)
    return result;
  else
    return (struct servent *) 0;
# elif defined (__GLIBC__)
  // GNU C library has a different signature
  ACE_OS::memset (buf, 0, sizeof (ACE_SERVENT_DATA));

  if (::getservbyname_r (svc,
                         proto,
                         result,
                         buf,
                         sizeof (ACE_SERVENT_DATA),
                         &result) == 0)
    return result;
  else
    return (struct servent *) 0;
# else
#   if defined(ACE_LACKS_NETDB_REENTRANT_FUNCTIONS)
  ACE_UNUSED_ARG (result);
  ACE_NETDBCALL_RETURN (::getservbyname (svc, proto),
                        struct servent *, 0,
                        buf, sizeof (ACE_SERVENT_DATA));
#   else
  ACE_SOCKCALL_RETURN (::getservbyname_r (svc, proto, result, buf,
                                          sizeof (ACE_SERVENT_DATA)),
                       struct servent *, 0);
#   endif /* ACE_LACKS_NETDB_REENTRANT_FUNCTIONS */
# endif /* defined (AIX) || defined (DIGITAL_UNIX) */
#elif defined (ACE_HAS_NONCONST_GETBY)
  ACE_UNUSED_ARG (buf);
  ACE_UNUSED_ARG (result);
  ACE_SOCKCALL_RETURN (::getservbyname (ACE_const_cast (char *, svc),
                                        ACE_const_cast (char *, proto)),
                       struct servent *,
                       0);
#else
  ACE_UNUSED_ARG (buf);
  ACE_UNUSED_ARG (result);

  ACE_SOCKCALL_RETURN (::getservbyname (svc,
                                        proto),
                       struct servent *,
                       0);
#endif /* defined (ACE_HAS_REENTRANT_FUNCTIONS) && !defined (UNIXWARE) */
}
