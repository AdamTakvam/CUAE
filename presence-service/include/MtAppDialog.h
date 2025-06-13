#ifndef MtAppDialgo_H_LOADED
#define MtAppDialgo_H_LOADED

#include <string>
#include "dum/AppDialog.hxx"
#include "msgs/MessageConstants.h"

using namespace std;
using namespace resip;

namespace Metreos
{
namespace Presence
{
class MtAppDialog : public AppDialog
{
public:
	enum Action {
		none					= 0,
		need_to_provide_answer	= 1,
		need_to_provide_offer	= 2
	};

	MtAppDialog(HandleManager& ham) : AppDialog(ham)
	{  
	}
	virtual ~MtAppDialog() 
	{ 
	}
	
	Action GetAction() { return m_action; };
	void SetAction(Action a) { m_action = a; };
	void SetDialogEstablished(bool b) { m_dialogEstablished = b; };
	bool IsDialogEstablished() { return m_dialogEstablished; };

protected:
	Action				m_action;
	bool				m_dialogEstablished;

};

} //namespace Sip
}//namespace Metreos

#endif