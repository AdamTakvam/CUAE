/* $Id: SingleIterator.java 7054 2005-06-07 17:48:33Z wert $
 *
 * Created by wert on Feb 16, 2005
 *
 * Copyright (c) 2005 Metreos, Inc. All rights reserved.
 */

package metreos.util;

import java.util.Iterator;
import java.util.NoSuchElementException;

/**
 * An iterator over a single object.
 * @param <E> 
 */
public class SingleIterator<E> implements Iterator<E>
{
	/**
	 * Constructs a SingleIterator.
	 *
	 * @param obj the object to be iterated.
	 */
	public SingleIterator( E obj )
	{
		this.obj = obj;
	}
	
	private E obj;
	
	/* (non-Javadoc)
	 * @see java.util.Iterator#hasNext()
	 */
	public boolean hasNext()
	{
		return obj != null;
	}

	/* (non-Javadoc)
	 * @see java.util.Iterator#next()
	 */
	public E next()
	{
		if (obj == null)
			throw new NoSuchElementException();
		
		E o = obj;
		obj = null;
		return o;
	}

	/* (non-Javadoc)
	 * @see java.util.Iterator#remove()
	 */
	public void remove()
	{
		throw new UnsupportedOperationException();
	}
}
