import SOAPpy
import os
import getopt
import string
import mysqllib
import MySQLdb

# YOU HAVE TO ENABLE REMOTE ACCESS OF THE ROOT USER TO MAKE THIS TEST WORK

# mysql howto
# GRANT ALL PRIVILEGES ON *.* TO 'root'@'%' IDENTIFIED BY 'metreos' WITH GRANT OPTION;
# FLUSH PRIVILEGES;

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

## Test for the enableFindMeNumbers AppsuiteAdmin WebServices API
    db = mysqllib.DB(host=ip, user='root', passwd='metreos', db='application_suite')
    s = SOAPpy.WSDL.Proxy('http://%s/appsuiteadmin/soap.php?wsdl' % ip)
    
    token = s.login('Administrator','metreos')

    numTestCases = 1
    numUsers = 3

##  I need to change this script so that it does a cleanup after the script that
##  deletes only users that are created while running this script.  This is a very
##  destructive action.    
    try:
        test = s.deleteAllUsers(token)
        db.execute('delete from as_external_numbers')
    except:
        print 'Unhandled exception in deleteAllUsers.'
        pass

##  TESTCASE-AS-SOAP-WS-API-1201 - enableFindMeNumbers for user. respectVoiceMail = TRUE - PASS
    # create a user, and add 3 Find Me numbers
        test = 0
    failure = 0
    initUserRecord(userRecord)
    expected = 'PASS'
    testcase = 'TESTCASE-AS-SOAP-WS-API-1201 - enableFindMeNumbers for user. respectVoiceMail = TRUE' 
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
    
    userId = 0
    
    if failure == 0:
        # using mysql to add 3 find me numbers to that user
        # first get as_users_id
        userId = db.query('select as_users_id from as_users')[0]['as_users_id']
        
        findMe = 0;
        # add 3 find me numbers, one marked as voicemail
        db.execute('insert into as_external_numbers (as_users_id, name, phone_number, delay_call_time, call_attempt_timeout, is_corporate, ar_enabled, is_blacklisted) ' + 
                    'VALUES (%s, \'%s\', \'%s\', %s, %s, %s, %s, %s)' % 
                    (userId, 'Office1', '11111', 0, 0, 1, 0, 0))
    
        db.execute('insert into as_external_numbers (as_users_id, name, phone_number, delay_call_time, call_attempt_timeout, is_corporate, ar_enabled, is_blacklisted) ' + 
                    'VALUES (%s, \'%s\', \'%s\', %s, %s, %s, %s, %s)' % 
                    (userId, 'Office2', '22222', 0, 0, 0, 0, 0))
                    
        db.execute('insert into as_external_numbers (as_users_id, name, phone_number, delay_call_time, call_attempt_timeout, is_corporate, ar_enabled, is_blacklisted) ' + 
                    'VALUES (%s, \'%s\', \'%s\', %s, %s, %s, %s, %s)' % 
                    (userId, 'Office3', '33333', 0, 0, 0, 0, 0))
                    
        s.enableFindMeNumbers(token, userRecord['username'], True)
        
        result = db.query('select * from as_external_numbers where ar_enabled = 1 ')

        test = result != None and len(result) == 1 and result[0]['name'] == 'Office1'
            
    # Log test results
    logStatus(test, expected, testcase)

    numTestCases += 1    
            
##  TESTCASE-AS-SOAP-WS-API-1202 - enableFindMeNumbers for user. respectVoiceMail = FALSE - PASS
    # disalbe all find mes, then do enable with rvm = false
    test = 0
    expected = 'PASS'
    testcase = 'TESTCASE-AS-SOAP-WS-API-1202 - enableFindMeNumbers for user. respectVoiceMail = FALSE' 

    # add 3 find me numbers, one marked as voicemail
    db.execute('update as_external_numbers set ar_enabled = 0')
            
    s.enableFindMeNumbers(token, userRecord['username'], False)
    
    result = db.query('select * from as_external_numbers where ar_enabled = 1 ')

    test = result != None and len(result) == 3
            
    # Log test results
    logStatus(test, expected, testcase)

    numTestCases += 1    
    
##  TESTCASE-AS-SOAP-WS-API-1203 - enableFindMeNumbers for user. Invalid value for respectVoiceMail - FAIL
    test = 1
    expected = 'FAIL'
    testcase = 'TESTCASE-AS-SOAP-WS-API-1203 - enableFindMeNumbers for user. Invalid value for respectVoiceMail' 

    try:
        s.enableFindMeNumbers(token, userRecord['username'], 'bleh')
        print 'enableFindMeNumbers succeeded on bad respectVoiceMail value'
    except SOAPpy.Types.faultType, err:
        test = 0
        #print '\nFAILED: %s. Unexpected exception %s' % (testcase, err.faultstring)
        pass
          
    # Log test results
    logStatus(test, expected, testcase)

    numTestCases += 1    
    
##  TESTCASE-AS-SOAP-WS-API-1204 - enableFindMeNumbers for user. User does not exist - FAIL
    test = 1
    expected = 'FAIL'
    testcase = 'TESTCASE-AS-SOAP-WS-API-1204 - enableFindMeNumbers for user. User does not exist' 

    try:
        s.enableFindMeNumbers(token, 'twinkle-toes', True)
        print 'enableFindMeNumbers succeeded on bogus username value'
    except SOAPpy.Types.faultType, err:
        test = 0
        #print '\nFAILED: %s. Unexpected exception %s' % (testcase, err.faultstring)
        pass
          
    # Log test results
    logStatus(test, expected, testcase)

    numTestCases += 1    

##  TESTCASE-AS-SOAP-WS-API-1205 - enableFindMeNumbers for user. Username is null - FAIL
    test = 1
    expected = 'FAIL'
    testcase = 'TESTCASE-AS-SOAP-WS-API-1205 - enableFindMeNumbers for user. Username is null' 

    try:
        s.enableFindMeNumbers(token, None, True)
        print 'enableFindMeNumbers succeeded on NULL username value'
    except SOAPpy.Types.faultType, err:
        test = 0
        #print '\nFAILED: %s. Unexpected exception %s' % (testcase, err.faultstring)
        pass
          
    # Log test results
    logStatus(test, expected, testcase)

    numTestCases += 1      
    
##  TESTCASE-AS-SOAP-WS-API-1206 - enableFindMeNumbers. Invalid token - FAIL
    test = 1
    expected = 'FAIL'
    testcase = 'TESTCASE-AS-SOAP-WS-API-1206 - enableFindMeNumbers. Invalid token' 

    try:
        s.enableFindMeNumbers(token, None, True)
        print 'enableFindMeNumbers succeeded on bogus token value'
    except SOAPpy.Types.faultType, err:
        test = 0
        #print '\nFAILED: %s. Unexpected exception %s' % (testcase, err.faultstring)
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

            print "\nStarting enableFindMeNumber test suite against %s\n" % args[0]
                            
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