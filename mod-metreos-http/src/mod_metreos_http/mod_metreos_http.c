/* mod_metreos_http.c */

/* Metreos HTTP module is an apache module based on Apache API 1.3 serving as proxy to accept any HTTP
   requests on a pre-defined port (8000).  This module then passes along an unique Apache transaction ID with original
   request data using Flatmap IPC to Metreos Application Server's HTTP provider and wait for Application Server 
   to return response data.  As soon as Metreos HTTP module receives the Flatmap data from HTTP provider, it 
   repackages the data into Apache's HTTP response structure then back to the requesting HTTP client. */ 

/* Besides Apache API 1.3, he module also uses cpp-framework's IPC code (which uses ACE framework) as the communication core.  Since cpp-framework
   is a C++ based library and Apache module is written in C, c-wrapperd.dll is basically a c language wrapper around cpp-framework's 
   IPC classes */

/* We discovered Apache Core Thread API does not get along well with ACE framework's threading model, we do use Windows base threading 
   modle in cpp-framework, which compiled using WIN_THREAD preprocessor, and cpp-core_wind.dll is the DLL we want to use for this module. */

/* Apache includes */
#include "httpd.h"
#include "http_config.h"
#include "http_core.h"
#include "http_log.h"
#include "http_main.h"
#include "http_protocol.h"
#include "http_request.h"
#include "http_conf_globals.h"
#include "util_script.h"
#include "multithread.h"

/* cpp-framework c-wrapper class includes */
#include "FlatmapIpcClientWrapper.h"

/*******************************************************************/
#define	DEFAULT_VHOST_SERVER_NAME		"metreos"               /* default Apache virtual server name, which defined in httpd.conf  (or module's conf file) */
#define DEFAULT_FLATMAP_SERVER_ADDRESS	"127.0.0.1"             /* Metreos Application Server IP address */
#define DEFAULT_FLATMAP_PORT			9434                    /* Port number for IPC communication between Apache module and Application Server */
#define MAX_UUID_LEN					48						/* Apache unique transaction id data length, Windows is 24 bytes long */
#define MAX_PENDING_REQUESTS			1000                    /* Array size for pending requests */
#define DEFAULT_HANDLER_TIMEOUT			300						/* HTTP request time out value, 300 secs */	
#define HEADER_SESSION_ID				"Metreos-SessionID"     /* Metreos Session ID in HEADER section*/
#define COOKIE_SESSION_ID				"metreosSessionId"      /* Metreos Session ID in COOKIE session */
#define QUERY_SESSION_ID				"metreossessionid"      /* Metreos Session ID in QUERY session */
#define METREOS_UNIQUE_ID				"METREOS_UNIQUE_ID"     /* HTTP unique transaction ID for Metreos HTTP requests */

/*******************************************************************/
/* Standard HTTP Header entry identifier for HTTP 1.0/1.1 */
#define	HTTP_UUID_NAME					"UUID"
#define HTTP_VERSION_NAME				"VERSION"
#define HTTP_METHOD_NAME				"METHOD"
#define HTTP_URI_NAME					"URI"
#define HTTP_QUERY_PARAMETERS_NAME		"QUERY_PARAMETERS"
#define HTTP_HAS_BODY_NAME				"HAS_BODY"					
#define HTTP_BODY_NAME					"BODY"					
#define HTTP_HEADER_NAME				"HEADER"
#define HTTP_COOKIE_NAME				"COOKIE"
#define HTTP_CONTENT_TYPE_NAME			"CONTENT-TYPE"
#define HTTP_CONTENT_LENGTH_NAME		"CONTENT-LENGTH"
#define HTTP_CONTENT_ENCODING_NAME		"CONTENT-ENCODING"
#define HTTP_CONTENT_LANGUAGE_NAME		"CONTENT-LANGUAGE"
#define HTTP_USERNAME_NAME				"USERNAME"
#define HTTP_PASSWORD_NAME				"PASSWORD"
#define HTTP_RESPONSE_CODE_NAME			"RESPONSE_CODE"
#define HTTP_RESPONSE_PHRASE_NAME		"RESPONSE_PHRASE"
#define HTTP_HOST_NAME					"HOST"
#define HTTP_REMOTE_HOST_NAME			"REMOTE_HOST"


/*******************************************************************/
typedef struct type_pending_request {
	char uuid[MAX_UUID_LEN + 1];		/* request unique id */
	HANDLE event;						/* synchronizing event */
	int done;							/* is request still alive? */
	int index;							/* index in global array */
	int taken;							/* seat is taken */
	ap_pool* pool;						/* resource pool */
	table *response;					/* HTTP response in name-value table format */
	void *rbody;						/* raw HTTP response body */
} pending_request;

/*******************************************************************/
typedef struct type_headers_data {
	int count;                          /* How many data entries in this flatmap */
	int len;                            /* The data length for this flatmap */
} headers_data;

/*******************************************************************/
static array_header *requests = NULL;           /* Array for pending requests */
static table *pending_request_table = NULL;     /* Hash table for Apache unique transaction ID to pending request mappings */
static headers_data headers_in_data;            /* HTTP request header data */   
static mutex *table_mutex = NULL;               /* Mutex object for pending_request_table */ 
static mutex *array_mutex = NULL;               /* Mutex object for requests array */
static mutex *headers_in_data_mutex = NULL;     /* Mutex object for HTTP request header data */
static int pending_request_counter = 0;         /* Pending request counter */

/*******************************************************************/
module MODULE_VAR_EXPORT metreos_http_module;   /* Module identifier used in Apache */

/*******************************************************************/
/* Utility function to identify if the incoming request is for Metreos Application Server.
   Since all requests using port 8000 have the Server Host Name as "metreos*.  This function
   is going to help us catch them */
static int isMetreosRequest(const char* pServerHostName)
{
	if (strcmp(pServerHostName, DEFAULT_VHOST_SERVER_NAME) == 0)
		return 1;

	return 0;
}

/*******************************************************************/
/* Utility function to find available slot in pending requests array */ 
static int findAvailableArrayElement()
{
	int i;
	pending_request *list = NULL;
	
	ap_acquire_mutex(array_mutex);

	list = (pending_request*)requests->elts;

	for (i=0; i<requests->nelts; i++)
	{
		if (!list[i].taken)
		{
			list[i].taken = 1;
			ap_release_mutex(array_mutex);
			return i;
		}
	}

	/* no free space or no element */
	ap_release_mutex(array_mutex);
	return -1;
}

/*******************************************************************/
/* Add a HTTP request into pending request array */
static int addRequest(const char* pszUuid, const int id, ap_pool* subpool)
{
	pending_request *list = NULL;
	pending_request *request = NULL;
	array_header* hdrs_arr = NULL;
	table_entry* hdrs = NULL;
	char* pszNum;
	int index = -1;

	ap_acquire_mutex(table_mutex);
	ap_acquire_mutex(array_mutex);

    /* check if there is too many pending request */
    if (pending_request_counter >= MAX_PENDING_REQUESTS)
    {
		fprintf(stderr,"METREOS - [ERROR] - Too many pending request (> 1000)!\n");
		fflush(stderr);

		ap_release_mutex(array_mutex);
		ap_release_mutex(table_mutex);
        return 0;
    }

    /* Make sure there is no duplicate pending requests */
	if (ap_table_get(pending_request_table, pszUuid))
	{
		ap_release_mutex(array_mutex);
		ap_release_mutex(table_mutex);

		fprintf(stderr,"METREOS - [ERROR] - Duplicate UUID, rejest this request!\n");
		fflush(stderr);

		return 0;
	}

	if (id < 0)
	{
        /* No entry for us to reuse, push a new one */
		request = (pending_request*)ap_push_array(requests);
		strcpy(request->uuid, pszUuid);
		request->taken = 1;
		request->done = 0;
		request->pool = subpool;
		request->event = CreateEvent(NULL, FALSE, FALSE, request->uuid);
		index = requests->nelts - 1;		
		request->index = index;
	}
	else
	{
        /* There is an empty slot, let's reuse it */
		index = id;
		list = (pending_request*)requests->elts;
		list[index].taken = 1;
		list[index].index = index;
		strcpy(list[index].uuid, pszUuid);
		list[index].done = 0;
		list[index].pool = subpool;
		list[index].event = CreateEvent(NULL, FALSE, FALSE, list[index].uuid);
	}

    pszNum = malloc(8);
	memset(pszNum, 0, sizeof(pszNum));
	itoa(index, pszNum, 10);
	ap_table_setn(pending_request_table, pszUuid, pszNum);
    pending_request_counter++;

	//fprintf(stderr,"METREOS - [INFO] - Added request, UUID=%s, Index=%s.\n", pszUuid, szNum);
	//fflush(stderr);
	
	ap_release_mutex(array_mutex);
	ap_release_mutex(table_mutex);

	return 1;
}

/*******************************************************************/
/* Remove pending request entry from requests array */
static int removeRequest(const char* pszUuid, int index)
{
	pending_request *list = NULL;
    char* pszNum = NULL;

	//fprintf(stderr,"METREOS - [INFO] - Remove request, UUID=%s, Index = %d.\n", pszUuid, index);
	//fflush(stderr);

	ap_acquire_mutex(table_mutex);

	ap_acquire_mutex(array_mutex);

	list = (pending_request*)requests->elts;

	if (strcmp(pszUuid, list[index].uuid) == 0)
	{
		list[index].taken = 0;
		list[index].done = 1;
        if (list[index].event)
		    CloseHandle(list[index].event);
		list[index].event = NULL;
        if (list[index].response)
            ap_clear_table(list[index].response);
        list[index].response = NULL;
        if (list[index].pool)
            ap_clear_pool(list[index].pool);
        pszNum = ap_table_get(pending_request_table, pszUuid);
        if (pszNum)
            free(pszNum);
		ap_table_unset(pending_request_table, pszUuid);
        pending_request_counter--;
	}

	ap_release_mutex(array_mutex);
	ap_release_mutex(table_mutex);


	return 1;
}

/*******************************************************************/
/* Notify Apache HTTP handler the response data has been sent back from HTTP Provider */ 
static int notifyResponse(const int id)
{
	pending_request *list = NULL;

	/* make sure to acquire array mutex before enter this fucntion */
	if (id < 0)
	{
		fprintf(stderr,"METREOS - [ERROR] - Invalid notify respopnse id.\n");
		fflush(stderr);

		return 0;
	}

	list = (pending_request*)requests->elts;

	if (list[id].event)
	{
		SetEvent(list[id].event);
	}

	return 1;
}

/*******************************************************************/
/* Lookup pending request in request array by UUID string */
static pending_request* findPendingRequestByUUID(const char* uuid)
{
	int index = -1;
	pending_request *list = NULL;
	pending_request *pr = NULL;
	array_header* hdrs_arr = NULL;
	table_entry* hdrs = NULL;

	/* Make sure to gain access to shared table and array before doing anything */
	if (ap_table_get(pending_request_table, uuid))
	{
		index = atoi(ap_table_get(pending_request_table, uuid));
	}
	else
	{
		fprintf(stderr,"METREOS - [ERROR] - unknown UUID=%s.\n", uuid);
		fflush(stderr);
	}

	list = (pending_request*)requests->elts;

	if (index >= 0)
		pr = &list[index];

	return pr;
}

/*******************************************************************/
/* The callback function to notify Apache module about new flatmap data */
int flatmapMessageNotifier(const flatmap_data flatmap)
{
	pending_request* pr = NULL;
	char szUuid[MAX_UUID_LEN + 1];
	int i, size;
	char* pValue = NULL;
	char* p = NULL;
	char szNum[16];
	flatmap_data_header header;

	ap_acquire_mutex(table_mutex);
	ap_acquire_mutex(array_mutex);

	memset(&szUuid, 0, sizeof(szUuid));
	strncpy(szUuid, flatmap.uuid, flatmap.uuid_len);

    /* Make sure this flatmap dta is for one of the pending requests */
	pr = findPendingRequestByUUID(szUuid);

	if (!pr || !pr->pool)
	{
		ap_release_mutex(array_mutex);
		ap_release_mutex(table_mutex);
		return 0;
	}

	/* fill in response table here... */
	pr->response = ap_make_table(pr->pool, flatmap.count);
	p = flatmap.data;
	size = 0;

	if (p)
	{
		for (i=0; i<flatmap.count; i++)
		{		
			memcpy(&header, p, sizeof(flatmap_data_header));
			p += sizeof(flatmap_data_header);
			size += sizeof(flatmap_data_header);

			/* assume all data is in string format */
			pValue = ap_pcalloc(pr->pool, header.data_size+1);
			memcpy(pValue, p, header.data_size);
			p += header.data_size;
			size += header.data_size;
			switch(header.http_data_type)
			{
				case HTTP_UUID:
					ap_table_add(pr->response, HTTP_UUID_NAME, pValue);
					break;

				case HTTP_URI:
					ap_table_add(pr->response, HTTP_URI_NAME, pValue);
					break;

				case HTTP_BODY:
					pr->rbody = ap_pcalloc(pr->pool, header.data_size+1);
					memcpy(pr->rbody, pValue, header.data_size);
					memset(&szNum, 0, sizeof(szNum));
					itoa(header.data_size, szNum, 10);
					ap_table_add(pr->response, HTTP_CONTENT_LENGTH_NAME, szNum);
					break;

				case HTTP_CONTENT_TYPE:
					ap_table_add(pr->response, HTTP_CONTENT_TYPE_NAME, pValue);
					break;

				case HTTP_USERNAME:
					ap_table_add(pr->response, HTTP_USERNAME_NAME, pValue);
					break;

				case HTTP_PASSWORD:
					ap_table_add(pr->response, HTTP_PASSWORD_NAME, pValue);
					break;

				case HTTP_RESPONSE_CODE:
					ap_table_add(pr->response, HTTP_RESPONSE_CODE_NAME, pValue);
					break;

				case HTTP_RESPONSE_PHRASE:
					ap_table_add(pr->response, HTTP_RESPONSE_PHRASE_NAME, pValue);
					break;

				case HTTP_HEADER:
					ap_table_add(pr->response, HTTP_HEADER_NAME, pValue);
					break;

				case HTTP_COOKIE:
					ap_table_add(pr->response, HTTP_COOKIE_NAME, pValue);
					break;
			}
		}
	}
	else
	{
		fprintf(stderr,"METREOS - [ERROR] - Invalid response data pointer.\n");
		fflush(stderr);
	}

    /* Signal the waiting thread to get the data */ 
	notifyResponse(pr->index);

	ap_release_mutex(array_mutex);
	ap_release_mutex(table_mutex);

	return 1;
}

/*******************************************************************/
/* filename-to-URI translation handler for Apache module */
static int metreos_http_translate(request_rec *r)
{
	if (!isMetreosRequest(r->server->server_hostname))
		return DECLINED;

	return OK;
}

/*******************************************************************/
/* Apache module initializer */
static void metreos_http_init(server_rec *s, pool *p)
{
    /* noop */
}

/*******************************************************************/
/* Apache module request fixup handler */
static int metreos_http_fixup(request_rec *r)
{
    /* noop */
	return DECLINED;
}

/*******************************************************************/
/* Apache module child process initializer */
static void metreos_http_child_init(server_rec *s, pool *p)
{
	pending_request *list = NULL;
	int i;

    if (!table_mutex)
        table_mutex = ap_create_mutex(NULL);

	if (!array_mutex)
		array_mutex = ap_create_mutex(NULL);

	if (!headers_in_data_mutex)
		headers_in_data_mutex = ap_create_mutex(NULL);

	if (!pending_request_table)
		pending_request_table = ap_make_table(p, MAX_PENDING_REQUESTS);

    /* Create pending requests array and init each entry */
	if (!requests)
	{
		requests = ap_make_array(p, MAX_PENDING_REQUESTS, sizeof(pending_request));	
		list = (pending_request*)requests->elts;
		for (i=0; i<requests->nelts; i++)
		{
			list[i].taken = 0;
			list[i].done = 1;
			list[i].event = NULL;
			list[i].pool = NULL;
			list[i].rbody = NULL;
		}
	}

    /* Create IPC client and connect to server */ 
	createFlatmapIpcClient();
	connectToFlatmapIpcServer(DEFAULT_FLATMAP_SERVER_ADDRESS, DEFAULT_FLATMAP_PORT);
    /* Assign callback function for flatmap IPC */
	assignMessageNotifier(flatmapMessageNotifier);
}

/*******************************************************************/
/* Apache module child process exit/cleanup handler */
static void metreos_http_child_exit(server_rec *s, pool *p)
{
    /* Clean house and disconnect from IPC server */
	disconnectFromFlatmapIpcServer();
	destroyFlatmapIpcClient();
}

/*******************************************************************/
/* Apache module HTTP handler, this is the place to fill in the response structure */
/* At this point, all the flatmap data entries have been retrieved and place in pending request's table */ 
static int metreos_http_handler(request_rec *r)
{
	pending_request* pr;
	array_header* hdrs_arr = NULL;
	table_entry* hdrs = NULL;
	int rstat, i, notified, retCode = 200, index, len = 0;
	char *key = NULL, *val = NULL;
	HANDLE event = NULL;
	char uuid[MAX_UUID_LEN + 1];		/* request unique id */
    char szStatusCode[8];               /* status code */

	if (!isMetreosRequest(r->server->server_hostname))
		return DECLINED;

	ap_acquire_mutex(table_mutex);
	ap_acquire_mutex(array_mutex);

    /* Make sure we can find the request in pending requests array */
	pr = findPendingRequestByUUID(ap_table_get(r->subprocess_env, METREOS_UNIQUE_ID));

	if (pr == NULL || !pr->event || pr->done)
	{
        /* We cannot find the request, ignore it */
		ap_release_mutex(table_mutex);
		ap_release_mutex(array_mutex);
		return DECLINED;
	}
	else
	{
        /* We found the one, get the UUID and event */
		event = pr->event;
		index = pr->index;
		memset(&uuid, 0, sizeof(uuid));
		strcpy(uuid, pr->uuid);
	}

	ap_release_mutex(table_mutex);
	ap_release_mutex(array_mutex);

	notified = 0;
    /* Now, we are waiting for HTTP provider to return response data */
	if (WAIT_OBJECT_0 == WaitForSingleObject(event, DEFAULT_HANDLER_TIMEOUT*1000))
	{
        /* This is not timeout or abandon, the request actually being processed by Application Server */
		notified = 1;
	}

    /* App server side has trouble processing the request, App server log should tell you what's wrong with it. */
    /* For example, the script is not there */
	if (!notified)
	{
		fprintf(stderr,"METREOS - [WARN] - Timeout or Abandoned, UUID=%s.\n", pr->uuid);
		fflush(stderr);
		removeRequest(uuid, index);
		return DECLINED;		
	}

	ap_acquire_mutex(table_mutex);
	ap_acquire_mutex(array_mutex);

    /* To be safe, let's do another lookup and make sure the request is still in pending requests array */
	pr = findPendingRequestByUUID(ap_table_get(r->subprocess_env, METREOS_UNIQUE_ID));
	if (pr == NULL || !pr->event || pr->done)
	{
		ap_release_mutex(table_mutex);
		ap_release_mutex(array_mutex);
		removeRequest(uuid, index);
		return DECLINED;
	}

    /* Retrieve response code, we only care about 200 */ 
	if (ap_table_get(pr->response, HTTP_RESPONSE_CODE_NAME))
		retCode = atoi(ap_table_get(pr->response, HTTP_RESPONSE_CODE_NAME));

    if (retCode == 404)
	{
        /* It fails anyway, let Apache do the job */
		ap_release_mutex(table_mutex);
		ap_release_mutex(array_mutex);
		removeRequest(uuid, index);
		return DECLINED;
	}
    else if (retCode != 200)
    {
        /* not OK, modify the return status code and status line, the status line's return code must match status code */
        r->status = retCode;        
	    memset(szStatusCode, 0, sizeof(szStatusCode));
	    itoa(retCode, szStatusCode, 10);
        r->status_line = szStatusCode;
    }

    /* Fill in the content type */
    r->content_type = ap_pstrdup(r->pool, ap_table_get(pr->response, HTTP_CONTENT_TYPE_NAME));

	/* Populate response headers from response table which we fills in earlier in flatmapMessageNotifier */
	ap_acquire_mutex(r->headers_out);
    hdrs_arr = ap_table_elts(pr->response);
    hdrs = (table_entry *)hdrs_arr->elts;
    for (i = 0; i < hdrs_arr->nelts; i++) 
	{
		if (hdrs[i].key == NULL) 
		{
			continue;
        }

		if ((strcmp(hdrs[i].key, HTTP_HEADER_NAME) != 0) &&
			(strcmp(hdrs[i].key, HTTP_COOKIE_NAME) != 0))
		{
			continue;
		}

		key = ap_getword(r->pool, &hdrs[i].val, ':');
		val = ap_getword(r->pool, &hdrs[i].val, NULL);

		if (strcmpi(key, HTTP_CONTENT_TYPE_NAME) == 0)
		{
			r->content_type = ap_pstrdup(r->pool, val);
		}
		else if (strcmpi(key, HTTP_CONTENT_ENCODING_NAME) == 0)
		{
			r->content_encoding = ap_pstrdup(r->pool, val);
		}
		else if (strcmpi(key, HTTP_CONTENT_LANGUAGE_NAME) == 0)
		{
			r->content_language = ap_pstrdup(r->pool, val);
		}
		else
		{
			ap_table_add(r->headers_out, key, val);		
		}
    }

    /* Response body */ 
	if (pr->rbody)
	{
		if (len == 0)
            len = atoi(ap_table_get(pr->response, HTTP_CONTENT_LENGTH_NAME));
		ap_set_content_length(r, len);
	}

	ap_release_mutex(r->headers_out);

    /* Set a hard time out and send HTTP header section to client side */
    ap_hard_timeout("metreos_http send header", r);
    ap_send_http_header(r);
    ap_kill_timeout(r);

	if (ap_table_get(pr->response, HTTP_RESPONSE_CODE_NAME))
		retCode = atoi(ap_table_get(pr->response, HTTP_RESPONSE_CODE_NAME));

    if (r->header_only) 
	{
		ap_release_mutex(table_mutex);
		ap_release_mutex(array_mutex);
		removeRequest(uuid, index);
		return OK;
    }

    /*
     * We need to include this little bit around every occurrence
     * of our sending strings hard-coded in the source.  This makes sure
     * that they are sent as ASCII, instead of as the EBCDIC which
     * they were compiled as.  We need to turn it off again when sending
     * content that hasn't been* compiled.  
     */
#ifdef CHARSET_EBCDIC
    ap_bsetflag(r->connection->client, B_EBCDIC2ASCII, r->ebcdic.conv_out = 1);
#endif

	if (pr->rbody)
	{
		ap_hard_timeout("metreos_http send body", r);
		rstat = ap_rwrite(pr->rbody, len, r);
		ap_kill_timeout(r);
	}

	ap_release_mutex(table_mutex);
	ap_release_mutex(array_mutex);
	removeRequest(uuid, index);

	return OK;
}

/*******************************************************************/
/* Apache module post read request handler */
static int metreos_http_post_read_request(request_rec *r)
{
    /* Noop for now */
	return DECLINED;
}

/*******************************************************************/
/* Trace function for Apache request header table */
static int header_trace(void *r, const char *key, const char *value)
{
	int bGood = 1;

    /* Put all the good header information into table */
    /* the reason why we want to verify the key and value is because */
    /* Cisco IP phone and 7970 may send out requests which do not follow */
    /* RFCs for HTTP 1.0/1.1.  For example, no HOST entry of HOST entry is blank. */

	if (key == NULL || value == NULL)
		bGood = 0;
	else if (strlen(key) == 0 || strlen(value) == 0)
		bGood = 0;

	if (bGood)
	{
		headers_in_data.len += strlen(key);
		headers_in_data.len += strlen(value);
		headers_in_data.len += 2;

		headers_in_data.count++;
	}

	return 1;
}

/*******************************************************************/
/* Utility function to read request body */
static int read_body(request_rec *r, const char **rbuf)
{
	int rc;

    /* Force client to re-send body as a block */
	if ((rc = ap_setup_client_block(r, REQUEST_CHUNKED_ERROR)) != OK) 
	{
		return rc;
	}

    /* Read HTTP request body block */
    if (ap_should_client_block(r)) 
	{
		char argsbuffer[4096];
		int rsize, len_read, rpos=0;
		long length = r->remaining;
		*rbuf = ap_pcalloc(r->pool, length + 1);

        /* Set a hard time out so it won't give up in the middle of read */
		ap_hard_timeout("read_body", r);
		while ((len_read = ap_get_client_block(r, argsbuffer, sizeof(argsbuffer))) > 0) 
		{           
			ap_reset_timeout(r);
			if ((rpos + len_read) > length) 
			{
				rsize = length - rpos;
			}
			else 
			{
				rsize = len_read;
			}
			memcpy((char*)*rbuf + rpos, argsbuffer, rsize);
			rpos += rsize;
		}
		ap_kill_timeout(r);
	}
	return rc;
}

/*******************************************************************/
/* Generate unique transaction id for the request */
static int reset_uuid(request_rec *r)
{
	ap_table_setn(r->subprocess_env, METREOS_UNIQUE_ID, ap_table_get(r->subprocess_env, "UNIQUE_ID"));	
	return 1;
}

/*******************************************************************/
/* Apache request header parser, here is the place where we re-package from apache
   request data structure to flatmap then pass it to HTTP Provider through IPC */
static int metreos_http_header_parser(request_rec *r)
{
	int index, i, size = 0, count = 0, port, apache_uuid = 0;
	flatmap_data* flatmap = NULL;
	flatmap_data_header header;
	array_header* hdrs_arr = NULL;
	table_entry* hdrs = NULL;
	char* p = NULL;
	const char *data = NULL;
	char szPort[10];
    ap_pool* subpool = NULL;

    /* Is this request belongs to us? Which means request through port 8000.*/
	if (!isMetreosRequest(r->server->server_hostname))
		return DECLINED;

    /* Create an unique transaction to associate with this request, this is for book keeping purposes */
	apache_uuid = reset_uuid(r);

    /* Some useful debuging code to print out the content in Apache's request data structure */
	/*
	fprintf(stderr, "!!! Metreos HTTP request !!!\n");
	fprintf(stderr, "request_time=%s\n", asctime(localtime(&(r->request_time))));
	fprintf(stderr, "uuid=%s\n", ap_table_get(r->subprocess_env, METREOS_UNIQUE_ID));
	fprintf(stderr, "method=%s\n", r->method);
	fprintf(stderr, "protocol=%s\n", r->protocol);
	fprintf(stderr, "version number=%d (1001 is 1.1)\n", r->proto_num);
	fprintf(stderr, "unparsed uri=%s\n", r->unparsed_uri);
	fprintf(stderr, "uri=%s\n", r->uri);
	fprintf(stderr, "query parameters=%s\n", r->args);
	fprintf(stderr, "hostname=%s\n", r->hostname);
	fprintf(stderr, "remote host=%s\n", ap_get_remote_host(r->connection, r->per_dir_config, REMOTE_NAME));
	fprintf(stderr, "server port=%d\n", ap_get_server_port(r));
	fprintf(stderr, "bytes_sent=%d\n", r->bytes_sent);
	fprintf(stderr, "header only (No body)=%d\n", r->header_only);
	ap_table_do(header_trace, r, r->headers_in, NULL);
	fprintf(stderr, "content type=%s\n", r->content_type);
	fprintf(stderr, "content language=%s\n", r->content_language);
	fprintf(stderr, "content encoding=%s\n", r->content_encoding);
	fflush(stderr);
	*/

	/* Prepare flatmap data place holder by allocating the memory from Apache's request resource pool */
    /* Since the resource is attached to the request, Apache's resource manager release them while the life cycle */
    /* of the request ends, which reduce the headache of keeping track of memoey alloaction */
	flatmap = (flatmap_data*)ap_pcalloc(r->pool, sizeof(flatmap_data));
	flatmap->count = 0;
	strcpy(flatmap->uuid, ap_table_get(r->subprocess_env, METREOS_UNIQUE_ID));
	flatmap->uuid_len = strlen(flatmap->uuid)+1;

    /* Go through each data entry which we want to pass to HTTP provider and figure out how much memory we need to allocate for this flatmap */
	/* Apache generated uuid? */
	size += 2;
	count++;

	/* method */
	size += (strlen(r->method)+1);
	count++;

	/* protocol */
	size += (strlen(r->protocol)+1);
	count++;

	/* uri */
	size += (strlen(r->uri)+1);
	count++;

	/* query parameters */
	if (r->args)
	{
		size += (strlen(r->args)+1);
		count++;
	}

	/* header only */
	size += 2;
	count++;

	/* header */
	ap_acquire_mutex(headers_in_data_mutex);
	headers_in_data.count = 0;
	headers_in_data.len = 0;
	ap_table_do(header_trace, r, r->headers_in, NULL);
	size += headers_in_data.len;
	count += headers_in_data.count;
	ap_release_mutex(headers_in_data_mutex);

	/* body */
	if (!r->header_only)
	{
		if (OK == read_body(r, &data))
		{
			if (data != NULL)
			{
				count++;
				size += (strlen(data)+1);
			}
		}
	}

	/* content type */
	if (r->content_type)
	{
		size += (strlen(r->content_type)+1);
		count++;
	}

	port = ap_get_server_port(r);
	memset(&szPort, 0, sizeof(szPort));
	itoa(port, szPort, 10);

	/* host */
	if (r->hostname  && strlen(r->hostname) > 0)
	{
		size += (strlen(r->hostname)+1);
		size += (strlen(szPort)+1);
		count++;
	}
	else
	{
		size += (strlen(r->connection->local_ip)+1);
		size += (strlen(szPort)+1);
		count++;
	}

	/* remote host */
	if (ap_get_remote_host(r->connection, r->per_dir_config, REMOTE_NAME))
	{
		size += (strlen(ap_get_remote_host(r->connection, r->per_dir_config, REMOTE_NAME))+1);
		size += (strlen(szPort)+1);
		count++;
	}

	size += (count *sizeof(flatmap_data_header));

	/* allocate space for flatmap data */
	flatmap->data = (char*)ap_pcalloc(r->pool, size);
	p = flatmap->data;

	flatmap->count = count;

	/* fill in each data entry */
	/* method */
	header.data_size = strlen(r->method)+1;
	header.flatmap_data_type = FLATMAP_STRING;
	header.http_data_type = HTTP_METHOD;
	memcpy(p, &header, sizeof(flatmap_data_header));
	p += sizeof(flatmap_data_header);
	memcpy(p, r->method, header.data_size);
	p += header.data_size;

	/* protocol */
	header.data_size = strlen(r->protocol)+1;
	header.flatmap_data_type = FLATMAP_STRING;
	header.http_data_type = HTTP_VERSION;
	memcpy(p, &header, sizeof(flatmap_data_header));
	p += sizeof(flatmap_data_header);
	memcpy(p, r->protocol, header.data_size);
	p += header.data_size;

	/* uri */
	header.data_size = strlen(r->uri)+1;
	header.flatmap_data_type = FLATMAP_STRING;
	header.http_data_type = HTTP_URI;
	memcpy(p, &header, sizeof(flatmap_data_header));
	p += sizeof(flatmap_data_header);
	memcpy(p, r->uri, header.data_size);
	p += header.data_size;

	/* query parameters */
	if (r->args)
	{
		header.data_size = strlen(r->args)+1;
		header.flatmap_data_type = FLATMAP_STRING;
		header.http_data_type = HTTP_QUERY_PARAMETERS;
		memcpy(p, &header, sizeof(flatmap_data_header));
		p += sizeof(flatmap_data_header);
		memcpy(p, r->args, header.data_size);
		p += header.data_size;
	}

	/* header only */
	header.data_size = 2;
	header.flatmap_data_type = FLATMAP_STRING;
	header.http_data_type = HTTP_HAS_BODY;
	memcpy(p, &header, sizeof(flatmap_data_header));
	p += sizeof(flatmap_data_header);
	if (r->header_only)
		memcpy(p, "0", header.data_size);
	else
		memcpy(p, "1", header.data_size);
	p += header.data_size;

	/* header */
    hdrs_arr = ap_table_elts(r->headers_in);
    hdrs = (table_entry *)hdrs_arr->elts;
    for (i = 0; i < hdrs_arr->nelts; i++) 
	{
		if (hdrs[i].key == NULL || hdrs[i].val == NULL) 
		{
			continue;
        }
		else if (strlen(hdrs[i].key) == 0 || strlen(hdrs[i].val) == 0)
		{
			continue;
		}

		header.data_size = strlen(hdrs[i].key) + strlen(hdrs[i].val) + 2;
		header.flatmap_data_type = FLATMAP_STRING;
		header.http_data_type = HTTP_HEADER;
		memcpy(p, &header, sizeof(flatmap_data_header));
		p += sizeof(flatmap_data_header);
		memcpy(p, ap_psprintf(r->pool, "%s:%s", hdrs[i].key, hdrs[i].val), header.data_size);
		p += header.data_size;
    }

	/* body */
	if (!r->header_only && data != NULL)
	{
		header.data_size = strlen(data) + 1;
		header.flatmap_data_type = FLATMAP_STRING;
		header.http_data_type = HTTP_BODY;
		memcpy(p, &header, sizeof(flatmap_data_header));
		p += sizeof(flatmap_data_header);
		memcpy(p, data, header.data_size);
		p += header.data_size;
	}

	/* content type */
	if (r->content_type)
	{
		header.data_size = strlen(r->content_type) + 1;
		header.flatmap_data_type = FLATMAP_STRING;
		header.http_data_type = HTTP_CONTENT_TYPE;
		memcpy(p, &header, sizeof(flatmap_data_header));
		p += sizeof(flatmap_data_header);
		memcpy(p, r->content_type, header.data_size);
		p += header.data_size;
	}

	/* host */
	if (r->hostname && strlen(r->hostname) > 0)
	{
		header.data_size = strlen(r->hostname) + 1 + strlen(szPort) + 1;
		header.flatmap_data_type = FLATMAP_STRING;
		header.http_data_type = HTTP_HOST;
		memcpy(p, &header, sizeof(flatmap_data_header));
		p += sizeof(flatmap_data_header);
		memcpy(p, ap_psprintf(r->pool, "%s:%s", r->hostname, szPort), header.data_size);
		p += header.data_size;
	}
	else
	{
		header.data_size = strlen(r->connection->local_ip) + 1 + strlen(szPort) + 1;
		header.flatmap_data_type = FLATMAP_STRING;
		header.http_data_type = HTTP_HOST;
		memcpy(p, &header, sizeof(flatmap_data_header));
		p += sizeof(flatmap_data_header);
		memcpy(p, ap_psprintf(r->pool, "%s:%s", r->connection->local_ip, szPort), header.data_size);
		p += header.data_size;
	}

	/* remote host */
	if (ap_get_remote_host(r->connection, r->per_dir_config, REMOTE_NAME))
	{
		header.data_size = strlen(ap_get_remote_host(r->connection, r->per_dir_config, REMOTE_NAME)) + 1 + strlen(szPort) + 1;
		header.flatmap_data_type = FLATMAP_STRING;
		header.http_data_type = HTTP_REMOTE_HOST;
		memcpy(p, &header, sizeof(flatmap_data_header));
		p += sizeof(flatmap_data_header);
		memcpy(p, ap_psprintf(r->pool, "%s:%s", ap_get_remote_host(r->connection, r->per_dir_config, REMOTE_NAME), szPort), header.data_size);
		p += header.data_size;
	}

	/* apache generated uuid? */
	header.data_size = 2;
	header.flatmap_data_type = FLATMAP_STRING;
	header.http_data_type = HTTP_APACHE_UUID;
	memcpy(p, &header, sizeof(flatmap_data_header));
	p += sizeof(flatmap_data_header);
	if (apache_uuid)
		memcpy(p, "1", header.data_size);
	else
		memcpy(p, "0", header.data_size);
	p += header.data_size;

	/* Put this request into pending requests array and create an event for this request if sent successfully */
	index = findAvailableArrayElement();

    subpool = ap_make_sub_pool(r->pool);
	if (!addRequest(ap_table_get(r->subprocess_env, METREOS_UNIQUE_ID), index, subpool))
	{
        /* Cannot add into pending array, bypass it and let Apache handle it */
        ap_clear_pool(subpool);
		return DECLINED;
	}

	else
	{
        /* Send flatmap to HTTP Provider on Application Server side */
		if (!sendReguestMessage(flatmap))
		{
			fprintf(stderr, "METREOS - [ERROR] - Failed to send request to IPC Client, UUID=%s.\n", flatmap->uuid);
			fflush(stderr);

			removeRequest(ap_table_get(r->subprocess_env, METREOS_UNIQUE_ID), index);
			return DECLINED;
		}
	}
	return OK;
}

/*******************************************************************/
/* Define list of HTTP handlers, we only use one and tie it every requests */
static const handler_rec metreos_http_handlers[] =
{
    {"*/*", metreos_http_handler},
    {NULL}
};

/*******************************************************************/
/* Standard Hooks to Apache module manager */
module MODULE_VAR_EXPORT metreos_http_module =
{
    STANDARD_MODULE_STUFF,
    metreos_http_init,					/* module initializer */
    NULL,								/* per-directory config creator */
    NULL,								/* dir config merger */
    NULL,								/* server config creator */
    NULL,								/* server config merger */
    NULL,								/* command table */
    metreos_http_handlers,				/* [7] list of handlers */
    metreos_http_translate,				/* [2] filename-to-URI translation */
    NULL,								/* [5] check/validate user_id */
    NULL,								/* [6] check user_id is valid *here* */
    NULL,								/* [4] check access by host address */
    NULL,								/* [7] MIME type checker/setter */
    NULL,								/* [8] fixups */
    NULL,								/* [10] logger */
    metreos_http_header_parser,			/* [3] header parser */
    metreos_http_child_init,			/* process initializer */
    metreos_http_child_exit,			/* process exit/cleanup */
    NULL								/* [1] post read_request handling */
};
