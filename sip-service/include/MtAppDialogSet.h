#ifndef MtAppDialogSet_H_LOADED
#define MtAppDialogSet_H_LOADED

#include <string>

#include "stack/SdpContents.hxx"
#include "dum/AppDialogSet.hxx"
#include "dum/DialogUsageManager.hxx"

#include "MtAppDialog.h"

using namespace std;

namespace Metreos
{
namespace Sip

{
class MtAppDialogSet : public resip::AppDialogSet
{
private:
	const static int UNKNOWN = 0;
	const static int INACTIVE = 1;
	const static int ALERTING = 2;
	const static int DIALING = 4;
	const static int DISCONNECTED = 8; // and failed
	const static int ESTABLISHED = 16;
	const static int INITIATED = 32;
	const static int OFFERED = 64;
	const static int ANSWERED = 128;
	
	const static int CONN_STATES = INACTIVE|ALERTING|DIALING|DISCONNECTED|ESTABLISHED|INITIATED|OFFERED;

	static string xlatConnState( int state )
	{
		switch (state)
		{
			case INACTIVE: return "INACTIVE";
			case ALERTING: return "ALERTING";
			case DIALING: return "DIALING";
			case DISCONNECTED: return "DISCONNECTED";
			case ESTABLISHED: return "ESTABLISHED";
			case INITIATED: return "INITIATED";
			case OFFERED: return "OFFERED";
			default: return "unknownConnState"+state;
		}
	}

public:
	MtAppDialogSet(resip::DialogUsageManager& dum, long cid) : AppDialogSet(dum), m_callId(cid), m_state(UNKNOWN)
	{  
	}
	virtual ~MtAppDialogSet() 
	{
	}

    virtual void destroy();

	virtual AppDialog* createAppDialog(const SipMessage& msg);

	virtual SharedPtr<UserProfile> selectUASUserProfile(const SipMessage& msg);
	
	long CallId() const;
	void CallId(long);

protected:
	//m_psdp contains all the necessary information to create a session
	long m_callId;
	unsigned int m_state;
/*
	//from
	string from;

	//to: fully qualified sip address
	string to;

	//parameters for connection field in sdp (c)
	string	txIP;
	uint	txPort;

	//media info
	//list of codec and frame size
	std::vector<MtMediaInfo>	mediaInfos;
*/		

};

inline long MtAppDialogSet::CallId() const
{
	return m_callId;
}

inline void MtAppDialogSet::CallId(long cid)
{
	m_callId = cid;
}

/*
class MtMediaInfo 
{
public:
	MtMediaInfo(std::string c, uint fs) : codec(c), frameSize(fs)
	{
	}
	virtual ~MtMediaInfo()
	{
	}

	string Codec()
	{
		return codec;
	}

	uint FrameSize()
	{
		return frameSize;
	}

protected:
	string		codec;
	uint		frameSize;
}

*/
}//namespace Sip
}//namespace Metreos
#endif