/* $Id: testTrace.java 7054 2005-06-07 17:48:33Z wert $
 *
 * Created by wert on Apr 13, 2005.
 * 
 * Copyright (c) 2005 Metreos, Inc. All rights reserved.
 */

package metreos.util.test;

import metreos.util.DefaultTrace;
import metreos.util.Trace;

/**
 * Description of testTrace
 */
public class testTrace implements Runnable
{
	/**
	 * @param args
	 * @throws InterruptedException
	 */
	public static void main( String[] args ) throws InterruptedException
	{
		Trace t = new DefaultTrace( 100, 20 );
		t.start();
		Trace.setTrace( t );
		
		testTrace tt = new testTrace();
		
		Thread t1 = new Thread( tt );
		t1.start();
		
		Thread t2 = new Thread( tt );
		t2.start();
		
		t1.join();
		t2.join();
		
		Trace.report( "done" );
		System.out.println( "-------" );
		
		t.stop();
	}
	
	public void run()
	{
		for (int i = 0; i < 1000; i++)
			Trace.report( Trace.m( i ) );
	}
}
