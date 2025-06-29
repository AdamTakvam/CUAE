/* $Id: FlatmapIpcListenerAdapter.java 11564 2005-10-07 14:52:59Z wert $
 *
 * Created by wert on Jan 27, 2005
 *
 * Copyright (c) 2005 Metreos, Inc. All rights reserved.
 */

package metreos.core.ipc.flatmaps;

import metreos.core.net.ipc.IpcConnection;
import metreos.core.net.ipc.IpcListener;
import metreos.core.net.ipc.IpcMessage;

/**
 * An adapter which translates IpcListener interface
 * to FlatmapIpcListener interface.
 * @param <T> The type of ipc connection
 */
public class FlatmapIpcListenerAdapter<T extends IpcConnection> implements IpcListener<T>
{
	/**
	 * Constructs an adapter from IpcListener to FlatmapIpcListener.
	 * 
	 * @param listener the flatmap ipc listener.
	 * @param printer something to nicely print flatmap ipc messages.
	 */
	public FlatmapIpcListenerAdapter( FlatmapIpcListener<T> listener,
		PrettyPrinter printer )
	{
		this.listener = listener;
		this.printer = printer;
	}

	private final FlatmapIpcListener<T> listener;
	
	private final PrettyPrinter printer;
	
	public void startup( T connection ) throws Exception
	{
		listener.startup( connection );
	}

	public void received( T connection, IpcMessage message )
	{
		FlatmapIpcMessage fmessage = null;
		
		try
		{
			fmessage = new FlatmapIpcMessage( message.getBytes(), printer, connection );
		}
		catch ( FlatmapException e )
		{
			exception( connection, e );
			return;
		}
		
		listener.received( connection, fmessage );
	}

	public void shutdown( T connection )
	{
		listener.shutdown( connection );
	}

	public void exception( T connection, Exception e )
	{
		listener.exception( connection, e );
	}
}
