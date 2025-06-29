/* $Id: PlainTerminalMonitor.java 30152 2007-03-06 21:47:50Z wert $
 *
 * Created by wert on Feb 17, 2005.
 *
 * Copyright (c) 2005 Metreos, Inc. All rights reserved.
 */

package metreos.service.jtapi;

import javax.telephony.Terminal;
import javax.telephony.TerminalObserver;
import javax.telephony.events.TermEv;

import metreos.core.ipc.flatmaps.FlatmapIpcMessage;
import metreos.util.Trace;

import com.cisco.jtapi.extensions.CiscoRTPInputStartedEv;
import com.cisco.jtapi.extensions.CiscoRTPInputStoppedEv;
import com.cisco.jtapi.extensions.CiscoRTPOutputStartedEv;
import com.cisco.jtapi.extensions.CiscoRTPOutputStoppedEv;
import com.cisco.jtapi.extensions.CiscoTerminal;

/**
 * blah.
 */
public class PlainTerminalMonitor implements TerminalMonitor, TerminalObserver
{
	/**
	 * @param name
	 * @param terminal
	 */
	public PlainTerminalMonitor( String name, CiscoTerminal terminal )
	{
		this.name = name;
		this.terminal = terminal;
	}

	/**
	 * Comment for <code>name</code>
	 */
	public final String name;

	/**
	 * Comment for <code>terminal</code>
	 */
	public final Terminal terminal;

	/**
	 * Comment for <code>cm</code>
	 */
	private CallMonitor cm;

	@Override
	public String toString()
	{
		if (toString0 == null)
			toString0 = "terminal( " + terminal + " )";
		return toString0;
	}
	
	private String toString0;

	/*
	 * (non-Javadoc)
	 * 
	 * @see javax.telephony.TerminalObserver#terminalChangedEvent(javax.telephony.events.TermEv[])
	 */
	public void terminalChangedEvent( TermEv[] eventList )
	{
		int n = eventList.length;
		for (int i = 0; i < n; i++)
		{
			TermEv event = eventList[i];
			try
			{
				terminalChangedEvent( event );
			}
			catch ( Exception e )
			{
				Trace.report( this, Trace.m( "caught exception delivering "
					).a( eventList[i] ), e );
			}
		}
	}

	private synchronized void terminalChangedEvent( TermEv event )
	{
		switch (event.getID())
		{
			case CiscoRTPInputStartedEv.ID:
				if (cm != null)
					cm.ciscoRTPInputStarted( (CiscoRTPInputStartedEv) event );
				break;

			case CiscoRTPInputStoppedEv.ID:
				if (cm != null)
					cm.ciscoRTPInputStopped( (CiscoRTPInputStoppedEv) event );
				break;

			case CiscoRTPOutputStartedEv.ID:
				if (cm != null)
					cm.ciscoRTPOutputStarted( (CiscoRTPOutputStartedEv) event );
				break;

			case CiscoRTPOutputStoppedEv.ID:
				if (cm != null)
					cm.ciscoRTPOutputStopped( (CiscoRTPOutputStoppedEv) event );
				break;

			default:
				Trace.report( this, Trace.m( "event not handled: " ).a( event ) );
				break;
		}
	}

	/**
	 * @param newCm
	 * @param active
	 */
	public synchronized void addCallMonitor( CallMonitor newCm, boolean active )
	{
		if (active)
			cm = newCm;
		else
			cm = null;
	}

	/* (non-Javadoc)
	 * @see service.TerminalMonitor#removeCallMonitor(service.CallMonitor)
	 */
	public synchronized void removeCallMonitor( CallMonitor monitor )
	{
		if (cm == monitor)
			cm = null;
	}

	/* (non-Javadoc)
	 * @see service.TerminalMonitor#getTerminal()
	 */
	public Terminal getTerminal()
	{
		return terminal;
	}

	/* (non-Javadoc)
	 * @see service.TerminalMonitor#newMessage(int)
	 */
	public FlatmapIpcMessage newMessage( int messageType )
	{
		// not needed for standalone fooling.
		return null;
	}
}