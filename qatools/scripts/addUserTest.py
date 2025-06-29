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

## Test for the addUser AppsuiteAdmin WebServices API
    s = SOAPpy.WSDL.Proxy('http://vonnegut/appsuiteadmin/soap.php?wsdl')
    
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

##  TESTCASE-AS-SOAP-WS-API-0101 - Add User. Required values only - PASS
    initUserRecord(userRecord)
    expected = 'PASS'
    testcase = 'TESTCASE-AS-SOAP-WS-API-0101 - Add User. Required values only' 
    userRecord['firstName'] = 'Test%s' % numTestCases
    userRecord['lastName'] = 'User%s' % numTestCases
    userRecord['username'] = 'tuser%s' % numTestCases
    userRecord['password'] = 'metreos'
    userRecord['pin'] = numTestCases
    userRecord['accountCode'] = numTestCases
    
    try:
        test = s.addUser(token, userRecord)
    except:
        print '\nFAILED: %s. Unexpected exception' % testcase
        pass
    # Log test results
    logStatus(test, expected, testcase)

    # Running the last test cases verified the following as well...
    logStatus(test, 'PASS', 'TESTCASE-AS-SOAP-WS-API-0111 - Add User. Null email')
    logStatus(test, 'PASS', 'TESTCASE-AS-SOAP-WS-API-0112 - Add User. Null status')
    logStatus(test, 'PASS', 'TESTCASE-AS-SOAP-WS-API-0113 - Add User. Null lockout threshold')
    logStatus(test, 'PASS', 'TESTCASE-AS-SOAP-WS-API-0114 - Add User. Null lockout duration')
    logStatus(test, 'PASS', 'TESTCASE-AS-SOAP-WS-API-0115 - Add User. Null max sessions')
    logStatus(test, 'PASS', 'TESTCASE-AS-SOAP-WS-API-0116 - Add User. Null pin change')
    logStatus(test, 'PASS', 'TESTCASE-AS-SOAP-WS-API-0117 - Add User. Null record')
    logStatus(test, 'PASS', 'TESTCASE-AS-SOAP-WS-API-0118 - Add User. Null record visible')
    logStatus(test, 'PASS', 'TESTCASE-AS-SOAP-WS-API-0119 - Add User. Null timezone')
    logStatus(test, 'PASS', 'TESTCASE-AS-SOAP-WS-API-0120 - Add User. Null AR transfer number')
    numTestCases += 11
    
##  TESTCASE-AS-SOAP-WS-API-0102 - Add User. All fields set to valid values - PASS
    initUserRecord(userRecord)
    expected = 'PASS'
    testcase = 'TESTCASE-AS-SOAP-WS-API-0102 - Add User. All fields set to valid values'   
    userRecord['firstName'] = 'Test%s' % numTestCases
    userRecord['lastName'] = 'User%s' % numTestCases
    userRecord['username'] = 'tuser%s' % numTestCases
    userRecord['password'] = 'metreos'
    userRecord['pin'] = numTestCases
    userRecord['accountCode'] = numTestCases
    userRecord['email'] = 'tuser%s@metreos.com' % numTestCases
    userRecord['status'] = 'Locked'
    userRecord['lockoutThreshold'] = 3
    userRecord['lockoutDuration'] = 70
    userRecord['maxSessions'] = 3
    userRecord['pinChange'] = True
    userRecord['record'] = True
    userRecord['recordVisible'] = True
    userRecord['timezone'] = 'Zulu'
    userRecord['arTransferNumber'] = '2000'
        
    try:
        test = s.addUser(token, userRecord)
    except:
        print '\nFAILED: %s. Unexpected exception' % testcase
        pass
    logStatus(test, expected, testcase)
    numTestCases += 1

##  TESTCASE-AS-SOAP-WS-API-0103 - Add User. Username already exists - FAIL
    initUserRecord(userRecord)
    expected = 'FAIL'
    testcase = 'TESTCASE-AS-SOAP-WS-API-0103 - Add User. Username already exists'   
    userRecord['firstName'] = 'Test%s' % numTestCases
    userRecord['lastName'] = 'User%s' % numTestCases
    userRecord['username'] = 'tuser%s' % numTestCases
    userRecord['password'] = 'metreos'
    userRecord['pin'] = numTestCases
    userRecord['accountCode'] = numTestCases
        
    try:
        rc = s.addUser(token, userRecord)
    except:
        print 'FAILED: %s. Unexpected exception' % testcase
        
    fakeUserNum = numTestCases+1
    userRecord['accountCode'] = fakeUserNum
    
    try:
        test = s.addUser(token, userRecord)
    except:
        test = 0
        pass
    
    logStatus(test, expected, testcase)
    numTestCases += 1

##  TESTCASE-AS-SOAP-WS-API-0104 - Add User. Account code already exists - FAIL
    initUserRecord(userRecord)
    expected = 'FAIL'
    testcase = 'TESTCASE-AS-SOAP-WS-API-0104 - Add User. Account code already exists'
    userRecord['firstName'] = 'Test%s' % numTestCases
    userRecord['lastName'] = 'User%s' % numTestCases
    userRecord['username'] = 'tuser%s' % numTestCases
    userRecord['password'] = 'metreos'
    userRecord['pin'] = numTestCases
    userRecord['accountCode'] = numTestCases
       
    try:
        rc = s.addUser(token, userRecord)
    except:
        print 'FAILED: %s. Unexpected exception' % testcase
        pass

    fakeUserNum = numTestCases+1
    userRecord['username'] = 'tuser%s' % fakeUserNum

    try:
        test = s.addUser(token, userRecord)
    except:
        test = 0
        pass
    
    logStatus(test, expected, testcase)
    numTestCases += 1

##  TESTCASE-AS-SOAP-WS-API-0105 - Add User. Null first name - FAIL
    initUserRecord(userRecord)
    expected = 'FAIL'
    testcase = 'TESTCASE-AS-SOAP-WS-API-0105 - Add User. Null first name'
    userRecord['firstName'] = None
    userRecord['lastName'] = 'User%s' % numTestCases
    userRecord['username'] = 'tuser%s' % numTestCases
    userRecord['password'] = 'metreos'
    userRecord['pin'] = numTestCases
    userRecord['accountCode'] = numTestCases

    try:
        test = s.addUser(token, userRecord)
    except:
        test = 0
        pass
    
    logStatus(test, expected, testcase)
    numTestCases += 1

##  TESTCASE-AS-SOAP-WS-API-0106 - Add User. Null last name - FAIL
    initUserRecord(userRecord)
    expected = 'FAIL'
    testcase = 'TESTCASE-AS-SOAP-WS-API-0106 - Add User. Null last name'
    userRecord['firstName'] = 'Test%s' % numTestCases
    userRecord['lastName'] = None
    userRecord['username'] = 'tuser%s' % numTestCases
    userRecord['password'] = 'metreos'
    userRecord['pin'] = numTestCases
    userRecord['accountCode'] = numTestCases

    try:
        test = s.addUser(token, userRecord)
    except:
        test = 0
        pass
    
    logStatus(test, expected, testcase)
    numTestCases += 1

##  TESTCASE-AS-SOAP-WS-API-0107 - Add User. Null username - FAIL
    initUserRecord(userRecord)
    expected = 'FAIL'
    testcase = 'TESTCASE-AS-SOAP-WS-API-0107 - Add User. Null username'
    userRecord['firstName'] = 'Test%s' % numTestCases
    userRecord['lastName'] = 'User%s' % numTestCases
    userRecord['username'] = None
    userRecord['password'] = 'metreos'
    userRecord['pin'] = numTestCases
    userRecord['accountCode'] = numTestCases

    try:
        test = s.addUser(token, userRecord)
    except:
        test = 0
        pass
    
    logStatus(test, expected, testcase)
    numTestCases += 1

##  TESTCASE-AS-SOAP-WS-API-0108 - Add User. Null password - FAIL
    initUserRecord(userRecord)
    expected = 'FAIL'
    testcase = 'TESTCASE-AS-SOAP-WS-API-0108 - Add User. Null password'
    userRecord['firstName'] = 'Test%s' % numTestCases
    userRecord['lastName'] = 'User%s' % numTestCases
    userRecord['username'] = 'tuser%s' % numTestCases
    userRecord['password'] = None
    userRecord['pin'] = numTestCases
    userRecord['accountCode'] = numTestCases

    try:
        test = s.addUser(token, userRecord)
    except:
        test = 0
        pass
    
    logStatus(test, expected, testcase)
    numTestCases += 1

##  TESTCASE-AS-SOAP-WS-API-0109 - Add User. Null pin - FAIL
    initUserRecord(userRecord)
    expected = 'FAIL'
    testcase = 'TESTCASE-AS-SOAP-WS-API-0109 - Add User. Null pin - FAIL'
    userRecord['firstName'] = 'Test%s' % numTestCases
    userRecord['lastName'] = 'User%s' % numTestCases
    userRecord['username'] = 'tuser%s' % numTestCases
    userRecord['password'] = 'metreos'
    userRecord['pin'] = None
    userRecord['accountCode'] = numTestCases

    try:
        test = s.addUser(token, userRecord)
    except:
        test = 0
        pass
    
    logStatus(test, expected, testcase)
    numTestCases += 1

##  TESTCASE-AS-SOAP-WS-API-0110 - Add User. Null account code - FAIL
    initUserRecord(userRecord)
    expected = 'FAIL'
    testcase = 'TESTCASE-AS-SOAP-WS-API-0110 - Add User. Null account code'
    userRecord['firstName'] = 'Test%s' % numTestCases
    userRecord['lastName'] = 'User%s' % numTestCases
    userRecord['username'] = 'tuser%s' % numTestCases
    userRecord['password'] = 'metreos'
    userRecord['pin'] = numTestCases
    userRecord['accountCode'] = None

    try:
        test = s.addUser(token, userRecord)
    except:
        test = 0
        pass
    
    logStatus(test, expected, testcase)
    numTestCases += 1

##  TESTCASE-AS-SOAP-WS-API-0121 - Add User. invalid pin - FAIL
    initUserRecord(userRecord)
    expected = 'FAIL'
    testcase = 'TESTCASE-AS-SOAP-WS-API-0121 - Add User. invalid pin'
    userRecord['firstName'] = 'Test%s' % numTestCases
    userRecord['lastName'] = 'User%s' % numTestCases
    userRecord['username'] = 'tuser%s' % numTestCases
    userRecord['password'] = 'metreos'
    userRecord['pin'] = 'hello'
    userRecord['accountCode'] = numTestCases

    try:
        test = s.addUser(token, userRecord)
    except:
        test = 0
        pass
    
    logStatus(test, expected, testcase)
    numTestCases += 1

##  TESTCASE-AS-SOAP-WS-API-0122 - Add User. invalid account code - FAIL
    initUserRecord(userRecord)
    expected = 'FAIL'
    testcase = 'TESTCASE-AS-SOAP-WS-API-0122 - Add User. invalid account code'
    userRecord['firstName'] = 'Test%s' % numTestCases
    userRecord['lastName'] = 'User%s' % numTestCases
    userRecord['username'] = 'tuser%s' % numTestCases
    userRecord['password'] = 'metreos'
    userRecord['pin'] = numTestCases
    userRecord['accountCode'] = 'hello'

    try:
        test = s.addUser(token, userRecord)
    except:
        test = 0
        pass
    
    logStatus(test, expected, testcase)
    numTestCases += 1

##  TESTCASE-AS-SOAP-WS-API-0123 - Add User. Set status to Active - PASS
    initUserRecord(userRecord)
    expected = 'PASS'
    testcase = 'TESTCASE-AS-SOAP-WS-API-0123 - Add User. Set status to Active'
    userRecord['firstName'] = 'Test%s' % numTestCases
    userRecord['lastName'] = 'User%s' % numTestCases
    userRecord['username'] = 'tuser%s' % numTestCases
    userRecord['password'] = 'metreos'
    userRecord['pin'] = numTestCases
    userRecord['accountCode'] = numTestCases
    userRecord['status'] = 'Active'

    try:
        test = s.addUser(token, userRecord)
    except:
        test = 0
        pass
    
    logStatus(test, expected, testcase)
    numTestCases += 1

##  TESTCASE-AS-SOAP-WS-API-0124 - Add User. Set status to Locked - PASS
    initUserRecord(userRecord)
    expected = 'PASS'
    testcase = 'TESTCASE-AS-SOAP-WS-API-0124 - Add User. Set status to Locked'
    userRecord['firstName'] = 'Test%s' % numTestCases
    userRecord['lastName'] = 'User%s' % numTestCases
    userRecord['username'] = 'tuser%s' % numTestCases
    userRecord['password'] = 'metreos'
    userRecord['pin'] = numTestCases
    userRecord['accountCode'] = numTestCases
    userRecord['status'] = 'Locked'

    try:
        test = s.addUser(token, userRecord)
    except:
        test = 0
        pass
    
    logStatus(test, expected, testcase)
    numTestCases += 1

##  TESTCASE-AS-SOAP-WS-API-0125 - Add User. Set status to Disabled - PASS
    initUserRecord(userRecord)
    expected = 'PASS'
    testcase = 'TESTCASE-AS-SOAP-WS-API-0125 - Add User. Set status to Disabled'
    userRecord['firstName'] = 'Test%s' % numTestCases
    userRecord['lastName'] = 'User%s' % numTestCases
    userRecord['username'] = 'tuser%s' % numTestCases
    userRecord['password'] = 'metreos'
    userRecord['pin'] = numTestCases
    userRecord['accountCode'] = numTestCases
    userRecord['status'] = 'Disabled'

    try:
        test = s.addUser(token, userRecord)
    except:
        test = 0
        pass
    
    logStatus(test, expected, testcase)
    numTestCases += 1

##  TESTCASE-AS-SOAP-WS-API-0126 - Add User. Set status to Deleted - PASS
    initUserRecord(userRecord)
    expected = 'PASS'
    testcase = 'TESTCASE-AS-SOAP-WS-API-0126 - Add User. Set status to Deleted'
    userRecord['firstName'] = 'Test%s' % numTestCases
    userRecord['lastName'] = 'User%s' % numTestCases
    userRecord['username'] = 'tuser%s' % numTestCases
    userRecord['password'] = 'metreos'
    userRecord['pin'] = numTestCases
    userRecord['accountCode'] = numTestCases
    userRecord['status'] = 'Deleted'

    try:
        test = s.addUser(token, userRecord)
    except:
        test = 0
        pass
    
    logStatus(test, expected, testcase)
    numTestCases += 1

##  TESTCASE-AS-SOAP-WS-API-0127 - Add User. Set status to invalid value - FAIL
    initUserRecord(userRecord)
    expected = 'FAIL'
    testcase = 'TESTCASE-AS-SOAP-WS-API-0127 - Add User. Set status to invalid value'
    userRecord['firstName'] = 'Test%s' % numTestCases
    userRecord['lastName'] = 'User%s' % numTestCases
    userRecord['username'] = 'tuser%s' % numTestCases
    userRecord['password'] = 'metreos'
    userRecord['pin'] = numTestCases
    userRecord['accountCode'] = numTestCases
    userRecord['status'] = 'Unhappy'

    try:
        test = s.addUser(token, userRecord)
    except:
         test = 0
         pass
    
    logStatus(test, expected, testcase)
    numTestCases += 1

##  TESTCASE-AS-SOAP-WS-API-0128 - Add User. Invalid integer value for lockout threshold - FAIL
    initUserRecord(userRecord)
    expected = 'FAIL'
    testcase = 'TESTCASE-AS-SOAP-WS-API-0128 - Add User. Invalid integer value for lockout threshold'
    userRecord['firstName'] = 'Test%s' % numTestCases
    userRecord['lastName'] = 'User%s' % numTestCases
    userRecord['username'] = 'tuser%s' % numTestCases
    userRecord['password'] = 'metreos'
    userRecord['pin'] = numTestCases
    userRecord['accountCode'] = numTestCases
    userRecord['lockoutThreshold'] = 'hello'

    try:
        test = s.addUser(token, userRecord)
    except:
        test = 0
        pass
    
    logStatus(test, expected, testcase)
    numTestCases += 1

##  [TESTCASE-AS-SOAP-WS-API-0129] - Add User. Invalid integer value for lockout duration - FAIL
    initUserRecord(userRecord)
    expected = 'FAIL'
    testcase = 'TESTCASE-AS-SOAP-WS-API-0129 - Add User. Invalid integer value for lockout duration'
    userRecord['firstName'] = 'Test%s' % numTestCases
    userRecord['lastName'] = 'User%s' % numTestCases
    userRecord['username'] = 'tuser%s' % numTestCases
    userRecord['password'] = 'metreos'
    userRecord['pin'] = numTestCases
    userRecord['accountCode'] = numTestCases
    userRecord['lockoutDuration'] = 'hello'

    try:
        test = s.addUser(token, userRecord)
    except:
        test = 0
        pass
    
    logStatus(test, expected, testcase)
    numTestCases += 1

##  TESTCASE-AS-SOAP-WS-API-0130 - Add User. Invalid integer value for max sessions - FAIL
    initUserRecord(userRecord)
    expected = 'FAIL'
    testcase = 'TESTCASE-AS-SOAP-WS-API-0130 - Add User. Invalid integer value for max sessions'
    userRecord['firstName'] = 'Test%s' % numTestCases
    userRecord['lastName'] = 'User%s' % numTestCases
    userRecord['username'] = 'tuser%s' % numTestCases
    userRecord['password'] = 'metreos'
    userRecord['pin'] = numTestCases
    userRecord['accountCode'] = numTestCases
    userRecord['maxSessions'] = 'hello'

    try:
        test = s.addUser(token, userRecord)
    except:
        test = 0
        pass
    
    logStatus(test, expected, testcase)
    numTestCases += 1

##  TESTCASE-AS-SOAP-WS-API-0131 - Add User. Invalid boolean value for pin change - FAIL
    initUserRecord(userRecord)
    expected = 'FAIL'
    testcase = 'TESTCASE-AS-SOAP-WS-API-0131 - Add User. Invalid boolean value for pin change'
    userRecord['firstName'] = 'Test%s' % numTestCases
    userRecord['lastName'] = 'User%s' % numTestCases
    userRecord['username'] = 'tuser%s' % numTestCases
    userRecord['password'] = 'metreos'
    userRecord['pin'] = numTestCases
    userRecord['accountCode'] = numTestCases
    userRecord['pinChange'] = 2

    try:
        test = s.addUser(token, userRecord)
    except:
        test = 0
        pass
    
    logStatus(test, expected, testcase)
    numTestCases += 1

##  TESTCASE-AS-SOAP-WS-API-0132 - Add User. Invalid boolean value for record
    initUserRecord(userRecord)
    expected = 'FAIL'
    testcase = 'TESTCASE-AS-SOAP-WS-API-0131 - Add User. Invalid boolean value for record'
    userRecord['firstName'] = 'Test%s' % numTestCases
    userRecord['lastName'] = 'User%s' % numTestCases
    userRecord['username'] = 'tuser%s' % numTestCases
    userRecord['password'] = 'metreos'
    userRecord['pin'] = numTestCases
    userRecord['accountCode'] = numTestCases
    userRecord['record'] = 'hello'

    try:
        test = s.addUser(token, userRecord)
    except:
        test = 0
        pass
    
    logStatus(test, expected, testcase)
    numTestCases += 1

##  TESTCASE-AS-SOAP-WS-API-0133 - Add User. Invalid boolean value for record visble - FAIL
    initUserRecord(userRecord)
    expected = 'FAIL'
    testcase = 'TESTCASE-AS-SOAP-WS-API-0133 - Add User. Invalid boolean value for record visble'
    userRecord['firstName'] = 'Test%s' % numTestCases
    userRecord['lastName'] = 'User%s' % numTestCases
    userRecord['username'] = 'tuser%s' % numTestCases
    userRecord['password'] = 'metreos'
    userRecord['pin'] = numTestCases
    userRecord['accountCode'] = numTestCases
    userRecord['recordVisible'] = 1.1

    try:
        test = s.addUser(token, userRecord)
    except:
        test = 0
        pass
    
    logStatus(test, expected, testcase)
    numTestCases += 1

##  TESTCASE-AS-SOAP-WS-API-0134 - Add User. Invalid timezone - FAIL
    initUserRecord(userRecord)
    expected = 'FAIL'
    testcase = 'TESTCASE-AS-SOAP-WS-API-0134 - Add User. Invalid timezone'
    userRecord['firstName'] = 'Test%s' % numTestCases
    userRecord['lastName'] = 'User%s' % numTestCases
    userRecord['username'] = 'tuser%s' % numTestCases
    userRecord['password'] = 'metreos'
    userRecord['pin'] = numTestCases
    userRecord['accountCode'] = numTestCases
    userRecord['timezone'] = 'jantime'

    try:
        test = s.addUser(token, userRecord)
    except:
        test = 0
        pass
    
    logStatus(test, expected, testcase)
    numTestCases += 1

##  TESTCASE-AS-SOAP-WS-API-0135 - Add User. Username is administrator - FAIL
    initUserRecord(userRecord)
    expected = 'FAIL'
    testcase = 'TESTCASE-AS-SOAP-WS-API-0135 - Add User. Username is administrator - FAIL'
    userRecord['firstName'] = 'Test%s' % numTestCases
    userRecord['lastName'] = 'User%s' % numTestCases
    userRecord['username'] = 'administrator'
    userRecord['password'] = 'metreos'
    userRecord['pin'] = numTestCases
    userRecord['accountCode'] = numTestCases

    try:
        test = s.addUser(token, userRecord)
    except:
        test = 0
        pass
    
    logStatus(test, expected, testcase)
    numTestCases += 1

##  TESTCASE-AS-SOAP-WS-API-0136 - Add User. Username is ADMINISTRATOR - FAIL
    initUserRecord(userRecord)
    expected = 'FAIL'
    testcase = 'TESTCASE-AS-SOAP-WS-API-0136 - Add User. Username is ADMINISTRATOR - FAIL'
    userRecord['firstName'] = 'Test%s' % numTestCases
    userRecord['lastName'] = 'User%s' % numTestCases
    userRecord['username'] = 'ADMINISTRATOR'
    userRecord['password'] = 'metreos'
    userRecord['pin'] = numTestCases
    userRecord['accountCode'] = numTestCases

    try:
        test = s.addUser(token, userRecord)
    except:
        test = 0
        pass
    
    logStatus(test, expected, testcase)
    numTestCases += 1    

##  TESTCASE-AS-SOAP-WS-API-0137 - Add User. Username is Administrator - FAIL
    initUserRecord(userRecord)
    expected = 'FAIL'
    testcase = 'TESTCASE-AS-SOAP-WS-API-0137 - Add User. Username is Administrator - FAIL'
    userRecord['firstName'] = 'Test%s' % numTestCases
    userRecord['lastName'] = 'User%s' % numTestCases
    userRecord['username'] = 'Administrator'
    userRecord['password'] = 'metreos'
    userRecord['pin'] = numTestCases
    userRecord['accountCode'] = numTestCases

    try:
        test = s.addUser(token, userRecord)
    except:
        test = 0
        pass
    
    logStatus(test, expected, testcase)
    numTestCases += 1

##  TESTCASE-AS-SOAP-WS-API-0138 - Add User. username exists, status = Deleted. PASS
    initUserRecord(userRecord)
    expected = 'PASS'
    testcase = 'TESTCASE-AS-SOAP-WS-API-0137 - Add User. username exists, status = Deleted'
    userRecord['firstName'] = 'Test%s' % numTestCases
    userRecord['lastName'] = 'User%s' % numTestCases
    userRecord['username'] = 'tuser%s' % numTestCases
    userRecord['password'] = 'metreos'
    userRecord['pin'] = numTestCases
    userRecord['accountCode'] = numTestCases

    try:
        test = s.addUser(token, userRecord)
    except:
        print 'Caught unexpected exception in addUser'
        pass
    
    try:
        test = s.deleteUser(token, userRecord['username'])
    except:
        print 'Caught unexpected exception in deleteUser'
        pass
    
    try:
        test = s.addUser(token, userRecord)
    except:
        test = 0
        pass
    
    logStatus(test, expected, testcase)
    numTestCases += 1           
## 
    test = s.getUserList(token)
    print '\nUsers: ', repr(test)

##    I will want to cleanup after running the test at some point.  I need to change
##    this script so that it only deletes users that were created during the test
##    try:
##        test = s.deleteAllUsers(token)
##    except:
##        print 'Unhandled exception in deleteAllUsers.'
##        pass


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

            print "\nStarting addUser test suite against %s\n" % args[0]
                            
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

