/* $Id: FailReason.java 32413 2007-05-25 21:07:53Z rvarghee $
 * 
 * Created by achaney on Jan 28, 2005
 *
 * Copyright (c) 2005 Metreos, Inc. All rights reserved.
 */
package metreos.service.jtapi;

/**
 * Defines the set of error conditions which can be handled in code.
 */
public interface FailReason 
{
	/**
	 * The device name specified to MakeCall is invalid. Which is to say,
	 * it has not been registered or does not exist.
	 * 
	 * Args:
	 *    MessageType
	 *    [FromCallId]
	 *    [ToCallId]
	 *    DeviceType
	 *    DeviceName
	 * 
	 * @see MsgType#MakeCall
	 */
	public final static int InvalidDeviceName = 2;

	/**
	 * The device type specified to Register, Unregister,
	 * or MakeCall is invalid.
	 * 
	 * Args:
	 *    MessageType
	 *    [FromCallId]
	 *    [ToCallId]
	 *    DeviceType
	 * 
	 * @see DeviceType
	 * @see MsgType#MakeCall
	 * @see MsgType#Register
	 * @see MsgType#Unregister
	 */
	public final static int InvalidDeviceType = 3;

	/**
	 * The message type is unknown.
	 * 
	 * Args:
	 *    MessageType
	 * 
	 * @see MsgType
	 */
	public static final int UnknownMessageType = 4;

	/**
	 * The "from" address specified to MakeCall is invalid.
	 * 
	 * Args:
	 *    MessageType
	 *    [FromCallId]
	 *    [ToCallId]
	 *    From
	 * 
	 * @see MsgType#MakeCall
	 */
	public static final int InvalidDn = 6;

	/**
	 * The "from" device has no provider.
	 * 
	 * Args:
	 *    MessageType
	 *    [FromCallId]
	 *    [ToCallId]
	 *    DeviceType
	 *    DeviceName
	 * 
	 * @see MsgType#MakeCall
	 */
	public static final int NoProvider = 7;

	/**
	 * The from device has no terminal.
	 * 
	 * Args:
	 *    MessageType
	 *    Message
	 *    Args
	 * 
	 * @see MsgType
	 */
	public static final int GeneralFailure = 9;

	/**
	 * The call id is undefined or in use. It isn't specified
	 * which one or what specifically the problem is.
	 * 
	 * Args:
	 *    MessageType
	 *    CallId
	 *    [NewCallId]
	 *    [OtherCallId]
	 */
	public static final int CallIdUnknown = 10;

	/**
	 * The codec is not one of the supposed codecs, or the
	 * framesize is not supported for that codec.
	 * 
	 * Args:
	 *    MessageType
	 *    Codec
	 *    Framesize
	 * 
	 * @see Codec
	 * @see MsgType#RegisterMediaCaps
	 * @see MsgType#UnregisterMediaCaps
	 */
	public static final int CodecNotSupported = 12;

	/**
	 * A required field is missing from a message.
	 * 
	 * Args:
	 *    MessageType
	 *    MessageField
	 */
	public static final int MissingField = 13;

	/**
	 * The provider cannot be contacted. Either the address is
	 * bad, the username is bad, or the password is bad.
	 * 
	 * Args:
	 *    CtiManager
	 *    Username
	 *    Password
	 *    Message
	 * 
	 * @see MsgField#CtiManager
	 * @see MsgField#Username
	 * @see MsgField#Password
	 * @see MsgField#Message
	 */
	public static final int BadProvider = 14;
	
	/**
	 * The destination is invalid. This will be the case where
	 * the 'To' field has invalid numbers or characters.
	 * 
	 * Args:
	 *    MessageType
	 *    Message
	 *    Args
	 * 
	 * @see MsgType
	 */
	public static final int InvalidDestination = 15;
	
	/**
	 * This failure is received when the JTAPI stack is unable
	 * to process a request. This will be the case where timeouts
	 * occur due to a particular precondition not being set.
	 * 
	 * Args:
	 *    MessageType
	 *    Message
	 *    Args
	 * 
	 * @see MsgType
	 */
	public static final int PlatformException = 16;	
	
}
