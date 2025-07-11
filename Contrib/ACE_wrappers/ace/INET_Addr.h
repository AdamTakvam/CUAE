/* -*- C++ -*- */

//=============================================================================
/**
 *  @file    INET_Addr.h
 *
 *  INET_Addr.h,v 4.39 2002/05/02 04:08:13 ossama Exp
 *
 *  @author Douglas C. Schmidt <schmidt@cs.wustl.edu>
 */
//=============================================================================

#ifndef ACE_INET_ADDR_H
#define ACE_INET_ADDR_H
#include "ace/pre.h"

#include "ace/Sock_Connect.h"

#if !defined (ACE_LACKS_PRAGMA_ONCE)
# pragma once
#endif /* ACE_LACKS_PRAGMA_ONCE */

#include "ace/Addr.h"

#if defined(VXWORKS)
#  include /**/ "inetLib.h"
#endif /* VXWORKS */

/**
 * @class ACE_INET_Addr
 *
 * @brief Defines a C++ wrapper facade for the Internet domain address
 * family format.
 */
class ACE_Export ACE_INET_Addr : public ACE_Addr
{
public:
  // = Initialization methods.

  /// Default constructor.
  ACE_INET_Addr (void);

  /// Copy constructor.
  ACE_INET_Addr (const ACE_INET_Addr &);

  /// Creates an <ACE_INET_Addr> from a sockaddr_in structure.
  ACE_INET_Addr (const sockaddr_in *, int len);

  /// Creates an <ACE_INET_Addr> from a <port_number> and the remote
  /// <host_name>. The port number is assumed to be in host byte order.
  /// To set a port already in network byte order, please @see set().
  /// Use address_family to select IPv6 (PF_INET6) vs. IPv4 (PF_INET).
  ACE_INET_Addr (u_short port_number,
                 const char host_name[],
                 int address_family = AF_UNSPEC);

  /**
   * Initializes an <ACE_INET_Addr> from the <address>, which can be
   * "ip-number:port-number" (e.g., "tango.cs.wustl.edu:1234" or
   * "128.252.166.57:1234").  If there is no ':' in the <address> it
   * is assumed to be a port number, with the IP address being
   * INADDR_ANY.
   */
  ACE_EXPLICIT ACE_INET_Addr (const char address[]);

  /**
   * Creates an <ACE_INET_Addr> from a <port_number> and an Internet
   * <ip_addr>.  This method assumes that <port_number> and <ip_addr>
   * are in host byte order. If you have addressing information in
   * network byte order, @see set().
   */
  ACE_INET_Addr (u_short port_number,
                 ACE_UINT32 ip_addr = INADDR_ANY);

  /// Uses <getservbyname> to create an <ACE_INET_Addr> from a
  /// <port_name>, the remote <host_name>, and the <protocol>.
  ACE_INET_Addr (const char port_name[],
                 const char host_name[],
                 const char protocol[] = "tcp");

  /**
   * Uses <getservbyname> to create an <ACE_INET_Addr> from a
   * <port_name>, an Internet <ip_addr>, and the <protocol>.  This
   * method assumes that <ip_addr> is in host byte order.
   */
  ACE_INET_Addr (const char port_name[],
                 ACE_UINT32 ip_addr,
                 const char protocol[] = "tcp");

#if defined (ACE_HAS_WCHAR)
  ACE_INET_Addr (u_short port_number,
                 const wchar_t host_name[],
                 int address_family = AF_UNSPEC);

  ACE_EXPLICIT ACE_INET_Addr (const wchar_t address[]);

  ACE_INET_Addr (const wchar_t port_name[],
                 const wchar_t host_name[],
                 const wchar_t protocol[] = ACE_TEXT_WIDE ("tcp"));

  ACE_INET_Addr (const wchar_t port_name[],
                 ACE_UINT32 ip_addr,
                 const wchar_t protocol[] = ACE_TEXT_WIDE ("tcp"));
#endif /* ACE_HAS_WCHAR */

  /// Default dtor.
  ~ACE_INET_Addr (void);

  // = Direct initialization methods.

  // These methods are useful after the object has been constructed.

  /// Initializes from another <ACE_INET_Addr>.
  int set (const ACE_INET_Addr &);

  /**
   * Initializes an <ACE_INET_Addr> from a <port_number> and the
   * remote <host_name>.  If <encode> is non-zero then <port_number> is
   * converted into network byte order, otherwise it is assumed to be
   * in network byte order already and are passed straight through.
   * address_family can be used to select IPv4/IPv6 if the OS has
   * IPv6 capability (ACE_HAS_IPV6 is defined). To specify IPv6, use
   * the value AF_INET6. To specify IPv4, use AF_INET.
   */
  int set (u_short port_number,
           const char host_name[],
           int encode = 1,
           int address_family = AF_UNSPEC);

  /**
   * Initializes an <ACE_INET_Addr> from a @param port_number and an
   * Internet @param ip_addr.  If @param encode is non-zero then the
   * port number and IP address are converted into network byte order,
   * otherwise they are assumed to be in network byte order already and
   * are passed straight through.
   */
  int set (u_short port_number,
           ACE_UINT32 ip_addr = INADDR_ANY,
           int encode = 1);

  /// Uses <getservbyname> to initialize an <ACE_INET_Addr> from a
  /// <port_name>, the remote <host_name>, and the <protocol>.
  int set (const char port_name[],
           const char host_name[],
           const char protocol[] = "tcp");

  /**
   * Uses <getservbyname> to initialize an <ACE_INET_Addr> from a
   * <port_name>, an <ip_addr>, and the <protocol>.  This assumes that
   * <ip_addr> is already in network byte order.
   */
  int set (const char port_name[],
           ACE_UINT32 ip_addr,
           const char protocol[] = "tcp");

  /**
   * Initializes an <ACE_INET_Addr> from the <addr>, which can be
   * "ip-number:port-number" (e.g., "tango.cs.wustl.edu:1234" or
   * "128.252.166.57:1234").  If there is no ':' in the <address> it
   * is assumed to be a port number, with the IP address being
   * INADDR_ANY.
   */
  int set (const char addr[]);

  /// Creates an <ACE_INET_Addr> from a sockaddr_in structure.
  int set (const sockaddr_in *,
           int len);

#if defined (ACE_HAS_WCHAR)
  int set (u_short port_number,
           const wchar_t host_name[],
           int encode = 1,
           int address_family = AF_UNSPEC);

  int set (const wchar_t port_name[],
           const wchar_t host_name[],
           const wchar_t protocol[] = ACE_TEXT_WIDE ("tcp"));

  int set (const wchar_t port_name[],
           ACE_UINT32 ip_addr,
           const wchar_t protocol[] = ACE_TEXT_WIDE ("tcp"));

  int set (const wchar_t addr[]);
#endif /* ACE_HAS_WCHAR */

  /// Return a pointer to the underlying network address.
  virtual void *get_addr (void) const;
  int get_addr_size(void) const;

  /// Set a pointer to the address.
  virtual void set_addr (void *, int len);

  /**
   * Transform the current <ACE_INET_Addr> address into string format.
   * If <ipaddr_format> is non-0 this produces "ip-number:port-number"
   * (e.g., "128.252.166.57:1234"), whereas if <ipaddr_format> is 0
   * this produces "ip-name:port-number" (e.g.,
   * "tango.cs.wustl.edu:1234").  Returns -1 if the <size> of the
   * <buffer> is too small, else 0.
   */
  virtual int addr_to_string (ACE_TCHAR buffer[],
                              size_t size,
                              int ipaddr_format = 1) const;

  /**
   * Initializes an <ACE_INET_Addr> from the <address>, which can be
   * "ip-addr:port-number" (e.g., "tango.cs.wustl.edu:1234"),
   * "ip-addr:port-name" (e.g., "tango.cs.wustl.edu:telnet"),
   * "ip-number:port-number" (e.g., "128.252.166.57:1234"), or
   * "ip-number:port-name" (e.g., "128.252.166.57:telnet").  If there
   * is no ':' in the <address> it is assumed to be a port number,
   * with the IP address being INADDR_ANY.
   */
  virtual int string_to_addr (const char address[]);

#if defined (ACE_HAS_WCHAR)
  /*
  virtual int string_to_addr (const char address[]);
  */
#endif /* ACE_HAS_WCHAR */

  /**
   * Sets the port number without affecting the host name.  If
   * <encode> is enabled then <port_number> is converted into network
   * byte order, otherwise it is assumed to be in network byte order
   * already and are passed straight through.
   */
  void set_port_number (u_short,
                        int encode = 1);

  /**
   * Sets the address without affecting the port number.  If
   * <encode> is enabled then <ip_addr> is converted into network
   * byte order, otherwise it is assumed to be in network byte order
   * already and are passed straight through.  The size of the address
   * is specified in the <len> parameter.
   */
  int set_address (const char *ip_addr,
                   int len,
                   int encode = 1);

  /// Return the port number, converting it into host byte-order.
  u_short get_port_number (void) const;

  /**
   * Return the character representation of the name of the host,
   * storing it in the <hostname> (which is assumed to be
   * <hostnamelen> bytes long).  This version is reentrant.  If
   * <hostnamelen> is greater than 0 then <hostname> will be
   * NUL-terminated even if -1 is returned. 
   */
  int get_host_name (char hostname[],
                     size_t hostnamelen) const;

#if defined (ACE_HAS_WCHAR)
  int get_host_name (wchar_t hostname[],
                     size_t hostnamelen) const;
#endif /* ACE_HAS_WCHAR */

  /**
   * Return the character representation of the hostname (this version
   * is non-reentrant since it returns a pointer to a static data
   * area).
   */
  const char *get_host_name (void) const;

  /// Return the "dotted decimal" Internet address.
  const char *get_host_addr (void) const;
  const char *get_host_addr (char *dst, int size) const;

  /// Return the 4-byte IP address, converting it into host byte
  /// order.
  ACE_UINT32 get_ip_address (void) const;

  /**
   * Returns true if <this> is less than <rhs>.  In this context,
   * "less than" is defined in terms of IP address and TCP port
   * number.  This operator makes it possible to use <ACE_INET_Addr>s
   * in STL maps.
   */
  int operator < (const ACE_INET_Addr &rhs) const;

  /// Compare two addresses for equality.  The addresses are considered
  /// equal if they contain the same IP address and port number.
  int operator == (const ACE_INET_Addr &SAP) const;

  /// Compare two addresses for inequality.
  int operator != (const ACE_INET_Addr &SAP) const;

  /// Computes and returns hash value.
  virtual u_long hash (void) const;

  /// Dump the state of an object.
  void dump (void) const;

  /// Declare the dynamic allocation hooks.
  ACE_ALLOC_HOOK_DECLARE;

private:
  /// Insure that @arg hostname is properly null-terminated.
  int get_host_name_i (char hostname[], size_t hostnamelen) const;

  // Methods to gain access to the actual address of
  // the underlying internet address structure.
  void *ip_addr_pointer (void) const;
  size_t ip_addr_size (void) const;
  int determine_type (void) const;

  /// Underlying representation.
  /// This union uses the knowledge that the two structures share the
  /// first member, sa_family (as all sockaddr structures do).
  union
  {
    sockaddr_in  in4_;
#if defined (ACE_HAS_IPV6)
    sockaddr_in6 in6_;
#endif /* ACE_HAS_IPV6 */
  } inet_addr_;

#if defined (VXWORKS)
  char buf_[INET_ADDR_LEN];
#endif
};

#if defined (__ACE_INLINE__)
#include "ace/INET_Addr.i"
#endif /* __ACE_INLINE__ */

#include "ace/post.h"
#endif /* ACE_INET_ADDR_H */
