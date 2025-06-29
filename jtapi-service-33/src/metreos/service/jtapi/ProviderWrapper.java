/* $Id: ProviderWrapper.java 30152 2007-03-06 21:47:50Z wert $
 *
 * Created by wert on Feb 1, 2005
 *
 * Copyright (c) 2005 Metreos, Inc. All rights reserved.
 */

package metreos.service.jtapi;

import java.util.HashSet;
import java.util.Iterator;
import java.util.List;
import java.util.Set;
import java.util.Vector;

import javax.telephony.Provider;
import javax.telephony.ProviderObserver;
import javax.telephony.events.ProvEv;
import javax.telephony.events.ProvInServiceEv;
import javax.telephony.events.ProvObservationEndedEv;
import javax.telephony.events.ProvOutOfServiceEv;
import javax.telephony.events.ProvShutdownEv;

import metreos.util.Trace;



/**
 * A wrapper for provider which keeps track of how many devices
 * there are using it.
 */
public class ProviderWrapper extends Thread implements ProviderObserver
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
	}

//	private final String key;
	
	private final List<String> managers;
	
	private final String username;
	
	private final String password;
	
	private final ProviderFactory factory;
	
	private final JTapiServer server;

	private boolean running;
	
	/**
	 * Our current provider.
	 */
	Provider provider;
	
	@Override
	public void run()
	{
		running = true;
		while (running)
		{
			try
			{
				synchronized( this )
				{
					try
					{
						wait( 4000 );
					}
					catch ( InterruptedException e1 )
					{
						// ignore
					}
				}
				doit();
			}
			catch ( Exception e )
			{
				Trace.report( this, e );
			}
		}
		closeProvider();
	}
	
	/**
	 * Looks for things to do.
	 */
	private void doit()
	{
		int n = devices.size();
		if (n > 0)
			openProvider();
		else
			closeProvider();
	}
	
	private synchronized Provider openProvider()
	{
		if (provider == null)
		{
			Iterator<String> i = managers.iterator();
			while (i.hasNext())
			{
				String manager = i.next();
				String providerStr = factory.getProviderString( manager, username, password );
				try
				{
					provider = factory.getProvider( providerStr );
					provider.addObserver( ProviderWrapper.this );
				}
				catch ( Exception e )
				{
					Trace.report( this, e );
					server.reportAndSend( server.newMessage( MsgType.Error )
						.add( MsgField.FailReason, FailReason.BadProvider )
						.add( MsgField.CtiManager, manager )
						.add( MsgField.Username, username )
						.add( MsgField.Password, password )
						.add( MsgField.Message, e.toString() ) );
				}
			}
		}
		return provider;
	}
	
	private void closeProvider()
	{
		Provider p;
		
		synchronized (this)
		{
			p = provider;
			provider = null;
		}
		
		if (p != null)
		{
			p.shutdown();
		}
	}

	/**
	 * Adds a listener for events on this provider.
	 * @param device
	 */
	public void addListener( Device device )
	{
		synchronized (devices)
		{
			boolean ok = devices.add( device );
			Trace.report( this, Trace.m( "added device " ).a( device ).a( ": " ).a( ok ) );
			if (providerInService)
				device.providerInService( provider );
		}
	}

	/**
	 * Removes a listener for events on this provider.
	 * @param device
	 */
	public void removeListener( Device device )
	{
		synchronized (devices)
		{
			boolean ok = devices.remove( device );
			Trace.report( this, Trace.m( "removed device " ).a( device ).a( ": " ).a( ok ) );
		}
	}

	/**
	 * Gets an iterator over the devices.
	 * @return an iterator over the devices.
	 */
	private Iterator<Device> getDevices()
	{
		synchronized (devices)
		{
			return new Vector<Device>( devices ).iterator();
		}
	}
	
	private final Set<Device> devices = new HashSet<Device>();

	/* (non-Javadoc)
	 * @see javax.telephony.ProviderObserver#providerChangedEvent(javax.telephony.events.ProvEv[])
	 */
	public void providerChangedEvent( ProvEv[] eventList )
	{
		int n = eventList.length;
		for (int i = 0; i < n; i++)
		{
			ProvEv event = eventList[i];
			Trace.report( this, Trace.m( "providerChangedEvent: event " ).a( event ) );
			switch ( event.getID())
			{
				case ProvInServiceEv.ID:
					providerInService = true;
					fireProvInServiceEv( (ProvInServiceEv) event );
					break;
					
				case ProvOutOfServiceEv.ID:
					providerInService = false;
					fireProvOutOfServiceEv( (ProvOutOfServiceEv) event );
					break;
				
				case ProvShutdownEv.ID:
					fireProvShutdownEv( (ProvShutdownEv) event );
					break;
				
				case ProvObservationEndedEv.ID:
					((ProvObservationEndedEv) event).getProvider().removeObserver( this );
					break;
				
				default:
					Trace.report( this, Trace.m( "providerChangedEvent: event not handled: " ).a( event ) );
					break;
			}
		}
	}
	
	private boolean providerInService;

	/**
	 * @param ev
	 */
	private void fireProvInServiceEv( ProvInServiceEv ev )
	{
		Iterator<Device> i = getDevices();
		while (i.hasNext())
		{
			Device device = i.next();
			Trace.report( this, Trace.m( "calling providerInService on " ).a( device ) );
			device.providerInService( ev.getProvider() );
		}
	}

	/**
	 * @param ev
	 */
	private void fireProvOutOfServiceEv( ProvOutOfServiceEv ev )
	{
		Iterator<Device> i = getDevices();
		while (i.hasNext())
		{
			Device device = i.next();
			Trace.report( this, Trace.m( "calling providerOutOfService on " ).a( device ) );
			device.providerOutOfService( ev.getProvider() );
		}
	}

	/**
	 * @param ev
	 */
	private void fireProvShutdownEv( ProvShutdownEv ev )
	{
		Iterator<Device> i = getDevices();
		while (i.hasNext())
		{
			Device device = i.next();
			Trace.report( this, Trace.m( "calling providerShutdown on " ).a( device ) );
			device.providerShutdown( ev.getProvider() );
		}
	}

	/**
	 * Shuts down the provider if there is one.
	 */
	public void shutdown()
	{
		running = false;
	}
}