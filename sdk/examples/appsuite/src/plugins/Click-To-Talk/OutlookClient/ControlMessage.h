#ifndef __CONTROL_MESSAGE_H__
#define __CONTROL_MESSAGE_H__

#include <list>
#include <string>

const char HEADER[]                 = "<?xml version=\"1.0\" encoding=\"utf-8\" ?>";
const char SCHEMA[]                 = "xmlns=\"http://metreos.com/InitiateCall.xsd\"";
const char RIGHT_BRACKET[]          = ">";
const char INITIATE_CALL_START[]    = "<initiateCall ";
const char INITIATE_CALL_END[]      = "</initiateCall>";
const char CALLEE_START[]           = "<callee ";
const char CALLEE_END[]             = "</callee>";
const char USERNAME[]               = "username=\"";
const char PASSWORD[]               = "password=\"";
const char RECORD[]                 = "record=\"";
const char EMAIL[]                  = "email=\"";
const char NAME[]                   = "name=\"";
const char END_QUOTE_SPACE[]        = "\" ";
const char END_QUOTE_RIGHT_BRACKET[] = "\">";

struct CalleeInfo
{
    std::string name;
    std::string number;
};

typedef std::list<CalleeInfo*>              CalleeInfo_list;
typedef CalleeInfo_list::const_iterator     CalleeInfo_list_const_iterator;

class ControlMessage
{
public:
    ControlMessage();
    ControlMessage(const std::string& user, const std::string& pass, 
        const std::string& emailAddr, bool rec);

    virtual ~ControlMessage();

    void SetEmail(const std::string& emailNew);
    void SetRecord(bool recordNew);
    void SetAuth(const std::string& user, const std::string& pass);

    void AddCallee(const std::string& name, const std::string& number);

    std::string ToXmlString() const;

protected:
    std::string GenerateHeaderXml() const;
    std::string GenerateFooterXml() const;
    std::string GenerateCalleeXml(CalleeInfo* callee) const;

    CalleeInfo_list     callees;
    std::string         email;
    std::string         username;
    std::string         password;
    bool                record;
};

#endif // __CONTROL_MESSAGE_H__