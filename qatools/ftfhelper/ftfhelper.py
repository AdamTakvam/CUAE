import zipfile
import logging
import os
import sys
import threading
import subprocess
import time
import xmlrpclib
import SimpleXMLRPCServer

def unzip(filename, targetdir='.'):
    if not targetdir.endswith(':') and not os.path.exists(targetdir):
        os.mkdir(targetdir)
    
    zf = zipfile.ZipFile(filename)
    for name in zf.namelist():
        if not name.endswith('/'):
            dirname = os.path.join(targetdir,os.path.dirname(name))
            if not os.path.exists(dirname):
                os.makedirs(dirname)
            logging.debug("Unzipping: %s",os.path.join(targetdir,name))  
            outfile = open(os.path.join(targetdir,name),'wb')
            outfile.write(zf.read(name))
            outfile.flush()
            outfile.close()

def execute(cmdline, cwd=None, env=None, outputLogger=None, stdin=None, stdinfile=None):
    logging.debug("Executing: " + " ".join(cmdline))
    if cwd:
        logging.debug("CWD: %s" % cwd)
    environ = None    
    if env:
        environ = os.environ
        environ.update(env)
        for x in env.keys():
            logging.debug("ENVIRONMENT: %s=%s" % (x, env[x]))

    if stdin or stdinfile:
        if stdinfile:
            stdin = open(stdinfile,'r').read()
        p = subprocess.Popen(cmdline, stdout=subprocess.PIPE, stderr=subprocess.STDOUT, stdin=subprocess.PIPE, cwd=cwd, env=environ)
        s = p.communicate(stdin)
    else:    
        p = subprocess.Popen(cmdline, stdout=subprocess.PIPE, stderr=subprocess.STDOUT, cwd=cwd, env=environ)
        s = p.communicate()
    
    if outputLogger:
        for l in s[0].split("\n"):
            outputLogger(l)

    logging.debug("RETVAL: %d" % -p.returncode)    
    
    return (-p.returncode, s[0])

class Execute(threading.Thread):
    def __init__(self, cmdList, cwd=None):
	threading.Thread.__init__(self)    
        self.cmdList = cmdList
        self.cwd     = cwd

    def run(self):
        self.running = 1
        try:
            execute(self.cmdList, cwd=self.cwd)
        except:
            pass
        self.running = 0    

class Updater(threading.Thread):
    
    def __init__(self, updFile, updateRoot=r'C:\updates', workingDir=r'C:\updates\working'):
        threading.Thread.__init__(self)
        self.updateRoot = updateRoot
        self.workingDir = workingDir
        self.updFile    = updFile
        self._status    = 'No Status'
    
    def _setStatus(self, msg):
        self._status = msg
	print msg
	  
    def _unzip(self):
        self._setStatus('Unzipping')
        
        p = os.path.join(self.updateRoot, self.updFile)
        if not os.path.exists(p):
            raise Exception, "No such update %s" % self.updFile
        try:
            os.rmdir(self.workingDir)
            os.mkdir(self.workingDir)
        except:
            pass
            
        unzip(p, self.workingDir)
        self._setStatus('Unzip Complete')
  
    def _updater(self):
        update_in_progress = os.path.join(self.workingDir,'update_in_progress')
        update_failed     = os.path.join(self.workingDir,'update_failed')
        update_success    = os.path.join(self.workingDir,'update_success')

        self._setStatus('Updating')
        t = Execute(['cmd','/c','install.bat'], cwd=self.workingDir)
        t.start()
        time.sleep(5)
        while t.running:
            if os.path.exists(update_in_progress):
                self._setStatus(open(update_in_progress,'r').read())
            elif os.path.exists(update_success):
                self._setStatus(open(update_success).read())
            elif os.path.exists(update_failed):
                self._setStatus(open(update_failed).read())
            else:
                self._setStatus('No Status')
            time.sleep(5)    
        self._setStatus('Update Complete')

    def run(self):
        try:
            self._unzip()
            self._updater()
        except:
	    self._setStatus('Update Failed')
            print '%s,%s' % (sys.exc_info()[0], sys.exc_info()[1])
            
class FTFHelper:
    def __init__(self):
        pass

    def runUpdate(self, updFile):
        self._updateThread = Updater(updFile)
        self._updateThread.start()
	return "Ok"

    def isUpdateComplete(self):
        try:
            if self._updateThread._status in ('Update Complete','Update Failed'):
                return True
            else:
                return False
        except:
            return True
  
    def updateStatus(self):
        try:
            return self._updateThread._status
        except:
            return "No Status"


def main():
    server = SimpleXMLRPCServer.SimpleXMLRPCServer(('10.1.12.190',8888))
    server.register_instance(FTFHelper())
    server.serve_forever()

if __name__=='__main__':
    main()

