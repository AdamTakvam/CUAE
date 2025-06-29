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

## Test for the addPin AppsuiteAdmin WebServices API
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

##  TESTCASE-AS-SOAP-WS-API-4201 - setPin. User exists - PASS
    # Add a user, which we will then reset the password of
    test = 0
    initUserRecord(userRecord)
    expected = 'PASS'
    testcase = 'TESTCASE-AS-SOAP-WS-API-4201 - setPin. User exists' 
    userRecord['firstName'] = 'Test%s' % numTestCases
    userRecord['lastName'] = 'User%s' % numTestCases
    userRecord['username'] = 'tuser%s' % numTestCases
    userRecord['password'] = 'metreos'
    userRecord['pin'] = numTestCases
    userRecord['accountCode'] = numTestCases
    userRecord['status'] = 'Active'

    try:
        s.addUser(token, userRecord)
        s.setPin(token, userRecord['username'], 1111222)
        pin = s.getPin(token, userRecord['username'])[0]
        test = '1111222' == pin
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
    
##  TESTCASE-AS-SOAP-WS-API-4202 - setPin. User does not exist - FAIL
    # use account from 4201, setPin on bogus username
    test = 1
    expected = 'FAIL'
    testcase = 'TESTCASE-AS-SOAP-WS-API-4202 - setPin. User does not exist'   
   
    try:
            s.setPin(token, '444445', 444445)
            print '\nFAILED: setPin succeeded on bogus user'
    except SOAPpy.Types.faultType, err:
        #print '\nFAILED: %s. Unexpected exception %s' % (testcase, err.faultstring)
        test = 0
        pass
          
    # Log test results
    logStatus(test, expected, testcase)

    numTestCases += 1    
    
##  TESTCASE-AS-SOAP-WS-API-4203 - setPin. User is NULL - FAIL
    # use account from 4201, setPassword on NULL username
    test = 1
    expected = 'FAIL'
    testcase = 'TESTCASE-AS-SOAP-WS-API-4203 - setPin. User is NULL - FAIL'   
    
    try:
            s.setPin(token, None, 444445) 
            print '\nFAILED: setPin succeeded with NULL username'
    except SOAPpy.Types.faultType, err:
        #print '\nFAILED: %s. Unexpected exception %s' % (testcase, err.faultstring)
        test = 0
        pass
          
    # Log test results
    logStatus(test, expected, testcase)

    numTestCases += 1

##  TESTCASE-AS-SOAP-WS-API-4204 - setPin. Invalid integer value for pin - FAIL
    # use account from 4201, setPin on NULL pin
    test = 1
    expected = 'FAIL'
    testcase = 'TESTCASE-AS-SOAP-WS-API-4204 - setPin. Invalid integer value for pin'   
    
    try:
            s.setPin(token, userRecord['username'], 'bleh')
            print '\nFAILED: setPin succeeded with bad pin'
    except SOAPpy.Types.faultType, err:
        #print '\nFAILED: %s. Unexpected exception %s' % (testcase, err.faultstring)
        test = 0
        pass
          
    # Log test results
    logStatus(test, expected, testcase)

    numTestCases += 1
    
##  TESTCASE-AS-SOAP-WS-API-4205 - setPin. Pin is NULL - FAIL
    # use account from 4201, setPinon NULL pin
    test = 1
    expected = 'FAIL'
    testcase = 'TESTCASE-AS-SOAP-WS-API-4205 - setPin. Pin is NULL'   
    
    try:
            s.setPin(token, userRecord['username'], None) 
            print '\nFAILED: setPin succeeded with NULL pin'
    except SOAPpy.Types.faultType, err:
        #print '\nFAILED: %s. Unexpected exception %s' % (testcase, err.faultstring)
        test = 0
        pass
          
    # Log test results
    logStatus(test, expected, testcase)

    numTestCases += 1
    
##  TESTCASE-AS-SOAP-WS-API-4206 - setPin. Invalid token - FAIL
    # use account from 4201, setPin on bogus token
    test = 1
    expected = 'FAIL'
    testcase = 'TESTCASE-AS-SOAP-WS-API-4206 - setPin. Invalid token'   
    
    try:
            s.setPassword('bleh', userRecord['username'], 'metreos') 
            print '\nFAILED: setPin succeeded with bad token'
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

            print "\nStarting setPin test suite against %s\n" % args[0]
                            
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