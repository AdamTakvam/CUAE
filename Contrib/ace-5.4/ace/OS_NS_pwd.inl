// -*- C++ -*-
// OS_NS_pwd.inl,v 1.2 2003/11/01 11:15:15 dhinton Exp

#include "ace/OS_NS_errno.h"

// Accessors to PWD file.

ACE_INLINE void
ACE_OS::endpwent (void)
{
#if !defined (ACE_LACKS_PWD_FUNCTIONS)
# if !defined (ACE_WIN32)
  ::endpwent ();
# else
# endif /* ACE_WIN32 */
#else
#endif /* ! ACE_LACKS_PWD_FUNCTIONS */
}

ACE_INLINE struct passwd *
ACE_OS::getpwent (void)
{
#if !defined (ACE_LACKS_PWD_FUNCTIONS)
# if !defined (ACE_WIN32)
  return ::getpwent ();
# else
  ACE_NOTSUP_RETURN (0);
# endif /* ACE_WIN32 */
#else
  ACE_NOTSUP_RETURN (0);
#endif /* ! ACE_LACKS_PWD_FUNCTIONS */
}

ACE_INLINE struct passwd *
ACE_OS::getpwnam (const char *name)
{
#if !defined (ACE_LACKS_PWD_FUNCTIONS)
# if !defined (ACE_WIN32)
  return ::getpwnam (name);
# else
  ACE_UNUSED_ARG (name);
  ACE_NOTSUP_RETURN (0);
# endif /* ACE_WIN32 */
#else
  ACE_UNUSED_ARG (name);
  ACE_NOTSUP_RETURN (0);
#endif /* ACE_LACKS_PWD_FUNCTIONS */
}

ACE_INLINE struct passwd *
ACE_OS::getpwnam_r (const char *name, struct passwd *pwent,
                    char *buffer, int buflen)
{
#if defined (ACE_HAS_POSIX_GETPWNAM_R)
  struct passwd *result;
  int status;

  status = ::getpwnam_r (name, pwent, buffer, buflen, &result);

  if (status != 0)
  {
    errno = status;
    result = 0;
  }
  return result;
#elif !defined (ACE_LACKS_PWD_FUNCTIONS)
# if defined (ACE_HAS_REENTRANT_FUNCTIONS)
#   if !defined (ACE_LACKS_PWD_REENTRANT_FUNCTIONS)
#     if defined (ACE_HAS_PTHREADS_STD) && \
      !defined (ACE_HAS_STHREADS) || \
      defined (HPUX_11)  || \
      defined (__USLC__) // Added by Roland Gigler for SCO UnixWare 7.
  struct passwd *result;
  int status;
#       if defined (DIGITAL_UNIX)
  ::_Pgetpwnam_r (name, pwent, buffer, buflen, &result);
#       else
  // VAC++ doesn't correctly grok the ::getpwnam_r - the function is redefined
  // in pwd.h, and that redefinition is used here
#         if defined (__IBMCPP__) && (__IBMCPP__ >= 400)   /* VAC++ 4 */
  status = _posix_getpwnam_r (name, pwent, buffer, buflen, &result);
#         else
  status = ::getpwnam_r (name, pwent, buffer, buflen, &result);
#         endif /* __IBMCPP__ && (__IBMCPP__ >= 400) */
  if (status != 0)
    {
      errno = status;
      result = 0;
    }
#       endif /* (DIGITAL_UNIX) */
  return result;
#     elif defined (AIX) || defined (HPUX_10)
  if (::getpwnam_r (name, pwent, buffer, buflen) == -1)
    return 0;
  else
    return pwent;
#     else
  return ::getpwnam_r (name, pwent, buffer, buflen);
#     endif /* ACE_HAS_PTHREADS_STD */
#   else
  ACE_UNUSED_ARG (name);
  ACE_UNUSED_ARG (pwent);
  ACE_UNUSED_ARG (buffer);
  ACE_UNUSED_ARG (buflen);
  ACE_NOTSUP_RETURN (0);
#   endif /* ! ACE_LACKS_PWD_REENTRANT_FUNCTIONS */
# else
  ACE_UNUSED_ARG (name);
  ACE_UNUSED_ARG (pwent);
  ACE_UNUSED_ARG (buffer);
  ACE_UNUSED_ARG (buflen);
  ACE_NOTSUP_RETURN (0);
# endif /* ACE_HAS_REENTRANT_FUNCTIONS */
#else
  ACE_UNUSED_ARG (name);
  ACE_UNUSED_ARG (pwent);
  ACE_UNUSED_ARG (buffer);
  ACE_UNUSED_ARG (buflen);
  ACE_NOTSUP_RETURN (0);
#endif /* ACE_HAS_POSIX_GETPWNAM_R */
}

ACE_INLINE void
ACE_OS::setpwent (void)
{
#if !defined (ACE_LACKS_PWD_FUNCTIONS)
# if !defined (ACE_WIN32)
  ::setpwent ();
# else
# endif /* ACE_WIN32 */
#else
#endif /* ! ACE_LACKS_PWD_FUNCTIONS */
}
