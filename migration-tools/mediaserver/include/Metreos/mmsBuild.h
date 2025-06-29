//
// mmsBuild.h 
//
// describes parameters of the current distribution build 
//
// Note that version and build number are defined in mmsServer.cpp
// If those items were to be defined here it would necessitate a
// complete recompile on every CVS checkin.
//   
#ifndef MMS_BUILD_H
#define MMS_BUILD_H
#ifdef MMS_WINPLATFORM
#pragma once
#endif

// We use this constant to specify a Metreos license for the maximum number
// of resources available to the server via other licenses such as HMP
#define MMS_MAXIMUM_AVAILABLE_RESOURCES (65535)
                                             
// A server build is configured for a specific number of maximum concurrent
// connections (sessions), irresepective of either the HMP license in effect
// or the maximum connections specified in the server config information. 
#define MEDIASERVER_CUSTOMER_CONCURRENCY_LICENSES (512)

// Voice resources should usually be set to maximum; however if the customer
// has fewer voice voice resources than IP resources (at this writing not
// an option with HMP), MMS can virtualize them to some extent, and so the
// option is here to license additional voice resources so acquired
#define MEDIASERVER_CUSTOMER_VOICE_LICENSES MMS_MAXIMUM_AVAILABLE_RESOURCES

// The option is here to limit available conferencing resources to fewer
// than that available with the customer's HMP license.
#define MEDIASERVER_CUSTOMER_CONFERENCE_LICENSES MMS_MAXIMUM_AVAILABLE_RESOURCES


#undef MMS_USING_MESSAGE_FACTORY


// - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
// Various compile-time constraints constants
// - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
#define MMS_MAX_PLAY_REC_FILES         8    // Maximum size of a playlist
#define MMS_MAX_TERMINATION_CONDITIONS 8    // TODO remove these if unused

#endif

