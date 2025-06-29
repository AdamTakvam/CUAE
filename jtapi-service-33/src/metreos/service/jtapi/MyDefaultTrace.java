/* $Id: MyDefaultTrace.java 30152 2007-03-06 21:47:50Z wert $
 *
 * Created by wert on Apr 13, 2005.
 *
 * Copyright (c) 2005 Metreos, Inc. All rights reserved.
 */

package metreos.service.jtapi;

import java.io.PrintStream;

import metreos.util.DefaultTrace;
import metreos.util.QueuedTrace;
import metreos.util.URL;

import com.cisco.jtapi.extensions.CiscoJtapiException;

/**
 * Description of MyDefaultTrace.
 */
public class MyDefaultTrace extends DefaultTrace
{
	/**
	 * Constructs the MyDefaultTrace with queuing disabled.
	 * 
	 * @see QueuedTrace#QueuedTrace()
	 */
	public MyDefaultTrace()
	{
		super();
	}
	
	/**
	 * Constructs the MyDefaultTrace with the specified
	 * queuing parameters.
	 *
	 * @param maxLen
	 * @param addDelay
	 * 
	 * @see QueuedTrace#QueuedTrace(int, int)
	 */
	public MyDefaultTrace( int maxLen, int addDelay )
	{
		super( maxLen, addDelay );
	}
	
	/**
	 * Constructs the MyDefaultTrace.
	 *
	 * @param url
	 */
	public MyDefaultTrace( URL url )
	{
		super( url );
	}
	
	@Override
	protected void formatThrowable( PrintStream ps, Throwable t )
	{
		if (t instanceof CiscoJtapiException)
		{
			CiscoJtapiException e = (CiscoJtapiException) t;
			ps.print( "additional info: '" );
			ps.print( e.getErrorName() );
			ps.print( "': '" );
			ps.print( e.getErrorDescription() );
			ps.println( "'" );
		}
		t.printStackTrace( ps );
	}
}
