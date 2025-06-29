/* $Id: ProviderWrapper.java 31932 2007-05-02 20:06:03Z rvarghee $
 *
 * Created by wert on Feb 1, 2005
 *
 * Copyright (c) 2005 Metreos, Inc. All rights reserved.
 */

package metreos.service.jtapi;

import java.util.ArrayList;
import java.util.Collections;
import java.util.HashSet;
import java.util.Iterator;
import java.util.List;
import java.util.Set;

import javax.telephony.Terminal;
import javax.telephony.events.ProvEv;
import javax.telephony.events.ProvInServiceEv;
import javax.telephony.events.ProvObservationEndedEv;
import javax.telephony.events.ProvOutOfServiceEv;
import javax.telephony.events.ProvShutdownEv;

import metreos.util.Assertion;
import metreos.util.Trace;

import com.cisco.jtapi.extensions.CiscoAddrAddedToTerminalEv;
import com.cisco.jtapi.extensions.CiscoAddrCreatedEv;
import com.cisco.jtapi.extensions.CiscoAddrRemovedEv;
import com.cisco.jtapi.extensions.CiscoAddrRemovedFromTerminalEv;
import com.cisco.jtapi.extensions.CiscoAddress;
import com.cisco.jtapi.extensions.CiscoProvider;
import com.cisco.jtapi.extensions.CiscoProviderObserver;
import com.cisco.jtapi.extensions.CiscoSynchronousObserver;
import com.cisco.jtapi.extensions.CiscoTermCreatedEv;
import com.cisco.jtapi.extensions.CiscoTermRemovedEv;

/**
 * A wrapper for provider which keeps track of how many devices
 * there are using it.
 */
public class ProviderWrapper implements CiscoProviderObserver, CiscoSynchronousObserver
{
	/**
	 * Constructs a provider wrapper.
	 * @param key
	 * @param managers
	 * @param username
	 * @param password
	 * @param factory
	 * @param server
	 */
	public ProviderWrapper( String key, List<String> managers, String username,
		String password, ProviderFactory factory, JTapiServer server )
	{
//		this.key = key;
		this.managers = managers;
		this.username = username;
		this.password = password;
		this.factory = factory;
		this.server = server;
		providerStr = factory.getProviderString( managers.iterator(), username, password );
		
	}

//	private final String key;
	
	private final List<String> managers;
	
	private final String username;
	
	private final String password;
	
	private final ProviderFactory factory;
	
	@SuppressWarnings("unused")
	private final JTapiServer server;
	
	private final String providerStr;
	
	@Override
	public String toString()
	{
		return "ProviderWrapper( "+managers+", "+username+", "+password+" )";
	}
	
	private void openProvider() throws Exception
	{
		Trace.report( this, Trace.m( "looking for provider " ).a( providerStr ) );
		provider = factory.getProvider( providerStr );
		try
		{
			provider.addObserver( this );
		}
		catch ( Exception e )
		{
			provider.shutdown();
			provider = null;
			throw e;
		}
	}
	
	private void closeProvider()
	{
		Trace.report( this, "closeProvider" );
		shutdown();
	}

	/**
	 * Shuts down the provider if there is one.
	 */
	public synchronized void shutdown()
	{
		if (provider != null)
			provider.shutdown();
	}
	
	private CiscoProvider provider;
	
//	public void run()
//	{
//		running = true;
//		while (running)
//		{
//			try
//			{
//				synchronized( this )
//				{
//					try
//					{
//						wait( 4000 );
//					}
//					catch ( InterruptedException e1 )
//					{
//						// ignore
//					}
//				}
//				doit();
//			}
//			catch ( Exception e )
//			{
//				Trace.report( this, e );
//			}
//		}
//		closeProvider();
//	}
	
//	/**
//	 * Looks for things to do.
//	 */
//	private void doit()
//	{
//		int n = devices.size();
//		if (n > 0)
//			openProvider();
//		else
//			closeProvider();
//	}
	
//	private synchronized Provider openProvider()
//	{
//		if (provider == null)
//		{
//			String providerStr = factory.getProviderString( managers.iterator(), username, password );
//			try
//			{
//				Trace.report( this, Trace.m( "looking for provider " ).a( providerStr ) );
//				provider = factory.getProvider( providerStr );
//				String version = provider.getVersion();
//				Trace.report( this, Trace.m( "provider found, version " ).a( version ) );
//				provider.addObserver( this );
//			}
//			catch ( Exception e )
//			{
//				Trace.report( this, e );
//				
//				FlatmapIpcMessage msg = server.newMessage( MsgType.Error )
//					.add( MsgField.FailReason, FailReason.BadProvider )
//					.add( MsgField.Username, username )
//					.add( MsgField.Password, password )
//					.add( MsgField.Message, e.toString() );
//				
//				Iterator<String> i = managers.iterator();
//				while (i.hasNext())
//					msg.add( MsgField.CtiManager, i.next() );
//				
//				server.report( msg.sendNE() );
//			}
//		}
//		return provider;
//	}
	
//	private void closeProvider()
//	{
//		Provider p;
//		
//		synchronized (this)
//		{
//			p = provider;
//			provider = null;
//		}
//		
//		if (p != null)
//		{
//			p.shutdown();
//		}
//	}

	////////////////////////////
	// DEVICE LIST MANAGEMENT //
	////////////////////////////

	/**
	 * Adds a listener for events on this provider. Called when a device
	 * is opened.
	 * 
	 * @param device
	 * @throws Exception 
	 * 
	 * @see Device#open()
	 */
	public synchronized void addListener( Device device ) throws Exception
	{
		boolean ok = devices.add( device );
		if (!ok)
		{
			Trace.report( this, Trace.m( "duplicate device for addListener " ).a( device ) );
			return;
		}
		
		Trace.report( this, Trace.m( "addListener added device " ).a( device ) );

		if (provider == null)
		{
			try
			{
				openProvider();
			}
			catch ( Exception e )
			{
				devices.remove( device );
				//Trace.report( this, e );
				throw new Exception( "addListener could not open provider "+providerStr+": "+e, e );
			}
		}
		else if (providerInService)
		{
			providerInService( device );
		}
	}

	/**
	 * Removes a listener for events on this provider. Called when a device
	 * is closed.
	 * 
	 * @param device
	 * 
	 * @see Device#close()
	 */
	public synchronized void removeListener( Device device )
	{
		boolean ok = devices.remove( device );
		if (!ok)
		{
			Trace.report( this, Trace.m( "ignoring removeListener " ).a( device ) );
			return;
		}
		
		Trace.report( this, Trace.m( "removeListener worked for " ).a( device ) );
		
		cleanup( device );
		
		if (devices.size() == 0)
			closeProvider();
	}

	/**
	 * Gets an iterator over the devices.
	 * @return an iterator over the devices.
	 */
	private Iterator<Device> getDevices()
	{
		return new ArrayList<Device>( devices ).iterator();
	}
	
	/**
	 * ProviderWrapper::getDevice
	 * --------------------------
	 * 
	 * Get the device when the device type is unknown. This will be the typical
	 * case when the Terminal Created events are received from JTAPI provider.
	 * 
	 *  - get the deviceMaps and look for the device that matches the 
	 *    device name
	 * 
	 * TBD: Consider flatening the device Map since the device name is unique
	 * in CCM for different types. Ex. You cannot have the same device name 
	 * for a CTI Route point and a CTI port 
	 * 
	 * @param deviceName
	 * 
	 * @return Device
	 */
	public Device getDevice(String deviceName)
	{
		Device device;
		// Iterate through the maps and find the device.
		Iterator<DeviceMap> i = server.getDeviceMaps();
		
		while (i.hasNext()) {
			
			// Check if the device exist in the map
			device = i.next().get(deviceName);
			if (device != null)
			{
				return device;
			}	
		}
		
		// didnt find the device in any of the maps
		return null;
	}
	
	private final Set<Device> devices = Collections.synchronizedSet( new HashSet<Device>() );

	/* (non-Javadoc)
	 * @see javax.telephony.ProviderObserver#providerChangedEvent(javax.telephony.events.ProvEv[])
	 */
	public void providerChangedEvent( ProvEv[] eventList )
	{
		int n = eventList.length;
		for (int i = 0; i < n; i++)
		{
			ProvEv event = eventList[i];
			try
			{
				Trace.report( this, Trace.m( "---- providerChangedEvent: Received " ).a( event ) );
				providerChangedEvent( event );
			}
			catch ( RuntimeException e )
			{
				Trace.report( this, Trace.m( "caught exception delivering " ).a( event ), e );
			}
		}
	}
	
	private void providerChangedEvent( ProvEv event )
	{
		switch ( event.getID())
		{
			case ProvInServiceEv.ID:
				provInService( (ProvInServiceEv) event );
				break;
				
			case ProvOutOfServiceEv.ID:
				provOutOfService( (ProvOutOfServiceEv) event );
				break;
			
			case ProvShutdownEv.ID:
				provShutdown( (ProvShutdownEv) event );
				break;
			
			case ProvObservationEndedEv.ID:
				provObservationEnded( (ProvObservationEndedEv) event );
				break;
			
			case CiscoAddrAddedToTerminalEv.ID:
				ciscoAddrAddedToTerminal( (CiscoAddrAddedToTerminalEv) event );
				break;
			
			case CiscoAddrRemovedFromTerminalEv.ID:
				ciscoAddrRemovedFromTerminal( (CiscoAddrRemovedFromTerminalEv) event );
				break;
			
			case CiscoAddrCreatedEv.ID:
				ciscoAddrCreated( (CiscoAddrCreatedEv) event );
				break;
			
			case CiscoAddrRemovedEv.ID:
				ciscoAddrRemoved( (CiscoAddrRemovedEv) event );
				break;
				
			case CiscoTermCreatedEv.ID:
				ciscoTermCreated( (CiscoTermCreatedEv) event );
				break;
			
			case CiscoTermRemovedEv.ID:
				ciscoTermRemoved( (CiscoTermRemovedEv) event );
				break;
			
			default:
				Trace.report( this, Trace.m( "---- providerChangedEvent: event not handled: " ).a( event ) );
				break;
		}
	}

	private void provInService( ProvInServiceEv ev )
	{
		String version = provider.getVersion();
		
		Trace.report( this, Trace.m( "provInService: Version: " ).a( version ) );
	
		providerInService = true;
		fireProvInServiceEv( ev );
	}

	private void provOutOfService( ProvOutOfServiceEv ev )
	{
		providerInService = false;
		fireProvOutOfServiceEv( ev );
	}

	private void provShutdown( ProvShutdownEv ev )
	{
		providerInService = false;
		fireProvShutdownEv( ev );
	}

	private void provObservationEnded( ProvObservationEndedEv ev )
	{
		Assertion.check( ev.getProvider() == provider, "ev.getProvider() == provider" );
		provider.removeObserver( this );
		provider = null;
	}

	private void ciscoAddrAddedToTerminal( CiscoAddrAddedToTerminalEv ev )
	{
		Trace.report( this, Trace.m( "ignoring CiscoAddrAddedToTerminalEv" )
			.a( ", address " ).a( ev.getAddress() )
			.a( ", terminal " ).a( ev.getTerminal() ) );
	}

	private void ciscoAddrRemovedFromTerminal( CiscoAddrRemovedFromTerminalEv ev )
	{
		Trace.report( this, Trace.m( "ignoring CiscoAddrRemovedFromTerminalEv" )
			.a( ", address " ).a( ev.getAddress() )
			.a( ", terminal " ).a( ev.getTerminal() ) );
	}

	/**
	 * ProviderWrapper::ciscoAddrCreated
	 * ---------------------------------
	 * 
	 * ciscoAddrCreatedEv is received when an address is created on the 
	 * device belonging to this provider. Handling of this event ensures
	 * that we do not have to restart the provider to start observing the
	 * events on this address.
	 * 
	 *  - find the terminals on which this address was added.
	 *  - delegate the request to its respective device
	 * 
	 * @param ev 
	 */
	private void ciscoAddrCreated( CiscoAddrCreatedEv ev )
	{
		Trace.report( this, Trace.m( "CiscoAddrCreatedEv " ).a( " address " ).a( ev.getAddress() ) );
	     
		try
		{
			Terminal [] xterms = ev.getAddress().getTerminals();
			if (xterms != null)
			{
				int n = xterms.length;
				for (int i = 0; i < n; i++)
				{				
					// get the device from the device map and delegate the event
					Device dm = getDevice( xterms[i].getName() );
					if (dm != null)
					{
						Trace.report( this, Trace.m( "CiscoAddrCreatedEv" ).a( ", address [" ).a( ev.getAddress() ).a("] on Device ").a(dm.getDeviceName()) );
						dm.addAddress( ev.getAddress() );
					}
					else 
					{
						Trace.report( this, Trace.m("ProviderWrapper::CiscoAddrCreated: Device is NULL") );
					}				
				}
			}
		}
		catch ( RuntimeException e )
		{
			Trace.report( e );
		}	
	}


	/**
	 * ProviderWrapper::ciscoAddrRemoved
	 * ---------------------------------
	 * 
	 * ciscoAddrRemovedEv is received when an address is removed from the 
	 * device belonging to this provider. Handling of this event ensures
	 * that we do not have to restart the provider to stop observing the
	 * events on this address.
	 * 
	 *  - find the terminals from which this address was removed.
	 *  - delegate the request to its respective device
	 *  
	 *  Note: Currently there is a bug in the CISCO JTAPI jar which does
	 *  not provide the list of terminals.
	 * 
	 * @param ev 
	 */	
	private void ciscoAddrRemoved(CiscoAddrRemovedEv ev) {
		
		Trace.report(this, Trace.m("CiscoAddrRemovedEv ").a(" address ").a(ev.getAddress()));

		try {
			
			// Get the terminals that this address was attached with.
			Terminal[] xterms = ev.getAddress().getTerminals();
			if (xterms != null) {
				
				int n = xterms.length;
				for (int i = 0; i < n; i++) {
					
					// get the device from the device map and delegate the event
					Device dm = getDevice(xterms[i].getName());
					if (dm != null) {
						
						Trace.report(this, Trace.m("CiscoAddrRemovedEv").a(", address [").a(ev.getAddress()).a(
								"] on Device ").a(dm.getDeviceName()));
						
						// Remove the address from the device
						dm.removeAddress((CiscoAddress) ev.getAddress());
						
					} else {
						
						Trace.report(this, Trace.m("ProviderWrapper::CiscoAddrRemoved: Device is NULL"));
					}
				}
			} else {
				
				Trace.report(this, Trace.m("CiscoAddrRemoved: Failed to get Terminals"));
			}
		} catch (RuntimeException e) {
			
			Trace.report(e);
		}

	}

	/**
	 * ProviderWrapper::ciscoTermCreated
	 * ---------------------------------
	 * 
	 * ciscoTermCreatedEv is received when an terminal is created on this 
	 * provider. Handling of this event ensures that we do not have to 
	 * restart the provider to start observing the events on this terminal.
	 * 
	 *  - find the terminal.
	 *  - delegate the request to the device
	 * 
	 * @param ev 
	 */
	private void ciscoTermCreated( CiscoTermCreatedEv ev )
	{
		Terminal term = ev.getTerminal();
		
		Trace.report( this, Trace.m( "ProviderWrapper::ciscoTermCreated: " ).a( term.getName() ) );

		// get the device from the device map and delegate the event
		Device dm = getDevice( term.getName() );
		if (dm != null)
		{
			dm.ciscoTermCreated( ev );
		}
		else 
		{
			Trace.report( this, Trace.m("ProviderWrapper::CiscoTermCreated: Device is NULL") );
		}				
	}

	/**
	 * ProviderWrapper::ciscoTermRemoved
	 * ---------------------------------
	 * 
	 * ciscoTermRemovedEv is received when an terminal is removed from this 
	 * provider. Handling of this event ensures that we do not have to 
	 * restart the provider to stop observing the events on this terminal.
	 * 
	 *  - find the terminal.
	 *  - delegate the request to the device
	 * 
	 * @param ev 
	 */	
	private void ciscoTermRemoved( CiscoTermRemovedEv ev )
	{
		Terminal term = ev.getTerminal();
		
		Trace.report( this, Trace.m("CiscoTermRemoved: ").a( term.getName()) );

		// get the device from the device map and delegate the event
		Device dm = getDevice( term.getName() );
		if (dm != null)
		{
			dm.ciscoTermRemoved( ev );
		}
		else 
		{
			Trace.report( this, Trace.m("CiscoTermRemoved: Device is NULL") );
		}	

	}

	private boolean providerInService;

	/////////////////////////////////////
	// NOTIFY ALL DEVICES ABOUT EVENTS //
	/////////////////////////////////////

	/**
	 * @param ev
	 */
	private void fireProvInServiceEv( ProvInServiceEv ev )
	{
		Assertion.check( ev.getProvider() == provider, "ev.getProvider() == provider" );
		Iterator<Device> i = getDevices();
		
		while (i.hasNext())
		{
			providerInService( i.next() );
		}
	}

	/**
	 * @param ev
	 */
	private void fireProvOutOfServiceEv( ProvOutOfServiceEv ev )
	{
		Assertion.check( ev.getProvider() == provider, "ev.getProvider() == provider" );
		Iterator<Device> i = getDevices();
		while (i.hasNext())
			providerOutOfService( i.next() );
	}

	/**
	 * @param ev
	 */
	private void fireProvShutdownEv( ProvShutdownEv ev )
	{
		Assertion.check( ev.getProvider() == provider, "ev.getProvider() == provider" );
		Iterator<Device> i = getDevices();
		while (i.hasNext())
			providerShutdown( i.next() );
	}

	//////////////////////////
	// DEVICE NOTIFICATIONS //
	//////////////////////////

	private void providerInService( Device device )
	{
		try
		{
			device.providerInService( provider );
		}
		catch ( RuntimeException e )
		{
			Trace.report( this, e );
		}
	}

	private void providerOutOfService( Device device )
	{
		try
		{
			device.providerOutOfService( provider );
		}
		catch ( RuntimeException e )
		{
			Trace.report( this, e );
		}
	}

	private void providerShutdown( Device device )
	{
		try
		{
			device.providerShutdown( provider );
		}
		catch ( RuntimeException e )
		{
			Trace.report( this, e );
		}
	}

	private void cleanup( Device device )
	{
		try
		{
			device.cleanup( provider );
		}
		catch ( RuntimeException e )
		{
			Trace.report( this, e );
		}
	}
}
