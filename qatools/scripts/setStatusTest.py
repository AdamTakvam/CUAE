import SOAPpy
import os
import getopt
import string

def run(ip):
    userRecord = {'firstName': None,
                  'lastName': None,
                  'username': None,
                  'password': None,
                  'pin': None,
                  'accountCode': None,
                  'email': None,
                  'status': None,
                  'lockoutThreshold': None,
                  'lockoutDuration': None,
                  'maxSessions': None,
                  'pinChange': None,
                  'record': None,
                  'recordVisible': None,
                  'timezone': None,
                  'arTransferNumber': None}
    
## Functions
    def initUserRecord(record):
        keys = record.keys()
        for k in keys:
            record[k] = None

    def logStatus(result, expected, testcase):
        if result == 1:
            status = 'PASS'
        if result == 0:
            status = 'FAIL'
        if result != 1 and result != 0:
            status = 'NONE'

        print 'Result:%s Expected:%s Testcase:%s' % (status, expected, testcase)

## Test for the setStatusTest AppsuiteAdmin WebServices API
    s = SOAPpy.WSDL.Proxy('http://%s/appsuiteadmin/soap.php?wsdl' % ip)
    
    token = s.login('Administrator','metreos')

    numTestCases = 1
    numUsers = 1

##  I need to change this script so that it does a cleanup after the script that
##  deletes only users that are created while running this script.  This is a very
##  destructive action.    
    try:
        test = s.deleteAllUsers(token)
    except:
        print 'Unhandled exception in deleteAllUsers.'
        pass

##  TESTCASE-AS-SOAP-WS-API-1001 - setStatus to Active when status is not equal to Active - PASS
    # Add a user to delete
    test = 0
    initUserRecord(userRecord)
    expected = 'PASS'
    testcase = 'TESTCASE-AS-SOAP-WS-API-1001 - setStatus to Active when status is not equal to Active' 
    userRecord['firstName'] = 'Test%s' % numTestCases
    userRecord['lastName'] = 'User%s' % numTestCases
    userRecord['username'] = 'tuser%s' % numTestCases
    userRecord['password'] = 'metreos'
    userRecord['pin'] = numTestCases
    userRecord['accountCode'] = numTestCases
    userRecord['status'] = 'Locked'

    try:
        s.addUser(token, userRecord)
        s.setStatus(token, userRecord['username'], 'Active')
        test = 'Active' == s.getStatus(token, userRecord['username'], 'Active')
        if test:
            test = 1
        else:
            test = 0
    except SOAPpy.Types.faultType, err:
        print '\nFAILED: %s. Unexpected exception %s' % (testcase, err.faultstring)
        pass
          
    # Log test results
    logStatus(test, expected, testcase)

    numTestCases += 1    
    
##  TESTCASE-AS-SOAP-WS-API-1002 - setStatus to Active when status is equal to Active - PASS
    # use account from 1001, setStatus to Active, and retrieve Status
    test = 0
    expected = 'PASS'
    testcase = 'TESTCASE-AS-SOAP-WS-API-1002 - setStatus to Active when status is equal to Active'   
   
    try:
        test = \
            s.setStatus(token, userRecord['username'], 'Active') and \
            'Active' == s.getStatus(token, userRecord['username'], 'Active')
    except SOAPpy.Types.faultType, err:
        print '\nFAILED: %s. Unexpected exception %s' % (testcase, err.faultstring)
        pass
          
    # Log test results
    logStatus(test, expected, testcase)

    numTestCases += 1    
    
##  TESTCASE-AS-SOAP-WS-API-1003 - setStatus to Locked when status is not equal to Locked - PASS
    # use account from 1001, setStatus to Locked, and retrieve Status
    test = 0
    expected = 'PASS'
    testcase = 'TESTCASE-AS-SOAP-WS-API-1003 - setStatus to Locked when status is not equal to Locked'   
    
    try:
        test = \
            s.setStatus(token, userRecord['username'], 'Locked') and \
            'Locked' == s.getStatus(token, userRecord['username'], 'Locked')
    except SOAPpy.Types.faultType, err:
        print '\nFAILED: %s. Unexpected exception %s' % (testcase, err.faultstring)
        pass
          
    # Log test results
    logStatus(test, expected, testcase)

    numTestCases += 1

##  TESTCASE-AS-SOAP-WS-API-1004 - setStatus to Locked when status is equal to Locked - PASS
    # use account from 1001, setStatus to Locked, and retrieve Status
    test = 0
    expected = 'PASS'
    testcase = 'TESTCASE-AS-SOAP-WS-API-1004 - setStatus to Locked when status is equal to Locked'
    
    try:
        test = \
            s.setStatus(token, userRecord['username'], 'Locked') and \
            'Locked' == s.getStatus(token, userRecord['username'], 'Locked')
    except SOAPpy.Types.faultType, err:
        print '\nFAILED: %s. Unexpected exception %s' % (testcase, err.faultstring)
        pass
          
    # Log test results
    logStatus(test, expected, testcase)
    
    numTestCases += 1
        
##  TESTCASE-AS-SOAP-WS-API-1005 - setStatus to Disabled when status is not equal to Disabled - PASS
    # use account from 1001, setStatus to Disabled, and retrieve Status
    test = 0
    expected = 'PASS'
    testcase = 'TESTCASE-AS-SOAP-WS-API-1005 - setStatus to Disabled when status is not equal to Disabled'
    
    try:
        test = \
            s.setStatus(token, userRecord['username'], 'Disabled') and \
            'Disabled' == s.getStatus(token, userRecord['username'], 'Disabled')
    except SOAPpy.Types.faultType, err:
        print '\nFAILED: %s. Unexpected exception %s' % (testcase, err.faultstring)
        pass
          
    # Log test results
    logStatus(test, expected, testcase)
    
    numTestCases += 1
    
##  TESTCASE-AS-SOAP-WS-API-1006 - setStatus to Disabled when status is equal to Disabled - PASS
    # use account from 1006, setStatus to Disabled, and retrieve Status
    test = 0
    expected = 'PASS'
    testcase = 'TESTCASE-AS-SOAP-WS-API-1006 - setStatus to Disabled when status is equal to Disabled'
    
    try:
        test = \
            s.setStatus(token, userRecord['username'], 'Disabled') and \
            'Disabled' == s.getStatus(token, userRecord['username'], 'Disabled')
    except SOAPpy.Types.faultType, err:
        print '\nFAILED: %s. Unexpected exception %s' % (testcase, err.faultstring)
        pass
          
    # Log test results
    logStatus(test, expected, testcase)
    
    numTestCases += 1

##  TESTCASE-AS-SOAP-WS-API-1007 - setStatus to Deleted when status is not equal to Deleted - PASS
    # use account from 1007, setStatus to Deleted, and retrieve Status
    test = 0
    expected = 'PASS'
    testcase = 'TESTCASE-AS-SOAP-WS-API-1007 - setStatus to Deleted when status is not equal to Deleted'
    
    # purposely have two try/excepts, so that if the first fails,
    # we know setStatus is messed up--and so that we can see getStatus
    # fail, as we expect it to
    try:
        s.setStatus(token, userRecord['username'], 'Deleted') 
        test = 1
    except SOAPpy.Types.faultType, err:
        print '\nFAILED: %s. Unexpected exception %s' % (testcase, err.faultstring)
        pass
        
    if test == 1:
        test = 0
        try:
            s.getStatus(token, userRecord['username'], 'Deleted')
            print 'FAILED: getStatus did not throw an SOAP exception as expected' 
        except SOAPpy.Types.faultType, err:
            test = 1
            pass
            
    # Log test results
    logStatus(test, expected, testcase)
    
    numTestCases += 1
    
##  TESTCASE-AS-SOAP-WS-API-1008 - setStatus to Deleted when status is equal to Deleted - FAIL
    # use account from 1001, setStatus to Deleted, and retrieve Status
    test = 1
    expected = 'FAIL'
    testcase = 'TESTCASE-AS-SOAP-WS-API-1008 - setStatus to Deleted when status is equal to Deleted'
    
    try:
        s.setStatus(token, userRecord['username'], 'Deleted')
        print 'FAILED: setStatus should throw a SOAP exception'
    except SOAPpy.Types.faultType, err:
        #print '\nFAILED: %s. Unexpected exception %s' % (testcase, err.faultstring)
        test = 0
        pass
          
    # Log test results
    logStatus(test, expected, testcase)
    
    numTestCases += 1
    
##  TESTCASE-AS-SOAP-WS-API-1009 - setStatus to Deleted when user does not exist - FAIL
    # make crazy username, and setStatus on it
    test = 1
    expected = 'FAIL'
    testcase = 'TESTCASE-AS-SOAP-WS-API-1009 - setStatus to Deleted when user does not exist'
    
    try:
        s.getStatus(token, 'yeahyeahyeah132NO', 'Deleted')
    except SOAPpy.Types.faultType, err:
        #print '\nFAILED: %s. Unexpected exception %s' % (testcase, err.faultstring)
        test = 0
        pass
          
    # Log test results
    logStatus(test, expected, testcase)
    
    numTestCases += 1
    
##  TESTCASE-AS-SOAP-WS-API-1010 - setStatus to NULL - FAIL
    # use account from 1001, and setStatus on it
    test = 1
    expected = 'FAIL'
    testcase = 'TESTCASE-AS-SOAP-WS-API-1010 - setStatus to NULL'
    
    try:
        s.setStatus(token, userRecord['username'], None)
    except SOAPpy.Types.faultType, err:
        #print '\nFAILED: %s. Unexpected exception %s' % (testcase, err.faultstring)
        test = 0
        pass
          
    # Log test results
    logStatus(test, expected, testcase)
    
    numTestCases += 1
    
##  TESTCASE-AS-SOAP-WS-API-1011 - setStatus to anything with invalid token - FAIL
    # use account from 1001, and setStatus on it
    test = 1
    expected = 'FAIL'
    testcase = 'TESTCASE-AS-SOAP-WS-API-1011 - setStatus to anything with invalid token'
    
    try:
        s.setStatus('yah', userRecord['username'],'Active')
    except SOAPpy.Types.faultType, err:
        #print '\nFAILED: %s. Unexpected exception %s' % (testcase, err.faultstring)
        test = 0
        pass
          
    # Log test results
    logStatus(test, expected, testcase)
    
    numTestCases += 1

    
def main(*argv): # expects foldername
    if argv is None:
        argv = sys.argv[1:]
    try:
        try:
            opts, args = getopt.getopt(argv, "h", ["help"])

            if len(args) == 0:
                ip = "127.0.0.1" # default to localhost
            else:
                ip = args[0]

            print "\nStarting setStatus test suite against %s\n" % args[0]
                            
            run(ip)

            print "\nTest done "
                        
        except getopt.error, msg:
             raise Usage(msg)
    except Usage, err:
        print >> sys.stderr, err.msg
        print >> sys.stderr, "for help use --help"
        return 2

class Usage(Exception):
    def __init__(self, msg):
        self.msg = msg
        
if __name__ == "__main__":
    sys.exit(main())