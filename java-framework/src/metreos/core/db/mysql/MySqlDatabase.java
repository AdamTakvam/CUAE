/* $Id: MySqlDatabase.java 30151 2007-03-06 21:47:46Z wert $
 *
 * Created by wert on May 17, 2005.
 *
 * Copyright (c) 2005 Metreos, Inc. All rights reserved.
 */

package metreos.core.db.mysql;

import java.util.HashMap;
import java.util.Iterator;
import java.util.Map;

import metreos.core.db.Database;
import metreos.core.db.DbConnection;
import metreos.util.URL;

/**
 * Description of MySqlDatabase.
 */
public class MySqlDatabase extends Database
{
	/**
	 * Constructs the MySqlDatabase.
	 *
	 * @param driverName
	 * @param serverName 
	 * @param port 
	 * @param databaseName 
	 * @param others 
	 * @throws ClassNotFoundException 
	 */
	public MySqlDatabase( String driverName, String serverName,
		Integer port, String databaseName, Map<String,String> others )
		throws ClassNotFoundException
	{
		super( driverName );
		this.serverName = serverName;
		this.port = port;
		this.databaseName = databaseName;
		this.others = others;
	}

	/**
	 * Constructs the MySqlDatabase.
	 * @param serverName 
	 * @param port 
	 * @param databaseName 
	 * @param others 
	 * @throws ClassNotFoundException 
	 */
	public MySqlDatabase( String serverName, Integer port,
		String databaseName, Map<String,String> others ) throws ClassNotFoundException
	{
		this( DEFAULT_DRIVER_NAME, serverName, port, databaseName, others );
	}
	
	/**
	 * Constructs the MySqlDatabase.
	 *
	 * @param url
	 * @throws ClassNotFoundException 
	 */
	public MySqlDatabase( URL url ) throws ClassNotFoundException
	{
		this( url.getTerm( DRIVER_NAME, DEFAULT_DRIVER_NAME ),
			url.getTerm( SERVER_NAME ), url.getIntegerTerm( PORT ),
			url.getTerm( DATABASE_NAME ), getOthers( url ) );
	}
	
	private final String serverName;
	
	private final Integer port;
	
	private final String databaseName;

	private final Map<String,String> others;
	
	private final static String DEFAULT_DRIVER_NAME = "com.mysql.jdbc.Driver";

	private final static String DRIVER_NAME = "driverName";

	private final static String SERVER_NAME = "serverName";

	private final static String PORT = "port";

	private final static String DATABASE_NAME = "databaseName";
	
	private static Map<String,String> getOthers( URL url )
	{
		Map<String,String> others = null;
		Iterator<String> i = url.getTermNames();
		while (i.hasNext())
		{
			String name = i.next();
			if (name.equals( DRIVER_NAME )) continue;
			if (name.equals( SERVER_NAME )) continue;
			if (name.equals( PORT )) continue;
			if (name.equals( DATABASE_NAME )) continue;
			String value = url.getTerm( name );
			if (others == null)
				others = new HashMap<String,String>();
			others.put( name, value );
		}
		return others;
	}

	@Override
	protected DbConnection newDbConnection( Database database )
	{
		return new DbConnection( this );
	}

	@Override
	protected String getConnectionUrl()
	{
		// jdbc:mysql://[host:port],[host:port].../[database][?propertyName1][=propertyValue1][&propertyName2][=propertyValue2]
		
		StringBuffer sb = new StringBuffer();
		sb.append( "jdbc:mysql://" );
		
		if (serverName != null)
		{
			sb.append( serverName );
			if (port != null)
			{
				sb.append( ':' );
				sb.append( port );
			}
		}
		
		sb.append( '/' );
		
		if (databaseName != null)
			sb.append( databaseName );
		
		boolean first = true;
		Iterator<Map.Entry<String,String>> i = others.entrySet().iterator();
		while (i.hasNext())
		{
			Map.Entry<String,String> me = i.next();
			String name = me.getKey();
			String value = me.getValue();
			
			if (first)
			{
				sb.append( '?' );
				first = false;
			}
			else
			{
				sb.append( '&' );
			}
			
			sb.append( name );
			sb.append( '=' );
			sb.append( value );
		}
		
		return sb.toString();
	}
}
