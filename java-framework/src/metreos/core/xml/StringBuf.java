/* $Id: StringBuf.java,v 1.5 2005/09/16 22:20:50 wert Exp $
 *
 * Created by wert on Apr 27, 2005.
 *
 * Copyright (c) 2005 Metreos, Inc. All rights reserved.
 */

package metreos.core.xml;

/**
 * A simple interface to a string buffer.
 */
public interface StringBuf
{
	/**
	 * Append a character to the string buffer.
	 * @param c the character to append.
	 */
	public void append( char c );
	
	/**
	 * Append a string to the string buffer.
	 * @param s the string of chars to append.
	 */
	public void append( String s );

	/**
	 * @return the number of characters in the string buffer.
	 */
	public int length();

	/**
	 * @return the current contents as a string.
	 */
	public String toString();

	/**
	 * Reset the string buffer to having no content.
	 */
	public void clear();

	/**
	 * @return a description of the string buffer.
	 */
	public String getDescr();
}
