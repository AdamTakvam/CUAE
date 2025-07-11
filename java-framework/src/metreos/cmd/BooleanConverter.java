/* $Id: BooleanConverter.java 6937 2005-05-31 16:26:56Z wert $
 *
 * Created by wert on Apr 19, 2005.
 *
 * Copyright (c) 2005 Metreos, Inc. All rights reserved.
 */

package metreos.cmd;

import java.util.HashMap;
import java.util.Map;

/**
 * Handle translation of various types of boolean values.
 */
public class BooleanConverter
{
	/**
	 * @param value
	 * @return result of translation of boolean value.
	 */
	public static Boolean valueOf( String value )
	{
		Boolean v = values.get( value.toLowerCase() );
		if (v == null)
			throw new IllegalArgumentException( "bad boolean value: "+value );
		return v;
	}
	
	/**
	 * Defines static key pairs to use for true/false item conversions.
	 * @param trueKey
	 * @param falseKey
	 */
	public static void define( String trueKey, String falseKey )
	{
		if (trueKey != null)
			values.put( trueKey.toLowerCase(), Boolean.TRUE );
		
		if (falseKey != null)
			values.put( falseKey.toLowerCase(), Boolean.FALSE );
	}
	
	private final static Map<String,Boolean> values = new HashMap<String,Boolean>();
	static
	{
		define( "true", "false" );
		define( "yes", "no" );
		define( "1", "0" );
	}
}
