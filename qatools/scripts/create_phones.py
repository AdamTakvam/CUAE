#!/usr/bin/python
from optparse import OptionParser

if __name__ == '__main__':
    p = OptionParser()
    p.add_option('--number',
        dest='numDevices',type='int',default=100)
    p.add_option('--mac',
        dest='macAddressBase',type='string',default='110011001100')
    p.add_option('--description',
        dest='description',type='string',default='BAT %s')
    p.add_option('--dn',
        dest='directoryNumberBase',type='int',default=1000)
    p.add_option('--increment',
        dest='dnIncrement',type='int',default=1)
    p.add_option('--output',
        dest='outputFile',default=None)
    
    (opt,args) = p.parse_args()
   
    out = []
    out.append("NUMBER OF LINES,MAC ADDRESS,DESCRIPTION,DIRECTORY NUMBER")
    for i in range(0,opt.numDevices):
        dn  = i*opt.dnIncrement + opt.directoryNumberBase
        mac = "%x" % (int(opt.macAddressBase,16) + i)
        out.append("1,%s,BAT %s,%s" % (mac,dn,dn))
    
    csv = "\r\n".join(out)
    if opt.outputFile:
        open(opt.outputFile,'w').write(csv)
    else:
        print csv 
