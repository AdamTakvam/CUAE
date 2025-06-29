/* $Id: StringArrayIterator.java 6937 2005-05-31 16:26:56Z wert $
 *
 * Created by wert on Mar 27, 2005.
 * 
 * Copyright (c) 2005 Metreos, Inc. All rights reserved.
 */

package metreos.cmd;

import java.util.Iterator;
import java.util.NoSuchElementException;

/**
 * Simply an Iterator for String[].
 */
public class StringArrayIterator implements Iterator<String>
{
	/**
	 * @param array
	 */
	public StringArrayIterator( String[] array )
	{
		this.array = array;
	}
	
	private final String[] array;
	
	private int index;
	
	/* (non-Javadoc)
	 * @see java.util.Iterator#hasNext()
	 */
	public boolean hasNext()
	{
		return index < array.length;
	}

	/* (non-Javadoc)
	 * @see java.util.Iterator#next()
	 */
	public String next()
	{
		if (index >= array.length)
			throw new NoSuchElementException();
		
		return array[index++];
	}

	/* (non-Javadoc)
	 * @see java.util.Iterator#remove()
	 */
	public void remove()
	{
		throw new UnsupportedOperationException();
	}
}
