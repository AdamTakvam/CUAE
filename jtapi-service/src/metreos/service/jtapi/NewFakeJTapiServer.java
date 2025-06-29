/* $Id: NewFakeJTapiServer.java 8162 2005-07-27 22:14:16Z wert $
 * 
 * Created by wert on Jan 31, 2005
 * 
 * Copyright (c) 2005 Metreos, Inc. All rights reserved.
 */

package metreos.service.jtapi;

import java.io.BufferedReader;
import java.io.IOException;
import java.io.InputStreamReader;
import java.net.DatagramSocket;
import java.net.InetAddress;
import java.net.Socket;
import java.net.SocketException;
import java.net.UnknownHostException;
import java.text.DecimalFormat;
import java.util.ArrayList;
import java.util.Formatter;
import java.util.HashMap;
import java.util.Hashtable;
import java.util.Iterator;
import java.util.List;
import java.util.Map;
import java.util.Vector;

import metreos.core.ipc.flatmaps.FlatmapIpcListener;
import metreos.core.ipc.flatmaps.FlatmapIpcListenerAdapter;
import metreos.core.ipc.flatmaps.FlatmapIpcMessage;
import metreos.core.ipc.flatmaps.FlatmapList;
import metreos.core.ipc.flatmaps.PrettyPrinter;
import metreos.core.ipc.flatmaps.FlatmapList.MapEntry;
import metreos.core.net.ConnectionHandler;
import metreos.core.net.DefaultServerListener;
import metreos.core.net.PlainServerSocketFactory;
import metreos.core.net.Server;
import metreos.core.net.ServerSocketFactory;
import metreos.core.net.ipc.IpcConnection;
import metreos.core.net.ipc.IpcConnectionHandler;
import metreos.util.AlarmListener;
import metreos.util.AlarmManager;
import metreos.util.Assertion;
import metreos.util.ClassURL;
import metreos.util.Monitor;
import metreos.util.SimpleTodo;
import metreos.util.StringUtil;
import metreos.util.Timer;
import metreos.util.TodoManager;
import metreos.util.Trace;
import metreos.util.URL;

/**
 * A server which is an adapter between metreos ipc/flatmap messages and JTapi
 * interface.
 */
public class NewFakeJTapiServer implements FlatmapIpcListener<IpcConnection>, JTapiServerMBean
{
	private final static String LOCALHOST = "127.0.0.1";
	
	private final static int REPORTING_INTERVAL = 3;
	
	private final static int DEFAULT_RATE = 14400;
	
	private final static int DEFAULT_NCALLERS = 100;
	
	private final static int DEFAULT_TALKTIME = 5000;
	
	private final static int DEFAULT_TEST_TIME = 300;

	private static void printUsageAndDie()
	{
		System.err.println( "usage: NewFakeJTapiServer [port [rate [nCallers [talkTime [testTime [logUrl]]]]]]" );
		System.exit( 1 );
	}
	
	/**
	 * Main entry point for server.
	 * 
	 * @param args
	 * @throws Exception
	 */
	public static void main( String[] args ) throws Exception
	{
		int port = 9100;
		int index = 0;
		int count = args.length;
		
		if (index < count)
		{
			String s = args[index++];
			if (s.equals( "-help" ))
				printUsageAndDie();
			
			try
			{
				port = Integer.parseInt( s );
				if (port < 1 || port > 65535)
				{
					System.err.println(
						"port must be a valid integer between 1 and 65535" );
					System.exit( 1 );
				}
			}
			catch ( NumberFormatException e )
			{
				System.err.println(
					"port must be a valid integer between 1 and 65535" );
				System.exit( 1 );
			}
		}

		final int rate;
		if (index < count)
			rate = Integer.parseInt( args[index++] );
		else
			rate = DEFAULT_RATE;
		
		final int nCallers;
		if (index < count)
			nCallers = Integer.parseInt( args[index++] );
		else
			nCallers = DEFAULT_NCALLERS;
		
		final int talkTime;
		if (index < count)
			talkTime = Integer.parseInt( args[index++] );
		else
			talkTime = DEFAULT_TALKTIME;
		
		final int testTime;
		if (index < count)
		{
			String s = args[index++];
			
			int mult;
			if (s.endsWith( "m" )) { mult = 60; s = s.substring( 0, s.length()-1 ); }
			else if (s.endsWith( "h" )) { mult = 60*60; s = s.substring( 0, s.length()-1 ); }
			else if (s.endsWith( "d" )) { mult = 24*60*60; s = s.substring( 0, s.length()-1 ); }
			else { mult = 1; }
			
			testTime = Integer.parseInt( s ) * mult;
		}
		else
			testTime = DEFAULT_TEST_TIME;
		
		// for dev mode add '?maxLen=0' (disables queuing)
		URL logUrl = new URL( "class:metreos.service.jtapi.MyDefaultTrace" );
		if (index < count)
		{
			try
			{
				logUrl = new URL( args[index++] );
			}
			catch ( RuntimeException e )
			{
				System.err.println( "logUrl not valid" );
				System.exit( 1 );
			}
			
			if (!logUrl.getScheme().equals( "class" ))
			{
				System.err.println( "logUrl must have 'class' scheme" );
				System.exit( 1 );
			}
		}
		
		if (index < count)
			printUsageAndDie();

		Trace trace = (Trace) ClassURL.makeInstance( Trace.class, logUrl );
		trace.start();
		Trace.setTrace( trace );
		
		Trace.report( Trace.m( "Fake JTapi Provider" ) );

		ServerSocketFactory ssf = new PlainServerSocketFactory();

		final PrettyPrinter printer = new MessagePrettyPrinter();
		
		DatagramSocket fakeSocket = null;
		
		InetAddress lh = InetAddress.getLocalHost();
		
		for (int i = 4800; i < 5000; i += 2)
		{
			try
			{
				fakeSocket = new DatagramSocket( i, lh );
				break;
			}
			catch ( SocketException e )
			{
				// ignore
			}
		}
		
		Assertion.check( fakeSocket != null, "fakeSocket != null" );
		
		final InetAddress fakeIp = fakeSocket.getLocalAddress();
		
		final int fakePort = fakeSocket.getLocalPort();
		
		Trace.report( Trace.m( "fake media destination: " ).a( fakeIp ).a( "/" ).a( fakePort ) );
		
		ConnectionHandler ch = new IpcConnectionHandler<IpcConnection>()
		{
			@Override
			protected void addListeners( IpcConnection connection )
			{
				connection.addListener( new FlatmapIpcListenerAdapter<IpcConnection>(
					new NewFakeJTapiServer( null, connection, printer, fakeIp,
						fakePort, rate, nCallers, talkTime, testTime ),
						printer ) );
			}

			@Override
			protected IpcConnection newIpcConnection( Server server, Socket socket )
				throws IOException
			{
				return new IpcConnection( server, socket );
			}
		};

		InetAddress intf = InetAddress.getByName( LOCALHOST );
		
		Server server = new Server( ssf, ch, intf, port, null );
		server.addListener( new DefaultServerListener()
		{
			@Override
			public void exception( Server server1, String what, Exception e )
			{
				Trace.report( server1, Trace.m( what ), e );
			}
		} );
		server.start();
		server.waitStarted( 15 * 1000 );

		Trace.report( server, Trace.m( "server started on port " ).a( server.getPort() ) );
		
		TodoManager oldTodoManager = TodoManager.setTodoManager( new TodoManager( 20 ).start() );
		Assertion.check( oldTodoManager == null, "oldTodoManager == null" );
		
		BufferedReader rdr = new BufferedReader( new InputStreamReader( System.in ) );
		while (true)
		{
			System.out.print( "hit q to stop: " );
			System.out.flush();
			String s = rdr.readLine();
			if (s.equals( "q" ))
			{
				System.out.println( "STOPPING..." );
				running = false;
				break;
			}
		}
	}
	
	/**
	 * Description of running.
	 */
	static boolean running = true;

	/**
	 * Constructs a new NewFakeJTapiServer.
	 * 
	 * @param factory
	 * @param ipcConnection
	 * @param printer
	 * @param fakeIp 
	 * @param fakePort 
	 * @param gRate call rate to maintain
	 * @param gNCallers number of callers to use
	 * @param gTalkTime length of talk time in millis
	 * @param gTestTime total length of test in seconds
	 */
	public NewFakeJTapiServer( ProviderFactory factory, IpcConnection ipcConnection,
		PrettyPrinter printer, InetAddress fakeIp, int fakePort, int gRate,
		int gNCallers, int gTalkTime, int gTestTime )
	{
		this.ipcConnection = ipcConnection;
		this.name = ipcConnection.getName();
		this.printer = printer;
		this.fakeIp = fakeIp;
		this.fakePort = fakePort;
		this.gRate = gRate;
		this.gNCallers = gNCallers;
		this.gTalkTime = gTalkTime;
		this.gTestTime = gTestTime;
	}

	/**
	 * The connection of this server.
	 */
	public final IpcConnection ipcConnection;

	@SuppressWarnings("unused")
	private final String name;

	/**
	 * Message pretty printer.
	 */
	public final PrettyPrinter printer;
	
	/**
	 * Description of fakeIp.
	 */
	final InetAddress fakeIp;
	
	/**
	 * Description of fakePort.
	 */
	final int fakePort;
	
	private final int gRate;
	
	private final int gNCallers;
	
	private final int gTalkTime;
	
	private final int gTestTime;
	
	/**
	 * Description of am.
	 */
	final AlarmManager am = new AlarmManager();
	{
		am.start();
	}
	
	@Override
	public String toString()
	{
		return "fjs";
	}

	//////////
	// misc //
	//////////

	/**
	 * Constructs a new message with the specified type and default printer.
	 * 
	 * @param messageType the message type of the new message.
	 * 
	 * @return a new message with the specified type and default printer.
	 *  
	 */
	public FlatmapIpcMessage newMessage( int messageType )
	{
		return new FlatmapIpcMessage( messageType, printer, ipcConnection );
	}

	/**
	 * Constructs a new error message with the specified fail reason.
	 * 
	 * @param failReason the fail reason of the new message.
	 * 
	 * @param messageType the message type that caused the error.
	 * 
	 * @return a new error message with the specified fail reason.
	 */
	public FlatmapIpcMessage newErrorMessage( int failReason, int messageType )
	{
		return newMessage( MsgType.Error )
			.add( MsgField.FailReason, failReason )
			.add( MsgField.MessageType, messageType );
	}
	
	/**
	 * @param messageType
	 * @param messageField
	 * @return a new missing field error message with the messageType
	 * and messageField filled in.
	 */
	public FlatmapIpcMessage newMissingFieldMessage( int messageType,
		int messageField )
	{
		return newErrorMessage( FailReason.MissingField, messageType )
			.add( MsgField.MessageField, messageField );
	}
	
	/**
	 * @param m
	 */
	void report( Trace.M m )
	{
		Trace.report( this, m );
	}
	
	/**
	 * @param m
	 */
	void report( String m )
	{
		Trace.report( this, m );
	}
	
	/**
	 * @param t
	 */
	void report( Throwable t )
	{
		Trace.report( this, t );
	}
	
	/**
	 * Reports a message sent.
	 * @param msg
	 */
	void reportAndSend( FlatmapIpcMessage msg )
	{
		//Trace.report( this, Trace.m( "sending message: " ).a( msg ) );
		msg.send();
	}
	
	private void reportAndSend( FlatmapIpcMessage msg, Throwable t )
	{
		Trace.report( this, Trace.m( "sending message: " ).a( msg ), t );
		msg.send();
	}

	////////////////////////////////
	// FlatmapIpcListener methods //
	////////////////////////////////

	/*
	 * (non-Javadoc)
	 * 
	 * @see metreos.core.ipc.flatmaps.FlatmapIpcListener#startup(metreos.core.net.ipc.IpcConnection)
	 */
	public void startup( IpcConnection connection )
	{
		report( Trace.m( "startup..." ) );
		
		try
		{
			StringBuffer sb = new StringBuffer();
			sb.append( getClass().getName() );
			sb.append( ":remoteAddress=" );
			sb.append( connection.getRemoteAddress().getHostAddress() );
			sb.append( ",remotePort=" );
			sb.append( connection.getRemotePort() );
			sb.append( ",localAddress=" );
			sb.append( connection.getLocalAddress().getHostAddress() );
			sb.append( ",localPort=" );
			sb.append( connection.getLocalPort() );
//			objName = null; // new ObjectName( sb.toString() );
//			mbs.registerMBean( this, objName );
		}
		catch ( Exception e )
		{
			report( e );
		}
		report( Trace.m( "startup done." ) );
	}

	/*
	 * (non-Javadoc)
	 * 
	 * @see metreos.core.ipc.flatmaps.FlatmapIpcListener#received(metreos.core.net.ipc.IpcConnection,
	 * metreos.core.ipc.flatmaps.FlatmapIpcMessage)
	 */
	public void received( final IpcConnection connection, final FlatmapIpcMessage message )
	{
		//report( Trace.m( "received msg " ).a( message ) );
		TodoManager.getTodoManager().add( new SimpleTodo()
		{
			public void doit()
			{
				doReceived( connection, message );
			}
		} );
	}

	/**
	 * @param connection
	 * @param message
	 */
	void doReceived( IpcConnection connection, FlatmapIpcMessage message )
	{
		try
		{
			switch (message.messageType)
			{
				case MsgType.Register:
					registerDevice( connection, message.args );
					break;
	
				case MsgType.Unregister:
					unregisterDevice( connection, message.args );
					break;
	
				case MsgType.RegisterMediaCaps:
					registerMediaCaps( connection, message.args );
					break;
	
				case MsgType.UnregisterMediaCaps:
					unregisterMediaCaps( connection, message.args );
					break;
	
				case MsgType.MakeCall:
					makeCall( connection, message.args );
					break;
	
				case MsgType.SetCallId:
					setCallId( connection, message.args );
					break;
	
				case MsgType.AcceptCall:
					acceptCall( connection, message.args );
					break;
	
				case MsgType.RedirectCall:
					redirectCall( connection, message.args );
					break;
	
				case MsgType.RejectCall:
					rejectCall( connection, message.args );
					break;
	
				case MsgType.ConferenceCall:
					conferenceCall( connection, message.args );
					break;
	
				case MsgType.AnswerCall:
					answerCall( connection, message.args );
					break;
	
				case MsgType.TransferCall:
					transferCall( connection, message.args );
					break;
	
				case MsgType.HangupCall:
					hangupCall( connection, message.args );
					break;
				
				case MsgType.SendUserInput:
					sendUserInput( connection, message.args );
					break;
					
				case MsgType.SetMedia:
					setMedia( connection, message.args );
					break;
					
				case MsgType.UseMohMedia:
					useMohMedia( connection, message.args );
					break;
	
				default:
					reportAndSend( newErrorMessage( FailReason.UnknownMessageType, message.messageType ) );
					break;
			}
		}
		catch ( Exception e )
		{
			reportAndSend( newErrorMessage( FailReason.GeneralFailure, message.messageType )
				.add( MsgField.Message, e.toString() )
				.add( MsgField.Args, message.args ), e );
		}
	}

	/*
	 * (non-Javadoc)
	 * 
	 * @see metreos.core.ipc.flatmaps.FlatmapIpcListener#shutdown(metreos.core.net.ipc.IpcConnection)
	 */
	public void shutdown( IpcConnection connection )
	{
		report( Trace.m( "shutdown..." ) );
		report( Trace.m( "shutdown done." ) );
	}

	/*
	 * (non-Javadoc)
	 * 
	 * @see metreos.core.ipc.flatmaps.FlatmapIpcListener#exception(metreos.core.net.ipc.IpcConnection,
	 * java.lang.Exception)
	 */
	public void exception( IpcConnection connection, Exception e )
	{
		report( e );
	}

	/////////////////
	// DEVICE MAPS //
	/////////////////

	/**
	 * Gets the device map for a given device type.
	 * 
	 * @param deviceType
	 * 
	 * @return a type-specific map of devices.
	 */
	private Map<String,Boolean> getDeviceMap( int deviceType )
	{
		if (deviceType == DeviceType.RoutePoint)
			return routePointMap;
		if (deviceType == DeviceType.CtiPort)
			return ctiPortMap;
		return null;
	}
	
	private Map<String,Boolean> routePointMap = new HashMap<String,Boolean>();
	
	private Map<String,Boolean> ctiPortMap = new HashMap<String,Boolean>();

	/////////////
	// DEVICES //
	/////////////

	/**
	 * Registers some devices in the appropriate device map.
	 * 
	 * @param connection
	 * @param args
	 */
	private void registerDevice( IpcConnection connection, FlatmapList args )
	{
		Iterator<MapEntry> i;
		
		List<String> managers = new Vector<String>();
		i = args.getValues( MsgField.CtiManager );
		while (i.hasNext())
			managers.add( i.next().stringValue() );
		if (managers.size() == 0)
		{
			reportAndSend( newMissingFieldMessage( MsgType.Register, MsgField.CtiManager ) );
			return;
		}
		
		String username = args.getString( MsgField.Username );
		if (username == null || username.length() == 0)
		{
			reportAndSend( newMissingFieldMessage( MsgType.Register, MsgField.Username ) );
			return;
		}
		
		String password = args.getString( MsgField.Password );
		if (password == null || password.length() == 0)
		{
			reportAndSend( newMissingFieldMessage( MsgType.Register, MsgField.Password ) );
			return;
		}
		
		Integer iDeviceType = args.getInteger( MsgField.DeviceType );
		if (iDeviceType == null)
		{
			reportAndSend( newMissingFieldMessage( MsgType.Register, MsgField.DeviceType ) );
			return;
		}
		
		int deviceType = iDeviceType.intValue();
		Map<String,Boolean> deviceMap = getDeviceMap( deviceType );
		if (deviceMap == null)
		{
			reportAndSend( newErrorMessage( FailReason.InvalidDeviceType, MsgType.Register )
					.add( MsgField.DeviceType, deviceType ) );
			return;
		}

		i = args.getValues( MsgField.DeviceName );
		while (i.hasNext())
		{
			String deviceName = i.next().stringValue();
			Boolean ok = deviceMap.put( deviceName, true );
			if (ok == null)
			{
				sendStatusUpdate( deviceType, deviceName, Status.DeviceOnline );
				startCalling( deviceType, deviceName,
					new CallStats( gTestTime, gRate, gNCallers, gTalkTime ) );
			}
		}
	}

	/**
	 * Unregisters some devices.
	 * 
	 * @param connection
	 * @param args
	 */
	private void unregisterDevice( IpcConnection connection, FlatmapList args )
	{
		Integer iDeviceType = args.getInteger( MsgField.DeviceType );
		if (iDeviceType == null)
		{
			reportAndSend( newMissingFieldMessage( MsgType.Unregister, MsgField.DeviceType ) );
			return;
		}
		
		int deviceType = iDeviceType.intValue();
		Map<String,Boolean> deviceMap = getDeviceMap( deviceType );
		if (deviceMap == null)
		{
			reportAndSend( newErrorMessage( FailReason.InvalidDeviceType, MsgType.Unregister )
					.add( MsgField.DeviceType, deviceType ) );
			return;
		}

		Iterator<MapEntry> i = args.getValues( MsgField.DeviceName );
		while (i.hasNext())
		{
			String deviceName = i.next().stringValue();
			boolean ok = deviceMap.remove( deviceName );
			if (ok)
				sendStatusUpdate( deviceType, deviceName, Status.DeviceOffline );
		}
	}

	private void sendStatusUpdate( int deviceType, String deviceName, int status )
	{
		reportAndSend( newMessage( MsgType.StatusUpdate )
			.add( MsgField.DeviceType, deviceType )
			.add( MsgField.DeviceName, deviceName )
			.add( MsgField.Status, status ) );
	}

	////////////////
	// MEDIA CAPS //
	////////////////

	/**
	 * @param connection
	 * @param args
	 */
	private void registerMediaCaps( IpcConnection connection, FlatmapList args )
	{
		int codec = args.getInteger( MsgField.Codec ).intValue();

		Iterator<MapEntry> i = args.getValues( MsgField.Framesize );
		while (i.hasNext())
		{
			int framesize = i.next().integerValue().intValue();
			
			Object mc = getMediaCaps( codec, framesize );
			if (mc == null)
			{
				reportAndSend( newErrorMessage( FailReason.CodecNotSupported, MsgType.RegisterMediaCaps )
						.add( MsgField.Codec, codec )
						.add( MsgField.Framesize, framesize ) );
				return;
			}
		}
	}

	/**
	 * @param connection
	 * @param args
	 */
	private void unregisterMediaCaps( IpcConnection connection, FlatmapList args )
	{
		int codec = args.getInteger( MsgField.Codec ).intValue();

		Iterator<MapEntry> i = args.getValues( MsgField.Framesize );
		while (i.hasNext())
		{
			int framesize = i.next().integerValue().intValue();
			
			Object mc = getMediaCaps( codec, framesize );
			if (mc == null)
			{
				reportAndSend( newErrorMessage( FailReason.CodecNotSupported, MsgType.UnregisterMediaCaps )
						.add( MsgField.Codec, codec )
						.add( MsgField.Framesize, framesize ) );
				return;
			}
		}
	}

	/**
	 * @param codec
	 * @param framesize
	 * @return the cisco codec value
	 */
	public static Object getMediaCaps( int codec, int framesize )
	{
		if (codec == Codec.G711u)
		{
			if (framesize == 20)
				return Boolean.TRUE;
			else if (framesize == 30)
				return Boolean.TRUE;
		}
		else if (codec == Codec.G711a)
		{
			if (framesize == 20)
				return Boolean.TRUE;
			else if (framesize == 30)
				return Boolean.TRUE;
		}
		return null;
	}

	//////////////////
	// CALL CONTROL //
	//////////////////

	/**
	 * @param connection
	 * @param args
	 * @throws Exception
	 */
	private void makeCall( IpcConnection connection, FlatmapList args )
		throws Exception
	{
		String fromCallId = args.getString( MsgField.FromCallId );

		String toCallId = args.getString( MsgField.ToCallId );
		
		Integer iDeviceType = args.getInteger( MsgField.DeviceType );
		if (iDeviceType == null)
		{
			reportAndSend( newMissingFieldMessage( MsgType.MakeCall, MsgField.DeviceType )
					.add( MsgField.FromCallId, fromCallId )
					.add( MsgField.ToCallId, toCallId ) );
			return;
		}
		
		int deviceType = iDeviceType.intValue();
		Map<String,Boolean> deviceMap = getDeviceMap( deviceType );
		if (deviceMap == null)
		{
			reportAndSend( newErrorMessage( FailReason.InvalidDeviceType, MsgType.MakeCall )
					.add( MsgField.FromCallId, fromCallId )
					.add( MsgField.ToCallId, toCallId )
					.add( MsgField.DeviceType, deviceType ) );
			return;
		}
		
		String deviceName = args.getString( MsgField.DeviceName );
		if (deviceName == null)
		{
			reportAndSend( newMissingFieldMessage( MsgType.MakeCall, MsgField.DeviceName )
					.add( MsgField.FromCallId, fromCallId )
					.add( MsgField.ToCallId, toCallId ) );
			return;
		}
		
		report( Trace.m( "MakeCall deviceName = "+deviceName ) );
		Boolean device = deviceMap.get( deviceName );
		if (device == null)
		{
			reportAndSend( newErrorMessage( FailReason.InvalidDeviceName, MsgType.MakeCall )
					.add( MsgField.FromCallId, fromCallId )
					.add( MsgField.ToCallId, toCallId )
					.add( MsgField.DeviceType, deviceType )
					.add( MsgField.DeviceName, deviceName ) );
			return;
		}
		
		if (!device)
		{
			reportAndSend( newErrorMessage( FailReason.NoProvider, MsgType.MakeCall )
				.add( MsgField.FromCallId, fromCallId )
				.add( MsgField.ToCallId, toCallId )
				.add( MsgField.DeviceType, deviceType )
				.add( MsgField.DeviceName, deviceName ) );
		return;
		}
		
		String from = args.getString( MsgField.From );
//		if (from == null)
//		{
//			report( newMissingFieldMessage( MsgType.MakeCall, MsgField.From )
//					.add( MsgField.FromCallId, fromCallId )
//					.add( MsgField.ToCallId, toCallId )
// );
//			return;
//		}
		
		String fromAddress = "12345";
		if (fromAddress == null)
		{
			reportAndSend( newErrorMessage( FailReason.InvalidDn, MsgType.MakeCall )
					.add( MsgField.FromCallId, fromCallId )
					.add( MsgField.ToCallId, toCallId )
					.add( MsgField.From, from ) );
			return;
		}
		
		String to = args.getString( MsgField.To );
		if (to == null)
		{
			reportAndSend( newMissingFieldMessage( MsgType.MakeCall, MsgField.To )
					.add( MsgField.FromCallId, fromCallId )
					.add( MsgField.ToCallId, toCallId ) );
			return;
		}
		
		str2inetAddress( args.getString( MsgField.RxIp ) );
		args.getInt( MsgField.RxPort, 0 );
		
		reportAndSend( newMessage( MsgType.MakeCallAccepted )
				.add( MsgField.FromCallId, fromCallId )
				.add( MsgField.ToCallId, toCallId )
				.add( MsgField.DeviceType, deviceType )
				.add( MsgField.DeviceName, deviceName )
				.add( MsgField.From, from )
				.add( MsgField.To, to ) );
	}

	/**
	 * @param connection
	 * @param args
	 */
	private void setCallId( IpcConnection connection, FlatmapList args )
	{
		String callId = args.getString( MsgField.CallId );
		if (callId == null)
		{
			reportAndSend( newMissingFieldMessage( MsgType.SetCallId, MsgField.CallId ) );
			return;
		}
		
		String newCallId = args.getString( MsgField.NewCallId );
		if (newCallId == null)
		{
			reportAndSend( newMissingFieldMessage( MsgType.SetCallId, MsgField.CallId ) );
			return;
		}
		
		if (cmMapRename( callId, newCallId ) == null)
		{
			reportAndSend( newErrorMessage( FailReason.CallIdUnknown, MsgType.SetCallId )
					.add( MsgField.CallId, callId )
					.add( MsgField.NewCallId, newCallId ) );
			return;
		}
		
		reportAndSend( newMessage( MsgType.SetCallIdOk )
			.add( MsgField.CallId, callId )
			.add( MsgField.NewCallId, newCallId ) );
	}

	/**
	 * @param connection
	 * @param args
	 */
	private void acceptCall( IpcConnection connection, FlatmapList args )
		throws Exception
	{
		String callId = args.getString( MsgField.CallId );
		if (callId == null)
		{
			reportAndSend( newMissingFieldMessage( MsgType.AcceptCall, MsgField.CallId ) );
			return;
		}
		
		String newCallId = args.getString( MsgField.NewCallId );

		FakeCall cm;
		if (newCallId != null)
		{
			cm = cmMapRename( callId, newCallId );
			if (cm == null)
			{
				reportAndSend( newErrorMessage( FailReason.CallIdUnknown, MsgType.AcceptCall )
						.add( MsgField.CallId, callId )
						.add( MsgField.NewCallId, newCallId ) );
				return;
			}
		}
		else
		{
			cm = cmMapGet( callId );
			if (cm == null)
			{
				reportAndSend( newErrorMessage( FailReason.CallIdUnknown, MsgType.AcceptCall )
						.add( MsgField.CallId, callId ) );
				return;
			}
		}
		cm.accept();
	}

	/**
	 * @param connection
	 * @param args
	 */
	private void redirectCall( IpcConnection connection, FlatmapList args )
	throws Exception
	{
		String callId = args.getString( MsgField.CallId );
		if (callId == null)
		{
			reportAndSend( newMissingFieldMessage( MsgType.RedirectCall, MsgField.CallId ) );
			return;
		}
		
		FakeCall cm = cmMapGet( callId );
		if (cm == null)
		{
			reportAndSend( newErrorMessage( FailReason.CallIdUnknown, MsgType.RedirectCall )
					.add( MsgField.CallId, callId ) );
			return;
		}
		
		String to = args.getString( MsgField.To );
		if (to == null)
		{
			reportAndSend( newMissingFieldMessage( MsgType.RedirectCall, MsgField.To ) );
			return;
		}

		cm.redirect( to );
	}

	/**
	 * @param connection
	 * @param args
	 */
	private void rejectCall( IpcConnection connection, FlatmapList args )
		throws Exception
	{
		String callId = args.getString( MsgField.CallId );
		if (callId == null)
		{
			reportAndSend( newMissingFieldMessage( MsgType.RejectCall, MsgField.CallId ) );
			return;
		}
		
		FakeCall cm = cmMapGet( callId );
		if (cm == null)
		{
			reportAndSend( newErrorMessage( FailReason.CallIdUnknown, MsgType.RejectCall )
					.add( MsgField.CallId, callId ) );
			return;
		}

		cm.reject();
	}

	private void conferenceCall( IpcConnection connection, FlatmapList args )
		throws Exception
	{
		String callId = args.getString( MsgField.CallId );
		if (callId == null)
		{
			reportAndSend( newMissingFieldMessage( MsgType.ConferenceCall, MsgField.CallId ) );
			return;
		}
		
		FakeCall cm = cmMapGet( callId );
		if (cm == null)
		{
			reportAndSend( newErrorMessage( FailReason.CallIdUnknown, MsgType.ConferenceCall )
					.add( MsgField.CallId, callId ) );
			return;
		}

		String otherCallId = args.getString( MsgField.OtherCallId );
		if (otherCallId == null)
		{
			reportAndSend( newMissingFieldMessage( MsgType.ConferenceCall, MsgField.OtherCallId ) );
			return;
		}
		
		FakeCall otherCm = cmMapGet( otherCallId );
		if (otherCm == null)
		{
			reportAndSend( newErrorMessage( FailReason.CallIdUnknown, MsgType.ConferenceCall )
					.add( MsgField.OtherCallId, otherCallId ) );
			return;
		}

		cm.conference( otherCm );
	}

	/**
	 * @param connection
	 * @param args
	 */
	private void answerCall( IpcConnection connection, FlatmapList args )
		throws Exception
	{
		String callId = args.getString( MsgField.CallId );
		if (callId == null)
		{
			reportAndSend( newMissingFieldMessage( MsgType.AnswerCall, MsgField.CallId ) );
			return;
		}
		
		FakeCall cm = cmMapGet( callId );
		if (cm == null)
		{
			reportAndSend( newErrorMessage( FailReason.CallIdUnknown, MsgType.AnswerCall )
					.add( MsgField.CallId, callId ) );
			return;
		}
		
		InetAddress rxIp = str2inetAddress( args.getString( MsgField.RxIp ) );
		int rxPort = args.getInt( MsgField.RxPort, 0 );

		cm.answer( rxIp, rxPort );
	}

	private InetAddress str2inetAddress( String s ) throws UnknownHostException
	{
		if (s == null || s.length() == 0)
			return null;
		
		return InetAddress.getByName( s );
	}

	/**
	 * @param connection
	 * @param args
	 */
	private void transferCall( IpcConnection connection, FlatmapList args )
		throws Exception
	{
		String callId = args.getString( MsgField.CallId );
		if (callId == null)
		{
			reportAndSend( newMissingFieldMessage( MsgType.TransferCall, MsgField.CallId ) );
			return;
		}
		
		FakeCall cm = cmMapGet( callId );
		if (cm == null)
		{
			reportAndSend( newErrorMessage( FailReason.CallIdUnknown, MsgType.TransferCall )
					.add( MsgField.CallId, callId ) );
			return;
		}
		
		String to = args.getString( MsgField.To );
		if (to == null)
		{
			reportAndSend( newMissingFieldMessage( MsgType.TransferCall, MsgField.To ) );
			return;
		}

		cm.transfer( to );
	}

	/**
	 * @param connection
	 * @param args
	 */
	private void hangupCall( IpcConnection connection, FlatmapList args )
		throws Exception
	{
		String callId = args.getString( MsgField.CallId );
		if (callId == null)
		{
			reportAndSend( newMissingFieldMessage( MsgType.HangupCall, MsgField.CallId ) );
			return;
		}
		
		FakeCall cm = cmMapGet( callId );
		if (cm == null)
		{
			reportAndSend( newErrorMessage( FailReason.CallIdUnknown, MsgType.HangupCall )
					.add( MsgField.CallId, callId ) );
			return;
		}

		cm.hangup();
	}

	/**
	 * @param connection
	 * @param args
	 * @throws Exception
	 */
	private void sendUserInput( IpcConnection connection, FlatmapList args )
		throws Exception
	{
		String callId = args.getString( MsgField.CallId );
		if (callId == null)
		{
			reportAndSend( newMissingFieldMessage( MsgType.SendUserInput, MsgField.CallId ) );
			return;
		}
		
		String digits = args.getString( MsgField.Digits );
		if (digits == null || digits.length() == 0)
		{
			reportAndSend( newMissingFieldMessage( MsgType.SendUserInput, MsgField.Digits ) );
			return;
		}
		
		FakeCall cm = cmMapGet( callId );
		if (cm == null)
		{
			reportAndSend( newErrorMessage( FailReason.CallIdUnknown, MsgType.SendUserInput )
					.add( MsgField.CallId, callId ) );
			return;
		}

		cm.sendUserInput( digits );
	}

	/**
	 * @param connection
	 * @param args
	 * @throws Exception
	 */
	private void setMedia( IpcConnection connection, FlatmapList args )
		throws Exception
	{
		String callId = args.getString( MsgField.CallId );
		if (callId == null)
		{
			reportAndSend( newMissingFieldMessage( MsgType.SetMedia, MsgField.CallId ) );
			return;
		}
		
		InetAddress rxIp = str2inetAddress( args.getString( MsgField.RxIp ) );
		int rxPort = args.getInt( MsgField.RxPort, 0 );
		
		//Trace.report( this, "setMedia: rxIp = "+rxIp+", rxPort = "+rxPort );
		
		FakeCall cm = cmMapGet( callId );
		if (cm == null)
		{
			reportAndSend( newErrorMessage( FailReason.CallIdUnknown, MsgType.SetMedia )
					.add( MsgField.CallId, callId ) );
			return;
		}

		cm.setMedia( rxIp, rxPort );
	}
	
	private void useMohMedia( IpcConnection connection, FlatmapList args )
	{
		String callId = args.getString( MsgField.CallId );
		if (callId == null)
		{
			reportAndSend( newMissingFieldMessage( MsgType.UseMohMedia, MsgField.CallId ) );
			return;
		}

		FakeCall cm = cmMapGet( callId );
		if (cm == null)
		{
			reportAndSend( newErrorMessage( FailReason.CallIdUnknown, MsgType.UseMohMedia )
					.add( MsgField.CallId, callId ) );
			return;
		}

		cm.useMohMedia();
	}

	//////////////
	// Call Map //
	//////////////
	
	private FakeCall cmMapGet( String callId )
	{
		return cmMap.get( callId );
	}

	/**
	 * @param fc
	 * @return true if it worked, false if there was already a call with that id.
	 */
	boolean cmMapPut( FakeCall fc )
	{
		synchronized (cmMap)
		{
			if (cmMap.containsKey( fc.id ))
				return false;
			
			cmMap.put( fc.id, fc );
			return true;
		}
	}

	/**
	 * @param fc
	 */
	void cmMapRemove( FakeCall fc )
	{
		cmMap.remove( fc.id );
	}
	
	private FakeCall cmMapRename( String oldCallId, String newCallId )
	{
		synchronized (cmMap)
		{
			FakeCall fc = cmMap.remove( oldCallId );
			if (fc != null)
			{
				fc.id = newCallId;
				cmMap.put( fc.id, fc );
			}
			return fc;
		}
	}
	
	private Map<String, FakeCall> cmMap = new Hashtable<String, FakeCall>();

	/////////
	// ETC //
	/////////
	
	private void startCalling( final int deviceType, final String deviceName,
		final CallStats stats )
	{
		new Thread()
		{
			@Override
			public void run()
			{
				try
				{
					DecimalFormat fromDf = new DecimalFormat( stats.fromPattern );
					DecimalFormat toDf = new DecimalFormat( stats.toPattern );
					
					List<Caller> callers = new ArrayList<Caller>();
					for (int i = 0; i < stats.nCallers; i++)
					{
						String from = fromDf.format( stats.fromBase+i );
						String to = toDf.format( stats.toBase+i );
						callers.add( new Caller( deviceType, deviceName,
							from, to, to, stats ) );
					}
					
					Thread.sleep( 5000 );
					
					stats.start();
					
					for (Caller caller: callers)
						caller.start();
					
					for (Caller caller: callers)
						caller.join();
					
					stats.stop();
					
					Thread.sleep( 5000 );
					
					System.exit( 0 );
				}
				catch ( InterruptedException e )
				{
					e.printStackTrace();
				}
			}
		}.start();
	}
	
	private class Caller extends Thread
	{
		/**
		 * Constructs the Caller.
		 *
		 * @param deviceType
		 * @param deviceName
		 * @param from
		 * @param to
		 * @param originalTo
		 * @param stats
		 */
		public Caller( int deviceType, String deviceName, String from,
			String to, String originalTo, CallStats stats )
		{
			super();
			this.deviceType = deviceType;
			this.deviceName = deviceName;
			this.from = from;
			this.to = to;
			this.originalTo = originalTo;
			this.stats = stats;
		}
		
		private final int deviceType;
		
		private final String deviceName;
		
		private final String from;
		
		private final String to;
		
		private final String originalTo;
		
		private final CallStats stats;
		
		@Override
		public void run()
		{
			boolean firstTime = true;
			CallInfo ci;
			while ((ci = stats.nextCallInfo( firstTime )) != null)
			{
				firstTime = false;
				FakeCall fc = null;
				try
				{
					Thread.sleep( ci.onHookTime );
					
					fc = new FakeCall( deviceType, deviceName,
						newId(), from, to, originalTo, ci.talkTime );
					
					while (!cmMapPut( fc ))
						fc.id = newId();
					
					stats.starting();
					fc.sendIncomingCall();
					
					boolean ok = fc.waitCallComplete( ci.timeout );
					if (ok)
						stats.completed( fc );
					else
						stats.failed( fc );
				}
				catch ( Exception e )
				{
					stats.failed( fc );
					report( e );
				}
				finally
				{
					cmMapRemove( fc );
				}
			}
		}
	}
	
	/**
	 * @return a new random call id
	 */
	String newId()
	{
		return StringUtil.rndString( 16 );
	}
	
	private class FakeCall implements AlarmListener
	{
		/**
		 * Constructs the FakeCall.
		 *
		 * @param deviceType
		 * @param deviceName
		 * @param id
		 * @param to
		 * @param from
		 * @param originalTo
		 * @param talkTime
		 */
		public FakeCall( int deviceType, String deviceName, String id,
			String to, String from, String originalTo, long talkTime )
		{
			this.deviceType = deviceType;
			this.deviceName = deviceName;
			this.id = id;
			this.to = to;
			this.from = from;
			this.originalTo = originalTo;
			this.talkTime = talkTime;
			monitor = new Monitor( "call monitor "+id );
			change( INIT, START );
		}
		
		@Override
		public String toString()
		{
			return id+", "+currentState;
		}
		
		private final int deviceType;
		
		private final String deviceName;

		/**
		 * Description of id.
		 */
		String id;
		
		private final String to;
		
		private final String from;
		
		private final String originalTo;

		private final long talkTime;
		
		/**
		 * Description of currentState.
		 */
		int currentState = INIT;
		
		/**
		 * Description of stateNanos.
		 */
		long[] stateNanos = new long[NSTATES];
		
		private final Monitor monitor;
		
		private static final int INIT = -1; // not a real state.
		private static final int START = 0;
		private static final int ACCEPT = 1;
		private static final int ANSWER = 2;
		private static final int STOP_MEDIA = 3;
		private static final int HANG_UP = 4;
		private static final int DONE = 5;
		private static final int NSTATES = 6;

		private synchronized void change( int oldState, int newState )
		{
			if (currentState != oldState)
				throw new UnsupportedOperationException(
					"change from "+oldState+" to "+newState+": current state is "+currentState );
			
			currentState = newState;
			stateNanos[newState] = Timer.getNanos();
			monitor.set( newState );
		}
		
		/**
		 * @param s1
		 * @param s2
		 * @return the time in millis of the specified state transition
		 */
		long delta( int s1, int s2 )
		{
			Assertion.check( s2 > s1, "s2 > s1" );
			Assertion.check( stateNanos[s1] != 0, "stateNanos[s1] != 0" );
			Assertion.check( stateNanos[s2] != 0, "stateNanos[s2] != 0" );
			return (stateNanos[s2] - stateNanos[s1])/Timer.NANOS_PER_MILLI;
		}
		
		/**
		 * @param maxDelay
		 * @return true if the call completed normally, false otherwise.
		 */
		public boolean waitCallComplete( long maxDelay )
		{
			try
			{
				monitor.waitUntilEq( DONE, maxDelay );
				return true;
				
			}
			catch ( InterruptedException e )
			{
				return false;
			}
		}
		
		/**
		 * Change media to fake music on hold.
		 */
		public void useMohMedia()
		{
			throw new UnsupportedOperationException( "useMohMedia not implemented" );
		}

		/**
		 * @param rxIp
		 * @param rxPort
		 */
		public void setMedia( InetAddress rxIp, int rxPort )
		{
			change( HANG_UP, DONE );
			reportAndSend( newMessage( MsgType.HangupCall )
				.add( MsgField.DeviceType, deviceType )
				.add( MsgField.DeviceName, deviceName )
				.add( MsgField.CallId, id )
				.add( MsgField.To, to )
				.add( MsgField.From, from )
				.add( MsgField.OriginalTo, originalTo )
				.add( MsgField.Cause, "NORMAL" )
				.add( MsgField.CallControlCause, "NORMAL" ) );
		}

		/**
		 * @param digits
		 */
		public void sendUserInput( String digits )
		{
			throw new UnsupportedOperationException( "sendUserInput not implemented" );
		}

		/**
		 * Hangup the call.
		 */
		public void hangup()
		{
			throw new UnsupportedOperationException( "hangup not implemented" );
		}

		/**
		 * Transfer the call.
		 * @param to1
		 */
		public void transfer( String to1 )
		{
			throw new UnsupportedOperationException( "transfer not implemented" );
		}

		/**
		 * Answer the call
		 * @param rxIp
		 * @param rxPort
		 */
		public void answer( InetAddress rxIp, int rxPort )
		{
			change( ANSWER, STOP_MEDIA );
			am.add( this, talkTime );
			
			reportAndSend( newMessage( MsgType.EstablishedCall )
				.add( MsgField.DeviceType, deviceType )
				.add( MsgField.DeviceName, deviceName )
				.add( MsgField.CallId, id )
				.add( MsgField.To, to )
				.add( MsgField.From, from )
				.add( MsgField.OriginalTo, originalTo )
				.add( MsgField.Cause, "normal" ) );
			
			reportAndSend( newMessage( MsgType.EstablishedMedia )
				.add( MsgField.DeviceType, deviceType )
				.add( MsgField.DeviceName, deviceName )
				.add( MsgField.CallId, id )
				.add( MsgField.To, to )
				.add( MsgField.From, from )
				.add( MsgField.OriginalTo, originalTo )
				.add( MsgField.TxIp, fakeIp.getHostAddress() )
				.add( MsgField.TxPort, fakePort )
				.add( MsgField.Codec, Codec.G711u )
				.add( MsgField.Framesize, 20 ) );
		}

		/**
		 * Join this call with another in a conference.
		 * @param otherCm
		 */
		public void conference( FakeCall otherCm )
		{
			throw new UnsupportedOperationException( "conference not implemented" );
		}

		/**
		 * Reject this call.
		 */
		public void reject()
		{
			throw new UnsupportedOperationException( "reject not implemented" );
		}

		/**
		 * Redirect this call.
		 * @param to1
		 */
		public void redirect( String to1 )
		{
			throw new UnsupportedOperationException( "redirect not implemented" );
		}

		/**
		 * Accept this call.
		 */
		public void accept()
		{
			change( ACCEPT, ANSWER );
			reportAndSend( newMessage( MsgType.RingingCall )
				.add( MsgField.DeviceType, deviceType )
				.add( MsgField.DeviceName, deviceName )
				.add( MsgField.CallId, id )
				.add( MsgField.To, to )
				.add( MsgField.From, from )
				.add( MsgField.OriginalTo, originalTo ) );
		}

		/**
		 * Report incoming call to the client.
		 */
		public void sendIncomingCall()
		{
			change( START, ACCEPT );
			reportAndSend( newMessage( MsgType.IncomingCall )
				.add( MsgField.DeviceType, deviceType )
				.add( MsgField.DeviceName, deviceName )
				.add( MsgField.CallId, id )
				.add( MsgField.To, to )
				.add( MsgField.From, from )
				.add( MsgField.OriginalTo, originalTo ) );
		}

		public int wakeup( AlarmManager manager, long due )
		{
			change( STOP_MEDIA, HANG_UP );
			reportAndSend( newMessage( MsgType.EstablishedMedia )
				.add( MsgField.DeviceType, deviceType )
				.add( MsgField.DeviceName, deviceName )
				.add( MsgField.CallId, id )
				.add( MsgField.To, to )
				.add( MsgField.From, from )
				.add( MsgField.OriginalTo, originalTo )
				.add( MsgField.TxIp, "" )
				.add( MsgField.TxPort, 0 )
				.add( MsgField.Codec, 0 )
				.add( MsgField.Framesize, 0 ) );
			return 0;
		}
	}
	
	/**
	 * Description of CallStats.
	 */
	private class CallStats
	{
		/**
		 * Constructs the CallStats.
		 *
		 * @param testTime
		 * @param rate
		 * @param nCallers
		 * @param talkTime
		 */
		public CallStats( int testTime, int rate, int nCallers, int talkTime )
		{
			if (testTime < 60) throw new IllegalArgumentException( "testTime < 60" );
			if (rate < 1) throw new IllegalArgumentException( "rate < 1" );
			if (nCallers < 1) throw new IllegalArgumentException( "nCallers < 1" );
			if (talkTime < 1) throw new IllegalArgumentException( "talkTime < 1" );
			
			this.testTime = testTime;
			this.rate = rate;
			this.nCallers = nCallers;
			this.talkTime = talkTime;
			
			report( "testTime = "+testTime );
			report( "rate = "+rate );
			report( "nCallers = "+nCallers );
			report( "talkTime = "+talkTime );

			//int callRate = rate;
			//int testTime = 24*60*60*1000;
			
			//int talkTime = 5000;
//			int onHookTime = 1000;
//			int callDuration = talkTime+onHookTime+200;
			
//			Trace.report( this, "estimated callDuration = "+callDuration );
			
//			int oneCallerRate = 3600000/callDuration;
			//int nCallers = callRate / oneCallerRate;
			
//			Trace.report( this, "nCallers = "+nCallers );
			
//			int nCalls = testTime / callDuration;
			
//			Trace.report( this, "nCalls = "+nCalls );
		}
		
		/**
		 * Count of calls that actually started (vs. those waiting to start)
		 */
		public synchronized void starting()
		{
			attempted++;
		}

		/**
		 * Length of test in seconds.
		 */
		public final int testTime;
		
		/**
		 * Description of rate.
		 */
		public final int rate;
		
		/**
		 * Description of nCallers.
		 */
		public final int nCallers;
		
		/**
		 * Description of talkTime.
		 */
		public final int talkTime;
		
		/**
		 * Description of toBase.
		 */
		public final int toBase = 3300;
		
		/**
		 * Description of toPattern.
		 */
		public final String toPattern = "0000";
		
		/**
		 * Description of fromBase.
		 */
		public final int fromBase = 5900;
		
		/**
		 * Description of fromPattern.
		 */
		public final String fromPattern = "0000";

		/**
		 * Reports that call activity is starting.
		 */
		public synchronized void start()
		{
			startTime = Timer.getNanos();
		}
		
		private long startTime = 0;

		/**
		 * @param firstTime true the first time a caller calls this method
		 * @return a CallInfo record for the next call, or null if we are done.
		 */
		public synchronized CallInfo nextCallInfo( boolean firstTime )
		{
			if (!running || realTime() >= testTime)
				return null;
			
			reportStats( false );
			
			// we calculate when a call should start based on the current
			// target bhca. if we are attempting 14,400 bhca, that's 4 calls
			// per second, or one call every 250 ms. thus call 1 should
			// start at virtual time 0, #2 at 250, #3 at 500, #4 at 750,
			// etc.
			
			double callStartTime = virtTime;
			
			double callInterval = getCallInterval( virtTime );
			
			double minDelay = 0.500; // 500 ms
			
			double delay = callStartTime - realTime();
			if (delay < minDelay)
			{
				// this means that we are falling behind. we need to
				// adjust callInterval smaller to speed things up.
				delay = minDelay;
				callInterval *= 0.9; // speed things up by 10%
			}
			
			virtTime += callInterval;
			
			int d = (int) (delay*1000);
			//report( "d = "+d );
			return new CallInfo( d, talkTime );
		}
		
		private double getCallInterval( double t )
		{
			return 3600.0 / getTargetBhca( t ); // in seconds.
		}

		@SuppressWarnings("unused")
		private int startToAcceptTime = 0;
		
		@SuppressWarnings("unused")
		private int acceptToAnswerTime = 100;
		
		@SuppressWarnings("unused")
		private int stopMediaToHangUpTime = 100;
		
		@SuppressWarnings("unused")
		private int hangUpToDoneTime = 0;
		
		private int calcBhca( int attempts, double period )
		{
			if (period < 1)
				period = 1;
			
			if (attempts == 0)
				return 0;
			
			return (int) (attempts * 3600L / period);
		}
		
		private int getTargetBhca( double t )
		{
			// start off at 3600 for 30 seconds, then
			// ramp up to rate during the next 30 seconds,
			// then stay at rate thereafter.
			
			final int baseRate = 3600;
			final double baseTime = 30;
			final double maxTime = 60;
			
			if (rate <= baseRate)
				return rate;
			
			if (t >= maxTime)
				return rate;
			
			if (t < baseTime)
				return baseRate;
			
			int incRate = rate - baseRate;
			double incTime = t - baseTime; // 0..(maxTime - baseTime)
			double mult = incTime / (maxTime - baseTime);
			Assertion.check( mult >= 0 && mult < 1.0, "mult >= 0 && mult < 1.0" );
			return (int)(baseRate + incRate * mult);
		}

		/**
		 * @param fc
		 */
		public synchronized void completed( FakeCall fc )
		{
			completed++;
			sumStartToAccept += fc.delta( FakeCall.START, FakeCall.ACCEPT );
			sumAcceptToAnswer += fc.delta( FakeCall.ACCEPT, FakeCall.ANSWER );
			sumAnswerToStopMedia += fc.delta( FakeCall.ANSWER, FakeCall.STOP_MEDIA );
			sumStopMediaToHangUp += fc.delta( FakeCall.STOP_MEDIA, FakeCall.HANG_UP );
			sumHangUpToDone += fc.delta( FakeCall.HANG_UP, FakeCall.DONE );
		}

		/**
		 * @param fc
		 */
		public synchronized void failed( FakeCall fc )
		{
			Trace.report( this, "call failed: "+fc );
			failed++;
		}

		/**
		 * Reports that all call activity is complete.
		 */
		public synchronized void stop()
		{
			reportStats( true );
		}
		
		private void reportStats( boolean done )
		{
			if (!done)
			{
				if (Timer.getSecondsSince( t0 ) < REPORTING_INTERVAL)
					return;
				
				t0 = Timer.getNanos();
			}
			
			double rt = realTime();
			String tstr = timeToString( rt );
			int bhca = calcBhca( attempted, rt );
			
			StringBuffer sb = new StringBuffer();
			new Formatter( sb ).format(
				"--- a/c/f %d/%d/%d time %s bhca %d target %d deltas %d/%d/%d/%d/%d%s",
				attempted, completed, failed, tstr, bhca, getTargetBhca( rt ),
				sdiv( sumStartToAccept, completed ),
				sdiv( sumAcceptToAnswer, completed ),
				sdiv( sumAnswerToStopMedia, completed ),
				sdiv( sumStopMediaToHangUp, completed ),
				sdiv( sumHangUpToDone, completed ),
				done ? " done" : "" );
			
			report( sb.toString() );
		}
		
		private String timeToString( double rt )
		{
			StringBuffer sb = new StringBuffer();
			int t = (int) rt;
			int s = t % 60;
			t /= 60;
			int m = t % 60;
			t /= 60;
			int h = t % 24;
			int d = t / 24;
			sb.append( d );
			sb.append( ':' );
			sb.append( h );
			sb.append( ':' );
			sb.append( m );
			sb.append( ':' );
			sb.append( s );
			return sb.toString();
		}

		private long sdiv( long x, int y )
		{
			if (y == 0)
				return 0;
			
			return x / y;
		}
		
		private long t0;

		//////////////////
		// GLOBAL STATS //
		//////////////////
		
		private double realTime()
		{
			return Timer.getSecondsSince( startTime );
		}
		
		private double virtTime = 0;
		
		private int attempted;
		
		private int completed;
		
		private int failed;

		//////////////////////////
		// COMPLETED CALL STATS //
		//////////////////////////
		
		private long sumStartToAccept;
		
		private long sumAcceptToAnswer;
		
		private long sumAnswerToStopMedia;
		
		private long sumStopMediaToHangUp;
		
		private long sumHangUpToDone;
	}
	
	/**
	 * Description of CallInfo.
	 */
	public static class CallInfo
	{
		/**
		 * Constructs the CallInfo.
		 *
		 * @param onHookTime
		 * @param talkTime
		 */
		public CallInfo( long onHookTime, long talkTime )
		{
			this.onHookTime = onHookTime;
			this.talkTime = talkTime;
			timeout = talkTime + 10000;
		}
		
		/**
		 * Description of onHookTime.
		 */
		public final long onHookTime;

		/**
		 * Description of talkTime.
		 */
		public final long talkTime;
		
		/**
		 * Description of timeout.
		 */
		public final long timeout;
	}
}