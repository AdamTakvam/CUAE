#include "LoggerTask.h"
#include <fstream>

using namespace Metreos;
using namespace Metreos::H323;

LoggerTask* LoggerTask::m_instance = 0;

LoggerTask::LoggerTask(bool logToConsole, bool logToFile) :
    m_fileLogLineCount(0),
    m_logToConsole(logToConsole),
    m_logToFile(logToFile)
{
}

LoggerTask* LoggerTask::Instance()
{
    if(m_instance == 0)
    {
        m_instance = new LoggerTask(true, true);
    }

    return m_instance;
}

int LoggerTask::svc(void)
{
    ACE_Message_Block* msg;
    while(getq(msg) != -1)
    {
        LogMsg* logMsg = dynamic_cast<LogMsg*>(msg);
        ACE_ASSERT(logMsg != 0);

        if(m_fileStream.is_open() == false)
        {
            FormatFilename();
            m_fileStream.open(m_filename.c_str(), ios_base::out | ios_base::trunc);
        }

        ACE_TCHAR buf[2048];

        static char* masktimestamp = ACE_LIB_TEXT("%s.%03ld");
        static char* maskstamped   = ACE_LIB_TEXT("%s %s");

        ACE_TCHAR timestamp[26]; // 0123456789012345678901234 
        int  length = 0;         // Oct 18 14:25:36.000 2003<nul>

        time_t now = logMsg->timestamp().sec();
        ACE_TCHAR ctp[26];
        ACE_OS::ctime_r(&now, ctp, sizeof ctp);

        ctp[19] = '\0';     // terminate after time
        ctp[24] = '\0';     // terminate after date

        ACE_OS::sprintf(timestamp, masktimestamp, ctp+4, (logMsg->timestamp().usec())/1000); 
        length = ACE_OS::sprintf(buf, maskstamped, timestamp, logMsg->msgData());

        if(m_logToConsole)
            std::cout << buf+6 << std::endl;

        if(m_logToFile)
        {
            m_fileStream << buf << std::endl;
            m_fileLogLineCount++;

            if(m_fileLogLineCount > 8000)
            {
                m_fileLogLineCount = 0;
                m_fileStream.close();
            }
        }

        delete logMsg;
        msg = 0;
    }

    return 0;
}

void LoggerTask::SetDirectory(const char* dir)
{
    ACE_ASSERT(dir != 0);
    this->m_directory = dir;
}

void LoggerTask::FormatFilename()
{
    ACE_Time_Value t = ACE_OS::gettimeofday();
    std::strstream name;

    if(m_directory.empty() == false)
        name << m_directory;

    name << "h323-" << t.msec() << "-" << t.usec() << ".log" << std::ends;

    m_filename = name.str();
}

void LoggerTask::Write(const char* fmt, ...)
{
    ACE_TCHAR* msg = new ACE_TCHAR[1024];

    va_list ap;
    va_start(ap, fmt);
    ACE_OS::vsprintf(msg, fmt, ap);
    va_end(ap);

    LogMsg* logMsg = new LogMsg(msg);
    this->putq(logMsg);
}

LogMsg::LogMsg(ACE_Allocator* a) : ACE_Message_Block(a) 
{ 
    this->msgInit();
}

LogMsg::LogMsg(const char *data, size_t size) : ACE_Message_Block(data, size) 
{ 
    this->msgInit();

    this->m_data = (char*)data;
}

LogMsg::LogMsg(size_t size, ACE_Message_Type type, ACE_Message_Block* cont, 
               const char* data) : ACE_Message_Block(size, type, cont, data) 
{ 
    this->msgInit();
}

LogMsg::LogMsg(const ACE_Message_Block &mb, size_t align) : ACE_Message_Block(mb,align) 
{ 
    this->msgInit();
}

LogMsg::~LogMsg()
{
    if(m_data   != 0) delete[] m_data;
}

void LogMsg::msgInit()  
{ 
    this->m_data = 0;
    m_timestamp = ACE_OS::gettimeofday(); 
}
