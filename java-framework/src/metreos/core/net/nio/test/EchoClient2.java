package metreos.core.net.nio.test;

import java.io.EOFException;
import java.io.IOException;
import java.net.InetSocketAddress;
import java.net.SocketAddress;
import java.nio.ByteBuffer;
import java.nio.ByteOrder;
import java.nio.channels.SocketChannel;

import metreos.core.net.nio.StreamHandler;
import metreos.core.net.nio.StreamHandlerFactory;
import metreos.core.net.nio.SuperSelector;
import metreos.core.net.nio.SuperSelector0;
import metreos.util.Monitor;
import metreos.util.Trace;

/**
 * @author wert
 */
public class EchoClient2
{
	/**
	 * @param args
	 * @throws Exception 
	 */
	public static void main( String[] args )
		throws Exception
	{
		//SocketAddress address = new InetSocketAddress( "10.1.12.165", 9123 ); // marge
		//SocketAddress address = new InetSocketAddress( "10.1.12.163", 9123 ); // lenny
//		SocketAddress address = new InetSocketAddress( "10.1.12.175", 9123 ); // fred
		SocketAddress address = new InetSocketAddress( "127.0.0.1", 9123 ); // localhost
		int nWorkers = 20;
		int maxSelectorSize = 32;
		
		boolean first = true;
		int i = 1;
		while (i <= 256)
		{
			if (!first)
			{
				int n = 15000;
				Trace.report( "sleeping "+n );
				Thread.sleep( n );
			}
			else
				first = false;
			
//			int k = 10000 * 240 / i / 2; // 10000 = packets/second, 240 = seconds/run
			int k = 10000;
			
			new EchoClient2().start( address, nWorkers, maxSelectorSize, i, PACKET_SIZE, k );
			
			if (i < 8192)
				i *= 2;
			else
				i += 1024;
		}
	}
	
	/**
	 * PACKET_SIZE chosen to simulate 300 byte packets. This is how much
	 * we can write to the server before we wait for an answer.
	 */
	public final static int PACKET_SIZE = 75; // in words

	/**
	 * Constructs the EchoClient.
	 */
	public EchoClient2()
	{
		// nothing to do.
	}
	
	private void start( SocketAddress address, int nWorkers, int maxSelectorSize,
		int nStreams, int packetSize, int packetCount ) throws Exception
	{
		Trace.report( Trace.m( "starting" )
			.a( " address=" ).a( address )
			.a( " nStreams=" ).a( nStreams )
			.a( " packetSize=" ).a( packetSize )
			.a( " packetCount=" ).a( packetCount ) );

		// make our super selector
		
		SuperSelector superSelector = new SuperSelector0( nWorkers, 0 );
		superSelector.start();
		
		long tStart = System.currentTimeMillis();
		
		EchoStreamHandler[] shs = new EchoStreamHandler[nStreams];
		for (int i = 0; i < nStreams; i++)
		{
//			Trace.report( Trace.m( "starting " ).a( i ) );
			shs[i] = tryStart( superSelector, address, packetSize, packetCount, i );
			if (shs[i] == null)
				Trace.report( this, Trace.m( "not started: " ).a( i ) );
		}
		
		long tAllStarted = System.currentTimeMillis();
		double startTime = (tAllStarted-tStart) / 1000.0;
		int startRate = (int)(nStreams / startTime);
		Trace.report( Trace.m( "all started in " ).a( startTime )
			.a( ", rate = " ).a( startRate ) );
		
		goAhead = true;
		for (int i = 0; i < nStreams; i++)
		{
			if (shs[i] != null)
				shs[i].resetInterestOps();
		}
		
		for (int i = 0; i < nStreams; i++)
		{
			if (shs[i] != null)
				shs[i].waitdone();
		}
		
		long tAllDone = System.currentTimeMillis();
		
		int inputCount = 0;
		int outputCount = 0;
		for (int i = 0; i < nStreams; i++)
		{
			if (shs[i] != null)
			{
				inputCount += shs[i].nextInput;
				outputCount += shs[i].nextOutput;
			}
		}
		
		int count = inputCount + outputCount; // in words (4 bytes each)
		int packets = count / packetSize;
		
		double time = (tAllDone-tAllStarted) / 1000.0;
		int rate = (int)(packets / time);
		int bytes = count*4;
		Trace.report( Trace.m( "done" )
			.a( "\t").a( nStreams )
			.a( "\t").a( startTime )
			.a( "\t").a( startRate )
			.a( "\t").a( time )
			.a( "\t").a( rate )
			.a( "\t").a( bytes ) );
		
		// let things settle down.
		Thread.sleep( 15000 );
		
		superSelector.stop( 15000 );
	}
	
//	private long t0;
	
//	private int n0;
	
	private EchoStreamHandler tryStart( SuperSelector wrapper,
		SocketAddress address, int packetSize, int packetCount, int i )
	{
		if (i == 0)
		{
//			t0 = System.currentTimeMillis();
//			n0 = i;
		}
		
		if (i%100 == 1)
		{
//			long t1 = System.currentTimeMillis();
//			int n1 = i;
//			Trace.report( Trace.m( "trying " ).a( i ).a( ", last took " ).a( (t1-t0)/(n1-n0) ) );
//			t0 = t1;
//			n0 = n1;
		}
		
		for (int j = 0; j < 4; j++)
		{
			final int id = i;
			final int limit = packetSize * packetCount;
			try
			{
				if (j > 0)
					Trace.report( Trace.m( "trying again on " ).a( id ) );
				
				clearStarted();
				EchoStreamHandler handler = (EchoStreamHandler) wrapper.newStreamHandler( address,
					false, new StreamHandlerFactory()
					{
						public StreamHandler newStreamHandler( SuperSelector wrapper1,
							SocketChannel channel )
						{
							return new EchoStreamHandler( wrapper1, channel, id, limit );
						}
					});
				waitStarted( 1000*(j+1) );
				return handler;
			}
			catch ( InterruptedException e )
			{
				// ignore.
			}
			catch ( IOException e )
			{
				// ignore.
			}
		}
		return null;
	}

	private void clearStarted()
	{
		startedMonitor.set( null );
	}
	
	private void waitStarted( int timeout ) throws InterruptedException
	{
		startedMonitor.waitUntilNotEq( null, timeout );
	}
	
	/**
	 * 
	 */
	void notifyStarted()
	{
		startedMonitor.set( "started" );
	}
	
	private Monitor startedMonitor = new Monitor( "started" );
	
	/**
	 * Description of allStarted.
	 */
	boolean goAhead;
	
	private class EchoStreamHandler extends StreamHandler
	{
		/**
		 * Constructs the EchoStreamHandler.
		 *
		 * @param wrapper
		 * @param channel
		 * @param id
		 * @param limit
		 */
		public EchoStreamHandler( SuperSelector wrapper, SocketChannel channel,
			int id, int limit )
		{
			super( wrapper, channel );
			this.id = "EchoStreamHandler "+id;
			this.limit = limit;
			//report( Trace.m( "startedCount" ) );
		}

		@SuppressWarnings("unused")
		private final String id;
		
		private final int limit;
		
		private boolean starting = true;

		private boolean doNotifyStarted = true;

		@Override
		protected void doRead() throws IOException
		{
			// here we are reading responses from the server.
			while (input.remaining() > 0)
			{
				int n = read( input, true );
				if (n == 0)
					break;
				
				// we got some input, check it.
				check( input );
				// there might be up to 3 bytes left in the buffer.
			}
			
			// did we get the first packet?
			if (doNotifyStarted && nextInput == nextOutput)
			{
				doNotifyStarted = false;
				notifyStarted();
				return;
			}
			
			// there might be up to 3 bytes left in the buffer.
			if (nextInput >= nextOutput && nextOutput >= limit)
			{
				if (nextInput > nextOutput)
					throw new IOException( "nextInput > nextOutput: "+nextInput );
				
				if (input.position() > 0)
					throw new IOException( "input.position() > 0: "+input.position() );
				
				throw new EOFException();
			}
		}

		/**
		 * @param in
		 * @throws IOException
		 */
		private void check( ByteBuffer in ) throws IOException
		{
			in.flip();
			while (in.remaining() >= 4)
			{
				int b = in.getInt();
//				report( Trace.m( "b: " ).a( b ) );
				if (b != nextInput)
					throw new IOException( "expected "+nextInput+" but got "+b );
				nextInput++;
			}
			// there might be up to 3 bytes left in the buffer.
			in.compact();
		}

		@Override
		protected boolean wantsWrite()
		{
			if (starting)
				return true;
			
			if (!goAhead)
				return false;
			
			if (output.hasRemaining())
				return true;
			
			if (nextOutput - nextInput == 0 && nextOutput < limit)
				return true;
			
			return false;
		}
		
		@Override
		protected void doWrite() throws IOException
		{
			if (starting)
			{
				starting = false;
				send();
				return;
			}
			
			if (output.hasRemaining())
				write( output );
			
			if (!output.hasRemaining())
				send();
		}

		/**
		 * @throws IOException
		 * 
		 */
		private void send() throws IOException
		{
			output.clear();
			
			for (int i = 0; i < PACKET_SIZE && nextOutput < limit; i++)
				output.putInt( nextOutput++ );
			
			output.flip();
			write( output );
		}

		@Override
		public synchronized void doClose( Object reason )
		{
			if (reason != EOF)
				Trace.report( this, Trace.m( "doClose" )
					.a( " " ).a( nextInput )
					.a( " " ).a( nextOutput )
					.a( " " ).a( reason ) );
			
			super.doClose( reason );
			
			done = true;
			notify();
		}
		
		private boolean done;
		
		/**
		 * @throws InterruptedException
		 */
		public synchronized void waitdone() throws InterruptedException
		{
			while (!done)
				wait();
		}

		private ByteBuffer input = ByteBuffer.allocate( 1024 );
		{
			input.order( ByteOrder.BIG_ENDIAN );
		}
		
		private ByteBuffer output = ByteBuffer.allocate( 1024 );
		{
			output.order( ByteOrder.BIG_ENDIAN );
			output.flip();
		}
		
		/**
		 * Description of nextInput.
		 */
		int nextInput = 0;
		
		/**
		 * Description of nextOutput.
		 */
		int nextOutput = 0;
	}
}
