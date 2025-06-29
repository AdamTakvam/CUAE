/* $Id: FileParameter.java 30151 2007-03-06 21:47:46Z wert $
 *
 * Created by wert on Mar 27, 2005.
 * 
 * Copyright (c) 2005 Metreos, Inc. All rights reserved.
 */

package metreos.cmd;

import java.io.File;

/**
 * A parameter whose value is a file.
 */
public class FileParameter extends Parameter
{
	/**
	 * @param cp
	 * @param name
	 * @param method
	 * @param description
	 * @param isRequired 
	 * @param constraint
	 * @throws Exception
	 */
	public FileParameter( CommandParser cp, String name, String method,
		String description, boolean isRequired, Constraint constraint ) throws Exception
	{
		super( cp, name, method, PARAMS, description, isRequired, constraint );
	}
	
	private final static Class[] PARAMS =
		{ CommandParser.class, Parameter.class, File.class };

	@Override
	public Object convertValue( String value )
	{
		return new File( value );
	}
}