#ifndef MEMORY_STREAM_H
#define MEMORY_STREAM_H

#include "cpp-core.h"
#include "ace/OS.h"
#include <string>

using namespace std;

namespace Metreos
{

namespace IPC
{

class CPPCORE_API MemoryStream
{
public:
    enum SeekOrigin
    {
        Begin,
        Current,
        End
    };

    MemoryStream(size_t initialCapacity = 128);
    ~MemoryStream();

    bool CanRead()
    {
        return true;
    }

    bool CanSeek()
    {
        return true;
    }

    bool CanWrite()
    {
        return true;
    }

    size_t Capacity()
    {
        return capacity;
    }

    void Flush()
    {
        // (Do nothing. Provided just to be complete.)
    }

    size_t Length()
    {
        return length;
    }

    size_t Position()
    {
        return position;
    }

    void Position(size_t newPosition)
    {
        position = newPosition < 0 ? 0 : newPosition;
    }

    char *GetBuffer()
    {
        return data;
    }

    char *GetBuffer(size_t &count)
    {
        count = capacity;
        return GetBuffer();
    }

    void Capacity(size_t newCapacity);
    size_t Read(char *buffer, size_t offset, size_t count);
    int ReadByte();
    long Seek(long offset, enum SeekOrigin loc);
    void SetLength(size_t newLength);   // TODO: Need to handle how this affects Position.
    char *ToArray(size_t &count);
    std::string ToString();
    void Write(char *buffer, size_t offset, size_t count);
    void WriteByte(char c);
    void WriteTo(MemoryStream &stream);

private:
    char *data;

    // Physical size of the buffer.
    size_t capacity;

    // Amount of data in the buffer (where next byte will be written).
    size_t length;

    // Position in buffer, advanced by reading.
    size_t position;

    // How much more to increase capacity when not big enough.
    // So we don't have to realloc every time we add stuff to stream.
    const static size_t ChunkSize = 64;

    // If buffer not big enough, make it that and then some.
    // This method does not change the values of the data--that must be
    // done by the caller.
    bool EnsureCapacity(size_t newLength);
};

} // namespace IPC
} // namespace Metreos

#endif // MEMORY_STREAM_H