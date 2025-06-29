/* $Id: AddressMonitor.java 30152 2007-03-06 21:47:50Z wert $
 *
 * Created by wert on Feb 17, 2005.
 *
 * Copyright (c) 2005 Metreos, Inc. All rights reserved.
 */

package metreos.service.jtapi;

import java.util.HashMap;
import java.util.Map;

import javax.telephony.Address;
import javax.telephony.AddressObserver;
import javax.telephony.CallObserver;
import javax.telephony.Connection;
import javax.telephony.Terminal;
import javax.telephony.TerminalConnection;
import javax.telephony.callcontrol.CallControlCallObserver;
import javax.telephony.callcontrol.events.CallCtlCallEv;
import javax.telephony.events.AddrEv;
import javax.telephony.events.CallActiveEv;
import javax.telephony.events.CallEv;
import javax.telephony.events.CallInvalidEv;
import javax.telephony.events.CallObservationEndedEv;
import javax.telephony.events.ConnEv;
import javax.telephony.events.TermConnEv;
import javax.telephony.media.MediaCallObserver;
import javax.telephony.media.events.MediaTermConnDtmfEv;

import metreos.core.ipc.flatmaps.FlatmapIpcMessage;
import metreos.util.Trace;

import com.cisco.jtapi.extensions.CiscoAddress;
import com.cisco.jtapi.extensions.CiscoCall;
import com.cisco.jtapi.extensions.CiscoCallID;


/**
 * blah
 */
public class AddressMonitor implements CallObserver,
	CallControlCallObserver, AddressObserver, MediaCallObserver
{
	/**
	 * @param cmMap
	 * @param address
	 * @param tm
	 */
	public AddressMonitor( CallMonitorMap cmMap, CiscoAddress address,
		TerminalMonitor tm )
	{
		this.cmMap = cmMap;
		this.address = address;
		this.tm = tm;
	}

	/**
	 * blah.
	 */
	public final CallMonitorMap cmMap;

	/**
	 * Comment for <code>address</code>
	 */
	public final CiscoAddress address;

	/**
	 * Comment for <code>tm</code>
	 */
	public final TerminalMonitor tm;

	@Override
	public String toString()
	{
		if (toString0 == null)
			toString0 = "address( " + address + "@" + tm.getTerminal() + " ) ["+super.toString()+"]";
		return toString0;
	}
	
	private String toString0;

	/*
	 * (non-Javadoc)
	 * 
	 * @see javax.telephony.CallObserver#callChangedEvent(javax.telephony.events.CallEv[])
	 */
	public void callChangedEvent( CallEv[] eventList )
	{
		int n = eventList.length;
		for (int i = 0; i < n; i++)
		{
			try
			{
				callChangedEvent( eventList[i] );
			}
			catch ( Exception e )
			{
				Trace.report( this, Trace.m( "caught exception delivering "
					).a( eventList[i] ), e );
			}
		}
	}

	private void callChangedEvent( CallEv event )
	{
		CiscoCall call = (CiscoCall) event.getCall();
		if (call == null)
			return;

		if (event instanceof ConnEv)
		{
			ConnEv ev = (ConnEv) event;
			Connection conn = ev.getConnection();
			Address a = conn.getAddress();
			if (!a.equals( address ))
			{
//				Trace.report( this, "*** ignoring event about address " + a
//					+ ": "
//					+ JTapiHelper.toString( ev, ev.getClass().getName() ) );
				return;
			}
		}

		if (event instanceof TermConnEv)
		{
			TermConnEv ev = (TermConnEv) event;
			TerminalConnection termConn = ev.getTerminalConnection();
			Terminal t = termConn.getTerminal();
			if (!t.equals( tm.getTerminal() ))
			{
//				Trace.report( this, "*** ignoring event about terminal "
//					+ t + ": "
//					+ JTapiHelper.toString( ev, ev.getClass().getName() ) );
				return;
			}
		}

		CiscoCallID callId = call.getCallID();
		if (event instanceof CallActiveEv)
		{
			synchronized (calls)
			{
				CallMonitor cm = getCallMonitor( callId );
				if (cm == null)
				{
					Trace.report( this, Trace.m( JTapiHelper.toString( event,
						event.getClass().getName() ) ) );
					cm = new CallMonitor( this, callId, cmMap.newCallId(), call );
					addCallMonitor( callId, cm );
					Trace.report( this, Trace.m( "added " ).a( cm ) );
				}
				else
				{
					Trace.report( this, Trace.m( "*** IGNORED *** " ).a( JTapiHelper.toString( event,
						event.getClass().getName() ) ) );
				}
			}
		}
		else if (event instanceof CallCtlCallEv)
		{
			CallMonitor cm = getCallMonitor( callId );
			CallCtlCallEv ev = (CallCtlCallEv) event;
			cm.callChangedEvent( ev );
		}
		else if (event instanceof CallInvalidEv)
		{
			CallMonitor cm = getCallMonitor( callId );
			cm.cleanup();
		}
		else if (event instanceof CallObservationEndedEv)
		{
			CallMonitor cm = removeCallMonitor( callId );
			Trace.report( this, Trace.m( "removed call " + cm ) );
		}
		else if (event instanceof MediaTermConnDtmfEv)
		{
			CallMonitor cm = getCallMonitor( callId );
			MediaTermConnDtmfEv ev = (MediaTermConnDtmfEv) event;
			cm.mediaTermConnDtmfEvent( ev );
		}
		else
		{
			// ignore event.
			Trace.report( this, Trace.m( "*** ignoring event about call " + callId + ": "
					+ JTapiHelper.toString( event, event.getClass().getName() ) ) );
		}
	}
	
	private void addCallMonitor( CiscoCallID callId, CallMonitor cm )
	{
		synchronized (calls)
		{
//			Trace.report( Trace.m( calls ) );
			calls.put( callId.toString(), cm );
			cmMap.add( cm );
		}
	}
	
	private CallMonitor getCallMonitor( CiscoCallID callId )
	{
		synchronized (calls)
		{
//			Trace.report( Trace.m( calls ) );
			return calls.get( callId.toString() );
		}
	}
	
	private CallMonitor removeCallMonitor( CiscoCallID callId )
	{
		synchronized (calls)
		{
//			Trace.report( Trace.m( calls ) );
			CallMonitor cm = calls.remove( callId.toString() );
			cmMap.remove( cm );
			return cm;
		}
	}
	
	private Map<String,CallMonitor> calls = new HashMap<String,CallMonitor>();

	/*
	 * (non-Javadoc)
	 * 
	 * @see javax.telephony.AddressObserver#addressChangedEvent(javax.telephony.events.AddrEv[])
	 */
	public void addressChangedEvent( AddrEv[] eventList )
	{
		int n = eventList.length;
		for (int i = 0; i < n; i++)
		{
			AddrEv event = eventList[i];
			Trace.report( this, Trace.m( address.getName() ).a( ": got event: " ).a( event ) );
		}
	}

	/**
	 * @param messageType
	 * @return a new flatmap ipc message
	 */
	public FlatmapIpcMessage newMessage( int messageType )
	{
		return tm.newMessage( messageType );
	}
}