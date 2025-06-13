#include "stdafx.h"
#include "ace/Log_Msg.h"
#include "ace/OS.h"
#include "ipc/HeaderExtension.h"

using namespace Metreos::IPC;

// Return header extension as a byte array as expected by the
// FlatmapList class.
// headerExtension - Byte array sized to hold all fields that will
//                   be copied into it.
// Returns header extension as a byte array.
void HeaderExtension::ToArray(char *headerExtension)
{
    ACE_ASSERT(headerExtension != NULL);
    // Paranoid sanity check for integral size.
    ACE_ASSERT(sizeof messageType == MessageTypeByteLength);

    // Copy extension-header fields to byte array at specific offset.
    // Addition fields may be added in the future.
    ACE_OS::memcpy(headerExtension + 0, &messageType, sizeof messageType);
}
