import sys
import os
import shutil
import time
from win32com.client import Dispatch

file = sys.argv[1]
docFile = os.path.basename(file)
workDir = os.path.dirname(file)
if file == docFile:
    workDir = os.getcwd()
    file    = os.path.join(workDir,docFile)

fName,fExt = os.path.splitext(os.path.basename(file))
pdfFile = fName + '.pdf'
srcPDFFile = pdfFile.replace('(','_').replace(')','_')

try:
    if fExt in ('.doc','.rtf'):
        myDoc = Dispatch("Word.Application")
        myDoc.Visible = 0
    elif fExt in ('.vsd',):
        myDoc = Dispatch("Visio.Application")
        myDoc.Visible = 0
    else:
        print "Do not know how to process '%s' files" % fExt
        sys.exit(0)
except:
    print "%s,%s" % (sys.exc_info()[0],sys.exc_info()[1])
    sys.exit(1)

try:
    d = myDoc.Documents.Open(file)
    if fExt in ('.vsd',):
        d.PrintOut(0,PrinterName='Adobe PDF')
        srcPDFFile = 'Visio-' + pdfFile
    else:    
        myDoc.ActivePrinter = "Adobe PDF"
        d.PrintOut(Background=0)
    time.sleep(15)
    
    d.Close()
    shutil.copy(r'c:\temp\\' + srcPDFFile,os.path.join(workDir,pdfFile))
    os.remove(r'c:\temp\\' + srcPDFFile)
except:
    print "%s,%s" % (sys.exc_info()[0],sys.exc_info()[1])
try:
    myDoc.Quit()
except:
    print "%s,%s" % (sys.exc_info()[0],sys.exc_info()[1])

myDoc = None

