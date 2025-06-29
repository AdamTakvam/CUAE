/* $Id: ParityConstraint.java 30151 2007-03-06 21:47:46Z wert $
 *
 * Created by wert on May 12, 2005.
 *
 * Copyright (c) 2005 Metreos, Inc. All rights reserved.
 */

package metreos.cmd;

/**
 * Description of ParityConstraint.
 */
public class ParityConstraint implements Constraint
{
	/**
	 * Constructs the ParityConstraint.
	 *
	 * @param wantsOdd
	 */
	public ParityConstraint( boolean wantsOdd )
	{
		this.wantsOdd = wantsOdd;
	}
	
	private final boolean wantsOdd;
	
	public void checkValue( Object value ) throws Exception
	{
		int v = ((Integer) value).intValue();
		boolean valueIsOdd = ((v & 1) != 0);
		if (valueIsOdd != wantsOdd)
			throw new Exception( wantsOdd ? "is not odd" : "is not even" );
	}

	@Override
	public String toString()
	{
		return wantsOdd ? "must be odd" : "must be even";
	}
}
