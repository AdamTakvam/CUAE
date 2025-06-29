/* $Id: Option.java 30151 2007-03-06 21:47:46Z wert $
 *
 * Created by wert on Mar 27, 2005.
 * 
 * Copyright (c) 2005 Metreos, Inc. All rights reserved.
 */

package metreos.cmd;

import java.lang.reflect.InvocationTargetException;
import java.util.Iterator;
import java.util.List;
import java.util.StringTokenizer;
import java.util.Vector;

/**
 * A command line value which is introduced by a specified token.
 * An option is not dependent upon position on the command line
 * to be recognised (but still, position on the command line might
 * be important).
 */
abstract public class Option extends OptParamBase
{
	/**
	 * @param cp
	 * @param tokens
	 * @param name
	 * @param method
	 * @param params
	 * @param description
	 * @param flags
	 * @param defaultValue
	 * @param constraint
	 * @throws Exception
	 */
	public Option( CommandParser cp, String tokens, String name, String method,
		Class[] params, String description, int flags, Object defaultValue,
		Constraint constraint ) throws Exception
	{
		super( cp, name, method, params, description, (flags & REQUIRED) != 0,
			constraint );
		
		if (!setTokens( tokens ))
			throw new IllegalArgumentException( "no tokens for option" );
		
		this.flags = flags;
		this.defaultValue = defaultValue;
		
		if (constraint != null && defaultValue != null)
			constraint.checkValue( defaultValue );
	}

	private final int flags;
	
	private final Object defaultValue;
	
	/**
	 * The option has no special handling.
	 */
	public static final int NONE = 0;

	/**
	 * Description of HIDDEN
	 */
	public static final int HIDDEN = 1;

	/**
	 * Description of SINGLETON
	 */
	public static final int SINGLETON = 2;

	/**
	 * Description of REQUIRED
	 */
	public static final int REQUIRED = 4;
	
	/**
	 * @return an iterator over the tokens of an option.
	 */
	public Iterator<String> getTokens()
	{
		return tokens.iterator();
	}
	
	/**
	 * @return true if the option is hidden.
	 */
	public boolean isHidden()
	{
		return matchFlags( HIDDEN );
	}
	
	/**
	 * @return true if the option may only appear once.
	 */
	public boolean isSingleton()
	{
		return matchFlags( SINGLETON );
	}

	/**
	 * @param flag
	 * @return true if the flag has been specified, false otherwise
	 */
	private boolean matchFlags( int flag )
	{
		return (flags & flag) != 0;
	}
	
	/**
	 * @return the default value of the option.
	 */
	public Object getDefaultValue()
	{
		return defaultValue;
	}

	@Override
	public String toString()
	{
		return toString( primaryToken );
	}
	
	/**
	 * @param token
	 * @return the 
	 */
	protected String toString( String token )
	{
		if (getName() != null)
			return token+' '+getName();
		
		return primaryToken;
	}

	////////////
	// TOKENS //
	////////////

	private boolean setTokens( String s )
	{
		tokens = new Vector<String>();
		primaryToken = null;
		
		StringTokenizer st = new StringTokenizer( s, "|,; \r\n\t" );
		while (st.hasMoreTokens())
		{
			String t = st.nextToken();
			tokens.add( t );
			if (primaryToken == null)
				primaryToken = t;
		}
		
		return tokens.size() > 0;
	}
	
	/**
	 * @return the first token in the token list of the option.
	 */
	public String getPrimaryToken()
	{
		return primaryToken;
	}
	
	private List<String> tokens;
	
	private String primaryToken;

	//////////
	// HELP //
	//////////

	@Override
	public void showShortDescription()
	{
		System.err.print( isRequired() ? " {" : " [" );
		
		System.err.print( ' ' );
		System.err.print( primaryToken );
		
		if (wantsValue())
		{
			System.err.print( ' ' );
			System.err.print( getName() );
		}
		
		System.err.print( isRequired() ? " }" : " ]" );
	}
	
	@Override
	public void showLongDescription()
	{
		Iterator<String> i = tokens.iterator();
		while (i.hasNext())
		{
			System.err.print( NAME_FLAG );
			System.err.print( i.next() );
			if (wantsValue())
			{
				System.err.print( ' ' );
				System.err.print( getName() );
			}
			System.err.println();
		}
		
		showDescription();
		
		showIsRequired();
		
		showConstraint();
		
		if (isSingleton())
		{
			System.err.print( DESC_FLAG );
			System.err.println( "singleton (may only be specified once)." );
		}
		
		if (defaultValue != null)
		{
			System.err.print( DESC_FLAG );
			System.err.print( "default: " );
			System.err.println( defaultValue );
		}
	}

	///////////
	// VALUE //
	///////////
	
	/**
	 * @return true if this option needs a value, false otherwise.
	 */
	public boolean wantsValue()
	{
		return true;
	}

	/**
	 * @param token
	 * @param tkns
	 * @return value from tokens converted as appropriate.
	 */
	public Object getAndConvertValue( String token, Iterator<String> tkns )
	{
		if (!tkns.hasNext())
		{
			System.err.println( "option '"+toString( token )+"': missing value" );
			return null;
		}
		
		return convertValue( tkns.next() );
	}
	
	/**
	 * @param token
	 * @param value
	 * @return true if the value is ok, false otherwise.
	 */
	@Override
	public boolean checkValue( String token, Object value )
	{
		return super.checkValue( "option '"+toString( token )+"'", value );
	}

	/**
	 * @param token
	 * @param value
	 * @param hiddenOk
	 * @return value returned by called method.
	 * @throws Exception
	 */
	public boolean deliverValue( String token, Object value, boolean hiddenOk )
		throws Exception
	{
		try
		{
			if (!checkOkToDeliver( token, hiddenOk ))
				return false;
			
			return callMethod( getArgs( token, value ) );
		}
		catch ( InvocationTargetException e )
		{
			throw (Exception) e.getTargetException();
		}
	}

	/**
	 * @param token
	 * @param hiddenOk
	 * @return false if flags prevent delivery of the option value.
	 */
	private boolean checkOkToDeliver( String token, boolean hiddenOk )
	{
		if (isHidden() && !hiddenOk)
		{
			getCommandParser().report( "option '"+toString( token )+"' not allowed", null, null );
			return false;
		}
		
		if (getCommandParser().isAlreadySpecified( this, true ) && isSingleton())
		{
			getCommandParser().report( "option '"+toString( token )+"' not allowed more than once", null, null );
			return false;
		}
		
		return true;
	}

	/**
	 * @param token
	 * @param value
	 * @return the args to pass to the method.
	 */
	protected Object[] getArgs( String token, Object value )
	{
		return new Object[] { getCommandParser(), this, token, value };
	}
}