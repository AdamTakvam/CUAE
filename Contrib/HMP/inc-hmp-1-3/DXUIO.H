#ifndef __DXUIO_H__
#define __DXUIO_H__

/*
 * User Defined I/O Functions for read()/write()/lseek()
 */

#if ( defined (__STDC__) || defined (__cplusplus) )

typedef struct dx_uio {
   int (* u_read)(int, char *, unsigned); /* User Defined read() */
   int (* u_write)(int, char *, unsigned);/* User Defined write() */
   long (* u_seek)(int, long, int);       /* User Defined lseek() */
} DX_UIO;

#else

typedef struct dx_uio {
   int (* u_read)();       /* User Defined replacement for read() */
   int (* u_write)();      /* User Defined replacement for write() */
   long (* u_seek)();       /* User Defined replacement for lseek() */
} DX_UIO;

#endif

/*
 * User Defined I/O Functions for direct play()/record()
 */
#if ( defined (__STDC__) || defined (__cplusplus) )

typedef struct dx_uio_direct {
   unsigned long uDirectFlags;
   int (*uPlayBufferAlloc)(int, char **, unsigned);
   void (*uPlayBufferFree)(int, char *);
   int (*uRecordBufferAlloc)(int, char **, unsigned);
   void (*uRecordBufferFree)(int, char *, unsigned);
} DX_UIO_DIRECT;

#else

typedef struct dx_uio_direct {
   unsigned long uDirectFlags;
   int (*uPlayBufferAlloc)();
   void (*uPlayBufferFree)();
   int (*uRecordBufferAlloc)();
   void (*uRecordBufferFree)();
} DX_UIO_DIRECT;

#endif

#endif	/* __DXUIO_H__	*/