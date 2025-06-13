#!C:\python24\python.exe
#
# install_mce.py
#
import sys
import shutil
import glob
import os
import logging
sys.path.insert(0,r'C:\Metreos\Framework\1.0\python')
import mcelib
from mceservices import ServiceAttributes

## MAIN

mcelib.initLogging(os.path.join(mcelib.METREOSROOT,'Logs','bootstrap.log'))
INSTALLROOT = os.path.join(mcelib.METREOSROOT,'mceadmin','install')

logging.info("Initialize MySQL")
mcelib.execute([mcelib.MYSQLSHELL,'--user=root','--password=metreos'],stdinfile=os.path.join(INSTALLROOT,'initial.sql'))
mcelib.execute([mcelib.MYSQLSHELL,'--user=root','--password=metreos'],stdinfile=os.path.join(mcelib.METREOSROOT,'appsuiteadmin','install','CreateDatabase.sql'))

logging.info("Initialize Apache/PHP")
shutil.copy2(os.path.join(INSTALLROOT,'httpd.conf'), os.path.join(mcelib.APACHEROOT, 'conf', 'httpd.conf'))
shutil.copy2(os.path.join(INSTALLROOT,'php.ini'),    os.path.join(mcelib.WINDOWSROOT,'php.ini'))
shutil.copy2(os.path.join(INSTALLROOT,'apache','favicon.ico'), os.path.join(mcelib.APACHEROOT,'htdocs','favicon.ico'))
mcelib.serviceRestart('Apache')

logging.info("Install Metreos Assemblies")
assemblies = glob.glob(os.path.join(mcelib.METREOSFRAMEWORK, 'CoreAssemblies', '*.dll'))
mcelib.initgac(*assemblies)

logging.info("Register Services")
for service in ServiceAttributes.keys():
    mcelib.serviceRegister(service, ServiceAttributes[service])

logging.info("Configure Media Server")
if not os.path.exists(os.path.join(mcelib.METREOSROOT,'Logs','MediaServer')):
    os.makedirs(os.path.join(mcelib.METREOSROOT,'Logs','MediaServer'))

shutil.copy2(os.path.join(INSTALLROOT,'mmsconfig.properties'), os.path.join(mcelib.METREOSROOT,'MediaServer','mmsconfig.properties'))
mcelib.registrySetValues(r'HKEY_LOCAL_MACHINE\SOFTWARE\Metreos\MediaServer', {
    'CommandLine' : (os.path.join(mcelib.METREOSROOT,'MediaServer','mmsserver.exe'),'string'),
    'ConfigPath'  : (os.path.join(mcelib.METREOSROOT,'MediaServer','mmsconfig.properties'),'string'),
})

logging.info("Refresh OpenSSH")
mcelib.opensshReload()

logging.info("Update Directory ACLs")
#mcelib.setAccess(mcelib.METREOSROOT,'Administrators')
#mcelib.grantAccess(mcelib.METREOSROOT,'SYSTEM')
dirs =  (['Logs',],['AppServer','Logs'],['AppServer','Deploy'],['MediaServer','Audio'],['MediaServer','Grammar'])
for d in map(lambda x: os.path.join(*[mcelib.METREOSROOT,]+x),dirs):
    try:
        os.makedirs(d)
    except:
        pass
    mcelib.grantAccess(d, 'SFTP Users')

logging.info("DONE.")
