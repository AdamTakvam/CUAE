/* $Id: StreamHandlerFactory.java 6358 2005-04-21 22:59:36Z wert $
 *
 * Created by wert on Mar 14, 2005.
 * 
 * Copyright (c) 2005 Metreos, Inc. All rights reserved.
 */

package metreos.core.net.nio;

import java.net.Socket;
import java.net.SocketAddress;
import java.nio.channels.ServerSocketChannel;
import java.nio.channels.SocketChannel;

/**
 * StreamHandlerFactory is an interface for constructing a new StreamHandler.
 * An StreamHandler is needed when a new SocketChannel is constructed
 * and registered with a Selector.
 * 
 * @see SuperSelector#newStreamHandler(SocketAddress, boolean, StreamHandlerFactory)
 * @see AcceptHandler#AcceptHandler(SuperSelector, ServerSocketChannel, StreamHandlerFactory)
 */
public interface StreamHandlerFactory
{
	/**
	 * Constructs a stream handler. This is also a good place for socket
	 * options to be set.
	 * 
	 * @param superSelector
	 * @param channel
	 * @return a stream handler
	 * 
	 * @see Socket#setKeepAlive(boolean)
	 * @see Socket#setPerformancePreferences(int, int, int)
	 * @see Socket#setReceiveBufferSize(int)
	 * @see Socket#setReuseAddress(boolean)
	 * @see Socket#setSendBufferSize(int)
	 * @see Socket#setSoLinger(boolean, int)
	 * @see Socket#setTcpNoDelay(boolean)
	 * @see Socket#setTrafficClass(int)
	 */
	public StreamHandler newStreamHandler( SuperSelector superSelector,
		SocketChannel channel );
}
