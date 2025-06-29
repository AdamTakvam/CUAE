/* $Id: Codec.java 7113 2005-06-10 16:00:16Z wert $
 * 
 * Created by achaney on Jan 28, 2005
 *
 * Copyright (c) 2005 Metreos, Inc. All rights reserved.
 */
package metreos.service.jtapi;

/**
 * Defines the set of allowable media codecs.
 */
public interface Codec 
{
	/**
	 * An unspecified codec.
	 */
	public final static int Unspecified = 0;
	
	/**
	 * A G711u codec.
	 */
	public final static int G711u = 1;
	
	/**
	 * A G711a codec.
	 */
	public final static int G711a = 2;
	
	/**
	 * A G723 codec.
	 */
	public final static int G723 = 3;
	
	/**
	 * A G729 codec.
	 */
	public final static int G729 = 4;
}
