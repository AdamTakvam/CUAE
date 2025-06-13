// g711.h

/**
 *
 * g711.h: Header file based on g711.c
 *
 */

#ifndef G711_H
#define G711_H

extern "C" unsigned char linear2alaw( int );

extern "C" int alaw2linear( unsigned char );

extern "C" unsigned char linear2ulaw( int );

extern "C" int ulaw2linear( unsigned char );

#endif
