/* $Id: ClassURL.java 7054 2005-06-07 17:48:33Z wert $
 *
 * Created by wert on Apr 13, 2005.
 *
 * Copyright (c) 2005 Metreos, Inc. All rights reserved.
 */

package metreos.util;

import java.lang.reflect.Constructor;
import java.lang.reflect.InvocationTargetException;

/**
 * Description of ClassURL.
 */
public class ClassURL
{
	/**
	 * @param clss
	 * @param url
	 * @return an instance of the specified class as defined by the url
	 */
	@SuppressWarnings(value={"unchecked"})
	public static Object makeInstance( Class clss, URL url )
	{
		if (!url.getScheme().equals( "class" ))
			throw new IllegalArgumentException( "url scheme not 'class'" );
		
		// find the class.
		
		String className = url.getUri();
		Class c;
		try
		{
			c = Class.forName( className );
		}
		catch ( ClassNotFoundException e )
		{
			throw new IllegalArgumentException(
				"url class ("+className+") could not be found" );
		}
		
		// make sure it is compatible.
		
		if (!clss.isAssignableFrom( c ))
			throw new IllegalArgumentException(
				"url class ("+c+") is not a subclass of "+clss );
		
		// find the constructor.
		
		Class[] params = { URL.class };
		Constructor constructor;
		try
		{
			constructor = c.getConstructor( params );
		}
		catch ( SecurityException e )
		{
			IllegalArgumentException iae = new IllegalArgumentException(
				"url class ("+c+") constructor could not be accessed" );
			iae.initCause( e );
			throw iae;
		}
		catch ( NoSuchMethodException e )
		{
			IllegalArgumentException iae = new IllegalArgumentException(
				"url class ("+c+") constructor could not be found" );
			iae.initCause( e );
			throw iae;
		}
		
		// make an instance.
		
		Object[] args = { url };
		try
		{
			return constructor.newInstance( args );
		}
		catch ( IllegalArgumentException e )
		{
			IllegalArgumentException iae = new IllegalArgumentException(
				"url class ("+c+") constructor: illegal args" );
			iae.initCause( e );
			throw iae;
		}
		catch ( InstantiationException e )
		{
			IllegalArgumentException iae = new IllegalArgumentException(
				"url class ("+c+") constructor: could not instantiate" );
			iae.initCause( e );
			throw iae;
		}
		catch ( IllegalAccessException e )
		{
			IllegalArgumentException iae = new IllegalArgumentException(
				"url class ("+c+") constructor: illegal access" );
			iae.initCause( e );
			throw iae;
		}
		catch ( InvocationTargetException e )
		{
			IllegalArgumentException iae = new IllegalArgumentException(
				"url class ("+c+") constructor: target failed" );
			iae.initCause( e.getTargetException() );
			throw iae;
		}
	}
}
