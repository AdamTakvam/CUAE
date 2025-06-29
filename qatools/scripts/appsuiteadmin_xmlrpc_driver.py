import xmlrpclib
import os

if __name__=='__main__':

    s = xmlrpclib.Server('http://10.1.12.227/appsuiteadmin/xml-rpc.php')

    token = s.login('Administrator','metreos')

##    print 'Testcase: UserClass1 - getUserList'
##    test = s.getUserList(token)
##    print '\nUsers: ', test['result']
##
###    print '\nTestcase: UserClass2 - deleteAllUsers'
###    test = s.deleteAllUsers(token);
##    
##    test = s.getUserList(token)
##    print '\nUsers: ', test['result']
##    
##    print '\nTest: UserClass3 - addUser'
##    print '\nAdding tuser1'
##    test = s.addUser(token, 'Test1', 'User1', 'tuser1', 'metreos', 1111, 1111)
##    print '\nAdding tuser2'
##    test = s.addUser(token, 'Test2', 'User2', 'tuser2', 'metreos', 2222, 2222)
##    print '\nAdding tuser3'
##    test = s.addUser(token, 'Test3', 'User3', 'tuser3', 'metreos', 3333, 3333)
##    print '\nAdding tuser4'
##    test = s.addUser(token, 'Test4', 'User4', 'tuser4', 'metreos', 4444, 4444)
##
##    test = s.getUserList(token)
##    print '\nUsers: ', test['result']
##
##    print '\nTestcase: UserClass4 - getAccountCodeList'
##    test = s.getAccountCodeList(token)
##    print '\nAccountCodes: ', test['result']
##    print
##
##    print '\nTestcase: UserClass5 - userExists - user exists'
##    test = s.userExists(token,'tuser1')
##    print  '\ntuser1 ', repr(test)
##    
##    print '\nTestcase: UserClass6 - userExists - user does not exist'
##    test = s.userExists(token,'notexist')
##    print '\nnotexist ', repr(test)
##    print
##
##    print '\nTestcase: UserClass7 - accountCodeExists - AC exists'
##    test = s.accountCodeExists(token,1111)
##    print '\n1111 ', repr(test)
####    if test['rc'] == False:
####        print test['error']
####        print
####    else:
####        print repr(test['result'])
####        print
##
##    print '\nTestcase: UserClass8 - accountCodeExists - AC does not exist'
##    test = s.accountCodeExists(token,9999)
##    print '\n9999 ', repr(test)   
####    if not test['rc']:
####        print test['error']
####        print
####    else:
####        print repr(test['result'])
####        print
##        
##    print '\nTestcase: UserClass9 - accountCodeExists - Null account code'
##    test = s.accountCodeExists(token,'')
##    print '\nblank ', repr(test)
##
####    if not test['rc']:
####        print test['error']
####        print
####    else:
####        print repr(test['result'])
####        print
##
##    print '\nTestcase: UserClass10 - accountCodeExists - invalid account code'
##    test = s.accountCodeExists(token,'hello')
##    print '\nhello ', repr(test)
####    if not test['rc']:
####        print test['error']
####        print
####    else:
####        print repr(test['result'])
####        print
##    
##    print '\nTestcase: UserClass11 - addUser - duplicate username'
##    test = s.addUser(token, 'Test5', 'User5', 'tuser4', 'metreos', 5555, 5555)
##    print '\ntuser4 ', repr(test)
##
##    print '\nTestcase: UserClass12 - addUser - duplicate account code'
##    test = s.addUser(token, 'Test6', 'User6', 'tuser6', 'metreos', 6666, 4444)
##    print '\n4444 ', repr(test)
##
##    test = s.getUserList(token)
##    print '\nUsers: ', test['result']
##    
##    print '\nTestcase: UserClass13 - deleteUser - user exists'
##    test = s.deleteUser(token, 'tuser1')
##    print '\ntuser1', repr(test)
##
##    test = s.getUserList(token)
##    print '\nUsers: ', test['result']
##        
##    print '\nTestcase: UserClass14 - deleteUser - user does not exist'
##    test = s.deleteUser(token, 'notexist')
##    print '\nnotexist ', repr(test)
##    
##    test = s.getUserList(token)
##    print '\nUsers: ', test['result']
##
##    print '\nTestcase: UserClass15 - getActiveSessions - user exists'
##    test = s.getActiveSessions(token, 'tuser2')
##    print '\ntuser100: ', repr(test)
##        
##    print '\nTestcase: UserClass16 - getActiveSessions - user does not exist'
##    test = s.getActiveSessions(token, 'notexist')
##    print '\nnotexist ', repr(test)
##
##    print '\nTestcase: UserClass17 - getCreateDate - user exists'
##    test = s.getCreateDate(token, 'tuser100')
##    print '\ntuser100: ', repr(test)
##        
##    print '\nTestcase: UserClass18 - getCreateDate - user does not exist'
##    test = s.getCreateDate(token, 'notexist')
##    print '\nnotexist ', repr(test)
##
##    print '\nTestcase: UserClass19 - getExternalAuthEnabled - user exists'
##    test = s.getExternalAuthEnabled(token, 'tuser100')
##    print '\ntuser100: ', repr(test)
##        
##    print '\nTestcase: UserClass20 - getExternalAuthEnabled  - user does not exist'
##    test = s.getExternalAuthEnabled(token, 'notexist')
##    print '\nnotexist ', repr(test)
##
##    print '\nTestcase: UserClass21 - getExternalAuthDn - user exists'
##    test = s.getExternalAuthDn(token, 'tuser100')
##    print '\ntuser100: ', repr(test)
##        
##    print '\nTestcase: UserClass22 - getExternalAuthDn - user does not exist'
##    test = s.getExternalAuthDn(token, 'notexist')
##    print '\nnotexist ', repr(test)
##
##    print '\nTestcase: UserClass23 - getNumFailedLogins - user exists'
##    test = s.getNumFailedLogins(token, 'tuser100')
##    print '\ntuser100: ', repr(test)
##        
##    print '\nTestcase: UserClass24 - getNumFailedLogins - user does not exist'
##    test = s.getNumFailedLogins(token, 'notexist')
##    print '\nnotexist ', repr(test)
##  
##    print '\nTestcase: UserClass25 - getLastLockout - user exists'
##    test = s.getLastLockout(token, 'tuser100')
##    print '\ntuser100: ', repr(test)
##        
##    print '\nTestcase: UserClass26 - getLastLockout - user does not exist'
##    test = s.getLastLockout(token, 'notexist')
##    print '\nnotexist ', repr(test)
## 
##    print '\nTestcase: UserClass27 - getLastUsed - user exists'
##    test = s.getLastUsed(token, 'tuser100')
##    print '\ntuser100: ', repr(test)
##        
##    print '\nTestcase: UserClass28 - getLastUsed - user does not exist'
##    test = s.getLastUsed(token, 'notexist')
##    print '\nnotexist ', repr(test)
##  
##    print '\nTestcase: UserClass29 - getLdapServerId - user exists'
##    test = s.getLdapServerId(token, 'tuser100')
##    print '\ntuser100: ', repr(test)
##        
##    print '\nTestcase: UserClass30 - getLdapServerId - user does not exist'
##    test = s.getLdapServerId(token, 'notexist')
##    print '\nnotexist ', repr(test)
##
##    print '\nTestcase: UserClass31 - getLdapSynched - user exists'
##    test = s.getLdapSynched(token, 'tuser100')
##    print '\ntuser100: ', repr(test)
##        
##    print '\nTestcase: UserClass32 - getLdapSynched - user does not exist'
##    test = s.getLdapSynched(token, 'notexist')
##    print '\nnotexist ', repr(test)
##
##    print '\nTestcase: UserClass33 - getUserId - user exists'
##    test = s.getUserId(token, 'tuser100')
##    print '\ntuser100: ', repr(test)
##        
##    print '\nTestcase: UserClass34 - getUserId - user does not exist'
##    test = s.getUserId(token, 'notexist')
##    print '\nnotexist ', repr(test)
##
##    print '\nTestcase: UserClass35 - getUsername - user exists'
##    test = s.getUsername(token, 'tuser100')
##    print '\ntuser100: ', repr(test)
##        
##    print '\nTestcase: UserClass36 - getUsername - user does not exist'
##    test = s.getUsername(token, 'notexist')
##    print '\nnotexist ', repr(test)
##
##    print '\nTestcase: UserClass37 - getAccountCode - user exists'
##    test = s.getAccountCode(token, 'tuser100')
##    print '\ntuser100: ', repr(test)
##        
##    print '\nTestcase: UserClass38 - getAccountCode - user does not exist'
##    test = s.getAccountCode(token, 'notexist')
##    print '\nnotexist ', repr(test)
##
##    print '\nTestcase: UserClass39 - getPassword - user exists'
##    test = s.getPassword(token, 'tuser100')
##    print '\ntuser100: ', repr(test)
##        
##    print '\nTestcase: UserClass40 - getPassword - user does not exist'
##    test = s.getPassword(token, 'notexist')
##    print '\nnotexist ', repr(test)
##
##    print '\nTestcase: UserClass41 - getFirstName - user exists'
##    test = s.getFirstName(token, 'tuser100')
##    print '\ntuser100: ', repr(test)
##        
##    print '\nTestcase: UserClass42 - getFirstName - user does not exist'
##    test = s.getFirstName(token, 'notexist')
##    print '\nnotexist ', repr(test)
##
##    print '\nTestcase: UserClass43 - getLastName - user exists'
##    test = s.getLastName(token, 'tuser100')
##    print '\ntuser100: ', repr(test)
##        
##    print '\nTestcase: UserClass44 - getLastName - user does not exist'
##    test = s.getLastName(token, 'notexist')
##    print '\nnotexist ', repr(test)
##
##    print '\nTestcase: UserClass45 - getEmail - user exists'
##    test = s.getEmail(token, 'tuser100')
##    print '\ntuser100: ', repr(test)
##        
##    print '\nTestcase: UserClass46 - getEmail - user does not exist'
##    test = s.getEmail(token, 'notexist')
##    print '\nnotexist ', repr(test)
##
##    print '\nTestcase: UserClass47 - getLockoutThreshold - user exists'
##    test = s.getLockoutThreshold(token, 'tuser100')
##    print '\ntuser100: ', repr(test)
##        
##    print '\nTestcase: UserClass48 - getLockoutThreshold - user does not exist'
##    test = s.getLockoutThreshold(token, 'notexist')
##    print '\nnotexist ', repr(test)
##
##    print '\nTestcase: UserClass49 - getLockoutDuration - user exists'
##    test = s.getLockoutDuration(token, 'tuser100')
##    print '\ntuser100: ', repr(test)
##        
##    print '\nTestcase: UserClass50 - getLockoutDuration - user does not exist'
##    test = s.getLockoutDuration(token, 'notexist')
##    print '\nnotexist ', repr(test)
##
##    print '\nTestcase: UserClass51 - getMaxSessions - user exists'
##    test = s.getMaxSessions(token, 'tuser100')
##    print '\ntuser100: ', repr(test)
##        
##    print '\nTestcase: UserClass52 - getMaxSessions - user does not exist'
##    test = s.getMaxSessions(token, 'notexist')
##    print '\nnotexist ', repr(test)
##
##    print '\nTestcase: UserClass53 - getPinChange - user exists'
##    test = s.getPinChange(token, 'tuser100')
##    print '\ntuser100: ', repr(test)
##        
##    print '\nTestcase: UserClass54 - getPinChange - user does not exist'
##    test = s.getPinChange(token, 'notexist')
##    print '\nnotexist ', repr(test)
##
##    print '\nTestcase: UserClass55 - getRecord - user exists'
##    test = s.getRecord(token, 'tuser100')
##    print '\ntuser100: ', repr(test)
##        
##    print '\nTestcase: UserClass56 - getRecord - user does not exist'
##    test = s.getRecord(token, 'notexist')
##    print '\nnotexist ', repr(test)
##
##    print '\nTestcase: UserClass57 - getRecordVisible - user exists'
##    test = s.getRecordVisible(token, 'tuser100')
##    print '\ntuser100: ', repr(test)
##        
##    print '\nTestcase: UserClass58 - getRecordVisible - user does not exist'
##    test = s.getRecordVisible(token, 'notexist')
##    print '\nnotexist ', repr(test)
##
##    print '\nTestcase: UserClass59 - getArTransferNumber - user exists'
##    test = s.getArTransferNumber(token, 'tuser100')
##    print '\ntuser100: ', repr(test)
##        
##    print '\nTestcase: UserClass60 - getArTransferNumber - user does not exist'
##    test = s.getArTransferNumber(token, 'notexist')
##    print '\nnotexist ', repr(test)
##
##    print '\nTestcase: UserClass61 - getTimezone - user exists'
##    test = s.getTimezone(token, 'tuser100')
##    print '\ntuser100: ', repr(test)
##        
##    print '\nTestcase: UserClass62 - getTimezone - user does not exist'
##    test = s.getTimezone(token, 'notexist')
##    print '\nnotexist ', repr(test)
##

#### Setup
#### tuser100, tuser101, tuser102, tuser103
#### need to be created manually to verify that fields are
#### actually getting created with correct default values    
    print '\nTestcase: UserClass63 - userIdExists - userId exists'
    rc = s.getUserId(token, 'tuser100')
    userId = rc['result']
    test = s.userIdExists(token, userId)
    print '\ntuser100: ', repr(test)
        
    print '\nTestcase: UserClass64 - userIdExists - userId does not exist'
    test = s.userIdExists(token, 200)
    print '\n200 ', repr(test)
    
    print '\nTestcase: UserClass65 - setUsername - userId exists'
    rc = s.getUserId(token, 'tuser101')
    userId = rc['result']
    test = s.setUsername(token, userId, 'uc65')
    print '\nNew Username uc65: ', repr(test)
        
    print '\nTestcase: UserClass66 - setUsername - userId does not exist'
    test = s.setUsername(token, 200, 'notexist')
    print '\nnotexist ', repr(test)

    print '\nTestcase: UserClass67 - changeUsername - user exists'
    test = s.changeUsername(token, 'tuser102', 'uc67')
    print '\nNew Username uc67: ', repr(test)
        
    print '\nTestcase: UserClass68 - changeUsername - user does not exist'
    test = s.changeUsername(token, 'notexist', 'tuser200')
    print '\nnotexist ', repr(test)

    print '\nTestcase: UserClass69 - setPin - valid values'
    test = s.setPin(token, 'tuser101', 500)
    print '\nUser: tuser101, Pin: 500 ', repr(test)
    
    print '\nTestcase: UserClass70 - setPin - user does not exist'
    test = s.setPin(token, 'notexist', 501)
    print '\nUser: notexist, Pin: 501 ', repr(test)
        
    print '\nTestcase: UserClass71 - setPin - pin is null'
    test = s.setPin(token, 'tuser101', '')
    print '\nUser: notexist, Pin: null ', repr(test)

    print '\nTestcase: UserClass72 - setPin - pin is string'
    test = s.setPin(token, 'tuser101', 'hello')
    print '\nUser: tuser103, Pin: hello ', repr(test)

    print '\nTestcase: UserClass73 - setAccountCode - valid values'
    test = s.setAccountCode(token, 'tuser101', 600)
    print '\nUser: tuser101, AC: 500 ', repr(test)
    
    print '\nTestcase: UserClass74 - setAccountCode - user does not exist'
    test = s.setAccountCode(token, 'notexist', 601)
    print '\nUser: notexist, AC: 501 ', repr(test)
        
    print '\nTestcase: UserClass75 - setAccountCode - account code is null'
    test = s.setAccountCode(token, 'tuser101', '')
    print '\nUser: tuser101, AC: null ', repr(test)

    print '\nTestcase: UserClass76 - setAccountCode - account code is string'
    test = s.setAccountCode(token, 'tuser101', 'hello')
    print '\nUser: tuser103, AC: hello ', repr(test)     

    print '\nTestcase: UserClass77 - setPassword - valid values'
    test = s.setPassword(token, 'tuser101', 'uc77')
    print '\nUser: tuser101, Password: uc77 ', repr(test)
    
    print '\nTestcase: UserClass78 - setPassword - user does not exist'
    test = s.setPassword(token, 'notexist', 'uc78')
    print '\nUser: notexist, Password: uc77 ', repr(test)
        
    print '\nTestcase: UserClass79 - setPassword - password is null'
    test = s.setPassword(token, 'tuser101', '')
    print '\nUser: tuser101, Password: null ', repr(test)

    print '\nTestcase: UserClass80 - setFirstName - valid values'
    test = s.setFirstName(token, 'tuser101', 'uc80')
    print '\nUser: tuser101, First name: uc77 ', repr(test)
    
    print '\nTestcase: UserClass81 - setFirstName - user does not exist'
    test = s.setFirstName(token, 'notexist', 'uc81')
    print '\nUser: notexist, First name: uc77 ', repr(test)
        
    print '\nTestcase: UserClass82 - setFirstName - first name is null'
    test = s.setFirstName(token, 'tuser101', '')
    print '\nUser: tuser101, First name: null ', repr(test)

    print '\nTestcase: UserClass83 - setLastName - valid values'
    test = s.setLastName(token, 'tuser101', 'uc83')
    print '\nUser: tuser101, Last name: uc77 ', repr(test)
    
    print '\nTestcase: UserClass84 - setLastName - user does not exist'
    test = s.setLastName(token, 'notexist', 'uc84')
    print '\nUser: notexist, Last name: uc77 ', repr(test)
        
    print '\nTestcase: UserClass85 - setLastName - last name is null'
    test = s.setLastName(token, 'tuser101', '')
    print '\nUser: tuser101, Last name: null ', repr(test)

    print '\nTestcase: UserClass86 - setEmail - valid values'
    test = s.setEmail(token, 'tuser101', 'uc86')
    print '\nUser: tuser101, email: uc77 ', repr(test)
    
    print '\nTestcase: UserClass87 - setEmail - user does not exist'
    test = s.setEmail(token, 'notexist', 'uc87')
    print '\nUser: notexist, email: uc77 ', repr(test)
        
    print '\nTestcase: UserClass88 - setEmail - email is null'
    test = s.setEmail(token, 'tuser101', '')
    print '\nUser: tuser101, email: null ', repr(test)

    print '\nTestcase: UserClass89 - setLockoutDuration - valid values'
    test = s.setLockoutDuration(token, 'tuser101', 5)
    print '\nUser: tuser101, LD: 5 ', repr(test)
    
    print '\nTestcase: UserClass90 - setLockoutDuration - user does not exist'
    test = s.setLockoutDuration(token, 'notexist', 5)
    print '\nUser: notexist, LD: 5 ', repr(test)

    print '\nTestcase: UserClass91 - setLockoutDuration - account code is string'
    test = s.setLockoutDuration(token, 'tuser101', 'hello')
    print '\nUser: tuser101, LD: hello ', repr(test)
    
    print '\nTestcase: UserClass92 - setLockoutDuration - lockout duration is null'
    test = s.setLockoutDuration(token, 'tuser101', '')
    print '\nUser: tuser101, LD: null ', repr(test)

    print '\nTestcase: UserClass93 - setLockoutThreshold - valid values'
    test = s.setLockoutThreshold(token, 'tuser101', 5)
    print '\nUser: tuser101, LT: 5 ', repr(test)
    
    print '\nTestcase: UserClass94 - setLockoutThreshold - user does not exist'
    test = s.setLockoutThreshold(token, 'notexist', 5)
    print '\nUser: notexist, LT: 6 ', repr(test)

    print '\nTestcase: UserClass95 - setLockoutThreshold - account code is string'
    test = s.setLockoutThreshold(token, 'tuser101', 'hello')
    print '\nUser: tuser101, LT: hello ', repr(test)
    
    print '\nTestcase: UserClass96 - setLockoutThreshold - lockout threshold is null'
    test = s.setLockoutThreshold(token, 'tuser101', '')
    print '\nUser: tuser101, LT: null ', repr(test)
    
## Cleanup - I need a user manually created so I can't use delete all users.
##    test = s.deleteUser(token, 'tuser2')
##    test = s.deleteUser(token, 'tuser3')
##    test = s.deleteUser(token, 'tuser4')
##    test = s.changeUsername(token, 'uc65', 'tuser101')
##    test = s.changeUsername(token, 'uc67', 'tuser102')

                         
