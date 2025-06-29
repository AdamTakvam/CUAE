@echo off

mysql -u root -p -e "select a.username,b.mac_address,a.first_name,a.last_name FROM as_users a,as_phone_devices b where a.status=1 AND a.as_users_id=b.as_users_id;" -D application_suite > c:\h323users.csv