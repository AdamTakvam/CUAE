/* $Id: ConnectionHandler.java 8458 2005-08-10 17:02:44Z wert $
 *
 * Created by wert on Jan 26, 2005
 *
 * Copyright (c) 2005 Metreos, Inc. All rights reserved.
 */

package metreos.core.net;

import java.io.IOException;
import java.net.Socket;

/**
 * A handler for connections (sockets accepted by a server).
 */
public interface ConnectionHandler
{
	/**
	 * Returns the description of this type of connection
	 * (e.g., "HTTP", "FTP", etc.)
	 * 
	 * @return the description of this type of connection.
	 */
	public String getDescription();

	/**
	 * Reports a socket has just been accepted. Implementor
	 * of this method might typically start a thread to
	 * read and write data on the socket.
	 * 
	 * @param server the server which is reporting the event.
	 * 
	 * @param socket the socket which has just been accepted.
	 * 
	 * @return a server listener for the connection
	 * 
	 * @throws IOException if the connection handler does not
	 * like the new socket for whatever reason.
	 */
	public ServerListener newConnection( Server server, Socket socket )
		throws IOException;
}
