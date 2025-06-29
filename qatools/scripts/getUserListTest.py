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

## Test for the getUser AppsuiteAdmin WebServices API
    s = SOAPpy.WSDL.Proxy('http://%s/appsuiteadmin/soap.php?wsdl' % ip)
    
    token = s.login('Administrator','metreos')

    numTestCases = 1
    numUsers = 3

##  I need to change this script so that it does a cleanup after the script that
##  deletes only users that are created while running this script.  This is a very
##  destructive action.    
    try:
        test = s.deleteAllUsers(token)
    except:
        print 'Unhandled exception in deleteAllUsers.'
        pass

##  TESTCASE-AS-SOAP-WS-API-0301 - getUserList. Users exist - PASS
    # create 3 users and get them
    test = 0
    failure = 0
    initUserRecord(userRecord)
    expected = 'PASS'
    testcase = 'TESTCASE-AS-SOAP-WS-API-0301 - getUserList. Users exist' 
    userRecord['firstName'] = 'Test%s' % numTestCases
    userRecord['lastName'] = 'User%s' % numTestCases
    userRecord['username'] = 'tuser%s' % numTestCases
    userRecord['password'] = 'metreos'
    userRecord['pin'] = numTestCases
    userRecord['accountCode'] = numTestCases
    userRecord['status'] = 'Active'

    try:
        s.addUser(token, userRecord)
    except SOAPpy.Types.faultType, err:
        failure = 1
        print '\nFAILED: %s. Unexpected exception %s' % (testcase, err.faultstring)
        pass
    
    if failure == 0:
        userRecord['firstName'] = 'Test%s' % (numTestCases + 1)
        userRecord['lastName'] = 'User%s' % (numTestCases + 1)
        userRecord['username'] = 'tuser%s' % (numTestCases + 1)
        userRecord['password'] = 'metreos1'
        userRecord['pin'] = numTestCases + 1
        userRecord['accountCode'] = numTestCases + 1
        userRecord['status'] = 'Active'
        
        try:
            s.addUser(token, userRecord)
        except SOAPpy.Types.faultType, err:
            failure = 1
            print '\nFAILED: %s. Unexpected exception %s' % (testcase, err.faultstring)
            pass
    
    if failure == 0:
        userRecord['firstName'] = 'Test%s' % (numTestCases + 2)
        userRecord['lastName'] = 'User%s' % (numTestCases + 2)
        userRecord['username'] = 'tuser%s' % (numTestCases + 2)
        userRecord['password'] = 'metreos2'
        userRecord['pin'] = numTestCases + 2
        userRecord['accountCode'] = numTestCases + 2
        userRecord['status'] = 'Active'
        
        try:
            s.addUser(token, userRecord)
        except SOAPpy.Types.faultType, err:
            failure = 1
            print '\nFAILED: %s. Unexpected exception %s' % (testcase, err.faultstring)
            pass
    
    if failure == 0:
        try:
            users = s.getUserList(token)
            
            userTest = len(users) > 0
            usernameCode = 0
            for user in users:
                userTest &= user == ('tuser%s' % (numTestCases + usernameCode))
                usernameCode = usernameCode + 1
               
            test = userTest
        except SOAPpy.Types.faultType, err:
            failure = 1
            print '\nFAILED: %s. Unexpected exception %s' % (testcase, err.faultstring)
            pass
        
    
    # Log test results
    logStatus(test, expected, testcase)

    numTestCases += 1    
        
##  TESTCASE-AS-SOAP-WS-API-0303 - getUserList. Invalid token - FAIL
    # attempt getUserList with bad token
    test = 0
    expected = 'FAIL'
    testcase = 'TESTCASE-AS-SOAP-WS-API-0303 - getUserList. Invalid token' 

    try:
        users = s.getUserList('bleh')
        test = 1
    except SOAPpy.Types.faultType, err:
            #print '\nFAILED: %s. Unexpected exception %s' % (testcase, err.faultstring)
            pass
    
    # Log test results
    logStatus(test, expected, testcase)

    numTestCases += 1    

##  TESTCASE-AS-SOAP-WS-API-0302 - getUserList. No users exist - PASS
    # deleteAllUsers, then perform getUserList
    test = 0
    expected = 'PASS'
    testcase = 'TESTCASE-AS-SOAP-WS-API-0302 - getUserList. No users exist' 
        
    try:
        s.deleteAllUsers(token)
        users = s.getUserList(token)
        test = users == None
    except SOAPpy.Types.faultType, err:
            print '\nFAILED: %s. Unexpected exception %s' % (testcase, err.faultstring)
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

            print "\nStarting getUser test suite against %s\n" % args[0]
                            
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