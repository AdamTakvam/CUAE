#ifndef __HTTP_HELPERS_H__
#define __HTTP_HELPERS_H__

const char MAKE_CALL_REQUEST_URI[]      = "http://%s:%u/click-to-talk/initiateCall";
const char STOP_RECORD_REQUEST_URI[]    = "http://%s:%u/click-to-talk/stopRecord";
const char ERROR_TAG[]                  = "error\r\n\r\n";
const char CONTENT_TYPE[]               = "text/xml";
const char CONTENT_TYPE_PLAIN[]         = "text/plain";


//////////////////////////////
//
// Extract the status code from a raw response.
//
inline int GetStatusCode(const char* response)
{
    int statusCode = -1;
    char statusStr[3];
    char* temp = (char*)response;

    memset(statusStr, 0, 3);

    if(temp != 0)
    {
        while(*temp != ' ')
        {
            temp++;
        }
        
        statusStr[0] = *(++temp);
        statusStr[1] = *(++temp);
        statusStr[2] = *(++temp);
    
        statusCode = atoi(statusStr);
    }

    return statusCode;
}


//////////////////////////////
//
// Extract the response, response length, and status code from the HTTP client.
//
inline int GetResponseFromClient(CAtlHttpClient& httpClient, int* statusCode, char** response)
{
    int responseLen = 0;

    responseLen = httpClient.GetResponseLength();

    // Did we get a response back?
    if(responseLen > 0)
    {
        *response = new char[responseLen + 1];
        memcpy(*response, httpClient.GetResponse(), responseLen);
        (*response)[responseLen] = 0;

        // Response received, so lets parse the status code out of it
        *statusCode = GetStatusCode(*response);
    }
    else
    {
        *statusCode = -1;
        *response = 0;
    }

    return responseLen;
}


//////////////////////////////
//
// Extract the response body and body length from the HTTP client.
//
inline int GetBodyFromClient(CAtlHttpClient& httpClient, char** body)
{
    int bodyLen = 0;
    
    bodyLen = httpClient.GetBodyLength();

    // Is there a body in the response?
    if(bodyLen > 0)
    {
        *body = new char[bodyLen + 1];
        memcpy(*body, httpClient.GetBody(), bodyLen);
        (*body)[bodyLen] = 0;
    }
    else
    {
        *body = 0;
    }

    return bodyLen;
}


//////////////////////////////
//
// Send a make call request to the application server.
//
inline bool SendMakeCallHttpRequest(char* appServer, DWORD appServerPort, std::string& msgStr, char** conferenceId)
{
    CAtlNavigateData navData;
    CAtlHttpClient httpClient;
    char requestUri[512];
	char* p = 0;
    char* response = 0;
    char* body = 0;
    long responseLen = 0;
    long bodyLen = 0;
    int statusCode = 0;
    bool retCode = false;

    // Create the request URI
    memset(requestUri, 0, sizeof(requestUri));
    _snprintf(requestUri, sizeof(requestUri) - 1, MAKE_CALL_REQUEST_URI, appServer, appServerPort);

    //   navData.SetSendStatusCallback((PFNATLSTATUSCALLBACK)SendStatusCallback, 0);
    //   navData.SetReadStatusCallback((PFNATLSTATUSCALLBACK)ReceiveStatusCallback, 0);

    // Set the method to POST and add our POST data to the body of the message
    navData.SetMethod(ATL_HTTP_METHOD_POST);
    navData.SetPostData((unsigned char*)msgStr.c_str(), (DWORD)msgStr.length(), CONTENT_TYPE);

    // Do the HTTP POST request
    httpClient.Navigate(requestUri, &navData);

    responseLen = GetResponseFromClient(httpClient, &statusCode, &response);
    bodyLen = GetBodyFromClient(httpClient, &body);

    httpClient.Close();

    if((responseLen == 0) || (statusCode == -1))
    {
        // No response, call failed.
        *conferenceId = 0;
        retCode = false;
    }
    else if(statusCode == 200)
    {
        // Successful response, do we have a conference ID ?
        if((bodyLen > 0) && (body != 0))
        {
			p = strstr(body, "\r\n\r\n");
			if (p != NULL)
			{
				// Yes, so save it off as an out parameter.
				// We don't copy the last four bytes as they are always \r\n\r\n.
				*conferenceId = new char[bodyLen - 3];
				memcpy(*conferenceId, body, bodyLen - 4);
				(*conferenceId)[bodyLen - 4] = 0;
			}
			else
			{
				*conferenceId = new char[bodyLen + 1];
				memcpy(*conferenceId, body, bodyLen);
				(*conferenceId)[bodyLen] = 0;
			}
        }
        else
        {
			// ATL HTTP Client may not parse the response string correctly, so let's do it ourself.
			p = strstr(response, "\r\n\r\n");
			if (p != NULL)
			{
				char* pszTemp = new char[responseLen];
				memset(pszTemp, 0, responseLen);
				strcpy(pszTemp, p+4);
				int len = (int)strlen(pszTemp);
				if (len > 0)
				{
					*conferenceId = new char[len+1];
					memcpy(*conferenceId, pszTemp, len);
					(*conferenceId)[len] = 0;
				}
				else
				{
		            *conferenceId = 0;
				}

				if (pszTemp != 0)
					delete [] pszTemp;
			}
			else
			{
	            *conferenceId = 0;
			}
        }

        retCode = true;
    }
    else
    {
        // Error reponse, call failed.
        *conferenceId = 0;
        retCode = false;
    }

	if (response != 0)
		delete response;

	if (body != 0)
		delete body;

    return retCode;
}

//////////////////////////////
//
// Send a stop recording request to the application server.
//
inline bool SendStopRecordHttpRequest(char* appServer, DWORD appServerPort, const char* conferenceId)
{
    CAtlNavigateData navData;
    CAtlHttpClient httpClient;
    char requestUri[512];
    char* response = 0;
    long responseLen = 0;
    int statusCode = 0;
    bool retCode = false;

	// No reason to go forward if the arguments are invalid
	if (!appServer || !conferenceId)
		return retCode;

    // Create the request URI
    memset(requestUri, 0, sizeof(requestUri));
    _snprintf(requestUri, sizeof(requestUri) - 1, STOP_RECORD_REQUEST_URI, appServer, appServerPort);

    //   navData.SetSendStatusCallback((PFNATLSTATUSCALLBACK)SendStatusCallback, 0);
    //   navData.SetReadStatusCallback((PFNATLSTATUSCALLBACK)ReceiveStatusCallback, 0);

    // Set the method to POST and add our POST data to the body of the message
    navData.SetMethod(ATL_HTTP_METHOD_POST);
    navData.SetPostData((unsigned char*)conferenceId, (DWORD)strlen(conferenceId), CONTENT_TYPE_PLAIN);

    // Do the HTTP POST request
    httpClient.Navigate(requestUri, &navData);

    responseLen = GetResponseFromClient(httpClient, &statusCode, &response);

    httpClient.Close();

    if((responseLen == 0) || (statusCode == -1))
    {
        // No response, stop record failed.
        retCode = false;
    }
    else if(statusCode == 200)
    {
        retCode = true;
    }
    else
    {
        retCode = false;
    }

	if (response != 0)
		delete response;

    return retCode;
}

#endif // __HTTP_HELPERS_H__