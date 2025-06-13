Inatall
-------
1) Install the SNMP service through Add/Remove Windows programs

2) Add a key to the end 
(eg. [HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\SNMP\Parameters\ExtensionAgents]
"10"="SOFTWARE\\Symbol\\MyAgent\\CurrentVersion")

3) Add another corresponding key
(eg. [HKEY_LOCAL_MACHINE\SOFTWARE\Symbol\MyAgent\CurrentVersion]
"Pathname"="G:\\VC\\SNMP\\MyAgent\\Debug\\MyAgent.dll")

4) Start the SNMP service 

5) Right click snmp Click Traps and add a community (eg. Public)

6) Add Trap destination (the ip name where the manager is running)

7) Under security tab make sure community rights to READ-WRITE

8) Under Log On tab make sure Profile is enabled