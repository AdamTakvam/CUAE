/* $Id: FakeJTapiServer.java 8162 2005-07-27 22:14:16Z wert $
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
import java.net.UnknownHostException;
import java.text.DecimalFormat;
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
public class FakeJTapiServer implements FlatmapIpcListener<IpcConnection>, JTapiServerMBean
{
	private static final String LOCALHOST = "127.0.0.1";

	private static void printUsageAndDie()
	{
		System.err.println( "usage: FakeJTapiServer port logUrl" );
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
		if (args.length > 0)
		{
			if (args[0].equals( "-help" ))
				printUsageAndDie();
			
			try
			{
				port = Integer.parseInt( args[0] );
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
		
		// for dev mode add '?maxLen=0' (disables queuing)
		URL logUrl = new URL( "class:metreos.service.jtapi.MyDefaultTrace" );
		if (args.length > 1)
		{
			try
			{
				logUrl = new URL( args[1] );
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
		
		if (args.length > 2)
			printUsageAndDie();

		Trace trace = (Trace) ClassURL.makeInstance( Trace.class, logUrl );
		trace.start();
		Trace.setTrace( trace );
		
//		IpcTrace it = new IpcTrace( QueuedTrace.DEFAULT_MAXLEN,
//			QueuedTrace.DEFAULT_ADDDELAY, null, 0, "FakeJTapiServer" );
//		//it.start();
//		it.rprt( "IpcTrace works" );
//		it.setMask( Trace.FAILURE_MASK );
		
//		DefaultTrace dt = new DefaultTrace( QueuedTrace.DEFAULT_MAXLEN,
//			QueuedTrace.DEFAULT_ADDDELAY)
//		{
//			protected void formatThrowable( PrintStream ps, Throwable t )
//			{
//				if (t instanceof CiscoJtapiException)
//				{
//					CiscoJtapiException e = (CiscoJtapiException) t;
//					ps.print( "additional info: " );
//					ps.print( e.getErrorName() );
//					ps.print( ": " );
//					ps.println( e.getErrorDescription() );
//				}
//				t.printStackTrace( ps );
//			}
//		};
//		//dt.start();
//		dt.rprt( "DefaultTrace works" );
//		dt.setMask( Trace.FAILURE_MASK );
		
//		ComboTrace ct = new ComboTrace( dt, it );
//		ct.rprt( "ComboTrace works" );
		
//		Trace.setTrace( ct );
//		Trace.report( "Trace works" );
		
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
			catch ( Exception e )
			{
				// ignore
			}
		}
		
		final InetAddress fakeIp = fakeSocket.getLocalAddress();
		
		final int fakePort = fakeSocket.getLocalPort();
		
		Trace.report( Trace.m( "fake media destination: " ).a( fakeIp ).a( "/" ).a( fakePort ) );
		
//		final MBeanServer mbs = ManagementFactory.getPlatformMBeanServer();
		final Object mbs = null;
		
		ConnectionHandler ch = new IpcConnectionHandler<IpcConnection>()
		{
			@Override
			protected void addListeners( IpcConnection connection )
			{
				connection.addListener( new FlatmapIpcListenerAdapter<IpcConnection>(
					new FakeJTapiServer( null, connection, printer, mbs, fakeIp, fakePort ),
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
		
		Server server = new Server( ssf, ch, intf, port, mbs );
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
	 * Constructs a new FakeJTapiServer.
	 * 
	 * @param factory
	 * @param ipcConnection
	 * @param printer
	 * @param mbs
	 * @param fakeIp 
	 * @param fakePort 
	 */
	public FakeJTapiServer( ProviderFactory factory, IpcConnection ipcConnection,
		PrettyPrinter printer, Object mbs, InetAddress fakeIp, int fakePort )
	{
		this.ipcConnection = ipcConnection;
		this.name = ipcConnection.getName();
		this.printer = printer;
		this.fakeIp = fakeIp;
		this.fakePort = fakePort;
//		this.mbs = mbs;
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
	
	private final InetAddress fakeIp;
	
	private final int fakePort;
	
	private final AlarmManager am = new AlarmManager();
	{
		am.start();
	}
	
//	private final Object mbs; //MBeanServer

//	private Object objName; // ObjectName
	
	@Override
	public String toString()
	{
		return "fjs";
	}

//	public String toString()
//	{
//		if (toString0 == null)
//			toString0 =  "FakeJTapiServer @ " + name;
//		return toString0;
//	}
//	
//	private String toString0;

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
	
	private void report( Trace.M m )
	{
		Trace.report( this, m );
	}
	
//	private void report( Trace.M m, Throwable t )
//	{
//		Trace.report( this, m, t );
//	}
	
	private void report( Throwable t )
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
				
				int callRate = 32400;
				int testTime = 120*60*60*1000;
				
				int talkTime = 5000;
				int onHookTime = 1000;
				int callDuration = talkTime+onHookTime+200;
				
				Trace.report( this, "callDuration = "+callDuration );
				
				int oneCallerRate = 3600000/callDuration;
				int nCallers = callRate / oneCallerRate;
				
				Trace.report( this, "nCallers = "+nCallers );
				
				int nCalls = testTime / callDuration;
				
				Trace.report( this, "nCalls = "+nCalls );
				
				startCalling( deviceType, deviceName, nCallers, nCalls, talkTime, onHookTime );
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
//		if (fromCallId == null)
//		{
//			report( newMissingFieldMessage( MsgType.MakeCall, MsgField.FromCallId ) );
//			return;
//		}

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
//					.add( MsgField.ToCallId, toCallId ) );
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

	private boolean cmMapPut( FakeCall fc )
	{
		synchronized (cmMap)
		{
			if (cmMap.containsKey( fc.id ))
				return false;
			
			cmMap.put( fc.id, fc );
			return true;
		}
	}

	private void cmMapRemove( FakeCall fc )
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
		final int nCallers, final int nCalls, final int talkTime,
		final int onHookTime )
	{
		new Thread()
		{
			@Override
			public void run()
			{
				try
				{
					DecimalFormat df = new DecimalFormat( "00" );
					
					Caller[] caller = new Caller[nCallers];
					for (int i = 0; i < nCallers; i++)
					{
						String s = df.format( i );
						caller[i] = new Caller( deviceType, deviceName,
							"59XX", "33"+s, "59"+s, nCalls, talkTime, onHookTime );
					}
					
					Thread.sleep( 5000 );
					
					for (int i = 0; i < nCallers; i++)
						caller[i].start();
					
					for (int i = 0; i < nCallers; i++)
						caller[i].join();
					
					new Formatter( System.out ).format( "----- a/c/f %d/%d/%d -----\n",
						attempted, completed, failed );
					
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
	
	/**
	 * blah
	 */
	synchronized void incrAttempted()
	{
		if (attempted == 0)
			t0 = firstAttempt = Timer.getNanos();
		
		attempted++;
		
		if (Timer.getMillisSince( t0 ) >= 15000)
		{
			t0 = Timer.getNanos();
			int bhca = (int)(attempted * 3600 / Timer.getSecondsSince( firstAttempt ));
			Trace.report( this, "----- a/c/f "+attempted+"/"+completed+"/"+failed
				+" bhca "+bhca
				+" deltas "+(tdeltas[0]/completed)+"/"+(tdeltas[1]/completed)+"/"+(tdeltas[2]/completed)+"/"+(tdeltas[3]/completed)+"/"+(tdeltas[4]/completed)+" -----" );
		}
	}
	
	private long firstAttempt;
	
	private long t0;
	
	/**
	 * Description of attempted.
	 */
	int attempted;
	
	/**
	 * blah
	 * @param deltas 
	 */
	synchronized void incrCompleted( long[] deltas )
	{
		completed++;
		
		int n = deltas.length;
		for (int i = 0; i < n; i++)
			tdeltas[i] += deltas[i];
	}
	
	private long[] tdeltas = new long[FakeCall.DONE]; 
	
	/**
	 * Description of completed.
	 */
	int completed;
	
	/**
	 * @param detail
	 */
	synchronized void incrFailed( String detail )
	{
		Trace.report( this, "call failed: "+detail );
		failed++;
		System.exit( 1 ); // stop at the first sign of trouble!
	}
	
	/**
	 * Description of failed.
	 */
	int failed;
	
	/**
	 * Description of running.
	 */
	static boolean running = true;
	
	@SuppressWarnings("all")
	private class Caller extends Thread
	{
		public Caller( int deviceType, String deviceName, String from,
			String to, String originalTo, int nCalls, int talkTime,
			int onHookTime )
		{
			super();
			this.deviceType = deviceType;
			this.deviceName = deviceName;
			this.from = from;
			this.to = to;
			this.originalTo = originalTo;
			this.nCalls = nCalls;
			this.talkTime = talkTime;
			this.onHookTime = onHookTime;
		}
		
		private final int deviceType;
		
		private final String deviceName;
		
		private final String from;
		
		private final String to;
		
		private final String originalTo;
		
		private final int nCalls;
		
		private final int talkTime;
		
		private final int onHookTime;
		
		public void run()
		{
			for (int i = 0; running & i < nCalls; i++)
			{
				FakeCall fc = null;
				
				try
				{
					Thread.sleep( onHookTime );
					
					fc = new FakeCall( deviceType, deviceName,
						newId(), from, to, originalTo, talkTime );
					
					while (!cmMapPut( fc ))
						fc.id = newId();
					
					incrAttempted();
					fc.sendIncomingCall();
					
					int finalState = fc.waitCallComplete( talkTime*2 );
					if (finalState == FakeCall.DONE)
					{
						int n = FakeCall.DONE;
						long[] deltas = new long[n];
						for (int j = 0; j < n; j++)
							deltas[j] = (fc.nanos[j+1] - fc.nanos[j])/1000000;
						incrCompleted( deltas );
					}
					else
					{
						incrFailed( fc.toString() );
					}
				}
				catch ( Exception e )
				{
					e.printStackTrace();
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
	
	@SuppressWarnings("all")
	private class FakeCall implements AlarmListener
	{
		public FakeCall( int deviceType, String deviceName, String id,
			String to, String from, String originalTo, int talkTime )
		{
			this.deviceType = deviceType;
			this.deviceName = deviceName;
			this.id = id;
			this.to = to;
			this.from = from;
			this.originalTo = originalTo;
			this.talkTime = talkTime;
			monitor = new Monitor( "call monitor "+id );
			change( -1, START );
		}
		
		public String toString()
		{
			return id+", "+currentState;
		}
		
		private final int deviceType;
		
		private final String deviceName;

		String id;
		
		private final String to;
		
		private final String from;
		
		private final String originalTo;

		private final int talkTime;
		
		int currentState = -1;
		
		long[] nanos = new long[NSTATES];
		
		private final Monitor monitor;
		
		private static final int START = 0;
		private static final int ACCEPT = 1;
		private static final int ANSWER = 2;
		private static final int KILL_MEDIA = 3;
		private static final int HANG_UP = 4;
		private static final int DONE = 5;
		private static final int TIMEOUT = 6;
		private static final int NSTATES = 7;

		private synchronized void change( int oldState, int newState )
		{
			if (currentState != oldState)
				throw new UnsupportedOperationException(
					"change from "+oldState+" to "+newState+": current state is "+currentState );
			
			currentState = newState;
			nanos[newState] = Timer.getNanos();
			monitor.set( newState );
		}
		
		public int waitCallComplete( int maxDelay )
		{
			try
			{
				monitor.waitUntilEq( DONE, maxDelay );
				return (Integer) monitor.get();
				
			}
			catch ( InterruptedException e )
			{
				Integer i = (Integer) monitor.get();
				if (i == null)
					return TIMEOUT;
				return i;
			}
		}
		
		public void useMohMedia()
		{
			throw new UnsupportedOperationException( "useMohMedia not implemented" );
		}

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

		public void sendUserInput( String digits )
		{
			throw new UnsupportedOperationException( "sendUserInput not implemented" );
		}

		public void hangup()
		{
			throw new UnsupportedOperationException( "hangup not implemented" );
		}

		public void transfer( String to )
		{
			throw new UnsupportedOperationException( "transfer not implemented" );
		}

		public void answer( InetAddress rxIp, int rxPort )
		{
			change( ANSWER, KILL_MEDIA );
			
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
			
			am.add( this, 5000 );
		}

		public void conference( FakeCall otherCm )
		{
			throw new UnsupportedOperationException( "conference not implemented" );
		}

		public void reject()
		{
			throw new UnsupportedOperationException( "reject not implemented" );
		}

		public void redirect( String to )
		{
			throw new UnsupportedOperationException( "redirect not implemented" );
		}

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
			change( KILL_MEDIA, HANG_UP );
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
}