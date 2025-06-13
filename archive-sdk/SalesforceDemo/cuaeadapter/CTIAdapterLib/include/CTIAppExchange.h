#pragma once
#include <map>
#include <list>
#include "stdafx.h"
#include <string>
#include "ctiobject.h"
#include "ctiuserinterface.h"
#include "CTIUtils.h"
#import "SF_MSApi4.dll" no_namespace named_guids

//This is the key used in all communications with SFDC.  The ID here is limited; partners should change this define to their own full-featured client ID.
#define CTI_CLIENT_KEY L"Cisco/1.0"
//This is the path to the web services API.  This should only be changed if Salesforce.com has moved to a new release of its web services API.
#define API_SUBPATH L"/services/Soap/c/8.0"

//This is the path to the code used to encrypt passwords.
#define CODE_SERVLET_PATH L"/servlet/servlet.OfflineCode"
//This is the path to the user-level keys in the CustomSetup table.
#define USER_KEY_PATH L"CTI/"

typedef std::map<std::wstring,StringList*> ObjectFieldMap;

class CCTIRelatedObjectSet;
class CCTIAppExchangeSaveThread;
class CCTIAppExchangeSearchThread;

class CSaveUserParamsInfo;

/** This class provides the adapter's link to Salesforce.com via Salesforce's AppExchange API.
 * @version 1.0
 *
 * This class provides the adapter's link to Salesforce.com via Salesforce's AppExchange API.  The AppExchange API
 * is a web services API, but access to that API is abstracted by the AppExchange Office Toolkit, a COM component.
 */
class CCTIAppExchange
{
protected:
	CCTIAppExchangeSaveThread* m_pSaveThread; /**< The worker thread that saves data to the AppExchange API. */
	CCTIAppExchangeSearchThread* m_pSearchThread; /**< The worker thread that searches for data in the AppExchange API. */

	/**
	 * The objects and fields for those objects that we should get for an internal call layout.
	 */
	ObjectFieldMap m_mapInternalLayout;
	/**
	 * The objects and fields for those objects that we should get for an external call layout.
	 */
	ObjectFieldMap m_mapInboundLayout;
	/**
	 * The objects and fields for those objects that we should get for an outbound call layout.
	 */
	ObjectFieldMap m_mapOutboundLayout;

	StringList m_listInboundInfoFields; /**< A list of the IDs of the standard info fields that should be shown in an inbound layout. */

	StringList m_listOutboundInfoFields; /**< A list of the IDs of the standard info fields that should be shown in an outbound layout. */

	StringList m_listInternalInfoFields; /**< A list of the IDs of the standard info fields that should be shown in an internal layout. */
	
	/**
	 * The fields that act as the name for their respective objects (e.g. for the Contact object, the fields are FirstName and LastName)
	 * Many objects actually have a "Name" field, but some don't.  For those that don't, we'll use this map to resolve the names.
	 */
	static ObjectFieldMap m_mapNameFields;

	/**
	 * A map relating all objects we've seen thus far to its set of field names.  We need this for generating queries and layouts.
	 */
	static ObjectFieldMap m_mapObjectsToFields;

	/**
	 * A map of objects to those objects' nonquoted fields.  We need to know this when generating SOQL queries, because numeric and date fields
	 * must be specified in those queries' conditions directly (as opposed to strings, which are quoted).
	 */
	static ObjectFieldMap m_mapNonquotedFields;

	std::wstring m_sInstance; /**< The instance which the current user is on (like "https://na1.salesforce.com") */

	std::wstring m_sTaskCompletedStatus; /**< The status that a task should be set to when it's closed (usually "Completed", but it's user-settable). */

	std::wstring m_sTaskSubject; /**< The task subject text that will be shown when call logs are created.  It's obtained from Salesforce.com, and it's different for every language. */

	/**
	 * Sets the instance which the current user is on (like "https://na1.salesforce.com")
	 *
	 * @param sInstance The instance which the current user is on (like "https://na1.salesforce.com")
	 */
	virtual void SetInstance(std::wstring sInstance) 
	{
		m_sInstance = sInstance;
	}
	
	std::wstring m_sSid; /**< The current user's session ID as granted by salesforce.com. */

	/**
	 * Sets the current user's session ID as granted by salesforce.com.
	 *
	 * @param sSid The current user's session ID as granted by salesforce.com.
	 */
	virtual void SetSid(std::wstring sSid) 
	{
		m_sSid=sSid;
	}

	std::wstring m_sUserId; /**< The current user's salesforce.com user ID. */

	/**
	 * Sets the current user's salesforce.com user ID.
	 *
	 * @param sUserId The current user's salesforce.com user ID.
	 */
	virtual void SetUserId(std::wstring sUserId) 
	{
		CCTILogger::Log(LOGLEVEL_MED,L"UserId set to %s.",sUserId);
		m_sUserId=sUserId;
	}

	ISForceSession4Ptr m_pSession; /**< The salesforce.com API session object. */

	bool m_bCanCreateCallLogs; /**< Flag indicating whether the current user can create call logs (which are Task objects). */
	
	static bool m_bPersonAccountsExist; /**< Flag indicating whether person accounts (a combination of accounts and contacts) exist in this user's organization. */
public:
	CCTIAppExchange(CCTIUserInterface* pUI);
	virtual ~CCTIAppExchange(void);

	CCTIUserInterface* m_pUI; /**< A pointer to the UI which instantiated this object. */

	/**
	 * Updates this object's instance and SID and sets it in the salesforce.com session.
	 * After calling this method, the salesforce.com session will be valid and ready for queries (as long as the SID is valid also).
	 *
	 * @param sInstance The instance which the current user is on (like "https://na1.salesforce.com"
	 * @param sSid The current user's session ID as granted by salesforce.com.
	 */
	virtual void UpdateSid(std::wstring sInstance, std::wstring sSid);

	/**
	 * This method will update cached information specific to the user.
	 * In particular, it will query for the user's call center information and layouts, then get an XSLT document in his language.
	 */
	virtual void UpdateCachedInformation();

	/**
	 * Clears out an object-field map and deletes all its sublists.
	 *
	 * @param map A reference to the map to reset.
	 */
	virtual void ResetObjectMap(ObjectFieldMap& map);

	/**
	 * @return True if the current user can create call logs (i.e. can create Task objects).
	 */
	virtual bool CanCreateCallLogs()
	{
		return m_bCanCreateCallLogs;
	}

	/**
	 * Searches the AppExchange API for the ANI (or for the data specified in mapAttachedData, if there is any).
	 *
	 * The data in mapAttachedData (a string-string map) should be specified in exactly this fashion:
	 * key: Object.Field
	 * value: InputValue
	 *
	 * So if a caller entered his case number through an IVR as "1003", then there should be a single entry in mapAttachedData which would look like:
	 * "Case.CaseNumber" = "1003" (where the left hand side of the equals is the key and the right hand side is the value)
	 * Which is to say that mapAttachedData["Case.CaseNumber"]=="1003".
	 * This function will generate a SOQL query based on this input and return any cases that match it.
	 *
	 * If mapAttachedData is empty, or if the SOQL query above generates no results, then this method will attempt
	 * a SOSL search on the ANI, returning objects as appropriate from 
	 *
	 * This method outputs CCTIRelatedObjectSet objects, for inclusion in a CCTILine.  
	 * Note that it is the responsibility of the caller to dispose of these objects when it is done with them.
	 *
	 * @param nLayout The layout to use for this search.  Should be one of LAYOUT_INCOMING_INTERNAL, LAYOUT_INCOMING_EXTERNAL, or LAYOUT_OUTBOUND.
	 * @param sANI The ANI for this call.
	 * @param mapAttachedData The attached data for this call as formatted in the description given here.
	 * @param pLog The call log that these search results should populate.
	 * @param listUiOutput A list of RelatedObjectSets that this method will output its results to.  It is the responsibility of the caller to delete any objects in this list.
	 * @param bFallBackOnANISearch (optional) True if this search should fall back on an ANI search if the attached data search has no results.  False otherwise.  Default is true.
	 */
	virtual void Search(int nLayout, std::wstring sANI, PARAM_MAP& mapAttachedData, CCTICallLog* pLog, RelObjSetList& listUiOutput, bool bFallBackOnANISearch=true);

	/**
	 * Instructs the CCTIAppExchangeSearchThread to begin an asynchronous search using the attached data.  The searches will be performed in the same order and 
	 * manner as the non-asynchronous version of this method; see the documentation for CCTIAppExchange::Search for details.
	 *
	 * @param sCallObjectId The unique call object identifier of this call.  The thread will use it upon search return to populate the search results into the UI.
	 * @param nLayout The layout to use for this search.  Should be one of LAYOUT_INCOMING_INTERNAL, LAYOUT_INCOMING_EXTERNAL, or LAYOUT_OUTBOUND.
	 * @param sANI The ANI for this call.
	 * @param mapAttachedData The attached data for this call as formatted in the description given here.
	 * @param pLog The call log that these search results should populate.
	 * @param bFallBackOnANISearch (optional) True if this search should fall back on an ANI search if the attached data search has no results.  False otherwise.  Default is true.
	 */
	virtual void AsyncSearch(std::wstring sCallObjectId, int nLayout, std::wstring sANI, PARAM_MAP& mapAttachedData,bool bFallBackOnANISearch=true);

	/**
	 * Returns the value of the input field from the first row of the result of the input SOQL query.
	 * Queries passed into this function should generally only return a single row (since the remaining rows will be ignored anyway).
	 *
	 * @param sQuery The SOQL query to run.
	 * @param sField The field whose value should be returned.
	 * @return The value of the input field from the first row of the result of the input SOQL query, or the empty string if there were no results or the field was missing.
	 */
	std::wstring GetValueFromQuery(std::wstring sQuery, const wchar_t* sField);

	/**
	 * Performs the query that gets all the softphone layout items and adds them to a list of pairs.
	 * We use a list of pairs instead of a map because we need to preserve the order of the items, which a map would not do.
	 *
	 * @param sSoftphoneLayoutId The softphone layout Id for which to gather the items
	 * @param listItems The list of pairs to populate.
	 */
	void GetSoftphoneLayoutItems(std::wstring sSoftphoneLayoutId, PairList& listItems);

	/**
	 * Adds a list of pairs obtained from GetSoftphoneLayoutItems to the appropriate layout map.
	 *
	 * @param listItems The list of pairs of items.
	 * @param listSectionIds The list of section ids to add to this map.
	 * @param mapSectionIdToObjectName The map of section ids to object names.
	 * @param mapLayout The layout map to populate.
	 */
	void AddSoftphoneLayoutItemsToMap(PairList& listItems, StringList& listSectionIds, PARAM_MAP& mapSectionIdToObjectName, ObjectFieldMap& mapLayout);

	/**
	 * Generates a BSTR-formatted (for the Office Toolkit) SOSL query according to the input layout and using the input ANI.
	 *
	 * @param nLayout The layout to use in the search.
	 * @param sANI The ANI to use in the search.
	 * @return A BSTR-formatted SOSL search query.
	 */
	virtual _bstr_t GeneratePhoneFieldsQuery(int nLayout, std::wstring sANI);

	/**
	 * Generates BSTR-formatted (for the Office Toolkit) SOQL queries according to the input layout and 
	 * using the input map of attached data.  Note that this method will only act upon data that is formatted
	 * in such a way that the key is of the form "Object.Field", where "Object" corresponds
	 * to the developer name of an object defined in Salesforce.com, and "Field" corresponds to the developer name
	 * of a field in the same.  So "Case.CaseNumber" would be a valid key.  The values corresponding these keys
	 * should be the values that the query should search on; so if there's an entry that is
	 * "Case.CaseNumber"->"1001" (assuming that the input layout actually contains the Case object),
	 * then this method will create a query on cases where Case.CaseNumber=1001.
	 *
	 * In the event that the input map is empty, the map has no keys of the format described above, or none
	 * of the keys correspond to objects that are in the layout, then the list of queries will be empty.
	 *
	 * Note that this method may create multiple queries, and as such the queries are outputted in the listOutputQueries parameter.
	 * One query will be created for each object type specified in the mapAttachedData.  So, for instance, if there's one key
	 * called "Contact.EmployeeId" and another called "Case.CaseNumber", then two queries will be generated, one for the contact
	 * and one for the case.  If, on the other hand, there are two keys, such as "Case.CaseNumber" and "Case.Priority", that refer
	 * to the same object, then only one query will be generated for the Case object, but with multiple conditions on it.
	 *
	 * @param nLayout The layout to use in the queries.
	 * @param mapAttachedData The map of attached data to generate queries for.
	 * @param listOutputQueries A list of queries that this method has generated.  This list should be empty when it's passed in, as it will contain the results of this method.
	 * @return 
	 */
	virtual void GenerateSOQLQueries(int nLayout, PARAM_MAP& mapAttachedData, std::list<_bstr_t>& listOutputQueries);

	/**
	 * Generates a SOQL query for the input log item (which consists of an object type and ID) and runs it,
	 * adding the results to the call log, and to the layout, if the object is in fact a member of the layout.
	 *
	 * @param nLayout The layout to use.
	 * @param sObjectType The object type (like "Contact")
	 * @param sId The ID of the object to search for.
	 * @param pLog The log to which this method will be adding its result.
	 * @param listUiOutput A list of CCTIRelatedObjectSet objects into which this method will output its results.  Should be empty when passed in.
	 */
	virtual void RunLogQuery(int nLayout, std::wstring& sObjectType, std::wstring& sId, CCTICallLog* pLog, RelObjSetList& listUiOutput);

	/**
	 * Returns the proper layout map for the input layout.
	 *
	 * @param nLayout The layout to get the map for.  Should be one of LAYOUT_INCOMING_INTERNAL, LAYOUT_INCOMING_EXTERNAL, or LAYOUT_OUTBOUND.
	 * @return The correct map for the specified layout.
	 */
	virtual ObjectFieldMap& GetMapForLayout(int nLayout);

	/**
	 * Returns the proper list of standard info fields for the input layout.
	 *
	 * @param nLayout The layout to get the info fields for.  Should be one of LAYOUT_INCOMING_INTERNAL, LAYOUT_INCOMING_EXTERNAL, or LAYOUT_OUTBOUND.
	 * @return The correct list of info fields for the specified layout.
	 */
	StringList* GetInfoFieldsForLayout(int nLayout);

	/**
	 * Returns the name fields for the object type specified.
	 *
	 * @param sObjectType The object type (like "Contact").
	 * @return A list of the name fields for the object.
	 */
	static StringList* GetNameFields(std::wstring sObjectType);

	/**
	 * Gets a CCTIRelatedObjectSet object corresponding to the input SObject, creating the CCTIRelatedObjectSet and adding it to the output list
	 * if necessary.
	 *
	 * @param listOutput The list of CCTIRelatedObjectSet objects that the caller method is working on.
	 * @param pSObject The SObject for which to find the corresponding CCTIRelatedObjectSet.
	 */
	static CCTIRelatedObjectSet* GetRelatedObjectSet(RelObjSetList& listOutput, ISObject4Ptr pSObject);

	/**
	 * Resets the instance and session ID and invalidates the salesforce.com session.  This should be called on logout. 
	 */
	virtual void Reset();

	/**
	 * Sets a field of the input SObject to the desired value.  This is a static method so it can be used from any thread.
	 *
	 * @param pSObject The SObject on which to set the value.
	 * @param sFieldName The name of the field on which to set the value.
	 * @param sValue The value to set the field to.
	 */
	static void SetFieldValue(ISObject4Ptr pSObject, _variant_t sFieldName, std::wstring& sValue);

	/**
	 * Sets a field of the input SObject to the desired value.  This is a static method so it can be used from any thread.
	 *
	 * @param pSObject The SObject on which to set the value.
	 * @param sFieldName The name of the field on which to set the value.
	 * @param sValue The value to set the field to.
	 */
	static void SetFieldValue(ISObject4Ptr pSObject, _variant_t sFieldName, _bstr_t sValue);

	/**
	 * Returns the value of the specified field in the input SObject, or the empty string if the field is empty or nonexistent.
	 *
	 * @param pSession The Sforce session in which the object exists.
	 * @param pSObject The SObject from which to get the field value.
	 * @param sFieldName The name of the field to retrieve.
	 * @return The value of the specified field in the input SObject, or the empty string if the field is empty or nonexistent.
	 */
	static std::wstring GetFieldValue(ISForceSession4Ptr pSession, ISObject4Ptr pSObject,const wchar_t* sFieldName);

	/**
	 * Gets the contents of the name fields of the input SObject.
	 *
	 * @param pSession The Sforce session in which the object exists.
	 * @param pSObject The SObject from which to get the name.
	 * @return The contents of the name fields of the SObject.
	 */
	static std::wstring GetSObjectName(ISForceSession4Ptr pSession, ISObject4Ptr pSObject);

	/**
	 * Takes as input the results of a query performed by the Office Toolkit, and populates those results
	 * into a list of CCTIRelatedObjectSets.  Note that the bIgnorePersonContacts parameter should generally be true for SOSL
	 * searches in orgs that have Person Accounts enabled, because in that case a phone number search may result in both
	 * an Account and a Contact that actually point to the same Person Account record (and so we only want to show the account
	 * and not the Contact).
	 *
	 * Note that this method is static because it is used by both the synchronous and asynchronous versions of search.
	 *
	 * @param pSession The session in which these query results are valid.
	 * @param pQueryResults The results of a query (SOSL or SOQL) as returned by the Office Toolkit.
	 * @param mapLayout The layout to use when populating these results.  
	 * @param listUiOutput A list of CCTIRelatedObjectSet objects into which this method will output its results.  Should be empty when passed in.
	 * @param bIgnorePersonContacts True if this method should ignore contacts where the IsPersonAccount field exists and is set to true.
	 * @return The number of query results that were populated.
	 */
	static int PopulateQueryResults(ISForceSession4Ptr pSession, IQueryResultSet4Ptr pQueryResults, ObjectFieldMap& mapLayout, RelObjSetList& listUiOutput, bool bIgnorePersonContacts);

	/**
	 * Adds a Salesforce.com field (and its corresponding Salesforce.com object, if necessary) to one of the layout maps.
	 *
	 * @param mapLayout The layout map to add the field to.
	 * @param sObject The Salesforce.com object owner of the field.
	 * @param sField The Salesforce.com field to add.
	 */
	virtual void AddFieldToMap(ObjectFieldMap& mapLayout, std::wstring sObject, std::wstring sField);

	/**
	 * Sets the instance and SID of the input Office Toolkit session object.
	 *
	 * @param pSession An Office Toolkit session object.
	 * @param sInstance The instance which the current user is on (like "https://na1.salesforce.com"
	 * @param sSid The current user's session ID as granted by salesforce.com.
	 */
	static void SetSessionInstanceAndSid(ISForceSession4Ptr pSession, std::wstring sInstance, std::wstring sSid);

	/**
	 * Creates an instance of the Office Toolkit COM object and without setting its instance and SID.
	 * The SetSessionInstanceAndSid method of this class must be called with this session to give it
	 * an instance and a SID prior to its use.
	 *
	 * @return The ISForceSession4Ptr that was created, or NULL if it could not be created.
	 */
	static ISForceSession4Ptr CreateOfficeToolkit();

	/**
	 * Creates an instance of the Office Toolkit COM object and immediately sets its instance and SID to the input values.
	 *
	 * @param sInstance The instance which the current user is on (like "https://na1.salesforce.com"
	 * @param sSid The current user's session ID as granted by salesforce.com.
	 *
	 * @return The ISForceSession4Ptr that was created, or NULL if it could not be created.
	 */
	static ISForceSession4Ptr CreateOfficeToolkit(std::wstring sInstance, std::wstring sSid);

	/**
	 * Queries the AppExchange API for information about the input object and stores that information in this CCTIAppExchange object
	 * for future reference from SOQL and SOSL queries.
	 *
	 * @param pSession The session to use to obtain information about the object.
	 * @param sObject The name of the object to get info about.
	 *
	 * @return A list of all the fields the object contains, or NULL if the user does not have permissions to view this object.
	 */
	static StringList* QueryObjectInfo(ISForceSession4Ptr pSession, std::wstring sObjectName);

	/**
	 * Generates a comma-separated list of fields to retrieve based on the input layout for the input object.
	 * Note that in addition to the fields explicitly specified in the layout, this method will also add the name fields
	 * and the Id field to the list.
	 *
	 * The list output by this method is useful in both SOSL and SOQL queries (SOSL in the return spec, SOQL in the Select)
	 *
	 * @param mapLayout The map of the layout to use.
	 * @param sObject The object for which to get the field list.
	 * @return A comma-separated list of fields to get for this object.
	 */
	static std::wstring GetFieldListForObject(ObjectFieldMap& mapLayout, std::wstring& sObject);

	/**
	 * Gets all the user keys for the current user beneath the "CTI/" path in the CustomSetup 
	 * table places them in the associated CCTIUserInterface subclass's map of user params.
	 *
	 * Note that if an entry called "PASSWORD" is found, this method will assume it's encrypted and
	 * attempt to decrypt it.
	 */
	virtual void LoadUserParamsFromCustomSetup();

	/**
	 * Saves the values in the input map to the user's CTI/ user key space in custom setup.
	 * This method operates asynchronously by calling CCTIAppExchangeSaveThread to perform
	 * the saving in the background.
	 *
	 * Note that if the input map includes an entry called "PASSWORD" then that value will be
	 * encrypted prior to being saved to CustomSetup.
	 *
	 * @param parameters The key-value pairs to save to the custom setup table.
	 */
	virtual void SaveUserParams(PARAM_MAP& parameters);

	/**
	 * Requests the save thread to update the extension field of the current user 
	 * to the specified value.
	 *
	 * @param sExtension The current user's extension.
	 */
	void SaveUserExtension(std::wstring& sExtension);

	/**
	 * Encrypts the source string using the specified key.
	 *
	 * @param sSource The string to encrypt.
	 * @param sKey The encryption key.
	 * @return The encrypted string as a string of hex digits.
	 */
	static std::wstring EncryptString(std::wstring& sSource, std::wstring& sKey);

	/**
	 * Decrypts the source string using the specified key.
	 *
	 * @param sSource The string to decrypt (should be a string of hex digits).
	 * @param sKey The encryption key.
	 * @return The decrypted string.
	 */
	static std::wstring DecryptString(std::wstring& sSource, std::wstring& sKey);

	/**
	 * @return The User ID of the currently logged in user.
	 */
	virtual std::wstring GetUserId() {
		return m_sUserId;
	}

	/**
	 * @return The SID of the currently logged in user.
	 */
	virtual std::wstring GetSid() {
		return m_sSid;
	}

	/**
	 * @return The instance of the currently logged in user.
	 */
	virtual std::wstring GetInstance() {
		return m_sInstance;
	}

	/**
	 * @return The user interface that owns this object.
	 */
	virtual CCTIUserInterface* GetCTIUserInterface()
	{
		return m_pUI;
	}

	/**
	 * Gets the task subject text that will be shown when call logs are created.
	 *
	 * @return The task subject text that will be shown when call logs are created.
	 */
	virtual std::wstring GetTaskSubject()
	{
		return m_sTaskSubject;
	}

	/**
	 * Sets the task subject text that will be shown when call logs are created.
	 *
	 * @param sTaskSubject The task subject text that will be shown when call logs are created.
	 */
	virtual void SetTaskSubject(std::wstring sTaskSubject) { m_sTaskSubject=sTaskSubject; };
	
	/**
	 * Gets the status that a task should be set to when it's closed (usually "Completed", but it's user-settable).
	 *
	 * @return The status that a task should be set to when it's closed.
	 */
	virtual std::wstring GetTaskCompletedStatus()
	{
		return m_sTaskCompletedStatus;
	}

	/**
	 * Sets the status that a task should be set to when it's closed (usually "Completed", but it's user-settable).
	 *
	 * @param sTaskCompletedStatus The status that a task should be set to when it's closed.
	 */
	virtual void SetTaskCompletedStatus(std::wstring sTaskCompletedStatus) { m_sTaskCompletedStatus=sTaskCompletedStatus; };

	/**
	 * Creates or updates a Task for a call log (by passing it off to the CCTIAppExchangeSaveThread for background saving).
	 *
	 * @param pLog The call log to create or update.
	 */
	virtual void UpsertCallLog(CCTICallLog* pLog);

	/**
	 * Queries the related data referred to by the 2 IDs expected to be in the sRelatedData parameter and adds
	 * information about them to the call log pertaining to the input call object ID.
	 *
	 * @param sCallObjectId The call object ID corresponding to the log that this related data should be added to.
	 * @param sRelatedData The contents of the related data attached item.  Should contain at most 2 IDs separated by a slash, like "
	 */
	virtual void QueryRelatedData(std::wstring& sCallObjectId, std::wstring& sRelatedData);

	/**
	 * Calls describeSoftphoneLayout on the server and populates the layout and info field maps with the results.
	 */
	virtual void DescribeSoftphoneLayout();
};
