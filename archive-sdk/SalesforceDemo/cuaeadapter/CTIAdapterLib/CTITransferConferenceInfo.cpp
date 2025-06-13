#include "StdAfx.h"
#include "ctitransferconferenceinfo.h"
#include "CTIStatic.h"
#include "CTIInfoField.h"
#include ".\include\ctitransferconferenceinfo.h"

CCTITransferConferenceInfo::CCTITransferConferenceInfo()
:m_nTransferOrConference(USERINFO_NONE),
m_pTransferredBy(NULL),
m_pConferencedBy(NULL),
m_pPartyName(NULL),
m_pPartyExtension(NULL)
{
	SetVisible(false);
}

CCTITransferConferenceInfo::~CCTITransferConferenceInfo(void)
{
}

void CCTITransferConferenceInfo::CreateChildren(void)
{
	m_pTransferredBy = AddStatic("TRANSFERRED_BY");
	m_pConferencedBy = AddStatic("CONFERENCED_BY");
	m_pPartyName = AddInfoField("NAME");
	m_pPartyExtension = AddInfoField("EXTENSION");
}

void CCTITransferConferenceInfo::SetTransferOrConference(int nTransferOrConference) { 
	m_nTransferOrConference = nTransferOrConference;
	m_pTransferredBy->SetVisible(m_nTransferOrConference==USERINFO_TRANSFER);
	m_pConferencedBy->SetVisible(m_nTransferOrConference==USERINFO_CONFERENCE);
}

void CCTITransferConferenceInfo::SetPartyName(std::string sPartyName)
{
	m_pPartyName->SetVisible(sPartyName.length()!=0);
	m_pPartyName->SetValue(sPartyName);
}

void CCTITransferConferenceInfo::SetPartyExtension(std::string sPartyExtension)
{
	m_pPartyExtension->SetVisible(sPartyExtension.length()!=0);
	m_pPartyExtension->SetValue(sPartyExtension);
}
