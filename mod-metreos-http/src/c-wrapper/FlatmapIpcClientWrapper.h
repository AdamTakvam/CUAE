// FlatmapIpcClientWrapper.h

#ifndef _FLATMAPIPCCLIENTWRAPPER_H_
#define _FLATMAPIPCCLIENTWRAPPER_H_

#include "c-wrapper.h"

/////////////////////////////////////////////////////////////////////
#define MESSAGE_APACHE_MODULE	3001            // Apache module flatmap IPC message type

/////////////////////////////////////////////////////////////////////
// The data structure to pass flatmap data between Apache module and flatmap client wrapper 
typedef struct type_flatmap_data {
	int count;                                  // How many data entries?
	int uuid_len;                               // Length of UUID data string 
	char uuid[48];                              // UUID string 
	char* data;					                // Real data
} flatmap_data;

/////////////////////////////////////////////////////////////////////
// The data header for each flatmap dta entry
typedef struct type_flatmap_data_header {
	int flatmap_data_type;                      // Flatmap data type
	int http_data_type;                         // HTTP data type
	int data_size;					            // Data size
} flatmap_data_header;

/////////////////////////////////////////////////////////////////////
typedef enum type_flatmap_datatype {
	FLATMAP_INT=1,				
	FLATMAP_BYTE=2, 
	FLATMAP_STRING=3, 
	FLATMAP_FLATMAP=4, 
	FLATMAP_LONG=5, 
	FLATMAP_DOUBLE=6
} flatmap_datatype;

/////////////////////////////////////////////////////////////////////
typedef enum type_http_datatype {
	HTTP_UUID=100,
	HTTP_VERSION=101,
	HTTP_METHOD=102,
	HTTP_URI=103, 
	HTTP_QUERY_PARAMETERS=104,
	HTTP_HAS_BODY=105,
	HTTP_BODY=106, 
	HTTP_HEADER=107,
	HTTP_COOKIE=108,
	HTTP_CONTENT_TYPE=109, 
	HTTP_CONTENT_LENGTH=110,
	HTTP_USERNAME=111,
	HTTP_PASSWORD=112,
	HTTP_RESPONSE_CODE=113,
	HTTP_RESPONSE_PHRASE=114,
	HTTP_HOST=115,
	HTTP_REMOTE_HOST=116,
	HTTP_APACHE_UUID=117
} http_datatype;

/////////////////////////////////////////////////////////////////////
// Callback function prototype for IPC data notifier
typedef int (*MessageNotifier)(const flatmap_data);

/////////////////////////////////////////////////////////////////////
#ifdef __cplusplus
extern "C" {
#endif

CWRAPPER_API int createFlatmapIpcClient();
CWRAPPER_API void destroyFlatmapIpcClient();
CWRAPPER_API int isFlatmapIpcClientAlive();
CWRAPPER_API int connectToFlatmapIpcServer(const char* pszServer, const unsigned int port);
CWRAPPER_API void disconnectFromFlatmapIpcServer();
CWRAPPER_API int isFlatmapIpcClientConnected();
CWRAPPER_API int sendReguestMessage(const flatmap_data* flatmap);
CWRAPPER_API void assignMessageNotifier(MessageNotifier pFunc);

#ifdef __cplusplus
}
#endif

#endif
