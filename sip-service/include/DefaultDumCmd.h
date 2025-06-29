#ifndef DefaultDumCmd_H_LOADED
#define DefaultDumCmd_H_LOADED

#include "resip/dum/DumCommand.hxx"
#include "resip/dum/DialogUsageManager.hxx"

using namespace std;
using namespace resip;

namespace Metreos
{
namespace Sip
{

class MtSipStackRuntime;

class DefaultDumCmd : public DumCommand
{
public:
	enum CommandId
	{
		Reject		= 10,
		Hangup		= 11,
		Hold		= 12,
		Resume		= 13,
		Refer		= 14,
		Redirect	= 15
	};

	DefaultDumCmd(CommandId cmdId,
					MtSipStackRuntime *pRuntime, 
					DialogUsageManager *pDum, 
					string stackCallId,
					NameAddr* pnaTo = NULL,
					char *pszNewRxip = NULL,
					int *pNewRxPort = NULL);
	DefaultDumCmd(const DefaultDumCmd& cmd);
	virtual ~DefaultDumCmd();

	virtual void executeCommand();

	Message* clone() const;
	ostream& encode(ostream& strm) const;
    ostream& encodeBrief(ostream& strm) const;

protected:
	CommandId				m_cmdId;
	MtSipStackRuntime		*m_pRuntime;
	DialogUsageManager		*m_pDum;
	string					m_stackCallId;
	NameAddr				*m_pnaTo;
	string					m_newRxIp;
	int						m_newRxPort;
};

}

}

#endif