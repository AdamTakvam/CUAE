/* $Id: testFlatmapIpc.java 30151 2007-03-06 21:47:46Z wert $
 *
 * Created by wert on Jan 28, 2005
 *
 * Copyright (c) 2005 Metreos, Inc. All rights reserved.
 */

package metreos.core.ipc.flatmaps.test;

import java.io.IOException;
import java.net.Socket;

import metreos.core.ipc.flatmaps.FlatmapIpcListener;
import metreos.core.ipc.flatmaps.FlatmapIpcListenerAdapter;
import metreos.core.ipc.flatmaps.FlatmapIpcMessage;
import metreos.core.ipc.flatmaps.FlatmapList;
import metreos.core.net.ConnectionHandler;
import metreos.core.net.PlainServerSocketFactory;
import metreos.core.net.Server;
import metreos.core.net.ServerSocketFactory;
import metreos.core.net.ipc.IpcConnection;
import metreos.core.net.ipc.IpcConnectionHandler;

/**
 * Test flatmap ipc.
 */
public class testFlatmapIpc implements FlatmapIpcListener<IpcConnection>
{
	/**
	 * Amount server adds to incoming message type when replying.
	 */
	protected static final int OFFSET = 100000;

	/**
	 * Main program.
	 * @param args
	 * @throws InterruptedException
	 * @throws IOException
	 */
	public static void main( String[] args ) throws InterruptedException, IOException
	{
		ServerSocketFactory ssf = new PlainServerSocketFactory();
		
		ConnectionHandler ch = new IpcConnectionHandler<IpcConnection>()
		{
			@Override
			protected void addListeners( IpcConnection connection )
			{
				connection.addListener( new FlatmapIpcListenerAdapter<IpcConnection>( new testFlatmapIpc(), null ) );
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
		
		// listen for responses...
		conn.addListener( new FlatmapIpcListenerAdapter<IpcConnection>( new FlatmapIpcListener<IpcConnection>()
		{
			public void startup( IpcConnection connection )
			{
				System.out.println( connection.toString()+": startup" );
			}

			public void received( IpcConnection connection, FlatmapIpcMessage message )
			{
//				System.out.println( connection.toString()+": received: "+
//						message.messageType+", args = "+message.args );
				if (message.messageType != expectedMessageType)
					System.out.println( "got "+message.messageType+
							", expected "+expectedMessageType );
				
				expectedMessageType = message.messageType+1;
				count++;
			}
			
			private int expectedMessageType = OFFSET;
			
			private int count = 0;

			public void shutdown( IpcConnection connection )
			{
				System.out.println( connection.toString()+": shutdown" );
				System.out.println( "count = "+count );
			}

			public void exception( IpcConnection connection, Exception e )
			{
				System.out.println( connection.toString()+": exception" );
				e.printStackTrace();
			}
		}, null ) );

		System.out.println( "starting conn..." );
		conn.start();
		System.out.println( "started." );
		
		Thread.sleep( 5*1000 );
		
		FlatmapList fmargs = new FlatmapList();
		fmargs.add( 1, 19 );
		fmargs.add( 2, "fred" );
		fmargs.add( 3, new byte[] { 1, 2, 3 } );
		
//		System.out.println( "sending 23( "+fmargs+" )..." );
		for (int i = 0; i < 100; i++)
			conn.send( new FlatmapIpcMessage( i, fmargs, null, null ) );
//		System.out.println( "sent 23( "+fmargs+" )." );
		
		Thread.sleep( 15*1000 );
		
		conn.close();
		
		System.out.println( "shutting down..." );
		server.shutdown( "your mama", 0 );
		System.out.println( "shut down." );
	}

	/* (non-Javadoc)
	 * @see metreos.core.ipc.flatmaps.FlatmapIpcListener#startup(metreos.core.net.ipc.IpcConnection)
	 */
	public void startup( IpcConnection connection )
	{
		System.out.println( connection.toString()+": startup" );
	}

	/* (non-Javadoc)
	 * @see metreos.core.ipc.flatmaps.FlatmapIpcListener#received(metreos.core.net.ipc.IpcConnection, metreos.core.ipc.flatmaps.FlatmapIpcMessage)
	 */
	public void received( IpcConnection connection, FlatmapIpcMessage message )
	{
//		System.out.println( connection.toString()+": received: "+
//			message.messageType+", args = "+message.args );
		
		try
		{
//			System.out.println( "sending 24( "+fmargs+" )..." );
			connection.send( new FlatmapIpcMessage( message.messageType+OFFSET,
				message.args, null, null ) );
//			System.out.println( "sent 24( "+fmargs+" )." );
		}
		catch ( Exception e )
		{
			exception( connection, e );
		}
	}

	/* (non-Javadoc)
	 * @see metreos.core.ipc.flatmaps.FlatmapIpcListener#shutdown(metreos.core.net.ipc.IpcConnection)
	 */
	public void shutdown( IpcConnection connection )
	{
		System.out.println( connection.toString()+": shutdown" );
	}

	/* (non-Javadoc)
	 * @see metreos.core.ipc.flatmaps.FlatmapIpcListener#exception(metreos.core.net.ipc.IpcConnection, java.lang.Exception)
	 */
	public void exception( IpcConnection connection, Exception e )
	{
		System.out.println( connection.toString()+": exception" );
		e.printStackTrace();
	}
}
