#ifndef SendDigitsCmd_H_LOADED
#define SendDigitsCmd_H_LOADED

#include "resip/dum/DumCommand.hxx"
#include "resip/dum/DialogUsageManager.hxx"

using namespace std;
using namespace resip;

namespace Metreos
{
namespace Sip
{

class MtSipStackRuntime;

class SendDigitsCmd : public DumCommand
{
public:
	SendDigitsCmd(MtSipStackRuntime *pRuntime, 
					const string& stackCallId,
					const string& digits);
	SendDigitsCmd(const SendDigitsCmd& cmd);
	virtual ~SendDigitsCmd();

	virtual void executeCommand();

	Message* clone() const;
	ostream& encode(ostream& strm) const;
    ostream& encodeBrief(ostream& strm) const;

protected:
	MtSipStackRuntime		*m_pRuntime;
	string					m_stackCallId;
	string					m_digits;
};

}

}

#endif