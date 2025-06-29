import SOAPpy
import os
import string

if __name__=='__main__':

    s = SOAPpy.WSDL.Proxy('http://eta/appsuiteadmin/soap.php?wsdl')
    
    token = s.login('Administrator','metreos')

    numTestCases = 1
    numUsers = 1
    
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

##    test = s.getUserList(token)
##    print '\nUsers: ', repr(test)

##    print '\nTestcase enableFindMeNumbers tuser1 true'
##    test = s.enableFindMeNumbers(token, 'tuser1', True)
##    print 'rc = %s' % test
##
##    print '\nTestcase enableFindMeNumbers tuser1 false'
##    test = s.enableFindMeNumbers(token, 'tuser1', False)
##    print 'rc = %s' % test
##

##    print '\nTestcase enableFindMeNumbers invalid value for respectVoicemail'
##    test = s.enableFindMeNumbers(token, 'tuser1', 3)
##    print 'rc = %s' % test

##    print '\nTestcase enableFindMeNumbers Null value for respectVoicemail'
##    test = s.enableFindMeNumbers(token, 'tuser1', None)
##    print 'rc = %s' % test

##    print '\nTestcase enableFindMeNumbers Null value for user'
##    test = s.enableFindMeNumbers(token, None, False)
##    print 'rc = %s' % test

##    print '\nTestcase enableFindMeNumbers user does not exist'
##    test = s.enableFindMeNumbers(token, 'jcapps', False)
##    print 'rc = %s' % test

##    print '\nTestcase enableFindMeNumbers no find me number for user'
##    test = s.enableFindMeNumbers(token, 'tuser2', False)
##    print 'rc = %s' % test
    
##    print '\nTestcase disableFindMeNumbers tuser1 - true'
##    test = s.disableFindMeNumbers(token, 'tuser1', True)
##    print 'rc = %s' % test
##
##    print '\nTestcase disableFindMeNumbers tuser1 - false'
##    test = s.disableFindMeNumbers(token, 'tuser1', False)
##    print 'rc = %s' % test
##
##    print '\nTestcase disableFindMeNumbers invalid value for respectVoicemail'
##    test = s.disableFindMeNumbers(token, 'tuser1', 3)
##    print 'rc = %s' % test
##    
##    print '\nTestcase disableFindMeNumbers Null value for respectVoicemail'
##    test = s.disableFindMeNumbers(token, 'tuser1', None)
##    print 'rc = %s' % test
##    
##    print '\nTestcase disableFindMeNumbers Null value for user'
##    test = s.disableFindMeNumbers(token, None, False)
##    print 'rc = %s' % test
##
##    print '\nTestcase disableFindMeNumbers user does not exist'
##    test = s.disableFindMeNumbers(token, 'jcapps', False)
##    print 'rc = %s' % test    
##    
    print '\nTestcase disableFindMeNumbers no find me number for user'
    test = s.disableFindMeNumbers(token, 'tuser2', False)
    print 'rc = %s' % test
    
####  Create users
##    print '\nTestcase: - addUser'
##    
##    print 'Creating %s users...' % numUsers
##    
##    for i in range(1,numUsers+1):
##        userRecord['firstName'] = 'Test%s' % i
##        userRecord['lastName'] = 'User%s' % i
##        userRecord['username'] = 'jcapps'
##        userRecord['password'] = 'metreos'
##        userRecord['pin'] = i
##        userRecord['accountCode'] = i
##        userRecord['email'] = 'tuser%s@metreos.com' % i
##        userRecord['status'] = None
##        userRecord['lockoutThreshold'] = 3
##        userRecord['lockoutDuration'] = 70
##        userRecord['maxSessions'] = 3
##        userRecord['pinChange'] = True
##        userRecord['record'] = True
##        userRecord['recordVisible'] = True
##        userRecord['timezone'] = 'Zulu'
##        userRecord['arTransferNumber'] = '2000'
##        
##    try:
##        test = s.addUser(token, userRecord)
##    except:
##        print '\nCaught exception in addUser: %s' % userRecord['username']

