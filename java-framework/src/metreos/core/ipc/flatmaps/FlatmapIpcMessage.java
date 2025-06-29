/* $Id: FlatmapIpcMessage.java 30151 2007-03-06 21:47:46Z wert $
 *
 * Created by wert on Jan 27, 2005
 *
 * Copyright (c) 2005 Metreos, Inc. All rights reserved.
 */

package metreos.core.ipc.flatmaps;

import metreos.core.net.ipc.IpcConnection;
import metreos.core.net.ipc.IpcMessage;

/**
 * An ipc message with flatmap body.
 */
public class FlatmapIpcMessage implements IpcMessage
{
	/**
	 * Constructs a flatmap ipc message.
	 * 
	 * @param messageType the ipc message type.
	 * 
	 * @param args the ipc message args.
	 * 
	 * @param printer the message pretty printer.
	 * 
	 * @param connection the connection to send to.
	 */
	public FlatmapIpcMessage( int messageType, FlatmapList args,
		PrettyPrinter printer, IpcConnection connection )
	{
		this.messageType = messageType;
		this.args = args;
		this.printer = printer;
		this.connection = connection;
	}
	
	/**
	 * Constructs a flatmap ipc message.
	 * 
	 * @param messageType the ipc message type.
	 * 
	 * @param printer the message pretty printer.
	 * 
	 * @param connection the connection to send to.
	 */
	public FlatmapIpcMessage( int messageType, PrettyPrinter printer,
		IpcConnection connection )
	{
		this( messageType, new FlatmapList(), printer, connection );
	}
	
	/**
	 * Constructs a flatmap ipc message.
	 * 
	 * @param bytes an array of bytes (read from an ipc connection most likely).
	 * 
	 * @param printer the message pretty printer.
	 * 
	 * @param connection the connection to send to.
	 * 
	 * @throws FlatmapException
	 */
	public FlatmapIpcMessage( byte[] bytes, PrettyPrinter printer,
		IpcConnection connection ) throws FlatmapException
	{
		args = Flatmap.fromFlatmap( bytes );
		byte[] hebytes = args.getHeaderExtension();
		FlatmapHeaderExtension he = new FlatmapHeaderExtension( hebytes );
		messageType = he.messageType;
		this.printer = printer;
		this.connection = connection;
	}
	
	/**
	 * The ipc message type.
	 */
	public final int messageType;
	
	/**
	 * The ipc message args.
	 */
	public final FlatmapList args;
	
	/**
	 * The message pretty printer.
	 */
	public final PrettyPrinter printer;
	
	/**
	 * The connection to send to.
	 */
	public final IpcConnection connection;

	/* (non-Javadoc)
	 * @see metreos.core.net.ipc.IpcMessage#getFlag()
	 */
	public int getFlag()
	{
		return 0;
	}

	/* (non-Javadoc)
	 * @see metreos.core.net.ipc.IpcMessage#getFlag2()
	 */
	public int getFlag2()
	{
		return 0;
	}

	/* (non-Javadoc)
	 * @see metreos.core.net.ipc.IpcMessage#getBytes()
	 */
	public byte[] getBytes()
	{
		try
		{
			FlatmapHeaderExtension he = new FlatmapHeaderExtension( messageType );
			return args.toFlatmap( he.toArray() );
		}
		catch ( FlatmapException e )
		{
			throw new RuntimeException( "caught exception", e );
		}
	}

	@Override
	public String toString()
	{
		return toString( printer );
	}
	
	/**
	 * @param pp
	 * @return string rep using the supplied pretty printer, or some
	 * standard rep if pretty printer is not specified.
	 */
	public String toString( PrettyPrinter pp )
	{
		if (pp != null)
			return pp.format( this );
		
		return "FlatmapIpcMessage "+messageType+": "+args;
	}

	/**
	 * @param key
	 * @param value
	 * @return the FlatmapIpcMessage
	 */
	public FlatmapIpcMessage add( int key, int value )
	{
		args.add( key, value );
		return this;
	}

	/**
	 * @param key
	 * @param value
	 * @return the FlatmapIpcMessage
	 */
	public FlatmapIpcMessage add( int key, String value )
	{
		if (value == null)
			return this;
		
		args.add( key, value );
		return this;
	}

	/**
	 * @param key
	 * @param value
	 * @return the FlatmapIpcMessage
	 */
	public FlatmapIpcMessage add( int key, FlatmapList value )
	{
		if (value == null)
			return this;
		
		args.add( key, value );
		return this;
	}

	/**
	 * @param key
	 * @param value
	 * @return the FlatmapIpcMessage
	 */
	public FlatmapIpcMessage add( int key, byte[] value )
	{
		if (value == null)
			return this;
		
		args.add( key, value );
		return this;
	}

	/**
	 * @param key
	 * @param value
	 * @return the FlatmapIpcMessage
	 */
	public FlatmapIpcMessage add( int key, Integer value )
	{
		if (value == null)
			return this;
		
		args.add( key, value );
		return this;
	}

	/**
	 * Sends the message.
	 */
	public void send()
	{
		connection.send( this );
		//return this;
	}
}
