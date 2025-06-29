/* $Id: Assertion.java 7054 2005-06-07 17:48:33Z wert $
 *
 * Created by wert on Jan 25, 2005
 *
 * Copyright (c) 2005 Metreos, Inc. All rights reserved.
 */

package metreos.util;

/**
 * Portable assertion checking.
 */
public class Assertion
{
	/**
	 * Checks a value for being true and if not it throws an exception
	 * with the specified description in the message.
	 * 
	 * @param value a boolean value which should be true.
	 * 
	 * @param description a description of value (e.g., "x < 100" ).
	 * 
	 * @throws AssertionException when value is not true.
	 */
	public static void check( boolean value, String description )
		throws AssertionException
	{
		if (!value)
			throw new AssertionException( "assertion failed: "+description );
	}
}
