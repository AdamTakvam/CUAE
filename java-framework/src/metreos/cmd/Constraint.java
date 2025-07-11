/* $Id: Constraint.java 6338 2005-04-19 20:19:53Z wert $
 *
 * Created by wert on Mar 27, 2005.
 * 
 * Copyright (c) 2005 Metreos, Inc. All rights reserved.
 */

package metreos.cmd;

/**
 * Constraint allows situational conditions to be placed upon
 * values otherwise acceptable given an item's type. Thus, you
 * can have an item whose value is >= 0, or whose value is a
 * readable file, etc.
 */
public interface Constraint
{
	/**
	 * @param value
	 * @throws Exception with a message if the value is bad.
	 */
	public void checkValue( Object value ) throws Exception;
}
