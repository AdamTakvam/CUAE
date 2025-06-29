import logging
import os
import sys
import types
import string
import subprocess
import win32con,win32api,win32pdhutil
import zipfile
import tarfile
import glob
import shutil
import re
from Queue import Queue, Full, Empty
try:
    import MySQLdb
    import MySQLdb.cursors
except:
    pass
    

## Constants
MYSQLROOT               = r'C:\Program Files\MySQL\MySQL Server 4.1'
MYSQLSHELL              = os.path.join(MYSQLROOT,'bin','mysql.exe')

### Classes
class DB:
    def __init__(self, *args, **kwargs):
        self.database = kwargs.get('db', None)
        self.username = kwargs.get('user', None)
        self.password = kwargs.get('passwd', None)
        self.host     = kwargs.get('host', None)
        kwargs.update({'cursorclass':MySQLdb.cursors.DictCursor})
        self.pool = Pool(apply(Constructor, [MySQLdb.connect,]+list(args), kwargs))

    def execute(self, *args, **kwargs):
        obj = self.pool.get()
        insert_id = None
        r         = None
        try:
            cursor = obj.cursor()
            r      = apply(cursor.execute, args, kwargs)
            insert_id = obj.insert_id()
            obj.commit()
        except:
            logging.exception("Error executing SQL statement %s" % repr(args))
            raise Exception, "DB.execute: could not execute SQL statement %s,%s" % (sys.exc_info()[0], sys.exc_info()[1])
            
        self.pool.put(obj)
        return (r,insert_id)
   
    def executemany(self, *args, **kwargs):
        obj = self.pool.get()
        r   = None
        try:
            cursor = obj.cursor()
            r      = apply(cursor.executemany, args, kwargs)
            obj.commit()
        except:
            logging.exception("Error executing SQL statement %s" % repr(args))
            raise Exception, "DB.executemany: could not execute SQL statement %s,%s" % (sys.exc_info()[0], sys.exc_info()[1])

        self.pool.put(obj)
        return (r, None)

    def old_executefile(self, filename):
        if not os.path.exists(filename):
            raise Exception, "File not found"
        sql = open(filename, 'r').read()
        return self.execute(sql)

    def executefile(self, filename):
        if not os.path.exists(filename):
            raise Exception, "File not found: %s" % filename
        cmd = [MYSQLSHELL,
            '--user=%s' % self.username,
            '--password=%s' % self.password,
            '--database=%s' % self.database]
        s = subprocess.Popen(cmd, stdout=subprocess.PIPE, stderr=subprocess.STDOUT, stdin=subprocess.PIPE).communicate(open(filename,'r').read())
        return s[0].strip()

    def query(self, *args, **kwargs):
        obj = self.pool.get()
        retval = None
        try:
            cursor = obj.cursor()
            r      = apply(cursor.execute, args, kwargs)
            retval = cursor.fetchall()
        except:
            logging.exception("Error executing SQL query %s" % repr(args))
            raise Exception, "DB.query: could not execute SQL statement %s,%s" % (sys.exc_info()[0], sys.exc_info()[1])

        self.pool.put(obj)
        return retval

class Pool (Queue):
    def __init__(self, constructor, poolsize=5):
        Queue.__init__(self, poolsize)
        self.constructor = constructor
    
    def get(self, block=1):
        try:
            return self.empty() and self.constructor() or Queue.get(self, block)
        except:
            return self.constructor()
    
    def put(self, obj, block=1):
        try:
            return self.full() and None or Queue.put(self, obj, block)
        except Full:
            pass


class Constructor:
    def __init__(self, function, *args, **kwargs):
        self.f      = function
        self.args   = list(args)
        self.kwargs = kwargs
    
    def __call__(self, *args, **kwargs):
        self.args += list(args)
        self.kwargs.update(kwargs)
        return apply(self.f, self.args, self.kwargs)

Callback = Constructor

def replacetree(src, dst, symlinks=False):
    """Recursively replace a directory tree"""
    names = os.listdir(src)
    errors = []
    if not os.path.exists(dst):
        os.makedirs(dst)
    for name in names:
        srcname = os.path.join(src, name)
        dstname = os.path.join(dst, name)
        try:
            if symlinks and os.path.islink(srcname):
                linkto = os.readlink(srcname)
                if os.path.exists(dstname):
                    os.unlink(dstname)
                os.symlink(linkto, dstname)
            elif os.path.isdir(srcname):
                replacetree(srcname, dstname, symlinks)
            else:
                shutil.copy2(srcname, dstname)
        except (IOError, os.error), why:
            errors.append((srcname, dstname, why))
    prunetree(src,dst)
    if errors:
        raise Error, errors
