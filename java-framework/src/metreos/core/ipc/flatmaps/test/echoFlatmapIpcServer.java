/* $Id: echoFlatmapIpcServer.java 30151 2007-03-06 21:47:46Z wert $
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
import metreos.core.net.ConnectionHandler;
import metreos.core.net.DefaultServerListener;
import metreos.core.net.PlainServerSocketFactory;
import metreos.core.net.Server;
import metreos.core.net.ServerSocketFactory;
import metreos.core.net.ipc.IpcConnection;
import metreos.core.net.ipc.IpcConnectionHandler;

/**
 * Test flatmap ipc.
 */
public class echoFlatmapIpcServer implements FlatmapIpcListener<IpcConnection>
{
	/**
	 * Value to add to message type when replying.
	 */
	protected static final int OFFSET = echoFlatmapIpcClient.OFFSET;

	/**
	 * Main program.
	 * @param args
	 * @throws InterruptedException
	 */
	public static void main( String[] args ) throws InterruptedException
	{
		ServerSocketFactory ssf = new PlainServerSocketFactory();
		
		ConnectionHandler ch = new IpcConnectionHandler<IpcConnection>()
		{
			@Override
			protected void addListeners( IpcConnection connection )
			{
				connection.addListener( new FlatmapIpcListenerAdapter<IpcConnection>(
					new echoFlatmapIpcServer(), null ) );
			}

			@Override
			protected IpcConnection newIpcConnection( Server server, Socket socket )
				throws IOException
			{
				return new IpcConnection( server, socket );
			}
		};
		
		Server server = new Server( ssf, ch, null, 9001, null );
		server.addListener( new DefaultServerListener()
		{
			@Override
			public void exception( Server server1, String what, Exception e )
			{
				System.err.println( "server "+server1+" caught exception" );
				e.printStackTrace();
			}
		} );
		server.start();
		server.waitStarted( 15*1000 );
		
		System.out.println( "server started on port "+server.getPort() );
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
		System.out.println( connection.toString()+": received: "+
			message.messageType+", args = "+message.args );
		
		try
		{
			if (message.messageType < 0)
			{
				connection.send( new FlatmapIpcMessage( -1, message.args, null, null ) );
				return;
			}
			
//			System.out.println( "sending 24( "+fmargs+" )..." );
			connection.send( new FlatmapIpcMessage( message.messageType+OFFSET, message.args, null, null ) );
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
