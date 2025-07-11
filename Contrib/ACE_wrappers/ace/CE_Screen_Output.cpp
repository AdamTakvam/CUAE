// -*- C++ -*-
// CE_Screen_Output.cpp,v 1.3 2002/06/09 22:09:17 schmidt Exp

#include "ace/CE_Screen_Output.h"
#if defined (ACE_HAS_WINCE)

#include "ace/Log_Msg.h"

ACE_CE_Screen_Output::ACE_CE_Screen_Output(HWND hEdit)
: handler_(hEdit)
, pFile_(0)
{
}

ACE_CE_Screen_Output::ACE_CE_Screen_Output()
: handler_(0)
, pFile_(0)
{
}

ACE_CE_Screen_Output::~ACE_CE_Screen_Output()
{
    if (pFile_ != 0) {
        fclose(pFile_);
    }
}

void ACE_CE_Screen_Output::log(ACE_Log_Record &log_record)
{
    ACE_TCHAR verbose_msg[ACE_Log_Record::MAXVERBOSELOGMSGLEN];
    int result = log_record.format_msg (ACE_LIB_TEXT("WindozeCE"),  // host name
                                        0,                          // verbose flag
                                        verbose_msg);

    if (result == 0)
    {
        verbose_msg[ ACE_OS::strlen(verbose_msg) - 1 ] = 0;  // CE does not like '\n' by itself.
        *this << verbose_msg << endl;
    }
}

void ACE_CE_Screen_Output::SetOutputWindow(HWND hEdit)
{
    handler_ = hEdit;
}

void ACE_CE_Screen_Output::clear()
{
    SetWindowText(handler_, 0);
}

ACE_CE_Screen_Output& ACE_CE_Screen_Output::operator << (ACE_TCHAR* output)
{
    int length = GetWindowTextLength(handler_);
    SendMessage(handler_, EM_SETSEL, length, length);
    SendMessage(handler_, EM_REPLACESEL, 0, (LPARAM)output);

    if (pFile_ != 0)
    {
        fwprintf(pFile_, L"%s", output);
    }

    return *this;
}

ACE_CE_Screen_Output& ACE_CE_Screen_Output::operator << (const ACE_TCHAR* output)
{
    ACE_TCHAR* buffer = ACE_OS::strdup(output);
    if (buffer != 0)
    {
        *this << buffer;
        delete buffer;
    }
    return *this;
}

ACE_CE_Screen_Output& ACE_CE_Screen_Output::operator << (ACE_ANTI_TCHAR* output)
{
    *this << ACE_TEXT_CHAR_TO_TCHAR(output);
    return *this;
}

ACE_CE_Screen_Output& ACE_CE_Screen_Output::operator << (const ACE_ANTI_TCHAR* output)
{
    *this << ACE_TEXT_CHAR_TO_TCHAR(output);
    return *this;
}

ACE_CE_Screen_Output& ACE_CE_Screen_Output::operator << (char output)
{
    *this << (int)output;
    return *this;
}

ACE_CE_Screen_Output& ACE_CE_Screen_Output::operator << (unsigned char output)
{
    *this << (int)output;
    return *this;
}

ACE_CE_Screen_Output& ACE_CE_Screen_Output::operator << (unsigned short output)
{
    ACE_TCHAR buffer[20];
    wsprintf(buffer, ACE_LIB_TEXT("%u"), output);
    *this << buffer;
    return *this;
}

ACE_CE_Screen_Output& ACE_CE_Screen_Output::operator << (int output)
{
    ACE_TCHAR buffer[20];
    wsprintf(buffer, ACE_LIB_TEXT("%d"), output);
    *this << buffer;
    return *this;
}

ACE_CE_Screen_Output& ACE_CE_Screen_Output::operator << (unsigned int output)
{
    ACE_TCHAR buffer[20];
    wsprintf(buffer, ACE_LIB_TEXT("%du"), output);
    *this << buffer;
    return *this;
}

ACE_CE_Screen_Output& ACE_CE_Screen_Output::operator << (float output)
{
    ACE_TCHAR buffer[20];
    swprintf(buffer, ACE_LIB_TEXT("%f"), output);
    *this << buffer;
    return *this;
}

ACE_CE_Screen_Output& ACE_CE_Screen_Output::operator << (long output)
{
    ACE_TCHAR buffer[20];
    wsprintf(buffer, ACE_LIB_TEXT("%l"), output);
    *this << buffer;
    return *this;
}

ACE_CE_Screen_Output& ACE_CE_Screen_Output::operator << (unsigned long output)
{
    ACE_TCHAR buffer[20];
    wsprintf(buffer, ACE_LIB_TEXT("%lu"), output);
    *this << buffer;
    return *this;
}

ACE_CE_Screen_Output& ACE_CE_Screen_Output::operator << (FILE* pFile)
{
    pFile_ = pFile;
    return *this;
}

#endif  // ACE_HAS_WINCE
