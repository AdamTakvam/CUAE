example usage:
    arsetup1.exe ccmIp ccmUser ccmPass StartMAC DeviceCount AccountPrefix MaxAXLWrite MCE [MCE...]
     
	arsetup1.exe 10.1.14.25 Administrator metreos SEP006060606000 50 777 60 127.0.0.1
	
	Remember, if you have '50' for device count, you will only generate 25 users in appsuiteadmin

	remember to change mysql user permissions on the root account of mysql to 
	accept not-local connections (I didn't make it configurable which mysql user is used to connect, ...
	you've got the code; patch it)
	
	The idea is that you will set a 'prefix DN' on SimClient callers, 
	and that you match that prefix when running this tool.  
	
	If you want run a new test with a different range of users, 
	just run appsuiteadmin/install/initialize_db.bat for now...
	(althought you don't really have to if you don't want, I guess...)
	
	You must have x:\ and samoa-framework built to build this, and WSE2 (Microsoft 'extension' to .net web services)