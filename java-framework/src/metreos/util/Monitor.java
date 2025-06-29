/* $Id: Monitor.java 30151 2007-03-06 21:47:46Z wert $
 *
 * Created by wert on Feb 14, 2005
 *
 * Copyright (c) 2005 Metreos, Inc. All rights reserved.
 */

package metreos.util;

/**
 * A class which we can use to monitor conditions.
 */
public class Monitor
{
	/**
	 * @param description
	 * @param initialValue
	 */
	public Monitor( String description, Object initialValue )
	{
		this.description = description;
		this.value = initialValue;
	}
	
	/**
	 * @param description
	 */
	public Monitor( String description )
	{
		this( description, null );
	}
	
	/**
	 * @return the description of this monitor.
	 */
	
	public String getDescription()
	{
		return description;
	}
	
	private final String description;
	
	private Object value;

	@Override
	public String toString()
	{
		return "Monitor "+description+": "+value;
	}
	
	/**
	 * @return the current value of the monitor.
	 */
	public Object get()
	{
		return value;
	}
	
	/**
	 * Sets the current value.
	 * @param newValue
	 * @return the old value.
	 */
	public synchronized Object set( Object newValue )
	{
		Object oldValue = value;
		value = newValue;
		notifyAll();
		return oldValue;
	}

	/**
	 * Waits until the monitor value is set.
	 * 
	 * @param maxDelay the max amount of time in ms to wait.
	 * If 0 is specified, we will wait forever.
	 * 
	 * @return the current value of the monitor.
	 * 
	 * @throws InterruptedException if we waited too long.
	 */
	private synchronized Object waitUntilSet( long maxDelay )
		throws InterruptedException
	{
		wait( maxDelay );
		return value;
	}

	/**
	 * @param desiredValue
	 * @param newValue
	 * @return the old value
	 * @throws InterruptedException
	 */
	public Object waitUntilEqAndSet( Object desiredValue,
		Object newValue ) throws InterruptedException
	{
		return waitUntilEqAndSet( desiredValue, 0, newValue );
	}

	/**
	 * @param desiredValue
	 * @param maxDelay
	 * @param newValue
	 * @return the old value
	 * @throws InterruptedException
	 */
	public synchronized Object waitUntilEqAndSet( Object desiredValue,
		long maxDelay, Object newValue ) throws InterruptedException
	{
		waitUntilEq( desiredValue, maxDelay );
		return set( newValue );
	}

	/**
	 * Waits forever until the value is set to the specified value.
	 * 
	 * @param desiredValue the value we are waiting for.
	 * 
	 * @throws InterruptedException
	 */
	public void waitUntilEq( Object desiredValue ) throws InterruptedException
	{
		waitUntilEq( desiredValue, 0 );
	}


	/**
	 * Waits until the value is set to the specified value.
	 * 
	 * @param desiredValue the value we are waiting for.
	 * 
	 * @param maxDelay the max amount of time in ms to wait.
	 * If 0 is specified, we will wait forever.
	 * 
	 * @throws InterruptedException if we waited too long.
	 */
	public synchronized void waitUntilEq( Object desiredValue, long maxDelay )
		throws InterruptedException
	{
		long now = System.currentTimeMillis();
		long endTime = maxDelay > 0 ?
			now + maxDelay : Long.MAX_VALUE;
		
		while (!eq( value, desiredValue ) && now < endTime)
		{
			waitUntilSet( endTime - now );
			now = System.currentTimeMillis();
		}
		
		if (!eq( value, desiredValue ))
			throw new InterruptedException( "timeout" );
	}

	/**
	 * @param undesiredValue
	 * @param newValue
	 * @return the old value
	 * @throws InterruptedException
	 */
	public Object waitUntilNotEqAndSet( Object undesiredValue, Object newValue )
		throws InterruptedException
	{
		return waitUntilNotEqAndSet( undesiredValue, 0, newValue );
	}

	/**
	 * @param undesiredValue
	 * @param maxDelay
	 * @param newValue
	 * @return the old value
	 * @throws InterruptedException
	 */
	public synchronized Object waitUntilNotEqAndSet( Object undesiredValue,
		long maxDelay, Object newValue ) throws InterruptedException
	{
		waitUntilNotEq( undesiredValue );
		return set( newValue );
	}
	
	/**
	 * Waits forever until the value is not the specified value.
	 * 
	 * @param undesiredValue the value we do not want.
	 * 
	 * @return the current value of the monitor.
	 * 
	 * @throws InterruptedException if we waited too long.
	 */
	public Object waitUntilNotEq( Object undesiredValue )
		throws InterruptedException
	{
		return waitUntilNotEq( undesiredValue, 0 );
	}
	
	/**
	 * Waits until the value is not the specified value.
	 * 
	 * @param undesiredValue the value we do not want.
	 * 
	 * @param maxDelay the max amount of time in ms to wait.
	 * If 0 is specified, we will wait forever.
	 * 
	 * @return the current value of the monitor.
	 * 
	 * @throws InterruptedException if we waited too long.
	 */
	public synchronized Object waitUntilNotEq( Object undesiredValue, long maxDelay )
		throws InterruptedException
	{
		long now = System.currentTimeMillis();
		long endTime = maxDelay > 0 ?
			now + maxDelay : Long.MAX_VALUE;
		
		while (eq( value, undesiredValue ) && now < endTime)
		{
			waitUntilSet( endTime - now );
			now = System.currentTimeMillis();
		}
		
		if (eq( value, undesiredValue ))
			throw new InterruptedException( "timeout" );
		
		return value;
	}

	/**
	 * Compares the specified values.
     * 
	 * @param v1 a value to compare, which may be null.
	 * 
	 * @param v2 another value to compare, which may be null.
	 * 
	 * @return true if the values are equal, false otherwise. If both
	 * values are null, they are considered equal.
	 */
	private boolean eq( Object v1, Object v2 )
	{
		if (v1 != null && v2 != null)
			return v1.equals( v2 );
		
		return v1 == v2;
	}
}
















