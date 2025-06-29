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

## Test for the deleteUser AppsuiteAdmin WebServices API
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

##  TESTCASE-AS-SOAP-WS-API-0801 - deleteUser. User exists - PASS
    # Add a user to delete
    initUserRecord(userRecord)
    expected = 'PASS'
    testcase = 'TESTCASE-AS-SOAP-WS-API-0801 - deleteUser. User exists' 
    userRecord['firstName'] = 'Test%s' % numTestCases
    userRecord['lastName'] = 'User%s' % numTestCases
    userRecord['username'] = 'tuser%s' % numTestCases
    userRecord['password'] = 'metreos'
    userRecord['pin'] = numTestCases
    userRecord['accountCode'] = numTestCases

    try:
        test = s.addUser(token, userRecord)
    except SOAPpy.Types.faultType, err:
        print '\nFAILED: %s. Unexpected exception %s' % (testcase, err.faultstring)
        pass
    
    if test == 1: # Carry on with test if addUser succeeds
        try:
            test = s.deleteUser(token, userRecord['username'])
        except SOAPpy.Types.faultType, err:
            #print '\nFAILED: %s. Unexpected exception %s' % (testcase, err.faultstring)
            pass
        
    # Log test results
    logStatus(test, expected, testcase)

    numTestCases += 1    
    
##  TESTCASE-AS-SOAP-WS-API-0802 - deleteUser. User does not exist - FAIL
    noException = 1
    expected = 'FAIL'
    testcase = 'TESTCASE-AS-SOAP-WS-API-0802 - deleteUser. User does not exist'   
   
    try:
        test = s.deleteUser(token, userRecord['username'])
    except SOAPpy.Types.faultType, err:
        noException = 0
        #print '\nFAILED: %s. Unexpected exception %s' % (testcase, err.faultstring)
        pass
        
    # Log test results
    logStatus(noException, expected, testcase)

    numTestCases += 1    
    
##  TESTCASE-AS-SOAP-WS-API-0803 - deleteUser. Username is NULL - FAIL
    noException = 1
    expected = 'FAIL'
    testcase = 'TESTCASE-AS-SOAP-WS-API-0803 - deleteUser. Username is NULL'   
   
    try:
        test = s.deleteUser(token, None)
    except SOAPpy.Types.faultType, err:
        noException = 0
        #print '\nFAILED: %s. Unexpected exception %s' % (testcase, err.faultstring)
        pass
        
    logStatus(noException, expected, testcase)
    
    numTestCases += 1

##  TESTCASE-AS-SOAP-WS-API-0804 - deleteUser. Invalid token - FAIL
    test = 0
    noException = 1
    expected = 'FAIL'
    testcase = 'TESTCASE-AS-SOAP-WS-API-0804 - deleteUser. Invalid token'
    
    try:
        test = s.addUser(token, userRecord)
    except SOAPpy.Types.faultType, err:
        print '\nFAILED: %s. Unexpected exception %s' % (testcase, err.faultstring)
        pass
    
    if test == 1:
        try:
            test = s.deleteUser(None, userRecord)
        except SOAPpy.Types.faultType, err:
            noException = 0
            #print '\nFAILED: %s. Unexpected exception %s' % (testcase, err.faultstring)
            pass
        
    logStatus(noException, expected, testcase)
    
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

            print "\nStarting deleteUser test suite against %s\n" % args[0]
                            
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