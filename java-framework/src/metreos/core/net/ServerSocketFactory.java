/* $Id: ServerSocketFactory.java 4937 2005-01-27 18:43:56Z wert $
 *
 * Created by wert on Jan 26, 2005
 *
 * Copyright (c) 2005 Metreos, Inc. All rights reserved.
 */

package metreos.core.net;

import java.io.IOException;
import java.net.InetAddress;
import java.net.ServerSocket;

/**
 * A factory for server sockets.
 */
public interface ServerSocketFactory
{
	/**
	 * Constructs a server socket listening on the specified interface
	 * and port.
	 * 
	 * @param intf the address of the interface to listen on, or null for all.
	 * 
	 * @param port the port to listen on, or 0 to request a port be assigned.
	 * 
	 * @return the server socket.
	 * 
	 * @throws IOException if there is a problem making the socket.
	 */
	public ServerSocket newServerSocket( InetAddress intf, int port )
		throws IOException;
}
