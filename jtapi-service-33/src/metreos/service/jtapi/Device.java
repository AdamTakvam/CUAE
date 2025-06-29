/* $Id: Device.java 12104 2005-10-19 19:18:25Z wert $
 *
 * Created by wert on Jan 31, 2005
 *
 * Copyright (c) 2005 Metreos, Inc. All rights reserved.
 */

package metreos.service.jtapi;

import java.net.InetAddress;
import java.util.HashMap;
import java.util.List;
import java.util.Map;

import javax.telephony.Address;
import javax.telephony.Connection;
import javax.telephony.MethodNotSupportedException;
import javax.telephony.Provider;
import javax.telephony.ResourceUnavailableException;
import javax.telephony.Terminal;
import javax.telephony.events.TermEv;
import javax.telephony.events.TermObservationEndedEv;

import metreos.core.ipc.flatmaps.FlatmapIpcMessage;
import metreos.util.Trace;

import com.cisco.jtapi.extensions.CiscoAddrCreatedEv;
import com.cisco.jtapi.extensions.CiscoAddrRemovedEv;
import com.cisco.jtapi.extensions.CiscoAddress;
import com.cisco.jtapi.extensions.CiscoCall;
import com.cisco.jtapi.extensions.CiscoCallID;
import com.cisco.jtapi.extensions.CiscoConnection;
import com.cisco.jtapi.extensions.CiscoMediaCapability;
import com.cisco.jtapi.extensions.CiscoProvider;
import com.cisco.jtapi.extensions.CiscoRTPInputStartedEv;
import com.cisco.jtapi.extensions.CiscoRTPInputStoppedEv;
import com.cisco.jtapi.extensions.CiscoRTPOutputStartedEv;
import com.cisco.jtapi.extensions.CiscoRTPOutputStoppedEv;
import com.cisco.jtapi.extensions.CiscoRegistrationException;
import com.cisco.jtapi.extensions.CiscoTermInServiceEv;
import com.cisco.jtapi.extensions.CiscoTermOutOfServiceEv;

/**
 * A telephony device.
 */
public abstract class Device implements TerminalMonitor
{
	/**
	 * Constructs a telephony device.
	 * 
	 * @param map the map of the device.
	 * 
	 * @param deviceName the name of the device.
	 * 
	 * @param managers the list of cti managers for this device.
	 * 
	 * @param username the username used to access this device.
	 * 
	 * @param password the password for the username.
	 * @param regAddr 
	 * @param regPort 
	 */
	public Device( DeviceMap map, String deviceName, List<String> managers,
		String username, String password, InetAddress regAddr, int regPort )
	{
		this.map = map;
		this.deviceName = deviceName;
		this.managers = managers;
		this.username = username;
		this.password = password;
		this.regAddr = regAddr;
		this.regPort = regPort;
	}

	/**
	 * The map of the device.
	 */
	private final DeviceMap map;

	/**
	 * The name of the device.
	 */
	private final String deviceName;

	/**
	 * The list of cti managers for this device.
	 */
	private final List<String> managers;

	/**
	 * The username used to access this device.
	 */
	private final String username;

	/**
	 * The password for the username.
	 */
	private final String password;
	
	private final InetAddress regAddr;
	
	private final int regPort;

	/////////////////////
	// FIELD ACCESSORS //
	/////////////////////

	/**
	 * @return our device map.
	 */
	public DeviceMap getMap()
	{
		return map;
	}

	/**
	 * Gets the device type.
	 * 
	 * @return the device type.
	 */
	public int getDeviceType()
	{
		return map.deviceType;
	}

	/**
	 * Gets the device name.
	 * 
	 * @return the name of the device.
	 */
	public String getDeviceName()
	{
		return deviceName;
	}

	//////////////////////
	// PROVIDER WRAPPER //
	//////////////////////

	/**
	 * Opens the device, attempting to make it active.
	 */
	public void open()
	{
		if (providerWrapper == null)
		{
			providerWrapper = map.server.getProviderWrapper( managers,
				username, password, this );
		}
	}

	/**
	 * Closes the device, terminating all operations.
	 */
	public void close()
	{
		closeProvider();

		if (providerWrapper != null)
		{
			providerWrapper.removeListener( this );
			providerWrapper = null;
		}
	}

	/**
	 * An encapsulation of provider.
	 */
	private ProviderWrapper providerWrapper;

	//////////////
	// PROVIDER //
	//////////////

	/**
	 * Notifies the device that a new provider is in service.
	 * @param newProv
	 */
	public void providerInService( Provider newProv )
	{
		setProvider( null, newProv );
	}

	/**
	 * Notifies the device that a current provider is out of service.
	 * @param oldProv
	 */
	public void providerOutOfService( Provider oldProv )
	{
		setProvider( oldProv, null );
	}

	/**
	 * Notifies the device that a current provider has been shutdown.
	 * @param oldProv
	 */
	public void providerShutdown( Provider oldProv )
	{
		setProvider( oldProv, null );
	}

	/**
	 * Changes the current provider.
	 * @param oldProv what we think our current provider should be.
	 * @param newProv what we want our new provider to be.
	 */
	private void setProvider( Provider oldProv, Provider newProv )
	{
		report( Trace.m( "setProvider: oldProv=" ).a( oldProv ).a( ", newProv=" ).a( newProv ) );
		if (provider == oldProv)
		{
			provider = (CiscoProvider) newProv;
			providerChanged();
		}
	}

	/**
	 * Removes our provider.
	 */
	private void closeProvider()
	{
		Provider p = provider;
		if (p != null)
			setProvider( p, null );
	}

	/**
	 * Tells the subclass that the provider changed.
	 */
	private void providerChanged()
	{
		report( Trace.m( "provider changed: " ).a( provider ) );

		if (provider != null)
		{
			termOpen();
		}
		else if (terminal != null)
		{
			termClose();
		}
	}

	/**
	 * @return our current provider.
	 */
	public Provider getProvider()
	{
		return provider;
	}

	/**
	 * Our current provider.
	 */
	private CiscoProvider provider;

	//////////////
	// TERMINAL //
	//////////////

	/**
	 * Opens the terminal and listens for events.
	 */
	private void termOpen()
	{
		try
		{
			Terminal t = provider.getTerminal( deviceName );

			registerMediaCaps( t, regAddr, regPort,
				map.server.getCiscoMediaCapabilies() );
			// addresses will be observed when the device comes online.
			addObserver( t );
			
			terminal = t;

			report( Trace.m( "term opened" ) );
		}
		catch ( Exception e )
		{
			report( e );
		}
	}

	/**
	 * Stops listening for events and closes the terminal.
	 */
	private void termClose()
	{
		unregisterMediaCaps( terminal );
		unobserveAddresses();
		removeObserver();
		report( Trace.m( "terminal closed" ) );
	}
	
	private void addObserver( Terminal t )
		throws ResourceUnavailableException, MethodNotSupportedException
	{
		t.addObserver( this );
	}

	private void removeObserver()
	{
		try
		{
			terminal.removeObserver( this );
		}
		catch ( Exception e )
		{
			report( e );
		}
	}

	/**
	 * 
	 */
	private void markOnline()
	{
		if (!online)
		{
			report( Trace.m( "terminal in service" ) );
			sendStatus( Status.DeviceOnline );
			online = true;
		}
	}

	/**
	 * 
	 */
	private void markOffline()
	{
		if (online)
		{
			report( Trace.m( "terminal out of service" ) );
			sendStatus( Status.DeviceOffline );
			online = false;
		}
	}

	/**
	 * Registers media caps if necessary.
	 * @param t
	 * @param addr 
	 * @param port 
	 * @param caps
	 * @throws CiscoRegistrationException
	 */
	abstract protected void registerMediaCaps( Terminal t, InetAddress addr,
		int port, CiscoMediaCapability[] caps ) throws CiscoRegistrationException;

	/**
	 * @param t
	 */
	abstract protected void unregisterMediaCaps( Terminal t );

	/**
	 * @return the terminal of this device.
	 */
	public Terminal getTerminal()
	{
		return terminal;
	}

	/**
	 * The terminal of this device.
	 */
	private Terminal terminal;

	/**
	 * The terminal is online or not.
	 */
	private boolean online;

	///////////////
	// ADDRESSES //
	///////////////

	/**
	 * Watches for calls on the specified addresses.
	 * @return true if at least one address was observed.
	 */
	private boolean observeAddresses()
	{
		Address[] addresses = terminal.getAddresses();
		boolean ok = false;
		if (addresses != null)
		{
			report( Trace.m( "observeAddresses: addresses=" ).a( addresses ) );
			int n = addresses.length;
			report( Trace.m( "observeAddresses: n=" ).a( n ) );
			for (int i = 0; i < n; i++)
			{
				report( Trace.m( "observeAddresses: found address=" ).a( addresses[i] ) );
				if (addAddress( addresses[i] ))
				{
					report( Trace.m( "observeAddresses: registered address=" ).a( addresses[i] ) );
					ok = true;
				}
				else
				{
					report( Trace.m( "observeAddresses: failed to register address=" ).a( addresses[i] ) );
				}
			}
		}
		report( Trace.m( "observeAddresses: ok=" ).a( ok ) );
		return ok;
	}

	/**
	 * 
	 */
	private void unobserveAddresses()
	{
		if (terminal == null)
			return;
		
		try
		{
			Address[] addresses = terminal.getAddresses();
			if (addresses != null)
			{
				int n = addresses.length;
				for (int i = 0; i < n; i++)
				{
					removeAddress( (CiscoAddress) addresses[i] );
				}
			}
		}
		catch ( RuntimeException e )
		{
			report( e );
		}
	}

	/**
	 * @param address
	 * @return true if the address was added.
	 */
	private boolean addAddress( Address address )
	{
		try
		{
			monitorTerminalAddress( (CiscoAddress) address );
			return true;
		}
		catch ( Exception e )
		{
			report( e );
			return false;
		}
	}

	/**
	 * @param address
	 */
	private void removeAddress( CiscoAddress address )
	{
		AddressMonitor am = removeFromAddrs( address );
		if (am != null)
		{
			report( Trace.m( "address.removeObserver... " ).a( address ).a( "@" ).a( terminal ) );
			address.removeObserver( am );
			report( Trace.m( "address.removeCallObserver... " ).a( address ).a( "@" ).a( terminal ) );
			address.removeCallObserver( am );
			report( Trace.m( "no longer monitoring " ).a( address ).a( "@" ).a( terminal ) );
		}
		else
		{
			report( Trace.m( "wasn't monitoring " ).a( address ).a( "@" ).a( terminal ) );
		}
	}

	/**
	 * @param address
	 * @throws Exception
	 */
	private void monitorTerminalAddress( CiscoAddress address )
		throws Exception
	{
		AddressMonitor am = addToAddrs( address );
		if (am != null)
		{
			// FIXME: use a todo to handle this!!!
			report( Trace.m( "address.addObserver... " ).a( address ).a( "@" ).a( terminal ) );
			address.addObserver( am );
			report( Trace.m( "address.addCallObserver... " ).a( address ).a( "@" ).a( terminal ) );
			address.addCallObserver( am );
			report( Trace.m( "monitoring " ).a( address ).a( "@" ).a( terminal ) );
		}
		else
		{
			report( Trace.m( "already monitoring " ).a( address ).a( "@" ).a( terminal ) );
		}
	}
	
	private AddressMonitor addToAddrs( CiscoAddress address )
	{
		String name = address.getName();
		synchronized (addrs)
		{
			if (!addrs.containsKey( name ))
			{
				AddressMonitor am = new AddressMonitor( map.cmMap, address, this );
				addrs.put( name, am );
				return am;
			}
			return null;
		}
	}
	
	private AddressMonitor removeFromAddrs( Address address )
	{
		String name = address.getName();
		synchronized (addrs)
		{
			return addrs.remove( name );
		}
	}

	private Map<String,AddressMonitor> addrs = new HashMap<String,AddressMonitor>();

	//////////////////////
	// TERMINAL CHANGED //
	//////////////////////

	/* (non-Javadoc)
	 * @see javax.telephony.TerminalObserver#terminalChangedEvent(javax.telephony.events.TermEv[])
	 */
	public void terminalChangedEvent( TermEv[] eventList )
	{
		int n = eventList.length;
//		report( "terminalChangedEvent: n = " + n );
		for (int i = 0; i < n; i++)
		{
			TermEv event = eventList[i];
			
			//			report( "got event " + i + ": " + event );
			switch (event.getID())
			{
				case CiscoTermInServiceEv.ID:
					ciscoTermInService( (CiscoTermInServiceEv) event );
					break;

				case CiscoTermOutOfServiceEv.ID:
					ciscoTermOutOfService( (CiscoTermOutOfServiceEv) event );
					break;

				case CiscoAddrCreatedEv.ID:
					ciscoAddrCreated( (CiscoAddrCreatedEv) event );
					break;

				case CiscoAddrRemovedEv.ID:
					ciscoAddrRemoved( (CiscoAddrRemovedEv) event );
					break;
				
				case CiscoRTPInputStartedEv.ID:
				{
					CiscoRTPInputStartedEv e = (CiscoRTPInputStartedEv) event;
					CallMonitor xcm = findCallMonitor( e.getCallID() );
					if (xcm != null)
						xcm.ciscoRTPInputStarted( e );
					else
						report( Trace.m( "ignored event: " ).a( event ) );
					break;
				}

				case CiscoRTPInputStoppedEv.ID:
				{
					CiscoRTPInputStoppedEv e = (CiscoRTPInputStoppedEv) event;
					CallMonitor xcm = findCallMonitor( e.getCallID() );
					if (xcm != null)
						xcm.ciscoRTPInputStopped( e );
					else
						report( Trace.m( "ignored event: " ).a( event ) );
					break;
				}

				case CiscoRTPOutputStartedEv.ID:
				{
					CiscoRTPOutputStartedEv e = (CiscoRTPOutputStartedEv) event;
					CallMonitor xcm = findCallMonitor( e.getCallID() );
					if (xcm != null)
						xcm.ciscoRTPOutputStarted( e );
					else
						report( Trace.m( "ignored event: " ).a( event ) );
					break;
				}

				case CiscoRTPOutputStoppedEv.ID:
				{
					CiscoRTPOutputStoppedEv e = (CiscoRTPOutputStoppedEv) event;
					CallMonitor xcm = findCallMonitor( e.getCallID() );
					if (xcm != null)
						xcm.ciscoRTPOutputStopped( e );
					else
						report( Trace.m( "ignored event: " ).a( event ) );
					break;
				}

				case TermObservationEndedEv.ID:
					termObservationEnded( (TermObservationEndedEv) event );
					break;

				default:
					//report( "terminalChangedEvent: event not handled: "+event );
					break;
			}
		}
	}
	
	/**
	 * @param callID
	 * @return blah
	 */
	public synchronized CallMonitor findCallMonitor( CiscoCallID callID )
	{
		CiscoCall call = callID.getCall();
		Connection[] connections = call.getConnections();
		int n = connections.length;
		for (int i = 0; i < n; i++)
		{
			CiscoConnection connection = (CiscoConnection) connections[i];
			int connId = connection.getConnectionID().intValue();
			Integer key = new Integer( connId );
			CallMonitor cm = conns.get( key );
			if (cm != null)
				return cm;
		}
		return null;
	}
	
	public synchronized void addCallMonitor( CallMonitor monitor, boolean active )
	{
		CiscoConnection conn = monitor.conn;
		int connId = conn.getConnectionID().intValue();
		Integer key = new Integer( connId );
		report( Trace.m( "call monitor established: " ).a( key ) );
		conns.put( key, monitor );
	}
	
	public synchronized void removeCallMonitor( CallMonitor monitor )
	{
		CiscoConnection conn = monitor.conn;
		if (conn == null)
			return;
		
		int connId = conn.getConnectionID().intValue();
		Integer key = new Integer( connId );
		report( Trace.m( "call monitor cleared: " ).a( key ) );
		conns.remove( key );
	}
	
	private Map<Integer,CallMonitor> conns = new HashMap<Integer,CallMonitor>();
	
	// private CallMonitor cm;

	/**
	 * @param e
	 */
	private void ciscoTermInService( CiscoTermInServiceEv e )
	{
		if (observeAddresses())
		{
			markOnline();
		}
	}

	/**
	 * @param e
	 */
	private void ciscoTermOutOfService( CiscoTermOutOfServiceEv e )
	{
		markOffline();
		unobserveAddresses();
	}

	/**
	 * @param e
	 */
	private void ciscoAddrCreated( CiscoAddrCreatedEv e )
	{
		report( Trace.m( "address created, address = " ).a( e.getAddress() ) );
		if (addAddress( e.getAddress() ))
			markOnline();
	}

	/**
	 * @param e
	 */
	private void ciscoAddrRemoved( CiscoAddrRemovedEv e )
	{
		report( Trace.m( "address removed, address = " ).a( e.getAddress() ) );
		removeAddress( (CiscoAddress) e.getAddress() );
		Address[] addresses = terminal.getAddresses();
		if (addresses == null || addresses.length == 0)
			markOffline();
	}

	/**
	 * @param e
	 */
	private void termObservationEnded( TermObservationEndedEv e )
	{
		terminal = null;
	}

	//////////
	// MISC //
	//////////

	/**
	 * @param from
	 * @return the address
	 */
	public Address getAddress( String from )
	{
		if (from != null)
		{
			try
			{
				Provider p = provider;
				if (p == null)
					return null;
				return p.getAddress( from );
			}
			catch ( Exception e )
			{
				report( e );
				return null;
			}
		}
		
		Address[] addresses = terminal.getAddresses();
		if (addresses == null || addresses.length == 0)
			return null;
		return addresses[0];
	}

	/**
	 * @param fromCallId
	 * @param toCallId
	 * @param fromAddress
	 * @param to
	 * @param rxIP
	 * @param rxPort
	 * @throws Exception
	 */
	public void makeCall( String fromCallId, String toCallId, Address fromAddress,
		String to, String rxIP, int rxPort ) throws Exception
	{
		CiscoCall call = (CiscoCall) provider.createCall();
		call.setObject( new CallData( fromCallId, toCallId, rxIP, rxPort ) );
		call.connect( terminal, fromAddress, to );
	}

	/**
	 * Sends device status to the client.
	 * @param status
	 * @see Status
	 */
	private void sendStatus( int status )
	{
		if (map.connection.isRunning())
			reportAndSend( newMessage( MsgType.StatusUpdate )
				.add( MsgField.Status, status ) );
	}

	/**
	 * @param m
	 */
	public void report( Trace.M m )
	{
		Trace.report( this, m );
	}

	/**
	 * @param m
	 * @param t
	 */
	public void report( Trace.M m, Throwable t )
	{
		Trace.report( this, m, t );
	}

	/**
	 * @param msg
	 */
	public void reportAndSend( FlatmapIpcMessage msg )
	{
		Trace.report( this, Trace.m( "sending message: " ).a( msg ) );
		msg.send();
	}

	/**
	 * @param msg
	 * @param t
	 */
	public void reportAndSend( FlatmapIpcMessage msg, Throwable t )
	{
		Trace.report( this, Trace.m( "sending message: " ).a( msg ), t );
		msg.send();
	}

	/**
	 * @param t
	 */
	public void report( Throwable t )
	{
		Trace.report( this, t );
	}

	/* (non-Javadoc)
	 * @see service.TerminalMonitor#newMessage(int)
	 */
	public FlatmapIpcMessage newMessage( int messageType )
	{
		return map.server.newMessage( messageType )
			.add( MsgField.DeviceType, getDeviceType() )
			.add( MsgField.DeviceName, deviceName );
	}

	/**
	 * @return true if device has a provider.
	 */
	public boolean hasProvider()
	{
		return provider != null;
	}
}