/* $Id: OptParamBase.java 6338 2005-04-19 20:19:53Z wert $
 *
 * Created by wert on Mar 27, 2005.
 * 
 * Copyright (c) 2005 Metreos, Inc. All rights reserved.
 */

package metreos.cmd;

import java.lang.reflect.Method;

/**
 * Shared base class of options and parameters.
 */
abstract public class OptParamBase
{
	/**
	 * @param cp
	 * @param name
	 * @param method
	 * @param params
	 * @param description
	 * @param isRequired 
	 * @param constraint 
	 * @throws Exception
	 */
	public OptParamBase( CommandParser cp, String name, String method,
		Class[] params, String description, boolean isRequired,
		Constraint constraint ) throws Exception
	{
		this.cp = cp;
		this.name = name;
		this.method = findMethod( cp.program, method, params );
		this.description = description;
		this.isRequired = isRequired;
		this.constraint = constraint;
	}
	
	private final CommandParser cp;
	
	private final String name;

	private final Method method;
	
	private final String description;
	
	private final boolean isRequired;
	
	private final Constraint constraint;
	
	/**
	 * @return the command parser of this item.
	 */
	public CommandParser getCommandParser()
	{
		return cp;
	}
	
	/**
	 * @return the name of this item.
	 */
	public String getName()
	{
		return name;
	}
	
	/**
	 * @return the description of this item.
	 */
	public String getDescription()
	{
		return description;
	}
	
	/**
	 * @return true if this item is required.
	 */
	public boolean isRequired()
	{
		return isRequired;
	}
	
	//////////
	// HELP //
	//////////
	
	/**
	 * Shows a very short synopsis of the item for the command line usage message.
	 *
	 */
	abstract public void showShortDescription();
	
	/**
	 * Shows the longer "help" description which appears underneath the
	 * command line usage message.
	 *
	 */
	abstract public void showLongDescription();

	/**
	 * Shows the text description of the item.
	 */
	protected void showDescription()
	{
		System.err.print( DESC_FLAG );
		System.err.println( description );
	}

	/**
	 * Shows whether the item is required or not.
	 */
	protected void showIsRequired()
	{
		if (isRequired())
		{
			System.err.print( DESC_FLAG );
			System.err.println( "required (must be specified)." );
		}
	}
	
	/**
	 * Shows the constraint if any.
	 */
	protected void showConstraint()
	{
		if (constraint != null)
		{
			System.err.print( DESC_FLAG );
			System.err.print( "constraint: " );
			System.err.println( constraint );
		}
	}
	
	/**
	 * Flag value to use at the beginning of the line for the
	 * name of the item.
	 */
	protected final static String NAME_FLAG = "   ";
	
	/**
	 * Flag value to use at beginning of the line for descriptive
	 * text of an item.
	 */
	protected final static String DESC_FLAG = "      # ";
	
	///////////
	// VALUE //
	///////////
	
	/**
	 * Invokes the constraint checkValue method if there is
	 * a constraint.
	 * 
	 * @param who
	 * 
	 * @param value
	 * 
	 * @return true if the parameter is ok, false otherwise. If false
	 * is returned, a message has been printed on System.err.
	 */
	public boolean checkValue( String who, Object value )
	{
		if (constraint != null)
		{
			try
			{
				constraint.checkValue( value );
			}
			catch ( Exception e )
			{
				System.err.print( who );
				System.err.print( ": validation of '" );
				System.err.print( value );
				System.err.print( "' failed: " );
				System.err.println( e.getMessage() );
				return false;
			}
		}
		return true;
	}
	
	/**
	 * @param value
	 * @return value converted as appropriate for this item
	 */
	abstract protected Object convertValue( String value );

	////////////
	// METHOD //
	////////////

	/**
	 * @param program
	 * @param method
	 * @param params
	 * @return the method for prog given params.
	 * @throws Exception
	 */
	private static Method findMethod( Program program, String method,
		Class[] params ) throws Exception
	{
		return program.getClass().getMethod( method, params );
	}

	/**
	 * @param args
	 * @return value returned from method, or true if null returned.
	 * @throws Exception
	 */
	public boolean callMethod( Object[] args )
		throws Exception
	{
		Object result = method.invoke( cp.program, args );
		
		if (result == null)
			return true;
		
		return ((Boolean) result).booleanValue();
	}
}