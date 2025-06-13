#include "stdafx.h"

BOOL APIENTRY DllMain(HANDLE hModule, DWORD ul_reason_for_call, LPVOID lpReserved)
{
    switch (ul_reason_for_call)
    {
        case DLL_PROCESS_ATTACH:
            OutputDebugString("LOUIS SNMP Module: Process Attach");
            break;
        case DLL_THREAD_ATTACH:
            OutputDebugString("LOUIS SNMP Module: Thread Attach");
            break;
        case DLL_THREAD_DETACH:
            OutputDebugString("LOUIS SNMP Module: Thread Detach");
            break;
        case DLL_PROCESS_DETACH:
            OutputDebugString("LOUIS SNMP Module: Process Detach");
            break;
    }

    return TRUE;
}

#define OID_SIZEOF( Oid )      ( sizeof Oid / sizeof(UINT) )
// this is the our branch starting point (clled prefix)
UINT g_unMyOIDPrefix[]	= {1, 3, 6, 1, 4, 1, 15};

AsnObjectIdentifier MIB_OidPrefix = { OID_SIZEOF(g_unMyOIDPrefix), g_unMyOIDPrefix};


BOOL SNMP_FUNC_TYPE SnmpExtensionInit(DWORD dwUptimeReference,HANDLE *phSubagentTrapEvent, AsnObjectIdentifier *pFirstSupportedRegion)
{
    OutputDebugString("LOUIS SNMP Module: SnmpExtensionInit");

    //g_hSimulateTrap = CreateEvent(NULL, FALSE, FALSE, NULL); // creaet this event for the trap

    *pFirstSupportedRegion = MIB_OidPrefix;
    *phSubagentTrapEvent = NULL; //g_hSimulateTrap; // by assigning it pass it to the SNMP service
    //										// So when ever you set this event service will call 
    //										// SnmpExtensionTrap exported function
    //
    //// on loading the our SNMP DLL create the thread
    //g_hTrapGenThread = CreateThread(NULL,0,TrapGenThread,NULL,0,NULL);

    //// hard coded initialization
    //g_szAbout = (char*)malloc(sizeof(char)*64);
    //strcpy(g_szAbout,"Author : Ramanan.T");
    //g_szName = (char*)malloc(sizeof(char)*64);
    //strcpy(g_szName,"Your Name");
    //g_asnIntAge = 0;

    //g_dwStartTime = GetTickCount();

    return SNMPAPI_NOERROR;
}


BOOL SNMP_FUNC_TYPE SnmpExtensionQuery(BYTE bPduType, SnmpVarBindList *pVarBindList, AsnInteger32 *pErrorStatus, AsnInteger32 *pErrorIndex)
{
    OutputDebugString("LOUIS SNMP Module: SnmpExtensionQuery");
    AsnAny* v = &pVarBindList->list[0].value;
    v->asnType = ASN_OCTETSTRING;
    v->asnValue.string.length = 5;
    v->asnValue.string.stream =(unsigned char*)SnmpUtilMemAlloc(v->asnValue.string.length * sizeof(char));

	memcpy(v->asnValue.string.stream,"Louis",v->asnValue.string.length);
	v->asnValue.string.dynamic = TRUE;
    Sleep(1000);

    //int nRet = 0;
    //AsnObjectName;
    //
    //*pErrorStatus = SNMP_ERRORSTATUS_NOERROR;
    //*pErrorIndex = 0;

    //for(UINT i=0;i<pVarBindList->len;i++)
    //{
    //	*pErrorStatus = SNMP_ERRORSTATUS_NOERROR;

    //	// what type of request we are getting?
    //	switch(bPduType)
    //	{
    //	case SNMP_PDU_GET:// // gets the variable value passed variable in pVarBindList
    //		*pErrorStatus = GetRequest(&pVarBindList->list[i]);
    //		if(*pErrorStatus != SNMP_ERRORSTATUS_NOERROR)
    //			*pErrorIndex++;
    //		break;
    //	case SNMP_PDU_GETNEXT: // gets the next variable related to the passed variable in pVarBindList
    //		*pErrorStatus = GetNextRequest(&pVarBindList->list[i]);
    //		if(*pErrorStatus != SNMP_ERRORSTATUS_NOERROR)
    //			*pErrorIndex++;
    //		break;
    //	case SNMP_PDU_SET: // sets a variable
    //		*pErrorStatus = SetRequest(&pVarBindList->list[i]);
    //		if(*pErrorStatus != SNMP_ERRORSTATUS_NOERROR)
    //			*pErrorIndex++;
    //		break;
    //	default:
    //		*pErrorStatus = SNMP_ERRORSTATUS_NOSUCHNAME;
    //		*pErrorIndex++;
    //	};
    //}	
    OutputDebugString("LOUIS SNMP Module: SnmpExtensionQuery EXIT");
    return SNMPAPI_NOERROR;
}


BOOL SNMP_FUNC_TYPE SnmpExtensionTrap(AsnObjectIdentifier *pEnterpriseOid, AsnInteger32 *pGenericTrapId, AsnInteger32 *pSpecificTrapId, AsnTimeticks *pTimeStamp, SnmpVarBindList *pVarBindList)
{
    //static int nNoOfTraps = 1; // just ignore this, I introduced this to send meny traps at once
    //							// any way below we are generating one trap with two values

    //if(nNoOfTraps) // if it is zero don't send traps
    //{
    //	pEnterpriseOid->idLength = sizeof(g_TrapOid);
    //	pEnterpriseOid->ids = g_TrapOid;

    //	*pGenericTrapId		= SNMP_GENERICTRAP_ENTERSPECIFIC;
    //	*pSpecificTrapId	= 1;   // ToasterControl Up trap.
    //	*pTimeStamp			= GetTickCount() - g_dwStartTime;

    //	// Allocate space for the Variable Bindings.
    //	pVarBindList->list = (SnmpVarBind*)SnmpUtilMemAlloc(2*sizeof(SnmpVarBind));

    //	SnmpUtilOidCpy(&pVarBindList->list[0].name,&MIB_OidPrefix);
    //	SnmpUtilOidAppend(&pVarBindList->list[0].name,&g_MyMibTable[1].asnOid);
    //	pVarBindList->list[0].value.asnType = ASN_OCTETSTRING;
    //	pVarBindList->list[0].value.asnValue.string.dynamic = TRUE;
    //	pVarBindList->list[0].value.asnValue.string.length = strlen(*(LPSTR*)g_MyMibTable[1].pStorageValue);
    //	pVarBindList->list[0].value.asnValue.string.stream =(unsigned char*)SnmpUtilMemAlloc(pVarBindList->list[0].value.asnValue.string.length * sizeof(char));
    //	memcpy(pVarBindList->list[0].value.asnValue.string.stream,*(LPSTR*)g_MyMibTable[1].pStorageValue,pVarBindList->list[0].value.asnValue.string.length);
    //			
    //	SnmpUtilOidCpy(&pVarBindList->list[1].name,&MIB_OidPrefix);
    //	SnmpUtilOidAppend(&pVarBindList->list[1].name,&g_MyMibTable[2].asnOid);
    //	pVarBindList->list[1].value.asnType = ASN_INTEGER;
    //	pVarBindList->list[1].value.asnValue.number = *((AsnInteger32*)g_MyMibTable[2].pStorageValue);

    //	pVarBindList->len = 2;

    //	nNoOfTraps--;
    //	// Indicate that valid trap data exists in the parameters.
    //	return TRUE;
    //}
    //nNoOfTraps = 1;
    // Indicate that no more traps are available, and parameters do not refer to any valid data
    return FALSE;
}

