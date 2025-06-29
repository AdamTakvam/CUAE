/* $Id: CollectionUtils.java 6939 2005-05-31 17:50:50Z wert $
 *
 * Created by wert on May 12, 2005.
 *
 * Copyright (c) 2005 Metreos, Inc. All rights reserved.
 */

package metreos.core.net.nio;

import java.util.Collection;
import java.util.Iterator;

/**
 * Description of CollectionUtils.
 */
public class CollectionUtils
{
	/**
	 * @param c
	 * @param f
	 */
	@SuppressWarnings(value={"unchecked"})
	public static void syncIterate( Collection c, IteratorFunc f )
	{
		synchronized (c)
		{
			Iterator i = c.iterator();
			while (i.hasNext())
				if (f.next( i, i.next() ))
					break;
			f.done( c );
		}
	}
	
	/**
	 * @param c
	 * @param f
	 * @return the object searched for, or null if not found.
	 */
	@SuppressWarnings(value={"unchecked"})
	public static Object syncSearch( Collection c, SearchFunc f )
	{
		synchronized (c)
		{
			Iterator i = c.iterator();
			while (i.hasNext())
			{
				Object o = f.next( i, i.next() );
				if (o != null)
					return o;
			}
			return null;
		}
	}
	
	/**
	 * Description of IteratorFunc.
	 */
	public interface IteratorFunc
	{
		/**
		 * @param i 
		 * @param o
		 * @return true if the iteration should stop.
		 */
		public boolean next( Iterator i, Object o );

		/**
		 * Reports that the iteration is done.
		 * @param c
		 */
		public void done( Collection c );
	}
	
	/**
	 * Description of IteratorFunc.
	 */
	public interface SearchFunc
	{
		/**
		 * @param i 
		 * @param o
		 * @return the object we are searching for, or null if not found yet.
		 */
		public Object next( Iterator i, Object o );
	}
}