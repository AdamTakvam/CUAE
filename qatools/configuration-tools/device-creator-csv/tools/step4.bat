@echo off

mysql -u root -p -e "select username from as_users where ldap_synched=1;" -D application_suite > c:\users.csv