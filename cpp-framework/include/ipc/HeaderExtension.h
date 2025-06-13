#ifndef HEADER_EXTENSION_H
#define HEADER_EXTENSION_H

#include "cpp-core.h"

namespace Metreos
{

namespace IPC
{

// Class for IPC message header extension.
class HeaderExtension
{
private:
    // Length of int fields in header-extension byte array.
    const static size_t IntLengthInHeaderExtension = 4;

    // Length of message type in header-extension byte array.
    const static size_t MessageTypeByteLength = IntLengthInHeaderExtension;

public:
    //
    // List of header-extension fields.
    // Additional fields may be added in the future.
    //

    // Message type.
    int messageType;

    // Length of extension header. Includes length of all fields.
    const static size_t SizeAsArray = MessageTypeByteLength;

    // Simple constructor with values for all header-extension fields.
    // messageType - Message type.
    HeaderExtension(int messageType) : messageType(messageType) {}

    // Construct header-extension object from a flatmap
    // header-extension byte array.
    // array - Flatmap header-extension byte array.
    HeaderExtension(char *array) : messageType(*(int *)array) {}


    // Return header extension as a byte array as expected by the
    // FlatmapList class.
    // headerExtension - Byte array sized to hold all fields that will
    //                   be copied into it.
    // Returns header extension as a byte array.
    void ToArray(char *headerExtension);
};

} // namespace IPC
} // namespace Metreos

#endif HEADER_EXTENSION_H