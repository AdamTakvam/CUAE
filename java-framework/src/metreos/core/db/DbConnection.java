/* $Id: DbConnection.java 7054 2005-06-07 17:48:33Z wert $
 *
 * Created by wert on May 17, 2005.
 *
 * Copyright (c) 2005 Metreos, Inc. All rights reserved.
 */

package metreos.core.db;

import java.sql.Connection;
import java.sql.ResultSet;
import java.sql.ResultSetMetaData;
import java.sql.SQLException;
import java.sql.Statement;
import java.util.HashMap;
import java.util.Map;

import metreos.util.Trace;


/**
 * Description of DbConnection.
 */
public class DbConnection
{
	/**
	 * Constructs the DbConnection.
	 *
	 * @param database
	 */
	public DbConnection( Database database )
	{
		this.database = database;
	}
	
	private final Database database;
	
	/**
	 * @param query
	 * @return a map with the name / value pairs in it.
	 * @throws DbException 
	 */
	public Map<String,Object> queryNameValue( String query ) throws DbException
	{
		return queryNameValue( query, DEFAULT_MAX_DELAY );
	}
	
	/**
	 * @param query
	 * @param maxDelay
	 * @return a map with the name / value pairs in it.
	 * @throws DbException 
	 */
	public Map<String,Object> queryNameValue( String query, int maxDelay )
		throws DbException
	{
		Map<String,Object> map = new HashMap<String,Object>();
		DbResults results = getResults( query, maxDelay );
		while (results.next())
		{
			String name = results.getString( 1 );
			Object value = results.getObject( 2 );
			map.put( name, value );
		}
		return map;
	}

	/**
	 * @param query
	 * @return DbResults from ResultSet from query
	 * @throws DbException
	 */
	public DbResults getResults( String query ) throws DbException
	{
		return getResults( query, DEFAULT_MAX_DELAY );
	}

	/**
	 * @param query
	 * @param maxDelay
	 * @return DbResults from ResultSet from query
	 * @throws DbException
	 */
	public DbResults getResults( String query, int maxDelay )
		throws DbException
	{
		Connection connection = database.openConnection( maxDelay );
		Trace.report( "got connection" );
		try
		{
			Statement stmt = connection.createStatement();
			Trace.report( "got statement" );
			try
			{
				ResultSet rs = stmt.executeQuery( query );
				Trace.report( "got result set" );
				try
				{
					return newDbResults( rs );
				}
				catch ( SQLException e )
				{
					throw new DbException( "could not get results", e );
				}
				finally
				{
					closeResultSet( rs );
				}
			}
			catch ( SQLException e )
			{
				throw new DbException( "could not execute query", e );
			}
			finally
			{
				closeStatement( stmt );
			}
		}
		catch ( SQLException e )
		{
			throw new DbException( "could not create statement", e );
		}
		finally
		{
			database.closeConnection( connection );
		}
	}

	/**
	 * @param stmt
	 */
	private void closeStatement( Statement stmt )
	{
		try
		{
			stmt.close();
		}
		catch ( SQLException e )
		{
			Trace.report( this, e );
		}
	}

	/**
	 * @param rs
	 */
	private void closeResultSet( ResultSet rs )
	{
		try
		{
			rs.close();
		}
		catch ( SQLException e )
		{
			Trace.report( this, e );
		}
	}

	/**
	 * @param rs ResultSet from query
	 * @return DbResults from ResultSet
	 * @throws SQLException 
	 */
	private DbResults newDbResults( ResultSet rs ) throws SQLException
	{
		ResultSetMetaData rsmd = rs.getMetaData();
		int numColumns = rsmd.getColumnCount();
		
		Trace.report( "numColumns = "+numColumns );
		
		String[] names = new String[numColumns];
		for (int i = 0; i < numColumns; i++)
			names[i] = rsmd.getColumnName( i+1 );
		
		DbResults results = new DbResults( names );
		
		while (rs.next())
		{
			Object[] values = new Object[numColumns];
			for (int i = 0; i < numColumns; i++)
				values[i] = rs.getObject( i+1 );
			results.addRow( values );
		}
		
		return results;
	}
	
	/**
	 * Amount of time in ms to wait for a query result.
	 */
	public int DEFAULT_MAX_DELAY = 60*1000;
}
