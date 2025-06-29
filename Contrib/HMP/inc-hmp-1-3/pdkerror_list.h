/**********************************************************************
*
*   C Header:       pdkerror_list.h
*   Instance:       1
*   Description:    
*   %created_by:    parikhn %
*   %date_created:  Tue Apr 09 13:23:25 2002 %
*
**********************************************************************/
/**********************************************************************
*  Copyright (C) 1998-2002 Intel Corporation.
*  All Rights Reserved
*
*  All names, products, and services mentioned herein are the 
*  trademarks or registered trademarks of their respective 
*  organizations and are the sole property of their respective owners.
**********************************************************************/

#ifndef _1_pdkerror_list_h_H
#define _1_pdkerror_list_h_H

#ifndef lint
static 
#ifdef __cplusplus          
const             /* C++ needs const */     
#endif
char    *_1_pdkerror_list_h = "@(#) %filespec: pdkerror_list.h-14 %  (%full_filespec: pdkerror_list.h-14:incl:gc#1 %)";
#endif

//
// Caution: if update this, also need to update gc_pdk_error.cpp for GC
// there is probably also a DM3 file that also needs to be updated
//
enum {
    EPDK_NOERR = 0,             // Success - must be 0 for compatability with GC
    EPDK_NOCALL,                // No call made/transfered
    EPDK_ALARM,                 // Function interrupted by alarm
    EPDK_ATTACHED,              // Voice resource already attached
    EPDK_DEVICE,                // Bad device handle
    EPDK_INVPROTOCOL,           // Invalid protocol name
    EPDK_PROTOCOL,              // Protocol error
    EPDK_SYNC,                  // The mode flag must be EV_ASYNC
    EPDK_ASYNC,                 // The mode flag must be EV_SYNC
    EPDK_TIMEOUT,               // Multi-tasking function timed out
    EPDK_UNSUPPORTED,           // Function is not supported
    EPDK_USER,                  // Function interrupted by user
    EPDK_VOICE,                 // No voice resource attached
    EPDK_NDEVICE,               // Too many devices opened
    EPDK_NPROTOCOL,             // Too many protocols opened
    EPDK_COMPATIBILITY,         // Incompatible component
    EPDK_PUTEVT,                // Error queuing event
    EPDK_DXOPEN,                // Error opening voice channel
    EPDK_NOMEM,                 // Out of memory
    EPDK_PFILE,                 // Error opening parameter file
    EPDK_SYSTEM,                // System error
    EPDK_CHSTATE,               // Error getting voice channel state
    EPDK_CLRTONE,               // Clear tone template failed
    EPDK_DISTONE,               // Disable tone failed
    EPDK_ENBTONE,               // Enable tone failed
    EPDK_GETEVT,                // Get event failed
    EPDK_PLAYTONE,              // Play tone failed
    EPDK_SETDMASK,              // set DTMF mask and method failed
    EPDK_GETDIGIT,              // Get DTMF or pulse digit failed
    EPDK_CLRDIGIT,              // Clear DTMF buffer failed
    EPDK_DIAL,                  // "Dialing failed"
    EPDK_SETSIGBITS,            // Set DTI signaling bits failed
    EPDK_SIGTYPE,               // Change transmit type failed
    EPDK_DTITASK,               // Start DTI task failed
    EPDK_SENDWINK,              // Send wink failed
    EPDK_INVSTATE,              // Invalid state
    EPDK_INVCRN,                // Invalid CRN
    EPDK_R2MF,                  // Bad opcode in R2MF functions
    EPDK_DTIPARM,               // Change DTI parameter failed
    EPDK_SETDTIEVT,             // set DTI signaling mask failed
    EPDK_SETIDLE,               // Change DTI idle state failed
    EPDK_INVLINEDEV,            // Invalid line device
    EPDK_INVPARM,               // Invalid parameter specified
    EPDK_SRL,                   // SRL error
    EPDK_USRATTRNOTSET,         // UsrAttr was not set for this line device
    EPDK_XMITALRM,              // Send alarm failed
    EPDK_SETALRM,               // Set alarm mode failed
    EPDK_INVDEVNAME,            // Invalid device name
    EPDK_DTOPEN,                // dt_open failed
    EPDK_ILLSTATE,              // Function is not supported in the current state
    EPDK_BUSY,                  // Line is busy
    EPDK_NOANSWER,              // Ring, no answer
    EPDK_NOT_INSERVICE,         // Number not in service
    EPDK_NOVOICE,               // Call needs voice, use     attach(
    EPDK_NO_TIMERS_AVL,         // No more timers available
    EPDK_TIMER_FAILED,          // Timer start failed
    EPDK_NO_MORE_AVL,           // No more information available
    EPDK_BUFFEROVFL,            // Buffer overflow
    EPDK_NOFREECALL,            // No free call object
    EPDK_NOTFOUND,              // Object not found
    EPDK_INV_CDP_LINE,          // Invalid line in cdp or sit file
    EPDK_DYN_CAST,              // Dynamic cast
    EPDK_INTERNAL,              // Internal
    EPDK_INVALID_ARG,           // Invalid argument
    EPDK_INVALID_OPERATION,     // Invalid operation
    EPDK_INVALID_OBJECT,        // Invalid object exception
    EPDK_UNSUPPORTED_FEATURE,   // Unsupported feature
    EPDK_NOT_ASSIGNED,          // Error number not assigned
    EPDK_SYSTEMEXCEPTION,       // System exception
    EPDK_NODATA,                // No data for request
    EPDK_BLOCKED,               // The channel is blocked
    EPDK_NO_MORE_DNIS,          // There is no more DNIS digits
    EPDK_LOADDXPARM,                // Unable to load DX parameter file
    EPDK_FATALERROR_ACTIVE,     // Fatal error occurred
    EPDK_RESETABLE_FATALERROR,  // Resetable fatal error occurred
    EPDK_RECOVERABLE_FATALERROR,    // Recoverable fatal error occurred
    EPDK_NON_RECOVERABLE_FATALERROR,    // Non-Recoverable fatal error occurred
    EPDK_FATALERROR_OCCURRED,   // Fatal error occurred
    EPDK_FAIL_OPEN_CDP,         // Failure to open a CDP file
    EPDK_INVALID_CDP_PLATFORM,  // Invalid platform of a cdp parameter in cdp file
    EPDK_INVALID_CDP_TYPE,      // Invalid type of a cdp parameter in cdp file
    EPDK_INVALID_CDP_NAME,      // Invalid name of a cdp parameter in cdp file
    EPDK_INVALID_CDP_VALUE,     // Invalid value of of a cdp parameter in cdp file
    EPDK_INVALID_CDP_FORMAT,    // Invalid format (mostly number of elements) of a cdp parameter in cdp file
    EPDK_UNLOADED_CDP_NAME,     // Unloaded cdp parameter's name: it is defined in cdp file but not used in psi file
    EPDK_UNLOADED_CDP_VALUE,    // Unloaded cdp value: it can be set in psidatebase
    EPDK_UNINIT_CDP_NAME,       // Uninitialized cdp parameter's name: it is used in psi file but not defined in cdp file
    EPDK_DISCONNECTED,          // Remote site is disconnected
    EPDK_INVTARGETTYPE,         // invalid target object type
    EPDK_INVTARGETID,           // invalid target object ID 
    EPDK_INVPARMBLK,            // invalid GC_PARM_BLKP
    EPDK_INVDATABUFFSIZE,       // invalid parm data buffer size
    EPDK_INVSETID,              // invalid set ID
    EPDK_INVPARMID,             // invalid parm ID
    EPDK_PARM_UPDATEPERM_ERR,   // the parameter is not allowed to be updated
    EPDK_PARM_VALUESIZE_ERR,    // value buffer size error
    EPDK_PARM_VALUE_ERR,        // parm value error
    EPDK_INVPARM_TARGET,        // invalid parmater for target object
    EPDK_INVPARM_GCLIB,         // invalid parameter for GCLib
    EPDK_INVPARM_CCLIB,         // invalid parameter for CCLib  
    EPDK_INVPARM_PROTOCOL,      // invalid parameter for protocol
    EPDK_INVPARM_FIRMWARE,      // invalid parameter for firmware
    EPDK_PARM_DATATYPE_ERR,     // the parameter data type error
    EPDK_NEXT_PARM_ERR,         // next parm error
    EPDK_INVUPDATEFLAG,         // Invalid update flag,
    EPDK_INVQUERYID,            // invalid query ID
    EPDK_QUERYEDATA_ERR,        // query data error
    EPDK_GLARE,                 // glare condition with incoming call present
    EPDK_CALLPROGRESS,          // Call progress
    EPDK_NODIALTONE,            // No dial tone
    EPDK_NORINGBACK,            // No ringback
    EPDK_REJECTED,              // Call is rejected
    EPDK_SITTONEDETECTED,       // Sit tone is detected
    EPDK_UNASSIGNEDNUMBER,      // Unassigned number
    EPDK_CONGESTION,       	  // Network congestion
    EPDK_NORMALCLEARING         // Normal clearing, e.g. other inbound side may
                                // have done a disconnect before connect
};

#endif
