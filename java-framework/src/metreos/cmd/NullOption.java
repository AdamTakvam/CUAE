/* $Id: NullOption.java 30151 2007-03-06 21:47:46Z wert $
 *
 * Created by wert on Mar 27, 2005.
 * 
 * Copyright (c) 2005 Metreos, Inc. All rights reserved.
 */

package metreos.cmd;

import metreos.util.Assertion;

/**
 * An option which has no value.
 */
public class NullOption extends Option
{
	/**
	 * @param cp
	 * @param tokens
	 * @param method
	 * @param description
	 * @param flags
	 * @throws Exception
	 */
	public NullOption( CommandParser cp, String tokens, String method,
		String description, int flags ) throws Exception
	{
		super( cp, tokens, null, method, PARAMS, description, flags, null, null );
	}
	
	private final static Class[] PARAMS =
		{ CommandParser.class, Option.class, String.class };
	
	@Override
	public boolean wantsValue()
	{
		return false;
	}

	@Override
	protected Object convertValue( String value )
	{
		throw new UnsupportedOperationException();
	}

	@Override
	protected Object[] getArgs( String token, Object value )
	{
		Assertion.check( value == null, "value == null" );
		return new Object[] { getCommandParser(), this, token };
	}
}