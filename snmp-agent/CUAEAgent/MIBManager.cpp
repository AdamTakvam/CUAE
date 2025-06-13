#include "stdafx.h"

//Fix for Ace thread include build error
TryEnterCriticalSection(
    IN OUT LPCRITICAL_SECTION lpCriticalSection
    );

#include "ipc/IpcClient.h"
#include "ace/Time_Value.h"
#include "Flatmap.h"

#include <sys/types.h>
#include <sys/stat.h>
#include <share.h>
#include <fcntl.h>
#include <stdio.h>
#include <stdlib.h>
#include <io.h>
#include <snmp.h>
#include <algorithm>
#include <string>
#include <iostream>
#include <errno.h>

#include "MIBManager.h"
#include "DLPLog.h"

#define DELAY 100
//Global Log File Object
CDLPLog *poLog = 0;


///////////////////////////////////////////////////////////
//  OnIncomingFlatMapMessage
//  
//  IPC Incoming Callback
//  
//
///////////////////////////////////////////////////////////
void CMIBManager::OnIncomingFlatMapMessage(const int messageType, const FlatMapReader& flatmap)
{
    poLog->vPrintDebug("Incoming Flatmap message: %d\n",messageType);

    FlatMapReader r;
    r = flatmap;

    char* pData = NULL;
    int type = FlatMap::INT;
    int nLen = r.find(messageType, &pData, &type, 0);

    if(nLen != 0)
    {

        int *pDataInt = (int *)pData;
        UINT nData = *pDataInt;
		bSetReturnRequestResults(&nData,messageType);
        poLog->vPrintDebug("*** Got BranchOID [%d] = [%d] ***\n",messageType,nData);
    }  
}

///////////////////////////////////////////////////////////
//  OnConnected
//  
//  IPC Connected Callback
//  
//
///////////////////////////////////////////////////////////
void CMIBManager::OnConnected()
{
    poLog->vPrintDebug("**** Connected ****\n");
    bConnected = true;
}

///////////////////////////////////////////////////////////
//  OnDisconnected
//  
//  IPC Disconnected Callback
//  
//
///////////////////////////////////////////////////////////
void CMIBManager::OnDisconnected()
{
   poLog->vPrintDebug("**** DisConnected ****\n");
   bConnected = false;
}

///////////////////////////////////////////////////////////
//  OnFailure
//  
//  IPC OnFailure Callback
//  
//
///////////////////////////////////////////////////////////
void CMIBManager::OnFailure()
{
    bConnected = false;
   poLog->vPrintDebug("**** ICP Failure ****\n");
}


///////////////////////////////////////////////////////////
//  nFindReturnRequestResults
//  
//  Find the Result Value
//  
//
///////////////////////////////////////////////////////////
bool CMIBManager::bFindReturnRequestResults(UINT *pData,int nBranchOID)
{
	poLog->vPrintDebug("----- In Find Return Request [%d] -----\n",nBranchOID);

	if(bConnected == true)
    {
        if(resultList.size() > 0)
        {
            for(std::list<COIDResult *>::iterator list_iter = resultList.begin(); 
                list_iter != resultList.end(); list_iter++)
            {
                COIDResult *poTempResult = (*list_iter);

                if(poTempResult->nOIDBranch == nBranchOID && poTempResult->bRecv == true)
                {
                    *pData = poTempResult->nValue;
                    resultList.erase(list_iter);
                    delete poTempResult;
                    return true;
                }
            }
        }
    }

    return false;
}

///////////////////////////////////////////////////////////
//  nFindReturnRequestResults
//  
//  Find the Result Value
//  
//
///////////////////////////////////////////////////////////
bool CMIBManager::bSetReturnRequestResults(UINT *pData,int nBranchOID)
{
	poLog->vPrintDebug("----- In Set Return Request ----- [%d]\n",nBranchOID);

	int nValueData = *pData;

    if(bConnected == true)
    {
		//Wait up to X seconds for a result
		for(int i=0; i<DELAY; i++)
		{	       
			//I found a result
			if(resultList.size() > 0)
			{
				poLog->vPrintDebug("Waited = [%d]\n",i);
				break;
			}

			Sleep(10);
			//ACE_OS::sleep(1);
		}

		try
		{
			if(resultList.size() > 0)
			{
				for(std::list<COIDResult *>::iterator list_iter = resultList.begin(); 
					list_iter != resultList.end(); list_iter++)
				{
					COIDResult *poTempResult = (*list_iter);

					if(poTempResult->nOIDBranch == nBranchOID && poTempResult->bRecv == false)
					{
			
						poLog->vPrintDebug("**** SetValue = [%d] ****\n",nValueData);
						poTempResult->nValue = nValueData;
			
						poTempResult->nRecvTime = GetTickCount();
						poLog->vPrintDebug("----- Found Result for [%d]- [%d] ------\n",nBranchOID,resultList.size());

						poTempResult->bRecv = true;
						return true;
					}
				}
			}

		} catch(...) 
		{
			poLog->vPrintDebug("----- ERROR TRY CATCH [%d] ------\n",nBranchOID);
		}

    }

	poLog->vPrintDebug("----- ERROR DID NOT FIND Result for [%d] ------\n",nBranchOID);

    return false;
}

///////////////////////////////////////////////////////////
//  nGetValueFromeServer
//  
//  Get the Value from the server
//  
//
///////////////////////////////////////////////////////////
bool CMIBManager::bGetValueFromeServer(UINT *pData,int nBranchOID)
{
	poLog->vPrintDebug("----- In Get Value From Server [%d]----\n",nBranchOID);

    if(bConnected == false)
    {
        vConnectToServer(500);
    }

	if(bConnected == false)
	{
		poLog->vPrintDebug("----- Error Could not connect to Server ----\n");
		return 0;
	}	


    FlatMapWriter map;
    map.insert(nBranchOID,nBranchOID);
    Write(nBranchOID, map);

    COIDResult *poOIDResult = new COIDResult(nBranchOID);
    resultList.push_back(poOIDResult);

	bool bRetVal = false;
	try
	{
		//Try to find the request return value up to X milli seconds
		for(int i=0; i<5; i++)
		{
			UINT nValue = 0;
	        
			bRetVal = bFindReturnRequestResults(&nValue,nBranchOID);
			if(bRetVal == true)
			{
				*pData =  nValue;
				bRetVal = true;
				break;
			}

			Sleep(100);
			//ACE_OS::sleep(1);
		}
	}
	catch(...)
	{
		poLog->vPrintDebug("----- Error Catch in bGetValue ----\n");
	}


    return bRetVal;
}

///////////////////////////////////////////////////////////
//  CMIBManager
//  
//  Constructor
//  
//
///////////////////////////////////////////////////////////
CMIBManager::CMIBManager(const char *configfilename)
{
    pTreeOIDObject = NULL;
    bConnected = false;
    bLoadConfigFile((char *)configfilename);

	//poLog = new CDLPLog("C:\\snmpagent.txt");
    poLog = new CDLPLog((char *)oLogFile.c_str());
    poLog->vPrintDebug("CUAE SNMP Agent 1.02 Using tree %s Server[%s] Port[%d]\n"
		,oTreeOID.c_str()  
		,oServerIP.c_str()
		,nServerPort);
}

///////////////////////////////////////////////////////////
//  vSetTreeOID
//  
//  Set the TreeOID String
//  
//
///////////////////////////////////////////////////////////
void CMIBManager::vSetTreeOID(char *str)
{
    oTreeOID.append(str);
}

///////////////////////////////////////////////////////////
//  pGetOIDTree
//  
//  Get OID Tree String
//  
//
///////////////////////////////////////////////////////////
const char * CMIBManager::pGetOIDTree(void)
{
    return oTreeOID.c_str();
}

///////////////////////////////////////////////////////////
//  pGetTreeAsnObject
//  
//  Get ASN Object
//  
//
///////////////////////////////////////////////////////////
AsnObjectIdentifier * CMIBManager::pGetTreeAsnObject(void)
{
    return pTreeMIB_OidPrefix;
}

///////////////////////////////////////////////////////////
//  CMIBManager
//  
//  Destructor
//  
//
///////////////////////////////////////////////////////////
CMIBManager::~CMIBManager(void)
{
    if(resultList.size() > 0)
    {
        for(std::list<COIDResult *>::iterator list_iter = resultList.begin(); 
            list_iter != resultList.end(); list_iter++)
        {
                COIDResult *poTempResult = (*list_iter);
                resultList.erase(list_iter);
                delete poTempResult;
        }
    }

    vDisconnectFromServer();

    //Check and Delete Tree OID Object
    if(pTreeOIDObject != NULL)
    {
        delete pTreeOIDObject;
        pTreeOIDObject = 0;
    }

    //Check and Delete Tree OID ASN Object
    if(pTreeMIB_OidPrefix != NULL)
    {
        delete pTreeMIB_OidPrefix;
        pTreeMIB_OidPrefix = 0;
    }

    delete poLog;
    poLog = 0;
}

///////////////////////////////////////////////////////////
//  StrSub
//  
//  Sub String helping function
//  
//
///////////////////////////////////////////////////////////
void StrSub(string& cp,string sub_this,string for_this,int num_times)
{
    size_t i,loc;
    if (cp.empty())
    {
        cp = sub_this;
        return;
    }

    for (i = 0; i != num_times; i++)
    {
        loc = cp.find(for_this,0);
        if (loc >= 0) cp.replace(loc,for_this.length(),sub_this);
        else return;
    }
}

///////////////////////////////////////////////////////////
//  nGetRequest
//  
//  Get Agent Request
//  
//
///////////////////////////////////////////////////////////
int CMIBManager::nGetRequest(SnmpVarBind *pVarBind)
{
    poLog->vPrintDebug("GetRequest Start\n");

    //Get the FULL OID string
    char *pStr2 = SnmpUtilOidToA(&pVarBind->name);

    //Put the Get OID into string object
    string oTStr1(pStr2);
   
    //Create temp string to hold branch OID
    char oTemp[5];
    memset(oTemp,0,5);

    //Ok find out how many characters in oid branch and break 
    //Max of 4 characters
    size_t nLen = (oTStr1.length() -1);
	
	//This will strip the .0 off the end as we see prtg add it on
	int nFound1 = 0;
	if(oTStr1[oTStr1.length()-1] == '0')
	{
		nFound1 = 1;
	}

	int nFound2 = 0;
	if(oTStr1[oTStr1.length()-2] == '.')
	{
		nFound2 = 1;
	}

	if(nFound1 && nFound2)
	{
		oTStr1.erase(oTStr1.length()-2,2);
	}

	//poLog->vPrintDebug("Here... = [%s]\n\n",oTStr1.c_str());
	
	//reset Len
	nLen = (oTStr1.length() -1);

    int nCnt = 0;
    for(int i=0; i<4; i++)
    {
        char nChar = oTStr1[nLen - i];
        if(nChar == '.')
        {
            break;
        }
        nCnt++;
    }

    //Ok now go backward and fill in the oid branch string
    int nCounter = nCnt;
    for(int j=0; j<nCnt; j++)
    {
        oTemp[nCounter-1] = oTStr1[nLen - j];
        nCounter--;
    }

    poLog->vPrintDebug("GetRequest OID Branch = [%s]\n",oTemp);

    int nBranchOID = atoi(oTemp); 
    UINT nValue = 0;
    bool bResult = false;
    bResult = bGetValueFromeServer(&nValue,nBranchOID);

	if(bResult == false)
	{
		nValue = 0;
		pVarBind->value.asnValue.number = *(AsnInteger32*)&nValue;
	}
	else
	{
		pVarBind->value.asnValue.number = *(AsnInteger32*)&nValue;
	}

	pVarBind->value.asnType = ASN_INTEGER32;

	return SNMP_ERRORSTATUS_NOERROR;
}

/* Not used yet but good code to keep around
///////////////////////////////////////////////////////////
//  unGetStoreVar
//  
//  Get stored values from Entry object
//  
//
///////////////////////////////////////////////////////////
UINT CMIBManager::unGetStoreVar(CMIBEntry* pMIB, AsnAny *pasnValue)
{
	// check rights is there to access
	if((pMIB->unAccess != SNMP_ACCESS_READ_ONLY)&&(pMIB->unAccess != SNMP_ACCESS_READ_WRITE)&&(pMIB->unAccess != SNMP_ACCESS_READ_CREATE))
    {
        return SNMP_ERRORSTATUS_GENERR;
    }
	// set the type
	pasnValue->asnType = pMIB->chType;
	
	switch(pasnValue->asnType)
	{
	    case ASN_INTEGER:
            {
		        pasnValue->asnValue.number = *(AsnInteger32*)pMIB->pStorageValue;
            }
            break;
	    case ASN_COUNTER32:
	    case ASN_GAUGE32:
	    case ASN_TIMETICKS:
	    case ASN_UNSIGNED32:
            {
                pasnValue->asnValue.unsigned32 = *(AsnUnsigned32*)pMIB->pStorageValue;
            }
            break;
	    case ASN_OCTETSTRING:
            {
                pasnValue->asnValue.string.length = (UINT)strlen((char *)pMIB->pStorageValue);
		        pasnValue->asnValue.string.stream =(unsigned char*)SnmpUtilMemAlloc(pasnValue->asnValue.string.length * sizeof(char));
		        memcpy(pasnValue->asnValue.string.stream,(char *)pMIB->pStorageValue,pasnValue->asnValue.string.length);

		        pasnValue->asnValue.string.dynamic = TRUE;
            }
            break;
	    case ASN_COUNTER64:
            {
		        pasnValue->asnValue.counter64 = *(AsnCounter64*)pMIB->pStorageValue;
            }
            break;
	    case ASN_OBJECTIDENTIFIER:
            {
		        SnmpUtilOidCpy(&pasnValue->asnValue.object,(AsnObjectIdentifier*)pMIB->pStorageValue);
            }
            break;
	    case ASN_IPADDRESS:
            {
		        pasnValue->asnValue.address.length = 4;
		        pasnValue->asnValue.string.dynamic = TRUE;

		        pasnValue->asnValue.address.stream[0] = ((char*)pMIB->pStorageValue)[0];
		        pasnValue->asnValue.address.stream[1] = ((char*)pMIB->pStorageValue)[1];
		        pasnValue->asnValue.address.stream[2] = ((char*)pMIB->pStorageValue)[2];
		        pasnValue->asnValue.address.stream[3] = ((char*)pMIB->pStorageValue)[3];
            }
            break;
	    case ASN_OPAQUE:
            {
		        AsnSequence;
            }
		    break;
	    case ASN_BITS:
	    case ASN_SEQUENCE:
            {
            }
		    break;	
	    case ASN_NULL:
	    default:
            {
		        return SNMP_ERRORSTATUS_GENERR;
            }
	}
	return SNMP_ERRORSTATUS_NOERROR;
}
*/

///////////////////////////////////////////////////////////
//  bCreateOID
//  
//  Create and parse the Tree OID
//  
//
///////////////////////////////////////////////////////////
bool CMIBManager::bCreateTreeOID(const char *pString)
{
    //Main MIB Tree OID
    pTreeOIDObject = new COIDObject(pString);

    //NULL out Var
    pTreeMIB_OidPrefix = NULL;

    //Check Object
    if(pTreeOIDObject != NULL)
    {
        //Check Tree OID
        if(pTreeOIDObject->pGetOID() != NULL)
        {
            //Allocate Asn Tree OID Structure
            pTreeMIB_OidPrefix = new AsnObjectIdentifier;
            
            //Set ASNObjectID Members
            pTreeMIB_OidPrefix->idLength =  pTreeOIDObject->nGetOIDSize();
            pTreeMIB_OidPrefix->ids = pTreeOIDObject->pGetOID();
        }
    }

    if(pTreeOIDObject == NULL || pTreeOIDObject->pGetOID() == NULL)
    {
        return false;
    }

    return true;
}

//////////////////////////////////////////////////////////
//  COIDObject
//  
//  Constructor
//  
//
///////////////////////////////////////////////////////////
COIDObject::COIDObject(const char *pString)
{
    pOIDPrefix = NULL;
    nOIDCnt = -1;
    bParseOIDString(pString);

}

///////////////////////////////////////////////////////////
//  COIDObject
//  
//  Destructor
//  
//
///////////////////////////////////////////////////////////
COIDObject::~COIDObject()
{
    if(pOIDPrefix != NULL)
    {
        delete [] pOIDPrefix;
        pOIDPrefix = 0;
    }
}

///////////////////////////////////////////////////////////
//  nGetOIDSize
//  
//  Get number of items an OID has
//  
//
///////////////////////////////////////////////////////////
int COIDObject::nGetOIDSize(void)
{
    return nOIDCnt;
}

///////////////////////////////////////////////////////////
//  bParseOIDString
//  
//  Parse a string that is made up of an OID tree number
//  example: ".1.3.6.1.4.1.15"
//
///////////////////////////////////////////////////////////
bool COIDObject::bParseOIDString(const char *pString)
{
    //Get the Len of the string
    size_t nLen = strlen(pString);
    
    //Check the Len and String
    if(nLen <= 0 || pString == NULL)
    {
        //Error Logging needs to be put here
        return 0;
    }

    //Create a temp string
    char *pStr1 = new char[nLen+1];
    //Clear it with a term NULL
    memset(pStr1,0,nLen+1);
    //Copy String
    memcpy(pStr1,pString,nLen);
    
    //Create the Sep
    char seps[]   = ".";

    //Init the Token Count
    int nTokenCnt = 0;

    //Token Pointer Vars
    char *token1 = NULL;
    char *next_token1 = NULL;

    //Get the Number of branches in the OID
    token1 = strtok(pStr1,seps);
    
    // While there are tokens in "string"
    while (token1 != NULL)
    {
        // Get next token:
        if (token1 != NULL)
        {
            //Get Next Token
            token1 = strtok( NULL,seps);
            //Inc Cnt
            nTokenCnt++;
        }
    }

    //Set the number of OID entries
    nOIDCnt = nTokenCnt;

    //Check Token Count
    if(nTokenCnt <= 0)
    {
        //Clean up temp string
        delete [] pStr1;

        //Error Logging needs to be put here
        return 0;
    }

    //Allocate OID
    pOIDPrefix = new UINT[nTokenCnt];
    //Clear it
    memset(pOIDPrefix,1,nTokenCnt);
    
    //Copy string to temp string
    memcpy(pStr1,pString,nLen);
    
    //Reset Vars
    token1 = NULL;
    next_token1 = NULL;
    nTokenCnt = 0;

    //Get the branches data from the OID
    token1 = strtok(pStr1,seps);
    // While there are tokens in "string"
    while (token1 != NULL)
    {
        // Get next token:
        if (token1 != NULL)
        {
            //Set OID Token in string to INT
            pOIDPrefix[nTokenCnt] = atoi(token1);

            //Get Next Token
            token1 = strtok( NULL,seps);
            nTokenCnt++;
        }
    }

    //Delete temp string
    delete [] pStr1;

    //Return Success
    return 1;
}

///////////////////////////////////////////////////////////
//  pGetOID
//  
//  Get OID Array Pointer
//  
//
///////////////////////////////////////////////////////////
UINT * COIDObject::pGetOID(void)
{
    return pOIDPrefix;
}

//////////////////////////////////////////////////////
//
//	Get File Size Helper function
//
//////////////////////////////////////////////////////
long CMIBManager::nGetFileSize(char *fileName)
{
	int fh = 0;
	long nSize = 0;

//Get File Size
#ifdef WIN32
    fh = _sopen(fileName, _O_RDONLY,_SH_DENYNO,_S_IREAD | _S_IWRITE );
	if(fh  <=  0 ) 
	{	
		int nVal = errno;
		switch(errno)
		{
			case EACCES:
				{
					int x=0;
				}
				break;
		};
		return -1;
	}
#endif

#ifndef WIN32
	if(( fh = open(fileName, O_RDONLY ) ) <=  0 ) 
	{
		return -1;
	}
#endif

#ifdef WIN32
	nSize = _lseek( fh, 0L , 2 );
	//size = (( unsigned short )tell(fh));
	_close( fh );
#endif


#ifndef WIN32
	nSize = lseek( fh, 0L , 2 );
	//size = (( unsigned short )tell(fh));
	close( fh );
#endif

	if(nSize <= 0)
	{
			return -1;
	}

    return nSize;
}

///////////////////////////////////////////////////////////
//  bLoadConfigFile
//  
//  Load data from config files, comma delimeted
//  
//
///////////////////////////////////////////////////////////
bool CMIBManager::bLoadConfigFile(char *filename)
{
/*
    //Get FileSize
    size_t nLen = nGetFileSize(filename);

    //Open File
    FILE *fp = NULL;
    fopen_s(&fp,filename,"r+");
    
    //Check File Pointer
    if(fp == NULL)
    {
        //Error
        return false;
    }

    //Allocate Buffer and make it NULL Terminated
    char *pBuffer = new char[nLen+1];
    memset(pBuffer,0,nLen+1);

    //Read File
    fread(pBuffer,nLen,1,fp);
    
    //Close Buffer
    fclose(fp);

    //Put buffer in a String class for parse
    string oStr(pBuffer);

   //Create the Sep
    char seps[]   = "\n";

    //Init the Token Count
    int nTokenCnt = 0;

    //Token Pointer Vars
    char *token1 = NULL;
    char *next_token1 = NULL;

    //Get the Number of branches in the OID
    token1 = strtok_s(pBuffer,seps,&next_token1);
    
    //List to hold the tokenized string
    list <string> stringList;

    // While there are tokens in "string"
    while (token1 != NULL)
    {
        // Get next token:
        if (token1 != NULL)
        {
            string otStr(token1);
            stringList.push_back(token1);

            //Get Next Token
            token1 = strtok_s(NULL,seps,&next_token1);
            //Inc Cnt
            nTokenCnt++;
        }
    }

    //Clean it up
    delete [] pBuffer;


    //Get TreeName
    bFindNameInConfigData("TREE_ROOT,",&oTreeOID,&stringList,true);

    bFindNameInConfigData("STATS_SERVER_IP,",&oServerIP,&stringList,true);

    string oTempStr;
    bFindNameInConfigData("STATS_SERVER_PORT,",&oTempStr,&stringList,true);
    nServerPort = atoi(oTempStr.c_str());

    bFindNameInConfigData("AGENT_LOG_FILE,",&oLogFile,&stringList,true);
*/

	oTreeOID = "1.3.6.1.4.1.22720.1";
	oServerIP = "127.0.0.1";
	nServerPort = 9202;
	oLogFile = "C:\\snmpagentlog.txt";

    return true;
}

///////////////////////////////////////////////////////////
//  bFindNameInConfigData
//  
//  Find Tree name from config file list of strings
//  
//
///////////////////////////////////////////////////////////
bool CMIBManager::bFindNameInConfigData(char *ptoken,string *poString,list <string> *stringList,bool bRemove)
{
    //Find TREE
    for(std::list<string>::iterator list_iter = stringList->begin(); 
        list_iter != stringList->end(); list_iter++)
    {

        string oTempStr = (*list_iter);

        size_t nRetVal = oTempStr.find(ptoken);
        if(nRetVal == 0)
        {
            size_t nSizeVal = strlen(ptoken);
            oTempStr.erase(0,nSizeVal);
            poString->append(oTempStr);

            if(bRemove == true)
            {
                stringList->erase(list_iter);
            }

            return true;
        }
    }

    return false;
}

///////////////////////////////////////////////////////////
//  vDisconnectFromServer
//  
//  Disconnect from Server
//  
//
///////////////////////////////////////////////////////////
void CMIBManager::vDisconnectFromServer(void)
{
    if(bConnected == true)
    {
        Disconnect();
        bConnected = false;
    }
}

///////////////////////////////////////////////////////////
//  vConnectToServer
//  
//  Connect to Server
//  
//
///////////////////////////////////////////////////////////
void CMIBManager::vConnectToServer(int nWaitTimeMS)
{
	if(bConnected == false)
	{
		bConnected = Connect(oServerIP.c_str(), nServerPort);
		Sleep(nWaitTimeMS);
	}

    if(bConnected == true)
    {
        poLog->vPrintDebug("**** Success Connected to Server[%s]-[%d]****\n",oServerIP.c_str(),nServerPort);
    }
    else
    {
       poLog->vPrintDebug("**** Error Could not Connect to Server[%s]-[%d]****\n",oServerIP.c_str(),nServerPort);
    }
}