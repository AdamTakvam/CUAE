/* $Id: EmptyIterator.java 7054 2005-06-07 17:48:33Z wert $
 *
 * Created by wert on Feb 16, 2005
 *
 * Copyright (c) 2005 Metreos, Inc. All rights reserved.
 */

package metreos.util;

import java.util.Iterator;
import java.util.NoSuchElementException;

/**
 * @param <E> 
 * @author wert
 */
public class EmptyIterator<E> implements Iterator<E>
{
	/* (non-Javadoc)
	 * @see java.util.Iterator#hasNext()
	 */
	public boolean hasNext()
	{
		return false;
	}

	/* (non-Javadoc)
	 * @see java.util.Iterator#next()
	 */
	public E next()
	{
		throw new NoSuchElementException();
	}

	/* (non-Javadoc)
	 * @see java.util.Iterator#remove()
	 */
	public void remove()
	{
		throw new IllegalStateException();
	}

	/**
	 * An empty iterator to use when you need it.
	 */
	public final static Iterator EMPTY_ITERATOR = new EmptyIterator();
}
