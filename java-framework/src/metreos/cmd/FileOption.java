/* $Id: FileOption.java 30151 2007-03-06 21:47:46Z wert $
 *
 * Created by wert on Mar 27, 2005.
 * 
 * Copyright (c) 2005 Metreos, Inc. All rights reserved.
 */

package metreos.cmd;

import java.io.File;

/**
 * An option whose value is a file.
 */
public class FileOption extends Option
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
	public FileOption( CommandParser cp, String tokens, String name, String method,
		String description, int flags, File defaultValue, Constraint constraint )
		throws Exception
	{
		super( cp, tokens, name, method, PARAMS, description, flags, defaultValue, constraint );
	}
	
	private final static Class[] PARAMS =
		{ CommandParser.class, Option.class, String.class, File.class };

	@Override
	protected Object convertValue( String value )
	{
		return new File( value );
	}
}