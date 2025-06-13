To configure:

1) The application will call a number specified as a configuration item in mceadmin.  So, make sure the Call Route Group configured on the Default partition will be able to successfully make the call to the destination number.

2) The application will allow you to test up to 3 grammar files concurrently--to do so, navigate to the application's configuration in mceadmin.  For the application to work, at least one grammar file has to have been deployed to the Nuance server and configured as Grammar 1.  An example grammar is deployed with this example.  It is called foodlist.grxml.  You can use it to test the application.

3) The application, once invoked, will play a tone and then perform a voice rec operation based on the grammar(s) specified.  The application will attempt to send an IP Phone Execute command to the IP address of the endpoint called with the score and meaning of the voice operation.  A Cisco IP Phone Execute command requires that a user be associated with the phone in CallManager, in order for it to succeed.  The application has the username and password that it uses to send this command as configuration items (defaulting to 'metreos/metreos').  If you do not configure and end user with the phone you are testing with and the application accordingly, then the only way that you can tell the score and meaning is to watch the AppServer log--it's reported there as well.

