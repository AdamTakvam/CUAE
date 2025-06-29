package metreos.core.net.nio.test;
import java.io.BufferedInputStream;
import java.io.BufferedOutputStream;
import java.io.DataInputStream;
import java.io.DataOutputStream;
import java.io.IOException;
import java.net.Socket;

/**
 * @author wert
 */
public class EchoClient extends Thread
{
	/**
	 * @param args
	 * @throws InterruptedException
	 */
	public static void main( String[] args ) throws InterruptedException
	{
		int nThreads = 2;
		int nSocketsPerThread = 2;
		int nWords = 40000000/(nThreads*nSocketsPerThread*2);

		System.out.println( "sockets = "+nThreads*nSocketsPerThread );
		System.out.println( "bytes = "+nWords*4 );
		
		EchoClient[] ecs = new EchoClient[nThreads];
		
		for (int i = 0; i < nThreads; i++)
			ecs[i] = new EchoClient( i, nSocketsPerThread, nWords );
		
		long t0 = System.currentTimeMillis();
		
		for (int i = 0; i < nThreads; i++)
			ecs[i].start();
		
		for (int i = 0; i < nThreads; i++)
			ecs[i].join();
		
		long t1 = System.currentTimeMillis();
		
		report( "all", nThreads*nSocketsPerThread*2*nWords, t1-t0 );
	}
	
	/**
	 * @param who
	 * @param words
	 * @param ticks
	 */
	public static void report( String who, int words, long ticks )
	{
		long bytes = words * 4L;
		double seconds = ticks / 1000.0;
		System.out.println( who+": bytes = "+words*4+", ticks = "+ticks+", rate = "+(bytes/seconds)+" bytes / second" );
	}
	
	/**
	 * Constructs the EchoClient.
	 *
	 * @param id
	 * @param nSockets
	 * @param nWords
	 */
	public EchoClient( int id, int nSockets, int nWords )
	{
		this.id = id;
		this.nSockets = nSockets;
		this.nWords = nWords;
	}
	
	private final int id;
	
	private final int nSockets;
	
	private final int nWords;

	@Override
	public void run()
	{
		try
		{
			final EchoReader[] ers = new EchoReader[nSockets];
			EchoWriter[] ews = new EchoWriter[nSockets];
			
			for (int i = 0; i < nSockets; i++)
			{
				Socket s = new Socket( "localhost", 9123 );
				
				DataInputStream dis = new DataInputStream( new BufferedInputStream( s.getInputStream() ) );
				DataOutputStream dos = new DataOutputStream( new BufferedOutputStream( s.getOutputStream() ) );
				
				ers[i] = new EchoReader( id*nSockets+i, s, dis, nWords, 75 );
				ews[i] = new EchoWriter( id*nSockets+i, dos, nWords, 75 );
			}
			
			Thread rdr = new Thread()
			{
				@Override
				public void run()
				{
					try
					{
						doit( ers );
					}
					catch ( Exception e )
					{
						e.printStackTrace();
					}
				}
			};
			
			rdr.start();
			
			doit( ews );
			
			rdr.join( 30000 );
		}
		catch ( Exception e )
		{
			e.printStackTrace();
		}
	}
	
	/**
	 * @param ers
	 * @throws IOException
	 */
	void doit( EchoReader[] ers ) throws IOException
	{
		int n = ers.length;
		boolean more = true;
		while (more)
		{
			more = false;
			for (int i = 0; i < n; i++)
			{
				EchoReader er = ers[i];
				if (er == null)
					continue;
				
				if (er.doit())
					more = true;
				else
					ers[i] = null;
			}	
		}
	}
	
	private void doit( EchoWriter[] ews ) throws IOException
	{
		int n = ews.length;
		boolean more = true;
		while (more)
		{
			more = false;
			for (int i = 0; i < n; i++)
			{
				EchoWriter ew = ews[i];
				if (ew == null)
					continue;
				
				if (ew.doit())
					more = true;
				else
					ews[i] = null;
			}	
		}
	}
	
/**
 * Description of EchoReader.
 */
//	public void xrun()
//	{
//		Socket s = null;
//		try
//		{
//			s = new Socket( "localhost", 9123 );
//			
//			InputStream is = new BufferedInputStream( s.getInputStream() );
//			OutputStream os = new BufferedOutputStream( s.getOutputStream() );
//			
//			long t0 = System.currentTimeMillis();
//			
//			EchoReader er = new EchoReader( new DataInputStream( is ), n );
//			er.start();
//			
////			System.out.println( "echo reader started" );
//			
//			DataOutputStream dos = new DataOutputStream( os );
//			int i = 0;
//			while (i < n)
//			{
//				dos.writeInt( i++ );
//			}
//			dos.flush();
//			
////			System.out.println( "waiting for echo reader to catch up" );
//			
//			er.join( 15000 );
//			
//			long t1 = System.currentTimeMillis();
//			
//			if (er.next != n)
//				System.out.println( "**** er.next ("+er.next+") != n ("+n+")" );
//			
//			report( "echo reader "+id, er.next+n, t1-t0 );
//		}
//		catch ( Exception e )
//		{
//			e.printStackTrace();
//		}
//		finally
//		{
//			try
//			{
//				s.close();
//			}
//			catch ( Exception e )
//			{
//				e.printStackTrace();
//			}
//		}
//	}
	
	public static class EchoReader
	{
		/**
		 * Constructs the EchoReader.
		 *
		 * @param id
		 * @param socket
		 * @param dis
		 * @param nWords
		 * @param chunkSize
		 */
		public EchoReader( int id, Socket socket, DataInputStream dis, int nWords, int chunkSize )
		{
			this.socket = socket;
			this.id = id;
			this.dis = dis;
			this.nWords = nWords;
			this.chunkSize = chunkSize;
		}

		private final int id;
		
		private final Socket socket;
		
		private final DataInputStream dis;
		
		private final int nWords;
		
		private final int chunkSize;
		
		/**
		 * @return true if doit should be called again, false otherwise.
		 * @throws IOException
		 */
		public boolean doit() throws IOException
		{
			if (next == 0)
				t0 = System.currentTimeMillis();
			
			int i = 0;
			while (i < chunkSize && next < nWords)
			{
				int b = dis.readInt();
				if (b != next)
					throw new IOException( "got "+b+", wanted "+next );
				next++;
				i++;
			}
			if (next >= nWords)
			{
				long t1 = System.currentTimeMillis();
				report( "echo reader "+id, nWords, t1-t0 );
				socket.close();
				return false;
			}
			return true;
		}
		
		/**
		 * Description of next.
		 */
		int next = 0;
		
		private long t0;
	}
	
	/**
	 * Description of EchoWriter.
	 */
	public static class EchoWriter
	{
		/**
		 * Constructs the EchoWriter.
		 *
		 * @param id
		 * @param dos
		 * @param nWords
		 * @param chunkSize
		 */
		public EchoWriter( int id, DataOutputStream dos, int nWords, int chunkSize )
		{
			this.id = id;
			this.dos = dos;
			this.nWords = nWords;
			this.chunkSize = chunkSize;
		}
		
		private final int id;

		private final DataOutputStream dos;
		
		private final int nWords;
		
		private final int chunkSize;
		
		/**
		 * @return true if doit should be called again, false otherwise.
		 * @throws IOException
		 */
		public boolean doit() throws IOException
		{
			if (next == 0)
				t0 = System.currentTimeMillis();
			
			int i = 0;
			while (i < chunkSize && next < nWords)
			{
				dos.writeInt( next );
				next++;
				i++;
			}
			dos.flush();
			
			if (next >= nWords)
			{
				long t1 = System.currentTimeMillis();
				report( "echo writer "+id, nWords, t1-t0 );
				return false;
			}
			
			return true;
		}
		
		private int next = 0;
		
		private long t0;
	}
}
