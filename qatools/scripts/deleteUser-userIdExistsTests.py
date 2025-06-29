import SOAPpy
import os
import string

if __name__=='__main__':

## Test for appsuiteadmin WebServices API
## I need to log pass fail results to a file in the following format:
## TestcaseID   TestcaseDesc    Pass/Fail
## There are enough tests that the reading the results is error prone and tedious.    
    s = SOAPpy.WSDL.Proxy('http://vonnegut/appsuiteadmin/soap.php?wsdl')
    
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
    
    print '\nTestcase: - getUserList'
    test = s.getUserList(token)
    print 'Users: ', repr(test)
    numTestCases += 1
    
    print '\nTestcase: - deleteAllUsers'
    try:
        test = s.deleteAllUsers(token)
    except:
        print 'Unexpected exception in deleteAllUsers.'

    numTestCases += 1      

    test = s.getUserList(token)
    print '\nUsers: ', repr(test)

##  Create users
    print '\nTestcase: - addUser'
    
    print 'Creating %s users...' % numUsers
    
    for i in range(1,numUsers+1):
        userRecord['firstName'] = 'Test%s' % i
        userRecord['lastName'] = 'User%s' % i
        userRecord['username'] = 'jcapps'
        userRecord['password'] = 'metreos'
        userRecord['pin'] = i
        userRecord['accountCode'] = i
        userRecord['email'] = 'tuser%s@metreos.com' % i
        userRecord['status'] = None
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
        print '\nCaught exception in addUser: %s' % userRecord['username']

    userId = s.getUserId(token, 'jcapps')
    print 'getUserId. Id = %s' % repr(userId)

    print 'Test1 - userId[0]'
    try:
        test = s.userIdExists(token, userId[0])
    except:
        'Caught exception in userIdExists. userId: %s' % userId[0]
    pass

    print 'userId %s: %s' % (userId[0], repr(test))

    print 'Test2 - 5'
    test = s.userIdExists(token, 5)
    print 'userId 5: %s' % repr(test)

    print 'Test3 - hello'
    try:
        test = s.userIdExists(token, 'hello')
    except:
        'Caught exception in userIdExists - userId is hello'
        pass

    print 'userId hello: %s' % repr(test)

    print 'Test4 - None'
    try:
        test = s.userIdExists(token, None)
    except:
        'Caught exception in userIdExists - userId is None'
        pass

    print 'userId None: %s' % repr(test)

    print 'Test5 - 1'
    try:
        test = s.userIdExists(token, 1)
    except:
        'Caught exception in userIdExists. userId: %s' % userId[0]
    pass

    print 'userId %s: %s' % (userId[0], repr(test))


    print 'deleteUser jcapps'
    test = s.deleteUser(token, 'jcapps')
    
    test = s.getUserList(token)
    print '\nUsers: ', repr(test)    
    print repr(test)
    
    

    

    
