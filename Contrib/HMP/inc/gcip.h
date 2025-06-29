/**********************************************************************
*  Copyright (C) 2000-2006 Intel Corporation.
*  All Rights Reserved
*
*  All names, products, and services mentioned herein are the 
*  trademarks or registered trademarks of their respective 
*  organizations and are the sole property of their respective owners.
**********************************************************************/
#ifndef _gc_1_gcip_h_H
#define _gc_1_gcip_h_H

#ifndef lint
static 
#ifdef __cplusplus          
const             /* C++ needs const */     
#endif
char    *_gc_1_gcip_h = "@(#) %filespec: gcip.h~igl#35 %  (%full_filespec: gcip.h~igl#35:incl:gc#2 %)";
#endif

#include "gcip_defs.h"
#include "gclib.h"
#include <memory.h>

/* Enable the #pragma pack(1) for WIN32 and UNIX */
#pragma pack(1)


/* IP_AUDIO_CAPABILITY : This structure is used to allow some minimum set of information
 * to be exchanged along with the audio codec identifier.
 * 
 * frames_per_pkt : When bundling more than one audio frame into a single transport packet, 
 * this value should represent the maximum number of frames per packet which will 
 * be sent on the wire. If set to zero, it indicates either that the exact number 
 * of frames per packet is not known which means it could be anything or it indicates
 * that this data is not applicable.
 *
 * VAD : For audio algorithms that support the concept of voice activity detection (VAD, 
 * also known as silence suppression), this value will be either GCPV_ENABLE or 
 * GCPV_DISABLE.  This parameter is ignored.for algorithms which do not support VAD. 
 * See product documentation for details.
 */
typedef struct
{

    unsigned long   frames_per_pkt;
    unsigned short  VAD;
} IP_AUDIO_CAPABILITY;



/* IP_VIDEO_CAPABILITY | This structure is used to allow some minimum set of information
 * to be exchanged along with the video codec identifier.
 *
 * mean_pict_intrvl : This field is used to indicate the maximum frame rate for 
 * the video stream. The frame rate maps to MPI through the following formula:  
 * fps = 29.97/MPI.
 */
typedef struct
{
    unsigned long   mean_pict_intrvl;
} IP_VIDEO_CAPABILITY;



/* IP_DATA_CAPABILITY | This structure is used to allow some minimum set of information
 * to be exchanged along with the data capability.
 *
 * max_bit_rate : This field is used to indicate the maximum bit rate which should be 
 * used for the data channel.  The bit rate/ should be specified in 100's of bit/sec.
 */
typedef struct
{
    unsigned long   max_bit_rate;
} IP_DATA_CAPABILITY;



/* IP_CAPABILITY_UNION | This union simply allows a way for the different capability
 * categories to define their own additional parameters of interest.
 *
 * audio : This is a structure which represents audio capability
 *
 * video : This is a structure which represents video capability.
 *
 * data : This is a structure which represents data capability.
 */
typedef union
{
    IP_AUDIO_CAPABILITY         audio;
    IP_VIDEO_CAPABILITY         video;
    IP_DATA_CAPABILITY          data;
} IP_CAPABILITY_UNION;



/* IP_CAPABILITY | This structure is intended to be a very simplified capability 
 * representation. Obviously it will not contain all the flexibility of the H.245 
 * terminal capability structure, but hopefully it will provide what might be 
 * classified as the first level of useful information beyond simply the capability 
 * or codec identifier.
 *
 * capability : This is the media capability for this structure.
 *
 * type : This is the capability category of capability specified in this structure.  
 * It indicates which member of the union is being used.
 *
 * direction : This is the direction for this capability. 
 *
 * payload_type : This field only applies to dynamic payload types.  
 *                It is ignored for well-known static payload type transcoding.
 *
 * extra : The contents of this IP_CAPABILITY_UNION will be indicated by the 
 * type field. 
 * 
 * rfu   : Reserved for future use. Must be set to zero when not used.
 *
 *
 */
typedef struct
{
    int                     capability;
    int                     type;
    int                     direction;
    int                     payload_type;
    IP_CAPABILITY_UNION     extra;
    char                    rfu[0x10]; 
} IP_CAPABILITY;



/* IP_MEDIA_STREAM_INFO | This structure provides the RTP and RTCP Port information along 
 * with information about the Capability properties that follow. 
 *
 * RTPAddress : This is the RTP IP address 
 *
 * RTPPort : This is the RTP port where media streaming takes place
 *
 * RTCPAddress : This is the RTCP IP address where RTCP receive reports are received. 
 * That is the receiver of the stream being transmitted will send reports regarding 
 * the RTP packets it is receiving. For a receive channel, this will be the IP address 
 * and port where the RTCP send reports are received. That is the transmitter of the 
 * stream will send reports regarding the RTP packets it is sending.
 *
 * RTCPPort : This is the RTCP port where streaming reports are received/sent.
 *
 */


typedef struct 
{
    char                RTPAddress[MAX_ADDRESS_LEN];
    int                 RTPPort;
    char                RTCPAddress[MAX_ADDRESS_LEN];
    int                 RTCPPort;
    char                rfu[3];
} IP_MEDIA_STREAM_INFO, *IP_MEDIA_STREAM_INFOP;


/* RTCP_SENDERREPORT | This structure provides RTCP sender information
 * 
 * timestamp:  Timestamp of the sender report. 
 *
 * tx_packets: The total number of RTP data packets transmitted by the
 *             sender since starting transmission up until the time 
 *             this SR packet was generated.
 *
 * tx_octets:  The total number of payload octets (i.e., not including 
 *             header or padding) transmitted in RTP data packets by 
 *             the sender since starting transmission up until the time
 *             this SR packet was generated.
 *
 * send_indication:  Boolean value as to whether the remote sender report
 *             has changed since the last transmission.
 */
typedef struct
{
    unsigned int  timestamp;
    unsigned int  tx_packets;
    unsigned int  tx_octets;
    unsigned int  send_indication;
} RTCP_SENDERREPORT;


/* RTCP_RECEIVERREPORT | This structure provides RTCP receiver information
 * 
 * fraction_lost:   The fraction of RTP data packets from source lost since the
 *                  previous SR or RR packet was sent. Expressed as a fixed
 *                  point number with the binary point at the left edge of the 
 *                  field, or put another way, equivalent to taking the integer
 *                  part after multiplying the loss fraction by 256.
 *
 * cumulative_lost: The total number of RTP data packets from source that have
 *                  been lost since the beginning of reception. Defined to be 
 *                  the number of packets expected less the number of packets
 *                  actually received, where the number of packets received
 *                  includes any which are late or duplicates. 
 *
 * sequence_number :The low 16 bits contain the highest sequence number 
 *                  received in an RTP data packet from source, and the most 
 *                  significant 16 bits extend that sequence number with the 
 *                  corresponding count of sequence number cycles.
 */
typedef struct
{
    unsigned int  fraction_lost;
    unsigned int  cumulative_lost;
    unsigned int  sequence_number;
} RTCP_RECEIVERREPORT;



/* RTCP_REPORT | This structure contains the sender and receiver RTCP
 * information.
 * 
 * sender : Contains the sender information
 *
 * receiver : Contains receiver information
 */
typedef struct
{
    RTCP_SENDERREPORT sender;
    RTCP_RECEIVERREPORT receiver;
} RTCP_REPORT;


/* IP_RTCPINFO | This structure provides RTCP information
 * 
 * local : RTCP information for the local end point.
 *
 * remote : RTCP information for the remote end point.
 */
typedef struct
{
    RTCP_REPORT local;
    RTCP_REPORT remote;
} IP_RTCPINFO;


/* IP_H221NONSTANDARD |
 *
 * country_code :
 *
 * extension : 
 *
 * manufacturer_code :
 */
typedef struct
{
    int  country_code;
    int  extension;
    int  manufacturer_code;
} IP_H221NONSTANDARD;

/* IP_TUNNELPROTOCOL_ALTID */
typedef struct
{
   unsigned long version;
   char     protocolType[MAX_TSM_ALTID_VARS_LENGTH];
   int      protocolTypeLength;
   char     protocolVariant[MAX_TSM_ALTID_VARS_LENGTH];
   int      protocolVariantLength;
   char     subIdentifier[MAX_TSM_ALTID_VARS_LENGTH];
   int      subIdentifierLength;
} IP_TUNNELPROTOCOL_ALTID;

/* IP_TUNNELPROTOCOL_OBJECTID */
typedef struct
{
   unsigned long version;
   char     TunneledProtocol_Oid[MAX_TSM_OBJID_VARS_LENGTH];
   int      TunneledProtocol_OidLength;
   char     subIdentifier[MAX_TSM_OBJID_VARS_LENGTH];
   int      subIdentifierLength;
} IP_TUNNELPROTOCOL_OBJECTID;

/* Use INIT_IP_TUNNELPROTOCOL_ALTID function to initialize the structure.
 * Application must initialize the structure before use.
 */
static __inline void INIT_IP_TUNNELPROTOCOL_ALTID (IP_TUNNELPROTOCOL_ALTID *pIpTpAltId) 
{
   memset((IP_TUNNELPROTOCOL_ALTID *)pIpTpAltId, 0, sizeof(IP_TUNNELPROTOCOL_ALTID));
   pIpTpAltId->version = IP_TUNNELPROTOCOL_ALTID_VERSION;            
}                           

/* Use INIT_IP_TUNNELPROTOCOL_OBJECTID function to initialize the structure.
 * Application must initialize the structure before use.
 */
static __inline void INIT_IP_TUNNELPROTOCOL_OBJECTID (IP_TUNNELPROTOCOL_OBJECTID *pIpTpObjId) 
{
   memset((IP_TUNNELPROTOCOL_OBJECTID *)pIpTpObjId, 0, sizeof(IP_TUNNELPROTOCOL_OBJECTID));
   pIpTpObjId->version = IP_TUNNELPROTOCOL_OBJECTID_VERSION;            
}

/* IP_UII_SIGNAL |
 *
 * type :
 *
 * duration :
 *
 * rtp_timestamp : 
 *
 * rtp_expiration_time :
 */
typedef struct
{
    char  type;
    int   duration;
    long  rtp_timestamp;
    long  rtp_expiration_time;
} IP_UII_SIGNAL;


/* IP_DTMF_DIGITS |
 *
 * digit_buf : digit string buffer of size 32
 *
 * num_digits : number of digits in string buffer
 */
typedef struct
{
    char          digit_buf[IP_MAX_DTMF_DIGITS];
    unsigned int  num_digits;
} IP_DTMF_DIGITS;


/* IP_ADDR |
 *
 * ip_ver : IP version as IPVER4 or IPVER6
 *
 * u_ipaddr : IP address as 32 bit IPv4 or 128 bit IPv6 (union)
 *            or IP_RETRIEVE_HOST_IPADDR to internally retrieve host IP address
 */
typedef struct
{
    unsigned char ip_ver;
    union
    {
        unsigned int ipv4;
        unsigned int ipv6[4];
    } u_ipaddr;
} IP_ADDR, *IP_ADDRP;

/*RTP_ADDR : for passing endpoint addresses (port and IP address) to app
*/
typedef struct
{
    int version;
    unsigned short port;
    unsigned char ip_ver;
    union
    {
        unsigned int ipv4;
        unsigned int ipv6[4];
    } u_ipaddr;
} RTP_ADDR, *RTP_ADDRP;

/* Payload for request errors*/
typedef struct {
   unsigned short  version;
   unsigned int error;
   char method[IP_SIP_METHODSIZE];
} REQUEST_ERROR, *REQUEST_ERRORP;

/* MIME_MEM | used in IP_VERTBOARD to specify MIME memory pool
 * Use INIT_MIME_MEM function to initialize the structure to defautl values
 *
 * version : 0x104 or up
 *
 * size : size of MIME buffer
 *
 * number: number of MIME buffer
 */
typedef struct
{
   unsigned short  version;              /* library use only. do not change value */
   unsigned int    size;                 /* size of MIME buffer */
   unsigned int    number;               /* number of MIME buffer */
} MIME_MEM;


#define MIME_MEM_VERSION   0x100

/* Use INIT_MIME_MEM function to initialize the structure to default values.
 *
 */
static __inline void INIT_MIME_MEM(MIME_MEM *pMimeMem) {
   pMimeMem->version = MIME_MEM_VERSION;            
   pMimeMem->size = (unsigned int)IP_CFG_DEFAULT;
   pMimeMem->number = (unsigned int)IP_CFG_DEFAULT;
}                           

/* IP_AUTHENTICATION | This structure is to configure SIP Digest Authentication 
 * authentication quadruplet.
 * 
 * version: version number of this structure
 *
 * realm : string that defines the protected domain
 *
 * identity: SIP URI string that uniquely identify the user in the realm.
 *
 * username : The user's name string in the specified realm
 *
 * password :  The user's password string associated to the user 
 *
 * The string pointer must not be NULL for realm, identity, username and password.
 */

typedef struct
{ 
    unsigned long        version;      /* library use only, do not change value */
    char*                realm;        /* must be null terminated string */
    char*                identity;     /* must be null terminated string */
    char*                username;     /* must be null terminated string, ignored during remove */
    char*                password;     /* must be null terminated string, ignored during remove */
} IP_AUTHENTICATION;

#define IP_AUTHENTICATION_VERSION   ((0x100 & 0xFFFF) | (sizeof(IP_AUTHENTICATION) << 16))

/* Use INIT_IP_AUTHENTICATION function to initialize the structure.
 * Application must initialize authentication quadruplet after initializing the structure.
 */
static __inline void INIT_IP_AUTHENTICATION(IP_AUTHENTICATION *pIPAuthentication) {
   pIPAuthentication ->version = IP_AUTHENTICATION_VERSION;            
   pIPAuthentication ->realm = NULL;
   pIPAuthentication ->identity = NULL;
   pIPAuthentication ->username = NULL;
   pIPAuthentication ->password = NULL;
}           

typedef enum{
// SSL versions are not required
//   ENUM_TLS_METHOD_SSL_V2 = 0,
//   ENUM_TLS_METHOD_SSL_V3,
   ENUM_TLS_METHOD_TLS_V1 = 2
}EnumSIP_TLS_METHOD;

typedef enum
{
    ENUM_Disabled=0,
    ENUM_Enabled=1
}  EnumSIP_Enabled;
 
/* SIP_TLS_ENGINE | Used in IP_VIRTBOARD to specify virtual board settings 
 *
 * Use INIT_SIP_TLS_ENGINE function to initialize the structure to default values
 *
 * version: version of structure 
 *
 * sip_tls_port: TLS port number GC will listen, default port number is 5061
 *
 * E_sip_tls_method: indicates the version of SSL to use: currently only TLSv1 is supported
 *
 * local_rsa_private_key_filename: file containing TLS RSA private key of local certificate, must be PEM 
 * (base64 encoded) X509 format, in plain text or encrypted.  default is NULL
 *
 * local_rsa_private_key_password: password string to read TLS RSA private key of local certificate, 
 * if it is encrypted.  default is NULL
 *
 * local_rsa_cert_filename: PEM file containing TLS RSA certificate representing local identity, must be PEM 
 * (base64 encoded) X509 format, in plain text or encrypted. default is NULL
 *
 * local_dss_private_key_filename: file containing TLS DSS private key of local certificate, must be PEM 
 * (base64 encoded) X509 format, in plain text or encrypted.  default is NULL
 *
 * local_dss_private_key_password: password string to read TLS DSS private key of local certificate, 
 * if it is encrypted.  default is NULL
 *
 * local_dss_cert_filename: PEM file containing TLS DSS certificate representing local identity, must be PEM 
 * (base64 encoded) X509 format, in plain text defaultis NULL
 *
 * ca_cert_number:  number of trusted certificates. TLS engine can trust zero, 
 * one or more root certificates. Once an engine trusts a root certificate, it will 
 * approve all valid certificates issued by that root certificate. Trusted certificates 
 * are (usually) root certificates. Default is 0.
 *
 * ca_cert_filename: array of trusted certificates filenames, must be PEM 
 * (base64 encoded) X509 format, in plain text. The size of array is 
 * ca_cert_number. Default is NULL.
 *
 * chain_cert_number: number of  chained certificates. An engine may hold a 
 * certificate that is not issued directly by a root certificate, but by a certificate 
 * authority delegated by that root certificate. To add this intermediate certificate
 * to the chain of certificates that the engine will present during a handshake, specify 
 * the chained certificates number here. Default is 0.
 *
 * chain_cert_filename: array of chained certificates filenames, must be PEM 
 * (base64 encoded) X509 format, in plain text. The size of array is 
 * chain_cert_number. Default is NULL.
 *
 * crl_number: number of CRL files. An engine may look up certificate revocation list
 * while examine the incoming certficates. To add one or more CRL files, specifiy 
 * the number of the CRL files here.Default is 0.
 *
 * crl_filename: array of CRL filenames, must be PEM format in plain text. The size of array is 
 * crl_number. Default is NULL.
 *
 * local_cipher_suite: the list of ciphers is specified by a specially formatted string
 * defined by OPENSSL. OPENSSL allows for several keywords in the elist, which are shortcuts
 * for sets of ciphers. Default is NULL which uses OPENSSL default string.
 *
 * dh_param_512_filename: filename containing DH parameter with 512 bit key length. 
 * Default is NULL which uses pre-built DH parameter with 512 bit key length. 
 *
 * dh_param_1024_filename: filename containing DH parameter with 1024 bit key length. 
 * Default is NULL which uses pre-built DH parameter with 1024 bit key length. 
 *
 * session_id: If session id is set, session caching is enabled on the server side.
 * session id will be provided to client during handshake so that client may reuse
 * the session for future connection. Default is NULL and session caching is disabled.
 *
 * E_client_cert_required: if set to ENUM_Enabled, TLS server will require client's certificate 
 * for mutual authentication. Default is ENUM_Disabled.
 *
 * E_block_udp_port: if set to ENUM_Enabled, UDP port will be disabled to prevent downgrade
 * attack. Both send and receive on UDP port will be rejected. Default is ENUM_Disabled.
 * 
 * E_block_tcp_port: if set to ENUM_Enabled, TCP port will be disabled to prevent downgrade
 * attack. Both send and receive on TCP port will be rejected. Default is ENUM_Disabled.
 */

typedef struct
{
   unsigned long version; 	/* version set by INIT_SIP_TLS_ENGINE */
   unsigned short  sip_tls_port;	/*  TLS port number GC will listen */
   EnumSIP_TLS_METHOD E_sip_tls_method;	/* TLS method*/
   char * local_rsa_private_key_filename; /* TLS local RSA private key file name */
   char * local_rsa_private_key_password; /* TLS local RSA private key password */
   char * local_rsa_cert_filename; /* TLS local RSA certificate file name */
   char * local_dss_private_key_filename; /* TLS local DSS private key file name */
   char * local_dss_private_key_password; /* TLS local DSS private key password */
   char * local_dss_cert_filename; /* TLS local DSS certificate file name */
   unsigned int ca_cert_number; /* number of CA certificates */
   char ** ca_cert_filename; 	 /* CA certificate file names */
   unsigned int chain_cert_number; /* chained certificate number*/
   char ** chain_cert_filename; /* chained certificate file names */
   unsigned int crl_number; /* number of CRL files */
   char ** crl_filename;    /* CRL file names containing certificate revocation list*/
   char * local_cipher_suite; /* local cipher suite list string*/
   char * dh_param_512_filename;  /* DH parameter file name with 512 bit key length */
   char * dh_param_1024_filename; /* DH parameter file name with 1024 bit key length*/
   char * session_id;   /* session identifier*/
   EnumSIP_Enabled  E_client_cert_required; /* client certificate is required */
   EnumSIP_Enabled  E_block_udp_port; /* block UDP port to prevent downgrade attack */
   EnumSIP_Enabled  E_block_tcp_port; /* block TCP port to prevent downgrade attack */
}SIP_TLS_ENGINE;


#define SIP_TLS_ENGINE_VERSION   ((0x100 & 0xFFFF) | (sizeof(SIP_TLS_ENGINE) << 16))

/* Use INIT_SIP_TLS_ENGINE function to initialize the structure.
 */
static __inline void INIT_SIP_TLS_ENGINE(SIP_TLS_ENGINE *pSipTLSEngine) {
   pSipTLSEngine ->version = SIP_TLS_ENGINE_VERSION;            
   pSipTLSEngine ->sip_tls_port = 5061;
   pSipTLSEngine ->E_sip_tls_method = ENUM_TLS_METHOD_TLS_V1;
   pSipTLSEngine ->local_rsa_private_key_filename = NULL;
   pSipTLSEngine ->local_rsa_private_key_password = NULL;
   pSipTLSEngine ->local_rsa_cert_filename = NULL;
   pSipTLSEngine ->local_dss_private_key_filename = NULL;
   pSipTLSEngine ->local_dss_private_key_password = NULL;
   pSipTLSEngine ->local_dss_cert_filename = NULL;
   pSipTLSEngine ->ca_cert_number = 0;
   pSipTLSEngine ->ca_cert_filename = NULL;
   pSipTLSEngine ->chain_cert_number = 0;
   pSipTLSEngine ->chain_cert_filename = NULL;
   pSipTLSEngine ->crl_number = 0;
   pSipTLSEngine ->crl_filename = NULL;
   pSipTLSEngine ->local_cipher_suite = NULL;
   pSipTLSEngine ->dh_param_512_filename = NULL;
   pSipTLSEngine ->dh_param_1024_filename = NULL;
   pSipTLSEngine ->session_id = NULL;
   pSipTLSEngine ->E_client_cert_required = ENUM_Disabled;
   pSipTLSEngine ->E_block_udp_port = ENUM_Disabled;
   pSipTLSEngine ->E_block_tcp_port = ENUM_Disabled;
}               

/* IP_VIRTBOARD | Used in IPCCLIB_START_DATA to specify virtual board settings 
 *
 * Use INIT_IP_VIRTBOARD function to initialize the structure to default values
 * the set any non-default parameters in structure.
 *
 * version : version of structure 
 *
 * total_max_calls : maximum total number of H.323 and SIP calls supported: 
 * possible values are 1-2016 or IP_CFG_MAX_AVAILABLE_CALLS
 *
 * h323_max_calls : maximum number of H.323 current calls supported: 
 * possible values are 1-2016 or IP_CFG_NO_CALLS or IP_CFG_MAX_AVAILABLE_CALLS
 *
 * sip_max_calls : maximum number of SIP current calls supported: 
 * possible values are 1-2016 or IP_CFG_NO_CALLS or IP_CFG_MAX_AVAILABLE_CALLS
 *
 * localIP : local IP address of type IP_ADDR 
 *
 * h323_signaling_port : H.323 call signaling port or IP_CFG_DEFAULT
 *
 * sip_signaling_port : SIP call signaling port or IP_CFG_DEFAULT
 *
 * reserved : must be set to NULL
 *
 * size - size of pack(1) structure.
 *
 * sip_msginfo_mask : bitmask to enable/disable access to SIP message fields
 *
 * sup_serv_mask : bitmask to enable/disable supplementary services
 * possible bits are IP_SUP_SERV_DISABLED or IP_SUP_SERV_CALL_XFER
 *
 * h323_msginfo_mask: bitmask to enable/disable information elements
 * exposure. Possible bits are IP_INFOELEMENT_DISABLED or
 * IP_INFOELEMENT_ENABLED
 *
 * terminal_type : Parameter representing h.323 and h.245 terminal type used
 *                 in GateKeeper registration and MSD respectivly.
 */
typedef enum 
{
    ENUM_PERSISTENCE_NONE = -1, 
    ENUM_PERSISTENCE_TRANSACT,         
    ENUM_PERSISTENCE_TRANSACT_USER          
} EnumSIP_Persistence;

typedef enum 
{
    ENUM_UDP =0,         
    ENUM_TCP =1,
    ENUM_TLS =2
} EnumSIP_TransportProtocol;

#define DEFAULT_MAXUDPMSGLEN (0)
#define VIRTBOARD_VERSION_TCP_SUPPORT 0x106 /*this value subject to change*/
#define VIRTBOARD_SIP_NOUDPMSGSIZECHECK  (0)

#define VIRTBOARD_VERSION_REQUEST_RETRY 0x107
#define VIRTBOARD_VERSION_OPTIONS_SUPPORT  0x108
#define VIRTBOARD_VERSION_SIP_REGISTRAR    0x109
#define VIRTBOARD_VERSION_TLS_SUPPORT      0x10a
#define VIRTBOARD_VERSION_SIP_PRACK		   0x10b
#define VIRTBOARD_VERSION_SESSION_TIMER_SUPPORT    0x10c
//New version number and structure member must be added to 5.1 line as well 
//to preserve compatibility with 5.2 line

typedef struct {
    unsigned short  version;              /* library use only. do not change value */
    unsigned int    total_max_calls;
    unsigned int    h323_max_calls;
    unsigned int    sip_max_calls;
    IP_ADDR         localIP;
    unsigned short  h323_signaling_port;
    unsigned short  sip_signaling_port;
    void            *reserved;            /* library use only. do not change value */
    unsigned short  size;                 /* library use only. do not change value */
    unsigned int    sip_msginfo_mask;     /* added in version 0x0101               */
    unsigned int    sup_serv_mask;        /* added in version 0x0102               */
    unsigned int    h323_msginfo_mask;    /* added in version 0x0103               */
    MIME_MEM        sip_mime_mem;         /* added in version 0x0104               */
    unsigned short  terminal_type;        /* added in version 0x0104               */
    IP_ADDR         outbound_proxy_IP;        /* added in version 0x0105 */
    unsigned short  outbound_proxy_port;      /* added in version 0x0105 */
    char*           outbound_proxy_hostname;  /* added in version 0x0105 */
/* the following added for VIRTBOARD_VERSION_TCP_SUPPORT */
    EnumSIP_Enabled E_SIP_tcpenabled;		
    EnumSIP_TransportProtocol  E_SIP_OutboundProxyTransport;  
    EnumSIP_Persistence E_SIP_Persistence;   
    unsigned short  SIP_maxUDPmsgLen;    
    EnumSIP_TransportProtocol  E_SIP_DefaultTransport;  
/* end VIRTBOARD_VERSION_TCP_SUPPORT additions*/

/* the following added for VIRTBOARD_VERSION_REQUEST_RETRY */
    EnumSIP_RequestRetry E_SIP_RequestRetry;
/* end VIRTBOARD_VERSION_REQUEST_RETRY additions*/
    EnumSIP_Enabled E_SIP_OPTIONS_Access;


/* Begin VIRTBOARD_VERSION_SIP_REGISTRAR additions */
    int    SIP_registrar_registrations;
/* End VIRTBOARD_VERSION_SIP_REGISTRAR additions   */

/* The following is added for VIRTBOARD_VERSION_TLS_SUPPORT support */
    SIP_TLS_ENGINE *sip_tls_engine;
/* end VIRTBOARD_VERSION_TLS_SUPPORT additions */

   /* Begin PRACK related additions. */
   EnumSIP_Enabled E_SIP_PrackEnabled;
   /* End PRACK related additions. */

/* The following is added for VIRTBOARD_VERSION_SESSION_TIMER_SUPPORT support */
    EnumSIP_Enabled E_SIP_SessionTimer_Enabled;	
    unsigned int    SIP_SessionTimer_SessionExpires;
    unsigned int    SIP_SessionTimer_MinSE;
/* end VIRTBOARD_VERSION_SESSION_TIMER_SUPPORT additions */
} IP_VIRTBOARD;

#define IP_VIRTBOARD_SIZE      sizeof(IP_VIRTBOARD)
#define IP_VIRTBOARD_VERSION   VIRTBOARD_VERSION_SESSION_TIMER_SUPPORT
 

/* Use INIT_IP_VIRTBOARD function to initialize the structure to default values.
 *
 * Applications must use this function to inititialize the IP_VIRTBOARD data 
 * structures to maintain forward binary compatability.
 */
static __inline void INIT_IP_VIRTBOARD(IP_VIRTBOARD *pIpVb) {
   memset(pIpVb, IP_CFG_DEFAULT, IP_VIRTBOARD_SIZE); 
   pIpVb->version = IP_VIRTBOARD_VERSION;            
   pIpVb->size = IP_VIRTBOARD_SIZE;
   pIpVb->sip_msginfo_mask = 0;
   pIpVb->sup_serv_mask = IP_SUP_SERV_DISABLED;
   pIpVb->h323_msginfo_mask = 0;
   INIT_MIME_MEM(&pIpVb->sip_mime_mem);
   pIpVb->terminal_type = IP_TT_GATEWAY;
   pIpVb->outbound_proxy_hostname = NULL;
/* the following added for VIRTBOARD_VERSION_TCP_SUPPORT */

   pIpVb->E_SIP_tcpenabled=ENUM_Disabled;		
   pIpVb->E_SIP_OutboundProxyTransport=ENUM_UDP;  
   pIpVb->E_SIP_Persistence=ENUM_PERSISTENCE_TRANSACT_USER;  
   pIpVb->SIP_maxUDPmsgLen=DEFAULT_MAXUDPMSGLEN;  
   pIpVb->E_SIP_DefaultTransport=ENUM_UDP;
/* the following added for VIRTBOARD_VERSION_REQUEST_RETRY */

   pIpVb->E_SIP_RequestRetry= ENUM_REQUEST_RETRY_ALL;
   pIpVb->E_SIP_OPTIONS_Access= ENUM_Disabled;

/* the following added for VIRTBOARD_VERSION_TCP_SUPPORT */
   pIpVb->sip_tls_engine = NULL;

/* the following added for VIRTBOARD_VERSION_SIP_PRACK */
   pIpVb->E_SIP_PrackEnabled = ENUM_Disabled;

/* the following added for VIRTBOARD_VERSION_SESSION_TIMER_SUPPORT */
   pIpVb->E_SIP_SessionTimer_Enabled = ENUM_Disabled;
   pIpVb->SIP_SessionTimer_SessionExpires = 1800;
   pIpVb->SIP_SessionTimer_MinSE = 90;

}                                                     

/* IPCCLIB_START_DATA | Used in CCLIB_START_STRUCT to configure GC_H3R_LIB library 
 *
 * version : version of structure - set to 0x100 for this release
 *                                - set to 0x101 for Request-URI feature
 *                                - set to 0x200 for get/set access of GC parm data
 *                                  greater than 255 bytes. 
 *
 * delimiter : ANSI character used to change default address string delimiter of ","
 *
 * num_boards : number of virtual boards specifed in board_info array element.
 *
 * board_info: array of virtual board board elements of type IP_VIRTBOARD
 *
 * media_operational_mode: 1PCC Embedded media or 3PCC Split Media library operation 
 *
 */
typedef struct 
{
    unsigned short         version;
    unsigned char          delimiter;
    unsigned char          num_boards; 
    IP_VIRTBOARD*          board_list;
    unsigned long          max_parm_data_size;
    enumIPCCLIBMediaMode   media_operational_mode;
} IPCCLIB_START_DATA;

/* Use INIT_IPCCLIB_START_DATA function to initialize the IPCCLIB_START_DATA structure.
 *
 * Applications must use this function to inititialize the IPCCLIB_START_DATA data 
 * structures to maintain forward binary compatability.
 */
static __inline void INIT_IPCCLIB_START_DATA(IPCCLIB_START_DATA *pIpStData, 
                                             unsigned char numBoards, 
                                             IP_VIRTBOARD *pIpVb) {
   pIpStData->version = 0x201;
   pIpStData->delimiter = ',';
   pIpStData->num_boards = numBoards;
   pIpStData->board_list = pIpVb;
   pIpStData->max_parm_data_size = (unsigned long)IP_CFG_DEFAULT;
   pIpStData->media_operational_mode = MEDIA_OPERATIONAL_MODE_1PCC;
}

/* IP_REGISTER_ADDRESS | Used in gc_ReqService to specify address of gatekeeper
 *                       (H.323) or registrar (SIP). 
 * reg_client : 128 character local address of registering host
 *
 * reg_server : 64 character local address of gatekeeper (H.323) or registrar (SIP)
 *
 * time_to_live : unicast TTL in seconds
 * 
 * max_hops : multicast TTL in hops
 */
typedef struct 
{
    char              reg_client [IP_REG_CLIENT_ADDR_LENGTH];
    char              reg_server [IP_REG_SERVER_ADDR_LENGTH];
    int               time_to_live;
    int               max_hops;
} IP_REGISTER_ADDRESS;


/* IP_RFC2833_EVENT | Used in GCEV_EXTENSION event for notification of detected 
 * incoming RFC2833 events.
 *
 * event : event type as defined per RFC 2833 as either GCPEV_ENABLE (ON) or 
 * GCPV_DISABLE (OFF).
 * 
 * state : 
 */
typedef struct {
    unsigned char     event;
    unsigned char     state;
} IP_RFC2833_EVENT;

/* IP_CONNECT | Used in gc_SetUserInfo() for set media device connection
 * in T38 fax server application.The data structure should support necessary
 * fields required by dev_connect() and dev_disconnect() functions.
 *
 * version :      version of this structure. currently supported is 0x100
 * mediaHandle:   media device handle required by dev_connect and dev_disconnect
 * faxHandle:     fax device handle requried by dev_connect
 * connectType:   connection type, full or half duplex defined eIPConnecType_e
 * 
 */

typedef enum {
     IP_FULLDUP = 1,
     IP_HALFDUP
} eIPConnectType_e;

typedef struct {
   unsigned short    version;
   int               mediaHandle;
   int               faxHandle;
   eIPConnectType_e  connectType;
} IP_CONNECT;

/* Use INIT_IP_CONNECT function to initialize the IP_CONNECT data structure.
 *
 * Applications must use this function to inititialize the IP_CONNECT data 
 * structures to maintain forward binary compatability.
 */
static __inline void INIT_IP_CONNECT(IP_CONNECT *pIpConnect, int mediaHandle, 
                                     int faxHandle, eIPConnectType_e  connectType){
   pIpConnect->version = 0x100;
   pIpConnect->mediaHandle = mediaHandle;  
   pIpConnect->faxHandle   = faxHandle;     
   pIpConnect->connectType = connectType; 
}

/* Following defines added for IP CCLIB Lan Cable disconnect alarms */

typedef enum
{
   TYPE_LAN_DISCONNECT = 0x01
} eIPCCLIB_ALARM_TYPE;

typedef enum
{
   CCLIB_ALARM_OFF = 0x0,
   CCLIB_ALARM_ON = 0x1
} eIPCCLIB_ALARM_STATE;

typedef struct ip_cclib_alarm_data_tag
{
   eIPCCLIB_ALARM_TYPE  eAlarmType;       /*The QoS parameter type */
   eIPCCLIB_ALARM_STATE eAlarmState;   /*indicate if On/Off */
} IP_CCLIB_ALARM_DATA, *PIP_CCLIB_ALARM_DATA;

typedef struct ip_cclib_alarm_status_tag
{
   unsigned int unAlarmCount;
   IP_CCLIB_ALARM_DATA   IPAlarmData[MAX_IPCCLIB_ALARMS];
} IP_CCLIB_ALARM_STATUS, *PIP_CCLIB_ALARM_STATUS;

void InitializeIPCCLIBAso( void);
void CallIpCcLibAlarm(long bdev, IP_CCLIB_ALARM_DATA* l_pAlarm_Data);
int ipaso_AlarmNumberToName( long alarm_number, char **name);

/* Following defines added for support of FR2558 */
typedef unsigned char CPN_TON;   /* Type of number */
typedef unsigned char CPN_NPI;   /* Numbering plan identification */
typedef unsigned char CPN_SI;    /* Screening Indicator       */
typedef unsigned char CPN_PI;    /* Presentation Indicator */

#pragma pack()


#endif
