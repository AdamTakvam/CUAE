/* $Id: IpcListener.java 11564 2005-10-07 14:52:59Z wert $
 *
 * Created by wert on Jan 27, 2005
 *
 * Copyright (c) 2005 Metreos, Inc. All rights reserved.
 */

package metreos.core.net.ipc;

/**
 * A listener for IPC connection events.
 * @param <T> The type of ipc connection
 */
public interface IpcListener<T>
{
	/**
	 * Notifies that the connection has been started.
	 * 
	 * @param connection the connection that has been started.
	 * 
	 * @throws Exception if the connection startup fails or is not allowed.
	 */
	public void startup( T connection ) throws Exception;

	/**
	 * Notifies that a message has been received on the connection.
	 * 
	 * @param connection the connection that received the message.
	 * 
	 * @param message the message that has been received
	 */
	public void received( T connection, IpcMessage message );

	/**
	 * Notifies that the connection has been shutdown.
	 * 
	 * @param connection the connection that has been shutdown.
	 */
	public void shutdown( T connection );

	/**
	 * Notifies that an exception was caught by the connection.
	 * 
	 * @param connection the connection that has caught an exception.
	 * 
	 * @param e the exception that was caught.
	 */
	public void exception( T connection, Exception e );
}
