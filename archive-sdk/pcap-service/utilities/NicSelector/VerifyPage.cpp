// HardwarePage.cpp : implementation file
//

#include "stdafx.h"
#include "NicSelector.h"
#include "VerifyPage.h"
#include "MasterDlg.h"

#include "pcap.h"

#ifdef _DEBUG
#define new DEBUG_NEW
#undef THIS_FILE
static char THIS_FILE[] = __FILE__;
#endif

/////////////////////////////////////////////////////////////////////////////
// CVerifyPage dialog


CVerifyPage::CVerifyPage(CWnd* pParent /*=NULL*/)
	: CNewWizPage(CVerifyPage::IDD, pParent)
{
	//{{AFX_DATA_INIT(CVerifyPage)
		// NOTE: the ClassWizard will add member initialization here
	//}}AFX_DATA_INIT

    m_bVerified = FALSE;
    m_bVerifying = FALSE;
    m_iTesingNic = 0;
}


void CVerifyPage::DoDataExchange(CDataExchange* pDX)
{
    CDialog::DoDataExchange(pDX);
    //{{AFX_DATA_MAP(CVerifyPage)
    //}}AFX_DATA_MAP
    DDX_Control(pDX, IDC_LIST_STEPS, m_ListLogs);
}


BEGIN_MESSAGE_MAP(CVerifyPage, CNewWizPage)
	//{{AFX_MSG_MAP(CVerifyPage)
	//}}AFX_MSG_MAP
    ON_BN_CLICKED(CB_SKIP_TEST, OnSkipTestClicked)
    ON_BN_CLICKED(IDC_BUTTON_TEST, OnBnClickedButtonTest)
END_MESSAGE_MAP()

/////////////////////////////////////////////////////////////////////////////
// CVerifyPage message handlers

BOOL CVerifyPage::OnInitDialog() 
{
	CNewWizPage::OnInitDialog();
	
	return TRUE;  // return TRUE unless you set the focus to a control
	              // EXCEPTION: OCX Property Pages should return FALSE
}

void CVerifyPage::OnSetActive()
{
    if (!m_pParent)
        return;

    if (((CMasterDlg*)m_pParent)->m_bAutoMode)
    {
        CheckDlgButton(CB_SKIP_TEST, 0);
        GetDlgItem(CB_SKIP_TEST)->EnableWindow(0);
    }
    else
    {
        GetDlgItem(CB_SKIP_TEST)->EnableWindow(1);
    }

    RemoveLogs();
}

BOOL CVerifyPage::OnKillActive()
{
    //RemoveLogs();

	return TRUE;
}

BOOL CVerifyPage::OnWizardFinish()
{
    CString sTitle;
    sTitle.LoadString(IDS_APP_TITLE);

    CString sMsg;
    if (this->m_bVerifying)
    {
        sMsg.LoadString(IDS_PLEASE_WAIT);
        MessageBox(sMsg, sTitle, MB_OK|MB_ICONEXCLAMATION);
        return FALSE;
    }

    if (!IsDlgButtonChecked(CB_SKIP_TEST) && !m_bVerified)
    {
        sTitle.LoadString(IDS_APP_TITLE);
        sMsg.LoadString(IDS_CHECK_AGAIN);
        MessageBox(sMsg, sTitle, MB_OK|MB_ICONEXCLAMATION);
        return FALSE;
    }

    UpdateConfig();

	return TRUE;
}

void CVerifyPage::OnSkipTestClicked()
{
    if (m_pParent)
        m_pParent->EnableFinish(IsDlgButtonChecked(CB_SKIP_TEST));

    if (m_iTesingNic == 0)
        m_iTesingNic = ((CMasterDlg*)m_pParent)->m_iNicIndex;
}

UINT PCapTestProc(LPVOID pParam)
{
    CVerifyPage* pObject = (CVerifyPage*)pParam;

    if (pObject == NULL)
        return 1;   

    pObject->m_bVerifying = TRUE;
    BOOL bFound = FALSE;
    do
    {
        pcap_if_t *alldevs;
        char errbuf[PCAP_ERRBUF_SIZE];

        if (pcap_findalldevs(&alldevs, errbuf) == -1)
        {
            pObject->m_bVerifying = FALSE;     
            return 0;
        }
        
        // Go through each device to find the pre-defined interface
        int numNics = 0;
        pcap_if_t *d;
        for(d=alldevs; d; d=d->next)
            ++numNics;
        
        if(numNics==0)
        {
            pObject->m_bVerifying = FALSE;  
            pcap_freealldevs(alldevs);
            return 0;
        }

        int inum = pObject->m_iTesingNic;

        if (inum > numNics)
        {
            pObject->m_bVerifying = FALSE;     
            pcap_freealldevs(alldevs);
            return 0;
        }

        // Jump to the selected adapter
        int i = 0;
        for(d=alldevs, i=0; i<inum-1 ;d=d->next, i++);
        
        // Open the device
        pcap_t *adhandle;
        if ((adhandle= pcap_open_live( d->name,                         // name of the device
                                        65536,                          // portion of the packet to capture. 
                                                                        // 65536 guarantees that the whole packet will be captured on all the link layers
                                        1,                              // promiscuous mode
                                        500,                            // read timeout
                                        errbuf                          // error buffer
                                        )) == NULL)
        {
            pcap_freealldevs(alldevs);
            pObject->m_bVerifying = FALSE;     
            return 0;
        }

	    // Check the link layer. We support only Ethernet for simplicity.
	    if(pcap_datalink(adhandle) != DLT_EN10MB)
	    {
            pcap_freealldevs(alldevs);
            pObject->m_bVerifying = FALSE;     
            return 0;
	    }

	    u_int netmask;
	    if(d->addresses != NULL)
		    // Retrieve the mask of the first address of the interface
		    netmask = ((struct sockaddr_in *)(d->addresses->netmask))->sin_addr.S_un.S_addr;
	    else
		    // If the interface is without addresses we suppose to be in a C class network
		    netmask=0xffffff; 

	    //compile the filter
	    char packet_filter[] = "(udp or (tcp port 2000)) or (vlan and (udp or tcp port 2000))";
	    struct bpf_program fcode;
	    if (pcap_compile(adhandle, &fcode, packet_filter, 1, netmask) <0)
	    {
            pcap_freealldevs(alldevs);
            pObject->m_bVerifying = FALSE;     
            return 0;
	    }

	    //set the filter
	    if (pcap_setfilter(adhandle, &fcode)<0)
	    {
            pcap_freealldevs(alldevs);
            pObject->m_bVerifying = FALSE;     
            return 0;
	    }

        // At this point, we don't need any more the device list.
        pcap_freealldevs(alldevs);
        
        // Retrieve the packets for the life of the thread
        int res = -1;
        const u_char *pkt_data;
        struct pcap_pkthdr *header;

        DWORD dwStart = GetTickCount();
        while(res = pcap_next_ex(adhandle, &header, &pkt_data) >= 0)
        {    
            if((GetTickCount()-dwStart) >= 10*1000)
                break;

            // Timeout elapsed        
            if(res == 0) 
                continue;

            if (header->len == 0)
                continue;

            char* packetData = new char[header->len];
            memset(packetData, 0, header->len);
            memcpy(packetData, pkt_data, header->len); 

            int el = 0;
            ethernet_header* eh = (ethernet_header*)packetData;
            if (eh->ether_type == 8)            // 0x08
                el = 14;            // IP
            else if (eh->ether_type == 129)     // 0x81
                el = 18;            // VLAN
            else
                continue;

	        ip_header* ih = (ip_header*)(packetData + el);  
            
            // retrieve tcp header
	        int ih_len = (ih->ver_ihl & 0xf) * 4;
	        tcp_header* th = (tcp_header*)((u_char*)ih + ih_len);

            int th_len = TH_OFF(th) * 4;

	        // convert from network byte order to host byte order
	        u_short sport = ntohs(th->sport);
	        u_short dport = ntohs(th->dport);

            if (sport == SKINNY_PORT || dport == SKINNY_PORT)
            {
                bFound = pObject->ProcessSkinnyPacket(packetData, header->len, el, ih_len, th_len);    // el is MAC header + VLAN tag
            }

            if (packetData)
                delete [] packetData;
            
            if (bFound)
                break;
        }

        pcap_close(adhandle);

        if (bFound)
            break;

        if (((CMasterDlg*)pObject->m_pParent)->m_bAutoMode && pObject->m_iTesingNic > 1)
        {
            pObject->m_iTesingNic--;
            pObject->SkinnyNotFound(FALSE);
        }
        else
            break;
    } while (!bFound);

    pObject->m_bVerifying = FALSE;     
    if (!bFound)
        pObject->SkinnyNotFound(TRUE);
    else
        pObject->SkinnyFound();

    return 0;
}

void CVerifyPage::OnBnClickedButtonTest()
{
    GetDlgItem(CB_SKIP_TEST)->EnableWindow(0);
    m_iTesingNic = ((CMasterDlg*)m_pParent)->m_iNicIndex;
    RemoveLogs();
    DoTest(TRUE);
}

void CVerifyPage::DoTest(BOOL bNew)
{
    // Diable all buttons
    if (m_pParent)
    {
        GetDlgItem(IDC_BUTTON_TEST)->EnableWindow(0);
        m_pParent->EnableBack(FALSE);
    }

    // Start test now...
    m_bVerified = FALSE;

    CString sMsg;

    sMsg = "====================";
    AddMessage(sMsg);

    sMsg.LoadString(IDS_START_TESTING);
    sMsg.FormatMessage(sMsg, m_iTesingNic);
    AddMessage(sMsg);

    sMsg.LoadString(IDS_OFFHOOK);
    AddMessage(sMsg);

    if (bNew)
        AfxBeginThread(PCapTestProc, (LPVOID)this);
}

void CVerifyPage::SkinnyFound()
{
    if (m_pParent)
        m_pParent->EnableBack(TRUE);

    GetDlgItem(IDC_BUTTON_TEST)->EnableWindow(1);
    GetDlgItem(CB_SKIP_TEST)->EnableWindow(1);

    m_bVerified = TRUE;

    CString sMsg;
    sMsg.LoadString(IDS_SKINNY_FOUND);
    AddMessage(sMsg);

    sMsg.LoadString(IDS_TESTING_DONE);
    AddMessage(sMsg);

    sMsg.LoadString(IDS_CLICK_FINISH);
    AddMessage(sMsg);
}

void CVerifyPage::SkinnyNotFound(BOOL bFinal)
{
    if (m_pParent)
        m_pParent->EnableBack(TRUE);

    GetDlgItem(IDC_BUTTON_TEST)->EnableWindow(1);

    CString sMsg;

    sMsg.LoadString(IDS_SKINNY_NOT_FOUND);
    AddMessage(sMsg);

    sMsg.LoadString(IDS_ONHOOK);
    AddMessage(sMsg);

    if (!bFinal)
        DoTest(FALSE);
    else
    {
        sMsg.LoadString(IDS_TESTING_DONE);
        AddMessage(sMsg);

        sMsg.LoadString(IDS_FAILED);
        AddMessage(sMsg);

        if (((CMasterDlg*)m_pParent)->m_bAutoMode)
        {
            CheckDlgButton(CB_SKIP_TEST, 0);
            GetDlgItem(CB_SKIP_TEST)->EnableWindow(0);
        }
        else
        {
            GetDlgItem(CB_SKIP_TEST)->EnableWindow(1);
        }
    }
}

BOOL CVerifyPage::ProcessSkinnyPacket(const char *pkt_data, int packet_len, int offset, int ih_len, int th_len)
{
    // Skipped header length
    int skip_len = offset + ih_len + th_len;

    // Calculate the length belongs to Skinny itself
    int len = packet_len - skip_len;

    // Point to where skinny starts
    char* pSkinny = (char*)pkt_data;
    pSkinny += skip_len;

    int l = sizeof(const struct skinny_common_header);

    // Only process one to find out
    if (len > l)
        return ProcessSkinnyMessage(pSkinny);

    return FALSE;
}

BOOL CVerifyPage::ProcessSkinnyMessage(const char *pptr) 
{
    const struct skinny_common_header *skinny_com_header;
    const char *tptr;
    u_int pdu_len, reserved;

    tptr = pptr;
    skinny_com_header = (const struct skinny_common_header *)pptr;

    // sanity checking of the header, make sure it is Skinny.
    pdu_len = skinny_com_header->size;
    reserved = skinny_com_header->reserved;
    if (pdu_len < 4 || reserved != 0)       // we need to have at least the length of message id
        return FALSE;

    return TRUE;
}

void CVerifyPage::AddMessage(CString sMsg)
{
    int count = m_ListLogs.GetCount();
    m_ListLogs.InsertString(count, sMsg);
}

void CVerifyPage::RemoveLogs()
{
    while(m_ListLogs.GetCount())
        m_ListLogs.DeleteString(0);
}

void CVerifyPage::UpdateConfig()
{
    char szPath[256];
    memset(&szPath, 0, sizeof(szPath));
    ::GetModuleFileName(AfxGetApp()->m_hInstance, szPath, sizeof(szPath));
    
    char* p = strrchr(szPath, '\\');
    if (p)
        *p = 0;

    CString sConfigFile = szPath;
    sConfigFile += "\\pcap-service.config";

    CString sNewConfigFile = szPath;
    sNewConfigFile += "\\pcap-service.config.new";

    CString sLine; 
    CStdioFile infile, outfile; 
    if(!infile.Open(sConfigFile, CFile::modeRead | CFile::typeText))
        return;

    if(!outfile.Open(sNewConfigFile, CFile::modeCreate | CFile::modeWrite | CFile::typeText))
    {
        infile.Close();
        return;
    }

    while(infile.ReadString(sLine) != NULL) 
    { 
        if (sLine.Find("ETHERNET_INTERFACE_ID=") != -1)
        {
            CString sData;
            sData.Format("ETHERNET_INTERFACE_ID=%d\n", m_iTesingNic);
            outfile.WriteString(sData);        
        }
        else
            outfile.WriteString(sLine+"\n");        
    }

    infile.Close();
    outfile.Close();

    CopyFile(sNewConfigFile, sConfigFile, FALSE);
    DeleteFile(sNewConfigFile);
}


BOOL CVerifyPage::OnQueryCancel()
{
    CString sTitle;
    sTitle.LoadString(IDS_APP_TITLE);

    CString sMsg;
    if (this->m_bVerifying)
    {
        sMsg.LoadString(IDS_PLEASE_WAIT);
        MessageBox(sMsg, sTitle, MB_OK|MB_ICONEXCLAMATION);
        return FALSE;
    }

    sMsg.LoadString(IDS_ABORT);

    if (IDOK == MessageBox(sMsg, sTitle, MB_OKCANCEL|MB_ICONEXCLAMATION))
	    return TRUE;
    
    return FALSE;
}
