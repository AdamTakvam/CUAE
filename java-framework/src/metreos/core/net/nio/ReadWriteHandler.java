/* $Id: ReadWriteHandler.java 30151 2007-03-06 21:47:46Z wert $
 *
 * Created by wert on Mar 16, 2005.
 *
 * Copyright (c) 2005 Metreos, Inc. All rights reserved.
 */

package metreos.core.net.nio;

import java.io.IOException;
import java.net.SocketAddress;
import java.nio.channels.SelectableChannel;
import java.nio.channels.SelectionKey;

/**
 * ReadWriteHandler is an abstract handler for read / write channels.
 * 
 * @see StreamHandler
 * @see DatagramHandler
 */
abstract public class ReadWriteHandler extends SelectableHandler
{
	/**
	 * Constructs the ReadWriteHandler.
	 *
	 * @param superSelector
	 * @param channel
	 */
	public ReadWriteHandler( SuperSelector superSelector, SelectableChannel channel )
	{
		super( superSelector, channel );
	}

	//////////////////////////
	// LOCAL SOCKET ADDRESS //
	//////////////////////////

	/**
	 * @return the local socket address if the socket is bound.
	 */
	abstract protected SocketAddress fetchLocalSocketAddress();

	/**
	 * @return the local socket address of this handler.
	 */
	public final SocketAddress getLocalSocketAddress()
	{
		return localSocketAddress;
	}
	
	/**
	 * @param address the local socket address of this handler.
	 */
	protected final void setLocalSocketAddress( SocketAddress address )
	{
		localSocketAddress = address;
		setupCachedToString();
	}
	
	private SocketAddress localSocketAddress;

	///////////////////////////
	// REMOTE SOCKET ADDRESS //
	///////////////////////////
	
	/**
	 * @return the remote socket address if the socket is connected.
	 */
	abstract protected SocketAddress fetchRemoteSocketAddress();

	/**
	 * @return the remote socket address of this handler.
	 */
	public final SocketAddress getRemoteSocketAddress()
	{
		return remoteSocketAddress;
	}
	
	/**
	 * @param address the remote socket address of this handler.
	 */
	protected final void setRemoteSocketAddress( SocketAddress address )
	{
		remoteSocketAddress = address;
		setupCachedToString();
	}
	
	private SocketAddress remoteSocketAddress;

	@Override
	public int getInterestOps()
	{
		if (wantsRead())
		{
			if (wantsWrite())
				return SelectionKey.OP_READ|SelectionKey.OP_WRITE;
			
			return SelectionKey.OP_READ;
		}
		else if (wantsWrite())
		{
			return SelectionKey.OP_WRITE;
		}
		
		return 0;
	}
	
	public void handleSelection() throws IOException
	{
		if (isReadable())
			doRead();
		
		if (isWritable())
			doWrite();
		
		resetInterestOps();
	}

	/**
	 * Determines if the handler wants more input right now or not. If
	 * so, read events are enabled in the selector, otherwise they are
	 * disabled. Reasons to disable input events are: a complete request
	 * has been received and is being processed; input buffers are full
	 * and we are waiting for them to be cleared; the server is too busy
	 * right now to process another request.
	 * 
	 * The default implementation always returns true.
	 * 
	 * @return true if more input can be processed, false if the input
	 * buffers are exhausted or if no input is needed right now or the
	 * server is busy.
	 */
	protected boolean wantsRead()
	{
		return true;
	}

	/**
	 * Determines if the handler has more output right now or not. If so,
	 * write events are enabled in the selector, otherwise they are
	 * disabled. Reasons to disable output events are: no output available
	 * for writing.
	 * 
	 * The default implementation always returns false.
	 * 
	 * @return true if there is output that can be written, false if
	 * there is no more output right now.
	 */
	protected boolean wantsWrite()
	{
		return false;
	}
	
	/**
	 * Performs a read operation on a channel that has been selected
	 * for read.
	 * 
	 * @throws IOException if there is a problem reading. The channel
	 * will be deregistered and closed.
	 */
	abstract protected void doRead()
		throws IOException;
	
	/**
	 * Performs a write operation on a channel that has been selected
	 * for write.
	 * 
	 * @throws IOException if there is a problem writing. The channel
	 * will be deregistered and closed.
	 */
	abstract protected void doWrite()
		throws IOException;
}
