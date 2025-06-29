/* $Id: IpcConnectionHandler.java 12925 2005-11-02 16:44:28Z wert $
 *
 * Created by wert on Jan 26, 2005
 *
 * Copyright (c) 2005 Metreos, Inc. All rights reserved.
 */

package metreos.core.net.ipc;

import java.io.IOException;
import java.net.Socket;

import metreos.core.net.ConnectionHandler;
import metreos.core.net.Server;

/**
 * A ConnectionHandler to handle incoming IPC connections.
 * @param <T> 
 */
abstract public class IpcConnectionHandler<T extends IpcConnection> implements ConnectionHandler
{
	/* (non-Javadoc)
	 * @see metreos.core.net.ConnectionHandler#getDescription()
	 */
	public String getDescription()
	{
		return "Metreos IPC";
	}

	/* (non-Javadoc)
	 * @see metreos.core.net.ConnectionHandler#newConnection(metreos.core.net.Server, java.net.Socket)
	 */
	public T newConnection( Server server, Socket socket )
		throws IOException
	{
		T connection = newIpcConnection( server, socket );
		addListeners( connection );
		connection.start();
		return connection;
	}
	
	/**
	 * @param server
	 * @param socket
	 * @return newly constructed IpcConnection to service the socket.
	 * @throws IOException 
	 */
	abstract protected T newIpcConnection( Server server, Socket socket )
		throws IOException;

	/**
	 * Adds the listeners to a newly created but not yet started connection.
	 * Subclasser must implement this method. The listeners receive notification
	 * of connection events, including messages received.
	 * 
	 * @param connection the newly created connection.
	 * @throws IOException if the subclass doesn't like the connection
	 * for some reason.
	 */
	abstract protected void addListeners( T connection )
		throws IOException;
}
