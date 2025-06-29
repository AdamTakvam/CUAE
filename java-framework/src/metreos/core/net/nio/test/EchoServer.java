/* $Id: EchoServer.java 30151 2007-03-06 21:47:46Z wert $
*
* Created by wert on Mar 14, 2005
*
* Copyright (c) 2005 Metreos, Inc. All rights reserved.
*/

package metreos.core.net.nio.test;

import java.io.EOFException;
import java.io.IOException;
import java.net.InetSocketAddress;
import java.net.SocketAddress;
import java.nio.ByteBuffer;
import java.nio.channels.DatagramChannel;
import java.nio.channels.ServerSocketChannel;
import java.nio.channels.SocketChannel;
import java.util.List;
import java.util.Vector;

import metreos.core.net.nio.AcceptHandler;
import metreos.core.net.nio.AcceptHandlerFactory;
import metreos.core.net.nio.DatagramHandler;
import metreos.core.net.nio.DatagramHandlerFactory;
import metreos.core.net.nio.PlainSocketAddressGen;
import metreos.core.net.nio.StreamHandler;
import metreos.core.net.nio.StreamHandlerFactory;
import metreos.core.net.nio.SuperSelector;
import metreos.core.net.nio.SuperSelector0;
import metreos.util.Trace;

/**
 * A simple example of using the nio server framework.
 */
public class EchoServer implements AcceptHandlerFactory,
	StreamHandlerFactory, DatagramHandlerFactory
{
	/**
	 * @param args
	 * @throws Exception
	 */
	public static void main( String[] args ) throws Exception
	{
		new EchoServer().start();
	}
	
	/**
	 * @throws Exception 
	 * @throws IOException
	 * 
	 */
	private void start() throws Exception
	{
		boolean wantTcp = true;
		boolean wantUdp = false;
		SocketAddress address = new InetSocketAddress( 9123 );

		// make our super selector
		
		SuperSelector superSelector = new SuperSelector0( 16, 0 );
		superSelector.start();
		
		// make our tcp server socket.
		
		if (wantTcp)
		{
			AcceptHandler handler = superSelector.newAcceptHandler( address, 250, this );
			
			Trace.report( this, Trace.m( "AcceptHandler bound to " )
					.a( handler.getListeningSocketAddress() ) );
		}
		
		// make our udp server socket.
		
		if (wantUdp)
		{
//			throw new UnsupportedOperationException( "udp not yet supported" );
			DatagramHandler handler = superSelector.newDatagramHandler(
				new PlainSocketAddressGen( address ), null, this );
			
			handler.setSendBufferSize( UDP_BUF_SIZE );
			handler.setReceiveBufferSize( UDP_BUF_SIZE );
			
			Trace.report( this, Trace.m( "DatagramHandler bound to " )
					.a( handler.getLocalSocketAddress() ) );
		}
	}
	
	private final static int UDP_BUF_SIZE = 1024*1024;

	/* (non-Javadoc)
	 * @see AcceptHandlerFactory#newAcceptHandler(SuperSelector, ServerSocketChannel)
	 */
	public AcceptHandler newAcceptHandler( SuperSelector superSelector,
		ServerSocketChannel channel )
	{
		return new AcceptHandler( superSelector, channel, this );
	}

	/* (non-Javadoc)
	 * @see StreamHandlerFactory#newStreamHandler(SuperSelector, SocketChannel)
	 */
	public StreamHandler newStreamHandler( SuperSelector superSelector,
		SocketChannel channel )
	{
		return new MyStreamHandler( superSelector, channel );
	}

	/* (non-Javadoc)
	 * @see DatagramHandlerFactory#newDatagramHandler(SuperSelector, DatagramChannel)
	 */
	public DatagramHandler newDatagramHandler( SuperSelector superSelector,
		DatagramChannel channel )
	{
		return new MyDatagramHandler( superSelector, channel );
	}
	
	private static class MyStreamHandler extends StreamHandler
	{
		/**
		 * Constructs the MyReadWriteHandler.
		 * @param superSelector
		 * @param channel
		 */
		public MyStreamHandler( SuperSelector superSelector, SocketChannel channel )
		{
			super( superSelector, channel );
		}

		@Override
		protected boolean wantsRead()
		{
			return input.remaining() > 0;
		}

		@Override
		protected boolean wantsWrite()
		{
			return output.remaining() > 0 || input.position() > 0;
		}

		@Override
		protected void doRead() throws IOException
		{
//			report( Trace.m( "+doRead" ) );
			while (input.remaining() > 0)
			{
				int n = read( input, false );
				if (n == 0)
					break;
				
				processInput( n );
			}
//			report( Trace.m( "-doRead" ) );
		}

		@Override
		protected void doWrite() throws IOException
		{
//			report( Trace.m( "+doWrite" ) );
			while (output.hasRemaining() || fillOutput())
			{
				int n = write( output );
				if (n == 0)
					break;
			}
//			report( Trace.m( "-doWrite" ) );
		}

		private void processInput( int n ) throws IOException
		{
			if (output.hasRemaining())
			{
				// output buffer still has stuff in it. no need to
				// worry about starting output.
				return;
			}
			
			// output buffer is empty. see if there is any
			// input that hasn't been processed.
			
			if (fillOutput())
			{
				// there is pending output.
				write( output );
				return;
			}
			
			// input buffer is empty, output buffer is empty.
			
			if (n < 0)
			{
				// the socket has been closed. we're done.
				throw new EOFException();
			}
		}
		
		private boolean fillOutput()
		{
			if (input.position() > 0)
			{
				swapInputAndOutput();
				return true;
			}
			return false;
		}
		
		private synchronized void swapInputAndOutput()
		{
//			report( "swap" );
			
			input.flip();
			
			ByteBuffer tmp = input;
			input = output;
			output = tmp;
			
			input.clear();
		}

		private ByteBuffer input = ByteBuffer.allocate( 1024 );

		private ByteBuffer output = ByteBuffer.allocate( 1024 );
		{
			output.flip();
		}
	}
	
	/**
	 * Description of MyDatagramHandler
	 */
	public class MyDatagramHandler extends DatagramHandler
	{
		/**
		 * @param superSelector
		 * @param channel
		 */
		public MyDatagramHandler( SuperSelector superSelector,
			DatagramChannel channel )
		{
			super( superSelector, channel );
		}

		@Override
		public int selectedInterestOps()
		{
			return getInterestOps();
		}
		
		private final static int MAX_QUEUE_SIZE = 100;

		@Override
		protected boolean wantsRead()
		{
			return queue.size() < MAX_QUEUE_SIZE;
		}

		@Override
		protected boolean wantsWrite()
		{
			return !queue.isEmpty() && !isWriteLocked();
		}
		
		private final static int MAX_PACKET_SIZE = 200;

		@Override
		protected void doRead() throws IOException
		{
			// this can be entered by multiple threads...
			ByteBuffer buffer = ByteBuffer.allocate( MAX_PACKET_SIZE );
			while (queue.size() < MAX_QUEUE_SIZE)
			{
				SocketAddress from = receive( buffer );
				if (from == null)
					break;
				
				buffer.flip();
				if (write( buffer, from ))
					buffer.clear(); // buffer written, can reuse it
				else
					buffer = ByteBuffer.allocate( MAX_PACKET_SIZE ); // buffer queued, get new one
			}
		}

		/**
		 * @param buffer
		 * @param address
		 * @return true if the packet was written, false otherwise.
		 * @throws IOException
		 */
		private boolean write( ByteBuffer buffer, SocketAddress address )
			throws IOException
		{
			if (queue.size() > 0 || !send( buffer, address ))
			{
				queue.add( new Outgoing( buffer, address ) );
				return false;
			}
			return true;
		}

		private List<Outgoing> queue = new Vector<Outgoing>();

		@Override
		protected void doWrite() throws IOException
		{
			// prevent more than one thread coming in here.
			if (!lockWrite())
				return;
			
			try
			{
				while (queue.size() > 0)
				{
					Outgoing packet = queue.get( 0 );
					if (!send( packet.buf, packet.address ))
						break;
					
					queue.remove( 0 );
				}
			}
			finally
			{
				unlockWrite();
			}
		}

		/**
		 * @return true if the write lock was acquired, false if writing
		 * was already locked by some other thread.
		 */
		private synchronized boolean lockWrite()
		{
			if (writeLock)
				return false;
			
			writeLock = true;
			return true;
		}

		/**
		 * 
		 */
		private void unlockWrite()
		{
			writeLock = false;
		}

		/**
		 * @return true if writing is locked.
		 */
		private boolean isWriteLocked()
		{
			return writeLock;
		}
		
		private boolean writeLock;
	}
	
	/**
	 * Description of Outgoing
	 */
	public static class Outgoing
	{
		/**
		 * @param buf
		 * @param address
		 */
		public Outgoing( ByteBuffer buf, SocketAddress address )
		{
			this.buf = buf;
			this.address = address;
		}
		
		/**
		 * Description of buf
		 */
		public final ByteBuffer buf;
		
		/**
		 * Description of address
		 */
		public final SocketAddress address;
	}
}
