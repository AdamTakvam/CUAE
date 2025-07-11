/* $Id: OrConstraint.java 30151 2007-03-06 21:47:46Z wert $
 *
 * Created by wert on May 12, 2005.
 *
 * Copyright (c) 2005 Metreos, Inc. All rights reserved.
 */

package metreos.cmd;

/**
 * Description of OrConstraint.
 */
public class OrConstraint implements Constraint
{
	/**
	 * Constructs the OrConstraint.
	 * @param a 
	 * @param b 
	 */
	public OrConstraint( Constraint a, Constraint b )
	{
		this.a = a;
		this.b = b;
	}
	
	private final Constraint a;
	
	private final Constraint b;
	
	public void checkValue( Object value ) throws Exception
	{
		try
		{
			a.checkValue( value );
		}
		catch ( Exception ea )
		{
			try
			{
				b.checkValue( value );
			}
			catch ( Exception eb )
			{
				throw new Exception( "failed both constraints: "+
					ea.getMessage()+" AND "+eb.getMessage() );
			}
		}
	}

	@Override
	public String toString()
	{
		return "("+a+") OR ("+b+")";
	}
}
