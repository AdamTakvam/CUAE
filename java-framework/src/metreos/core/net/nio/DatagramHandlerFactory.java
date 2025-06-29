/* $Id: DatagramHandlerFactory.java 6817 2005-05-20 22:06:30Z wert $
 *
 * Created by wert on Mar 14, 2005.
 * 
 * Copyright (c) 2005 Metreos, Inc. All rights reserved.
 */

package metreos.core.net.nio;

import java.net.SocketAddress;
import java.nio.channels.DatagramChannel;

/**
 * DatagramHandlerFactory is an interface for constructing a new
 * DatagramHandler. A DatagramHandler is needed when a new
 * DatagramChannel is constructed and registered with a Selector.
 * 
 * @see SuperSelector#newDatagramHandler(SocketAddressGen, SocketAddress, DatagramHandlerFactory)
 */
public interface DatagramHandlerFactory
{
	/**
	 * @param superSelector
	 * @param channel
	 * @return a DatagramHandler for the channel.
	 */
	public DatagramHandler newDatagramHandler( SuperSelector superSelector,
		DatagramChannel channel );
}
