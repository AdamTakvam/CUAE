/* $Id: FlatmapException.java 6626 2005-05-11 21:21:12Z wert $
 *
 * Created by wert on Jan 25, 2005
 *
 * Copyright (c) 2005 Metreos, Inc. All rights reserved.
 */

package metreos.core.ipc.flatmaps;

import java.io.IOException;

/**
 * An exception thrown when there are problems reading a flatmap.
 */
public class FlatmapException extends IOException
{
	/**
	 * just in case somebody serializes it.
	 */
	private static final long serialVersionUID = 4049634607399776568L;

	/**
	 * Constructs a flatmap exception.
	 * 
	 * @param message describes the problem.
	 */
	public FlatmapException( String message )
	{
		super( message );
	}
	
	/**
	 * Constructs a flatmap exception.
	 * 
	 * @param message describes the problem.
	 * 
	 * @param cause an exception that caused the problem.
	 */
	public FlatmapException( String message, Exception cause )
	{
		super( message );
		initCause( cause );
	}
}
