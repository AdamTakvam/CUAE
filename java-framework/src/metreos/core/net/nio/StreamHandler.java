/* $Id: StreamHandler.java 30151 2007-03-06 21:47:46Z wert $
 *
 * Created by wert on Mar 14, 2005.
 *
 * Copyright (c) 2005 Metreos, Inc. All rights reserved.
 */

package metreos.core.net.nio;

import java.io.EOFException;
import java.io.IOException;
import java.net.Socket;
import java.net.SocketAddress;
import java.nio.ByteBuffer;
import java.nio.channels.SelectionKey;
import java.nio.channels.SocketChannel;

/**
 * A connect / read / write handler for a SocketChannel.
 */
abstract public class StreamHandler extends ReadWriteHandler
{
	/**
	 * Constructs the StreamHandler.
	 * @param superSelector the super selector.
	 * @param channel the channel being managed.
	 */
	public StreamHandler( SuperSelector superSelector, SocketChannel channel )
	{
		super( superSelector, channel );
		this.channel = channel;
	}
	
	/**
	 * Our SocketChannel.
	 */
	private final SocketChannel channel;
	
	/**
	 * @return our socket channel.
	 */
	public final SocketChannel getSocketChannel()
	{
		return channel;
	}

	@Override
	protected final SocketAddress fetchLocalSocketAddress()
	{
		try
		{
			Socket s = channel.socket();
			if (!s.isBound())
				return null;
			return s.getLocalSocketAddress();
		}
		catch ( RuntimeException e )
		{
			return null;
		}
	}

	@Override
	protected final SocketAddress fetchRemoteSocketAddress()
	{
		try
		{
			Socket s = channel.socket();
			if (!s.isConnected())
				return null;
			return s.getRemoteSocketAddress();
		}
		catch ( RuntimeException e )
		{
			return null;
		}
	}

	@Override
	protected void setupCachedToString()
	{
		setCachedToString(
			getClass().getName()+
			"( "+
			getLocalSocketAddress()+
			" -> "+
			getRemoteSocketAddress()+
			" )" );
	}

	@Override
	public final int getInterestOps()
	{
		// OP_CONNECT is mutually exclusive with OP_READ and OP_WRITE.
		if (wantsConnect())
			return SelectionKey.OP_CONNECT;
		
		return super.getInterestOps();
	}

	@Override
	public final int selectedInterestOps()
	{
		// block further selection until we return.
		return 0;
	}

	@Override
	public final void handleSelection() throws IOException
	{
		if (isConnectable())
		{
			doConnect();
			resetInterestOps();
			return;
		}
		
		super.handleSelection();
	}
	
	private boolean wantsConnect()
	{
		return channel.isConnectionPending();
	}
	
	private void doConnect() throws IOException
	{
		if (channel.finishConnect())
			doConnected();
	}
	
	/**
	 * Reports that we are connected. We may not be
	 * registered with the super selector yet, and
	 * thus may not have a selection key.
	 */
	protected void doConnected()
	{
		// subclass may want to do something.
	}

	/**
	 * @param buffer
	 * @param throwEof
	 * @return the number of bytes read.
	 * @throws IOException
	 */
	protected final int read( ByteBuffer buffer, boolean throwEof )
		throws IOException
	{
		try
		{
//			Trace.report( this, Trace.m( "reading " ).a( buffer.remaining() ) );
			int n = channel.read( buffer );
//			Trace.report( this, Trace.m( "read " ).a( n ) );
			
			if (n >= 0)
				return n;

			if (throwEof)
				throw new EOFException( "n < 0" );
			
			return -1;
		}
		catch ( IOException e )
		{
			String s = e.getMessage();
			
			if (s != null)
			{
				s = s.toLowerCase();
				
				if (s.indexOf( "reset" ) >= 0)
				{
					if (throwEof)
						throw new EOFException( s );
					return -1;
				}
				
				if (s.indexOf( "closed" ) >= 0)
				{
					if (throwEof)
						throw new EOFException( s );
					return -1;
				}
			}
			
			throw e;
		}
	}
	
	/**
	 * @param buffer
	 * @return the number of bytes written (possibly 0).
	 * @throws IOException
	 */
	public int write( ByteBuffer buffer ) throws IOException
	{
		try
		{
			return channel.write( buffer );
		}
		catch ( IOException e )
		{
			String s = e.getMessage();
			if (s != null)
			{
				s = s.toLowerCase();
				if (s.indexOf( "buffer space" ) >= 0)
					return 0;
			}
			throw e;
		}
	}
}
