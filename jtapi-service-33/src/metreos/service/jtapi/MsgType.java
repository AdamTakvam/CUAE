/* $Id: MsgType.java 32402 2007-05-25 16:03:46Z rvarghee $
 * 
 * Created by achaney on Jan 28, 2005
 *
 * Copyright (c) 2005 Metreos, Inc. All rights reserved.
 */
package metreos.service.jtapi;

/**
 * This interface defines the IPC flatmap message types used when
 * communicating with the AppServer's JTAPI provider. 
 * 
 * Note: The constants defined here must correspond exactly with
 * those defined in the MsgType enumeration in JTapiProxy.cs in
 * the JTAPI provider.
 * 
 * 'Error' and 'Ack' are sent by the JTapiServer to indicate the
 * failure or success of certain actions. The only exception is
 * IncomingCall, where the provider will send an 'Ack' immediately
 * to the stack for the sole purpose of communicating the call ID. 
 */
public interface MsgType 
{
	//////////////
	// REQUESTS //
	//////////////
	
	/**
	 * Requests the server to start monitoring some devices.
	 * 
	 * Args:
	 *    CtiManager+
	 *    Username
	 *    Password
	 *    DeviceType
	 *    DeviceName+
	 * 
	 * Returns Error for any missing fields, bad values, or exceptions.
	 * 
	 * Returns StatusUpdate for any changes in address, device,
	 * or provider status.
	 * 
	 * @see #StatusUpdate
	 * @see #Error
	 */
	public final static int Register = 1;
	
	/**
	 * Requests the server to stop monitoring some devices.
	 * 
	 * Args:
	 *    DeviceType
	 *    DeviceName+
	 * 
	 * Returns Error for any missing fields, bad values, or exceptions.
	 * 
	 * Returns StatusUpdate for any changes in address, device,
	 * or provider status.
	 * 
	 * @see #StatusUpdate
	 * @see #Error
	 */
	public final static int Unregister = 2;
	
	/**
	 * Requests the server to register the media capabilities.
	 * 
	 * Args:
	 *    Codec
	 *    Framesize+
	 * 
	 * Returns Error for any missing fields, bad values, or exceptions.
	 * 
	 * @see #Error
	 */
	public final static int RegisterMediaCaps = 4;
	
	/**
	 * Requests the server to unregister the media capabilities.
	 * 
	 * Args:
	 *    Codec
	 *    Framesize+
	 * 
	 * Returns Error for any missing fields, bad values, or exceptions.
	 * 
	 * @see #Error
	 */
	public final static int UnregisterMediaCaps = 5;
	
	/**
	 * Reports to the client that a device has changed status.
	 * 
	 * Args:
	 *    DeviceType
	 *    DeviceName
	 *    Status
	 * 
	 * @see DeviceType
	 * @see Status
	 */
	public final static int StatusUpdate = 3;

	/**
	 * Requests the server to send some digits. This is more
	 * or less the same as if the user entered them on the
	 * keypad.
	 * 
	 * Args:
	 *    CallId
	 *    Digits
	 */
	public static final int SendUserInput = 96;

	/**
	 * Reports to the client that a digit has been recognised.
	 * 
	 * Args:
	 *    CallId
	 *    Digits
	 */
	public static final int GotDigits = 97;

	//////////////////
	// CALL CONTROL //
	//////////////////
	
	/**
	 * Requests the server to make a call from a device. If From is not
	 * specified, picks some address on the device. If the device is a
	 * media device (cti port or route point) then rxIP and rxPort must
	 * be specified.
	 * 
	 * Args:
	 *    [FromCallId]
	 *    [ToCallId]
	 *    DeviceType
	 *    DeviceName
	 *    [From]
	 *    To
	 *    [RxIP]
	 *    [RxPort]
	 * 
	 * Returns Error for any missing fields, bad values, or exceptions.
	 * Returns MakeCallAccepted if the make call request was accepted
	 * by the provider.
	 * 
	 * @see #MakeCallAccepted
	 */
	public final static int MakeCall = 21;

	/**
	 * Reports to the client that a MakeCall request was
	 * accepted by the provider. This doesn't mean the call
	 * will be made, just that it was accepted.
	 * 
	 * Args:
	 *    [FromCallId]
	 *    [ToCallId]
	 *    DeviceType
	 *    DeviceName
	 *    [From]
	 *    To
	 * 
	 * @see #MakeCall
	 */
	public static final int MakeCallAccepted = 33;

	/**
	 * Reports to the client that a call has been initiated.
	 * 
	 * Args:
	 *    CallId
	 *    DeviceType
	 *    DeviceName
	 *    From
	 *    To
	 *    OriginalTo
	 */
	public static final int InitiatedCall = 35;
	
	/**
	 * Reports to the client that there is an incoming call. Client
	 * should respond with AcceptCall, RejectCall, or RedirectCall.
	 * 
	 * Args:
	 *    CallId
	 *    DeviceType
	 *    DeviceName
	 *    From
	 *    To
	 *    OriginalTo
	 * 
	 * @see #AcceptCall
	 * @see #RejectCall
	 * @see #RedirectCall
	 */
	public final static int IncomingCall = 20;
	
	/**
	 * Requests the server to accept an incoming call
	 * and optionally rename it. Server will accept
     * the call and may then respond with Ringing or
     * HangupCall. Sent by the client in response to
     * IncomingCall.
	 * 
	 * Args:
	 *    [OldCallId]
	 *    CallId
	 * 
	 * Returns Error for any missing fields, bad values, or exceptions.
	 * 
	 * @see #IncomingCall
	 * @see #RingingCall
	 * @see #HangupCall
	 * @see #Error
	 */
	public final static int AcceptCall = 26;
	
	/**
	 * Redirects an incoming call to another line.
	 * 
	 * Args:
	 *    CallId
	 *    To
	 * 
	 * Returns Error for any missing fields, bad values, or exceptions.
	 * 
	 * @see #IncomingCall
	 * @see #AcceptCall
	 * @see #RejectCall
	 * @see #Error
	 */
	public static final int RedirectCall = 27;
	
	/**
	 * Requests the server to reject an incoming call.
	 * Server will reject the call and may then respond
	 * with HangupCall. Sent by the client in response to
     * IncomingCall.
	 * 
	 * Args:
	 *    CallId
	 * 
	 * Returns Error for any missing fields, bad values, or exceptions.
	 * 
	 * @see #IncomingCall
	 * @see #HangupCall
	 * @see #Error
	 */
	public final static int RejectCall = 24;

	/**
	 * Reports to the client that there is a ringing call. Client
	 * might respond with AnswerCall.
	 * 
	 * Args:
	 *    CallId
	 *    DeviceType
	 *    DeviceName
	 *    From
	 *    To
	 *    OriginalTo
	 * 
	 * @see #AnswerCall
	 */
	public static final int RingingCall = 99;
	
	/**
	 * Requests the server to answer a call. Server will
	 * answer and then may respond with EstablishedCall
	 * or HangupCall. If the device is a media device
	 * (cti port or route point) then rxIP and rxPort
	 * must be specified.
	 * 
	 * Args:
	 *    CallId
	 *    [RxIP]
	 *    [RxPort]
	 * 
	 * Returns Error for any missing fields, bad values, or exceptions.
	 * 
	 * @see #EstablishedCall
	 * @see #HangupCall
	 * @see #Error
	 */
	public final static int AnswerCall = 22;
	
	/**
	 * Reports to the client that there is an established call.
	 * 
	 * Args:
	 *    CallId
	 *    DeviceType
	 *    DeviceName
	 *    From
	 *    To
	 *    OriginalTo
	 */
	public static final int EstablishedCall = 31;

	/**
	 * Reports to the client that media params have been established.
	 * 
	 * Args:
	 * 
	 * Args:
	 *    CallId
	 *    DeviceType
	 *    DeviceName
	 *    From
	 *    To
	 *    OriginalTo
	 *    TxIP
	 *    TxPort
	 */
	public static final int EstablishedMedia = 98;

	/**
	 * Reports to the client that a call was put on hold. Any
	 * terminal sharing a line with the call may pick it up.
	 * 
	 * Args:
	 *    CallId
	 *    DeviceType
	 *    DeviceName
	 *    From
	 *    To
	 *    OriginalTo
	 */
	public static final int HeldCall = 34;

	/**
	 * Reports to the client that a held call was picked up
	 * by another terminal sharing the line with this call.
	 * 
	 * Args:
	 *    CallId
	 *    DeviceType
	 *    DeviceName
	 *    From
	 *    To
	 *    OriginalTo
	 */
	public static final int InUseCall = 37;

	/**
	 * Transfers an established call to another line.
	 * 
	 * Args:
	 *    CallId
	 *    To
	 * 
	 * Returns Error for any missing fields, bad values, or exceptions.
	 * 
	 * @see #EstablishedCall
	 * @see #Error
	 */
	public static final int TransferCall = 28;

	/**
	 * Conferences two established calls together.
	 * 
	 * Args:
	 *    CallId
	 *    OtherCallId
	 * 
	 * Returns Error for any missing fields, bad values, or exceptions.
	 * 
	 * @see #EstablishedCall
	 * @see #Error
	 */
	public static final int ConferenceCall = 29;
	
	/**
	 * Requests the server to disconnect from an answered call,
	 * or reports to the client that a disconnection from a call.
	 * In the case of a report to the client, the cause is a
	 * string token which notes the cause of the disconnect,
	 * and the device is also specified.
	 * 
	 * Args:
	 *    CallId
	 *    [DeviceType]
	 *    [DeviceName]
	 *    [From]
	 *    [To]
	 *    [OriginalTo]
	 *    [Cause]
	 *    [CallControlCause]
	 * 
	 * Returns Error for any missing fields, bad values, or exceptions.
	 * 
	 * @see #Error
	 */
	public final static int HangupCall = 25;

	//////////
	// MISC //
	//////////
	
	/**
	 * Reports to the client that there is an error in a request.
	 * 
	 * Args:
	 *    FailReason
	 *    [Message]
	 *    [MessageType]
	 *    [MessageField]
	 *    (other args depend upon the reason)
	 * 
	 * @see FailReason
	 */
	public final static int Error = 0;
	
	/**
	 * Renames a call.
	 * 
	 * Args:
	 *    OldCallId
	 *    CallId
	 * 
	 * Returns SetCallIdOk if the operation worked. Until the client
	 * sees this message, it must continue to expect and use the old
	 * id.
	 * 
	 * Returns Error for any missing fields, bad values, or exceptions.
	 * 
	 * @see #SetCallIdOk
	 * @see #Error
	 */
	public static final int SetCallId = 30;

	/**
	 * Reports to the client that a call has been renamed.
	 * 
	 * Args:
	 *    OldCallId
	 *    CallId
	 * 
	 * @see #SetCallId
	 */
	public static final int SetCallIdOk = 36;

	/**
	 * Description of Answered
	 */
	public static final int Answered = 45;

	/**
	 * Description of SetMedia.
	 */
	public static final int SetMedia = 46;
	
	/**
	 * Sets the log level of the jtapi server.
	 * 
	 * Args:
	 *    Level
	 * 
	 * Returns Error for any missing fields, bad values, or exceptions.
	 */
	public static final int SetLogLevel = 50;
}
