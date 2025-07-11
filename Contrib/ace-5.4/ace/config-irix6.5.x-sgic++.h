// -*- C++ -*-
// config-irix6.5.x-sgic++.h,v 1.3 2003/12/09 15:02:27 dhinton Exp

// Use this file for IRIX 6.5.x

#ifndef ACE_CONFIG_IRIX65X_H
#define ACE_CONFIG_IRIX65X_H
#include /**/ "ace/pre.h"

// Include IRIX 6.[234] configuration
#include "ace/config-irix6.x-sgic++.h"

#undef ACE_HAS_EXPLICIT_TEMPLATE_INSTANTIATION
#define ACE_HAS_STD_TEMPLATE_CLASS_MEMBER_SPECIALIZATION   

// Irix 6.5 man pages show that they exist
#undef ACE_LACKS_CONDATTR_PSHARED
#undef ACE_LACKS_MUTEXATTR_PSHARED

#include /**/ "ace/post.h"
#endif /* ACE_CONFIG_IRIX65X_H */
