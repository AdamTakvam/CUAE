/* $Id: testAlarm1.java 30151 2007-03-06 21:47:46Z wert $
 *
 * Created by wert on Apr 29, 2005.
 *
 * Copyright (c) 2005 Metreos, Inc. All rights reserved.
 */

package metreos.util.test;

import metreos.util.AlarmListener;
import metreos.util.AlarmManager;
import metreos.util.Trace;

/**
 * Description of testAlarm.
 */
public class testAlarm1 implements AlarmListener
{
	/**
	 * @param args
	 * @throws InterruptedException 
	 */
	public static void main( String[] args ) throws InterruptedException
	{
		doit( 1 );
		doit( 1 );
		doit( 1 );
		
		int i = 1;
		while (i <= 64)
		{
			doit( i );
			i *= 2;
		}
		
		Trace.shutdown();
	}
	
	private static void doit( int nWorkers ) throws InterruptedException
	{
		AlarmManager am = new AlarmManager();
		summary = new int[NBUCKETS];
		total = 0;
		
//		Trace.report( "starting" );
		am.start( nWorkers );
//		Trace.report( "started" );
		
		for (int i = 0; i < 3000; i++)
			am.add( new testAlarm1( i ), (int) (Math.random()*100) );
		
		Thread.sleep( 30000 );
		
//		Trace.report( "stopping" );
		am.stop();
//		Trace.report( "stopped" );
		
		Thread.sleep( 5000 );
		
		System.out.print( "done " );
		System.out.print( nWorkers );
		System.out.print( ", total " );
		System.out.print( total );
//		System.out.print( " ::" );
		for (int i = 0; i < NBUCKETS; i++)
		{
			System.out.print( "\t" );
			System.out.print( summary[i]*1000L/total );
		}
		System.out.println();
	}

	private synchronized static void summarize( int[] buckets )
	{
		for (int i = 0; i < NBUCKETS; i++)
		{
			summary[i] += buckets[i];
			total += buckets[i];
		}
	}
	
	private final static int NBUCKETS = 10;
	
	private static int total;
	private static int[] summary;

	/**
	 * Constructs the testAlarm1.
	 *
	 * @param id
	 */
	public testAlarm1( int id )
	{
		this.id = id;
	}
	
	private final int id;

	@Override
	public String toString()
	{
		return "testAlarm1_"+id;
	}
	
	public int wakeup( AlarmManager manager, long due )
	{
		int delay = (int) ((System.currentTimeMillis()+5-due) / 5);
		
		if (delay < 0)
			delay = 0;
		else if (delay >= buckets.length)
			delay = buckets.length-1;
		
		buckets[delay]++;
		count--;
		
		if (count == 0)
		{
			summarize( buckets );
			return 0;
		}
		
		return 20; // rate = 50/sec
	}
	
	private int count = 1000; // 20 seconds
	
	private int[] buckets = new int[NBUCKETS];
}
