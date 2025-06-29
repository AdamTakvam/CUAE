/* $Id$
 *
 * Created by wert on Aug 18, 2005.
 *
 * Copyright (c) 2005 Metreos, Inc. All rights reserved.
 */

package metreos.service.jtapi.test;

import java.lang.reflect.Field;
import java.lang.reflect.Method;
import java.lang.reflect.Modifier;
import java.util.Collections;
import java.util.HashMap;
import java.util.HashSet;
import java.util.Map;
import java.util.Set;

import javax.telephony.Address;
import javax.telephony.JtapiPeerFactory;
import javax.telephony.Terminal;
import javax.telephony.callcontrol.CallControlCallObserver;
import javax.telephony.events.AddrEv;
import javax.telephony.events.CallEv;
import javax.telephony.events.Ev;
import javax.telephony.events.ProvEv;
import javax.telephony.events.ProvInServiceEv;
import javax.telephony.events.TermEv;

import metreos.util.Trace;

import com.cisco.jtapi.extensions.CiscoAddrInServiceEv;
import com.cisco.jtapi.extensions.CiscoAddress;
import com.cisco.jtapi.extensions.CiscoAddressObserver;
import com.cisco.jtapi.extensions.CiscoG711MediaCapability;
import com.cisco.jtapi.extensions.CiscoJtapiException;
import com.cisco.jtapi.extensions.CiscoJtapiPeer;
import com.cisco.jtapi.extensions.CiscoMediaCapability;
import com.cisco.jtapi.extensions.CiscoMediaTerminal;
import com.cisco.jtapi.extensions.CiscoProvider;
import com.cisco.jtapi.extensions.CiscoProviderObserver;
import com.cisco.jtapi.extensions.CiscoRegistrationException;
import com.cisco.jtapi.extensions.CiscoRouteTerminal;
import com.cisco.jtapi.extensions.CiscoTermInServiceEv;
import com.cisco.jtapi.extensions.CiscoTerminalObserver;

@SuppressWarnings("all")
public class testJtapi implements
	CiscoProviderObserver,
	CiscoTerminalObserver,
	CiscoAddressObserver,
	CallControlCallObserver
{
	public static void main( String[] args )
		throws Exception
	{
		if (args.length != 2)
			throw new Exception( "usage: testJtapi ccm device" );
		
		String host = args[0];
		String term = args[1];
		
		CiscoJtapiPeer jtapiPeer = (CiscoJtapiPeer) JtapiPeerFactory.getJtapiPeer( null );
		CiscoProvider provider = (CiscoProvider) jtapiPeer.getProvider( host+";login=wert;passwd=metreos" );
		
		testJtapi prog = new testJtapi( provider, term );
		provider.addObserver( prog );
	}
	
	public testJtapi( CiscoProvider provider, String term )
	{
		this.provider = provider;
		this.term = term;
	}

	private final CiscoProvider provider;
	
	private final String term;
	
	public void report( String msg )
	{
		Trace.report( this, msg );
	}
	
	public void report( String msg, Ev ev )
	{
		Trace.report( this, EventFormat.format( msg, ev ) );
	}
	
	public void report( Throwable t )
	{
		Trace.report( this, t );
	}
	
	public void report( String msg, Throwable t )
	{
		Trace.report( this, msg, t );
	}
	
	public void report( String msg, Ev ev, Throwable t )
	{
		if (t instanceof CiscoJtapiException)
		{
			CiscoJtapiException je = (CiscoJtapiException) t;
			report( "CiscoJtapiException: "+je.getErrorDescription() );
		}
		Trace.report( this, EventFormat.format( msg, ev ), t );
	}
	
	public String toString()
	{
		return "testJtapi";
	}
	
	public void providerChangedEvent( ProvEv[] evs )
	{
		for (ProvEv ev: evs)
		{
			report( "delivering event", ev );
			try
			{
				if (!providerChangedEvent( ev ))
					report( "ignoring event", ev );
			}
			catch ( Exception e )
			{
				report( "caught exception delivering event", ev, e );
			}
		}
	}

	private boolean providerChangedEvent( ProvEv ev )
		throws Exception
	{
		switch (ev.getID())
		{
			case ProvInServiceEv.ID:
				return provInService( (ProvInServiceEv) ev );
			
			default:
				return false;
		}
	}

	private boolean provInService( ProvInServiceEv ev )
		throws Exception
	{
		Terminal x = provider.getTerminal( term );
		
		if (x instanceof CiscoRouteTerminal)
		{
			CiscoRouteTerminal rt = (CiscoRouteTerminal) x;
			
			rt.addObserver( this );
			rt.addCallObserver( this );
			
			CiscoMediaCapability[] caps =
			{
				new CiscoG711MediaCapability(
					CiscoG711MediaCapability.FRAMESIZE_TWENTY_MILLISECOND_PACKET ),
				new CiscoG711MediaCapability(
					CiscoG711MediaCapability.FRAMESIZE_THIRTY_MILLISECOND_PACKET )
			};
			
			try
			{
				rt.register( caps, CiscoRouteTerminal.DYNAMIC_MEDIA_REGISTRATION );
			}
			catch ( CiscoRegistrationException e )
			{
				report( "caught exception registering "+rt, e );
			}
			
			return true;
		}
		else if (x instanceof CiscoMediaTerminal)
		{
			CiscoMediaTerminal mt = (CiscoMediaTerminal) x;
			
			mt.addObserver( this );
			mt.addCallObserver( this );
			
			CiscoMediaCapability[] caps =
			{
				new CiscoG711MediaCapability(
					CiscoG711MediaCapability.FRAMESIZE_TWENTY_MILLISECOND_PACKET ),
				new CiscoG711MediaCapability(
					CiscoG711MediaCapability.FRAMESIZE_THIRTY_MILLISECOND_PACKET )
			};
			
			try
			{
				mt.register( caps );
			}
			catch ( CiscoRegistrationException e )
			{
				report( "caught exception registering "+mt, e );
			}
			
			return true;
		}
		return false;
	}

	public void terminalChangedEvent( TermEv[] evs )
	{
		for (TermEv ev: evs)
		{
			report( "delivering event", ev );
			try
			{
				if (!terminalChangedEvent( ev ))
					report( "ignoring event", ev );
			}
			catch ( Exception e )
			{
				report( "caught exception delivering event", ev, e );
			}
		}
	}

	private boolean terminalChangedEvent( TermEv ev )
		throws Exception
	{
		switch (ev.getID())
		{
			case CiscoTermInServiceEv.ID:
				return ciscoTermInService( (CiscoTermInServiceEv) ev );
			
			default:
				return false;
		}
	}

	private boolean ciscoTermInService( CiscoTermInServiceEv ev )
		throws Exception
	{
		Terminal t = ev.getTerminal();
		report( "term "+t+" in service" );
		Address[] addrs = t.getAddresses();
		for (Address addr: addrs)
			addr.addObserver( this );
		return true;
	}

	public void callChangedEvent( CallEv[] evs )
	{
		for (CallEv ev: evs)
		{
			report( "delivering event", ev );
			try
			{
				if (!callChangedEvent( ev ))
					report( "ignoring event", ev );
			}
			catch ( Exception e )
			{
				report( "caught exception delivering event", ev, e );
			}
		}
	}

	private boolean callChangedEvent( CallEv ev )
	{
		// ignore for now
		return false;
	}

	public void addressChangedEvent( AddrEv[] evs )
	{
		for (AddrEv ev: evs)
		{
			report( "delivering event", ev );
			try
			{
				if (!addressChangedEvent( ev ))
					report( "ignoring event", ev );
			}
			catch ( Exception e )
			{
				report( "caught exception delivering event", ev, e );
			}
		}
	}

	private boolean addressChangedEvent( AddrEv ev ) throws Exception
	{
		switch (ev.getID())
		{
			case CiscoAddrInServiceEv.ID:
				return ciscoAddrInService( (CiscoAddrInServiceEv) ev );
			
			default:
				return false;
		}
	}

	private boolean ciscoAddrInService( CiscoAddrInServiceEv ev ) throws Exception
	{
		CiscoAddress a = (CiscoAddress) ev.getAddress();
		Terminal t = ev.getTerminal();
		
		report( a.getName()+"@"+t.getName()+" in service" );
		
		a.setAutoAcceptStatus( CiscoAddress.AUTOACCEPT_ON, t );
		return true;
	}
	
	private static class EventFormat
	{
		public static String format( String msg, Ev ev )
		{
			StringBuffer sb = new StringBuffer();
			
			if (msg != null && msg.length() > 0)
			{
				sb.append( msg );
				sb.append( ": " );
			}
			
			Class c = ev.getClass();
			String className = c.getName();
			sb.append( className );
			
			sb.append( " { " );
			Method[] ms = c.getMethods();
			for (Method m: ms)
			{
				String name = m.getName();
				if (name.startsWith( "get" ))
				{
					String n = m.getName().substring( 3 );
					
					if (denySet.contains( n ))
						continue;
					
					ValueFormatter vf = formatters.get( className+'.'+n );
					if (vf == null)
					{
						vf = formatters.get( n );
						if (vf == null)
							vf = GENERIC_FORMATTER;
					}
					
					try
					{
						m.setAccessible( true );
						Object o = m.invoke( ev );
						sb.append( n );
						sb.append( '=' );
						sb.append( o != null ? vf.format( c, o ) : "null" );
						sb.append( "; " );
					}
					catch ( Exception e )
					{
						Trace.report( "caught exception fetching value for "+n, e );
						sb.append( n );
						sb.append( "=(error); " );
					}
				}
			}
			sb.append( '}' );
			return sb.toString();
		}
		
		public final static ValueFormatter GENERIC_FORMATTER = new ValueFormatter()
		{
			public String format( Class c, Object o )
			{
				return o.toString();
			}
		};
		
		private final static Map<String, ValueFormatter> formatters = new HashMap<String, ValueFormatter>();
		static
		{
			formatters.put( "Cause", new CauseValueFormatter() );
			formatters.put( "CallControlCause", new CauseValueFormatter() );
			formatters.put( "MetaCode", new MetaCodeValueFormatter() );
		}
		
		private final static Set<String> denySet = new HashSet<String>();
		static
		{
			denySet.add( "Class" );
			denySet.add( "ID" );
			denySet.add( "Observed" );
		}
		
		public interface ValueFormatter
		{
			public String format( Class c, Object o );
		}
		
		abstract public static class XlatValueFormatter implements ValueFormatter
		{
			public String getXlat( Object o )
			{
				return xlats.get( o );
			}
			
			public void putXlat( Object o, String xlat )
			{
				xlats.put( o, xlat );
			}
			
			abstract protected boolean matches( String name );
			
			abstract protected String format( String name );
			
			public String getXlats( Class c, Object o )
			{
				String r = null;
				Field[] fs = c.getFields();
				for (Field f: fs)
				{
					String name = f.getName();
					if ((f.getModifiers() & Modifier.STATIC) != Modifier.STATIC)
						continue;
					if (!matches( name ))
						continue;
					String n = format( name );
					try
					{
						Object x = f.getInt( null );
						if (!xlats.containsKey( x ))
						{
							xlats.put( x, n );
							if (o.equals( x ))
								r = n;
						}
					}
					catch ( Exception e )
					{
						Trace.report( "caught exception fetching value for "+name, e );
						continue;
					}
				}
				return r;
			}
			
			private Map<Object, String> xlats = Collections.synchronizedMap( new HashMap<Object, String>() );
		}
		
		public static class MetaCodeValueFormatter extends XlatValueFormatter
		{
			public String format( Class c, Object o )
			{
				String xlat = getXlat( o );
				if (xlat == null)
				{
					xlat = getXlats( c, o );
					if (xlat == null)
					{
						xlat = GENERIC_FORMATTER.format( c, o );
						putXlat( o, xlat );
					}
				}
				return xlat;
			}
			
			protected boolean matches( String name )
			{
				return name.startsWith( "META_" );
			}
			
			protected String format( String name )
			{
				return name.substring( 5 );
			}
		}
		
		public static class CauseValueFormatter extends XlatValueFormatter
		{
			public String format( Class c, Object o )
			{
				String xlat = getXlat( o );
				if (xlat == null)
				{
					xlat = getXlats( c, o );
					if (xlat == null)
					{
						xlat = GENERIC_FORMATTER.format( c, o );
						putXlat( o, xlat );
					}
				}
				return xlat;
			}
			
			protected boolean matches( String name )
			{
				return name.startsWith( "CAUSE_" );
			}
			
			protected String format( String name )
			{
				return name.substring( 6 );
			}
		}
	}
}
