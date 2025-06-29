/* $Id: testJTapiServer.java 32224 2007-05-16 21:26:24Z wert $
 *
 * Created by wert on Jan 31, 2005
 *
 * Copyright (c) 2005 Metreos, Inc. All rights reserved.
 */

package metreos.service.jtapi.test;

import metreos.cmd.CommandParser;
import metreos.cmd.IntRangeConstraint;
import metreos.cmd.Option;
import metreos.cmd.Parameter;
import metreos.cmd.Program;
import metreos.cmd.StringLengthConstraint;
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
import metreos.util.Trace;

/**
 * Test the JTapiServer
 */
public class testJTapiServer extends Program implements FlatmapIpcListener<IpcConnection>
{
	/**
	 * @param args
	 * @throws Exception
	 */
	public static void main( String[] args ) throws Exception
	{
		main( new testJTapiServer(), args );
	}
	
	@Override
	protected void defineOptionsAndParameters( CommandParser cp ) throws Exception
	{
		cp.defineIntegerOption( "-acceptMask", "num", "setAcceptMask",
			"Trace mask of events to log",
			Option.NONE, new Integer( 3 ), null );
		
		cp.defineStringOption( "-host", "name", "setHost",
			"jtapi service host name",
			Option.NONE, "localhost", new StringLengthConstraint( 1, 255 ) );
		
		cp.defineIntegerOption( "-port", "num", "setPort",
			"jtapi service port number",
			Option.NONE, new Integer( 9100 ), new IntRangeConstraint( 0, 65535 ) );
		
		cp.defineStringParameter( "script", "setScript",
			"jtapi service script",
			false, false, new StringLengthConstraint( 1, 255 ) );
	}
	
	/**
	 * @param cp
	 * @param option
	 * @param token
	 * @param value
	 */
	public void setAcceptMask( CommandParser cp, Option option, String token,
		Integer value )
	{
		acceptMask = value.intValue();
	}
	
	private int acceptMask;
	
	/**
	 * @param cp
	 * @param option
	 * @param token
	 * @param value
	 */
	public void setHost( CommandParser cp, Option option, String token,
		String value )
	{
		host = value;
	}
	
	private String host;
	
	/**
	 * @param cp
	 * @param option
	 * @param token
	 * @param value
	 */
	public void setPort( CommandParser cp, Option option, String token,
		Integer value )
	{
		port = value.intValue();
	}
	
	private int port;
	
	/**
	 * @param cp
	 * @param parameter
	 * @param value
//	 * @throws ParserConfigurationException 
	 */
	public void setScript( CommandParser cp, Parameter parameter,
		String value ) // throws ParserConfigurationException
	{
//		File f = new File( value );
//		DocumentBuilderFactory factory = DocumentBuilderFactory.newInstance();
//		DocumentBuilder docBuilder = factory.newDocumentBuilder();
//		try
//		{
//			script = docBuilder.parse( f );
//		}
//		catch ( SAXException e )
//		{
//			Trace.report( Trace.m( "caught exception while reading " ).a( f ), e );
//		}
//		catch ( IOException e )
//		{
//			Trace.report( Trace.m( "caught exception while reading " ).a( f ), e );
//		}
	}
	
//	private Document script;

	@Override
	protected void run() throws Exception
	{
		Trace.getTrace().setMask( acceptMask );
		am.start();
		
		try
		{
			try
			{
				Trace.report( "starting connection to "+host+":"+port+"..." );
				connection = new IpcConnection( host, port );
				connection.addListener( new FlatmapIpcListenerAdapter<IpcConnection>( this, printer ) );
				connection.start();
				connection.waitStarted( 15*1000 );
				Trace.report( "connection started." );
				runScript();
			}
			finally
			{
				Trace.report( "closing connection..." );
				if (connection != null)
					connection.close();
				Trace.report( "closed." );
			}
		}
		finally
		{
			am.stop();
			Trace.shutdown();
		}
	}
	
	private IpcConnection connection;
	
	private PrettyPrinter printer = new MessagePrettyPrinter();
	
	private AlarmManager am = new AlarmManager();
	
	private void runScript() throws Exception
	{
		setLogLevel( 0 );
		registerMediaCaps( Codec.G711u, 10 );
		setLogLevel( 1 );
		registerMediaCaps( Codec.G711u, 20 );
		setLogLevel( 2 );
		registerMediaCaps( Codec.G711u, 30 );
		setLogLevel( 3 );
		registerMediaCaps( Codec.G711u, 30 );
		
		register( MANAGER, USERNAME, PASSWORD, DeviceType.RoutePoint, ROUTE_POINT_1 );
		
		Thread.sleep( 300*1000 );
		
		unregister( DeviceType.RoutePoint, ROUTE_POINT_1 );
		unregisterMediaCaps( Codec.G711u, 20 );
	}
	
	private static final String MANAGER = "m-ccm-10-pub";
	private static final String USERNAME = "wert";
	private static final String PASSWORD = "metreos";
//	
	private static final String ROUTE_POINT_1 = "wert-rp1x";
//	private static final String ROUTE_POINT_1_PATTERN = "58XX";
//	
//	private static final String ROUTE_POINT_2 = "wert-rp2";
//	private static final String ROUTE_POINT_2_PATTERN = "59XX";

//	/**
//	 * The main program.
//	 * @param args
//	 * @throws Exception
//	 */
//	public static void snort( String[] args )
//		throws Exception
//	{
//		
//		// listen for responses...
//		
////		testJTapiServer client = new testJTapiServer( connection, printer, am );
//		
//
//
//		registerMediaCaps( Codec.G711u, 20 );
//		registerMediaCaps( Codec.G711u, 30 );
//		registerMediaCaps( Codec.G711a, 20 );
//		registerMediaCaps( Codec.G711a, 30 );
//
////		register( DeviceType.CtiPort, CTI_PORT_1 );
//		register( DeviceType.RoutePoint, ROUTE_POINT_1 );
//		
//		Thread.sleep( 5*24*60*60*1000 );
//
////		unregister( DeviceType.CtiPort, CTI_PORT_1 );
//		unregister( DeviceType.RoutePoint, ROUTE_POINT_1 );
//
//		unregisterMediaCaps( Codec.G711u, 20 );
//		unregisterMediaCaps( Codec.G711u, 30 );
//		unregisterMediaCaps( Codec.G711a, 20 );
//		unregisterMediaCaps( Codec.G711a, 30 );
//		
//		Thread.sleep( 15*1000 );
//	}
	
	private FlatmapIpcMessage newMessage( int msgType )
	{
		return new FlatmapIpcMessage( msgType, printer, connection );
	}
	
	private void setLogLevel( int level )
	{
		reportAndSend( newMessage( MsgType.SetLogLevel )
			.add( MsgField.Level, level ) );
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
	
	private void register( String ccm, String username, String password,
		int deviceType, String deviceName )
	{
		reportAndSend( newMessage( MsgType.Register )
			.add( MsgField.CtiManager, ccm )
			.add( MsgField.Username, username )
			.add( MsgField.Password, password )
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
			Trace.report( Trace.m( "received: " ).a( message ) );
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
						.add( MsgField.RxIp, "127.0.0.1" )
						.add( MsgField.RxPort, 11234 ) );
					break;
				}
				
				default:
//					Trace.report( Trace.m( "received [ignored]: " ).a( message ) );
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
