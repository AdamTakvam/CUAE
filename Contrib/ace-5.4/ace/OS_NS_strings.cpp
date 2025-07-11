// -*- C++ -*-
// OS_NS_strings.cpp,v 1.3 2003/11/13 20:20:14 dhinton Exp

#include "ace/OS_NS_strings.h"

ACE_RCSID(ace, OS_NS_strings, "OS_NS_strings.cpp,v 1.3 2003/11/13 20:20:14 dhinton Exp")

#if !defined (ACE_HAS_INLINED_OSCALLS)
# include "ace/OS_NS_strings.inl"
#endif /* ACE_HAS_INLINED_OS_CALLS */

#if defined (ACE_LACKS_STRCASECMP)
#  include "ace/OS_NS_ctype.h"
#endif /* ACE_LACKS_STRCASECMP */

#if defined (ACE_LACKS_STRCASECMP)
int
ACE_OS::strcasecmp_emulation (const char *s, const char *t)
{
  const char *scan1 = s;
  const char *scan2 = t;

  while (*scan1 != 0
         && ACE_OS::to_lower (*scan1)
            == ACE_OS::to_lower (*scan2))
    {
      ++scan1;
      ++scan2;
    }

  // The following case analysis is necessary so that characters which
  // look negative collate low against normal characters but high
  // against the end-of-string NUL.

  if (*scan1 == '\0' && *scan2 == '\0')
    return 0;
  else if (*scan1 == '\0')
    return -1;
  else if (*scan2 == '\0')
    return 1;
  else
    return ACE_OS::to_lower (*scan1) - ACE_OS::to_lower (*scan2);
}
#endif /* ACE_LACKS_STRCASECMP */

#if defined (ACE_LACKS_STRCASECMP)
int
ACE_OS::strncasecmp_emulation (const char *s,
                               const char *t,
                               size_t len)
{
  const char *scan1 = s;
  const char *scan2 = t;
  size_t count = 0;

  while (count++ < len
         && *scan1 != 0
         && ACE_OS::to_lower (*scan1)
            == ACE_OS::to_lower (*scan2))
    {
      ++scan1;
      ++scan2;
    }

  if (count > len)
    return 0;

  // The following case analysis is necessary so that characters which
  // look negative collate low against normal characters but high
  // against the end-of-string NUL.

  if (*scan1 == '\0' && *scan2 == '\0')
    return 0;
  else if (*scan1 == '\0')
    return -1;
  else if (*scan2 == '\0')
    return 1;
  else
    return ACE_OS::to_lower (*scan1) - ACE_OS::to_lower (*scan2);
}
#endif /* ACE_LACKS_STRCASECMP */
