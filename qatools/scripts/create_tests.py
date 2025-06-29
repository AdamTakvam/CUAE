#
import xmlrpclib
import string
import re
import sys
from optparse import OptionParser

def parseContent(content):
    """
    parseContent(<confluence-document-text>)

    Parse the content of a confluence document. Extract lines of the form:
       
       [TESTCASE-XXXXX-nnnn] - <descriptive-text>
    
    Return a list of tuples containing (<link-name>,<descriptive-text>)
    """
    testCases = []
    for x in content.split("\n"):
        m = re.match(r'.+\[(TESTCASE-.+)\][-\s]*(.+)', x)
        if m:
            testCases.append((m.group(1),m.group(2)))
    return testCases

def registerCases(s, token, suiteTitle):
    testTemplate="""h2. Scope
{excerpt}%s{excerpt}

h2. Description
blah

h2. Setup
blah

h2. Verify
blah
    """

    try:
        suite = s.confluence1.getPage(token,'QA',suiteTitle)
        testCases = parseContent(suite['content'])
        print suiteTitle
        
        # Add Test Cases
        for t in testCases:
            page = {
                'space'    : 'QA',
                'parentId' : suite['id'],
                'title'    : t[0],
                'content'  : testTemplate % t[1]
                }
            try:
                s.confluence1.storePage(token, page)
                print "  %s" % t[0]
            except:
                pass
    except:
        pass

if __name__=='__main__':
    
    # Process Options
    parser = OptionParser()
    parser.add_option('-u','--user',dest='user',default=None)
    parser.add_option('-p','--password',dest='password',default=None)
    parser.add_option('-s','--suite',dest='suite',default=None)
    parser.add_option('-f','--file',dest='file',default=None)
    parser.add_option('--url',dest='url',default='http://ast-metreos-lnx1/confluence/rpc/xmlrpc')
    (options, args) = parser.parse_args()
   
    # Abort if user and password not set
    if not options.user or not options.password:
        raise Exception,"Must specify a 'user' and 'password'"

    # Abort if neither a suite or suite-file are specified
    if not options.suite and not options.file:
        raise Exception, "Must specify either a 'suite' or a 'file' that list suites"

    # Compute suite-list
    if not options.suite:
        if not os.path.exists(options.file):
            raise Exception, "No such file %s" % options.file
        suiteList = open(options.suite,'r').read().split("\n")
    else:
        suiteList = [options.suite]
        
    # Do it
    s     = xmlrpclib.Server(options.url)
    token = s.confluence1.login(options.user,options.password)
    for suiteTitle in suiteList:
        registerCases(s,token,suiteTitle)
    
