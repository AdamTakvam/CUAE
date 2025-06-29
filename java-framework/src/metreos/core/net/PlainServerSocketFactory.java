/* $Id: PlainServerSocketFactory.java 4937 2005-01-27 18:43:56Z wert $
 *
 * Created by wert on Jan 27, 2005
 *
 * Copyright (c) 2005 Metreos, Inc. All rights reserved.
 */

package metreos.core.net;

import java.io.IOException;
import java.net.InetAddress;
import java.net.ServerSocket;

/**
 * A server socket factory which makes plain server sockets.
 */
public class PlainServerSocketFactory implements ServerSocketFactory
{
	/* (non-Javadoc)
	 * @see metreos.core.net.ServerSocketFactory#newServerSocket(java.net.InetAddress, int)
	 */
	public ServerSocket newServerSocket( InetAddress intf, int port )
			throws IOException
	{
		return new ServerSocket( port, 250, intf );
	}
}
