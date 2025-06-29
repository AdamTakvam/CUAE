/* $Id: DefaultServerListener.java 4937 2005-01-27 18:43:56Z wert $
 *
 * Created by wert on Jan 26, 2005
 *
 * Copyright (c) 2005 Metreos, Inc. All rights reserved.
 */

package metreos.core.net;

/**
 * An implementation of ServerListener which does nothing
 * for each method thereof. Can be used as a convenient way
 * to implement just a few methods of ServerListener.
 * 
 * @see metreos.core.net.ServerListener
 */
public class DefaultServerListener implements ServerListener
{
	/*
	 * @see metreos.core.net.ServerListener#started(metreos.core.net.Server, java.net.ServerSocket)
	 */
	public void started( Server server )
	{
		// subclass responsibility
	}

	/*
	 * @see metreos.core.net.ServerListener#stopped(metreos.core.net.Server)
	 */
	public void stopped( Server server )
	{
		// subclass responsibility
		server.removeListener( this );
	}

	/*
	 * @see metreos.core.net.ServerListener#exception(metreos.core.net.Server, java.lang.String, java.lang.Exception)
	 */
	public void exception( Server server, String what, Exception e )
	{
		// subclass responsibility
	}

	/*
	 * @see metreos.core.net.ServerListener#shutdown(metreos.core.net.Server, java.lang.String)
	 */
	public void shutdown( Server server, String reason )
	{
		// subclass responsibility
	}
}
