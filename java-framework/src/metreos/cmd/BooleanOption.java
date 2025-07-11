/* $Id: BooleanOption.java 30151 2007-03-06 21:47:46Z wert $
 *
 * Created by wert on Mar 27, 2005.
 * 
 * Copyright (c) 2005 Metreos, Inc. All rights reserved.
 */

package metreos.cmd;

/**
 * An option whose value is a boolean.
 */
public class BooleanOption extends Option
{
	/**
	 * @param cp
	 * @param tokens
	 * @param name
	 * @param method
	 * @param description
	 * @param flags
	 * @param defaultValue
	 * @param constraint
	 * @throws Exception
	 */
	public BooleanOption( CommandParser cp, String tokens, String name, String method,
		String description, int flags, Boolean defaultValue, Constraint constraint )
		throws Exception
	{
		super( cp, tokens, name, method, PARAMS, description, flags, defaultValue, constraint );
	}
	
	private final static Class[] PARAMS =
		{ CommandParser.class, Option.class, String.class, Boolean.class };

	@Override
	protected Object convertValue( String value )
	{
		return BooleanConverter.valueOf( value );
	}
}