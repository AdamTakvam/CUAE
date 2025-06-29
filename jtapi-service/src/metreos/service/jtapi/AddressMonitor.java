/* $Id: AddressMonitor.java 31932 2007-05-02 20:06:03Z rvarghee $
 *
 * Created by wert on Feb 17, 2005.
 *
 * Copyright (c) 2005 Metreos, Inc. All rights reserved.
 */

package metreos.service.jtapi;

import java.net.InetAddress;
import java.util.HashMap;
import java.util.Map;

import javax.telephony.Address;
import javax.telephony.CallObserver;
import javax.telephony.Connection;
import javax.telephony.Terminal;
import javax.telephony.TerminalConnection;
import javax.telephony.callcontrol.CallControlCallObserver;
import javax.telephony.callcontrol.events.CallCtlCallEv;
import javax.telephony.events.AddrEv;
import javax.telephony.events.AddrObservationEndedEv;
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

import com.cisco.jtapi.extensions.CiscoAddrInServiceEv;
import com.cisco.jtapi.extensions.CiscoAddrOutOfServiceEv;
import com.cisco.jtapi.extensions.CiscoAddress;
import com.cisco.jtapi.extensions.CiscoAddressObserver;
import com.cisco.jtapi.extensions.CiscoCall;
import com.cisco.jtapi.extensions.CiscoCallEv;
import com.cisco.jtapi.extensions.CiscoCallID;
import com.cisco.jtapi.extensions.CiscoConferenceEndEv;
import com.cisco.jtapi.extensions.CiscoConferenceStartEv;
import com.cisco.jtapi.extensions.CiscoConsultCallActiveEv;
import com.cisco.jtapi.extensions.CiscoMediaTerminal;
import com.cisco.jtapi.extensions.CiscoRTPHandle;
import com.cisco.jtapi.extensions.CiscoSynchronousObserver;
import com.cisco.jtapi.extensions.CiscoTransferEndEv;
import com.cisco.jtapi.extensions.CiscoTransferStartEv;

/**
 * blah
 */
public class AddressMonitor implements CiscoAddressObserver,
	CallObserver, CallControlCallObserver, MediaCallObserver,
	CiscoSynchronousObserver
{
	/**
	 * @param cmMap
	 * @param address
	 * @param tm
	 * @param fakeIp 
	 * @param fakePort 
	 * @param thirdParty 
	 */
	public AddressMonitor( CallMonitorMap cmMap, CiscoAddress address,
		TerminalMonitor tm, InetAddress fakeIp, int fakePort, boolean thirdParty )
	{
		this.cmMap = cmMap;
		this.address = address;
		this.tm = tm;
		this.fakeIp = fakeIp;
		this.fakePort = fakePort;
		this.thirdParty = thirdParty;
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
	
	private final InetAddress fakeIp;
	
	private final int fakePort;
	
	private final boolean thirdParty;

	@Override
	public String toString()
	{
		if (toString0 == null)
			toString0 = "address( " + address + "@" + tm.getTerminalName() + " ) ["+super.toString()+"]";
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
		Trace.report( this, Trace.m( "delivering " ).a( n ).a( " call events" ) );
		for (int i = 0; i < n; i++)
		{
			CallEv event = eventList[i];
			try
			{
				Trace.report( this, Trace.m( "delivering " ).a( event ) );
				if (!callChangedEvent( event ))
					Trace.report( this, Trace.m( "ignoring " ).a( event ) );
			}
			catch ( Exception e )
			{
				Trace.report( this, Trace.m( "caught exception delivering " ).a( event ), e );
			}
		}
		Trace.report( this, Trace.m( "done delivering " ).a( n ).a( " call events" ) );
	}

	private boolean callChangedEvent( CallEv event )
	{
		CiscoCall call = (CiscoCall) event.getCall();
		if (call == null)
		{
			Trace.report( this, Trace.m( "*** ignoring event with null call" ) );
			return false;
		}
		
		if (event instanceof ConnEv)
		{
			ConnEv ev = (ConnEv) event;
			Connection conn = ev.getConnection();
			Address a = conn.getAddress();
			if (!a.equals( address ))
			{
				Trace.report( this, Trace.m( "*** ignoring event about address: " )
					.a( JTapiHelper.toString( ev, ev.getClass().getName() ) ) );
				return false;
			}
		}

		if (event instanceof TermConnEv)
		{
			TermConnEv ev = (TermConnEv) event;
			TerminalConnection termConn = ev.getTerminalConnection();
			Terminal t = termConn.getTerminal();
			if (!t.equals( tm.getTerminal() ))
			{
				Trace.report( this, Trace.m( "*** ignoring event about terminal: " )
					.a( JTapiHelper.toString( ev, ev.getClass().getName() ) ) );
				return false;
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
					cm = new CallMonitor( this, callId, cmMap.newCallId(), call, fakeIp, fakePort, thirdParty );
					addCallMonitor( callId, cm );
					Trace.report( this, Trace.m( "added " ).a( cm ) );
					return true;
				}
				Trace.report( this, Trace.m( "*** ignoring event with call monitor" ) );
				return false;
			}
		}
		else if (event instanceof CallCtlCallEv)
		{
			CallMonitor cm = getCallMonitor( callId );
			CallCtlCallEv ev = (CallCtlCallEv) event;
			return cm.callChangedEvent( ev );
		}
		else if (event instanceof CallInvalidEv)
		{
			CallMonitor cm = getCallMonitor( callId );
			cm.cleanup();
			return true;
		}
		else if (event instanceof CallObservationEndedEv)
		{
			CallMonitor cm = removeCallMonitor( callId );
			Trace.report( this, Trace.m( "removed call " + cm ) );
			return true;
		}
		else if (event instanceof MediaTermConnDtmfEv)
		{
			CallMonitor cm = getCallMonitor( callId );
			MediaTermConnDtmfEv ev = (MediaTermConnDtmfEv) event;
			cm.mediaTermConnDtmfEvent( ev );
			return true;
		}
		else if (event instanceof CiscoCallEv)
		{
			switch (event.getID())
			{
				case CiscoConferenceStartEv.ID:
					ciscoConferenceStart( (CiscoConferenceStartEv) event );
					return true;
				case CiscoConferenceEndEv.ID:
					ciscoConferenceEnd( (CiscoConferenceEndEv) event );
					return true;
				case CiscoConsultCallActiveEv.ID:
					ciscoConsultCallActive( (CiscoConsultCallActiveEv) event );
					return true;
				case CiscoTransferStartEv.ID:
					ciscoTransferStart( (CiscoTransferStartEv) event );
					return true;
				case CiscoTransferEndEv.ID:
					ciscoTransferEnd( (CiscoTransferEndEv) event );
					return true;
				default:
					return false;
			}
		}
		else
		{
			Trace.report( this, Trace.m( "*** ignoring event with no handler" ) );
			return false;
		}
	}

	private void ciscoConferenceStart( CiscoConferenceStartEv ev )
	{
		// TODO Auto-generated method stub
	}

	private void ciscoConferenceEnd( CiscoConferenceEndEv ev )
	{
		// TODO Auto-generated method stub
	}

	private void ciscoConsultCallActive( CiscoConsultCallActiveEv ev )
	{
		// TODO Auto-generated method stub
	}

	private void ciscoTransferStart( CiscoTransferStartEv ev )
	{
		CiscoCall finalCall = (CiscoCall) ev.getFinalCall();
		CallMonitor finalCm = getCallMonitor( finalCall );
		
		Address transferFrom = ev.getTransferControllerAddress();
		
		CiscoCall transferredCall = (CiscoCall) ev.getTransferredCall();
		CallMonitor transCm = getCallMonitor( transferredCall );
		
		Trace.report( this, Trace.m( "transferStart" )
			.a( ", finalCall=" ).a( finalCall )
			.a( ", transferredCall=" ).a( transferredCall )
			.a( ", callFrom=" ).a( finalCm.from )
			.a( ", callTo=" ).a( finalCm.to )
			.a( ", call.getCallingAddress=" ).a( finalCm.call.getCallingAddress() )
			.a( ", call.getCalledAddress=" ).a( finalCm.call.getCalledAddress() )
			.a( ", call.getCurrentCallingAddress=" ).a( finalCm.call.getCurrentCallingAddress() )
			.a( ", call.getCurrentCalledAddress=" ).a( finalCm.call.getCurrentCalledAddress() )
			.a( ", transferFrom=" ).a( transferFrom )
			.a( ", transferredCall.getCallingAddress=" ).a( transferredCall != null ? transferredCall.getCallingAddress() : null )
			.a( ", transferredCall.getCalledAddress=" ).a( transferredCall != null ? transferredCall.getCalledAddress() : null ) );
		
		if (finalCm.from == null || finalCm.to == null)
		{
			// this is a new call (to us) which will be populated with
			// connections and stuff. our problem is that we want it to
			// appear to the appserver to have the id of transferredCall.
			// so, we gotta do two things: swap the id of the two calls,
			// and avoid sending messages to the appserver until the
			// transfer is complete.
			
			transCm.setSilent( true );
			finalCm.setSilent( true );
			finalCm.transCm = transCm;
		}
	}

	private void ciscoTransferEnd( CiscoTransferEndEv ev )
	{
		String me = address.getName();
		
		CiscoCall finalCall = (CiscoCall) ev.getFinalCall();
		CallMonitor finalCm = getCallMonitor( finalCall );
		
		String transferFrom = ev.getTransferControllerAddress().getName();
		
		CiscoCall transferredCall = (CiscoCall) ev.getTransferredCall();
		
		boolean success = ev.isSuccess();
		
		String currentCalling = finalCm.call.getCurrentCallingAddress().getName();
		String currentCalled = finalCm.call.getCurrentCalledAddress().getName();
		
		Trace.report( this, Trace.m( "transferEnd" )
			.a( ", me=" ).a( me )
			.a( ", finalCall=" ).a( finalCall )
			.a( ", transferredCall=" ).a( transferredCall )
			.a( ", callFrom=" ).a( finalCm.from )
			.a( ", callTo=" ).a( finalCm.to )
			.a( ", call.getCallingAddress=" ).a( finalCm.call.getCallingAddress() )
			.a( ", call.getCalledAddress=" ).a( finalCm.call.getCalledAddress() )
			.a( ", call.getCurrentCallingAddress=" ).a( currentCalling )
			.a( ", call.getCurrentCalledAddress=" ).a( currentCalled )
			.a( ", transferFrom=" ).a( transferFrom )
			.a( ", transferredCall.getCallingAddress=" ).a( transferredCall != null ? transferredCall.getCallingAddress() : null )
			.a( ", transferredCall.getCalledAddress=" ).a( transferredCall != null ? transferredCall.getCalledAddress() : null )
			.a( ", success=" ).a( success ) );
		
		if (!success || equals( transferFrom, me ))
		{
			Trace.report( this, Trace.m( "transfer failed or is away from me" ) );
			// wake up finalCm if it is silent.
			finalCm.setSilent( false );
			finalCm.transCallId = null;
			// if transCm is silent we leave it that way.
			// we don't want to hear from it ever.
			return;
		}
		
		if (finalCm.isSilent())
		{
			finalCm.rename( finalCm.transCm.ourCallId );
			
			CiscoRTPHandle handle = finalCm.transCm.getRTPHandle();
			if (handle != null)
				finalCm.setRTPHandle( handle );

			Trace.report( this, Trace.m( "setting silent to false" ) );
			finalCm.setSilent( false );
		}
		
		String target;
		if (equals( currentCalling, me ))
		{
			target = currentCalled;
		}
		else if (equals( currentCalled, me ))
		{
			target = currentCalling;
		}
		else
		{
			Trace.report( this, Trace.m( "i'm not a part of final call" ) );
			return;
		}
		
		if (equals( finalCm.from, transferFrom ))
		{
			Trace.report( this, Trace.m( "transfer changes FROM: " ).a( transferFrom ).a( " -> " ).a( target ) );
			finalCm.setFrom( transferFrom, target, "transfer" );
		}
		else if (equals( finalCm.to, transferFrom ))
		{
			Trace.report( this, Trace.m( "transfer changes TO: " ).a( transferFrom ).a( " -> " ).a( target ) );
			finalCm.setTo( transferFrom, target, "transfer" );
		}
		else
		{
			Trace.report( this, Trace.m( "transferFrom is not a part of this call" ) );
			return;
		}
	}

	private boolean equals( String a, String b )
	{
		// note: should match patterns in a and b?
		return a.equals( b );
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
	
	private CallMonitor getCallMonitor( CiscoCall call )
	{
		if (call == null)
			return null;
		
		return getCallMonitor( call.getCallID() );
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
			try
			{
				Trace.report( this, Trace.m( "---- AddressMonitor::addressChangedEvent for [").a(event.getAddress()).a("] Received " ).a( event ) );
				if (!addressChangedEvent( event ))
					Trace.report( this, Trace.m( "ignored " ).a( event ) );
			}
			catch ( Exception e )
			{
				Trace.report( this, Trace.m( "caught exception delivering " ).a( event ), e );
			}
		}
	}
	
	private boolean addressChangedEvent( AddrEv ev ) throws Exception
	{
		switch (ev.getID())
		{
			case AddrObservationEndedEv.ID:
				return addrObservationEnded( (AddrObservationEndedEv) ev );
			
			case CiscoAddrInServiceEv.ID:
				return ciscoAddrInService( (CiscoAddrInServiceEv) ev );
			
			case CiscoAddrOutOfServiceEv.ID:
				return ciscoAddrOutOfService( (CiscoAddrOutOfServiceEv) ev );
			
			default:
				return false;
		}
	}

	private boolean ciscoAddrInService( CiscoAddrInServiceEv ev )
		throws Exception
	{
		Address a = ev.getAddress();
		Terminal t = ev.getTerminal();
		if (t instanceof CiscoMediaTerminal)
		{
			Trace.report( this, a.getName()+"@"+t.getName()+" online, setting autoaccept status on" );
			((CiscoAddress) a).setAutoAcceptStatus( CiscoAddress.AUTOACCEPT_ON, t );
		}
		return true;
	}

	private boolean ciscoAddrOutOfService( CiscoAddrOutOfServiceEv ev )
	{
		// TODO Auto-generated method stub
		return false;
	}

	private boolean addrObservationEnded( AddrObservationEndedEv ev )
	{
		// TODO Auto-generated method stub
		return false;
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