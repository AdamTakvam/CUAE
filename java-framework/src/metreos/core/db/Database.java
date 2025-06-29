/* $Id: Database.java 7054 2005-06-07 17:48:33Z wert $
 *
 * Created by wert on May 17, 2005.
 *
 * Copyright (c) 2005 Metreos, Inc. All rights reserved.
 */

package metreos.core.db;

import java.sql.Connection;
import java.sql.DriverManager;
import java.sql.SQLException;

import metreos.util.Trace;


/**
 * Description of Database.
 */
abstract public class Database
{
	/**
	 * Constructs the Database.
	 *
	 * @param driverName
	 * @throws ClassNotFoundException 
	 */
	public Database( String driverName ) throws ClassNotFoundException
	{
		Class.forName( driverName );
	}

	private final static long RETRY_DELAY = 15*1000;
	
	/**
	 * @return a db connection.
	 */
	public DbConnection connect()
	{
		return newDbConnection( this );
	}

	/**
	 * @param database
	 * @return a DbConnection for this database.
	 */
	abstract protected DbConnection newDbConnection( Database database );

	/**
	 * @param maxDelay 
	 * @return a Connection appropriate for accessing the database.
	 * @throws DbException 
	 */
	public Connection openConnection( int maxDelay ) throws DbException
	{
		String url = getConnectionUrl();
		SQLException failure = null;
		
		long now = System.currentTimeMillis();
		long endTime = now + maxDelay;
		while (endTime >= now)
		{
			try
			{
				return DriverManager.getConnection( url );
			}
			catch ( SQLException e )
			{
				failure = e;
			}
			
			try
			{
				if (maxDelay > 0)
					Thread.sleep( RETRY_DELAY );
			}
			catch ( InterruptedException e )
			{
				break;
			}
			
			now = System.currentTimeMillis();
		}
		
		throw new DbException( "timeout opening connection", failure );
	}
	
	/**
	 * @return an appropriate connection url to connect to this database.
	 */
	abstract protected String getConnectionUrl();
	
	/**
	 * @param connection
	 */
	public void closeConnection( Connection connection )
	{
		try
		{
			connection.close();
		}
		catch ( SQLException e )
		{
			Trace.report( this, e );
		}
	}
}
