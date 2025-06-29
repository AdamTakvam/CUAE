/* $Id: JTapiHelper.java 7164 2005-06-13 20:30:41Z wert $
 *
 * Created by wert on Feb 15, 2005
 *
 * Copyright (c) 2005 Metreos, Inc. All rights reserved.
 */

package metreos.service.jtapi;

import javax.telephony.Call;
import javax.telephony.Connection;
import javax.telephony.TerminalConnection;
import javax.telephony.callcontrol.CallControlConnection;
import javax.telephony.callcontrol.CallControlTerminalConnection;
import javax.telephony.callcontrol.events.CallCtlCallEv;
import javax.telephony.callcontrol.events.CallCtlConnEv;
import javax.telephony.callcontrol.events.CallCtlEv;
import javax.telephony.callcontrol.events.CallCtlTermConnEv;
import javax.telephony.events.CallEv;
import javax.telephony.events.ConnEv;
import javax.telephony.events.Ev;
import javax.telephony.events.TermConnEv;
import javax.telephony.events.TermEv;

import com.cisco.jtapi.extensions.CiscoCall;
import com.cisco.jtapi.extensions.CiscoConnection;
import com.cisco.jtapi.extensions.CiscoMediaOpenLogicalChannelEv;
import com.cisco.jtapi.extensions.CiscoRTPInputProperties;
import com.cisco.jtapi.extensions.CiscoRTPInputStartedEv;
import com.cisco.jtapi.extensions.CiscoRTPInputStoppedEv;
import com.cisco.jtapi.extensions.CiscoRTPOutputProperties;
import com.cisco.jtapi.extensions.CiscoRTPOutputStartedEv;
import com.cisco.jtapi.extensions.CiscoRTPOutputStoppedEv;
import com.cisco.jtapi.extensions.CiscoTerminalConnection;

/**
 * blah.
 */
abstract public class JTapiHelper
{
	/**
	 * @param ev
	 * @param tag
	 * @return string rep
	 */
	public static String toString( CallEv ev, String tag )
	{
		return cleanup( tag )+
			addEv( ev )+
			addCallEv( ev );
	}
	
	/**
	 * @param ev
	 * @param tag
	 * @return string rep
	 */
	public static String toString( ConnEv ev, String tag )
	{
		return cleanup( tag )+
			addEv( ev )+
			addCallEv( ev )+
		    addConnEv( ev );
	}

	/**
	 * @param ev
	 * @param tag
	 * @return string rep
	 */
	public static String toString( TermConnEv ev, String tag )
	{
		return cleanup( tag )+
			addEv( ev )+
			addCallEv( ev )+
			addTermConnEv( ev );
	}

	/**
	 * @param ev
	 * @param tag
	 * @return string rep
	 */
	public static String toString( CallCtlConnEv ev, String tag )
	{
		return cleanup( tag )+
			addEv( ev )+
			addCallEv( ev )+
			addConnEv( ev )+
			addCallCtlEv( ev )+
			addCallCtlCallEv( ev )+
			addCallCtlConnEv( ev );
	}

	/**
	 * @param ev
	 * @param tag
	 * @return string rep
	 */
	public static String toString( CallCtlTermConnEv ev, String tag )
	{
		return cleanup( tag )+
		addEv( ev )+
		addCallEv( ev )+
		addTermConnEv( ev )+
		addCallCtlEv( ev )+
		addCallCtlCallEv( ev )+
		addCallCtlTermConnEv( ev );
	}

	/**
	 * @param ev
	 * @param tag
	 * @return string rep
	 */
	public static String toString( CiscoMediaOpenLogicalChannelEv ev, String tag )
	{
		return cleanup( tag )+
			addEv( ev )+
			addTermEv( ev )+
			", rtpHandle = "+ev.getCiscoRTPHandle()+
			", payloadType = "+ev.getPayLoadType()+
			", packetSize = "+ev.getPacketSize();
	}

	/**
	 * @param ev
	 * @param tag
	 * @return string rep
	 */
	public static String toString( CiscoRTPInputStartedEv ev, String tag )
	{
		CiscoRTPInputProperties inputProps = ev.getRTPInputProperties();
		return cleanup( tag )+
			addEv( ev )+
			addTermEv( ev )+
			", localAddress = "+inputProps.getLocalAddress().getHostAddress()+
			", localPort = "+inputProps.getLocalPort()+
			", payloadType = "+inputProps.getPayloadType()+
			", packetSize = "+inputProps.getPacketSize();
	}

	/**
	 * @param ev
	 * @param tag
	 * @return string rep
	 */
	public static String toString( CiscoRTPInputStoppedEv ev, String tag )
	{
		return cleanup( tag )+
			addEv( ev )+
			addTermEv( ev );
	}

	/**
	 * @param ev
	 * @param tag
	 * @return string rep
	 */
	public static String toString( CiscoRTPOutputStartedEv ev, String tag )
	{
		CiscoRTPOutputProperties outputProps = ev.getRTPOutputProperties();
		return cleanup( tag )+
			addEv( ev )+
			addTermEv( ev )+
			", remoteAddress = "+outputProps.getRemoteAddress().getHostAddress()+
			", remotePort = "+outputProps.getRemotePort()+
			", payloadType = "+outputProps.getPayloadType()+
			", packetSize = "+outputProps.getPacketSize();
	}

	/**
	 * @param ev
	 * @param tag
	 * @return string rep
	 */
	public static String toString( CiscoRTPOutputStoppedEv ev, String tag )
	{
		return cleanup( tag )+
			addEv( ev )+
			addTermEv( ev );
	}

	///////////////////
	// PRIVATE STUFF //
	///////////////////
	
	private static String cleanup( String s )
	{
		int index = s.lastIndexOf( '.' );
		if (index >= 0)
			s = s.substring( index+1 );
		if (s.endsWith( "Impl" ))
			s = s.substring( 0, s.length()-4 );
		return s;
	}

	/**
	 * @param ev
	 * @return string rep
	 */
	private static String addEv( Ev ev )
	{
		return 
			", cause = "+xlatCause( ev.getCause() )+
			", metaCode = "+xlatMetaCode( ev.getMetaCode() );
	}

	/**
	 * @param ev
	 * @return string rep
	 */
	private static String addCallEv( CallEv ev )
	{
		CiscoCall call = (CiscoCall) ev.getCall();
		return
			", callId = "+call.getCallID()+
			", callState = "+xlatCallState( call.getState() );
	}

	/**
	 * @param ev
	 * @return string rep
	 */
	private static String addConnEv( ConnEv ev )
	{
		CiscoConnection conn = (CiscoConnection) ev.getConnection();
		return
			", conn.getAddress = "+conn.getAddress()+
			", conn.getCallControlState = "+xlatConnCallControlState( conn.getCallControlState() )+
			", conn.getConnectionID = "+conn.getConnectionID()+
			", conn.getReason = "+xlatReason( conn.getReason() )+
			", conn.getState = "+xlatConnState( conn.getState() );
	}

	/**
	 * @param ev
	 * @return string rep
	 */
	private static String addTermConnEv( TermConnEv ev )
	{
		CiscoTerminalConnection termConn = (CiscoTerminalConnection) ev.getTerminalConnection();
		CiscoConnection conn = (CiscoConnection) termConn.getConnection();
		return
		", termConn.getCallControlState = "+xlatTermConnCallControlState( termConn.getCallControlState() )+
		", termConn.getState = "+xlatTermConnState( termConn.getState() )+
		", termConn.getTerminal = "+termConn.getTerminal()+
		", conn.getAddress = "+conn.getAddress()+
		", conn.getCallControlState = "+xlatConnCallControlState( conn.getCallControlState() )+
		", conn.getConnectionID = "+conn.getConnectionID()+
		", conn.getReason = "+xlatReason( conn.getReason() )+
		", conn.getState = "+xlatConnState( conn.getState() );
	}

	/**
	 * @param ev
	 * @return string rep
	 */
	private static String addCallCtlEv( CallCtlEv ev )
	{
		return ", callControlCause = "+xlatCallControlCause( ev.getCallControlCause() );
	}

	/**
	 * @param ev
	 * @return string rep
	 */
	private static String addCallCtlCallEv( CallCtlCallEv ev )
	{
		return
			", calledAddress = "+ev.getCalledAddress()+
			", callingAddress = "+ev.getCallingAddress()+
			", callingTerminal = "+ev.getCallingTerminal()+
			", lastRedirectedAddress = "+ev.getLastRedirectedAddress();
	}

	/**
	 * @param ev
	 * @return string rep
	 */
	private static String addCallCtlConnEv( CallCtlConnEv ev )
	{
		return "";
	}
	
	/**
	 * @param ev
	 * @return string rep
	 */
	private static String addCallCtlTermConnEv( CallCtlTermConnEv ev )
	{
		return "";
	}
	
	private static String addTermEv( TermEv ev )
	{
		return ", terminal = "+ev.getTerminal().getName();
	}

	/////////////////
	// TRANSLATORS //
	/////////////////
	
	/**
	 * @param state
	 * @return string rep
	 */
	public static String xlatTermConnState( int state )
	{
		switch( state )
		{
			case CallControlTerminalConnection.BRIDGED: return "BRIDGED";
			case CallControlTerminalConnection.DROPPED: return "DROPPED";
			case CallControlTerminalConnection.HELD: return "HELD";
			case CallControlTerminalConnection.IDLE: return "IDLE";
			case CallControlTerminalConnection.INUSE: return "INUSE";
			case CallControlTerminalConnection.RINGING: return "RINGING";
			case CallControlTerminalConnection.TALKING: return "TALKING";
			case CallControlTerminalConnection.UNKNOWN: return "UNKNOWN";
			case TerminalConnection.ACTIVE: return "ACTIVE";
			case TerminalConnection.DROPPED: return "DROPPED";
			case TerminalConnection.IDLE: return "IDLE";
			case TerminalConnection.PASSIVE: return "PASSIVE";
			case TerminalConnection.RINGING: return "RINGING";
			case TerminalConnection.UNKNOWN: return "UNKNOWN";
			default: return ""+state;
		}
	}

	/**
	 * @param state
	 * @return string rep
	 */
	public static String xlatTermConnCallControlState( int state )
	{
		switch( state )
		{
			case TerminalConnection.ACTIVE: return "ACTIVE";
			case CallControlTerminalConnection.BRIDGED: return "BRIDGED";
			case CallControlTerminalConnection.DROPPED: return "DROPPED";
			case CallControlTerminalConnection.HELD: return "HELD";
			case CallControlTerminalConnection.IDLE: return "IDLE";
			case CallControlTerminalConnection.INUSE: return "INUSE";
			case TerminalConnection.PASSIVE: return "PASSIVE";
			case CallControlTerminalConnection.RINGING: return "RINGING";
			case CallControlTerminalConnection.TALKING: return "TALKING";
			case CallControlTerminalConnection.UNKNOWN: return "UNKNOWN";
			default: return ""+state;
		}
	}
	
	/**
	 * @param state
	 * @return string rep
	 */
	public static String xlatConnCallControlState( int state )
	{
		switch( state )
		{
			case CallControlConnection.ALERTING: return "ALERTING";
//			case CallControlConnection.CONNECTED: return "CONNECTED";
			case CallControlConnection.DIALING: return "DIALING";
			case CallControlConnection.DISCONNECTED: return "DISCONNECTED";
			case CallControlConnection.ESTABLISHED: return "ESTABLISHED";
			case CallControlConnection.FAILED: return "FAILED";
			case CallControlConnection.IDLE: return "IDLE";
			case CallControlConnection.INITIATED: return "INITIATED";
//			case CallControlConnection.INPROGRESS: return "INPROGRESS";
			case CallControlConnection.NETWORK_ALERTING: return "NETWORK_ALERTING";
			case CallControlConnection.NETWORK_REACHED: return "NETWORK_REACHED";
			case CallControlConnection.OFFERED: return "OFFERED";
			case CallControlConnection.QUEUED: return "QUEUED";
			case CallControlConnection.UNKNOWN: return "UNKNOWN";
			default: return ""+state;
		}
	}

	/**
	 * @param state
	 * @return string rep
	 */
	public static String xlatConnState( int state )
	{
		switch( state )
		{
			case Connection.ALERTING: return "ALERTING";
			case Connection.CONNECTED: return "CONNECTED";
			case Connection.DISCONNECTED: return "DISCONNECTED";
			case Connection.FAILED: return "FAILED";
			case Connection.IDLE: return "IDLE";
			case Connection.INPROGRESS: return "INPROGRESS";
			case Connection.UNKNOWN: return "UNKNOWN";
			default: return ""+state;
		}
	}

	/**
	 * @param reason
	 * @return string rep
	 */
	public static String xlatReason( int reason )
	{
		switch( reason )
		{
			case CiscoConnection.REASON_DIRECTCALL: return "DIRECTCALL";
			case CiscoConnection.REASON_FORWARDALL: return "FORWARDALL";
			case CiscoConnection.REASON_FORWARDBUSY: return "FORWARDBUSY";
			case CiscoConnection.REASON_FORWARDNOANSWER: return "FORWARDNOANSWER";
			case CiscoConnection.REASON_OUTBOUND: return "OUTBOUND";
			case CiscoConnection.REASON_REDIRECT: return "REDIRECT";
			case CiscoConnection.REASON_TRANSFERREDCALL: return "TRANSFERREDCALL";
			default: return ""+reason;
		}
	}

	/**
	 * @param state
	 * @return string rep
	 */
	public static String xlatCallState( int state )
	{
		switch (state)
		{
			case Call.IDLE: return "idle";
			case Call.ACTIVE: return "active";
			case Call.INVALID: return "invalid";
			default: return ""+state;
		}
	}
	
	/**
	 * @param metaCode
	 * @return string form of meta code
	 */
	public static String xlatMetaCode( int metaCode )
	{
		switch (metaCode)
		{
			case Ev.META_CALL_ADDITIONAL_PARTY: return "CALL_ADDITIONAL_PARTY";
			case Ev.META_CALL_ENDING: return "CALL_ENDING";
			case Ev.META_CALL_MERGING: return "CALL_MERGING";
			case Ev.META_CALL_PROGRESS: return "CALL_PROGRESS";
			case Ev.META_CALL_REMOVING_PARTY: return "CALL_REMOVING_PARTY";
			case Ev.META_CALL_STARTING: return "CALL_STARTING";
			case Ev.META_CALL_TRANSFERRING: return "CALL_TRANSFERRING";
			case Ev.META_SNAPSHOT: return "SNAPSHOT";
			case Ev.META_UNKNOWN: return "UNKNOWN";
			default: return ""+metaCode;
		}
	}
	
	/**
	 * @param cause
	 * @return string form of ConnEv cause.
	 */
	public static String xlatCause( int cause )
	{
		switch (cause)
		{
			case Ev.CAUSE_CALL_CANCELLED: return "CALL_CANCELLED";
			case Ev.CAUSE_DEST_NOT_OBTAINABLE: return "DEST_NOT_OBTAINABLE";
			case Ev.CAUSE_INCOMPATIBLE_DESTINATION: return "INCOMPATIBLE_DESTINATION";
			case Ev.CAUSE_LOCKOUT: return "LOCKOUT";
			case Ev.CAUSE_NETWORK_CONGESTION: return "NETWORK_CONGESTION";
			case Ev.CAUSE_NETWORK_NOT_OBTAINABLE: return "NETWORK_NOT_OBTAINABLE";
			case Ev.CAUSE_NEW_CALL: return "NEW_CALL";
			case Ev.CAUSE_NORMAL: return "NORMAL";
			case Ev.CAUSE_RESOURCES_NOT_AVAILABLE: return "RESOURCES_NOT_AVAILABLE";
			case Ev.CAUSE_SNAPSHOT: return "SNAPSHOT";
			case Ev.CAUSE_UNKNOWN: return "UNKNOWN";
			default: return ""+cause;
		}
	}
	
	/**
	 * @param cause
	 * @return string rep
	 */
	public static String xlatCallControlCause( int cause )
	{
		switch (cause)
		{
			case CallCtlEv.CAUSE_ALTERNATE: return "ALTERNATE";
			case CallCtlEv.CAUSE_BUSY: return "BUSY";
			case CallCtlEv.CAUSE_CALL_BACK: return "CALL_BACK";
			case CallCtlEv.CAUSE_CALL_NOT_ANSWERED: return "CALL_NOT_ANSWERED";
			case CallCtlEv.CAUSE_CALL_PICKUP: return "CALL_PICKUP";
			case CallCtlEv.CAUSE_CONFERENCE: return "CONFERENCE";
			case CallCtlEv.CAUSE_DO_NOT_DISTURB: return "DO_NOT_DISTURB";
			case CallCtlEv.CAUSE_PARK: return "PARK";
			case CallCtlEv.CAUSE_REDIRECTED: return "REDIRECTED";
			case CallCtlEv.CAUSE_REORDER_TONE: return "REORDER_TONE";
			case CallCtlEv.CAUSE_TRANSFER: return "TRANSFER";
			case CallCtlEv.CAUSE_TRUNKS_BUSY: return "TRUNKS_BUSY";
			case CallCtlEv.CAUSE_UNHOLD: return "UNHOLD";
			default: return xlatCause( cause );
		}
	}
}
