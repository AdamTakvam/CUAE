###################################################################
## This script generates appsuiteadmin users based on configuration
## items below.  Only required fields are supported for now.  
##
## Developer:       Date:
## Jan Capps        27-MAR-2006
##
###################################################################
import SOAPpy
import os
import string

if __name__=='__main__':

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

## Configuration items
    iNumUsers = 5
    bDeleteAllUsers = False
    sFirstNamePrefix = 'Test'
    sLastNamePrefix = 'User'
    sUsernamePrefix = 'tuser'
    sPasswordPrefix = 'metreos'
    bUniquePasswords = False
    sAppServer = None
    sUrl = 'http://%s/appsuiteadmin/soap.php?wsdl' % sAppServer

## End Configuration items
    
    s = SOAPpy.WSDL.Proxy(sUrl)
    
    token = s.login('Administrator','metreos')

    if bDeleteAllUsers:
        try:
            test = s.deleteAllUsers(token)
        except:
            print 'Unhandled exception in deleteAllUsers.'
            pass
   
##  Add users
    for i in range(iNumUsers):
        initUserRecord(userRecord)
        userRecord['firstName'] = '%s%s' % (sFirstNamePrefix, i+1)
        userRecord['lastName'] = '%s%s' % (sLastNamePrefix, i+1)
        userRecord['username'] = '%s%s' % (sUsernamePrefix, i+1)

        if bUniquePasswords:
            userRecord['password'] = '%s%s' % (sPasswordPrefix, i+1)
        else:
            userRecord['password'] = 'metreos'
        
        userRecord['pin'] = i+1
        userRecord['accountCode'] = i+1
    
        try:
            test = s.addUser(token, userRecord)
        except:
            print '\nFAILED: %s. Unexpected exception' % userRecord['username']
            pass





