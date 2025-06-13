#include "stdafx.h"
#include "ace/Log_Msg.h"
#include "ipc/MemoryStream.h"

using namespace Metreos::IPC;

MemoryStream::MemoryStream(size_t initialCapacity) :
    capacity(initialCapacity),
    length(0), 
    position(0) 
{
    data = (char *)ACE_OS::malloc(initialCapacity);
}

MemoryStream::~MemoryStream()
{
    ACE_OS::free(data);
}

size_t MemoryStream::Read(char *buffer, size_t offset, size_t count)
{
    ACE_ASSERT(data != NULL);
    ACE_ASSERT(length <= capacity);

    size_t bytesRead = 0;

    if (buffer != NULL)
    {
        if (position >= length) // Already at (or past) end of data
        {
            // (Do nothing.)
        }
        else if (position < length)
        {
            // Calculate maximum amount that we could read then
            // take the lesser of that and the requested number of
            // bytes to read.
            bytesRead = length - position;
            if (count < bytesRead)
            {
                bytesRead = count;
            }

            ACE_OS::memcpy(&buffer[offset], &data[position], bytesRead);

            position += bytesRead;
        }
    }

    return bytesRead;
}

int MemoryStream::ReadByte()
{
    char c;

    return Read(&c, 0, sizeof c) == 0 ? -1 : c;
}

long MemoryStream::Seek(long offset, enum SeekOrigin loc)
{
    switch (loc)
    {
    case Begin:
        Position(offset);
        break;

    case Current:
        Position(position + offset);
        break;

    case End:
        Position(length + offset);
        break;

    default:
        ACE_ASSERT(false);  // Can't happen.
    }

    if (position < 0)
    {
        position = 0;
    }

    return position;
}

void MemoryStream::SetLength(size_t newLength)
{
    ACE_ASSERT(data != NULL);
    ACE_ASSERT(length <= capacity);

    if (newLength > length)
    {
        EnsureCapacity(newLength);
        ACE_OS::memset(&data[length], 0, newLength - length);
    }
    else
    {
        if (position > newLength)
        {
            position = newLength;
        }
    }

    length = newLength;
}

char *MemoryStream::ToArray(size_t &count)
{
    char *p = new char[length];

    count = Read(p, 0, length);

    return p;
}

string MemoryStream::ToString()
{
    // TODO: Return buffer in hex or something.
    return "";
}

void MemoryStream::Write(char *buffer, size_t offset, size_t count)
{
    ACE_ASSERT(data != NULL);
    ACE_ASSERT(length <= capacity);

    if (buffer != NULL)
    {
        size_t oldPosition = position;
        size_t newPosition = position + count;

        if (newPosition > length)
        {
            EnsureCapacity(newPosition);
            length = newPosition;
        }

        ACE_OS::memcpy(&data[oldPosition], &buffer[offset], count);

        position = newPosition;
    }
}

void MemoryStream::WriteByte(char c)
{
    Write(&c, 0, sizeof c);
}

void MemoryStream::WriteTo(MemoryStream &stream)
{
    stream.Write(data, 0, length);
}

void MemoryStream::Capacity(size_t newCapacity)
{
    capacity = newCapacity;
    data = (char *)ACE_OS::realloc((char *)data, capacity);

    // TODO: Handle null pointer.

    if (length > capacity)
    {
        length = capacity;
    }

    if (position > length)
    {
        position = length;
    }
}

// If buffer not big enough, make it that and then some.
// This method does not change the values of the data--that must be
// done by the caller.
bool MemoryStream::EnsureCapacity(size_t newLength)
{
    ACE_ASSERT(data != NULL);
    ACE_ASSERT(length <= capacity);

    char *oldData = data;

    if (newLength > capacity)
    {
        Capacity(newLength + ChunkSize);
    }

    return data != oldData;
}
