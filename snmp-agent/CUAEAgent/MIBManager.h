#pragma once

#pragma warning( disable: 4996 )

#include "ipc/FlatmapIpcClient.h"
#include "ipc/FlatmapIpcServer.h"
#include <snmp.h>
#include <list>		// list class library
using namespace std;
using namespace Metreos;
using namespace Metreos::IPC;

class CMIBManager;

class COIDResult
{
private:

public:
	unsigned long nUID;
    int nOIDBranch;
    UINT nValue;
    bool bRecv;
    unsigned long nSentTime;
    unsigned long nRecvTime;

    COIDResult(int ntOIDBranch)
    {
        nOIDBranch = ntOIDBranch;
        nValue = 0;
        bRecv = 0;

        nSentTime = GetTickCount();
        nRecvTime = 0;
    };

    ~COIDResult()
    {
    };

};

//////////////////////////////////////////////
// COIDObject Class
//////////////////////////////////////////////
class COIDObject
{
private:
    // this is the our branch starting point (clled prefix)
    UINT *pOIDPrefix;
    bool bParseOIDString(const char *string);
    int nOIDCnt;
public:
    COIDObject(const char *pString);
    virtual ~COIDObject();
    
    UINT * pGetOID(void);
    int nGetOIDSize(void);
};

//////////////////////////////////////////////
// CMIBManager Class
//////////////////////////////////////////////
class CMIBManager : public Metreos::IPC::FlatMapIpcClient
{
private:
    string oServerIP;
    int nServerPort;
    string oLogFile;

    AsnObjectIdentifier *pTreeMIB_OidPrefix;
    COIDObject *pTreeOIDObject;

    long nGetFileSize(char *fileName);
    bool bLoadConfigFile(char *filename);
    bool bFindNameInConfigData(char *ptoken,string *poString,list <string> *oList,bool bRemove);
 
public:
    CMIBManager(const char *pString);
    virtual ~CMIBManager(void);
    bool bConnected;
    string oTreeOID;
    list <COIDResult *> resultList;

    void vConnectToServer(int nWaitTime);
    void vDisconnectFromServer(void);

    bool bCheckTreeOID(void);
    void vSetTreeOID(char *str);
    const char *pGetOIDTree(void);
    bool bCreateTreeOID(const char *pString);
    AsnObjectIdentifier * pGetTreeAsnObject(void);
 
    bool bGetValueFromeServer(UINT *pData,int nBranchOID);
    bool bFindReturnRequestResults(UINT *pData,int nBranchOID);
    bool bSetReturnRequestResults(UINT *pData,int nBranchOID);

    int nGetRequest(SnmpVarBind *pVarBind);
    //UINT unGetStoreVar(CMIBEntry* pMIB, AsnAny *pasnValue);

    virtual void OnIncomingFlatMapMessage(const int messageType, const FlatMapReader& flatmap);
    virtual void OnConnected();
    virtual void OnDisconnected();
	virtual void OnFailure();
};
