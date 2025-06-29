/* $Id: DatagramHandler.java 30151 2007-03-06 21:47:46Z wert $
 *
 * Created by wert on Mar 14, 2005.
 *
 * Copyright (c) 2005 Metreos, Inc. All rights reserved.
 */

package metreos.core.net.nio;

import java.io.IOException;
import java.net.DatagramSocket;
import java.net.SocketAddress;
import java.net.SocketException;
import java.nio.ByteBuffer;
import java.nio.channels.DatagramChannel;

import metreos.util.Assertion;
import metreos.util.SimpleTodo;

/**
 * A connect / read / write handler for a DatagramChannel.
 */
abstract public class DatagramHandler extends ReadWriteHandler
{
	/**
	 * Constructs the DatagramHandler.
	 * @param superSelector the super selector.
	 * @param channel the channel being managed.
	 */
	public DatagramHandler( SuperSelector superSelector, DatagramChannel channel )
	{
		super( superSelector, channel );
		this.channel = channel;
		setLocalSocketAddress( fetchLocalSocketAddress() );
		SocketAddress addr = fetchRemoteSocketAddress();
		setRemoteSocketAddress( addr );
		doConnected( addr != null );
	}
	
	/**
	 * Our DatagramChannel.
	 */
	private final DatagramChannel channel;
	
	/**
	 * @return our datagram channel.
	 */
	public final DatagramChannel getDatagramChannel()
	{
		return channel;
	}

	@Override
	protected final SocketAddress fetchLocalSocketAddress()
	{
		try
		{
			DatagramSocket s = channel.socket();
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
			DatagramSocket s = channel.socket();
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

	/**
	 * @param addr
	 */
	public void connect( final SocketAddress addr )
	{
		getSuperSelector().addToTodoList( new SimpleTodo()
		{
			public void doit()
			{
				try
				{
					doConnect( addr );
				}
				catch ( Exception e )
				{
					fireTodoException( e );
				}
			}
		} );
	}
	
	/**
	 */
	public void disconnect()
	{
		getSuperSelector().addToTodoList( new SimpleTodo()
		{
			public void doit()
			{
				try
				{
					doConnect( null );
				}
				catch ( Exception e )
				{
					fireTodoException( e );
				}
			}
		} );
	}
	
	/**
	 * @param addr
	 * @throws IOException 
	 */
	void doConnect( SocketAddress addr ) throws IOException
	{
		if (isConnected())
		{
			if (addr != null && isConnectedTo( addr ))
				return; // already connected to addr, we're done
			
			channel.disconnect();
			setRemoteSocketAddress( null );
			doConnected( false );
		}
		
		if (addr == null)
			return; // want disconnected, we're done.
		
		//report( "connecting to "+addr );
		channel.connect( addr );
		setRemoteSocketAddress( addr );
		doConnected( true );
	}
	
	/**
	 * @param isConnected
	 */
	protected void doConnected( boolean isConnected )
	{
		// subclass may want to do something
	}

	/**
	 * @return true if the channel is connected
	 */
	protected final boolean isConnected()
	{
		return getRemoteSocketAddress() != null;
	}
	
	/**
	 * @param addr
	 * @return true if the channel is connected to the specified address
	 */
	protected final boolean isConnectedTo( SocketAddress addr )
	{
		return isConnected() && addr.equals( getRemoteSocketAddress() );
	}
	
	/**
	 * @param tos
	 * @throws SocketException
	 */
	public final void setTrafficClass( int tos ) throws SocketException
	{
		if (tos != 0)
			channel.socket().setTrafficClass( tos );
	}
	
	/**
	 * @param bufSize
	 * @throws SocketException 
	 */
	public final void setReceiveBufferSize( int bufSize ) throws SocketException
	{
		channel.socket().setReceiveBufferSize( bufSize );
	}
	
	/**
	 * @param bufSize
	 * @throws SocketException 
	 */
	public final void setSendBufferSize( int bufSize ) throws SocketException
	{
		channel.socket().setSendBufferSize( bufSize );
	}

	@Override
	public int selectedInterestOps()
	{
		// block further selection until we return.
		return 0;
	}

	/**
	 * @param buffer a buffer BIGGER than the largest expected message.
	 * 
	 * @return socket address of the sender if a message was read into
	 * buffer, null otherwise.
	 * 
	 * @throws IOException if there is a problem reading, or if the message
	 * read is exactly the size of the buffer (indicates buffer overflow).
	 */
	protected SocketAddress receive( ByteBuffer buffer )
		throws IOException
	{
		buffer.clear();
		
//		report( Trace.m( "receiving " ).a( buffer.remaining() ).a( " bytes" ) );
		SocketAddress from = channel.receive( buffer );
//		report( Trace.m( "received " ).a( buffer.position() ).a( " bytes from " ).a( from ) );
		
		if (from == null)
			return null;
		
		if (buffer.remaining() == 0)
			throw new IOException( "buffer overflow" );
		
		return from;
	}
	
	/**
	 * @param buffer a buffer BIGGER than the largest expected message.
	 * 
	 * @return true if a message was read into buffer, false otherwise.
	 * 
	 * @throws IOException if there is a problem reading, or if the message
	 * read is exactly the size of the buffer (indicates buffer overflow).
	 */
	protected boolean read( ByteBuffer buffer )
		throws IOException
	{
		buffer.clear();
		
//		report( Trace.m( "reading " ).a( buffer.remaining() ).a( " bytes" ) );
		int n = channel.read( buffer );
//		report( Trace.m( "read " ).a( n ).a( " bytes" ) );
		
		if (buffer.remaining() == 0)
			throw new IOException( "buffer overflow" );
		
		return n != 0;
	}
	
	/**
	 * @param buffer
	 * @param to
	 * @return true if buffer was sent, false otherwise.
	 * @throws IOException
	 */
	public boolean send( ByteBuffer buffer, SocketAddress to )
		throws IOException
	{
		try
		{
			int length = buffer.remaining();
			int n = channel.send( buffer, to );
			if (n == length)
				return true;
			
			Assertion.check( n == 0, "n == 0" );
			return false;
		}
		catch ( IOException e )
		{
			String s = e.getMessage();
			if (s != null)
			{
				s = s.toLowerCase();
				if (s.indexOf( "buffer space" ) >= 0)
					return false;
			}
			throw e;
		}
	}
	
	/**
	 * @param buffer
	 * @return true if buffer was written, false otherwise.
	 * @throws IOException
	 */
	public boolean write( ByteBuffer buffer )
		throws IOException
	{
		try
		{
			int length = buffer.remaining();
			int n = channel.write( buffer );
			if (n == length)
				return true;
			
			Assertion.check( n == 0, "n == 0" );
			return false;
		}
		catch ( IOException e )
		{
			String s = e.getMessage();
			if (s != null)
			{
				s = s.toLowerCase();
				if (s.indexOf( "buffer space" ) >= 0)
					return false;
			}
			throw e;
		}
	}
}
