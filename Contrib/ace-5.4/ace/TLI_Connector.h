/* -*- C++ -*- */

//=============================================================================
/**
 *  @file    TLI_Connector.h
 *
 *  TLI_Connector.h,v 4.16 2003/07/19 19:04:14 dhinton Exp
 *
 *  @author Doug Schmidt
 */
//=============================================================================


#ifndef ACE_TLI_CONNECTOR_H
#define ACE_TLI_CONNECTOR_H
#include /**/ "ace/pre.h"

#include "ace/TLI_Stream.h"
#include "ace/Log_Msg.h"

#if !defined (ACE_LACKS_PRAGMA_ONCE)
# pragma once
#endif /* ACE_LACKS_PRAGMA_ONCE */

#if defined (ACE_HAS_TLI)

/**
 * @class ACE_TLI_Connector
 *
 * @brief Defines an active connection factory for the ACE_TLI C++
 * wrappers.
 */
class ACE_Export ACE_TLI_Connector
{
public:
  // = Initialization methods.
  /// Default constructor.
  ACE_TLI_Connector (void);

  /**
   * Actively connect and produce a <new_stream> if things go well.
   * The <remote_sap> is the address that we are trying to connect
   * with.  The <timeout> is the amount of time to wait to connect.
   * If it's 0 then we block indefinitely.  If *timeout == {0, 0} then
   * the connection is done using non-blocking mode.  In this case, if
   * the connection can't be made immediately the value of -1 is
   * returned with <errno == EWOULDBLOCK>.  If *timeout > {0, 0} then
   * this is the maximum amount of time to wait before timing out.  If the
   * time expires before the connection is made <errno == ETIME>.  The
   * <local_sap> is the value of local address to bind to.  If it's
   * the default value of <ACE_Addr::sap_any> then the user is letting
   * the OS do the binding.  If <reuse_addr> == 1 then the
   * <local_addr> is reused, even if it hasn't been cleanedup yet.
   */
  ACE_TLI_Connector (ACE_TLI_Stream &new_stream,
                     const ACE_Addr &remote_sap,
                     ACE_Time_Value *timeout = 0,
                     const ACE_Addr &local_sap = ACE_Addr::sap_any,
                     int reuse_addr = 0,
                     int flags = O_RDWR,
                     int perms = 0,
                     const char device[] = ACE_TLI_TCP_DEVICE,
                     struct t_info *info = 0,
                     int rw_flag = 1,
                     struct netbuf *udata = 0,
                     struct netbuf *opt = 0);

  /**
   * Actively connect and produce a <new_stream> if things go well.
   * The <remote_sap> is the address that we are trying to connect
   * with.  The <timeout> is the amount of time to wait to connect.
   * If it's 0 then we block indefinitely.  If *timeout == {0, 0} then
   * the connection is done using non-blocking mode.  In this case, if
   * the connection can't be made immediately the value of -1 is
   * returned with <errno == EWOULDBLOCK>.  If *timeout > {0, 0} then
   * this is the maximum amount of time to wait before timing out.  If the
   * time expires before the connection is made <errno == ETIME>.  The
   * <local_sap> is the value of local address to bind to.  If it's
   * the default value of <ACE_Addr::sap_any> then the user is letting
   * the OS do the binding.  If <reuse_addr> == 1 then the
   * <local_addr> is reused, even if it hasn't been cleanedup yet.
   */
  int connect (ACE_TLI_Stream &new_stream,
               const ACE_Addr &remote_sap,
               ACE_Time_Value *timeout = 0,
               const ACE_Addr &local_sap = ACE_Addr::sap_any,
               int reuse_addr = 0,
               int flags = O_RDWR,
               int perms = 0,
               const char device[] = ACE_TLI_TCP_DEVICE,
               struct t_info *info = 0,
               int rw_flag = 1,
               struct netbuf *udata = 0,
               struct netbuf *opt = 0);

  /**
   * Try to complete a non-blocking connection.
   * If connection completion is successful then <new_stream> contains
   * the connected ACE_SOCK_Stream.  If <remote_sap> is non-NULL then it
   * will contain the address of the connected peer.
   */
  int complete (ACE_TLI_Stream &new_stream,
                ACE_Addr *remote_sap,
                ACE_Time_Value *tv);

  /// Resets any event associations on this handle
  int reset_new_handle (ACE_HANDLE handle);

  // = Meta-type info
  typedef ACE_INET_Addr PEER_ADDR;
  typedef ACE_TLI_Stream PEER_STREAM;

  /// Dump the state of an object.
  void dump (void) const;

  /// Declare the dynamic allocation hooks.
  ACE_ALLOC_HOOK_DECLARE;
};

#if defined (__ACE_INLINE__)
#include "ace/TLI_Connector.i"
#endif /* __ACE_INLINE__ */

#endif /* ACE_HAS_TLI */
#include /**/ "ace/post.h"
#endif /* ACE_TLI_CONNECTOR_H */
