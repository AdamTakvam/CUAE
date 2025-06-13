#pragma once

#include <string>
#include <comutil.h>
#include <map>
#include <list>
#include <algorithm>
#include "CriticalSection.h"

#define OFFICE_TOOLKIT_PROGID OLESTR("SForceOfficeToolkit4.SForceSession4.1")

typedef std::map<std::wstring,std::wstring> PARAM_MAP;
typedef std::list<std::wstring> StringList;
typedef std::list<_bstr_t> BSTRList;

typedef std::pair<std::wstring,std::wstring> StringPair;
typedef std::list<StringPair> PairList;

/**
 * A utility class containing methods for conversions to and from COM data types and other such convenience methods
 *
 * @version 1.0
 */
class CCTIUtils
{
public:
	CCTIUtils(void);
	~CCTIUtils(void);

	/*****************************************
	 *   Utility functions                   *
	 *                                       *
	 *****************************************

	 Since we're working in a COM-based environment, these are handy to have around.
	 */

	/**
	 * Converts a VARIANT (or _variant_t) to a std::wstring.
	 *
	 * @param vt The VARIANT to convert.
	 * @return An std::wstring containing the contents of the variant.
	 */
	static inline std::wstring CCTIUtils::VariantToString(VARIANT& variant)
	{
		if (variant.vt!=VT_BOOL) {
			_bstr_t bs(variant);
			return std::wstring(static_cast<const wchar_t*>(bs));
		} else {
			return (variant.boolVal==VARIANT_TRUE?L"true":L"false");
		}
	}

	/**
	 * Converts an std::wstring to a _variant_t, which can be used anywhere a VARIANT is needed.
	 *
	 * @param str The std::wstring to convert.
	 * @return A _variant_t containing the contents of the string.
	 */
	static inline _variant_t CCTIUtils::StringToVariant(const std::wstring& str)
	{
		return _variant_t(str.c_str());
	}
	
	/**
	 * Converts a BSTR (or _bstr_t) to a std::wstring.
	 *
	 * @param bString The BSTR to convert.
	 * @return An std::wstring containing the contents of the BSTR.
	 */
	static inline std::wstring CCTIUtils::BSTRToString(const _bstr_t bString)
	{
		if(bString.length()==0) return L"";
		return std::wstring((wchar_t*)bString);
	}

	/**
	 * Converts an std::wstring to a _bstr_t, which can be used anywhere a BSTR is needed.
	 *
	 * @param str The std::wstring to convert.
	 * @return A _bstr_t containing the contents of the string.
	 */
	static inline _bstr_t CCTIUtils::StringToBSTR(const std::wstring& str)
	{
		_bstr_t bs(str.c_str());
		return bs;
	}

	/**
	 * Returns an upper case version of the input string.
	 *
	 * @param str The string to upper-case.
	 * @return An upper case version of the input string.
	 */
	static inline std::wstring CCTIUtils::ToUpperCase(const std::wstring& str)
	{
		std::wstring sReturn=str;
		transform (sReturn.begin(),sReturn.end(), sReturn.begin(), toupper);
		return sReturn;
	}

	/**
	 * Returns a version of the input string where the first wchar_tacter is upper case.
	 *
	 * @param str The string to convert.
	 * @return A version of the input string where the first wchar_tacter is upper case.
	 */
	static inline std::wstring CCTIUtils::FirstCharToUpperCase(const std::wstring& str)
	{
		std::wstring sReturn;
		if (str.size()>0) sReturn+=toupper(str.at(0));
		if (str.size()>1) sReturn+=str.substr(1,str.size()-1);
		return sReturn;
	}

	/**
	 * Converts an integer to a std::wstring.
	 *
	 * @param nNumber The integer to convert.
	 * @return An std::wstring containing the integer.
	 */
	static inline std::wstring IntToString(int nNumber)
	{
		wchar_t sInt[65];
		_itow(nNumber,sInt,10);
		return std::wstring(sInt);
	}

	/**
	 * Converts an std::wstring to an integer.
	 *
	 * @param sNumber The std::wstring to convert.
	 * @return An int containing the contents of the string (or the empty string if no int is found in the string).
	 */
	static inline int CCTIUtils::StringToInt(std::wstring sNumber) {
		return _wtoi(sNumber.c_str());
	}

	/**
	 * Searches for a target in a search string and replaces it with the given string.
	 *
	 * @param p_target The target to search for.
	 * @param p_search The string to search for the target in.
	 * @param p_replace The replacement string.
	 * @return A new string which is a copy of the old string where all the occurrences of the target have been replaced by the replacement string.
	 */
	static inline std::wstring CCTIUtils::SearchAndReplace( 
		const std::wstring& p_target,
		const std::wstring& p_search,
		const std::wstring& p_replace )
	{
		std::wstring str( p_target );
		std::wstring::size_type i = str.find( p_search );
	    
		// loop while replacing all occurrences
		while( i != std::wstring::npos )
		{
			str.replace( i, p_search.size(), p_replace );
			i = str.find( p_search, i + p_replace.size() );
		}

		return str;
	}

	/**
	 * Converts a byte array to a hexadecimal string format. The resulting string will have
	 * 2 x (length of byte array) characters.
	 *
	 * @param pBuf The byte array to read
	 * @param nLength The length of the byte array
	 * @return A string representing the byte array.
	 */
	static std::wstring BytesToHexString(BYTE* pBuf, UINT nLength)
	{
		std::wstring result;
		for ( UINT i=0; i < nLength; i++) 
		{

			wchar_t hexByte[3];
			if (pBuf[i] == 0) {
				swprintf(hexByte, L"00");
			}
			else if (pBuf[i] <= 15) 	{
				swprintf(hexByte, L"0%x",pBuf[i]);
			}
			else {
				swprintf(hexByte, L"%x",pBuf[i]);
			}

			result += hexByte;
		}
		return result;
	}

	/**
	 * Converts a hexadecimal string to a byte array. The string must have an even number of characters.
	 *
	 * @param hexString The hex string to convert.
	 * @param pBuf The destination buffer.
	 * @param nNumBytes The number of bytes the destination buffer can contain.
	 */
	static void HexStringToBytes(std::wstring hexString, BYTE* pBuf, UINT nNumBytes)
	{
		for (UINT i=0; i < nNumBytes && i<(hexString.size()/2); i++){
			int nextByte;
			swscanf(hexString.c_str() + (2 * i), L"%2x", &nextByte);
			pBuf[i] = (BYTE)nextByte;
		}
	}

	/**
	 * Returns the input phone number stripped of all punctuation except # and * (unless otherwise specified).
	 *
	 * @param sOriginal The orignal phone number.
	 * @param bStripStarAndHash True if this method should also strip # and * from the number.
	 *
	 * @return The stripped phone number.
	 */
	static std::wstring GetStrippedPhoneNumber(std::wstring& sOriginal, bool bStripStarAndHash=true) 
	{
		//Create a "stripped" version of the number, with just the digits
		const wchar_t* pszOriginal = sOriginal.c_str();
		wchar_t* sStrippedDN = new wchar_t[sOriginal.length()+1];
		int nStrippedPosition = 0;
		for (UINT i=0;i<sOriginal.length();i++) {
			//For some reason, the isdigit function occasionally crashes, so we'll use the ascii codes instead.
			if ((sOriginal.at(i)>='0' && sOriginal.at(i)<='9') ||
				(!bStripStarAndHash && (sOriginal.at(i)=='#' || sOriginal.at(i)=='*'))) {
				sStrippedDN[nStrippedPosition] = pszOriginal[i];
				nStrippedPosition++;
			}
		}
		//Null terminator
		sStrippedDN[nStrippedPosition]=NULL;
		
		std::wstring sReturn = sStrippedDN;
		delete sStrippedDN;
		return sReturn;
	}

	/**
	 * Returns true if the base string begins with the prefix string, false otherwise.
	 * For instance, if the base string were KEY_914155551212, calling this method with the prefix
	 * of KEY_91 would return true.
	 *
	 * @param sBase The base string.
	 * @param sPrefix The prefix string.
	 * @return True if the base string begins with the prefix string.
	 */
	static bool StringBeginsWith(std::wstring& sBase, std::wstring& sPrefix)
	{
		if (sPrefix.length()>sBase.length()) return false;
		std::wstring::iterator itBase = sBase.begin();
		
		for (std::wstring::iterator itPrefix = sPrefix.begin();itPrefix!=sPrefix.end();itPrefix++) {
			if (*itPrefix!=*itBase) {
				return false;
			}
			itBase++;
		}
		
		//If we're here, then all the prefix letters checked out, so the base does indeed begin with the prefix
		return true;
	}

	/**
	 * Returns true if the base string begins with the prefix string, false otherwise.
	 * For instance, if the base string were KEY_914155551212, calling this method with the prefix
	 * of KEY_91 would return true.
	 *
	 * @param sBase The base string.
	 * @param sPrefix The prefix string.
	 * @return True if the base string begins with the prefix string.
	 */
	static bool StringBeginsWith(std::wstring& sBase, wchar_t* sPrefix)
	{
		return StringBeginsWith(sBase,std::wstring(sPrefix));
	}

	/**
	 * Returns true if the base string ends with the postfix string, false otherwise.
	 * For instance, if the base string were KEY_914155551212, calling this method with the postfix
	 * of KEY_12 would return true.
	 *
	 * @param sBase The base string.
	 * @param sPrefix The prefix string.
	 * @return True if the base string ends with the postfix string.
	 */
	static bool StringEndsWith(std::wstring& sBase, std::wstring& sPostfix)
	{
		if (sPostfix.length()>sBase.length()) return false;
		std::wstring::reverse_iterator itBase = sBase.rbegin();
		
		for (std::wstring::reverse_iterator itPostfix = sPostfix.rbegin();itPostfix!=sPostfix.rend();itPostfix++) {
			if (*itPostfix!=*itBase) {
				return false;
			}
			itBase++;
		}
		
		//If we're here, then all the prefix letters checked out, so the base does indeed begin with the prefix
		return true;
	}

	
	/**
	 * Splits a string where it contains the delimiter and returns separate strings in the output list
	 *
	 * @param sSource The source string to split.  This string will remain unchanged.
	 * @param sDelimiter The delimiter on which to split.
	 * @param listOutput The list into which the split strings will be output.
	 * @param nInitialOffset (optional) The offset of the string to start splitting at.  Any characters prior to this offset will be ignored and not output as part of the split strings.
	 *
	 * @return The number of strings returned in the output array
	 */
	static int Split(const std::wstring& sSource, const std::wstring& sDelimiter, StringList& listOutput, std::wstring::size_type nInitialOffset=0)
	{
		int nSplits = 1;

		std::wstring::size_type offset = nInitialOffset;
		std::wstring::size_type sDelimiterIndex = 0;
	    
		sDelimiterIndex = sSource.find(sDelimiter, offset);

		while (sDelimiterIndex != std::wstring::npos)
		{
			std::wstring sSplitString = sSource.substr(offset, sDelimiterIndex - offset);
			if (!sSplitString.empty()) {
				listOutput.push_back(sSplitString);
				nSplits++;
			}
			offset += sDelimiterIndex - offset + sDelimiter.length();
			sDelimiterIndex = sSource.find(sDelimiter, offset);
		}

		listOutput.push_back(sSource.substr(offset));

		return nSplits;
	};

	/**
	 * Returns a new string containing the reverse of the input string.
	 *
	 * @param sInput The input string (which is not altered).
	 * @return The reverse of the input string.
	 */
	static std::wstring ReverseString(const std::wstring& sInput)
	{
		std::wstring sReturnString;
		sReturnString.resize(sInput.size());
		//Append the input string backwards to the return string using the reverse iterators
		sReturnString.insert(sReturnString.begin(),sInput.rbegin(),sInput.rend());
		return sReturnString;
	}
};



/**
 * Begins a FOREACH_QUERYRESULT loop that loops through each query result in a IQueryResultSet3Ptr and puts each 
 * individual query result in the input CComVariant variable.  It saves us from having to repeat a whole bunch
 * of boilerplate code every time we want to iterate through some query results.
 *
 * @param pQueryResults An IQueryResultSet3Ptr containing the query results.
 * @param pSObject A ISObject4Ptr into which each query result will be stored.
 */
#define BEGIN_FOREACH_QUERYRESULT(pQueryResults,pSObject) \
		if (pQueryResults) {\
			IUnknownPtr e = pQueryResults->Get_NewEnum();\
			IEnumVARIANT* pEnum;\
			ULONG lFetch;\
			CComVariant vtQueryResult;\
			ISObject4Ptr pSObject = NULL;\
			HRESULT hr = e->QueryInterface( IID_IEnumVARIANT, (void**) &pEnum );\
			while (pEnum->Next( 1, &vtQueryResult, &lFetch ) == S_OK && lFetch==1)\
			{\
				SError queryResultError = pQueryResults->Error;\
				if(queryResultError==NO_SF_ERROR)\
				{\
					pSObject = vtQueryResult.pdispVal;\
					vtQueryResult.Clear();

 /**
  *	Ends the FOREACH_QUERYRESULT loop.  Must always be called at some point after BEGIN_FOREACH_QUERYRESULT.
  */
#define END_FOREACH_QUERYRESULT() \
				}\
			}\
		}
