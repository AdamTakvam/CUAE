/* $Id: JTapiServer.java 32413 2007-05-25 21:07:53Z rvarghee $
 * 
 * Created by wert on Jan 31, 2005
 * 
 * Copyright (c) 2005 Metreos, Inc. All rights reserved.
 */

package metreos.service.jtapi;

import java.io.IOException;
import java.net.DatagramSocket;
import java.net.InetAddress;
import java.net.Socket;
import java.net.UnknownHostException;
import java.util.ArrayList;
import java.util.HashSet;
import java.util.Hashtable;
import java.util.Iterator;
import java.util.List;
import java.util.Map;
import java.util.Set;
import java.util.Vector;

import javax.telephony.Address;
import javax.telephony.JtapiPeerFactory;

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
import metreos.core.net.ipc.IpcConnectionHandler;
import metreos.util.Assertion;
import metreos.util.ClassURL;
import metreos.util.SimpleTodo;
import metreos.util.TodoManager;
import metreos.util.Trace;
import metreos.util.URL;

import com.cisco.jtapi.InvalidPartyExceptionImpl;
import com.cisco.jtapi.PlatformExceptionImpl;
import com.cisco.jtapi.extensions.CiscoG711MediaCapability;
import com.cisco.jtapi.extensions.CiscoG723MediaCapability;
import com.cisco.jtapi.extensions.CiscoG729MediaCapability;
import com.cisco.jtapi.extensions.CiscoJtapiException;
import com.cisco.jtapi.extensions.CiscoJtapiPeer;
import com.cisco.jtapi.extensions.CiscoJtapiVersion;
import com.cisco.jtapi.extensions.CiscoMediaCapability;
import com.cisco.jtapi.extensions.CiscoRTPPayload;

/**
 * A server which is an adapter between metreos ipc/flatmap messages and JTapi
 * interface.
 */
public class JTapiServer implements FlatmapIpcListener<MyIpcConnection>, JTapiServerMBean
{
	private static final String LOCALHOST = "127.0.0.1";

	private static void printUsageAndDie()
	{
		System.err.println( "usage: JTapiServer port logUrl" );
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
//		Field[] fields = CiscoJtapiException.class.getFields();
//		for (Field field: fields)
//		{
//			System.out.println( "CiscoJtapiException field "+field.getName()+" = "+field.getInt( null ) );
//		}
		
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
//			QueuedTrace.DEFAULT_ADDDELAY, null, 0, "JTapiServer" );
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
		
		Trace.report( true, Trace.m( new CiscoJtapiVersion() ) );

		CiscoJtapiPeer jtapiPeer = (CiscoJtapiPeer) JtapiPeerFactory.getJtapiPeer( null );

		final ProviderFactory factory = new ProviderFactory( jtapiPeer );

		ServerSocketFactory ssf = new PlainServerSocketFactory();

		final PrettyPrinter printer = new MessagePrettyPrinter();
		
		DatagramSocket fakeSocket = null;
		
		InetAddress lh = InetAddress.getByName("127.0.0.1");
		
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
		
		final InetAddress fakeMediaIp = fakeSocket.getLocalAddress();
		
		final int fakeMediaPort = fakeSocket.getLocalPort();
		
		Trace.report( Trace.m( "fake media destination: " ).a( fakeMediaIp ).a( "/" ).a( fakeMediaPort ) );
		
		ConnectionHandler ch = new IpcConnectionHandler<MyIpcConnection>()
		{
			@Override
			protected MyIpcConnection newIpcConnection( Server server, Socket socket )
				throws IOException
			{
				return new MyIpcConnection( server, socket, printer );
			}
			
			@Override
			protected void addListeners( MyIpcConnection connection )
			{
				connection.addListener( new FlatmapIpcListenerAdapter<MyIpcConnection>(
					new JTapiServer( factory, connection, fakeMediaIp, fakeMediaPort ),
						printer ) );
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
	}

	/**
	 * Constructs a new JTapiServer.
	 * 
	 * @param factory
	 * @param ipcConnection
	 * @param fakeMediaIp 
	 * @param fakeMediaPort 
	 */
	public JTapiServer( ProviderFactory factory, MyIpcConnection ipcConnection,
		InetAddress fakeMediaIp, int fakeMediaPort )
	{
		this.factory = factory;
		this.ipcConnection = ipcConnection;
		this.fakeMediaIp = fakeMediaIp;
		this.fakeMediaPort = fakeMediaPort;
		name = ipcConnection.getName();
	}

	private final ProviderFactory factory;

	private final MyIpcConnection ipcConnection;
	
	private final InetAddress fakeMediaIp;
	
	private final int fakeMediaPort;

	private final String name;

	@Override
	public String toString()
	{
		if (toString0 == null)
			toString0 =  "JTapiServer @ " + name;
		return toString0;
	}
	
	private String toString0;

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
		return new FlatmapIpcMessage( messageType, ipcConnection.getPrinter(), ipcConnection );
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
	
	private void report( Trace.M m, Throwable t )
	{
		Trace.report( this, m, t );
	}
	
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
		Trace.report( this, Trace.m( "sending message: " ).a( msg ) );
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
	public void startup( MyIpcConnection connection )
		throws Exception
	{
		if (!setConnected( true ))
			throw new IOException( "already connected" );
		
		
		report( Trace.m( "startup..." ) );
		
		addDeviceMap( new CtiPortMap( DeviceType.CtiPort, this, connection, cmMap, fakeMediaIp, fakeMediaPort ) );
		addDeviceMap( new RoutePointMap( DeviceType.RoutePoint, this, connection, cmMap, fakeMediaIp, fakeMediaPort ) );
		addDeviceMap( new CtiPortMap( DeviceType.MonitoredDevice, this, connection, cmMap, fakeMediaIp, fakeMediaPort ) );
		
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
			report( Trace.m( "startup done." ) );
		}
		catch ( Exception e )
		{
			report( Trace.m( "startup failed." ), e );
			throw e;
		}
	}
	
	private static synchronized boolean setConnected( boolean status )
	{
		if (connected == status)
			return false;
		
		connected = status;
		return true;
	}
	
	private static boolean connected = false;

	/*
	 * (non-Javadoc)
	 * 
	 * @see metreos.core.ipc.flatmaps.FlatmapIpcListener#received(metreos.core.net.ipc.IpcConnection,
	 * metreos.core.ipc.flatmaps.FlatmapIpcMessage)
	 */
	public void received( final MyIpcConnection connection, final FlatmapIpcMessage message )
	{
		report( Trace.m( "received message: " ).a( message ) );
		
		if (syncs.contains( message.messageType ))
		{
			doReceived( connection, message );
			return;
		}
		
		TodoManager.getTodoManager().add( new SimpleTodo()
		{
			public void doit()
			{
				doReceived( connection, message );
			}
		} );
	}
	
	private final static Set<Integer> syncs = new HashSet<Integer>();
	static
	{
		syncs.add( MsgType.RegisterMediaCaps );
		syncs.add( MsgType.Register );
		syncs.add( MsgType.UnregisterMediaCaps );
		syncs.add( MsgType.Unregister );
	}

	/**
	 * @param connection
	 * @param message
	 */
	void doReceived( MyIpcConnection connection, FlatmapIpcMessage message )
	{
		try
		{
			switch (message.messageType)
			{
				case MsgType.SetLogLevel:
					setLogLevel( connection, message.args );
					break;
				
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
					
				case MsgType.HoldCall:
					holdCall( connection, message.args );
					break;
					
				case MsgType.ResumeCall:
					resumeCall( connection, message.args );
					break;
	
				default:
					reportAndSend( newErrorMessage( FailReason.UnknownMessageType, message.messageType ) );
					break;
			}
		}
		catch ( PlatformExceptionImpl e )
		{
			if (e instanceof CiscoJtapiException)
			{
				CiscoJtapiException cje = (CiscoJtapiException) e;
			
			    reportAndSend( newErrorMessage( FailReason.PlatformException, message.messageType )
					.add( MsgField.Message, e.toString() )
					.add( MsgField.Message, cje.getErrorName()+": "+
						cje.getErrorDescription() )
					.add( MsgField.Args, message.args ), e );
			}
		}
		catch ( InvalidPartyExceptionImpl e )
		{
			if (e instanceof CiscoJtapiException)
			{
				CiscoJtapiException cje = (CiscoJtapiException) e;
			
			    reportAndSend( newErrorMessage( FailReason.InvalidDestination, message.messageType )
					.add( MsgField.Message, e.toString() )
					.add( MsgField.Message, cje.getErrorName()+": "+
						cje.getErrorDescription() )
					.add( MsgField.Args, message.args ), e );
			}
		}
		catch ( Exception e )
		{
			if (e instanceof CiscoJtapiException)
			{
				CiscoJtapiException cje = (CiscoJtapiException) e;
				
				//report( e );
				reportAndSend( newErrorMessage( FailReason.GeneralFailure, message.messageType )
						.add( MsgField.Message, e.toString() )
						.add( MsgField.Message, cje.getErrorName()+": "+
							cje.getErrorDescription() )
						.add( MsgField.Args, message.args ), e );
			}
			else
			{
				//report( e );
				reportAndSend( newErrorMessage( FailReason.GeneralFailure, message.messageType )
						.add( MsgField.Message, e.toString() )
						.add( MsgField.Args, message.args ), e );
			}
		}
	}

	private void setLogLevel( MyIpcConnection connection, FlatmapList args )
	{
	
		Integer level = args.getInteger( MsgField.Level );
		if (level == null)
		{
			reportAndSend( newMissingFieldMessage( MsgType.SetLogLevel, MsgField.Level ) );
			return;
		}
		
		// convert log level to mask
		int mask = 1;
		if (level >= 3)
		{
			mask = 3;
		}
		Trace.getTrace().setMask( mask );
		
		Trace.report( this, "The log level has been set to level: " + level + " mask: " + mask );
	}

	/*
	 * (non-Javadoc)
	 * 
	 * @see metreos.core.ipc.flatmaps.FlatmapIpcListener#shutdown(metreos.core.net.ipc.IpcConnection)
	 */
	public void shutdown( MyIpcConnection connection )
	{
		report( Trace.m( "shutdown..." ) );

		{
			Iterator<DeviceMap> i = getDeviceMaps();
			while (i.hasNext())
				i.next().dropAll();
		}
		
		{
			Iterator<String> i = getProviderWrapperKeys().iterator();
			while (i.hasNext())
				shutdownProviderWrapper( i.next() );
		}
		
//		try
//		{
//			mbs.unregisterMBean( objName );
//		}
//		catch ( Exception e )
//		{
//			report( e );
//		}
		
		report( Trace.m( "shutdown done." ) );
		
		setConnected( false );
	}

	/*
	 * (non-Javadoc)
	 * 
	 * @see metreos.core.ipc.flatmaps.FlatmapIpcListener#exception(metreos.core.net.ipc.IpcConnection,
	 * java.lang.Exception)
	 */
	public void exception( MyIpcConnection connection, Exception e )
	{
		report( e );
	}

	/////////////////
	// DEVICE MAPS //
	/////////////////

	/**
	 * Returns an iterator over the device maps.
	 * 
	 * @return an iterator over the device maps.
	 */
	public Iterator<DeviceMap> getDeviceMaps()
	{
		return new ArrayList<DeviceMap>( deviceMaps.values() ).iterator();
	}

	/**
	 * Gets the device map for a given device type.
	 * 
	 * @param deviceType
	 * 
	 * @return a type-specific map of devices.
	 */
	private DeviceMap getDeviceMap( int deviceType )
	{
		return deviceMaps.get( deviceType );
	}

	/**
	 * Adds a map.
	 * 
	 * @param deviceMap
	 */
	private void addDeviceMap( DeviceMap deviceMap )
	{
		deviceMaps.put( deviceMap.deviceType, deviceMap );
	}

	private Map<Integer,DeviceMap> deviceMaps = new Hashtable<Integer,DeviceMap>();

	/////////////
	// DEVICES //
	/////////////

	/**
	 * Registers some devices in the appropriate device map.
	 * 
	 * @param connection
	 * @param args
	 * @throws Exception 
	 */
	private void registerDevice( MyIpcConnection connection, FlatmapList args )
		throws Exception
	{
		Iterator<MapEntry> i;
		
		List<String> managers = new Vector<String>();
		Set<String> managersCheck = new HashSet<String>();
		i = args.getValues( MsgField.CtiManager );
		while (i.hasNext())
		{
			String manager = i.next().stringValue();
			if (!managersCheck.contains( manager ))
			{
				managers.add( manager );
				managersCheck.add( manager );
			}
			else
			{
				report( Trace.m( "duplicate manager ignored: " ).a( manager ) );
			}
		}
		managersCheck = null;
		
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
		DeviceMap dm = getDeviceMap( deviceType );
		if (dm == null)
		{
			reportAndSend( newErrorMessage( FailReason.InvalidDeviceType, MsgType.Register )
					.add( MsgField.DeviceType, deviceType ) );
			return;
		}
		
		boolean thirdParty = args.getInt( MsgField.ThirdParty, 0 ) == 1;

		i = args.getValues( MsgField.DeviceName );
		while (i.hasNext())
		{
			String deviceName = i.next().stringValue();
			dm.add( deviceName, managers, username, password, thirdParty );
		}
	}

	/**
	 * Unregisters some devices.
	 * 
	 * @param connection
	 * @param args
	 */
	private void unregisterDevice( MyIpcConnection connection, FlatmapList args )
	{
		Integer iDeviceType = args.getInteger( MsgField.DeviceType );
		if (iDeviceType == null)
		{
			reportAndSend( newMissingFieldMessage( MsgType.Unregister, MsgField.DeviceType ) );
			return;
		}
		
		int deviceType = iDeviceType.intValue();
		DeviceMap dm = getDeviceMap( deviceType );
		if (dm == null)
		{
			reportAndSend( newErrorMessage( FailReason.InvalidDeviceType, MsgType.Unregister )
					.add( MsgField.DeviceType, deviceType ) );
			return;
		}

		Iterator<MapEntry> i = args.getValues( MsgField.DeviceName );
		while (i.hasNext())
		{
			String deviceName = i.next().stringValue();
			dm.drop( deviceName );
		}
	}

	/**
	 * Gets a device which corresponds to specified device type and name.
	 * 
	 * @param deviceType
	 * @param deviceName
	 * @return the device, or null if the device is not being monitored.
	 */
	public Device getDevice( int deviceType, String deviceName )
	{
		DeviceMap dm = getDeviceMap( deviceType );
		if (dm == null)
			return null;

		return dm.get( deviceName );
	}

	////////////////
	// MEDIA CAPS //
	////////////////

	/**
	 * @param connection
	 * @param args
	 */
	private void registerMediaCaps( MyIpcConnection connection, FlatmapList args )
	{
		int codec = args.getInteger( MsgField.Codec ).intValue();

		Iterator<MapEntry> i = args.getValues( MsgField.Framesize );
		while (i.hasNext())
		{
			int framesize = i.next().integerValue().intValue();
			
			MediaCap mc = getMediaCaps( codec, framesize );
			if (mc == null || !mc.capability.isSupported())
			{
				reportAndSend( newErrorMessage( FailReason.CodecNotSupported, MsgType.RegisterMediaCaps )
						.add( MsgField.Codec, codec )
						.add( MsgField.Framesize, framesize ) );
				return;
			}
			mediaCaps.add( mc );
		}
	}

	/**
	 * @param connection
	 * @param args
	 */
	private void unregisterMediaCaps( MyIpcConnection connection, FlatmapList args )
	{
		int codec = args.getInteger( MsgField.Codec ).intValue();

		Iterator<MapEntry> i = args.getValues( MsgField.Framesize );
		while (i.hasNext())
		{
			int framesize = i.next().integerValue().intValue();
			
			MediaCap mc = getMediaCaps( codec, framesize );
			if (mc == null || !mc.capability.isSupported())
			{
				reportAndSend( newErrorMessage( FailReason.CodecNotSupported, MsgType.UnregisterMediaCaps )
						.add( MsgField.Codec, codec )
						.add( MsgField.Framesize, framesize ) );
				return;
			}
			
			synchronized (mediaCaps)
			{
				mediaCaps.remove( mc );
			}
		}
	}

	/**
	 * @param codec
	 * @param framesize
	 * @return the cisco codec value
	 */
	public static MediaCap getMediaCaps( int codec, int framesize )
	{
		if (codec == Codec.G711u)
		{
			if (framesize == 10)
				return new MediaCap( codec, framesize, new CiscoMediaCapability(
					CiscoRTPPayload.G711ULAW64K,
					10 ) );
			else if (framesize == 20)
				return new MediaCap( codec, framesize, new CiscoMediaCapability(
					CiscoRTPPayload.G711ULAW64K,
					CiscoG711MediaCapability.FRAMESIZE_TWENTY_MILLISECOND_PACKET ) );
			else if (framesize == 30)
				return new MediaCap( codec, framesize, new CiscoMediaCapability(
					CiscoRTPPayload.G711ULAW64K,
					CiscoG711MediaCapability.FRAMESIZE_THIRTY_MILLISECOND_PACKET ) );
		}
		else if (codec == Codec.G711a)
		{
			if (framesize == 10)
				return new MediaCap( codec, framesize, new CiscoMediaCapability(
					CiscoRTPPayload.G711ALAW64K,
					10 ) );
			else if (framesize == 20)
				return new MediaCap( codec, framesize, new CiscoMediaCapability(
					CiscoRTPPayload.G711ALAW64K,
					CiscoG711MediaCapability.FRAMESIZE_TWENTY_MILLISECOND_PACKET ) );
			else if (framesize == 30)
				return new MediaCap( codec, framesize, new CiscoMediaCapability(
					CiscoRTPPayload.G711ALAW64K,
					CiscoG711MediaCapability.FRAMESIZE_THIRTY_MILLISECOND_PACKET ) );
		}
		else if (codec == Codec.G723)
		{
			if (framesize == 20)
				return new MediaCap( codec, framesize, new CiscoMediaCapability(
					CiscoRTPPayload.G7231,
					CiscoG723MediaCapability.FRAMESIZE_TWENTY_MILLISECOND_PACKET ) );
			else if (framesize == 30)
				return new MediaCap( codec, framesize, new CiscoMediaCapability(
					CiscoRTPPayload.G7231, 
					CiscoG723MediaCapability.FRAMESIZE_THIRTY_MILLISECOND_PACKET ) );
			else if (framesize == 60)
				return new MediaCap( codec, framesize, new CiscoMediaCapability(
					CiscoRTPPayload.G7231, 
					CiscoG723MediaCapability.FRAMESIZE_SIXTY_MILLISECOND_PACKET ) );
		}
		else if (codec == Codec.G729)
		{
			if (framesize == 10)
				return new MediaCap( codec, framesize, new CiscoMediaCapability(
					CiscoRTPPayload.G729ANNEXA, 
					10 ) );
			else if (framesize == 20)
				return new MediaCap( codec, framesize, new CiscoMediaCapability(
					CiscoRTPPayload.G729ANNEXA, 
					CiscoG729MediaCapability.FRAMESIZE_TWENTY_MILLISECOND_PACKET ) );
			else if (framesize == 30)
				return new MediaCap( codec, framesize, new CiscoMediaCapability(
					CiscoRTPPayload.G729ANNEXA,
					CiscoG729MediaCapability.FRAMESIZE_THIRTY_MILLISECOND_PACKET ) );
			else if (framesize == 40)
				return new MediaCap( codec, framesize, new CiscoMediaCapability(
					CiscoRTPPayload.G729ANNEXA,
					40 ) );
			else if (framesize == 50)
				return new MediaCap( codec, framesize, new CiscoMediaCapability(
					CiscoRTPPayload.G729ANNEXA,
					50 ) );
			else if (framesize == 60)
				return new MediaCap( codec, framesize, new CiscoMediaCapability(
					CiscoRTPPayload.G729ANNEXA,
					CiscoG729MediaCapability.FRAMESIZE_SIXTY_MILLISECOND_PACKET ) );
		}
		return null;
	}

	/**
	 * @param payloadType
	 * @return the metreos codec
	 */
	public static int ciscoToMetreosCodec( int payloadType )
	{
		switch (payloadType)
		{
			case CiscoRTPPayload.G711ULAW64K: return Codec.G711u;
			case CiscoRTPPayload.G711ALAW64K: return Codec.G711a;
			case CiscoRTPPayload.G7231: return Codec.G723;
			case CiscoRTPPayload.G729: return Codec.G729;
			case CiscoRTPPayload.G729ANNEXA: return Codec.G729;
			default: return Codec.Unspecified;
		}
	}

	/**
	 * @param packetSize
	 * @return the metreos framesize
	 */
	public static int ciscoToMetreosFramesize( int packetSize )
	{
		switch (packetSize)
		{
		    case 10: // There is no constant defined for 10 ms packetSize
			    return 10;
			case CiscoG711MediaCapability.FRAMESIZE_TWENTY_MILLISECOND_PACKET:
				return 20;
			case CiscoG711MediaCapability.FRAMESIZE_THIRTY_MILLISECOND_PACKET:
				return 30;
			default:
				return 0;
		}
	}

	/**
	 * Get cisco media capabilities.
	 * 
	 * @return media capabilities.
	 */
	public CiscoMediaCapability[] getCiscoMediaCapabilies()
	{
		CiscoMediaCapability[] ciscoMediaCaps = new CiscoMediaCapability[mediaCaps.size()];
		int index = 0;
		Iterator<MediaCap> i = mediaCaps.iterator();
		while (i.hasNext())
		{
			MediaCap mc = i.next();
			ciscoMediaCaps[ index++ ] = mc.capability;
		}
		return ciscoMediaCaps;
	}

	private Set<MediaCap> mediaCaps = new HashSet<MediaCap>();

	//////////////////
	// CALL CONTROL //
	//////////////////

	/**
	 * @param connection
	 * @param args
	 * @throws Exception
	 */
	private void makeCall( MyIpcConnection connection, FlatmapList args )
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
		DeviceMap deviceMap = getDeviceMap( deviceType );
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
		Device device = deviceMap.get( deviceName );
		if (device == null)
		{
			reportAndSend( newErrorMessage( FailReason.InvalidDeviceName, MsgType.MakeCall )
					.add( MsgField.FromCallId, fromCallId )
					.add( MsgField.ToCallId, toCallId )
					.add( MsgField.DeviceType, deviceType )
					.add( MsgField.DeviceName, deviceName ) );
			return;
		}
		
		if (!device.hasProvider())
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
		
		Address fromAddress = device.getAddress( from );
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
		
		InetAddress rxIp = str2inetAddress( args.getString( MsgField.RxIp ) );
		int rxPort = args.getInt( MsgField.RxPort, 0 );
		
		device.makeCall( fromCallId, toCallId, fromAddress, to, rxIp, rxPort );
		
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
	private void setCallId( MyIpcConnection connection, FlatmapList args )
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
		
		if (cmMap.rename( callId, newCallId ) == null)
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
	private void acceptCall( MyIpcConnection connection, FlatmapList args )
		throws Exception
	{
		String callId = args.getString( MsgField.CallId );
		if (callId == null)
		{
			reportAndSend( newMissingFieldMessage( MsgType.AcceptCall, MsgField.CallId ) );
			return;
		}
		
		String newCallId = args.getString( MsgField.NewCallId );

		CallMonitor cm;
		if (newCallId != null)
		{
			cm = cmMap.rename( callId, newCallId );
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
			cm = cmMap.get( callId );
			if (cm == null)
			{
				reportAndSend( newErrorMessage( FailReason.CallIdUnknown, MsgType.AcceptCall )
						.add( MsgField.CallId, callId ) );
				return;
			}
		}
		
		synchronized (cm.MSG_LOCK)
		{
			cm.accept();
		}
	}

	/**
	 * @param connection
	 * @param args
	 */
	private void redirectCall( MyIpcConnection connection, FlatmapList args )
	throws Exception
	{
		String callId = args.getString( MsgField.CallId );
		if (callId == null)
		{
			reportAndSend( newMissingFieldMessage( MsgType.RedirectCall, MsgField.CallId ) );
			return;
		}
		
		CallMonitor cm = cmMap.get( callId );
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

		synchronized (cm.MSG_LOCK)
		{
			cm.redirect( to );
		}
	}

	/**
	 * @param connection
	 * @param args
	 */
	private void rejectCall( MyIpcConnection connection, FlatmapList args )
		throws Exception
	{
		String callId = args.getString( MsgField.CallId );
		if (callId == null)
		{
			reportAndSend( newMissingFieldMessage( MsgType.RejectCall, MsgField.CallId ) );
			return;
		}
		
		CallMonitor cm = cmMap.get( callId );
		if (cm == null)
		{
			reportAndSend( newErrorMessage( FailReason.CallIdUnknown, MsgType.RejectCall )
					.add( MsgField.CallId, callId ) );
			return;
		}

		synchronized (cm.MSG_LOCK)
		{
			cm.reject();
		}
	}

	private void conferenceCall( MyIpcConnection connection, FlatmapList args )
		throws Exception
	{
		String callId = args.getString( MsgField.CallId );
		if (callId == null)
		{
			reportAndSend( newMissingFieldMessage( MsgType.ConferenceCall, MsgField.CallId ) );
			return;
		}
		
		CallMonitor cm = cmMap.get( callId );
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
		
		CallMonitor otherCm = cmMap.get( otherCallId );
		if (otherCm == null)
		{
			reportAndSend( newErrorMessage( FailReason.CallIdUnknown, MsgType.ConferenceCall )
					.add( MsgField.OtherCallId, otherCallId ) );
			return;
		}

		synchronized (cm.MSG_LOCK)
		{
			cm.conference( otherCm );
		}
	}

	/**
	 * @param connection
	 * @param args
	 */
	private void answerCall( MyIpcConnection connection, FlatmapList args )
		throws Exception
	{
		String callId = args.getString( MsgField.CallId );
		if (callId == null)
		{
			reportAndSend( newMissingFieldMessage( MsgType.AnswerCall, MsgField.CallId ) );
			return;
		}
		
		CallMonitor cm = cmMap.get( callId );
		if (cm == null)
		{
			reportAndSend( newErrorMessage( FailReason.CallIdUnknown, MsgType.AnswerCall )
					.add( MsgField.CallId, callId ) );
			return;
		}
		
		InetAddress rxIp = str2inetAddress( args.getString( MsgField.RxIp ) );
		int rxPort = args.getInt( MsgField.RxPort, 0 );

		synchronized (cm.MSG_LOCK)
		{
			cm.answer( rxIp, rxPort );
		}
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
	private void transferCall( MyIpcConnection connection, FlatmapList args )
		throws Exception
	{
		String callId = args.getString( MsgField.CallId );
		if (callId == null)
		{
			reportAndSend( newMissingFieldMessage( MsgType.TransferCall, MsgField.CallId ) );
			return;
		}
		
		CallMonitor cm = cmMap.get( callId );
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

		synchronized (cm.MSG_LOCK)
		{
			cm.transfer( to );
		}
	}

	/**
	 * @param connection
	 * @param args
	 */
	private void hangupCall( MyIpcConnection connection, FlatmapList args )
		throws Exception
	{
		String callId = args.getString( MsgField.CallId );
		if (callId == null)
		{
			reportAndSend( newMissingFieldMessage( MsgType.HangupCall, MsgField.CallId ) );
			return;
		}
		
		CallMonitor cm = cmMap.get( callId );
		if (cm == null)
		{
			reportAndSend( newErrorMessage( FailReason.CallIdUnknown, MsgType.HangupCall )
					.add( MsgField.CallId, callId ) );
			return;
		}

		synchronized (cm.MSG_LOCK)
		{
			cm.hangup();
		}
	}

	/**
	 * @param connection
	 * @param args
	 * @throws Exception
	 */
	private void sendUserInput( MyIpcConnection connection, FlatmapList args )
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
		
		CallMonitor cm = cmMap.get( callId );
		if (cm == null)
		{
			reportAndSend( newErrorMessage( FailReason.CallIdUnknown, MsgType.SendUserInput )
					.add( MsgField.CallId, callId ) );
			return;
		}

		synchronized (cm.MSG_LOCK)
		{
			cm.sendUserInput( digits );
		}
	}

	/**
	 * @param connection
	 * @param args
	 * @throws Exception
	 */
	private void setMedia( MyIpcConnection connection, FlatmapList args )
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
		
		Trace.report( this, "setMedia: rxIp = "+rxIp+", rxPort = "+rxPort );
		
		CallMonitor cm = cmMap.get( callId );
		if (cm == null)
		{
			reportAndSend( newErrorMessage( FailReason.CallIdUnknown, MsgType.SetMedia )
					.add( MsgField.CallId, callId ) );
			return;
		}

		synchronized (cm.MSG_LOCK)
		{
			cm.setMedia( rxIp, rxPort );
		}
	}
	
	private void useMohMedia( MyIpcConnection connection, FlatmapList args )
	{
		String callId = args.getString( MsgField.CallId );
		if (callId == null)
		{
			reportAndSend( newMissingFieldMessage( MsgType.UseMohMedia, MsgField.CallId ) );
			return;
		}

		CallMonitor cm = cmMap.get( callId );
		if (cm == null)
		{
			reportAndSend( newErrorMessage( FailReason.CallIdUnknown, MsgType.UseMohMedia )
					.add( MsgField.CallId, callId ) );
			return;
		}

		synchronized (cm.MSG_LOCK)
		{
			cm.useMohMedia();
		}
	}

	private void holdCall( MyIpcConnection connection, FlatmapList args ) throws Exception
	{
		String callId = args.getString( MsgField.CallId );
		if (callId == null)
		{
			reportAndSend( newMissingFieldMessage( MsgType.HoldCall, MsgField.CallId ) );
			return;
		}

		CallMonitor cm = cmMap.get( callId );
		if (cm == null)
		{
			reportAndSend( newErrorMessage( FailReason.CallIdUnknown, MsgType.HoldCall )
					.add( MsgField.CallId, callId ) );
			return;
		}

		cm.holdCall();
	}

	private void resumeCall( MyIpcConnection connection, FlatmapList args )
		throws Exception
	{
		String callId = args.getString( MsgField.CallId );
		if (callId == null)
		{
			reportAndSend( newMissingFieldMessage( MsgType.ResumeCall, MsgField.CallId ) );
			return;
		}

		CallMonitor cm = cmMap.get( callId );
		if (cm == null)
		{
			reportAndSend( newErrorMessage( FailReason.CallIdUnknown, MsgType.ResumeCall )
					.add( MsgField.CallId, callId ) );
			return;
		}
		
		InetAddress rxIp = str2inetAddress( args.getString( MsgField.RxIp ) );
		int rxPort = args.getInt( MsgField.RxPort, 0 );

		cm.resumeCall( rxIp, rxPort );
	}
	
	private CallMonitorMap cmMap = new CallMonitorMap();

	///////////////////////
	// provider wrappers //
	///////////////////////

	/**
	 * Gets a provider.
	 * 
	 * @param managers
	 * @param username
	 * @param password
	 * @return a provider.
	 */
	public ProviderWrapper getProviderWrapper( List<String> managers, String username,
		String password )
	{
		String key = managers.toString() + '/' + username + '/' + password;

		ProviderWrapper pw;
		synchronized (providerWrappers)
		{
			pw = providerWrappers.get( key );
			if (pw == null)
			{
				pw = new ProviderWrapper( key, managers, username, password,
					factory, this  );
				providerWrappers.put( key, pw );
			}
		}
		return pw;
	}
	
	/**
	 * @param key
	 */
	public void shutdownProviderWrapper( String key )
	{
		ProviderWrapper pw = providerWrappers.remove( key );
		if (pw != null)
			pw.shutdown();
	}

	/**
	 * Gets an iterator over the provider wrappers.
	 * 
	 * @return an iterator over the provider wrappers.
	 */
	private List<String> getProviderWrapperKeys()
	{
		return new Vector<String>( providerWrappers.keySet() );
	}

	private Map<String,ProviderWrapper> providerWrappers = new Hashtable<String,ProviderWrapper>();
}