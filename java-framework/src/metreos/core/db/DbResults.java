/* $Id: DbResults.java 6937 2005-05-31 16:26:56Z wert $
 *
 * Created by wert on May 18, 2005.
 *
 * Copyright (c) 2005 Metreos, Inc. All rights reserved.
 */

package metreos.core.db;

import java.util.HashMap;
import java.util.List;
import java.util.Map;
import java.util.Vector;

/**
 * Description of DbResults.
 */
public class DbResults
{
	/**
	 * Constructs the DbResults.
	 *
	 * @param names
	 */
	public DbResults( String[] names )
	{
		this.names = names;
		for (int i = 0; i < names.length; i++)
			columns.put( names[i], new Integer( i+1 ) );
	}
	
	private String[] names;
	
	private Map<String,Integer> columns = new HashMap<String,Integer>();
	
	/**
	 * @return the number of columns in this set.
	 */
	public int countColumns()
	{
		return names.length;
	}
	
	/**
	 * @return the number of rows in this set.
	 */
	public int countRows()
	{
		return rows.size();
	}

	/**
	 * @param values
	 */
	public void addRow( Object[] values )
	{
		rows.add( values );
	}
	
	private List<Object[]> rows = new Vector<Object[]>();
	
	/**
	 * @return true if we are on a row, false otherwise.
	 */
	public boolean onARow()
	{
		if (rowIndex >= 0 && rowIndex < rows.size())
		{
			currentRow = rows.get( rowIndex );
			return true;
		}
		currentRow = null;
		return false;
	}
	
	private int rowIndex = -1;
	
	private Object[] currentRow;
	
	/**
	 * Moves the cursor to its starting place before the first row.
	 */
	public void rewind()
	{
		rowIndex = -1;
	}
	
	/**
	 * Moves the cursor to the first row.
	 * @return true if we are on a row.
	 */
	public boolean first()
	{
		rowIndex = 0;
		return onARow();
	}
	
	/**
	 * Moves the cursor to the last row.
	 * @return true if we are on a row.
	 */
	public boolean last()
	{
		rowIndex = rows.size()-1;
		return onARow();
	}
	
	/**
	 * Moves the cursor to the previous row.
	 * @return true if we are on a row.
	 */
	public boolean prev()
	{
		if (rowIndex >= 0)
			rowIndex--;
		return onARow();
	}
	/**
	 * Moves the cursor to the next row.
	 * @return true if we are on a row.
	 */
	public boolean next()
	{
		if (rowIndex < rows.size())
			rowIndex++;
		return onARow();
	}
	
	/**
	 * @param index a column id (1..countColumns).
	 * @return the name of the specified column.
	 */
	public String getColumnName( int index )
	{
		return names[index-1];
	}
	
	/**
	 * @param name
	 * @return the column id of the column (1..countColumns).
	 * @throws DbException
	 */
	public int getColumnId( String name ) throws DbException
	{
		Integer i = columns.get( name );
		if (i == null)
			throw new DbException( "no such column: "+name );
		return i.intValue();
	}
	
	/**
	 * @param name
	 * @return the value of the specified column
	 * @throws DbException
	 */
	public String getString( String name ) throws DbException
	{
		return (String) getObject( name );
	}
	
	/**
	 * @param index
	 * @return the value of the specified column
	 * @throws DbException 
	 */
	public String getString( int index ) throws DbException
	{
		return (String) getObject( index );
	}
	
	/**
	 * @param name
	 * @return the value of the specified column
	 * @throws DbException
	 */
	public Integer getInteger( String name ) throws DbException
	{
		return (Integer) getObject( name );
	}
	
	/**
	 * @param index
	 * @return the value of the specified column
	 * @throws DbException 
	 */
	public Integer getInteger( int index ) throws DbException
	{
		return (Integer) getObject( index );
	}

	/**
	 * @param name
	 * @return the object at the specified index.
	 * @throws DbException 
	 */
	public Object getObject( String name ) throws DbException
	{
		return getObject( getColumnId( name ) );
	}

	/**
	 * @param index
	 * @return the object at the specified index.
	 * @throws DbException 
	 */
	public Object getObject( int index ) throws DbException
	{
		if (currentRow == null)
			throw new DbException( "not on a row" );
		if (index < 1 || index > currentRow.length)
			throw new DbException( "bad column id: "+index );
		return currentRow[index-1];
	}
}
