// -*- C++ -*-

//=============================================================================
/**
 *  @file     sslconf.h
 *
 *  sslconf.h,v 1.7 2003/07/19 19:04:15 dhinton Exp
 *
 *  @author   Carlos O'Ryan <coryan@ece.uci.edu>
 */
//=============================================================================


#ifndef ACE_SSLCONF_H
#define ACE_SSLCONF_H

#include /**/ "ace/pre.h"

#include "ace/config-all.h"

#if !defined (ACE_DEFAULT_SSL_CERT_FILE)
#  ifdef WIN32
#    define ACE_DEFAULT_SSL_CERT_FILE "cert.pem"
#  else
#    define ACE_DEFAULT_SSL_CERT_FILE "/etc/ssl/cert.pem"
#  endif  /* WIN32 */
#endif /* ACE_DEFAULT_SSL_CERT_FILE */

#if !defined (ACE_DEFAULT_SSL_CERT_DIR)
#  ifdef WIN32
#    define ACE_DEFAULT_SSL_CERT_DIR "certs"
#  else
#    define ACE_DEFAULT_SSL_CERT_DIR "/etc/ssl/certs"
#  endif  /* WIN32 */
#endif /* ACE_DEFAULT_SSL_CERT_DIR */

#if !defined (ACE_SSL_CERT_FILE_ENV)
#define ACE_SSL_CERT_FILE_ENV "SSL_CERT_FILE"
#endif /* ACE_SSL_CERT_FILE_ENV */

#if !defined (ACE_SSL_CERT_DIR_ENV)
#define ACE_SSL_CERT_DIR_ENV  "SSL_CERT_DIR"
#endif /* ACE_SSL_CERT_DIR_ENV */

#if !defined (ACE_SSL_EGD_FILE_ENV)
#define ACE_SSL_EGD_FILE_ENV  "SSL_EGD_FILE"
#endif /* ACE_SSL_EGD_FILE_ENV */

#if !defined (ACE_SSL_RAND_FILE_ENV)
#define ACE_SSL_RAND_FILE_ENV  "SSL_RAND_FILE"
#endif /* ACE_SSL_RAND_FILE_ENV */

#include /**/ "ace/post.h"

#endif /* ACE_SSLCONF_H */
