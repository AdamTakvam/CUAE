#include "StdAfx.h"
#include "CTIAppExchange.h"
#include "CTIRelatedObjectSet.h"
#include "CTIRelatedObject.h"
#include "CTIUserInterface.h"
#include "CTIField.h"
#include <atlsafe.h>
#include <algorithm>
#include "CTIAppExchangeSaveThread.h"
#include "CTIAppExchangeSearchThread.h"
#include "ctiappexchange.h"
#include "CTIUtils.h"
#import "SF_MSApi4.dll" no_namespace named_guids

//Initializer blocks for static member variables
ObjectFieldMap CCTIAppExchange::m_mapNameFields;
ObjectFieldMap CCTIAppExchange::m_mapObjectsToFields;
ObjectFieldMap CCTIAppExchange::m_mapNonquotedFields;
bool CCTIAppExchange::m_bPersonAccountsExist = false;

CCTIAppExchange::CCTIAppExchange(CCTIUserInterface* pUI)
:m_pUI(pUI),
 m_pSession(NULL),
 m_pSaveThread(NULL),
 m_pSearchThread(NULL),
 m_bCanCreateCallLogs(false)
{
}

CCTIAppExchange::~CCTIAppExchange(void)
{
	//This will delete all the lists in the object maps
	Reset();

	//Stop the save thread
	delete m_pSaveThread;
	m_pSaveThread=NULL;

	delete m_pSearchThread;
	m_pSearchThread = NULL;

	m_pSession.Release();
}

void CCTIAppExchange::UpdateSid(std::wstring sInstance, std::wstring sSid)
{
	if (sSid!=m_sSid || sInstance!=m_sInstance) {
		SetSid(sSid);
		SetInstance(sInstance);

		if (sSid.empty()) {
			SetUserId(L"");
			m_bCanCreateCallLogs = false;
		} else {
			if (m_pSession==NULL) {
				m_pSession=CreateOfficeToolkit(sInstance,sSid);
				ISObject4Ptr pDefinition = m_pSession->CreateObject("CustomSetupDefinition");

				//Fire up the worker threads while we're here
				if (m_pSaveThread==NULL) m_pSaveThread = new CCTIAppExchangeSaveThread(this);
				if (m_pSearchThread==NULL) m_pSearchThread = new CCTIAppExchangeSearchThread(this);
			} else {
				SetSessionInstanceAndSid(m_pSession,sInstance,sSid);
			}

			try {
				if (CCTIUtils::BSTRToString(m_pSession->UserId)!=m_sUserId) 
				{
					CCTILogger::Log(LOGLEVEL_MED,L"CCTIAppExchange::UpdateSid: New user detected!  Updating cached information.");
					SetUserId(CCTIUtils::BSTRToString(m_pSession->UserId));
					
					std::wstring sFullName = CCTIUtils::BSTRToString(m_pSession->UserFullName);
					m_pUI->SetCurrentUserName(sFullName);

					CCTILogger::Log(LOGLEVEL_MED,L"CCTIAppExchange::UpdateSid: User %s authenticated.",sFullName);

					UpdateCachedInformation();
				}
			} catch (_com_error ce) {
				CCTILogger::Log(LOGLEVEL_LOW,L"CCTIAppExchange::UpdateSid: Unable to get user ID.  Error message: %s.",(wchar_t*)m_pSession->GetErrorMessage());
				//Reset the user id so that the cached information will all be reloaded next time
				SetUserId(L"");
			}
		}
	}
}

void CCTIAppExchange::ResetObjectMap(ObjectFieldMap& map) {
	for (ObjectFieldMap::iterator it=map.begin();it!=map.end();it++) {
		StringList* pList = it->second;
		delete pList;
	}
	map.clear();
}

void CCTIAppExchange::UpdateCachedInformation()
{
	ResetObjectMap(m_mapInternalLayout);
	ResetObjectMap(m_mapInboundLayout);
	ResetObjectMap(m_mapOutboundLayout);

	LoadUserParamsFromCustomSetup();

	//First get the profile
	std::wstring sQuery = L"Select ProfileId,UserPermissionsCallCenterAutoLogin From User Where Id='"+m_sUserId+L"'";
	IQueryResultSet4Ptr pQueryResults = NULL;
	std::wstring sProfileId;

	try {
		pQueryResults = m_pSession->Query(CCTIUtils::StringToBSTR(sQuery),VARIANT_FALSE);

		ISObject4Ptr pSObject;
		BEGIN_FOREACH_QUERYRESULT(pQueryResults,pSObject)
			//Get the field value of the returned object
			sProfileId = GetFieldValue(m_pSession,pSObject,L"ProfileId");
			std::wstring sAutoLogin = GetFieldValue(m_pSession,pSObject,L"UserPermissionsCallCenterAutoLogin");
			m_pUI->SetAutoLogin(sAutoLogin==L"-1");
			//There should only be one row anyway
			break;
		END_FOREACH_QUERYRESULT()
	} catch(_com_error ce) {
		CCTILogger::Log(LOGLEVEL_LOW,L"CCTIAppExchange::UpdateCachedInformation: Error getting User information: %s.",(wchar_t*)m_pSession->GetErrorMessage());
	}

	DescribeSoftphoneLayout();

	//Finally, see if this user can create Task objects for call logs
	try {
		//Ensure that the user has CRUD permissions to create this object
		ISObject4Ptr pSObject = m_pSession->CreateObject(CCTIUtils::StringToBSTR(L"Task"));
		//If we've gotten to this point without an exception, then the user can create tasks.
		m_bCanCreateCallLogs = (pSObject->Createable==VARIANT_TRUE);
	} catch (_com_error ce) {
		//If we're here, the object couldn't be created (b/c the user probably doesn't have permissions for it), so we'll exclude this object from the list.
		CCTILogger::Log(LOGLEVEL_MED,L"CCTIAppExchange::UpdateCachedInformation: The user cannot create Task objects, so he is unable to create call logs.");
	}
}

void CCTIAppExchange::DescribeSoftphoneLayout()
{
	//HTTP to get describeSoftphoneLayout XML

	_bstr_t bsDescribeURL = m_pSession->GetServerUrl();
	std::wstring sDescribeCall = L"<?xml version=\"1.0\" encoding=\"UTF-8\"?> \
	<soapenv:Envelope xmlns:soapenv=\"http://schemas.xmlsoap.org/soap/envelope/\" \
	xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\" xmlns:xsi=\"http://www.w3.org/2001/\
	XMLSchema-instance\">\
	<soapenv:Header>\
	<ns1:SessionHeader soapenv:mustUnderstand=\"0\" \
	xmlns:ns1=\"urn:enterprise.soap.sforce.com\">\
	<ns1:sessionId>";
	sDescribeCall += GetSid() + L"</ns1:sessionId> \
	</ns1:SessionHeader> \
	</soapenv:Header> \
	<soapenv:Body> \
	<describeSoftphoneLayout xmlns=\"urn:enterprise.soap.sforce.com\"/> \
	</soapenv:Body> \
	</soapenv:Envelope> ";

	_variant_t isSoap = true;
	_bstr_t bsSoftphoneLayout;
	try {
		bsSoftphoneLayout = m_pSession->MakeHttpRequest(bsDescribeURL,KEY_POST,CCTIUtils::StringToBSTR(sDescribeCall),"",VARIANT_FALSE,VARIANT_FALSE,&isSoap);
		
		CCTILogger::Log(LOGLEVEL_HIGH,L"CCTIAppExchange::DescribeSoftphoneLayout: Retrieved softphone layout: %s",(wchar_t*)bsSoftphoneLayout);
	} catch (_com_error ce) {
		CCTILogger::Log(LOGLEVEL_MED,L"CCTIAppExchange::DescribeSoftphoneLayout: Unable to retrieve softphone layout: %s.",m_pSession->GetErrorMessage());
	}

	try {
		if (bsSoftphoneLayout.length()>0) {
			MSXML2::IXMLDOMDocumentPtr xLayout;
			HRESULT hr = xLayout.CreateInstance(__uuidof(MSXML2::FreeThreadedDOMDocument60));
			if (xLayout->loadXML(bsSoftphoneLayout) == VARIANT_TRUE)
			{
				//Seems like MSXML gets messed up by the namespace in the XML, so we have to use this workaround
				//MSXML2::IXMLDOMNodeListPtr pCallTypesNodeList = xLayout->selectNodes(L"//callTypes]");
				MSXML2::IXMLDOMNodeListPtr pCallTypesNodeList = xLayout->selectNodes(L"//*[name() = 'callTypes']");
				int nLength = pCallTypesNodeList->length;
				for (int nCurrentCallTypes=0; nCurrentCallTypes<pCallTypesNodeList->length; nCurrentCallTypes++)
				{
					MSXML2::IXMLDOMNodePtr pCallTypes = pCallTypesNodeList->item[nCurrentCallTypes];
					MSXML2::IXMLDOMNodePtr pCallType = pCallTypes->selectSingleNode(L"./*[name() = 'name']/text()");
					if (pCallType) {
						_bstr_t bsCallType = pCallType->nodeValue;
						std::wstring sCallType = bsCallType;

						int nLayout = -1;

						if (sCallType==L"Internal") {
							nLayout = LAYOUT_INCOMING_INTERNAL;
						} else if (sCallType==L"Outbound") {
							nLayout = LAYOUT_OUTBOUND;
						} else if (sCallType==L"Inbound") {
							nLayout = LAYOUT_INCOMING_EXTERNAL;
						}

						if (nLayout!=-1) {
							ObjectFieldMap& mapLayout = GetMapForLayout(nLayout);
							StringList* listInfoFields = GetInfoFieldsForLayout(nLayout);

							//First get all the infoField nodes that live under the current callTypes node
							//MSXML2::IXMLDOMNodeListPtr pInfoFieldNodeList = xLayout->selectNodes(L"//infoField");
							MSXML2::IXMLDOMNodeListPtr pInfoFieldNodeList = pCallTypes->selectNodes(L"./*[name() = 'infoFields']/*[name() = 'name']/text()");
							for (int nCurrentInfoField=0; nCurrentInfoField<pInfoFieldNodeList->length; nCurrentInfoField++)
							{
								MSXML2::IXMLDOMNodePtr pInfoField = pInfoFieldNodeList->item[nCurrentInfoField];
								_bstr_t bsInfoField = pInfoField->nodeValue;
								std::wstring sInfoField = bsInfoField;
								listInfoFields->push_back(sInfoField);

								CCTILogger::Log(LOGLEVEL_HIGH,L"CCTIAppExchange::DescribeSoftphoneLayout: Added info field %s to %s layout.",sInfoField,sCallType);
							}
							
							MSXML2::IXMLDOMNodeListPtr pSectionsNodeList = pCallTypes->selectNodes(L"./*[name() = 'sections']");
							for (int nCurrentSections=0; nCurrentSections<pSectionsNodeList->length; nCurrentSections++)
							{
								MSXML2::IXMLDOMNodePtr pSections = pSectionsNodeList->item[nCurrentSections];

								MSXML2::IXMLDOMNodePtr pEntityApiName = pSections->selectSingleNode(L"./*[name() = 'entityApiName']/text()");
								_bstr_t bsEntityApiName = pEntityApiName->nodeValue;
								std::wstring sEntityApiName = bsEntityApiName;

								StringList* listObjectFields = QueryObjectInfo(m_pSession, sEntityApiName);

								MSXML2::IXMLDOMNodeListPtr pItemsNodeList = pSections->selectNodes(L"./*[name() = 'items']/*[name() = 'itemApiName']/text()");
								int nNumberOfItems = pItemsNodeList->length;
								for (int nCurrentItems=0; nCurrentItems<pItemsNodeList->length; nCurrentItems++)
								{
									MSXML2::IXMLDOMNodePtr pItemApiName = pItemsNodeList->item[nCurrentItems];
									_bstr_t bsItemApiName = pItemApiName->nodeValue;
									std::wstring sItemApiName = bsItemApiName;

									//Make sure the field is actually in the object
									if (find(listObjectFields->begin(),listObjectFields->end(),sItemApiName)!=listObjectFields->end()) {
										AddFieldToMap(mapLayout,sEntityApiName,sItemApiName);
										CCTILogger::Log(LOGLEVEL_HIGH,L"CCTIAppExchange::DescribeSoftphoneLayout: Added %s:%s to %s layout.",sEntityApiName,sItemApiName,sCallType);
									} else {
										CCTILogger::Log(LOGLEVEL_LOW,L"CCTIAppExchange::DescribeSoftphoneLayout: Warning: Field %s:%s is in the %s layout, but does not actually exist on the object.  Field was ignored.",sEntityApiName,sItemApiName,sCallType);
									}
								}
							}
						}
					}
				}
			} else {
				CCTILogger::Log(LOGLEVEL_LOW,L"CCTIAppExchange::DescribeSoftphoneLayout: Error parsing describeSoftphoneLayout XML: %s.",xLayout->parseError->reason);
			}
		}
	} catch (_com_error ce) {
		CCTILogger::Log(LOGLEVEL_MED,L"CCTIAppExchange::DescribeSoftphoneLayout: Unable to parse softphone layout: %s.",ce.ErrorMessage());
	}
}

void CCTIAppExchange::GetSoftphoneLayoutItems(std::wstring sSoftphoneLayoutId, PairList& listItems) 
{
	std::wstring sQuery = L"Select ItemDevName, LayoutSectionId from SoftphoneLayoutItem Where LayoutId='"+sSoftphoneLayoutId+L"' Order By ItemOrder";
	IQueryResultSet4Ptr pQueryResults = NULL;
	ISObject4Ptr pSObject;

	try
	{
		pQueryResults = m_pSession->Query(CCTIUtils::StringToBSTR(sQuery),VARIANT_FALSE);
		BEGIN_FOREACH_QUERYRESULT(pQueryResults,pSObject)
			//Get the field value of the returned object
			std::wstring sSectionIdValue = GetFieldValue(m_pSession,pSObject,L"LayoutSectionId");
			std::wstring sItemNameValue = GetFieldValue(m_pSession,pSObject,L"ItemDevName");

			//Add the two fields to the pair
			StringPair pair;
			pair.first = sSectionIdValue;
			pair.second = sItemNameValue;

			listItems.push_back(pair);
		END_FOREACH_QUERYRESULT()
	}
	catch(_com_error ce)
	{
		CCTILogger::Log(LOGLEVEL_LOW,L"CCTIAppExchange::AddFieldsFromQueryToMap: Error: %s.",(wchar_t*)m_pSession->GetErrorMessage());
	}
	
	if (pQueryResults) pQueryResults.Release();
}

void CCTIAppExchange::AddSoftphoneLayoutItemsToMap(PairList& listItems, StringList& listSectionIds, PARAM_MAP& mapSectionIdToObjectName, ObjectFieldMap& mapLayout) {
	//For each section ID in the list, find its layout items in the query results and add them to the map.
	for (StringList::iterator it=listSectionIds.begin();it!=listSectionIds.end();it++) {
		std::wstring sSectionId = *it;
		std::wstring sObjectName = mapSectionIdToObjectName[sSectionId];

		StringList* pListFields = QueryObjectInfo(m_pSession, sObjectName);

		//The field list could be null if the user does not have permissions to view this object
		if (pListFields!=NULL) {
			for (PairList::iterator itPair = listItems.begin(); itPair!=listItems.end(); itPair++) {
				//The first of this pair is the section id, the second of it is the field name
				StringPair pair = *itPair;
				
				//Make sure the user can actually see this field, otherwise we don't want to add it in here...
				if (pair.first==sSectionId) {
					if (find(pListFields->begin(),pListFields->end(),pair.second)!=pListFields->end()) {
						//AddFieldToMap(mapLayout,sObjectName,pair.second);
					} else {
						CCTILogger::Log(LOGLEVEL_MED,L"CCTIAppExchange::AddSoftphoneLayoutItemsToMap: Excluded field %s from object %s because it is not a valid field for this user.",pair.second,sObjectName);
					}
				}
			}
		}
	}
}

std::wstring CCTIAppExchange::GetFieldValue(ISForceSession4Ptr pSession, ISObject4Ptr pSObject,const wchar_t* sFieldName) 
{
	std::wstring sValue;
	try {
		IField4Ptr pField = pSObject->Item(_variant_t(sFieldName));
		if (pField) {
			_bstr_t bsField(pField->Value);
			sValue = CCTIUtils::BSTRToString(bsField);
			pField.Release();
		} else {
			CCTILogger::Log(LOGLEVEL_LOW,L"CCTIAppExchange::GetFieldValue: Unable to get field %s.",sFieldName);
		}
	}
	catch(_com_error ce)
	{
		CCTILogger::Log(LOGLEVEL_LOW,L"CCTIAppExchange::GetFieldValue: Error getting field %s: %s.",sFieldName,(wchar_t*)pSession->GetErrorMessage());
	}
	return sValue;
}

std::wstring CCTIAppExchange::GetValueFromQuery(std::wstring sQuery, const wchar_t* sField) {
	CCTILogger::Log(LOGLEVEL_MED,L"CCTIAppExchange::GetValueFromQuery: Query: %s\n",sQuery);
	std::wstring sValue;
	IQueryResultSet4Ptr	pQueryResults = NULL;
	
	try
	{
		pQueryResults = m_pSession->Query(CCTIUtils::StringToBSTR(sQuery),VARIANT_FALSE);

		ISObject4Ptr pSObject;
		BEGIN_FOREACH_QUERYRESULT(pQueryResults,pSObject)
			//Get the field value of the returned object
			sValue = GetFieldValue(m_pSession,pSObject,sField);
			//We only want the first row anyway -- this will break the FOREACH_QUERYRESULT loop
			break;
		END_FOREACH_QUERYRESULT()
	}
	catch(_com_error ce)
	{
		CCTILogger::Log(LOGLEVEL_LOW,L"CCTIAppExchange::GetValueFromQuery: Error getting query results: %s.",(wchar_t*)m_pSession->GetErrorMessage());
	}
	CCTILogger::Log(LOGLEVEL_MED,L"CCTIAppExchange::GetValueFromQuery: Got value from query: %s: %s\n",sField,sValue);
	return sValue;
}

void CCTIAppExchange::RunLogQuery(int nLayout, std::wstring& sObjectType, std::wstring& sId, CCTICallLog* pLog, RelObjSetList& listUiOutput)
{
	ObjectFieldMap mapLayout = GetMapForLayout(nLayout);

	//First, make sure this object is valid for this user (and get its fields)
	StringList* pListObjectFields = QueryObjectInfo(m_pSession, sObjectType);

	//Now see if this object is in the current layout
	StringList* listFields = mapLayout[sObjectType];

	//If it's not in the layout, look it up anyway so we can add it to the call log...
	bool bInLayout = (listFields!=NULL);

	//But only if the user can actually see this object.
	if (pListObjectFields!=NULL) {
		std::wstring sNameFieldsList;

		if (!bInLayout) {
			StringList* pListNameFields = m_mapNameFields[sObjectType];
			if (pListNameFields!=NULL) {
				size_t nIndex = 1;
				size_t nNumFields = pListNameFields->size();
				for (StringList::iterator it = pListNameFields->begin(); it!=pListNameFields->end(); it++) {
					sNameFieldsList += *it;
					if (nIndex<nNumFields) {
						//Add a comma if this is not the last name field
						sNameFieldsList += L",";
					}
					nIndex++;
				}
			}
		}

		//Create the query for this object for this layout
		std::wstring sQuery = L"Select ";
		sQuery += bInLayout?GetFieldListForObject(mapLayout,sObjectType):sNameFieldsList;
		sQuery += L" From "+sObjectType;
		sQuery += L" Where Id='" + sId +L"'";

		CCTILogger::Log(LOGLEVEL_MED,L"CCTIAppExchange::RunLogQuery: Generated query: %s.",sQuery);

		_bstr_t bsQuery = CCTIUtils::StringToBSTR(sQuery);
		IQueryResultSet4Ptr	pQueryResults = NULL;

		try
		{
			pQueryResults = m_pSession->Query(bsQuery,VARIANT_FALSE);
		}
		catch(_com_error ce)
		{
			CCTILogger::Log(LOGLEVEL_LOW,L"CCTIAppExchange::RunLogQuery: Error running query: %s.",(wchar_t*)m_pSession->GetErrorMessage());
		}

		if (pQueryResults) {
			if (bInLayout) {
				PopulateQueryResults(m_pSession,pQueryResults,mapLayout,listUiOutput,false);
			}
			
			//No matter what, we add the object to the log
			BEGIN_FOREACH_QUERYRESULT(pQueryResults,pSObject)
				std::wstring sObjectType = CCTIUtils::FirstCharToUpperCase(CCTIUtils::BSTRToString(pSObject->ObjectType));

				//Now get the name
				std::wstring sName = GetSObjectName(m_pSession, pSObject);

				//And the object label (like "Contact")
				std::wstring sObjectLabel = CCTIUtils::BSTRToString(pSObject->Label);

				pLog->AddObject(sId,sObjectLabel,sName,sObjectType,false);
			END_FOREACH_QUERYRESULT()

			pQueryResults.Release();
		}
	} else {
		CCTILogger::Log(LOGLEVEL_MED,L"CCTIAppExchange::GenerateLogQuery: The object %s is not valid for this user; could not create query with that object.",sObjectType);
	}
}

void CCTIAppExchange::AsyncSearch(std::wstring sCallObjectId, int nLayout, std::wstring sANI, PARAM_MAP& mapAttachedData,bool bFallBackOnANISearch)
{
	ObjectFieldMap mapLayout = GetMapForLayout(nLayout);

	//For an async search we prepare our queries in advance and send them off to the search thread for execution
	std::wstring sCallLogWho = mapAttachedData[KEY_LOG_WHO];
	std::wstring sCallLogWhat = mapAttachedData[KEY_LOG_WHAT];

	BSTRList listIVRQueries;
	GenerateSOQLQueries(nLayout,mapAttachedData,listIVRQueries);

	_bstr_t bsANIQuery = bFallBackOnANISearch?GeneratePhoneFieldsQuery(nLayout,sANI):L"";

	m_pSearchThread->Search(nLayout,sCallObjectId,sANI,sCallLogWho,sCallLogWhat,listIVRQueries,bsANIQuery);

	if (!mapAttachedData[KEY_COMMENTS].empty()) {
		CCTICallLog* pLog = m_pUI->GetCallLogForCallId(sCallObjectId);
		if (pLog) pLog->SetComments(mapAttachedData[KEY_COMMENTS]);
	}
}

void CCTIAppExchange::Search(int nLayout, std::wstring sANI, PARAM_MAP& mapAttachedData, CCTICallLog* pLog, RelObjSetList& listUiOutput, bool bFallBackOnANISearch)
{
	IQueryResultSet4Ptr	pQueryResults = NULL;
	bool bPerformANISearch = bFallBackOnANISearch;

	ObjectFieldMap mapLayout = GetMapForLayout(nLayout);

	if (!mapAttachedData.empty()) {
		bool bPerformIVRQuery = true;
		
		BSTRList listSOQLQueries;

		if (pLog) {
			std::wstring sWhoId;
			std::wstring sWhatId;
			//First see if there are any log whos or whats that we should show.  Those take priority.
			for (PARAM_MAP::iterator it = mapAttachedData.begin(); it!= mapAttachedData.end(); it++) {
				std::wstring sKey = it->first;
				std::wstring sValue = it->second;
				if (sKey==KEY_LOG_WHO || sKey==KEY_LOG_WHAT) {
					std::wstring sObjectType;
					std::wstring sId;

					size_t nColon = sValue.find(':');
					if (nColon!=std::wstring::npos && nColon!=sValue.length()-1) {
						sObjectType = sValue.substr(0,nColon);
						sId = sValue.substr(nColon+1,sValue.length()-nColon);
					} else {
						CCTILogger::Log(LOGLEVEL_LOW,L"CCTIAppExchange::Search: Error: no object type found in %s value %s.",sKey,sValue);
					}

					if (!sObjectType.empty()) {
						RunLogQuery(nLayout,sObjectType,sId,pLog,listUiOutput);

						if (sKey==KEY_LOG_WHO) {
							sWhoId = sId;
						} else sWhatId = sId;

						//No further searches needed.
						bPerformANISearch = false;
						bPerformIVRQuery = false;
					}
				} else if (sKey==KEY_COMMENTS) {
					pLog->SetComments(sValue);
				}
			}

			//Make sure the transferred who and what are both selected.
			pLog->SelectObject(sWhoId);
			pLog->SelectObject(sWhatId);
		}

		if (bPerformIVRQuery) {
			GenerateSOQLQueries(nLayout,mapAttachedData,listSOQLQueries);
		}

		if (!listSOQLQueries.empty()) {
			int nTotalQueryResults = 0;

			//Run the queries on the attached data and populate the UI list accordingly
			for (BSTRList::iterator itQuery=listSOQLQueries.begin();itQuery!=listSOQLQueries.end();itQuery++) {
				_bstr_t bsQuery = *itQuery;
				try
				{
					pQueryResults = m_pSession->Query(bsQuery,VARIANT_FALSE);
					nTotalQueryResults += PopulateQueryResults(m_pSession,pQueryResults,mapLayout,listUiOutput,false);
				}
				catch(_com_error ce)
				{
					CCTILogger::Log(LOGLEVEL_LOW,L"CCTIAppExchange::Search: Error running IVR query: %s.",(wchar_t*)m_pSession->GetErrorMessage());
				}
			}

			//We'll have to go perform an ANI search if the queries on the attached data returned no results.
			bPerformANISearch = bPerformANISearch && (nTotalQueryResults==0);
		}
	}

	if (bPerformANISearch && !sANI.empty()) {
		try
		{
			_bstr_t bSearch = GeneratePhoneFieldsQuery(nLayout,sANI);
			pQueryResults = m_pSession->Search(bSearch,VARIANT_FALSE);
			PopulateQueryResults(m_pSession,pQueryResults,mapLayout,listUiOutput,m_bPersonAccountsExist);
			pQueryResults.Release();
		}
		catch(_com_error ce)
		{
			CCTILogger::Log(LOGLEVEL_LOW,(wchar_t*)m_pSession->GetErrorMessage());
		}
	}
}

_bstr_t CCTIAppExchange::GeneratePhoneFieldsQuery(int nLayout, std::wstring sANI)
{
	std::wstring sQuery = L"FIND {"+sANI+L"} IN PHONE FIELDS RETURNING ";

	std::wstring sReturnSpec;

	ObjectFieldMap mapLayout = GetMapForLayout(nLayout);
	ObjectFieldMap::iterator itObjects = mapLayout.begin();
	while(itObjects!=mapLayout.end()) {
		std::wstring sObjectName = itObjects->first;
		StringList* pFieldList = itObjects->second;
		itObjects++;

		sReturnSpec += sObjectName + L"(";

		sReturnSpec += GetFieldListForObject(mapLayout,sObjectName);
		
		//Close it off and add a comma if there are more objects to go
		sReturnSpec += itObjects!=mapLayout.end()?L"),":L")";
	}

	sQuery += sReturnSpec;

	CCTILogger::Log(LOGLEVEL_MED,L"Generated query %s.",sQuery);

	return CCTIUtils::StringToBSTR(sQuery);
}

void CCTIAppExchange::GenerateSOQLQueries(int nLayout, PARAM_MAP& mapAttachedData, std::list<_bstr_t>& listOutputQueries)
{
	ObjectFieldMap mapLayout = GetMapForLayout(nLayout);

	//Maps objects to queries so that if more than one key shows up on the same object, we can coalesce it into a single query
	//So if there's a Case.CaseNumber and a Case.Priority, then we'll have a single query: "Where CaseNumber=x and Priority=y"
	PARAM_MAP mapObjectQueries;

	//Iterate through the mapAttachedData, finding any keys which map to the format Object.Field
	for (PARAM_MAP::iterator it=mapAttachedData.begin();it!=mapAttachedData.end();it++) {
		std::wstring sKey = it->first;
		std::wstring sValue = it->second;
		static const std::wstring::size_type nNotFound = -1;
		
		std::wstring::size_type indexPeriod = sKey.find(L".");

		if (indexPeriod!=nNotFound) {
			//We found a period.  See if what's before it corresponds to an object in the layout.
			std::wstring sObject = sKey.substr(0,indexPeriod);
			std::wstring sField = sKey.substr(indexPeriod+1,sKey.length()-indexPeriod-1);
			StringList* listFields = mapLayout[sObject];
			if (listFields!=NULL) {
				//This is an object that is present in our current layout.  Add a query for it.

				//First, make sure the query field is actually a valid field for this user
				StringList* pListObjectFields = QueryObjectInfo(m_pSession, sObject);
				if (pListObjectFields!=NULL && find(pListObjectFields->begin(),pListObjectFields->end(),sField)!=pListObjectFields->end()) {
					//See if we've already created a query for it
					std::wstring sQuery = mapObjectQueries[sObject];

					if (sQuery.length()==0) {
						//No existing query found.  Create a new one.
						sQuery += L"Select ";
						sQuery += GetFieldListForObject(mapLayout,sObject);

						sQuery += L" From "+sObject;

						sQuery += L" Where ";
					} else {
						//An existing query was found.  Add an and to the where clause.
						sQuery+=L" and ";
					}

					//Add the condition.
					sQuery += sField+L"=";

					//If the field is not numeric, we put quotes around the value.
					bool bQuoteValue = true;
					StringList* pListNonquotedFields = m_mapNonquotedFields[sObject];
					if (pListNonquotedFields!=NULL &&
						find(pListNonquotedFields->begin(),pListNonquotedFields->end(),sField)!=pListNonquotedFields->end()) {
						//The field is numeric.  Don't quote the value int he condition.
						bQuoteValue=false;
					}

					sQuery += bQuoteValue?L"'"+sValue+L"'":sValue;

					//Update the map with the latest query.
					mapObjectQueries[sObject]=sQuery;
				} else {
					CCTILogger::Log(LOGLEVEL_MED,L"CCTIAppExchange::GenerateSOQLQueries: The field %s of object %s is not valid for this user; could not create query with that field in the where clause.",sField,sObject);
				}
			}
		}
	}

	//Now we're done making queries.  Iterate through the map we used to build the queries and output the queries to the list output variable.
	for (PARAM_MAP::iterator itMap=mapObjectQueries.begin();itMap!=mapObjectQueries.end();itMap++) {
		std::wstring sQuery = itMap->second;
		CCTILogger::Log(LOGLEVEL_MED,L"Generated query %s.",sQuery);
		listOutputQueries.push_back(CCTIUtils::StringToBSTR(sQuery));
	}
}

ObjectFieldMap& CCTIAppExchange::GetMapForLayout(int nLayout)
{
	switch (nLayout) {
		case LAYOUT_INCOMING_INTERNAL:
			return m_mapInternalLayout;
		case LAYOUT_INCOMING_EXTERNAL:
			return m_mapInboundLayout;
		case LAYOUT_OUTBOUND:
			return m_mapOutboundLayout;
		default:
			//We should never get here, but we'll just return the inbound map by default.
			return m_mapInboundLayout;
	}
}

StringList* CCTIAppExchange::GetNameFields(std::wstring sObjectType)
{
	return m_mapNameFields[sObjectType];
}

StringList* CCTIAppExchange::GetInfoFieldsForLayout(int nLayout)
{
	switch (nLayout) {
		case LAYOUT_INCOMING_INTERNAL:
			return &m_listInternalInfoFields;
		case LAYOUT_INCOMING_EXTERNAL:
			return &m_listInboundInfoFields;
		case LAYOUT_OUTBOUND:
			return &m_listOutboundInfoFields;
		default:
			//We should never get here, but we'll just return the inbound map by default.
			return &m_listInboundInfoFields;
	}
}

std::wstring CCTIAppExchange::GetSObjectName(ISForceSession4Ptr pSession, ISObject4Ptr pSObject)
{
	//This is the value of the field called "Name," if one is available.  It takes precedence over anything we cobble together.
	std::wstring sNameFieldValue;
	
	//This is the name we'll cobble together from name fields if we can't find a field called "Name"
	std::wstring sAggregatedName;

	_variant_t vtFields = pSObject->Fields;

	//Iterate through the fields, finding name fields (or the Name field itself) and appending them to our aggregated name
	CComSafeArray<VARIANT> arrayFields;
	arrayFields.Attach(vtFields.parray);
	for (LONG i=arrayFields.GetLowerBound();i<=arrayFields.GetUpperBound();i++) {
		IField4Ptr pField = arrayFields[i];
		std::wstring sFieldName = CCTIUtils::BSTRToString(pField->Name);
		if (sFieldName==L"Name") {
			sNameFieldValue = CCTIUtils::VariantToString(pField->Value);
			break;
		}

		_variant_t vtIsNameField = pField->NameField;
		if (vtIsNameField.boolVal==VARIANT_TRUE) {
			//Add a space in there if we're appending more taxt onto an existing name
			if (!sAggregatedName.empty()) {
				sAggregatedName += L" ";
			}
			sAggregatedName += CCTIUtils::VariantToString(pField->Value);
		}
	}
	arrayFields.Detach();

	if (!sNameFieldValue.empty()) {
		return sNameFieldValue;
	} else { 
		return sAggregatedName;
	}
}

int CCTIAppExchange::PopulateQueryResults(ISForceSession4Ptr pSession, IQueryResultSet4Ptr pQueryResults, ObjectFieldMap& mapLayout, RelObjSetList& listUiOutput, bool bIgnorePersonContacts)
{
	int nNumResults = 0;
	SError queryResultError = pQueryResults->GetError();
	if(queryResultError==NO_SF_ERROR)
	{
		CCTILogger::Log(LOGLEVEL_MED,L"CCTIAppExchange::PopulateQueryResults: ANI query found %d results.",pQueryResults->GetSize());

		ISObject4Ptr pSObject;
		try {
			long nQueryResultSize = pQueryResults->Size;
			BEGIN_FOREACH_QUERYRESULT(pQueryResults,pSObject)
				std::wstring sObjectType = CCTIUtils::FirstCharToUpperCase(CCTIUtils::BSTRToString(pSObject->ObjectType));

				bool bIgnoreObject = false;
				if (bIgnorePersonContacts && sObjectType==L"Contact") {
					try {
						IField4Ptr pIsPersonAccountField = pSObject->Item(_variant_t(L"IsPersonAccount"));
						if (pIsPersonAccountField) {
							_variant_t vtIsPersonAccount = pIsPersonAccountField->Value;
							bIgnoreObject = (vtIsPersonAccount.boolVal == VARIANT_TRUE);
						} else {
							CCTILogger::Log(LOGLEVEL_MED,L"CCTIAppExchange::PopulateQueryResults: No IsPersonAccount field found.  Contact will be shown.");
						}
					}
					catch(_com_error ce)
					{
						CCTILogger::Log(LOGLEVEL_MED,L"CCTIAppExchange::PopulateQueryResults: No IsPersonAccount field found.  Contact will be shown.");
					}
				}

				//Get the ID of the returned object
				std::wstring sId = GetFieldValue(pSession,pSObject,L"Id");

				//Now get the name
				std::wstring sName = GetSObjectName(pSession, pSObject);

				StringList* pFieldList = mapLayout[sObjectType];
				StringList* pNameFieldList = GetNameFields(sObjectType);

				//No sense outputting the fields if we've got a whole bunch of results, all we really need is the name for multiple results anyway
				if (pFieldList && !pFieldList->empty() && !bIgnoreObject) {
					//Get a related object set that corresponds to this type of object
					CCTIRelatedObjectSet* pObjectSet = GetRelatedObjectSet(listUiOutput,pSObject);

					//Now we've got enough to create the related object
					CCTIRelatedObject* pRelatedObject = new CCTIRelatedObject(sId,sName);
					pObjectSet->AddRelatedObject(pRelatedObject);

					if (nQueryResultSize==1) {
						for (StringList::iterator itFields = pFieldList->begin();itFields!=pFieldList->end();itFields++) 
						{
							CCTIField* pField = NULL;
							//The name fields get special treatment -- we took care of them already
							if (find(pNameFieldList->begin(),pNameFieldList->end(),*itFields)==pNameFieldList->end()) {
								IField4Ptr field = pSObject->Item(CCTIUtils::StringToVariant(*itFields));

								//This is the API name of the field
								_bstr_t bsFieldName = field->Name;
								std::wstring sFieldId = CCTIUtils::BSTRToString(bsFieldName);

								//This is a nice localized label for the field
								std::wstring sFieldLabel = CCTIUtils::BSTRToString(field->Label);
								std::wstring sFieldValue = CCTIUtils::VariantToString(field->Value);

								pField = new CCTIField(sFieldId,sFieldLabel,sFieldValue);

								//Bool variants should be rendered as a checkbox
								pField->SetCheckbox(field->Value.vt == VT_BOOL);

								field.Release();
							} else {
								pField = new CCTIField(L"Name",L"",sName);
							}

							pRelatedObject->AddField(pField);
						}
					}
				}

				nNumResults++;
			END_FOREACH_QUERYRESULT()
		} catch (_com_error ce) {
			CCTILogger::Log(LOGLEVEL_LOW,L"CCTIAppExchange::PopulateQueryResults: Error while getting results from ANI query: %s.",(wchar_t*)pSession->GetErrorMessage());
		}
	} else {
		CCTILogger::Log(LOGLEVEL_LOW,L"CCTIAppExchange::PopulateQueryResults: Error in ANI query: %s.",(wchar_t*)pSession->GetErrorMessage());
	}

	return nNumResults;
}

void CCTIAppExchange::Reset(void)
{
	//Release the office toolkit session
	//m_pSession.Release();
	CCTILogger::Log(LOGLEVEL_MED,L"CCTIAppExchange::Reset: Clearing all cached info.");

	SetUserId(L"");
	SetSid(L"");
	SetInstance(L"");

	ResetObjectMap(m_mapInternalLayout);
	ResetObjectMap(m_mapInboundLayout);
	ResetObjectMap(m_mapOutboundLayout);
	ResetObjectMap(m_mapNameFields);
	ResetObjectMap(m_mapNonquotedFields);
	ResetObjectMap(m_mapObjectsToFields);
}

CCTIRelatedObjectSet* CCTIAppExchange::GetRelatedObjectSet(RelObjSetList& listOutput, ISObject4Ptr pSObject)
{
	std::wstring sObjectType = CCTIUtils::FirstCharToUpperCase(CCTIUtils::BSTRToString(pSObject->ObjectType));
	for (RelObjSetList::iterator it = listOutput.begin();it!=listOutput.end();it++) {
		CCTIRelatedObjectSet* pSet = *it;
		if (pSet->GetId()==sObjectType) {
			//Found it!
			return pSet;
		}
	}

	//If we get to this point, we haven't found the object set.  We'll create it now.
	std::wstring sObjectLabel = CCTIUtils::BSTRToString(pSObject->Label);
	//TODO: Get the plural label properly
	std::wstring sObjectPluralLabel = CCTIUtils::BSTRToString(pSObject->PluralLabel);
	//std::wstring sObjectPluralLabel = sObjectLabel+"s";

	CCTIRelatedObjectSet* pSet = new CCTIRelatedObjectSet(sObjectType,sObjectLabel,sObjectPluralLabel);
	listOutput.push_back(pSet);

	return pSet;
}

void CCTIAppExchange::AddFieldToMap(ObjectFieldMap& mapLayout, std::wstring sObject, std::wstring sField)
{
	StringList* pList = mapLayout[sObject];
	if (pList==NULL) {
		pList=new StringList();
		mapLayout[sObject] = pList;
	}
	pList->push_back(sField);
	CCTILogger::Log(LOGLEVEL_MED,L"CCTIAppExchange::AddFieldToMap: Added object %s, field %s to map.",sObject,sField);
}

ISForceSession4Ptr CCTIAppExchange::CreateOfficeToolkit()
{
	ISForceSession4Ptr pSession;
	HRESULT hr = pSession.CreateInstance(OFFICE_TOOLKIT_PROGID);
	pSession->PutUseCache(VARIANT_FALSE);
	if(FAILED(hr))
	{
		CCTILogger::Log(LOGLEVEL_LOW,L"CCTIAppExchange::CreateOfficeToolkit: Failed to create OfficeToolkit.");
		return NULL;
	}

	//Do not change this header, or else this class will be unable to access key tables in the AppExchange API.
	pSession->SetSoapHeader(L"CallOptions", L"client", CTI_CLIENT_KEY);
	pSession->SetSoapHeader(L"QueryOptions",L"batchSize",KEY_50);
	pSession->HTTPConnectTimeout = 11;
	pSession->HTTPReceiveTimeout = 11;

	return pSession;
}

ISForceSession4Ptr CCTIAppExchange::CreateOfficeToolkit(std::wstring sInstance, std::wstring sSid)
{
	ISForceSession4Ptr pSession = CCTIAppExchange::CreateOfficeToolkit();
	pSession->PutUseCache(VARIANT_FALSE);
	CCTIAppExchange::SetSessionInstanceAndSid(pSession,sInstance,sSid);

	return pSession;
}

void CCTIAppExchange::SetSessionInstanceAndSid(ISForceSession4Ptr pSession, std::wstring sInstance, std::wstring sSid)
{
	std::wstring sURL = sInstance+API_SUBPATH;

	try {
		std::wstring sCurrentUrl = CCTIUtils::BSTRToString(pSession->GetServerUrl());
		std::wstring sCurrentSid = CCTIUtils::BSTRToString(pSession->GetSessionId());

		if (sCurrentUrl!=sURL) {
			pSession->SetServerUrl(CCTIUtils::StringToBSTR(sURL));
			CCTILogger::Log(LOGLEVEL_MED,L"CCTIAppExchange::SetSessionInstanceAndSid: Setting URL to %s",sURL);
		}

		if (sCurrentSid!=sSid) {
			pSession->SessionId = CCTIUtils::StringToBSTR(sSid);
			CCTILogger::Log(LOGLEVEL_MED,L"CCTIAppExchange::SetSessionInstanceAndSid: Setting SID to %s",sSid);
		}
	} catch (_com_error ce) {
		CCTILogger::Log(LOGLEVEL_MED,L"CCTIAppExchange::SetSessionInstanceAndSid: Error setting instance and SID.  %s",pSession->GetErrorMessage());
	}
}

StringList* CCTIAppExchange::QueryObjectInfo(ISForceSession4Ptr pSession, std::wstring sObjectName)
{
	sObjectName = CCTIUtils::FirstCharToUpperCase(sObjectName);
	
	ObjectFieldMap::iterator itObject = m_mapObjectsToFields.find(sObjectName);

	if (itObject==m_mapObjectsToFields.end()) {
		//We haven't seen this object yet, so go get its info
		ISObject4Ptr pSObject = NULL;

		try {
			//Ensure that the user has CRUD permissions to access this object
			pSObject = pSession->CreateObject(CCTIUtils::StringToBSTR(sObjectName));
		} catch (_com_error ce) {
			//If we're here, the object couldn't be created (b/c the user probably doesn't have permissions for it), so we'll exclude this object from the list.
			CCTILogger::Log(LOGLEVEL_MED,L"CCTIAppExchange::QueryObjectInfo: The object %s has been excluded for this user; most likely the user does not have permissions for this object.  %s",sObjectName,(wchar_t*)pSession->GetErrorMessage());
			return NULL;
		}

		if (pSObject!=NULL) {
			StringList* pListAllFields = new StringList();
			StringList* pListNonquotedFields = new StringList();
			StringList* pListNameFields = new StringList();

			m_mapObjectsToFields[sObjectName] = pListAllFields;

			try {		        
				_variant_t vtFields = pSObject->Fields;

				//Iterate through the fields, finding name fields and numeric fields
				CComSafeArray<VARIANT> arrayFields;
				arrayFields.Attach(vtFields.parray);
				for (LONG i=arrayFields.GetLowerBound();i<=arrayFields.GetUpperBound();i++) {
					IField4Ptr pField = arrayFields[i];
					std::wstring sFieldName = CCTIUtils::BSTRToString(pField->Name);

					pListAllFields->push_back(sFieldName);

					LONG nFieldType = pField->VariantType;
					_variant_t vtIsNameField = pField->NameField;

					if (sFieldName.length()>0) {
						if (nFieldType==VT_I4 || nFieldType==VT_DECIMAL || nFieldType==VT_DATE) {
							CCTILogger::Log(LOGLEVEL_MED,L"CCTIAppExchange::QueryObjectInfo: Found non-quoted field %s in %s.",sFieldName,sObjectName);
							pListNonquotedFields->push_back(sFieldName);
						}

						if (vtIsNameField.boolVal==VARIANT_TRUE) {
							CCTILogger::Log(LOGLEVEL_MED,L"CCTIAppExchange::QueryObjectInfo: Found name field %s in %s.",sFieldName,sObjectName);
							pListNameFields->push_back(sFieldName);
						}
					}
				}
				arrayFields.Detach();
				pSObject.Release();
			} catch (_com_error ce) {
				CCTILogger::Log(LOGLEVEL_LOW,L"CCTIAppExchange::QueryObjectInfo: Error querying object info for %s: %s",sObjectName,(wchar_t*)pSession->GetErrorMessage());
			}

			if (!pListNonquotedFields->empty()) {
				m_mapNonquotedFields[sObjectName]=pListNonquotedFields;
			} else {
				//No fields to set, just delete it
				delete pListNonquotedFields;
			}

			if (!pListNameFields->empty()) {
				m_mapNameFields[sObjectName]=pListNameFields;
			} else {
				//No fields to set, just delete it
				delete pListNameFields;
			}

			if (sObjectName==L"Contact")
			{
				//Person accounts exist if we find a field on Contact called "IsPersonAccount"
				m_bPersonAccountsExist = (find(pListAllFields->begin(),pListAllFields->end(),L"IsPersonAccount")!=pListAllFields->end());
			}

			return pListAllFields;
		}
	} else {
		//We've already cached this object's info, just return it now.
		return itObject->second;
	}

	return NULL;
}

std::wstring CCTIAppExchange::GetFieldListForObject(ObjectFieldMap& mapLayout, std::wstring& sObject)
{
	std::wstring sFieldList;
	StringList* pFieldList = mapLayout[sObject];

	if (pFieldList!=NULL) {
		for(StringList::iterator itFields = pFieldList->begin();itFields!=pFieldList->end();itFields++) 
		{
			//Add the field to the list (and a comma if necessary)
			if (*itFields!=L"Name" && *itFields!=L"Id") {
				//The fields "Name" and "Id" are special cases which we'll handle in a moment.
				sFieldList += *itFields;
				sFieldList += L",";
			}
		}

		//Now we'll handle getting the name fields, which we always do regardless of whether the layout includes it.
		StringList* pNameList = m_mapNameFields[sObject];
		if (pNameList==NULL || pNameList->empty()) {
			//If there's no explicitly specified name field for an object (there always should be), then its name field must be "Name"
			sFieldList += L"Name,";
		} else {
			if ((pNameList->size()==1 && *(pNameList->begin())==L"Name") ||
				find(pNameList->begin(),pNameList->end(),L"Name")!=pNameList->end()) {
				//There's a name field called "Name."  Add it now instead of any other name fields that may arise.
				sFieldList += L"Name,";
			} else {
				//Add the name fields to the list
				for(StringList::iterator itNameFields = pNameList->begin();itNameFields!=pNameList->end();itNameFields++) 
				{
					std::wstring sNameField = *itNameFields;

					StringList::iterator itField = find(pFieldList->begin(),pFieldList->end(),sNameField);

					if (itField==pFieldList->end()) {
						//It wasn't already added by the layout -- add it now
						sFieldList += sNameField;
						sFieldList += L",";
					}
				}
			}
		}

		//And for all objects, we must return the Id field, which should never be explicitly specified in the layout.
		sFieldList += L"Id";

		//For Contacts, if IsPersonAccount exists, we have to add that as well
		if (sObject==L"Contact" && m_bPersonAccountsExist) {
			sFieldList += L",IsPersonAccount";
		}
	}

	return sFieldList;
}

void CCTIAppExchange::SetFieldValue(ISObject4Ptr pSObject, _variant_t sFieldName, std::wstring& sValue)
{
	SetFieldValue(pSObject,sFieldName,CCTIUtils::StringToBSTR(sValue));
}

void CCTIAppExchange::SetFieldValue(ISObject4Ptr pSObject, _variant_t sFieldName, _bstr_t sValue) 
{
	try {
		IField4Ptr pField = pSObject->Item(sFieldName);
		if (pField!=NULL) {
			pField->Value=sValue;
			pField.Release();
		} else {
			std::wstring sObjectName = CCTIUtils::BSTRToString(pSObject->Label);
			CCTILogger::Log(LOGLEVEL_LOW,L"CCTIAppExchange::SetFieldValue: Error setting field %s of object %s: field does not exist.",(CCTIUtils::VariantToString(sFieldName)),sObjectName);
		}
	} catch (_com_error ce) {
		CCTILogger::Log(LOGLEVEL_LOW,L"CCTIAppExchange::SetFieldValue: COM Error setting field %s to %s: %s.",(CCTIUtils::VariantToString(sFieldName)),(wchar_t*)sValue,(wchar_t*)ce.ErrorMessage());
	}
}

void CCTIAppExchange::LoadUserParamsFromCustomSetup() 
{
	std::wstring sKeyPath = L"CTI/";
	std::wstring sQuery = L"Select KeyPath,Value From CustomSetup Where ParentId='"+m_sUserId+L"' and KeyPath Like '"+sKeyPath+L"%'";

	PARAM_MAP parameters;

	IQueryResultSet4Ptr	pQueryResults = NULL;
	try
	{
		pQueryResults = m_pSession->Query(CCTIUtils::StringToBSTR(sQuery),VARIANT_FALSE);

		ISObject4Ptr pSObject;
		BEGIN_FOREACH_QUERYRESULT(pQueryResults,pSObject)
			//Get the key path of the returned object
			std::wstring sFullKeyPath = GetFieldValue(m_pSession,pSObject,L"KeyPath");

			//Get the value
			std::wstring sValue = GetFieldValue(m_pSession,pSObject,L"Value");

			//Chuck the full path, leaving only this subkey's relative path
			std::wstring sSubKeyPath = CCTIUtils::SearchAndReplace(sFullKeyPath,sKeyPath,L"");

			parameters[sSubKeyPath] = sValue;

			CCTILogger::Log(LOGLEVEL_MED,L"CCTIAppExchange::LoadUserParamsFromCustomSetup: Found user param %s=%s.",sSubKeyPath,sFullKeyPath);
		END_FOREACH_QUERYRESULT()
	}
	catch(_com_error ce)
	{
		CCTILogger::Log(LOGLEVEL_LOW,L"CCTIAppExchange::LoadUserParamsFromCustomSetup: Error loading user params from CustomSetup: %s.",(wchar_t*)m_pSession->GetErrorMessage());
	}

	if (pQueryResults) pQueryResults.Release();

	if (parameters.find(KEY_PASSWORD)!=parameters.end()) {
		//Decrypt the password
		
		std::wstring sPassword = parameters[KEY_PASSWORD];
		
		if (!sPassword.empty()) {
			//First get the key
			_bstr_t bsCodeUrl = CCTIUtils::StringToBSTR(m_sInstance+CODE_SERVLET_PATH);
			m_pSession->SetCookie(bsCodeUrl,"sid",CCTIUtils::StringToBSTR(m_sSid));
			_bstr_t bsCode = m_pSession->MakeHttpRequest(bsCodeUrl,KEY_GET,L"",L"",false,false);

			if (bsCode.length()>0) {
				//Get the reversed Offline Code key for this user
				std::wstring sKey = CCTIUtils::ReverseString(CCTIUtils::BSTRToString(bsCode));
				//Put the decrypted password in the map
				parameters[KEY_PASSWORD] = DecryptString(sPassword,sKey);
			} else {
				CCTILogger::Log(LOGLEVEL_LOW,L"CCTIAppExchange::LaunchSaveUserParamsThread: Unable to get encryption code from OfflineCode servlet.  Could not encrypt password.");
			}
		}
	}

	m_pUI->SetUserParametersMap(parameters);
}

void CCTIAppExchange::SaveUserParams(PARAM_MAP& parameters)
{
	if (m_pSaveThread) m_pSaveThread->SaveUserParamsToCustomSetup(parameters);
}

void CCTIAppExchange::SaveUserExtension(std::wstring& sExtension)
{
	if (m_pSaveThread) m_pSaveThread->SaveUserExtension(sExtension);
}

std::wstring CCTIAppExchange::EncryptString(std::wstring& sSource, std::wstring& sKey)
{
	UINT nMessageSize = (UINT)sSource.size();
	if (nMessageSize==0) return L"";

	//This will be both the input and output buffer.
	wchar_t* sMessage = new wchar_t[nMessageSize];
	wcsncpy(sMessage,sSource.c_str(),nMessageSize);

	UINT nMessageBytes = nMessageSize*sizeof(wchar_t);
	UINT nKeyBytes = (UINT)sKey.size() * sizeof(wchar_t);
	BYTE* pszMessage = (BYTE*)sMessage;
	const BYTE* pszKey = (BYTE*)sKey.c_str();

	for (UINT i=0;i<nMessageBytes;i++) {
		pszMessage[i] = (pszMessage[i%nMessageBytes] + pszKey[i%nKeyBytes])%255;
	}
	
	std::wstring sReturn = CCTIUtils::BytesToHexString(pszMessage,nMessageBytes);

	CCTILogger::Log(LOGLEVEL_MED,L"CCTIAppExchange::EncryptString: Cleaning up keys and buffers.");
	if (sMessage) delete sMessage;

#ifdef DEBUG
	std::wstring sDecrypted = DecryptString(sReturn,sKey);
	ATLASSERT(sSource == sDecrypted);
#endif

	return sReturn;
}

std::wstring CCTIAppExchange::DecryptString(std::wstring& sSource, std::wstring& sKey)
{
	//There are two characters in the string for every byte in the destination array.
	UINT nMessageBytes = (UINT)(sSource.size()/2);
	if (nMessageBytes==0) return L"";

	//Leave a spot on the end there for null terminators
	BYTE* pszMessage = new BYTE[nMessageBytes+sizeof(wchar_t)];
	//Init the buffer to NULLs
	memset(pszMessage,NULL,nMessageBytes+sizeof(wchar_t));
	int nKeyBytes = (UINT)sKey.size() * sizeof(wchar_t);
	const BYTE* pszKey = (BYTE*)sKey.c_str();

	CCTIUtils::HexStringToBytes(sSource,pszMessage,nMessageBytes);

	for (UINT i=0;i<nMessageBytes;i++) {
		pszMessage[i] = (pszMessage[i%nMessageBytes] - pszKey[i%nKeyBytes])%255;
	}

	wchar_t* pszDecoded = (wchar_t*)pszMessage;

	//Make sure there aren't any invalid characters that got decrypted
	//(if there are, the decryption likely failed)
	bool bInvalidChar = false;
	wchar_t cChar = NULL;
	int nChar=0;
	do {
		cChar = pszDecoded[nChar];
		if (cChar!=NULL && iswgraph(cChar)==0 && iswspace(cChar)==0) {
			//If it's not a printable character and it's not a whitespace character then it's an invalid character (and the decryption probably failed).  Put a space in its place.
			pszDecoded[nChar] = L' ';
			bInvalidChar = true;
		}
		nChar++;
	} while (cChar!=NULL);

	if (bInvalidChar) {
		CCTILogger::Log(LOGLEVEL_LOW,L"CCTIAppExchange::DecryptString: Error: decrypted text contained invalid characters and is probably corrupt.");
	}

	std::wstring sReturn = pszDecoded;
	delete pszMessage;

	return sReturn;
}

void CCTIAppExchange::UpsertCallLog(CCTICallLog* pLog) {
	if (m_pSaveThread) {
		m_pSaveThread->UpsertCallLog(pLog);
	} else {
		CCTILogger::Log(LOGLEVEL_LOW,L"CCTIAppExchange::UpsertCallLog: Could not save log; save thread does not exist.");
	}
}

void CCTIAppExchange::QueryRelatedData(std::wstring& sCallObjectId, std::wstring& sRelatedData)
{
	//There will be two IDs specified in the related data separated by a slash.
	StringList listIds;
	CCTIUtils::Split(sRelatedData,std::wstring(L"/"),listIds);
	
	for (StringList::iterator it=listIds.begin();it!=listIds.end();it++) {
		std::wstring sId = *it;

		if (!sId.empty()) {
			
		}
	}
}
