How to use the script:

1) Place the user-creator-csv.exe script anywhere on the Application Server.

2) Take a backup of your users in appsuiteadmin by going to appsuiteadmin > Settings & Records Backup > and saving the backup off box.

3) Using cmd,  invoke the user-creator-csv.exe.

A typical invocation would be:

> user-creator-csv.exe -users:c:\my_users.csv -u:Administrator -p:metreos

You can run user-creator-csv.exe with no arguments to have help print out.


4) Once the tool is done, it creates a results.txt file.  Errors, warnings, and logging is printed out in this file.   Please look over all errors and warnings!!


