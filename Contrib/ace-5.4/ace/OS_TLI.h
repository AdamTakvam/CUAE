// -*- C++ -*-

//=============================================================================
/**
 *  @file   OS_TLI.h
 *
 *  OS_TLI.h,v 4.17 2003/12/22 22:50:34 shuston Exp
 *
 *  @author Doug Schmidt <schmidt@cs.wustl.edu>
 */
//=============================================================================


#ifndef ACE_OS_TLI_H
# define ACE_OS_TLI_H
# include /**/ "ace/pre.h"

# include "ace/config-all.h"

# if !defined (ACE_LACKS_PRAGMA_ONCE)
#  pragma once
# endif /* ACE_LACKS_PRAGMA_ONCE */

# include "ace/OS_Errno.h"
# include "ace/ACE_export.h"

# if defined (ACE_EXPORT_MACRO)
#  undef ACE_EXPORT_MACRO
# endif
# define ACE_EXPORT_MACRO ACE_Export

# if !(defined(ACE_HAS_TLI) || defined(ACE_HAS_XTI))
// Dummies to help compilation.
struct t_call { };
struct t_bind { };
struct t_info { };
struct t_optmgmt { };
struct t_discon { };
struct t_unitdata { };
struct t_uderr { };
struct netbuf { };

# else /* !(ACE_HAS_TLI || ACE_HAS_XTI) */

#   if defined (ACE_HAS_CONFLICTING_XTI_MACROS)
      // Make sure tcp.h gets included before sys/xti.h.
#     include "ace/os_include/netinet/os_tcp.h"
#     undef TCP_NODELAY
#     undef TCP_MAXSEG
#   endif /* ACE_HAS_CONFLICTING_XTI_MACROS */

#   if defined (ACE_HAS_XTI)
#     if defined (ACE_HAS_SYS_XTI_H)   /* Nonstandard header placement */
#       define class ace_xti_class
#       include /**/ <sys/xti.h>
#       undef class
#     elif defined (ACE_HAS_FORE_ATM_XTI)
#       include /**/ <fore_xti/xti_user_types.h>
#       include /**/ <fore_xti/xti.h>
#       include /**/ <fore_xti/xti_atm.h>
#       include /**/ <fore_xti/netatm/atm.h>
#       include /**/ <fore_xti/ans.h>
#     else
#       include /**/ <xti.h>
#     endif /* ACE_HAS_SYS_XTI_H */

      // If the xti.h file redefines the function names, do it now, else
      // when the function definitions are encountered, they won't match the
      // declaration here.
#     if defined (ACE_REDEFINES_XTI_FUNCTIONS)
#       include /**/ <xti.h>
#       if defined (UNIXWARE_2_0)         /* They apparently forgot one... */
           extern "C" int _xti_error(char *);
#       endif /* UNIXWARE_2_0 */
#     endif /* ACE_REDEFINES_XTI_FUNCTIONS */

      // The XTI API for obtaining local/peer addresses is t_getprotaddr().
      // The ACE API was developed in TLI days and so uses t_getname().
      // t_getname() has a type argument that specifies which address is
      // desired, local or peer, while t_getprotaddr() gets both at once.
      // t_getname() has values defined for the type, so these aren't defined
      // for XTI systems. So, define them here for ACE API users to use.
      // These values were taken from sys/tiuser.h on Solaris.
#     if !defined (LOCALNAME)
#       define LOCALNAME  0
#     endif
#     if !defined (REMOTENAME)
#       define REMOTENAME 1
#     endif

#   else /* !ACE_HAS_XTI, so this is a TLI system */
#     if !defined (ACE_HAS_TLI_PROTOTYPES)

// Define ACE_TLI headers for systems that don't prototype them....
extern "C"
{
  int t_accept(int fildes, int resfd, struct t_call *call);
  char *t_alloc(int fildes, int struct_type, int fields);
  int t_bind(int fildes, struct t_bind *req, struct t_bind *ret);
  int t_close(int fildes);
  int t_connect(int fildes, struct t_call *sndcall,
                struct t_call *rcvcall);
  void t_error(const char *errmsg);
  int t_free(char *ptr, int struct_type);
  int t_getinfo(int fildes, struct t_info *info);
  int t_getname (int fildes, struct netbuf *namep, int type);
  int t_getstate(int fildes);
  int t_listen(int fildes, struct t_call *call);
  int t_look(int fildes);
  int t_open(char *path, int oflag, struct t_info *info);
  int t_optmgmt(int fildes, struct t_optmgmt *req,
                struct t_optmgmt *ret);
  int t_rcv(int fildes, char *buf, unsigned int nbytes, int *flags);
  int t_rcvconnect(int fildes, struct t_call *call);
  int t_rcvdis(int fildes, struct t_discon *discon);
  int t_rcvrel(int fildes);
  int t_rcvudata(int fildes, struct t_unitdata *unitdata, int *flags);
  int t_rcvuderr(int fildes, struct t_uderr *uderr);
  int t_snd(int fildes, const char *buf, unsigned int nbytes, int flags);
  int t_snddis(int fildes, struct t_call *call);
  int t_sndrel(int fildes);
  int t_sndudata(int fildes, struct t_unitdata *unitdata);
  int t_sync(int fildes);
  int t_unbind(int fildes);
}
#     endif /* !ACE_HAS_TLI_PROTOTYPES */

#     if defined (ACE_HAS_TIUSER_H_BROKEN_EXTERN_C)
extern "C" {
#     endif /* ACE_HAS_TIUSER_H_BROKEN_EXTERN_C */
#     if defined (ACE_HAS_TIUSER_H)
#       include /**/ <tiuser.h>
#     else
#       /* What to do here??? Is there a tli.h? */
#     endif /* ACE_HAS_TIUSER_H */
#     if defined (ACE_HAS_TIUSER_H_BROKEN_EXTERN_C)
}
#     endif /* ACE_HAS_TIUSER_H_BROKEN_EXTERN_C */

#     if defined (ACE_HAS_SVR4_TLI)
         // t_getname is a TLI extension added by some platforms before XTI
         // was widely available. However, it's not often in the system's
         // header files. Sun OS, for example, is like this.
         extern "C" int t_getname (int, struct netbuf *, int);
#     endif /* ACE_HAS_SVR4_TLI */
#   endif /* !ACE_HAS_XTI */

# endif /* ACE_HAS_XTI || ACE_HAS_TLI */

/**
 * @namespace ACE_OS
 *
 * @brief This class is a wrapper for the XTI/TLI operations
 *
 */
namespace ACE_OS
{
  // = A set of wrappers for XTI/TLI.
  ACE_NAMESPACE_INLINE_FUNCTION
  int t_accept (ACE_HANDLE fildes,
                ACE_HANDLE resfd,
                struct t_call
                *call);

  ACE_NAMESPACE_INLINE_FUNCTION
  char *t_alloc (ACE_HANDLE fildes,
                 int struct_type,
                 int
                 fields);

  ACE_NAMESPACE_INLINE_FUNCTION
  int t_bind (ACE_HANDLE fildes,
              struct t_bind *req,
              struct t_bind *ret);

  ACE_NAMESPACE_INLINE_FUNCTION
  int t_close (ACE_HANDLE fildes);

  ACE_NAMESPACE_INLINE_FUNCTION
  int t_connect (ACE_HANDLE fildes,
                 struct t_call *sndcall,
                 struct t_call *rcvcall);

  ACE_NAMESPACE_INLINE_FUNCTION
  void t_error (const char *errmsg);

  ACE_NAMESPACE_INLINE_FUNCTION
  int t_free (char *ptr,
              int struct_type);

  ACE_NAMESPACE_INLINE_FUNCTION
  int t_getinfo (ACE_HANDLE fildes,
                 struct t_info *info);

  ACE_NAMESPACE_INLINE_FUNCTION
  int t_getname (ACE_HANDLE fildes,
                 struct netbuf *namep,
                 int type);

  ACE_NAMESPACE_INLINE_FUNCTION
  int t_getstate (ACE_HANDLE fildes);

  ACE_NAMESPACE_INLINE_FUNCTION
  int t_listen (ACE_HANDLE fildes,
                struct t_call *call);

  ACE_NAMESPACE_INLINE_FUNCTION
  int t_look (ACE_HANDLE fildes);

  ACE_NAMESPACE_INLINE_FUNCTION
  ACE_HANDLE t_open (char *path,
                     int oflag,
                     struct t_info *info);

  ACE_NAMESPACE_INLINE_FUNCTION
  int t_optmgmt (ACE_HANDLE handle,
                 struct t_optmgmt *req,
                 struct t_optmgmt *ret);

  ACE_NAMESPACE_INLINE_FUNCTION
  int t_rcv (ACE_HANDLE fildes,
             char *buf,
             unsigned int nbytes,
             int *flags);

  ACE_NAMESPACE_INLINE_FUNCTION
  int t_rcvdis (ACE_HANDLE fildes,
                struct t_discon *discon);

  ACE_NAMESPACE_INLINE_FUNCTION
  int t_rcvrel (ACE_HANDLE fildes);

  ACE_NAMESPACE_INLINE_FUNCTION
  int t_rcvudata (ACE_HANDLE fildes,
                  struct t_unitdata *unitdata,
                  int *flags);

  ACE_NAMESPACE_INLINE_FUNCTION
  int t_rcvuderr (ACE_HANDLE fildes,
                  struct t_uderr *uderr);

  ACE_NAMESPACE_INLINE_FUNCTION
  int t_snd (ACE_HANDLE fildes,
             const char *buf,
             unsigned int nbytes,
             int flags);

  ACE_NAMESPACE_INLINE_FUNCTION
  int t_snddis (ACE_HANDLE fildes,
                struct t_call *call);

  ACE_NAMESPACE_INLINE_FUNCTION
  int t_sndrel (ACE_HANDLE fildes);

  ACE_NAMESPACE_INLINE_FUNCTION
  int t_sync (ACE_HANDLE fildes);

  ACE_NAMESPACE_INLINE_FUNCTION
  int t_unbind (ACE_HANDLE fildes);

} /* namespace ACE_OS */

# if defined (ACE_HAS_INLINED_OSCALLS)
#   if defined (ACE_INLINE)
#     undef ACE_INLINE
#   endif /* ACE_INLINE */
#   define ACE_INLINE inline
#   include "ace/OS_TLI.inl"
# endif /* ACE_HAS_INLINED_OSCALLS */

#include /**/ "ace/post.h"
#endif /* ACE_OS_TLI_H */
