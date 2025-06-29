/* $Id: IpcEchoServer.java 30151 2007-03-06 21:47:46Z wert $
*
* Created by wert on Mar 14, 2005
*
* Copyright (c) 2005 Metreos, Inc. All rights reserved.
*/

package metreos.core.net.nio.test;

import java.io.IOException;
import java.net.InetSocketAddress;
import java.net.SocketAddress;
import java.nio.ByteBuffer;
import java.nio.channels.DatagramChannel;
import java.nio.channels.ServerSocketChannel;
import java.nio.channels.SocketChannel;
import java.util.List;
import java.util.Vector;

import metreos.core.ipc.flatmaps.FlatmapIpcMessage;
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
public class IpcEchoServer implements AcceptHandlerFactory,
	StreamHandlerFactory, DatagramHandlerFactory
{
	/**
	 * @param args
	 * @throws Exception
	 */
	public static void main( String[] args ) throws Exception
	{
		new IpcEchoServer().start();
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
		SocketAddress address = new InetSocketAddress( 9124 );

		// make our super selector
		
		SuperSelector superSelector = new SuperSelector0( 20, 0 );
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
			
			Trace.report( this, Trace.m( "DatagramHandler bound to " )
					.a( handler.getLocalSocketAddress() ) );
		}
	}

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
		return new MyIpcStreamHandler( superSelector, channel, 1 );
	}

	/* (non-Javadoc)
	 * @see DatagramHandlerFactory#newDatagramHandler(SuperSelector, DatagramChannel)
	 */
	public DatagramHandler newDatagramHandler( SuperSelector superSelector,
		DatagramChannel channel )
	{
		return new MyDatagramHandler( superSelector, channel );
	}
	
	private static class MyIpcStreamHandler extends FlatmapIpcStreamHandler
	{
		/**
		 * Constructs the MyReadWriteHandler.
		 * @param superSelector
		 * @param channel
		 * @param maxOutputQueueSize 
		 */
		public MyIpcStreamHandler( SuperSelector superSelector,
			SocketChannel channel, int maxOutputQueueSize )
		{
			super( superSelector, channel, maxOutputQueueSize );
		}

		@Override
		protected void handleMessage( FlatmapIpcMessage msg ) throws IOException
		{
			msg.args.add( 2, respSeqNo++ );
			send( new FlatmapIpcMessage( msg.messageType+1, msg.args, null, null ) );
		}

		private int respSeqNo = 1;
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

		@Override
		protected void doRead() throws IOException
		{
			ByteBuffer buffer = null;
			while (queue.size() < MAX_QUEUE_SIZE)
			{
				if (buffer == null)
					buffer = ByteBuffer.allocate( 1024 );
					
				SocketAddress from = receive( buffer );
				if (from == null)
					break;
				
				buffer.flip();
				if (!write( buffer, from ))
					buffer = null;
				else
					buffer.clear();
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
