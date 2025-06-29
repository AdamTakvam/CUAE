#include "StdAfx.h"
#include "ControlMessage.h"

ControlMessage::ControlMessage() :
    record(false)
{}

ControlMessage::ControlMessage(const std::string& user, 
                               const std::string& pass, 
                               const std::string& emailAddr, 
                               bool rec) :
    username(user),
    password(pass),
    email(emailAddr),
    record(rec)
{}

ControlMessage::~ControlMessage()
{
    CalleeInfo_list_const_iterator i;

    for(i = callees.begin(); i != callees.end(); i++)
    {
        if(*i != 0)
        {
            delete *i;
        }
    }

    callees.clear();
}

void ControlMessage::SetEmail(const std::string& emailNew)
{
    email = emailNew;
}

void ControlMessage::SetRecord(bool recordNew)
{
    record = recordNew;
}

void ControlMessage::SetAuth(const std::string& user, const std::string& pass)
{
    username = user;
    password = pass;
}

void ControlMessage::AddCallee(const std::string& name, const std::string& number)
{
    CalleeInfo* info = new CalleeInfo;

    info->name = name;
    info->number = number;

    callees.push_back(info);
}

std::string ControlMessage::ToXmlString() const
{
    std::string xmlMsg;

    // Build the message header.
    // Includes everything up until the first 'callee' statement.
    xmlMsg = GenerateHeaderXml();
    
    // Add entries for each of our callees.
    CalleeInfo_list_const_iterator i;
    for(i = callees.begin(); i != callees.end(); i++)
    {
        xmlMsg += GenerateCalleeXml(*i);
    }

    // Build the message footer.
    xmlMsg += GenerateFooterXml();

    return xmlMsg;
}

std::string ControlMessage::GenerateHeaderXml() const
{
    std::string xmlMsg;

    xmlMsg += HEADER;
    xmlMsg += "\n";

    // Generate: '<initiateCall '
    xmlMsg += INITIATE_CALL_START;

    // Generate: 'username="someUser" '
    xmlMsg += USERNAME;
    xmlMsg += username;
    xmlMsg += END_QUOTE_SPACE;

    // Generate: 'password="somePassword" '
    xmlMsg += PASSWORD;
    xmlMsg += password;
    xmlMsg += END_QUOTE_SPACE;

    // Generate: 'record="true" ' 
    xmlMsg += RECORD;
    xmlMsg += record ? "true" : "false";
    xmlMsg += END_QUOTE_SPACE;

    // Generate: 'email="someEmail" '
    xmlMsg += EMAIL;
    xmlMsg += email;
    xmlMsg += END_QUOTE_SPACE;

    // Generate: 'xmlns="http://metreos.com/InitiateCall.xsd">'
    xmlMsg += SCHEMA;
    xmlMsg += RIGHT_BRACKET;

    xmlMsg += "\n";

    return xmlMsg;
}

std::string ControlMessage::GenerateFooterXml() const
{
    std::string xmlMsg;

    // Generate: '</initiateCall>'
    xmlMsg = INITIATE_CALL_END;
    xmlMsg += "\n";

    return xmlMsg;
}

std::string ControlMessage::GenerateCalleeXml(CalleeInfo* callee) const
{
    std::string xmlMsg;

    // Generate: '<callee name="someName">12345</callee>'
    xmlMsg = CALLEE_START;
    xmlMsg += NAME;
    xmlMsg += callee->name;
    xmlMsg += END_QUOTE_RIGHT_BRACKET;
    xmlMsg += callee->number;
    xmlMsg += CALLEE_END;

    xmlMsg += "\n";

    return xmlMsg;
}