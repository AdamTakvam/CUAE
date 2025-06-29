/* $Id: Trace.java 32219 2007-05-16 20:57:26Z wert $
 *
 * Created by wert on Feb 16, 2005
 *
 * Copyright (c) 2005 Metreos, Inc. All rights reserved.
 */

package metreos.util;

import java.util.Iterator;
import java.util.List;
import java.util.Vector;

/**
 * Support for simple event tracing.
 */
abstract public class Trace
{
	/**
	 * Constructs the Trace.
	 */
	public Trace()
	{
		// nothing to do.
	}
	
	/**
	 * Constructs the Trace.
	 *
	 * @param acceptMask
	 */
	public Trace( int acceptMask )
	{
		this.acceptMask = acceptMask;
	}
	
	/**
	 * Constructs the Trace.
	 *
	 * @param url
	 */
	public Trace( URL url )
	{
		Integer i = url.getIntegerTerm( "acceptMask" );
		if (i != null)
			acceptMask = i.intValue();
	}
	
	////////////////
	// EVENT MASK //
	////////////////

	/**
	 * @param eventMask
	 * @return true if the event mask matches the current accept
	 * mask.
	 */
	public boolean matchesMask( int eventMask )
	{
		return (eventMask & acceptMask) != 0;
	}
	
	/**
	 * @return the current event mask value.
	 */
	public int getMask()
	{
		return acceptMask;
	}
	
	/**
	 * @param mask the new event mask value.
	 * @return this trace.
	 */
	public Trace setMask( int mask )
	{
		acceptMask = mask;
		return this;
	}
	
	private int acceptMask = DEFAULT_MASK;
	
	/**
	 * Default value of event mask (-1). Accepts all events.
	 */
	public final static int DEFAULT_MASK = -1;
	
	/**
	 * Event mask used to report events which denote failures.
	 */
	public final static int FAILURE_MASK = 1;
	
	/**
	 * Event mask used to report events which denote non-failures.
	 */
	public final static int NORMAL_MASK = 2;
	
	///////////////
	// REPORTING //
	///////////////

	/**
	 * @param force force logging even if log levels would prevent.
	 * @param msg
	 */
	public void rprt( boolean force, String msg )
	{
		rprt( force, null, m( msg ), null );
	}

	/**
	 * @param force force logging even if log levels would prevent.
	 * @param msg
	 */
	public void rprt( boolean force, M msg )
	{
		rprt( force, null, msg, null );
	}

	/**
	 * @param t
	 */
	public void rprt( Throwable t )
	{
		rprt( false, null, CAUGHT_EXCEPTION, t );
	}

	/**
	 * @param msg
	 * @param t
	 */
	public void rprt( String msg, Throwable t )
	{
		rprt( false, null, m( msg ), t );
	}

	/**
	 * @param msg
	 * @param t
	 */
	public void rprt( M msg, Throwable t )
	{
		rprt( false, null, msg, t );
	}

	/**
	 * @param who
	 * @param msg
	 */
	public void rprt( Object who, String msg )
	{
		rprt( false, who, m( msg ), null );
	}

	/**
	 * @param who
	 * @param msg
	 */
	public void rprt( Object who, M msg )
	{
		rprt( false, who, msg, null );
	}

	/**
	 * @param who
	 * @param t
	 */
	public void rprt( Object who, Throwable t )
	{
		rprt( false, who, CAUGHT_EXCEPTION, t );
	}

	/**
	 * @param force force logging even if log levels would prevent.
	 * @param who
	 * @param msg
	 * @param t
	 */
	public void rprt( boolean force, Object who, String msg, Throwable t )
	{
		rprt( force, who, m( msg ), t );
	}

	/**
	 * @param force force logging even if log levels would prevent.
	 * @param who
	 * @param msg
	 * @param t
	 */
	public void rprt( boolean force, Object who, M msg, Throwable t )
	{
		int eventMask = getMask( force, t );
		
		if (who == null)
			who = Thread.currentThread();
		
		rprt( System.currentTimeMillis(), eventMask, who, msg, t );
	}

	////////////////////
	// REPORTING IMPL //
	////////////////////
	
	/**
	 * Starts the current trace, whatever that means.
	 * @return this trace object.
	 * @throws RuntimeException if there was a problem starting.
	 */
	abstract public Trace start() throws RuntimeException;
	
	/**
	 * @param when
	 * @param eventMask
	 * @param who
	 * @param msg
	 * @param t
	 */
	abstract protected void rprt( long when, int eventMask, Object who, M msg,
		Throwable t );

	/**
	 * Stops the current trace, whatever that means.
	 */
	abstract public void stop();

	////////////////////
	// STATIC METHODS //
	////////////////////

	/**
	 * @param msg
	 */
	public static void report( String msg )
	{
		getTrace().rprt( false, msg );
	}

	/**
	 * @param force force logging even if log levels would prevent.
	 * would normally prevent it.
	 * @param msg
	 */
	public static void report( boolean force, String msg )
	{
		getTrace().rprt( force, msg );
	}

	/**
	 * @param msg
	 */
	public static void report( M msg )
	{
		getTrace().rprt( false, msg );
	}

	/**
	 * @param force force logging even if log levels would prevent.
	 * would normally prevent it.
	 * @param msg
	 */
	public static void report( boolean force, M msg )
	{
		getTrace().rprt( force, msg );
	}
	
	/**
	 * @param t
	 */
	public static void report( Throwable t )
	{
		getTrace().rprt( t );
	}

	/**
	 * @param msg
	 * @param t
	 */
	public static void report( String msg, Throwable t )
	{
		getTrace().rprt( msg, t );
	}

	/**
	 * @param msg
	 * @param t
	 */
	public static void report( M msg, Throwable t )
	{
		getTrace().rprt( msg, t );
	}
	
	/**
	 * @param who
	 * @param msg
	 */
	public static void report( Object who, String msg )
	{
		getTrace().rprt( who, msg );
	}
	
	/**
	 * @param who
	 * @param msg
	 */
	public static void report( Object who, M msg )
	{
		getTrace().rprt( who, msg );
	}
	
	/**
	 * @param who
	 * @param t
	 */
	public static void report( Object who, Throwable t )
	{
		getTrace().rprt( who, t );
	}
	
	/**
	 * @param who
	 * @param msg
	 * @param t
	 */
	public static void report( Object who, String msg, Throwable t )
	{
		getTrace().rprt( false, who, msg, t );
	}
	
	/**
	 * @param who
	 * @param msg
	 * @param t
	 */
	public static void report( Object who, M msg, Throwable t )
	{
		getTrace().rprt( false, who, msg, t );
	}
	
	private static int getMask( boolean force, Throwable t )
	{
		if (force)
			return FAILURE_MASK | NORMAL_MASK;
		return t != null ? FAILURE_MASK : NORMAL_MASK;
	}
	
	/**
	 * @param value
	 * @return a new message assembler with the specified
	 * value as the first element.
	 */
	public static M m( Object value )
	{
		return new M( value );
	}
	
	/**
	 * @param value
	 * @return a new message assembler with the specified
	 * value as the first element.
	 */
	public static M m( int value )
	{
		return new M( value );
	}
	
	/**
	 * @param value
	 * @return a new message assembler with the specified
	 * value as the first element.
	 */
	public static M m( long value )
	{
		return new M( value );
	}
	
	/**
	 * @param value
	 * @return a new message assembler with the specified
	 * value as the first element.
	 */
	public static M m( double value )
	{
		return new M( value );
	}
	
	/**
	 * @param value
	 * @return a new message assembler with the specified
	 * value as the first element.
	 */
	public static M m( boolean value )
	{
		return new M( value );
	}

	/**
	 * @return the current trace object. if there isn't one, default one is created.
	 */
	public static Trace getTrace()
	{
		if (trace == null)
		{
			synchronized (Trace.class)
			{
				if (trace == null)
				{
					trace = new DefaultTrace();
					trace.start();
				}
			}
		}
		return trace;
	}
	
	/**
	 * @param newTrace
	 * @return the old trace.
	 */
	public static synchronized Trace setTrace( Trace newTrace )
	{
		Trace oldTrace = trace;
		trace = newTrace;
		return oldTrace;
	}
	
	private static Trace trace;
	
	/**
	 * Message associated with reports of a throwable but
	 * no message.
	 */
	public final static String CAUGHT_EXCEPTION = "caught exception";
	
	/**
	 * A message assembler. Delays concatenation and toString
	 * of objects until they are actually going to be written
	 * to the log.
	 */
	public static class M
	{
		/**
		 * Constructs the message assembler.
		 *
		 * @param value
		 */
		public M( Object value )
		{
			first = value;
		}
		
		/**
		 * Constructs the M.
		 *
		 * @param value
		 */
		public M( int value )
		{
			this( new Integer( value ) );
		}

		/**
		 * Constructs the M.
		 *
		 * @param value
		 */
		public M( long value )
		{
			this( new Long( value ) );
		}

		/**
		 * Constructs the M.
		 *
		 * @param value
		 */
		public M( double value )
		{
			this( new Double( value ) );
		}

		/**
		 * Constructs the M.
		 *
		 * @param value
		 */
		public M( boolean value )
		{
			this( new Boolean( value ) );
		}

		private Object first;
		
		private List<Object> others;
		
		/**
		 * Adds the specified value to the message assembler values.
		 * @param value
		 * @return this message assembler
		 */
		public M a( Object value )
		{
			if (others == null)
				others = new Vector<Object>();
			others.add( value );
			return this;
		}
		
		/**
		 * Adds the specified value to the message assembler values.
		 * @param value
		 * @return this message assembler
		 */
		public M a( int value )
		{
			return a( new Integer( value ) );
		}
		
		/**
		 * Adds the specified value to the message assembler values.
		 * @param value
		 * @return this message assembler
		 */
		public M a( long value )
		{
			return a( new Long( value ) );
		}
		
		/**
		 * Adds the specified value to the message assembler values.
		 * @param value
		 * @return this message assembler
		 */
		public M a( double value )
		{
			return a( new Double( value ) );
		}

		/**
		 * Adds the specified value to the message assembler values.
		 * @param value
		 * @return this message assembler
		 */
		public M a( boolean value )
		{
			return a( new Boolean( value ) );
		}

		@Override
		public String toString()
		{
			StringBuffer sb = new StringBuffer();
			print( sb );
			return sb.toString();
		}
		
		/**
		 * Prints the message parts to the specified stream.
		 * @param sb
		 */
		public void print( StringBuffer sb )
		{
			sb.append( first );
			if (others != null)
			{
				Iterator<Object> i = others.iterator();
				while (i.hasNext())
					sb.append( i.next() );
			}
		}
	}

	/**
	 * Stops the current trace if there is one and clears it.
	 */
	public static void shutdown()
	{
		Trace tr = setTrace( null );
		if (tr != null)
			tr.stop();
	}
}