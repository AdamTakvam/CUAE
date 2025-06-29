/* $Id: testIpc.java 30151 2007-03-06 21:47:46Z wert $
 *
 * Created by wert on Jan 27, 2005
 *
 * Copyright (c) 2005 Metreos, Inc. All rights reserved.
 */

package metreos.core.net.ipc.test;

import java.io.IOException;
import java.net.Socket;
import java.net.UnknownHostException;

import metreos.core.net.ConnectionHandler;
import metreos.core.net.PlainServerSocketFactory;
import metreos.core.net.Server;
import metreos.core.net.ServerSocketFactory;
import metreos.core.net.ipc.IpcConnection;
import metreos.core.net.ipc.IpcConnectionHandler;
import metreos.core.net.ipc.IpcListener;
import metreos.core.net.ipc.IpcMessage;

/**
 * Test the basic functionality of the Ipc connection.
 */
public class testIpc implements IpcListener<IpcConnection>
{
	/**
	 * Main program.
	 * @param args
	 * @throws InterruptedException
	 * @throws UnknownHostException
	 * @throws IOException
	 */
	public static void main( String[] args ) throws InterruptedException, UnknownHostException, IOException
	{
		ServerSocketFactory ssf = new PlainServerSocketFactory();
		
		ConnectionHandler ch = new IpcConnectionHandler<IpcConnection>()
		{
			@Override
			protected void addListeners( IpcConnection connection )
			{
				connection.addListener( new testIpc() );
			}

			@Override
			protected IpcConnection newIpcConnection( Server server, Socket socket )
				throws IOException
			{
				return new IpcConnection( server, socket );
			}
		};
		
		Server server = new Server( ssf, ch, null, 2345, null );
		server.start();
		
		Thread.sleep( 5*1000 );
		
		IpcConnection conn = new IpcConnection( "localhost", 2345 );
		
		conn.addListener( new IpcListener<IpcConnection>()
		{
			public void startup( IpcConnection connection )
			{
				System.out.println( connection.toString()+": startup" );
			}

			public void received( IpcConnection connection, IpcMessage message )
			{
				System.out.println( connection.toString()+": received: "+message );
			}

			public void shutdown( IpcConnection connection )
			{
				System.out.println( connection.toString()+": shutdown" );
			}

			public void exception( IpcConnection connection, Exception e )
			{
				System.out.println( connection.toString()+": exception" );
				e.printStackTrace();
			}
		} );

		System.out.println( "starting conn..." );
		conn.start();
		System.out.println( "started." );
		
		Thread.sleep( 5*1000 );

		System.out.println( "sending 1, 2, 3..." );
		byte[] bytes = { 1, 2, 3 };
		conn.send( 0x27|IpcConnection.HAS_ADDITIONAL_FLAG, 0x28, bytes );
		System.out.println( "sent 1, 2, 3." );
		
		Thread.sleep( 5*1000 );
		
		conn.close();
		
		System.out.println( "shutting down..." );
		server.shutdown( "your mama", 0 );
		System.out.println( "shut down." );
	}

	/* (non-Javadoc)
	 * @see metreos.core.net.ipc.IpcListener#startup(metreos.core.net.ipc.IpcConnection)
	 */
	public void startup( IpcConnection connection )
	{
		System.out.println( connection.toString()+": startup" );
	}

	/* (non-Javadoc)
	 * @see metreos.core.net.ipc.IpcListener#payloadReceived(metreos.core.net.ipc.IpcConnection, int, int, byte[])
	 */
	public void received( IpcConnection connection, IpcMessage message )
	{
		System.out.println( connection.toString()+": received: "+message );
		try
		{
			System.out.println( "sending 2, 3, 4..." );
			byte[] bytes = { 2, 3, 4 };
			connection.send( 0x37|IpcConnection.HAS_ADDITIONAL_FLAG, 0x38, bytes );
			System.out.println( "sent 2, 3, 4..." );
		}
		catch ( Exception e )
		{
			exception( connection, e );
		}
	}

	/* (non-Javadoc)
	 * @see metreos.core.net.ipc.IpcListener#shutdown(metreos.core.net.ipc.IpcConnection)
	 */
	public void shutdown( IpcConnection connection )
	{
		System.out.println( connection.toString()+": shutdown" );
	}

	/* (non-Javadoc)
	 * @see metreos.core.net.ipc.IpcListener#exception(metreos.core.net.ipc.IpcConnection, java.lang.Exception)
	 */
	public void exception( IpcConnection connection, Exception e )
	{
		System.out.println( connection.toString()+": exception" );
		e.printStackTrace();
	}
}
