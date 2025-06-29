/* $Id: MyIpcTrace.java 30152 2007-03-06 21:47:50Z wert $
 *
 * Created by wert on Apr 13, 2005.
 *
 * Copyright (c) 2005 Metreos, Inc. All rights reserved.
 */

package metreos.service.jtapi;

import java.io.PrintWriter;

import metreos.core.log.IpcTrace;
import metreos.util.URL;

import com.cisco.jtapi.extensions.CiscoJtapiException;

/**
 * Description of MyIpcTrace.
 */
public class MyIpcTrace extends IpcTrace
{
	/**
	 * Constructs the MyIpcTrace.
	 *
	 * @param maxLen
	 * @param addDelay
	 * @param loggingHost
	 * @param loggingPort
	 * @param id
	 */
	public MyIpcTrace( int maxLen, int addDelay, String loggingHost,
		int loggingPort, String id )
	{
		super( maxLen, addDelay, loggingHost, loggingPort, id );
	}
	
	/**
	 * Constructs the MyIpcTrace.
	 *
	 * @param url
	 */
	public MyIpcTrace( URL url )
	{
		super( url );
	}
	
	@Override
	protected void formatThrowable( PrintWriter pw, Throwable t )
	{
		if (t instanceof CiscoJtapiException)
		{
			CiscoJtapiException e = (CiscoJtapiException) t;
			pw.print( "additional info: " );
			pw.print( e.getErrorName() );
			pw.print( ": " );
			pw.println( e.getErrorDescription() );
		}
		t.printStackTrace( pw );
	}
}