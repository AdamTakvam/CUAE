import sys
import os
import glob

def generate(comment,path):
    out = []
    out.append('<!-- %s -->'%comment)
    out.append('<!-- Applications in %s -->'%path)
    a = glob.glob('%s/*' % path)
    for x in a:
        dirname = os.path.normpath(x).replace('\\','/')
        maxfile = os.path.basename(x)
        out.append('<compileMax dir="%s" maxfile="%s" />' % (dirname, maxfile))
    return "\n\n" + "\n".join(out) + "\n"    

if __name__=='__main__':
    out = ''
    out += generate('ARE TestBank', 'TestBank/ARE')
    out += generate('Core TestBank', 'TestBank/Core')
    out += generate('Max TestBank', 'TestBank/Max')
    out += generate('SMA TestBank', 'TestBank/SMA')
    out += generate('Provider TestBank', 'TestBank/Provider')
    out += generate('IVT TestBank', 'TestBank/IVT')
    out += generate('App TestBank', 'TestBank/App')

    print '<?xml version="1.0" encoding="utf-8" ?>'
    print out
