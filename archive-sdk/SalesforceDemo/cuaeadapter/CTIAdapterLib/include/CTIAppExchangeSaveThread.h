#pragma once
#include "CTIAppExchange.h"

class CCTICallLog;
class CSaveThreadInfo;
class CCTIUserInterface;

/**
 * @version 1.0
 * 
 * This class encapsulates a thread whose purpose is to save items to the Salesforce.com database via
 * the AppExchange API.  One use is creating and updating Task objects for call logs.  Another is saving
 * user login parameters.
 *
 * This thread exists so that data can be saved in the background without freezing the user interface.
 * The save thread is entirely managed by an instance of this class, which means that the thread gets 
 * created and started in the constructor of this class, and stopped and destroyed in the destructor.
 */
class CCTIAppExchangeSaveThread
{
protected:
	static bool m_bExiting; /**< A static flag that indicates to the save thread, if it's true, that the adapter is exiting, so it shouldn't make any calls to memory outside its thread space. */
	DWORD m_dwThreadId; /**< The thread ID of the save thread. */
	CCTIAppExchange* m_pAppExchange; /**< The parent CCTIAppExchange of this object. */
	HWND m_hWnd; /**< The handle of the hidden window used to send messages from the search thread to the main thread. */
	std::wstring m_sHiddenWindowClassName; /**< The class name of the hidden window.  It is generated dynamically before the window is created. */

	/**
	 * Launches the save thread.  This should only be called by this class's constructor
	 * so that there is a CCTIAppExchangeSaveThread instance that encapsulates the thread.
	 *
	 * @param pThreadParam 
	 * @return Always 0.
	 */
	static DWORD WINAPI RunThread(LPVOID pThreadParam);

	/**
	 * Causes the save thread encapsulated by this object to exit.  Should not be called directly;
	 * this method is called by this object's destructor, which is the preferred method of exiting the thread.
	 */
	void ExitThread();

	/************************************************************************************************
	 * Thread methods -- these are the methods that the save thread calls when it receives messages *
	 ************************************************************************************************/

	/**
	 * Creates a Task object for the input call log.
	 *
	 * @param pSession The Office Toolkit session.
	 * @param pInfo The Save Thread Info object that stores the information that this method requires.
	 */
	static void ThreadUpsertCallLog(ISForceSession4Ptr pSession,CSaveThreadInfo* pInfo);

	/**
	 * Saves the user parameters specified into CustomSetup's CTI/ subkey.
	 *
	 * @param pSession The Office Toolkit session.
	 * @param sInstance The current user's instance (like https://na1.salesforce.com)
	 * @param parameters The parameters to save.
	 */
	static void ThreadSaveUserParams(ISForceSession4Ptr pSession,CSaveThreadInfo* pThreadInfo);

	/**
	 * Initializes this save thread.
	 *
	 * @param pSession The Office Toolkit session.
	 * @param pAppExchange The AppExchange object that owns this thread.
	 */
	static void ThreadInitializeSave(ISForceSession4Ptr pSession, CCTIAppExchange* pAppExchange);

	/**
	 * Saves the input extension into the Extension field of the User object with the user ID stored in the CCTIAppExchange object.
	 *
	 * @param pSession The Office Toolkit session.
	 * @param sUserId The user ID for which to update the extension.
	 * @param sExtension The extension to save.
	 */
	static void ThreadSaveUserExtension(ISForceSession4Ptr pSession,std::wstring& sUserId,std::wstring& sExtension);
public:
	/**
	 * Creates an instance of CCTIAppExchangeSaveThread and starts the save thread.
	 */
	CCTIAppExchangeSaveThread(CCTIAppExchange* pAppExchange);

	/**
	 * Destroys this instance of CCTIAppExchangeSaveThread and stops the save thread.
	 */
	~CCTIAppExchangeSaveThread();

	/**
	 * Requests the save thread to create or update a Task object with the information 
	 * contained in the call log object.  When the save thread has finished, it
	 * will store the ID of the newly created task in the call log object's ID attribute 
	 * (or it will update the Task object with the ID that was previously saved in the call log)
	 *
	 * @param pLog The call log object.
	 */
	void UpsertCallLog(CCTICallLog* pLog);

	/**
	 * Saves the values in the input map to the user's CTI/ user key space in custom setup.
	 *
	 * Note that if the input map includes an entry called KEY_PASSWORD then that value will be
	 * encrypted prior to being saved to CustomSetup.
	 *
	 * @param parameters The key-value pairs to save to the custom setup table.
	 */
	void SaveUserParamsToCustomSetup(PARAM_MAP& parameters);

	/**
	 * Updates the extension field of the current user to the specified value.
	 *
	 * @param sExtension The current user's extension.
	 */
	void SaveUserExtension(std::wstring& sExtension);

	/**
	 * The WindowProc method that handles calls from the hidden window that this class
	 * uses to communicate with the main thread.
	 *
	 * @param hwnd The window handle.
	 * @param msg The message.
	 * @param wp The word param.
	 * @param lp The long param.
	 * @return Always 0.
	 */
	static LRESULT CALLBACK HiddenWindowProc(HWND hwnd, UINT msg, WPARAM wp, LPARAM lp);
};

/**
 * This class exists solely to provide information to CCTIAppExchangeSaveThread when commands are
 * issued to its underlying thread.  It generally does not need to be instantiated by classes
 * other than CCTIAppExchangeSaveThread.
 *
 * @version 1.0
 */
class CSaveThreadInfo
{
public:
	CSaveThreadInfo(CCTIAppExchange* pAppExchange);
	CSaveThreadInfo(CCTIAppExchange* pAppExchange, PARAM_MAP& parameters, HWND hHiddenWindow, CCTICallLog* m_pCallLog=NULL);

	/**
	 * This constructor is used when the save thread needs to pass information about the newly saved call log
	 * back to the main thread.
	 *
	 * @param pUI A pointer to the main user interface object.
	 * @param sCallObjectId The call object ID of the newly saved call log.
	 * @param sId The ID of the newly saved call log.
	 */
	CSaveThreadInfo(CCTIUserInterface* pUI, std::wstring sCallObjectId, std::wstring sId);

	HWND m_hHiddenWindow; /**< Handle to the hidden window which the save thread can use to communicate back to the main thread. */

	CCTIUserInterface* m_pUI; /**< Pointer to the main user interface object.  Note that this pointer should never be accessed from the context of the save thread -- it should only be passed back to the hidden window processor. */

	std::wstring m_sSid; /**< The current SID. */
	std::wstring m_sInstance; /**< The instance to connect to. */
	std::wstring m_sUserId; /**< The current user ID. */
	std::wstring m_sExtension; /**< The extension of the current user (for the SaveUserExtension method). */
	PARAM_MAP m_parameters; /**< A map of custom setup parameters to save (for the SaveUserParamsToCustomSetup method) */

	std::wstring m_sLogCallObjectId; /**< The call object identifier from the call log. */
	std::wstring m_sLogId; /**< The Salesforce.com ID of the call log, if it has one. */
	std::wstring m_sLogLabel; /**< The label of the call log. */
	std::wstring m_sLogComments; /**< The comments on the call log. */
	std::wstring m_sLogDisposition; /**< The call disposition of the call log. */
	int m_nLogDuration; /**< The duration in seconds of the call log. */
	int m_nLogCallType; /**< The call type of the call log. */
	std::wstring m_sTaskCompletedStatus; /**< The "task completed" status. */
	std::wstring m_sLogWhoId; /**< The who ID of the log. */
	std::wstring m_sLogWhatId; /**< The what ID of the log. */
	bool m_bLogDirty; /**< The dirty flag of the call log. */
};