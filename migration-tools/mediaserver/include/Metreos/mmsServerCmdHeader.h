// 
// mmsServerCmdHeader.h 
//                     
// Media server result code definitions
//
// Flatmap header extension for MMS command maps
// 
#ifndef MMS_SERVERCMD_HEADER_H
#define MMS_SERVERCMD_HEADER_H
#ifdef MMS_WINPLATFORM
#pragma once
#endif
#include "mmsFlatMap.h"

class MmsIpcAdapter;


                                            // MmsFlatMap header extension
struct MmsServerCmdHeader                   // used by MMS protocol adapters
{ 
  int  length;                              // Length of extension
  int  sig;                                 // MmsServerCmdHeader signature
  int  command;                             // Server command
  int  serverID;                            // Client's ID for server in cluster
  int  sessionID;                           // Mapping to server Session object
  int  connectionID;                        // Server connection ID
  int  operationID;                         // Server-generated operation ID
  int  transactionID;                       // Client message correlation ID
  int  returncode;                          // Command result
  int  reasoncode;                          // Command result reason code
  int  termreason;                          // Termination
  long elapsed;                             // Elapsed media time
  MmsIpcAdapter* sender;                    // Protocol adapter which owns map
  void* clienthandle;                       // Handle of adapter client
  unsigned int param;                       // Arbitrary info set along the way
  unsigned int flags;                       // Bitflags (update under lock)
  unsigned int xflags;                      // Bitflags (turn-based, no lock)
  enum{signature=0xbaddecaf};               // Definition of signature bit pattern

  enum bitflags
  { IS_ERROR=0x1,                           // Command resulted in an error
    IS_CONFERENCE=0x2,                      // Command operates on a conference
    IS_EXISTING_CONNECTION=0x4,             // Session already exists 
    IS_NOSTARTMEDIA=0x8,                    // Half connection request
    IS_MULTISESSION=0x10,                   // Command operates on >1 sessions 
    IS_SESSION_BREAK_IN=0x20,               // Command operates outside session 
    IS_SESSION_RECONNECT=0x40,              // Connect is switching port/IP
    IS_MODIFY_CONNECT=0x80,                 // Connect modifies connection attributes
    IS_REXMIT_CONNECTION=0x100,             // Connect initially listens to itself 0322
    IS_UTILITY_SESSION=0x200,               // Command uses temp session context
    IS_RESULT_STRING_EXPECTED=0x400,        // MMS invented filename 
    IS_CONCURRENT_SERVICE_THREAD=0x800,     // Command thread is not serialized
  };
                                            // Bitflags which may be modified
  enum xflags                               // only at agreed-upon locations:
  { IS_PROVISIONAL_RESULT=0x1,              // Returned package is provisional
    IS_DEPENDENCY_PENDING=0x2               // Prior command has not yet completed
  };

  MmsServerCmdHeader()  
  { memset(this,0,sizeof(MmsServerCmdHeader));
    length = sizeof(MmsServerCmdHeader);
    sig    = signature;
  }
};


                                            // Macros to peek inside map binary
#define isServerCmdHeader(buf) \
  (((MmsServerCmdHeader*)(buf+sizeof(MmsFlatMap::MapHeader)))->sig == MmsServerCmdHeader::signature) 

#define getFlatmapSender(buf)  \
  (((MmsServerCmdHeader*)(buf+sizeof(MmsFlatMap::MapHeader)))->sender)
#define getFlatmapCommand(buf) \
  (((MmsServerCmdHeader*)(buf+sizeof(MmsFlatMap::MapHeader)))->command)  
#define getFlatmapSessionID(buf) \
  (((MmsServerCmdHeader*)(buf+sizeof(MmsFlatMap::MapHeader)))->sessionID)  
#define getFlatmapConnectionID(buf) \
  (((MmsServerCmdHeader*)(buf+sizeof(MmsFlatMap::MapHeader)))->connectionID) 
#define getFlatmapOperationID(buf) \
  (((MmsServerCmdHeader*)(buf+sizeof(MmsFlatMap::MapHeader)))->operationID) 
#define getFlatmapRetcode(buf) \
  (((MmsServerCmdHeader*)(buf+sizeof(MmsFlatMap::MapHeader)))->returncode) 
#define getFlatmapRescode(buf) \
  (((MmsServerCmdHeader*)(buf+sizeof(MmsFlatMap::MapHeader)))->reasoncode) 
#define getFlatmapTermReason(buf) \
  (((MmsServerCmdHeader*)(buf+sizeof(MmsFlatMap::MapHeader)))->termreason) 
#define getFlatmapElapsedTime(buf) \
  (((MmsServerCmdHeader*)(buf+sizeof(MmsFlatMap::MapHeader)))->elapsed)   
#define getFlatmapParam(buf) \
  (((MmsServerCmdHeader*)(buf+sizeof(MmsFlatMap::MapHeader)))->param) 
#define getFlatmapTransID(buf) \
  (((MmsServerCmdHeader*)(buf+sizeof(MmsFlatMap::MapHeader)))->transactionID) 
#define getFlatmapClientHandle(buf) \
  (((MmsServerCmdHeader*)(buf+sizeof(MmsFlatMap::MapHeader)))->clienthandle) 
#define getFlatmapServerID(buf) \
  (((MmsServerCmdHeader*)(buf+sizeof(MmsFlatMap::MapHeader)))->serverID)
#define getFlatmapFlags(buf) \
  (((MmsServerCmdHeader*)(buf+sizeof(MmsFlatMap::MapHeader)))->flags)
#define getFlatmapXflags(buf) \
  (((MmsServerCmdHeader*)(buf+sizeof(MmsFlatMap::MapHeader)))->xflags)

#define setFlatmapSessionID(buf,n) \
  (((MmsServerCmdHeader*)(buf+sizeof(MmsFlatMap::MapHeader)))->sessionID) = (n) 
#define setFlatmapConnectionID(buf,n) \
  (((MmsServerCmdHeader*)(buf+sizeof(MmsFlatMap::MapHeader)))->connectionID) = (n) 
#define setFlatmapOperationID(buf,n) \
  (((MmsServerCmdHeader*)(buf+sizeof(MmsFlatMap::MapHeader)))->operationID) = (n) 
#define setFlatmapTransID(buf,n) \
  (((MmsServerCmdHeader*)(buf+sizeof(MmsFlatMap::MapHeader)))->transactionID) = (n)    
#define setFlatmapParam(buf,n) \
  (((MmsServerCmdHeader*)(buf+sizeof(MmsFlatMap::MapHeader)))->param) = (n)  
#define setFlatmapRetcode(buf,n) \
  (((MmsServerCmdHeader*)(buf+sizeof(MmsFlatMap::MapHeader)))->returncode) = (n) 
#define setFlatmapRescode(buf,n) \
  (((MmsServerCmdHeader*)(buf+sizeof(MmsFlatMap::MapHeader)))->reasoncode) = (n)   
#define setFlatmapTermReason(buf,n) \
  (((MmsServerCmdHeader*)(buf+sizeof(MmsFlatMap::MapHeader)))->termreason) = (n)  
#define setFlatmapElapsedTime(buf,n) \
  (((MmsServerCmdHeader*)(buf+sizeof(MmsFlatMap::MapHeader)))->elapsed) = (n)  
#define setFlatmapClientHandle(buf,n) \
  (((MmsServerCmdHeader*)(buf+sizeof(MmsFlatMap::MapHeader)))->clienthandle) = (n) 
#define setFlatmapServerID(buf,n) \
  (((MmsServerCmdHeader*)(buf+sizeof(MmsFlatMap::MapHeader)))->serverID) = (n) 
#define setFlatmapFlags(buf,n) \
  (((MmsServerCmdHeader*)(buf+sizeof(MmsFlatMap::MapHeader)))->flags) = (n) 
#define setFlatmapXflags(buf,n) \
  (((MmsServerCmdHeader*)(buf+sizeof(MmsFlatMap::MapHeader)))->xflags) = (n) 

#define setFlatmapFlag(buf,n) \
  (((MmsServerCmdHeader*)(buf+sizeof(MmsFlatMap::MapHeader)))->flags)  |= (n) 
#define setFlatmapXflag(buf,n) \
  (((MmsServerCmdHeader*)(buf+sizeof(MmsFlatMap::MapHeader)))->xflags) |= (n) 
#define clearFlatmapFlag(buf,n) \
  (((MmsServerCmdHeader*)(buf+sizeof(MmsFlatMap::MapHeader)))->flags)  &= ~(n) 
#define clearFlatmapXflag(buf,n) \
  (((MmsServerCmdHeader*)(buf+sizeof(MmsFlatMap::MapHeader)))->xflags) &= ~(n)  

#define isFlatmapFlagSet(buf,n) \
 ((((MmsServerCmdHeader*)(buf+sizeof(MmsFlatMap::MapHeader)))->flags)  & (n)) != 0
#define isFlatmapXflagSet(buf,n) \
 ((((MmsServerCmdHeader*)(buf+sizeof(MmsFlatMap::MapHeader)))->xflags) & (n)) != 0   


#define setSessionID        1               // session.putSessionMapHeader IDs
#define setConnectionID     2
#define setTransID          3
#define setParam            4
#define setFlag             5
#define setRetcode          6
#define setReasonCode       7
#define setTermReason       8
#define setElapsedTime      9
#define setClientHandle    10
#define setServerId        11
#define setOperationId     12

                                            // Reason codes
#define MMS_REASON_CODER_NOT_RECOGNIZED  6  // Coder specified was invalid
#define MMS_REASON_CODER_NOT_AVAILABLE   7  // Requested coder not available
#define MMS_REASON_NO_ACTION            16  // No action was taken
#define MMS_REASON_NO_CHANGE            17  // Nothing was changed

  
#define MMS_ISCOMMANDERROR(n)       ((n)>3) // Return codes from server to client
#define MMS_ERROR_NOERROR                1  // 1-3 not used as error indicators
#define MMS_ERROR_NOERROR2               2  // Command in wait state
#define MMS_ERROR_TURNAROUNDTEST         2  // Command purposely not handled 
#define MMS_ERROR_SERVER_BUSY            4  // All sessions are in use
#define MMS_ERROR_SERVER_INACTIVE        5  // Server disabled likely shutdown
#define MMS_ERROR_SERVER_INTERNAL        6  // Server code or logic error
#define MMS_ERROR_DEVICE                 7  // Device error
#define MMS_ERROR_RESOURCE_UNAVAILABLE   8  // Media resource not available 
#define MMS_ERROR_STATE                  9  // Server in unexpected state 
#define MMS_ERROR_EVENT_REGISTRATION    10  // Event registration error
#define MMS_ERROR_ASYNC_EVENT           11  // Unspecified event error 
#define MMS_ERROR_TIMEOUT_SESSION       12  // Session inactivity
#define MMS_ERROR_TIMEOUT_OPERATION     13  // Command timed out
#define MMS_ERROR_CONNECTION_BUSY       14  // Session busy with prior request
#define MMS_ERROR_ALREADY_CONNECTED     15  // A connection already exists
#define MMS_ERROR_NOT_CONNECTED         16  // No connection exists
#define MMS_ERROR_EVENT_UNKNOWN         20  // Unrecognized event fired
#define MMS_ERROR_NO_SUCH_COMMAND       21  // Command number not in our list
#define MMS_ERROR_CONNECTION_ID         22  // Connection ID invalid format
#define MMS_ERROR_NO_SUCH_CONNECTION    23  // Connection ID not registered
#define MMS_ERROR_NO_SUCH_CONFERENCE    24  // Session is not in conference
#define MMS_ERROR_NO_SUCH_OPERATION     25  // No operation in progress
#define MMS_ERROR_TOO_FEW_PARAMETERS    26  // Insufficient params supplied
#define MMS_ERROR_PARAMETER_VALUE       27  // Value error e.q. non-numeric
#define MMS_ERROR_FILEOPEN              30  // File open error  
#define MMS_ERROR_FILEIO                31  // File read or write error
#define MMS_ERROR_MALFORMED_REQUEST     35  // Server command format error
#define MMS_ERROR_TERMINATION_CONDITION 36  // No such condition or bad value


#endif

