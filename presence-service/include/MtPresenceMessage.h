/**
 * The primary message class passed amongst the various tasks throughout the
 * SIP stack process.  
 *
 */

#ifndef MtPresenceMessage_H_LOADED
#define MtPresenceMessage_H_LOADED

#include "ace/Message_Block.h"


namespace Metreos
{
class FlatMapWriter;

namespace Presence
{
class MtPresenceMessage : public ACE_Message_Block
{
public:
	MtPresenceMessage();
	MtPresenceMessage(unsigned int type, char* data = NULL, size_t len = 0);
	MtPresenceMessage(unsigned int type, FlatMapWriter* pfmw);

	virtual ~MtPresenceMessage();

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

inline unsigned int MtPresenceMessage::Type() const
{
	return m_type;
}

inline char* MtPresenceMessage::Payload()
{
	return m_data;
}

}
}

#endif