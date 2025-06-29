/* $Id: DbException.java 7038 2005-06-06 21:26:17Z wert $
 *
 * Created by wert on May 17, 2005.
 *
 * Copyright (c) 2005 Metreos, Inc. All rights reserved.
 */

package metreos.core.db;

import java.sql.SQLException;

/**
 * Description of DbException.
 */
public class DbException extends Exception
{
	/**
	 * 
	 */
	private static final long serialVersionUID = -3197141449612723124L;

	/**
	 * Constructs the DbException.
	 *
	 * @param msg
	 * @param failure
	 */
	public DbException( String msg, SQLException failure )
	{
		super( msg, failure );
	}

	/**
	 * Constructs the DbException.
	 *
	 * @param msg
	 */
	public DbException( String msg )
	{
		super( msg );
	}
}
