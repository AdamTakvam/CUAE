/* $Id: testAlarm.java 30151 2007-03-06 21:47:46Z wert $
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
public class testAlarm implements AlarmListener
{
	/**
	 * Constructs the testAlarm.
	 *
	 * @param id
	 * @param interval 
	 * @param count 
	 * @param whatsBehind 
	 */
	public testAlarm( int id, int interval, int count, int whatsBehind )
	{
		this.id = id;
		this.interval = interval;
		this.count = count;
		this.whatsBehind = whatsBehind;
	}
	
	private final int id;
	
	private final int interval;
	
	private final int count;
	
	private final int whatsBehind;
	
	private void start()
	{
		am.add( this, interval );
	}

	public synchronized int wakeup( AlarmManager manager, long due )
	{
		if (index < count)
		{
			index++;
			long t = System.currentTimeMillis() - due;
			if (t >= whatsBehind)
			{
//				Trace.report( this, "falling behind by "+t );
				behind++;
			}
			else
			{
				delay( 1 );
			}
			return interval;
		}
		
		notify();
		return 0;
	}
	
	private int index = 0;
	
	private int behind = 0;

	private synchronized int waitDone( long due )
	{
		try
		{
			long now = System.currentTimeMillis();
			while (due > now && index < count)
			{
				wait( due - now );
				now = System.currentTimeMillis();
			}
		}
		catch ( InterruptedException e )
		{
			// ignore
		}
		int remaining = count - index;
		index = count; // this forces quit.
		return remaining;
	}

	@Override
	public String toString()
	{
		return "app "+id;
	}
	
	private static AlarmManager am = new AlarmManager();

	/**
	 * @param args
	 */
	public static void main( String[] args )
	{
		int amThreads = 150;
		int nApps = 10000;
		int interval = 1000; // in ms
		int duration = 120*1000; // in ms
		int whatsBehind = 500; // in ms
		int count = duration / interval;
		
		am.start( amThreads );
		
		testAlarm[] apps = new testAlarm[nApps];
		for (int i = 0; i < nApps; i++)
			apps[i] = new testAlarm( i, interval, count, whatsBehind );

		Trace.report( "started "+nApps );
		
		long t0 = System.currentTimeMillis();
		
		for (int i = 0; i < nApps; i++)
			apps[i].start(  );
		
		long due = t0 + duration * 3 / 2;
		int remaining = 0;
		for (int i = 0; i < nApps; i++)
			remaining += apps[i].waitDone( due );
		
		long t1 = System.currentTimeMillis();
		
		int behind = 0;
		for (int i = 0; i < nApps; i++)
			behind += apps[i].behind;
		
		int tCount = count*nApps;
		
		Trace.report( "done"
			+", "+nApps+" took "+(t1-t0)+" ms"
			+", remaining = "+remaining
			+", count = "+tCount
			+", behind = "+behind
			+", behind% = "+(behind*100/tCount) );
		
		am.stop();
		Trace.shutdown();
	}

	/**
	 * @param ms
	 */
	public static void delay( int ms )
	{
		try
		{
			Thread.sleep ( ms );
		}
		catch ( InterruptedException e )
		{
			// ignore
		}
	}
}
