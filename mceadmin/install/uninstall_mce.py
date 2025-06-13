#!C:\python24\python.exe
# 
# uninstall_mce.py
#
import os
import sys
import logging
import glob
sys.path.insert(0,r'C:\Metreos\Framework\1.0\python')
import mcelib
from mceservices import ServiceAttributes

mcelib.initLogging(os.path.join(mcelib.METREOSROOT,'Logs','unbootstrap.log'))

logging.info("Stop Services")
mcelib.serviceStop('Apache')
for service in ServiceAttributes.keys():
    mcelib.serviceStop(service)

logging.info("Unregister Services")
for service in ServiceAttributes.keys():
    mcelib.serviceUnregister(service, ServiceAttributes[service])

logging.info("Uninstall Assemblies")
assemblies     = glob.glob(os.path.join(mcelib.METREOSFRAMEWORK,'CoreAssemblies','*.dll'))
assembly_names = map(lambda x: os.path.splitext(os.path.basename(x))[0], assemblies)
mcelib.cleargac(*assembly_names)

logging.info("Drop database")
mcelib.execute([mcelib.MYSQLSHELL,'--user=root','--password=metreos','-e','drop database mce'])
mcelib.execute([mcelib.MYSQLSHELL,'--user=root','--password=metreos','-e','drop database application_suite'])

