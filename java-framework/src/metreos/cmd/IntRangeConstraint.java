/* $Id: IntRangeConstraint.java 30151 2007-03-06 21:47:46Z wert $
 *
 * Created by wert on Apr 29, 2005.
 * 
 * Copyright (c) 2005 Metreos, Inc. All rights reserved.
 */

package metreos.cmd;

/**
 * Description of IntRangeConstraint
 */
public class IntRangeConstraint implements Constraint
{
	/**
	 * Constructs the IntRangeConstraint.
	 *
	 * @param min the minimum value allowed.
	 * 
	 * @param max the maximum value allowed.
	 */
	public IntRangeConstraint( int min, int max )
	{
		if (min > max)
			throw new IllegalArgumentException( "min > max" );
		this.min = min;
		this.max = max;
	}
	
	private final int min;
	
	private final int max;
	
	/* (non-Javadoc)
	 * @see metreos.cmd.Constraint#checkValue(java.lang.Object)
	 */
	public void checkValue( Object value ) throws Exception
	{
		int v = ((Integer) value).intValue();
		if (v < min || v > max)
			throw new Exception( "value not in range "+min+" to "+max );
	}

	@Override
	public String toString()
	{
		return "value must be in range "+min+" to "+max;
	}
}
