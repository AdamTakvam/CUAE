/* $Id: testJTapiServer.java 12104 2005-10-19 19:18:25Z wert $
 *
 * Created by wert on Jan 31, 2005
 *
 * Copyright (c) 2005 Metreos, Inc. All rights reserved.
 */

package metreos.service.jtapi.test;

import metreos.core.ipc.flatmaps.FlatmapIpcListener;
import metreos.core.ipc.flatmaps.FlatmapIpcListenerAdapter;
import metreos.core.ipc.flatmaps.FlatmapIpcMessage;
import metreos.core.ipc.flatmaps.PrettyPrinter;
import metreos.core.net.ipc.IpcConnection;
import metreos.service.jtapi.Codec;
import metreos.service.jtapi.DeviceType;
import metreos.service.jtapi.MessagePrettyPrinter;
import metreos.service.jtapi.MsgField;
import metreos.service.jtapi.MsgType;
import metreos.util.AlarmManager;
import metreos.util.DefaultTrace;
import metreos.util.Trace;

/**
 * Test the JTapiServer
 */
public class testJTapiServer implements FlatmapIpcListener<IpcConnection>
{
	private static final String MANAGER = "10.1.14.25";
	private static final String USERNAME = "metreos";
	private static final String PASSWORD = "metreos";
	
//	private static final String CTI_PORT_1 = "wert-41-01";
//	private static final String CTI_PORT_1_LINE_1 = "5901";
	
	private static final String ROUTE_POINT_1 = "JAVAMAN";
//	private static final String ROUTE_POINT_1_PATTERN = "57XX";

	/**
	 * The main program.
	 * @param args
	 * @throws Exception
	 */
	public static void main( String[] args )
		throws Exception
	{
		Trace.setTrace( new DefaultTrace().start().setMask( 1 ) );
		IpcConnection connection = new IpcConnection( "localhost", 9110 );
		PrettyPrinter printer = new MessagePrettyPrinter();
		
		AlarmManager am = new AlarmManager();
		am.start();
		
		// listen for responses...
		
		testJTapiServer client = new testJTapiServer( connection, printer, am );
		connection.addListener( new FlatmapIpcListenerAdapter<IpcConnection>( client, printer ) );

		Trace.report( "starting connection..." );
		connection.start();
		connection.waitStarted( 15*1000 );
		Trace.report( "started." );

		client.registerMediaCaps( Codec.G711u, 20 );
		client.registerMediaCaps( Codec.G711u, 30 );
		client.registerMediaCaps( Codec.G711a, 20 );
		client.registerMediaCaps( Codec.G711a, 30 );

//		client.register( DeviceType.CtiPort, CTI_PORT_1 );
		client.register( DeviceType.RoutePoint, ROUTE_POINT_1 );
		
		Thread.sleep( 5*24*60*60*1000 );

//		client.unregister( DeviceType.CtiPort, CTI_PORT_1 );
		client.unregister( DeviceType.RoutePoint, ROUTE_POINT_1 );

		client.unregisterMediaCaps( Codec.G711u, 20 );
		client.unregisterMediaCaps( Codec.G711u, 30 );
		client.unregisterMediaCaps( Codec.G711a, 20 );
		client.unregisterMediaCaps( Codec.G711a, 30 );
		
		Thread.sleep( 15*1000 );
		
		am.stop();
		
		Trace.report( "closing connection..." );
		connection.close();
		Trace.report( "closed." );
		
		Trace.shutdown();
	}

	/**
	 * @param connection
	 * @param printer
	 * @param am
	 */
	public testJTapiServer( IpcConnection connection, PrettyPrinter printer,
		AlarmManager am )
	{
		this.connection = connection;
		this.printer = printer;
		this.am = am;
	}
	
	/**
	 * Description of connection.
	 */
	public final IpcConnection connection;
	
	/**
	 * A message pretty printer.
	 */
	public final PrettyPrinter printer;

	/**
	 * Description of am.
	 */
	public final AlarmManager am;
	
	private FlatmapIpcMessage newMessage( int msgType )
	{
		return new FlatmapIpcMessage( msgType, printer, connection );
	}

	private void registerMediaCaps( int codec, int framesize )
	{
		reportAndSend( newMessage( MsgType.RegisterMediaCaps )
			.add( MsgField.Codec, codec )
			.add( MsgField.Framesize, framesize ) );
	}

	private void unregisterMediaCaps( int codec, int framesize )
	{
		reportAndSend( newMessage( MsgType.UnregisterMediaCaps )
			.add( MsgField.Codec, codec )
			.add( MsgField.Framesize, framesize ) );
	}
	
	private void register( int deviceType, String deviceName )
	{
		reportAndSend( newMessage( MsgType.Register )
			.add( MsgField.CtiManager, MANAGER )
			.add( MsgField.Username, USERNAME )
			.add( MsgField.Password, PASSWORD )
			.add( MsgField.DeviceType, deviceType )
			.add( MsgField.DeviceName, deviceName ) );
	}

	private void unregister( int deviceType, String deviceName )
	{
		reportAndSend( newMessage( MsgType.Unregister )
			.add( MsgField.DeviceType, deviceType )
			.add( MsgField.DeviceName, deviceName ) );
	}

	/**
	 * @param msg
	 */
	static void reportAndSend( FlatmapIpcMessage msg )
	{
		Trace.report( Trace.m( "sending message: " ).a( msg ) );
		msg.send();
	}

	/* (non-Javadoc)
	 * @see metreos.core.ipc.flatmaps.FlatmapIpcListener#startup(metreos.core.net.ipc.IpcConnection)
	 */
	public void startup( IpcConnection conn )
	{
		// ignore
	}

	/* (non-Javadoc)
	 * @see metreos.core.ipc.flatmaps.FlatmapIpcListener#received(metreos.core.net.ipc.IpcConnection, metreos.core.ipc.flatmaps.FlatmapIpcMessage)
	 */
	public void received( final IpcConnection conn, FlatmapIpcMessage message )
	{
		try
		{
			switch (message.messageType)
			{
				case MsgType.StatusUpdate:
					// ignore
					break;
				
				case MsgType.IncomingCall:
				{
					String callId = message.args.getString( MsgField.CallId );
					reportAndSend( newMessage( MsgType.AcceptCall )
						.add( MsgField.CallId, callId ) );
					break;
				}
				
				case MsgType.RingingCall:
				{
					String callId = message.args.getString( MsgField.CallId );
					reportAndSend( newMessage( MsgType.AnswerCall )
						.add( MsgField.CallId, callId )
						.add( MsgField.RxIP, "127.0.0.1" )
						.add( MsgField.RxPort, 11234 ) );
					break;
				}
				
				default:
					Trace.report( Trace.m( "received [ignored]: " ).a( message ) );
					break;
			}
		}
		catch ( Exception e )
		{
			Trace.report( e );
		}
	}

	/* (non-Javadoc)
	 * @see metreos.core.ipc.flatmaps.FlatmapIpcListener#shutdown(metreos.core.net.ipc.IpcConnection)
	 */
	public void shutdown( IpcConnection conn )
	{
		// ignore
	}

	/* (non-Javadoc)
	 * @see metreos.core.ipc.flatmaps.FlatmapIpcListener#exception(metreos.core.net.ipc.IpcConnection, java.lang.Exception)
	 */
	public void exception( IpcConnection conn, Exception e )
	{
		Trace.report( this, e );
	}
}
