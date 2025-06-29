#ifndef MtAppDialgo_H_LOADED
#define MtAppDialgo_H_LOADED

#include <string>
#include "dum/AppDialog.hxx"
#include "msgs/MessageConstants.h"

using namespace std;
using namespace resip;

namespace Metreos
{
namespace Sip
{
class MtAppDialog : public AppDialog
{
public:
	enum Action {
		none					= 0,
		need_to_provide_answer	= 1,
		need_to_provide_offer	= 2
	};

	MtAppDialog(HandleManager& ham) : AppDialog(ham), m_action(none), m_dialogEstablished(false), 
		m_waitForMoh(false), m_rxIpForResume(""), m_rxPortForResume(0), m_requestMov(MediaOption::sendrecv)
	{  
	}
	virtual ~MtAppDialog() 
	{ 
	}
	
	Action GetAction() { return m_action; };
	void SetAction(Action a) { m_action = a; };
	void SetDialogEstablished(bool b) { m_dialogEstablished = b; };
	bool IsDialogEstablished() { return m_dialogEstablished; };
	bool IsWaitingForMoh() { return m_waitForMoh; }
	void SetWaitForMoh(bool w) { m_waitForMoh = w; }

	bool IsWaitingForResumeAnswer() { return m_rxPortForResume != 0; };
	void InitialResumeRequested(string rxIp, int rxPort);
	void ClearResumeRequest();
	const string& ResumeIp() const { return m_rxIpForResume; };
	int ResumePort() const { return m_rxPortForResume; };

	void SetRequestMov(MediaOption::Value m) { m_requestMov = m; };
	MediaOption::Value GetRequestMov() { return m_requestMov; };
protected:
	Action				m_action;
	bool				m_dialogEstablished;
	bool				m_waitForMoh;
	MediaOption::Value	m_requestMov;			//media option from the request

	//because CallManager requires a null media ip (0.0.0.0) with inactive media attribute
	//before real media ip/port information, I have to save the real ip/port for resume 
	//from Telephony manager. I'll need them when the initial INVITE with null media ip
	//has been answered.
	string				m_rxIpForResume;		//the real ip for resume operation
	int					m_rxPortForResume;		//the real port for resume operation
};

inline void MtAppDialog::InitialResumeRequested(string rxIp, int rxPort)
{
	m_rxIpForResume = rxIp;
	m_rxPortForResume = rxPort;
}

inline void MtAppDialog::ClearResumeRequest()
{
	m_rxIpForResume = "";
	m_rxPortForResume = 0;
}

} //namespace Sip
}//namespace Metreos

#endif