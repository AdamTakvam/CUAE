/* $Id: Convert.java 7054 2005-06-07 17:48:33Z wert $
 *
 * Created by wert on Jan 27, 2005
 *
 * Copyright (c) 2005 Metreos, Inc. All rights reserved.
 */

package metreos.util;

/**
 * Misc utilities for converting bytes to ints, etc.
 */
public abstract class Convert
{
	/**
	 * Converts bytes from a buffer into a 32-bit int.
	 * 
	 * @param bytes the buffer.
	 * 
	 * @param offset the offset in the buffer.
	 * 
	 * @return a 32-bit int.
	 */
	public static int getInt( byte[] bytes, int offset )
	{
		int v0 = bytes[offset++] & 255;
		int v1 = bytes[offset++] & 255;
		int v2 = bytes[offset++] & 255;
		int v3 = bytes[offset++] & 255;
		return (v3 << 24) | (v2 << 16) | (v1 << 8) | v0;
	}

	/**
	 * Converts a 32-bit int into bytes in a buffer.
	 * @param value a 32-bit int.
	 * @param bytes the buffer.
	 * @param offset the offset in the buffer.
	 */
	public static void putInt( int value, byte[] bytes, int offset )
	{
		bytes[offset++] = (byte) value;
		bytes[offset++] = (byte) (value >> 8);
		bytes[offset++] = (byte) (value >> 16);
		bytes[offset++] = (byte) (value >> 24);
	}
	
	
	/**
	 * Converts bytes from a buffer into a 64-bit long.
	 * 
	 * @param bytes the buffer.
	 * 
	 * @param offset the offset in the buffer.
	 * 
	 * @return a 64-bit long.
	 */
	public static long getLong( byte[] bytes, int offset )
	{
		long lo = getInt( bytes, offset ) & 0xffffffffL;
		long hi = getInt( bytes, offset+4 ) & 0xffffffffL;
		return (hi << 32) + lo;
	}
		
	
	/**
	 * Converts a 64-bit long into bytes in a buffer.
	 * @param value a 64-bit long.
	 * @param bytes the buffer.
	 * @param offset the offset in the buffer.
	 */
	public static void putLong( long value, byte[] bytes, int offset )
	{
		putInt( (int) value, bytes, offset );
		putInt( (int) (value >>> 32), bytes, offset+4 );
	}
	
	/**
	 * Converts bytes from a buffer into a 64-bit floating pt.
	 * 
	 * @param bytes the buffer.
	 * 
	 * @param offset the offset in the buffer.
	 * 
	 * @return a 64-bit fp.
	 */
	public static double getDouble( byte[] bytes, int offset )
	{				
		return Double.longBitsToDouble( getLong( bytes, offset ) );
	}
	
	/**
	 * Converts a 64-bit floating pt into bytes in a buffer.
	 * @param value a 64-bit fp.
	 * @param bytes the buffer.
	 * @param offset the offset in the buffer.
	 */
	public static void putDouble(double value, byte[] bytes, int offset )
	{		
		putLong( Double.doubleToLongBits( value ), bytes, offset );	
	}
}
