import SOAPpy
import os
import string

if __name__=='__main__':

## Test for appsuiteadmin WebServices API
## I need to log pass fail results to a file in the following format:
## TestcaseID   TestcaseDesc    Pass/Fail
## There are enough tests that the reading the results is error prone and tedious.    
##    s = SOAPpy.WSDL.Proxy('http://vonnegut/appsuiteadmin/soap.php?wsdl')

    s = SOAPpy.WSDL.eProxy('http://yourappliancenameorip/appsuiteadmin/soap.php?wsdl')
    
    token = s.login('Administrator','metreos')

    numTestCases = 1
    numUsers = 200
    
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
        userRecord['username'] = 'tuser%s' % i
        userRecord['password'] = 'metreos'
        userRecord['pin'] = i
        userRecord['accountCode'] = i
        userRecord['email'] = 'tuser%s@metreos.com' % i
        userRecord['status'] = None
        userRecord['lockoutThreshold'] = 3
        userRecord['lockoutDuration'] = 70
        userRecord['maxSessions'] = 3
        userRecord['pinChange'] = True
        userRecord['record'] = False
        userRecord['recordVisible'] = False
        userRecord['timezone'] = 'Zulu'
        userRecord['arTransferNumber'] = '2000'
        
        try:
            test = s.addUser(token, userRecord)
        except:
            print '\nCaught exception in addUser: %s' % userRecord['username']

    test = s.getUserList(token)       
    print '\nUsers: ', repr(test)
    numTestCases += 1  
## End Create Users
    
    print '\nTestcase: - getAccountCodeList'
    test = s.getAccountCodeList(token)
    print 'AccountCodes: ', repr(test)
    numTestCases += 1
    
    print '\nTestcase: - userExists - user exists'
    test = s.userExists(token,'tuser5')
    print  'User: tuser5 ', repr(test)
    numTestCases += 1

    print '\nTestcase: - userExists - user does not exist'
    test = s.userExists(token,'notexist')
    print 'User: notexist', repr(test)
    numTestCases += 1

    print '\nTestcase: - accountCodeExists - AC exists'
    test = s.accountCodeExists(token, 7)
    print '7: ', repr(test)
    numTestCases += 1
    
    print '\nTestcase: - accountCodeExists - AC does not exist'
    ac = numUsers+1
    test = s.accountCodeExists(token, ac)
    print 'User: ac', repr(test)
    numTestCases += 1        

    print '\nTestcase: - accountCodeExists - Null account code'  
    try:
        test = s.accountCodeExists(token,'')
    except:
        print 'Caught null account code exception'
        pass
    print 'Account Code: None'
    numTestCases += 1       

    print '\nTestcase: - accountCodeExists - invalid account code'
    try:
        test = s.accountCodeExists(token,'hello')   
    except:
        print 'Caught invalid account code exception'
        pass
    print 'Account Code: hello'    
    numTestCases += 1        

    print '\nTestcase: - addUser - duplicate username' 
    try:
        test = s.addUser(token, 'Test1', 'User1001', 'tuser1001', 'metreos', 1001, 1001)
    except:
        print 'Caught duplicate username exception'
        pass
    print 'User: tuser1'
    numTestCases += 1
    
    print '\nTestcase: - addUser - duplicate account code'
    try:
        test = s.addUser(token, 'Test1002', 'User1002', 'tuser1002', 'metreos', 1001, 12)
        test = s.getUserList(token)
        print '\nUsers: ', repr(test)        
    except:
        print 'Caught duplicate account code exception'
        pass
    print 'Account Code: 12'
    numTestCases += 1        

    print '\nTestcase: - deleteUser - user exists'
    user = 'tuser%s' % numTestCases
    try:
        test = s.deleteUser(token, user)
    except:
        'Unexpected exception in deleteUser'
        pass
    
    print '%s: ' % user, repr(test)
    numTestCases += 1
    
    test = s.getUserList(token)
    print '\nUsers: ', repr(test)

    print '\nTestcase: - deleteUser - user does not exist'   
    try:
        test = s.deleteUser(token, 'notexist')
        print 'User: notexist'
    except:
        print 'Caught a deleteUser exception: user does not exist'
        pass
    numTestCases += 1   

    print '\nTestcase: - getActiveSessions - user exists'
    user = 'tuser%s' % numTestCases
    test = s.getActiveSessions(token, user)
    print '%s: ' % user, repr(test)
    numTestCases += 1  

    print '\nTestcase: - getActiveSessions - user does not exist'
    try:
        test = s.getActiveSessions(token, 'notexist')
    except:
        print 'Caught exception in getActiveSessions - user does not exist'
        pass
    print 'User: notexist'
    numTestCases += 1

    print '\nTestcase: - getCreateDate - user exists'
    user = 'tuser%s' % numTestCases
    test = s.getCreateDate(token, user)
    print '%s: ' % user, repr(test)
    numTestCases += 1

## This testcase is failing.  I can't figure out why.  For some reason rc['rc']
## is not set after call to userExists in WebService.getCreateDate.  An error is
## getting logged to php.log but an exception is not being thrown.  I have spent
## too much time on this and am moving on.  I will come back to it later when I
## can look at it with fresh eyes.
    
    print '\nTestcase: - getCreateDate - user does not exist'
    try:
        test = s.getCreateDate(token, 'notexist')
    except:
        'Caught exception in getCreateDate - user does not exist'
        pass
    print 'User: notexist'
    numTestCases += 1

    print '\nTestcase:UserClass%s - getExternalAuthEnabled - user exists'
    user = 'tuser%s' % numTestCases
    test = s.getExternalAuthEnabled(token, user)
    print '%s' % user, repr(test)
    numTestCases += 1

    print '\nTestcase: - getExternalAuthEnabled  - user does not exist'
    try:
        test = s.getExternalAuthEnabled(token, 'notexist')
    except:
        print 'Caught exception in getExternalAuthEnabled - user does not exist'
        pass
    print 'User: notexist'
    numTestCases += 1 

    print '\nTestcase: - getExternalAuthDn - user exists'
    user = 'tuser%s' % numTestCases
    test = s.getExternalAuthDn(token, user)
    print '%s' % user, repr(test)
    numTestCases += 1

    print '\nTestcase: - getExternalAuthDn - user does not exist'
    try:
        test = s.getExternalAuthDn(token, 'notexist')
    except:
        print 'Caught exception in getExternalAuthDn - user does not exist'
        pass
    print 'User: notexist'
    numTestCases += 1 

    print '\nTestcase: - getNumFailedLogins - user exists'
    user = 'tuser%s' % numTestCases
    test = s.getNumFailedLogins(token, user)
    print '%s' % user, repr(test)
    numTestCases += 1

    print '\nTestcase: - getNumFailedLogins - user does not exist'
    try:
        test = s.getNumFailedLogins(token, 'notexist')
    except:
        print 'Caught exception in getNumFailedLogins - user does not exist'
        pass
    print 'User: notexist'
    numTestCases += 1 

    print '\nTestcase: - getLastLockout - user exists'
    user = 'tuser%s' % numTestCases
    test = s.getLastLockout(token, user)    
    print '%s' % user, repr(test)
    numTestCases += 1

    print '\nTestcase: - getLastLockout - user does not exist'
    try:
        test = s.getLastLockout(token, 'notexist')
    except:
        print 'Caught exception in getLastLockout - user does not exist'
        pass
    print 'User: notexist'
    numTestCases += 1  

    print '\nTestcase: - getLastUsed - user exists'
    user = 'tuser%s' % numTestCases
    test = s.getLastUsed(token, user) 
    print '%s' % user, repr(test)
    numTestCases += 1
    
    print '\nTestcase: - getLastUsed - user does not exist'
    try:
        test = s.getLastUsed(token, 'notexist')
    except:
        print 'Caught exception in getLastUsed - user does not exist'
        pass
    print 'User: notexist'
    numTestCases += 1

    print '\nTestcase: - getLdapServerId - user exists'
    user = 'tuser%s' % numTestCases
    test = s.getLdapServerId(token, user)
    print '%s' % user, repr(test)
    numTestCases += 1

    print '\nTestcase: - getLdapServerId - user does not exist'
    try:
        test = s.getLdapServerId(token, 'notexist')
    except:
        print 'Caught exception in getLdapServerId - user does not exist'
        pass
    print 'User: notexist'
    numTestCases += 1

    print '\nTestcase: - getLdapSynched - user exists'
    user = 'tuser%s' % numTestCases
    test = s.getLdapSynched(token, user)    
    print '%s' % user, repr(test)
    numTestCases += 1

    print '\nTestcase: - getLdapSynched - user does not exist'
    try:
        test = s.getLdapSynched(token, 'notexist')        
    except:
        print 'Caught exception in getLdapSynched - user does not exist'
        pass
    print 'User: notexist'
    numTestCases += 1

    print '\nTestcase: - getUserId - user exists'
    user = 'tuser%s' % numTestCases
    test = s.getUserId(token, user)
    print '%s' % user, repr(test)
    numTestCases += 1
    
    print '\nTestcase: - getUserId - user does not exist'
    try:
        test = s.getUserId(token, 'notexist')
    except:
        print 'Caught exception in getUserId - user does not exist'
        pass
    print 'User: notexist'
    numTestCases += 1
    
    print '\nTestcase: - getUsername - user exists'
    user = 'tuser%s' % numTestCases
    test = s.getUsername(token, user)
    print '%s' % user, repr(test)
    numTestCases += 1

    print '\nTestcase: - getUsername - user does not exist'
    try:
        test = s.getUsername(token, 'notexist')
    except:
        print 'Caught exception in getUsername - user does not exist'
        pass
    print 'User: notexist'
    numTestCases += 1

    print '\nTestcase: - getAccountCode - user exists'
    user = 'tuser%s' % numTestCases
    test = s.getAccountCode(token, user)    
    print '%s' % user, repr(test)
    numTestCases += 1

    print '\nTestcase: - getAccountCode - user does not exist'
    try:
        test = s.getAccountCode(token, 'notexist')
    except:
        print 'Caught exception in getAccountCode - user does not exist'
        pass
    print 'User: notexist'
    numTestCases += 1
    
    print '\nTestcase: - getPassword - user exists'
    user = 'tuser%s' % numTestCases
    test = s.getPassword(token, user) 
    print '%s' % user, repr(test)
    numTestCases += 1

    print '\nTestcase: - getPassword - user does not exist'
    try:
        test = s.getPassword(token, 'notexist')    
    except:
        print 'Caught exception in getPassword - user does not exist'
        pass
    print 'User: notexist'
    numTestCases += 1

    print '\nTestcase: - getFirstName - user exists'
    user = 'tuser%s' % numTestCases
    test = s.getFirstName(token, user)    
    print '%s' % user, repr(test)
    numTestCases += 1

    print '\nTestcase: - getFirstName - user does not exist'
    try:
        test = s.getFirstName(token, 'notexist') 
    except:
        print 'Caught exception in getFirstName - user does not exist'
        pass
    print 'User: notexist'
    numTestCases += 1

    print '\nTestcase: - getLastName - user exists'
    user = 'tuser%s' % numTestCases
    test = s.getLastName(token, user)
    print '%s' % user, repr(test)
    numTestCases += 1

    print '\nTestcase: - getLastName - user does not exist'
    try:
        test = s.getLastName(token, 'notexist')
    except:
        print 'Caught exception in getLastName - user does not exist'
        pass
    print 'User: notexist'
    numTestCases += 1

    print '\nTestcase: - getEmail - user exists'
    user = 'tuser%s' % numTestCases
    test = s.getEmail(token, user)
    print '%s' % user, repr(test)
    numTestCases += 1

    print '\nTestcase: - getEmail - user does not exist'
    try:
        test = s.getEmail(token, 'notexist')
    except:
        print 'Caught exception in getEmail - user does not exist'
        pass
    print 'User: notexist'
    numTestCases += 1

    print '\nTestcase: - getLockoutThreshold - user exists'
    user = 'tuser%s' % numTestCases
    test = s.getLockoutThreshold(token, user)
    print '%s' % user, repr(test)
    numTestCases += 1
    
    print '\nTestcase: - getLockoutThreshold - user does not exist'
    try:
        test = s.getLockoutThreshold(token, 'notexist')
    except:
        print 'Caught exception in getLockoutThreshold - user does not exist'
        pass
    print 'User: notexist'
    numTestCases += 1

    print '\nTestcase: - getLockoutDuration - user exists'
    user = 'tuser%s' % numTestCases
    test = s.getLockoutDuration(token, user)
    print '%s' % user, repr(test)
    numTestCases += 1
       
    print '\nTestcase: - getLockoutDuration - user does not exist'
    try:
        test = s.getLockoutDuration(token, 'notexist')
    except:
        print 'Caught exception in getLockoutDuration - user does not exist'
        pass
    print 'User: notexist'
    numTestCases += 1

    print '\nTestcase: - getMaxSessions - user exists'
    user = 'tuser%s' % numTestCases
    test = s.getMaxSessions(token, user)
    print '%s' % user, repr(test)
    numTestCases += 1
    
    print '\nTestcase: - getMaxSessions - user does not exist'
    try:
        test = s.getMaxSessions(token, 'notexist')
    except:
        print 'Caught exception in getMaxSessions - user does not exist'
        pass
    print 'User: notexist'
    numTestCases += 1

    print '\nTestcase: - getPinChange - user exists'
    user = 'tuser%s' % numTestCases
    test = s.getPinChange(token, user)
    print '%s' % user, repr(test)
    numTestCases += 1

    print '\nTestcase: - getPinChange - user does not exist'
    try:
        test = s.getPinChange(token, 'notexist')
    except:
        print 'Caught exception in getPinChange - user does not exist'
        pass
    print 'User: notexist'
    numTestCases += 1

    print '\nTestcase: - getRecord - user exists'
    user = 'tuser%s' % numTestCases
    test = s.getRecord(token, user)
    print '%s' % user, repr(test)
    numTestCases += 1
    
    print '\nTestcase: - getRecord - user does not exist'
    try:
        test = s.getRecord(token, 'notexist')
    except:
        print 'Caught exception in getRecord - user does not exist'
        pass
    print 'User: notexist'
    numTestCases += 1

    print '\nTestcase: - getRecordVisible - user exists'
    user = 'tuser%s' % numTestCases
    test = s.getRecordVisible(token, user)
    print '%s' % user, repr(test)
    numTestCases += 1

    print '\nTestcase: - getRecordVisible - user does not exist'
    try:
        test = s.getRecordVisible(token, 'notexist')
    except:
        print 'Caught exception in getRecordVisible - user does not exist'
        pass
    print 'User: notexist'
    numTestCases += 1

    print '\nTestcase: - getArTransferNumber - user exists'
    user = 'tuser%s' % numTestCases
    test = s.getArTransferNumber(token, user)
    print '%s' % user, repr(test)
    numTestCases += 1
           
    print '\nTestcase: - getArTransferNumber - user does not exist'
    try:
        test = s.getArTransferNumber(token, 'notexist')
    except:
        print 'Caught exception in getArTransferNumber - user does not exist'
        pass
    print 'User: notexist'
    numTestCases += 1

    print '\nTestcase: - getTimezone - user exists'
    user = 'tuser%s' % numTestCases
    test = s.getTimezone(token, user)
    print '%s' % user, repr(test)
    numTestCases += 1
      
    print '\nTestcase: - getTimezone - user does not exist'
    try:
        test = s.getTimezone(token, 'notexist')
    except:
        print 'Caught exception in getTimezone - user does not exist'
        pass
    print 'User: notexist'
    numTestCases += 1

    test = None
    print '\nTestcase: - userIdExists - userId exists'
    user = 'tuser%s' % numTestCases
    userId = s.getUserId(token, user)
    try:
        test = s.userIdExists(token, userId[0])
    except:
        'Caught unexpected exception in userIdExists'
        pass
    
    print 'User: %s UserId: %s' % (user, userId[0]), repr(test)
    numTestCases += 1

## This testcase is failing.  The return code is not getting set correctly and I can't
## tell why.  I need to move on.
    test = None
    print '\nTestcase: - userIdExists - userId does not exist'
    try:
        test = s.userIdExists(token, 5000)
        print 'rc = %s' % repr(test)
    except:
        print 'Caught exception in userIdExists - user does not exist'
        pass
    print 'UserId: 5000'
    numTestCases += 1

## This testcase is failing.  I can't get my verification to work
    test = None
    print '\nTestcase: - userIdExists - userId is a string'
    try:
        test = s.userIdExists(token, 'hello')
        print 'rc = %s', repr(test)
    except:
        print 'Caught exception in userIdExists - userId is a string'
        pass
    print 'UserId: hello'
    numTestCases += 1

## This testcase is failing.  I can't get my verification to work   
    print '\nTestcase: - userIdExists - userId is None'
    try:
        test = s.userIdExists(token, None)
    except:
        print 'Caught exception in userIdExists - userId is None'
        pass
    print 'UserId: None'
    numTestCases += 1    

    print '\nTestcase: - setUsername - userId exists'
    user = 'tuser%s' % numTestCases    
    userId = s.getUserId(token, user)
    test = s.setUsername(token, userId[0], 'newusername')
    print '%s' % user, repr(test)
    numTestCases += 1

    print '\nTestcase: - setUsername - userId does not exist'
    user = 'tuser%s' % numTestCases 
    try:
        test = s.setUsername(token, 5000, 'newusername')
    except:
        print 'Caught exception in setUsername - userId does not exist'
        pass
    print 'User: %s  UserId: %s' % (user, 5000)
    numTestCases += 1

    print '\nTestcase: - changeUsername - user exists'
    user = 'tuser%s' % numTestCases
    test = s.changeUsername(token, user, 'newusername')
    print 'Change username from %s to %s' % (user, 'newusername'), repr(test)
    numTestCases += 1

    print '\nTestcase: - changeUsername - user does not exist'
    user = 'tuser%s' % numTestCases  
    try:
        test = s.changeUsername(token, 'notexist', user)
    except:
        print 'Caught exception in setUsername - user does not exist'
        pass
    print 'Change username from %s to %s' % (user, 'notexist')
    numTestCases += 1

    print '\nTestcase: - changeUsername - user is administrator'
    user = 'tuser%s' % numTestCases  
    try:
        test = s.changeUsername(token, user, 'administrator')
    except:
        print 'Caught exception in setUsername - user is administrator'
        pass
    print 'Change username from %s to %s' % (user,'administrator')
    numTestCases += 1

    print '\nTestcase: - changeUsername - user is ADMINISTRATOR'
    user = 'tuser%s' % numTestCases  
    try:
        test = s.changeUsername(token, user, 'ADMINISTRATOR')
    except:
        print 'Caught exception in setUsername - user is ADMINISTRATOR'
        pass
    print 'Change username from %s to %s' % (user, 'ADMINISTRATOR')
    numTestCases += 1
    
    print '\nTestcase: - setPin - valid values'
    user = 'tuser%s' % numTestCases
    test = s.setPin(token, user, 5000)
    print 'Change pin from %s to %s' % (numTestCases, 5000), repr(test)
    numTestCases += 1

    print '\nTestcase: - setPin - user does not exist'
    try:
        test = s.setPin(token, 'notexist', 5000)
    except:
        print 'Caught exception in setPin - user does not exist'
        pass
    print 'Change pin from %s to %s' % (numTestCases, 5000)
    numTestCases += 1

    print '\nTestcase: - setPin - pin is null' 
    user = 'tuser%s' % numTestCases
    try:
        test = s.setPin(token, user, None)
    except:
        print 'Caught exception in setPin - pin is null'
    print 'Change pin from %s to %s' % (numTestCases, None)
    numTestCases += 1
    
    print '\nTestcase: - setPin - pin is string'
    user = 'tuser%s' % numTestCases
    try:
        test = s.setPin(token, user, 'hello')
    except:
        print 'Caught exception in setPin - pin is a string'
    print 'Change pin from %s to %s' % (numTestCases, 'hello')
    numTestCases += 1

    print '\nTestcase: - setAccountCode - valid values'
    user = 'tuser%s' % numTestCases
    test = s.setAccountCode(token, user, 5000)
    print 'Change account code for user %s to %s' % (user, 5000), repr(test)
    numTestCases += 1
    
    print '\nTestcase: - setAccountCode - user does not exist'
    try:
        test = s.setAccountCode(token, 'notexist', numTestCases)
    except:
        print 'Caught exception in setAccountCode - user does not exist'
        pass
    print 'Change account code for user: %s to %s' % ('notexist', 5000)
    numTestCases += 1
    
    print '\nTestcase: - setAccountCode - account code is null'
    user = 'tuser%s' % numTestCases
    try:
        test = s.setAccountCode(token, user, None)
    except:
        print 'Caught exception in setAccountCode - account code is null'
    print 'Change account code for user: %s to %s' % (user, None)
    numTestCases += 1

    print '\nTestcase: - setAccountCode - account code is a string'
    user = 'tuser%s' % numTestCases
    try:
        test = s.setAccountCode(token, user, 'hello')
    except:
        print 'Caught exception in setAccountCode - account code is a string'
    print 'Change account code for user: %s to %s' % (user, 'hello')
    numTestCases += 1

    print '\nTestcase: - setPassword - valid values'
    user = 'tuser%s' % numTestCases
    test = s.setPassword(token, user, user)
    print 'Change password for user %s from metreos to %s' % (user, user), repr(test)
    numTestCases += 1

    print '\nTestcase: - setPassword - user does not exist'
    user = 'tuser%s' % numTestCases
    try:
        test = s.setPassword(token, 'notexist', user)
    except:
        print 'Caught exception in setPassword - user does not exist'
    print 'Change password for user: %s from metreos to %s' % ('notexist', user)
    numTestCases += 1

    print '\nTestcase: - setPassword - password is null'
    user = 'tuser%s' % numTestCases
    try:
        test = s.setPassword(token, user, None)
    except:
        print 'Caught exception in setPassword - password is null'
        pass
    print 'Change password for user %s from metreos to %s' % (user, None)
    numTestCases += 1

    print '\nTestcase: - setFirstName - valid values'
    user = 'tuser%s' % numTestCases
    test = s.setFirstName(token, user, 'newfirstname')
    print 'Set first name for user %s to %s' % (user, 'newfirstname'), repr(test)
    numTestCases += 1

    print '\nTestcase: - setFirstName - user does not exist'
    user = 'tuser%s' % numTestCases
    try:
        test = s.setFirstName(token, 'notexist', 'newfirstname')
    except:
        print 'Caught exception in setFirstName - user does not exist'
        pass
    print 'Change password for user %s to %s' % ('notexist','newfirstname')
    numTestCases += 1
    

    print '\nTestcase: - setFirstName - first name is null'
    user = 'tuser%s' % numTestCases
    try:
        test = s.setFirstName(token, user, None)
    except:
        print 'Caught exception in setFirstName - first name is null'
        pass
    print 'Change first name for user %s to %s' % (user, None)
    numTestCases += 1
    
    print '\nTestcase: - setLastName - valid values'
    user = 'tuser%s' % numTestCases
    test = s.setLastName(token, user, 'newlastname')
    print 'Set last name for user %s to %s' % (user, 'newlastname'), repr(test)
    numTestCases += 1

    print '\nTestcase: - setLastName - user does not exist'
    user = 'tuser%s' % numTestCases
    try:
        test = s.setLastName(token, 'notexist', 'newlastname')
    except:
        print 'Caught exception in setLastName - user does not exist'
        pass
    print 'Change last name for user %s to %s' % ('notexist', 'newlastname')
    numTestCases += 1

    print '\nTestcase: - setLastName - last name is null'
    user = 'tuser%s' % numTestCases
    try:
        test = s.setLastName(token, user, None)
    except:
        print 'Caught exception in setLastName - last name is null'
        pass
    print 'Change last name for user %s to %s' % (user, None)
    numTestCases += 1

    print '\nTestcase: - setEmail - valid values'
    user = 'tuser%s' % numTestCases
    email = 'newemail@metreos.com'
    test = s.setEmail(token, user, email)
    print 'Set email for user %s from %s@metreos.com to %s' % (user, user, email), repr(test)
    numTestCases += 1
    
    print '\nTestcase: - setEmail - user does not exist'
    user = 'tuser%s' % numTestCases
    email = 'newemail@metreos.com'
    try:
        test = s.setEmail(token, 'notexist', email)
    except:
        print 'Caught exception in setEmail - user does not exist'
        pass
    print 'Set email for user %s from %s@metreos.com to %s' % ('notexist', user, email), repr(test)
    numTestCases += 1

    print '\nTestcase: - setEmail - email is null'
    user = 'tuser%s' % numTestCases
    try:
        test = s.setEmail(token, user, None)
    except:
        print 'Caught unexpected exception in setEmail - email is null but that is ok'
        pass
    print 'Set email for user %s from %s@metreos.com to %s' % (user, user, None), repr(test)
    numTestCases += 1

    print '\nTestcase: - setLockoutDuration - valid values'
    user = 'tuser%s' % numTestCases
    test = s.setLockoutDuration(token, user, numTestCases)
    print 'Set lockout duration for user %s to %s' % (user, numTestCases), repr(test)
    numTestCases += 1

    print '\nTestcase: - setLockoutDuration - user does not exist'
    user = 'tuser%s' % numTestCases
    try:
        test = s.setLockoutDuration(token, 'notexist', numTestCases)
    except:
        print 'Caught exception in setLockoutDuration - user does not exist'
        pass
    print 'Change lockout duration for user %s to %s' % (user, numTestCases)
    numTestCases += 1    

    print '\nTestcase: - setLockoutDuration - lockout duration is a string'
    user = 'tuser%s' % numTestCases
    try:
        test = s.setLockoutDuration(token, user, 'hello')
    except:
        print 'Caught exception in setLockoutDuration - lockout duration is a string'
        pass
    print 'Change lockout duration for user %s to %s' % (user, 'hello')
    numTestCases += 1    
    
    print '\nTestcase: - setLockoutDuration - lockout duration is null'
    user = 'tuser%s' % numTestCases
    try:
        test = s.setLockoutDuration(token, user, None)
    except:
        print 'Caught exception in setLockoutDuration - lockout duration is null'
        pass
    print 'Change lockout duration for user %s to %s' % (user, None)
    numTestCases += 1
    
    print '\nTestcase: - setLockoutThreshold - valid values'
    user = 'tuser%s' % numTestCases
    test = s.setLockoutThreshold(token, user, numTestCases)
    print 'Set lockout duration for user %s to %s' % (user, numTestCases), repr(test)
    numTestCases += 1

    print '\nTestcase: - setLockoutThreshold - lockout threshold is null'
    user = 'tuser%s' % numTestCases
    try:
        test = s.setLockoutThreshold(token, user, None)
    except:
        print 'Caught exception in setLockoutThreshold - lockout threshold is null'
        pass
    print 'Change lockout threshold for user %s to %s' % (user, None)
    numTestCases += 1

    print '\nTestcase: - setLockoutThreshold - user does not exist' 
    try:
        test = s.setLockoutThreshold(token, 'notexist', numTestCases)
    except:
        print 'Caught exception in setLockoutThreshold - the user does not exist'
        pass
    print 'Change lockout threshold for user %s to %s' % ('notexist', numTestCases)
    numTestCases += 1

    print '\nTestcase: - setLockoutThreshold - lockout threshold is a string' 
    user = 'tuser%s' % numTestCases
    try:
        test = s.setLockoutThreshold(token, user, 'hello')
    except:
        print 'Caught exception in setLockoutThreshold - lockout threshold is a string'
        pass
    print 'Change lockout threshold for user %s to %s' % (user, 'hello')
    numTestCases += 1
    
    print '\nTestcase: - setLockoutThreshold - lockout threshold is null'
    user = 'tuser%s' % numTestCases
    try:
        test = s.setLockoutThreshold(token, user, None)
    except:
        print 'Caught exception in setLockoutThreshold - lockout threshold is null'
        pass
    print 'Change lockout threshold for user %s to %s' % (user, None)
    numTestCases += 1

    print '\nTestcase: - setTimezone - user exists'
    user = 'tuser%s' % numTestCases
    test = s.setTimezone(token, user, 'Zulu')
    print 'User: %s Timezone: %s' % (user, 'Zulu'), repr(test)
    numTestCases += 1
      
    print '\nTestcase: - setTimezone - user does not exist'
    try:
        test = s.setTimezone(token, 'notexist', 'Zulu')
    except:
        print 'Caught exception in setTimezone - user does not exist'
        pass
    print 'User: notexist'
    numTestCases += 1
  
    print '\nTestcase: - getAddUserRequiredCols'
    test = s.getAddUserRequiredCols(token)
    print repr(test)
    numTestCases += 1

    print '\nTestcase: - getAddUserCols'
    test = s.getAddUserCols(token)
    print repr(test)
    numTestCases += 1

    print '\nTestcase: - setRecord - True'
    user = 'tuser%s' % numTestCases
    try:
        test = s.setRecord(token, user, True)
        print repr(test)        
    except:
        print 'Caught exception in setRecord'       
    numTestCases += 1
    
    print '\nTestcase: - setRecord - 1'
    user = 'tuser%s' % numTestCases
    try:
        test = s.setRecord(token, user, 1)
        print repr(test)
    except:
        print 'Caught exception in setRecord'
    numTestCases += 1

    print '\nTestcase: - setRecord = 0'
    user = 'tuser%s' % numTestCases    
    try:
        test = s.setRecord(token, user, 0)
        print repr(test)
    except:
        print 'Caught exception in setRecord'
    numTestCases += 1
   
    print '\nTestcase: - setRecord - False'
    user = 'tuser%s' % numTestCases
    try:
        test = s.setRecord(token, user, False)
        print repr(test)
    except:
        print 'Caught exception in setRecord'
    numTestCases += 1

    print '\nTestcase: - setRecord - invalid value'
    user = 'tuser%s' % numTestCases
    try:
        test = s.setRecord(token, user, 3)
        print repr(test)
    except:
        print 'Caught exception in setRecord'
    numTestCases += 1
##
    print '\nTestcase: - setRecordVisible - True'
    user = 'tuser%s' % numTestCases
    try:
        test = s.setRecordVisible(token, user, True)
        print repr(test)        
    except:
        print 'Caught exception in setRecordVisible'
        
    numTestCases += 1
    
    print '\nTestcase: - setRecordVisible - 1'
    user = 'tuser%s' % numTestCases
    try:
        test = s.setRecordVisible(token, user, 1)
        print repr(test)
    except:
        print 'Caught exception in setRecordVisible'
    numTestCases += 1

    print '\nTestcase: - setRecordVisible = 0'
    user = 'tuser%s' % numTestCases    
    try:
        test = s.setRecordVisible(token, user, 0)
        print repr(test)
    except:
        print 'Caught exception in setRecordVisible'
    numTestCases += 1
   
    print '\nTestcase: - setRecordVisible - False'
    user = 'tuser%s' % numTestCases
    try:
        test = s.setRecordVisible(token, user, False)
        print repr(test)
    except:
        print 'Caught exception in setRecordVisible'
    numTestCases += 1

    print '\nTestcase: - setRecordVisible - invalid value'
    user = 'tuser%s' % numTestCases
    try:
        test = s.setRecordVisible(token, user, 3)
        print repr(test)
    except:
        print 'Caught exception in setRecordVisible'
    numTestCases += 1

##
    print '\nTestcase: - setPinChange - True'
    user = 'tuser%s' % numTestCases
    try:
        test = s.setPinChange(token, user, True)
        print repr(test)
        print user
    except:
        print 'Caught exception in setPinChange'
        
    numTestCases += 1
    
    print '\nTestcase: - setPinChange - 1'
    user = 'tuser%s' % numTestCases
    try:
        test = s.setPinChange(token, user, 1)
        print repr(test)
        print user
    except:
        print 'Caught exception in setPinChange'
    numTestCases += 1
   
    print '\nTestcase: - setPinChange = 0'
    user = 'tuser%s' % numTestCases    
    try:
        test = s.setPinChange(token, user, 0)
        print repr(test)
        print user
    except:
        print 'Caught exception in setPinChange'
    numTestCases += 1
   
    print '\nTestcase: - setPinChange - False'
    user = 'tuser%s' % numTestCases
    try:
        test = s.setPinChange(token, user, False)
        print repr(test)
        print user
    except:
        print 'Caught exception in setPinChange'
    numTestCases += 1

    print '\nTestcase: - setPinChange - invalid value'
    user = 'tuser%s' % numTestCases
    try:
        test = s.setPinChange(token, user, 3)
        print repr(test)
        print user
    except:
        print 'Caught exception in setPinChange'
    numTestCases += 1
