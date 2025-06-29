package metreos.service.jtapi.test;

import java.lang.reflect.Field;
import java.lang.reflect.Method;
import java.lang.reflect.Modifier;
import java.util.Collections;
import java.util.HashMap;
import java.util.HashSet;
import java.util.Iterator;
import java.util.Map;
import java.util.Set;

import javax.telephony.Address;
import javax.telephony.JtapiPeerFactory;
import javax.telephony.Provider;
import javax.telephony.Terminal;
import javax.telephony.callcontrol.CallControlCallObserver;
import javax.telephony.events.AddrEv;
import javax.telephony.events.CallEv;
import javax.telephony.events.Ev;
import javax.telephony.events.ProvEv;
import javax.telephony.events.ProvInServiceEv;
import javax.telephony.events.ProvOutOfServiceEv;
import javax.telephony.events.TermEv;

import metreos.util.Assertion;
import metreos.util.Trace;

import com.cisco.jtapi.extensions.CiscoAddrAddedToTerminalEv;
import com.cisco.jtapi.extensions.CiscoAddrCreatedEv;
import com.cisco.jtapi.extensions.CiscoAddrInServiceEv;
import com.cisco.jtapi.extensions.CiscoAddrOutOfServiceEv;
import com.cisco.jtapi.extensions.CiscoAddrRemovedEv;
import com.cisco.jtapi.extensions.CiscoAddrRemovedFromTerminalEv;
import com.cisco.jtapi.extensions.CiscoAddress;
import com.cisco.jtapi.extensions.CiscoAddressObserver;
import com.cisco.jtapi.extensions.CiscoG711MediaCapability;
import com.cisco.jtapi.extensions.CiscoJtapiException;
import com.cisco.jtapi.extensions.CiscoJtapiPeer;
import com.cisco.jtapi.extensions.CiscoJtapiVersion;
import com.cisco.jtapi.extensions.CiscoMediaCapability;
import com.cisco.jtapi.extensions.CiscoMediaTerminal;
import com.cisco.jtapi.extensions.CiscoProvider;
import com.cisco.jtapi.extensions.CiscoProviderObserver;
import com.cisco.jtapi.extensions.CiscoRouteTerminal;
import com.cisco.jtapi.extensions.CiscoTermCreatedEv;
import com.cisco.jtapi.extensions.CiscoTermInServiceEv;
import com.cisco.jtapi.extensions.CiscoTermOutOfServiceEv;
import com.cisco.jtapi.extensions.CiscoTermRemovedEv;
import com.cisco.jtapi.extensions.CiscoTerminalObserver;

@SuppressWarnings("all")
public class testJtapi3 implements CiscoProviderObserver
{
	//
	// Arguments are :
	//
	// arg[0] = Server IP Address
	// arg[1] = Username
	// arg[2] = password
	// arg[3] = Terminal name
	//
	public static void main( String[] args )
		throws Exception
	{
		if (args.length != 4)
			throw new IllegalArgumentException( "usage: ccm login passwd term" );

		String connectionString = args[0] +";login=" + args[1] + ";passwd=" + args[2];
		String terminalName = args[3];
		
		Trace.report( "Connection String : " + connectionString );
		Trace.report( "CiscoJtapiVersion = "+new CiscoJtapiVersion() );
		
		CiscoJtapiPeer jtapiPeer = (CiscoJtapiPeer) JtapiPeerFactory.getJtapiPeer( null );
		CiscoProvider provider = (CiscoProvider) jtapiPeer.getProvider( connectionString );
		testJtapi3 prog = new testJtapi3( terminalName );
		provider.addObserver( prog );
	}

	public testJtapi3( String terminalName )
	{
		this.terminalName = terminalName;
	}
	
	private final String terminalName;
	
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
		Trace.report( this, EventFormat.format( msg, ev ), t );
	}
	
	public String toString()
	{
		return "pobs";
	}

	public void providerChangedEvent( ProvEv[] evs )
	{
		report( "pce delivering "+evs.length+" events" );
		for (ProvEv ev: evs)
		{
			report( "pce delivering event", ev );

			try
			{
				if (!providerChangedEvent( ev ))
					report( "pce ignoring event", ev );
			}

			catch ( Exception e )
			{
				if (e instanceof CiscoJtapiException)
				{
					CiscoJtapiException je = (CiscoJtapiException) e;
					report( "CiscoJtapiException: " + e.getMessage() );
				}

				report( "pce caught exception delivering event", ev, e );
			}
		}
		report( "pce done delivering "+evs.length+" events" );
	}

	private boolean providerChangedEvent( ProvEv ev )
		throws Exception
	{
		switch (ev.getID())
		{
			case ProvInServiceEv.ID:
				return provInService( (ProvInServiceEv) ev );

			case ProvOutOfServiceEv.ID:
				return provOutOfService( (ProvOutOfServiceEv) ev );
			
			case CiscoTermCreatedEv.ID:
				return ciscoTermCreated( (CiscoTermCreatedEv) ev );
			
			case CiscoTermRemovedEv.ID:
				return ciscoTermRemoved( (CiscoTermRemovedEv) ev );
			
			case CiscoAddrCreatedEv.ID:
				return ciscoAddrCreated( (CiscoAddrCreatedEv) ev );
				
			case CiscoAddrRemovedEv.ID:
				return ciscoAddrRemoved( (CiscoAddrRemovedEv) ev );
			
			case CiscoAddrAddedToTerminalEv.ID:
				return ciscoAddrAddedToTerminal( (CiscoAddrAddedToTerminalEv) ev );
			
			case CiscoAddrRemovedFromTerminalEv.ID:
				return ciscoAddrRemovedFromTerminal( (CiscoAddrRemovedFromTerminalEv) ev );
			
			default:
				return false;
		}
	}

	private boolean provInService( ProvInServiceEv ev )
		throws Exception
	{
		Provider p = ev.getProvider();
		if (mto == null)
		{
			Terminal t = p.getTerminal( terminalName );

			CiscoMediaCapability[] caps =
			{
				new CiscoG711MediaCapability(
					CiscoG711MediaCapability.FRAMESIZE_TWENTY_MILLISECOND_PACKET ),
				new CiscoG711MediaCapability(
					CiscoG711MediaCapability.FRAMESIZE_THIRTY_MILLISECOND_PACKET )
			};
			
			try
			{
				if (t instanceof CiscoRouteTerminal)
					mto = new MyRouteTerminalObserver( (CiscoRouteTerminal) t );
				else if (t instanceof CiscoMediaTerminal)
					mto = new MyMediaTerminalObserver( (CiscoMediaTerminal) t );
				else
					throw new IllegalArgumentException( "terminal "+t+" is not a CiscoRouteTerminal or a CiscoMediaTerminal" );
				
				mto.provInService( p );
				mto.register( caps );
				
				return true;
			}
			catch ( Exception e )
			{
				mto = null;
				throw e;
			}
		}
		else
		{
			mto.provInService( p );
			return true;
		}
	}

	private boolean provOutOfService( ProvOutOfServiceEv ev )
		throws Exception
	{
		if (mto != null)
		{
			mto.provOutOfService( ev.getProvider() );
			return true;
		}
		return false;
	}

	private boolean ciscoTermCreated( CiscoTermCreatedEv ev )
	{
		// TODO Auto-generated method stub
		return false;
	}

	private boolean ciscoTermRemoved( CiscoTermRemovedEv ev )
	{
		// TODO Auto-generated method stub
		return false;
	}

	private boolean ciscoAddrCreated( CiscoAddrCreatedEv ev )
	{
		Address a = ev.getAddress();
		Set<Terminal> terms = getTerms( a );
		
		if (mto != null)
			return mto.ciscoAddrCreated( a, terms );
		
		return false;
	}

	private boolean ciscoAddrRemoved( CiscoAddrRemovedEv ev )
	{
		Address a = ev.getAddress();
		Set<Terminal> terms = getTerms( a );
		
		if (mto != null)
			return mto.ciscoAddrRemoved( a, terms );
		
		return false;
	}

	private boolean ciscoAddrAddedToTerminal( CiscoAddrAddedToTerminalEv ev )
	{
		Address a = ev.getAddress();
		Terminal t = ev.getTerminal();
		
		if (mto != null)
			return mto.ciscoAddrAddedToTerminal( a, t );
		
		return false;
	}

	private boolean ciscoAddrRemovedFromTerminal( CiscoAddrRemovedFromTerminalEv ev )
	{
		Address a = ev.getAddress();
		Terminal t = ev.getTerminal();
		
		if (mto != null)
			return mto.ciscoAddrRemovedFromTerminal( a, t );
		
		return false;
	}
	
	private Set<Terminal> getTerms( Address a )
	{
		Set<Terminal> terms = new HashSet<Terminal>();
		Terminal[] ts = a.getTerminals();
		if (ts != null)
			for (Terminal t: ts)
				terms.add( t );
		return terms;
	}
	
	private MyTerminalObserver mto;
	
	private static class MyRouteTerminalObserver extends MyTerminalObserver
	{
		public MyRouteTerminalObserver( CiscoRouteTerminal t )
			throws Exception
		{
			this.t = t;
		}
	
		final private CiscoRouteTerminal t;
		
		final public Terminal t()
		{
			return t;
		}
		
		public void register( CiscoMediaCapability[] caps )
			throws Exception
		{
			try
			{
				addObservers();
				t.register( caps, CiscoRouteTerminal.DYNAMIC_MEDIA_REGISTRATION );
			}
			catch ( Exception e )
			{
				removeObservers();
				throw e;
			}
		}
		
		public void unregister()
			throws Exception
		{
			t.unregister();
			removeObservers();
		}
	}
	
	private static class MyMediaTerminalObserver extends MyTerminalObserver
	{
		public MyMediaTerminalObserver( CiscoMediaTerminal t )
			throws Exception
		{
			this.t = t;
		}
	
		final private CiscoMediaTerminal t;
		
		final public Terminal t()
		{
			return t;
		}
		
		public void register( CiscoMediaCapability[] caps )
			throws Exception
		{
			try
			{
				addObservers();
				t.register( caps );
			}
			catch ( Exception e )
			{
				removeObservers();
				throw e;
			}
		}
		
		public void unregister()
			throws Exception
		{
			t.unregister();
			removeObservers();
		}
	}
	
	abstract private static class MyTerminalObserver
		implements CiscoTerminalObserver, CallControlCallObserver, CiscoAddressObserver
	{
		public MyTerminalObserver()
		{
			// nothing to do.
		}

		abstract public Terminal t();
		
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
			Trace.report( this, EventFormat.format( msg, ev ), t );
		}
		
		public String toString()
		{
			return t().getName();
		}

		//////////////////
		// REGISTRATION //
		//////////////////
		
		abstract public void register( CiscoMediaCapability[] caps )
			throws Exception;

		abstract public void unregister()
			throws Exception;
		
		protected void addObservers()
			throws Exception
		{
			t().addObserver( this );
			t().addCallObserver( this );
		}
		
		protected void removeObservers()
		{
			t().removeCallObserver( this );
			t().removeObserver( this );
		}

		/////////////////////
		// PROVIDER EVENTS //
		/////////////////////
		
		public void provInService( Provider p )
		{
			report( "provider "+p+" in service" );
			Assertion.check( this.p == null, "this.p == null" );
			this.p = p;
		}
		
		public void provOutOfService( Provider p )
		{
			report( "provider "+p+" out of service" );
			Assertion.check( this.p == p, "this.p == p" );
			this.p = null;
		}
		
		private Provider p;

		public boolean ciscoAddrCreated( Address a, Set<Terminal> terms )
		{
			// is this address now assigned to this device?
			if (terms.contains( t() ))
				return addObserver( a );
			
			return false;
		}

		public boolean ciscoAddrRemoved( Address a, Set<Terminal> terms )
		{
			// is this address still assigned to this device?
			if (terms.contains( t() ))
				return false;
			
			return removeObserver( a );
		}

		public boolean ciscoAddrAddedToTerminal( Address a, Terminal t )
		{
			if (t.equals( t() ))
				return addObserver( a );
			
			return false;
		}
		
		public boolean ciscoAddrRemovedFromTerminal( Address a, Terminal t )
		{
			if (t.equals( t() ))
				return removeObserver( a );
			
			return false;
		}

		/////////////////////
		// TERMINAL EVENTS //
		/////////////////////
		
		public void terminalChangedEvent( TermEv[] evs )
		{
			report( "tce delivering "+evs.length+" events" );
			for (TermEv ev: evs)
			{
				report( "tce delivering event", ev );

				try
				{
					if (!terminalChangedEvent( ev ))
						report( "tce ignoring event", ev );
				}

				catch ( Exception e )
				{
					report( "tce caught exception delivering event", ev, e );
				}
			}
			report( "tce done delivering "+evs.length+" events" );
		}

		private boolean terminalChangedEvent( TermEv ev )
			throws Exception
		{
			switch (ev.getID())
			{
				case CiscoTermInServiceEv.ID:
					return ciscoTermInService( (CiscoTermInServiceEv) ev );

				case CiscoTermOutOfServiceEv.ID:
					return ciscoTermOutOfService( (CiscoTermOutOfServiceEv) ev );
				
				default:
					return false;
			}
		}

		private boolean ciscoTermInService( CiscoTermInServiceEv ev )
		{
			checkTerminal( ev.getTerminal() );
			report( "terminal in service" );
			
			Address[] as = t().getAddresses();
			if (as != null)
				for (Address a: as)
					addObserver( a );
			
			return true;
		}

		private boolean ciscoTermOutOfService( CiscoTermOutOfServiceEv ev )
		{
			checkTerminal( ev.getTerminal() );
			report( "terminal out of service" );
			
			for (Iterator<Address> i = getObservedAddrs(); i.hasNext();)
				removeObserver( i.next() );
			
			return true;
		}
		
		private void checkTerminal( Terminal t )
		{
			Assertion.check( t() == t, "t == t1" );
		}

		////////////////////////
		// OBSERVED ADDRESSES //
		////////////////////////

		private Iterator<Address> getObservedAddrs()
		{
			return new HashSet<Address>( observedAddrs ).iterator();
		}

		private boolean addObserver( Address a )
		{
			if (observedAddrs.add( a ))
			{
				try
				{
					report( "adding address observer "+a );
					a.addObserver( this );
					report( "added address observer "+a );
					return true;
				}
				catch ( Exception e )
				{
					report( "caught exception adding address observer "+a, e );
					observedAddrs.remove( a );
					return false;
				}
			}
			return false;
		}

		private boolean removeObserver( Address a )
		{
			if (observedAddrs.remove( a ))
			{
				report( "removing address observer "+a );
				a.removeObserver( this );
				report( "removed address observer "+a );
				return true;
			}
			return false;
		}
		
		private Set<Address> observedAddrs = Collections.synchronizedSet( new HashSet<Address>() );

		////////////////////
		// ADDRESS EVENTS //
		////////////////////

		public void addressChangedEvent( AddrEv[] evs )
		{
			report( "ace delivering "+evs.length+" events" );
			for (AddrEv ev: evs)
			{
				report( "ace delivering event", ev );

				try
				{
					if (!addressChangedEvent( ev ))
						report( "ace ignoring event", ev );
				}

				catch ( Exception e )
				{
					report( "ace caught exception delivering event", ev, e );
				}
			}
			report( "ace done delivering "+evs.length+" events" );
		}
		
		public boolean addressChangedEvent( AddrEv ev ) throws Exception
		{
			switch (ev.getID())
			{
				case CiscoAddrInServiceEv.ID:
					return ciscoAddrInService( (CiscoAddrInServiceEv) ev );
				
				case CiscoAddrOutOfServiceEv.ID:
					return ciscoAddrOutOfService( (CiscoAddrOutOfServiceEv) ev );
				
				default:
					return false;
			}
		}

		private boolean ciscoAddrInService( CiscoAddrInServiceEv ev )
			throws Exception
		{
			if (!ev.getTerminal().equals( t() ))
				return false;
			
			CiscoAddress a = (CiscoAddress) ev.getAddress();
			report( "address "+a+" in service" );
			a.setAutoAcceptStatus( CiscoAddress.AUTOACCEPT_ON, t() );
			return true;
		}

		private boolean ciscoAddrOutOfService( CiscoAddrOutOfServiceEv ev )
		{
			if (!ev.getTerminal().equals( t() ))
				return false;
			
			CiscoAddress a = (CiscoAddress) ev.getAddress();
			report( "address "+a+" out of service" );
			return true;
		}

		///////////////////
		// CALL OBSERVER //
		///////////////////

		public void callChangedEvent( CallEv[] evs )
		{
			report( "callChangedEvent delivering "+evs.length+" events" );
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
			report( "callChangedEvent done delivering "+evs.length+" events" );
		}

		private boolean callChangedEvent( CallEv ev )
		{
			return true;
		}
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
