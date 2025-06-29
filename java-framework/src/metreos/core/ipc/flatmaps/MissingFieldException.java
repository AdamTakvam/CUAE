/* $Id: MissingFieldException.java 6808 2005-05-20 17:59:29Z wert $
 *
 * Created by wert on May 2, 2005.
 *
 * Copyright (c) 2005 Metreos, Inc. All rights reserved.
 */

package metreos.core.ipc.flatmaps;

/**
 * Description of MissingFieldException.
 */
public class MissingFieldException extends Exception
{
	/**
	 * 
	 */
	private static final long serialVersionUID = 4120853278344034356L;

	/**
	 * Constructs the MissingFieldException.
	 *
	 * @param field the id of the field that is missing.
	 */
	public MissingFieldException( int field )
	{
		super( "missing field "+field );
		this.field = field;
	}

	private final int field;
	
	/**
	 * @return the id of the field that is missing.
	 */
	public int getField()
	{
		return field;
	}
}
