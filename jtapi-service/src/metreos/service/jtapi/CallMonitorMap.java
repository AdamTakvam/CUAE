/* $Id: CallMonitorMap.java 8309 2005-08-03 16:20:29Z wert $
 *
 * Created by wert on Feb 17, 2005.
 *
 * Copyright (c) 2005 Metreos, Inc. All rights reserved.
 */

package metreos.service.jtapi;

import java.util.HashMap;
import java.util.Map;

import metreos.util.StringUtil;
import metreos.util.Trace;


/**
 * Map of call monitors, by our call id.
 */
public class CallMonitorMap
{
	/**
	 * @param cm
	 */
	public synchronized void add( CallMonitor cm )
	{
		callMonitors.put( cm.ourCallId, cm );
	}

	/**
	 * @param ourCallId
	 * @return the associated call monitor
	 */
	public synchronized CallMonitor get( String ourCallId )
	{
		return callMonitors.get( ourCallId );
	}

	/**
	 * @param oldCallId
	 * @param newCallId
	 * @return true if it worked, false otherwise.
	 */
	public synchronized CallMonitor rename( String oldCallId, String newCallId )
	{
		Trace.report( this, Trace.m( "rename " ).a( oldCallId ).a( " -> " ).a( newCallId ) );
		
		if (callMonitors.containsKey( newCallId ))
			return null;
		
		CallMonitor cm = callMonitors.remove( oldCallId );
		if (cm == null)
			return null;

		cm.ourCallId = newCallId;
		callMonitors.put( newCallId, cm );
		return cm;
	}

	/**
	 * @param cm
	 */
	public synchronized void remove( CallMonitor cm )
	{
		callMonitors.remove( cm.ourCallId );
	}

	/**
	 * @return a new unique interim call id.
	 */
	public synchronized String newCallId()
	{
		return StringUtil.rndString( 16 );
	}

	private final Map<String,CallMonitor> callMonitors = new HashMap<String,CallMonitor>();
}