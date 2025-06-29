import win32pdhutil
import win32api
import win32con
import time
import sys
import os
import datetime

# Whether to wait to begin the loop to continually restart the MMS
bPhasedApproach = 'true'

sMediaServer = 'mmsserver'
bLoopForever = 'true'

# Number of minutes to wait between restarts of the Media Server
iNumMinutes = 20

# If a phased approach, the number of minutes to wait before starting the kill MMS loop
iNumPhasedMins = 10

def killProcess(procname):
    try:
        win32pdhutil.GetPerformanceAttributes('Process','ID Process',procname)
    except:
        pass

    pids = win32pdhutil.FindPerformanceAttributesByName(procname)
    try:
        pids.remove(win32api.GetCurrentProcessId())
    except ValueError:
        pass
    
    if len(pids) == 0:
        result = "Can't find %s" % procname
    elif len(pids)>1:
        result = "Found too many %s's - pids=`%s`" % (procname, pids)
    else:
        handle = win32api.OpenProcess(win32con.PROCESS_TERMINATE, 0, pids[0])
        win32api.TerminateProcess(handle, 0)
        win32api.CloseHandle(handle)
        result = ""
    
    return result

if __name__=='__main__':

    print "killProcess1.py started...\n"
    
    iNumIterations = 0
    
#    if len(sys.argv) > 2:
#        print "Usage: %s

    if bPhasedApproach == 'true':
        print "Using phased approach. %i Minutes to start of killForever loop\n" % 60*iNumPhasedMins
        time.sleep(60*iNumPhasedMins)
        
    while bLoopForever == 'true':
        iNumIterations += 1

        # Open log file
        f = open('C:\Metreos\Logs\killProcess.txt','a+')
        
        # Sleep for iNumNinutes minutes
        print "Entered killForever loop. %i Minutes until MMS restart\n" % 60*iNumMinutes
        time.sleep(60*iNumMinutes)

        # Kill MMS process
        rc = killProcess(sMediaServer)

        # Write the number of iterations of this loop, the date, and the time to the log file
        t = datetime.datetime.now()
        EpochSeconds = time.mktime(t.timetuple())
        now = datetime.datetime.fromtimestamp(EpochSeconds)
        prettynow = now.ctime()
        printstring = "Media Server Death #%i Time: %s\n" % (iNumIterations, prettynow)       
        print printstring
        f.write(printstring)
        f.close()
                
#        if iNumIterations > 3:     
#          bLoopForever = 'false'
    
    
    
