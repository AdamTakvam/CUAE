## Appsuiteadmin WebService Login API Test
##
## Implements testcases:
##  TESTCASE-AS-SOAP-WS-API-0201 - login. Username = administrator, password = metreos - PASS
##  TESTCASE-AS-SOAP-WS-API-0202 - login. Username = Administrator - PASS
##  TESTCASE-AS-SOAP-WS-API-0203 - login. Username = ADMINISTRATOR - PASS
##  TESTCASE-AS-SOAP-WS-API-0204 - login. Username exists but is not administrator - FAIL
##  TESTCASE-AS-SOAP-WS-API-0205 - login. Username does not exist - FAIL
##  TESTCASE-AS-SOAP-WS-API-0206 - login. Incorrect password - FAIL
##  TESTCASE-AS-SOAP-WS-API-0207 - login. Null username - FAIL
##  TESTCASE-AS-SOAP-WS-API-0208 - login. Null password - FAIL
##

import SOAPpy
import os
import string
import getopt
from optparse import OptionParser

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

##    Add command-line args later
##    appliance = 'vonnegut'
##    
##    # Process Options
##    parser = OptionParser()
##    parser.add_option('-a','--appliance',dest='appliance',default='vonnegut')
##
##    (options, args) = parser.parse_args()
##   
##    # Set appliance if it is passed in
##    if options.appliance:
##        appliance = options.appliance


## Test for the login AppsuiteAdmin WebServices API
    s = SOAPpy.WSDL.Proxy('http://vonnegut/appsuiteadmin/soap.php?wsdl')
    token = s.login('administrator','metreos')
    
## Setup

    if not s.userExists(token, 'LoginUser'):
        initUserRecord(userRecord)
        userRecord['firstName'] = 'Test'
        userRecord['lastName'] = 'User'
        userRecord['username'] = 'LoginUser'
        userRecord['password'] = 'metreos'

        accountCodeList = s.getAccountCodeList(token)
        maxAccountCode = 0

        if accountCodeList:
            for ac in accountCodeList:
                if ac > maxAccountCode:
                    maxAccountCode = ac      

        tempNum = int(maxAccountCode) + 1
            
        userRecord['accountCode'] = tempNum
        userRecord['pin'] = tempNum
      
        try:
            test = s.addUser(token, userRecord)
        except:
            print '\nFAILED: %s. Unexpected exception on create user in setup'

## End Setup        

##  TESTCASE-AS-SOAP-WS-API-0201 - login. Username = administrator, password = metreos - PASS
    expected = 'PASS'
    testcase = 'TESTCASE-AS-SOAP-WS-API-0201 - login. Username = administrator, password = metreos'

    try:
        token = s.login('administrator','metreos')
    except:
        if expected == 'FAIL':
            status = 'PASS'
        else:
            status = 'FAIL'
            
        print '%s Testcase: %s' % (status, testcase)
        pass
        
    # Log test results
    print '%s Testcase: %s' % (expected, testcase)

##  TESTCASE-AS-SOAP-WS-API-0202 - login. Username = Administrator - PASS
    expected = 'PASS'
    testcase = 'TESTCASE-AS-SOAP-WS-API-0202 - login. Username = Administrator'

    try:
        token = s.login('Administrator','metreos')
    except:
        if expected == 'FAIL':
            status = 'PASS'
        else:
            status = 'FAIL'
            
        print '%s Testcase: %s' % (status, testcase)
        pass
        
    # Log test results
    print '%s Testcase: %s' % (expected, testcase)

##  TESTCASE-AS-SOAP-WS-API-0203 - login. Username = ADMINISTRATOR - PASS
    expected = 'PASS'
    testcase = 'TESTCASE-AS-SOAP-WS-API-0203 - login. Username = ADMINISTRATOR'

    try:
        token = s.login('ADMINISTRATOR','metreos')
    except:
        if expected == 'FAIL':
            status = 'PASS'
        else:
            status = 'FAIL'
            
        print '%s Testcase: %s' % (status, testcase)
        pass
        
    # Log test results
    print '%s Testcase: %s' % (expected, testcase)

##  TESTCASE-AS-SOAP-WS-API-0204 - login. Username exists but is not administrator - FAIL
    expected = 'FAIL'
    testcase = 'TESTCASE-AS-SOAP-WS-API-0204 - login. Username exists but is not administrator'
    status = None
    
    try:
        token = s.login('LoginUser','metreos')
    except:
        if expected == 'FAIL':
            status = 'PASS'
        else:
            status = 'FAIL'
            
        print '%s Testcase: %s' % (status, testcase)
        pass
        
    # Log test results
    if status == None:
        print 'FAIL Testcase: %s' % testcase
        print 'An exception should have been thrown but but was not'
##
##  TESTCASE-AS-SOAP-WS-API-0205 - login. Username does not exist - FAIL
    expected = 'FAIL'
    testcase = 'TESTCASE-AS-SOAP-WS-API-0205 - login. Username does not exist'
    status = None

    try:
        token = s.login('notexist','metreos')
    except:
        if expected == 'FAIL':
            status = 'PASS'
        else:
            status = 'FAIL'
            
        print '%s Testcase: %s' % (status, testcase)
        pass
        
    # Log test results
    if status == None:
        print 'FAIL Testcase: %s' % testcase
        print 'An exception should have been thrown but but was not'
##

##  TESTCASE-AS-SOAP-WS-API-0206 - login. Incorrect password - FAIL
    expected = 'FAIL'
    testcase = 'TESTCASE-AS-SOAP-WS-API-0206 - login. Incorrect password'
    status = None
    
    try:
        token = s.login('Administrator','incorrect')
    except:
        if expected == 'FAIL':
            status = 'PASS'
        else:
            status = 'FAIL'
            
        print '%s Testcase: %s' % (status, testcase)
        pass
        
    # Log test results
    if status == None:
        print 'FAIL Testcase: %s' % testcase
        print 'An exception should have been thrown but but was not'
    
##
##  TESTCASE-AS-SOAP-WS-API-0207 - login. Null username - FAIL
    expected = 'FAIL'
    testcase = 'TESTCASE-AS-SOAP-WS-API-0207 - login. Null username'
    status = None
    
    try:
        token = s.login(None,'metreos')
    except:
        if expected == 'FAIL':
            status = 'PASS'
        else:
            status = 'FAIL'
            
        print '%s Testcase: %s' % (status, testcase)
        pass
        
    # Log test results
    if status == None:
        print 'FAIL Testcase: %s' % testcase
        print 'An exception should have been thrown but but was not'
##

##  TESTCASE-AS-SOAP-WS-API-0208 - login. Null password - FAIL
    expected = 'FAIL'
    testcase = 'TESTCASE-AS-SOAP-WS-API-0208 - login. Null password'
    status = None
    
    try:
        token = s.login('Administrator',None)
    except:
        if expected == 'FAIL':
            status = 'PASS'
        else:
            status = 'FAIL'
            
        print '%s Testcase: %s' % (status, testcase)
        pass
        
    # Log test results
    if status == None:
        print 'FAIL Testcase: %s' % testcase
        print 'An exception should have been thrown but but was not'
##

## Cleanup
    token = s.login('administrator','metreos')
    
    if s.userExists(token, 'LoginUser'):
        s.deleteUser(token,'LoginUser')

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

            print "\nStarting disableFindMeNumbers test suite against %s\n" % args[0]
                            
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



