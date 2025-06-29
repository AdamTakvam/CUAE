#include "stdafx.h"
#include "FlatMap.h"

#include "msgs/MessageConstants.h"
#include "MtSipMessage.h"

using namespace Metreos;
using namespace Metreos::Sip;
using namespace Metreos::Sip::Msgs;

MtSipMessage::MtSipMessage()
{
	m_type	= Msgs::Error;
	m_flags = 0;
	m_param = 0;
	m_data = NULL;
	m_length = 0;
}

MtSipMessage::MtSipMessage(unsigned int type, char* data, size_t len) : m_type(type), m_data(data), m_length(len)
{
	m_flags = 0;
	m_param = 0;
}

MtSipMessage::MtSipMessage(unsigned int type, FlatMapWriter* pfmw)
{
	m_type = type;
	m_flags = 0;
	m_param = 0;
	m_length = pfmw->length();
	m_data = new char[m_length];
	pfmw->marshal(m_data);
}

MtSipMessage::~MtSipMessage()
{
	delete[] m_data;
}

Metreos::FlatMapWriter *MtSipMessage::CreateResponse()
{
	FlatMapWriter *pw = new FlatMapWriter();
	return pw;
}
