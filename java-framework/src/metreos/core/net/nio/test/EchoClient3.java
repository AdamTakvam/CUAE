package metreos.core.net.nio.test;

import java.io.IOException;
import java.net.InetSocketAddress;
import java.net.SocketAddress;
import java.nio.ByteBuffer;
import java.nio.channels.ClosedChannelException;
import java.nio.channels.DatagramChannel;

import metreos.core.net.nio.DatagramHandler;
import metreos.core.net.nio.DatagramHandlerFactory;
import metreos.core.net.nio.PlainSocketAddressGen;
import metreos.core.net.nio.SuperSelector;
import metreos.core.net.nio.SuperSelector0;
import metreos.util.AlarmListener;
import metreos.util.AlarmManager;
import metreos.util.Monitor;
import metreos.util.Trace;

/**
 * @author wert
 */
public class EchoClient3
{
	/**
	 * @param args
	 * @throws Exception 
	 */
	public static void main( String[] args )
		throws Exception
	{
//		SocketAddress to = new InetSocketAddress( "10.1.12.165", 9123 ); // marge
//		SocketAddress to = new InetSocketAddress( "10.1.12.163", 9123 ); // lenny
		SocketAddress to = new InetSocketAddress( "10.1.12.175", 9123 ); // fred
//		SocketAddress to = new InetSocketAddress( "127.0.0.1", 9123 ); // localhost
		
//		SocketAddress from = new InetSocketAddress( "127.0.0.1", 0 ); // localhost
		SocketAddress from = null;
		int nExtraWorkers = 20;
		int maxSelectorSize = 32;
		
		int[] counts = { 64, 96, 128, 192, 256, 384, 512 };
		for (int i = 0; i < counts.length; i++)
		{
			if (i != 0)
				Thread.sleep( 15*1000 );
			
			int count = counts[i];
			
//			int k = 10000 * 240 / count / 2; // 10000 = packets/second, 240 = seconds/run
			int k = 50 * 120; // 120 seconds of audio (at 50 packets / second)
			
			new EchoClient3().start( from, to, nExtraWorkers, maxSelectorSize, count, k );
		}
	}
	
	/**
	 * RTP_PACKET_SIZE chosen to simulate 160 byte audio segment in
	 * an RTP packet.
	 */
	public final static int RTP_PACKET_SIZE = 180; // in bytes

	/**
	 * Constructs the EchoClient.
	 */
	public EchoClient3()
	{
		// nothing to do.
	}
	
	private void start( SocketAddress from, SocketAddress to, int nWorkers,
		int maxSelectorSize, int nClients, int packetCount )
		throws Exception
	{
		Trace.report( Trace.m( "starting" )
			.a( " from=" ).a( from )
			.a( " to=" ).a( to )
			.a( " nClients=" ).a( nClients )
			.a( " packetCount=" ).a( packetCount ) );

		// make our super selector
		
		SuperSelector superSelector = new SuperSelector0( nWorkers, 0 );
		superSelector.start();
		
		am.start( nWorkers );
		
		long tStart = System.currentTimeMillis();
		
		EchoDatagramHandler[] shs = new EchoDatagramHandler[nClients];
		for (int i = 0; i < nClients; i++)
		{
			shs[i] = tryStart( superSelector, from, to, i, packetCount );
			if (shs[i] == null)
				Trace.report( Trace.m( "not started: " ).a( i ) );
		}
		
		long tAllStarted = System.currentTimeMillis();
		Trace.report( Trace.m( "all started in " ).a( tAllStarted-tStart ) );

//		Trace.report( Trace.m( "waiting..." ) );
//		Thread.sleep( 30*60*1000 );
//		Trace.report( Trace.m( "done waiting." ) );
		
		goAhead = true;
		for (int i = 0; i < nClients; i++)
		{
			if (shs[i] != null)
				shs[i].resetInterestOps();
		}
		
		long when = System.currentTimeMillis() + 180*1000;
		for (int i = 0; i < nClients; i++)
		{
			if (shs[i] != null)
				shs[i].waitdone( when );
		}
		
		long tAllDone = System.currentTimeMillis();
		
		int inputCount = 0;
		int outputCount = 0;
		for (int i = 0; i < nClients; i++)
		{
			if (shs[i] != null)
			{
				inputCount += shs[i].recv_count;
				outputCount += shs[i].sent_count;
			}
		}
		
		int count = inputCount + outputCount; // in packets
		
		double time = (tAllDone-tAllStarted) / 1000.0;
		
		Trace.report( Trace.m( "done in " ).a( time )
			.a( " seconds, with " ).a( count )
			.a( " packets transferred (" )
			.a( count / time ).a( " packets / second)" ) );
		
		am.stop();
//		t.join();
		
		// let things settle down.
		Thread.sleep( 15000 );
		
		superSelector.stop( 15000 );
	}
	
	/**
	 * Description of am.
	 */
	AlarmManager am = new AlarmManager();
	
//	private long t0;
	
//	private int n0;
	
	private EchoDatagramHandler tryStart( SuperSelector wrapper,
		SocketAddress from, SocketAddress to, final int id,
		final int packetCount )
	{
		if (id == 0)
		{
//			t0 = System.currentTimeMillis();
//			n0 = i;
		}
		
		if (id%100 == 1)
		{
//			long t1 = System.currentTimeMillis();
//			int n1 = i;
//			Trace.report( Trace.m( "trying " ).a( i ).a( ", last took " ).a( (t1-t0)/(n1-n0) ) );
//			t0 = t1;
//			n0 = n1;
		}
		
		for (int j = 0; j < 4; j++)
		{
			try
			{
				if (j > 0)
					Trace.report( Trace.m( "trying again on " ).a( id ) );
				
				clearStarted();
				EchoDatagramHandler handler = (EchoDatagramHandler)
					wrapper.newDatagramHandler( new PlainSocketAddressGen( from ), to,
						new DatagramHandlerFactory()
						{
							public DatagramHandler newDatagramHandler( SuperSelector wrapper1,
								DatagramChannel channel )
							{
								DatagramHandler h = new EchoDatagramHandler( wrapper1, channel, id, packetCount );
//								Trace.report( h, Trace.m( "bound to " ).a( channel.socket().getLocalSocketAddress() )
//									.a( " connected to " ).a( channel.socket().getRemoteSocketAddress() ) );
								return h;
							}
						});
				waitStarted( 1000*(j+1) );
				return handler;
			}
			catch ( InterruptedException e )
			{
				// ignore.
				Trace.report( Trace.m( "timeout" ) );
			}
			catch ( IOException e )
			{
				Trace.report( e );
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
	
	private Monitor startedMonitor = new Monitor( "started monitor" );
	
	/**
	 * Description of allStarted.
	 */
	boolean goAhead;
	
	private class EchoDatagramHandler extends DatagramHandler implements AlarmListener
	{
		/**
		 * Constructs the EchoDatagramHandler.
		 *
		 * @param wrapper
		 * @param channel
		 * @param id
		 * @param count
		 */
		public EchoDatagramHandler( SuperSelector wrapper, DatagramChannel channel,
			int id, int count )
		{
			super( wrapper, channel );
			this.id = id;
			this.count = count;
		}

		@SuppressWarnings("unused")
		private final int id;
		
		private final int count;

		@Override
		public int selectedInterestOps()
		{
			return 0;
		}

		private final static int MAX_PACKET_SIZE = 200;
		
		@Override
		protected void doRead() throws IOException
		{
			// read a message from the server.
			
			ByteBuffer buf = ByteBuffer.allocate( MAX_PACKET_SIZE );
//			SocketAddress from;
			while (receive( buf ) != null)
			{
				long now = System.currentTimeMillis();
				
				buf.flip();
				int seq = buf.getInt();
				long delay = now - buf.getLong();
				buf.clear();
				
				total_delay += delay;
				recv_count++;
				
//				report( "received from "+from+": seq = "+seq+", delay "+delay );
				
				if (seq == 0)
					notifyStarted();
			}
		}
		
		/**
		 * @return true if the send worked, false otherwise.
		 * @throws IOException
		 */
		public boolean send() throws IOException
		{
			ByteBuffer buf = ByteBuffer.allocate( MAX_PACKET_SIZE );
			
			buf.putInt( sent_count++ );
			buf.putLong( System.currentTimeMillis() );
			while (buf.position() < RTP_PACKET_SIZE)
				buf.putInt( 0 );
			
			buf.flip();
			return write( buf );
		}
		
		/**
		 * Description of sent_seq.
		 */
		public int sent_count = 0;
		
		/**
		 * Description of recv_seq.
		 */
		public int recv_count = 0;
		
		/**
		 * Description of total_delay.
		 */
		public long total_delay = 0;

		@Override
		protected boolean wantsWrite()
		{
			return sent_count == 0 || (goAhead && sent_count == 1);
		}
		
		@Override
		protected void doWrite() throws IOException
		{
			am.add( this, 20 );
			send();
		}

		@Override
		public synchronized void doClose( Object reason )
		{
			am.remove( this );
			
			if (sent_count < count)
				report( Trace.m( "doClose" )
					.a( " " ).a( sent_count )
					.a( " " ).a( recv_count )
					.a( " " ).a( total_delay )
					.a( " " ).a( reason ) );
			
			super.doClose( reason );
			
			done = true;
			notify();
		}
		
		private boolean done;
		
		/**
		 * @param when 
		 * @return true if we are done, false otherwise.
		 * @throws InterruptedException
		 */
		public synchronized boolean waitdone( long when ) throws InterruptedException
		{
			long delay = when - System.currentTimeMillis();
			while (!done && delay > 0)
				wait( delay );
			
			if (!done)
			{
				doClose( "timeout" );
				return false;
			}
			
			return true;
		}

		public synchronized int wakeup( AlarmManager manager, long due )
		{
			if (sent_count >= count)
			{
				doClose( "done" );
				return 0;
			}
			
			try
			{
				if (!send())
					report( "out of buffer space" );
				return 20;
			}
			catch ( ClosedChannelException e )
			{
				// ignore
				return 0;
			}
			catch ( IOException e )
			{
				report( e );
				return 0;
			}
		}
	}
}
