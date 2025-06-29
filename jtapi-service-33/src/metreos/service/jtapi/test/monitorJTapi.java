/* $Id: monitorJTapi.java 30152 2007-03-06 21:47:50Z wert $
 * 
 * Created by wert on Jan 25, 2005
 * 
 * Copyright (c) 2005 Metreos, Inc. All rights reserved.
 */

package metreos.service.jtapi.test;

import java.util.Hashtable;
import java.util.Map;

import javax.telephony.Address;
import javax.telephony.JtapiPeerFactory;
import javax.telephony.MethodNotSupportedException;
import javax.telephony.ProviderObserver;
import javax.telephony.ResourceUnavailableException;
import javax.telephony.Terminal;
import javax.telephony.events.ProvEv;
import javax.telephony.events.ProvInServiceEv;
import javax.telephony.events.ProvOutOfServiceEv;
import javax.telephony.events.ProvShutdownEv;

import metreos.service.jtapi.AddressMonitor;
import metreos.service.jtapi.CallMonitorMap;
import metreos.service.jtapi.PlainTerminalMonitor;
import metreos.service.jtapi.TerminalMonitor;
import metreos.util.Monitor;
import metreos.util.Trace;


import com.cisco.jtapi.extensions.CiscoAddress;
import com.cisco.jtapi.extensions.CiscoJtapiPeer;
import com.cisco.jtapi.extensions.CiscoProvider;
import com.cisco.jtapi.extensions.CiscoTerminal;

/**
 * Test the JTapi interfaces / impl.
 */
public class monitorJTapi implements ProviderObserver
{
	private static final String PROV_ONLINE = "online";

	private static final Object PROV_OFFLINE = "offline";

	private static final Object PROV_SHUTDOWN = "shutdown";

	private static final String WILDCARD = "*";

	/**
	 * The main program.
	 * 
	 * @param args
	 * @throws Exception
	 */
	public static void main( String[] args ) throws Exception
	{
		String device;
		if (args.length > 0)
			device = args[0];
		else
			device = WILDCARD;

		String line;
		if (args.length > 1)
			line = args[1];
		else
			line = WILDCARD;

		new monitorJTapi().doit( device, line );
	}

	private CiscoProvider provider;

	private Monitor providerStatus = new Monitor( "provider status" );

	private void report( String msg )
	{
		Trace.report( this, Trace.m( msg ) );
	}
	
	private void doit( String device, String line ) throws Exception
	{
		CiscoJtapiPeer peer = (CiscoJtapiPeer) JtapiPeerFactory
			.getJtapiPeer( "" );

		String host = "10.1.10.25";
		String login = "wert";
		String passwd = "metreos";

		String providerString = host + ";login=" + login + ";passwd=" + passwd;

		report( "Opening " + providerString + "..." );
		provider = (CiscoProvider) peer.getProvider( providerString );

		provider.addObserver( this );

		report( "waiting for provider online..." );
		providerStatus.waitUntilEq( PROV_ONLINE, 5000 );
		report( "provider online." );

		//		ProviderCapabilities caps = provider.getCapabilities();
		//		AddressCapabilities addrCaps = provider.getAddressCapabilities();
		//		CallCapabilities callCaps = provider.getCallCapabilities();
		//		ConnectionCapabilities connCaps =
		// provider.getConnectionCapabilities();
		//		ProviderCapabilities provCaps = provider.getProviderCapabilities();
		//		TerminalCapabilities termCaps = provider.getTerminalCapabilities();
		//		TerminalConnectionCapabilities termConnCaps =
		// provider.getTerminalConnectionCapabilities();

		//		Trace.report( this, "Capabilities = " + caps );
		//		Trace.report( this, "AddressCapabilities = " + addrCaps );
		//		Trace.report( this, "CallCapabilities = " + callCaps );
		//		Trace.report( this, "ConnectionCapabilities = " + connCaps );
		//		Trace.report( this, "ProviderCapabilities = " + provCaps );
		//		Trace.report( this, "TerminalCapabilities = " + termCaps );
		//		Trace.report( this, "TerminalConnectionCapabilities = " +
		// termConnCaps );

		if (device.equals( WILDCARD ) && line.equals( WILDCARD ))
		{
			Address[] addresses = provider.getAddresses();
			if (addresses != null)
			{
				report( "addresses.length = " + addresses.length );
				for (int i = 0; i < addresses.length; i++)
				{
					report( "address = " + addresses[i] );
				}
			}
			else
				report( "no addresses" );

			Terminal[] terminals = provider.getTerminals();
			if (terminals != null)
			{
				report( "terminals.length = " + terminals.length );
				for (int i = 0; i < terminals.length; i++)
				{
					report( "terminal = " + terminals[i] );
				}
			}
			else
				report( "no terminals" );

			shutdown( 0 );
		}

		CallMonitorMap cmDb = new CallMonitorMap();

		if (device.equals( WILDCARD ))
		{
			CiscoAddress address = (CiscoAddress) provider.getAddress( line );
			Terminal[] terminals = address.getTerminals();
			int n = terminals.length;
			for (int i = 0; i < n; i++)
				monitorTerminalAddress( cmDb, (CiscoTerminal) terminals[i],
					address );
		}
		else if (line.equals( WILDCARD ))
		{
			CiscoTerminal terminal = (CiscoTerminal) provider
				.getTerminal( device );
			Address[] addresses = terminal.getAddresses();
			int n = addresses.length;
			for (int i = 0; i < n; i++)
				monitorTerminalAddress( cmDb, terminal,
					(CiscoAddress) addresses[i] );
		}
		else
		{
			CiscoTerminal terminal = (CiscoTerminal) provider
				.getTerminal( device );
			CiscoAddress address = (CiscoAddress) provider.getAddress( line );
			monitorTerminalAddress( cmDb, terminal, address );
		}

		Thread.sleep( 300 * 1000 );

		shutdown( 0 );

	}

	/**
	 * @param terminal
	 * @param address
	 * @throws MethodNotSupportedException
	 * @throws ResourceUnavailableException
	 */
	private void monitorTerminalAddress( CallMonitorMap cmDb,
		CiscoTerminal terminal, CiscoAddress address )
		throws ResourceUnavailableException, MethodNotSupportedException
	{
		report( "monitoring " + terminal + "@" + address );
		TerminalMonitor tm = getTerminalMonitor( terminal );
		AddressMonitor cm = new AddressMonitor( cmDb, address, tm );
		address.addObserver( cm );
		address.addCallObserver( cm );

	}

	/**
	 * @param terminal
	 * @return the terminal monitor for this terminal. creates one if there
	 * isn't one already.
	 * @throws MethodNotSupportedException
	 * @throws ResourceUnavailableException
	 */
	private synchronized TerminalMonitor getTerminalMonitor(
		CiscoTerminal terminal ) throws ResourceUnavailableException,
		MethodNotSupportedException
	{
		String name = terminal.getName();
		TerminalMonitor tm = terminalMonitors.get( name );
		if (tm == null)
		{
			tm = new PlainTerminalMonitor( name, terminal );
			terminalMonitors.put( name, tm );
			terminal.addObserver( tm );
		}
		return tm;
	}

	private Map<String,TerminalMonitor> terminalMonitors = new Hashtable<String,TerminalMonitor>();

	private void shutdown( int code ) throws InterruptedException
	{
		report( "shutting down..." );
		provider.shutdown();
		report( "waiting for shut down..." );
		providerStatus.waitUntilEq( PROV_SHUTDOWN, 5000 );
		report( "shut down." );
		System.exit( code );
	}

	/*
	 * (non-Javadoc)
	 * 
	 * @see javax.telephony.ProviderObserver#providerChangedEvent(javax.telephony.events.ProvEv[])
	 */
	public void providerChangedEvent( ProvEv[] eventList )
	{
		int n = eventList.length;
		for (int i = 0; i < n; i++)
		{
			ProvEv event = eventList[i];
			switch (event.getID())
			{
				case ProvInServiceEv.ID:
					report( "provider is online" );
					providerStatus.set( PROV_ONLINE );
					break;

				case ProvOutOfServiceEv.ID:
					report( "provider is offline" );
					providerStatus.set( PROV_OFFLINE );
					break;

				case ProvShutdownEv.ID:
					report( "provider is shutdown" );
					providerStatus.set( PROV_SHUTDOWN );
					break;

				default:
					report( "event not handled: " + event );
					break;
			}
		}
	}

	@Override
	public String toString()
	{
		return "monitorJTapi";
	}
}