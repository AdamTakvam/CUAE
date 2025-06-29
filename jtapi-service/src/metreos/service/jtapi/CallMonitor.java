/* $Id: CallMonitor.java 32215 2007-05-16 19:51:38Z wert $
 *
 * Created by wert on Feb 17, 2005.
 *
 * Copyright (c) 2005 Metreos, Inc. All rights reserved.
 */

package metreos.service.jtapi;

import java.net.InetAddress;
import java.util.HashSet;
import java.util.Iterator;
import java.util.Set;

import javax.telephony.Address;
import javax.telephony.CallObserver;
import javax.telephony.InvalidStateException;
import javax.telephony.Terminal;
import javax.telephony.TerminalConnection;
import javax.telephony.callcontrol.CallControlCallObserver;
import javax.telephony.callcontrol.CallControlConnection;
import javax.telephony.callcontrol.events.CallCtlCallEv;
import javax.telephony.callcontrol.events.CallCtlConnAlertingEv;
import javax.telephony.callcontrol.events.CallCtlConnDialingEv;
import javax.telephony.callcontrol.events.CallCtlConnDisconnectedEv;
import javax.telephony.callcontrol.events.CallCtlConnEstablishedEv;
import javax.telephony.callcontrol.events.CallCtlConnEv;
import javax.telephony.callcontrol.events.CallCtlConnFailedEv;
import javax.telephony.callcontrol.events.CallCtlConnInitiatedEv;
import javax.telephony.callcontrol.events.CallCtlConnOfferedEv;
import javax.telephony.callcontrol.events.CallCtlTermConnBridgedEv;
import javax.telephony.callcontrol.events.CallCtlTermConnDroppedEv;
import javax.telephony.callcontrol.events.CallCtlTermConnEv;
import javax.telephony.callcontrol.events.CallCtlTermConnHeldEv;
import javax.telephony.callcontrol.events.CallCtlTermConnInUseEv;
import javax.telephony.callcontrol.events.CallCtlTermConnRingingEv;
import javax.telephony.callcontrol.events.CallCtlTermConnTalkingEv;
import javax.telephony.events.CallEv;
import javax.telephony.events.CallInvalidEv;
import javax.telephony.media.MediaTerminalConnection;
import javax.telephony.media.capabilities.MediaTerminalConnectionCapabilities;
import javax.telephony.media.events.MediaTermConnDtmfEv;

import metreos.core.ipc.flatmaps.FlatmapIpcMessage;
import metreos.util.Assertion;
import metreos.util.Trace;

import com.cisco.jtapi.extensions.CiscoCall;
import com.cisco.jtapi.extensions.CiscoCallID;
import com.cisco.jtapi.extensions.CiscoConnection;
import com.cisco.jtapi.extensions.CiscoMediaOpenLogicalChannelEv;
import com.cisco.jtapi.extensions.CiscoRTPHandle;
import com.cisco.jtapi.extensions.CiscoRTPInputStartedEv;
import com.cisco.jtapi.extensions.CiscoRTPInputStoppedEv;
import com.cisco.jtapi.extensions.CiscoRTPOutputProperties;
import com.cisco.jtapi.extensions.CiscoRTPOutputStartedEv;
import com.cisco.jtapi.extensions.CiscoRTPOutputStoppedEv;
import com.cisco.jtapi.extensions.CiscoRTPParams;
import com.cisco.jtapi.extensions.CiscoSynchronousObserver;
import com.cisco.jtapi.extensions.CiscoTermButtonPressedEv;
import com.cisco.jtapi.extensions.CiscoTerminalConnection;


/**
 * Monitor a call for a particular address monitor.
 */
public class CallMonitor
{
	/**
	 * @param am
	 * @param ciscoCallId
	 * @param call
	 * @param ourCallId
	 * @param fakeIp 
	 * @param fakePort 
	 * @param thirdParty 
	 */
	public CallMonitor( AddressMonitor am, CiscoCallID ciscoCallId,
		String ourCallId, CiscoCall call, InetAddress fakeIp, int fakePort,
		boolean thirdParty )
	{
		this.am = am;
		this.ciscoCallId = ciscoCallId;
		this.ourCallId = ourCallId;
		this.call = call;
		this.mohIp = fakeIp;
		this.mohPort = fakePort;
		this.thirdParty = thirdParty;
	}

	/**
	 * The address monitor which saw the call.
	 */
	public final AddressMonitor am;

	/**
	 * The cisco assigned call id.
	 */
	public final CiscoCallID ciscoCallId;

	/**
	 * The jtapi server assigned call id.
	 */
	public String ourCallId;

	/**
	 * Description of call.
	 */
	final CiscoCall call;
	
	private final InetAddress mohIp;
	
	private final int mohPort;
	
	private final boolean thirdParty;
	
	/**
	 * Description of conn.
	 */
	CiscoConnection conn;
	
	/**
	 * The connection address name of conn.
	 */
	String connAddress;

	/**
	 * Description of termConn.
	 */
	CiscoTerminalConnection termConn;
	
	/**
	 * The terminal name of termConn.
	 */
	String termConnTerminal;
	
	private Boolean dtmfDetection = null;
	
	private boolean holdPending = false;
	
	private boolean hasLogicalChannel;
	
	/**
	 * Description of from.
	 */
	String from;
	
	/**
	 * Description of to
	 */
	String to;
	
	private String originalTo;

	private CiscoRTPHandle rtpHandle;

	/**
	 * Description of rtpParams.
	 */
	public CiscoRTPParams rtpParams;
	
	/**
	 * Description of lastRTPParams.
	 */
	public CiscoRTPParams lastRTPParams;
	
	//private boolean rtpParamsSet;
	
	//private boolean dontNotifyTheAppServer;
	
	private int disconnectCause = 0;

	private int disconnectCallControlCause = 0;
	
	private int failCause = 0;
	
	private int failCallControlCause = 0;

	private int origin = 0;

	private int dropCause = 0;
	
	private int dropCallControlCause = 0;

	private CiscoRTPOutputProperties pendingRTPOutputProperties;
	
	/**
	 * Description of MSG_LOCK.
	 */
	public final Object MSG_LOCK = new Object();

	@Override
	public String toString()
	{
		StringBuffer sb = new StringBuffer();
		
		if (origin == INITIATED)
			sb.append( "initiated " );
		else if (origin == OFFERED)
			sb.append( "offered " );
		
		sb.append( "call" );

		sb.append( ' ' );
		sb.append( xlatConnState( connState ) );
		sb.append( '/' );
		sb.append( xlatTermConnState( termConnState ) );

		sb.append( ' ' );
		sb.append( ourCallId );
		
		sb.append( ' ' );
		sb.append( ciscoCallId );
		
		if (connAddress != null)
		{
			sb.append( ' ' );
			sb.append( connAddress );
		}
		
		if (termConnTerminal != null)
		{
			sb.append( ' ' );
			sb.append( termConnTerminal );
		}
		
		return sb.toString();
	}
	
	/**
	 * @param m
	 */
	void report( Trace.M m )
	{
		Trace.report( this, m );
	}
	
	private void report( Trace.M m, Throwable t )
	{
		Trace.report( this, m, t );
	}
	
	/**
	 * @param t
	 */
	void report( Throwable t )
	{
		Trace.report( this, t );
	}
	
	private void reportAndSend( FlatmapIpcMessage msg )
	{
		Trace.report( this, Trace.m( "sending message: " ).a( msg ) );
		msg.send();
	}

	/**
	 * @param ev
	 * @return true if delivered, false otherwise
	 */
	public boolean callChangedEvent( CallCtlCallEv ev )
	{
		try
		{
			Trace.report( this, Trace.m( "delivering " ).a( ev ) );
			if (!callChangedEvent0( ev ))
				Trace.report( this, Trace.m( "ignored " ).a( ev ) );
			return true;
		}
		catch ( Exception e )
		{
			Trace.report( this, Trace.m( "caught exception delivering " ).a( ev ), e );
			// if we got an exception we must have been trying to deliver it
			return true;
		}
	}

	/**
	 * @param ev
	 */
	public void mediaTermConnDtmfEvent( MediaTermConnDtmfEv ev )
	{
		char digit = ev.getDtmfDigit();
		
		StringBuffer sb = new StringBuffer( 1 );
		sb.append( digit );
		String digits = sb.toString();
		
		report( Trace.m( "got digits " ).a( digits ) );
		
		sendGotDigits( digits, 1 );
	}
	
	private boolean callChangedEvent0( CallCtlCallEv ev )
		throws Exception
	{
		if (ev instanceof CallCtlTermConnEv)
			report( Trace.m( JTapiHelper.toString( (CallCtlTermConnEv) ev,
				ev.getClass().getName() ) ) );
		else if (ev instanceof CallCtlConnEv)
			report( Trace.m( JTapiHelper.toString( (CallCtlConnEv) ev,
				ev.getClass().getName() ) ) );
		else
			report( Trace.m( JTapiHelper.toString( ev,
				ev.getClass().getName() ) ) );

		switch (ev.getID())
		{
			///////////////////
			// CallCtlConnEv //
			///////////////////

			case CallCtlConnAlertingEv.ID:
				callCtlConnAlerting( (CallCtlConnAlertingEv) ev );
				return true;

			case CallCtlConnDialingEv.ID:
				callCtlConnDialing( (CallCtlConnDialingEv) ev );
				return true;

			case CallCtlConnDisconnectedEv.ID:
				callCtlConnDisconnected( (CallCtlConnDisconnectedEv) ev );
				return true;

			case CallCtlConnEstablishedEv.ID:
				callCtlConnEstablished( (CallCtlConnEstablishedEv) ev );
				return true;

			case CallCtlConnFailedEv.ID:
				callCtlConnFailed( (CallCtlConnFailedEv) ev );
				return true;

			case CallCtlConnInitiatedEv.ID:
				callCtlConnInitiated( (CallCtlConnInitiatedEv) ev );
				return true;

			case CallCtlConnOfferedEv.ID:
				callCtlConnOffered( (CallCtlConnOfferedEv) ev );
				return true;

			///////////////////////
			// CallCtlTermConnEv //
			///////////////////////

			case CallCtlTermConnBridgedEv.ID:
				callCtlTermConnBridged( (CallCtlTermConnBridgedEv) ev );
				return true;

			case CallCtlTermConnDroppedEv.ID:
				callCtlTermConnDropped( (CallCtlTermConnDroppedEv) ev );
				return true;

			case CallCtlTermConnHeldEv.ID:
				callCtlTermConnHeld( (CallCtlTermConnHeldEv) ev );
				return true;

			case CallCtlTermConnInUseEv.ID:
				callCtlTermConnInUse( (CallCtlTermConnInUseEv) ev );
				return true;

			case CallCtlTermConnRingingEv.ID:
				callCtlTermConnRinging( (CallCtlTermConnRingingEv) ev );
				return true;

			case CallCtlTermConnTalkingEv.ID:
				callCtlTermConnTalking( (CallCtlTermConnTalkingEv) ev );
				return true;
			
			default:
				return false;
		}
	}

	/////////////////////////
	// CONN EVENT HANDLERS //
	/////////////////////////

	/**
	 * @param ev
	 */
	private void callCtlConnAlerting( CallCtlConnAlertingEv ev )
	{
		setConn( ev );
		changeConnState( OFFERED, ALERTING );
	}

	/**
	 * @param ev
	 */
	private void callCtlConnDialing( CallCtlConnDialingEv ev )
	{
		setConn( ev );
		changeConnState( INITIATED, DIALING );
	}

	/**
	 * This connection has been disconnected.
	 * @param ev
	 */
	private void callCtlConnDisconnected( CallCtlConnDisconnectedEv ev )
	{
		conn = null;
		disconnectCause = ev.getCause();
		disconnectCallControlCause = ev.getCallControlCause();
		changeConnState( CONN_STATES, DISCONNECTED );
	}

	/**
	 * Our connection is established.
	 * @param ev
	 */
	private void callCtlConnEstablished( CallCtlConnEstablishedEv ev )
	{
		setConn( ev );
		changeConnState( INACTIVE|INITIATED|ALERTING|DIALING, ESTABLISHED );
	}

	private void setConn( CallCtlConnEv ev )
	{
		CiscoConnection c = (CiscoConnection) ev.getConnection();
		if (conn != c)
		{
			conn = c;
			connAddress = conn.getAddress().getName();
		}
		xsetupAddrs( ev );
	}

	/**
	 * @param ev
	 */
	private void callCtlConnFailed( CallCtlConnFailedEv ev )
	{
		conn = null;
		failCause = ev.getCause();
		failCallControlCause = ev.getCallControlCause();
		changeConnState( CONN_STATES, DISCONNECTED );
	}

	/**
	 * We started this connection.
	 * @param ev
	 */
	private void callCtlConnInitiated( CallCtlConnInitiatedEv ev )
		throws Exception
	{
		setConn( ev );
		origin = INITIATED;
		CallData cd = (CallData) call.getObject();
		if (cd != null)
		{
			synchronized (cd)
			{
				if (cd.fromCallId != null)
				{
					rename( cd.fromCallId );
					
					setRTPIpPort( cd.rxIp, cd.rxPort, true, false );
					
					cd.fromCallId = null;
					cd.rxIp = null;
					cd.rxPort = 0;
				}
			}
		}
		
		try
		{
			call.addObserver( new AnswerObserver() );
		}
		catch ( Exception e )
		{
			Trace.report( this, e );
		}
		
		changeConnState( INACTIVE, INITIATED );
	}
	
	/**
	 * Description of AnswerObserver
	 */
	public class AnswerObserver implements CallObserver, CallControlCallObserver, CiscoSynchronousObserver
	{
		/* (non-Javadoc)
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
					report( e );
				}
			}
		}

		private Set<String> alerting = new HashSet<String>();
		private Set<String> answered = new HashSet<String>();
		
		private void callChangedEvent( CallEv event )
		{
			if (event instanceof CallCtlTermConnEv)
				report( Trace.m( "===== " ).a( JTapiHelper.toString( (CallCtlTermConnEv) event,
					event.getClass().getName() ) ) );
			else if (event instanceof CallCtlConnEv)
				report( Trace.m( "===== " ).a( JTapiHelper.toString( (CallCtlConnEv) event,
					event.getClass().getName() ) ) );
			else
				report( Trace.m( "===== " ).a( JTapiHelper.toString( event,
					event.getClass().getName() ) ) );

			if (event instanceof CallCtlConnOfferedEv)
			{
				CallCtlConnOfferedEv ev = (CallCtlConnOfferedEv) event;
//				CiscoConnection c = (CiscoConnection) ev.getConnection();
//				Address connAddress = c.getAddress();
//				String connAddressName = connAddress.getName();
				Address calledAddress = ev.getCalledAddress();
				String calledAddressName = calledAddress.getName();
				report( Trace.m( "adding " ).a( calledAddressName ).a( " to alerting" ) );
				alerting.add( calledAddressName );
			}
			else if (event instanceof CallCtlConnAlertingEv)
			{
				CallCtlConnAlertingEv ev = (CallCtlConnAlertingEv) event;
//				CiscoConnection c = (CiscoConnection) ev.getConnection();
//				Address connAddress = c.getAddress();
//				String connAddressName = connAddress.getName();
				Address calledAddress = ev.getCalledAddress();
				String calledAddressName = calledAddress.getName();
				report( Trace.m( "adding " ).a( calledAddressName ).a( " to alerting" ) );
				alerting.add( calledAddressName );
			}
			else if (event instanceof CallCtlConnEstablishedEv)
			{
				CallCtlConnEstablishedEv ev = (CallCtlConnEstablishedEv) event;
				CiscoConnection c = (CiscoConnection) ev.getConnection();
				Address xconnAddress = c.getAddress();
				String connAddressName = xconnAddress.getName();
				Address xCalledAddress = ev.getCalledAddress();
				String calledAddress = xCalledAddress != null ? xCalledAddress.getName() : "";
//				Address calledAddress = ev.getCalledAddress();
//				String calledAddressName = calledAddress.getName();
				if (contains( alerting, connAddressName ) && !contains( answered, connAddressName ))
				{
					report( Trace.m( "===== adding " ).a( connAddressName ).a( " to answered" ) );
					answered.add( connAddressName );
					callAnswered( connAddressName, connAddressName );
				}
				else if (contains( alerting, calledAddress ) && !contains( answered, calledAddress ))
				{
					// handle the case 
					report( Trace.m( "===== adding " ).a( calledAddress ).a( " to answered at " ).a ( calledAddress ) );
					answered.add( calledAddress );
					answered.add( connAddressName );
					callAnswered( connAddressName, calledAddress );
				}
				else if (!contains( alerting, connAddressName ))
				{
					report( Trace.m( "===== address " ).a( connAddressName ).a( " NOT alerting" ) );
				}
				else if (contains( answered, connAddressName ))
				{
					report( Trace.m( "===== address " ).a( connAddressName ).a( " ALREADY answered" ) );
				}
				else
				{
					report( Trace.m( "===== address " ).a( connAddressName ).a( " STATE UNKNOWN" ) );
				}
			}
			else if (event instanceof CallInvalidEv)
			{
				CallInvalidEv ev = (CallInvalidEv) event;
				ev.getCall().removeObserver( this );
			}
		}

		private boolean contains( Set<String> patternSet, String pattern )
		{
			if (patternSet.contains( pattern ))
				return true;
			
			Iterator<String> i = patternSet.iterator();
			while (i.hasNext())
			{
				if (matches( i.next(), pattern ))
					return true;
			}
			
			return false;
		}

		private boolean matches( String pat1, String pat2 )
		{
			// this is where some fool might try to
			// match two patterns to see if they match.
			// for now, i'm only considering patterns
			// using X to match a single digit. Patterns
			// with characters other than digits or X will
			// cause an exception.
			int n1 = pat1.length();
			int n2 = pat2.length();
			if (n1 != n2)
				return false;
			
			for (int i = 0; i < n1; i++)
				if (!match( pat1.charAt( i ), pat2.charAt( i ) ))
					return false;
			return true;
		}

		private boolean match( char a, char b )
		{
			check( a );
			check( b );
			if (a == b)
				return true;
			if (a == 'X' || b == 'X')
				return true;
			if (a == 'x' || b == 'x')
				return true;
			return false;
		}

		private void check( char c )
		{
			if (c >= '0' && c <= '9')
				return;
			
			if (c == 'X' || c == 'x')
				return;
			
			throw new IllegalArgumentException( "bad pattern char '"+c+"'" );
		}
	}
	
	/**
	 * Reports that the call has been answered.
	 * @param who the number of the device that answered.
	 * @param at the number that was originally dialed
	 */
	void callAnswered( String who, String at )
	{
		report( Trace.m( "answered " ).a( who ).a( " at " ).a( at ) );
		sendAnswered( who, at );
	}

	/**
	 * We are being offered a connection.
	 * @param ev
	 */
	private void callCtlConnOffered( CallCtlConnOfferedEv ev )
	{
		setConn( ev );
		origin = OFFERED;
		CallData cd = (CallData) call.getObject();
		if (cd != null)
		{
			synchronized (cd)
			{
				if (cd.toCallId != null)
				{
					rename( cd.toCallId );
					cd.toCallId = null;
				}
			}
		}
		changeConnState( INACTIVE, OFFERED );
	}

	//////////////////////////////
	// TERM CONN EVENT HANDLERS //
	//////////////////////////////

	/**
	 * @param ev
	 */
	private void callCtlTermConnBridged( CallCtlTermConnBridgedEv ev )
	{
		setTermConn( ev.getTerminalConnection() );
		am.tm.addCallMonitor( this, false );
		changeTermConnState( IDLE|RINGING|TALKING|HELD, BRIDGED );
	}

	/**
	 * @param tc
	 */
	private void setTermConn( TerminalConnection tc )
	{
		if (termConn != tc)
		{
			termConn = (CiscoTerminalConnection) tc;
			if (termConn != null)
			{
				Terminal t = tc.getTerminal();
				if (t != null)
					termConnTerminal = t.getName();
			}
			dtmfDetection = null;
		}
		
		if (dtmfDetection == null)
			dtmfDetection = setDtmfDetection( tc );
	}

	private Boolean setDtmfDetection( TerminalConnection tc )
	{
		if (tc != null)
		{
			if (tc instanceof MediaTerminalConnection)
			{
				MediaTerminalConnection mtc = (MediaTerminalConnection) tc;
				
				MediaTerminalConnectionCapabilities caps =
					(MediaTerminalConnectionCapabilities) mtc.getCapabilities();
				if (caps.canDetectDtmf())
				{
					int state = mtc.getMediaAvailability();
					if (state == MediaTerminalConnection.AVAILABLE)
					{
						try
						{
							report( Trace.m( "calling mtc.setDtmfDetection( true )" ) );
							mtc.setDtmfDetection( true );
							report( Trace.m( "mtc.setDtmfDetection( true ) worked" ) );
							return Boolean.TRUE;
						}
						catch ( Exception e )
						{
							report( Trace.m( "no dtmf detection, mtc.setDtmfDetection( true ) failed" ), e );
							return Boolean.FALSE;
						}
					}
					report( Trace.m( "no dtmf detection, media not available" ) );
					return null;
				}
				report( Trace.m( "no dtmf detection, caps indicates cannot detect" ) );
				return Boolean.FALSE;
			}
			report( Trace.m( "no dtmf detection, not a media terminal" ) );
			return Boolean.FALSE;
		}
		return null;
	}

	/**
	 * Our terminal connection is dropped.
	 * @param ev
	 */
	private void callCtlTermConnDropped( CallCtlTermConnDroppedEv ev )
	{
		setTermConn( null );
		am.tm.removeCallMonitor( this );
		dropCause = ev.getCause();
		dropCallControlCause = ev.getCallControlCause();
		changeTermConnState( ~DROPPED, DROPPED );
	}

	/**
	 * Our terminal connection is on hold.
	 * @param ev
	 */
	private void callCtlTermConnHeld( CallCtlTermConnHeldEv ev )
	{
		setTermConn( ev.getTerminalConnection() );
		am.tm.addCallMonitor( this, false );
		changeTermConnState( TALKING|BRIDGED|INUSE, HELD );
	}

	/**
	 * @param ev
	 */
	private void callCtlTermConnInUse( CallCtlTermConnInUseEv ev )
	{
		setTermConn( ev.getTerminalConnection() );
		am.tm.addCallMonitor( this, false );
		changeTermConnState( RINGING|HELD|BRIDGED, INUSE );
	}

	/**
	 * Our terminal connection is ringing.
	 * @param ev
	 */
	private void callCtlTermConnRinging( CallCtlTermConnRingingEv ev )
	{
		setTermConn( ev.getTerminalConnection() );
		am.tm.addCallMonitor( this, true );
		changeTermConnState( IDLE, RINGING );
	}

	/**
	 * Our terminal connection is talking.
	 * @param ev
	 */
	private void callCtlTermConnTalking( CallCtlTermConnTalkingEv ev )
	{
		setTermConn( ev.getTerminalConnection() );
		am.tm.addCallMonitor( this, true );
		changeTermConnState( IDLE|RINGING|HELD, TALKING );
	}

	/////////////////////////////
	// terminal changed events //
	/////////////////////////////

	/**
	 * We are being asked to supply media for the current terminal connection.
	 * @param ev
	 */
	public void ciscoMediaOpenLogicalChannel(
		CiscoMediaOpenLogicalChannelEv ev )
	{
		report( Trace.m( JTapiHelper.toString( ev, ev.getClass().getName() ) ).a( ": hasLogicalChannel" ) );
		
//		int payloadType = ev.getPayLoadType();
//		int packetSize = ev.getPacketSize();
		
		setRTPHandle( ev.getCiscoRTPHandle() );
	}
	
	/**
	 * @return the current pending rtpHandle, if any
	 */
	public CiscoRTPHandle getRTPHandle()
	{
		return rtpHandle;
	}

	/**
	 * @param handle
	 * @return the current rtp handle, if any
	 */
	public boolean setRTPHandle( CiscoRTPHandle handle )
	{
		report( Trace.m( "setRTPHandle to " ).a( handle ) );
		hasLogicalChannel = true;
		rtpHandle = handle;
		lastRTPParams = null;
		return setRTPParams();
	}
	
	private boolean isWaitingForMedia()
	{
		return rtpHandle != null;
	}

	/**
	 * @param newRxIp
	 * @param newRxPort
	 * @param callSetRTPParams
	 */
	private boolean setRTPIpPort( InetAddress newRxIp, int newRxPort,
		boolean callSetRTPParams, boolean fake )
	{
		report( Trace.m( "setRTPIpPort to " )
			.a( newRxIp ).a( "/" ).a( newRxPort )
			.a( ", callSetRTPParams " ).a( callSetRTPParams )
			.a( ", fake " ).a( fake ) );
		
		if (newRxIp != null && newRxPort != 0)
			rtpParams = new CiscoRTPParams( newRxIp, newRxPort );
		else
			rtpParams = null;
		
		fakeMediaSet = fake;
		
		if (callSetRTPParams)
			return setRTPParams();
		
		return false;
	}

	private boolean setRTPParams()
	{
		if (rtpHandle == null || rtpParams == null)
		{
			report( Trace.m( "setRTPParams but no handle or no media:" )
//				.a( " rtpHandle " ).a( rtpHandle )
//				.a( " rtpParams " ).a( rtpParams )
				);
			return false;
		}
		
		try
		{
			report( Trace.m( "setRTPParams:" )
//				.a( " rtpHandle " ).a( rtpHandle )
//				.a( " rtpParams " ).a( rtpParams )
				);
			
			if (am.tm.setRTPParams( rtpHandle, rtpParams ))
			{
				lastRTPParams = rtpParams;
				
				report( Trace.m( "setRTPParams done:" )
//					.a( " rtpHandle " ).a( rtpHandle )
//					.a( " rtpParams " ).a( rtpParams )
					);
				
				return true;
			}
			// setRTPParams failed because of platform exception
			report( Trace.m( "*** setRTPParams failed:" )
//				.a( " rtpHandle " ).a( rtpHandle )
//				.a( " rtpParams " ).a( rtpParams )
				);
			return false;
		}
		catch ( Exception e )
		{
			report( Trace.m( "*** setRTPParams failed:" )
//				.a( " rtpHandle " ).a( rtpHandle )
//				.a( " rtpParams " ).a( rtpParams )
				, e );
			
			return false;
		}
		finally
		{
			rtpHandle = null;
			rtpParams = null;
		}
	}

	/**
	 * Input has started on the current terminal connection.
	 * @param ev
	 */
	public void ciscoRTPInputStarted( CiscoRTPInputStartedEv ev )
	{
		report( Trace.m( JTapiHelper.toString( ev, ev.getClass().getName() ) )
			.a( ", hasLogicalChannel = " ).a( hasLogicalChannel ) );
		
		if (!hasLogicalChannel)
			return;
		
		// nothing to do.
	}

	/**
	 * Input has stopped on the current terminal connection.
	 * @param ev
	 */
	public void ciscoRTPInputStopped( CiscoRTPInputStoppedEv ev )
	{
		report( Trace.m( JTapiHelper.toString( ev, ev.getClass().getName() ) )
			.a( ", hasLogicalChannel = " ).a( hasLogicalChannel )
			.a( ", fakeMediaSet = " ).a( fakeMediaSet ) );
		
		if (!hasLogicalChannel)
			return;
		
		if (fakeMediaSet)
			return;
		
		sendEstablishedMedia( null );
	}

	/**
	 * Output has started on the current terminal connection.
	 * @param ev
	 */
	public void ciscoRTPOutputStarted( CiscoRTPOutputStartedEv ev )
	{
		report( Trace.m( JTapiHelper.toString( ev, ev.getClass().getName() ) )
			.a( ", hasLogicalChannel = " ).a( hasLogicalChannel )
			.a( ", silent = " ).a( silent ) );
		
		if (!hasLogicalChannel)
			return;
		
		if (!silent)
			sendEstablishedMedia( ev.getRTPOutputProperties() );
		else
			pendingRTPOutputProperties = ev.getRTPOutputProperties();
	}
	
	/**
	 * 
	 */
	public void flushPendingRTPOutputProperties()
	{
		if (pendingRTPOutputProperties != null)
		{
			sendEstablishedMedia( pendingRTPOutputProperties );
			pendingRTPOutputProperties = null;
		}
	}

	/**
	 * Output has stopped on the current terminal connection.
	 * @param ev
	 */
	public void ciscoRTPOutputStopped( CiscoRTPOutputStoppedEv ev )
	{
		report( Trace.m( JTapiHelper.toString( ev, ev.getClass().getName() ) )
			.a( ", hasLogicalChannel = " ).a( hasLogicalChannel ) );
		
		if (!hasLogicalChannel)
			return;
		
		// nothing to do.
	}

	/**
	 * @param ev
	 */
	public void ciscoTermButtonPressed( CiscoTermButtonPressedEv ev )
	{
		sendGotDigits( buttonToString( ev.getButtonPressed() ), 2 );
	}

	private String buttonToString( int button )
	{
		switch( button )
		{
			case CiscoTermButtonPressedEv.CHARA: return "A";
			case CiscoTermButtonPressedEv.CHARB: return "B";
			case CiscoTermButtonPressedEv.CHARC: return "C";
			case CiscoTermButtonPressedEv.CHARD: return "D";
			case CiscoTermButtonPressedEv.ONE: return "1";
			case CiscoTermButtonPressedEv.TWO: return "2";
			case CiscoTermButtonPressedEv.THREE: return "3";
			case CiscoTermButtonPressedEv.FOUR: return "4";
			case CiscoTermButtonPressedEv.FIVE: return "5";
			case CiscoTermButtonPressedEv.SIX: return "6";
			case CiscoTermButtonPressedEv.SEVEN: return "7";
			case CiscoTermButtonPressedEv.EIGHT: return "8";
			case CiscoTermButtonPressedEv.NINE: return "9";
			case CiscoTermButtonPressedEv.STAR: return "*";
			case CiscoTermButtonPressedEv.ZERO: return "0";
			case CiscoTermButtonPressedEv.POUND: return "#";
			default: return "_";
		}
	}

	///////////////////////////
	// CONN STATE MANAGEMENT //
	///////////////////////////
	
	private boolean matchConnState( int connStateMask )
	{
		return (connState & connStateMask) != 0;
	}

	/**
	 * @param connStateMask
	 */
	private void checkConnState( int connStateMask )
	{
		if (!matchConnState( connStateMask ))
			throw new UnsupportedOperationException(
				"matchConnState( "+
				xlatConnStateMask( connStateMask )+
				" ), current state is "+
				xlatConnState( connState ));
	}
	
	private synchronized void setConnState( int newConnState )
	{
		if (newConnState == connState)
			return;
		
		validateConnState( newConnState );
		
		int oldConnState = connState;
		connState = newConnState;
		
		report( Trace.m( "connState changed from " )
			.a( xlatConnState( oldConnState ) )
			.a( " to " )
			.a( xlatConnState( newConnState ) ) );
		
		stateChanged( oldConnState, termConnState, newConnState );
	}

	private synchronized boolean tryChangeConnState( int connStateMask,
		int newConnState )
	{
		if (matchConnState( connStateMask ))
		{
			setConnState( newConnState );
			return true;
		}
		return false;
	}
	
	private void changeConnState( int connStateMask, int newConnState )
	{
		if (!tryChangeConnState( connStateMask, newConnState ))
			throw new UnsupportedOperationException(
				"changeConnState( "+
				xlatConnStateMask( connStateMask )+
				", "+
				xlatConnState( newConnState )+
				" ), current state is "+
				xlatConnState( connState ) );
	}
	
	private static String xlatConnStateMask( int connStateMask )
	{
		StringBuffer sb = new StringBuffer();
		int connState = 1;
		while (connStateMask != 0)
		{
			if ((connStateMask & connState) != 0)
			{
				if (sb.length() > 0)
					sb.append( '|' );
				sb.append( xlatConnState( connState ) );
				connStateMask &= ~connState;
			}
			connState <<= 1;
		}
		return sb.toString();
	}

	private static boolean isConnState( int connState )
	{
		return (CONN_STATES & connState) != 0;
	}
	private static void validateConnState( int connState )
	{
		switch (connState)
		{
			case INACTIVE:
			case ALERTING:
			case DIALING:
			case DISCONNECTED:
			case ESTABLISHED:
			case INITIATED:
			case OFFERED:
				return;
			
			default:
				throw new IllegalArgumentException( "bad connState "+connState );
		}
	}
	
	private static String xlatConnState( int state )
	{
		switch (state)
		{
			case INACTIVE: return "INACTIVE";
			case ALERTING: return "ALERTING";
			case DIALING: return "DIALING";
			case DISCONNECTED: return "DISCONNECTED";
			case ESTABLISHED: return "ESTABLISHED";
			case INITIATED: return "INITIATED";
			case OFFERED: return "OFFERED";
			default: return "unknownConnState"+state;
		}
	}

	private int connState = INACTIVE;
	
	private final static int INACTIVE = 1;
	
	private final static int ALERTING = 2;
	
	private final static int DIALING = 4;
	
	private final static int DISCONNECTED = 8; // and failed
	
	private final static int ESTABLISHED = 16;
	
	private final static int INITIATED = 32;
	
	private final static int OFFERED = 64;
	
	private final static int CONN_STATES = INACTIVE|ALERTING|DIALING|DISCONNECTED|ESTABLISHED|INITIATED|OFFERED;

	////////////////////////////////
	// TERM CONN STATE MANAGEMENT //
	////////////////////////////////
	
	private boolean matchTermConnState( int termConnStateMask )
	{
		return (termConnState & termConnStateMask) != 0;
	}
	
	private synchronized void setTermConnState( int newTermConnState )
	{
		if (newTermConnState == termConnState)
			return;
		
		validateTermConnState( newTermConnState );
		
		int oldTermConnState = termConnState;
		termConnState = newTermConnState;
		
		report( Trace.m( "termConnState changed from " )
			.a( xlatTermConnState( oldTermConnState ) )
			.a( " to " )
			.a( xlatTermConnState( newTermConnState ) ) );
		
		stateChanged( connState, oldTermConnState, newTermConnState );
	}

	private synchronized boolean tryChangeTermConnState( int termConnStateMask, int newTermConnState )
	{
		if (matchTermConnState( termConnStateMask ))
		{
			setTermConnState( newTermConnState );
			return true;
		}
		return false;
	}
	
	private void changeTermConnState( int termConnStateMask, int newTermConnState )
	{
		holdPending = false; // we may be going into hold, or perhaps dropped.
		if (!tryChangeTermConnState( termConnStateMask, newTermConnState ))
			throw new UnsupportedOperationException(
				"cannot change termConnState from "+
				xlatTermConnStateMask( termConnStateMask )+
				", current state is "+
				xlatTermConnState( termConnState ) );
	}
	
	private static String xlatTermConnStateMask( int stateMask )
	{
		StringBuffer sb = new StringBuffer();
		int state = 1;
		while (stateMask != 0)
		{
			if ((stateMask & state) != 0)
			{
				if (sb.length() > 0)
					sb.append( '|' );
				sb.append( xlatTermConnState( state ) );
				stateMask &= ~state;
			}
			state <<= 1;
		}
		return sb.toString();
	}
	
	private static boolean isTermConnState( int termConnState )
	{
		return (TERM_CONN_STATES & termConnState) != 0;
	}
	
	private static void validateTermConnState( int termConnState )
	{
		if (!isTermConnState( termConnState ))
			throw new IllegalArgumentException( "bad termConnState "+termConnState );
	}
	
	private static String xlatTermConnState( int state )
	{
		switch (state)
		{
			case IDLE: return "IDLE";
			case BRIDGED: return "BRIDGED";
			case DROPPED: return "DROPPED";
			case HELD: return "HELD";
			case INUSE: return "INUSE";
			case RINGING: return "RINGING";
			case TALKING: return "TALKING";
			default: return "unknownTermConnState"+state;
		}
	}
	
	private int termConnState = IDLE;
	
	private final static int BRIDGED = 128;
	
	private final static int DROPPED = 256;
	
	private final static int HELD = 512;
	
	private final static int INUSE = 1024;
	
	private final static int RINGING = 2048;
	
	private final static int TALKING = 4096;
	
	private final static int IDLE = 8192;
	
	private final static int TERM_CONN_STATES = IDLE|BRIDGED|DROPPED|HELD|INUSE|RINGING|TALKING;
	
	///////////////////
	// NOTIFY CLIENT //
	///////////////////

	private static String xlatState( int state )
	{
		if (isConnState( state ))
			return xlatConnState( state );
		else if (isTermConnState( state ))
			return xlatTermConnState( state );
		else
			return "unknownState"+state;
	}
	
	private void stateChanged( int oldConnState, int oldTermConnState, int newState )
	{
		switch (oldConnState|oldTermConnState|(newState<<16))
		{
			case INACTIVE|IDLE|(INITIATED<<16):
				doit( DO_NOTHING );
				break;
				
			case INACTIVE|IDLE|(OFFERED<<16):
				doit( SEND_INCOMING );
				break;
				
			case INACTIVE|IDLE|(ESTABLISHED<<16):
				doit( DO_NOTHING );
				break;
			
			case INITIATED|IDLE|(DIALING<<16):
				doit( DO_NOTHING );
				break;
			
			case INITIATED|IDLE|(TALKING<<16):
				doit( SEND_INITIATED );
				break;
			
			case INITIATED|IDLE|(BRIDGED<<16):
				doit( DO_NOTHING );
				break;
				
			case INITIATED|TALKING|(DIALING<<16):
				doit( DO_NOTHING );
				break;
				
			case INITIATED|TALKING|(DISCONNECTED<<16):
				doit( SEND_HANGUP );
				
			case INITIATED|TALKING|(DROPPED<<16):
				doit( SEND_HANGUP );
				break;
			
			case INITIATED|BRIDGED|(DIALING<<16):
				doit( DO_NOTHING );
				break;
			
			case INITIATED|BRIDGED|(INUSE<<16):
				doit( DO_NOTHING );
				break;
			
			case INITIATED|BRIDGED|(DISCONNECTED<<16):
				doit( DO_NOTHING );
				break;
			
			case INITIATED|INUSE|(DIALING<<16):
				doit( DO_NOTHING );
				break;
			
			case INITIATED|INUSE|(ESTABLISHED<<16):
				doit( SEND_INUSE );
				break;
			
			case INITIATED|INUSE|(DROPPED<<16):
				doit( DO_NOTHING );
				break;
			
			case INITIATED|DROPPED|(DISCONNECTED<<16):
				doit( DO_NOTHING );
				break;
				
			case DIALING|IDLE|(BRIDGED<<16):
				doit( DO_NOTHING );
				break;
			
			case DIALING|TALKING|(ESTABLISHED<<16):
				doit( SEND_ESTABLISHED );
				break;
			
			case DIALING|TALKING|(DROPPED<<16):
				doit( SEND_HANGUP );
				break;
			
			case DIALING|TALKING|(DISCONNECTED<<16):
				doit( SEND_HANGUP ); // was DO_NOTHING
				break;
			
			case DIALING|BRIDGED|(ESTABLISHED<<16):
				doit( SEND_INUSE );
				break;
			
			case DIALING|BRIDGED|(DISCONNECTED<<16):
				doit( DO_NOTHING );
				break;
			
			case DIALING|BRIDGED|(DROPPED<<16):
				doit( DO_NOTHING );
				break;
			
			case DIALING|INUSE|(ESTABLISHED<<16):
				doit( SEND_INUSE );
				break;
				
			case DIALING|INUSE|(DROPPED<<16):
				doit( DO_NOTHING );
				break;
			
			case DIALING|INUSE|(DISCONNECTED<<16):
				doit( DO_NOTHING );
				break;
			
			case DIALING|DROPPED|(DISCONNECTED<<16):
				doit( DO_NOTHING );
				break;
				
			case OFFERED|IDLE|(ALERTING<<16):
				doit( DO_NOTHING );
				break;
				
			case OFFERED|IDLE|(DISCONNECTED<<16):
				doit( SEND_HANGUP ); // was DO_NOTHING
				break;
				
			case ALERTING|IDLE|(RINGING<<16):
				doit( SEND_RINGING );
				break;
				
			case ALERTING|IDLE|(DISCONNECTED<<16):
				doit( SEND_HANGUP ); // was DO_NOTHING
				break;
			
			case ALERTING|RINGING|(ESTABLISHED<<16):
				doit( DO_NOTHING );
				break;
			
			case ALERTING|RINGING|(DROPPED<<16):
				doit( SEND_HANGUP );
				break;
			
			case ALERTING|DROPPED|(DISCONNECTED<<16):
				doit( DO_NOTHING );
				break;
			
			case ESTABLISHED|IDLE|(TALKING<<16):
				doit( ASK_ADAM );
				break;
			
			case ESTABLISHED|IDLE|(BRIDGED<<16):
				doit( SEND_INUSE );
				break;
			
			case ESTABLISHED|RINGING|(TALKING<<16):
				doit( SEND_ESTABLISHED );
				break;
				
			case ESTABLISHED|RINGING|(BRIDGED<<16):
				doit( SEND_INUSE );
				break;
				
			case ESTABLISHED|RINGING|(INUSE<<16):
				doit( SEND_INUSE );
				break;
				
			case ESTABLISHED|TALKING|(DISCONNECTED<<16):
				doit( SEND_HANGUP ); // was DO_NOTHING
				break;
			
			case ESTABLISHED|TALKING|(HELD<<16):
				doit( SEND_HELD );
				break;
			
			case ESTABLISHED|TALKING|(BRIDGED<<16):
				doit( SEND_INUSE );
				break;
			
			case ESTABLISHED|TALKING|(DROPPED<<16):
				doit( SEND_HANGUP );
				break;
				
			case ESTABLISHED|HELD|(TALKING<<16):
				doit( SEND_ESTABLISHED );
				break;
			
			case ESTABLISHED|HELD|(BRIDGED<<16):
				doit( SEND_INUSE );
				break;
			
			case ESTABLISHED|HELD|(INUSE<<16):
				doit( SEND_INUSE );
				break;
			
			case ESTABLISHED|HELD|(DROPPED<<16):
				doit( SEND_HANGUP );
				break;
			
			case ESTABLISHED|BRIDGED|(HELD<<16):
				doit( SEND_HELD );
				break;
			
			case ESTABLISHED|BRIDGED|(DISCONNECTED<<16):
				doit( SEND_HANGUP );
				break;
			
			case ESTABLISHED|BRIDGED|(DROPPED<<16):
				doit( SEND_HANGUP );
				break;
			
			case ESTABLISHED|INUSE|(HELD<<16):
				doit( SEND_HELD );
				break;
			
			case ESTABLISHED|INUSE|(DROPPED<<16):
				doit( SEND_HANGUP );
				break;
			
			case ESTABLISHED|DROPPED|(DISCONNECTED<<16):
				doit( DO_NOTHING );
				break;
				
			case DISCONNECTED|IDLE|(DROPPED<<16):
				doit( DO_NOTHING ); // was SEND_HANGUP
				break;
				
			case DISCONNECTED|TALKING|(DROPPED<<16):
				doit( DO_NOTHING ); // was SEND_HANGUP
				break;
			
			case DISCONNECTED|BRIDGED|(DROPPED<<16):
				doit( DO_NOTHING );
				break;
			
			case DISCONNECTED|INUSE|(DROPPED<<16):
				doit( DO_NOTHING );
				break;
			
			default:
				report( Trace.m( "do not know how to handle " )
					.a( xlatConnState( oldConnState ) )
					.a( "|" )
					.a( xlatTermConnState( oldTermConnState ) )
					.a( "|(" )
					.a( xlatState( newState ) )
					.a( "<<16)" ), new Throwable() );
		}
	}
	
	private void doit( int cmd )
	{
		switch (cmd)
		{
			case DO_NOTHING:
				break;
				
			case SEND_INITIATED:
				sendInitiatedCall();
				break;
				
			case SEND_INUSE:
				sendInUseCall();
				break;
				
			case SEND_ESTABLISHED:
				sendEstablishedCall( "normal" );
				break;
				
			case SEND_INCOMING:
				sendIncomingCall();
				break;
				
			case SEND_RINGING:
				sendRingingCall();
				break;
				
			case ASK_ADAM:
				//report( new Throwable( "ask adam" ) );
				sendInitiatedCall();
				sendEstablishedCall( "normal" );
				break;
				
			case SEND_HELD:
				sendHeldCall();
				break;
			
			case SEND_HANGUP:
				sendHangupCall();
				break;
			
			default:
				throw new UnsupportedOperationException( "unknown cmd "+cmd );
		}
	}
	
	private final static int DO_NOTHING = 0;
	
	private final static int SEND_INITIATED = 1;
	
	private final static int SEND_INUSE = 2;
	
	private final static int SEND_ESTABLISHED = 3;
	
	private final static int SEND_INCOMING = 4;
	
	private final static int SEND_RINGING = 5;
	
	private final static int ASK_ADAM = 6;
	
	private final static int SEND_HELD = 7;
	
	private final static int SEND_HANGUP = 8;

	/**
	 * @param ev
	 */
	private void xsetupAddrs( CallCtlConnEv ev )
	{
		CiscoConnection xconn = (CiscoConnection) ev.getConnection();
		CiscoCall xcall = (CiscoCall) ev.getCall();
		
		Address xconnAddress = xconn.getAddress();
		
		Address evCalledAddress = ev.getCalledAddress();
		Address cCalledAddress = xcall.getCalledAddress();
		Address cCurCalledAddress = xcall.getCurrentCalledAddress();
		Address cModCalledAddress = xcall.getModifiedCalledAddress();

		Address evCallingAddress = ev.getCallingAddress();
		Address cCallingAddress = xcall.getCallingAddress();
		Address cCurCallingAddress = xcall.getCurrentCallingAddress();
		Address cModCallingAddress = xcall.getModifiedCallingAddress();

		Address evLastRedirAddress = ev.getLastRedirectedAddress();
		Address cLastRedirAddress = xcall.getLastRedirectedAddress();
		
		report( Trace.m( "xsetupAddrs" )
			.a( " connAddr=" ).a( xconnAddress )
			.a( ", called (ev=" ).a( evCalledAddress )
			.a( ", c=" ).a( cCalledAddress )
			.a( ", cCur=" ).a( cCurCalledAddress )
			.a( ", cMod=" ).a( cModCalledAddress )
			.a( "), calling (ev=" ).a( evCallingAddress )
			.a( ", c=" ).a( cCallingAddress )
			.a( ", cCur=" ).a( cCurCallingAddress )
			.a( ", cMod=" ).a( cModCallingAddress )
			.a( "), redir (evLast=" ).a( evLastRedirAddress )
			.a( ", cLast=" ).a( cLastRedirAddress )
			.a( ")" ) );
		
		if (from == null)
		{
			if (cCurCallingAddress != null)
			{
				// was evCallingAddress
				from = cCurCallingAddress.getName();
			}
		}
		
		if (to == null)
		{
			if (origin == OFFERED)
				to = am.address.getName();
			else if (cCurCalledAddress != null)
			{
				// was evCalledAddress
				to = cCurCalledAddress.getName();
			}
		}
		
		if (originalTo == null)
		{
			if (cCalledAddress != null)
				originalTo = cCalledAddress.getName();
		}
	}
	
	private void sendAnswered( String who, String at )
	{
		reportAndSend( prepMsg( am.newMessage( MsgType.Answered ) ) );
	}
	
	private void sendGotDigits( String digits, int source )
	{
		reportAndSend( prepMsg( am.newMessage( MsgType.GotDigits )
			.add( MsgField.Digits, digits )
			.add( MsgField.ButtonSource, source ) ) );
	}

	/**
	 */
	private void sendInitiatedCall()
	{
		reportAndSend( prepMsg( am.newMessage( MsgType.InitiatedCall ) ) );
	}

	/**
	 */
	private void sendEstablishedCall( String cause )
	{
		reportAndSend( prepMsg( am.newMessage( MsgType.EstablishedCall )
			.add( MsgField.Cause, cause ) ) );
	}
	
	private void sendEstablishedMedia( CiscoRTPOutputProperties outputProps )
	{
		int payloadType = outputProps != null ? outputProps.getPayloadType() : 0;
		int packetSize = outputProps != null ? outputProps.getPacketSize() : 0;
		InetAddress txIp = outputProps != null ? outputProps.getRemoteAddress() : null;
		int txPort = outputProps != null ? outputProps.getRemotePort() : 0;
		
		if (holdPending)
		{
			report( Trace.m( "wanted to sendEstablishedMedia, but holdPending" ) );
			return;
		}
		
		if (matchTermConnState(HELD))
		{
			// TODO temporarily removed for experimental purposes.
//			report( Trace.m( "wanted to sendEstablishedMedia, but term conn state HELD" ) );
//			return;
		}
		
		int codec = JTapiServer.ciscoToMetreosCodec( payloadType );
		int framesize = JTapiServer.ciscoToMetreosFramesize( packetSize );
		
		reportAndSend( prepMsg( am.newMessage( MsgType.EstablishedMedia )
			.add( MsgField.Codec, codec )
			.add( MsgField.Framesize, framesize )
			.add( MsgField.TxIp, txIp != null ? txIp.getHostAddress() : "" )
			.add( MsgField.TxPort, txPort ) ) );
	}
	
	private void sendHeldCall()
	{
//		if (dontNotifyTheAppServer)
//			return;
		
		reportAndSend( prepMsg( am.newMessage( MsgType.HeldCall ) ) );
	}
	
	private void sendInUseCall()
	{
		reportAndSend( prepMsg( am.newMessage( MsgType.InUseCall ) ) );
	}

	/**
	 */
	private void sendHangupCall()
	{
		// This is an attempt to capture the most relevant reason
		// for a hangup. We generate the hangup on a transition
		// to dropped, which mostly comes before a transition
		// to failed or disconnected. But if a transition is made
		// to failed or disconnected first, that takes precedence.
		
		int cause;
		if (failCause != 0)
			cause = failCause;
		else if (disconnectCause != 0)
			cause = disconnectCause;
		else
			cause = dropCause;
		
		int callControlCause;
		if (failCallControlCause != 0)
			callControlCause = failCallControlCause;
		else if (disconnectCallControlCause != 0)
			callControlCause = disconnectCallControlCause;
		else
			callControlCause = dropCallControlCause;
		
		reportAndSend( prepMsg( am.newMessage( MsgType.HangupCall )
			.add( MsgField.Cause, JTapiHelper.xlatCause( cause ) )
			.add( MsgField.CallControlCause, JTapiHelper.xlatCallControlCause( callControlCause ) ) ) );
	}
	
	private FlatmapIpcMessage prepMsg( FlatmapIpcMessage msg )
	{
		msg.add( MsgField.CallId, ourCallId )
			.add( MsgField.From, from )
			.add( MsgField.To, to )
			.add( MsgField.OriginalTo, originalTo );
		return msg;
	}

	/**
	 */
	private void sendIncomingCall()
	{
		reportAndSend( prepMsg( am.newMessage( MsgType.IncomingCall ) ) );
	}

	private void sendRingingCall()
	{
		doSendRingingCall( false );
	}
	
	private void doSendRingingCall( boolean isAccepting )
	{
		if (isAccepting)
		{
			Assertion.check( !accepting, "!accepting" );
			accepting = true;
		}
		else // must be ringing
		{
			Assertion.check( !ringing, "!ringing" );
			if (!accepting)
				accepting = thirdParty;
			ringing = true;
		}
		
		if (accepting && ringing)
			reportAndSend( prepMsg( am.newMessage( MsgType.RingingCall ) ) );
	}
	
	private boolean accepting = false;
	
	private boolean ringing = false;
	
	//////////////////
	// CALL CONTROL //
	//////////////////
	
	/**
	 * Accepts an offered connection.
	 * @throws Exception
	 */
	public void accept()
		throws Exception
	{
		checkConnState( OFFERED | ALERTING );
		doAccept();
		doSendRingingCall( true );
	}
	
	/**
	 * performs action of accept.
	 */
	private void doAccept()
	{
		int ccs = conn.getCallControlState();
		if (ccs == CallControlConnection.OFFERED)
		{
			try
			{
				report( Trace.m( "conn.accept()..." ) );
				conn.accept();
				report( Trace.m( "conn.accept() done" ) );
			}
			catch ( InvalidStateException e )
			{
				report( Trace.m( "accept failed: " )
					.a( JTapiHelper.xlatConnCallControlState( e.getState() ) ) );
			}
			catch ( Exception e )
			{
				report( e );
			}
		}
		else
		{
			report( Trace.m( "accept ignored: " )
				.a( JTapiHelper.xlatConnCallControlState( ccs ) ) );
		}
	}

	/**
	 * Redirects an offered connection.
	 * @param redirectTo
	 * @throws Exception
	 */
	public void redirect( String redirectTo )
		throws Exception
	{
		checkConnState( OFFERED | ALERTING );
		report( Trace.m( "conn.redirect( " ).a( redirectTo ).a( ", CiscoConnection.REDIRECT_NORMAL, CiscoConnection.ADDRESS_SEARCH_SPACE )..." ) );
		conn.redirect( redirectTo, CiscoConnection.REDIRECT_NORMAL, CiscoConnection.ADDRESS_SEARCH_SPACE );
		report( Trace.m( "conn.redirect( " ).a( redirectTo ).a( ", CiscoConnection.REDIRECT_NORMAL, CiscoConnection.ADDRESS_SEARCH_SPACE ) done" ) );
	}

	/**
	 * Rejects an offered connection.
	 */
	public void reject()
	{
		try
		{
			if (matchConnState( OFFERED ))
			{
				CiscoConnection c = conn;
				if (c != null)
				{
					report( Trace.m( "conn.reject() ..." ) );
					try
					{
						c.reject();
					}
					catch ( NullPointerException e )
					{
						// ignore
					}
					report( Trace.m( "conn.reject() done" ) );
				}
			}
			else if (matchConnState( ALERTING|ESTABLISHED ))
			{
				CiscoConnection c = conn;
				if (c != null)
				{
					report( Trace.m( "conn.disconnect() ..." ) );
					try
					{
						c.disconnect();
					}
					catch ( NullPointerException e )
					{
						// ignore
					}
					report( Trace.m( "conn.disconnect() done" ) );
				}
			}
		}
		catch ( RuntimeException e )
		{
			throw e;
		}
		catch ( Exception e )
		{
			report( Trace.m( "*** reject ignoring exception" ).a( e ) );
		}
	}

	/**
	 * Answers a ringing terminal connection.
	 * @param newRxIp
	 * @param newRxPort
	 * @throws Exception
	 */
	public void answer( InetAddress newRxIp, int newRxPort )
		throws Exception
	{
		checkConnState( ALERTING );
		setRTPIpPort( newRxIp, newRxPort, false, false );
		doAnswer();
	}
	
	/**
	 * Performs action of answer.
	 * @throws Exception 
	 */
	void doAnswer() throws Exception
	{
		if (termConn == null)
		{
			TerminalConnection[] tcs = conn.getTerminalConnections();
			int n = tcs.length;
			report( Trace.m( "Looking for term conns, n = " ).a( n ) );
			for (int i = 0; i < n && termConn == null; i++)
			{
				TerminalConnection tc = tcs[i];
				Terminal t = tc.getTerminal();
				report( Trace.m( "Looking for term conns, term = " ).a( t ) );
				if (t.equals( am.tm.getTerminal() ))
					setTermConn( tc );
			}
		}
		
//		if (termConn == null)
//		{
//			Address a = conn.getAddress();
//			Terminal[] ts = a.getTerminals();
//			int n = ts.length;
//			report( Trace.m( "Looking for terms, n = " ).a( n ) );
//			for (int i = 0; i < n; i++)
//			{
//				Terminal t = ts[i];
//				report( Trace.m( "Looking for terms, term = " ).a( t ) );
//			}
//		}
		
		if (termConn == null)
			throw new Exception( "cannot answer, no terminal connection" );
		
		report( Trace.m( "termConn.answer()..." ) );
		termConn.answer();
		report( Trace.m( "termConn.answer() done" ) );
	}

	/**
	 * Transfers an established terminal connection.
	 * @param transferTo
	 * @throws Exception
	 */
	public void transfer( String transferTo )
		throws Exception
	{
		report( Trace.m( "call.transfer( " ).a( transferTo ).a( " )" ) );
		call.transfer( transferTo );
	}

	/**
	 * Conference two calls together.
	 * @param otherCm
	 * @throws Exception
	 */
	public void conference( CallMonitor otherCm )
		throws Exception
	{
		report( Trace.m( "call.conference( " ).a( otherCm ).a( " )" ) );
		call.conference( otherCm.call );
	}
	
	/**
	 * Puts a terminal connection on hold.
	 * @param setHoldPending 
	 * @throws Exception
	 */
	public void hold( boolean setHoldPending )
		throws Exception
	{
		if (termConn == null)
			throw new Exception( "cannot hold, no terminal connection" );
		
		try
		{
			report( Trace.m( "termConn.hold()..." ) );
			holdPending = setHoldPending;
			termConn.hold();
			report( Trace.m( "termConn.hold() done" ) );
		}
		catch ( Exception e )
		{
			holdPending = false;
			throw e;
		}
	}
	
	/**
	 * Removes a terminal connection from hold.
	 * @throws Exception
	 */
	public void unhold()
		throws Exception
	{
		if (termConn == null)
			throw new Exception( "cannot unhold, no terminal connection" );
		
		report( Trace.m( "termConn.unhold()..." ) );
		termConn.unhold();
		report( Trace.m( "termConn.unhold() done" ) );
	}

	/**
	 * Hangs up an established connection.
	 */
	public void hangup()
	{
		try
		{
			if (matchConnState( OFFERED ))
			{
				CiscoConnection c = conn;
				if (c != null)
				{
					report( Trace.m( "calling conn.reject()" ) );
					try
					{
						c.reject();
					}
					catch ( NullPointerException e )
					{
						report( Trace.m( "*** conn.reject() threw NullPointerException" ) );
					}
					report( Trace.m( "called conn.reject()" ) );
				}
			}
			else if (matchConnState( INITIATED|DIALING|ALERTING|ESTABLISHED ))
			{
				CiscoConnection c = conn;
				if (c != null)
				{
					report( Trace.m( "calling conn.disconnect()" ) );
					try
					{
						c.disconnect();
					}
					catch ( NullPointerException e )
					{
						report( Trace.m( "*** conn.disconnect() threw NullPointerException" ) );
					}
					report( Trace.m( "called conn.disconnect()" ) );
				}
			}
		}
		catch ( RuntimeException e )
		{
			throw e;
		}
		catch ( Exception e )
		{
			report( Trace.m( "*** hangup ignoring exception" ).a( e ) );
		}
	}

	/**
	 * @param digits
	 * @throws Exception
	 */
	public void sendUserInput( String digits ) throws Exception
	{
		if (termConn == null)
		{
			//throw new Exception( "no terminal connection" );
			report( Trace.m( "sendUserInput: no terminal connection" ) );
			return;
		}
		
		if (!(termConn instanceof MediaTerminalConnection))
		{
			//throw new Exception( "terminal connection not MediaTerminalConnection" );
			report( Trace.m( "sendUserInput: terminal connection not MediaTerminalConnection" ) );
			return;
		}

		MediaTerminalConnection mtc = (MediaTerminalConnection) termConn;
		
		MediaTerminalConnectionCapabilities caps =
			(MediaTerminalConnectionCapabilities) mtc.getCapabilities();
		
		if (!caps.canGenerateDtmf())
		{
			//throw new Exception( "terminal connection not capable" );
			report( Trace.m( "sendUserInput: terminal connection not capable" ) );
			return;
		}
		report( Trace.m( "mtc.generateDtmf( " ).a( digits ).a( " ) ..." ) );
		mtc.generateDtmf( digits );
		report( Trace.m( "mtc.generateDtmf( " ).a( digits ).a( " ) done" ) );
	}

	/**
	 * suppresses sending of messages to appserver.
	 */
	private boolean silent = false;

	/**
	 * callid of this call after a transfer is complete.
	 */
	public String transCallId = null;

	/**
	 * Description of transCm.
	 */
	public CallMonitor transCm;
	
	private boolean fakeMediaSet;

	/**
	 * @param newRxIp
	 * @param newRxPort
	 * @throws Exception 
	 */
	public void setMedia( InetAddress newRxIp, int newRxPort )
		throws Exception
	{
		report( Trace.m( "setMedia" )
//			.a( ", rtpHandle=" ).a( rtpHandle )
			.a( ", newRxIp=" ).a( newRxIp )
			.a( ", newRxPort=" ).a( newRxPort ) );
		
		// this means hold or set fake media!
		if (newRxIp == null || newRxPort == 0)
		{
			setRTPIpPort( null, 0, false, false );
			try
			{
				hold( true );
			}
			catch ( Exception e )
			{
				report( e );
				throw e;
			}
			return;
		}
		
		// if we are talking and not waiting for media, hold
		
		if (matchTermConnState( TALKING ) && !isWaitingForMedia())
		{
			if (isMediaTheSame( newRxIp, newRxPort ))
				return;
			
			try
			{
				hold( true );
			}
			catch ( Exception e )
			{
				report( e );
				throw e;
			}
			
			waitUntilTermConnState( HELD );
		}
		
		setRTPIpPort( newRxIp, newRxPort, true, false );
		
		if (matchTermConnState( HELD ))
			unhold();
	}
	
	/**
	 * 
	 */
	public void useMohMedia()
	{
		// this is a special case. our peer is on hold and
		// this side needs media set as a destination for
		// music on hold to keep call manager happy. but
		// later the moh will be stopped, which we need to
		// ignore and not notify the app.
		setRTPIpPort( mohIp, mohPort, true, true );
	}

	private boolean isMediaTheSame( InetAddress newRxIp, int newRxPort )
		throws Exception
	{
		CiscoRTPParams params = lastRTPParams;
		if (params == null)
			return false;
		
		InetAddress oldRxIp = params.getRTPAddress();
		int oldRxPort = params.getRTPPort();
		
		report( Trace.m( "isMediaTheSame" )
			.a( ", newRxIp=" ).a( newRxIp )
			.a( ", oldRxIp=" ).a( oldRxIp )
			.a( ", newRxPort=" ).a( newRxPort )
			.a( ", oldRxPort=" ).a( oldRxPort ) );
		
		return newRxIp.equals( oldRxIp ) && (newRxPort == oldRxPort);
	}

	private boolean waitUntilTermConnState( int newState ) throws InterruptedException
	{
		while (!matchTermConnState( newState|DROPPED ))
			Thread.sleep( 10 );
		return matchTermConnState( newState );
	}

	@SuppressWarnings("unused")
	private void waitUntilTermConnStateNot( int newState ) throws InterruptedException
	{
		while (matchTermConnState( newState ))
			Thread.sleep( 10 );
	}

	//////////
	// MISC //
	//////////

	/**
	 * @param newCallId
	 */
	public void rename( String newCallId )
	{
		if (am.cmMap.rename( ourCallId, newCallId ) != this)
			throw new IllegalArgumentException( "rename: newCallId already in use: "+newCallId );
	}

	/**
	 * The call has died.
	 */
	public void cleanup()
	{
		report( Trace.m( "cleanup" ) );
	}

	/**
	 * A new call monitor is taking over our device.
	 */
	public void byebye()
	{
		report( Trace.m( "byebye" ) );
	}

	/**
	 * @param oldFrom
	 * @param newFrom
	 * @param cause
	 */
	public void setFrom( String oldFrom, String newFrom, String cause )
	{
		Assertion.check( from.equals( oldFrom ), "from.equals( oldFrom )" );
		from = newFrom;
		sendEstablishedCall( cause );
	}

	/**
	 * @param oldTo
	 * @param newTo
	 * @param cause
	 */
	public void setTo( String oldTo, String newTo, String cause )
	{
		Assertion.check( to.equals( oldTo ), "to.equals( oldFrom )" );
		to = newTo;
		sendEstablishedCall( cause );
	}

	/**
	 * @param b
	 */
	public synchronized void setSilent( boolean b )
	{
		silent = b;
		if (!b)
			flushPendingRTPOutputProperties();
	}

	/**
	 * @return true if we are in silent mode
	 */
	public boolean isSilent()
	{
		return silent;
	}

	/**
	 * Place the call on hold.
	 * @throws Exception 
	 */
	public void holdCall() throws Exception
	{
		hold( false );
	}

	/**
	 * Unholds the call with the specified media (optional).
	 * @param newRxIp
	 * @param newRxPort
	 * @throws Exception 
	 */
	public void resumeCall( InetAddress newRxIp, int newRxPort ) throws Exception
	{
		setRTPIpPort( newRxIp, newRxPort, false, false );
		unhold();
	}
}