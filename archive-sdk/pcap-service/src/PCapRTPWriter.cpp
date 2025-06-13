// PCapRTPWriter.cpp

#include "stdafx.h"

#ifdef WIN32
#ifdef PCAP_MEM_LEAK_DETECTION
#   define DEBUG_NEW new(_NORMAL_BLOCK, __FILE__, __LINE__)
#   define new DEBUG_NEW 
#endif
#endif

#include "PCapCommon.h"
#include "PCapRTPWriter.h"

#include <fstream>
//#include "g711.h"

using namespace Metreos;
using namespace Metreos::PCap;

PCapRTPWriter::PCapRTPWriter()
{
    pBuffer = NULL;
    pLoc = NULL;
    nAvailableLen = 0;  
    memset(&szFilePath, 0, sizeof(szFilePath));
}

PCapRTPWriter::~PCapRTPWriter()
{
    WriteInternalPacketBuffer();

    if (pBuffer)
    {
        delete [] pBuffer;
        pBuffer = NULL;
    }

    pLoc = NULL;

    nAvailableLen = 0;
}

bool PCapRTPWriter::CreateRTPFile(const char* folder, const u_int callIdentifier, const u_int dport)
{
    ResetInternalPacketBuffer();

    wsprintf(szFilePath, "%s\\%d-%d.rtp", folder, callIdentifier, dport);

    if (IsFileExist())
        return true;

    ofstream outfile;
    outfile.open(szFilePath, ios::out); 

    /*
    // write AU file header
    char c[1];
	*c = (unsigned char)0x2e;         
    outfile.write(c, 1);
	*c = (unsigned char)0x73;
    outfile.write(c, 1);
	*c = (unsigned char)0x6e;
    outfile.write(c, 1);
	*c = (unsigned char)0x64;
    outfile.write(c, 1);
	// header offset == 24 bytes
	*c = (unsigned char)0x00; 
    outfile.write(c, 1);
    outfile.write(c, 1);
    outfile.write(c, 1);
	*c = (unsigned char)0x18; 
    outfile.write(c, 1);
	// total length, it is permited to set this to 0xffffffff 
	*c = (unsigned char)0xff; 
    outfile.write(c, 1);
    outfile.write(c, 1);
    outfile.write(c, 1);
    outfile.write(c, 1);
	// encoding format == 8 bit ulaw
	*c = (unsigned char)0x00; 
    outfile.write(c, 1);
    outfile.write(c, 1);
    outfile.write(c, 1);
	*c = (unsigned char)0x01; 
    outfile.write(c, 1);
	// sample rate == 8000 Hz
	*c = (unsigned char)0x00; 
    outfile.write(c, 1);
    outfile.write(c, 1);
	*c = (unsigned char)0x1f; 
    outfile.write(c, 1);
	*c = (unsigned char)0x40; 
    outfile.write(c, 1);
	// channels == 1
	*c = (unsigned char)0x00; 
    outfile.write(c, 1);
    outfile.write(c, 1);
    outfile.write(c, 1);
	*c = (unsigned char)0x01; 
    outfile.write(c, 1);
    */

    outfile.close();

    return true;
}

bool PCapRTPWriter::WritePacket(const void* payload, int len)
{
    if (nAvailableLen <= len)
    {
        WriteInternalPacketBuffer();
        ResetInternalPacketBuffer();
    }

    memcpy(pLoc, payload, len);

    pLoc += len;

    nAvailableLen -= len;

    return true;
}

bool PCapRTPWriter::WriteInternalPacketBuffer()
{
    ACE_ASSERT(pBuffer);

    int len = MAX_BUFFER_SIZE - nAvailableLen;
    if (len == 0) // no data
        return false;

    ofstream outfile;
    outfile.open(szFilePath, ios::out | ios::ate | ios::app | ios::binary); 

    /*
    char *p = pBuffer;
    for (int i=0; i<len; i++)
    {
        int n = ulaw2linear((unsigned char)*p);
        char c = (unsigned char)linear2ulaw(n);
        outfile.write(&c, 1);

        p++;
    }
    */

    outfile.write(pBuffer, len);

    outfile.close();
    return true;
}

void PCapRTPWriter::ResetInternalPacketBuffer()
{
    if (pBuffer)
    {
        delete [] pBuffer;
        pBuffer = NULL;
    }

    pBuffer = new char[MAX_BUFFER_SIZE];
    memset(pBuffer, 0, MAX_BUFFER_SIZE);

    pLoc = pBuffer;
    nAvailableLen = MAX_BUFFER_SIZE;    
}

bool PCapRTPWriter::IsFileExist()
{
    long l,m;
    ifstream file (szFilePath, ios::in|ios::binary);
    l = file.tellg();
    file.seekg (0, ios::end);
    m = file.tellg();
    file.close();

    return ((m-l) > 0);
}
