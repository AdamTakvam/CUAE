// get.cpp,v 1.9 2003/11/10 01:48:02 dhinton Exp

// ============================================================================
//
// = LIBRARY
//    asnmp
//
// = FILENAME
//    get.cpp
//
// = DESCRIPTION
//  Sample application demonstrating synchronous Snmp::get API  
//  to access an SNMP Version 1 agent.
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

ACE_RCSID(get, get, "get.cpp,v 1.9 2003/11/10 01:48:02 dhinton Exp")

//
// SNMPv1 Get Application
//
class getapp {
  public:
  getapp(int argc, char **argv); // process command line args
  int valid() const;             // verify transaction can proceed
  int run();                     //  issue transaction
  static void usage();           // operator help message

  private: 
  getapp(const getapp&);

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
  getapp get(argc, argv);
  if (get.valid())
     return get.run();
  else
    getapp::usage();
  return 1;
}

int getapp::valid() const 
{ 
 return valid_; 
}
getapp::getapp(int argc, char *argv[]): valid_(0)
{
   Oid req, def_oid("1.3.6.1.2.1.1.1.0");      // default is sysDescr
   if ( argc < 2) 
     return; 
    
   address_ = argv[argc - 1];
   if ( !address_.valid()) {
      cout << "ERROR: Invalid IPv4 address or DNS hostname: " \
     << argv[argc] << "\n";
      return;
   }

   ACE_Get_Opt get_opt (argc, argv, "o:c:r:t:p:");
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
         target_.set_read_community(community_);
         break;

       case 'r':
         target_.set_retry(ACE_OS::atoi (get_opt.optarg));
         break;

       case 't':
         target_.set_timeout(ACE_OS::atoi (get_opt.optarg));
         break;

       default:
         break;
       }

  Vb vb;                                  // construct a Vb object
  if (req.valid()) 
     vb.set_oid( req);                    // set the Oid portion of the Vb
  else { 
     vb.set_oid( def_oid);               // set the Oid portion of the Vb
  }
  pdu_ += vb;
  vb.get_oid(oid_); // store for later use
  valid_ = 1;
}

void getapp::usage()
{
  cout << "Usage:\n";
  cout << "get [options] dotted-quad | DNSName[:port]\n";
  cout << "      -o OID defaults to 1.3.6.1.2.1.1.1.0 (mibII sysDescr.0) \n";
  cout << "      -c Community_name, default is 'public' \n";
  cout << "      -r N  retries default is N = 1 retry\n";
  cout << "      -t N  timeout in seconds default is 1 second" << endl;
}


int getapp::run()
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
       " GET SAMPLE PROGRAM \nOID: " << oid_.to_string() << "\n";
   target_.get_address(address_); // target updates port used
   int rc;
   const char *name = address_.resolve_hostname(rc);

   cout << "Device: " << address_ << " ";
   cout << (rc ? "<< did not resolve via gethostbyname() >>" : name) << "\n"; 
   cout << "[ Retries=" << target_.get_retry() << " \
        Timeout=" << target_.get_timeout() <<" ms " << "Community=" << \
         community_.to_string() << " ]"<< endl;

   if (snmp_.get( pdu_, target_) == SNMP_CLASS_SUCCESS) {
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
    cout << "ASNMP:ERROR: get command failed reason: " << ptr << endl;
  }

  cout << "\nASNMP:INFO: command completed normally.\n"<< endl;
  return 0;
} 

