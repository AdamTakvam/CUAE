/* $Id: testServer.java 30151 2007-03-06 21:47:46Z wert $
 *
 * Created by wert on Jan 26, 2005
 *
 * Copyright (c) 2005 Metreos, Inc. All rights reserved.
 */

package metreos.core.net.test;

import java.io.EOFException;
import java.io.IOException;
import java.io.InputStream;
import java.io.OutputStream;
import java.net.Socket;
import java.net.SocketTimeoutException;

import metreos.core.net.ConnectionHandler;
import metreos.core.net.DefaultServerListener;
import metreos.core.net.PlainServerSocketFactory;
import metreos.core.net.Server;
import metreos.core.net.ServerListener;
import metreos.core.net.ServerSocketFactory;

/**
 * Test the Server class.
 */
public class testServer
{
	/**
	 * The main program.
	 * @param args
	 * @throws InterruptedException
	 */
	public static void main( String[] args ) throws InterruptedException
	{
		ServerSocketFactory ssf = new PlainServerSocketFactory();
		
		ConnectionHandler ch = new ConnectionHandler()
		{
			public String getDescription()
			{
				return "snort";
			}

			public ServerListener newConnection( Server server, Socket socket )
			{
				new MyConnection( server, socket, this ).start();
				return null;
			}
		};
		
		Server server = new Server( ssf, ch, null, 0, null );
		server.addListener( new DefaultServerListener() );
		server.start();
		
		Thread.sleep( 15*1000 );
		
		System.out.println( "shutting down..." );
		server.shutdown( "your mama", 0 );
		System.out.println( "shut down." );
	}
	
	/**
	 * A thread to manage the connection.
	 */
	public static class MyConnection extends Thread implements ServerListener
	{
		/**
		 * Constructs a thread to manage the connection.
		 * @param server
		 * @param socket
		 * @param ch
		 */
		public MyConnection( Server server, Socket socket, ConnectionHandler ch )
		{
			super( "Server for "+ch.getDescription()+" remote "+
				socket.getInetAddress().getHostAddress()+":"+socket.getPort() );
			this.server = server;
			this.socket = socket;
//			this.ch = ch;
		}
		
		private final Server server;
		
		private final Socket socket;
		
//		private final ConnectionHandler ch;
		
		private boolean running = true;

		@Override
		public void run()
		{
			server.addListener( this );
			
			InputStream is = null;
			OutputStream os = null;
			
			try
			{
				socket.setSoTimeout( 100 );
				
				is = socket.getInputStream();
				os = socket.getOutputStream();
				
				os.write( 'r' );
				os.write( 'e' );
				os.write( 'a' );
				os.write( 'd' );
				os.write( 'y' );
				os.write( '\r' );
				os.write( '\n' );
				os.flush();
				
				while (running)
				{
					int c;
					
					try
					{
						c = is.read();
					}
					catch ( EOFException e )
					{
						c = -1;
					}
					catch ( SocketTimeoutException e )
					{
						continue;
					}
					
					System.out.println( toString()+": got c = "+c );
					
					if (c < 0)
						break;
					
					os.write( c );
					os.flush();
				}
				
				os.write( 'd' );
				os.write( 'o' );
				os.write( 'n' );
				os.write( 'e' );
				os.write( '\r' );
				os.write( '\n' );
				os.flush();
			}
			catch ( IOException e )
			{
				e.printStackTrace();
			}
			finally
			{
				running = false;
				server.removeListener( this );
				
				try
				{
					socket.close();
				}
				catch ( Exception e1 )
				{
					e1.printStackTrace();
				}
			}
		}

		/* (non-Javadoc)
		 * @see metreos.core.net.ServerListener#started(metreos.core.net.Server)
		 */
		public void started( Server server1 )
		{
			// ignore
		}

		/* (non-Javadoc)
		 * @see metreos.core.net.ServerListener#stopped(metreos.core.net.Server)
		 */
		public void stopped( Server server1 )
		{
			// ignore
		}

		/* (non-Javadoc)
		 * @see metreos.core.net.ServerListener#exception(metreos.core.net.Server, java.lang.String, java.lang.Exception)
		 */
		public void exception( Server server1, String what, Exception e )
		{
			// ignore
		}

		/* (non-Javadoc)
		 * @see metreos.core.net.ServerListener#shutdown(metreos.core.net.Server, java.lang.String)
		 */
		public void shutdown( Server server1, String reason )
		{
			running = false;
			System.out.println( toString()+": shutdown: "+reason );
		}
	}
}
