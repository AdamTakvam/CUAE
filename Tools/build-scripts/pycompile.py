#!/bin/env python
import sys
import py_compile
import compileall

if __name__=='__main__':
    s = not compileall.main()
    sys.exit(s)
    
