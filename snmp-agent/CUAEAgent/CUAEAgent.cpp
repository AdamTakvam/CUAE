// CUAEAgent.cpp : Defines the entry point for the DLL application.
//

#include "stdafx.h"

//Fix for Ace thread include build error
TryEnterCriticalSection(
    IN OUT LPCRITICAL_SECTION lpCriticalSection
    );

#include <stdio.h>
#include <snmp.h>
#include "MIBManager.h"
#include "DLPLog.h"

//Global Log File Object
extern CDLPLog *poLog;

//Global MIB Manager
CMIBManager *poMIBManager = NULL;

bool bBusy = 0;

///////////////////////////////////////////////////////////
//  DllMain
//
//  DLL Main
//
//
///////////////////////////////////////////////////////////
BOOL APIENTRY DllMain( HANDLE hModule,DWORD  ul_reason_for_call, LPVOID lpReserved)
{
	switch (ul_reason_for_call)
	{
		case DLL_PROCESS_ATTACH:
            {
            }
			break;
		case DLL_THREAD_ATTACH:
			break;
		case DLL_THREAD_DETACH:
			break;
		case DLL_PROCESS_DETACH:
            {
                if(poMIBManager != NULL)
                {
                    delete poMIBManager;
                    poMIBManager = NULL;
                }
            }
            break;
    }

    return TRUE;
}

// this is the our branch starting point (clled prefix)
//UINT g_unMyOIDPrefix[]	= {1, 3, 6, 1, 4, 1, 15};
//#define OID_SIZEOF( Oid )      ( sizeof Oid / sizeof(UINT) )
//AsnObjectIdentifier MIB_OidPrefix = { OID_SIZEOF(g_unMyOIDPrefix), g_unMyOIDPrefix};

///////////////////////////////////////////////////////////
//  SnmpExtensionInit
//
//  When exported funtion will be called during DLL
//  loading and initialization
//
///////////////////////////////////////////////////////////
BOOL SNMP_FUNC_TYPE SnmpExtensionInit(DWORD dwUptimeReference,HANDLE *phSubagentTrapEvent, AsnObjectIdentifier *pFirstSupportedRegion)
{
	poMIBManager = new CMIBManager("");

    //Check MIB Manager Object
    if(poMIBManager == NULL)
    {
        return SNMPAPI_ERROR;
    }

    poMIBManager->bCreateTreeOID(poMIBManager->oTreeOID.c_str());

    //Set ASN OID
    *pFirstSupportedRegion = *poMIBManager->pGetTreeAsnObject();
    
	//Set OID off of global
	//*pFirstSupportedRegion = MIB_OidPrefix;

	return SNMPAPI_NOERROR;
}

///////////////////////////////////////////////////////////
//  SnmpExtensionQuery
//
//  This export is to query the MIB table and fields
//
//
///////////////////////////////////////////////////////////
BOOL SNMP_FUNC_TYPE SnmpExtensionQuery(BYTE bPduType, SnmpVarBindList *pVarBindList, AsnInteger32 *pErrorStatus, AsnInteger32 *pErrorIndex)
{
	int nRet = 0;
	AsnObjectName;

	*pErrorStatus = SNMP_ERRORSTATUS_NOERROR;
	*pErrorIndex = 0;

	for(UINT i=0;i<pVarBindList->len;i++)
	{
		*pErrorStatus = SNMP_ERRORSTATUS_NOERROR;

		// what type of request we are getting?
		switch(bPduType)
		{
		    case SNMP_PDU_GET:// gets the variable value passed variable in pVarBindList
                {

					if(bBusy == true)
					{
						for(int i=0; i<5000; i++)
						{
							if(bBusy == false)
							{
								break;
							}

							Sleep(1);
						}


						if(bBusy == true)
						{
							poLog->vPrintDebug("*** ERRROR BUSY = TRUE *** \n");
						}
					}

					bBusy = true;
                    poLog->vPrintDebug("--------- Got a SNMP Get ---------\n");

                    if(poMIBManager == 0)
                    {
                        poLog->vPrintDebug("MIB Manager is NULL \n");
                    }
                    else
                    {
                        *pErrorStatus = poMIBManager->nGetRequest(&pVarBindList->list[i]);
                    }

                    if(*pErrorStatus != SNMP_ERRORSTATUS_NOERROR)
                    {
			            *pErrorIndex++;
                    }

					poLog->vPrintDebug("--------- Finsihed SNMP Get ---------\n\n");
					bBusy = false;

                }
			    break;
		    case SNMP_PDU_GETNEXT: // gets the next variable related to the passed variable in pVarBindList
                {
                    //*pErrorStatus = poMIBManager->nGetNextRequest(&pVarBindList->list[i]);
		            //if(*pErrorStatus != SNMP_ERRORSTATUS_NOERROR)
                   // {
			         //   *pErrorIndex++;
                    //}
                    //ntCnt++;
                }
			    break;
		    case SNMP_PDU_SET: // sets a variable
			    //*pErrorStatus = SetRequest(&pVarBindList->list[i]);
			    //if(*pErrorStatus != SNMP_ERRORSTATUS_NOERROR)
			    //	*pErrorIndex++;
			    break;
		    default:
                {
			        *pErrorStatus = SNMP_ERRORSTATUS_NOSUCHNAME;
			        *pErrorIndex++;
                }
		};
	}

	return SNMPAPI_NOERROR;
}

///////////////////////////////////////////////////////////
//  SnmpExtensionTrap
//
//  This export is for Extensions traps.
//
//
///////////////////////////////////////////////////////////
BOOL SNMP_FUNC_TYPE SnmpExtensionTrap(AsnObjectIdentifier *pEnterpriseOid, AsnInteger32 *pGenericTrapId, AsnInteger32 *pSpecificTrapId, AsnTimeticks *pTimeStamp, SnmpVarBindList *pVarBindList)
{
    return FALSE;
}