/* $Id: AcceptHandler.java 30151 2007-03-06 21:47:46Z wert $
 *
 * Created by wert on Mar 14, 2005.
 *
 * Copyright (c) 2005 Metreos, Inc. All rights reserved.
 */

package metreos.core.net.nio;

import java.io.IOException;
import java.net.ServerSocket;
import java.net.SocketAddress;
import java.nio.channels.SelectionKey;
import java.nio.channels.ServerSocketChannel;
import java.nio.channels.SocketChannel;

import metreos.util.Trace;


/**
 * The AcceptHandler is a handler for accept events on a server
 * socket channel. It accepts connections, uses the factory to
 * get a new stream handler, and registers the handler and channel
 * with the superSelector.
 */
public class AcceptHandler extends SelectableHandler
{
	/**
	 * Constructs the AcceptHandler.
	 *
	 * @param superSelector
	 * @param serverChannel
	 * @param factory
	 */
	public AcceptHandler( SuperSelector superSelector,
		ServerSocketChannel serverChannel, StreamHandlerFactory factory )
	{
		super( superSelector, serverChannel );
		this.serverChannel = serverChannel;
		this.factory = factory;
		setupCachedToString();
	}
	
	private final ServerSocketChannel serverChannel;
	
	private final StreamHandlerFactory factory;

	@Override
	protected void setupCachedToString()
	{
		setCachedToString( "AcceptHandler( "+getListeningSocketAddress()+" )" );
	}
	
	/**
	 * @return the socket address that we are listening on.
	 */
	public SocketAddress getListeningSocketAddress()
	{
		try
		{
			ServerSocket s = serverChannel.socket();
			return s.isBound() ? s.getLocalSocketAddress() : null;
		}
		catch ( RuntimeException e )
		{
			return null;
		}
	}

	@Override
	public final int getInterestOps()
	{
		if (wantsAccept())
			return SelectionKey.OP_ACCEPT;
		
		return 0;
	}
	
	/**
	 * @return true if more accepts are ok, false otherwise.
	 */
	protected boolean wantsAccept()
	{
		return true;
	}

	@Override
	public int selectedInterestOps()
	{
		// allow multiple threads to concurrently call handleSelection
		// subclasser can return 0 to block futher accepts until the
		// first handleSelection returns (or updates the interest ops).
		return getInterestOps();
	}

	public final void handleSelection() throws IOException
	{
		if (isAcceptable())
			doAccept();
		
		resetInterestOps();
	}

	private void doAccept() throws IOException
	{
		SocketChannel channel;
		if ((channel = serverChannel.accept()) != null)
		{
//			Trace.report( this, Trace.m( "accepted " ).a( channel ) );
			try
			{
				channel.configureBlocking( false );
				
				StreamHandler handler = factory.newStreamHandler(
					getSuperSelector(), channel );
				
				handler.register();
			}
			catch ( IOException e )
			{
				report( e );
				close( channel );
			}
			catch ( RuntimeException e )
			{
				report( e );
				close( channel );
			}
		}
	}
	
	private void close( SocketChannel channel )
	{
		try
		{
//			Trace.report( this, "channel.close: "+channel );
			channel.close();
		}
		catch ( IOException e )
		{
			Trace.report( channel, e );
		}
	}
}