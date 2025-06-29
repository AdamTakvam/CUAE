package metreos.core.net.nio.test;

import java.io.EOFException;
import java.io.IOException;
import java.net.InetSocketAddress;
import java.net.SocketAddress;
import java.nio.channels.SocketChannel;

import metreos.core.ipc.flatmaps.FlatmapIpcMessage;
import metreos.core.net.nio.StreamHandler;
import metreos.core.net.nio.StreamHandlerFactory;
import metreos.core.net.nio.SuperSelector;
import metreos.core.net.nio.SuperSelector0;
import metreos.util.AlarmListener;
import metreos.util.AlarmManager;
import metreos.util.Assertion;
import metreos.util.Monitor;
import metreos.util.Trace;

/**
 * @author wert
 */
public class IpcEchoClient
{
	/**
	 * @param args
	 * @throws Exception 
	 */
	public static void main( String[] args )
		throws Exception
	{
//		SocketAddress address = new InetSocketAddress( "127.0.0.1", 9124 ); // localhost
//		SocketAddress address = new InetSocketAddress( "10.1.12.165", 9124 ); // marge
//		SocketAddress address = new InetSocketAddress( "10.1.12.163", 9124 ); // lenny
		SocketAddress address = new InetSocketAddress( "10.1.12.175", 9124 ); // fred
		int nWorkers = 20;
		int maxSelectorSize = 8;

		Trace.report( Trace.m( "" )
			.a( "\t").a( "nStreams" )
			.a( "\t").a( "startTime" )
			.a( "\t").a( "startRate" )
			.a( "\t").a( "time" )
			.a( "\t").a( "rate" ) );
		
		boolean first = true;
		int i = 1;
		while (i <= 15360)
		{
			if (!first)
			{
				int n = 300000;
				Trace.report( "sleeping "+n );
				Thread.sleep( n );
			}
			else
				first = false;
			
			int limit = 10000 * 240 / i / 2; // 10000 = packets/second, 240 = seconds/run
//			int limit = 10;
			
			new IpcEchoClient().start( address, nWorkers, maxSelectorSize, i, limit );
			
			if (i < 8192)
				i *= 2;
			else
				i += 1024;
		}
		
		Trace.shutdown();
	}

	/**
	 * Constructs the EchoClient.
	 */
	public IpcEchoClient()
	{
		// nothing to do.
	}
	
	private void start( SocketAddress address, int nWorkers,
		int maxSelectorSize, int nStreams,
		int limit ) throws Exception
	{
		Trace.report( Trace.m( "starting" )
			.a( " address=" ).a( address )
			.a( " nStreams=" ).a( nStreams )
			.a( " limit=" ).a( limit ) );

		// make our super selector
		
		SuperSelector superSelector = new SuperSelector0( nWorkers, 0 );
		superSelector.start();
		
		AlarmManager am = new AlarmManager();
		am.start();
		
		long tStart = System.currentTimeMillis();
		
		MyFlatmapIpcStreamHandler[] shs = new MyFlatmapIpcStreamHandler[nStreams];
		for (int i = 0; i < nStreams; i++)
		{
//			Trace.report( Trace.m( "starting " ).a( i ) );
			shs[i] = tryStart( superSelector, address, limit, i, am );
			if (shs[i] == null)
				Trace.report( this, Trace.m( "not started: " ).a( i ) );
		}
		
		long tAllStarted = System.currentTimeMillis();
		long t = tAllStarted-tStart;
		if (t == 0)
			t = 1;
		double startTime = (t) / 1000.0;
		int startRate = (int)(nStreams / startTime);
		Trace.report( Trace.m( "all started in " ).a( startTime )
			.a( ", rate = " ).a( startRate ) );
		
		goAhead = true;
		for (int i = 0; i < nStreams; i++)
		{
			if (shs[i] != null)
				shs[i].speak();
		}
		
		long endTime = System.currentTimeMillis() + 10*60*1000;
		for (int i = 0; i < nStreams; i++)
		{
			if (shs[i] != null)
				shs[i].waitdone( endTime - System.currentTimeMillis() );
		}
		
		long tAllDone = System.currentTimeMillis();
		
		int inputCount = 0;
		int outputCount = 0;
		for (int i = 0; i < nStreams; i++)
		{
			if (shs[i] != null)
			{
				inputCount += shs[i].reqSeqNo;
				outputCount += shs[i].respSeqNo;
			}
		}
		
		int count = inputCount + outputCount;
		
		double time = (tAllDone-tAllStarted) / 1000.0;
		int rate = (int)(count / time);
		Trace.report( Trace.m( "done" )
			.a( "\t").a( nStreams )
			.a( "\t").a( startTime )
			.a( "\t").a( startRate )
			.a( "\t").a( time )
			.a( "\t").a( rate ) );
		
		// let things settle down.
		Thread.sleep( 20000 );
		
		am.stop();
		
		superSelector.stop( 15000 );
	}
	
//	private long t0;
	
//	private int n0;
	
	private MyFlatmapIpcStreamHandler tryStart( SuperSelector wrapper,
		SocketAddress address, final int limit, int i, final AlarmManager am )
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
			try
			{
				if (j > 0)
					Trace.report( Trace.m( "trying again on " ).a( id ) );
				
				clearStarted();
				MyFlatmapIpcStreamHandler handler = (MyFlatmapIpcStreamHandler)
					wrapper.newStreamHandler( address, false,
						new StreamHandlerFactory()
						{
							public StreamHandler newStreamHandler( SuperSelector wrapper1,
								SocketChannel channel )
							{
								return new MyFlatmapIpcStreamHandler( wrapper1,
									channel, id, limit, am );
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
	
	/**
	 * Filler to pad out the message length to 300 bytes.
	 * The message we are sending is about 73 bytes long without
	 * the filler. So the filler needs to be about 227 bytes long.
	 */
	final static String FILLER = "blah, blah, blah, blah, blah, blah, blah," +
			" blah, blah, blah, blah, blah, blah, blah, blah, blah, blah," +
			" blah, blah, blah, blah, blah, blah, blah, blah, blah, blah," +
			" blah, blah, blah, blah, blah, blah, blah, blah, blah, blah," +
			" blah.";
	
	private class MyFlatmapIpcStreamHandler extends FlatmapIpcStreamHandler
		implements AlarmListener
	{
		/**
		 * Constructs the MyFlatmapIpcStreamHandler.
		 *
		 * @param wrapper
		 * @param channel
		 * @param id
		 * @param limit
		 * @param am 
		 */
		public MyFlatmapIpcStreamHandler( SuperSelector wrapper,
			SocketChannel channel, int id, int limit, AlarmManager am )
		{
			super( wrapper, channel, 1 );
			this.id = "MyFlatmapIpcStreamHandler "+id;
			this.limit = limit;
			this.am = am;
			//report( Trace.m( "startedCount" ) );
		}

		@SuppressWarnings("unused")
		private final String id;
		
		private final int limit;
		
		private AlarmManager am;
		
		private boolean starting = true;

//		private boolean doNotifyStarted = true;

		@Override
		protected void handleMessage( FlatmapIpcMessage msg ) throws IOException
		{
			am.remove( this );
			
			Assertion.check( msg.messageType == REQ_MSG_TYPE+1, "msg.messageType == REQ_MSG_TYPE+1" );
			
			Integer i = msg.args.getInteger( REQ_SEQ_NO_FIELD );
			Assertion.check( i.intValue() == reqSeqNo-1, "i.intValue() == reqSeqNo-1" );
			
			Integer j = msg.args.getInteger( RESP_SEQ_NO_FIELD );
			Assertion.check( j.intValue() == respSeqNo+1, "j.intValue() == respSeqNo+1" );
			respSeqNo = j.intValue();
			
			String k = msg.args.getString( FILLER_FIELD );
			Assertion.check( k.equals( FILLER ), "k.equals( FILLER )" );
			
			if (i.intValue() == 0)
			{
				notifyStarted();
				return;
			}
			
			if (reqSeqNo >= limit)
				throw new EOFException();

			speak();
		}

		/**
		 * @throws IOException
		 */
		public void speak() throws IOException
		{
			alarms = 0;
			am.add( this, TICKLE_TIME );
			send( new FlatmapIpcMessage( REQ_MSG_TYPE, null, null )
				.add( REQ_SEQ_NO_FIELD, reqSeqNo++ ).add( FILLER_FIELD, FILLER ) );
		}
		
		private final static int TICKLE_TIME = 5*1000;

		public int wakeup( AlarmManager manager, long now )
		{
			alarms++;
			if (alarms >= 3)
			{
				doClose( "timeout" );
				return 0;
			}
			return TICKLE_TIME;
		}
		
		private int alarms = 0;
		
		private final int REQ_MSG_TYPE = 1;
		
		private final int REQ_SEQ_NO_FIELD = 1;
		
		private final int RESP_SEQ_NO_FIELD = 2;
		
		private final int FILLER_FIELD = 99;
		
		/**
		 * Description of reqSeqNo.
		 */
		int reqSeqNo = 0;
		
		/**
		 * Description of respSeqNo.
		 */
		int respSeqNo = 0;

		@Override
		protected boolean wantsWrite()
		{
			if (starting)
				return true;
			
			if (!goAhead)
				return false;
			
			return super.wantsWrite();
		}
		
		@Override
		protected void doWrite() throws IOException
		{
			if (starting)
			{
				starting = false;
				speak();
				return;
			}
			
			super.doWrite();
		}

		@Override
		public void doClose( Object reason )
		{
			if (reason != EOF)
				Trace.report( this, Trace.m( "doClose" )
					.a( " " ).a( reqSeqNo )
					.a( " " ).a( respSeqNo )
					.a( " " ).a( reason ) );
			
			super.doClose( reason );
			
			saydone();
		}
		
		private synchronized void saydone()
		{
			done = true;
			notify();
		}
		
		/**
		 * @param maxDelay 
		 * @throws InterruptedException
		 */
		public synchronized void waitdone( long maxDelay ) throws InterruptedException
		{
			while (!done)
				wait( maxDelay > 0 ? maxDelay : 1 );
		}
		
		private boolean done;
	}
}
