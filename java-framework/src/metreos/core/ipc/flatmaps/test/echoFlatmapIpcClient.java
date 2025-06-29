/* $Id: echoFlatmapIpcClient.java 10080 2005-09-06 15:45:12Z METREOS\wert $
 *
 * Created by wert on Jan 28, 2005
 *
 * Copyright (c) 2005 Metreos, Inc. All rights reserved.
 */

package metreos.core.ipc.flatmaps.test;

import java.io.IOException;

import metreos.core.ipc.flatmaps.FlatmapIpcListener;
import metreos.core.ipc.flatmaps.FlatmapIpcListenerAdapter;
import metreos.core.ipc.flatmaps.FlatmapIpcMessage;
import metreos.core.ipc.flatmaps.FlatmapList;
import metreos.core.net.ipc.IpcConnection;

/**
 * Test flatmap ipc.
 */
public class echoFlatmapIpcClient implements FlatmapIpcListener<IpcConnection>
{
	/**
	 * Value to add to messageType when replying.
	 */
	protected static final int OFFSET = 1000;

	/**
	 * Main program.
	 * @param args
	 * @throws InterruptedException
	 * @throws IOException
	 */
	public static void main( String[] args ) throws InterruptedException, IOException
	{
		IpcConnection connection = new IpcConnection( "localhost", 9001 );
		
		// listen for responses...
		
		echoFlatmapIpcClient client = new echoFlatmapIpcClient();
		connection.addListener( new FlatmapIpcListenerAdapter<IpcConnection>( client, null ) );

		System.out.println( "starting connection..." );
		connection.start();
		connection.waitStarted( 15*1000 );
		System.out.println( "started." );
		
		FlatmapList fmargs = new FlatmapList();
		fmargs.add( 1, 19 );
		fmargs.add( 2, "fred" );
		fmargs.add( 3, new byte[] { 1, 2, 3 } );
		
		long t0 = System.currentTimeMillis();
		
		for (int i = 0; i < 40000; i++)
			connection.send( new FlatmapIpcMessage( i, fmargs, null, null ) );
		
		connection.send( new FlatmapIpcMessage( -1, fmargs, null, null ) );
		
		client.waitDone( 15*1000 );
		
		long t1 = System.currentTimeMillis();
		System.out.println( "t = "+(t1-t0) );
		
		System.out.println( "closing connection..." );
		connection.close();
		System.out.println( "closed." );
	}

	/////////////////////
	// CLIENT LISTENER //
	/////////////////////
	
	/**
	 * @param timeout
	 * @throws InterruptedException
	 */
	public synchronized void waitDone( int timeout ) throws InterruptedException
	{
		wait( timeout );
	}

	public void startup( IpcConnection connection )
	{
		System.out.println( connection.toString()+": startup" );
	}

	public synchronized void received( IpcConnection connection, FlatmapIpcMessage message )
	{
//		System.out.println( connection.toString()+": received: "+
//				message.messageType+", args = "+message.args );
		
		if (message.messageType < 0)
		{
			// done!
			notifyAll();
			return;
		}
		
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
}
