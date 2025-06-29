package metreos.core.net.nio.test;

import java.io.IOException;
import java.net.DatagramSocket;
import java.net.InetSocketAddress;
import java.net.SocketAddress;
import java.nio.ByteBuffer;
import java.nio.channels.ClosedChannelException;
import java.nio.channels.DatagramChannel;

import metreos.util.AlarmListener;
import metreos.util.AlarmManager;
import metreos.util.Monitor;
import metreos.util.Trace;


/**
 * @author wert
 */
public class EchoClient3a
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
//		SocketAddress to = new InetSocketAddress( "10.1.12.175", 9123 ); // fred
		SocketAddress to = new InetSocketAddress( "10.1.12.102", 9123 ); // javaman
//		SocketAddress to = new InetSocketAddress( "127.0.0.1", 9123 ); // localhost
		
//		SocketAddress from = new InetSocketAddress( "127.0.0.1", 0 ); // localhost
		SocketAddress from = null;
		
		int[] counts = { 192, 256, 384, 512, 640, 768 };
		for (int i = 0; i < counts.length; i++)
		{
			if (i != 0)
				Thread.sleep( 15*1000 );
			
			int count = counts[i];
			
//			int k = 10000 * 240 / count / 2; // 10000 = packets/second, 240 = seconds/run
			int k = 50 * 120; // 120 seconds of audio (at 50 packets / second)
			
			new EchoClient3a().start( from, to, count, k );
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
	public EchoClient3a()
	{
		// nothing to do.
	}
	
	private void start( SocketAddress from, SocketAddress to,
		int nClients, int packetCount ) throws Exception
	{
		Trace.report( Trace.m( "starting" )
			.a( " from=" ).a( from )
			.a( " to=" ).a( to )
			.a( " nClients=" ).a( nClients )
			.a( " packetCount=" ).a( packetCount ) );

		am.start( 1 );
		
		long tStart = System.currentTimeMillis();
		
		EchoDatagramHandler[] shs = new EchoDatagramHandler[nClients];
		for (int i = 0; i < nClients; i++)
		{
			shs[i] = tryStart( from, to, i, packetCount );
			if (shs[i] == null)
				Trace.report( Trace.m( "not started: " ).a( i ) );
		}
		
		long tAllStarted = System.currentTimeMillis();
		Trace.report( Trace.m( "all started in " ).a( tAllStarted-tStart ) );

//		Trace.report( Trace.m( "waiting..." ) );
//		Thread.sleep( 30*60*1000 );
//		Trace.report( Trace.m( "done waiting." ) );
		
		for (int i = 0; i < nClients; i++)
		{
			if (shs[i] != null)
				shs[i].goAhead();
		}
		
		long when = System.currentTimeMillis() + 5*60*1000;
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
		int dropped = nClients * packetCount * 2 - count;
		int percent = dropped * 100 / count;
		
		double time = (tAllDone-tAllStarted) / 1000.0;
		
		Trace.report( Trace.m( "done in " ).a( time )
			.a( ", with " ).a( count )
			.a( " / " ).a( dropped )
			.a( " (").a( percent ).a( "%)" )
			.a( " (").a( (int) (count / time) ).a( " p/s)" ) );
		
		am.stop();
//		t.join();
		
		// let things settle down.
		Thread.sleep( 15000 );
	}
	
	/**
	 * Description of am.
	 */
	AlarmManager am = new AlarmManager();
	
	private final static int UDP_BUF_SIZE = 32*1024;
	
//	private long t0;
	
//	private int n0;
	
	private EchoDatagramHandler tryStart( SocketAddress from, SocketAddress to,
		final int id, final int packetCount )
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
				
				DatagramChannel dc = DatagramChannel.open();
				
				DatagramSocket ds = dc.socket();
				ds.setSendBufferSize( UDP_BUF_SIZE );
				ds.setReceiveBufferSize( UDP_BUF_SIZE );
				ds.bind( from );
				
				dc.connect( to );
				
				EchoDatagramHandler handler = new EchoDatagramHandler( dc, id, packetCount );
				handler.start();
				handler.send();
				
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
	
	private class EchoDatagramHandler extends Thread implements AlarmListener
	{
		/**
		 * Constructs the EchoDatagramHandler.
		 *
		 * @param channel
		 * @param id
		 * @param count
		 */
		public EchoDatagramHandler( DatagramChannel channel, int id, int count )
		{
			super( "EchoDatagramHandler "+id );
			this.channel = channel;
			this.count = count;
		}
		
		/**
		 * @throws IOException 
		 * 
		 */
		public void goAhead() throws IOException
		{
			am.add( this, 20 );
			send();
		}

		private final DatagramChannel channel;
		
		private final int count;

		private final static int MAX_PACKET_SIZE = 200;

		@Override
		public void run()
		{
			try
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
			catch ( Exception e )
			{
				Trace.report( e );
			}
		}
		
		private SocketAddress receive( ByteBuffer buf ) throws IOException
		{
			try
			{
				return channel.receive( buf );
			}
			catch ( ClosedChannelException e )
			{
				return null;
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
		
		private boolean write( ByteBuffer buf ) throws IOException
		{
			try
			{
				channel.write( buf );
				return true;
			}
			catch ( ClosedChannelException e )
			{
				// ignore.
				return false;
			}
			catch ( IOException e )
			{
				String s = e.getMessage();
				if (s != null && s.indexOf( "buffer space" ) >= 0)
					return false;
				throw e;
			}
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

		/**
		 * @param reason
		 */
		public synchronized void doClose( Object reason )
		{
			am.remove( this );
			
			if (sent_count < count)
				Trace.report( Trace.m( "doClose" )
					.a( " " ).a( sent_count )
					.a( " " ).a( recv_count )
					.a( " " ).a( total_delay )
					.a( " " ).a( reason ) );
			
			if (reason instanceof Exception)
				Trace.report( (Exception) reason );
			
			try
			{
				channel.close();
			}
			catch ( IOException e )
			{
				Trace.report( e );
			}
			
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
					Trace.report( "out of buffer space" );
				return 20;
			}
			catch ( ClosedChannelException e )
			{
				// ignore
				return 0;
			}
			catch ( IOException e )
			{
				Trace.report( e );
				return 0;
			}
		}
	}
}
