/* $Id: StdPrettyPrinter.java 19261 2006-01-12 17:21:45Z wert $
 *
 * Created by wert on Feb 9, 2005
 *
 * Copyright (c) 2005 Metreos, Inc. All rights reserved.
 */

package metreos.core.ipc.flatmaps;

import java.util.Iterator;

import metreos.core.ipc.flatmaps.FlatmapList.MapEntry;


/**
 * A JTapi message pretty printer.
 */
public class StdPrettyPrinter implements PrettyPrinter, StdMessageDefs
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
	protected String xlatMessageType( int messageType )
	{
		switch (messageType)
		{
			case Error: return "Error";
			default: return Integer.toString( messageType );
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
			
			sb.append( xlatMessageField( me.key ) );
			sb.append( '=' );
			formatValue( sb, me.key, me.dataType, me.dataValue );
		}
		sb.append( " }" );
	}

	/**
	 * @param messageField
	 * @return string format of message field
	 */
	protected String xlatMessageField( int messageField )
	{
		switch (messageField)
		{
			case Args: return "Args";
			case ErrorMessage: return "ErrorMessage";
			case FailReason: return "FailReason";
			case MessageField: return "MessageField";
			case MessageType: return "MessageType";
			default: return Integer.toString( messageField );
		}
	}

	/**
	 * @param sb
	 * @param key 
	 * @param dataType 
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
	 * @return string rep of the specified dataType
	 */
	private String xlatDataType( int dataType )
	{
		switch (dataType)
		{
			case FlatmapList.VT_BYTE: return "b";
			case FlatmapList.VT_FLATMAP: return "f";
			case FlatmapList.VT_INT: return "i";
			case FlatmapList.VT_STRING: return "s";
			case FlatmapList.VT_LONG: return "l";
			case FlatmapList.VT_DOUBLE: return "d";
			default: return Integer.toString( dataType );
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
			
			case FlatmapList.VT_INT:
				if (!formatCodedField( sb, key, ((Integer) dataValue).intValue() ))
					sb.append( dataValue );
				break;
			
			default:
				sb.append( dataValue );
				break;
		}
	}

	/**
	 * @param sb
	 * @param key
	 * @param value
	 * @return true if the coded field was decoded, false otherwise.
	 */
	protected boolean formatCodedField( StringBuffer sb, int key, int value )
	{
		switch (key)
		{
			case FailReason:
				sb.append( xlatFailReason( value ) );
				return true;
			
			case MessageField:
				sb.append( xlatMessageField( value ) );
				return true;
		}
		return false;
	}

	/**
	 * @param failReason
	 * @return string value of fail reason
	 */
	protected String xlatFailReason( int failReason )
	{
		switch (failReason)
		{
			case GeneralFailure: return "GeneralFailure";
			case MissingField: return "MissingField";
			case UnknownMessageType: return "UnknownMessageType";
			default: return Integer.toString( failReason );
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
