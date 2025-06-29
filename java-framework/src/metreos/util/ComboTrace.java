/* $Id: ComboTrace.java 30151 2007-03-06 21:47:46Z wert $
 *
 * Created by wert on Apr 12, 2005.
 *
 * Copyright (c) 2005 Metreos, Inc. All rights reserved.
 */

package metreos.util;


/**
 * Description of ComboTrace.
 */
public class ComboTrace extends Trace
{
	/**
	 * Constructs the ComboTrace, which writes trace reports to both
	 * of its trace objects.
	 *
	 * @param a
	 * @param b
	 */
	public ComboTrace( Trace a, Trace b )
	{
		this.a = a;
		this.b = b;
	}
	
	private final Trace a;
	
	private final Trace b;

	@Override
	public Trace start()
	{
		a.start();
		b.start();
		return this;
	}

	@Override
	protected void rprt( long when, int eventMask, Object who, M msg, Throwable t )
	{
		a.rprt( when, eventMask, who, msg, t );
		b.rprt( when, eventMask, who, msg, t );
	}

	@Override
	public void stop()
	{
		a.stop();
		b.stop();
	}
}
