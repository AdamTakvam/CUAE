/* $Id: AcceptHandlerFactory.java 6339 2005-04-19 21:04:51Z wert $
 *
 * Created by wert on Mar 14, 2005.
 * 
 * Copyright (c) 2005 Metreos, Inc. All rights reserved.
 */

package metreos.core.net.nio;

import java.net.SocketAddress;
import java.nio.channels.ServerSocketChannel;

/**
 * AcceptHandlerFactory is an interface for constructing a new AcceptHandler.
 * An AcceptHandler is needed when a new ServerSocketChannel is constructed
 * and registered with a Selector.
 * 
 * @see SuperSelector#newAcceptHandler(SocketAddress, int, AcceptHandlerFactory)
 */
public interface AcceptHandlerFactory
{
	/**
	 * @param superSelector
	 * @param channel
	 * @return an AcceptHandler for the channel
	 */
	public AcceptHandler newAcceptHandler( SuperSelector superSelector,
		ServerSocketChannel channel );
}
