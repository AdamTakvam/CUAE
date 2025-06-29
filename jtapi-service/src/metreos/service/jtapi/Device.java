/* $Id: Device.java 32224 2007-05-16 21:26:24Z wert $
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
import com.cisco.jtapi.extensions.CiscoMediaOpenLogicalChannelEv;
import com.cisco.jtapi.extensions.CiscoProvider;
import com.cisco.jtapi.extensions.CiscoRTPHandle;
import com.cisco.jtapi.extensions.CiscoRTPInputStartedEv;
import com.cisco.jtapi.extensions.CiscoRTPInputStoppedEv;
import com.cisco.jtapi.extensions.CiscoRTPOutputStartedEv;
import com.cisco.jtapi.extensions.CiscoRTPOutputStoppedEv;
import com.cisco.jtapi.extensions.CiscoRTPParams;
import com.cisco.jtapi.extensions.CiscoRegistrationException;
import com.cisco.jtapi.extensions.CiscoTermButtonPressedEv;
import com.cisco.jtapi.extensions.CiscoTermCreatedEv;
import com.cisco.jtapi.extensions.CiscoTermEvFilter;
import com.cisco.jtapi.extensions.CiscoTermInServiceEv;
import com.cisco.jtapi.extensions.CiscoTermOutOfServiceEv;
import com.cisco.jtapi.extensions.CiscoTermRemovedEv;
import com.cisco.jtapi.extensions.CiscoTerminal;

/**
 * A telephony device.
 */
public abstract class Device implements
	TerminalMonitor
//	, CallObserver
//	, CallControlCallObserver
//	, MediaCallObserver
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
	 * 
	 * @param thirdParty monitor for third party call control.
	 */
	public Device( DeviceMap map, String deviceName, List<String> managers,
		String username, String password, boolean thirdParty )
	{
		this.map = map;
		this.deviceName = deviceName;
		this.managers = managers;
		this.username = username;
		this.password = password;
		this.thirdParty = thirdParty;
		this.addresses = null;
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
	
	/**
	 * This device is being monitored for third party call control.
	 */
	private final boolean thirdParty;
	
	/**
	 * The addresses (DNs) for this device
	 */
	private Address[] addresses;
	

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
	
	/**
	 * @return true if this is a third party monitored device.
	 */
	public boolean isThirdParty()
	{
		return thirdParty;
	}

	//////////////////////
	// PROVIDER WRAPPER //
	//////////////////////

	/**
	 * Opens the device, attempting to make it active. Called by device
	 * map when the device is registered by the application.
	 * @throws Exception 
	 */
	public void open() throws Exception
	{
		if (providerWrapper == null)
		{
			providerWrapper = map.server.getProviderWrapper( managers,
				username, password );
			try
			{
				providerWrapper.addListener( this );
			}
			catch ( Exception e )
			{
				providerWrapper = null;
				throw e;
			}
		}
	}

	/**
	 * Closes the device, terminating all operations. Called by
	 * device map when the device has been unregistered by the
	 * application. Also called when an existing device is re-registered
	 * by the application.
	 */
	public void close()
	{
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
	public void providerInService( CiscoProvider newProv )
	{
		Trace.report( this, "Rx providerInService from Provider for device: " + deviceName);
		setProvider( null, newProv );
	}

	/**
	 * Notifies the device that a current provider is out of service.
	 * @param oldProv
	 */
	public void providerOutOfService( CiscoProvider oldProv )
	{
		setProvider( oldProv, null );
	}

	/**
	 * Notifies the device that a current provider has been shutdown.
	 * @param oldProv
	 */
	public void providerShutdown( CiscoProvider oldProv )
	{
		setProvider( oldProv, null );
	}

	/**
	 * Changes the current provider.
	 * @param oldProv what we think our current provider should be.
	 * @param newProv what we want our new provider to be.
	 */
	private void setProvider( CiscoProvider oldProv, CiscoProvider newProv )
	{
		report( Trace.m( "setProvider: oldProv=" ).a( oldProv ).a( ", newProv=" ).a( newProv ) );
		if (provider == oldProv)
		{
			provider = newProv;
			providerChanged();
		}
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
	public CiscoProvider getProvider()
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
	 * Device::ciscoTermCreated
	 * ------------------------
	 * 
	 * ciscoTermCreatedEv is received when an terminal is created on this 
	 * provider. Handling of this event ensures that we do not have to 
	 * restart the provider to start observing the events on this terminal.
	 * 
	 *  - make termOnline
	 * 
	 * @param event
	 */
	public void ciscoTermCreated( CiscoTermCreatedEv event )
	{
		termOnline();
	}
	
	/**
	 * Device::ciscoTermRemoved
	 * ------------------------
	 * 
	 * ciscoTermRemovedEv is received when an terminal is removed from this 
	 * provider. Handling of this event ensures that we do not have to 
	 * restart the provider to stop observing the events on this terminal.
	 * 
	 *  - make termOffline (unobserve Address and remove Observer)
	 * 
	 * @param event 
	 */	
	public void ciscoTermRemoved( CiscoTermRemovedEv event )
	{
		termOffline();
	}
	
	
	private void termOnline()
	{
		report( Trace.m( "TermOnline:: About to register and add Observer" ) );
		
		// follow the same path as registration
		termOpen();
	}	
	
	private void termOffline()
	{
		report( Trace.m( "TermOffline:: About to unregister and remove Observer" ) );
		
		// follow the same path as unregistration
		termClose();
	}		
	
	
	
	/**
	 * Opens the terminal and listens for events.
	 */
	private void termOpen()
	{
		try
		{			
			report( Trace.m( "Device::termOpen: About to getTerminal from Provider - " ).a(deviceName) );
			
			Terminal t = provider.getTerminal( deviceName );
			
			// If the terminal is not associated
			// with the app user or if the password is incorrect, we 
			// will get a CTIERR_UNSPECIFIED exception.
			
			registerMediaCaps( t, map.server.getCiscoMediaCapabilies() );

			// addresses will be observed when the device comes online.
			
			terminal = t;
			
			addObserver( t );
			
			report( Trace.m( "Device::termOpen: Terminal ready to observe events - " ).a(deviceName) );
		}
		catch ( Exception e )
		{
			report( e );
			terminal = null;
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
		
		report( Trace.m( "Device::addObserver: About to add terminal Observer " ).a(deviceName) );
		t.addObserver( this );
		
		//t.addCallObserver( this );
		
		report( Trace.m( "Device::addObserver: Enabling button pressed events ... " ).a(deviceName) );
		CiscoTerminal ct = (CiscoTerminal) t;
		CiscoTermEvFilter ctef = ct.getFilter();
		ctef.setButtonPressedEnabled( true );
		ctef.setDeviceDataEnabled( true );
		ctef.setRTPEventsEnabled( true );
			
		ct.setFilter( ctef );	
		report( Trace.m( "Device::addObserver: Enabled button pressed events ... " ).a(deviceName) );
	}

	private void removeObserver()
	{
		try
		{
			report( Trace.m( "calling removeObserver ..." ) );
			terminal.removeObserver( this );
//			terminal.removeCallObserver( this );
			report( Trace.m( "calling removeObserver done" ) );
		}
		catch ( Exception e )
		{
			report( Trace.m( "caught exception calling removeObserver" ), e );
		}
	}
	
//	public void callChangedEvent( CallEv[] eventList )
//	{
//		int n = eventList.length;
//		Trace.report( this, Trace.m( "delivering " ).a( n ).a( " call events" ) );
//		for (int i = 0; i < n; i++)
//		{
//			CallEv event = eventList[i];
//			try
//			{
//				Trace.report( this, Trace.m( "delivering " ).a( event ) );
//				if (!callChangedEvent( event ))
//					Trace.report( this, Trace.m( "ignoring " ).a( event ) );
//			}
//			catch ( Exception e )
//			{
//				Trace.report( this, Trace.m( "caught exception delivering " ).a( event ), e );
//			}
//		}
//		Trace.report( this, Trace.m( "done delivering " ).a( n ).a( " call events" ) );
//	}
//	
//	private boolean callChangedEvent( CallEv event )
//	{
//		return false;
//	}

	/**
	 * 
	 */
	private void markOnline()
	{
		if (!online)
		{
			report( Trace.m( "markOnline: Terminal [").a(deviceName).a("] is online; Sending DeviceOnline to Appliance" ) );
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
	 * @param caps
	 * @throws CiscoRegistrationException
	 */
	abstract protected void registerMediaCaps( Terminal t,
		CiscoMediaCapability[] caps ) throws CiscoRegistrationException;

	/**
	 * @param t
	 */
	abstract protected void unregisterMediaCaps( Terminal t );
	
	/* (non-Javadoc)
	 * @see metreos.service.jtapi.TerminalMonitor#setRTPParams(com.cisco.jtapi.extensions.CiscoRTPHandle, com.cisco.jtapi.extensions.CiscoRTPParams)
	 */
	abstract public boolean setRTPParams( CiscoRTPHandle handle,
		CiscoRTPParams params ) throws Exception;

	/**
	 * @return the terminal of this device.
	 */
	public Terminal getTerminal()
	{
		return terminal;
	}
	
	public String getTerminalName()
	{
		return deviceName;
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
		addresses = terminal.getAddresses();
		boolean ok = false;
		if (addresses != null)
		{
			int n = addresses.length;
			report( Trace.m( "observeAddresses: Device [").a(deviceName).a("] has [").a( n ).a("] observeAddresses" ) );
			for (Address address: addresses)
			{
				report( Trace.m( "observeAddresses: Device [").a(deviceName).a("] Address: " ).a( address ) );
				if (addAddress( address ))
				{
					report( Trace.m( "observeAddresses: registered address=" ).a( address ) );
					ok = true;
				}
				else
				{
					report( Trace.m( "observeAddresses: failed to register address=" ).a( address ) );
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
			Address[] xaddrs = terminal.getAddresses();
			if (xaddrs != null)
			{
				int n = xaddrs.length;
				for (int i = 0; i < n; i++)
				{
					removeAddress( (CiscoAddress) xaddrs[i] );
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
	public boolean addAddress( Address address )
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
	public void removeAddress( CiscoAddress address )
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
			report( Trace.m( "Device::monitorTerminalAddress - address.addObserver... " ).a( address ).a( "@" ).a( terminal ) );
			address.addObserver( am );
			report( Trace.m( "Device::monitorTerminalAddress - address.addCallObserver... " ).a( address ).a( "@" ).a( terminal ) );
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
				AddressMonitor am = new AddressMonitor( map.cmMap, address,
					this, map.fakeIp, map.fakePort, thirdParty );
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
		for (int i = 0; i < n; i++)
		{
			TermEv event = eventList[i];
			try
			{
				report( Trace.m( "---- Device::terminalChangedEvent for [").a(deviceName).a("] Received " ).a( event ) );
				if (!terminalChangedEvent( event ))
					report( Trace.m( "*** ignored " ).a( event ) );
			}
			catch ( Exception e )
			{
				report( Trace.m( "caught exception delivering " ).a( event ), e );
			}
		}
	}
	
	private boolean terminalChangedEvent( TermEv event )
	{
		switch (event.getID())
		{
			case CiscoTermInServiceEv.ID:
				ciscoTermInService( (CiscoTermInServiceEv) event );
				return true;

			case CiscoTermOutOfServiceEv.ID:
				ciscoTermOutOfService( (CiscoTermOutOfServiceEv) event );
				return true;

			case CiscoAddrCreatedEv.ID:				
				ciscoAddrCreated( (CiscoAddrCreatedEv) event );
				return true;

			case CiscoAddrRemovedEv.ID:
				ciscoAddrRemoved( (CiscoAddrRemovedEv) event );
				return true;
			
			case CiscoTermButtonPressedEv.ID:
				return ciscoTermButtonPressed( (CiscoTermButtonPressedEv) event );
			
			case CiscoMediaOpenLogicalChannelEv.ID:
				return ciscoMediaOpenLogicalChannel( (CiscoMediaOpenLogicalChannelEv) event );
			
			case CiscoRTPInputStartedEv.ID:
				return ciscoRTPInputStarted( (CiscoRTPInputStartedEv) event );

			case CiscoRTPInputStoppedEv.ID:
				return ciscoRTPInputStopped( (CiscoRTPInputStoppedEv) event );

			case CiscoRTPOutputStartedEv.ID:
				return ciscoRTPOutputStarted( (CiscoRTPOutputStartedEv) event );

			case CiscoRTPOutputStoppedEv.ID:
				return ciscoRTPOutputStopped( (CiscoRTPOutputStoppedEv) event );

			case TermObservationEndedEv.ID:
				termObservationEnded( (TermObservationEndedEv) event );
				return true;

			default:
				return false;
		}
	}

	private boolean ciscoMediaOpenLogicalChannel( CiscoMediaOpenLogicalChannelEv ev )
	{
		CallMonitor xcm = findCallMonitor( ev.getCiscoRTPHandle() );
		if (xcm != null)
		{
			xcm.ciscoMediaOpenLogicalChannel( ev );
			return true;
		}
		return false;
	}

	private boolean ciscoRTPInputStarted( CiscoRTPInputStartedEv ev )
	{
		CallMonitor xcm = findCallMonitor( ev.getCallID() );
		if (xcm != null)
		{
			xcm.ciscoRTPInputStarted( ev );
			return true;
		}
		return false;
	}

	private boolean ciscoRTPInputStopped( CiscoRTPInputStoppedEv ev )
	{
		CallMonitor xcm = findCallMonitor( ev.getCallID() );
		if (xcm != null)
		{
			xcm.ciscoRTPInputStopped( ev );
			return true;
		}
		return false;
	}

	private boolean ciscoRTPOutputStarted( CiscoRTPOutputStartedEv ev )
	{
		CallMonitor xcm = findCallMonitor( ev.getCallID() );
		if (xcm != null)
		{
			xcm.ciscoRTPOutputStarted( ev );
			return true;
		}
		return false;
	}

	private boolean ciscoRTPOutputStopped( CiscoRTPOutputStoppedEv ev )
	{
		CallMonitor xcm = findCallMonitor( ev.getCallID() );
		if (xcm != null)
		{
			xcm.ciscoRTPOutputStopped( ev );
			return true;
		}
		return false;
	}

	private boolean ciscoTermButtonPressed( CiscoTermButtonPressedEv ev )
	{
		CallMonitor xcm = activeCallMonitor;
		if (xcm != null)
		{
			xcm.ciscoTermButtonPressed( ev );
			return true;
		}
		return false;
	}

	/**
	 * @param callID
	 * @return blah
	 */
	public synchronized CallMonitor findCallMonitor( CiscoCallID callID )
	{
		if (callID == null)
			return null;
		
		CiscoCall call = callID.getCall();
		Connection[] connections = call.getConnections();
		if (connections == null)
			return null;
		
		int n = connections.length;
		for (int i = 0; i < n; i++)
		{
			CiscoConnection connection = (CiscoConnection) connections[i];
			int connId = connection.getConnectionID().intValue();
			CallMonitor monitor = conns.get( connId );
			if (monitor != null)
				return monitor;
		}
		return null;
	}
	
	/**
	 * @param handle
	 * @return blah
	 */
	public synchronized CallMonitor findCallMonitor( CiscoRTPHandle handle )
	{
		int connId = handle.getHandle();
		CallMonitor monitor = conns.get( connId );
		report( Trace.m( "findCallMonitor" )
			.a( ", connId=" ).a( connId )
			.a( ", monitor=" ).a( monitor ) );
		return monitor;
	}
	
	public synchronized void addCallMonitor( CallMonitor monitor, boolean active )
	{
		CiscoConnection conn = monitor.conn;
		int connId = conn.getConnectionID().intValue();
		CallMonitor oldMonitor = conns.put( connId, monitor );
		report( Trace.m( "addCallMonitor" )
			.a( ", connId=" ).a( connId )
			.a( ", monitor=" ).a( monitor )
			.a( ", oldMonitor=" ).a( oldMonitor )
			.a( ", active=" ).a( active ) );
		if (active)
			activeCallMonitor = monitor;
	}
	
	public synchronized void removeCallMonitor( CallMonitor monitor )
	{
		CiscoConnection conn = monitor.conn;
		if (conn == null)
			return;
		
		int connId = conn.getConnectionID().intValue();
		if (conns.get( connId ) == monitor)
		{
			conns.remove( connId );
			report( Trace.m( "removeCallMonitor" )
				.a( ", connId=" ).a( connId )
				.a( ", monitor=" ).a( monitor ) );
			if (activeCallMonitor == monitor)
				activeCallMonitor = null;
		}
	}
	
	private Map<Integer,CallMonitor> conns = new HashMap<Integer,CallMonitor>();
	
	private CallMonitor activeCallMonitor;
	
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
		report( Trace.m( "Address created, address = " ).a( e.getAddress() ).a(" on Terminal ").a(this.deviceName) );
		if (addAddress( e.getAddress() ))
			markOnline();
	}

	/**
	 * @param e
	 */
	private void ciscoAddrRemoved( CiscoAddrRemovedEv e )
	{
		report( Trace.m( "Address Removed, address = " ).a( e.getAddress() ).a(" on Terminal ").a(this.deviceName) );
		removeAddress( (CiscoAddress) e.getAddress() );
		Address[] xaddrs = terminal.getAddresses();
		if (xaddrs == null || xaddrs.length == 0)
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
				CiscoProvider p = provider;
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
		
		Address[] xaddrs = terminal.getAddresses();
		if (xaddrs == null || xaddrs.length == 0)
			return null;
		return xaddrs[0];
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
		String to, InetAddress rxIP, int rxPort ) throws Exception
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
		if (map.connection.isRunning() && addresses != null && addresses.length > 0)
			reportAndSend( newMessage( MsgType.StatusUpdate )
				.add( MsgField.Status, status )
				.add( MsgField.From, addresses[0].toString()));
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

	/**
	 * The device is being removed from the provider's list.
	 * @param provider2
	 */
	public void cleanup( CiscoProvider provider2 )
	{
		setProvider( provider2, null );
	}
}