/* -*-C++-*- */
// target.h,v 1.7 2003/08/26 19:16:37 dhinton Exp
#ifndef TARGET_
#define TARGET_
// ============================================================================
//
// = LIBRARY
//    asnmp
//
// = FILENAME
//    target.h
//
// = DESCRIPTION
//
// = AUTHOR
//    Peter E Mellquist
//    Michael R. MacFaden (ported to ACE)
// ============================================================================
/*===================================================================
  Copyright (c) 1996
  Hewlett-Packard Company

  ATTENTION: USE OF THIS SOFTWARE IS SUBJECT TO THE FOLLOWING TERMS.
  Permission to use, copy, modify, distribute and/or sell this software
  and/or its documentation is hereby granted without fee. User agrees
  to display the above copyright notice and this license notice in all
  copies of the software and any documentation of the software. User
  agrees to assume all liability for the use of the software; Hewlett-Packard
  makes no representations about the suitability of this software for any
  purpose. It is provided "AS-IS" without warranty of any kind,either express
  or implied. User hereby grants a royalty-free license to any and all
  derivatives based upon this software code base.
=====================================================================*/

//----[ includes ]------------------------------------------------
#include "ace/INET_Addr.h"

#if !defined (ACE_LACKS_PRAGMA_ONCE)
# pragma once
#endif /* ACE_LACKS_PRAGMA_ONCE */

#include "asnmp/address.h"
#include "asnmp/oid.h"
#include "asnmp/octet.h"

//----[ enumerated types for SNMP versions ]-------------------------
enum snmp_version {
   version1,       // 0
   version2c,       // 1 (Cisco IOS does not have IOS v2c agent available yet)
   version3        // 2 (IETF working on this version)
};

// targets contain the following default properties
enum ASNMP_Defs {
  DEF_TIMEOUT = 1,              // unit: seconds
  DEF_RETRIES = 1,              // no retry default
  DEF_MAX_SNMP_PACKET = 1430,   // maximum pdu len on the wire (eth mtu-ip hdr)
                                // split bigger pdus (not implemented yet)
  DEF_AGENT_PORT = 161,         // port # for SNMP agent
  DEF_TRAP_PORT = 162,          // port # for SNMP trap receiver
  DEF_VERSION = version1,       // default SNMP version
  MAX_COMM_STR_LEN = 255,       // max value this impl will accept from client
  MAX_TARGET_STRING_REP = 2048  // max value this impl will accept from client
};

#define READ_COMM_STR "public"
#define WRITE_COMM_STR "private"

// Abstract class used to provide a virtual interface into Targets
//
class ASNMP_Export SnmpTarget
  // = TITLE
  //      Abstract Base class SnmpTarget is used to Collect all transmission
  //      details to communicate with an SNMP
{
 public:

    virtual ~SnmpTarget();
    // allow destruction of derived classes

    int valid() const;
    // return validity of target

    void set_retry( const int r);
    // set the retry value

    void set_timeout( const unsigned long t);
    // set the object's timeout (in seconds)

    void set_version( const snmp_version v);
    // set the instance version

    void set_max_pdu_size(const unsigned long max_pdu_sz);
    // all classes constructed will have this write community string

    void set_default_timeout( const unsigned long t);
    // change the "class" default default timeout (in seconds)

    void set_default_retry( const int r);
    // change the default send retries

    void set_default_max_pdu_size(const unsigned long max_pdu_sz);
    // all classes constructed will have this write community string

    void set_default_version( const snmp_version v);
    // change class default

    // *** get methods ***

    int get_default_retry() const;
    // get the retry value

    unsigned long get_timeout() const;
    // get the timeout (seconds)

    unsigned long get_max_pdu_size() const;
    // get instance max buffer size

    void get_default_max_pdu_size(const unsigned long max_pdu_sz);
    // all classes constructed will have this write community string

    snmp_version get_version() const;
    // get the version

    snmp_version get_default_version() const;

    int get_retry() const;
    // return send retry number for this instancd

    virtual SnmpTarget *clone() const = 0;
    // virtual clone operation for creating a new SnmpTarget from an existing
    // SnmpTarget.  The caller MUST use the delete operation on the return
    // value when done.

   friend int operator==(const SnmpTarget& lhs, const SnmpTarget& rhs);
   // manipulate the base part

   SnmpTarget& operator=(const SnmpTarget& lhs);

   protected:
     SnmpTarget();
     //     SnmpTarget(const SnmpTarget &);

    int validity_;
    // used by derived class instances

    unsigned long timeout_;
    // instance value xmit timeout in milli secs

    int retries_;
    // instance value number of retries

    unsigned max_pdu_size_;
    // size of pdu

    snmp_version version_;
    // instance value the snmp version

    // class wide default values
    static unsigned long default_timeout_;
    // xmit timeout in secs

    static int default_retries_;
    // number of retries

    static unsigned long default_max_pdu_size_;
    static snmp_version default_version_;
    // snmp protocol version
};

//----[  UdpTarget class ]----------------------------------------------
// UDP/IP transport using "community string" based agents  (targets)
//
class ASNMP_Export UdpTarget: public SnmpTarget
  // = TITLE
  //      Concrete class UdpTarget contains all Details for communicating
  //      with a SNMPv1 agent over UDP/IPv4 transport
{
  public:
    UdpTarget();

    UdpTarget( const UdpAddress& udp);
    // constructor with only address
    // assumes default as public, public
    // can be constructed with IP address object

    UdpTarget( ACE_INET_Addr& ace_inet_addr);
    // can be constructed with Udp address object TODO: merge addresses

    ~UdpTarget();
    // destructor

    // ** set ***

    void set_read_community( const OctetStr& new_read_community);
    // set the read community using an OctetStr

    void set_write_community( const OctetStr& write_community);
    // set the write community using an OctetStr

    int set_address( UdpAddress &udp_address);
    // set the address

    // ** get ***

    void get_read_community( OctetStr& read_community_oct) const;
    // get the read community as an Octet Str object

    void get_write_community( OctetStr &write_community_oct) const;
    // get the write community as an OctetStr

    void get_address( UdpAddress& address) const;
    // get the address

    void set_default_read_community(const OctetStr& rd_community);
    // all classes constructed will have this read community string

    void set_default_write_community(const OctetStr& wr_community);
    // all classes constructed will have this write community string

    void get_default_read_community(OctetStr& rd_community) const;
    // all classes constructed will have this read community string

    void get_default_write_community(OctetStr& wr_community) const;
    // all classes constructed will have this write community string

    UdpTarget& operator=( const UdpTarget& target);
    // overloaded assignment

    friend int operator==( const UdpTarget &lhs, const UdpTarget &rhs);
    // compare two C targets

    const char *to_string();
    // string representation of object

    SnmpTarget *clone() const;
    // clone from existing UdpTarget

  protected:
    static OctetStr default_rd_community_;
    static OctetStr default_wr_community_;
    OctetStr read_community_;
    OctetStr write_community_;
    UdpAddress udp_address_;
    snmp_version version_;

    char output_buffer_[MAX_TARGET_STRING_REP];
    // to_string() rep of data
};


#endif //TARGET_
