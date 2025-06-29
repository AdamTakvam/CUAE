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

## Test for the addPassword AppsuiteAdmin WebServices API
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

##  TESTCASE-AS-SOAP-WS-API-0901 - setPassword. User exists - PASS
    # Add a user, which we will then reset the password of
    test = 0
    initUserRecord(userRecord)
    expected = 'PASS'
    testcase = 'TESTCASE-AS-SOAP-WS-API-0901 - setPassword. User exists' 
    userRecord['firstName'] = 'Test%s' % numTestCases
    userRecord['lastName'] = 'User%s' % numTestCases
    userRecord['username'] = 'tuser%s' % numTestCases
    userRecord['password'] = 'metreos'
    userRecord['pin'] = numTestCases
    userRecord['accountCode'] = numTestCases
    userRecord['status'] = 'Active'

    try:
        s.addUser(token, userRecord)
        s.setPassword(token, userRecord['username'], 'stepbystep')
        password = s.getPassword(token, userRecord['username'])[0]
        test = 'stepbystep' == password 
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
    
##  TESTCASE-AS-SOAP-WS-API-0902 - setPassword. User does not exist - FAIL
    # use account from 0901, setPassword on bogus username
    test = 1
    expected = 'FAIL'
    testcase = 'TESTCASE-AS-SOAP-WS-API-0902 - setPassword. User does not exist'   
   
    try:
            s.setPassword(token, 'stepbystep', 'stepbystep')
            print '\nFAILED: setPassword succeeded on bogus user'
    except SOAPpy.Types.faultType, err:
        #print '\nFAILED: %s. Unexpected exception %s' % (testcase, err.faultstring)
        test = 0
        pass
          
    # Log test results
    logStatus(test, expected, testcase)

    numTestCases += 1    
    
##  TESTCASE-AS-SOAP-WS-API-0903 - setPassword. Username is NULL - FAIL
    # use account from 0901, setPassword on NULL username
    test = 1
    expected = 'FAIL'
    testcase = 'TESTCASE-AS-SOAP-WS-API-0903 - setPassword. Username is NULL'   
    
    try:
            s.setPassword(token, None, 'metreos') 
            print '\nFAILED: setPassword succeeded with NULL username'
    except SOAPpy.Types.faultType, err:
        #print '\nFAILED: %s. Unexpected exception %s' % (testcase, err.faultstring)
        test = 0
        pass
          
    # Log test results
    logStatus(test, expected, testcase)

    numTestCases += 1

##  TESTCASE-AS-SOAP-WS-API-0904 - setPassword. Password is NULL - FAIL
    # use account from 0901, setPassword on NULL password
    test = 1
    expected = 'FAIL'
    testcase = 'TESTCASE-AS-SOAP-WS-API-0904 - setPassword. Password is NULL'   
    
    try:
            s.setPassword(token, userRecord['username'], None) 
            print '\nFAILED: setPassword succeeded with NULL password'
    except SOAPpy.Types.faultType, err:
        #print '\nFAILED: %s. Unexpected exception %s' % (testcase, err.faultstring)
        test = 0
        pass
          
    # Log test results
    logStatus(test, expected, testcase)

    numTestCases += 1
    
##  TESTCASE-AS-SOAP-WS-API-0905 - setPassword. Invalid token - FAIL
    # use account from 0901, setPassword on bogus token
    test = 1
    expected = 'FAIL'
    testcase = 'TESTCASE-AS-SOAP-WS-API-0905 - setPassword. Invalid token'   
    
    try:
            s.setPassword('bleh', userRecord['username'], 'metreos') 
            print '\nFAILED: setPassword succeeded with bad token'
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

            print "\nStarting setPassword test suite against %s\n" % args[0]
                            
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