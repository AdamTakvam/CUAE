#ifndef LOGGER_TASK_H
#define LOGGER_TASK_H

#include "H323Common.h"
#include <fstream>

#include "ace/Message_Block.h"

namespace Metreos
{

namespace H323
{

class LoggerTask : public ACE_Task<ACE_MT_SYNCH>
{
public:
    static LoggerTask* Instance();
    void Write(const char* msg, ...);
    void SetDirectory(const char* dir);

protected:
    LoggerTask(bool logToConsole, bool logToFile);
    virtual int svc(void);
    void FormatFilename();

    static LoggerTask*  m_instance;
    bool         m_logToConsole;
    bool         m_logToFile;
    std::string  m_filename;
    std::string  m_directory;
    std::fstream m_fileStream;
    int          m_fileLogLineCount;
};

class LogMsg : public ACE_Message_Block
{
public:
    LogMsg(ACE_Allocator* a = 0);
    LogMsg(const char *data, size_t size = 0);
    LogMsg(const ACE_Message_Block &mb, size_t align);
    LogMsg(size_t size, ACE_Message_Type type = MB_DATA, ACE_Message_Block* cont = 0, const char* data = 0);

    virtual ~LogMsg();

    void  msgInit();

    int isEmpty() const                 { return this->base() == 0; }
    char* msgData() const               { return this->m_data; }
    ACE_Time_Value timestamp() const    { return this->m_timestamp; }

protected:
    char*           m_data;
    ACE_Time_Value  m_timestamp;
};

} // namespace H323
} // namespace Metreos

#endif // LOGGER_TASK_H