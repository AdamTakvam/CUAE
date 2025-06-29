/* $Id: AssertionException.java 7054 2005-06-07 17:48:33Z wert $
 *
 * Created by wert on Jan 25, 2005
 *
 * Copyright (c) 2005 Metreos, Inc. All rights reserved.
 */

package metreos.util;

/**
 * Exception thrown upon assertion failure.
 * 
 * @see Assertion#check(boolean, String)
 */
public class AssertionException extends RuntimeException
{
	/**
	 * 
	 */
	private static final long serialVersionUID = 3545520603445735478L;

	/**
	 * Constructs an assertion exception.
	 * 
	 * @param message describes the failure.
	 */
	public AssertionException( String message )
	{
		super( message );
	}
}
