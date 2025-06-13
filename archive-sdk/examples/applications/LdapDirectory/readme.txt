A working LDAP server must be set up for this application. 

 --A sample LDAP server is provided with fake user data in contrib\openldap.zip.   This must be extracted in c:\
 --To run, execute c:\openldap\slapd -d 1 (alot of verbose garbage will print to the console: this is normal)

For the Cisco IP Phone-7970 to work with this example, contrib\directorysearch-main.png must be placed in web accessable location, and the path of this image must be configured in the web management front-end of the Application Server. 



To build, execute install.bat.
