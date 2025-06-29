@echo off

mysql -u root -p -e "update as_users set password = 'lehman'" -D application_suite