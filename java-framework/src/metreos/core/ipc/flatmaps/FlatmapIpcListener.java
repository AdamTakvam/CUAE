/* $Id: FlatmapIpcListener.java 11564 2005-10-07 14:52:59Z wert $
 *
 * Created by wert on Jan 27, 2005
 *
 * Copyright (c) 2005 Metreos, Inc. All rights reserved.
 */

package metreos.core.ipc.flatmaps;

import metreos.core.net.ipc.IpcConnection;

/**
 * A listener for Flatmap IPC connection events.
 * @param <T> the actual ipc connection type we are using.
 */
public interface FlatmapIpcListener<T extends IpcConnection>
{
	/**
	 * Notifies that the connection has been started.
	 * 
	 * @param connection the connection that has been started.
	 * 
	 * @throws Exception if the connection startup failed or
	 * is not allowed.
	 */
	void startup( T connection ) throws Exception;

	/**
	 * Notifies that a message has been received on the connection.
	 * 
	 * @param connection the connection that received the message.
	 * 
	 * @param message the message that has been received
	 */
	void received( T connection, FlatmapIpcMessage message );

	/**
	 * Notifies that the connection has been shutdown.
	 * 
	 * @param connection the connection that has been shutdown.
	 */
	void shutdown( T connection );

	/**
	 * Notifies that an exception was caught by the connection.
	 * 
	 * @param connection the connection that has caught an exception.
	 * 
	 * @param e the exception that was caught.
	 */
	void exception( T connection, Exception e );
}
