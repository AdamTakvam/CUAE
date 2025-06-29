/* $Id: ServerListener.java 4937 2005-01-27 18:43:56Z wert $
 *
 * Created by wert on Jan 26, 2005
 *
 * Copyright (c) 2005 Metreos, Inc. All rights reserved.
 */

package metreos.core.net;

/**
 * A listener for server events.
 */
public interface ServerListener
{
	/**
	 * Notifies the listener that the server has started.
	 * 
	 * @param server the server which is reporting the event.
	 */
	public void started( Server server );

	/**
	 * Notifies the listener that the server has stopped.
	 * Non-connection listeners that receive stopped notification
	 * should remove their listener.
	 * 
	 * @param server the server which is reporting the event.
	 * 
	 * @see Server#removeListener(ServerListener)
	 */
	public void stopped( Server server );

	/**
	 * Notifies the listener that the server has caught an exception.
	 * 
	 * @param server the server which is reporting the event.
	 * 
	 * @param what the activity that caused the exception.
	 * 
	 * @param e the exception that was caught.
	 */
	public void exception( Server server, String what, Exception e );

	/**
	 * Notifies the listener that the server has been shutdown.
	 * Connection handlers that receive shutdown notification
	 * should wind up activities, remove their listener, and
	 * shutdown.
	 * 
	 * @param server the server which is reporting the event.
	 * 
	 * @param reason the reason for the shutdown.
	 * 
	 * @see Server#removeListener(ServerListener)
	 */
	public void shutdown( Server server, String reason );
}
