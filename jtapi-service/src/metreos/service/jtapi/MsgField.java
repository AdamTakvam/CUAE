/* $Id: MsgField.java 32224 2007-05-16 21:26:24Z wert $
 * 
 * Created by achaney on Jan 28, 2005
 *
 * Copyright (c) 2005 Metreos, Inc. All rights reserved.
 */
package metreos.service.jtapi;

/**
 * @author achaney
 *
 * This interface defines the IPC flatmap message fields used when communicating
 *   with the AppServer's JTAPI provider. 
 * 
 * Note: The constants defined here must correspond exactly with those defined in the 
 *   MsgField enumeration in JTapiProxy.cs in the JTAPI provider.
 * 
 * DeviceType is an integer field indicating whether the specified device name represents 
 *   a CTI Port or CTI Route Point using values 0 or 1, respectively.
 */
public interface MsgField
{
	/**
	 * The args of the message which suffered a general failure.
	 * 
	 * @see MsgType#Error
	 * @see FailReason#GeneralFailure
	 */
	public static final int Args = 52;

	/**
	 * The cause of a hangup. This is a string token such as "NORMAL",
	 * "BUSY", "CONFERENCE", "REDIRECTED", "TRANSFER", etc.
	 * 
	 * @see MsgType#HangupCall
	 */
	public static final int CallControlCause = 53;
	
	/**
	 * The id of a call.
	 * 
	 * @see MsgType#AcceptCall
	 * @see MsgType#AnswerCall
	 * @see MsgType#ConferenceCall
	 * @see MsgType#EstablishedCall
	 * @see MsgType#EstablishedMedia
	 * @see MsgType#HangupCall
	 * @see MsgType#HeldCall
	 * @see MsgType#IncomingCall
	 * @see MsgType#InitiatedCall
	 * @see MsgType#RedirectCall
	 * @see MsgType#RejectCall
	 * @see MsgType#RingingCall
	 * @see MsgType#SetCallId
	 * @see MsgType#SetCallIdOk
	 * @see MsgType#TransferCall
	 */
	public final static int CallId = 40;

	/**
	 * The cause of a hangup. This is a string token such as "NORMAL",
	 * "CALL_CANCELLED", or "DEST_NOT_OBTAINABLE".
	 * 
	 * @see MsgType#HangupCall
	 */
	public static final int Cause = 46;
	
	/**
	 * The codec identifier.
	 * 
	 * @see Codec
	 * @see MsgType#RegisterMediaCaps
	 * @see MsgType#UnregisterMediaCaps
	 */
	public final static int Codec = 25;
	
	/**
	 * The address of a cti manager.
	 * 
	 * @see MsgType#Register
	 */
	public final static int CtiManager = 2;
	
	/**
	 * The name of a cti port or route point.
	 * 
	 * @see MsgType#Register
	 * @see MsgType#Unregister
	 * @see MsgType#MakeCall
	 * @see MsgType#StatusUpdate
	 * @see MsgType#MakeCallAccepted
	 * @see MsgType#IncomingCall
	 * @see MsgType#RingingCall
	 * @see MsgType#HangupCall
	 * @see MsgType#EstablishedCall
	 * @see MsgType#EstablishedMedia
	 * @see MsgType#HeldCall
	 * @see MsgType#InitiatedCall
	 */
	public final static int DeviceName = 5;
	
	/**
	 * The type of a device (cti port or route point)
	 * 
	 * @see DeviceType
	 * @see MsgType#Register
	 * @see MsgType#Unregister
	 * @see MsgType#MakeCall
	 * @see MsgType#StatusUpdate
	 * @see MsgType#MakeCallAccepted
	 * @see MsgType#IncomingCall
	 * @see MsgType#RingingCall
	 * @see MsgType#HangupCall
	 * @see MsgType#EstablishedCall
	 * @see MsgType#EstablishedMedia
	 * @see MsgType#HeldCall
	 * @see MsgType#InitiatedCall
	 */
	public final static int DeviceType = 6;
	
	/**
	 * The digits that were dialed. NOT YET USED.
	 */
	public static final int Digits = 51;
	
	/**
	 * The failure reason.
	 * 
	 * @see FailReason
	 * @see MsgType#Error
	 */
	public final static int FailReason = 0;
	
	/**
	 * The framesize for the codec (in milliseconds).
	 * 
	 * @see MsgType#RegisterMediaCaps
	 * @see MsgType#UnregisterMediaCaps
	 */
	public final static int Framesize = 26;
	
	/**
	 * The "from" address of a call.
	 * 
	 * @see MsgType#EstablishedCall
	 * @see MsgType#EstablishedMedia
	 * @see MsgType#HangupCall
	 * @see MsgType#HeldCall
	 * @see MsgType#IncomingCall
	 * @see MsgType#InitiatedCall
	 * @see MsgType#MakeCall
	 * @see MsgType#MakeCallAccepted
	 * @see MsgType#RingingCall
	 */
	public final static int From = 42;

	/**
	 * A name for the "from" side of a call.
	 * 
	 * @see MsgType#MakeCall
	 * @see MsgType#MakeCallAccepted
	 */
	public static final int FromCallId = 48;

	/**
	 * The message of a GeneralFailure.
	 * 
	 * @see MsgType#Error
	 */
	public static final int Message = 99;

	/**
	 * The field that is missing.
	 * 
	 * @see MsgField
	 * @see MsgType#Error
	 */
	public static final int MessageField = 50;
	
	/**
	 * The message type associated with a failure (if any).
	 * 
	 * @see MsgType
	 * @see MsgType#Error
	 */
	public static final int MessageType = 7;
	
	/**
	 * The new call id when a rename is requested.
	 * 
	 * @see MsgType#AcceptCall
	 * @see MsgType#SetCallId
	 * @see MsgType#SetCallIdOk
	 */
	public final static int NewCallId = 45;
	
	/**
	 * The original "to" address of the call before any redirection.
	 * 
	 * @see MsgType#EstablishedCall
	 * @see MsgType#EstablishedMedia
	 * @see MsgType#HangupCall
	 * @see MsgType#HeldCall
	 * @see MsgType#IncomingCall
	 * @see MsgType#InitiatedCall
	 * @see MsgType#RingingCall
	 */
	public final static int OriginalTo = 43;

	/**
	 * A second call to be merged into the first call.
	 * 
	 * @see MsgType#ConferenceCall
	 */
	public static final int OtherCallId = 47;
	
	/**
	 * The password for the user.
	 * 
	 * @see MsgType#Register
	 */
	public final static int Password = 4;
	
	/**
	 * The address to receive audio data for a made or answered call.
	 * 
	 * @see MsgType#MakeCall
	 * @see MsgType#AnswerCall
	 */
	public final static int RxIp = 23;
	
	/**
	 * The port to receive audio data for a made or answered call.
	 * 
	 * @see MsgType#MakeCall
	 * @see MsgType#AnswerCall
	 */
	public final static int RxPort = 24;
	
	/**
	 * The device status.
	 * 
	 * @see Status
	 * @see MsgType#StatusUpdate
	 */
	public final static int Status = 1;
	
	/**
	 * The "to" address of a call.
	 * 
	 * @see MsgType#EstablishedCall
	 * @see MsgType#EstablishedMedia
	 * @see MsgType#HangupCall
	 * @see MsgType#HeldCall
	 * @see MsgType#MakeCall
	 * @see MsgType#MakeCallAccepted
	 * @see MsgType#IncomingCall
	 * @see MsgType#InitiatedCall
	 * @see MsgType#RedirectCall
	 * @see MsgType#RingingCall
	 * @see MsgType#TransferCall
	 */
	public final static int To = 41;

	/**
	 * A name for the "to" connection of a call.
	 * 
	 * @see MsgType#MakeCall
	 * @see MsgType#MakeCallAccepted
	 */
	public static final int ToCallId = 49;
	
	/**
	 * The address to send audio data for an established call.
	 *  
	 * @see MsgType#EstablishedMedia
	 */
	public final static int TxIp = 21;
	
	/**
	 * The port to send audio data for an established call.
	 * 
	 * @see MsgType#EstablishedMedia
	 */
	public final static int TxPort = 22;
	
	/**
	 * The user name used to login to a cti manager.
	 * 
	 * @see MsgType#Register
	 */
	public final static int Username = 3;

	/**
	 * Source of the button press: 1 == peer, 2 == self
	 */
	public static final int ButtonSource = 54;

	/**
	 * Monitor the device for third party call control: 0 == false, 1 == true.
	 * 
	 * @see MsgType#Register
	 */
	public static final int ThirdParty = 55;
	
	/**
	 * The level of event reporting.
	 * 
	 * @see MsgType#SetLogLevel
	 */
	public static final int Level = 56;
}
