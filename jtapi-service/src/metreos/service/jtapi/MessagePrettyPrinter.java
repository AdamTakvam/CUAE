/* $Id: MessagePrettyPrinter.java 32413 2007-05-25 21:07:53Z rvarghee $
 *
 * Created by wert on Feb 9, 2005
 *
 * Copyright (c) 2005 Metreos, Inc. All rights reserved.
 */

package metreos.service.jtapi;

import java.util.Iterator;

import com.cisco.jtapi.CiscoJtapiExceptionDefinition;

import metreos.core.ipc.flatmaps.FlatmapException;
import metreos.core.ipc.flatmaps.FlatmapIpcMessage;
import metreos.core.ipc.flatmaps.FlatmapList;
import metreos.core.ipc.flatmaps.PrettyPrinter;
import metreos.core.ipc.flatmaps.FlatmapList.MapEntry;

/**
 * A JTapi message pretty printer.
 */
public class MessagePrettyPrinter implements PrettyPrinter
{
	/* (non-Javadoc)
	 * @see metreos.core.ipc.flatmaps.PrettyPrinter#format(metreos.core.ipc.flatmaps.FlatmapIpcMessage)
	 */
	public String format( FlatmapIpcMessage message )
	{
		StringBuffer sb = new StringBuffer();
		sb.append( formatMessageType( message.messageType ) );
		formatFlatmapList( sb, message.args );
		return sb.toString();
	}

	/**
	 * @param messageType
	 * @return string format of message type.
	 */
	private String formatMessageType( int messageType )
	{
		return xlatMessageType( messageType );
	}

	/**
	 * @param messageType
	 * @return string format of message type.
	 */
	private String xlatMessageType( int messageType )
	{
		switch (messageType)
		{
			case MsgType.AcceptCall: return "AcceptCall";
			case MsgType.AnswerCall: return "AnswerCall";
			case MsgType.Answered: return "Answered";
			case MsgType.ConferenceCall: return "ConferenceCall";
			case MsgType.Error: return "Error";
			case MsgType.EstablishedCall: return "EstablishedCall";
			case MsgType.EstablishedMedia: return "EstablishedMedia";
			case MsgType.GotDigits: return "GotDigits";
			case MsgType.HangupCall: return "HangupCall";
			case MsgType.HeldCall: return "HeldCall";
			case MsgType.IncomingCall: return "IncomingCall";
			case MsgType.InitiatedCall: return "InitiatedCall";
			case MsgType.InUseCall: return "InUseCall";
			case MsgType.MakeCall: return "MakeCall";
			case MsgType.MakeCallAccepted: return "MakeCallAccepted";
			case MsgType.RedirectCall: return "RedirectCall";
			case MsgType.Register: return "Register";
			case MsgType.RegisterMediaCaps: return "RegisterMediaCaps";
			case MsgType.RejectCall: return "RejectCall";
			case MsgType.RingingCall: return "RingingCall";
			case MsgType.SendUserInput: return "SendUserInput";
			case MsgType.SetCallId: return "SetCallId";
			case MsgType.SetCallIdOk: return "SetCallIdOk";
			case MsgType.SetLogLevel: return "SetLogLevel";
			case MsgType.SetMedia: return "SetMedia";
			case MsgType.StatusUpdate: return "StatusUpdate";
			case MsgType.TransferCall: return "TransferCall";
			case MsgType.Unregister: return "Unregister";
			case MsgType.UnregisterMediaCaps: return "UnregisterMediaCaps";
			case MsgType.UseMohMedia: return "UseMohMedia";
			default: return "*unknownMessageType="+messageType+"*";
		}
	}

	/**
	 * @param sb
	 * @param args
	 */
	private void formatFlatmapList( StringBuffer sb, FlatmapList args )
	{
		sb.append( "{ " );
		Iterator<MapEntry> i = args.iterator();
		boolean first = true;
		while (i.hasNext())
		{
			MapEntry me = i.next();
			
			if (!first)
				sb.append( ", " );
			else
				first = false;
			
			sb.append( formatMessageField( me.key ) );
			sb.append( " = " );
			formatValue( sb, me.key, me.dataType, me.dataValue );
		}
		sb.append( " }" );
	}

	/**
	 * @param messageField
	 */
	private String formatMessageField( int messageField )
	{
		return xlatMessageField( messageField );
	}

	/**
	 * @param messageField
	 */
	private String xlatMessageField( int messageField )
	{
		switch (messageField)
		{
			case MsgField.Args: return "Args";
			case MsgField.ButtonSource: return "ButtonSource";
			case MsgField.CallControlCause: return "CallControlCause";
			case MsgField.CallId: return "CallId";
			case MsgField.Cause: return "Cause";
			case MsgField.Codec: return "Codec";
			case MsgField.CtiManager: return "CtiManager";
			case MsgField.DeviceName: return "DeviceName";
			case MsgField.DeviceType: return "DeviceType";
			case MsgField.Digits: return "Digits";
			case MsgField.FailReason: return "FailReason";
			case MsgField.Framesize: return "Framesize";
			case MsgField.From: return "From";
			case MsgField.FromCallId: return "FromCallId";
			case MsgField.Level: return "Level";
			case MsgField.Message: return "Message";
			case MsgField.MessageField: return "MessageField";
			case MsgField.MessageType: return "MessageType";
			case MsgField.NewCallId: return "NewCallId";
			case MsgField.OriginalTo: return "OriginalTo";
			case MsgField.OtherCallId: return "OtherCallId";
			case MsgField.Password: return "Password";
			case MsgField.RxIp: return "RxIP";
			case MsgField.RxPort: return "RxPort";
			case MsgField.Status: return "Status";
			case MsgField.ThirdParty: return "ThirdParty";
			case MsgField.To: return "To";
			case MsgField.ToCallId: return "ToCallId";
			case MsgField.TxIp: return "TxIP";
			case MsgField.TxPort: return "TxPort";
			case MsgField.Username: return "Username";
			default: return "*unknownMessageField="+messageField+"*";
		}
	}
	
	/**
	 * @param ccnException
	 * @return a description of a ccnexception
	 */
	public String xlateCCNException( int ccnException )
	{
		return CiscoJtapiExceptionDefinition.getErrName( ccnException )+
		": "+CiscoJtapiExceptionDefinition.getErrDescription( ccnException );
	}

	/**
	 * @param sb
	 * @param dataValue
	 */
	private void formatValue( StringBuffer sb, int key, int dataType,
		Object dataValue )
	{
		sb.append( "(" );
		sb.append( xlatDataType( dataType ) );
		sb.append( ") " );
		formatDataValue( sb, key, dataType, dataValue );
	}

	/**
	 * @param dataType
	 */
	private String xlatDataType( int dataType )
	{
		switch (dataType)
		{
			case FlatmapList.VT_BYTE: return "b";
			case FlatmapList.VT_FLATMAP: return "f";
			case FlatmapList.VT_INT: return "i";
			case FlatmapList.VT_STRING: return "s";
			default: return "*u*";
		}
	}

	/**
	 * @param sb
	 * @param key
	 * @param dataType
	 * @param dataValue
	 */
	private void formatDataValue( StringBuffer sb, int key, int dataType,
		Object dataValue )
	{
		switch (dataType)
		{
			case FlatmapList.VT_BYTE:
				formatBytes( sb, (byte[]) dataValue );
				break;
			
			case FlatmapList.VT_FLATMAP:
				if (dataValue instanceof FlatmapList)
					formatFlatmap( sb, (FlatmapList) dataValue );
				else
					formatFlatmap( sb, (byte[]) dataValue );
				break;
			
			default:
				switch (key)
				{
					case MsgField.Codec:
						sb.append( xlatCodec( ((Integer) dataValue).intValue() ) );
						break;
					
					case MsgField.DeviceType:
						sb.append( xlatDeviceType( ((Integer) dataValue).intValue() ) );
						break;
					
					case MsgField.FailReason:
						sb.append( xlatFailReason( ((Integer) dataValue).intValue() ) );
						break;
					
					case MsgField.MessageField:
						sb.append( xlatMessageField( ((Integer) dataValue).intValue() ) );
						break;
					
					case MsgField.MessageType:
						sb.append( xlatMessageType( ((Integer) dataValue).intValue() ) );
						break;
					
					case MsgField.Status:
						sb.append( xlatStatus( ((Integer) dataValue).intValue() ) );
						break;
					
					default:
						sb.append( dataValue );
						break;
				}
				break;
		}
	}

	/**
	 * @param i
	 * @return string rep
	 */
	private String xlatCodec( int i )
	{
		switch (i)
		{
			case Codec.Unspecified: return "unspec";
			case Codec.G711a: return "G711a";
			case Codec.G711u: return "G711u";
			case Codec.G723: return "G723";
			case Codec.G729: return "G729";
			default: return "*unknownCodec*("+i+")";
		}
	}

	/**
	 * @param i
	 * @return string rep
	 */
	private String xlatDeviceType( int i )
	{
		switch (i)
		{
			case DeviceType.CtiPort: return "CtiPort";
			case DeviceType.RoutePoint: return "RoutePoint";
			case DeviceType.MonitoredDevice: return "MonitoredDevice";
			default: return "*unknownDeviceType*("+i+")";
		}
	}

	/**
	 * @param i
	 * @return string rep
	 */
	private String xlatFailReason( int i )
	{
		switch (i)
		{
			case FailReason.BadProvider: return "BadProvider";
			case FailReason.CallIdUnknown: return "CallIdUnknown";
			case FailReason.CodecNotSupported: return "CodecNotSupported";
			case FailReason.GeneralFailure: return "GeneralFailure";
			case FailReason.InvalidDeviceName: return "InvalidDeviceName";
			case FailReason.InvalidDeviceType: return "InvalidDeviceType";
			case FailReason.InvalidDn: return "InvalidDn";
			case FailReason.MissingField: return "MissingField";
			case FailReason.NoProvider: return "NoProvider";
			case FailReason.InvalidDestination: return "InvalidDestination";
			case FailReason.PlatformException: return "PlatformException";			
			case FailReason.UnknownMessageType: return "UnknownMessageType";
			default: return "*unknownFailReason*("+i+")";
		}
	}

	/**
	 * @param i
	 * @return string rep
	 */
	private String xlatStatus( int i )
	{
		switch (i)
		{
			case Status.DeviceOffline: return "DeviceOffline";
			case Status.DeviceOnline: return "DeviceOnline";
			default: return "*unknownStatus*("+i+")";
		}
	}

	/**
	 * @param sb
	 * @param bytes
	 */
	private void formatBytes( StringBuffer sb, byte[] bytes )
	{
		int n = bytes.length;
		sb.append( n );
		sb.append( ": " );
		for (int i = 0; i < n; i++)
			formatByte( sb, bytes[i] );
	}

	/**
	 * @param sb
	 * @param b
	 */
	private void formatByte( StringBuffer sb, byte b )
	{
		sb.append( toHex( (b >>> 4) & 255 ) );
		sb.append( toHex( b & 255 ) );
	}

	/**
	 * @param i value between 0 and 15
	 * @return the hex char for i
	 */
	private char toHex( int i )
	{
		if (i < 10)
			return (char) ('0' + i);
		
		return (char) ('A' + i - 10);
	}

	/**
	 * @param sb
	 * @param fml
	 */
	private void formatFlatmap( StringBuffer sb, FlatmapList fml )
	{
		formatFlatmapList( sb, fml );
	}

	/**
	 * @param sb
	 * @param bs
	 */
	private void formatFlatmap( StringBuffer sb, byte[] bs )
	{
		try
		{
			formatFlatmapList( sb, new FlatmapList( bs ) );
		}
		catch ( FlatmapException e )
		{
			formatBytes( sb, bs );
		}
	}
}
