/* $Id: CharIterator.java 7054 2005-06-07 17:48:33Z wert $
 *
 * Created by wert on Feb 16, 2005
 *
 * Copyright (c) 2005 Metreos, Inc. All rights reserved.
 */

package metreos.util;

/**
 * @author wert
 */
public class CharIterator
{
	/**
	 * @param s
	 */
	public CharIterator( String s )
	{
		this.s = s;
		this.n = s.length();
	}
	
	private final String s;
	
	private final int n;
	
	private int i;

	/**
	 * @return true if there are more chars.
	 */
	public boolean hasNext()
	{
		return i < n;
	}

	/**
	 * @return the next character in sequence.
	 */
	public char next()
	{
		return s.charAt( i++ );
	}
}
