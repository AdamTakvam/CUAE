/**
 * The primary message class passed amongst the various tasks throughout the
 * SIP stack process.  
 *
 */

#ifndef MtSipMessage_H_LOADED
#define MtSipMessage_H_LOADED

#include "ace/Message_Block.h"


namespace Metreos
{
class FlatMapWriter;

namespace Sip
{
class MtSipMessage : public ACE_Message_Block
{
public:
	MtSipMessage();
	MtSipMessage(unsigned int type, char* data = NULL, size_t len = 0);
	MtSipMessage(unsigned int type, FlatMapWriter* pfmw);

	virtual ~MtSipMessage();

	unsigned int Type() const;
	char* Payload();


	FlatMapWriter* CreateResponse();

protected:
	unsigned int	m_type;
	long			m_param;
	unsigned int	m_flags;
	char*			m_data;
	size_t			m_length;


};

inline unsigned int MtSipMessage::Type() const
{
	return m_type;
}

inline char* MtSipMessage::Payload()
{
	return m_data;
}

}
}

#endif