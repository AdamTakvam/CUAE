/* $Id$
 *
 * Created by wert on Jan 25, 2007.
 *
 * Copyright (c) 2007 Cisco Systems. All rights reserved.
 */

package metreos.service.jtapi.test;

import java.net.InetAddress;
import java.text.DateFormat;
import java.text.SimpleDateFormat;
import java.util.ArrayList;
import java.util.Collections;
import java.util.Date;
import java.util.HashMap;
import java.util.HashSet;
import java.util.Iterator;
import java.util.List;
import java.util.Map;
import java.util.Set;

import javax.telephony.Address;
import javax.telephony.AddressObserver;
import javax.telephony.CallObserver;
import javax.telephony.Connection;
import javax.telephony.InvalidArgumentException;
import javax.telephony.InvalidStateException;
import javax.telephony.JtapiPeerFactory;
import javax.telephony.MethodNotSupportedException;
import javax.telephony.PrivilegeViolationException;
import javax.telephony.ProviderObserver;
import javax.telephony.ResourceUnavailableException;
import javax.telephony.Terminal;
import javax.telephony.TerminalConnection;
import javax.telephony.TerminalObserver;
import javax.telephony.callcontrol.CallControlCallObserver;
import javax.telephony.callcontrol.events.CallCtlCallEv;
import javax.telephony.callcontrol.events.CallCtlConnAlertingEv;
import javax.telephony.callcontrol.events.CallCtlConnDisconnectedEv;
import javax.telephony.callcontrol.events.CallCtlConnEstablishedEv;
import javax.telephony.callcontrol.events.CallCtlConnEv;
import javax.telephony.callcontrol.events.CallCtlConnOfferedEv;
import javax.telephony.callcontrol.events.CallCtlEv;
import javax.telephony.callcontrol.events.CallCtlTermConnDroppedEv;
import javax.telephony.callcontrol.events.CallCtlTermConnEv;
import javax.telephony.callcontrol.events.CallCtlTermConnRingingEv;
import javax.telephony.callcontrol.events.CallCtlTermConnTalkingEv;
import javax.telephony.events.AddrEv;
import javax.telephony.events.AddrObservationEndedEv;
import javax.telephony.events.CallActiveEv;
import javax.telephony.events.CallEv;
import javax.telephony.events.CallInvalidEv;
import javax.telephony.events.CallObservationEndedEv;
import javax.telephony.events.ConnAlertingEv;
import javax.telephony.events.ConnConnectedEv;
import javax.telephony.events.ConnCreatedEv;
import javax.telephony.events.ConnDisconnectedEv;
import javax.telephony.events.ConnEv;
import javax.telephony.events.ConnFailedEv;
import javax.telephony.events.ConnInProgressEv;
import javax.telephony.events.ConnUnknownEv;
import javax.telephony.events.Ev;
import javax.telephony.events.ProvEv;
import javax.telephony.events.ProvInServiceEv;
import javax.telephony.events.ProvObservationEndedEv;
import javax.telephony.events.ProvOutOfServiceEv;
import javax.telephony.events.ProvShutdownEv;
import javax.telephony.events.TermConnActiveEv;
import javax.telephony.events.TermConnCreatedEv;
import javax.telephony.events.TermConnDroppedEv;
import javax.telephony.events.TermConnEv;
import javax.telephony.events.TermConnPassiveEv;
import javax.telephony.events.TermConnRingingEv;
import javax.telephony.events.TermConnUnknownEv;
import javax.telephony.events.TermEv;
import javax.telephony.events.TermObservationEndedEv;

import com.cisco.jtapi.CallManagerObserver;
import com.cisco.jtapi.extensions.CiscoAddrAddedToTerminalEv;
import com.cisco.jtapi.extensions.CiscoAddrAutoAcceptStatusChangedEv;
import com.cisco.jtapi.extensions.CiscoAddrCreatedEv;
import com.cisco.jtapi.extensions.CiscoAddrEv;
import com.cisco.jtapi.extensions.CiscoAddrInServiceEv;
import com.cisco.jtapi.extensions.CiscoAddrOutOfServiceEv;
import com.cisco.jtapi.extensions.CiscoAddrRemovedEv;
import com.cisco.jtapi.extensions.CiscoAddrRemovedFromTerminalEv;
import com.cisco.jtapi.extensions.CiscoAddress;
import com.cisco.jtapi.extensions.CiscoAddressObserver;
import com.cisco.jtapi.extensions.CiscoCall;
import com.cisco.jtapi.extensions.CiscoCallEv;
import com.cisco.jtapi.extensions.CiscoCallID;
import com.cisco.jtapi.extensions.CiscoConnection;
import com.cisco.jtapi.extensions.CiscoConnectionID;
import com.cisco.jtapi.extensions.CiscoEv;
import com.cisco.jtapi.extensions.CiscoG711MediaCapability;
import com.cisco.jtapi.extensions.CiscoJtapiPeer;
import com.cisco.jtapi.extensions.CiscoJtapiVersion;
import com.cisco.jtapi.extensions.CiscoMediaCapability;
import com.cisco.jtapi.extensions.CiscoMediaOpenLogicalChannelEv;
import com.cisco.jtapi.extensions.CiscoMediaTerminal;
import com.cisco.jtapi.extensions.CiscoOutOfServiceEv;
import com.cisco.jtapi.extensions.CiscoProvEv;
import com.cisco.jtapi.extensions.CiscoProvider;
import com.cisco.jtapi.extensions.CiscoProviderObserver;
import com.cisco.jtapi.extensions.CiscoRTPHandle;
import com.cisco.jtapi.extensions.CiscoRTPInputProperties;
import com.cisco.jtapi.extensions.CiscoRTPInputStartedEv;
import com.cisco.jtapi.extensions.CiscoRTPInputStoppedEv;
import com.cisco.jtapi.extensions.CiscoRTPOutputProperties;
import com.cisco.jtapi.extensions.CiscoRTPOutputStartedEv;
import com.cisco.jtapi.extensions.CiscoRTPOutputStoppedEv;
import com.cisco.jtapi.extensions.CiscoRTPParams;
import com.cisco.jtapi.extensions.CiscoRouteTerminal;
import com.cisco.jtapi.extensions.CiscoTermCreatedEv;
import com.cisco.jtapi.extensions.CiscoTermEv;
import com.cisco.jtapi.extensions.CiscoTermInServiceEv;
import com.cisco.jtapi.extensions.CiscoTermOutOfServiceEv;
import com.cisco.jtapi.extensions.CiscoTermRemovedEv;
import com.cisco.jtapi.extensions.CiscoTerminal;
import com.cisco.jtapi.extensions.CiscoTerminalConnection;
import com.cisco.jtapi.extensions.CiscoTerminalObserver;
import com.cisco.jtapi.extensions.CiscoUnregistrationException;
import com.sun.org.apache.bcel.internal.generic.CPInstruction;

@SuppressWarnings("all")
public class testJtapi4
{
	public static void main( String[] args ) throws Exception
	{
		log( INFO, "jtapi version %s\n", new CiscoJtapiVersion() );
		CiscoJtapiPeer jtapiPeer = (CiscoJtapiPeer) JtapiPeerFactory.getJtapiPeer( null );
		ProviderFactory factory = new ProviderFactory( jtapiPeer );
		
		List<String> managers = new ArrayList<String>();
		managers.add( "m-ccm-12-pub" );
		String username = "wert";
		String password = "flargle";
		String key = managers.toString() + "/" + username;
		
		ProviderWrapper pw = new ProviderWrapper( key, managers, username,
			password, factory );
		
		Map<Integer, CallMonitor> callMons = null; // new HashMap<Integer, CallMonitor>();
		
		DeviceMonitor dev1 = new DeviceMonitor( pw, "wert-rp1", callMons );
		DeviceMonitor dev2 = new DeviceMonitor( pw, "wert-rp2", callMons );

		dev1.start();
		dev2.start();
		
		Thread.sleep( 24*60*60*1000 );
		
		dev1.stop();
		dev2.stop();
		
		Thread.sleep( 15*1000 );
	}
	
	private final static DateFormat df = new SimpleDateFormat( "HH:mm:ss.SSS" );
	
	public final static int ERROR = 0;
	
	public final static int INFO = 1;
	
	public final static int VERBOSE = 2;
	
	public static void log( int level, String fmt, Object... args )
	{
		if (level > VERBOSE)
			return;
		
		Date now = new Date();
		String s = String.format( fmt, args );
		synchronized (df)
		{
			System.out.print( df.format( now )+' '+s );
		}
	}
	
	public static Object d( Ev event )
	{
		return new EventDecoder( event );
	}
	
	public static void perform( Runnable r )
	{
		new Thread( r ).start();
		//r.run();
	}
	
	public static class ProviderFactory
	{
		public ProviderFactory( CiscoJtapiPeer jtapiPeer )
		{
			this.jtapiPeer = jtapiPeer;
		}
		
		private final CiscoJtapiPeer jtapiPeer;

		public CiscoProvider getProvider( List<String> managers, String username,
			String password )
		{
			return (CiscoProvider) jtapiPeer.getProvider(
				str( managers, username, password ) );
		}

		private String str( List<String> managers, String username, String password )
		{
			return String.format( "%s;login=%s;passwd=%s",
				str( managers ), username, password );
		}

		private String str( List<String> managers )
		{
			StringBuffer sb = new StringBuffer();
			for (String manager: managers)
			{
				if (sb.length() > 0)
					sb.append( "," );
				sb.append( manager );
			}
			return sb.toString();
		}
	}
	
	public static class ProviderWrapper implements CiscoProviderObserver
	{
		public ProviderWrapper( String key, List<String> managers,
			String username, String password, ProviderFactory factory )
		{
			this.key = key;
			this.managers = managers;
			this.username = username;
			this.password = password;
			this.factory = factory;
		}

		private final String key;
		
		private final List<String> managers;
		
		private final String username;
		
		private final String password;
		
		private final ProviderFactory factory;
		
		public String toString()
		{
			return "ProviderWrapper("+key+")";
		}
		
		public void addListener( DeviceMonitor dm ) throws Exception
		{
			synchronized (devMons)
			{
				if (devMons.containsKey( dm.name ))
					throw new IllegalArgumentException( "device name conflict: "+dm.name );
				
				devMons.put( dm.name, dm );
				
				if (devMons.size() == 1)
					startProvider();
			}
		}
		
		public void removeListener( DeviceMonitor dm )
		{
			synchronized (devMons)
			{
				if (!devMons.containsKey( dm.name ))
					throw new IllegalArgumentException( "device name conflict: "+dm.name );
				
				devMons.remove( dm.name );
				
				if (devMons.size() == 0)
					stopProvider();
			}
		}

		private DeviceMonitor findDevice( String name )
		{
			synchronized (devMons)
			{
				return devMons.get( name );
			}
		}
		
		private List<DeviceMonitor> getDeviceMonitors()
		{
			synchronized (devMons)
			{
				return new ArrayList<DeviceMonitor>( devMons.values() );
			}
		}
		
		private final Map<String, DeviceMonitor> devMons = new HashMap<String, DeviceMonitor>();
		
		private synchronized void startProvider()
			throws Exception
		{
			CiscoProvider cp = factory.getProvider( managers, username, password );
			try
			{
				cp.addObserver( this );
				_provider = cp;
			}
			catch ( Exception e )
			{
				cp.shutdown();
			}
		}
		
		private void stopProvider()
		{
			CiscoProvider cp = _provider;
			if (cp != null)
				cp.shutdown();
		}
		
		public void shutdown()
		{
			for (DeviceMonitor dm: getDeviceMonitors())
				dm.stop();
		}
		
		private CiscoProvider _provider;

		public void providerChangedEvent( ProvEv[] events )
		{
			for (ProvEv event: events)
				try
				{
					providerChangedEvent( event );
				}
				catch ( Exception e )
				{
					e.printStackTrace();
				}
		}

		private void providerChangedEvent( ProvEv event )
			throws Exception
		{
			log( VERBOSE, "%s: delivered %s\n", this, d( event ) );
			switch (event.getID())
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
					_provider = null;
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
					log( VERBOSE, "%s: ignored %s\n", this, d( event ) );
					break;
			}
		}

		private void provInService( ProvInServiceEv event )
			throws Exception
		{
			for (DeviceMonitor dm: getDeviceMonitors())
				dm.provInServiceEv( event );
		}

		private void provOutOfService( ProvOutOfServiceEv event )
		{
			for (DeviceMonitor dm: getDeviceMonitors())
				dm.provOutOfService( event );
		}

		private void provShutdown( ProvShutdownEv event )
		{
			for (DeviceMonitor dm: getDeviceMonitors())
				dm.provShutdown( event );
		}

		private void ciscoAddrAddedToTerminal( CiscoAddrAddedToTerminalEv event )
		{
			Terminal term = event.getTerminal();
			DeviceMonitor dm = findDevice( term.getName() );
			if (dm != null)
				dm.ciscoAddrAddedToTerminal( event );
		}

		private void ciscoAddrRemovedFromTerminal( CiscoAddrRemovedFromTerminalEv event )
		{
			Terminal term = event.getTerminal();
			DeviceMonitor dm = findDevice( term.getName() );
			if (dm != null)
				dm.ciscoAddrRemovedFromTerminal( event );
		}

		private void ciscoAddrCreated( CiscoAddrCreatedEv event )
		{
			// address has been added to a terminal (but we don't know
			// which one).
			Address addr = event.getAddress();
			Terminal[] terms = addr.getTerminals();
			if (terms != null)
			{
				for (Terminal term: terms)
				{
					DeviceMonitor dm = findDevice( term.getName() );
					if (dm != null)
						dm.ciscoAddrCreated( event );
				}
			}
		}

		private void ciscoAddrRemoved( CiscoAddrRemovedEv event )
		{
			// address has been removed from a terminal (but we don't
			// know which one).
			Address addr = event.getAddress();
			for (DeviceMonitor dm: getDeviceMonitors())
				dm.ciscoAddrRemoved( event );
		}

		private void ciscoTermCreated( CiscoTermCreatedEv event )
		{
			// terminal added to the provider's domain
			Terminal term = event.getTerminal();
			DeviceMonitor dm = findDevice( term.getName() );
			if (dm != null)
				dm.ciscoTermCreated( event );
		}

		private void ciscoTermRemoved( CiscoTermRemovedEv event )
		{
			// terminal removed from the provider's domain
			Terminal term = event.getTerminal();
			DeviceMonitor dm = findDevice( term.getName() );
			if (dm != null)
				dm.ciscoTermRemoved( event );
		}
	}
	
	public static class DeviceMonitor
		implements CiscoTerminalObserver
	{
		public DeviceMonitor( ProviderWrapper pw, String name,
			Map<Integer, CallMonitor> callMons )
		{
			this.pw = pw;
			this.name = name;
			this.callMons = callMons;
		}

		private final ProviderWrapper pw;
		
		private final String name;
		
		private final Map<Integer, CallMonitor> callMons;
		
		public String toString()
		{
			return "DeviceMonitor("+name+")";
		}
		
		public void start() throws Exception
		{
			log( VERBOSE, "%s: starting\n", this );
			pw.addListener( this );
		}
		
		public void stop()
		{
			log( VERBOSE, "%s: stopping\n", this );
			pw.removeListener( this );
		}

		public void provInServiceEv( ProvInServiceEv event )
			throws Exception
		{
			CiscoProvider cp = (CiscoProvider) event.getProvider();
			termOnline( cp );
		}

		public void provOutOfService( ProvOutOfServiceEv event )
		{
			synchronized (this)
			{
				termOffline( crterm );
			}
		}

		public void provShutdown( ProvShutdownEv event )
		{
			synchronized (this)
			{
				termOffline( crterm );
			}
		}

		/**
		 * Reports that an address already in the provider's domain
		 * has been added to a terminal.
		 * @param event
		 */
		public void ciscoAddrAddedToTerminal( CiscoAddrAddedToTerminalEv event )
		{
			CiscoAddress caddr = (CiscoAddress) event.getAddress();
			addAddress( caddr );
		}

		public void ciscoAddrRemovedFromTerminal( CiscoAddrRemovedFromTerminalEv event )
		{
			CiscoAddress caddr = (CiscoAddress) event.getAddress();
			removeAddress( caddr );
		}

		/**
		 * Reports that an address has been added to the provider's
		 * domain. That means that the address has been added to a
		 * terminal in the provider's domain. We don't know if this
		 * terminal is affected or not, so we have to check. If this
		 * terminal does know about the address, we can ignore this event.
		 * @param event
		 */
		public void ciscoAddrCreated( CiscoAddrCreatedEv event )
		{
			CiscoRouteTerminal crt = crterm;
			if (crt == null)
				return;
			
			CiscoAddress caddr = (CiscoAddress) event.getAddress();
			
			Address[] addrs = crt.getAddresses();
			if (addrs != null)
			{
				for (Address addr: addrs)
				{
					if (caddr.equals( addr ))
						addAddress( caddr );
				}
			}
		}

		/**
		 * Reports that an address has been removed from the provider's
		 * domain. That means that the address has been removed from a
		 * terminal in the provider's domain. We don't know if this terminal
		 * is affected or not, so we have to check. If this terminal does
		 * not know about the address, we can ignore this event.
		 * @param event
		 */
		public void ciscoAddrRemoved( CiscoAddrRemovedEv event )
		{
			CiscoAddress caddr = (CiscoAddress) event.getAddress();
			removeAddress( caddr );
		}

		/**
		 * Reports that a terminal has been added to the provider's
		 * domain. That means that the logged in user has been associated
		 * with the terminal.
		 */
		public void ciscoTermCreated( CiscoTermCreatedEv event )
		{
			CiscoProvider cp = (CiscoProvider) event.getProvider();
			termOnline( cp );
		}

		/**
		 * Reports that a terminal has been removed from the provider's
		 * domain. That means that the logged in user is no longer associated
		 * with the terminal.
		 * @param event  
		 */
		public void ciscoTermRemoved( CiscoTermRemovedEv event )
		{
			synchronized (this)
			{
				termOffline( crterm );
			}
		}
		
		private void termOnline( CiscoProvider cp )
		{
			synchronized (this)
			{
				if (crterm == null)
				{
					CiscoRouteTerminal crt = null;
					
					try
					{
						crt = (CiscoRouteTerminal) cp.getTerminal( name );
					}
					catch ( Exception e )
					{
						log( VERBOSE, "%s: term %s is not in provider's domain\n", this, name );
						return;
					}
					
					try
					{
						registerMedia( crt );
						crt.addObserver( this );
					}
					catch ( Exception e )
					{
						termOffline( crt );
						e.printStackTrace();
					}

					crterm = crt;
				}
			}
		}

		private void termOffline( CiscoRouteTerminal crt )
		{
			if (crt != null)
			{
				unregisterMedia( crt );
				crt.removeObserver( this );
			}
		}
		
		private void registerMedia( CiscoRouteTerminal ct )
			throws Exception
		{
			CiscoMediaCapability[] caps =
			{
				new CiscoG711MediaCapability(
					CiscoG711MediaCapability.FRAMESIZE_TWENTY_MILLISECOND_PACKET )
			};
			ct.register( caps, CiscoRouteTerminal.DYNAMIC_MEDIA_REGISTRATION );
		}
		
		private void unregisterMedia( CiscoRouteTerminal ct )
		{
			try
			{
				ct.unregister();
			}
			catch ( CiscoUnregistrationException e )
			{
				e.printStackTrace();
			}
		}

		private CiscoRouteTerminal crterm;

		public void terminalChangedEvent( TermEv[] events )
		{
			for (TermEv event: events)
			{
				try
				{
					terminalChangedEvent( event );
				}
				catch ( Exception e )
				{
					e.printStackTrace();
				}
			}
		}

		private void terminalChangedEvent( TermEv event )
			throws Exception
		{
			log( VERBOSE, "%s: delivered %s\n", this, d( event ) );
			switch (event.getID())
			{
				case CiscoTermInServiceEv.ID:
					ciscoTermInService( (CiscoTermInServiceEv) event );
					break;
				
				case CiscoTermOutOfServiceEv.ID:
					ciscoTermOutOfService( (CiscoTermOutOfServiceEv) event );
					break;
				
				case TermObservationEndedEv.ID:
					termObservationEnded( (TermObservationEndedEv) event );
					break;
					
				case CiscoMediaOpenLogicalChannelEv.ID:
					ciscoMediaOpenLogicalChannel( (CiscoMediaOpenLogicalChannelEv) event );
					break;
					
				case CiscoRTPInputStartedEv.ID:
					ciscoRTPInputStarted( (CiscoRTPInputStartedEv) event );
					break;
					
				case CiscoRTPInputStoppedEv.ID:
					ciscoRTPInputStopped( (CiscoRTPInputStoppedEv) event );
					break;
					
				case CiscoRTPOutputStartedEv.ID:
					ciscoRTPOutputStarted( (CiscoRTPOutputStartedEv) event );
					break;
					
				case CiscoRTPOutputStoppedEv.ID:
					ciscoRTPOutputStopped( (CiscoRTPOutputStoppedEv) event );
					break;
				
				default:
					log( VERBOSE, "%s: ignored %s\n", this, d( event ) );
					break;
			}
		}

		private void ciscoTermInService( CiscoTermInServiceEv event )
		{
			CiscoRouteTerminal crt = crterm;
			if (crt == null)
				return;
			
			Address[] addrs = crt.getAddresses();
			if (addrs != null)
			{
				log( VERBOSE, "%s: found addrs\n", this );
				for (Address addr: addrs)
					addAddress( (CiscoAddress) addr );
				log( VERBOSE, "%s: end of addrs\n", this );
			}
			else
			{
				log( VERBOSE, "%s: no addrs found\n", this );
			}
		}

		private void ciscoTermOutOfService( CiscoTermOutOfServiceEv event )
		{
			clearAddresses();
		}

		private void termObservationEnded( TermObservationEndedEv event )
		{
			synchronized (this)
			{
				crterm = null;
			}
		}

		private void ciscoMediaOpenLogicalChannel( CiscoMediaOpenLogicalChannelEv event )
			throws Exception
		{
			final CiscoRTPHandle rtpHandle = event.getCiscoRTPHandle();
			int payLoadType = event.getPayLoadType();
			int packetSize = event.getPacketSize();

			CiscoCall call = ((CiscoProvider) crterm.getProvider()).getCall( rtpHandle );
			CiscoCallID callId = call.getCallID();
			final int id = callId.intValue();
			
//			LogicalChannel lc = new LogicalChannel( this, cm, payLoadType, packetSize );
			
			perform( new Runnable()
			{
				public void run()
				{
					try
					{
						log( VERBOSE, "%s: making rtp params %d %d\n",
							DeviceMonitor.this, id, rtpHandle.getHandle() );

						InetAddress addr = InetAddress.getByName( "10.89.30.144" );
						CiscoRTPParams rtpParams = new CiscoRTPParams( addr, 12345 );

						log( VERBOSE, "%s: setting rtp params %d %d\n",
							DeviceMonitor.this, id, rtpHandle.getHandle() );
						
						crterm.setRTPParams( rtpHandle, rtpParams );
						
						log( VERBOSE, "%s: done setting rtp params %d %d\n",
							DeviceMonitor.this, id, rtpHandle.getHandle() );
					}
					catch ( Exception e )
					{
						e.printStackTrace();
					}
				}
			} );
		}

		/**
		 * Notifies us of the rtp parameters of the media coming to us.
		 * @param event
		 */
		private void ciscoRTPInputStarted( CiscoRTPInputStartedEv event )
		{
			CiscoCallID callId = event.getCallID();
			CiscoRTPInputProperties rtpInputProperties = event.getRTPInputProperties();
			
			int id = callId.intValue();
//			CallMonitor cm = callMons.get( id );
			
			// TODO Auto-generated method stub
		}

		/**
		 * Notifies us that rtp to us has stopped.
		 * @param event
		 */
		private void ciscoRTPInputStopped( CiscoRTPInputStoppedEv event )
		{
			CiscoCallID callId = event.getCallID();
			
			int id = callId.intValue();
//			CallMonitor cm = callMons.get( id );
			
			// TODO Auto-generated method stub
		}

		/**
		 * Notifies us of the rtp parameters of the media we are supposed
		 * to send to them.
		 * @param event
		 */
		private void ciscoRTPOutputStarted( CiscoRTPOutputStartedEv event )
		{
			CiscoCallID callId = event.getCallID();
			CiscoRTPOutputProperties rtpOutputProperties = event.getRTPOutputProperties();
			
			int id = callId.intValue();
//			CallMonitor cm = callMons.get( id );
			
			// TODO Auto-generated method stub
		}

		/**
		 * Notifies us that rtp to them has stopped.
		 * @param event
		 */
		private void ciscoRTPOutputStopped( CiscoRTPOutputStoppedEv event )
		{
//			CiscoCallID callId = event.getCallID();
//			
//			int id = callId.intValue();
//			CallMonitor cm = callMons.get( id );
//			
//			log( VERBOSE, "%s: CiscoRTPOutputStoppedEv %d\n", this, id );
			
			// TODO Auto-generated method stub
		}

		private void clearAddresses()
		{
			synchronized (addrMons)
			{
				log( VERBOSE, "%s: addresses cleared\n", this );
				Iterator<AddressMonitor> i = addrMons.values().iterator();
				while (i.hasNext())
				{
					AddressMonitor am = i.next();
					i.remove();
					am.stop();
				}
			}
		}

		private void addAddress( CiscoAddress caddr )
		{
			synchronized (addrMons)
			{
				if (!addrMons.containsKey( caddr.getName() ))
				{
					CiscoRouteTerminal crt = crterm;
					if (crt == null)
						return;
					
					AddressMonitor am = new AddressMonitor( this, crt, caddr, callMons );
					try
					{
						am.start();
						log( VERBOSE, "%s: added address %s\n", this, caddr );
						addrMons.put( caddr.getName(), am );
					}
					catch ( Exception e )
					{
						log( VERBOSE, "%s: failed to add address %s\n", this, caddr );
						e.printStackTrace();
					}
				}
			}
		}

		private void removeAddress( CiscoAddress caddr )
		{
			synchronized (addrMons)
			{
				AddressMonitor am = addrMons.remove( caddr.getName() );
				if (am != null)
				{
					log( VERBOSE, "%s: removed address %s\n", this, caddr );
					am.stop();
				}
			}
		}
		
		private final Map<String, AddressMonitor> addrMons = new HashMap<String, AddressMonitor>();

		public void callChangedEvent( CallEv[] events )
		{
			// ignore (this is here to catch call information internally)
		}
	}
	
	public static class AddressMonitor
		implements CiscoAddressObserver, CallObserver
	{
		public AddressMonitor( DeviceMonitor dm, CiscoTerminal cterm,
			CiscoAddress caddr, Map<Integer, CallMonitor> callMons )
		{
			this.dm = dm;
			this.cterm = cterm;
			this.caddr = caddr;
			this.callMons = callMons;
		}
		
		private final DeviceMonitor dm;
		
		private final CiscoTerminal cterm;
		
		private final CiscoAddress caddr;
		
		private final Map<Integer, CallMonitor> callMons;
		
		public String toString()
		{
			return "AddressMonitor("+cterm+", "+caddr+")";
		}

		public void start() throws Exception
		{
			caddr.addObserver( this );
			caddr.addCallObserver( this );
		}

		public void stop()
		{
			caddr.removeObserver( this );
			caddr.removeCallObserver( this );
		}

		public void addressChangedEvent( AddrEv[] events )
		{
			for (AddrEv event: events)
			{
				try
				{
					addressChangedEvent( event );
				}
				catch ( Exception e )
				{
					e.printStackTrace();
				}
			}
		}

		private void addressChangedEvent( AddrEv event )
			throws Exception
		{
			log( VERBOSE, "%s: delivered %s\n", this, d( event ) );
			switch (event.getID())
			{
				case CiscoAddrInServiceEv.ID:
					ciscoAddrInService( (CiscoAddrInServiceEv) event );
					break;
				
				case CiscoAddrOutOfServiceEv.ID:
					ciscoAddrOutOfService( (CiscoAddrOutOfServiceEv) event );
					break;
				
				case AddrObservationEndedEv.ID:
					// ignore.
					break;
				
				case CiscoAddrAutoAcceptStatusChangedEv.ID:
					// ignore.
					break;
				
				default:
					log( VERBOSE, "%s: ignored %s\n", this, d( event ) );
					break;
			}
		}

		private void ciscoAddrInService( CiscoAddrInServiceEv event )
			throws Exception
		{
			caddr.setAutoAcceptStatus( CiscoAddress.AUTOACCEPT_ON, cterm );
		}

		private void ciscoAddrOutOfService( CiscoAddrOutOfServiceEv event )
		{
			// ignore
		}

		public void callChangedEvent( CallEv[] events )
		{
			for (CallEv event: events)
			{
				try
				{
					callChangedEvent( event );
				}
				catch ( Exception e )
				{
					e.printStackTrace();
				}
			}
		}

		private void callChangedEvent( CallEv event ) throws Exception
		{
//			log( VERBOSE, "%s: delivered %s\n", this, d( event ) );
			switch (event.getID())
			{
				case CallActiveEv.ID:
				{
					CiscoCall call = (CiscoCall) event.getCall();
					call.addObserver( new CallMonitor( dm, this, cterm, caddr, call, callMons ) );
					call.removeObserver( this );
					break;
				}
				
				default:
//					log( VERBOSE, "%s: ignored %s\n", this, d( event ) );
					break;
			}
		}
	}
	
	public static class CallMonitor
		implements CallObserver, CallControlCallObserver
	{
		public CallMonitor( DeviceMonitor dm, AddressMonitor am,
			CiscoTerminal cterm, CiscoAddress caddr, CiscoCall call,
			Map<Integer, CallMonitor> callMons )
		{
			this.dm = dm;
			this.am = am;
			this.cterm = cterm;
			this.caddr = caddr;
			this.call = call;
			this.callMons = callMons;
			id = call.getCallID().intValue();
		}
		
		private final DeviceMonitor dm;
		
		private final AddressMonitor am;
		
		private final CiscoTerminal cterm;
		
		private final CiscoAddress caddr;
		
		private final CiscoCall call;
		
		private final Map<Integer, CallMonitor> callMons;
		
		private final int id;
		
		public String toString()
		{
			return "Call("+id+")";
		}

		public void callChangedEvent( CallEv[] events )
		{
			for (CallEv event: events)
			{
				try
				{
					if (event instanceof ConnEv)
					{
						ConnEv ev = (ConnEv) event;
						Connection c = ev.getConnection();
						if (c != null)
						{
							Address a = c.getAddress();
							if (a != null)
							{
								if (!a.equals( caddr ))
									continue;
							}
						}
					}
					
					if (event instanceof TermConnEv)
					{
						TermConnEv ev = (TermConnEv) event;
						TerminalConnection tc = ev.getTerminalConnection();
						if (tc != null)
						{
							Terminal t = tc.getTerminal();
							if (t != null)
							{
								if (!t.equals( cterm ))
									continue;
							}
						}
					}
					
					callChangedEvent( event );
				}
				catch ( Exception e )
				{
					e.printStackTrace();
				}
			}
		}

		private void callChangedEvent( CallEv event )
			throws Exception
		{
			log( VERBOSE, "%s: delivered %s\n", this, d( event ) );
			switch (event.getID())
			{
				case CallActiveEv.ID:
//					callMons.put( id, this );
					break;
				
				case CallInvalidEv.ID:
					call.removeObserver( this );
					break;
				
				case CallObservationEndedEv.ID:
//					callMons.remove( id );
					break;
					
				// [ADDRESS] CONNECTION EVENTS //
					
				case ConnCreatedEv.ID:
					connCreated( (ConnCreatedEv) event );
					break;
					
				case ConnInProgressEv.ID:
					connInProgress( (ConnInProgressEv) event );
					break;
					
				case ConnAlertingEv.ID:
					connAlerting( (ConnAlertingEv) event );
					break;
					
				case ConnConnectedEv.ID:
					connConnected( (ConnConnectedEv) event );
					break;
					
				case ConnDisconnectedEv.ID:
					connDisconnected( (ConnDisconnectedEv) event );
					break;
					
				case ConnFailedEv.ID:
					connFailed( (ConnFailedEv) event );
					break;
					
				case ConnUnknownEv.ID:
					connUnknown( (ConnUnknownEv) event );
					break;
				
				// TERMINAL CONNECTION EVENTS //
					
				case TermConnActiveEv.ID:
					termConnActive( (TermConnActiveEv) event );
					break;
					
				case TermConnCreatedEv.ID:
					termConnCreated( (TermConnCreatedEv) event );
					break;
					
				case TermConnDroppedEv.ID:
					termConnDropped( (TermConnDroppedEv) event );
					break;
					
				case TermConnPassiveEv.ID:
					termConnPassive( (TermConnPassiveEv) event );
					break;
					
				case TermConnRingingEv.ID:
					termConnRinging( (TermConnRingingEv) event );
					break;
					
				case TermConnUnknownEv.ID:
					termConnUnknown( (TermConnUnknownEv) event );
					break;
				
				default:
					log( VERBOSE, "%s: ignored %s\n", this, d( event ) );
					break;
			}
		}

		///////////////////////
		// CONNECTION EVENTS //
		///////////////////////

		/**
		 * Notifies that a connection has been created between a call and
		 * an address of a device we are monitoring. The state of the
		 * connection is IDLE.
		 * @param event
		 */
		private void connCreated( ConnCreatedEv event )
		{
			int cid = getConnId( event );
			
			// TODO Auto-generated method stub
		}

		/**
		 * Notifies that the connection state has changed to INPROGRESS.
		 * @param event
		 */
		private void connInProgress( ConnInProgressEv event )
		{
			int cid = getConnId( event );
			
			// TODO Auto-generated method stub
		}

		/**
		 * Notifies that the connection state has changed to ALERTING.
		 * @param event
		 */
		private void connAlerting( ConnAlertingEv event )
		{
			int cid = getConnId( event );
			
			// TODO Auto-generated method stub
		}

		/**
		 * Notifies that the connection state has changed to CONNECTED.
		 * @param event
		 */
		private void connConnected( ConnConnectedEv event )
		{
			int cid = getConnId( event );
			
			// TODO Auto-generated method stub
		}

		/**
		 * Notifies that the connection state has changed to DISCONNECTED.
		 * @param event
		 */
		private void connDisconnected( ConnDisconnectedEv event )
		{
			int cid = getConnId( event );
			
			// TODO Auto-generated method stub
		}

		/**
		 * Notifies that the connection state has changed to FAILED.
		 * @param event
		 */
		private void connFailed( ConnFailedEv event )
		{
			int cid = getConnId( event );
			
			// TODO Auto-generated method stub
		}

		/**
		 * Notifies that the connection state has changed to UKNOWN.
		 * @param event
		 */
		private void connUnknown( ConnUnknownEv event )
		{
			int cid = getConnId( event );
			
			// TODO Auto-generated method stub
		}
		
		public int getConnId( ConnEv event )
		{
			CiscoConnection conn = (CiscoConnection) event.getConnection();
			if (conn == null)
				return 0;
			
			CiscoConnectionID connId = conn.getConnectionID();
			if (connId == null)
				return 0;
			
			return connId.intValue();
		}

		////////////////////////////////
		// TERMINAL CONNECTION EVENTS //
		////////////////////////////////

		/**
		 * Notifies that a terminal connection has been created between a
		 * call and a device we are monitoring. The state of the terminal
		 * connection is IDLE.
		 * @param event
		 */
		private void termConnCreated( TermConnCreatedEv event )
		{
			int cid = getConnId( event );
			
			// TODO Auto-generated method stub
		}

		/**
		 * Notifies that the terminal connection state has changed to RINGING.
		 * @param event
		 */
		private void termConnRinging( TermConnRingingEv event )
			throws Exception
		{
			final int cid = getConnId( event );
			
			final CiscoTerminalConnection ctc = (CiscoTerminalConnection) event.getTerminalConnection();
			
			perform( new Runnable()
			{
				public void run()
				{
					try
					{
						log( VERBOSE, "%s: answer %d %d\n", CallMonitor.this, id, cid );
						ctc.answer();
						log( VERBOSE, "%s: answered %d %d\n", CallMonitor.this, id, cid );
					}
					catch ( Exception e )
					{
						//e.printStackTrace();
						log( ERROR, "%s: answer failed: %s\n", CallMonitor.this, id, cid, e );
					}
				}
			} );
		}

		/**
		 * Notifies that the terminal connection state has changed to ACTIVE.
		 * @param event
		 */
		private void termConnActive( TermConnActiveEv event )
		{
			int cid = getConnId( event );
			
			// TODO Auto-generated method stub
		}

		/**
		 * Notifies that the terminal connection state has changed to PASSIVE.
		 * @param event
		 */
		private void termConnPassive( TermConnPassiveEv event )
		{
			int cid = getConnId( event );
			
			// TODO Auto-generated method stub
		}

		/**
		 * Notifies that the terminal connection state has changed to DROPPED.
		 * @param event
		 */
		private void termConnDropped( TermConnDroppedEv event )
		{
			int cid = getConnId( event );
			
			// TODO Auto-generated method stub
		}

		/**
		 * Notifies that the terminal connection state has changed to UNKNOWN.
		 * @param event
		 */
		private void termConnUnknown( TermConnUnknownEv event )
		{
			int cid = getConnId( event );
			
			// TODO Auto-generated method stub
		}
		
		public int getConnId( TermConnEv event )
		{
			CiscoTerminalConnection termConn = (CiscoTerminalConnection) event.getTerminalConnection();
			if (termConn == null)
				return 0;
			
			CiscoConnection conn = (CiscoConnection) termConn.getConnection();
			if (conn == null)
				return 0;
			
			CiscoConnectionID connId = conn.getConnectionID();
			if (connId == null)
				return 0;
			
			return connId.intValue();
		}
	}
	
	public static class EventDecoder
	{
		public EventDecoder( Ev event )
		{
			this.event = event;
		}
		
		private final Ev event;
		
		public String toString()
		{
			Class clss = event.getClass();
			// clss will be an Impl of some single interface.
			Class[] intfs = clss.getInterfaces();
			assert intfs.length == 1 : "intfs.length == 1";
			Class intf = intfs[0];
//			for (Class c: clss.getInterfaces())
//				System.out.print( c );
//			System.out.println();
			StringBuffer sb = new StringBuffer( rightmost( intf.getName(), '.' ) );
			Set<Class> done = new HashSet<Class>();
			decodeArgs( sb, done, intf );
			return sb.toString();
		}

		private void decodeArgs( StringBuffer sb, Set<Class> done, Class intf )
		{
			if (done.contains( intf ))
				return;
			
			done.add( intf );
			
			doDecodeArgs( sb, intf );
			
			for (Class c: intf.getInterfaces())
				decodeArgs( sb, done, c );
		}

		private void doDecodeArgs( StringBuffer sb, Class intf )
		{
			if (intf == AddrEv.class)
				decodeAddrEv( sb, (AddrEv) event );
			else if (intf == CallEv.class)
				decodeCallEv( sb, (CallEv) event );
			else if (intf == ProvEv.class)
				decodeProvEv( sb, (ProvEv) event );
			else if (intf == TermEv.class)
				decodeTermEv( sb, (TermEv) event );
			else if (intf == CiscoAddrInServiceEv.class)
				decodeCiscoAddrInServiceEv( sb, (CiscoAddrInServiceEv) event );
			else if (intf == CiscoAddrOutOfServiceEv.class)
				decodeCiscoAddrOutOfServiceEv( sb, (CiscoAddrOutOfServiceEv) event );
			else if (intf == CiscoAddrAutoAcceptStatusChangedEv.class)
				decodeCiscoAddrAutoAcceptStatusChangedEv( sb, (CiscoAddrAutoAcceptStatusChangedEv) event );
			else if (intf == CiscoMediaOpenLogicalChannelEv.class)
				decodeCiscoMediaOpenLogicalChannelEv( sb, (CiscoMediaOpenLogicalChannelEv) event );
			else if (intf == CiscoRTPInputStartedEv.class)
				decodeCiscoRTPInputStartedEv( sb, (CiscoRTPInputStartedEv) event );
			else if (intf == CiscoRTPInputStoppedEv.class)
				decodeCiscoRTPInputStoppedEv( sb, (CiscoRTPInputStoppedEv) event );
			else if (intf == CiscoRTPOutputStartedEv.class)
				decodeCiscoRTPOutputStartedEv( sb, (CiscoRTPOutputStartedEv) event );
			else if (intf == CiscoRTPOutputStoppedEv.class)
				decodeCiscoRTPOutputStoppedEv( sb, (CiscoRTPOutputStoppedEv) event );
			else if (intf == CallCtlCallEv.class)
				decodeCallCtlCallEv( sb, (CallCtlCallEv) event );
			else if (intf == CallCtlConnEv.class)
				decodeCallCtlConnEv( sb, (CallCtlConnEv) event );
			else if (intf == CallCtlEv.class)
				decodeCallCtlEv( sb, (CallCtlEv) event );
			else if (intf == CallCtlTermConnEv.class)
				decodeCallCtlTermConnEv( sb, (CallCtlTermConnEv) event );
			else if (intf == ConnEv.class)
				decodeConnEv( sb, (ConnEv) event );
			else if (intf == TermConnEv.class)
				decodeTermConnEv( sb, (TermConnEv) event );
			else if (intf == CiscoAddrAddedToTerminalEv.class)
				decodeCiscoAddrAddedToTerminalEv( sb, (CiscoAddrAddedToTerminalEv) event );
			else if (intf == CiscoAddrRemovedFromTerminalEv.class)
				decodeCiscoAddrRemovedFromTerminalEv( sb, (CiscoAddrRemovedFromTerminalEv) event );
			else if (intf == CiscoAddrCreatedEv.class)
				decodeCiscoAddrCreatedEv( sb, (CiscoAddrCreatedEv) event );
			else if (intf == CiscoAddrRemovedEv.class)
				decodeCiscoAddrRemovedEv( sb, (CiscoAddrRemovedEv) event );
			else if (intf == CiscoTermCreatedEv.class)
				decodeCiscoTermCreatedEv( sb, (CiscoTermCreatedEv) event );
			else if (intf == CiscoTermRemovedEv.class)
				decodeCiscoTermRemovedEv( sb, (CiscoTermRemovedEv) event );
//			else if (intf == blah.class)
//			decodeBlah( sb, (blah) event );
			else if (intf == Ev.class)
				;
			else if (intf == CiscoProvEv.class)
				;
			else if (intf == ProvInServiceEv.class)
				;
			else if (intf == ProvObservationEndedEv.class)
				;
			else if (intf == ProvOutOfServiceEv.class)
				;
			else if (intf == ProvShutdownEv.class)
				;
			else if (intf == CiscoEv.class)
				;
			else if (intf == CiscoAddrEv.class)
				;
			else if (intf == CiscoCallEv.class)
				;
			else if (intf == CiscoTermEv.class)
				;
			else if (intf == CiscoOutOfServiceEv.class)
				;
			else if (intf == CiscoTermInServiceEv.class)
				;
			else if (intf == CiscoTermOutOfServiceEv.class)
				;
			else if (intf == CallActiveEv.class)
				;
			else if (intf == ConnCreatedEv.class)
				;
			else if (intf == ConnInProgressEv.class)
				;
			else if (intf == CallCtlConnOfferedEv.class)
				;
			else if (intf == ConnAlertingEv.class)
				;
			else if (intf == CallCtlConnAlertingEv.class)
				;
			else if (intf == TermConnCreatedEv.class)
				;
			else if (intf == TermConnRingingEv.class)
				;
			else if (intf == CallCtlTermConnRingingEv.class)
				;
			else if (intf == ConnConnectedEv.class)
				;
			else if (intf == CallCtlConnEstablishedEv.class)
				;
			else if (intf == TermConnActiveEv.class)
				;
			else if (intf == CallCtlTermConnTalkingEv.class)
				;
			else if (intf == TermConnDroppedEv.class)
				;
			else if (intf == CallCtlTermConnDroppedEv.class)
				;
			else if (intf == ConnDisconnectedEv.class)
				;
			else if (intf == CallCtlConnDisconnectedEv.class)
				;
			else if (intf == CallInvalidEv.class)
				;
			else if (intf == CallObservationEndedEv.class)
				;
			else if (xxx.add( intf ))
			{
				System.out.printf( "---- need support for %s\n", intf );
			}
		}
		
		private void decodeCiscoTermRemovedEv( StringBuffer sb, CiscoTermRemovedEv ev )
		{
			sb.append( "; term=" );
			sb.append( ev.getTerminal() );
		}

		private void decodeCiscoTermCreatedEv( StringBuffer sb, CiscoTermCreatedEv ev )
		{
			sb.append( "; term=" );
			sb.append( ev.getTerminal() );
		}

		private void decodeCiscoAddrRemovedEv( StringBuffer sb, CiscoAddrRemovedEv ev )
		{
			sb.append( "; addr=" );
			sb.append( ev.getAddress() );
		}

		private void decodeCiscoAddrCreatedEv( StringBuffer sb, CiscoAddrCreatedEv ev )
		{
			sb.append( "; addr=" );
			sb.append( ev.getAddress() );
		}

		private void decodeCiscoAddrRemovedFromTerminalEv( StringBuffer sb, CiscoAddrRemovedFromTerminalEv ev )
		{
			sb.append( "; term=" );
			sb.append( ev.getTerminal() );
			sb.append( "; addr=" );
			sb.append( ev.getAddress() );
		}

		private void decodeCiscoAddrAddedToTerminalEv( StringBuffer sb, CiscoAddrAddedToTerminalEv ev )
		{
			sb.append( "; term=" );
			sb.append( ev.getTerminal() );
			sb.append( "; addr=" );
			sb.append( ev.getAddress() );
		}

		private void decodeTermConnEv( StringBuffer sb, TermConnEv ev )
		{
			CiscoTerminalConnection tc = (CiscoTerminalConnection) ev.getTerminalConnection();
			decodeCiscoTerminalConnection( sb, tc );
		}

		private void decodeCiscoTerminalConnection( StringBuffer sb, CiscoTerminalConnection tc )
		{
			sb.append( "; term=" );
			sb.append( tc.getTerminal() );
			decodeCiscoConnection( sb, (CiscoConnection) tc.getConnection() );
		}

		private void decodeCiscoConnection( StringBuffer sb, CiscoConnection c )
		{
			sb.append( "; addr=" );
			sb.append( c.getAddress() );
			sb.append( "; connId=" );
			sb.append( c.getConnectionID().intValue() );
			decodeCiscoCall( sb, (CiscoCall) c.getCall() );
		}

		private void decodeCiscoCall( StringBuffer sb, CiscoCall c )
		{
			sb.append( "; callId=" );
			sb.append( c.getCallID().intValue() );
			sb.append( "; calledAddress=" );
			sb.append( c.getCalledAddress() );
			sb.append( "; callingAddress=" );
			sb.append( c.getCallingAddress() );
			sb.append( "; lastRedirectedAddress=" );
			sb.append( c.getLastRedirectedAddress() );
			sb.append( "; currentCalledAddress=" );
			sb.append( c.getCurrentCalledAddress() );
			sb.append( "; currentCallingAddress=" );
			sb.append( c.getCurrentCallingAddress() );
			sb.append( "; modifiedCalledAddress=" );
			sb.append( c.getModifiedCalledAddress() );
			sb.append( "; modifiedCallingAddress=" );
			sb.append( c.getModifiedCallingAddress() );
		}

		private void decodeConnEv( StringBuffer sb, ConnEv ev )
		{
			decodeCiscoConnection( sb, (CiscoConnection) ev.getConnection() );
		}

		private void decodeCallCtlTermConnEv( StringBuffer sb, CallCtlTermConnEv ev )
		{
			// nothing.
		}

		private void decodeCallCtlEv( StringBuffer sb, CallCtlEv ev )
		{
			// nothing
		}

		private void decodeCallCtlConnEv( StringBuffer sb, CallCtlConnEv ev )
		{
			// nothing
		}

		private void decodeCallCtlCallEv( StringBuffer sb, CallCtlCallEv ev )
		{
			// nothing
		}

		private void decodeCiscoRTPOutputStoppedEv( StringBuffer sb, CiscoRTPOutputStoppedEv ev )
		{
			CiscoCallID x = ev.getCallID();
			if (x != null)
			{
				sb.append( "; callId=" );
				sb.append( x.intValue() );
			}
		}

		private void decodeCiscoRTPOutputStartedEv( StringBuffer sb, CiscoRTPOutputStartedEv ev )
		{
			sb.append( "; callId=" );
			sb.append( ev.getCallID().intValue() );
			CiscoRTPOutputProperties p = ev.getRTPOutputProperties();
		}

		private void decodeCiscoRTPInputStoppedEv( StringBuffer sb, CiscoRTPInputStoppedEv ev )
		{
			CiscoCallID x = ev.getCallID();
			if (x != null)
			{
				sb.append( "; callId=" );
				sb.append( x.intValue() );
			}
		}

		private void decodeCiscoRTPInputStartedEv( StringBuffer sb, CiscoRTPInputStartedEv ev )
		{
			sb.append( "; callId=" );
			sb.append( ev.getCallID().intValue() );
			CiscoRTPInputProperties p = ev.getRTPInputProperties();
		}

		private void decodeCiscoMediaOpenLogicalChannelEv( StringBuffer sb, CiscoMediaOpenLogicalChannelEv ev )
		{
			sb.append( "; rtpHandle=" );
			sb.append( ev.getCiscoRTPHandle().getHandle() );
			sb.append( "; packetSize=" );
			sb.append( ev.getPacketSize() );
			sb.append( "; payLoadType=" );
			sb.append( ev.getPayLoadType() );
		}

		private final static Set<Class> xxx = Collections.synchronizedSet( new HashSet<Class>() );

		private void decodeCiscoAddrAutoAcceptStatusChangedEv( StringBuffer sb, CiscoAddrAutoAcceptStatusChangedEv ev )
		{
			sb.append( "; term=" );
			sb.append( ev.getTerminal() );
			sb.append( "; status=" );
			sb.append( ev.getAutoAcceptStatus() );
		}

		private void decodeCiscoAddrOutOfServiceEv( StringBuffer sb, CiscoAddrOutOfServiceEv ev )
		{
			sb.append( "; term=" );
			sb.append( ev.getTerminal() );
		}

		private void decodeCiscoAddrInServiceEv( StringBuffer sb, CiscoAddrInServiceEv ev )
		{
			sb.append( "; term=" );
			sb.append( ev.getTerminal() );
		}

		private void decodeCallEv( StringBuffer sb, CallEv ev )
		{
			decodeCiscoCall( sb, (CiscoCall) ev.getCall() );
		}

		private void decodeProvEv( StringBuffer sb, ProvEv ev )
		{
			sb.append( "; prov=" );
			sb.append( ev.getProvider().getName() );
		}

		private void decodeAddrEv( StringBuffer sb, AddrEv ev )
		{
			sb.append( "; addr=" );
			sb.append( ev.getAddress().getName() );
		}

		private void decodeTermEv( StringBuffer sb, TermEv ev )
		{
			sb.append( "; term=" );
			sb.append( ev.getTerminal() );
		}

		private String rightmost( String s, char c )
		{
			int i = s.lastIndexOf( c );
			if (i < 0)
				return s;
			return s.substring( i+1 );
		}
	}
}
