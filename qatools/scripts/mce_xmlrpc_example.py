import xmlrpclib


if __name__=='__main__':

    # WebService URL for MCEADMIN, replace 'mceadmin' with
    # 'appsuiteadmin' if you want to access the AppSuiteAdmin
    s = xmlrpclib.Server('http://10.1.12.190/mceadmin/xml-rpc.php')

    # Login to the webservice interface and get an auth-token, the
    # auth-token must be used on future requests
    
    token = s.login('Administrator','metreos')

    # make a call
    users = s.getUserList(token)
    print repr(users)
