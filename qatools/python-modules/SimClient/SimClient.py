# SimClient module
#
import Tkinter
import sys
import os
import string
import types
import time

def _tclEncode(x):
    if type(x) == types.StringType:
        return x
    elif type(x) in (types.IntType, types.LongType):
        return str(x)
    elif type(x) == types.NoneType:
        return '{}'
    elif type(x) in (types.TupleType, types.ListType):
        return '{' + " ".join(map(_tclEncode,x)) + '}'
    elif type(x) in (types.DictType,):
        a = []
        for k in x.keys():
            a.append(k)
            a.append(x[k])
        return _tclEncode(a)
    else:
        return '{}'

def _pyEncode(x):
    # TODO
    return x

class _Callback:
    def __init__(self, interp, function):
        self.interp   = interp
        self.function = function
    
    def __call__(self, *args, **kwargs):
        return apply(self.interp, (self.function,args, kwargs))

class _RetryCallback:
    def __init__(self, function, *args, **kwargs):
        self.function = function
        self.args = list(args)
        self.kwargs = kwargs

    def __call__(self, *args, **kwargs):
        self.args += list(args)
        self.kwargs.update(kwargs)
        return apply(self.function, self.args, self.kwargs)

class _XMLCallback:
    def __init__(self, interp, function):
        self.interp   = interp
        self.function = function
     
    def __call__(self, *args, **kwargs):
        return apply(self.interp, (self.function,), kwargs)

class _TclSCWrapper:
    def __init__(self):
        i = Tkinter.Tk(useTk=0)
        self.tcl = i.tk
        self.tcl.eval("load SCWrapper.dll SCWrapper")

    def __eval(self, cmd, args, kwargs):
        a = list(args)
        for k in kwargs.keys():
            a.append('-' + k)
            a.append(kwargs[k])
        command = cmd + " " + " ".join(map(_tclEncode,a))
        r = self.tcl.eval(command)
        return _pyEncode(r)
    
    def __getattr__(self, name):
        return _Callback(self.__eval, name)

class SimClient:
    def __init__(self, client):
        #_initLogging()
        self.sc       = _TclSCWrapper()
        self.clientIP = client
        self._connect()

    def __del__(self):
        try:
            self._disconnect()
        except:
            pass

    def _disconnect(self):
        try:
            self.sc.SCReleaseServerControl(self.serverID)
        except:
            pass
        time.sleep(1)    
        try:    
            self.sc.SCShutdownServer(self.launchID)
        except:
            pass
    
    def _connect(self):
        r = self.sc.SCLaunchServer(self.clientIP)
        (launchIdString, portNumberString) = r.split('\n')
        self.launchID   = string.strip(launchIdString.split(':')[1])
        self.portNumber = string.strip(portNumberString.split(':')[1])
        self.serverID = self._retryForTime(_RetryCallback(self.sc.SCGetServerControl, self.clientIP, self.portNumber), errorMessage="Could not get server control")
        try:
            self._retryForTime(_RetryCallback(self.sc.SCLaunchLoggingServer, self.clientIP))
        except:
            pass

    def _retryForTime(self, functionCall, timeout=15, errorMessage="Timeout on functionCall"):
        starttime = time.time()
        done = 0
        while not done:
            if starttime + timeout < time.time():
                raise Exception, errorMessage
            try:
                r = functionCall()
                done = 1
            except:
                time.sleep(1)
                pass
        return r
    
    def getConfigList(self):
        r = self.sc.SCGetConfigList(self.serverID)
        return r.split(' ')
        
    def _callXML(self,msgName,asyncFlag=0,**kwargs):
        msg = '<message><msgName>%s</msgName>' % msgName
        for x in kwargs.keys():
            msg += '<%s>%s</%s>'  % (x,kwargs[x],x)
        msg += '</message>'
        r = self.sc.SCLoadXML(self.serverID, msg, asyn=asyncFlag)
        # TODO parse XML
        return r
   
    def __getattr__(self, name):
        return _XMLCallback(self._callXML, name)
        #if name.split('_')[0] == 'xml':
        #    return _XMLCallback(self._callXML, name.split('_')[1])

if __name__=='__main__':
    s = SimClient('10.1.14.200')
    print s.xml_getConfigList()
    del s
    
