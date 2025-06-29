/* $Id: DefaultTrace.java 30151 2007-03-06 21:47:46Z wert $
 *
 * Created by wert on Feb 16, 2005
 *
 * Copyright (c) 2005 Metreos, Inc. All rights reserved.
 */

package metreos.util;

import java.io.PrintStream;
import java.text.DateFormat;
import java.text.SimpleDateFormat;
import java.util.Date;

/**
 * Simple trace built on print streams.
 */
public class DefaultTrace extends QueuedTrace
{
	/**
	 * Constructs a DefaultTrace with specified out and err streams
	 * and specified params for QueuedTrace.
	 * 
	 * @param maxLen
	 * @param addDelay
	 * @param out
	 * @param err
	 * 
	 * @see QueuedTrace#QueuedTrace(int, int)
	 */
	public DefaultTrace( int maxLen, int addDelay, PrintStream out, PrintStream err )
	{
		super( maxLen, addDelay );
		this.out = out;
		this.err = err;
	}
	
	/**
	 * Constructs the DefaultTrace with standard out and err streams
	 * and specified params for QueuedTrace.
	 *
	 * @param maxLen
	 * @param addDelay
	 * 
	 * @see QueuedTrace#QueuedTrace(int, int)
	 */
	public DefaultTrace( int maxLen, int addDelay )
	{
		super( maxLen, addDelay );
	}
	
	/**
	 * Constructs a DefaultTrace with specified out and err streams
	 * and default params for QueuedTrace.
	 * 
	 * @param out
	 * @param err
	 * 
	 * @see QueuedTrace#QueuedTrace()
	 */
	public DefaultTrace( PrintStream out, PrintStream err )
	{
		this.out = out;
		this.err = err;
	}
	
	/**
	 * Constructs a DefaultTrace with standard out and err streams
	 * and default params for QueuedTrace.
	 * 
	 * @see QueuedTrace#QueuedTrace()
	 */
	public DefaultTrace()
	{
		// nothing to do.
	}
	
	/**
	 * Constructs the DefaultTrace.
	 *
	 * @param url
	 */
	public DefaultTrace( URL url )
	{
		super( url );
	}

	private PrintStream out = System.out;
	
	private PrintStream err = System.out; // System.err;

	@Override
	protected void showReport0( long when, int eventMask, Object who, String msg,
		Throwable t )
	{
		PrintStream ps = (eventMask&FAILURE_MASK) != 0 ? err : out;

		ps.print( df.format( new Date( when ) ) );
		ps.print( ' ' );
		
		if (who != null)
		{
			ps.print( who );
			ps.print( ": " );
		}
		
		ps.println( msg );
		
		if (t != null)
			formatThrowable( ps, t );
		
		ps.flush();
	}
	
	/**
	 * Formats up information from a throwable. The default is
	 * to show the standard information shown by printing the
	 * stack trace.
	 * 
	 * @param ps
	 * @param t
	 * 
	 * @see Throwable#printStackTrace()
	 */
	protected void formatThrowable( PrintStream ps, Throwable t )
	{
		t.printStackTrace( ps );
	}

	// no spaces in this so that it will be a single token.
	private DateFormat df = new SimpleDateFormat( "yyyy-MM-dd_HH:mm:ss.SSS" );
}
