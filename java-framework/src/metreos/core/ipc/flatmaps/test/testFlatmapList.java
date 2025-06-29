/* $Id: testFlatmapList.java 6730 2005-05-17 18:40:27Z wert $
 *
 * Created by wert on Jan 26, 2005
 *
 * Copyright (c) 2005 Metreos, Inc. All rights reserved.
 */

package metreos.core.ipc.flatmaps.test;

import java.text.DecimalFormat;
import java.text.NumberFormat;

import metreos.core.ipc.flatmaps.Flatmap;
import metreos.core.ipc.flatmaps.FlatmapException;
import metreos.core.ipc.flatmaps.FlatmapList;

/**
 * Test flat map list.
 */
public class testFlatmapList
{
	/**
	 * The main program.
	 * @param args
	 * @throws FlatmapException
	 */
	public static void main( String[] args ) throws FlatmapException
	{
		FlatmapList fml = new FlatmapList().add( 23, 34 ).add( 56, 78 );
		
		FlatmapList list = new FlatmapList();
		list.add( 2, "c" );
		list.add( 1, "a" );
		list.add( 2, "d" );
		list.add( 1, "b" );
		list.add( 1, fml );
		list.add( 3, 3.14159 );
		list.add( 4, 12345678987654321L );
		System.out.println( "list = "+list );
		
		System.out.println( "entry 2/1 = "+list.find( 2, 1 ) );
		System.out.println( "entry 2/2 = "+list.find( 2, 2 ) );
		System.out.println( "entry 2/3 = "+list.find( 2, 3 ) );
		
//		list.remove( 2, 2 );
//		System.out.println( "list = "+list );
//		list.remove( 2 );
//		System.out.println( "list = "+list );
//		list.remove( 2 );
//		System.out.println( "list = "+list );
		
		byte[] bytes = list.toFlatmap();
		dump( bytes );
		
		FlatmapList list2 = Flatmap.fromFlatmap( bytes );
		System.out.println( "list2 = "+list2 );
	}
	
	/**
	 * Dumps the byte array to System.out.
	 * @param bytes
	 */
	public static void dump( byte[] bytes )
	{
		NumberFormat nf = new DecimalFormat( " #" );
		int n = bytes.length;
		System.out.println( "bytes["+n+"] =" );
		for (int i = 0; i < n; i++)
			System.out.print( nf.format( bytes[i]&255 ) );
		System.out.println();
	}
}
