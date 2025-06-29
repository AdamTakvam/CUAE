/* $Id: CallMonitor.java 30152 2007-03-06 21:47:50Z wert $
 *
 * Created by wert on Feb 17, 2005.
 *
 * Copyright (c) 2005 Metreos, Inc. All rights reserved.
 */

package metreos.service.jtapi;

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
import metreos.util.Trace;

import com.cisco.jtapi.extensions.CiscoCall;
import com.cisco.jtapi.extensions.CiscoCallID;
import com.cisco.jtapi.extensions.CiscoConnection;
import com.cisco.jtapi.extensions.CiscoRTPInputStartedEv;
import com.cisco.jtapi.extensions.CiscoRTPInputStoppedEv;
import com.cisco.jtapi.extensions.CiscoRTPOutputProperties;
import com.cisco.jtapi.extensions.CiscoRTPOutputStartedEv;
import com.cisco.jtapi.extensions.CiscoRTPOutputStoppedEv;
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
	 */
	public CallMonitor( AddressMonitor am, CiscoCallID ciscoCallId,
		String ourCallId, CiscoCall call )
	{
		this.am = am;
		this.ciscoCallId = ciscoCallId;
		this.ourCallId = ourCallId;
		this.call = call;
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

	private final CiscoCall call;
	
	/**
	 * Description of conn.
	 */
	CiscoConnection conn;

	/**
	 * Description of termConn.
	 */
	CiscoTerminalConnection termConn;
	
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

	@SuppressWarnings("unused")
	private String rxIP;

	@SuppressWarnings("unused")
	private int rxPort;
	
	@SuppressWarnings("unused")
	private boolean rtpParamsSet;
	
	private int disconnectCause = 0;

	private int disconnectCallControlCause = 0;
	
	private int failCause = 0;
	
	private int failCallControlCause = 0;

	private int origin = 0;

	private int dropCause = 0;
	
	private int dropCallControlCause = 0;

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
		
		if (conn != null)
		{
			sb.append( ' ' );
			sb.append( conn.getAddress() );
		}
		
		if (termConn != null)
		{
			sb.append( ' ' );
			sb.append( termConn.getTerminal() );
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
	 */
	public void callChangedEvent( CallCtlCallEv ev )
	{
		try
		{
			callChangedEvent0( ev );
		}
		catch ( Exception e )
		{
			report( e );
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
		
		reportAndSend( am.newMessage( MsgType.GotDigits )
			.add( MsgField.CallId, ourCallId )
			.add( MsgField.From, from )
			.add( MsgField.To, to )
			.add( MsgField.OriginalTo, originalTo )
			.add( MsgField.Digits, digits ) );
	}
	
	private void callChangedEvent0( CallCtlCallEv ev )
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
				break;

			case CallCtlConnDialingEv.ID:
				callCtlConnDialing( (CallCtlConnDialingEv) ev );
				break;

			case CallCtlConnDisconnectedEv.ID:
				callCtlConnDisconnected( (CallCtlConnDisconnectedEv) ev );
				break;

			case CallCtlConnEstablishedEv.ID:
				callCtlConnEstablished( (CallCtlConnEstablishedEv) ev );
				break;

			case CallCtlConnFailedEv.ID:
				callCtlConnFailed( (CallCtlConnFailedEv) ev );
				break;

			case CallCtlConnInitiatedEv.ID:
				callCtlConnInitiated( (CallCtlConnInitiatedEv) ev );
				break;

			case CallCtlConnOfferedEv.ID:
				callCtlConnOffered( (CallCtlConnOfferedEv) ev );
				break;

			///////////////////////
			// CallCtlTermConnEv //
			///////////////////////

			case CallCtlTermConnBridgedEv.ID:
				callCtlTermConnBridged( (CallCtlTermConnBridgedEv) ev );
				break;

			case CallCtlTermConnDroppedEv.ID:
				callCtlTermConnDropped( (CallCtlTermConnDroppedEv) ev );
				break;

			case CallCtlTermConnHeldEv.ID:
				callCtlTermConnHeld( (CallCtlTermConnHeldEv) ev );
				break;

			case CallCtlTermConnInUseEv.ID:
				callCtlTermConnInUse( (CallCtlTermConnInUseEv) ev );
				break;

			case CallCtlTermConnRingingEv.ID:
				callCtlTermConnRinging( (CallCtlTermConnRingingEv) ev );
				break;

			case CallCtlTermConnTalkingEv.ID:
				callCtlTermConnTalking( (CallCtlTermConnTalkingEv) ev );
				break;
			
			default:
				report( Trace.m( "*** event not handled: " ).a( ev ) );
				break;
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
		conn = (CiscoConnection) ev.getConnection();
		setupAddrs( ev );
		changeConnState( OFFERED, ALERTING );
	}

	/**
	 * @param ev
	 */
	private void callCtlConnDialing( CallCtlConnDialingEv ev )
	{
		conn = (CiscoConnection) ev.getConnection();
		setupAddrs( ev );
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
		conn = (CiscoConnection) ev.getConnection();
		setupAddrs( ev );
		changeConnState( INACTIVE|ALERTING|DIALING, ESTABLISHED );
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
		conn = (CiscoConnection) ev.getConnection();
		origin = INITIATED;
		setupAddrs( ev );
		CallData cd = (CallData) call.getObject();
		if (cd != null)
		{
			synchronized (cd)
			{
				if (cd.fromCallId != null)
				{
					rename( cd.fromCallId );
					rxIP = cd.rxIP;
					rxPort = cd.rxPort;
					
					cd.fromCallId = null;
					cd.rxIP = null;
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
	public class AnswerObserver implements CallObserver, CallControlCallObserver
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
				Address connAddress = c.getAddress();
				String connAddressName = connAddress.getName();
//				Address calledAddress = ev.getCalledAddress();
//				String calledAddressName = calledAddress.getName();
				if (contains( alerting, connAddressName ) && !contains( answered, connAddressName ))
				{
					report( Trace.m( "===== adding " ).a( connAddressName ).a( " to answered" ) );
					answered.add( connAddressName );
					callAnswered( connAddressName );
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
	 * @param who the device that answered.
	 */
	void callAnswered( String who )
	{
		report( Trace.m( "answered at " ).a( who ) );
		reportAndSend( am.newMessage( MsgType.Answered )
			.add( MsgField.CallId, ourCallId )
			.add( MsgField.From, from )
			.add( MsgField.To, to )
			.add( MsgField.OriginalTo, originalTo ) );
	}

	/**
	 * We are being offered a connection.
	 * @param ev
	 */
	private void callCtlConnOffered( CallCtlConnOfferedEv ev )
	{
		conn = (CiscoConnection) ev.getConnection();
		origin = OFFERED;
		setupAddrs( ev );
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
		changeTermConnState( IDLE, BRIDGED );
	}

	/**
	 * @param tc
	 */
	private void setTermConn( TerminalConnection tc )
	{
		if (termConn == tc)
			return;
		
		termConn = (CiscoTerminalConnection) tc;
		
		if (tc != null && tc instanceof MediaTerminalConnection)
		{
			MediaTerminalConnection mtc = (MediaTerminalConnection) termConn;
			
			int state = mtc.getMediaAvailability();
			MediaTerminalConnectionCapabilities caps =
				(MediaTerminalConnectionCapabilities) mtc.getCapabilities();
			
			if (state == MediaTerminalConnection.AVAILABLE && caps.canDetectDtmf())
			{
				try
				{
					report( Trace.m( "mtc.setDtmfDetection( true )" ) );
					mtc.setDtmfDetection( true );
				}
				catch ( Exception e )
				{
					report( e );
				}
			}
		}
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
		changeTermConnState( TALKING|INUSE, HELD );
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
	 * Input has started on the current terminal connection.
	 * @param ev
	 */
	public void ciscoRTPInputStarted( CiscoRTPInputStartedEv ev )
	{
		report( Trace.m( JTapiHelper.toString( ev, ev.getClass().getName() ) ) );
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
		report( Trace.m( JTapiHelper.toString( ev, ev.getClass().getName() ) ) );
		if (!hasLogicalChannel)
			return;
		
		// nothing to do.
	}

	/**
	 * Output has started on the current terminal connection.
	 * @param ev
	 */
	public void ciscoRTPOutputStarted( CiscoRTPOutputStartedEv ev )
	{
		report( Trace.m( JTapiHelper.toString( ev, ev.getClass().getName() ) ) );
		if (!hasLogicalChannel)
			return;
		
		CiscoRTPOutputProperties outputProps = ev.getRTPOutputProperties();
		int payloadType = outputProps.getPayloadType();
		int packetSize = outputProps.getPacketSize();
		String txIP = outputProps.getRemoteAddress().getHostAddress();
		int txPort = outputProps.getRemotePort();
		
		sendEstablishedMedia( payloadType, packetSize, txIP, txPort );
	}

	/**
	 * Output has stopped on the current terminal connection.
	 * @param ev
	 */
	public void ciscoRTPOutputStopped( CiscoRTPOutputStoppedEv ev )
	{
		report( Trace.m( JTapiHelper.toString( ev, ev.getClass().getName() ) ) );
		if (!hasLogicalChannel)
			return;
		
		// nothing to do.
	}

	///////////////////////////
	// CONN STATE MANAGEMENT //
	///////////////////////////
	
	private synchronized boolean matchConnState( int connStateMask )
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
		
		stateChanged( newConnState );
		
		report( Trace.m( "connState changed from " )
			.a( xlatConnState( connState ) )
			.a( " to " )
			.a( xlatConnState( newConnState ) ) );
		
		connState = newConnState;
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
	
	private synchronized boolean matchTermConnState( int termConnStateMask )
	{
		return (termConnState & termConnStateMask) != 0;
	}
	
	private synchronized void setTermConnState( int newTermConnState )
	{
		if (newTermConnState == termConnState)
			return;
		
		validateTermConnState( newTermConnState );
		
		stateChanged( newTermConnState );
		
		report( Trace.m( "termConnState changed from " )
			.a( xlatTermConnState( termConnState ) )
			.a( " to " )
			.a( xlatTermConnState( newTermConnState ) ) );
		
		termConnState = newTermConnState;
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
	
	private void stateChanged( int newState )
	{
		switch (connState|termConnState|(newState<<16))
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
			
			case INITIATED|IDLE|(TALKING<<16):
				doit( SEND_INITIATED );
				break;
			
			case INITIATED|IDLE|(BRIDGED<<16):
				doit( DO_NOTHING );
				break;
				
			case INITIATED|TALKING|(DIALING<<16):
				doit( DO_NOTHING );
				break;
				
			case INITIATED|TALKING|(DROPPED<<16):
				doit( SEND_HANGUP );
				break;
			
			case INITIATED|BRIDGED|(INUSE<<16):
				doit( DO_NOTHING );
				break;
			
			case INITIATED|INUSE|(DIALING<<16):
				doit( DO_NOTHING );
				break;
			
			case INITIATED|INUSE|(DROPPED<<16):
				doit( DO_NOTHING );
				break;
			
			case INITIATED|DROPPED|(DISCONNECTED<<16):
				doit( DO_NOTHING );
				break;
				
			case DIALING|TALKING|(ESTABLISHED<<16):
				doit( SEND_ESTABLISHED );
				break;
			
			case DIALING|TALKING|(DROPPED<<16):
				doit( SEND_HANGUP );
				break;
			
			case DIALING|TALKING|(DISCONNECTED<<16):
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
				doit( DO_NOTHING );
				break;
				
			case ALERTING|IDLE|(RINGING<<16):
				doit( SEND_RINGING );
				break;
				
			case ALERTING|IDLE|(DISCONNECTED<<16):
				doit( DO_NOTHING );
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
			
			case ESTABLISHED|RINGING|(TALKING<<16):
				doit( SEND_ESTABLISHED );
				break;
				
			case ESTABLISHED|RINGING|(INUSE<<16):
				doit( SEND_INUSE );
				break;
				
			case ESTABLISHED|TALKING|(DISCONNECTED<<16):
				doit( DO_NOTHING );
				break;
			
			case ESTABLISHED|TALKING|(HELD<<16):
				doit( SEND_HELD );
				break;
			
			case ESTABLISHED|TALKING|(DROPPED<<16):
				doit( SEND_HANGUP );
				break;
				
			case ESTABLISHED|HELD|(TALKING<<16):
				doit( SEND_ESTABLISHED );
				break;
			
			case ESTABLISHED|HELD|(INUSE<<16):
				doit( SEND_INUSE );
				break;
			
			case ESTABLISHED|HELD|(DROPPED<<16):
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
				doit( SEND_HANGUP );
				break;
				
			case DISCONNECTED|TALKING|(DROPPED<<16):
				doit( SEND_HANGUP );
				break;
			
			case DISCONNECTED|INUSE|(DROPPED<<16):
				doit( DO_NOTHING );
				break;
			
			default:
				report( Trace.m( "do not know how to handle " )
					.a( xlatConnState( connState ) )
					.a( "|" )
					.a( xlatTermConnState( termConnState ) )
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
				sendEstablishedCall();
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
				sendEstablishedCall();
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
	private void setupAddrs( CallCtlConnEv ev )
	{
		Address a;
		
		if (from == null)
		{
			a = ev.getCallingAddress();
			if (a != null)
				from = a.getName();
		}
		
		if (to == null)
		{
			if (origin == OFFERED)
				to = am.address.getName();
			else
			{
				a = ev.getCalledAddress();
				if (a != null)
					to = a.getName();
			}
		}
		
		if (originalTo == null)
		{
			a = ev.getCalledAddress();
			if (a != null)
				originalTo = a.getName();
		}
	}

	/**
	 */
	private void sendInitiatedCall()
	{
		reportAndSend( am.newMessage( MsgType.InitiatedCall )
			.add( MsgField.CallId, ourCallId )
			.add( MsgField.From, from )
			.add( MsgField.To, to )
			.add( MsgField.OriginalTo, originalTo ) );
	}

	/**
	 */
	private void sendEstablishedCall()
	{
		reportAndSend( am.newMessage( MsgType.EstablishedCall )
			.add( MsgField.CallId, ourCallId )
			.add( MsgField.From, from )
			.add( MsgField.To, to )
			.add( MsgField.OriginalTo, originalTo ) );
	}

	private void sendEstablishedMedia( int payloadType, int packetSize,
		String txIP, int txPort )
	{
		int codec = JTapiServer.ciscoToMetreosCodec( payloadType );
		int framesize = JTapiServer.ciscoToMetreosFramesize( packetSize );
		
		reportAndSend( am.tm.newMessage( MsgType.EstablishedMedia )
			.add( MsgField.CallId, ourCallId )
			.add( MsgField.From, from )
			.add( MsgField.To, to )
			.add( MsgField.OriginalTo, originalTo )
			.add( MsgField.Codec, codec )
			.add( MsgField.Framesize, framesize )
			.add( MsgField.TxIP, txIP )
			.add( MsgField.TxPort, txPort ) );
	}
	
	private void sendHeldCall()
	{
		reportAndSend( am.newMessage( MsgType.HeldCall )
			.add( MsgField.CallId, ourCallId )
			.add( MsgField.From, from )
			.add( MsgField.To, to )
			.add( MsgField.OriginalTo, originalTo ) );
	}
	
	private void sendInUseCall()
	{
		reportAndSend( am.newMessage( MsgType.InUseCall )
			.add( MsgField.CallId, ourCallId )
			.add( MsgField.From, from )
			.add( MsgField.To, to )
			.add( MsgField.OriginalTo, originalTo ) );
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
		
		reportAndSend( am.newMessage( MsgType.HangupCall )
			.add( MsgField.CallId, ourCallId )
			.add( MsgField.From, from )
			.add( MsgField.To, to )
			.add( MsgField.OriginalTo, originalTo )
			.add( MsgField.Cause, JTapiHelper.xlatCause( cause ) )
			.add( MsgField.CallControlCause, JTapiHelper.xlatCallControlCause( callControlCause ) ) );
	}

	/**
	 */
	private void sendIncomingCall()
	{
		reportAndSend( am.newMessage( MsgType.IncomingCall )
			.add( MsgField.CallId, ourCallId )
			.add( MsgField.From, from )
			.add( MsgField.To, to )
			.add( MsgField.OriginalTo, originalTo ) );
	}

	/**
	 */
	private void sendRingingCall()
	{
		reportAndSend( am.newMessage( MsgType.RingingCall )
			.add( MsgField.CallId, ourCallId )
			.add( MsgField.From, from )
			.add( MsgField.To, to )
			.add( MsgField.OriginalTo, originalTo ) );
	}
	
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
	}
	
	/**
	 * performs action of accept.
	 */
	void doAccept()
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
		report( Trace.m( "conn.redirect( "+redirectTo+" )..." ) );
		conn.redirect( redirectTo );
		report( Trace.m( "conn.redirect( "+redirectTo+" ) done" ) );
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
					report( Trace.m( "conn.reject()" ) );
					c.reject();
				}
			}
			else if (matchConnState( ALERTING|ESTABLISHED ))
			{
				CiscoConnection c = conn;
				if (c != null)
				{
					report( Trace.m( "conn.disconnect()" ) );
					c.disconnect();
				}
			}
		}
		catch ( RuntimeException e )
		{
			throw e;
		}
		catch ( Exception e )
		{
			// ignore
		}
	}
	
	/**
	 * @throws Exception
	 */
	public void answer() throws Exception
	{
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
				if (t.getName().equals( am.tm.getTerminal().getName()))
					termConn = (CiscoTerminalConnection) tc;
			}
		}
		
		if (termConn == null)
		{
			Address a = conn.getAddress();
			Terminal[] ts = a.getTerminals();
			int n = ts.length;
			report( Trace.m( "Looking for terms, n = " ).a( n ) );
			for (int i = 0; i < n; i++)
			{
				Terminal t = ts[i];
				report( Trace.m( "Looking for terms, term = " ).a( t ) );
			}
		}
		
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
		report( Trace.m( "call.transfer( "+transferTo+" )" ) );
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
		report( Trace.m( "call.conference( "+otherCm+" )" ) );
		call.conference( otherCm.call );
	}
	
	/**
	 * Puts a terminal connection on hold.
	 * @throws Exception
	 */
	public void hold()
		throws Exception
	{
		report( Trace.m( "termConn.hold()" ) );
		termConn.hold();
	}
	
	/**
	 * Removes a terminal connection from hold.
	 * @throws Exception
	 */
	public void unhold()
		throws Exception
	{
		report( Trace.m( "termConn.unhold()" ) );
		termConn.unhold();
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
					report( Trace.m( "conn.reject()" ) );
					c.reject();
				}
			}
			else if (matchConnState( INITIATED|DIALING|ALERTING|ESTABLISHED ))
			{
				CiscoConnection c = conn;
				if (c != null)
				{
					report( Trace.m( "conn.disconnect()" ) );
					c.disconnect();
				}
			}
		}
		catch ( RuntimeException e )
		{
			throw e;
		}
		catch ( Exception e )
		{
			// ignore
		}
	}

	/**
	 * @param digits
	 * @throws Exception
	 */
	public void sendUserInput( String digits ) throws Exception
	{
		if (termConn == null)
			throw new Exception( "no terminal connection" );
		
		if (!(termConn instanceof MediaTerminalConnection))
			throw new Exception( "terminal connection not MediaTerminalConnection" );

		MediaTerminalConnection mtc = (MediaTerminalConnection) termConn;
		
		MediaTerminalConnectionCapabilities caps =
			(MediaTerminalConnectionCapabilities) mtc.getCapabilities();
		
		if (!caps.canGenerateDtmf())
			throw new Exception( "terminal connection not capable" );
		
		report( Trace.m( "mtc.generateDtmf( "+digits+" )" ) );
		mtc.generateDtmf( digits );
	}

	//////////
	// MISC //
	//////////

	/**
	 * @param newCallId
	 */
	private void rename( String newCallId )
	{
		am.cmMap.rename( ourCallId , newCallId );
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
}