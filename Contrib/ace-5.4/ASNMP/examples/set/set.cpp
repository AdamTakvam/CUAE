// set.cpp,v 1.7 2003/11/10 01:48:02 dhinton Exp

// ============================================================================
//
// = LIBRARY
//    asnmp
//
// = FILENAME
//    set.cpp
//
// = DESCRIPTION
//  Sample application demonstrating synchronous Snmp::set API  
//  to update an oid in an SNMP Version 1 agent.
//
// = AUTHOR
//  Peter E. Mellquist original code
//  Michael R MacFaden mrm@cisco.com rework API/ACE integration
//
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
  purpose. It is provided "AS-IS without warranty of any kind,either express
  or implied. User hereby grants a royalty-free license to any and all
  derivatives based upon this software code base.
=====================================================================*/

#include "asnmp/snmp.h"
#include "ace/Get_Opt.h"

// FUZZ: disable check_for_streams_include
#include "ace/streams.h"

ACE_RCSID(set, set, "set.cpp,v 1.7 2003/11/10 01:48:02 dhinton Exp")

//
// SNMPv1 Set Application
//
class set {
  public:
  set(int argc, char **argv); // process command line args
  int valid() const;             // verify transaction can proceed
  int run();                     //  issue transaction
  static void usage();           // operator help message

  private: 
  set(const set&);

  UdpAddress address_;
  Pdu pdu_;                                // construct a request Pdu
  Oid oid_;
  OctetStr community_;
  Snmp snmp_;
  UdpTarget target_;
  int valid_;
};


// main entry point 
int main( int argc, char *argv[])  
{
  set get(argc, argv);
  if (get.valid())
     return get.run();
  else
    set::usage();
  return 1;
}

int
set::valid() const 
{ 
 return valid_; 
}

set::set(int argc, char *argv[]): valid_(0)
{
   Vb vb;                                  // construct a Vb object
   Oid req;
   if ( argc < 2) 
     return; 
   target_.get_write_community(community_); 
   address_ = argv[argc - 1];
   if ( !address_.valid()) {
      cout << "ERROR: Invalid IPv4 address or DNS hostname: " \
     << argv[argc] << "\n";
      return;
   }

   ACE_Get_Opt get_opt (argc, argv, "o:c:r:t:I:U:C:G:T:O:S:P:");
   for (int c; (c = get_opt ()) != -1; )
     switch (c)
       {
       case 'o':
         req = get_opt.optarg;
         if (req.valid() == 0) 
         cout << "ERROR: oid value: " <<get_opt.optarg  \
              << "is not valid. using default.\n";
         break;

       case 'c':
         community_ = get_opt.optarg;
         target_.set_write_community(community_);
         break;

       case 'r':
         target_.set_retry(ACE_OS::atoi (get_opt.optarg));
         break;

       case 't':
         target_.set_timeout(ACE_OS::atoi (get_opt.optarg));
         break;

       case 'I': // Integer32
         {
         SnmpInt32 o(ACE_OS::atoi(get_opt.optarg)); 
         vb.set_value(o);
         pdu_ += vb;
         }
        break;

       case 'U': // Unsigned32
         {
         SnmpUInt32 o(ACE_OS::atoi(get_opt.optarg)); 
         vb.set_value(o);
         pdu_ += vb;
         }
        break;

       case 'C': // Counter32
         {
         Counter32 o(ACE_OS::atoi(get_opt.optarg)); 
         vb.set_value(o);
         pdu_ += vb;
         }
         break;

       case 'G': // Gauge32
        {
         Gauge32 o(ACE_OS::atoi(get_opt.optarg)); 
         vb.set_value(o);
         pdu_ += vb;
         }
        break;

       case 'T': // TimeTicks
        {
         TimeTicks o(ACE_OS::atoi(get_opt.optarg)); 
         vb.set_value(o);
         pdu_ += vb;
         }
        break;

       case 'O': // Oid as a variable identifier 
        {
         oid_ = get_opt.optarg; 
         vb.set_oid(oid_); // when value is set, pdu updated
         }
         break;

       case 'S': // Octet String
         {
         OctetStr o(get_opt.optarg); 
         vb.set_value(o);                    // set the Oid portion of the Vb
         pdu_ += vb;
         }
         break;

       case 'P': // Oid String as a value
         {
         Oid o(get_opt.optarg); 
         vb.set_value(o);                    // set the Oid portion of the Vb
         pdu_ += vb;
         }
         break;

       default:
         break;
       }

  // if user didn't set anything use defaults 
  if (pdu_.get_vb_count() == 0) {
   Oid def_oid("1.3.6.1.2.1.1.4.0");      // defualt is sysName
   OctetStr def_value("sysName.0 updated by ASNMP set command");
   vb.set_oid(def_oid);
   vb.set_value(def_value);
   pdu_ += vb;
   cout << "INFO: using defaults, setting sysName to : " <<  \
        def_value.to_string() << endl; 
  }

  valid_ = 1;
}

void set::usage()
{
  cout << "Usage:\n";
  cout << "next [options] dotted-quad | DNSName[:port]\n";
  cout << "      -o OID starts with oid after 1.3.6.1.2.1.1.1.0 (mibII sysDescr.0) \n";
  cout << "      -c Community_name, default is 'private' \n";
  cout << "      -r N  retries default is N = 1 retry\n";
  cout << "      -t N  timeout in seconds default is 1 second\n"; 
  cout << "      -O oid_to_set -{I,U,G,S,P} value\n";
  cout << "      where I=int32, U=uint32, G=gauge32, S=octet, P=oid" << endl;
}


int set::run()
{ 

   //----------[ create a ASNMP session ]-----------------------------------
   if ( snmp_.valid() != SNMP_CLASS_SUCCESS) {
      cout << "\nASNMP:ERROR:Create session failed: "<< 
          snmp_.error_string()<< "\n";
      return 1;
   }

   //--------[ build up ASNMP object needed ]-------------------------------
   if (address_.get_port() == 0)
     address_.set_port(DEF_AGENT_PORT);
   target_.set_address( address_);         // make a target using the address

   //-------[ issue the request, blocked mode ]-----------------------------
   cout << "\nASNMP:INFO:SNMP Version " << (target_.get_version()+ 1) << \
       " SET SAMPLE PROGRAM \nOID: " << oid_.to_string() << "\n";
   target_.get_address(address_); // target updates port used
   int rc;
   const char *name = address_.resolve_hostname(rc);

   cout << "Device: " << address_ << " ";
   cout << (rc ? "<< did not resolve via gethostbyname() >>" : name) << "\n"; 
   cout << "[ Retries=" << target_.get_retry() << " \
        Timeout=" << target_.get_timeout() <<" ms " << "Community=" << \
         community_.to_string() << " ]"<< endl;

   if (snmp_.set( pdu_, target_) == SNMP_CLASS_SUCCESS) {
       Vb vb;
      // check to see if there are any errors
      if (pdu_.get_error_status()) {
        cout << "ERROR: agent replied as follows\n"; 
        cout << pdu_.agent_error_reason() << endl; 
      }
      else {
        VbIter iter(pdu_);
        while (iter.next(vb)) {
 	  cout << "\tOid = " << vb.to_string_oid() << "\n";
 	  cout << "\tValue = " << vb.to_string_value() << "\n";
       }
     }
   }
   else {
    const char *ptr = snmp_.error_string();
    cout << "ASNMP:ERROR: set command failed reason: " << ptr << endl;
  }
  cout << "ASNMP:INFO:command completed normally.\n"<< endl;
  return 0;
} 

