@echo off
echo reboot > rebooting
net stop Apache && net start Apache
del rebooting
exit 0